// Decompiled with JetBrains decompiler
// Type: EnemyDorryMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyDorryMiniboss : EnemyDual
{
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public GameObject targetObject;
  public EnemyDorryMiniboss.DorryState currentState;
  public EnemyDorryMiniboss.DorryStateChange onStateChange;
  public bool IsNG;
  [SerializeField]
  public float movementSpeed;
  [SerializeField]
  public float chaseSpeed;
  [SerializeField]
  public LayerMask obstaclesLayermask;
  [SerializeField]
  public float obstaclesDistanceCheck;
  public float repathTimer;
  [SerializeField]
  public Transform damageCollidersParent;
  [SerializeField]
  public ColliderEvents[] damageColliders;
  [SerializeField]
  public float damage;
  [SerializeField]
  public float closeAttackDistance;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float prob_attack_1_attack_5;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float prob_attack_1_attack_5_rage;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float prob_attack_2_attack_4;
  [SerializeField]
  public GameObject projectilePrefab;
  [SerializeField]
  public bool rageActive;
  [SerializeField]
  public ParticleSystem levitateVFX;
  [SerializeField]
  public float attack_1_delay;
  [SerializeField]
  public float attack_1_duration;
  [SerializeField]
  public float attack_2_A_delay;
  [SerializeField]
  public float attack_2_A_duration;
  [SerializeField]
  public float attack_2_B_delay;
  [SerializeField]
  public float attack_2_B_duration;
  [SerializeField]
  public float attack_3_delay;
  [SerializeField]
  public float attack_3_duration;
  [SerializeField]
  public float attack_3_chargeSpeed;
  [SerializeField]
  public float attack_3_ng_projectileRate;
  [SerializeField]
  public float attack_3_ng_projectileAngleOffset;
  [SerializeField]
  public float attack_3_ng_projectileSpeed;
  [SerializeField]
  public float attack_4_delay;
  [SerializeField]
  public float attack_4_duration;
  [SerializeField]
  public float attack_4_chargeSpeed;
  [SerializeField]
  public float attack_4_angleIncrement;
  [SerializeField]
  public float attack_4_angularSpeed;
  [SerializeField]
  public float attack_5_delay;
  [SerializeField]
  public float attack_5_duration;
  [SerializeField]
  public float attack_6_delay;
  [SerializeField]
  public float attack_6_duration;
  [SerializeField]
  public float attack_6_chargeSpeed;
  [SerializeField]
  public float attack_6_impactDistance;
  [SerializeField]
  public float attack_7_delay;
  [SerializeField]
  public float attack_7_duration;
  [SerializeField]
  public float attack_7_chargeSpeed;
  [SerializeField]
  public float attack_7_playerMinDistance;
  [SerializeField]
  public float attack_7_partnerMaxDistance;
  [SerializeField]
  public LayerMask attack_7_layerMask;
  [SerializeField]
  public float attack_8_projectileAngleRate;
  [SerializeField]
  public float attack_8_projectileRate;
  [SerializeField]
  public float attack_8_projectileSpeed;
  [SerializeField]
  public float attack_8_delay;
  [SerializeField]
  public float attack_8_duration;
  [SerializeField]
  public float attack_8_chargeSpeed;
  [SerializeField]
  public float attack_8_angleIncrement;
  [SerializeField]
  public float attack_8_angularSpeed;
  [SerializeField]
  public float attack_9_projectileCount;
  [SerializeField]
  public float attack_9_projectileSpeed;
  [SerializeField]
  public float attack_9_delay;
  [SerializeField]
  public float attack_9_duration;
  [SerializeField]
  public float attack_9_chargeSpeed;
  [SerializeField]
  public float attack_9_impactDistance;
  [SerializeField]
  public float attack_12_projectileAngleOffset;
  [SerializeField]
  public float attack_12_projectileRate;
  [SerializeField]
  public float attack_12_projectileSpeed;
  [SerializeField]
  public float attack_12_delay;
  [SerializeField]
  public float attack_12_duration;
  [SerializeField]
  public float attack_12_chargeSpeed;
  [SerializeField]
  public float attack_together_delay;
  [SerializeField]
  public float attack_together_duration;
  [SerializeField]
  public float attack_together_chargeSpeed;
  [SerializeField]
  public float attack_together_angleIncrement;
  [SerializeField]
  public float attack_together_angularSpeed;
  [SerializeField]
  public float idleTimePhase1;
  [SerializeField]
  public float idleTimePhase2;
  public float idleTimer;
  [SerializeField]
  public float phaseDelay;
  [SerializeField]
  public float phaseDuration;
  [SerializeField]
  public float phaseHp;
  public Vector2 linealDirection;
  public Vector2 linealMovement;
  public Vector2 angularMovement;
  public Vector2 currentMovement;
  public Coroutine idleRoutine;
  public Coroutine moveRoutine;
  public Coroutine attack_1_Routine;
  public Coroutine attack_2_Routine;
  public Coroutine attack_3_Routine;
  public Coroutine attack_4_Routine;
  public Coroutine attack_5_Routine;
  public Coroutine attack_6_Routine;
  public Coroutine attack_7_Routine;
  public Coroutine attack_8_Routine;
  public Coroutine attack_9_Routine;
  public Coroutine attack_12_Routine;
  public Coroutine sync_Routine;
  public Coroutine attack_together_Routine;
  public Coroutine phase_Routine;
  public Health EnemyHealth;

  public EnemyDorryMiniboss.DorryState CURRENT_DORRY_STATE
  {
    get => this.currentState;
    set
    {
      if (this.onStateChange != null)
        this.OnStateChange(value, this.currentState);
      this.currentState = value;
    }
  }

  public Vector2 FacingDirection
  {
    get
    {
      return new Vector2(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f))).normalized;
    }
  }

  public Vector2 PlayerDirection
  {
    get
    {
      return new Vector2(Mathf.Cos(Utils.GetAngle(this.transform.position, this.targetObject.transform.position) * ((float) Math.PI / 180f)), Mathf.Sin(Utils.GetAngle(this.transform.position, this.targetObject.transform.position) * ((float) Math.PI / 180f))).normalized;
    }
  }

  public string IdleAnim => !this.rageActive ? "idle" : "idle-rage";

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    this.InitDamageColliders();
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
    this.onStateChange += new EnemyDorryMiniboss.DorryStateChange(this.OnStateChange);
    this.attackTogetherTimeStamp = GameManager.GetInstance().CurrentTime;
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    this.DisposeDamageColliders();
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    this.onStateChange -= new EnemyDorryMiniboss.DorryStateChange(this.OnStateChange);
    this.OnPhaseStart = this.OnPhaseStart - new EnemyDual.PhaseAction(DoubleUIBossHUD.Instance.OnBoss1Phase);
  }

  public override void FixedUpdate()
  {
    base.FixedUpdate();
    this.linealMovement *= Time.deltaTime * this.Spine.timeScale;
    this.angularMovement *= Time.deltaTime * this.Spine.timeScale;
    this.currentMovement = this.linealMovement + this.angularMovement;
    if (!this.HasObstacle(this.currentMovement.normalized, this.obstaclesDistanceCheck, this.obstaclesLayermask))
    {
      this.rb.MovePosition(this.rb.position + this.currentMovement);
    }
    else
    {
      this.state.facingAngle += 180f;
      this.state.LookAngle = this.state.facingAngle;
      this.linealDirection = this.FacingDirection;
    }
    this.linealMovement = Vector2.zero;
    this.angularMovement = Vector2.zero;
    this.currentMovement = Vector2.zero;
  }

  public new void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!PlayerController.CanParryAttacks || FromBehind || AttackType != Health.AttackTypes.Melee)
      return;
    this.health.WasJustParried = true;
    this.SimpleSpineFlash.FlashWhite(false);
    this.SeperateObject = true;
    this.UsePathing = true;
    this.health.invincible = false;
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
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.health.HasShield || this.health.WasJustParried)
      return;
    if (this.state.CURRENT_STATE != StateMachine.State.Attacking)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
        this.state.CURRENT_STATE = StateMachine.State.HitRight;
      if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
        this.state.CURRENT_STATE = StateMachine.State.HitLeft;
    }
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    this.SimpleSpineFlash.FlashFillRed();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemyDorryMiniboss.targetObject == (UnityEngine.Object) null)
    {
      enemyDorryMiniboss.SetTarget();
      yield return (object) null;
    }
    enemyDorryMiniboss.state.CURRENT_STATE = StateMachine.State.Dancing;
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator IdleState()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    while ((double) (enemyDorryMiniboss.idleTimer -= Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) > 0.0)
      yield return (object) null;
    while ((UnityEngine.Object) enemyDorryMiniboss.targetObject == (UnityEngine.Object) null)
    {
      enemyDorryMiniboss.SetTarget();
      yield return (object) null;
    }
    if (!enemyDorryMiniboss.rageActive && enemyDorryMiniboss.hasToPhase)
      enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Phase;
    else if (!enemyDorryMiniboss.rageActive && enemyDorryMiniboss.IsAttackTogetherReady)
      enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Sync;
    else if ((double) Vector2.Distance((Vector2) enemyDorryMiniboss.transform.position, (Vector2) enemyDorryMiniboss.targetObject.transform.position) >= (double) enemyDorryMiniboss.attack_7_playerMinDistance)
    {
      if (!enemyDorryMiniboss.rageActive)
      {
        if ((double) Vector2.Distance((Vector2) enemyDorryMiniboss.partner.transform.position, (Vector2) enemyDorryMiniboss.targetObject.transform.position) >= (double) enemyDorryMiniboss.attack_7_partnerMaxDistance)
          enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Attack_7;
        else
          enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Chase;
      }
      else
        enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Attack_12;
    }
    else
      enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Chase;
  }

  public IEnumerator ChaseState()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    while ((UnityEngine.Object) enemyDorryMiniboss.targetObject != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(enemyDorryMiniboss.transform.position, enemyDorryMiniboss.targetObject.transform.position) < (double) enemyDorryMiniboss.closeAttackDistance)
      {
        float num = UnityEngine.Random.value;
        enemyDorryMiniboss.CURRENT_DORRY_STATE = enemyDorryMiniboss.rageActive ? ((double) num < (double) enemyDorryMiniboss.prob_attack_1_attack_5_rage ? EnemyDorryMiniboss.DorryState.Attack_1 : EnemyDorryMiniboss.DorryState.Attack_5) : ((double) num < (double) enemyDorryMiniboss.prob_attack_1_attack_5 ? EnemyDorryMiniboss.DorryState.Attack_1 : EnemyDorryMiniboss.DorryState.Attack_5);
        yield break;
      }
      Vector3 position1 = enemyDorryMiniboss.targetObject.transform.position;
      Vector3 closestPoint;
      Vector3 position2 = BiomeGenerator.PointWithinIsland(position1, out closestPoint) ? position1 : closestPoint;
      enemyDorryMiniboss.FacePosition(position2);
      enemyDorryMiniboss.Move(enemyDorryMiniboss.chaseSpeed);
      yield return (object) null;
    }
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_1()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    yield return (object) enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageColliderTime(enemyDorryMiniboss.damageColliders[0], enemyDorryMiniboss.attack_1_delay));
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_1_duration * enemyDorryMiniboss.Spine.timeScale);
    float num = UnityEngine.Random.value;
    enemyDorryMiniboss.CURRENT_DORRY_STATE = enemyDorryMiniboss.IsNG ? EnemyDorryMiniboss.DorryState.Attack_3 : ((double) num < (double) enemyDorryMiniboss.prob_attack_2_attack_4 ? EnemyDorryMiniboss.DorryState.Attack_2 : (enemyDorryMiniboss.rageActive ? EnemyDorryMiniboss.DorryState.Attack_8 : EnemyDorryMiniboss.DorryState.Attack_4));
  }

  public IEnumerator Attack_2()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageColliderTime(enemyDorryMiniboss.damageColliders[1], enemyDorryMiniboss.attack_2_A_delay));
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_2_A_duration * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageColliderTime(enemyDorryMiniboss.damageColliders[2], enemyDorryMiniboss.attack_2_B_delay));
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_2_B_duration * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Attack_3;
  }

  public IEnumerator Attack_3()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    float delayTime = 0.0f;
    while ((double) (delayTime += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.attack_3_delay)
    {
      enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position);
      yield return (object) null;
    }
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageCollider(enemyDorryMiniboss.damageColliders[3], 0.0f));
    float time = 0.0f;
    float projectileTime = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.attack_3_duration)
    {
      Vector3 direction = Quaternion.AngleAxis(enemyDorryMiniboss.state.facingAngle, Vector3.forward) * Vector3.right;
      if (!enemyDorryMiniboss.HasObstacle((Vector2) direction, 1f, enemyDorryMiniboss.obstaclesLayermask))
      {
        enemyDorryMiniboss.Move(enemyDorryMiniboss.attack_3_chargeSpeed);
        if (enemyDorryMiniboss.rageActive && enemyDorryMiniboss.IsNG)
        {
          projectileTime += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale;
          if ((double) projectileTime >= (double) enemyDorryMiniboss.attack_3_ng_projectileRate)
          {
            enemyDorryMiniboss.ShootProjectile(enemyDorryMiniboss.rb.position, enemyDorryMiniboss.state.facingAngle + enemyDorryMiniboss.attack_3_ng_projectileAngleOffset, enemyDorryMiniboss.attack_3_ng_projectileSpeed);
            enemyDorryMiniboss.ShootProjectile(enemyDorryMiniboss.rb.position, enemyDorryMiniboss.state.facingAngle - enemyDorryMiniboss.attack_3_ng_projectileAngleOffset, enemyDorryMiniboss.attack_3_ng_projectileSpeed);
            projectileTime = 0.0f;
          }
        }
        yield return (object) null;
      }
      else
        break;
    }
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-3-impact", false);
    enemyDorryMiniboss.DisableDamageCollider(enemyDorryMiniboss.damageColliders[3]);
    yield return (object) new WaitForSeconds(1f * enemyDorryMiniboss.Spine.timeScale);
    float num = UnityEngine.Random.value;
    enemyDorryMiniboss.CURRENT_DORRY_STATE = enemyDorryMiniboss.IsNG ? ((double) num < (double) enemyDorryMiniboss.prob_attack_2_attack_4 ? EnemyDorryMiniboss.DorryState.Attack_2 : (enemyDorryMiniboss.rageActive ? EnemyDorryMiniboss.DorryState.Attack_8 : EnemyDorryMiniboss.DorryState.Attack_4)) : EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator Attack_4()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position);
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_4_delay * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-4-loop", true);
    enemyDorryMiniboss.levitateVFX.Play();
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageCollider(enemyDorryMiniboss.damageColliders[4], 0.0f));
    float time = 0.0f;
    enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position);
    while ((double) (time += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.attack_4_duration)
    {
      enemyDorryMiniboss.Move(enemyDorryMiniboss.attack_4_chargeSpeed, enemyDorryMiniboss.PlayerDirection);
      enemyDorryMiniboss.MoveCircle(enemyDorryMiniboss.attack_4_angleIncrement, enemyDorryMiniboss.attack_4_angularSpeed);
      yield return (object) null;
    }
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, enemyDorryMiniboss.IdleAnim, true);
    enemyDorryMiniboss.levitateVFX.Stop();
    enemyDorryMiniboss.DisableDamageCollider(enemyDorryMiniboss.damageColliders[4]);
    yield return (object) new WaitForSeconds(1f * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator Attack_5()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageColliderTime(enemyDorryMiniboss.damageColliders[5], enemyDorryMiniboss.attack_5_delay));
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_5_duration * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.CURRENT_DORRY_STATE = enemyDorryMiniboss.rageActive ? EnemyDorryMiniboss.DorryState.Attack_9 : EnemyDorryMiniboss.DorryState.Attack_6;
  }

  public IEnumerator Attack_6()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.health.invincible = true;
    float timeStamp = GameManager.GetInstance().CurrentTime;
    Vector2 targetPosition = (Vector2) enemyDorryMiniboss.targetObject.transform.position;
    while ((double) GameManager.GetInstance().TimeSince(timeStamp) < (double) enemyDorryMiniboss.attack_6_delay)
    {
      targetPosition = (Vector2) enemyDorryMiniboss.targetObject.transform.position;
      enemyDorryMiniboss.FacePosition((Vector3) targetPosition);
      yield return (object) null;
    }
    while ((double) Vector2.Distance(enemyDorryMiniboss.rb.position, targetPosition) > (double) enemyDorryMiniboss.attack_6_impactDistance)
    {
      Vector3 direction = Quaternion.AngleAxis(enemyDorryMiniboss.state.facingAngle, Vector3.forward) * Vector3.right;
      if (!enemyDorryMiniboss.HasObstacle((Vector2) direction, 1f, enemyDorryMiniboss.obstaclesLayermask))
      {
        enemyDorryMiniboss.Move(enemyDorryMiniboss.attack_6_chargeSpeed);
        yield return (object) null;
      }
      else
        break;
    }
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-6-charge", false);
    enemyDorryMiniboss.Spine.AnimationState.AddAnimation(0, "attack-6-impact", false, 0.0f);
    enemyDorryMiniboss.Spine.AnimationState.AddAnimation(0, enemyDorryMiniboss.IdleAnim, true, 0.0f);
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageColliderTime(enemyDorryMiniboss.damageColliders[6], enemyDorryMiniboss.attack_6_delay));
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_6_duration * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.health.invincible = false;
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator Attack_7()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    float delayTime = 0.0f;
    while ((double) (delayTime += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.attack_7_delay)
    {
      enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position);
      yield return (object) null;
    }
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageCollider(enemyDorryMiniboss.damageColliders[7], 0.0f));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.attack_7_duration)
    {
      enemyDorryMiniboss.Move(enemyDorryMiniboss.attack_7_chargeSpeed);
      Vector3 direction = Quaternion.AngleAxis(enemyDorryMiniboss.state.facingAngle, Vector3.forward) * Vector3.right;
      if (!enemyDorryMiniboss.HasObstacle((Vector2) direction, 2f, enemyDorryMiniboss.obstaclesLayermask))
        yield return (object) null;
      else
        break;
    }
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-7-end", false);
    enemyDorryMiniboss.DisableDamageCollider(enemyDorryMiniboss.damageColliders[7]);
    yield return (object) new WaitForSeconds(1f * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator Attack_8()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position);
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_4_delay * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-4-loop", true);
    enemyDorryMiniboss.levitateVFX.Play();
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageCollider(enemyDorryMiniboss.damageColliders[4], 0.0f));
    float time = 0.0f;
    float projectileTime = 0.0f;
    float projectileAngleOffset = 0.0f;
    enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position);
    while ((double) (time += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.attack_4_duration)
    {
      enemyDorryMiniboss.MoveCircle(enemyDorryMiniboss.attack_8_angleIncrement, enemyDorryMiniboss.attack_8_angularSpeed);
      enemyDorryMiniboss.Move(enemyDorryMiniboss.attack_8_chargeSpeed, enemyDorryMiniboss.PlayerDirection);
      projectileTime += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale;
      if ((double) projectileTime >= (double) enemyDorryMiniboss.attack_8_projectileRate)
      {
        enemyDorryMiniboss.ShootProjectile(enemyDorryMiniboss.rb.position, projectileAngleOffset, enemyDorryMiniboss.attack_8_projectileSpeed);
        enemyDorryMiniboss.ShootProjectile(enemyDorryMiniboss.rb.position, projectileAngleOffset + 120f, enemyDorryMiniboss.attack_8_projectileSpeed);
        enemyDorryMiniboss.ShootProjectile(enemyDorryMiniboss.rb.position, projectileAngleOffset - 120f, enemyDorryMiniboss.attack_8_projectileSpeed);
        projectileAngleOffset += enemyDorryMiniboss.attack_8_projectileAngleRate;
        projectileTime = 0.0f;
      }
      yield return (object) null;
    }
    enemyDorryMiniboss.DisableDamageCollider(enemyDorryMiniboss.damageColliders[4]);
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-4-end", false);
    enemyDorryMiniboss.Spine.AnimationState.AddAnimation(0, enemyDorryMiniboss.IdleAnim, true, 0.0f);
    enemyDorryMiniboss.levitateVFX.Stop();
    yield return (object) new WaitForSeconds(1f * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator Attack_9()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.health.invincible = true;
    float timeStamp = GameManager.GetInstance().CurrentTime;
    Vector2 targetPosition = (Vector2) enemyDorryMiniboss.targetObject.transform.position;
    while ((double) GameManager.GetInstance().TimeSince(timeStamp) < (double) enemyDorryMiniboss.attack_9_delay)
    {
      targetPosition = (Vector2) enemyDorryMiniboss.targetObject.transform.position;
      enemyDorryMiniboss.FacePosition((Vector3) targetPosition);
      yield return (object) null;
    }
    while ((double) Vector2.Distance(enemyDorryMiniboss.rb.position, targetPosition) > (double) enemyDorryMiniboss.attack_6_impactDistance)
    {
      Vector3 direction = Quaternion.AngleAxis(enemyDorryMiniboss.state.facingAngle, Vector3.forward) * Vector3.right;
      if (!enemyDorryMiniboss.HasObstacle((Vector2) direction, 1f, enemyDorryMiniboss.obstaclesLayermask))
      {
        enemyDorryMiniboss.Move(enemyDorryMiniboss.attack_9_chargeSpeed);
        yield return (object) null;
      }
      else
        break;
    }
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-6-charge", false);
    enemyDorryMiniboss.Spine.AnimationState.AddAnimation(0, "attack-6-impact", false, 0.0f);
    enemyDorryMiniboss.Spine.AnimationState.AddAnimation(0, enemyDorryMiniboss.IdleAnim, true, 0.0f);
    yield return (object) enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageColliderTime(enemyDorryMiniboss.damageColliders[6], enemyDorryMiniboss.attack_9_delay));
    float num = 360f / enemyDorryMiniboss.attack_9_projectileCount;
    for (int index = 0; (double) index < (double) enemyDorryMiniboss.attack_9_projectileCount; ++index)
      enemyDorryMiniboss.ShootProjectile(enemyDorryMiniboss.rb.position, num * (float) index, 5f);
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_9_duration * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.health.invincible = false;
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator Attack_12()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    float delay = 0.0f;
    while ((double) delay < (double) enemyDorryMiniboss.attack_12_delay)
    {
      enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position);
      delay += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale;
      yield return (object) null;
    }
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageCollider(enemyDorryMiniboss.damageColliders[7], 0.0f));
    float time = 0.0f;
    float projectileTime = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.attack_12_duration)
    {
      Vector3 direction = Quaternion.AngleAxis(enemyDorryMiniboss.state.facingAngle, Vector3.forward) * Vector3.right;
      if (!enemyDorryMiniboss.HasObstacle((Vector2) direction, 1f, enemyDorryMiniboss.obstaclesLayermask))
      {
        enemyDorryMiniboss.Move(enemyDorryMiniboss.attack_12_chargeSpeed);
        projectileTime += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale;
        if ((double) projectileTime >= (double) enemyDorryMiniboss.attack_12_projectileRate)
        {
          enemyDorryMiniboss.ShootProjectile(enemyDorryMiniboss.rb.position, enemyDorryMiniboss.state.facingAngle + enemyDorryMiniboss.attack_12_projectileAngleOffset, enemyDorryMiniboss.attack_12_projectileSpeed);
          enemyDorryMiniboss.ShootProjectile(enemyDorryMiniboss.rb.position, enemyDorryMiniboss.state.facingAngle - enemyDorryMiniboss.attack_12_projectileAngleOffset, enemyDorryMiniboss.attack_12_projectileSpeed);
          projectileTime = 0.0f;
        }
        yield return (object) null;
      }
      else
        break;
    }
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-7-end", false);
    enemyDorryMiniboss.DisableDamageCollider(enemyDorryMiniboss.damageColliders[7]);
    yield return (object) new WaitForSeconds(1f * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator Sync()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.syncMaxTime && !enemyDorryMiniboss.hasToPhase)
      yield return (object) null;
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_Together()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position, enemyDorryMiniboss.targetObject.transform.position);
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.attack_together_delay * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-4-loop", true);
    enemyDorryMiniboss.levitateVFX.Play();
    enemyDorryMiniboss.StartCoroutine((IEnumerator) enemyDorryMiniboss.EnableDamageCollider(enemyDorryMiniboss.damageColliders[4], 0.0f));
    float time = 0.0f;
    enemyDorryMiniboss.FacePosition(enemyDorryMiniboss.targetObject.transform.position, enemyDorryMiniboss.targetObject.transform.position);
    enemyDorryMiniboss.linealDirection = enemyDorryMiniboss.FacingDirection;
    while ((double) (time += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.attack_together_duration)
    {
      enemyDorryMiniboss.Move(enemyDorryMiniboss.attack_together_chargeSpeed, enemyDorryMiniboss.linealDirection);
      enemyDorryMiniboss.MoveCircle(enemyDorryMiniboss.attack_together_angleIncrement, enemyDorryMiniboss.attack_together_angularSpeed);
      yield return (object) null;
    }
    enemyDorryMiniboss.DisableDamageCollider(enemyDorryMiniboss.damageColliders[4]);
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "attack-4-end", false);
    enemyDorryMiniboss.Spine.AnimationState.AddAnimation(0, enemyDorryMiniboss.IdleAnim, true, 0.0f);
    enemyDorryMiniboss.levitateVFX.Stop();
    yield return (object) new WaitForSeconds(1f * enemyDorryMiniboss.Spine.timeScale);
    enemyDorryMiniboss.attackTogetherTimeStamp = GameManager.GetInstance().CurrentTime;
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
  }

  public IEnumerator Phase()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "rage-a", false);
    yield return (object) new WaitForSeconds(enemyDorryMiniboss.phaseDelay);
    enemyDorryMiniboss.Spine.Skeleton.SetSkin(enemyDorryMiniboss.IsNG ? "rage-juiced" : "rage");
    enemyDorryMiniboss.Spine.Skeleton.SetSlotsToSetupPose();
    enemyDorryMiniboss.Spine.Skeleton.SetBonesToSetupPose();
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, "rage-b", false);
    enemyDorryMiniboss.Spine.AnimationState.AddAnimation(0, "idle-rage", false, 0.0f);
    float healingAmount = (enemyDorryMiniboss.health.totalHP - enemyDorryMiniboss.health.HP) / enemyDorryMiniboss.phaseDuration;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDorryMiniboss.Spine.timeScale) < (double) enemyDorryMiniboss.phaseDuration)
    {
      enemyDorryMiniboss.health.Heal(healingAmount * Time.deltaTime);
      DoubleUIBossHUD.Instance.ForceHealthAmount1(enemyDorryMiniboss.health.HP / enemyDorryMiniboss.health.totalHP);
      yield return (object) null;
    }
    enemyDorryMiniboss.health.totalHP = enemyDorryMiniboss.phaseHp;
    enemyDorryMiniboss.health.HP = enemyDorryMiniboss.health.totalHP;
    DoubleUIBossHUD.Instance.ForceHealthAmount1(1f);
    enemyDorryMiniboss.health.invincible = false;
    enemyDorryMiniboss.rageActive = true;
    enemyDorryMiniboss.hasToPhase = false;
    enemyDorryMiniboss.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Idle;
    yield return (object) null;
  }

  public void ShootProjectile(Vector2 position, float angle, float speed)
  {
    Projectile component = ObjectPool.Spawn(this.projectilePrefab, (Vector3) position, Quaternion.identity).GetComponent<Projectile>();
    component.Angle = angle;
    component.Speed = speed;
    component.team = this.health.team;
    component.Owner = this.health;
    component.LifeTime = 20f;
  }

  public void SetTarget()
  {
    Health closestTarget = this.GetClosestTarget(this.health.team == Health.Team.PlayerTeam);
    if (!(bool) (UnityEngine.Object) closestTarget)
      return;
    this.targetObject = closestTarget.gameObject;
    this.VisionRange = int.MaxValue;
  }

  public void FacePosition(Vector3 position)
  {
    float angle = Utils.GetAngle(this.transform.position, position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? 1f : -1f;
    Vector3 vector3 = new Vector3(-1f, 1f, 1f);
    this.damageCollidersParent.localScale = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? Vector3.one : vector3;
  }

  public void FacePosition(Vector3 position, Vector3 lookAt)
  {
    float angle1 = Utils.GetAngle(this.transform.position, position);
    this.state.facingAngle = angle1;
    this.state.LookAngle = angle1;
    float angle2 = Utils.GetAngle(this.transform.position, lookAt);
    this.Spine.skeleton.ScaleX = (double) angle2 <= 90.0 || (double) angle2 >= 270.0 ? 1f : -1f;
    Vector3 vector3 = new Vector3(-1f, 1f, 1f);
    this.damageCollidersParent.localScale = (double) angle2 <= 90.0 || (double) angle2 >= 270.0 ? Vector3.one : vector3;
  }

  public void OnStateChange(
    EnemyDorryMiniboss.DorryState newState,
    EnemyDorryMiniboss.DorryState prevState)
  {
    switch (newState)
    {
      case EnemyDorryMiniboss.DorryState.Idle:
        this.idleTimer = this.rageActive ? this.idleTimePhase2 : this.idleTimePhase1;
        this.Spine.AnimationState.SetAnimation(0, this.IdleAnim, true);
        if (this.idleRoutine != null)
          this.StopCoroutine(this.idleRoutine);
        this.idleRoutine = this.StartCoroutine((IEnumerator) this.IdleState());
        break;
      case EnemyDorryMiniboss.DorryState.Chase:
        this.repathTimer = 1f;
        this.speed = this.movementSpeed * this.SpeedMultiplier;
        this.Spine.AnimationState.SetAnimation(0, "move", true);
        if (this.moveRoutine != null)
          this.StopCoroutine(this.moveRoutine);
        this.moveRoutine = this.StartCoroutine((IEnumerator) this.ChaseState());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_1:
        this.Spine.AnimationState.SetAnimation(0, "attack-1", false);
        this.Spine.AnimationState.AddAnimation(0, this.IdleAnim, true, 0.0f);
        if (this.attack_1_Routine != null)
          this.StopCoroutine(this.attack_1_Routine);
        this.attack_1_Routine = this.StartCoroutine((IEnumerator) this.Attack_1());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_2:
        this.Spine.AnimationState.SetAnimation(0, "attack-2", false);
        this.Spine.AnimationState.AddAnimation(0, this.IdleAnim, true, 0.0f);
        if (this.attack_2_Routine != null)
          this.StopCoroutine(this.attack_2_Routine);
        this.attack_2_Routine = this.StartCoroutine((IEnumerator) this.Attack_2());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_3:
        this.Spine.AnimationState.SetAnimation(0, "attack-3-start", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-3-loop", true, 0.0f);
        if (this.attack_3_Routine != null)
          this.StopCoroutine(this.attack_3_Routine);
        this.attack_3_Routine = this.StartCoroutine((IEnumerator) this.Attack_3());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_4:
        this.Spine.AnimationState.SetAnimation(0, "attack-4-start", false);
        if (this.attack_4_Routine != null)
          this.StopCoroutine(this.attack_4_Routine);
        this.attack_4_Routine = this.StartCoroutine((IEnumerator) this.Attack_4());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_5:
        this.Spine.AnimationState.SetAnimation(0, "attack-5", false);
        this.Spine.AnimationState.AddAnimation(0, "fly", true, 0.0f);
        if (this.attack_5_Routine != null)
          this.StopCoroutine(this.attack_5_Routine);
        this.attack_5_Routine = this.StartCoroutine((IEnumerator) this.Attack_5());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_6:
        if (this.attack_6_Routine != null)
          this.StopCoroutine(this.attack_6_Routine);
        this.attack_6_Routine = this.StartCoroutine((IEnumerator) this.Attack_6());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_7:
        this.Spine.AnimationState.SetAnimation(0, "attack-7-start", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-7-charge", true, 0.0f);
        if (this.attack_7_Routine != null)
          this.StopCoroutine(this.attack_7_Routine);
        this.attack_7_Routine = this.StartCoroutine((IEnumerator) this.Attack_7());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_8:
        this.Spine.AnimationState.SetAnimation(0, "attack-4-start", false);
        if (this.attack_8_Routine != null)
          this.StopCoroutine(this.attack_8_Routine);
        this.attack_8_Routine = this.StartCoroutine((IEnumerator) this.Attack_8());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_9:
        if (this.attack_9_Routine != null)
          this.StopCoroutine(this.attack_9_Routine);
        this.attack_9_Routine = this.StartCoroutine((IEnumerator) this.Attack_9());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_12:
        this.Spine.AnimationState.SetAnimation(0, "attack-7-start", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-7-charge", true, 0.0f);
        if (this.attack_12_Routine != null)
          this.StopCoroutine(this.attack_12_Routine);
        this.attack_12_Routine = this.StartCoroutine((IEnumerator) this.Attack_12());
        break;
      case EnemyDorryMiniboss.DorryState.Sync:
        this.Spine.AnimationState.SetAnimation(0, this.IdleAnim, true);
        if (this.sync_Routine != null)
          this.StopCoroutine(this.sync_Routine);
        this.sync_Routine = this.StartCoroutine((IEnumerator) this.Sync());
        break;
      case EnemyDorryMiniboss.DorryState.Attack_Together:
        this.Spine.AnimationState.SetAnimation(0, "attack-4-start", false);
        if (this.sync_Routine != null)
          this.StopCoroutine(this.sync_Routine);
        if (this.attack_together_Routine != null)
          this.StopCoroutine(this.attack_together_Routine);
        this.attack_together_Routine = this.StartCoroutine((IEnumerator) this.Attack_Together());
        break;
      case EnemyDorryMiniboss.DorryState.Phase:
        if (this.phase_Routine != null)
          this.StopCoroutine(this.phase_Routine);
        this.phase_Routine = this.StartCoroutine((IEnumerator) this.Phase());
        break;
    }
  }

  public void FindPath(Vector3 PointToCheck) => this.givePath(PointToCheck, forceIgnoreAStar: true);

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, this.IdleAnim, true);
  }

  public IEnumerator PlaceIE()
  {
    EnemyDorryMiniboss enemyDorryMiniboss = this;
    enemyDorryMiniboss.ClearPaths();
    Vector3 offset = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      enemyDorryMiniboss.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 vector3 = (PlayerFarming.Instance.transform.position + offset) with
      {
        z = 0.0f
      };
      enemyDorryMiniboss.transform.position = vector3;
      yield return (object) null;
    }
    enemyDorryMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDorryMiniboss.Spine.AnimationState.SetAnimation(0, enemyDorryMiniboss.IdleAnim, true);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null))
      return;
    if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  public void OnDamageTriggerStay(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null))
      return;
    if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  public IEnumerator EnableDamageColliderTime(ColliderEvents damageCollider, float initialDelay)
  {
    if ((bool) (UnityEngine.Object) damageCollider)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      damageCollider.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      damageCollider.SetActive(false);
    }
  }

  public IEnumerator EnableDamageCollider(ColliderEvents damageCollider, float initialDelay)
  {
    if ((bool) (UnityEngine.Object) damageCollider)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      damageCollider.SetActive(true);
    }
  }

  public void DisableDamageCollider(ColliderEvents damageCollider)
  {
    if (!(bool) (UnityEngine.Object) damageCollider)
      return;
    damageCollider.SetActive(false);
  }

  public void DisableAllDamageColliders()
  {
    foreach (ColliderEvents damageCollider in this.damageColliders)
      damageCollider.SetActive(false);
  }

  public void Move(float movementSpeed)
  {
    if (float.IsNaN(this.state.facingAngle))
      return;
    if (float.IsNaN(this.speed) || float.IsInfinity(this.speed))
      this.speed = 0.0f;
    this.speed = movementSpeed;
    this.moveVX = this.speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f));
    this.moveVY = this.speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f));
    if (float.IsNaN(this.moveVX) || float.IsInfinity(this.moveVX))
      this.moveVX = 0.0f;
    if (float.IsNaN(this.moveVY) || float.IsInfinity(this.moveVY))
      this.moveVY = 0.0f;
    this.linealMovement = new Vector2(this.moveVX, this.moveVY);
  }

  public void Move(float movementSpeed, Vector2 direction)
  {
    if (float.IsNaN(this.state.facingAngle))
      return;
    if (float.IsNaN(this.speed) || float.IsInfinity(this.speed))
      this.speed = 0.0f;
    direction.Normalize();
    this.speed = movementSpeed;
    this.moveVX = this.speed * direction.x;
    this.moveVY = this.speed * direction.y;
    if (!this.UseDeltaTime)
    {
      double unscaledDeltaTime = (double) GameManager.UnscaledDeltaTime;
    }
    else
    {
      double deltaTime = (double) GameManager.DeltaTime;
    }
    if (float.IsNaN(this.moveVX) || float.IsInfinity(this.moveVX))
      this.moveVX = 0.0f;
    if (float.IsNaN(this.moveVY) || float.IsInfinity(this.moveVY))
      this.moveVY = 0.0f;
    this.linealMovement = new Vector2(this.moveVX, this.moveVY);
  }

  public void MoveCircle(float angleIncrement, float angularSpeed)
  {
    this.angularMovement = this.FacingDirection * angularSpeed;
    this.state.facingAngle += angleIncrement * Time.deltaTime;
    this.state.LookAngle = this.state.facingAngle;
  }

  public void InitDamageColliders()
  {
    foreach (ColliderEvents damageCollider in this.damageColliders)
    {
      damageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      damageCollider.SetActive(false);
    }
    this.damageColliders[4].OnTriggerStayEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerStay);
  }

  public void DisposeDamageColliders()
  {
    foreach (ColliderEvents damageCollider in this.damageColliders)
    {
      damageCollider.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      damageCollider.SetActive(false);
    }
    this.damageColliders[4].OnTriggerStayEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerStay);
  }

  public bool HasObstacle(Vector2 direction, float distance, LayerMask layerMask)
  {
    return (UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, direction, distance, (int) layerMask).collider != (UnityEngine.Object) null;
  }

  public override void OnPartnerDeath(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnPartnerDeath(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.hasToPhase = true;
    this.health.invincible = true;
    this.OnPhaseStart = this.OnPhaseStart + new EnemyDual.PhaseAction(DoubleUIBossHUD.Instance.OnBoss1Phase);
  }

  public enum DorryState
  {
    Idle,
    Chase,
    Attack_1,
    Attack_2,
    Attack_3,
    Attack_4,
    Attack_5,
    Attack_6,
    Attack_7,
    Attack_8,
    Attack_9,
    Attack_10,
    Attack_11,
    Attack_12,
    Sync,
    Attack_Together,
    Phase,
  }

  public delegate void DorryStateChange(
    EnemyDorryMiniboss.DorryState NewState,
    EnemyDorryMiniboss.DorryState PrevState);
}
