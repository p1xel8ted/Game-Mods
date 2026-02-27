// Decompiled with JetBrains decompiler
// Type: UpgradeShopItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RectTransform _container;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _canAffordIcon;
  [SerializeField]
  private Image _cantAffordIcon;
  [SerializeField]
  private Image _selectedIcon;
  [SerializeField]
  private TextMeshProUGUI _costText;
  [SerializeField]
  private UpgradeAlert _alert;
  [SerializeField]
  private Material _blackAndWhiteMaterial;
  private UpgradeSystem.Type _upgradeType;
  private Vector2 _anchoredOrigin;
  private bool _canAfford;
  private Material _blackAndWhiteMaterialInstance;
  private Coroutine cSelectionRoutine;

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

  private void OnButtonClicked()
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

  private IEnumerator FadeIn(float Delay)
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

  private bool CheckCanAfford(UpgradeSystem.Type type)
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

  private IEnumerator Selected(float Starting, float Target)
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

  private IEnumerator DeSelected()
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
