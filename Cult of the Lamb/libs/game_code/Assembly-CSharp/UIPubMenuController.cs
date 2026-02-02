// Decompiled with JetBrains decompiler
// Type: UIPubMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIPubMenuController : UIMenuBase
{
  public Action<InventoryItem.ITEM_TYPE> OnItemQueued;
  public Action<InventoryItem.ITEM_TYPE, int> OnItemRemovedFromQueue;
  [SerializeField]
  public DrinkRecipeInfoCardController _infoCardController;
  public RecipeItem recipeIconPrefab;
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
  public List<RecipeItem> _recipeItems = new List<RecipeItem>();
  public List<RecipeItem> _queuedItems = new List<RecipeItem>();
  public Tween _queuedTextTween;
  public StructuresData _structureInfo;
  public Interaction_Pub _pub;

  public void Show(StructuresData pubData, Interaction_Pub pub, bool instant = false)
  {
    this._structureInfo = pubData;
    this._pub = pub;
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    for (int index = 0; index < this._vacantSlots.Length; ++index)
      this._vacantSlots[index].SetActive(index < this._pub.Brain.MaxQueue);
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    foreach (InventoryItem.ITEM_TYPE allDrink in CookingData.GetAllDrinks())
    {
      if (CookingData.CanMakeMeal(allDrink))
        CookingData.TryDiscoverRecipe(allDrink);
      if (!DataManager.Instance.MAJOR_DLC && CookingData.IsRecipeDLC(allDrink))
        Debug.Log((object) $"Drink  {allDrink.ToString()} is DLC only, skipping");
      else
        itemTypeList.Add(allDrink);
    }
    itemTypeList.Sort((Comparison<InventoryItem.ITEM_TYPE>) ((a, b) => CookingData.HasRecipeDiscovered(b).CompareTo(CookingData.HasRecipeDiscovered(a))));
    foreach (InventoryItem.ITEM_TYPE type in itemTypeList)
    {
      RecipeItem recipeItem = UnityEngine.Object.Instantiate<RecipeItem>(this.recipeIconPrefab, this.Container);
      recipeItem.OnRecipeChosen += new Action<RecipeItem>(this.OnRecipeChosen);
      recipeItem.Button.OnSelected += new System.Action(this.OnQueueableItemSelected);
      recipeItem.Configure(type, true, false);
      recipeItem.FadeIn((float) this._recipeItems.Count * 0.03f);
      this._recipeItems.Add(recipeItem);
    }
    this.OverrideDefault((Selectable) this._recipeItems[0].Button);
    this.ActivateNavigation();
    for (int index = 0; index < this._structureInfo.QueuedMeals.Count; ++index)
      this.MakeQueuedItem(this._structureInfo.QueuedMeals[index].MealType);
    this.UpdateQueueText();
    this.UpdateQuantities();
  }

  public void UpdateQueueText()
  {
    this.amountQueuedText.text = $"{this._queuedItems.Count}/{this._pub.Brain.MaxQueue}".Colour(this._queuedItems.Count > 0 ? StaticColors.GreenColor : StaticColors.RedColor);
  }

  public void UpdateQuantities()
  {
    foreach (UIInventoryItem recipeItem in this._recipeItems)
      recipeItem.UpdateQuantity();
    foreach (UIInventoryItem queuedItem in this._queuedItems)
      queuedItem.UpdateQuantity();
  }

  public void OnRecipeChosen(RecipeItem item)
  {
    if (this._structureInfo.QueuedMeals.Count >= this._pub.Brain.MaxQueue || !CookingData.CanMakeMeal(item.Type))
    {
      this.ShowMaxQueued();
      item.Shake();
    }
    else
    {
      InventoryItem.ITEM_TYPE itemType = item.Type;
      GameManager.GetInstance().WaitForSecondsRealtime(0.35f, (System.Action) (() =>
      {
        if (itemType == InventoryItem.ITEM_TYPE.DRINK_BEER)
          AudioManager.Instance.PlayOneShot("event:/building/brewery/select_bubbly_drink", PlayerFarming.Instance.gameObject);
        else if (itemType == InventoryItem.ITEM_TYPE.DRINK_COCKTAIL)
          AudioManager.Instance.PlayOneShot("event:/building/brewery/select_carbonated_drink", PlayerFarming.Instance.gameObject);
        else if (itemType == InventoryItem.ITEM_TYPE.DRINK_WINE || itemType == InventoryItem.ITEM_TYPE.DRINK_GIN)
          AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", PlayerFarming.Instance.gameObject);
        else if (itemType == InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE || itemType == InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE)
        {
          AudioManager.Instance.PlayOneShot("event:/building/brewery/select_icecream", PlayerFarming.Instance.gameObject);
        }
        else
        {
          switch (itemType)
          {
            case InventoryItem.ITEM_TYPE.DRINK_EGGNOG:
              AudioManager.Instance.PlayOneShot("event:/building/brewery/select_crack_egg", PlayerFarming.Instance.gameObject);
              break;
            case InventoryItem.ITEM_TYPE.DRINK_CHILLI:
              AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
              break;
            case InventoryItem.ITEM_TYPE.DRINK_LIGHTNING:
              AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
              break;
            case InventoryItem.ITEM_TYPE.DRINK_SIN:
              AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
              break;
            case InventoryItem.ITEM_TYPE.DRINK_GRASS:
              AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
              break;
            case InventoryItem.ITEM_TYPE.DRINK_MILKSHAKE:
              AudioManager.Instance.PlayOneShot("event:/building/brewery/select_icecream", this.transform.position);
              break;
          }
        }
      }));
      this.AddToQueue(item.Type);
    }
  }

  public void OnQueueableItemSelected()
  {
    if (this._queuedItems.Count >= this._pub.Brain.MaxQueue)
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

  public void AddToQueue(InventoryItem.ITEM_TYPE meal)
  {
    this._structureInfo.QueuedMeals.Add(new Interaction_Kitchen.QueuedMeal()
    {
      MealType = meal,
      CookingDuration = CookingData.GetMealCookDuration(meal),
      CookedTime = 0.0f,
      Ingredients = CookingData.GetRecipeSimplified(meal)
    });
    foreach (InventoryItem inventoryItem in CookingData.GetRecipe(meal)[0])
      Inventory.ChangeItemQuantity(inventoryItem.type, -inventoryItem.quantity);
    RecipeItem recipeItem1 = this.MakeQueuedItem(meal);
    Vector3 localScale = recipeItem1.RectTransform.localScale;
    recipeItem1.RectTransform.localScale = Vector3.one * 1.2f;
    recipeItem1.RectTransform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._infoCardController.CurrentCard.Configure(meal);
    Action<InventoryItem.ITEM_TYPE> onItemQueued = this.OnItemQueued;
    if (onItemQueued != null)
      onItemQueued(meal);
    this.UpdateQueueText();
    this.UpdateQuantities();
    this.OnQueueableItemSelected();
    foreach (UIInventoryItem recipeItem2 in this._recipeItems)
      recipeItem2.Button.Confirmable = this._queuedItems.Count < this._pub.Brain.MaxQueue;
  }

  public RecipeItem MakeQueuedItem(InventoryItem.ITEM_TYPE resource)
  {
    RecipeItem recipeItem = UnityEngine.Object.Instantiate<RecipeItem>(this.recipeIconPrefab, this.queuedContainer);
    recipeItem.Configure(resource, false, true);
    recipeItem.Button.OnSelected += new System.Action(this.OnQueuedItemSelected);
    recipeItem.OnRecipeChosen += new Action<RecipeItem>(this.RemoveFromQueue);
    this._vacantSlots[this._queuedItems.Count].SetActive(false);
    this._queuedItems.Add(recipeItem);
    recipeItem.transform.SetSiblingIndex(this._queuedItems.Count - 1);
    return recipeItem;
  }

  public void RemoveFromQueue(RecipeItem item)
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
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._recipeItems[0].Button);
    int index = this._queuedItems.IndexOf(item);
    this._structureInfo.QueuedMeals.RemoveAt(index);
    foreach (InventoryItem inventoryItem in CookingData.GetRecipe(item.Type)[0])
      Inventory.AddItem(inventoryItem.type, inventoryItem.quantity);
    this._queuedItems.Remove(item);
    this._vacantSlots[this._queuedItems.Count].SetActive(true);
    if (index == 0 && this._queuedItems.Count > 0)
    {
      this._queuedItems[0].Configure(this._queuedItems[0].Type, false, true);
      this._structureInfo.Progress = 0.0f;
    }
    Action<InventoryItem.ITEM_TYPE, int> removedFromQueue = this.OnItemRemovedFromQueue;
    if (removedFromQueue != null)
      removedFromQueue(item.Type, index);
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
