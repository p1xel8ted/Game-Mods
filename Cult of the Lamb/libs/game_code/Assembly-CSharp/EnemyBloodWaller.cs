// Decompiled with JetBrains decompiler
// Type: EnemyBloodWaller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyBloodWaller : UnitObject
{
  public AssetReferenceGameObject smallIndicatorPrefab;
  public AssetReferenceGameObject largeIndicatorPrefab;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public GameObject loadedSmallIndicatorPrefab;
  public GameObject loadedLargeIndicatorPrefab;
  public List<GameObject> currentIndicators = new List<GameObject>();
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public EnemyBloodWaller.BloodWallerState currentState;
  public EnemyBloodWaller.DorryStateChange onStateChange;
  [SerializeField]
  public float movementSpeed;
  [SerializeField]
  public float maxTimeKeepingDistance;
  [SerializeField]
  public float lostKeepDistanceTimePerHit = 0.25f;
  public float keepDistanceTime;
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
  public GameObject bloodWall;
  [SerializeField]
  public float createWallDelay;
  [SerializeField]
  public float wallRadius;
  [SerializeField]
  public float timeBetweenWalls;
  [SerializeField]
  public int wallsAmount;
  [SerializeField]
  public int maxCurrentWalls;
  [SerializeField]
  public bool wallsDamageAlways;
  [SerializeField]
  public LayerMask wallBlockCreationLayermask;
  [SerializeField]
  public float wallBlockCreationRadius;
  [SerializeField]
  public GameObject bigBloodWall;
  [SerializeField]
  public float createBigWallDelay;
  [SerializeField]
  public float createBigWallChaseWindow;
  [SerializeField]
  public float createBigWallDodgeWindow;
  [SerializeField]
  public float createBigWallChaseSpeed;
  [SerializeField]
  public float bigWallLifetime;
  [SerializeField]
  public SpriteRenderer bigWallIndicator;
  [SerializeField]
  public GameObject closeRangeWall;
  [SerializeField]
  public int closeRangeWallsCount = 6;
  [SerializeField]
  public float closeRangeWallRecoverTime = 1.5f;
  [SerializeField]
  public float closeRangeWallRadius = 1.5f;
  [SerializeField]
  public GameObject directionalWall;
  [SerializeField]
  public float directionalWallDuration = 1.5f;
  [SerializeField]
  public float directionalWallCreateDelay = 0.05f;
  [SerializeField]
  public float directionalWallRecoverTime = 1.5f;
  [SerializeField]
  public int directionalWallCount = 6;
  [SerializeField]
  public float directionalWallOffset = 1f;
  [SerializeField]
  public float directionalWallStartSpawnOffset = 1.5f;
  [SerializeField]
  public float idleTime;
  public float idleTimer;
  [SerializeField]
  public List<Health> currentWalls = new List<Health>();
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/death";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/warning";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/gethit";
  [EventRef]
  public string AttackSpikeRingStartSFX = "event:/dlc/dungeon06/enemy/bloodwaller/attack_spike_small_ring_start";
  [EventRef]
  public string AttackSmallSpikeEmergeSFX = "event:/dlc/dungeon06/enemy/bloodwaller/attack_spike_small_emerge";
  [EventRef]
  public string AttackLargeSpikeStartSFX = "event:/dlc/dungeon06/enemy/bloodwaller/attack_spike_large_start";
  [EventRef]
  public string AttackLargeSpikeWindupSFX = "event:/dlc/dungeon06/enemy/bloodwaller/attack_spike_large_windup";
  public float flashTickTimer;
  public Color indicatorColor = Color.white;
  public Coroutine idleRoutine;
  public Coroutine moveRoutine;
  public Coroutine createWallRoutine;
  public Coroutine createBigWallRoutine;
  public Coroutine createCloseRangeWallRoutine;
  public Coroutine createWallInLineRoutine;

  public EnemyBloodWaller.BloodWallerState CURRENT_BLOOD_WALLER_STATE
  {
    get => this.currentState;
    set
    {
      if (this.onStateChange != null)
        this.OnStateChange(value, this.currentState);
      this.currentState = value;
    }
  }

  public int CurrentWallsAmount => this.currentWalls.Count;

  public bool ShouldCreateWall => this.CurrentWallsAmount < this.maxCurrentWalls;

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    this.LoadAssets();
  }

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.smallIndicatorPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyBloodWaller.loadedAddressableAssets.Add(obj);
      this.loadedSmallIndicatorPrefab = obj.Result;
      this.loadedSmallIndicatorPrefab.CreatePool(20, true);
    });
    Addressables.LoadAssetAsync<GameObject>((object) this.largeIndicatorPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyBloodWaller.loadedAddressableAssets.Add(obj);
      this.loadedLargeIndicatorPrefab = obj.Result;
      this.loadedLargeIndicatorPrefab.CreatePool(5, true);
    });
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    if (EnemyBloodWaller.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in EnemyBloodWaller.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    EnemyBloodWaller.loadedAddressableAssets.Clear();
  }

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(((UnitObject) this).OnHitEarly);
    this.InitDamageColliders();
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
    this.onStateChange += new EnemyBloodWaller.DorryStateChange(this.OnStateChange);
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(((UnitObject) this).OnHitEarly);
    this.DisposeDamageColliders();
    this.bigWallIndicator.gameObject.SetActive(false);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    this.onStateChange -= new EnemyBloodWaller.DorryStateChange(this.OnStateChange);
    this.DisableAllIndicators();
  }

  public void DisableAllIndicators()
  {
    foreach (GameObject currentIndicator in this.currentIndicators)
    {
      if ((bool) (UnityEngine.Object) currentIndicator)
        currentIndicator.Recycle();
    }
    this.currentIndicators.Clear();
  }

  public override void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (PlayerController.CanParryAttacks && !FromBehind && AttackType == Health.AttackTypes.Melee)
    {
      this.health.WasJustParried = true;
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.DisableForces = false;
    }
    this.keepDistanceTime += this.lostKeepDistanceTimePerHit;
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
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    while (this.currentWalls.Count > 0)
    {
      Health currentWall = this.currentWalls[0];
      if ((bool) (UnityEngine.Object) currentWall)
        currentWall.DealDamage(999f, this.gameObject, this.transform.position);
      else
        this.currentWalls.Remove(currentWall);
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
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
    EnemyBloodWaller enemyBloodWaller = this;
    enemyBloodWaller.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    enemyBloodWaller.state.CURRENT_STATE = StateMachine.State.Dancing;
    enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.Idle;
  }

  public IEnumerator IdleState()
  {
    while ((double) (this.idleTimer -= Time.deltaTime * this.Spine.timeScale) > 0.0)
      yield return (object) null;
    this.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.KeepDistance;
    yield return (object) null;
  }

  public IEnumerator KeepDistanceState()
  {
    EnemyBloodWaller enemyBloodWaller = this;
    Vector3 targetPosition = BiomeGenerator.GetRandomPositionInIsland();
    Health closestTarget1 = enemyBloodWaller.GetClosestTarget();
    bool validPosition = (UnityEngine.Object) closestTarget1 != (UnityEngine.Object) null && (double) Vector3.Distance(targetPosition, closestTarget1.transform.position) > (double) enemyBloodWaller.closeAttackDistance;
    while (!validPosition)
    {
      targetPosition = BiomeGenerator.GetRandomPositionInIsland();
      Health closestTarget2 = enemyBloodWaller.GetClosestTarget();
      if ((UnityEngine.Object) closestTarget2 != (UnityEngine.Object) null)
        validPosition = (double) Vector3.Distance(targetPosition, closestTarget2.transform.position) > (double) enemyBloodWaller.closeAttackDistance;
      yield return (object) null;
    }
    enemyBloodWaller.keepDistanceTime = 0.0f;
    while ((UnityEngine.Object) enemyBloodWaller.GetClosestTarget() != (UnityEngine.Object) null)
    {
      if ((double) enemyBloodWaller.keepDistanceTime >= (double) enemyBloodWaller.maxTimeKeepingDistance)
      {
        if ((double) Vector3.Distance(enemyBloodWaller.transform.position, enemyBloodWaller.GetClosestTarget().transform.position) > (double) enemyBloodWaller.closeAttackDistance)
        {
          enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = enemyBloodWaller.ShouldCreateWall ? EnemyBloodWaller.BloodWallerState.CreateWall : EnemyBloodWaller.BloodWallerState.CreateBigWall;
          yield break;
        }
        if (enemyBloodWaller.createCloseRangeWallRoutine == null)
        {
          enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.CreateCloseRangeWall;
          yield break;
        }
        enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.Idle;
        yield break;
      }
      enemyBloodWaller.keepDistanceTime += Time.deltaTime * enemyBloodWaller.Spine.timeScale;
      enemyBloodWaller.FacePosition(targetPosition);
      enemyBloodWaller.Move(enemyBloodWaller.movementSpeed);
      yield return (object) null;
    }
    enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.Idle;
    yield return (object) null;
  }

  public IEnumerator CreateWallState()
  {
    EnemyBloodWaller enemyBloodWaller = this;
    float num1 = 360f / (float) enemyBloodWaller.wallsAmount;
    Vector2 position = (Vector2) enemyBloodWaller.GetClosestTarget().transform.position;
    float num2 = Utils.GetAngle(enemyBloodWaller.transform.position, enemyBloodWaller.GetClosestTarget().transform.position) + 180f + num1 / 2f;
    float num3 = Utils.GetAngle(enemyBloodWaller.transform.position, enemyBloodWaller.GetClosestTarget().transform.position) + 180f - num1 / 2f;
    List<Vector3> wallPositionListOne = new List<Vector3>();
    List<Vector3> wallPositionListTwo = new List<Vector3>();
    for (int index = 0; index < enemyBloodWaller.wallsAmount / 2; ++index)
    {
      Vector2 vector2_1 = new Vector2(Mathf.Cos((float) ((double) num2 * 3.1415927410125732 / 180.0)), Mathf.Sin((float) ((double) num2 * 3.1415927410125732 / 180.0))) * enemyBloodWaller.wallRadius;
      Vector2 vector2_2 = new Vector2(Mathf.Cos((float) ((double) num3 * 3.1415927410125732 / 180.0)), Mathf.Sin((float) ((double) num3 * 3.1415927410125732 / 180.0))) * enemyBloodWaller.wallRadius;
      Vector3 vector3_1 = (Vector3) (position + vector2_1);
      Vector3 vector3_2 = (Vector3) (position + vector2_2);
      wallPositionListOne.Add(vector3_1);
      wallPositionListTwo.Add(vector3_2);
      num2 += num1;
      num3 -= num1;
      if (enemyBloodWaller.CheckWallObstacles(wallPositionListOne[index]) && BiomeGenerator.PointWithinIsland(wallPositionListOne[index], out Vector3 _))
        enemyBloodWaller.SpawnIndicatorAtPosition(wallPositionListOne[index], false);
      if (enemyBloodWaller.CheckWallObstacles(wallPositionListTwo[index]) && BiomeGenerator.PointWithinIsland(wallPositionListTwo[index], out Vector3 _))
        enemyBloodWaller.SpawnIndicatorAtPosition(wallPositionListTwo[index], false);
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBloodWaller.Spine.timeScale) < (double) enemyBloodWaller.createWallDelay)
    {
      enemyBloodWaller.FacePosition(enemyBloodWaller.GetClosestTarget().transform.position);
      yield return (object) null;
    }
    enemyBloodWaller.Spine.AnimationState.SetAnimation(0, "un-bite", false);
    enemyBloodWaller.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.createWallDelay, enemyBloodWaller.Spine);
    enemyBloodWaller.DisableAllIndicators();
    for (int i = 0; i < enemyBloodWaller.wallsAmount / 2; ++i)
    {
      if (enemyBloodWaller.CheckWallObstacles(wallPositionListOne[i]) && BiomeGenerator.PointWithinIsland(wallPositionListOne[i], out Vector3 _))
      {
        GameObject wall = ObjectPool.Spawn(enemyBloodWaller.bloodWall, enemyBloodWaller.transform.parent, wallPositionListOne[i]);
        enemyBloodWaller.InitWall(wall, MMVibrate.HapticTypes.MediumImpact);
      }
      if (enemyBloodWaller.CheckWallObstacles(wallPositionListTwo[i]) && BiomeGenerator.PointWithinIsland(wallPositionListTwo[i], out Vector3 _))
      {
        GameObject wall = ObjectPool.Spawn(enemyBloodWaller.bloodWall, enemyBloodWaller.transform.parent, wallPositionListTwo[i]);
        enemyBloodWaller.InitWall(wall, MMVibrate.HapticTypes.MediumImpact);
      }
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.timeBetweenWalls, enemyBloodWaller.Spine);
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.createWallDelay, enemyBloodWaller.Spine);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.9f, enemyBloodWaller.Spine);
    enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.Idle;
    yield return (object) null;
  }

  public void SpawnIndicatorAtPosition(Vector3 position, bool large = true)
  {
    GameObject gameObject = large ? ObjectPool.Spawn(this.loadedLargeIndicatorPrefab) : ObjectPool.Spawn(this.loadedSmallIndicatorPrefab);
    gameObject.transform.position = position;
    this.currentIndicators.Add(gameObject);
  }

  public IEnumerator BigSpikeAttack()
  {
    EnemyBloodWaller enemyBloodWaller = this;
    Health closestTarget = enemyBloodWaller.GetClosestTarget();
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBloodWaller.Spine.timeScale) < (double) enemyBloodWaller.createWallDelay)
    {
      if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
        enemyBloodWaller.FacePosition(closestTarget.transform.position);
      else
        closestTarget = enemyBloodWaller.GetClosestTarget();
      yield return (object) null;
    }
    enemyBloodWaller.ClearPaths();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.createWallDelay, enemyBloodWaller.Spine);
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
      closestTarget = enemyBloodWaller.GetClosestTarget();
    enemyBloodWaller.bigWallIndicator.gameObject.SetActive(true);
    enemyBloodWaller.bigWallIndicator.transform.position = closestTarget.transform.position;
    if (!string.IsNullOrEmpty(enemyBloodWaller.AttackLargeSpikeWindupSFX))
      AudioManager.Instance.PlayOneShot(enemyBloodWaller.AttackLargeSpikeWindupSFX, enemyBloodWaller.bigWallIndicator.transform.position);
    Vector2 position1 = (Vector2) closestTarget.transform.position;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBloodWaller.Spine.timeScale) < (double) enemyBloodWaller.createBigWallChaseWindow)
    {
      if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
        closestTarget = enemyBloodWaller.GetClosestTarget();
      Vector2 position2 = (Vector2) closestTarget.transform.position;
      enemyBloodWaller.bigWallIndicator.transform.position = Vector3.MoveTowards(enemyBloodWaller.bigWallIndicator.transform.position, (Vector3) position2, enemyBloodWaller.createBigWallChaseSpeed * Time.deltaTime * enemyBloodWaller.Spine.timeScale);
      if (enemyBloodWaller.bigWallIndicator.gameObject.activeSelf && (double) enemyBloodWaller.flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        enemyBloodWaller.indicatorColor = enemyBloodWaller.indicatorColor == Color.white ? Color.red : Color.white;
        enemyBloodWaller.bigWallIndicator.material.SetColor("_Color", enemyBloodWaller.indicatorColor);
        enemyBloodWaller.flashTickTimer = 0.0f;
      }
      enemyBloodWaller.flashTickTimer += Time.deltaTime;
      yield return (object) null;
    }
    enemyBloodWaller.Spine.AnimationState.SetAnimation(0, "un-bite", false);
    enemyBloodWaller.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.createBigWallDodgeWindow, enemyBloodWaller.Spine);
    GameObject newWall = ObjectPool.Spawn(enemyBloodWaller.bigBloodWall, enemyBloodWaller.transform.parent, enemyBloodWaller.bigWallIndicator.transform.position);
    enemyBloodWaller.InitWall(newWall, MMVibrate.HapticTypes.HeavyImpact);
    enemyBloodWaller.bigWallIndicator.gameObject.SetActive(false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.bigWallLifetime, enemyBloodWaller.Spine);
    enemyBloodWaller.DestroyWall(newWall);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.9f, enemyBloodWaller.Spine);
    enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.Idle;
    yield return (object) null;
  }

  public bool CheckWallObstacles(Vector3 point)
  {
    return (UnityEngine.Object) Physics2D.OverlapCircle((Vector2) point, this.wallBlockCreationRadius, (int) this.wallBlockCreationLayermask) == (UnityEngine.Object) null;
  }

  public void FacePosition(Vector3 position)
  {
    if ((double) Vector3.Distance(this.transform.position, position) < 1.0)
      return;
    float angle = Utils.GetAngle(this.transform.position, position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 vector3 = new Vector3(-1f, 1f, 1f);
    this.damageCollidersParent.localScale = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? vector3 : Vector3.one;
  }

  public void OnStateChange(
    EnemyBloodWaller.BloodWallerState newState,
    EnemyBloodWaller.BloodWallerState prevState)
  {
    switch (newState)
    {
      case EnemyBloodWaller.BloodWallerState.Idle:
        this.idleTimer = this.idleTime;
        this.Spine.AnimationState.SetAnimation(0, "idle", true);
        if (this.idleRoutine != null)
          this.StopCoroutine(this.idleRoutine);
        this.idleRoutine = this.StartCoroutine((IEnumerator) this.IdleState());
        break;
      case EnemyBloodWaller.BloodWallerState.KeepDistance:
        this.repathTimer = 1f;
        this.speed = this.movementSpeed * this.SpeedMultiplier;
        this.Spine.AnimationState.SetAnimation(0, "walk", true);
        if (this.moveRoutine != null)
          this.StopCoroutine(this.moveRoutine);
        this.moveRoutine = this.StartCoroutine((IEnumerator) this.KeepDistanceState());
        break;
      case EnemyBloodWaller.BloodWallerState.CreateWall:
        this.Spine.AnimationState.SetAnimation(0, "bite", false);
        this.Spine.AnimationState.AddAnimation(0, "idle-down", true, 0.0f);
        if (!string.IsNullOrEmpty(this.AttackSpikeRingStartSFX))
          AudioManager.Instance.PlayOneShot(this.AttackSpikeRingStartSFX);
        if (this.createWallRoutine != null)
          this.StopCoroutine(this.createWallRoutine);
        this.createWallRoutine = this.StartCoroutine((IEnumerator) this.CreateWallState());
        break;
      case EnemyBloodWaller.BloodWallerState.CreateBigWall:
        this.ClearPaths();
        this.Spine.AnimationState.SetAnimation(0, "bite", false);
        this.Spine.AnimationState.AddAnimation(0, "idle-down", true, 0.0f);
        if (!string.IsNullOrEmpty(this.AttackLargeSpikeStartSFX))
          AudioManager.Instance.PlayOneShot(this.AttackLargeSpikeStartSFX);
        if (this.moveRoutine != null)
          this.StopCoroutine(this.moveRoutine);
        if (this.createBigWallRoutine != null)
          this.StopCoroutine(this.createBigWallRoutine);
        this.createBigWallRoutine = this.StartCoroutine((IEnumerator) this.BigSpikeAttack());
        break;
      case EnemyBloodWaller.BloodWallerState.CreateWallLine:
        this.ClearPaths();
        this.Spine.AnimationState.SetAnimation(0, "bite", false);
        this.Spine.AnimationState.AddAnimation(0, "idle-down", true, 0.0f);
        if (!string.IsNullOrEmpty(this.AttackLargeSpikeStartSFX))
          AudioManager.Instance.PlayOneShot(this.AttackLargeSpikeStartSFX);
        if (this.moveRoutine != null)
          this.StopCoroutine(this.moveRoutine);
        this.createWallInLineRoutine = this.StartCoroutine((IEnumerator) this.CreateWallsInDirection());
        break;
      case EnemyBloodWaller.BloodWallerState.CreateCloseRangeWall:
        this.ClearPaths();
        this.Spine.AnimationState.SetAnimation(0, "bite", false);
        this.Spine.AnimationState.AddAnimation(0, "idle-down", true, 0.0f);
        if (!string.IsNullOrEmpty(this.AttackLargeSpikeStartSFX))
          AudioManager.Instance.PlayOneShot(this.AttackLargeSpikeStartSFX);
        if (this.moveRoutine != null)
          this.StopCoroutine(this.moveRoutine);
        this.createCloseRangeWallRoutine = this.StartCoroutine((IEnumerator) this.CreateCloseRangeWall());
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
      if ((bool) (UnityEngine.Object) damageCollider)
        damageCollider.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      if ((bool) (UnityEngine.Object) damageCollider)
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
      if ((bool) (UnityEngine.Object) damageCollider)
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
    if ((double) this.Spine.timeScale < 1.0 / 1000.0 || float.IsNaN(this.state.facingAngle))
      return;
    if (float.IsNaN(this.speed) || float.IsInfinity(this.speed))
      this.speed = 0.0f;
    this.speed = Mathf.Clamp(movementSpeed, 0.0f, this.maxSpeed);
    this.moveVX = this.speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f));
    this.moveVY = this.speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f));
    float num = Time.deltaTime * this.Spine.timeScale;
    if (float.IsNaN(this.moveVX) || float.IsInfinity(this.moveVX))
      this.moveVX = 0.0f;
    if (float.IsNaN(this.moveVY) || float.IsInfinity(this.moveVY))
      this.moveVY = 0.0f;
    this.rb.MovePosition(this.rb.position + new Vector2(this.vx, this.vy) * num + new Vector2(this.moveVX, this.moveVY) * num + new Vector2(this.seperatorVX, this.seperatorVY) * num + new Vector2(this.knockBackVX, this.knockBackVY) * num);
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

  public void OnWallDies(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.currentWalls.Contains(Victim))
      return;
    this.currentWalls.Remove(Victim);
  }

  public void InitWall(GameObject wall, MMVibrate.HapticTypes hapticTypes)
  {
    wall.transform.localScale = Vector3.zero;
    Health componentInChildren1 = wall.GetComponentInChildren<Health>();
    ColliderEvents componentInChildren2 = wall.GetComponentInChildren<ColliderEvents>();
    if ((bool) (UnityEngine.Object) componentInChildren1)
    {
      this.currentWalls.Add(componentInChildren1);
      componentInChildren1.OnDie += new Health.DieAction(this.OnWallDies);
    }
    if ((bool) (UnityEngine.Object) componentInChildren2)
    {
      componentInChildren2.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      if (this.wallsDamageAlways)
        this.StartCoroutine((IEnumerator) this.EnableDamageCollider(componentInChildren2, 0.0f));
      else
        this.StartCoroutine((IEnumerator) this.EnableDamageColliderTime(componentInChildren2, 0.0f));
    }
    wall.transform.DOScale(Vector3.one, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
    wall.transform.parent = BiomeGenerator.Instance.CurrentRoom.generateRoom.transform;
    if (!string.IsNullOrEmpty(this.AttackSmallSpikeEmergeSFX))
      AudioManager.Instance.PlayOneShot(this.AttackSmallSpikeEmergeSFX, wall.transform.position);
    MMVibrate.Haptic(hapticTypes);
  }

  public void DestroyWall(GameObject wall)
  {
    wall.transform.DOScale(Vector3.zero, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBounce).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) wall)));
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/bloodwaller/attack_spike_barrier_despawn", wall.transform.position);
    Health componentInChildren = wall.GetComponentInChildren<Health>();
    if (!this.currentWalls.Contains(componentInChildren))
      return;
    this.currentWalls.Remove(componentInChildren);
  }

  public IEnumerator CreateCloseRangeWall()
  {
    EnemyBloodWaller enemyBloodWaller = this;
    float num = 360f / (float) enemyBloodWaller.closeRangeWallsCount;
    Vector2 position = (Vector2) enemyBloodWaller.transform.position;
    float angle = 0.0f;
    for (int index = 0; index < enemyBloodWaller.closeRangeWallsCount; ++index)
    {
      Vector2 vector2 = new Vector2(Mathf.Cos(angle * ((float) Math.PI / 180f)), Mathf.Sin(angle * ((float) Math.PI / 180f))) * enemyBloodWaller.closeRangeWallRadius;
      Vector3 startPosition = (Vector3) (position + vector2);
      enemyBloodWaller.StartCoroutine((IEnumerator) enemyBloodWaller.SpawnWallsInDirection(enemyBloodWaller.closeRangeWall, startPosition, angle, 1, 0.0f, MMVibrate.HapticTypes.HeavyImpact));
      angle += num;
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.closeRangeWallRecoverTime, enemyBloodWaller.Spine);
    enemyBloodWaller.createCloseRangeWallRoutine = (Coroutine) null;
    enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.Idle;
  }

  public IEnumerator CreateWallsInDirection()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBloodWaller enemyBloodWaller = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyBloodWaller.createCloseRangeWallRoutine = (Coroutine) null;
      enemyBloodWaller.CURRENT_BLOOD_WALLER_STATE = EnemyBloodWaller.BloodWallerState.Idle;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyBloodWaller.FacePosition(enemyBloodWaller.GetClosestTarget().transform.position);
    float angle = Utils.GetAngle(enemyBloodWaller.transform.position, enemyBloodWaller.GetClosestTarget().transform.position);
    Vector3 vector3 = new Vector3(Mathf.Cos(angle * ((float) Math.PI / 180f)), Mathf.Sin(angle * ((float) Math.PI / 180f))) * enemyBloodWaller.directionalWallStartSpawnOffset;
    enemyBloodWaller.StartCoroutine((IEnumerator) enemyBloodWaller.SpawnWallsInDirection(enemyBloodWaller.directionalWall, enemyBloodWaller.transform.position + vector3, angle, enemyBloodWaller.directionalWallCount, enemyBloodWaller.directionalWallOffset, MMVibrate.HapticTypes.MediumImpact));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.closeRangeWallRecoverTime, enemyBloodWaller.Spine);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator SpawnWallsInDirection(
    GameObject wallPrefab,
    Vector3 startPosition,
    float angle,
    int wallsCount,
    float spawnPointOffset,
    MMVibrate.HapticTypes hapticType)
  {
    EnemyBloodWaller enemyBloodWaller = this;
    int x;
    for (x = 0; x < wallsCount; ++x)
    {
      Vector2 vector2 = (Vector2) (startPosition + (Vector3) Utils.DegreeToVector2(angle) * (float) x * spawnPointOffset);
      if (enemyBloodWaller.CheckWallObstacles((Vector3) vector2) && BiomeGenerator.PointWithinIsland((Vector3) vector2, out Vector3 _))
      {
        enemyBloodWaller.SpawnIndicatorAtPosition((Vector3) vector2);
        yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.directionalWallCreateDelay, enemyBloodWaller.Spine);
      }
    }
    if ((UnityEngine.Object) wallPrefab == (UnityEngine.Object) enemyBloodWaller.loadedLargeIndicatorPrefab && !string.IsNullOrEmpty(enemyBloodWaller.AttackLargeSpikeWindupSFX))
      AudioManager.Instance.PlayOneShot(enemyBloodWaller.AttackLargeSpikeWindupSFX, enemyBloodWaller.bigWallIndicator.transform.position);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.createWallDelay, enemyBloodWaller.Spine);
    enemyBloodWaller.DisableAllIndicators();
    List<GameObject> walls = new List<GameObject>();
    for (x = 0; x < wallsCount; ++x)
    {
      Vector2 vector2 = (Vector2) (startPosition + (Vector3) Utils.DegreeToVector2(angle) * (float) x * spawnPointOffset);
      if (enemyBloodWaller.CheckWallObstacles((Vector3) vector2) && BiomeGenerator.PointWithinIsland((Vector3) vector2, out Vector3 _))
      {
        GameObject wall = ObjectPool.Spawn(wallPrefab, enemyBloodWaller.transform.parent, (Vector3) vector2);
        enemyBloodWaller.InitWall(wall, hapticType);
        walls.Add(wall);
        yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.directionalWallCreateDelay, enemyBloodWaller.Spine);
      }
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyBloodWaller.directionalWallDuration, enemyBloodWaller.Spine);
    foreach (GameObject wall in walls)
    {
      enemyBloodWaller.DestroyWall(wall);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(0.05f, enemyBloodWaller.Spine);
    }
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__69_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemyBloodWaller.loadedAddressableAssets.Add(obj);
    this.loadedSmallIndicatorPrefab = obj.Result;
    this.loadedSmallIndicatorPrefab.CreatePool(20, true);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__69_1(AsyncOperationHandle<GameObject> obj)
  {
    EnemyBloodWaller.loadedAddressableAssets.Add(obj);
    this.loadedLargeIndicatorPrefab = obj.Result;
    this.loadedLargeIndicatorPrefab.CreatePool(5, true);
  }

  public enum BloodWallerState
  {
    Idle,
    KeepDistance,
    CreateWall,
    CreateBigWall,
    CreateWallLine,
    CreateCloseRangeWall,
  }

  public delegate void DorryStateChange(
    EnemyBloodWaller.BloodWallerState NewState,
    EnemyBloodWaller.BloodWallerState PrevState);
}
