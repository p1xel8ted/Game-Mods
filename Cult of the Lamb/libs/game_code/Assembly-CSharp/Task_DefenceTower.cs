// Decompiled with JetBrains decompiler
// Type: Task_DefenceTower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Task_DefenceTower : Task
{
  public WorkPlace workplace;
  public WorshipperInfoManager wim;
  public Health health;
  public Health EnemyTarget;
  public float DetectEnemyRange = 5f;

  public Task_DefenceTower() => this.Type = Task_Type.DEFENCE_TOWER;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.wim = t.GetComponent<WorshipperInfoManager>();
    this.workplace = WorkPlace.GetWorkPlaceByID(this.wim.v_i.WorkPlace);
    this.health = t.GetComponent<Health>();
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if ((double) Vector3.Distance(this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, this.t.transform.position) > 0.5)
        {
          this.Timer = 0.0f;
          this.PathToPosition(this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position);
          TaskDoer t = this.t;
          t.EndOfPath = t.EndOfPath + new System.Action(this.ArriveAtWorkPlace);
          break;
        }
        this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
        break;
      case StateMachine.State.CustomAction0:
        this.Defend();
        if ((double) Vector3.Distance(this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, this.t.transform.position) <= 0.5)
          break;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        break;
    }
  }

  public void Defend()
  {
    if ((UnityEngine.Object) this.EnemyTarget == (UnityEngine.Object) null)
      this.GetTarget();
    else if ((double) (this.Timer -= Time.deltaTime) < 0.0)
    {
      Projectile component = ObjectPool.Spawn(Resources.Load("Prefabs/Weapons/Arrow") as GameObject, this.t.transform.parent).GetComponent<Projectile>();
      component.transform.position = this.t.transform.position + new Vector3(0.5f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f);
      component.Angle = this.state.facingAngle;
      component.team = this.health.team;
      component.Damage = 0.6f;
      component.Owner = this.health;
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      this.EnemyTarget = (Health) null;
    }
    else
      this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.EnemyTarget.transform.position);
  }

  public void GetTarget()
  {
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && allUnit.team != Health.Team.Neutral && allUnit.attackers.Count < 4 && (double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.t.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.t.transform.position)))
      {
        float num2 = Vector3.Distance(this.t.transform.position, allUnit.gameObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          health = allUnit;
          num1 = num2;
        }
      }
    }
    if (!((UnityEngine.Object) health != (UnityEngine.Object) null))
      return;
    health.attackers.Add(this.t.gameObject);
    this.TargetObject = health.gameObject;
    this.EnemyTarget = health;
    this.Timer = 2f;
  }

  public void ArriveAtWorkPlace()
  {
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.ArriveAtWorkPlace);
  }

  public override void ClearTask()
  {
    this.workplace.EndJob(this.t, this.wim.v_i.WorkPlaceSlot);
    base.ClearTask();
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.ArriveAtWorkPlace);
  }
}
