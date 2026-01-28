// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDemonSummonMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  [Header("Info Card")]
  [SerializeField]
  public DemonInfoCardController _demonMenuInfoCardController;
  [Header("Limits")]
  [SerializeField]
  public RectTransform _shootyLimitContainer;
  [SerializeField]
  public TextMeshProUGUI _shootyLimit;
  [SerializeField]
  public RectTransform _chompLimitContainer;
  [SerializeField]
  public TextMeshProUGUI _chompLimit;
  [SerializeField]
  public RectTransform _arrowsLimitContainer;
  [SerializeField]
  public TextMeshProUGUI _arrowsLimit;
  [SerializeField]
  public RectTransform _collectorsLimitContainer;
  [SerializeField]
  public TextMeshProUGUI _collectorsLimit;
  [SerializeField]
  public RectTransform _exploderLimitContainer;
  [SerializeField]
  public TextMeshProUGUI _exploderLimit;
  [SerializeField]
  public RectTransform _spiritLimitContainer;
  [SerializeField]
  public TextMeshProUGUI _spiritLimit;
  [SerializeField]
  public RectTransform _rotLimitContainer;
  [SerializeField]
  public TextMeshProUGUI _rotLimit;
  public List<int> _demonIDS = new List<int>();

  public override TwitchVoting.VotingType VotingType => TwitchVoting.VotingType.DEMON;

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this._rotLimitContainer.gameObject.SetActive(DataManager.Instance.RecruitedRotFollower);
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    foreach (FollowerInformationBox followerInfoBox in this._followerInfoBoxes)
    {
      int demonType = DemonModel.GetDemonType(followerInfoBox.FollowerInfo);
      followerInfoBox.Button.Confirmable = followerInfoBox.Button.Confirmable && !this._demonIDS.Contains(demonType);
      followerInfoBox.Button.OnConfirmDenied += (System.Action) (() => this.OnConfirmationDenied(demonType));
      if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Leshy) && followerInfoBox.FollowerInfo.ID == 99990 || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Heket) && followerInfoBox.FollowerInfo.ID == 99991 || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Kallamar) && followerInfoBox.FollowerInfo.ID == 99992 || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.HealingBishop_Shamura) && followerInfoBox.FollowerInfo.ID == 99993 || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LegendarySword) && followerInfoBox.FollowerInfo.ID == 100000)
        followerInfoBox.ShowObjective();
    }
  }

  public override void FollowerSelected(FollowerInfo followerInfo)
  {
    this._demonMenuInfoCardController.enabled = false;
    this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.SetActiveStateForMenu(false);
    this.StartCoroutine((IEnumerator) this.FocusFollowerCard(followerInfo));
  }

  public new IEnumerator FocusFollowerCard(FollowerInfo followerInfo)
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
    yield return (object) summonMenuController._uiHoldInteraction.DoHoldInteraction((Action<float>) (progress =>
    {
      float num = (float) (1.0 + 0.25 * (double) progress);
      currentCard.RectTransform.localScale = new Vector3(num, num, num);
      currentCard.RectTransform.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * progress * this._uiHoldInteraction.HoldTime * 2f);
      MMVibrate.RumbleContinuous(progress * 0.2f, progress * 0.2f, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
      if (currentCard.RedOutline.gameObject.activeSelf != (double) progress > 0.0)
        currentCard.RedOutline.gameObject.SetActive((double) progress > 0.0);
      currentCard.RedOutline.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 1.2f, 1.2f), progress);
    }), (System.Action) (() =>
    {
      TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = currentCard.RedOutline.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() => currentCard.RedOutline.gameObject.SetActive(false));
      currentCard.RectTransform.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      cancel = true;
      MMVibrate.StopRumble();
    }));
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
  }

  public void OnConfirmationDenied(int demonType)
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
      case 13:
        this.ShakeLimit(this._rotLimitContainer);
        break;
    }
  }

  public void ShakeLimit(RectTransform container)
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
    int num7 = 0;
    foreach (int num8 in this._demonIDS)
    {
      switch (num8)
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
        case 13:
          ++num7;
          continue;
        default:
          continue;
      }
    }
    if (num1 == 0)
      this._shootyLimit.color = StaticColors.GreyColor;
    else
      this._shootyLimit.color = StaticColors.OffWhiteColor;
    if (num2 == 0)
      this._chompLimit.color = StaticColors.GreyColor;
    else
      this._chompLimit.color = StaticColors.OffWhiteColor;
    if (num3 == 0)
      this._arrowsLimit.color = StaticColors.GreyColor;
    else
      this._arrowsLimit.color = StaticColors.OffWhiteColor;
    if (num4 == 0)
      this._collectorsLimit.color = StaticColors.GreyColor;
    else
      this._collectorsLimit.color = StaticColors.OffWhiteColor;
    if (num5 == 0)
      this._exploderLimit.color = StaticColors.GreyColor;
    else
      this._exploderLimit.color = StaticColors.OffWhiteColor;
    if (num6 == 0)
      this._spiritLimit.color = StaticColors.GreyColor;
    else
      this._spiritLimit.color = StaticColors.OffWhiteColor;
    if (num7 == 0)
      this._rotLimit.color = StaticColors.GreyColor;
    else
      this._rotLimit.color = StaticColors.OffWhiteColor;
    this._shootyLimit.text = $"{num1}/{1}";
    this._chompLimit.text = $"{num2}/{1}";
    this._arrowsLimit.text = $"{num3}/{1}";
    this._collectorsLimit.text = $"{num4}/{1}";
    this._exploderLimit.text = $"{num5}/{1}";
    this._spiritLimit.text = $"{num6}/{1}";
    this._rotLimit.text = $"{num7}/{1}";
  }
}
