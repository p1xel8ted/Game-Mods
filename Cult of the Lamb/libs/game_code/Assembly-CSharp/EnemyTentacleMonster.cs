// Decompiled with JetBrains decompiler
// Type: EnemyTentacleMonster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using CotL.Projectiles;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyTentacleMonster : UnitObject
{
  public SkeletonAnimation spine;
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootProjectilesAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string holyHandGrenadeAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string holyHandGrenadeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string sweepingSpawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string randomSpawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string teleportAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string daggerAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string swordAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string swordLoopAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string swordAttackAnimation;
  [Space]
  [SerializeField]
  public CircleCollider2D physicsCollider;
  [SerializeField]
  public float teleportMinDistanceToPlayer;
  [SerializeField]
  public float lungeSpeed;
  [SerializeField]
  public float daggerOverrideMinRadius;
  [SerializeField]
  public float daggerOverrideMaxRadius;
  [SerializeField]
  public float daggerOverrideIntervals;
  [SerializeField]
  public ColliderEvents daggerCollider;
  [Space]
  [SerializeField]
  public float swordAttackWithinRadius;
  [SerializeField]
  public float swordChaseSpeed;
  [SerializeField]
  public float swordMaxChaseDuration;
  [SerializeField]
  public float swordRetargetPlayerInterval;
  [SerializeField]
  public ColliderEvents swordCollider;
  [SerializeField]
  public float projectilePatternCircleAnticipation;
  [SerializeField]
  public float projectilePatternCircleDuration;
  [SerializeField]
  public ProjectilePattern projectilePatternCircle;
  [Space]
  [SerializeField]
  public float projectilePatternSnakeAnticipation;
  [SerializeField]
  public float projectilePatternSnakeDuration;
  [SerializeField]
  public ProjectilePatternBeam projectilePatternSnake;
  [Space]
  [SerializeField]
  public float projectilePatternScatterAnticipation;
  [SerializeField]
  public float projectilePatternScatterDuration;
  [SerializeField]
  public ProjectilePattern projectilePatternScatter;
  [Space]
  [SerializeField]
  public float projectilePatternRingsAnticipation;
  [SerializeField]
  public float projectilePatternRingsDuration;
  [SerializeField]
  public float projectilePatternRingsSpeed;
  [SerializeField]
  public float projectilePatternRingsAcceleration;
  [SerializeField]
  public float projectilePatternRingsRadius;
  [SerializeField]
  public ProjectileCircleBase projectilePatternRings;
  [Space]
  [SerializeField]
  public float projectilePatternRandomAnticipation;
  [SerializeField]
  public float projectilePatternRandomDuration;
  [SerializeField]
  public ProjectilePattern projectilePatternRandom;
  [Space]
  [SerializeField]
  public float projectilePatternBeamAnticipation;
  [SerializeField]
  public float projectilePatternBeamDuration;
  [SerializeField]
  public ProjectilePatternBeam projectilePatternBeam;
  [SerializeField]
  public float hhgAnticipation;
  [SerializeField]
  public float hhgMinDistance;
  [SerializeField]
  public Vector2 hhgSpawnAmount;
  [SerializeField]
  public Vector2 hhgSpawnOffset;
  [SerializeField]
  public Vector2 hhgSpawnDelay;
  [SerializeField]
  public AssetReferenceGameObject[] hhgEnemiesList;
  [SerializeField]
  public float ssAnticipation;
  [SerializeField]
  public float ssDistanceBetween;
  [SerializeField]
  public float ssDelayBetween;
  [SerializeField]
  public float ssForce;
  [SerializeField]
  public AnimationCurve ssArc;
  [SerializeField]
  public Vector2 ssSpawnAmount;
  [SerializeField]
  public AssetReferenceGameObject[] ssEnemiesList;
  [SerializeField]
  public float rsAnticipation;
  [SerializeField]
  public Vector2 rsSpawnAmount;
  [SerializeField]
  public AssetReferenceGameObject[] rsEnemiesList;
  [SerializeField]
  public float enragedDuration = 2f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float p2HealthThreshold = 0.6f;
  [Space]
  [SerializeField]
  public Interaction_MonsterHeart interaction_MonsterHeart;
  [SerializeField]
  public GameObject blockingCollider;
  [SerializeField]
  public GameObject followerToSpawn;
  public List<GameObject> spawnedEnemies = new List<GameObject>();
  public Health currentTarget;
  public Coroutine currentAttackRoutine;
  public EventInstance grenadeLoopingSoundInstance;
  public bool facePlayer = true;
  public bool isDead;
  public bool queuePhaseIncrement;
  public bool activated;
  public bool attacking;
  public bool m = true;
  public int currentPhaseNumber;
  public float startingHealth;
  public float repathTimestamp;
  public bool recentlyTeleported;
  public bool juicedForm;
  public List<Collider2D> projectileTargets = new List<Collider2D>();
  public List<AsyncOperationHandle<GameObject>> loadedhhgEnemies = new List<AsyncOperationHandle<GameObject>>();
  public List<AsyncOperationHandle<GameObject>> loadedssEnemies = new List<AsyncOperationHandle<GameObject>>();
  public List<AsyncOperationHandle<GameObject>> loadedrsEnemies = new List<AsyncOperationHandle<GameObject>>();
  public bool isOptimizationsInitialized;
  public Vector2 lowestRoomPoint;
  public Vector2 highestRoomPoint;

  public bool moving
  {
    get => this.m;
    set
    {
      this.m = value;
      if (this.m)
        return;
      this.ClearPaths();
    }
  }

  public override void Awake()
  {
    base.Awake();
    this.daggerCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.CombatAttack_OnTriggerEnterEvent);
    this.swordCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.CombatAttack_OnTriggerEnterEvent);
    this.juicedForm = GameManager.Layer2;
    if (this.juicedForm)
    {
      this.health.totalHP *= 1.5f;
      this.health.HP = this.health.totalHP;
      this.rsSpawnAmount *= 1.25f;
      for (int index = 0; index < this.projectilePatternCircle.Waves.Length; ++index)
      {
        this.projectilePatternCircle.Waves[index].Speed *= 1.5f;
        this.projectilePatternCircle.Waves[index].FinishDelay /= 1.5f;
      }
      for (int index = 0; index < this.projectilePatternSnake.BulletWaves.Length; ++index)
      {
        this.projectilePatternSnake.BulletWaves[index].Speed *= 1.5f;
        this.projectilePatternSnake.BulletWaves[index].DelayBetweenBullets /= 1.5f;
      }
      for (int index = 0; index < this.projectilePatternScatter.Waves.Length; ++index)
      {
        this.projectilePatternScatter.Waves[index].Speed *= 1.5f;
        this.projectilePatternScatter.Waves[index].FinishDelay /= 1.5f;
      }
      for (int index = 0; index < this.projectilePatternRandom.Waves.Length; ++index)
      {
        this.projectilePatternRandom.Waves[index].Speed *= 1.5f;
        this.projectilePatternRandom.Waves[index].FinishDelay /= 1.5f;
      }
      for (int index = 0; index < this.projectilePatternBeam.BulletWaves.Length; ++index)
      {
        this.projectilePatternBeam.BulletWaves[index].Speed *= 1.5f;
        this.projectilePatternBeam.BulletWaves[index].DelayBetweenBullets /= 1.5f;
      }
      this.projectilePatternRingsSpeed *= 1.5f;
    }
    this.InitializeProjectilePatternRings();
  }

  public void Preload()
  {
    Debug.Log((object) "Enemy Tentacle Monster::Preload");
    for (int index = 0; index < this.hhgEnemiesList.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.hhgEnemiesList[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedhhgEnemies.Add(obj);
        obj.Result.CreatePool(Mathf.CeilToInt(this.hhgSpawnAmount.y) * 4, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
    Debug.Log((object) "Enemy Tentacle Monster::Preload 1");
    for (int index = 0; index < this.ssEnemiesList.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.ssEnemiesList[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedssEnemies.Add(obj);
        obj.Result.CreatePool(Mathf.CeilToInt(this.ssSpawnAmount.y) * 4, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
    Debug.Log((object) "Enemy Tentacle Monster::Preload 2");
    for (int index = 0; index < this.rsEnemiesList.Length; ++index)
    {
      Debug.Log((object) ("Enemy Tentacle Monster: Starting load of " + this.rsEnemiesList[index].SubObjectName));
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.rsEnemiesList[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        Debug.Log((object) ("Enemy Tentacle Monster: Finished Loading " + obj.Result.name));
        this.loadedrsEnemies.Add(obj);
        obj.Result.CreatePool(Mathf.CeilToInt(this.rsSpawnAmount.y) * 4, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
  }

  public IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyTentacleMonster enemyTentacleMonster = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Debug.Log((object) "Enemy Tentacle Monster::Start");
    enemyTentacleMonster.startingHealth = enemyTentacleMonster.health.HP;
    enemyTentacleMonster.health.SlowMoOnkill = false;
    ProjectilePatternBase.OnProjectileSpawned += new ProjectilePatternBase.ProjectileEvent(enemyTentacleMonster.OnProjectileSpawned);
    UnityEngine.Physics.autoSimulation = false;
    SimulationManager.Pause();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    AudioManager.Instance.StopLoop(this.grenadeLoopingSoundInstance);
    ProjectilePatternBase.OnProjectileSpawned -= new ProjectilePatternBase.ProjectileEvent(this.OnProjectileSpawned);
    if (this.loadedhhgEnemies != null)
    {
      foreach (AsyncOperationHandle<GameObject> loadedhhgEnemy in this.loadedhhgEnemies)
        Addressables.Release((AsyncOperationHandle) loadedhhgEnemy);
      this.loadedhhgEnemies.Clear();
    }
    if (this.loadedssEnemies != null)
    {
      foreach (AsyncOperationHandle<GameObject> loadedssEnemy in this.loadedssEnemies)
        Addressables.Release((AsyncOperationHandle) loadedssEnemy);
      this.loadedssEnemies.Clear();
    }
    if (this.loadedrsEnemies == null)
      return;
    foreach (AsyncOperationHandle<GameObject> loadedrsEnemy in this.loadedrsEnemies)
      Addressables.Release((AsyncOperationHandle) loadedrsEnemy);
    this.loadedrsEnemies.Clear();
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    if (this.queuePhaseIncrement && this.currentAttackRoutine == null)
    {
      this.queuePhaseIncrement = false;
      this.IncrementPhase();
    }
    if (this.facePlayer && this.activated && !this.isDead)
      this.LookAt(this.GetAngleToTarget());
    if (this.activated && !this.isDead && this.moving)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float repathTimestamp = this.repathTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) repathTimestamp & currentTime.HasValue)
      {
        if (this.attacking)
        {
          if ((double) Vector3.Distance(this.currentTarget.transform.position, this.transform.position) < 2.0)
            this.givePath(this.currentTarget.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 2f));
          else
            this.givePath(this.currentTarget.transform.position + Vector3.up);
        }
        else
          this.givePath(this.GetPositionAwayFromPlayer());
        this.repathTimestamp = GameManager.GetInstance().CurrentTime + this.swordRetargetPlayerInterval;
      }
    }
    if (this.isDead)
      return;
    if (this.activated && this.currentPhaseNumber == 1)
    {
      this.UpdatePhase1();
    }
    else
    {
      if (!this.activated || this.currentPhaseNumber < 2)
        return;
      this.UpdatePhase2();
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.activated)
      this.StartCoroutine((IEnumerator) this.DelayAddCamera());
    this.Preload();
    this.currentTarget = (Health) PlayerFarming.Instance.health;
  }

  public IEnumerator DelayAddCamera()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyTentacleMonster enemyTentacleMonster = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().AddToCamera(enemyTentacleMonster.gameObject);
      GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public bool CanSpawnEnemies() => this.spawnedEnemies.Count < 15;

  public void BeginPhase1()
  {
    this.InitializeOptimizations();
    GameManager.GetInstance().AddToCamera(this.gameObject);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    this.StartCoroutine((IEnumerator) this.DelayCallback(1f, (System.Action) (() =>
    {
      this.activated = true;
      this.currentPhaseNumber = 1;
      this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.RandomSpawnIE());
    })));
  }

  public IEnumerator DelayCallback(float delay, System.Action callback)
  {
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.spine.timeScale) < (double) delay)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void InitializeOptimizations()
  {
    if (this.isOptimizationsInitialized)
      return;
    this.projectileTargets.Clear();
    Collider2D[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<Collider2D>();
    PolygonCollider2D roomZoneCollider = (PolygonCollider2D) null;
    CompositeCollider2D compositeCollider2D = (CompositeCollider2D) null;
    for (int index = 0; index < objectsOfTypeAll.Length; ++index)
    {
      if ((UnityEngine.Object) roomZoneCollider == (UnityEngine.Object) null && objectsOfTypeAll[index].transform.name == "Collider" && objectsOfTypeAll[index].transform.parent.name == "Boss Room Island(Clone)")
        roomZoneCollider = objectsOfTypeAll[index] as PolygonCollider2D;
      if ((UnityEngine.Object) compositeCollider2D == (UnityEngine.Object) null && (UnityEngine.Object) (objectsOfTypeAll[index] as CompositeCollider2D) != (UnityEngine.Object) null)
        compositeCollider2D = objectsOfTypeAll[index] as CompositeCollider2D;
      if (objectsOfTypeAll[index].gameObject.activeInHierarchy && (objectsOfTypeAll[index].gameObject.layer == LayerMask.NameToLayer("Player") || objectsOfTypeAll[index].gameObject.layer == LayerMask.NameToLayer("Obstacles")))
        this.projectileTargets.Add(objectsOfTypeAll[index]);
    }
    List<Projectile> list = new List<Projectile>();
    ObjectPool.GetPooled<Projectile>(this.projectilePatternCircle.BulletPrefab, list, true);
    ObjectPool.GetPooled<Projectile>(this.projectilePatternSnake.BulletPrefab, list, true);
    ObjectPool.GetPooled<Projectile>(this.projectilePatternScatter.BulletPrefab, list, true);
    ObjectPool.GetPooled<Projectile>(this.projectilePatternRandom.BulletPrefab, list, true);
    ObjectPool.GetPooled<Projectile>(this.projectilePatternBeam.BulletPrefab, list, true);
    ProjectileCirclePattern projectilePatternRings = (ProjectileCirclePattern) this.projectilePatternRings;
    if ((UnityEngine.Object) projectilePatternRings.ProjectilePrefab != (UnityEngine.Object) null)
      ObjectPool.GetPooled<Projectile>(projectilePatternRings.ProjectilePrefab, list, true);
    this.InitializeRoomBounds(roomZoneCollider);
    for (int index = 0; index < list.Count; ++index)
    {
      list[index].TargetObjects = this.projectileTargets;
      list[index].CollideOnlyTargets = true;
      list[index].RoomBoundsCompositeCollider = compositeCollider2D;
      list[index].InitializeRoomBounds(this.lowestRoomPoint, this.highestRoomPoint);
    }
    this.isOptimizationsInitialized = true;
  }

  public void InitializeRoomBounds(PolygonCollider2D roomZoneCollider)
  {
    float x1 = float.MaxValue;
    float x2 = float.MinValue;
    float y1 = float.MaxValue;
    float y2 = float.MinValue;
    for (int index = 0; index < roomZoneCollider.points.Length; ++index)
    {
      Vector2 vector2 = (Vector2) roomZoneCollider.transform.TransformPoint((Vector3) roomZoneCollider.points[index]);
      if ((double) vector2.x > (double) x2)
        x2 = vector2.x;
      if ((double) vector2.x < (double) x1)
        x1 = vector2.x;
      if ((double) vector2.y > (double) y2)
        y2 = vector2.y;
      if ((double) vector2.y < (double) y1)
        y1 = vector2.y;
    }
    this.lowestRoomPoint = new Vector2(x1, y1);
    this.highestRoomPoint = new Vector2(x2, y2);
  }

  public void UpdatePhase1()
  {
    float num1 = Vector3.Distance(this.currentTarget.transform.position, this.transform.position);
    if (this.currentAttackRoutine != null)
      return;
    if (UnityEngine.Random.Range(0, 3) == 0 && !this.recentlyTeleported)
    {
      this.recentlyTeleported = true;
      if ((double) num1 < (double) this.daggerOverrideMinRadius)
      {
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.DaggerAttackIE());
        return;
      }
      if ((double) num1 < (double) this.daggerOverrideMinRadius * 2.0)
      {
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.TeleportAwayFromPlayerIE(0.25f));
        return;
      }
    }
    this.recentlyTeleported = false;
    int num2 = UnityEngine.Random.Range(0, 5);
    if (num2 < 2 && this.CanSpawnEnemies())
    {
      if (UnityEngine.Random.Range(0, 2) == 0 && this.spawnedEnemies.Count < 3)
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.RandomSpawnIE());
      else
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.SweepingSpawnIE());
    }
    else if (num2 == 2 && (double) num1 > 4.0)
    {
      this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.SwordAttackIE());
    }
    else
    {
      switch (UnityEngine.Random.Range(0, 4))
      {
        case 0:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileBeamIE());
          break;
        case 1:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRandomIE());
          break;
        default:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsIE());
          break;
      }
    }
  }

  public void BeginPhase2()
  {
    this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.Phase2IE());
  }

  public IEnumerator Phase2IE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.EnragedIE());
    enemyTentacleMonster.maxSpeed *= 2f;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.ShootProjectileCircleIE());
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void UpdatePhase2()
  {
    float num1 = Vector3.Distance(this.currentTarget.transform.position, this.transform.position);
    if (this.currentAttackRoutine != null)
      return;
    if (UnityEngine.Random.Range(0, 3) == 0 && !this.recentlyTeleported)
    {
      this.recentlyTeleported = true;
      if ((double) num1 < (double) this.daggerOverrideMinRadius)
      {
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.DaggerAttackIE());
        return;
      }
      if ((double) num1 < (double) this.daggerOverrideMinRadius * 2.0)
      {
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.TeleportAwayFromPlayerIE(0.25f));
        return;
      }
    }
    this.recentlyTeleported = false;
    int num2 = UnityEngine.Random.Range(0, 5);
    if (num2 < 2 && this.CanSpawnEnemies())
    {
      int num3 = UnityEngine.Random.Range(0, 3);
      if (num3 == 0 && this.spawnedEnemies.Count < 5)
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.RandomSpawnIE());
      else if (num3 == 1)
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.HolyHandGrenadeIE());
      else
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.SweepingSpawnIE());
    }
    else if (num2 == 2)
    {
      if ((double) num1 > 4.0)
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.SwordAttackIE());
      else
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.DaggerRapidAttackIE());
    }
    else
    {
      switch (UnityEngine.Random.Range(0, 3))
      {
        case 0:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileCircleIE());
          break;
        case 1:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRandomIE());
          break;
        default:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsTripleIE());
          break;
      }
    }
  }

  public void IncrementPhase()
  {
    if (this.currentAttackRoutine != null)
    {
      this.queuePhaseIncrement = true;
    }
    else
    {
      if (this.currentAttackRoutine != null)
        this.StopCoroutine(this.currentAttackRoutine);
      ++this.currentPhaseNumber;
      if (this.currentPhaseNumber != 2)
        return;
      this.BeginPhase2();
    }
  }

  public void HolyHandGrenade() => this.StartCoroutine((IEnumerator) this.HolyHandGrenadeIE());

  public IEnumerator HolyHandGrenadeIE()
  {
    EnemyTentacleMonster enemyTentacleMonster1 = this;
    enemyTentacleMonster1.spine.AnimationState.SetAnimation(0, enemyTentacleMonster1.holyHandGrenadeAnticipationAnimation, false);
    enemyTentacleMonster1.spine.AnimationState.AddAnimation(0, enemyTentacleMonster1.holyHandGrenadeAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_start", enemyTentacleMonster1.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster1.spine.timeScale) < (double) enemyTentacleMonster1.hhgAnticipation)
      yield return (object) null;
    AudioManager.Instance.StopLoop(enemyTentacleMonster1.grenadeLoopingSoundInstance);
    enemyTentacleMonster1.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster1.gameObject, true);
    int spawnAmount = (int) UnityEngine.Random.Range(enemyTentacleMonster1.hhgSpawnAmount.x, enemyTentacleMonster1.hhgSpawnAmount.y + 1f);
    for (int i = 0; i < spawnAmount; ++i)
    {
      EnemyTentacleMonster enemyTentacleMonster = enemyTentacleMonster1;
      Vector3 position = enemyTentacleMonster1.GetPosition(enemyTentacleMonster1.currentTarget.transform.position, enemyTentacleMonster1.currentTarget.state.LookAngle + UnityEngine.Random.Range(enemyTentacleMonster1.hhgSpawnOffset.x, enemyTentacleMonster1.hhgSpawnOffset.y), enemyTentacleMonster1.hhgMinDistance);
      GameObject spawnedEnemy = ObjectPool.Spawn(enemyTentacleMonster1.loadedhhgEnemies[UnityEngine.Random.Range(0, enemyTentacleMonster1.loadedhhgEnemies.Count)].Result, enemyTentacleMonster1.transform.parent.parent, position, Quaternion.identity);
      EnemyExploder component1 = spawnedEnemy.GetComponent<EnemyExploder>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.InitializeOptimization(enemyTentacleMonster1.lowestRoomPoint, enemyTentacleMonster1.highestRoomPoint);
      UnitObject component2 = spawnedEnemy.GetComponent<UnitObject>();
      component2.CanHaveModifier = false;
      enemyTentacleMonster1.spawnedEnemies.Add(spawnedEnemy);
      component2.gameObject.SetActive(false);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(component2.transform.position, enemyTentacleMonster1.transform.parent, component2.gameObject);
      component2.GetComponent<Health>().OnDie += (Health.DieAction) ((Attacker, AttackLocation, Victim, AttackType, AttackFlags) => enemyTentacleMonster.spawnedEnemies.Remove(spawnedEnemy));
      float dur = UnityEngine.Random.Range(enemyTentacleMonster1.hhgSpawnDelay.x, enemyTentacleMonster1.hhgSpawnDelay.y);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyTentacleMonster1.spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    enemyTentacleMonster1.spine.AnimationState.SetAnimation(0, enemyTentacleMonster1.idleAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster1.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster1.gameObject);
    float dura = UnityEngine.Random.Range(0.5f, 1.25f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster1.spine.timeScale) < (double) dura)
      yield return (object) null;
    enemyTentacleMonster1.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster1.currentTarget = enemyTentacleMonster1.ReconsiderPlayerTarget();
  }

  public void SweepingSpawn() => this.StartCoroutine((IEnumerator) this.SweepingSpawnIE());

  public IEnumerator SweepingSpawnIE()
  {
    EnemyTentacleMonster enemyTentacleMonster1 = this;
    enemyTentacleMonster1.spine.AnimationState.SetAnimation(0, enemyTentacleMonster1.sweepingSpawnAnimation, false);
    enemyTentacleMonster1.spine.AnimationState.AddAnimation(0, enemyTentacleMonster1.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/staff", enemyTentacleMonster1.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster1.spine.timeScale) < (double) enemyTentacleMonster1.ssAnticipation)
      yield return (object) null;
    int spawnAmount = (int) UnityEngine.Random.Range(enemyTentacleMonster1.ssSpawnAmount.x, enemyTentacleMonster1.ssSpawnAmount.y + 1f);
    Mathf.RoundToInt((float) (spawnAmount / 2));
    float angle = Utils.GetAngle(enemyTentacleMonster1.transform.position, enemyTentacleMonster1.currentTarget.transform.position) * ((float) Math.PI / 180f);
    for (int i = 0; i < spawnAmount; ++i)
    {
      EnemyTentacleMonster enemyTentacleMonster = enemyTentacleMonster1;
      float time1 = (float) i / (float) spawnAmount;
      GameObject spawnedEnemy = ObjectPool.Spawn(enemyTentacleMonster1.loadedssEnemies[UnityEngine.Random.Range(0, enemyTentacleMonster1.ssEnemiesList.Length)].Result, enemyTentacleMonster1.transform.parent.parent, enemyTentacleMonster1.transform.position, Quaternion.identity);
      EnemyExploder component1 = spawnedEnemy.GetComponent<EnemyExploder>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.InitializeOptimization(enemyTentacleMonster1.lowestRoomPoint, enemyTentacleMonster1.highestRoomPoint);
      UnitObject component2 = spawnedEnemy.GetComponent<UnitObject>();
      component2.CanHaveModifier = false;
      enemyTentacleMonster1.spawnedEnemies.Add(spawnedEnemy);
      component2.GetComponent<Health>().OnDie += (Health.DieAction) ((Attacker, AttackLocation, Victim, AttackType, AttackFlags) => enemyTentacleMonster.spawnedEnemies.Remove(spawnedEnemy));
      DropLootOnDeath component3 = component2.GetComponent<DropLootOnDeath>();
      if ((bool) (UnityEngine.Object) component3)
        component3.GiveXP = false;
      component2.GetComponent<UnitObject>().DoKnockBack(angle, enemyTentacleMonster1.ssForce * enemyTentacleMonster1.ssArc.Evaluate(time1), 1f);
      if (component2 is EnemyJellyCharger)
      {
        ((EnemyJellyCharger) component2).AllowMultipleChargers = true;
        component2.VisionRange = int.MaxValue;
      }
      angle += enemyTentacleMonster1.ssDistanceBetween;
      if ((double) enemyTentacleMonster1.ssDelayBetween != 0.0)
      {
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyTentacleMonster1.spine.timeScale) < (double) enemyTentacleMonster1.ssDelayBetween)
          yield return (object) null;
      }
    }
    float dur = UnityEngine.Random.Range(0.5f, 1.25f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster1.spine.timeScale) < (double) dur)
      yield return (object) null;
    enemyTentacleMonster1.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster1.currentTarget = enemyTentacleMonster1.ReconsiderPlayerTarget();
  }

  public void RandomSpawn() => this.StartCoroutine((IEnumerator) this.RandomSpawnIE());

  public IEnumerator RandomSpawnIE()
  {
    EnemyTentacleMonster enemyTentacleMonster1 = this;
    enemyTentacleMonster1.moving = false;
    enemyTentacleMonster1.spine.AnimationState.SetAnimation(0, enemyTentacleMonster1.randomSpawnAnimation, false);
    enemyTentacleMonster1.spine.AnimationState.AddAnimation(0, enemyTentacleMonster1.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/staff", enemyTentacleMonster1.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster1.spine.timeScale) < (double) enemyTentacleMonster1.rsAnticipation)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/staff_magic", enemyTentacleMonster1.gameObject);
    int num1 = (int) UnityEngine.Random.Range(enemyTentacleMonster1.rsSpawnAmount.x, enemyTentacleMonster1.rsSpawnAmount.y + 1f);
    for (int index = 0; index < num1; ++index)
    {
      EnemyTentacleMonster enemyTentacleMonster = enemyTentacleMonster1;
      List<AsyncOperationHandle<GameObject>> loadedrsEnemies1 = enemyTentacleMonster1.loadedrsEnemies;
      // ISSUE: explicit non-virtual call
      int num2 = UnityEngine.Random.Range(0, loadedrsEnemies1 != null ? __nonvirtual (loadedrsEnemies1.Count) : 0);
      List<AsyncOperationHandle<GameObject>> loadedrsEnemies2 = enemyTentacleMonster1.loadedrsEnemies;
      // ISSUE: explicit non-virtual call
      Debug.Log((object) $"loadedrsEnemies Count: {(loadedrsEnemies2 != null ? __nonvirtual (loadedrsEnemies2.Count) : -1)}, chosenIndex: {num2}");
      GameObject spawnedEnemy = ObjectPool.Spawn(enemyTentacleMonster1.loadedrsEnemies[UnityEngine.Random.Range(0, enemyTentacleMonster1.loadedrsEnemies.Count)].Result, enemyTentacleMonster1.transform.parent.parent, enemyTentacleMonster1.GetRandomPosition(Vector3.zero, 0.0f, 3f, 8f), Quaternion.identity);
      EnemyPatroller component1 = spawnedEnemy.GetComponent<EnemyPatroller>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.InitializeOptimization(enemyTentacleMonster1.lowestRoomPoint, enemyTentacleMonster1.highestRoomPoint);
      UnitObject component2 = spawnedEnemy.GetComponent<UnitObject>();
      component2.CanHaveModifier = false;
      enemyTentacleMonster1.spawnedEnemies.Add(spawnedEnemy);
      component2.gameObject.SetActive(false);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(component2.transform.position, enemyTentacleMonster1.transform.parent.parent, component2.gameObject);
      DropLootOnDeath component3 = component2.GetComponent<DropLootOnDeath>();
      if ((bool) (UnityEngine.Object) component3)
        component3.GiveXP = false;
      component2.GetComponent<Health>().OnDie += (Health.DieAction) ((Attacker, AttackLocation, Victim, AttackType, AttackFlags) => enemyTentacleMonster.spawnedEnemies.Remove(spawnedEnemy));
    }
    float dur = UnityEngine.Random.Range(0.5f, 1.25f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster1.spine.timeScale) < (double) dur)
      yield return (object) null;
    enemyTentacleMonster1.moving = true;
    enemyTentacleMonster1.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster1.currentTarget = enemyTentacleMonster1.ReconsiderPlayerTarget();
  }

  public void OnProjectileSpawned()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_spawn", this.gameObject);
  }

  public void ShootProjectileCircle()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileCircleIE());
  }

  public IEnumerator ShootProjectileCircleIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, enemyTentacleMonster.currentTarget.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.moving = false;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternCircleAnticipation)
      yield return (object) null;
    enemyTentacleMonster.projectilePatternCircle.Shoot();
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternCircleDuration)
      yield return (object) null;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    float dur = UnityEngine.Random.Range(0.5f, 1.25f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) dur)
      yield return (object) null;
    enemyTentacleMonster.moving = true;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster.currentTarget = enemyTentacleMonster.ReconsiderPlayerTarget();
  }

  public void ShootProjectileSnake()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileSnakeIE());
  }

  public IEnumerator ShootProjectileSnakeIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, enemyTentacleMonster.currentTarget.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternSnakeAnticipation)
      yield return (object) null;
    enemyTentacleMonster.projectilePatternSnake.Shoot(enemyTentacleMonster.currentTarget.gameObject);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternSnakeDuration)
      yield return (object) null;
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void ShootProjectileRandom()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileRandomIE());
  }

  public IEnumerator ShootProjectileRandomIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, enemyTentacleMonster.currentTarget.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternRandomAnticipation)
      yield return (object) null;
    enemyTentacleMonster.projectilePatternRandom.Shoot();
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternRandomDuration)
      yield return (object) null;
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    float dur = UnityEngine.Random.Range(0.5f, 1.25f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) dur)
      yield return (object) null;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster.currentTarget = enemyTentacleMonster.ReconsiderPlayerTarget();
  }

  public void ShootProjectileScatter()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileScatterIE());
  }

  public IEnumerator ShootProjectileScatterIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, enemyTentacleMonster.currentTarget.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternScatterAnticipation)
      yield return (object) null;
    enemyTentacleMonster.projectilePatternScatter.Shoot();
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternScatterDuration)
      yield return (object) null;
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void ShootProjectileBeam()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileBeamIE());
  }

  public IEnumerator ShootProjectileBeamIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, enemyTentacleMonster.currentTarget.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternBeamAnticipation)
      yield return (object) null;
    enemyTentacleMonster.projectilePatternBeam.Shoot(enemyTentacleMonster.currentTarget.gameObject);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternBeamDuration)
      yield return (object) null;
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    float dur = UnityEngine.Random.Range(0.5f, 1.25f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) dur)
      yield return (object) null;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster.currentTarget = enemyTentacleMonster.ReconsiderPlayerTarget();
  }

  public void ShootProjectileRings()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileRingsIE());
  }

  public void InitializeProjectilePatternRings()
  {
    if (this.projectilePatternRings is ProjectileCirclePattern)
    {
      ProjectileCirclePattern projectilePatternRings = (ProjectileCirclePattern) this.projectilePatternRings;
      if ((UnityEngine.Object) projectilePatternRings.ProjectilePrefab != (UnityEngine.Object) null)
        ObjectPool.CreatePool<Projectile>(projectilePatternRings.ProjectilePrefab, projectilePatternRings.BaseProjectilesCount * 3);
    }
    ObjectPool.CreatePool<ProjectileCircleBase>(this.projectilePatternRings, 3);
  }

  public IEnumerator ShootProjectileRingsIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, enemyTentacleMonster.currentTarget.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.moving = false;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternRingsAnticipation)
      yield return (object) null;
    Projectile component = ObjectPool.Spawn<ProjectileCircleBase>(enemyTentacleMonster.projectilePatternRings, enemyTentacleMonster.transform.parent).GetComponent<Projectile>();
    component.transform.position = enemyTentacleMonster.transform.position + (Vector3) (Utils.DegreeToVector2(enemyTentacleMonster.GetAngleToPlayer()) * enemyTentacleMonster.projectilePatternRingsRadius * 2f);
    component.Angle = enemyTentacleMonster.GetAngleToTarget();
    component.health = enemyTentacleMonster.health;
    component.team = Health.Team.Team2;
    component.Speed = enemyTentacleMonster.projectilePatternRingsSpeed;
    component.Acceleration = enemyTentacleMonster.projectilePatternRingsAcceleration;
    component.GetComponent<ProjectileCircleBase>().InitDelayed(enemyTentacleMonster.currentTarget.gameObject, enemyTentacleMonster.projectilePatternRingsRadius, 0.0f, new System.Action(enemyTentacleMonster.\u003CShootProjectileRingsIE\u003Eb__128_0));
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternRingsDuration)
      yield return (object) null;
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    float dur = UnityEngine.Random.Range(0.5f, 1.25f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) dur)
      yield return (object) null;
    enemyTentacleMonster.moving = true;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster.currentTarget = enemyTentacleMonster.ReconsiderPlayerTarget();
  }

  public void ShootProjectileRingsTriple()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileRingsTripleIE());
  }

  public IEnumerator ShootProjectileRingsTripleIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, enemyTentacleMonster.currentTarget.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.moving = false;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternRingsAnticipation)
      yield return (object) null;
    List<float> floatList = new List<float>()
    {
      0.0f,
      0.5f,
      1f
    };
    for (int index = 0; index < 3; ++index)
    {
      float t = (float) index / 2f;
      float angleToPlayer = enemyTentacleMonster.GetAngleToPlayer();
      Vector3 vector2 = (Vector3) Utils.DegreeToVector2(angleToPlayer);
      Vector3 vector3 = Vector3.Lerp((Vector3) Utils.DegreeToVector2(angleToPlayer - 90f), (Vector3) Utils.DegreeToVector2(angleToPlayer + 90f), t) * 1.25f;
      Projectile component = ObjectPool.Spawn<ProjectileCircleBase>(enemyTentacleMonster.projectilePatternRings, enemyTentacleMonster.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyTentacleMonster.transform.position + (vector2 + vector3) * enemyTentacleMonster.projectilePatternRingsRadius * 2f;
      component.Angle = angleToPlayer;
      component.health = enemyTentacleMonster.health;
      component.team = Health.Team.Team2;
      component.Speed = enemyTentacleMonster.projectilePatternRingsSpeed;
      component.Acceleration = enemyTentacleMonster.projectilePatternRingsAcceleration;
      float shootDelay = floatList[UnityEngine.Random.Range(0, floatList.Count)];
      floatList.Remove(shootDelay);
      component.GetComponent<ProjectileCircleBase>().InitDelayed(enemyTentacleMonster.currentTarget.gameObject, enemyTentacleMonster.projectilePatternRingsRadius, shootDelay, new System.Action(enemyTentacleMonster.\u003CShootProjectileRingsTripleIE\u003Eb__130_0));
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) enemyTentacleMonster.projectilePatternRingsDuration)
      yield return (object) null;
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    float dur = UnityEngine.Random.Range(0.5f, 1.25f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) dur)
      yield return (object) null;
    enemyTentacleMonster.moving = true;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster.currentTarget = enemyTentacleMonster.ReconsiderPlayerTarget();
  }

  public void DaggerAttack() => this.StartCoroutine((IEnumerator) this.DaggerAttackIE());

  public IEnumerator DaggerAttackIE(bool finishAttack = true)
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.attacking = true;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.daggerAttackAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/dagger_attack", enemyTentacleMonster.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 0.40000000596046448)
      yield return (object) null;
    float ogMaxSpeed = enemyTentacleMonster.maxSpeed;
    enemyTentacleMonster.maxSpeed = enemyTentacleMonster.lungeSpeed;
    enemyTentacleMonster.speed = enemyTentacleMonster.lungeSpeed;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 0.20000000298023224)
      yield return (object) null;
    enemyTentacleMonster.maxSpeed = ogMaxSpeed;
    enemyTentacleMonster.speed = enemyTentacleMonster.maxSpeed / 2f;
    enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.EnableCollider(enemyTentacleMonster.daggerCollider.gameObject, 0.25f));
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 1.5)
      yield return (object) null;
    if (finishAttack)
      enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster.currentTarget = enemyTentacleMonster.ReconsiderPlayerTarget();
    enemyTentacleMonster.attacking = false;
  }

  public IEnumerator DaggerRapidAttackIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    for (int i = 0; i < UnityEngine.Random.Range(2, 5); ++i)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.DaggerAttackIE(false));
    float dur = UnityEngine.Random.Range(0.5f, 1f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) dur)
      yield return (object) null;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void SwordAttack() => this.StartCoroutine((IEnumerator) this.SwordAttackIE());

  public IEnumerator SwordAttackIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.attacking = true;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.swordAnticipationAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.swordLoopAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/sword_charge", enemyTentacleMonster.gameObject);
    float timeStamp = GameManager.GetInstance().CurrentTime + enemyTentacleMonster.swordMaxChaseDuration;
    enemyTentacleMonster.repathTimestamp = 0.0f;
    float ogMaxSpeed = enemyTentacleMonster.maxSpeed;
    enemyTentacleMonster.DisableForces = false;
    TweenerCore<float, float, FloatOptions> tween = DOTween.To(new DOGetter<float>(enemyTentacleMonster.\u003CSwordAttackIE\u003Eb__135_0), new DOSetter<float>(enemyTentacleMonster.\u003CSwordAttackIE\u003Eb__135_1), enemyTentacleMonster.swordChaseSpeed, 2f);
    float t = 0.0f;
    while ((double) Vector3.Distance(enemyTentacleMonster.currentTarget.transform.position, enemyTentacleMonster.transform.position) > (double) enemyTentacleMonster.swordAttackWithinRadius && (double) GameManager.GetInstance().CurrentTime < (double) timeStamp || (double) t < 1.0)
    {
      t += Time.deltaTime * enemyTentacleMonster.spine.timeScale;
      yield return (object) null;
    }
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.swordAttackAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/sword_attack", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.speed = enemyTentacleMonster.lungeSpeed;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.EnableCollider(enemyTentacleMonster.swordCollider.gameObject, 0.15f));
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    tween.Kill();
    enemyTentacleMonster.maxSpeed = ogMaxSpeed;
    enemyTentacleMonster.speed = enemyTentacleMonster.maxSpeed / 2f;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 1.0)
      yield return (object) null;
    enemyTentacleMonster.attacking = false;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster.currentTarget = enemyTentacleMonster.ReconsiderPlayerTarget();
  }

  public IEnumerator EnableCollider(GameObject collider, float duration)
  {
    collider.SetActive(true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.spine.timeScale) < (double) duration)
      yield return (object) null;
    collider.SetActive(false);
  }

  public void CombatAttack_OnTriggerEnterEvent(Collider2D collider)
  {
    if (!collider.CompareTag("Player"))
      return;
    PlayerFarming component = collider.GetComponent<PlayerFarming>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.health.DealDamage(1f, this.gameObject, this.transform.position, false, Health.AttackTypes.Melee, false, (Health.AttackFlags) 0);
  }

  public void TeleportAwayFromPlayer()
  {
    this.StartCoroutine((IEnumerator) this.TeleportAwayFromPlayerIE(0.5f));
  }

  public void TeleportNearPlayer()
  {
    this.StartCoroutine((IEnumerator) this.TeleportNearPlayerIE(0.0f));
  }

  public void TeleportToPostion(Vector3 position)
  {
    this.StartCoroutine((IEnumerator) this.TeleportToPositionIE(position));
  }

  public IEnumerator TeleportAwayFromPlayerIE(float endDelay = 1f, bool nullifyRoutine = true)
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.moving = false;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportToPositionIE(enemyTentacleMonster.GetPositionAwayFromPlayer(), endDelay));
    if (nullifyRoutine)
    {
      enemyTentacleMonster.moving = true;
      enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    }
  }

  public IEnumerator TeleportNearPlayerIE(float endDelay = 1f)
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.moving = false;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportOutIE());
    Vector3 position = enemyTentacleMonster.currentTarget.transform.position;
    float degree = (float) UnityEngine.Random.Range(0, 360);
    int num = 0;
    while (num++ < 32 /*0x20*/)
    {
      if ((bool) Physics2D.Raycast((Vector2) position, Utils.DegreeToVector2(degree), enemyTentacleMonster.teleportMinDistanceToPlayer, (int) enemyTentacleMonster.layerToCheck))
      {
        degree = Utils.Repeat(degree + 11.25f, 360f);
      }
      else
      {
        position += (Vector3) Utils.DegreeToVector2(degree) * enemyTentacleMonster.teleportMinDistanceToPlayer;
        break;
      }
    }
    enemyTentacleMonster.transform.position = position;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportInIE());
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) endDelay)
      yield return (object) null;
    enemyTentacleMonster.moving = true;
  }

  public IEnumerator TeleportToPositionIE(Vector3 position, float endDelay = 0.0f)
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportOutIE());
    enemyTentacleMonster.transform.position = position;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportInIE());
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < (double) endDelay)
      yield return (object) null;
    enemyTentacleMonster.currentTarget = enemyTentacleMonster.ReconsiderPlayerTarget();
  }

  public IEnumerator TeleportOutIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.teleportAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/teleport_away", enemyTentacleMonster.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 0.5)
      yield return (object) null;
    enemyTentacleMonster.physicsCollider.enabled = false;
  }

  public IEnumerator TeleportInIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/teleport_return", enemyTentacleMonster.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 0.5)
      yield return (object) null;
    enemyTentacleMonster.physicsCollider.enabled = true;
  }

  public IEnumerator EnragedIE()
  {
    this.spine.AnimationState.SetAnimation(0, "roar", false);
    this.spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.spine.timeScale) < 0.699999988079071)
      yield return (object) null;
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 1.3f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * this.spine.timeScale) < 2.4000000953674316)
      yield return (object) null;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.simpleSpineFlash.FlashFillRed(0.25f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/gethit");
    Vector3 Position = (AttackLocation + Attacker.transform.position) / 2f;
    BiomeConstants.Instance.EmitHitVFX(Position, Quaternion.identity.z, "HitFX_Weak");
    float num = this.health.HP / this.startingHealth;
    if (this.currentPhaseNumber != 1 || (double) num > (double) this.p2HealthThreshold)
      return;
    this.IncrementPhase();
  }

  public void LookAt(float angle)
  {
    this.state.LookAngle = angle;
    this.state.facingAngle = angle;
  }

  public float GetAngleToTarget()
  {
    float angleToPlayer = this.GetAngleToPlayer();
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return angleToPlayer;
    float num = 32f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(angleToPlayer * ((float) Math.PI / 180f)), Mathf.Sin(angleToPlayer * ((float) Math.PI / 180f))), 2.5f, (int) this.layerToCheck); ++index)
      angleToPlayer += (float) (360.0 / ((double) num + 1.0));
    return angleToPlayer;
  }

  public float GetAngleToPlayer()
  {
    return !((UnityEngine.Object) this.currentTarget != (UnityEngine.Object) null) ? 0.0f : Utils.GetAngle(this.transform.position, this.currentTarget.transform.position);
  }

  public Vector3 GetPositionAwayFromPlayer()
  {
    float x = this.currentTarget.transform.position.x;
    float y;
    for (y = this.currentTarget.transform.position.y; (double) Vector3.Distance(new Vector3(x, y), this.currentTarget.transform.position) < 4.0; y = UnityEngine.Random.Range(-5.5f, 5.5f))
      x = UnityEngine.Random.Range(-10f, 10f);
    return new Vector3(x, y, 0.0f);
  }

  public Vector3 GetRandomPosition(
    Vector3 startingPosition,
    float radius,
    float minDist,
    float maxDist)
  {
    float degree = (float) UnityEngine.Random.Range(0, 360);
    Vector3 origin = startingPosition;
    int num1 = 0;
    while (num1++ < 32 /*0x20*/)
    {
      float num2 = UnityEngine.Random.Range(minDist, maxDist);
      if ((bool) Physics2D.Raycast((Vector2) origin, Utils.DegreeToVector2(degree), num2 - radius, (int) this.layerToCheck))
      {
        degree = Utils.Repeat(degree + (float) UnityEngine.Random.Range(0, 360), 360f);
      }
      else
      {
        origin += (Vector3) Utils.DegreeToVector2(degree) * (num2 - radius);
        break;
      }
    }
    return origin;
  }

  public Vector3 GetPosition(Vector3 startingPosition, float angle, float distance)
  {
    float degree = angle;
    Vector3 origin = startingPosition;
    int num = 0;
    while (num++ < 32 /*0x20*/)
    {
      if ((bool) Physics2D.Raycast((Vector2) origin, Utils.DegreeToVector2(degree), distance, (int) this.layerToCheck))
      {
        degree = Utils.Repeat(degree + (float) UnityEngine.Random.Range(0, 360), 360f);
      }
      else
      {
        origin += (Vector3) Utils.DegreeToVector2(degree) * distance;
        break;
      }
    }
    return origin;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    AudioManager.Instance.StopLoop(this.grenadeLoopingSoundInstance);
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
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("KILL_BOSS_3"));
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.daggerCollider.gameObject.SetActive(false);
    this.swordCollider.gameObject.SetActive(false);
    this.KillAllSpawnedEnemies();
    GameManager.GetInstance().CamFollowTarget.MinZoom = 11f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 13f;
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/death");
    this.isDead = true;
    this.moving = false;
    UnityEngine.Physics.autoSimulation = true;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Die());
  }

  public IEnumerator Die()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.ClearPaths();
    enemyTentacleMonster.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: false);
    GameManager.GetInstance().OnConversationNext(enemyTentacleMonster.gameObject, 12f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/death", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.rb.velocity = (Vector2) Vector3.zero;
    enemyTentacleMonster.rb.isKinematic = true;
    enemyTentacleMonster.rb.simulated = false;
    enemyTentacleMonster.rb.bodyType = RigidbodyType2D.Static;
    if ((double) enemyTentacleMonster.transform.position.x > 11.0)
      enemyTentacleMonster.transform.position = new Vector3(11f, enemyTentacleMonster.transform.position.y, 0.0f);
    if ((double) enemyTentacleMonster.transform.position.x < -11.0)
      enemyTentacleMonster.transform.position = new Vector3(-11f, enemyTentacleMonster.transform.position.y, 0.0f);
    if ((double) enemyTentacleMonster.transform.position.y > 7.0)
      enemyTentacleMonster.transform.position = new Vector3(enemyTentacleMonster.transform.position.x, 7f, 0.0f);
    if ((double) enemyTentacleMonster.transform.position.y < -7.0)
      enemyTentacleMonster.transform.position = new Vector3(enemyTentacleMonster.transform.position.x, -7f, 0.0f);
    yield return (object) new WaitForEndOfFrame();
    float time = 0.0f;
    enemyTentacleMonster.simpleSpineFlash.StopAllCoroutines();
    enemyTentacleMonster.simpleSpineFlash.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
    enemyTentacleMonster.state.CURRENT_STATE = StateMachine.State.Dieing;
    bool beatenLayer2 = DataManager.Instance.BeatenKallamarLayer2;
    bool isDeathWithHeart = !DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location) && !DungeonSandboxManager.Active && !DataManager.Instance.SurvivalModeActive;
    if (isDeathWithHeart)
    {
      enemyTentacleMonster.spine.AnimationState.SetAnimation(0, "die", false);
      enemyTentacleMonster.spine.AnimationState.AddAnimation(0, "dead", true, 0.0f);
    }
    else
    {
      if (enemyTentacleMonster.juicedForm && !DataManager.Instance.BeatenKallamarLayer2 && !DungeonSandboxManager.Active)
      {
        enemyTentacleMonster.spine.AnimationState.SetAnimation(0, "die-follower", false);
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 3.8299999237060547)
          yield return (object) null;
        enemyTentacleMonster.spine.gameObject.SetActive(false);
        PlayerReturnToBase.Disabled = true;
        GameObject Follower = UnityEngine.Object.Instantiate<GameObject>(enemyTentacleMonster.followerToSpawn, enemyTentacleMonster.transform.position, Quaternion.identity, enemyTentacleMonster.transform.parent);
        Follower.GetComponent<Interaction_FollowerSpawn>().Play("CultLeader 3", ScriptLocalization.NAMES_CultLeaders.Dungeon3, cursedState: Thought.Ill);
        DataManager.SetFollowerSkinUnlocked("CultLeader 3");
        DataManager.Instance.BeatenKallamarLayer2 = true;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.NewGamePlus3);
        while ((UnityEngine.Object) Follower != (UnityEngine.Object) null)
          yield return (object) null;
        GameManager.GetInstance().OnConversationEnd();
        Interaction_Chest.Instance?.RevealBossReward(InventoryItem.ITEM_TYPE.GOD_TEAR);
        enemyTentacleMonster.KillAllSpawnedEnemies();
        enemyTentacleMonster.StopAllCoroutines();
        yield break;
      }
      enemyTentacleMonster.spine.AnimationState.SetAnimation(0, "die-noheart", false);
      enemyTentacleMonster.spine.AnimationState.AddAnimation(0, "dead-noheart", true, 0.0f);
      if (!DungeonSandboxManager.Active)
        RoomLockController.RoomCompleted();
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTentacleMonster.spine.timeScale) < 4.5)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    enemyTentacleMonster.blockingCollider.SetActive(false);
    enemyTentacleMonster.rb.isKinematic = true;
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3))
      enemyTentacleMonster.interaction_MonsterHeart.ObjectiveToComplete = Objectives.CustomQuestTypes.BishopsOfTheOldFaith3;
    enemyTentacleMonster.interaction_MonsterHeart.Play(beatenLayer2 || !GameManager.Layer2 ? InventoryItem.ITEM_TYPE.NONE : InventoryItem.ITEM_TYPE.GOD_TEAR);
    SimulationManager.UnPause();
    if (isDeathWithHeart)
      enemyTentacleMonster.spine.AnimationState.SetAnimation(0, "dead", false);
    else
      enemyTentacleMonster.spine.AnimationState.SetAnimation(0, "dead-noheart", true);
    enemyTentacleMonster.KillAllSpawnedEnemies();
    enemyTentacleMonster.StopAllCoroutines();
  }

  public void KillAllSpawnedEnemies()
  {
    for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null)
      {
        Health component = this.spawnedEnemies[index].GetComponent<Health>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.enabled = true;
          component.DealDamage(component.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
        }
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

  public override void OnDisable()
  {
    base.OnDisable();
    AudioManager.Instance.StopLoop(this.grenadeLoopingSoundInstance);
    this.currentAttackRoutine = (Coroutine) null;
    this.moving = true;
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__93_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedhhgEnemies.Add(obj);
    obj.Result.CreatePool(Mathf.CeilToInt(this.hhgSpawnAmount.y) * 4, true);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__93_1(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedssEnemies.Add(obj);
    obj.Result.CreatePool(Mathf.CeilToInt(this.ssSpawnAmount.y) * 4, true);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__93_2(AsyncOperationHandle<GameObject> obj)
  {
    Debug.Log((object) ("Enemy Tentacle Monster: Finished Loading " + obj.Result.name));
    this.loadedrsEnemies.Add(obj);
    obj.Result.CreatePool(Mathf.CeilToInt(this.rsSpawnAmount.y) * 4, true);
  }

  [CompilerGenerated]
  public void \u003CBeginPhase1\u003Eb__100_0()
  {
    this.activated = true;
    this.currentPhaseNumber = 1;
    this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.RandomSpawnIE());
  }

  [CompilerGenerated]
  public void \u003CShootProjectileRingsIE\u003Eb__128_0()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CShootProjectileRingsTripleIE\u003Eb__130_0()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.gameObject);
  }

  [CompilerGenerated]
  public float \u003CSwordAttackIE\u003Eb__135_0() => this.maxSpeed;

  [CompilerGenerated]
  public void \u003CSwordAttackIE\u003Eb__135_1(float x) => this.maxSpeed = x;
}
