// Decompiled with JetBrains decompiler
// Type: EnemyHopper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyHopper : UnitObject
{
  public ColliderEvents damageColliderEvents;
  public IEnumerator damageColliderRoutine;
  public List<SkeletonAnimation> Spine = new List<SkeletonAnimation>();
  public SimpleSpineFlash SimpleSpineFlash;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string OnLandSoundPath = string.Empty;
  [EventRef]
  public string OnJumpSoundPath = string.Empty;
  [EventRef]
  public string OnLayEggSoundPath = string.Empty;
  [EventRef]
  public string OnCroakSoundPath = string.Empty;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  public bool _playedVO;
  public GameObject targetObject;
  public float TargetAngle;
  public List<Collider2D> collider2DList = new List<Collider2D>();
  public Health EnemyHealth;
  public bool canBeParried = true;
  public float signPostParryWindow = 0.2f;
  public float attackParryWindow = 0.3f;
  public GameManager gm;
  public CircleCollider2D physicsCollider;
  public float dancingTimestamp;
  public float dancingDuration;
  public float idleTimestamp;
  public float idleDur = 0.6f;
  public float signPostDur = 0.2f;
  public bool canBeStunned = true;
  public bool stunnedStopsAttack = true;
  public float stunnedTimestamp;
  public float stunnedDur = 0.5f;
  public float chargingTimestamp;
  public float chargingDuration = 0.5f;
  public float attackingTimestamp;
  public float attackRange;
  public float hoppingTimestamp;
  public float hoppingDur = 0.4f;
  public float attackingDur = 0.2f;
  public bool isFleeing;
  public float hopMoveSpeed = 1f;
  public AnimationCurve hopSpeedCurve;
  public AnimationCurve hopZCurve;
  public float hopZHeight;
  public static float zFallSpeed = 15f;
  public bool hasCollidedWithObstacle;
  public bool canLayEggs;
  public GameObject eggPrefab;
  public float lastLaidEggTimestamp;
  public float minTimeBetweenEggs = 6f;
  public bool alwaysTargetPlayer;
  public static List<EnemyHopper> EnemyHoppers = new List<EnemyHopper>();
  public static int maxHoppersPerRoom = 3;
  public static int maxEggsPerRoom = 1;
  public string chargingAnimationString = "lay-egg";
  public bool canSprayPoison;
  public GameObject poisonPrefab;
  public List<FollowAsTail> TailPieces = new List<FollowAsTail>();
  public float KnockBackMultipier = 0.7f;

  public override void Awake()
  {
    base.Awake();
    this.physicsCollider = this.GetComponent<CircleCollider2D>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    this.state.OnStateChange += new StateMachine.StateChange(this.OnStateChange);
    this.gm = GameManager.GetInstance();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if ((UnityEngine.Object) this.gm != (UnityEngine.Object) null)
      this.idleTimestamp = this.gm.CurrentTime;
    if (EnemyHopper.EnemyHoppers != null)
      this.idleTimestamp += (float) EnemyHopper.EnemyHoppers.Count * 0.1f;
    else
      this.idleTimestamp += UnityEngine.Random.Range(0.0f, this.idleDur);
    EnemyHopper.EnemyHoppers.Add(this);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnDisable()
  {
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    this.state.OnStateChange -= new StateMachine.StateChange(this.OnStateChange);
    this.ClearPaths();
    EnemyHopper.EnemyHoppers.Remove(this);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void Update()
  {
    base.Update();
    this.Seperate(0.5f, true);
    if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
    {
      this.gm = GameManager.GetInstance();
      if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
        return;
    }
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.UpdateStateIdle();
        break;
      case StateMachine.State.Moving:
        this.UpdateStateMoving();
        break;
      case StateMachine.State.Charging:
        this.UpdateStateCharging();
        break;
      case StateMachine.State.Vulnerable:
        this.UpdateStateVulnerable();
        break;
      case StateMachine.State.Dancing:
        this.UpdateStateDancing();
        break;
    }
    int index = -1;
    while (++index < this.TailPieces.Count)
    {
      if ((UnityEngine.Object) this.TailPieces[index] != (UnityEngine.Object) null)
        this.TailPieces[index].UpdatePosition();
    }
    if (this.state.CURRENT_STATE != StateMachine.State.Moving)
      this.Spine[0].transform.localPosition = Vector3.Lerp(this.Spine[0].transform.localPosition, Vector3.zero, Time.deltaTime * EnemyHopper.zFallSpeed);
    this.move();
  }

  public virtual void UpdateStateIdle()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.idleTimestamp) >= ((double) this.idleDur - (double) this.signPostParryWindow) * (double) this.Spine[0].timeScale)
      this.canBeParried = true;
    float num = this.idleDur - this.signPostDur;
    if ((double) this.gm.TimeSince(this.idleTimestamp) >= (double) num * (double) this.Spine[0].timeScale)
      this.SimpleSpineFlash.FlashWhite((float) (0.75 * ((double) this.gm.TimeSince(this.idleTimestamp) - (double) num / (double) this.signPostDur)) * this.Spine[0].timeScale);
    if ((double) this.gm.TimeSince(this.idleTimestamp) < (double) this.idleDur * (double) this.Spine[0].timeScale)
      return;
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null && (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
      this.targetObject = this.GetClosestTarget().gameObject;
    bool flag = this.TargetIsVisible();
    if (this.canLayEggs && (double) this.gm.TimeSince(this.lastLaidEggTimestamp - 0.5f) >= (double) this.minTimeBetweenEggs * (double) this.Spine[0].timeScale && EnemyHopper.EnemyHoppers.Count < EnemyHopper.maxHoppersPerRoom && EnemyEgg.EnemyEggs.Count < EnemyHopper.maxEggsPerRoom)
    {
      this.alwaysTargetPlayer = false;
      this.isFleeing = true;
    }
    this.TargetAngle = !this.alwaysTargetPlayer ? (!this.isFleeing ? (!flag ? this.GetRandomFacingAngle() : this.GetAngleToTarget()) : this.GetFleeAngle()) : (!((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null) ? this.GetFleeAngle() : this.GetAngleToTarget());
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
    foreach (SkeletonAnimation skeletonAnimation in this.Spine)
    {
      if ((double) this.Spine[0].timeScale != 9.9999997473787516E-05)
        skeletonAnimation.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    if (this.ShouldSprayPoison())
      this.SprayPoison();
    this.state.CURRENT_STATE = StateMachine.State.Moving;
  }

  public virtual void UpdateStateMoving()
  {
    if (!this._playedVO)
    {
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.gameObject);
      this._playedVO = true;
    }
    this.speed = this.hopSpeedCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / (this.hoppingDur * this.Spine[0].timeScale)) * this.hopMoveSpeed;
    if (this.hasCollidedWithObstacle || this.TargetIsInAttackRange())
      this.speed *= 0.5f;
    this.speed *= this.Spine[0].timeScale;
    this.Spine[0].transform.localPosition = -Vector3.forward * this.hopZCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopZHeight * this.Spine[0].timeScale;
    this.canBeParried = (double) this.gm.TimeSince(this.hoppingTimestamp) <= (double) this.attackParryWindow * (double) this.Spine[0].timeScale;
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) < (double) this.hoppingDur / (double) this.Spine[0].timeScale)
      return;
    this.speed = 0.0f;
    this.DoAttack();
    this._playedVO = false;
    if (this.ShouldStartCharging())
      this.state.CURRENT_STATE = StateMachine.State.Charging;
    else
      this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void DoAttack()
  {
    AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
    if (this.damageColliderRoutine != null)
      this.StopCoroutine((IEnumerator) this.damageColliderRoutine);
    this.damageColliderRoutine = this.TurnOnDamageColliderForDuration(this.attackingDur);
    this.StartCoroutine((IEnumerator) this.damageColliderRoutine);
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive((double) this.Spine[0].timeScale != 9.9999997473787516E-05);
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      this.SimpleSpineFlash.FlashWhite(1f - Mathf.Clamp01(t / duration));
      yield return (object) null;
    }
    this.SimpleSpineFlash.FlashWhite(false);
    this.damageColliderEvents.SetActive(false);
  }

  public virtual void UpdateStateVulnerable()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.stunnedTimestamp) < (double) this.stunnedDur * (double) this.Spine[0].timeScale)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void UpdateStateCharging()
  {
    if ((double) this.gm.TimeSince(this.chargingTimestamp) < (double) this.chargingDuration * (double) this.Spine[0].timeScale || !this.canLayEggs || EnemyHopper.EnemyHoppers.Count >= EnemyHopper.maxHoppersPerRoom || EnemyEgg.EnemyEggs.Count >= EnemyHopper.maxEggsPerRoom)
      return;
    this.LayEgg();
    this.idleDur = this.signPostParryWindow;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.alwaysTargetPlayer = true;
    this.isFleeing = false;
  }

  public virtual void UpdateStateDancing()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.dancingTimestamp) < (double) this.dancingDuration * (double) this.Spine[0].timeScale)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void GetTailPieces()
  {
    this.TailPieces = new List<FollowAsTail>((IEnumerable<FollowAsTail>) this.GetComponentsInChildren<FollowAsTail>());
  }

  public bool TargetIsVisible()
  {
    if (!GameManager.RoomActive || (UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
      return false;
    float a = Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.targetObject.transform.position, this.transform.position));
    return (double) a <= (double) this.VisionRange && this.CheckLineOfSightOnTarget(this.targetObject, this.targetObject.transform.position, Mathf.Min(a, (float) this.VisionRange));
  }

  public bool TargetIsInAttackRange()
  {
    return GameManager.RoomActive && !((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null) && (double) Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.targetObject.transform.position, this.transform.position)) <= (double) this.attackRange * 0.75;
  }

  public Vector3 GetHopPositionTowardsTarget(Vector3 targetPos)
  {
    return (targetPos - this.transform.position).normalized * this.attackRange + this.transform.position;
  }

  public float GetAngleToTarget()
  {
    float angle = Utils.GetAngle(this.transform.position, this.targetObject.transform.position);
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return angle;
    float num = 32f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(angle * ((float) Math.PI / 180f)), Mathf.Sin(angle * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      angle += (float) (360.0 / ((double) num + 1.0));
    return angle;
  }

  public float GetRandomFacingAngle()
  {
    float randomFacingAngle = (float) UnityEngine.Random.Range(0, 360);
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return randomFacingAngle;
    float num = 16f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(randomFacingAngle * ((float) Math.PI / 180f)), Mathf.Sin(randomFacingAngle * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      randomFacingAngle += (float) (360.0 / ((double) num + 1.0));
    return randomFacingAngle;
  }

  public float GetFleeAngle()
  {
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
      return this.GetRandomFacingAngle();
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(4, 7);
      Vector3 toPosition = this.targetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.targetObject.transform.position, 1.5f, (Vector2) Vector3.Normalize(toPosition - this.targetObject.transform.position), distance, (int) this.layerToCheck);
      if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
        return Utils.GetAngle(this.transform.position, toPosition);
      if ((double) this.MagnitudeFindDistanceBetween(this.targetObject.transform.position, (Vector3) raycastHit2D.centroid) > 9.0)
        return Utils.GetAngle(this.transform.position, toPosition);
    }
    return this.GetRandomFacingAngle();
  }

  public virtual bool ShouldStartCharging()
  {
    return GameManager.RoomActive && this.canLayEggs && (double) this.gm.TimeSince(this.lastLaidEggTimestamp) >= (double) this.minTimeBetweenEggs * (double) this.Spine[0].timeScale && EnemyHopper.EnemyHoppers.Count < EnemyHopper.maxHoppersPerRoom && EnemyEgg.EnemyEggs.Count < EnemyHopper.maxEggsPerRoom;
  }

  public bool ShouldSprayPoison() => this.canSprayPoison;

  public virtual void OnStateChange(StateMachine.State newState, StateMachine.State prevState)
  {
    if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
      return;
    switch (newState)
    {
      case StateMachine.State.Idle:
        if (newState == prevState)
          break;
        this.idleTimestamp = this.gm.CurrentTime;
        foreach (SkeletonAnimation skeletonAnimation in this.Spine)
        {
          if (skeletonAnimation.AnimationState != null)
          {
            if (prevState == StateMachine.State.Moving)
              skeletonAnimation.AnimationState.SetAnimation(0, "jump-end", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          }
        }
        if (!string.IsNullOrEmpty(this.OnLandSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnLandSoundPath, this.transform.position);
        this.targetObject = (GameObject) null;
        this.SimpleSpineFlash.FlashWhite(false);
        break;
      case StateMachine.State.Moving:
        this.hasCollidedWithObstacle = false;
        this.hoppingTimestamp = this.gm.CurrentTime;
        this.SimpleSpineFlash.FlashWhite(false);
        foreach (SkeletonAnimation skeletonAnimation in this.Spine)
        {
          if (skeletonAnimation.AnimationState != null)
            skeletonAnimation.AnimationState.SetAnimation(0, "jump", false);
        }
        if (string.IsNullOrEmpty(this.OnJumpSoundPath))
          break;
        AudioManager.Instance.PlayOneShot(this.OnJumpSoundPath, this.transform.position);
        break;
      case StateMachine.State.Charging:
        foreach (SkeletonAnimation skeletonAnimation in this.Spine)
        {
          if (skeletonAnimation.AnimationState != null)
          {
            skeletonAnimation.AnimationState.SetAnimation(0, this.chargingAnimationString, false);
            skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          }
        }
        if (!string.IsNullOrEmpty(this.OnLayEggSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnLayEggSoundPath, this.transform.position);
        this.chargingTimestamp = this.gm.CurrentTime;
        break;
      case StateMachine.State.Vulnerable:
        this.stunnedTimestamp = this.gm.CurrentTime;
        this.SimpleSpineFlash.FlashWhite(false);
        if (!this.stunnedStopsAttack || !((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
          break;
        this.damageColliderEvents.SetActive(false);
        break;
      case StateMachine.State.Dancing:
        this.dancingTimestamp = this.gm.CurrentTime;
        foreach (SkeletonAnimation skeletonAnimation in this.Spine)
        {
          if (skeletonAnimation.AnimationState != null)
          {
            skeletonAnimation.AnimationState.SetAnimation(0, "roar", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          }
        }
        if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
          this.damageColliderEvents.SetActive(false);
        if (string.IsNullOrEmpty(this.OnCroakSoundPath))
          break;
        AudioManager.Instance.PlayOneShot(this.OnCroakSoundPath, this.transform.position);
        break;
    }
  }

  public new void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.canBeParried)
      return;
    this.health.WasJustParried = true;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (string.IsNullOrEmpty(this.DeathVO))
      return;
    AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.health.HasShield)
      return;
    if ((double) this.KnockBackMultipier != 0.0)
      this.DoKnockBack(Attacker, this.KnockBackMultipier, 1f);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.SimpleSpineFlash.FlashWhite(false);
    this.UsePathing = true;
    this.health.invincible = false;
    this.targetObject = (GameObject) null;
    if (this.canBeStunned && (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Attacking))
    {
      this.state.CURRENT_STATE = StateMachine.State.Vulnerable;
      foreach (SkeletonAnimation skeletonAnimation in this.Spine)
      {
        if (skeletonAnimation.AnimationState != null)
        {
          skeletonAnimation.AnimationState.SetAnimation(0, "hit", false);
          skeletonAnimation.AnimationState.AddAnimation(0, "idle", false, 0.0f);
        }
      }
    }
    this.SimpleSpineFlash.FlashFillRed();
  }

  public void LayEgg()
  {
    if ((UnityEngine.Object) this.gm != (UnityEngine.Object) null)
      this.lastLaidEggTimestamp = this.gm.CurrentTime;
    this.SimpleSpineFlash.FlashWhite(false);
    int num = 1;
    for (int index = 0; index < num; ++index)
    {
      GameObject gameObject = ObjectPool.Spawn(this.eggPrefab, (UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null ? this.transform.parent : (Transform) null, this.transform.position + new Vector3(0.0f, -0.5f, 0.0f), Quaternion.identity);
      gameObject.GetComponent<UnitObject>().DisableForces = true;
      gameObject.GetComponent<DeadBodySliding>().Init(this.gameObject, UnityEngine.Random.Range(0.0f, 360f), 1200f);
      gameObject.transform.localScale = Vector3.zero;
      gameObject.transform.DOScale(Vector3.one, 1f);
    }
  }

  public void SprayPoison()
  {
    this.SimpleSpineFlash.FlashWhite(false);
    int num = 1;
    for (int index = 0; index < num; ++index)
      UnityEngine.Object.Instantiate<GameObject>(this.poisonPrefab, this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle, Quaternion.identity, (Transform) null);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Moving || collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    Debug.Log((object) "I have hopped into an obstacle", (UnityEngine.Object) this.gameObject);
    this.hasCollidedWithObstacle = true;
    this.TargetAngle += (double) this.TargetAngle > 180.0 ? -180f : 180f;
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }
}
