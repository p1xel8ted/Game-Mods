// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KitchenMenu.UICookingFireMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RecipesMenu _recipesMenu;
  [SerializeField]
  private RecipeQueue _recipeQueue;
  [SerializeField]
  private MMButton _cookButton;
  [SerializeField]
  private RectTransform _cookButtonRectTransform;
  [SerializeField]
  private ButtonHighlightController _highlightController;
  [SerializeField]
  private RecipeInfoCardController _infoCardController;
  [Header("Hunger Bar")]
  [SerializeField]
  private TextMeshProUGUI _followerCount;
  protected StructuresData _kitchenData;
  private bool _didConfirm;

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
      this.StartCoroutine((IEnumerator) this.Wait());
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    base.OnShowStarted();
    this.StartCoroutine((IEnumerator) this.Wait());
  }

  private void Update()
  {
    if (!this._canvasGroup.interactable || !InputManager.UI.GetCookButtonDown())
      return;
    if (this._kitchenData.QueuedMeals.Count > 0)
      this.ConfirmCooking();
    else
      this.ShakeCookButton();
  }

  private void ConfirmCooking()
  {
    if (this._kitchenData.QueuedMeals.Count <= 0)
      return;
    this._didConfirm = true;
    this.Hide();
  }

  private void ShakeCookButton()
  {
    this._cookButtonRectTransform.DOKill();
    this._cookButtonRectTransform.anchoredPosition = Vector2.zero;
    this._cookButtonRectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  private IEnumerator Wait()
  {
    yield return (object) null;
    this.DetermineSelectable();
  }

  private void DetermineSelectable()
  {
    IMMSelectable mmSelectable = this._recipesMenu.ProvideFirstSelectable();
    if (mmSelectable != null)
      this.OverrideDefault(mmSelectable.Selectable);
    this.ActivateNavigation();
  }

  private void OnCookButtonSelected()
  {
    this._highlightController.Image.color = new Color(1f, 1f, 1f, 1f);
    this._highlightController.transform.DOKill();
    this._highlightController.transform.DOShakeScale(0.1f, new Vector3(-0.1f, 0.1f, 1f), 5, fadeOut: false).SetUpdate<Tweener>(true);
  }

  private void OnCookButtonDeselected()
  {
    this._highlightController.Image.color = new Color(0.0f, 0.5f, 1f, 1f);
  }

  private void OnRecipeChosen(InventoryItem.ITEM_TYPE recipe)
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

  private void OnRecipeRemoved(InventoryItem.ITEM_TYPE recipe, int index)
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

  private IMMSelectable ProvideFallback() => this._recipesMenu.ProvideSelectable();

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

  protected override void OnHideCompleted()
  {
    if (this._didConfirm)
    {
      System.Action onConfirm = this.OnConfirm;
      if (onConfirm != null)
        onConfirm();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private int RecipeLimit()
  {
    return 12 + (this._kitchenData.Type == StructureBrain.TYPES.KITCHEN_II ? 5 : 0);
  }
}
