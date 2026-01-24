// Decompiled with JetBrains decompiler
// Type: TraderTrackerItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class TraderTrackerItems
{
  [Key(0)]
  public InventoryItem.ITEM_TYPE itemForTrade;
  [Key(1)]
  public int SellPrice;
  [Key(2)]
  public int BuyPrice;
  [Key(3)]
  public int BuyOffsetPercent = 20;
  [Key(4)]
  public int BuyOffset;
  [Key(5)]
  public int SellOffset;
  [Key(6)]
  public int LastDayChecked;

  [IgnoreMember]
  public int BuyPriceActual
  {
    get
    {
      return (int) Mathf.Clamp((float) this.BuyPrice - (float) this.BuyPrice * ((float) this.BuyOffset / 100f), 1f, (float) this.BuyPrice);
    }
  }

  [IgnoreMember]
  public int SellPriceActual => this.SellPrice + this.SellOffset;
}
