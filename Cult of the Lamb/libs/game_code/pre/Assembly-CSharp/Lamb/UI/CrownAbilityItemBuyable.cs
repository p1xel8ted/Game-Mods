// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CrownAbilityItemBuyable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CrownAbilityItemBuyable : BaseMonoBehaviour
{
  private const string kUnlockedLayer = "Unlocked";
  private const string kLockedLayer = "Locked";
  public Action<CrownAbilityItemBuyable> OnUpgradeChosen;
  [SerializeField]
  private RectTransform _shakeContainer;
  [SerializeField]
  private Animator _animator;
  [SerializeField]
  private TextMeshProUGUI _costText;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private UpgradeTypeMapping _iconMapping;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private GameObject _alert;
  private UpgradeSystem.Type _type;
  private Vector2 _origin;
  private StructuresData.ItemCost _cost;
  private bool _unlocked;

  public MMButton Button => this._button;

  public UpgradeSystem.Type Type => this._type;

  public StructuresData.ItemCost Cost => this._cost;

  public void Configure(UpgradeSystem.Type type)
  {
    this._type = type;
    this._origin = this._shakeContainer.anchoredPosition;
    this._cost = UpgradeSystem.GetCost(this._type)[0];
    this._unlocked = UpgradeSystem.GetUnlocked(this._type);
    this.UpdateState();
    this._icon.sprite = this._iconMapping.GetItem(this._type).Sprite;
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._button.OnConfirmDenied += new System.Action(this.Shake);
  }

  public void UpdateState()
  {
    this._button.Confirmable = this._cost.CanAfford() && !this._unlocked;
    this._alert.SetActive(this._cost.CanAfford() && !this._unlocked);
    if (!this._unlocked)
    {
      if (this._cost.CanAfford())
        this._icon.color = StaticColors.OffWhiteColor;
      else
        this._icon.color = StaticColors.GreyColor;
      this._costText.text = StructuresData.ItemCost.GetCostString(this._cost);
    }
    else
    {
      this._costText.text = "";
      this._icon.color = StaticColors.OffWhiteColor;
    }
  }

  private void OnButtonClicked()
  {
    Action<CrownAbilityItemBuyable> onUpgradeChosen = this.OnUpgradeChosen;
    if (onUpgradeChosen == null)
      return;
    onUpgradeChosen(this);
  }

  private void Shake()
  {
    this._shakeContainer.DOKill();
    this._shakeContainer.localScale = (Vector3) Vector2.one;
    this._shakeContainer.anchoredPosition = this._origin;
    this._shakeContainer.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void Bump()
  {
    this._shakeContainer.localScale = Vector3.one * 1.4f;
    this._shakeContainer.DOKill();
    this._shakeContainer.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }
}
