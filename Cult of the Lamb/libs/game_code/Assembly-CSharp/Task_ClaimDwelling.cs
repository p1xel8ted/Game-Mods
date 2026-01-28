// Decompiled with JetBrains decompiler
// Type: Task_ClaimDwelling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Task_ClaimDwelling : Task
{
  public Worshipper w;
  public Dwelling dwelling;

  public override void StartTask(TaskDoer t, GameObject TargetObject)
  {
    base.StartTask(t, TargetObject);
    this.Type = Task_Type.CLAIM_DWELLING;
    this.w = t.GetComponent<Worshipper>();
    this.dwelling = (Dwelling) null;
    Debug.Log((object) this.w.wim.v_i.SkinName);
    this.ClearOnComplete = true;
  }

  public override void TaskUpdate()
  {
    base.TaskUpdate();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        if ((double) Vector3.Distance(this.TargetObject.transform.position, this.t.transform.position) > (double) Farm.FarmTileSize)
        {
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetObject.transform.position);
          break;
        }
        this.Timer = 0.0f;
        this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
        break;
      case StateMachine.State.Moving:
        if ((double) Vector3.Distance(this.TargetObject.transform.position, this.t.transform.position) > (double) Farm.FarmTileSize)
        {
          if ((double) (this.Timer += Time.deltaTime) <= 1.0)
            break;
          this.Timer = 0.0f;
          this.PathToPosition(this.TargetObject.transform.position);
          break;
        }
        this.Timer = 0.0f;
        this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
        break;
      case StateMachine.State.CustomAction0:
        if ((double) (this.Timer += Time.deltaTime) <= 1.0)
          break;
        this.w.wim.v_i.DwellingClaimed = true;
        this.w.wim.v_i.Complaint_House = false;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        this.dwelling.SetBedImage(this.w.wim.v_i.DwellingSlot, Dwelling.SlotState.CLAIMED);
        this.w.wim.SetOutfit(this.w.wim.v_i.Outfit, false);
        this.ClearTask();
        this.t.ClearTask();
        break;
    }
  }

  public override void ClearTask()
  {
    Debug.Log((object) ("CLEAR claim dwelling! " + this.w.wim.v_i.SkinName));
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    base.ClearTask();
  }
}
