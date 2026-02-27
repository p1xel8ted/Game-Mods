// Decompiled with JetBrains decompiler
// Type: EnemySwordsman
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemySwordsman : UnitObject
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
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  private GameObject TargetObject;
  private float AttackDelay;
  private bool canBeParried;
  private static float signPostParryWindow = 0.2f;
  private static float attackParryWindow = 0.15f;
  protected Vector3 Force;
  public float KnockbackModifier = 1f;
  [HideInInspector]
  public float Angle;
  private Vector3 TargetPosition;
  private float RepathTimer;
  private float TeleportDelay;
  private EnemySwordsman.State MyState;
  private float MaxAttackDelay;
  private Health EnemyHealth;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Awake()
  {
    base.Awake();
    if (!((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null))
      return;
    this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
  }

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
    this.rb.simulated = true;
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
    this.DisableForces = false;
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
    this.DisableForces = false;
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
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.Spine.AnimationState.SetAnimation(1, "hurt-eyes", false);
    if (this.MyState != EnemySwordsman.State.Attacking)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.StopAllCoroutines();
      this.DisableForces = false;
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
    EnemySwordsman enemySwordsman = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemySwordsman.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemySwordsman.DisableForces = true;
    enemySwordsman.Force = (enemySwordsman.transform.position - Attacker.transform.position).normalized * 500f;
    enemySwordsman.rb.AddForce((Vector2) (enemySwordsman.Force * enemySwordsman.KnockbackModifier));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator HurtRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemySwordsman enemySwordsman = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
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

  protected IEnumerator WaitForTarget()
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemySwordsman.GetClosestTarget();
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemySwordsman.TargetObject = closestTarget.gameObject;
        enemySwordsman.requireLineOfSite = false;
        enemySwordsman.VisionRange = int.MaxValue;
      }
      enemySwordsman.RepathTimer -= Time.deltaTime;
      if ((double) enemySwordsman.RepathTimer <= 0.0)
      {
        if (enemySwordsman.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemySwordsman.Spine.AnimationName != "run")
            enemySwordsman.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemySwordsman.Spine.AnimationName != "idle")
          enemySwordsman.Spine.AnimationState.SetAnimation(0, "idle", true);
        enemySwordsman.TargetPosition = enemySwordsman.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        enemySwordsman.FindPath(enemySwordsman.TargetPosition);
        enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, enemySwordsman.TargetPosition);
        enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
      }
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) null)
      {
        enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(enemySwordsman.TargetObject.transform.position, enemySwordsman.transform.position);
      if ((double) a <= (double) enemySwordsman.VisionRange)
      {
        if (!enemySwordsman.requireLineOfSite || enemySwordsman.CheckLineOfSight(enemySwordsman.TargetObject.transform.position, Mathf.Min(a, (float) enemySwordsman.VisionRange)))
          InRange = true;
        else
          enemySwordsman.LookAtTarget();
      }
      yield return (object) null;
    }
    enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.ChasePlayer());
  }

  private void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    if (!(this.Spine.AnimationName != "jeer"))
      return;
    this.Spine.randomOffset = true;
    this.Spine.AnimationState.SetAnimation(0, "jeer", true);
  }

  public IEnumerator ChasePlayer()
  {
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.MyState = EnemySwordsman.State.WaitAndTaunt;
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsman.AttackDelay = UnityEngine.Random.Range(enemySwordsman.AttackDelayRandomRange.x, enemySwordsman.AttackDelayRandomRange.y);
    if (enemySwordsman.health.HasShield)
      enemySwordsman.AttackDelay = 2.5f;
    enemySwordsman.MaxAttackDelay = UnityEngine.Random.Range(enemySwordsman.MaxAttackDelayRandomRange.x, enemySwordsman.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) null)
      {
        enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
        break;
      }
      if ((UnityEngine.Object) enemySwordsman.damageColliderEvents != (UnityEngine.Object) null)
        enemySwordsman.damageColliderEvents.SetActive(false);
      enemySwordsman.TeleportDelay -= Time.deltaTime;
      enemySwordsman.AttackDelay -= Time.deltaTime;
      enemySwordsman.MaxAttackDelay -= Time.deltaTime;
      if (enemySwordsman.MyState == EnemySwordsman.State.WaitAndTaunt)
      {
        if (enemySwordsman.Spine.AnimationName != "roll-stop" && enemySwordsman.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemySwordsman.Spine.AnimationName != "run")
            enemySwordsman.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemySwordsman.Spine.AnimationName != "cheer1")
          enemySwordsman.Spine.AnimationState.SetAnimation(0, "cheer1", true);
        if ((UnityEngine.Object) enemySwordsman.TargetObject == (UnityEngine.Object) PlayerFarming.Instance.gameObject && enemySwordsman.health.IsCharmed && (UnityEngine.Object) enemySwordsman.GetClosestTarget() != (UnityEngine.Object) null)
          enemySwordsman.TargetObject = enemySwordsman.GetClosestTarget().gameObject;
        enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position);
        enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
        if (enemySwordsman.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          if ((double) (enemySwordsman.RepathTimer -= Time.deltaTime) < 0.0)
          {
            if (enemySwordsman.CustomAttackLogic())
              break;
            if ((double) enemySwordsman.MaxAttackDelay < 0.0 || (double) Vector3.Distance(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position) < (double) enemySwordsman.AttackWithinRange)
            {
              enemySwordsman.AttackWithinRange = float.MaxValue;
              if ((bool) (UnityEngine.Object) enemySwordsman.TargetObject)
              {
                if (enemySwordsman.ChargeAndAttack && ((double) enemySwordsman.MaxAttackDelay < 0.0 || (double) enemySwordsman.AttackDelay < 0.0))
                {
                  enemySwordsman.health.invincible = false;
                  enemySwordsman.StopAllCoroutines();
                  enemySwordsman.DisableForces = false;
                  enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.FightPlayer());
                }
                else if (!enemySwordsman.health.HasShield)
                {
                  enemySwordsman.Angle = (float) (((double) Utils.GetAngle(enemySwordsman.TargetObject.transform.position, enemySwordsman.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                  enemySwordsman.TargetPosition = enemySwordsman.TargetObject.transform.position + new Vector3(enemySwordsman.MaintainTargetDistance * Mathf.Cos(enemySwordsman.Angle), enemySwordsman.MaintainTargetDistance * Mathf.Sin(enemySwordsman.Angle));
                  enemySwordsman.FindPath(enemySwordsman.TargetPosition);
                }
              }
            }
            else if ((bool) (UnityEngine.Object) enemySwordsman.TargetObject && (double) Vector3.Distance(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position) > (double) enemySwordsman.MoveCloserDistance + (enemySwordsman.health.HasShield ? 0.0 : 1.0))
            {
              enemySwordsman.Angle = (float) (((double) Utils.GetAngle(enemySwordsman.TargetObject.transform.position, enemySwordsman.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemySwordsman.TargetPosition = enemySwordsman.TargetObject.transform.position + new Vector3(enemySwordsman.MaintainTargetDistance * Mathf.Cos(enemySwordsman.Angle), enemySwordsman.MaintainTargetDistance * Mathf.Sin(enemySwordsman.Angle));
              enemySwordsman.FindPath(enemySwordsman.TargetPosition);
            }
          }
        }
        else if ((double) (enemySwordsman.RepathTimer += Time.deltaTime) > 2.0)
        {
          enemySwordsman.RepathTimer = 0.0f;
          enemySwordsman.state.CURRENT_STATE = StateMachine.State.Idle;
        }
      }
      enemySwordsman.Seperate(0.5f);
      yield return (object) null;
    }
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    if (!string.IsNullOrEmpty(this.WarningVO))
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
    this.TargetObject = TargetObject;
    float a = Vector3.Distance(TargetObject.transform.position, this.transform.position);
    if ((double) a > (double) this.VisionRange)
      return;
    if (!this.requireLineOfSite || this.CheckLineOfSight(TargetObject.transform.position, Mathf.Min(a, (float) this.VisionRange)))
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    else
      this.LookAtTarget();
  }

  public virtual bool CustomAttackLogic() => false;

  private void FindPath(Vector3 PointToCheck)
  {
    this.RepathTimer = 0.2f;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.NewPositionDistance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) > 1.0)
      {
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
        if ((double) this.TeleportDelay >= 0.0)
          return;
        this.Teleport();
      }
    }
    else
    {
      this.TargetPosition = PointToCheck;
      this.givePath(PointToCheck);
    }
  }

  private void Teleport()
  {
    if (this.MyState != EnemySwordsman.State.WaitAndTaunt || (double) this.health.HP <= 0.0)
      return;
    float num1 = 100f;
    float num2;
    if ((double) (num2 = num1 - 1f) <= 0.0 || (UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
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
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.ClearPaths();
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.Moving;
    enemySwordsman.UsePathing = false;
    enemySwordsman.health.invincible = true;
    enemySwordsman.SeperateObject = false;
    enemySwordsman.MyState = EnemySwordsman.State.Teleporting;
    enemySwordsman.ClearPaths();
    Vector3 position = enemySwordsman.transform.position;
    float Progress = 0.0f;
    enemySwordsman.Spine.AnimationState.SetAnimation(0, "roll", true);
    enemySwordsman.state.facingAngle = enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, Position);
    enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 b = Position;
    float Duration = Vector3.Distance(position, b) / 10f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      enemySwordsman.speed = 10f * Time.deltaTime;
      yield return (object) null;
    }
    enemySwordsman.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsman.Spine.AnimationState.SetAnimation(0, "roll-stop", false);
    yield return (object) new WaitForSeconds(0.3f);
    enemySwordsman.UsePathing = true;
    enemySwordsman.RepathTimer = 0.5f;
    enemySwordsman.TeleportDelay = enemySwordsman.TeleportDelayTarget;
    enemySwordsman.SeperateObject = true;
    enemySwordsman.health.invincible = false;
    enemySwordsman.MyState = EnemySwordsman.State.WaitAndTaunt;
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
    EnemySwordsman enemySwordsman = this;
    enemySwordsman.MyState = EnemySwordsman.State.Attacking;
    enemySwordsman.UsePathing = true;
    enemySwordsman.givePath(enemySwordsman.TargetObject.transform.position);
    enemySwordsman.Spine.AnimationState.SetAnimation(0, "run-charge", true);
    enemySwordsman.RepathTimer = 0.0f;
    int NumAttacks = enemySwordsman.DoubleAttack ? 2 : 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = 0.5f;
    while (Loop)
    {
      if ((UnityEngine.Object) enemySwordsman.Spine == (UnityEngine.Object) null || enemySwordsman.Spine.AnimationState == null || enemySwordsman.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        enemySwordsman.Seperate(0.5f);
        switch (enemySwordsman.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            enemySwordsman.TargetObject = (GameObject) null;
            enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
            yield break;
          case StateMachine.State.Moving:
            if ((bool) (UnityEngine.Object) enemySwordsman.TargetObject)
            {
              enemySwordsman.state.LookAngle = Utils.GetAngle(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position);
              enemySwordsman.Spine.skeleton.ScaleX = (double) enemySwordsman.state.LookAngle <= 90.0 || (double) enemySwordsman.state.LookAngle >= 270.0 ? -1f : 1f;
              enemySwordsman.state.LookAngle = enemySwordsman.state.facingAngle = Utils.GetAngle(enemySwordsman.transform.position, enemySwordsman.TargetObject.transform.position);
            }
            if ((UnityEngine.Object) enemySwordsman.TargetObject != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) enemySwordsman.transform.position, (Vector2) enemySwordsman.TargetObject.transform.position) < (double) AttackDistance)
            {
              enemySwordsman.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemySwordsman.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "grunt-attack-charge2" : "grunt-attack-charge", false);
            }
            else
            {
              if ((double) (enemySwordsman.RepathTimer += Time.deltaTime) > 0.20000000298023224 && (bool) (UnityEngine.Object) enemySwordsman.TargetObject)
              {
                enemySwordsman.RepathTimer = 0.0f;
                enemySwordsman.givePath(enemySwordsman.TargetObject.transform.position);
              }
              if ((UnityEngine.Object) enemySwordsman.damageColliderEvents != (UnityEngine.Object) null)
              {
                if ((double) enemySwordsman.state.Timer < 0.20000000298023224 && !enemySwordsman.health.WasJustParried)
                  enemySwordsman.damageColliderEvents.SetActive(true);
                else
                  enemySwordsman.damageColliderEvents.SetActive(false);
              }
            }
            if ((UnityEngine.Object) enemySwordsman.damageColliderEvents != (UnityEngine.Object) null)
            {
              enemySwordsman.damageColliderEvents.SetActive(false);
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemySwordsman.damageColliderEvents != (UnityEngine.Object) null)
              enemySwordsman.damageColliderEvents.SetActive(false);
            enemySwordsman.SimpleSpineFlash.FlashWhite(enemySwordsman.state.Timer / SignPostDelay);
            enemySwordsman.state.Timer += Time.deltaTime;
            if ((double) enemySwordsman.state.Timer >= (double) SignPostDelay - (double) EnemySwordsman.signPostParryWindow)
              enemySwordsman.canBeParried = true;
            if ((double) enemySwordsman.state.Timer >= (double) SignPostDelay)
            {
              enemySwordsman.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, enemySwordsman.state.LookAngle);
              enemySwordsman.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              enemySwordsman.speed = AttackSpeed * 0.0166666675f;
              enemySwordsman.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "grunt-attack-impact2" : "grunt-attack-impact", false);
              enemySwordsman.canBeParried = true;
              enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.EnableDamageCollider(0.0f));
              if (!string.IsNullOrEmpty(enemySwordsman.attackSoundPath))
                AudioManager.Instance.PlayOneShot(enemySwordsman.attackSoundPath, enemySwordsman.transform.position);
              if (!string.IsNullOrEmpty(enemySwordsman.AttackVO))
              {
                AudioManager.Instance.PlayOneShot(enemySwordsman.AttackVO, enemySwordsman.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime;
            enemySwordsman.speed = AttackSpeed * Time.deltaTime;
            enemySwordsman.SimpleSpineFlash.FlashWhite(false);
            enemySwordsman.canBeParried = (double) enemySwordsman.state.Timer <= (double) EnemySwordsman.attackParryWindow;
            if ((double) (enemySwordsman.state.Timer += Time.deltaTime) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
            {
              if (++AttackCount <= NumAttacks)
              {
                AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
                enemySwordsman.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                enemySwordsman.Spine.AnimationState.SetAnimation(0, "grunt-attack-charge2", false);
                SignPostDelay = 0.3f;
                break;
              }
              Loop = false;
              enemySwordsman.SimpleSpineFlash.FlashWhite(false);
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    enemySwordsman.TargetObject = (GameObject) null;
    enemySwordsman.StartCoroutine((IEnumerator) enemySwordsman.WaitForTarget());
  }

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
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
