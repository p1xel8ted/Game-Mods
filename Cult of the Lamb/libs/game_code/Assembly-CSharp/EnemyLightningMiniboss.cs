// Decompiled with JetBrains decompiler
// Type: EnemyLightningMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyLightningMiniboss : UnitObject
{
  [SerializeField]
  public Vector2 timeBetweenAttacks;
  [SerializeField]
  public float slamAttackLightningSpeed;
  [SerializeField]
  public float slamAttackCooldown;
  [SerializeField]
  public LightningBossOrbSpinner spinnerSmall;
  [SerializeField]
  public LightningBossOrbSpinner spinnerLarge;
  [SerializeField]
  public LightningBossOrbSpinner spinnerLargePingPong;
  [SerializeField]
  public LightningBossOrbSpinner spinnerHuge;
  [SerializeField]
  public LightningBossOrbSpinner spinnerHugePingPong;
  [SerializeField]
  public GameObject lightningShield;
  [SerializeField]
  public GameObject spinnerOrigin;
  [SerializeField]
  public GameObject teleportOutParticle;
  [SerializeField]
  public GameObject teleportInParticle;
  public GameObject beamPosition;
  public ParticleSystem chargeSparks;
  public ParticleSystem chargeSphere;
  public GameObject LightningImpact;
  [SerializeField]
  public float beamThickness;
  [SerializeField]
  public float beamAttackDuration;
  [SerializeField]
  public float beamTrackingSpeed;
  public float stunDuration = 3.5f;
  public GameObject projectilePrefab;
  public float bombSpeed = 4f;
  [SerializeField]
  public Vector2 bombShowerAmount;
  [SerializeField]
  public float timeBetweenBombs;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackRecoverAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string KnockedOutAnimationStart;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string KnockedOutAnimationLoop;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string KnockedOutRecoverAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string TeleportAnimation;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string AttackBeamStartSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_beam_start";
  [EventRef]
  public string AttackBeamLoopSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_beam_loop";
  [EventRef]
  public string AttackThrowStartSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_throw_start";
  [EventRef]
  public string AttackThrowLoopSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_throw_loop";
  [EventRef]
  public string AttackThrowLaunchSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_throw_launch";
  [EventRef]
  public string AttackOrbStartSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_orb_start";
  [EventRef]
  public string AttackOrbLoopSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_orb_loop";
  [EventRef]
  public string AttackOrbVulnerableStartSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_orb_vulnerable_start";
  [EventRef]
  public string AttackOrbVulnerableRecoverSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_orb_vulnerable_recover";
  [EventRef]
  public string AttackLightningRingSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_lightning_ring_start";
  [EventRef]
  public string TeleportAwaySFX = "event:/enemy/teleport_away";
  [EventRef]
  public string TeleportAppearSFX = "event:/enemy/teleport_appear";
  [EventRef]
  public string AttackOrbNarrowLoopSFX = "event:/dlc/dungeon05/enemy/miniboss_lightning/attack_orb_narrow_loop";
  public EventInstance beamStartInstanceSFX;
  public EventInstance beamLoopInstanceSFX;
  public EventInstance throwLoopInstanceSFX;
  public EventInstance orbLoopInstanceSFX;
  public EventInstance orbNarrowLoopInstanceSFX;
  public EventInstance attackOrbStartInstanceSFX;
  public string ordDeadCountParam = "enemyMinibossLightningNumberOfOrbsDestroyed";
  public SimpleSpineFlash SimpleSpineFlash;
  public bool DisableKnockback;
  public float KnockbackModifier = 1.5f;
  public float Angle;
  public Vector3 Force;
  public Vector3 lerpTarget;
  public const float minBombRange = 4f;
  public const float maxBombRange = 8f;
  public float moveTimer;
  public float attackTimer;
  public float slamAttackTimestamp;
  public float closeByTimer;
  public bool attacking;
  public bool secondPhase;
  public bool updateDirection = true;
  public Coroutine attackRoutine;
  public GameObject spinningOrbsParent;
  public bool isInvincible;
  public EnemyLightningMiniboss.Attacks[] attackOrder;
  public int currentAttackIndex;

  public override void Awake()
  {
    base.Awake();
    this.PopulateAttackArray();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.secondPhase)
      this.StartCoroutine((IEnumerator) this.TriggerSecondPhase());
    this.health.OnDieEarly += new Health.DieAction(this.UnlockSkin);
    this.ShuffleAttackOrder();
  }

  public void PopulateAttackArray()
  {
    this.attackOrder = new EnemyLightningMiniboss.Attacks[4]
    {
      EnemyLightningMiniboss.Attacks.Laser,
      EnemyLightningMiniboss.Attacks.SpinnerSmall,
      EnemyLightningMiniboss.Attacks.SpinnerLarge,
      EnemyLightningMiniboss.Attacks.BombShower
    };
  }

  public void ShuffleAttackOrder()
  {
    EnemyLightningMiniboss.Attacks attacks1 = this.attackOrder[this.attackOrder.Length - 1];
    for (int index1 = this.attackOrder.Length - 1; index1 > 0; --index1)
    {
      int index2 = UnityEngine.Random.Range(0, index1 + 1);
      ref EnemyLightningMiniboss.Attacks local1 = ref this.attackOrder[index1];
      ref EnemyLightningMiniboss.Attacks local2 = ref this.attackOrder[index2];
      EnemyLightningMiniboss.Attacks attacks2 = this.attackOrder[index2];
      EnemyLightningMiniboss.Attacks attacks3 = this.attackOrder[index1];
      local1 = attacks2;
      int num = (int) attacks3;
      local2 = (EnemyLightningMiniboss.Attacks) num;
    }
    if (!this.attackOrder[0].Equals((object) attacks1))
      return;
    int index = UnityEngine.Random.Range(1, this.attackOrder.Length);
    ref EnemyLightningMiniboss.Attacks local3 = ref this.attackOrder[0];
    ref EnemyLightningMiniboss.Attacks local4 = ref this.attackOrder[index];
    EnemyLightningMiniboss.Attacks attacks4 = this.attackOrder[index];
    EnemyLightningMiniboss.Attacks attacks5 = this.attackOrder[0];
    local3 = attacks4;
    int num1 = (int) attacks5;
    local4 = (EnemyLightningMiniboss.Attacks) num1;
  }

  public void UnlockSkin(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (DataManager.GetFollowerSkinUnlocked("Boss Dog 3"))
      return;
    DataManager.SetFollowerSkinUnlocked("Boss Dog 3");
  }

  public void GiveSkin(GameObject Attacker)
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    FollowerSkinCustomTarget.Create(this.Spine.transform.position, Attacker.transform.position, 2f, "Boss Dog 3", (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().AddPlayerToCamera();
    }));
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.health.OnDieEarly -= new Health.DieAction(this.UnlockSkin);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.SimpleSpineFlash.FlashWhite(false);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, false);
    this.lightningShield.gameObject.SetActive(false);
    this.chargeSparks.gameObject.SetActive(false);
    this.chargeSphere.gameObject.SetActive(false);
    this.LightningImpact.SetActive(false);
    ArrowLightningBeam.ClearBeams();
    this.attackTimer = UnityEngine.Random.Range(this.timeBetweenAttacks.x, this.timeBetweenAttacks.y);
    this.attacking = false;
    this.updateDirection = true;
    this.spinnerOrigin.gameObject.SetActive(false);
    this.health.invincible = false;
    this.health.IsDeflecting = false;
    this.isInvincible = false;
    if (!((UnityEngine.Object) this.spinningOrbsParent != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.spinningOrbsParent);
    this.spinningOrbsParent = (GameObject) null;
  }

  public override void Update()
  {
    base.Update();
    this.attackTimer -= Time.deltaTime * this.Spine.timeScale;
    if (this.updateDirection)
      this.LookAtTarget();
    if (this.attacking)
      return;
    if ((double) this.health.HP < (double) this.health.totalHP / 2.0 && !this.secondPhase)
    {
      this.StopAllCoroutines();
      this.CleanupEventInstances();
      MMVibrate.StopRumble();
      this.ClearPaths();
      this.StartCoroutine((IEnumerator) this.TriggerSecondPhase());
    }
    else if ((double) this.attackTimer <= 0.0)
      this.DoNextAttack();
    else if ((double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < 4.0 && (double) Time.time > (double) this.slamAttackTimestamp)
    {
      this.closeByTimer += Time.deltaTime;
      if ((double) this.closeByTimer <= 2.0)
        return;
      this.LightningSlamAttack();
    }
    else
    {
      this.closeByTimer = 0.0f;
      if ((double) Time.time <= (double) this.moveTimer || this.secondPhase)
        return;
      this.moveTimer = Time.time + 4f;
      Vector3 vector3 = this.transform.position;
      while ((double) Vector3.Distance(vector3, this.transform.position) < 6.0)
        vector3 = (Vector3) (UnityEngine.Random.insideUnitCircle * 8f);
      this.givePath(vector3);
      this.Spine.AnimationState.SetAnimation(0, this.MovingAnimation, true);
    }
  }

  public void DoNextAttack()
  {
    ++this.currentAttackIndex;
    if (this.currentAttackIndex > this.attackOrder.Length - 1)
    {
      this.ShuffleAttackOrder();
      this.currentAttackIndex = 0;
    }
    switch (this.attackOrder[this.currentAttackIndex])
    {
      case EnemyLightningMiniboss.Attacks.Laser:
        this.LightningBeamAttack();
        break;
      case EnemyLightningMiniboss.Attacks.SpinnerSmall:
        this.SpinnerSmallAttack();
        break;
      case EnemyLightningMiniboss.Attacks.SpinnerLarge:
        this.DoLargeSpinnerAttackDecision();
        break;
      case EnemyLightningMiniboss.Attacks.BombShower:
        this.BombShower();
        break;
    }
  }

  public void DoLargeSpinnerAttackDecision()
  {
    if (!this.secondPhase)
    {
      if ((double) Vector3.Distance(this.transform.position, Vector3.zero) > 2.0)
        this.StartCoroutine((IEnumerator) this.TeleportIE(Vector3.zero, (System.Action) (() => this.SpinnerLargePingPongAttack())));
      else
        this.SpinnerLargePingPongAttack();
    }
    else
    {
      if ((double) Vector3.Distance(this.transform.position, Vector3.zero) <= 2.0)
        return;
      this.StartCoroutine((IEnumerator) this.TeleportIE(Vector3.zero, (System.Action) (() =>
      {
        if ((double) UnityEngine.Random.value < 0.5)
          this.StartCoroutine((IEnumerator) this.SpinnerAttackIE(this.spinnerHuge, false, allowedSecondaryAttack: true));
        else
          this.StartCoroutine((IEnumerator) this.SpinnerAttackIE(this.spinnerHugePingPong, false, allowedSecondaryAttack: true));
      })));
    }
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO);
    MMVibrate.StopRumble();
    this.CleanupEventInstances();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnDestroy()
  {
    MMVibrate.StopRumble();
    this.CleanupEventInstances();
    base.OnDestroy();
  }

  public void CleanupEventInstances()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.beamStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.attackOrbStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopLoop(this.beamLoopInstanceSFX);
    AudioManager.Instance.StopLoop(this.throwLoopInstanceSFX);
    AudioManager.Instance.StopLoop(this.orbLoopInstanceSFX);
    AudioManager.Instance.StopLoop(this.orbNarrowLoopInstanceSFX);
  }

  public void SpinnerSmallAttack()
  {
    this.StartCoroutine((IEnumerator) this.SpinnerAttackIE(this.spinnerSmall, true, true));
  }

  public void SpinnerLargeAttack()
  {
    this.StartCoroutine((IEnumerator) this.SpinnerAttackIE(this.spinnerLarge, false));
  }

  public void SpinnerLargePingPongAttack()
  {
    this.StartCoroutine((IEnumerator) this.SpinnerAttackIE(this.spinnerLargePingPong, false));
  }

  public void flinchFromOrbHit()
  {
    Debug.Log((object) "Flinch from orb hit");
    this.Spine.AnimationState.SetAnimation(0, "hurt", false);
    this.Spine.AnimationState.AddAnimation(0, "attack-orbs-loop", true, 0.0f);
  }

  public IEnumerator SpinnerAttackIE(
    LightningBossOrbSpinner prefab,
    bool canMove,
    bool playNarrowLoop = false,
    bool allowedSecondaryAttack = false)
  {
    EnemyLightningMiniboss lightningMiniboss = this;
    lightningMiniboss.attacking = true;
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, "attack-orbs-start", false);
    lightningMiniboss.Spine.AnimationState.AddAnimation(0, "attack-orbs-loop", true, 0.0f);
    if (!string.IsNullOrEmpty(lightningMiniboss.AttackOrbStartSFX))
      lightningMiniboss.attackOrbStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(lightningMiniboss.AttackOrbStartSFX, lightningMiniboss.transform);
    float teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 1.0)
    {
      lightningMiniboss.SimpleSpineFlash.FlashMeWhite(teleportTime / 1f);
      yield return (object) null;
    }
    lightningMiniboss.SimpleSpineFlash.FlashWhite(false);
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    LightningBossOrbSpinner spinner = UnityEngine.Object.Instantiate<LightningBossOrbSpinner>(prefab, lightningMiniboss.transform).GetComponent<LightningBossOrbSpinner>();
    spinner.gameObject.SetActive(true);
    spinner.parent = lightningMiniboss.gameObject;
    lightningMiniboss.isInvincible = true;
    if (!string.IsNullOrEmpty(lightningMiniboss.AttackBeamLoopSFX) && playNarrowLoop && !string.IsNullOrEmpty(lightningMiniboss.AttackOrbNarrowLoopSFX))
      lightningMiniboss.orbNarrowLoopInstanceSFX = AudioManager.Instance.CreateLoop(lightningMiniboss.AttackOrbNarrowLoopSFX, lightningMiniboss.gameObject, true);
    if (!string.IsNullOrEmpty(lightningMiniboss.AttackOrbLoopSFX))
      lightningMiniboss.orbLoopInstanceSFX = AudioManager.Instance.CreateLoop(lightningMiniboss.AttackOrbLoopSFX, lightningMiniboss.gameObject, true);
    lightningMiniboss.spinningOrbsParent = spinner.gameObject;
    lightningMiniboss.lightningShield.SetActive(true);
    lightningMiniboss.spinnerOrigin.SetActive(true);
    lightningMiniboss.lightningShield.transform.localScale = Vector3.zero;
    lightningMiniboss.lightningShield.transform.DOScale(2f, 0.25f);
    lightningMiniboss.updateDirection = false;
    float lookUpdateTimer = 0.0f;
    float lookUpdateRate = 1f;
    while (spinner.orbDeadCount < spinner.Orbs.Count)
    {
      lookUpdateTimer += Time.deltaTime * lightningMiniboss.Spine.timeScale;
      if ((double) lookUpdateTimer >= (double) lookUpdateRate)
      {
        lookUpdateTimer = 0.0f;
        lightningMiniboss.LookAtTarget();
      }
      if (canMove)
      {
        lightningMiniboss.moveTimer = Time.time + 4f;
        Vector3 vector3 = lightningMiniboss.transform.position;
        while ((double) Vector3.Distance(vector3, lightningMiniboss.transform.position) < 6.0)
          vector3 = (Vector3) (UnityEngine.Random.insideUnitCircle * 8f);
        lightningMiniboss.givePath(vector3);
      }
      yield return (object) null;
    }
    ArrowLightningBeam.ClearBeams();
    lightningMiniboss.attacking = true;
    if (lightningMiniboss.attackRoutine != null)
      lightningMiniboss.StopCoroutine(lightningMiniboss.attackRoutine);
    lightningMiniboss.chargeSparks.gameObject.SetActive(false);
    lightningMiniboss.chargeSphere.gameObject.SetActive(false);
    lightningMiniboss.LightningImpact.SetActive(false);
    MMVibrate.StopRumble();
    lightningMiniboss.CleanupEventInstances();
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.KnockedOutAnimationStart, false);
    lightningMiniboss.Spine.AnimationState.AddAnimation(0, lightningMiniboss.KnockedOutAnimationLoop, true, 0.0f);
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.KnockedOut;
    if (!string.IsNullOrEmpty(lightningMiniboss.AttackOrbVulnerableStartSFX))
      AudioManager.Instance.PlayOneShot(lightningMiniboss.AttackOrbVulnerableStartSFX);
    lightningMiniboss.lightningShield.gameObject.SetActive(false);
    lightningMiniboss.isInvincible = false;
    lightningMiniboss.spinnerOrigin.gameObject.SetActive(false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * lightningMiniboss.Spine.timeScale) < (double) lightningMiniboss.stunDuration)
      yield return (object) null;
    TrackEntry trackEntry = lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.KnockedOutRecoverAnimation, false);
    lightningMiniboss.Spine.AnimationState.AddAnimation(0, lightningMiniboss.IdleAnimation, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * lightningMiniboss.Spine.timeScale) < (double) trackEntry.Animation.Duration)
      yield return (object) null;
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    if (!string.IsNullOrEmpty(lightningMiniboss.AttackOrbVulnerableRecoverSFX))
      AudioManager.Instance.PlayOneShot(lightningMiniboss.AttackOrbVulnerableRecoverSFX);
    lightningMiniboss.attackTimer = UnityEngine.Random.Range(lightningMiniboss.timeBetweenAttacks.x, lightningMiniboss.timeBetweenAttacks.y);
    lightningMiniboss.attacking = false;
    lightningMiniboss.updateDirection = true;
    if (allowedSecondaryAttack)
    {
      if ((double) Vector3.Distance(lightningMiniboss.transform.position, Vector3.zero) > 1.0)
        yield return (object) lightningMiniboss.StartCoroutine((IEnumerator) lightningMiniboss.TeleportIE(Vector3.zero, (System.Action) null));
      if ((double) UnityEngine.Random.value < 0.5)
        lightningMiniboss.StartCoroutine((IEnumerator) lightningMiniboss.SpinnerAttackIE(lightningMiniboss.spinnerHuge, false));
      else
        lightningMiniboss.StartCoroutine((IEnumerator) lightningMiniboss.SpinnerAttackIE(lightningMiniboss.spinnerHugePingPong, false));
    }
  }

  public void Teleport()
  {
    this.attackRoutine = this.StartCoroutine((IEnumerator) this.TeleportIE(Vector3.zero, (System.Action) null));
  }

  public void UpdateDeadOrbParam(int count)
  {
    if (!this.orbLoopInstanceSFX.isValid())
      return;
    AudioManager.Instance.SetEventInstanceParameter(this.orbLoopInstanceSFX, this.ordDeadCountParam, (float) count);
  }

  public IEnumerator TeleportIE(Vector3 destination, System.Action callback)
  {
    EnemyLightningMiniboss lightningMiniboss = this;
    lightningMiniboss.updateDirection = false;
    lightningMiniboss.health.invincible = true;
    lightningMiniboss.attacking = true;
    lightningMiniboss.ClearPaths();
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Teleporting;
    lightningMiniboss.UsePathing = false;
    lightningMiniboss.SeperateObject = false;
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.TeleportAnimation, false);
    lightningMiniboss.Spine.AnimationState.AddAnimation(0, lightningMiniboss.IdleAnimation, true, 0.0f);
    lightningMiniboss.state.facingAngle = lightningMiniboss.state.LookAngle = Utils.GetAngle(lightningMiniboss.transform.position, destination);
    lightningMiniboss.Spine.skeleton.ScaleX = (double) lightningMiniboss.state.LookAngle <= 90.0 || (double) lightningMiniboss.state.LookAngle >= 270.0 ? -1f : 1f;
    lightningMiniboss.teleportOutParticle.gameObject.SetActive(true);
    lightningMiniboss.teleportOutParticle.transform.parent = (Transform) null;
    if (!string.IsNullOrEmpty(lightningMiniboss.TeleportAwaySFX))
      AudioManager.Instance.PlayOneShot(lightningMiniboss.TeleportAwaySFX);
    float teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 0.43333333730697632)
      yield return (object) null;
    lightningMiniboss.teleportInParticle.gameObject.SetActive(true);
    lightningMiniboss.transform.position = destination;
    if (!string.IsNullOrEmpty(lightningMiniboss.TeleportAppearSFX))
      AudioManager.Instance.PlayOneShot(lightningMiniboss.TeleportAppearSFX);
    teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 0.56666666269302368)
      yield return (object) null;
    lightningMiniboss.UsePathing = true;
    lightningMiniboss.SeperateObject = true;
    lightningMiniboss.health.invincible = false;
    System.Action action = callback;
    if (action != null)
      action();
    lightningMiniboss.attackTimer = UnityEngine.Random.Range(lightningMiniboss.timeBetweenAttacks.x, lightningMiniboss.timeBetweenAttacks.y);
    lightningMiniboss.teleportOutParticle.transform.parent = lightningMiniboss.transform;
    lightningMiniboss.teleportOutParticle.transform.localPosition = Vector3.zero;
    lightningMiniboss.updateDirection = true;
  }

  public void LightningSlamAttack()
  {
    this.attackRoutine = this.StartCoroutine((IEnumerator) this.LightningSlamAttackIE(Vector3.zero, (System.Action) null));
  }

  public IEnumerator LightningSlamAttackIE(Vector3 destination, System.Action callback)
  {
    EnemyLightningMiniboss lightningMiniboss = this;
    lightningMiniboss.updateDirection = false;
    lightningMiniboss.attacking = true;
    lightningMiniboss.slamAttackTimestamp = Time.time + lightningMiniboss.slamAttackCooldown;
    lightningMiniboss.ClearPaths();
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    lightningMiniboss.UsePathing = false;
    lightningMiniboss.SeperateObject = false;
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.AttackStartAnimation, false);
    if (!string.IsNullOrEmpty(lightningMiniboss.AttackLightningRingSFX))
      AudioManager.Instance.PlayOneShot(lightningMiniboss.AttackLightningRingSFX, lightningMiniboss.transform.position);
    if (!string.IsNullOrEmpty(lightningMiniboss.WarningVO))
      AudioManager.Instance.PlayOneShot(lightningMiniboss.WarningVO, lightningMiniboss.transform.position);
    float teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 1.4299999475479126)
    {
      lightningMiniboss.SimpleSpineFlash.FlashMeWhite(teleportTime / 1.43f);
      yield return (object) null;
    }
    lightningMiniboss.SimpleSpineFlash.FlashWhite(false);
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.AttackRecoverAnimation, false);
    lightningMiniboss.Spine.AnimationState.AddAnimation(0, lightningMiniboss.IdleAnimation, true, 0.0f);
    LightningRingExplosion.CreateExplosion(lightningMiniboss.transform.position, lightningMiniboss.health.team, lightningMiniboss.health, lightningMiniboss.slamAttackLightningSpeed, 1f, 0.0f);
    teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 2.0)
      yield return (object) null;
    lightningMiniboss.UsePathing = true;
    lightningMiniboss.SeperateObject = true;
    System.Action action = callback;
    if (action != null)
      action();
    lightningMiniboss.attackTimer = 1.5f;
    lightningMiniboss.attacking = false;
    lightningMiniboss.updateDirection = true;
  }

  public void LightningBeamAttack()
  {
    this.attackRoutine = this.StartCoroutine((IEnumerator) this.LightningBeamAttackIE());
  }

  public IEnumerator LightningBeamAttackIE()
  {
    EnemyLightningMiniboss lightningMiniboss = this;
    lightningMiniboss.attacking = true;
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.AttackStartAnimation, false);
    lightningMiniboss.Spine.AnimationState.AddAnimation(0, lightningMiniboss.AttackLoopAnimation, true, 0.0f);
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (!string.IsNullOrEmpty(lightningMiniboss.AttackBeamStartSFX))
      lightningMiniboss.beamStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(lightningMiniboss.AttackBeamStartSFX, lightningMiniboss.transform);
    float teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 1.0)
    {
      lightningMiniboss.SimpleSpineFlash.FlashMeWhite(teleportTime / 1f);
      yield return (object) null;
    }
    lightningMiniboss.SimpleSpineFlash.FlashWhite(false);
    float progress = 0.0f;
    bool isBeamSetup = false;
    bool isBeamReady = false;
    List<ArrowLightningBeam> currentBeams = new List<ArrowLightningBeam>();
    do
    {
      progress += Time.deltaTime * lightningMiniboss.Spine.timeScale;
      if (!isBeamSetup && (double) progress >= 0.5)
      {
        lightningMiniboss.chargeSparks.gameObject.SetActive(true);
        lightningMiniboss.chargeSphere.gameObject.SetActive(true);
        isBeamSetup = true;
        if (!string.IsNullOrEmpty(lightningMiniboss.AttackBeamLoopSFX))
          lightningMiniboss.beamLoopInstanceSFX = AudioManager.Instance.CreateLoop(lightningMiniboss.AttackBeamLoopSFX, lightningMiniboss.LightningImpact, true);
        if (!string.IsNullOrEmpty(lightningMiniboss.AttackVO))
          AudioManager.Instance.PlayOneShot(lightningMiniboss.AttackVO, lightningMiniboss.transform.position);
        MMVibrate.RumbleContinuous(0.5f, 0.75f);
        Vector3[] positions = new Vector3[2]
        {
          lightningMiniboss.beamPosition.transform.position,
          lightningMiniboss.GetClosestTarget().transform.position
        };
        for (int index = 0; (double) index < (double) lightningMiniboss.beamThickness; ++index)
          ArrowLightningBeam.CreateBeam(positions, true, 0.5f, lightningMiniboss.beamAttackDuration, Health.Team.Team2, (Transform) null, startWidthAtZero: true, result: (System.Action<ArrowLightningBeam>) (beam => currentBeams.Add(beam)), signpostDuration: 0.0f);
      }
      if (!isBeamReady && (double) currentBeams.Count == (double) lightningMiniboss.beamThickness)
      {
        isBeamReady = true;
        lightningMiniboss.LightningImpact.SetActive(true);
      }
      if (isBeamSetup & isBeamReady && (double) progress < (double) lightningMiniboss.beamAttackDuration && (double) lightningMiniboss.Spine.timeScale > 0.5)
      {
        for (int index = 0; (double) index < (double) lightningMiniboss.beamThickness; ++index)
          lightningMiniboss.CalculateBeamPosition(currentBeams[index], lightningMiniboss.GetClosestTarget(), lightningMiniboss.LightningImpact);
      }
      yield return (object) null;
    }
    while ((double) progress <= (double) lightningMiniboss.beamAttackDuration);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.StopLoop(lightningMiniboss.beamLoopInstanceSFX);
    MMVibrate.StopRumble();
    lightningMiniboss.chargeSparks.gameObject.SetActive(false);
    lightningMiniboss.chargeSphere.gameObject.SetActive(false);
    lightningMiniboss.LightningImpact.SetActive(false);
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.IdleAnimation, true);
    lightningMiniboss.attackTimer = UnityEngine.Random.Range(lightningMiniboss.timeBetweenAttacks.x, lightningMiniboss.timeBetweenAttacks.y);
    lightningMiniboss.attacking = false;
  }

  public void CalculateBeamPosition(
    ArrowLightningBeam beam,
    Health currentTarget,
    GameObject impactEffect)
  {
    if ((UnityEngine.Object) currentTarget == (UnityEngine.Object) null)
      return;
    this.lerpTarget = Vector3.Lerp(this.lerpTarget, currentTarget.transform.position, this.beamTrackingSpeed * Time.deltaTime);
    beam.UpdatePositions(new Vector3[2]
    {
      this.beamPosition.transform.position,
      this.lerpTarget
    });
    if (!((UnityEngine.Object) impactEffect != (UnityEngine.Object) null))
      return;
    impactEffect.transform.position = new Vector3(this.lerpTarget.x, this.lerpTarget.y, 0.0f);
  }

  public void BombShower()
  {
    this.attackRoutine = this.StartCoroutine((IEnumerator) this.BombShowerIE());
  }

  public IEnumerator BombShowerIE()
  {
    EnemyLightningMiniboss lightningMiniboss = this;
    lightningMiniboss.updateDirection = false;
    lightningMiniboss.attacking = true;
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.AttackStartAnimation, false);
    lightningMiniboss.Spine.AnimationState.AddAnimation(0, lightningMiniboss.AttackLoopAnimation, true, 0.0f);
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (!string.IsNullOrEmpty(lightningMiniboss.AttackThrowStartSFX))
      AudioManager.Instance.PlayOneShot(lightningMiniboss.AttackThrowStartSFX, lightningMiniboss.gameObject);
    if (!string.IsNullOrEmpty(lightningMiniboss.WarningVO))
      AudioManager.Instance.PlayOneShot(lightningMiniboss.WarningVO, lightningMiniboss.transform.position);
    float teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 1.0)
    {
      lightningMiniboss.SimpleSpineFlash.FlashMeWhite(teleportTime / 1f);
      yield return (object) null;
    }
    lightningMiniboss.SimpleSpineFlash.FlashWhite(false);
    lightningMiniboss.throwLoopInstanceSFX = AudioManager.Instance.CreateLoop(lightningMiniboss.AttackThrowLoopSFX, lightningMiniboss.gameObject, true);
    int amount = (int) UnityEngine.Random.Range(lightningMiniboss.bombShowerAmount.x, lightningMiniboss.bombShowerAmount.y + 1f);
    for (int i = 0; i < amount; ++i)
    {
      lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.AttackStartAnimation, false);
      Spine.Animation animation = lightningMiniboss.Spine.AnimationState.Data.SkeletonData.FindAnimation("throw-bomb");
      if (animation != null)
      {
        Vector3 targetPos = (double) UnityEngine.Random.value < 0.20000000298023224 ? lightningMiniboss.GetClosestTarget().transform.position : (Vector3) (UnityEngine.Random.insideUnitCircle * 8f);
        lightningMiniboss.state.facingAngle = lightningMiniboss.state.LookAngle = Utils.GetAngle(lightningMiniboss.transform.position, targetPos);
        float animLength = animation.Duration - 0.33f;
        lightningMiniboss.Spine.AnimationState.SetAnimation(0, animation, false);
        yield return (object) new WaitForSeconds(0.33f);
        lightningMiniboss.DoThrowProjectile(targetPos);
        yield return (object) new WaitForSeconds(animLength);
        targetPos = new Vector3();
      }
      else
      {
        lightningMiniboss.Spine.AnimationState.AddAnimation(0, lightningMiniboss.AttackStartAnimation, false, 0.0f);
        lightningMiniboss.Spine.AnimationState.AddAnimation(0, lightningMiniboss.AttackLoopAnimation, true, 0.0f);
        lightningMiniboss.DoThrowProjectile((double) UnityEngine.Random.value < 0.20000000298023224 ? lightningMiniboss.GetClosestTarget().transform.position : (Vector3) (UnityEngine.Random.insideUnitCircle * 8f));
        yield return (object) new WaitForSeconds(lightningMiniboss.timeBetweenBombs);
      }
    }
    AudioManager.Instance.StopLoop(lightningMiniboss.throwLoopInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.IdleAnimation, true);
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    lightningMiniboss.attackTimer = UnityEngine.Random.Range(lightningMiniboss.timeBetweenAttacks.x, lightningMiniboss.timeBetweenAttacks.y) + 1f;
    lightningMiniboss.attacking = false;
    lightningMiniboss.updateDirection = true;
  }

  public IEnumerator TriggerSecondPhase()
  {
    EnemyLightningMiniboss lightningMiniboss = this;
    MMVibrate.StopRumble();
    lightningMiniboss.CleanupEventInstances();
    lightningMiniboss.secondPhase = true;
    lightningMiniboss.attacking = true;
    if ((double) Vector3.Distance(lightningMiniboss.transform.position, Vector3.zero) > 1.0)
      yield return (object) lightningMiniboss.StartCoroutine((IEnumerator) lightningMiniboss.TeleportIE(Vector3.zero, (System.Action) null));
    if ((double) UnityEngine.Random.value < 0.5)
      lightningMiniboss.StartCoroutine((IEnumerator) lightningMiniboss.SpinnerAttackIE(lightningMiniboss.spinnerHuge, false, allowedSecondaryAttack: true));
    else
      lightningMiniboss.StartCoroutine((IEnumerator) lightningMiniboss.SpinnerAttackIE(lightningMiniboss.spinnerHugePingPong, false, allowedSecondaryAttack: true));
    lightningMiniboss.attackTimer = 1.5f;
  }

  public void DoThrowProjectile(Vector3 targetVector)
  {
    MortarBomb component = UnityEngine.Object.Instantiate<GameObject>(this.projectilePrefab, this.transform.position + (targetVector - this.transform.position).normalized * 8f, Quaternion.identity, this.transform.parent).GetComponent<MortarBomb>();
    component.gameObject.SetActive(true);
    float bombSpeed = this.bombSpeed;
    float moveDuration = Vector2.Distance((Vector2) this.transform.position, (Vector2) component.transform.position) / bombSpeed;
    component.Play(this.beamPosition.transform.position, moveDuration, Health.Team.Team2, PlayDefaultSFX: false, customSFX: this.AttackThrowLaunchSFX);
  }

  public override void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHitEarly(Attacker, AttackLocation, AttackType, FromBehind);
    if (!this.isInvincible)
      return;
    this.health.invincible = true;
    this.health.IsDeflecting = true;
    BiomeConstants.Instance.EmitHitVFX(this.transform.position + Vector3.back, Quaternion.identity.z, "HitFX_Weak");
    this.SimpleSpineFlash.FlashFillRed();
    if ((UnityEngine.Object) UIBossHUD.Instance != (UnityEngine.Object) null)
      UIBossHUD.Instance.Shake();
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      this.health.invincible = false;
      this.health.IsDeflecting = false;
    }));
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
    this.SimpleSpineFlash.FlashFillRed();
  }

  public IEnumerator HurtRoutine(bool playAnimation)
  {
    EnemyLightningMiniboss lightningMiniboss = this;
    lightningMiniboss.damageColliderEvents.SetActive(false);
    lightningMiniboss.ClearPaths();
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.KnockBack;
    lightningMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    if (playAnimation)
      lightningMiniboss.Spine.AnimationState.SetAnimation(0, lightningMiniboss.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 0.5)
      yield return (object) null;
    lightningMiniboss.DisableForces = false;
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyLightningMiniboss lightningMiniboss = this;
    lightningMiniboss.DisableForces = true;
    lightningMiniboss.Angle = Utils.GetAngle(Attacker.transform.position, lightningMiniboss.transform.position) * ((float) Math.PI / 180f);
    lightningMiniboss.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(lightningMiniboss.Angle), 1500f * Mathf.Sin(lightningMiniboss.Angle)) * lightningMiniboss.KnockbackModifier);
    lightningMiniboss.rb.AddForce((Vector2) lightningMiniboss.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * lightningMiniboss.Spine.timeScale) < 0.5)
      yield return (object) null;
    lightningMiniboss.DisableForces = false;
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  [CompilerGenerated]
  public void \u003CDoLargeSpinnerAttackDecision\u003Eb__89_0()
  {
    this.SpinnerLargePingPongAttack();
  }

  [CompilerGenerated]
  public void \u003CDoLargeSpinnerAttackDecision\u003Eb__89_1()
  {
    if ((double) UnityEngine.Random.value < 0.5)
      this.StartCoroutine((IEnumerator) this.SpinnerAttackIE(this.spinnerHuge, false, allowedSecondaryAttack: true));
    else
      this.StartCoroutine((IEnumerator) this.SpinnerAttackIE(this.spinnerHugePingPong, false, allowedSecondaryAttack: true));
  }

  [CompilerGenerated]
  public void \u003COnHitEarly\u003Eb__110_0()
  {
    this.health.invincible = false;
    this.health.IsDeflecting = false;
  }

  public enum Attacks
  {
    Laser,
    SpinnerSmall,
    SpinnerLarge,
    LightningSlam,
    BombShower,
  }
}
