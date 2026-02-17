// Decompiled with JetBrains decompiler
// Type: EnemyBat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyBat : UnitObject
{
  [SerializeField]
  public SpriteRenderer Aiming;
  public static List<EnemyBat> enemyBats = new List<EnemyBat>();
  public DeadBodyFlying DeadBody;
  public ColliderEvents damageColliderEvents;
  public SimpleSpineFlash SimpleSpineFlash;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  public SkeletonAnimation warningIcon;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  public Vector3? StartingPosition;
  public Vector3 TargetPosition;
  public bool canBeStunned = true;
  public int RanDirection = 1;
  public float MaximumRange = 5f;
  public float IdleSpeed = 0.03f;
  public float ChaseSpeed = 0.1f;
  public float turningSpeed = 1f;
  public float angleNoiseAmplitude;
  public float angleNoiseFrequency;
  public float timestamp;
  [SerializeField]
  public bool useAcceleration;
  [SerializeField]
  public float acceleration = 2f;
  public float noticePlayerDistance = 5f;
  public float attackPlayerDistance = 3f;
  public bool ChasingPlayer;
  public Vector2 AttackCoolDownDuration = new Vector2(1f, 2f);
  public float AttackCoolDown;
  public float DistanceCheck;
  public bool NoticedPlayer;
  public bool avoidTarget;
  [SerializeField]
  public float postAttackDelay = 0.2f;
  public bool Attacking;
  public float AttackForce = 2500f;
  public int CurrentAttackNum;
  public int NumAttacks = 1;
  public System.Action<int> OnAttack;
  public System.Action OnAttackComplete;
  public float Angle;
  public Vector3 Force;
  public float KnockbackForceModifier = 1f;
  public float KnockbackDuration = 0.5f;
  public bool AttackAfterKnockback;
  public Health EnemyHealth;

  public void Start()
  {
    if (!((UnityEngine.Object) this.Aiming != (UnityEngine.Object) null))
      return;
    this.Aiming.gameObject.SetActive(false);
  }

  public override void OnEnable()
  {
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnEnable();
    EnemyBat.enemyBats.Add(this);
    if (!this.StartingPosition.HasValue)
    {
      this.StartingPosition = new Vector3?(this.transform.position);
      this.TargetPosition = this.StartingPosition.Value;
      this.maxSpeed = this.IdleSpeed;
    }
    this.timestamp = !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null) ? Time.time : GameManager.GetInstance().CurrentTime;
    this.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseAmplitude += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.state.CURRENT_STATE = StateMachine.State.Moving;
    this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.RanDirection = (double) UnityEngine.Random.value < 0.5 ? -1 : 1;
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "wingFlap"))
      return;
    AudioManager.Instance.PlayOneShot("event:/enemy/small_wings_flap", this.gameObject);
  }

  public override void Update()
  {
    if (this.useAcceleration)
    {
      if (this.UsePathing)
      {
        if (this.pathToFollow == null)
        {
          this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime * this.Spine.timeScale;
          this.move();
          return;
        }
        if (this.currentWaypoint >= this.pathToFollow.Count)
        {
          this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime * this.Spine.timeScale;
          this.move();
          return;
        }
      }
      if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
      {
        this.speed += (float) (((double) this.maxSpeed * (double) this.SpeedMultiplier - (double) this.speed) / (7.0 * (double) this.acceleration)) * GameManager.DeltaTime * this.Spine.timeScale;
        if (this.UsePathing)
        {
          if ((double) this.Spine.timeScale != 9.9999997473787516E-05)
            this.state.facingAngle = Mathf.LerpAngle(this.state.facingAngle, Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]), Time.deltaTime * this.turningSpeed);
          if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.pathToFollow[this.currentWaypoint]) <= (double) this.StoppingDistance)
          {
            ++this.currentWaypoint;
            if (this.currentWaypoint == this.pathToFollow.Count)
            {
              this.state.CURRENT_STATE = StateMachine.State.Idle;
              System.Action endOfPath = this.EndOfPath;
              if (endOfPath != null)
                endOfPath();
              this.pathToFollow = (List<Vector3>) null;
            }
          }
        }
      }
      else
        this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime * this.Spine.timeScale;
      this.move();
    }
    else
      base.Update();
  }

  public override void FixedUpdate()
  {
    if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null) || (double) this.Spine.timeScale == 9.9999997473787516E-05)
      return;
    base.FixedUpdate();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!this.ChasingPlayer)
    {
      if (!this.NoticedPlayer)
      {
        if (!string.IsNullOrEmpty(this.WarningVO))
          AudioManager.Instance.PlayOneShot(this.WarningVO, this.gameObject);
        if ((bool) (UnityEngine.Object) this.warningIcon)
        {
          this.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
          this.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, 2f);
        }
        this.NoticedPlayer = true;
      }
      this.maxSpeed = this.ChaseSpeed;
      this.ChasingPlayer = true;
    }
    if (this.Attacking && this.canBeStunned)
    {
      this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    else if (this.damageColliderEvents.gameObject.activeSelf)
    {
      this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && (double) this.KnockbackForceModifier != 0.0)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    if (!(bool) (UnityEngine.Object) this.SimpleSpineFlash)
      return;
    this.SimpleSpineFlash.FlashFillRed();
  }

  public IEnumerator HurtRoutine()
  {
    EnemyBat enemyBat = this;
    if (!string.IsNullOrEmpty(enemyBat.GetHitVO))
      AudioManager.Instance.PlayOneShot(enemyBat.GetHitVO, enemyBat.gameObject);
    enemyBat.damageColliderEvents.SetActive(false);
    enemyBat.Attacking = false;
    enemyBat.ClearPaths();
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBat.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyBat.DisableForces = false;
    enemyBat.StartCoroutine((IEnumerator) enemyBat.ActiveRoutine());
  }

  public virtual IEnumerator ActiveRoutine()
  {
    EnemyBat enemyBat = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      float turningSpeed = enemyBat.turningSpeed;
      if (!enemyBat.ChasingPlayer)
      {
        enemyBat.state.LookAngle = enemyBat.state.facingAngle;
        if (GameManager.RoomActive && (UnityEngine.Object) enemyBat.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(enemyBat.transform.position, enemyBat.GetClosestTarget().transform.position) < (double) enemyBat.noticePlayerDistance)
        {
          if (!enemyBat.NoticedPlayer)
          {
            if (!string.IsNullOrEmpty(enemyBat.WarningVO))
              AudioManager.Instance.PlayOneShot(enemyBat.WarningVO, enemyBat.gameObject);
            if ((UnityEngine.Object) enemyBat.warningIcon != (UnityEngine.Object) null)
            {
              enemyBat.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
              enemyBat.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, 2f);
            }
            enemyBat.NoticedPlayer = true;
          }
          enemyBat.maxSpeed = enemyBat.ChaseSpeed;
          enemyBat.ChasingPlayer = true;
        }
      }
      else
      {
        float num1;
        if ((UnityEngine.Object) enemyBat.GetClosestTarget() == (UnityEngine.Object) null || (double) Vector3.Distance(enemyBat.transform.position, enemyBat.GetClosestTarget().transform.position) > 12.0)
        {
          enemyBat.TargetPosition = enemyBat.StartingPosition.Value;
          enemyBat.maxSpeed = enemyBat.IdleSpeed;
          enemyBat.ChasingPlayer = false;
        }
        else
        {
          if (enemyBat.avoidTarget)
          {
            enemyBat.TargetPosition = -enemyBat.GetClosestTarget().transform.position;
            int num2 = 0;
            while (num2 < 10 && (double) Vector3.Magnitude(enemyBat.TargetPosition - enemyBat.transform.position) < 3.0)
            {
              num1 = Vector3.Magnitude(enemyBat.TargetPosition - enemyBat.transform.position);
              Debug.Log((object) $"Dist {num1.ToString()} {num2.ToString()}");
              ++num2;
              enemyBat.TargetPosition *= 3f;
            }
          }
          else
            enemyBat.TargetPosition = enemyBat.GetClosestTarget().transform.position;
          enemyBat.state.LookAngle = Utils.GetAngle(enemyBat.transform.position, enemyBat.GetClosestTarget().transform.position);
        }
        if (enemyBat.avoidTarget)
        {
          enemyBat.StartCoroutine((IEnumerator) enemyBat.FleeRoutine());
        }
        else
        {
          enemyBat.AttackCoolDown = num1 = enemyBat.AttackCoolDown - Time.deltaTime;
          if ((double) num1 < 0.0)
          {
            if (!enemyBat.ShouldStartCharging())
            {
              if (enemyBat.ShouldAttack())
                goto label_24;
            }
            else
              break;
          }
        }
      }
      enemyBat.Angle = Mathf.LerpAngle(enemyBat.Angle, Utils.GetAngle(enemyBat.transform.position, enemyBat.TargetPosition), Time.deltaTime * turningSpeed);
      if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null && (double) enemyBat.angleNoiseAmplitude > 0.0 && (double) enemyBat.angleNoiseFrequency > 0.0 && (double) Vector3.Distance(enemyBat.TargetPosition, enemyBat.transform.position) < (double) enemyBat.MaximumRange)
        enemyBat.Angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(enemyBat.timestamp) * enemyBat.angleNoiseFrequency, 0.0f) - 0.5f) * enemyBat.angleNoiseAmplitude * (float) enemyBat.RanDirection;
      if (!enemyBat.useAcceleration)
        enemyBat.speed = enemyBat.maxSpeed * enemyBat.SpeedMultiplier;
      enemyBat.state.facingAngle = enemyBat.Angle;
      yield return (object) null;
    }
    enemyBat.StartCoroutine((IEnumerator) enemyBat.ChargingRoutine());
    yield break;
