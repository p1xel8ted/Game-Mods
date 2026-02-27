// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RefineryItemInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class RefineryItemInfoCard : ItemInfoCard
{
  [SerializeField]
  private TextMeshProUGUI _itemCost;

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
      costText = $"{costText + (costValue > itemQuantity ? "<color=#ff0000>" : "<color=#FEF0D3>") + FontImageNames.GetIconByType(cost[index].CostItem)}{(object) itemQuantity}</color> / {costValue.ToString()}  ";
    }
    Debug.Log((object) ("GetCostText: " + costText));
    return costText;
  }
}
