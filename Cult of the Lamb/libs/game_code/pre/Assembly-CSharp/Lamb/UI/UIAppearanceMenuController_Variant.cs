// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIAppearanceMenuController_Variant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private RectTransform _variantContent;
  [SerializeField]
  private MMButton _randomiseVariant;
  private Follower _follower;
  private int _cachedVariant;
  private List<IndoctrinationVariantItem> _variantItems = new List<IndoctrinationVariantItem>();

  private void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
  }

  private void OnDisable()
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

  protected override void OnShowStarted()
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

  protected override void OnHideStarted() => this.ApplyCachedSettings();

  public void ApplyCachedSettings()
  {
    Action<int> onVariantChanged = this.OnVariantChanged;
    if (onVariantChanged == null)
      return;
    onVariantChanged(this._cachedVariant);
  }

  private void ChooseRandomVariant()
  {
    List<IndoctrinationVariantItem> list = this._variantItems.Where<IndoctrinationVariantItem>((Func<IndoctrinationVariantItem, bool>) (i => !i.Locked)).ToList<IndoctrinationVariantItem>();
    list.Remove(this._variantItems[this._follower.Brain.Info.SkinVariation]);
    if (list.Count <= 0)
      return;
    this.OnVariantItemSelected(list.RandomElement<IndoctrinationVariantItem>());
  }

  private void OnVariantItemSelected(IndoctrinationVariantItem variantItem)
  {
    int selection = this._variantItems.IndexOf(variantItem);
    this._cachedVariant = selection;
    Action<int> onVariantChanged = this.OnVariantChanged;
    if (onVariantChanged != null)
      onVariantChanged(selection);
    this.UpdateVariantSelection(selection);
    this.Hide();
  }

  private void UpdateVariantSelection(int selection)
  {
    for (int index = 0; index < this._variantItems.Count; ++index)
    {
      if (index == selection)
        this._variantItems[index].SetAsSelected();
      else
        this._variantItems[index].SetAsDefault();
    }
  }

  private void OnSelection(Selectable current)
  {
    IndoctrinationVariantItem component;
    if (!current.TryGetComponent<IndoctrinationVariantItem>(out component))
      return;
    Action<int> onVariantChanged = this.OnVariantChanged;
    if (onVariantChanged == null)
      return;
    onVariantChanged(this._variantItems.IndexOf(component));
  }

  private void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
