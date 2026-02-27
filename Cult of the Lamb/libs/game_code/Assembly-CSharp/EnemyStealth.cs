// Decompiled with JetBrains decompiler
// Type: EnemyStealth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyStealth : BaseMonoBehaviour
{
  public EnemyStealth.Activity StartingActivity;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string StartingAnimation = "idle";
  public bool LoopAnimation = true;
  public List<Vector3> PatrolRoute = new List<Vector3>();
  public float PatrolSpeed = 0.02f;
  public float WaitBetweenPatrolPoints = 0.5f;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string PatrolWalk = "run";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string PatrolIdle = "idle";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SleepingAnimation = "sleeping";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string WakeUpAnimation = "wake-up";
  public LayerMask DetectionLayer;
  public Vector3 StartPosition;
  public int Patrol;
  public float RepathTimer;
  public UnitObject unitObject;
  public UnitObject AIScriptToDisable;
  public StateMachine state;
  [HideInInspector]
  public Health health;
  [SerializeField]
  public float alarmTime = 1.8f;
  [SerializeField]
  public float warnTime = 1f;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RaiseAlarm;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string DetectPlayer;
  public SkeletonAnimation warningIcon;
  public GameObject EnemyStealthUIPrefab;
  public EnemyStealthUI EnemyStealthUI;
  public static List<EnemyStealth> EnemyStealths = new List<EnemyStealth>();
  public float EnterRadius = 5f;
  public float ExitRadius = 6f;
  public float _AlertLevel;
  public float AlertLimit = 3f;
  public Coroutine cPatrolRoutine;
  public float Distance;
  public float VisionRage = 8f;
  public float VisionConeAngle = 40f;
  public float CloseConeAngle = 120f;
  public float DetectEvenIfStealth = 3f;
  public int VisibleEnemies;
  public Health EnemyHealth;

  public float AlertLevel
  {
    get => this._AlertLevel;
    set => this._AlertLevel = Mathf.Clamp(value, 0.0f, this.AlertLimit);
  }

  public void Start()
  {
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
    this.AIScriptToDisable.enabled = false;
    this.EnemyStealthUI = Object.Instantiate<GameObject>(this.EnemyStealthUIPrefab, this.transform).GetComponent<EnemyStealthUI>();
    this.EnemyStealthUI.UpdateProgress(0.0f);
    this.health.Unaware = true;
    this.StartPosition = this.transform.position;
    this.PatrolRoute.Insert(0, Vector3.zero);
    this.Spine.Initialize(false);
    switch (this.StartingActivity)
    {
      case EnemyStealth.Activity.Animation:
        this.Spine.AnimationState.SetAnimation(0, this.StartingAnimation, this.LoopAnimation);
        this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
        break;
      case EnemyStealth.Activity.Patrol:
        if (this.cPatrolRoutine != null)
          this.StopCoroutine(this.cPatrolRoutine);
        this.cPatrolRoutine = this.StartCoroutine(this.PatrolRoutine());
        break;
      case EnemyStealth.Activity.Sleep:
        this.Spine.AnimationState.SetAnimation(0, this.SleepingAnimation, true);
        this.DetectPlayer = this.WakeUpAnimation;
        break;
    }
  }

  public void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.health.Unaware)
      return;
    this.ClearPatrol();
    if (Health.team2.Count > 1)
      this.StartCoroutine(this.RaiseAlarmRoutine(this.EnemyHealth));
    else
      this.StartCoroutine(this.BeWarnedRoutine(this.EnemyHealth, 0.0f));
  }

  public void OnEnable() => EnemyStealth.EnemyStealths.Add(this);

  public void OnDisable()
  {
    EnemyStealth.EnemyStealths.Remove(this);
    this.ClearPatrol();
    if (!((Object) this.state != (Object) null))
      return;
    this.state.OnStateChange -= new StateMachine.StateChange(this.OnStateChange);
  }

  public void OnDestroy()
  {
    if (!((Object) this.EnemyStealthUI != (Object) null))
      return;
    Object.Destroy((Object) this.EnemyStealthUI.gameObject);
  }

  public void ClearPatrol()
  {
    if (this.cPatrolRoutine != null)
      this.StopCoroutine(this.cPatrolRoutine);
    if ((Object) this.unitObject != (Object) null)
      Object.Destroy((Object) this.unitObject);
    this.unitObject = (UnitObject) null;
  }

  public IEnumerator PatrolRoutine()
  {
    EnemyStealth enemyStealth1 = this;
    if ((Object) enemyStealth1.unitObject == (Object) null)
      enemyStealth1.unitObject = enemyStealth1.gameObject.AddComponent<UnitObject>();
    enemyStealth1.unitObject.maxSpeed = enemyStealth1.PatrolSpeed;
    enemyStealth1.unitObject.distanceBetweenDustClouds = 1f;
    enemyStealth1.state.OnStateChange += new StateMachine.StateChange(enemyStealth1.OnStateChange);
    while (true)
    {
      enemyStealth1.state.LookAngle = enemyStealth1.state.facingAngle;
      if (enemyStealth1.unitObject.pathToFollow == null)
      {
        yield return (object) new WaitForSeconds(enemyStealth1.WaitBetweenPatrolPoints);
        EnemyStealth enemyStealth2 = enemyStealth1;
        EnemyStealth enemyStealth3 = enemyStealth1;
        int num1 = enemyStealth1.Patrol + 1;
        int num2 = num1;
        enemyStealth3.Patrol = num2;
        int num3 = num1 % enemyStealth1.PatrolRoute.Count;
        enemyStealth2.Patrol = num3;
        enemyStealth1.unitObject.givePath(enemyStealth1.StartPosition + enemyStealth1.PatrolRoute[enemyStealth1.Patrol]);
      }
      else if ((double) (enemyStealth1.RepathTimer += Time.deltaTime) > 0.5)
      {
        enemyStealth1.unitObject.givePath(enemyStealth1.StartPosition + enemyStealth1.PatrolRoute[enemyStealth1.Patrol]);
        enemyStealth1.RepathTimer = 0.0f;
      }
      yield return (object) null;
    }
  }

  public void OnStateChange(StateMachine.State NewState, StateMachine.State PrevState)
  {
    if (NewState != StateMachine.State.Idle)
    {
      if (NewState != StateMachine.State.Moving)
        return;
      this.Spine.AnimationState.SetAnimation(0, this.PatrolWalk, true);
    }
    else
      this.Spine.AnimationState.SetAnimation(0, this.PatrolIdle, true);
  }

  public void Update()
  {
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    if (!this.health.Unaware || this.StartingActivity == EnemyStealth.Activity.Sleep || !GameManager.RoomActive)
      return;
    this.state.facingAngle = this.state.LookAngle;
    this.VisibleEnemies = 0;
    foreach (Health health in Health.playerTeam)
    {
      this.Distance = Vector3.Distance(this.transform.position, health.transform.position);
      if ((Object) health != (Object) null && !health.InStealthCover && (double) this.Distance < (double) this.VisionRage)
      {
        if ((double) this.Distance < (double) this.DetectEvenIfStealth)
        {
          RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (health.transform.position - this.transform.position), this.VisionRage, (int) this.DetectionLayer);
          if ((Object) raycastHit2D.collider != (Object) null && (Object) raycastHit2D.collider.gameObject == (Object) health.gameObject)
          {
            ++this.VisibleEnemies;
            this.AlertLevel += Time.deltaTime * 10f;
            this.EnemyHealth = health;
          }
        }
        else
        {
          RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (health.transform.position - this.transform.position), this.VisionRage, (int) this.DetectionLayer);
          if ((Object) raycastHit2D.collider != (Object) null && (Object) raycastHit2D.collider.gameObject == (Object) health.gameObject)
          {
            ++this.VisibleEnemies;
            this.EnemyHealth = health;
            if (health.state.CURRENT_STATE == StateMachine.State.Stealth && (double) this.Distance > (double) this.DetectEvenIfStealth)
              this.AlertLevel += Time.deltaTime * 2f;
            else
              this.AlertLevel += Time.deltaTime * 10f;
          }
        }
      }
    }
    if (this.VisibleEnemies <= 0 && (double) this.AlertLevel < (double) this.AlertLimit)
      this.AlertLevel -= Time.deltaTime * 2f;
    this.EnemyStealthUI.UpdateProgress(this.AlertLevel / this.AlertLimit);
    if ((double) this.AlertLevel <= 0.0)
      return;
    this.ClearPatrol();
    foreach (EnemyStealth enemyStealth in EnemyStealth.EnemyStealths)
    {
      if (enemyStealth.health.Unaware && (double) Vector3.Distance(this.transform.position, enemyStealth.transform.position) < 15.0)
        enemyStealth.BeWarned(this.EnemyHealth, 0.0f);
    }
  }

  public IEnumerator RaiseAlarmRoutine(Health TargetObject)
  {
    EnemyStealth enemyStealth1 = this;
    if ((Object) enemyStealth1.EnemyStealthUI != (Object) null)
      Object.Destroy((Object) enemyStealth1.EnemyStealthUI.gameObject);
    if ((Object) TargetObject != (Object) null)
      enemyStealth1.state.facingAngle = Utils.GetAngle(enemyStealth1.transform.position, TargetObject.transform.position);
    enemyStealth1.health.Unaware = false;
    enemyStealth1.state.CURRENT_STATE = StateMachine.State.RaiseAlarm;
    enemyStealth1.Spine.AnimationState.SetAnimation(0, enemyStealth1.RaiseAlarm, false);
    foreach (EnemyStealth enemyStealth2 in EnemyStealth.EnemyStealths)
    {
      if ((Object) enemyStealth2 != (Object) null && (Object) enemyStealth2.health != (Object) null && enemyStealth2.health.Unaware && (double) Vector3.Distance(enemyStealth1.transform.position, enemyStealth2.transform.position) < 15.0)
        enemyStealth2.BeWarned(TargetObject, 1.3f);
    }
    yield return (object) new WaitForSeconds(enemyStealth1.alarmTime);
    if ((Object) enemyStealth1.AIScriptToDisable != (Object) null)
      enemyStealth1.AIScriptToDisable.enabled = true;
    enemyStealth1.enabled = false;
  }

  public void BeWarned(Health TargetObject, float Delay)
  {
    this.StartCoroutine(this.BeWarnedRoutine(TargetObject, Delay));
  }

  public IEnumerator BeWarnedRoutine(Health TargetObject, float Delay)
  {
    EnemyStealth enemyStealth = this;
    enemyStealth.health.Unaware = false;
    if ((Object) enemyStealth.EnemyStealthUI != (Object) null)
      Object.Destroy((Object) enemyStealth.EnemyStealthUI.gameObject);
    if ((double) enemyStealth.AlertLevel <= (double) enemyStealth.AlertLimit * 0.30000001192092896)
      yield return (object) new WaitForSeconds(Delay);
    if ((Object) TargetObject != (Object) null)
      enemyStealth.state.facingAngle = Utils.GetAngle(enemyStealth.transform.position, TargetObject.transform.position);
    enemyStealth.state.CURRENT_STATE = StateMachine.State.RaiseAlarm;
    enemyStealth.Spine.AnimationState.SetAnimation(0, enemyStealth.DetectPlayer, false);
    yield return (object) new WaitForSeconds(0.17f);
    if ((Object) enemyStealth.warningIcon != (Object) null)
    {
      enemyStealth.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
      enemyStealth.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, 2f);
    }
    yield return (object) new WaitForSeconds(enemyStealth.warnTime);
    if ((Object) enemyStealth.AIScriptToDisable != (Object) null)
    {
      enemyStealth.AIScriptToDisable.enabled = true;
      if ((Object) TargetObject != (Object) null)
        enemyStealth.AIScriptToDisable.BeAlarmed(TargetObject.gameObject);
    }
    enemyStealth.enabled = false;
  }

  public void OnDrawGizmos()
  {
    if (this.StartingActivity != EnemyStealth.Activity.Patrol)
      return;
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

  public enum Activity
  {
    Animation,
    Patrol,
    Sleep,
  }
}
