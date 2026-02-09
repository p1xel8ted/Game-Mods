// Decompiled with JetBrains decompiler
// Type: TaskDoer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TaskDoer : UnitObject
{
  [HideInInspector]
  public bool InConversation;
  public static List<TaskDoer> TaskDoers = new List<TaskDoer>();
  public Task _CurrentTask;
  public int Position;
  public List<Task> TaskList = new List<Task>();
  public System.Action OnCollision;

  public virtual Task CurrentTask
  {
    get => this._CurrentTask;
    set
    {
      value?.StartTask(this, (GameObject) null);
      this._CurrentTask = value;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Position = TaskDoer.TaskDoers.Count;
    TaskDoer.TaskDoers.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    TaskDoer.TaskDoers.Remove(this);
  }

  public static void AddNewTaskToTeam(Task_Type TaskType, Health.Team Team, bool ClearOnComplete)
  {
    foreach (TaskDoer taskDoer in TaskDoer.TaskDoers)
    {
      if (taskDoer.health.team == Team)
        taskDoer.AddNewTask(TaskType, ClearOnComplete);
    }
  }

  public void AddNewTask(Task_Type TaskType, bool ClearOnComplete)
  {
    if (this.CurrentTask != null)
      this.CurrentTask.ClearTask();
    this.CurrentTask = Task.GetTaskByType(TaskType);
    this.CurrentTask.ClearOnComplete = ClearOnComplete;
  }

  public void AddNewTask(Task task, bool ClearOnComplete)
  {
    if (this.CurrentTask != null)
      this.CurrentTask.ClearTask();
    this.CurrentTask = task;
    this.CurrentTask.ClearOnComplete = ClearOnComplete;
  }

  public void ClearTask() => this.CurrentTask = (Task) null;

  public override void Update() => base.Update();

  public void LookOutForEnemies()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && allUnit.team != Health.Team.Neutral && !allUnit.InanimateObject && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < 5.0 && this.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.transform.position)))
      {
        this.AddNewTask(Task_Type.COMBAT, true);
        break;
      }
    }
  }

  public void OnCollisionStay2D(Collision2D collision)
  {
    if (this.OnCollision == null)
      return;
    this.OnCollision();
  }
}
