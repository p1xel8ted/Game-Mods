// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CurseItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CurseItem : PlayerMenuItem<EquipmentType>
{
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private GameObject _lockedContainer;

  public EquipmentType Type { private set; get; }

  public override void Configure(EquipmentType type)
  {
    this.Type = type;
    this._icon.gameObject.SetActive(type != EquipmentType.None);
    this._lockedContainer.SetActive(type == EquipmentType.None);
    if (type == EquipmentType.None)
      return;
    this._icon.sprite = EquipmentManager.GetEquipmentData(type).WorldSprite;
  }
}
