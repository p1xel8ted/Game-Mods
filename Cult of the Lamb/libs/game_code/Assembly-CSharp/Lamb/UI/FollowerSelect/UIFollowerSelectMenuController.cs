// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FollowerSelect.UIFollowerSelectMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.UI.Menus;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.FollowerSelect;

public class UIFollowerSelectMenuController : UIFollowerSelectBase<FollowerInformationBox>
{
  [SerializeField]
  public TMP_Text _header;
  [SerializeField]
  public GameObject blurBackground;
  [Header("Info Card")]
  [SerializeField]
  public RectTransform _rootContainer;
  [SerializeField]
  public RectTransform _cardContainer;
  [Header("Non Required")]
  [SerializeField]
  public PrisonMenuInfoCardController _defaultInfoCard;
  [SerializeField]
  public UIHoldInteraction _uiHoldInteraction;
  public string kSelectedFollowerAnimationState = "Selected";
  public string kCancelSelectionAnimationState = "Cancelled";
  public string kConfirmedSelectionAnimationState = "Confirmed";
  public UpgradeSystem.Type _followerSelectionType;
  public List<ObjectivesData> _cachedObjectives;
  public bool showDefaultInfoCard;
  public bool disabledDynamicRes;
  public List<TwitchVoting.VotingType> SortUsingSin = new List<TwitchVoting.VotingType>()
  {
    TwitchVoting.VotingType.DRUM_CIRCLE,
    TwitchVoting.VotingType.RITUAL_ATONE,
    TwitchVoting.VotingType.RITUAL_CANNIBAL,
    TwitchVoting.VotingType.RITUAL_NUDISM,
    TwitchVoting.VotingType.RITUAL_PURGE,
    TwitchVoting.VotingType.DRINK
  };

  public new void OnDisable()
  {
  }

  public void Show(
    List<FollowerSelectEntry> followerSelectEntries,
    bool instant = false,
    UpgradeSystem.Type followerSelectionType = UpgradeSystem.Type.Count,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true,
    bool showDefaultInfoCard = false,
    bool excludeLightningTargets = true)
  {
    if (excludeLightningTargets)
      followerSelectEntries.RemoveAll((Predicate<FollowerSelectEntry>) (entry =>
      {
        Follower followerById = FollowerManager.FindFollowerByID(entry.FollowerInfo.ID);
        return (UnityEngine.Object) followerById != (UnityEngine.Object) null && (UnityEngine.Object) followerById.Interaction_FollowerInteraction != (UnityEngine.Object) null && followerById.Interaction_FollowerInteraction.LightningIncoming;
      }));
    this._followerSelectionType = followerSelectionType;
    this.Show(followerSelectEntries, instant, hideOnSelection, cancellable, hasSelection);
    this.showDefaultInfoCard = showDefaultInfoCard;
    if (!((UnityEngine.Object) this._defaultInfoCard != (UnityEngine.Object) null))
      return;
    this._defaultInfoCard.gameObject.SetActive(showDefaultInfoCard);
  }

  public override void OnShowStarted()
  {
    if (this._cachedObjectives != null)
      this._cachedObjectives.Clear();
    if (!this._hasSelection)
      this._header.text = ScriptLocalization.Inventory.FOLLOWERS;
    base.OnShowStarted();
    if (!this.SortUsingSin.Contains(this.VotingType))
      return;
    this.OnSortingMethodChanged(4);
  }

  public void SetHeaderText(string text) => this._header.text = text;

  public override void OnShowFinished()
  {
    base.OnShowFinished();
    foreach (FollowerInformationBox followerInfoBox in this._followerInfoBoxes)
    {
      if (this.DoesFollowerHaveObjective(followerInfoBox.FollowerInfo))
        followerInfoBox.ShowObjective();
    }
  }

