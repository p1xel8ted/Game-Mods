// Decompiled with JetBrains decompiler
// Type: EnemyWoodmanBig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
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
public class EnemyWoodmanBig : UnitObject
{
  public bool isOn;
  public float ActivationRange = 2f;
  public int faithAmmoOnHit = 1;
  public int NewPositionDistance = 3;
  public float MaintainTargetDistance = 4.5f;
  public float MoveCloserDistance = 4f;
  public float AttackWithinRange = 4f;
  public GameObject FireFX;
  public float followPlayerRepathTime = 0.2f;
  public float chaseDelay = 0.3f;
  public bool DoubleAttack = true;
  public bool ChargeAndAttack = true;
  public Vector2 MaxAttackDelayRandomRange = new Vector2(4f, 6f);
  public Vector2 AttackDelayRandomRange = new Vector2(0.5f, 2f);
  public ColliderEvents damageColliderEvents;
  [SerializeField]
  public bool requireLineOfSite = true;
  [SerializeField]
  public float jumpDiveMaxRange = 3f;
  [SerializeField]
  public float jumpDiveMinDistance = 1f;
  [SerializeField]
  public float jumpDiveDuration = 6f;
  [SerializeField]
  public float diveBombArcHeight = 4f;
  [SerializeField]
  public float jumpRecoveryTime = 1f;
  public static Vector2 timeRangeToTheNextJump = new Vector2(3f, 6f);
  public static float lastJumpTime = 0.0f;
  public static float timeToTheNextJump = 0.0f;
  public bool isJumpDiving;
  public AssetReferenceGameObject woodmanSmallPrefab;
  public int woodmanOnDeathCount = 3;
  public float woodmanSpawnRadius = 1f;
  public GameObject loadedWoodmanSmall;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SerializeField]
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string baseSkin;
  [SerializeField]
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string inactiveSkin;
  [SerializeField]
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string rageSkin;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string wakeUpAnim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string walkAnim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string rageModeAnim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackChargeAnim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackCharge2Anim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackImpactAnim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackImpact2Anim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string rebuildAnim;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string resurrectAnim;
  public ParticleSystem particlesRunes;
  public SpawnParticles particlesSnow;
  public SpawnParticles particlesBreak;
  public SpriteRenderer spriteSnow;
  [SerializeField]
  public GameObject deathDestroyVFX;
  public float CircleCastOffset = 1f;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/woodman/death_vo";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/woodman/gethit_vo";
  [EventRef]
  public string AwakeSFX = "event:/dlc/dungeon05/enemy/woodman/mv_awake";
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string RegenerateSFX = "event:/dlc/dungeon05/enemy/woodman/mv_regenerate";
  [EventRef]
  public string BreakSFX = "event:/dlc/dungeon05/enemy/woodman/mv_break";
  [EventRef]
  public string AttackBiteStartSFX = "event:/dlc/dungeon05/enemy/woodman/attack_bite_start";
  [EventRef]
  public string AttackBiteSwipeSFX = "event:/dlc/dungeon05/enemy/woodman/attack_bite_swipe";
  [EventRef]
  public string AttackJumpStartSFX = "event:/dlc/dungeon05/enemy/woodman/attack_jump_start";
  [EventRef]
  public string AttackJumpLaunchSFX = "event:/dlc/dungeon05/enemy/woodman/attack_jump_launch";
  [EventRef]
  public string AttackJumpLandSFX = "event:/dlc/dungeon05/enemy/woodman/attack_jump_land";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string PanicAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  public EventInstance regenerateInstanceSFX;
  public float AttackDelay;
  public bool canBeParried;
  public static float signPostParryWindow = 0.2f;
  public static float attackParryWindow = 0.15f;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public bool \u003CFollowPlayer\u003Ek__BackingField;
  public static List<EnemyWoodmanBig> woodmen = new List<EnemyWoodmanBig>();
  public bool hasBeenHit;
  public Coroutine hurtRoutine;
  public Coroutine activatingRoutine;
  public Vector3 Force;
  public float KnockbackModifier = 1f;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public EnemyWoodmanBig.State MyState;
  public float MaxAttackDelay;
  public Health EnemyHealth;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  public Coroutine jumpDiveRoutine;

  public float Damage
  {
    get => this.\u003CDamage\u003Ek__BackingField;
    set => this.\u003CDamage\u003Ek__BackingField = value;
  }

  public bool FollowPlayer
  {
    get => this.\u003CFollowPlayer\u003Ek__BackingField;
    set => this.\u003CFollowPlayer\u003Ek__BackingField = value;
  }

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
    Addressables.LoadAssetAsync<GameObject>((object) this.woodmanSmallPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedWoodmanSmall = obj.Result;
      this.loadedWoodmanSmall.CreatePool(this.woodmanOnDeathCount, true);
    });
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnEnable()
  {
    EnemyWoodmanBig.woodmen.Add(this);
    if (!this.isOn)
    {
      this.Spine.skeleton.SetSkin(this.inactiveSkin);
      this.Spine.Skeleton.SetSlotsToSetupPose();
    }
    this.SeperateObject = true;
    base.OnEnable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    if (this.state.CURRENT_STATE != StateMachine.State.Dead)
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
  }

  public override void OnDisable()
  {
    EnemyWoodmanBig.woodmen.Remove(this);
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    AudioManager.Instance.StopOneShotInstanceEarly(this.regenerateInstanceSFX, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    ObjectPool.Spawn(this.deathDestroyVFX, this.transform.parent, this.transform.position);
    this.SpawnWoodmen();
  }

  public void SpawnWoodmen()
  {
    for (int index = 0; index < this.woodmanOnDeathCount; ++index)
    {
      float f = (float) ((double) index * (360.0 / (double) this.woodmanOnDeathCount) * (Math.PI / 180.0));
      Vector3 closestPoint;
      BiomeGenerator.PointWithinIsland(this.transform.position + new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f) * this.woodmanSpawnRadius, out closestPoint);
      Health component = ObjectPool.Spawn(this.loadedWoodmanSmall, this.transform.parent, closestPoint).GetComponent<Health>();
      Interaction_Chest.Instance?.AddEnemy(component);
      EnemyRoundsBase.Instance?.AddEnemyToRound(component);
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.hasBeenHit = true;
    PlayerFarming component = Attacker.GetComponent<PlayerFarming>();
    if ((bool) (UnityEngine.Object) component)
      component.GetBlackSoul(this.faithAmmoOnHit);
    if (this.health.HasShield || this.health.WasJustParried)
      return;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.Spine.AnimationState.SetAnimation(1, "hurt-eyes", false);
    if (this.MyState != EnemyWoodmanBig.State.Attacking)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.isJumpDiving = false;
      this.LockToGround = true;
      this.StopAllCoroutines();
      this.DisableForces = false;
      if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
        this.state.CURRENT_STATE = StateMachine.State.HitRight;
      if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
        this.state.CURRENT_STATE = StateMachine.State.HitLeft;
      if (AttackType != Health.AttackTypes.Heavy && (!(AttackType == Health.AttackTypes.Projectile & FromBehind) || this.health.HasShield))
      {
        if (this.hurtRoutine != null)
          this.StopCoroutine(this.hurtRoutine);
        this.hurtRoutine = this.StartCoroutine((IEnumerator) this.HurtRoutine());
      }
    }
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    if (AttackType != Health.AttackTypes.NoKnockBack)
    {
      if (this.jumpDiveRoutine != null)
        this.StopJumpDiveAttackEarly();
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    }
    this.SimpleSpineFlash.FlashFillRed();
  }

  public void ActivateWoodman()
  {
    if (this.activatingRoutine != null)
      return;
    this.activatingRoutine = this.StartCoroutine((IEnumerator) this.ActivateWoodmanRoutine());
  }

  public IEnumerator ActivateWoodmanRoutine()
  {
    EnemyWoodmanBig enemyWoodmanBig = this;
    enemyWoodmanBig.particlesRunes.Play();
    enemyWoodmanBig.particlesSnow.Spawn(90f, 360f);
    enemyWoodmanBig.spriteSnow.gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(enemyWoodmanBig.AwakeSFX))
      AudioManager.Instance.PlayOneShot(enemyWoodmanBig.AwakeSFX, enemyWoodmanBig.transform.position);
    enemyWoodmanBig.FireFX.SetActive(true);
    if (enemyWoodmanBig.health.HasShield)
      enemyWoodmanBig.Spine.Skeleton.SetSkin(enemyWoodmanBig.baseSkin);
    else
      enemyWoodmanBig.Spine.Skeleton.SetSkin(enemyWoodmanBig.rageSkin);
    enemyWoodmanBig.Spine.Skeleton.SetSlotsToSetupPose();
    enemyWoodmanBig.isOn = true;
    TrackEntry trackEntry = enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.wakeUpAnim, false);
    enemyWoodmanBig.Spine.AnimationState.AddAnimation(0, enemyWoodmanBig.idleAnim, true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(trackEntry.Animation.Duration, enemyWoodmanBig.Spine);
    enemyWoodmanBig.activatingRoutine = (Coroutine) null;
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyWoodmanBig enemyWoodmanBig = this;
    enemyWoodmanBig.DisableForces = true;
    enemyWoodmanBig.Force = (enemyWoodmanBig.transform.position - Attacker.transform.position).normalized * 500f;
    enemyWoodmanBig.rb.AddForce((Vector2) (enemyWoodmanBig.Force * enemyWoodmanBig.KnockbackModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWoodmanBig.Spine.timeScale) < 0.5)
      yield return (object) null;
    Vector3 closestPoint;
    if (!BiomeGenerator.PointWithinIsland(enemyWoodmanBig.transform.position, out closestPoint))
      enemyWoodmanBig.transform.position = closestPoint - closestPoint.normalized;
    enemyWoodmanBig.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemyWoodmanBig enemyWoodmanBig = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWoodmanBig.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyWoodmanBig.StartCoroutine((IEnumerator) enemyWoodmanBig.WaitForTarget());
    enemyWoodmanBig.hurtRoutine = (Coroutine) null;
  }

  public IEnumerator WaitForTarget()
  {
    EnemyWoodmanBig enemyWoodmanBig = this;
    enemyWoodmanBig.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    bool InRange = false;
    while (!InRange)
    {
      float distanceToTarget = float.MaxValue;
      do
      {
        distanceToTarget = Vector3.Distance(enemyWoodmanBig.GetClosestTarget().transform.position, enemyWoodmanBig.transform.position);
        if (!enemyWoodmanBig.isOn && ((double) distanceToTarget <= (double) enemyWoodmanBig.ActivationRange || enemyWoodmanBig.hasBeenHit))
        {
          enemyWoodmanBig.ActivateAllWoodmen();
          enemyWoodmanBig.ActivateWoodman();
          yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWoodmanBig.Spine.skeleton.Data.FindAnimation(enemyWoodmanBig.wakeUpAnim).Duration, enemyWoodmanBig.Spine);
        }
        yield return (object) null;
      }
      while (!enemyWoodmanBig.isOn);
      foreach (UnitObject woodman in EnemyWoodmanBig.woodmen)
      {
        if (woodman.state.CURRENT_STATE == StateMachine.State.Dead)
        {
          InRange = true;
          break;
        }
      }
      if ((double) distanceToTarget <= (double) enemyWoodmanBig.VisionRange)
      {
        if (!enemyWoodmanBig.requireLineOfSite || enemyWoodmanBig.CheckLineOfSightOnTarget(enemyWoodmanBig.GetClosestTarget().gameObject, enemyWoodmanBig.GetClosestTarget().transform.position, Mathf.Min(distanceToTarget, (float) enemyWoodmanBig.VisionRange)))
          InRange = true;
        else
          enemyWoodmanBig.LookAtTarget();
      }
      yield return (object) null;
    }
    enemyWoodmanBig.FindPath(enemyWoodmanBig.TargetPosition);
    enemyWoodmanBig.StartCoroutine((IEnumerator) enemyWoodmanBig.ChasePlayer());
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    if (!(this.Spine.AnimationName != this.walkAnim))
      return;
    this.Spine.randomOffset = true;
    this.Spine.AnimationState.SetAnimation(0, this.walkAnim, true);
  }

  public IEnumerator ChasePlayer()
  {
    EnemyWoodmanBig enemyWoodmanBig = this;
    enemyWoodmanBig.ResetJumpCooldown();
    enemyWoodmanBig.MyState = EnemyWoodmanBig.State.WaitAndTaunt;
    enemyWoodmanBig.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyWoodmanBig.AttackDelay = UnityEngine.Random.Range(enemyWoodmanBig.AttackDelayRandomRange.x, enemyWoodmanBig.AttackDelayRandomRange.y);
    if (enemyWoodmanBig.health.HasShield)
      enemyWoodmanBig.AttackDelay = 2.5f;
    enemyWoodmanBig.MaxAttackDelay = UnityEngine.Random.Range(enemyWoodmanBig.MaxAttackDelayRandomRange.x, enemyWoodmanBig.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyWoodmanBig.damageColliderEvents != (UnityEngine.Object) null)
        enemyWoodmanBig.damageColliderEvents.SetActive(false);
      enemyWoodmanBig.AttackDelay -= Time.deltaTime * enemyWoodmanBig.Spine.timeScale;
      enemyWoodmanBig.MaxAttackDelay -= Time.deltaTime * enemyWoodmanBig.Spine.timeScale;
      if (enemyWoodmanBig.MyState == EnemyWoodmanBig.State.WaitAndTaunt)
      {
        if (enemyWoodmanBig.Spine.AnimationName != "roll-stop" && enemyWoodmanBig.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyWoodmanBig.Spine.AnimationName != enemyWoodmanBig.walkAnim)
            enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.walkAnim, true);
        }
        else if (enemyWoodmanBig.Spine.AnimationName != enemyWoodmanBig.idleAnim)
          enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.idleAnim, true);
        enemyWoodmanBig.state.LookAngle = Utils.GetAngle(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position);
        enemyWoodmanBig.Spine.skeleton.ScaleX = (double) enemyWoodmanBig.state.LookAngle <= 90.0 || (double) enemyWoodmanBig.state.LookAngle >= 270.0 ? -1f : 1f;
        float num = Vector3.Distance(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position);
        if (enemyWoodmanBig.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          if ((double) (enemyWoodmanBig.RepathTimer -= Time.deltaTime * enemyWoodmanBig.Spine.timeScale) < 0.0)
          {
            if ((double) enemyWoodmanBig.MaxAttackDelay < 0.0 || (double) num < (double) enemyWoodmanBig.AttackWithinRange)
            {
              enemyWoodmanBig.AttackWithinRange = float.MaxValue;
              if ((bool) (UnityEngine.Object) enemyWoodmanBig.GetClosestTarget())
              {
                if (enemyWoodmanBig.ChargeAndAttack && ((double) enemyWoodmanBig.MaxAttackDelay < 0.0 || (double) enemyWoodmanBig.AttackDelay < 0.0))
                {
                  enemyWoodmanBig.StopAllCoroutines();
                  enemyWoodmanBig.DisableForces = false;
                  enemyWoodmanBig.StartCoroutine((IEnumerator) enemyWoodmanBig.FightPlayer());
                }
                else if (!enemyWoodmanBig.health.HasShield)
                {
                  enemyWoodmanBig.Angle = (float) (((double) Utils.GetAngle(enemyWoodmanBig.GetClosestTarget().transform.position, enemyWoodmanBig.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                  enemyWoodmanBig.TargetPosition = enemyWoodmanBig.GetClosestTarget().transform.position + new Vector3(enemyWoodmanBig.MaintainTargetDistance * Mathf.Cos(enemyWoodmanBig.Angle), enemyWoodmanBig.MaintainTargetDistance * Mathf.Sin(enemyWoodmanBig.Angle));
                  enemyWoodmanBig.FindPath(enemyWoodmanBig.TargetPosition);
                }
              }
            }
            else if ((bool) (UnityEngine.Object) enemyWoodmanBig.GetClosestTarget() && (double) Vector3.Distance(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position) > (double) enemyWoodmanBig.MoveCloserDistance + (enemyWoodmanBig.health.HasShield ? 0.0 : 1.0))
            {
              enemyWoodmanBig.Angle = (float) (((double) Utils.GetAngle(enemyWoodmanBig.GetClosestTarget().transform.position, enemyWoodmanBig.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemyWoodmanBig.TargetPosition = enemyWoodmanBig.GetClosestTarget().transform.position + new Vector3(enemyWoodmanBig.MaintainTargetDistance * Mathf.Cos(enemyWoodmanBig.Angle), enemyWoodmanBig.MaintainTargetDistance * Mathf.Sin(enemyWoodmanBig.Angle));
              enemyWoodmanBig.FindPath(enemyWoodmanBig.TargetPosition);
            }
          }
        }
        else if ((double) (enemyWoodmanBig.RepathTimer += Time.deltaTime * enemyWoodmanBig.Spine.timeScale) > 2.0)
        {
          enemyWoodmanBig.RepathTimer = 0.0f;
          enemyWoodmanBig.state.CURRENT_STATE = StateMachine.State.Idle;
        }
        if ((double) Time.time >= (double) EnemyWoodmanBig.lastJumpTime + (double) EnemyWoodmanBig.timeToTheNextJump && (double) num >= (double) enemyWoodmanBig.jumpDiveMinDistance)
        {
          enemyWoodmanBig.DoJumpDiveAttack();
          yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWoodmanBig.Spine);
        }
      }
      enemyWoodmanBig.Seperate(0.5f);
      yield return (object) null;
    }
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    float a = Vector3.Distance(TargetObject.transform.position, this.transform.position);
    if ((double) a > (double) this.VisionRange)
      return;
    if (!this.requireLineOfSite || this.CheckLineOfSightOnTarget(TargetObject, TargetObject.transform.position, Mathf.Min(a, (float) this.VisionRange)))
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    else
      this.LookAtTarget();
  }

  public void FindPath(Vector3 PointToCheck)
  {
    this.RepathTimer = 0.2f;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.NewPositionDistance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) <= 1.0)
        return;
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset;
      this.givePath(this.TargetPosition);
    }
    else if (this.FollowPlayer && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) <= 1.5)
        return;
      this.TargetPosition = PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle;
      this.givePath(this.TargetPosition);
    }
    else
    {
      this.TargetPosition = PointToCheck;
      this.givePath(PointToCheck);
    }
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }

  public IEnumerator FightPlayer(float AttackDistance = 1.5f)
  {
    EnemyWoodmanBig enemyWoodmanBig = this;
    enemyWoodmanBig.MyState = EnemyWoodmanBig.State.Attacking;
    enemyWoodmanBig.UsePathing = true;
    enemyWoodmanBig.givePath(enemyWoodmanBig.GetClosestTarget().transform.position);
    enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.walkAnim, true);
    float distanceResetTimer = 0.0f;
    Vector2 previousPosition = (Vector2) enemyWoodmanBig.transform.position;
    enemyWoodmanBig.RepathTimer = 0.0f;
    int NumAttacks = enemyWoodmanBig.DoubleAttack ? 2 : 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = 0.5f;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyWoodmanBig.Spine == (UnityEngine.Object) null || enemyWoodmanBig.Spine.AnimationState == null || enemyWoodmanBig.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        enemyWoodmanBig.Seperate(0.5f);
        switch (enemyWoodmanBig.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            enemyWoodmanBig.StartCoroutine((IEnumerator) enemyWoodmanBig.WaitForTarget());
            yield break;
          case StateMachine.State.Moving:
            if ((bool) (UnityEngine.Object) enemyWoodmanBig.GetClosestTarget())
            {
              enemyWoodmanBig.state.LookAngle = Utils.GetAngle(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position);
              enemyWoodmanBig.Spine.skeleton.ScaleX = (double) enemyWoodmanBig.state.LookAngle <= 90.0 || (double) enemyWoodmanBig.state.LookAngle >= 270.0 ? -1f : 1f;
              enemyWoodmanBig.state.LookAngle = enemyWoodmanBig.state.facingAngle = Utils.GetAngle(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position);
            }
            if ((UnityEngine.Object) enemyWoodmanBig.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) enemyWoodmanBig.transform.position, (Vector2) enemyWoodmanBig.GetClosestTarget().transform.position) < (double) AttackDistance)
            {
              enemyWoodmanBig.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? enemyWoodmanBig.attackCharge2Anim : enemyWoodmanBig.attackChargeAnim, false);
              if (!string.IsNullOrEmpty(enemyWoodmanBig.AttackBiteStartSFX))
                AudioManager.Instance.PlayOneShot(enemyWoodmanBig.AttackBiteStartSFX, enemyWoodmanBig.gameObject);
            }
            else
            {
              if ((double) (enemyWoodmanBig.RepathTimer += Time.deltaTime * enemyWoodmanBig.Spine.timeScale) > 0.20000000298023224 && (bool) (UnityEngine.Object) enemyWoodmanBig.GetClosestTarget())
              {
                enemyWoodmanBig.RepathTimer = 0.0f;
                enemyWoodmanBig.givePath(enemyWoodmanBig.GetClosestTarget().transform.position);
              }
              if (enemyWoodmanBig.FollowPlayer && (bool) (UnityEngine.Object) enemyWoodmanBig.GetClosestTarget())
              {
                double magnitude = (double) ((Vector2) enemyWoodmanBig.transform.position - previousPosition).magnitude;
                previousPosition = (Vector2) enemyWoodmanBig.transform.position;
                if (magnitude <= 0.14000000059604645)
                {
                  distanceResetTimer += Time.deltaTime * enemyWoodmanBig.Spine.timeScale;
                  if ((double) distanceResetTimer > 6.5)
                    enemyWoodmanBig.transform.position = PlayerFarming.Instance.transform.position;
                }
                else
                  distanceResetTimer = 0.0f;
              }
              if ((UnityEngine.Object) enemyWoodmanBig.damageColliderEvents != (UnityEngine.Object) null)
              {
                if ((double) enemyWoodmanBig.state.Timer < 0.20000000298023224 && !enemyWoodmanBig.health.WasJustParried)
                  enemyWoodmanBig.damageColliderEvents.SetActive(true);
                else
                  enemyWoodmanBig.damageColliderEvents.SetActive(false);
              }
            }
            if ((UnityEngine.Object) enemyWoodmanBig.damageColliderEvents != (UnityEngine.Object) null)
            {
              enemyWoodmanBig.damageColliderEvents.SetActive(false);
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyWoodmanBig.damageColliderEvents != (UnityEngine.Object) null)
              enemyWoodmanBig.damageColliderEvents.SetActive(false);
            enemyWoodmanBig.SimpleSpineFlash.FlashWhite(enemyWoodmanBig.state.Timer / SignPostDelay);
            enemyWoodmanBig.state.Timer += Time.deltaTime * enemyWoodmanBig.Spine.timeScale;
            if ((double) enemyWoodmanBig.state.Timer >= (double) SignPostDelay - (double) EnemyWoodmanBig.signPostParryWindow)
              enemyWoodmanBig.canBeParried = true;
            if ((double) enemyWoodmanBig.state.Timer >= (double) SignPostDelay)
            {
              enemyWoodmanBig.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, enemyWoodmanBig.state.LookAngle);
              enemyWoodmanBig.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              enemyWoodmanBig.speed = AttackSpeed * 0.0166666675f;
              enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? enemyWoodmanBig.attackImpact2Anim : enemyWoodmanBig.attackImpactAnim, false);
              enemyWoodmanBig.canBeParried = true;
              enemyWoodmanBig.StartCoroutine((IEnumerator) enemyWoodmanBig.EnableDamageCollider(0.0f));
              enemyWoodmanBig.DoKnockBack(enemyWoodmanBig.GetClosestTarget().gameObject, -1f, 1f);
              if (!string.IsNullOrEmpty(enemyWoodmanBig.AttackBiteSwipeSFX))
                AudioManager.Instance.PlayOneShot(enemyWoodmanBig.AttackBiteSwipeSFX, enemyWoodmanBig.transform.position);
              if (!string.IsNullOrEmpty(enemyWoodmanBig.AttackVO))
              {
                AudioManager.Instance.PlayOneShot(enemyWoodmanBig.AttackVO, enemyWoodmanBig.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime * enemyWoodmanBig.Spine.timeScale;
            enemyWoodmanBig.speed = AttackSpeed * Time.deltaTime * enemyWoodmanBig.Spine.timeScale;
            enemyWoodmanBig.SimpleSpineFlash.FlashWhite(false);
            enemyWoodmanBig.canBeParried = (double) enemyWoodmanBig.state.Timer <= (double) EnemyWoodmanBig.attackParryWindow;
            if ((double) (enemyWoodmanBig.state.Timer += Time.deltaTime * enemyWoodmanBig.Spine.timeScale) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
            {
              if (++AttackCount <= NumAttacks)
              {
                AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
                enemyWoodmanBig.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.attackChargeAnim, false);
                SignPostDelay = 0.3f;
                break;
              }
              Loop = false;
              enemyWoodmanBig.SimpleSpineFlash.FlashWhite(false);
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    enemyWoodmanBig.StartCoroutine((IEnumerator) enemyWoodmanBig.WaitForTarget());
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.FollowPlayer)
      return;
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null))
      return;
    if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  public IEnumerator EnableDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.damageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.damageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      this.damageColliderEvents.SetActive(false);
    }
  }

  public void DoJumpDiveAttack()
  {
    if (this.jumpDiveRoutine != null)
      return;
    this.jumpDiveRoutine = this.StartCoroutine((IEnumerator) this.JumpDiveRoutine());
  }

  public void StopJumpDiveAttackEarly()
  {
    if (this.jumpDiveRoutine != null)
      this.StopCoroutine(this.jumpDiveRoutine);
    this.Spine.AnimationState.SetAnimation(0, this.walkAnim, true);
    this.jumpDiveRoutine = (Coroutine) null;
    this.SimpleSpineFlash.FlashWhite(false);
    this.SeperateObject = true;
    this.UsePathing = true;
    this.isJumpDiving = false;
    this.LockToGround = true;
  }

  public IEnumerator JumpDiveRoutine()
  {
    EnemyWoodmanBig enemyWoodmanBig = this;
    EnemyWoodmanBig.lastJumpTime = Time.time;
    enemyWoodmanBig.ResetJumpCooldown();
    enemyWoodmanBig.isJumpDiving = true;
    enemyWoodmanBig.ClearPaths();
    enemyWoodmanBig.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.wakeUpAnim, false);
    float anticipationDuration = enemyWoodmanBig.Spine.Skeleton.Data.FindAnimation(enemyWoodmanBig.wakeUpAnim).Duration * enemyWoodmanBig.Spine.timeScale;
    float signpostTime = 0.0f;
    if (!string.IsNullOrEmpty(enemyWoodmanBig.AttackJumpStartSFX))
      AudioManager.Instance.PlayOneShot(enemyWoodmanBig.AttackJumpStartSFX, enemyWoodmanBig.transform.position);
    while ((double) enemyWoodmanBig.Spine.timeScale == 9.9999997473787516E-05)
      yield return (object) null;
    while ((double) (signpostTime += Time.deltaTime * enemyWoodmanBig.Spine.timeScale) < (double) anticipationDuration)
    {
      enemyWoodmanBig.SimpleSpineFlash.FlashWhite(signpostTime / anticipationDuration);
      enemyWoodmanBig.state.LookAngle = Utils.GetAngle(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position);
      enemyWoodmanBig.state.facingAngle = Utils.GetAngle(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position);
      yield return (object) null;
    }
    enemyWoodmanBig.SimpleSpineFlash.FlashWhite(false);
    enemyWoodmanBig.state.LookAngle = Utils.GetAngle(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position);
    enemyWoodmanBig.state.facingAngle = Utils.GetAngle(enemyWoodmanBig.transform.position, enemyWoodmanBig.GetClosestTarget().transform.position);
    enemyWoodmanBig.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (!string.IsNullOrEmpty(enemyWoodmanBig.AttackJumpLaunchSFX))
      AudioManager.Instance.PlayOneShot(enemyWoodmanBig.AttackJumpLaunchSFX, enemyWoodmanBig.transform.position);
    enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.idleAnim, false);
    Vector3 startPosition = enemyWoodmanBig.transform.position;
    Vector3 targetPosition = enemyWoodmanBig.GetClosestTarget().transform.position;
    if ((double) Vector2.Distance((Vector2) targetPosition, (Vector2) startPosition) > (double) enemyWoodmanBig.jumpDiveMaxRange)
      targetPosition = startPosition + (targetPosition - startPosition).normalized * enemyWoodmanBig.jumpDiveMaxRange;
    double jumpDiveDuration = (double) enemyWoodmanBig.jumpDiveDuration;
    float progress = 0.0f;
    Vector3 curve = startPosition + (targetPosition - startPosition) / 2f + Vector3.back * enemyWoodmanBig.diveBombArcHeight;
    enemyWoodmanBig.LockToGround = false;
    while ((double) (progress += Time.deltaTime * enemyWoodmanBig.Spine.timeScale) < (double) enemyWoodmanBig.jumpDiveDuration)
    {
      float t = progress / enemyWoodmanBig.jumpDiveDuration;
      Vector3 a = Vector3.Lerp(startPosition, curve, t);
      Vector3 b = Vector3.Lerp(curve, targetPosition, t);
      enemyWoodmanBig.transform.position = Vector3.Lerp(a, b, t);
      yield return (object) null;
    }
    targetPosition.z = 0.0f;
    enemyWoodmanBig.transform.position = targetPosition;
    if (!string.IsNullOrEmpty(enemyWoodmanBig.AttackJumpLandSFX))
      AudioManager.Instance.PlayOneShot(enemyWoodmanBig.AttackJumpLandSFX, enemyWoodmanBig.gameObject);
    yield return (object) enemyWoodmanBig.StartCoroutine((IEnumerator) enemyWoodmanBig.JumpRecoveryRoutine());
    enemyWoodmanBig.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyWoodmanBig.isJumpDiving = false;
    enemyWoodmanBig.FindPath(enemyWoodmanBig.TargetPosition);
    enemyWoodmanBig.jumpDiveRoutine = (Coroutine) null;
  }

  public IEnumerator JumpRecoveryRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWoodmanBig enemyWoodmanBig = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.idleAnim, true);
      enemyWoodmanBig.LockToGround = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyWoodmanBig.LockToGround = true;
    enemyWoodmanBig.Spine.AnimationState.SetAnimation(0, enemyWoodmanBig.wakeUpAnim, false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyWoodmanBig.jumpRecoveryTime);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ResetJumpCooldown()
  {
    EnemyWoodmanBig.timeToTheNextJump = UnityEngine.Random.Range(EnemyWoodmanBig.timeRangeToTheNextJump.x, EnemyWoodmanBig.timeRangeToTheNextJump.y);
  }

  public void ActivateAllWoodmen()
  {
    foreach (EnemyWoodmanBig woodman in EnemyWoodmanBig.woodmen)
    {
      if (!woodman.isOn)
        woodman.ActivateWoodman();
    }
    foreach (EnemyWoodman woodman in EnemyWoodman.woodmen)
    {
      if (!woodman.isOn)
        woodman.ActivateWoodman();
    }
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__81_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedWoodmanSmall = obj.Result;
    this.loadedWoodmanSmall.CreatePool(this.woodmanOnDeathCount, true);
  }

  public enum State
  {
    WaitAndTaunt,
    Attacking,
    Resurrecting,
  }
}
