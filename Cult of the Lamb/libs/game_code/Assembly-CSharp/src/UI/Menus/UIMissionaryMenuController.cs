// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.UIMissionaryMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI.FollowerSelect;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class UIMissionaryMenuController : UIFollowerSelectMenuController
{
  public Action<FollowerInfo, InventoryItem.ITEM_TYPE> OnMissionaryChosen;
  [Header("Info Card")]
  [SerializeField]
  public MissionInfoCardController _missionInfoCardController;
  public bool _choosingFollower;
  public bool _cannotCancel;

  public override TwitchVoting.VotingType VotingType => TwitchVoting.VotingType.MISSIONARY;

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this.SetActiveStateForMenu(this._missionInfoCardController.gameObject, false);
  }

  public override void FinalizeVote(FollowerInfo followerInfo)
  {
    this._cannotCancel = true;
    this._controlPrompts.HideCancelButton();
    this.StartCoroutine((IEnumerator) this.DeferredFinalize(followerInfo));
  }

  public IEnumerator DeferredFinalize(FollowerInfo followerInfo)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIMissionaryMenuController missionaryMenuController = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      missionaryMenuController._missionInfoCardController.ShowCardWithParam(followerInfo);
      missionaryMenuController._missionInfoCardController.enabled = false;
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      missionaryMenuController.SetActiveStateForMenu(false);
      missionaryMenuController.StartCoroutine((IEnumerator) missionaryMenuController.FocusFollowerCard(followerInfo));
      missionaryMenuController._choosingFollower = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void FollowerSelected(FollowerInfo followerInfo)
  {
    this._missionInfoCardController.enabled = false;
    this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.SetActiveStateForMenu(false);
    this.StartCoroutine((IEnumerator) this.FocusFollowerCard(followerInfo));
    this._choosingFollower = true;
  }

  public override void DoRelease()
  {
    base.DoRelease();
    this.SetActiveStateForMenu(this._missionInfoCardController.gameObject, false);
  }

  public new IEnumerator FocusFollowerCard(FollowerInfo followerInfo)
  {
    UIMissionaryMenuController missionaryMenuController = this;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    UIMissionaryMenuController.\u003C\u003Ec__DisplayClass11_0 cDisplayClass110 = new UIMissionaryMenuController.\u003C\u003Ec__DisplayClass11_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.\u003C\u003E4__this = this;
    missionaryMenuController._controlPrompts.HideAcceptButton();
    missionaryMenuController._sortingDropdown.Dropdown.enabled = false;
    missionaryMenuController._missionInfoCardController.enabled = false;
    missionaryMenuController._animator.Play(missionaryMenuController.kSelectedFollowerAnimationState);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.currentCard = missionaryMenuController._missionInfoCardController.CurrentCard;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.currentCard.RectTransform.SetParent((Transform) missionaryMenuController._rootContainer, true);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.currentCard.RectTransform.DOKill();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.currentCard.SetScrollActive(true);
    missionaryMenuController._animator.Play(missionaryMenuController.kSelectedFollowerAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
    missionaryMenuController._controlPrompts.ShowAcceptButton();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    cDisplayClass110.currentCard.OnMissionSelected += new Action<InventoryItem.ITEM_TYPE>(cDisplayClass110.\u003CFocusFollowerCard\u003Eg__MissionChosen\u007C1);
    // ISSUE: reference to a compiler-generated field
    missionaryMenuController.SetActiveStateForMenu(cDisplayClass110.currentCard.gameObject, true);
    missionaryMenuController._canvasGroup.interactable = true;
    // ISSUE: reference to a compiler-generated field
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) cDisplayClass110.currentCard.FirstAvailableButton());
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.chosenItem = InventoryItem.ITEM_TYPE.NONE;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass110.cancel = false;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    while (cDisplayClass110.chosenItem == InventoryItem.ITEM_TYPE.NONE && !cDisplayClass110.cancel)
    {
      if (InputManager.UI.GetCancelButtonDown() && !missionaryMenuController._cannotCancel)
      {
        // ISSUE: reference to a compiler-generated method
        cDisplayClass110.\u003CFocusFollowerCard\u003Eg__Cancel\u007C0();
      }
      yield return (object) null;
    }
    // ISSUE: reference to a compiler-generated field
    missionaryMenuController.SetActiveStateForMenu(cDisplayClass110.currentCard.gameObject, false);
    missionaryMenuController._canvasGroup.interactable = false;
    // ISSUE: reference to a compiler-generated field
    if (cDisplayClass110.cancel)
    {
      missionaryMenuController._controlPrompts.HideAcceptButton();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.currentCard.SetScrollActive(false);
      Debug.Log((object) "Run cancel routine".Colour(Color.yellow));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.currentCard.FollowerSpine.AnimationState.SetAnimation(0, "idle", true);
      Vector2 endValue = (Vector2) missionaryMenuController._rootContainer.InverseTransformPoint(missionaryMenuController._cardContainer.TransformPoint(Vector3.zero));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.currentCard.RectTransform.DOKill();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClass110.currentCard.RectTransform.DOLocalMove((Vector3) endValue, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(cDisplayClass110.\u003CFocusFollowerCard\u003Eb__2));
      missionaryMenuController._animator.Play(missionaryMenuController.kCancelSelectionAnimationState);
      yield return (object) new WaitForSecondsRealtime(1f);
      missionaryMenuController._controlPrompts.ShowAcceptButton();
      missionaryMenuController._canvasGroup.interactable = true;
      missionaryMenuController.SetActiveStateForMenu(missionaryMenuController._scrollRect.gameObject, true);
      missionaryMenuController.ActivateNavigation();
      missionaryMenuController._sortingDropdown.Dropdown.enabled = true;
      missionaryMenuController._sortingDropdown.Dropdown.Interactable = true;
      missionaryMenuController._sortingDropdown.Dropdown.Button.Interactable = true;
      missionaryMenuController._sortingDropdown.Dropdown.Button.SetInteractionState(true);
      missionaryMenuController._missionInfoCardController.enabled = true;
      missionaryMenuController._choosingFollower = false;
    }
    else
    {
      Debug.Log((object) "Run hide routine".Colour(Color.red));
      missionaryMenuController._controlPrompts.HideAcceptButton();
      missionaryMenuController._controlPrompts.HideCancelButton();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass110.currentCard.RectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) missionaryMenuController._animator.YieldForAnimation(missionaryMenuController.kConfirmedSelectionAnimationState);
      Action<FollowerInfo, InventoryItem.ITEM_TYPE> missionaryChosen = missionaryMenuController.OnMissionaryChosen;
      if (missionaryChosen != null)
      {
        // ISSUE: reference to a compiler-generated field
        missionaryChosen(followerInfo, cDisplayClass110.chosenItem);
      }
      missionaryMenuController.Hide(true);
    }
  }

  public override void OnCancelButtonInput()
  {
    if (!this._choosingFollower)
      base.OnCancelButtonInput();
    else
      AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
  }
}
