// Decompiled with JetBrains decompiler
// Type: BuyEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class BuyEntry
{
  [Key(0)]
  public bool Bought;
  [Key(1)]
  public bool Decoration;
  [Key(2)]
  public bool TarotCard;
  [Key(3)]
  public TarotCards.Card Card;
  [Key(4)]
  public StructureBrain.TYPES decorationToBuy;
  [Key(5)]
  public InventoryItem.ITEM_TYPE itemToBuy;
  [Key(6)]
  public InventoryItem.ITEM_TYPE costType;
  [Key(7)]
  public int itemCost;
  [Key(8)]
  public int quantity = 1;
  [Key(9)]
  public int GroupID = -1;
  [Key(10)]
  public bool SingleQuantityItem;
  [IgnoreMember]
  public bool pickedForSale;

  public BuyEntry()
  {
  }

  public BuyEntry(
    InventoryItem.ITEM_TYPE itemToBuy,
    InventoryItem.ITEM_TYPE CostType,
    int itemCost,
    int quantity = 1)
  {
    this.itemToBuy = itemToBuy;
    this.itemCost = itemCost;
    this.costType = CostType;
    this.quantity = quantity;
  }

  public BuyEntry(
    StructureBrain.TYPES decorationToBuy,
    InventoryItem.ITEM_TYPE CostType,
    int itemCost,
    int quantity = 1)
  {
    this.decorationToBuy = decorationToBuy;
    this.itemCost = itemCost;
    this.costType = CostType;
    this.Decoration = true;
  }

  public void Start() => this.pickedForSale = false;
}
