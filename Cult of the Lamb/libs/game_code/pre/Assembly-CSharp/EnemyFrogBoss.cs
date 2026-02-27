// Decompiled with JetBrains decompiler
// Type: EnemyFrogBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
public class EnemyFrogBoss : UnitObject
{
  public SkeletonAnimation Spine;
  [SerializeField]
  private SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string hopAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string hopAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string hopEndAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string mortarStrikeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string mortarValleyAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string eggSpawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string burpAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string bounceAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string enragedAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string tongueAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string tongueEndAnimation;
  [SerializeField]
  private CircleCollider2D physicsCollider;
  [SerializeField]
  private float hopAnticipation;
  [SerializeField]
  private float hopDuration;
  [SerializeField]
  private float hopSpeed;
  [SerializeField]
  private float hopZHeight;
  [SerializeField]
  private AnimationCurve hopSpeedCurve;
  [SerializeField]
  private AnimationCurve hopZCurve;
  [SerializeField]
  private ColliderEvents damageColliderEvents;
  [SerializeField]
  private LayerMask unitMask;
  [SerializeField]
  private GameObject shadow;
  [SerializeField]
  private MortarBomb randomMortarPrefab;
  [SerializeField]
  private float randomMortarDuration;
  [SerializeField]
  private float randomMortarAnticipation;
  [SerializeField]
  private Vector2 randomMortarsToSpawn;
  [SerializeField]
  private Vector2 randomMortarDistance;
  [SerializeField]
  private Vector2 randomMortarDelayBetweenShots;
  [SerializeField]
  private MortarBomb targetedMortarPrefab;
  [SerializeField]
  private float targetedMortarDuration;
  [SerializeField]
  private float targetedMortarAnticipation;
  [SerializeField]
  private Vector2 targetedMortarsToSpawn;
  [SerializeField]
  private Vector2 targetedMortarDelayBetweenShots;
  [SerializeField]
  private GameObject eggPrefab;
  [SerializeField]
  private Vector2 eggsToSpawn;
  [SerializeField]
  private float eggSpawnDelay;
  [SerializeField]
  private float eggsMultipleAnticipation;
  [SerializeField]
  private Vector2 eggsMultipleToSpawn;
  [SerializeField]
  private Vector2 eggsMultipleSpawnDelay;
  [SerializeField]
  private Vector2 eggsKnockback;
  [SerializeField]
  private ParticleSystem aoeParticles;
  [SerializeField]
  private float aoeDuration;
  [SerializeField]
  private Vector2 projectilesToSpawn;
  [SerializeField]
  private Vector2 projectileDelayBetweenSpawn;
  [SerializeField]
  private GameObject burpPosition;
  [SerializeField]
  private float burpAnticipation;
  [SerializeField]
  private GameObject projectilePrefab;
  [SerializeField]
  private Vector2 bounces;
  [SerializeField]
  private float bounceAnticipation;
  [SerializeField]
  private Vector2 timeBetweenBounce;
  [SerializeField]
  private Vector3 sitPosition = new Vector3(0.0f, 7.75f, -0.2f);
  [SerializeField]
  private float tongueSpitDelay = 0.25f;
  [SerializeField]
  private FrogBossTongue tonguePrefab;
  [SerializeField]
  private GameObject tonguePosition;
  [SerializeField]
  private float tongueWhipAnticipation = 1f;
  [SerializeField]
  private float tongueWhipDuration = 0.5f;
  [SerializeField]
  private float tongueRetrieveDelay = 2f;
  [SerializeField]
  private float tongueRetrieveDuration = 0.1f;
  [SerializeField]
  private Vector2 tongueWhipDelay;
  [SerializeField]
  private Vector2 tongueWhipAmount;
  [SerializeField]
  private float tongueScatterRadius = 10f;
  [SerializeField]
  private float tongueScatterPostAnticipation;
  [SerializeField]
  private float tongueScatterPreAnticipation;
  [SerializeField]
  private float tongueScatterWhipDuration;
  [SerializeField]
  private Vector2 tongueScatterDelay;
  [SerializeField]
  private Vector2 tongueScatterAmount;
  [SerializeField]
  private float enragedDuration = 2f;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float p2HealthThreshold = 0.6f;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float p3HealthThreshold = 0.3f;
  [SerializeField]
  private AssetReferenceGameObject miniBossFrogTarget;
  [SerializeField]
  private float miniBossSpawningDelay = 0.5f;
  [SerializeField]
  private int miniBossFrogsSpawnAmount;
  [SerializeField]
  private float spawnForce = 0.75f;
  [SerializeField]
  private Vector3[] miniBossSpawnPositions = new Vector3[0];
  [SerializeField]
  private int maxEnemies;
  [SerializeField]
  private AssetReferenceGameObject enemyHopperTarget;
  [SerializeField]
  private Vector2 spawnDelay;
  [SerializeField]
  private Vector3[] spawnPositions = new Vector3[0];
  [Space]
  [SerializeField]
  private GameObject cameraTarget;
  [SerializeField]
  private Renderer distortionObject;
  [SerializeField]
  private Interaction_MonsterHeart interaction_MonsterHeart;
  [SerializeField]
  private GameObject playerBlocker;
  private bool attacking;
  private bool anticipating;
  private bool hasCollidedWithObstacle;
  private bool queuePhaseIncrement;
  private float anticipationTimer;
  private float anticipationDuration;
  private float targetAngle;
  private float startingHealth;
  private int currentPhaseNumber;
  private Coroutine currentPhaseRoutine;
  private Coroutine currentAttackRoutine;
  private IEnumerator damageColliderRoutine;
  private Collider2D collider;
  private ShowHPBar hpBar;
  private int enemiesAlive;
  private List<UnitObject> spawnedEnemies = new List<UnitObject>();
  private List<FrogBossTongue> tongues = new List<FrogBossTongue>();
  private List<Projectile> projectiles = new List<Projectile>();
  private bool isDead;
  private bool facePlayer;
  private bool usingTongue;
  private bool active;
  private float spawnTimestamp;
  private int miniBossesKilled;
  private int previousAttackIndex;

