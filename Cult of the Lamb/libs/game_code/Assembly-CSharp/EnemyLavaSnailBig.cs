// Decompiled with JetBrains decompiler
// Type: EnemyLavaSnailBig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyLavaSnailBig : UnitObject
{
  public static List<EnemyLavaSnailBig> Snails = new List<EnemyLavaSnailBig>();
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FallAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FuseBigAnimation;
  [Header("Targeted Projectiles")]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  public bool LoopSignPostAttackAnimation = true;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [Header("Bombardment")]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BombardmentSignPostAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BombardmentAttackAnimation;
  [Header("Melee")]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MeleetSignPostAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MeleeAttackAnimation;
  [HideInInspector]
  public bool HasMerged;
  public SpriteRenderer ShadowSpriteRenderer;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public float KnockbackModifier = 1f;
  public bool CanBeInterrupted = true;
  public float DamageColliderDuration = -1f;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/warning";
  [EventRef]
  public string MoveSFX = "event:/dlc/dungeon06/enemy/lavasnail_large/move";
  [EventRef]
  public string ProjectileA_StartSFX = "event:/dlc/dungeon06/enemy/lavasnail_large/attack_projectile_a_start";
  [EventRef]
  public string ProjectileA_ShootSFX = "event:/dlc/dungeon06/enemy/lavasnail_large/attack_projectile_a_shoot";
  [EventRef]
  public string ProjectileB_StartSFX = "event:/dlc/dungeon06/enemy/lavasnail_large/attack_projectile_b_start";
  [EventRef]
  public string ProjectileB_ShootSFX = "event:/dlc/dungeon06/enemy/lavasnail_large/attack_projectile_b_shoot";
  [EventRef]
  public string ProjectileLandSFX = "sfx_enemy_lavasnail_large_attack_projectile_land_01";
  [EventRef]
  public string ProjectileRainSFX = "event:/dlc/dungeon06/enemy/lavasnail_large/attack_projectile_rain";
  [EventRef]
  public string SlamStartSFX = "event:/dlc/dungeon06/enemy/lavasnail_large/attack_slam_start";
  [EventRef]
  public string SlamImpactSFX = "event:/dlc/dungeon06/enemy/lavasnail_large/attack_slam_ground_impact";
  public EventInstance attackStartAInstance;
  public EventInstance projectileBInstance;
  public EventInstance slamStartInstance;
  public float AttackDelayTime;
  public bool Attacking;
  public float currentAttackDelay;
  public float AttackCooldown = 0.2f;
  public float SignPostAttackDuration = 0.5f;
  [SerializeField]
  public float postAttackRestTime = 1.5f;
  [SerializeField]
  public Vector2 timeBetweenAttacksRange;
  [SerializeField]
  public Transform bombOrigin;
  [SerializeField]
  public LayerMask attackDamageLayers;
  [SerializeField]
  public float gravSpeed = -15f;
  [Header("Targeted Shots")]
  [SerializeField]
  public GameObject targetedArrowPrefab;
  [SerializeField]
  public int numTargetedShots;
  [SerializeField]
  public float bombDistanceFromTarget = 2f;
  [Tooltip("Chance between target shoot and bombardment attack.")]
  [Range(0.0f, 1f)]
  [SerializeField]
  public float targetShootChance = 0.7f;
  [SerializeField]
  public Vector2 targetedShotsDistanceRange = new Vector2(1f, 6f);
  [SerializeField]
  public float targetedProjectileStartHeight = -0.3f;
  [Header("Bombardment")]
  [SerializeField]
  public int bombardmentShotsNumber = 8;
  [SerializeField]
  public Vector2 bombardmentDistanceRange = new Vector2(1f, 4f);
  [SerializeField]
  public float bombardmentProjectileStartHeight = -0.2f;
  [Header("Melee")]
  [SerializeField]
  public float meleeDetectionDistance = 1.5f;
  [SerializeField]
  public GameObject meleeLavaPool;
  public float attackTimestamp;
  public float timeToNextAttack;
  public const float bombDuration = 1f;
  public bool DisableKnockback;
  public float Angle;
  public Vector3 Force;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public Vector2 timeToChangeTargetRange = new Vector2(10f, 15f);
  public float newTargetTimestamp;
  public float timeToNewTarget;
  public AnimationCurve movementPulse;
  [Range(0.1f, 10f)]
  public float pulseMovmentTime = 1f;
  public float currentMovementTime;
  public float frequency = 4f;
  public float amplitude = 1f;
  public Vector3 cyclePos;
  public Vector3 cycleTarget;
  public Vector3 currentDir;
  public Vector3 desiredDir;
  public Vector3 movementAxis;
  [SerializeField]
  public LayerMask obstacleLayermask;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyLavaSnailBig.Snails.Add(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.newTargetTimestamp = (bool) (UnityEngine.Object) GameManager.GetInstance() ? GameManager.GetInstance().CurrentTime : Time.time;
    this.attackTimestamp = (bool) (UnityEngine.Object) GameManager.GetInstance() ? GameManager.GetInstance().CurrentTime : Time.time;
    this.timeToNextAttack = UnityEngine.Random.Range(this.timeBetweenAttacksRange.x, this.timeBetweenAttacksRange.y);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.timeToNewTarget = 0.0f;
    if (this.HasMerged)
      this.Spine.AnimationState.SetAnimation(0, this.FuseBigAnimation, false);
    if (GameManager.RoomActive)
    {
      this.Attacking = false;
      this.health.invincible = false;
      foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.StartCoroutine((IEnumerator) this.WanderingRoutine());
    }
    else
      this.StartCoroutine((IEnumerator) this.WanderingRoutine());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyLavaSnailBig.Snails.Remove(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
  }

  public virtual IEnumerator WanderingRoutine()
  {
    EnemyLavaSnailBig enemyLavaSnailBig = this;
    yield return (object) new WaitForEndOfFrame();
    if (enemyLavaSnailBig.HasMerged)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyLavaSnailBig.Spine.timeScale) < 1.3333300352096558)
        yield return (object) null;
      enemyLavaSnailBig.HasMerged = false;
    }
    while (true)
    {
      while ((double) enemyLavaSnailBig.Spine.timeScale != 9.9999997473787516E-05)
      {
        if ((double) GameManager.GetInstance().TimeSince(enemyLavaSnailBig.newTargetTimestamp) >= (double) enemyLavaSnailBig.timeToNewTarget)
        {
          enemyLavaSnailBig.GetNewTargetPosition();
          enemyLavaSnailBig.StartPulseMovement();
        }
        if (enemyLavaSnailBig.MovingAnimation != "")
        {
          if (enemyLavaSnailBig.state.CURRENT_STATE == StateMachine.State.Moving && enemyLavaSnailBig.Spine.AnimationName != enemyLavaSnailBig.MovingAnimation)
            enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.MovingAnimation, true);
          if (enemyLavaSnailBig.state.CURRENT_STATE == StateMachine.State.Idle && enemyLavaSnailBig.Spine.AnimationName != enemyLavaSnailBig.IdleAnimation)
            enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.IdleAnimation, true);
        }
        if (enemyLavaSnailBig.ShouldAttack())
          enemyLavaSnailBig.StartCoroutine((IEnumerator) enemyLavaSnailBig.AttackRoutine());
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public virtual bool ShouldAttack()
  {
    return (double) GameManager.GetInstance().TimeSince(this.attackTimestamp) >= (double) this.timeToNextAttack && (double) (this.currentAttackDelay -= Time.deltaTime) < 0.0 && !this.Attacking && GameManager.RoomActive;
  }

  public void OnMoveSpineEvent()
  {
    AudioManager.Instance.PlayOneShot(this.MoveSFX, this.gameObject);
  }

  public IEnumerator AttackRoutine()
  {
    EnemyLavaSnailBig enemyLavaSnailBig = this;
    enemyLavaSnailBig.SetUpPreAttackData();
    if ((UnityEngine.Object) enemyLavaSnailBig.GetClosestTarget() != (UnityEngine.Object) null)
    {
      if ((double) UnityEngine.Random.value < (double) enemyLavaSnailBig.targetShootChance)
        yield return (object) enemyLavaSnailBig.StartCoroutine((IEnumerator) enemyLavaSnailBig.TargetShootAttackRoutine());
      else
        yield return (object) enemyLavaSnailBig.StartCoroutine((IEnumerator) enemyLavaSnailBig.BombardmentAttackRountine());
    }
    else
      yield return (object) enemyLavaSnailBig.StartCoroutine((IEnumerator) enemyLavaSnailBig.BombardmentAttackRountine());
    enemyLavaSnailBig.SetUpPostAttackData();
  }

  public IEnumerator TargetShootAttackRoutine()
  {
    EnemyLavaSnailBig enemyLavaSnailBig = this;
    enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.SignPostAttackAnimation, enemyLavaSnailBig.LoopSignPostAttackAnimation);
    enemyLavaSnailBig.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemyLavaSnailBig.attackStartAInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyLavaSnailBig.ProjectileA_StartSFX, enemyLavaSnailBig.transform);
    float Progress = 0.0f;
    float Duration = enemyLavaSnailBig.SignPostAttackDuration;
    while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemyLavaSnailBig.Spine.timeScale)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnailBig.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnailBig.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    int numTargetedShots = enemyLavaSnailBig.numTargetedShots;
    float direction = enemyLavaSnailBig.state.LookAngle;
    if ((UnityEngine.Object) enemyLavaSnailBig.GetClosestTarget() != (UnityEngine.Object) null)
    {
      direction = Utils.GetAngle(enemyLavaSnailBig.transform.position, enemyLavaSnailBig.GetClosestTarget().transform.position);
      enemyLavaSnailBig.LookAtAngle(Utils.GetAngle(enemyLavaSnailBig.transform.position, enemyLavaSnailBig.GetClosestTarget().transform.position));
    }
    enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.AttackAnimation, false);
    enemyLavaSnailBig.Spine.AnimationState.AddAnimation(0, enemyLavaSnailBig.IdleAnimation, true, 1f);
    while (--numTargetedShots >= 0)
    {
      CameraManager.shakeCamera(0.2f, direction);
      Vector3 vector3 = enemyLavaSnailBig.GetClosestTarget().transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * enemyLavaSnailBig.bombDistanceFromTarget;
      Vector3 closestPoint;
      if (!BiomeGenerator.PointWithinIsland(vector3, out closestPoint))
        vector3 = closestPoint - closestPoint.normalized;
      GameObject gameObject = ObjectPool.Spawn(enemyLavaSnailBig.targetedArrowPrefab, enemyLavaSnailBig.transform.parent, enemyLavaSnailBig.bombOrigin.position, Quaternion.identity);
      AudioManager.Instance.PlayOneShot(enemyLavaSnailBig.ProjectileA_ShootSFX, enemyLavaSnailBig.transform.position);
      GrenadeBullet component = gameObject.GetComponent<GrenadeBullet>();
      float num = Utils.GetAngle(enemyLavaSnailBig.transform.position, vector3) + (float) UnityEngine.Random.Range(-10, 10);
      double projectileStartHeight = (double) enemyLavaSnailBig.targetedProjectileStartHeight;
      double Angle = (double) num;
      double Speed = (double) UnityEngine.Random.Range(enemyLavaSnailBig.targetedShotsDistanceRange.x, enemyLavaSnailBig.targetedShotsDistanceRange.y);
      double Grav = (double) UnityEngine.Random.Range(enemyLavaSnailBig.gravSpeed - 2f, enemyLavaSnailBig.gravSpeed + 2f);
      int team = (int) enemyLavaSnailBig.health.team;
      string projectileLandSfx = enemyLavaSnailBig.ProjectileLandSFX;
      component.Play((float) projectileStartHeight, (float) Angle, (float) Speed, (float) Grav, (Health.Team) team, customImpactSFX: projectileLandSfx);
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnailBig.Spine.timeScale) < (double) enemyLavaSnailBig.AttackCooldown)
      yield return (object) null;
  }

  public IEnumerator BombardmentAttackRountine()
  {
    EnemyLavaSnailBig enemyLavaSnailBig = this;
    enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.BombardmentSignPostAttackAnimation, enemyLavaSnailBig.LoopSignPostAttackAnimation);
    enemyLavaSnailBig.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemyLavaSnailBig.projectileBInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyLavaSnailBig.ProjectileB_StartSFX, enemyLavaSnailBig.transform);
    float Progress = 0.0f;
    float Duration = enemyLavaSnailBig.SignPostAttackDuration;
    while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemyLavaSnailBig.Spine.timeScale)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnailBig.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnailBig.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    int i = enemyLavaSnailBig.bombardmentShotsNumber;
    float aimingAngle = enemyLavaSnailBig.state.LookAngle;
    enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.BombardmentAttackAnimation, false);
    enemyLavaSnailBig.Spine.AnimationState.AddAnimation(0, enemyLavaSnailBig.IdleAnimation, true, 2f);
    while (--i >= 0)
    {
      CameraManager.shakeCamera(0.2f, aimingAngle);
      Vector3 closestPoint;
      if (!BiomeGenerator.PointWithinIsland(enemyLavaSnailBig.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle, out closestPoint))
      {
        Vector3 vector3 = closestPoint - closestPoint.normalized;
      }
      GameObject gameObject = ObjectPool.Spawn(enemyLavaSnailBig.targetedArrowPrefab, enemyLavaSnailBig.transform.parent, enemyLavaSnailBig.bombOrigin.position, Quaternion.identity);
      AudioManager.Instance.PlayOneShot(enemyLavaSnailBig.ProjectileB_ShootSFX, enemyLavaSnailBig.transform.position);
      GrenadeBullet component = gameObject.GetComponent<GrenadeBullet>();
      component.SetOwner(enemyLavaSnailBig.gameObject);
      float Angle = (float) UnityEngine.Random.Range(0, 360);
      component.Play(enemyLavaSnailBig.bombardmentProjectileStartHeight, Angle, UnityEngine.Random.Range(enemyLavaSnailBig.bombardmentDistanceRange.x, enemyLavaSnailBig.bombardmentDistanceRange.y), UnityEngine.Random.Range(enemyLavaSnailBig.gravSpeed - 2f, enemyLavaSnailBig.gravSpeed + 2f), enemyLavaSnailBig.health.team, customImpactSFX: enemyLavaSnailBig.ProjectileLandSFX);
      yield return (object) new WaitForSeconds(0.3f / (float) enemyLavaSnailBig.bombardmentShotsNumber);
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnailBig.Spine.timeScale) < (double) enemyLavaSnailBig.AttackCooldown)
      yield return (object) null;
  }

  public IEnumerator MeleeAttackRoutine()
  {
    EnemyLavaSnailBig enemyLavaSnailBig = this;
    enemyLavaSnailBig.SetUpPreAttackData();
    enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.MeleetSignPostAttackAnimation, false);
    enemyLavaSnailBig.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemyLavaSnailBig.slamStartInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyLavaSnailBig.SlamStartSFX, enemyLavaSnailBig.transform);
    if (!string.IsNullOrEmpty(enemyLavaSnailBig.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyLavaSnailBig.WarningVO, enemyLavaSnailBig.transform.position);
    float Progress = 0.0f;
    float Duration = enemyLavaSnailBig.SignPostAttackDuration;
    while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemyLavaSnailBig.Spine.timeScale)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnailBig.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnailBig.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    double lookAngle = (double) enemyLavaSnailBig.state.LookAngle;
    if ((UnityEngine.Object) enemyLavaSnailBig.GetClosestTarget() != (UnityEngine.Object) null)
    {
      double angle = (double) Utils.GetAngle(enemyLavaSnailBig.transform.position, enemyLavaSnailBig.GetClosestTarget().transform.position);
      enemyLavaSnailBig.LookAtAngle(Utils.GetAngle(enemyLavaSnailBig.transform.position, enemyLavaSnailBig.GetClosestTarget().transform.position));
    }
    float meleeAnimationDuration = enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.MeleeAttackAnimation, false).Animation.Duration;
    yield return (object) new WaitForSeconds(0.1f);
    Vector3 position = enemyLavaSnailBig.bombOrigin.position with
    {
      z = 0.0f
    };
    foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) position, 2f, (int) enemyLavaSnailBig.attackDamageLayers))
    {
      Health component2 = component1.gameObject.GetComponent<Health>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2 != (UnityEngine.Object) enemyLavaSnailBig.health && (component2.team != enemyLavaSnailBig.health.team || enemyLavaSnailBig.health.IsCharmedEnemy))
        component2.DealDamage(1f, enemyLavaSnailBig.gameObject, position);
    }
    BiomeConstants.Instance.EmitHammerEffects(position, 0.5f, scale: 2f, playSFX: false);
    TrapLava.CreateLava(enemyLavaSnailBig.meleeLavaPool, position, enemyLavaSnailBig.transform.parent, enemyLavaSnailBig.health);
    if (!string.IsNullOrEmpty(enemyLavaSnailBig.SlamImpactSFX))
      AudioManager.Instance.PlayOneShot(enemyLavaSnailBig.SlamImpactSFX, enemyLavaSnailBig.gameObject);
    yield return (object) new WaitForSeconds(meleeAnimationDuration - 0.1f);
    enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnailBig.Spine.timeScale) < (double) enemyLavaSnailBig.AttackCooldown)
      yield return (object) null;
    enemyLavaSnailBig.SetUpPostAttackData();
  }

  public void SetUpPreAttackData()
  {
    this.Attacking = true;
    this.ClearPaths();
  }

  public void SetUpPostAttackData()
  {
    this.DisableForces = false;
    this.attackTimestamp = GameManager.GetInstance().CurrentTime;
    this.timeToNextAttack = UnityEngine.Random.Range(this.timeBetweenAttacksRange.x, this.timeBetweenAttacksRange.y);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.currentAttackDelay = this.AttackDelayTime;
    this.Attacking = false;
    this.newTargetTimestamp = GameManager.GetInstance().CurrentTime;
    this.timeToNewTarget = this.postAttackRestTime;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    this.StopAllInstanceSFX();
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
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && AttackType != Health.AttackTypes.NoReaction && !this.DisableKnockback && this.CanBeInterrupted)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyLavaSnailBig enemyLavaSnailBig = this;
    enemyLavaSnailBig.DisableForces = true;
    enemyLavaSnailBig.Angle = Utils.GetAngle(Attacker.transform.position, enemyLavaSnailBig.transform.position) * ((float) Math.PI / 180f);
    enemyLavaSnailBig.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyLavaSnailBig.Angle), 1500f * Mathf.Sin(enemyLavaSnailBig.Angle)) * enemyLavaSnailBig.KnockbackModifier);
    enemyLavaSnailBig.rb.AddForce((Vector2) enemyLavaSnailBig.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnailBig.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLavaSnailBig.DisableForces = false;
  }

  public IEnumerator ApplyForceRoutine(Vector3 forcePosition)
  {
    EnemyLavaSnailBig enemyLavaSnailBig = this;
    enemyLavaSnailBig.DisableForces = true;
    enemyLavaSnailBig.Angle = Utils.GetAngle(forcePosition, enemyLavaSnailBig.transform.position) * ((float) Math.PI / 180f);
    enemyLavaSnailBig.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyLavaSnailBig.Angle), 1500f * Mathf.Sin(enemyLavaSnailBig.Angle)) * enemyLavaSnailBig.KnockbackModifier);
    enemyLavaSnailBig.rb.AddForce((Vector2) enemyLavaSnailBig.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnailBig.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLavaSnailBig.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemyLavaSnailBig enemyLavaSnailBig = this;
    enemyLavaSnailBig.damageColliderEvents.SetActive(false);
    enemyLavaSnailBig.Attacking = false;
    enemyLavaSnailBig.ClearPaths();
    enemyLavaSnailBig.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyLavaSnailBig.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyLavaSnailBig.Spine.AnimationState.SetAnimation(0, enemyLavaSnailBig.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnailBig.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLavaSnailBig.DisableForces = false;
    enemyLavaSnailBig.StartCoroutine((IEnumerator) enemyLavaSnailBig.WanderingRoutine());
  }

  public void GetNewTargetPosition()
  {
    this.newTargetTimestamp = GameManager.GetInstance().CurrentTime;
    this.timeToNewTarget = UnityEngine.Random.Range(this.timeToChangeTargetRange.x, this.timeToChangeTargetRange.y);
    Vector3 vector3 = BiomeGenerator.GetRandomPositionInIsland();
    if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
      vector3 = this.GetClosestTarget().transform.position;
    int num = 100;
    while ((double) Vector2.Distance((Vector2) vector3, (Vector2) this.transform.position) <= 4.0)
    {
      vector3 = BiomeGenerator.GetRandomPositionInIsland();
      if (--num <= 0)
        break;
    }
    this.CalculateMovement(vector3);
  }

  public void CalculateMovement(Vector3 pos)
  {
    this.state.CURRENT_STATE = StateMachine.State.Moving;
    this.cycleTarget = pos;
    this.cyclePos = this.transform.position;
    this.currentDir = this.desiredDir;
    this.movementAxis = (Vector3) Vector2.Perpendicular((Vector2) this.currentDir);
    this.desiredDir = (this.cycleTarget - this.cyclePos).normalized;
  }

  public override void Update()
  {
    float deltaTime = this.UseDeltaTime ? GameManager.DeltaTime : GameManager.UnscaledDeltaTime;
    this.currentMovementTime -= Time.deltaTime;
    if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
    {
      Health closestTarget = this.GetClosestTarget();
      if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) closestTarget.transform.position, (Vector2) this.transform.position) < (double) this.meleeDetectionDistance)
        this.StartCoroutine((IEnumerator) this.MeleeAttackRoutine());
      this.CalculateCurrentSpeed(deltaTime);
      if ((double) this.currentMovementTime <= 0.0)
        this.StartPulseMovement();
      this.currentDir = (Vector3) Vector2.Lerp((Vector2) this.currentDir, (Vector2) this.desiredDir, 0.0075f * deltaTime).normalized;
      this.movementAxis = (Vector3) Vector2.Perpendicular((Vector2) this.currentDir);
      this.cyclePos += this.currentDir * deltaTime * this.speed;
      this.LookAtAngle(Utils.GetAngle(this.transform.position, this.cyclePos + this.movementAxis * Mathf.Sin(Time.time * this.frequency) * this.amplitude));
      if (this.CheckObstacle((Vector3) new Vector2(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f))), 1f))
      {
        this.GetNewTargetPosition();
        return;
      }
    }
    else
      this.speed += (float) ((0.0 - (double) this.speed) / 4.0) * deltaTime;
    this.move();
  }

  public void StartPulseMovement() => this.currentMovementTime = this.pulseMovmentTime;

  public void CalculateCurrentSpeed(float deltaTime)
  {
    this.speed = this.maxSpeed * this.movementPulse.Evaluate(Mathf.Clamp01((this.pulseMovmentTime - this.currentMovementTime) / this.pulseMovmentTime)) * deltaTime;
  }

  public void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public bool CheckObstacle(Vector3 dir, float distance)
  {
    return (bool) (UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) dir, distance, (int) this.obstacleLayermask).collider;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void StopAllInstanceSFX()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.attackStartAInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.projectileBInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.slamStartInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.red);
    Vector3 cycleTarget = this.cycleTarget;
    Utils.DrawCircleXY(this.cycleTarget, 0.2f, Color.blue);
    Vector3 currentDir = this.currentDir;
    Utils.DrawLine(this.transform.position, this.transform.position + this.currentDir * 2f, Color.red);
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
