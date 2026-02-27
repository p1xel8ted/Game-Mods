// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIItemSelectorOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMTools;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIItemSelectorOverlayController : UIMenuBase
{
  public static List<UIItemSelectorOverlayController> SelectorOverlays = new List<UIItemSelectorOverlayController>();
  public Action<InventoryItem.ITEM_TYPE> OnItemChosen;
  public GameObject ItemSelectorContainer;
  public RectTransform BackgroundRectTransform;
  public CoopIndicatorIcon BottomCoopIndicator;
  public GameObject PromptContainer;
  public Func<InventoryItem.ITEM_TYPE, TraderTrackerItems> CostProvider;
  [Header("Item Selector")]
  [SerializeField]
  public RectTransform _itemSelectorRectTransform;
  [SerializeField]
  public CanvasGroup _itemSelectorCanvasGroup;
  [SerializeField]
  public RectTransform _itemContent;
  [SerializeField]
  public GameObject _buttonPrompt;
  [SerializeField]
  public TextMeshProUGUI _buttonPromptText;
  [SerializeField]
  public TextMeshProUGUI _coinsText;
  [SerializeField]
  public GameObject _coinsContainer;
  [SerializeField]
  public GameObject _backPrompt;
  [SerializeField]
  public GameObject _backText;
  [SerializeField]
  public CoopIndicatorIcon selectorCoopIndicator;
  [Header("Templates")]
  [SerializeField]
  public GenericInventoryItem _inventoryItemTemplate;
  public ItemSelector.Category _category;
  public ItemSelector.Params _params;
  public List<GenericInventoryItem> _inventoryItems = new List<GenericInventoryItem>();
  public string _contextString;
  public bool _didCancel;
  public List<InventoryItem> _customInventory;
  public bool _inventoryPopulated;
  [HideInInspector]
  public PlayerFarming playerFarming;
  public FollowerLocation AwakeLocation;
  [SerializeField]
  public RectTransform canvasRect;
  [SerializeField]
  public Canvas canvas;
  public Vector3[] edgeVector = new Vector3[4];
  public Transform CustomTarget;
  public string _addtionalText;
  public string _progressString = "";

  public override bool _addToActiveMenus => false;

  public ItemSelector.Context Context => this._context;

  public List<InventoryItem.ITEM_TYPE> _items => this._category.TrackedItems;

  public Vector2 _offset => this._params.Offset;

  public ItemSelector.Context _context => this._params.Context;

  public bool _hideOnSelection => this._params.HideOnSelection;

  public bool _showQuantity => !this._params.HideQuantity;

  public bool _showEmpty => this._params.ShowEmpty;

  public InventoryItem.ITEM_TYPE _mostRecentItem => this._category.MostRecentItem;

  public bool _preventCancellation => this._params.PreventCancellation;

  public bool _showCoins => this._params.ShowCoins;

  public bool _showProgressText => this._params.ShowProgressText;

  public bool _disableForceClose => this._params.DisableForceClose;

  public bool _dontCache => this._params.DontCache;

  public static void CloseAllOverLays()
  {
    foreach (UIItemSelectorOverlayController selectorOverlay in UIItemSelectorOverlayController.SelectorOverlays)
    {
      if (selectorOverlay.IsHiding)
        break;
      selectorOverlay.Hide();
    }
  }

  public static bool CanRegainControl(PlayerFarming playerFarming)
  {
    foreach (UIItemSelectorOverlayController selectorOverlay in UIItemSelectorOverlayController.SelectorOverlays)
    {
      if (!((UnityEngine.Object) selectorOverlay.playerFarming != (UnityEngine.Object) playerFarming) && !selectorOverlay.IsHiding)
        return false;
    }
    return true;
  }

  public override void Awake()
  {
    base.Awake();
    this.AwakeLocation = LocationManager.GetLocation();
    this._itemSelectorCanvasGroup.alpha = 0.0f;
    if (UIItemSelectorOverlayController.SelectorOverlays.Count > 0)
      UIItemSelectorOverlayController.CloseAllOverLays();
    if (UIItemSelectorOverlayController.SelectorOverlays.Contains(this))
      return;
    UIItemSelectorOverlayController.SelectorOverlays.Add(this);
  }

  public override void OnDestroy()
  {
    if (UIItemSelectorOverlayController.SelectorOverlays.Contains(this))
      UIItemSelectorOverlayController.SelectorOverlays.Remove(this);
    this.UpdateSelectorLocations();
    base.OnDestroy();
  }

  public void OnPlayerLocationSetHide() => this.Hide();

  public void UpdateSelectorLocations()
  {
    if (LocationManager.GetLocation() != this.AwakeLocation)
    {
      this.Hide(true);
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.canvas)
        return;
      Camera worldCamera = this.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? (Camera) null : this.canvas.worldCamera;
      foreach (UIItemSelectorOverlayController selectorOverlay in UIItemSelectorOverlayController.SelectorOverlays)
      {
        if (!(bool) (UnityEngine.Object) selectorOverlay.playerFarming)
        {
          RectTransform selectorRectTransform = this._itemSelectorRectTransform;
          if ((bool) (UnityEngine.Object) selectorRectTransform)
            selectorRectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
          Vector3 screenPoint = Camera.main.WorldToScreenPoint(((UnityEngine.Object) selectorOverlay.CustomTarget != (UnityEngine.Object) null ? selectorOverlay.CustomTarget.transform : selectorOverlay.playerFarming.CameraBone.transform).position);
          RectTransform canvasRect = this.canvasRect;
          RectTransform selectorRectTransform = this._itemSelectorRectTransform;
          if ((bool) (UnityEngine.Object) canvasRect && (bool) (UnityEngine.Object) selectorRectTransform)
          {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, (Vector2) screenPoint, worldCamera, out localPoint);
            Vector2 vector2 = localPoint + new Vector2(0.0f, 200f);
            Rect rect = selectorRectTransform.rect;
            Vector2 size = rect.size;
            Vector2 pivot = selectorRectTransform.pivot;
            rect = canvasRect.rect;
            float min1 = rect.xMin + size.x * pivot.x;
            rect = canvasRect.rect;
            float max1 = rect.xMax - size.x * (1f - pivot.x);
            rect = canvasRect.rect;
            float min2 = rect.yMin + size.y * pivot.y;
            rect = canvasRect.rect;
            float max2 = rect.yMax - size.y * (1f - pivot.y);
            vector2.x = Mathf.Clamp(vector2.x, min1, max1);
            vector2.y = Mathf.Clamp(vector2.y, min2, max2);
            selectorRectTransform.anchoredPosition = vector2;
          }
        }
      }
    }
  }

  public void Show(
    PlayerFarming playerFarming,
    List<InventoryItem> inventoryItems,
    ItemSelector.Params parameters,
    bool instant = false)
  {
    this.playerFarming = playerFarming;
    this._customInventory = inventoryItems;
    List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem inventoryItem in this._customInventory)
      items.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
    this.Show(playerFarming, items, parameters, instant);
  }

  public void Show(
    Transform CustomTarget,
    PlayerFarming playerFarming,
    List<InventoryItem> inventoryItems,
    ItemSelector.Params parameters,
    bool instant = false)
  {
    this.playerFarming = playerFarming;
    this._customInventory = inventoryItems;
    List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem inventoryItem in this._customInventory)
      items.Add((InventoryItem.ITEM_TYPE) inventoryItem.type);
    this.Show(playerFarming, items, parameters, instant);
    this.CustomTarget = CustomTarget;
  }

  public void Show(
    PlayerFarming playerFarming,
    List<InventoryItem.ITEM_TYPE> items,
    ItemSelector.Params parameters,
    bool instant = false,
    bool dontCache = false)
  {
    this.playerFarming = playerFarming;
    this.CustomTarget = (Transform) null;
    this._params = parameters;
    this._category = DataManager.Instance.GetItemSelectorCategory(parameters.Key);
    if (this._dontCache)
      this._category.TrackedItems.Clear();
    foreach (InventoryItem.ITEM_TYPE itemType in items)
    {
      if (!this._category.TrackedItems.Contains(itemType) && (!parameters.RequiresDiscovery || this.GetItemQuantity(itemType) != 0))
        this._category.TrackedItems.Add(itemType);
    }
    this._contextString = LocalizationManager.GetTranslation($"UI/ItemSelector/Context/{this._context}");
    this.Show(instant);
    if (playerFarming.isLamb)
      this.selectorCoopIndicator.SetIcon(CoopIndicatorIcon.CoopIcon.Lamb);
    else
      this.selectorCoopIndicator.SetIcon(CoopIndicatorIcon.CoopIcon.Goat);
    this.selectorCoopIndicator.gameObject.SetActive(false);
    this.UpdateSelectorLocations();
    if (this._hideOnSelection)
      return;
    this.StartCoroutine(this.WaitUntilRelease());
  }

  public IEnumerator WaitUntilRelease()
  {
    PlayerFarming inputPlayerFarming = this.playerFarming;
    if ((UnityEngine.Object) inputPlayerFarming == (UnityEngine.Object) null)
      inputPlayerFarming = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    while (!InputManager.UI.GetAcceptButtonUp(inputPlayerFarming))
      yield return (object) null;
    MonoSingleton<UINavigatorNew>.Instance.AllowAcceptHold = true;
  }

  public override void OnShowStarted()
  {
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
          if (!this.playerFarming.rewiredPlayer.controllers.hasMouse)
            inventoryItemInstance.Button.PreventMouseSelection = true;
          inventoryItemInstance.Button.onClick.AddListener((UnityAction) (() => this.OnItemClicked(inventoryItemInstance)));
          inventoryItemInstance.Button.OnSelected += (System.Action) (() => this.OnItemSelected(inventoryItemInstance));
          this._inventoryItems.Add(inventoryItemInstance);
          inventoryItemInstance.GetComponent<MMButton>().playerFarming = this.playerFarming;
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
    this._inventoryPopulated = true;
    if (this._inventoryItems.Count == 0)
    {
      this._buttonPromptText.text = "-";
      this._buttonPrompt.SetActive(false);
    }
    this.BottomCoopIndicator.SetIcon(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.isLamb ? CoopIndicatorIcon.CoopIcon.Lamb : CoopIndicatorIcon.CoopIcon.Goat);
    if (CoopManager.CoopActive)
    {
      this.BottomCoopIndicator.gameObject.SetActive(true);
      int num = 90;
      if (this.playerFarming.isLamb)
        num = 0;
      this.PromptContainer.transform.position = new Vector3(this.PromptContainer.transform.position.x, this.PromptContainer.transform.position.y + (float) num, 0.0f);
    }
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
  }

  public int GetItemQuantity(InventoryItem.ITEM_TYPE itemType)
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

  public void SetItemQuantity(InventoryItem.ITEM_TYPE itemType, int Delta)
  {
    if (this._customInventory != null)
    {
      foreach (InventoryItem inventoryItem in this._customInventory)
      {
        if ((InventoryItem.ITEM_TYPE) inventoryItem.type == itemType)
          inventoryItem.quantity += Delta;
      }
    }
    this.UpdateQuantities();
  }

  public override IEnumerator DoShowAnimation()
  {
    this._itemSelectorRectTransform.DOScale((Vector3) (Vector2.one * 1.15f), 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._itemSelectorCanvasGroup.DOFade(1f, 0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    this._itemSelectorRectTransform.DOScale((Vector3) Vector2.one, 0.125f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.125f);
  }

  public override IEnumerator DoHideAnimation()
  {
    this._itemSelectorRectTransform.DOKill();
    this._itemSelectorCanvasGroup.DOKill();
    this._itemSelectorRectTransform.DOScale((Vector3) (Vector2.one * 1.15f), 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.1f);
    this._itemSelectorRectTransform.DOScale((Vector3) (Vector2.one * 0.75f), 0.125f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._itemSelectorCanvasGroup.DOFade(0.0f, 0.1f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.125f);
  }

  public void OnItemClicked(GenericInventoryItem item)
  {
    // ISSUE: variable of a compiler-generated type
    UIItemSelectorOverlayController.\u003C\u003Ec__DisplayClass74_0 cDisplayClass740;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass740.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass740.item = item;
    if (this._context == ItemSelector.Context.SetLabel)
    {
      this.\u003COnItemClicked\u003Eg__Choose\u007C74_0(ref cDisplayClass740);
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      if (this.GetItemQuantity(cDisplayClass740.item.Type) > 0)
      {
        if (this._context == ItemSelector.Context.Buy)
        {
          Func<InventoryItem.ITEM_TYPE, TraderTrackerItems> costProvider = this.CostProvider;
          // ISSUE: reference to a compiler-generated field
          TraderTrackerItems traderTrackerItems = costProvider != null ? costProvider(cDisplayClass740.item.Type) : (TraderTrackerItems) null;
          if (traderTrackerItems != null && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) >= traderTrackerItems.SellPriceActual)
          {
            this.\u003COnItemClicked\u003Eg__Choose\u007C74_0(ref cDisplayClass740);
            return;
          }
        }
        else
        {
          this.\u003COnItemClicked\u003Eg__Choose\u007C74_0(ref cDisplayClass740);
          return;
        }
      }
      // ISSUE: reference to a compiler-generated field
      cDisplayClass740.item.Shake();
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback");
    }
  }

  public void OnItemSelected(GenericInventoryItem item)
  {
    this._category.MostRecentItem = item.Type;
    this.RefreshContextText();
  }

  public void RefreshContextText()
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
          this._addtionalText = $" <color=red>+ {Math.Round((double) ((float) traderTrackerItems.SellPrice / (float) traderTrackerItems.SellPriceActual * 100f), 0).ToString()}%</color> ";
        this._buttonPromptText.text = string.Format(this._contextString, (object) (InventoryItem.LocalizedName(this._mostRecentItem) ?? ""), (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, traderTrackerItems.SellPriceActual)) + this._addtionalText;
      }
      else
        this._buttonPromptText.text = string.Format(this._contextString, (object) InventoryItem.LocalizedName(this._mostRecentItem), (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, traderTrackerItems.BuyPriceActual, ignoreAffordability: true)) + this._addtionalText;
    }
    else
      this._buttonPromptText.text = string.Format(this._contextString, (object) InventoryItem.LocalizedName(this._mostRecentItem)) + this._addtionalText;
  }

  public void setProgressString(string value) => this._progressString = value;

  public void Update()
  {
    if (this._showProgressText && this._progressString != "")
    {
      this._coinsContainer.SetActive(true);
      this._coinsText.text = this._progressString;
    }
    else if (this._showCoins)
    {
      this._coinsContainer.SetActive(true);
      this._coinsText.text = "<sprite name=\"icon_blackgold\"> " + Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD).ToString();
    }
    else
    {
      this._coinsContainer.SetActive(false);
      this._coinsText.text = "";
    }
    this.UpdateSelectorLocations();
    if (this._disableForceClose || this.IsHiding || !LetterBox.IsPlaying && !MMConversation.isPlaying && !((UnityEngine.Object) PlacementObject.Instance != (UnityEngine.Object) null) && !MonoSingleton<UIManager>.Instance.MenusBlocked)
      return;
    this.Hide();
  }

  public void UpdateQuantities()
  {
    foreach (UIInventoryItem inventoryItem in this._inventoryItems)
      inventoryItem.UpdateQuantity();
    this.RefreshContextText();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || this._preventCancellation || !this._inventoryPopulated)
      return;
    this.playerFarming.DodgeDelay = 0.25f;
    this.Hide();
    this._didCancel = true;
  }

  public override void OnHideStarted()
  {
    MonoSingleton<UINavigatorNew>.Instance.AllowAcceptHold = false;
    if ((UnityEngine.Object) PlacementObject.Instance != (UnityEngine.Object) null)
      this.SetTimeScaleOnHidden = false;
    if (LetterBox.IsPlaying || MMConversation.isPlaying)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
        player.state.CURRENT_STATE = StateMachine.State.InActive;
    }
    else
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.state.CURRENT_STATE == StateMachine.State.InActive)
        {
          bool flag = false;
          foreach (UIItemSelectorOverlayController selectorOverlay in UIItemSelectorOverlayController.SelectorOverlays)
          {
            if ((UnityEngine.Object) selectorOverlay != (UnityEngine.Object) this && (UnityEngine.Object) selectorOverlay.playerFarming == (UnityEngine.Object) player && (UnityEngine.Object) selectorOverlay.playerFarming == (UnityEngine.Object) this.playerFarming)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            player.state.CURRENT_STATE = StateMachine.State.Idle;
        }
      }
    }
  }

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003COnItemClicked\u003Eg__Choose\u007C74_0(
    [In] ref UIItemSelectorOverlayController.\u003C\u003Ec__DisplayClass74_0 obj0)
  {
    Action<InventoryItem.ITEM_TYPE> onItemChosen = this.OnItemChosen;
    if (onItemChosen != null)
    {
      // ISSUE: reference to a compiler-generated field
      onItemChosen(obj0.item.Type);
    }
    if (this._hideOnSelection)
      this.Hide();
    else
      this.UpdateQuantities();
  }
}
