// Decompiled with JetBrains decompiler
// Type: Task_GoToAndStop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_GoToAndStop : Task
{
  public bool DoCallback = true;
  public bool ClearCurrentTaskAfterGoToAndStop = true;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    t.UsePathing = true;
  }

  public override void TaskUpdate()
  {
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
    {
      this.EndOfPath();
    }
    else
    {
      base.TaskUpdate();
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector3.Distance(this.TargetObject.transform.position, this.t.transform.position) > (double) this.t.StoppingDistance)
          {
            this.Timer = 0.0f;
            this.PathToPosition(this.TargetObject.transform.position);
            TaskDoer t = this.t;
            t.EndOfPath = t.EndOfPath + new System.Action(this.EndOfPath);
            break;
          }
          this.t.transform.position = Vector3.Lerp(this.t.transform.position, this.TargetObject.transform.position, 2f * Time.deltaTime);
          this.EndOfPath();
          break;
        case StateMachine.State.Moving:
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetObject.transform.position);
          break;
      }
    }
  }

  private void EndOfPath()
  {
    Worshipper component = this.t.GetComponent<Worshipper>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && this.DoCallback)
      component.EndGoToAndStop();
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.EndOfPath);
  }

  public override void ClearTask()
  {
    TaskDoer t = this.t;
    t.EndOfPath = t.EndOfPath - new System.Action(this.EndOfPath);
    if (!this.ClearCurrentTaskAfterGoToAndStop)
      return;
    base.ClearTask();
  }
}
