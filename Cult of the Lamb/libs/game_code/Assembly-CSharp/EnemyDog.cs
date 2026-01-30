// Decompiled with JetBrains decompiler
// Type: EnemyDog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyDog : UnitObject
{
  public static List<EnemyDog> Dogs = new List<EnemyDog>();
  public static int CurrentAttackerIndex = 0;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  public bool LoopSignPostAttackAnimation = true;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DiveIntoGroundAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string HiddenAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string EmergeAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StunnedAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StunnedResetAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string JumpAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string JumpAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string HowlAnimation;
  [SerializeField]
  public GameObject lighting;
  public SpriteRenderer ShadowSpriteRenderer;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public float NumberOfAttacks = 1f;
  public float KnockbackModifier = 1f;
  public float AttackForceModifier = 1f;
  public bool CounterAttack;
  public bool CanBeInterrupted = true;
  public bool AttackTowardsPlayer;
  public float DamageColliderDuration = -1f;
  public bool DisableKnockback;
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer;
  public int DistanceToPathTowardsPlayer = 6;
  public SkeletonAnimation warningIcon;
  public GameObject TrailPrefab;
  public List<GameObject> Trails = new List<GameObject>();
  public float DelayBetweenTrails = 0.2f;
  public float TrailsTimer;
  public GameObject lastTrailSegment;
  public Vector3 previousTrailsSpawnPosition;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic/warning";
  [EventRef]
  public string HowlVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_basic_small/howl";
  [EventRef]
  public string AttackSFX = "event:/enemy/chaser/chaser_attack";
  [EventRef]
  public string AttackChargeSFX = "event:/enemy/chaser/chaser_charge";
  [EventRef]
  public string AttackBurrowInJumpSFX = "event:/dlc/dungeon05/enemy/dog_burrow/attack_burrow_in_jump";
  [EventRef]
  public string AttackBurronInGroundSFX = "event:/dlc/dungeon05/enemy/dog_burrow/attack_burrow_in_ground";
  [EventRef]
  public string AttackBurrowOutSFX = "event:/dlc/dungeon05/enemy/dog_burrow/attack_burrow_out";
  [EventRef]
  public string AttackBurrowOutLandSFX = "event:/dlc/dungeon05/enemy/dog_burrow/attack_burrow_out_land";
  [EventRef]
  public string AttackBurrowLoopSFX = "event:/enemy/tunnel_worm/tunnel_worm_underground_loop";
  [EventRef]
  public string HopSFX = "event:/dlc/dungeon05/enemy/dog_burrow/mv_hop";
  [EventRef]
  public string AttackJumpLaunchSFX = "event:/dlc/dungeon05/enemy/dog_exploding/attack_jump_launch";
  [EventRef]
  public string AttackJumpLandSFX = "event:/dlc/dungeon05/enemy/dog_exploding/attack_jump_land";
  public bool Melee = true;
  public bool Burrow = true;
  public bool DiveBomb = true;
  public float MeleeAttackCooldown;
  [HideInInspector]
  public float meleeCooldownTimer;
  public float AttackDuration = 1f;
  public float SignPostAttackDuration = 0.2f;
  public float MeleeAttackRange = 3f;
  public float BurrowDelay = 2f;
  public float BurrowDuration = 4f;
  public float BurrowMoveSpeed = 3f;
  public int BurrowAttackCount = 2;
  public float BurrowStunnedDuration = 3f;
  public float hopMoveSpeed = 1f;
  public AnimationCurve hopSpeedCurve;
  public AnimationCurve hopZCurve;
  public float hopZHeight = 3f;
  public float hoppingDuration = 0.4f;
  public float delayBeforeAttack = 0.3f;
  public float emergeZHeight;
  public float emergeDuration = 0.4f;
  public ParticleSystem SnowParticles;
  public float DiveAttackRange = 3f;
  public float DiveBombDuration = 6f;
  public float DiveBombArcHeight = 4f;
  public float ExplosionRadius = 2f;
  public ParticleSystem AoEParticles;
  public float KnockbackTargetDistance = 10f;
  public float KnockbackTargetAngle = 55f;
  [SerializeField]
  public LayerMask lockOnMask;
  public GameObject DeathBombPrefab;
  public int DeathBombsToFire = 3;
  public float DeathBombSpread = 2f;
  public float DeathMinBombRange = 2.5f;
  public float DeathMaxBombRange = 6f;
  public float DeathBombSpeed = 8f;
  public Vector3 TargetPosition;
  public Vector2 MaxMoveDistance;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public Vector2 IdleWaitRange = new Vector2(1f, 3f);
  public float MinimumPlayerDistance = 3f;
  public float TurningArc = 90f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  public ShowHPBar ShowHPBar;
  public EventInstance loopingSoundInstance;
  public bool IsAttacking;
  public bool IsDiveBombing;
  public bool IsStunned;
  public bool IsBurrowing;
  public bool hasCollidedWithObstacle;
  public Vector3 previousSpawnPosition;
  public bool PathingToPlayer;
  public float initialDelay;
  public float IdleWait;
  public float RandomDirection;
  public float GravitySpeed = 1f;
  public float HidingHeight = 5f;
  public float Angle;
  public Vector3 Force;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShownWarning;
  public int currentBurrowAttackCount;
  public float currentBurrowDelay;
  public float delayBeforeRepeatBurrow = 0.2f;
  public float knockbackGravity = 6f;
  public bool isKnockedTowardsEnemy;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BackStunnedAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BackStunnedResetAnimation;
  public float BackStunnedDuration = 2.25f;
  public bool EnableBackStun = true;
  public int BackStunComboTarget = 1;
  [Range(0.0f, 1f)]
  public float BackStunChance = 1f;
  public float BackStunHealthThreshold = 0.5f;
  public bool SkipHowlRoutines;
  public float BaseAccuracy = 1.2f;
  public float AccuracyGrowthRate = 0.1f;
  public float BaseSpeed = 1.2f;
  public float SpeedGrowthRate = 0.05f;
  public float MaxTimeToExplode = 30f;
  public float timer;
  public bool timerStarted;
  public float DetectionDelay = 0.1f;
  public bool isDetectionDelayActive;
  public BombTimerUI bombTimerUI;
  public GameObject bombFuseParticles;
  public float lastHitTime;
  public int backStunCombo;

  public virtual bool ShouldMeleeAttack()
  {
    return !this.IsAttacking && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.MeleeAttackRange && GameManager.RoomActive && EnemyDog.Dogs.IndexOf(this) == EnemyDog.CurrentAttackerIndex;
  }

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.ShowHPBar = this.GetComponent<ShowHPBar>();
    EnemyDog.Dogs.Add(this);
    this.RandomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.IdleWait = 0.0f;
    this.IsAttacking = false;
    this.IsStunned = false;
    this.health.invincible = false;
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyDog.Dogs.Remove(this);
    if (EnemyDog.Dogs.Count > 0 && EnemyDog.CurrentAttackerIndex >= EnemyDog.Dogs.Count)
      EnemyDog.CurrentAttackerIndex = 0;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.Spine.transform.localPosition = Vector3.zero;
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
  }

  public void ShowWarningIcon(float duration = 2f)
  {
    if ((UnityEngine.Object) this.warningIcon == (UnityEngine.Object) null || this.ShownWarning)
      return;
    this.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
    this.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, duration);
    this.ShownWarning = true;
    if (string.IsNullOrEmpty(this.WarningVO))
      return;
    AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
  }

  public IEnumerator BackStunnedRoutine()
  {
    EnemyDog enemyDog = this;
    enemyDog.IsAttacking = false;
    enemyDog.ClearPaths();
    enemyDog.DisableForces = false;
    enemyDog.CanBeInterrupted = false;
    enemyDog.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    if (!string.IsNullOrEmpty(enemyDog.BackStunnedAnimation) && !string.IsNullOrEmpty(enemyDog.BackStunnedResetAnimation))
    {
      enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.BackStunnedAnimation, false);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/dog_pack/mv_knockdown_start", enemyDog.gameObject);
      float t = 0.0f;
      while ((double) (t += Time.deltaTime * enemyDog.Spine.timeScale) < (double) enemyDog.BackStunnedDuration)
        yield return (object) null;
      enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.BackStunnedResetAnimation, false);
      enemyDog.Spine.AnimationState.AddAnimation(0, enemyDog.IdleAnimation, true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/dog_pack/mv_knockdown_getup", enemyDog.gameObject);
      Spine.Animation animation = enemyDog.Spine.Skeleton.Data.FindAnimation(enemyDog.BackStunnedResetAnimation);
      if (animation != null)
        yield return (object) new WaitForSeconds(animation.Duration * enemyDog.Spine.timeScale);
    }
    enemyDog.CanBeInterrupted = true;
    enemyDog.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDog.IdleWait = 0.0f;
    enemyDog.StartCoroutine((IEnumerator) enemyDog.ActiveRoutine());
  }

  public virtual IEnumerator ActiveRoutine()
  {
    EnemyDog enemyDog = this;
    enemyDog.health.invincible = false;
    enemyDog.DisableKnockback = false;
    enemyDog.CanBeInterrupted = true;
    enemyDog.IsAttacking = false;
    enemyDog.IsDiveBombing = false;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while ((double) enemyDog.Spine.timeScale != 9.9999997473787516E-05)
      {
        if (enemyDog.state.CURRENT_STATE == StateMachine.State.Idle && (double) (enemyDog.IdleWait -= Time.deltaTime) <= 0.0)
        {
          enemyDog.GetNewTargetPosition();
          if ((UnityEngine.Object) enemyDog.GetClosestTarget() != (UnityEngine.Object) null && (UnityEngine.Object) enemyDog.bombTimerUI != (UnityEngine.Object) null && !enemyDog.bombTimerUI.gameObject.activeSelf)
          {
            enemyDog.bombTimerUI?.SetTimerValue(1f);
            enemyDog.bombFuseParticles.SetActive(true);
            enemyDog.StartCoroutine((IEnumerator) enemyDog.TimerRoutine());
          }
        }
        if ((UnityEngine.Object) enemyDog.GetClosestTarget() != (UnityEngine.Object) null && !enemyDog.IsAttacking && !enemyDog.IsStunned && GameManager.RoomActive)
          enemyDog.state.LookAngle = Utils.GetAngle(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position);
        else
          enemyDog.state.LookAngle = enemyDog.state.facingAngle;
        if (enemyDog.MovingAnimation != "")
        {
          if (enemyDog.state.CURRENT_STATE == StateMachine.State.Moving && enemyDog.Spine.AnimationName != enemyDog.MovingAnimation)
            enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.MovingAnimation, true);
          if (enemyDog.state.CURRENT_STATE == StateMachine.State.Idle && enemyDog.Spine.AnimationName != enemyDog.IdleAnimation)
            enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.IdleAnimation, true);
        }
        if ((UnityEngine.Object) enemyDog.GetClosestTarget() == (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) enemyDog.GetClosestTarget() != (UnityEngine.Object) null && enemyDog.IsTargetVisible())
            enemyDog.ShowWarningIcon();
          else
            yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.DoHowlRoutine());
        }
        else
          yield return (object) enemyDog.DetermineMeleeType();
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public IEnumerator DetermineMeleeType()
  {
    EnemyDog enemyDog = this;
    if (EnemyDog.Dogs.IndexOf(enemyDog) == EnemyDog.CurrentAttackerIndex && (double) Vector3.Distance(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position) > (double) enemyDog.MeleeAttackRange * 3.0)
      EnemyDog.CurrentAttackerIndex = EnemyDog.CurrentAttackerIndex = UnityEngine.Random.Range(0, EnemyDog.Dogs.Count);
    if (enemyDog.Melee && enemyDog.IsTargetWithinMeleeRange())
    {
      if (enemyDog.ShouldMeleeAttack())
        yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.MeleeAttackRoutine());
    }
    else if (enemyDog.Burrow && enemyDog.ShouldBurrowAttack())
    {
      enemyDog.currentBurrowDelay = enemyDog.BurrowDelay;
      enemyDog.currentBurrowAttackCount = enemyDog.BurrowAttackCount;
      yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.BurrowAttackRoutine());
    }
    else if (enemyDog.DiveBomb && enemyDog.ShouldDiveBomb())
      yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.DiveBombRoutine(1f, 1f));
  }

  public IEnumerator DoHowlRoutine()
  {
    EnemyDog enemyDog = this;
    if (!enemyDog.SkipHowlRoutines)
    {
      enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.HowlAnimation, true);
      enemyDog.Spine.AnimationState.AddAnimation(0, enemyDog.IdleAnimation, false, 0.0f);
      if (!string.IsNullOrEmpty(enemyDog.HowlVO))
        AudioManager.Instance.PlayOneShot(enemyDog.HowlVO, enemyDog.transform.position);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(2f, 4f));
    }
  }

  public bool IsTargetVisible()
  {
    return (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.VisionRange && GameManager.RoomActive;
  }

  public bool IsTargetWithinMeleeRange()
  {
    return (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.MeleeAttackRange && GameManager.RoomActive;
  }

  public virtual bool ShouldBurrowAttack()
  {
    return (double) (this.currentBurrowDelay -= Time.deltaTime) < 0.0 && !this.IsBurrowing && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.VisionRange && GameManager.RoomActive;
  }

  public virtual bool ShouldDiveBomb()
  {
    return !this.IsDiveBombing && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.DiveAttackRange && GameManager.RoomActive;
  }

  public virtual IEnumerator MeleeAttackRoutine()
  {
    EnemyDog enemyDog = this;
    enemyDog.IsAttacking = true;
    enemyDog.ClearPaths();
    int CurrentAttack = 0;
    float time = 0.0f;
    while ((double) ++CurrentAttack <= (double) enemyDog.NumberOfAttacks)
    {
      enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.SignPostAttackAnimation, enemyDog.LoopSignPostAttackAnimation);
      enemyDog.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      if (!string.IsNullOrEmpty(enemyDog.AttackChargeSFX))
        AudioManager.Instance.PlayOneShot(enemyDog.AttackChargeSFX, enemyDog.transform.position);
      if ((UnityEngine.Object) enemyDog.GetClosestTarget() != (UnityEngine.Object) null)
      {
        enemyDog.state.LookAngle = Utils.GetAngle(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position);
        enemyDog.state.facingAngle = enemyDog.state.LookAngle;
      }
      float Progress = 0.0f;
      float Duration = enemyDog.SignPostAttackDuration;
      while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemyDog.Spine.timeScale)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in enemyDog.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(Progress / Duration);
        yield return (object) null;
      }
      foreach (SimpleSpineFlash simpleSpineFlash in enemyDog.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      if (enemyDog.AttackTowardsPlayer)
      {
        if ((UnityEngine.Object) enemyDog.GetClosestTarget() != (UnityEngine.Object) null)
        {
          enemyDog.state.LookAngle = Utils.GetAngle(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position);
          enemyDog.state.facingAngle = enemyDog.state.LookAngle;
        }
        enemyDog.DoKnockBack(enemyDog.GetClosestTarget().gameObject, -1f, 1f);
      }
      else
      {
        enemyDog.DisableForces = true;
        enemyDog.Force = (Vector3) (new Vector2(2500f * Mathf.Cos(enemyDog.state.LookAngle * ((float) Math.PI / 180f)), 2500f * Mathf.Sin(enemyDog.state.LookAngle * ((float) Math.PI / 180f))) * enemyDog.AttackForceModifier);
        enemyDog.rb.AddForce((Vector2) enemyDog.Force);
      }
      enemyDog.damageColliderEvents.SetActive(true);
      if (!string.IsNullOrEmpty(enemyDog.AttackVO))
        AudioManager.Instance.PlayOneShot(enemyDog.AttackVO, enemyDog.transform.position);
      if (!string.IsNullOrEmpty(enemyDog.AttackSFX))
        AudioManager.Instance.PlayOneShot(enemyDog.AttackSFX, enemyDog.transform.position);
      enemyDog.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
      enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.AttackAnimation, false);
      enemyDog.Spine.AnimationState.AddAnimation(0, enemyDog.IdleAnimation, true, 0.0f);
      if ((double) enemyDog.DamageColliderDuration != -1.0)
        enemyDog.StartCoroutine((IEnumerator) enemyDog.EnableCollider(enemyDog.DamageColliderDuration));
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyDog.Spine.timeScale) < (double) enemyDog.AttackDuration * 0.699999988079071)
        yield return (object) null;
      enemyDog.damageColliderEvents.SetActive(false);
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDog.Spine.timeScale) < (double) enemyDog.AttackDuration * 0.30000001192092896)
      yield return (object) null;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyDog.MeleeAttackCooldown, enemyDog.Spine);
    enemyDog.DisableForces = false;
    enemyDog.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDog.IdleWait = 0.0f;
    enemyDog.meleeCooldownTimer = enemyDog.MeleeAttackCooldown;
    enemyDog.IsAttacking = false;
    EnemyDog.CurrentAttackerIndex = (EnemyDog.CurrentAttackerIndex + 1) % EnemyDog.Dogs.Count;
  }

  public IEnumerator BurrowAttackRoutine()
  {
    EnemyDog enemyDog = this;
    while (enemyDog.currentBurrowAttackCount > 0)
    {
      --enemyDog.currentBurrowAttackCount;
      yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.DoDiveIntoGround());
      yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.DoBurrowChase());
      if (GameManager.RoomActive)
      {
        yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.PauseBeforeEmergeAttack());
        yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.EmergeAttack());
        if (enemyDog.currentBurrowAttackCount <= 0)
          yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.StunnedRoutine());
        else
          yield return (object) new WaitForSeconds(enemyDog.delayBeforeRepeatBurrow);
      }
    }
  }

  public IEnumerator DoDiveIntoGround()
  {
    EnemyDog enemyDog = this;
    if (!string.IsNullOrEmpty(enemyDog.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackVO, enemyDog.gameObject);
    float progress = 0.0f;
    enemyDog.DisableKnockback = true;
    enemyDog.CanBeInterrupted = false;
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.DiveIntoGroundAnimation, false);
    enemyDog.Spine.AnimationState.AddAnimation(0, enemyDog.HiddenAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyDog.AttackBurrowInJumpSFX))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackBurrowInJumpSFX, enemyDog.transform.position);
    while ((double) (progress += Time.deltaTime * enemyDog.Spine.timeScale) < (double) enemyDog.hoppingDuration)
    {
      enemyDog.speed = enemyDog.hopSpeedCurve.Evaluate(progress / (enemyDog.hoppingDuration * enemyDog.Spine.timeScale)) * enemyDog.hopMoveSpeed;
      if (enemyDog.hasCollidedWithObstacle)
        enemyDog.speed *= 0.5f;
      enemyDog.speed *= enemyDog.Spine.timeScale;
      enemyDog.Spine.transform.localPosition = -Vector3.forward * enemyDog.hopZCurve.Evaluate(progress / enemyDog.hoppingDuration) * enemyDog.hopZHeight * enemyDog.Spine.timeScale;
      yield return (object) null;
    }
    if ((double) progress >= (double) enemyDog.hoppingDuration / (double) enemyDog.Spine.timeScale)
    {
      enemyDog.speed = 0.0f;
      enemyDog.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    enemyDog.DisableKnockback = false;
    enemyDog.CanBeInterrupted = true;
    enemyDog.Spine.transform.localPosition = Vector3.zero;
  }

  public void DoDiveImpactEffects()
  {
    if ((bool) (UnityEngine.Object) this.lighting)
      this.lighting.SetActive(false);
    if ((bool) (UnityEngine.Object) this.ShadowSpriteRenderer)
      this.ShadowSpriteRenderer.enabled = false;
    this.SpawnTrails();
    this.SnowParticles.Play();
  }

  public IEnumerator DoBurrowChase()
  {
    EnemyDog enemyDog = this;
    enemyDog.health.invincible = true;
    enemyDog.emitDustClouds = false;
    enemyDog.TargetPosition = enemyDog.GetClosestTarget().transform.position;
    float time = 0.0f;
    float progress = 0.0f;
    float duration = 0.366666675f;
    enemyDog.transform.position.z = 0.0f;
    enemyDog.TargetPosition.z = 0.0f;
    progress = 0.0f;
    if (!string.IsNullOrEmpty(enemyDog.AttackBurronInGroundSFX))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackBurronInGroundSFX, enemyDog.gameObject);
    while ((double) (time += Time.deltaTime * enemyDog.Spine.timeScale) < 0.25)
      yield return (object) null;
    while ((double) (progress += Time.deltaTime * enemyDog.Spine.timeScale) < (double) duration)
    {
      enemyDog.SpawnTrails();
      yield return (object) null;
    }
    enemyDog.ShowHPBar?.Hide();
    enemyDog.state.CURRENT_STATE = StateMachine.State.Fleeing;
    if (!enemyDog.loopingSoundInstance.isValid())
      enemyDog.loopingSoundInstance = AudioManager.Instance.CreateLoop(enemyDog.AttackBurrowLoopSFX, enemyDog.gameObject, true);
    enemyDog.previousSpawnPosition = Vector3.positiveInfinity;
    while ((double) (progress += Time.deltaTime * enemyDog.Spine.timeScale) < (double) enemyDog.BurrowDuration)
    {
      Health closestTarget = enemyDog.GetClosestTarget();
      if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
      {
        enemyDog.TargetPosition = closestTarget.transform.position;
        if ((double) Vector3.Distance(enemyDog.transform.position, enemyDog.TargetPosition) >= 0.5)
        {
          enemyDog.transform.position = Vector3.MoveTowards(enemyDog.transform.position, enemyDog.TargetPosition, enemyDog.BurrowMoveSpeed * Time.deltaTime);
          enemyDog.SpawnTrails(true);
          yield return (object) null;
        }
        else
          break;
      }
      else
        break;
    }
    enemyDog.health.invincible = false;
    AudioManager.Instance.StopLoop(enemyDog.loopingSoundInstance);
    if (!string.IsNullOrEmpty(enemyDog.AttackBurrowOutSFX))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackBurrowOutSFX, enemyDog.gameObject);
    enemyDog.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDog.IsBurrowing = false;
    enemyDog.emitDustClouds = true;
  }

  public void SpawnTrails(bool enableDamageColliders = false)
  {
    if ((double) (this.TrailsTimer += Time.deltaTime) <= (double) this.DelayBetweenTrails || (double) Vector3.Distance(this.transform.position, this.previousTrailsSpawnPosition) <= 0.10000000149011612)
      return;
    this.TrailsTimer = 0.0f;
    this.lastTrailSegment = (GameObject) null;
    if (this.Trails.Count > 0)
    {
      foreach (GameObject trail in this.Trails)
      {
        if (!trail.activeSelf)
        {
          this.lastTrailSegment = trail;
          this.lastTrailSegment.transform.position = this.transform.position;
          this.lastTrailSegment.SetActive(true);
          break;
        }
      }
    }
    if ((UnityEngine.Object) this.lastTrailSegment == (UnityEngine.Object) null)
    {
      this.lastTrailSegment = UnityEngine.Object.Instantiate<GameObject>(this.TrailPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      this.Trails.Add(this.lastTrailSegment);
      if (enableDamageColliders)
      {
        ColliderEvents componentInChildren = this.lastTrailSegment.GetComponentInChildren<ColliderEvents>();
        if ((bool) (UnityEngine.Object) componentInChildren)
          componentInChildren.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      }
    }
    this.previousTrailsSpawnPosition = this.lastTrailSegment.transform.position;
  }

  public IEnumerator PauseBeforeEmergeAttack()
  {
    EnemyDog enemyDog = this;
    enemyDog.health.invincible = true;
    enemyDog.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDog.ShowWarningIcon(enemyDog.delayBeforeAttack);
    float t = 0.0f;
    while ((double) (t += Time.deltaTime * enemyDog.Spine.timeScale) < (double) enemyDog.delayBeforeAttack)
      yield return (object) null;
    enemyDog.health.invincible = false;
  }

  public IEnumerator EmergeAttack()
  {
    EnemyDog enemyDog = this;
    enemyDog.DisableKnockback = true;
    enemyDog.CanBeInterrupted = false;
    if ((bool) (UnityEngine.Object) enemyDog.lighting)
      enemyDog.lighting.SetActive(true);
    if ((bool) (UnityEngine.Object) enemyDog.ShadowSpriteRenderer)
      enemyDog.ShadowSpriteRenderer.enabled = true;
    enemyDog.SnowParticles.Play();
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.EmergeAnimation, false);
    if (!string.IsNullOrEmpty(enemyDog.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackVO, enemyDog.gameObject);
    Vector3 vector3 = enemyDog.Spine.transform.localPosition + Vector3.back * enemyDog.emergeZHeight;
    enemyDog.Spine.transform.DOKill();
    enemyDog.Spine.transform.DOMoveZ(-enemyDog.emergeZHeight, enemyDog.emergeDuration * enemyDog.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic);
    enemyDog.damageColliderEvents.SetActive(true);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    yield return (object) new WaitForSeconds(enemyDog.emergeDuration * enemyDog.Spine.timeScale);
    enemyDog.Spine.transform.DOKill();
    enemyDog.Spine.transform.DOMoveZ(0.0f, enemyDog.emergeDuration * enemyDog.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCubic);
    yield return (object) new WaitForSeconds(enemyDog.emergeDuration * enemyDog.Spine.timeScale);
    if (!string.IsNullOrEmpty(enemyDog.AttackBurrowOutLandSFX))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackBurrowOutLandSFX, enemyDog.gameObject);
    enemyDog.damageColliderEvents.SetActive(false);
    enemyDog.DisableKnockback = false;
    enemyDog.CanBeInterrupted = true;
  }

  public IEnumerator StunnedRoutine()
  {
    EnemyDog enemyDog = this;
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.StunnedAnimation, true);
    enemyDog.state.CURRENT_STATE = StateMachine.State.Idle;
    float t = 0.0f;
    while ((double) (t += Time.deltaTime * enemyDog.Spine.timeScale) < (double) enemyDog.BurrowStunnedDuration)
      yield return (object) null;
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.StunnedResetAnimation, false);
    enemyDog.Spine.AnimationState.AddAnimation(0, enemyDog.IdleAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyDog.HopSFX))
      AudioManager.Instance.PlayOneShot(enemyDog.HopSFX, enemyDog.transform.position);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyDog.Spine.Skeleton.Data.FindAnimation(enemyDog.StunnedResetAnimation).Duration, enemyDog.Spine);
  }

  public IEnumerator DiveBombRoutine(float anticpationMultiplier, float explosionRadiusMultiplier)
  {
    EnemyDog enemyDog = this;
    enemyDog.IsDiveBombing = true;
    enemyDog.ClearPaths();
    enemyDog.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.JumpAnticipationAnimation, false);
    float anticipationDuration = enemyDog.Spine.Skeleton.Data.FindAnimation(enemyDog.JumpAnticipationAnimation).Duration * enemyDog.Spine.timeScale;
    float signpostTime = 0.0f;
    if (!string.IsNullOrEmpty(enemyDog.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyDog.WarningVO, enemyDog.transform.position);
    while ((double) enemyDog.Spine.timeScale == 9.9999997473787516E-05)
      yield return (object) null;
    while ((double) (signpostTime += Time.deltaTime * enemyDog.Spine.timeScale) < (double) anticipationDuration * (double) anticpationMultiplier)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in enemyDog.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(signpostTime / (anticipationDuration * anticpationMultiplier));
      enemyDog.state.LookAngle = Utils.GetAngle(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position);
      enemyDog.state.facingAngle = Utils.GetAngle(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemyDog.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    enemyDog.state.LookAngle = Utils.GetAngle(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position);
    enemyDog.state.facingAngle = Utils.GetAngle(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position);
    enemyDog.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (!string.IsNullOrEmpty(enemyDog.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackVO, enemyDog.transform.position);
    if (!string.IsNullOrEmpty(enemyDog.AttackJumpLaunchSFX))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackJumpLaunchSFX, enemyDog.transform.position);
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.JumpAnimation, false);
    Vector3 startPosition = enemyDog.transform.position;
    float num1 = enemyDog.BaseAccuracy + enemyDog.AccuracyGrowthRate * (enemyDog.MaxTimeToExplode - enemyDog.timer);
    Vector3 targetPosition = Vector3.Lerp(enemyDog.GetClosestTarget().transform.position, enemyDog.transform.position, Mathf.Clamp01(1f / num1));
    float num2 = enemyDog.BaseSpeed + enemyDog.SpeedGrowthRate * (enemyDog.MaxTimeToExplode - enemyDog.timer);
    float adjustedDiveDuration = enemyDog.DiveBombDuration / num2;
    float progress = 0.0f;
    Vector3 curve = startPosition + (targetPosition - startPosition) / 2f + Vector3.back * enemyDog.DiveBombArcHeight;
    enemyDog.LockToGround = false;
    while ((double) (progress += Time.deltaTime * enemyDog.Spine.timeScale) < (double) adjustedDiveDuration)
    {
      Vector3 a = Vector3.Lerp(startPosition, curve, progress / adjustedDiveDuration);
      Vector3 b = Vector3.Lerp(curve, targetPosition, progress / adjustedDiveDuration);
      enemyDog.transform.position = Vector3.Lerp(a, b, progress / adjustedDiveDuration);
      yield return (object) null;
    }
    targetPosition.z = 0.0f;
    enemyDog.transform.position = targetPosition;
    if ((double) Vector3.Distance(enemyDog.transform.position, enemyDog.GetClosestTarget().transform.position) <= (double) enemyDog.ExplosionRadius * (double) explosionRadiusMultiplier)
      enemyDog.Explode();
    else
      yield return (object) enemyDog.StartCoroutine((IEnumerator) enemyDog.MissRecoveryRoutine());
    enemyDog.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDog.IsDiveBombing = false;
    enemyDog.LockToGround = true;
  }

  public IEnumerator TimerRoutine()
  {
    this.timer = 0.0f;
    while ((double) this.timer < (double) this.MaxTimeToExplode)
    {
      this.timer += Time.deltaTime;
      this.bombTimerUI?.SetTimerValue((this.MaxTimeToExplode - this.timer) / this.MaxTimeToExplode);
      yield return (object) null;
    }
    this.Explode();
  }

  public void Explode()
  {
    this.Spine.AnimationState.SetAnimation(0, this.LandAnimation, false);
    if (!string.IsNullOrEmpty(this.AttackJumpLandSFX))
      AudioManager.Instance.PlayOneShot(this.AttackJumpLandSFX, this.transform.position);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    this.AoEParticles.Play();
    this.health.DealDamage(float.PositiveInfinity, PlayerFarming.Instance.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
    this.StopCoroutine((IEnumerator) this.TimerRoutine());
    this.timerStarted = false;
    this.bombTimerUI?.SetTimerValue(0.0f);
  }

  public IEnumerator MissRecoveryRoutine()
  {
    EnemyDog enemyDog = this;
    if (!string.IsNullOrEmpty(enemyDog.AttackJumpLandSFX))
      AudioManager.Instance.PlayOneShot(enemyDog.AttackJumpLandSFX, enemyDog.transform.position);
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.StunnedAnimation, false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyDog.Spine);
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.IdleAnimation, true);
  }

  public IEnumerator EnableCollider(float dur)
  {
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) dur)
      yield return (object) null;
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnDieEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.LockToGround)
      this.health.BloodOnDie = false;
    base.OnDieEarly(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    if (this.DiveBomb)
    {
      Explosion.CreateExplosion(this.transform.position, Health.Team.KillAll, this.health, this.ExplosionRadius);
      if ((UnityEngine.Object) this.DeathBombPrefab != (UnityEngine.Object) null && this.DeathBombsToFire > 0)
      {
        Transform transform = (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null ? this.GetClosestTarget().transform : (Transform) null;
        Vector3 vector3_1 = (UnityEngine.Object) transform != (UnityEngine.Object) null ? transform.position : this.transform.position;
        for (int index = 0; index < this.DeathBombsToFire; ++index)
        {
          Vector3 vector3_2 = vector3_1;
          if (this.DeathBombsToFire > 1)
            vector3_2 += (Vector3) UnityEngine.Random.insideUnitCircle * this.DeathBombSpread;
          MortarBomb component = UnityEngine.Object.Instantiate<GameObject>(this.DeathBombPrefab, vector3_2, Quaternion.identity, this.transform.parent).GetComponent<MortarBomb>();
          Vector3 normalized = (vector3_2 - this.transform.position).normalized;
          double num1 = (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) vector3_2);
          Vector3 vector3_3 = (this.transform.position + normalized * UnityEngine.Random.Range(this.DeathMinBombRange, this.DeathMaxBombRange)) with
          {
            z = 0.0f
          };
          component.transform.position = vector3_3;
          double num2 = (double) Mathf.Max(0.01f, Vector2.Distance((Vector2) this.transform.position, (Vector2) component.transform.position) / Mathf.Max(0.01f, this.DeathBombSpeed));
          component.Play(this.transform.position + new Vector3(0.0f, 0.0f, -1.5f), UnityEngine.Random.Range(1f, 2f), this.health.team);
        }
      }
    }
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if (!this.DisableKnockback)
      this.damageColliderEvents.SetActive(false);
    if (this.CanBeInterrupted && AttackType != Health.AttackTypes.NoReaction)
    {
      this.StopAllCoroutines();
      this.DisableForces = false;
      if ((double) this.lastHitTime == 0.0 || (double) Time.time - (double) this.lastHitTime < 1.0)
        ++this.backStunCombo;
      else
        this.backStunCombo = 0;
      this.lastHitTime = Time.time;
      if ((1.0 / (double) this.health.totalHP * (double) this.health.HP >= (double) this.BackStunHealthThreshold || !this.EnableBackStun ? 0 : ((double) this.transform.position.z > -0.02500000037252903 ? 1 : 0)) != 0 && !string.IsNullOrEmpty(this.BackStunnedAnimation) && !string.IsNullOrEmpty(this.BackStunnedResetAnimation))
      {
        this.BackStunHealthThreshold = 0.0f;
        this.StartCoroutine((IEnumerator) this.ApplyBackstunForceRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), 25f));
        this.StartCoroutine((IEnumerator) this.BackStunnedRoutine());
      }
      else
        this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (this.DiveBomb && AttackType != Health.AttackTypes.NoKnockBack && AttackType != Health.AttackTypes.NoReaction && !this.DisableKnockback && this.CanBeInterrupted && (UnityEngine.Object) Attacker != (UnityEngine.Object) null)
      this.StartCoroutine((IEnumerator) this.DiveBombRoutine(0.25f, float.MaxValue));
    else if (AttackType != Health.AttackTypes.NoKnockBack && AttackType != Health.AttackTypes.NoReaction && !this.DisableKnockback && this.CanBeInterrupted)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f)));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  public IEnumerator KnockTowardsEnemy(GameObject Attacker)
  {
    EnemyDog enemyDog = this;
    if (enemyDog.isKnockedTowardsEnemy)
      enemyDog.StopCoroutine((IEnumerator) enemyDog.KnockTowardsEnemy(Attacker));
    enemyDog.isKnockedTowardsEnemy = true;
    Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2) enemyDog.transform.position, enemyDog.KnockbackTargetDistance, (int) enemyDog.lockOnMask);
    yield return (object) null;
    Vector3 vector3 = enemyDog.transform.position - Attacker.transform.position;
    Vector3 normalized1 = vector3.normalized;
    Collider2D collider2D1 = (Collider2D) null;
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (SimpleSpineFlash simpleSpineFlash in enemyDog.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
    if (colliders.Length != 0)
    {
      foreach (Collider2D collider2D2 in colliders)
      {
        if (!((UnityEngine.Object) collider2D2 == (UnityEngine.Object) null) && !((UnityEngine.Object) collider2D2.gameObject == (UnityEngine.Object) null))
        {
          vector3 = enemyDog.transform.position - collider2D2.transform.position;
          Vector3 normalized2 = vector3.normalized;
          float f = Mathf.Abs(Vector3.Angle(normalized1, normalized2) - 180f);
          bool flag = (double) f < (double) enemyDog.KnockbackTargetAngle;
          if (flag)
          {
            Health component = collider2D2.GetComponent<Health>();
            if (collider2D2.gameObject.name == "Enemy Winter Dog Miniboss")
            {
              num1 = component.HP;
              collider2D1 = collider2D2;
              num2 = f;
            }
            if (((UnityEngine.Object) collider2D1 == (UnityEngine.Object) null || (double) Mathf.Abs(f) < (double) num2) && flag && (bool) (UnityEngine.Object) component && component.team == Health.Team.Team2 && (UnityEngine.Object) collider2D2.gameObject != (UnityEngine.Object) enemyDog.gameObject && (double) component.HP >= (double) num1)
            {
              num1 = component.HP;
              collider2D1 = collider2D2;
              num2 = f;
            }
          }
        }
      }
    }
    Collider2D collider2D3 = collider2D1;
    if ((UnityEngine.Object) collider2D3 != (UnityEngine.Object) null && (UnityEngine.Object) collider2D3.gameObject != (UnityEngine.Object) enemyDog.gameObject && (double) enemyDog.MagnitudeFindDistanceBetween(collider2D3.transform.position, enemyDog.transform.position) < (double) enemyDog.KnockbackTargetDistance * (double) enemyDog.KnockbackTargetDistance)
    {
      float angle = Utils.GetAngle(enemyDog.transform.position, collider2D3.transform.position) * ((float) Math.PI / 180f);
      enemyDog.StartCoroutine((IEnumerator) enemyDog.ApplyForceRoutine(angle));
    }
    else
    {
      float angle = Utils.GetAngle(Attacker.transform.position, enemyDog.transform.position) * ((float) Math.PI / 180f);
      enemyDog.StartCoroutine((IEnumerator) enemyDog.ApplyForceRoutine(angle));
    }
    enemyDog.ClearPaths();
    enemyDog.isKnockedTowardsEnemy = false;
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  public IEnumerator ApplyBackstunForceRoutine(float angle, float localMultiplier = 1f)
  {
    EnemyDog enemyDog = this;
    enemyDog.DisableForces = true;
    float timer = 0.0f;
    Vector2 Force = new Vector2(1500f * Mathf.Cos(angle), 1500f * Mathf.Sin(angle)) * localMultiplier;
    while ((double) timer < 0.25)
    {
      enemyDog.rb.AddForce(Force * Time.deltaTime);
      timer += Time.deltaTime;
      yield return (object) null;
    }
    enemyDog.DisableForces = false;
  }

  public IEnumerator ApplyForceRoutine(float angle, float localMultiplier = 1f)
  {
    EnemyDog enemyDog = this;
    enemyDog.Angle = angle;
    enemyDog.DisableForces = true;
    enemyDog.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyDog.Angle), 1500f * Mathf.Sin(enemyDog.Angle)) * enemyDog.KnockbackModifier * localMultiplier);
    enemyDog.rb.AddForce((Vector2) enemyDog.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDog.Spine.timeScale) < 0.5)
    {
      if ((double) enemyDog.transform.position.z < 0.0)
        enemyDog.transform.position += Vector3.forward * enemyDog.knockbackGravity * Time.deltaTime;
      yield return (object) null;
    }
    enemyDog.transform.position = new Vector3(enemyDog.transform.position.x, enemyDog.transform.position.y, 0.0f);
    if (enemyDog.DiveBomb)
      enemyDog.health.DealDamage(float.PositiveInfinity, PlayerFarming.Instance.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
    enemyDog.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemyDog enemyDog = this;
    enemyDog.damageColliderEvents.SetActive(false);
    enemyDog.IsAttacking = false;
    enemyDog.ClearPaths();
    enemyDog.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyDog.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDog.Spine.AnimationState.SetAnimation(0, enemyDog.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDog.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyDog.DisableForces = false;
    enemyDog.IdleWait = 0.0f;
    enemyDog.StartCoroutine((IEnumerator) enemyDog.ActiveRoutine());
    if (enemyDog.CounterAttack)
      enemyDog.StartCoroutine((IEnumerator) enemyDog.MeleeAttackRoutine());
  }

  public void GetNewTargetPosition()
  {
    float num1 = 100f;
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, closestTarget.transform.position) < (double) this.DistanceToPathTowardsPlayer && this.CheckLineOfSightOnTarget(closestTarget.gameObject, closestTarget.transform.position, (float) this.DistanceToPathTowardsPlayer))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, closestTarget.transform.position) * ((float) Math.PI / 180f);
    }
    if ((double) this.ChanceToPathTowardsPlayer >= 1.0 && GameManager.RoomActive && (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
    {
      float num2 = Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position);
      if ((double) num2 > (double) this.MinimumPlayerDistance && (double) num2 < (double) this.DistanceToPathTowardsPlayer)
      {
        Vector3 normalized = (this.GetClosestTarget().transform.position - this.transform.position).normalized;
        float num3 = this.MaxMoveDistance.y;
        RaycastHit2D raycastHit2D;
        if ((UnityEngine.Object) (raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) normalized, 100f, (int) this.layerToCheck)).collider != (UnityEngine.Object) null)
          num3 = Mathf.Min(num3, Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.point));
        this.TargetPosition = this.transform.position + normalized * UnityEngine.Random.Range(this.MaxMoveDistance.x, num3);
        return;
      }
      this.PathingToPlayer = false;
    }
    while ((double) --num1 > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      if (!this.PathingToPlayer)
        this.RandomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      this.PathingToPlayer = false;
      float radius = 0.2f;
      Vector3 targetLocation = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if (this.ShowDebug)
        {
          this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - targetLocation) * this.CircleCastOffset);
          this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
        if (this.ShowDebug)
        {
          this.EndPoints.Add(new Vector3(targetLocation.x, targetLocation.y));
          this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
        this.givePath(targetLocation);
        this.TargetPosition = targetLocation;
        break;
      }
    }
  }

  public void DoBusiness() => this.StartCoroutine((IEnumerator) this.BusinessRoutine());

  public IEnumerator BusinessRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyDog enemyDog = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", enemyDog.gameObject);
      GameManager.GetInstance().OnConversationNew();
      PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      DecorationCustomTarget.Create(enemyDog.transform.position, PlayerFarming.Instance.gameObject.transform.position, 1f, StructureBrain.TYPES.DECORATION_MONSTERSHRINE, new System.Action(enemyDog.FinishedGettingDecoration));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void FinishedGettingDecoration()
  {
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.red);
    if ((double) this.ChanceToPathTowardsPlayer > 0.0)
      Utils.DrawCircleXY(this.transform.position, (float) this.DistanceToPathTowardsPlayer, Color.cyan);
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], this.CircleCastRadius, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], this.CircleCastRadius, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }
}
