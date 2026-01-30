// Decompiled with JetBrains decompiler
// Type: EnemyWoodman
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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

#nullable disable
public class EnemyWoodman : UnitObject
{
  public bool isOn;
  public float ActivationRange = 2f;
  [SerializeField]
  public DropLootOnDeath dropLootOnDeath;
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
  [SerializeField]
  public SimpleRadialProgressBar resurrectProgressBar;
  [Range(0.01f, 1f)]
  [SerializeField]
  public float resurrectHealthPrecentage = 0.5f;
  [SerializeField]
  public float resurrectionAwaitTimeHarderMode = 5f;
  [SerializeField]
  public float resurrectionAwaitTimeEasyMode = 10f;
  public float resurrectionAwaitTime = 5f;
  public float currentResurrectionTimer;
  public float resurrectionAnticipationTimer = 1f;
  public bool permanentlyDead;
  public ShowHPBar showHPBar;
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
  public static List<EnemyWoodman> woodmen = new List<EnemyWoodman>();
  public bool hasBeenHit;
  public Coroutine hurtRoutine;
  public Coroutine activatingRoutine;
  public Vector3 Force;
  public float KnockbackModifier = 1f;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public EnemyWoodman.State MyState;
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
      this.health.totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    this.health.CanIncreaseDamageMultiplier = false;
    this.resurrectionAwaitTime = DifficultyManager.PrimaryDifficulty != DifficultyManager.Difficulty.Easy ? this.resurrectionAwaitTimeHarderMode : this.resurrectionAwaitTimeEasyMode;
    this.showHPBar = this.GetComponent<ShowHPBar>();
    this.resurrectProgressBar.Hide(true);
    if (!((UnityEngine.Object) this.dropLootOnDeath != (UnityEngine.Object) null))
      return;
    this.dropLootOnDeath.enabled = false;
    this.dropLootOnDeath.SetHealth();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnEnable()
  {
    EnemyWoodman.woodmen.Add(this);
    if (!this.isOn)
    {
      if (this.health.HasShield)
        this.Spine.skeleton.SetSkin("off-" + UnityEngine.Random.Range(1, 3).ToString());
      else
        this.Spine.skeleton.SetSkin($"off-{UnityEngine.Random.Range(1, 3).ToString()}-no-shield");
      this.Spine.Skeleton.SetSlotsToSetupPose();
    }
    this.SeperateObject = true;
    base.OnEnable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    if (this.MyState == EnemyWoodman.State.Resurrecting)
      this.StartCoroutine((IEnumerator) this.ResurrectRoutine());
    else if (this.state.CURRENT_STATE != StateMachine.State.Dead)
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
  }

  public override void OnDisable()
  {
    EnemyWoodman.woodmen.Remove(this);
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
  }

  public override void Update()
  {
    base.Update();
    if (this.state.CURRENT_STATE != StateMachine.State.Dead)
      return;
    this.currentResurrectionTimer += Time.deltaTime * this.Spine.timeScale;
    float resurrectionProgress = this.currentResurrectionTimer / this.resurrectionAwaitTime;
    this.UpdateResurrectProgress(resurrectionProgress);
    this.PulseResurrectionUI(resurrectionProgress);
    if ((double) this.currentResurrectionTimer >= (double) this.resurrectionAwaitTime - (double) this.resurrectionAnticipationTimer && this.MyState != EnemyWoodman.State.Resurrecting)
      this.StartCoroutine((IEnumerator) this.ResurrectRoutine());
    if (!this.CanKillAllWoodmen())
      return;
    this.TryKillAllWoodmen();
  }

