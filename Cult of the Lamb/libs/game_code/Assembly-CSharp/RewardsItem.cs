// Decompiled with JetBrains decompiler
// Type: RewardsItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class RewardsItem
{
  public static RewardsItem instance;
  public RewardsItem.ChestRewards chestReward;
  public float ChanceToSpawn;
  [Range(0.0f, 100f)]
  public float probabilityChance;
  [HideInInspector]
  public float SpawnNumber;
  public List<RewardsItem.ChestRewards> GoodReward = new List<RewardsItem.ChestRewards>()
  {
    RewardsItem.ChestRewards.FOLLOWER_GIFT,
    RewardsItem.ChestRewards.FOLLOWER_NECKLASE,
    RewardsItem.ChestRewards.TAROT,
    RewardsItem.ChestRewards.FOLLOWER_SKIN,
    RewardsItem.ChestRewards.FOLLOWER_SKIN,
    RewardsItem.ChestRewards.BASE_DECORATION,
    RewardsItem.ChestRewards.BASE_DECORATION,
    RewardsItem.ChestRewards.WOOL
  };

  public static RewardsItem Instance
  {
    get
    {
      if (RewardsItem.instance == null)
      {
        RewardsItem rewardsItem = new RewardsItem();
      }
      return RewardsItem.instance;
    }
    set => RewardsItem.instance = value;
  }

  public RewardsItem() => RewardsItem.instance = this;

  public void OnEnable() => RewardsItem.Instance = this;

  public void OnDisable()
  {
    if (RewardsItem.Instance != this)
      return;
    RewardsItem.Instance = (RewardsItem) null;
  }

  public InventoryItem.ITEM_TYPE ReturnItemType(RewardsItem.ChestRewards chestRewardType)
  {
    switch (chestRewardType)
    {
      case RewardsItem.ChestRewards.SEEDS:
        if (DataManager.Instance.PleasureEnabled && (double) UnityEngine.Random.value < 0.20000000298023224)
          return (double) UnityEngine.Random.value < 0.5 ? InventoryItem.ITEM_TYPE.SEED_HOPS : InventoryItem.ITEM_TYPE.SEED_GRAPES;
        if ((DataManager.Instance.TailorEnabled || DataManager.Instance.PleasureEnabled) && (double) UnityEngine.Random.value < 0.20000000298023224)
          return InventoryItem.ITEM_TYPE.SEED_COTTON;
        if (SeasonsManager.Active && (double) UnityEngine.Random.value < 0.20000000298023224)
          return InventoryItem.ITEM_TYPE.SEED_CHILLI;
        switch (PlayerFarming.Location)
        {
          case FollowerLocation.Dungeon1_1:
            return InventoryItem.ITEM_TYPE.SEED;
          case FollowerLocation.Dungeon1_2:
            return InventoryItem.ITEM_TYPE.SEED_PUMPKIN;
          case FollowerLocation.Dungeon1_3:
            return InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER;
          case FollowerLocation.Dungeon1_4:
            return InventoryItem.ITEM_TYPE.SEED_BEETROOT;
          default:
            return InventoryItem.ITEM_TYPE.NONE;
        }
      case RewardsItem.ChestRewards.GOLD:
        return InventoryItem.ITEM_TYPE.BLACK_GOLD;
      case RewardsItem.ChestRewards.BIOME_RESOURCE:
        return this.GetRandomBiomeResource();
      case RewardsItem.ChestRewards.FOLLOWER_NECKLASE:
        return DataManager.AllNecklaces[UnityEngine.Random.Range(0, 5)];
      case RewardsItem.ChestRewards.FOLLOWER_GIFT:
        return PlayerFarming.Location == FollowerLocation.IntroDungeon ? InventoryItem.ITEM_TYPE.BLACK_GOLD : DataManager.AllGifts[UnityEngine.Random.Range(0, DataManager.AllGifts.Count)];
      case RewardsItem.ChestRewards.FOLLOWER_SKIN:
        return DataManager.CheckIfThereAreSkinsAvailable() ? InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN : InventoryItem.ITEM_TYPE.NONE;
      case RewardsItem.ChestRewards.BASE_DECORATION:
        return DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count > 0 ? InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION : InventoryItem.ITEM_TYPE.NONE;
      case RewardsItem.ChestRewards.HEART:
        return (double) UnityEngine.Random.value < 0.5 ? ((double) UnityEngine.Random.value >= 0.5 ? InventoryItem.ITEM_TYPE.HALF_HEART : InventoryItem.ITEM_TYPE.RED_HEART) : ((double) UnityEngine.Random.value >= 0.5 ? InventoryItem.ITEM_TYPE.HALF_BLUE_HEART : InventoryItem.ITEM_TYPE.BLUE_HEART);
      case RewardsItem.ChestRewards.TAROT:
        return InventoryItem.ITEM_TYPE.TRINKET_CARD;
      case RewardsItem.ChestRewards.GOLD_NUGGETS:
        return InventoryItem.ITEM_TYPE.GOLD_NUGGET;
      case RewardsItem.ChestRewards.GOLD_BAR:
        return InventoryItem.ITEM_TYPE.GOLD_REFINED;
      case RewardsItem.ChestRewards.DOCTRINE_STONE:
        return InventoryItem.ITEM_TYPE.DOCTRINE_STONE;
      case RewardsItem.ChestRewards.OUTFIT:
        return InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT;
      case RewardsItem.ChestRewards.LORE_STONE:
        return InventoryItem.ITEM_TYPE.LORE_STONE;
      case RewardsItem.ChestRewards.FLOCKADE_PIECE:
        switch (PlayerFarming.Location)
        {
          case FollowerLocation.Dungeon1_1:
            return InventoryItem.ITEM_TYPE.NONE;
          case FollowerLocation.Dungeon1_2:
            return InventoryItem.ITEM_TYPE.NONE;
          case FollowerLocation.Dungeon1_3:
            return InventoryItem.ITEM_TYPE.NONE;
          case FollowerLocation.Dungeon1_4:
            return InventoryItem.ITEM_TYPE.NONE;
          case FollowerLocation.Dungeon1_5:
            return InventoryItem.ITEM_TYPE.FLOCKADE_PIECE;
          case FollowerLocation.Dungeon1_6:
            return InventoryItem.ITEM_TYPE.FLOCKADE_PIECE;
          default:
            return InventoryItem.ITEM_TYPE.NONE;
        }
      case RewardsItem.ChestRewards.WOOL:
        return InventoryItem.ITEM_TYPE.WOOL;
      default:
        Debug.Log((object) "Uh Oh no reward for you :(");
        return InventoryItem.ITEM_TYPE.NONE;
    }
  }

  public InventoryItem.ITEM_TYPE GetRandomBiomeResource()
  {
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    if (PlayerFarming.Location >= FollowerLocation.Dungeon1_1)
    {
      itemTypeList.Add(InventoryItem.ITEM_TYPE.LOG);
      itemTypeList.Add(InventoryItem.ITEM_TYPE.BERRY);
    }
    if (PlayerFarming.Location >= FollowerLocation.Dungeon1_2)
    {
      itemTypeList.Add(InventoryItem.ITEM_TYPE.STONE);
      itemTypeList.Add(InventoryItem.ITEM_TYPE.PUMPKIN);
    }
    if (PlayerFarming.Location >= FollowerLocation.Dungeon1_3)
      itemTypeList.Add(InventoryItem.ITEM_TYPE.CAULIFLOWER);
    if (PlayerFarming.Location >= FollowerLocation.Dungeon1_4)
      itemTypeList.Add(InventoryItem.ITEM_TYPE.BEETROOT);
    if (PlayerFarming.Location >= FollowerLocation.Dungeon1_5)
      itemTypeList.Add(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD);
    if (PlayerFarming.Location >= FollowerLocation.Dungeon1_6)
    {
      itemTypeList.Add(InventoryItem.ITEM_TYPE.MAGMA_STONE);
      itemTypeList.Add(InventoryItem.ITEM_TYPE.CHILLI);
    }
    return itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)];
  }

  public bool IsBiomeResource(InventoryItem.ITEM_TYPE item)
  {
    return item == InventoryItem.ITEM_TYPE.LOG || item == InventoryItem.ITEM_TYPE.STONE || item == InventoryItem.ITEM_TYPE.GOLD_REFINED || item == InventoryItem.ITEM_TYPE.LOG_REFINED || item == InventoryItem.ITEM_TYPE.STONE_REFINED || item == InventoryItem.ITEM_TYPE.WOOL || item == InventoryItem.ITEM_TYPE.MAGMA_STONE || item == InventoryItem.ITEM_TYPE.LIGHTNING_SHARD;
  }

  public InventoryItem.ITEM_TYPE GetRandomChestReward()
  {
    return this.ReturnItemType((RewardsItem.ChestRewards) UnityEngine.Random.Range(0, Enum.GetNames(typeof (RewardsItem.ChestRewards)).Length));
  }

  public RewardsItem.ChestRewards GetWeapon()
  {
    return UnityEngine.Random.Range(0, 2) == 0 ? RewardsItem.ChestRewards.PLAYER_CURSE : RewardsItem.ChestRewards.PLAYER_WEAPON;
  }

  public RewardsItem.ChestRewards GetGoodReward(
    bool includeTarot = true,
    bool IsBoss = false,
    bool includeSkin = true,
    bool includeDeco = true)
  {
    if (PlayerFleeceManager.FleecePreventTarotCards())
      includeTarot = false;
    if (DungeonSandboxManager.Active)
      return (double) UnityEngine.Random.value < 0.5 ? RewardsItem.ChestRewards.HEART : RewardsItem.ChestRewards.TAROT;
    if (DataManager.Instance.HadNecklaceOnRun > 0)
      this.GoodReward.Remove(RewardsItem.ChestRewards.FOLLOWER_NECKLASE);
    this.GoodReward.Remove(RewardsItem.ChestRewards.OUTFIT);
    if (!DataManager.Instance.OnboardedWool)
      this.GoodReward.Remove(RewardsItem.ChestRewards.WOOL);
    if (!DataManager.CheckIfThereAreSkinsAvailable() || !includeSkin)
      this.GoodReward.Remove(RewardsItem.ChestRewards.FOLLOWER_SKIN);
    if (DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count <= 0 || !includeDeco)
      this.GoodReward.Remove(RewardsItem.ChestRewards.BASE_DECORATION);
    this.GoodReward.Remove(RewardsItem.ChestRewards.LORE_STONE);
    if (this.GoodReward.Count <= 0)
      return RewardsItem.ChestRewards.GOLD;
    bool flag1 = false;
    while (!flag1)
    {
      RewardsItem.ChestRewards goodReward = this.GoodReward[UnityEngine.Random.Range(0, this.GoodReward.Count)];
      bool flag2;
      switch (goodReward)
      {
        case RewardsItem.ChestRewards.GOLD:
          if (!IsBoss)
            return RewardsItem.ChestRewards.GOLD;
          continue;
        case RewardsItem.ChestRewards.FOLLOWER_NECKLASE:
          ++DataManager.Instance.HadNecklaceOnRun;
          return RewardsItem.ChestRewards.FOLLOWER_NECKLASE;
        case RewardsItem.ChestRewards.FOLLOWER_GIFT:
          flag2 = true;
          return PlayerFarming.Location == FollowerLocation.IntroDungeon ? RewardsItem.ChestRewards.GOLD : RewardsItem.ChestRewards.FOLLOWER_GIFT;
        case RewardsItem.ChestRewards.FOLLOWER_SKIN:
          return RewardsItem.ChestRewards.FOLLOWER_SKIN;
        case RewardsItem.ChestRewards.BASE_DECORATION:
          return RewardsItem.ChestRewards.BASE_DECORATION;
        case RewardsItem.ChestRewards.TAROT:
          if (includeTarot)
            return RewardsItem.ChestRewards.TAROT;
          flag1 = false;
          continue;
        case RewardsItem.ChestRewards.GOLD_NUGGETS:
          return RewardsItem.ChestRewards.GOLD_NUGGETS;
        case RewardsItem.ChestRewards.GOLD_BAR:
          return RewardsItem.ChestRewards.GOLD_BAR;
        case RewardsItem.ChestRewards.OUTFIT:
          return RewardsItem.ChestRewards.OUTFIT;
        case RewardsItem.ChestRewards.LORE_STONE:
          Debug.Log((object) "NO available Lore");
          return RewardsItem.ChestRewards.GOLD;
        default:
          flag2 = true;
          return goodReward;
      }
    }
    return RewardsItem.ChestRewards.GOLD;
  }

  public enum ChestRewards
  {
    NONE,
    FOOD,
    SEEDS,
    GOLD,
    MUSHROOM,
    BIOME_RESOURCE,
    FOLLOWER_NECKLASE,
    FOLLOWER_GIFT,
    FOLLOWER_SKIN,
    PLAYER_WEAPON,
    PLAYER_CURSE,
    BASE_DECORATION,
    HEART,
    TAROT,
    GOLD_NUGGETS,
    GOLD_BAR,
    SPIDERS,
    DOCTRINE_STONE,
    OUTFIT,
    LORE_STONE,
    DISSENTER,
    FLOCKADE_PIECE,
    WOOL,
    MISSIONARY,
  }
}
