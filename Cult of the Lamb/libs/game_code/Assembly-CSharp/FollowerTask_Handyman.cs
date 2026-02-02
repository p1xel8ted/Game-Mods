// Decompiled with JetBrains decompiler
// Type: FollowerTask_Handyman
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerTask_Handyman : FollowerTask
{
  public FollowerTask_Handyman.TargetState currentState;
  public Structures_Toolshed targetToolshed;
  public StructureBrain targetStructure;
  public Follower follower;
  public List<InventoryItem> removedItems = new List<InventoryItem>();

  public override FollowerTaskType Type => FollowerTaskType.Handyman;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockSocial => true;

  public override bool BlockThoughts => true;

  public override bool BlockReactTasks => true;

  public FollowerTask_Handyman(int targetToolshed, int targetStructure)
  {
    this.targetToolshed = StructureManager.GetStructureByID<Structures_Toolshed>(targetToolshed);
    this.targetStructure = StructureManager.GetStructureByID<StructureBrain>(targetStructure);
    this.currentState = FollowerTask_Handyman.TargetState.Building;
  }

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    if (this.targetToolshed != null)
      this.targetToolshed.ReservedForTask = true;
    if (this.targetStructure == null)
      return;
    this.targetStructure.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    if (this.targetToolshed != null)
      this.targetToolshed.ReservedForTask = false;
    if (this.targetStructure == null)
      return;
    this.targetStructure.ReservedForTask = false;
  }

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Handyman ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public override void OnStart()
  {
    base.OnStart();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void SimDoingBegin(SimFollower simFollower)
  {
    base.SimDoingBegin(simFollower);
    this.Loop();
  }

  public override void OnArrive()
  {
    base.OnArrive();
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      this.ClearDestination();
      Vector3 destination = this.GetDestination(this.follower);
      float num = Vector3.Distance(this.Brain.LastPosition, destination);
      Debug.Log((object) $"Checking distance to destination: {{Destination:{destination},LastPosition:{this.Brain.LastPosition},Distance:{num}, StoppingDistance:{this.follower.StoppingDistance}}}");
      if ((double) this.GetDistanceToDestination() > 2.0 * (double) this.follower.StoppingDistance)
      {
        Debug.Log((object) "Destination has moved, heading to new location");
        this.SetState(FollowerTaskState.GoingTo);
        return;
      }
    }
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && (UnityEngine.Object) this.follower.Spine != (UnityEngine.Object) null)
      this.follower.SetHat(FollowerHatType.Handyman);
    if (this.currentState == FollowerTask_Handyman.TargetState.Target && (UnityEngine.Object) this.follower != (UnityEngine.Object) null && (UnityEngine.Object) this.follower.Spine != (UnityEngine.Object) null && !this.targetStructure.ReservedByPlayer && (this.targetStructure.Data.IsCollapsed || this.targetStructure.Data.Exhausted) && StructureManager.GetStructureByID<StructureBrain>(this.targetStructure.Data.ID) != null)
      this.follower.TimedAnimation("action", 3.5f, new System.Action(this.Loop));
    else
      this.Loop();
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    follower.Interaction_FollowerInteraction.enabled = true;
    follower.SetHat(FollowerHatType.None);
    follower.SetOutfit(FollowerOutfitType.Follower, false);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this.follower.SetHat(FollowerHatType.None);
    this.follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    this.follower.Interaction_FollowerInteraction.enabled = true;
    if (this.targetToolshed == null)
      return;
    foreach (InventoryItem removedItem in this.removedItems)
    {
      for (int index = 0; index < removedItem.quantity; ++index)
        this.targetToolshed.DepositItemUnstacked((InventoryItem.ITEM_TYPE) removedItem.type);
    }
    this.removedItems.Clear();
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null))
      return;
    this.follower.SetHat(FollowerHatType.None);
    this.follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    this.follower.Interaction_FollowerInteraction.enabled = true;
  }

  public void Loop()
  {
    if (this.targetStructure == null || this.targetToolshed == null)
      this.End();
    else if (this.currentState == FollowerTask_Handyman.TargetState.Target)
    {
      if (this.targetStructure.ReservedByPlayer)
        this.targetStructure = (StructureBrain) null;
      if (this.targetStructure != null)
        this.targetStructure.Repaired();
      this.targetStructure = this.GetNextStructure();
      if (this.targetStructure != null)
      {
        this.currentState = FollowerTask_Handyman.TargetState.Building;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
      else
        this.End();
    }
    else
    {
      if (this.currentState != FollowerTask_Handyman.TargetState.Building)
        return;
      if (this.targetStructure != null)
      {
        if (!this.targetToolshed.HasEnoughResources(new List<InventoryItem>()
        {
          new InventoryItem(StructuresData.GetBuildRubbleType(this.targetStructure.Data.Type, true), 3)
        }))
          return;
        this.targetToolshed.RemoveItems(new List<InventoryItem>()
        {
          new InventoryItem(StructuresData.GetBuildRubbleType(this.targetStructure.Data.Type, true), 3)
        });
        this.removedItems = new List<InventoryItem>()
        {
          new InventoryItem(StructuresData.GetBuildRubbleType(this.targetStructure.Data.Type, true), 3)
        };
        this.currentState = FollowerTask_Handyman.TargetState.Target;
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
      else
        this.End();
    }
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (this.targetToolshed != null)
      return;
    this.Abort();
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.currentState == FollowerTask_Handyman.TargetState.Building)
      return this.targetToolshed.Data.Position;
    return this.currentState == FollowerTask_Handyman.TargetState.Target && this.targetStructure != null ? this.targetStructure.Data.Position : Vector3.zero;
  }

  public static List<int> GetTargetFollowers()
  {
    List<int> targetFollowers = new List<int>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (Interaction_HealingBay.RequiresHealing(allBrain) && !DataManager.Instance.Followers_Imprisoned_IDs.Contains(allBrain.Info.ID) && !DataManager.Instance.Followers_OnMissionary_IDs.Contains(allBrain.Info.ID) && !DataManager.Instance.Followers_LeftInTheDungeon_IDs.Contains(allBrain.Info.ID) && !DataManager.Instance.Followers_Recruit.Contains(allBrain._directInfoAccess) && !DataManager.Instance.Followers_Demons_IDs.Contains(allBrain.Info.ID) && !DataManager.Instance.Followers_TraitManipulating_IDs.Contains(allBrain.Info.ID) && !DataManager.Instance.Followers_Transitioning_IDs.Contains(allBrain.Info.ID) && allBrain.CurrentTaskType != FollowerTaskType.Floating)
        targetFollowers.Add(allBrain.Info.ID);
    }
    return targetFollowers;
  }

  public StructureBrain GetNextStructure()
  {
    List<StructureBrain> ts = new List<StructureBrain>();
    foreach (StructureBrain allBrain in StructureBrain.AllBrains)
    {
      if ((allBrain.Data.IsCollapsed || allBrain.Data.Exhausted) && !allBrain.ReservedByPlayer)
      {
        if (this.targetToolshed.HasEnoughResources(new List<InventoryItem>()
        {
          new InventoryItem(StructuresData.GetBuildRubbleType(allBrain.Data.Type, true), 3)
        }))
          ts.Add(allBrain);
      }
    }
    if (ts.Count <= 0)
      return (StructureBrain) null;
    ts.Shuffle<StructureBrain>();
    return ts[0];
  }

  public enum TargetState
  {
    Building,
    Target,
  }
}
