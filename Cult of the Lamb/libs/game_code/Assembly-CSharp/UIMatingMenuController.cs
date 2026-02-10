// Decompiled with JetBrains decompiler
// Type: UIMatingMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UIMatingMenuController : UIMenuBase
{
  [SerializeField]
  public UIFollowerSelectMenuController followerSelectMenu;
  [SerializeField]
  public UIMatingInfoCard matingInfoCard;
  [SerializeField]
  public MMScrollRect scrollRect;
  [SerializeField]
  public Image fade;
  [SerializeField]
  public TMP_Text backPromptText;
  [SerializeField]
  public MMButton mateButton;
  [SerializeField]
  public GameObject mateButtonContainer;
  [SerializeField]
  public ButtonHighlightController _highlightController;
  public FollowerInfo follower1;
  public FollowerInfo follower2;
  public FollowerInfo highlightedFollower;
  public Interaction_MatingTent matingTent;
  public Action<FollowerInfo, FollowerInfo> OnFollowersChosen;

  public void Show(
    Interaction_MatingTent matingTent,
    List<FollowerSelectEntry> followerSelectEntries)
  {
    this.Show();
    this.matingTent = matingTent;
    this.followerSelectMenu.Show(followerSelectEntries, followerSelectionType: UpgradeSystem.Type.Building_MatingTent, hideOnSelection: false, cancellable: false);
    this.followerSelectMenu.VotingType = TwitchVoting.VotingType.MATING;
    UIFollowerSelectMenuController followerSelectMenu1 = this.followerSelectMenu;
    followerSelectMenu1.OnFollowerSelected = followerSelectMenu1.OnFollowerSelected + new Action<FollowerInfo>(this.OnFollowerSelected);
    UIFollowerSelectMenuController followerSelectMenu2 = this.followerSelectMenu;
    followerSelectMenu2.OnFollowerHighlighted = followerSelectMenu2.OnFollowerHighlighted + new Action<FollowerInfo>(this.OnFollowerHighlighted);
    UIFollowerSelectMenuController followerSelectMenu3 = this.followerSelectMenu;
    followerSelectMenu3.OnShow = followerSelectMenu3.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in this.followerSelectMenu.FollowerInfoBoxes)
      {
        if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
          followerInfoBox.EnableChosen();
      }
    });
    UIFollowerSelectMenuController followerSelectMenu4 = this.followerSelectMenu;
    followerSelectMenu4.OnShownCompleted = followerSelectMenu4.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in this.followerSelectMenu.FollowerInfoBoxes)
      {
        if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
          followerInfoBox.EnableChosen();
        FollowerInformationBox followerInformationBox = followerInfoBox;
        followerInformationBox.OnFollowerHighlighted = followerInformationBox.OnFollowerHighlighted + new Action<FollowerInfo>(this.EnableScrollRect);
      }
    });
    this.matingInfoCard.Configure(matingTent, (FollowerInfo) null, (FollowerInfo) null, this);
    this.scrollRect.enabled = true;
    this.mateButtonContainer.SetActive(false);
    this.mateButton.onClick.AddListener(new UnityAction(this.ConfirmMatingButtonPress));
    this.mateButton.OnSelected += new System.Action(this.OnMateButtonSelected);
    this.mateButton.OnDeselected += new System.Action(this.OnMateButtonDeselected);
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    foreach (FollowerInformationBox followerInfoBox in this.followerSelectMenu.FollowerInfoBoxes)
      followerInfoBox.OnFollowerHighlighted = followerInfoBox.OnFollowerHighlighted - new Action<FollowerInfo>(this.EnableScrollRect);
    this.mateButton.onClick.RemoveListener(new UnityAction(this.ConfirmMatingButtonPress));
    this.mateButton.OnSelected -= new System.Action(this.OnMateButtonSelected);
    this.mateButton.OnDeselected -= new System.Action(this.OnMateButtonDeselected);
  }

  public void EnableScrollRect(FollowerInfo info)
  {
    if ((UnityEngine.Object) this.scrollRect != (UnityEngine.Object) null)
      this.scrollRect.enabled = true;
    for (int index = 0; index < this.followerSelectMenu.FollowerInfoBoxes.Count; ++index)
      this.followerSelectMenu.FollowerInfoBoxes[index].Button.Interactable = true;
  }

  public void DisableScrollRect()
  {
    if ((UnityEngine.Object) this.scrollRect != (UnityEngine.Object) null)
      this.scrollRect.enabled = false;
    for (int index = 0; index < this.followerSelectMenu.FollowerInfoBoxes.Count; ++index)
      this.followerSelectMenu.FollowerInfoBoxes[index].Button.Interactable = this.followerSelectMenu.FollowerInfoBoxes[index].FollowerInfo == this.follower1 || this.followerSelectMenu.FollowerInfoBoxes[index].FollowerInfo == this.follower2 || this.followerSelectMenu.FollowerInfoBoxes[index].FollowerInfo == this.highlightedFollower;
  }

  public void OnFollowerSelected(FollowerInfo follower)
  {
    if (this.follower1 != null && follower == this.follower1)
    {
      this.matingInfoCard.Configure(this.matingTent, follower, this.follower2, this, true);
      this.follower1 = (FollowerInfo) null;
    }
    else if (this.follower2 != null && follower == this.follower2)
    {
      this.matingInfoCard.Configure(this.matingTent, this.follower1, follower, this, fadeFollower2: true);
      this.follower2 = (FollowerInfo) null;
    }
    else if (this.follower1 == null)
    {
      this.follower1 = follower;
      this.matingInfoCard.Configure(this.matingTent, this.follower1, this.follower2, this);
    }
    else if (this.follower2 == null)
    {
      this.follower2 = follower;
      this.matingInfoCard.Configure(this.matingTent, this.follower1, this.follower2, this);
    }
    else
    {
      this.follower2 = follower;
      this.matingInfoCard.Configure(this.matingTent, this.follower1, this.follower2, this);
    }
    if (this.follower1 != null && this.follower2 != null)
    {
      if (!this.mateButtonContainer.activeSelf)
      {
        this.mateButtonContainer.SetActive(true);
        this.mateButtonContainer.transform.localScale = Vector3.one;
        this.mateButtonContainer.transform.DOKill();
        this.mateButtonContainer.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f).SetUpdate<Tweener>(true);
        if (!InputManager.General.MouseInputActive)
        {
          this.DisableScrollRect();
          MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.mateButton);
        }
      }
    }
    else
      this.mateButtonContainer.SetActive(false);
    this.UpdateCheckboxes();
  }

  public void OnFollowerHighlighted(FollowerInfo follower)
  {
    if (this.follower1 == null)
    {
      if (this.followerSelectMenu.GetEntryFromFollower(follower.ID).AvailabilityStatus == FollowerSelectEntry.Status.Available)
        this.matingInfoCard.Configure(this.matingTent, follower, this.follower2, this, true);
      else
        this.matingInfoCard.Configure(this.matingTent, (FollowerInfo) null, this.follower2, this);
    }
    else if (this.follower2 == null)
    {
      if (follower == this.follower1 && this.follower1 != null)
        this.matingInfoCard.Configure(this.matingTent, this.follower1, (FollowerInfo) null, this);
      else if (this.followerSelectMenu.GetEntryFromFollower(follower.ID).AvailabilityStatus == FollowerSelectEntry.Status.Available)
        this.matingInfoCard.Configure(this.matingTent, this.follower1, follower, this, fadeFollower2: true);
      else
        this.matingInfoCard.Configure(this.matingTent, this.follower1, (FollowerInfo) null, this);
    }
    this.highlightedFollower = follower;
    this.UpdateCheckboxes();
  }

  public void UpdateCheckboxes()
  {
    foreach (FollowerInformationBox followerInfoBox in this.followerSelectMenu.FollowerInfoBoxes)
    {
      if (followerInfoBox.FollowerInfo == this.follower1 || followerInfoBox.FollowerInfo == this.follower2)
        followerInfoBox.SetChosen();
      else
        followerInfoBox.RemoveChosen();
    }
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.EnableScrollRect((FollowerInfo) null);
    this.mateButtonContainer.SetActive(false);
    if (this.follower2 != null)
    {
      if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null && !(bool) (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponentInParent<FollowerInformationBox>())
      {
        for (int index = 0; index < this.followerSelectMenu.FollowerInfoBoxes.Count; ++index)
        {
          if (this.followerSelectMenu.FollowerInfoBoxes[index].FollowerInfo == this.follower2)
            MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.followerSelectMenu.FollowerInfoBoxes[index].Button);
        }
      }
      this.matingInfoCard.Configure(this.matingTent, this.follower1, this.follower2, this, fadeFollower2: true);
      this.follower2 = (FollowerInfo) null;
    }
    else if (this.follower1 != null)
    {
      if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null && !(bool) (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponentInParent<FollowerInformationBox>())
      {
        for (int index = 0; index < this.followerSelectMenu.FollowerInfoBoxes.Count; ++index)
        {
          if (this.followerSelectMenu.FollowerInfoBoxes[index].FollowerInfo == this.follower1)
            MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.followerSelectMenu.FollowerInfoBoxes[index].Button);
        }
      }
      this.matingInfoCard.Configure(this.matingTent, this.follower1, (FollowerInfo) null, this, true);
      this.follower1 = (FollowerInfo) null;
    }
    else if (this.follower1 == null)
    {
      base.OnCancelButtonInput();
      UIFollowerSelectMenuController followerSelectMenu = this.followerSelectMenu;
      followerSelectMenu.OnHidden = followerSelectMenu.OnHidden + (System.Action) (() => this.Hide(true));
      this.followerSelectMenu.Hide();
    }
    this.UpdateCheckboxes();
  }

  public void OnMateButtonSelected()
  {
    this._highlightController.Image.color = new Color(1f, 1f, 1f, 1f);
    this._highlightController.transform.DOKill();
    this._highlightController.transform.DOShakeScale(0.05f, new Vector3(-0.05f, 0.05f, 1f), 3, fadeOut: false).SetUpdate<Tweener>(true);
  }

  public void OnMateButtonDeselected()
  {
    this._highlightController.Image.color = new Color(0.0f, 0.5f, 1f, 1f);
  }

  public void ConfirmMatingButtonPress()
  {
    Action<FollowerInfo, FollowerInfo> onFollowersChosen = this.OnFollowersChosen;
    if (onFollowersChosen != null)
      onFollowersChosen(this.follower1, this.follower2);
    UIFollowerSelectMenuController followerSelectMenu = this.followerSelectMenu;
    followerSelectMenu.OnHidden = followerSelectMenu.OnHidden + (System.Action) (() => this.Hide(true));
    this.followerSelectMenu.Hide();
  }

  public override void OnHideCompleted()
  {
    this.mateButton.onClick.RemoveListener(new UnityAction(this.ConfirmMatingButtonPress));
    this.mateButton.OnSelected -= new System.Action(this.OnMateButtonSelected);
    this.mateButton.OnDeselected -= new System.Action(this.OnMateButtonDeselected);
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__13_0()
  {
    foreach (FollowerInformationBox followerInfoBox in this.followerSelectMenu.FollowerInfoBoxes)
    {
      if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
        followerInfoBox.EnableChosen();
    }
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__13_1()
  {
    foreach (FollowerInformationBox followerInfoBox in this.followerSelectMenu.FollowerInfoBoxes)
    {
      if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
        followerInfoBox.EnableChosen();
      FollowerInformationBox followerInformationBox = followerInfoBox;
      followerInformationBox.OnFollowerHighlighted = followerInformationBox.OnFollowerHighlighted + new Action<FollowerInfo>(this.EnableScrollRect);
    }
  }

  [CompilerGenerated]
  public void \u003COnCancelButtonInput\u003Eb__20_0() => this.Hide(true);

  [CompilerGenerated]
  public void \u003CConfirmMatingButtonPress\u003Eb__23_0() => this.Hide(true);
}
