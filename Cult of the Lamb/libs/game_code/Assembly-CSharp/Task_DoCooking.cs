// Decompiled with JetBrains decompiler
// Type: Task_DoCooking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_DoCooking : Task
{
  public Structure Kitchen;
  public SimpleInventory T_Inventory;
  public WorkPlace workplace;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Kitchen = TargetObject.GetComponent<Structure>();
    this.T_Inventory = t.GetComponent<SimpleInventory>();
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Timer = 0.0f;
        this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
        this.Kitchen.RemoveInventoryByType(InventoryItem.ITEM_TYPE.WHEAT);
        this.Kitchen.RemoveInventoryByType(InventoryItem.ITEM_TYPE.WHEAT);
        this.Kitchen.RemoveInventoryByType(InventoryItem.ITEM_TYPE.WHEAT);
        break;
      case StateMachine.State.CustomAction0:
        if ((double) (this.Timer += Time.deltaTime) > 10.0)
        {
          this.T_Inventory.GiveItem(InventoryItem.ITEM_TYPE.MEAT);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          if (this.ParentTask == null)
            break;
          this.ParentTask.ClearCurrentTask();
          break;
        }
        if (this.workplace.HasPower())
          break;
        InventoryItem inventoryItem1 = new InventoryItem();
        inventoryItem1.Init(7, 1);
        this.Kitchen.DepositInventory(inventoryItem1);
        InventoryItem inventoryItem2 = new InventoryItem();
        inventoryItem2.Init(7, 1);
        this.Kitchen.DepositInventory(inventoryItem2);
        InventoryItem inventoryItem3 = new InventoryItem();
        inventoryItem3.Init(7, 1);
        this.Kitchen.DepositInventory(inventoryItem3);
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        if (this.ParentTask == null)
          break;
        this.ParentTask.ClearCurrentTask();
        break;
    }
  }
}
