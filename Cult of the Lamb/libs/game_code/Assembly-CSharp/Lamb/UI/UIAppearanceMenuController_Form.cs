// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIAppearanceMenuController_Form
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
  public MMScrollRect _scrollRect;
  [SerializeField]
  public Transform _lockIcon;
  [Header("Content")]
  [SerializeField]
  public RectTransform _miscContent;
  [SerializeField]
  public RectTransform _specialEventsContent;
  [SerializeField]
  public RectTransform _dlcContent;
  [SerializeField]
  public RectTransform _majorDlcContent;
  [SerializeField]
  public RectTransform _darkwoodContent;
  [SerializeField]
  public RectTransform _anuraContent;
  [SerializeField]
  public RectTransform _anchorDeepContent;
  [SerializeField]
  public RectTransform _silkCradleContent;
  [SerializeField]
  public RectTransform _dungeon5Content;
  [SerializeField]
  public RectTransform _dungeon6Content;
  [Header("Counts")]
  [SerializeField]
  public TextMeshProUGUI _miscUnlocked;
  [SerializeField]
  public TextMeshProUGUI _specialEventsUnlocked;
  [SerializeField]
  public TextMeshProUGUI _dlcUnlocked;
  [SerializeField]
  public TextMeshProUGUI _majorDlcUnlocked;
  [SerializeField]
  public TextMeshProUGUI _darkwoodUnlocked;
  [SerializeField]
  public TextMeshProUGUI _anuraUnlocked;
  [SerializeField]
  public TextMeshProUGUI _anchorDeepUnlocked;
  [SerializeField]
  public TextMeshProUGUI _silkCradleUnlocked;
  [SerializeField]
  public TextMeshProUGUI _dungeon5Unlocked;
  [SerializeField]
  public TextMeshProUGUI _dungeon6Unlocked;
  [SerializeField]
  public GameObject _requiresMajorDLC;
  [SerializeField]
  public GameObject _requiresMajorDLCSpacer;
  [Header("Randomisation")]
  [SerializeField]
  public MMButton _randomiseButtonMisc;
  [SerializeField]
  public MMButton _randomiseButtonSpecialEvents;
  [SerializeField]
  public MMButton _randomiseButtonDLC;
  [SerializeField]
  public MMButton _randomiseButtonMajorDLC;
  [SerializeField]
  public MMButton _randomiseButtonDungeon1;
  [SerializeField]
  public MMButton _randomiseButtonDungeon2;
  [SerializeField]
  public MMButton _randomiseButtonDungeon3;
  [SerializeField]
  public MMButton _randomiseButtonDungeon4;
  [SerializeField]
  public MMButton _randomiseButtonDungeon5;
  [SerializeField]
  public MMButton _randomiseButtonDungeon6;
  [Header("Headers")]
  [SerializeField]
  public GameObject _dlcHeader;
  [SerializeField]
  public GameObject _majorDlcHeader;
  [SerializeField]
  public GameObject _specialEventsHeader;
  public List<IndoctrinationFormItem> _formItems = new List<IndoctrinationFormItem>();
  public Follower _follower;
  public int _cachedForm;

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
    this._randomiseButtonMajorDLC.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Major_DLC)));
    this._randomiseButtonDungeon1.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon1)));
    this._randomiseButtonDungeon2.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon2)));
    this._randomiseButtonDungeon3.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon3)));
    this._randomiseButtonDungeon4.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon4)));
    this._randomiseButtonDungeon5.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon5)));
    this._randomiseButtonDungeon6.onClick.AddListener((UnityAction) (() => this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon6)));
    this.Show(instant);
  }

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

  public override void OnShowStarted()
  {
    this._cachedForm = this._follower.Brain.Info.SkinCharacter;
    if (this._cachedForm == 0)
      this._follower.Brain.Info.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(this._follower.Brain.Info.SkinName.StripNumbers());
    this._scrollRect.normalizedPosition = Vector2.one;
    this._requiresMajorDLC.gameObject.SetActive(!DataManager.Instance.MAJOR_DLC);
    this._requiresMajorDLCSpacer.gameObject.SetActive(!DataManager.Instance.MAJOR_DLC);
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
      foreach (string majorDlcSkin in DataManager.Instance.MajorDLCSkins)
      {
        if (DataManager.GetFollowerSkinUnlocked(majorDlcSkin))
        {
          flag2 = true;
          break;
        }
      }
      this._majorDlcContent.gameObject.SetActive(flag2);
      this._majorDlcHeader.SetActive(flag2);
      bool flag3 = false;
      foreach (string specialEventSkin in DataManager.Instance.SpecialEventSkins)
      {
        if (DataManager.GetFollowerSkinUnlocked(specialEventSkin))
        {
          flag3 = true;
          break;
        }
      }
      this._specialEventsContent.gameObject.SetActive(flag3);
      this._specialEventsHeader.SetActive(flag3);
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Other), this._miscContent, UIFollowerFormsMenuController.MiscOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.SpecialEvents), this._specialEventsContent, UIFollowerFormsMenuController.SpecialEventsOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.DLC), this._dlcContent, UIFollowerFormsMenuController.DLCOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Major_DLC), this._majorDlcContent));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon1), this._darkwoodContent, UIFollowerFormsMenuController.DarkwoodOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon2), this._anuraContent, UIFollowerFormsMenuController.AnuraOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon3), this._anchorDeepContent, UIFollowerFormsMenuController.AnchordeepOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon4), this._silkCradleContent, UIFollowerFormsMenuController.SilkCradleOrder));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon5), this._dungeon5Content, UIFollowerFormsMenuController.Dungeon5Order));
      this._formItems.AddRange((IEnumerable<IndoctrinationFormItem>) this.Populate(WorshipperData.Instance.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon6), this._dungeon6Content, UIFollowerFormsMenuController.Dungeon6Order));
      this.SetUnlockedText(this._miscUnlocked, WorshipperData.DropLocation.Other);
      this.SetUnlockedText(this._specialEventsUnlocked, WorshipperData.DropLocation.SpecialEvents);
      this.SetUnlockedText(this._dlcUnlocked, WorshipperData.DropLocation.DLC);
      this.SetUnlockedText(this._majorDlcUnlocked, WorshipperData.DropLocation.Major_DLC);
      this.SetUnlockedText(this._darkwoodUnlocked, WorshipperData.DropLocation.Dungeon1);
      this.SetUnlockedText(this._anuraUnlocked, WorshipperData.DropLocation.Dungeon2);
      this.SetUnlockedText(this._anchorDeepUnlocked, WorshipperData.DropLocation.Dungeon3);
      this.SetUnlockedText(this._silkCradleUnlocked, WorshipperData.DropLocation.Dungeon4);
      this.SetUnlockedText(this._dungeon5Unlocked, WorshipperData.DropLocation.Dungeon5);
      this.SetUnlockedText(this._dungeon6Unlocked, WorshipperData.DropLocation.Dungeon6);
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

  public List<IndoctrinationFormItem> Populate(
    List<WorshipperData.SkinAndData> types,
    RectTransform contentContainer,
    string[] order = null)
  {
    if (order != null)
      types.Sort((Comparison<WorshipperData.SkinAndData>) ((a, b) => order.IndexOf<string>(a.Title).CompareTo(order.IndexOf<string>(b.Title))));
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

  public void SetUnlockedText(TextMeshProUGUI target, WorshipperData.DropLocation dropLocation)
  {
    int num = 0;
    List<WorshipperData.SkinAndData> skinsFromLocation = WorshipperData.Instance.GetSkinsFromLocation(dropLocation);
    foreach (WorshipperData.SkinAndData skinAndData in skinsFromLocation)
    {
      if (!skinAndData.Invariant && DataManager.GetFollowerSkinUnlocked(skinAndData.Skin[0].Skin))
        ++num;
    }
    string str = LocalizeIntegration.ReverseText($"{num}/{skinsFromLocation.Count}");
    target.text = string.Format(LocalizationManager.GetTranslation("UI/Collected"), (object) str);
  }

  public override void OnHideStarted()
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

  public void ChooseRandomForm(WorshipperData.DropLocation dropLocation)
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

  public void OnFormSelected(IndoctrinationFormItem formItem)
  {
    this._cachedForm = this.GetFormItemIndex(formItem);
    this.UpdateSelection(formItem);
    Action<int> onFormChanged = this.OnFormChanged;
    if (onFormChanged != null)
      onFormChanged(this._cachedForm);
    this.Hide();
  }

  public void UpdateSelection(IndoctrinationFormItem formItem)
  {
    foreach (IndoctrinationFormItem formItem1 in this._formItems)
    {
      if ((UnityEngine.Object) formItem1 == (UnityEngine.Object) formItem)
        formItem1.SetAsSelected();
      else
        formItem1.SetAsDefault();
    }
  }

  public void OnSelection(Selectable current)
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

  public void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  public int GetFormItemIndex(IndoctrinationFormItem formItem)
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

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_0() => this.ChooseRandomForm(WorshipperData.DropLocation.Other);

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_1()
  {
    this.ChooseRandomForm(WorshipperData.DropLocation.SpecialEvents);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_2() => this.ChooseRandomForm(WorshipperData.DropLocation.DLC);

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_3()
  {
    this.ChooseRandomForm(WorshipperData.DropLocation.Major_DLC);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_4()
  {
    this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon1);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_5()
  {
    this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon2);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_6()
  {
    this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon3);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_7()
  {
    this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon4);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_8()
  {
    this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon5);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__42_9()
  {
    this.ChooseRandomForm(WorshipperData.DropLocation.Dungeon6);
  }
}
