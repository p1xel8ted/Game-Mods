// Decompiled with JetBrains decompiler
// Type: Task_Barracks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_Barracks : Task
{
  public WorshipperInfoManager wim;
  public WorkPlace workplace;
  public Structure structure;
  public Worshipper w;
  public SimpleSpineEventListener SpineEventListener;
  public Barracks barracks;

  public Task_Barracks() => this.Type = Task_Type.BARRACKS;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.wim = t.GetComponent<WorshipperInfoManager>();
    this.workplace = WorkPlace.GetWorkPlaceByID(this.wim.v_i.WorkPlace);
    this.structure = this.workplace.gameObject.GetComponent<Structure>();
    this.w = t.GetComponent<Worshipper>();
    this.SpineEventListener = this.w.GetComponentInChildren<SimpleSpineEventListener>();
    this.SpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.barracks = this.structure.GetComponent<Barracks>();
  }

  public void OnSpineEvent(string EventName)
  {
    if (!(EventName == "hit"))
      return;
    this.barracks.SlotTargets[this.wim.v_i.WorkPlaceSlot].DealDamage(0.0f, this.t.gameObject, this.t.transform.position);
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if ((double) Vector2.Distance((Vector2) this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, (Vector2) this.t.transform.position) <= (double) this.t.StoppingDistance)
          break;
        this.Timer = 0.0f;
        this.PathToPosition(this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position);
        TaskDoer t = this.t;
        t.EndOfPath = t.EndOfPath + new System.Action(this.ArriveAtWorkPlace);
        break;
      case StateMachine.State.Moving:
        if ((double) Vector2.Distance((Vector2) this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, (Vector2) this.t.transform.position) <= (double) this.t.StoppingDistance || (double) (this.Timer += Time.deltaTime) <= 1.0)
          break;
        this.Timer = 0.0f;
        this.PathToPosition(this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position);
        break;
      case StateMachine.State.CustomAction0:
        this.DoWork();
        if ((double) Vector2.Distance((Vector2) this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, (Vector2) this.t.transform.position) <= (double) this.t.StoppingDistance)
          break;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        break;
    }
  }

  public void DoWork() => this.structure.Structure_Info.Progress += 1f * Time.deltaTime;

  public void ArriveAtWorkPlace()
  {
    this.t.transform.position = this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position;
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    this.state.facingAngle = Utils.GetAngle(this.t.transform.position, this.barracks.SlotTargets[this.wim.v_i.WorkPlaceSlot].transform.position);
    this.w.SetAnimation("barracks-training", true);
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.ArriveAtWorkPlace);
  }

  public override void ClearTask()
  {
    this.SpineEventListener.OnSpineEvent -= new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.workplace.EndJob(this.t, this.wim.v_i.WorkPlaceSlot);
    base.ClearTask();
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.ArriveAtWorkPlace);
  }
}
