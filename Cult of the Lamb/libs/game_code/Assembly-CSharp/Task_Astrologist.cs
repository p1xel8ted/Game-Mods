// Decompiled with JetBrains decompiler
// Type: Task_Astrologist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Task_Astrologist : Task
{
  public Structure structure;
  public WorkPlace workplace;
  public WorshipperInfoManager wim;
  public Task_GoToAndStop GoToAndStop;
  public Astrologist astrologist;
  public Worshipper w;
  public int CurrentBoard;

  public Task_Astrologist() => this.Type = Task_Type.ASTROLOGIST;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    Debug.Log((object) "START TASK!");
    base.StartTask(t, TargetObject);
    this.wim = t.GetComponent<WorshipperInfoManager>();
    this.workplace = WorkPlace.GetWorkPlaceByID(this.wim.v_i.WorkPlace);
    this.astrologist = this.workplace.GetComponent<Astrologist>();
    this.w = t.GetComponent<Worshipper>();
  }

  public override void ClearTask()
  {
    this.workplace.EndJob(this.t, this.wim.v_i.WorkPlaceSlot);
    if (this.GoToAndStop != null)
      this.GoToAndStop.ClearTask();
    this.GoToAndStop = (Task_GoToAndStop) null;
    if (this.CurrentTask != null)
      this.CurrentTask.ClearTask();
    this.CurrentTask = (Task) null;
    base.ClearTask();
  }

  public override void TaskUpdate()
  {
    if (this.GoToAndStop != null)
      this.GoToAndStop.TaskUpdate();
    if (this.CurrentBoard >= this.astrologist.MoonBoardsUpdated.Count)
    {
      if ((double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position) > (double) this.t.StoppingDistance && this.GoToAndStop == null)
      {
        this.GoToAndStop = new Task_GoToAndStop();
        this.GoToAndStop.StartTask(this.t, this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].gameObject);
        this.GoToAndStop.DoCallback = false;
      }
      if ((double) Vector2.Distance((Vector2) this.t.transform.position, (Vector2) this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position) > (double) this.t.StoppingDistance || this.t.state.CURRENT_STATE == StateMachine.State.CustomAction0)
        return;
      this.t.state.CURRENT_STATE = StateMachine.State.CustomAction0;
      this.t.state.facingAngle = 0.0f;
      this.w.SetAnimation("astrologer", true);
    }
    else
    {
      if (this.astrologist.MoonBoardsUpdated[this.CurrentBoard] || this.GoToAndStop != null)
        return;
      this.t.StartCoroutine((IEnumerator) this.GoToAndUpdateImage(this.astrologist.MoonBoardPositions[this.CurrentBoard].gameObject));
    }
  }

  public IEnumerator GoToAndUpdateImage(GameObject ImageToUpdate)
  {
    Task_Astrologist taskAstrologist = this;
    taskAstrologist.GoToAndStop = new Task_GoToAndStop();
    taskAstrologist.GoToAndStop.StartTask(taskAstrologist.t, ImageToUpdate);
    taskAstrologist.GoToAndStop.DoCallback = false;
    while ((double) Vector2.Distance((Vector2) taskAstrologist.t.transform.position, (Vector2) ImageToUpdate.transform.position) > (double) taskAstrologist.t.StoppingDistance || taskAstrologist.state.CURRENT_STATE == StateMachine.State.Moving)
      yield return (object) null;
    taskAstrologist.t.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    taskAstrologist.t.state.facingAngle = Utils.GetAngle(taskAstrologist.t.transform.position, ImageToUpdate.transform.position);
    yield return (object) new WaitForSeconds(1f);
    taskAstrologist.astrologist.MoonBoards[taskAstrologist.CurrentBoard].sprite = taskAstrologist.astrologist.MoonBoardsImage[taskAstrologist.CurrentBoard];
    taskAstrologist.GoToAndStop = (Task_GoToAndStop) null;
    ++taskAstrologist.CurrentBoard;
  }
}
