// Decompiled with JetBrains decompiler
// Type: TraderTrackerItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class TraderTrackerItems
{
  public InventoryItem.ITEM_TYPE itemForTrade;
  public int SellPrice;
  public int BuyPrice;
  public int BuyOffsetPercent = 20;
  public int BuyOffset;
  public int SellOffset;
  public int LastDayChecked;

  public int BuyPriceActual
  {
    get
    {
      return (int) Mathf.Clamp((float) this.BuyPrice - (float) this.BuyPrice * ((float) this.BuyOffset / 100f), 1f, (float) this.BuyPrice);
    }
  }

  public int SellPriceActual => this.SellPrice + this.SellOffset;
}
