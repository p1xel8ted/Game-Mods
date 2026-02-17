// Decompiled with JetBrains decompiler
// Type: EnemyJailer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyJailer : UnitObject
{
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public Animator chainSmashAnimator;
  public IndicatorFlash targetIndicator;
  public EnemyJailer.JailerState currentState;
  public EnemyJailer.DorryStateChange onStateChange;
  [SerializeField]
  public float movementSpeed;
  [SerializeField]
  public float chaseSpeed;
  [SerializeField]
  public float maxTimeMovingToTarget;
  public float repathTimer;
  [SerializeField]
  public Transform damageCollidersParent;
  [SerializeField]
  public ColliderEvents[] damageColliders;
  [SerializeField]
  public float closeAttackDistance;
  [SerializeField]
  public float damage;
  [SerializeField]
  public ChainHook chainHook;
  [SerializeField]
  public WeaponData weaponData;
  [SerializeField]
  public EquipmentType equipmentType;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float prob_attack_0_attack_5;
  [SerializeField]
  public TrailRenderer trail;
  [SerializeField]
  public float attack_0_delay;
  [SerializeField]
  public float attack_0_duration;
  [SerializeField]
  public int attack_0_comboIndex;
  [SerializeField]
  [Range(0.01f, 10f)]
  public float attack_0_timeScaleMultiplier = 1f;
  [SerializeField]
  public float attack_1_delay;
  [SerializeField]
  public float attack_1_duration;
  [SerializeField]
  public int attack_1_comboIndex;
  [SerializeField]
  [Range(0.01f, 10f)]
  public float attack_1_timeScaleMultiplier = 1f;
  [SerializeField]
  public float attack_2_delay;
  [SerializeField]
  public float attack_2_duration;
  [SerializeField]
  public int attack_2_comboIndex;
  [SerializeField]
  [Range(0.01f, 10f)]
  public float attack_2_timeScaleMultiplier = 1f;
  [SerializeField]
  public float attack_3_delay;
  [SerializeField]
  public float attack_3_duration;
  [SerializeField]
  public int attack_3_comboIndex;
  [SerializeField]
  [Range(0.01f, 10f)]
  public float attack_3_timeScaleMultiplier = 1f;
  [SerializeField]
  public float attack_4_delay;
  [SerializeField]
  public float attack_4_duration;
  [SerializeField]
  public int attack_4_comboIndex;
  [SerializeField]
  public float attack_4_minDistance;
  [SerializeField]
  public float attack_4_maxDistance;
  [SerializeField]
  [Range(0.01f, 10f)]
  public float attack_4_timeScaleMultiplier = 1f;
  [Header("Projectiles")]
  [SerializeField]
  public Projectile attack_4_projectile;
  [SerializeField]
  public float attack_4_projectileSpeed;
  [SerializeField]
  public float attack_4_projectileLifetime;
  [SerializeField]
  public float attack_4_projectileRadiusOffset;
  [SerializeField]
  public int attack_4_projectileAmount;
  [SerializeField]
  public float attack_5_delay;
  [SerializeField]
  public float attack_5_duration;
  [SerializeField]
  public int attack_5_comboIndex;
  [SerializeField]
  public float attack_5_minDistance;
  [SerializeField]
  public float attack_5_maxDistance;
  [SerializeField]
  public float attack_5_timeBetweenSlams;
  [SerializeField]
  [Range(0.01f, 10f)]
  public float attack_5_timeScaleMultiplier = 1f;
  [SerializeField]
  public float idleTime;
  public float idleTimer;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/warning";
  [EventRef]
  public string AttackArcStartVO = " event:/dlc/dungeon06/enemy/jailer/attack_arc_start attack";
  [EventRef]
  public string AttackArcStartSFX = "event:/dlc/dungeon06/enemy/jailer/attack_arc_start";
  [EventRef]
  public string AttackSlamStartSFX = "event:/dlc/dungeon06/enemy/jailer/attack_slam_single_start";
  [EventRef]
  public string AttackSlamImpactSFX = "event:/dlc/dungeon06/enemy/jailer/attack_slam_single_impact";
  public EventInstance chainSwingInstanceSFX;
  public EventInstance slamStartInstanceSFX;
  public EventInstance slamImpactInstanceSFX;
  public Coroutine idleRoutine;
  public Coroutine moveRoutine;
  public Coroutine attack_0_Routine;
  public Coroutine attack_1_Routine;
  public Coroutine attack_2_Routine;
  public Coroutine attack_3_Routine;
  public Coroutine attack_4_Routine;
  public Coroutine attack_5_Routine;

  public EnemyJailer.JailerState CURRENT_JAILER_STATE
  {
    get => this.currentState;
    set
    {
      if (this.onStateChange != null)
        this.OnStateChange(value, this.currentState);
      this.currentState = value;
    }
  }

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    this.InitDamageColliders();
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
    this.onStateChange += new EnemyJailer.DorryStateChange(this.OnStateChange);
    this.trail.emitting = false;
    this.targetIndicator.gameObject.SetActive(false);
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    this.DisposeDamageColliders();
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    this.onStateChange -= new EnemyJailer.DorryStateChange(this.OnStateChange);
  }

  public new void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!PlayerController.CanParryAttacks || FromBehind || AttackType != Health.AttackTypes.Melee)
      return;
    this.health.WasJustParried = true;
    this.SimpleSpineFlash.FlashWhite(false);
    this.SeperateObject = true;
    this.UsePathing = true;
    this.health.invincible = false;
    this.DisableForces = false;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.StopAttackInstanceSFX();
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.gameObject);
    if (this.health.HasShield || this.health.WasJustParried)
      return;
    if (this.state.CURRENT_STATE != StateMachine.State.Attacking)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.DisableForces = false;
      if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
        this.state.CURRENT_STATE = StateMachine.State.HitRight;
      if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
        this.state.CURRENT_STATE = StateMachine.State.HitLeft;
    }
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    this.SimpleSpineFlash.FlashFillRed();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyJailer enemyJailer = this;
    enemyJailer.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    enemyJailer.state.CURRENT_STATE = StateMachine.State.Dancing;
    enemyJailer.CURRENT_JAILER_STATE = EnemyJailer.JailerState.Idle;
  }

  public IEnumerator IdleState()
  {
    while ((double) (this.idleTimer -= Time.deltaTime * this.Spine.timeScale) > 0.0)
      yield return (object) null;
    this.CURRENT_JAILER_STATE = EnemyJailer.JailerState.MoveToTarget;
    yield return (object) null;
  }

  public IEnumerator MoveToTargetState()
  {
    EnemyJailer enemyJailer = this;
    Vector3 targetPosition = enemyJailer.GetClosestTarget().transform.position;
    while ((double) Vector2.Distance((Vector2) enemyJailer.transform.position, (Vector2) targetPosition) > (double) enemyJailer.closeAttackDistance)
    {
      targetPosition = enemyJailer.GetClosestTarget().transform.position;
      enemyJailer.FacePosition(targetPosition);
      enemyJailer.Move(enemyJailer.movementSpeed);
      yield return (object) null;
    }
    float num = UnityEngine.Random.value;
    enemyJailer.CURRENT_JAILER_STATE = (double) num < (double) enemyJailer.prob_attack_0_attack_5 ? EnemyJailer.JailerState.Attack_0 : EnemyJailer.JailerState.Attack_5;
    yield return (object) null;
  }

  public IEnumerator Attack_0()
  {
    EnemyJailer enemyJailer = this;
    float timeScaleFraction = 1f / enemyJailer.attack_0_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackArcStartVO))
      AudioManager.Instance.PlayOneShot(enemyJailer.AttackArcStartVO, enemyJailer.gameObject);
    string animationName = (double) enemyJailer.Spine.skeleton.ScaleX < 0.0 ? "attack-throw-low" : "attack-throw-high";
    enemyJailer.Spine.AnimationState.SetAnimation(0, animationName, false).TimeScale = enemyJailer.attack_0_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackArcStartSFX))
      enemyJailer.chainSwingInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyJailer.AttackArcStartSFX, enemyJailer.transform);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_0_delay * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.ChainAttack_0();
    enemyJailer.trail.emitting = true;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_0_duration * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.trail.emitting = false;
    enemyJailer.CURRENT_JAILER_STATE = EnemyJailer.JailerState.Attack_4;
    yield return (object) null;
  }

  public IEnumerator Attack_1()
  {
    EnemyJailer enemyJailer = this;
    float timeScaleFraction = 1f / enemyJailer.attack_1_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackArcStartVO))
      AudioManager.Instance.PlayOneShot(enemyJailer.AttackArcStartVO, enemyJailer.gameObject);
    string animationName = (double) enemyJailer.Spine.skeleton.ScaleX < 0.0 ? "attack-throw-low" : "attack-throw-high";
    enemyJailer.Spine.AnimationState.SetAnimation(0, animationName, false).TimeScale = enemyJailer.attack_1_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackArcStartSFX))
      enemyJailer.chainSwingInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyJailer.AttackArcStartSFX, enemyJailer.transform);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_1_delay * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.ChainAttack_1();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_1_duration * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.CURRENT_JAILER_STATE = EnemyJailer.JailerState.Attack_2;
    yield return (object) null;
  }

  public IEnumerator Attack_2()
  {
    EnemyJailer enemyJailer = this;
    float timeScaleFraction = 1f / enemyJailer.attack_2_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackArcStartVO))
      AudioManager.Instance.PlayOneShot(enemyJailer.AttackArcStartVO, enemyJailer.gameObject);
    string animationName = (double) enemyJailer.Spine.skeleton.ScaleX < 0.0 ? "attack-throw-high" : "attack-throw-low";
    enemyJailer.Spine.AnimationState.SetAnimation(0, animationName, false).TimeScale = enemyJailer.attack_2_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackArcStartSFX))
      enemyJailer.chainSwingInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyJailer.AttackArcStartSFX, enemyJailer.transform);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_2_delay * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.ChainAttack_2();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_2_duration * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.CURRENT_JAILER_STATE = EnemyJailer.JailerState.Attack_3;
    yield return (object) null;
  }

  public IEnumerator Attack_3()
  {
    EnemyJailer enemyJailer = this;
    float timeScaleFraction = 1f / enemyJailer.attack_3_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackArcStartVO))
      AudioManager.Instance.PlayOneShot(enemyJailer.AttackArcStartVO, enemyJailer.gameObject);
    string animationName = (double) enemyJailer.Spine.skeleton.ScaleX < 0.0 ? "attack-throw-low" : "attack-throw-high";
    enemyJailer.Spine.AnimationState.SetAnimation(0, animationName, false).TimeScale = enemyJailer.attack_3_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackArcStartSFX))
      enemyJailer.chainSwingInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyJailer.AttackArcStartSFX, enemyJailer.transform);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_3_delay * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.ChainAttack_3();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_3_duration * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.CURRENT_JAILER_STATE = EnemyJailer.JailerState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_4()
  {
    EnemyJailer enemyJailer = this;
    float timeScaleFraction = 1f / enemyJailer.attack_4_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyJailer.WarningVO, enemyJailer.transform.position);
    string animationName = "attack-throw-smash";
    enemyJailer.Spine.AnimationState.SetAnimation(0, animationName, false).TimeScale = enemyJailer.attack_4_timeScaleMultiplier;
    if (!string.IsNullOrEmpty(enemyJailer.AttackSlamStartSFX))
      enemyJailer.slamStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyJailer.AttackSlamStartSFX, enemyJailer.transform);
    float time = 0.0f;
    enemyJailer.targetIndicator.gameObject.SetActive(true);
    while ((double) (time += Time.deltaTime * enemyJailer.Spine.timeScale) < (double) enemyJailer.attack_4_delay * (double) timeScaleFraction)
    {
      enemyJailer.FacePosition(enemyJailer.GetClosestTarget().transform.position);
      enemyJailer.targetIndicator.transform.position = enemyJailer.GetClosestTarget().transform.position;
      yield return (object) null;
    }
    enemyJailer.ChainAttack_4();
    enemyJailer.trail.emitting = true;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_4_duration * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.trail.emitting = false;
    enemyJailer.CURRENT_JAILER_STATE = EnemyJailer.JailerState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_5()
  {
    EnemyJailer enemyJailer = this;
    float time = 0.0f;
    string attack5Anim = "attack-throw-smash";
    float timeScaleFraction = 1f / enemyJailer.attack_5_timeScaleMultiplier;
    for (int x = 0; x < 3; ++x)
    {
      if (!string.IsNullOrEmpty(enemyJailer.WarningVO))
        AudioManager.Instance.PlayOneShot(enemyJailer.WarningVO, enemyJailer.transform.position);
      if (!string.IsNullOrEmpty(enemyJailer.AttackSlamStartSFX))
        enemyJailer.slamStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyJailer.AttackSlamStartSFX, enemyJailer.transform);
      enemyJailer.trail.emitting = false;
      enemyJailer.Spine.AnimationState.SetAnimation(0, attack5Anim, false).TimeScale = enemyJailer.attack_5_timeScaleMultiplier;
      while ((double) (time += Time.deltaTime * enemyJailer.Spine.timeScale) < (double) enemyJailer.attack_5_delay * (double) timeScaleFraction)
      {
        enemyJailer.FacePosition(enemyJailer.GetClosestTarget().transform.position, true);
        yield return (object) null;
      }
      time = 0.0f;
      enemyJailer.ChainAttack_5();
      enemyJailer.trail.emitting = true;
      if (x < 2)
        yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_5_timeBetweenSlams * timeScaleFraction, enemyJailer.Spine);
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyJailer.attack_5_duration * timeScaleFraction, enemyJailer.Spine);
    enemyJailer.trail.emitting = false;
    enemyJailer.CURRENT_JAILER_STATE = EnemyJailer.JailerState.Idle;
    yield return (object) null;
  }

  public void FacePosition(Vector3 position, bool setIndicator = false)
  {
    if (setIndicator)
    {
      this.targetIndicator.transform.position = position;
      this.targetIndicator.gameObject.SetActive(true);
    }
    float angle = Utils.GetAngle(this.transform.position, position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 vector3 = new Vector3(-1f, 1f, 1f);
    this.damageCollidersParent.localScale = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? vector3 : Vector3.one;
  }

  public void OnStateChange(EnemyJailer.JailerState newState, EnemyJailer.JailerState prevState)
  {
    switch (newState)
    {
      case EnemyJailer.JailerState.Idle:
        this.idleTimer = this.idleTime;
        this.Spine.AnimationState.SetAnimation(0, "idle", true);
        if (this.idleRoutine != null)
          this.StopCoroutine(this.idleRoutine);
        this.idleRoutine = this.StartCoroutine((IEnumerator) this.IdleState());
        break;
      case EnemyJailer.JailerState.MoveToTarget:
        this.repathTimer = 1f;
        this.speed = this.movementSpeed * this.SpeedMultiplier;
        this.Spine.AnimationState.SetAnimation(0, "move", true);
        if (this.moveRoutine != null)
          this.StopCoroutine(this.moveRoutine);
        this.moveRoutine = this.StartCoroutine((IEnumerator) this.MoveToTargetState());
        break;
      case EnemyJailer.JailerState.Attack_0:
        this.FacePosition(this.GetClosestTarget().transform.position);
        Debug.Log((object) "Jailer attack 0");
        if (this.attack_0_Routine != null)
          this.StopCoroutine(this.attack_0_Routine);
        this.attack_0_Routine = this.StartCoroutine((IEnumerator) this.Attack_0());
        break;
      case EnemyJailer.JailerState.Attack_1:
        this.FacePosition(this.GetClosestTarget().transform.position);
        Debug.Log((object) "Jailer attack 1");
        if (this.attack_1_Routine != null)
          this.StopCoroutine(this.attack_1_Routine);
        this.attack_1_Routine = this.StartCoroutine((IEnumerator) this.Attack_1());
        break;
      case EnemyJailer.JailerState.Attack_2:
        this.FacePosition(this.GetClosestTarget().transform.position);
        Debug.Log((object) "Jailer attack 2");
        if (this.attack_2_Routine != null)
          this.StopCoroutine(this.attack_2_Routine);
        this.attack_2_Routine = this.StartCoroutine((IEnumerator) this.Attack_2());
        break;
      case EnemyJailer.JailerState.Attack_3:
        this.FacePosition(this.GetClosestTarget().transform.position);
        Debug.Log((object) "Jailer attack 3");
        if (this.attack_3_Routine != null)
          this.StopCoroutine(this.attack_3_Routine);
        this.attack_3_Routine = this.StartCoroutine((IEnumerator) this.Attack_3());
        break;
      case EnemyJailer.JailerState.Attack_4:
        this.FacePosition(this.GetClosestTarget().transform.position);
        Debug.Log((object) "Jailer attack 4");
        if (this.attack_4_Routine != null)
          this.StopCoroutine(this.attack_4_Routine);
        this.attack_4_Routine = this.StartCoroutine((IEnumerator) this.Attack_4());
        break;
      case EnemyJailer.JailerState.Attack_5:
        this.FacePosition(this.GetClosestTarget().transform.position, true);
        Debug.Log((object) "Jailer attack 5");
        if (this.attack_5_Routine != null)
          this.StopCoroutine(this.attack_5_Routine);
        this.attack_5_Routine = this.StartCoroutine((IEnumerator) this.Attack_5());
        break;
    }
  }

  public void StopAttackInstanceSFX()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.chainSwingInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.slamStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.slamImpactInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void FindPath(Vector3 PointToCheck) => this.givePath(PointToCheck, forceIgnoreAStar: true);

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    this.ClearPaths();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.PlaceIE());
  }

  public IEnumerator PlaceIE()
  {
    EnemyJailer enemyJailer = this;
    enemyJailer.ClearPaths();
    Vector3 insideUnitCircle = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    enemyJailer.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJailer.Spine.AnimationState.SetAnimation(0, "idle", true);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    if (component.team != this.health.team)
    {
      component.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || component.isPlayer)
        return;
      component.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    }
  }

  public IEnumerator EnableDamageColliderTime(ColliderEvents damageCollider, float initialDelay)
  {
    if ((bool) (UnityEngine.Object) damageCollider)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      damageCollider.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      damageCollider.SetActive(false);
    }
  }

  public IEnumerator EnableDamageCollider(ColliderEvents damageCollider, float initialDelay)
  {
    if ((bool) (UnityEngine.Object) damageCollider)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      damageCollider.SetActive(true);
    }
  }

  public void DisableDamageCollider(ColliderEvents damageCollider)
  {
    if (!(bool) (UnityEngine.Object) damageCollider)
      return;
    damageCollider.SetActive(false);
  }

  public void DisableAllDamageColliders()
  {
    foreach (ColliderEvents damageCollider in this.damageColliders)
      damageCollider.SetActive(false);
  }

  public void Move(float movementSpeed)
  {
    if (float.IsNaN(this.state.facingAngle))
      return;
    if (float.IsNaN(this.speed) || float.IsInfinity(this.speed))
      this.speed = 0.0f;
    this.speed = Mathf.Clamp(movementSpeed, 0.0f, this.maxSpeed);
    this.moveVX = this.speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f));
    this.moveVY = this.speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f));
    float num = this.UseDeltaTime ? GameManager.DeltaTime : GameManager.UnscaledDeltaTime;
    if (float.IsNaN(this.moveVX) || float.IsInfinity(this.moveVX))
      this.moveVX = 0.0f;
    if (float.IsNaN(this.moveVY) || float.IsInfinity(this.moveVY))
      this.moveVY = 0.0f;
    this.rb.MovePosition(this.rb.position + new Vector2(this.vx, this.vy) * Time.deltaTime + new Vector2(this.moveVX, this.moveVY) * num + new Vector2(this.seperatorVX, this.seperatorVY) * num + new Vector2(this.knockBackVX, this.knockBackVY) * num);
  }

  public void InitDamageColliders()
  {
    foreach (ColliderEvents damageCollider in this.damageColliders)
    {
      damageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      damageCollider.SetActive(false);
    }
  }

  public void DisposeDamageColliders()
  {
    foreach (ColliderEvents damageCollider in this.damageColliders)
    {
      damageCollider.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      damageCollider.SetActive(false);
    }
  }

  public void ChainAttack_0()
  {
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_0_comboIndex];
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetVisuals(this.equipmentType);
    Vector3 vector3 = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    this.chainHook.Init(new EllipseMovement(Vector3.up, Vector3.right, -(new Vector3(-vector3.y, vector3.x) * Mathf.Sign(this.transform.localScale.x)) * combo.OffsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, this.state.facingAngle + combo.StartAngle, combo.AngleToMove, combo.Duration / this.attack_0_timeScaleMultiplier, combo.RadiusMultiplierOverTime), 1f, rangeRadius, attackFlags, true, combo.ScaleMultiplierOverTime, false, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, ownerSpine: this.Spine);
    this.chainHook.SetHideTimeMultiplier(1f / this.attack_0_timeScaleMultiplier);
  }

  public void ChainAttack_1()
  {
    if (!string.IsNullOrEmpty(this.AttackVO))
      AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_1_comboIndex];
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetVisuals(this.equipmentType);
    Vector3 vector3 = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    this.chainHook.Init(new EllipseMovement(Vector3.up, Vector3.right, -new Vector3(-vector3.y, vector3.x) * combo.OffsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, this.state.facingAngle + combo.StartAngle, combo.AngleToMove, combo.Duration / this.attack_1_timeScaleMultiplier, combo.RadiusMultiplierOverTime), 1f, rangeRadius, attackFlags, true, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, ownerSpine: this.Spine);
    this.chainHook.SetHideTimeMultiplier(1f / this.attack_1_timeScaleMultiplier);
  }

  public void ChainAttack_2()
  {
    if (!string.IsNullOrEmpty(this.AttackVO))
      AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_2_comboIndex];
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetVisuals(this.equipmentType);
    Vector3 vector3 = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    this.chainHook.Init(new EllipseMovement(Vector3.up, Vector3.right, -new Vector3(-vector3.y, vector3.x) * combo.OffsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, this.state.facingAngle + combo.StartAngle, combo.AngleToMove, combo.Duration / this.attack_2_timeScaleMultiplier, combo.RadiusMultiplierOverTime), 1f, rangeRadius, attackFlags, true, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, ownerSpine: this.Spine);
    this.chainHook.SetHideTimeMultiplier(1f / this.attack_2_timeScaleMultiplier);
  }

  public void ChainAttack_3()
  {
    if (!string.IsNullOrEmpty(this.AttackVO))
      AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_3_comboIndex];
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetVisuals(this.equipmentType);
    Vector3 vector3 = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    this.chainHook.Init(new EllipseMovement(Vector3.up, Vector3.right, -new Vector3(-vector3.y, vector3.x) * combo.OffsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, this.state.facingAngle + combo.StartAngle, combo.AngleToMove, combo.Duration / this.attack_3_timeScaleMultiplier, combo.RadiusMultiplierOverTime), 1f, rangeRadius, attackFlags, true, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, ownerSpine: this.Spine);
    this.chainHook.SetHideTimeMultiplier(1f / this.attack_3_timeScaleMultiplier);
  }

  public void ChainAttack_4()
  {
    if (!string.IsNullOrEmpty(this.AttackVO))
      AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_4_comboIndex];
    Vector3 up = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    Vector3 vector3_1 = new Vector3(-up.y, up.x);
    float offsetMultiplier = combo.OffsetMultiplier;
    float num1 = Vector3.Distance(this.GetClosestTarget().transform.position, this.transform.position);
    combo.EllipseRadiusX = Mathf.Clamp(num1, this.attack_4_minDistance, this.attack_4_maxDistance);
    combo.EllipseRadiusY = Mathf.Clamp(num1, this.attack_4_minDistance, this.attack_4_maxDistance);
    EllipseMovement ellipseMovement = new EllipseMovement(up, Vector3.back, vector3_1 * offsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, combo.StartAngle, combo.AngleToMove, combo.Duration / this.attack_4_timeScaleMultiplier, combo.RadiusMultiplierOverTime);
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetCollidersActive(false);
    this.chainHook.Init(ellipseMovement, 1f, rangeRadius, attackFlags, false, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, is3D: true, canHurtFriendlyUnits: true, onComplete: (System.Action<Vector3>) (hitPosition =>
    {
      this.chainHook.SetCollidersActive(true);
      Vector3 Position = hitPosition with { z = 0.0f };
      BiomeConstants.Instance.EmitHammerEffects(Position, 1f, 1.2f, 0.3f, true, 1.5f, false);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      this.targetIndicator.gameObject.SetActive(false);
      this.chainSmashAnimator.Play("Land");
      if (!string.IsNullOrEmpty(this.AttackSlamImpactSFX))
        this.slamImpactInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackSlamImpactSFX, this.transform);
      for (int index = 0; index < this.attack_4_projectileAmount; ++index)
      {
        float num2 = (float) (45.0 + (double) (360f / (float) this.attack_4_projectileAmount) * (double) index);
        Vector3 vector3_2 = (Vector3) (new Vector2(Mathf.Cos(num2 * ((float) Math.PI / 180f)), Mathf.Sin(num2 * ((float) Math.PI / 180f))) * this.attack_4_projectileRadiusOffset);
        Projectile projectile = ObjectPool.Spawn<Projectile>(this.attack_4_projectile, this.chainHook.GetHookPosition() + vector3_2, Quaternion.identity);
        projectile.Speed = this.attack_4_projectileSpeed;
        projectile.Angle = num2;
        projectile.Owner = this.health;
        projectile.team = this.health.team;
        projectile.LifeTime = this.attack_4_projectileLifetime;
      }
    }), ownerSpine: this.Spine);
    this.chainHook.SetHideTimeMultiplier(1f / this.attack_4_timeScaleMultiplier);
  }

  public void ChainAttack_5()
  {
    if (!string.IsNullOrEmpty(this.AttackVO))
      AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_5_comboIndex];
    Vector3 up = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    Vector3 vector3 = new Vector3(-up.y, up.x);
    float offsetMultiplier = combo.OffsetMultiplier;
    float num = Vector3.Distance(this.GetClosestTarget().transform.position, this.transform.position);
    combo.EllipseRadiusX = Mathf.Clamp(num, this.attack_4_minDistance, this.attack_4_maxDistance);
    combo.EllipseRadiusY = Mathf.Clamp(num, this.attack_4_minDistance, this.attack_4_maxDistance);
    EllipseMovement ellipseMovement = new EllipseMovement(up, Vector3.back, vector3 * offsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, combo.StartAngle, combo.AngleToMove, combo.Duration / this.attack_5_timeScaleMultiplier, combo.RadiusMultiplierOverTime);
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetCollidersActive(false);
    this.chainHook.Init(ellipseMovement, 1f, rangeRadius, attackFlags, false, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, is3D: true, canHurtFriendlyUnits: true, onComplete: (System.Action<Vector3>) (hitPosition =>
    {
      this.chainHook.SetCollidersActive(true);
      Vector3 Position = hitPosition with { z = 0.0f };
      BiomeConstants.Instance.EmitHammerEffects(Position, 1f, 1.2f, 0.3f, true, 1.5f, false);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      this.targetIndicator.gameObject.SetActive(false);
      this.chainSmashAnimator.Play("Land");
      if (string.IsNullOrEmpty(this.AttackSlamImpactSFX))
        return;
      this.slamImpactInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackSlamImpactSFX, this.transform);
    }), ownerSpine: this.Spine);
    this.chainHook.SetHideTimeMultiplier(1f / this.attack_5_timeScaleMultiplier);
  }

  [CompilerGenerated]
  public void \u003CChainAttack_4\u003Eb__113_0(Vector3 hitPosition)
  {
    this.chainHook.SetCollidersActive(true);
    Vector3 Position = hitPosition with { z = 0.0f };
    BiomeConstants.Instance.EmitHammerEffects(Position, 1f, 1.2f, 0.3f, true, 1.5f, false);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.targetIndicator.gameObject.SetActive(false);
    this.chainSmashAnimator.Play("Land");
    if (!string.IsNullOrEmpty(this.AttackSlamImpactSFX))
      this.slamImpactInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackSlamImpactSFX, this.transform);
    for (int index = 0; index < this.attack_4_projectileAmount; ++index)
    {
      float num = (float) (45.0 + (double) (360f / (float) this.attack_4_projectileAmount) * (double) index);
      Vector3 vector3 = (Vector3) (new Vector2(Mathf.Cos(num * ((float) Math.PI / 180f)), Mathf.Sin(num * ((float) Math.PI / 180f))) * this.attack_4_projectileRadiusOffset);
      Projectile projectile = ObjectPool.Spawn<Projectile>(this.attack_4_projectile, this.chainHook.GetHookPosition() + vector3, Quaternion.identity);
      projectile.Speed = this.attack_4_projectileSpeed;
      projectile.Angle = num;
      projectile.Owner = this.health;
      projectile.team = this.health.team;
      projectile.LifeTime = this.attack_4_projectileLifetime;
    }
  }

  [CompilerGenerated]
  public void \u003CChainAttack_5\u003Eb__114_0(Vector3 hitPosition)
  {
    this.chainHook.SetCollidersActive(true);
    Vector3 Position = hitPosition with { z = 0.0f };
    BiomeConstants.Instance.EmitHammerEffects(Position, 1f, 1.2f, 0.3f, true, 1.5f, false);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.targetIndicator.gameObject.SetActive(false);
    this.chainSmashAnimator.Play("Land");
    if (string.IsNullOrEmpty(this.AttackSlamImpactSFX))
      return;
    this.slamImpactInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackSlamImpactSFX, this.transform);
  }

  public enum JailerState
  {
    Idle,
    MoveToTarget,
    Attack_0,
    Attack_1,
    Attack_2,
    Attack_3,
    Attack_4,
    Attack_5,
  }

  public delegate void DorryStateChange(
    EnemyJailer.JailerState NewState,
    EnemyJailer.JailerState PrevState);
}
