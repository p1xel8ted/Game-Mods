// Decompiled with JetBrains decompiler
// Type: SingleChoiceRewardOptionMiniBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SingleChoiceRewardOptionMiniBoss : SingleChoiceRewardOption
{
  public override List<BuyEntry> GetOptions()
  {
    return new List<BuyEntry>()
    {
      new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.BERRY,
        quantity = (15 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
        SingleQuantityItem = false
      },
      new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.SEED,
        quantity = 7 + this.AdditionalPerDungeon(),
        SingleQuantityItem = false
      },
      new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.LOG,
        quantity = (15 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
        SingleQuantityItem = false
      },
      new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.STONE,
        quantity = (7 + this.AdditionalPerDungeon()) * this.BeatenBishopMultiplier(),
        SingleQuantityItem = false
      },
      new BuyEntry()
      {
        itemToBuy = InventoryItem.ITEM_TYPE.GOLD_NUGGET,
        quantity = Random.Range(20, 30) + this.AdditionalPerDungeon(),
        SingleQuantityItem = false
      },
      new BuyEntry()
      {
        itemToBuy = (double) Random.value < 0.75 ? DataManager.AllGifts[Random.Range(0, DataManager.AllGifts.Count)] : DataManager.AllNecklaces[Random.Range(0, DataManager.AllNecklaces.Count)],
        quantity = 1,
        GroupID = 1,
        SingleQuantityItem = true
      },
      new BuyEntry()
      {
        itemToBuy = (double) Random.value < 0.5 ? InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN : InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION,
        quantity = 1,
        GroupID = 1,
        SingleQuantityItem = true
      }
    };
  }

  private int AdditionalPerDungeon()
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

  private int BeatenBishopMultiplier()
  {
    return !DataManager.Instance.DungeonCompleted(PlayerFarming.Location) ? 1 : 2;
  }
}
