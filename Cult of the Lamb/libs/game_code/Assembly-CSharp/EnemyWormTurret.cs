// Decompiled with JetBrains decompiler
// Type: EnemyWormTurret
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class EnemyWormTurret : UnitObject
{
  public SimpleSpineFlash SimpleSpineFlash;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AppearAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DisapearAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string HiddenAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ShootAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AnticipationShootAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AnticipationSpikeAnimation;
  [SerializeField]
  public GameObject lighting;
  public GameObject TrailPrefab;
  public List<EnemyBurrowingTrail> Trails = new List<EnemyBurrowingTrail>();
  public float DelayBetweenTrails = 0.2f;
  public float TrailsTimer;
  [SerializeField]
  public bool repositionOnHit = true;
  [SerializeField]
  public bool spawns;
  [SerializeField]
  public Vector2 spawnAmount;
  [SerializeField]
  public Vector2 spawnDelay;
  [SerializeField]
  public int maxActiveSpawns;
  [SerializeField]
  public UnitObject spawnable;
  [SerializeField]
  public bool canSpikeCircle;
  [SerializeField]
  public float spikeAnticipation;
  [SerializeField]
  public int circleSpikeAmount = 4;
  [SerializeField]
  public float circleDelayBetweenSpikes = 0.2f;
  [SerializeField]
  public float circleDistanceBetweenSpikes = 0.2f;
  [SerializeField]
  public int circleSpikeDistance = 10;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string ShootSFX = "event:/enemy/shoot_magicenergy";
  [EventRef]
  public string ShootSpikeSfx = "event:/enemy/shoot_arrowspike";
  public ShowHPBar ShowHPBar;
  public GameObject targetObject;
  public float spawnTimestamp = 5f;
  public float spawnRadius = 5f;
  public int spawnedAmount;
  public bool targeted;
  public bool active;
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
  public List<EnemyBurrowingTrail> spawnedSpikes = new List<EnemyBurrowingTrail>();
  public Coroutine currentRoutine;
  public float initialDelay;
  public GameObject t;
  public Vector3 previousSpawnPosition;
  public bool ShootingSpikeCircle;
  public bool Shoots = true;
  public GameObject Prefab;
  public float ShootDelay = 0.25f;
  public Vector2 DelayBetweenShots = new Vector2(0.1f, 0.3f);
  public float NumberOfShotsToFire = 5f;
  public float DistanceFromPlayerToFire = 5f;
  public float GravSpeed = -15f;
  public float AnticipationTime;
  public float Arc = 360f;
  public float shootCooldown = 0.5f;
  public Vector2 RandomArcOffset = new Vector2(0.0f, 0.0f);
  public Vector2 ShootDistanceRange = new Vector2(2f, 3f);
  public Vector3 ShootOffset;
  public bool BulletsTargetPlayer = true;
  public bool anticipating;
  public float anticipatingTimer;
  public float anticipationTime;
  public GameObject g;
  public GrenadeBullet GrenadeBullet;
  public float CacheShootDirectionCache;
  public float Angle;
  public Vector3 Force;
  public bool DiveAboveGround;
  public float ArcHeight = 5f;
  public EventInstance loopingSoundInstance;
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer = 0.8f;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public bool PathingToPlayer;
  public float RandomDirection;
  public float DistanceToPathTowardsPlayer = 6f;
  public float MinimumPlayerDistance = 3f;
  public float MoveSpeed = 5f;
  public float MoveDelay;
  public Vector2 MaxMoveDistance;
  public float IdleWait = 0.5f;
  public Vector3 TargetPosition;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public void Start()
  {
    this.initialDelay = UnityEngine.Random.Range(0.0f, 2f);
    this.CreatePool(this.circleSpikeAmount * this.circleSpikeDistance * 2);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.DisableForces = true;
    this.ShowHPBar = this.GetComponent<ShowHPBar>();
    this.StartCoroutine(this.CreateSpineEventListener());
    if (this.active)
    {
      if (this.currentRoutine != null)
        this.StopCoroutine(this.currentRoutine);
      this.currentRoutine = this.StartCoroutine(this.MoveRoutine());
    }
    this.active = true;
  }

  public IEnumerator CreateSpineEventListener()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWormTurret enemyWormTurret = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyWormTurret.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(enemyWormTurret.Shoot);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Shoot(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "shoot") || !this.Shoots)
      return;
    if (this.currentRoutine != null)
      this.StopCoroutine(this.currentRoutine);
    this.currentRoutine = this.StartCoroutine(this.ShootRoutine());
  }

  public void SpawnTrails()
  {
    if ((double) (this.TrailsTimer += Time.deltaTime) <= (double) this.DelayBetweenTrails || (double) Vector3.Distance(this.transform.position, this.previousSpawnPosition) <= 0.10000000149011612)
      return;
    this.TrailsTimer = 0.0f;
    GameObject gameObject = this.TrailPrefab.Spawn(this.transform.parent, this.transform.position, Quaternion.identity);
    EnemyBurrowingTrail component1 = gameObject.GetComponent<EnemyBurrowingTrail>();
    SimpleSpineDeactivateAfterPlay component2 = gameObject.GetComponent<SimpleSpineDeactivateAfterPlay>();
    if ((bool) (UnityEngine.Object) component2)
      component2.RecycleOnComplete = true;
    this.Trails.Add(component1);
    if ((bool) (UnityEngine.Object) component1)
      component1.OnDeactivate += new System.Action<EnemyBurrowingTrail>(this.OnDecativateTrail);
    if ((bool) (UnityEngine.Object) component1.ColliderEvents)
    {
      component1.ColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      component1.ColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.previousSpawnPosition = gameObject.transform.position;
  }

  public void OnDecativateTrail(EnemyBurrowingTrail trail)
  {
    trail.OnDeactivate -= new System.Action<EnemyBurrowingTrail>(this.OnDecativateTrail);
    if (this.Trails.Contains(trail))
      this.Trails.Remove(trail);
    if (this.spawnedSpikes.Contains(trail))
      this.spawnedSpikes.Remove(trail);
    if (!(bool) (UnityEngine.Object) trail.ColliderEvents)
      return;
    trail.ColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider?.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !this.health.IsCharmedEnemy)
      return;
    component.DealDamage(1f, this.gameObject, component.transform.position);
  }

  public override void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    this.ClearTrails(this.Trails);
    this.ClearTrails(this.spawnedSpikes);
    base.OnDestroy();
  }

  public void ClearTrails(List<EnemyBurrowingTrail> trails)
  {
    for (int index = trails.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) trails[index] != (UnityEngine.Object) null)
        this.OnDecativateTrail(trails[index]);
    }
  }

  public IEnumerator SpawnSpikesInDirectionsIE(
    int amount,
    float delayBetweenSpikes,
    float distanceBetweenSpikes)
  {
    EnemyWormTurret enemyWormTurret = this;
    float time = 0.0f;
    enemyWormTurret.ShootingSpikeCircle = true;
    enemyWormTurret.anticipating = true;
    enemyWormTurret.anticipatingTimer = 0.0f;
    enemyWormTurret.anticipationTime = enemyWormTurret.spikeAnticipation;
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_screech");
    if (!enemyWormTurret.AnticipationSpikeAnimation.IsNullOrEmpty())
    {
      yield return (object) enemyWormTurret.Spine.YieldForAnimation(enemyWormTurret.AnticipationSpikeAnimation);
    }
    else
    {
      while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) enemyWormTurret.anticipationTime)
        yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/boss/worm/spike_attack");
    enemyWormTurret.Spine.AnimationState.SetAnimation(0, enemyWormTurret.ShootAnimation, false);
    enemyWormTurret.Spine.AnimationState.AddAnimation(0, enemyWormTurret.IdleAnimation, true, 0.0f);
    enemyWormTurret.anticipating = false;
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.1f, 0.1f, false);
    yield return (object) new WaitForEndOfFrame();
    enemyWormTurret.SimpleSpineFlash.FlashWhite(false);
    int num = UnityEngine.Random.Range(0, 360);
    for (int index = 0; index < amount; ++index)
    {
      Vector3 direction = new Vector3(Mathf.Cos((float) num * ((float) Math.PI / 180f)), Mathf.Sin((float) num * ((float) Math.PI / 180f)), 0.0f);
      enemyWormTurret.StartCoroutine(enemyWormTurret.ShootSpikesInDirectionIE(direction, delayBetweenSpikes, distanceBetweenSpikes));
      num = (int) Utils.Repeat((float) (num + 360 / amount), 360f);
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) enemyWormTurret.spikeAnticipation + (double) delayBetweenSpikes + 1.0)
      yield return (object) null;
    enemyWormTurret.ShootingSpikeCircle = false;
  }

  public IEnumerator ShootSpikesInDirectionIE(
    Vector3 direction,
    float delayBetweenSpikes,
    float distanceBetweenSpikes)
  {
    EnemyWormTurret enemyWormTurret = this;
    Vector3 position = enemyWormTurret.transform.position;
    for (int i = 0; i < enemyWormTurret.circleSpikeDistance; ++i)
    {
      enemyWormTurret.GetSpawnSpike().transform.position = position;
      position += direction * distanceBetweenSpikes;
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) delayBetweenSpikes)
        yield return (object) null;
    }
  }

  public void CreatePool(int count)
  {
    this.TrailPrefab.CreatePool(count, true);
    if (!((UnityEngine.Object) this.spawnable != (UnityEngine.Object) null) || !((UnityEngine.Object) this.spawnable.gameObject != (UnityEngine.Object) null))
      return;
    this.spawnable.CreatePool<UnitObject>(this.maxActiveSpawns);
  }

  public GameObject GetSpawnSpike()
  {
    GameObject spawnSpike = this.TrailPrefab.Spawn(this.transform.parent, this.transform.position, Quaternion.identity);
    EnemyBurrowingTrail component = spawnSpike.GetComponent<EnemyBurrowingTrail>();
    this.spawnedSpikes.Add(component);
    if ((bool) (UnityEngine.Object) component)
      component.OnDeactivate += new System.Action<EnemyBurrowingTrail>(this.OnDecativateTrail);
    if (!(bool) (UnityEngine.Object) component.ColliderEvents)
      return spawnSpike;
    component.ColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    component.ColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    return spawnSpike;
  }

  public IEnumerator ShootRoutine()
  {
    EnemyWormTurret enemyWormTurret = this;
    enemyWormTurret.CacheShootDirectionCache = (UnityEngine.Object) enemyWormTurret.GetClosestTarget() == (UnityEngine.Object) null ? 0.0f : Utils.GetAngle(enemyWormTurret.transform.position, enemyWormTurret.GetClosestTarget().transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f, false);
    float randomStartAngle = (float) UnityEngine.Random.Range(0, 360);
    int i = -1;
    while ((double) ++i < (double) enemyWormTurret.NumberOfShotsToFire)
    {
      if (!string.IsNullOrEmpty(enemyWormTurret.ShootSFX))
        AudioManager.Instance.PlayOneShot(enemyWormTurret.ShootSFX, enemyWormTurret.transform.position);
      float Angle = (enemyWormTurret.BulletsTargetPlayer ? (float) ((double) enemyWormTurret.CacheShootDirectionCache - (double) enemyWormTurret.Arc / 2.0 + (double) enemyWormTurret.Arc / (double) enemyWormTurret.NumberOfShotsToFire * (double) i) : randomStartAngle) + UnityEngine.Random.Range(enemyWormTurret.RandomArcOffset.x, enemyWormTurret.RandomArcOffset.y);
      enemyWormTurret.GrenadeBullet = ObjectPool.Spawn(enemyWormTurret.Prefab, enemyWormTurret.transform.position + enemyWormTurret.ShootOffset, Quaternion.identity).GetComponent<GrenadeBullet>();
      enemyWormTurret.GrenadeBullet.SetOwner(enemyWormTurret.gameObject);
      enemyWormTurret.GrenadeBullet.Play(-1f, Angle, UnityEngine.Random.Range(enemyWormTurret.ShootDistanceRange.x, enemyWormTurret.ShootDistanceRange.y), UnityEngine.Random.Range(enemyWormTurret.GravSpeed - 2f, enemyWormTurret.GravSpeed + 2f), enemyWormTurret.health.team);
      randomStartAngle = Utils.Repeat(randomStartAngle + 360f / enemyWormTurret.NumberOfShotsToFire, 360f);
      if (enemyWormTurret.DelayBetweenShots != Vector2.zero)
      {
        float time = 0.0f;
        float dur = UnityEngine.Random.Range(enemyWormTurret.DelayBetweenShots.x, enemyWormTurret.DelayBetweenShots.y);
        while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) dur)
          yield return (object) null;
      }
    }
  }

  public override void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.Shoot);
    base.OnDisable();
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
    if (this.repositionOnHit && this.GetNewTargetPosition() && !this.ShootingSpikeCircle)
    {
      this.StopAllCoroutines();
      if (this.currentRoutine != null)
        this.StopCoroutine(this.currentRoutine);
      this.currentRoutine = this.StartCoroutine(this.MoveRoutine());
    }
    this.StartCoroutine(this.ApplyForceRoutine(Attacker));
    if ((UnityEngine.Object) this.SimpleSpineFlash != (UnityEngine.Object) null)
      this.SimpleSpineFlash.FlashFillRed();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyWormTurret enemyWormTurret = this;
    enemyWormTurret.Angle = Utils.GetAngle(Attacker.transform.position, enemyWormTurret.transform.position) * ((float) Math.PI / 180f);
    enemyWormTurret.Force = (Vector3) new Vector2(100f * Mathf.Cos(enemyWormTurret.Angle), 100f * Mathf.Sin(enemyWormTurret.Angle));
    enemyWormTurret.rb.AddForce((Vector2) enemyWormTurret.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < 0.5)
      yield return (object) null;
    if (enemyWormTurret.state.CURRENT_STATE == StateMachine.State.Idle)
      enemyWormTurret.IdleWait = 0.0f;
  }

  public IEnumerator ActiveRoutine()
  {
    EnemyWormTurret enemyWormTurret = this;
    enemyWormTurret.state.CURRENT_STATE = StateMachine.State.Idle;
    while (enemyWormTurret.state.CURRENT_STATE != StateMachine.State.Idle || (double) (enemyWormTurret.IdleWait -= Time.deltaTime) > 0.0 || !enemyWormTurret.GetNewTargetPosition())
      yield return (object) null;
    if (enemyWormTurret.currentRoutine != null)
      enemyWormTurret.StopCoroutine(enemyWormTurret.currentRoutine);
    enemyWormTurret.currentRoutine = enemyWormTurret.StartCoroutine(enemyWormTurret.DiveAboveGround ? enemyWormTurret.DiveMoveRoutine() : enemyWormTurret.MoveRoutine());
  }

  public IEnumerator DiveMoveRoutine()
  {
    EnemyWormTurret enemyWormTurret = this;
    CameraManager.shakeCamera(0.3f);
    Vector3 StartPosition = enemyWormTurret.transform.position;
    float Progress = 0.0f;
    float Duration = Vector3.Distance(StartPosition, enemyWormTurret.TargetPosition) / enemyWormTurret.MoveSpeed;
    Vector3 Curve = StartPosition + (enemyWormTurret.TargetPosition - StartPosition) / 2f + Vector3.back * enemyWormTurret.ArcHeight;
    while ((double) (Progress += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) Duration)
    {
      Vector3 a = Vector3.Lerp(StartPosition, Curve, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      Vector3 b = Vector3.Lerp(Curve, enemyWormTurret.TargetPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      enemyWormTurret.transform.position = Vector3.Lerp(a, b, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    enemyWormTurret.TargetPosition.z = 0.0f;
    enemyWormTurret.transform.position = enemyWormTurret.TargetPosition;
    enemyWormTurret.Spine.transform.localPosition = Vector3.zero;
    enemyWormTurret.state.CURRENT_STATE = StateMachine.State.Idle;
    if (enemyWormTurret.currentRoutine != null)
      enemyWormTurret.StopCoroutine(enemyWormTurret.currentRoutine);
    enemyWormTurret.currentRoutine = enemyWormTurret.StartCoroutine(enemyWormTurret.ActiveRoutine());
  }

  public IEnumerator MoveRoutine()
  {
    EnemyWormTurret enemyWormTurret = this;
    enemyWormTurret.Spine.AnimationState.AddAnimation(0, enemyWormTurret.IdleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) enemyWormTurret.initialDelay)
      yield return (object) null;
    enemyWormTurret.initialDelay = 0.0f;
    yield return (object) new WaitForEndOfFrame();
    enemyWormTurret.Spine.AnimationState.SetAnimation(0, enemyWormTurret.DisapearAnimation, false);
    enemyWormTurret.Spine.AnimationState.AddAnimation(0, enemyWormTurret.HiddenAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_disappear_underground", enemyWormTurret.gameObject);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < 0.25)
      yield return (object) null;
    enemyWormTurret.health.invincible = true;
    enemyWormTurret.previousSpawnPosition = Vector3.positiveInfinity;
    float Progress = 0.0f;
    float Duration = 0.366666675f;
    while ((double) (Progress += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) Duration)
    {
      enemyWormTurret.SpawnTrails();
      yield return (object) null;
    }
    if ((bool) (UnityEngine.Object) enemyWormTurret.lighting)
      enemyWormTurret.lighting.SetActive(false);
    enemyWormTurret.ShowHPBar?.Hide();
    enemyWormTurret.state.CURRENT_STATE = StateMachine.State.Fleeing;
    if (!enemyWormTurret.loopingSoundInstance.isValid())
      enemyWormTurret.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/enemy/tunnel_worm/tunnel_worm_underground_loop", enemyWormTurret.gameObject, true);
    Vector3 StartPosition = enemyWormTurret.transform.position with
    {
      z = 0.0f
    };
    enemyWormTurret.TargetPosition.z = 0.0f;
    Progress = 0.0f;
    Duration = Vector3.Distance(StartPosition, enemyWormTurret.TargetPosition) / enemyWormTurret.MoveSpeed;
    enemyWormTurret.previousSpawnPosition = Vector3.positiveInfinity;
    while ((double) (Progress += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) Duration)
    {
      enemyWormTurret.transform.position = Vector3.Lerp(StartPosition, enemyWormTurret.TargetPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      enemyWormTurret.SpawnTrails();
      yield return (object) null;
    }
    enemyWormTurret.transform.position = enemyWormTurret.TargetPosition;
    enemyWormTurret.health.invincible = false;
    AudioManager.Instance.StopLoop(enemyWormTurret.loopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", enemyWormTurret.gameObject);
    enemyWormTurret.Spine.AnimationState.SetAnimation(0, enemyWormTurret.AppearAnimation, false);
    enemyWormTurret.Spine.AnimationState.AddAnimation(0, enemyWormTurret.IdleAnimation, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < 0.550000011920929)
      yield return (object) null;
    if ((bool) (UnityEngine.Object) enemyWormTurret.lighting)
      enemyWormTurret.lighting.SetActive(true);
    enemyWormTurret.state.CURRENT_STATE = StateMachine.State.Idle;
    if (GameManager.RoomActive && enemyWormTurret.Shoots)
    {
      if (enemyWormTurret.canSpikeCircle && (double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
      {
        yield return (object) enemyWormTurret.StartCoroutine(enemyWormTurret.SpawnSpikesInDirectionsIE(enemyWormTurret.circleSpikeAmount, enemyWormTurret.circleDelayBetweenSpikes, enemyWormTurret.circleDistanceBetweenSpikes));
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) enemyWormTurret.MoveDelay)
          yield return (object) null;
        if (enemyWormTurret.GetNewTargetPosition())
        {
          if (enemyWormTurret.currentRoutine != null)
            enemyWormTurret.StopCoroutine(enemyWormTurret.currentRoutine);
          enemyWormTurret.currentRoutine = enemyWormTurret.StartCoroutine(enemyWormTurret.MoveRoutine());
        }
        else
        {
          if (enemyWormTurret.currentRoutine != null)
            enemyWormTurret.StopCoroutine(enemyWormTurret.currentRoutine);
          enemyWormTurret.currentRoutine = enemyWormTurret.StartCoroutine(enemyWormTurret.ActiveRoutine());
        }
      }
      else
        enemyWormTurret.StartCoroutine(enemyWormTurret.ShootAtPlayerRoutine());
    }
    else
    {
      if (enemyWormTurret.canSpikeCircle)
        yield return (object) enemyWormTurret.StartCoroutine(enemyWormTurret.SpawnSpikesInDirectionsIE(enemyWormTurret.circleSpikeAmount, enemyWormTurret.circleDelayBetweenSpikes, enemyWormTurret.circleDistanceBetweenSpikes));
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) enemyWormTurret.MoveDelay)
        yield return (object) null;
      if (enemyWormTurret.GetNewTargetPosition())
      {
        if (enemyWormTurret.currentRoutine != null)
          enemyWormTurret.StopCoroutine(enemyWormTurret.currentRoutine);
        enemyWormTurret.currentRoutine = enemyWormTurret.StartCoroutine(enemyWormTurret.MoveRoutine());
      }
      else
      {
        if (enemyWormTurret.currentRoutine != null)
          enemyWormTurret.StopCoroutine(enemyWormTurret.currentRoutine);
        enemyWormTurret.currentRoutine = enemyWormTurret.StartCoroutine(enemyWormTurret.ActiveRoutine());
      }
    }
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
      this.targetObject = PlayerFarming.FindClosestPlayerGameObject(this.transform.position);
    if (!this.targeted && (bool) (UnityEngine.Object) this.targetObject && (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) < (double) this.VisionRange)
    {
      this.targeted = true;
      if (this.currentRoutine != null)
        this.StopCoroutine(this.currentRoutine);
      this.currentRoutine = this.StartCoroutine(this.ActiveRoutine());
    }
    if (this.anticipating)
    {
      this.anticipatingTimer += Time.deltaTime;
      float amt = (float) ((double) this.anticipatingTimer / (double) this.anticipationTime * 0.75);
      this.SimpleSpineFlash.FlashWhite(amt);
      if ((double) amt > 1.0)
      {
        this.anticipating = false;
        this.anticipatingTimer = 0.0f;
      }
    }
    if (!this.targeted || !this.spawns)
      return;
    float? currentTime = GameManager.GetInstance()?.CurrentTime;
    float spawnTimestamp = this.spawnTimestamp;
    if (!((double) currentTime.GetValueOrDefault() > (double) spawnTimestamp & currentTime.HasValue) || this.spawnedAmount >= this.maxActiveSpawns)
      return;
    this.Spawn();
  }

  public void Spawn()
  {
    int num = (int) UnityEngine.Random.Range(this.spawnAmount.x, this.spawnAmount.y + 1f);
    for (int index = 0; index < num && this.spawnedAmount < this.maxActiveSpawns; ++index)
    {
      UnitObject unitObject = ObjectPool.Spawn<UnitObject>(this.spawnable, this.transform.parent, (Vector3) (UnityEngine.Random.insideUnitCircle * this.spawnRadius), Quaternion.identity);
      unitObject.health.OnDie += new Health.DieAction(this.SpawnedEnemyKilled);
      DropLootOnDeath component = unitObject.GetComponent<DropLootOnDeath>();
      if ((bool) (UnityEngine.Object) component)
        component.GiveXP = false;
      this.spawnedEnemies.Add(unitObject);
      ++this.spawnedAmount;
    }
    this.spawnTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.spawnDelay.x, this.spawnDelay.y);
  }

  public void SpawnedEnemyKilled(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    for (int index = 0; index < this.spawnedEnemies.Count; ++index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null && (UnityEngine.Object) this.spawnedEnemies[index].health == (UnityEngine.Object) Victim)
        this.spawnedEnemies[index].health.OnDie -= new Health.DieAction(this.SpawnedEnemyKilled);
    }
    --this.spawnedAmount;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    foreach (UnitObject spawnedEnemy in this.spawnedEnemies)
    {
      if ((UnityEngine.Object) spawnedEnemy != (UnityEngine.Object) null)
      {
        spawnedEnemy.health.enabled = true;
        spawnedEnemy.health.DealDamage(spawnedEnemy.health.totalHP, this.gameObject, spawnedEnemy.transform.position, AttackType: Health.AttackTypes.Heavy, dealDamageImmediately: true);
      }
    }
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    if (this.health.DestroyOnDeath)
      return;
    this.StartCoroutine(this.DelayedDestroy());
    this.gameObject.SetActive(false);
  }

  public IEnumerator DelayedDestroy()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWormTurret enemyWormTurret = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) enemyWormTurret.gameObject);
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

  public IEnumerator ShootAtPlayerRoutine()
  {
    EnemyWormTurret enemyWormTurret = this;
    float time = 0.0f;
    enemyWormTurret.anticipating = true;
    enemyWormTurret.anticipatingTimer = 0.0f;
    enemyWormTurret.anticipationTime = enemyWormTurret.AnticipationTime;
    if (!enemyWormTurret.AnticipationShootAnimation.IsNullOrEmpty())
    {
      yield return (object) enemyWormTurret.Spine.YieldForAnimation(enemyWormTurret.AnticipationShootAnimation);
    }
    else
    {
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) enemyWormTurret.anticipationTime - (double) enemyWormTurret.ShootDelay)
        yield return (object) null;
    }
    enemyWormTurret.Spine.AnimationState.SetAnimation(0, enemyWormTurret.ShootAnimation, false);
    enemyWormTurret.Spine.AnimationState.AddAnimation(0, enemyWormTurret.IdleAnimation, true, 0.0f);
    if (enemyWormTurret.AnticipationShootAnimation.IsNullOrEmpty())
    {
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) enemyWormTurret.ShootDelay)
        yield return (object) null;
    }
    enemyWormTurret.anticipating = false;
    yield return (object) new WaitForEndOfFrame();
    enemyWormTurret.SimpleSpineFlash.FlashWhite(false);
    if (enemyWormTurret.currentRoutine != null)
      enemyWormTurret.StopCoroutine(enemyWormTurret.currentRoutine);
    enemyWormTurret.currentRoutine = enemyWormTurret.StartCoroutine(enemyWormTurret.ShootRoutine());
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWormTurret.Spine.timeScale) < (double) enemyWormTurret.shootCooldown)
      yield return (object) null;
    enemyWormTurret.state.CURRENT_STATE = StateMachine.State.Idle;
    if (enemyWormTurret.currentRoutine != null)
      enemyWormTurret.StopCoroutine(enemyWormTurret.currentRoutine);
    enemyWormTurret.currentRoutine = enemyWormTurret.StartCoroutine(enemyWormTurret.ActiveRoutine());
  }

  public bool GetNewTargetPosition()
  {
    float num1 = 100f;
    if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.DistanceToPathTowardsPlayer)
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position) * ((float) Math.PI / 180f);
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
        return true;
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
      Vector3 vector3 = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      {
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
        this.TargetPosition = vector3;
        return true;
      }
    }
    return false;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
    Utils.DrawCircleXY(this.transform.position + this.ShootOffset, 0.5f, Color.yellow);
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
