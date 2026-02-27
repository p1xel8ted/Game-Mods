// Decompiled with JetBrains decompiler
// Type: EnemyWormBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using FMOD.Studio;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unify;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyWormBoss : UnitObject
{
  public SkeletonAnimation Spine;
  [SerializeField]
  private SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string hitAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string jumpAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string diveAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string popInAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string popOutAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string spikeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string shootAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string summonAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string headSmashAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string enragedAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string dieAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string deadAnimation;
  [Space]
  [SerializeField]
  private GameObject cameraTarget;
  [SerializeField]
  private Interaction_MonsterHeart interaction_MonsterHeart;
  [SerializeField]
  private ColliderEvents damageCollider;
  [SerializeField]
  private int spikeAmount = 4;
  [SerializeField]
  private GameObject spikePrefab;
  [SerializeField]
  private float spikeAnticipation;
  [SerializeField]
  private float directionalDelayBetweenSpikes = 0.2f;
  [SerializeField]
  private float directionalDistanceBetweenSpikes = 0.2f;
  [SerializeField]
  private int circleSpikeAmount = 4;
  [SerializeField]
  private float circleDelayBetweenSpikes = 0.2f;
  [SerializeField]
  private float circleDistanceBetweenSpikes = 0.2f;
  [SerializeField]
  private float inAirDuration = 2f;
  [SerializeField]
  [Range(0.0f, 1f)]
  [Tooltip("0.75 means will stop targeting the player in the last 25% of in air time")]
  private float targetPercentage = 0.7f;
  [SerializeField]
  private float popOutDelay = 2f;
  [SerializeField]
  private float targetLerpSpeed = 10f;
  [SerializeField]
  private GameObject warningObject;
  [SerializeField]
  private GameObject groundImpactVFX;
  [SerializeField]
  private GameObject bulletPrefab;
  [SerializeField]
  private float shootAnticipation = 1f;
  [SerializeField]
  private Vector2 delayBetweenShots = new Vector2(0.1f, 0.3f);
  [SerializeField]
  public float numberOfShotsToFire = 45f;
  [SerializeField]
  private Vector2 gravSpeed;
  [SerializeField]
  private float arc;
  [SerializeField]
  private Vector2 randomArcOffset = new Vector2(0.0f, 0.0f);
  [SerializeField]
  private Vector2 shootDistanceRange = new Vector2(2f, 3f);
  [SerializeField]
  private GameObject ShootBone;
  [SerializeField]
  private bool bulletsTargetPlayer = true;
  [SerializeField]
  private float moveSpeed = 2.5f;
  [SerializeField]
  private float delayBetweenTrailSpike = 0.2f;
  [SerializeField]
  private GameObject trailSpikePrefab;
  [SerializeField]
  private float headSmashAnticipation = 2f;
  [SerializeField]
  private float zSpacing;
  [SerializeField]
  private Vector3[] headSmashPositions = new Vector3[3];
  [SerializeField]
  private Vector2 p1SpawnAmount;
  [SerializeField]
  private float p1SpawnAnticipation;
  [SerializeField]
  private Vector2 p1DelayBetweenSpawns;
  [SerializeField]
  private AssetReferenceGameObject[] p1SpawnablesList;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float p2HealthThreshold = 0.6f;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float p3HealthThreshold = 0.3f;
  [SerializeField]
  private float p3MoveSpeed = 5f;
  [Space]
  [SerializeField]
  private Vector2 p3SpawnAmount;
  [SerializeField]
  private float p3SpawnAnticipation;
  [SerializeField]
  private float enragedDuration = 6f;
  [SerializeField]
  private GameObject[] deathParticlePrefabs;
  private bool targetingPlayer;
  private bool anticipating;
  private bool phaseChangeBlocked;
  private float anticipationTimer;
  private float anticipationDuration;
  private float spawnRadius = 7f;
  private float trailTimer;
  private int currentPhaseNumber = 1;
  private float startingHealth;
  private float ogSpacing;
  private bool queuePhaseIncrement;
  private bool active;
  private bool isDead;
  private List<GameObject> trailSpikes = new List<GameObject>();
  private List<GameObject> spawnedSpikes = new List<GameObject>();
  private List<UnitObject> spawnedEnemies = new List<UnitObject>();
  private Coroutine currentPhaseRoutine;
  private Coroutine currentAttackRoutine;
  private int previousAttackIndex;
  private EventInstance loopingSoundInstance;
  private List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public GameObject CameraTarget => this.cameraTarget;

  private void Start()
  {
    this.damageCollider.gameObject.SetActive(false);
    this.damageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.startingHealth = this.health.HP;
    this.ogSpacing = this.Spine.zSpacing;
    if (DataManager.Instance.playerDeathsInARowFightingLeader < 2)
      return;
    this.p1SpawnAmount -= Vector2.one;
    this.p3SpawnAmount -= Vector2.one * 2f;
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
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
      this.anticipationTimer += Time.deltaTime;
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

  private void IncrementPhase()
  {
    if (this.phaseChangeBlocked)
    {
      this.queuePhaseIncrement = true;
    }
    else
    {
      this.StopCoroutine(this.currentAttackRoutine);
      this.StopCoroutine(this.currentPhaseRoutine);
      ++this.currentPhaseNumber;
      foreach (GameObject spawnedSpike in this.spawnedSpikes)
        spawnedSpike.SetActive(false);
      this.anticipating = false;
      this.health.enabled = true;
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

  private IEnumerator DelayAddCamera()
  {
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().AddToCamera(this.cameraTarget);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
  }

  public void BeginPhase1()
  {
    GameManager.GetInstance().AddToCamera(this.cameraTarget);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase1IE(true));
  }

  private IEnumerator Phase1IE(bool firstLoop)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.active = true;
    if (firstLoop)
      yield return (object) enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnIE(enemyWormBoss.p1SpawnablesList, (int) UnityEngine.Random.Range(enemyWormBoss.p1SpawnAmount.x, enemyWormBoss.p1SpawnAmount.y + 1f), enemyWormBoss.p1SpawnAnticipation, enemyWormBoss.p1DelayBetweenSpawns));
    for (int i = 0; i < 3; ++i)
    {
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.moveSpeed, enemyWormBoss.GetRandomPosition(7f), true)));
      if (i == 2)
        yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnSpikesInDirectionsIE(enemyWormBoss.spikeAmount, enemyWormBoss.directionalDelayBetweenSpikes, enemyWormBoss.directionalDistanceBetweenSpikes)));
      else
        yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootIE()));
    }
    enemyWormBoss.currentPhaseRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.Phase1IE(false));
  }

  private void BeginPhase2()
  {
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase2IE(true));
  }

  private IEnumerator Phase2IE(bool firstLoop)
  {
    EnemyWormBoss enemyWormBoss = this;
    if (firstLoop)
    {
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.p3MoveSpeed, enemyWormBoss.GetRandomPosition(7f), false)));
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.EnragedIE());
      yield return (object) new WaitForSeconds(2f);
      yield return (object) enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnIE(enemyWormBoss.p1SpawnablesList, (int) UnityEngine.Random.Range(enemyWormBoss.p1SpawnAmount.x, enemyWormBoss.p1SpawnAmount.y + 1f), enemyWormBoss.p1SpawnAnticipation, enemyWormBoss.p1DelayBetweenSpawns, false));
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

  private void BeginPhase3()
  {
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase3IE(true));
  }

  private IEnumerator Phase3IE(bool firstLoop)
  {
    EnemyWormBoss enemyWormBoss = this;
    if (firstLoop)
    {
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.p3MoveSpeed, Vector3.zero, false)));
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.EnragedIE());
      yield return (object) new WaitForSeconds(2f);
      yield return (object) enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnIE(enemyWormBoss.p1SpawnablesList, (int) UnityEngine.Random.Range(enemyWormBoss.p3SpawnAmount.x, enemyWormBoss.p3SpawnAmount.y + 1f), enemyWormBoss.p3SpawnAnticipation, enemyWormBoss.p1DelayBetweenSpawns, false));
      yield return (object) new WaitForSeconds(2f);
    }
    for (int i = 1; i < 3; ++i)
    {
      if (i % 2 == 0)
        yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.TunnelMoveIE(enemyWormBoss.p3MoveSpeed, enemyWormBoss.GetRandomPosition(7f), true)));
      else
        yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.HeadSmashIE()));
    }
    if (UnityEngine.Random.Range(0, 2) == 0)
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.SpawnSpikesInDirectionsIE(enemyWormBoss.circleSpikeAmount, enemyWormBoss.circleDelayBetweenSpikes, enemyWormBoss.circleDistanceBetweenSpikes)));
    else
      yield return (object) (enemyWormBoss.currentAttackRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootIE()));
    enemyWormBoss.currentPhaseRoutine = enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.Phase3IE(false));
  }

  private void JumpDive() => this.StartCoroutine((IEnumerator) this.JumpDiveIE());

  private IEnumerator JumpDiveIE()
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.Spine.ForceVisible = true;
    enemyWormBoss.phaseChangeBlocked = true;
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.jumpAnimation, false);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.2f);
    yield return (object) new WaitForSeconds(1f);
    enemyWormBoss.health.enabled = false;
    yield return (object) new WaitForSeconds(0.75f);
    enemyWormBoss.warningObject.SetActive(true);
    enemyWormBoss.targetingPlayer = true;
    enemyWormBoss.transform.position = PlayerFarming.Instance.transform.position;
    float dur = enemyWormBoss.inAirDuration * enemyWormBoss.targetPercentage;
    float t = 0.0f;
    while ((double) t < (double) dur)
    {
      enemyWormBoss.transform.position = Vector3.Lerp(enemyWormBoss.transform.position, PlayerFarming.Instance.transform.position, enemyWormBoss.targetLerpSpeed * Time.deltaTime);
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyWormBoss.warningObject.transform.localPosition = Vector3.zero;
    enemyWormBoss.targetingPlayer = false;
    yield return (object) new WaitForSeconds(enemyWormBoss.inAirDuration - t);
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.diveAnimation, false);
    enemyWormBoss.warningObject.SetActive(false);
    yield return (object) new WaitForSeconds(0.3f);
    enemyWormBoss.damageCollider.gameObject.SetActive(true);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.2f, 0.2f);
    yield return (object) new WaitForSeconds(0.1f);
    if ((UnityEngine.Object) enemyWormBoss.groundImpactVFX != (UnityEngine.Object) null)
    {
      ParticleSystem component = enemyWormBoss.groundImpactVFX.GetComponent<ParticleSystem>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.Play();
    }
    enemyWormBoss.damageCollider.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(enemyWormBoss.popOutDelay);
    enemyWormBoss.health.enabled = true;
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.popInAnimation, false);
    enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    enemyWormBoss.phaseChangeBlocked = false;
    enemyWormBoss.Spine.ForceVisible = false;
  }

  private void SpawnSpikesInDirections()
  {
    this.StartCoroutine((IEnumerator) this.SpawnSpikesInDirectionsIE(this.spikeAmount, this.directionalDelayBetweenSpikes, this.directionalDistanceBetweenSpikes));
  }

  private void SpawnSpikesInCircle()
  {
    this.StartCoroutine((IEnumerator) this.SpawnSpikesInDirectionsIE(this.circleSpikeAmount, this.circleDelayBetweenSpikes, this.circleDistanceBetweenSpikes));
  }

  private IEnumerator SpawnSpikesInDirectionsIE(
    int amount,
    float delayBetweenSpikes,
    float distanceBetweenSpikes)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.phaseChangeBlocked = true;
    yield return (object) new WaitForEndOfFrame();
    int num = UnityEngine.Random.Range(0, 360);
    for (int index = 0; index < amount; ++index)
    {
      Vector3 direction = new Vector3(Mathf.Cos((float) num * ((float) Math.PI / 180f)), Mathf.Sin((float) num * ((float) Math.PI / 180f)), 0.0f);
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.ShootSpikesInDirectionIE(direction, delayBetweenSpikes, distanceBetweenSpikes));
      num = (int) Mathf.Repeat((float) (num + 360 / amount), 360f);
    }
    AudioManager.Instance.PlayOneShot("event:/boss/worm/spike_attack", AudioManager.Instance.Listener);
    yield return (object) new WaitForSeconds((float) ((double) enemyWormBoss.spikeAnticipation + (double) delayBetweenSpikes + 1.0));
    enemyWormBoss.phaseChangeBlocked = false;
  }

  public IEnumerator ShootSpikesInDirectionIE(
    Vector3 direction,
    float delayBetweenSpikes,
    float distanceBetweenSpikes)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.spikeAnimation, false);
    enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
    enemyWormBoss.anticipating = true;
    enemyWormBoss.anticipationDuration = enemyWormBoss.spikeAnticipation;
    yield return (object) new WaitForSeconds(enemyWormBoss.spikeAnticipation);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, 0.5f);
    yield return (object) new WaitForEndOfFrame();
    Vector3 position = enemyWormBoss.transform.position;
    for (int i = 0; i < 30; ++i)
    {
      enemyWormBoss.GetSpawnSpike().transform.position = position;
      position += direction * distanceBetweenSpikes;
      if ((bool) Physics2D.Raycast((Vector2) position, (Vector2) direction, 1f, (int) enemyWormBoss.layerToCheck))
        break;
      yield return (object) new WaitForSeconds(delayBetweenSpikes);
    }
  }

  private GameObject GetSpawnSpike()
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
      ColliderEvents componentInChildren = spawnSpike.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren)
        componentInChildren.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    return spawnSpike;
  }

  private void Shoot() => this.StartCoroutine((IEnumerator) this.ShootIE());

  private IEnumerator ShootIE()
  {
    EnemyWormBoss enemyWormBoss = this;
    AudioManager.Instance.PlayOneShot("event:/boss/worm/spit_projectiles", AudioManager.Instance.Listener);
    enemyWormBoss.anticipating = true;
    enemyWormBoss.anticipationDuration = enemyWormBoss.shootAnticipation;
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.shootAnimation, false);
    enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyWormBoss.shootAnticipation);
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.phaseChangeBlocked = true;
    float angle = Utils.GetAngle(enemyWormBoss.transform.position, PlayerFarming.Instance.transform.position);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.4f);
    int index = -1;
    while ((double) ++index < (double) enemyWormBoss.numberOfShotsToFire)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", AudioManager.Instance.Listener);
      enemyWormBoss.StartCoroutine((IEnumerator) enemyWormBoss.BulletDelay(angle, index, UnityEngine.Random.Range(enemyWormBoss.delayBetweenShots.x, enemyWormBoss.delayBetweenShots.y)));
    }
    yield return (object) new WaitForSeconds(1f);
    enemyWormBoss.phaseChangeBlocked = false;
  }

  private IEnumerator BulletDelay(float shootAngle, int index, float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    ObjectPool.Spawn(this.bulletPrefab, this.ShootBone.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>().Play(-6f, (float) ((double) shootAngle - (double) this.arc / 2.0 + (double) this.arc / (double) this.numberOfShotsToFire * (double) index) + UnityEngine.Random.Range(this.randomArcOffset.x, this.randomArcOffset.y), UnityEngine.Random.Range(this.shootDistanceRange.x, this.shootDistanceRange.y), UnityEngine.Random.Range(this.gravSpeed.x, this.gravSpeed.y));
  }

  private void SpawnP1()
  {
    this.StartCoroutine((IEnumerator) this.SpawnIE(this.p1SpawnablesList, (int) UnityEngine.Random.Range(this.p1SpawnAmount.x, this.p1SpawnAmount.y + 1f), this.p1SpawnAnticipation, this.p1DelayBetweenSpawns));
  }

  private void SpawnP3()
  {
    this.StartCoroutine((IEnumerator) this.SpawnIE(this.p1SpawnablesList, (int) UnityEngine.Random.Range(this.p3SpawnAmount.x, this.p3SpawnAmount.y + 1f), this.p3SpawnAnticipation, this.p1DelayBetweenSpawns));
  }

  private IEnumerator SpawnIE(
    AssetReferenceGameObject[] spawnables,
    int amount,
    float anticipationTime,
    Vector2 delayBetweenSpawns,
    bool playAnimations = true)
  {
    EnemyWormBoss enemyWormBoss1 = this;
    enemyWormBoss1.phaseChangeBlocked = true;
    AudioManager.Instance.PlayOneShot("event:/boss/worm/spike_attack", AudioManager.Instance.Listener);
    yield return (object) new WaitForEndOfFrame();
    if (playAnimations)
    {
      enemyWormBoss1.Spine.AnimationState.SetAnimation(0, enemyWormBoss1.summonAnimation, false);
      enemyWormBoss1.Spine.AnimationState.AddAnimation(0, enemyWormBoss1.idleAnimation, true, 0.0f);
      yield return (object) new WaitForSeconds(anticipationTime);
      yield return (object) new WaitForEndOfFrame();
      CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
    }
    for (int i = 0; i < amount; ++i)
    {
      EnemyWormBoss enemyWormBoss = enemyWormBoss1;
      Vector3 spawnPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * enemyWormBoss1.spawnRadius);
      Addressables.LoadAssetAsync<GameObject>((object) spawnables[UnityEngine.Random.Range(0, spawnables.Length)]).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        enemyWormBoss.loadedAddressableAssets.Add(obj);
        UnitObject component1 = EnemySpawner.Create(spawnPosition, enemyWormBoss.transform.parent, obj.Result).GetComponent<UnitObject>();
        component1.CanHaveModifier = false;
        component1.RemoveModifier();
        enemyWormBoss.spawnedEnemies.Add(component1);
        DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
        if (!(bool) (UnityEngine.Object) component2)
          return;
        component2.GiveXP = false;
      });
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(delayBetweenSpawns.x, delayBetweenSpawns.y));
    }
    yield return (object) new WaitForSeconds(1f);
    enemyWormBoss1.phaseChangeBlocked = false;
  }

  private IEnumerator SpawnIE(
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
    yield return (object) new WaitForSeconds(anticipationTime);
    yield return (object) new WaitForEndOfFrame();
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
    for (int index = 0; index < amount; ++index)
    {
      if (!enemyWormBoss.isDead)
      {
        Vector3 position = (Vector3) (UnityEngine.Random.insideUnitCircle * enemyWormBoss.spawnRadius);
        UnitObject unitObject = UnityEngine.Object.Instantiate<UnitObject>(spawnables[UnityEngine.Random.Range(0, spawnables.Length)], position, Quaternion.identity, enemyWormBoss.transform.parent);
        unitObject.CanHaveModifier = false;
        unitObject.RemoveModifier();
        unitObject.transform.position = spawnPositions[index];
        enemyWormBoss.spawnedEnemies.Add(unitObject);
        DropLootOnDeath component = unitObject.GetComponent<DropLootOnDeath>();
        if ((bool) (UnityEngine.Object) component)
          component.GiveXP = false;
      }
    }
    yield return (object) new WaitForSeconds(1f);
    enemyWormBoss.phaseChangeBlocked = false;
  }

  private void TunnelMove()
  {
    this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.TunnelMoveIE(this.moveSpeed, this.GetRandomPosition(7f), true));
  }

  private IEnumerator TunnelMoveIE(float moveSpeed, Vector3 position, bool popOut)
  {
    EnemyWormBoss enemyWormBoss = this;
    enemyWormBoss.phaseChangeBlocked = true;
    yield return (object) new WaitForEndOfFrame();
    enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.popOutAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", enemyWormBoss.gameObject);
    yield return (object) new WaitForSeconds(1f);
    enemyWormBoss.health.enabled = false;
    enemyWormBoss.Spine.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_disappear_underground", enemyWormBoss.gameObject);
    float Progress = 0.0f;
    float Duration = 0.366666675f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      enemyWormBoss.SpawnTrailSpikes();
      yield return (object) null;
    }
    enemyWormBoss.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/enemy/tunnel_worm/tunnel_worm_underground_loop", enemyWormBoss.gameObject, true);
    Vector3 startPosition = enemyWormBoss.transform.position;
    Progress = 0.0f;
    Duration = Vector3.Distance(startPosition, position) / moveSpeed;
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.3f, Duration);
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      enemyWormBoss.transform.position = Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      enemyWormBoss.SpawnTrailSpikes();
      yield return (object) null;
    }
    enemyWormBoss.transform.position = position;
    enemyWormBoss.health.enabled = true;
    enemyWormBoss.Spine.gameObject.SetActive(true);
    AudioManager.Instance.StopLoop(enemyWormBoss.loopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", enemyWormBoss.gameObject);
    enemyWormBoss.simpleSpineFlash.FlashWhite(false);
    if (popOut)
    {
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, enemyWormBoss.popInAnimation, false);
      enemyWormBoss.Spine.AnimationState.AddAnimation(0, enemyWormBoss.idleAnimation, true, 0.0f);
      yield return (object) new WaitForSeconds(1.5f);
      enemyWormBoss.phaseChangeBlocked = false;
    }
    else
      enemyWormBoss.phaseChangeBlocked = false;
  }

  private void SpawnTrailSpikes()
  {
    if ((double) (this.trailTimer += Time.deltaTime) <= (double) this.delayBetweenTrailSpike)
      return;
    this.trailTimer = 0.0f;
    this.GetTrailSpike();
  }

  private GameObject GetTrailSpike()
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
      trailSpike1 = UnityEngine.Object.Instantiate<GameObject>(this.trailSpikePrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      this.trailSpikes.Add(trailSpike1);
      ColliderEvents componentInChildren = trailSpike1.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren)
        componentInChildren.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    return trailSpike1;
  }

  private void HeadSmash() => this.StartCoroutine((IEnumerator) this.HeadSmashIE());

  private IEnumerator HeadSmashIE()
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
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    DOTween.To(new DOGetter<float>(enemyWormBoss.\u003CHeadSmashIE\u003Eb__116_0), new DOSetter<float>(enemyWormBoss.\u003CHeadSmashIE\u003Eb__116_1), enemyWormBoss.ogSpacing, 0.25f);
    yield return (object) new WaitForSeconds(1f);
    enemyWormBoss.phaseChangeBlocked = false;
  }

  private IEnumerator EnableDamageCollider(float time, Vector3 position)
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
    yield return (object) new WaitForSeconds(this.enragedDuration);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 1f);
    yield return (object) new WaitForSeconds(1.5f);
  }

  private Vector3 GetRandomPosition(float radius) => (Vector3) (UnityEngine.Random.insideUnitCircle * radius);

  protected virtual void OnDamageTriggerEnter(Collider2D collider)
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
    PlayerFarming.Instance.health.invincible = true;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_BOSS_1"));
    this.damageCollider.gameObject.SetActive(false);
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
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
    this.isDead = true;
    GameManager.GetInstance().CamFollowTarget.MinZoom = 6f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 20f;
    this.StopCoroutine(this.currentAttackRoutine);
    this.StopCoroutine(this.currentPhaseRoutine);
    this.StartCoroutine((IEnumerator) this.Die());
  }

  private IEnumerator Die()
  {
    EnemyWormBoss enemyWormBoss = this;
    foreach (GameObject deathParticlePrefab in enemyWormBoss.deathParticlePrefabs)
      UnityEngine.Object.Instantiate<GameObject>(deathParticlePrefab, enemyWormBoss.transform.position, Quaternion.identity, enemyWormBoss.transform.parent);
    enemyWormBoss.ClearPaths();
    enemyWormBoss.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew();
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
    enemyWormBoss.simpleSpineFlash.StopAllCoroutines();
    enemyWormBoss.simpleSpineFlash.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
    enemyWormBoss.state.CURRENT_STATE = StateMachine.State.Dieing;
    AudioManager.Instance.PlayOneShot("event:/boss/worm/death", AudioManager.Instance.Listener);
    if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
    {
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, "die", false);
      enemyWormBoss.Spine.AnimationState.AddAnimation(0, "dead", true, 0.0f);
    }
    else
    {
      enemyWormBoss.Spine.AnimationState.SetAnimation(0, "die-noheart", false);
      enemyWormBoss.Spine.AnimationState.AddAnimation(0, "dead-noheart", true, 0.0f);
    }
    yield return (object) new WaitForSeconds(4.2f);
    for (int index = 0; index < 20; ++index)
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemyWormBoss.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f), Vector3.zero, Color.red);
    GameManager.GetInstance().OnConversationEnd();
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      enemyWormBoss.interaction_MonsterHeart.ObjectiveToComplete = Objectives.CustomQuestTypes.BishopsOfTheOldFaith1;
    enemyWormBoss.interaction_MonsterHeart.Play();
  }

  private void OnDrawGizmosSelected()
  {
    foreach (Vector3 headSmashPosition in this.headSmashPositions)
      Utils.DrawCircleXY(this.transform.InverseTransformPoint(headSmashPosition), 0.5f, Color.yellow);
  }
}
