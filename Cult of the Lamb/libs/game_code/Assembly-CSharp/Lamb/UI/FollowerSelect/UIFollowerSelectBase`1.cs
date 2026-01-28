// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerSelect.UIFollowerSelectBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using src.UI.Overlays.TwitchFollowerVotingOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.FollowerSelect;

public abstract class UIFollowerSelectBase<T> : UIMenuBase, ITwitchVotingProvider where T : FollowerSelectItem
{
  public Action<FollowerInfo> OnFollowerSelectedThirdInteraction;
  public Action<FollowerInfo> OnFollowerSelected;
  public Action<FollowerInfo> OnFollowerHighlighted;
  [Header("Follower Select")]
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public GameObject _availableHeaderContainer;
  [SerializeField]
  public TMP_Text _availableCount;
  [SerializeField]
  public RectTransform _availableContentContainer;
  [SerializeField]
  public GameObject _unavailableHeaderContainer;
  [SerializeField]
  public TMP_Text _unavailableCount;
  [SerializeField]
  public RectTransform _unavailableContentContainer;
  [SerializeField]
  public GameObject _noFollowerText;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public MMSelectable_Dropdown _sortingDropdown;
  [CompilerGenerated]
  public List<FollowerSelectEntry> \u003CFollowerSelectEntries\u003Ek__BackingField;
  public List<T> _followerInfoBoxes = new List<T>();
  public bool _didCancel;
  public bool _hideOnSelection;
  public bool _cancellable;
  public bool _hasSelection;
  public int _currentSortingMethod;
  public string[] _sortingMethods;
  public int shownFollowersCount;
  public int availableCount;
  public int unavailableCount;
  public int itemsToShowOnLoading = 4;
  public MMButton overrideSelectable;
  [CompilerGenerated]
  public TwitchVoting.VotingType \u003CVotingType\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CAllowsVoting\u003Ek__BackingField = true;

  public List<FollowerSelectEntry> FollowerSelectEntries
  {
    set => this.\u003CFollowerSelectEntries\u003Ek__BackingField = value;
    get => this.\u003CFollowerSelectEntries\u003Ek__BackingField;
  }

  public List<T> FollowerInfoBoxes => this._followerInfoBoxes;

  public override void Awake()
  {
    if (DataManager.Instance.PleasureEnabled)
      this._sortingMethods = new string[5]
      {
        ScriptLocalization.UI_Settings_Accessibility_MovementStyle.Default,
        ScriptLocalization.UI_Indoctrination.Name,
        ScriptLocalization.UI_FollowerInfo.Age.Replace(":", string.Empty).Replace("{0}", string.Empty),
        ScriptLocalization.Interactions.Level,
        ScriptLocalization.UI.Sin
      };
    else
      this._sortingMethods = new string[4]
      {
        ScriptLocalization.UI_Settings_Accessibility_MovementStyle.Default,
        ScriptLocalization.UI_Indoctrination.Name,
        ScriptLocalization.UI_FollowerInfo.Age.Replace(":", string.Empty).Replace("{0}", string.Empty),
        ScriptLocalization.Interactions.Level
      };
    base.Awake();
  }

  public void Show(
    List<FollowerSelectEntry> followerSelectEntries,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    this.FollowerSelectEntries = followerSelectEntries;
    this._hideOnSelection = hideOnSelection;
    this._cancellable = cancellable;
    this._hasSelection = hasSelection;
    this._scrollRect.normalizedPosition = Vector2.one;
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    if (!this._hasSelection)
      this._controlPrompts.HideAcceptButton();
    this._scrollRect.enabled = false;
    this._noFollowerText.SetActive(this.FollowerSelectEntries.Count == 0);
    if (!this._cancellable && (UnityEngine.Object) this._controlPrompts != (UnityEngine.Object) null)
      this._controlPrompts.HideCancelButton();
    this._sortingDropdown.Dropdown.PrefillContent(this._sortingMethods);
    this._sortingDropdown.Dropdown.OnValueChanged += new Action<int>(this.OnSortingMethodChanged);
    this._sortingDropdown.Dropdown.OnOpenDropdownOverlay += (System.Action) (() =>
    {
      if (!((UnityEngine.Object) this._controlPrompts != (UnityEngine.Object) null))
        return;
      this._controlPrompts.gameObject.SetActive(false);
    });
    this._sortingDropdown.Dropdown.OnCloseDropdownOverlay += (System.Action) (() =>
    {
      if (!((UnityEngine.Object) this._controlPrompts != (UnityEngine.Object) null))
        return;
      this._controlPrompts.gameObject.SetActive(true);
    });
    this._sortingDropdown.Interactable = false;
    if (this.FollowerSelectEntries.Count > 0)
    {
      this.shownFollowersCount = 0;
      this.availableCount = 0;
      this.unavailableCount = 0;
      this.overrideSelectable = (MMButton) null;
      this._availableContentContainer.gameObject.SetActive(false);
      this.FollowerSelectEntries = this.FollowerSelectEntries.OrderBy<FollowerSelectEntry, int>((Func<FollowerSelectEntry, int>) (f => f.AvailabilityStatus != FollowerSelectEntry.Status.Available ? 1 : 0)).ToList<FollowerSelectEntry>();
      for (int index = 0; index < this.FollowerSelectEntries.Count; ++index)
      {
        if (this.FollowerSelectEntries[index].FollowerInfo.IsFakeBrain)
          this.FollowerSelectEntries[index].AvailabilityStatus = FollowerSelectEntry.Status.Unavailable;
        if (this.FollowerSelectEntries[index].FollowerInfo.ID == 100006 && this.FollowerSelectEntries[index].FollowerInfo.CursedState != Thought.Child && DataManager.Instance.HasMidasHiding)
          this.FollowerSelectEntries[index].AvailabilityStatus = FollowerSelectEntry.Status.Unavailable;
        this.AddFollowerEntry(this.FollowerSelectEntries[index]);
        ++this.shownFollowersCount;
        if (index >= this.itemsToShowOnLoading - 1)
          break;
      }
      for (int index = 0; index < this._followerInfoBoxes.Count; ++index)
      {
        this._followerInfoBoxes[index].gameObject.SetActive(true);
        if (index >= this.itemsToShowOnLoading - 1)
          break;
      }
      this.availableCount = this.FollowerSelectEntries.FindAll((Predicate<FollowerSelectEntry>) (f => f.AvailabilityStatus == FollowerSelectEntry.Status.Available)).Count;
      this.unavailableCount = this.FollowerSelectEntries.Count - this.availableCount;
      this._availableCount.text = $"{(object) this.availableCount}/{(object) this.FollowerSelectEntries.Count}";
      this._unavailableCount.text = $"{(object) this.unavailableCount}/{(object) this.FollowerSelectEntries.Count}";
      this._availableContentContainer.gameObject.SetActive(true);
      if (this._unavailableContentContainer.childCount == 0 && this.FollowerSelectEntries[this.FollowerSelectEntries.Count - 1].AvailabilityStatus == FollowerSelectEntry.Status.Available)
        this._unavailableHeaderContainer.SetActive(false);
      LayoutRebuilder.ForceRebuildLayoutImmediate(this._contentContainer);
    }
    this._scrollRect.enabled = true;
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    if (this.FollowerSelectEntries.Count > 0)
    {
      for (int shownFollowersCount = this.shownFollowersCount; shownFollowersCount < this.FollowerSelectEntries.Count; ++shownFollowersCount)
      {
        if (this.FollowerSelectEntries[shownFollowersCount].FollowerInfo.IsFakeBrain)
          this.FollowerSelectEntries[shownFollowersCount].AvailabilityStatus = FollowerSelectEntry.Status.Unavailable;
        if (this.FollowerSelectEntries[shownFollowersCount].FollowerInfo.ID == 100006 && this.FollowerSelectEntries[shownFollowersCount].FollowerInfo.CursedState != Thought.Child && DataManager.Instance.HasMidasHiding)
          this.FollowerSelectEntries[shownFollowersCount].AvailabilityStatus = FollowerSelectEntry.Status.Unavailable;
        this.AddFollowerEntry(this.FollowerSelectEntries[shownFollowersCount]);
      }
      for (int shownFollowersCount = this.shownFollowersCount; shownFollowersCount < this._followerInfoBoxes.Count; ++shownFollowersCount)
        this._followerInfoBoxes[shownFollowersCount].gameObject.SetActive(true);
      if (this._unavailableContentContainer.childCount != 0)
      {
        this._availableHeaderContainer.SetActive(true);
        this._unavailableHeaderContainer.SetActive(true);
      }
      if ((UnityEngine.Object) this.overrideSelectable == (UnityEngine.Object) null)
        this.overrideSelectable = this._followerInfoBoxes[0].Button;
      this.OverrideDefault((Selectable) this.overrideSelectable);
      UIFollowerSelectMenuController componentInParent = this.GetComponentInParent<UIFollowerSelectMenuController>();
      if (TwitchAuthentication.IsAuthenticated && !TwitchVoting.Deactivated && this.AllowsVoting && (UnityEngine.Object) componentInParent != (UnityEngine.Object) null && componentInParent.ProvideInfo().Count > 0)
      {
        TwitchInformationBox twitchInformationBox = MonoSingleton<UIManager>.Instance.TwitchInformationBox.Spawn<TwitchInformationBox>((Transform) this._contentContainer);
        twitchInformationBox.transform.localScale = Vector3.one;
        twitchInformationBox.OnFollowerSelected = twitchInformationBox.OnFollowerSelected + new Action<FollowerInfo>(this.FollowerSelected);
        twitchInformationBox.Button.SetInteractionState(true);
        twitchInformationBox.Button._vibrateOnConfirm = false;
        twitchInformationBox.transform.SetAsFirstSibling();
        twitchInformationBox.Configure((ITwitchVotingProvider) componentInParent);
      }
    }
    this.ActivateNavigation();
    this._sortingDropdown.Interactable = true;
    this.OnHide = this.OnHide + new System.Action(this.CallOnHide);
  }

  public void AddFollowerEntry(FollowerSelectEntry followerSelectEntry)
  {
    T obj;
    if (followerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
    {
      obj = this.PrefabTemplate().Spawn<T>((Transform) this._availableContentContainer, false);
      if ((UnityEngine.Object) this.overrideSelectable == (UnityEngine.Object) null)
        this.overrideSelectable = obj.Button;
      obj.Button.Confirmable = obj.Button.Interactable = true;
    }
    else
      obj = this.PrefabTemplate().Spawn<T>((Transform) this._unavailableContentContainer, false);
    obj.transform.localScale = Vector3.one;
    obj.Configure(followerSelectEntry);
    // ISSUE: variable of a boxed type
    __Boxed<T> local1 = (object) obj;
    local1.OnFollowerSelected = local1.OnFollowerSelected + new Action<FollowerInfo>(this.FollowerSelected);
    // ISSUE: variable of a boxed type
    __Boxed<T> local2 = (object) obj;
    local2.OnFollowerHighlighted = local2.OnFollowerHighlighted + this.OnFollowerHighlighted;
    obj.Button.SetInteractionState(true);
    obj.Button._vibrateOnConfirm = false;
    this._followerInfoBoxes.Add(obj);
  }

  public void UpdateSelections()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (T followerInfoBox in this.FollowerInfoBoxes)
    {
      if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available && (UnityEngine.Object) followerInfoBox.transform.parent != (UnityEngine.Object) this._availableContentContainer)
        followerInfoBox.transform.parent = (Transform) this._availableContentContainer;
      else if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus != FollowerSelectEntry.Status.Available && (UnityEngine.Object) followerInfoBox.transform.parent == (UnityEngine.Object) this._availableContentContainer)
        followerInfoBox.transform.parent = (Transform) this._unavailableContentContainer;
      followerInfoBox.Configure(followerInfoBox.FollowerSelectEntry);
      if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
        ++num1;
      else
        ++num2;
    }
    this._availableCount.text = $"{(object) num1}/{(object) this.FollowerSelectEntries.Count}";
    this._unavailableCount.text = $"{(object) num2}/{(object) this.FollowerSelectEntries.Count}";
  }

  public virtual void OnSortingMethodChanged(int sortMethod)
  {
    MMButton mmButton = (MMButton) null;
    if (this._currentSortingMethod == sortMethod)
      return;
    this._currentSortingMethod = sortMethod;
    switch (sortMethod)
    {
      case 0:
        this._followerInfoBoxes = this._followerInfoBoxes.OrderBy<T, int>((Func<T, int>) (x => this.FollowerSelectEntries.IndexOf(x.FollowerSelectEntry))).ToList<T>();
        break;
      case 1:
        this._followerInfoBoxes = this._followerInfoBoxes.OrderBy<T, string>((Func<T, string>) (x => x.FollowerInfo.Name)).ToList<T>();
        break;
      case 2:
        this._followerInfoBoxes = this._followerInfoBoxes.OrderBy<T, int>((Func<T, int>) (x => x.FollowerInfo.Age)).ToList<T>();
        this._followerInfoBoxes.Reverse();
        break;
      case 3:
        this._followerInfoBoxes = this._followerInfoBoxes.OrderBy<T, int>((Func<T, int>) (x => x.FollowerInfo.XPLevel)).ToList<T>();
        this._followerInfoBoxes.Reverse();
        break;
      case 4:
        this._followerInfoBoxes = this._followerInfoBoxes.OrderBy<T, int>((Func<T, int>) (x => x.FollowerInfo.Pleasure)).ToList<T>();
        this._followerInfoBoxes.Reverse();
        break;
    }
    for (int index = 0; index < this._followerInfoBoxes.Count; ++index)
    {
      this._followerInfoBoxes[index].transform.SetSiblingIndex(index);
      if ((UnityEngine.Object) mmButton == (UnityEngine.Object) null && this._followerInfoBoxes[index].FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
        mmButton = this._followerInfoBoxes[index].Button;
    }
    this._scrollRect.normalizedPosition = Vector2.zero;
    if ((UnityEngine.Object) mmButton == (UnityEngine.Object) null && this._followerInfoBoxes.Count > 0)
      mmButton = this._followerInfoBoxes[0].Button;
    this.OverrideDefaultOnce((Selectable) mmButton);
  }

  public abstract T PrefabTemplate();

  public virtual void FollowerSelected(FollowerInfo followerInfo)
  {
    for (int index = this._followerInfoBoxes.Count - 1; index >= 0; --index)
    {
      if (this._followerInfoBoxes[index].FollowerInfo.ID == followerInfo.ID)
      {
        Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
        if (followerSelected != null)
          followerSelected(followerInfo);
        if (this._hideOnSelection)
          this.Hide();
      }
    }
  }

  public void CallOnHide() => this.StartCoroutine((IEnumerator) this.HideFollowersRoutine());

  public IEnumerator HideFollowersRoutine()
  {
    UIFollowerSelectBase<T> followerSelectBase = this;
    yield return (object) null;
    foreach (T followerInfoBox in followerSelectBase._followerInfoBoxes)
    {
      // ISSUE: variable of a boxed type
      __Boxed<T> local1 = (object) followerInfoBox;
      local1.OnFollowerSelected = local1.OnFollowerSelected - new Action<FollowerInfo>(followerSelectBase.FollowerSelected);
      // ISSUE: variable of a boxed type
      __Boxed<T> local2 = (object) followerInfoBox;
      local2.OnFollowerHighlighted = local2.OnFollowerHighlighted - followerSelectBase.OnFollowerHighlighted;
      followerInfoBox.Recycle<T>();
    }
  }

  public void ShowCustomAcceptTerm(string term) => this._controlPrompts.UpdateAcceptTerm(term);

  public void HideCustomAcceptTerm() => this._controlPrompts.UpdateAcceptTerm("UI/Generic/Accept");

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    this.OnHide = this.OnHide - new System.Action(this.CallOnHide);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable || !this._cancellable)
      return;
    UIManager.PlayAudio("event:/ui/go_back");
    this._didCancel = true;
    this.Hide();
  }

  public virtual TwitchVoting.VotingType VotingType
  {
    get => this.\u003CVotingType\u003Ek__BackingField;
    set => this.\u003CVotingType\u003Ek__BackingField = value;
  }

  public virtual bool AllowsVoting
  {
    get => this.\u003CAllowsVoting\u003Ek__BackingField;
    set => this.\u003CAllowsVoting\u003Ek__BackingField = value;
  }

  public virtual List<FollowerInfo> ProvideInfo()
  {
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>();
    foreach (FollowerSelectEntry followerSelectEntry in this.FollowerSelectEntries)
    {
      if (followerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
        followerInfoList.Add(followerSelectEntry.FollowerInfo);
    }
    return followerInfoList;
  }

  public virtual void FinalizeVote(FollowerInfo followerInfo)
  {
    Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
    if (followerSelected != null)
      followerSelected(followerInfo);
    if (this.VotingType == TwitchVoting.VotingType.MATING)
      return;
    this.Hide();
  }

  public FollowerSelectEntry GetEntryFromFollower(int followerID)
  {
    foreach (FollowerSelectEntry followerSelectEntry in this.FollowerSelectEntries)
    {
      if (followerSelectEntry.FollowerInfo.ID == followerID)
        return followerSelectEntry;
    }
    return (FollowerSelectEntry) null;
  }

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__34_2()
  {
    if (!((UnityEngine.Object) this._controlPrompts != (UnityEngine.Object) null))
      return;
    this._controlPrompts.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__34_3()
  {
    if (!((UnityEngine.Object) this._controlPrompts != (UnityEngine.Object) null))
      return;
    this._controlPrompts.gameObject.SetActive(true);
  }

  [CompilerGenerated]
  public int \u003COnSortingMethodChanged\u003Eb__38_0(T x)
  {
    return this.FollowerSelectEntries.IndexOf(x.FollowerSelectEntry);
  }
}
