// Decompiled with JetBrains decompiler
// Type: FollowerTask_ClaimDwelling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FollowerTask_ClaimDwelling : FollowerTask
{
  public const int CLAIM_DURATION_GAME_MINUTES = 2;
  private Dwelling.DwellingAndSlot _dwellingAndSlot;
  private Structures_Bed _dwelling;
  private float _progress;
  public static Action<int> OnClaimHome;

  public override FollowerTaskType Type => FollowerTaskType.ClaimDwelling;

  public override FollowerLocation Location => this._dwelling.Data.Location;

  public override int UsingStructureID => this._dwellingAndSlot.ID;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_ClaimDwelling(Dwelling.DwellingAndSlot dwellingAndSlot)
  {
    this._dwellingAndSlot = dwellingAndSlot;
    this._dwelling = StructureManager.GetStructureByID<Structures_Bed>(this._dwellingAndSlot.ID);
  }

  protected override int GetSubTaskCode()
  {
    return this._dwellingAndSlot.ID * 100 + this._dwellingAndSlot.dwellingslot;
  }

  public override void ClaimReservations()
  {
    Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(this._dwellingAndSlot.ID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    Structures_Bed structureById = StructureManager.GetStructureByID<Structures_Bed>(this._dwellingAndSlot.ID);
    if (structureById == null)
      return;
    structureById.ReservedForTask = false;
  }

  protected override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  protected override void OnEnd()
  {
    this._brain.AssignDwelling(this._dwellingAndSlot, this._brain.Info.ID, true);
    Action<int> onClaimHome = FollowerTask_ClaimDwelling.OnClaimHome;
    if (onClaimHome != null)
      onClaimHome(this._brain.Info.ID);
    base.OnEnd();
  }

  protected override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    this._progress += deltaGameTime;
    if ((double) this._progress < 2.0)
      return;
    this.End();
  }

  protected override Vector3 UpdateDestination(Follower follower)
  {
    return StructureManager.GetStructureByID<Structures_Bed>(this._dwellingAndSlot.ID).Data.Position;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    follower.TimedAnimation("Reactions/react-happy1", 2.1f, (System.Action) (() => base.OnFinaliseBegin(follower)));
  }

  private Dwelling FindDwelling() => Dwelling.GetDwellingByID(this._dwellingAndSlot.ID);
}
