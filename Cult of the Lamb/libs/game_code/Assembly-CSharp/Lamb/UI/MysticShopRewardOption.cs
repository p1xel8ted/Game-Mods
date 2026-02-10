// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MysticShopRewardOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MysticShopRewardOption : MonoBehaviour
{
  [SerializeField]
  public GameObject _container;
  [SerializeField]
  public Image _itemGraphic;
  [SerializeField]
  public InventoryIconMapping _iconMapping;
  public InventoryItem.ITEM_TYPE _item;

  public void Configure(InventoryItem.ITEM_TYPE item, RelicType relicType)
  {
    Debug.Log((object) $"Configure! {item.ToString()}   {relicType.ToString()}");
    this._item = item;
    if (item == InventoryItem.ITEM_TYPE.RELIC)
      this._itemGraphic.sprite = EquipmentManager.GetRelicIcon(relicType);
    else
      this._itemGraphic.sprite = this._iconMapping.GetImage(this._item);
  }

  public void Choose() => this._container.SetActive(false);
}
