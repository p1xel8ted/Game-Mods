// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIItemSelectorOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIItemSelectorOverlayController : UIMenuBase
{
  public Action<InventoryItem.ITEM_TYPE> OnItemChosen;
  public Func<InventoryItem.ITEM_TYPE, TraderTrackerItems> CostProvider;
  [Header("Item Selector")]
  [SerializeField]
  private RectTransform _itemSelectorRectTransform;
  [SerializeField]
  private CanvasGroup _itemSelectorCanvasGroup;
  [SerializeField]
  private RectTransform _itemContent;
  [SerializeField]
  private GameObject _buttonPrompt;
  [SerializeField]
  private TextMeshProUGUI _buttonPromptText;
  [SerializeField]
  private TextMeshProUGUI _coinsText;
  [SerializeField]
  private GameObject _coinsContainer;
  [SerializeField]
  private GameObject _backPrompt;
  [SerializeField]
  private GameObject _backText;
  [Header("Templates")]
  [SerializeField]
  private GenericInventoryItem _inventoryItemTemplate;
  private ItemSelector.Category _category;
  private ItemSelector.Params _params;
  private List<GenericInventoryItem> _inventoryItems = new List<GenericInventoryItem>();
  private string _contextString;
  private bool _didCancel;
  private List<InventoryItem> _customInventory;
  private string _addtionalText;

  private List<InventoryItem.ITEM_TYPE> _items => this._category.TrackedItems;

  private Vector2 _offset => this._params.Offset;

  private ItemSelector.Context _context => this._params.Context;

  private bool _hideOnSelection => this._params.HideOnSelection;

  private bool _showQuantity => !this._params.HideQuantity;

  private bool _showEmpty => this._params.ShowEmpty;

  private InventoryItem.ITEM_TYPE _mostRecentItem => this._category.MostRecentItem;

  private bool _preventCancellation => this._params.PreventCancellation;

  private bool _showCoins => this._params.ShowCoins;

  public override void Awake()
  {
    base.Awake();
    this._itemSelectorCanvasGroup.alpha = 0.0f;
  }

  public void Show(
    List<InventoryItem> inventoryItems,
    ItemSelector.Params parameters,
    bool instant = false)
  {
    this._customInventory = inventoryItems;
    List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem inventoryItem in this._customInventory)
      items.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
    this.Show(items, parameters, instant);
  }

  public void Show(
    List<InventoryItem.ITEM_TYPE> items,
    ItemSelector.Params parameters,
    bool instant = false)
  {
    this._params = parameters;
    this._category = DataManager.Instance.GetItemSelectorCategory(parameters.Key);
    foreach (InventoryItem.ITEM_TYPE itemType in items)
    {
      if (!this._category.TrackedItems.Contains(itemType) && (!parameters.RequiresDiscovery || this.GetItemQuantity(itemType) != 0))
        this._category.TrackedItems.Add(itemType);
    }
    this._contextString = LocalizationManager.GetTranslation($"UI/ItemSelector/Context/{this._context}");
    this.Show(instant);
    if (this._hideOnSelection)
      return;
    this.StartCoroutine((IEnumerator) this.WaitUntilRelease());
  }

  private IEnumerator WaitUntilRelease()
  {
    while (!InputManager.UI.GetAcceptButtonUp())
      yield return (object) null;
    MonoSingleton<UINavigatorNew>.Instance.AllowAcceptHold = true;
  }

  protected override void OnShowStarted()
  {
    this._itemSelectorRectTransform.localScale = (Vector3) (Vector2.one * 0.75f);
    this._itemSelectorRectTransform.anchoredPosition += this._offset;
    this._backPrompt.SetActive(!this._preventCancellation);
    this._backText.SetActive(!this._preventCancellation);
    if (this._items.Count > 0 && this._inventoryItems.Count == 0)
    {
      int index = 0;
      foreach (InventoryItem.ITEM_TYPE itemType in this._items)
      {
        if (this._showEmpty || this.GetItemQuantity(itemType) != 0)
        {
          GenericInventoryItem inventoryItemInstance = this._inventoryItemTemplate.Instantiate<GenericInventoryItem>((Transform) this._itemContent);
          if (this._customInventory != null)
            inventoryItemInstance.Configure(this._customInventory[this._items.IndexOf(itemType)], this._showQuantity);
          else
            inventoryItemInstance.Configure(itemType, this._showQuantity);
          inventoryItemInstance.Button.onClick.AddListener((UnityAction) (() => this.OnItemClicked(inventoryItemInstance)));
          inventoryItemInstance.Button.OnSelected += (System.Action) (() => this.OnItemSelected(inventoryItemInstance));
          this._inventoryItems.Add(inventoryItemInstance);
          if (itemType == this._category.MostRecentItem)
            index = this._inventoryItems.Count - 1;
        }
      }
      if (this._inventoryItems.Count > 0)
      {
        this.OverrideDefault((Selectable) this._inventoryItems[index].Button);
        this.ActivateNavigation();
      }
    }
    if (this._inventoryItems.Count != 0)
      return;
    this._buttonPromptText.text = "-";
    this._buttonPrompt.SetActive(false);
  }

  private int GetItemQuantity(InventoryItem.ITEM_TYPE itemType)
  {
    if (this._customInventory != null)
    {
      foreach (InventoryItem inventoryItem in this._customInventory)
      {
        if ((InventoryItem.ITEM_TYPE) inventoryItem.type == itemType)
          return inventoryItem.quantity;
      }
    }
    return Inventory.GetItemQuantity(itemType);
  }

  protected override IEnumerator DoShowAnimation()
  {
    this._itemSelectorRectTransform.DOScale((Vector3) (Vector2.one * 1.15f), 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._itemSelectorCanvasGroup.DOFade(1f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    this._itemSelectorRectTransform.DOScale((Vector3) Vector2.one, 0.125f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.125f);
  }

  protected override IEnumerator DoHideAnimation()
  {
    this._itemSelectorRectTransform.DOKill();
    this._itemSelectorCanvasGroup.DOKill();
    this._itemSelectorRectTransform.DOScale((Vector3) (Vector2.one * 1.15f), 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    this._itemSelectorRectTransform.DOScale((Vector3) (Vector2.one * 0.75f), 0.125f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._itemSelectorCanvasGroup.DOFade(0.0f, 0.1f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.125f);
  }

  private void OnItemClicked(GenericInventoryItem item)
  {
    if (this._context == ItemSelector.Context.SetLabel)
    {
      Choose();
    }
    else
    {
      if (this.GetItemQuantity(item.Type) > 0)
      {
        if (this._context == ItemSelector.Context.Buy)
        {
          Func<InventoryItem.ITEM_TYPE, TraderTrackerItems> costProvider = this.CostProvider;
          TraderTrackerItems traderTrackerItems = costProvider != null ? costProvider(item.Type) : (TraderTrackerItems) null;
          if (traderTrackerItems != null && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) >= traderTrackerItems.SellPriceActual)
          {
            Choose();
            return;
          }
        }
        else
        {
          Choose();
          return;
        }
      }
      item.Shake();
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback");
    }

    void Choose()
    {
      Action<InventoryItem.ITEM_TYPE> onItemChosen = this.OnItemChosen;
      if (onItemChosen != null)
        onItemChosen(item.Type);
      if (this._hideOnSelection)
        this.Hide();
      else
        this.UpdateQuantities();
    }
  }

  private void OnItemSelected(GenericInventoryItem item)
  {
    this._category.MostRecentItem = item.Type;
    this.RefreshContextText();
  }

  private void RefreshContextText()
  {
    if (this._context == ItemSelector.Context.Sell || this._context == ItemSelector.Context.Buy)
    {
      Func<InventoryItem.ITEM_TYPE, TraderTrackerItems> costProvider = this.CostProvider;
      TraderTrackerItems traderTrackerItems = costProvider != null ? costProvider(this._mostRecentItem) : (TraderTrackerItems) null;
      if (traderTrackerItems == null)
        return;
      if (this._context == ItemSelector.Context.Buy)
      {
        if (traderTrackerItems.SellOffset > 0)
          this._addtionalText = $" <color=red>+ {(object) Math.Round((double) ((float) traderTrackerItems.SellPrice / (float) traderTrackerItems.SellPriceActual * 100f), 0)}%</color> ";
        this._buttonPromptText.text = string.Format(this._contextString, (object) (InventoryItem.LocalizedName(this._mostRecentItem) ?? ""), (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, traderTrackerItems.SellPriceActual)) + this._addtionalText;
      }
      else
        this._buttonPromptText.text = string.Format(this._contextString, (object) InventoryItem.LocalizedName(this._mostRecentItem), (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, traderTrackerItems.BuyPriceActual, ignoreAffordability: true)) + this._addtionalText;
    }
    else
      this._buttonPromptText.text = string.Format(this._contextString, (object) InventoryItem.LocalizedName(this._mostRecentItem)) + this._addtionalText;
  }

  private void Update()
  {
    if (this._showCoins)
    {
      this._coinsContainer.SetActive(true);
      this._coinsText.text = "<sprite name=\"icon_blackgold\"> " + (object) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
    }
    else
    {
      this._coinsContainer.SetActive(false);
      this._coinsText.text = "";
    }
  }

  public void UpdateQuantities()
  {
    foreach (UIInventoryItem inventoryItem in this._inventoryItems)
      inventoryItem.UpdateQuantity();
    this.RefreshContextText();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this._preventCancellation)
      return;
    this.Hide();
    this._didCancel = true;
  }

  protected override void OnHideStarted()
  {
    MonoSingleton<UINavigatorNew>.Instance.AllowAcceptHold = false;
  }

  protected override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
