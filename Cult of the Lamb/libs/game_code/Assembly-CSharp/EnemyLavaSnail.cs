// Decompiled with JetBrains decompiler
// Type: EnemyLavaSnail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyLavaSnail : UnitObject
{
  public static List<EnemyLavaSnail> Snails = new List<EnemyLavaSnail>();
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  public bool LoopSignPostAttackAnimation = true;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ChargeAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FallAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FuseSmallAnimation;
  public SpriteRenderer ShadowSpriteRenderer;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public float KnockbackModifier = 1f;
  public int NumberOfAttacks = 1;
  public float AttackForceModifier = 1f;
  public bool CounterAttack;
  public bool SlamAttack;
  public bool CanBeInterrupted = true;
  public bool AttackTowardsPlayer;
  public float DamageColliderDuration = -1f;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/warning";
  [EventRef]
  public string MoveSFX = "event:/dlc/dungeon06/enemy/lavasnail_small/move";
  [EventRef]
  public string CombineSFX = "event:/dlc/dungeon06/enemy/lavasnail_small/combine_into_large";
  public EventInstance combineInstanceSFX;
  public float RandomDirection;
  [SerializeField]
  public float searchDistance = 1f;
  [SerializeField]
  public float mergeDistance = 0.5f;
  [SerializeField]
  public float mergeDuration = 1f;
  [SerializeField]
  public EnemyLavaSnailBig mergeResult;
  public EnemyLavaSnail mergeTarget;
  public bool merging;
  public int pathPositionIndex;
  public static List<EnemyLavaSnail> MergingSnails = new List<EnemyLavaSnail>();
  public float AttackDelayTime;
  public bool Attacking;
  public bool IsStunned;
  [SerializeField]
  public float attackForce = 2500f;
  [HideInInspector]
  public float AttackDelay;
  public float AttackDuration = 1f;
  public float SignPostAttackDuration = 0.5f;
  public bool DisableKnockback;
  public float Angle;
  public Vector3 Force;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public Vector2 timeToChangeTargetRange = new Vector2(10f, 15f);
  public float newTargetTimestamp;
  public float timeToNewTarget;
  public float frequency = 4f;
  public float amplitude = 1f;
  public Vector3 cyclePos;
  public Vector3 cycleTarget;
  public Vector3 currentDir;
  public Vector3 desiredDir;
  public Vector3 movementAxis;
  [SerializeField]
  public LayerMask obstacleLayermask;
  [SerializeField]
  public AreaBurnTick burnTick;
  [SerializeField]
  public float burnTickInterval = 1f;
  [SerializeField]
  public float burnTickDamage = 1f;
  [SerializeField]
  public float burnDamageDuration = 1f;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyLavaSnail.Snails.Add(this);
    this.burnTick.Initialize();
    this.RandomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.newTargetTimestamp = (bool) (UnityEngine.Object) GameManager.GetInstance() ? GameManager.GetInstance().CurrentTime : Time.time;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.timeToNewTarget = 0.0f;
    this.burnTick.EnableDamage(this.burnTickDamage, this.burnTickInterval, this.burnDamageDuration);
    if (GameManager.RoomActive)
    {
      this.Attacking = false;
      this.IsStunned = false;
      this.health.invincible = false;
      foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.StartCoroutine((IEnumerator) this.WanderingRoutine());
    }
    else
      this.StartCoroutine((IEnumerator) this.WanderingRoutine());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyLavaSnail.Snails.Remove(this);
    this.burnTick.Cleanup();
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
    if (!this.merging)
      return;
    this.mergeTarget = (EnemyLavaSnail) null;
    EnemyLavaSnail.MergingSnails.Remove(this);
    this.merging = false;
  }

  public virtual IEnumerator WanderingRoutine()
  {
    EnemyLavaSnail enemyLavaSnail = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while ((double) enemyLavaSnail.Spine.timeScale != 9.9999997473787516E-05)
      {
        if ((double) Vector2.Distance((Vector2) enemyLavaSnail.transform.position, (Vector2) enemyLavaSnail.cycleTarget) <= (double) enemyLavaSnail.StoppingDistance || (double) GameManager.GetInstance().TimeSince(enemyLavaSnail.newTargetTimestamp) >= (double) enemyLavaSnail.timeToNewTarget)
          enemyLavaSnail.GetNewTargetPosition();
        if (enemyLavaSnail.MovingAnimation != "")
        {
          if (enemyLavaSnail.state.CURRENT_STATE == StateMachine.State.Moving && enemyLavaSnail.Spine.AnimationName != enemyLavaSnail.MovingAnimation)
            enemyLavaSnail.Spine.AnimationState.SetAnimation(0, enemyLavaSnail.MovingAnimation, true);
          if (enemyLavaSnail.state.CURRENT_STATE == StateMachine.State.Idle && enemyLavaSnail.Spine.AnimationName != enemyLavaSnail.IdleAnimation)
            enemyLavaSnail.Spine.AnimationState.SetAnimation(0, enemyLavaSnail.IdleAnimation, true);
        }
        if (enemyLavaSnail.ShouldMerge() && (double) UnityEngine.Random.value < 0.10000000149011612)
          enemyLavaSnail.StartMerge(true);
        else if (enemyLavaSnail.ShouldAttack())
          enemyLavaSnail.StartCoroutine((IEnumerator) enemyLavaSnail.AttackRoutine());
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public bool ShouldMerge()
  {
    return !this.merging && (UnityEngine.Object) this.mergeTarget != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, this.mergeTarget.transform.position) < (double) this.mergeDistance && !EnemyLavaSnail.MergingSnails.Contains(this.mergeTarget) && !EnemyLavaSnail.MergingSnails.Contains(this);
  }

  public bool IsMergeTargetClose()
  {
    EnemyLavaSnail closestMergeTarget = this.GetClosestMergeTarget();
    if ((UnityEngine.Object) closestMergeTarget != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, closestMergeTarget.transform.position) < (double) this.searchDistance)
    {
      this.mergeTarget = closestMergeTarget;
      return true;
    }
    this.mergeTarget = (EnemyLavaSnail) null;
    return false;
  }

  public EnemyLavaSnail GetClosestMergeTarget()
  {
    EnemyLavaSnail closestMergeTarget = (EnemyLavaSnail) null;
    float num1 = float.MaxValue;
    foreach (EnemyLavaSnail snail in EnemyLavaSnail.Snails)
    {
      if (!((UnityEngine.Object) snail == (UnityEngine.Object) this))
      {
        float num2 = Vector3.Distance(this.transform.position, snail.transform.position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          closestMergeTarget = snail;
        }
      }
    }
    return closestMergeTarget;
  }

  public void StartMerge(bool initiatedMerge)
  {
    EnemyLavaSnail.MergingSnails.Add(this);
    this.merging = true;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StopAllCoroutines();
    if (initiatedMerge)
      this.mergeTarget.StartMerge(false);
    this.StartCoroutine((IEnumerator) this.MergeRoutine(initiatedMerge));
  }

  public IEnumerator MergeRoutine(bool initiatedMerge)
  {
    EnemyLavaSnail enemyLavaSnail = this;
    float Progress = 0.0f;
    float Duration = enemyLavaSnail.mergeDuration;
    if (initiatedMerge && !string.IsNullOrEmpty(enemyLavaSnail.CombineSFX))
      enemyLavaSnail.combineInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyLavaSnail.CombineSFX, enemyLavaSnail.transform);
    enemyLavaSnail.Spine.AnimationState.SetAnimation(0, enemyLavaSnail.FuseSmallAnimation, false);
    while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemyLavaSnail.Spine.timeScale)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnail.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnail.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    if (initiatedMerge)
    {
      EnemyLavaSnailBig enemyLavaSnailBig = ObjectPool.Spawn<EnemyLavaSnailBig>(enemyLavaSnail.mergeResult, enemyLavaSnail.transform.parent, enemyLavaSnail.transform.position, Quaternion.identity);
      enemyLavaSnailBig.transform.position = enemyLavaSnail.transform.position;
      enemyLavaSnailBig.HasMerged = true;
      Health component = enemyLavaSnailBig.GetComponent<Health>();
      if ((bool) (UnityEngine.Object) component)
        Interaction_Chest.Instance?.AddEnemy(component);
      if (enemyLavaSnail.HasModifier)
        enemyLavaSnailBig.ForceSetModifier(enemyLavaSnail.modifier.Modifier);
      else if (enemyLavaSnail.mergeTarget.HasModifier)
        enemyLavaSnailBig.ForceSetModifier(enemyLavaSnail.mergeTarget.modifier.Modifier);
    }
    Interaction_Chest.Instance?.Enemies.Remove(enemyLavaSnail.health);
    Health.team2.Remove(enemyLavaSnail.health);
    EnemyLavaSnail.MergingSnails.Remove(enemyLavaSnail);
    UnityEngine.Object.Destroy((UnityEngine.Object) enemyLavaSnail.gameObject);
  }

  public void StopMerging()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.combineInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    if (!this.merging)
      return;
    this.StopAllCoroutines();
    if ((UnityEngine.Object) this.mergeTarget != (UnityEngine.Object) null)
    {
      this.mergeTarget.mergeTarget = (EnemyLavaSnail) null;
      this.mergeTarget.StopMerging();
    }
    this.mergeTarget = (EnemyLavaSnail) null;
    EnemyLavaSnail.MergingSnails.Remove(this);
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    if ((double) this.health.HP <= 0.0)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.timeToNewTarget = 0.0f;
    this.StartCoroutine((IEnumerator) this.WanderingRoutine());
  }

  public virtual bool ShouldAttack()
  {
    Health closestTarget = this.GetClosestTarget();
    return (UnityEngine.Object) closestTarget != (UnityEngine.Object) null && (double) (this.AttackDelay -= Time.deltaTime) < 0.0 && !this.Attacking && (double) Vector3.Distance(this.transform.position, closestTarget.transform.position) < (double) this.VisionRange && GameManager.RoomActive;
  }

  public virtual IEnumerator AttackRoutine()
  {
    EnemyLavaSnail enemyLavaSnail = this;
    enemyLavaSnail.Attacking = true;
    enemyLavaSnail.ClearPaths();
    int CurrentAttack = 0;
    float time = 0.0f;
    while (++CurrentAttack <= enemyLavaSnail.NumberOfAttacks)
    {
      enemyLavaSnail.Spine.AnimationState.SetAnimation(0, enemyLavaSnail.SignPostAttackAnimation, enemyLavaSnail.LoopSignPostAttackAnimation);
      enemyLavaSnail.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", enemyLavaSnail.transform.position);
      if ((UnityEngine.Object) enemyLavaSnail.GetClosestTarget() != (UnityEngine.Object) null)
      {
        enemyLavaSnail.state.LookAngle = Utils.GetAngle(enemyLavaSnail.transform.position, enemyLavaSnail.GetClosestTarget().transform.position);
        enemyLavaSnail.state.facingAngle = enemyLavaSnail.state.LookAngle;
      }
      float Progress = 0.0f;
      float Duration = enemyLavaSnail.SignPostAttackDuration;
      while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemyLavaSnail.Spine.timeScale)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnail.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(Progress / Duration);
        yield return (object) null;
      }
      foreach (SimpleSpineFlash simpleSpineFlash in enemyLavaSnail.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      if (enemyLavaSnail.AttackTowardsPlayer)
      {
        if ((UnityEngine.Object) enemyLavaSnail.GetClosestTarget() != (UnityEngine.Object) null)
        {
          enemyLavaSnail.state.LookAngle = Utils.GetAngle(enemyLavaSnail.transform.position, enemyLavaSnail.GetClosestTarget().transform.position);
          enemyLavaSnail.state.facingAngle = enemyLavaSnail.state.LookAngle;
        }
        enemyLavaSnail.Force = (Vector3) (new Vector2(enemyLavaSnail.attackForce * Mathf.Cos(enemyLavaSnail.state.LookAngle * ((float) Math.PI / 180f)), enemyLavaSnail.attackForce * Mathf.Sin(enemyLavaSnail.state.LookAngle * ((float) Math.PI / 180f))) * enemyLavaSnail.AttackForceModifier);
        enemyLavaSnail.rb.AddForce((Vector2) enemyLavaSnail.Force);
      }
      else
      {
        enemyLavaSnail.DisableForces = true;
        enemyLavaSnail.Force = (Vector3) (new Vector2(enemyLavaSnail.attackForce * Mathf.Cos(enemyLavaSnail.state.LookAngle * ((float) Math.PI / 180f)), enemyLavaSnail.attackForce * Mathf.Sin(enemyLavaSnail.state.LookAngle * ((float) Math.PI / 180f))) * enemyLavaSnail.AttackForceModifier);
        enemyLavaSnail.rb.AddForce((Vector2) enemyLavaSnail.Force);
      }
      enemyLavaSnail.damageColliderEvents.SetActive(true);
      if (!string.IsNullOrEmpty(enemyLavaSnail.AttackVO))
        AudioManager.Instance.PlayOneShot(enemyLavaSnail.AttackVO, enemyLavaSnail.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", enemyLavaSnail.transform.position);
      enemyLavaSnail.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
      enemyLavaSnail.Spine.AnimationState.SetAnimation(0, enemyLavaSnail.AttackAnimation, false);
      enemyLavaSnail.Spine.AnimationState.AddAnimation(0, enemyLavaSnail.IdleAnimation, true, 0.0f);
      if ((double) enemyLavaSnail.DamageColliderDuration != -1.0)
        enemyLavaSnail.StartCoroutine((IEnumerator) enemyLavaSnail.EnableCollider(enemyLavaSnail.DamageColliderDuration));
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyLavaSnail.Spine.timeScale) < (double) enemyLavaSnail.AttackDuration * 0.699999988079071)
        yield return (object) null;
      enemyLavaSnail.damageColliderEvents.SetActive(false);
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnail.Spine.timeScale) < (double) enemyLavaSnail.AttackDuration * 0.30000001192092896)
      yield return (object) null;
    enemyLavaSnail.DisableForces = false;
    enemyLavaSnail.GetNewTargetPosition();
    enemyLavaSnail.AttackDelay = enemyLavaSnail.AttackDelayTime;
    enemyLavaSnail.Attacking = false;
  }

  public IEnumerator EnableCollider(float dur)
  {
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) dur)
      yield return (object) null;
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    this.StopMerging();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
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
    if (this.merging)
      return;
    if (!this.DisableKnockback)
      this.damageColliderEvents.SetActive(false);
    if (this.CanBeInterrupted && AttackType != Health.AttackTypes.NoReaction)
    {
      this.StopAllCoroutines();
      this.DisableForces = false;
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && AttackType != Health.AttackTypes.NoReaction && !this.DisableKnockback && this.CanBeInterrupted)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyLavaSnail enemyLavaSnail = this;
    enemyLavaSnail.DisableForces = true;
    enemyLavaSnail.Angle = Utils.GetAngle(Attacker.transform.position, enemyLavaSnail.transform.position) * ((float) Math.PI / 180f);
    enemyLavaSnail.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyLavaSnail.Angle), 1500f * Mathf.Sin(enemyLavaSnail.Angle)) * enemyLavaSnail.KnockbackModifier);
    enemyLavaSnail.rb.AddForce((Vector2) enemyLavaSnail.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnail.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLavaSnail.DisableForces = false;
  }

  public IEnumerator ApplyForceRoutine(Vector3 forcePosition)
  {
    EnemyLavaSnail enemyLavaSnail = this;
    enemyLavaSnail.DisableForces = true;
    enemyLavaSnail.Angle = Utils.GetAngle(forcePosition, enemyLavaSnail.transform.position) * ((float) Math.PI / 180f);
    enemyLavaSnail.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyLavaSnail.Angle), 1500f * Mathf.Sin(enemyLavaSnail.Angle)) * enemyLavaSnail.KnockbackModifier);
    enemyLavaSnail.rb.AddForce((Vector2) enemyLavaSnail.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnail.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLavaSnail.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemyLavaSnail enemyLavaSnail = this;
    enemyLavaSnail.damageColliderEvents.SetActive(false);
    enemyLavaSnail.Attacking = false;
    enemyLavaSnail.ClearPaths();
    enemyLavaSnail.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyLavaSnail.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyLavaSnail.Spine.AnimationState.SetAnimation(0, enemyLavaSnail.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLavaSnail.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLavaSnail.DisableForces = false;
    enemyLavaSnail.timeToNewTarget = 1f;
    enemyLavaSnail.StartCoroutine((IEnumerator) enemyLavaSnail.WanderingRoutine());
    if (enemyLavaSnail.CounterAttack)
      enemyLavaSnail.StartCoroutine((IEnumerator) enemyLavaSnail.AttackRoutine());
  }

  public void GetNewTargetPosition()
  {
    this.newTargetTimestamp = GameManager.GetInstance().CurrentTime;
    this.timeToNewTarget = UnityEngine.Random.Range(this.timeToChangeTargetRange.x, this.timeToChangeTargetRange.y);
    this.RandomDirection = UnityEngine.Random.Range(0.0f, 360f) * ((float) Math.PI / 180f);
    Vector3 positionInIsland = BiomeGenerator.GetRandomPositionInIsland();
    int num = 100;
    while ((double) Vector2.Distance((Vector2) positionInIsland, (Vector2) this.transform.position) <= 4.0)
    {
      positionInIsland = BiomeGenerator.GetRandomPositionInIsland();
      if (--num <= 0)
        break;
    }
    this.CalculateMovement(positionInIsland);
  }

  public void CalculateMovement(Vector3 pos)
  {
    this.state.CURRENT_STATE = StateMachine.State.Moving;
    this.cycleTarget = pos;
    this.cyclePos = this.transform.position;
    this.currentDir = this.desiredDir;
    this.movementAxis = (Vector3) Vector2.Perpendicular((Vector2) this.currentDir);
    this.desiredDir = (this.cycleTarget - this.cyclePos).normalized;
  }

  public override void Update()
  {
    float num = this.UseDeltaTime ? GameManager.DeltaTime : GameManager.UnscaledDeltaTime;
    if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
    {
      this.speed += (float) (((double) this.maxSpeed * (double) this.SpeedMultiplier - (double) this.speed) / 7.0) * num;
      if (this.IsMergeTargetClose())
      {
        Vector3 position = this.mergeTarget.transform.position;
        this.SetMergePath(position);
        if (this.pathToFollow != null && this.pathToFollow.Count > this.pathPositionIndex)
        {
          this.HandleMergePathTargetPosition();
        }
        else
        {
          this.ClearMergePath();
          this.LookAtAngle(Utils.GetAngle(this.transform.position, position));
        }
      }
      else
      {
        this.ClearMergePath();
        if (this.CheckObstacle((Vector3) new Vector2(Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f))), 1f))
        {
          this.GetNewTargetPosition();
          return;
        }
        this.currentDir = (Vector3) Vector2.Lerp((Vector2) this.currentDir, (Vector2) this.desiredDir, 0.0075f * num).normalized;
        this.movementAxis = (Vector3) Vector2.Perpendicular((Vector2) this.currentDir);
        this.cyclePos += this.currentDir * num * this.speed;
        this.LookAtAngle(Utils.GetAngle(this.transform.position, this.cyclePos + this.movementAxis * Mathf.Sin(Time.time * this.frequency) * this.amplitude));
      }
    }
    else
      this.speed += (float) ((0.0 - (double) this.speed) / 4.0) * num;
    this.move();
  }

  public void OnMoveSpineEvent()
  {
    AudioManager.Instance.PlayOneShot(this.MoveSFX, this.gameObject);
  }

  public void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public void SetMergePath(Vector3 targetPos)
  {
    if (this.pathToFollow != null)
      return;
    this.pathPositionIndex = 0;
    this.givePath(targetPos, this.mergeTarget.gameObject);
  }

  public void HandleMergePathTargetPosition()
  {
    this.LookAtAngle(Utils.GetAngle(this.transform.position, this.pathToFollow[this.pathPositionIndex]));
    if ((double) Vector3.Distance(this.transform.position, this.pathToFollow[this.pathPositionIndex]) > 0.30000001192092896)
      return;
    ++this.pathPositionIndex;
  }

  public void ClearMergePath()
  {
    this.pathToFollow = (List<Vector3>) null;
    this.pathPositionIndex = 0;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public bool CheckObstacle(Vector3 dir, float distance)
  {
    LayerMask layer = (LayerMask) LayerMask.NameToLayer("Island");
    return (bool) (UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) dir, distance, (int) this.obstacleLayermask).collider;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.red);
    Utils.DrawCircleXY(this.transform.position, this.searchDistance, Color.cyan);
    Vector3 cycleTarget = this.cycleTarget;
    Utils.DrawCircleXY(this.cycleTarget, 0.2f, Color.blue);
    Vector3 currentDir = this.currentDir;
    Utils.DrawLine(this.transform.position, this.transform.position + this.currentDir * 2f, Color.red);
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
