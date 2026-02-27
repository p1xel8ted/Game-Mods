// Decompiled with JetBrains decompiler
// Type: UIReapSoulsMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.FollowerSelect;
using Spine.Unity;
using src.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UIReapSoulsMenuController : UIFollowerSelectMenuController
{
  [SerializeField]
  public Image bar;
  [SerializeField]
  public RectTransform blade;
  [SerializeField]
  public GameObject bladeFlame;
  [SerializeField]
  public GameObject sinIcon;
  [SerializeField]
  public GameObject reapButtonContainer;
  [SerializeField]
  public MMButton reapButton;
  [SerializeField]
  public ButtonHighlightController _highlightController;
  [SerializeField]
  public SkeletonGraphic[] spines;
  public List<FollowerInfo> selectedFollowers = new List<FollowerInfo>();
  public Action<List<FollowerInfo>> OnFollowersChosen;

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this.reapButtonContainer.gameObject.SetActive(false);
    foreach (FollowerInformationBox followerInfoBox in this.FollowerInfoBoxes)
    {
      if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
        followerInfoBox.EnableChosen();
    }
    this.UpdateBar();
    this.reapButton.onClick.AddListener(new UnityAction(this.ConfirmReapButton));
    this.reapButton.OnSelected += new System.Action(this.OnReapButtonSelected);
    this.reapButton.OnDeselected += new System.Action(this.OnReapButtonDeselected);
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    this.reapButton.onClick.RemoveAllListeners();
    this.reapButton.OnSelected -= new System.Action(this.OnReapButtonSelected);
    this.reapButton.OnDeselected -= new System.Action(this.OnReapButtonDeselected);
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    foreach (FollowerInformationBox followerInfoBox in this.FollowerInfoBoxes)
    {
      if (followerInfoBox.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
        followerInfoBox.EnableChosen();
    }
  }

  public void UpdateBar()
  {
    float t = (float) this.selectedFollowers.Count / 3f;
    this.bar.fillAmount = t;
    this.blade.DOKill();
    this.blade.DOAnchorPos(Vector2.Lerp(new Vector2(0.0f, 230f), new Vector2(0.0f, 50f), t), 0.25f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    this.sinIcon.transform.DOKill();
    this.sinIcon.transform.localScale = (Vector3) Vector2.one;
    if ((double) t >= 1.0)
      this.sinIcon.transform.DOPunchScale((Vector3) (Vector2.one * 0.2f), 0.2f).SetUpdate<Tweener>(true);
    this.bladeFlame.gameObject.SetActive((double) t >= 1.0);
    this.reapButtonContainer.gameObject.SetActive((double) t >= 1.0);
    if ((double) t < 1.0)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.reapButton);
  }

  public override void FollowerSelected(FollowerInfo followerInfo)
  {
    base.FollowerSelected(followerInfo);
    if (this.selectedFollowers.Contains(followerInfo))
      this.selectedFollowers.Remove(followerInfo);
    else
      this.selectedFollowers.Add(followerInfo);
    for (int index = 0; index < this.spines.Length; ++index)
    {
      if (index <= this.selectedFollowers.Count - 1)
      {
        this.spines[index].color = Color.white;
        this.spines[index].ConfigureFollower(this.selectedFollowers[index]);
      }
      else
        this.spines[index].color = Color.black;
    }
    this.UpdateBar();
    this.UpdateCheckboxes();
  }

  public void UpdateCheckboxes()
  {
    foreach (FollowerInformationBox followerInfoBox in this.FollowerInfoBoxes)
    {
      if (this.selectedFollowers.Contains(followerInfoBox.FollowerInfo))
        followerInfoBox.SetChosen();
      else
        followerInfoBox.RemoveChosen();
    }
  }

  public void ConfirmReapButton()
  {
    Action<List<FollowerInfo>> onFollowersChosen = this.OnFollowersChosen;
    if (onFollowersChosen != null)
      onFollowersChosen(this.selectedFollowers);
    this.Hide();
  }

  public void OnReapButtonSelected()
  {
    this._highlightController.Image.color = new Color(1f, 1f, 1f, 1f);
    this._highlightController.transform.DOKill();
    this._highlightController.transform.DOShakeScale(0.05f, new Vector3(-0.05f, 0.05f, 1f), 3, fadeOut: false).SetUpdate<Tweener>(true);
  }

  public void OnReapButtonDeselected()
  {
    this._highlightController.Image.color = new Color(0.0f, 0.5f, 1f, 1f);
  }
}
