// Decompiled with JetBrains decompiler
// Type: FollowerTask_Medic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerTask_Medic : FollowerTask
{
  public FollowerTask_Medic.TargetState targetState;
  public int targetStructure;
  public int targetFollower;
  public Follower follower;
  public Follower carryingFollower;
  public Coroutine healingRoutine;
  public List<InventoryItem> removedHealingItems = new List<InventoryItem>();

  public override FollowerTaskType Type => FollowerTaskType.Medic;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool BlockTaskChanges => true;

  public override bool BlockSocial => true;

  public override bool BlockThoughts => true;

  public override bool BlockReactTasks => true;

  public FollowerTask_Medic(int targetStructure, int targetFollower)
  {
    this.targetStructure = targetStructure;
    this.targetFollower = targetFollower;
  }

  public override void ClaimReservations()
  {
    base.ClaimReservations();
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.targetStructure);
    if (structureById == null)
      return;
    structureById.ReservedForTask = true;
  }

  public override void ReleaseReservations()
  {
    base.ReleaseReservations();
    StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.targetStructure);
    if (structureById == null)
      return;
    structureById.ReservedForTask = false;
  }

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return FollowerRole == FollowerRole.Medic ? PriorityCategory.WorkPriority : PriorityCategory.Low;
  }

  public override void OnStart()
  {
    base.OnStart();
    this.targetState = FollowerTask_Medic.TargetState.Building;
    this.SetState(FollowerTaskState.GoingTo);
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
    {
      this.follower.SetHat(FollowerHatType.Medic);
      FollowerBrain.SetFollowerCostume(this.follower.Spine.Skeleton, this.follower.Brain.Info.XPLevel, this.follower.Brain.Info.SkinName, this.follower.Brain.Info.SkinColour, FollowerOutfitType.Medic, this.follower.Brain.Info.Hat, FollowerClothingType.None, this.follower.Brain.Info.Customisation, this.follower.Brain.Info.Special, this.follower.Brain.Info.Necklace, string.Empty, this.follower.Brain._directInfoAccess);
      this.ProgressTask();
    }
    else
      this.End();
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
    this.DropCarryingFollower(true);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      if (this.healingRoutine != null && Interaction_HealingBay.HealingBays.Count > 0)
      {
        Interaction_HealingBay.HealingBays[0].StopCoroutine(this.healingRoutine);
        Interaction_HealingBay.HealingBays[0].SetActivated(false);
        this.healingRoutine = (Coroutine) null;
      }
      this.follower.SetHat(FollowerHatType.None);
      this.follower.SetOutfit(FollowerOutfitType.Follower, false);
      this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
      this.follower.Interaction_FollowerInteraction.enabled = true;
      Structures_Medic structureById = StructureManager.GetStructureByID<Structures_Medic>(this.targetStructure);
      if (structureById != null)
      {
        foreach (InventoryItem removedHealingItem in this.removedHealingItems)
        {
          for (int index = 0; index < removedHealingItem.quantity; ++index)
            structureById.DepositItemUnstacked((InventoryItem.ITEM_TYPE) removedHealingItem.type);
        }
        this.removedHealingItems.Clear();
      }
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(this.targetFollower));
      if (brain != null && brain.CurrentTaskType == FollowerTaskType.EnforcerManualControl)
        brain.CompleteCurrentTask();
    }
    this.DropCarryingFollower(true);
  }

  public void DropCarryingFollower(bool completeTask)
  {
    if (!((UnityEngine.Object) this.carryingFollower != (UnityEngine.Object) null))
      return;
    this.carryingFollower.ShowAllFollowerIcons();
    this.carryingFollower.Spine.transform.localPosition = Vector3.zero;
    Follower f1 = this.carryingFollower;
    Follower f2 = this.follower;
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => f1.transform.parent = f2.transform.parent));
    if (completeTask)
    {
      double num = (double) this.carryingFollower.SetBodyAnimation("idle", true);
      this.carryingFollower.Brain.CompleteCurrentTask();
    }
    this.carryingFollower = (Follower) null;
  }

  public override void OnEnd()
  {
    base.OnEnd();
    if ((UnityEngine.Object) this.follower != (UnityEngine.Object) null)
    {
      this.follower.SetHat(FollowerHatType.None);
      this.follower.SetOutfit(FollowerOutfitType.Follower, false);
      this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
      this.follower.Interaction_FollowerInteraction.enabled = true;
    }
    this.DropCarryingFollower(true);
  }

  public new void ProgressTask()
  {
    this.healingRoutine = (Coroutine) null;
    Structures_Medic structureById = StructureManager.GetStructureByID<Structures_Medic>(this.targetStructure);
    if (structureById == null)
    {
      this.Abort();
    }
    else
    {
      ++this.targetState;
      if (this.targetState == FollowerTask_Medic.TargetState.Loop)
      {
        List<Structures_HealingBay> healingBays = StructureManager.GetAllStructuresOfType<Structures_HealingBay>();
        FollowerBrain brain = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(this.targetFollower));
        Follower follower = FollowerManager.FindFollowerByID(this.targetFollower);
        this.removedHealingItems.AddRange((IEnumerable<InventoryItem>) Interaction_HealingBay.GetCost(brain, healingBays.Count > 0 && healingBays[0].Data.Type == StructureBrain.TYPES.HEALING_BAY_2));
        structureById.RemoveItems(this.removedHealingItems);
        if ((UnityEngine.Object) follower != (UnityEngine.Object) null)
        {
          if (healingBays.Count > 0 && healingBays[0].ReservedByPlayer)
          {
            this.Abort();
          }
          else
          {
            this.carryingFollower = follower;
            this.carryingFollower.TimedAnimation("Medic/carried-stop", 0.9f);
            this.follower.Interaction_FollowerInteraction.enabled = false;
            AudioManager.Instance.PlayOneShot("event:/player/body_drop", this.follower.gameObject);
            this.follower.TimedAnimation("Medic/carry-placedown", 1f, (System.Action) (() =>
            {
              if (healingBays.Count > 0 && healingBays[0].ReservedByPlayer)
              {
                this.follower.Interaction_FollowerInteraction.enabled = true;
                this.Abort();
              }
              else
              {
                this.DropCarryingFollower(false);
                follower.transform.position = healingBays.Count > 0 ? healingBays[0].Data.Position : this.carryingFollower.transform.position;
                if (follower.Brain.CurrentTaskType != FollowerTaskType.EnforcerManualControl)
                  follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_EnforcingManualControl(this.Brain, FollowerTaskType.Medic));
                this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
                if (Interaction_HealingBay.HealingBays.Count == 0)
                {
                  this.follower.Interaction_FollowerInteraction.enabled = true;
                  this.Abort();
                }
                else
                  this.healingRoutine = Interaction_HealingBay.HealingBays[0].StartCoroutine((IEnumerator) Interaction_HealingBay.HealingBays[0].HealingRoutineFromMedic(this.follower, follower, (System.Action) (() =>
                  {
                    this.healingRoutine = (Coroutine) null;
                    this.removedHealingItems.Clear();
                    this.follower.Interaction_FollowerInteraction.enabled = true;
                    if (Interaction_HealingBay.HealingBays.Count > 0 && Interaction_HealingBay.HealingBays[0].structureBrain.ReservedByPlayer)
                      this.Abort();
                    else
                      this.Loop();
                  })));
              }
            }));
          }
        }
        else
        {
          Interaction_HealingBay.HealFollower(brain);
          brain.CompleteCurrentTask();
          this.Loop();
        }
      }
      else
      {
        if (this.targetState == FollowerTask_Medic.TargetState.Follower)
        {
          FollowerBrain brain = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(this.targetFollower));
          if (brain != null)
          {
            if (brain.CurrentTaskType != FollowerTaskType.EnforcerManualControl && (!FollowerManager.FollowerLocked(brain.Info.ID) || brain.HasTrait(FollowerTrait.TraitType.ExistentialDread) || brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified)))
            {
              brain.HardSwapToTask((FollowerTask) new FollowerTask_EnforcingManualControl(this.Brain, FollowerTaskType.Medic));
            }
            else
            {
              this.Abort();
              return;
            }
          }
        }
        else if (this.targetState == FollowerTask_Medic.TargetState.HealingBay)
        {
          FollowerBrain brainById = FollowerBrain.FindBrainByID(this.targetFollower);
          if (brainById == null || !Interaction_HealingBay.RequiresHealing(brainById))
          {
            this.Abort();
            return;
          }
          if (PlayerFarming.Location == FollowerLocation.Base)
          {
            Follower followerById = FollowerManager.FindFollowerByID(this.targetFollower);
            if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
            {
              this.carryingFollower = followerById;
              this.carryingFollower.HideAllFollowerIcons();
              this.carryingFollower.transform.parent = this.follower.transform;
              this.carryingFollower.transform.localPosition = Vector3.zero;
              this.carryingFollower.Spine.transform.localPosition = new Vector3(0.0f, -0.033f, -0.078f);
              double num = (double) this.carryingFollower.SetBodyAnimation("Medic/carried-start", false);
              this.carryingFollower.AddBodyAnimation("Medic/carried-run", true, 0.0f);
              this.carryingFollower.FacePosition(structureById.Data.Position);
              this.follower.FacePosition(structureById.Data.Position);
              this.follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Medic/carry-run");
              AudioManager.Instance.PlayOneShot("event:/player/body_pickup", this.follower.gameObject);
              this.follower.TimedAnimation("Medic/carry-start", 0.5f, (System.Action) (() =>
              {
                this.ClearDestination();
                this.SetState(FollowerTaskState.GoingTo);
              }));
              return;
            }
          }
        }
        this.ClearDestination();
        this.SetState(FollowerTaskState.GoingTo);
      }
    }
  }

  public void Loop()
  {
    Structures_Medic structureById = StructureManager.GetStructureByID<Structures_Medic>(this.targetStructure);
    List<Structures_HealingBay> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_HealingBay>();
    this.targetState = FollowerTask_Medic.TargetState.Building;
    List<int> targetFollowers = FollowerTask_Medic.GetTargetFollowers();
    if (targetFollowers.Count > 0 && structureById != null)
    {
      for (int index = 0; index < targetFollowers.Count; ++index)
      {
        FollowerBrain brain = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(targetFollowers[index]));
        if (brain != null && brain.CurrentTaskType != FollowerTaskType.ManualControl && brain.CurrentTaskType != FollowerTaskType.EnforcerManualControl && (!FollowerManager.FollowerLocked(brain.Info.ID) || brain.HasTrait(FollowerTrait.TraitType.ExistentialDread) || brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified)) && structureById.HasEnoughResources(Interaction_HealingBay.GetCost(brain, structuresOfType[0].Data.Type == StructureBrain.TYPES.HEALING_BAY_2)))
        {
          this.targetFollower = targetFollowers[index];
          this.ProgressTask();
          return;
        }
      }
    }
    this.End();
  }

  public override int GetSubTaskCode() => 0;

  public override void TaskTick(float deltaGameTime)
  {
    if (PlayerFarming.Location == FollowerLocation.Base && (Interaction_HealingBay.HealingBays.Count > 0 && Interaction_HealingBay.HealingBays[0].structureBrain.ReservedByPlayer || Interaction_HealingBay.HealingBays.Count == 0))
    {
      this.Abort();
    }
    else
    {
      if (StructureManager.GetStructureByID<Structures_Medic>(this.targetStructure) != null)
        return;
      this.Abort();
    }
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    if (this.targetState == FollowerTask_Medic.TargetState.Building)
    {
      StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(this.targetStructure);
      if (structureById != null)
        return structureById.Data.Position;
    }
    else if (this.targetState == FollowerTask_Medic.TargetState.Follower)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.targetFollower);
      if (PlayerFarming.Location == FollowerLocation.Base)
      {
        Follower followerById = FollowerManager.FindFollowerByID(this.targetFollower);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
          return followerById.transform.position;
      }
      else if (infoById != null)
        return infoById.LastPosition;
    }
    else if (this.targetState == FollowerTask_Medic.TargetState.HealingBay)
    {
      List<Structures_HealingBay> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_HealingBay>();
      if (structuresOfType.Count > 0)
        return Interaction_HealingBay.HealingBays.Count > 0 ? Interaction_HealingBay.HealingBays[0].FollowerPosition.transform.position : structuresOfType[0].Data.Position;
    }
    return Vector3.zero;
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

  [CompilerGenerated]
  public void \u003CProgressTask\u003Eb__31_1()
  {
    this.healingRoutine = (Coroutine) null;
    this.removedHealingItems.Clear();
    this.follower.Interaction_FollowerInteraction.enabled = true;
    if (Interaction_HealingBay.HealingBays.Count > 0 && Interaction_HealingBay.HealingBays[0].structureBrain.ReservedByPlayer)
      this.Abort();
    else
      this.Loop();
  }

  [CompilerGenerated]
  public void \u003CProgressTask\u003Eb__31_2()
  {
    this.ClearDestination();
    this.SetState(FollowerTaskState.GoingTo);
  }

  public enum TargetState
  {
    Building,
    Follower,
    HealingBay,
    Loop,
  }
}
