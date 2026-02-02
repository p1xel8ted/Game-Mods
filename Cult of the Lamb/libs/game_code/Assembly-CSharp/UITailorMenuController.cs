// Decompiled with JetBrains decompiler
// Type: UITailorMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using Lamb.UI.Menus.PlayerMenu;
using src.UI;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UITailorMenuController : UIMenuBase
{
  public System.Action OnConfirm;
  [Header("Kitchen Menu")]
  public TailorMenuConfigure _recipesMenu;
  public TailorMenuConfigure _specialRecipesMenu;
  public TailorMenuConfigure _winterRecipesMenu;
  public TailorMenuConfigure _DLCRecipesMenu;
  public TailorQueue _tailorQueue;
  [SerializeField]
  public MMButton _cookButton;
  [SerializeField]
  public RectTransform _cookButtonRectTransform;
  [SerializeField]
  public ButtonHighlightController _highlightController;
  public TailorInfoCardController _infoCardController;
  [SerializeField]
  public TMP_Text totalText;
  [SerializeField]
  public UIPauseDetailsMenuTabNavigatorBase tabNavigator;
  [SerializeField]
  public MMScrollRect _scrollView;
  [SerializeField]
  public TextMeshProUGUI unlockedText;
  [SerializeField]
  public TextMeshProUGUI unlockedUniqueText;
  [SerializeField]
  public TextMeshProUGUI unlockedWinterText;
  [SerializeField]
  public TextMeshProUGUI unlockedDLCText;
  [Header("Prompts")]
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public HorizontalLayoutGroup horGroup;
  [SerializeField]
  public GameObject addToQueueButton;
  [SerializeField]
  public GameObject removeToQueueButton;
  [SerializeField]
  public GameObject openButton;
  [SerializeField]
  public GameObject selectButton;
  public Structures_Tailor tailor;
  public bool _didConfirm;
  [CompilerGenerated]
  public bool \u003CCancellable\u003Ek__BackingField = true;
  public TailorItem.InMenu _inMenu = TailorItem.InMenu.Craft;

  public bool Cancellable
  {
    get => this.\u003CCancellable\u003Ek__BackingField;
    set => this.\u003CCancellable\u003Ek__BackingField = value;
  }

  public virtual void Show(Structures_Tailor tailor, bool instant = false)
  {
    this.tailor = tailor;
    this.openButton.gameObject.SetActive(false);
    this.selectButton.gameObject.SetActive(false);
    this.removeToQueueButton.gameObject.SetActive(false);
    this.addToQueueButton.gameObject.SetActive(true);
    this._controlPrompts.ForceRebuild();
    this.totalText.gameObject.SetActive(false);
    this._recipesMenu.Configure(tailor, true);
    this._specialRecipesMenu.Configure(tailor, false);
    this._DLCRecipesMenu.Configure(tailor, false, true);
    this._winterRecipesMenu.Configure(tailor, false, MajorDLC: true);
    this._tailorQueue.Configure(tailor);
    TextMeshProUGUI unlockedText = this.unlockedText;
    int num1 = TailorManager.GetUnlockedClothingCount(true, false, false, false);
    string str1 = num1.ToString();
    num1 = TailorManager.GetClothingCount(true, false, false, false);
    string str2 = num1.ToString();
    string str3 = $"{str1}/{str2}";
    unlockedText.text = str3;
    TextMeshProUGUI unlockedUniqueText = this.unlockedUniqueText;
    int num2 = TailorManager.GetUnlockedClothingCount(false, true, false, false);
    string str4 = num2.ToString();
    num2 = TailorManager.GetClothingCount(false, true, false, false);
    string str5 = num2.ToString();
    string str6 = $"{str4}/{str5}";
    unlockedUniqueText.text = str6;
    TextMeshProUGUI unlockedWinterText = this.unlockedWinterText;
    int num3 = TailorManager.GetUnlockedClothingCount(false, false, false, true);
    string str7 = num3.ToString();
    num3 = TailorManager.GetClothingCount(false, false, false, true);
    string str8 = num3.ToString();
    string str9 = $"{str7}/{str8}";
    unlockedWinterText.text = str9;
    TextMeshProUGUI unlockedDlcText = this.unlockedDLCText;
    int num4 = TailorManager.GetUnlockedClothingCount(false, false, true, false);
    string str10 = num4.ToString();
    num4 = TailorManager.GetClothingCount(false, false, true, false);
    string str11 = num4.ToString();
    string str12 = $"{str10}/{str11}";
    unlockedDlcText.text = str12;
    this._recipesMenu.OnRecipeSelected += new Action<ClothingData, string>(this.OnRecipeSelected);
    this._recipesMenu.OnShakeConfigureCard += new System.Action(this.OnShakeConfigureCard);
    this._specialRecipesMenu.OnRecipeSelected += new Action<ClothingData, string>(this.OnRecipeSelected);
    this._winterRecipesMenu.OnRecipeSelected += new Action<ClothingData, string>(this.OnRecipeSelected);
    this._DLCRecipesMenu.OnRecipeSelected += new Action<ClothingData, string>(this.OnRecipeSelected);
    this._tailorQueue.RequestSelectable = new Func<IMMSelectable>(this.ProvideFallback);
    this._tailorQueue.OnRecipeRemoved += new Action<TailorItem, int>(this.OnRecipeRemoved);
    this._tailorQueue.OnRemoveQueueSelected += new Action<bool>(this.ShowRemoveQueueButton);
    this._cookButton.onClick.AddListener(new UnityAction(this.ConfirmCooking));
    this._cookButton.OnConfirmDenied += new System.Action(this.ShakeCookButton);
    this._cookButton.OnSelected += new System.Action(this.OnCookButtonSelected);
    this._cookButton.OnDeselected += new System.Action(this.OnCookButtonDeselected);
    this._cookButton.Confirmable = tailor.Data.QueuedClothings.Count > 0;
    ClothingData clothingData = TailorManager.GetAvailableWeatherClothing()[0];
    this._infoCardController.ForceCurrentCard(this._infoCardController.Card1, clothingData);
    this._infoCardController.Card1.Configure(clothingData, this.tailor, "", this._inMenu);
    this._infoCardController.Card2.Configure(clothingData, this.tailor, "", this._inMenu);
    this._highlightController.SetAsRed();
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null)
      this.StartCoroutine((IEnumerator) this.Wait());
    UIPauseDetailsMenuTabNavigatorBase tabNavigator = this.tabNavigator;
    tabNavigator.OnTabChanged = tabNavigator.OnTabChanged + new Action<int>(this.OnTabChanged);
    this.Show(instant);
  }

  public void ShowRemoveQueueButton(bool val)
  {
    UnityEngine.Debug.Log((object) ("Show remove queue button: " + val.ToString()));
    if (val)
    {
      this.removeToQueueButton.gameObject.SetActive(true);
      this.addToQueueButton.gameObject.SetActive(false);
    }
    else
    {
      this.removeToQueueButton.gameObject.SetActive(false);
      this.addToQueueButton.gameObject.SetActive(true);
    }
    this._controlPrompts.ForceRebuild();
  }

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    base.OnShowStarted();
    this.StartCoroutine((IEnumerator) this.Wait());
  }

  public void Update()
  {
    if (!this._canvasGroup.interactable || !InputManager.UI.GetCookButtonDown() || !this._cookButton.gameObject.activeInHierarchy)
      return;
    this._cookButton.TryPerformConfirmAction();
  }

  public void ConfirmCooking()
  {
    if (this.tailor.Data.QueuedClothings.Count <= 0)
      return;
    this._didConfirm = true;
    this.Hide();
  }

  public void ShakeCookButton()
  {
    this._cookButtonRectTransform.DOKill();
    this._cookButtonRectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public IEnumerator Wait()
  {
    yield return (object) null;
    this.DetermineSelectable();
  }

  public void DetermineSelectable()
  {
    if (DataManager.Instance.previouslyAssignedClothing != FollowerClothingType.None && this.tabNavigator.DefaultTabIndex == 2 && TailorManager.GetCraftedCount(DataManager.Instance.previouslyAssignedClothing) >= 1)
    {
      IMMSelectable mmSelectable = (IMMSelectable) this.SelectDefaultAssignButton();
      if (mmSelectable != null)
        this.OverrideDefault(mmSelectable.Selectable);
    }
    else
    {
      IMMSelectable mmSelectable = this._recipesMenu.ProvideFirstSelectable();
      if (mmSelectable != null)
        this.OverrideDefault(mmSelectable.Selectable);
    }
    this.ActivateNavigation();
  }

  public void OnCookButtonSelected()
  {
    this._highlightController.Image.color = new Color(1f, 1f, 1f, 1f);
    this._highlightController.transform.DOKill();
    this._highlightController.transform.DOShakeScale(0.1f, new Vector3(-0.1f, 0.1f, 1f), 5, fadeOut: false).SetUpdate<Tweener>(true);
  }

  public void OnCookButtonDeselected()
  {
    this._highlightController.Image.color = new Color(0.0f, 0.5f, 1f, 1f);
  }

  public void OnRecipeSelected(ClothingData recipe, string variant)
  {
    this._infoCardController.ShowCardWithParam(recipe);
    this._infoCardController.CurrentCard?.Configure(recipe, this.tailor, variant, this._inMenu);
  }

  public void OnShakeConfigureCard() => this._infoCardController.CurrentCard.ShakeInfo();

  public void OnRecipeChosen(ClothingData recipe, string variant)
  {
    if (!recipe.CanBeCrafted || this.tailor.Data.QueuedClothings.Count == this.RecipeLimit())
      return;
    UIManager.PlayAudio("event:/ui/confirm_selection");
    UIManager.PlayAudio("event:/player/layer_clothes");
    this._highlightController.SetAsRed();
    foreach (ClothingData.CostItem costItem in new List<ClothingData.CostItem>((IEnumerable<ClothingData.CostItem>) recipe.Cost))
      Inventory.ChangeItemQuantity(costItem.ItemType, -costItem.Cost);
    this.tailor.Data.Inventory.Clear();
    this.tailor.Data.QueuedClothings.Add(new StructuresData.ClothingStruct()
    {
      ClothingType = recipe.ClothingType,
      Variant = variant
    });
    this._tailorQueue.AddRecipe(recipe, variant).OnItemHighlighted += (Action<TailorItem>) (item => this.OnRecipeSelected(item.ClothingData, variant));
    this._recipesMenu.UpdateQuantities();
    this._specialRecipesMenu.UpdateQuantities();
    this._winterRecipesMenu.UpdateQuantities();
    this._DLCRecipesMenu.UpdateQuantities();
    if (this.tailor.Data.QueuedClothings.Count == this.RecipeLimit() && !InputManager.General.MouseInputActive)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._cookButton);
    this._cookButton.Confirmable = this.tailor.Data.QueuedClothings.Count > 0;
    if (!((UnityEngine.Object) this._infoCardController.CurrentCard != (UnityEngine.Object) null))
      return;
    this._infoCardController.CurrentCard.Configure(recipe, this.tailor, variant);
  }

  public void OnRecipeRemoved(TailorItem recipe, int index)
  {
    foreach (ClothingData.CostItem costItem in new List<ClothingData.CostItem>((IEnumerable<ClothingData.CostItem>) recipe.ClothingData.Cost))
      Inventory.AddItem(costItem.ItemType, costItem.Cost);
    this.tailor.Data.QueuedClothings.RemoveAt(index);
    this._recipesMenu.UpdateQuantities();
    this._specialRecipesMenu.UpdateQuantities();
    this._winterRecipesMenu.UpdateQuantities();
    this._DLCRecipesMenu.UpdateQuantities();
    this._cookButton.Confirmable = this.tailor.Data.QueuedClothings.Count > 0;
    this._infoCardController.CurrentCard.Configure(recipe.ClothingData, this.tailor, recipe.Variant);
  }

  public IMMSelectable ProvideFallback()
  {
    IMMSelectable mmSelectable = this._DLCRecipesMenu.ProvideSelectable();
    if (mmSelectable == null)
      this._specialRecipesMenu.ProvideSelectable();
    return mmSelectable ?? this._recipesMenu.ProvideSelectable();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || !this.Cancellable)
      return;
    this.Hide();
    System.Action onCancel = this.OnCancel;
    if (onCancel == null)
      return;
    onCancel();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted()
  {
    if (this._didConfirm)
    {
      System.Action onConfirm = this.OnConfirm;
      if (onConfirm != null)
        onConfirm();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public int RecipeLimit() => 5;

  public void ForceAssignTab()
  {
    this.tabNavigator.DefaultTabIndex = 2;
    this.StartCoroutine((IEnumerator) this.ShowUnlockSequence());
    this.OnTabChanged(2);
  }

  public MMButton SelectDefaultAssignButton()
  {
    foreach (TailorItem tailorItem in this._recipesMenu._items)
    {
      if (tailorItem.ClothingData.ClothingType == DataManager.Instance.previouslyAssignedClothing && TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) >= 1)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) tailorItem.Button);
        return tailorItem.Button;
      }
    }
    foreach (TailorItem tailorItem in this._DLCRecipesMenu._items)
    {
      if (tailorItem.ClothingData.ClothingType == DataManager.Instance.previouslyAssignedClothing && TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) >= 1)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) tailorItem.Button);
        return tailorItem.Button;
      }
    }
    foreach (TailorItem tailorItem in this._winterRecipesMenu._items)
    {
      if (tailorItem.ClothingData.ClothingType == DataManager.Instance.previouslyAssignedClothing && TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) >= 1)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) tailorItem.Button);
        return tailorItem.Button;
      }
    }
    foreach (TailorItem tailorItem in this._specialRecipesMenu._items)
    {
      if (tailorItem.ClothingData.ClothingType == DataManager.Instance.previouslyAssignedClothing && TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) >= 1)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) tailorItem.Button);
        return tailorItem.Button;
      }
    }
    return (MMButton) null;
  }

  public IEnumerator ShowUnlockSequence()
  {
    UITailorMenuController tailorMenuController = this;
    if (DataManager.Instance._revealingOutfits.Count > 0)
    {
      UnityEngine.Debug.Log((object) "Do Show Animation: ".Colour(Color.yellow));
      yield return (object) new WaitForSecondsRealtime(0.1f);
      foreach (TailorItem tailorItem in tailorMenuController._recipesMenu._items)
      {
        if (DataManager.Instance._revealingOutfits.ContainsKey(tailorItem.ClothingData.ClothingType))
          tailorItem.HideAllAlerts();
      }
      foreach (TailorItem tailorItem in tailorMenuController._specialRecipesMenu._items)
      {
        if (DataManager.Instance._revealingOutfits.ContainsKey(tailorItem.ClothingData.ClothingType))
          tailorItem.HideAllAlerts();
      }
      foreach (TailorItem tailorItem in tailorMenuController._winterRecipesMenu._items)
      {
        if (DataManager.Instance._revealingOutfits.ContainsKey(tailorItem.ClothingData.ClothingType))
          tailorItem.HideAllAlerts();
      }
      foreach (TailorItem tailorItem in tailorMenuController._DLCRecipesMenu._items)
      {
        if (DataManager.Instance._revealingOutfits.ContainsKey(tailorItem.ClothingData.ClothingType))
          tailorItem.HideAllAlerts();
      }
      tailorMenuController._controlPrompts.gameObject.SetActive(false);
      TailorItem target = (TailorItem) null;
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      tailorMenuController.SetActiveStateForMenu(false);
      yield return (object) tailorMenuController.\u003C\u003En__0();
      yield return (object) new WaitForSecondsRealtime(0.1f);
      List<TailorItem> targetItems = new List<TailorItem>();
      foreach (TailorItem tailorItem in tailorMenuController._recipesMenu._items)
      {
        if (DataManager.Instance._revealingOutfits.ContainsKey(tailorItem.ClothingData.ClothingType))
          targetItems.Add(tailorItem);
      }
      foreach (TailorItem tailorItem in tailorMenuController._specialRecipesMenu._items)
      {
        if (DataManager.Instance._revealingOutfits.ContainsKey(tailorItem.ClothingData.ClothingType))
          targetItems.Add(tailorItem);
      }
      foreach (TailorItem tailorItem in tailorMenuController._winterRecipesMenu._items)
      {
        if (DataManager.Instance._revealingOutfits.ContainsKey(tailorItem.ClothingData.ClothingType))
          targetItems.Add(tailorItem);
      }
      foreach (TailorItem tailorItem in tailorMenuController._DLCRecipesMenu._items)
      {
        if (DataManager.Instance._revealingOutfits.ContainsKey(tailorItem.ClothingData.ClothingType))
          targetItems.Add(tailorItem);
      }
      targetItems = targetItems.OrderByDescending<TailorItem, float>((Func<TailorItem, float>) (x => x.transform.position.y)).ToList<TailorItem>();
      for (int i = 0; i < targetItems.Count; ++i)
      {
        target = targetItems[i];
        if ((UnityEngine.Object) target != (UnityEngine.Object) null)
        {
          UIManager.PlayAudio("event:/sermon/scroll_sermon_menu");
          yield return (object) tailorMenuController._scrollView.DoScrollTo(target.RectTransform);
          yield return (object) new WaitForSecondsRealtime(0.1f);
          DataManager.Instance._revealingOutfits.Remove(targetItems[i].ClothingData.ClothingType);
          yield return (object) target.DoUnlock();
        }
        yield return (object) new WaitForSecondsRealtime(0.1f);
        tailorMenuController._infoCardController.Card1.Configure(target.ClothingData, tailorMenuController.tailor, "", TailorItem.InMenu.Assign);
        yield return (object) new WaitForSecondsRealtime(0.1f);
      }
      tailorMenuController._controlPrompts.gameObject.SetActive(true);
      tailorMenuController.SetActiveStateForMenu(true);
      if (targetItems.Count > 0)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) targetItems[targetItems.Count - 1].Button);
        tailorMenuController._scrollView.ScrollTo(targetItems[targetItems.Count - 1].RectTransform);
      }
      DataManager.Instance._revealingOutfits.Clear();
      target = (TailorItem) null;
      targetItems = (List<TailorItem>) null;
    }
  }

  public void OnTabChanged(int newTabIndex)
  {
    IMMSelectable currentSelectable = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable;
    this.openButton.gameObject.SetActive(false);
    this.selectButton.gameObject.SetActive(false);
    this.removeToQueueButton.gameObject.SetActive(false);
    this.addToQueueButton.gameObject.SetActive(false);
    switch (newTabIndex)
    {
      case 0:
        this.addToQueueButton.gameObject.SetActive(true);
        break;
      case 1:
        this.openButton.gameObject.SetActive(true);
        break;
      case 2:
        this.selectButton.gameObject.SetActive(true);
        break;
    }
    this._controlPrompts.ForceRebuild();
    this.horGroup.CalculateLayoutInputHorizontal();
    if (currentSelectable != null || newTabIndex == 0)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._recipesMenu._items[0].Button);
    System.Action onDeselected = this._cookButton.OnDeselected;
    if (onDeselected != null)
      onDeselected();
    this._tailorQueue.ForceDeselectItems();
  }

  public void ScrollToCurrentSelection()
  {
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null || MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null || !((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable != (UnityEngine.Object) null))
      return;
    this._scrollView.ScrollTo(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<RectTransform>());
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => this.DoShowAnimation();
}
