// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIAppearanceMenuController_Colour
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

public class UIAppearanceMenuController_Colour : UIMenuBase
{
  public Action<int> OnColourChanged;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [Header("Colour")]
  [SerializeField]
  private RectTransform _colourContent;
  [SerializeField]
  private MMButton _randomiseColour;
  private Follower _follower;
  private int _cachedColour;
  private List<IndoctrinationColourItem> _colourItems = new List<IndoctrinationColourItem>();

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
    this._randomiseColour.onClick.AddListener(new UnityAction(this.ChooseRandomColour));
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    this._cachedColour = this._follower.Brain.Info.SkinColour;
    this._scrollRect.normalizedPosition = Vector2.one;
    this._scrollRect.enabled = false;
    WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[this._follower.Brain.Info.SkinCharacter];
    if (this._colourItems.Count > 0)
    {
      foreach (Component colourItem in this._colourItems)
        UnityEngine.Object.Destroy((UnityEngine.Object) colourItem.gameObject);
      this._colourItems.Clear();
    }
    List<WorshipperData.SlotsAndColours> slotsAndColoursList = new List<WorshipperData.SlotsAndColours>();
    slotsAndColoursList.AddRange((IEnumerable<WorshipperData.SlotsAndColours>) character.SlotAndColours);
    foreach (WorshipperData.SlotsAndColours slotsAndColours in slotsAndColoursList)
    {
      IndoctrinationColourItem indoctrinationColourItem1 = MonoSingleton<UIManager>.Instance.FollowerColourItemTemplate.Spawn<IndoctrinationColourItem>((Transform) this._colourContent);
      indoctrinationColourItem1.transform.localScale = Vector3.one;
      indoctrinationColourItem1.Configure(this._follower.Brain.Info.SkinVariation, character.SlotAndColours.IndexOf(slotsAndColours), character);
      IndoctrinationColourItem indoctrinationColourItem2 = indoctrinationColourItem1;
      indoctrinationColourItem2.OnItemSelected = indoctrinationColourItem2.OnItemSelected + new Action<IndoctrinationColourItem>(this.OnColourItemSelected);
      this._colourItems.Add(indoctrinationColourItem1);
    }
    this.UpdateColourSelection(this._follower.Brain.Info.SkinColour);
    this.OverrideDefaultOnce((Selectable) this._colourItems[this._follower.Brain.Info.SkinColour].Button);
    this.ActivateNavigation();
    this._scrollRect.enabled = true;
  }

  protected override void OnHideStarted() => this.ApplyCachedSettings();

  public void ApplyCachedSettings()
  {
    Action<int> onColourChanged = this.OnColourChanged;
    if (onColourChanged == null)
      return;
    onColourChanged(this._cachedColour);
  }

  private void ChooseRandomColour()
  {
    List<IndoctrinationColourItem> list = this._colourItems.Where<IndoctrinationColourItem>((Func<IndoctrinationColourItem, bool>) (i => !i.Locked)).ToList<IndoctrinationColourItem>();
    list.Remove(this._colourItems[this._follower.Brain.Info.SkinColour]);
    if (list.Count <= 0)
      return;
    this.OnColourItemSelected(list.RandomElement<IndoctrinationColourItem>());
  }

  private void OnColourItemSelected(IndoctrinationColourItem colourItem)
  {
    int selection = this._colourItems.IndexOf(colourItem);
    this._cachedColour = selection;
    Action<int> onColourChanged = this.OnColourChanged;
    if (onColourChanged != null)
      onColourChanged(selection);
    this.UpdateColourSelection(selection);
    this.Hide();
  }

  private void UpdateColourSelection(int selection)
  {
    for (int index = 0; index < this._colourItems.Count; ++index)
    {
      if (index == selection)
        this._colourItems[index].SetAsSelected();
      else
        this._colourItems[index].SetAsDefault();
    }
  }

  private void OnSelection(Selectable current)
  {
    IndoctrinationColourItem component;
    if (!current.TryGetComponent<IndoctrinationColourItem>(out component))
      return;
    Action<int> onColourChanged = this.OnColourChanged;
    if (onColourChanged == null)
      return;
    onColourChanged(this._colourItems.IndexOf(component));
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
