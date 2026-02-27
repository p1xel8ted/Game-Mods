// Decompiled with JetBrains decompiler
// Type: Task_Dancer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_Dancer : Task
{
  private WorshipperInfoManager wim;
  private WorkPlace workplace;
  private Structure structure;
  private Worshipper w;
  private float PowerTimer;

  public Task_Dancer() => this.Type = Task_Type.DANCER;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.wim = t.GetComponent<WorshipperInfoManager>();
    this.workplace = WorkPlace.GetWorkPlaceByID(this.wim.v_i.WorkPlace);
    this.structure = this.workplace.gameObject.GetComponent<Structure>();
    this.w = t.GetComponent<Worshipper>();
  }

  private void OnProgressCompleted()
  {
    this.w.TimedAnimation("cheer", 2f, new System.Action(((Task) this).ClearTask));
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    if ((double) this.structure.Structure_Info.Progress >= (double) this.structure.Structure_Info.ProgressTarget)
      this.OnProgressCompleted();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if ((double) Vector2.Distance((Vector2) this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, (Vector2) this.t.transform.position) > (double) this.t.StoppingDistance)
        {
          this.Timer = 0.0f;
          this.PathToPosition(this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position);
          TaskDoer t = this.t;
          t.EndOfPath = t.EndOfPath + new System.Action(this.ArriveAtWorkPlace);
          break;
        }
        this.ArriveAtWorkPlace();
        break;
      case StateMachine.State.Moving:
        if ((double) Vector2.Distance((Vector2) this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, (Vector2) this.t.transform.position) <= (double) this.t.StoppingDistance || (double) (this.Timer += Time.deltaTime) <= 1.0)
          break;
        this.Timer = 0.0f;
        this.PathToPosition(this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position);
        break;
      case StateMachine.State.Dancing:
        this.DoWork();
        if ((double) Vector2.Distance((Vector2) this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, (Vector2) this.t.transform.position) <= (double) this.t.StoppingDistance)
          break;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        break;
    }
  }

  private void GeneratePower()
  {
    if ((double) (this.PowerTimer += Time.deltaTime) <= 1.0)
      return;
    this.PowerTimer = 0.0f;
  }

  private void DoWork()
  {
    if (!this.structure.Structure_Info.WorkIsRequiredForProgress)
      return;
    this.structure.Structure_Info.Progress += 1f * Time.deltaTime;
  }

  private void ArriveAtWorkPlace()
  {
    this.t.transform.position = this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position;
    this.state.CURRENT_STATE = StateMachine.State.Dancing;
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.ArriveAtWorkPlace);
    this.workplace.ArrivedAtJob();
  }

  public override void ClearTask()
  {
    this.workplace.EndJob(this.t, this.wim.v_i.WorkPlaceSlot);
    base.ClearTask();
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.ArriveAtWorkPlace);
  }
}
