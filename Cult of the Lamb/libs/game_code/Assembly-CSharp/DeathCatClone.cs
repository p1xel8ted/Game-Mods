// Decompiled with JetBrains decompiler
// Type: DeathCatClone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using CotL.Projectiles;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class DeathCatClone : UnitObject
{
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string appearAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string disappearAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string transformAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string hurtAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string hurtLoopAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string stopHurtAnimation;
  public SkeletonAnimation GroundCracksSpine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "GroundCracksSpine")]
  public string crackingAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "GroundCracksSpine")]
  public string crackedAnimation;
  public SkeletonAnimation Chain4Spine;
  public SkeletonAnimation Chain5Spine;
  public SkeletonAnimation Chain6Spine;
  public SkeletonAnimation Chain7Spine;
  public SkeletonAnimation Chain8Spine;
  public SkeletonAnimation Chain9Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain4Spine")]
  public string breakAnimation1;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain4Spine")]
  public string breakAnimation2;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain4Spine")]
  public string breakAnimation3;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Chain4Spine")]
  public string brokenAnimation;
  [SerializeField]
  public GameObject Chain5;
  [SerializeField]
  public GameObject Chain7;
  [SerializeField]
  public GameObject Chain9;
  [Space]
  [SerializeField]
  public ColliderEvents distortionObject;
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
  public float projectilePatternCrossDuration;
  [SerializeField]
  public ProjectileCross projectilePatternCross;
  [Space]
  [SerializeField]
  public ProjectilePatternBase swirls;
  [SerializeField]
  public ProjectilePatternBase bouncers;
  [SerializeField]
  public ProjectilePatternBase circles;
  [SerializeField]
  public ProjectilePatternBase circlesFast;
  [SerializeField]
  public ProjectilePatternBase pulse;
  [SerializeField]
  public GameObject trapPrefab;
  public List<TrapProjectileCross> crossTraps;
  [SerializeField]
  public Vector2 distanceRange = new Vector2(1f, 3f);
  [SerializeField]
  public Vector2 idleWaitRange = new Vector2(1f, 3f);
  [SerializeField]
  public float acceleration = 5f;
  [SerializeField]
  public float turningSpeed = 1f;
  [SerializeField]
  public bool manualAttacking;
  public Vector2 timeBetweenAttacks;
  [SerializeField]
  public EnemyDeathCatBoss deathCatBoss;
  [SerializeField]
  public int maxEnemies;
  [SerializeField]
  public Vector2 timeBetweenEnemies;
  [SerializeField]
  public AssetReferenceGameObject[] spawningEnemiesList;
  [SerializeField]
  public bool SpawnEnemies;
  public GameObject targetObject;
  public float randomDirection;
  public LayerMask bounceMask;
  public float attackTimestamp;
  public float spawnEnemyTimestamp;
  public Coroutine currentAttackRoutine;
  [CompilerGenerated]
  public bool \u003CIsFake\u003Ek__BackingField;
  public bool isMoving = true;
  public bool isSpawning = true;
  public int countSinceTeleport = -1;
  public int phase = 1;
  public bool initialised;
  public ProjectileCross projectileCross;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public EventInstance groundCrackSoundInstance;
  public int count;
  public float IdleWait;
  public int DetectEnemyRange = 8;

  public bool IsFake
  {
    get => this.\u003CIsFake\u003Ek__BackingField;
    set => this.\u003CIsFake\u003Ek__BackingField = value;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  public override void Awake()
  {
    base.Awake();
    this.InitializeTraps();
    this.InitializeProjectilePatternRings();
    this.InitializeProjectileCross();
  }

  public void Start()
  {
    this.health.BlackSoulOnHit = true;
    this.timeBetweenAttacks = new Vector2(1f, 2f);
    this.bounceMask = (LayerMask) ((int) this.bounceMask | 1 << LayerMask.NameToLayer("Island"));
    this.bounceMask = (LayerMask) ((int) this.bounceMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.bounceMask = (LayerMask) ((int) this.bounceMask | 1 << LayerMask.NameToLayer("Units"));
    this.attackTimestamp = GameManager.GetInstance().CurrentTime + 1f;
    this.spawnEnemyTimestamp = GameManager.GetInstance().CurrentTime + 0.5f;
    this.Spine.AnimationState.SetAnimation(0, this.shootAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.GetComponent<Health>().invincible = false;
    if (!(bool) (UnityEngine.Object) this.distortionObject)
      return;
    this.distortionObject.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.DamageEnemies);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.initialised)
    {
      this.SeperateObject = true;
      this.randomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      this.state.facingAngle = this.randomDirection * 57.29578f;
      this.StartCoroutine(this.ActiveRoutine());
      this.speed = 0.0f;
      this.initialised = true;
      UIBossHUD.Play(this.health, ScriptLocalization.NAMES.DeathNPC);
      this.currentAttackRoutine = (Coroutine) null;
      AudioManager.Instance.SetMusicRoomID(3, "deathcat_room_id");
    }
    else
      GameManager.GetInstance().StartCoroutine(this.FrameDelay((System.Action) (() =>
      {
        this.currentAttackRoutine = (Coroutine) null;
        AudioManager.Instance.SetMusicRoomID(3, "deathcat_room_id");
        this.StartCoroutine(this.ActiveRoutine());
      })));
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.ClearPaths();
    this.StopAllCoroutines();
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public void Attack(DeathCatClone.AttackType attackType)
  {
    this.GetNewTarget();
    switch (attackType)
    {
      case DeathCatClone.AttackType.Projectiles_Rings:
        this.ShootProjectileRings();
        break;
      case DeathCatClone.AttackType.Projectiles_Swirls:
        this.ShootProjectileSwirls();
        break;
      case DeathCatClone.AttackType.Projectiles_Bouncers:
        this.ShootProjectileBouncers();
        break;
      case DeathCatClone.AttackType.Projectiles_Circles:
        this.ShootProjectileCircles();
        break;
      case DeathCatClone.AttackType.Projectiles_Cross:
        this.ShootProjectileCross();
        break;
      case DeathCatClone.AttackType.Projectiles_Pulse:
        this.ShootProjectilePulse();
        break;
      case DeathCatClone.AttackType.Projectiles_TripleRings:
        this.ShootProjectileRingsTriple();
        break;
      case DeathCatClone.AttackType.Projectiles_CirclesFast:
        this.ShootProjectileCirclesFast();
        break;
      case DeathCatClone.AttackType.Traps_Targeted:
        this.TrapPatternChasePlayer();
        break;
      case DeathCatClone.AttackType.Traps_Pattern0:
        this.TrapPattern0();
        break;
      case DeathCatClone.AttackType.Traps_Pattern1:
        this.TrapPattern1();
        break;
      case DeathCatClone.AttackType.Traps_Pattern2:
        this.TrapPattern2();
        break;
      case DeathCatClone.AttackType.Traps_Scattered:
        this.TrapScattered();
        break;
      case DeathCatClone.AttackType.Teleport_Attack0:
        this.TeleportAttack0();
        break;
      case DeathCatClone.AttackType.Teleport_Attack1:
        this.TeleportAttack1();
        break;
      case DeathCatClone.AttackType.Teleport_Attack2:
        this.TeleportAttack2();
        break;
    }
  }

  public void Reveal() => this.StartCoroutine(this.RevealIE());

  public IEnumerator RevealIE()
  {
    if (this.Spine.AnimationState == null)
      yield return (object) new WaitForEndOfFrame();
    this.Spine.AnimationState.SetAnimation(0, this.appearAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
  }

  public void Hide() => this.StartCoroutine(this.HideIE());

  public IEnumerator HideIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DeathCatClone deathCatClone = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) deathCatClone.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameManager.GetInstance().RemoveFromCamera(deathCatClone.gameObject);
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.disappearAnimation, false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.StopAllCoroutines();
    this.enabled = false;
    GameManager.GetInstance().StartCoroutine(this.DieIE());
    if ((bool) (UnityEngine.Object) this.projectileCross)
      GameManager.GetInstance().StartCoroutine(this.projectileCross.DisableProjectiles());
    PlayerFarming.Instance.playerWeapon.DoSlowMo(false);
    UIBossHUD.Hide();
  }

  public IEnumerator DieIE()
  {
    DeathCatClone deathCatClone = this;
    GameManager.SetGlobalOcclusionActive(false);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(deathCatClone.gameObject, 14f);
    UIBossHUD.Instance.ForceHealthAmount(0.0f);
    deathCatClone.Chain5.SetActive(false);
    deathCatClone.Chain7.SetActive(false);
    deathCatClone.Chain9.SetActive(false);
    deathCatClone.groundCrackSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/deathcat/first_death", deathCatClone.gameObject, true);
    deathCatClone.GroundCracksSpine.AnimationState.SetAnimation(0, deathCatClone.crackingAnimation, false);
    deathCatClone.GroundCracksSpine.AnimationState.AddAnimation(0, deathCatClone.crackedAnimation, true, 0.0f);
    deathCatClone.Chain4Spine.AnimationState.SetAnimation(0, deathCatClone.breakAnimation1, false);
    deathCatClone.Chain6Spine.AnimationState.SetAnimation(0, deathCatClone.breakAnimation2, false);
    deathCatClone.Chain8Spine.AnimationState.SetAnimation(0, deathCatClone.breakAnimation3, false);
    deathCatClone.Chain4Spine.AnimationState.AddAnimation(0, deathCatClone.brokenAnimation, true, 0.0f);
    deathCatClone.Chain6Spine.AnimationState.AddAnimation(0, deathCatClone.brokenAnimation, true, 0.0f);
    deathCatClone.Chain8Spine.AnimationState.AddAnimation(0, deathCatClone.brokenAnimation, true, 0.0f);
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.DamagedRoutineIE(false, true, false));
    DeathCatController.Instance.conversation3.Play();
    CameraManager.instance.Stopshake();
    MMVibrate.StopRumble();
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(deathCatClone.gameObject, 14f);
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.TeleportOutIE());
    deathCatClone.gameObject.SetActive(false);
    DeathCatController.Instance.DeathCatBigTransform();
  }

  public void SetFake()
  {
    this.IsFake = true;
    this.health.HP = 3f;
  }

  public void SetReal()
  {
    this.IsFake = false;
    this.health.HP = this.health.totalHP;
  }

  public void InitializeProjectilePatternRings()
  {
    int initialPoolSize = 9;
    if (this.projectilePatternRings is ProjectileCirclePattern)
    {
      ProjectileCirclePattern projectilePatternRings = (ProjectileCirclePattern) this.projectilePatternRings;
      if ((UnityEngine.Object) projectilePatternRings.ProjectilePrefab != (UnityEngine.Object) null)
        ObjectPool.CreatePool<Projectile>(projectilePatternRings.ProjectilePrefab, projectilePatternRings.BaseProjectilesCount * initialPoolSize);
    }
    ObjectPool.CreatePool<ProjectileCircleBase>(this.projectilePatternRings, initialPoolSize);
  }

  public void ShootProjectileRings()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.ShootProjectileRingsIE());
  }

  public IEnumerator ShootProjectileRingsIE()
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; i <= UnityEngine.Random.Range(3, 6); ++i)
    {
      Projectile component = ObjectPool.Spawn<ProjectileCircleBase>(deathCatClone.projectilePatternRings, deathCatClone.transform.parent).GetComponent<Projectile>();
      component.transform.position = deathCatClone.transform.position;
      component.Angle = deathCatClone.GetAngleToPlayer();
      component.health = deathCatClone.health;
      component.team = Health.Team.Team2;
      component.Speed = deathCatClone.projectilePatternRingsSpeed;
      component.Acceleration = deathCatClone.projectilePatternRingsAcceleration;
      component.GetComponent<ProjectileCircleBase>().InitDelayed(deathCatClone.targetObject, deathCatClone.projectilePatternRingsRadius * 2f, 0.0f);
      yield return (object) new WaitForSeconds(0.5f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void InitializeProjectileCross()
  {
    this.projectileCross = UnityEngine.Object.Instantiate<ProjectileCross>(this.projectilePatternCross, this.transform.parent);
    this.projectileCross.gameObject.SetActive(false);
  }

  public void ShootProjectileCross()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.ShootProjectileCrossIE());
  }

  public IEnumerator ShootProjectileCrossIE()
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.isMoving = false;
    yield return (object) new WaitForSeconds(0.5f);
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    deathCatClone.projectileCross.gameObject.SetActive(true);
    deathCatClone.projectileCross.InitDelayed();
    deathCatClone.projectileCross.transform.position = deathCatClone.transform.position;
    deathCatClone.projectileCross.Projectile.health = deathCatClone.health;
    deathCatClone.projectileCross.Projectile.team = Health.Team.Team2;
    float duration = deathCatClone.projectilePatternCrossDuration;
    float t = 0.0f;
    float timer = 0.0f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      timer += Time.deltaTime;
      if ((double) timer > 2.0)
      {
        timer = 0.0f;
        deathCatClone.Attack(DeathCatClone.AttackType.Projectiles_Circles);
      }
      yield return (object) null;
    }
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.projectileCross.DisableProjectiles());
    deathCatClone.isMoving = true;
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void ShootProjectileSwirls()
  {
    this.Spine.AnimationState.SetAnimation(0, this.shootAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.currentAttackRoutine = this.StartCoroutine(this.ShootProjectileSwirlsIE());
  }

  public IEnumerator ShootProjectileSwirlsIE()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.swirls.ShootIE(target: deathCatClone.targetObject));
    yield return (object) new WaitForSeconds(1f);
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void ShootProjectileBouncers()
  {
    this.Spine.AnimationState.SetAnimation(0, this.shootAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.currentAttackRoutine = this.StartCoroutine(this.ShootProjectileBouncersIE());
  }

  public IEnumerator ShootProjectileBouncersIE()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.bouncers.ShootIE(target: deathCatClone.targetObject));
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void ShootProjectileCircles()
  {
    this.Spine.AnimationState.SetAnimation(0, this.shootAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.StartCoroutine(this.ShootProjectileCirclesIE());
  }

  public IEnumerator ShootProjectileCirclesIE()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.circles.ShootIE(target: deathCatClone.targetObject));
    yield return (object) new WaitForSeconds(3f);
  }

  public void ShootProjectileCirclesFast()
  {
    this.Spine.AnimationState.SetAnimation(0, this.shootAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.currentAttackRoutine = this.StartCoroutine(this.ShootProjectileCirclesFastIE());
  }

  public IEnumerator ShootProjectileCirclesFastIE()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.circlesFast.ShootIE(target: deathCatClone.targetObject));
    yield return (object) new WaitForSeconds(5f);
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void ShootProjectilePulse()
  {
    this.Spine.AnimationState.SetAnimation(0, this.shootAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.currentAttackRoutine = this.StartCoroutine(this.ShootProjectilePulseIE());
  }

  public IEnumerator ShootProjectilePulseIE()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.pulse.ShootIE(target: deathCatClone.targetObject));
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public float GetAngleToPlayer()
  {
    return Utils.GetAngle(this.transform.position, this.targetObject.transform.position);
  }

  public void ShootProjectileRingsTriple()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.ShootProjectileRingsTripleIE());
  }

  public IEnumerator ShootProjectileRingsTripleIE()
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, true);
    yield return (object) new WaitForSeconds(0.5f);
    for (int index = 0; index < UnityEngine.Random.Range(5, 9); ++index)
    {
      float t = (float) index / 2f;
      float angleToPlayer = deathCatClone.GetAngleToPlayer();
      Vector3 vector2 = (Vector3) Utils.DegreeToVector2(angleToPlayer);
      Vector3 vector3 = Vector3.Lerp((Vector3) Utils.DegreeToVector2(angleToPlayer - 90f), (Vector3) Utils.DegreeToVector2(angleToPlayer + 90f), t) * 1.25f;
      Projectile component = ObjectPool.Spawn<ProjectileCircleBase>(deathCatClone.projectilePatternRings, deathCatClone.transform.parent).GetComponent<Projectile>();
      component.transform.position = deathCatClone.transform.position;
      component.Angle = angleToPlayer;
      component.health = deathCatClone.health;
      component.team = Health.Team.Team2;
      component.Speed = deathCatClone.projectilePatternRingsSpeed;
      component.Acceleration = deathCatClone.projectilePatternRingsAcceleration;
      float shootDelay = (float) index * 0.5f;
      component.GetComponent<ProjectileCircleBase>().InitDelayed(PlayerFarming.Instance.gameObject, deathCatClone.projectilePatternRingsRadius, shootDelay, new System.Action(deathCatClone.\u003CShootProjectileRingsTripleIE\u003Eb__104_0));
    }
    yield return (object) new WaitForSeconds(deathCatClone.projectilePatternRingsDuration + 3f);
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.idleAnimation, true);
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void TrapPatternChasePlayer()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TrapPatternChasePlayerIE());
  }

  public void InitializeTraps() => ObjectPool.CreatePool(this.trapPrefab, 40);

  public IEnumerator TrapPatternChasePlayerIE()
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    Vector3 Position = Vector3.zero;
    int i = -1;
    float Dist = 1f;
    deathCatClone.state.facingAngle = Utils.GetAngle(deathCatClone.transform.position, deathCatClone.targetObject.transform.position);
    float facingAngle = deathCatClone.state.facingAngle;
    while (++i < 20)
    {
      GameObject gameObject = ObjectPool.Spawn(deathCatClone.trapPrefab);
      float angle = Utils.GetAngle(deathCatClone.transform.position + Position, deathCatClone.targetObject.transform.position);
      Position += new Vector3(Dist * Mathf.Cos(angle * ((float) Math.PI / 180f)), Dist * Mathf.Sin(angle * ((float) Math.PI / 180f)));
      gameObject.transform.position = deathCatClone.transform.position + Position;
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(1f);
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void TrapPattern0()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TrapPattern0IE());
  }

  public IEnumerator TrapPattern0IE()
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    deathCatClone.state.facingAngle = Utils.GetAngle(deathCatClone.transform.position, deathCatClone.targetObject.transform.position);
    int r = UnityEngine.Random.Range(3, 6);
    int i = -1;
    while (++i < 10)
    {
      int num1 = -1;
      while (++num1 < r)
      {
        GameObject gameObject = ObjectPool.Spawn(deathCatClone.trapPrefab);
        float f = (float) (((double) deathCatClone.state.facingAngle + (double) (90 * num1)) * (Math.PI / 180.0));
        float num2 = (float) (i * 2);
        Vector3 vector3 = new Vector3(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
        gameObject.transform.position = deathCatClone.transform.position + vector3;
      }
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(1f);
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void TrapPattern1()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TrapPattern1IE());
  }

  public IEnumerator TrapPattern1IE()
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.isMoving = false;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    deathCatClone.state.facingAngle = Utils.GetAngle(deathCatClone.transform.position, deathCatClone.targetObject.transform.position);
    int i = -1;
    while (++i < 10)
    {
      GameObject gameObject = ObjectPool.Spawn(deathCatClone.trapPrefab);
      float f = (float) (((double) deathCatClone.state.facingAngle + (double) (36 * i)) * (Math.PI / 180.0));
      float num = 3f;
      Vector3 vector3 = new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
      gameObject.transform.position = deathCatClone.transform.position + vector3;
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.02f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    deathCatClone.isMoving = true;
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void TrapPattern2()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TrapPattern2IE());
  }

  public IEnumerator TrapPattern2IE()
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.isMoving = false;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    deathCatClone.state.facingAngle = Utils.GetAngle(deathCatClone.transform.position, deathCatClone.targetObject.transform.position);
    float startingAngle = deathCatClone.state.facingAngle;
    int i = -1;
    while (++i < 10)
    {
      int num1 = -1;
      while (++num1 < UnityEngine.Random.Range(4, 7))
      {
        GameObject gameObject = ObjectPool.Spawn(deathCatClone.trapPrefab);
        float num2 = (float) (((double) startingAngle + (double) (90 * num1)) * (Math.PI / 180.0));
        float num3 = (float) (i * 2);
        float f = num2 + (float) i * 0.1f;
        Vector3 vector3 = new Vector3(num3 * Mathf.Cos(f), num3 * Mathf.Sin(f));
        gameObject.transform.position = deathCatClone.transform.position + vector3;
      }
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.1f);
    }
    yield return (object) new WaitForSeconds(1f);
    deathCatClone.isMoving = true;
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void TrapScattered()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TrapScatteredIE());
  }

  public IEnumerator TrapScatteredIE(int c = 0)
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.isMoving = false;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    deathCatClone.state.facingAngle = Utils.GetAngle(deathCatClone.transform.position, deathCatClone.targetObject.transform.position);
    List<Vector3> spawnedTrapsPositions = new List<Vector3>();
    int rand = UnityEngine.Random.Range(30, 40);
    for (int i = 0; i < UnityEngine.Random.Range(30, 40); ++i)
    {
      Vector3 b = Vector3.zero;
      int num = 0;
      while (num++ < 30)
      {
        b = new Vector3(UnityEngine.Random.Range(-11f, 11f), UnityEngine.Random.Range(-7f, 7f), 0.0f);
        bool flag = false;
        foreach (Vector3 a in spawnedTrapsPositions)
        {
          if ((double) Vector3.Distance(a, b) < 1.5)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          break;
      }
      if (rand == i)
        b = deathCatClone.targetObject.transform.position;
      spawnedTrapsPositions.Add(b);
      ObjectPool.Spawn(deathCatClone.trapPrefab).transform.position = b;
      yield return (object) new WaitForSeconds(0.05f);
    }
    if (c < 3 && UnityEngine.Random.Range(0, 2) == 0)
    {
      deathCatClone.currentAttackRoutine = deathCatClone.StartCoroutine(deathCatClone.TrapScatteredIE(c + 1));
    }
    else
    {
      yield return (object) new WaitForSeconds(1f);
      deathCatClone.isMoving = true;
      deathCatClone.currentAttackRoutine = (Coroutine) null;
    }
  }

  public void TeleportAttack0()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TeleportAttack0IE());
  }

  public IEnumerator TeleportAttack0IE()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.TeleportToPositionIE(deathCatClone.GetPositionAwayFromPlayer()));
    deathCatClone.isMoving = false;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.shootAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.25f);
    int j = -1;
    while (++j < UnityEngine.Random.Range(2, 4))
    {
      deathCatClone.state.facingAngle = Utils.GetAngle(deathCatClone.transform.position, deathCatClone.targetObject.transform.position);
      float Angle = deathCatClone.state.facingAngle * ((float) Math.PI / 180f);
      int i = -1;
      while (++i < 15)
      {
        GameObject gameObject = ObjectPool.Spawn(deathCatClone.trapPrefab);
        float num = (float) (i * 2);
        Vector3 vector3 = new Vector3(num * Mathf.Cos(Angle), num * Mathf.Sin(Angle));
        gameObject.transform.position = deathCatClone.transform.position + vector3;
        CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
        yield return (object) new WaitForSeconds(0.05f);
      }
      yield return (object) new WaitForSeconds(0.1f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    deathCatClone.isMoving = true;
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void TeleportAttack1()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TeleportAttack1IE());
  }

  public IEnumerator TeleportAttack1IE()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.TeleportToPositionIE(deathCatClone.GetPositionAwayFromPlayer()));
    deathCatClone.isMoving = false;
    yield return (object) (deathCatClone.currentAttackRoutine = deathCatClone.StartCoroutine(deathCatClone.ShootProjectileRingsIE()));
    deathCatClone.isMoving = true;
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void TeleportAttack2()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TeleportAttack12E());
  }

  public IEnumerator TeleportAttack12E()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.TeleportToPositionIE(deathCatClone.GetPositionAwayFromPlayer()));
    deathCatClone.isMoving = false;
    yield return (object) (deathCatClone.currentAttackRoutine = deathCatClone.StartCoroutine(deathCatClone.ShootProjectileRingsTripleIE()));
    deathCatClone.isMoving = true;
    deathCatClone.currentAttackRoutine = (Coroutine) null;
  }

  public void TeleportRandomly()
  {
    this.currentAttackRoutine = this.StartCoroutine(this.TeleportRandomlyIE());
  }

  public IEnumerator TeleportRandomlyIE(float endDelay = 1f)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DeathCatClone deathCatClone = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      deathCatClone.currentAttackRoutine = (Coroutine) null;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) deathCatClone.StartCoroutine(deathCatClone.TeleportToPositionIE(deathCatClone.GetRandomPosition(), endDelay));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator TeleportToPositionIE(Vector3 position, float endDelay = 0.0f)
  {
    DeathCatClone deathCatClone = this;
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.TeleportOutIE());
    deathCatClone.transform.position = position;
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) deathCatClone.StartCoroutine(deathCatClone.TeleportInIE());
    yield return (object) new WaitForSeconds(endDelay);
  }

  public IEnumerator TeleportOutIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DeathCatClone deathCatClone = this;
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
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.disappearAnimation, false);
    deathCatClone.health.invincible = true;
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/teleport_away", deathCatClone.gameObject);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator TeleportInIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DeathCatClone deathCatClone = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      deathCatClone.health.invincible = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.appearAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/teleport_return", deathCatClone.gameObject);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public Vector3 GetRandomPosition()
  {
    Vector3 b;
    do
    {
      b = (Vector3) (UnityEngine.Random.insideUnitCircle * 7f);
    }
    while ((double) Vector3.Distance(this.transform.position, b) <= 3.0);
    return b;
  }

  public Vector3 GetPositionAwayFromPlayer()
  {
    List<RaycastHit2D> raycastHit2DList = new List<RaycastHit2D>();
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(1f, 1f), 100f, (int) this.layerToCheck));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(-1f, 1f), 100f, (int) this.layerToCheck));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(-0.5f, -0.5f), 100f, (int) this.layerToCheck));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(0.5f, -0.5f), 100f, (int) this.layerToCheck));
    RaycastHit2D raycastHit2D1 = raycastHit2DList[0];
    for (int index = 1; index < raycastHit2DList.Count; ++index)
    {
      if ((UnityEngine.Object) this.targetObject != (UnityEngine.Object) null)
      {
        Vector3 position1 = this.targetObject.transform.position;
        RaycastHit2D raycastHit2D2 = raycastHit2DList[index];
        Vector3 point1 = (Vector3) raycastHit2D2.point;
        if ((double) Vector3.Distance(position1, point1) > (double) Vector3.Distance(this.targetObject.transform.position, (Vector3) raycastHit2D1.point))
        {
          Vector3 position2 = this.transform.position;
          raycastHit2D2 = raycastHit2DList[index];
          Vector3 point2 = (Vector3) raycastHit2D2.point;
          if ((double) Vector3.Distance(position2, point2) > 3.5)
            raycastHit2D1 = raycastHit2DList[index];
        }
      }
    }
    if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D1.point) <= 3.5)
    {
      raycastHit2DList.Remove(raycastHit2D1);
      raycastHit2D1 = raycastHit2DList[UnityEngine.Random.Range(0, raycastHit2DList.Count)];
    }
    return (Vector3) (raycastHit2D1.point + ((Vector2) this.transform.position - raycastHit2D1.point).normalized * 3f);
  }

  public override void Update()
  {
    if (this.UsePathing)
    {
      if (this.pathToFollow == null)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime;
        if (!this.isMoving)
          return;
        this.move();
        return;
      }
      if (this.currentWaypoint >= this.pathToFollow.Count)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime;
        if (!this.isMoving)
          return;
        this.move();
        return;
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
    {
      this.speed += (float) (((double) this.maxSpeed * (double) this.SpeedMultiplier - (double) this.speed) / (7.0 * (double) this.acceleration)) * GameManager.DeltaTime;
      if (this.UsePathing)
      {
        this.state.facingAngle = Mathf.LerpAngle(this.state.facingAngle, Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]), Time.deltaTime * this.turningSpeed);
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.pathToFollow[this.currentWaypoint]) <= (double) this.StoppingDistance)
        {
          ++this.currentWaypoint;
          if (this.currentWaypoint == this.pathToFollow.Count)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            System.Action endOfPath = this.EndOfPath;
            if (endOfPath != null)
              endOfPath();
            this.pathToFollow = (List<Vector3>) null;
          }
        }
      }
    }
    else
      this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime;
    this.DisableForces = !this.isMoving;
    this.move();
    if (this.currentAttackRoutine == null && (double) this.attackTimestamp == -1.0)
    {
      this.attackTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.timeBetweenAttacks.x, this.timeBetweenAttacks.y);
      if (this.phase > 1)
        this.attackTimestamp -= UnityEngine.Random.Range(0.5f, 1.5f);
    }
    else if (this.currentAttackRoutine != null)
      this.attackTimestamp = -1f;
    if (this.manualAttacking && (double) GameManager.GetInstance().CurrentTime > (double) this.attackTimestamp && (double) this.attackTimestamp != -1.0 && this.currentAttackRoutine == null)
      this.RandomAttack();
    if (this.SpawnEnemies && (double) GameManager.GetInstance().CurrentTime > (double) this.spawnEnemyTimestamp && Health.team2.Count < this.maxEnemies + 1 && this.isSpawning)
    {
      this.SpawnRandomEnemy();
      this.spawnEnemyTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.timeBetweenEnemies.x, this.timeBetweenEnemies.y);
    }
    if (this.phase != 1 || (double) this.health.HP / (double) this.health.totalHP >= 0.5 || this.currentAttackRoutine != null)
      return;
    this.phase = 2;
    this.DamagedRoutine(true);
    this.maxEnemies = Mathf.RoundToInt((float) this.maxEnemies * 1.25f);
  }

  public void SpawnRandomEnemy()
  {
    UnitObject enemy;
    Addressables.LoadAssetAsync<GameObject>((object) this.spawningEnemiesList[UnityEngine.Random.Range(0, this.spawningEnemiesList.Length)]).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedAddressableAssets.Add(obj);
      enemy = ObjectPool.Spawn(obj.Result, this.transform.parent, this.GetRandomPosition(), Quaternion.identity).GetComponent<UnitObject>();
      enemy.gameObject.SetActive(false);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(enemy.transform.position, this.transform.parent, enemy.gameObject);
      enemy.GetComponent<UnitObject>().CanHaveModifier = false;
      enemy.GetComponent<UnitObject>().RemoveModifier();
      enemy.transform.parent = this.transform.parent.parent;
      DropLootOnDeath component = enemy.GetComponent<DropLootOnDeath>();
      if (!(bool) (UnityEngine.Object) component)
        return;
      component.GiveXP = false;
    });
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.SimpleSpineFlash.FlashFillRed();
  }

  public void RandomAttack()
  {
    int num = UnityEngine.Random.Range(0, 2);
    if (this.count <= 1)
    {
      ++this.count;
      switch (num)
      {
        case 0:
          this.Attack(DeathCatClone.AttackType.Teleport_Attack0);
          break;
        case 1:
          if (this.phase == 1)
          {
            this.Attack(DeathCatClone.AttackType.Teleport_Attack1);
            break;
          }
          this.Attack(DeathCatClone.AttackType.Teleport_Attack2);
          break;
      }
    }
    else if (this.count == 2)
    {
      ++this.count;
      this.TeleportRandomly();
    }
    else
    {
      this.count = 0;
      switch (num)
      {
        case 0:
          if (UnityEngine.Random.Range(0, 2) == 0)
          {
            this.Attack(DeathCatClone.AttackType.Traps_Scattered);
            break;
          }
          if (this.phase == 1)
          {
            this.Attack(DeathCatClone.AttackType.Projectiles_Swirls);
            break;
          }
          this.Attack(DeathCatClone.AttackType.Projectiles_CirclesFast);
          break;
        case 1:
          if (UnityEngine.Random.Range(0, 2) == 0)
          {
            this.Attack(DeathCatClone.AttackType.Traps_Pattern1);
            break;
          }
          this.Attack(DeathCatClone.AttackType.Traps_Pattern2);
          break;
      }
    }
  }

  public void DamagedRoutine(bool teleportAfter)
  {
    this.currentAttackRoutine = this.StartCoroutine(this.DamagedRoutineIE(teleportAfter));
  }

  public IEnumerator DamagedRoutineIE(
    bool teleportAfter,
    bool wakeKnockedPlayers = false,
    bool playStopHurt = true)
  {
    DeathCatClone deathCatClone = this;
    deathCatClone.isSpawning = false;
    deathCatClone.isMoving = false;
    deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.hurtAnimation, false);
    deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.hurtLoopAnimation, true, 0.0f);
    deathCatClone.distortionObject.SetActive(true);
    deathCatClone.distortionObject.transform.DOScale(25f, 3.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(deathCatClone.\u003CDamagedRoutineIE\u003Eb__135_0));
    yield return (object) new WaitForSeconds(1f);
    deathCatClone.Chain5Spine.AnimationState.SetAnimation(0, deathCatClone.breakAnimation1, false);
    deathCatClone.Chain7Spine.AnimationState.SetAnimation(0, deathCatClone.breakAnimation2, false);
    deathCatClone.Chain9Spine.AnimationState.SetAnimation(0, deathCatClone.breakAnimation3, false);
    deathCatClone.Chain5Spine.AnimationState.AddAnimation(0, deathCatClone.brokenAnimation, true, 0.0f);
    deathCatClone.Chain7Spine.AnimationState.AddAnimation(0, deathCatClone.brokenAnimation, true, 0.0f);
    deathCatClone.Chain9Spine.AnimationState.AddAnimation(0, deathCatClone.brokenAnimation, true, 0.0f);
    if (!teleportAfter)
    {
      MMVibrate.RumbleContinuous(0.5f, 0.75f);
      CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, float.MaxValue);
    }
    yield return (object) new WaitForSeconds(2f);
    if (PlayerFarming.IsAnyPlayerKnockedOut() & wakeKnockedPlayers)
    {
      bool awaitKnockedPlayer = true;
      CoopManager.Instance.OnKnockedPlayerAwoken += (System.Action) (() => awaitKnockedPlayer = false);
      CoopManager.Instance.WakeAllKnockedOutPlayersWithHealth();
      while (awaitKnockedPlayer)
        yield return (object) null;
    }
    PlayerFarming.ReloadAllFaith();
    if (playStopHurt)
    {
      deathCatClone.Spine.AnimationState.SetAnimation(0, deathCatClone.stopHurtAnimation, false);
      deathCatClone.Spine.AnimationState.AddAnimation(0, deathCatClone.idleAnimation, true, 0.0f);
    }
    deathCatClone.currentAttackRoutine = (Coroutine) null;
    if (teleportAfter)
    {
      deathCatClone.currentAttackRoutine = deathCatClone.StartCoroutine(deathCatClone.TeleportToPositionIE(Vector3.zero));
      yield return (object) deathCatClone.currentAttackRoutine;
      deathCatClone.Attack(DeathCatClone.AttackType.Projectiles_Cross);
      deathCatClone.isSpawning = true;
    }
  }

  public void DamageEnemies(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == Health.Team.PlayerTeam || !((UnityEngine.Object) component != (UnityEngine.Object) this.health))
      return;
    component.ImpactOnHit = false;
    component.ScreenshakeOnDie = false;
    component.ScreenshakeOnHit = false;
    component.invincible = false;
    component.untouchable = false;
    component.DealDamage(component.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Poison, dealDamageImmediately: true);
  }

  public IEnumerator ActiveRoutine()
  {
    DeathCatClone deathCatClone = this;
    yield return (object) new WaitForSeconds(0.2f);
    Debug.Log((object) nameof (ActiveRoutine));
    while (true)
    {
      while (GameManager.GetInstance().CamFollowTarget.Contains(deathCatClone.gameObject) || !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Dieing || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.GameOver)
      {
        if (deathCatClone.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          double num = (double) (deathCatClone.IdleWait -= Time.deltaTime);
        }
        deathCatClone.GetNewTargetPosition();
        if ((UnityEngine.Object) deathCatClone.targetObject == (UnityEngine.Object) null && Time.frameCount % 10 == 0)
          deathCatClone.GetNewTarget();
        yield return (object) null;
      }
      GameManager.GetInstance().AddToCamera(deathCatClone.gameObject);
      GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    }
  }

  public void GetNewTargetPosition()
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      return;
    float num1 = 100f;
    while ((double) --num1 > 0.0)
    {
      float num2 = UnityEngine.Random.Range(this.distanceRange.x, this.distanceRange.y);
      this.randomDirection += (float) UnityEngine.Random.Range(-45, 45) * ((float) Math.PI / 180f);
      float radius = 0.1f;
      Vector3 targetLocation = this.transform.position + new Vector3(num2 * Mathf.Cos(this.randomDirection), num2 * Mathf.Sin(this.randomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), num2 * 0.5f, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      {
        this.randomDirection += 0.17453292f;
      }
      else
      {
        this.IdleWait = UnityEngine.Random.Range(this.idleWaitRange.x, this.idleWaitRange.y);
        this.givePath(targetLocation);
        break;
      }
    }
  }

  public void GetNewTarget()
  {
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.transform.position)))
      {
        float num2 = Vector3.Distance(this.transform.position, allUnit.gameObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          health = allUnit;
          num1 = num2;
        }
      }
    }
    if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      this.targetObject = health.gameObject;
    else
      this.targetObject = this.ReconsiderPlayerTarget().gameObject;
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if ((int) this.layerToCheck != ((int) this.bounceMask | 1 << collision.gameObject.layer))
      return;
    if ((double) this.speed < (double) this.maxSpeed)
      this.speed *= 1.2f;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.IdleWait = UnityEngine.Random.Range(this.idleWaitRange.x, this.idleWaitRange.y);
    this.state.facingAngle = Utils.GetAngle((Vector3) (Vector2) this.transform.position, (Vector3) ((Vector2) this.transform.position + Vector2.Reflect(Utils.DegreeToVector2(this.state.facingAngle), collision.contacts[0].normal)));
  }

  public void OnLanguageChanged()
  {
    UIBossHUD.Instance?.UpdateName(ScriptLocalization.NAMES.DeathNPC);
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__73_0()
  {
    this.currentAttackRoutine = (Coroutine) null;
    AudioManager.Instance.SetMusicRoomID(3, "deathcat_room_id");
    this.StartCoroutine(this.ActiveRoutine());
  }

  [CompilerGenerated]
  public void \u003CShootProjectileRingsTripleIE\u003Eb__104_0()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CDamagedRoutineIE\u003Eb__135_0()
  {
    this.distortionObject.transform.localScale = Vector3.zero;
    this.distortionObject.SetActive(false);
  }

  public enum AttackType
  {
    None = 0,
    Projectiles_Rings = 1,
    Projectiles_Swirls = 2,
    Projectiles_Bouncers = 3,
    Projectiles_Circles = 4,
    Projectiles_Cross = 5,
    Projectiles_Pulse = 6,
    Projectiles_TripleRings = 7,
    Projectiles_CirclesFast = 8,
    Traps_Targeted = 50, // 0x00000032
    Traps_Pattern0 = 51, // 0x00000033
    Traps_Pattern1 = 52, // 0x00000034
    Traps_Pattern2 = 53, // 0x00000035
    Traps_Scattered = 54, // 0x00000036
    Teleport_Attack0 = 100, // 0x00000064
    Teleport_Attack1 = 101, // 0x00000065
    Teleport_Attack2 = 102, // 0x00000066
  }
}
