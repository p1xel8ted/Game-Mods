// Decompiled with JetBrains decompiler
// Type: Task_SleepOutside
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_SleepOutside : Task
{
  public Vector3 SleepingPosition;
  public WorshipperInfoManager wim;
  public bool DoYawn = true;
  public Worshipper w;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    t.GetComponent<SimpleInventory>().DropItem();
    this.Type = Task_Type.SLEEP;
    this.w = t.GetComponent<Worshipper>();
  }

  public void FindNewSleepingPosition()
  {
    this.wim = this.t.GetComponent<WorshipperInfoManager>();
    Task_SleepOutside.SleepOutsidePosition(this.wim.v_i);
    this.SleepingPosition = this.wim.v_i.SleptOutsidePosition;
  }

  public static void SleepOutsidePosition(Villager_Info v_i)
  {
    v_i.SleptOutside = true;
    v_i.SleptOutsidePosition = TownCentre.Instance.RandomPositionInTownCentre();
  }

  public void EndOfPath()
  {
    this.Timer = 0.0f;
    if ((UnityEngine.Object) this.w != (UnityEngine.Object) null && this.DoYawn)
    {
      this.w.bubble.Play(WorshipperBubble.SPEECH_TYPE.HOME);
      this.w.TimedAnimation("sleepy", 3.8f, new System.Action(this.DoSleep));
    }
    else
      this.DoSleep();
    this.DoYawn = true;
    AstarPath.active.GetNearest(this.t.transform.position).node.Walkable = false;
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.EndOfPath);
  }

  public void DoSleep()
  {
    this.state.CURRENT_STATE = StateMachine.State.Sleeping;
    this.w.SetAnimation("sleep", true);
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    if (this.state.CURRENT_STATE != StateMachine.State.Sleeping)
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector3.Distance(this.SleepingPosition, this.t.transform.position) > (double) Farm.FarmTileSize)
          {
            this.Timer = 0.0f;
            this.PathToPosition(this.SleepingPosition);
            TaskDoer t = this.t;
            t.EndOfPath = t.EndOfPath + new System.Action(this.EndOfPath);
            break;
          }
          this.EndOfPath();
          break;
        case StateMachine.State.Moving:
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.PathToPosition(this.SleepingPosition);
          break;
      }
    }
    else
    {
      this.w.wim.v_i.Sleep += Time.deltaTime * 5f;
      if ((double) this.w.wim.v_i.Sleep < 60.0)
        return;
      this.ClearTask();
      this.t.ClearTask();
    }
  }

  public override void ClearTask()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Sleeping && (UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
      AstarPath.active.GetNearest(this.t.transform.position).node.Walkable = true;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.EndOfPath);
    base.ClearTask();
  }
}
