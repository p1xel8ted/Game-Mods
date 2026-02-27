// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KitchenMenu.UICookingFireMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using src.UI;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.KitchenMenu;

public class UICookingFireMenuController : UIMenuBase
{
  public System.Action OnConfirm;
  [Header("Kitchen Menu")]
  [SerializeField]
  public RecipesMenu _recipesMenu;
  [SerializeField]
  public RecipeQueue _recipeQueue;
  [SerializeField]
  public MMButton _cookButton;
  [SerializeField]
  public RectTransform _cookButtonRectTransform;
  [SerializeField]
  public ButtonHighlightController _highlightController;
  [SerializeField]
  public RecipeInfoCardController _infoCardController;
  [Header("Hunger Bar")]
  [SerializeField]
  public TextMeshProUGUI _followerCount;
  public StructuresData _kitchenData;
  public bool _didConfirm;

  public virtual void Show(StructuresData kitchenData, bool instant = false)
  {
    this._kitchenData = kitchenData;
    this._recipesMenu.Configure(kitchenData);
    this._recipeQueue.Configure(kitchenData);
    foreach (Interaction_Kitchen.QueuedMeal queuedMeal in this._kitchenData.QueuedMeals)
    {
      this._infoCardController.Card1.HungerRecipeController.AddRecipe(queuedMeal.MealType);
      this._infoCardController.Card2.HungerRecipeController.AddRecipe(queuedMeal.MealType);
    }
    this._recipesMenu.OnRecipeChosen += new Action<InventoryItem.ITEM_TYPE>(this.OnRecipeChosen);
    this._recipeQueue.RequestSelectable = new Func<IMMSelectable>(this.ProvideFallback);
    this._recipeQueue.OnRecipeRemoved += new Action<InventoryItem.ITEM_TYPE, int>(this.OnRecipeRemoved);
    this._cookButton.onClick.AddListener(new UnityAction(this.ConfirmCooking));
    this._cookButton.OnConfirmDenied += new System.Action(this.ShakeCookButton);
    this._cookButton.OnSelected += new System.Action(this.OnCookButtonSelected);
    this._cookButton.OnDeselected += new System.Action(this.OnCookButtonDeselected);
    this._cookButton.Confirmable = kitchenData.QueuedMeals.Count > 0;
    this._highlightController.SetAsRed();
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable == null)
      this.StartCoroutine(this.Wait());
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    base.OnShowStarted();
    this.StartCoroutine(this.Wait());
  }

  public void Update()
  {
    if (!this._canvasGroup.interactable || !InputManager.UI.GetCookButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      return;
    this._cookButton.TryPerformConfirmAction();
  }

  public void ConfirmCooking()
  {
    if (this._kitchenData.QueuedMeals.Count <= 0)
      return;
    this._didConfirm = true;
    this.Hide();
  }

  public void ShakeCookButton()
  {
    this._cookButtonRectTransform.DOKill();
    this._cookButtonRectTransform.anchoredPosition = Vector2.zero;
    this._cookButtonRectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public IEnumerator Wait()
  {
    yield return (object) null;
    this.DetermineSelectable();
  }

  public void DetermineSelectable()
  {
    IMMSelectable mmSelectable = this._recipesMenu.ProvideFirstSelectable();
    if (mmSelectable != null)
      this.OverrideDefault(mmSelectable.Selectable);
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

  public void OnRecipeChosen(InventoryItem.ITEM_TYPE recipe)
  {
    UIManager.PlayAudio("event:/cooking/add_food_ingredient");
    this._highlightController.SetAsRed();
    List<InventoryItem> inventoryItemList = CookingData.GetRecipe(recipe)[0];
    foreach (InventoryItem inventoryItem in inventoryItemList)
      Inventory.ChangeItemQuantity(inventoryItem.type, -inventoryItem.quantity);
    this._recipesMenu.UpdateQuantities();
    this._infoCardController.CurrentCard.Configure(recipe);
    this._kitchenData.Inventory.Clear();
    this._kitchenData.QueuedMeals.Add(new Interaction_Kitchen.QueuedMeal()
    {
      MealType = recipe,
      CookingDuration = CookingData.GetMealCookDuration(recipe),
      Ingredients = inventoryItemList
    });
    this._recipeQueue.AddRecipe(recipe);
    this._infoCardController.Card1.HungerRecipeController.AddRecipe(recipe);
    this._infoCardController.Card2.HungerRecipeController.AddRecipe(recipe);
    if ((this._kitchenData.QueuedMeals.Count == this.RecipeLimit() || this._recipesMenu.ReadilyAvailableMeals() == 0) && !InputManager.General.MouseInputActive)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._cookButton);
    this._cookButton.Confirmable = this._kitchenData.QueuedMeals.Count > 0;
  }

  public void OnRecipeRemoved(InventoryItem.ITEM_TYPE recipe, int index)
  {
    if ((UnityEngine.Object) this._infoCardController.CurrentCard != (UnityEngine.Object) null)
      this._infoCardController.CurrentCard.Configure(recipe);
    foreach (InventoryItem ingredient in this._kitchenData.QueuedMeals[index].Ingredients)
      Inventory.AddItem(ingredient.type, ingredient.quantity);
    this._recipesMenu.UpdateQuantities();
    this._kitchenData.QueuedMeals.RemoveAt(index);
    this._infoCardController.Card1.HungerRecipeController.RemoveRecipe(recipe);
    this._infoCardController.Card2.HungerRecipeController.RemoveRecipe(recipe);
    this._cookButton.Confirmable = this._kitchenData.QueuedMeals.Count > 0;
  }

  public IMMSelectable ProvideFallback() => this._recipesMenu.ProvideSelectable();

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

  public int RecipeLimit()
  {
    return 12 + (this._kitchenData.Type == StructureBrain.TYPES.KITCHEN_II ? 5 : 0);
  }
}
