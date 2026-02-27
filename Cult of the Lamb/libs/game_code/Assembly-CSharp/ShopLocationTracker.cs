// Decompiled with JetBrains decompiler
// Type: ShopLocationTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