  public override void OnDieEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDieEarly(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!this.permanentlyDead)
      return;
    this.health.CanIncreaseDamageMultiplier = true;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StopAllCoroutines();
    this.LockToGround = true;
    this.DisableForces = false;
    this.SeperateObject = false;
    this.isJumpDiving = false;
    this.Spine.ClearState();
    this.Spine.Skeleton.SetSkin(this.inactiveSkin);
    this.Spine.Skeleton.SetSlotsToSetupPose();
    this.FireFX.SetActive(false);
    this.speed = 0.0f;
    this.rb.velocity = (Vector2) Vector3.zero;
    this.ClearPaths();
    if (this.permanentlyDead)
    {
      if (!string.IsNullOrEmpty(this.DeathVO))
        AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
      AudioManager.Instance.StopOneShotInstanceEarly(this.regenerateInstanceSFX, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
      ObjectPool.Spawn(this.deathDestroyVFX, this.transform.parent, this.transform.position);
      this.dropLootOnDeath?.Play(Attacker);
    }
    else
    {
      this.currentResurrectionTimer = 0.0f;
      this.seperatorVX = 0.0f;
      this.seperatorVY = 0.0f;
      this.state.facingAngle = this.state.LookAngle = 270f;
      this.resurrectProgressBar.Show();
      this.Spine.AnimationState.SetAnimation(0, this.rebuildAnim, true);
      this.particlesBreak.Spawn(90f, 360f);
      if (!string.IsNullOrEmpty(this.BreakSFX))
        AudioManager.Instance.PlayOneShot(this.BreakSFX, this.gameObject);
      if (string.IsNullOrEmpty(this.RegenerateSFX))
        return;
      this.regenerateInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.RegenerateSFX, this.transform);
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
    if (this.MyState != EnemyWoodman.State.Attacking)
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

  public void OnDestroyShield()
  {
    this.Spine.Skeleton.SetSkin(this.rageSkin);
    this.Spine.Skeleton.SetSlotsToSetupPose();
  }

  public void ActivateWoodman()
  {
    if (this.activatingRoutine != null)
      return;
    this.activatingRoutine = this.StartCoroutine((IEnumerator) this.ActivateWoodmanRoutine());
  }

  public IEnumerator ActivateWoodmanRoutine()
  {
    EnemyWoodman enemyWoodman = this;
    enemyWoodman.particlesRunes.Play();
    enemyWoodman.particlesSnow.Spawn(90f, 360f);
    enemyWoodman.spriteSnow.gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(enemyWoodman.AwakeSFX))
      AudioManager.Instance.PlayOneShot(enemyWoodman.AwakeSFX, enemyWoodman.transform.position);
    enemyWoodman.FireFX.SetActive(true);
    if (enemyWoodman.health.HasShield)
      enemyWoodman.Spine.Skeleton.SetSkin(enemyWoodman.baseSkin);
    else
      enemyWoodman.Spine.Skeleton.SetSkin(enemyWoodman.rageSkin);
    enemyWoodman.Spine.Skeleton.SetSlotsToSetupPose();
    enemyWoodman.isOn = true;
    TrackEntry trackEntry = enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.wakeUpAnim, false);
    enemyWoodman.Spine.AnimationState.AddAnimation(0, enemyWoodman.idleAnim, true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(trackEntry.Animation.Duration, enemyWoodman.Spine);
    enemyWoodman.activatingRoutine = (Coroutine) null;
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyWoodman enemyWoodman = this;
    enemyWoodman.DisableForces = true;
    enemyWoodman.Force = (enemyWoodman.transform.position - Attacker.transform.position).normalized * 500f;
    enemyWoodman.rb.AddForce((Vector2) (enemyWoodman.Force * enemyWoodman.KnockbackModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWoodman.Spine.timeScale) < 0.5)
      yield return (object) null;
    Vector3 closestPoint;
    if (!BiomeGenerator.PointWithinIsland(enemyWoodman.transform.position, out closestPoint))
      enemyWoodman.transform.position = closestPoint - closestPoint.normalized;
    enemyWoodman.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemyWoodman enemyWoodman = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWoodman.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyWoodman.StartCoroutine((IEnumerator) enemyWoodman.WaitForTarget());
  }

