// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDemonSummonMenuController
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
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIDemonSummonMenuController : UIFollowerSelectMenuController
{
  private string kSelectedFollowerAnimationState = "Selected";
  private string kCancelSelectionAnimationState = "Cancelled";
  private string kConfirmedSelectionAnimationState = "Confirmed";
  [Header("Info Card")]
  [SerializeField]
  private DemonInfoCardController _demonMenuInfoCardController;
  [SerializeField]
  private RectTransform _rootContainer;
  [SerializeField]
  private RectTransform _cardContainer;
  [Header("Limits")]
  [SerializeField]
  private RectTransform _shootyLimitContainer;
  [SerializeField]
  private TextMeshProUGUI _shootyLimit;
  [SerializeField]
  private RectTransform _chompLimitContainer;
  [SerializeField]
  private TextMeshProUGUI _chompLimit;
  [SerializeField]
  private RectTransform _arrowsLimitContainer;
  [SerializeField]
  private TextMeshProUGUI _arrowsLimit;
  [SerializeField]
  private RectTransform _collectorsLimitContainer;
  [SerializeField]
  private TextMeshProUGUI _collectorsLimit;
  [SerializeField]
  private RectTransform _exploderLimitContainer;
  [SerializeField]
  private TextMeshProUGUI _exploderLimit;
  [SerializeField]
  private RectTransform _spiritLimitContainer;
  [SerializeField]
  private TextMeshProUGUI _spiritLimit;
  [Header("Misc")]
  [SerializeField]
  private UIHoldInteraction _uiHoldInteraction;
  private List<int> _demonIDS = new List<int>();

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    foreach (FollowerInformationBox followerInfoBox in this._followerInfoBoxes)
    {
      int demonType = DemonModel.GetDemonType(followerInfoBox.FollowerInfo);
      followerInfoBox.Button.Confirmable = !this._demonIDS.Contains(demonType);
      followerInfoBox.Button.OnConfirmDenied += (System.Action) (() => this.OnConfirmationDenied(demonType));
    }
  }

  protected override void FollowerSelected(FollowerInfo followerInfo)
  {
    this._demonMenuInfoCardController.enabled = false;
    this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.SetActiveStateForMenu(false);
    this.StartCoroutine((IEnumerator) this.FocusFollowerCard(followerInfo));
  }

  private IEnumerator FocusFollowerCard(FollowerInfo followerInfo)
  {
    UIDemonSummonMenuController summonMenuController = this;
    summonMenuController._controlPrompts.HideAcceptButton();
    summonMenuController._uiHoldInteraction.Reset();
    DemonInfoCard currentCard = summonMenuController._demonMenuInfoCardController.CurrentCard;
    currentCard.RectTransform.SetParent((Transform) summonMenuController._rootContainer, true);
    currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    summonMenuController._animator.Play(summonMenuController.kSelectedFollowerAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
    bool cancel = false;
    yield return (object) summonMenuController._uiHoldInteraction.DoHoldInteraction(new Action<float>(OnHold), new System.Action(OnCancel));
    MMVibrate.StopRumble();
    if (cancel)
    {
      currentCard.RectTransform.DOLocalMove((Vector3) (Vector2) summonMenuController._rootContainer.InverseTransformPoint(summonMenuController._cardContainer.TransformPoint(Vector3.zero)), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => currentCard.RectTransform.SetParent((Transform) this._cardContainer, true)));
      summonMenuController._animator.Play(summonMenuController.kCancelSelectionAnimationState);
      yield return (object) new WaitForSecondsRealtime(1f);
      summonMenuController._controlPrompts.ShowAcceptButton();
      summonMenuController.SetActiveStateForMenu(true);
      summonMenuController._demonMenuInfoCardController.enabled = true;
    }
    else
    {
      summonMenuController._controlPrompts.HideCancelButton();
      currentCard.RectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) summonMenuController._animator.YieldForAnimation(summonMenuController.kConfirmedSelectionAnimationState);
      Action<FollowerInfo> followerSelected = summonMenuController.OnFollowerSelected;
      if (followerSelected != null)
        followerSelected(followerInfo);
      summonMenuController.Hide(true);
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

  private void OnConfirmationDenied(int demonType)
  {
    switch (demonType)
    {
      case 0:
        this.ShakeLimit(this._shootyLimitContainer);
        break;
      case 1:
        this.ShakeLimit(this._chompLimitContainer);
        break;
      case 2:
        this.ShakeLimit(this._arrowsLimitContainer);
        break;
      case 3:
        this.ShakeLimit(this._collectorsLimitContainer);
        break;
      case 4:
        this.ShakeLimit(this._exploderLimitContainer);
        break;
      case 5:
        this.ShakeLimit(this._spiritLimitContainer);
        break;
    }
  }

  private void ShakeLimit(RectTransform container)
  {
    container.transform.DOKill();
    container.anchoredPosition = Vector2.zero;
    container.transform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void UpdateDemonCounts(List<int> followerIDs)
  {
    foreach (int followerId in followerIDs)
      this._demonIDS.Add(DemonModel.GetDemonType(followerId));
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    foreach (int num7 in this._demonIDS)
    {
      switch (num7)
      {
        case 0:
          ++num1;
          continue;
        case 1:
          ++num2;
          continue;
        case 2:
          ++num3;
          continue;
        case 3:
          ++num4;
          continue;
        case 4:
          ++num5;
          continue;
        case 5:
          ++num6;
          continue;
        default:
          continue;
      }
    }
    this._shootyLimit.text = $"{num1}/{1}";
    this._chompLimit.text = $"{num2}/{1}";
    this._arrowsLimit.text = $"{num3}/{1}";
    this._collectorsLimit.text = $"{num4}/{1}";
    this._exploderLimit.text = $"{num5}/{1}";
    this._spiritLimit.text = $"{num6}/{1}";
  }
}
