// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.UIPrisonMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class UIPrisonMenuController : UIFollowerSelectMenuController
{
  private string kSelectedFollowerAnimationState = "Selected";
  private string kCancelSelectionAnimationState = "Cancelled";
  private string kConfirmedSelectionAnimationState = "Confirmed";
  [Header("Info Card")]
  [SerializeField]
  private PrisonMenuInfoCardController _prisonMenuInfoCardController;
  [SerializeField]
  private RectTransform _rootContainer;
  [SerializeField]
  private RectTransform _cardContainer;
  [Header("Misc")]
  [SerializeField]
  private UIHoldInteraction _uiHoldInteraction;

  protected override void FollowerSelected(FollowerInfo followerInfo)
  {
    this._prisonMenuInfoCardController.enabled = false;
    this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.SetActiveStateForMenu(false);
    this.StartCoroutine((IEnumerator) this.FocusFollowerCard(followerInfo));
  }

  private IEnumerator FocusFollowerCard(FollowerInfo followerInfo)
  {
    UIPrisonMenuController prisonMenuController = this;
    prisonMenuController._controlPrompts.HideAcceptButton();
    prisonMenuController._uiHoldInteraction.Reset();
    PrisonInfoCard currentCard = prisonMenuController._prisonMenuInfoCardController.CurrentCard;
    currentCard.RectTransform.SetParent((Transform) prisonMenuController._rootContainer, true);
    currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    prisonMenuController._animator.Play(prisonMenuController.kSelectedFollowerAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
    currentCard.FollowerSpine.AnimationState.SetAnimation(0, "Reactions/react-worried1", true);
    bool cancel = false;
    yield return (object) prisonMenuController._uiHoldInteraction.DoHoldInteraction(new Action<float>(OnHold), new System.Action(OnCancel));
    MMVibrate.StopRumble();
    if (cancel)
    {
      currentCard.FollowerSpine.AnimationState.SetAnimation(0, "idle", true);
      currentCard.RectTransform.DOLocalMove((Vector3) (Vector2) prisonMenuController._rootContainer.InverseTransformPoint(prisonMenuController._cardContainer.TransformPoint(Vector3.zero)), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => currentCard.RectTransform.SetParent((Transform) this._cardContainer, true)));
      prisonMenuController._animator.Play(prisonMenuController.kCancelSelectionAnimationState);
      yield return (object) new WaitForSecondsRealtime(1f);
      prisonMenuController._controlPrompts.ShowAcceptButton();
      prisonMenuController.SetActiveStateForMenu(true);
      prisonMenuController._prisonMenuInfoCardController.enabled = true;
    }
    else
    {
      prisonMenuController._controlPrompts.HideCancelButton();
      currentCard.RectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) prisonMenuController._animator.YieldForAnimation(prisonMenuController.kConfirmedSelectionAnimationState);
      Action<FollowerInfo> followerSelected = prisonMenuController.OnFollowerSelected;
      if (followerSelected != null)
        followerSelected(followerInfo);
      prisonMenuController.Hide(true);
    }

    void OnHold(float progress)
    {
      float num = (float) (1.0 + 0.25 * (double) progress);
      currentCard.RectTransform.localScale = new Vector3(num, num, num);
      currentCard.RectTransform.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * progress * this._uiHoldInteraction.HoldTime * 2f);
      MMVibrate.RumbleContinuous(progress * 0.2f, progress * 0.2f);
      if (currentCard.RedOutline.gameObject.activeSelf != (double) progress > 0.0)
        currentCard.RedOutline.gameObject.SetActive((double) progress > 0.0);
      currentCard.RedOutline.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 1.2f, 1.2f), progress);
    }

    void OnCancel()
    {
      TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = currentCard.RedOutline.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() => currentCard.RedOutline.gameObject.SetActive(false));
      currentCard.RectTransform.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      cancel = true;
      MMVibrate.StopRumble();
    }
  }
}
