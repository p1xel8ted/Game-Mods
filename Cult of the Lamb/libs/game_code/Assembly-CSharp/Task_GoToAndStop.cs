// Decompiled with JetBrains decompiler
// Type: Task_GoToAndStop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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

  public void EndOfPath()
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
