// Decompiled with JetBrains decompiler
// Type: Task_ReturnToStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_ReturnToStation : Task
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
