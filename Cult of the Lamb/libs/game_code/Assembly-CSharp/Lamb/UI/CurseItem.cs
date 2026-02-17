// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CurseItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CurseItem : PlayerMenuItem<EquipmentType>
{
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public GameObject _lockedContainer;
  [CompilerGenerated]
  public EquipmentType \u003CType\u003Ek__BackingField;

  public EquipmentType Type
  {
    set => this.\u003CType\u003Ek__BackingField = value;
    get => this.\u003CType\u003Ek__BackingField;
  }

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
