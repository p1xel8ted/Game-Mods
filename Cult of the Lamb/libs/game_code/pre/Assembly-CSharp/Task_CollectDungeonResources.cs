// Decompiled with JetBrains decompiler
// Type: Task_CollectDungeonResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_CollectDungeonResources : Task
{
  private SimpleInventory T_Inventory;
  private PickUp pickUp;

  public Task_CollectDungeonResources()
  {
    this.Type = Task_Type.COLLECT_DUNGEON_RESOURCES;
    this.ClearOnComplete = true;
  }

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.pickUp = TargetObject.GetComponent<PickUp>();
    this.T_Inventory = t.GetComponent<SimpleInventory>();
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    if ((Object) this.TargetObject == (Object) null)
    {
      this.ClearTask();
    }
    else
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetObject.transform.position);
          break;
        case StateMachine.State.Moving:
          if ((double) Vector3.Distance(this.t.transform.position, this.TargetObject.transform.position) < 1.0)
          {
            this.Timer = 0.0f;
            this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
            break;
          }
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetObject.transform.position);
          break;
        case StateMachine.State.CustomAction0:
          double num = (double) (this.Timer += Time.deltaTime);
          break;
      }
    }
  }

  public override void ClearTask()
  {
    base.ClearTask();
    if ((Object) this.pickUp != (Object) null)
      this.pickUp.Reserved = (GameObject) null;
    this.pickUp = (PickUp) null;
  }
}
