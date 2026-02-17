// Decompiled with JetBrains decompiler
// Type: Task_Combat_Shield
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_Combat_Shield : Task_Combat
{
  public float ChargeRange = 5f;
  public float ChargeDuration = 0.3f;
  public float ChargeDelay;
  public float HitTimer;
  public float DefendTimer;

  public Task_Combat_Shield() => this.Type = Task_Type.SHIELD;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.health = t.gameObject.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public new void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.ClearTarget();
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public override void TaskUpdate()
  {
    this.thisChecked = 0;
    this.ChargeDelay -= Time.deltaTime;
    if ((Object) this.TargetObject != (Object) null)
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector2.Distance((Vector2) this.TargetPosition(), (Vector2) this.t.transform.position) < (double) this.AttackRange)
          {
            this.Timer = 0.0f;
            this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
            this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            break;
          }
          this.Timer = 0.0f;
          this.SortAttackPosition();
          this.PathToPosition(this.TargetPosition());
          break;
        case StateMachine.State.Moving:
          if ((double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) this.TargetObject.transform.position) > (double) this.LoseEnemyRange)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            this.ClearTarget();
            break;
          }
          if ((double) this.ChargeDelay < 0.0 && (double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) this.TargetObject.transform.position) < (double) this.ChargeRange)
          {
            this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
            this.Timer = 0.0f;
            this.state.CURRENT_STATE = StateMachine.State.Charging;
            break;
          }
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.ClearTarget();
          this.GetNewTarget();
          this.SortAttackPosition();
          this.PathToPosition(this.TargetPosition());
          break;
        case StateMachine.State.Defending:
          if ((double) (this.DefendTimer += Time.deltaTime) < (double) this.DefendingDuration)
            break;
          this.DoCounterAttack();
          this.DefendTimer = 0.0f;
          break;
        case StateMachine.State.SignPostAttack:
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.PreAttackDuration)
            break;
          this.Timer = 0.0f;
          if ((Object) this.TargetObject == (Object) null)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          this.DoAttack(this.AttackRange);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.PostAttackDuration)
            break;
          this.Timer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Vulnerable;
          if (!((Object) this.TargetObject == (Object) null))
            break;
          this.ClearTarget();
          break;
        case StateMachine.State.HitLeft:
        case StateMachine.State.HitRight:
          if ((double) (this.HitTimer += Time.deltaTime) < 0.40000000596046448)
            break;
          this.DoCounterAttack();
          this.Timer = 0.0f;
          this.HitTimer = 0.0f;
          break;
        case StateMachine.State.RecoverFromCounterAttack:
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Vulnerable;
          if (!((Object) this.TargetObject == (Object) null))
            break;
          this.ClearTarget();
          break;
        case StateMachine.State.Charging:
          if ((double) (this.Timer += Time.deltaTime) > 0.20000000298023224)
          {
            this.t.speed = 0.2f;
            if ((double) Vector2.Distance((Vector2) this.TargetPosition(), (Vector2) this.t.transform.position) < (double) this.AttackRange)
            {
              this.Timer = 0.0f;
              this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
              this.DoAttack(this.AttackRange);
              this.ChargeDelay = 5f;
              break;
            }
            if ((double) this.Timer <= (double) this.ChargeDuration + 0.5)
              break;
            this.state.CURRENT_STATE = StateMachine.State.Vulnerable;
            this.ChargeDelay = 5f;
            break;
          }
          this.t.speed = 0.0f;
          break;
      }
    }
    else
    {
      this.GetNewTarget();
      if (!((Object) this.TargetObject == (Object) null))
        return;
      this.ClearTask();
    }
  }

  public void DoCounterAttack()
  {
    if ((Object) this.TargetObject == (Object) null)
    {
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    else
    {
      this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.TargetObject.transform.position);
      if ((double) Vector2.Distance((Vector2) this.TargetPosition(), (Vector2) this.t.transform.position) < (double) this.AttackRange)
        this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      else
        this.state.CURRENT_STATE = StateMachine.State.Charging;
    }
  }

  public override void ClearTask()
  {
    this.ClearTarget();
    base.ClearTask();
  }

  public new static int SortByY(Vector2 p1, Vector2 p2) => p1.y.CompareTo(p2.y);
}
