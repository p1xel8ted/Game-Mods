// Decompiled with JetBrains decompiler
// Type: TailorMenuConfigure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TailorMenuConfigure : MonoBehaviour
{
  public const int kMaxRecipes = 5;
  public Action<ClothingData, string> OnRecipeChosen;
  public Action<ClothingData, string> OnRecipeSelected;
  public System.Action OnShakeConfigureCard;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public TailorItem _recipeItemTemplate;
  [SerializeField]
  public GameObject _noRecipesText;
  public Structures_Tailor _kitchenData;
  [CompilerGenerated]
  public List<TailorItem> \u003C_items\u003Ek__BackingField = new List<TailorItem>();

  public List<TailorItem> _items
  {
    get => this.\u003C_items\u003Ek__BackingField;
    set => this.\u003C_items\u003Ek__BackingField = value;
  }

  public void Configure(Structures_Tailor tailor, bool DefaultRobes, bool DLC = false, bool MajorDLC = false)
  {
    this._kitchenData = tailor;
    if (this._items.Count == 0)
    {
      List<ClothingData> clothingDataList = new List<ClothingData>();
      if (DLC)
      {
        foreach (ClothingData clothingData in TailorManager.GetDLCClothing())
          clothingDataList.Add(clothingData);
        clothingDataList.Sort((Comparison<ClothingData>) ((a, b) => a.IsCultist.CompareTo(b.IsCultist)));
        clothingDataList.Sort((Comparison<ClothingData>) ((a, b) => a.IsHeretic.CompareTo(b.IsHeretic)));
        clothingDataList.Sort((Comparison<ClothingData>) ((a, b) => a.IsSinful.CompareTo(b.IsSinful)));
        clothingDataList.Sort((Comparison<ClothingData>) ((a, b) => a.IsPilgrim.CompareTo(b.IsPilgrim)));
      }
      else if (MajorDLC)
      {
        foreach (ClothingData clothingData in TailorManager.GetMajorDLCClothing())
          clothingDataList.Add(clothingData);
      }
      else
      {
        foreach (ClothingData clothingData in DefaultRobes ? TailorManager.GetAvailableWeatherClothing() : TailorManager.GetUnlockedSpecialClothing())
          clothingDataList.Add(clothingData);
        foreach (ClothingData clothingData in DefaultRobes ? TailorManager.GetUnavailableWeatherClothing() : TailorManager.GetLockedSpecialClothing())
          clothingDataList.Add(clothingData);
      }
      foreach (ClothingData data in clothingDataList)
      {
        TailorItem tailorItem = this._recipeItemTemplate.Instantiate<TailorItem>((Transform) this._contentContainer);
        tailorItem.Configure(data, DataManager.Instance.GetClothingVariant(data.ClothingType), showQuantity: true);
        tailorItem.SetCount();
        tailorItem.OnItemSelected += new Action<TailorItem>(this.OnRecipeClicked);
        tailorItem.OnItemHighlighted += new Action<TailorItem>(this.RecipeSelected);
        tailorItem.OnShakeConfigureCard += new Action<TailorItem>(this.ShakeConfigureCard);
        this._items.Add(tailorItem);
      }
    }
    else
      this.UpdateQuantities();
    this._noRecipesText.SetActive(this._items.Count == 0);
  }

  public void OnRecipeClicked(TailorItem item)
  {
    if (this._kitchenData.Data.QueuedClothings.Count == this.RecipeLimit() && item.GetMenu() == TailorItem.InMenu.Craft)
      item.Shake();
    Action<ClothingData, string> onRecipeChosen = this.OnRecipeChosen;
    if (onRecipeChosen == null)
      return;
    onRecipeChosen(item.ClothingData, item.Variant);
  }

  public void RecipeSelected(TailorItem item)
  {
    Action<ClothingData, string> onRecipeSelected = this.OnRecipeSelected;
    if (onRecipeSelected == null)
      return;
    onRecipeSelected(item.ClothingData, item.Variant);
  }

  public void ShakeConfigureCard(TailorItem item)
  {
    if (item.ClothingData.CanBeCrafted)
      return;
    System.Action shakeConfigureCard = this.OnShakeConfigureCard;
    if (shakeConfigureCard == null)
      return;
    shakeConfigureCard();
  }

  public void UpdateQuantities()
  {
    foreach (UIInventoryItem uiInventoryItem in this._items)
      uiInventoryItem.UpdateQuantity();
  }

  public IMMSelectable ProvideFirstSelectable()
  {
    return this._items.Count > 0 ? (IMMSelectable) this._items[0].Button : (IMMSelectable) null;
  }

  public IMMSelectable ProvideSelectable()
  {
    return this._items.Count > 0 ? (IMMSelectable) this._items.LastElement<TailorItem>().Button : (IMMSelectable) null;
  }

  public int RecipeLimit() => 5;

  public int ReadilyAvailableMeals()
  {
    int num = 0;
    foreach (TailorItem tailorItem in this._items)
      num += tailorItem.Quantity;
    return num;
  }

  public void UpdateItems(FollowerClothingType clothingType, string variant)
  {
    foreach (TailorItem tailorItem in this._items)
    {
      if (tailorItem.ClothingData.ClothingType == clothingType)
        tailorItem.UpdateIcon(variant);
    }
  }
}
