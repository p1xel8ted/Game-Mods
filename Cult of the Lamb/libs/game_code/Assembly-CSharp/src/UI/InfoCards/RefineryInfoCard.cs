// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RefineryInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.Assets;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class RefineryInfoCard : UIInfoCardBase<RefineryItem>
{
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _loreText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public Image _itemBefore;
  [SerializeField]
  public TextMeshProUGUI _itemBeforeQuantity;
  [SerializeField]
  public GameObject _itemBeforeAdditionalContainer;
  [SerializeField]
  public Image _itemBeforeAdditional;
  [SerializeField]
  public TextMeshProUGUI _itemBeforeQuantityAdditional;
  [SerializeField]
  public Image _itemAfter;
  [SerializeField]
  public TextMeshProUGUI _itemAfterQuantity;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [SerializeField]
  public TextMeshProUGUI _acquisitionText;
  [SerializeField]
  public InventoryIconMapping _iconMapping;

  public override void Configure(RefineryItem config)
  {
    List<StructuresData.ItemCost> cost = Structures_Refinery.GetCost(config.Type, config.Variant);
    StructuresData.ItemCost itemCost = new StructuresData.ItemCost(config.Type, Structures_Refinery.GetAmount(config.Type));
    this._headerText.text = LocalizationManager.GetTranslation($"Inventory/{config.Type}");
    this._loreText.text = LocalizationManager.GetTranslation($"Inventory/{config.Type}/Lore");
    this._descriptionText.text = LocalizationManager.GetTranslation($"Inventory/{config.Type}/Description");
    this._itemBefore.sprite = this._iconMapping.GetImage(cost[0].CostItem);
    this._itemBeforeQuantity.text = cost[0].CostValue.ToString();
    this._itemBeforeAdditionalContainer.gameObject.SetActive(cost.Count > 1);
    if (cost.Count > 1)
    {
      this._itemBeforeAdditional.sprite = this._iconMapping.GetImage(cost[1].CostItem);
      this._itemBeforeQuantityAdditional.text = cost[1].CostValue.ToString();
    }
    this._itemAfter.sprite = this._iconMapping.GetImage(config.Type);
    this._itemAfterQuantity.text = Structures_Refinery.GetAmount(config.Type).ToString();
    this._costText.text = StructuresData.ItemCost.GetCostStringWithQuantity(cost);
    this._acquisitionText.text = StructuresData.ItemCost.GetCostStringWithQuantity(itemCost);
  }
}
