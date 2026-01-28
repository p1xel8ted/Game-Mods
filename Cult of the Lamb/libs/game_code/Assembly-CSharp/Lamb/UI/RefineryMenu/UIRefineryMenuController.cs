// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RefineryMenu.UIRefineryMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public RefineryInfoCardController _infoCardController;
  public RefineryItem refineryIconPrefab;
  public Transform Container;
  [SerializeField]
  public Transform queuedContainer;
  [SerializeField]
  public TMP_Text amountQueuedText;
  [SerializeField]
  public GameObject[] _vacantSlots;
  [SerializeField]
  public GameObject _addToQueuePrompt;
  [SerializeField]
  public GameObject _removeFromQueuePrompt;
  [SerializeField]
  public GameObject _queueFullPrompt;
  public List<RefineryItem> _refineryItems = new List<RefineryItem>();
  public List<RefineryItem> _queuedItems = new List<RefineryItem>();
  public Tween _queuedTextTween;
  public StructuresData _structureInfo;
  public Interaction_Refinery _interactionRefinery;
  public InventoryItem.ITEM_TYPE[] _refinableResources = new InventoryItem.ITEM_TYPE[9]
  {
    InventoryItem.ITEM_TYPE.LOG_REFINED,
    InventoryItem.ITEM_TYPE.STONE_REFINED,
    InventoryItem.ITEM_TYPE.BLACK_GOLD,
    InventoryItem.ITEM_TYPE.GOLD_REFINED,
    InventoryItem.ITEM_TYPE.SILK_THREAD,
    InventoryItem.ITEM_TYPE.YEW_HOLY,
    InventoryItem.ITEM_TYPE.MAGMA_STONE,
    InventoryItem.ITEM_TYPE.MAGMA_STONE,
    InventoryItem.ITEM_TYPE.FLOWER_RED
  };
  public InventoryItem.ITEM_TYPE[] _dlcResources = new InventoryItem.ITEM_TYPE[5]
  {
    InventoryItem.ITEM_TYPE.YEW_HOLY,
    InventoryItem.ITEM_TYPE.YEW_CURSED,
    InventoryItem.ITEM_TYPE.MAGMA_STONE,
    InventoryItem.ITEM_TYPE.LIGHTNING_SHARD,
    InventoryItem.ITEM_TYPE.FLOWER_RED
  };

  public int kMaxItems => this._structureInfo.Type != StructureBrain.TYPES.REFINERY ? 10 : 5;

  public void Show(
    StructuresData refineryData,
    Interaction_Refinery interactionRefinery,
    bool instant = false)
  {
    this._structureInfo = refineryData;
    this._interactionRefinery = interactionRefinery;
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    for (int index = 0; index < this._vacantSlots.Length; ++index)
      this._vacantSlots[index].SetActive(index < this.kMaxItems);
    Dictionary<InventoryItem.ITEM_TYPE, int> dictionary = new Dictionary<InventoryItem.ITEM_TYPE, int>();
    foreach (InventoryItem.ITEM_TYPE refinableResource in this._refinableResources)
    {
      if (!this._dlcResources.Contains<InventoryItem.ITEM_TYPE>(refinableResource) || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem) && DataManager.Instance.MAJOR_DLC)
      {
        if (dictionary.ContainsKey(refinableResource))
          dictionary[refinableResource]++;
        else
          dictionary.Add(refinableResource, 0);
        RefineryItem refineryItem = UnityEngine.Object.Instantiate<RefineryItem>(this.refineryIconPrefab, this.Container);
        refineryItem.OnItemSelected += new Action<RefineryItem>(this.OnItemSelected);
        refineryItem.Button.OnSelected += new System.Action(this.OnQueueableItemSelected);
        refineryItem.Configure(refinableResource, variant: dictionary[refinableResource]);
        refineryItem.FadeIn((float) this._refineryItems.Count * 0.03f);
        this._refineryItems.Add(refineryItem);
      }
    }
    this.OverrideDefault((Selectable) this._refineryItems[0].Button);
    this.ActivateNavigation();
    for (int index = 0; index < this._structureInfo.QueuedResources.Count; ++index)
    {
      if (this._structureInfo.QueuedRefineryVariants.Count <= index)
        this._structureInfo.QueuedRefineryVariants.Add(0);
      this.MakeQueuedItem(this._structureInfo.QueuedResources[index], this._structureInfo.QueuedRefineryVariants[index]);
    }
    this.UpdateQueueText();
    this.UpdateQuantities();
  }

  public void UpdateQueueText()
  {
    Color colour = this._queuedItems.Count > 0 ? StaticColors.GreenColor : StaticColors.RedColor;
    this.amountQueuedText.text = (LocalizeIntegration.FormatCurrentMax(this._queuedItems.Count.ToString(), this.kMaxItems.ToString()) ?? "").Colour(colour);
  }

  public void UpdateQuantities()
  {
    foreach (UIInventoryItem refineryItem in this._refineryItems)
      refineryItem.UpdateQuantity();
    foreach (UIInventoryItem queuedItem in this._queuedItems)
      queuedItem.UpdateQuantity();
  }

  public void OnItemSelected(RefineryItem item)
  {
    if (!item.CanAfford || this._structureInfo.QueuedResources.Count >= this.kMaxItems)
      return;
    if (this._structureInfo.QueuedResources.Count >= this.kMaxItems)
    {
      this.ShowMaxQueued();
      item.Shake();
    }
    else
      this.AddToQueue(item);
  }

  public void OnQueueableItemSelected()
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

  public void OnQueuedItemSelected()
  {
    this._addToQueuePrompt.SetActive(false);
    this._removeFromQueuePrompt.SetActive(true);
    this._queueFullPrompt.SetActive(false);
  }

  public void AddToQueue(RefineryItem item)
  {
    this._structureInfo.QueuedResources.Add(item.Type);
    if (this._structureInfo.QueuedRefineryVariants.Count > this._structureInfo.QueuedResources.Count)
      this._structureInfo.QueuedRefineryVariants[this._structureInfo.QueuedResources.Count - 1] = item.Variant;
    else
      this._structureInfo.QueuedRefineryVariants.Add(item.Variant);
    foreach (StructuresData.ItemCost itemCost in Structures_Refinery.GetCost(item.Type, item.Variant))
      Inventory.ChangeItemQuantity((int) itemCost.CostItem, -itemCost.CostValue);
    RefineryItem refineryItem1 = this.MakeQueuedItem(item.Type, item.Variant);
    Vector3 localScale = refineryItem1.RectTransform.localScale;
    refineryItem1.RectTransform.localScale = Vector3.one * 1.2f;
    refineryItem1.RectTransform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._infoCardController.CurrentCard.Configure(item);
    Action<InventoryItem.ITEM_TYPE> onItemQueued = this.OnItemQueued;
    if (onItemQueued != null)
      onItemQueued(item.Type);
    this.UpdateQueueText();
    this.UpdateQuantities();
    this.OnQueueableItemSelected();
    if (item.Type == InventoryItem.ITEM_TYPE.YEW_HOLY)
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.RefineChargedRostone);
    foreach (RefineryItem refineryItem2 in this._refineryItems)
      refineryItem2.Button.Confirmable = this._queuedItems.Count < this.kMaxItems && refineryItem2.CanAfford;
  }

  public RefineryItem MakeQueuedItem(InventoryItem.ITEM_TYPE resource, int variant)
  {
    RefineryItem refineryItem = UnityEngine.Object.Instantiate<RefineryItem>(this.refineryIconPrefab, this.queuedContainer);
    refineryItem.Configure(resource, true, this._queuedItems.Count, false, variant);
    refineryItem.UpdateRefiningProgress(this._structureInfo.Progress / ((Structures_Refinery) StructureBrain.GetOrCreateBrain(this._structureInfo)).RefineryDuration(refineryItem.Type));
    refineryItem.Button.OnSelected += new System.Action(this.OnQueuedItemSelected);
    refineryItem.OnItemSelected += new Action<RefineryItem>(this.RemoveFromQueue);
    this._vacantSlots[this._queuedItems.Count].SetActive(false);
    this._queuedItems.Add(refineryItem);
    refineryItem.transform.SetSiblingIndex(this._queuedItems.Count - 1);
    return refineryItem;
  }

  public void RemoveFromQueue(RefineryItem item)
  {
    if ((UnityEngine.Object) this._infoCardController.CurrentCard != (UnityEngine.Object) null)
      this._infoCardController.CurrentCard.Configure(item);
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
    this._structureInfo.QueuedRefineryVariants.RemoveAt(index);
    foreach (StructuresData.ItemCost itemCost in Structures_Refinery.GetCost(item.Type, item.Variant))
      Inventory.AddItem((int) itemCost.CostItem, itemCost.CostValue);
    this._queuedItems.Remove(item);
    this._vacantSlots[this._queuedItems.Count].SetActive(true);
    if (index == 0 && this._queuedItems.Count > 0)
    {
      this._queuedItems[0].Configure(this._queuedItems[0].Type, true, showQuantity: false, variant: this._queuedItems[0].Variant);
      this._structureInfo.Progress = 0.0f;
    }
    if (this._queuedItems.Count == 0 && (UnityEngine.Object) this._interactionRefinery != (UnityEngine.Object) null)
      this._interactionRefinery.OnCompleteRefining();
    UnityEngine.Object.Destroy((UnityEngine.Object) item.gameObject);
    this.UpdateQueueText();
    this.UpdateQuantities();
  }

  public void ShowMaxQueued()
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

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
