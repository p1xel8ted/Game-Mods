// Decompiled with JetBrains decompiler
// Type: EnemyBrogyMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
public class EnemyBrogyMiniboss : EnemyDual
{
  public bool IsNG;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public GameObject targetObject;
  public EnemyBrogyMiniboss.BrogyState currentState;
  public EnemyBrogyMiniboss.BrogyStateChange onStateChange;
  [SerializeField]
  public float movementSpeed;
  [SerializeField]
  public float chaseSpeed;
  [SerializeField]
  public LayerMask obstaclesLayermask;
  [SerializeField]
  public float obstaclesDistanceCheck;
  [SerializeField]
  public float maxDistanceMove;
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
  public float prob_attack_5_attack_6;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float prob_attack_7_attack_8;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float prob_attack_5_attack_10;
  [SerializeField]
  public GameObject projectilePrefab;
  [SerializeField]
  public bool rageActive;
  [SerializeField]
  public ParticleSystem levitateVFX;
  [SerializeField]
  public float attack_2_delay;
  [SerializeField]
  public float attack_2_duration;
  [SerializeField]
  public float attack_3_delay;
  [SerializeField]
  public float attack_3_duration;
  [SerializeField]
  public int attack_3_wavesAmount;
  [SerializeField]
  public float attack_3_wavesRate;
  [SerializeField]
  public int attack_3_projectilesAmount;
  [SerializeField]
  public int attack_3_projectileSpeed;
  [SerializeField]
  public float attack_4_delay;
  [SerializeField]
  public float attack_4_duration;
  [SerializeField]
  public float attack_4_chargeSpeed;
  [SerializeField]
  public float attack_4_circleSpeed;
  [SerializeField]
  public float attack_4_circleRadius;
  [SerializeField]
  public float attack_5_delay;
  [SerializeField]
  public float attack_5_duration;
  [SerializeField]
  public float attack_5_rageDuration;
  [SerializeField]
  public float attack_5_trackingSpeed;
  [SerializeField]
  public float attack_5_length;
  [SerializeField]
  public GameObject attack_5_lineTrigger;
  [SerializeField]
  public GameObject attack_5_damageCollider;
  [SerializeField]
  [EventRef]
  public string BeamLoopSFX = string.Empty;
  public EventInstance beamLoop;
  [SerializeField]
  public float attack_6_delay;
  [SerializeField]
  public float attack_6_duration;
  [SerializeField]
  public float attack_6_projectileAngleRate;
  [SerializeField]
  public float attack_6_projectileRate;
  [SerializeField]
  public float attack_6_projectileSpeed;
  [SerializeField]
  public float attack_6_angleIncrement;
  [SerializeField]
  public float attack_6_angularSpeed;
  [SerializeField]
  public float attack_7_delay;
  [SerializeField]
  public float attack_7_duration;
  [SerializeField]
  public int attack_7_wavesAmount;
  [SerializeField]
  public float attack_7_wavesRate;
  [SerializeField]
  public int attack_7_projectilesAmount;
  [SerializeField]
  public int attack_7_projectileSpeed;
  [SerializeField]
  public float attack_7_extraAnglePerWave;
  [SerializeField]
  public float attack_8_minDistancePlayer;
  [SerializeField]
  public float attack_8_movementSpeed;
  [SerializeField]
  public int attack_8_projectileAmountPerCircle;
  [SerializeField]
  public int attack_8_circleRadius;
  [SerializeField]
  public int attack_8_projectileSpeed;
  [SerializeField]
  public float attack_9_projectilesAmount;
  [SerializeField]
  public float attack_9_projectileSpeed;
  [SerializeField]
  public float attack_9_randomAngle;
  [SerializeField]
  public int attack_9_wavesAmount;
  [SerializeField]
  public float attack_9_wavesRate;
  [SerializeField]
  public float attack_9_delay;
  [FormerlySerializedAs("attack_9_duration")]
  [SerializeField]
  public float attack_9_afterCastIdle;
  [SerializeField]
  public float attack_9_cooldown;
  [SerializeField]
  public float attack_9_rageCooldown;
  [SerializeField]
  public float attack_10_delay;
  [SerializeField]
  public float attack_10_duration;
  [SerializeField]
  public float attack_10_projectileAngleRate;
  [SerializeField]
  public float attack_10_projectileRate;
  [SerializeField]
  public float attack_10_projectileSpeed;
  [SerializeField]
  public float attack_10_angleIncrement;
  [SerializeField]
  public float attack_10_movementSpeed;
  [SerializeField]
  public float attack_10_angularSpeed;
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
  [SerializeField]
  public CircleCollider2D bossCollider;
  public Vector2 linealDirection;
  public Vector2 linealMovement;
  public Vector2 angularMovement;
  public Vector2 currentMovement;
  public Coroutine idleRoutine;
  public Coroutine moveRoutine;
  public Coroutine attack_2_Routine;
  public Coroutine attack_3_Routine;
  public Coroutine attack_4_Routine;
  public Coroutine attack_5_Routine;
  public Coroutine attack_6_Routine;
  public Coroutine attack_7_Routine;
  public Coroutine attack_8_Routine;
  public float attack_9_cooldoownTimestamp;
  public Coroutine attack_9_Routine;
  public Coroutine attack_10_Routine;
  public Coroutine sync_Routine;
  public Coroutine attack_together_Routine;
  public Coroutine phase_Routine;
  public Health EnemyHealth;

