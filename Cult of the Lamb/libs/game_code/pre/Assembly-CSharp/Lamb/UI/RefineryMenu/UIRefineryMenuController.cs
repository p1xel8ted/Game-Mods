// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RefineryMenu.UIRefineryMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.RefineryMenu;

public class UIRefineryMenuController : UIMenuBase
{
  public Action<InventoryItem.ITEM_TYPE> OnItemQueued;
  [SerializeField]
  private RefineryInfoCardController _infoCardController;
  public RefineryItem refineryIconPrefab;
  public Transform Container;
  [SerializeField]
  private Transform queuedContainer;
  [SerializeField]
  private TMP_Text amountQueuedText;
  [SerializeField]
  private GameObject[] _vacantSlots;
  [SerializeField]
  private GameObject _addToQueuePrompt;
  [SerializeField]
  private GameObject _removeFromQueuePrompt;
  [SerializeField]
  private GameObject _queueFullPrompt;
  private List<RefineryItem> _refineryItems = new List<RefineryItem>();
  private List<RefineryItem> _queuedItems = new List<RefineryItem>();
  private Tween _queuedTextTween;
  private StructuresData _structureInfo;
  private Interaction_Refinery _interactionRefinery;
  private InventoryItem.ITEM_TYPE[] _refinableResources = new InventoryItem.ITEM_TYPE[4]
  {
    InventoryItem.ITEM_TYPE.LOG_REFINED,
    InventoryItem.ITEM_TYPE.STONE_REFINED,
    InventoryItem.ITEM_TYPE.BLACK_GOLD,
    InventoryItem.ITEM_TYPE.GOLD_REFINED
  };

  private int kMaxItems => this._structureInfo.Type != StructureBrain.TYPES.REFINERY ? 10 : 5;

  public void Show(
    StructuresData refineryData,
    Interaction_Refinery interactionRefinery,
    bool instant = false)
  {
    this._structureInfo = refineryData;
    this._interactionRefinery = interactionRefinery;
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    for (int index = 0; index < this._vacantSlots.Length; ++index)
      this._vacantSlots[index].SetActive(index < this.kMaxItems);
    foreach (InventoryItem.ITEM_TYPE refinableResource in this._refinableResources)
    {
      RefineryItem refineryItem = UnityEngine.Object.Instantiate<RefineryItem>(this.refineryIconPrefab, this.Container);
      refineryItem.OnItemSelected += new Action<RefineryItem>(this.OnItemSelected);
      refineryItem.Button.OnSelected += new System.Action(this.OnQueueableItemSelected);
      refineryItem.Configure(refinableResource, false, 0, true);
      refineryItem.FadeIn((float) this._refineryItems.Count * 0.03f);
      this._refineryItems.Add(refineryItem);
    }
    this.OverrideDefault((Selectable) this._refineryItems[0].Button);
    this.ActivateNavigation();
    for (int index = 0; index < this._structureInfo.QueuedResources.Count; ++index)
      this.MakeQueuedItem(this._structureInfo.QueuedResources[index]);
    this.UpdateQueueText();
    this.UpdateQuantities();
  }

  private void UpdateQueueText()
  {
    this.amountQueuedText.text = $"{this._queuedItems.Count}/{this.kMaxItems}".Colour(this._queuedItems.Count > 0 ? StaticColors.GreenColor : StaticColors.RedColor);
  }

  private void UpdateQuantities()
  {
    foreach (UIInventoryItem refineryItem in this._refineryItems)
      refineryItem.UpdateQuantity();
    foreach (UIInventoryItem queuedItem in this._queuedItems)
      queuedItem.UpdateQuantity();
  }

  private void OnItemSelected(RefineryItem item)
  {
    if (!item.CanAfford || this._structureInfo.QueuedResources.Count >= this.kMaxItems)
      return;
    if (this._structureInfo.QueuedResources.Count >= this.kMaxItems)
    {
      this.ShowMaxQueued();
      item.Shake();
    }
    else
      this.AddToQueue(item.Type);
  }

  private void OnQueueableItemSelected()
  {
    if (this._structureInfo.QueuedResources.Count >= this.kMaxItems)
    {
      this._addToQueuePrompt.SetActive(false);
      this._removeFromQueuePrompt.SetActive(false);
      this._queueFullPrompt.SetActive(true);
    }
    else
    {
      this._addToQueuePrompt.SetActive(true);
      this._removeFromQueuePrompt.SetActive(false);
      this._queueFullPrompt.SetActive(false);
    }
  }

