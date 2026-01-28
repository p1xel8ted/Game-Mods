// Decompiled with JetBrains decompiler
// Type: Lamb.UI.ItemInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class ItemInfoCard : UIInfoCardBase<InventoryItem.ITEM_TYPE>
{
  [SerializeField]
  public GenericInventoryItem _inventoryIcon;
  [SerializeField]
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemLore;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;

  public override void Configure(InventoryItem.ITEM_TYPE config)
  {
    this._inventoryIcon.Configure(config, false);
    this._itemHeader.text = LocalizationManager.GetTranslation($"Inventory/{config}");
    this._itemLore.text = LocalizationManager.GetTranslation($"Inventory/{config}/Lore");
    this._itemDescription.text = LocalizationManager.GetTranslation($"Inventory/{config}/Description");
  }
}
