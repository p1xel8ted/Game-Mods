// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RefineryItemInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class RefineryItemInfoCard : ItemInfoCard
{
  [SerializeField]
  public TextMeshProUGUI _itemCost;

  public override void Configure(InventoryItem.ITEM_TYPE config)
  {
    this._itemCost.text = this.GetCostText(config);
    base.Configure(config);
  }

  public string GetCostText(InventoryItem.ITEM_TYPE type)
  {
    string costText = "";
    List<StructuresData.ItemCost> cost = Structures_Refinery.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      int itemQuantity = Inventory.GetItemQuantity((int) cost[index].CostItem);
      int costValue = cost[index].CostValue;
      costText = $"{costText + (costValue > itemQuantity ? "<color=#ff0000>" : "<color=#FEF0D3>") + FontImageNames.GetIconByType(cost[index].CostItem)}{itemQuantity.ToString()}</color> / {costValue.ToString()}  ";
    }
    Debug.Log((object) ("GetCostText: " + costText));
    return costText;
  }
}