  private void OnQueuedItemSelected()
  {
    this._addToQueuePrompt.SetActive(false);
    this._removeFromQueuePrompt.SetActive(true);
    this._queueFullPrompt.SetActive(false);
  }

  private void AddToQueue(InventoryItem.ITEM_TYPE resource)
  {
    this._structureInfo.QueuedResources.Add(resource);
    foreach (StructuresData.ItemCost itemCost in Structures_Refinery.GetCost(resource))
      Inventory.ChangeItemQuantity((int) itemCost.CostItem, -itemCost.CostValue);
    RefineryItem refineryItem1 = this.MakeQueuedItem(resource);
    Vector3 localScale = refineryItem1.RectTransform.localScale;
    refineryItem1.RectTransform.localScale = Vector3.one * 1.2f;
    refineryItem1.RectTransform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._infoCardController.CurrentCard.Configure(resource);
    Action<InventoryItem.ITEM_TYPE> onItemQueued = this.OnItemQueued;
    if (onItemQueued != null)
      onItemQueued(resource);
    this.UpdateQueueText();
    this.UpdateQuantities();
    this.OnQueueableItemSelected();
    foreach (RefineryItem refineryItem2 in this._refineryItems)
      refineryItem2.Button.Confirmable = this._queuedItems.Count < this.kMaxItems && refineryItem2.CanAfford;
  }

  private RefineryItem MakeQueuedItem(InventoryItem.ITEM_TYPE resource)
  {
    RefineryItem refineryItem = UnityEngine.Object.Instantiate<RefineryItem>(this.refineryIconPrefab, this.queuedContainer);
    refineryItem.Configure(resource, true, this._queuedItems.Count, false);
    refineryItem.UpdateRefiningProgress(this._structureInfo.Progress / ((Structures_Refinery) StructureBrain.GetOrCreateBrain(this._structureInfo)).RefineryDuration(refineryItem.Type));
    refineryItem.Button.OnSelected += new System.Action(this.OnQueuedItemSelected);
    refineryItem.OnItemSelected += new Action<RefineryItem>(this.RemoveFromQueue);
    this._vacantSlots[this._queuedItems.Count].SetActive(false);
    this._queuedItems.Add(refineryItem);
    refineryItem.transform.SetSiblingIndex(this._queuedItems.Count - 1);
    return refineryItem;
  }

  private void RemoveFromQueue(RefineryItem item)
  {
    if ((UnityEngine.Object) this._infoCardController.CurrentCard != (UnityEngine.Object) null)
      this._infoCardController.CurrentCard.Configure(item.Type);
    IMMSelectable selectableOnRight = item.Button.FindSelectableOnRight() as IMMSelectable;
    IMMSelectable selectableOnLeft = item.Button.FindSelectableOnLeft() as IMMSelectable;
    if (this._queuedItems.IndexOf(item) < this._queuedItems.Count - 1 && selectableOnRight != null)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(selectableOnRight);
    else if (this._queuedItems.IndexOf(item) > 0 && selectableOnLeft != null)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(selectableOnLeft);
    else if (this._queuedItems.Count - 1 > 0)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._queuedItems[this._queuedItems.Count - 2].Button);
    else
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._refineryItems[0].Button);
    int index = this._queuedItems.IndexOf(item);
    this._structureInfo.QueuedResources.RemoveAt(index);
    foreach (StructuresData.ItemCost itemCost in Structures_Refinery.GetCost(item.Type))
      Inventory.AddItem((int) itemCost.CostItem, itemCost.CostValue);
    this._queuedItems.Remove(item);
    this._vacantSlots[this._queuedItems.Count].SetActive(true);
    if (index == 0 && this._queuedItems.Count > 0)
    {
      this._queuedItems[0].Configure(this._queuedItems[0].Type, true, showQuantity: false);
      this._structureInfo.Progress = 0.0f;
    }
    if (this._queuedItems.Count == 0 && (UnityEngine.Object) this._interactionRefinery != (UnityEngine.Object) null)
      this._interactionRefinery.OnCompleteRefining();
    UnityEngine.Object.Destroy((UnityEngine.Object) item.gameObject);
    this.UpdateQueueText();
    this.UpdateQuantities();
  }

  private void ShowMaxQueued()
  {
    if (this._queuedTextTween != null && !this._queuedTextTween.IsComplete())
    {
      this._queuedTextTween.Complete();
      this.amountQueuedText.transform.localScale = Vector3.one;
    }
    this._queuedTextTween = (Tween) this.amountQueuedText.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
