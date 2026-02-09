// Decompiled with JetBrains decompiler
// Type: Vendor
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class Vendor
{
  public const float TRADING_WITH_BANK_COEFF = 0.05f;
  public const float TRADING_WITH_BANK_MAX_BASE_COUNT_PART_SALE = 0.25f;
  public const float TRADING_WITH_BANK_MAX_BASE_COUNT_PART_BUY = 0.15f;
  public const float ADDITIONAL_SUM_COEFF = 0.1f;
  public const string VENDOR_INITED = "vendor_inited";
  public const string MONEY = "money";
  public const string VENDOR_TIER = "vendor_tier";
  public const string LEVELUP_BAR = "levelup_bar_";
  public string id;
  public VendorDefinition _definition;
  [NonSerialized]
  public MultiInventory inventory;
  [NonSerialized]
  public MultiInventory drawing_inventory;
  [NonSerialized]
  public Item vendor_data;
  [NonSerialized]
  public Item cur_offer;
  public int max_tier;

  public VendorDefinition definition
  {
    get
    {
      if (this._definition == null)
        this._definition = string.IsNullOrEmpty(this.id) || this.id == "empty" ? (VendorDefinition) null : GameBalance.me.GetDataOrNull<VendorDefinition>(this.id);
      return this._definition;
    }
  }

  public float cur_money
  {
    get => this.vendor_data == null ? 0.0f : this.vendor_data.GetParam("money");
    set
    {
      if (this.vendor_data == null)
        return;
      this.vendor_data.SetParam("money", value);
    }
  }

  public int cur_tier
  {
    get => this.vendor_data == null ? 0 : this.vendor_data.GetParamInt("vendor_tier");
    set
    {
      if (this.vendor_data == null)
        return;
      this.vendor_data.SetParam("vendor_tier", (float) value);
    }
  }

  public Vendor(
    MultiInventory vendor_inventories,
    VendorDefinition vendor_definition,
    Item vendor_data)
  {
    this.id = vendor_definition.id;
    this._definition = vendor_definition;
    this.vendor_data = vendor_data;
    this.cur_offer = new Item();
    for (int index = 1; index < 3; ++index)
      vendor_data.SetParam("levelup_bar_" + index.ToString(), 0.0f);
    this.inventory = vendor_inventories;
    if ((double) Mathf.Abs(vendor_data.GetParam("vendor_inited") - 1f) <= 0.0099999997764825821)
      return;
    this.cur_tier = vendor_definition.start_tire;
    this.cur_money = vendor_definition.start_money;
    this.FillVendorInventory();
    vendor_data.SetParam("vendor_inited", 1f);
  }

  public void FillDrawingMultiInventory()
  {
    List<Item>[] objListArray = new List<Item>[3];
    for (int index = 0; index < objListArray.Length; ++index)
      objListArray[index] = new List<Item>();
    foreach (Item obj in this.inventory.all[0].data.inventory)
    {
      if (obj != null && obj.definition != null)
      {
        int productTier = obj.definition.product_tier;
        switch (productTier)
        {
          case 1:
          case 2:
          case 3:
            objListArray[productTier - 1].Add(obj);
            continue;
          default:
            continue;
        }
      }
    }
    this.max_tier = 3;
    for (int index = 2; index >= 0 && objListArray[index].Count == 0; --index)
      this.max_tier = index;
    Inventory[] inventoryArray = new Inventory[3];
    if (this.max_tier == 0)
      this.max_tier = 1;
    for (int index1 = 0; index1 < this.max_tier; ++index1)
    {
      int n = index1 + 1;
      Item data = new Item();
      data.SetParam("inventory_size", (float) (objListArray[index1].Count + 100));
      inventoryArray[index1] = new Inventory(data, $"{GJL.L("items_tier")} {GJCommons.GetRomeNumber(n)}")
      {
        is_locked = n > this.cur_tier,
        vendor_tier_info = new Inventory.VendorTierInfo()
        {
          progressbar_visible = false,
          tier_1 = this.cur_tier,
          tier_2 = n
        }
      };
      if (n == this.cur_tier + 1)
      {
        inventoryArray[index1].vendor_tier_info.progressbar_visible = true;
        inventoryArray[index1].vendor_tier_info.progress = this.GetVendorLevelupProgress();
      }
      for (int index2 = 0; index2 < objListArray[index1].Count; ++index2)
        inventoryArray[index1].data.AddItem(objListArray[index1][index2]);
    }
    if (this.drawing_inventory == null)
    {
      this.drawing_inventory = new MultiInventory(inventoryArray[0]);
      for (int index = 1; index < this.max_tier; ++index)
        this.drawing_inventory.AddInventory(inventoryArray[index]);
    }
    else
    {
      List<Inventory> inventories = new List<Inventory>();
      for (int index = 0; index < 3 && inventoryArray[index] != null; ++index)
        inventories.Add(inventoryArray[index]);
      this.drawing_inventory.SetInventories(inventories);
    }
  }

  public float GetSingleItemPrice(Item item, int items_count = 0)
  {
    return item == null || item.definition == null ? 0.0f : this.GetSingleItemPrice(item.definition, items_count);
  }

  public float GetSingleItemPrice(ItemDefinition item_def, int items_count = 0)
  {
    if (!this.CanTradeItem(item_def))
      return 0.0f;
    if (items_count == 0)
      items_count = this.GetItemsCount(item_def.id);
    int modifiedBaseCount = this.GetModifiedBaseCount(item_def);
    return (float) Math.Round((double) item_def.GetPrice(items_count, modifiedBaseCount), 2);
  }

  public bool CanSellItem(Item item, bool check_tier = false)
  {
    return item != null && this.CanSellItem(item.definition, check_tier);
  }

  public bool CanSellItem(ItemDefinition item_def, bool check_tier = false)
  {
    if (item_def == null || item_def.product_types == null || item_def.product_types.Count == 0 || check_tier && item_def.product_tier > this.cur_tier || !this.CanTradeItemType(item_def.product_types))
      return false;
    foreach (VendorDefinition.ItemModificator itemModificator in this.definition.not_selling)
    {
      if (itemModificator.item_name == item_def.id && (itemModificator.tier < 1 || itemModificator.tier == this.cur_tier))
        return false;
    }
    return true;
  }

  public bool CanBuyItem(Item item, bool check_tier = false)
  {
    return item != null && this.CanBuyItem(item.definition, check_tier);
  }

  public bool CanBuyItem(ItemDefinition item_def, bool check_tier = false)
  {
    if (item_def == null || item_def.product_types == null || item_def.product_types.Count == 0 || check_tier && item_def.product_tier > this.cur_tier || !this.CanTradeItemType(item_def.product_types))
      return false;
    foreach (VendorDefinition.ItemModificator itemModificator in this.definition.not_buying)
    {
      if (itemModificator.item_name == item_def.id && (itemModificator.tier < 1 || itemModificator.tier == this.cur_tier))
        return false;
    }
    return true;
  }

  public bool CanBuyOrSellItem(ItemDefinition item_def, bool check_tier = false)
  {
    if (item_def == null || item_def.product_types == null || item_def.product_types.Count == 0 || check_tier && item_def.product_tier > this.cur_tier || !this.CanTradeItemType(item_def.product_types))
      return false;
    bool flag = true;
    foreach (VendorDefinition.ItemModificator itemModificator in this.definition.not_buying)
    {
      if (itemModificator.item_name == item_def.id && (itemModificator.tier < 1 || itemModificator.tier == this.cur_tier))
      {
        flag = false;
        break;
      }
    }
    if (flag)
      return true;
    foreach (VendorDefinition.ItemModificator itemModificator in this.definition.not_selling)
    {
      if (itemModificator.item_name == item_def.id && (itemModificator.tier < 1 || itemModificator.tier == this.cur_tier))
        return false;
    }
    return true;
  }

  public bool CanTradeItem(ItemDefinition item_def)
  {
    return item_def != null && item_def.product_types != null && item_def.product_types.Count != 0 && this.CanTradeItemType(item_def.product_types);
  }

  public bool CanTradeItemType(List<string> item_types)
  {
    foreach (string itemType in item_types)
    {
      if (this.definition.GetProductTypes().Contains(itemType))
        return true;
    }
    return false;
  }

  public int GetModifiedBaseCount(ItemDefinition item_def)
  {
    int baseCount = item_def.base_count;
    foreach (VendorDefinition.CountModificator countModificator in this.definition.count_modificators)
    {
      if (countModificator.item_name == item_def.id && countModificator.tier <= this.cur_tier)
        baseCount = countModificator.base_count;
    }
    return baseCount;
  }

  public int GetItemsCount(string item_id)
  {
    return this.inventory != null ? this.inventory.GetTotalCount(item_id) : 0;
  }

  public void FillVendorInventory()
  {
    foreach (ItemDefinition item_def in GameBalance.me.items_data)
    {
      if (this.CanBuyOrSellItem(item_def))
      {
        int baseCount = item_def.base_count;
        foreach (VendorDefinition.CountModificator countModificator in this.definition.count_modificators)
        {
          if (countModificator.item_name == item_def.id)
            baseCount = countModificator.base_count;
        }
        if (baseCount != 0)
          this.inventory.AddItem(item_def.id, item_def.product_tier > this.cur_tier ? 1 : item_def.base_count);
      }
    }
  }

  public int CalcPurchaseAtTheEndOfDay(ItemDefinition item_def)
  {
    int modifiedBaseCount = this.GetModifiedBaseCount(item_def);
    float basePrice = item_def.base_price;
    int itemsCount = this.GetItemsCount(item_def.id);
    float singleItemPrice = this.GetSingleItemPrice(item_def, itemsCount);
    if (modifiedBaseCount > 0 && (double) Mathf.Abs(((float) itemsCount - (float) modifiedBaseCount) / (float) modifiedBaseCount) < 0.05000000074505806)
      return 0;
    int num;
    if (item_def.is_static_cost)
    {
      num = modifiedBaseCount - itemsCount;
    }
    else
    {
      num = (double) Mathf.Abs(singleItemPrice - basePrice) >= 0.0099999997764825821 ? ((double) singleItemPrice < (double) basePrice ? Mathf.FloorToInt((float) ((double) modifiedBaseCount * ((double) itemsCount / (double) modifiedBaseCount) * 0.05000000074505806 * -1.0)) : Mathf.CeilToInt((float) ((double) modifiedBaseCount * ((double) modifiedBaseCount / ((double) itemsCount + 1.0)) * 0.05000000074505806))) : Math.Sign(modifiedBaseCount - itemsCount);
      if ((double) num > (double) modifiedBaseCount * 0.15000000596046448)
        num = Mathf.CeilToInt((float) (Math.Sign(num) * modifiedBaseCount) * 0.15f);
      else if ((double) num < (double) (-1 * modifiedBaseCount) * 0.25)
        num = Mathf.FloorToInt((float) (Math.Sign(num) * modifiedBaseCount) * 0.25f);
    }
    if (itemsCount > modifiedBaseCount && num > 0)
      Debug.LogError((object) $"Something wrong with item {{{item_def.id}}} while vendor [{this.definition.id}] trading with bank #1. Call Bulat {{{itemsCount.ToString()}/{modifiedBaseCount.ToString()}; {singleItemPrice.ToString()}/{basePrice.ToString()}}}");
    else if (itemsCount < modifiedBaseCount && num < 0)
      Debug.LogError((object) $"Something wrong with item {{{item_def.id}}} while vendor [{this.definition.id}] trading with bank #2. Call Bulat {{{itemsCount.ToString()}/{modifiedBaseCount.ToString()}; {singleItemPrice.ToString()}/{basePrice.ToString()}}}");
    return num;
  }

  public void TradeWithBank()
  {
    this.cur_money += this.definition.daily_money_income;
    List<ItemDefinition> itemDefinitionList = new List<ItemDefinition>();
    List<int> intList = new List<int>();
    foreach (ItemDefinition item_def in GameBalance.me.items_data)
    {
      if (this.CanBuyOrSellItem(item_def) && item_def.product_tier <= this.cur_tier)
      {
        int num = this.CalcPurchaseAtTheEndOfDay(item_def);
        if (num > 0)
        {
          itemDefinitionList.Add(item_def);
          intList.Add(num);
        }
        else if (num < 0)
        {
          this.inventory.RemoveItem(item_def.id, Math.Abs(num));
          this.cur_money -= (float) num * item_def.base_price * item_def.custom_sell_price_koeff;
        }
      }
    }
    if (itemDefinitionList.Count != intList.Count)
    {
      Debug.LogError((object) "FATAL ERROR! Count of items not equal to count of counts!");
    }
    else
    {
      bool flag;
      do
      {
        flag = false;
        for (int index = 0; index < intList.Count; ++index)
        {
          if (intList[index] > 0 && (double) this.cur_money >= (double) itemDefinitionList[index].base_price)
          {
            intList[index]--;
            this.cur_money -= itemDefinitionList[index].base_price;
            this.inventory.AddItem(itemDefinitionList[index].id, 1);
            flag = true;
          }
        }
      }
      while (flag);
    }
  }

  public bool TryLevelUpVendor()
  {
    if ((double) this.GetVendorLevelupProgress() <= 1.0)
      return false;
    ++this.cur_tier;
    Stats.DesignEvent($"VendorLevel:{this.id}:{this.cur_tier.ToString()}");
    return true;
  }

  public float GetMoneyNeededForVendorLevelUp()
  {
    if (this.cur_tier >= 3)
      return float.PositiveInfinity;
    float num1 = 0.0f;
    bool flag = (double) Mathf.Abs(this.definition.levelup_costs[this.cur_tier - 1]) < 0.0099999997764825821;
    float num2 = 0.0f;
    ++this.cur_tier;
    foreach (ItemDefinition item_def in GameBalance.me.items_data)
    {
      if (item_def.product_tier <= this.cur_tier && this.CanBuyOrSellItem(item_def))
      {
        int modifiedBaseCount = this.GetModifiedBaseCount(item_def);
        int totalCount = this.inventory.GetTotalCount(item_def.id);
        int num3;
        float num4 = (float) (num3 = modifiedBaseCount - totalCount) * item_def.base_price;
        if (num3 < 0)
          num4 *= item_def.custom_sell_price_koeff;
        num1 += num4;
        if (flag)
          num2 += (float) ((double) modifiedBaseCount * (double) item_def.base_price * 0.10000000149011612);
      }
    }
    --this.cur_tier;
    return (double) num1 >= 0.0 ? (!flag ? num1 + this.definition.levelup_costs[this.cur_tier - 1] : num1 + num2) : 0.01f;
  }

  public void OnEndOfDay()
  {
    if (string.IsNullOrEmpty(this.definition?.id))
    {
      if (string.IsNullOrEmpty(this.id))
        Debug.LogWarning((object) "Found vendor with null id");
      else if (this.id == "empty")
        Debug.LogWarning((object) "Found vendor with id=\"empty\"");
      else if (this.definition == null)
        Debug.LogWarning((object) $"Not found definition for vendor with id=\"{this.id}\"");
      else
        Debug.LogWarning((object) "Found vendor with null definiton.id");
    }
    else
    {
      this.TradeWithBank();
      if (this.cur_tier < 3)
      {
        if (!this.TryLevelUpVendor())
          return;
        Debug.Log((object) $"Vendor \"{this.definition.id}\" level up. Cur tier = {this.cur_tier.ToString()}");
      }
      else
        Debug.Log((object) $"Vendor \"{this.definition.id}\" already max tier.");
    }
  }

  public float GetVendorLevelupProgress()
  {
    float levelUpBar = this.GetLevelUpBar();
    float totalGoods = this.GetTotalGoods();
    float totalGoodsOnNextTier = this.GetTotalGoodsOnNextTier();
    Debug.Log((object) $"Vendor \"{this.id}\" real level up progress: {totalGoods.ToString()}/{totalGoodsOnNextTier.ToString()}");
    float num = totalGoodsOnNextTier - levelUpBar;
    float vendorLevelupProgress = (float) (((double) totalGoods - (double) num) / ((double) totalGoodsOnNextTier - (double) num));
    if ((double) vendorLevelupProgress < 0.0)
      vendorLevelupProgress = 0.0f;
    return vendorLevelupProgress;
  }

  public float GetTotalGoods()
  {
    float curMoney = this.cur_money;
    for (int index = 0; index < GameBalance.me.items_data.Count; ++index)
    {
      ItemDefinition item_def = GameBalance.me.items_data[index];
      if (item_def.product_tier <= this.cur_tier && this.CanBuyOrSellItem(item_def))
      {
        float num = (float) this.inventory.GetTotalCount(item_def.id) * item_def.base_price;
        if ((double) item_def.custom_sell_price_koeff > 1.0)
          num *= item_def.custom_sell_price_koeff;
        curMoney += num;
      }
    }
    return curMoney;
  }

  public float GetTotalGoodsOnNextTier()
  {
    if (this.cur_tier >= 3)
      return 0.0f;
    float num1 = 0.0f;
    ++this.cur_tier;
    for (int index = 0; index < GameBalance.me.items_data.Count; ++index)
    {
      ItemDefinition item_def = GameBalance.me.items_data[index];
      if (item_def.product_tier <= this.cur_tier && this.CanBuyOrSellItem(item_def))
      {
        float num2 = (float) this.GetModifiedBaseCount(item_def) * item_def.base_price;
        num1 += num2;
      }
    }
    --this.cur_tier;
    return (double) Mathf.Abs(this.definition.levelup_costs[this.cur_tier - 1]) >= 0.0099999997764825821 ? num1 + this.definition.levelup_costs[this.cur_tier - 1] : num1 * 1.1f;
  }

  public float GetLevelUpBar()
  {
    if (this.vendor_data == null)
      return 0.0f;
    if (this.cur_tier >= 3)
    {
      Debug.LogError((object) "Wrong tier!");
      return 0.0f;
    }
    if ((double) this.vendor_data.GetParam("levelup_bar_" + this.cur_tier.ToString()) < 0.10000000149011612)
      this.CalculateLevelUpBar();
    return this.vendor_data.GetParam("levelup_bar_" + this.cur_tier.ToString());
  }

  public void CalculateLevelUpBar()
  {
    if (this.cur_tier >= 3)
    {
      Debug.LogError((object) "Wrong tier #2!");
    }
    else
    {
      float num1 = this.GetTotalGoods() - this.cur_money;
      float totalGoodsOnNextTier = this.GetTotalGoodsOnNextTier();
      float num2 = totalGoodsOnNextTier - num1;
      if ((double) num2 < 0.10000000149011612)
      {
        Debug.LogError((object) $"Wrong bar for vendor \"{this.id}\": {num2.ToString()} = {totalGoodsOnNextTier.ToString()} - {num1.ToString()}");
        num2 = totalGoodsOnNextTier;
      }
      this.vendor_data.SetParam("levelup_bar_" + this.cur_tier.ToString(), num2);
    }
  }
}