  public IEnumerator WaitForTarget()
  {
    EnemyWoodman enemyWoodman = this;
    enemyWoodman.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    bool InRange = false;
    while (!InRange)
    {
      float distanceToTarget = float.MaxValue;
      do
      {
        distanceToTarget = Vector3.Distance(enemyWoodman.GetClosestTarget().transform.position, enemyWoodman.transform.position);
        if (!enemyWoodman.isOn && ((double) distanceToTarget <= (double) enemyWoodman.ActivationRange || enemyWoodman.hasBeenHit))
        {
          enemyWoodman.ActivateAllWoodmen();
          enemyWoodman.ActivateWoodman();
          yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWoodman.Spine.skeleton.Data.FindAnimation(enemyWoodman.wakeUpAnim).Duration, enemyWoodman.Spine);
        }
        yield return (object) null;
      }
      while (!enemyWoodman.isOn);
      foreach (UnitObject woodman in EnemyWoodman.woodmen)
      {
        if (woodman.state.CURRENT_STATE == StateMachine.State.Dead)
        {
          InRange = true;
          break;
        }
      }
      if ((double) distanceToTarget <= (double) enemyWoodman.VisionRange || enemyWoodman.hasBeenHit)
      {
        if (!enemyWoodman.requireLineOfSite || enemyWoodman.CheckLineOfSightOnTarget(enemyWoodman.GetClosestTarget().gameObject, enemyWoodman.GetClosestTarget().transform.position, Mathf.Min(distanceToTarget, (float) enemyWoodman.VisionRange)))
          InRange = true;
        else
          enemyWoodman.LookAtTarget();
      }
      yield return (object) null;
    }
    enemyWoodman.FindPath(enemyWoodman.TargetPosition);
    enemyWoodman.StartCoroutine((IEnumerator) enemyWoodman.ChasePlayer());
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
    EnemyWoodman enemyWoodman = this;
    enemyWoodman.ResetJumpCooldown();
    enemyWoodman.MyState = EnemyWoodman.State.WaitAndTaunt;
    enemyWoodman.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyWoodman.AttackDelay = UnityEngine.Random.Range(enemyWoodman.AttackDelayRandomRange.x, enemyWoodman.AttackDelayRandomRange.y);
    if (enemyWoodman.health.HasShield)
      enemyWoodman.AttackDelay = 2.5f;
    enemyWoodman.MaxAttackDelay = UnityEngine.Random.Range(enemyWoodman.MaxAttackDelayRandomRange.x, enemyWoodman.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyWoodman.damageColliderEvents != (UnityEngine.Object) null)
        enemyWoodman.damageColliderEvents.SetActive(false);
      enemyWoodman.AttackDelay -= Time.deltaTime * enemyWoodman.Spine.timeScale;
      enemyWoodman.MaxAttackDelay -= Time.deltaTime * enemyWoodman.Spine.timeScale;
      if (enemyWoodman.MyState == EnemyWoodman.State.WaitAndTaunt)
      {
        if (enemyWoodman.Spine.AnimationName != "roll-stop" && enemyWoodman.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyWoodman.Spine.AnimationName != enemyWoodman.walkAnim)
            enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.walkAnim, true);
        }
        else if (enemyWoodman.Spine.AnimationName != enemyWoodman.idleAnim)
          enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.idleAnim, true);
        enemyWoodman.state.LookAngle = Utils.GetAngle(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position);
        enemyWoodman.Spine.skeleton.ScaleX = (double) enemyWoodman.state.LookAngle <= 90.0 || (double) enemyWoodman.state.LookAngle >= 270.0 ? -1f : 1f;
        float num = Vector3.Distance(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position);
        if (enemyWoodman.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          if ((double) (enemyWoodman.RepathTimer -= Time.deltaTime * enemyWoodman.Spine.timeScale) < 0.0)
          {
            if ((double) enemyWoodman.MaxAttackDelay < 0.0 || (double) num < (double) enemyWoodman.AttackWithinRange)
            {
              enemyWoodman.AttackWithinRange = float.MaxValue;
              if ((bool) (UnityEngine.Object) enemyWoodman.GetClosestTarget())
              {
                if (enemyWoodman.ChargeAndAttack && ((double) enemyWoodman.MaxAttackDelay < 0.0 || (double) enemyWoodman.AttackDelay < 0.0))
                {
                  enemyWoodman.StopAllCoroutines();
                  enemyWoodman.DisableForces = false;
                  enemyWoodman.StartCoroutine((IEnumerator) enemyWoodman.FightPlayer());
                }
                else if (!enemyWoodman.health.HasShield)
                {
                  enemyWoodman.Angle = (float) (((double) Utils.GetAngle(enemyWoodman.GetClosestTarget().transform.position, enemyWoodman.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                  enemyWoodman.TargetPosition = enemyWoodman.GetClosestTarget().transform.position + new Vector3(enemyWoodman.MaintainTargetDistance * Mathf.Cos(enemyWoodman.Angle), enemyWoodman.MaintainTargetDistance * Mathf.Sin(enemyWoodman.Angle));
                  enemyWoodman.FindPath(enemyWoodman.TargetPosition);
                }
              }
            }
            else if ((bool) (UnityEngine.Object) enemyWoodman.GetClosestTarget() && (double) Vector3.Distance(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position) > (double) enemyWoodman.MoveCloserDistance + (enemyWoodman.health.HasShield ? 0.0 : 1.0))
            {
              enemyWoodman.Angle = (float) (((double) Utils.GetAngle(enemyWoodman.GetClosestTarget().transform.position, enemyWoodman.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemyWoodman.TargetPosition = enemyWoodman.GetClosestTarget().transform.position + new Vector3(enemyWoodman.MaintainTargetDistance * Mathf.Cos(enemyWoodman.Angle), enemyWoodman.MaintainTargetDistance * Mathf.Sin(enemyWoodman.Angle));
              enemyWoodman.FindPath(enemyWoodman.TargetPosition);
            }
          }
        }
        else if ((double) (enemyWoodman.RepathTimer += Time.deltaTime * enemyWoodman.Spine.timeScale) > 2.0)
        {
          enemyWoodman.RepathTimer = 0.0f;
          enemyWoodman.state.CURRENT_STATE = StateMachine.State.Idle;
        }
        if ((double) Time.time >= (double) EnemyWoodman.lastJumpTime + (double) EnemyWoodman.timeToTheNextJump && (double) num >= (double) enemyWoodman.jumpDiveMinDistance)
        {
          enemyWoodman.DoJumpDiveAttack();
          yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWoodman.Spine);
        }
      }
      enemyWoodman.Seperate(0.5f);
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
      if ((double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) <= 1.5)
        return;
      this.TargetPosition = this.GetClosestTarget().transform.position + (Vector3) UnityEngine.Random.insideUnitCircle;
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
    EnemyWoodman enemyWoodman = this;
    enemyWoodman.MyState = EnemyWoodman.State.Attacking;
    enemyWoodman.UsePathing = true;
    enemyWoodman.givePath(enemyWoodman.GetClosestTarget().transform.position);
    enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.walkAnim, true);
    float distanceResetTimer = 0.0f;
    Vector2 previousPosition = (Vector2) enemyWoodman.transform.position;
    enemyWoodman.RepathTimer = 0.0f;
    int NumAttacks = enemyWoodman.DoubleAttack ? 2 : 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = 0.5f;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyWoodman.Spine == (UnityEngine.Object) null || enemyWoodman.Spine.AnimationState == null || enemyWoodman.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        enemyWoodman.Seperate(0.5f);
        switch (enemyWoodman.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            enemyWoodman.StartCoroutine((IEnumerator) enemyWoodman.WaitForTarget());
            yield break;
          case StateMachine.State.Moving:
            if ((bool) (UnityEngine.Object) enemyWoodman.GetClosestTarget())
            {
              enemyWoodman.state.LookAngle = Utils.GetAngle(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position);
              enemyWoodman.Spine.skeleton.ScaleX = (double) enemyWoodman.state.LookAngle <= 90.0 || (double) enemyWoodman.state.LookAngle >= 270.0 ? -1f : 1f;
              enemyWoodman.state.LookAngle = enemyWoodman.state.facingAngle = Utils.GetAngle(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position);
            }
            if ((UnityEngine.Object) enemyWoodman.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) enemyWoodman.transform.position, (Vector2) enemyWoodman.GetClosestTarget().transform.position) < (double) AttackDistance)
            {
              enemyWoodman.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyWoodman.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? enemyWoodman.attackCharge2Anim : enemyWoodman.attackChargeAnim, false);
              if (!string.IsNullOrEmpty(enemyWoodman.AttackBiteStartSFX))
                AudioManager.Instance.PlayOneShot(enemyWoodman.AttackBiteStartSFX, enemyWoodman.gameObject);
            }
            else
            {
              if ((double) (enemyWoodman.RepathTimer += Time.deltaTime * enemyWoodman.Spine.timeScale) > 0.20000000298023224 && (bool) (UnityEngine.Object) enemyWoodman.GetClosestTarget())
              {
                enemyWoodman.RepathTimer = 0.0f;
                enemyWoodman.givePath(enemyWoodman.GetClosestTarget().transform.position);
              }
              if (enemyWoodman.FollowPlayer && (bool) (UnityEngine.Object) enemyWoodman.GetClosestTarget())
              {
                double magnitude = (double) ((Vector2) enemyWoodman.transform.position - previousPosition).magnitude;
                previousPosition = (Vector2) enemyWoodman.transform.position;
                if (magnitude <= 0.14000000059604645)
                {
                  distanceResetTimer += Time.deltaTime * enemyWoodman.Spine.timeScale;
                  if ((double) distanceResetTimer > 6.5)
                    enemyWoodman.transform.position = enemyWoodman.GetClosestTarget().transform.position;
                }
                else
                  distanceResetTimer = 0.0f;
              }
              if ((UnityEngine.Object) enemyWoodman.damageColliderEvents != (UnityEngine.Object) null)
              {
                if ((double) enemyWoodman.state.Timer < 0.20000000298023224 && !enemyWoodman.health.WasJustParried)
                  enemyWoodman.damageColliderEvents.SetActive(true);
                else
                  enemyWoodman.damageColliderEvents.SetActive(false);
              }
            }
            if ((UnityEngine.Object) enemyWoodman.damageColliderEvents != (UnityEngine.Object) null)
            {
              enemyWoodman.damageColliderEvents.SetActive(false);
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyWoodman.damageColliderEvents != (UnityEngine.Object) null)
              enemyWoodman.damageColliderEvents.SetActive(false);
            enemyWoodman.SimpleSpineFlash.FlashWhite(enemyWoodman.state.Timer / SignPostDelay);
            enemyWoodman.state.Timer += Time.deltaTime * enemyWoodman.Spine.timeScale;
            if ((double) enemyWoodman.state.Timer >= (double) SignPostDelay - (double) EnemyWoodman.signPostParryWindow)
              enemyWoodman.canBeParried = true;
            if ((double) enemyWoodman.state.Timer >= (double) SignPostDelay)
            {
              enemyWoodman.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, enemyWoodman.state.LookAngle);
              enemyWoodman.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              enemyWoodman.speed = AttackSpeed * 0.0166666675f;
              enemyWoodman.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? enemyWoodman.attackImpact2Anim : enemyWoodman.attackImpactAnim, false);
              enemyWoodman.canBeParried = true;
              enemyWoodman.StartCoroutine((IEnumerator) enemyWoodman.EnableDamageCollider(0.0f));
              enemyWoodman.DoKnockBack(enemyWoodman.GetClosestTarget().gameObject, -1f, 1f);
              if (!string.IsNullOrEmpty(enemyWoodman.AttackBiteSwipeSFX))
                AudioManager.Instance.PlayOneShot(enemyWoodman.AttackBiteSwipeSFX, enemyWoodman.transform.position);
              if (!string.IsNullOrEmpty(enemyWoodman.AttackVO))
              {
                AudioManager.Instance.PlayOneShot(enemyWoodman.AttackVO, enemyWoodman.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime * enemyWoodman.Spine.timeScale;
            enemyWoodman.speed = AttackSpeed * Time.deltaTime * enemyWoodman.Spine.timeScale;
            enemyWoodman.SimpleSpineFlash.FlashWhite(false);
            enemyWoodman.canBeParried = (double) enemyWoodman.state.Timer <= (double) EnemyWoodman.attackParryWindow;
            if ((double) (enemyWoodman.state.Timer += Time.deltaTime * enemyWoodman.Spine.timeScale) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
            {
              if (++AttackCount <= NumAttacks)
              {
                AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
                enemyWoodman.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.attackChargeAnim, false);
                SignPostDelay = 0.3f;
                break;
              }
              Loop = false;
              enemyWoodman.SimpleSpineFlash.FlashWhite(false);
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    enemyWoodman.StartCoroutine((IEnumerator) enemyWoodman.WaitForTarget());
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
    EnemyWoodman enemyWoodman = this;
    EnemyWoodman.lastJumpTime = Time.time;
    enemyWoodman.ResetJumpCooldown();
    enemyWoodman.isJumpDiving = true;
    enemyWoodman.ClearPaths();
    enemyWoodman.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.wakeUpAnim, false);
    float anticipationDuration = enemyWoodman.Spine.Skeleton.Data.FindAnimation(enemyWoodman.wakeUpAnim).Duration * enemyWoodman.Spine.timeScale;
    float signpostTime = 0.0f;
    if (!string.IsNullOrEmpty(enemyWoodman.AttackJumpStartSFX))
      AudioManager.Instance.PlayOneShot(enemyWoodman.AttackJumpStartSFX, enemyWoodman.transform.position);
    while ((double) enemyWoodman.Spine.timeScale == 9.9999997473787516E-05)
      yield return (object) null;
    while ((double) (signpostTime += Time.deltaTime * enemyWoodman.Spine.timeScale) < (double) anticipationDuration)
    {
      enemyWoodman.SimpleSpineFlash.FlashWhite(signpostTime / anticipationDuration);
      enemyWoodman.state.LookAngle = Utils.GetAngle(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position);
      enemyWoodman.state.facingAngle = Utils.GetAngle(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position);
      yield return (object) null;
    }
    enemyWoodman.SimpleSpineFlash.FlashWhite(false);
    enemyWoodman.state.LookAngle = Utils.GetAngle(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position);
    enemyWoodman.state.facingAngle = Utils.GetAngle(enemyWoodman.transform.position, enemyWoodman.GetClosestTarget().transform.position);
    enemyWoodman.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (!string.IsNullOrEmpty(enemyWoodman.AttackJumpLaunchSFX))
      AudioManager.Instance.PlayOneShot(enemyWoodman.AttackJumpLaunchSFX, enemyWoodman.transform.position);
    enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.idleAnim, false);
    Vector3 startPosition = enemyWoodman.transform.position;
    Vector3 targetPosition = enemyWoodman.GetClosestTarget().transform.position;
    if ((double) Vector2.Distance((Vector2) targetPosition, (Vector2) startPosition) > (double) enemyWoodman.jumpDiveMaxRange)
      targetPosition = startPosition + (targetPosition - startPosition).normalized * enemyWoodman.jumpDiveMaxRange;
    double jumpDiveDuration = (double) enemyWoodman.jumpDiveDuration;
    float progress = 0.0f;
    Vector3 curve = startPosition + (targetPosition - startPosition) / 2f + Vector3.back * enemyWoodman.diveBombArcHeight;
    enemyWoodman.LockToGround = false;
    while ((double) (progress += Time.deltaTime * enemyWoodman.Spine.timeScale) < (double) enemyWoodman.jumpDiveDuration)
    {
      float t = progress / enemyWoodman.jumpDiveDuration;
      Vector3 a = Vector3.Lerp(startPosition, curve, t);
      Vector3 b = Vector3.Lerp(curve, targetPosition, t);
      enemyWoodman.transform.position = Vector3.Lerp(a, b, t);
      yield return (object) null;
    }
    targetPosition.z = 0.0f;
    enemyWoodman.transform.position = targetPosition;
    if (!string.IsNullOrEmpty(enemyWoodman.AttackJumpLandSFX))
      AudioManager.Instance.PlayOneShot(enemyWoodman.AttackJumpLandSFX, enemyWoodman.gameObject);
    yield return (object) enemyWoodman.StartCoroutine((IEnumerator) enemyWoodman.JumpRecoveryRoutine());
    enemyWoodman.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyWoodman.isJumpDiving = false;
    enemyWoodman.FindPath(enemyWoodman.TargetPosition);
    enemyWoodman.jumpDiveRoutine = (Coroutine) null;
  }

  public IEnumerator JumpRecoveryRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWoodman enemyWoodman = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.idleAnim, true);
      enemyWoodman.LockToGround = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyWoodman.LockToGround = true;
    enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.wakeUpAnim, false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyWoodman.jumpRecoveryTime);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator ResurrectRoutine()
  {
    EnemyWoodman enemyWoodman = this;
    enemyWoodman.resurrectProgressBar.Hide();
    enemyWoodman.MyState = EnemyWoodman.State.Resurrecting;
    float progress = 0.0f;
    enemyWoodman.health.enabled = true;
    enemyWoodman.health.untouchable = true;
    enemyWoodman.state.CURRENT_STATE = StateMachine.State.Idle;
    while ((double) progress <= (double) enemyWoodman.resurrectionAnticipationTimer)
    {
      enemyWoodman.SimpleSpineFlash.FlashWhite(progress / 1f);
      progress += Time.deltaTime * enemyWoodman.Spine.timeScale;
      yield return (object) null;
    }
    enemyWoodman.currentResurrectionTimer = 0.0f;
    enemyWoodman.SimpleSpineFlash.FlashWhite(false);
    enemyWoodman.particlesRunes.Play();
    enemyWoodman.FireFX.SetActive(true);
    TrackEntry trackEntry = enemyWoodman.Spine.AnimationState.SetAnimation(0, enemyWoodman.resurrectAnim, false);
    enemyWoodman.Spine.AnimationState.AddAnimation(0, enemyWoodman.idleAnim, true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(trackEntry.Animation.Duration, enemyWoodman.Spine);
    enemyWoodman.health.untouchable = false;
    enemyWoodman.health.Revive(enemyWoodman.health.totalHP * enemyWoodman.resurrectHealthPrecentage);
    enemyWoodman.LockToGround = false;
    enemyWoodman.Spine.Skeleton.SetSkin(enemyWoodman.rageSkin);
    enemyWoodman.Spine.Skeleton.SetSlotsToSetupPose();
    enemyWoodman.MyState = EnemyWoodman.State.WaitAndTaunt;
    enemyWoodman.StartCoroutine((IEnumerator) enemyWoodman.WaitForTarget());
  }

  public void ResetJumpCooldown()
  {
    EnemyWoodman.timeToTheNextJump = UnityEngine.Random.Range(EnemyWoodman.timeRangeToTheNextJump.x, EnemyWoodman.timeRangeToTheNextJump.y);
  }

  public bool CanKillAllWoodmen()
  {
    for (int index = EnemyWoodman.woodmen.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) EnemyWoodman.woodmen[index] != (UnityEngine.Object) null && EnemyWoodman.woodmen[index].state.CURRENT_STATE != StateMachine.State.Dead)
        return false;
    }
    return true;
  }

  public void TryKillAllWoodmen()
  {
    for (int index = EnemyWoodman.woodmen.Count - 1; index >= 0; --index)
    {
      if (!EnemyWoodman.woodmen[index].permanentlyDead)
      {
        EnemyWoodman.woodmen[index].showHPBar.DestroyOnDeath = true;
        EnemyWoodman.woodmen[index].health.DestroyOnDeath = true;
        EnemyWoodman.woodmen[index].health.HP = 1f;
        EnemyWoodman.woodmen[index].health.enabled = true;
        EnemyWoodman.woodmen[index].permanentlyDead = true;
        EnemyWoodman.woodmen[index].health.DealDamage(999f, EnemyWoodman.woodmen[index].gameObject, EnemyWoodman.woodmen[index].transform.position, dealDamageImmediately: true, AttackFlags: Health.AttackFlags.Crit);
      }
    }
  }

  public void UpdateResurrectProgress(float resurrectionProgress)
  {
    this.resurrectProgressBar.UpdateProgress(resurrectionProgress);
  }

  public void PulseResurrectionUI(float resurrectionProgress)
  {
    if ((double) resurrectionProgress < 0.60000002384185791)
      return;
    this.resurrectProgressBar.UpdatePulse();
  }

  public void ActivateAllWoodmen()
  {
    foreach (EnemyWoodman woodman in EnemyWoodman.woodmen)
    {
      if (!woodman.isOn)
        woodman.ActivateWoodman();
    }
    foreach (EnemyWoodmanBig woodman in EnemyWoodmanBig.woodmen)
    {
      if (!woodman.isOn)
        woodman.ActivateWoodman();
    }
  }

  public enum State
  {
    WaitAndTaunt,
    Attacking,
    Resurrecting,
  }
}
