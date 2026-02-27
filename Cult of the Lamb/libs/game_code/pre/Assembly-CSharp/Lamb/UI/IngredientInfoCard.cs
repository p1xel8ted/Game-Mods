// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IngredientInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class IngredientInfoCard : UIInfoCardBase<InventoryItem.ITEM_TYPE>
{
  [SerializeField]
  private IngredientItem _ingredientItem;
  [SerializeField]
  private TextMeshProUGUI _itemHeader;
  [SerializeField]
  private TextMeshProUGUI _itemLore;
  [SerializeField]
  private TextMeshProUGUI _itemDescription;

  public override void Configure(InventoryItem.ITEM_TYPE config)
  {
    this._ingredientItem.Configure(config, false, true);
    this._itemHeader.text = LocalizationManager.GetTranslation($"Inventory/{config}");
    this._itemLore.text = LocalizationManager.GetTranslation($"Inventory/{config}/Lore");
    this._itemDescription.text = LocalizationManager.GetTranslation($"Inventory/{config}/Description");
  }
}
