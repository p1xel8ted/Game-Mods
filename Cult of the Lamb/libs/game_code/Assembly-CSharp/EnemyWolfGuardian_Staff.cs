// Decompiled with JetBrains decompiler
// Type: EnemyWolfGuardian_Staff
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyWolfGuardian_Staff : EnemyWolfGuardian
{
  public float StaffRangedAttackDuration = 3f;
  public float StaffSpecialDuration = 5f;
  public float StaffBeamTrackingSpeed = 3f;
  public int BeamThickness = 5;
  public Transform StaffBeamOrigin;
  public ColliderEvents BeamDamageColliderEvents;
  public ColliderEvents SwipeDamageColliderEvents;
  public float DistanceBeforeSlam = 2f;
  public float GroundSlamSpeed = 10f;
  public ParticleSystem chargeSparks;
  public ParticleSystem chargeSphere;
  public AssetReferenceGameObject LightningImpactPrefab;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffAttackOneAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffAttackTwoAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffRangedStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffRangedLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffRangedStopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffSpecialStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffSpecialLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StaffSpecialStopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string GroundSlamAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string GroundSlamStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string GroundSlamMovingAnimation;
  public AssetReferenceGameObject IndicatorPrefabLarge;
  public ParticleSystem GroundSlamImpactParticles;
  [EventRef]
  public string BeamLoopSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/staff_attack_beam_loop";
  [EventRef]
  public string BeamGroundedStartSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/staff_attack_beam_grounded_start";
  [EventRef]
  public string BeamFlyingStartSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/staff_attack_beam_flying_start";
  [EventRef]
  public string AttackMeleeJabSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/staff_attack_jab_start";
  [EventRef]
  public string AttackMeleeFollowupSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/staff_attack_jab_followup";
  public float LightningRingExpansionSpeed = 5f;
  public float LightningEnemyDamage = 4f;
  public float LightningRingMaxRadius = 2f;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;
  public EventInstance beamLoopInstanceSFX;
  public float nextShotTime;
  public Vector3 lerpTarget;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public List<ArrowLightningBeam> currentBeams = new List<ArrowLightningBeam>();
  public GameObject lightningImpact;
  public GameObject loadedSlamIndicator;
  public GameObject currentIndicator;
  public GameObject currentImpactEffect;

  public event UnitObject.Action OnBeamHit;

  public override void Awake()
  {
    base.Awake();
    this.LoadAssets();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!((UnityEngine.Object) this.BeamDamageColliderEvents != (UnityEngine.Object) null))
      return;
    this.BeamDamageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnBeamTriggerEnter);
    this.BeamDamageColliderEvents.SetActive(false);
  }

  public override void Update() => base.Update();

  public override void OnDisable()
  {
    if ((UnityEngine.Object) this.BeamDamageColliderEvents != (UnityEngine.Object) null)
      this.BeamDamageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnBeamTriggerEnter);
    base.OnDisable();
  }

  public void LoadAssets()
  {
    if (this.LightningImpactPrefab != null)
      Addressables.LoadAssetAsync<GameObject>((object) this.LightningImpactPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        EnemyWolfGuardian_Staff.loadedAddressableAssets.Add(obj);
        this.lightningImpact = obj.Result;
        this.lightningImpact.CreatePool(5, true);
      });
    Addressables.LoadAssetAsync<GameObject>((object) this.IndicatorPrefabLarge).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyWolfGuardian_Staff.loadedAddressableAssets.Add(obj);
      this.loadedSlamIndicator = obj.Result;
      this.loadedSlamIndicator.SetActive(false);
    });
  }

  public override void SetupAnimArrays()
  {
    this.meleeAnims = new string[2]
    {
      this.StaffAttackOneAnimation,
      this.StaffAttackTwoAnimation
    };
    this.meleeSfxDictionary = new Dictionary<string, string>()
    {
      {
        this.StaffAttackOneAnimation,
        this.AttackMeleeJabSFX
      },
      {
        this.StaffAttackTwoAnimation,
        this.AttackMeleeFollowupSFX
      }
    };
  }

  public override void LookAtTarget(bool UpdateMeleeColliderPosition = true)
  {
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, closestTarget.transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    if (!UpdateMeleeColliderPosition)
      return;
    this.MeleeColliders.localPosition = new Vector3(1.5f * ((double) closestTarget.transform.position.x > (double) this.transform.position.x ? -1f : 1f), 0.0f, 0.0f);
  }

  public override void GoToRangedAttackState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Staff.StaffRangedAttackMasterState());
  }

  public override void GoToSpecialAttackState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Staff.StaffSpecialAttackMasterState());
  }

  public override void DoAttackHitEvent()
  {
    ColliderEvents eventsForMeleeAttack = this.GetColliderEventsForMeleeAttack();
    if ((UnityEngine.Object) eventsForMeleeAttack != (UnityEngine.Object) null)
      eventsForMeleeAttack.SetActive(true);
    Collider2D[] collider2DArray;
    if ((UnityEngine.Object) eventsForMeleeAttack == (UnityEngine.Object) this.damageColliderEvents)
    {
      BoxCollider2D component = eventsForMeleeAttack.GetComponent<BoxCollider2D>();
      Bounds bounds = component.bounds;
      Vector2 center = (Vector2) bounds.center;
      bounds = component.bounds;
      Vector2 size = (Vector2) bounds.size;
      collider2DArray = Physics2D.OverlapBoxAll(center, size, 0.0f);
    }
    else
    {
      CircleCollider2D component = eventsForMeleeAttack.GetComponent<CircleCollider2D>();
      collider2DArray = Physics2D.OverlapCircleAll((Vector2) eventsForMeleeAttack.transform.position, component.radius);
    }
    foreach (Collider2D collider in collider2DArray)
      this.OnDamageTriggerEnter(collider);
    CameraManager.shakeCamera(0.4f, this.state.facingAngle);
    AudioManager.Instance.PlayOneShot(this.AttackVO, this.transform.position);
    this.DashParticles.Play();
  }

  public ColliderEvents GetColliderEventsForMeleeAttack()
  {
    return this.Spine.AnimationState.GetCurrent(0).Animation.Name == this.StaffAttackTwoAnimation ? this.SwipeDamageColliderEvents : this.damageColliderEvents;
  }

  public override void StopAttack()
  {
    base.StopAttack();
    this.DoBeamCleanup(this.currentImpactEffect, this.currentIndicator);
  }

  public override void DoAttackEndEvent()
  {
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    if ((UnityEngine.Object) this.SwipeDamageColliderEvents != (UnityEngine.Object) null)
      this.SwipeDamageColliderEvents.SetActive(false);
    this.DashParticles.Stop();
  }

  public Vector3 CalculateBeamPosition(Health currentTarget)
  {
    if ((UnityEngine.Object) currentTarget == (UnityEngine.Object) null)
      return Vector3.zero;
    this.lerpTarget = Vector3.Lerp(this.lerpTarget, currentTarget.transform.position, this.StaffBeamTrackingSpeed * Time.deltaTime * this.Spine.timeScale);
    return this.lerpTarget;
  }

  public void UpdateBeamPosition(
    ArrowLightningBeam beam,
    Vector3 targetPosition,
    GameObject impactEffect)
  {
    beam.UpdatePositions(new Vector3[2]
    {
      this.StaffBeamOrigin.position,
      targetPosition
    });
    Vector3 vector3 = new Vector3(this.lerpTarget.x, this.lerpTarget.y, 0.0f);
    if ((UnityEngine.Object) impactEffect != (UnityEngine.Object) null)
      impactEffect.transform.position = vector3;
    this.BeamDamageColliderEvents.transform.position = vector3;
  }

  public void OnBeamTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !component.IsCharmedEnemy)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    UnitObject.Action onBeamHit = this.OnBeamHit;
    if (onBeamHit == null)
      return;
    onBeamHit();
  }

  public override void OnDieEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.DoBeamCleanup(this.currentImpactEffect, this.currentIndicator);
    base.OnDieEarly(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public void ResetLerpTarget(Health currentTarget)
  {
    this.lerpTarget = currentTarget.transform.position;
  }

  public void DoBeamCleanup(GameObject impactEffect, GameObject indicator)
  {
    this.StopChargingEffect();
    if ((bool) (UnityEngine.Object) impactEffect)
      ObjectPool.Recycle(impactEffect);
    if ((bool) (UnityEngine.Object) indicator)
      ObjectPool.Recycle(indicator);
    this.BeamDamageColliderEvents.SetActive(false);
    MMVibrate.StopRumbleForAllPlayers();
    if (!string.IsNullOrEmpty(this.BeamLoopSFX))
      AudioManager.Instance.StopLoop(this.beamLoopInstanceSFX);
    foreach (ArrowLightningBeam currentBeam in this.currentBeams)
      currentBeam.ForceStop(0.2f);
    this.currentBeams.Clear();
    if (string.IsNullOrEmpty(this.BeamLoopSFX))
      return;
    AudioManager.Instance.StopLoop(this.beamLoopInstanceSFX);
  }

  public void StartChargingEffect()
  {
    this.chargeSparks.Play();
    this.chargeSphere.Play();
  }

  public void StopChargingEffect()
  {
    this.chargeSparks.Stop();
    this.chargeSphere.Stop();
  }

  public override void OnDestroy()
  {
    if (!string.IsNullOrEmpty(this.BeamLoopSFX))
      AudioManager.Instance.StopLoop(this.beamLoopInstanceSFX);
    base.OnDestroy();
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__49_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemyWolfGuardian_Staff.loadedAddressableAssets.Add(obj);
    this.lightningImpact = obj.Result;
    this.lightningImpact.CreatePool(5, true);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__49_1(AsyncOperationHandle<GameObject> obj)
  {
    EnemyWolfGuardian_Staff.loadedAddressableAssets.Add(obj);
    this.loadedSlamIndicator = obj.Result;
    this.loadedSlamIndicator.SetActive(false);
  }

  public class StaffRangedAttackMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Staff parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public int childStateIndex;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Ranged;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Staff) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.childStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
      this.childStateMachine.SetParent((EnemyWolfGuardian) this.parent);
      this.SetupStateSequence();
      this.childStateMachine.SetState(this.stateSequence[this.childStateIndex]);
    }

    public override void Update()
    {
      this.childStateMachine.Update();
      if (!this.IsCurrentStateComplete())
        return;
      this.ProgressToNextChildState();
    }

    public override void OnExit()
    {
      if (this.childStateIndex != 1)
        return;
      this.childStateMachine.GetCurrentState().OnExit();
    }

    public void SetupStateSequence()
    {
      this.stateSequence = new SimpleState[3]
      {
        (SimpleState) new EnemyWolfGuardian.JumpAwayState(),
        (SimpleState) new EnemyWolfGuardian_Staff.BeamAttackStateGrounded(),
        (SimpleState) new EnemyWolfGuardian.RecoveryState(this.parent.SpecialAttackRecoveryTime)
      };
    }

    public bool IsCurrentStateComplete() => this.childStateMachine.GetCurrentState().IsComplete();

    public void ProgressToNextChildState()
    {
      ++this.childStateIndex;
      if (this.childStateIndex < this.stateSequence.Length)
      {
        this.childStateMachine.SetState(this.stateSequence[this.childStateIndex]);
      }
      else
      {
        this.parent.ResetAttackCooldown();
        this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
      }
    }
  }

  public class BeamAttackStateGrounded : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Staff parent;
    public Health currentTargetHealth;
    public float progress;
    public float cooldownProgress;
    public float startBeamTime = 1f;
    public float attackDuration;
    public float endBuffer = 0.4f;
    public float cooldownAnimLength;
    public float minimumAttackDuration = 1.5f;
    public bool isBeamSetup;
    public bool isBeamReady;
    public bool isAttackFinished;
    public bool isFinishingUp;
    public bool didBeamHitSomething;
    public List<ArrowLightningBeam> currentBeams = new List<ArrowLightningBeam>();
    public Vector3 indicatorScale = new Vector3(0.5f, 0.5f, 1f);

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Ranged;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Staff) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.SetTargetObject();
      this.currentTargetHealth = this.parent.GetClosestTarget();
      if ((UnityEngine.Object) this.currentTargetHealth == (UnityEngine.Object) null)
      {
        this.parent.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.RecoveryState(this.parent.RangedAttackRecoveryTime));
      }
      else
      {
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.StaffRangedStartAnimation, false);
        this.parent.Spine.AnimationState.AddAnimation(0, this.parent.StaffRangedLoopAnimation, true, 0.0f);
        this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
        this.parent.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(((EnemyWolfGuardian) this.parent).OnDamageTriggerEnter);
        this.attackDuration = this.parent.StaffRangedAttackDuration;
        this.parent.StartChargingEffect();
        this.parent.ResetLerpTarget(this.currentTargetHealth);
        this.parent.currentIndicator = ObjectPool.Spawn(this.parent.loadedSlamIndicator, this.parent.transform.parent);
        this.parent.currentIndicator.transform.position = this.currentTargetHealth.transform.position;
        this.parent.currentIndicator.transform.localScale = this.indicatorScale;
        this.parent.currentIndicator.transform.parent = this.parent.transform.parent;
        this.cooldownAnimLength = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.StaffRangedStopAnimation).Duration;
        if (!string.IsNullOrEmpty(this.parent.BeamGroundedStartSFX))
          AudioManager.Instance.PlayOneShot(this.parent.BeamGroundedStartSFX);
        this.parent.OnBeamHit += new UnitObject.Action(this.OnBeamHit);
      }
    }

    public override void Update()
    {
      if ((UnityEngine.Object) this.currentTargetHealth == (UnityEngine.Object) null)
      {
        this.isFinishingUp = true;
        this.parent.DoBeamCleanup(this.parent.currentImpactEffect, this.parent.currentImpactEffect);
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.StaffRangedStopAnimation, false);
        this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
      }
      this.parent.LookAtTarget(this.currentTargetHealth);
      Vector3 beamPosition = this.parent.CalculateBeamPosition(this.currentTargetHealth);
      this.parent.currentIndicator.transform.position = beamPosition;
      this.parent.currentIndicator.transform.parent = this.parent.transform.parent;
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if (!this.isBeamSetup && (double) this.progress >= (double) this.startBeamTime)
      {
        this.isBeamSetup = true;
        if (!string.IsNullOrEmpty(this.parent.BeamLoopSFX))
          this.parent.beamLoopInstanceSFX = AudioManager.Instance.CreateLoop(this.parent.BeamLoopSFX, this.parent.currentIndicator, true);
        Vector3[] positions = new Vector3[2]
        {
          this.parent.StaffBeamOrigin.position,
          this.parent.TargetObject.transform.position
        };
        for (int index = 0; index < this.parent.BeamThickness; ++index)
          ArrowLightningBeam.CreateBeam(positions, true, 0.5f, this.attackDuration - this.endBuffer - this.startBeamTime, Health.Team.Team2, (Transform) null, true, true, (System.Action<ArrowLightningBeam>) (beam => this.currentBeams.Add(beam)), 0.0f);
        MMVibrate.RumbleContinuousForAllPlayers(0.8f, 1.2f);
      }
      if (!this.isBeamReady && this.currentBeams.Count == this.parent.BeamThickness)
      {
        this.isBeamReady = true;
        this.parent.currentImpactEffect = ObjectPool.Spawn(this.parent.lightningImpact, this.parent.transform.parent);
        this.parent.currentImpactEffect.SetActive(true);
        this.parent.BeamDamageColliderEvents.SetActive(true);
      }
      if (this.isBeamSetup && this.isBeamReady && (UnityEngine.Object) this.currentTargetHealth != (UnityEngine.Object) null && (double) this.progress < (double) this.attackDuration + (double) this.startBeamTime)
      {
        for (int index = 0; index < this.parent.BeamThickness; ++index)
          this.parent.UpdateBeamPosition(this.currentBeams[index], beamPosition, this.parent.currentImpactEffect);
      }
      if (((double) this.progress >= (double) this.attackDuration || this.didBeamHitSomething && (double) this.progress >= (double) this.startBeamTime + (double) this.minimumAttackDuration) && !this.isFinishingUp)
      {
        this.isFinishingUp = true;
        this.parent.DoBeamCleanup(this.parent.currentImpactEffect, this.parent.currentImpactEffect);
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.StaffRangedStopAnimation, false);
        this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
      }
      if (!this.isFinishingUp)
        return;
      this.cooldownProgress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.cooldownProgress <= (double) this.cooldownAnimLength)
        return;
      this.isAttackFinished = true;
    }

    public void OnBeamHit() => this.didBeamHitSomething = true;

    public override void OnExit()
    {
      if (!string.IsNullOrEmpty(this.parent.BeamLoopSFX))
        AudioManager.Instance.StopLoop(this.parent.beamLoopInstanceSFX);
      this.parent.OnBeamHit -= new UnitObject.Action(this.OnBeamHit);
      this.parent.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(((EnemyWolfGuardian) this.parent).OnDamageTriggerEnter);
      this.parent.DoBeamCleanup(this.parent.currentImpactEffect, this.parent.currentIndicator);
    }

    public override bool IsComplete() => this.isAttackFinished;

    [CompilerGenerated]
    public void \u003CUpdate\u003Eb__19_0(ArrowLightningBeam beam) => this.currentBeams.Add(beam);
  }

  public class BeamAttackStateFlying : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Staff parent;
    public Health currentTargetHealth;
    public float progress;
    public float cooldownProgress;
    public float startBeamTime = 1f;
    public float attackDuration;
    public float endBuffer = 0.4f;
    public float cooldownAnimLength;
    public float minimumAttackDuration = 1.5f;
    public bool isBeamSetup;
    public bool isBeamReady;
    public bool isAttackFinished;
    public bool isFinishingUp;
    public bool didBeamHitSomething;
    public Vector3 indicatorScale = new Vector3(0.5f, 0.5f, 1f);

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Staff) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.SetTargetObject();
      this.currentTargetHealth = this.parent.GetClosestTarget();
      if ((UnityEngine.Object) this.currentTargetHealth == (UnityEngine.Object) null)
      {
        this.parent.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.FlyDownState((EnemyWolfGuardian.WarriorTrioSimpleState) new EnemyWolfGuardian.IdleState()));
      }
      else
      {
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.StaffSpecialStartAnimation, false);
        this.parent.Spine.AnimationState.AddAnimation(0, this.parent.StaffSpecialLoopAnimation, true, 0.0f);
        this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
        this.parent.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(((EnemyWolfGuardian) this.parent).OnDamageTriggerEnter);
        this.attackDuration = this.parent.StaffSpecialDuration;
        this.parent.StartChargingEffect();
        this.parent.ResetLerpTarget(this.currentTargetHealth);
        this.parent.currentIndicator = ObjectPool.Spawn(this.parent.loadedSlamIndicator, this.parent.transform.parent);
        this.parent.currentIndicator.transform.position = this.currentTargetHealth.transform.position;
        this.parent.currentIndicator.transform.localScale = this.indicatorScale;
        this.parent.currentIndicator.transform.parent = this.parent.transform.parent;
        this.cooldownAnimLength = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.StaffSpecialStopAnimation).Duration;
        if (!string.IsNullOrEmpty(this.parent.BeamFlyingStartSFX))
          AudioManager.Instance.PlayOneShot(this.parent.BeamFlyingStartSFX);
        this.parent.OnBeamHit += new UnitObject.Action(this.OnBeamHit);
      }
    }

    public override void Update()
    {
      this.parent.LookAtTarget(this.currentTargetHealth);
      Vector3 beamPosition = this.parent.CalculateBeamPosition(this.currentTargetHealth);
      if ((UnityEngine.Object) this.parent.currentIndicator != (UnityEngine.Object) null)
      {
        this.parent.currentIndicator.transform.position = beamPosition;
        this.parent.currentIndicator.transform.parent = this.parent.transform.parent;
      }
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) Vector3.Distance(this.currentTargetHealth.transform.position, this.parent.transform.position) < (double) this.parent.DistanceBeforeSlam && (double) this.parent.Spine.timeScale > 0.0099999997764825821)
      {
        this.parent.DoBeamCleanup(this.parent.currentImpactEffect, (GameObject) null);
        this.parent.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Staff.GroundSlamTravelState());
      }
      if (!this.isBeamSetup && (double) this.progress >= (double) this.startBeamTime)
      {
        this.isBeamSetup = true;
        if (!string.IsNullOrEmpty(this.parent.BeamLoopSFX))
          this.parent.beamLoopInstanceSFX = AudioManager.Instance.CreateLoop(this.parent.BeamLoopSFX, this.parent.currentIndicator, true);
        MMVibrate.RumbleContinuousForAllPlayers(0.8f, 1.2f);
        Vector3[] positions = new Vector3[2]
        {
          this.parent.StaffBeamOrigin.position,
          this.parent.TargetObject.transform.position
        };
        for (int index = 0; index < this.parent.BeamThickness; ++index)
          ArrowLightningBeam.CreateBeam(positions, true, 0.5f, this.attackDuration - this.endBuffer - this.startBeamTime, Health.Team.Team2, (Transform) null, true, true, (System.Action<ArrowLightningBeam>) (beam => this.parent.currentBeams.Add(beam)), 0.0f);
      }
      if (!this.isBeamReady && this.parent.currentBeams.Count == this.parent.BeamThickness)
      {
        this.isBeamReady = true;
        this.parent.currentImpactEffect = ObjectPool.Spawn(this.parent.lightningImpact, this.parent.transform.parent);
        this.parent.currentImpactEffect.SetActive(true);
        this.parent.BeamDamageColliderEvents.SetActive(true);
      }
      if (this.isBeamSetup && this.isBeamReady && (UnityEngine.Object) this.currentTargetHealth != (UnityEngine.Object) null && (double) this.progress < (double) this.attackDuration + (double) this.startBeamTime)
      {
        for (int index = 0; index < this.parent.currentBeams.Count; ++index)
          this.parent.UpdateBeamPosition(this.parent.currentBeams[index], beamPosition, this.parent.currentImpactEffect);
      }
      if (((double) this.progress >= (double) this.attackDuration || this.didBeamHitSomething && (double) this.progress >= (double) this.startBeamTime + (double) this.minimumAttackDuration) && !this.isFinishingUp)
      {
        this.isFinishingUp = true;
        this.parent.DoBeamCleanup(this.parent.currentImpactEffect, this.parent.currentIndicator);
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.StaffSpecialStopAnimation, false);
      }
      if (!this.isFinishingUp)
        return;
      this.cooldownProgress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.cooldownProgress <= (double) this.cooldownAnimLength)
        return;
      this.isAttackFinished = true;
    }

    public void OnBeamHit() => this.didBeamHitSomething = true;

    public override void OnExit()
    {
      if (!string.IsNullOrEmpty(this.parent.BeamLoopSFX))
        AudioManager.Instance.StopLoop(this.parent.beamLoopInstanceSFX);
      this.parent.OnBeamHit -= new UnitObject.Action(this.OnBeamHit);
      this.parent.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(((EnemyWolfGuardian) this.parent).OnDamageTriggerEnter);
      this.parent.DoBeamCleanup(this.parent.currentImpactEffect, this.parent.currentIndicator);
    }

    public override bool IsComplete() => this.isAttackFinished;

    [CompilerGenerated]
    public void \u003CUpdate\u003Eb__18_0(ArrowLightningBeam beam)
    {
      this.parent.currentBeams.Add(beam);
    }
  }

  public class StaffSpecialAttackMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public int childStateIndex;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.childStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
      this.childStateMachine.SetParent(this.parent);
      this.SetupStateSequence();
      this.childStateMachine.SetState(this.stateSequence[this.childStateIndex]);
    }

    public override void Update()
    {
      this.childStateMachine.Update();
      if (!this.IsCurrentStateComplete())
        return;
      this.ProgressToNextSubState();
    }

    public override void OnExit() => this.childStateMachine.GetCurrentState().OnExit();

    public void SetupStateSequence()
    {
      this.stateSequence = new SimpleState[4]
      {
        (SimpleState) new EnemyWolfGuardian.FlyUpState(),
        (SimpleState) new EnemyWolfGuardian_Staff.BeamAttackStateFlying(),
        (SimpleState) new EnemyWolfGuardian.FlyDownState(),
        (SimpleState) new EnemyWolfGuardian.RecoveryState(this.parent.SpecialAttackRecoveryTime)
      };
    }

    public bool IsCurrentStateComplete() => this.childStateMachine.GetCurrentState().IsComplete();

    public void ProgressToNextSubState()
    {
      ++this.childStateIndex;
      if (this.childStateIndex < this.stateSequence.Length)
      {
        this.childStateMachine.SetState(this.stateSequence[this.childStateIndex]);
      }
      else
      {
        this.parent.ResetAttackCooldown();
        this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
      }
    }
  }

  public class GroundSlamTravelState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Staff parent;
    public Vector3 startPos;
    public Vector3 targetPos;
    public float progress;
    public float flightProgress;
    public float distance;
    public float timeBeforeMove;
    public float flightDuration;
    public Vector3 indicatorScale = Vector3.one;
    public float indicatorLerpDuration = 0.5f;
    public Health currentTargetHealth;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Staff) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.GroundSlamStartAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.GroundSlamMovingAnimation, true, 0.0f);
      this.currentTargetHealth = this.parent.SetTargetObject();
      if (!string.IsNullOrEmpty(this.parent.AirSlamSFX))
        AudioManager.Instance.PlayOneShot(this.parent.AirSlamSFX);
      if ((UnityEngine.Object) this.currentTargetHealth == (UnityEngine.Object) null)
      {
        this.parent.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.FlyDownState((EnemyWolfGuardian.WarriorTrioSimpleState) new EnemyWolfGuardian.IdleState()));
        if ((UnityEngine.Object) this.parent.currentIndicator != (UnityEngine.Object) null)
          ObjectPool.Recycle(this.parent.currentIndicator);
      }
      this.parent.LookAtTarget(this.currentTargetHealth);
      this.startPos = this.parent.transform.position;
      if ((UnityEngine.Object) this.parent.currentIndicator == (UnityEngine.Object) null)
      {
        this.parent.currentIndicator = ObjectPool.Spawn(this.parent.loadedSlamIndicator, this.targetPos, Quaternion.identity);
        this.parent.currentIndicator.transform.parent = this.parent.transform.parent;
      }
      if ((UnityEngine.Object) this.parent.currentIndicator != (UnityEngine.Object) null)
      {
        this.parent.currentIndicator.SetActive(true);
        this.parent.currentIndicator.transform.DOScale(this.indicatorScale, this.indicatorLerpDuration);
      }
      this.timeBeforeMove = this.parent.Spine.Skeleton.Data.FindAnimation(this.parent.GroundSlamStartAnimation).Duration;
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress < (double) this.timeBeforeMove && (double) this.parent.Spine.timeScale > 0.0099999997764825821)
      {
        this.UpdateIndicatorPosition();
        this.parent.LookAtTarget(this.currentTargetHealth);
      }
      if ((double) this.progress >= (double) this.timeBeforeMove)
      {
        this.flightProgress += Time.deltaTime * this.parent.Spine.timeScale;
        this.parent.transform.position = this.parent.CalculateFlyProgress(this.flightProgress, this.flightDuration, this.startPos, this.targetPos, this.parent.EaseInBackCurve);
        if ((double) this.flightProgress < (double) this.flightDuration)
        {
          foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
            simpleSpineFlash.FlashWhite(Mathf.Clamp01(this.progress / this.flightDuration));
        }
      }
      if ((double) this.flightProgress < (double) this.flightDuration)
        return;
      this.parent.transform.position = this.targetPos;
      this.parent.health.invincible = false;
      this.parent.LockToGround = true;
      this.parent.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Staff.GroundSlamImpactState());
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(0.0f);
    }

    public void UpdateIndicatorPosition()
    {
      if (!((UnityEngine.Object) this.parent.currentIndicator != (UnityEngine.Object) null))
        return;
      this.targetPos = this.parent.TargetObject.transform.position;
      this.distance = Vector3.Distance(this.startPos, this.targetPos);
      this.flightDuration = this.distance / this.parent.GroundSlamSpeed;
      this.parent.currentIndicator.transform.position = this.targetPos;
      this.parent.currentIndicator.transform.parent = this.parent.transform.parent;
    }

    public override void OnExit()
    {
      if (!((UnityEngine.Object) this.parent.currentIndicator != (UnityEngine.Object) null))
        return;
      this.parent.currentIndicator.SetActive(false);
    }
  }

  public class GroundSlamImpactState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Staff parent;
    public float progress;
    public float animationDuration;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Staff) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.GroundSlamAnimation, false);
      this.animationDuration = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.GroundSlamAnimation).Duration;
      this.parent.GroundSlamImpactParticles.Play();
      BiomeConstants.Instance.EmitHammerEffects(this.parent.transform.position, cameraShakeIntensityMax: 1.5f, cameraShakeDuration: 0.5f);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      if ((UnityEngine.Object) this.parent.warriorTrioManager != (UnityEngine.Object) null)
        LightningRingExplosion.CreateExplosion(this.parent.transform.position, this.parent.health.team, this.parent.health, this.parent.LightningRingExpansionSpeed, 1f, this.parent.LightningEnemyDamage, maxRadiusTarget: this.parent.LightningRingMaxRadius);
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(0.0f);
      ObjectPool.Recycle(this.parent.currentIndicator);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress < (double) this.animationDuration)
        return;
      this.parent.ResetAttackCooldown();
      this.parent.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
    }

    public override void OnExit()
    {
      this.parent.DoAttackEndEvent();
      this.parent.damageColliderEvents.SetActive(false);
      this.parent.SetStatusImmunity(false);
    }
  }
}
