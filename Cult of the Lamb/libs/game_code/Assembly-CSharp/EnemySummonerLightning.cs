// Decompiled with JetBrains decompiler
// Type: EnemySummonerLightning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WebSocketSharp;

#nullable disable
public class EnemySummonerLightning : UnitObject
{
  public bool Lightning = true;
  public bool Melee;
  public SkeletonAnimation skeletonAnimation;
  public SimpleSpineFlash simpleSpineFlash;
  public GameObject Arrow;
  public SpriteRenderer Shadow;
  public ParticleSystem summonParticles;
  public ParticleSystem teleportEffect;
  public float SeperationRadius = 0.5f;
  public GameObject TargetObject;
  public float Range = 6f;
  public float KnockbackSpeed = 0.2f;
  public CircleCollider2D CircleCollider;
  public AssetReferenceGameObject[] EnemyList;
  [SerializeField]
  public bool isSpawnablesIncreasingDamageMultiplier = true;
  public SpriteRenderer aimingIcon;
  public SimpleSpineFlash SimpleSpineFlash;
  public ModifierIcon modifierIconPrefab;
  public float flashTickTimer;
  public Color indicatorColor = Color.red;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string trackAttackAimingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string trackAttackImpactAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string meleeAttackAnimation;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/attack";
  [EventRef]
  public string AttackLongVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/attack_long";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/warning";
  [EventRef]
  public string MeleeWarningVO = "event:/dlc/dungeon05/enemy/lightning_tracker/attack_melee_warning";
  [EventRef]
  public string SummonSfx = "event:/enemy/summon";
  [EventRef]
  public string MeleeSwingSFX = "event:/dlc/dungeon05/enemy/bellknight/attack_basic_swing";
  [EventRef]
  public string MeleeReturnSFX = "event:/dlc/dungeon05/enemy/lightning_tracker/attack_melee_return";
  [EventRef]
  public string AttackLightningRingStart = "event:/dlc/dungeon05/enemy/lightning_tracker/attack_lightning_ring_start";
  public EventInstance lightningRingStartInstance;
  public RelicData lightningData;
  public const float LIGHTNING_AIM_LERP_SPEED = 5f;
  public bool lightningHasLockedOn;
  public float MortarDelay = 1f;
  public float LightningDelay = 0.5f;
  public float TeleportDelay = 3f;
  public float TeleportDelayMin = 3f;
  public float TeleportDelayMax = 5f;
  public float TeleportFleeRadius = 2.5f;
  public float TeleportFleeDelayMax = 2f;
  public float TeleportMinDistance = 10f;
  public float TeleportMaxDistance = 14f;
  [Tooltip("How long will the target reticule follow the player for before attack is triggered?")]
  public float LightingAimingTime = 4f;
  public float LightingPlayerDamage = 1f;
  public float LightingEnemyDamage = 8f;
  public float LightingRadius = 3f;
  [Tooltip("How long does a target need to stand next to another target to transfer 'locked on' status")]
  public float LightningSwitchTargetTime = 0.5f;
  public float LightningSwitchScanDuration = 1.5f;
  public float LightningSwitchDistance = 0.5f;
  [UnityEngine.Range(0.0f, 30f)]
  [Tooltip("The number of projectiles to be fired out from where the lightning strikes")]
  public int LightningImpactProjectileCount = 8;
  public float LightningDelayMin = 2f;
  public float LightningDelayMax = 4f;
  public float LightningStrikeRadius = 4f;
  public float LightningExpansionSpeed = 5f;
  public float LightningEnemyDamage = 0.8f;
  [Tooltip("The number of melee attacks this unit will make before teleporting away")]
  public int MeleeAttackCount = 1;
  public float MeleeRadius = 2.5f;
  public float SignPostCloseCombatDelay = 0.5f;
  public bool MeleeKnockbackEnabled = true;
  public float MeleeKnockbackStrength = 1.5f;
  public float CloseCombatCooldown;
  public static float SIGNPOST_PARRY_WINDOW = 0.2f;
  public static float ATTACK_PARRY_WINDOW = 0.15f;
  public bool canBeParried;
  public ColliderEvents damageColliderEvents;
  public float MeleeCooldownCounter;
  public float meleeCooldown = 0.5f;
  public float currentMeleeAttacks;
  public float FleeDelay = 1f;
  public float StartSpeed = 0.4f;
  public string LoopedSoundSFX;
  public EventInstance LoopedSound;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool shorterTeleportSearch;
  public int shorterTeleportSteps = 12;
  public float LIGHTNING_DAMAGE_PLAYER = 1f;
  public float damageColliderStartX;
  public Coroutine TeleportRoutine;
  public Coroutine MeleeRoutine;
  public Coroutine LightningRoutine;
  public bool Stunned;
  public bool Teleporting;
  public Coroutine cTeleporting;
  public bool isRingLightning;
  public float ringLightningDelay;
  public VFXParticle currentLightingVFX;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public void ResetMeleeAttacks()
  {
    this.currentMeleeAttacks = (float) this.MeleeAttackCount;
    this.MeleeCooldownCounter = 0.0f;
  }

