// Decompiled with JetBrains decompiler
// Type: BuyEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class BuyEntry
{
  public bool Bought;
  public bool Decoration;
  public bool TarotCard;
  public TarotCards.Card Card;
  public StructureBrain.TYPES decorationToBuy;
  public InventoryItem.ITEM_TYPE itemToBuy;
  public InventoryItem.ITEM_TYPE costType;
  public int itemCost;
  public int quantity = 1;
  public int GroupID = -1;
  public bool SingleQuantityItem;
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

  private void Start() => this.pickedForSale = false;
}
