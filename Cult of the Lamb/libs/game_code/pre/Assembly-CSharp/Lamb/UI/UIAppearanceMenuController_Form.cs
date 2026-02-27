// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIAppearanceMenuController_Form
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIAppearanceMenuController_Form : UIMenuBase
{
  public Action<int> OnFormChanged;
  [Header("Forms Menu")]
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private Transform _lockIcon;
  [Header("Content")]
  [SerializeField]
  private RectTransform _miscContent;
  [SerializeField]
  private RectTransform _specialEventsContent;
  [SerializeField]
  private RectTransform _dlcContent;
  [SerializeField]
  private RectTransform _darkwoodContent;
  [SerializeField]
  private RectTransform _anuraContent;
  [SerializeField]
  private RectTransform _anchorDeepContent;
  [SerializeField]
  private RectTransform _silkCradleContent;
  [Header("Counts")]
  [SerializeField]
  private TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  private TextMeshProUGUI _specialEventsUnlocked;
  [SerializeField]
  private TextMeshProUGUI _dlcUnlocked;
  [SerializeField]
  private TextMeshProUGUI _darkwoodUnlocked;
  [SerializeField]
  private TextMeshProUGUI _anuraUnlocked;
  [SerializeField]
  private TextMeshProUGUI _anchorDeepUnlocked;
  [SerializeField]
  private TextMeshProUGUI _silkCradleUnlocked;
  [Header("Randomisation")]
  [SerializeField]
  private MMButton _randomiseButtonMisc;
  [SerializeField]
  private MMButton _randomiseButtonSpecialEvents;
  [SerializeField]
  private MMButton _randomiseButtonDLC;
  [SerializeField]
  private MMButton _randomiseButtonDungeon1;
  [SerializeField]
  private MMButton _randomiseButtonDungeon2;
  [SerializeField]
  private MMButton _randomiseButtonDungeon3;
  [SerializeField]
  private MMButton _randomiseButtonDungeon4;
  [Header("Headers")]
  [SerializeField]
  private GameObject _dlcHeader;
  [SerializeField]
  private GameObject _specialEventsHeader;
  private List<IndoctrinationFormItem> _formItems = new List<IndoctrinationFormItem>();
  private Follower _follower;
  private int _cachedForm;

  public override void Awake()
  {
    this._lockIcon.gameObject.SetActive(false);
    base.Awake();
  }

  public void Show(Follower follower, bool instant = false)
  {
    this._follower = follower;
    this._randomiseButtonMisc.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Other)));
    this._randomiseButtonSpecialEvents.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.SpecialEvents)));
    this._randomiseButtonDLC.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.DLC)));
    this._randomiseButtonDungeon1.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon1)));
    this._randomiseButtonDungeon2.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon2)));
    this._randomiseButtonDungeon3.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon3)));
    this._randomiseButtonDungeon4.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon4)));
    this.Show(instant);
  }

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

  protected override void OnShowStarted()
  {
    this._cachedForm = this._follower.Brain.Info.SkinCharacter;
    this._scrollRect.normalizedPosition = Vector2.one;
    if (this._formItems.Count == 0)
    {
      this._scrollRect.enabled = false;
      bool flag1 = false;
      foreach (string dlcSkin in DataManager.Instance.DLCSkins)
      {
        if (DataManager.GetFollowerSkinUnlocked(dlcSkin))
        {
          flag1 = true;
          break;
        }
      }
      this._dlcContent.gameObject.SetActive(flag1);
      this._dlcHeader.SetActive(flag1);
      bool flag2 = false;
      foreach (string specialEventSkin in DataManager.Instance.SpecialEventSkins)
      {
        if (DataManager.GetFollowerSkinUnlocked(specialEventSkin))
        {
          flag2 = true;
          break;
        }
      }
      this._specialEventsContent.gameObject.SetActive(flag2);
      this._specialEventsHeader.SetActive(flag2);
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Other), this._miscContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.SpecialEvents), this._specialEventsContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.DLC), this._dlcContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon1), this._darkwoodContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon2), this._anuraContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon3), this._anchorDeepContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon4), this._silkCradleContent));
      this.SetUnlockedText(this._miscUnlocked, WorshipperData.DropLocation.Other);
      this.SetUnlockedText(this._specialEventsUnlocked, WorshipperData.DropLocation.SpecialEvents);
      this.SetUnlockedText(this._dlcUnlocked, WorshipperData.DropLocation.DLC);
      this.SetUnlockedText(this._darkwoodUnlocked, WorshipperData.DropLocation.Dungeon1);
      this.SetUnlockedText(this._anuraUnlocked, WorshipperData.DropLocation.Dungeon2);
      this.SetUnlockedText(this._anchorDeepUnlocked, WorshipperData.DropLocation.Dungeon3);
      this.SetUnlockedText(this._silkCradleUnlocked, WorshipperData.DropLocation.Dungeon4);
      this._scrollRect.enabled = true;
    }
    WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[this._follower.Brain.Info.SkinCharacter];
    foreach (IndoctrinationFormItem formItem in this._formItems)
    {
      if (formItem.Skin == character.Skin[0].Skin)
      {
        this.OverrideDefaultOnce((Selectable) formItem.Button);
        this.UpdateSelection(formItem);
        break;
      }
    }
    this.ActivateNavigation();
  }

  private List<IndoctrinationFormItem> Populate(
    List<WorshipperData.SkinAndData> types,
    RectTransform contentContainer)
  {
    List<IndoctrinationFormItem> indoctrinationFormItemList = new List<IndoctrinationFormItem>();
    foreach (WorshipperData.SkinAndData type in types)
    {
      IndoctrinationFormItem indoctrinationFormItem1 = MonoSingleton<UIManager>.Instance.FollowerFormItemTemplate.Spawn<IndoctrinationFormItem>((Transform) contentContainer);
      indoctrinationFormItem1.transform.localScale = Vector3.one;
      indoctrinationFormItem1.Configure(type);
      IndoctrinationFormItem indoctrinationFormItem2 = indoctrinationFormItem1;
      indoctrinationFormItem2.OnItemSelected = indoctrinationFormItem2.OnItemSelected + new Action<IndoctrinationFormItem>(this.OnFormSelected);
      this._formItems.Add(indoctrinationFormItem1);
    }
    return indoctrinationFormItemList;
  }

  private void SetUnlockedText(TextMeshProUGUI target, WorshipperData.DropLocation dropLocation)
  {
    int num = 0;
    List<WorshipperData.SkinAndData> skinsFromLocation = WorshipperData.Instance.GetSkinsFromLocation(dropLocation);
    foreach (WorshipperData.SkinAndData skinAndData in skinsFromLocation)
    {
      if (!skinAndData.Invariant && DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin))
        ++num;
    }
    target.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) $"{num}/{skinsFromLocation.Count}");
  }

  protected override void OnHideStarted()
  {
    this._scrollRect.enabled = false;
    this.ApplyCachedSettings();
  }

  public void ApplyCachedSettings()
  {
    Action<int> onFormChanged = this.OnFormChanged;
    if (onFormChanged != null)
      onFormChanged(this._cachedForm);
    this._lockIcon.gameObject.SetActive(false);
  }

  private void ChooseRandomForm(WorshipperData.DropLocation dropLocation)
  {
    List<IndoctrinationFormItem> list = new List<IndoctrinationFormItem>();
    foreach (IndoctrinationFormItem formItem in this._formItems)
    {
      if (!formItem.Locked && formItem.DropLocation == dropLocation && formItem.Skin != this._follower.Brain.Info.SkinName.StripNumbers())
        list.Add(formItem);
    }
    if (list.Count <= 0)
      return;
    this.OnFormSelected(list.RandomElement<IndoctrinationFormItem>());
  }

  private void OnFormSelected(IndoctrinationFormItem formItem)
  {
    this._cachedForm = this.GetFormItemIndex(formItem);
    this.UpdateSelection(formItem);
    Action<int> onFormChanged = this.OnFormChanged;
    if (onFormChanged != null)
      onFormChanged(this._cachedForm);
    this.Hide();
  }

  private void UpdateSelection(IndoctrinationFormItem formItem)
  {
    foreach (IndoctrinationFormItem formItem1 in this._formItems)
    {
      if ((UnityEngine.Object) formItem1 == (UnityEngine.Object) formItem)
        formItem1.SetAsSelected();
      else
        formItem1.SetAsDefault();
    }
  }

  private void OnSelection(Selectable current)
  {
    IndoctrinationFormItem component;
    if (!current.TryGetComponent<IndoctrinationFormItem>(out component))
      return;
    this._lockIcon.gameObject.SetActive(component.Locked);
    Action<int> onFormChanged = this.OnFormChanged;
    if (onFormChanged == null)
      return;
    onFormChanged(this.GetFormItemIndex(component));
  }

  private void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  private int GetFormItemIndex(IndoctrinationFormItem formItem)
  {
    foreach (WorshipperData.SkinAndData character in WorshipperData.Instance.Characters)
    {
      if (formItem.Skin == character.Skin[0].Skin)
        return WorshipperData.Instance.Characters.IndexOf(character);
    }
    return -1;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
