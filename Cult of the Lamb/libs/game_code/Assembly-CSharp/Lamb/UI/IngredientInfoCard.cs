// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IngredientInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class IngredientInfoCard : UIInfoCardBase<InventoryItem.ITEM_TYPE>
{
  [SerializeField]
  public IngredientItem _ingredientItem;
  [SerializeField]
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemLore;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;

  public override void Configure(InventoryItem.ITEM_TYPE config)
  {
    this._ingredientItem.Configure(config, false, true);
    this._itemHeader.text = LocalizationManager.GetTranslation($"Inventory/{config}");
    this._itemLore.text = LocalizationManager.GetTranslation($"Inventory/{config}/Lore");
    this._itemDescription.text = LocalizationManager.GetTranslation($"Inventory/{config}/Description");
  }
}
