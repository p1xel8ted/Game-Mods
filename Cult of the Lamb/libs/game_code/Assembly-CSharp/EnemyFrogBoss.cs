// Decompiled with JetBrains decompiler
// Type: EnemyFrogBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyFrogBoss : UnitObject
{
  public const float PHASE3_SPAWN_MULTIPLAYER = 1.6f;
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string hopAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string hopAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string hopEndAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string mortarStrikeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string mortarValleyAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string eggSpawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string burpAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string bounceAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string enragedAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string tongueAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string tongueEndAnimation;
  [SerializeField]
  public CircleCollider2D physicsCollider;
  [SerializeField]
  public float hopAnticipation;
  [SerializeField]
  public float hopDuration;
  [SerializeField]
  public float hopSpeed;
  [SerializeField]
  public float hopZHeight;
  [SerializeField]
  public AnimationCurve hopSpeedCurve;
  [SerializeField]
  public AnimationCurve hopZCurve;
  [SerializeField]
  public ColliderEvents damageColliderEvents;
  [SerializeField]
  public LayerMask unitMask;
  [SerializeField]
  public GameObject shadow;
  [SerializeField]
  public MortarBomb randomMortarPrefab;
  [SerializeField]
  public float randomMortarDuration;
  [SerializeField]
  public float randomMortarAnticipation;
  [SerializeField]
  public Vector2 randomMortarsToSpawn;
  [SerializeField]
  public Vector2 randomMortarDistance;
  [SerializeField]
  public Vector2 randomMortarDelayBetweenShots;
  [SerializeField]
  public MortarBomb targetedMortarPrefab;
  [SerializeField]
  public float targetedMortarDuration;
  [SerializeField]
  public float targetedMortarAnticipation;
  [SerializeField]
  public Vector2 targetedMortarsToSpawn;
  [SerializeField]
  public Vector2 targetedMortarDelayBetweenShots;
  [SerializeField]
  public GameObject eggPrefab;
  [SerializeField]
  public Vector2 eggsToSpawn;
  [SerializeField]
  public float eggSpawnDelay;
  [SerializeField]
  public float eggsMultipleAnticipation;
  [SerializeField]
  public Vector2 eggsMultipleToSpawn;
  [SerializeField]
  public Vector2 eggsMultipleSpawnDelay;
  [SerializeField]
  public Vector2 eggsKnockback;
  [SerializeField]
  public ParticleSystem aoeParticles;
  [SerializeField]
  public float aoeDuration;
  [SerializeField]
  public Vector2 projectilesToSpawn;
  [SerializeField]
  public Vector2 projectileDelayBetweenSpawn;
  [SerializeField]
  public GameObject burpPosition;
  [SerializeField]
  public float burpAnticipation;
  [SerializeField]
  public GameObject projectilePrefab;
  [SerializeField]
  public Vector2 bounces;
  [SerializeField]
  public float bounceAnticipation;
  [SerializeField]
  public Vector2 timeBetweenBounce;
  [SerializeField]
  public Vector3 sitPosition = new Vector3(0.0f, 7.75f, -0.2f);
  [SerializeField]
  public float tongueSpitDelay = 0.25f;
  [SerializeField]
  public FrogBossTongue tonguePrefab;
  [SerializeField]
  public GameObject tonguePosition;
  [SerializeField]
  public float tongueWhipAnticipation = 1f;
  [SerializeField]
  public float tongueWhipDuration = 0.5f;
  [SerializeField]
  public float tongueRetrieveDelay = 2f;
  [SerializeField]
  public float tongueRetrieveDuration = 0.1f;
  [SerializeField]
  public Vector2 tongueWhipDelay;
  [SerializeField]
  public Vector2 tongueWhipAmount;
  [SerializeField]
  public float tongueScatterRadius = 10f;
  [SerializeField]
  public float tongueScatterPostAnticipation;
  [SerializeField]
  public float tongueScatterPreAnticipation;
  [SerializeField]
  public float tongueScatterWhipDuration;
  [SerializeField]
  public Vector2 tongueScatterDelay;
  [SerializeField]
  public Vector2 tongueScatterAmount;
  [SerializeField]
  public float enragedDuration = 2f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float p2HealthThreshold = 0.6f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float p3HealthThreshold = 0.3f;
  [SerializeField]
  public AssetReferenceGameObject miniBossFrogTarget;
  [SerializeField]
  public AssetReferenceGameObject miniBossOther;
  [SerializeField]
  public float miniBossSpawningDelay = 0.5f;
  [SerializeField]
  public int miniBossFrogsSpawnAmount;
  [SerializeField]
  public float spawnForce = 0.75f;
  [SerializeField]
  public Vector3[] miniBossSpawnPositions = new Vector3[0];
  [SerializeField]
  public int maxEnemies;
  [SerializeField]
  public AssetReferenceGameObject enemyHopperTarget;
  [SerializeField]
  public AssetReferenceGameObject[] enemyRare;
  [SerializeField]
  public Vector2 spawnDelay;
  [SerializeField]
  public Vector3[] spawnPositions = new Vector3[0];
  [Space]
  [SerializeField]
  public Renderer distortionObject;
  [SerializeField]
  public Interaction_MonsterHeart interaction_MonsterHeart;
  [SerializeField]
  public GameObject playerBlocker;
  [SerializeField]
  public GameObject followerToSpawn;
  public GameObject cameraTarget;
  public Health currentTarget;
  public bool attacking;
  public bool anticipating;
  public bool hasCollidedWithObstacle;
  public bool queuePhaseIncrement;
  public float anticipationTimer;
  public float anticipationDuration;
  public float targetAngle;
  public float startingHealth;
  public int currentPhaseNumber;
  public Coroutine currentPhaseRoutine;
  public Coroutine currentAttackRoutine;
  public IEnumerator damageColliderRoutine;
  public Collider2D collider;
  public ShowHPBar hpBar;
  public int enemiesAlive;
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
  public List<FrogBossTongue> tongues = new List<FrogBossTongue>();
  public List<Projectile> projectiles = new List<Projectile>();
  public Projectile baseProjectile;
  public bool isDead;
  public bool facePlayer;
  public bool usingTongue;
  public bool active;
  public float spawnTimestamp;
  public int miniBossesKilled;
  public int previousAttackIndex;
  public bool juicedForm;
  public AsyncOperationHandle<GameObject> enemyHopperTargetHandle;
  public AsyncOperationHandle<GameObject> miniBossFrogTargetHandle;
  public AsyncOperationHandle<GameObject> miniBossOtherHandle;
  public List<AsyncOperationHandle<GameObject>> loadedEnemyRare = new List<AsyncOperationHandle<GameObject>>();

  public GameObject TonguePosition => this.tonguePosition;

  public override void Awake()
  {
    base.Awake();
    this.cameraTarget = this.GetComponentInParent<MiniBossController>().cameraTarget;
  }

  public void Preload()
  {
    for (int index = 0; index < this.enemyRare.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.enemyRare[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedEnemyRare.Add(obj);
        obj.Result.CreatePool(20, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
    AsyncOperationHandle<GameObject> asyncOperationHandle1 = Addressables.LoadAssetAsync<GameObject>((object) this.enemyHopperTarget);
    asyncOperationHandle1.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.enemyHopperTargetHandle = obj;
      obj.Result.CreatePool(20, true);
    });
    asyncOperationHandle1.WaitForCompletion();
    AsyncOperationHandle<GameObject> asyncOperationHandle2 = Addressables.LoadAssetAsync<GameObject>((object) this.miniBossFrogTarget);
    asyncOperationHandle2.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.miniBossFrogTargetHandle = obj;
      obj.Result.CreatePool(3, true);
    });
    asyncOperationHandle2.WaitForCompletion();
    AsyncOperationHandle<GameObject> asyncOperationHandle3 = Addressables.LoadAssetAsync<GameObject>((object) this.miniBossOther);
    asyncOperationHandle3.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.miniBossOtherHandle = obj;
      obj.Result.CreatePool(3, true);
    });
    asyncOperationHandle3.WaitForCompletion();
  }

  public IEnumerator Start()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(enemyFrogBoss.OnTriggerEnterEvent);
    enemyFrogBoss.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(enemyFrogBoss.AnimationEvent);
    enemyFrogBoss.collider = enemyFrogBoss.GetComponent<Collider2D>();
    enemyFrogBoss.hpBar = enemyFrogBoss.GetComponent<ShowHPBar>();
    enemyFrogBoss.startingHealth = enemyFrogBoss.health.HP;
    if (DataManager.Instance.playerDeathsInARowFightingLeader >= 2)
      enemyFrogBoss.maxEnemies -= 3;
    enemyFrogBoss.facePlayer = true;
    enemyFrogBoss.juicedForm = GameManager.Layer2;
    if (enemyFrogBoss.juicedForm)
    {
      enemyFrogBoss.maxEnemies = (int) ((double) enemyFrogBoss.maxEnemies * 1.3300000429153442);
      enemyFrogBoss.health.totalHP *= 1.5f;
      enemyFrogBoss.health.HP = enemyFrogBoss.health.totalHP;
      enemyFrogBoss.miniBossFrogsSpawnAmount = 4;
      enemyFrogBoss.randomMortarDuration /= 1.5f;
    }
    enemyFrogBoss.health.SlowMoOnkill = false;
    yield return (object) null;
    enemyFrogBoss.Preload();
    enemyFrogBoss.InitializeMortarStrikes();
    enemyFrogBoss.InitializeBurpingProjectiles();
    SimulationManager.Pause();
    if ((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null)
      SkeletonAnimationLODGlobalManager.Instance.DisableCulling(enemyFrogBoss.Spine.transform, enemyFrogBoss.Spine);
  }

  public override void Update()
  {
    if (!this.usingTongue)
      base.Update();
    if (this.anticipating)
    {
      this.anticipationTimer += Time.deltaTime * this.Spine.timeScale;
      if ((double) this.anticipationTimer / (double) this.anticipationDuration > 1.0)
      {
        this.anticipating = false;
        this.anticipationTimer = 0.0f;
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Moving && this.hasCollidedWithObstacle)
      this.speed *= 0.5f;
    if (this.queuePhaseIncrement)
    {
      this.queuePhaseIncrement = false;
      this.IncrementPhase();
    }
    if (this.currentPhaseNumber > 0)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float spawnTimestamp = this.spawnTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) spawnTimestamp & currentTime.HasValue && !this.isDead)
        this.SpawnEnemy();
    }
    if (!(bool) (UnityEngine.Object) PlayerFarming.Instance || !this.facePlayer || this.isDead)
      return;
    this.LookAt(this.GetAngleToTarget());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.simpleSpineFlash.FlashFillRed(0.25f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/get_hit");
    Vector3 Position = (AttackLocation + Attacker.transform.position) / 2f;
    BiomeConstants.Instance.EmitHitVFX(Position, Quaternion.identity.z, "HitFX_Weak");
    float num = this.health.HP / this.startingHealth;
    if ((this.currentPhaseNumber != 1 || (double) num > (double) this.p2HealthThreshold) && (this.currentPhaseNumber != 2 || (double) num > (double) this.p3HealthThreshold))
      return;
    this.IncrementPhase();
  }

  public void IncrementPhase()
  {
    if (this.attacking)
    {
      this.queuePhaseIncrement = true;
    }
    else
    {
      if (this.currentAttackRoutine != null)
        this.StopCoroutine(this.currentAttackRoutine);
      if (this.currentPhaseRoutine != null)
        this.StopCoroutine(this.currentPhaseRoutine);
      ++this.currentPhaseNumber;
      this.usingTongue = false;
      this.anticipating = false;
      if (this.currentPhaseNumber == 2)
      {
        this.BeginPhase2();
      }
      else
      {
        if (this.currentPhaseNumber < 3)
          return;
        this.BeginPhase3();
      }
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.active)
      return;
    this.StopCoroutine(this.currentAttackRoutine);
    this.StopCoroutine(this.currentPhaseRoutine);
    this.usingTongue = false;
    foreach (Component tongue in this.tongues)
      tongue.gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.DelayAddCamera());
    if (this.currentPhaseNumber == 1)
      this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase1IE(false));
    else if (this.currentPhaseNumber == 2)
    {
      this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase2IE(false));
    }
    else
    {
      if (this.currentPhaseNumber != 3 || this.miniBossesKilled < this.miniBossFrogsSpawnAmount)
        return;
      this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase3IE(false));
    }
  }

  public IEnumerator DelayAddCamera()
  {
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().AddToCamera(this.cameraTarget);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
  }

  public void BeginPhase1()
  {
    this.simpleSpineFlash.SetFacing = SimpleSpineFlash.SetFacingMode.None;
    GameManager.GetInstance().AddToCamera(this.cameraTarget);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    this.health.untouchable = false;
    this.currentPhaseNumber = 1;
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase1IE(true));
  }

  public IEnumerator Phase1IE(bool firstLoop)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.active = true;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    int num1 = firstLoop ? 1 : 0;
    for (int i = 0; i < 3; ++i)
    {
      int num2 = enemyFrogBoss.previousAttackIndex;
      while (enemyFrogBoss.previousAttackIndex == num2)
        num2 = UnityEngine.Random.Range(0, 3);
      enemyFrogBoss.previousAttackIndex = num2;
      enemyFrogBoss.currentTarget = enemyFrogBoss.ReconsiderPlayerTarget();
      switch (num2)
      {
        case 0:
          if (enemyFrogBoss.juicedForm && (double) UnityEngine.Random.value > 0.5)
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.currentTarget.transform.position)));
          else
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(enemyFrogBoss.hopAnticipation, enemyFrogBoss.GetAngleToTarget())));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.BurpProjectilesIE()));
          break;
        case 1:
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.sitPosition)));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.TongueRapidAttackIE()));
          break;
        case 2:
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.currentTarget.transform.position)));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.MortarStrikeTargetedIE()));
          yield return (object) new WaitForSeconds(1f);
          break;
      }
    }
    enemyFrogBoss.currentPhaseRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.Phase1IE(false));
  }

  public void BeginPhase2()
  {
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase2IE(true));
  }

  public IEnumerator Phase2IE(bool firstLoop)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    if (firstLoop)
      yield return (object) enemyFrogBoss.EnragedIE();
    yield return (object) new WaitForEndOfFrame();
    for (int i = 0; i < 3; ++i)
    {
      int num = enemyFrogBoss.previousAttackIndex;
      while (enemyFrogBoss.previousAttackIndex == num)
        num = UnityEngine.Random.Range(0, 3);
      enemyFrogBoss.previousAttackIndex = num;
      enemyFrogBoss.currentTarget = enemyFrogBoss.ReconsiderPlayerTarget();
      switch (num)
      {
        case 0:
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.currentTarget.transform.position)));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.MortarStrikeRandomIE()));
          break;
        case 1:
          if (enemyFrogBoss.juicedForm && (double) UnityEngine.Random.value > 0.5)
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.currentTarget.transform.position)));
          else
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(enemyFrogBoss.hopAnticipation, enemyFrogBoss.GetAngleToTarget())));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.BurpProjectilesIE(1.3f)));
          break;
        case 2:
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.sitPosition)));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.TongueRapidAttackIE()));
          break;
      }
    }
    enemyFrogBoss.currentPhaseRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.Phase2IE(false));
  }

  public void BeginPhase3()
  {
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase3IE(true));
  }

  public IEnumerator Phase3IE(bool firstLoop)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    if (firstLoop)
    {
      yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(Vector3.zero));
      enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.SpawnMiniBossesIE());
      yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.EnragedIE());
      yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopUpIE(enemyFrogBoss.hopAnticipation));
    }
    else
    {
      for (int i = 0; i < 3; ++i)
      {
        int num = enemyFrogBoss.previousAttackIndex;
        while (enemyFrogBoss.previousAttackIndex == num)
          num = UnityEngine.Random.Range(0, 3);
        enemyFrogBoss.previousAttackIndex = num;
        enemyFrogBoss.currentTarget = enemyFrogBoss.ReconsiderPlayerTarget();
        switch (num)
        {
          case 0:
            if (enemyFrogBoss.juicedForm && (double) UnityEngine.Random.value > 0.5)
              yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.currentTarget.transform.position)));
            else
              yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(enemyFrogBoss.hopAnticipation, enemyFrogBoss.GetAngleToTarget())));
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.MortarStrikeTargetedIE()));
            break;
          case 1:
            yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.sitPosition));
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.TongueScatterAttackIE()));
            break;
          case 2:
            if (enemyFrogBoss.juicedForm && (double) UnityEngine.Random.value > 0.5)
              yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.currentTarget.transform.position)));
            else
              yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(enemyFrogBoss.hopAnticipation, enemyFrogBoss.GetAngleToTarget())));
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.BurpProjectilesIE(1.6f)));
            break;
        }
      }
      enemyFrogBoss.currentPhaseRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.Phase3IE(false));
    }
  }

  public void MortarStrikeRandom()
  {
    this.StartCoroutine((IEnumerator) this.MortarStrikeRandomIE());
  }

  public void InitializeMortarStrikes()
  {
    List<MortarBomb> mortarBombList = new List<MortarBomb>();
    for (int index = 0; (double) index < (double) this.targetedMortarsToSpawn.y; ++index)
    {
      MortarBomb mortarBomb = ObjectPool.Spawn<MortarBomb>(this.targetedMortarPrefab, this.transform.parent);
      mortarBomb.destroyOnFinish = false;
      mortarBombList.Add(mortarBomb);
    }
    for (int index = 0; (double) index < (double) this.randomMortarsToSpawn.y; ++index)
    {
      MortarBomb mortarBomb = ObjectPool.Spawn<MortarBomb>(this.randomMortarPrefab, this.transform.parent);
      mortarBomb.destroyOnFinish = false;
      mortarBombList.Add(mortarBomb);
    }
    for (int index = 0; index < mortarBombList.Count; ++index)
      mortarBombList[index].gameObject.Recycle();
  }

  public IEnumerator MortarStrikeRandomIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = enemyFrogBoss.randomMortarAnticipation;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.anticipationDuration)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.attacking = true;
    enemyFrogBoss.facePlayer = false;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.mortarValleyAnimation, false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 0.60000002384185791)
      yield return (object) null;
    int shotsToFire = (int) UnityEngine.Random.Range(enemyFrogBoss.randomMortarsToSpawn.x, enemyFrogBoss.randomMortarsToSpawn.y);
    float aimingAngle = UnityEngine.Random.Range(0.0f, 360f);
    for (int i = 0; i < shotsToFire; ++i)
    {
      Vector3 targetPosition = enemyFrogBoss.transform.position + (Vector3) Utils.DegreeToVector2(aimingAngle) * UnityEngine.Random.Range(enemyFrogBoss.randomMortarDistance.x, enemyFrogBoss.randomMortarDistance.y);
      enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.ShootMortarPosition(targetPosition));
      aimingAngle += (float) (360 / shotsToFire);
      float dur = UnityEngine.Random.Range(enemyFrogBoss.randomMortarDelayBetweenShots.x, enemyFrogBoss.randomMortarDelayBetweenShots.y);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    enemyFrogBoss.attacking = false;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyFrogBoss.facePlayer = true;
  }

  public void MortarStrikeTargeted()
  {
    this.StartCoroutine((IEnumerator) this.MortarStrikeTargetedIE());
  }

  public IEnumerator MortarStrikeTargetedIE()
  {
    this.anticipating = true;
    this.anticipationDuration = this.targetedMortarAnticipation;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.anticipationDuration)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    this.attacking = true;
    int shotsToFire = (int) UnityEngine.Random.Range(this.targetedMortarsToSpawn.x, this.targetedMortarsToSpawn.y);
    for (int i = 0; i < shotsToFire; ++i)
    {
      this.Spine.AnimationState.SetAnimation(0, this.mortarStrikeAnimation, false);
      AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_spit");
      float dur = UnityEngine.Random.Range(this.targetedMortarDelayBetweenShots.x, this.targetedMortarDelayBetweenShots.y);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    this.Spine.AnimationState.SetAnimation(0, this.idleAnimation, false);
    this.attacking = false;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.5)
      yield return (object) null;
  }

  public IEnumerator ShootMortarTarget()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    Health closestTarget = enemyFrogBoss.GetClosestTarget();
    Health health = (UnityEngine.Object) closestTarget == (UnityEngine.Object) null ? (Health) PlayerFarming.Instance.health : closestTarget;
    MortarBomb mortarBomb = ObjectPool.Spawn<MortarBomb>(enemyFrogBoss.targetedMortarPrefab, enemyFrogBoss.transform.parent, (Vector3) AstarPath.active.GetNearest(health.transform.position).node.position, Quaternion.identity);
    mortarBomb.destroyOnFinish = false;
    float num = Mathf.Clamp(Vector3.Distance(enemyFrogBoss.transform.position, enemyFrogBoss.burpPosition.transform.position) / 3f, 1f, float.MaxValue);
    mortarBomb.Play(enemyFrogBoss.burpPosition.transform.position, enemyFrogBoss.targetedMortarDuration * num, Health.Team.Team2);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_spawn");
    yield return (object) new WaitForSeconds(enemyFrogBoss.targetedMortarDuration * num);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_explode");
  }

  public IEnumerator ShootMortarPosition(Vector3 targetPosition)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyFrogBoss enemyFrogBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_explode");
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    MortarBomb mortarBomb = ObjectPool.Spawn<MortarBomb>(enemyFrogBoss.randomMortarPrefab, enemyFrogBoss.transform.parent, (Vector3) AstarPath.active.GetNearest(targetPosition).node.position, Quaternion.identity);
    mortarBomb.destroyOnFinish = false;
    mortarBomb.Play(enemyFrogBoss.burpPosition.transform.position, enemyFrogBoss.randomMortarDuration, Health.Team.Team2);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_spawn");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyFrogBoss.randomMortarDuration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void SpawnEggs() => this.StartCoroutine((IEnumerator) this.SpawnEggsIE());

  public void SpawnEggsCircle() => this.StartCoroutine((IEnumerator) this.SpawnEggsCircleIE(1f));

  public void SpawnEggsForward()
  {
    this.StartCoroutine((IEnumerator) this.SpawnEggsForward(1f, 1.3f));
  }

  public IEnumerator SpawnEggsIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    int eggCount = (int) UnityEngine.Random.Range(enemyFrogBoss.eggsToSpawn.x, enemyFrogBoss.eggsToSpawn.y + 1f);
    yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(0.0f, enemyFrogBoss.GetAngleToTarget()));
    for (int i = 0; i < eggCount; ++i)
    {
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.eggSpawnAnimation, false);
      yield return (object) new WaitForSeconds(enemyFrogBoss.eggSpawnDelay);
      enemyFrogBoss.SpawnEgg(enemyFrogBoss.transform.position, 0.75f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.25f);
      yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(0.0f, enemyFrogBoss.GetAngleToTarget()));
    }
    yield return (object) new WaitForSeconds(1f);
  }

  public IEnumerator SpawnEggsCircleIE(float spawnMultiplier)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = enemyFrogBoss.eggsMultipleAnticipation;
    yield return (object) new WaitForSeconds(enemyFrogBoss.anticipationDuration);
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.attacking = true;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.eggSpawnAnimation, false);
    int eggCount = (int) ((double) UnityEngine.Random.Range(enemyFrogBoss.eggsMultipleToSpawn.x, enemyFrogBoss.eggsMultipleToSpawn.y + 1f) * (double) spawnMultiplier);
    float aimingAngle = UnityEngine.Random.Range(0.0f, 360f);
    for (int i = 0; i < eggCount; ++i)
    {
      enemyFrogBoss.SpawnEgg(enemyFrogBoss.transform.position, UnityEngine.Random.Range(enemyFrogBoss.eggsKnockback.x, enemyFrogBoss.eggsKnockback.y), aimingAngle);
      aimingAngle += (float) (360 / eggCount);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyFrogBoss.eggsMultipleSpawnDelay.x, enemyFrogBoss.eggsMultipleSpawnDelay.y));
    }
    yield return (object) new WaitForSeconds(1f);
    enemyFrogBoss.attacking = false;
  }

  public IEnumerator SpawnEggsForward(float spawnMultiplier, float knockMultiplier)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = enemyFrogBoss.eggsMultipleAnticipation;
    yield return (object) new WaitForSeconds(enemyFrogBoss.anticipationDuration);
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.attacking = true;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.eggSpawnAnimation, false);
    int eggCount = (int) ((double) UnityEngine.Random.Range(enemyFrogBoss.eggsMultipleToSpawn.x, enemyFrogBoss.eggsMultipleToSpawn.y + 1f) * (double) spawnMultiplier);
    float aimingAngle = UnityEngine.Random.Range(-1f, 1f);
    for (int i = 0; i < eggCount; ++i)
    {
      enemyFrogBoss.SpawnEgg(enemyFrogBoss.transform.position, UnityEngine.Random.Range(enemyFrogBoss.eggsKnockback.x, enemyFrogBoss.eggsKnockback.y) * knockMultiplier, aimingAngle + 80f);
      aimingAngle = Utils.Repeat(aimingAngle + 1f / (float) eggCount, 2f) - 1f;
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyFrogBoss.eggsMultipleSpawnDelay.x, enemyFrogBoss.eggsMultipleSpawnDelay.y));
    }
    yield return (object) new WaitForSeconds(1f);
    enemyFrogBoss.attacking = false;
  }

  public void SpawnEgg(Vector3 position, float knockback = 0.0f, float angle = 0.0f)
  {
    UnitObject component = UnityEngine.Object.Instantiate<GameObject>(this.eggPrefab, position, Quaternion.identity, (UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null ? this.transform.parent : (Transform) null).GetComponent<UnitObject>();
    if ((double) knockback == 0.0)
      return;
    component.DoKnockBack(angle, knockback, 1f);
  }

  public void Hop()
  {
    this.StartCoroutine((IEnumerator) this.HopIE(this.hopAnticipation, this.GetAngleToTarget()));
  }

  public void HopToBack()
  {
    this.StartCoroutine((IEnumerator) this.HopToPositionIE(this.sitPosition));
  }

  public void HopAOE()
  {
    this.StartCoroutine((IEnumerator) this.HopIE(this.hopAnticipation, this.GetAngleToTarget()));
  }

  public void HopUp() => this.StartCoroutine((IEnumerator) this.HopUpIE(this.hopAnticipation));

  public void HopDown() => this.StartCoroutine((IEnumerator) this.HopDownIE());

  public IEnumerator HopIE(float anticipation, float angle)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = anticipation;
    enemyFrogBoss.facePlayer = false;
    enemyFrogBoss.targetAngle = angle;
    enemyFrogBoss.LookAt(angle);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnticipationAnimation, false);
    enemyFrogBoss.playerBlocker.SetActive(false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/jump");
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    enemyFrogBoss.hpBar.Hide();
    enemyFrogBoss.IgnoreCollision(true);
    float hopStartTime = GameManager.GetInstance().CurrentTime;
    float t = 0.0f;
    while ((double) t < (double) enemyFrogBoss.hopDuration)
    {
      enemyFrogBoss.speed = enemyFrogBoss.hopSpeedCurve.Evaluate(GameManager.GetInstance().TimeSince(hopStartTime) / enemyFrogBoss.hopDuration) * enemyFrogBoss.hopSpeed;
      enemyFrogBoss.Spine.transform.localPosition = -Vector3.forward * enemyFrogBoss.hopZCurve.Evaluate(GameManager.GetInstance().TimeSince(hopStartTime) / enemyFrogBoss.hopDuration) * enemyFrogBoss.hopZHeight;
      t += Time.deltaTime * enemyFrogBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyFrogBoss.Spine.transform.localPosition = Vector3.zero;
    enemyFrogBoss.IgnoreCollision(false);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopEndAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/land");
    enemyFrogBoss.DoAOE();
    enemyFrogBoss.facePlayer = true;
    enemyFrogBoss.playerBlocker.SetActive(true);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
  }

  public IEnumerator HopToPositionIE(Vector3 position)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnticipationAnimation, false);
    enemyFrogBoss.playerBlocker.SetActive(false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyFrogBoss.IgnoreCollision(true);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/jump");
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    Vector3 fromPosition = enemyFrogBoss.transform.position;
    enemyFrogBoss.LookAt(Utils.GetAngle(enemyFrogBoss.transform.position, position));
    enemyFrogBoss.hpBar.Hide();
    float hopStartTime = GameManager.GetInstance().CurrentTime;
    float t = 0.0f;
    while ((double) t < (double) enemyFrogBoss.hopDuration)
    {
      float num = GameManager.GetInstance().TimeSince(hopStartTime) / enemyFrogBoss.hopDuration;
      enemyFrogBoss.transform.position = Vector3.Lerp(fromPosition, position, num);
      enemyFrogBoss.Spine.transform.localPosition = -Vector3.forward * enemyFrogBoss.hopZCurve.Evaluate(num) * enemyFrogBoss.hopZHeight;
      t += Time.deltaTime * enemyFrogBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyFrogBoss.Spine.transform.localPosition = Vector3.zero;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopEndAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/land");
    enemyFrogBoss.DoAOE();
    enemyFrogBoss.playerBlocker.SetActive(true);
    enemyFrogBoss.IgnoreCollision(false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 0.5)
      yield return (object) null;
  }

  public IEnumerator HopUpIE(float anticipation)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = anticipation;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnticipationAnimation, false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.anticipationDuration)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnimation, false);
    enemyFrogBoss.playerBlocker.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/jump");
    GameManager.GetInstance().RemoveFromCamera(enemyFrogBoss.cameraTarget);
    enemyFrogBoss.IgnoreCollision(true);
    enemyFrogBoss.health.invincible = true;
    enemyFrogBoss.shadow.SetActive(false);
    enemyFrogBoss.hpBar.Hide();
    float t = 0.0f;
    while ((double) t < (double) enemyFrogBoss.hopDuration)
    {
      enemyFrogBoss.Spine.transform.localPosition += -(Vector3.forward * 5f) * enemyFrogBoss.hopSpeed;
      t += Time.deltaTime * enemyFrogBoss.Spine.timeScale;
      yield return (object) null;
    }
  }

  public IEnumerator HopDownIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.IgnoreCollision(false);
    enemyFrogBoss.health.invincible = false;
    enemyFrogBoss.shadow.SetActive(true);
    float t = 0.0f;
    while ((double) t < (double) enemyFrogBoss.hopDuration)
    {
      enemyFrogBoss.Spine.transform.localPosition -= -Vector3.forward * enemyFrogBoss.hopSpeed * 2f;
      t += Time.deltaTime * enemyFrogBoss.Spine.timeScale;
      yield return (object) null;
    }
    GameManager.GetInstance().AddToCamera(enemyFrogBoss.cameraTarget);
    enemyFrogBoss.Spine.transform.localPosition = Vector3.zero;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopEndAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/land");
    enemyFrogBoss.DoAOE();
    enemyFrogBoss.playerBlocker.SetActive(true);
  }

  public void DoAOE()
  {
    if (this.damageColliderRoutine != null)
      this.StopCoroutine((IEnumerator) this.damageColliderRoutine);
    this.damageColliderRoutine = this.TurnOnDamageColliderForDuration(this.damageColliderEvents.gameObject, this.aoeDuration);
    this.StartCoroutine((IEnumerator) this.damageColliderRoutine);
    if ((UnityEngine.Object) this.aoeParticles != (UnityEngine.Object) null)
      this.aoeParticles.Play();
    float radius = 3f;
    foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.collider.transform.position, radius))
    {
      UnitObject component2 = component1.GetComponent<UnitObject>();
      if ((bool) (UnityEngine.Object) component2 && component2.health.team == Health.Team.Team2 && (UnityEngine.Object) component2 != (UnityEngine.Object) this)
        component2.DoKnockBack(this.collider.gameObject, Mathf.Clamp(radius - Vector3.Distance(this.transform.position, component2.transform.position), 0.1f, 3f) / 2f, 0.5f);
    }
    this.distortionObject.gameObject.SetActive(true);
    this.distortionObject.transform.localScale = Vector3.one;
    this.distortionObject.material.SetFloat("_FishEyeIntensity", 0.1f);
    this.distortionObject.transform.DOScale(2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.distortionObject.gameObject.SetActive(false)));
    float v = 1f;
    DOTween.To((DOGetter<float>) (() => v), (DOSetter<float>) (x => v = x), 0.0f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutQuint).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.distortionObject.material.SetFloat("_FishEyeIntensity", v)));
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.back * 0.5f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1f, 0.2f);
  }

  public void BurpProjectiles() => this.StartCoroutine((IEnumerator) this.BurpProjectilesIE());

  public void InitializeBurpingProjectiles()
  {
    this.baseProjectile = this.projectilePrefab.GetComponent<Projectile>();
    ObjectPool.CreatePool(this.projectilePrefab, (int) ((double) this.projectilesToSpawn.y * 1.6000000238418579));
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < gameObjectList.Count; ++index)
      gameObjectList[index].GetComponent<Projectile>().UseLowQualityAnimation();
  }

  public IEnumerator BurpProjectilesIE(float spawnMultiplier = 1f)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = enemyFrogBoss.burpAnticipation;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.burpAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.anticipationDuration)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.LookAt(enemyFrogBoss.GetAngleToTarget());
    int amountToSpawn = (int) ((double) UnityEngine.Random.Range(enemyFrogBoss.projectilesToSpawn.x, enemyFrogBoss.projectilesToSpawn.y) * (double) spawnMultiplier);
    for (int i = 0; i < amountToSpawn; ++i)
    {
      Projectile component = ObjectPool.Spawn(enemyFrogBoss.projectilePrefab, enemyFrogBoss.transform.parent).GetComponent<Projectile>();
      component.UseDelay = true;
      component.CollideOnlyTarget = true;
      component.transform.position = new Vector3(enemyFrogBoss.burpPosition.transform.position.x, enemyFrogBoss.burpPosition.transform.position.y, 0.0f);
      if (SettingsManager.Settings.Game.PerformanceMode)
      {
        component.UseLowQualityAnimation();
        component.ArrowImage.transform.localPosition = new Vector3(0.0f, 0.0f, enemyFrogBoss.burpPosition.transform.position.z);
      }
      else
        component.GetComponentInChildren<SkeletonAnimation>().transform.localPosition = new Vector3(0.0f, 0.0f, enemyFrogBoss.burpPosition.transform.position.z);
      component.ModifyingZ = true;
      component.Angle = UnityEngine.Random.Range(-90f, 0.0f);
      component.team = enemyFrogBoss.health.team;
      component.Speed = enemyFrogBoss.baseProjectile.Speed + UnityEngine.Random.Range(-0.5f, 0.5f);
      component.turningSpeed = enemyFrogBoss.baseProjectile.turningSpeed + UnityEngine.Random.Range(-0.1f, 0.1f);
      component.angleNoiseFrequency = enemyFrogBoss.baseProjectile.angleNoiseFrequency + UnityEngine.Random.Range(-0.1f, 0.1f);
      component.LifeTime = enemyFrogBoss.baseProjectile.LifeTime + UnityEngine.Random.Range(0.0f, 0.3f);
      component.Owner = enemyFrogBoss.health;
      component.InvincibleTime = 1f;
      component.SetTarget((Health) enemyFrogBoss.closestPlayerFarming.health);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyFrogBoss.projectileDelayBetweenSpawn.x, enemyFrogBoss.projectileDelayBetweenSpawn.y));
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
  }

  public void BounceAOE() => this.StartCoroutine((IEnumerator) this.BounceAOEIE());

  public IEnumerator BounceAOEIE()
  {
    this.anticipating = true;
    this.anticipationDuration = this.bounceAnticipation;
    yield return (object) new WaitForSeconds(this.anticipationDuration);
    yield return (object) new WaitForEndOfFrame();
    this.LookAt(this.GetAngleToTarget());
    int bouncesCount = (int) UnityEngine.Random.Range(this.bounces.x, this.bounces.y);
    for (int i = 0; i < bouncesCount; ++i)
    {
      this.attacking = true;
      this.Spine.AnimationState.SetAnimation(0, this.bounceAnimation, false);
      yield return (object) new WaitForSeconds(0.4f);
      this.DoAOE();
      this.attacking = false;
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(this.timeBetweenBounce.x, this.timeBetweenBounce.y));
    }
  }

  public void TongueRapidAttack() => this.StartCoroutine((IEnumerator) this.TongueRapidAttackIE());

  public void TongueScatterAttack()
  {
    this.StartCoroutine((IEnumerator) this.TongueScatterAttackIE());
  }

  public IEnumerator TongueRapidAttackIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.usingTongue = true;
    float time = 0.0f;
    int tongueAttackAmount = (int) UnityEngine.Random.Range(enemyFrogBoss.tongueWhipAmount.x, enemyFrogBoss.tongueWhipAmount.y);
    for (int i = 0; i < tongueAttackAmount; ++i)
    {
      enemyFrogBoss.anticipating = true;
      enemyFrogBoss.anticipationDuration = enemyFrogBoss.tongueWhipAnticipation + enemyFrogBoss.tongueSpitDelay;
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.tongueAttackAnimation, false);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.anticipationDuration)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      enemyFrogBoss.facePlayer = false;
      enemyFrogBoss.attacking = true;
      AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_attack");
      Vector3 position = enemyFrogBoss.currentTarget.transform.position;
      enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.GetTongue().SpitTongueIE(position, enemyFrogBoss.tongueSpitDelay, enemyFrogBoss.tongueWhipDuration, enemyFrogBoss.tongueRetrieveDelay, enemyFrogBoss.tongueRetrieveDuration));
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.tongueWhipDuration + (double) enemyFrogBoss.tongueRetrieveDelay + 0.5)
        yield return (object) null;
      AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_return");
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.tongueEndAnimation, false);
      enemyFrogBoss.attacking = false;
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyFrogBoss.facePlayer = true;
    enemyFrogBoss.usingTongue = false;
  }

  public IEnumerator TongueScatterAttackIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.usingTongue = true;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = enemyFrogBoss.tongueWhipAnticipation + enemyFrogBoss.tongueSpitDelay;
    enemyFrogBoss.attacking = true;
    enemyFrogBoss.facePlayer = false;
    int amount = (int) UnityEngine.Random.Range(enemyFrogBoss.tongueScatterAmount.x, enemyFrogBoss.tongueScatterAmount.y);
    int randomTargetPlayerNumber = UnityEngine.Random.Range(0, amount);
    float delay = 0.0f;
    float time = 0.0f;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_attack");
    for (int i = 0; i < amount; ++i)
    {
      Vector3 targetPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * enemyFrogBoss.tongueScatterRadius);
      if (i == randomTargetPlayerNumber)
        targetPosition = enemyFrogBoss.currentTarget.transform.position;
      enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.GetTongue().SpitTongueIE(targetPosition, enemyFrogBoss.tongueScatterWhipDuration, enemyFrogBoss.tongueWhipDuration, (float) ((double) enemyFrogBoss.tongueRetrieveDelay - (double) delay + 0.5), enemyFrogBoss.tongueRetrieveDuration));
      if (i != amount - 1)
      {
        float d = UnityEngine.Random.Range(enemyFrogBoss.tongueScatterDelay.x, enemyFrogBoss.tongueScatterDelay.y);
        delay += d;
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) d)
          yield return (object) null;
      }
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.tongueScatterPreAnticipation)
      yield return (object) null;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.tongueAttackAnimation, false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.tongueWhipDuration + (double) enemyFrogBoss.tongueRetrieveDelay + 0.40000000596046448)
      yield return (object) null;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.tongueScatterPostAnticipation)
      yield return (object) null;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.tongueEndAnimation, false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyFrogBoss.attacking = false;
    enemyFrogBoss.facePlayer = true;
    enemyFrogBoss.usingTongue = false;
  }

  public FrogBossTongue GetTongue()
  {
    foreach (FrogBossTongue tongue in this.tongues)
    {
      if (!tongue.gameObject.activeSelf)
        return tongue;
    }
    FrogBossTongue tongue1 = UnityEngine.Object.Instantiate<FrogBossTongue>(this.tonguePrefab, this.tonguePosition.transform.position, Quaternion.identity, this.transform);
    tongue1.transform.localPosition = Vector3.zero;
    this.tongues.Add(tongue1);
    return tongue1;
  }

  public void SpawnMiniBosses() => this.StartCoroutine((IEnumerator) this.SpawnMiniBossesIE());

  public IEnumerator SpawnMiniBossesIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.facePlayer = false;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.burpAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < (double) enemyFrogBoss.miniBossSpawningDelay)
      yield return (object) null;
    for (int index = 0; index < enemyFrogBoss.miniBossFrogsSpawnAmount; ++index)
    {
      GameObject result = enemyFrogBoss.miniBossFrogTargetHandle.Result;
      if (index > 1)
        result = enemyFrogBoss.miniBossOtherHandle.Result;
      UnitObject component = ObjectPool.Spawn(result, enemyFrogBoss.transform.parent.parent, enemyFrogBoss.miniBossSpawnPositions[index], Quaternion.identity).GetComponent<UnitObject>();
      component.gameObject.SetActive(false);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(component.transform.position, enemyFrogBoss.transform.parent, component.gameObject);
      ShowHPBar showHpBar = component.GetComponent<ShowHPBar>();
      if ((UnityEngine.Object) showHpBar == (UnityEngine.Object) null)
        showHpBar = component.gameObject.AddComponent<ShowHPBar>();
      showHpBar.zOffset = 2f;
      component.GetComponent<Health>().OnDie += new Health.DieAction(enemyFrogBoss.MiniBossKilled);
      enemyFrogBoss.spawnedEnemies.Add(component);
    }
    enemyFrogBoss.facePlayer = true;
  }

  public void MiniBossKilled(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    ++this.miniBossesKilled;
    if (this.miniBossesKilled < this.miniBossFrogsSpawnAmount)
      return;
    this.StartCoroutine((IEnumerator) this.AllMiniBossesKilled());
  }

  public IEnumerator AllMiniBossesKilled()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopDownIE());
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFrogBoss.Spine.timeScale) < 1.5)
      yield return (object) null;
    enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.Phase3IE(false));
  }

  public void AnimationEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "mortar"))
      return;
    this.StartCoroutine((IEnumerator) this.ShootMortarTarget());
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    RoomLockController.RoomCompleted(true, false);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!(bool) (UnityEngine.Object) DungeonSandboxManager.Instance)
      {
        player.health.invincible = true;
        player.health.untouchable = true;
      }
      player.playerWeapon.DoSlowMo(false);
    }
    this.Spine.transform.localPosition = Vector3.zero;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("KILL_BOSS_2"));
    this.Spine.transform.localPosition = Vector3.zero;
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.damageColliderEvents.gameObject.SetActive(false);
    this.KillAllSpawnedEnemies();
    GameManager.GetInstance().CamFollowTarget.MinZoom = 11f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 13f;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/death");
    this.isDead = true;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Die());
  }

  public IEnumerator EnragedIE()
  {
    yield return (object) new WaitForEndOfFrame();
    this.facePlayer = false;
    this.Spine.AnimationState.SetAnimation(0, this.enragedAnimation, false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.enragedDuration)
      yield return (object) null;
    CameraManager.instance.ShakeCameraForDuration(3.5f, 4f, 1f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 1.5)
      yield return (object) null;
    this.facePlayer = true;
  }

  public float GetRandomAngle()
  {
    float randomAngle = (float) UnityEngine.Random.Range(0, 360);
    float num = 32f;
    for (int index = 0; (double) index < (double) num; ++index)
    {
      Vector3 normalized = (Vector3) new Vector2(Mathf.Cos(randomAngle * ((float) Math.PI / 180f)), Mathf.Sin(randomAngle * ((float) Math.PI / 180f))).normalized;
      if ((bool) Physics2D.Raycast((Vector2) (this.transform.position - normalized), (Vector2) normalized, this.physicsCollider.radius * 3f, (int) this.layerToCheck))
        randomAngle = Utils.Repeat(randomAngle + (float) (360.0 / ((double) num + 1.0)), 360f);
      else
        break;
    }
    return randomAngle;
  }

  public float GetAngleToTarget()
  {
    return !((UnityEngine.Object) this.currentTarget != (UnityEngine.Object) null) ? 0.0f : Utils.GetAngle(this.transform.position, this.currentTarget.transform.position);
  }

  public void LookAt(float angle)
  {
    this.state.LookAngle = angle;
    this.state.facingAngle = angle;
  }

  public void OnTriggerEnterEvent(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Moving || collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    Debug.Log((object) "I have hopped into an obstacle", (UnityEngine.Object) this.gameObject);
    this.hasCollidedWithObstacle = true;
    this.targetAngle += (double) this.targetAngle > 180.0 ? -180f : 180f;
    this.LookAt(this.targetAngle);
  }

  public IEnumerator TurnOnDamageColliderForDuration(GameObject collider, float duration)
  {
    collider.SetActive(true);
    yield return (object) new WaitForSeconds(duration);
    collider.SetActive(false);
  }

  public void SpawnEnemy()
  {
    if (this.enemiesAlive >= this.maxEnemies)
      return;
    ++this.enemiesAlive;
    GameObject result = this.enemyHopperTargetHandle.Result;
    if (this.juicedForm && (double) UnityEngine.Random.value < 0.20000000298023224)
      result = this.loadedEnemyRare[UnityEngine.Random.Range(0, this.enemyRare.Length)].Result;
    UnitObject component1 = ObjectPool.Spawn(result, this.transform.parent, this.spawnPositions[UnityEngine.Random.Range(0, this.spawnPositions.Length)], Quaternion.identity).GetComponent<UnitObject>();
    component1.GetComponent<EnemyHopper>().alwaysTargetPlayer = true;
    component1.GetComponent<Collider2D>().isTrigger = true;
    component1.GetComponent<Health>().OnDie += new Health.DieAction(this.Enemy_OnDie);
    this.spawnedEnemies.Add(component1);
    DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
    if ((bool) (UnityEngine.Object) component2)
      component2.GiveXP = false;
    this.spawnTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.spawnDelay.x, this.spawnDelay.y);
  }

  public void Enemy_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.enemiesAlive;
  }

  public IEnumerator Die()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.ClearPaths();
    enemyFrogBoss.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: false);
    GameManager.GetInstance().OnConversationNext(enemyFrogBoss.cameraTarget, 12f);
    enemyFrogBoss.anticipating = false;
    enemyFrogBoss.playerBlocker.SetActive(false);
    enemyFrogBoss.rb.velocity = (Vector2) Vector3.zero;
    enemyFrogBoss.rb.isKinematic = true;
    enemyFrogBoss.rb.simulated = false;
    enemyFrogBoss.rb.bodyType = RigidbodyType2D.Static;
    if ((double) enemyFrogBoss.transform.position.x > 11.0)
      enemyFrogBoss.transform.position = new Vector3(11f, enemyFrogBoss.transform.position.y, 0.0f);
    if ((double) enemyFrogBoss.transform.position.x < -11.0)
      enemyFrogBoss.transform.position = new Vector3(-11f, enemyFrogBoss.transform.position.y, 0.0f);
    if ((double) enemyFrogBoss.transform.position.y > 7.0)
      enemyFrogBoss.transform.position = new Vector3(enemyFrogBoss.transform.position.x, 7f, 0.0f);
    if ((double) enemyFrogBoss.transform.position.y < -7.0)
      enemyFrogBoss.transform.position = new Vector3(enemyFrogBoss.transform.position.x, -7f, 0.0f);
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.simpleSpineFlash.StopAllCoroutines();
    enemyFrogBoss.simpleSpineFlash.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
    enemyFrogBoss.state.CURRENT_STATE = StateMachine.State.Dieing;
    bool beatenLayer2 = DataManager.Instance.BeatenHeketLayer2;
    bool isDeathWithHeart = !DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location) && !DungeonSandboxManager.Active && !DataManager.Instance.SurvivalModeActive;
    if (isDeathWithHeart)
    {
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, "die", false);
      enemyFrogBoss.Spine.AnimationState.AddAnimation(0, "dead", true, 0.0f);
    }
    else
    {
      if (enemyFrogBoss.juicedForm && !DataManager.Instance.BeatenHeketLayer2 && !DungeonSandboxManager.Active)
      {
        enemyFrogBoss.Spine.AnimationState.SetAnimation(0, "die-follower", false);
        yield return (object) new WaitForSeconds(3.33f);
        enemyFrogBoss.Spine.gameObject.SetActive(false);
        PlayerReturnToBase.Disabled = true;
        GameObject Follower = UnityEngine.Object.Instantiate<GameObject>(enemyFrogBoss.followerToSpawn, enemyFrogBoss.transform.position, Quaternion.identity, enemyFrogBoss.transform.parent);
        Follower.GetComponent<Interaction_FollowerSpawn>().Play("CultLeader 2", ScriptLocalization.NAMES_CultLeaders.Dungeon2, cursedState: Thought.BecomeStarving);
        DataManager.SetFollowerSkinUnlocked("CultLeader 2");
        DataManager.Instance.BeatenHeketLayer2 = true;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.NewGamePlus2);
        while ((UnityEngine.Object) Follower != (UnityEngine.Object) null)
          yield return (object) null;
        GameManager.GetInstance().OnConversationEnd();
        Interaction_Chest.Instance?.RevealBossReward(InventoryItem.ITEM_TYPE.GOD_TEAR);
        enemyFrogBoss.KillAllSpawnedEnemies();
        enemyFrogBoss.StopAllCoroutines();
        yield break;
      }
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, "die-noheart", false);
      enemyFrogBoss.Spine.AnimationState.AddAnimation(0, "dead-noheart", true, 0.0f);
      if (!DungeonSandboxManager.Active)
        RoomLockController.RoomCompleted();
    }
    yield return (object) new WaitForSeconds(3.2f);
    GameManager.GetInstance().OnConversationEnd();
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2))
      enemyFrogBoss.interaction_MonsterHeart.ObjectiveToComplete = Objectives.CustomQuestTypes.BishopsOfTheOldFaith2;
    enemyFrogBoss.interaction_MonsterHeart.Play(beatenLayer2 || !GameManager.Layer2 ? InventoryItem.ITEM_TYPE.NONE : InventoryItem.ITEM_TYPE.GOD_TEAR);
    if (isDeathWithHeart)
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, "dead", false);
    else
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, "dead-noheart", true);
    enemyFrogBoss.KillAllSpawnedEnemies();
    enemyFrogBoss.StopAllCoroutines();
  }

  public void KillAllSpawnedEnemies()
  {
    for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null)
      {
        this.spawnedEnemies[index].health.enabled = true;
        this.spawnedEnemies[index].health.DealDamage(this.spawnedEnemies[index].health.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
    foreach (Component tongue in this.tongues)
      tongue.gameObject.SetActive(false);
  }

  public void OnDrawGizmosSelected()
  {
    foreach (Vector3 spawnPosition in this.spawnPositions)
      Utils.DrawCircleXY(spawnPosition, 0.5f, Color.blue);
    foreach (Vector3 bossSpawnPosition in this.miniBossSpawnPositions)
      Utils.DrawCircleXY(bossSpawnPosition, 0.5f, Color.green);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Addressables.Release<GameObject>(this.enemyHopperTargetHandle);
    Addressables.Release<GameObject>(this.miniBossFrogTargetHandle);
    Addressables.Release<GameObject>(this.miniBossOtherHandle);
    if (this.loadedEnemyRare == null)
      return;
    foreach (AsyncOperationHandle<GameObject> handle in this.loadedEnemyRare)
      Addressables.Release((AsyncOperationHandle) handle);
    this.loadedEnemyRare.Clear();
  }

  public void IgnoreCollision(bool ignore)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      Physics2D.IgnoreCollision(this.collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, ignore);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__123_3(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedEnemyRare.Add(obj);
    obj.Result.CreatePool(20, true);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__123_0(AsyncOperationHandle<GameObject> obj)
  {
    this.enemyHopperTargetHandle = obj;
    obj.Result.CreatePool(20, true);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__123_1(AsyncOperationHandle<GameObject> obj)
  {
    this.miniBossFrogTargetHandle = obj;
    obj.Result.CreatePool(3, true);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__123_2(AsyncOperationHandle<GameObject> obj)
  {
    this.miniBossOtherHandle = obj;
    obj.Result.CreatePool(3, true);
  }
}
