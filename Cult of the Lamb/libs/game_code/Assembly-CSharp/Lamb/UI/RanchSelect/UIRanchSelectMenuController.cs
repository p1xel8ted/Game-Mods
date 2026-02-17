// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchSelect.UIRanchSelectMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.UI.RanchSelect;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI.RanchSelect;

public class UIRanchSelectMenuController : UIRanchSelectBase<RanchMenuItem>
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
  public RanchSelectMenuInfoCardController _defaultInfoCard;
  [SerializeField]
  public UIHoldInteraction _uiHoldInteraction;
  [SerializeField]
  public TMP_Text _uiHoldText;
  [SerializeField]
  public GameObject _editPostsButton;
  [SerializeField]
  public GameObject ranchSizeObject;
  [SerializeField]
  public TMP_Text ranchSizeText;
  [SerializeField]
  public GameObject ranchTooSmallObject;
  [SerializeField]
  public TMP_Text ranchTooSmallText;
  [SerializeField]
  public GameObject ritualActiveGameObject;
  [SerializeField]
  public TMP_Text ritualActiveText;
  public string kSelectedFollowerAnimationState = "Selected";
  public string kCancelSelectionAnimationState = "Cancelled";
  public string kConfirmedSelectionAnimationState = "Confirmed";
  public int _ranchCapacity;
  public bool showDefaultInfoCard;
  public bool showEditPosts;

  public void Show(
    List<RanchSelectEntry> followerSelectEntries,
    bool instant = false,
    bool hideOnSelection = true,
    bool cancellable = true,
    bool hasSelection = true,
    bool showDefaultInfoCard = false,
    bool showNecklaces = false)
  {
    this.ranchSizeObject.SetActive(false);
    this.ranchTooSmallObject.gameObject.SetActive(false);
    this._header.text = ScriptLocalization.UI_RanchMenu.SelectAnimal;
    this.Show(followerSelectEntries, instant, hideOnSelection, cancellable, hasSelection);
    this._editPostsButton.SetActive(false);
    this.ritualActiveGameObject.SetActive(false);
    if (FollowerBrainStats.IsRanchHarvest || FollowerBrainStats.IsRanchMeat)
    {
      this.ritualActiveGameObject.SetActive(true);
      this.ritualActiveText.text = !FollowerBrainStats.IsRanchHarvest ? ScriptLocalization.UI_RanchMenu.Header : ScriptLocalization.UI_RanchMenu.SelectAnimal;
    }
    this.showDefaultInfoCard = showDefaultInfoCard;
    if ((UnityEngine.Object) this._defaultInfoCard != (UnityEngine.Object) null)
      this._defaultInfoCard.gameObject.SetActive(showDefaultInfoCard);
    this.Show(followerSelectEntries, instant, true, true, true);
  }

  public void Show(
    List<RanchSelectEntry> followerSelectEntries,
    int capacity,
    bool instant,
    bool showDefaultInfoCard,
    bool showEditPosts,
    Structures_Ranch brain)
  {
    this.ranchSizeObject.gameObject.SetActive(true);
    int num1;
    if ((double) brain.Capacity <= (double) brain.RanchingTiles.Count / 2.0)
    {
      this.ranchTooSmallObject.gameObject.SetActive(true);
      TMP_Text ranchTooSmallText = this.ranchTooSmallText;
      string[] strArray = new string[5]
      {
        ScriptLocalization.UI_RanchMenu.RequiresBiggerRanch,
        "  ",
        null,
        null,
        null
      };
      num1 = brain.RanchingTiles.Count;
      strArray[2] = num1.ToString();
      strArray[3] = "/";
      strArray[4] = ((float) brain.Capacity * 3f).ToString();
      string str = string.Concat(strArray);
      ranchTooSmallText.text = str;
    }
    else
      this.ranchTooSmallObject.gameObject.SetActive(false);
    float num2 = (float) brain.RanchingTiles.Count / 3f;
    if ((double) brain.Capacity < (double) num2)
      num2 = (float) brain.Capacity;
    TMP_Text ranchSizeText = this.ranchSizeText;
    string[] strArray1 = new string[11];
    strArray1[0] = ScriptLocalization.UI_RanchMenu.RanchSize;
    strArray1[1] = " ";
    num1 = brain.RanchingTiles.Count;
    strArray1[2] = num1.ToString();
    strArray1[3] = "/";
    strArray1[4] = ((float) brain.Capacity * 3f).ToString();
    strArray1[5] = "\n";
    strArray1[6] = ScriptLocalization.UI_RanchMenu.AnimalPlotCapacity;
    strArray1[7] = " ";
    strArray1[8] = num2.ToString();
    strArray1[9] = "/";
    num1 = brain.Capacity;
    strArray1[10] = num1.ToString();
    string str1 = string.Concat(strArray1);
    ranchSizeText.text = str1;
    this._header.text = ScriptLocalization.UI_RanchMenu.Header;
    this._ranchCapacity = capacity;
    this.showEditPosts = showEditPosts;
    this._editPostsButton.SetActive(showEditPosts);
    this._controlPrompts.HideAcceptButton();
    this.ritualActiveGameObject.SetActive(false);
    if (FollowerBrainStats.IsRanchHarvest || FollowerBrainStats.IsRanchMeat)
    {
      this.ritualActiveGameObject.SetActive(true);
      this.ritualActiveText.text = !FollowerBrainStats.IsRanchHarvest ? ScriptLocalization.UI_RanchMenu.Header : ScriptLocalization.UI_RanchMenu.SelectAnimal;
    }
    this.Show(followerSelectEntries, capacity, instant);
  }

  public void Update()
  {
    if (!this._canvasGroup.interactable || !this.showEditPosts || !InputManager.UI.GetEditBuildingsButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      return;
    System.Action onEditPosts = this.OnEditPosts;
    if (onEditPosts != null)
      onEditPosts();
    this.Hide();
  }

  public override RanchMenuItem PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.RanchInformationBoxTemplate;
  }

  public void SetBackgroundState(bool active) => this.blurBackground.gameObject.SetActive(active);

  public override void FollowerSelected(RanchSelectEntry followerInfo)
  {
    if (this.showDefaultInfoCard)
    {
      this._defaultInfoCard.enabled = false;
      if ((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null && MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null)
        this.OverrideDefaultOnce(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable);
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      this.SetActiveStateForMenu(false);
      this.StartCoroutine((IEnumerator) this.FocusFollowerCard(followerInfo));
    }
    else
      base.FollowerSelected(followerInfo);
  }

  public IEnumerator FocusFollowerCard(RanchSelectEntry followerInfo)
  {
    UIRanchSelectMenuController selectMenuController = this;
    selectMenuController._controlPrompts.HideAcceptButton();
    selectMenuController._uiHoldInteraction.Reset();
    RanchSelectMenuInfoCard currentCard = selectMenuController._defaultInfoCard.CurrentCard;
    currentCard.RectTransform.SetParent((Transform) selectMenuController._rootContainer, true);
    currentCard.RectTransform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    selectMenuController._animator.Play(selectMenuController.kSelectedFollowerAnimationState);
    yield return (object) new WaitForSecondsRealtime(1f);
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
      AudioManager.Instance.PlayOneShot("event:/ui/close_menu");
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
      Action<RanchSelectEntry> onAnimalSelected = selectMenuController.OnAnimalSelected;
      if (onAnimalSelected != null)
        onAnimalSelected(followerInfo);
      selectMenuController.Hide(true);
    }
    yield return (object) null;
  }

  public void SetHoldText(string text) => this._uiHoldText.text = text;
}
