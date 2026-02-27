// Decompiled with JetBrains decompiler
// Type: TraderTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class TraderTracker
{
  public FollowerLocation location = FollowerLocation.None;
  public List<TraderTrackerItems> itemsToTrade = new List<TraderTrackerItems>();
  public string traderName = "";
  public List<InventoryItem.ITEM_TYPE> itemsForSale = new List<InventoryItem.ITEM_TYPE>();
  public List<InventoryItem.ITEM_TYPE> blackList = new List<InventoryItem.ITEM_TYPE>();
  private bool inBlackList;

  public void GetItemsForSale()
  {
    this.itemsForSale.Clear();
    foreach (TraderTrackerItems traderTrackerItems in this.itemsToTrade)
    {
      this.inBlackList = false;
      if (this.blackList.Count > 0)
      {
        foreach (InventoryItem.ITEM_TYPE black in this.blackList)
        {
          if (traderTrackerItems.itemForTrade == black)
            this.inBlackList = true;
        }
      }
      if (!this.inBlackList)
        this.itemsForSale.Add(traderTrackerItems.itemForTrade);
    }
  }
}
