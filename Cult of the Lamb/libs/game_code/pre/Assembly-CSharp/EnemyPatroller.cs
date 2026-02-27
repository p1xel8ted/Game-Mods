// Decompiled with JetBrains decompiler
// Type: EnemyPatroller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using Spine.Unity.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyPatroller : UnitObject
{
  private int Patrol;
  public List<Vector3> PatrolRoute = new List<Vector3>();
  private Vector3 StartPosition;
  private List<Collider2D> collider2DList;
  private Health EnemyHealth;
  private float RepathTimer;
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
  private float psychoThreshold;
  [SerializeField]
  private float psychoDiveMultiplier = 1f;
  [SerializeField]
  private float psychoAttackRangeMultiplier = 1f;
  [SerializeField]
  private float psychoDiveSpeedMultiplier = 1f;
  private bool isPsycho;
  private float maxHealth;
  private bool NoticedPlayer;
  private bool FirstPath = true;
  public bool JumpOnHit = true;
  private float Angle;
  private Vector3 Force;
  public float KnockbackForceModifier = 1f;
  public float KnockbackDelay = 0.5f;
  private bool ApplyingForce;
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer = 0.8f;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  private bool PathingToPlayer;
  private float RandomDirection;
  private float DistanceToPathTowardsPlayer = 6f;
  private Vector3 TargetPosition;
  public int NumberOfDives = 3;
  public ParticleSystem AoEParticles;
  public float MoveSpeed = 8f;
  public float ArcHeight = 5f;
  private float DiveDelayTimer;
  public float DiveDelay = 3f;
  public List<FollowAsTail> TailPieces = new List<FollowAsTail>();

  protected virtual void Start()
  {
    this.StartPosition = this.transform.position;
    this.PatrolRoute.Insert(0, Vector3.zero);
    this.maxHealth = this.health.totalHP;
  }

  private IEnumerator ActiveRoutine()
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
          Debug.Log((object) ("pathToFollow " + (object) enemyPatroller1.pathToFollow));
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

  private IEnumerator DelayDiveRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyPatroller enemyPatroller = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyPatroller.StartCoroutine((IEnumerator) enemyPatroller.DiveMoveRoutine());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.3f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator ApplyForceRoutine(GameObject Attacker)
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
    yield return (object) new WaitForSeconds(enemyPatroller.KnockbackDelay);
    enemyPatroller.DisableForces = false;
    if (!enemyPatroller.JumpOnHit)
    {
      enemyPatroller.ApplyingForce = false;
      enemyPatroller.Spine.AnimationState.SetAnimation(0, "animation", true);
      enemyPatroller.StartCoroutine((IEnumerator) enemyPatroller.ActiveRoutine());
    }
  }

  private bool GetNewPlayerPosition()
  {
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
      return false;
    float distance = 4f;
    float f = Utils.GetAngle(this.transform.position, closestTarget.transform.position) * ((float) Math.PI / 180f);
    bool flag = true;
    float num = 100f;
    while ((double) --num > 0.0)
    {
      if (!flag)
        f += (float) UnityEngine.Random.Range(-90, 90) * ((float) Math.PI / 180f);
      flag = false;
      Vector3 vector3 = this.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(vector3 - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
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

  public void GetNewTargetPosition()
  {
    float num = 100f;
    if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.DistanceToPathTowardsPlayer && this.CheckLineOfSight(PlayerFarming.Instance.transform.position, this.DistanceToPathTowardsPlayer))
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
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
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

  private IEnumerator DiveMoveRoutine()
  {
    EnemyPatroller enemyPatroller = this;
    enemyPatroller.ClearPaths();
    enemyPatroller.state.CURRENT_STATE = StateMachine.State.Idle;
    int i = -1;
    while (++i < enemyPatroller.NumberOfDives)
    {
      if (enemyPatroller.GetNewPlayerPosition())
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_jump", enemyPatroller.transform.position);
        enemyPatroller.health.invincible = true;
        enemyPatroller.Spine.AnimationState.SetAnimation(0, "jump", false);
        Vector3 StartPosition = enemyPatroller.transform.position;
        float Progress = 0.0f;
        float Duration = Vector3.Distance(StartPosition, enemyPatroller.TargetPosition) / enemyPatroller.MoveSpeed;
        Vector3 Curve = StartPosition + (enemyPatroller.TargetPosition - StartPosition) / 2f + Vector3.back * enemyPatroller.ArcHeight;
        while ((double) (Progress += Time.deltaTime) < (double) Duration)
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
        yield return (object) new WaitForSeconds(0.15f);
        enemyPatroller.damageColliderEvents.SetActive(true);
        yield return (object) new WaitForSeconds(0.15f);
        enemyPatroller.damageColliderEvents.SetActive(false);
        if (i < enemyPatroller.NumberOfDives - 1)
          yield return (object) new WaitForSeconds(0.2f);
        StartPosition = new Vector3();
        Curve = new Vector3();
      }
    }
    enemyPatroller.DiveDelayTimer = enemyPatroller.DiveDelay;
    enemyPatroller.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyPatroller.StartCoroutine((IEnumerator) enemyPatroller.ActiveRoutine());
  }

  private void GetTailPieces()
  {
    this.TailPieces = new List<FollowAsTail>((IEnumerable<FollowAsTail>) this.GetComponentsInChildren<FollowAsTail>());
  }

  private void SetTailPieces()
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

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  private void OnDrawGizmos()
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
