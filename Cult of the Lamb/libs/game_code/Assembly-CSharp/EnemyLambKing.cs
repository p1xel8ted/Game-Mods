// Decompiled with JetBrains decompiler
// Type: EnemyLambKing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

#nullable disable
public class EnemyLambKing : UnitObject
{
  public float RepositionDistanceFromCenter = 2f;
  [SerializeField]
  public bool requireLineOfSite = true;
  [FormerlySerializedAs("projectilePrefab")]
  [SerializeField]
  public GameObject mortarProjectilePrefab;
  [SerializeField]
  public GameObject shotgunProjectilePrefab;
  [SerializeField]
  public Transform shotgunProjectileOrigin;
  public AssetReferenceGameObject JumpIndicatorPrefab;
  public EnemyLambKing.LambKingAttack lastAttack = EnemyLambKing.LambKingAttack.None;
  public bool armored = true;
  [SerializeField]
  public int bombsNeededToBreakArmor = 3;
  [SerializeField]
  public float timeToRestoreArmor = 5f;
  [SerializeField]
  public float armorDamageMultiplier = 0.2f;
  public float restoreArmorElapsed;
  public float initialDamageModifier;
  public float bombsImpacted;
  [SerializeField]
  public Vector2 MainAttackDelayRandomRange = new Vector2(6f, 6.2f);
  [SerializeField]
  public Vector2 MaxMainAttackDelayRandomRange = new Vector2(6f, 6.2f);
  [SerializeField]
  public List<EnemyLambKing.LambKingAttackChance> mainAttackChances;
  [SerializeField]
  public List<EnemyLambKing.LambKingAttackChance> secondaryAttackChances;
  [SerializeField]
  public List<EnemyLambKing.LambKingAttackChance> jumpAttackChances;
  public ColliderEvents MortarLineDamageColliderEvents;
  [FormerlySerializedAs("doMeleeDamage")]
  [SerializeField]
  public bool mineLineMeleeDamage;
  [SerializeField]
  public float mortarLineAttackWindup = 0.5f;
  [SerializeField]
  public float[] mortarAngles;
  [FormerlySerializedAs("targetDistanceFromPlayer")]
  [SerializeField]
  public float LineTargetDistanceFromPlayer = 2f;
  [SerializeField]
  public float timeBetweenLineShots = 0.4f;
  [SerializeField]
  public float lineMortarTrajectoryDuration = 0.7f;
  [FormerlySerializedAs("minimumAimDistance")]
  [SerializeField]
  public float mineLineMinimumAimDistance = 0.4f;
  [SerializeField]
  public float crossAngle = 45f;
  [SerializeField]
  public float mortarCrossAttackWindup = 0.5f;
  [SerializeField]
  public float timeBetweenCrossShots = 0.5f;
  [SerializeField]
  public float distanceBetweenCrossShots = 2f;
  [SerializeField]
  public float crossShotsPerRow = 4f;
  [SerializeField]
  public float crossMortarTrajectoryDuration = 0.7f;
  [SerializeField]
  public float mortarRainAttackWindup = 0.5f;
  [SerializeField]
  public float timeBetweenRainShots = 0.5f;
  [SerializeField]
  public float mortarRainAttackDuration = 2f;
  [SerializeField]
  public float mortarRainAreaRadius = 5f;
  [SerializeField]
  public float rainMortarTrajectoryDuration = 0.7f;
  [SerializeField]
  public float shotgunProjectileSpeed = 0.5f;
  [SerializeField]
  public float shotgunProjectileRadius = 0.4f;
  [SerializeField]
  public Vector2 shotgunProjectilesPerShot = new Vector2(6f, 8f);
  [SerializeField]
  public int shotgunShots = 3;
  [SerializeField]
  public float shotgunTimeToSpawnBullets = 0.433333f;
  [SerializeField]
  public float shotgunTimeBetweenShots = 0.7333333f;
  [SerializeField]
  public float shotgunAttackWindup = 0.7f;
  [SerializeField]
  public EnemyLambKing.LambKingProjectileCirclePatternAttack[] circlePatterns;
  public Stack<EnemyLambKing.LambKingProjectileCirclePatternAttack> circlePatternStack;
  [SerializeField]
  public float circleAttackShakeIntensity = 3f;
  public ColliderEvents JumpDamageColliderEvents;
  [SerializeField]
  public float minimumJumpDistance = 4f;
  [SerializeField]
  public float JumpAttackDuration = 1f;
  [SerializeField]
  public float JumpAttackWindup = 0.5f;
  [SerializeField]
  public float afterJumpWaitTime = 0.3f;
  [SerializeField]
  public AnimationCurve jumpFlightCurve = AnimationCurve.Linear(0.0f, 1f, 0.0f, 1f);
  [SerializeField]
  public float jumpLandShakeIntensity = 3f;
  [SerializeField]
  public float JumpHitColliderDuration = 0.2f;
  [SerializeField]
  public EnemyLambKing.LambKingProjectileCirclePatternAttack landingAttack;
  [FormerlySerializedAs("bulbLambPrefab")]
  [SerializeField]
  public AssetReferenceGameObject summonPrefab;
  [SerializeField]
  public EnemyLambKing.LambKingSummon[] summons;
  public Stack<EnemyLambKing.LambKingSummon> summonStack;
  [SerializeField]
  public float spawnDistance = 0.2f;
  [SerializeField]
  public float summonDelay = 0.6f;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public TrackEntry TrackEntry;
  [FormerlySerializedAs("BombsSpawnPattern")]
  public bool BombsSpawnProjectilePattern;
  [SerializeField]
  public EnemyLambKing.LambKingProjectileCirclePatternAttack bombProjectilePattern;
  public bool hitBombsSpawnPattern;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SerializeField]
  public CircleCollider2D bodyCollider;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/warning";
  [EventRef]
  public string AttackFleshSwarmerSpawnSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_flesh_swarmer_spawn_start";
  [EventRef]
  public string AttackFleshSwarmerSpawnVO = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_flesh_swarmer_spawn_start_vo";
  [EventRef]
  public string AttackJumpStartSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_jump_start";
  [EventRef]
  public string AttackSlamProjectileSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_slam_projectile_start";
  [EventRef]
  public string AttackSpitProjectileStartSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_spit_projectile_start";
  [EventRef]
  public string AttackOrganAStartSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_organ_a_start";
  [EventRef]
  public string AttackOrganAStartVO = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_organ_a_start_vo";
  [EventRef]
  public string AttackOrganBStartSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_organ_b_start";
  [EventRef]
  public string AttackOrganBStartVO = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_organ_b_start_vo";
  [EventRef]
  public string AttackOrganCStartSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_organ_c_start";
  [EventRef]
  public string AttackOrganCStartVO = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_organ_c_start_vo";
  [EventRef]
  public string AttackOrganLaunchSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/attack_organ_launch";
  [EventRef]
  public string MoveSingleStompSFX = "event:/dlc/dungeon06/enemy/miniboss_kingoflambs/mv_stomp_single";
  public EventInstance spitProjectileInstanceSFX;
  public EventInstance jumpStartInstanceSFX;
  public GameObject TargetObject;
  public float MainAttackDelay;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public bool \u003CFollowPlayer\u003Ek__BackingField;
  public GameObject jumpIndicatorLoaded;
  public GameObject currentJumpIndicator;
  public GameObject summonObjectLoaded;
  public Vector3 Force;
  public float KnockbackModifier = 1f;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public EnemyLambKing.State MyState;
  public float MaxMainAttackDelay;
  public Vector3 shootPosition;
  public float jumpAimingSpeed = 0.5f;
  public List<Health> ChildrenList = new List<Health>();
  public bool Spawning;
  public bool noMoreSpawns;
  public Health EnemyHealth;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public float Damage
  {
    get => this.\u003CDamage\u003Ek__BackingField;
    set => this.\u003CDamage\u003Ek__BackingField = value;
  }

  public bool FollowPlayer
  {
    get => this.\u003CFollowPlayer\u003Ek__BackingField;
    set => this.\u003CFollowPlayer\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    this.InitializeShotgunBullets();
    this.mainAttackChances = this.mainAttackChances.OrderByDescending<EnemyLambKing.LambKingAttackChance, float>((Func<EnemyLambKing.LambKingAttackChance, float>) (x => x.chance)).ToList<EnemyLambKing.LambKingAttackChance>();
    this.circlePatternStack = new Stack<EnemyLambKing.LambKingProjectileCirclePatternAttack>((IEnumerable<EnemyLambKing.LambKingProjectileCirclePatternAttack>) ((IEnumerable<EnemyLambKing.LambKingProjectileCirclePatternAttack>) this.circlePatterns).OrderBy<EnemyLambKing.LambKingProjectileCirclePatternAttack, float>((Func<EnemyLambKing.LambKingProjectileCirclePatternAttack, float>) (x => x.PercentageFrom)).ToArray<EnemyLambKing.LambKingProjectileCirclePatternAttack>());
    this.summonStack = new Stack<EnemyLambKing.LambKingSummon>((IEnumerable<EnemyLambKing.LambKingSummon>) ((IEnumerable<EnemyLambKing.LambKingSummon>) this.summons).OrderBy<EnemyLambKing.LambKingSummon, int>((Func<EnemyLambKing.LambKingSummon, int>) (x => x.Percent)).ToArray<EnemyLambKing.LambKingSummon>());
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    this.LoadAssets();
  }

  public void LoadAssets()
  {
    if (this.JumpIndicatorPrefab != null)
      Addressables.LoadAssetAsync<GameObject>((object) this.JumpIndicatorPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        this.jumpIndicatorLoaded = obj.Result;
        this.jumpIndicatorLoaded.CreatePool(1, true);
      });
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.summonPrefab);
    asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.summonObjectLoaded = obj.Result;
      this.loadedAddressableAssets.Add(obj);
      this.summonObjectLoaded.CreatePool(24, true);
    });
    asyncOperationHandle.WaitForCompletion();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this.currentJumpIndicator != (UnityEngine.Object) null)
    {
      ObjectPool.Recycle(this.currentJumpIndicator);
      this.currentJumpIndicator = (GameObject) null;
    }
    if (this.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      this.loadedAddressableAssets.Clear();
    }
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    this.CleanupEventInstances();
  }

  public void CleanupEventInstances()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.spitProjectileInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.jumpStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if ((UnityEngine.Object) this.MortarLineDamageColliderEvents != (UnityEngine.Object) null)
    {
      this.MortarLineDamageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.MortarLineDamageColliderEvents.SetActive(false);
    }
    if ((UnityEngine.Object) this.JumpDamageColliderEvents != (UnityEngine.Object) null)
    {
      this.JumpDamageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.JumpDamageColliderEvents.SetActive(false);
    }
    this.initialDamageModifier = this.health.DamageModifier;
    if (!this.CheckAndTrySummon())
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
    this.restoreArmorElapsed = this.timeToRestoreArmor;
    this.ResetMainAttackCooldown();
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    if ((UnityEngine.Object) this.MortarLineDamageColliderEvents != (UnityEngine.Object) null)
      this.MortarLineDamageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    if ((UnityEngine.Object) this.JumpDamageColliderEvents != (UnityEngine.Object) null)
      this.JumpDamageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
  }

  public void DoLegLiftSpineEvent()
  {
    if (string.IsNullOrEmpty(this.MoveSingleStompSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.MoveSingleStompSFX);
  }

  public void RefreshArmorVisuals(bool armored)
  {
    this.Spine.Skeleton.SetSkin(armored ? "base" : "burnt");
    this.Spine.Skeleton.SetToSetupPose();
  }

  public void ToggleArmor(bool toggle)
  {
    this.armored = toggle;
    this.restoreArmorElapsed = toggle ? this.timeToRestoreArmor : 0.0f;
    this.health.DamageModifier = toggle ? this.armorDamageMultiplier : this.initialDamageModifier;
    this.bombsImpacted = 0.0f;
    this.RefreshArmorVisuals(toggle);
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
    this.CleanupEventInstances();
    this.noMoreSpawns = true;
    foreach (Health children in this.ChildrenList)
    {
      if ((UnityEngine.Object) children != (UnityEngine.Object) null && (double) children.HP > 0.0)
        children.DealDamage(999f, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
    }
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.SimpleSpineFlash.FlashWhite(false);
    this.UsePathing = true;
    this.health.invincible = false;
    this.DisableForces = false;
    this.SimpleSpineFlash.FlashFillRed();
    this.CheckAndTrySummon();
  }

  public float CurrentHealthPercent
  {
    get => (float) ((double) this.health.CurrentHP / (double) this.health.totalHP * 100.0);
  }

  public bool CheckAndTrySummon()
  {
    if (this.noMoreSpawns || this.summonStack.Count <= 0 || (double) this.summonStack.Peek().Percent < (double) this.CurrentHealthPercent || (double) this.CurrentHealthPercent <= 1.0 || this.state.CURRENT_STATE == StateMachine.State.SignPostAttack || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack)
      return false;
    this.StopAllCoroutines();
    if ((UnityEngine.Object) this.currentJumpIndicator != (UnityEngine.Object) null)
    {
      ObjectPool.Recycle(this.currentJumpIndicator);
      this.currentJumpIndicator = (GameObject) null;
    }
    this.StartCoroutine((IEnumerator) this.DoSummon(this.summonStack.Peek().Quantity));
    this.summonStack.Pop();
    return true;
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.DisableForces = true;
    enemyLambKing.Force = (enemyLambKing.transform.position - Attacker.transform.position).normalized * 500f;
    enemyLambKing.rb.AddForce((Vector2) (enemyLambKing.Force * enemyLambKing.KnockbackModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLambKing.DisableForces = false;
  }

  public IEnumerator WaitForTarget()
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemyLambKing.GetClosestTarget(enemyLambKing.health.team == Health.Team.PlayerTeam, ignoreNonUnits: true);
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemyLambKing.TargetObject = closestTarget.gameObject;
        enemyLambKing.requireLineOfSite = false;
        enemyLambKing.VisionRange = int.MaxValue;
      }
      enemyLambKing.RepathTimer -= Time.deltaTime * enemyLambKing.Spine.timeScale;
      if ((double) enemyLambKing.RepathTimer <= 0.0)
      {
        if (enemyLambKing.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyLambKing.Spine.AnimationName != "move")
            enemyLambKing.Spine.AnimationState.SetAnimation(0, "move", true);
        }
        else if (enemyLambKing.Spine.AnimationName != "idle")
          enemyLambKing.Spine.AnimationState.SetAnimation(0, "idle", true);
        enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
      }
      yield return (object) null;
    }
    enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.ChasePlayer());
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null || (double) this.Spine.timeScale < 1.0 / 1000.0)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
  }

  public void LookAtTarget(GameObject target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null || (double) this.Spine.timeScale < 1.0 / 1000.0)
      return;
    float angle = Utils.GetAngle(this.transform.position, target.transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
  }

  public void LookAtTarget(Vector3 targetPosition)
  {
    if ((double) this.Spine.timeScale < 1.0 / 1000.0)
      return;
    float angle = Utils.GetAngle(this.transform.position, targetPosition);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
  }

  public IEnumerator ChasePlayer()
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.MyState = EnemyLambKing.State.WaitAndTaunt;
    enemyLambKing.state.CURRENT_STATE = StateMachine.State.Idle;
    bool repositionedThisTime = false;
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null)
      {
        enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
        break;
      }
      if ((UnityEngine.Object) enemyLambKing.MortarLineDamageColliderEvents != (UnityEngine.Object) null)
        enemyLambKing.MortarLineDamageColliderEvents.SetActive(false);
      if ((UnityEngine.Object) enemyLambKing.JumpDamageColliderEvents != (UnityEngine.Object) null)
        enemyLambKing.JumpDamageColliderEvents.SetActive(false);
      enemyLambKing.ProcessDelayCooldowns();
      if (enemyLambKing.MyState == EnemyLambKing.State.WaitAndTaunt)
      {
        if (enemyLambKing.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyLambKing.Spine.AnimationName != "move")
            enemyLambKing.Spine.AnimationState.SetAnimation(0, "move", true);
        }
        else if (enemyLambKing.Spine.AnimationName != "idle")
          enemyLambKing.Spine.AnimationState.SetAnimation(0, "idle", true);
        if ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) PlayerFarming.Instance.gameObject && enemyLambKing.health.IsCharmed && (UnityEngine.Object) enemyLambKing.GetClosestTarget() != (UnityEngine.Object) null)
          enemyLambKing.TargetObject = enemyLambKing.GetClosestTarget().gameObject;
        enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
        switch (enemyLambKing.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
          case StateMachine.State.Moving:
            if ((double) enemyLambKing.MaxMainAttackDelay <= 0.0 || (double) enemyLambKing.MainAttackDelay <= 0.0)
            {
              enemyLambKing.lastAttack = enemyLambKing.GetRandomAttack(enemyLambKing.GetNextAttacks(enemyLambKing.lastAttack));
              switch (enemyLambKing.lastAttack)
              {
                case EnemyLambKing.LambKingAttack.MortarLine:
                  enemyLambKing.TryExecuteAttack(enemyLambKing.MineLineAttack());
                  break;
                case EnemyLambKing.LambKingAttack.MortarCross:
                  enemyLambKing.TryExecuteAttack(enemyLambKing.MortarCrossAttack());
                  break;
                case EnemyLambKing.LambKingAttack.MortarRain:
                  enemyLambKing.TryExecuteAttack(enemyLambKing.MortarRainAttack());
                  break;
                case EnemyLambKing.LambKingAttack.CircleProjectiles:
                  enemyLambKing.TryExecuteAttack(enemyLambKing.CircleProjectilesAttack());
                  break;
                case EnemyLambKing.LambKingAttack.ShotgunProjectiles:
                  enemyLambKing.TryExecuteAttack(enemyLambKing.ShotgunProjectilesAttack());
                  break;
                case EnemyLambKing.LambKingAttack.JumpAttack:
                  enemyLambKing.TryExecuteJumpAttack(enemyLambKing.JumpAttack());
                  break;
                default:
                  enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
                  break;
              }
            }
            else
            {
              if (!repositionedThisTime)
              {
                enemyLambKing.Angle = (float) (((double) Utils.GetAngle(enemyLambKing.TargetObject.transform.position, enemyLambKing.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                enemyLambKing.TargetPosition = Vector3.zero + new Vector3(enemyLambKing.RepositionDistanceFromCenter * Mathf.Cos(enemyLambKing.Angle), enemyLambKing.RepositionDistanceFromCenter * Mathf.Sin(enemyLambKing.Angle));
                enemyLambKing.LookAtTarget(enemyLambKing.TargetPosition);
                enemyLambKing.FindPath(enemyLambKing.TargetPosition);
                repositionedThisTime = true;
                break;
              }
              break;
            }
            break;
        }
      }
      yield return (object) null;
    }
  }

  public List<EnemyLambKing.LambKingAttackChance> GetNextAttacks(
    EnemyLambKing.LambKingAttack lambKingAttack)
  {
    if (lambKingAttack == EnemyLambKing.LambKingAttack.JumpAttack)
      return this.mainAttackChances;
    foreach (EnemyLambKing.LambKingAttackChance mainAttackChance in this.mainAttackChances)
    {
      if (mainAttackChance.Attack == lambKingAttack)
        return this.secondaryAttackChances;
    }
    foreach (EnemyLambKing.LambKingAttackChance secondaryAttackChance in this.secondaryAttackChances)
    {
      if (secondaryAttackChance.Attack == lambKingAttack)
        return this.jumpAttackChances;
    }
    return this.mainAttackChances;
  }

  public EnemyLambKing.LambKingAttack GetRandomAttack(
    List<EnemyLambKing.LambKingAttackChance> attackChances)
  {
    if (attackChances.Count == 0)
      return EnemyLambKing.LambKingAttack.None;
    int num = UnityEngine.Random.Range(0, 100);
    foreach (EnemyLambKing.LambKingAttackChance attackChance in attackChances)
    {
      if ((double) num < (double) attackChance.chance)
        return attackChance.Attack;
    }
    return attackChances[UnityEngine.Random.Range(0, attackChances.Count)].Attack;
  }

  public void ProcessDelayCooldowns()
  {
    this.MainAttackDelay -= Time.deltaTime * this.Spine.timeScale;
    this.MaxMainAttackDelay -= Time.deltaTime * this.Spine.timeScale;
  }

  public void TryExecuteAttack(IEnumerator attackRoutine)
  {
    if (!(bool) (UnityEngine.Object) this.TargetObject)
      return;
    this.health.invincible = false;
    this.StopAllCoroutines();
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StartCoroutine((IEnumerator) attackRoutine);
  }

  public void TryExecuteJumpAttack(IEnumerator attackRoutine)
  {
    if (!(bool) (UnityEngine.Object) this.TargetObject || (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < (double) this.minimumJumpDistance)
      return;
    this.health.invincible = false;
    this.StopAllCoroutines();
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StartCoroutine((IEnumerator) attackRoutine);
  }

  public IEnumerator MortarCrossAttack()
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.MyState = EnemyLambKing.State.Attacking;
    float SignPostDelay = enemyLambKing.mortarCrossAttackWindup;
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyLambKing.Spine == (UnityEngine.Object) null || enemyLambKing.Spine.AnimationState == null || enemyLambKing.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        float shotsFired = 0.0f;
        switch (enemyLambKing.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) enemyLambKing.TargetObject)
            {
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
              enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-2-start", false);
              if (!string.IsNullOrEmpty(enemyLambKing.AttackOrganCStartSFX))
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackOrganCStartSFX);
              if (!string.IsNullOrEmpty(enemyLambKing.AttackOrganCStartVO))
              {
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackOrganCStartVO);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null)
              enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
            enemyLambKing.SimpleSpineFlash.FlashWhite(enemyLambKing.state.Timer / SignPostDelay);
            enemyLambKing.state.Timer += Time.deltaTime * enemyLambKing.Spine.timeScale;
            if ((double) enemyLambKing.state.Timer >= (double) SignPostDelay)
            {
              while ((double) shotsFired < (double) enemyLambKing.crossShotsPerRow)
              {
                enemyLambKing.SimpleSpineFlash.FlashWhite(false);
                enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-2-loop", false);
                CameraManager.shakeCamera(0.4f, enemyLambKing.state.LookAngle);
                enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
                enemyLambKing.FireCrossPattern(enemyLambKing.distanceBetweenCrossShots * (enemyLambKing.crossShotsPerRow - shotsFired), enemyLambKing.crossAngle);
                ++shotsFired;
                float time = 0.0f;
                while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) enemyLambKing.timeBetweenCrossShots)
                  yield return (object) null;
                yield return (object) null;
              }
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            enemyLambKing.MainAttackDelay = UnityEngine.Random.Range(enemyLambKing.MainAttackDelayRandomRange.x, enemyLambKing.MainAttackDelayRandomRange.y);
            enemyLambKing.MaxMainAttackDelay = UnityEngine.Random.Range(enemyLambKing.MaxMainAttackDelayRandomRange.x, enemyLambKing.MaxMainAttackDelayRandomRange.y);
            enemyLambKing.Spine.YieldForAnimation("attack-2-end");
            Loop = false;
            enemyLambKing.SimpleSpineFlash.FlashWhite(false);
            break;
          default:
            enemyLambKing.TargetObject = (GameObject) null;
            enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
            yield break;
        }
        yield return (object) null;
      }
    }
    enemyLambKing.ResetMainAttackCooldown();
    enemyLambKing.TargetObject = (GameObject) null;
    enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
  }

  public IEnumerator MortarRainAttack()
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.MyState = EnemyLambKing.State.Attacking;
    float SignPostDelay = enemyLambKing.mortarRainAttackWindup;
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyLambKing.Spine == (UnityEngine.Object) null || enemyLambKing.Spine.AnimationState == null || enemyLambKing.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        switch (enemyLambKing.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) enemyLambKing.TargetObject)
            {
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-3-start", false);
              enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
              if (!string.IsNullOrEmpty(enemyLambKing.AttackOrganBStartSFX))
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackOrganBStartSFX);
              if (!string.IsNullOrEmpty(enemyLambKing.AttackOrganBStartVO))
              {
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackOrganBStartVO);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null)
              enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
            enemyLambKing.SimpleSpineFlash.FlashWhite(enemyLambKing.state.Timer / SignPostDelay);
            enemyLambKing.state.Timer += Time.deltaTime * enemyLambKing.Spine.timeScale;
            if ((double) enemyLambKing.state.Timer >= (double) SignPostDelay)
            {
              float rainElapsedTime = 0.0f;
              enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-3-loop", true);
              while ((double) rainElapsedTime <= (double) enemyLambKing.mortarRainAttackDuration)
              {
                enemyLambKing.SimpleSpineFlash.FlashWhite(false);
                CameraManager.shakeCamera(0.4f, enemyLambKing.state.LookAngle);
                enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
                Vector3 vector3 = (Vector3) (UnityEngine.Random.insideUnitCircle * enemyLambKing.mortarRainAreaRadius);
                Vector3 closestPoint;
                if (!BiomeGenerator.PointWithinIsland(vector3, out closestPoint))
                  vector3 = closestPoint;
                enemyLambKing.ShootMortarProjectile(vector3, enemyLambKing.rainMortarTrajectoryDuration);
                float time = 0.0f;
                while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) enemyLambKing.timeBetweenRainShots)
                  yield return (object) null;
                rainElapsedTime += time;
                yield return (object) null;
              }
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            enemyLambKing.Spine.YieldForAnimation("attack-3-end");
            enemyLambKing.SimpleSpineFlash.FlashWhite(false);
            Loop = false;
            break;
          default:
            enemyLambKing.TargetObject = (GameObject) null;
            enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
            yield break;
        }
        yield return (object) null;
      }
    }
    enemyLambKing.ResetMainAttackCooldown();
    enemyLambKing.TargetObject = (GameObject) null;
    enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
  }

  public IEnumerator MineLineAttack()
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.MyState = EnemyLambKing.State.Attacking;
    bool Loop = true;
    float SignPostDelay = enemyLambKing.mortarLineAttackWindup;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyLambKing.Spine == (UnityEngine.Object) null || enemyLambKing.Spine.AnimationState == null || enemyLambKing.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        Vector3 playerDirection;
        Vector3 aimPosition;
        switch (enemyLambKing.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) enemyLambKing.TargetObject)
            {
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-1", true);
              enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
              if (!string.IsNullOrEmpty(enemyLambKing.AttackOrganAStartSFX))
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackOrganAStartSFX, enemyLambKing.transform.position);
              if (!string.IsNullOrEmpty(enemyLambKing.AttackOrganAStartVO))
              {
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackOrganAStartVO, enemyLambKing.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyLambKing.MortarLineDamageColliderEvents != (UnityEngine.Object) null)
              enemyLambKing.MortarLineDamageColliderEvents.SetActive(false);
            enemyLambKing.SimpleSpineFlash.FlashWhite(enemyLambKing.state.Timer / SignPostDelay);
            enemyLambKing.state.Timer += Time.deltaTime * enemyLambKing.Spine.timeScale;
            playerDirection = (enemyLambKing.transform.position - enemyLambKing.TargetObject.transform.position).normalized;
            aimPosition = enemyLambKing.TargetObject.transform.position;
            if ((double) Vector3.Distance(aimPosition, enemyLambKing.transform.position) < (double) enemyLambKing.mineLineMinimumAimDistance)
            {
              playerDirection = -playerDirection;
              Vector3 vector3 = enemyLambKing.transform.position + playerDirection * enemyLambKing.mineLineMinimumAimDistance;
              if (BiomeGenerator.PointWithinIsland(vector3 + playerDirection * enemyLambKing.LineTargetDistanceFromPlayer, out Vector3 _))
                aimPosition = vector3;
              else
                playerDirection = -playerDirection;
            }
            if ((double) enemyLambKing.state.Timer >= (double) SignPostDelay)
            {
              float[] numArray = enemyLambKing.mortarAngles;
              for (int index = 0; index < numArray.Length; ++index)
              {
                float angle = numArray[index];
                if ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null)
                  yield return (object) null;
                enemyLambKing.SimpleSpineFlash.FlashWhite(false);
                Vector3 vector3 = (Vector3) EnemyLambKing.RotateVector((Vector2) playerDirection, angle);
                enemyLambKing.shootPosition = aimPosition + vector3 * enemyLambKing.LineTargetDistanceFromPlayer;
                enemyLambKing.ShootMortarProjectile(enemyLambKing.shootPosition, enemyLambKing.lineMortarTrajectoryDuration);
                if (enemyLambKing.mineLineMeleeDamage)
                  enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.EnableSlashDamageCollider(0.0f));
                CameraManager.shakeCamera(0.4f, enemyLambKing.state.LookAngle);
                float time = 0.0f;
                while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) enemyLambKing.timeBetweenLineShots)
                  yield return (object) null;
              }
              numArray = (float[]) null;
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            enemyLambKing.SimpleSpineFlash.FlashWhite(false);
            Loop = false;
            break;
        }
        playerDirection = new Vector3();
        aimPosition = new Vector3();
        yield return (object) null;
      }
    }
    enemyLambKing.ResetMainAttackCooldown();
    enemyLambKing.TargetObject = (GameObject) null;
    enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
  }

  public IEnumerator JumpAttack()
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.MyState = EnemyLambKing.State.Attacking;
    float SignPostDelay = enemyLambKing.JumpAttackWindup;
    bool Loop = true;
    enemyLambKing.ClearPaths();
    while (Loop)
    {
      if ((UnityEngine.Object) enemyLambKing.Spine == (UnityEngine.Object) null || enemyLambKing.Spine.AnimationState == null || enemyLambKing.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        float time;
        switch (enemyLambKing.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) enemyLambKing.TargetObject)
            {
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyLambKing.Spine.AnimationState.SetAnimation(0, "jump-start", false);
              enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
              if ((UnityEngine.Object) enemyLambKing.jumpIndicatorLoaded != (UnityEngine.Object) null)
              {
                enemyLambKing.currentJumpIndicator = ObjectPool.Spawn(enemyLambKing.jumpIndicatorLoaded);
                enemyLambKing.currentJumpIndicator.transform.position = enemyLambKing.TargetObject.transform.position + Vector3.back * 0.1f;
                enemyLambKing.currentJumpIndicator.transform.parent = enemyLambKing.transform.parent;
              }
              if (!string.IsNullOrEmpty(enemyLambKing.AttackJumpStartSFX))
                enemyLambKing.jumpStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyLambKing.AttackJumpStartSFX, enemyLambKing.transform);
              if (!string.IsNullOrEmpty(enemyLambKing.WarningVO))
              {
                AudioManager.Instance.PlayOneShot(enemyLambKing.WarningVO);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null)
              enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
            enemyLambKing.SimpleSpineFlash.FlashWhite(enemyLambKing.state.Timer / SignPostDelay);
            enemyLambKing.state.Timer += Time.deltaTime * enemyLambKing.Spine.timeScale;
            if ((double) enemyLambKing.state.Timer < (double) SignPostDelay && (UnityEngine.Object) enemyLambKing.currentJumpIndicator != (UnityEngine.Object) null)
              enemyLambKing.currentJumpIndicator.transform.position = Vector3.MoveTowards(enemyLambKing.currentJumpIndicator.transform.position, enemyLambKing.TargetObject.transform.position + Vector3.back * 0.1f, enemyLambKing.jumpAimingSpeed * Time.deltaTime * enemyLambKing.Spine.timeScale);
            if ((double) enemyLambKing.state.Timer >= (double) SignPostDelay)
            {
              enemyLambKing.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, enemyLambKing.state.LookAngle);
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              enemyLambKing.Spine.AnimationState.SetAnimation(0, "jump-impact", false);
              Vector3 startPos = enemyLambKing.transform.position;
              Vector3 targetPos = enemyLambKing.currentJumpIndicator.transform.position;
              float epsilon = enemyLambKing.bodyCollider.radius + 0.1f;
              if ((double) Vector3.Distance(startPos, targetPos) < (double) enemyLambKing.minimumJumpDistance)
              {
                Vector3 point = startPos + (targetPos - enemyLambKing.transform.position).normalized * enemyLambKing.minimumJumpDistance;
                if (BiomeGenerator.PointWithinIsland(point, out Vector3 _))
                  targetPos = point;
              }
              targetPos = enemyLambKing.CheckForMapEdgeOnLanding(targetPos, startPos, epsilon);
              targetPos = enemyLambKing.CheckForMapEdgeOnLanding(targetPos, startPos, epsilon);
              enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
              time = 0.0f;
              while ((double) time < (double) enemyLambKing.JumpAttackDuration)
              {
                if (!PlayerRelic.TimeFrozen)
                {
                  time += Time.deltaTime * enemyLambKing.Spine.timeScale;
                  enemyLambKing.transform.position = Vector3.Lerp(startPos, targetPos, enemyLambKing.jumpFlightCurve.Evaluate(time / enemyLambKing.JumpAttackDuration));
                }
                yield return (object) null;
              }
              if (!string.IsNullOrEmpty(enemyLambKing.AttackVO))
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackVO, enemyLambKing.transform.position);
              startPos = new Vector3();
              targetPos = new Vector3();
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            CameraManager.shakeCamera(enemyLambKing.jumpLandShakeIntensity, 270f);
            int shotsFired = 0;
            if ((UnityEngine.Object) enemyLambKing.currentJumpIndicator != (UnityEngine.Object) null)
            {
              ObjectPool.Recycle(enemyLambKing.currentJumpIndicator);
              enemyLambKing.currentJumpIndicator = (GameObject) null;
            }
            Explosion.CreateExplosion(enemyLambKing.transform.position, enemyLambKing.health.team, enemyLambKing.health, 2f, 0.0f, shakeMultiplier: 2f);
            BiomeConstants.Instance.EmitSmokeExplosionVFX(enemyLambKing.transform.position);
            MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
            enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.EnableJumpDamageCollider(0.0f));
            while (shotsFired < enemyLambKing.landingAttack.attackWaves.Length)
            {
              enemyLambKing.Spine.AnimationState.SetAnimation(0, "jump-end", false);
              enemyLambKing.SimpleSpineFlash.FlashWhite(false);
              enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
              EnemyLambKing.LambKingProjectileCircleWave attackWave = enemyLambKing.landingAttack.attackWaves[shotsFired];
              enemyLambKing.FireCircleProjectilePattern(attackWave.Projectiles, attackWave.Offseted, attackWave.ProjectileSpeed, enemyLambKing.transform.position);
              if (!string.IsNullOrEmpty(enemyLambKing.attackSoundPath))
                AudioManager.Instance.PlayOneShot(enemyLambKing.attackSoundPath, enemyLambKing.transform.position);
              if (!string.IsNullOrEmpty(enemyLambKing.AttackVO))
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackVO, enemyLambKing.transform.position);
              ++shotsFired;
              time = 0.0f;
              while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) enemyLambKing.landingAttack.TimeBetweenShots)
                yield return (object) null;
              yield return (object) null;
            }
            time = 0.0f;
            while ((double) time < (double) enemyLambKing.afterJumpWaitTime)
            {
              time += Time.deltaTime * enemyLambKing.Spine.timeScale;
              yield return (object) null;
            }
            Loop = false;
            enemyLambKing.SimpleSpineFlash.FlashWhite(false);
            break;
          default:
            enemyLambKing.TargetObject = (GameObject) null;
            enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
            yield break;
        }
        yield return (object) null;
      }
    }
    enemyLambKing.Seperate(0.5f);
    enemyLambKing.TargetObject = (GameObject) null;
    enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
  }

  public Vector3 CheckForMapEdgeOnLanding(Vector3 targetPos, Vector3 startPos, float epsilon)
  {
    Vector3 normalized = (targetPos - startPos).normalized;
    List<Vector3> checkDirs = new List<Vector3>()
    {
      normalized,
      (Vector3) EnemyLambKing.RotateVector((Vector2) normalized, 90f).normalized,
      (Vector3) EnemyLambKing.RotateVector((Vector2) normalized, 180f).normalized,
      (Vector3) EnemyLambKing.RotateVector((Vector2) normalized, 270f).normalized
    };
    List<Vector3> source = this.CheckPointAtDirections(epsilon, checkDirs, targetPos);
    if (source.Count == checkDirs.Count)
      return targetPos;
    List<Vector3> list = source.Where<Vector3>((Func<Vector3, bool>) (p => this.CheckPointAtDirections(epsilon, checkDirs, p).Count >= checkDirs.Count)).ToList<Vector3>();
    return list.Count <= 0 ? this.transform.position : list.RandomElement<Vector3>();
  }

  public List<Vector3> CheckPointAtDirections(
    float epsilon,
    List<Vector3> checkDirs,
    Vector3 startPosition)
  {
    List<Vector3> vector3List = new List<Vector3>();
    foreach (Vector3 point in checkDirs.Select<Vector3, Vector3>((Func<Vector3, Vector3>) (d => startPosition + d * epsilon)))
    {
      if (BiomeGenerator.PointWithinIsland(point, out Vector3 _))
        vector3List.Add(point);
    }
    return vector3List;
  }

  public IEnumerator CircleProjectilesAttack()
  {
    EnemyLambKing enemyLambKing = this;
    float SignPostDelay = enemyLambKing.shotgunAttackWindup;
    enemyLambKing.SimpleSpineFlash.FlashWhite(enemyLambKing.state.Timer / SignPostDelay);
    bool loop = true;
    while (loop)
    {
      if ((UnityEngine.Object) enemyLambKing.Spine == (UnityEngine.Object) null || enemyLambKing.Spine.AnimationState == null || enemyLambKing.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        if ((double) enemyLambKing.circlePatternStack.Peek().PercentageFrom > (double) enemyLambKing.CurrentHealthPercent && enemyLambKing.circlePatternStack.Count > 1)
          enemyLambKing.circlePatternStack.Pop();
        EnemyLambKing.LambKingProjectileCirclePatternAttack pattern = enemyLambKing.circlePatternStack.Peek();
        int shotsFired = 0;
        switch (enemyLambKing.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) enemyLambKing.TargetObject)
            {
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
              if (!string.IsNullOrEmpty(enemyLambKing.AttackSlamProjectileSFX))
                AudioManager.Instance.PlayOneShot(enemyLambKing.AttackSlamProjectileSFX, enemyLambKing.gameObject);
              if (!string.IsNullOrEmpty(enemyLambKing.WarningVO))
              {
                AudioManager.Instance.PlayOneShot(enemyLambKing.WarningVO);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null)
              enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
            enemyLambKing.SimpleSpineFlash.FlashWhite(enemyLambKing.state.Timer / SignPostDelay);
            enemyLambKing.state.Timer += Time.deltaTime * enemyLambKing.Spine.timeScale;
            if ((double) enemyLambKing.state.Timer >= (double) SignPostDelay)
            {
              enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-4", false);
              while (shotsFired < pattern.attackWaves.Length)
              {
                enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-4", false);
                float time = 0.0f;
                while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) pattern.TimeToShootFromAnimStart)
                  yield return (object) null;
                enemyLambKing.SimpleSpineFlash.FlashWhite(false);
                CameraManager.shakeCamera(enemyLambKing.circleAttackShakeIntensity, 270f);
                if ((UnityEngine.Object) enemyLambKing.TargetObject != (UnityEngine.Object) null)
                  enemyLambKing.LookAtTarget(enemyLambKing.TargetObject);
                EnemyLambKing.LambKingProjectileCircleWave attackWave = pattern.attackWaves[shotsFired];
                enemyLambKing.FireCircleProjectilePattern(attackWave.Projectiles, attackWave.Offseted, attackWave.ProjectileSpeed, enemyLambKing.transform.position);
                if (!string.IsNullOrEmpty(enemyLambKing.attackSoundPath))
                  AudioManager.Instance.PlayOneShot(enemyLambKing.attackSoundPath, enemyLambKing.transform.position);
                if (!string.IsNullOrEmpty(enemyLambKing.AttackVO))
                  AudioManager.Instance.PlayOneShot(enemyLambKing.AttackVO, enemyLambKing.transform.position);
                ++shotsFired;
                while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) pattern.TimeBetweenShots)
                  yield return (object) null;
                yield return (object) null;
              }
              enemyLambKing.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            loop = false;
            enemyLambKing.SimpleSpineFlash.FlashWhite(false);
            break;
          default:
            enemyLambKing.TargetObject = (GameObject) null;
            enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
            yield break;
        }
        yield return (object) null;
        pattern = (EnemyLambKing.LambKingProjectileCirclePatternAttack) null;
      }
    }
    enemyLambKing.ResetMainAttackCooldown();
    enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
  }

  public IEnumerator ShotgunProjectilesAttack()
  {
    EnemyLambKing enemyLambKing = this;
    int shots = (int) UnityEngine.Random.Range(enemyLambKing.shotgunProjectilesPerShot.x, enemyLambKing.shotgunProjectilesPerShot.y);
    float radius = enemyLambKing.shotgunProjectileRadius;
    int amount = enemyLambKing.shotgunShots;
    float SignPostDelay = enemyLambKing.shotgunAttackWindup;
    GameObject target = (UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null ? PlayerFarming.Instance.gameObject : enemyLambKing.TargetObject;
    if ((UnityEngine.Object) enemyLambKing.TargetObject == (UnityEngine.Object) null)
    {
      enemyLambKing.StopAllCoroutines();
      enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
    }
    Vector3 targetLastPosition = enemyLambKing.TargetObject.transform.position;
    enemyLambKing.SimpleSpineFlash.FlashWhite(enemyLambKing.state.Timer / SignPostDelay);
    float windupTimeElapsed = 0.0f;
    while ((double) (windupTimeElapsed += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) SignPostDelay)
      yield return (object) null;
    if (!string.IsNullOrEmpty(enemyLambKing.AttackSpitProjectileStartSFX))
      enemyLambKing.spitProjectileInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyLambKing.AttackSpitProjectileStartSFX, enemyLambKing.transform);
    if (!string.IsNullOrEmpty(enemyLambKing.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyLambKing.WarningVO);
    for (int t = 0; t < amount; ++t)
    {
      enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-5", false);
      float betweenShotsTime = 0.0f;
      if ((UnityEngine.Object) target != (UnityEngine.Object) null)
      {
        enemyLambKing.LookAtTarget(target);
        targetLastPosition = target.transform.position;
      }
      else
        enemyLambKing.LookAtTarget(targetLastPosition);
      while ((double) (betweenShotsTime += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) enemyLambKing.shotgunTimeToSpawnBullets)
        yield return (object) null;
      for (int index = 0; index < shots; ++index)
      {
        if ((UnityEngine.Object) enemyLambKing.shotgunProjectileOrigin.gameObject != (UnityEngine.Object) null)
          enemyLambKing.ShootProjectile(radius, targetLastPosition, enemyLambKing.shotgunProjectileSpeed, enemyLambKing.shotgunProjectileOrigin.position);
      }
      while ((double) (betweenShotsTime += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) enemyLambKing.shotgunTimeBetweenShots)
        yield return (object) null;
    }
    enemyLambKing.ResetMainAttackCooldown();
    enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
  }

  public void ShootProjectile(
    float radius,
    Vector3 targetPosition,
    float projectileSpeed,
    Vector3 origin)
  {
    float num1 = 90f;
    float angle = Utils.GetAngle(origin, targetPosition);
    float current = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? 0.0f : 180f;
    float num2 = num1 * 0.5f;
    float num3 = Mathf.DeltaAngle(current, angle);
    float num4 = angle;
    if ((double) num3 < -(double) num2)
      num4 = current - num2;
    else if ((double) num3 > (double) num2)
      num4 = current + num2;
    Vector3 vector3 = (Vector3) (UnityEngine.Random.insideUnitCircle * radius);
    Projectile component = ObjectPool.Spawn(this.shotgunProjectilePrefab, origin).GetComponent<Projectile>();
    component.transform.position = origin + vector3;
    component.Angle = num4;
    component.team = this.health.team;
    component.Speed = projectileSpeed;
    component.LifeTime = 4f + UnityEngine.Random.Range(0.0f, 0.3f);
    component.Owner = this.health;
  }

  public void ShootProjectile(float radius, float angle, float projectileSpeed, Vector3 origin)
  {
    Vector3 vector3 = (Vector3) (UnityEngine.Random.insideUnitCircle * radius);
    Projectile component = ObjectPool.Spawn(this.shotgunProjectilePrefab, origin).GetComponent<Projectile>();
    component.transform.position = origin + vector3;
    component.Angle = angle;
    component.team = this.health.team;
    component.Speed = projectileSpeed;
    component.LifeTime = 4f + UnityEngine.Random.Range(0.0f, 0.3f);
    component.Owner = this.health;
  }

  public void ShootMortarProjectile(Vector3 spawnPosition, float trajectoryDuration)
  {
    KnockableMortarBomb bomb = ObjectPool.Spawn(this.mortarProjectilePrefab, spawnPosition, Quaternion.identity).GetComponent<KnockableMortarBomb>();
    bomb.transform.parent = this.transform.parent;
    if (this.BombsSpawnProjectilePattern)
      bomb.OnExplode += (System.Action) (() =>
      {
        if (bomb.IsHit && (!bomb.IsHit || !this.hitBombsSpawnPattern))
          return;
        this.StartCoroutine((IEnumerator) this.FireBombProjectilePattern(bomb.transform));
      });
    bomb.Play(this.transform.position + new Vector3(0.0f, 0.0f, -1.5f), trajectoryDuration, this.health, false);
    if (string.IsNullOrEmpty(this.AttackOrganLaunchSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.AttackOrganLaunchSFX);
  }

  public IEnumerator FireBombProjectilePattern(Transform bombTransform)
  {
    EnemyLambKing enemyLambKing = this;
    CameraManager.shakeCamera(0.4f, enemyLambKing.state.LookAngle);
    int shotsFired = 0;
    while (shotsFired < enemyLambKing.bombProjectilePattern.attackWaves.Length)
    {
      EnemyLambKing.LambKingProjectileCircleWave attackWave = enemyLambKing.bombProjectilePattern.attackWaves[shotsFired];
      enemyLambKing.FireCircleProjectilePattern(attackWave.Projectiles, attackWave.Offseted, attackWave.ProjectileSpeed, bombTransform.position);
      ++shotsFired;
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) enemyLambKing.bombProjectilePattern.TimeBetweenShots)
        yield return (object) null;
      yield return (object) null;
    }
  }

  public void FireCrossPattern(float distanceFromCenter, float angle)
  {
    foreach (Vector2 vector2 in new List<Vector2>()
    {
      EnemyLambKing.RotateVector(Vector2.up, angle).normalized,
      EnemyLambKing.RotateVector(Vector2.down, angle).normalized,
      EnemyLambKing.RotateVector(Vector2.left, angle).normalized,
      EnemyLambKing.RotateVector(Vector2.right, angle).normalized
    })
    {
      Vector3 vector3 = (Vector3) ((Vector2) this.transform.position + vector2 * distanceFromCenter);
      if (BiomeGenerator.PointWithinIsland(vector3, out Vector3 _))
        this.ShootMortarProjectile(vector3, this.crossMortarTrajectoryDuration);
    }
  }

  public void FireCircleProjectilePattern(
    int quantity,
    bool spaced,
    float projectileSpeed,
    Vector3 origin)
  {
    float num1 = 360f / (float) quantity;
    float num2 = 0.0f;
    for (int index = 0; index < quantity; ++index)
    {
      this.ShootProjectile(0.0f, num2 + (spaced ? num1 / 2f : 0.0f), projectileSpeed, origin);
      num2 += num1;
    }
  }

  public IEnumerator DoSummon(int spawnCount)
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.Spine.AnimationState.SetAnimation(0, "attack-6", false);
    if (!string.IsNullOrEmpty(enemyLambKing.AttackFleshSwarmerSpawnSFX))
      AudioManager.Instance.PlayOneShot(enemyLambKing.AttackFleshSwarmerSpawnSFX);
    if (!string.IsNullOrEmpty(enemyLambKing.AttackFleshSwarmerSpawnVO))
      AudioManager.Instance.PlayOneShot(enemyLambKing.AttackFleshSwarmerSpawnVO);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) enemyLambKing.summonDelay)
      yield return (object) null;
    if ((double) enemyLambKing.CurrentHealthPercent >= 1.0)
    {
      for (; spawnCount > 0 && (double) enemyLambKing.health.HP > 0.0; --spawnCount)
      {
        enemyLambKing.Spawning = true;
        GameObject gameObject = ObjectPool.Spawn(enemyLambKing.summonObjectLoaded, enemyLambKing.transform.parent);
        gameObject.transform.position = enemyLambKing.transform.position;
        EnemyFleshSwarmer component1 = gameObject.GetComponent<EnemyFleshSwarmer>();
        component1.SetSpawner(enemyLambKing.health);
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          component1.StartingState = EnemyFleshSwarmer.StartingStates.Wandering;
        foreach (DropLootOnDeath component2 in gameObject.GetComponents<DropLootOnDeath>())
          component2.GiveXP = false;
        foreach (Behaviour component3 in gameObject.GetComponents<DropMultipleLootOnDeath>())
          component3.enabled = false;
        Health component4 = gameObject.GetComponent<Health>();
        component4.OnDie += new Health.DieAction(enemyLambKing.OnSpawnedDie);
        enemyLambKing.ChildrenList.Add(component4);
        component4.StartCoroutine((IEnumerator) enemyLambKing.MoveTweenRoutine(gameObject.transform, 1f));
      }
      enemyLambKing.Spawning = false;
      enemyLambKing.StartCoroutine((IEnumerator) enemyLambKing.WaitForTarget());
      yield return (object) null;
    }
  }

  public static Vector2 RotateVector(Vector2 vector, float angle)
  {
    double f = (double) angle * (Math.PI / 180.0);
    float num1 = Mathf.Cos((float) f);
    float num2 = Mathf.Sin((float) f);
    return new Vector2((float) ((double) vector.x * (double) num1 - (double) vector.y * (double) num2), (float) ((double) vector.x * (double) num2 + (double) vector.y * (double) num1));
  }

  public void ResetMainAttackCooldown()
  {
    this.MainAttackDelay = UnityEngine.Random.Range(this.MainAttackDelayRandomRange.x, this.MainAttackDelayRandomRange.y);
    this.MaxMainAttackDelay = UnityEngine.Random.Range(this.MaxMainAttackDelayRandomRange.x, this.MaxMainAttackDelayRandomRange.y);
  }

  public void InitializeShotgunBullets()
  {
    ObjectPool.CreatePool(this.shotgunProjectilePrefab, (int) this.shotgunProjectilesPerShot.y * 4);
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    if (!string.IsNullOrEmpty(this.WarningVO))
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
    this.TargetObject = TargetObject;
    float a = Vector3.Distance(TargetObject.transform.position, this.transform.position);
    if ((double) a > (double) this.VisionRange)
      return;
    if (!this.requireLineOfSite || this.CheckLineOfSightOnTarget(TargetObject, TargetObject.transform.position, Mathf.Min(a, (float) this.VisionRange)))
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    else
      this.LookAtTarget();
  }

  public void FindPath(Vector3 PointToCheck)
  {
    this.RepathTimer = 0.2f;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), this.RepositionDistanceFromCenter, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) <= 1.0)
        return;
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * 0.2f);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * 0.2f;
      this.givePath(this.TargetPosition);
    }
    else if (this.FollowPlayer && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) <= 1.5)
        return;
      this.TargetPosition = PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle;
      this.givePath(this.TargetPosition);
    }
    else
    {
      this.TargetPosition = PointToCheck;
      this.givePath(PointToCheck);
    }
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.FollowPlayer)
      return;
    this.ClearPaths();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.PlaceIE());
  }

  public IEnumerator PlaceIE()
  {
    EnemyLambKing enemyLambKing = this;
    enemyLambKing.ClearPaths();
    Vector3 offset = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      enemyLambKing.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 vector3 = (PlayerFarming.Instance.transform.position + offset) with
      {
        z = 0.0f
      };
      enemyLambKing.transform.position = vector3;
      yield return (object) null;
    }
    enemyLambKing.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyLambKing.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null))
      return;
    if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  public IEnumerator EnableSlashDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.MortarLineDamageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.MortarLineDamageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      this.MortarLineDamageColliderEvents.SetActive(false);
    }
  }

  public IEnumerator EnableJumpDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.JumpDamageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.JumpDamageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.JumpHitColliderDuration)
        yield return (object) null;
      this.JumpDamageColliderEvents.SetActive(false);
    }
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], 0.2f, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], 0.2f, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }

  public void OnSpawnedDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    Victim.OnDie -= new Health.DieAction(this.OnSpawnedDie);
    this.ChildrenList.Remove(Victim);
  }

  public IEnumerator ScaleUp(Transform t, float duration)
  {
    yield return (object) new WaitForEndOfFrame();
    float Progress = 0.0f;
    float Duration = duration;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      t.localScale = Vector3.one * Mathf.SmoothStep(0.2f, 1f, Progress / Duration);
      yield return (object) null;
    }
    t.localScale = Vector3.one;
  }

  public IEnumerator MoveTweenRoutine(Transform t, float duration)
  {
    EnemyLambKing enemyLambKing = this;
    yield return (object) new WaitForEndOfFrame();
    float Progress = 0.0f;
    float Duration = duration;
    Vector3 StartPosition = enemyLambKing.transform.position;
    int num = 5;
    Vector3 TargetPosition;
    do
    {
      TargetPosition = StartPosition + (Vector3) UnityEngine.Random.insideUnitCircle.normalized * enemyLambKing.spawnDistance;
      --num;
    }
    while (!BiomeGenerator.PointWithinIsland(TargetPosition, out Vector3 _) && num >= 0);
    while ((double) (Progress += Time.deltaTime * enemyLambKing.Spine.timeScale) < (double) Duration)
    {
      t.position = Vector3.Lerp(StartPosition, TargetPosition, Progress / Duration);
      yield return (object) null;
    }
    t.position = TargetPosition;
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__108_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    this.jumpIndicatorLoaded = obj.Result;
    this.jumpIndicatorLoaded.CreatePool(1, true);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__108_1(AsyncOperationHandle<GameObject> obj)
  {
    this.summonObjectLoaded = obj.Result;
    this.loadedAddressableAssets.Add(obj);
    this.summonObjectLoaded.CreatePool(24, true);
  }

  public enum LambKingAttack
  {
    MortarLine,
    MortarCross,
    MortarRain,
    CircleProjectiles,
    ShotgunProjectiles,
    JumpAttack,
    None,
  }

  [Serializable]
  public class LambKingAttackChance
  {
    public EnemyLambKing.LambKingAttack Attack;
    public float chance;
  }

  public enum State
  {
    WaitAndTaunt,
    Teleporting,
    Attacking,
  }

  [Serializable]
  public class LambKingProjectileCirclePatternAttack
  {
    public bool PercentageDisabled;
    public float PercentageFrom = 100f;
    public float TimeBetweenShots = 0.7f;
    public float TimeToShootFromAnimStart = 0.5f;
    public EnemyLambKing.LambKingProjectileCircleWave[] attackWaves;
  }

  [Serializable]
  public class LambKingProjectileCircleWave
  {
    public float ProjectileSpeed = 0.5f;
    public int Projectiles = 8;
    public bool Offseted;
  }

  [Serializable]
  public class LambKingSummon
  {
    public int Quantity;
    public int Percent;
  }
}
