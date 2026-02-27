// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CurseWheelItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private EquipmentType _curseType;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private WeaponCurseIconMapping _iconMapping;
  [SerializeField]
  private Image _lockIcon;
  [SerializeField]
  private Image _selectedIcon;
  private bool _isUnlocked;

  public EquipmentType CurseType => EquipmentType.Tentacles;

  private void Start()
  {
    this._icon.color = new Color(0.0f, this._isUnlocked ? 1f : 0.0f, 1f, 1f);
    this._lockIcon.gameObject.SetActive(!this._isUnlocked);
    this.SetSelected(DataManager.Instance.CurrentCurse == this._curseType);
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
