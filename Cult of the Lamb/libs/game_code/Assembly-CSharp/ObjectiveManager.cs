// Decompiled with JetBrains decompiler
// Type: ObjectiveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ObjectiveManager : BaseMonoBehaviour
{
  public const int kObjectiveHistoryLimit = 10;
  public const int onScreenObjectivesLimit = 3;
  public static Action<string> OnObjectiveGroupCompleted;
  public static Action<string> OnObjectiveTracked;
  public static Action<string> OnObjectiveUntracked;
  public static Action<string> OnObjectiveUntrackedFromQue;
  public static List<System.Action> _eventQueue = new List<System.Action>();
  public static List<string> _trackingQueue = new List<string>();
  public static bool OverrideQueuing = false;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveAdded;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveUpdated;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveCompleted;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveRemoved;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveFailed;

  public static List<string> TrackedUniqueGroupIDs
  {
    get => DataManager.Instance.TrackedObjectiveGroupIDs;
    set => DataManager.Instance.TrackedObjectiveGroupIDs = value;
  }

  public static void InvokeOrQueue(System.Action action)
  {
    if (ObjectiveManager.RequiresQueueing())
      ObjectiveManager._eventQueue.Add(action);
    else
      action();
  }

  public static bool RequiresQueueing()
  {
    if (ObjectiveManager.OverrideQueuing)
      return false;
    return (UnityEngine.Object) HUD_Manager.Instance == (UnityEngine.Object) null || HUD_Manager.Instance.Hidden || UIMenuBase.ActiveMenus.Count > 0;
  }

  public static void DispatchQueues()
  {
    if (ObjectiveManager._eventQueue.Count > 0)
    {
      for (int index = 0; index < ObjectiveManager._eventQueue.Count; ++index)
      {
        System.Action action = ObjectiveManager._eventQueue[index];
        if (action != null)
          action();
      }
      ObjectiveManager._eventQueue.Clear();
    }
    if (ObjectiveManager._trackingQueue.Count <= 0)
      return;
    foreach (string tracking in ObjectiveManager._trackingQueue)
    {
      if (ObjectiveManager.IsTracked(tracking))
      {
        Action<string> objectiveTracked = ObjectiveManager.OnObjectiveTracked;
        if (objectiveTracked != null)
          objectiveTracked(tracking);
      }
      else
      {
        Action<string> objectiveUntracked = ObjectiveManager.OnObjectiveUntracked;
        if (objectiveUntracked != null)
          objectiveUntracked(tracking);
      }
    }
    ObjectiveManager._trackingQueue.Clear();
  }

  public static void TrackGroup(string uniqueGroupID)
  {
    if (!ObjectiveManager.IsTracked(uniqueGroupID))
    {
      ObjectiveManager.TrackedUniqueGroupIDs.Add(uniqueGroupID);
      if (ObjectiveManager.RequiresQueueing())
      {
        if (!ObjectiveManager._trackingQueue.Contains(uniqueGroupID))
          ObjectiveManager._trackingQueue.Add(uniqueGroupID);
      }
      else
      {
        Action<string> objectiveTracked = ObjectiveManager.OnObjectiveTracked;
        if (objectiveTracked != null)
          objectiveTracked(uniqueGroupID);
      }
    }
    ObjectiveManager.CheckTrackedQuestsLimit();
  }

  public static void CheckTrackedQuestsLimit()
  {
    if (ObjectiveManager.TrackedUniqueGroupIDs.Count <= 3)
      return;
    ObjectiveManager.UntrackGroup(ObjectiveManager.TrackedUniqueGroupIDs[0]);
  }

  public static void UntrackGroup(string uniqueGroupID, bool ignoreQueue = false)
  {
    if (!ObjectiveManager.IsTracked(uniqueGroupID))
      return;
    ObjectiveManager.TrackedUniqueGroupIDs.Remove(uniqueGroupID);
    Action<string> untrackedFromQue = ObjectiveManager.OnObjectiveUntrackedFromQue;
    if (untrackedFromQue != null)
      untrackedFromQue(uniqueGroupID);
    if (!ignoreQueue && ObjectiveManager.RequiresQueueing())
    {
      if (ObjectiveManager._trackingQueue.Contains(uniqueGroupID))
        return;
      ObjectiveManager._trackingQueue.Add(uniqueGroupID);
    }
    else
    {
      Action<string> objectiveUntracked = ObjectiveManager.OnObjectiveUntracked;
      if (objectiveUntracked == null)
        return;
      objectiveUntracked(uniqueGroupID);
    }
  }

  public static bool IsTracked(string uniqueGroupID)
  {
    return ObjectiveManager.TrackedUniqueGroupIDs.Contains(uniqueGroupID);
  }

  public static bool AnyTracked() => ObjectiveManager.TrackedUniqueGroupIDs.Count > 0;

  public static List<ObjectivesData> GetAllObjectivesOfGroup(string uniqueGroupID)
  {
    List<ObjectivesData> objectivesOfGroup = new List<ObjectivesData>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.UniqueGroupID == uniqueGroupID)
        objectivesOfGroup.Add(objective);
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective.UniqueGroupID == uniqueGroupID)
        objectivesOfGroup.Add(completedObjective);
    }
    foreach (ObjectivesData failedObjective in DataManager.Instance.FailedObjectives)
    {
      if (failedObjective.UniqueGroupID == uniqueGroupID)
        objectivesOfGroup.Add(failedObjective);
    }
    return objectivesOfGroup;
  }

  public static List<ObjectivesData> GetAllObjectivesOfGroupID(string groupID)
  {
    List<ObjectivesData> objectivesOfGroupId = new List<ObjectivesData>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.GroupId == groupID)
        objectivesOfGroupId.Add(objective);
    }
    return objectivesOfGroupId;
  }

  public static bool AnyQuestsExist()
  {
    return DataManager.Instance.Objectives.Count + DataManager.Instance.CompletedObjectives.Count + DataManager.Instance.FailedObjectives.Count + DataManager.Instance.CompletedObjectivesHistory.Count + DataManager.Instance.FailedObjectivesHistory.Count > 0;
  }

  public static bool AllObjectivesComplete(string groupID)
  {
    return ObjectiveManager.AllObjectivesComplete(ObjectiveManager.GetAllObjectivesOfGroup(groupID));
  }

  public static bool AllObjectivesComplete(List<ObjectivesData> objectivesData)
  {
    int num = 0;
    foreach (ObjectivesData objectivesData1 in objectivesData)
    {
      if (objectivesData1.IsComplete || objectivesData1.IsFailed)
        ++num;
    }
    return num == objectivesData.Count;
  }

  public static List<string> AllObjectiveGroupIDs()
  {
    List<string> stringList = new List<string>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (!stringList.Contains(objective.UniqueGroupID))
        stringList.Add(objective.UniqueGroupID);
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (!stringList.Contains(completedObjective.UniqueGroupID))
        stringList.Add(completedObjective.UniqueGroupID);
    }
    foreach (ObjectivesData failedObjective in DataManager.Instance.FailedObjectives)
    {
      if (!stringList.Contains(failedObjective.UniqueGroupID))
        stringList.Add(failedObjective.UniqueGroupID);
    }
    return stringList;
  }

  public static void MaintainObjectiveHistoryList(ref List<ObjectivesDataFinalized> objectivesList)
  {
    List<string> list = new List<string>();
    foreach (ObjectivesDataFinalized objectivesDataFinalized in objectivesList)
    {
      if (!list.Contains(objectivesDataFinalized.UniqueGroupID))
        list.Add(objectivesDataFinalized.UniqueGroupID);
    }
    string str = list.LastElement<string>();
    while (list.Count > 10)
    {
      objectivesList.RemoveAt(objectivesList.Count - 1);
      if (objectivesList.LastElement<ObjectivesDataFinalized>().UniqueGroupID != str)
      {
        list.Remove(str);
        str = list.LastElement<string>();
      }
    }
  }

  public static bool GroupExists(string groupID, bool includeHistory = false)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.GroupId == groupID)
        return true;
    }
    if (includeHistory)
    {
      foreach (ObjectivesDataFinalized objectivesDataFinalized in DataManager.Instance.CompletedObjectivesHistory)
      {
        if (objectivesDataFinalized != null && objectivesDataFinalized.GroupId == groupID)
          return true;
      }
    }
    return false;
  }

  public static void Add(ObjectivesData objective, bool autoTrack = false, bool isDLCObjective = false)
  {
    foreach (ObjectivesData objective1 in DataManager.Instance.Objectives)
    {
      if (objective1.ID == objective.ID)
        return;
    }
    if (!objective.IsWinterObjective)
      objective.IsWinterObjective = isDLCObjective;
    List<ObjectivesData> objectivesDataList = new List<ObjectivesData>((IEnumerable<ObjectivesData>) DataManager.Instance.Objectives);
    objectivesDataList.AddRange((IEnumerable<ObjectivesData>) DataManager.Instance.CompletedObjectives);
    objectivesDataList.AddRange((IEnumerable<ObjectivesData>) DataManager.Instance.FailedObjectives);
    string str = "";
    foreach (ObjectivesData objectivesData in objectivesDataList)
    {
      if (objectivesData.GroupId == objective.GroupId)
      {
        str = objectivesData.UniqueGroupID;
        break;
      }
    }
    if (string.IsNullOrEmpty(str))
      str = ObjectiveManager.GetUniqueID();
    objective.UniqueGroupID = str;
    DataManager.Instance.Objectives.Add(objective);
    if (!objective.IsInitialised())
      objective.Init(true);
    ObjectiveManager.InvokeOrQueue((System.Action) (() =>
    {
      ObjectiveManager.ObjectiveUpdated onObjectiveAdded = ObjectiveManager.OnObjectiveAdded;
      if (onObjectiveAdded == null)
        return;
      onObjectiveAdded(objective);
    }));
    if (!ObjectiveManager.AnyTracked() || objective.AutoTrack || (double) objective.ExpireTimestamp != -1.0)
      ObjectiveManager.TrackGroup(objective.UniqueGroupID);
    if (objective.Type != Objectives.TYPES.CUSTOM && objective.Type != Objectives.TYPES.SEND_FOLLOWER_BED_REST)
      ObjectiveManager.UpdateObjective(objective);
    if (!autoTrack)
      return;
    ObjectiveManager.TrackGroup(objective.UniqueGroupID);
  }

  public static void CompleteCustomObjective(
    Objectives.CustomQuestTypes customQuestType,
    int targetFollowerID = -1)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == customQuestType)
      {
        ((Objectives_Custom) objective).ResultFollowerID = targetFollowerID;
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void FailObjective(string groupTitle)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.GroupId == groupTitle)
      {
        objective.IsFailed = true;
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteFindChildrenObjective()
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective is Objectives_FindChildren)
      {
        objective.Complete();
        ObjectiveManager.UpdateObjective(objective);
      }
    }
  }

  public static void CompleteShowFleeceObjective()
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective is Objectives_ShowFleece objectivesShowFleece && (PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece == objectivesShowFleece.FleeceType)
      {
        objective.Complete();
        ObjectiveManager.UpdateObjective(objective);
      }
    }
  }

  public static void CompleteAnimalObjective(InventoryItem.ITEM_TYPE animal, int level)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective is Objectives_GetAnimal objectivesGetAnimal && objectivesGetAnimal.AnimalType == animal && level >= objectivesGetAnimal.Level)
      {
        objective.Complete();
        ObjectiveManager.UpdateObjective(objective);
      }
    }
  }

  public static void GiveItem(InventoryItem.ITEM_TYPE itemType)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.GIVE_ITEM && ((Objectives_GiveItem) objective).TargetType == itemType)
      {
        ((Objectives_GiveItem) objective).AddItem(itemType);
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteDefeatKnucklebones(string CharacterNameTerm)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.DEFEAT_KNUCKLEBONES)
        Debug.Log((object) $"{((Objectives_DefeatKnucklebones) objective).CharacterNameTerm}      {CharacterNameTerm}");
      if (objective.Type == Objectives.TYPES.DEFEAT_KNUCKLEBONES && ((Objectives_DefeatKnucklebones) objective).CharacterNameTerm == CharacterNameTerm)
      {
        Debug.Log((object) "REMOVE!".Colour(Color.yellow));
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void FailDefeatKnucklebones(string CharacterNameTerm)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.DEFEAT_KNUCKLEBONES)
        Debug.Log((object) $"{((Objectives_DefeatKnucklebones) objective).CharacterNameTerm}      {CharacterNameTerm}");
      if (objective.Type == Objectives.TYPES.DEFEAT_KNUCKLEBONES && ((Objectives_DefeatKnucklebones) objective).CharacterNameTerm == CharacterNameTerm)
      {
        objective.FailLocked = false;
        objective.Failed();
        break;
      }
    }
  }

  public static void FailCustomObjective(
    Objectives.CustomQuestTypes customQuestType,
    int targetFollowerID = -1)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == customQuestType && ((Objectives_Custom) objective).TargetFollowerID == targetFollowerID)
      {
        objective.FailLocked = false;
        objective.Failed();
      }
    }
  }

  public static void FailObjective(Objectives.TYPES questType)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == questType)
      {
        objective.FailLocked = false;
        objective.Failed();
      }
    }
  }

  public static void FailLockCustomObjective(
    Objectives.CustomQuestTypes customQuestType,
    bool locked)
  {
    Debug.Log((object) ("Unlock custom objective: " + customQuestType.ToString()));
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == customQuestType)
        objective.FailLocked = locked;
    }
  }

  public static void FailUniqueFollowerObjectives(int uniqueFollowerID)
  {
    if (!FollowerManager.UniqueFollowerIDs.Contains(uniqueFollowerID))
      return;
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.CUSTOM)
      {
        if (((Objectives_Custom) objective).TargetFollowerID == uniqueFollowerID)
        {
          objective.FailLocked = false;
          objective.Failed();
        }
      }
      else if (objective.Follower == uniqueFollowerID)
      {
        objective.FailLocked = false;
        objective.Failed();
      }
    }
  }

  public static void SetRitualObjectivesFailLocked()
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      if (DataManager.Instance.Objectives[index].Type == Objectives.TYPES.PERFORM_RITUAL)
        DataManager.Instance.Objectives[index].FailLocked = true;
    }
  }

  public static void CompleteRitualObjective(
    UpgradeSystem.Type ritualType,
    int targetFollowerID_1 = -1,
    int targetFollowerID_2 = -1)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.PERFORM_RITUAL)
      {
        objective.FailLocked = false;
        if (((Objectives_PerformRitual) objective).Ritual == ritualType)
          ((Objectives_PerformRitual) objective).CheckComplete(targetFollowerID_1, targetFollowerID_2);
        if (((Objectives_PerformRitual) objective).Ritual == UpgradeSystem.Type.Ritual_Ressurect || ((Objectives_PerformRitual) objective).Ritual == UpgradeSystem.Type.Ritual_Funeral)
          objective.FailLocked = true;
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteEatMealObjective(StructureBrain.TYPES mealType, int targetFollowerID_1 = -1)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.EAT_MEAL)
      {
        ((Objectives_EatMeal) objective).CheckComplete(mealType, targetFollowerID_1);
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteDrinkObjective(
    InventoryItem.ITEM_TYPE drinkType,
    int targetFollowerID_1 = -1)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.DRINK)
      {
        ((Objectives_Drink) objective).CheckComplete(drinkType, targetFollowerID_1);
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteCraftClothingObjective(FollowerClothingType clothingType)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.CRAFT_CLOTHING)
      {
        ((Objectives_CraftClothing) objective).CheckComplete(clothingType);
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteAssignClothingObjective(
    FollowerClothingType clothingType,
    int targetFollowerID_1 = -1)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.ASSIGN_CLOTHING)
      {
        ((Objectives_AssignClothing) objective).CheckComplete(clothingType, targetFollowerID_1);
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteMatingObjective(int targetFollowerID_1 = -1, int targetFollowerID_2 = -1)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.MATING)
      {
        ((Objectives_Mating) objective).CheckComplete(targetFollowerID_1, targetFollowerID_2);
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompletLegendaryWeaponRunObjective(EquipmentType weapon)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.LEGENDARY_WEAPON_RUN)
      {
        ((Objectives_LegendaryWeaponRun) objective).CheckComplete(weapon);
        if (ObjectiveManager.UpdateObjective(objective))
        {
          --index;
          if (!ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToBlacksmithJobBoard))
            ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Blacksmith", Objectives.CustomQuestTypes.ReturnToBlacksmithJobBoard), true, true);
        }
      }
    }
  }

  public static void CompleteFlowerBasketsObjective()
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.FLOWER_BASKETS)
      {
        ((Objectives_FlowerBaskets) objective).PotFillCheck();
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void UpdateLegendarySwordReturnObjective()
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.LEGENDARY_SWORD_RETURN && ObjectiveManager.UpdateObjective(objective))
        --index;
    }
  }

  public static void CompleteLegendarySwordReturnObjective()
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.LEGENDARY_SWORD_RETURN)
      {
        objective.Complete();
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteFeedAnimalObjective(InventoryItem.ITEM_TYPE food, int targetAnimalID)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.FEED_ANIMAL)
      {
        ((Objectives_FeedAnimal) objective).CheckComplete(targetAnimalID, food);
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public static void CompleteWalkAnimalObjective(int targetAnimalID)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective != null && objective.Type == Objectives.TYPES.WALK_ANIMAL)
      {
        ((Objectives_WalkAnimal) objective).CheckComplete(targetAnimalID);
        if (ObjectiveManager.UpdateObjective(objective))
          --index;
      }
    }
  }

  public void OnEnable()
  {
    Debug.Log((object) "ObjectiveManager - OnDisable".Colour(Color.cyan));
    ObjectiveManager.InvokeOrQueue((System.Action) (() =>
    {
      foreach (string objectiveGroupId in ObjectiveManager.AllObjectiveGroupIDs())
      {
        List<ObjectivesData> objectivesOfGroup = ObjectiveManager.GetAllObjectivesOfGroup(objectiveGroupId);
        if (ObjectiveManager.AllObjectivesComplete(objectivesOfGroup))
        {
          foreach (ObjectivesData objective in objectivesOfGroup)
          {
            if (objective.IsComplete)
            {
              ObjectiveManager.ObjectiveUpdated objectiveCompleted = ObjectiveManager.OnObjectiveCompleted;
              if (objectiveCompleted != null)
                objectiveCompleted(objective);
            }
            else if (objective.IsFailed)
            {
              ObjectiveManager.ObjectiveUpdated onObjectiveFailed = ObjectiveManager.OnObjectiveFailed;
              if (onObjectiveFailed != null)
                onObjectiveFailed(objective);
            }
          }
        }
      }
    }));
    Inventory.OnInventoryUpdated += new Inventory.InventoryUpdated(this.OnInventoryUpdated);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    FollowerManager.OnFollowerAdded += new FollowerManager.FollowerChanged(this.OnFollowerChanged);
    FollowerManager.OnFollowerRemoved += new FollowerManager.FollowerChanged(this.OnFollowerChanged);
    Structure.OnItemDeposited += new Structure.StructureInventoryChanged(this.OnStructureItemDeposited);
    UnitObject.OnEnemyKilled += new UnitObject.EnemyKilled(this.OnEnemyKilled);
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
    RatauGiveSpells.OnDummyShot += new System.Action(this.OnDummyShot);
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.StructureBrainRemoved);
    FollowerBrain.OnBrainRemoved += new Action<int>(this.OnBrainRemoved);
    HUD_Manager.OnShown += new System.Action(ObjectiveManager.DispatchQueues);
    UIMenuBase.OnFinalMenuHidden += new System.Action(ObjectiveManager.DispatchQueues);
    UIObjectiveGroup.OnObjectiveGroupBeginHide += new Action<string>(ObjectiveManager.OnObjectiveGroupBeginHide);
    ObjectiveManager.OnObjectiveCompleted += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveCompletedEvent);
    ObjectiveManager.OnObjectiveAdded += new ObjectiveManager.ObjectiveUpdated(this.ObjectiveAdded);
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
  }

  public void OnDisable()
  {
    Inventory.OnInventoryUpdated -= new Inventory.InventoryUpdated(this.OnInventoryUpdated);
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    FollowerManager.OnFollowerAdded -= new FollowerManager.FollowerChanged(this.OnFollowerChanged);
    FollowerManager.OnFollowerRemoved -= new FollowerManager.FollowerChanged(this.OnFollowerChanged);
    Structure.OnItemDeposited -= new Structure.StructureInventoryChanged(this.OnStructureItemDeposited);
    UnitObject.OnEnemyKilled -= new UnitObject.EnemyKilled(this.OnEnemyKilled);
    RatauGiveSpells.OnDummyShot -= new System.Action(this.OnDummyShot);
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.StructureBrainRemoved);
    FollowerBrain.OnBrainRemoved -= new Action<int>(this.OnBrainRemoved);
    HUD_Manager.OnShown -= new System.Action(ObjectiveManager.DispatchQueues);
    UIMenuBase.OnFinalMenuHidden -= new System.Action(ObjectiveManager.DispatchQueues);
    UIObjectiveGroup.OnObjectiveGroupBeginHide -= new Action<string>(ObjectiveManager.OnObjectiveGroupBeginHide);
    ObjectiveManager.OnObjectiveCompleted -= new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveCompletedEvent);
    ObjectiveManager.OnObjectiveAdded -= new ObjectiveManager.ObjectiveUpdated(this.ObjectiveAdded);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  public void Update()
  {
    if (DataManager.Instance == null)
      return;
    for (int index = DataManager.Instance.Objectives.Count - 1; index >= 0; --index)
      DataManager.Instance.Objectives[index].Update();
  }

  public void OnInventoryUpdated()
  {
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COLLECT_ITEM);
  }

  public void OnBrainRemoved(int followerID) => ObjectiveManager.CheckObjectives();

  public void OnStructureAdded(StructuresData structure)
  {
    ObjectiveManager.CheckObjectives(Objectives.TYPES.BUILD_STRUCTURE);
  }

  public void OnFollowerChanged(int followerID)
  {
    ObjectiveManager.CheckObjectives(Objectives.TYPES.RECRUIT_FOLLOWER);
  }

  public void RoomLockController_OnRoomCleared()
  {
    this.StartCoroutine((IEnumerator) this.DelayedCheckCombatObjectives());
  }

  public void StructureBrainRemoved(StructuresData structure)
  {
    this.StartCoroutine((IEnumerator) this.DelayedStructureRemoved());
  }

  public void OnObjectiveCompletedEvent(ObjectivesData objective)
  {
    if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.CookFirstMeal)
    {
      for (int index = DataManager.Instance.Objectives.Count - 1; index >= 0; --index)
      {
        ObjectivesData objective1 = DataManager.Instance.Objectives[index];
        if (objective1.Type == Objectives.TYPES.COLLECT_ITEM && ((Objectives_CollectItem) objective1).ItemType == InventoryItem.ITEM_TYPE.BERRY)
        {
          ((Objectives_CollectItem) objective1).Count = ((Objectives_CollectItem) objective1).Target;
          ObjectiveManager.UpdateObjective(objective1);
        }
      }
    }
    else if (objective is Objectives_Custom objectivesCustom && (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.NewGamePlus1 || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.NewGamePlus2 || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.NewGamePlus3 || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.NewGamePlus4) && DataManager.Instance.BeatenLeshyLayer2 && DataManager.Instance.BeatenHeketLayer2 && DataManager.Instance.BeatenKallamarLayer2 && DataManager.Instance.BeatenShamuraLayer2 && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.MysticShopReturn) && !ObjectiveManager.HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes.MysticShopReturn))
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom(objective.GroupId, Objectives.CustomQuestTypes.MysticShopReturn));
    if (objective is Objectives_CollectItem objectivesCollectItem)
    {
      if (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.CRYSTAL && objectivesCollectItem.CustomTerm == "Objectives/Custom/CrystalForLighthouse" && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LighthouseReturn) && !ObjectiveManager.HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes.LighthouseReturn))
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/CrystalForLighthouse", Objectives.CustomQuestTypes.LighthouseReturn));
      else if (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.GOD_TEAR && objectivesCollectItem.CustomTerm == "Objectives/CollectItem/DivineCrystals" && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.MysticShopReturn) && !ObjectiveManager.HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes.MysticShopReturn))
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/MysticShop", Objectives.CustomQuestTypes.MysticShopReturn));
      if (!ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToRancher) && (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.YEW_CURSED && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YEW_CURSED) >= 2 || objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.LIGHTNING_SHARD && objectivesCollectItem.Target < 10 && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD) >= 2))
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Ranching", Objectives.CustomQuestTypes.ReturnToRancher), true, true);
      if (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.FISHING_ROD)
        ObjectiveManager.Add((ObjectivesData) new Objectives_GiveItem("Objectives/GroupTitles/LegendaryDagger", "NAMES/Scylla", 1, InventoryItem.ITEM_TYPE.FISHING_ROD, location: FollowerLocation.Dungeon1_5), true, true);
      if (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.MAGMA_STONE && objectivesCollectItem.Target == 5)
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/WarmCult", Objectives.CustomQuestTypes.InteractYngyaShrine), true, true);
    }
    if (objective is Objectives_PlaceStructure objectivesPlaceStructure && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToDecoJobBoard) && objectivesPlaceStructure.GroupId == "Objectives/GroupTitles/JobBoard/Deco")
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Deco", Objectives.CustomQuestTypes.ReturnToDecoJobBoard), true, true);
    if (objective is Objectives_ShowFleece objectivesShowFleece && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToPriestJobBoard) && objectivesShowFleece.GroupId == "Objectives/GroupTitles/JobBoard/Priest")
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Priest", Objectives.CustomQuestTypes.ReturnToPriestJobBoard), true, true);
    if (objective is Objectives_WinFlockadeBet objectivesWinFlockadeBet && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToFlockadeJobBoard) && objectivesWinFlockadeBet.GroupId == "Objectives/GroupTitles/JobBoard/Flockade")
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Flockade", Objectives.CustomQuestTypes.ReturnToFlockadeJobBoard), true, true);
    if (objective is Objectives_BuildStructure objectivesBuildStructure && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnToDecoJobBoard) && objectivesBuildStructure.GroupId == "Objectives/GroupTitles/JobBoard/Deco")
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/JobBoard/Deco", Objectives.CustomQuestTypes.ReturnToDecoJobBoard), true, true);
    if (objective is Objectives_CraftClothing objectivesCraftClothing && objectivesCraftClothing.GroupId == "Objectives/GroupTitles/Executioner")
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Executioner", Objectives.CustomQuestTypes.GivePlimboOutfit));
    if (!(objective is Objectives_BlizzardOffering))
      return;
    bool flag = true;
    List<ObjectivesData> objectivesOfGroup = ObjectiveManager.GetAllObjectivesOfGroup(objective.UniqueGroupID);
    if (objectivesOfGroup.Count <= 0)
      return;
    for (int index = 0; index < objectivesOfGroup.Count; ++index)
    {
      if (!objectivesOfGroup[index].IsComplete)
        flag = false;
    }
    if (!flag)
      return;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom(objectivesOfGroup[0].GroupId, Objectives.CustomQuestTypes.WaitForBlizzardToFinish), true, true);
  }

  public void ObjectiveAdded(ObjectivesData objective)
  {
    if (!(objective is Objectives_BuildStructure objectivesBuildStructure) || objectivesBuildStructure.StructureType != StructureBrain.TYPES.DECORATION_BONE_CANDLE || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Decorations2))
      return;
    ObjectiveManager.Add((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Building_Decorations2));
  }

  public IEnumerator DelayedStructureRemoved()
  {
    yield return (object) new WaitForEndOfFrame();
    ObjectiveManager.CheckObjectives(Objectives.TYPES.REMOVE_STRUCTURES);
  }

  public IEnumerator DelayedCheckCombatObjectives()
  {
    yield return (object) new WaitForEndOfFrame();
    ObjectiveManager.CheckObjectives(Objectives.TYPES.NO_DODGE);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.NO_DAMAGE);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.NO_CURSES);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.NO_HEALING);
  }

  public void OnStructureItemDeposited(Structure structure, InventoryItem item)
  {
    if (structure.Type != StructureBrain.TYPES.KITCHEN && structure.Type != StructureBrain.TYPES.KITCHEN_II)
      return;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.DEPOSIT_FOOD);
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    for (int index = DataManager.Instance.FailedObjectives.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.FailedObjectives[index] is Objectives_RoomChallenge)
        ObjectiveManager.ObjectiveRemoved(DataManager.Instance.FailedObjectives[index]);
    }
  }

  public void OnEnemyKilled(Enemy enemy)
  {
    ObjectiveManager.CheckObjectives(Objectives.TYPES.KILL_ENEMIES);
  }

  public void OnDummyShot()
  {
    Debug.Log((object) "UPDATE!");
    ObjectiveManager.CheckObjectives(Objectives.TYPES.SHOOT_DUMMIES);
  }

  public void OnNewDayStarted() => ObjectiveManager.UpdateLegendarySwordReturnObjective();

  public static void CheckObjectives(Objectives.TYPES objectiveType)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == objectiveType && ObjectiveManager.UpdateObjective(objective))
        --index;
    }
  }

  public static void CheckObjectives()
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (!(objective is Objectives_Custom) && ObjectiveManager.UpdateObjective(objective))
        --index;
    }
  }

  public static void ObjectiveRemoved(ObjectivesData objective)
  {
    ObjectiveManager.ObjectiveUpdated objectiveRemoved = ObjectiveManager.OnObjectiveRemoved;
    if (objectiveRemoved == null)
      return;
    objectiveRemoved(objective);
  }

  public static bool UpdateObjective(ObjectivesData objective)
  {
    bool flag = false;
    ObjectiveManager.InvokeOrQueue((System.Action) (() =>
    {
      ObjectiveManager.ObjectiveUpdated objectiveUpdated = ObjectiveManager.OnObjectiveUpdated;
      if (objectiveUpdated == null)
        return;
      objectiveUpdated(objective);
    }));
    if (objective.TryComplete())
    {
      DataManager.Instance.Objectives.Remove(objective);
      DataManager.Instance.CompletedObjectives.Add(objective);
      ObjectiveManager.InvokeOrQueue((System.Action) (() =>
      {
        ObjectiveManager.ObjectiveUpdated objectiveCompleted = ObjectiveManager.OnObjectiveCompleted;
        if (objectiveCompleted == null)
          return;
        objectiveCompleted(objective);
      }));
      flag = true;
    }
    else if (objective.IsFailed)
    {
      DataManager.Instance.Objectives.Remove(objective);
      DataManager.Instance.FailedObjectives.Add(objective);
      ObjectiveManager.InvokeOrQueue((System.Action) (() =>
      {
        ObjectiveManager.ObjectiveUpdated onObjectiveFailed = ObjectiveManager.OnObjectiveFailed;
        if (onObjectiveFailed == null)
          return;
        onObjectiveFailed(objective);
      }));
      flag = true;
    }
    return flag;
  }

  public static void OnObjectiveGroupBeginHide(string uniqueGroupID)
  {
    ObjectiveManager.UpdateQuestStatus(uniqueGroupID);
  }

  public static void UpdateQuestStatus(string uniqueGroupID, bool playAnimation = true)
  {
    List<ObjectivesData> objectivesOfGroup = ObjectiveManager.GetAllObjectivesOfGroup(uniqueGroupID);
    if (!ObjectiveManager.AllObjectivesComplete(objectivesOfGroup))
      return;
    if (objectivesOfGroup.Count > 0)
    {
      string groupId = objectivesOfGroup[0].GroupId;
      foreach (ObjectivesData objective in objectivesOfGroup)
      {
        ObjectiveManager.RemoveCompleteObjective(objective);
        ObjectiveManager.RemoveFailedObjective(objective);
      }
      if (ObjectiveManager.IsTracked(uniqueGroupID))
        ObjectiveManager.TrackedUniqueGroupIDs.Remove(uniqueGroupID);
      Action<string> objectiveGroupCompleted = ObjectiveManager.OnObjectiveGroupCompleted;
      if (objectiveGroupCompleted == null)
        return;
      objectiveGroupCompleted(groupId);
    }
    else
      Debug.LogWarning((object) "Objectives has length of 0, something has gone wrong with this , break here");
  }

  public static void RemoveCompleteObjective(ObjectivesData objective)
  {
    if (!DataManager.Instance.CompletedObjectives.Contains(objective))
      return;
    DataManager.Instance.CompletedObjectives.Remove(objective);
  }

  public static void RemoveFailedObjective(ObjectivesData objective)
  {
    if (!DataManager.Instance.FailedObjectives.Contains(objective))
      return;
    DataManager.Instance.FailedObjectives.Remove(objective);
  }

  public static int GetNumberOfObjectivesInGroup(string groupID)
  {
    int objectivesInGroup = 0;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.GroupId == groupID || LocalizationManager.GetTranslation(groupID) == objective.GroupId)
        ++objectivesInGroup;
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective.GroupId == groupID || LocalizationManager.GetTranslation(groupID) == completedObjective.GroupId)
        ++objectivesInGroup;
    }
    return objectivesInGroup;
  }

  public static List<ObjectivesData> GetCompletedObjectivesInGroup(string groupID)
  {
    List<ObjectivesData> objectivesInGroup = new List<ObjectivesData>();
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective.GroupId == groupID)
        objectivesInGroup.Add(completedObjective);
    }
    return objectivesInGroup;
  }

  public static void ObjectiveFailed(ObjectivesData objective)
  {
    if (objective.Type != Objectives.TYPES.CUSTOM)
      return;
    switch (((Objectives_Custom) objective).CustomQuestType)
    {
      case Objectives.CustomQuestTypes.CrisisOfFaith:
        List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
        for (int index = FollowerBrain.AllBrains.Count - 1; index >= 0; --index)
        {
          if (!FollowerManager.FollowerLocked(FollowerBrain.AllBrains[index].Info.ID) && FollowerBrain.AllBrains[index].Info.CursedState == Thought.None)
            followerBrainList.Add(FollowerBrain.AllBrains[index]);
        }
        for (int index = 0; index < 2 && followerBrainList.Count != 0; ++index)
        {
          float itemQuantity = (float) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
          FollowerBrain followerBrain = followerBrainList[UnityEngine.Random.Range(0, followerBrainList.Count)];
          followerBrain.Stats.DissentGold = Mathf.Floor(UnityEngine.Random.Range(itemQuantity * 0.1f, itemQuantity * 0.25f));
          followerBrain.LeavingCult = true;
          followerBrainList.Remove(followerBrain);
        }
        break;
      case Objectives.CustomQuestTypes.GameOver:
        DataManager.Instance.GameOver = true;
        break;
    }
  }

  public static bool HasCustomObjectiveOfType(Objectives.CustomQuestTypes type)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_Custom objectivesCustom && objectivesCustom.CustomQuestType == type)
        return true;
    }
    return false;
  }

  public static bool HasCustomObjectiveOfAnyType(Objectives.CustomQuestTypes[] types)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_Custom objectivesCustom && types.Contains<Objectives.CustomQuestTypes>(objectivesCustom.CustomQuestType))
        return true;
    }
    return false;
  }

  public static bool HasCustomObjective<T>()
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is T)
        return true;
    }
    return false;
  }

  public static bool IsAnimalValid(StructuresData.Ranchable_Animal animal)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_GetAnimal objectivesGetAnimal && objectivesGetAnimal.AnimalType == animal.Type && animal.Level >= objectivesGetAnimal.Level)
        return true;
    }
    return false;
  }

  public static bool HasAnimalObjectiveActive()
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_GetAnimal)
        return true;
    }
    return false;
  }

  public static bool HasCustomObjective(Objectives.TYPES objectiveType)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Type == objectiveType)
        return true;
    }
    return false;
  }

  public static List<ObjectivesData> GetCustomObjectives(
    Objectives.TYPES objectiveType,
    bool includeActive = true,
    bool includedCompleted = false)
  {
    List<ObjectivesData> customObjectives = new List<ObjectivesData>();
    if (includeActive)
    {
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective.Type == objectiveType)
          customObjectives.Add(objective);
      }
    }
    if (includedCompleted)
    {
      foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
      {
        if (completedObjective.Type == objectiveType)
          customObjectives.Add(completedObjective);
      }
    }
    return customObjectives;
  }

  public static List<T> GetCustomCompletedObjectives<T>() where T : ObjectivesDataFinalized
  {
    List<T> completedObjectives = new List<T>();
    foreach (ObjectivesDataFinalized objectivesDataFinalized in DataManager.Instance.CompletedObjectivesHistory)
    {
      if (objectivesDataFinalized is T obj)
        completedObjectives.Add(obj);
    }
    return completedObjectives;
  }

  public static bool HasBuildStructureObjective(StructureBrain.TYPES structure)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_BuildStructure objectivesBuildStructure && objectivesBuildStructure.StructureType == structure)
        return true;
    }
    return false;
  }

  public static List<T> GetObjectivesOfType<T>()
  {
    List<T> objectivesOfType = new List<T>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is T)
      {
        T obj = objective as T;
        objectivesOfType.Add(obj);
      }
    }
    return objectivesOfType;
  }

  public static bool HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes type)
  {
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective is Objectives_Custom objectivesCustom && objectivesCustom.CustomQuestType == type)
        return true;
    }
    return false;
  }

  public static string GetUniqueID() => (++DataManager.Instance.ObjectiveGroupID).ToString();

  public static List<Objectives_GiveItem> GetGiveItemObjectives(
    InventoryItem.ITEM_TYPE item,
    bool checkComplete = true)
  {
    List<Objectives_GiveItem> objectivesOfType = ObjectiveManager.GetObjectivesOfType<Objectives_GiveItem>();
    for (int index = objectivesOfType.Count - 1; index >= 0; --index)
    {
      if (checkComplete && objectivesOfType[index].IsComplete)
        objectivesOfType.Remove(objectivesOfType[index]);
      else if (objectivesOfType[index].TargetType != item)
        objectivesOfType.Remove(objectivesOfType[index]);
    }
    return objectivesOfType;
  }

  public static bool HasCollectItemObjective(
    InventoryItem.ITEM_TYPE item,
    string groupID = null,
    bool includeCompleted = true)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if ((includeCompleted || !objective.IsComplete) && (groupID == null || !(objective.GroupId != groupID)) && objective is Objectives_CollectItem objectivesCollectItem && objectivesCollectItem.ItemType == item)
        return true;
    }
    return false;
  }

  public static bool HasObjectiveOfGroupID(string groupID)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.GroupId == groupID)
        return true;
    }
    return false;
  }

  public delegate void ObjectiveUpdated(ObjectivesData objective);
}
