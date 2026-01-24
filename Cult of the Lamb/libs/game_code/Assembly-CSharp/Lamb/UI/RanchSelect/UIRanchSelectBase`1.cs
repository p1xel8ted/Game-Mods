// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchSelect.UIRanchSelectBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.RanchSelect;

public abstract class UIRanchSelectBase<T> : UIMenuBase where T : RanchSelectItem
{
  public Action<RanchSelectEntry> OnAnimalSelected;
  public Action<RanchSelectEntry> OnAnimalHighlighted;
  public System.Action OnEditPosts;
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
  public GameObject _capacityHeaderContainer;
  [SerializeField]
  public TMP_Text _capacityCount;
  [SerializeField]
  public GameObject _noFollowerText;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [SerializeField]
  public MMSelectable_Dropdown _sortingDropdown;
  [CompilerGenerated]
  public List<RanchSelectEntry> \u003CFollowerSelectEntries\u003Ek__BackingField;
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
  public int _capacity;
  public MMButton overrideSelectable;
  public bool forSelection;

  public List<RanchSelectEntry> FollowerSelectEntries
  {
    set => this.\u003CFollowerSelectEntries\u003Ek__BackingField = value;
    get => this.\u003CFollowerSelectEntries\u003Ek__BackingField;
  }

  public List<T> FollowerInfoBoxes => this._followerInfoBoxes;

  public override void Awake() => base.Awake();

  public void Show(
    List<RanchSelectEntry> followerSelectEntries,
    int capacity,
    bool instant,
    bool hideOnSelection = false,
    bool cancellable = true,
    bool hasSelection = true)
  {
    foreach (RanchSelectEntry followerSelectEntry in followerSelectEntries)
      Debug.Log((object) ("FollowerSelectEntry: " + followerSelectEntry.AnimalInfo.Type.ToString()));
    Debug.Log((object) ("FollowerSelectEntries: " + followerSelectEntries.Count.ToString()));
    this.FollowerSelectEntries = followerSelectEntries;
    this._hideOnSelection = hideOnSelection;
    this._cancellable = cancellable;
    this._hasSelection = hasSelection;
    this._capacity = capacity;
    this._scrollRect.normalizedPosition = Vector2.one;
    this.Show(instant);
  }

  public void Show(
    List<RanchSelectEntry> followerSelectEntries,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    foreach (RanchSelectEntry followerSelectEntry in followerSelectEntries)
      Debug.Log((object) ("FollowerSelectEntry: " + followerSelectEntry.AnimalInfo.Type.ToString()));
    Debug.Log((object) ("FollowerSelectEntries: " + followerSelectEntries.Count.ToString()));
    this.forSelection = true;
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
    if (this.FollowerSelectEntries.Count > 0)
    {
      this.shownFollowersCount = 0;
      this.availableCount = 0;
      this.unavailableCount = 0;
      this.overrideSelectable = (MMButton) null;
      this._availableContentContainer.gameObject.SetActive(false);
      this.FollowerSelectEntries = this.FollowerSelectEntries.OrderBy<RanchSelectEntry, int>((Func<RanchSelectEntry, int>) (f => f.AvailabilityStatus != RanchSelectEntry.Status.Available ? 1 : 0)).ToList<RanchSelectEntry>();
      for (int index = 0; index < this.FollowerSelectEntries.Count; ++index)
      {
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
      this.availableCount = this.FollowerSelectEntries.FindAll((Predicate<RanchSelectEntry>) (f => f.AvailabilityStatus == RanchSelectEntry.Status.Available)).Count;
      this.unavailableCount = this.FollowerSelectEntries.Count - this.availableCount;
      if ((bool) (UnityEngine.Object) this._capacityHeaderContainer)
        this._capacityCount.text = $"{(object) this.FollowerSelectEntries.Count}/{(object) this._capacity}";
      if ((bool) (UnityEngine.Object) this._availableHeaderContainer)
        this._availableCount.text = $"{(object) this.availableCount}/{(object) this.FollowerSelectEntries.Count}";
      if ((bool) (UnityEngine.Object) this._unavailableHeaderContainer)
        this._unavailableCount.text = $"{(object) this.unavailableCount}/{(object) this.FollowerSelectEntries.Count}";
      this._availableContentContainer.gameObject.SetActive(true);
      if (this._unavailableContentContainer.childCount == 0 && (bool) (UnityEngine.Object) this._unavailableHeaderContainer && this.FollowerSelectEntries[this.FollowerSelectEntries.Count - 1].AvailabilityStatus == RanchSelectEntry.Status.Available)
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
        this.AddFollowerEntry(this.FollowerSelectEntries[shownFollowersCount]);
      for (int shownFollowersCount = this.shownFollowersCount; shownFollowersCount < this._followerInfoBoxes.Count; ++shownFollowersCount)
        this._followerInfoBoxes[shownFollowersCount].gameObject.SetActive(true);
      if (this._unavailableContentContainer.childCount != 0)
      {
        if ((bool) (UnityEngine.Object) this._availableHeaderContainer)
          this._availableHeaderContainer.SetActive(true);
        if ((bool) (UnityEngine.Object) this._unavailableHeaderContainer)
          this._unavailableHeaderContainer.SetActive(true);
        if ((bool) (UnityEngine.Object) this._capacityHeaderContainer)
          this._capacityHeaderContainer.SetActive(true);
      }
      if ((UnityEngine.Object) this.overrideSelectable == (UnityEngine.Object) null)
        this.overrideSelectable = this._followerInfoBoxes[0].Button;
      this.OverrideDefault((Selectable) this.overrideSelectable);
    }
    this.ActivateNavigation();
    this.OnHide = this.OnHide + new System.Action(this.CallOnHide);
  }

  public virtual void AddFollowerEntry(RanchSelectEntry followerSelectEntry)
  {
    T obj;
    if (followerSelectEntry.AvailabilityStatus == RanchSelectEntry.Status.Available)
    {
      obj = this.PrefabTemplate().Spawn<T>((Transform) this._availableContentContainer, false);
      if ((UnityEngine.Object) this.overrideSelectable == (UnityEngine.Object) null)
        this.overrideSelectable = obj.Button;
    }
    else
      obj = this.PrefabTemplate().Spawn<T>((Transform) this._unavailableContentContainer, false);
    obj.transform.localScale = Vector3.one;
    obj.Configure(followerSelectEntry, true);
    // ISSUE: variable of a boxed type
    __Boxed<T> local1 = (object) obj;
    local1.OnFollowerSelected = local1.OnFollowerSelected + new Action<RanchSelectEntry>(this.FollowerSelected);
    // ISSUE: variable of a boxed type
    __Boxed<T> local2 = (object) obj;
    local2.OnFollowerHighlighted = local2.OnFollowerHighlighted + this.OnAnimalHighlighted;
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
      if (followerInfoBox.RanchSelectEntry.AvailabilityStatus == RanchSelectEntry.Status.Available && (UnityEngine.Object) followerInfoBox.transform.parent != (UnityEngine.Object) this._availableContentContainer)
        followerInfoBox.transform.parent = (Transform) this._availableContentContainer;
      else if (followerInfoBox.RanchSelectEntry.AvailabilityStatus != RanchSelectEntry.Status.Available && (UnityEngine.Object) followerInfoBox.transform.parent == (UnityEngine.Object) this._availableContentContainer)
        followerInfoBox.transform.parent = (Transform) this._unavailableContentContainer;
      followerInfoBox.Configure(followerInfoBox.RanchSelectEntry, true);
      if (followerInfoBox.RanchSelectEntry.AvailabilityStatus == RanchSelectEntry.Status.Available)
        ++num1;
      else
        ++num2;
    }
    if ((bool) (UnityEngine.Object) this._capacityHeaderContainer)
      this._capacityCount.text = $"{(object) this.FollowerSelectEntries.Count}/{(object) this._capacity}";
    if ((bool) (UnityEngine.Object) this._availableHeaderContainer)
      this._availableCount.text = $"{(object) num1}/{(object) this.FollowerSelectEntries.Count}";
    if (!(bool) (UnityEngine.Object) this._unavailableHeaderContainer)
      return;
    this._unavailableCount.text = $"{(object) num2}/{(object) this.FollowerSelectEntries.Count}";
  }

  public abstract T PrefabTemplate();

  public virtual void FollowerSelected(RanchSelectEntry followerInfo)
  {
    for (int index = this._followerInfoBoxes.Count - 1; index >= 0; --index)
    {
      if (this._followerInfoBoxes[index].AnimalInfo.ID == followerInfo.AnimalInfo.ID)
      {
        Action<RanchSelectEntry> onAnimalSelected = this.OnAnimalSelected;
        if (onAnimalSelected != null)
          onAnimalSelected(followerInfo);
        if (this._hideOnSelection)
          this.Hide();
      }
    }
  }

  public void CallOnHide() => this.StartCoroutine((IEnumerator) this.HideFollowersRoutine());

  public IEnumerator HideFollowersRoutine()
  {
    UIRanchSelectBase<T> uiRanchSelectBase = this;
    yield return (object) null;
    foreach (T followerInfoBox in uiRanchSelectBase._followerInfoBoxes)
    {
      // ISSUE: variable of a boxed type
      __Boxed<T> local1 = (object) followerInfoBox;
      local1.OnFollowerSelected = local1.OnFollowerSelected - new Action<RanchSelectEntry>(uiRanchSelectBase.FollowerSelected);
      // ISSUE: variable of a boxed type
      __Boxed<T> local2 = (object) followerInfoBox;
      local2.OnFollowerHighlighted = local2.OnFollowerHighlighted - uiRanchSelectBase.OnAnimalHighlighted;
      followerInfoBox.Recycle<T>();
    }
  }

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

  public RanchSelectEntry GetRanchSelectEntry(int ID)
  {
    for (int index = 0; index < this.FollowerSelectEntries.Count; ++index)
    {
      if (this.FollowerSelectEntries[index].AnimalInfo.ID == ID)
        return this.FollowerSelectEntries[index];
    }
    return (RanchSelectEntry) null;
  }
}
