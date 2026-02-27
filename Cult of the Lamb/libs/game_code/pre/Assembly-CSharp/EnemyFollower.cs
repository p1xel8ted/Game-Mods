// Decompiled with JetBrains decompiler
// Type: EnemyFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyFollower : UnitObject
{
  public int NewPositionDistance = 3;
  public float MaintainTargetDistance = 4.5f;
  public float MoveCloserDistance = 4f;
  public float AttackWithinRange = 4f;
  public bool DoubleAttack = true;
  public bool ChargeAndAttack = true;
  public Vector2 MaxAttackDelayRandomRange = new Vector2(4f, 6f);
  public Vector2 AttackDelayRandomRange = new Vector2(0.5f, 2f);
  public ColliderEvents damageColliderEvents;
  [SerializeField]
  private bool requireLineOfSite = true;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public float TeleportDelayTarget = 1f;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  private GameObject TargetObject;
  private float AttackDelay;
  private bool canBeParried;
  private static float signPostParryWindow = 0.2f;
  private static float attackParryWindow = 0.15f;
  protected Vector3 Force;
  [HideInInspector]
  public float Angle;
  private Vector3 TargetPosition;
  private float RepathTimer;
  private float TeleportDelay;
  private EnemyFollower.State MyState;
  private float MaxAttackDelay;
  private Health EnemyHealth;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
  }

  private void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!PlayerController.CanParryAttacks || !this.canBeParried || FromBehind || AttackType != Health.AttackTypes.Melee)
      return;
    this.health.WasJustParried = true;
    this.SimpleSpineFlash.FlashWhite(false);
    this.SeperateObject = true;
    this.UsePathing = true;
    this.health.invincible = false;
    this.StopAllCoroutines();
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
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (this.MyState != EnemyFollower.State.Attacking)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.StopAllCoroutines();
      if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
        this.state.CURRENT_STATE = StateMachine.State.HitRight;
      if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
        this.state.CURRENT_STATE = StateMachine.State.HitLeft;
      if (AttackType != Health.AttackTypes.Heavy && (!(AttackType == Health.AttackTypes.Projectile & FromBehind) || this.health.HasShield))
        this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    if (AttackType != Health.AttackTypes.NoKnockBack)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    this.SimpleSpineFlash.FlashFillRed();
  }

  protected virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyFollower enemyFollower = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyFollower.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyFollower.DisableForces = true;
    enemyFollower.Force = (enemyFollower.transform.position - Attacker.transform.position).normalized * 500f;
    enemyFollower.rb.AddForce((Vector2) enemyFollower.Force);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator HurtRoutine()
  {
    EnemyFollower enemyFollower = this;
    yield return (object) new WaitForSeconds(0.3f);
    if ((UnityEngine.Object) enemyFollower.TargetObject != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) enemyFollower.transform.position, (Vector2) enemyFollower.TargetObject.transform.position) < 3.0)
      enemyFollower.StartCoroutine((IEnumerator) enemyFollower.FightPlayer(3.5f));
    else
      enemyFollower.StartCoroutine((IEnumerator) enemyFollower.ChasePlayer());
  }

  private IEnumerator WaitForTarget()
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.Spine.Initialize(false);
    if (GameManager.RoomActive)
    {
      while ((UnityEngine.Object) enemyFollower.TargetObject == (UnityEngine.Object) null)
      {
        enemyFollower.TargetObject = GameObject.FindWithTag("Player");
        yield return (object) null;
      }
      bool InRange = false;
      while (!InRange)
      {
        if ((UnityEngine.Object) enemyFollower.TargetObject == (UnityEngine.Object) null)
        {
          enemyFollower.StartCoroutine((IEnumerator) enemyFollower.WaitForTarget());
          yield break;
        }
        float a = Vector3.Distance(enemyFollower.TargetObject.transform.position, enemyFollower.transform.position);
        if ((double) a <= (double) enemyFollower.VisionRange && (!enemyFollower.requireLineOfSite || enemyFollower.CheckLineOfSight(enemyFollower.TargetObject.transform.position, Mathf.Min(a, (float) enemyFollower.VisionRange))))
          InRange = true;
        yield return (object) null;
      }
      enemyFollower.StartCoroutine((IEnumerator) enemyFollower.ChasePlayer());
    }
  }

  public IEnumerator ChasePlayer()
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.MyState = EnemyFollower.State.WaitAndTaunt;
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyFollower.AttackDelay = UnityEngine.Random.Range(enemyFollower.AttackDelayRandomRange.x, enemyFollower.AttackDelayRandomRange.y);
    if (enemyFollower.health.HasShield)
      enemyFollower.AttackDelay = 2.5f;
    enemyFollower.MaxAttackDelay = UnityEngine.Random.Range(enemyFollower.MaxAttackDelayRandomRange.x, enemyFollower.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop && !((UnityEngine.Object) enemyFollower.TargetObject == (UnityEngine.Object) null))
    {
      if ((UnityEngine.Object) enemyFollower.damageColliderEvents != (UnityEngine.Object) null)
        enemyFollower.damageColliderEvents.SetActive(false);
      enemyFollower.TeleportDelay -= Time.deltaTime;
      enemyFollower.AttackDelay -= Time.deltaTime;
      enemyFollower.MaxAttackDelay -= Time.deltaTime;
      if (enemyFollower.MyState == EnemyFollower.State.WaitAndTaunt)
      {
        if (enemyFollower.Spine.AnimationName != "roll-stop" && enemyFollower.state.CURRENT_STATE == StateMachine.State.Moving && enemyFollower.Spine.AnimationName != "run-enemy")
          enemyFollower.Spine.AnimationState.SetAnimation(1, "run-enemy", true);
        enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
        enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
        if (enemyFollower.state.CURRENT_STATE == StateMachine.State.Idle && (double) (enemyFollower.RepathTimer -= Time.deltaTime) < 0.0)
        {
          if (enemyFollower.CustomAttackLogic())
            break;
          if ((double) enemyFollower.MaxAttackDelay < 0.0 || (double) Vector3.Distance(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position) < (double) enemyFollower.AttackWithinRange)
          {
            if (enemyFollower.ChargeAndAttack && ((double) enemyFollower.MaxAttackDelay < 0.0 || (double) enemyFollower.AttackDelay < 0.0))
            {
              enemyFollower.health.invincible = false;
              enemyFollower.StopAllCoroutines();
              enemyFollower.StartCoroutine((IEnumerator) enemyFollower.FightPlayer());
            }
            else if (!enemyFollower.health.HasShield)
            {
              enemyFollower.Angle = (float) (((double) Utils.GetAngle(enemyFollower.TargetObject.transform.position, enemyFollower.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemyFollower.TargetPosition = enemyFollower.TargetObject.transform.position + new Vector3(enemyFollower.MaintainTargetDistance * Mathf.Cos(enemyFollower.Angle), enemyFollower.MaintainTargetDistance * Mathf.Sin(enemyFollower.Angle));
              enemyFollower.FindPath(enemyFollower.TargetPosition);
            }
          }
          else if ((double) Vector3.Distance(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position) > (double) enemyFollower.MoveCloserDistance + (enemyFollower.health.HasShield ? 0.0 : 1.0))
          {
            enemyFollower.Angle = (float) (((double) Utils.GetAngle(enemyFollower.TargetObject.transform.position, enemyFollower.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
            enemyFollower.TargetPosition = enemyFollower.TargetObject.transform.position + new Vector3(enemyFollower.MaintainTargetDistance * Mathf.Cos(enemyFollower.Angle), enemyFollower.MaintainTargetDistance * Mathf.Sin(enemyFollower.Angle));
            enemyFollower.FindPath(enemyFollower.TargetPosition);
          }
        }
      }
      enemyFollower.Seperate(0.5f);
      yield return (object) null;
    }
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    this.TargetObject = TargetObject;
    this.StartCoroutine((IEnumerator) this.ChasePlayer());
  }

  public virtual bool CustomAttackLogic() => false;

  private void FindPath(Vector3 PointToCheck)
  {
    this.RepathTimer = 0.2f;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.NewPositionDistance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) <= 1.0)
        return;
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset;
      this.givePath(this.TargetPosition);
    }
    else
    {
      this.TargetPosition = PointToCheck;
      this.givePath(PointToCheck);
    }
  }

  private void Teleport()
  {
    if (this.MyState != EnemyFollower.State.WaitAndTaunt || (double) this.health.HP <= 0.0)
      return;
    float num1 = 100f;
    float num2;
    if ((double) (num2 = num1 - 1f) <= 0.0)
      return;
    float f = (float) (((double) Utils.GetAngle(this.transform.position, this.TargetObject.transform.position) + (double) UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
    float distance = 4.5f;
    float radius = 0.2f;
    Vector3 Position = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(Position - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - Position) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.StartCoroutine((IEnumerator) this.TeleportRoutine((Vector3) raycastHit2D.centroid));
    }
    else
    {
      if (this.ShowDebug)
      {
        this.EndPoints.Add(new Vector3(Position.x, Position.y));
        this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.StartCoroutine((IEnumerator) this.TeleportRoutine(Position));
    }
  }

  private IEnumerator TeleportRoutine(Vector3 Position)
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.ClearPaths();
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Moving;
    enemyFollower.UsePathing = false;
    enemyFollower.health.invincible = true;
    enemyFollower.SeperateObject = false;
    enemyFollower.MyState = EnemyFollower.State.Teleporting;
    enemyFollower.ClearPaths();
    Vector3 position = enemyFollower.transform.position;
    float Progress = 0.0f;
    enemyFollower.Spine.AnimationState.SetAnimation(1, "roll", true);
    enemyFollower.state.facingAngle = enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, Position);
    enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 b = Position;
    float Duration = Vector3.Distance(position, b) / 10f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      enemyFollower.speed = 10f * Time.deltaTime;
      yield return (object) null;
    }
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyFollower.Spine.AnimationState.SetAnimation(1, "roll-stop", false);
    yield return (object) new WaitForSeconds(0.3f);
    enemyFollower.UsePathing = true;
    enemyFollower.RepathTimer = 0.5f;
    enemyFollower.TeleportDelay = enemyFollower.TeleportDelayTarget;
    enemyFollower.SeperateObject = true;
    enemyFollower.health.invincible = false;
    enemyFollower.MyState = EnemyFollower.State.WaitAndTaunt;
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
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

  private IEnumerator FightPlayer(float AttackDistance = 1.5f)
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.MyState = EnemyFollower.State.Attacking;
    enemyFollower.UsePathing = true;
    enemyFollower.givePath(enemyFollower.TargetObject.transform.position);
    enemyFollower.Spine.AnimationState.SetAnimation(1, "run-enemy", true);
    enemyFollower.RepathTimer = 0.0f;
    int NumAttacks = enemyFollower.DoubleAttack ? 2 : 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = 0.5f;
    while (Loop)
    {
      enemyFollower.Seperate(0.5f);
      switch (enemyFollower.state.CURRENT_STATE)
      {
        case StateMachine.State.Moving:
          enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
          enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
          enemyFollower.state.LookAngle = enemyFollower.state.facingAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
          if ((double) Vector2.Distance((Vector2) enemyFollower.transform.position, (Vector2) enemyFollower.TargetObject.transform.position) < (double) AttackDistance)
          {
            enemyFollower.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            enemyFollower.Spine.AnimationState.SetAnimation(1, "attack-charge", false);
          }
          else
          {
            if ((double) (enemyFollower.RepathTimer += Time.deltaTime) > 0.20000000298023224)
            {
              enemyFollower.RepathTimer = 0.0f;
              enemyFollower.givePath(enemyFollower.TargetObject.transform.position);
            }
            if ((UnityEngine.Object) enemyFollower.damageColliderEvents != (UnityEngine.Object) null)
            {
              if ((double) enemyFollower.state.Timer < 0.20000000298023224 && !enemyFollower.health.WasJustParried)
                enemyFollower.damageColliderEvents.SetActive(true);
              else
                enemyFollower.damageColliderEvents.SetActive(false);
            }
          }
          if ((UnityEngine.Object) enemyFollower.damageColliderEvents != (UnityEngine.Object) null)
          {
            enemyFollower.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          if ((UnityEngine.Object) enemyFollower.damageColliderEvents != (UnityEngine.Object) null)
            enemyFollower.damageColliderEvents.SetActive(false);
          enemyFollower.SimpleSpineFlash.FlashWhite(enemyFollower.state.Timer / SignPostDelay);
          enemyFollower.state.Timer += Time.deltaTime;
          if ((double) enemyFollower.state.Timer >= (double) SignPostDelay - (double) EnemyFollower.signPostParryWindow)
            enemyFollower.canBeParried = true;
          if ((double) enemyFollower.state.Timer >= (double) SignPostDelay)
          {
            enemyFollower.SimpleSpineFlash.FlashWhite(false);
            CameraManager.shakeCamera(0.4f, enemyFollower.state.LookAngle);
            enemyFollower.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemyFollower.speed = AttackSpeed * 0.0166666675f;
            enemyFollower.Spine.AnimationState.SetAnimation(1, "attack-impact", false);
            enemyFollower.canBeParried = true;
            enemyFollower.StartCoroutine((IEnumerator) enemyFollower.EnableDamageCollider(0.0f));
            if (!string.IsNullOrEmpty(enemyFollower.attackSoundPath))
            {
              AudioManager.Instance.PlayOneShot(enemyFollower.attackSoundPath, enemyFollower.transform.position);
              break;
            }
            break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) AttackSpeed > 0.0)
            AttackSpeed -= 1f * GameManager.DeltaTime;
          enemyFollower.speed = AttackSpeed * Time.deltaTime;
          enemyFollower.canBeParried = (double) enemyFollower.state.Timer <= (double) EnemyFollower.attackParryWindow;
          if ((double) (enemyFollower.state.Timer += Time.deltaTime) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
          {
            if (++AttackCount <= NumAttacks)
            {
              AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
              enemyFollower.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyFollower.Spine.AnimationState.SetAnimation(1, "attack-charge", false);
              SignPostDelay = 0.3f;
              break;
            }
            Loop = false;
            enemyFollower.SimpleSpineFlash.FlashWhite(false);
            break;
          }
          break;
      }
      yield return (object) null;
    }
    enemyFollower.StartCoroutine((IEnumerator) enemyFollower.ChasePlayer());
  }

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  private IEnumerator EnableDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.damageColliderEvents)
    {
      yield return (object) new WaitForSeconds(initialDelay);
      this.damageColliderEvents.SetActive(true);
      yield return (object) new WaitForSeconds(0.2f);
      this.damageColliderEvents.SetActive(false);
    }
  }

  private enum State
  {
    WaitAndTaunt,
    Teleporting,
    Attacking,
  }
}
