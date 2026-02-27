// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CrownAbilityItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Assets;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CrownAbilityItem : PlayerMenuItem<UpgradeSystem.Type>
{
  [SerializeField]
  private bool _ignoreLockedState;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private GameObject _lockedContainer;
  [SerializeField]
  private UpgradeTypeMapping _upgradeTypeMapping;

  public UpgradeSystem.Type Type { private set; get; }

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
