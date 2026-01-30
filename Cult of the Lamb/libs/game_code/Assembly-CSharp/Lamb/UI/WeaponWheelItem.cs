// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WeaponWheelItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class WeaponWheelItem : UIRadialWheelItem
{
  [SerializeField]
  public TarotCards.Card _weaponType;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public WeaponCurseIconMapping _iconMapping;
  [SerializeField]
  public Image _lockIcon;
  [SerializeField]
  public Image _selectedIcon;
  public bool _isUnlocked;

  public TarotCards.Card WeaponType => this._weaponType;

  public void Start()
  {
    this._icon.sprite = this._iconMapping.GetSprite(this._weaponType);
    this._icon.color = new Color(0.0f, this._isUnlocked ? 1f : 0.0f, 1f, 1f);
    this._lockIcon.gameObject.SetActive(!this._isUnlocked);
  }

  public override string GetTitle() => TarotCards.LocalisedName(this._weaponType);

  public override bool IsValidOption() => this._isUnlocked;

  public override bool Visible() => true;

  public override void DoSelected()
  {
    base.DoSelected();
    if (!this._isUnlocked)
      return;
    this._icon.DOKill();
    DOTweenModuleUI.DOColor(this._icon, Color.white, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public override void DoDeselected()
  {
    base.DoDeselected();
    if (!this._isUnlocked)
      return;
    this._icon.DOKill();
    DOTweenModuleUI.DOColor(this._icon, new Color(0.0f, 1f, 1f, 1f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }

  public void SetSelected(bool selected) => this._selectedIcon.gameObject.SetActive(selected);

  public override string GetDescription()
  {
    return TarotCards.LocalisedDescription(this._weaponType, (PlayerFarming) null);
  }
}