  public GameObject TonguePosition => this.tonguePosition;

  private void Start()
  {
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationEvent);
    this.collider = this.GetComponent<Collider2D>();
    this.hpBar = this.GetComponent<ShowHPBar>();
    this.startingHealth = this.health.HP;
    if (DataManager.Instance.playerDeathsInARowFightingLeader >= 2)
      this.maxEnemies -= 3;
    this.facePlayer = true;
  }

  public override void Update()
  {
    if (!this.usingTongue)
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

  private void IncrementPhase()
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

  private IEnumerator DelayAddCamera()
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
    this.currentPhaseNumber = 1;
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase1IE(true));
  }

  private IEnumerator Phase1IE(bool firstLoop)
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
      switch (num2)
      {
        case 0:
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(enemyFrogBoss.hopAnticipation, enemyFrogBoss.GetAngleToTarget())));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.BurpProjectilesIE()));
          break;
        case 1:
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.sitPosition)));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.TongueRapidAttackIE()));
          break;
        case 2:
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(PlayerFarming.Instance.transform.position)));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.MortarStrikeTargetedIE()));
          yield return (object) new WaitForSeconds(1f);
          break;
      }
    }
    enemyFrogBoss.currentPhaseRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.Phase1IE(false));
  }

  private void BeginPhase2()
  {
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase2IE(true));
  }

  private IEnumerator Phase2IE(bool firstLoop)
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
      switch (num)
      {
        case 0:
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(PlayerFarming.Instance.transform.position)));
          yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.MortarStrikeRandomIE()));
          break;
        case 1:
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

  private void BeginPhase3()
  {
    this.currentPhaseRoutine = this.StartCoroutine((IEnumerator) this.Phase3IE(true));
  }

  private IEnumerator Phase3IE(bool firstLoop)
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
        switch (num)
        {
          case 0:
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(enemyFrogBoss.hopAnticipation, enemyFrogBoss.GetAngleToTarget())));
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.MortarStrikeTargetedIE()));
            break;
          case 1:
            yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopToPositionIE(enemyFrogBoss.sitPosition));
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.TongueScatterAttackIE()));
            break;
          case 2:
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopIE(enemyFrogBoss.hopAnticipation, enemyFrogBoss.GetAngleToTarget())));
            yield return (object) (enemyFrogBoss.currentAttackRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.BurpProjectilesIE(1.6f)));
            break;
        }
      }
      enemyFrogBoss.currentPhaseRoutine = enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.Phase3IE(false));
    }
  }

  private void MortarStrikeRandom()
  {
    this.StartCoroutine((IEnumerator) this.MortarStrikeRandomIE());
  }

  private IEnumerator MortarStrikeRandomIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = enemyFrogBoss.randomMortarAnticipation;
    yield return (object) new WaitForSeconds(enemyFrogBoss.anticipationDuration);
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.attacking = true;
    enemyFrogBoss.facePlayer = false;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.mortarValleyAnimation, false);
    yield return (object) new WaitForSeconds(0.6f);
    int shotsToFire = (int) UnityEngine.Random.Range(enemyFrogBoss.randomMortarsToSpawn.x, enemyFrogBoss.randomMortarsToSpawn.y);
    float aimingAngle = UnityEngine.Random.Range(0.0f, 360f);
    for (int i = 0; i < shotsToFire; ++i)
    {
      Vector3 targetPosition = enemyFrogBoss.transform.position + (Vector3) Utils.DegreeToVector2(aimingAngle) * UnityEngine.Random.Range(enemyFrogBoss.randomMortarDistance.x, enemyFrogBoss.randomMortarDistance.y);
      enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.ShootMortarPosition(targetPosition));
      aimingAngle += (float) (360 / shotsToFire);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyFrogBoss.randomMortarDelayBetweenShots.x, enemyFrogBoss.randomMortarDelayBetweenShots.y));
    }
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    enemyFrogBoss.attacking = false;
    yield return (object) new WaitForSeconds(1f);
    enemyFrogBoss.facePlayer = true;
  }

  private void MortarStrikeTargeted()
  {
    this.StartCoroutine((IEnumerator) this.MortarStrikeTargetedIE());
  }

  private IEnumerator MortarStrikeTargetedIE()
  {
    this.anticipating = true;
    this.anticipationDuration = this.targetedMortarAnticipation;
    yield return (object) new WaitForSeconds(this.anticipationDuration);
    yield return (object) new WaitForEndOfFrame();
    this.attacking = true;
    int shotsToFire = (int) UnityEngine.Random.Range(this.targetedMortarsToSpawn.x, this.targetedMortarsToSpawn.y);
    for (int i = 0; i < shotsToFire; ++i)
    {
      this.Spine.AnimationState.SetAnimation(0, this.mortarStrikeAnimation, false);
      AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_spit");
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(this.targetedMortarDelayBetweenShots.x, this.targetedMortarDelayBetweenShots.y));
    }
    this.Spine.AnimationState.SetAnimation(0, this.idleAnimation, false);
    this.attacking = false;
    yield return (object) new WaitForSeconds(0.5f);
  }

  private IEnumerator ShootMortarTarget()
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    EnemyFrogBoss enemyFrogBoss = this;
    if (num1 != 0)
    {
      if (num1 != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_explode");
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    MortarBomb mortarBomb = UnityEngine.Object.Instantiate<MortarBomb>(enemyFrogBoss.targetedMortarPrefab, (Vector3) AstarPath.active.GetNearest(PlayerFarming.Instance.transform.position).node.position, Quaternion.identity, enemyFrogBoss.transform.parent);
    float num2 = Mathf.Clamp(Vector3.Distance(enemyFrogBoss.transform.position, enemyFrogBoss.burpPosition.transform.position) / 3f, 1f, float.MaxValue);
    Vector3 position = enemyFrogBoss.burpPosition.transform.position;
    double moveDuration = (double) enemyFrogBoss.targetedMortarDuration * (double) num2;
    mortarBomb.Play(position, (float) moveDuration, Health.Team.Team2);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_spawn");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyFrogBoss.targetedMortarDuration * num2);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator ShootMortarPosition(Vector3 targetPosition)
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
    UnityEngine.Object.Instantiate<MortarBomb>(enemyFrogBoss.randomMortarPrefab, (Vector3) AstarPath.active.GetNearest(targetPosition).node.position, Quaternion.identity, enemyFrogBoss.transform.parent).Play(enemyFrogBoss.burpPosition.transform.position, enemyFrogBoss.randomMortarDuration, Health.Team.Team2);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_spawn");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyFrogBoss.randomMortarDuration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void SpawnEggs() => this.StartCoroutine((IEnumerator) this.SpawnEggsIE());

  private void SpawnEggsCircle() => this.StartCoroutine((IEnumerator) this.SpawnEggsCircleIE(1f));

  private void SpawnEggsForward()
  {
    this.StartCoroutine((IEnumerator) this.SpawnEggsForward(1f, 1.3f));
  }

  private IEnumerator SpawnEggsIE()
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

  private IEnumerator SpawnEggsCircleIE(float spawnMultiplier)
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

  private IEnumerator SpawnEggsForward(float spawnMultiplier, float knockMultiplier)
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
      aimingAngle = Mathf.Repeat(aimingAngle + 1f / (float) eggCount, 2f) - 1f;
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyFrogBoss.eggsMultipleSpawnDelay.x, enemyFrogBoss.eggsMultipleSpawnDelay.y));
    }
    yield return (object) new WaitForSeconds(1f);
    enemyFrogBoss.attacking = false;
  }

  private void SpawnEgg(Vector3 position, float knockback = 0.0f, float angle = 0.0f)
  {
    UnitObject component = UnityEngine.Object.Instantiate<GameObject>(this.eggPrefab, position, Quaternion.identity, (UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null ? this.transform.parent : (Transform) null).GetComponent<UnitObject>();
    if ((double) knockback == 0.0)
      return;
    component.DoKnockBack(angle, knockback, 1f);
  }

  private void Hop()
  {
    this.StartCoroutine((IEnumerator) this.HopIE(this.hopAnticipation, this.GetAngleToTarget()));
  }

  private void HopToBack()
  {
    this.StartCoroutine((IEnumerator) this.HopToPositionIE(this.sitPosition));
  }

  private void HopAOE()
  {
    this.StartCoroutine((IEnumerator) this.HopIE(this.hopAnticipation, this.GetAngleToTarget()));
  }

  private void HopUp() => this.StartCoroutine((IEnumerator) this.HopUpIE(this.hopAnticipation));

  private void HopDown() => this.StartCoroutine((IEnumerator) this.HopDownIE());

  private IEnumerator HopIE(float anticipation, float angle)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = anticipation;
    enemyFrogBoss.facePlayer = false;
    enemyFrogBoss.targetAngle = angle;
    enemyFrogBoss.LookAt(angle);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnticipationAnimation, false);
    enemyFrogBoss.playerBlocker.SetActive(false);
    yield return (object) new WaitForSeconds(0.5f);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/jump");
    yield return (object) new WaitForSeconds(0.1f);
    enemyFrogBoss.hpBar.Hide();
    Physics2D.IgnoreCollision(enemyFrogBoss.collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, true);
    float hopStartTime = GameManager.GetInstance().CurrentTime;
    float t = 0.0f;
    while ((double) t < (double) enemyFrogBoss.hopDuration)
    {
      enemyFrogBoss.speed = enemyFrogBoss.hopSpeedCurve.Evaluate(GameManager.GetInstance().TimeSince(hopStartTime) / enemyFrogBoss.hopDuration) * enemyFrogBoss.hopSpeed;
      enemyFrogBoss.Spine.transform.localPosition = -Vector3.forward * enemyFrogBoss.hopZCurve.Evaluate(GameManager.GetInstance().TimeSince(hopStartTime) / enemyFrogBoss.hopDuration) * enemyFrogBoss.hopZHeight;
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyFrogBoss.Spine.transform.localPosition = Vector3.zero;
    Physics2D.IgnoreCollision(enemyFrogBoss.collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, false);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopEndAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/land");
    enemyFrogBoss.DoAOE();
    enemyFrogBoss.facePlayer = true;
    enemyFrogBoss.playerBlocker.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
  }

  private IEnumerator HopToPositionIE(Vector3 position)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnticipationAnimation, false);
    enemyFrogBoss.playerBlocker.SetActive(false);
    yield return (object) new WaitForSeconds(0.5f);
    Physics2D.IgnoreCollision(enemyFrogBoss.collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, true);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/jump");
    yield return (object) new WaitForSeconds(0.1f);
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
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyFrogBoss.Spine.transform.localPosition = Vector3.zero;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopEndAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/land");
    enemyFrogBoss.DoAOE();
    enemyFrogBoss.playerBlocker.SetActive(true);
    Physics2D.IgnoreCollision(enemyFrogBoss.collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, false);
    yield return (object) new WaitForSeconds(0.5f);
  }

  private IEnumerator HopUpIE(float anticipation)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = anticipation;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnticipationAnimation, false);
    yield return (object) new WaitForSeconds(enemyFrogBoss.anticipationDuration);
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.hopAnimation, false);
    enemyFrogBoss.playerBlocker.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/jump");
    GameManager.GetInstance().RemoveFromCamera(enemyFrogBoss.cameraTarget);
    Physics2D.IgnoreCollision(enemyFrogBoss.collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, true);
    enemyFrogBoss.health.invincible = true;
    enemyFrogBoss.shadow.SetActive(false);
    enemyFrogBoss.hpBar.Hide();
    float t = 0.0f;
    while ((double) t < (double) enemyFrogBoss.hopDuration)
    {
      enemyFrogBoss.Spine.transform.localPosition += -(Vector3.forward * 5f) * enemyFrogBoss.hopSpeed;
      t += Time.deltaTime;
      yield return (object) null;
    }
  }

  private IEnumerator HopDownIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    Physics2D.IgnoreCollision(enemyFrogBoss.collider, (Collider2D) PlayerFarming.Instance.circleCollider2D, false);
    enemyFrogBoss.health.invincible = false;
    enemyFrogBoss.shadow.SetActive(true);
    float t = 0.0f;
    while ((double) t < (double) enemyFrogBoss.hopDuration)
    {
      enemyFrogBoss.Spine.transform.localPosition -= -Vector3.forward * enemyFrogBoss.hopSpeed * 2f;
      t += Time.deltaTime;
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

  private void DoAOE()
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

  private void BurpProjectiles() => this.StartCoroutine((IEnumerator) this.BurpProjectilesIE());

  private IEnumerator BurpProjectilesIE(float spawnMultiplier = 1f)
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.anticipating = true;
    enemyFrogBoss.anticipationDuration = enemyFrogBoss.burpAnticipation;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.burpAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyFrogBoss.anticipationDuration);
    yield return (object) new WaitForEndOfFrame();
    enemyFrogBoss.LookAt(enemyFrogBoss.GetAngleToTarget());
    int amountToSpawn = (int) ((double) UnityEngine.Random.Range(enemyFrogBoss.projectilesToSpawn.x, enemyFrogBoss.projectilesToSpawn.y) * (double) spawnMultiplier);
    for (int i = 0; i < amountToSpawn; ++i)
    {
      Projectile component = UnityEngine.Object.Instantiate<GameObject>(enemyFrogBoss.projectilePrefab, enemyFrogBoss.transform.parent).GetComponent<Projectile>();
      component.transform.position = new Vector3(enemyFrogBoss.burpPosition.transform.position.x, enemyFrogBoss.burpPosition.transform.position.y, 0.0f);
      component.GetComponentInChildren<SkeletonAnimation>().transform.localPosition = new Vector3(0.0f, 0.0f, enemyFrogBoss.burpPosition.transform.position.z);
      component.ModifyingZ = true;
      component.Angle = UnityEngine.Random.Range(-90f, 0.0f);
      component.team = enemyFrogBoss.health.team;
      component.Speed += UnityEngine.Random.Range(-0.5f, 0.5f);
      component.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
      component.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
      component.LifeTime += UnityEngine.Random.Range(0.0f, 0.3f);
      component.Owner = enemyFrogBoss.health;
      component.InvincibleTime = 1f;
      component.SetTarget(PlayerFarming.Health);
      enemyFrogBoss.projectiles.Add(component);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyFrogBoss.projectileDelayBetweenSpawn.x, enemyFrogBoss.projectileDelayBetweenSpawn.y));
    }
    yield return (object) new WaitForSeconds(1f);
  }

  private void BounceAOE() => this.StartCoroutine((IEnumerator) this.BounceAOEIE());

  private IEnumerator BounceAOEIE()
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

  private void TongueRapidAttack() => this.StartCoroutine((IEnumerator) this.TongueRapidAttackIE());

  private void TongueScatterAttack()
  {
    this.StartCoroutine((IEnumerator) this.TongueScatterAttackIE());
  }

  private IEnumerator TongueRapidAttackIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.usingTongue = true;
    int tongueAttackAmount = (int) UnityEngine.Random.Range(enemyFrogBoss.tongueWhipAmount.x, enemyFrogBoss.tongueWhipAmount.y);
    for (int i = 0; i < tongueAttackAmount; ++i)
    {
      enemyFrogBoss.anticipating = true;
      enemyFrogBoss.anticipationDuration = enemyFrogBoss.tongueWhipAnticipation + enemyFrogBoss.tongueSpitDelay;
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.tongueAttackAnimation, false);
      yield return (object) new WaitForSeconds(enemyFrogBoss.anticipationDuration);
      yield return (object) new WaitForEndOfFrame();
      enemyFrogBoss.facePlayer = false;
      enemyFrogBoss.attacking = true;
      AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_attack");
      Vector3 position = PlayerFarming.Instance.transform.position;
      enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.GetTongue().SpitTongueIE(position, enemyFrogBoss.tongueSpitDelay, enemyFrogBoss.tongueWhipDuration, enemyFrogBoss.tongueRetrieveDelay, enemyFrogBoss.tongueRetrieveDuration));
      yield return (object) new WaitForSeconds((float) ((double) enemyFrogBoss.tongueWhipDuration + (double) enemyFrogBoss.tongueRetrieveDelay + 0.5));
      AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_return");
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.tongueEndAnimation, false);
      enemyFrogBoss.attacking = false;
    }
    yield return (object) new WaitForSeconds(1f);
    enemyFrogBoss.facePlayer = true;
    enemyFrogBoss.usingTongue = false;
  }

  private IEnumerator TongueScatterAttackIE()
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
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_attack");
    for (int i = 0; i < amount; ++i)
    {
      Vector3 targetPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * enemyFrogBoss.tongueScatterRadius);
      if (i == randomTargetPlayerNumber)
        targetPosition = PlayerFarming.Instance.transform.position;
      enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.GetTongue().SpitTongueIE(targetPosition, enemyFrogBoss.tongueScatterWhipDuration, enemyFrogBoss.tongueWhipDuration, (float) ((double) enemyFrogBoss.tongueRetrieveDelay - (double) delay + 0.5), enemyFrogBoss.tongueRetrieveDuration));
      if (i != amount - 1)
      {
        float seconds = UnityEngine.Random.Range(enemyFrogBoss.tongueScatterDelay.x, enemyFrogBoss.tongueScatterDelay.y);
        delay += seconds;
        yield return (object) new WaitForSeconds(seconds);
      }
    }
    yield return (object) new WaitForSeconds(enemyFrogBoss.tongueScatterPreAnticipation);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.tongueAttackAnimation, false);
    yield return (object) new WaitForSeconds((float) ((double) enemyFrogBoss.tongueWhipDuration + (double) enemyFrogBoss.tongueRetrieveDelay + 0.40000000596046448));
    yield return (object) new WaitForSeconds(enemyFrogBoss.tongueScatterPostAnticipation);
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.tongueEndAnimation, false);
    yield return (object) new WaitForSeconds(1f);
    enemyFrogBoss.attacking = false;
    enemyFrogBoss.facePlayer = true;
    enemyFrogBoss.usingTongue = false;
  }

  private FrogBossTongue GetTongue()
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

  private void SpawnMiniBosses() => this.StartCoroutine((IEnumerator) this.SpawnMiniBossesIE());

  private IEnumerator SpawnMiniBossesIE()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.facePlayer = false;
    enemyFrogBoss.Spine.AnimationState.SetAnimation(0, enemyFrogBoss.burpAnimation, false);
    enemyFrogBoss.Spine.AnimationState.AddAnimation(0, enemyFrogBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyFrogBoss.miniBossSpawningDelay);
    for (int index = 0; index < enemyFrogBoss.miniBossFrogsSpawnAmount; ++index)
    {
      // ISSUE: reference to a compiler-generated method
      Addressables.InstantiateAsync((object) enemyFrogBoss.miniBossFrogTarget, enemyFrogBoss.miniBossSpawnPositions[index], Quaternion.identity, enemyFrogBoss.transform.parent).Completed += new System.Action<AsyncOperationHandle<GameObject>>(enemyFrogBoss.\u003CSpawnMiniBossesIE\u003Eb__156_0);
    }
    enemyFrogBoss.facePlayer = true;
  }

  private void MiniBossKilled(
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

  private IEnumerator AllMiniBossesKilled()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    yield return (object) enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.HopDownIE());
    yield return (object) new WaitForSeconds(1.5f);
    enemyFrogBoss.StartCoroutine((IEnumerator) enemyFrogBoss.Phase3IE(false));
  }

  private void AnimationEvent(TrackEntry trackEntry, Spine.Event e)
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
    PlayerFarming.Instance.health.invincible = true;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_BOSS_2"));
    this.Spine.transform.localPosition = Vector3.zero;
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.damageColliderEvents.gameObject.SetActive(false);
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
    GameManager.GetInstance().CamFollowTarget.MinZoom = 6f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 20f;
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
    yield return (object) new WaitForSeconds(this.enragedDuration);
    CameraManager.instance.ShakeCameraForDuration(3.5f, 4f, 1f);
    yield return (object) new WaitForSeconds(1.5f);
    this.facePlayer = true;
  }

  private float GetRandomAngle()
  {
    float randomAngle = (float) UnityEngine.Random.Range(0, 360);
    float num = 32f;
    for (int index = 0; (double) index < (double) num; ++index)
    {
      Vector3 normalized = (Vector3) new Vector2(Mathf.Cos(randomAngle * ((float) Math.PI / 180f)), Mathf.Sin(randomAngle * ((float) Math.PI / 180f))).normalized;
      if ((bool) Physics2D.Raycast((Vector2) (this.transform.position - normalized), (Vector2) normalized, this.physicsCollider.radius * 3f, (int) this.layerToCheck))
        randomAngle = Mathf.Repeat(randomAngle + (float) (360.0 / ((double) num + 1.0)), 360f);
      else
        break;
    }
    return randomAngle;
  }

  private float GetAngleToTarget()
  {
    float angleToTarget = (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position) : 0.0f;
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return angleToTarget;
    float num = 32f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(angleToTarget * ((float) Math.PI / 180f)), Mathf.Sin(angleToTarget * ((float) Math.PI / 180f))), 2.5f, (int) this.layerToCheck); ++index)
      angleToTarget = Mathf.Repeat(angleToTarget + (float) (360.0 / ((double) num + 1.0)), 360f);
    return angleToTarget;
  }

  private void LookAt(float angle)
  {
    this.state.LookAngle = angle;
    this.state.facingAngle = angle;
  }

  private void OnTriggerEnterEvent(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Moving || collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    Debug.Log((object) "I have hopped into an obstacle", (UnityEngine.Object) this.gameObject);
    this.hasCollidedWithObstacle = true;
    this.targetAngle += (double) this.targetAngle > 180.0 ? -180f : 180f;
    this.LookAt(this.targetAngle);
  }

  private IEnumerator TurnOnDamageColliderForDuration(GameObject collider, float duration)
  {
    collider.SetActive(true);
    yield return (object) new WaitForSeconds(duration);
    collider.SetActive(false);
  }

  private void SpawnEnemy()
  {
    if (this.enemiesAlive >= this.maxEnemies)
      return;
    ++this.enemiesAlive;
    Addressables.InstantiateAsync((object) this.enemyHopperTarget, this.spawnPositions[UnityEngine.Random.Range(0, this.spawnPositions.Length)], Quaternion.identity, this.transform.parent).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      UnitObject component1 = obj.Result.GetComponent<UnitObject>();
      component1.GetComponent<Health>().OnDie += new Health.DieAction(this.Enemy_OnDie);
      this.spawnedEnemies.Add(component1);
      DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
      if (!(bool) (UnityEngine.Object) component2)
        return;
      component2.GiveXP = false;
    });
    this.spawnTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.spawnDelay.x, this.spawnDelay.y);
  }

  private void Enemy_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.enemiesAlive;
  }

  private IEnumerator Die()
  {
    EnemyFrogBoss enemyFrogBoss = this;
    enemyFrogBoss.ClearPaths();
    enemyFrogBoss.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew();
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
    if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
    {
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, "die", false);
      enemyFrogBoss.Spine.AnimationState.AddAnimation(0, "dead", true, 0.0f);
    }
    else
    {
      enemyFrogBoss.Spine.AnimationState.SetAnimation(0, "die-noheart", false);
      enemyFrogBoss.Spine.AnimationState.AddAnimation(0, "dead-noheart", true, 0.0f);
    }
    yield return (object) new WaitForSeconds(3.2f);
    GameManager.GetInstance().OnConversationEnd();
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2))
      enemyFrogBoss.interaction_MonsterHeart.ObjectiveToComplete = Objectives.CustomQuestTypes.BishopsOfTheOldFaith2;
    enemyFrogBoss.interaction_MonsterHeart.Play();
  }

  private void OnDrawGizmosSelected()
  {
    foreach (Vector3 spawnPosition in this.spawnPositions)
      Utils.DrawCircleXY(spawnPosition, 0.5f, Color.blue);
    foreach (Vector3 bossSpawnPosition in this.miniBossSpawnPositions)
      Utils.DrawCircleXY(bossSpawnPosition, 0.5f, Color.green);
  }
}
