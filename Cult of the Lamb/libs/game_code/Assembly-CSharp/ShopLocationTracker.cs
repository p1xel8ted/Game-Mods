// Decompiled with JetBrains decompiler
// Type: ShopLocationTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class ShopLocationTracker
{
  [Key(0)]
  public string shopKeeperName = "";
  [Key(1)]
  public int lastDayUsed = 1;
  [Key(2)]
  public FollowerLocation location = FollowerLocation.None;
  [Key(3)]
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
