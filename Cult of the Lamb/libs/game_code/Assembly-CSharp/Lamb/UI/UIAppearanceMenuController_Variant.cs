// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIAppearanceMenuController_Variant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIAppearanceMenuController_Variant : UIMenuBase
{
  public Action<int> OnVariantChanged;
  [Header("Variant")]
  [SerializeField]
  public RectTransform _variantContent;
  [SerializeField]
  public MMButton _randomiseVariant;
  public Follower _follower;
  public int _cachedVariant;
  public List<IndoctrinationVariantItem> _variantItems = new List<IndoctrinationVariantItem>();

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

  public void Show(Follower follower, bool instant = false)
  {
    this._follower = follower;
    this._randomiseVariant.onClick.AddListener(new UnityAction(this.ChooseRandomVariant));
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    this._cachedVariant = this._follower.Brain.Info.SkinVariation;
    WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[this._follower.Brain.Info.SkinCharacter];
    if (this._variantItems.Count > 0)
    {
      foreach (Component variantItem in this._variantItems)
        UnityEngine.Object.Destroy((UnityEngine.Object) variantItem.gameObject);
      this._variantItems.Clear();
    }
    foreach (WorshipperData.CharacterSkin characterSkin in character.Skin)
    {
      IndoctrinationVariantItem indoctrinationVariantItem1 = MonoSingleton<UIManager>.Instance.FollowerVariantItemTemplate.Spawn<IndoctrinationVariantItem>((Transform) this._variantContent);
      indoctrinationVariantItem1.transform.localScale = Vector3.one;
      indoctrinationVariantItem1.Configure(character.Skin.IndexOf(characterSkin), this._follower.Brain.Info.SkinColour, character);
      IndoctrinationVariantItem indoctrinationVariantItem2 = indoctrinationVariantItem1;
      indoctrinationVariantItem2.OnItemSelected = indoctrinationVariantItem2.OnItemSelected + new Action<IndoctrinationVariantItem>(this.OnVariantItemSelected);
      this._variantItems.Add(indoctrinationVariantItem1);
    }
    this.UpdateVariantSelection(this._follower.Brain.Info.SkinVariation);
    this.OverrideDefaultOnce((Selectable) this._variantItems[this._follower.Brain.Info.SkinVariation].Button);
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
    List<IndoctrinationVariantItem> list = this._variantItems.Where<IndoctrinationVariantItem>((Func<IndoctrinationVariantItem, bool>) (i => !i.Locked)).ToList<IndoctrinationVariantItem>();
    list.Remove(this._variantItems[this._follower.Brain.Info.SkinVariation]);
    if (list.Count <= 0)
      return;
    this.OnVariantItemSelected(list.RandomElement<IndoctrinationVariantItem>());
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
    int num = this._variantItems.IndexOf(component);
    if (num < 0)
      return;
    Action<int> onVariantChanged = this.OnVariantChanged;
    if (onVariantChanged == null)
      return;
    onVariantChanged(num);
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
