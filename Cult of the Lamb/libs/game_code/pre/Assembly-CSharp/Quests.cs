// Decompiled with JetBrains decompiler
// Type: Quests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public static class Quests
{
  public const int MaxQuestsActive = 1;
  private static List<ObjectivesData> QuestsAll = new List<ObjectivesData>()
  {
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_AlmsToPoor, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_AssignFaithEnforcer, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_AssignTaxCollector, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Brainwashing, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_ConsumeFollower, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_DonationRitual, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Enlightenment, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Fast, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Feast, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_FasterBuilding, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Fightpit, requiredFollowers: 2, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_FishingRitual, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Funeral, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_HarvestRitual, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Holiday, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Ressurect, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Sacrifice, requiredFollowers: 1, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Wedding, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_WorkThroughNight, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.SendFollowerOnMissionary, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.SendFollowerToPrison, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_CookMeal("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.NONE, 10, 4800f),
    (ObjectivesData) new Objectives_CookMeal("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.MEAL_GREAT, 3, 9600f),
    (ObjectivesData) new Objectives_CookMeal("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.MEAL_GREAT_FISH, 2, 4800f),
    (ObjectivesData) new Objectives_PlaceStructure("Objectives/GroupTitles/Quest", StructureBrain.Categories.AESTHETIC, 3, 4800f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.UseFirePit, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/Quest", StructureBrain.TYPES.OUTHOUSE, 2, 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.MurderFollower, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.MurderFollowerAtNight, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_EatMeal("Objectives/GroupTitles/Quest", StructureBrain.TYPES.MEAL_POOP, 3600f),
    (ObjectivesData) new Objectives_EatMeal("Objectives/GroupTitles/Quest", StructureBrain.TYPES.MEAL_FOLLOWER_MEAT, 4800f),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.BecomeStarving, Random.Range(2, 3)),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.Ill, Random.Range(2, 3)),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.OldAge, Random.Range(2, 3) - 1),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.Dissenter, Random.Range(2, 3) - 1),
    (ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.FLOWER_RED, 10, false, FollowerLocation.Dungeon1_1, 4800f),
    (ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 10, false, FollowerLocation.Dungeon1_2, 4800f),
    (ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.CRYSTAL, 10, false, FollowerLocation.Dungeon1_3, 4800f),
    (ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.SPIDER_WEB, 10, false, FollowerLocation.Dungeon1_4, 4800f),
    (ObjectivesData) new Objectives_FindFollower("Objectives/GroupTitles/Quest", FollowerLocation.Dungeon1_1, "Cat", 0, 0, "Test", 0, 4800f),
    (ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/Quest", StructureBrain.TYPES.DECORATION_BONE_CANDLE, expireTimestamp: 3600f),
    (ObjectivesData) new Objectives_TalkToFollower("Objectives/GroupTitles/Quest", "Story_6_0/Response", 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.KillFollower, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_FindFollower("Objectives/GroupTitles/Quest", FollowerLocation.Dungeon1_2, "Cat", 0, 0, "Test", 1, 4800f),
    (ObjectivesData) new Objectives_FindFollower("Objectives/GroupTitles/Quest", FollowerLocation.Dungeon1_3, "Cat", 0, 0, "Test", 2, 4800f),
    (ObjectivesData) new Objectives_FindFollower("Objectives/GroupTitles/Quest", FollowerLocation.Dungeon1_4, "Cat", 0, 0, "Test", 3, 4800f),
    (ObjectivesData) new Objectives_EatMeal("Objectives/GroupTitles/Quest", StructureBrain.TYPES.MEAL_GRASS, 3600f)
  };
  private static StoryObjectiveData[] AllStoryObjectiveDatas = new StoryObjectiveData[0];
  public static bool IsDebug = false;
  public static List<Objectives_RoomChallenge> DungeonRoomChallenges = new List<Objectives_RoomChallenge>()
  {
    (Objectives_RoomChallenge) new Objectives_NoCurses("Objectives/GroupTitles/Challenge", 3),
    (Objectives_RoomChallenge) new Objectives_NoDamage("Objectives/GroupTitles/Challenge", 3),
    (Objectives_RoomChallenge) new Objectives_NoDodge("Objectives/GroupTitles/Challenge", 3)
  };

  public static ObjectivesData GetQuest(
    int followerID,
    int targetFollowerID_1 = -1,
    int targetFollowerID_2 = -1,
    int deadFollowerID = -1,
    bool assignTargetFollowers = false,
    ObjectivesData targetQuest = null)
  {
    float num = 1f;
    List<ObjectivesData> objectivesDataList1 = new List<ObjectivesData>();
    List<ObjectivesData> objectivesDataList2 = new List<ObjectivesData>();
    if (targetFollowerID_1 != -1 && (followerID != 666 || targetQuest != null))
    {
      objectivesDataList1.AddRange((IEnumerable<ObjectivesData>) Quests.QuestsAll);
      for (int index1 = objectivesDataList1.Count - 1; index1 >= 0; --index1)
      {
        objectivesDataList1[index1].Index = index1;
        if (index1 == 25)
        {
          objectivesDataList1[index1] = (ObjectivesData) null;
        }
        else
        {
          bool flag1 = false;
          for (int index2 = 0; index2 < DataManager.Instance.CompletedQuestsHistorys.Count; ++index2)
          {
            if (DataManager.Instance.CompletedQuestsHistorys[index2].QuestIndex == index1 && !DataManager.Instance.CompletedQuestsHistorys[index2].IsStory && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.CompletedQuestsHistorys[index2].QuestTimestamp < (double) DataManager.Instance.CompletedQuestsHistorys[index2].QuestCooldownDuration)
              flag1 = true;
          }
          if (flag1 && targetQuest == null)
          {
            objectivesDataList1[index1] = (ObjectivesData) null;
          }
          else
          {
            if ((double) CultFaithManager.CurrentFaith < 25.0 && objectivesDataList1[index1] != null && objectivesDataList1[index1] is Objectives_PlaceStructure && ((Objectives_PlaceStructure) objectivesDataList1[index1]).category == StructureBrain.Categories.AESTHETIC)
              objectivesDataList2.Add(objectivesDataList1[index1]);
            if (objectivesDataList1[index1] is Objectives_PerformRitual)
            {
              Objectives_PerformRitual objectivesPerformRitual = (Objectives_PerformRitual) objectivesDataList1[index1];
              if (assignTargetFollowers)
              {
                objectivesPerformRitual.TargetFollowerID_1 = targetFollowerID_1;
                objectivesPerformRitual.TargetFollowerID_2 = targetFollowerID_2;
              }
              if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Sacrifice || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Wedding || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Fightpit || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_AssignFaithEnforcer || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_AssignTaxCollector || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_ConsumeFollower)
              {
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Fightpit & assignTargetFollowers)
                {
                  objectivesPerformRitual.TargetFollowerID_2 = followerID;
                  if (objectivesPerformRitual.TargetFollowerID_1 == objectivesPerformRitual.TargetFollowerID_2)
                    objectivesPerformRitual.TargetFollowerID_2 = targetFollowerID_2;
                }
                else if (assignTargetFollowers)
                {
                  objectivesPerformRitual.TargetFollowerID_1 = followerID;
                  objectivesPerformRitual.TargetFollowerID_2 = -1;
                }
                if (FollowerInfo.GetInfoByID(followerID) == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
              if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Ressurect)
              {
                if (deadFollowerID != -1)
                {
                  if (assignTargetFollowers)
                    objectivesPerformRitual.TargetFollowerID_1 = deadFollowerID;
                  objectivesPerformRitual.FailLocked = true;
                }
                else
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
              if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Funeral)
              {
                if (deadFollowerID != -1)
                {
                  List<Structures_Grave> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Grave>(FollowerLocation.Base);
                  bool flag2 = false;
                  foreach (StructureBrain structureBrain in structuresOfType)
                  {
                    if (structureBrain.Data.FollowerID == deadFollowerID)
                    {
                      flag2 = true;
                      break;
                    }
                  }
                  if (flag2)
                  {
                    if (assignTargetFollowers)
                      objectivesPerformRitual.TargetFollowerID_1 = deadFollowerID;
                    objectivesPerformRitual.FailLocked = true;
                  }
                  else
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
                else
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
              if (!UpgradeSystem.GetUnlocked(objectivesPerformRitual.Ritual) || (double) UpgradeSystem.GetCoolDownNormalised(objectivesPerformRitual.Ritual) > 0.0)
                objectivesDataList1[index1] = (ObjectivesData) null;
              if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Sacrifice && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.AgainstSacrifice))
                objectivesDataList1[index1] = (ObjectivesData) null;
              if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Wedding && FollowerInfo.GetInfoByID(followerID) != null && FollowerInfo.GetInfoByID(followerID).MarriedToLeader)
                objectivesDataList1[index1] = (ObjectivesData) null;
              if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Funeral && FollowerInfo.GetInfoByID(followerID, true) != null && FollowerInfo.GetInfoByID(followerID, true).HadFuneral)
                objectivesDataList1[index1] = (ObjectivesData) null;
              if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Fightpit && (targetFollowerID_2 == -1 || targetFollowerID_2 == targetFollowerID_1 || FollowerInfo.GetInfoByID(targetFollowerID_1) == null || FollowerInfo.GetInfoByID(targetFollowerID_2) == null))
                objectivesDataList1[index1] = (ObjectivesData) null;
              if ((objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Sacrifice || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_ConsumeFollower) && DataManager.Instance.Followers.Count < 5 && targetQuest == null)
                objectivesDataList1[index1] = (ObjectivesData) null;
            }
            else if (objectivesDataList1[index1] is Objectives_Custom)
            {
              Objectives_Custom objectivesCustom = (Objectives_Custom) objectivesDataList1[index1];
              if (assignTargetFollowers)
                objectivesCustom.TargetFollowerID = targetFollowerID_1;
              if (FollowerInfo.GetInfoByID(targetFollowerID_1) == null)
                objectivesDataList1[index1] = (ObjectivesData) null;
              if (assignTargetFollowers)
              {
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.MurderFollowerAtNight)
                  objectivesCustom.TargetFollowerID = followerID;
                else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerOnMissionary)
                  objectivesCustom.TargetFollowerAllowOldAge = false;
                else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.UseFirePit)
                  objectivesCustom.TargetFollowerID = -1;
              }
              if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerOnMissionary && StructureManager.GetAllStructuresOfType<Structures_Missionary>().Count <= 0 || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison && StructureManager.GetAllStructuresOfType<Structures_Prison>().Count <= 0 || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.MurderFollower && !DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower) || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.MurderFollowerAtNight && !DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower) || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.UseFirePit && StructureManager.GetAllStructuresOfType<Structures_DancingFirePit>().Count <= 0)
                objectivesDataList1[index1] = (ObjectivesData) null;
              if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.KillFollower && targetQuest == null)
                objectivesDataList1[index1] = (ObjectivesData) null;
              if ((objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.MurderFollower || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.MurderFollowerAtNight) && DataManager.Instance.Followers.Count < 5 && targetQuest == null)
                objectivesDataList1[index1] = (ObjectivesData) null;
            }
            else if (objectivesDataList1[index1] is Objectives_FindFollower)
            {
              FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
              if (infoById == null)
              {
                objectivesDataList1[index1] = (ObjectivesData) null;
              }
              else
              {
                Objectives_FindFollower objectivesFindFollower = (Objectives_FindFollower) objectivesDataList1[index1];
                objectivesFindFollower.TargetFollowerName = FollowerInfo.GenerateName();
                objectivesFindFollower.FollowerSkin = infoById.SkinName;
                objectivesFindFollower.FollowerVariant = infoById.SkinVariation;
                objectivesFindFollower.FollowerColour = infoById.SkinColour;
                if (objectivesFindFollower.TargetLocation != FollowerLocation.Base && !DataManager.Instance.DungeonCompleted(objectivesFindFollower.TargetLocation))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesFindFollower.TargetLocation == FollowerLocation.Dungeon1_1 && targetQuest == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
            }
            else if (objectivesDataList1[index1] is Objectives_BuildStructure)
            {
              Objectives_BuildStructure objectivesBuildStructure = (Objectives_BuildStructure) objectivesDataList1[index1];
              if (objectivesBuildStructure.StructureType == StructureBrain.TYPES.DECORATION_BONE_CANDLE)
              {
                if (followerID != 666 || targetQuest == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                else
                  continue;
              }
              if (objectivesBuildStructure.StructureType == StructureBrain.TYPES.OUTHOUSE && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Outhouse))
                objectivesDataList1[index1] = (ObjectivesData) null;
              else if (StructureManager.GetAllStructuresOfType(FollowerLocation.Base, objectivesBuildStructure.StructureType).Count > 0)
                objectivesDataList1[index1] = (ObjectivesData) null;
            }
            else if (objectivesDataList1[index1] is Objectives_CookMeal)
            {
              Objectives_CookMeal objectivesCookMeal = (Objectives_CookMeal) objectivesDataList1[index1];
              if (!CookingData.CanMakeMeal(objectivesCookMeal.MealType) && CookingData.GetCookedMeal(objectivesCookMeal.MealType) <= 0 && targetQuest == null)
                objectivesDataList1[index1] = (ObjectivesData) null;
            }
            else if (objectivesDataList1[index1] is Objectives_EatMeal)
            {
              Objectives_EatMeal objectivesEatMeal = (Objectives_EatMeal) objectivesDataList1[index1];
              if (objectivesEatMeal.MealType == StructureBrain.TYPES.MEAL_FOLLOWER_MEAT)
              {
                if (DataManager.Instance.Followers.Count < 5 && targetQuest == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT) <= 0 && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Sacrifice) && !DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (DataManager.Instance.RecipesDiscovered.Contains(InventoryItem.ITEM_TYPE.MEAL_FOLLOWER_MEAT) && targetQuest == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
              if (objectivesEatMeal.MealType == StructureBrain.TYPES.MEAL_POOP && TimeManager.CurrentDay < 10 && targetQuest == null)
                objectivesDataList1[index1] = (ObjectivesData) null;
              else if (objectivesEatMeal.MealType == StructureBrain.TYPES.MEAL_POOP && TimeManager.CurrentDay >= 10 && !DataManager.Instance.RecipesDiscovered.Contains(InventoryItem.ITEM_TYPE.MEAL_POOP) && targetQuest == null)
                objectivesDataList2.Add((ObjectivesData) objectivesEatMeal);
              if (objectivesEatMeal.MealType == StructureBrain.TYPES.MEAL_GRASS && DataManager.Instance.RecipesDiscovered.Contains(InventoryItem.ITEM_TYPE.MEAL_GRASS) && targetQuest == null)
                objectivesDataList1[index1] = (ObjectivesData) null;
              else if (objectivesEatMeal.MealType == StructureBrain.TYPES.MEAL_GRASS && !DataManager.Instance.RecipesDiscovered.Contains(InventoryItem.ITEM_TYPE.MEAL_GRASS) && targetQuest == null)
                objectivesDataList2.Add((ObjectivesData) objectivesEatMeal);
              objectivesEatMeal.TargetFollower = followerID;
            }
            else if (objectivesDataList1[index1] is Objectives_CollectItem)
            {
              Objectives_CollectItem objectivesCollectItem = (Objectives_CollectItem) objectivesDataList1[index1];
              if (objectivesCollectItem.TargetLocation != FollowerLocation.Base && !DataManager.Instance.DungeonCompleted(objectivesCollectItem.TargetLocation))
                objectivesDataList1[index1] = (ObjectivesData) null;
              if (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.MUSHROOM_SMALL && !DataManager.Instance.VisitedLocations.Contains(FollowerLocation.Hub1_Sozo))
                objectivesDataList1[index1] = (ObjectivesData) null;
            }
            else if (objectivesDataList1[index1] is Objectives_RecruitCursedFollower)
            {
              Objectives_RecruitCursedFollower recruitCursedFollower = (Objectives_RecruitCursedFollower) objectivesDataList1[index1];
              foreach (DataManager.QuestHistoryData completedQuestsHistory in DataManager.Instance.CompletedQuestsHistorys)
              {
                if (Quests.QuestsAll[completedQuestsHistory.QuestIndex].Type == Objectives.TYPES.RECRUIT_CURSED_FOLLOWER && (double) TimeManager.TotalElapsedGameTime - (double) completedQuestsHistory.QuestTimestamp < (double) completedQuestsHistory.QuestCooldownDuration)
                {
                  objectivesDataList1[index1] = (ObjectivesData) null;
                  break;
                }
              }
              if (objectivesDataList1[index1] != null)
              {
                if (TimeManager.CurrentDay >= 10 || TimeManager.CurrentDay > 5 && DataManager.Instance.Followers.Count < 3)
                  objectivesDataList2.Add((ObjectivesData) recruitCursedFollower);
                else
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
            }
            else if (objectivesDataList1[index1] is Objectives_TalkToFollower)
            {
              if (targetQuest == null)
                objectivesDataList1[index1] = (ObjectivesData) null;
              else
                ((Objectives_TalkToFollower) objectivesDataList1[index1]).TargetFollower = targetFollowerID_1;
            }
            else if (objectivesDataList1[index1] is Objectives_PlaceStructure objectivesPlaceStructure)
            {
              bool flag3 = false;
              foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
              {
                if (StructuresData.GetCategory(allStructure) == StructureBrain.Categories.AESTHETIC && StructuresData.GetUnlocked(allStructure))
                {
                  flag3 = true;
                  break;
                }
              }
              if (objectivesPlaceStructure.category == StructureBrain.Categories.AESTHETIC && !flag3)
                objectivesDataList1[index1] = (ObjectivesData) null;
            }
          }
        }
      }
    }
    if (targetQuest != null)
      return objectivesDataList2.Contains(targetQuest) || objectivesDataList1.Contains(targetQuest) ? targetQuest : (ObjectivesData) null;
    if (objectivesDataList2.Count > 0 && targetQuest == null)
      return Quests.GetQuest(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, true, objectivesDataList2[Random.Range(0, objectivesDataList2.Count)]);
    if (targetQuest == null)
    {
      Objectives_Story currentStoryObjective = Quests.GetCurrentStoryObjective(followerID);
      if (currentStoryObjective != null)
        return (ObjectivesData) currentStoryObjective;
      if ((double) Random.value < (double) num && TimeManager.CurrentDay > 3)
      {
        StoryData newStory = Quests.GetNewStory(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID);
        if (newStory != null)
        {
          DataManager.Instance.StoryObjectives.Add(newStory);
          return (ObjectivesData) Quests.GetCurrentStoryObjective(followerID);
        }
      }
    }
    if (targetQuest != null)
      return targetQuest;
    Dictionary<int, ObjectivesData> source = new Dictionary<int, ObjectivesData>();
    for (int index = 0; index < objectivesDataList1.Count; ++index)
    {
      if (objectivesDataList1[index] != null)
        source.Add(index, objectivesDataList1[index]);
    }
    if (source.Count == 0)
      return (ObjectivesData) null;
    KeyValuePair<int, ObjectivesData> keyValuePair = source.ElementAt<KeyValuePair<int, ObjectivesData>>(Random.Range(0, source.Count));
    return Quests.GetQuest(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, true, keyValuePair.Value);
  }

  public static ObjectivesData GetRandomBaseQuest(
    int followerID,
    int targetFollowerID_1 = -1,
    int targetFollowerID_2 = -1,
    int deadFollowerID = -1)
  {
    int num = 0;
    while (num++ < 32 /*0x20*/)
    {
      ObjectivesData quest = Quests.GetQuest(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID);
      if (quest != null && !Quests.ObjectiveAlreadyActive(quest))
      {
        quest.ResetInitialisation();
        return quest;
      }
    }
    return (ObjectivesData) null;
  }

  private static bool ObjectiveAlreadyActive(ObjectivesData objective)
  {
    foreach (ObjectivesData objective1 in DataManager.Instance.Objectives)
    {
      if (objective1.Type == objective.Type && objective1.GroupId == objective.GroupId && objective1.Text == objective.Text)
        return true;
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective.Type == objective.Type && completedObjective.GroupId == objective.GroupId && completedObjective.Text == objective.Text)
        return true;
    }
    return false;
  }

  public static StoryData GetNewStory(
    int questGiverFollowerID,
    int targetFollowerID_1 = -1,
    int targetFollowerID_2 = -1,
    int deadFollowerID = -1)
  {
    if (Quests.AllStoryObjectiveDatas.Length == 0)
      Quests.AllStoryObjectiveDatas = Resources.LoadAll<StoryObjectiveData>("Data/Story Data");
    List<int> intList = new List<int>();
    intList.Add(questGiverFollowerID);
    intList.Add(targetFollowerID_1);
    intList.Add(targetFollowerID_2);
    intList.Add(deadFollowerID);
    for (int index = intList.Count - 1; index >= 0; --index)
    {
      if (intList[index] == -1)
        intList.RemoveAt(index);
    }
    foreach (StoryData storyObjective in DataManager.Instance.StoryObjectives)
    {
      if (intList.Contains(storyObjective.EntryStoryItem.QuestGiverFollowerID) || intList.Contains(storyObjective.EntryStoryItem.TargetFollowerID_1) || intList.Contains(storyObjective.EntryStoryItem.TargetFollowerID_2) || intList.Contains(storyObjective.EntryStoryItem.DeadFollowerID))
        return (StoryData) null;
    }
    List<StoryObjectiveData> storyObjectiveDataList = new List<StoryObjectiveData>();
    foreach (StoryObjectiveData storyObjectiveData in Quests.AllStoryObjectiveDatas)
    {
      if (storyObjectiveData.IsEntryStory)
      {
        bool flag1 = false;
        foreach (StoryData storyObjective in DataManager.Instance.StoryObjectives)
        {
          if (storyObjectiveData.UniqueStoryID == storyObjective.EntryStoryItem.StoryObjectiveData.UniqueStoryID)
          {
            flag1 = true;
            break;
          }
        }
        if (!flag1)
        {
          bool flag2 = false;
          for (int index = 0; index < DataManager.Instance.CompletedQuestsHistorys.Count; ++index)
          {
            if (DataManager.Instance.CompletedQuestsHistorys[index].QuestIndex == storyObjectiveData.UniqueStoryID && DataManager.Instance.CompletedQuestsHistorys[index].IsStory && (double) TimeManager.TotalElapsedGameTime - (double) DataManager.Instance.CompletedQuestsHistorys[index].QuestTimestamp < (double) DataManager.Instance.CompletedQuestsHistorys[index].QuestCooldownDuration)
              flag2 = true;
          }
          bool flag3 = true;
          if (storyObjectiveData.ConditionalVariables != null)
          {
            foreach (BiomeGenerator.VariableAndCondition conditionalVariable in storyObjectiveData.ConditionalVariables)
            {
              if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
                flag3 = false;
            }
          }
          if (flag3)
          {
            bool flag4 = false;
            if (Quests.GetQuest(questGiverFollowerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, targetQuest: Quests.QuestsAll[storyObjectiveData.QuestIndex]) != null)
              flag4 = true;
            if (!flag2 & flag4)
            {
              if (storyObjectiveData.QuestGiverRequiresID == questGiverFollowerID)
              {
                storyObjectiveDataList.Clear();
                storyObjectiveDataList.Add(storyObjectiveData);
                break;
              }
              storyObjectiveDataList.Add(storyObjectiveData);
            }
          }
        }
      }
    }
    if (storyObjectiveDataList.Count <= 0)
      return (StoryData) null;
    StoryDataItem storyDataItem = new StoryDataItem();
    storyDataItem.StoryObjectiveData = storyObjectiveDataList[Random.Range(0, storyObjectiveDataList.Count)];
    storyDataItem.QuestGiverFollowerID = questGiverFollowerID;
    storyDataItem.FollowerID = questGiverFollowerID;
    storyDataItem.TargetFollowerID_1 = targetFollowerID_1;
    storyDataItem.TargetFollowerID_2 = targetFollowerID_2;
    storyDataItem.DeadFollowerID = deadFollowerID;
    StoryData newStory = new StoryData();
    newStory.EntryStoryItem = storyDataItem;
    Quests.CreateStoryDataItemForStoryObjectiveData(newStory.EntryStoryItem);
    return newStory;
  }

  public static Objectives_Story GetCurrentStoryObjective(int followerID)
  {
    List<StoryData> stories = new List<StoryData>((IEnumerable<StoryData>) DataManager.Instance.StoryObjectives);
    for (int index = stories.Count - 1; index >= 0; --index)
    {
      StoryData storyData = stories[index];
      if (FollowerInfo.GetInfoByID(storyData.EntryStoryItem.QuestGiverFollowerID) != null)
      {
        List<StoryDataItem> fromStoryDataItem = Quests.GetChildStoryDataItemsFromStoryDataItem(storyData.EntryStoryItem);
        bool flag = false;
        foreach (StoryDataItem storyDataItem in fromStoryDataItem)
        {
          if (storyDataItem.StoryObjectiveData.RequireTarget_1 && FollowerInfo.GetInfoByID(storyDataItem.TargetFollowerID_1) == null || storyDataItem.StoryObjectiveData.RequireTarget_2 && FollowerInfo.GetInfoByID(storyDataItem.TargetFollowerID_2) == null || storyDataItem.StoryObjectiveData.RequireTarget_Deadbody && FollowerInfo.GetInfoByID(storyDataItem.DeadFollowerID) != null)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          StoryDataItem currentStoryDataItem = Quests.GetCurrentStoryDataItem(storyData.EntryStoryItem.QuestGiverFollowerID, stories);
          if (currentStoryDataItem != null)
          {
            if (Quests.GetQuest(currentStoryDataItem.FollowerID, currentStoryDataItem.TargetFollowerID_1, currentStoryDataItem.TargetFollowerID_2, currentStoryDataItem.DeadFollowerID, targetQuest: currentStoryDataItem.Objective) == null)
            {
              stories.RemoveAt(index);
              continue;
            }
            continue;
          }
          stories.RemoveAt(index);
          continue;
        }
      }
      DataManager.Instance.StoryObjectives.RemoveAt(index);
      stories.RemoveAt(index);
      Quests.AddObjectiveToHistory(storyData.EntryStoryItem.StoryObjectiveData.UniqueStoryID, 120000f, true);
    }
    StoryDataItem currentStoryDataItem1 = Quests.GetCurrentStoryDataItem(followerID, stories);
    StoryDataItem storyObjectiveParent = Quests.GetStoryObjectiveParent(Quests.GetFollowerStoryData(followerID), currentStoryDataItem1);
    return currentStoryDataItem1 != null ? new Objectives_Story(currentStoryDataItem1, storyObjectiveParent) : (Objectives_Story) null;
  }

  private static StoryDataItem GetStoryObjectiveParent(
    StoryData rootStoryData,
    StoryDataItem childDataItem)
  {
    return rootStoryData == null || childDataItem == null ? (StoryDataItem) null : Quests.GetStoryObjectiveParent(rootStoryData.EntryStoryItem, childDataItem);
  }

  private static StoryDataItem GetStoryObjectiveParent(
    StoryDataItem parentDataItem,
    StoryDataItem childDataItem)
  {
    foreach (StoryDataItem childStoryDataItem in parentDataItem.ChildStoryDataItems)
    {
      if (childStoryDataItem == childDataItem)
        return parentDataItem;
      if (Quests.GetStoryObjectiveParent(childStoryDataItem, childDataItem) != null)
        return childStoryDataItem;
    }
    return (StoryDataItem) null;
  }

  public static List<StoryDataItem> GetChildStoryDataItemsFromStoryDataItem(
    StoryDataItem storyDataItem)
  {
    List<StoryDataItem> fromStoryDataItem = new List<StoryDataItem>();
    fromStoryDataItem.AddRange((IEnumerable<StoryDataItem>) storyDataItem.ChildStoryDataItems);
    foreach (StoryDataItem childStoryDataItem in storyDataItem.ChildStoryDataItems)
      fromStoryDataItem.AddRange((IEnumerable<StoryDataItem>) Quests.GetChildStoryDataItemsFromStoryDataItem(childStoryDataItem));
    return fromStoryDataItem;
  }

  private static void CreateStoryDataItemForStoryObjectiveData(StoryDataItem parentItem)
  {
    foreach (StoryObjectiveData chilldStoryItem in parentItem.StoryObjectiveData.ChilldStoryItems)
    {
      StoryDataItem parentItem1 = new StoryDataItem();
      parentItem1.StoryObjectiveData = chilldStoryItem;
      parentItem1.QuestGiverFollowerID = parentItem.QuestGiverFollowerID;
      parentItem1.FollowerID = parentItem.QuestGiverFollowerID;
      parentItem1.TargetFollowerID_1 = parentItem.TargetFollowerID_1;
      parentItem1.TargetFollowerID_2 = parentItem.TargetFollowerID_2;
      parentItem1.DeadFollowerID = parentItem.DeadFollowerID;
      parentItem.ChildStoryDataItems.Add(parentItem1);
      Quests.CreateStoryDataItemForStoryObjectiveData(parentItem1);
    }
  }

  public static StoryDataItem GetCurrentStoryDataItem(int followerID, List<StoryData> stories)
  {
    foreach (StoryData storey in stories)
    {
      if (storey.EntryStoryItem.QuestGiverFollowerID == followerID)
      {
        StoryDataItem currentStoryDataItem = Quests.GetCurrentStoryDataItem(storey.EntryStoryItem);
        if (currentStoryDataItem != null)
          return currentStoryDataItem.QuestDeclined || FollowerInfo.GetInfoByID(currentStoryDataItem.FollowerID) == null || FollowerInfo.GetInfoByID(currentStoryDataItem.TargetFollowerID_1) == null || FollowerInfo.GetInfoByID(currentStoryDataItem.TargetFollowerID_2) == null ? (StoryDataItem) null : currentStoryDataItem;
      }
    }
    return (StoryDataItem) null;
  }

  private static StoryDataItem GetCurrentStoryDataItem(StoryDataItem storyDataItem)
  {
    int targetFollowerID_2 = -1;
    int deadFollowerID = -1;
    int followerID = storyDataItem.QuestGiverFollowerID;
    int targetFollowerID_1 = storyDataItem.QuestGiverFollowerID;
    if (storyDataItem.TargetFollowerID_1 != -1 && storyDataItem.StoryObjectiveData.RequireTarget_1)
      targetFollowerID_1 = storyDataItem.TargetFollowerID_1;
    if (storyDataItem.StoryObjectiveData.RequireTarget_1 && storyDataItem.StoryObjectiveData.RequireTarget_2 && storyDataItem.StoryObjectiveData.TargetQuestGiver)
    {
      targetFollowerID_1 = storyDataItem.QuestGiverFollowerID;
      targetFollowerID_2 = storyDataItem.TargetFollowerID_1;
    }
    if (storyDataItem.TargetFollowerID_1 != -1 && storyDataItem.StoryObjectiveData.RequireTarget_1 && !storyDataItem.StoryObjectiveData.TargetQuestGiver)
      followerID = storyDataItem.TargetFollowerID_1;
    if (storyDataItem.TargetFollowerID_2 != -1 && storyDataItem.StoryObjectiveData.RequireTarget_2 && !storyDataItem.StoryObjectiveData.TargetQuestGiver)
      targetFollowerID_2 = storyDataItem.TargetFollowerID_2;
    if (storyDataItem.DeadFollowerID != -1 && storyDataItem.StoryObjectiveData.RequireTarget_Deadbody)
      deadFollowerID = storyDataItem.DeadFollowerID;
    ObjectivesData quest1 = Quests.GetQuest(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, targetQuest: Quests.QuestsAll[storyDataItem.StoryObjectiveData.QuestIndex]);
    if (!storyDataItem.QuestGiven)
    {
      if (quest1 == null)
        return (StoryDataItem) null;
      ObjectivesData quest2 = Quests.GetQuest(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, true, Quests.QuestsAll[storyDataItem.StoryObjectiveData.QuestIndex]);
      storyDataItem.Objective = quest2;
      storyDataItem.Objective.CompleteTerm = storyDataItem.StoryObjectiveData.CompleteQuestTerm;
      return storyDataItem;
    }
    foreach (StoryDataItem childStoryDataItem in storyDataItem.ChildStoryDataItems)
    {
      StoryDataItem currentStoryDataItem = Quests.GetCurrentStoryDataItem(childStoryDataItem);
      if (currentStoryDataItem != null)
        return currentStoryDataItem;
    }
    return (StoryDataItem) null;
  }

  public static StoryData GetFollowerStoryData(int followerID)
  {
    foreach (StoryData storyObjective in DataManager.Instance.StoryObjectives)
    {
      if (storyObjective.EntryStoryItem.QuestGiverFollowerID == followerID)
        return storyObjective;
    }
    return (StoryData) null;
  }

  public static bool IsFollowerLeaderInStory(int followerID)
  {
    foreach (StoryData storyObjective in DataManager.Instance.StoryObjectives)
    {
      if (storyObjective.EntryStoryItem.QuestGiverFollowerID == followerID)
        return true;
    }
    return false;
  }

  public static bool IsFollowerInStory(int followerID)
  {
    foreach (StoryData storyObjective in DataManager.Instance.StoryObjectives)
    {
      if (Quests.IsFollowerInStoryChild(followerID, storyObjective.EntryStoryItem))
        return true;
    }
    return false;
  }

  private static bool IsFollowerInStoryChild(int followerID, StoryDataItem storyItem)
  {
    if (followerID == storyItem.QuestGiverFollowerID || followerID == storyItem.TargetFollowerID_1 || followerID == storyItem.TargetFollowerID_2 || followerID == storyItem.DeadFollowerID)
      return true;
    foreach (StoryDataItem childStoryDataItem in storyItem.ChildStoryDataItems)
    {
      if (Quests.IsFollowerInStoryChild(followerID, childStoryDataItem))
        return true;
    }
    return false;
  }

  public static ObjectivesData GetRandomDungeonChallenge()
  {
    int num = 0;
    while (num++ < 32 /*0x20*/)
    {
      ObjectivesData randomRoomChallenge = Quests.GetRandomRoomChallenge();
      if (randomRoomChallenge != null && !Quests.ObjectiveAlreadyActive(randomRoomChallenge))
      {
        DataManager.Instance.DungeonObjectives.Add(randomRoomChallenge);
        randomRoomChallenge.Init(true);
        return randomRoomChallenge;
      }
    }
    return (ObjectivesData) null;
  }

  private static ObjectivesData GetRandomRoomChallenge()
  {
    return (ObjectivesData) Quests.DungeonRoomChallenges[Random.Range(0, Quests.DungeonRoomChallenges.Count)];
  }

  public static List<ObjectivesData> GetCurrentFollowerQuests(int followerID)
  {
    List<ObjectivesData> currentFollowerQuests = new List<ObjectivesData>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Follower == followerID)
        currentFollowerQuests.Add(objective);
    }
    foreach (ObjectivesData failedObjective in DataManager.Instance.FailedObjectives)
    {
      if (failedObjective.Follower == followerID)
        currentFollowerQuests.Add(failedObjective);
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective.Follower == followerID)
        currentFollowerQuests.Add(completedObjective);
    }
    return currentFollowerQuests;
  }

  public static List<ObjectivesData> GetUnCompletedFollowerQuests(int followerID, string groupID)
  {
    List<ObjectivesData> completedFollowerQuests = new List<ObjectivesData>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Follower == followerID && (groupID == "" || objective.GroupId == groupID))
        completedFollowerQuests.Add(objective);
    }
    return completedFollowerQuests;
  }

  public static void AddObjectiveToHistory(
    int questIndex,
    float questCooldownDuration,
    bool isStory = false)
  {
    bool flag = false;
    for (int index = 0; index < DataManager.Instance.CompletedQuestsHistorys.Count; ++index)
    {
      if (DataManager.Instance.CompletedQuestsHistorys[index].QuestIndex == questIndex && DataManager.Instance.CompletedQuestsHistorys[index].IsStory == isStory)
      {
        DataManager.Instance.CompletedQuestsHistorys[index].QuestTimestamp = TimeManager.TotalElapsedGameTime;
        DataManager.Instance.CompletedQuestsHistorys[index].QuestCooldownDuration = questCooldownDuration;
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    DataManager.Instance.CompletedQuestsHistorys.Add(new DataManager.QuestHistoryData()
    {
      QuestIndex = questIndex,
      QuestTimestamp = TimeManager.TotalElapsedGameTime,
      IsStory = isStory,
      QuestCooldownDuration = questCooldownDuration
    });
  }
}
