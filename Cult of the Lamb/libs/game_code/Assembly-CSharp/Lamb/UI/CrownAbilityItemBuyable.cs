// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CrownAbilityItemBuyable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CrownAbilityItemBuyable : BaseMonoBehaviour
{
  public const string kUnlockedLayer = "Unlocked";
  public const string kLockedLayer = "Locked";
  public Action<CrownAbilityItemBuyable> OnUpgradeChosen;
  [SerializeField]
  public RectTransform _shakeContainer;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public UpgradeTypeMapping _iconMapping;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public GameObject _alert;
  [SerializeField]
  public Image _flashIcon;
  public UpgradeSystem.Type _type;
  public Vector2 _origin;
  public StructuresData.ItemCost _cost;
  public bool _unlocked;

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
    this._flashIcon.gameObject.SetActive(false);
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

  public void OnButtonClicked()
  {
    Action<CrownAbilityItemBuyable> onUpgradeChosen = this.OnUpgradeChosen;
    if (onUpgradeChosen == null)
      return;
    onUpgradeChosen(this);
  }

  public void ForceIncognitoState()
  {
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
      this._alert.gameObject.SetActive(false);
    this._costText.text = "";
    this._icon.color = new Color(0.0f, 1f, 1f, 1f);
    this._flashIcon.gameObject.SetActive(true);
    this._flashIcon.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
  }

  public void AnimateIncognitoOut()
  {
    this._icon.DOKill();
    DOTweenModuleUI.DOColor(this._icon, Color.white, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._flashIcon.DOKill();
    DOTweenModuleUI.DOFade(this._flashIcon, 0.0f, 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() =>
    {
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
    }));
  }

  public void Shake()
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

  [CompilerGenerated]
  public void \u003CAnimateIncognitoOut\u003Eb__25_0()
  {
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
}
