// Decompiled with JetBrains decompiler
// Type: MissionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public static class MissionManager
{
  public const int MaxAvailableMissionsAllowed = 3;
  public const int DaysTillNewMission = 1;
  public const int MinExpiryDays = 3;
  public const int MaxExpiryDays = 5;
  private static List<InventoryItem.ITEM_TYPE> possibleRewards = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.STONE,
    InventoryItem.ITEM_TYPE.LOG,
    InventoryItem.ITEM_TYPE.BERRY,
    InventoryItem.ITEM_TYPE.BLACK_GOLD,
    InventoryItem.ITEM_TYPE.FISH
  };
  private static List<InventoryItem.ITEM_TYPE> possibleGoldRewards = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION,
    InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN
  };

  public static void AddMission(
    MissionManager.MissionType missionType,
    int difficulty,
    bool goldenMission = false)
  {
    MissionManager.Mission mission = new MissionManager.Mission()
    {
      MissionType = missionType,
      Difficulty = difficulty,
      Rewards = MissionManager.GetRewards(goldenMission ? 3 : difficulty),
      GoldenMission = goldenMission,
      ExpiryTimestamp = TimeManager.TotalElapsedGameTime + (float) (Random.Range(3, 6) * 1200)
    };
    if (goldenMission)
    {
      List<BuyEntry> list = ((IEnumerable<BuyEntry>) mission.Rewards).ToList<BuyEntry>();
      list.Add(MissionManager.GetGoldenReward(mission));
      mission.Rewards = list.ToArray();
      mission.Difficulty = 3;
    }
    DataManager.Instance.AvailableMissions.Add(mission);
  }

  private static StructuresData GetStructureData()
  {
    List<Structures_MissionShrine> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_MissionShrine>();
    return structuresOfType.Count > 0 ? structuresOfType[0].Data : (StructuresData) null;
  }

  private static BuyEntry[] GetRewards(int difficulty)
  {
    int num1 = Random.Range(1 + Mathf.FloorToInt((float) (difficulty / 2)), difficulty);
    List<BuyEntry> buyEntryList = new List<BuyEntry>();
    for (int index = 0; index < num1; ++index)
    {
      int num2 = 0;
      while (++num2 < 20)
      {
        BuyEntry randomReward = MissionManager.GetRandomReward();
        if (!buyEntryList.Contains(randomReward))
        {
          randomReward.quantity = 5 * difficulty;
          buyEntryList.Add(randomReward);
          break;
        }
      }
    }
    return buyEntryList.ToArray();
  }

  public static void RemoveMission(MissionManager.Mission mission)
  {
    DataManager.Instance.ActiveMissions.Remove(mission);
  }

  private static BuyEntry GetGoldenReward(MissionManager.Mission mission)
  {
    int num = 0;
    while (++num < 20)
    {
      InventoryItem.ITEM_TYPE possibleGoldReward = MissionManager.possibleGoldRewards[Random.Range(0, MissionManager.possibleGoldRewards.Count)];
      if (possibleGoldReward == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION && DataManager.CheckAvailableDecorations())
      {
        mission.Decoration = DataManager.GetRandomLockedDecoration();
        return new BuyEntry(possibleGoldReward, InventoryItem.ITEM_TYPE.NONE, 0, 0);
      }
      if (possibleGoldReward == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN && DataManager.CheckIfThereAreSkinsAvailable())
      {
        mission.FollowerSkin = DataManager.GetRandomLockedSkin();
        return new BuyEntry(possibleGoldReward, InventoryItem.ITEM_TYPE.NONE, 0, 0);
      }
    }
    return MissionManager.GetRewards(3)[0];
  }

  private static BuyEntry GetRandomReward()
  {
    return new BuyEntry(MissionManager.possibleRewards[Random.Range(0, MissionManager.possibleRewards.Count)], InventoryItem.ITEM_TYPE.NONE, 0);
  }

  public static string GetMissionName(MissionManager.Mission mission)
  {
    return LocalizationManager.GetTermTranslation($"Interactions/Missions/{mission.MissionType}");
  }

  public static string GetExpiryFormatted(MissionManager.Mission mission)
  {
    int num = Mathf.FloorToInt((float) (((double) mission.ExpiryTimestamp - (double) TimeManager.TotalElapsedGameTime) / 1200.0));
    return num > 0 ? string.Format(LocalizationManager.GetTranslation("UI/Generic/Days"), (object) (num + 1)) : "< 1 " + LocalizationManager.GetTranslation("UI/Generic/Days");
  }

  public enum MissionType
  {
    Bounty,
  }

  public class Mission
  {
    public MissionManager.MissionType MissionType;
    public int Difficulty;
    public int ID;
    public float ExpiryTimestamp;
    public BuyEntry[] Rewards;
    public bool GoldenMission;
    public StructureBrain.TYPES Decoration;
    public string FollowerSkin;
  }
}
