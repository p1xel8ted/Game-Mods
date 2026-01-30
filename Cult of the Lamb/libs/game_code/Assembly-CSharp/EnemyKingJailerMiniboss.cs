// Decompiled with JetBrains decompiler
// Type: EnemyKingJailerMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class EnemyKingJailerMiniboss : UnitObject
{
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public GameObject IndicatorLarge;
  public GameObject IndicatorSmall;
  public SpriteRenderer AimingArrow;
  public GameObject targetObject;
  public EnemyKingJailerMiniboss.KingJailerState currentState;
  public EnemyKingJailerMiniboss.DorryStateChange onStateChange;
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
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/attack";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/warning";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/gethit";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/death";
  [EventRef]
  public string AttackBigCageStartVO = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_bigcage_start_vo";
  [EventRef]
  public string AttackArcStartVO = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_arc_start_vo";
  [EventRef]
  public string AttackBoomerangStartVO = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_boomerang_start_vo";
  [EventRef]
  public string AttackBoomerangStartSFX = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_boomerang_start";
  [EventRef]
  public string AttackBoomerangCatchSFX = "event:/dlc/dungeon06/enemy/boomerang/attack_head_catch";
  [EventRef]
  public string AttackBigCageStartSFX = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_bigcage_start";
  [EventRef]
  public string AttackBigCageImpactSFX = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_bigcage_impact";
  [EventRef]
  public string AttackArcStartSFX = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_arc_start";
  [EventRef]
  public string AttackArcImpactFX = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_bigcage_impact";
  [EventRef]
  public string AttackSpinningTrapSFX = "event:/dlc/dungeon06/enemy/miniboss_kingjailer/attack_spinningtrap_start";
  public EventInstance boomerangStartInstanceSFX;
  public EventInstance cageAttackStartInstanceSFX;
  public EventInstance arcStartInstanceSFX;
  public EventInstance spinningTrapStartInstanceSFX;
  public string boomerangCatchAnim = "attack-boomerang-catch";
  public string boomerangHurtAnim = "hit";
  [SerializeField]
  public float maxWalkDuration;
  [SerializeField]
  public float closeAttackDistance;
  [SerializeField]
  public float damage;
  [SerializeField]
  public ChainHook chainHook;
  [SerializeField]
  public WeaponData weaponData;
  [SerializeField]
  public EquipmentType smashEquipment;
  [SerializeField]
  public EquipmentType swingEquipment;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float prob_attack_0_attack_1;
  [SerializeField]
  public TrailRenderer trail;
  [SerializeField]
  public float fleshBallColliderRadius;
  [SerializeField]
  public float cageColliderRadius;
  [SerializeField]
  public float attack_1_prob;
  [SerializeField]
  public float attack_2_weight;
  [SerializeField]
  public float attack_3_weight;
  [SerializeField]
  public float attack_4_weight;
  [SerializeField]
  public float attack_5_weight;
  [SerializeField]
  public float attack_6_weight;
  [SerializeField]
  public float attack_1_delay;
  [SerializeField]
  public float attack_1_duration;
  [SerializeField]
  public int attack_1_comboIndex;
  [SerializeField]
  public float attack_2_delay;
  [SerializeField]
  public float attack_2_duration;
  [SerializeField]
  public int attack_2_comboIndex;
  [SerializeField]
  public float attack_2_minDistance;
  [SerializeField]
  public float attack_2_maxDistance;
  [SerializeField]
  public float attack_3_delay;
  [SerializeField]
  public float attack_3_duration;
  [SerializeField]
  public float attack_3_loopTime;
  [SerializeField]
  public int attack_3_comboIndex;
  [SerializeField]
  public Projectile attack_3_projectile;
  [SerializeField]
  public float attack_3_projectileSpeed;
  [SerializeField]
  public float attack_3_projectileLifetime;
  [SerializeField]
  public float attack_3_projectileRadiusOffset;
  [SerializeField]
  public int attack_3_projectileAmount;
  [SerializeField]
  public bool attack_3_followsPlayer;
  [SerializeField]
  public float attack_3_minDistance;
  [SerializeField]
  public float attack_3_maxDistance;
  [SerializeField]
  public float attack_4_delay;
  [SerializeField]
  public float attack_4_duration;
  [SerializeField]
  public float attack_4_throwAngle;
  [SerializeField]
  public float attack_4_boomerangSpeed;
  [SerializeField]
  public float attack_4_boomerangDecelartion = 3f;
  [SerializeField]
  public float attack_4_boomerangTurningSpeed;
  [SerializeField]
  public float attack_4_boomerangRecoverSpeed = 6f;
  [SerializeField]
  public float attack_4_boomerangRecoverTurningSpeed = 10f;
  [SerializeField]
  public float attack_4_boomerangHomecomingTime;
  [SerializeField]
  public float attack_4_boomerangRecoverDistance;
  [SerializeField]
  public float attack_4_boomerangStartScale = 1.2f;
  [SerializeField]
  public float attack_4_boomerangScaleDuration = 0.5f;
  [SerializeField]
  public float attack_4_boomerangSpawnDistance = 2.5f;
  [SerializeField]
  public Transform attack_4_boomerangOrigin;
  [SerializeField]
  public Projectile attack_4_boomerangProjectile;
  public Projectile currentBoomerang;
  public float currentBoomerangTimestamp;
  [SerializeField]
  public float attack_5_delay;
  [SerializeField]
  public float attack_5_duration;
  [SerializeField]
  public int attack_5_comboIndex;
  [SerializeField]
  public TrapFleshBall attack_5_fleshBallTrap;
  [SerializeField]
  public int maxFleshBallTraps;
  public EnemyKingJailerMiniboss.Attack_5_Conditions currentAttack5Conditions;
  public int currentAttack5ConditionsLoop;
  [SerializeField]
  public EnemyKingJailerMiniboss.Attack_5_Conditions[] attack_5_conditions;
  public List<TrapFleshBall> currentFleshBallTraps = new List<TrapFleshBall>();
  [SerializeField]
  public float attack_6_delay;
  [SerializeField]
  public float attack_6_duration;
  [SerializeField]
  public TrapRopeChainHook attack_6_ropeChainHookTrap;
  [SerializeField]
  public int maxRopeTraps;
  [SerializeField]
  public Transform ropeTrapPointsParent;
  [SerializeField]
  public Transform[] ropeTrapPoints;
  public Dictionary<Transform, TrapRopeChainHook> currentRopeTraps = new Dictionary<Transform, TrapRopeChainHook>();
  [SerializeField]
  public float idleTime;
  public float idleTimer;
  [SerializeField]
  public bool isNG;
  public float reticuleFollowSpeed = 5f;
  public HeadDirectionManager headDirectionManager;
  public bool boomerangHitPlayer;
  public bool boomerangKnockedBack;
  public GameObject currentTargetReticule;
  public Coroutine idleRoutine;
  public Coroutine moveRoutine;
  public Coroutine attack_1_Routine;
  public Coroutine attack_2_Routine;
  public Coroutine attack_3_Routine;
  public Coroutine attack_4_Routine;
  public Coroutine attack_5_Routine;
  public Coroutine attack_6_Routine;

  public EnemyKingJailerMiniboss.KingJailerState CURRENT_JAILER_STATE
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
    this.headDirectionManager = this.GetComponent<HeadDirectionManager>();
    this.SetAimingArrowActive(false);
    this.DisableSlamIndicators();
  }

  public void SetAimingArrowActive(bool state) => this.AimingArrow?.gameObject.SetActive(state);

  public void SetAimingArrowDirection(float angle)
  {
    if ((UnityEngine.Object) this.AimingArrow == (UnityEngine.Object) null)
      return;
    this.AimingArrow.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
  }

  public override void OnDestroy()
  {
    this.StopEventInstanceSFX();
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
    this.onStateChange += new EnemyKingJailerMiniboss.DorryStateChange(this.OnStateChange);
    this.trail.Clear();
    this.trail.emitting = false;
    this.ropeTrapPointsParent.parent = (Transform) null;
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
    this.onStateChange -= new EnemyKingJailerMiniboss.DorryStateChange(this.OnStateChange);
    this.ropeTrapPointsParent.parent = this.transform;
    this.DestroyAllRopeTraps();
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
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    this.DisableSlamIndicators();
    this.SetAimingArrowActive(false);
    this.StopEventInstanceSFX();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if ((UnityEngine.Object) this.currentBoomerang != (UnityEngine.Object) null)
      this.currentBoomerang.DestroyProjectile(true);
    if ((bool) (UnityEngine.Object) this.transform.parent && (bool) (UnityEngine.Object) this.transform.parent.GetComponent<MiniBossController>())
    {
      for (int index = 0; index < Health.team2.Count; ++index)
      {
        if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health)
        {
          Health.team2[index].invincible = false;
          Health.team2[index].enabled = true;
          Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position);
        }
      }
    }
    this.DestroyAllRopeTraps();
  }

  public void StopEventInstanceSFX()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.boomerangStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.cageAttackStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.arcStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.spinningTrapStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO);
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
      this.FacePosition(Attacker.transform.position);
    this.SimpleSpineFlash.FlashFillRed();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyKingJailerMiniboss kingJailerMiniboss = this;
    kingJailerMiniboss.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) kingJailerMiniboss.targetObject == (UnityEngine.Object) null)
    {
      kingJailerMiniboss.SetTarget();
      yield return (object) null;
    }
    kingJailerMiniboss.state.CURRENT_STATE = StateMachine.State.Dancing;
    kingJailerMiniboss.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Idle;
  }

  public IEnumerator IdleState()
  {
    while ((double) (this.idleTimer -= Time.deltaTime * this.Spine.timeScale) > 0.0)
      yield return (object) null;
    while ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
    {
      this.SetTarget();
      yield return (object) null;
    }
    this.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.MoveToTarget;
    yield return (object) null;
  }

  public IEnumerator MoveToTargetState()
  {
    EnemyKingJailerMiniboss kingJailerMiniboss = this;
    while ((UnityEngine.Object) kingJailerMiniboss.targetObject == (UnityEngine.Object) null)
    {
      kingJailerMiniboss.SetTarget();
      yield return (object) null;
    }
    Vector3 targetPosition = kingJailerMiniboss.targetObject.transform.position;
    float timer = 0.0f;
    while ((double) Vector2.Distance((Vector2) kingJailerMiniboss.transform.position, (Vector2) targetPosition) > (double) kingJailerMiniboss.closeAttackDistance && (double) (timer += Time.deltaTime) < (double) kingJailerMiniboss.maxWalkDuration)
    {
      targetPosition = kingJailerMiniboss.targetObject.transform.position;
      kingJailerMiniboss.FacePosition(targetPosition);
      kingJailerMiniboss.Move(kingJailerMiniboss.movementSpeed);
      yield return (object) null;
    }
    float num = UnityEngine.Random.value;
    EnemyKingJailerMiniboss.KingJailerState kingJailerState = EnemyKingJailerMiniboss.KingJailerState.Attack_1;
    if ((double) num < (double) kingJailerMiniboss.attack_1_prob)
      kingJailerState = EnemyKingJailerMiniboss.KingJailerState.Attack_1;
    else if ((double) num < (double) kingJailerMiniboss.attack_3_weight)
      kingJailerState = EnemyKingJailerMiniboss.KingJailerState.Attack_3;
    else if ((double) num < (double) kingJailerMiniboss.attack_4_weight)
      kingJailerState = EnemyKingJailerMiniboss.KingJailerState.Attack_4;
    else if ((double) num < (double) kingJailerMiniboss.attack_6_weight && (double) kingJailerMiniboss.health.HP < (double) kingJailerMiniboss.health.totalHP / 2.0)
      kingJailerState = EnemyKingJailerMiniboss.KingJailerState.Attack_6;
    for (int index = 0; index < kingJailerMiniboss.attack_5_conditions.Length; ++index)
    {
      EnemyKingJailerMiniboss.Attack_5_Conditions attack5Condition = kingJailerMiniboss.attack_5_conditions[index];
      if ((double) kingJailerMiniboss.health.CurrentHP / (double) kingJailerMiniboss.health.totalHP < (double) attack5Condition.hp && !attack5Condition.thrown)
      {
        kingJailerState = EnemyKingJailerMiniboss.KingJailerState.Attack_5;
        kingJailerMiniboss.currentAttack5Conditions = attack5Condition;
        attack5Condition.thrown = true;
        break;
      }
    }
    kingJailerMiniboss.CURRENT_JAILER_STATE = kingJailerState;
    yield return (object) null;
  }

  public void Attack1() => this.StartCoroutine((IEnumerator) this.Attack_1());

  public void Attack2() => this.StartCoroutine((IEnumerator) this.Attack_2());

  public void Attack3() => this.StartCoroutine((IEnumerator) this.Attack_3());

  public void Attack4() => this.StartCoroutine((IEnumerator) this.Attack_4());

  public void Attack5() => this.StartCoroutine((IEnumerator) this.Attack_5());

  public void Attack6() => this.StartCoroutine((IEnumerator) this.Attack_6());

  public IEnumerator Attack_1()
  {
    EnemyKingJailerMiniboss kingJailerMiniboss = this;
    kingJailerMiniboss.trail.Clear();
    kingJailerMiniboss.SetAimingArrowActive(true);
    float timer = 0.0f;
    while ((double) timer < (double) kingJailerMiniboss.attack_1_delay)
    {
      timer += Time.deltaTime * kingJailerMiniboss.Spine.timeScale;
      kingJailerMiniboss.SetAimingArrowDirection(kingJailerMiniboss.state.facingAngle);
      yield return (object) null;
    }
    kingJailerMiniboss.SetAimingArrowActive(false);
    kingJailerMiniboss.ChainAttack_1();
    kingJailerMiniboss.trail.Clear();
    yield return (object) null;
    kingJailerMiniboss.trail.emitting = true;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(kingJailerMiniboss.attack_1_duration, kingJailerMiniboss.Spine);
    kingJailerMiniboss.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Attack_2;
    yield return (object) null;
  }

  public IEnumerator Attack_2()
  {
    float time = 0.0f;
    float minDistance = this.isNG ? this.attack_3_minDistance : this.attack_2_minDistance;
    float maxDistance = this.isNG ? this.attack_3_maxDistance : this.attack_2_maxDistance;
    this.ActivateSlamIndicator(this.IndicatorSmall, this.targetObject.transform.position, minDistance, maxDistance);
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.attack_2_delay)
    {
      this.FacePosition(this.targetObject.transform.position);
      this.UpdateSlamIndicatorPosition(minDistance, maxDistance);
      yield return (object) null;
    }
    if (this.isNG)
      this.ChainAttack_3();
    else
      this.ChainAttack_2();
    this.trail.emitting = true;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(this.attack_2_duration, this.Spine);
    this.trail.emitting = false;
    this.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_3()
  {
    float time = 0.0f;
    float attacksAmount = this.isNG ? 3f : 1f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 1.3329999446868896)
    {
      this.FacePosition(this.targetObject.transform.position);
      yield return (object) null;
    }
    for (int i = 0; (double) i < (double) attacksAmount; ++i)
    {
      this.ActivateSlamIndicator(this.IndicatorLarge, this.targetObject.transform.position, this.attack_3_minDistance, this.attack_3_maxDistance);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.attack_3_delay)
      {
        this.FacePosition(this.targetObject.transform.position);
        yield return (object) null;
      }
      this.ChainAttack_3();
      this.trail.emitting = true;
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.attack_3_loopTime)
        yield return (object) null;
      if ((double) i == (double) attacksAmount - 1.0)
      {
        this.Spine.AnimationState.GetCurrent(0).Loop = false;
        this.Spine.AnimationState.AddAnimation(0, "attack-cage-smash-end", false, 0.0f);
      }
      this.trail.emitting = false;
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(this.attack_3_duration, this.Spine);
    this.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_4()
  {
    EnemyKingJailerMiniboss kingJailerMiniboss = this;
    float timer = 0.0f;
    while ((double) timer <= (double) kingJailerMiniboss.attack_4_delay)
    {
      if ((UnityEngine.Object) kingJailerMiniboss.targetObject != (UnityEngine.Object) null)
        kingJailerMiniboss.FacePosition(kingJailerMiniboss.targetObject.transform.position);
      timer += Time.deltaTime * kingJailerMiniboss.Spine.timeScale;
      yield return (object) null;
    }
    kingJailerMiniboss.ThrowBoomerang();
    yield return (object) new WaitUntil((Func<bool>) new Func<bool>(kingJailerMiniboss.\u003CAttack_4\u003Eb__146_0));
    string animationName = kingJailerMiniboss.boomerangKnockedBack ? kingJailerMiniboss.boomerangHurtAnim : kingJailerMiniboss.boomerangCatchAnim;
    string soundPath = kingJailerMiniboss.boomerangKnockedBack ? kingJailerMiniboss.GetHitVO : kingJailerMiniboss.AttackBoomerangCatchSFX;
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath, kingJailerMiniboss.transform.position);
    kingJailerMiniboss.Spine.AnimationState.SetAnimation(0, animationName, false);
    kingJailerMiniboss.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    if (!string.IsNullOrEmpty(animationName))
    {
      Spine.Animation reactionAnim = kingJailerMiniboss.Spine.skeleton.Data.FindAnimation(animationName);
      float t = 0.0f;
      while ((double) t <= (double) reactionAnim.Duration)
      {
        t += Time.deltaTime * kingJailerMiniboss.Spine.timeScale;
        yield return (object) null;
      }
      reactionAnim = (Spine.Animation) null;
    }
    kingJailerMiniboss.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_5()
  {
    EnemyKingJailerMiniboss kingJailerMiniboss1 = this;
    float timer = 0.0f;
    while ((double) timer <= (double) kingJailerMiniboss1.attack_5_delay)
    {
      timer += Time.deltaTime * kingJailerMiniboss1.Spine.timeScale;
      yield return (object) null;
    }
    kingJailerMiniboss1.ChainAttack_5();
    kingJailerMiniboss1.trail.Clear();
    yield return (object) null;
    kingJailerMiniboss1.trail.emitting = true;
    timer = 0.0f;
    while ((double) timer <= (double) kingJailerMiniboss1.attack_5_duration)
    {
      timer += Time.deltaTime * kingJailerMiniboss1.Spine.timeScale;
      yield return (object) null;
    }
    EnemyKingJailerMiniboss kingJailerMiniboss2 = kingJailerMiniboss1;
    int num1 = kingJailerMiniboss1.currentAttack5ConditionsLoop + 1;
    int num2 = num1;
    kingJailerMiniboss2.currentAttack5ConditionsLoop = num2;
    if (num1 < kingJailerMiniboss1.currentAttack5Conditions.fleshBallAmount)
    {
      kingJailerMiniboss1.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Attack_5;
    }
    else
    {
      kingJailerMiniboss1.currentAttack5Conditions = (EnemyKingJailerMiniboss.Attack_5_Conditions) null;
      kingJailerMiniboss1.currentAttack5ConditionsLoop = 0;
      kingJailerMiniboss1.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Idle;
    }
    yield return (object) null;
  }

  public IEnumerator Attack_6()
  {
    float timer = 0.0f;
    while ((double) timer <= (double) this.attack_6_delay)
    {
      timer += Time.deltaTime * this.Spine.timeScale;
      yield return (object) null;
    }
    Transform targetTransform = this.ropeTrapPoints[UnityEngine.Random.Range(0, this.ropeTrapPoints.Length)];
    while (this.currentRopeTraps.ContainsKey(targetTransform) && (UnityEngine.Object) this.currentRopeTraps[targetTransform] != (UnityEngine.Object) null)
    {
      targetTransform = this.ropeTrapPoints[UnityEngine.Random.Range(0, this.ropeTrapPoints.Length)];
      yield return (object) null;
    }
    TrapRopeChainHook trap = ObjectPool.Spawn<TrapRopeChainHook>(this.attack_6_ropeChainHookTrap, targetTransform.position);
    this.AddRopeTrap(targetTransform, trap);
    timer = 0.0f;
    while ((double) timer <= (double) this.attack_6_duration)
    {
      timer += Time.deltaTime * this.Spine.timeScale;
      yield return (object) null;
    }
    this.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Idle;
    yield return (object) null;
  }

  public void AddRopeTrap(Transform point, TrapRopeChainHook trap)
  {
    if (this.currentRopeTraps.ContainsKey(point))
      this.currentRopeTraps[point] = trap;
    else
      this.currentRopeTraps.Add(point, trap);
  }

  public void DestroyAllRopeTraps()
  {
    List<Transform> list = this.currentRopeTraps.Keys.ToList<Transform>();
    for (int index = 0; index < this.currentRopeTraps.Keys.Count; ++index)
    {
      Transform key = list[index];
      if ((UnityEngine.Object) this.currentRopeTraps[key] != (UnityEngine.Object) null && (double) this.currentRopeTraps[key].GetHealth() > 0.0)
        this.currentRopeTraps[key].DestroyEarly();
    }
  }

  public void SetTarget()
  {
    Health closestTarget = this.GetClosestTarget(this.health.team == Health.Team.PlayerTeam);
    if (!(bool) (UnityEngine.Object) closestTarget)
      return;
    this.targetObject = closestTarget.gameObject;
    this.VisionRange = int.MaxValue;
  }

  public void FacePosition(Vector3 position)
  {
    if ((double) this.Spine.timeScale <= 1.0 / 1000.0)
      return;
    float angle = Utils.GetAngle(this.transform.position, position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 vector3 = new Vector3(-1f, 1f, 1f);
    this.damageCollidersParent.localScale = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? vector3 : Vector3.one;
  }

  public void OnStateChange(
    EnemyKingJailerMiniboss.KingJailerState newState,
    EnemyKingJailerMiniboss.KingJailerState prevState)
  {
    this.SetTarget();
    switch (newState)
    {
      case EnemyKingJailerMiniboss.KingJailerState.Idle:
        this.idleTimer = this.idleTime;
        this.Spine.AnimationState.SetAnimation(0, "idle", true);
        if (this.idleRoutine != null)
          this.StopCoroutine(this.idleRoutine);
        this.idleRoutine = this.StartCoroutine((IEnumerator) this.IdleState());
        break;
      case EnemyKingJailerMiniboss.KingJailerState.MoveToTarget:
        this.repathTimer = 1f;
        this.speed = this.movementSpeed * this.SpeedMultiplier;
        this.Spine.AnimationState.SetAnimation(0, "move", true);
        if (this.moveRoutine != null)
          this.StopCoroutine(this.moveRoutine);
        this.moveRoutine = this.StartCoroutine((IEnumerator) this.MoveToTargetState());
        break;
      case EnemyKingJailerMiniboss.KingJailerState.Attack_1:
        this.FacePosition(this.targetObject.transform.position);
        this.Spine.AnimationState.SetAnimation(0, (double) this.Spine.skeleton.ScaleX < 0.0 ? "attack-throw-low" : "attack-throw-high", false);
        if (!string.IsNullOrEmpty(this.AttackArcStartSFX))
          this.arcStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackArcStartSFX, this.transform);
        if (!string.IsNullOrEmpty(this.AttackArcStartVO))
          AudioManager.Instance.PlayOneShot(this.AttackArcStartVO);
        if (this.attack_1_Routine != null)
          this.StopCoroutine(this.attack_1_Routine);
        this.attack_1_Routine = this.StartCoroutine((IEnumerator) this.Attack_1());
        break;
      case EnemyKingJailerMiniboss.KingJailerState.Attack_2:
        this.FacePosition(this.targetObject.transform.position);
        this.Spine.AnimationState.SetAnimation(0, "attack-throw-smash", false);
        if (this.attack_2_Routine != null)
          this.StopCoroutine(this.attack_2_Routine);
        this.attack_2_Routine = this.StartCoroutine((IEnumerator) this.Attack_2());
        break;
      case EnemyKingJailerMiniboss.KingJailerState.Attack_3:
        this.FacePosition(this.targetObject.transform.position);
        this.Spine.AnimationState.SetAnimation(0, "attack-cage-smash-start", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-cage-smash-loop", true, 0.0f);
        if (!string.IsNullOrEmpty(this.AttackBigCageStartSFX))
          this.cageAttackStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackBigCageStartSFX, this.transform);
        if (!string.IsNullOrEmpty(this.AttackBigCageStartVO))
          AudioManager.Instance.PlayOneShot(this.AttackBigCageStartVO);
        if (this.attack_3_Routine != null)
          this.StopCoroutine(this.attack_3_Routine);
        this.attack_3_Routine = this.StartCoroutine((IEnumerator) this.Attack_3());
        break;
      case EnemyKingJailerMiniboss.KingJailerState.Attack_4:
        this.FacePosition(this.targetObject.transform.position);
        this.Spine.AnimationState.SetAnimation(0, "attack-boomerang", false);
        this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        if (!string.IsNullOrEmpty(this.AttackBoomerangStartSFX))
          this.boomerangStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackBoomerangStartSFX, this.transform);
        if (!string.IsNullOrEmpty(this.AttackBoomerangStartVO))
          AudioManager.Instance.PlayOneShot(this.AttackBoomerangStartVO);
        if (this.attack_4_Routine != null)
          this.StopCoroutine(this.attack_4_Routine);
        this.attack_4_Routine = this.StartCoroutine((IEnumerator) this.Attack_4());
        break;
      case EnemyKingJailerMiniboss.KingJailerState.Attack_5:
        if (this.CountFleshBallTraps() >= this.maxFleshBallTraps)
        {
          this.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Attack_1;
          break;
        }
        this.FacePosition(this.targetObject.transform.position);
        this.Spine.AnimationState.SetAnimation(0, (double) this.Spine.skeleton.ScaleX < 0.0 ? "attack-throw-low" : "attack-throw-high", false);
        if (!string.IsNullOrEmpty(this.AttackSpinningTrapSFX))
          this.spinningTrapStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackSpinningTrapSFX, this.transform);
        if (!string.IsNullOrEmpty(this.AttackVO))
          AudioManager.Instance.PlayOneShot(this.AttackVO);
        if (this.attack_5_Routine != null)
          this.StopCoroutine(this.attack_5_Routine);
        this.attack_5_Routine = this.StartCoroutine((IEnumerator) this.Attack_5());
        break;
      case EnemyKingJailerMiniboss.KingJailerState.Attack_6:
        if (this.CountRopeTraps() >= this.maxRopeTraps)
        {
          this.CURRENT_JAILER_STATE = EnemyKingJailerMiniboss.KingJailerState.Attack_3;
          break;
        }
        this.FacePosition(this.targetObject.transform.position);
        this.Spine.AnimationState.SetAnimation(0, "attack-summon", false);
        if (!string.IsNullOrEmpty(this.AttackSpinningTrapSFX))
          this.spinningTrapStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackSpinningTrapSFX, this.transform);
        if (this.attack_6_Routine != null)
          this.StopCoroutine(this.attack_6_Routine);
        this.attack_6_Routine = this.StartCoroutine((IEnumerator) this.Attack_6());
        break;
    }
  }

  public void FindPath(Vector3 PointToCheck) => this.givePath(PointToCheck, forceIgnoreAStar: true);

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, "idle", true);
  }

  public IEnumerator PlaceIE()
  {
    EnemyKingJailerMiniboss kingJailerMiniboss = this;
    kingJailerMiniboss.ClearPaths();
    Vector3 offset = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      kingJailerMiniboss.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 vector3 = (PlayerFarming.Instance.transform.position + offset) with
      {
        z = 0.0f
      };
      kingJailerMiniboss.transform.position = vector3;
      yield return (object) null;
    }
    kingJailerMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    kingJailerMiniboss.Spine.AnimationState.SetAnimation(0, "idle", true);
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
    float num = (this.UseDeltaTime ? GameManager.DeltaTime : GameManager.UnscaledDeltaTime) * this.Spine.timeScale;
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

  public void ChainAttack_1()
  {
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_1_comboIndex];
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetVisuals(this.swingEquipment);
    Vector3 vector3 = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    this.chainHook.Init(new EllipseMovement(Vector3.up, Vector3.right, -(new Vector3(-vector3.y, vector3.x) * Mathf.Sign(this.transform.localScale.x)) * combo.OffsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, this.state.facingAngle, combo.AngleToMove, combo.Duration / 1f, combo.RadiusMultiplierOverTime), 1f, rangeRadius, attackFlags, true, combo.ScaleMultiplierOverTime, false, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, ownerSpine: this.Spine);
  }

  public void ChainAttack_2()
  {
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_2_comboIndex];
    Vector3 position1 = this.currentTargetReticule.transform.position;
    Vector3 normalized = (this.currentTargetReticule.transform.position - this.transform.position).normalized;
    Vector3 vector3_1 = new Vector3(-normalized.y, normalized.x);
    float offsetMultiplier = combo.OffsetMultiplier;
    float ellipseRadiusX = combo.EllipseRadiusX;
    float ellipseRadiusY = combo.EllipseRadiusY;
    Vector3 position2 = this.transform.position;
    double num = (double) Vector3.Distance(position1, position2);
    float radius2 = Mathf.Clamp((float) num, this.attack_2_minDistance, this.attack_2_maxDistance);
    float radius1 = Mathf.Clamp((float) num, this.attack_2_minDistance, this.attack_2_maxDistance);
    EllipseMovement ellipseMovement = new EllipseMovement(normalized, Vector3.back, vector3_1 * offsetMultiplier, radius1, radius2, combo.StartAngle, combo.AngleToMove, combo.Duration / 1f, combo.RadiusMultiplierOverTime);
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetCollidersActive(false);
    this.chainHook.SetVisuals(this.swingEquipment);
    if (!string.IsNullOrEmpty(this.AttackVO))
      AudioManager.Instance.PlayOneShot(this.AttackVO);
    this.chainHook.Init(ellipseMovement, 1f, rangeRadius, attackFlags, false, combo.ScaleMultiplierOverTime, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, is3D: true, canHurtFriendlyUnits: true, onComplete: (System.Action<Vector3>) (hitPosition =>
    {
      this.chainHook.SetCollidersActive(true);
      Vector3 vector3_2 = hitPosition with { z = 0.0f };
      BiomeConstants.Instance.EmitHammerEffects(vector3_2, 1f, 1.2f, 0.3f, true, 1.5f, false);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      this.DisableSlamIndicators();
      if (string.IsNullOrEmpty(this.AttackArcImpactFX))
        return;
      AudioManager.Instance.PlayOneShot(this.AttackArcImpactFX, vector3_2);
    }), ownerSpine: this.Spine);
  }

  public void ChainAttack_3()
  {
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_3_comboIndex];
    Vector3 position = this.currentTargetReticule.transform.position;
    Vector3 normalized = (this.currentTargetReticule.transform.position - this.transform.position).normalized;
    Vector3 vector3_1 = new Vector3(-normalized.y, normalized.x);
    float offsetMultiplier = combo.OffsetMultiplier;
    float radius2 = combo.EllipseRadiusX;
    float radius1 = combo.EllipseRadiusY;
    if (this.attack_3_followsPlayer)
    {
      double num = (double) Vector3.Distance(position, this.transform.position);
      radius2 = Mathf.Clamp((float) num, this.attack_3_minDistance, this.attack_3_maxDistance);
      radius1 = Mathf.Clamp((float) num, this.attack_3_minDistance, this.attack_3_maxDistance);
    }
    EllipseMovement ellipseMovement = new EllipseMovement(normalized, Vector3.back, vector3_1 * offsetMultiplier, radius1, radius2, combo.StartAngle, combo.AngleToMove, combo.Duration / 1f, combo.RadiusMultiplierOverTime);
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetCollidersActive(false);
    this.chainHook.SetVisuals(this.smashEquipment);
    if (!string.IsNullOrEmpty(this.AttackVO))
      AudioManager.Instance.PlayOneShot(this.AttackVO);
    this.chainHook.Init(ellipseMovement, 1f, rangeRadius, attackFlags, false, combo.ScaleMultiplierOverTime, false, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, is3D: true, onComplete: (System.Action<Vector3>) (hitPosition =>
    {
      this.chainHook.SetCollidersActive(true);
      Vector3 vector3_2 = hitPosition with { z = 0.0f };
      BiomeConstants.Instance.EmitHammerEffects(vector3_2, 1f, 1.2f, 0.3f, true, 1.5f, false);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
      this.DisableSlamIndicators();
      if (!string.IsNullOrEmpty(this.AttackBigCageImpactSFX))
        AudioManager.Instance.PlayOneShot(this.AttackBigCageImpactSFX, vector3_2);
      for (int index = 0; index < this.attack_3_projectileAmount; ++index)
      {
        float num = (float) (45.0 + (double) (360f / (float) this.attack_3_projectileAmount) * (double) index);
        Vector3 vector3_3 = (Vector3) (new Vector2(Mathf.Cos(num * ((float) Math.PI / 180f)), Mathf.Sin(num * ((float) Math.PI / 180f))) * this.attack_3_projectileRadiusOffset);
        Projectile projectile = ObjectPool.Spawn<Projectile>(this.attack_3_projectile, this.chainHook.GetHookPosition() + vector3_3, Quaternion.identity);
        projectile.Speed = this.attack_3_projectileSpeed;
        projectile.Angle = num;
        projectile.Owner = this.health;
        projectile.team = this.health.team;
        projectile.LifeTime = this.attack_3_projectileLifetime;
      }
    }), ownerSpine: this.Spine);
  }

  public void ChainAttack_5()
  {
    PlayerWeapon.WeaponCombos combo = this.weaponData.Combos[this.attack_5_comboIndex];
    float rangeRadius = combo.RangeRadius;
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0;
    this.chainHook.SetVisuals(this.swingEquipment);
    Vector3 vector3 = new Vector3(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    this.chainHook.Init(new EllipseMovement(Vector3.up, Vector3.right, -(new Vector3(-vector3.y, vector3.x) * Mathf.Sign(this.transform.localScale.x)) * combo.OffsetMultiplier, combo.EllipseRadiusY, combo.EllipseRadiusX, this.state.facingAngle + combo.StartAngle, combo.AngleToMove, combo.Duration / 1f, combo.RadiusMultiplierOverTime), 1f, rangeRadius, attackFlags, false, combo.ScaleMultiplierOverTime, false, colliderEnabledDelay: combo.ColliderEnabledDelay, colliderDisabledDelay: combo.ColliderDisabledDelay, onComplete: (System.Action<Vector3>) (hitPosition =>
    {
      hitPosition.z = 0.0f;
      TrapFleshBall trapFleshBall = ObjectPool.Spawn<TrapFleshBall>(this.attack_5_fleshBallTrap, this.chainHook.GetHookPosition());
      if (!(bool) (UnityEngine.Object) trapFleshBall)
        return;
      this.currentFleshBallTraps.Add(trapFleshBall);
      float angle = Utils.GetAngle(Vector3.zero, this.chainHook.Hook.right * -1f);
      trapFleshBall.EnableTrap(angle);
    }), ownerSpine: this.Spine);
  }

  public void ActivateSlamIndicator(
    GameObject indicator,
    Vector3 targetPosition,
    float minDistance,
    float maxDistance)
  {
    this.currentTargetReticule = indicator;
    Vector3 vector3 = targetPosition - this.transform.position;
    float num = Mathf.Clamp(vector3.magnitude, minDistance, maxDistance);
    this.currentTargetReticule.transform.position = this.transform.position + vector3.normalized * num;
    this.currentTargetReticule.SetActive(true);
  }

  public void UpdateSlamIndicatorPosition(float minDistance, float maxDistance)
  {
    if (!((UnityEngine.Object) this.currentTargetReticule != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
      this.SetTarget();
    Vector3 vector3 = this.targetObject.transform.position - this.transform.position;
    float num = Mathf.Clamp(vector3.magnitude, minDistance, maxDistance);
    this.currentTargetReticule.transform.position = Vector3.Lerp(this.currentTargetReticule.transform.position, this.transform.position + vector3.normalized * num, Time.deltaTime * this.reticuleFollowSpeed * this.Spine.timeScale);
  }

  public void DisableSlamIndicators()
  {
    this.IndicatorLarge.SetActive(false);
    this.IndicatorSmall.SetActive(false);
  }

  public float CalculateBoomerangeAngle()
  {
    Vector3 boomerangDirection = this.GetBoomerangDirection();
    return (float) (((double) Mathf.Atan2(boomerangDirection.y, boomerangDirection.x) * 57.295780181884766 + 360.0) % 360.0);
  }

  public Vector3 CalculateBoomerangSpawnPos()
  {
    float boomerangSpawnDistance = this.attack_4_boomerangSpawnDistance;
    return this.transform.position + this.GetBoomerangDirection() * boomerangSpawnDistance;
  }

  public Vector3 GetBoomerangDirection()
  {
    Vector3 boomerangDirection = Vector3.zero;
    boomerangDirection = !(this.Spine.AnimationState.GetCurrent(1).Animation.Name == this.headDirectionManager.North) ? ((double) this.Spine.skeleton.ScaleX >= 0.0 ? new Vector3(-1f, -1f, 0.0f) : new Vector3(1f, -1f, 0.0f)) : ((double) this.Spine.skeleton.ScaleX >= 0.0 ? new Vector3(-1f, 1f, 0.0f) : new Vector3(1f, 1f, 0.0f));
    return boomerangDirection;
  }

  public void ThrowBoomerang()
  {
    CameraManager.instance.ShakeCameraForDuration(1f, 1.1f, 0.1f);
    this.boomerangKnockedBack = false;
    float boomerangeAngle = this.CalculateBoomerangeAngle();
    Projectile boomerang = ObjectPool.Spawn<Projectile>(this.attack_4_boomerangProjectile, this.CalculateBoomerangSpawnPos() with
    {
      z = 0.0f
    });
    boomerang.Angle = boomerangeAngle;
    boomerang.Speed = this.attack_4_boomerangSpeed;
    boomerang.Deceleration = this.attack_4_boomerangDecelartion;
    boomerang.team = this.health.team;
    boomerang.LifeTime = 30f;
    boomerang.IgnoreIsland = true;
    boomerang.Trail.time = 0.3f;
    boomerang.Owner = this.health;
    boomerang.homeInOnTarget = true;
    boomerang.turningSpeed = this.attack_4_boomerangTurningSpeed;
    boomerang.CollideOnlyTarget = true;
    boomerang.SetTarget(this.targetObject.GetComponent<Health>());
    boomerang.Destroyable = false;
    Vector3 localScale = boomerang.transform.localScale;
    boomerang.transform.localScale *= this.attack_4_boomerangStartScale;
    boomerang.transform.DOScale(localScale, this.attack_4_boomerangScaleDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    boomerang.onHitOwner += new Projectile.OnHitUnit(this.OnHitOwner);
    boomerang.onHitPlayer += new Projectile.OnHitUnit(this.OnHitPlayer);
    boomerang.onKnockedBack += new Projectile.OnKnockedBack(this.OnProjetileKnockedback);
    boomerang.OnDestroyProjectile.AddListener(new UnityAction(this.OnDestroyProjetile));
    boomerang.SetParentSpine(this.Spine);
    boomerang.transform.parent = this.transform.parent;
    boomerang.GetComponentInChildren<Rotator>().SetParentSpine(this.Spine);
    this.currentBoomerang = boomerang;
    this.currentBoomerangTimestamp = GameManager.GetInstance().CurrentTime;
    this.StartCoroutine((IEnumerator) this.BoomerangMovement(boomerang));
  }

  public IEnumerator BoomerangMovement(Projectile boomerang)
  {
    float boomerangTravelTimer = 0.0f;
    while ((double) boomerangTravelTimer <= (double) this.attack_4_boomerangHomecomingTime && !this.boomerangHitPlayer)
    {
      boomerangTravelTimer += Time.deltaTime * this.Spine.timeScale;
      yield return (object) null;
    }
    boomerang.SetTarget(boomerang.Owner);
    boomerang.CollideOnlyTarget = false;
    boomerang.Speed = this.attack_4_boomerangRecoverSpeed;
    boomerang.turningSpeed = this.attack_4_boomerangRecoverTurningSpeed;
    while ((UnityEngine.Object) this.currentBoomerang != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(boomerang.transform.position, boomerang.Owner.transform.position) < (double) this.attack_4_boomerangRecoverDistance)
      {
        this.boomerangKnockedBack = boomerang.KnockedBack;
        this.OnHitOwner(boomerang);
      }
      yield return (object) null;
    }
  }

  public void OnHitOwner(Projectile boomerang)
  {
    this.boomerangHitPlayer = false;
    boomerang.Destroyable = true;
    CameraManager.instance.ShakeCameraForDuration(1f, 1.1f, 0.1f);
    boomerang.DestroyProjectile();
  }

  public void OnHitPlayer(Projectile boomerang)
  {
    if (this.boomerangHitPlayer)
      return;
    this.boomerangHitPlayer = true;
  }

  public void OnProjetileKnockedback() => this.boomerangKnockedBack = true;

  public void OnDestroyProjetile()
  {
    this.currentBoomerang.onHitOwner -= new Projectile.OnHitUnit(this.OnHitOwner);
    this.currentBoomerang.onHitPlayer -= new Projectile.OnHitUnit(this.OnHitPlayer);
    this.currentBoomerang.onKnockedBack -= new Projectile.OnKnockedBack(this.OnProjetileKnockedback);
    this.currentBoomerang.OnDestroyProjectile.RemoveListener(new UnityAction(this.OnDestroyProjetile));
    this.currentBoomerang = (Projectile) null;
  }

  public int CountFleshBallTraps()
  {
    int num = 0;
    this.currentFleshBallTraps.RemoveAll((Predicate<TrapFleshBall>) (x => !(bool) (UnityEngine.Object) x));
    foreach (UnityEngine.Object currentFleshBallTrap in this.currentFleshBallTraps)
    {
      if (currentFleshBallTrap != (UnityEngine.Object) null)
        ++num;
    }
    return num;
  }

  public int CountRopeTraps()
  {
    int num = 0;
    foreach (Transform key in this.currentRopeTraps.Keys)
    {
      if ((UnityEngine.Object) this.currentRopeTraps[key] != (UnityEngine.Object) null)
        ++num;
    }
    return num;
  }

  [CompilerGenerated]
  public bool \u003CAttack_4\u003Eb__146_0() => (UnityEngine.Object) this.currentBoomerang == (UnityEngine.Object) null;

  [CompilerGenerated]
  public void \u003CChainAttack_2\u003Eb__168_0(Vector3 hitPosition)
  {
    this.chainHook.SetCollidersActive(true);
    Vector3 vector3 = hitPosition with { z = 0.0f };
    BiomeConstants.Instance.EmitHammerEffects(vector3, 1f, 1.2f, 0.3f, true, 1.5f, false);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.DisableSlamIndicators();
    if (string.IsNullOrEmpty(this.AttackArcImpactFX))
      return;
    AudioManager.Instance.PlayOneShot(this.AttackArcImpactFX, vector3);
  }

  [CompilerGenerated]
  public void \u003CChainAttack_3\u003Eb__169_0(Vector3 hitPosition)
  {
    this.chainHook.SetCollidersActive(true);
    Vector3 vector3_1 = hitPosition with { z = 0.0f };
    BiomeConstants.Instance.EmitHammerEffects(vector3_1, 1f, 1.2f, 0.3f, true, 1.5f, false);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.DisableSlamIndicators();
    if (!string.IsNullOrEmpty(this.AttackBigCageImpactSFX))
      AudioManager.Instance.PlayOneShot(this.AttackBigCageImpactSFX, vector3_1);
    for (int index = 0; index < this.attack_3_projectileAmount; ++index)
    {
      float num = (float) (45.0 + (double) (360f / (float) this.attack_3_projectileAmount) * (double) index);
      Vector3 vector3_2 = (Vector3) (new Vector2(Mathf.Cos(num * ((float) Math.PI / 180f)), Mathf.Sin(num * ((float) Math.PI / 180f))) * this.attack_3_projectileRadiusOffset);
      Projectile projectile = ObjectPool.Spawn<Projectile>(this.attack_3_projectile, this.chainHook.GetHookPosition() + vector3_2, Quaternion.identity);
      projectile.Speed = this.attack_3_projectileSpeed;
      projectile.Angle = num;
      projectile.Owner = this.health;
      projectile.team = this.health.team;
      projectile.LifeTime = this.attack_3_projectileLifetime;
    }
  }

  [CompilerGenerated]
  public void \u003CChainAttack_5\u003Eb__170_0(Vector3 hitPosition)
  {
    hitPosition.z = 0.0f;
    TrapFleshBall trapFleshBall = ObjectPool.Spawn<TrapFleshBall>(this.attack_5_fleshBallTrap, this.chainHook.GetHookPosition());
    if (!(bool) (UnityEngine.Object) trapFleshBall)
      return;
    this.currentFleshBallTraps.Add(trapFleshBall);
    float angle = Utils.GetAngle(Vector3.zero, this.chainHook.Hook.right * -1f);
    trapFleshBall.EnableTrap(angle);
  }

  public enum KingJailerState
  {
    Idle,
    MoveToTarget,
    Attack_1,
    Attack_2,
    Attack_3,
    Attack_4,
    Attack_5,
    Attack_6,
    Attack_7,
  }

  public delegate void DorryStateChange(
    EnemyKingJailerMiniboss.KingJailerState NewState,
    EnemyKingJailerMiniboss.KingJailerState PrevState);

  [Serializable]
  public class Attack_5_Conditions
  {
    [SerializeField]
    public float hp;
    [SerializeField]
    public int fleshBallAmount;
    [SerializeField]
    public bool thrown;
  }
}
