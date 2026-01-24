// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RecipeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class RecipeInfoCard : UIInfoCardBase<InventoryItem.ITEM_TYPE>
{
  [SerializeField]
  public RecipeItem _recipeItem;
  [SerializeField]
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [SerializeField]
  public TextMeshProUGUI _mealEffects;
  [SerializeField]
  public GameObject[] _starFills;
  [SerializeField]
  public TextMeshProUGUI[] _ingredients;
  [SerializeField]
  public HungerRecipeController _hungerRecipeController;
  [SerializeField]
  public TextMeshProUGUI _hungerDelta;
  [SerializeField]
  public TextMeshProUGUI _categoryIcon;
  [SerializeField]
  public TextMeshProUGUI _followerCount;

  public HungerRecipeController HungerRecipeController => this._hungerRecipeController;

  public override void Configure(InventoryItem.ITEM_TYPE config)
  {
    this._recipeItem.Configure(config, false, false);
    this._itemHeader.text = LocalizationManager.GetTranslation($"CookingData/{config}/Name");
    this._itemDescription.text = LocalizationManager.GetTranslation($"CookingData/{config}/Description");
    CookingData.MealEffect[] mealEffects1 = CookingData.GetMealEffects(config);
    if (mealEffects1.Length == 0)
    {
      this._mealEffects.gameObject.SetActive(false);
    }
    else
    {
      this._mealEffects.text = "";
      this._mealEffects.gameObject.SetActive(true);
      foreach (CookingData.MealEffect mealEffect in mealEffects1)
      {
        this._mealEffects.text += CookingData.GetEffectDescription(mealEffect, config);
        this._mealEffects.text += "\n";
      }
    }
    ThoughtData thoughtData = CookingData.ThoughtDataForMeal(config);
    if ((double) thoughtData.Modifier != 0.0)
    {
      bool flag = (double) thoughtData.Modifier > 0.0;
      this._mealEffects.text += "\n";
      TextMeshProUGUI mealEffects2 = this._mealEffects;
      mealEffects2.text = $"{mealEffects2.text}{(flag ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">")}{((flag ? "+" : "-") + Mathf.Abs(thoughtData.Modifier).ToString()).Colour(flag ? StaticColors.GreenColor : StaticColors.RedColor)} {ScriptLocalization.UI_PrisonMenu.Faith}";
    }
    int satationLevel = CookingData.GetSatationLevel(config);
    for (int index = 0; index < this._starFills.Length; ++index)
      this._starFills[index].SetActive(satationLevel >= index + 1);
    this._ingredients[0].text = "";
    this._ingredients[0].isRightToLeftText = false;
    foreach (List<InventoryItem> inventoryItemList in CookingData.GetRecipe(config))
    {
      foreach (InventoryItem inventoryItem in inventoryItemList)
      {
        this._ingredients[0].text += new StructuresData.ItemCost((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity).ToStringShowQuantity();
        this._ingredients[0].isRightToLeftText = LocalizeIntegration.IsArabic();
        this._ingredients[0].text += "    ";
        this._ingredients[0].gameObject.SetActive(true);
      }
      this._ingredients[0].text += "\n \n";
    }
    if ((Object) this._hungerRecipeController != (Object) null)
    {
      float f = Mathf.Round(this._hungerRecipeController.GetDelta(config));
      if ((double) Mathf.Abs(f) > 0.0)
      {
        Color colour = (double) f > 0.0 ? StaticColors.GreenColor : StaticColors.RedColor;
        this._hungerDelta.text = ((double) f > 0.0 ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">") + Mathf.Abs(f).ToString().Colour(colour);
      }
      else
        this._hungerDelta.text = string.Empty;
    }
    this._categoryIcon.text = "";
    this._followerCount.text = "<sprite name=\"icon_Followers\">" + (object) DataManager.Instance.Followers.Count;
  }
}
