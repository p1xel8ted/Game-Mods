// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICustomizeClothesMenuController_Colour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UICustomizeClothesMenuController_Colour : UIMenuBase
{
  public Action<ClothingData, int, string> OnClothingChanged;
  public Action<ClothingData, int, string> OnClothingSelected;
  [Header("Variant")]
  [SerializeField]
  public MMScrollRect _scrollRectVariant;
  [SerializeField]
  public RectTransform _variantContent;
  [SerializeField]
  public RectTransform _variantContainer;
  [SerializeField]
  public MMButton _randomiseVariant;
  [Header("Colour")]
  [SerializeField]
  public MMScrollRect _scrollRectColour;
  [SerializeField]
  public RectTransform _colourContent;
  [SerializeField]
  public MMButton _randomiseColour;
  public List<IndoctrinationColourItem> _variantsItems = new List<IndoctrinationColourItem>();
  public List<IndoctrinationColourItem> _colourItems = new List<IndoctrinationColourItem>();
  public Follower follower;
  public string cachedOutfitVariant;
  public string currentOutfitVariant;
  public int cachedOutfitColour;
  public int currentOutfitColour;
  public ClothingData clothingData;

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
  }

  public new void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectionChanged);
  }

  public void Show(FollowerClothingType clothingType, string variant, bool instant = false)
  {
    this.clothingData = TailorManager.GetClothingData(clothingType);
    this.cachedOutfitColour = this.currentOutfitColour = DataManager.Instance.GetClothingColour(clothingType);
    this.cachedOutfitVariant = this.currentOutfitVariant = variant;
    this._randomiseVariant.onClick.AddListener(new UnityAction(this.ChooseRandomVariant));
    this._randomiseColour.onClick.AddListener(new UnityAction(this.ChooseRandomColour));
    this.Show(instant);
  }

  public void Show(
    Follower follower,
    FollowerClothingType clothingType,
    string variant,
    bool instant = false)
  {
    this.follower = follower;
    this.Show(clothingType, variant, instant);
  }

  public override void OnShowStarted()
  {
    this._scrollRectColour.normalizedPosition = Vector2.one;
    this._scrollRectColour.enabled = false;
    this._scrollRectVariant.normalizedPosition = Vector2.one;
    this._scrollRectVariant.enabled = false;
    List<WorshipperData.SlotsAndColours> slotAndColours = this.clothingData.SlotAndColours;
    if (this._colourItems.Count > 0)
    {
      foreach (Component colourItem in this._colourItems)
        UnityEngine.Object.Destroy((UnityEngine.Object) colourItem.gameObject);
      this._colourItems.Clear();
    }
    List<WorshipperData.SlotsAndColours> slotsAndColoursList = new List<WorshipperData.SlotsAndColours>();
    slotsAndColoursList.AddRange((IEnumerable<WorshipperData.SlotsAndColours>) slotAndColours);
    MMButton newSelectable = (MMButton) null;
    for (int color = 0; color < slotsAndColoursList.Count; ++color)
    {
      IndoctrinationColourItem indoctrinationColourItem1 = MonoSingleton<UIManager>.Instance.FollowerColourItemTemplate.Spawn<IndoctrinationColourItem>((Transform) this._colourContent);
      indoctrinationColourItem1.transform.localScale = Vector3.one;
      indoctrinationColourItem1.Configure(this.clothingData, color, this.currentOutfitVariant);
      IndoctrinationColourItem indoctrinationColourItem2 = indoctrinationColourItem1;
      indoctrinationColourItem2.OnItemSelected = indoctrinationColourItem2.OnItemSelected + new Action<IndoctrinationColourItem>(this.OnColourItemSelected);
      IndoctrinationColourItem indoctrinationColourItem3 = indoctrinationColourItem1;
      indoctrinationColourItem3.OnItemHighlighted = indoctrinationColourItem3.OnItemHighlighted + new Action<IndoctrinationColourItem>(this.OnColourItemHighlighted);
      this._colourItems.Add(indoctrinationColourItem1);
    }
    for (int index = 0; index < this.clothingData.Variants.Count; ++index)
    {
      IndoctrinationColourItem indoctrinationColourItem4 = MonoSingleton<UIManager>.Instance.FollowerColourItemTemplate.Spawn<IndoctrinationColourItem>((Transform) this._variantContent);
      indoctrinationColourItem4.transform.localScale = Vector3.one;
      indoctrinationColourItem4.Configure(this.clothingData, DataManager.Instance.GetClothingColour(this.clothingData.ClothingType), this.clothingData.Variants[index]);
      IndoctrinationColourItem indoctrinationColourItem5 = indoctrinationColourItem4;
      indoctrinationColourItem5.OnItemHighlighted = indoctrinationColourItem5.OnItemHighlighted + new Action<IndoctrinationColourItem>(this.OnVariantItemHighlighted);
      IndoctrinationColourItem indoctrinationColourItem6 = indoctrinationColourItem4;
      indoctrinationColourItem6.OnItemSelected = indoctrinationColourItem6.OnItemSelected + new Action<IndoctrinationColourItem>(this.OnVariantItemSelected);
      if (this.cachedOutfitVariant == this.clothingData.Variants[index])
        newSelectable = indoctrinationColourItem4.Button;
      this._variantsItems.Add(indoctrinationColourItem4);
    }
    if (this.clothingData.Variants.Count <= 1)
    {
      this._variantContainer.gameObject.SetActive(false);
      newSelectable = this._colourItems[0].Button;
    }
    if ((UnityEngine.Object) newSelectable == (UnityEngine.Object) null)
      newSelectable = this._variantsItems[0].Button;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) newSelectable);
    this.ActivateNavigation();
    this._scrollRectColour.enabled = true;
    this._scrollRectVariant.enabled = true;
  }

  public override void OnHideStarted() => this.ApplyCachedSettings();

  public void ApplyCachedSettings()
  {
    DataManager.Instance.SetClothingColour(this.clothingData.ClothingType, this.cachedOutfitColour);
    DataManager.Instance.SetClothingVariant(this.clothingData.ClothingType, this.cachedOutfitVariant);
    Action<ClothingData, int, string> onClothingChanged = this.OnClothingChanged;
    if (onClothingChanged != null)
      onClothingChanged(this.clothingData, this.cachedOutfitColour, this.cachedOutfitVariant);
    this._scrollRectColour.normalizedPosition = Vector2.one;
    this._scrollRectColour.enabled = false;
    this._scrollRectVariant.normalizedPosition = Vector2.one;
    this._scrollRectVariant.enabled = false;
  }

  public void ChooseRandomVariant()
  {
    this.OnVariantItemSelected(this._variantsItems[UnityEngine.Random.Range(0, this._variantsItems.Count)]);
  }

  public void ChooseRandomColour()
  {
    this.OnColourItemSelected(this._colourItems[UnityEngine.Random.Range(0, this._colourItems.Count)]);
  }

  public void OnVariantItemSelected(IndoctrinationColourItem colourItem)
  {
    if (!this.CanvasGroup.interactable)
      return;
    if ((bool) (UnityEngine.Object) this.follower)
      this.follower.Brain.AssignClothing(colourItem.ClothingType, colourItem.ClothingVariant);
    this.currentOutfitVariant = colourItem.ClothingVariant;
    this.cachedOutfitVariant = colourItem.ClothingVariant;
    Action<ClothingData, int, string> clothingSelected = this.OnClothingSelected;
    if (clothingSelected != null)
      clothingSelected(this.clothingData, this.currentOutfitColour, this.currentOutfitVariant);
    this.UpdateVariantSelection(colourItem.ClothingVariant);
    for (int index = 0; index < this._colourItems.Count; ++index)
      this._colourItems[index].Configure(this.clothingData, index, colourItem.ClothingVariant);
    DataManager.Instance.SetClothingVariant(colourItem.ClothingType, colourItem.ClothingVariant);
  }

  public void OnVariantItemHighlighted(IndoctrinationColourItem colourItem)
  {
    if (!this.CanvasGroup.interactable)
      return;
    this.UpdateVariantSelection(colourItem.ClothingVariant);
    if (this.currentOutfitColour != this.cachedOutfitColour)
      DataManager.Instance.SetClothingColour(colourItem.ClothingType, this.cachedOutfitColour);
    this.currentOutfitVariant = colourItem.ClothingVariant;
    Action<ClothingData, int, string> onClothingChanged = this.OnClothingChanged;
    if (onClothingChanged == null)
      return;
    onClothingChanged(this.clothingData, this.cachedOutfitColour, this.currentOutfitVariant);
  }

  public void OnColourItemHighlighted(IndoctrinationColourItem colourItem)
  {
    if (!this.CanvasGroup.interactable)
      return;
    this.UpdateColourSelection(colourItem.ClothingColour);
    if (this.currentOutfitVariant != this.cachedOutfitVariant)
    {
      this.currentOutfitVariant = this.cachedOutfitVariant;
      DataManager.Instance.SetClothingVariant(colourItem.ClothingType, this.currentOutfitVariant);
    }
    this.currentOutfitColour = colourItem.ClothingColour;
    DataManager.Instance.SetClothingColour(colourItem.ClothingType, colourItem.ClothingColour);
    Action<ClothingData, int, string> onClothingChanged = this.OnClothingChanged;
    if (onClothingChanged == null)
      return;
    onClothingChanged(this.clothingData, this.currentOutfitColour, this.currentOutfitVariant);
  }

  public void OnColourItemSelected(IndoctrinationColourItem colourItem)
  {
    if (!this.CanvasGroup.interactable)
      return;
    DataManager.Instance.SetClothingColour(colourItem.ClothingType, colourItem.ClothingColour);
    this.currentOutfitColour = colourItem.ClothingColour;
    this.cachedOutfitColour = colourItem.ClothingColour;
    Action<ClothingData, int, string> clothingSelected = this.OnClothingSelected;
    if (clothingSelected != null)
      clothingSelected(this.clothingData, this.currentOutfitColour, this.currentOutfitVariant);
    this.UpdateColourSelection(colourItem.ClothingColour);
    for (int index = 0; index < this._variantsItems.Count; ++index)
      this._variantsItems[index].Configure(this.clothingData, DataManager.Instance.GetClothingColour(this.clothingData.ClothingType), this.clothingData.Variants.Count > 0 ? this.clothingData.Variants[index] : FollowerBrain.GetClothingName(this.clothingData.ClothingType));
  }

  public void UpdateColourSelection(int selection)
  {
    for (int index = 0; index < this._colourItems.Count; ++index)
    {
      if (this._colourItems[index].ClothingColour == selection)
        this._colourItems[index].SetAsSelected();
      else
        this._colourItems[index].SetAsDefault();
    }
  }

  public void UpdateVariantSelection(string variant)
  {
    for (int index = 0; index < this._variantsItems.Count; ++index)
    {
      if (this._variantsItems[index].ClothingVariant == variant)
        this._variantsItems[index].SetAsSelected();
      else
        this._variantsItems[index].SetAsDefault();
    }
  }

  public void OnSelection(Selectable current)
  {
    if (!current.TryGetComponent<IndoctrinationColourItem>(out IndoctrinationColourItem _))
      return;
    Action<ClothingData, int, string> onClothingChanged = this.OnClothingChanged;
    if (onClothingChanged == null)
      return;
    onClothingChanged(this.clothingData, this.currentOutfitColour, this.currentOutfitVariant);
  }

  public void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
