// Decompiled with JetBrains decompiler
// Type: FollowerTask_StealBed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_StealBed : FollowerTask
{
  public Dwelling.DwellingAndSlot _dwellingAndSlot;
  public Structures_Bed _dwelling;
  public float _progress;
  public static Action<int> OnClaimHome;

  public override FollowerTaskType Type => FollowerTaskType.StealBed;

  public override FollowerLocation Location => this._dwelling.Data.Location;

  public override int UsingStructureID => this._dwellingAndSlot.ID;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public FollowerTask_StealBed(Dwelling.DwellingAndSlot dwellingAndSlot)
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
    Action<int> onClaimHome = FollowerTask_StealBed.OnClaimHome;
    if (onClaimHome != null)
      onClaimHome(this._brain.Info.ID);
    base.OnEnd();
  }

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    Vector3 vector3 = StructureManager.GetStructureByID<Structures_Bed>(this._dwellingAndSlot.ID).Data.Position;
    Dwelling dwelling = this.FindDwelling();
    if ((UnityEngine.Object) dwelling != (UnityEngine.Object) null)
      vector3 = dwelling.GetDwellingSlotPosition(this._dwellingAndSlot.dwellingslot);
    return vector3 + ((double) UnityEngine.Random.value > 0.5 ? Vector3.right : Vector3.left);
  }

  public override void OnDoingBegin(Follower follower)
  {
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    Follower target = (Follower) null;
    foreach (Follower follower1 in Follower.Followers)
    {
      if (follower1.Brain.Info.ID == this._dwelling.Data.FollowerID)
      {
        target = follower1;
        break;
      }
    }
    if ((UnityEngine.Object) target == (UnityEngine.Object) null || target.Brain.CurrentTaskType != FollowerTaskType.Sleep)
      this.End();
    else
      follower.StartCoroutine((IEnumerator) this.StealBedIE(follower, target));
  }

  public Dwelling FindDwelling() => Dwelling.GetDwellingByID(this._dwellingAndSlot.ID);

  public IEnumerator StealBedIE(Follower follower, Follower target)
  {
    FollowerTask_StealBed followerTaskStealBed = this;
    follower.FacePosition(target.transform.position);
    follower.TimedAnimation("Conversations/talk-hate1", 2.83333325f);
    yield return (object) new WaitForSeconds(1.41666663f);
    target.TimedAnimation("Reactions/react-cry", 9f);
    yield return (object) new WaitForSeconds(1.41666663f);
    follower.TimedAnimation("Reactions/react-laugh", 2.83333325f);
    yield return (object) new WaitForSeconds(2.83333325f);
    follower.Brain.ClearDwelling();
    target.Brain.ClearDwelling();
    if ((double) UnityEngine.Random.value < 0.10000000149011612 && !target.Brain.HasTrait(FollowerTrait.TraitType.Scared) && !target.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred))
      target.AddTrait(FollowerTrait.TraitType.Scared, true);
    followerTaskStealBed.End();
  }
}
