// Decompiled with JetBrains decompiler
// Type: EnemyWolfGuardianMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
public class EnemyWolfGuardianMiniboss : UnitObject
{
  public float SpeedModifierIncrementPerPhase = 1f;
  public float RangedAttackSignpostTime = 1f;
  public int MeleeAttackCount = 3;
  public float MeleeRange = 3f;
  public float MeleeRecoveryTime = 0.3f;
  public float AttackForce = 1000f;
  public float AttackForceIncrementPerPhase = 500f;
  public float SpecialAttackRecoveryTime = 2f;
  public float SpecialAttackFlyHeight = -3f;
  public float SpecialAttackTimeToRise = 2f;
  public float StaffRangedAttackDuration = 3f;
  public float StaffSpecialDuration = 5f;
  public float StaffBeamTrackingSpeed = 3f;
  public float StaffBeamBarrierLength = 1f;
  public GameObject StaffBeamBarrierLineTrigger;
  public ParticleSystem AxeSpecialImpactParticles;
  public float AxeSpecialGroundSlamSpeed = 10f;
  public float AxeThrowReturnTime = 0.4f;
  public float AxeThrowSpreadAngle = 10f;
  public int GroundSlamCount = 3;
  public float GroundSlamTimeBetweenAttacks = 0.8f;
  public float TimeBetweenSlashes = 1f;
  public int SwordRangedProjectileCount = 3;
  public float SlashProjectileSpeed = 15f;
  public ProjectileVerticalSlash VerticalSlashPrefab;
  public float SwordSpecialAngleSpread = 10f;
  public float SwordSpecialTimeToGround = 0.5f;
  public int SwordSpecialSlamCount = 2;
  public float SwordSpecialTimeBetweenSlams = 1f;
  public float JumpAwayMinDistance = 4f;
  public float JumpAwayMaxDistance = 8f;
  public float JumpHeight = 4f;
  public float JumpDuration = 0.8f;
  public Vector2 SpawnAmount;
  public Vector2 DelayBetweenSpawns;
  public float SpawnedEnemiesCooldown = 1f;
  public AssetReferenceGameObject Spawnable;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  public ParticleSystem DashParticles;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string WalkAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffMeleeSignPostAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffMeleeAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MeleeRecoveryAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RangedAttackSignPostAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RangedAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RangedRecoveryAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SpawnEnemiesAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AxeAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SwordAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string PainAnimation;
  [EventRef]
  public string AttackVO = "event:/enemy/vocals/humanoid_large/attack";
  [EventRef]
  public string onHitSoundSFX = "event:/enemy/impact_blunt";
  [EventRef]
  public string MeleeAttackSFX = string.Empty;
  [EventRef]
  public string SwordSlashSFX = string.Empty;
  [EventRef]
  public string GroundSlamSFX = string.Empty;
  [EventRef]
  public string BeamLoopSFX = string.Empty;
  [EventRef]
  public string RiseUpSFX = string.Empty;
  [EventRef]
  public string FlyDownSFX = string.Empty;
  [EventRef]
  public string SpawnEnemiesSFX = string.Empty;
  public DeadBodySliding deadBodySliding;
  public EnemyWolfGuardianMiniboss.BossPhase currentPhase;
  public SimpleSpineFlash SimpleSpineFlash;
  public SimpleSpineEventListener simpleSpineEventListener;
  public List<Collider2D> collider2DList;
  public Collider2D HealthCollider;
  public ProjectileVerticalSlash projectileSlash;
  public Vector3 TargetPosition;
  public GameObject TargetObject;
  public Health EnemyHealth;
  public AsyncOperationHandle<GameObject> loadedEnemyAddressable;
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
  public float GlobalAttackDelay = 2f;
  public float GlobalRingAttackDelay = 10f;
  public float GlobalPatternAttackDelay = 5f;
  public float beamAttackCooldownCounter;
  public float groundSlamCooldownCounter;
  public float airSlashCooldownCounter;
  public float circleCastRadius = 0.5f;
  public float circleCastOffset = 1f;
  public float swordSpecialDropOffsetTime = 0.5f;
  public float axeRangedRecoveryTime = 2f;
  public float diveBombColliderDuration = 0.2f;
  public float nextPhaseHealthVal;
  public float repathTimer;
  public bool ChasingPlayer;
  public float rangedAttackCooldownCounter;
  public float groundSlamDelta = 0.5f;
  public EventInstance beamLoop;

