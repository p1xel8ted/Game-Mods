// Decompiled with JetBrains decompiler
// Type: TraderTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class TraderTracker
{
  [Key(0)]
  public FollowerLocation location = FollowerLocation.None;
  [Key(1)]
  public List<TraderTrackerItems> itemsToTrade = new List<TraderTrackerItems>();
  [Key(3)]
  public string traderName = "";
  [Key(2)]
  public List<InventoryItem.ITEM_TYPE> itemsForSale = new List<InventoryItem.ITEM_TYPE>();
  [Key(4)]
  public List<InventoryItem.ITEM_TYPE> blackList = new List<InventoryItem.ITEM_TYPE>();
  [IgnoreMember]
  public bool inBlackList;

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
