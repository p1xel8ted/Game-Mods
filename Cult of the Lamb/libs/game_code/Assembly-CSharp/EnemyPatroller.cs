// Decompiled with JetBrains decompiler
// Type: EnemyPatroller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using Spine.Unity.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyPatroller : UnitObject
{
  public int Patrol;
  public List<Vector3> PatrolRoute = new List<Vector3>();
  public Vector3 StartPosition;
  public List<Collider2D> collider2DList;
  public Health EnemyHealth;
  public float RepathTimer;
  public Transform scaleThis;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public float AttackTriggerRange = 1f;
  public SkeletonAnimation Spine;
  public SkeletonUtilityEyeConstraint skeletonUtilityEyeConstraint;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation warningIcon;
  public float PatrolSpeed = 0.05f;
  public float ChaseSpeed = 0.1f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float psychoThreshold;
  [SerializeField]
  public float psychoDiveMultiplier = 1f;
  [SerializeField]
  public float psychoAttackRangeMultiplier = 1f;
  [SerializeField]
  public float psychoDiveSpeedMultiplier = 1f;
  public bool isPsycho;
  public float maxHealth;
  public bool NoticedPlayer;
  public bool FirstPath = true;
  public bool JumpOnHit = true;
  public float Angle;
  public Vector3 Force;
  public float KnockbackForceModifier = 1f;
  public float KnockbackDelay = 0.5f;
  public bool ApplyingForce;
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer = 0.8f;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public bool PathingToPlayer;
  public float RandomDirection;
  public float DistanceToPathTowardsPlayer = 6f;
  public Vector2 lowestRoomBoundPoint;
  public Vector2 highestRoomBoundPoint;
  public bool isOptimizationsInitialized;
  public Vector3 TargetPosition;
  public int NumberOfDives = 3;
  public ParticleSystem AoEParticles;
  public float MoveSpeed = 8f;
  public float ArcHeight = 5f;
  public float DiveDelayTimer;
  public float DiveDelay = 3f;
  public List<FollowAsTail> TailPieces = new List<FollowAsTail>();

  public virtual void Start()
  {
    this.StartPosition = this.transform.position;
    this.PatrolRoute.Insert(0, Vector3.zero);
    this.maxHealth = this.health.totalHP;
  }

  public IEnumerator ActiveRoutine()
  {
    EnemyPatroller enemyPatroller1 = this;
    yield return (object) new WaitForEndOfFrame();
    enemyPatroller1.Spine.AnimationState.SetAnimation(0, "animation", true);
    if ((UnityEngine.Object) enemyPatroller1.damageColliderEvents != (UnityEngine.Object) null)
      enemyPatroller1.damageColliderEvents.SetActive(false);
    while ((double) (enemyPatroller1.DiveDelayTimer -= Time.deltaTime) > 0.0 || !((UnityEngine.Object) enemyPatroller1.GetClosestTarget() != (UnityEngine.Object) null) || (double) Vector3.Distance(enemyPatroller1.transform.position, enemyPatroller1.GetClosestTarget().transform.position) >= (double) enemyPatroller1.AttackTriggerRange)
    {
      if (!enemyPatroller1.NoticedPlayer && enemyPatroller1.PatrolRoute.Count > 1)
      {
        if (enemyPatroller1.pathToFollow == null)
        {
          Debug.Log((object) ("pathToFollow " + enemyPatroller1.pathToFollow?.ToString()));
          EnemyPatroller enemyPatroller2 = enemyPatroller1;
          EnemyPatroller enemyPatroller3 = enemyPatroller1;
          int num1 = enemyPatroller1.Patrol + 1;
          int num2 = num1;
          enemyPatroller3.Patrol = num2;
          int num3 = num1 % enemyPatroller1.PatrolRoute.Count;
          enemyPatroller2.Patrol = num3;
          enemyPatroller1.givePath(enemyPatroller1.StartPosition + enemyPatroller1.PatrolRoute[enemyPatroller1.Patrol]);
        }
        else if ((double) (enemyPatroller1.RepathTimer += Time.deltaTime) > (enemyPatroller1.FirstPath ? 0.0 : 0.5))
        {
          enemyPatroller1.FirstPath = true;
          enemyPatroller1.maxSpeed = enemyPatroller1.PatrolSpeed;
          enemyPatroller1.givePath(enemyPatroller1.StartPosition + enemyPatroller1.PatrolRoute[enemyPatroller1.Patrol]);
          enemyPatroller1.RepathTimer = 0.0f;
        }
      }
      else if (enemyPatroller1.state.CURRENT_STATE == StateMachine.State.Idle)
      {
        enemyPatroller1.maxSpeed = enemyPatroller1.ChaseSpeed;
        enemyPatroller1.GetNewTargetPosition();
      }
      yield return (object) null;
    }
    if (!enemyPatroller1.NoticedPlayer)
    {
      if (!string.IsNullOrEmpty("event:/enemy/vocals/worm/warning"))
        AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/warning", enemyPatroller1.transform.position);
      enemyPatroller1.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
      enemyPatroller1.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, 2f);
      enemyPatroller1.StartCoroutine((IEnumerator) enemyPatroller1.DelayDiveRoutine());
    }
    else
      enemyPatroller1.StartCoroutine((IEnumerator) enemyPatroller1.DiveMoveRoutine());
    enemyPatroller1.NoticedPlayer = true;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.JumpOnHit && (double) this.DiveDelayTimer > 0.0)
    {
      this.StopAllCoroutines();
      this.DisableForces = false;
      this.StartCoroutine((IEnumerator) this.DelayDiveRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && !this.ApplyingForce)
    {
      if (!this.JumpOnHit)
        this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    }
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
    if ((double) this.health.HP / (double) this.maxHealth >= (double) this.psychoThreshold || this.isPsycho)
      return;
    this.DiveDelay *= this.psychoDiveMultiplier;
    this.DiveDelayTimer = 0.0f;
    this.AttackTriggerRange *= this.psychoAttackRangeMultiplier;
    this.MoveSpeed *= this.psychoDiveSpeedMultiplier;
    this.ChanceToPathTowardsPlayer = 1f;
    this.isPsycho = true;
  }

  public IEnumerator DelayDiveRoutine()
  {
    EnemyPatroller enemyPatroller = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyPatroller.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyPatroller.StartCoroutine((IEnumerator) enemyPatroller.DiveMoveRoutine());
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyPatroller enemyPatroller = this;
    if (!enemyPatroller.JumpOnHit)
    {
      enemyPatroller.ApplyingForce = true;
      enemyPatroller.Spine.AnimationState.SetAnimation(0, "attack-impact", false);
      enemyPatroller.Spine.AnimationState.AddAnimation(0, "attack-charge", true, 0.0f);
    }
    enemyPatroller.DisableForces = true;
    enemyPatroller.Angle = Utils.GetAngle(Attacker.transform.position, enemyPatroller.transform.position) * ((float) Math.PI / 180f);
    enemyPatroller.Force = (Vector3) new Vector2(1000f * Mathf.Cos(enemyPatroller.Angle), 1000f * Mathf.Sin(enemyPatroller.Angle));
    enemyPatroller.rb.AddForce((Vector2) (enemyPatroller.Force * enemyPatroller.KnockbackForceModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyPatroller.Spine.timeScale) < (double) enemyPatroller.KnockbackDelay)
      yield return (object) null;
    enemyPatroller.DisableForces = false;
    if (!enemyPatroller.JumpOnHit)
    {
      enemyPatroller.ApplyingForce = false;
      enemyPatroller.Spine.AnimationState.SetAnimation(0, "animation", true);
      enemyPatroller.StartCoroutine((IEnumerator) enemyPatroller.ActiveRoutine());
    }
  }

  public bool GetNewPlayerPosition()
  {
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
      return false;
    float distance = 4f;
    float f = Utils.GetAngle(this.transform.position, closestTarget.transform.position) * ((float) Math.PI / 180f);
    bool flag1 = true;
    float num = 2f;
    while ((double) --num > 0.0)
    {
      if (!flag1)
        f += (float) UnityEngine.Random.Range(-90, 90) * ((float) Math.PI / 180f);
      flag1 = false;
      Vector3 vector3 = this.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      bool flag2 = false;
      float radius = 0.2f;
      if (this.isOptimizationsInitialized)
      {
        Vector2 vector2 = (Vector2) vector3;
        if ((double) vector2.x <= (double) this.lowestRoomBoundPoint.x || (double) vector2.x >= (double) this.highestRoomBoundPoint.x || (double) vector2.y <= (double) this.lowestRoomBoundPoint.y || (double) vector2.y >= (double) this.highestRoomBoundPoint.y)
          flag2 = true;
      }
      else if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
        flag2 = true;
      if (flag2)
      {
        f = 180f - f;
      }
      else
      {
        this.TargetPosition = vector3;
        return true;
      }
    }
    return false;
  }

  public void InitializeOptimization(Vector2 lowest, Vector2 highest)
  {
    if (this.isOptimizationsInitialized)
      return;
    this.lowestRoomBoundPoint = new Vector2(lowest.x + 2f, lowest.y + 2f);
    this.highestRoomBoundPoint = new Vector2(highest.x - 2f, highest.y - 2f);
    this.isOptimizationsInitialized = true;
  }

  public void GetNewTargetPosition()
  {
    float num = 2f;
    PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(this.transform.position);
    if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.DistanceToPathTowardsPlayer && this.CheckLineOfSightOnTarget(closestPlayer.gameObject, closestPlayer.transform.position, this.DistanceToPathTowardsPlayer))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position) * ((float) Math.PI / 180f);
    }
    while ((double) --num > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      if (!this.PathingToPlayer)
        this.RandomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      this.PathingToPlayer = false;
      float radius = 0.2f;
      Vector3 targetLocation = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      bool flag = false;
      if (this.isOptimizationsInitialized)
      {
        Vector2 vector2 = (Vector2) targetLocation;
        if ((double) vector2.x <= (double) this.lowestRoomBoundPoint.x || (double) vector2.x >= (double) this.highestRoomBoundPoint.x || (double) vector2.y <= (double) this.lowestRoomBoundPoint.y || (double) vector2.y >= (double) this.highestRoomBoundPoint.y)
          flag = true;
      }
      else if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
        flag = true;
      if (flag)
      {
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
        this.givePath(targetLocation);
        break;
      }
    }
  }

  public IEnumerator DiveMoveRoutine()
  {
    EnemyPatroller enemyPatroller = this;
    enemyPatroller.ClearPaths();
    enemyPatroller.state.CURRENT_STATE = StateMachine.State.Idle;
    int i = -1;
    while (++i < enemyPatroller.NumberOfDives)
    {
      if (enemyPatroller.GetNewPlayerPosition())
      {
        while ((double) enemyPatroller.Spine.timeScale == 9.9999997473787516E-05)
          yield return (object) null;
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_jump", enemyPatroller.transform.position);
        enemyPatroller.health.invincible = true;
        enemyPatroller.Spine.AnimationState.SetAnimation(0, "jump", false);
        Vector3 StartPosition = enemyPatroller.transform.position;
        float Progress = 0.0f;
        float Duration = Vector3.Distance(StartPosition, enemyPatroller.TargetPosition) / enemyPatroller.MoveSpeed;
        Vector3 Curve = StartPosition + (enemyPatroller.TargetPosition - StartPosition) / 2f + Vector3.back * enemyPatroller.ArcHeight;
        while ((double) (Progress += Time.deltaTime * enemyPatroller.Spine.timeScale) < (double) Duration)
        {
          Vector3 a = Vector3.Lerp(StartPosition, Curve, Progress / Duration);
          Vector3 b = Vector3.Lerp(Curve, enemyPatroller.TargetPosition, Progress / Duration);
          enemyPatroller.transform.position = Vector3.Lerp(a, b, Progress / Duration);
          yield return (object) null;
        }
        enemyPatroller.TargetPosition.z = 0.0f;
        enemyPatroller.transform.position = enemyPatroller.TargetPosition;
        enemyPatroller.Spine.transform.localPosition = Vector3.zero;
        enemyPatroller.Spine.AnimationState.SetAnimation(0, "land", false);
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_land", enemyPatroller.transform.position);
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        enemyPatroller.AoEParticles.Play();
        enemyPatroller.health.invincible = false;
        float time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyPatroller.Spine.timeScale) < 0.15000000596046448)
          yield return (object) null;
        enemyPatroller.damageColliderEvents.SetActive(true);
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyPatroller.Spine.timeScale) < 0.15000000596046448)
          yield return (object) null;
        enemyPatroller.damageColliderEvents.SetActive(false);
        if (i < enemyPatroller.NumberOfDives - 1)
        {
          while ((double) (time += Time.deltaTime * enemyPatroller.Spine.timeScale) < 0.20000000298023224)
            yield return (object) null;
        }
        StartPosition = new Vector3();
        Curve = new Vector3();
      }
    }
    enemyPatroller.DiveDelayTimer = enemyPatroller.DiveDelay;
    enemyPatroller.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyPatroller.StartCoroutine((IEnumerator) enemyPatroller.ActiveRoutine());
  }

  public void GetTailPieces()
  {
    this.TailPieces = new List<FollowAsTail>((IEnumerable<FollowAsTail>) this.GetComponentsInChildren<FollowAsTail>());
  }

  public void SetTailPieces()
  {
    int index = -1;
    while (++index < this.TailPieces.Count)
      this.TailPieces[index].FollowObject = index != 0 ? this.TailPieces[index - 1].transform : this.Spine.transform;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.AttackTriggerRange, Color.white);
    if (!Application.isPlaying)
    {
      int index = -1;
      while (++index < this.PatrolRoute.Count)
      {
        if (index == this.PatrolRoute.Count - 1 || index == 0)
          Utils.DrawLine(this.transform.position, this.transform.position + this.PatrolRoute[index], Color.yellow);
        if (index > 0)
          Utils.DrawLine(this.transform.position + this.PatrolRoute[index - 1], this.transform.position + this.PatrolRoute[index], Color.yellow);
        Utils.DrawCircleXY(this.transform.position + this.PatrolRoute[index], 0.2f, Color.yellow);
      }
    }
    else
    {
      int index = -1;
      while (++index < this.PatrolRoute.Count)
      {
        if (index == this.PatrolRoute.Count - 1 || index == 0)
          Utils.DrawLine(this.StartPosition, this.StartPosition + this.PatrolRoute[index], Color.yellow);
        if (index > 0)
          Utils.DrawLine(this.StartPosition + this.PatrolRoute[index - 1], this.StartPosition + this.PatrolRoute[index], Color.yellow);
        Utils.DrawCircleXY(this.StartPosition + this.PatrolRoute[index], 0.2f, Color.yellow);
      }
    }
  }
}
