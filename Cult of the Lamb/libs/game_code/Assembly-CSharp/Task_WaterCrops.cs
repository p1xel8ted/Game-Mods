// Decompiled with JetBrains decompiler
// Type: Task_WaterCrops
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_WaterCrops : Task
{
  public WorkPlace workplace;
  public Crop crop;
  public float WateringDuration = 4f;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
  }

  public override void TaskUpdate()
  {
    if (!this.workplace.HasPower())
    {
      if (this.ParentTask == null)
        return;
      this.ParentTask.ClearCurrentTask();
    }
    else if ((Object) this.TargetObject == (Object) null)
    {
      this.GetCrop();
      if (!((Object) this.TargetObject == (Object) null) || this.ParentTask == null)
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
          this.crop.DoWork((float) (1.0 / ((double) this.WateringDuration / (double) Time.deltaTime)));
          if ((double) (this.Timer += Time.deltaTime) <= (double) this.WateringDuration)
            break;
          (this.ParentTask as Task_Farmer).Crops.Remove(this.crop);
          this.crop.Reserved = (GameObject) null;
          this.crop = (Crop) null;
          this.TargetObject = (GameObject) null;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
      }
    }
  }

  public void GetCrop()
  {
    Crop crop1 = (Crop) null;
    float num1 = float.MaxValue;
    foreach (Crop crop2 in (this.ParentTask as Task_Farmer).Crops)
    {
      if ((Object) crop2 != (Object) null && (Object) crop2.Reserved == (Object) null)
      {
        float num2 = Vector3.Distance(crop2.gameObject.transform.position, this.t.transform.position);
        if ((double) num2 < (double) num1)
        {
          crop1 = crop2;
          num1 = num2;
        }
      }
    }
    if (!((Object) crop1 != (Object) null))
      return;
    this.TargetObject = crop1.gameObject;
    this.crop = crop1;
    crop1.Reserved = this.t.gameObject;
  }

  public override void ClearTask()
  {
    this.TargetObject = (GameObject) null;
    if ((Object) this.crop != (Object) null)
      this.crop.Reserved = (GameObject) null;
    this.crop = (Crop) null;
  }
}
