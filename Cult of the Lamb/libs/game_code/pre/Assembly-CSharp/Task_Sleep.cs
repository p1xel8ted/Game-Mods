// Decompiled with JetBrains decompiler
// Type: Task_Sleep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_Sleep : Task
{
  public bool DoYawn = true;
  private Dwelling dwelling;
  private Worshipper w;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    t.GetComponent<SimpleInventory>().DropItem();
    this.Type = Task_Type.SLEEP;
    this.w = t.GetComponent<Worshipper>();
  }

  public void Init(Dwelling dwelling, GameObject TargetObject, bool DoYawn)
  {
    this.dwelling = dwelling;
    this.TargetObject = TargetObject;
    this.DoYawn = DoYawn;
  }

  private void EndOfPath()
  {
    new GameObject()
    {
      transform = {
        position = this.TargetObject.transform.position
      }
    }.name = $"{this.w.wim.v_i.SkinName}  {this.w.wim.v_i.Name}";
    this.Timer = 0.0f;
    this.DoSleep();
    this.DoYawn = true;
  }

  private void DoSleep()
  {
    this.t.transform.position = this.TargetObject.transform.position;
    this.state.CURRENT_STATE = StateMachine.State.Sleeping;
    this.dwelling.SetBedImage(this.w.wim.v_i.DwellingSlot, Dwelling.SlotState.IN_USE);
    this.w.wim.v_i.DwellingClaimed = true;
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    if (this.state.CURRENT_STATE != StateMachine.State.Sleeping)
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector2.Distance((Vector2) this.TargetObject.transform.position, (Vector2) this.t.transform.position) > (double) Farm.FarmTileSize)
          {
            this.Timer = 0.0f;
            this.PathToPosition(this.TargetObject.transform.position);
            break;
          }
          this.EndOfPath();
          break;
        case StateMachine.State.Moving:
          if ((double) Vector2.Distance((Vector2) this.TargetObject.transform.position, (Vector2) this.t.transform.position) > (double) Farm.FarmTileSize)
          {
            if ((double) (this.Timer += Time.deltaTime) <= 1.0)
              break;
            this.Timer = 0.0f;
            this.PathToPosition(this.TargetObject.transform.position);
            break;
          }
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          break;
      }
    }
    else
    {
      float num1;
      float num2;
      switch (this.dwelling.Structure.Type)
      {
        case StructureBrain.TYPES.BED_2:
          num1 = 180f;
          num2 = 25f;
          break;
        case StructureBrain.TYPES.BED_3:
          num1 = 120f;
          num2 = 20f;
          break;
        default:
          num1 = 60f;
          num2 = 10f;
          break;
      }
      this.w.Sleep += Time.deltaTime * num2;
      if ((double) this.w.Sleep >= (double) num1)
      {
        if (this.dwelling.Structure.Type != StructureBrain.TYPES.BED_2)
          ;
        this.ClearTask();
        this.t.ClearTask();
      }
      else
      {
        if ((double) Vector2.Distance((Vector2) this.TargetObject.transform.position, (Vector2) this.t.transform.position) <= (double) Farm.FarmTileSize)
          return;
        this.Timer = 0.0f;
        this.PathToPosition(this.TargetObject.transform.position);
      }
    }
  }

  public override void ClearTask()
  {
    switch (this.dwelling.Structure.Type)
    {
      case StructureBrain.TYPES.BED_2:
        this.w.Sleep = 180f;
        break;
      case StructureBrain.TYPES.BED_3:
        this.w.Sleep = 120f;
        break;
      default:
        this.w.Sleep = 60f;
        break;
    }
    if ((Object) this.dwelling != (Object) null)
      this.dwelling.SetBedImage(this.w.wim.v_i.DwellingSlot, Dwelling.SlotState.CLAIMED);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    base.ClearTask();
  }
}
