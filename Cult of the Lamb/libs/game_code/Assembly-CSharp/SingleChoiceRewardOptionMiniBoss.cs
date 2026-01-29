// Decompiled with JetBrains decompiler
// Type: SingleChoiceRewardOptionMiniBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SingleChoiceRewardOptionMiniBoss : SingleChoiceRewardOption
{
  public override List<BuyEntry> GetOptions()
  {
    List<BuyEntry> options = new List<BuyEntry>();
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6)
    {
      bool flag1 = (double) Random.value < 0.75;
      bool flag2 = false;
      if (!flag2 && PlayerFarming.Location == FollowerLocation.Dungeon1_6 && !DataManager.Instance.CollectedYewMutated)
      {
        options.Add(new BuyEntry()
        {
          itemToBuy = InventoryItem.ITEM_TYPE.YEW_CURSED,
          quantity = 6,
          SingleQuantityItem = false
        });
        return options;
      }
      if (!flag2 && PlayerFarming.Location == FollowerLocation.Dungeon1_5 && DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_MiniBoss && (!DataManager.Instance.CollectedLightningShards || ObjectiveManager.HasCollectItemObjective(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, "Objectives/GroupTitles/Ranching", false)))
      {
        options.Add(new BuyEntry()
        {
          itemToBuy = InventoryItem.ITEM_TYPE.LIGHTNING_SHARD,
          quantity = 25,
          SingleQuantityItem = false
        });
        return options;
      }
      if (!flag2 && PlayerFarming.Location == FollowerLocation.Dungeon1_5 && DataManager.Instance.TotalShrineGhostJuice <= 4 && (!DataManager.Instance.CollectedRotstone || ObjectiveManager.HasCollectItemObjective(InventoryItem.ITEM_TYPE.MAGMA_STONE, "Objectives/GroupTitles/WarmCult", false)))
      {
        options.Add(new BuyEntry()
        {
          itemToBuy = InventoryItem.ITEM_TYPE.MAGMA_STONE,
          quantity = 25,
          SingleQuantityItem = false
        });
        return options;
      }
      if (flag1)
      {
        if (DataManager.Instance.BlizzardOfferingsCompleted <= 4)
        {
          int offeringsCompleted = DataManager.Instance.BlizzardOfferingsCompleted;
          options.Add(new BuyEntry()
          {
            itemToBuy = InventoryItem.Necklaces_DLC[offeringsCompleted],
            quantity = 1,
            SingleQuantityItem = true
          });
        }
        else if ((double) Random.value < 0.5)
          options.Add(new BuyEntry()
          {
            itemToBuy = InventoryItem.Necklaces_DLC[Random.Range(0, InventoryItem.Necklaces_DLC.Count)],
            quantity = 1,
            SingleQuantityItem = true
          });
        if (DataManager.Instance.OnboardedWool && (double) Random.value < 0.10000000149011612)
        {
          List<InventoryItem.ITEM_TYPE> animalsWeightedList = AnimalMarketplaceManager.GetUnlockedAnimalsWeightedList();
          options.Add(new BuyEntry()
          {
            itemToBuy = animalsWeightedList[Random.Range(0, animalsWeightedList.Count)],
            quantity = 1,
            SingleQuantityItem = true
          });
        }
        if (DataManager.Instance.ShowLoyaltyBars)
        {
          InventoryItem.ITEM_TYPE[] itemTypeArray = new InventoryItem.ITEM_TYPE[2]
          {
            InventoryItem.ITEM_TYPE.GIFT_SMALL,
            InventoryItem.ITEM_TYPE.GIFT_MEDIUM
          };
          options.Add(new BuyEntry()
          {
            itemToBuy = itemTypeArray[Random.Range(0, itemTypeArray.Length)],
            quantity = 1,
            SingleQuantityItem = true
          });
        }
        if (DataManager.CheckIfThereAreSkinsAvailable())
          options.Add(new BuyEntry()
          {
            itemToBuy = InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN,
            quantity = 1,
            SingleQuantityItem = true
          });
        if (DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count > 0)
        {
          for (int index = 0; index < 3; ++index)
            options.Add(new BuyEntry()
            {
              itemToBuy = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION,
              quantity = 1,
              SingleQuantityItem = true
            });
        }
        InventoryItem.ITEM_TYPE[] itemTypeArray1 = new InventoryItem.ITEM_TYPE[4]
        {
          InventoryItem.ITEM_TYPE.GOLD_REFINED,
          InventoryItem.ITEM_TYPE.LOG_REFINED,
          InventoryItem.ITEM_TYPE.STONE_REFINED,
          InventoryItem.ITEM_TYPE.SILK_THREAD
        };
        options.Add(new BuyEntry()
        {
          itemToBuy = itemTypeArray1[Random.Range(0, itemTypeArray1.Length)],
          quantity = Random.Range(5, 9),
          SingleQuantityItem = false
        });
        return options;
      }
      float num = 0.25f;
      if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6 || PlayerFarming.Location == FollowerLocation.Boss_Wolf || PlayerFarming.Location == FollowerLocation.Boss_Yngya)
        num = 0.75f;
      if ((double) Random.value < (double) num && DataManager.CheckIfThereAreSkinsAvailable())
        options.Add(new BuyEntry()
        {
          itemToBuy = InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN,
          quantity = 1,
          SingleQuantityItem = true
        });
      if ((double) Random.value < (double) num && DataManager.Instance.GetDecorationListFromLocation(PlayerFarming.Location).Count > 0)
        options.Add(new BuyEntry()
        {
          itemToBuy = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION,
          quantity = 1,
          SingleQuantityItem = true
        });
      switch (PlayerFarming.Location)
      {
        case FollowerLocation.Dungeon1_5:
          options.Add(new BuyEntry()
          {
            itemToBuy = InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION,
            quantity = 1,
            SingleQuantityItem = true
          });
          break;
        case FollowerLocation.Dungeon1_6:
          options.Add(new BuyEntry()
          {
            itemToBuy = InventoryItem.ITEM_TYPE.YEW_CURSED,
            quantity = (5 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
            SingleQuantityItem = false
          });
          options.Add(new BuyEntry()
          {
            itemToBuy = InventoryItem.ITEM_TYPE.SEED_CHILLI,
            quantity = (2 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
            SingleQuantityItem = false
          });
          options.Add(new BuyEntry()
          {
            itemToBuy = InventoryItem.ITEM_TYPE.CHILLI,
            quantity = (1 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
            SingleQuantityItem = false
          });
          if (DataManager.Instance.RancherOnboardedHolyYew && (double) Random.value < 0.10000000149011612)
          {
            options.Add(new BuyEntry()
            {
              itemToBuy = InventoryItem.ITEM_TYPE.YEW_HOLY,
              quantity = Random.Range(1, 3),
              SingleQuantityItem = false
            });
            break;
          }
          break;
      }
      options.Add(new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.MAGMA_STONE,
        quantity = (4 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
        SingleQuantityItem = false
      });
      if (DataManager.Instance.OnboardedWool)
        options.Add(new BuyEntry()
        {
          itemToBuy = InventoryItem.ITEM_TYPE.WOOL,
          quantity = Random.Range(6, 10),
          SingleQuantityItem = false
        });
    }
    else
    {
      options.Add(new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.BERRY,
        quantity = (15 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
        SingleQuantityItem = false
      });
      options.Add(new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.SEED,
        quantity = 7 + this.AdditionalPerDungeon(),
        SingleQuantityItem = false
      });
      options.Add(new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.LOG,
        quantity = (15 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
        SingleQuantityItem = false
      });
      options.Add(new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.STONE,
        quantity = (7 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
        SingleQuantityItem = false
      });
      options.Add(new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.GOLD_NUGGET,
        quantity = Random.Range(20, 30) + this.AdditionalPerDungeon(),
        SingleQuantityItem = false
      });
      float num = Random.value;
      options.Add(new BuyEntry()
      {
        itemToBuy = (double) num < 0.75 ? DataManager.AllGifts[Random.Range(0, DataManager.AllGifts.Count)] : DataManager.AllNecklaces[Random.Range(0, 5)],
        quantity = !DataManager.Instance.DeathCatBeaten || (double) num >= 0.75 ? 1 : Random.Range(2, 4),
        GroupID = 1,
        SingleQuantityItem = true
      });
      options.Add(new BuyEntry()
      {
        itemToBuy = (double) Random.value < 0.5 ? InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN : InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION,
        quantity = 1,
        GroupID = 1,
        SingleQuantityItem = true
      });
    }
    return options;
  }

  public int AdditionalPerDungeon()
  {
    switch (DataManager.Instance.BossesCompleted.Count)
    {
      case 1:
        return 2;
      case 2:
        return 4;
      case 3:
        return 6;
      case 4:
        return 8;
      case 5:
        return 10;
      default:
        return 0;
    }
  }

  public int BeatenBishopMultiplier()
  {
    return !DataManager.Instance.DungeonCompleted(PlayerFarming.Location) ? 1 : 2;
  }
}
