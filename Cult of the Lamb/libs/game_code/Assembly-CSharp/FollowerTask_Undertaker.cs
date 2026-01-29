// Decompiled with JetBrains decompiler
// Type: FollowerTask_Undertaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Undertaker : FollowerTask
{
  public int _morgueID;
  public bool pickedUpBody;
  public bool carryingBody;
  public bool droppedBody;
  public Structures_Morgue _morgue;
  public Structures_DeadWorshipper _deadBody;
  public Interaction_HarvestMeat harvestMeat;

  public override FollowerTaskType Type => FollowerTaskType.Undertaker;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Undertaker ? PriorityCategory.OverrideWorkPriority : PriorityCategory.Low;
  }

  public FollowerTask_Undertaker(int morgueID)
  {
    this._morgueID = morgueID;
    this._morgue = StructureManager.GetStructureByID<Structures_Morgue>(this._morgueID);
  }

  public override int GetSubTaskCode() => this._morgueID;

  public override void ClaimReservations()
  {
    if (this._morgue != null)
      this._morgue.ReservedForTask = true;
    if (this._deadBody == null)
      return;
    this._deadBody.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    if (this._morgue != null)
      this._morgue.ReservedForTask = false;
    if (this._deadBody == null)
      return;
    this._deadBody.ReservedForTask = false;
  }

  public override void OnStart() => this.SetState(FollowerTaskState.GoingTo);

  public int GetNextStructure()
  {
    List<StructureBrain> structureBrainList1 = new List<StructureBrain>();
    structureBrainList1.AddRange((IEnumerable<StructureBrain>) StructureManager.GetAllStructuresOfType(this.Location, StructureBrain.TYPES.DEAD_WORSHIPPER));
    List<StructureBrain> structureBrainList2 = new List<StructureBrain>();
    foreach (StructureBrain structureBrain in structureBrainList1)
    {
      if (!structureBrain.ReservedByPlayer && !structureBrain.ReservedForTask && !structureBrain.Data.BeenInMorgueAlready && !structureBrain.Data.BodyWrapped)
      {
        FollowerInfo followerInfoById = FollowerManager.GetDeadFollowerInfoByID(structureBrain.Data.FollowerID);
        if (followerInfoById == null || !followerInfoById.FrozeToDeath && !followerInfoById.DiedFromRot)
          structureBrainList2.Add(structureBrain);
      }
    }
    if (structureBrainList2.Count <= 0)
      return -1;
    StructureBrain structureBrain1 = (StructureBrain) null;
    foreach (StructureBrain structureBrain2 in structureBrainList2)
    {
      if (structureBrain1 == null || (double) Vector3.Distance(structureBrain2.Data.Position, this._brain.LastPosition) < (double) Vector3.Distance(structureBrain1.Data.Position, this._brain.LastPosition))
        structureBrain1 = structureBrain2;
    }
    return structureBrain1.Data.ID;
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if (this.carryingBody && this._deadBody != null && !this.droppedBody)
      this.DropBody();
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((bool) (UnityEngine.Object) followerById)
      followerById.SetOutfit(FollowerOutfitType.Follower, false);
    if ((UnityEngine.Object) this.harvestMeat != (UnityEngine.Object) null)
    {
      this.harvestMeat.enabled = true;
      this.harvestMeat.Interactable = true;
    }
    this.harvestMeat = (Interaction_HarvestMeat) null;
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if ((this.carryingBody || this.pickedUpBody) && this._deadBody != null && !this.droppedBody)
      this.DropBody();
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if ((bool) (UnityEngine.Object) followerById)
      followerById.SetOutfit(FollowerOutfitType.Follower, false);
    if ((UnityEngine.Object) this.harvestMeat != (UnityEngine.Object) null)
    {
      this.harvestMeat.enabled = true;
      this.harvestMeat.Interactable = true;
    }
    this.harvestMeat = (Interaction_HarvestMeat) null;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    if (this._deadBody == null || this.pickedUpBody)
      return;
    this.harvestMeat = this.FindBody(this._deadBody.Data.ID);
    if (!((UnityEngine.Object) this.harvestMeat != (UnityEngine.Object) null))
      return;
    this.harvestMeat.enabled = false;
    this.harvestMeat.Interactable = false;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player.interactor.CurrentInteraction == (UnityEngine.Object) this.harvestMeat)
        player.interactor.CurrentInteraction = (Interaction) null;
    }
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "CorpseCollect")
    {
      this.PickUpBody();
    }
    else
    {
      if (!(e.Data.Name == "CorpseDrop"))
        return;
      this.droppedBody = true;
      this._morgue.DepositBody(this._deadBody.Data.FollowerID);
    }
  }

  public void Loop()
  {
    int nextStructure = this.GetNextStructure();
    if (this._deadBody != null && !this.carryingBody)
    {
      this.carryingBody = true;
      this.PickUpBody();
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else if (this._deadBody != null && this.carryingBody)
    {
      this.ClearDestination();
      this._morgue.DepositBody(this._deadBody.Data.FollowerID);
      this.RemoveBodyFromExistence();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else if (nextStructure != -1)
    {
      this.carryingBody = false;
      this.pickedUpBody = false;
      this._deadBody = StructureManager.GetStructureByID<Structures_DeadWorshipper>(nextStructure);
      this._deadBody.ReservedForTask = true;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
      this.harvestMeat = this.FindBody(this._deadBody.Data.ID);
      if (!((UnityEngine.Object) this.harvestMeat != (UnityEngine.Object) null))
        return;
      this.harvestMeat.enabled = false;
      this.harvestMeat.Interactable = false;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player.interactor.CurrentInteraction == (UnityEngine.Object) this.harvestMeat)
          player.interactor.CurrentInteraction = (Interaction) null;
      }
    }
    else
      this.End();
  }

  public Structure GetStructure(int ID)
  {
    foreach (Structure structure in Structure.Structures)
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && structure.Brain.Data != null && structure.Brain.Data.ID == ID)
        return structure;
    }
    return (Structure) null;
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this._deadBody != null && !this.carryingBody)
      return this._deadBody.Data.Position;
    Interaction_Morgue morgue = this.FindMorgue();
    if ((UnityEngine.Object) morgue != (UnityEngine.Object) null)
      return morgue.transform.position + Vector3.back;
    this.End();
    return Vector3.zero;
  }

  public void PickUpBody()
  {
    DeadWorshipper deadWorshipper1 = (DeadWorshipper) null;
    foreach (DeadWorshipper deadWorshipper2 in DeadWorshipper.DeadWorshippers)
    {
      if (deadWorshipper2.StructureInfo != null && deadWorshipper2.StructureInfo.ID == this._deadBody.Data.ID)
      {
        deadWorshipper1 = deadWorshipper2;
        break;
      }
    }
    if (!((UnityEngine.Object) deadWorshipper1 != (UnityEngine.Object) null))
      return;
    this.pickedUpBody = true;
    deadWorshipper1.gameObject.SetActive(false);
  }

  public void DropBody()
  {
    int followerId = this._deadBody.Data.FollowerID;
    this.RemoveBodyFromExistence();
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
    infoByType.Position = this._brain.LastPosition;
    infoByType.BodyWrapped = false;
    infoByType.FollowerID = followerId;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this._brain.LastPosition, Vector2Int.one, false, (Action<GameObject>) (g =>
    {
      DeadWorshipper component = g.GetComponent<DeadWorshipper>();
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(component.transform.position);
      if (tileAtWorldPosition == null)
        return;
      component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
    }));
    if ((UnityEngine.Object) this.harvestMeat != (UnityEngine.Object) null)
    {
      this.harvestMeat.enabled = true;
      this.harvestMeat.Interactable = true;
    }
    this.harvestMeat = (Interaction_HarvestMeat) null;
  }

  public void RemoveBodyFromExistence()
  {
    DeadWorshipper deadWorshipper1 = (DeadWorshipper) null;
    foreach (DeadWorshipper deadWorshipper2 in DeadWorshipper.DeadWorshippers)
    {
      if (deadWorshipper2.StructureInfo.ID == this._deadBody.Data.ID)
      {
        deadWorshipper1 = deadWorshipper2;
        break;
      }
    }
    if ((UnityEngine.Object) deadWorshipper1 != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) deadWorshipper1.gameObject);
    if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null)
    {
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(this._deadBody.Data.Position);
      if (tileAtWorldPosition != null)
        this._deadBody.RemoveFromGrid(tileAtWorldPosition.Position);
    }
    StructureManager.RemoveStructure((StructureBrain) this._deadBody);
    this.pickedUpBody = false;
    this.carryingBody = false;
    this._deadBody = (Structures_DeadWorshipper) null;
    this.harvestMeat = (Interaction_HarvestMeat) null;
  }

  public override void OnArrive()
  {
    base.OnArrive();
    Follower followerById = FollowerManager.FindFollowerByID(this._brain.Info.ID);
    if (!(bool) (UnityEngine.Object) followerById || this._deadBody == null)
      return;
    if ((UnityEngine.Object) followerById.Spine != (UnityEngine.Object) null)
      FollowerBrain.SetFollowerCostume(followerById.Spine.Skeleton, followerById.Brain.Info.XPLevel, followerById.Brain.Info.SkinName, followerById.Brain.Info.SkinColour, FollowerOutfitType.Undertaker, followerById.Brain.Info.Hat, FollowerClothingType.None, followerById.Brain.Info.Customisation, followerById.Brain.Info.Special, followerById.Brain.Info.Necklace, string.Empty, followerById.Brain._directInfoAccess);
    followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Undertaker/run");
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    if (this._deadBody == null)
    {
      this.Loop();
    }
    else
    {
      if (this._deadBody == null || !this._deadBody.ReservedForTask)
        return;
      this.Loop();
    }
  }

  public override void OnDoingBegin(Follower follower)
  {
    if (this._deadBody == null)
    {
      this.Loop();
    }
    else
    {
      if (this._deadBody == null)
        return;
      follower.FacePosition(this._deadBody.Data.Position);
      if (!this._deadBody.ReservedForTask)
        return;
      if (!this.carryingBody)
      {
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Undertaker/run-carrying");
        follower.TimedAnimation("Undertaker/collect-corpse", 4.33f, (System.Action) (() => this.Loop()));
      }
      else
      {
        follower.Interaction_FollowerInteraction.enabled = false;
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Undertaker/run");
        follower.TimedAnimation("Undertaker/drop-corpse", 3.66f, (System.Action) (() =>
        {
          this.RemoveBodyFromExistence();
          this.Loop();
          follower.Interaction_FollowerInteraction.enabled = true;
          this.droppedBody = false;
        }));
      }
    }
  }

  public override void Cleanup(Follower follower)
  {
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    base.Cleanup(follower);
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    follower.SetOutfit(FollowerOutfitType.Follower, false);
    follower.Interaction_FollowerInteraction.enabled = true;
    follower.ClearPath();
  }

  public Interaction_Morgue FindMorgue()
  {
    Interaction_Morgue morgue1 = (Interaction_Morgue) null;
    foreach (Interaction_Morgue morgue2 in Interaction_Morgue.Morgues)
    {
      if ((UnityEngine.Object) morgue2 != (UnityEngine.Object) null && morgue2.StructureInfo.ID == this._morgueID)
      {
        morgue1 = morgue2;
        break;
      }
    }
    return morgue1;
  }

  public Interaction_HarvestMeat FindBody(int id)
  {
    Interaction_HarvestMeat body = (Interaction_HarvestMeat) null;
    foreach (Structure structure in Structure.GetListOfType(StructureBrain.TYPES.DEAD_WORSHIPPER))
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Structure_Info.ID == id)
      {
        body = structure.GetComponent<Interaction_HarvestMeat>();
        if (body.DeadWorshipper.followerInfo.DiedFromRot)
          body = (Interaction_HarvestMeat) null;
        else
          break;
      }
    }
    return body;
  }

  public override void TaskTick(float deltaGameTime)
  {
  }
}
