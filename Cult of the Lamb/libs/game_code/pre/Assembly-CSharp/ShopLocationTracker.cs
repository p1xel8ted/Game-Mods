// Decompiled with JetBrains decompiler
// Type: ShopLocationTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ShopLocationTracker
{
  public string shopKeeperName = "";
  public int lastDayUsed = 1;
  public FollowerLocation location = FollowerLocation.None;
  public List<BuyEntry> itemsForSale = new List<BuyEntry>();

  public ShopLocationTracker()
  {
  }

  public ShopLocationTracker(FollowerLocation Location) => this.location = Location;

  public ShopLocationTracker(
    FollowerLocation Location,
    int LastDayUsed,
    List<BuyEntry> ItemsForSale,
    string ShopKeeperName)
  {
    this.shopKeeperName = ShopKeeperName;
    this.lastDayUsed = LastDayUsed;
    this.location = Location;
    this.itemsForSale = ItemsForSale;
  }
}
