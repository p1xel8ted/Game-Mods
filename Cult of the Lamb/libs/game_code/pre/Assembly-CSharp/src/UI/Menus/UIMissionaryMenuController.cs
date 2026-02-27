// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.UIMissionaryMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private string kSelectedFollowerAnimationState = "Selected";
  private string kCancelSelectionAnimationState = "Cancelled";
  private string kConfirmedSelectionAnimationState = "Confirmed";
  [Header("Info Card")]
  [SerializeField]
  private MissionInfoCardController _missionInfoCardController;
  [SerializeField]
  private RectTransform _rootContainer;
  [SerializeField]
  private RectTransform _cardContainer;
  private bool _choosingFollower;

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    this.SetActiveStateForMenu(this._missionInfoCardController.gameObject, false);
  }

  protected override void FollowerSelected(FollowerInfo followerInfo)
  {
    this._missionInfoCardController.enabled = false;
    this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.SetActiveStateForMenu(false);
    this.StartCoroutine((IEnumerator) this.FocusFollowerCard(followerInfo));
    this._choosingFollower = true;
  }

  private IEnumerator FocusFollowerCard(FollowerInfo followerInfo)
  {
    UIMissionaryMenuController missionaryMenuController = this;
    missionaryMenuController._controlPrompts.HideAcceptButton();
    missionaryMenuController._missionInfoCardController.enabled = false;
    missionaryMenuController._animator.Play(missionaryMenuController.kSelectedFollowerAnimationState);
    MissionInfoCard currentCard = missionaryMenuController._missionInfoCardController.CurrentCard;
    currentCard.RectTransform.SetParent((Transform) missionaryMenuController._rootContainer, true);
    currentCard.RectTransform.DOKill();
    currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    missionaryMenuController._animator.Play(missionaryMenuController.kSelectedFollowerAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
    missionaryMenuController._controlPrompts.ShowAcceptButton();
    currentCard.OnMissionSelected += new Action<InventoryItem.ITEM_TYPE>(MissionChosen);
    missionaryMenuController.SetActiveStateForMenu(currentCard.gameObject, true);
    missionaryMenuController._canvasGroup.interactable = true;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) currentCard.FirstAvailableButton());
    InventoryItem.ITEM_TYPE chosenItem = InventoryItem.ITEM_TYPE.NONE;
    bool cancel = false;
    while (chosenItem == InventoryItem.ITEM_TYPE.NONE && !cancel)
    {
      if (InputManager.UI.GetCancelButtonDown())
        Cancel();
      yield return (object) null;
    }
    missionaryMenuController.SetActiveStateForMenu(currentCard.gameObject, false);
    missionaryMenuController._canvasGroup.interactable = false;
    if (cancel)
    {
      missionaryMenuController._controlPrompts.HideAcceptButton();
      Debug.Log((object) "Run cancel routine".Colour(Color.yellow));
      currentCard.FollowerSpine.AnimationState.SetAnimation(0, "idle", true);
      Vector2 endValue = (Vector2) missionaryMenuController._rootContainer.InverseTransformPoint(missionaryMenuController._cardContainer.TransformPoint(Vector3.zero));
      currentCard.RectTransform.DOKill();
      currentCard.RectTransform.DOLocalMove((Vector3) endValue, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => currentCard.RectTransform.SetParent((Transform) this._cardContainer, true)));
      missionaryMenuController._animator.Play(missionaryMenuController.kCancelSelectionAnimationState);
      yield return (object) new WaitForSecondsRealtime(1f);
      missionaryMenuController._controlPrompts.ShowAcceptButton();
      missionaryMenuController._canvasGroup.interactable = true;
      missionaryMenuController.SetActiveStateForMenu(missionaryMenuController._contentContainer.gameObject, true);
      missionaryMenuController.ActivateNavigation();
      missionaryMenuController._missionInfoCardController.enabled = true;
      missionaryMenuController._choosingFollower = false;
    }
    else
    {
      Debug.Log((object) "Run hide routine".Colour(Color.red));
      missionaryMenuController._controlPrompts.HideAcceptButton();
      missionaryMenuController._controlPrompts.HideCancelButton();
      currentCard.RectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) missionaryMenuController._animator.YieldForAnimation(missionaryMenuController.kConfirmedSelectionAnimationState);
      Action<FollowerInfo, InventoryItem.ITEM_TYPE> missionaryChosen = missionaryMenuController.OnMissionaryChosen;
      if (missionaryChosen != null)
        missionaryChosen(followerInfo, chosenItem);
      missionaryMenuController.Hide(true);
    }

    void Cancel() => cancel = true;

    void MissionChosen(InventoryItem.ITEM_TYPE itemType) => chosenItem = itemType;
  }

  public override void OnCancelButtonInput()
  {
    if (!this._choosingFollower)
      base.OnCancelButtonInput();
    else
      AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
  }
}
