// Decompiled with JetBrains decompiler
// Type: TailorQueue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TailorQueue : BaseMonoBehaviour
{
  public Func<IMMSelectable> RequestSelectable;
  public Action<TailorItem, int> OnRecipeRemoved;
  public Action<bool> OnRemoveQueueSelected;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public TailorItem _inventoryItemTemplate;
  [SerializeField]
  public TextMeshProUGUI _count;
  [SerializeField]
  public GameObject[] _vacantSlots;
  public Structures_Tailor tailor;
  public List<TailorItem> _items = new List<TailorItem>();

  public List<TailorItem> Items => this._items;

  public void Configure(Structures_Tailor tailor)
  {
    this.tailor = tailor;
    for (int index = 0; index < this._vacantSlots.Length; ++index)
      this._vacantSlots[index].SetActive(index < this.RecipeLimit());
    foreach (StructuresData.ClothingStruct queuedClothing in tailor.Data.QueuedClothings)
      this.AddRecipe(TailorManager.GetClothingData(queuedClothing.ClothingType), queuedClothing.Variant);
    this.UpdateCount();
  }

  public TailorItem AddRecipe(ClothingData recipe, string variant)
  {
    TailorItem tailorItem = this.MakeRecipe(recipe, variant);
    this.UpdateCount();
    return tailorItem;
  }

  public TailorItem MakeRecipe(ClothingData recipe, string variant)
  {
    TailorItem newItem = this._inventoryItemTemplate.Instantiate<TailorItem>((Transform) this._contentContainer);
    newItem.Button.onClick.AddListener((UnityAction) (() => this.RemoveRecipe(newItem)));
    Vector3 localScale = newItem.RectTransform.localScale;
    newItem.RectTransform.localScale = Vector3.one * 1.2f;
    newItem.RectTransform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    newItem.Configure(recipe, variant, true);
    this._vacantSlots[this._items.Count].SetActive(false);
    this._items.Add(newItem);
    newItem.transform.SetSiblingIndex(this._items.Count - 1);
    newItem.OnRemoveQueueSelected += new Action<bool>(this.OnRemoveSelected);
    return newItem;
  }

  public void OnRemoveSelected(bool val)
  {
    Debug.Log((object) ("Tailor Queue, Show remove queue button: " + val.ToString()));
    Action<bool> removeQueueSelected = this.OnRemoveQueueSelected;
    if (removeQueueSelected == null)
      return;
    removeQueueSelected(val);
  }

  public void RemoveRecipe(TailorItem item)
  {
    Action<TailorItem, int> onRecipeRemoved = this.OnRecipeRemoved;
    if (onRecipeRemoved != null)
      onRecipeRemoved(item, this._items.IndexOf(item));
    IMMSelectable selectableOnRight = item.Button.FindSelectableOnRight() as IMMSelectable;
    IMMSelectable selectableOnLeft = item.Button.FindSelectableOnLeft() as IMMSelectable;
    if (this._items.IndexOf(item) < this._items.Count - 1 && selectableOnRight != null)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(selectableOnRight);
    else if (this._items.IndexOf(item) > 0 && selectableOnLeft != null)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(selectableOnLeft);
    else if (this._items.Count - 1 > 0)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._items[this._items.Count - 2].Button);
    else
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(this.RequestSelectable());
    this._items.Remove(item);
    UnityEngine.Object.Destroy((UnityEngine.Object) item.gameObject);
    this._vacantSlots[this._items.Count].SetActive(true);
    this.UpdateCount();
  }

  public void UpdateCount()
  {
    Color colour = this._items.Count > 0 ? StaticColors.GreenColor : StaticColors.RedColor;
    this._count.text = $"{this._items.Count}/{this.RecipeLimit()}".Colour(colour);
  }

  public int RecipeLimit() => 5;

  public void UpdateItems(FollowerClothingType clothingType, string variant)
  {
    foreach (TailorItem tailorItem in this._items)
    {
      if (tailorItem.ClothingData.ClothingType == clothingType)
        tailorItem.UpdateIcon(variant);
    }
  }

  public void ForceDeselectItems()
  {
    for (int index = 0; index < this._items.Count; ++index)
      this._items[index].ForceDeselect();
  }
}
