// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FleeceItemBuyable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.Assets;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class FleeceItemBuyable : MonoBehaviour
{
  public const string kUnlockedLayer = "Unlocked";
  public const string kLockedLayer = "Locked";
  public Action<int> OnFleeceChosen;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _shakeContainer;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public FleeceIconMapping _fleeceIconMapping;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public Image _outline;
  [SerializeField]
  public GameObject _alert;
  [SerializeField]
  public Image _flashIcon;
  [SerializeField]
  public int _forcedFleeceIndex = -1;
  [SerializeField]
  public Image _DLCStar;
  [SerializeField]
  public FleeceItem _transmogFleece;
  [SerializeField]
  public GameObject _transmogFleeceContainer;
  [SerializeField]
  public int _sinCost = -1;
  public bool postGameFleece;
  public int _fleeceIndex;
  public bool _unlocked;
  public Vector2 _origin;
  public StructuresData.ItemCost _cost;
  public int _transmogCache;

  public int ForcedFleeceIndex => this._forcedFleeceIndex;

  public MMButton Button => this._button;

  public int Fleece => this._fleeceIndex;

  public StructuresData.ItemCost Cost => this._cost;

  public RectTransform RectTransform => this._rectTransform;

  public bool Unlocked => this._unlocked;

  public void Configure(int index)
  {
    this._fleeceIndex = index;
    if (this.postGameFleece && !DataManager.Instance.PostGameFleecesOnboarded || index == 676 && !DataManager.Instance.CowboyFleeceOnboarded || index == 1003 && !DataManager.Instance.GoatFleeceOnboarded)
      this.gameObject.SetActive(false);
    else if (this._forcedFleeceIndex != -1 && !DataManager.Instance.UnlockedFleeces.Contains(this._forcedFleeceIndex) && this._sinCost == -1)
    {
      index = this._forcedFleeceIndex;
      this.gameObject.SetActive(false);
    }
    else
    {
      this._outline.color = StaticColors.GreenColor;
      this._origin = this._shakeContainer.anchoredPosition;
      this._cost = new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.TALISMAN, 1);
      if (this._sinCost != -1)
        this._cost = new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, this._sinCost);
      this._unlocked = DataManager.Instance.UnlockedFleeces.Contains(this._fleeceIndex);
      this._button.Confirmable = this._cost.CanAfford() && !this._unlocked || this._unlocked;
      this.UpdateState();
      this._fleeceIconMapping.GetImage(this._fleeceIndex, this._icon);
      this._fleeceIconMapping.GetImage(this._fleeceIndex, this._outline);
      this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
      this._button.OnConfirmDenied += new System.Action(this.Shake);
      this._flashIcon.gameObject.SetActive(false);
      this._DLCStar.gameObject.SetActive(false);
      if (index == 1001 || index == 999 || index == 1002)
        this._DLCStar.gameObject.SetActive(true);
      this._transmogFleeceContainer.gameObject.SetActive(false);
      Debug.Log((object) "Config Buyable");
      for (int index1 = 0; index1 < DataManager.Instance.CustomisedFleeceOptions.Count; ++index1)
      {
        if ((double) DataManager.Instance.CustomisedFleeceOptions[index1].x == (double) index && (double) DataManager.Instance.CustomisedFleeceOptions[index1].x != (double) DataManager.Instance.CustomisedFleeceOptions[index1].y)
        {
          if ((double) DataManager.Instance.CustomisedFleeceOptions[index1].y == 1003.0 && CoopManager.CoopActive)
            DataManager.Instance.CustomisedFleeceOptions[index1] = new Vector2(DataManager.Instance.CustomisedFleeceOptions[index1].x, 0.0f);
          this._transmogFleeceContainer.gameObject.SetActive(true);
          this._transmogFleece.Configure((int) DataManager.Instance.CustomisedFleeceOptions[index1].x);
          if (this._transmogCache != (int) DataManager.Instance.CustomisedFleeceOptions[index1].y)
            this.PunchPositionFleece();
          this._fleeceIconMapping.GetImage((int) DataManager.Instance.CustomisedFleeceOptions[index1].y, this._icon);
          this._fleeceIconMapping.GetImage((int) DataManager.Instance.CustomisedFleeceOptions[index1].y, this._outline);
          if ((int) DataManager.Instance.CustomisedFleeceOptions[index1].y == 1001 || (int) DataManager.Instance.CustomisedFleeceOptions[index1].y == 999)
            this._DLCStar.gameObject.SetActive(true);
          this._transmogCache = (int) DataManager.Instance.CustomisedFleeceOptions[index1].y;
        }
      }
    }
  }

  public void PunchPositionFleece()
  {
    Debug.Log((object) "Punch Fleece");
    this._shakeContainer.DOKill();
    this._shakeContainer.localScale = (Vector3) Vector2.one;
    this._shakeContainer.anchoredPosition = this._origin;
    this._shakeContainer.transform.DOPunchPosition(new Vector3(0.0f, 5f, 0.0f), 0.5f);
  }

  public void UpdateState()
  {
    if (this._cost == null)
      return;
    this._alert.SetActive(this._cost.CanAfford() && !this._unlocked);
    this._costText.isRightToLeftText = LocalizeIntegration.IsArabic();
    if (!this._unlocked)
    {
      this._outline.gameObject.SetActive(false);
      this._costText.text = StructuresData.ItemCost.GetCostString(new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.TALISMAN, 1));
      if (this._sinCost != -1)
        this._costText.text = StructuresData.ItemCost.GetCostString(new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, this._sinCost));
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

  public void OnButtonClicked()
  {
    if (this._fleeceIndex == 1003 && CoopManager.CoopActive)
    {
      this.Shake();
    }
    else
    {
      Action<int> onFleeceChosen = this.OnFleeceChosen;
      if (onFleeceChosen == null)
        return;
      onFleeceChosen(this._fleeceIndex);
    }
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
        this._costText.text = StructuresData.ItemCost.GetCostString(new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.TALISMAN, 1));
        if (this._sinCost != -1)
          this._costText.text = StructuresData.ItemCost.GetCostString(new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, this._sinCost));
        if (this._cost.CanAfford())
          this._icon.color = new Color(1f, 1f, 1f, 1f);
        else
          this._icon.color = new Color(0.0f, 1f, 1f, 1f);
      }
      else
        this._costText.text = "";
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

  public void PrepareForUnlock()
  {
    this._costText.text = string.Empty;
    this._shakeContainer.gameObject.SetActive(false);
  }

  public IEnumerator DoUnlock()
  {
    this._shakeContainer.gameObject.SetActive(true);
    this._shakeContainer.localScale = Vector3.zero;
    this._icon.color = new Color(1f, 1f, 1f, 1f);
    this._flashIcon.gameObject.SetActive(true);
    this._flashIcon.color = new Color(1f, 1f, 1f, 1f);
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
      this._alert.gameObject.SetActive(false);
    UIManager.PlayAudio("event:/unlock_building/unlock");
    this._shakeContainer.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    DOTweenModuleUI.DOColor(this._flashIcon, new Color(1f, 1f, 1f, 0.0f), 0.25f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
    {
      Vector3 one = Vector3.one;
      this._alert.transform.localScale = Vector3.zero;
      this._alert.transform.DOScale(one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._alert.gameObject.SetActive(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
    }
    yield return (object) null;
  }

  [CompilerGenerated]
  public void \u003CAnimateIncognitoOut\u003Eb__41_0()
  {
    if (!this._unlocked)
    {
      this._costText.text = StructuresData.ItemCost.GetCostString(new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.TALISMAN, 1));
      if (this._sinCost != -1)
        this._costText.text = StructuresData.ItemCost.GetCostString(new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.PLEASURE_POINT, this._sinCost));
      if (this._cost.CanAfford())
        this._icon.color = new Color(1f, 1f, 1f, 1f);
      else
        this._icon.color = new Color(0.0f, 1f, 1f, 1f);
    }
    else
      this._costText.text = "";
  }
}
