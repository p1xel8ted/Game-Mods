// Decompiled with JetBrains decompiler
// Type: EnemyDogMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyDogMiniboss : UnitObject
{
  public EnemyDogMiniboss.BossPhase currentPhase;
  public SimpleSpineFlash SimpleSpineFlash;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BurrowAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BurrowDiveIntoGroundAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string BurrowHiddenAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StunnedAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SpawnEnemiesAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string VomitAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string VomitAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ChargeAttackSignPostAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ChargeAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ChargeAttackWallCollisionAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DiveBombAnticipation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DiveBombJumpAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DiveBombLandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DiveBombRecoveryAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string HowlAnimation;
  public GameObject Lighting;
  public SpriteRenderer ShadowSpriteRenderer;
  public GameObject TrailPrefab;
  public List<GameObject> Trails = new List<GameObject>();
  public float DelayBetweenTrails = 0.2f;
  public float TrailsTimer;
  public Collider2D UnitCollider;
  public bool repositionOnHit = true;
  public Vector2 delaysBetweenAttacks;
  public Vector2 delaysBetweenMoving;
  public AssetReferenceGameObject IndicatorPrefabLarge;
  public float VomitAttackDuration = 3f;
  public float VomitVoleyInterval = 0.1f;
  public float ShotsPerVoley = 5f;
  public float VomitAttackCooldown = 1f;
  public Transform VomitStartTransform;
  public float ChargeAttackCooldown = 3f;
  public float ChargeAttackPauseBeforeReposition = 0.5f;
  public float ChargeAttackRepositionTime = 0.5f;
  public float ChargeAttackRepositionHeight = 1.5f;
  public float ChargeAttackSpeed = 3f;
  public float IcicleSpawnRadius = 8f;
  public int IcicleCount = 15;
  public float IcicleDropDelayMin = 2f;
  public float IcicleDropDelayMax = 4f;
  public float IcicleFallSpeed = 15f;
  public AssetReferenceGameObject IciclePrefab;
  public float DiveBombDuration = 2f;
  public float DiveBombArcHeight = 4f;
  public float DiveBombLandingCooldown = 2f;
  public ParticleSystem DiveBombImpactParticles;
  [SerializeField]
  public Vector2 spawnAmount;
  [SerializeField]
  public int maxActiveSpawns;
  public float SpawnEnemiesAnticipation;
  public float SpawnEnemiesCooldown;
  [SerializeField]
  public Vector2 delayBetweenSpawns;
  [SerializeField]
  public float spawnForce;
  [SerializeField]
  public Vector2 timeBetweenSpawns;
  [SerializeField]
  public AssetReferenceGameObject[] spawnables;
  public float BurrowMoveSpeed = 1f;
  public float BurrowUndergroundMinTime = 2f;
  public float BurrowUndergroundMaxTime = 4f;
  public AnimationCurve BurrowSpeedCurve;
  public AnimationCurve BurrowDiveCurve;
  public float BurrowDiveHeight = 3f;
  public float BurrowDiveDuration = 0.4f;
  public float DelayBeforeBurrowEmerge = 0.3f;
  public float BurrowEmergeHeight = 3f;
  public float BurrowEmergeDuration = 0.4f;
  public float BurrowStunnedDuration = 3f;
  public float BurrowRecoveryTime = 1f;
  public ParticleSystem SnowParticles;
  public float HowlWarmupTime = 1f;
  public int HowlRepeatCount = 3;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/miniboss_dog/death_vo";
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string ShootSFX = "event:/enemy/shoot_magicenergy";
  [EventRef]
  public string IcicleRumbleSfx = string.Empty;
  [EventRef]
  public string WallImpactSfx = string.Empty;
  [EventRef]
  public string AttackBurrowStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/attack_burrow_start";
  [EventRef]
  public string AttackBurrowLoopSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/attack_burrow_loop";
  [EventRef]
  public string AttackBurrowBurstSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/attack_burrow_burst";
  [EventRef]
  public string AttackJumpWarningVO = "event:/dlc/dungeon05/enemy/miniboss_dog/attack_jump_warning_vo";
  [EventRef]
  public string AttackJumpLandSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/attack_jump_land";
  [EventRef]
  public string AttackVomitBombStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/attack_vomit_bomb_start";
  [EventRef]
  public string AttackVomitBombLaunchSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/attack_vomit_bomb_launch";
  [EventRef]
  public string AttackVomitEnemyStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/attack_vomit_enemy_start";
  public EventInstance attackJumpWarningInstanceSFX;
  public EventInstance attackBurrowInstanceSFX;
  public EventInstance vomitBombStartInstanceSFX;
  public EventInstance vomitEnemyInstanceSFX;
  public GameObject GrenadePrefab;
  public Vector2 DelayBetweenShots = new Vector2(0.1f, 0.3f);
  public float NumberOfShotsToFire = 5f;
  public float GravSpeed = -15f;
  public Vector2 RandomArcOffset = new Vector2(0.0f, 0.0f);
  public Vector2 ShootDistanceRange = new Vector2(2f, 3f);
  public Vector3 ShootOffset;
  public float BombSpeed;
  public MortarBomb bombPrefab;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  public float CircleCastRadius = 0.5f;
  public ColliderEvents damageColliderEvents;
  public GameObject trailObject;
  public Projectile projectileBullet;
  public GrenadeBullet projectileGrenade;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
  public AsyncOperationHandle<GameObject> loadedIcicleAsset;
  public GameObject loadedSlamIndicator;
  public GameObject currentSlamIndicator;
  public ShowHPBar ShowHPBar;
  public GameObject targetObject;
  public Coroutine currentChargeAttack;
  public Coroutine currentRoutine;
  public Coroutine currentAttack;
  public Vector3 previousSpawnPosition;
  public bool active;
  public bool ShootingSpikeCircle;
  public float diveBombColliderDuration = 0.2f;
  public float howlScreenshakeDelay = 0.9f;
  public float burrowUndergroundAttackDistance = 0.5f;
  public float pauseBeforeNextPhase = 1f;
  public Vector3 diveBombTargetPosition;
  public bool inFinalAttack;

  public IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyDogMiniboss enemyDogMiniboss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyDogMiniboss.Preload();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyDogMiniboss.active = true;
    enemyDogMiniboss.OnEnable();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.ShowHPBar = this.GetComponent<ShowHPBar>();
    if (this.active)
    {
      if (this.currentRoutine != null)
        this.StopCoroutine(this.currentRoutine);
      this.StartCoroutine((IEnumerator) this.RunBehaviour());
    }
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.active = true;
    this.health.OnDieEarly += new Health.DieAction(this.UnlockSkin);
    RespawnRoomManager.OnRespawnRoomShown += new System.Action(this.OnRespawnRoomShown);
  }

  public void StopEventInstances()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.attackJumpWarningInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.vomitBombStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.vomitEnemyInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopLoop(this.attackBurrowInstanceSFX);
  }

  public void UnlockSkin(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (DataManager.GetFollowerSkinUnlocked("Boss Dog 1"))
      return;
    DataManager.SetFollowerSkinUnlocked("Boss Dog 1");
  }

  public void GiveSkin(GameObject Attacker)
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    FollowerSkinCustomTarget.Create(this.Spine.transform.position, Attacker.transform.position, 2f, "Boss Dog 1", (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().AddPlayerToCamera();
    }));
  }

  public IEnumerator RunBehaviour()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    while (true)
    {
      float num = UnityEngine.Random.value;
      if (enemyDogMiniboss.spawnedEnemies.Count >= enemyDogMiniboss.maxActiveSpawns)
        num += 0.25f;
      enemyDogMiniboss.ClearPaths();
      if ((double) num < 0.25)
        yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoSpawnEnemies(EnemyDogMiniboss.SpawnableType.Burrow, EnemyDogMiniboss.SpawnableType.Charger, EnemyDogMiniboss.SpawnableType.DiveBomb));
      else if ((double) num < 0.5)
        yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoDiveBombAttack(UnityEngine.Random.Range(3, 6)));
      else if ((double) num < 0.75)
        yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoVomitAttack());
      else
        yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoBurrowAttack());
      yield return (object) new WaitForSeconds(1f);
      float timeBetweenAttacks = UnityEngine.Random.Range(enemyDogMiniboss.delaysBetweenAttacks.x, enemyDogMiniboss.delaysBetweenAttacks.y);
      float time = 0.0f;
      float changePath = 0.0f;
      while ((double) (time += Time.deltaTime) < (double) timeBetweenAttacks)
      {
        if ((double) Time.time > (double) changePath)
        {
          changePath = Time.time + UnityEngine.Random.Range(enemyDogMiniboss.delaysBetweenMoving.x, enemyDogMiniboss.delaysBetweenMoving.y);
          Vector3 vector3 = (Vector3) (UnityEngine.Random.insideUnitCircle * 7f);
          enemyDogMiniboss.givePath(vector3);
          enemyDogMiniboss.state.LookAngle = Utils.GetAngle(enemyDogMiniboss.transform.position, vector3);
          enemyDogMiniboss.state.facingAngle = enemyDogMiniboss.state.LookAngle;
        }
        if (enemyDogMiniboss.state.CURRENT_STATE == StateMachine.State.Moving && enemyDogMiniboss.Spine.AnimationName != enemyDogMiniboss.MovingAnimation)
          enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.MovingAnimation, true);
        if (enemyDogMiniboss.state.CURRENT_STATE == StateMachine.State.Idle && enemyDogMiniboss.Spine.AnimationName != enemyDogMiniboss.IdleAnimation)
          enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.IdleAnimation, true);
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public void SpawnEnemies()
  {
    this.StartCoroutine((IEnumerator) this.DoSpawnEnemies(EnemyDogMiniboss.SpawnableType.Burrow, EnemyDogMiniboss.SpawnableType.Charger, EnemyDogMiniboss.SpawnableType.DiveBomb));
  }

  public void BurrowAttack() => this.StartCoroutine((IEnumerator) this.DoBurrowAttack());

  public IEnumerator DoChargeAttack(int totalAttacks = 1)
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.ClearPaths();
    int attackCount = 0;
    while (attackCount < totalAttacks)
    {
      ++attackCount;
      enemyDogMiniboss.LookAtClosestPlayer();
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoChargeAttackSignPost());
      enemyDogMiniboss.LookAtClosestPlayer();
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoChargeAttackMovement());
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoChargeAttackWallCollision());
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoChargeAttackReposition());
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoChargeAttackCooldown());
    }
  }

  public IEnumerator DoVomitAttack()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoVomitAnticipation());
    yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoVomitBombs());
    yield return (object) new WaitForSeconds(enemyDogMiniboss.VomitAttackCooldown);
  }

  public IEnumerator DoVomitAnticipation()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.LookAtClosestPlayer();
    float seconds = enemyDogMiniboss.Spine.Skeleton.Data.FindAnimation(enemyDogMiniboss.VomitAnticipationAnimation).Duration * enemyDogMiniboss.Spine.timeScale;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.VomitAnticipationAnimation, false);
    if (!string.IsNullOrEmpty(enemyDogMiniboss.AttackVomitBombStartSFX))
      enemyDogMiniboss.vomitBombStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyDogMiniboss.AttackVomitBombStartSFX, enemyDogMiniboss.transform);
    yield return (object) new WaitForSeconds(seconds);
  }

  public IEnumerator DoVomitBombs()
  {
    float shootTimer = 0.0f;
    float progress = 0.0f;
    this.Spine.AnimationState.SetAnimation(0, this.VomitAttackAnimation, false);
    this.LookAtClosestPlayer();
    while ((double) (progress += Time.deltaTime * this.Spine.timeScale) < (double) this.VomitAttackDuration)
    {
      if ((double) shootTimer <= 0.0)
      {
        shootTimer = this.VomitVoleyInterval;
        this.LerpFacingToCloserPlayer(progress / this.VomitAttackDuration);
        this.DoVomitShootVolley();
      }
      shootTimer -= Time.deltaTime * this.Spine.timeScale;
      yield return (object) null;
    }
    this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
  }

  public IEnumerator DoChargeAttackSignPost()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.ChargeAttackSignPostAnimation, false);
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float progress = 0.0f;
    float animationDuration = enemyDogMiniboss.Spine.Skeleton.Data.FindAnimation(enemyDogMiniboss.ChargeAttackSignPostAnimation).Duration * enemyDogMiniboss.Spine.timeScale;
    while ((double) (progress += Time.deltaTime) < (double) animationDuration / (double) enemyDogMiniboss.Spine.timeScale)
    {
      enemyDogMiniboss.SimpleSpineFlash.FlashWhite(progress / animationDuration);
      yield return (object) null;
    }
    enemyDogMiniboss.SimpleSpineFlash.FlashWhite(false);
  }

  public IEnumerator DoChargeAttackMovement()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.damageColliderEvents?.SetActive(true);
    enemyDogMiniboss.UsePathing = false;
    enemyDogMiniboss.speed = enemyDogMiniboss.ChargeAttackSpeed;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.ChargeAttackAnimation, true);
    Vector3 normalized = new Vector3(Mathf.Cos(enemyDogMiniboss.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(enemyDogMiniboss.state.facingAngle * ((float) Math.PI / 180f)), 0.0f).normalized;
    Vector3 position = enemyDogMiniboss.transform.position;
    Vector3 endValue = Vector3.zero;
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) enemyDogMiniboss.transform.position, (Vector2) normalized, float.MaxValue, (int) enemyDogMiniboss.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      endValue = (Vector3) raycastHit2D.point + -normalized * enemyDogMiniboss.UnitCollider.bounds.extents.x;
      Debug.Log((object) ("Hit object: " + raycastHit2D.collider.gameObject.name));
    }
    Vector3 b = endValue;
    float num = Vector3.Distance(position, b) / enemyDogMiniboss.speed;
    if (!string.IsNullOrEmpty(enemyDogMiniboss.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyDogMiniboss.AttackVO, enemyDogMiniboss.transform.position);
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.Charging;
    enemyDogMiniboss.transform.DOMove(endValue, num * enemyDogMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad);
    yield return (object) new WaitForSeconds(num * enemyDogMiniboss.Spine.timeScale);
  }

  public IEnumerator DoChargeAttackWallCollision()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyDogMiniboss enemyDogMiniboss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyDogMiniboss.speed = 0.0f;
      enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.IdleAnimation, true);
      enemyDogMiniboss.targetObject = (GameObject) null;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 1f);
    AudioManager.Instance.PlayOneShot(enemyDogMiniboss.GetHitVO, enemyDogMiniboss.transform.position);
    AudioManager.Instance.PlayOneShot(enemyDogMiniboss.WallImpactSfx, enemyDogMiniboss.transform.position);
    enemyDogMiniboss.ClearPaths();
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.ChargeAttackWallCollisionAnimation, false);
    AudioManager.Instance.PlayOneShot(enemyDogMiniboss.IcicleRumbleSfx, Vector3.zero);
    enemyDogMiniboss.SpawnIcicles();
    enemyDogMiniboss.damageColliderEvents.gameObject.SetActive(false);
    MMVibrate.RumbleContinuous(0.5f, 0.75f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) enemyDogMiniboss.Spine.YieldForAnimation(enemyDogMiniboss.ChargeAttackWallCollisionAnimation);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoChargeAttackReposition()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    float halfMoveTime = enemyDogMiniboss.ChargeAttackRepositionTime * 0.5f;
    yield return (object) new WaitForSeconds(enemyDogMiniboss.ChargeAttackPauseBeforeReposition * enemyDogMiniboss.Spine.timeScale);
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.DiveBombJumpAnimation, false);
    enemyDogMiniboss.transform.DOKill();
    enemyDogMiniboss.transform.DOMoveZ(-enemyDogMiniboss.ChargeAttackRepositionHeight, halfMoveTime * enemyDogMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic);
    yield return (object) new WaitForSeconds(halfMoveTime * enemyDogMiniboss.Spine.timeScale);
    enemyDogMiniboss.LookAtClosestPlayer();
    enemyDogMiniboss.transform.DOKill();
    enemyDogMiniboss.transform.DOMoveZ(0.0f, halfMoveTime * enemyDogMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCubic);
    yield return (object) new WaitForSeconds(halfMoveTime * enemyDogMiniboss.Spine.timeScale);
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.DiveBombLandAnimation, false);
    yield return (object) new WaitForSeconds(enemyDogMiniboss.Spine.Skeleton.Data.FindAnimation(enemyDogMiniboss.DiveBombLandAnimation).Duration * enemyDogMiniboss.Spine.timeScale);
  }

  public void SpawnIcicles()
  {
    for (int index = 0; index < this.IcicleCount; ++index)
      ObjectPool.Spawn(this.loadedIcicleAsset.Result, this.transform.parent, (Vector3) (UnityEngine.Random.insideUnitCircle * this.IcicleSpawnRadius), Quaternion.identity).GetComponent<IcicleControl>()?.Drop(UnityEngine.Random.Range(this.IcicleDropDelayMin, this.IcicleDropDelayMax), this.IcicleFallSpeed);
  }

  public Vector3 LookAtClosestPlayer()
  {
    Health closestTarget = this.GetClosestTarget(true);
    Vector3 toPosition = (UnityEngine.Object) closestTarget != (UnityEngine.Object) null ? closestTarget.transform.position : Vector3.zero;
    this.state.LookAngle = Utils.GetAngle(this.transform.position, toPosition);
    this.state.facingAngle = this.state.LookAngle;
    return toPosition;
  }

  public void LerpFacingToCloserPlayer(float progress)
  {
    Health closestTarget = this.GetClosestTarget(true);
    this.state.LookAngle = Mathf.Lerp(this.state.LookAngle, Utils.GetAngle(this.transform.position, (UnityEngine.Object) closestTarget != (UnityEngine.Object) null ? closestTarget.transform.position : Vector3.zero), progress);
    this.state.facingAngle = this.state.LookAngle;
  }

  public IEnumerator DoChargeAttackCooldown()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyDogMiniboss enemyDogMiniboss = this;
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
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.IdleAnimation, true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyDogMiniboss.ChargeAttackCooldown * enemyDogMiniboss.Spine.timeScale);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoSpawnEnemies(
    params EnemyDogMiniboss.SpawnableType[] spawnableType)
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.LookAtClosestPlayer();
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.SpawnEnemiesAnimation, false);
    enemyDogMiniboss.Spine.AnimationState.AddAnimation(0, enemyDogMiniboss.IdleAnimation, true, 0.0f);
    List<AsyncOperationHandle<GameObject>> enemyTypesToSpawn = new List<AsyncOperationHandle<GameObject>>();
    foreach (EnemyDogMiniboss.SpawnableType index in spawnableType)
      enemyTypesToSpawn.Add(enemyDogMiniboss.loadedAddressableAssets[(int) index]);
    if (!string.IsNullOrEmpty(enemyDogMiniboss.AttackVomitEnemyStartSFX))
      enemyDogMiniboss.vomitEnemyInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyDogMiniboss.AttackVomitEnemyStartSFX, enemyDogMiniboss.transform);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) enemyDogMiniboss.SpawnEnemiesAnticipation)
      yield return (object) null;
    int amount = (int) UnityEngine.Random.Range(enemyDogMiniboss.spawnAmount.x, enemyDogMiniboss.spawnAmount.y + 1f);
    for (int i = 0; i < amount; ++i)
    {
      UnitObject component = ObjectPool.Spawn(enemyTypesToSpawn[UnityEngine.Random.Range(0, enemyTypesToSpawn.Count)].Result, enemyDogMiniboss.transform.parent, enemyDogMiniboss.VomitStartTransform.position, Quaternion.identity).GetComponent<UnitObject>();
      float angle = Utils.GetAngle(enemyDogMiniboss.transform.position, enemyDogMiniboss.VomitStartTransform.position) + UnityEngine.Random.Range(-2f, 2f);
      enemyDogMiniboss.spawnedEnemies.Add(component);
      component.health.OnDie += new Health.DieAction(enemyDogMiniboss.SpawnedEnemyKilled);
      component.CanHaveModifier = false;
      component.RemoveModifier();
      component.DoKnockBack(angle, enemyDogMiniboss.spawnForce, 0.75f);
      float dur = UnityEngine.Random.Range(enemyDogMiniboss.delayBetweenSpawns.x, enemyDogMiniboss.delayBetweenSpawns.y);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) enemyDogMiniboss.SpawnEnemiesCooldown)
      yield return (object) null;
  }

  public void SpawnedEnemyKilled(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null && (UnityEngine.Object) this.spawnedEnemies[index].health == (UnityEngine.Object) Victim)
      {
        this.spawnedEnemies[index].health.OnDie -= new Health.DieAction(this.SpawnedEnemyKilled);
        this.spawnedEnemies.RemoveAt(index);
      }
    }
  }

  public IEnumerator DoHowlLoop(int count, bool spawnIcicles = false)
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.transform.DOKill();
    enemyDogMiniboss.transform.position = new Vector3(enemyDogMiniboss.transform.position.x, enemyDogMiniboss.transform.position.y, 0.0f);
    for (int i = 0; i < count; ++i)
    {
      float progress = 0.0f;
      bool hasShaken = false;
      enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.IdleAnimation, true);
      float howlAnimDuration = enemyDogMiniboss.Spine.Skeleton.Data.FindAnimation(enemyDogMiniboss.ChargeAttackSignPostAnimation).Duration;
      yield return (object) new WaitForSeconds(enemyDogMiniboss.HowlWarmupTime * enemyDogMiniboss.Spine.timeScale);
      if (spawnIcicles)
        enemyDogMiniboss.SpawnIcicles();
      enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.HowlAnimation, false);
      while ((double) (progress += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) howlAnimDuration)
      {
        if (!hasShaken && (double) progress > (double) enemyDogMiniboss.howlScreenshakeDelay)
        {
          hasShaken = true;
          CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 1f);
        }
        yield return (object) null;
      }
    }
  }

  public void BombAttack() => this.StartCoroutine((IEnumerator) this.DoVomitAttack());

  public IEnumerator DoDiveBombAttack(int totalAttacks)
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    for (int i = 0; i < totalAttacks; ++i)
    {
      enemyDogMiniboss.inFinalAttack = i >= totalAttacks - 1;
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoSignpostDiveBombAttack());
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoDiveBombMovement());
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoDiveBombLanding());
    }
    enemyDogMiniboss.inFinalAttack = false;
  }

  public IEnumerator DoSignpostDiveBombAttack()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.DiveBombAnticipation, false);
    float anticipationDuration = enemyDogMiniboss.Spine.Skeleton.Data.FindAnimation(enemyDogMiniboss.DiveBombAnticipation).Duration * enemyDogMiniboss.Spine.timeScale;
    enemyDogMiniboss.diveBombTargetPosition = enemyDogMiniboss.LookAtClosestPlayer();
    if (!string.IsNullOrEmpty(enemyDogMiniboss.AttackJumpWarningVO))
      enemyDogMiniboss.attackJumpWarningInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyDogMiniboss.AttackJumpWarningVO, enemyDogMiniboss.transform);
    if ((UnityEngine.Object) enemyDogMiniboss.loadedSlamIndicator != (UnityEngine.Object) null)
    {
      enemyDogMiniboss.currentSlamIndicator = ObjectPool.Spawn(enemyDogMiniboss.loadedSlamIndicator);
      enemyDogMiniboss.currentSlamIndicator.transform.position = new Vector3(enemyDogMiniboss.diveBombTargetPosition.x, enemyDogMiniboss.diveBombTargetPosition.y, 0.0f);
      enemyDogMiniboss.currentSlamIndicator.SetActive(true);
    }
    float signpostTime = 0.0f;
    while ((double) enemyDogMiniboss.Spine.timeScale == 9.9999997473787516E-05)
      yield return (object) null;
    while ((double) (signpostTime += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) anticipationDuration)
    {
      if (enemyDogMiniboss.inFinalAttack)
      {
        float amt = (float) ((double) signpostTime / (double) anticipationDuration * 0.25);
        enemyDogMiniboss.SimpleSpineFlash.FlashRed(amt);
        CameraManager.shakeCamera(amt * 3f);
      }
      else
        enemyDogMiniboss.SimpleSpineFlash.FlashWhite(signpostTime / anticipationDuration);
      yield return (object) null;
    }
    if (enemyDogMiniboss.inFinalAttack)
      enemyDogMiniboss.SimpleSpineFlash.FlashRed(0.25f);
    else
      enemyDogMiniboss.SimpleSpineFlash.FlashWhite(false);
  }

  public IEnumerator DoDiveBombMovement()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_jump", enemyDogMiniboss.transform.position);
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.DiveBombJumpAnimation, false);
    Vector3 startPosition = enemyDogMiniboss.transform.position;
    float progress = 0.0f;
    Vector3 curve = startPosition + (enemyDogMiniboss.diveBombTargetPosition - startPosition) / 2f + Vector3.back * enemyDogMiniboss.DiveBombArcHeight;
    enemyDogMiniboss.LockToGround = false;
    while ((double) (progress += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) enemyDogMiniboss.DiveBombDuration)
    {
      Vector3 a = Vector3.Lerp(startPosition, curve, progress / enemyDogMiniboss.DiveBombDuration);
      Vector3 b = Vector3.Lerp(curve, enemyDogMiniboss.diveBombTargetPosition, progress / enemyDogMiniboss.DiveBombDuration);
      enemyDogMiniboss.transform.position = Vector3.Lerp(a, b, progress / enemyDogMiniboss.DiveBombDuration);
      yield return (object) null;
    }
    enemyDogMiniboss.diveBombTargetPosition.z = 0.0f;
    enemyDogMiniboss.transform.position = enemyDogMiniboss.diveBombTargetPosition;
  }

  public IEnumerator DoDiveBombLanding()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.DiveBombLandAnimation, false);
    enemyDogMiniboss.Spine.AnimationState.AddAnimation(0, enemyDogMiniboss.DiveBombRecoveryAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyDogMiniboss.AttackJumpLandSFX))
      AudioManager.Instance.PlayOneShot(enemyDogMiniboss.AttackJumpLandSFX, enemyDogMiniboss.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    if ((UnityEngine.Object) enemyDogMiniboss.currentSlamIndicator != (UnityEngine.Object) null)
      ObjectPool.Recycle(enemyDogMiniboss.currentSlamIndicator);
    enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.TurnOnDamageColliderForDuration(enemyDogMiniboss.diveBombColliderDuration));
    BiomeConstants.Instance.EmitSmokeExplosionVFX(enemyDogMiniboss.transform.position + Vector3.back * 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    if ((bool) (UnityEngine.Object) enemyDogMiniboss.DiveBombImpactParticles)
      enemyDogMiniboss.DiveBombImpactParticles.Play();
    if (enemyDogMiniboss.inFinalAttack)
      CameraManager.shakeCamera(25f);
    float cooldownProgress = 0.0f;
    while ((double) (cooldownProgress += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) enemyDogMiniboss.DiveBombLandingCooldown)
      yield return (object) null;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.DiveBombRecoveryAnimation, true);
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive((double) this.Spine.timeScale > 9.9999997473787516E-05);
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      this.SimpleSpineFlash.FlashWhite(1f - Mathf.Clamp01(t / duration));
      yield return (object) null;
    }
    this.SimpleSpineFlash.FlashWhite(false);
    this.damageColliderEvents.SetActive(false);
  }

  public IEnumerator DoBurrowAttack()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    if (enemyDogMiniboss.state.CURRENT_STATE != StateMachine.State.Burrowing)
      yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoBurrowDiveIntoGround());
    yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoBurrowChase());
    yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoBurrowPauseBeforeEmerge());
    yield return (object) enemyDogMiniboss.StartCoroutine((IEnumerator) enemyDogMiniboss.DoBurrowEmergeAttack());
  }

  public IEnumerator DoBurrowDiveIntoGround()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.Diving;
    if (!string.IsNullOrEmpty(enemyDogMiniboss.AttackBurrowStartSFX))
      AudioManager.Instance.PlayOneShot(enemyDogMiniboss.AttackBurrowStartSFX, enemyDogMiniboss.gameObject);
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.BurrowDiveIntoGroundAnimation, false);
    enemyDogMiniboss.Spine.AnimationState.AddAnimation(0, enemyDogMiniboss.BurrowHiddenAnimation, true, 0.0f);
    float progress = 0.0f;
    while ((double) (progress += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) enemyDogMiniboss.BurrowDiveDuration)
    {
      enemyDogMiniboss.speed = enemyDogMiniboss.BurrowSpeedCurve.Evaluate(progress / (enemyDogMiniboss.BurrowDiveDuration * enemyDogMiniboss.Spine.timeScale)) * enemyDogMiniboss.BurrowMoveSpeed;
      enemyDogMiniboss.speed *= enemyDogMiniboss.Spine.timeScale;
      enemyDogMiniboss.transform.position = new Vector3(enemyDogMiniboss.transform.position.x, enemyDogMiniboss.transform.position.y, -enemyDogMiniboss.BurrowDiveCurve.Evaluate(progress / enemyDogMiniboss.BurrowDiveDuration) * enemyDogMiniboss.BurrowDiveHeight * enemyDogMiniboss.Spine.timeScale);
      yield return (object) null;
    }
    if ((double) progress >= (double) enemyDogMiniboss.BurrowDiveDuration / (double) enemyDogMiniboss.Spine.timeScale)
    {
      if (!enemyDogMiniboss.attackBurrowInstanceSFX.isValid())
        enemyDogMiniboss.attackBurrowInstanceSFX = AudioManager.Instance.CreateLoop(enemyDogMiniboss.AttackBurrowLoopSFX, enemyDogMiniboss.gameObject, true);
      enemyDogMiniboss.speed = 0.0f;
    }
  }

  public void DoDiveImpactEffects()
  {
    if ((bool) (UnityEngine.Object) this.Lighting)
      this.Lighting.SetActive(false);
    if ((bool) (UnityEngine.Object) this.ShadowSpriteRenderer)
      this.ShadowSpriteRenderer.enabled = false;
    this.SpawnTrails();
    this.SnowParticles.Play();
  }

  public IEnumerator DoBurrowChase()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.Burrowing;
    enemyDogMiniboss.health.ClearBurn();
    enemyDogMiniboss.health.invincible = true;
    Vector3 position1 = enemyDogMiniboss.GetClosestTarget(true).transform.position;
    float time = 0.0f;
    float progress = 0.0f;
    float duration = 0.366666675f;
    enemyDogMiniboss.transform.position.z = 0.0f;
    position1.z = 0.0f;
    progress = 0.0f;
    MMVibrate.RumbleContinuous(0.5f, 0.75f);
    while ((double) (time += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < 0.25)
      yield return (object) null;
    while ((double) (progress += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) duration)
    {
      enemyDogMiniboss.SpawnTrails();
      yield return (object) null;
    }
    if ((bool) (UnityEngine.Object) enemyDogMiniboss.Lighting)
      enemyDogMiniboss.Lighting.SetActive(false);
    enemyDogMiniboss.ShowHPBar?.Hide();
    enemyDogMiniboss.previousSpawnPosition = Vector3.positiveInfinity;
    while ((double) (progress += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) enemyDogMiniboss.BurrowUndergroundMaxTime)
    {
      Vector3 position2 = enemyDogMiniboss.GetClosestTarget().transform.position;
      if ((double) Vector3.Distance(enemyDogMiniboss.transform.position, position2) >= (double) enemyDogMiniboss.burrowUndergroundAttackDistance || (double) progress <= (double) enemyDogMiniboss.BurrowUndergroundMinTime)
      {
        enemyDogMiniboss.transform.position = Vector3.MoveTowards(enemyDogMiniboss.transform.position, position2, enemyDogMiniboss.BurrowMoveSpeed * Time.deltaTime);
        enemyDogMiniboss.SpawnTrails();
        yield return (object) null;
      }
      else
        break;
    }
    enemyDogMiniboss.health.invincible = false;
    MMVibrate.StopRumble();
    AudioManager.Instance.StopLoop(enemyDogMiniboss.attackBurrowInstanceSFX);
    if ((bool) (UnityEngine.Object) enemyDogMiniboss.Lighting)
      enemyDogMiniboss.Lighting.SetActive(true);
  }

  public IEnumerator DoBurrowPauseBeforeEmerge()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.health.invincible = true;
    float t = 0.0f;
    while ((double) (t += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) enemyDogMiniboss.DelayBeforeBurrowEmerge)
      yield return (object) null;
  }

  public IEnumerator DoBurrowEmergeAttack()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.Flying;
    if (!string.IsNullOrEmpty(enemyDogMiniboss.AttackBurrowBurstSFX))
      AudioManager.Instance.PlayOneShot(enemyDogMiniboss.AttackBurrowBurstSFX, enemyDogMiniboss.gameObject);
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.BurrowAttackAnimation, false);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    if ((bool) (UnityEngine.Object) enemyDogMiniboss.Lighting)
      enemyDogMiniboss.Lighting.SetActive(true);
    if ((bool) (UnityEngine.Object) enemyDogMiniboss.ShadowSpriteRenderer)
      enemyDogMiniboss.ShadowSpriteRenderer.enabled = true;
    enemyDogMiniboss.health.invincible = false;
    enemyDogMiniboss.state.LookAngle = Utils.GetAngle(enemyDogMiniboss.transform.position, enemyDogMiniboss.GetClosestTarget().transform.position);
    enemyDogMiniboss.state.facingAngle = enemyDogMiniboss.state.LookAngle;
    enemyDogMiniboss.transform.DOKill();
    enemyDogMiniboss.transform.DOMoveZ(-enemyDogMiniboss.BurrowEmergeHeight, enemyDogMiniboss.BurrowEmergeDuration * enemyDogMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic);
    yield return (object) new WaitForSeconds(enemyDogMiniboss.BurrowEmergeDuration * enemyDogMiniboss.Spine.timeScale);
    enemyDogMiniboss.transform.DOKill();
    enemyDogMiniboss.transform.DOMoveZ(0.0f, enemyDogMiniboss.BurrowEmergeDuration * enemyDogMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCubic);
    yield return (object) new WaitForSeconds(enemyDogMiniboss.BurrowEmergeDuration * enemyDogMiniboss.Spine.timeScale);
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.IdleAnimation, true);
  }

  public IEnumerator DoBurrowStunned()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    enemyDogMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.StunnedAnimation, true);
    float t = 0.0f;
    while ((double) (t += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) enemyDogMiniboss.BurrowStunnedDuration)
      yield return (object) null;
    enemyDogMiniboss.Spine.AnimationState.SetAnimation(0, enemyDogMiniboss.IdleAnimation, true);
    yield return (object) new WaitForSeconds(enemyDogMiniboss.BurrowRecoveryTime * enemyDogMiniboss.Spine.timeScale);
  }

  public IEnumerator DoBurrowEmergeShoot()
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f, false);
    float randomStartAngle = (float) UnityEngine.Random.Range(0, 360);
    int i = -1;
    while ((double) ++i < (double) enemyDogMiniboss.NumberOfShotsToFire)
    {
      if (!string.IsNullOrEmpty(enemyDogMiniboss.ShootSFX))
        AudioManager.Instance.PlayOneShot(enemyDogMiniboss.ShootSFX, enemyDogMiniboss.transform.position);
      float Angle = randomStartAngle + UnityEngine.Random.Range(enemyDogMiniboss.RandomArcOffset.x, enemyDogMiniboss.RandomArcOffset.y);
      enemyDogMiniboss.projectileGrenade = ObjectPool.Spawn(enemyDogMiniboss.GrenadePrefab, enemyDogMiniboss.transform.position + enemyDogMiniboss.ShootOffset, Quaternion.identity).GetComponent<GrenadeBullet>();
      enemyDogMiniboss.projectileGrenade.SetOwner(enemyDogMiniboss.gameObject);
      enemyDogMiniboss.projectileGrenade.Play(-1f, Angle, UnityEngine.Random.Range(enemyDogMiniboss.ShootDistanceRange.x, enemyDogMiniboss.ShootDistanceRange.y), UnityEngine.Random.Range(enemyDogMiniboss.GravSpeed - 2f, enemyDogMiniboss.GravSpeed + 2f), enemyDogMiniboss.health.team);
      randomStartAngle = Utils.Repeat(randomStartAngle + 360f / enemyDogMiniboss.NumberOfShotsToFire, 360f);
      if (enemyDogMiniboss.DelayBetweenShots != Vector2.zero)
      {
        float time = 0.0f;
        float dur = UnityEngine.Random.Range(enemyDogMiniboss.DelayBetweenShots.x, enemyDogMiniboss.DelayBetweenShots.y);
        while ((double) (time += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < (double) dur)
          yield return (object) null;
      }
    }
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (this.currentChargeAttack == null || (this.layerToCheck.value & 1 << collision.gameObject.layer) <= 0)
      return;
    this.OnDamageTriggerEnter(collision.collider);
    if (!((UnityEngine.Object) this.targetObject != (UnityEngine.Object) null))
      return;
    this.DoKnockBack(this.targetObject, -1f, 1f);
  }

  public void Preload()
  {
    for (int index = 0; index < this.spawnables.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.spawnables[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        obj.Result.CreatePool(24, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
    AsyncOperationHandle<GameObject> asyncOperationHandle1 = Addressables.LoadAssetAsync<GameObject>((object) this.IciclePrefab);
    asyncOperationHandle1.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedIcicleAsset = obj;
      obj.Result.CreatePool(this.IcicleCount, true);
    });
    asyncOperationHandle1.WaitForCompletion();
    AsyncOperationHandle<GameObject> asyncOperationHandle2 = Addressables.LoadAssetAsync<GameObject>((object) this.IndicatorPrefabLarge);
    asyncOperationHandle2.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedAddressableAssets.Add(obj);
      this.loadedSlamIndicator = obj.Result;
      this.loadedSlamIndicator.SetActive(false);
    });
    asyncOperationHandle2.WaitForCompletion();
  }

  public bool IsReadyForNextBossPhase()
  {
    return this.currentPhase == EnemyDogMiniboss.BossPhase.Zero && (double) this.health.HP < (double) this.health.totalHP * 0.5 && this.currentPhase != EnemyDogMiniboss.BossPhase.One;
  }

  public void SpawnTrails()
  {
    if ((double) (this.TrailsTimer += Time.deltaTime) <= (double) this.DelayBetweenTrails)
      return;
    this.TrailsTimer = 0.0f;
    this.trailObject = (GameObject) null;
    if (this.Trails.Count > 0)
    {
      foreach (GameObject trail in this.Trails)
      {
        if (!trail.activeSelf)
        {
          this.trailObject = trail;
          this.trailObject.transform.position = this.transform.position;
          this.trailObject.transform.localScale = new Vector3(this.trailObject.transform.localScale.x * ((double) UnityEngine.Random.value < 0.5 ? -1f : 1f), this.trailObject.transform.localScale.y, this.trailObject.transform.localScale.z);
          this.trailObject.SetActive(true);
          break;
        }
      }
    }
    if ((UnityEngine.Object) this.trailObject == (UnityEngine.Object) null)
    {
      this.trailObject = UnityEngine.Object.Instantiate<GameObject>(this.TrailPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      this.trailObject.transform.localScale = new Vector3(this.trailObject.transform.localScale.x * ((double) UnityEngine.Random.value < 0.5 ? -1f : 1f), this.trailObject.transform.localScale.y, this.trailObject.transform.localScale.z);
      this.Trails.Add(this.trailObject);
      ColliderEvents componentInChildren = this.trailObject.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren)
        componentInChildren.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.previousSpawnPosition = this.trailObject.transform.position;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !this.health.IsCharmedEnemy)
      return;
    float Damage = 1f;
    if (this.inFinalAttack)
      Damage = 2f;
    component.DealDamage(Damage, this.gameObject, component.transform.position);
  }

  public override void OnDestroy()
  {
    foreach (GameObject trail in this.Trails)
    {
      if ((UnityEngine.Object) trail != (UnityEngine.Object) null)
      {
        ColliderEvents componentInChildren = trail.GetComponentInChildren<ColliderEvents>();
        if ((bool) (UnityEngine.Object) componentInChildren)
          componentInChildren.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      }
    }
    base.OnDestroy();
    this.StopEventInstances();
    if (!((UnityEngine.Object) this.currentSlamIndicator != (UnityEngine.Object) null))
      return;
    ObjectPool.Recycle(this.currentSlamIndicator);
  }

  public void DiveBombAttack()
  {
    this.StartCoroutine((IEnumerator) this.DoDiveBombAttack(UnityEngine.Random.Range(3, 6)));
  }

  public void DoVomitShootVolley()
  {
    if (!string.IsNullOrEmpty(this.ShootSFX))
      AudioManager.Instance.PlayOneShot(this.ShootSFX, this.transform.position);
    for (int index = 0; (double) index < (double) this.ShotsPerVoley; ++index)
      this.ShootBomb(this.VomitStartTransform.position, (Vector3) (UnityEngine.Random.insideUnitCircle * 8f));
  }

  public void ShootBomb(Vector3 startPos, Vector3 target)
  {
    MortarBomb mortarBomb = UnityEngine.Object.Instantiate<MortarBomb>(this.bombPrefab, target, Quaternion.identity, this.transform.parent);
    if (!((UnityEngine.Object) mortarBomb != (UnityEngine.Object) null))
      return;
    mortarBomb.gameObject.SetActive(true);
    mortarBomb.transform.position = target;
    float bombSpeed = this.BombSpeed;
    float moveDuration = Vector2.Distance((Vector2) startPos, (Vector2) mortarBomb.transform.position) / bombSpeed;
    mortarBomb.Play(startPos, moveDuration, Health.Team.Team2, PlayDefaultSFX: false, customSFX: this.AttackVomitBombLaunchSFX, doHaptics: true);
  }

  public override void OnDisable()
  {
    this.health.OnDieEarly -= new Health.DieAction(this.UnlockSkin);
    RespawnRoomManager.OnRespawnRoomShown -= new System.Action(this.OnRespawnRoomShown);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
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
    if ((UnityEngine.Object) this.SimpleSpineFlash != (UnityEngine.Object) null)
      this.SimpleSpineFlash.FlashFillRed();
    AudioManager.Instance.StopLoop(this.attackBurrowInstanceSFX);
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyDogMiniboss enemyDogMiniboss = this;
    float f = Utils.GetAngle(Attacker.transform.position, enemyDogMiniboss.transform.position) * ((float) Math.PI / 180f);
    Vector2 force = new Vector2(100f * Mathf.Cos(f), 100f * Mathf.Sin(f));
    enemyDogMiniboss.rb.AddForce(force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMiniboss.Spine.timeScale) < 0.5)
      yield return (object) null;
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
    for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null)
      {
        this.spawnedEnemies[index].health.enabled = true;
        this.spawnedEnemies[index].health.DealDamage(this.spawnedEnemies[index].health.totalHP, this.gameObject, this.spawnedEnemies[index].transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
    this.StopEventInstances();
    if (this.health.DestroyOnDeath)
      return;
    this.StartCoroutine((IEnumerator) this.DelayedDestroy());
    this.gameObject.SetActive(false);
  }

  public void OnRespawnRoomShown()
  {
    if (!((UnityEngine.Object) this.currentSlamIndicator != (UnityEngine.Object) null))
      return;
    ObjectPool.Recycle(this.currentSlamIndicator);
  }

  public IEnumerator DelayedDestroy()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyDogMiniboss enemyDogMiniboss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) enemyDogMiniboss.gameObject);
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

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__169_2(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(24, true);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__169_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedIcicleAsset = obj;
    obj.Result.CreatePool(this.IcicleCount, true);
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__169_1(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    this.loadedSlamIndicator = obj.Result;
    this.loadedSlamIndicator.SetActive(false);
  }

  public enum BossPhase
  {
    Zero,
    One,
  }

  public enum SpawnableType
  {
    Charger,
    Burrow,
    DiveBomb,
  }
}
