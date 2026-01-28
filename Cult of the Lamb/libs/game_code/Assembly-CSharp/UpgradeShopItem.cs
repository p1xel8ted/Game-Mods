// Decompiled with JetBrains decompiler
// Type: UpgradeShopItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI.Alerts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UpgradeShopItem : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public Action<UpgradeSystem.Type> OnUpgradeSelected;
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public Image _canAffordIcon;
  [SerializeField]
  public Image _cantAffordIcon;
  [SerializeField]
  public Image _selectedIcon;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [SerializeField]
  public UpgradeAlert _alert;
  [SerializeField]
  public Material _blackAndWhiteMaterial;
  public UpgradeSystem.Type _upgradeType;
  public Vector2 _anchoredOrigin;
  public bool _canAfford;
  public Material _blackAndWhiteMaterialInstance;
  public Coroutine cSelectionRoutine;

  public MMButton Button => this._button;

  public UpgradeSystem.Type Type => this._upgradeType;

  public void Configure(UpgradeSystem.Type type, float delay)
  {
    this._upgradeType = type;
    this._anchoredOrigin = this._container.anchoredPosition;
    this._icon.sprite = UpgradeSystem.GetIcon(type);
    this._costText.text = StructuresData.ItemCost.GetCostString(UpgradeSystem.GetCost(type));
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._alert.Configure(type);
    this._selectedIcon.enabled = false;
    this._canAfford = this.CheckCanAfford(type);
    this._canAffordIcon.gameObject.SetActive(this._canAfford);
    this._cantAffordIcon.gameObject.SetActive(!this._canAfford);
    if (!this._canAfford)
    {
      this._blackAndWhiteMaterialInstance = new Material(this._blackAndWhiteMaterial);
      this._blackAndWhiteMaterialInstance.SetFloat("_GrayscaleLerpFade", 1f);
      this._icon.material = this._blackAndWhiteMaterialInstance;
      this._cantAffordIcon.material = this._blackAndWhiteMaterialInstance;
    }
    this.StartCoroutine((IEnumerator) this.FadeIn(delay));
  }

  public void OnButtonClicked()
  {
    if (this._canAfford)
    {
      Action<UpgradeSystem.Type> onUpgradeSelected = this.OnUpgradeSelected;
      if (onUpgradeSelected == null)
        return;
      onUpgradeSelected(this._upgradeType);
    }
    else
    {
      this._container.transform.DOKill();
      this._container.anchoredPosition = this._anchoredOrigin;
      this._container.transform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
    }
  }

  public IEnumerator FadeIn(float Delay)
  {
    this._canvasGroup.alpha = 0.0f;
    yield return (object) new WaitForSecondsRealtime(Delay);
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this._canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    this._canvasGroup.alpha = 1f;
  }

  public bool CheckCanAfford(UpgradeSystem.Type type)
  {
    if (CheatConsole.BuildingsFree)
      return true;
    List<StructuresData.ItemCost> cost = UpgradeSystem.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      if (Inventory.GetItemQuantity((int) cost[index].CostItem) < cost[index].CostValue)
        return false;
    }
    return true;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this._alert.TryRemoveAlert();
    this._selectedIcon.enabled = true;
    if (this.cSelectionRoutine != null)
      this.StopCoroutine(this.cSelectionRoutine);
    this.cSelectionRoutine = this.StartCoroutine((IEnumerator) this.Selected(this.transform.localScale.x, 1.2f));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this._selectedIcon.enabled = false;
    if (this.cSelectionRoutine != null)
      this.StopCoroutine(this.cSelectionRoutine);
    this.cSelectionRoutine = this.StartCoroutine((IEnumerator) this.DeSelected());
  }

  public IEnumerator Selected(float Starting, float Target)
  {
    UpgradeShopItem upgradeShopItem = this;
    float Progress = 0.0f;
    float Duration = 0.1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      float num = Mathf.SmoothStep(Starting, Target, Progress / Duration);
      upgradeShopItem.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = Target;
    upgradeShopItem.transform.localScale = Vector3.one * num1;
  }

  public IEnumerator DeSelected()
  {
    UpgradeShopItem upgradeShopItem = this;
    float Progress = 0.0f;
    float Duration = 0.3f;
    float StartingScale = upgradeShopItem.transform.localScale.x;
    float TargetScale = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      float num = Mathf.SmoothStep(StartingScale, TargetScale, Progress / Duration);
      upgradeShopItem.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = TargetScale;
    upgradeShopItem.transform.localScale = Vector3.one * num1;
  }
}
