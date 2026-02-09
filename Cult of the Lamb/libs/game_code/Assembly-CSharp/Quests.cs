// Decompiled with JetBrains decompiler
// Type: Quests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public static class Quests
{
  public const int MaxQuestsActive = 1;
  public static List<ObjectivesData> QuestsAll = new List<ObjectivesData>()
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
    (ObjectivesData) new Objectives_PlaceStructure("Objectives/GroupTitles/Quest", StructureBrain.Categories.AESTHETIC, Objectives_PlaceStructure.DecorationType.ANY, 3, 4800f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.UseFirePit, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/Quest", StructureBrain.TYPES.OUTHOUSE, 2, 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.MurderFollower, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.MurderFollowerAtNight, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_EatMeal("Objectives/GroupTitles/Quest", StructureBrain.TYPES.MEAL_POOP, 3600f),
    (ObjectivesData) new Objectives_EatMeal("Objectives/GroupTitles/Quest", StructureBrain.TYPES.MEAL_FOLLOWER_MEAT, 4800f),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.BecomeStarving, UnityEngine.Random.Range(2, 3)),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.Ill, UnityEngine.Random.Range(2, 3)),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.OldAge, UnityEngine.Random.Range(2, 3) - 1),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.Dissenter, UnityEngine.Random.Range(2, 3) - 1),
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
    (ObjectivesData) new Objectives_EatMeal("Objectives/GroupTitles/Quest", StructureBrain.TYPES.MEAL_GRASS, 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_BecomeDisciple, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_AssignClothing("Objectives/GroupTitles/Quest", FollowerClothingType.Normal_12, 3600f),
    (ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/Quest", StructureBrain.TYPES.BED_3, expireTimestamp: 3600f),
    (ObjectivesData) new Objectives_Drink("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.DRINK_EGGNOG, 3600f),
    (ObjectivesData) new Objectives_EatMeal("Objectives/GroupTitles/Quest", StructureBrain.TYPES.MEAL_GREAT_MEAT, 3600f),
    (ObjectivesData) new Objectives_Mating("Objectives/GroupTitles/Quest", -1, -1, 3600f),
    (ObjectivesData) new Objectives_AssignClothing("Objectives/GroupTitles/Quest", FollowerClothingType.Robes_Fancy, 3600f),
    (ObjectivesData) new Objectives_AssignClothing("Objectives/GroupTitles/Quest", FollowerClothingType.Suit_Fancy, 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Nudism, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.OpenPub, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_AtoneSin, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Cannibal, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_EatMeal("Objectives/GroupTitles/Quest", StructureBrain.TYPES.MEAL_MEAT, 600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.DrumCircle, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Wedding, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.Freezing, UnityEngine.Random.Range(2, 3) - 1),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_FollowerWedding, requiredFollowers: 2, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_Mating("Objectives/GroupTitles/Quest", -1, -1, 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.DrumCircle, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.KillFollowersSpouse, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Wedding, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Divorce, requiredFollowers: 1, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Divorce, requiredFollowers: 2, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_FollowerWedding, requiredFollowers: 2, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_FinishRace("Objectives/GroupTitles/Quest", expireTimestamp: 3600f),
    (ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.MAGMA_STONE, 10, false, FollowerLocation.Dungeon1_6, 4800f),
    (ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 10, false, FollowerLocation.Dungeon1_5, 4800f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.BuildGoodSnowman, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_Snowman, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_ConvertToRot, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.ChangeTraits),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.KillInFurnace, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_BuildWinterDecorations("Objectives/GroupTitles/Quest", 3, 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.SendSpouseOnMissionary, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.TakeCultsPhoto, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_RanchHarvest, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_PerformRitual("Objectives/GroupTitles/Quest", UpgradeSystem.Type.Ritual_RanchMeat, questExpireDuration: 4800f),
    (ObjectivesData) new Objectives_Drink("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.DRINK_CHILLI, 3600f),
    (ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Quest", Objectives.CustomQuestTypes.SendFollowerToMatingTent, questExpireDuration: 3600f),
    (ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Quest", InventoryItem.ITEM_TYPE.FLOWER_RED, 10, false, FollowerLocation.Dungeon1_1, 4800f)
  };
  public static List<int> RequiresDrunkFollowerQuests = new List<int>()
  {
    59,
    60,
    61
  };
  public static List<int> IgnoreCooldownQuests = new List<int>()
  {
    47
  };
  public static List<int> StoryQuests = new List<int>()
  {
    12,
    15,
    19,
    35,
    36,
    37,
    38,
    40,
    41,
    42,
    47,
    48 /*0x30*/,
    49,
    50,
    51,
    53,
    54,
    55,
    63 /*0x3F*/,
    64 /*0x40*/,
    65,
    66,
    67
  };
  public static List<int> RemovedQuests = new List<int>()
  {
    25,
    57
  };
  public static List<int> LeshyQuests = new List<int>()
  {
    29,
    3,
    52
  };
  public static List<int> HeketQuests = new List<int>()
  {
    8,
    7,
    13,
    11,
    20,
    27
  };
  public static List<int> KallamarQuests = new List<int>()
  {
    52,
    19
  };
  public static List<int> ShamuraQuests = new List<int>()
  {
    16 /*0x10*/,
    10,
    17,
    60
  };
  public static List<int> DeathCatQuests = new List<int>()
  {
    18,
    14,
    15,
    16 /*0x10*/,
    1,
    2
  };
  public static StoryObjectiveData[] allStoryObjectiveDatas;
  public static Dictionary<int, StoryObjectiveData> storyObjectiveDataByID;
  public static Dictionary<string, StoryObjectiveData> storyObjectiveDataByName;
  public static bool IS_DEBUG = false;
  public static int DEBUG_FORCED_STORY = -1;
  public static List<Objectives_RoomChallenge> DungeonRoomChallenges = new List<Objectives_RoomChallenge>()
  {
    (Objectives_RoomChallenge) new Objectives_NoCurses("Objectives/GroupTitles/Challenge", 3),
    (Objectives_RoomChallenge) new Objectives_NoDamage("Objectives/GroupTitles/Challenge", 3),
    (Objectives_RoomChallenge) new Objectives_NoDodge("Objectives/GroupTitles/Challenge", 3)
  };

  public static StoryObjectiveData[] AllStoryObjectiveDatas
  {
    get
    {
      if (Quests.allStoryObjectiveDatas == null)
        Quests.allStoryObjectiveDatas = Resources.LoadAll<StoryObjectiveData>("Data/Story Data");
      return Quests.allStoryObjectiveDatas;
    }
  }

  public static void BuildCaches()
  {
    Quests.storyObjectiveDataByID = new Dictionary<int, StoryObjectiveData>();
    Quests.storyObjectiveDataByName = new Dictionary<string, StoryObjectiveData>((IEqualityComparer<string>) StringComparer.Ordinal);
    foreach (StoryObjectiveData storyObjectiveData in Quests.AllStoryObjectiveDatas)
    {
      if (!((UnityEngine.Object) storyObjectiveData == (UnityEngine.Object) null))
      {
        string key = string.IsNullOrEmpty(storyObjectiveData.AssetName) ? storyObjectiveData.name : storyObjectiveData.AssetName;
        Quests.storyObjectiveDataByID[storyObjectiveData.UniqueStoryID] = storyObjectiveData;
        Quests.storyObjectiveDataByName[key] = storyObjectiveData;
      }
    }
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
  public static void RuntimePrewarm()
  {
    StoryObjectiveData[] storyObjectiveDatas = Quests.AllStoryObjectiveDatas;
    Quests.BuildCaches();
  }

  public static StoryObjectiveData GetStoryObjectiveDataByName(string name)
  {
    if (Quests.storyObjectiveDataByName == null)
      Quests.BuildCaches();
    StoryObjectiveData storyObjectiveData;
    return Quests.storyObjectiveDataByName == null || !Quests.storyObjectiveDataByName.TryGetValue(name, out storyObjectiveData) ? (StoryObjectiveData) null : storyObjectiveData;
  }

  public static StoryObjectiveData GetStoryObjectiveDataByID(int id)
  {
    if (Quests.storyObjectiveDataByID == null)
      Quests.BuildCaches();
    StoryObjectiveData storyObjectiveData;
    return Quests.storyObjectiveDataByID == null || !Quests.storyObjectiveDataByID.TryGetValue(id, out storyObjectiveData) ? (StoryObjectiveData) null : storyObjectiveData;
  }

  public static ObjectivesData GetQuest(
    int followerID,
    int targetFollowerID_1 = -1,
    int targetFollowerID_2 = -1,
    int deadFollowerID = -1,
    bool assignTargetFollowers = false,
    ObjectivesData targetQuest = null,
    bool requireFollower = true)
  {
    float num = 1f;
    List<ObjectivesData> objectivesDataList1 = new List<ObjectivesData>();
    List<ObjectivesData> objectivesDataList2 = new List<ObjectivesData>();
    if ((targetFollowerID_1 != -1 || !requireFollower) && (!FollowerManager.UniqueFollowerIDs.Contains(followerID) || targetQuest != null))
    {
      objectivesDataList1.AddRange((IEnumerable<ObjectivesData>) Quests.QuestsAll);
      for (int index1 = objectivesDataList1.Count - 1; index1 >= 0; --index1)
      {
        objectivesDataList1[index1].Index = index1;
        if (Quests.RemovedQuests.Contains(index1))
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
          if (flag1 && targetQuest == null && !Quests.IgnoreCooldownQuests.Contains(index1))
            objectivesDataList1[index1] = (ObjectivesData) null;
          else if (targetQuest == null && Quests.StoryQuests.Contains(index1))
          {
            objectivesDataList1[index1] = (ObjectivesData) null;
          }
          else
          {
            if (Quests.RequiresDrunkFollowerQuests.Contains(index1))
            {
              FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
              if ((infoById != null ? ((double) infoById.Drunk <= 0.0 ? 1 : 0) : 0) != 0)
              {
                objectivesDataList1[index1] = (ObjectivesData) null;
                continue;
              }
            }
            if (!Quests.StoryQuests.Contains(index1) && (followerID == 99990 && !Quests.LeshyQuests.Contains(index1) || followerID == 99991 && !Quests.HeketQuests.Contains(index1) || followerID == 99992 && !Quests.KallamarQuests.Contains(index1) || followerID == 99993 && !Quests.ShamuraQuests.Contains(index1) || followerID == 666 && !Quests.DeathCatQuests.Contains(index1)))
            {
              objectivesDataList1[index1] = (ObjectivesData) null;
            }
            else
            {
              if ((double) CultFaithManager.CurrentFaith < 25.0 && objectivesDataList1[index1] != null && objectivesDataList1[index1] is Objectives_PlaceStructure && ((Objectives_PlaceStructure) objectivesDataList1[index1]).category == StructureBrain.Categories.AESTHETIC)
                objectivesDataList2.Add(objectivesDataList1[index1]);
              if (objectivesDataList1[index1] is Objectives_AssignClothing)
              {
                Objectives_AssignClothing objectivesAssignClothing = (Objectives_AssignClothing) objectivesDataList1[index1];
                if (!DataManager.Instance.UnlockedClothing.Contains(objectivesAssignClothing.ClothingType) || StructureManager.GetAllStructuresOfType<Structures_Tailor>().Count <= 0)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                objectivesAssignClothing.TargetFollower = !assignTargetFollowers ? followerID : targetFollowerID_1;
                FollowerInfo infoById = FollowerInfo.GetInfoByID(objectivesAssignClothing.TargetFollower);
                FollowerClothingType? clothing = infoById?.Clothing;
                FollowerClothingType clothingType = objectivesAssignClothing.ClothingType;
                if (clothing.GetValueOrDefault() == clothingType & clothing.HasValue)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if ((objectivesAssignClothing.ClothingType == FollowerClothingType.Robes_Fancy || objectivesAssignClothing.ClothingType == FollowerClothingType.Suit_Fancy) && infoById != null && !infoById.MarriedToLeader)
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
              else if (objectivesDataList1[index1] is Objectives_Mating)
              {
                Objectives_Mating objectivesMating = (Objectives_Mating) objectivesDataList1[index1];
                if (StructureManager.GetAllStructuresOfType<Structures_MatingTent>().Count <= 0)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                objectivesMating.TargetFollower_1 = followerID;
                objectivesMating.TargetFollower_2 = targetFollowerID_1;
                if (objectivesMating.TargetFollower_1 == objectivesMating.TargetFollower_2)
                  objectivesMating.TargetFollower_2 = targetFollowerID_2;
                if (objectivesMating.TargetFollower_1 == -1 || objectivesMating.TargetFollower_2 == -1 || FollowerManager.AreSiblings(objectivesMating.TargetFollower_1, objectivesMating.TargetFollower_2) || FollowerManager.IsChildOf(objectivesMating.TargetFollower_1, objectivesMating.TargetFollower_2) || FollowerManager.IsChildOf(objectivesMating.TargetFollower_2, objectivesMating.TargetFollower_1) || FollowerManager.IsChild(objectivesMating.TargetFollower_1) || FollowerManager.IsChild(objectivesMating.TargetFollower_2) || (double) Interaction_MatingTent.GetChanceToMate(objectivesMating.TargetFollower_1, objectivesMating.TargetFollower_2) <= 0.0)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (targetQuest == null)
                  objectivesMating.CompleteTerm = "GiveQuest/Mating/Complete";
              }
              else if (objectivesDataList1[index1] is Objectives_Drink)
              {
                Objectives_Drink objectivesDrink = (Objectives_Drink) objectivesDataList1[index1];
                if (!CookingData.HasRecipeDiscovered(objectivesDrink.DrinkType) || StructureManager.GetAllStructuresOfType<Structures_Pub>().Count <= 0)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (CookingData.GettAllWinterDrinks().Contains<InventoryItem.ITEM_TYPE>(objectivesDrink.DrinkType))
                {
                  if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else
                    objectivesDrink.IsWinterObjective = true;
                }
                objectivesDrink.TargetFollower = !assignTargetFollowers ? followerID : targetFollowerID_1;
              }
              else if (objectivesDataList1[index1] is Objectives_PerformRitual)
              {
                Objectives_PerformRitual objectivesPerformRitual = (Objectives_PerformRitual) objectivesDataList1[index1];
                if (assignTargetFollowers)
                {
                  objectivesPerformRitual.TargetFollowerID_1 = targetFollowerID_1;
                  objectivesPerformRitual.TargetFollowerID_2 = targetFollowerID_2;
                }
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Sacrifice || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Wedding || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Fightpit || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_AssignFaithEnforcer || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_AssignTaxCollector || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_ConsumeFollower || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_FollowerWedding || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce)
                {
                  if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Fightpit & assignTargetFollowers)
                  {
                    objectivesPerformRitual.TargetFollowerID_2 = followerID;
                    if (objectivesPerformRitual.TargetFollowerID_1 == objectivesPerformRitual.TargetFollowerID_2)
                      objectivesPerformRitual.TargetFollowerID_2 = targetFollowerID_2;
                    if (objectivesPerformRitual.TargetFollowerID_2 == -1)
                      objectivesDataList1[index1] = (ObjectivesData) null;
                  }
                  else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_FollowerWedding)
                  {
                    if (!assignTargetFollowers)
                    {
                      objectivesPerformRitual.TargetFollowerID_1 = followerID;
                      objectivesPerformRitual.TargetFollowerID_2 = targetFollowerID_1;
                      if (objectivesPerformRitual.TargetFollowerID_1 == objectivesPerformRitual.TargetFollowerID_2)
                        objectivesPerformRitual.TargetFollowerID_2 = targetFollowerID_2;
                    }
                    if (objectivesPerformRitual.TargetFollowerID_1 == -1 || objectivesPerformRitual.TargetFollowerID_2 == -1 || FollowerManager.IsChild(objectivesPerformRitual.TargetFollowerID_1) || FollowerManager.IsChild(objectivesPerformRitual.TargetFollowerID_2) || FollowerManager.AreSiblings(objectivesPerformRitual.TargetFollowerID_1, objectivesPerformRitual.TargetFollowerID_2) || FollowerManager.IsChildOf(objectivesPerformRitual.TargetFollowerID_1, objectivesPerformRitual.TargetFollowerID_2) || FollowerManager.IsChildOf(objectivesPerformRitual.TargetFollowerID_2, objectivesPerformRitual.TargetFollowerID_1))
                    {
                      objectivesDataList1[index1] = (ObjectivesData) null;
                    }
                    else
                    {
                      FollowerInfo infoById1 = FollowerInfo.GetInfoByID(objectivesPerformRitual.TargetFollowerID_1);
                      FollowerInfo infoById2 = FollowerInfo.GetInfoByID(objectivesPerformRitual.TargetFollowerID_2);
                      if (infoById1 == null || infoById1.SpouseFollowerID != -1)
                        objectivesDataList1[index1] = (ObjectivesData) null;
                      if (infoById2 == null || infoById2.SpouseFollowerID != -1)
                        objectivesDataList1[index1] = (ObjectivesData) null;
                    }
                  }
                  else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce)
                  {
                    FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
                    if (infoById == null)
                      objectivesDataList1[index1] = (ObjectivesData) null;
                    else if (!infoById.Traits.Contains(FollowerTrait.TraitType.MarriedUnhappily) && !infoById.Traits.Contains(FollowerTrait.TraitType.MarriedJealous) && !infoById.Traits.Contains(FollowerTrait.TraitType.MarriedMurderouslyJealous))
                      objectivesDataList1[index1] = (ObjectivesData) null;
                    else if (infoById.MarriedToLeader)
                    {
                      if (objectivesPerformRitual.RequiredFollowers == 2)
                        objectivesDataList1[index1] = (ObjectivesData) null;
                      else
                        objectivesPerformRitual.TargetFollowerID_1 = followerID;
                    }
                    else if (objectivesPerformRitual.RequiredFollowers != 2)
                    {
                      objectivesDataList1[index1] = (ObjectivesData) null;
                    }
                    else
                    {
                      objectivesPerformRitual.TargetFollowerID_1 = followerID;
                      objectivesPerformRitual.TargetFollowerID_2 = infoById.SpouseFollowerID;
                    }
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
                  if (deadFollowerID != -1 && FollowerManager.GetDeadFollowerInfoByID(deadFollowerID) != null)
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
                  FollowerInfo followerInfoById = FollowerManager.GetDeadFollowerInfoByID(deadFollowerID);
                  if (deadFollowerID != -1 && followerInfoById != null && !followerInfoById.HadFuneral)
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
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce)
                {
                  FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
                  if (!UpgradeSystem.GetUnlocked(objectivesPerformRitual.Ritual))
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else if (infoById == null)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else if (infoById.MarriedToLeader && (double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.Type.Ritual_Wedding) > 0.0)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else if (infoById.SpouseFollowerID != -1 && (double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.Type.Ritual_FollowerWedding) > 0.0)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
                else if (!UpgradeSystem.GetUnlocked(objectivesPerformRitual.Ritual) || (double) UpgradeSystem.GetCoolDownNormalised(objectivesPerformRitual.Ritual) > 0.0)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_BecomeDisciple)
                {
                  FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
                  if (infoById != null && (infoById.IsDisciple || infoById.XPLevel < 10))
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Sacrifice && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.AgainstSacrifice))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Wedding && FollowerInfo.GetInfoByID(followerID) != null && FollowerInfo.GetInfoByID(followerID).MarriedToLeader)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Funeral && FollowerInfo.GetInfoByID(followerID, true) != null && FollowerInfo.GetInfoByID(followerID, true).HadFuneral)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Fightpit && (targetFollowerID_2 == -1 || targetFollowerID_2 == targetFollowerID_1 || FollowerInfo.GetInfoByID(targetFollowerID_1) == null || FollowerInfo.GetInfoByID(targetFollowerID_2) == null))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if ((objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_AssignFaithEnforcer || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_AssignTaxCollector) && targetQuest == null)
                {
                  FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
                  if (infoById != null && (infoById.TaxEnforcer || infoById.FaithEnforcer))
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
                if ((objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Sacrifice || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_ConsumeFollower) && DataManager.Instance.Followers.Count < 5 && targetQuest == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if ((objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_RanchHarvest || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_RanchMeat) && StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH).Count + StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH_2).Count == 0)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Snowman && (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SNOWMAN).Count == 0))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_RanchHarvest || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_RanchMeat || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_ConvertToRot || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Snowman || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce || objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_FollowerWedding)
                  objectivesPerformRitual.IsWinterObjective = true;
                if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Cannibal && targetQuest == null)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Ritual_Cannibal/Complete";
                else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Wedding && targetQuest == null && (double) FollowerInfo.GetInfoByID(followerID).Drunk > 0.0)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Drunk/Ritual_Wedding/Complete";
                else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_FollowerWedding && targetQuest == null)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Ritual_FollowerWedding/Complete";
                else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce && targetQuest == null && FollowerInfo.GetInfoByID(followerID).MarriedToLeader)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Ritual_Divorce/Player/Complete";
                else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Divorce && targetQuest == null)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Ritual_Divorce/Complete";
                else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_RanchHarvest && targetQuest == null)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Ritual_RanchHarvest/Complete";
                else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_RanchMeat && targetQuest == null)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Ritual_RanchMeat/Complete";
                else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_Snowman && targetQuest == null)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Ritual_Snowman/Complete";
                else if (objectivesPerformRitual.Ritual == UpgradeSystem.Type.Ritual_ConvertToRot && targetQuest == null)
                  objectivesPerformRitual.CompleteTerm = "GiveQuest/PerformRitual/Ritual_ConvertToRot/Complete";
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
                  else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendSpouseOnMissionary)
                    objectivesCustom.TargetFollowerAllowOldAge = false;
                  else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.UseFirePit)
                    objectivesCustom.TargetFollowerID = -1;
                  else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.OpenPub)
                    objectivesCustom.TargetFollowerID = -1;
                }
                if ((objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.SendFollowerOnMissionary || StructureManager.GetAllStructuresOfType<Structures_Missionary>().Count > 0) && (objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.SendFollowerToPrison || StructureManager.GetAllStructuresOfType<Structures_Prison>().Count > 0) && (objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.MurderFollower || DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower)) && (objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.MurderFollowerAtNight || DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.LawOrder_MurderFollower)) && (objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.UseFirePit || StructureManager.GetAllStructuresOfType<Structures_DancingFirePit>().Count > 0) && (objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.DrumCircle || StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.DRUM_CIRCLE).Count > 0) && (objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.OpenPub || StructureManager.GetAllStructuresOfType<Structures_Pub>().Count > 0))
                {
                  if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.BuildGoodSnowman)
                  {
                    if (StructureManager.GetAllStructuresOfTypes(StructureBrain.TYPES.ICE_BLOCK).Count <= 0)
                      goto label_170;
                  }
                  if ((objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.SendSpouseOnMissionary || StructureManager.GetAllStructuresOfType<Structures_Missionary>().Count > 0) && (objectivesCustom.CustomQuestType != Objectives.CustomQuestTypes.SendFollowerToMatingTent || StructureManager.GetAllStructuresOfType<Structures_MatingTent>().Count > 0))
                    goto label_171;
                }