  public bool DoesFollowerHaveObjective(FollowerInfo followerInfo)
  {
    if (this._cachedObjectives == null)
    {
      this._cachedObjectives = new List<ObjectivesData>();
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        switch (objective)
        {
          case Objectives_PerformRitual objectivesPerformRitual1 when objectivesPerformRitual1.Ritual == this._followerSelectionType:
            this._cachedObjectives.Add(objective);
            continue;
          case Objectives_PerformRitual objectivesPerformRitual2 when objectivesPerformRitual2.Ritual == UpgradeSystem.Type.Ritual_Divorce && (this._followerSelectionType == UpgradeSystem.Type.Ritual_FollowerWedding || this._followerSelectionType == UpgradeSystem.Type.Ritual_Wedding):
            this._cachedObjectives.Add(objective);
            continue;
          case Objectives_Custom objectivesCustom:
            switch (this._followerSelectionType)
            {
              case UpgradeSystem.Type.Building_Prison:
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison)
                {
                  this._cachedObjectives.Add(objective);
                  continue;
                }
                continue;
              case UpgradeSystem.Type.Building_Missionary:
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerOnMissionary)
                {
                  this._cachedObjectives.Add(objective);
                  continue;
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendSpouseOnMissionary)
                {
                  this._cachedObjectives.Add(objective);
                  continue;
                }
                continue;
              case UpgradeSystem.Type.Building_Drum:
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.DrumCircle)
                {
                  this._cachedObjectives.Add(objective);
                  continue;
                }
                continue;
              case UpgradeSystem.Type.Building_MatingTent:
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToMatingTent)
                {
                  this._cachedObjectives.Add(objective);
                  continue;
                }
                if (objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.DepositFollower)
                {
                  this._cachedObjectives.Add(objective);
                  continue;
                }
                continue;
              default:
                continue;
            }
          case Objectives_AssignClothing _ when this._followerSelectionType == UpgradeSystem.Type.Building_Tailor:
            this._cachedObjectives.Add(objective);
            continue;
          case Objectives_Mating _ when this._followerSelectionType == UpgradeSystem.Type.Building_MatingTent:
            this._cachedObjectives.Add(objective);
            continue;
          default:
            continue;
        }
      }
    }
    foreach (ObjectivesData cachedObjective in this._cachedObjectives)
    {
      if (cachedObjective is Objectives_PerformRitual objectivesPerformRitual && (objectivesPerformRitual.TargetFollowerID_1 == followerInfo.ID || objectivesPerformRitual.TargetFollowerID_2 == followerInfo.ID))
        return true;
      if (cachedObjective is Objectives_Custom objectivesCustom)
      {
        if (objectivesCustom.TargetFollowerID == followerInfo.ID || objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.DepositFollower && this._followerSelectionType == UpgradeSystem.Type.Building_MatingTent && followerInfo.SkinName.Contains("Bug"))
          return true;
      }
      else if (this._followerSelectionType == UpgradeSystem.Type.Building_Tailor && cachedObjective is Objectives_AssignClothing objectivesAssignClothing && objectivesAssignClothing.TargetFollower == followerInfo.ID && TailorMenu_Assign.CurrentAssigningClothingType == objectivesAssignClothing.ClothingType || this._followerSelectionType == UpgradeSystem.Type.Building_MatingTent && cachedObjective is Objectives_Mating objectivesMating && (objectivesMating.TargetFollower_1 == followerInfo.ID || objectivesMating.TargetFollower_2 == followerInfo.ID))
        return true;
    }
    return false;
  }

  public override FollowerInformationBox PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.FollowerInformationBoxTemplate;
  }

  public void SetBackgroundState(bool active) => this.blurBackground.gameObject.SetActive(active);

  public override void FollowerSelected(FollowerInfo followerInfo)
  {
    if (this.showDefaultInfoCard)
    {
      this._defaultInfoCard.enabled = false;
      this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      this.SetActiveStateForMenu(false);
      this.StartCoroutine((IEnumerator) this.FocusFollowerCard(followerInfo));
    }
    else
      base.FollowerSelected(followerInfo);
  }

  public IEnumerator FocusFollowerCard(FollowerInfo followerInfo)
  {
    UIFollowerSelectMenuController selectMenuController = this;
    selectMenuController._controlPrompts.HideAcceptButton();
    selectMenuController._uiHoldInteraction.Reset();
    PrisonInfoCard currentCard = selectMenuController._defaultInfoCard.CurrentCard;
    currentCard.RectTransform.SetParent((Transform) selectMenuController._rootContainer, true);
    currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    selectMenuController._animator.Play(selectMenuController.kSelectedFollowerAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
    currentCard.FollowerSpine.AnimationState.SetAnimation(0, "Reactions/react-worried1", true);
    bool cancel = false;
    yield return (object) selectMenuController._uiHoldInteraction.DoHoldInteraction((Action<float>) (progress =>
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
      currentCard.FollowerSpine.AnimationState.SetAnimation(0, "idle", true);
      currentCard.RectTransform.DOLocalMove((Vector3) (Vector2) selectMenuController._rootContainer.InverseTransformPoint(selectMenuController._cardContainer.TransformPoint(Vector3.zero)), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => currentCard.RectTransform.SetParent((Transform) this._cardContainer, true)));
      selectMenuController._animator.Play(selectMenuController.kCancelSelectionAnimationState);
      yield return (object) new WaitForSecondsRealtime(1f);
      selectMenuController._controlPrompts.ShowAcceptButton();
      selectMenuController.SetActiveStateForMenu(true);
      selectMenuController._defaultInfoCard.enabled = true;
    }
    else
    {
      selectMenuController._controlPrompts.HideCancelButton();
      currentCard.RectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) selectMenuController._animator.YieldForAnimation(selectMenuController.kConfirmedSelectionAnimationState);
      Action<FollowerInfo> followerSelected = selectMenuController.OnFollowerSelected;
      if (followerSelected != null)
        followerSelected(followerInfo);
      selectMenuController.Hide(true);
    }
  }
}
