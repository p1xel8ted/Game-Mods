// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICustomizeClothesController_Variant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UICustomizeClothesController_Variant : UIMenuBase
{
  public Action<int> OnVariantChanged;
  [Header("Variant")]
  [SerializeField]
  public RectTransform _variantContent;
  [SerializeField]
  public MMButton _randomiseVariant;
  public int _cachedVariant;
  public List<IndoctrinationVariantItem> _variantItems = new List<IndoctrinationVariantItem>();
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

  public void Show(FollowerClothingType clothingType, bool instant = false)
  {
    this.clothingData = TailorManager.GetClothingData(clothingType);
    this._randomiseVariant.onClick.AddListener(new UnityAction(this.ChooseRandomVariant));
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    this._cachedVariant = 0;
    this.ActivateNavigation();
  }

  public override void OnHideStarted() => this.ApplyCachedSettings();

  public void ApplyCachedSettings()
  {
    Action<int> onVariantChanged = this.OnVariantChanged;
    if (onVariantChanged == null)
      return;
    onVariantChanged(this._cachedVariant);
  }

  public void ChooseRandomVariant()
  {
  }

  public void OnVariantItemSelected(IndoctrinationVariantItem variantItem)
  {
    int selection = this._variantItems.IndexOf(variantItem);
    this._cachedVariant = selection;
    Action<int> onVariantChanged = this.OnVariantChanged;
    if (onVariantChanged != null)
      onVariantChanged(selection);
    this.UpdateVariantSelection(selection);
    this.Hide();
  }

  public void UpdateVariantSelection(int selection)
  {
    for (int index = 0; index < this._variantItems.Count; ++index)
    {
      if (index == selection)
        this._variantItems[index].SetAsSelected();
      else
        this._variantItems[index].SetAsDefault();
    }
  }

  public void OnSelection(Selectable current)
  {
    IndoctrinationVariantItem component;
    if (!current.TryGetComponent<IndoctrinationVariantItem>(out component))
      return;
    Action<int> onVariantChanged = this.OnVariantChanged;
    if (onVariantChanged == null)
      return;
    onVariantChanged(this._variantItems.IndexOf(component));
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
