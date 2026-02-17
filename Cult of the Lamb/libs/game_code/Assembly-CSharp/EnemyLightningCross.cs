// Decompiled with JetBrains decompiler
// Type: EnemyLightningCross
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
public class EnemyLightningCross : UnitObject
{
  public static List<EnemyLightningCross> lightningGuys = new List<EnemyLightningCross>();
  public float MaxChaseSpeed = 0.1f;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ElectrifiedMovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LightningSignpostAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DeathSignpostAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MeleeRecoveryAnimation;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public float AttackDistance = 1f;
  public float AttackCooldown = 3f;
  public float MeleeRecoveryDuration = 1.66f;
  public float TriggerDistance = 5f;
  public float LightningEnemyDamage = 4f;
  public float LightningExpansionSpeed = 0.8f;
  public float HealCooldown = 5f;
  public AssetReferenceGameObject LightningAttackPrefab;
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/warning";
  [EventRef]
  public string LightningSingleStartSFX = "event:/dlc/dungeon05/enemy/lightning_cross/attack_lightning_single_start";
  [EventRef]
  public string LightningRingStartSFX = "event:/dlc/dungeon05/enemy/lightning_cross/attack_lightning_ring_start";
  [EventRef]
  public string LightningRingReturnSFX = "event:/dlc/dungeon05/enemy/lightning_cross/attack_lightning_ring_return";
  [EventRef]
  public string ChaseStartSFX = "event:/dlc/dungeon05/enemy/lightning_cross/mv_chase_start";
  [EventRef]
  public string ChaseLoopSFX = "event:/dlc/dungeon05/enemy/lightning_cross/mv_chase_electricity_loop";
  public string attackHealStartSFX = "event:/dlc/dungeon05/enemy/lightning_cross/attack_heal_start";
  public bool DisableKnockback;
  public float KnockbackModifier = 1.5f;
  public DeadBodySliding deadBodySliding;
  public Health EnemyHealth;
  public GameObject TargetObject;
  public EnemyLightningCross.LightningGuyStateMachine logicStateMachine;
  public EventInstance chaseLoopInstance;
  public Vector3 Force;
  public bool ShownWarning;
  public bool PathingToPlayer;
  public bool isKnockedTowardsEnemy;
  public float knockbackStrength = 3f;
  public float RandomDirection;
  public float HidingHeight = 5f;
  public float lightningHitGroundDelay = 0.5f;
  public float lightningNextStrikeTimer;
  public float Angle;
  public float chaseDelay = 0.3f;
  public float followPlayerRepathTime = 0.2f;
  public float knockbackGravity = 6f;
  public float tooCloseToAllyDistance = 0.4f;
  public float lastAttackTimestamp;
  public float lastHealTimestamp;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;
  public GameObject loadedLightningImpact;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public GameObject lastLightningRingExplosion;

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
    this.SetupStateMachine();
    if ((UnityEngine.Object) this.deadBodySliding != (UnityEngine.Object) null)
      ObjectPool.CreatePool<DeadBodySliding>(this.deadBodySliding, ObjectPool.CountPooled<DeadBodySliding>(this.deadBodySliding) + 1, true);
    this.LoadAssets();
  }

  public void SetupStateMachine()
  {
    this.logicStateMachine = new EnemyLightningCross.LightningGuyStateMachine();
    this.logicStateMachine.SetParent(this);
    this.logicStateMachine.SetState((SimpleState) new EnemyLightningCross.IdleState());
  }

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.LightningAttackPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyLightningCross.loadedAddressableAssets.Add(obj);
      this.loadedLightningImpact = obj.Result;
      this.loadedLightningImpact.CreatePool(1, true);
    });
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyLightningCross.lightningGuys.Add(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.health.invincible = false;
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
  }

  public new virtual void Update()
  {
    base.Update();
    this.logicStateMachine.Update();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyLightningCross.lightningGuys.Remove(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    AudioManager.Instance.StopLoop(this.chaseLoopInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void SpineEventStaffUp()
  {
    if (string.IsNullOrEmpty(this.LightningRingReturnSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.LightningRingReturnSFX);
  }

  public void DoLightningExplosion(Vector3 position)
  {
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    this.lastLightningRingExplosion = LightningRingExplosion.CreateExplosion(position, this.health.team, this.health, this.LightningExpansionSpeed, 1f, this.LightningEnemyDamage);
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
    if (!this.DisableKnockback)
      this.damageColliderEvents.SetActive(false);
    if (AttackType != Health.AttackTypes.NoReaction)
    {
      this.DisableForces = false;
      this.StartCoroutine((IEnumerator) this.HurtRoutine(!(this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemyLightningCross.LightningRingAttackState))));
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && AttackType != Health.AttackTypes.NoReaction && !this.DisableKnockback)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
    if (!(this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemyLightningCross.IdleState)))
      return;
    this.AggroAllLightningGuys();
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
    AudioManager.Instance.StopLoop(this.chaseLoopInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
    if (!((UnityEngine.Object) this.lastLightningRingExplosion != (UnityEngine.Object) null))
      return;
    LightningRingExplosion component = this.lastLightningRingExplosion.GetComponent<LightningRingExplosion>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.forceEnd = true;
    this.lastLightningRingExplosion = (GameObject) null;
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyLightningCross enemyLightningCross = this;
    enemyLightningCross.DisableForces = true;
    enemyLightningCross.Angle = Utils.GetAngle(Attacker.transform.position, enemyLightningCross.transform.position) * ((float) Math.PI / 180f);
    enemyLightningCross.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyLightningCross.Angle), 1500f * Mathf.Sin(enemyLightningCross.Angle)) * enemyLightningCross.KnockbackModifier);
    enemyLightningCross.rb.AddForce((Vector2) enemyLightningCross.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLightningCross.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLightningCross.DisableForces = false;
  }

  public IEnumerator ApplyForceRoutine(float angle)
  {
    EnemyLightningCross enemyLightningCross = this;
    enemyLightningCross.Angle = angle;
    enemyLightningCross.DisableForces = true;
    enemyLightningCross.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyLightningCross.Angle), 1500f * Mathf.Sin(enemyLightningCross.Angle)) * enemyLightningCross.KnockbackModifier);
    enemyLightningCross.rb.AddForce((Vector2) enemyLightningCross.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLightningCross.Spine.timeScale) < 0.5)
    {
      if ((double) enemyLightningCross.transform.position.z < 0.0)
        enemyLightningCross.transform.position += Vector3.forward * enemyLightningCross.knockbackGravity * Time.deltaTime;
      yield return (object) null;
    }
    enemyLightningCross.transform.position = new Vector3(enemyLightningCross.transform.position.x, enemyLightningCross.transform.position.y, 0.0f);
    enemyLightningCross.DisableForces = false;
  }

  public IEnumerator HurtRoutine(bool playAnimation)
  {
    EnemyLightningCross enemyLightningCross = this;
    enemyLightningCross.damageColliderEvents.SetActive(false);
    enemyLightningCross.ClearPaths();
    enemyLightningCross.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyLightningCross.state.CURRENT_STATE = StateMachine.State.Idle;
    if (playAnimation)
      enemyLightningCross.Spine.AnimationState.SetAnimation(0, enemyLightningCross.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLightningCross.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLightningCross.DisableForces = false;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public void AggroAllLightningGuys()
  {
    foreach (EnemyLightningCross lightningGuy in EnemyLightningCross.lightningGuys)
    {
      if (lightningGuy.logicStateMachine.GetCurrentState().GetType() == typeof (EnemyLightningCross.IdleState) & (double) lightningGuy.health.HP > 0.0)
        lightningGuy.logicStateMachine.SetState((SimpleState) new EnemyLightningCross.HitByLightingState());
    }
  }

  public Health FindTargetToHeal()
  {
    Health targetToHeal = (Health) null;
    float num = float.MaxValue;
    Health.Team team = this.health.team;
    foreach (Health allUnit in Health.allUnits)
    {
      if ((UnityEngine.Object) allUnit != (UnityEngine.Object) null && allUnit.team == team && (double) allUnit.HP < (double) allUnit.totalHP && (double) allUnit.HP < (double) num && (UnityEngine.Object) allUnit != (UnityEngine.Object) this.health)
      {
        targetToHeal = allUnit;
        num = allUnit.HP;
      }
    }
    return targetToHeal;
  }

  public override void OnDestroy()
  {
    if (EnemyLightningCross.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in EnemyLightningCross.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      EnemyLightningCross.loadedAddressableAssets.Clear();
    }
    base.OnDestroy();
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__57_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemyLightningCross.loadedAddressableAssets.Add(obj);
    this.loadedLightningImpact = obj.Result;
    this.loadedLightningImpact.CreatePool(1, true);
  }

  public class LightningGuyStateMachine : SimpleStateMachine
  {
    [CompilerGenerated]
    public EnemyLightningCross \u003CParent\u003Ek__BackingField;

    public EnemyLightningCross Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set => this.\u003CParent\u003Ek__BackingField = value;
    }

    public void SetParent(EnemyLightningCross parent) => this.Parent = parent;
  }

  public class IdleState : SimpleState
  {
    public EnemyLightningCross parent;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningCross.LightningGuyStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      Health closestTarget = this.parent.GetClosestTarget();
      if (!((UnityEngine.Object) closestTarget != (UnityEngine.Object) null) || (double) Vector3.Distance(this.parent.transform.position, closestTarget.transform.position) >= (double) this.parent.TriggerDistance)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.HitByLightingState());
      this.parent.AggroAllLightningGuys();
    }

    public override void OnExit()
    {
    }
  }

  public class HitByLightingState : SimpleState
  {
    public EnemyLightningCross parent;
    public RelicData lightningData;
    public bool hasExploded;
    public float lightningTimer;
    public float explosionTriggerTime = 0.5f;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningCross.LightningGuyStateMachine) this.parentStateMachine).Parent;
      this.lightningData = EquipmentManager.GetRelicData(RelicType.LightningStrike);
      this.lightningData.VFXData.ImpactVFXObject.SpawnVFX(this.parent.transform, true);
      this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.LightningSignpostAnimation, true);
      AudioManager.Instance.PlayOneShot(this.parent.WarningVO, this.parent.transform.position);
      AudioManager.Instance.PlayOneShot(this.parent.LightningSingleStartSFX, this.parent.transform.position);
    }

    public override void Update()
    {
      this.lightningTimer += Time.deltaTime;
      if ((double) this.lightningTimer <= (double) this.explosionTriggerTime || this.hasExploded)
        return;
      this.hasExploded = true;
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      this.parent.lastLightningRingExplosion = Explosion.CreateExplosion(this.parent.transform.position, this.parent.health.team, this.parent.health, 1f);
      this.parent.lastHealTimestamp = GameManager.GetInstance().CurrentTime;
      this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.ChaseTargetState());
    }

    public override void OnExit()
    {
    }
  }

  public class ChaseTargetState : SimpleState
  {
    public EnemyLightningCross parent;
    public float progress;
    public float repathTimer;
    public float attackBuffer = 0.5f;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningCross.LightningGuyStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Moving;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.LightningSignpostAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.ElectrifiedMovingAnimation, true, 0.0f);
      this.parent.maxSpeed = this.parent.MaxChaseSpeed;
      this.parent.TargetObject = this.GetNewTarget();
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      if (!string.IsNullOrEmpty(this.parent.ChaseLoopSFX))
        this.parent.chaseLoopInstance = AudioManager.Instance.CreateLoop(this.parent.ChaseLoopSFX, this.parent.gameObject, true);
      if (string.IsNullOrEmpty(this.parent.ChaseStartSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.ChaseStartSFX);
    }

    public override void Update()
    {
      if (this.IsTargetValid())
        this.parent.TargetObject = this.GetNewTarget();
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((UnityEngine.Object) this.parent.TargetObject != (UnityEngine.Object) null)
      {
        this.parent.LookAtTarget();
        if ((double) (this.repathTimer += Time.deltaTime) > (double) this.parent.followPlayerRepathTime)
        {
          this.repathTimer = 0.0f;
          this.parent.givePath(this.parent.TargetObject.transform.position);
        }
      }
      if (!this.IsAttackValid(this.progress))
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.LightningRingAttackState());
    }

    public override void OnExit() => AudioManager.Instance.StopLoop(this.parent.chaseLoopInstance);

    public bool IsAttackValid(float progress)
    {
      return (double) progress >= (double) this.attackBuffer && this.IsPlayerInRange() && this.IsMeleeCooldownOver();
    }

    public GameObject GetNewTarget() => this.parent.GetClosestTarget(true)?.gameObject;

    public bool IsPlayerInRange()
    {
      if ((UnityEngine.Object) this.parent.TargetObject != (UnityEngine.Object) null)
        return (double) Vector3.Distance(this.parent.transform.position, this.parent.TargetObject.transform.position) <= (double) this.parent.AttackDistance;
      this.parent.TargetObject = this.GetNewTarget();
      return false;
    }

    public bool IsMeleeCooldownOver()
    {
      float num = this.parent.lastAttackTimestamp + this.parent.AttackCooldown;
      return (double) GameManager.GetInstance().CurrentTime >= (double) num;
    }

    public bool IsTargetValid()
    {
      return (UnityEngine.Object) this.parent.TargetObject != (UnityEngine.Object) null && (double) this.parent.TargetObject.GetComponent<Health>().HP <= 0.0;
    }
  }

  public class LightningRingAttackState : SimpleState
  {
    public EnemyLightningCross parent;
    public float progress;
    public float attackAnimDuration;
    public float colliderEnableTime = 0.3f;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningCross.LightningGuyStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
      this.parent.LookAtTarget();
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.AttackAnimation, false);
      this.attackAnimDuration = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.AttackAnimation).Duration;
      if (!string.IsNullOrEmpty(this.parent.LightningRingStartSFX))
        AudioManager.Instance.PlayOneShot(this.parent.LightningRingStartSFX);
      if (string.IsNullOrEmpty(this.parent.WarningVO))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.WarningVO, this.parent.gameObject);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress >= (double) this.colliderEnableTime && !this.parent.damageColliderEvents.isActiveAndEnabled)
        this.parent.damageColliderEvents.SetActive(true);
      if ((double) this.progress >= (double) this.attackAnimDuration)
      {
        this.parent.DoLightningExplosion(this.parent.transform.position);
        this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.LightningRingRecoveryState());
      }
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(this.progress / this.attackAnimDuration);
    }

    public override void OnExit()
    {
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.parent.damageColliderEvents.SetActive(false);
      this.parent.lastAttackTimestamp = GameManager.GetInstance().CurrentTime;
    }
  }

  public class LightningRingRecoveryState : SimpleState
  {
    public EnemyLightningCross parent;
    public float progress;
    public float recoveryDuration;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningCross.LightningGuyStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.MeleeRecoveryAnimation, true);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress < (double) this.parent.MeleeRecoveryDuration)
        return;
      if (!string.IsNullOrEmpty(this.parent.LightningRingReturnSFX))
        AudioManager.Instance.PlayOneShot(this.parent.LightningRingReturnSFX);
      if ((double) GameManager.GetInstance().CurrentTime > (double) this.parent.lastHealTimestamp + (double) this.parent.HealCooldown && (UnityEngine.Object) this.parent.FindTargetToHeal() != (UnityEngine.Object) null && this.parent.health.team != Health.Team.PlayerTeam)
        this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.HealAllySignpostState());
      else
        this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.ChaseTargetState());
    }

    public override void OnExit()
    {
    }
  }

  public class HealAllySignpostState : SimpleState
  {
    public EnemyLightningCross parent;
    public Health healTarget;
    public float progress;
    public float signpostAnimLength;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningCross.LightningGuyStateMachine) this.parentStateMachine).Parent;
      this.healTarget = this.parent.FindTargetToHeal();
      if ((UnityEngine.Object) this.healTarget == (UnityEngine.Object) null)
        this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.ChaseTargetState());
      this.parent.ClearPaths();
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.LightningSignpostAnimation, true);
      this.signpostAnimLength = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.LightningSignpostAnimation).Duration;
      AudioManager.Instance.PlayOneShot(this.parent.attackHealStartSFX, this.parent.transform.position);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress < (double) this.signpostAnimLength)
        return;
      if ((UnityEngine.Object) this.parent.FindTargetToHeal() == (UnityEngine.Object) null || this.parent.health.team == Health.Team.PlayerTeam)
        this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.LightningRingRecoveryState());
      else
        this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.HealAllyActionState());
    }

    public override void OnExit()
    {
    }
  }

  public class HealAllyActionState : SimpleState
  {
    public EnemyLightningCross parent;
    public Health healTarget;
    public float progress;
    public float beamDuration = 0.5f;
    public float beamSignpost = 0.5f;
    public float stateDuration = 0.5f;

    public override void OnEnter()
    {
      this.parent = ((EnemyLightningCross.LightningGuyStateMachine) this.parentStateMachine).Parent;
      this.healTarget = this.parent.FindTargetToHeal();
      this.parent.ClearPaths();
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.MeleeRecoveryAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
      this.stateDuration = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.MeleeRecoveryAnimation).Duration;
      this.DoHealLogic();
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress < (double) this.stateDuration)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyLightningCross.ChaseTargetState());
    }

    public void DoHealLogic()
    {
      if (!((UnityEngine.Object) this.healTarget != (UnityEngine.Object) null) && this.parent.health.team == Health.Team.PlayerTeam)
        return;
      SoulCustomTarget.Create(this.healTarget.gameObject, this.parent.transform.position, Color.white, (System.Action) (() =>
      {
        if (!((UnityEngine.Object) this.healTarget != (UnityEngine.Object) null))
          return;
        if ((double) this.healTarget.HP > 0.0)
        {
          this.healTarget.HP = this.healTarget.totalHP;
          this.healTarget.GetComponent<ShowHPBar>()?.OnHit(this.healTarget.gameObject, Vector3.zero, Health.AttackTypes.Melee, false);
        }
        AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.healTarget.gameObject.transform.position);
        BiomeConstants.Instance.EmitHeartPickUpVFX(this.healTarget.transform.position - Vector3.forward, 0.0f, "red", "burst_big");
      }), 0.75f);
    }

    public override void OnExit()
    {
      this.parent.lastHealTimestamp = GameManager.GetInstance().CurrentTime;
    }

    [CompilerGenerated]
    public void \u003CDoHealLogic\u003Eb__8_0()
    {
      if (!((UnityEngine.Object) this.healTarget != (UnityEngine.Object) null))
        return;
      if ((double) this.healTarget.HP > 0.0)
      {
        this.healTarget.HP = this.healTarget.totalHP;
        this.healTarget.GetComponent<ShowHPBar>()?.OnHit(this.healTarget.gameObject, Vector3.zero, Health.AttackTypes.Melee, false);
      }
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.healTarget.gameObject.transform.position);
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.healTarget.transform.position - Vector3.forward, 0.0f, "red", "burst_big");
    }
  }
}
