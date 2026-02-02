// Decompiled with JetBrains decompiler
// Type: EnemyBulbLamb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
public class EnemyBulbLamb : UnitObject
{
  [SerializeField]
  public SpriteRenderer Aiming;
  public List<KnockableMortarBomb> activeProjectiles = new List<KnockableMortarBomb>();
  public Coroutine currentAttackRoutine;
  public DeadBodyFlying DeadBody;
  public ColliderEvents damageColliderEvents;
  public SimpleSpineFlash SimpleSpineFlash;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  public SkeletonAnimation warningIcon;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/warning";
  [EventRef]
  public string AttackSpitSFX = "event:/dlc/dungeon06/enemy/bulblamb/attack_spit_start";
  [EventRef]
  public string WingFlapSFX = "event:/dlc/dungeon06/enemy/bulblamb/mv_wing_flap_os";
  public Vector3? StartingPosition;
  public Vector3 TargetPosition;
  public Vector2 RandomRepositionDistance = new Vector2(2.5f, 3f);
  public int RepositionsBetweenAttacks = 2;
  public float repositionCooldown = 3f;
  public int repositionCounter;
  public bool canBeStunned = true;
  public int ShotsToFire = 1;
  public float bombExplodeDelay = 2.5f;
  public float delayBeforeShot = 0.4f;
  public float delayAfterShot = 0.2f;
  public GameObject projectilePrefab;
  [FormerlySerializedAs("bombDuration")]
  public float mortarTrajectoryDuration = 0.75f;
  public float minBombRange = 2.5f;
  public float maxBombRange = 5f;
  public float slamAttackRange = 2f;
  public float delayBeforeSlam = 0.4f;
  [Range(0.0f, 1f)]
  public float slamPrepareAnimationDelayNormalzied = 0.8f;
  public float slamPrepareDuration = 0.4f;
  public float slamPrepareZMoveDistance = 1f;
  public float slamDuration = 0.4f;
  public float slamZMoveDistance = 1f;
  public float slamRecoverTime = 1f;
  public int slamBombSpawnCount = 1;
  public float originalSpineZPosition;
  public float MaximumRange = 5f;
  public float attackPlayerDistance = 3f;
  public Vector2 AttackCoolDownDuration = new Vector2(1f, 2f);
  public float AttackCoolDown;
  public bool Attacking;
  public bool NoticedPlayer;
  public float Angle;
  public Vector3 Force;
  public float KnockbackForceModifier = 1f;
  public float KnockbackDuration = 0.5f;
  public bool AttackAfterKnockback;
  public Health EnemyHealth;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public void Start()
  {
    this.originalSpineZPosition = this.Spine.transform.position.z;
    if (!((UnityEngine.Object) this.Aiming != (UnityEngine.Object) null))
      return;
    this.Aiming.gameObject.SetActive(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.StartingPosition.HasValue)
    {
      this.StartingPosition = new Vector3?(this.transform.position);
      this.TargetPosition = this.StartingPosition.Value;
    }
    this.repositionCounter = this.RepositionsBetweenAttacks;
    this.state.CURRENT_STATE = StateMachine.State.Moving;
    this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.MovementRoutine());
  }

  public override void FixedUpdate()
  {
    if ((double) this.Spine.timeScale == 9.9999997473787516E-05)
      return;
    base.FixedUpdate();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.gameObject);
    if (this.Attacking && this.canBeStunned)
    {
      this.CleanupAttackState();
      this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && (double) this.KnockbackForceModifier != 0.0)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public void DoFlapWingsAction()
  {
    AudioManager.Instance.PlayOneShot(this.WingFlapSFX, this.gameObject);
  }

  public IEnumerator HurtRoutine()
  {
    EnemyBulbLamb enemyBulbLamb = this;
    enemyBulbLamb.damageColliderEvents.SetActive(false);
    enemyBulbLamb.Attacking = false;
    enemyBulbLamb.ClearPaths();
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBulbLamb.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyBulbLamb.DisableForces = false;
    enemyBulbLamb.StartCoroutine((IEnumerator) enemyBulbLamb.MovementRoutine());
  }

  public IEnumerator MovementRoutine()
  {
    EnemyBulbLamb enemyBulbLamb = this;
    enemyBulbLamb.state.CURRENT_STATE = StateMachine.State.Idle;
    float repositionTimer = 0.0f;
    float distanceResetTimer = 0.0f;
    Vector2 previousPosition = (Vector2) enemyBulbLamb.transform.position;
    while (true)
    {
      if ((double) repositionTimer < (double) enemyBulbLamb.repositionCooldown && enemyBulbLamb.state.CURRENT_STATE == StateMachine.State.Idle)
        repositionTimer += Time.deltaTime * enemyBulbLamb.Spine.timeScale;
      if ((double) (enemyBulbLamb.AttackCoolDown -= Time.deltaTime) > 0.0 || !enemyBulbLamb.ShouldAttack() || enemyBulbLamb.state.CURRENT_STATE != StateMachine.State.Idle)
      {
        Vector2 vector2;
        if ((double) repositionTimer >= (double) enemyBulbLamb.repositionCooldown && enemyBulbLamb.repositionCounter > 0 && enemyBulbLamb.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          --enemyBulbLamb.repositionCounter;
          float num1 = 100f;
          float num2 = 0.03f;
          float radius = enemyBulbLamb.GetComponent<CircleCollider2D>().radius + num2;
          enemyBulbLamb.TargetEnemy = enemyBulbLamb.GetClosestTarget(ignoreNonUnits: true);
          bool flag = true;
          if ((bool) (UnityEngine.Object) enemyBulbLamb.TargetEnemy)
            flag = (double) Vector3.Distance(enemyBulbLamb.transform.position, enemyBulbLamb.TargetEnemy.transform.position) <= (double) enemyBulbLamb.maxBombRange;
          while ((double) --num1 > 0.0)
          {
            Vector2 direction = Vector2.zero;
            if (!flag && (UnityEngine.Object) enemyBulbLamb.TargetEnemy != (UnityEngine.Object) null)
            {
              float maxInclusive = 30f;
              float f = UnityEngine.Random.Range(-maxInclusive, maxInclusive) * ((float) Math.PI / 180f);
              Vector2 normalized = (Vector2) (enemyBulbLamb.TargetEnemy.transform.position - enemyBulbLamb.transform.position).normalized;
              direction = new Vector2((float) ((double) normalized.x * (double) Mathf.Cos(f) - (double) normalized.y * (double) Mathf.Sin(f)), (float) ((double) normalized.x * (double) Mathf.Sin(f) + (double) normalized.y * (double) Mathf.Cos(f)));
            }
            else
            {
              vector2 = UnityEngine.Random.insideUnitCircle;
              direction = vector2.normalized;
            }
            float distance = UnityEngine.Random.Range(enemyBulbLamb.RandomRepositionDistance.x, enemyBulbLamb.RandomRepositionDistance.y);
            Vector3 PointToCheck = enemyBulbLamb.transform.position + (Vector3) direction * distance;
            RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) (enemyBulbLamb.transform.position + (Vector3) direction * 0.2f), radius, direction, distance, (int) enemyBulbLamb.layerToCheck);
            if ((UnityEngine.Object) raycastHit2D.collider == (UnityEngine.Object) null)
            {
              enemyBulbLamb.FindPath(PointToCheck);
              break;
            }
            Debug.Log((object) raycastHit2D.collider);
          }
          enemyBulbLamb.state.LookAngle = Utils.GetAngle(enemyBulbLamb.transform.position, enemyBulbLamb.TargetPosition);
          repositionTimer = 0.0f;
        }
        enemyBulbLamb.TargetEnemy = enemyBulbLamb.GetClosestTarget(ignoreNonUnits: true);
        if ((bool) (UnityEngine.Object) enemyBulbLamb.TargetEnemy)
        {
          vector2 = (Vector2) enemyBulbLamb.transform.position - previousPosition;
          double magnitude = (double) vector2.magnitude;
          previousPosition = (Vector2) enemyBulbLamb.transform.position;
          if (magnitude <= 0.14000000059604645)
          {
            distanceResetTimer += Time.deltaTime * enemyBulbLamb.Spine.timeScale;
            if ((double) distanceResetTimer > 6.5)
              enemyBulbLamb.transform.position = PlayerFarming.Instance.transform.position;
          }
          else
            distanceResetTimer = 0.0f;
        }
        enemyBulbLamb.state.facingAngle = enemyBulbLamb.Angle;
        yield return (object) null;
      }
      else
        break;
    }
    enemyBulbLamb.ClearPaths();
    enemyBulbLamb.repositionCounter = enemyBulbLamb.RepositionsBetweenAttacks;
    enemyBulbLamb.TargetEnemy = enemyBulbLamb.GetClosestTarget(ignoreNonUnits: true);
    if ((UnityEngine.Object) enemyBulbLamb.TargetEnemy != (UnityEngine.Object) null && (double) Vector3.Distance(enemyBulbLamb.transform.position, enemyBulbLamb.TargetEnemy.transform.position) < (double) enemyBulbLamb.slamAttackRange)
      enemyBulbLamb.currentAttackRoutine = enemyBulbLamb.StartCoroutine((IEnumerator) enemyBulbLamb.AttackSlamRoutine());
    else
      enemyBulbLamb.StartCoroutine((IEnumerator) enemyBulbLamb.AttackWithTimeout());
  }

  public IEnumerator ShootProjectileRoutine()
  {
    EnemyBulbLamb enemyBulbLamb = this;
    if (!enemyBulbLamb.ValidateAttackConditions())
    {
      enemyBulbLamb.ForceCompleteAttack();
    }
    else
    {
      enemyBulbLamb.TargetEnemy = enemyBulbLamb.GetClosestTarget(ignoreNonUnits: true);
      if ((UnityEngine.Object) enemyBulbLamb.TargetEnemy == (UnityEngine.Object) null)
      {
        enemyBulbLamb.ForceCompleteAttack();
      }
      else
      {
        Vector3 targetPosition = enemyBulbLamb.TargetEnemy.transform.position;
        enemyBulbLamb.Angle = Utils.GetAngle(enemyBulbLamb.transform.position, targetPosition);
        enemyBulbLamb.state.facingAngle = enemyBulbLamb.Angle;
        enemyBulbLamb.state.LookAngle = enemyBulbLamb.Angle;
        for (int i = 0; i < enemyBulbLamb.ShotsToFire; ++i)
        {
          if (!enemyBulbLamb.ValidateAttackConditions())
          {
            enemyBulbLamb.ForceCompleteAttack();
            yield break;
          }
          AudioManager.Instance.PlayOneShot(enemyBulbLamb.AttackSpitSFX, enemyBulbLamb.gameObject);
          enemyBulbLamb.Spine.AnimationState.SetAnimation(0, enemyBulbLamb.AttackAnimation, false);
          enemyBulbLamb.Spine.AnimationState.AddAnimation(0, enemyBulbLamb.IdleAnimation, true, 0.0f);
          float timeBefore = 0.0f;
          while ((double) (timeBefore += Time.deltaTime * enemyBulbLamb.Spine.timeScale) < (double) enemyBulbLamb.delayBeforeShot)
            yield return (object) null;
          enemyBulbLamb.TargetEnemy = enemyBulbLamb.GetClosestTarget(ignoreNonUnits: true);
          if (enemyBulbLamb.ShotsToFire > 1 && (UnityEngine.Object) enemyBulbLamb.TargetEnemy != (UnityEngine.Object) null)
            targetPosition = enemyBulbLamb.ClampProjectileTargetPosition(enemyBulbLamb.TargetEnemy.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 0.4f);
          if ((UnityEngine.Object) enemyBulbLamb.TargetEnemy != (UnityEngine.Object) null)
            enemyBulbLamb.SpawnBomb(targetPosition, enemyBulbLamb.TargetEnemy);
          float timeAfter = 0.0f;
          while ((double) (timeAfter += Time.deltaTime * enemyBulbLamb.Spine.timeScale) < (double) enemyBulbLamb.delayAfterShot)
            yield return (object) null;
        }
        enemyBulbLamb.Attacking = false;
        enemyBulbLamb.StartCoroutine((IEnumerator) enemyBulbLamb.MovementRoutine());
      }
    }
  }

  public Vector3 ClampProjectileTargetPosition(Vector3 target)
  {
    Vector3 closestPoint;
    return BiomeGenerator.PointWithinIsland(target, out closestPoint) ? target : closestPoint + (Vector3.zero - closestPoint).normalized * 2f;
  }

  public void FindPath(Vector3 PointToCheck)
  {
    this.TargetPosition = PointToCheck;
    this.givePath(this.TargetPosition);
    this.Points.Add(PointToCheck + Vector3.Normalize(this.transform.position - PointToCheck));
    this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyBulbLamb enemyBulbLamb = this;
    enemyBulbLamb.DisableForces = true;
    enemyBulbLamb.Angle = Utils.GetAngle(Attacker.transform.position, enemyBulbLamb.transform.position) * ((float) Math.PI / 180f);
    enemyBulbLamb.Force = (Vector3) new Vector2(1000f * Mathf.Cos(enemyBulbLamb.Angle), 1000f * Mathf.Sin(enemyBulbLamb.Angle));
    enemyBulbLamb.rb.AddForce((Vector2) (enemyBulbLamb.Force * enemyBulbLamb.KnockbackForceModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBulbLamb.Spine.timeScale) < (double) enemyBulbLamb.KnockbackDuration)
      yield return (object) null;
    enemyBulbLamb.DisableForces = false;
    enemyBulbLamb.rb.velocity = Vector2.zero;
    if (enemyBulbLamb.AttackAfterKnockback)
      enemyBulbLamb.AttackCoolDown = 0.0f;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.gameObject);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!(bool) (UnityEngine.Object) this.DeadBody)
      return;
    GameObject gameObject = this.DeadBody.gameObject.Spawn();
    gameObject.transform.position = this.transform.position;
    gameObject.GetComponent<DeadBodyFlying>().Init(Utils.GetAngle(AttackLocation, this.transform.position));
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.SetActive(false);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public void OnDrawGizmos()
  {
    if (this.StartingPosition.HasValue)
      Utils.DrawCircleXY(this.TargetPosition, 0.3f, Color.red);
    if (this.StartingPosition.HasValue)
      Utils.DrawCircleXY(this.TargetPosition, this.MaximumRange, Color.red);
    else
      Utils.DrawCircleXY(this.transform.position, this.MaximumRange, Color.red);
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], 0.1f, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], 0.1f, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && (this.health.team != Health.Team.PlayerTeam || !this.health.IsCharmedEnemy))
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public virtual bool ShouldAttack() => GameManager.RoomActive && this.repositionCounter <= 0;

  public void OnProjectileDestroyed()
  {
    this.activeProjectiles.RemoveAll((Predicate<KnockableMortarBomb>) (p => (UnityEngine.Object) p == (UnityEngine.Object) null || !p.gameObject.activeInHierarchy));
    if (this.activeProjectiles.Count != 0 || !this.Attacking)
      return;
    this.ForceCompleteAttack();
  }

  public void ForceCompleteAttack()
  {
    if (this.currentAttackRoutine != null)
    {
      this.StopCoroutine(this.currentAttackRoutine);
      this.currentAttackRoutine = (Coroutine) null;
    }
    this.CleanupAttackState();
    this.StartCoroutine((IEnumerator) this.MovementRoutine());
  }

  public void CleanupAttackState()
  {
    foreach (KnockableMortarBomb activeProjectile in this.activeProjectiles)
    {
      if ((UnityEngine.Object) activeProjectile != (UnityEngine.Object) null)
      {
        activeProjectile.OnExplode -= new System.Action(this.OnProjectileDestroyed);
        activeProjectile.OnDestroyProjectile -= new System.Action(this.OnProjectileDestroyed);
      }
    }
    this.activeProjectiles.Clear();
    this.Attacking = false;
    this.currentAttackRoutine = (Coroutine) null;
  }

  public IEnumerator AttackWithTimeout()
  {
    EnemyBulbLamb enemyBulbLamb = this;
    float attackStartTime = Time.time;
    enemyBulbLamb.currentAttackRoutine = enemyBulbLamb.StartCoroutine((IEnumerator) enemyBulbLamb.ShootProjectileRoutine());
    while (enemyBulbLamb.Attacking && (double) Time.time - (double) attackStartTime < 10.0)
    {
      yield return (object) new WaitForSeconds(0.1f);
      if ((UnityEngine.Object) enemyBulbLamb.GetClosestTarget() == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) "BulbLamb: Target lost during attack, aborting");
        enemyBulbLamb.ForceCompleteAttack();
        yield break;
      }
    }
    if (enemyBulbLamb.Attacking)
    {
      Debug.LogWarning((object) "BulbLamb: Attack timeout, forcing completion");
      enemyBulbLamb.ForceCompleteAttack();
    }
  }

  public bool ValidateAttackConditions()
  {
    this.TargetEnemy = this.GetClosestTarget();
    if ((UnityEngine.Object) this.TargetEnemy == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "BulbLamb: No valid target for attack");
      return false;
    }
    if (!((UnityEngine.Object) this.health == (UnityEngine.Object) null) && (double) this.health.HP > 0.0)
      return true;
    Debug.LogWarning((object) "BulbLamb: Invalid health state during attack");
    return false;
  }

  public IEnumerator AttackSlamRoutine()
  {
    EnemyBulbLamb enemyBulbLamb = this;
    if (!enemyBulbLamb.ValidateAttackConditions())
    {
      enemyBulbLamb.ForceCompleteAttack();
    }
    else
    {
      enemyBulbLamb.TargetEnemy = enemyBulbLamb.GetClosestTarget(ignoreNonUnits: true);
      if ((UnityEngine.Object) enemyBulbLamb.TargetEnemy == (UnityEngine.Object) null)
      {
        enemyBulbLamb.ForceCompleteAttack();
      }
      else
      {
        enemyBulbLamb.Attacking = true;
        AudioManager.Instance.PlayOneShot(enemyBulbLamb.WarningVO, enemyBulbLamb.gameObject);
        enemyBulbLamb.SimpleSpineFlash.FlashWhite(true);
        DOVirtual.DelayedCall(enemyBulbLamb.slamPrepareDuration * enemyBulbLamb.slamPrepareAnimationDelayNormalzied, new TweenCallback(enemyBulbLamb.\u003CAttackSlamRoutine\u003Eb__80_0));
        DG.Tweening.Sequence attackSequence = DOTween.Sequence();
        Tween t1 = (Tween) enemyBulbLamb.Spine.transform.DOMoveZ(enemyBulbLamb.originalSpineZPosition - enemyBulbLamb.slamPrepareZMoveDistance, enemyBulbLamb.slamPrepareDuration).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(enemyBulbLamb.delayBeforeSlam).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(enemyBulbLamb.\u003CAttackSlamRoutine\u003Eb__80_1));
        Tween t2 = (Tween) enemyBulbLamb.Spine.transform.DOMoveZ(enemyBulbLamb.originalSpineZPosition + enemyBulbLamb.slamZMoveDistance, enemyBulbLamb.slamDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBounce).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(enemyBulbLamb.\u003CAttackSlamRoutine\u003Eb__80_2));
        Tween t3 = (Tween) enemyBulbLamb.Spine.transform.DOMoveZ(enemyBulbLamb.originalSpineZPosition, enemyBulbLamb.slamRecoverTime).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
        attackSequence.Append(t1);
        attackSequence.Append(t2);
        attackSequence.Append(t3);
        attackSequence.Play<DG.Tweening.Sequence>();
        while (!attackSequence.IsComplete() && attackSequence.active)
          yield return (object) null;
        enemyBulbLamb.Attacking = false;
        enemyBulbLamb.StartCoroutine((IEnumerator) enemyBulbLamb.MovementRoutine());
      }
    }
  }

  public void SpawnBomb(Vector3 targetPosition, Health closestTarget)
  {
    float num1 = 2.2f;
    Vector3 position1 = this.transform.position;
    Vector3 position2 = new Vector3(position1.x, position1.y + num1, position1.z);
    KnockableMortarBomb component1 = ObjectPool.Spawn(this.projectilePrefab, position2, Quaternion.identity).GetComponent<KnockableMortarBomb>();
    component1.ExplodeOnTouch = false;
    this.activeProjectiles.Add(component1);
    component1.OnExplode += new System.Action(this.OnProjectileDestroyed);
    component1.OnDestroyProjectile += new System.Action(this.OnProjectileDestroyed);
    component1.SetOwner(this.gameObject);
    UnitObject component2 = closestTarget.GetComponent<UnitObject>();
    Vector3 vector3_1 = new Vector3(component2.vx, component2.vy, 0.0f);
    Vector3 normalized = (targetPosition + vector3_1 - position1).normalized;
    float num2 = Vector3.Distance(position1, targetPosition);
    Vector3 vector3_2 = this.ClampProjectileTargetPosition(position1 + normalized * Mathf.Clamp(num2, this.minBombRange, this.maxBombRange));
    component1.transform.position = vector3_2;
    component1.Play(position2 + new Vector3(0.0f, 0.0f, -1.5f), this.mortarTrajectoryDuration, this.bombExplodeDelay, this.health, false);
    this.SimpleSpineFlash.FlashWhite(false);
  }

  [CompilerGenerated]
  public void \u003CAttackSlamRoutine\u003Eb__80_0()
  {
    this.Spine.AnimationState.SetAnimation(0, this.SignPostAttackAnimation, false);
  }

  [CompilerGenerated]
  public void \u003CAttackSlamRoutine\u003Eb__80_1()
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/bulblamb/attack_spit_fast_start", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CAttackSlamRoutine\u003Eb__80_2()
  {
    BiomeConstants.Instance.EmitHammerEffects(this.transform.position, 0.1f, 1.2f, 0.3f, true, 1f, false);
    Explosion.CreateExplosion(this.transform.position, this.health.team, this.health, this.slamAttackRange, Team2Damage: 4f, noDistort: true);
    for (int index = 0; index < this.slamBombSpawnCount; ++index)
      this.SpawnBomb(this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 3f, this.TargetEnemy);
    this.SimpleSpineFlash.FlashWhite(false);
    this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
  }
}
