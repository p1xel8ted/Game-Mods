// Decompiled with JetBrains decompiler
// Type: Task_ReturnToStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
internal class Task_ReturnToStation : Task
{
  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
  }

  public override void TaskUpdate()
  {
    if ((Object) this.TargetObject == (Object) null)
    {
      if (this.ParentTask == null)
        return;
      this.ParentTask.ClearCurrentTask();
    }
    else
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetObject.gameObject.transform.position);
          break;
        case StateMachine.State.Moving:
          if ((double) Vector3.Distance(this.t.transform.position, this.TargetObject.transform.position) <= (double) Farm.FarmTileSize)
          {
            if (this.ParentTask == null)
              break;
            this.ParentTask.ClearCurrentTask();
            break;
          }
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetObject.gameObject.transform.position);
          break;
      }
    }
  }
}
