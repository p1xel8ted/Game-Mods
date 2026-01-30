// Decompiled with JetBrains decompiler
// Type: Task_Farmer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Task_Farmer : Task
{
  public WorshipperInfoManager wim;
  public WorkPlace workplace;
  public List<Crop> Crops = new List<Crop>();

  public Task_Farmer()
  {
    this.Type = Task_Type.FARMER;
    this.SpineSkin = "Farmer";
    this.SpineHatSlot = "Hats/HAT_Farm";
  }

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.wim = t.GetComponent<WorshipperInfoManager>();
    this.workplace = WorkPlace.GetWorkPlaceByID(this.wim.v_i.WorkPlace);
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
      if (this.workplace.HasPower())
      {
        if (this.Crops.Count <= 0)
        {
          this.CreateCropList();
        }
        else
        {
          this.CurrentTask = (Task) new Task_WaterCrops();
          this.CurrentTask.StartTask(this.t, (GameObject) null);
          this.CurrentTask.ParentTask = (Task) this;
          (this.CurrentTask as Task_WaterCrops).workplace = this.workplace;
        }
      }
      if ((double) Vector3.Distance(this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].transform.position, this.t.transform.position) <= (double) Farm.FarmTileSize)
        return;
      this.CurrentTask = (Task) new Task_ReturnToStation();
      this.CurrentTask.StartTask(this.t, this.workplace.Positions[this.wim.v_i.WorkPlaceSlot].gameObject);
      this.CurrentTask.ParentTask = (Task) this;
    }
    else
      this.CurrentTask.TaskUpdate();
  }

  public void CreateCropList()
  {
    foreach (Crop crop in Crop.Crops)
    {
      if (!crop.WorkCompleted)
        this.Crops.Add(crop);
    }
  }
}
