// Decompiled with JetBrains decompiler
// Type: FollowerTask_ClaimDwelling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_ClaimDwelling : FollowerTask
{
  public const int CLAIM_DURATION_GAME_MINUTES = 2;
  public Dwelling.DwellingAndSlot _dwellingAndSlot;
  public Structures_Bed _dwelling;
  public float _progress;
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

  public override int GetSubTaskCode()
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

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public override void OnEnd()
  {
    this._brain.AssignDwelling(this._dwellingAndSlot, this._brain.Info.ID, true);
    Action<int> onClaimHome = FollowerTask_ClaimDwelling.OnClaimHome;
    if (onClaimHome != null)
      onClaimHome(this._brain.Info.ID);
    base.OnEnd();
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.State != FollowerTaskState.Doing)
      return;
    this._progress += deltaGameTime;
    if ((double) this._progress < 2.0)
      return;
    this.End();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3 = StructureManager.GetStructureByID<Structures_Bed>(this._dwellingAndSlot.ID).Data.Position;
    Dwelling dwelling = this.FindDwelling();
    if ((UnityEngine.Object) dwelling != (UnityEngine.Object) null)
      vector3 = dwelling.GetDwellingSlotPosition(this._dwellingAndSlot.dwellingslot);
    return vector3;
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  public override void OnFinaliseBegin(Follower follower)
  {
    follower.TimedAnimation("Reactions/react-happy1", 2.1f, (System.Action) (() => this.\u003C\u003En__0(follower)));
  }

  public Dwelling FindDwelling() => Dwelling.GetDwellingByID(this._dwellingAndSlot.ID);

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0(Follower follower) => base.OnFinaliseBegin(follower);
}
