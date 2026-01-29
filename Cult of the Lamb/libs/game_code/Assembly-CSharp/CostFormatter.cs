// Decompiled with JetBrains decompiler
// Type: CostFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
    List<StructuresData.ItemCost> itemCostList = new List<StructuresData.ItemCost>();
    for (int index1 = 0; index1 < itemCosts.Length; ++index1)
    {
      bool flag = false;
      for (int index2 = 0; index2 < itemCostList.Count; ++index2)
      {
        if (itemCostList[index2].CostItem == itemCosts[index1].CostItem)
        {
          itemCostList[index2].CostValue += itemCosts[index1].CostValue;
          flag = true;
          break;
        }
      }
      if (!flag)
        itemCostList.Add(new StructuresData.ItemCost(itemCosts[index1].CostItem, itemCosts[index1].CostValue));
    }
    foreach (StructuresData.ItemCost itemCost in itemCostList)
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

  public static string FormatCostBonus(
    InventoryItem.ITEM_TYPE itemType,
    int cost,
    int bonus,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    return CostFormatter.FormatCostBonus(itemType, cost, bonus, Inventory.GetItemQuantity(itemType), showQuantity, ignoreAffordability);
  }

  public static string FormatCostBonus(
    InventoryItem.ITEM_TYPE itemType,
    int cost,
    int bonus,
    int quantity,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    string str1 = LocalizeIntegration.ReverseText(cost.ToString());
    string str2 = $"{FontImageNames.GetIconByType(itemType)} {str1}".Bold();
    string str3 = LocalizeIntegration.ReverseText(quantity.ToString());
    string str4 = !LocalizeIntegration.IsArabic() ? $"({str3})".Size(-2) : $"){str3}(".Size(-2);
    string str5 = "+".Colour(StaticColors.GreenColor) + bonus.ToString().Colour(StaticColors.GreenColor) + str4.Colour(StaticColors.GreyColor);
    return $"{str2} {str5}";
  }

  public static string FormatCost(
    InventoryItem.ITEM_TYPE itemType,
    int cost,
    int quantity,
    bool showQuantity = true,
    bool ignoreAffordability = false)
  {
    string str1 = LocalizeIntegration.ReverseText(cost.ToString());
    string str2 = $"{FontImageNames.GetIconByType(itemType)} {str1}".Bold();
    string str3 = LocalizeIntegration.ReverseText(quantity.ToString());
    string str4 = !LocalizeIntegration.IsArabic() ? $"({str3})".Size(-2) : $"){str3}(".Size(-2);
    return cost > quantity && !ignoreAffordability ? $"{str2.Colour("#FD1D03")} {(showQuantity ? str4.Colour("#BA1400") : string.Empty)}" : $"{str2} {(showQuantity ? str4.Colour(StaticColors.GreyColor) : string.Empty)}";
  }
}