  public override void OnEnable()
  {
    this.Preload();
    this.SeperateObject = true;
    base.OnEnable();
    this.SimpleSpineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
    this.simpleSpineEventListener = this.GetComponent<SimpleSpineEventListener>();
    this.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.HealthCollider = this.GetComponent<Collider2D>();
    this.DashParticles.Stop();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.health.OnAddCharm += new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared += new Health.StasisEvent(this.ReconsiderTarget);
    this.TargetPosition = this.transform.position;
    this.nextPhaseHealthVal = this.health.totalHP;
    this.nextPhaseHealthVal -= this.health.totalHP * 0.33f;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.simpleSpineEventListener.OnSpineEvent -= new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.health.OnAddCharm -= new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared -= new Health.StasisEvent(this.ReconsiderTarget);
  }

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null)
      DataManager.Instance.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
    this.StaffBeamBarrierLineTrigger.SetActive(false);
  }

  public override void Update()
  {
    base.Update();
    if (this.state.CURRENT_STATE != StateMachine.State.Moving || !this.ChasingPlayer)
      return;
    this.LookAtTarget();
    if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.TargetObject.transform.position) < 3.0 || (double) (this.repathTimer += Time.deltaTime * this.Spine.timeScale) <= 0.20000000298023224)
      return;
    this.repathTimer = 0.0f;
    this.GetPath();
  }

  public override void FixedUpdate()
  {
    base.FixedUpdate();
    if (!this.IsReadyForNextBossPhase())
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.TransitionToNextPhase(EnemyWolfGuardianMiniboss.BossPhase.One));
  }

  public void Preload()
  {
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.Spawnable);
    asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedEnemyAddressable = obj;
      obj.Result.CreatePool(24, true);
    });
    asyncOperationHandle.WaitForCompletion();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) guardianMiniboss.TargetObject == (UnityEngine.Object) null)
    {
      guardianMiniboss.SetTargetObject();
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) guardianMiniboss.TargetObject == (UnityEngine.Object) null)
      {
        guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.WaitForTarget());
        yield break;
      }
      if ((double) Vector3.Distance(guardianMiniboss.TargetObject.transform.position, guardianMiniboss.transform.position) <= (double) guardianMiniboss.VisionRange)
        InRange = true;
      yield return (object) null;
    }
    guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.FightPlayer());
  }

  public void SetTargetObject()
  {
    Health closestTarget = this.GetClosestTarget(true);
    if (!(bool) (UnityEngine.Object) closestTarget)
      return;
    this.TargetObject = closestTarget.gameObject;
    this.VisionRange = int.MaxValue;
  }

  public void ReconsiderTarget()
  {
    this.TargetObject = (GameObject) null;
    this.SetTargetObject();
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget(true) == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/gethit", this.transform.position);
    if (!string.IsNullOrEmpty(this.onHitSoundSFX))
      AudioManager.Instance.PlayOneShot(this.onHitSoundSFX, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/death", this.transform.position);
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StopAllCoroutines();
  }

  public void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "Invincible On":
        this.health.invincible = true;
        this.HealthCollider.enabled = false;
        break;
      case "Invincible Off":
        this.health.invincible = false;
        this.HealthCollider.enabled = true;
        break;
    }
  }

  public void GetPath()
  {
    this.ChasingPlayer = false;
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      this.SetTargetObject();
    this.TargetPosition = this.TargetObject.transform.position;
    this.ChasingPlayer = true;
    if ((double) Vector3.Distance(this.TargetPosition, this.transform.position) > (double) this.StoppingDistance)
    {
      this.givePath(this.TargetPosition, this.TargetObject);
      if (!(this.Spine.AnimationName != this.WalkAnimation))
        return;
      this.Spine.AnimationState.SetAnimation(0, this.WalkAnimation, true);
    }
    else
    {
      if (!(this.Spine.AnimationName != this.IdleAnimation))
        return;
      this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
    }
  }

  public IEnumerator TransitionToNextPhase(EnemyWolfGuardianMiniboss.BossPhase phase)
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    ++guardianMiniboss.currentPhase;
    if (guardianMiniboss.currentPhase != EnemyWolfGuardianMiniboss.BossPhase.Two)
      guardianMiniboss.nextPhaseHealthVal -= guardianMiniboss.health.totalHP * 0.33f;
    guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.PainAnimation, false);
    Spine.Animation animation = guardianMiniboss.Spine.skeleton.Data.FindAnimation(guardianMiniboss.PainAnimation);
    guardianMiniboss.transform.DOKill();
    if ((double) guardianMiniboss.transform.position.z < 0.0)
      guardianMiniboss.transform.DOMoveZ(0.0f, guardianMiniboss.SwordSpecialTimeToGround * guardianMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(animation.Duration * guardianMiniboss.Spine.timeScale);
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.SpawnEnemies());
    guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.FightPlayer());
  }

  public bool IsReadyForNextBossPhase()
  {
    return this.currentPhase != EnemyWolfGuardianMiniboss.BossPhase.Two && (double) this.health.HP <= (double) this.nextPhaseHealthVal;
  }

  public IEnumerator FightPlayer()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    Debug.Log((object) $"Current phase: {guardianMiniboss.currentPhase}");
    while (true)
    {
      switch (guardianMiniboss.currentPhase)
      {
        case EnemyWolfGuardianMiniboss.BossPhase.Zero:
          yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoPhaseZero());
          continue;
        case EnemyWolfGuardianMiniboss.BossPhase.One:
          yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoPhaseOne());
          continue;
        case EnemyWolfGuardianMiniboss.BossPhase.Two:
          yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoPhaseTwo());
          continue;
        default:
          continue;
      }
    }
  }

  public IEnumerator SpawnEnemies()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.SpawnEnemiesAnimation, false);
    guardianMiniboss.Spine.AnimationState.AddAnimation(0, guardianMiniboss.IdleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot(guardianMiniboss.SpawnEnemiesSFX, guardianMiniboss.transform.position);
    yield return (object) new WaitForSeconds(guardianMiniboss.Spine.Skeleton.Data.FindAnimation(guardianMiniboss.SpawnEnemiesAnimation).Duration * guardianMiniboss.Spine.timeScale);
    int amountToSpawn = (int) UnityEngine.Random.Range(guardianMiniboss.SpawnAmount.x, guardianMiniboss.SpawnAmount.y + 1f);
    for (int i = 0; i < amountToSpawn; ++i)
    {
      UnitObject component = ObjectPool.Spawn(guardianMiniboss.loadedEnemyAddressable.Result, guardianMiniboss.transform.parent, guardianMiniboss.transform.position, Quaternion.identity).GetComponent<UnitObject>();
      guardianMiniboss.spawnedEnemies.Add(component);
      component.health.OnDie += new Health.DieAction(guardianMiniboss.SpawnedEnemyKilled);
      component.CanHaveModifier = false;
      component.RemoveModifier();
      float dur = UnityEngine.Random.Range(guardianMiniboss.DelayBetweenSpawns.x, guardianMiniboss.DelayBetweenSpawns.y);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * guardianMiniboss.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    yield return (object) new WaitForSeconds(guardianMiniboss.SpawnedEnemiesCooldown * guardianMiniboss.Spine.timeScale);
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

  public IEnumerator DoPhaseZero()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    bool Loop = true;
    while (Loop)
    {
      guardianMiniboss.LookAtTarget();
      guardianMiniboss.GetPath();
      yield return (object) guardianMiniboss.MoveTowardsClosestPlayer();
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeAttack());
      yield return (object) guardianMiniboss.MoveTowardsClosestPlayer();
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeAttack());
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoAxeRangedAttack());
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoAxeRangedAttack());
    }
  }

  public IEnumerator DoPhaseOne()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.SpeedMultiplier += guardianMiniboss.SpeedModifierIncrementPerPhase;
    guardianMiniboss.AttackForce += guardianMiniboss.AttackForceIncrementPerPhase;
    bool Loop = true;
    while (Loop)
    {
      guardianMiniboss.LookAtTarget();
      guardianMiniboss.GetPath();
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoSlashRangedAttack());
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoSlashRangedAttack());
      yield return (object) guardianMiniboss.MoveTowardsClosestPlayer();
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeAttack());
      yield return (object) guardianMiniboss.MoveTowardsClosestPlayer();
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeAttack());
    }
  }

  public IEnumerator DoPhaseTwo()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.SpeedMultiplier += guardianMiniboss.SpeedModifierIncrementPerPhase;
    guardianMiniboss.AttackForce += guardianMiniboss.AttackForceIncrementPerPhase;
    bool Loop = true;
    while (Loop)
    {
      guardianMiniboss.LookAtTarget();
      guardianMiniboss.GetPath();
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoBeamSpecialAttack());
      yield return (object) guardianMiniboss.MoveTowardsClosestPlayer();
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeAttack());
      yield return (object) guardianMiniboss.MoveTowardsClosestPlayer();
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeAttack());
    }
  }

  public IEnumerator MoveTowardsClosestPlayer()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Moving;
    Health closestTarget = guardianMiniboss.GetClosestTarget(true);
    guardianMiniboss.TargetObject = closestTarget?.gameObject;
    if ((UnityEngine.Object) guardianMiniboss.TargetObject != (UnityEngine.Object) null)
    {
      guardianMiniboss.TargetPosition = guardianMiniboss.TargetObject.transform.position;
      guardianMiniboss.givePath(guardianMiniboss.TargetPosition, guardianMiniboss.TargetObject);
      if (guardianMiniboss.Spine.AnimationName != guardianMiniboss.WalkAnimation)
        guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.WalkAnimation, true);
      while ((double) Vector2.Distance((Vector2) guardianMiniboss.transform.position, (Vector2) guardianMiniboss.TargetPosition) > (double) guardianMiniboss.MeleeRange)
        yield return (object) null;
    }
  }

  public IEnumerator DoMeleeAttack()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeSignpost());
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeMainLogic());
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoMeleeRecovery());
  }

  public IEnumerator DoMeleeSignpost()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.StaffMeleeSignPostAnimation, false);
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float progress = 0.0f;
    float animationDuration = guardianMiniboss.Spine.Skeleton.Data.FindAnimation(guardianMiniboss.StaffMeleeSignPostAnimation).Duration * guardianMiniboss.Spine.timeScale;
    while ((double) (progress += Time.deltaTime) < (double) animationDuration / (double) guardianMiniboss.Spine.timeScale)
    {
      guardianMiniboss.SimpleSpineFlash.FlashWhite(progress / animationDuration);
      yield return (object) null;
    }
    guardianMiniboss.SimpleSpineFlash.FlashWhite(false);
  }

  public IEnumerator DoMeleeMainLogic()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    int meleeAttackCount = guardianMiniboss.MeleeAttackCount;
    DataManager.Instance.LastSimpleGuardianAttacked = TimeManager.TotalElapsedGameTime;
    if (!string.IsNullOrEmpty(guardianMiniboss.AttackVO))
      AudioManager.Instance.PlayOneShot(guardianMiniboss.AttackVO, guardianMiniboss.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/warning", guardianMiniboss.transform.position);
    CameraManager.shakeCamera(0.4f, guardianMiniboss.state.facingAngle);
    for (int i = 0; i < guardianMiniboss.MeleeAttackCount; ++i)
    {
      guardianMiniboss.LookAtTarget();
      if ((UnityEngine.Object) guardianMiniboss.damageColliderEvents != (UnityEngine.Object) null)
        guardianMiniboss.damageColliderEvents.SetActive(true);
      guardianMiniboss.DashParticles.Play();
      AudioManager.Instance.PlayOneShot(guardianMiniboss.AttackVO, guardianMiniboss.transform.position);
      guardianMiniboss.DisableForces = true;
      Vector3 force = (Vector3) new Vector2(guardianMiniboss.AttackForce * Mathf.Cos(guardianMiniboss.state.facingAngle * ((float) Math.PI / 180f)), guardianMiniboss.AttackForce * Mathf.Sin(guardianMiniboss.state.facingAngle * ((float) Math.PI / 180f)));
      guardianMiniboss.rb.AddForce((Vector2) force);
      guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.StaffMeleeAttackAnimation, false);
      yield return (object) new WaitForSeconds(guardianMiniboss.Spine.skeleton.Data.FindAnimation(guardianMiniboss.StaffMeleeAttackAnimation).Duration * guardianMiniboss.Spine.timeScale);
      guardianMiniboss.DashParticles.Stop();
      if ((UnityEngine.Object) guardianMiniboss.damageColliderEvents != (UnityEngine.Object) null)
        guardianMiniboss.damageColliderEvents.SetActive(false);
      guardianMiniboss.DisableForces = false;
      guardianMiniboss.rb.velocity = Vector2.zero;
    }
    guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.IdleAnimation, true);
  }

  public IEnumerator DoMeleeRecovery()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.IdleAnimation, true);
      guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
      guardianMiniboss.ReconsiderTarget();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.MeleeRecoveryAnimation, false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(guardianMiniboss.MeleeRecoveryTime * guardianMiniboss.Spine.timeScale);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoBeamRangedAttack()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.JumpToEmptySpace());
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoRangedAttackSignpost());
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.BeamRangedAttackLogic());
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.StaffRangedRecovery());
  }

  public IEnumerator DoRangedAttackSignpost()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float progress = 0.0f;
    while ((double) (progress += Time.deltaTime * guardianMiniboss.Spine.timeScale) < (double) guardianMiniboss.RangedAttackSignpostTime)
    {
      guardianMiniboss.LookAtTarget();
      yield return (object) null;
    }
  }

  public IEnumerator BeamRangedAttackLogic()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      guardianMiniboss.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(guardianMiniboss.OnDamageTriggerEnter);
      guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    Health closestTarget = guardianMiniboss.GetClosestTarget();
    guardianMiniboss.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(guardianMiniboss.OnDamageTriggerEnter);
    guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.RangedAttackAnimation, true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoBeamAttack(closestTarget, guardianMiniboss.StaffRangedAttackDuration));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator StaffRangedRecovery()
  {
    yield return (object) null;
  }

  public IEnumerator DoAxeRangedAttack()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.JumpToEmptySpace());
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoRangedAttackSignpost());
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoAxeRangedLogic());
  }

  public IEnumerator DoAxeRangedLogic()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWolfGuardianMiniboss owner = this;
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
    owner.ReconsiderTarget();
    owner.LookAtTarget();
    owner.state.CURRENT_STATE = StateMachine.State.Attacking;
    ThrownAxe_Enemy.SpawnThrowingAxe((UnitObject) owner, owner.transform.position, owner.state.facingAngle - owner.AxeThrowSpreadAngle, owner.AxeThrowReturnTime);
    ThrownAxe_Enemy.SpawnThrowingAxe((UnitObject) owner, owner.transform.position, owner.state.facingAngle, owner.AxeThrowReturnTime);
    ThrownAxe_Enemy.SpawnThrowingAxe((UnitObject) owner, owner.transform.position, owner.state.facingAngle + owner.AxeThrowSpreadAngle, owner.AxeThrowReturnTime);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(owner.axeRangedRecoveryTime);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoSlashRangedAttack()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.JumpToEmptySpace());
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoRangedAttackSignpost());
    guardianMiniboss.GetClosestTarget();
    for (int i = 0; i < guardianMiniboss.SwordRangedProjectileCount; ++i)
    {
      guardianMiniboss.LookAtTarget();
      if (!string.IsNullOrEmpty(guardianMiniboss.SwordSlashSFX))
        AudioManager.Instance.PlayOneShot(guardianMiniboss.SwordSlashSFX, guardianMiniboss.transform.position);
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.SlashProjectilRoutine(guardianMiniboss.state.LookAngle));
    }
  }

  public IEnumerator DoBeamSpecialAttack()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    Health currentTarget = guardianMiniboss.GetClosestTarget();
    yield return (object) null;
    guardianMiniboss.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(guardianMiniboss.OnDamageTriggerEnter);
    guardianMiniboss.health.invincible = true;
    guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.IdleAnimation, true);
    guardianMiniboss.LockToGround = false;
    guardianMiniboss.transform.DOKill();
    guardianMiniboss.transform.DOMoveZ(guardianMiniboss.SpecialAttackFlyHeight, guardianMiniboss.SpecialAttackTimeToRise).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(1.5f);
    yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.DoBeamAttack(currentTarget, guardianMiniboss.StaffSpecialDuration));
    guardianMiniboss.health.invincible = false;
    guardianMiniboss.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(guardianMiniboss.OnDamageTriggerEnter);
    guardianMiniboss.transform.DOMoveZ(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.5f * guardianMiniboss.Spine.timeScale);
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianMiniboss.LockToGround = false;
  }

  public IEnumerator DoBeamAttack(Health currentTarget, float attackTime)
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    float progress = attackTime;
    guardianMiniboss.StaffBeamBarrierLineTrigger.SetActive(true);
    Vector3 lerpTarget = currentTarget.transform.position;
    if (!string.IsNullOrEmpty(guardianMiniboss.BeamLoopSFX))
      guardianMiniboss.beamLoop = AudioManager.Instance.CreateLoop(guardianMiniboss.BeamLoopSFX, guardianMiniboss.gameObject, true);
    while ((UnityEngine.Object) currentTarget != (UnityEngine.Object) null && (double) progress > 0.0)
    {
      progress -= Time.deltaTime;
      lerpTarget = Vector3.Lerp(lerpTarget, currentTarget.transform.position, guardianMiniboss.StaffBeamTrackingSpeed * Time.deltaTime);
      Vector3 vector3 = guardianMiniboss.transform.position + Vector3.back + (lerpTarget - guardianMiniboss.transform.position) / 2f;
      float num = Vector3.Distance(guardianMiniboss.transform.position, lerpTarget);
      guardianMiniboss.StaffBeamBarrierLineTrigger.transform.position = vector3;
      guardianMiniboss.StaffBeamBarrierLineTrigger.transform.LookAt(lerpTarget, Vector3.forward);
      guardianMiniboss.StaffBeamBarrierLineTrigger.transform.Rotate(-90f, 0.0f, 90f);
      Vector3 localScale = guardianMiniboss.StaffBeamBarrierLineTrigger.transform.localScale with
      {
        x = num * guardianMiniboss.StaffBeamBarrierLength
      };
      guardianMiniboss.StaffBeamBarrierLineTrigger.transform.localScale = localScale;
      BiomeConstants.Instance.EmitHammerEffects(lerpTarget, 0.5f, 1f, 0.1f);
      yield return (object) null;
    }
    if (!string.IsNullOrEmpty(guardianMiniboss.BeamLoopSFX))
      AudioManager.Instance.StopLoop(guardianMiniboss.beamLoop);
    guardianMiniboss.StaffBeamBarrierLineTrigger.SetActive(false);
  }

  public IEnumerator SlashProjectilRoutine(float angle)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
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
    guardianMiniboss.projectileSlash = ObjectPool.Spawn<ProjectileVerticalSlash>(guardianMiniboss.VerticalSlashPrefab, guardianMiniboss.transform.position, Quaternion.identity).GetComponent<ProjectileVerticalSlash>();
    guardianMiniboss.projectileSlash.Play(guardianMiniboss.transform.position, angle, guardianMiniboss.SlashProjectileSpeed, guardianMiniboss.health.team);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(guardianMiniboss.TimeBetweenSlashes);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoSlamSpecialAttack()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    guardianMiniboss.health.invincible = true;
    guardianMiniboss.LockToGround = false;
    for (int i = 0; i < guardianMiniboss.GroundSlamCount; ++i)
    {
      guardianMiniboss.transform.DOKill();
      guardianMiniboss.transform.DOMoveZ(guardianMiniboss.SpecialAttackFlyHeight, guardianMiniboss.SpecialAttackTimeToRise * guardianMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      if (!string.IsNullOrEmpty(guardianMiniboss.RiseUpSFX))
        AudioManager.Instance.PlayOneShot(guardianMiniboss.RiseUpSFX, guardianMiniboss.transform.position);
      yield return (object) new WaitForSeconds((guardianMiniboss.SpecialAttackTimeToRise - guardianMiniboss.groundSlamDelta) * guardianMiniboss.Spine.timeScale);
      yield return (object) guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.GroundSlamLogic());
      yield return (object) new WaitForSeconds(guardianMiniboss.GroundSlamTimeBetweenAttacks * guardianMiniboss.Spine.timeScale);
    }
    yield return (object) new WaitForSeconds(guardianMiniboss.SpecialAttackRecoveryTime * guardianMiniboss.Spine.timeScale);
    guardianMiniboss.damageColliderEvents.OnTriggerStayEvent -= new ColliderEvents.TriggerEvent(guardianMiniboss.OnDamageTriggerEnter);
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianMiniboss.LockToGround = true;
    guardianMiniboss.health.invincible = false;
  }

  public IEnumerator GroundSlamLogic()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    Vector3 position = guardianMiniboss.GetClosestTarget().transform.position;
    float num = Vector3.Distance(guardianMiniboss.transform.position, position) / guardianMiniboss.AxeSpecialGroundSlamSpeed;
    guardianMiniboss.transform.DOKill();
    guardianMiniboss.transform.DOMove(position, num * guardianMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    if (!string.IsNullOrEmpty(guardianMiniboss.FlyDownSFX))
      AudioManager.Instance.PlayOneShot(guardianMiniboss.FlyDownSFX, guardianMiniboss.transform.position);
    yield return (object) new WaitForSeconds(num * guardianMiniboss.Spine.timeScale);
    if (!string.IsNullOrEmpty(guardianMiniboss.GroundSlamSFX))
      AudioManager.Instance.PlayOneShot(guardianMiniboss.SwordSlashSFX, guardianMiniboss.transform.position);
    BiomeConstants.Instance.EmitHammerEffects(guardianMiniboss.transform.position, 0.5f, 1f, 0.1f);
    guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.TurnOnDamageColliderForDuration(guardianMiniboss.diveBombColliderDuration));
    guardianMiniboss.damageColliderEvents.OnTriggerStayEvent += new ColliderEvents.TriggerEvent(guardianMiniboss.OnDamageTriggerEnter);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
    if ((bool) (UnityEngine.Object) guardianMiniboss.AxeSpecialImpactParticles)
      guardianMiniboss.AxeSpecialImpactParticles.Play();
    guardianMiniboss.health.invincible = false;
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

  public IEnumerator DoSlashSpecialAttack()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    yield return (object) null;
    guardianMiniboss.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(guardianMiniboss.OnDamageTriggerEnter);
    guardianMiniboss.health.invincible = true;
    guardianMiniboss.Spine.AnimationState.SetAnimation(0, guardianMiniboss.IdleAnimation, true);
    guardianMiniboss.LockToGround = false;
    for (int i = 0; i < guardianMiniboss.SwordSpecialSlamCount; ++i)
    {
      guardianMiniboss.LookAtTarget();
      guardianMiniboss.transform.DOKill();
      guardianMiniboss.transform.DOMoveZ(guardianMiniboss.SpecialAttackFlyHeight, guardianMiniboss.SpecialAttackTimeToRise * guardianMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      float progress = 0.0f;
      while ((double) (progress += Time.deltaTime * guardianMiniboss.Spine.timeScale) < (double) guardianMiniboss.SpecialAttackTimeToRise * (double) guardianMiniboss.Spine.timeScale)
      {
        guardianMiniboss.LookAtTarget();
        yield return (object) null;
      }
      guardianMiniboss.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(guardianMiniboss.OnDamageTriggerEnter);
      guardianMiniboss.transform.DOKill();
      guardianMiniboss.transform.DOMoveZ(0.0f, guardianMiniboss.SwordSpecialTimeToGround * guardianMiniboss.Spine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      progress = 0.0f;
      float adjustedDuration = guardianMiniboss.SwordSpecialTimeToGround - guardianMiniboss.swordSpecialDropOffsetTime;
      while ((double) (progress += Time.deltaTime * guardianMiniboss.Spine.timeScale) < (double) adjustedDuration * (double) guardianMiniboss.Spine.timeScale)
      {
        guardianMiniboss.LookAtTarget();
        yield return (object) null;
      }
      if (!string.IsNullOrEmpty(guardianMiniboss.GroundSlamSFX))
        AudioManager.Instance.PlayOneShot(guardianMiniboss.GroundSlamSFX, guardianMiniboss.transform.position);
      BiomeConstants.Instance.EmitHammerEffects(new Vector3(guardianMiniboss.transform.position.x, guardianMiniboss.transform.position.y, 0.0f), 0.5f, 1f, 0.1f);
      guardianMiniboss.LookAtTarget();
      if (!string.IsNullOrEmpty(guardianMiniboss.SwordSlashSFX))
        AudioManager.Instance.PlayOneShot(guardianMiniboss.SwordSlashSFX, guardianMiniboss.transform.position);
      guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.SlashProjectilRoutine(guardianMiniboss.state.LookAngle - guardianMiniboss.SwordSpecialAngleSpread));
      guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.SlashProjectilRoutine(guardianMiniboss.state.LookAngle));
      guardianMiniboss.StartCoroutine((IEnumerator) guardianMiniboss.SlashProjectilRoutine(guardianMiniboss.state.LookAngle + guardianMiniboss.SwordSpecialAngleSpread));
      guardianMiniboss.health.invincible = false;
      yield return (object) new WaitForSeconds(guardianMiniboss.SwordSpecialTimeBetweenSlams);
    }
    guardianMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianMiniboss.LockToGround = false;
    yield return (object) new WaitForSeconds(guardianMiniboss.SpecialAttackRecoveryTime * guardianMiniboss.Spine.timeScale);
  }

  public IEnumerator JumpToEmptySpace()
  {
    EnemyWolfGuardianMiniboss guardianMiniboss = this;
    Vector3 startPosition = guardianMiniboss.transform.position;
    Vector3 targetPosition = guardianMiniboss.GetEmptySpaceInRange();
    Vector3 curve = startPosition + (targetPosition - startPosition) / 2f + Vector3.back * guardianMiniboss.JumpHeight;
    float progress = 0.0f;
    guardianMiniboss.LockToGround = false;
    while ((double) (progress += Time.deltaTime * guardianMiniboss.Spine.timeScale) < (double) guardianMiniboss.JumpDuration)
    {
      Vector3 a = Vector3.Lerp(startPosition, curve, progress / guardianMiniboss.JumpDuration);
      Vector3 b = Vector3.Lerp(curve, targetPosition, progress / guardianMiniboss.JumpDuration);
      guardianMiniboss.transform.position = Vector3.Lerp(a, b, progress / guardianMiniboss.JumpDuration);
      yield return (object) null;
    }
    targetPosition.z = 0.0f;
    guardianMiniboss.transform.position = targetPosition;
    guardianMiniboss.LockToGround = true;
  }

  public Vector3 GetEmptySpaceInRange()
  {
    Vector3 emptySpaceInRange = this.transform.position;
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = UnityEngine.Random.Range(this.JumpAwayMinDistance, this.JumpAwayMaxDistance);
      Vector3 vector3 = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.TargetObject.transform.position, this.circleCastRadius, (Vector2) Vector3.Normalize(vector3 - this.TargetObject.transform.position), distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if ((double) Vector3.Distance(this.TargetObject.transform.position, (Vector3) raycastHit2D.centroid) > (double) this.JumpAwayMinDistance)
        {
          emptySpaceInRange = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.TargetObject.transform.position - vector3) * this.circleCastOffset;
          break;
        }
      }
      else
      {
        emptySpaceInRange = vector3;
        break;
      }
    }
    return emptySpaceInRange;
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !component.IsCharmedEnemy)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__96_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedEnemyAddressable = obj;
    obj.Result.CreatePool(24, true);
  }

  public enum BossPhase
  {
    Zero,
    One,
    Two,
  }
}
