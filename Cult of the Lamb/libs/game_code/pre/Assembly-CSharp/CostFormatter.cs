// Decompiled with JetBrains decompiler
// Type: CostFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CostFormatter
{
  public static string FormatCosts(
    List<InventoryItem> items,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    List<StructuresData.ItemCost> itemCosts = new List<StructuresData.ItemCost>();
    foreach (InventoryItem inventoryItem in items)
      itemCosts.Add(new StructuresData.ItemCost((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity));
    return CostFormatter.FormatCosts(itemCosts, showQuantity, ignoreAffordability);
  }

  public static string FormatCosts(
    List<StructuresData.ItemCost> itemCosts,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    return CostFormatter.FormatCosts(itemCosts.ToArray(), showQuantity, ignoreAffordability);
  }

  public static string FormatCosts(
    StructuresData.ItemCost[] itemCosts,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    string empty = string.Empty;
    foreach (StructuresData.ItemCost itemCost in itemCosts)
      empty += CostFormatter.FormatCost(itemCost, showQuantity, ignoreAffordability);
    return empty;
  }

  public static string FormatCost(
    StructuresData.ItemCost itemCost,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    return CostFormatter.FormatCost(itemCost.CostItem, itemCost.CostValue, Inventory.GetItemQuantity(itemCost.CostItem), showQuantity, ignoreAffordability);
  }

  public static string FormatCost(
    InventoryItem.ITEM_TYPE itemType,
    int cost,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    return CostFormatter.FormatCost(itemType, cost, Inventory.GetItemQuantity(itemType), showQuantity, ignoreAffordability);
  }

  public static string FormatCost(
    InventoryItem.ITEM_TYPE itemType,
    int cost,
    int quantity,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    string str1 = $"{FontImageNames.GetIconByType(itemType)} {(object) cost}".Bold();
    string str2 = $"({(object) quantity})".Size(-2);
    return cost > quantity && !ignoreAffordability ? $"{str1.Colour("#FD1D03")} {(showQuantity ? str2.Colour("#BA1400") : string.Empty)}" : $"{str1} {(showQuantity ? str2.Colour(StaticColors.GreyColor) : string.Empty)}";
  }
}
