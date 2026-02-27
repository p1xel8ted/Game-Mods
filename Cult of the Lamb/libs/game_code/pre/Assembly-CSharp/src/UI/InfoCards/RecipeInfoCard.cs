// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RecipeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RecipeItem _recipeItem;
  [SerializeField]
  private TextMeshProUGUI _itemHeader;
  [SerializeField]
  private TextMeshProUGUI _itemDescription;
  [SerializeField]
  private TextMeshProUGUI _mealEffects;
  [SerializeField]
  private GameObject[] _starFills;
  [SerializeField]
  private TextMeshProUGUI[] _ingredients;
  [SerializeField]
  private HungerRecipeController _hungerRecipeController;
  [SerializeField]
  private TextMeshProUGUI _hungerDelta;
  [SerializeField]
  private TextMeshProUGUI _categoryIcon;
  [SerializeField]
  private TextMeshProUGUI _followerCount;

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
      mealEffects2.text = $"{mealEffects2.text}{(flag ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">")}{((flag ? (object) "+" : (object) "-").ToString() + (object) Mathf.Abs(thoughtData.Modifier)).Colour(flag ? StaticColors.GreenColor : StaticColors.RedColor)} {ScriptLocalization.UI_PrisonMenu.Faith}";
    }
    int satationLevel = CookingData.GetSatationLevel(config);
    for (int index = 0; index < this._starFills.Length; ++index)
      this._starFills[index].SetActive(satationLevel >= index + 1);
    this._ingredients[0].text = "";
    foreach (List<InventoryItem> inventoryItemList in CookingData.GetRecipe(config))
    {
      foreach (InventoryItem inventoryItem in inventoryItemList)
      {
        this._ingredients[0].text += new StructuresData.ItemCost((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity).ToStringShowQuantity();
        this._ingredients[0].text += "    ";
        this._ingredients[0].gameObject.SetActive(true);
      }
      this._ingredients[0].text += "\n \n";
    }
    float f = Mathf.Round(this._hungerRecipeController.GetDelta(config));
    Color colour = (double) f > 0.0 ? StaticColors.GreenColor : StaticColors.RedColor;
    this._hungerDelta.text = ((double) f > 0.0 ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">") + Mathf.Abs(f).ToString().Colour(colour);
    this._categoryIcon.text = "";
    this._followerCount.text = "<sprite name=\"icon_Followers\">" + (object) DataManager.Instance.Followers.Count;
  }
}
