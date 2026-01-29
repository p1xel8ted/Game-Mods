// Decompiled with JetBrains decompiler
// Type: UIExhumeSpiritsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine.Unity;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIExhumeSpiritsMenuController : UIFollowerSelectMenuController
{
  [SerializeField]
  public MMScrollRect scrollRect;
  [SerializeField]
  public TMP_Text text;
  [SerializeField]
  public SkeletonGraphic[] spines;
  [SerializeField]
  public MMButton button;
  public List<FollowerInfo> followersSelected = new List<FollowerInfo>();
  public Action<List<FollowerInfo>> OnFollowersChosen;

  public void Start()
  {
    foreach (Graphic spine in this.spines)
      spine.color = Color.black;
    this.OnFollowerHighlighted = this.OnFollowerHighlighted + new Action<FollowerInfo>(this.EnableScrollRect);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.OnFollowerHighlighted = this.OnFollowerHighlighted - new Action<FollowerInfo>(this.EnableScrollRect);
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.button != (UnityEngine.Object) null)
      this.button.transform.parent.gameObject.SetActive(this.followersSelected.Count > 0);
    this.text.text = this.followersSelected.Count > 1 ? LocalizationManager.GetTranslation("UI/ReleaseSpirits") : LocalizationManager.GetTranslation("UI/ReleaseSpirit");
  }

  public override void FollowerSelected(FollowerInfo followerInfo)
  {
    if (this.followersSelected.Count >= 5 && !this.followersSelected.Contains(followerInfo))
    {
      foreach (FollowerInformationBox followerInfoBox in this.FollowerInfoBoxes)
      {
        if (followerInfoBox.FollowerInfo == this.followersSelected[0])
          followerInfoBox.RemoveChosen();
      }
      this.followersSelected.RemoveAt(0);
    }
    if (this.followersSelected.Contains(followerInfo))
      this.followersSelected.Remove(followerInfo);
    else
      this.followersSelected.Add(followerInfo);
    foreach (FollowerInformationBox followerInfoBox in this.FollowerInfoBoxes)
    {
      if (followerInfoBox.FollowerInfo == followerInfo)
      {
        if (this.followersSelected.Contains(followerInfo))
          followerInfoBox.SetChosen();
        else
          followerInfoBox.RemoveChosen();
      }
    }
    foreach (Graphic spine in this.spines)
      spine.color = Color.black;
    for (int index = 0; index < this.followersSelected.Count; ++index)
    {
      this.spines[index].color = Color.white;
      this.spines[index].ConfigureFollower(this.followersSelected[index]);
    }
    if (InputManager.General.MouseInputActive || this.followersSelected.Count < 5)
      return;
    this.DisableScrollRect();
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.button);
  }

  public override void OnShowFinished()
  {
    base.OnShowFinished();
    foreach (FollowerInformationBox followerInfoBox in this.FollowerInfoBoxes)
      followerInfoBox.EnableChosen();
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    foreach (FollowerInformationBox followerInfoBox in this.FollowerInfoBoxes)
      followerInfoBox.EnableChosen();
  }

  public override void OnCancelButtonInput()
  {
    if (!this.scrollRect.enabled && this.CanvasGroup.interactable)
    {
      this.EnableScrollRect((FollowerInfo) null);
      if (this.followersSelected.Count <= 0)
        return;
      for (int index = 0; index < this.FollowerInfoBoxes.Count; ++index)
      {
        if (this.FollowerInfoBoxes[index].FollowerInfo == this.followersSelected[this.followersSelected.Count - 1])
          MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.FollowerInfoBoxes[index].Button);
      }
      this.FollowerSelected(this.followersSelected[this.followersSelected.Count - 1]);
    }
    else
      base.OnCancelButtonInput();
  }

  public void FollowersChosen()
  {
    Action<List<FollowerInfo>> onFollowersChosen = this.OnFollowersChosen;
    if (onFollowersChosen != null)
      onFollowersChosen(this.followersSelected);
    this.Hide();
  }

  public void DisableScrollRect()
  {
    if ((UnityEngine.Object) this.scrollRect != (UnityEngine.Object) null)
      this.scrollRect.enabled = false;
    for (int index = 0; index < this.FollowerInfoBoxes.Count; ++index)
      this.FollowerInfoBoxes[index].Button.Interactable = false;
  }

  public void EnableScrollRect(FollowerInfo info)
  {
    if ((UnityEngine.Object) this.scrollRect != (UnityEngine.Object) null)
      this.scrollRect.enabled = true;
    for (int index = 0; index < this.FollowerInfoBoxes.Count; ++index)
      this.FollowerInfoBoxes[index].Button.Interactable = true;
  }
}
