// Decompiled with JetBrains decompiler
// Type: FollowerTask_LogisticsPorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_LogisticsPorter : FollowerTask
{
  public FollowerTask_LogisticsPorter.TargetState targetState;
  public int logisticsStructure;
  public int rootstructure;
  public int targetStucture;
  public StructureBrain.TYPES rootStructureType;
  public StructureBrain.TYPES targetStructureType;
  public Follower follower;
  public List<InventoryItem> heldItems = new List<InventoryItem>();

  public override FollowerTaskType Type => FollowerTaskType.Logistics;

  public override FollowerLocation Location => FollowerLocation.Base;

  public StructureBrain.TYPES RootStructureType => this.rootStructureType;

  public StructureBrain.TYPES TargetStructureType => this.targetStructureType;

  public override bool BlockSocial => true;

  public override bool BlockReactTasks => true;

  public override bool BlockTaskChanges => true;

  public virtual bool TransportItems => true;

  public virtual int MaxItems => 10;

  public FollowerTask_LogisticsPorter(
    int logisticsStructure,
    int rootStructure,
    int targetStructure)
  {
    this.logisticsStructure = logisticsStructure;
    this.rootstructure = rootStructure;
    this.targetStucture = targetStructure;
    this.rootStructureType = StructureManager.GetStructureTypeByID(rootStructure);
    this.targetStructureType = StructureManager.GetStructureTypeByID(targetStructure);
  }

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.logisticsStructure);
    if (structureById == null)
      return;
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.logisticsStructure);
    if (structureById == null)
      return;
    structureById.ReservedForTask = false;
  }

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Logistics ? PriorityCategory.ExtremelyUrgent : PriorityCategory.Medium;
  }

  public override void OnStart()
  {
    base.OnStart();
    this.targetState = FollowerTask_LogisticsPorter.TargetState.Home;
    this.SetState(FollowerTaskState.GoingTo);
  }

  public override void OnArrive()
  {
    base.OnArrive();
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null && (UnityEngine.Object) this.follower.Spine != (UnityEngine.Object) null)
    {
      FollowerBrain.SetFollowerCostume(this.follower.Spine.Skeleton, this.follower.Brain.Info.XPLevel, this.follower.Brain.Info.SkinName, this.follower.Brain.Info.SkinColour, FollowerOutfitType.Logistics, this.follower.Brain.Info.Hat, FollowerClothingType.None, this.follower.Brain.Info.Customisation, this.follower.Brain.Info.Special, this.follower.Brain.Info.Necklace, string.Empty, this.follower.Brain._directInfoAccess);
      this.ProgressTask();
    }
    else if (PlayerFarming.Location != FollowerLocation.Base)
      this.ProgressTask();
    else
      this.End();
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.SetOutfit(FollowerOutfitType.Follower, false);
    follower.Interaction_FollowerInteraction.enabled = true;
    follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
      this.follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.DropItems();
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
      this.follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.DropItems();
  }

  public virtual void DropItems()
  {
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      for (int index = 0; index < this.heldItems.Count; ++index)
        InventoryItem.Spawn((InventoryItem.ITEM_TYPE) this.heldItems[index].type, this.heldItems[index].quantity, this.Brain.LastPosition);
    }
    else
    {
      StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.rootstructure);
      if (structureById != null)
      {
        for (int index = 0; index < this.heldItems.Count; ++index)
          structureById.DepositItem((InventoryItem.ITEM_TYPE) this.heldItems[index].type, this.heldItems[index].quantity);
      }
      else
      {
        List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(in PlayerFarming.Location);
        if (structuresOfType.Count > 0)
        {
          for (int index = 0; index < this.heldItems.Count; ++index)
            structuresOfType[0].DepositItem((InventoryItem.ITEM_TYPE) this.heldItems[index].type, this.heldItems[index].quantity);
        }
      }
    }
    this.heldItems.Clear();
  }

  public override void ProgressTask()
  {
    if (this.targetState == FollowerTask_LogisticsPorter.TargetState.Home)
    {
      ++this.targetState;
      this.ClearDestination();
      this.SetState(FollowerTaskState.GoingTo);
    }
    else if (this.targetState == FollowerTask_LogisticsPorter.TargetState.Root)
    {
      StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.rootstructure);
      if (structureById != null)
      {
        List<InventoryItem> collection = this.CollectItemsFromStructure(structureById);
        if (collection == null || this.TransportItems && collection.Count == 0)
        {
          this.End();
        }
        else
        {
          this.heldItems.AddRange((IEnumerable<InventoryItem>) collection);
          if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
          {
            this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Logistics/run-logistics");
            this.follower.Interaction_FollowerInteraction.enabled = false;
            this.follower.TimedAnimation("Logistics/fill-logistics", 4.6f, (System.Action) (() =>
            {
              ++this.targetState;
              this.ClearDestination();
              this.SetState(FollowerTaskState.GoingTo);
              this.follower.Interaction_FollowerInteraction.enabled = true;
            }));
          }
          else
          {
            ++this.targetState;
            this.ClearDestination();
            this.SetState(FollowerTaskState.GoingTo);
          }
        }
      }
      else
        this.End();
    }
    else
    {
      if (this.targetState != FollowerTask_LogisticsPorter.TargetState.Target)
        return;
      StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.targetStucture);
      if (structureById != null)
      {
        List<InventoryItem> leftovers = this.DepositItems(structureById);
        if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
        {
          this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
          this.follower.Interaction_FollowerInteraction.enabled = false;
          this.follower.TimedAnimation("Logistics/empty-logistics", 4.266667f, (System.Action) (() =>
          {
            this.follower.Interaction_FollowerInteraction.enabled = true;
            if (leftovers.Count > 0)
            {
              this.heldItems = leftovers;
              this.DropItems();
            }
            this.heldItems.Clear();
            this.Loop(leftovers);
          }));
        }
        else
        {
          if (leftovers.Count > 0)
          {
            this.heldItems = leftovers;
            this.DropItems();
          }
          this.heldItems.Clear();
          this.Loop(leftovers);
        }
      }
      else
        this.End();
    }
  }

  public virtual List<InventoryItem> CollectItemsFromStructure(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < structureBrain.Data.Inventory.Count; ++index)
      inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) structureBrain.Data.Inventory[index].type, structureBrain.Data.Inventory[index].quantity));
    for (int index = 0; index < inventoryItemList.Count; ++index)
      structureBrain.RemoveItems((InventoryItem.ITEM_TYPE) inventoryItemList[index].type, inventoryItemList[index].quantity);
    return inventoryItemList;
  }

  public virtual List<InventoryItem> DepositItems(StructureBrain structureBrain)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    for (int index = 0; index < this.heldItems.Count; ++index)
    {
      if (structureBrain.Data.Inventory.Count < this.MaxItems)
        structureBrain.DepositItem((InventoryItem.ITEM_TYPE) this.heldItems[index].type, this.heldItems[index].quantity);
      else
        inventoryItemList.Add(new InventoryItem((InventoryItem.ITEM_TYPE) this.heldItems[index].type, this.heldItems[index].quantity));
    }
    return inventoryItemList;
  }

  public virtual void Loop(List<InventoryItem> leftovers) => this.End();

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "CorpseCollect")
      return;
    int num = e.Data.Name == "CorpseDrop" ? 1 : 0;
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.targetState == FollowerTask_LogisticsPorter.TargetState.Home)
    {
      StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.logisticsStructure);
      if (structureById != null)
        return structureById.Data.Position;
    }
    else if (this.targetState == FollowerTask_LogisticsPorter.TargetState.Root)
    {
      StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.rootstructure);
      if (structureById != null)
        return structureById.Data.Position;
    }
    else if (this.targetState == FollowerTask_LogisticsPorter.TargetState.Target)
    {
      StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.targetStucture);
      if (structureById != null)
        return structureById.Data.Position;
    }
    return Vector3.zero;
  }

  [CompilerGenerated]
  public void \u003CProgressTask\u003Eb__38_0()
  {
    ++this.targetState;
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
    this.follower.Interaction_FollowerInteraction.enabled = true;
  }

  public enum TargetState
  {
    Home,
    Root,
    Target,
  }
}
