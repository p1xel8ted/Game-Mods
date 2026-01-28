// Decompiled with JetBrains decompiler
// Type: RefineryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class RefineryItem : UIInventoryItem, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  public Action<RefineryItem> OnItemSelected;
  [SerializeField]
  public GameObject _progressContainer;
  [SerializeField]
  public Image _progressRing;
  [SerializeField]
  public Image _canAffordIcon;
  [SerializeField]
  public Image _cantAffordIcon;
  [SerializeField]
  public Image _selectedIcon;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [SerializeField]
  public CanvasGroup _cancelCanvasGroup;
  public Vector2 _anchoredOrigin;
  public bool _canAfford;
  public bool _queued;
  public bool _firstQueuedResource;
  public int _variant;

  public bool CanAfford => this._canAfford;

  public int Variant => this._variant;

  public void Awake()
  {
    this._button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
    this._button.OnConfirmDenied += new System.Action(((UIInventoryItem) this).Shake);
  }

  public void Configure(
    InventoryItem.ITEM_TYPE type,
    bool queued = false,
    int queuedIndex = 0,
    bool showQuantity = true,
    int variant = 0)
  {
    this.Configure(type, showQuantity);
    this._queued = queued;
    this._variant = variant;
    this._anchoredOrigin = this._container.anchoredPosition;
    this._selectedIcon.enabled = false;
    this._cancelCanvasGroup.alpha = 0.0f;
    this._costText.gameObject.SetActive(!queued);
    this._costText.isRightToLeftText = LocalizeIntegration.IsArabic();
    this._costText.text = StructuresData.ItemCost.GetCostString(Structures_Refinery.GetCost(type, variant));
    if ((UnityEngine.Object) this._amountText != (UnityEngine.Object) null)
    {
      this._amountText.gameObject.SetActive(Structures_Refinery.GetAmount(type) > 1);
      this._amountText.text = "x" + Structures_Refinery.GetAmount(type).ToString();
    }
    this._firstQueuedResource = queuedIndex == 0;
    this._progressContainer.SetActive(queued && this._firstQueuedResource);
    if (!this._queued)
      this._button.Confirmable = this._canAfford;
    else
      this._button.Confirmable = true;
  }

  public override void UpdateQuantity()
  {
    this._canAfford = this.CheckCanAfford(this.Type) && !this._queued;
    this._costText.text = StructuresData.ItemCost.GetCostString(Structures_Refinery.GetCost(this.Type, this._variant));
    if (!this._queued)
    {
      this._button.Confirmable = this._canAfford;
      if (this._showQuantity)
      {
        this._icon.color = new Color(this._canAfford ? 1f : 0.0f, 1f, 1f, 1f);
        this._canAffordIcon.color = new Color(this._canAfford ? 1f : 0.0f, 1f, 1f, 1f);
      }
      this._canAffordIcon.gameObject.SetActive(this._canAfford);
      this._cantAffordIcon.gameObject.SetActive(!this._canAfford);
    }
    else
    {
      this._button.Confirmable = true;
      this._canAffordIcon.gameObject.SetActive(true);
      this._cantAffordIcon.gameObject.SetActive(false);
    }
  }

  public void OnButtonClicked()
  {
    Action<RefineryItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }

  public void UpdateRefiningProgress(float normTime) => this._progressRing.fillAmount = normTime;

  public bool CheckCanAfford(InventoryItem.ITEM_TYPE type)
  {
    if (CheatConsole.BuildingsFree)
      return true;
    bool flag = true;
    foreach (StructuresData.ItemCost itemCost in Structures_Refinery.GetCost(type, this._variant))
    {
      if (!itemCost.CanAfford())
        flag = false;
    }
    return flag;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this._selectedIcon.enabled = true;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Selected(this.transform.localScale.x, 1.2f));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this._selectedIcon.enabled = false;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DeSelected());
  }

  public IEnumerator Selected(float starting, float target)
  {
    RefineryItem refineryItem = this;
    refineryItem._canvasGroup.alpha = 1f;
    refineryItem.transform.localScale = Vector3.one;
    float progress = 0.0f;
    float duration = 0.1f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      float num = Mathf.SmoothStep(starting, target, progress / duration);
      if (refineryItem._queued)
        refineryItem._cancelCanvasGroup.alpha = progress / duration;
      refineryItem.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = target;
    refineryItem.transform.localScale = Vector3.one * num1;
    if (refineryItem._queued)
      refineryItem._cancelCanvasGroup.alpha = 1f;
  }

  public IEnumerator DeSelected()
  {
    RefineryItem refineryItem = this;
    float progress = 0.0f;
    float duration = 0.3f;
    float startingScale = refineryItem.transform.localScale.x;
    float targetScale = 1f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      float num = Mathf.SmoothStep(startingScale, targetScale, progress / duration);
      refineryItem.transform.localScale = Vector3.one * num;
      if (refineryItem._queued)
        refineryItem._cancelCanvasGroup.alpha = (float) (1.0 - (double) progress / (double) duration);
      yield return (object) null;
    }
    float num1 = targetScale;
    refineryItem.transform.localScale = Vector3.one * num1;
    if (refineryItem._queued)
      refineryItem._cancelCanvasGroup.alpha = 0.0f;
  }
}
