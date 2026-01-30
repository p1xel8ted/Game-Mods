// Decompiled with JetBrains decompiler
// Type: EnemyWormBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using FMOD.Studio;
using I2.Loc;
using MMBiomeGeneration;
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
public class EnemyWormBoss : UnitObject
{
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string hitAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string jumpAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string diveAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string popInAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string popOutAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spikeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string summonAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string headSmashAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string enragedAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string dieAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string deadAnimation;
  [Space]
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public Interaction_MonsterHeart interaction_MonsterHeart;
  [SerializeField]
  public ColliderEvents damageCollider;
  [SerializeField]
  public int spikeAmount = 4;
  [SerializeField]
  public GameObject spikePrefab;
  [SerializeField]
  public float spikeAnticipation;
  [SerializeField]
  public float directionalDelayBetweenSpikes = 0.2f;
  [SerializeField]
  public float directionalDistanceBetweenSpikes = 0.2f;
  [SerializeField]
  public int circleSpikeAmount = 4;
  [SerializeField]
  public float circleDelayBetweenSpikes = 0.2f;
  [SerializeField]
  public float circleDistanceBetweenSpikes = 0.2f;
  [SerializeField]
  public float inAirDuration = 2f;
  [SerializeField]
  [Range(0.0f, 1f)]
  [Tooltip("0.75 means will stop targeting the player in the last 25% of in air time")]
  public float targetPercentage = 0.7f;
  [SerializeField]
  public float popOutDelay = 2f;
  [SerializeField]
  public float targetLerpSpeed = 10f;
  [SerializeField]
  public GameObject warningObject;
  [SerializeField]
  public GameObject groundImpactVFX;
  [SerializeField]
  public GameObject bulletPrefab;
  [SerializeField]
  public float shootAnticipation = 1f;
  [SerializeField]
  public Vector2 delayBetweenShots = new Vector2(0.1f, 0.3f);
  [SerializeField]
  public float numberOfShotsToFire = 45f;
  [SerializeField]
  public Vector2 gravSpeed;
  [SerializeField]
  public float arc;
  [SerializeField]
  public Vector2 randomArcOffset = new Vector2(0.0f, 0.0f);
  [SerializeField]
  public Vector2 shootDistanceRange = new Vector2(2f, 3f);
  [SerializeField]
  public GameObject ShootBone;
  [SerializeField]
  public bool bulletsTargetPlayer = true;
  [SerializeField]
  public float moveSpeed = 2.5f;
  [SerializeField]
  public float delayBetweenTrailSpike = 0.2f;
  [SerializeField]
  public GameObject trailSpikePrefab;
  [SerializeField]
  public float headSmashAnticipation = 2f;
  [SerializeField]
  public float zSpacing;
  [SerializeField]
  public Vector3[] headSmashPositions = new Vector3[3];
  [SerializeField]
  public Vector2 p1SpawnAmount;
  [SerializeField]
  public float p1SpawnAnticipation;
  [SerializeField]
  public Vector2 p1DelayBetweenSpawns;
  [SerializeField]
  public AssetReferenceGameObject[] p1SpawnablesList;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float p2HealthThreshold = 0.6f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float p3HealthThreshold = 0.3f;
  [SerializeField]
  public float p3MoveSpeed = 5f;
  [Space]
  [SerializeField]
  public Vector2 p3SpawnAmount;
  [SerializeField]
  public float p3SpawnAnticipation;
  [SerializeField]
  public float enragedDuration = 6f;
  [SerializeField]
  public GameObject[] deathParticlePrefabs;
  [SerializeField]
  public GameObject followerToSpawn;
  public bool targetingPlayer;
  public bool anticipating;
  public bool phaseChangeBlocked;
  public float anticipationTimer;
  public float anticipationDuration;
  public float spawnRadius = 7f;
  public float trailTimer;
  public int currentPhaseNumber = 1;
  public float startingHealth;
  public float ogSpacing;
  public bool queuePhaseIncrement;
  public bool active;
  public bool isDead;
  public List<GameObject> trailSpikes = new List<GameObject>();
  public List<GameObject> spawnedSpikes = new List<GameObject>();
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
  public int startingTrailSpikes = 48 /*0x30*/;
  public int startingSpawnedSpikes = 260;
  public Coroutine currentPhaseRoutine;
  public Coroutine currentAttackRoutine;
  public Coroutine currentSpawnEnemiesRoutine;
  public int previousAttackIndex;
  public bool juicedForm;
  public List<GameObject> loadedP1SpawnablesList;
  public EventInstance loopingSoundInstance;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public GameObject CameraTarget => this.cameraTarget;

