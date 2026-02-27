// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerSelect.UIFollowerSelectBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.FollowerSelect;

public abstract class UIFollowerSelectBase<T> : UIMenuBase where T : FollowerSelectItem
{
  public Action<FollowerInfo> OnFollowerSelected;
  [Header("Follower Select")]
  [SerializeField]
  protected RectTransform _contentContainer;
  [SerializeField]
  private GameObject _noFollowerText;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  protected UIMenuControlPrompts _controlPrompts;
  private List<FollowerInfo> _followerInfo = new List<FollowerInfo>();
  protected List<T> _followerInfoBoxes = new List<T>();
  protected bool _didCancel;
  protected bool _hideOnSelection;
  protected bool _cancellable;
  protected bool _hasSelection;

  public List<T> FollowerInfoBoxes => this._followerInfoBoxes;

  public void Show(
    List<int> followerIDS,
    List<int> blackList = null,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    List<FollowerInfo> followerInfo = new List<FollowerInfo>();
    foreach (int ID in followerIDS)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(ID);
      followerInfo.Add(infoById);
    }
    List<FollowerInfo> blackList1 = new List<FollowerInfo>();
    if (blackList != null)
    {
      foreach (int black in blackList)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(black);
        blackList1.Add(infoById);
      }
    }
    this.Show(followerInfo, blackList1, instant, hideOnSelection, cancellable);
  }

  public void Show(
    List<FollowerBrain> followerBrains,
    List<FollowerBrain> blackList = null,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    List<FollowerInfo> followerInfo = new List<FollowerInfo>();
    foreach (FollowerBrain followerBrain in followerBrains)
      followerInfo.Add(followerBrain._directInfoAccess);
    List<FollowerInfo> blackList1 = new List<FollowerInfo>();
    if (blackList != null)
    {
      foreach (FollowerBrain black in blackList)
        blackList1.Add(black._directInfoAccess);
    }
    this.Show(followerInfo, blackList1, instant, hideOnSelection, cancellable);
  }

  public void Show(
    List<Follower> followers,
    List<Follower> blackList = null,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    List<FollowerInfo> followerInfo = new List<FollowerInfo>();
    foreach (Follower follower in followers)
      followerInfo.Add(follower.Brain._directInfoAccess);
    List<FollowerInfo> blackList1 = new List<FollowerInfo>();
    if (blackList != null)
    {
      foreach (Follower black in blackList)
        blackList1.Add(black.Brain._directInfoAccess);
    }
    this.Show(followerInfo, blackList1, instant, hideOnSelection, cancellable);
  }

  public void Show(
    List<SimFollower> simFollowers,
    List<SimFollower> blackList = null,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    List<FollowerInfo> followerInfo = new List<FollowerInfo>();
    foreach (SimFollower simFollower in simFollowers)
      followerInfo.Add(simFollower.Brain._directInfoAccess);
    List<FollowerInfo> blackList1 = new List<FollowerInfo>();
    if (blackList != null)
    {
      foreach (SimFollower black in blackList)
        blackList1.Add(black.Brain._directInfoAccess);
    }
    this.Show(followerInfo, blackList1, instant, hideOnSelection, cancellable);
  }

  public void Show(
    List<FollowerInfo> followerInfo,
    List<FollowerInfo> blackList = null,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true)
  {
    if (blackList == null)
      blackList = new List<FollowerInfo>();
    foreach (FollowerInfo followerInfo1 in followerInfo)
    {
      if (!blackList.Contains(followerInfo1))
        this._followerInfo.Add(followerInfo1);
    }
    this._hideOnSelection = hideOnSelection;
    this._cancellable = cancellable;
    this._hasSelection = hasSelection;
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    if (!this._hasSelection)
      this._controlPrompts.HideAcceptButton();
    this._scrollRect.enabled = false;
    this._noFollowerText.SetActive(this._followerInfo.Count == 0);
    if (!this._cancellable && (UnityEngine.Object) this._controlPrompts != (UnityEngine.Object) null)
      this._controlPrompts.HideCancelButton();
    if (this._followerInfo.Count > 0)
    {
      foreach (FollowerInfo followerInfo in this._followerInfo)
      {
        T obj = this.PrefabTemplate().Spawn<T>((Transform) this._contentContainer);
        obj.transform.localScale = Vector3.one;
        obj.Configure(followerInfo);
        // ISSUE: variable of a boxed type
        __Boxed<T> local = (object) obj;
        local.OnFollowerSelected = local.OnFollowerSelected + new Action<FollowerInfo>(this.FollowerSelected);
        obj.Button.SetInteractionState(true);
        obj.Button._vibrateOnConfirm = false;
        this._followerInfoBoxes.Add(obj);
      }
      this.OverrideDefault((Selectable) this._followerInfoBoxes[0].Button);
      this.ActivateNavigation();
    }
    this._scrollRect.enabled = true;
    this._scrollRect.normalizedPosition = Vector2.one;
  }

  protected abstract T PrefabTemplate();

  protected virtual void FollowerSelected(FollowerInfo followerInfo)
  {
    foreach (T followerInfoBox in this._followerInfoBoxes)
    {
      if (followerInfoBox.FollowerInfo.ID == followerInfo.ID)
      {
        Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
        if (followerSelected != null)
          followerSelected(followerInfo);
        if (this._hideOnSelection)
          this.Hide();
      }
    }
  }

  protected override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    foreach (T followerInfoBox in this._followerInfoBoxes)
      followerInfoBox.Recycle<T>();
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
}
