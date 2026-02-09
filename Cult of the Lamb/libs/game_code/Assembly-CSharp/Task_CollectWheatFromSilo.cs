// Decompiled with JetBrains decompiler
// Type: Task_CollectWheatFromSilo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_CollectWheatFromSilo : Task
{
  public PickUp pickUp;
  public Structure TargetSilo;
  public SimpleInventory T_Inventory;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.T_Inventory = t.GetComponent<SimpleInventory>();
    this.T_Inventory.DropItem();
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    if (this.T_Inventory.GetItemType() != InventoryItem.ITEM_TYPE.WHEAT)
    {
      if ((Object) this.TargetSilo == (Object) null)
      {
        this.GetTargetStructure();
        if (!((Object) this.TargetSilo == (Object) null) || this.ParentTask == null)
          return;
        this.ParentTask.ClearCurrentTask();
      }
      else
      {
        switch (this.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            this.Timer = 0.0f;
            this.PathToPosition(this.TargetSilo.gameObject.transform.position);
            break;
          case StateMachine.State.Moving:
            if ((double) Vector3.Distance(this.t.transform.position, this.TargetSilo.transform.position) <= 1.0)
            {
              this.Timer = 0.0f;
              this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
              break;
            }
            if ((double) (this.Timer += Time.deltaTime) <= 1.0)
              break;
            this.Timer = 0.0f;
            this.PathToPosition(this.TargetSilo.gameObject.transform.position);
            break;
          case StateMachine.State.CustomAction0:
            if ((double) (this.Timer += Time.deltaTime) <= 0.5)
              break;
            if (this.TargetSilo.HasInventoryType(InventoryItem.ITEM_TYPE.WHEAT))
            {
              this.TargetSilo.RemoveInventoryByType(InventoryItem.ITEM_TYPE.WHEAT);
              this.T_Inventory.GiveItem(InventoryItem.ITEM_TYPE.WHEAT);
              this.TargetSilo = (Structure) null;
              this.state.CURRENT_STATE = StateMachine.State.Idle;
              break;
            }
            if (this.ParentTask == null)
              break;
            this.ParentTask.ClearCurrentTask();
            break;
        }
      }
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
          if ((double) Vector3.Distance(this.t.transform.position, this.TargetObject.transform.position) <= 1.0)
          {
            this.Timer = 0.0f;
            this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
            break;
          }
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetObject.gameObject.transform.position);
          break;
        case StateMachine.State.CustomAction0:
          if ((double) (this.Timer += Time.deltaTime) <= 0.5)
            break;
          Structure component = this.TargetObject.GetComponent<Structure>();
          InventoryItem inventoryItem = new InventoryItem();
          inventoryItem.Init(7, 1);
          if ((Object) component != (Object) null)
            component.Inventory.Add(inventoryItem);
          this.T_Inventory.RemoveItem();
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          if (this.ParentTask == null)
            break;
          this.ParentTask.ClearCurrentTask();
          break;
      }
    }
  }

  public void GetTargetStructure()
  {
    Structure structure1 = (Structure) null;
    float num1 = float.MaxValue;
    foreach (Structure structure2 in Structure.Structures)
    {
      if (structure2.HasInventoryType(InventoryItem.ITEM_TYPE.WHEAT) && structure2.Type == StructureBrain.TYPES.WHEAT_SILO)
      {
        float num2 = Vector3.Distance(structure2.gameObject.transform.position, this.t.transform.position);
        if ((double) num2 < (double) num1)
        {
          structure1 = structure2;
          num1 = num2;
        }
      }
    }
    if (!((Object) structure1 != (Object) null))
      return;
    this.TargetSilo = structure1;
  }

  public override void ClearTask()
  {
    this.TargetObject = (GameObject) null;
    if ((Object) this.pickUp != (Object) null)
      this.pickUp.Reserved = (GameObject) null;
    this.pickUp = (PickUp) null;
  }
}
