// Decompiled with JetBrains decompiler
// Type: Trading
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Trading
{
  public const float SELLING_PRICE_MODIFICATOR = 0.75f;
  public Vendor trader;
  public WorldGameObject player;
  public MultiInventory player_inventory;
  public Item player_offer = new Item();

  public float player_money
  {
    get => this.player.data.money;
    set => this.player.data.money = value;
  }

  public Trading(WorldGameObject wgo)
  {
    this.trader = wgo.vendor;
    this.trader.FillDrawingMultiInventory();
    this.player = MainGame.me.player;
    this.player_inventory = this.player.GetMultiInventory(sortWGOS: true, include_bags: true);
  }

  public bool IsUsableItem(Item item)
  {
    if (item == null || item.IsEmpty())
      return true;
    foreach (string productType in item.definition.product_types)
    {
      if (this.trader.definition.GetProductTypes().Contains(productType))
        return true;
    }
    return false;
  }

  public InventoryWidget.ItemFilterResult SellableItemsFilter(Item item, InventoryWidget widget)
  {
    if (item == null || item.IsEmpty())
      return InventoryWidget.ItemFilterResult.Hide;
    return !this.trader.CanSellItem(item, true) ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
  }

  public InventoryWidget.ItemFilterResult BuyableItemsFilter(Item item, InventoryWidget widget)
  {
    return !this.trader.CanBuyItem(item, true) ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
  }

  public InventoryWidget.ItemFilterResult FilterItem(Item item)
  {
    return !this.IsUsableItem(item) ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
  }

  public float GetTotalBalance()
  {
    float num1 = 0.0f;
    List<string> stringList1 = new List<string>();
    foreach (Item obj in this.player_offer.inventory)
    {
      if (!stringList1.Contains(obj.id))
      {
        int totalCount = this.player_offer.GetTotalCount(obj.id);
        for (int index = 0; index < totalCount; ++index)
          num1 += Mathf.Round(this.GetSingleItemCostInPlayerInventory(obj, -index) * 100f) / 100f;
        stringList1.Add(obj.id);
      }
    }
    float num2 = 0.0f;
    List<string> stringList2 = new List<string>();
    foreach (Item obj in this.trader.cur_offer.inventory)
    {
      if (!stringList2.Contains(obj.id))
      {
        int totalCount = this.trader.cur_offer.GetTotalCount(obj.id);
        for (int index = 0; index < totalCount; ++index)
          num2 += Mathf.Round(this.GetSingleItemCostInTraderInventory(obj, index + 1) * 100f) / 100f;
        stringList2.Add(obj.id);
      }
    }
    return num1 - num2;
  }

  public float GetSingleItemCostInTraderInventory(Item item, int count_modificator = 0)
  {
    int items_count = this.trader.inventory.GetTotalCount(item.id) + count_modificator;
    return Mathf.Round(this.trader.GetSingleItemPrice(item, items_count) * 100f) / 100f;
  }

  public float GetSingleItemCostInTraderInventory(string item_id, int count_modificator = 0)
  {
    return this.GetSingleItemCostInTraderInventory(new Item(item_id, this.trader.inventory.GetTotalCount(item_id)), count_modificator);
  }

  public float GetSingleItemCostInPlayerInventory(Item item, int count_modificator = 0)
  {
    int items_count = this.trader.inventory.GetTotalCount(item.id) + this.player_offer.GetTotalCount(item.id) + count_modificator;
    float num = this.trader.GetSingleItemPrice(item, items_count) * 0.75f;
    if ((double) num > (double) item.definition.base_price)
      num = item.definition.base_price;
    return Mathf.Round(num * 100f) / 100f;
  }

  public float GetSingleItemCostInPlayerInventory(string item_id, int count_modificator = 0)
  {
    return this.GetSingleItemCostInPlayerInventory(new Item(item_id, this.player_inventory.GetTotalCount(item_id)), count_modificator);
  }

  public bool CanAcceptOffer()
  {
    if (!this.player_inventory.CanAddItems(this.trader.cur_offer.inventory, true) || !this.trader.inventory.CanAddItems(this.player_offer.inventory))
      return false;
    float totalBalance = this.GetTotalBalance();
    return (double) this.player_money + (double) totalBalance >= 0.0 && (double) this.trader.cur_money - (double) totalBalance >= 0.0;
  }

  public void DoAcceptOffer(bool need_check = true)
  {
    if (need_check && !this.CanAcceptOffer())
      return;
    float totalBalance = this.GetTotalBalance();
    this.player_money += totalBalance;
    if ((double) totalBalance > 0.0)
      Stats.PlayerAddMoney(totalBalance, this.trader.id);
    else
      Stats.PlayerDecMoney(-totalBalance, this.trader.id);
    this.trader.cur_money -= totalBalance;
    if (!this.trader.inventory.AddItems(this.player_offer.inventory))
    {
      Debug.LogError((object) "Can not add player's offer to vendor's inventory");
    }
    else
    {
      foreach (Item obj in this.trader.cur_offer.inventory)
      {
        if (!MainGame.me.player.AddToInventory(obj))
          Debug.LogError((object) $"Can not add vendor's offer's item \"{obj.id}\" to players's inventory");
        else if (obj.definition.equipment_type != ItemDefinition.EquipmentType.None)
          MainGame.me.player.TryEquipPickupedDrop(obj);
      }
      foreach (Item obj in this.player_offer.inventory)
      {
        for (int index = 0; index < obj.value; ++index)
          obj.OnTraded();
      }
      foreach (Item obj in this.trader.cur_offer.inventory)
      {
        for (int index = 0; index < obj.value; ++index)
          obj.OnTraded();
      }
      this.player_offer.inventory.Clear();
      this.trader.cur_offer.inventory.Clear();
      Debug.Log((object) "Accepted offer!");
      this.trader.FillDrawingMultiInventory();
    }
  }

  public static string FormatMoney(float value, bool print_zero = false, bool use_spaces = true)
  {
    int num1 = (double) value < 0.0 ? 1 : 0;
    value = Mathf.Round(Mathf.Abs(value) * 100f) / 100f;
    int num2 = Mathf.FloorToInt(value / 100f);
    int num3 = Mathf.FloorToInt(value - (float) num2 * 100f);
    int num4 = Mathf.RoundToInt((float) (((double) value - (double) num2 * 100.0 - (double) num3) * 100.0));
    string str1 = use_spaces ? " " : "";
    string str2 = (num1 != 0 ? "-" : "") + (num2 > 0 ? "(gld)" + num2.ToString() : "") + (num2 <= 0 || num3 <= 0 && num4 <= 0 ? "" : str1) + (num3 > 0 ? "(slv)" + num3.ToString() : "") + (num3 <= 0 || num4 <= 0 ? "" : str1) + (num4 > 0 ? "(brz)" + num4.ToString() : "");
    return str2.Length == 0 & print_zero ? "(brz)0" : str2;
  }

  public static void DrawMoneyOnLabel(UILabel label, float price, bool print_zero = false)
  {
    label.text = Trading.FormatMoney(price, print_zero);
  }
}
