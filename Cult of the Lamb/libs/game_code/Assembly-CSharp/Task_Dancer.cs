// Decompiled with JetBrains decompiler
// Type: Task_Dancer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_Dancer : Task
{
  public WorshipperInfoManager wim;
  public WorkPlace workplace;
  public Structure structure;
  public Worshipper w;
  public float PowerTimer;

  public Task_Dancer() => this.Type = Task_Type.DANCER;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.wim = t.GetComponent<WorshipperInfoManager>();
    this.workplace = WorkPlace.GetWorkPlaceByID(this.wim.v_i.WorkPlace);
    this.structure = this.workplace.gameObject.GetComponent<Structure>();
    this.w = t.GetComponent<Worshipper>();
  }

  public void OnProgressCompleted()
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

  public void GeneratePower()
  {
    if ((double) (this.PowerTimer += Time.deltaTime) <= 1.0)
      return;
    this.PowerTimer = 0.0f;
  }

  public void DoWork()
  {
    if (!this.structure.Structure_Info.WorkIsRequiredForProgress)
      return;
    this.structure.Structure_Info.Progress += 1f * Time.deltaTime;
  }

  public void ArriveAtWorkPlace()
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
