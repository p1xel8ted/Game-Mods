// Decompiled with JetBrains decompiler
// Type: Task_Archer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_Archer : Task
{
  public Health health;
  public Health EnemyHealth;
  public float DetectEnemyRange;
  public float AttackRange;
  public float LoseEnemyRange;
  public float PreAttackDuration;
  public float PostAttackDuration;
  public float MinimumRange;
  public float DefaultMinimumRange;
  public GameObject Arrow;

  public Task_Archer() => this.Type = Task_Type.ARCHER;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.health = t.gameObject.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void Init(
    float DetectEnemyRange,
    float AttackRange,
    float LoseEnemyRange,
    float PreAttackDuration,
    float PostAttackDuration,
    float MinimumRange,
    GameObject Arrow)
  {
    this.DetectEnemyRange = DetectEnemyRange;
    this.AttackRange = AttackRange;
    this.LoseEnemyRange = LoseEnemyRange;
    this.PreAttackDuration = PreAttackDuration;
    this.PostAttackDuration = PostAttackDuration;
    this.DefaultMinimumRange = this.MinimumRange = MinimumRange;
    this.Arrow = Arrow;
  }

  public Vector3 TargetPosition()
  {
    return (Object) this.TargetObject == (Object) null ? Vector3.zero : this.TargetObject.transform.position;
  }

  public override void TaskUpdate()
  {
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
          if (!this.t.IsPathPossible(this.t.transform.position, this.TargetPosition()))
            break;
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetPosition());
          this.MinimumRange = this.DefaultMinimumRange + (float) Random.Range(-2, 2);
          break;
        case StateMachine.State.Moving:
          float num = Vector2.Distance((Vector2) this.TargetPosition(), (Vector2) this.t.transform.position);
          if ((double) num > (double) this.LoseEnemyRange || (double) num < (double) this.MinimumRange || !this.t.IsPathPossible(this.t.transform.position, this.TargetPosition()))
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            this.ClearTarget();
            break;
          }
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.ClearTarget();
          this.GetNewTarget();
          this.PathToPosition(this.TargetPosition());
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
          this.DoAttack(this.AttackRange, StateMachine.State.RecoverFromAttack);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.PostAttackDuration)
            break;
          this.Timer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          if (!((Object) this.TargetObject == (Object) null))
            break;
          this.ClearTarget();
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

  public override void DoAttack(float AttackRange, StateMachine.State NextState = StateMachine.State.RecoverFromAttack)
  {
    this.Timer = 0.0f;
    this.state.CURRENT_STATE = NextState;
    Projectile component = ObjectPool.Spawn(this.Arrow, this.t.transform.parent).GetComponent<Projectile>();
    component.transform.position = this.t.transform.position;
    component.Angle = this.state.facingAngle;
    component.team = this.health.team;
    component.Owner = this.health;
  }

  public void GetNewTarget()
  {
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.t.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.t.transform.position)))
      {
        float num2 = Vector3.Distance(this.t.transform.position, allUnit.gameObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          health = allUnit;
          num1 = num2;
        }
      }
    }
    if (!((Object) health != (Object) null))
      return;
    this.TargetObject = health.gameObject;
    this.EnemyHealth = health;
    this.EnemyHealth.attackers.Add(this.t.gameObject);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.ClearTarget();
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public override void ClearTask()
  {
    this.ClearTarget();
    base.ClearTask();
  }

  public void ClearTarget()
  {
    if ((Object) this.EnemyHealth != (Object) null)
      this.EnemyHealth.attackers.Remove(this.t.gameObject);
    this.TargetObject = (GameObject) null;
    this.EnemyHealth = (Health) null;
    this.Timer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }
}
