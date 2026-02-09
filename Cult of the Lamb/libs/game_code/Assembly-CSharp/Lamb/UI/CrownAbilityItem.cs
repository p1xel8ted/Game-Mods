// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CrownAbilityItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CrownAbilityItem : PlayerMenuItem<UpgradeSystem.Type>
{
  [SerializeField]
  public bool _ignoreLockedState;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public GameObject _lockedContainer;
  [SerializeField]
  public UpgradeTypeMapping _upgradeTypeMapping;
  [CompilerGenerated]
  public UpgradeSystem.Type \u003CType\u003Ek__BackingField;

  public UpgradeSystem.Type Type
  {
    set => this.\u003CType\u003Ek__BackingField = value;
    get => this.\u003CType\u003Ek__BackingField;
  }

  public override void Configure(UpgradeSystem.Type type)
  {
    this.Type = type;
    if (this._ignoreLockedState)
      this._lockedContainer.SetActive(false);
    else
      this._lockedContainer.SetActive(!UpgradeSystem.GetUnlocked(type));
    this._icon.sprite = this._upgradeTypeMapping.GetItem(type).Sprite;
  }
}
