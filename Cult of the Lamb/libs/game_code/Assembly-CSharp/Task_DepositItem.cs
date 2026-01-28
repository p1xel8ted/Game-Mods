// Decompiled with JetBrains decompiler
// Type: Task_DepositItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_DepositItem : Task
{
  public SimpleInventory T_Inventory;
  public Structure TargetStructure;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.T_Inventory = t.GetComponent<SimpleInventory>();
    this.TargetStructure = TargetObject.GetComponent<Structure>();
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
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
          if ((double) (this.Timer += Time.deltaTime) <= 0.5)
            break;
          InventoryItem inventoryItem = new InventoryItem();
          inventoryItem.Init((int) this.T_Inventory.GetItemType(), 1);
          this.TargetStructure.DepositInventory(inventoryItem);
          this.T_Inventory.RemoveItem();
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          this.TargetObject = (GameObject) null;
          break;
      }
    }
  }
}