label_24:
    enemyBat.CurrentAttackNum = 0;
    enemyBat.StartCoroutine((IEnumerator) enemyBat.AttackRoutine());
  }

  public virtual IEnumerator AttackRoutine()
  {
    EnemyBat enemyBat1 = this;
    enemyBat1.Attacking = true;
    enemyBat1.Spine.AnimationState.SetAnimation(0, enemyBat1.SignPostAttackAnimation, false);
    float Progress = 0.0f;
    float Duration = 1f;
    float CurrentSpeed = enemyBat1.speed;
    float flashTickTimer = 0.0f;
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", enemyBat1.transform.position);
    System.Action<int> onAttack = enemyBat1.OnAttack;
    if (onAttack != null)
      onAttack(enemyBat1.CurrentAttackNum);
    while ((double) (Progress += Time.deltaTime * enemyBat1.Spine.timeScale) < (double) Duration)
    {
      if ((UnityEngine.Object) enemyBat1.GetClosestTarget() != (UnityEngine.Object) null)
        enemyBat1.state.LookAngle = Utils.GetAngle(enemyBat1.transform.position, enemyBat1.GetClosestTarget().transform.position);
      enemyBat1.speed = Mathf.SmoothStep(CurrentSpeed, 0.0f, Progress / Duration);
      if ((UnityEngine.Object) enemyBat1.Aiming != (UnityEngine.Object) null)
      {
        enemyBat1.Aiming.gameObject.SetActive(true);
        if ((double) enemyBat1.Spine.timeScale != 9.9999997473787516E-05)
          enemyBat1.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyBat1.state.LookAngle);
        if ((double) flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
        {
          enemyBat1.Aiming.color = enemyBat1.Aiming.color == Color.red ? Color.white : Color.red;
          flashTickTimer = 0.0f;
        }
        flashTickTimer += Time.deltaTime;
      }
      if ((bool) (UnityEngine.Object) enemyBat1.SimpleSpineFlash)
        enemyBat1.SimpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    if ((bool) (UnityEngine.Object) enemyBat1.SimpleSpineFlash)
      enemyBat1.SimpleSpineFlash.FlashWhite(false);
    enemyBat1.Spine.AnimationState.SetAnimation(0, enemyBat1.AttackAnimation, false);
    enemyBat1.Spine.AnimationState.AddAnimation(0, enemyBat1.IdleAnimation, true, 0.0f);
    enemyBat1.damageColliderEvents.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", enemyBat1.transform.position);
    if (!string.IsNullOrEmpty(enemyBat1.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyBat1.AttackVO, enemyBat1.gameObject);
    enemyBat1.DisableForces = true;
    enemyBat1.Angle = enemyBat1.state.LookAngle * ((float) Math.PI / 180f);
    enemyBat1.Force = (Vector3) new Vector2(enemyBat1.AttackForce * Mathf.Cos(enemyBat1.Angle), enemyBat1.AttackForce * Mathf.Sin(enemyBat1.Angle));
    enemyBat1.rb.AddForce((Vector2) enemyBat1.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBat1.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyBat1.damageColliderEvents.SetActive(false);
    if ((UnityEngine.Object) enemyBat1.Aiming != (UnityEngine.Object) null)
      enemyBat1.Aiming.gameObject.SetActive(false);
    EnemyBat enemyBat2 = enemyBat1;
    int num1 = enemyBat1.CurrentAttackNum + 1;
    int num2 = num1;
    enemyBat2.CurrentAttackNum = num2;
    if (num1 < enemyBat1.NumAttacks)
    {
      enemyBat1.StartCoroutine((IEnumerator) enemyBat1.AttackRoutine());
    }
    else
    {
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyBat1.Spine.timeScale) < (double) enemyBat1.postAttackDelay)
        yield return (object) null;
      enemyBat1.DisableForces = false;
      enemyBat1.Attacking = false;
      enemyBat1.AttackCoolDown = UnityEngine.Random.Range(enemyBat1.AttackCoolDownDuration.x, enemyBat1.AttackCoolDownDuration.y);
      System.Action onAttackComplete = enemyBat1.OnAttackComplete;
      if (onAttackComplete != null)
        onAttackComplete();
      enemyBat1.StartCoroutine((IEnumerator) enemyBat1.ActiveRoutine());
    }
  }

  public virtual IEnumerator ChargingRoutine()
  {
    yield return (object) null;
  }

  public virtual IEnumerator FleeRoutine()
  {
    yield return (object) null;
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyBat enemyBat = this;
    enemyBat.DisableForces = true;
    enemyBat.Angle = Utils.GetAngle(Attacker.transform.position, enemyBat.transform.position) * ((float) Math.PI / 180f);
    enemyBat.Force = (Vector3) new Vector2(1000f * Mathf.Cos(enemyBat.Angle), 1000f * Mathf.Sin(enemyBat.Angle));
    enemyBat.rb.AddForce((Vector2) (enemyBat.Force * enemyBat.KnockbackForceModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBat.Spine.timeScale) < (double) enemyBat.KnockbackDuration)
      yield return (object) null;
    enemyBat.DisableForces = false;
    enemyBat.rb.velocity = Vector2.zero;
    if (enemyBat.AttackAfterKnockback)
      enemyBat.AttackCoolDown = 0.0f;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.gameObject);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!(bool) (UnityEngine.Object) this.DeadBody)
      return;
    GameObject gameObject = this.DeadBody.gameObject.Spawn();
    gameObject.transform.position = this.transform.position;
    gameObject.GetComponent<DeadBodyFlying>().Init(Utils.GetAngle(AttackLocation, this.transform.position));
  }

  public override void OnDisable()
  {
    if (this.Spine.AnimationState != null)
      this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    EnemyBat.enemyBats.Remove(this);
    base.OnDisable();
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.SetActive(false);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public void OnDrawGizmos()
  {
    if (this.StartingPosition.HasValue)
      Utils.DrawCircleXY(this.TargetPosition, 0.3f, Color.red);
    if (this.StartingPosition.HasValue)
      Utils.DrawCircleXY(this.TargetPosition, this.MaximumRange, Color.red);
    else
      Utils.DrawCircleXY(this.transform.position, this.MaximumRange, Color.red);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && (this.health.team != Health.Team.PlayerTeam || !this.health.IsCharmedEnemy))
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public virtual bool ShouldStartCharging() => false;

  public virtual bool ShouldAttack()
  {
    return (double) this.DistanceCheck < (double) this.attackPlayerDistance && GameManager.RoomActive;
  }
}
