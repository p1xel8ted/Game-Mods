// Decompiled with JetBrains decompiler
// Type: EnemyWolfGuardian
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyWolfGuardian : UnitObject
{
  public static float LastWolfGuardianAttacked = float.MinValue;
  public EnemyWolfGuardian.WeaponVarient WeaponVariant;
  public EnemyWolfGuardian.AttackTypes[] randomAttackOne = new EnemyWolfGuardian.AttackTypes[1];
  public EnemyWolfGuardian.AttackTypes[] randomAttackTwo = new EnemyWolfGuardian.AttackTypes[2]
  {
    EnemyWolfGuardian.AttackTypes.Melee,
    EnemyWolfGuardian.AttackTypes.Ranged
  };
  public EnemyWolfGuardian.AttackTypes[] randomAttackThree = new EnemyWolfGuardian.AttackTypes[1]
  {
    EnemyWolfGuardian.AttackTypes.Special
  };
  public EnemyWolfGuardian.AttackTypes[] randomAttackFour = new EnemyWolfGuardian.AttackTypes[1]
  {
    EnemyWolfGuardian.AttackTypes.Special
  };
  public float JumpAwayMinDistance = 4f;
  public float JumpAwayMaxDistance = 8f;
  public float JumpHeight = 4f;
  public float JumpDuration = 0.8f;
  public float RangedAttackRecoveryTime = 0.5f;
  public float MeleeRange = 3f;
  public float SpecialAttackRecoveryTime = 1f;
  public float SwordLightningTime = 3f;
  public AnimationCurve EaseOutBackCurve;
  public AnimationCurve EaseInBackCurve;
  public float FlyUpTime = 2f;
  public float FlyDownTime = 2f;
  public float FlyHeight = -2f;
  public float AttackForce = 1000f;
  [SerializeField]
  public bool requireLineOfSite = true;
  public Transform MeleeColliders;
  public ColliderEvents damageColliderEvents;
  public Collider2D meleeCollider;
  public SkeletonAnimation Spine;
  public ParticleSystem DashParticles;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string JumpAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string WalkAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FlyingAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FlyingStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FlyingLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FlyingStopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string TauntAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FalseDeathStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FalseDeathAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FalseDeathEndAnimation;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  [EventRef]
  public string AttackVO = "event:/enemy/vocals/humanoid_large/attack";
  [EventRef]
  public string AttackLongVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/attack_long";
  [EventRef]
  public string onHitSoundSFX = "event:/enemy/impact_blunt";
  [EventRef]
  public string WarningVO = "event:/enemy/vocals/humanoid_large/warning";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/gethit";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/death";
  [EventRef]
  public string FlyUpSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/shared_mv_fly_start";
  [EventRef]
  public string AirSlamSFX = string.Empty;
  [EventRef]
  public string WingFlapSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/shared_mv_fly_start";
  [EventRef]
  public string JumpSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/shared_mv_jump_launch";
  [EventRef]
  public string JumpLandSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/shared_mv_jump_land";
  [EventRef]
  public string IntroDrawWeaponSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/intro_draw_weapon";
  [EventRef]
  public string BossSwoopOutSFX = string.Empty;
  [EventRef]
  public string IncapacitateSFX = string.Empty;
  public EventInstance meleeEventInstanceSFX;
  public DeadBodySliding deadBodySliding;
  [CompilerGenerated]
  public GameObject \u003CTargetObject\u003Ek__BackingField;
  public HeadDirectionManager headDirectionManager;
  public EnemyWolfGuardian.WolfGuardianStateMachine logicStateMachine;
  public SimpleSpineFlash SimpleSpineFlash;
  public List<Collider2D> collider2DList;
  public Collider2D HealthCollider;
  public Vector3 TargetPosition;
  public Health EnemyHealth;
  public EnemyWolfGuardian.AttackTypes[][] attackSequence;
  public float GlobalAttackDelay = 2f;
  public float GlobalRingAttackDelay = 10f;
  public float GlobalPatternAttackDelay = 5f;
  public float beamAttackCooldownCounter;
  public float groundSlamCooldownCounter;
  public float airSlashCooldownCounter;
  public float circleCastRadius = 0.5f;
  public float circleCastOffset = 1f;
  public float swordSpecialDropOffsetTime = 0.5f;
  public float warriorTrioAttackCooldown = 1.5f;
  public float attackCooldownTimer;
  public float repathTimer;
  public static List<EnemyWolfGuardian> WolfGuardians = new List<EnemyWolfGuardian>();
  public bool ChasingPlayer;
  public float rangedAttackCooldownCounter;
  public string[] meleeAnims;
  public Dictionary<string, string> meleeSfxDictionary;
  public int attackSequenceIndex;
  public WarriorTrioManager warriorTrioManager;
  public int InActivePositionIndex = -1;

  public GameObject TargetObject
  {
    get => this.\u003CTargetObject\u003Ek__BackingField;
    set => this.\u003CTargetObject\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    this.SetupAnimArrays();
    this.state = this.GetComponent<StateMachine>();
    this.headDirectionManager = this.GetComponent<HeadDirectionManager>();
    if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null)
      EnemyWolfGuardian.LastWolfGuardianAttacked = GameManager.GetInstance().CurrentTime;
    this.attackSequence = new EnemyWolfGuardian.AttackTypes[4][]
    {
      this.randomAttackOne,
      this.randomAttackTwo,
      this.randomAttackThree,
      this.randomAttackFour
    };
    this.warriorTrioManager = this.GetComponentInParent<WarriorTrioManager>();
    if (this.logicStateMachine != null)
      return;
    this.SetupStateMachine((UnityEngine.Object) this.warriorTrioManager == (UnityEngine.Object) null);
  }

  public void SetupStateMachine(bool setIdle = true)
  {
    this.logicStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
    this.logicStateMachine.SetParent(this);
    if (!setIdle)
      return;
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
  }

  public virtual void SetupAnimArrays()
  {
  }

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    if (!EnemyWolfGuardian.WolfGuardians.Contains(this))
      EnemyWolfGuardian.WolfGuardians.Add(this);
    this.SimpleSpineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
    this.HealthCollider = this.GetComponent<Collider2D>();
    this.DashParticles.Stop();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.health.OnAddCharm += new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared += new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnPoisonedHit += new Health.HitAction(((UnitObject) this).OnHit);
    this.health.OnBurnHit += new Health.HitAction(((UnitObject) this).OnHit);
    this.TargetPosition = this.transform.position;
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    EnemyWolfGuardian.LastWolfGuardianAttacked = GameManager.GetInstance().CurrentTime;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyWolfGuardian.WolfGuardians.Remove(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.health.OnAddCharm -= new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared -= new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnPoisonedHit -= new Health.HitAction(((UnitObject) this).OnHit);
    this.health.OnBurnHit -= new Health.HitAction(((UnitObject) this).OnHit);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.CleanupEventInstances();
  }

  public override void Update()
  {
    base.Update();
    this.logicStateMachine.Update();
    if (!((UnityEngine.Object) this.warriorTrioManager != (UnityEngine.Object) null) || (double) this.attackCooldownTimer <= 0.0)
      return;
    this.attackCooldownTimer -= Time.deltaTime * this.Spine.timeScale;
  }

  public virtual void CleanupEventInstances()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.meleeEventInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void GoToTauntState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.TauntState());
  }

  public void GoToFalseDeathState()
  {
    if ((UnityEngine.Object) this.warriorTrioManager != (UnityEngine.Object) null)
      this.warriorTrioManager.IncrementFalseDeaths();
    this.CleanupEventInstances();
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.FalseDeathState());
  }

  public void GoToFalseDeathRecoveryState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.FalseDeathEndState());
  }

  public void GoToWaitStateForCutscene(Vector3 finalPosition)
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.WaitMasterState(finalPosition));
  }

  public void GoToFlyInState(
    Vector3 endPosition,
    float duration,
    bool completeAfter,
    bool lookAtPlayer)
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.FlyAwayState(endPosition, duration, completeAfter, lookAtPlayer));
  }

  public void GoToIntroPrepState(Vector3 startPos)
  {
    this.state = this.GetComponent<StateMachine>();
    this.headDirectionManager = this.GetComponent<HeadDirectionManager>();
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IntroState(startPos));
  }

  public void ResetAttackCooldown()
  {
    if (!((UnityEngine.Object) this.warriorTrioManager != (UnityEngine.Object) null))
      return;
    switch (this.warriorTrioManager.ActiveWarriors.Count)
    {
      case 1:
        this.attackCooldownTimer = 0.0f;
        break;
      case 2:
        this.attackCooldownTimer = this.warriorTrioAttackCooldown;
        break;
      case 3:
        this.attackCooldownTimer = this.warriorTrioAttackCooldown * 2f;
        break;
    }
  }

  public virtual void DoAttackHitEvent()
  {
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(true);
    foreach (Collider2D collider in Physics2D.OverlapCircleAll((Vector2) this.damageColliderEvents.transform.position, ((CircleCollider2D) this.meleeCollider).radius))
      this.OnDamageTriggerEnter(collider);
    CameraManager.shakeCamera(0.4f, this.state.facingAngle);
    AudioManager.Instance.PlayOneShot(this.AttackVO, this.transform.position);
    this.DashParticles.Play();
  }

  public virtual void DoAttackEndEvent()
  {
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    this.DashParticles.Stop();
  }

  public void DoWingFlapEvent()
  {
    if (string.IsNullOrEmpty(this.WingFlapSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.WingFlapSFX, this.transform.position);
  }

  public void DoJumpLandEvent()
  {
    if (string.IsNullOrEmpty(this.JumpLandSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.JumpLandSFX, this.transform.position);
  }

  public EnemyWolfGuardian.AttackTypes GetCurrentAttackType()
  {
    EnemyWolfGuardian.AttackTypes[] attackTypesArray = this.attackSequence[this.attackSequenceIndex];
    return attackTypesArray[UnityEngine.Random.Range(0, attackTypesArray.Length)];
  }

  public void IncrementAttackSequenceIndex()
  {
    ++this.attackSequenceIndex;
    if (this.attackSequenceIndex < this.attackSequence.Length)
      return;
    this.attackSequenceIndex = 0;
  }

  public Health SetTargetObject()
  {
    Health closestTarget = this.GetClosestTarget();
    if ((bool) (UnityEngine.Object) closestTarget)
    {
      this.TargetObject = closestTarget.gameObject;
      this.requireLineOfSite = false;
      this.VisionRange = int.MaxValue;
    }
    return closestTarget;
  }

  public void ReconsiderTarget()
  {
    this.TargetObject = (GameObject) null;
    this.SetTargetObject();
  }

  public virtual void LookAtTarget(bool UpdateMeleeColliderPosition = true)
  {
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, closestTarget.transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    if (!UpdateMeleeColliderPosition)
      return;
    this.MeleeColliders.localPosition = (closestTarget.transform.position - this.transform.position).normalized * 1.5f;
  }

  public void SetStatusImmunity(bool immune)
  {
    bool flag = !immune;
    this.health.CanBeBurned = flag;
    this.health.CanBeElectrified = flag;
    this.health.CanBeIced = flag;
    this.health.CanBePoisoned = flag;
    if (flag)
      return;
    this.health.ClearBurn();
    this.health.ClearCharm();
    this.health.ClearIce();
    this.health.ClearPoison();
  }

  public void LookAtTarget(Health target, bool UpdateMeleeColliderPosition = true)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, target.transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    if (!UpdateMeleeColliderPosition)
      return;
    this.MeleeColliders.position = this.transform.position + (target.transform.position - this.transform.position).normalized * 1.5f;
  }

  public void LookAtPosition(Vector3 position)
  {
    float angle = Utils.GetAngle(this.transform.position, position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public void ForceHeadLookAtTarget(Vector3 target)
  {
    this.LookAtPosition(target);
    this.headDirectionManager.ForceUpdate();
  }

  public float GetTauntAnimDuration()
  {
    return this.Spine.skeleton.Data.FindAnimation(this.TauntAnimation).Duration;
  }

  public float GetRecoveryAnimDuration()
  {
    return this.Spine.skeleton.Data.FindAnimation(this.FalseDeathEndAnimation).Duration;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if ((UnityEngine.Object) this.warriorTrioManager != (UnityEngine.Object) null)
      this.warriorTrioManager.OnWarriorDamaged(this);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO);
    if (!string.IsNullOrEmpty(this.onHitSoundSFX))
      AudioManager.Instance.PlayOneShot(this.onHitSoundSFX, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public override void OnDieEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDieEarly(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!((UnityEngine.Object) this.warriorTrioManager != (UnityEngine.Object) null) || this.warriorTrioManager.IsInFinalPhase)
      return;
    this.health.invincible = true;
    this.ClearPaths();
    this.SimpleSpineFlash.FlashFillRed();
    this.StopAllCoroutines();
    this.ClearKnocbackRoutine();
    this.GoToFalseDeathState();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.CleanupEventInstances();
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StopAllCoroutines();
    if (!((UnityEngine.Object) this.warriorTrioManager != (UnityEngine.Object) null))
      return;
    this.warriorTrioManager.IncrementFinalPhaseDeaths();
    this.warriorTrioManager.OnWarriorDamaged(this);
  }

  public void TravelToTarget()
  {
    if (this.logicStateMachine.GetCurrentState() is EnemyWolfGuardian.InActiveMasterState)
      return;
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      this.TargetObject = this.GetClosestTarget().gameObject;
    this.ChasingPlayer = false;
    Vector3 position = this.TargetObject.transform.position;
    float num = (double) position.x > (double) this.transform.position.x ? -this.MeleeRange : this.MeleeRange;
    this.TargetPosition = new Vector3(position.x + num, position.y, position.z);
    this.ChasingPlayer = true;
    if (!this.IsCloseToMeleeAttackPosition())
    {
      this.givePath(this.TargetPosition);
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

  public bool IsCloseToMeleeAttackPosition()
  {
    return (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.TargetPosition) - (double) this.StoppingDistance - 1.4012984643248171E-45 <= (double) this.StoppingDistance;
  }

  public virtual void GoToRangedAttackState()
  {
  }

  public virtual void GoToSpecialAttackState()
  {
  }

  public virtual void GoToMeleeAttackState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.MeleeAttackState());
  }

  public virtual void GoToIdleState()
  {
    this.CleanupEventInstances();
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
  }

  public virtual void GoToInActiveState(Vector3 position, float duration)
  {
    this.CleanupEventInstances();
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.InActiveMasterState(position, duration));
  }

  public virtual void GoToActiveState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.ActiveMasterState());
  }

  public bool IsInFalseDeathState()
  {
    return this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemyWolfGuardian.FalseDeathState);
  }

  public Vector3 GetEmptySpaceInRange(float maxRange)
  {
    Vector3 emptySpaceInRange = this.transform.position;
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      Vector3 vector3 = this.TargetObject.transform.position + new Vector3(Mathf.Cos(f), Mathf.Sin(f)) * maxRange;
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.TargetObject.transform.position, this.circleCastRadius, (Vector2) Vector3.Normalize(vector3 - this.TargetObject.transform.position), maxRange, (int) this.layerToCheck);
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

  public Vector3 GetJumpMovementCurve(
    Vector3 startPosition,
    Vector3 targetPosition,
    float jumpHeight)
  {
    return startPosition + (targetPosition - startPosition) / 2f + Vector3.back * jumpHeight;
  }

  public Vector3 CalculateJumpProgress(
    float progress,
    float duration,
    Vector3 startPos,
    Vector3 targetPos,
    Vector3 movementCurve)
  {
    return Vector3.Lerp(Vector3.Lerp(startPos, movementCurve, progress / duration), Vector3.Lerp(movementCurve, targetPos, progress / duration), progress / duration);
  }

  public Vector3 CalculateFlyProgress(
    float progress,
    float duration,
    Vector3 startPos,
    Vector3 targetPos,
    AnimationCurve easeCurve)
  {
    float time = progress / duration;
    float t = easeCurve.Evaluate(time);
    return Vector3.Lerp(startPos, targetPos, t);
  }

  public float GetAnimationDuration(string animationName)
  {
    return this.Spine.skeleton.Data.FindAnimation(animationName).Duration;
  }

  public bool IsTargetWithinRange(float range)
  {
    return (double) Vector3.Distance(this.TargetObject.transform.position, this.transform.position) <= (double) range;
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !component.IsCharmedEnemy)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public virtual void StopAttack()
  {
    this.ClearPaths();
    this.StopAllCoroutines();
    this.ClearKnocbackRoutine();
  }

  public bool IsAttackInProgress(EnemyWolfGuardian.StateType attack)
  {
    return ((EnemyWolfGuardian.WarriorTrioSimpleState) this.logicStateMachine.GetCurrentState()).CurrentStateType == attack;
  }

  public void ClearKnocbackRoutine()
  {
    if (!this.DisableForces || this.knockRoutine == null)
      return;
    this.DisableForces = false;
    this.knockRoutine = (Coroutine) null;
  }

  public enum WeaponVarient
  {
    Staff,
    Axe,
    Sword,
  }

  public enum AttackTypes
  {
    Melee,
    Ranged,
    Special,
  }

  public enum StateType
  {
    None,
    Melee,
    Ranged,
    Special,
    Idle,
    Movement,
  }

  public class WarriorTrioSimpleState : SimpleState
  {
    public virtual EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.None;

    public EnemyWolfGuardian.StateType CurrentStateType => this.StateType;

    public override void OnEnter()
    {
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
    }
  }

  public class WolfGuardianStateMachine : SimpleStateMachine
  {
    [CompilerGenerated]
    public EnemyWolfGuardian \u003CParent\u003Ek__BackingField;

    public EnemyWolfGuardian Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set => this.\u003CParent\u003Ek__BackingField = value;
    }

    public void SetParent(EnemyWolfGuardian parent) => this.Parent = parent;
  }

  public class IdleState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Idle;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
    }

    public override void Update()
    {
      if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive)
        return;
      this.parent.SetTargetObject();
      if ((UnityEngine.Object) this.parent.warriorTrioManager != (UnityEngine.Object) null && (double) this.parent.attackCooldownTimer > 0.0)
      {
        this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.GoToRandomPositionState());
      }
      else
      {
        if (!((UnityEngine.Object) this.parent.TargetObject != (UnityEngine.Object) null))
          return;
        EnemyWolfGuardian.AttackTypes currentAttackType = this.parent.GetCurrentAttackType();
        this.parent.LookAtTarget();
        float a = Vector3.Distance(this.parent.TargetObject.transform.position, this.parent.transform.position);
        switch (currentAttackType)
        {
          case EnemyWolfGuardian.AttackTypes.Melee:
            if ((UnityEngine.Object) this.parent.warriorTrioManager != (UnityEngine.Object) null && this.parent.warriorTrioManager.IsAttackInProgress(EnemyWolfGuardian.StateType.Melee))
            {
              this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.GoToRandomPositionState());
              break;
            }
            if ((double) a > (double) this.parent.VisionRange || this.parent.requireLineOfSite && !this.parent.CheckLineOfSightOnTarget(this.parent.TargetObject, this.parent.TargetObject.transform.position, Mathf.Min(a, (float) this.parent.VisionRange)))
              break;
            this.parent.IncrementAttackSequenceIndex();
            this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.ChasePlayerState());
            break;
          case EnemyWolfGuardian.AttackTypes.Ranged:
            this.parent.IncrementAttackSequenceIndex();
            this.parent.GoToRangedAttackState();
            break;
          case EnemyWolfGuardian.AttackTypes.Special:
            if ((UnityEngine.Object) this.parent.warriorTrioManager != (UnityEngine.Object) null && this.parent.warriorTrioManager.IsAttackInProgress(EnemyWolfGuardian.StateType.Special))
            {
              this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.GoToRandomPositionState());
              break;
            }
            this.parent.IncrementAttackSequenceIndex();
            this.parent.GoToSpecialAttackState();
            break;
        }
      }
    }

    public override void OnExit()
    {
    }
  }

  public class ChasePlayerState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Melee;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.WalkAnimation, true);
      this.parent.LookAtTarget();
    }

    public override void Update()
    {
      if ((UnityEngine.Object) this.parent.TargetObject == (UnityEngine.Object) null)
        this.parent.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
      this.parent.LookAtTarget();
      if (this.parent.IsCloseToMeleeAttackPosition())
      {
        this.parent.GoToMeleeAttackState();
      }
      else
      {
        if ((double) (this.parent.repathTimer += Time.deltaTime * this.parent.Spine.timeScale) <= 0.20000000298023224)
          return;
        this.parent.repathTimer = 0.0f;
        this.parent.TravelToTarget();
        this.parent.LookAtTarget();
      }
    }

    public override void OnExit()
    {
    }
  }

  public class MeleeAttackState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public float currentAnimTime;
    public float progress;
    public float currentAnimEventTimeStamp;
    public int attackCount = 2;
    public int currentAttackIndex = -1;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Melee;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.attackCount = this.parent.meleeAnims.Length;
      this.parent.LookAtTarget();
    }

    public override void Update()
    {
      if ((double) this.progress >= (double) this.currentAnimTime)
      {
        ++this.currentAttackIndex;
        if (this.currentAttackIndex >= this.attackCount)
        {
          this.parent.ResetAttackCooldown();
          this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
        }
        else
        {
          this.parent.LookAtTarget();
          Spine.Animation currentMeleeAnim = this.GetCurrentMeleeAnim(this.currentAttackIndex);
          string currentMeleeSfx = this.GetCurrentMeleeSFX(this.currentAttackIndex);
          this.currentAnimTime = currentMeleeAnim.Duration;
          this.currentAnimEventTimeStamp = this.GetFirstEventTimeStamp(currentMeleeAnim);
          this.parent.Spine.AnimationState.SetAnimation(0, currentMeleeAnim, false);
          this.parent.DisableForces = true;
          this.parent.rb.AddForce((Vector2) (Vector3) new Vector2(this.parent.AttackForce * Mathf.Cos(this.parent.state.LookAngle * ((float) Math.PI / 180f)), this.parent.AttackForce * Mathf.Sin(this.parent.state.LookAngle * ((float) Math.PI / 180f))));
          if (!string.IsNullOrEmpty(currentMeleeSfx))
            this.parent.meleeEventInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(currentMeleeSfx, this.parent.transform);
          this.progress = 0.0f;
        }
      }
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress > (double) this.currentAnimEventTimeStamp)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(0.0f);
      }
      else
      {
        foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(this.progress / this.currentAnimEventTimeStamp);
      }
    }

    public override void OnExit()
    {
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      this.parent.damageColliderEvents.SetActive(false);
      this.parent.DisableForces = false;
      this.parent.rb.velocity = Vector2.zero;
    }

    public Spine.Animation GetCurrentMeleeAnim(int index)
    {
      return this.parent.Spine.skeleton.Data.FindAnimation(this.parent.meleeAnims[index]);
    }

    public string GetCurrentMeleeSFX(int index)
    {
      return this.parent.meleeSfxDictionary[this.parent.meleeAnims[index]];
    }

    public float GetFirstEventTimeStamp(Spine.Animation animation)
    {
      float firstEventTimeStamp = 0.0f;
      foreach (Timeline timeline in animation.Timelines)
      {
        if (timeline is EventTimeline eventTimeline)
          firstEventTimeStamp = eventTimeline.Events[0].Time;
      }
      return firstEventTimeStamp;
    }
  }

  public class JumpAwayState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public Vector3 jumpStartPos;
    public Vector3 jumpTargetPos;
    public Vector3 movementCurve;
    public bool isJumpingAway;
    public float jumpProgress;
    public float jumpDistance;
    public float targetRangeForJump = 6f;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Ranged;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
      this.parent.SetTargetObject();
      this.jumpDistance = UnityEngine.Random.Range(this.parent.JumpAwayMinDistance, this.parent.JumpAwayMaxDistance);
      this.isJumpingAway = this.parent.IsTargetWithinRange(this.targetRangeForJump);
      if (!this.isJumpingAway)
        return;
      this.jumpStartPos = this.parent.transform.position;
      this.jumpTargetPos = this.parent.GetEmptySpaceInRange(this.jumpDistance);
      this.parent.LockToGround = false;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.JumpAnimation, false);
      this.movementCurve = this.parent.GetJumpMovementCurve(this.jumpStartPos, this.jumpTargetPos, this.parent.JumpHeight);
      if (string.IsNullOrEmpty(this.parent.JumpSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.JumpSFX, this.parent.transform.position);
    }

    public override void Update()
    {
      if (!this.isJumpingAway)
        return;
      if ((double) (this.jumpProgress += Time.deltaTime * this.parent.Spine.timeScale) < (double) this.parent.JumpDuration)
      {
        this.parent.transform.position = this.parent.CalculateJumpProgress(this.jumpProgress, this.parent.JumpDuration, this.jumpStartPos, this.jumpTargetPos, this.movementCurve);
      }
      else
      {
        this.parent.transform.position = this.jumpTargetPos;
        this.jumpTargetPos.z = 0.0f;
        this.parent.LockToGround = true;
        this.parent.LookAtTarget();
        this.isJumpingAway = false;
      }
    }

    public override void OnExit()
    {
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
    }

    public override bool IsComplete() => !this.isJumpingAway;
  }

  public class RecoveryState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public float duration;
    public float progress;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Idle;

    public RecoveryState(float recoveryTime) => this.duration = recoveryTime;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      this.parent.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      this.parent.LookAtTarget();
    }

    public override void OnExit()
    {
    }

    public override bool IsComplete() => (double) this.progress >= (double) this.duration;
  }

  public class FlyUpState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public Vector3 startPos;
    public Vector3 targetPos;
    public float stateProgress;
    public float flyingProgress;
    public float flyUpSpeedMultiplier = 1f;
    public float delayBeforeFlypUp;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public FlyUpState(float flyUpSpeedMultiplier = 1f)
    {
      this.flyUpSpeedMultiplier = flyUpSpeedMultiplier;
    }

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.parent.OnDamageTriggerEnter);
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.FlyingAnticipationAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.FlyingStartAnimation, false, 0.0f);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.FlyingLoopAnimation, true, 0.0f);
      this.parent.LockToGround = false;
      this.parent.health.invincible = true;
      this.startPos = this.parent.transform.position;
      this.targetPos = new Vector3(this.startPos.x, this.startPos.y, this.parent.FlyHeight);
      if (!string.IsNullOrEmpty(this.parent.FlyUpSFX))
        AudioManager.Instance.PlayOneShot(this.parent.FlyUpSFX, this.parent.transform.position);
      this.delayBeforeFlypUp = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.FlyingAnticipationAnimation).Duration;
      this.parent.DashParticles.Stop();
      this.parent.SetStatusImmunity(true);
    }

    public override void Update()
    {
      this.stateProgress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.stateProgress < (double) this.delayBeforeFlypUp)
        return;
      this.flyingProgress += Time.deltaTime * this.parent.Spine.timeScale;
      this.parent.transform.position = this.parent.CalculateFlyProgress(this.flyingProgress, this.parent.FlyUpTime / this.flyUpSpeedMultiplier, this.startPos, this.targetPos, this.parent.EaseOutBackCurve);
    }

    public override void OnExit()
    {
    }

    public override bool IsComplete()
    {
      return (double) this.flyingProgress >= (double) this.parent.FlyUpTime / (double) this.flyUpSpeedMultiplier;
    }
  }

  public class FlyAwayState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public float duration;
    public float progress;
    public Vector3 position;
    public bool completeAfter;
    public bool lookAtPlayer;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public FlyAwayState(Vector3 position, float duration, bool completeAfter, bool lookAtPlayer = false)
    {
      this.position = position;
      this.duration = duration;
      this.completeAfter = completeAfter;
      this.lookAtPlayer = lookAtPlayer;
    }

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      Health closestTarget1 = this.parent.GetClosestTarget();
      this.parent.LookAtPosition(!this.lookAtPlayer || !((UnityEngine.Object) closestTarget1 != (UnityEngine.Object) null) ? Vector3.zero : closestTarget1.transform.position);
      this.parent.headDirectionManager.ForceUpdate();
      this.parent.transform.DOMove(this.position, this.duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if (this.completeAfter)
          this.progress = this.duration;
        Health closestTarget2 = this.parent.GetClosestTarget();
        this.parent.LookAtPosition(!this.lookAtPlayer || !((UnityEngine.Object) closestTarget2 != (UnityEngine.Object) null) ? Vector3.zero : closestTarget2.transform.position);
      }));
    }

    public override void Update()
    {
      if (!this.lookAtPlayer)
        return;
      this.parent.LookAtTarget();
    }

    public override void OnExit()
    {
    }

    public override bool IsComplete() => (double) this.progress >= (double) this.duration;

    [CompilerGenerated]
    public void \u003COnEnter\u003Eb__9_0()
    {
      if (this.completeAfter)
        this.progress = this.duration;
      Health closestTarget = this.parent.GetClosestTarget();
      this.parent.LookAtPosition(!this.lookAtPlayer || !((UnityEngine.Object) closestTarget != (UnityEngine.Object) null) ? Vector3.zero : closestTarget.transform.position);
    }
  }

  public class FlyDownState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public Vector3 startPos;
    public Vector3 targetPos;
    public float progress;
    public float duration;
    public string landingSFX;
    public bool disableInvincible;
    public EnemyWolfGuardian.WarriorTrioSimpleState nextState;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public FlyDownState(
      EnemyWolfGuardian.WarriorTrioSimpleState nextState = null,
      string landingSFX = "",
      bool disableInvincible = true)
    {
      this.nextState = nextState;
      this.landingSFX = landingSFX;
      this.disableInvincible = disableInvincible;
    }

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.parent.OnDamageTriggerEnter);
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.FlyingStopAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
      this.startPos = this.parent.transform.position;
      this.targetPos = new Vector3(this.startPos.x, this.startPos.y, 0.0f);
      if (this.disableInvincible)
        this.parent.health.invincible = false;
      this.duration = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.FlyingStopAnimation).Duration;
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      this.parent.transform.position = this.parent.CalculateFlyProgress(this.progress, this.duration, this.startPos, this.targetPos, this.parent.EaseOutBackCurve);
      if (this.nextState == null || (double) this.progress < (double) this.duration)
        return;
      if (!string.IsNullOrEmpty(this.landingSFX))
        AudioManager.Instance.PlayOneShot(this.landingSFX, this.parent.transform.position);
      this.parent.logicStateMachine.SetState((SimpleState) this.nextState);
    }

    public override void OnExit()
    {
      this.parent.SetStatusImmunity(false);
      this.parent.LockToGround = true;
    }

    public override bool IsComplete() => (double) this.progress >= (double) this.duration;
  }

  public class InActiveMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public float flyUpProgress;
    public float flyDownProgress;
    public int stateIndex;
    public Vector3 position;
    public float duration;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Idle;

    public InActiveMasterState(Vector3 position, float duration)
    {
      this.position = position;
      this.duration = duration;
    }

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.childStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
      this.childStateMachine.SetParent(this.parent);
      this.SetupStateSequence();
      this.childStateMachine.SetState(this.stateSequence[this.stateIndex]);
      this.parent.LookAtPosition(this.position);
      if (string.IsNullOrEmpty(this.parent.BossSwoopOutSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.BossSwoopOutSFX, this.parent.transform.position);
    }

    public override void Update()
    {
      if (this.IsCurrentStateComplete())
        this.ProgressToNextSubState();
      this.childStateMachine.Update();
    }

    public override void OnExit()
    {
    }

    public void SetupStateSequence()
    {
      this.stateSequence = new SimpleState[2]
      {
        (SimpleState) new EnemyWolfGuardian.FlyUpState(2f),
        (SimpleState) new EnemyWolfGuardian.FlyAwayState(this.position, this.duration, false)
      };
    }

    public bool IsCurrentStateComplete() => this.childStateMachine.GetCurrentState().IsComplete();

    public void ProgressToNextSubState()
    {
      ++this.stateIndex;
      if (this.stateIndex < this.stateSequence.Length)
        this.childStateMachine.SetState(this.stateSequence[this.stateIndex]);
      else
        this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
    }
  }

  public class ActiveMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public float flyUpProgress;
    public float flyDownProgress;
    public int stateIndex;
    public Vector3 position;
    public float duration;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.childStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
      this.childStateMachine.SetParent(this.parent);
      this.SetupStateSequence();
      this.childStateMachine.SetState(this.stateSequence[this.stateIndex]);
      this.parent.LookAtPosition(this.position);
      if (string.IsNullOrEmpty(this.parent.AttackLongVO))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.AttackLongVO, this.parent.gameObject);
    }

    public override void Update()
    {
      if (this.IsCurrentStateComplete())
        this.ProgressToNextSubState();
      this.childStateMachine.Update();
    }

    public override void OnExit()
    {
    }

    public void SetupStateSequence()
    {
      this.stateSequence = new SimpleState[3]
      {
        (SimpleState) new EnemyWolfGuardian.FlyAwayState(this.parent.transform.position + -this.parent.transform.position.normalized, 1f, true),
        (SimpleState) new EnemyWolfGuardian.FlyDownState(),
        (SimpleState) new EnemyWolfGuardian.IdleState()
      };
    }

    public bool IsCurrentStateComplete() => this.childStateMachine.GetCurrentState().IsComplete();

    public void ProgressToNextSubState()
    {
      ++this.stateIndex;
      if (this.stateIndex < this.stateSequence.Length)
        this.childStateMachine.SetState(this.stateSequence[this.stateIndex]);
      else
        this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
    }
  }

  public class FalseDeathState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Idle;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.ClearPaths();
      this.parent.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      this.parent.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.parent.OnDamageTriggerEnter);
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.FalseDeathStartAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.FalseDeathAnimation, true, 0.0f);
      this.parent.health.invincible = true;
      this.parent.isImmuneToKnockback = true;
      this.parent.DashParticles.Stop();
      this.parent.health.ClearAllStasisEffects();
      if (string.IsNullOrEmpty(this.parent.IncapacitateSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.IncapacitateSFX, this.parent.transform.position);
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
      this.parent.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.parent.OnDamageTriggerEnter);
    }
  }

  public class FalseDeathEndState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.FalseDeathEndAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
      this.parent.ClearPaths();
    }

    public override void Update()
    {
    }

    public override void OnExit() => this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public class TauntState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.TauntAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
      this.parent.ClearPaths();
    }

    public override void Update()
    {
    }

    public override void OnExit()
    {
    }
  }

  public class WaitMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public float flyUpProgress;
    public float flyDownProgress;
    public int stateIndex;
    public Vector3 position;
    public Vector3 endPos;
    public float duration;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public WaitMasterState(Vector3 endPos) => this.endPos = endPos;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.childStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
      this.childStateMachine.SetParent(this.parent);
      this.SetupStateSequence();
      this.childStateMachine.SetState(this.stateSequence[this.stateIndex]);
      this.parent.LookAtPosition(this.position);
    }

    public override void Update()
    {
      if (this.IsCurrentStateComplete())
        this.ProgressToNextSubState();
      this.childStateMachine.Update();
    }

    public override void OnExit()
    {
    }

    public void SetupStateSequence()
    {
      this.stateSequence = new SimpleState[3]
      {
        (SimpleState) new EnemyWolfGuardian.FlyAwayState(this.endPos, 1f, true),
        (SimpleState) new EnemyWolfGuardian.FlyDownState(disableInvincible: false),
        (SimpleState) new EnemyWolfGuardian.WaitState()
      };
    }

    public bool IsCurrentStateComplete() => this.childStateMachine.GetCurrentState().IsComplete();

    public void ProgressToNextSubState()
    {
      ++this.stateIndex;
      if (this.stateIndex >= this.stateSequence.Length)
        return;
      this.childStateMachine.SetState(this.stateSequence[this.stateIndex]);
    }
  }

  public class WaitState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Idle;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      this.parent.ClearPaths();
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      this.parent.headDirectionManager.ForceUpdate();
    }

    public override void OnExit()
    {
    }
  }

  public class GoToRandomPositionState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public Vector3 randPosition;
    public float stopDistance = 0.5f;
    public float movementRange = 3f;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Movement;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.ClearPaths();
      Vector3 closestPoint;
      BiomeGenerator.PointWithinIsland(new Vector3(UnityEngine.Random.Range(-this.movementRange, this.movementRange), UnityEngine.Random.Range(-this.movementRange, this.movementRange), 0.0f), out closestPoint);
      this.randPosition = closestPoint;
      this.parent.givePath(this.randPosition);
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.WalkAnimation, true);
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      if ((double) Vector3.Distance(this.parent.transform.position, this.randPosition) >= (double) this.stopDistance && !this.PathIsFinished())
        return;
      this.parent.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
    }

    public bool PathIsFinished()
    {
      return this.parent.pathToFollow == null || this.parent.currentWaypoint > this.parent.pathToFollow.Count || (double) this.parent.speed <= 0.0;
    }

    public override void OnExit()
    {
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      this.parent.ClearPaths();
    }
  }

  public class IntroState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public Vector3 startPos;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Idle;

    public IntroState(Vector3 startPos) => this.startPos = startPos;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.transform.position = this.startPos;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.FlyingLoopAnimation, true);
      this.parent.ForceHeadLookAtTarget(Vector3.zero);
    }

    public override void Update()
    {
      this.parent.ReconsiderTarget();
      if (!((UnityEngine.Object) this.parent.TargetObject != (UnityEngine.Object) null))
        return;
      this.parent.LookAtTarget();
    }

    public override void OnExit()
    {
    }
  }
}