  public override void Awake()
  {
    base.Awake();
    this.damageColliderStartX = this.damageColliderEvents.transform.localPosition.x;
  }

  public new void Update()
  {
    if ((UnityEngine.Object) this.aimingIcon != (UnityEngine.Object) null && this.aimingIcon.gameObject.activeSelf)
    {
      if ((double) this.flashTickTimer >= (this.lightningHasLockedOn ? 0.059999998658895493 : 0.11999999731779099) && BiomeConstants.Instance.IsFlashLightsActive)
      {
        this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
        this.aimingIcon.material.SetColor("_Color", this.indicatorColor);
        this.flashTickTimer = 0.0f;
      }
      else
        this.flashTickTimer += Time.deltaTime;
    }
    if (!this.isRingLightning)
      return;
    this.ringLightningDelay -= Time.deltaTime;
    if ((double) this.ringLightningDelay > 0.0)
      return;
    this.isRingLightning = false;
    LightningRingExplosion.CreateExplosion(this.transform.position, this.health.team, this.health, this.LightningExpansionSpeed, this.LIGHTNING_DAMAGE_PLAYER, this.LightningEnemyDamage);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "Teleport":
        this.Teleport();
        break;
      case "Fireball":
        if (string.IsNullOrEmpty(this.AttackVO))
          break;
        AudioManager.Instance.PlayOneShot(this.AttackVO, this.transform.position);
        break;
    }
  }

  public override void OnEnable()
  {
    this.SeperateObject = true;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.CircleCollider = this.GetComponent<CircleCollider2D>();
    this.lightningData = EquipmentManager.GetRelicData(RelicType.LightningStrike);
    this.aimingIcon?.gameObject.SetActive(false);
    this.LightningDelay = UnityEngine.Random.Range(this.LightningDelayMin, this.LightningDelayMax);
    this.ResetMeleeAttacks();
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    base.OnEnable();
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnCloseCombatDamagerTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.ClearPaths();
    this.StopAndClearCoroutines(true);
    if (!this.LoopedSoundSFX.IsNullOrEmpty())
      AudioManager.Instance.StopLoop(this.LoopedSound);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnCloseCombatDamagerTriggerEnter);
    }
    this.skeletonAnimation.AnimationState.SetAnimation(0, this.idleAnimation, false);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Shadow.enabled = true;
    this.Teleporting = false;
    this.DisableForces = false;
    this.simpleSpineFlash.FlashWhite(false);
    this.aimingIcon.gameObject.SetActive(false);
    AudioManager.Instance.StopOneShotInstanceEarly(this.lightningRingStartInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.LoopedSoundSFX.IsNullOrEmpty())
      return;
    AudioManager.Instance.StopLoop(this.LoopedSound);
  }

  public void StopAndClearCoroutines(bool clearLightning = false)
  {
    this.StopAllCoroutines();
    this.TeleportRoutine = (Coroutine) null;
    this.MeleeRoutine = (Coroutine) null;
    this.LightningRoutine = (Coroutine) null;
    if ((UnityEngine.Object) this.aimingIcon != (UnityEngine.Object) null && (UnityEngine.Object) this.aimingIcon.gameObject != (UnityEngine.Object) null)
      this.aimingIcon.gameObject.SetActive(false);
    if (clearLightning && (UnityEngine.Object) this.currentLightingVFX != (UnityEngine.Object) null)
    {
      this.currentLightingVFX.CancelVFX();
      this.currentLightingVFX = (VFXParticle) null;
    }
    AudioManager.Instance.StopOneShotInstanceEarly(this.lightningRingStartInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    this.skeletonAnimation.AnimationState.SetAnimation(0, this.idleAnimation, true);
  }

  public IEnumerator WaitForTarget()
  {
    EnemySummonerLightning summonerLightning = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    if (!summonerLightning.LoopedSoundSFX.IsNullOrEmpty())
      summonerLightning.LoopedSound = AudioManager.Instance.CreateLoop(summonerLightning.LoopedSoundSFX, summonerLightning.skeletonAnimation.gameObject, true);
    while ((double) PlayerFarming.GetClosestPlayerDist(summonerLightning.transform.position) > (double) summonerLightning.Range)
      yield return (object) null;
    summonerLightning.StopAndClearCoroutines();
    summonerLightning.StartCoroutine((IEnumerator) summonerLightning.ChaseTarget());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.simpleSpineFlash.FlashWhite(false);
    if (AttackType == Health.AttackTypes.Projectile)
    {
      if (!this.Stunned)
      {
        if (!string.IsNullOrEmpty(this.GetHitVO))
          AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
        CameraManager.shakeCamera(0.4f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
        GameManager.GetInstance().HitStop();
        BiomeConstants.Instance.EmitHitVFX(AttackLocation + Vector3.back * 1f, Quaternion.identity.z, "HitFX_Weak");
        this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
        this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
        this.simpleSpineFlash.FlashFillRed();
        this.StopAndClearCoroutines(true);
        this.StartCoroutine((IEnumerator) this.DoStunned());
      }
      else
      {
        CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
        GameObject gameObject = BiomeConstants.Instance.HitFX_Blocked.Spawn();
        gameObject.transform.position = AttackLocation + Vector3.back * 0.5f;
        gameObject.transform.rotation = Quaternion.identity;
      }
    }
    else
    {
      if (!string.IsNullOrEmpty(this.GetHitVO))
        AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
      CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
      BiomeConstants.Instance.EmitHitVFX(AttackLocation + Vector3.back * 1f, Quaternion.identity.z, "HitFX_Weak");
      this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.simpleSpineFlash.FlashFillRed();
      this.state.facingAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      if (this.LightningRoutine != null)
        return;
      this.StopAndClearCoroutines();
      this.TeleportRoutine = this.StartCoroutine((IEnumerator) this.DoTeleport());
    }
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.LoopedSoundSFX.IsNullOrEmpty())
      AudioManager.Instance.StopLoop(this.LoopedSound);
    if (this.state.CURRENT_STATE != StateMachine.State.Dieing)
    {
      if (!string.IsNullOrEmpty(this.DeathVO))
        AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
      this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.aimingIcon.gameObject);
  }

  public IEnumerator ChaseTarget()
  {
    EnemySummonerLightning summonerLightning = this;
    summonerLightning.state.CURRENT_STATE = StateMachine.State.Idle;
    bool Loop = true;
    while (Loop)
    {
      Health closestTarget = summonerLightning.GetClosestTarget();
      if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
      {
        summonerLightning.skeletonAnimation.AnimationState.SetAnimation(0, summonerLightning.idleAnimation, true);
        yield return (object) null;
      }
      else
      {
        summonerLightning.TargetObject = closestTarget.gameObject;
        summonerLightning.state.facingAngle = Utils.GetAngle(summonerLightning.transform.position, summonerLightning.TargetObject.transform.position);
        float distanceToTarget = Vector3.Distance(summonerLightning.TargetObject.transform.position, summonerLightning.transform.position);
        summonerLightning.FleeDelay -= Time.deltaTime * summonerLightning.skeletonAnimation.timeScale;
        summonerLightning.TeleportDelay -= Time.deltaTime * summonerLightning.skeletonAnimation.timeScale;
        summonerLightning.MeleeCooldownCounter -= Time.deltaTime * summonerLightning.skeletonAnimation.timeScale;
        if (summonerLightning.Melee && (double) summonerLightning.MeleeCooldownCounter <= 0.0 && (double) summonerLightning.currentMeleeAttacks > 0.0 && (double) distanceToTarget < (double) summonerLightning.MeleeRadius)
        {
          --summonerLightning.currentMeleeAttacks;
          yield return (object) (summonerLightning.MeleeRoutine = summonerLightning.StartCoroutine((IEnumerator) summonerLightning.DoCloseCombatAttack()));
          summonerLightning.MeleeRoutine = (Coroutine) null;
          summonerLightning.MeleeCooldownCounter = summonerLightning.meleeCooldown;
          summonerLightning.FleeDelay = 0.0f;
        }
        if (!((UnityEngine.Object) closestTarget == (UnityEngine.Object) null))
        {
          if ((double) summonerLightning.FleeDelay < 0.0 && (double) distanceToTarget < (double) summonerLightning.TeleportFleeRadius || (double) summonerLightning.TeleportDelay < 0.0)
          {
            summonerLightning.FleeDelay = summonerLightning.TeleportFleeDelayMax;
            summonerLightning.TeleportDelay = UnityEngine.Random.Range(summonerLightning.TeleportDelayMin, summonerLightning.TeleportDelayMax);
            summonerLightning.ResetMeleeAttacks();
            summonerLightning.LightningDelay = 0.5f;
            yield return (object) (summonerLightning.TeleportRoutine = summonerLightning.StartCoroutine((IEnumerator) summonerLightning.DoTeleport()));
            summonerLightning.TeleportRoutine = (Coroutine) null;
          }
          if (!((UnityEngine.Object) closestTarget == (UnityEngine.Object) null))
          {
            if (summonerLightning.Lightning && (double) (summonerLightning.LightningDelay -= Time.deltaTime * summonerLightning.skeletonAnimation.timeScale) < 0.0 && (double) distanceToTarget > (double) summonerLightning.TeleportFleeRadius)
            {
              summonerLightning.StopAndClearCoroutines();
              summonerLightning.LightningDelay = UnityEngine.Random.Range(summonerLightning.LightningDelayMin, summonerLightning.LightningDelayMax);
              summonerLightning.LightningRoutine = summonerLightning.StartCoroutine((IEnumerator) summonerLightning.DoLightningStrike());
              break;
            }
            yield return (object) null;
            closestTarget = (Health) null;
          }
        }
      }
    }
  }

  public IEnumerator DoStunned()
  {
    EnemySummonerLightning summonerLightning = this;
    summonerLightning.Stunned = true;
    summonerLightning.health.ArrowAttackVulnerability = 1f;
    summonerLightning.health.MeleeAttackVulnerability = 1f;
    summonerLightning.state.CURRENT_STATE = StateMachine.State.Attacking;
    summonerLightning.skeletonAnimation.AnimationState.SetAnimation(0, summonerLightning.idleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * summonerLightning.skeletonAnimation.timeScale) < 2.0)
      yield return (object) null;
    summonerLightning.Stunned = false;
    summonerLightning.health.ArrowAttackVulnerability = 1f;
    summonerLightning.health.MeleeAttackVulnerability = 0.1f;
    summonerLightning.StopAndClearCoroutines();
    summonerLightning.StartCoroutine((IEnumerator) summonerLightning.DoTeleport());
  }

  public void SpineEventWeaponSwing()
  {
    if (string.IsNullOrEmpty(this.MeleeSwingSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.MeleeSwingSFX, this.transform.position);
  }

  public void SpineEventWeaponSwingReturn()
  {
    if (string.IsNullOrEmpty(this.MeleeReturnSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.MeleeReturnSFX);
  }

  public IEnumerator DoCloseCombatAttack()
  {
    EnemySummonerLightning summonerLightning = this;
    summonerLightning.ClearPaths();
    summonerLightning.state.CURRENT_STATE = StateMachine.State.Attacking;
    float Progress = 0.0f;
    summonerLightning.skeletonAnimation.AnimationState.SetAnimation(0, summonerLightning.meleeAttackAnimation, false);
    summonerLightning.skeletonAnimation.AnimationState.AddAnimation(0, summonerLightning.idleAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(summonerLightning.AttackVO))
      AudioManager.Instance.PlayOneShot(summonerLightning.MeleeWarningVO, summonerLightning.transform.position);
    if (!string.IsNullOrEmpty(summonerLightning.MeleeWarningVO))
      AudioManager.Instance.PlayOneShot(summonerLightning.MeleeWarningVO, summonerLightning.transform.position);
    summonerLightning.state.facingAngle = summonerLightning.state.LookAngle = Utils.GetAngle(summonerLightning.transform.position, summonerLightning.TargetObject.transform.position);
    summonerLightning.damageColliderEvents.transform.localPosition = (double) summonerLightning.state.LookAngle <= 90.0 || (double) summonerLightning.state.LookAngle >= 270.0 ? new Vector3(summonerLightning.damageColliderStartX, 0.0f, 0.0f) : new Vector3(-summonerLightning.damageColliderStartX, 0.0f, 0.0f);
    summonerLightning.skeletonAnimation.skeleton.ScaleX = (double) summonerLightning.state.LookAngle <= 90.0 || (double) summonerLightning.state.LookAngle >= 270.0 ? -1f : 1f;
    while ((double) (Progress += Time.deltaTime * summonerLightning.skeletonAnimation.timeScale) < (double) summonerLightning.SignPostCloseCombatDelay)
    {
      if ((double) Progress >= (double) summonerLightning.SignPostCloseCombatDelay - (double) EnemySummonerLightning.SIGNPOST_PARRY_WINDOW)
        summonerLightning.canBeParried = true;
      summonerLightning.SimpleSpineFlash.FlashWhite(Progress / summonerLightning.SignPostCloseCombatDelay);
      yield return (object) null;
    }
    summonerLightning.SimpleSpineFlash.FlashWhite(false);
    Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.deltaTime * summonerLightning.skeletonAnimation.timeScale) < (double) Duration)
    {
      if ((UnityEngine.Object) summonerLightning.damageColliderEvents != (UnityEngine.Object) null)
        summonerLightning.damageColliderEvents.SetActive(true);
      summonerLightning.canBeParried = (double) Progress <= (double) EnemySummonerLightning.ATTACK_PARRY_WINDOW;
      yield return (object) null;
    }
    if ((UnityEngine.Object) summonerLightning.damageColliderEvents != (UnityEngine.Object) null)
      summonerLightning.damageColliderEvents.SetActive(false);
    summonerLightning.canBeParried = false;
    yield return (object) new WaitForSeconds(0.5f);
    summonerLightning.CloseCombatCooldown = 1f;
    summonerLightning.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void OnCloseCombatDamagerTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    if (!this.MeleeKnockbackEnabled)
      return;
    component.GetComponent<UnitObject>()?.DoKnockBack(this.gameObject, this.MeleeKnockbackStrength, 0.5f);
  }

  public IEnumerator DoTeleport()
  {
    EnemySummonerLightning summonerLightning = this;
    summonerLightning.state.CURRENT_STATE = StateMachine.State.Teleporting;
    summonerLightning.Shadow.enabled = false;
    summonerLightning.Teleporting = true;
    if ((UnityEngine.Object) summonerLightning.damageColliderEvents != (UnityEngine.Object) null)
      summonerLightning.damageColliderEvents.SetActive(false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * summonerLightning.skeletonAnimation.timeScale) < 0.15000000596046448)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", summonerLightning.gameObject);
    summonerLightning.summonParticles.startDelay = 0.0f;
    summonerLightning.summonParticles.Play();
    summonerLightning.teleportEffect.Play();
    summonerLightning.skeletonAnimation.AnimationState.SetAnimation(0, "teleport", false);
    summonerLightning.skeletonAnimation.AnimationState.AddAnimation(0, summonerLightning.idleAnimation, true, 0.0f);
    summonerLightning.CircleCollider.enabled = false;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * summonerLightning.skeletonAnimation.timeScale) < 0.800000011920929)
      yield return (object) null;
    summonerLightning.StopAndClearCoroutines();
    summonerLightning.StartCoroutine((IEnumerator) summonerLightning.ChaseTarget());
  }

  public void Teleport()
  {
    Vector3 vector3_1 = (UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null ? Vector3.zero : this.TargetObject.transform.position;
    if (this.shorterTeleportSearch)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = UnityEngine.Random.Range(this.TeleportMinDistance, this.TeleportMaxDistance);
      for (int index = 0; index < this.shorterTeleportSteps; ++index)
      {
        Vector3 vector3_2 = vector3_1 + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
        RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) vector3_1, this.CircleCastRadius, (Vector2) Vector3.Normalize(vector3_2 - vector3_1), distance, (int) this.layerToCheck);
        if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(vector3_1, (Vector3) raycastHit2D.centroid) > (double) this.TeleportMinDistance)
          {
            if (this.ShowDebug)
            {
              this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y));
              this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
            }
            this.transform.position = (Vector3) raycastHit2D.centroid + Vector3.Normalize(vector3_1 - vector3_2) * this.CircleCastOffset;
            break;
          }
          f += (float) (360.0 / (double) this.shorterTeleportSteps * (Math.PI / 180.0));
        }
        else
        {
          if (this.ShowDebug)
          {
            this.EndPoints.Add(new Vector3(vector3_2.x, vector3_2.y));
            this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
          }
          this.transform.position = vector3_2;
          break;
        }
      }
    }
    else
    {
      float num = 100f;
      while ((double) --num > 0.0)
      {
        float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
        float distance = UnityEngine.Random.Range(this.TeleportMinDistance, this.TeleportMaxDistance);
        Vector3 vector3_3 = vector3_1 + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
        RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) vector3_1, this.CircleCastRadius, (Vector2) Vector3.Normalize(vector3_3 - vector3_1), distance, (int) this.layerToCheck);
        if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(vector3_1, (Vector3) raycastHit2D.centroid) > (double) this.TeleportMinDistance)
          {
            if (this.ShowDebug)
            {
              this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y));
              this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
            }
            this.transform.position = (Vector3) raycastHit2D.centroid + Vector3.Normalize(vector3_1 - vector3_3) * this.CircleCastOffset;
            break;
          }
        }
        else
        {
          if (this.ShowDebug)
          {
            this.EndPoints.Add(new Vector3(vector3_3.x, vector3_3.y));
            this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
          }
          this.transform.position = vector3_3;
          break;
        }
      }
    }
    if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
      this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
    this.CircleCollider.enabled = true;
    this.Shadow.enabled = true;
    this.summonParticles.startDelay = 0.0f;
    this.summonParticles.Play();
    this.teleportEffect.Play();
    this.Teleporting = false;
    AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", this.gameObject);
  }

  public void DoRingLightning()
  {
    this.isRingLightning = true;
    this.ringLightningDelay = 0.5f;
  }

  public IEnumerator DoLightningStrike()
  {
    EnemySummonerLightning summonerLightning = this;
    List<Health> allTargets = summonerLightning.GetAllTargets(Health.Team.KillAll);
    Health switchTarget = (Health) null;
    summonerLightning.lightningHasLockedOn = false;
    float aimTime = summonerLightning.LightingAimingTime;
    float switchTargetTime = summonerLightning.LightningSwitchTargetTime;
    float switchScanDuration = summonerLightning.LightningSwitchScanDuration;
    float switchBuffer = 1f;
    if ((UnityEngine.Object) summonerLightning.TargetObject == (UnityEngine.Object) null)
    {
      summonerLightning.StartCoroutine((IEnumerator) summonerLightning.ChaseTarget());
    }
    else
    {
      summonerLightning.aimingIcon.gameObject.SetActive(true);
      summonerLightning.aimingIcon.transform.parent = summonerLightning.transform.parent;
      summonerLightning.aimingIcon.transform.position = summonerLightning.TargetObject.transform.position;
      summonerLightning.skeletonAnimation.AnimationState.SetAnimation(0, summonerLightning.trackAttackAimingAnimation, true);
      summonerLightning.lightningRingStartInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup(summonerLightning.AttackLightningRingStart, summonerLightning.gameObject.transform);
      float Timer = 0.0f;
      while ((double) aimTime > 0.0)
      {
        if ((UnityEngine.Object) summonerLightning.TargetObject == (UnityEngine.Object) null)
        {
          summonerLightning.SimpleSpineFlash?.FlashWhite(false);
          summonerLightning.aimingIcon.gameObject.SetActive(false);
          AudioManager.Instance.StopOneShotInstanceEarly(summonerLightning.lightningRingStartInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
          summonerLightning.StartCoroutine((IEnumerator) summonerLightning.ChaseTarget());
          yield break;
        }
        Timer += Time.deltaTime * summonerLightning.skeletonAnimation.timeScale;
        summonerLightning.state.facingAngle = Utils.GetAngle(summonerLightning.transform.position, summonerLightning.TargetObject.transform.position);
        summonerLightning.SimpleSpineFlash?.FlashWhite(Timer / summonerLightning.LightingAimingTime);
        switchBuffer -= Time.deltaTime * summonerLightning.skeletonAnimation.timeScale;
        aimTime -= Time.deltaTime * summonerLightning.skeletonAnimation.timeScale;
        if ((UnityEngine.Object) summonerLightning.TargetObject != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) switchTarget != (UnityEngine.Object) null)
          {
            if ((double) switchScanDuration > 0.0)
            {
              switchScanDuration -= Time.deltaTime * summonerLightning.skeletonAnimation.timeScale;
              summonerLightning.aimingIcon.transform.position = Vector3.MoveTowards(summonerLightning.aimingIcon.transform.position, switchTarget.transform.position, 5f * Time.deltaTime * summonerLightning.skeletonAnimation.timeScale);
            }
          }
          else
          {
            summonerLightning.aimingIcon.transform.position = Vector3.MoveTowards(summonerLightning.aimingIcon.transform.position, summonerLightning.TargetObject.transform.position, 5f * Time.deltaTime * summonerLightning.skeletonAnimation.timeScale);
            if ((double) switchBuffer <= 0.0)
            {
              foreach (Health health in allTargets)
              {
                if (!((UnityEngine.Object) health == (UnityEngine.Object) null) && !((UnityEngine.Object) summonerLightning.TargetObject == (UnityEngine.Object) health.gameObject) && health.team != summonerLightning.health.team && (double) Vector3.Distance(summonerLightning.TargetObject.transform.position, health.transform.position) < (double) summonerLightning.LightningSwitchDistance)
                {
                  if ((UnityEngine.Object) switchTarget == (UnityEngine.Object) health)
                    switchTargetTime -= Time.deltaTime * summonerLightning.skeletonAnimation.timeScale;
                  else if ((UnityEngine.Object) switchTarget == (UnityEngine.Object) null)
                  {
                    switchBuffer = 1f;
                    switchTarget = health;
                    switchTargetTime = summonerLightning.LightningSwitchTargetTime;
                  }
                }
              }
            }
          }
          yield return (object) null;
        }
        else
        {
          summonerLightning.StartCoroutine((IEnumerator) summonerLightning.ChaseTarget());
          yield break;
        }
      }
      summonerLightning.simpleSpineFlash.FlashWhite(false);
      summonerLightning.lightningHasLockedOn = true;
      if (!string.IsNullOrEmpty(summonerLightning.WarningVO))
        AudioManager.Instance.PlayOneShot(summonerLightning.WarningVO, summonerLightning.transform.position);
      summonerLightning.skeletonAnimation.AnimationState.SetAnimation(0, summonerLightning.trackAttackImpactAnimation, false);
      summonerLightning.skeletonAnimation.AnimationState.AddAnimation(0, summonerLightning.idleAnimation, true, 0.0f);
      summonerLightning.summonParticles.startDelay = 1.5f;
      summonerLightning.summonParticles.Play();
      summonerLightning.currentLightingVFX = (VFXParticle) summonerLightning.lightningData.VFXData.ImpactVFXObject.SpawnVFX(summonerLightning.aimingIcon.transform, true);
      if (!string.IsNullOrEmpty(summonerLightning.AttackLongVO))
        AudioManager.Instance.PlayOneShot(summonerLightning.AttackLongVO);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, summonerLightning.skeletonAnimation);
      summonerLightning.aimingIcon.gameObject.SetActive(false);
      AudioManager.Instance.StopOneShotInstanceEarly(summonerLightning.lightningRingStartInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
      Explosion.CreateExplosion(summonerLightning.aimingIcon.transform.position, summonerLightning.health.team, summonerLightning.health, summonerLightning.LightingRadius, summonerLightning.LightingPlayerDamage, summonerLightning.LightingEnemyDamage, includeOwner: true);
      LightningRingExplosion.CreateExplosion(summonerLightning.aimingIcon.transform.position, summonerLightning.health.team, summonerLightning.health, summonerLightning.LightningExpansionSpeed, summonerLightning.LIGHTNING_DAMAGE_PLAYER, summonerLightning.LightningEnemyDamage);
      summonerLightning.StopAndClearCoroutines();
      summonerLightning.StartCoroutine((IEnumerator) summonerLightning.ChaseTarget());
    }
  }

  public List<Health> GetAllTargets(Health.Team team)
  {
    List<Health> allTargets = new List<Health>();
    switch (team)
    {
      case Health.Team.Neutral:
        allTargets = new List<Health>((IEnumerable<Health>) Health.neutralTeam);
        break;
      case Health.Team.PlayerTeam:
        allTargets = new List<Health>((IEnumerable<Health>) Health.playerTeam);
        break;
      case Health.Team.Team2:
        allTargets = new List<Health>((IEnumerable<Health>) Health.team2);
        break;
      case Health.Team.KillAll:
        allTargets = new List<Health>((IEnumerable<Health>) Health.playerTeam);
        allTargets.AddRange((IEnumerable<Health>) Health.team2);
        allTargets.AddRange((IEnumerable<Health>) Health.neutralTeam);
        break;
    }
    for (int index = allTargets.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) allTargets[index] == (UnityEngine.Object) null)
        allTargets.RemoveAt(index);
    }
    allTargets.RemoveAll((Predicate<Health>) (t => t.InanimateObject));
    return allTargets;
  }

  public void OnDrawGizmos()
  {
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
