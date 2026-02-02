// Decompiled with JetBrains decompiler
// Type: EnemyQuadLightning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
public class EnemyQuadLightning : UnitObject
{
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation2;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LightningSignpostAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LightningSignpostAnimation2;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string CircleAttackAnimation;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public Transform[] directionalArrows;
  public float fleeDistance = 3f;
  public Vector2 QuadCooldownRange = new Vector2(2f, 4f);
  public float CircleCooldown = 3f;
  public float AttackRecoveryDuration = 2f;
  public float AattackSignpostDuration = 0.5f;
  public float delayBetweenRunesAndLightning = 1.2f;
  public float delayBetweenRunesAndStrike = 0.5f;
  public float beamDuration = 0.5f;
  public float beamSignpost = 0.5f;
  public float beamWidth = 0.7f;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string LightningSFX = "event:/explosion/explosion";
  [EventRef]
  public string AttackSFX = "event:/enemy/shoot_magicenergy";
  [EventRef]
  public string CloseAttackSFX = "event:/dlc/dungeon05/enemy/lightning_quad/attack_reticle_start";
  [EventRef]
  public string RunesStraightSFX = "event:/dlc/dungeon05/enemy/lightning_quad/attack_runes_start";
  [EventRef]
  public string RunesDiagonalSFX = "event:/dlc/dungeon05/enemy/lightning_quad/attack_runes_start_diagonal";
  public bool DisableKnockback;
  public float KnockbackModifier = 1.5f;
  public DeadBodySliding deadBodySliding;
  public ParticleSystem runes;
  public AssetReferenceGameObject LightningAttackPrefab;
  public Health EnemyHealth;
  public GameObject TargetObject;
  public EnemyQuadLightning.QuadLightningStateMachine logicStateMachine;
  public Vector3 Force;
  public Vector3 offset;
  public bool ShownWarning;
  public bool PathingToPlayer;
  public bool isKnockedTowardsEnemy;
  public bool inRange;
  public bool canLoseRange;
  public float currentQuadAttackCooldown = 3f;
  public float knockbackStrength = 3f;
  public float RandomDirection;
  public float HidingHeight = 5f;
  public float lightningHitGroundDelay = 0.5f;
  public float lightningNextStrikeTimer;
  public float Angle;
  public float chaseDelay = 0.3f;
  public float knockbackGravity = 6f;
  public float lastQuadAttackTimeStamp;
  public float lastCircleAttackTimeStamp;
  public float checkPlayerTimestamp;
  public float checkPlayerInterval = 0.1f;
  public float directTargetDistance;
  public float targetTrackingOffset;
  public float lookBuffer = 0.3f;
  public float lastLookChange;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public float delayAttackCircle;
  public float distanceFromUnitLightningCircle = 1.4f;
  public float timeBetweenCircleStrikes = 0.2f;
  public int numLightningCircle = 6;
  public GameObject loadedLightningImpact;
  public EnemyQuadLightning.BeamPattern currentBeamPattern;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
    this.LoadAssets();
    this.SetupStateMachine();
    this.CreatePools();
    this.SetDirectionalArrowsActive(false);
    this.currentQuadAttackCooldown = UnityEngine.Random.Range(this.QuadCooldownRange.x, this.QuadCooldownRange.y);
  }

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.LightningAttackPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyQuadLightning.loadedAddressableAssets.Add(obj);
      this.loadedLightningImpact = obj.Result;
      this.loadedLightningImpact.CreatePool(16 /*0x10*/, true);
    });
  }

  public void CreatePools()
  {
    if (!((UnityEngine.Object) this.deadBodySliding != (UnityEngine.Object) null))
      return;
    ObjectPool.CreatePool<DeadBodySliding>(this.deadBodySliding, ObjectPool.CountPooled<DeadBodySliding>(this.deadBodySliding) + 1, true);
  }

  public void SetupStateMachine()
  {
    this.logicStateMachine = new EnemyQuadLightning.QuadLightningStateMachine();
    this.logicStateMachine.SetParent(this);
    this.logicStateMachine.SetState((SimpleState) new EnemyQuadLightning.IdleState());
  }

  public override void OnEnable()
  {
    base.OnEnable();
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

  public override void OnDestroy()
  {
    if (EnemyQuadLightning.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in EnemyQuadLightning.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      EnemyQuadLightning.loadedAddressableAssets.Clear();
    }
    base.OnDestroy();
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
      this.StartCoroutine((IEnumerator) this.HurtRoutine(!(this.logicStateMachine.GetCurrentState().GetType() == typeof (EnemyQuadLightning.AttackState))));
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && AttackType != Health.AttackTypes.NoReaction && !this.DisableKnockback)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.state.CURRENT_STATE = StateMachine.State.Dead;
    if (string.IsNullOrEmpty(this.DeathVO))
      return;
    AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null || (double) GameManager.GetInstance().CurrentTime < (double) this.lastLookChange + (double) this.lookBuffer)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.lastLookChange = Time.time;
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyQuadLightning enemyQuadLightning = this;
    enemyQuadLightning.DisableForces = true;
    enemyQuadLightning.Angle = Utils.GetAngle(Attacker.transform.position, enemyQuadLightning.transform.position) * ((float) Math.PI / 180f);
    enemyQuadLightning.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyQuadLightning.Angle), 1500f * Mathf.Sin(enemyQuadLightning.Angle)) * enemyQuadLightning.KnockbackModifier);
    enemyQuadLightning.rb.AddForce((Vector2) enemyQuadLightning.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyQuadLightning.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyQuadLightning.DisableForces = false;
  }

  public IEnumerator HurtRoutine(bool playAnimation)
  {
    EnemyQuadLightning enemyQuadLightning = this;
    enemyQuadLightning.damageColliderEvents.SetActive(false);
    enemyQuadLightning.ClearPaths();
    enemyQuadLightning.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyQuadLightning.state.CURRENT_STATE = StateMachine.State.Idle;
    if (playAnimation)
      enemyQuadLightning.Spine.AnimationState.SetAnimation(0, enemyQuadLightning.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyQuadLightning.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyQuadLightning.DisableForces = false;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public GameObject GetNewTarget() => this.GetClosestTarget(true)?.gameObject;

  public bool IsQuadCooldownOver()
  {
    if ((double) GameManager.GetInstance().CurrentTime < (double) this.lastQuadAttackTimeStamp + (double) this.currentQuadAttackCooldown)
      return false;
    this.currentQuadAttackCooldown = UnityEngine.Random.Range(this.QuadCooldownRange.x, this.QuadCooldownRange.y);
    if (this.IsEnemyWithinFleeDistance())
      this.currentQuadAttackCooldown *= 0.5f;
    return true;
  }

  public bool IsEnemyWithinFleeDistance()
  {
    Health closestTarget = this.GetClosestTarget();
    return (UnityEngine.Object) closestTarget != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, closestTarget.transform.position) < (double) this.fleeDistance;
  }

  public bool IsCircleCooldownOver()
  {
    return (double) GameManager.GetInstance().CurrentTime >= (double) this.lastCircleAttackTimeStamp + (double) this.CircleCooldown;
  }

  public IEnumerator CircleLightningAttack()
  {
    EnemyQuadLightning enemyQuadLightning = this;
    enemyQuadLightning.Spine.AnimationState.SetAnimation(0, enemyQuadLightning.CircleAttackAnimation, false);
    Vector3 dir = (enemyQuadLightning.GetClosestTarget().transform.position - enemyQuadLightning.transform.position).normalized;
    for (int i = 0; i < enemyQuadLightning.numLightningCircle; ++i)
    {
      float angle = 360f / (float) enemyQuadLightning.numLightningCircle * (float) i;
      Vector3 vector3 = enemyQuadLightning.transform.position + Quaternion.AngleAxis(angle, Vector3.forward) * (dir * enemyQuadLightning.distanceFromUnitLightningCircle);
      GameObject lightning1 = ObjectPool.Spawn(enemyQuadLightning.loadedLightningImpact, vector3, Quaternion.identity);
      if ((UnityEngine.Object) lightning1 != (UnityEngine.Object) null)
        lightning1.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(enemyQuadLightning.health, vector3, (System.Action) (() => ObjectPool.Recycle(lightning1)), true);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyQuadLightning.timeBetweenCircleStrikes, enemyQuadLightning.Spine);
    }
    enemyQuadLightning.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator LightningAttack(EnemyQuadLightning.BeamPattern beamPattern)
  {
    EnemyQuadLightning enemyQuadLightning = this;
    int beamThickness = 5;
    float beamLength = 10f;
    string soundPath = beamPattern == EnemyQuadLightning.BeamPattern.Straight ? enemyQuadLightning.RunesStraightSFX : enemyQuadLightning.RunesDiagonalSFX;
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath, enemyQuadLightning.transform.position);
    enemyQuadLightning.runes.Play();
    enemyQuadLightning.Spine.AnimationState.SetAnimation(0, enemyQuadLightning.AttackAnimation2, false);
    enemyQuadLightning.Spine.AnimationState.AddAnimation(0, enemyQuadLightning.IdleAnimation, true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyQuadLightning.delayBetweenRunesAndStrike, enemyQuadLightning.Spine);
    GameObject lightning = ObjectPool.Spawn(enemyQuadLightning.loadedLightningImpact, enemyQuadLightning.transform.position, Quaternion.identity);
    lightning.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(enemyQuadLightning.health, enemyQuadLightning.transform.position, (System.Action) (() => ObjectPool.Recycle(lightning)), false);
    Vector3 directionOne = beamPattern == EnemyQuadLightning.BeamPattern.Straight ? Vector3.up : new Vector3(1f, 1f, 0.0f);
    Vector3 directionTwo = beamPattern == EnemyQuadLightning.BeamPattern.Straight ? Vector3.down : new Vector3(1f, -1f, 0.0f);
    Vector3 directionThree = beamPattern == EnemyQuadLightning.BeamPattern.Straight ? Vector3.right : new Vector3(-1f, -1f, 0.0f);
    Vector3 directionFour = beamPattern == EnemyQuadLightning.BeamPattern.Straight ? Vector3.left : new Vector3(-1f, 1f, 0.0f);
    enemyQuadLightning.SetDirectionalArrowsActive(true);
    enemyQuadLightning.RotateDirectionalArrows(enemyQuadLightning.directionalArrows[0], directionOne);
    enemyQuadLightning.RotateDirectionalArrows(enemyQuadLightning.directionalArrows[1], directionTwo);
    enemyQuadLightning.RotateDirectionalArrows(enemyQuadLightning.directionalArrows[2], directionThree);
    enemyQuadLightning.RotateDirectionalArrows(enemyQuadLightning.directionalArrows[3], directionFour);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyQuadLightning.delayBetweenRunesAndLightning - enemyQuadLightning.delayBetweenRunesAndStrike, enemyQuadLightning.Spine);
    enemyQuadLightning.SetDirectionalArrowsActive(false);
    if (!string.IsNullOrEmpty(enemyQuadLightning.LightningSFX))
      AudioManager.Instance.PlayOneShot(enemyQuadLightning.LightningSFX, enemyQuadLightning.transform.position);
    for (int index = 0; index < beamThickness; ++index)
    {
      enemyQuadLightning.CreateBeam(enemyQuadLightning.transform.position, enemyQuadLightning.transform.position + directionOne * beamLength);
      enemyQuadLightning.CreateBeam(enemyQuadLightning.transform.position, enemyQuadLightning.transform.position + directionTwo * beamLength);
      enemyQuadLightning.CreateBeam(enemyQuadLightning.transform.position, enemyQuadLightning.transform.position + directionThree * beamLength);
      enemyQuadLightning.CreateBeam(enemyQuadLightning.transform.position, enemyQuadLightning.transform.position + directionFour * beamLength);
    }
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
  }

  public void RotateDirectionalArrows(Transform arrow, Vector3 direction)
  {
    float angle = Utils.GetAngle(this.transform.position, this.transform.position + direction);
    arrow.eulerAngles = new Vector3(0.0f, 0.0f, angle);
  }

  public void SetDirectionalArrowsActive(bool state)
  {
    for (int index = 0; index < this.directionalArrows.Length; ++index)
      this.directionalArrows[index].gameObject.SetActive(state);
  }

  public void CreateBeam(Vector3 from, Vector3 target)
  {
    int num = (int) Vector3.Distance(from, target) + 2;
    List<Vector3> vector3List = new List<Vector3>();
    vector3List.Add(from);
    for (int index = 1; index < num + 1; ++index)
    {
      float t = (float) index / (float) num;
      Vector3 vector3 = Vector3.Lerp(from, target, t) + (Vector3) UnityEngine.Random.insideUnitCircle * this.beamWidth;
      vector3List.Add(vector3);
    }
    vector3List.Add(target);
    ArrowLightningBeam.CreateBeam(vector3List.ToArray(), true, 0.75f, this.beamDuration, this.health.team, (Transform) null, signpostDuration: this.beamSignpost, health: this.health);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__71_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemyQuadLightning.loadedAddressableAssets.Add(obj);
    this.loadedLightningImpact = obj.Result;
    this.loadedLightningImpact.CreatePool(16 /*0x10*/, true);
  }

  public enum BeamPattern
  {
    Straight,
    Diagonal,
  }

  public class QuadLightningStateMachine : SimpleStateMachine
  {
    [CompilerGenerated]
    public EnemyQuadLightning \u003CParent\u003Ek__BackingField;

    public EnemyQuadLightning Parent
    {
      get => this.\u003CParent\u003Ek__BackingField;
      set => this.\u003CParent\u003Ek__BackingField = value;
    }

    public void SetParent(EnemyQuadLightning parent) => this.Parent = parent;
  }

  public class IdleState : SimpleState
  {
    public EnemyQuadLightning parent;
    public float idleDuration = 0.2f;
    public float progress;

    public override void OnEnter()
    {
      this.parent = ((EnemyQuadLightning.QuadLightningStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      this.parent.ClearPaths();
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      if (!GameManager.RoomActive)
        return;
      if (this.parent.IsQuadCooldownOver())
        this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.AttackSignpostState());
      if (this.parent.IsEnemyWithinFleeDistance())
      {
        if (this.parent.IsCircleCooldownOver())
          this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.CircleAttackState());
        else
          this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.RunFromEnemyState());
      }
      if ((double) (this.progress += Time.deltaTime * this.parent.Spine.timeScale) < (double) this.idleDuration)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.WanderState());
    }

    public override void OnExit()
    {
    }
  }

  public class CircleAttackState : SimpleState
  {
    public EnemyQuadLightning parent;
    public float stateDuration = 2f;
    public float progress;
    public float flashDuration = 0.5f;

    public override void OnEnter()
    {
      this.parent = ((EnemyQuadLightning.QuadLightningStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
      this.parent.speed = 0.0f;
      this.parent.ClearPaths();
      this.parent.DisableForces = true;
      this.parent.DisableKnockback = true;
      this.parent.StartCoroutine((IEnumerator) this.parent.CircleLightningAttack());
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      AudioManager.Instance.PlayOneShot(this.parent.CloseAttackSFX);
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      if ((double) this.progress < (double) this.flashDuration)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(this.progress / this.flashDuration);
      }
      else
      {
        foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(false);
      }
      if ((double) (this.progress += Time.deltaTime * this.parent.Spine.timeScale) < (double) this.stateDuration)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.IdleState());
    }

    public override void OnExit()
    {
      this.parent.DisableForces = false;
      this.parent.DisableKnockback = false;
      this.parent.lastCircleAttackTimeStamp = GameManager.GetInstance().CurrentTime;
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.parent.lastQuadAttackTimeStamp += 0.5f;
    }
  }

  public class RunFromEnemyState : SimpleState
  {
    public EnemyQuadLightning parent;
    public float attackBuffer = 0.5f;
    public float runDistance = 3f;

    public override void OnEnter()
    {
      this.parent = ((EnemyQuadLightning.QuadLightningStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Fleeing;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.MovingAnimation, true);
      this.parent.TargetObject = this.parent.GetNewTarget();
      this.parent.ClearPaths();
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      if (this.parent.IsQuadCooldownOver())
        this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.AttackSignpostState());
      if ((double) Vector3.Distance(this.parent.TargetObject.transform.position, this.parent.transform.position) > (double) this.parent.fleeDistance)
        this.parent.logicStateMachine.SetState((SimpleState) new EnemyQuadLightning.IdleState());
      else if (this.parent.IsCircleCooldownOver())
      {
        this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.CircleAttackState());
      }
      else
      {
        Vector3 closestPoint;
        BiomeGenerator.PointWithinIsland((this.parent.transform.position - this.parent.TargetObject.transform.position).normalized * this.parent.fleeDistance, out closestPoint);
        this.parent.givePath(closestPoint);
      }
    }

    public override void OnExit()
    {
    }
  }

  public class WanderState : SimpleState
  {
    public EnemyQuadLightning parent;
    public Vector2 DistanceRange = new Vector2(5f, 8f);
    public Vector3 currentWanderTarget;
    public float TurningArc = 90f;
    public float stopBuffer = 0.5f;

    public override void OnEnter()
    {
      this.parent = ((EnemyQuadLightning.QuadLightningStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Moving;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.MovingAnimation, true);
      this.parent.TargetObject = this.parent.GetNewTarget();
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.currentWanderTarget = this.GetNewTargetPosition();
      this.parent.givePath(this.currentWanderTarget);
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      if (this.parent.IsEnemyWithinFleeDistance())
        this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.RunFromEnemyState());
      if ((double) Vector3.Distance(this.parent.transform.position, this.currentWanderTarget) > (double) this.parent.StoppingDistance + (double) this.stopBuffer && this.parent.pathToFollow != null)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.IdleState());
    }

    public Vector3 GetNewTargetPosition()
    {
      float num = 100f;
      while ((double) --num > 0.0)
      {
        float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
        this.parent.RandomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
        float radius = 0.2f;
        Vector3 newTargetPosition = this.parent.transform.position + new Vector3(distance * Mathf.Cos(this.parent.RandomDirection), distance * Mathf.Sin(this.parent.RandomDirection));
        if (!((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.parent.transform.position, radius, (Vector2) Vector3.Normalize(newTargetPosition - this.parent.transform.position), distance, (int) this.parent.layerToCheck).collider != (UnityEngine.Object) null))
          return newTargetPosition;
        this.parent.RandomDirection = 90f - this.parent.RandomDirection;
      }
      return this.parent.transform.position;
    }

    public override void OnExit()
    {
    }
  }

  public class AttackSignpostState : SimpleState
  {
    public EnemyQuadLightning parent;
    public float progress;
    public float delayBeforeIndicators = 0.3f;
    public bool isIndicatorsSet;

    public override void OnEnter()
    {
      this.parent = ((EnemyQuadLightning.QuadLightningStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.LightningSignpostAnimation, false);
      this.parent.ClearPaths();
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress <= (double) this.parent.AattackSignpostDuration)
        return;
      this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.AttackState());
    }

    public override void OnExit()
    {
    }
  }

  public class AttackState : SimpleState
  {
    public EnemyQuadLightning parent;

    public override void OnEnter()
    {
      this.parent = ((EnemyQuadLightning.QuadLightningStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
      this.parent.LookAtTarget();
      this.parent.DisableKnockback = true;
      if (this.parent.currentBeamPattern == EnemyQuadLightning.BeamPattern.Straight)
      {
        this.parent.currentBeamPattern = EnemyQuadLightning.BeamPattern.Diagonal;
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.LightningSignpostAnimation2, false);
      }
      else
      {
        this.parent.currentBeamPattern = EnemyQuadLightning.BeamPattern.Straight;
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.LightningSignpostAnimation, false);
      }
      this.parent.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.OnAnimationComplete);
      this.parent.StartCoroutine((IEnumerator) this.parent.LightningAttack(this.parent.currentBeamPattern));
    }

    public override void Update()
    {
      this.parent.LookAtTarget();
      TrackEntry current = this.parent.Spine.AnimationState.GetCurrent(0);
      if (current != null && current.Animation != null && current.Animation.Name == this.parent.AttackAnimation2)
      {
        float duration = current.Animation.Duration;
        float amt = (double) duration > 0.0 ? Mathf.Clamp01(current.TrackTime / duration) : 1f;
        foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(amt);
      }
      else
      {
        foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(false);
      }
    }

    public void OnAnimationComplete(TrackEntry trackEntry)
    {
      if (trackEntry.Animation == null || !(trackEntry.Animation.Name == this.parent.AttackAnimation2))
        return;
      this.parent.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.OnAnimationComplete);
      this.parentStateMachine.SetState((SimpleState) new EnemyQuadLightning.IdleState());
    }

    public override void OnExit()
    {
      this.parent.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.OnAnimationComplete);
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.parent.lastQuadAttackTimeStamp = GameManager.GetInstance().CurrentTime;
      this.parent.lastCircleAttackTimeStamp += 0.5f;
      this.parent.DisableKnockback = false;
    }
  }
}
