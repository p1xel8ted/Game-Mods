// Decompiled with JetBrains decompiler
// Type: MissionaryManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.FollowerSelect;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class MissionaryManager
{
  public static int BaseChance_Follower = 65;
  public static int BaseChance_Wood = 75;
  public static int BaseChance_Stone = 75;
  public static int BaseChance_Gold = 70;
  public static int BaseChance_Food = 80 /*0x50*/;
  public static int BaseChance_Bones = 75;
  public static int BaseChance_RefinedMaterials = 70;
  public static int BaseChance_Seeds = 80 /*0x50*/;
  public static int BaseChance_Rotstone = 75;
  public static int BaseChance_ChargedShards = 75;
  public static int BaseChance_CursedYew = 75;
  public static int BaseChance_Wool = 75;
  public static int RandomSeedSpread = 5;
  public static IntRange DurationRange = new IntRange(1, 3);
  public const int kMeatIterations = 2;
  public static IntRange FollowerRange = new IntRange(1, 1);
  public static IntRange WoodRange = new IntRange(30, 42);
  public static IntRange StoneRange = new IntRange(18, 26);
  public static IntRange GoldRange = new IntRange(28, 52);
  public static IntRange BoneRange = new IntRange(40, 70);
  public static IntRange SeedRange = new IntRange(5, 8);
  public static IntRange MeatRange = new IntRange(16 /*0x10*/, 24);
  public static IntRange MorselRange = new IntRange(5, 9);
  public static IntRange RefinedRange = new IntRange(3, 6);
  public static IntRange RotstoneRange = new IntRange(20, 32 /*0x20*/);
  public static IntRange ChargedShardRange = new IntRange(20, 32 /*0x20*/);
  public static IntRange CursedYewRange = new IntRange(8, 14);
  public static IntRange WoolRange = new IntRange(2, 6);

  public static int GetDurationDeterministic(FollowerInfo info, InventoryItem.ITEM_TYPE type)
  {
    return MissionaryManager.DurationRange.Random((int) (info.ID + type + DataManager.Instance.CurrentDayIndex));
  }

  public static float GetBaseChanceMultiplier(
    InventoryItem.ITEM_TYPE type,
    FollowerInfo followerInfo)
  {
    float chanceMultiplier = 0.9f;
    if (followerInfo.XPLevel == 1)
      chanceMultiplier += 0.15f;
    else if (followerInfo.XPLevel <= 2)
      chanceMultiplier += 0.2f;
    else if (followerInfo.XPLevel <= 4)
      chanceMultiplier += 0.25f;
    else if (followerInfo.XPLevel > 4)
      chanceMultiplier += 0.3f;
    if (FollowerBrain.GetOrCreateBrain(followerInfo).CurrentState.Type == FollowerStateType.Exhausted)
      chanceMultiplier -= 0.5f;
    if (FollowerBrain.GetOrCreateBrain(followerInfo).CurrentState.Type == FollowerStateType.Drunk)
      chanceMultiplier -= 0.25f;
    if (FollowerBrain.GetOrCreateBrain(followerInfo).HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
      chanceMultiplier -= 0.75f;
    if (FollowerBrain.GetOrCreateBrain(followerInfo).HasTrait(FollowerTrait.TraitType.MissionaryInspired))
      chanceMultiplier += 0.05f;
    if (FollowerBrain.GetOrCreateBrain(followerInfo).HasTrait(FollowerTrait.TraitType.MissionaryExcited))
      chanceMultiplier += 0.05f;
    if (FollowerBrainStats.BrainWashed || followerInfo.SozoBrainshed)
      chanceMultiplier += 0.2f;
    return chanceMultiplier;
  }

  public static float GetChance(
    InventoryItem.ITEM_TYPE type,
    FollowerInfo followerInfo,
    StructureBrain.TYPES missionaryType)
  {
    float chanceMultiplier = MissionaryManager.GetBaseChanceMultiplier(type, followerInfo);
    if (DataManager.Instance.NextMissionarySuccessful || followerInfo.Necklace == InventoryItem.ITEM_TYPE.Necklace_Missionary)
      return 1f;
    System.Random random = new System.Random((int) (followerInfo.ID + type));
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Wood + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.STONE:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Stone + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.MEAT:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Food + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.SEED:
      case InventoryItem.ITEM_TYPE.SEED_PUMPKIN:
      case InventoryItem.ITEM_TYPE.SEED_BEETROOT:
      case InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Seeds + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.BONE:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Bones + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.BLACK_GOLD:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Gold + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.LOG_REFINED:
      case InventoryItem.ITEM_TYPE.STONE_REFINED:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_RefinedMaterials + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.FOLLOWERS:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Follower + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.LIGHTNING_SHARD:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_ChargedShards + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.WOOL:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Wool + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.MAGMA_STONE:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_Rotstone + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      case InventoryItem.ITEM_TYPE.YEW_CURSED:
        return Mathf.Clamp((float) (MissionaryManager.BaseChance_CursedYew + random.Next(-MissionaryManager.RandomSeedSpread, MissionaryManager.RandomSeedSpread)) / 100f * chanceMultiplier, 0.0f, 0.95f);
      default:
        return 0.0f;
    }
  }

  public static InventoryItem[] GetReward(
    InventoryItem.ITEM_TYPE type,
    float chance,
    int followerID)
  {
    float num = UnityEngine.Random.Range(0.0f, 1f);
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (completedObjective.Follower == followerID)
      {
        chance = float.MaxValue;
        break;
      }
    }
    if ((double) chance >= (double) num)
    {
      switch (type)
      {
        case InventoryItem.ITEM_TYPE.LOG:
          return MissionaryManager.GetWoodReward();
        case InventoryItem.ITEM_TYPE.STONE:
          return MissionaryManager.GetStoneReward();
        case InventoryItem.ITEM_TYPE.MEAT:
          return MissionaryManager.GetFoodReward();
        case InventoryItem.ITEM_TYPE.SEED:
        case InventoryItem.ITEM_TYPE.SEED_PUMPKIN:
        case InventoryItem.ITEM_TYPE.SEEDS:
        case InventoryItem.ITEM_TYPE.SEED_BEETROOT:
        case InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER:
          return new InventoryItem[1]
          {
            MissionaryManager.GetSeedsReward(type)
          };
        case InventoryItem.ITEM_TYPE.BONE:
          return MissionaryManager.GetBoneReward();
        case InventoryItem.ITEM_TYPE.BLACK_GOLD:
          return MissionaryManager.GetGoldReward();
        case InventoryItem.ITEM_TYPE.LOG_REFINED:
          return new InventoryItem[1]
          {
            MissionaryManager.GetRefinedReward(InventoryItem.ITEM_TYPE.LOG_REFINED)
          };
        case InventoryItem.ITEM_TYPE.STONE_REFINED:
          return new InventoryItem[1]
          {
            MissionaryManager.GetRefinedReward(InventoryItem.ITEM_TYPE.STONE_REFINED)
          };
        case InventoryItem.ITEM_TYPE.FOLLOWERS:
          return MissionaryManager.GetFollowerReward();
        case InventoryItem.ITEM_TYPE.LIGHTNING_SHARD:
          return new InventoryItem[1]
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, MissionaryManager.ChargedShardRange.Random())
          };
        case InventoryItem.ITEM_TYPE.WOOL:
          return new InventoryItem[1]
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.WOOL, MissionaryManager.WoolRange.Random())
          };
        case InventoryItem.ITEM_TYPE.MAGMA_STONE:
          return new InventoryItem[1]
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.MAGMA_STONE, MissionaryManager.RotstoneRange.Random())
          };
        case InventoryItem.ITEM_TYPE.YEW_CURSED:
          return new InventoryItem[1]
          {
            new InventoryItem(InventoryItem.ITEM_TYPE.YEW_CURSED, MissionaryManager.CursedYewRange.Random())
          };
      }
    }
    return new InventoryItem[0];
  }

  public static IntRange GetRewardRange(InventoryItem.ITEM_TYPE type)
  {
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.LOG:
        return MissionaryManager.WoodRange;
      case InventoryItem.ITEM_TYPE.STONE:
        return MissionaryManager.StoneRange;
      case InventoryItem.ITEM_TYPE.MEAT:
        return new IntRange(Mathf.Min(MissionaryManager.MeatRange.Min, MissionaryManager.MorselRange.Min), Mathf.Max(MissionaryManager.MeatRange.Max, MissionaryManager.MeatRange.Max));
      case InventoryItem.ITEM_TYPE.SEED:
      case InventoryItem.ITEM_TYPE.SEED_PUMPKIN:
      case InventoryItem.ITEM_TYPE.SEEDS:
      case InventoryItem.ITEM_TYPE.SEED_BEETROOT:
      case InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER:
        return MissionaryManager.SeedRange;
      case InventoryItem.ITEM_TYPE.BONE:
        return MissionaryManager.BoneRange;
      case InventoryItem.ITEM_TYPE.BLACK_GOLD:
        return MissionaryManager.GoldRange;
      case InventoryItem.ITEM_TYPE.LOG_REFINED:
      case InventoryItem.ITEM_TYPE.STONE_REFINED:
        return MissionaryManager.RefinedRange;
      case InventoryItem.ITEM_TYPE.FOLLOWERS:
        return MissionaryManager.FollowerRange;
      case InventoryItem.ITEM_TYPE.LIGHTNING_SHARD:
        return MissionaryManager.ChargedShardRange;
      case InventoryItem.ITEM_TYPE.WOOL:
        return MissionaryManager.WoolRange;
      case InventoryItem.ITEM_TYPE.MAGMA_STONE:
        return MissionaryManager.RotstoneRange;
      case InventoryItem.ITEM_TYPE.YEW_CURSED:
        return MissionaryManager.CursedYewRange;
      default:
        return new IntRange(0, 0);
    }
  }

  public static InventoryItem[] GetFollowerReward()
  {
    return new InventoryItem[1]
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.FOLLOWERS, MissionaryManager.FollowerRange.Random())
    };
  }

  public static InventoryItem[] GetWoodReward()
  {
    return new InventoryItem[1]
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.LOG, MissionaryManager.WoodRange.Random())
    };
  }

  public static InventoryItem[] GetStoneReward()
  {
    return new InventoryItem[1]
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.STONE, MissionaryManager.StoneRange.Random())
    };
  }

  public static InventoryItem[] GetGoldReward()
  {
    return new InventoryItem[1]
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, MissionaryManager.GoldRange.Random())
    };
  }

  public static InventoryItem[] GetBoneReward()
  {
    return new InventoryItem[1]
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.BONE, MissionaryManager.BoneRange.Random())
    };
  }

  public static InventoryItem[] GetFoodReward()
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    InventoryItem inventoryItem1 = new InventoryItem(InventoryItem.ITEM_TYPE.MEAT);
    InventoryItem inventoryItem2 = new InventoryItem(InventoryItem.ITEM_TYPE.MEAT_MORSEL);
    inventoryItem1.quantity += MissionaryManager.MeatRange.Random();
    inventoryItem2.quantity += MissionaryManager.MorselRange.Random();
    inventoryItemList.Add(inventoryItem1);
    inventoryItemList.Add(inventoryItem2);
    return inventoryItemList.ToArray();
  }

  public static InventoryItem GetSeedsReward(InventoryItem.ITEM_TYPE overrideType)
  {
    InventoryItem.ITEM_TYPE Type = InventoryItem.ITEM_TYPE.SEED;
    int num = new System.Random(TimeManager.CurrentDay).Next(0, 4);
    if (num == 0 && DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_1))
      Type = InventoryItem.ITEM_TYPE.SEED_PUMPKIN;
    else if (num == 1 && DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2))
      Type = InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER;
    else if (num == 2 && DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3))
      Type = InventoryItem.ITEM_TYPE.SEED_BEETROOT;
    if (overrideType != InventoryItem.ITEM_TYPE.SEEDS)
      Type = overrideType;
    return new InventoryItem(Type, MissionaryManager.SeedRange.Random());
  }

  public static InventoryItem GetRefinedReward(InventoryItem.ITEM_TYPE type)
  {
    return new InventoryItem(type, MissionaryManager.RefinedRange.Random());
  }

  public static string GetExpiryFormatted(float timeStamp)
  {
    int num = Mathf.RoundToInt((float) (Mathf.RoundToInt(timeStamp / 1200f) - Mathf.RoundToInt(TimeManager.TotalElapsedGameTime / 1200f)));
    if (num > 1)
      return string.Format(LocalizationManager.GetTranslation("UI/Generic/Days"), (object) num);
    return num == 1 ? "1 " + LocalizationManager.GetTranslation("UI/Day") : "< 1 " + LocalizationManager.GetTranslation("UI/Day");
  }

  public static List<FollowerSelectEntry> FollowersAvailableForMission()
  {
    List<FollowerSelectEntry> followerSelectEntryList = new List<FollowerSelectEntry>();
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      int id = follower.Brain.Info.ID;
      if (follower.Brain.Info != null && follower.Brain._directInfoAccess.IsSnowman)
        followerSelectEntryList.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.Unavailable));
      else if (follower.Brain.Info.CursedState != Thought.None)
        followerSelectEntryList.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerCursedStateAvailability(follower)));
      else if (follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
        followerSelectEntryList.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerCursedStateAvailability(follower)));
      else
        followerSelectEntryList.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain)));
    }
    followerSelectEntryList.Sort((Comparison<FollowerSelectEntry>) ((a, b) => b.FollowerInfo.XPLevel.CompareTo(a.FollowerInfo.XPLevel)));
    return followerSelectEntryList;
  }
}
