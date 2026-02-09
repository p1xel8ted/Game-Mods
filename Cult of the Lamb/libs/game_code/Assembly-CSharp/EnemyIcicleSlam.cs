// Decompiled with JetBrains decompiler
// Type: EnemyIcicleSlam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
public class EnemyIcicleSlam : UnitObject
{
  public static List<EnemyIcicleSlam> lightningGuys = new List<EnemyIcicleSlam>();
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
  public string MeleeRecoveryAnimation;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public Transform TargetReticule;
  public ParticleSystem AOEParticles;
  public float AttackDistance = 1f;
  public float AttackCooldown = 3f;
  public float MeleeRecoveryDuration = 2f;
  public AssetReferenceGameObject IciclePrefab;
  public float IcicleFallSpeed = 15f;
  public int IcicleCount = 10;
  public float IcicleSpacing = 0.6f;
  public float IcicleInitialDistance = 1f;
  public float IcicleIncrementalDelay = 0.5f;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string LightningStrikeSFX = "event:/enemy/shoot_magicenergy";
  public bool DisableKnockback;
  public float KnockbackModifier = 1.5f;
  public float ChasePlayerTimeMin = 3f;
  public float ChasePlayerTimeMax = 5f;
  public DeadBodySliding deadBodySliding;
  public AsyncOperationHandle<GameObject> loadedIcicleAsset;
  public GameObject lastAttacker;
  public Health EnemyHealth;
  public GameObject TargetObject;
  public EnemyIcicleSlam.IcicleSlamStateMachine logicStateMachine;
  public Vector3 Force;
  public bool ShownWarning;
  public bool PathingToPlayer;
  public bool isKnockedTowardsEnemy;
  public bool isDying;
  public bool isReadyToDie;
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
  public float lockTime = 1f;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
    this.SetupStateMachine();
    this.lastAttackTimestamp -= this.AttackCooldown;
    if (!((UnityEngine.Object) this.deadBodySliding != (UnityEngine.Object) null))
      return;
    ObjectPool.CreatePool<DeadBodySliding>(this.deadBodySliding, ObjectPool.CountPooled<DeadBodySliding>(this.deadBodySliding) + 1, true);
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    this.LoadAssets();
  }

  public void LoadAssets()
  {
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.IciclePrefab);
    asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedIcicleAsset = obj;
      obj.Result.CreatePool(this.IcicleCount, true);
    });
    asyncOperationHandle.WaitForCompletion();
  }

  public void SetupStateMachine()
  {
    this.logicStateMachine = new EnemyIcicleSlam.IcicleSlamStateMachine();
    this.logicStateMachine.SetParent(this);
    this.logicStateMachine.SetState((SimpleState) new EnemyIcicleSlam.IdleState());
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyIcicleSlam.lightningGuys.Add(this);
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
    EnemyIcicleSlam.lightningGuys.Remove(this);
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
      this.StartCoroutine((IEnumerator) this.HurtRoutine(!(this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemyIcicleSlam.IcicleAttackState))));
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && AttackType != Health.AttackTypes.NoReaction && !this.DisableKnockback)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
    this.lastAttacker = Attacker;
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
    EnemyIcicleSlam enemyIcicleSlam = this;
    enemyIcicleSlam.DisableForces = true;
    enemyIcicleSlam.Angle = Utils.GetAngle(Attacker.transform.position, enemyIcicleSlam.transform.position) * ((float) Math.PI / 180f);
    enemyIcicleSlam.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyIcicleSlam.Angle), 1500f * Mathf.Sin(enemyIcicleSlam.Angle)) * enemyIcicleSlam.KnockbackModifier);
    enemyIcicleSlam.rb.AddForce((Vector2) enemyIcicleSlam.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyIcicleSlam.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyIcicleSlam.DisableForces = false;
  }

  public IEnumerator HurtRoutine(bool playAnimation)
  {
    EnemyIcicleSlam enemyIcicleSlam = this;
    enemyIcicleSlam.damageColliderEvents.SetActive(false);
    enemyIcicleSlam.ClearPaths();
    enemyIcicleSlam.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyIcicleSlam.state.CURRENT_STATE = StateMachine.State.Idle;
    if (playAnimation)
      enemyIcicleSlam.Spine.AnimationState.SetAnimation(0, enemyIcicleSlam.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyIcicleSlam.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyIcicleSlam.DisableForces = false;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public void DoIcicleAttack()
  {
    float f = this.state.facingAngle * ((float) Math.PI / 180f);
    Vector3 normalized = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f).normalized;
    Vector3 vector3 = normalized * this.IcicleInitialDistance;
    for (int index = 0; index < this.IcicleCount; ++index)
      ObjectPool.Spawn(this.loadedIcicleAsset.Result, this.transform.parent, this.transform.position + vector3 + normalized * this.IcicleSpacing * (float) (index + 1), Quaternion.identity).GetComponent<IcicleControl>()?.Drop(this.IcicleIncrementalDelay * (float) (index + 1), this.IcicleFallSpeed);
  }

  public void PlayTargetReticule()
  {
    float f = this.state.facingAngle * ((float) Math.PI / 180f);
    this.TargetReticule.position = this.transform.position + new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f).normalized;
    this.TargetReticule.gameObject.SetActive(true);
    this.TargetReticule.localScale = Vector3.zero;
    Quaternion rotation = this.TargetReticule.rotation;
    this.TargetReticule.Rotate(0.0f, 0.0f, -180f);
    float delay = this.lockTime / 6f;
    this.TargetReticule.DOScale(2.5f, delay * 2f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(delay);
    this.TargetReticule.DORotateQuaternion(rotation, delay * 2f).SetDelay<TweenerCore<Quaternion, Quaternion, NoOptions>>(delay);
  }

  public void HideTargetReticle()
  {
    this.TargetReticule.DOKill();
    this.TargetReticule.DOScale(0.0f, this.lockTime / 4f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(this.EndTargetReticleAnimation));
  }

  public void EndTargetReticleAnimation() => this.TargetReticule.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__56_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedIcicleAsset = obj;
    obj.Result.CreatePool(this.IcicleCount, true);
  }

  public class IcicleSlamStateMachine : SimpleStateMachine
  {
    [CompilerGenerated]
    public EnemyIcicleSlam \u003CParent\u003Ek__BackingField;

    public EnemyIcicleSlam Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set => this.\u003CParent\u003Ek__BackingField = value;
    }

    public void SetParent(EnemyIcicleSlam parent) => this.Parent = parent;
  }

  public class IdleState : SimpleState
  {
    public EnemyIcicleSlam parent;

    public override void OnEnter()
    {
      this.parent = ((EnemyIcicleSlam.IcicleSlamStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      if ((double) PlayerFarming.GetClosestPlayerDist(this.parent.transform.position) >= (double) this.parent.VisionRange)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyIcicleSlam.ChasePlayerState());
    }

    public override void OnExit()
    {
    }
  }

  public class ChasePlayerState : SimpleState
  {
    public EnemyIcicleSlam parent;
    public float progress;
    public float repathTimer;
    public float attackBuffer = 0.5f;

    public override void OnEnter()
    {
      this.parent = ((EnemyIcicleSlam.IcicleSlamStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Moving;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.ElectrifiedMovingAnimation, true);
      this.parent.maxSpeed = this.parent.MaxChaseSpeed;
      this.parent.TargetObject = this.GetNewTarget();
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
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
      this.parentStateMachine.SetState((SimpleState) new EnemyIcicleSlam.IcicleAttackState());
    }

    public override void OnExit()
    {
    }

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

  public class IcicleAttackState : SimpleState
  {
    public EnemyIcicleSlam parent;
    public float progress;
    public float attackAnimDuration;
    public float colliderEnableTime = 0.5f;

    public override void OnEnter()
    {
      this.parent = ((EnemyIcicleSlam.IcicleSlamStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
      this.parent.LookAtTarget();
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.AttackAnimation, false);
      this.attackAnimDuration = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.AttackAnimation).Duration;
      this.parent.PlayTargetReticule();
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress >= (double) this.colliderEnableTime && !this.parent.damageColliderEvents.isActiveAndEnabled)
        this.parent.damageColliderEvents.SetActive(true);
      if ((double) this.progress >= (double) this.attackAnimDuration)
      {
        this.parent.damageColliderEvents.SetActive(false);
        CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.5f);
        this.parent.DoIcicleAttack();
        this.parent.HideTargetReticle();
        this.parentStateMachine.SetState((SimpleState) new EnemyIcicleSlam.MeleeRecoveryState());
      }
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(this.progress / this.attackAnimDuration);
    }

    public override void OnExit()
    {
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.parent.lastAttackTimestamp = GameManager.GetInstance().CurrentTime;
    }
  }

  public class MeleeRecoveryState : SimpleState
  {
    public EnemyIcicleSlam parent;
    public float progress;

    public override void OnEnter()
    {
      this.parent = ((EnemyIcicleSlam.IcicleSlamStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.MeleeRecoveryAnimation, true);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress < (double) this.parent.MeleeRecoveryDuration)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyIcicleSlam.ChasePlayerState());
    }

    public override void OnExit()
    {
    }
  }
}
