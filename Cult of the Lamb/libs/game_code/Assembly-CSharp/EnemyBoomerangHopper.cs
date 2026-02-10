// Decompiled with JetBrains decompiler
// Type: EnemyBoomerangHopper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class EnemyBoomerangHopper : UnitObject
{
  public ColliderEvents damageColliderEvents;
  public IEnumerator damageColliderRoutine;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnim = "idle";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string jumpAnim = "jump";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string jumpEndAnim = "jump-end";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackChargeAnim = "attack-shoot-charge";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackThrowAnim = "attack-shoot-throw";
  [EventRef]
  public string attackHeadStartSFX = "event:/dlc/dungeon06/enemy/boomerang/attack_head_start";
  [EventRef]
  public string attackHeadThrowSFX = "event:/dlc/dungeon06/enemy/boomerang/attack_head_throw";
  [EventRef]
  public string OnLandSoundPath = "event:/enemy/hopper/hopper_land";
  [EventRef]
  public string OnJumpSoundPath = "event:/enemy/hopper/hopper_jump";
  [EventRef]
  public string OnHeadCatchSFX = "event:/dlc/dungeon06/enemy/boomerang/attack_head_catch";
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/warning";
  public bool _playedVO;
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
  public static List<EnemyBoomerangHopper> EnemyHoppers = new List<EnemyBoomerangHopper>();
  public static int maxHoppersPerRoom = 3;
  public static int maxEggsPerRoom = 1;
  public string chargingAnimationString = "lay-egg";
  public bool canSprayPoison;
  public GameObject poisonPrefab;
  [SerializeField]
  public GameObject boomerangProjectile;
  [SerializeField]
  public int jumpsBeforeAttack;
  [SerializeField]
  public int jumpsTowardsTarget = 5;
  [SerializeField]
  public float boomerangeAttackDur = 1f;
  [SerializeField]
  public float boomerangSignDur = 0.25f;
  [SerializeField]
  [Tooltip("Boomerang speed towards target")]
  public float boomerangSpeed = 8f;
  [SerializeField]
  [Tooltip("Boomerang turning speed towards target")]
  public float boomerangTurningSpeed = 3f;
  [SerializeField]
  [Tooltip("Boomerang angle added towards target to make curve")]
  public float boomerangThrowAngle = 90f;
  [SerializeField]
  [Tooltip("Boomerang speed towards owner")]
  public float boomerangRecoverSpeed = 6f;
  [SerializeField]
  [Tooltip("Boomerang turning speed towards owner")]
  public float boomerangRecoverTurningSpeed = 10f;
  [SerializeField]
  [Tooltip("How much time boomerang flies towards target before return to owner")]
  public float boomerangHomecomingTime;
  [SerializeField]
  [Tooltip("Distance from owner at boomerang is recovered by owner")]
  public float boomerangRecoverDistance = 0.25f;
  public int jumpsPerformed;
  public float boomerangAttackStartTimeStamp;
  public float boomerangTimeStamp;
  public Projectile currentBoomerang;
  public bool boomerangThrown;
  public float attackProgressTimer;
  public List<FollowAsTail> TailPieces = new List<FollowAsTail>();
  public float KnockBackMultipier = 0.7f;
  public bool boomerangHitPlayer;
  public Vector3 targetPosition;

  public bool ShouldThrowBoomerang
  {
    get
    {
      return (UnityEngine.Object) this.currentBoomerang == (UnityEngine.Object) null && this.jumpsPerformed >= this.jumpsBeforeAttack;
    }
  }

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
    if (EnemyBoomerangHopper.EnemyHoppers != null)
      this.idleTimestamp += (float) EnemyBoomerangHopper.EnemyHoppers.Count * 0.1f;
    else
      this.idleTimestamp += UnityEngine.Random.Range(0.0f, this.idleDur);
    EnemyBoomerangHopper.EnemyHoppers.Add(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.alwaysTargetPlayer = false;
    this.SetNewTargetPosition();
  }

  public override void OnDisable()
  {
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    this.state.OnStateChange -= new StateMachine.StateChange(this.OnStateChange);
    this.ClearPaths();
    EnemyBoomerangHopper.EnemyHoppers.Remove(this);
    this.SeperateObject = true;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.Spine.AnimationState.SetAnimation(1, "head-on", false);
    this.Spine.Skeleton.SetSkin("head");
    this.Spine.Skeleton.SetToSetupPose();
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
      case StateMachine.State.Attacking:
        this.UpdateStateAttacking();
        break;
      case StateMachine.State.SignPostAttack:
        this.UpdateStateSignPostAttack();
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
      this.Spine.transform.localPosition = Vector3.Lerp(this.Spine.transform.localPosition, Vector3.zero, Time.deltaTime * EnemyBoomerangHopper.zFallSpeed);
    this.move();
  }

  public virtual void UpdateStateIdle()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.idleTimestamp) >= ((double) this.idleDur - (double) this.signPostParryWindow) * (double) this.Spine.timeScale)
      this.canBeParried = true;
    float num1 = this.idleDur - this.signPostDur;
    double num2 = (double) this.gm.TimeSince(this.idleTimestamp);
    double num3 = (double) num1 * (double) this.Spine.timeScale;
    if ((double) this.gm.TimeSince(this.idleTimestamp) < (double) this.idleDur * (double) this.Spine.timeScale)
      return;
    bool flag = this.TargetIsVisible();
    if (this.jumpsPerformed == 0)
      this.SetNewTargetPosition();
    if (this.alwaysTargetPlayer)
      this.TargetAngle = !((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null) ? this.GetAngleToPosition(this.targetPosition) : this.GetAngleToTarget();
    else if (flag)
      this.TargetAngle = this.GetAngleToPosition(this.targetPosition);
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
    if ((double) this.Spine.timeScale != 9.9999997473787516E-05)
      this.Spine.Skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    this.state.CURRENT_STATE = StateMachine.State.Moving;
  }

  public virtual void UpdateStateMoving()
  {
    this.speed = this.hopSpeedCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / (this.hoppingDur * this.Spine.timeScale)) * this.hopMoveSpeed * this.Spine.timeScale;
    if (this.hasCollidedWithObstacle || this.TargetIsInAttackRange())
      this.speed *= 0.5f;
    this.speed *= this.Spine.timeScale;
    this.Spine.transform.localPosition = -Vector3.forward * this.hopZCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopZHeight * this.Spine.timeScale;
    this.canBeParried = (double) this.gm.TimeSince(this.hoppingTimestamp) <= (double) this.attackParryWindow * (double) this.Spine.timeScale;
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) < (double) this.hoppingDur / (double) this.Spine.timeScale)
      return;
    this.speed = 0.0f;
    this.DoAttack();
    this._playedVO = false;
    ++this.jumpsPerformed;
    if (this.ShouldThrowBoomerang)
    {
      if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
      {
        this.TargetAngle = this.GetAngleToTarget();
        this.state.LookAngle = this.TargetAngle;
        this.state.facingAngle = this.TargetAngle;
        if ((double) this.Spine.timeScale != 9.9999997473787516E-05)
          this.Spine.Skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
      }
      this.FinishJumps();
      this.state.CURRENT_STATE = StateMachine.State.Attacking;
    }
    else
      this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void UpdateStateSignPostAttack()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
    {
      this.FinishJumps();
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    this.speed = this.hopSpeedCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / (this.hoppingDur * this.Spine.timeScale)) * this.hopMoveSpeed * this.Spine.timeScale;
    if (this.hasCollidedWithObstacle || (double) Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.GetClosestTarget().transform.position, this.transform.position)) < 0.34999999403953552)
      this.speed *= 0.5f;
    this.speed *= this.Spine.timeScale;
    this.Spine.transform.localPosition = -Vector3.forward * this.hopZCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopZHeight * this.Spine.timeScale;
    this.canBeParried = (double) this.gm.TimeSince(this.hoppingTimestamp) <= (double) this.attackParryWindow * (double) this.Spine.timeScale;
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) < (double) this.hoppingDur / (double) this.Spine.timeScale)
      return;
    this.speed = 0.0f;
    this.DoAttack();
    ++this.jumpsPerformed;
    if (this.jumpsPerformed >= this.jumpsTowardsTarget || (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < 0.800000011920929)
    {
      this.FinishJumps();
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    else
    {
      this.TargetAngle = (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null ? this.GetAngleToTarget() : this.GetAngleToPosition(this.targetPosition);
      this.state.LookAngle = this.TargetAngle;
      this.state.facingAngle = this.TargetAngle;
      if ((double) this.Spine.timeScale != 9.9999997473787516E-05)
        this.Spine.Skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
      this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    }
  }

  public virtual void DoAttack()
  {
    if (this.damageColliderRoutine != null)
      this.StopCoroutine((IEnumerator) this.damageColliderRoutine);
    this.damageColliderRoutine = this.TurnOnDamageColliderForDuration(this.attackingDur);
    this.StartCoroutine((IEnumerator) this.damageColliderRoutine);
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive((double) this.Spine.timeScale != 9.9999997473787516E-05);
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.SimpleSpineFlash.FlashWhite(false);
    this.damageColliderEvents.SetActive(false);
  }

  public virtual void UpdateStateVulnerable()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.stunnedTimestamp) < (double) this.stunnedDur * (double) this.Spine.timeScale)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void UpdateStateCharging()
  {
    if ((double) this.gm.TimeSince(this.chargingTimestamp) < (double) this.chargingDuration * (double) this.Spine.timeScale || !this.canLayEggs || EnemyBoomerangHopper.EnemyHoppers.Count >= EnemyBoomerangHopper.maxHoppersPerRoom || EnemyEgg.EnemyEggs.Count >= EnemyBoomerangHopper.maxEggsPerRoom)
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
    if (!((UnityEngine.Object) this.currentBoomerang == (UnityEngine.Object) null))
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void UpdateStateAttacking()
  {
    this.attackProgressTimer += Time.deltaTime * this.Spine.timeScale;
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
    {
      this.FinishJumps();
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    if (!this.boomerangThrown && this.Spine.AnimationName != this.attackChargeAnim)
    {
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.gameObject);
      AudioManager.Instance.PlayOneShot(this.attackHeadStartSFX, this.gameObject);
      this.Spine.AnimationState.SetAnimation(0, this.attackChargeAnim, false);
    }
    if ((double) this.attackProgressTimer <= (double) this.boomerangeAttackDur)
      return;
    if (!this.boomerangThrown)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
      AudioManager.Instance.PlayOneShot(this.attackHeadThrowSFX, this.gameObject);
      this.ThrowBoomerang();
      this.Spine.AnimationState.SetAnimation(0, this.attackThrowAnim, false);
      this.Spine.Skeleton.SetSkin("no-head");
      this.Spine.Skeleton.SetToSetupPose();
    }
    if ((double) this.attackProgressTimer <= (double) this.boomerangSignDur + (double) this.boomerangeAttackDur)
      return;
    if (this.jumpsPerformed == 0)
      this.SetNewTargetPosition();
    this.Spine.AnimationState.SetAnimation(0, this.idleAnim, true);
    this.TargetAngle = (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null ? this.GetAngleToTarget() : this.GetAngleToPosition(this.targetPosition);
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
    if ((double) this.Spine.timeScale != 9.9999997473787516E-05)
      this.Spine.Skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
  }

  public void GetTailPieces()
  {
    this.TailPieces = new List<FollowAsTail>((IEnumerable<FollowAsTail>) this.GetComponentsInChildren<FollowAsTail>());
  }

  public bool TargetIsVisible()
  {
    if (!GameManager.RoomActive || (UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return false;
    float a = Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.GetClosestTarget().transform.position, this.transform.position));
    return (double) a <= (double) this.VisionRange && this.CheckLineOfSightOnTarget(this.GetClosestTarget().gameObject, this.GetClosestTarget().transform.position, Mathf.Min(a, (float) this.VisionRange));
  }

  public bool TargetIsInAttackRange()
  {
    return GameManager.RoomActive && !((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null) && (double) Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.GetClosestTarget().transform.position, this.transform.position)) <= (double) this.attackRange * 0.75;
  }

  public Vector3 GetHopPositionTowardsTarget(Vector3 targetPos)
  {
    return (targetPos - this.transform.position).normalized * this.attackRange + this.transform.position;
  }

  public float GetAngleToTarget()
  {
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return angle;
    float num = 32f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(angle * ((float) Math.PI / 180f)), Mathf.Sin(angle * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      angle += (float) (360.0 / ((double) num + 1.0));
    return angle;
  }

  public float GetAngleToPosition(Vector3 position)
  {
    float angle = Utils.GetAngle(this.transform.position, position);
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
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return this.GetRandomFacingAngle();
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(4, 7);
      Vector3 toPosition = this.GetClosestTarget().transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      Vector3 direction = Vector3.Normalize(toPosition - this.GetClosestTarget().transform.position);
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.GetClosestTarget().transform.position, 1.5f, (Vector2) direction, distance, (int) this.layerToCheck);
      if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
        return Utils.GetAngle(this.transform.position, toPosition);
      if ((double) this.MagnitudeFindDistanceBetween(this.GetClosestTarget().transform.position, (Vector3) raycastHit2D.centroid) > 9.0)
        return Utils.GetAngle(this.transform.position, toPosition);
    }
    return this.GetRandomFacingAngle();
  }

  public virtual bool ShouldStartCharging()
  {
    return GameManager.RoomActive && this.canLayEggs && (double) this.gm.TimeSince(this.lastLaidEggTimestamp) >= (double) this.minTimeBetweenEggs * (double) this.Spine.timeScale && EnemyBoomerangHopper.EnemyHoppers.Count < EnemyBoomerangHopper.maxHoppersPerRoom && EnemyEgg.EnemyEggs.Count < EnemyBoomerangHopper.maxEggsPerRoom;
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
        MonoBehaviour.print((object) "to idle");
        if (prevState == StateMachine.State.Moving)
        {
          this.Spine.AnimationState.SetAnimation(0, this.jumpEndAnim, false);
          this.Spine.AnimationState.AddAnimation(0, this.idleAnim, true, 0.0f);
        }
        else
          this.Spine.AnimationState.SetAnimation(0, this.idleAnim, true);
        if (!string.IsNullOrEmpty(this.OnLandSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnLandSoundPath, this.transform.position);
        this.SimpleSpineFlash.FlashWhite(false);
        break;
      case StateMachine.State.Moving:
        this.hasCollidedWithObstacle = false;
        this.hoppingTimestamp = this.gm.CurrentTime;
        this.SimpleSpineFlash.FlashWhite(false);
        MonoBehaviour.print((object) "to move");
        this.Spine.AnimationState.SetAnimation(0, this.jumpAnim, false);
        if (string.IsNullOrEmpty(this.OnJumpSoundPath))
          break;
        AudioManager.Instance.PlayOneShot(this.OnJumpSoundPath, this.transform.position);
        break;
      case StateMachine.State.Attacking:
        this.attackProgressTimer = 0.0f;
        this.boomerangThrown = false;
        break;
      case StateMachine.State.SignPostAttack:
        this.hasCollidedWithObstacle = false;
        this.hoppingTimestamp = this.gm.CurrentTime;
        this.SimpleSpineFlash.FlashWhite(false);
        MonoBehaviour.print((object) "to signpost");
        this.Spine.AnimationState.SetAnimation(0, this.jumpAnim, false);
        if (string.IsNullOrEmpty(this.OnJumpSoundPath))
          break;
        AudioManager.Instance.PlayOneShot(this.OnJumpSoundPath, this.transform.position);
        break;
      case StateMachine.State.Charging:
        this.Spine.AnimationState.SetAnimation(0, this.chargingAnimationString, false);
        this.Spine.AnimationState.AddAnimation(0, this.idleAnim, true, 0.0f);
        this.chargingTimestamp = this.gm.CurrentTime;
        break;
      case StateMachine.State.Vulnerable:
        this.FinishJumps();
        this.stunnedTimestamp = this.gm.CurrentTime;
        this.SimpleSpineFlash.FlashWhite(false);
        if (!this.stunnedStopsAttack || !((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
          break;
        this.damageColliderEvents.SetActive(false);
        break;
      case StateMachine.State.Dancing:
        this.dancingTimestamp = this.gm.CurrentTime;
        MonoBehaviour.print((object) "to dance");
        this.Spine.AnimationState.SetAnimation(0, "roar", false);
        this.Spine.AnimationState.AddAnimation(0, this.idleAnim, true, 0.0f);
        if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
          break;
        this.damageColliderEvents.SetActive(false);
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
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    if (!((UnityEngine.Object) this.currentBoomerang != (UnityEngine.Object) null))
      return;
    this.currentBoomerang.DestroyProjectile(true);
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
    if (!string.IsNullOrEmpty(this.GetHitVO) && !this.boomerangThrown)
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.SimpleSpineFlash.FlashWhite(false);
    this.UsePathing = true;
    this.health.invincible = false;
    this.state.CURRENT_STATE = StateMachine.State.Vulnerable;
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

  public void ThrowBoomerang()
  {
    Projectile component = ObjectPool.Spawn(this.boomerangProjectile, this.transform.position).GetComponent<Projectile>();
    component.transform.parent = this.transform.parent;
    component.Angle = this.GetAngleToTarget() + Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)) * this.boomerangThrowAngle;
    component.Speed = this.boomerangSpeed;
    component.team = this.health.team;
    component.LifeTime = 30f;
    component.IgnoreIsland = true;
    component.Trail.time = 0.3f;
    component.Owner = this.health;
    component.homeInOnTarget = true;
    component.turningSpeed = this.boomerangTurningSpeed;
    component.CollideOnlyTarget = true;
    component.SetTarget(this.GetClosestTarget().GetComponent<Health>());
    component.Destroyable = false;
    component.SetParentSpine(this.Spine);
    component.onHitOwner += new Projectile.OnHitUnit(this.OnHitOwner);
    component.onHitPlayer += new Projectile.OnHitUnit(this.OnHitPlayer);
    component.OnDestroyProjectile.AddListener(new UnityAction(this.OnDestroyProjetile));
    this.currentBoomerang = component;
    this.boomerangTimeStamp = this.gm.CurrentTime;
    Rotator componentInChildren = component.GetComponentInChildren<Rotator>();
    if ((bool) (UnityEngine.Object) componentInChildren)
      componentInChildren.SetParentSpine(this.Spine);
    this.StartCoroutine((IEnumerator) this.BoomerangMovement(component));
    this.boomerangThrown = true;
  }

  public IEnumerator BoomerangMovement(Projectile boomerang)
  {
    while ((double) this.gm.TimeSince(this.boomerangTimeStamp) <= (double) this.boomerangHomecomingTime && !this.boomerangHitPlayer)
      yield return (object) null;
    boomerang.SetTarget(boomerang.Owner);
    boomerang.CollideOnlyTarget = false;
    boomerang.Speed = this.boomerangRecoverSpeed;
    boomerang.turningSpeed = this.boomerangRecoverTurningSpeed;
    while ((UnityEngine.Object) this.currentBoomerang != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(boomerang.transform.position, boomerang.Owner.transform.position) < (double) this.boomerangRecoverDistance)
        this.OnHitOwner(boomerang);
      yield return (object) null;
    }
  }

  public void OnHitOwner(Projectile boomerang)
  {
    this.boomerangHitPlayer = false;
    boomerang.Destroyable = true;
    boomerang.DestroyProjectile();
    AudioManager.Instance.PlayOneShot(this.OnHeadCatchSFX, this.gameObject);
    this.Spine.AnimationState.SetAnimation(1, "head-on", false);
    this.Spine.Skeleton.SetSkin("head");
    this.Spine.Skeleton.SetToSetupPose();
  }

  public void OnHitPlayer(Projectile boomerang)
  {
    if (this.boomerangHitPlayer)
      return;
    this.boomerangHitPlayer = true;
  }

  public void OnDestroyProjetile()
  {
    this.currentBoomerang.onHitOwner -= new Projectile.OnHitUnit(this.OnHitOwner);
    this.currentBoomerang.onHitPlayer -= new Projectile.OnHitUnit(this.OnHitPlayer);
    this.currentBoomerang.OnDestroyProjectile.RemoveListener(new UnityAction(this.OnDestroyProjetile));
    this.currentBoomerang = (Projectile) null;
  }

  public void SetNewTargetPosition()
  {
    float num = 100f;
    Health closestTarget = this.GetClosestTarget();
    while ((double) --num > 0.0)
    {
      Vector3 positionInIsland = BiomeGenerator.GetRandomPositionInIsland();
      if (!((UnityEngine.Object) closestTarget == (UnityEngine.Object) null) && ((double) Vector3.Distance(closestTarget.transform.position, positionInIsland) >= 2.5 || (double) Vector3.Distance(this.transform.position, positionInIsland) >= 5.0))
      {
        this.targetPosition = positionInIsland;
        break;
      }
    }
  }

  public void FinishJumps() => this.jumpsPerformed = 0;
}
