// Decompiled with JetBrains decompiler
// Type: EnemySpider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemySpider : UnitObject
{
  public static List<EnemySpider> EnemySpiders = new List<EnemySpider>();
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash Body;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  public bool LoopSignPostAttackAnimation = true;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  public SpriteRenderer ShadowSpriteRenderer;
  public SimpleSpineFlash SimpleSpineFlash;
  public float KnockbackModifier = 1f;
  public int NumberOfAttacks = 1;
  public float AttackForceModifier = 1f;
  public bool CounterAttack;
  public bool CanBeInterrupted = true;
  public bool hasMovementDelay = true;
  [SerializeField]
  public bool wander;
  [SerializeField]
  public bool flee;
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer;
  public int DistanceToPathTowardsPlayer = 6;
  public SkeletonAnimation warningIcon;
  public float RandomDirection;
  public float initialAttackDelayTimer;
  public float initialMovementDelay;
  [SerializeField]
  public string attackSfx;
  [SerializeField]
  public string breakFreeSfx;
  [SerializeField]
  public string deathSfx;
  [SerializeField]
  public string getHitSfx;
  [SerializeField]
  public string jumpSfx;
  [SerializeField]
  public string stuckSfx;
  [SerializeField]
  public string warningSfx;
  [CompilerGenerated]
  public Vector3 \u003CAttackingTargetPosition\u003Ek__BackingField;
  public bool updateDirection = true;
  public float AttackDelayTime;
  [CompilerGenerated]
  public bool \u003CAttacking\u003Ek__BackingField;
  public bool IsStunned;
  [HideInInspector]
  public float AttackDelay;
  public float AttackDuration = 1f;
  public float SignPostAttackDuration = 0.5f;
  public bool DisableKnockback;
  public float KnockBackModifier = 1f;
  public float Angle;
  public Vector3 Force;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public Vector2 IdleWaitRange = new Vector2(1f, 3f);
  public float IdleWait;
  public bool PathingToPlayer;
  public Health EnemyHealth;
  public int DetectEnemyRange = 8;

  public Vector3 AttackingTargetPosition
  {
    get => this.\u003CAttackingTargetPosition\u003Ek__BackingField;
    set => this.\u003CAttackingTargetPosition\u003Ek__BackingField = value;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Spine.ForceVisible = true;
    EnemySpider.EnemySpiders.Add(this);
    if (this.hasMovementDelay)
    {
      this.initialAttackDelayTimer = (bool) (UnityEngine.Object) GameManager.GetInstance() ? GameManager.GetInstance().CurrentTime + 1f : Time.time + 1f;
      this.initialMovementDelay = ((bool) (UnityEngine.Object) GameManager.GetInstance() ? GameManager.GetInstance().CurrentTime : Time.time) + UnityEngine.Random.Range(1f, 1.5f);
    }
    this.RandomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    this.Attacking = false;
    this.updateDirection = true;
    this.health.enabled = true;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    AudioManager.Instance.PlayOneShot(this.deathSfx, this.transform.position);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemySpider.EnemySpiders.Remove(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.ClearPaths();
    this.StopAllCoroutines();
    if (!(bool) (UnityEngine.Object) this.SimpleSpineFlash)
      return;
    this.SimpleSpineFlash.FlashWhite(false);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.TargetEnemy == (UnityEngine.Object) null)
      this.TargetEnemy = this.GetClosestTarget();
    this.ShadowSpriteRenderer.transform.position = this.Spine.transform.position;
  }

  public new void LateUpdate()
  {
    int num = !((UnityEngine.Object) this.TargetEnemy != (UnityEngine.Object) null) ? 0 : ((double) this.TargetEnemy.transform.position.y > (double) this.transform.position.y ? 1 : 0);
    bool flag = (double) this.state.facingAngle > 90.0 && (double) this.state.facingAngle < 270.0;
    if (this.updateDirection && (bool) (UnityEngine.Object) this.Body)
      this.Body.transform.localScale = new Vector3(flag ? this.Body.transform.localScale.y : this.Body.transform.localScale.y * -1f, this.Body.transform.localScale.y, this.Body.transform.localScale.z);
    if (this.Attacking || this.state.CURRENT_STATE != StateMachine.State.Idle || !(bool) (UnityEngine.Object) this.TargetEnemy)
      return;
    this.LookAtTarget();
  }

  public virtual IEnumerator ActiveRoutine()
  {
    EnemySpider enemySpider = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while (GameManager.RoomActive)
      {
        if (enemySpider.state.CURRENT_STATE == StateMachine.State.Idle && (double) (enemySpider.IdleWait -= Time.deltaTime) <= 0.0 && (double) GameManager.GetInstance().CurrentTime > (double) enemySpider.initialMovementDelay)
        {
          if (enemySpider.wander)
            enemySpider.GetNewTargetPosition();
          else
            enemySpider.Flee();
          enemySpider.speed = enemySpider.maxSpeed * enemySpider.SpeedMultiplier;
        }
        if ((UnityEngine.Object) enemySpider.TargetEnemy != (UnityEngine.Object) null && !enemySpider.Attacking && !enemySpider.IsStunned)
          enemySpider.state.LookAngle = Utils.GetAngle(enemySpider.transform.position, enemySpider.TargetEnemy.transform.position);
        else
          enemySpider.state.LookAngle = enemySpider.state.facingAngle;
        if (enemySpider.MovingAnimation != "" && (double) GameManager.GetInstance().CurrentTime > (double) enemySpider.initialMovementDelay)
        {
          if (enemySpider.state.CURRENT_STATE == StateMachine.State.Moving && enemySpider.Spine.AnimationName != enemySpider.MovingAnimation)
            enemySpider.SetAnimation(enemySpider.MovingAnimation, true);
          if (enemySpider.state.CURRENT_STATE == StateMachine.State.Idle && enemySpider.Spine.AnimationName != enemySpider.IdleAnimation)
            enemySpider.SetAnimation(enemySpider.IdleAnimation, true);
        }
        if (enemySpider.ShouldAttack())
          enemySpider.StartCoroutine((IEnumerator) enemySpider.AttackRoutine());
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public virtual bool ShouldAttack()
  {
    return (double) (this.AttackDelay -= Time.deltaTime) < 0.0 && !this.Attacking && (bool) (UnityEngine.Object) this.TargetEnemy && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) < (double) this.VisionRange && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer;
  }

  public bool Attacking
  {
    get => this.\u003CAttacking\u003Ek__BackingField;
    set => this.\u003CAttacking\u003Ek__BackingField = value;
  }

  public virtual IEnumerator AttackRoutine()
  {
    EnemySpider enemySpider = this;
    enemySpider.Attacking = true;
    enemySpider.ClearPaths();
    if (!((UnityEngine.Object) enemySpider.TargetEnemy == (UnityEngine.Object) null))
    {
      int CurrentAttack = 0;
      float time = 0.0f;
      while (++CurrentAttack <= enemySpider.NumberOfAttacks)
      {
        enemySpider.SetAnimation(enemySpider.SignPostAttackAnimation, enemySpider.LoopSignPostAttackAnimation);
        enemySpider.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
        AudioManager.Instance.PlayOneShot(enemySpider.warningSfx, enemySpider.transform.position);
        enemySpider.TargetEnemy = enemySpider.GetClosestTarget();
        enemySpider.LookAtTarget();
        float Progress = 0.0f;
        float Duration = enemySpider.SignPostAttackDuration;
        while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemySpider.Spine.timeScale)
        {
          enemySpider.SimpleSpineFlash.FlashWhite(Progress / Duration);
          yield return (object) null;
        }
        enemySpider.SimpleSpineFlash.FlashWhite(false);
        enemySpider.DisableForces = true;
        enemySpider.Force = (Vector3) (new Vector2(2500f * Mathf.Cos(enemySpider.state.LookAngle * ((float) Math.PI / 180f)), 2500f * Mathf.Sin(enemySpider.state.LookAngle * ((float) Math.PI / 180f))) * enemySpider.AttackForceModifier);
        enemySpider.rb.AddForce((Vector2) enemySpider.Force);
        enemySpider.damageColliderEvents.SetActive(true);
        AudioManager.Instance.PlayOneShot(enemySpider.attackSfx, enemySpider.transform.position);
        enemySpider.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
        enemySpider.SetAnimation(enemySpider.AttackAnimation);
        enemySpider.AddAnimation(enemySpider.IdleAnimation, true);
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemySpider.Spine.timeScale) < (double) enemySpider.AttackDuration)
          yield return (object) null;
        enemySpider.damageColliderEvents.SetActive(false);
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemySpider.Spine.timeScale) < 0.5)
          yield return (object) null;
      }
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySpider.Spine.timeScale) < (double) enemySpider.AttackDuration * 0.5)
        yield return (object) null;
      enemySpider.DisableForces = false;
      enemySpider.state.CURRENT_STATE = StateMachine.State.Idle;
      enemySpider.IdleWait = 0.0f;
      enemySpider.AttackDelay = enemySpider.AttackDelayTime;
      enemySpider.Attacking = false;
      enemySpider.TargetEnemy = (Health) null;
      enemySpider.GetNewTargetPosition();
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!this.DisableKnockback)
      this.damageColliderEvents.SetActive(false);
    if (this.Attacking && this.CanBeInterrupted)
    {
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && !this.DisableKnockback && this.CanBeInterrupted)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    this.SimpleSpineFlash.FlashFillRed();
    AudioManager.Instance.PlayOneShot(this.getHitSfx, this.transform.position);
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemySpider enemySpider = this;
    enemySpider.DisableForces = true;
    enemySpider.Angle = Utils.GetAngle(Attacker.transform.position, enemySpider.transform.position) * ((float) Math.PI / 180f);
    enemySpider.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemySpider.Angle), 1500f * Mathf.Sin(enemySpider.Angle)) * enemySpider.KnockbackModifier);
    enemySpider.rb.AddForce((Vector2) (enemySpider.Force * enemySpider.KnockBackModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpider.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemySpider.DisableForces = false;
  }

  public IEnumerator ApplyForceRoutine(Vector3 forcePosition)
  {
    EnemySpider enemySpider = this;
    enemySpider.DisableForces = true;
    enemySpider.Angle = Utils.GetAngle(forcePosition, enemySpider.transform.position) * ((float) Math.PI / 180f);
    enemySpider.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemySpider.Angle), 1500f * Mathf.Sin(enemySpider.Angle)) * enemySpider.KnockbackModifier);
    enemySpider.rb.AddForce((Vector2) enemySpider.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpider.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemySpider.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemySpider enemySpider = this;
    enemySpider.damageColliderEvents.SetActive(false);
    enemySpider.Attacking = false;
    enemySpider.ClearPaths();
    enemySpider.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemySpider.SetAnimation(enemySpider.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpider.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemySpider.DisableForces = false;
    enemySpider.IdleWait = 0.0f;
    enemySpider.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpider.StartCoroutine((IEnumerator) enemySpider.ActiveRoutine());
    if (enemySpider.CounterAttack)
      enemySpider.StartCoroutine((IEnumerator) enemySpider.AttackRoutine());
  }

  public void GetNewTargetPosition()
  {
    float num = 100f;
    if ((UnityEngine.Object) this.TargetEnemy != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) < (double) this.DistanceToPathTowardsPlayer && this.CheckLineOfSightOnTarget(this.TargetEnemy.gameObject, this.TargetEnemy.transform.position, (float) this.DistanceToPathTowardsPlayer))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, this.TargetEnemy.transform.position) * ((float) Math.PI / 180f);
    }
    while ((double) --num > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      if (!this.PathingToPlayer)
        this.RandomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      this.PathingToPlayer = false;
      float radius = 0.2f;
      Vector3 targetLocation = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      {
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
        this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
        this.givePath(targetLocation);
        break;
      }
    }
  }

  public void Flee()
  {
    if ((bool) (UnityEngine.Object) this.TargetEnemy)
    {
      float num = 100f;
      while ((double) --num > 0.0)
      {
        float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
        float distance = (float) UnityEngine.Random.Range(7, 10);
        Vector3 targetLocation = this.TargetEnemy.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
        if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.TargetEnemy.transform.position, 0.5f, (Vector2) Vector3.Normalize(targetLocation - this.TargetEnemy.transform.position), distance, (int) this.layerToCheck).collider == (UnityEngine.Object) null)
        {
          this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
          this.givePath(targetLocation);
        }
      }
      this.GetNewTargetPosition();
    }
    else
    {
      this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
      this.givePath(this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
    }
  }

  public void SetAnimation(string animationName, bool loop = false)
  {
    this.Spine.AnimationState.SetAnimation(0, animationName, loop);
  }

  public void AddAnimation(string animationName, bool loop = false)
  {
    this.Spine.AnimationState.AddAnimation(0, animationName, loop, 0.0f);
  }

  public void GetNewTarget()
  {
    if (!GameManager.RoomActive)
      return;
    Health closestTarget = this.GetClosestTarget();
    if (!((UnityEngine.Object) closestTarget != (UnityEngine.Object) null))
      return;
    this.EnemyHealth = closestTarget;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public void LookAtTarget()
  {
    if (!(bool) (UnityEngine.Object) this.TargetEnemy)
      return;
    this.LookAtAngle(Utils.GetAngle(this.transform.position, this.TargetEnemy.transform.position));
  }

  public void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }
}