label_170:
                objectivesDataList1[index1] = (ObjectivesData) null;
label_171:
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison && objectivesDataList1[index1] != null && FollowerInfo.GetInfoByID(objectivesCustom.TargetFollowerID) != null && (FollowerInfo.GetInfoByID(objectivesCustom.TargetFollowerID).CursedState == Thought.OldAge || FollowerInfo.GetInfoByID(objectivesCustom.TargetFollowerID).IsSnowman))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.KillFollower && targetQuest == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.BuildGoodSnowman)
                {
                  if (DataManager.Instance.HasBuildGoodSnowmanQuestAccepted && !DataManager.Instance.HasLifeToTheIceRitualQuestAccepted)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || TimeManager.CurrentDay >= SeasonsManager.SeasonTimestamp)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.KillFollowersSpouse)
                {
                  FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
                  if (infoById == null || infoById.SpouseFollowerID == -1 || FollowerInfo.GetInfoByID(infoById.SpouseFollowerID) == null)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else
                    objectivesCustom.TargetFollowerID = infoById.SpouseFollowerID;
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.DrumCircle)
                {
                  if (targetQuest == null)
                    objectivesCustom.TargetFollowerID = -1;
                  else if (!Quests.IsLoverQuestAvailable(followerID, targetFollowerID_1))
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.TakeCultsPhoto)
                  objectivesCustom.TargetFollowerID = -1;
                if ((objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.MurderFollower || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.MurderFollowerAtNight) && DataManager.Instance.Followers.Count < 5 && targetQuest == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendSpouseOnMissionary)
                {
                  FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
                  if (infoById == null || infoById.SpouseFollowerID == -1 || infoById.Traits.Contains(FollowerTrait.TraitType.MarriedHappily) || FollowerInfo.GetInfoByID(infoById.SpouseFollowerID) == null)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else
                    objectivesCustom.TargetFollowerID = infoById.SpouseFollowerID;
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.KillInFurnace)
                {
                  if (!DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.FurnaceFollower) || StructureManager.GetAllStructuresOfType<Structures_Furnace>().Count <= 0 || targetFollowerID_1 == -1)
                  {
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  }
                  else
                  {
                    objectivesCustom.FailLocked = true;
                    objectivesCustom.TargetFollowerID = targetFollowerID_1;
                  }
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.ChangeTraits)
                {
                  if (StructureManager.GetAllStructuresOfType<Structures_TraitManipulator>().Count <= 0)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else
                    objectivesCustom.TargetFollowerID = followerID;
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToMatingTent && followerID != 100007)
                {
                  FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
                  if (infoById == null)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else if (!infoById.Traits.Contains(FollowerTrait.TraitType.PureBlood) && !infoById.Traits.Contains(FollowerTrait.TraitType.PureBlood_1) && !infoById.Traits.Contains(FollowerTrait.TraitType.PureBlood_2) && !infoById.Traits.Contains(FollowerTrait.TraitType.PureBlood_3))
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  if (infoById != null && infoById.Traits.Contains(FollowerTrait.TraitType.Celibate) || !DataManager.Instance.HasPureBloodMatingQuestAccepted)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  objectivesCustom.TargetFollowerID = followerID;
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.ChangeTraits || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.TakeCultsPhoto || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendSpouseOnMissionary || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToMatingTent || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.BuildGoodSnowman || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.KillInFurnace)
                  objectivesCustom.IsWinterObjective = true;
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.OpenPub)
                  objectivesCustom.CompleteTerm = "GiveQuest/OpenPub/Complete";
                else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.DrumCircle)
                  objectivesCustom.CompleteTerm = "GiveQuest/DrumCircle/Complete";
                else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.ChangeTraits)
                  objectivesCustom.CompleteTerm = "GiveQuest/ChangeTraits/Complete";
                else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.TakeCultsPhoto)
                  objectivesCustom.CompleteTerm = "GiveQuest/TakeCultsPhoto/Complete";
                else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.BuildGoodSnowman)
                  objectivesCustom.CompleteTerm = "GiveQuest/BuildGoodSnowman/Complete";
                else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendSpouseOnMissionary)
                  objectivesCustom.CompleteTerm = "GiveQuest/SendSpouseOnMissionary/Complete";
                else if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.KillInFurnace)
                  objectivesCustom.CompleteTerm = "GiveQuest/KillInFurnace/Complete";
              }
              else if (objectivesDataList1[index1] is Objectives_FindFollower)
              {
                FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID);
                bool flag3 = false;
                foreach (int uniqueFollowerId in FollowerManager.UniqueFollowerIDs)
                {
                  if (followerID == uniqueFollowerId)
                  {
                    flag3 = true;
                    break;
                  }
                }
                if (flag3)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                else if (infoById == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                else if (infoById.BornInCult)
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
                  if (objectivesFindFollower.TargetLocation != FollowerLocation.Base && (!DataManager.Instance.DungeonCompleted(objectivesFindFollower.TargetLocation) || DataManager.Instance.DeathCatBeaten && !DataManager.Instance.UnlockedDungeonDoor.Contains(objectivesFindFollower.TargetLocation)))
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
                if (objectivesBuildStructure.StructureType == StructureBrain.TYPES.OUTHOUSE && (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Outhouse) || StructureManager.GetAllStructuresOfType(FollowerLocation.Base, StructureBrain.TYPES.OUTHOUSE_2).Count > 0))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                else if (objectivesBuildStructure.StructureType == StructureBrain.TYPES.BED_3)
                {
                  if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Beds3))
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
                else if (StructureManager.GetAllStructuresOfType(FollowerLocation.Base, objectivesBuildStructure.StructureType).Count > 0)
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
              else if (objectivesDataList1[index1] is Objectives_CookMeal)
              {
                Objectives_CookMeal objectivesCookMeal = (Objectives_CookMeal) objectivesDataList1[index1];
                if ((!CookingData.CanMakeMeal(objectivesCookMeal.MealType) || CookingData.GetCookedMeal(objectivesCookMeal.MealType) <= 0) && targetQuest == null)
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
                if (objectivesEatMeal.MealType == StructureBrain.TYPES.MEAL_MEAT && targetQuest == null)
                  objectivesEatMeal.CompleteTerm = "GiveQuest/EatMeal/MEAL_MEAT/Complete";
                objectivesEatMeal.TargetFollower = followerID;
              }
              else if (objectivesDataList1[index1] is Objectives_CollectItem)
              {
                Objectives_CollectItem objectivesCollectItem = (Objectives_CollectItem) objectivesDataList1[index1];
                if (objectivesCollectItem.TargetLocation == FollowerLocation.Dungeon1_5)
                {
                  if (!DataManager.Instance.dungeonLocationsVisited.Contains(FollowerLocation.Dungeon1_5))
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  objectivesCollectItem.IsWinterObjective = true;
                }
                else if (objectivesCollectItem.TargetLocation == FollowerLocation.Dungeon1_6)
                {
                  if (!DataManager.Instance.dungeonLocationsVisited.Contains(FollowerLocation.Dungeon1_6))
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  objectivesCollectItem.IsWinterObjective = true;
                }
                else if (objectivesCollectItem.TargetLocation != FollowerLocation.Base && (!DataManager.Instance.DungeonCompleted(objectivesCollectItem.TargetLocation) || DataManager.Instance.DeathCatBeaten && !DataManager.Instance.UnlockedDungeonDoor.Contains(objectivesCollectItem.TargetLocation)))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.MUSHROOM_SMALL && !DataManager.Instance.VisitedLocations.Contains(FollowerLocation.Hub1_Sozo))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (targetQuest != null && objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.FLOWER_RED && index1 == 86 && !Quests.IsLoverQuestAvailable(followerID, targetFollowerID_1))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.MAGMA_STONE && targetQuest == null)
                  objectivesCollectItem.CompleteTerm = "GiveQuest/CollectItem/MAGMA_STONE/Complete";
                else if (objectivesCollectItem.ItemType == InventoryItem.ITEM_TYPE.LIGHTNING_SHARD && targetQuest == null)
                  objectivesCollectItem.CompleteTerm = "GiveQuest/CollectItem/LIGHTNING_SHARD/Complete";
              }
              else if (objectivesDataList1[index1] is Objectives_RecruitCursedFollower)
              {
                Objectives_RecruitCursedFollower recruitCursedFollower = (Objectives_RecruitCursedFollower) objectivesDataList1[index1];
                foreach (DataManager.QuestHistoryData completedQuestsHistory in DataManager.Instance.CompletedQuestsHistorys)
                {
                  if (completedQuestsHistory.QuestIndex != -1 && Quests.QuestsAll[completedQuestsHistory.QuestIndex].Type == Objectives.TYPES.RECRUIT_CURSED_FOLLOWER && (double) TimeManager.TotalElapsedGameTime - (double) completedQuestsHistory.QuestTimestamp < (double) completedQuestsHistory.QuestCooldownDuration)
                  {
                    objectivesDataList1[index1] = (ObjectivesData) null;
                    break;
                  }
                }
                if (objectivesDataList1[index1] != null)
                {
                  if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && DataManager.Instance.WintersOccured > 1 && recruitCursedFollower.CursedState == Thought.Freezing)
                    objectivesDataList2.Add((ObjectivesData) recruitCursedFollower);
                  else if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter && recruitCursedFollower.CursedState == Thought.Freezing)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                  else if (TimeManager.CurrentDay >= 10 || TimeManager.CurrentDay > 5 && DataManager.Instance.Followers.Count < 3)
                    objectivesDataList2.Add((ObjectivesData) recruitCursedFollower);
                  else
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
              }
              else if (objectivesDataList1[index1] is Objectives_TalkToFollower)
              {
                if (targetQuest == null)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                else if (FollowerManager.IsChild(targetFollowerID_1))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                else
                  ((Objectives_TalkToFollower) objectivesDataList1[index1]).TargetFollower = targetFollowerID_1;
              }
              else if (objectivesDataList1[index1] is Objectives_PlaceStructure objectivesPlaceStructure)
              {
                bool flag4 = false;
                foreach (StructureBrain.TYPES allStructure in StructuresData.AllStructures)
                {
                  if (StructuresData.GetCategory(allStructure) == StructureBrain.Categories.AESTHETIC && StructuresData.GetUnlocked(allStructure))
                  {
                    flag4 = true;
                    break;
                  }
                }
                if (objectivesPlaceStructure.category == StructureBrain.Categories.AESTHETIC && !flag4)
                  objectivesDataList1[index1] = (ObjectivesData) null;
              }
              else if (objectivesDataList1[index1] is Objectives_FinishRace)
              {
                if (Interaction_RacingGate.RacingGates.Count < 5 || !Interaction_RacingGate.IsAnyGateType(GateType.Start) || !Interaction_RacingGate.IsAnyGateType(GateType.End))
                  objectivesDataList1[index1] = (ObjectivesData) null;
                List<Structures_Ranch> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Ranch>();
                bool flag5 = false;
                foreach (StructureBrain structureBrain in structuresOfType)
                {
                  foreach (StructuresData.Ranchable_Animal animal in structureBrain.Data.Animals)
                  {
                    if (animal.Age >= 2)
                    {
                      flag5 = true;
                      break;
                    }
                  }
                }
                if (!flag5)
                  objectivesDataList1[index1] = (ObjectivesData) null;
                if (objectivesDataList1[index1] != null)
                  objectivesDataList1[index1].CompleteTerm = "GiveQuest/FinishRace/Complete";
              }
              else if (objectivesDataList1[index1] is Objectives_BuildWinterDecorations)
              {
                if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
                {
                  objectivesDataList1[index1] = (ObjectivesData) null;
                }
                else
                {
                  bool flag6 = false;
                  foreach (StructureBrain.TYPES Types in DataManager.DecorationsForType(DataManager.DecorationType.Major_DLC))
                  {
                    if (StructuresData.GetUnlocked(Types))
                    {
                      flag6 = true;
                      break;
                    }
                  }
                  if (!flag6)
                    objectivesDataList1[index1] = (ObjectivesData) null;
                }
                if (objectivesDataList1[index1] != null)
                  objectivesDataList1[index1].CompleteTerm = "GiveQuest/BuildWinterDecorations/Complete";
              }
            }
          }
        }
      }
    }
    if (targetQuest != null)
      return objectivesDataList2.Contains(targetQuest) || objectivesDataList1.Contains(targetQuest) ? targetQuest.Clone() : (ObjectivesData) null;
    if (objectivesDataList2.Count > 0 && targetQuest == null)
      return Quests.GetQuest(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, true, objectivesDataList2[UnityEngine.Random.Range(0, objectivesDataList2.Count)]);
    if (targetQuest == null)
    {
      Objectives_Story currentStoryObjective = Quests.GetCurrentStoryObjective(followerID);
      if (currentStoryObjective != null)
        return (ObjectivesData) currentStoryObjective;
      if ((double) UnityEngine.Random.value < (double) num && TimeManager.CurrentDay > 3)
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
      return targetQuest.Clone();
    Dictionary<int, ObjectivesData> source = new Dictionary<int, ObjectivesData>();
    for (int index = 0; index < objectivesDataList1.Count; ++index)
    {
      if (objectivesDataList1[index] != null)
        source.Add(index, objectivesDataList1[index]);
    }
    if (source.Count == 0)
      return (ObjectivesData) null;
    int index3 = UnityEngine.Random.Range(0, source.Count);
    KeyValuePair<int, ObjectivesData> keyValuePair = source.ElementAt<KeyValuePair<int, ObjectivesData>>(index3);
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

  public static bool ObjectiveAlreadyActive(ObjectivesData objective)
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
        if (!flag1 && (!storyObjectiveData.QuestGiverRequiresDisciple || FollowerInfo.GetInfoByID(questGiverFollowerID).IsDisciple))
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
            if (Quests.GetQuest(questGiverFollowerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, targetQuest: Quests.QuestsAll[storyObjectiveData.QuestIndex], requireFollower: storyObjectiveData.RequireTarget_1) != null)
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
    StoryDataItem storyQuest = new StoryDataItem();
    int index1 = UnityEngine.Random.Range(0, storyObjectiveDataList.Count);
    storyQuest.StoryObjectiveData = storyObjectiveDataList[index1];
    storyQuest.QuestGiverFollowerID = questGiverFollowerID;
    storyQuest.FollowerID = questGiverFollowerID;
    storyQuest.TargetFollowerID_1 = targetFollowerID_1;
    storyQuest.TargetFollowerID_2 = targetFollowerID_2;
    storyQuest.DeadFollowerID = deadFollowerID;
    List<int> excludeList = new List<int>()
    {
      questGiverFollowerID
    };
    if (storyQuest.StoryObjectiveData.NewTarget1Follower)
    {
      storyQuest.TargetFollowerID_1 = FollowerManager.GetPossibleQuestFollowerID(excludeList, storyQuest);
      excludeList.Add(storyQuest.TargetFollowerID_1);
    }
    if (storyQuest.StoryObjectiveData.NewTarget2Follower)
      storyQuest.TargetFollowerID_2 = FollowerManager.GetPossibleQuestFollowerID(excludeList, storyQuest);
    StoryData newStory = new StoryData();
    newStory.EntryStoryItem = storyQuest;
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
          if (storyDataItem.StoryObjectiveData.RequireTarget_1 && FollowerInfo.GetInfoByID(storyDataItem.TargetFollowerID_1) == null || storyDataItem.StoryObjectiveData.RequireTarget_2 && FollowerInfo.GetInfoByID(storyDataItem.TargetFollowerID_2) == null)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          StoryDataItem currentStoryDataItem = Quests.GetCurrentStoryDataItem(followerID, stories);
          if (currentStoryDataItem != null)
          {
            if (Quests.GetQuest(currentStoryDataItem.FollowerID, currentStoryDataItem.TargetFollowerID_1, currentStoryDataItem.TargetFollowerID_2, currentStoryDataItem.DeadFollowerID, targetQuest: Quests.QuestsAll[currentStoryDataItem.Objective.Index], requireFollower: currentStoryDataItem.StoryObjectiveData.RequireTarget_1) == null)
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

  public static StoryDataItem GetStoryObjectiveParent(
    StoryData rootStoryData,
    StoryDataItem childDataItem)
  {
    return rootStoryData == null || childDataItem == null ? (StoryDataItem) null : Quests.GetStoryObjectiveParent(rootStoryData.EntryStoryItem, childDataItem);
  }

  public static StoryDataItem GetStoryObjectiveParent(
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

  public static void CreateStoryDataItemForStoryObjectiveData(StoryDataItem parentItem)
  {
    foreach (StoryObjectiveData chilldStoryItem in parentItem.StoryObjectiveData.ChilldStoryItems)
    {
      StoryDataItem storyDataItem = new StoryDataItem();
      storyDataItem.StoryObjectiveData = chilldStoryItem;
      storyDataItem.QuestGiverFollowerID = parentItem.QuestGiverFollowerID;
      storyDataItem.FollowerID = parentItem.QuestGiverFollowerID;
      storyDataItem.TargetFollowerID_1 = parentItem.TargetFollowerID_1;
      storyDataItem.TargetFollowerID_2 = parentItem.TargetFollowerID_2;
      storyDataItem.DeadFollowerID = parentItem.DeadFollowerID;
      storyDataItem.CachedTargetFollowerID_1 = parentItem.CachedTargetFollowerID_1;
      storyDataItem.CachedTargetFollowerID_2 = parentItem.CachedTargetFollowerID_2;
      if (storyDataItem.StoryObjectiveData.NewTarget1Follower)
      {
        if (storyDataItem.StoryObjectiveData.CacheTarget_1)
          storyDataItem.CachedTargetFollowerID_1 = storyDataItem.TargetFollowerID_1;
        if (storyDataItem.StoryObjectiveData.UseCachedTarget_1)
        {
          storyDataItem.TargetFollowerID_1 = parentItem.CachedTargetFollowerID_1;
        }
        else
        {
          List<int> excludeList = new List<int>()
          {
            storyDataItem.QuestGiverFollowerID,
            storyDataItem.TargetFollowerID_1,
            storyDataItem.TargetFollowerID_2,
            parentItem.CachedTargetFollowerID_1,
            parentItem.CachedTargetFollowerID_2
          };
          storyDataItem.TargetFollowerID_1 = FollowerManager.GetPossibleQuestFollowerID(excludeList, storyDataItem);
        }
      }
      if (storyDataItem.StoryObjectiveData.NewTarget2Follower)
      {
        if (storyDataItem.StoryObjectiveData.CacheTarget_2)
          storyDataItem.CachedTargetFollowerID_2 = storyDataItem.TargetFollowerID_2;
        if (storyDataItem.StoryObjectiveData.UseCachedTarget_2)
        {
          storyDataItem.TargetFollowerID_2 = parentItem.CachedTargetFollowerID_2;
        }
        else
        {
          List<int> excludeList = new List<int>()
          {
            storyDataItem.QuestGiverFollowerID,
            storyDataItem.TargetFollowerID_1,
            storyDataItem.TargetFollowerID_2,
            parentItem.CachedTargetFollowerID_1,
            parentItem.CachedTargetFollowerID_2
          };
          storyDataItem.TargetFollowerID_2 = FollowerManager.GetPossibleQuestFollowerID(excludeList, storyDataItem);
        }
      }
      if (storyDataItem.StoryObjectiveData.SwapTarget1AsQuestGiver)
      {
        int questGiverFollowerId = storyDataItem.QuestGiverFollowerID;
        storyDataItem.QuestGiverFollowerID = storyDataItem.TargetFollowerID_1;
        storyDataItem.FollowerID = storyDataItem.TargetFollowerID_1;
        storyDataItem.TargetFollowerID_1 = questGiverFollowerId;
      }
      else if (storyDataItem.StoryObjectiveData.SwapTarget2AsQuestGiver)
      {
        int questGiverFollowerId = storyDataItem.QuestGiverFollowerID;
        storyDataItem.QuestGiverFollowerID = storyDataItem.TargetFollowerID_2;
        storyDataItem.FollowerID = storyDataItem.TargetFollowerID_2;
        storyDataItem.TargetFollowerID_2 = questGiverFollowerId;
      }
      parentItem.ChildStoryDataItems.Add(storyDataItem);
      Quests.CreateStoryDataItemForStoryObjectiveData(storyDataItem);
    }
  }

  public static StoryDataItem GetCurrentStoryDataItem(int followerID, List<StoryData> stories)
  {
    foreach (StoryData storey in stories)
    {
      StoryDataItem currentStoryDataItem = Quests.GetCurrentStoryDataItem(storey.EntryStoryItem);
      if (currentStoryDataItem != null && currentStoryDataItem.QuestGiverFollowerID == followerID)
        return currentStoryDataItem.QuestDeclined || FollowerInfo.GetInfoByID(currentStoryDataItem.FollowerID) == null || currentStoryDataItem.StoryObjectiveData.RequireTarget_1 && FollowerInfo.GetInfoByID(currentStoryDataItem.TargetFollowerID_1) == null || currentStoryDataItem.StoryObjectiveData.RequireTarget_2 && FollowerInfo.GetInfoByID(currentStoryDataItem.TargetFollowerID_2) == null || currentStoryDataItem.StoryObjectiveData.RequireTarget_Deadbody && FollowerInfo.GetInfoByID(currentStoryDataItem.DeadFollowerID) != null ? (StoryDataItem) null : currentStoryDataItem;
    }
    return (StoryDataItem) null;
  }

  public static StoryDataItem GetCurrentStoryDataItem(StoryDataItem storyDataItem)
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
    ObjectivesData quest1 = Quests.GetQuest(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, targetQuest: Quests.QuestsAll[storyDataItem.StoryObjectiveData.QuestIndex], requireFollower: storyDataItem.StoryObjectiveData.RequireTarget_1);
    if (!storyDataItem.QuestGiven)
    {
      if (quest1 != null)
      {
        ObjectivesData quest2 = Quests.GetQuest(followerID, targetFollowerID_1, targetFollowerID_2, deadFollowerID, true, Quests.QuestsAll[storyDataItem.StoryObjectiveData.QuestIndex], storyDataItem.StoryObjectiveData.RequireTarget_1);
        if (quest2 != null)
        {
          storyDataItem.Objective = quest2;
          storyDataItem.Objective.CompleteTerm = storyDataItem.StoryObjectiveData.CompleteQuestTerm;
          List<string> stringList = new List<string>();
          if (storyDataItem.TargetFollowerID_1 != -1 && storyDataItem.TargetFollowerID_1 != storyDataItem.QuestGiverFollowerID && (storyDataItem.StoryObjectiveData.RequireTarget_1 || storyDataItem.StoryObjectiveData.TargetQuestGiver))
          {
            FollowerInfo infoById = FollowerInfo.GetInfoByID(!storyDataItem.StoryObjectiveData.TargetQuestGiver || storyDataItem.StoryObjectiveData.RequireTarget_1 ? storyDataItem.TargetFollowerID_1 : storyDataItem.QuestGiverFollowerID, true);
            if (infoById != null)
              stringList.Add(infoById.Name);
          }
          if (storyDataItem.TargetFollowerID_2 != -1 && storyDataItem.StoryObjectiveData.RequireTarget_2)
          {
            FollowerInfo infoById = FollowerInfo.GetInfoByID(storyDataItem.TargetFollowerID_2, true);
            if (infoById != null)
              stringList.Add(infoById.Name);
          }
          if (storyDataItem.DeadFollowerID != -1 && storyDataItem.StoryObjectiveData.RequireTarget_Deadbody && FollowerInfo.GetInfoByID(storyDataItem.DeadFollowerID, true) != null)
          {
            FollowerInfo infoById = FollowerInfo.GetInfoByID(storyDataItem.DeadFollowerID, true);
            if (infoById != null)
              stringList.Add(infoById.Name);
          }
          if (storyDataItem.CachedTargetFollowerID_1 != -1)
          {
            FollowerInfo infoById = FollowerInfo.GetInfoByID(storyDataItem.CachedTargetFollowerID_1, true);
            if (infoById != null)
              stringList.Add(infoById.Name);
          }
          if (storyDataItem.CachedTargetFollowerID_2 != -1)
          {
            FollowerInfo infoById = FollowerInfo.GetInfoByID(storyDataItem.CachedTargetFollowerID_2, true);
            if (infoById != null)
              stringList.Add(infoById.Name);
          }
          string[] array = stringList.ToArray();
          storyDataItem.Objective.CompleteTermArguments = array;
          return storyDataItem;
        }
      }
      return (StoryDataItem) null;
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

  public static bool IsFollowerValidForStoryQuest(
    StoryDataItem storyDataItem,
    int followerID,
    bool asTarget1)
  {
    return asTarget1 ? Quests.GetQuest(storyDataItem.QuestGiverFollowerID, followerID, storyDataItem.TargetFollowerID_2, storyDataItem.DeadFollowerID, targetQuest: Quests.QuestsAll[storyDataItem.StoryObjectiveData.QuestIndex], requireFollower: storyDataItem.StoryObjectiveData.RequireTarget_1) != null : Quests.GetQuest(storyDataItem.QuestGiverFollowerID, storyDataItem.TargetFollowerID_1, followerID, storyDataItem.DeadFollowerID, targetQuest: Quests.QuestsAll[storyDataItem.StoryObjectiveData.QuestIndex], requireFollower: storyDataItem.StoryObjectiveData.RequireTarget_1) != null;
  }

  public static bool IsFollowerInStoryChild(int followerID, StoryDataItem storyItem)
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

  public static bool IsLoverQuestAvailable(int followerID, int targetFollowerID)
  {
    return !FollowerManager.IsChildOf(followerID, targetFollowerID) && !FollowerManager.IsChildOf(targetFollowerID, followerID) && !FollowerManager.IsChild(followerID) && !FollowerManager.IsChild(targetFollowerID) && !FollowerManager.AreSiblings(followerID, targetFollowerID);
  }

  public static ObjectivesData GetRandomDungeonChallenge()
  {
    int num = 0;
    while (num++ < 32 /*0x20*/)
    {
      ObjectivesData randomRoomChallenge = Quests.GetRandomRoomChallenge();
      if (Quests.IsDungeonChallengeAvailable(randomRoomChallenge))
      {
        DataManager.Instance.DungeonObjectives.Add(randomRoomChallenge);
        randomRoomChallenge.Init(true);
        return randomRoomChallenge;
      }
    }
    return (ObjectivesData) null;
  }

  public static ObjectivesData GetRandomRoomChallenge()
  {
    return (ObjectivesData) Quests.DungeonRoomChallenges[UnityEngine.Random.Range(0, Quests.DungeonRoomChallenges.Count)];
  }

  public static bool IsDungeonChallengeAvailable(ObjectivesData objective)
  {
    return objective != null && !Quests.ObjectiveAlreadyActive(objective) && (objective.Type != Objectives.TYPES.NO_CURSES || !PlayerFleeceManager.FleeceSwapsWeaponForCurse()) && (objective.Type != Objectives.TYPES.NO_DODGE || !PlayerFleeceManager.FleecePreventsRoll());
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

  public static ObjectivesData GetDebugQuest(
    int followerID,
    int targetFollowerID_1 = -1,
    int targetFollowerID_2 = -1,
    int deadFollowerID = -1)
  {
    if (Quests.DEBUG_FORCED_STORY != -1)
    {
      StoryObjectiveData storyObjectiveData1 = (StoryObjectiveData) null;
      foreach (StoryObjectiveData storyObjectiveData2 in Quests.AllStoryObjectiveDatas)
      {
        if (storyObjectiveData2.IsEntryStory && storyObjectiveData2.UniqueStoryID == Quests.DEBUG_FORCED_STORY)
          storyObjectiveData1 = storyObjectiveData2;
      }
      foreach (StoryData storyObjective in DataManager.Instance.StoryObjectives)
      {
        if (storyObjective.EntryStoryItem.StoryObjectiveData.UniqueStoryID == Quests.DEBUG_FORCED_STORY)
          return (ObjectivesData) null;
      }
      StoryDataItem storyQuest = new StoryDataItem();
      storyQuest.StoryObjectiveData = storyObjectiveData1;
      storyQuest.QuestGiverFollowerID = followerID;
      storyQuest.FollowerID = followerID;
      storyQuest.TargetFollowerID_1 = targetFollowerID_1;
      storyQuest.TargetFollowerID_2 = targetFollowerID_2;
      storyQuest.DeadFollowerID = deadFollowerID;
      List<int> excludeList = new List<int>() { followerID };
      if (storyQuest.StoryObjectiveData.NewTarget1Follower)
      {
        storyQuest.TargetFollowerID_1 = FollowerManager.GetPossibleQuestFollowerID(excludeList, storyQuest);
        excludeList.Add(storyQuest.TargetFollowerID_1);
      }
      if (storyQuest.StoryObjectiveData.NewTarget2Follower)
        storyQuest.TargetFollowerID_2 = FollowerManager.GetPossibleQuestFollowerID(excludeList, storyQuest);
      StoryData storyData = new StoryData();
      storyData.EntryStoryItem = storyQuest;
      Quests.CreateStoryDataItemForStoryObjectiveData(storyData.EntryStoryItem);
      DataManager.Instance.StoryObjectives.Add(storyData);
      return (ObjectivesData) Quests.GetCurrentStoryObjective(followerID);
    }
    Objectives_RecruitCursedFollower debugQuest = new Objectives_RecruitCursedFollower("Objectives/GroupTitles/Quest", Thought.Freezing, UnityEngine.Random.Range(2, 3) - 1);
    debugQuest.Follower = followerID;
    return (ObjectivesData) debugQuest;
  }
}
