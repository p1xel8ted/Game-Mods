// Decompiled with JetBrains decompiler
// Type: FollowerTask_InHealing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerTask_InHealing : FollowerTask
{
  public float CacheRest;
  public int _prisonID;
  public StructureBrain _prison;

  public override FollowerTaskType Type => FollowerTaskType.InHealing;

  public override FollowerLocation Location => this._prison.Data.Location;

  public override bool DisablePickUpInteraction => true;

  public override int UsingStructureID => this._prisonID;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_InHealing(int prisonID)
  {
    this._prisonID = prisonID;
    this._prison = StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
  }

  public override int GetSubTaskCode() => this._prisonID;

  public override void OnStart()
  {
    StructureManager.GetStructureByID<StructureBrain>(this._prisonID);
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnEnd()
  {
    StructureManager.GetStructureByID<StructureBrain>(this._prisonID).Data.FollowerID = -1;
    base.OnEnd();
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    HealingBay prison = this.FindPrison();
    return !((UnityEngine.Object) prison == (UnityEngine.Object) null) ? prison.HealingBayLocation.position : follower.transform.position;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    if (this.State != FollowerTaskState.Doing)
      return;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Sick/idle-sick");
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Sick/idle-sick");
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    HealingBay prison = this.FindPrison();
    if ((UnityEngine.Object) prison != (UnityEngine.Object) null)
      follower.GoTo(prison.HealingBayExitLocation.transform.position, new System.Action(((FollowerTask) this).Complete));
    else
      this.Complete();
  }

  public HealingBay FindPrison()
  {
    HealingBay prison = (HealingBay) null;
    foreach (HealingBay healingBay in HealingBay.HealingBays)
    {
      if (healingBay.StructureInfo.ID == this._prisonID)
      {
        prison = healingBay;
        break;
      }
    }
    return prison;
  }

  public override float RestChange(float deltaGameTime)
  {
    return (float) (100.0 * 0.699999988079071 * ((double) deltaGameTime / 240.0));
  }
}
