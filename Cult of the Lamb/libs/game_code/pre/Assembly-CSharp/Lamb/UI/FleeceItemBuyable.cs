// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FleeceItemBuyable
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

public class FleeceItemBuyable : MonoBehaviour
{
  private const string kUnlockedLayer = "Unlocked";
  private const string kLockedLayer = "Locked";
  public Action<int> OnFleeceChosen;
  [SerializeField]
  private RectTransform _shakeContainer;
  [SerializeField]
  private Animator _animator;
  [SerializeField]
  private TextMeshProUGUI _costText;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private FleeceIconMapping _fleeceIconMapping;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private Image _outline;
  [SerializeField]
  private GameObject _alert;
  private int _fleeceIndex;
  private bool _unlocked;
  private Vector2 _origin;
  private StructuresData.ItemCost _cost;

  public MMButton Button => this._button;

  public int Fleece => this._fleeceIndex;

  public StructuresData.ItemCost Cost => this._cost;

  public void Configure(int index)
  {
    this._fleeceIndex = index;
    this._outline.color = StaticColors.GreenColor;
    this._origin = this._shakeContainer.anchoredPosition;
    this._cost = new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.TALISMAN, 1);
    this._unlocked = DataManager.Instance.UnlockedFleeces.Contains(this._fleeceIndex);
    this._button.Confirmable = this._cost.CanAfford() && !this._unlocked || this._unlocked;
    this.UpdateState();
    this._fleeceIconMapping.GetImage(this._fleeceIndex, this._icon);
    this._fleeceIconMapping.GetImage(this._fleeceIndex, this._outline);
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._button.OnConfirmDenied += new System.Action(this.Shake);
  }

  public void UpdateState()
  {
    this._alert.SetActive(this._cost.CanAfford() && !this._unlocked);
    if (!this._unlocked)
    {
      this._outline.gameObject.SetActive(false);
      this._costText.text = StructuresData.ItemCost.GetCostString(new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.TALISMAN, 1));
      if (this._cost.CanAfford())
        this._icon.color = new Color(1f, 1f, 1f, 1f);
      else
        this._icon.color = new Color(0.0f, 1f, 1f, 1f);
    }
    else
    {
      this._icon.color = new Color(1f, 1f, 1f, 1f);
      this._costText.text = "";
      this._outline.gameObject.SetActive(DataManager.Instance.PlayerFleece == this._fleeceIndex);
    }
  }

  private void OnButtonClicked()
  {
    Action<int> onFleeceChosen = this.OnFleeceChosen;
    if (onFleeceChosen == null)
      return;
    onFleeceChosen(this._fleeceIndex);
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
}
