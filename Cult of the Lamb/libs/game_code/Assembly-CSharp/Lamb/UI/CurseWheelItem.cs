// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CurseWheelItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CurseWheelItem : UIRadialWheelItem
{
  [SerializeField]
  public EquipmentType _curseType;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public WeaponCurseIconMapping _iconMapping;
  [SerializeField]
  public Image _lockIcon;
  [SerializeField]
  public Image _selectedIcon;
  public bool _isUnlocked;

  public EquipmentType CurseType => EquipmentType.Tentacles;

  public void Start()
  {
    this._icon.color = new Color(0.0f, this._isUnlocked ? 1f : 0.0f, 1f, 1f);
    this._lockIcon.gameObject.SetActive(!this._isUnlocked);
    this.SetSelected(PlayerFarming.Instance.currentCurse == this._curseType);
  }

  public override string GetTitle() => "";

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

  public override string GetDescription() => "";
}
