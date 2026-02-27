// Decompiled with JetBrains decompiler
// Type: Task_Cook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_Cook : Task
{
  private WorshipperInfoManager wim;
  private WorkPlace workplace;
  private Structure Kitchen;
  private SimpleInventory inventory;

  public Task_Cook()
  {
    this.Type = Task_Type.COOK;
    this.SpineSkin = "Cook";
    this.SpineHatSlot = "Hats/HAT_Che";
  }

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.wim = t.GetComponent<WorshipperInfoManager>();
    this.workplace = WorkPlace.GetWorkPlaceByID(this.wim.v_i.WorkPlace);
    this.Kitchen = this.workplace.gameObject.GetComponent<Structure>();
    this.inventory = t.GetComponent<SimpleInventory>();
  }

  public override void ClearTask()
  {
    this.workplace.EndJob(this.t, this.wim.v_i.WorkPlaceSlot);
    if (this.CurrentTask != null)
      this.CurrentTask.ClearTask();
    this.CurrentTask = (Task) null;
    base.ClearTask();
  }

  public override void TaskUpdate()
  {
    if (this.CurrentTask == null)
    {
      if (this.inventory.GetItemType() == InventoryItem.ITEM_TYPE.MEAT)
      {
        Structure ofType = Structure.GetOfType(StructureBrain.TYPES.COOKED_FOOD_SILO);
        if ((Object) ofType != (Object) null)
        {
          this.CurrentTask = (Task) new Task_DepositItem();
          this.CurrentTask.StartTask(this.t, ofType.gameObject);
          this.CurrentTask.ParentTask = (Task) this;
          return;
        }
      }
      if (this.Kitchen.Inventory.Count < 3)
      {
        foreach (Structure structure in Structure.Structures)
        {
          if (structure.HasInventoryType(InventoryItem.ITEM_TYPE.WHEAT) && !structure.IsType(StructureBrain.TYPES.KITCHEN) && !structure.IsType(StructureBrain.TYPES.KITCHEN_II))
          {
            this.CurrentTask = (Task) new Task_CollectWheatFromSilo();
            this.CurrentTask.StartTask(this.t, this.Kitchen.gameObject);
            this.CurrentTask.ParentTask = (Task) this;
            return;
          }
        }
      }
      if ((double) Vector3.Distance(this.workplace.Positions[0].transform.position, this.t.transform.position) > (double) Farm.FarmTileSize)
      {
        this.CurrentTask = (Task) new Task_ReturnToStation();
        this.CurrentTask.StartTask(this.t, this.workplace.Positions[0].gameObject);
        this.CurrentTask.ParentTask = (Task) this;
      }
      else
      {
        if (this.Kitchen.Inventory.Count < 3 || !this.workplace.HasPower())
          return;
        this.CurrentTask = (Task) new Task_DoCooking();
        this.CurrentTask.StartTask(this.t, this.Kitchen.gameObject);
        this.CurrentTask.ParentTask = (Task) this;
        (this.CurrentTask as Task_DoCooking).workplace = this.workplace;
      }
    }
    else
      this.CurrentTask.TaskUpdate();
  }
}