  public EnemyBrogyMiniboss.BrogyState CURRENT_BROGY_STATE
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
    this.onStateChange += new EnemyBrogyMiniboss.BrogyStateChange(this.OnStateChange);
    this.attack_9_cooldoownTimestamp = GameManager.GetInstance().CurrentTime;
    this.attackTogetherTimeStamp = GameManager.GetInstance().CurrentTime;
    this.attack_5_damageCollider.SetActive(false);
    this.attack_5_lineTrigger.SetActive(false);
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
    this.onStateChange -= new EnemyBrogyMiniboss.BrogyStateChange(this.OnStateChange);
    this.OnPhaseStart = this.OnPhaseStart - new EnemyDual.PhaseAction(DoubleUIBossHUD.Instance.OnBoss2Phase);
  }

  public override void FixedUpdate()
  {
    base.FixedUpdate();
    this.linealMovement *= Time.deltaTime * this.Spine.timeScale;
    this.angularMovement *= Time.deltaTime * this.Spine.timeScale;
    this.currentMovement = this.linealMovement + this.angularMovement;
    if (!this.HasObstacle(this.currentMovement.normalized, 0.5f, this.obstaclesLayermask))
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
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemyBrogyMiniboss.targetObject == (UnityEngine.Object) null)
    {
      enemyBrogyMiniboss.SetTarget();
      yield return (object) null;
    }
    enemyBrogyMiniboss.state.CURRENT_STATE = StateMachine.State.Dancing;
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Idle;
  }

  public IEnumerator IdleState()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    while ((double) (enemyBrogyMiniboss.idleTimer -= Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) > 0.0)
      yield return (object) null;
    while ((UnityEngine.Object) enemyBrogyMiniboss.targetObject == (UnityEngine.Object) null)
    {
      enemyBrogyMiniboss.SetTarget();
      yield return (object) null;
    }
    if (!enemyBrogyMiniboss.rageActive && enemyBrogyMiniboss.hasToPhase)
      enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Phase;
    else if (!enemyBrogyMiniboss.rageActive && enemyBrogyMiniboss.IsAttackTogetherReady)
    {
      enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Sync;
    }
    else
    {
      if ((double) GameManager.GetInstance().TimeSince(enemyBrogyMiniboss.attack_9_cooldoownTimestamp) > (enemyBrogyMiniboss.rageActive ? (double) enemyBrogyMiniboss.attack_9_rageCooldown : (double) enemyBrogyMiniboss.attack_9_cooldown))
        enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Attack_9;
      else if ((double) Vector3.Distance(enemyBrogyMiniboss.transform.position, enemyBrogyMiniboss.targetObject.transform.position) < (double) enemyBrogyMiniboss.closeAttackDistance)
      {
        enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Attack_2;
      }
      else
      {
        float num = UnityEngine.Random.value;
        enemyBrogyMiniboss.CURRENT_BROGY_STATE = !enemyBrogyMiniboss.rageActive ? ((double) num < 0.5 ? EnemyBrogyMiniboss.BrogyState.Attack_5 : EnemyBrogyMiniboss.BrogyState.Attack_6) : ((double) num < (double) enemyBrogyMiniboss.prob_attack_5_attack_10 ? EnemyBrogyMiniboss.BrogyState.Attack_5 : EnemyBrogyMiniboss.BrogyState.Attack_10);
      }
      yield return (object) null;
    }
  }

  public IEnumerator KeepDistanceState()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    int attemps = 0;
    Vector3 targetPosition = BiomeGenerator.GetRandomPositionInIsland();
    while ((double) Vector3.Distance(targetPosition, enemyBrogyMiniboss.targetObject.transform.position) < (double) enemyBrogyMiniboss.closeAttackDistance * 2.0 || !BiomeGenerator.PointWithinIsland(targetPosition, out Vector3 _))
    {
      targetPosition = BiomeGenerator.GetRandomPositionInIsland();
      ++attemps;
      yield return (object) null;
      if (attemps >= 100)
        break;
    }
    Vector3 closestPoint;
    if (!BiomeGenerator.PointWithinIsland(targetPosition, out closestPoint))
      targetPosition = closestPoint + -closestPoint.normalized * 2.5f;
    if ((double) Vector2.Distance((Vector2) enemyBrogyMiniboss.transform.position, (Vector2) targetPosition) > (double) enemyBrogyMiniboss.maxDistanceMove)
    {
      Vector3 vector3 = targetPosition - enemyBrogyMiniboss.transform.position;
      targetPosition = enemyBrogyMiniboss.transform.position + vector3.normalized * enemyBrogyMiniboss.maxDistanceMove;
    }
    Vector2 origin = (Vector2) enemyBrogyMiniboss.transform.position;
    while ((UnityEngine.Object) enemyBrogyMiniboss.targetObject != (UnityEngine.Object) null)
    {
      float num = 0.1f;
      if ((double) Vector3.Distance(enemyBrogyMiniboss.transform.position, targetPosition) < (double) enemyBrogyMiniboss.bossCollider.radius + (double) num)
      {
        enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Idle;
        yield break;
      }
      enemyBrogyMiniboss.FacePosition(targetPosition, (Vector3) origin);
      enemyBrogyMiniboss.Move(enemyBrogyMiniboss.movementSpeed);
      yield return (object) null;
    }
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "backstep-end", false);
    yield return (object) new WaitForSeconds(0.5f);
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_2()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
    enemyBrogyMiniboss.StartCoroutine((IEnumerator) enemyBrogyMiniboss.EnableDamageColliderTime(enemyBrogyMiniboss.damageColliders[0], enemyBrogyMiniboss.attack_2_delay));
    yield return (object) new WaitForSeconds(enemyBrogyMiniboss.attack_2_duration * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = enemyBrogyMiniboss.rageActive ? ((double) UnityEngine.Random.value >= (double) enemyBrogyMiniboss.prob_attack_7_attack_8 ? EnemyBrogyMiniboss.BrogyState.Attack_8 : EnemyBrogyMiniboss.BrogyState.Attack_7) : EnemyBrogyMiniboss.BrogyState.Attack_3;
  }

  public IEnumerator Attack_3()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.health.invincible = true;
    float delayTime = 0.0f;
    while ((double) (delayTime += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.attack_3_delay)
    {
      enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
      yield return (object) null;
    }
    int wavesSpawned = 0;
    float rateTime = 0.0f;
    while (wavesSpawned < enemyBrogyMiniboss.attack_3_wavesAmount)
    {
      if ((double) (rateTime += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) >= (double) enemyBrogyMiniboss.attack_3_wavesRate)
      {
        float num = 360f / (float) enemyBrogyMiniboss.attack_3_projectilesAmount;
        float angle = 0.0f;
        for (int index = 0; index < enemyBrogyMiniboss.attack_3_projectilesAmount; ++index)
        {
          enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, angle, (float) enemyBrogyMiniboss.attack_3_projectileSpeed);
          angle += num + (float) ((double) num / 2.0 * (double) wavesSpawned % 2.0);
        }
        rateTime = 0.0f;
        ++wavesSpawned;
      }
      yield return (object) null;
    }
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-3-end", false);
    enemyBrogyMiniboss.Spine.AnimationState.AddAnimation(0, enemyBrogyMiniboss.IdleAnim, true, 0.0f);
    yield return (object) new WaitForSeconds(1f * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.health.invincible = false;
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.KeepDistance;
  }

  public IEnumerator Attack_4()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
    yield return (object) new WaitForSeconds(enemyBrogyMiniboss.attack_4_delay * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-4-loop", true);
    enemyBrogyMiniboss.StartCoroutine((IEnumerator) enemyBrogyMiniboss.EnableDamageCollider(enemyBrogyMiniboss.damageColliders[4], 0.0f));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.attack_4_duration)
    {
      enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
      enemyBrogyMiniboss.Move(enemyBrogyMiniboss.attack_4_chargeSpeed);
      yield return (object) null;
    }
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, enemyBrogyMiniboss.IdleAnim, true);
    enemyBrogyMiniboss.DisableDamageCollider(enemyBrogyMiniboss.damageColliders[4]);
    yield return (object) new WaitForSeconds(1f * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Idle;
  }

  public IEnumerator Attack_5()
  {
    yield return (object) new WaitForSeconds(this.attack_5_delay * this.Spine.timeScale);
    float time = 0.0f;
    this.attack_5_lineTrigger.SetActive(true);
    this.attack_5_damageCollider.SetActive(true);
    this.attack_5_damageCollider.transform.position = this.targetObject.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 1.5f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.attack_5_duration)
    {
      this.CalculateBeamPosition(this.targetObject);
      yield return (object) null;
    }
    this.attack_5_lineTrigger.SetActive(false);
    this.attack_5_damageCollider.SetActive(false);
    this.Spine.AnimationState.SetAnimation(0, "attack-5-end", false);
    this.Spine.AnimationState.AddAnimation(0, this.IdleAnim, true, 0.0f);
    yield return (object) new WaitForSeconds(this.attack_5_delay * this.Spine.timeScale);
    this.CURRENT_BROGY_STATE = this.rageActive ? EnemyBrogyMiniboss.BrogyState.Attack_7 : EnemyBrogyMiniboss.BrogyState.KeepDistance;
  }

  public IEnumerator Attack_6()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
    yield return (object) new WaitForSeconds(enemyBrogyMiniboss.attack_6_delay * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-6-loop", true);
    enemyBrogyMiniboss.StartCoroutine((IEnumerator) enemyBrogyMiniboss.EnableDamageCollider(enemyBrogyMiniboss.damageColliders[1], 0.0f));
    float time = 0.0f;
    float projectileTime = 0.0f;
    float projectileAngleOffset = 0.0f;
    enemyBrogyMiniboss.levitateVFX.Play();
    while ((double) (time += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.attack_6_duration)
    {
      projectileTime += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale;
      if ((double) projectileTime >= (double) enemyBrogyMiniboss.attack_6_projectileRate)
      {
        enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, projectileAngleOffset, enemyBrogyMiniboss.attack_6_projectileSpeed);
        enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, projectileAngleOffset + 120f, enemyBrogyMiniboss.attack_6_projectileSpeed);
        enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, projectileAngleOffset - 120f, enemyBrogyMiniboss.attack_6_projectileSpeed);
        projectileAngleOffset += enemyBrogyMiniboss.attack_6_projectileAngleRate;
        projectileTime = 0.0f;
      }
      yield return (object) null;
    }
    enemyBrogyMiniboss.DisableDamageCollider(enemyBrogyMiniboss.damageColliders[1]);
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-6-end", false);
    enemyBrogyMiniboss.Spine.AnimationState.AddAnimation(0, enemyBrogyMiniboss.IdleAnim, true, 0.0f);
    enemyBrogyMiniboss.levitateVFX.Stop();
    yield return (object) new WaitForSeconds(1f * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.KeepDistance;
  }

  public IEnumerator Attack_7()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.health.invincible = true;
    float delayTime = 0.0f;
    while ((double) (delayTime += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.attack_7_delay)
    {
      enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
      yield return (object) null;
    }
    int wavesSpawned = 0;
    float rateTime = 0.0f;
    float extraAngleOffset = 0.0f;
    while (wavesSpawned < enemyBrogyMiniboss.attack_7_wavesAmount)
    {
      if ((double) (rateTime += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) >= (double) enemyBrogyMiniboss.attack_7_wavesRate)
      {
        float num = 360f / (float) enemyBrogyMiniboss.attack_7_projectilesAmount;
        float angle = 0.0f;
        for (int index = 0; index < enemyBrogyMiniboss.attack_7_projectilesAmount; ++index)
        {
          enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, angle, (float) enemyBrogyMiniboss.attack_7_projectileSpeed);
          angle += num + (float) ((double) num / 2.0 * (double) wavesSpawned % 2.0) + extraAngleOffset;
        }
        rateTime = 0.0f;
        extraAngleOffset += enemyBrogyMiniboss.IsNG ? enemyBrogyMiniboss.attack_7_extraAnglePerWave : 0.0f;
        ++wavesSpawned;
      }
      yield return (object) null;
    }
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-3-end", false);
    enemyBrogyMiniboss.Spine.AnimationState.AddAnimation(0, enemyBrogyMiniboss.IdleAnim, true, 0.0f);
    yield return (object) new WaitForSeconds(1f * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.health.invincible = false;
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.KeepDistance;
  }

  public IEnumerator Attack_8()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.health.invincible = true;
    float delayTime = 0.0f;
    while ((double) (delayTime += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.attack_3_delay)
    {
      enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
      yield return (object) null;
    }
    int attemps = 0;
    Vector3 targetPosition = BiomeGenerator.GetRandomPositionInIsland();
    while ((double) Vector3.Distance(targetPosition, enemyBrogyMiniboss.targetObject.transform.position) < (double) enemyBrogyMiniboss.attack_8_minDistancePlayer || !BiomeGenerator.PointWithinIsland(targetPosition, out Vector3 _))
    {
      targetPosition = BiomeGenerator.GetRandomPositionInIsland();
      ++attemps;
      yield return (object) null;
      if (attemps >= 100)
        break;
    }
    Vector3 closestPoint;
    if (!BiomeGenerator.PointWithinIsland(targetPosition, out closestPoint))
      targetPosition = closestPoint + -closestPoint.normalized * 2.5f;
    while ((double) Vector3.Distance(enemyBrogyMiniboss.transform.position, targetPosition) > 0.30000001192092896)
    {
      enemyBrogyMiniboss.FacePosition(targetPosition, enemyBrogyMiniboss.targetObject.transform.position);
      enemyBrogyMiniboss.Move(enemyBrogyMiniboss.attack_8_movementSpeed);
      yield return (object) null;
    }
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-3-end", false);
    enemyBrogyMiniboss.Spine.AnimationState.AddAnimation(0, enemyBrogyMiniboss.IdleAnim, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    enemyBrogyMiniboss.StartCoroutine((IEnumerator) enemyBrogyMiniboss.EnableDamageColliderTime(enemyBrogyMiniboss.damageColliders[1], 0.0f));
    for (int index1 = 0; index1 < 4; ++index1)
    {
      float num1 = 45f + (float) (90 * index1);
      Vector2 vector2 = new Vector2(Mathf.Cos((float) ((double) num1 * 3.1415927410125732 / 180.0)), Mathf.Sin((float) ((double) num1 * 3.1415927410125732 / 180.0))) * (float) enemyBrogyMiniboss.attack_8_circleRadius;
      Vector3 vector3_1 = enemyBrogyMiniboss.transform.position + (Vector3) vector2;
      float angle = 0.0f;
      float num2 = 360f / (float) enemyBrogyMiniboss.attack_8_projectileAmountPerCircle;
      for (int index2 = 0; index2 < enemyBrogyMiniboss.attack_8_projectileAmountPerCircle; ++index2)
      {
        if ((double) angle >= (double) num1 - 90.0 && (double) angle <= (double) num1 + 90.0)
        {
          Vector3 vector3_2 = vector3_1 + (Vector3) (new Vector2(Mathf.Cos((float) ((double) angle * 3.1415927410125732 / 180.0)), Mathf.Sin((float) ((double) angle * 3.1415927410125732 / 180.0))) * (float) enemyBrogyMiniboss.attack_8_circleRadius);
          if (BiomeGenerator.PointWithinIsland(vector3_2, out Vector3 _))
            enemyBrogyMiniboss.ShootProjectile((Vector2) vector3_2, angle, (float) enemyBrogyMiniboss.attack_8_projectileSpeed);
        }
        angle += num2;
      }
    }
    enemyBrogyMiniboss.health.invincible = false;
    yield return (object) new WaitForSeconds(1f * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.KeepDistance;
  }

  public IEnumerator Attack_9()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    float timeStamp = GameManager.GetInstance().CurrentTime;
    Vector2 position1 = (Vector2) enemyBrogyMiniboss.targetObject.transform.position;
    while ((double) GameManager.GetInstance().TimeSince(timeStamp) < (double) enemyBrogyMiniboss.attack_9_delay)
    {
      Vector2 position2 = (Vector2) enemyBrogyMiniboss.targetObject.transform.position;
      enemyBrogyMiniboss.FacePosition((Vector3) position2, enemyBrogyMiniboss.targetObject.transform.position);
      yield return (object) null;
    }
    int wavesSpawned = 0;
    float rateTime = 0.0f;
    while (wavesSpawned < enemyBrogyMiniboss.attack_9_wavesAmount)
    {
      if ((double) (rateTime += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) >= (double) enemyBrogyMiniboss.attack_9_wavesRate)
      {
        float num1 = 360f / enemyBrogyMiniboss.attack_9_projectilesAmount;
        float num2 = 0.0f;
        float num3 = UnityEngine.Random.Range(-enemyBrogyMiniboss.attack_9_randomAngle, enemyBrogyMiniboss.attack_9_randomAngle);
        for (int index = 0; (double) index < (double) enemyBrogyMiniboss.attack_9_projectilesAmount; ++index)
        {
          enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, num2 + num3, enemyBrogyMiniboss.attack_9_projectileSpeed);
          num2 += num1;
          num3 = UnityEngine.Random.Range(-enemyBrogyMiniboss.attack_9_randomAngle, enemyBrogyMiniboss.attack_9_randomAngle);
        }
        rateTime = 0.0f;
        ++wavesSpawned;
      }
      yield return (object) null;
    }
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-3-end", false);
    enemyBrogyMiniboss.Spine.AnimationState.AddAnimation(0, enemyBrogyMiniboss.IdleAnim, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyBrogyMiniboss.attack_9_afterCastIdle * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.attack_9_cooldoownTimestamp = GameManager.GetInstance().CurrentTime;
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.KeepDistance;
  }

  public IEnumerator Attack_10()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
    yield return (object) new WaitForSeconds(enemyBrogyMiniboss.attack_10_delay * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-6-loop", true);
    enemyBrogyMiniboss.StartCoroutine((IEnumerator) enemyBrogyMiniboss.EnableDamageCollider(enemyBrogyMiniboss.damageColliders[1], 0.0f));
    enemyBrogyMiniboss.levitateVFX.Play();
    float time = 0.0f;
    float projectileTime = 0.0f;
    float projectileAngleOffset = 0.0f;
    enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
    enemyBrogyMiniboss.linealDirection = enemyBrogyMiniboss.FacingDirection;
    while ((double) (time += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.attack_10_duration)
    {
      enemyBrogyMiniboss.MoveCircle(enemyBrogyMiniboss.attack_10_angleIncrement, enemyBrogyMiniboss.attack_10_angularSpeed);
      projectileTime += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale;
      if ((double) projectileTime >= (double) enemyBrogyMiniboss.attack_10_projectileRate)
      {
        enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, projectileAngleOffset, enemyBrogyMiniboss.attack_10_projectileSpeed);
        enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, projectileAngleOffset + 120f, enemyBrogyMiniboss.attack_10_projectileSpeed);
        enemyBrogyMiniboss.ShootProjectile((Vector2) enemyBrogyMiniboss.transform.position, projectileAngleOffset - 120f, enemyBrogyMiniboss.attack_10_projectileSpeed);
        projectileAngleOffset += enemyBrogyMiniboss.attack_10_projectileAngleRate;
        projectileTime = 0.0f;
      }
      yield return (object) null;
    }
    enemyBrogyMiniboss.DisableDamageCollider(enemyBrogyMiniboss.damageColliders[1]);
    enemyBrogyMiniboss.levitateVFX.Stop();
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, enemyBrogyMiniboss.IdleAnim, true);
    yield return (object) new WaitForSeconds(1f * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Attack_7;
  }

  public IEnumerator Sync()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    float time = 0.0f;
    EnemyDorryMiniboss dorry = enemyBrogyMiniboss.partner as EnemyDorryMiniboss;
    while ((double) (time += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.syncMaxTime)
    {
      if (dorry.CURRENT_DORRY_STATE == EnemyDorryMiniboss.DorryState.Sync)
      {
        dorry.CURRENT_DORRY_STATE = EnemyDorryMiniboss.DorryState.Attack_Together;
        enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Attack_Together;
        yield break;
      }
      if (!enemyBrogyMiniboss.hasToPhase)
        yield return (object) null;
      else
        break;
    }
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Idle;
    yield return (object) null;
  }

  public IEnumerator Attack_Together()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
    yield return (object) new WaitForSeconds(enemyBrogyMiniboss.attack_together_delay * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-6-loop", true);
    enemyBrogyMiniboss.levitateVFX.Play();
    enemyBrogyMiniboss.StartCoroutine((IEnumerator) enemyBrogyMiniboss.EnableDamageCollider(enemyBrogyMiniboss.damageColliders[1], 0.0f));
    float time = 0.0f;
    enemyBrogyMiniboss.FacePosition(enemyBrogyMiniboss.targetObject.transform.position, enemyBrogyMiniboss.targetObject.transform.position);
    enemyBrogyMiniboss.linealDirection = enemyBrogyMiniboss.FacingDirection;
    while ((double) (time += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.attack_together_duration)
    {
      enemyBrogyMiniboss.Move(enemyBrogyMiniboss.attack_together_chargeSpeed, enemyBrogyMiniboss.linealDirection);
      enemyBrogyMiniboss.MoveCircle(enemyBrogyMiniboss.attack_together_angleIncrement, enemyBrogyMiniboss.attack_together_angularSpeed);
      yield return (object) null;
    }
    enemyBrogyMiniboss.DisableDamageCollider(enemyBrogyMiniboss.damageColliders[1]);
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "attack-6-end", false);
    enemyBrogyMiniboss.Spine.AnimationState.AddAnimation(0, enemyBrogyMiniboss.IdleAnim, true, 0.0f);
    enemyBrogyMiniboss.levitateVFX.Stop();
    yield return (object) new WaitForSeconds(1f * enemyBrogyMiniboss.Spine.timeScale);
    enemyBrogyMiniboss.attackTogetherTimeStamp = GameManager.GetInstance().CurrentTime;
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.Idle;
  }

  public IEnumerator Phase()
  {
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "rage-a", false);
    yield return (object) new WaitForSeconds(enemyBrogyMiniboss.phaseDelay);
    enemyBrogyMiniboss.Spine.Skeleton.SetSkin(enemyBrogyMiniboss.IsNG ? "rage-juiced" : "rage");
    enemyBrogyMiniboss.Spine.Skeleton.SetSlotsToSetupPose();
    enemyBrogyMiniboss.Spine.Skeleton.SetBonesToSetupPose();
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, "rage-b", false);
    enemyBrogyMiniboss.Spine.AnimationState.AddAnimation(0, "idle-rage", false, 0.0f);
    float healingAmount = (enemyBrogyMiniboss.health.totalHP - enemyBrogyMiniboss.health.HP) / enemyBrogyMiniboss.phaseDuration;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBrogyMiniboss.Spine.timeScale) < (double) enemyBrogyMiniboss.phaseDuration)
    {
      enemyBrogyMiniboss.health.Heal(healingAmount * Time.deltaTime);
      DoubleUIBossHUD.Instance.ForceHealthAmount2(enemyBrogyMiniboss.health.HP / enemyBrogyMiniboss.health.totalHP);
      yield return (object) null;
    }
    enemyBrogyMiniboss.health.totalHP = enemyBrogyMiniboss.phaseHp;
    enemyBrogyMiniboss.health.HP = enemyBrogyMiniboss.health.totalHP;
    DoubleUIBossHUD.Instance.ForceHealthAmount2(1f);
    enemyBrogyMiniboss.health.invincible = false;
    enemyBrogyMiniboss.rageActive = true;
    enemyBrogyMiniboss.hasToPhase = false;
    enemyBrogyMiniboss.CURRENT_BROGY_STATE = EnemyBrogyMiniboss.BrogyState.KeepDistance;
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
    EnemyBrogyMiniboss.BrogyState newState,
    EnemyBrogyMiniboss.BrogyState prevState)
  {
    switch (newState)
    {
      case EnemyBrogyMiniboss.BrogyState.Idle:
        this.idleTimer = this.rageActive ? this.idleTimePhase2 : this.idleTimePhase1;
        this.Spine.AnimationState.SetAnimation(0, this.IdleAnim, true);
        if (this.idleRoutine != null)
          this.StopCoroutine(this.idleRoutine);
        this.idleRoutine = this.StartCoroutine((IEnumerator) this.IdleState());
        break;
      case EnemyBrogyMiniboss.BrogyState.KeepDistance:
        this.speed = this.movementSpeed * this.SpeedMultiplier;
        this.Spine.AnimationState.SetAnimation(0, "backstep-start", true);
        this.Spine.AnimationState.AddAnimation(0, "backstep-loop", true, 0.0f);
        if (this.moveRoutine != null)
          this.StopCoroutine(this.moveRoutine);
        this.moveRoutine = this.StartCoroutine((IEnumerator) this.KeepDistanceState());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_2:
        this.Spine.AnimationState.SetAnimation(0, "attack-2", false);
        this.Spine.AnimationState.AddAnimation(0, this.IdleAnim, true, 0.0f);
        if (this.attack_2_Routine != null)
          this.StopCoroutine(this.attack_2_Routine);
        this.attack_2_Routine = this.StartCoroutine((IEnumerator) this.Attack_2());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_3:
        this.Spine.AnimationState.SetAnimation(0, "attack-3-start", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-3-loop", true, 0.0f);
        if (this.attack_3_Routine != null)
          this.StopCoroutine(this.attack_3_Routine);
        this.attack_3_Routine = this.StartCoroutine((IEnumerator) this.Attack_3());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_4:
        this.Spine.AnimationState.SetAnimation(0, "attack-4-charge", false);
        if (this.attack_4_Routine != null)
          this.StopCoroutine(this.attack_4_Routine);
        this.attack_4_Routine = this.StartCoroutine((IEnumerator) this.Attack_4());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_5:
        this.Spine.AnimationState.SetAnimation(0, "attack-5-charge", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-5-loop", true, 0.0f);
        if (this.attack_5_Routine != null)
          this.StopCoroutine(this.attack_5_Routine);
        this.attack_5_Routine = this.StartCoroutine((IEnumerator) this.Attack_5());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_6:
        this.Spine.AnimationState.SetAnimation(0, "attack-6-start", false);
        if (this.attack_6_Routine != null)
          this.StopCoroutine(this.attack_6_Routine);
        this.attack_6_Routine = this.StartCoroutine((IEnumerator) this.Attack_6());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_7:
        this.Spine.AnimationState.SetAnimation(0, "attack-3-start", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-3-loop", true, 0.0f);
        if (this.attack_7_Routine != null)
          this.StopCoroutine(this.attack_7_Routine);
        this.attack_7_Routine = this.StartCoroutine((IEnumerator) this.Attack_7());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_8:
        this.Spine.AnimationState.SetAnimation(0, "attack-3-start", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-3-loop", true, 0.0f);
        if (this.attack_8_Routine != null)
          this.StopCoroutine(this.attack_8_Routine);
        this.attack_8_Routine = this.StartCoroutine((IEnumerator) this.Attack_8());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_9:
        this.Spine.AnimationState.SetAnimation(0, "attack-3-start", false);
        this.Spine.AnimationState.AddAnimation(0, "attack-3-loop", true, 0.0f);
        if (this.attack_9_Routine != null)
          this.StopCoroutine(this.attack_9_Routine);
        this.attack_9_Routine = this.StartCoroutine((IEnumerator) this.Attack_9());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_10:
        this.Spine.AnimationState.SetAnimation(0, "attack-6-start", false);
        if (this.attack_10_Routine != null)
          this.StopCoroutine(this.attack_10_Routine);
        this.attack_10_Routine = this.StartCoroutine((IEnumerator) this.Attack_10());
        break;
      case EnemyBrogyMiniboss.BrogyState.Sync:
        this.Spine.AnimationState.SetAnimation(0, this.IdleAnim, true);
        if (this.sync_Routine != null)
          this.StopCoroutine(this.sync_Routine);
        this.sync_Routine = this.StartCoroutine((IEnumerator) this.Sync());
        break;
      case EnemyBrogyMiniboss.BrogyState.Attack_Together:
        this.Spine.AnimationState.SetAnimation(0, "attack-6-start", false);
        if (this.sync_Routine != null)
          this.StopCoroutine(this.sync_Routine);
        if (this.attack_together_Routine != null)
          this.StopCoroutine(this.attack_together_Routine);
        this.attack_together_Routine = this.StartCoroutine((IEnumerator) this.Attack_Together());
        break;
      case EnemyBrogyMiniboss.BrogyState.Phase:
        this.Spine.AnimationState.SetAnimation(0, this.IdleAnim, true);
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
    EnemyBrogyMiniboss enemyBrogyMiniboss = this;
    enemyBrogyMiniboss.ClearPaths();
    Vector3 offset = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      enemyBrogyMiniboss.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 vector3 = (PlayerFarming.Instance.transform.position + offset) with
      {
        z = 0.0f
      };
      enemyBrogyMiniboss.transform.position = vector3;
      yield return (object) null;
    }
    enemyBrogyMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyBrogyMiniboss.Spine.AnimationState.SetAnimation(0, enemyBrogyMiniboss.IdleAnim, true);
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
    this.damageColliders[1].OnTriggerStayEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerStay);
  }

  public void DisposeDamageColliders()
  {
    foreach (ColliderEvents damageCollider in this.damageColliders)
    {
      damageCollider.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      damageCollider.SetActive(false);
    }
    this.damageColliders[1].OnTriggerStayEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerStay);
  }

  public void CalculateBeamPosition(GameObject currentTarget)
  {
    Vector3 vector3_1 = Vector3.Lerp(this.attack_5_damageCollider.transform.position, currentTarget.transform.position, Time.deltaTime * this.attack_5_trackingSpeed);
    this.attack_5_damageCollider.transform.position = vector3_1;
    Vector3.Lerp(currentTarget.transform.position, currentTarget.transform.position, this.attack_5_trackingSpeed * Time.deltaTime);
    Vector3 vector3_2 = this.transform.position + Vector3.back * 2f + (vector3_1 - this.transform.position) / 2f;
    float num = Vector3.Distance(this.transform.position, vector3_1);
    this.attack_5_lineTrigger.transform.position = vector3_2;
    this.attack_5_lineTrigger.transform.LookAt(vector3_1, Vector3.forward);
    this.attack_5_lineTrigger.transform.Rotate(-90f, 0.0f, 90f);
    this.attack_5_lineTrigger.transform.localScale = this.attack_5_lineTrigger.transform.localScale with
    {
      x = num * this.attack_5_length
    };
    BiomeConstants.Instance.EmitHammerEffects(vector3_1, 0.5f, 1f, 0.1f);
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
    this.OnPhaseStart = this.OnPhaseStart + new EnemyDual.PhaseAction(DoubleUIBossHUD.Instance.OnBoss2Phase);
  }

  public enum BrogyState
  {
    Idle,
    KeepDistance,
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
    Sync,
    Attack_Together,
    Phase,
  }

  public delegate void BrogyStateChange(
    EnemyBrogyMiniboss.BrogyState NewState,
    EnemyBrogyMiniboss.BrogyState PrevState);
}
