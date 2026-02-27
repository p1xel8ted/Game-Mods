// Decompiled with JetBrains decompiler
// Type: ObjectiveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private const int kObjectiveHistoryLimit = 10;
  public static Action<string> OnObjectiveGroupCompleted;
  public static Action<string> OnObjectiveTracked;
  public static Action<string> OnObjectiveUntracked;
  private static List<System.Action> _eventQueue = new List<System.Action>();
  private static List<string> _trackingQueue = new List<string>();

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveAdded;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveUpdated;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveCompleted;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveRemoved;

  public static event ObjectiveManager.ObjectiveUpdated OnObjectiveFailed;

  private static List<string> TrackedUniqueGroupIDs
  {
    get => DataManager.Instance.TrackedObjectiveGroupIDs;
    set => DataManager.Instance.TrackedObjectiveGroupIDs = value;
  }

  private static void InvokeOrQueue(System.Action action)
  {
    if (ObjectiveManager.RequiresQueueing())
      ObjectiveManager._eventQueue.Add(action);
    else
      action();
  }

  private static bool RequiresQueueing()
  {
    return (UnityEngine.Object) HUD_Manager.Instance == (UnityEngine.Object) null || HUD_Manager.Instance.Hidden || UIMenuBase.ActiveMenus.Count > 0;
  }

  private static void DispatchQueues()
  {
    if (ObjectiveManager._eventQueue.Count > 0)
    {
      Debug.Log((object) "ObjectiveManager - Dispatch Event Queue".Colour(Color.yellow));
      foreach (System.Action action in ObjectiveManager._eventQueue)
      {
        if (action != null)
          action();
      }
      ObjectiveManager._eventQueue.Clear();
    }
    if (ObjectiveManager._trackingQueue.Count <= 0)
      return;
    Debug.Log((object) "ObjectiveManager - Dispatch Tracking Queue".Colour(Color.yellow));
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
    if (ObjectiveManager.IsTracked(uniqueGroupID))
      return;
    ObjectiveManager.TrackedUniqueGroupIDs.Add(uniqueGroupID);
    if (ObjectiveManager.RequiresQueueing())
    {
      if (ObjectiveManager._trackingQueue.Contains(uniqueGroupID))
        return;
      ObjectiveManager._trackingQueue.Add(uniqueGroupID);
    }
    else
    {
      Action<string> objectiveTracked = ObjectiveManager.OnObjectiveTracked;
      if (objectiveTracked == null)
        return;
      objectiveTracked(uniqueGroupID);
    }
  }

  public static void UntrackGroup(string uniqueGroupID, bool ignoreQueue = false)
  {
    if (!ObjectiveManager.IsTracked(uniqueGroupID))
      return;
    ObjectiveManager.TrackedUniqueGroupIDs.Remove(uniqueGroupID);
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

  public static bool GroupExists(string groupID)
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.GroupId == groupID)
        return true;
    }
    return false;
  }

  public static void Add(ObjectivesData objective, bool autoTrack = false)
  {
    foreach (ObjectivesData objective1 in DataManager.Instance.Objectives)
    {
      if (objective1.ID == objective.ID)
        return;
    }
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

  public static void FailCustomObjective(Objectives.CustomQuestTypes customQuestType)
  {
    for (int index = 0; index < DataManager.Instance.Objectives.Count; ++index)
    {
      ObjectivesData objective = DataManager.Instance.Objectives[index];
      if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == customQuestType)
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
      objective.FailLocked = false;
      if (objective.Type == Objectives.TYPES.PERFORM_RITUAL)
      {
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

  private void OnEnable()
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
  }

  private void OnDisable()
  {
    Debug.Log((object) "ObjectiveManager - OnEnable".Colour(Color.red));
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
  }

  private void Update()
  {
    if (DataManager.Instance == null)
      return;
    for (int index = DataManager.Instance.Objectives.Count - 1; index >= 0; --index)
      DataManager.Instance.Objectives[index].Update();
  }

  private void OnInventoryUpdated()
  {
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COLLECT_ITEM);
  }

  private void OnBrainRemoved(int followerID) => ObjectiveManager.CheckObjectives();

  private void OnStructureAdded(StructuresData structure)
  {
    ObjectiveManager.CheckObjectives(Objectives.TYPES.BUILD_STRUCTURE);
  }

  private void OnFollowerChanged(int followerID)
  {
    ObjectiveManager.CheckObjectives(Objectives.TYPES.RECRUIT_FOLLOWER);
  }

  private void RoomLockController_OnRoomCleared()
  {
    this.StartCoroutine((IEnumerator) this.DelayedCheckCombatObjectives());
  }

  private void StructureBrainRemoved(StructuresData structure)
  {
    this.StartCoroutine((IEnumerator) this.DelayedStructureRemoved());
  }

  private void OnObjectiveCompletedEvent(ObjectivesData objective)
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
    if (!(objective is Objectives_CollectItem objectivesCollectItem) || objectivesCollectItem.ItemType != InventoryItem.ITEM_TYPE.CRYSTAL || !(objectivesCollectItem.CustomTerm == ScriptLocalization.Objectives_Custom.CrystalForLighthouse) || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LighthouseReturn) || ObjectiveManager.HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes.LighthouseReturn))
      return;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/CrystalForLighthouse", Objectives.CustomQuestTypes.LighthouseReturn));
  }

  private void ObjectiveAdded(ObjectivesData objective)
  {
    if (!(objective is Objectives_BuildStructure objectivesBuildStructure) || objectivesBuildStructure.StructureType != StructureBrain.TYPES.DECORATION_BONE_CANDLE || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Decorations2))
      return;
    ObjectiveManager.Add((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Building_Decorations2));
  }

  private IEnumerator DelayedStructureRemoved()
  {
    yield return (object) new WaitForEndOfFrame();
    ObjectiveManager.CheckObjectives(Objectives.TYPES.REMOVE_STRUCTURES);
  }

  private IEnumerator DelayedCheckCombatObjectives()
  {
    yield return (object) new WaitForEndOfFrame();
    ObjectiveManager.CheckObjectives(Objectives.TYPES.NO_DODGE);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.NO_DAMAGE);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.NO_CURSES);
    ObjectiveManager.CheckObjectives(Objectives.TYPES.NO_HEALING);
  }

  private void OnStructureItemDeposited(Structure structure, InventoryItem item)
  {
    if (structure.Type != StructureBrain.TYPES.KITCHEN && structure.Type != StructureBrain.TYPES.KITCHEN_II)
      return;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.DEPOSIT_FOOD);
  }

  private void BiomeGenerator_OnBiomeChangeRoom()
  {
    for (int index = DataManager.Instance.FailedObjectives.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.FailedObjectives[index] is Objectives_RoomChallenge)
        ObjectiveManager.ObjectiveRemoved(DataManager.Instance.FailedObjectives[index]);
    }
  }

  private void OnEnemyKilled(Enemy enemy)
  {
    ObjectiveManager.CheckObjectives(Objectives.TYPES.KILL_ENEMIES);
  }

  private void OnDummyShot()
  {
    Debug.Log((object) "UPDATE!");
    ObjectiveManager.CheckObjectives(Objectives.TYPES.SHOOT_DUMMIES);
  }

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

  private static void OnObjectiveGroupBeginHide(string uniqueGroupID)
  {
    ObjectiveManager.UpdateQuestStatus(uniqueGroupID);
  }

  public static void UpdateQuestStatus(string uniqueGroupID, bool playAnimation = true)
  {
    List<ObjectivesData> objectivesOfGroup = ObjectiveManager.GetAllObjectivesOfGroup(uniqueGroupID);
    if (!ObjectiveManager.AllObjectivesComplete(objectivesOfGroup))
      return;
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
          followerBrain.Stats.DissentGold = Mathf.Floor(UnityEngine.Random.Range(itemQuantity * 0.1f, itemQuantity * 0.4f));
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

  public static bool HasCustomObjective<T>()
  {
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      ObjectivesData objectivesData;
      if ((objectivesData = objective) is T)
      {
        T obj = (T) objectivesData;
        return true;
      }
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

  public static List<T> GetObjectivesOfType<T>()
  {
    List<T> objectivesOfType = new List<T>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      ObjectivesData objectivesData;
      if ((objectivesData = objective) is T)
      {
        T obj = (T) objectivesData;
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

  public delegate void ObjectiveUpdated(ObjectivesData objective);
}