  public void Preload()
  {
    this.InitializeGranadeBullets();
    this.InitializeTrailSpikes();
    this.InitializeSpawnedSpikes();
    this.loadedP1SpawnablesList = new List<GameObject>();
    for (int index1 = 0; index1 < this.p1SpawnablesList.Length; ++index1)
      Addressables.LoadAssetAsync<GameObject>((object) this.p1SpawnablesList[index1]).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        this.loadedP1SpawnablesList.Add(obj.Result);
        ObjectPool.CreatePool(obj.Result, Mathf.CeilToInt(this.p1SpawnAmount.y + this.p3SpawnAmount.y));
        List<GameObject> list = new List<GameObject>();
        ObjectPool.GetPooled(obj.Result, list, false);
        if (list.Count > 0)
        {
          SpawnDeadBodyOnDeath component = list[0].gameObject.GetComponent<SpawnDeadBodyOnDeath>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            ObjectPool.CreatePool<DeadBodySliding>(component.deadBodySliding, Mathf.CeilToInt(this.p1SpawnAmount.y + this.p3SpawnAmount.y), true);
        }
        for (int index2 = 0; index2 < list.Count; ++index2)
        {
          foreach (SkeletonAnimation componentsInChild in list[index2].GetComponentsInChildren<SkeletonAnimation>(true))
            componentsInChild.LowestQuality = true;
        }
      });
    foreach (GameObject deathParticlePrefab in this.deathParticlePrefabs)
      ObjectPool.CreatePool(deathParticlePrefab, 1);
    ObjectPool.CreatePool(this.followerToSpawn, 1);
    if (SettingsManager.Settings == null || !SettingsManager.Settings.Game.PerformanceMode)
      return;
    StencilLighting_DecalSprite componentInChildren = this.transform.GetComponentInChildren<StencilLighting_DecalSprite>();
    if (!((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null))
      return;
    componentInChildren.transform.localScale = new Vector3(18f, 22f, 5.31f);
  }

  public void Start()
  {
    this.damageCollider.gameObject.SetActive(false);
    this.damageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.startingHealth = this.health.HP;
    this.ogSpacing = this.Spine.zSpacing;
    if (DataManager.Instance.playerDeathsInARowFightingLeader >= 2)
    {
      this.p1SpawnAmount -= Vector2.one;
      this.p3SpawnAmount -= Vector2.one * 2f;
    }
    this.juicedForm = GameManager.Layer2;
    if (this.juicedForm)
    {
      this.moveSpeed *= 1.5f;
      this.circleDelayBetweenSpikes *= 0.75f;
      this.directionalDelayBetweenSpikes *= 0.75f;
      this.p1SpawnAmount *= 1.5f;
      this.p3SpawnAmount *= 1.5f;
      this.numberOfShotsToFire *= 1.33f;
      this.health.totalHP *= 1.5f;
      this.health.HP = this.health.totalHP;
    }
    this.health.SlowMoOnkill = false;
    if ((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null)
      SkeletonAnimationLODGlobalManager.Instance.DisableCulling(this.Spine.transform, this.Spine);
    SimulationManager.Pause();
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    string name = e.Data.Name;
    switch (name)
    {
      case "diveDown":
        AudioManager.Instance.PlayOneShot("event:/boss/worm/dive_down", AudioManager.Instance.Listener);
        break;
      case "diveUp":
        AudioManager.Instance.PlayOneShot("event:/boss/worm/dive_up", AudioManager.Instance.Listener);
        break;
      case "pushThroughGround":
        AudioManager.Instance.PlayOneShot("event:/boss/worm/push_through_ground", AudioManager.Instance.Listener);
        break;
      case "spawnMiniboss":
        break;
      default:
        int num = name == "spikeAttack" ? 1 : 0;
        break;
    }
  }

  public override void Update()
  {
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
    if (!this.queuePhaseIncrement)
      return;
    this.queuePhaseIncrement = false;
    this.IncrementPhase();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    AudioManager.Instance.PlayOneShot("event:/boss/worm/get_hit", AudioManager.Instance.Listener);
    Vector3 Position = (AttackLocation + Attacker.transform.position) / 2f;
    BiomeConstants.Instance.EmitHitVFX(Position, Quaternion.identity.z, "HitFX_Weak");
    this.simpleSpineFlash.FlashFillRed(0.25f);
    float num = this.health.HP / this.startingHealth;
    if ((this.currentPhaseNumber != 1 || (double) num > (double) this.p2HealthThreshold) && (this.currentPhaseNumber != 2 || (double) num > (double) this.p3HealthThreshold))
      return;
    this.IncrementPhase();
  }

  public void IncrementPhase()
  {
    if (this.phaseChangeBlocked)
    {
      this.queuePhaseIncrement = true;
    }
    else
    {
      if (this.currentAttackRoutine != null)
        this.StopCoroutine(this.currentAttackRoutine);
      if (this.currentSpawnEnemiesRoutine != null)
        this.StopCoroutine(this.currentSpawnEnemiesRoutine);
      if (this.currentPhaseRoutine != null)
        this.StopCoroutine(this.currentPhaseRoutine);
      ++this.currentPhaseNumber;
      foreach (GameObject spawnedSpike in this.spawnedSpikes)
        spawnedSpike.SetActive(false);
      this.anticipating = false;
      this.health.invincible = false;
      if (this.currentPhaseNumber == 2)
      {
        this.BeginPhase2();
      }
      else
      {
        if (this.currentPhaseNumber != 3)
          return;
        this.BeginPhase3();
      }
    }
  }

  public override void OnDisable()
  {
    base.OnDisable();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.active)
      return;
    if (this.currentAttackRoutine != null)
      this.StopCoroutine(this.currentAttackRoutine);
    if (this.currentPhaseRoutine != null)
      this.StopCoroutine(this.currentSpawnEnemiesRoutine);
    if (this.currentPhaseRoutine != null)
      this.StopCoroutine(this.currentPhaseRoutine);
    this.warningObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.DelayAddCamera());
    if (this.currentPhaseNumber == 1)
      this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase1IE(false));
    else if (this.currentPhaseNumber == 2)
    {
      this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase2IE(false));
    }
    else
    {
      if (this.currentPhaseNumber != 3)
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
    this.health.untouchable = false;
    GameManager.GetInstance().AddToCamera(this.cameraTarget);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase1IE(true));
  }

  public IEnumerator Phase1IE(bool firstLoop)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.active = true;
    if (firstLoop)
      yield return (object) (enemyWormBoss.currentSpawnEnemiesRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnIE(enemyWormBoss.loadedP1SpawnablesList, (int) UnityEngine.Random.Range(enemyWormBoss.p1SpawnAmount.x, enemyWormBoss.p1SpawnAmount.y + 1f), enemyWormBoss.p1SpawnAnticipation, enemyWormBoss.p1DelayBetweenSpawns)));
    for (int i = 0; i < 3; ++i)
    {
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.moveSpeed, enemyWormBoss.GetRandomPosition(7f), true)));
      if (i == 2)
      {
        if (enemyWormBoss.juicedForm && (double) UnityEngine.Random.value > 0.5 && (UnityEngine.Object) enemyWormBoss.GetClosestTarget() != (UnityEngine.Object) null)
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootTargetedSpikesInDirectionIE(enemyWormBoss.directionalDelayBetweenSpikes / 3f, enemyWormBoss.directionalDistanceBetweenSpikes)));
        else
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnSpikesInDirectionsIE(enemyWormBoss.spikeAmount, enemyWormBoss.directionalDelayBetweenSpikes, enemyWormBoss.directionalDistanceBetweenSpikes)));
      }
      else
        yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootIE()));
    }
    enemyWormBoss.currentPhaseRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.Phase1IE(false));
  }

  public void BeginPhase2()
  {
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase2IE(true));
  }

  public IEnumerator Phase2IE(bool firstLoop)
  {
    EnemyWormBoss enemyWormBoss = this;
    if (firstLoop)
    {
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.p3MoveSpeed, enemyWormBoss.GetRandomPosition(7f), false)));
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.EnragedIE());
      yield return (object) new WaitForSeconds(2f);
      yield return (object) (enemyWormBoss.currentSpawnEnemiesRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnIE(enemyWormBoss.loadedP1SpawnablesList, (int) UnityEngine.Random.Range(enemyWormBoss.p1SpawnAmount.x, enemyWormBoss.p1SpawnAmount.y + 1f), enemyWormBoss.p1SpawnAnticipation, enemyWormBoss.p1DelayBetweenSpawns, false)));
      yield return (object) new WaitForSeconds(2f);
    }
    for (int i = 0; i < 3; ++i)
    {
      int num = enemyWormBoss.previousAttackIndex;
      while (enemyWormBoss.previousAttackIndex == num)
        num = UnityEngine.Random.Range(0, 3);
      enemyWormBoss.previousAttackIndex = num;
      switch (num)
      {
        case 0:
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.JumpDiveIE()));
          break;
        case 1:
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.moveSpeed, enemyWormBoss.GetRandomPosition(7f), true)));
          if (enemyWormBoss.juicedForm && (double) UnityEngine.Random.value > 0.5 && (UnityEngine.Object) enemyWormBoss.GetClosestTarget() != (UnityEngine.Object) null)
          {
            yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootTargetedSpikesInDirectionIE(enemyWormBoss.directionalDelayBetweenSpikes / 3f, enemyWormBoss.directionalDistanceBetweenSpikes)));
            break;
          }
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnSpikesInDirectionsIE(enemyWormBoss.circleSpikeAmount, enemyWormBoss.circleDelayBetweenSpikes, enemyWormBoss.circleDistanceBetweenSpikes)));
          break;
        case 2:
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.moveSpeed, enemyWormBoss.GetRandomPosition(7f), true)));
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootIE()));
          break;
      }
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));
    }
    enemyWormBoss.currentPhaseRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.Phase2IE(false));
  }

  public void BeginPhase3()
  {
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase3IE(true));
  }

  public IEnumerator Phase3IE(bool firstLoop)
  {
    EnemyWormBoss enemyWormBoss = this;
    if (firstLoop)
    {
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.p3MoveSpeed, Vector3.zero, false)));
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.EnragedIE());
      yield return (object) new WaitForSeconds(2f);
      yield return (object) (enemyWormBoss.currentSpawnEnemiesRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnIE(enemyWormBoss.loadedP1SpawnablesList, (int) UnityEngine.Random.Range(enemyWormBoss.p3SpawnAmount.x, enemyWormBoss.p3SpawnAmount.y + 1f), enemyWormBoss.p3SpawnAnticipation, enemyWormBoss.p1DelayBetweenSpawns, false)));
      yield return (object) new WaitForSeconds(2f);
    }
    for (int i = 1; i < 3; ++i)
    {
      if (i % 2 == 0)
      {
        if (enemyWormBoss.juicedForm && (double) UnityEngine.Random.value > 0.5 && (UnityEngine.Object) enemyWormBoss.GetClosestTarget() != (UnityEngine.Object) null)
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootTargetedSpikesInDirectionIE(enemyWormBoss.directionalDelayBetweenSpikes / 3f, enemyWormBoss.directionalDistanceBetweenSpikes)));
        else
          yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.p3MoveSpeed, enemyWormBoss.GetRandomPosition(7f), true)));
      }
      else
        yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.HeadSmashIE()));
    }
    if (UnityEngine.Random.Range(0, 2) == 0)
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnSpikesInDirectionsIE(enemyWormBoss.circleSpikeAmount, enemyWormBoss.circleDelayBetweenSpikes, enemyWormBoss.circleDistanceBetweenSpikes)));
    else
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootIE()));
    enemyWormBoss.currentPhaseRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.Phase3IE(false));
  }

  public void JumpDive() => this.StartCoroutine((IEnumerator) this.JumpDiveIE());

  public IEnumerator JumpDiveIE()
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.Spine.ForceVisible = true;
    enemyWormBoss.phaseChangeBlocked = true;
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.jumpAnimation, false);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.2f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyWormBoss.health.invincible = true;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 0.75)
      yield return (object) null;
    enemyWormBoss.warningObject.SetActive(true);
    enemyWormBoss.targetingPlayer = true;
    Health target = enemyWormBoss.GetClosestTarget();
    target = (UnityEngine.Object) target != (UnityEngine.Object) null ? target : (Health) PlayerFarming.Instance.health;
    Vector3 position = target.transform.position;
    enemyWormBoss.transform.position = position;
    float dur = enemyWormBoss.inAirDuration * enemyWormBoss.targetPercentage;
    float t = 0.0f;
    while ((double) t < (double) dur)
    {
      enemyWormBoss.transform.position = Vector3.Lerp(enemyWormBoss.transform.position, target.transform.position, enemyWormBoss.targetLerpSpeed * Time.deltaTime);
      t += Time.deltaTime * enemyWormBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyWormBoss.warningObject.transform.localPosition = Vector3.zero;
    enemyWormBoss.targetingPlayer = false;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) enemyWormBoss.inAirDuration - (double) t)
      yield return (object) null;
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.diveAnimation, false);
    enemyWormBoss.warningObject.SetActive(false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyWormBoss.damageCollider.gameObject.SetActive(true);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.2f, 0.2f);
    if (enemyWormBoss.juicedForm)
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnSpikesInDirectionsIE(enemyWormBoss.circleSpikeAmount, enemyWormBoss.circleDelayBetweenSpikes / 2f, enemyWormBoss.circleDistanceBetweenSpikes, false));
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    if ((UnityEngine.Object) enemyWormBoss.groundImpactVFX != (UnityEngine.Object) null)
    {
      ParticleSystem component = enemyWormBoss.groundImpactVFX.GetComponent<ParticleSystem>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.Play();
    }
    enemyWormBoss.damageCollider.gameObject.SetActive(false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) enemyWormBoss.popOutDelay)
      yield return (object) null;
    enemyWormBoss.health.invincible = false;
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.popInAnimation, false);
    enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyWormBoss.phaseChangeBlocked = false;
    enemyWormBoss.Spine.ForceVisible = false;
  }

  public void SpawnSpikesInDirections()
  {
    this.StartCoroutine((IEnumerator) this.SpawnSpikesInDirectionsIE(this.spikeAmount, this.directionalDelayBetweenSpikes, this.directionalDistanceBetweenSpikes));
  }

  public void SpawnSpikesInCircle()
  {
    this.StartCoroutine((IEnumerator) this.SpawnSpikesInDirectionsIE(this.circleSpikeAmount, this.circleDelayBetweenSpikes, this.circleDistanceBetweenSpikes));
  }

  public void InitializeSpawnedSpikes()
  {
    for (int index = 0; index < this.startingSpawnedSpikes; ++index)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spikePrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      gameObject.SetActive(false);
      this.spawnedSpikes.Add(gameObject);
      SkeletonAnimation componentInChildren1 = gameObject.GetComponentInChildren<SkeletonAnimation>(true);
      if (SettingsManager.Settings.Game.PerformanceMode)
      {
        gameObject.GetComponent<SimpleSpineDeactivateAfterPlay>().UseLowQualitySpikes(SimpleSpineDeactivateAfterPlay.SpikeType.EnemyWormBoss);
      }
      else
      {
        if ((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null)
          SkeletonAnimationLODGlobalManager.Instance.AddMinimumQualityLOD(gameObject.transform, componentInChildren1);
        SkeletonAnimationLODManager component = gameObject.transform.GetComponent<SkeletonAnimationLODManager>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.IgnoreCulling = true;
      }
      ColliderEvents componentInChildren2 = gameObject.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren2)
        componentInChildren2.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
  }

  public IEnumerator SpawnSpikesInDirectionsIE(
    int amount,
    float delayBetweenSpikes,
    float distanceBetweenSpikes,
    bool anticipate = true)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.phaseChangeBlocked = true;
    yield return (object) new WaitForEndOfFrame();
    int num = UnityEngine.Random.Range(0, 360);
    for (int index = 0; index < amount; ++index)
    {
      Vector3 direction = new Vector3(Mathf.Cos((float) num * ((float) Math.PI / 180f)), Mathf.Sin((float) num * ((float) Math.PI / 180f)), 0.0f);
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootSpikesInDirectionIE(direction, delayBetweenSpikes, distanceBetweenSpikes, anticipate));
      num = (int) Utils.Repeat((float) (num + 360 / amount), 360f);
    }
    AudioManager.Instance.PlayOneShot("event:/boss/worm/spike_attack", AudioManager.Instance.Listener);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) enemyWormBoss.spikeAnticipation + (double) delayBetweenSpikes + 1.0)
      yield return (object) null;
    enemyWormBoss.phaseChangeBlocked = false;
  }

  public IEnumerator ShootSpikesInDirectionIE(
    Vector3 direction,
    float delayBetweenSpikes,
    float distanceBetweenSpikes,
    bool anticipate = true)
  {
    EnemyWormBoss enemyWormBoss = this;
    float time;
    if (anticipate)
    {
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.spikeAnimation, false);
      enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
      enemyWormBoss.anticipating = true;
      enemyWormBoss.anticipationDuration = enemyWormBoss.spikeAnticipation;
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) enemyWormBoss.spikeAnticipation)
        yield return (object) null;
      CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, 0.5f);
      yield return (object) new WaitForEndOfFrame();
    }
    Vector3 position = enemyWormBoss.transform.position;
    for (int i = 0; i < 30; ++i)
    {
      enemyWormBoss.GetSpawnSpike().transform.position = position;
      position += direction * distanceBetweenSpikes;
      if ((bool) Physics2D.Raycast((Vector2) position, (Vector2) direction, 1f, (int) enemyWormBoss.layerToCheck))
        break;
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) delayBetweenSpikes)
        yield return (object) null;
    }
  }

  public IEnumerator ShootTargetedSpikesInDirectionIE(
    float delayBetweenSpikes,
    float distanceBetweenSpikes)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.spikeAnimation, false);
    enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
    enemyWormBoss.anticipating = true;
    enemyWormBoss.anticipationDuration = enemyWormBoss.spikeAnticipation;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) enemyWormBoss.spikeAnticipation)
      yield return (object) null;
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, 0.5f);
    yield return (object) new WaitForEndOfFrame();
    Vector3 normalized = (enemyWormBoss.GetClosestTarget().transform.position - enemyWormBoss.transform.position).normalized;
    Vector3 direction1 = Quaternion.Euler(0.0f, 0.0f, -25f) * normalized;
    Vector3 direction2 = Quaternion.Euler(0.0f, 0.0f, 25f) * normalized;
    enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootSpikesInDirectionIE(direction1, delayBetweenSpikes, distanceBetweenSpikes, false));
    enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootSpikesInDirectionIE(direction2, delayBetweenSpikes, distanceBetweenSpikes, false));
    yield return (object) enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootSpikesInDirectionIE(normalized, delayBetweenSpikes, distanceBetweenSpikes, false));
  }

  public GameObject GetSpawnSpike()
  {
    GameObject spawnSpike = (GameObject) null;
    if (this.spawnedSpikes.Count > 0)
    {
      foreach (GameObject spawnedSpike in this.spawnedSpikes)
      {
        if (!spawnedSpike.activeSelf)
        {
          spawnSpike = spawnedSpike;
          spawnSpike.transform.position = this.transform.position;
          spawnSpike.SetActive(true);
          break;
        }
      }
    }
    if ((UnityEngine.Object) spawnSpike == (UnityEngine.Object) null)
    {
      spawnSpike = UnityEngine.Object.Instantiate<GameObject>(this.spikePrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      this.spawnedSpikes.Add(spawnSpike);
      if (SettingsManager.Settings.Game.PerformanceMode)
        spawnSpike.GetComponent<SimpleSpineDeactivateAfterPlay>().UseLowQualitySpikes(SimpleSpineDeactivateAfterPlay.SpikeType.EnemyWormBoss);
      ColliderEvents componentInChildren = spawnSpike.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren)
        componentInChildren.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    return spawnSpike;
  }

  public void Shoot() => this.StartCoroutine((IEnumerator) this.ShootIE());

  public void InitializeGranadeBullets()
  {
    ObjectPool.CreatePool(this.bulletPrefab, (int) this.numberOfShotsToFire);
  }

  public IEnumerator ShootIE()
  {
    EnemyWormBoss enemyWormBoss = this;
    AudioManager.Instance.PlayOneShot("event:/boss/worm/spit_projectiles", AudioManager.Instance.Listener);
    enemyWormBoss.anticipating = true;
    enemyWormBoss.anticipationDuration = enemyWormBoss.shootAnticipation;
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.shootAnimation, false);
    enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) enemyWormBoss.shootAnticipation)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.phaseChangeBlocked = true;
    Health closestTarget = enemyWormBoss.GetClosestTarget();
    Health health = (UnityEngine.Object) closestTarget != (UnityEngine.Object) null ? closestTarget : (Health) PlayerFarming.Instance.health;
    float angle = Utils.GetAngle(enemyWormBoss.transform.position, health.transform.position);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.4f);
    int index = -1;
    while ((double) ++index < (double) enemyWormBoss.numberOfShotsToFire)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", AudioManager.Instance.Listener);
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.BulletDelay(angle, index, UnityEngine.Random.Range(enemyWormBoss.delayBetweenShots.x, enemyWormBoss.delayBetweenShots.y)));
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyWormBoss.phaseChangeBlocked = false;
  }

  public IEnumerator BulletDelay(float shootAngle, int index, float delay)
  {
    EnemyWormBoss enemyWormBoss = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) delay)
      yield return (object) null;
    Vector3 position = enemyWormBoss.ShootBone.transform.position;
    GrenadeBullet component = ObjectPool.Spawn(enemyWormBoss.bulletPrefab, position, Quaternion.identity).GetComponent<GrenadeBullet>();
    component.SetOwner(enemyWormBoss.gameObject);
    component.Play(-6f, (float) ((double) shootAngle - (double) enemyWormBoss.arc / 2.0 + (double) enemyWormBoss.arc / (double) enemyWormBoss.numberOfShotsToFire * (double) index) + UnityEngine.Random.Range(enemyWormBoss.randomArcOffset.x, enemyWormBoss.randomArcOffset.y), UnityEngine.Random.Range(enemyWormBoss.shootDistanceRange.x, enemyWormBoss.shootDistanceRange.y), UnityEngine.Random.Range(enemyWormBoss.gravSpeed.x, enemyWormBoss.gravSpeed.y));
  }

  public void SpawnP1()
  {
    this.StartCoroutine((IEnumerator) this.SpawnIE(this.loadedP1SpawnablesList, (int) UnityEngine.Random.Range(this.p1SpawnAmount.x, this.p1SpawnAmount.y + 1f), this.p1SpawnAnticipation, this.p1DelayBetweenSpawns));
  }

  public void SpawnP3()
  {
    this.StartCoroutine((IEnumerator) this.SpawnIE(this.loadedP1SpawnablesList, (int) UnityEngine.Random.Range(this.p3SpawnAmount.x, this.p3SpawnAmount.y + 1f), this.p3SpawnAnticipation, this.p1DelayBetweenSpawns));
  }

  public IEnumerator SpawnIE(
    List<GameObject> spawnables,
    int amount,
    float anticipationTime,
    Vector2 delayBetweenSpawns,
    bool playAnimations = true)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.phaseChangeBlocked = true;
    AudioManager.Instance.PlayOneShot("event:/boss/worm/spike_attack", AudioManager.Instance.Listener);
    float time = 0.0f;
    yield return (object) new WaitForEndOfFrame();
    if (playAnimations)
    {
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.summonAnimation, false);
      enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) anticipationTime)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
    }
    for (int i = 0; i < amount; ++i)
    {
      Vector3 Position = (Vector3) (UnityEngine.Random.insideUnitCircle * enemyWormBoss.spawnRadius);
      if (!BiomeGenerator.Instance.CurrentRoom.Completed || DungeonSandboxManager.Active)
      {
        UnitObject component1 = EnemySpawner.Create(Position, enemyWormBoss.transform.parent, spawnables[UnityEngine.Random.Range(0, spawnables.Count)]).GetComponent<UnitObject>();
        component1.CanHaveModifier = false;
        component1.RemoveModifier();
        enemyWormBoss.spawnedEnemies.Add(component1);
        DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
        if ((bool) (UnityEngine.Object) component2)
          component2.GiveXP = false;
        time = 0.0f;
        float dur = UnityEngine.Random.Range(delayBetweenSpawns.x, delayBetweenSpawns.y);
        while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) dur)
          yield return (object) null;
      }
      else
        break;
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyWormBoss.phaseChangeBlocked = false;
  }

  public IEnumerator SpawnIE(
    UnitObject[] spawnables,
    int amount,
    Vector3[] spawnPositions,
    float anticipationTime)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.phaseChangeBlocked = true;
    AudioManager.Instance.PlayOneShot("event:/boss/worm/spike_attack", AudioManager.Instance.Listener);
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.summonAnimation, false);
    enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) anticipationTime)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
    for (int index = 0; index < amount; ++index)
    {
      if (!enemyWormBoss.isDead)
      {
        Vector3 position = (Vector3) (UnityEngine.Random.insideUnitCircle * enemyWormBoss.spawnRadius);
        UnitObject unitObject = ObjectPool.Spawn<UnitObject>(spawnables[UnityEngine.Random.Range(0, spawnables.Length)], enemyWormBoss.transform.parent, position, Quaternion.identity);
        unitObject.CanHaveModifier = false;
        unitObject.RemoveModifier();
        unitObject.transform.position = spawnPositions[index];
        enemyWormBoss.spawnedEnemies.Add(unitObject);
        DropLootOnDeath component = unitObject.GetComponent<DropLootOnDeath>();
        if ((bool) (UnityEngine.Object) component)
          component.GiveXP = false;
      }
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyWormBoss.phaseChangeBlocked = false;
  }

  public void TunnelMove()
  {
    this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.TunnelMoveIE(this.moveSpeed, this.GetRandomPosition(7f), true));
  }

  public void InitializeTrailSpikes()
  {
    for (int index = 0; index < this.startingTrailSpikes; ++index)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.trailSpikePrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      gameObject.SetActive(false);
      SkeletonAnimation componentInChildren1 = gameObject.GetComponentInChildren<SkeletonAnimation>(true);
      if (SettingsManager.Settings.Game.PerformanceMode)
        gameObject.GetComponent<SimpleSpineDeactivateAfterPlay>().UseLowQualitySpikes(SimpleSpineDeactivateAfterPlay.SpikeType.EnemyWormBoss);
      else if ((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null)
        SkeletonAnimationLODGlobalManager.Instance.DisableCulling(gameObject.transform, componentInChildren1);
      this.trailSpikes.Add(gameObject);
      ColliderEvents componentInChildren2 = gameObject.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren2)
        componentInChildren2.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
  }

  public IEnumerator TunnelMoveIE(float moveSpeed, Vector3 position, bool popOut)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.phaseChangeBlocked = true;
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.popOutAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", enemyWormBoss.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyWormBoss.health.invincible = true;
    enemyWormBoss.Spine.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_disappear_underground", enemyWormBoss.gameObject);
    float Progress = 0.0f;
    float Duration = 0.366666675f;
    while ((double) (Progress += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) Duration)
    {
      enemyWormBoss.SpawnTrailSpikes();
      yield return (object) null;
    }
    enemyWormBoss.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/enemy/tunnel_worm/tunnel_worm_underground_loop", enemyWormBoss.gameObject, true);
    Vector3 startPosition = enemyWormBoss.transform.position;
    Progress = 0.0f;
    Duration = Vector3.Distance(startPosition, position) / moveSpeed;
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.3f, Duration);
    while ((double) (Progress += Time.deltaTime * enemyWormBoss.Spine.timeScale) < (double) Duration)
    {
      enemyWormBoss.transform.position = Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      enemyWormBoss.SpawnTrailSpikes();
      yield return (object) null;
    }
    enemyWormBoss.transform.position = position;
    enemyWormBoss.health.invincible = false;
    enemyWormBoss.Spine.gameObject.SetActive(true);
    AudioManager.Instance.StopLoop(enemyWormBoss.loopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", enemyWormBoss.gameObject);
    yield return (object) null;
    enemyWormBoss.simpleSpineFlash.FlashWhite(0.0f);
    if (popOut)
    {
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.popInAnimation, false);
      enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 1.5)
        yield return (object) null;
      enemyWormBoss.phaseChangeBlocked = false;
    }
    else
      enemyWormBoss.phaseChangeBlocked = false;
  }

  public void SpawnTrailSpikes()
  {
    if ((double) (this.trailTimer += Time.deltaTime) <= (double) this.delayBetweenTrailSpike)
      return;
    this.trailTimer = 0.0f;
    this.GetTrailSpike();
  }

  public GameObject GetTrailSpike()
  {
    GameObject trailSpike1 = (GameObject) null;
    if (this.trailSpikes.Count > 0)
    {
      foreach (GameObject trailSpike2 in this.trailSpikes)
      {
        if (!trailSpike2.activeSelf)
        {
          trailSpike1 = trailSpike2;
          trailSpike1.transform.position = this.transform.position;
          trailSpike1.SetActive(true);
          break;
        }
      }
    }
    if ((UnityEngine.Object) trailSpike1 == (UnityEngine.Object) null)
    {
      if (SettingsManager.Settings.Game.PerformanceMode)
        trailSpike1.GetComponent<SimpleSpineDeactivateAfterPlay>().UseLowQualitySpikes(SimpleSpineDeactivateAfterPlay.SpikeType.EnemyWormBoss);
      this.trailSpikes.Add(trailSpike1);
      ColliderEvents componentInChildren = trailSpike1.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren)
        componentInChildren.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    return trailSpike1;
  }

  public void HeadSmash() => this.StartCoroutine((IEnumerator) this.HeadSmashIE());

  public IEnumerator HeadSmashIE()
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.anticipating = true;
    enemyWormBoss.anticipationDuration = enemyWormBoss.headSmashAnticipation;
    yield return (object) new WaitForSeconds(enemyWormBoss.headSmashAnticipation);
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.phaseChangeBlocked = true;
    AudioManager.Instance.PlayOneShot("event:/boss/worm/slam_attack", enemyWormBoss.gameObject);
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.headSmashAnimation, false);
    enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
    enemyWormBoss.Spine.zSpacing = enemyWormBoss.zSpacing;
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(1.3f, 1.5f, 0.3f);
    yield return (object) enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.EnableDamageCollider(0.1f, enemyWormBoss.headSmashPositions[0]));
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.instance.ShakeCameraForDuration(1.3f, 1.5f, 0.3f);
    yield return (object) enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.EnableDamageCollider(0.1f, enemyWormBoss.headSmashPositions[1]));
    yield return (object) new WaitForSeconds(0.8f);
    CameraManager.instance.ShakeCameraForDuration(1.6f, 1.8f, 0.3f);
    yield return (object) enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.EnableDamageCollider(0.1f, enemyWormBoss.headSmashPositions[2]));
    DOTween.To(new DOGetter<float>(enemyWormBoss.\u003CHeadSmashIE\u003Eb__127_0), new DOSetter<float>(enemyWormBoss.\u003CHeadSmashIE\u003Eb__127_1), enemyWormBoss.ogSpacing, 0.25f);
    yield return (object) new WaitForSeconds(1f);
    enemyWormBoss.phaseChangeBlocked = false;
  }

  public IEnumerator EnableDamageCollider(float time, Vector3 position)
  {
    this.damageCollider.transform.localPosition = position;
    this.damageCollider.gameObject.SetActive(true);
    yield return (object) new WaitForSeconds(time);
    this.damageCollider.gameObject.SetActive(false);
  }

  public IEnumerator EnragedIE()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/worm/roar", AudioManager.Instance.Listener);
    yield return (object) new WaitForEndOfFrame();
    this.Spine.AnimationState.SetAnimation(0, this.enragedAnimation, false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.enragedDuration)
      yield return (object) null;
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 1f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 1.5)
      yield return (object) null;
  }

  public Vector3 GetRandomPosition(float radius)
  {
    return this.juicedForm && (double) UnityEngine.Random.value > 0.5 && (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null ? this.GetClosestTarget().transform.position : (Vector3) (UnityEngine.Random.insideUnitCircle * radius);
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
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
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("KILL_BOSS_1"));
    this.damageCollider.gameObject.SetActive(false);
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.KillAllSpawnedEnemies();
    this.isDead = true;
    GameManager.GetInstance().CamFollowTarget.MinZoom = 11f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 13f;
    if (this.currentAttackRoutine != null)
      this.StopCoroutine(this.currentAttackRoutine);
    if (this.currentPhaseRoutine != null)
      this.StopCoroutine(this.currentSpawnEnemiesRoutine);
    if (this.currentPhaseRoutine != null)
      this.StopCoroutine(this.currentPhaseRoutine);
    this.StartCoroutine((IEnumerator) this.Die());
  }

  public IEnumerator Die()
  {
    EnemyWormBoss enemyWormBoss = this;
    foreach (GameObject deathParticlePrefab in enemyWormBoss.deathParticlePrefabs)
      ObjectPool.Spawn(deathParticlePrefab, enemyWormBoss.transform.parent, enemyWormBoss.transform.position, Quaternion.identity);
    enemyWormBoss.ClearPaths();
    enemyWormBoss.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: false);
    GameManager.GetInstance().OnConversationNext(enemyWormBoss.cameraTarget, 12f);
    enemyWormBoss.anticipating = false;
    enemyWormBoss.Spine.zSpacing = enemyWormBoss.ogSpacing;
    enemyWormBoss.rb.velocity = (Vector2) Vector3.zero;
    enemyWormBoss.rb.isKinematic = true;
    enemyWormBoss.rb.simulated = false;
    enemyWormBoss.rb.bodyType = RigidbodyType2D.Static;
    if ((double) enemyWormBoss.transform.position.x > 11.0)
      enemyWormBoss.transform.position = new Vector3(11f, enemyWormBoss.transform.position.y, 0.0f);
    if ((double) enemyWormBoss.transform.position.x < -11.0)
      enemyWormBoss.transform.position = new Vector3(-11f, enemyWormBoss.transform.position.y, 0.0f);
    if ((double) enemyWormBoss.transform.position.y > 7.0)
      enemyWormBoss.transform.position = new Vector3(enemyWormBoss.transform.position.x, 7f, 0.0f);
    if ((double) enemyWormBoss.transform.position.y < -7.0)
      enemyWormBoss.transform.position = new Vector3(enemyWormBoss.transform.position.x, -7f, 0.0f);
    yield return (object) new WaitForEndOfFrame();
    float time = 0.0f;
    enemyWormBoss.simpleSpineFlash.StopAllCoroutines();
    enemyWormBoss.simpleSpineFlash.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
    enemyWormBoss.state.CURRENT_STATE = StateMachine.State.Dieing;
    AudioManager.Instance.PlayOneShot("event:/boss/worm/death", AudioManager.Instance.Listener);
    bool beatenLayer2 = DataManager.Instance.BeatenLeshyLayer2;
    bool isDeathWithHeart = !DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location) && !DungeonSandboxManager.Active && !DataManager.Instance.SurvivalModeActive;
    if (isDeathWithHeart)
    {
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, "die", false);
      enemyWormBoss.Spine.AnimationState.AddAnimation(0, "dead", true, 0.0f);
    }
    else
    {
      if (enemyWormBoss.juicedForm && !DataManager.Instance.BeatenLeshyLayer2 && !DungeonSandboxManager.Active)
      {
        enemyWormBoss.Spine.AnimationState.SetAnimation(0, "die-follower", false);
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 4.5)
          yield return (object) null;
        enemyWormBoss.Spine.gameObject.SetActive(false);
        PlayerReturnToBase.Disabled = true;
        GameObject Follower = ObjectPool.Spawn(enemyWormBoss.followerToSpawn, enemyWormBoss.transform.parent, enemyWormBoss.transform.position, Quaternion.identity);
        Follower.GetComponent<Interaction_FollowerSpawn>().Play("CultLeader 1", ScriptLocalization.NAMES_CultLeaders.Dungeon1);
        DataManager.SetFollowerSkinUnlocked("CultLeader 1");
        DataManager.Instance.BeatenLeshyLayer2 = true;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.NewGamePlus1);
        while ((UnityEngine.Object) Follower != (UnityEngine.Object) null)
          yield return (object) null;
        GameManager.GetInstance().OnConversationEnd();
        Interaction_Chest.Instance?.RevealBossReward(InventoryItem.ITEM_TYPE.GOD_TEAR);
        enemyWormBoss.KillAllSpawnedEnemies();
        enemyWormBoss.StopAllCoroutines();
        yield break;
      }
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, "die-noheart", false);
      enemyWormBoss.Spine.AnimationState.AddAnimation(0, "dead-noheart", true, 0.0f);
      if (!DungeonSandboxManager.Active)
        RoomLockController.RoomCompleted();
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormBoss.Spine.timeScale) < 4.1999998092651367)
      yield return (object) null;
    for (int index = 0; index < 20; ++index)
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemyWormBoss.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f), Vector3.zero, Color.red);
    GameManager.GetInstance().OnConversationEnd();
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      enemyWormBoss.interaction_MonsterHeart.ObjectiveToComplete = Objectives.CustomQuestTypes.BishopsOfTheOldFaith1;
    enemyWormBoss.interaction_MonsterHeart.Play(beatenLayer2 || !GameManager.Layer2 ? InventoryItem.ITEM_TYPE.NONE : InventoryItem.ITEM_TYPE.GOD_TEAR);
    if (isDeathWithHeart)
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, "dead", false);
    else
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, "dead-noheart", true);
    enemyWormBoss.StopAllCoroutines();
  }

  public void KillAllSpawnedEnemies()
  {
    foreach (UnitObject spawnedEnemy in this.spawnedEnemies)
    {
      if ((UnityEngine.Object) spawnedEnemy != (UnityEngine.Object) null)
      {
        spawnedEnemy.health.enabled = true;
        spawnedEnemy.health.invincible = false;
        spawnedEnemy.health.untouchable = false;
        spawnedEnemy.health.DealDamage(spawnedEnemy.health.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health)
      {
        Health.team2[index].enabled = true;
        Health.team2[index].invincible = false;
        Health.team2[index].untouchable = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
  }

  public void OnDrawGizmosSelected()
  {
    foreach (Vector3 headSmashPosition in this.headSmashPositions)
      Utils.DrawCircleXY(this.transform.InverseTransformPoint(headSmashPosition), 0.5f, Color.yellow);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__86_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    this.loadedP1SpawnablesList.Add(obj.Result);
    ObjectPool.CreatePool(obj.Result, Mathf.CeilToInt(this.p1SpawnAmount.y + this.p3SpawnAmount.y));
    List<GameObject> list = new List<GameObject>();
    ObjectPool.GetPooled(obj.Result, list, false);
    if (list.Count > 0)
    {
      SpawnDeadBodyOnDeath component = list[0].gameObject.GetComponent<SpawnDeadBodyOnDeath>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        ObjectPool.CreatePool<DeadBodySliding>(component.deadBodySliding, Mathf.CeilToInt(this.p1SpawnAmount.y + this.p3SpawnAmount.y), true);
    }
    for (int index = 0; index < list.Count; ++index)
    {
      foreach (SkeletonAnimation componentsInChild in list[index].GetComponentsInChildren<SkeletonAnimation>(true))
        componentsInChild.LowestQuality = true;
    }
  }

  [CompilerGenerated]
  public float \u003CHeadSmashIE\u003Eb__127_0() => this.Spine.zSpacing;

  [CompilerGenerated]
  public void \u003CHeadSmashIE\u003Eb__127_1(float x) => this.Spine.zSpacing = x;
}
