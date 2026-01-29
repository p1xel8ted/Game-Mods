// Decompiled with JetBrains decompiler
// Type: UITraitManipulatorMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class UITraitManipulatorMenuController : UIFollowerSelectMenuController
{
  [SerializeField]
  public TMP_Text holdText;
  [SerializeField]
  public TraitMenuTabNavigatorBase tabNavigator;
  [Header("Info Card")]
  [SerializeField]
  public TraitInfoCardController _traitInfoCardController;
  [SerializeField]
  public TraitManipulatorInfoCardController _traitManipulatorMenuInfoCardController;
  [SerializeField]
  public CanvasGroup _contentCanvasGroup;
  public Action<FollowerInfo, UITraitManipulatorMenuController.Type, FollowerTrait.TraitType> OnFollowerChosen;
  [CompilerGenerated]
  public UITraitManipulatorMenuController.Type \u003CSelectionType\u003Ek__BackingField;
  public StructureBrain StructureBrain;

  public override TwitchVoting.VotingType VotingType => TwitchVoting.VotingType.TRAIT_MANIPULATOR;

  public UITraitManipulatorMenuController.Type SelectionType
  {
    get => this.\u003CSelectionType\u003Ek__BackingField;
    set => this.\u003CSelectionType\u003Ek__BackingField = value;
  }

  public void Show(List<FollowerSelectEntry> followerSelectEntries, StructureBrain structure)
  {
    this.tabNavigator.ClearPersistentTab();
    this.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    this.StructureBrain = structure;
    TraitMenuTabNavigatorBase tabNavigator = this.tabNavigator;
    tabNavigator.OnTabChanged = tabNavigator.OnTabChanged + (Action<int>) (i => this.SelectionChosen(i));
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    for (int index = 0; index < this.tabNavigator.NumTabs; ++index)
    {
      if (index == 1 && this.StructureBrain.Data.Type < StructureBrain.TYPES.TRAIT_MANIPULATOR_2 || index == 2 && this.StructureBrain.Data.Type < StructureBrain.TYPES.TRAIT_MANIPULATOR_3)
        this.tabNavigator._tabs[index].SetLocked();
    }
    TraitInfoCardController infoCardController = this._traitInfoCardController;
    infoCardController.OnInfoCardShown = infoCardController.OnInfoCardShown + (Action<TraitInfoCard>) (infoCard =>
    {
      if (this.SelectionType != UITraitManipulatorMenuController.Type.Remove)
        return;
      this.holdText.text = string.Format($"{LocalizationManager.GetTranslation($"UI/TraitManipulator/{this.SelectionType}")}: <color=#FFD201>{FollowerTrait.GetLocalizedTitle(infoCard.Trait)}");
    });
  }

  public override void OnShowCompleted() => base.OnShowCompleted();

  public void SelectionChosen(int index)
  {
    this.SelectionType = (UITraitManipulatorMenuController.Type) index;
    IMMSelectable currentSelectable = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(currentSelectable);
    this._traitManipulatorMenuInfoCardController.CurrentCard.Show(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponentInParent<FollowerInformationBox>().FollowerInfo, true);
    int index1 = 0;
    List<FollowerTrait.TraitType> traitTypeList = new List<FollowerTrait.TraitType>();
    for (int index2 = 0; index2 < this.FollowerInfoBoxes.Count; ++index2)
    {
      FollowerInformationBox followerInfoBox = this.FollowerInfoBoxes[index2];
      followerInfoBox.Button.Confirmable = true;
      traitTypeList.Clear();
      traitTypeList.AddRange((IEnumerable<FollowerTrait.TraitType>) followerInfoBox.FollowerInfo.Traits);
      if (this.SelectionType == UITraitManipulatorMenuController.Type.Add)
      {
        for (int index3 = traitTypeList.Count - 1; index3 >= 0; --index3)
        {
          if (followerInfoBox.FollowerInfo.HasTraitFromNecklace(traitTypeList[index3]))
            traitTypeList.RemoveAt(index3);
        }
      }
      if (traitTypeList.Count >= 6 && this.SelectionType == UITraitManipulatorMenuController.Type.Add)
      {
        followerInfoBox.transform.SetAsLastSibling();
        followerInfoBox.FollowerSelectEntry.AvailabilityStatus = FollowerSelectEntry.Status.UnavailableTooManyTraits;
      }
      else if ((followerInfoBox.FollowerInfo.Traits.Count <= 1 || !this.HasActiveTraits(followerInfoBox.FollowerInfo.Traits)) && this.SelectionType == UITraitManipulatorMenuController.Type.Remove)
      {
        followerInfoBox.transform.SetAsLastSibling();
        followerInfoBox.FollowerSelectEntry.AvailabilityStatus = FollowerSelectEntry.Status.UnavailableNotEnoughTraits;
      }
      else if (followerInfoBox.FollowerInfo.CursedState == Thought.Ill)
      {
        followerInfoBox.transform.SetAsLastSibling();
        followerInfoBox.FollowerSelectEntry.AvailabilityStatus = FollowerSelectEntry.Status.UnavailableIll;
      }
      else if (followerInfoBox.FollowerInfo.CursedState == Thought.Dissenter)
      {
        followerInfoBox.transform.SetAsLastSibling();
        followerInfoBox.FollowerSelectEntry.AvailabilityStatus = FollowerSelectEntry.Status.UnavailableDissenting;
      }
      else if (followerInfoBox.FollowerInfo.CursedState == Thought.BecomeStarving)
      {
        followerInfoBox.transform.SetAsLastSibling();
        followerInfoBox.FollowerSelectEntry.AvailabilityStatus = FollowerSelectEntry.Status.UnavailableStarving;
      }
      else if (followerInfoBox.FollowerInfo.CursedState == Thought.Freezing)
      {
        followerInfoBox.transform.SetAsLastSibling();
        followerInfoBox.FollowerSelectEntry.AvailabilityStatus = FollowerSelectEntry.Status.UnavailableFreezing;
      }
      else
      {
        followerInfoBox.Button.Confirmable = true;
        followerInfoBox.FollowerSelectEntry.AvailabilityStatus = FollowerManager.GetFollowerAvailabilityStatus(followerInfoBox.FollowerInfo);
        followerInfoBox.transform.SetSiblingIndex(index1);
        ++index1;
      }
      this.FollowerSelectEntries[index2].AvailabilityStatus = followerInfoBox.FollowerSelectEntry.AvailabilityStatus;
      followerInfoBox.Configure(followerInfoBox.FollowerInfo);
      followerInfoBox.ShowItemCostValue((InventoryItem.ITEM_TYPE) Interaction_TraitManipulator.GetCost()[0].type, Interaction_TraitManipulator.GetCost()[0].quantity);
    }
  }

  public override void FollowerSelected(FollowerInfo followerInfo)
  {
    foreach (FollowerSelectEntry followerSelectEntry in this.FollowerSelectEntries)
    {
      if (followerSelectEntry.FollowerInfo == followerInfo && followerSelectEntry.AvailabilityStatus != FollowerSelectEntry.Status.Available)
        return;
    }
    this._traitManipulatorMenuInfoCardController.enabled = false;
    this.StartCoroutine((IEnumerator) this.FocusFollowerCard(followerInfo));
  }

  public new IEnumerator FocusFollowerCard(FollowerInfo followerInfo)
  {
    UITraitManipulatorMenuController manipulatorMenuController = this;
    if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null)
    {
      manipulatorMenuController._contentCanvasGroup.interactable = false;
      manipulatorMenuController._traitInfoCardController.Card1.GetComponent<CanvasGroup>().interactable = false;
      manipulatorMenuController._traitInfoCardController.Card2.GetComponent<CanvasGroup>().interactable = false;
      manipulatorMenuController.tabNavigator.SetInteractable(false);
      IMMSelectable currentSelectable = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable;
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      manipulatorMenuController._cancellable = false;
      TraitManipulatorInfoCard currentCard = manipulatorMenuController._traitManipulatorMenuInfoCardController.CurrentCard;
      currentCard.RectTransform.SetParent((Transform) manipulatorMenuController._rootContainer, true);
      Vector3 fromPos = currentCard.transform.position;
      currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      manipulatorMenuController.holdText.text = LocalizationManager.GetTranslation($"UI/TraitManipulator/{manipulatorMenuController.SelectionType}");
      manipulatorMenuController._uiHoldInteraction.Reset();
      manipulatorMenuController._controlPrompts.HideAcceptButton();
      manipulatorMenuController._animator.Play(manipulatorMenuController.kSelectedFollowerAnimationState);
      yield return (object) new WaitForSecondsRealtime(1f);
      manipulatorMenuController._traitInfoCardController.Card1.GetComponent<CanvasGroup>().interactable = true;
      manipulatorMenuController._traitInfoCardController.Card2.GetComponent<CanvasGroup>().interactable = true;
      manipulatorMenuController._traitManipulatorMenuInfoCardController.Card1.GetComponent<CanvasGroup>().interactable = true;
      manipulatorMenuController._traitManipulatorMenuInfoCardController.Card2.GetComponent<CanvasGroup>().interactable = true;
      currentCard.TraitsSelectable(true);
      bool cancel = false;
      IndoctrinationTraitItem indoctrinationTraitItem = (IndoctrinationTraitItem) null;
      foreach (IndoctrinationTraitItem trait in currentCard.Traits)
      {
        if (trait.Selectable.Interactable)
        {
          indoctrinationTraitItem = trait;
          break;
        }
      }
      if (manipulatorMenuController.SelectionType == UITraitManipulatorMenuController.Type.Add)
      {
        indoctrinationTraitItem = currentCard.Traits[currentCard.Traits.Count - 1];
        manipulatorMenuController.holdText.text = string.Format($"{LocalizationManager.GetTranslation($"UI/TraitManipulator/{manipulatorMenuController.SelectionType}")}: <color=#FFD201>{FollowerTrait.GetLocalizedTitle(indoctrinationTraitItem.TraitType)}");
      }
      if ((UnityEngine.Object) indoctrinationTraitItem != (UnityEngine.Object) null)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) indoctrinationTraitItem.Selectable);
        manipulatorMenuController._traitInfoCardController.Card1.Show(indoctrinationTraitItem.TraitType, false);
      }
      yield return (object) manipulatorMenuController._uiHoldInteraction.DoHoldInteraction((Action<float>) (progress =>
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
        this._cancellable = true;
        this._contentCanvasGroup.interactable = true;
        MonoSingleton<UINavigatorNew>.Instance.Clear();
        this._controlPrompts.ShowAcceptButton();
        this._traitInfoCardController.Card1.GetComponent<CanvasGroup>().interactable = false;
        this._traitInfoCardController.Card2.GetComponent<CanvasGroup>().interactable = false;
        this._traitManipulatorMenuInfoCardController.enabled = true;
        TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = currentCard.RedOutline.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() => currentCard.RedOutline.gameObject.SetActive(false));
        currentCard.RectTransform.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        cancel = true;
        MMVibrate.StopRumble();
      }));
      AudioManager.Instance.PlayOneShot("event:/dlc/building/exorcismaltar/exorcism_trait_accept");
      FollowerTrait.TraitType selectedTrait = FollowerTrait.TraitType.None;
      if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null && (bool) (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<IndoctrinationTraitItem>())
        selectedTrait = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<IndoctrinationTraitItem>().TraitType;
      manipulatorMenuController.SetActiveStateForMenu(false);
      MMVibrate.StopRumble();
      if (cancel)
      {
        AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
        currentCard.RectTransform.DOMove(fromPos, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => currentCard.RectTransform.SetParent((Transform) this._cardContainer, true)));
        manipulatorMenuController._animator.Play(manipulatorMenuController.kCancelSelectionAnimationState);
        yield return (object) new WaitForSecondsRealtime(1f);
        manipulatorMenuController._contentCanvasGroup.interactable = true;
        manipulatorMenuController._controlPrompts.ShowAcceptButton();
        manipulatorMenuController.SetActiveStateForMenu(true);
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(currentSelectable);
        for (int index = 0; index < manipulatorMenuController.tabNavigator.NumTabs; ++index)
        {
          if (index == 1 && manipulatorMenuController.StructureBrain.Data.Type < StructureBrain.TYPES.TRAIT_MANIPULATOR_2 || index == 2 && manipulatorMenuController.StructureBrain.Data.Type < StructureBrain.TYPES.TRAIT_MANIPULATOR_3)
            manipulatorMenuController.tabNavigator._tabs[index].SetLocked();
        }
        manipulatorMenuController.tabNavigator.SetInteractable(true);
        manipulatorMenuController._traitManipulatorMenuInfoCardController.Card1.TraitsSelectable(false);
        manipulatorMenuController._traitManipulatorMenuInfoCardController.Card2.TraitsSelectable(false);
        manipulatorMenuController._traitManipulatorMenuInfoCardController.Card1.GetComponent<CanvasGroup>().interactable = false;
        manipulatorMenuController._traitManipulatorMenuInfoCardController.Card2.GetComponent<CanvasGroup>().interactable = false;
        manipulatorMenuController._traitManipulatorMenuInfoCardController.enabled = true;
      }
      else
      {
        manipulatorMenuController._traitInfoCardController.Card1.Hide();
        manipulatorMenuController._traitInfoCardController.Card2.Hide();
        manipulatorMenuController._controlPrompts.HideCancelButton();
        currentCard.RectTransform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        AudioManager.Instance.PlayOneShot("event:/dlc/building/exorcismaltar/exorcism_confirm");
        yield return (object) manipulatorMenuController._animator.YieldForAnimation(manipulatorMenuController.kConfirmedSelectionAnimationState);
        if (manipulatorMenuController.SelectionType == UITraitManipulatorMenuController.Type.Add)
          selectedTrait = currentCard.Traits[currentCard.Traits.Count - 1].TraitType;
        Action<FollowerInfo> followerSelected = manipulatorMenuController.OnFollowerSelected;
        if (followerSelected != null)
          followerSelected(followerInfo);
        Action<FollowerInfo, UITraitManipulatorMenuController.Type, FollowerTrait.TraitType> onFollowerChosen = manipulatorMenuController.OnFollowerChosen;
        if (onFollowerChosen != null)
          onFollowerChosen(followerInfo, manipulatorMenuController.SelectionType, selectedTrait);
        manipulatorMenuController.Hide(true);
      }
    }
  }

  public bool HasActiveTraits(List<FollowerTrait.TraitType> traits)
  {
    foreach (FollowerTrait.TraitType trait in traits)
    {
      if (!FollowerTrait.UniqueTraits.Contains(trait))
        return true;
    }
    return false;
  }

  public override void OnCancelButtonInput()
  {
    if (!this.CanvasGroup.interactable || !this._cancellable)
      return;
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
    base.OnCancelButtonInput();
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void FinalizeVote(FollowerInfo followerInfo)
  {
    foreach (FollowerInformationBox followerInfoBox in this._followerInfoBoxes)
    {
      if (followerInfoBox.FollowerInfo == followerInfo)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) followerInfoBox.Button);
        if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null && (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable != (UnityEngine.Object) null)
          this._scrollRect.ScrollTo(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<RectTransform>());
        Action<FollowerInfo> followerSelected = followerInfoBox.OnFollowerSelected;
        if (followerSelected == null)
          return;
        followerSelected(followerInfo);
        return;
      }
    }
    this.Hide();
  }

  [CompilerGenerated]
  public void \u003CShow\u003Eb__14_0(int i) => this.SelectionChosen(i);

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__15_0(TraitInfoCard infoCard)
  {
    if (this.SelectionType != UITraitManipulatorMenuController.Type.Remove)
      return;
    this.holdText.text = string.Format($"{LocalizationManager.GetTranslation($"UI/TraitManipulator/{this.SelectionType}")}: <color=#FFD201>{FollowerTrait.GetLocalizedTitle(infoCard.Trait)}");
  }

  public enum Type
  {
    Shuffle,
    Remove,
    Add,
    None,
  }
}
