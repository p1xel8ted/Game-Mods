// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RefineryInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class RefineryInfoCard : UIInfoCardBase<InventoryItem.ITEM_TYPE>
{
  [SerializeField]
  private TextMeshProUGUI _headerText;
  [SerializeField]
  private TextMeshProUGUI _loreText;
  [SerializeField]
  private TextMeshProUGUI _descriptionText;
  [SerializeField]
  private Image _itemBefore;
  [SerializeField]
  private TextMeshProUGUI _itemBeforeQuantity;
  [SerializeField]
  private Image _itemAfter;
  [SerializeField]
  private TextMeshProUGUI _itemAfterQuantity;
  [SerializeField]
  private TextMeshProUGUI _costText;
  [SerializeField]
  private TextMeshProUGUI _acquisitionText;
  [SerializeField]
  private InventoryIconMapping _iconMapping;

  public override void Configure(InventoryItem.ITEM_TYPE config)
  {
    StructuresData.ItemCost itemCost1 = Structures_Refinery.GetCost(config)[0];
    StructuresData.ItemCost itemCost2 = new StructuresData.ItemCost(config, Structures_Refinery.GetAmount(config));
    this._headerText.text = LocalizationManager.GetTranslation($"Inventory/{config}");
    this._descriptionText.text = LocalizationManager.GetTranslation($"Inventory/{config}/Description");
    this._itemBefore.sprite = this._iconMapping.GetImage(itemCost1.CostItem);
    this._itemBeforeQuantity.text = itemCost1.CostValue.ToString();
    this._itemAfter.sprite = this._iconMapping.GetImage(config);
    this._itemAfterQuantity.text = Structures_Refinery.GetAmount(config).ToString();
    this._costText.text = StructuresData.ItemCost.GetCostStringWithQuantity(itemCost1);
    this._acquisitionText.text = StructuresData.ItemCost.GetCostStringWithQuantity(itemCost2);
  }
}
