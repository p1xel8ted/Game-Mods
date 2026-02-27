// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PrisonerMenu.UIPrisonerMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine.Unity;
using src.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.PrisonerMenu;

public class UIPrisonerMenuController : UIMenuBase
{
  public Action<FollowerInfo> OnFollowerReleased;
  public Action<FollowerInfo> OnReEducate;
  [Header("Prisoner Menu")]
  [SerializeField]
  private TextMeshProUGUI _nameText;
  [SerializeField]
  private SkeletonGraphic _followerSkeleton;
  [Header("Buttons")]
  [SerializeField]
  private MMButton _readMindButton;
  [SerializeField]
  private MMButton _releaseButton;
  [SerializeField]
  private MMButton _reeducateButton;
  [SerializeField]
  private TextMeshProUGUI _reeducateButtonText;
  [Header("Stats")]
  [SerializeField]
  private Image _faithProgressBar;
  [SerializeField]
  private Image _faithProgressBarWhite;
  [SerializeField]
  private TextMeshProUGUI _durationText;
  [Header("Dissenter")]
  [SerializeField]
  private TextMeshProUGUI _dissenterText;
  [SerializeField]
  private GameObject _dissentingContainer;
  [SerializeField]
  private TextMeshProUGUI _noLongerDissentingText;
  [SerializeField]
  private GameObject _noLongerDissentingContainer;
  [SerializeField]
  private GameObject _wasNotDissentingContainer;
  [SerializeField]
  private GameObject _wasNotDissentingOriginalSinContainer;
  [Header("Other")]
  [SerializeField]
  private GameObject _reeducateAlert;
  [SerializeField]
  private RectTransform _reeducatedTextTransform;
  private FollowerInfo _followerInfo;
  private FollowerBrain _followerBrain;
  private StructuresData _structuresData;
  private bool _didCancel;

  public void Show(FollowerInfo followerInfo, StructuresData structuresData, bool instant = false)
  {
    this._followerInfo = followerInfo;
    this._structuresData = structuresData;
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    this._followerBrain = FollowerBrain.GetOrCreateBrain(this._followerInfo);
    this._nameText.text = this._followerInfo.Name;
    this._followerSkeleton.ConfigureFollower(this._followerInfo);
    this._followerSkeleton.ConfigurePrison(this._followerInfo, this._structuresData, true);
    this._readMindButton.onClick.AddListener(new UnityAction(this.OnReadMindButtonClicked));
    this._releaseButton.onClick.AddListener(new UnityAction(this.OnReleaseClicked));
    this._reeducateButton.onClick.AddListener(new UnityAction(this.OnReeducatePressed));
    this._reeducateButton.OnSelected += new System.Action(this.OnReeducateSelected);
    this._reeducateButton.OnDeselected += new System.Action(this.OnReeducateDeselected);
    this._reeducateButton.OnConfirmDenied += new System.Action(this.ShakeReeducate);
    if (this._followerBrain != null)
      this._reeducateButton.Confirmable = !this._followerBrain.Stats.ReeducatedAction && this._followerBrain.Info.CursedState == Thought.Dissenter;
    this._reeducateAlert.SetActive(this._reeducateButton.Confirmable);
    this.UpdateStats();
  }

  private void UpdateStats()
  {
    if (this._followerBrain != null)
    {
      this._dissenterText.text = string.Format(this._dissenterText.text, (object) this._followerBrain.Info.Name);
      this._dissentingContainer.SetActive(this._followerBrain.Info.CursedState == Thought.Dissenter);
      this._noLongerDissentingText.text = string.Format(this._noLongerDissentingText.text, (object) this._followerBrain.Info.Name);
      this._noLongerDissentingContainer.SetActive(this._followerBrain.Info.CursedState != Thought.Dissenter && (double) this._followerBrain._directInfoAccess.Reeducation <= 0.0);
      this._wasNotDissentingContainer.SetActive(!this._noLongerDissentingContainer.activeSelf && this._followerBrain.Info.CursedState != Thought.Dissenter && !DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Disciplinarian));
      this._wasNotDissentingOriginalSinContainer.SetActive(!this._noLongerDissentingContainer.activeSelf && this._followerBrain.Info.CursedState != Thought.Dissenter && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Disciplinarian));
      this._reeducatedTextTransform.gameObject.SetActive(this._followerBrain.Stats.ReeducatedAction);
      if (this._followerBrain.Info.CursedState == Thought.Dissenter)
      {
        this._faithProgressBar.fillAmount = (float) (1.0 - (double) this._followerBrain.Stats.Reeducation / 100.0);
        this._faithProgressBarWhite.fillAmount = this._faithProgressBar.fillAmount;
      }
      else
        this._faithProgressBar.fillAmount = 1f;
      this._faithProgressBar.color = StaticColors.ColorForThreshold(this._faithProgressBar.fillAmount);
    }
    if (this._structuresData == null)
      return;
    this._durationText.text = this.GetExpiryFormatted(TimeManager.TotalElapsedGameTime - this._structuresData.FollowerImprisonedTimestamp);
  }

  private string GetExpiryFormatted(float timeStamp)
  {
    return string.Format(ScriptLocalization.UI_Generic.Days, (object) Mathf.RoundToInt(timeStamp / 1200f));
  }

  private void OnReadMindButtonClicked()
  {
    UIFollowerSummaryMenuController followerSummaryMenu = MonoSingleton<UIManager>.Instance.FollowerSummaryMenuTemplate.Instantiate<UIFollowerSummaryMenuController>();
    UIFollowerSummaryMenuController summaryMenuController = followerSummaryMenu;
    summaryMenuController.OnHidden = summaryMenuController.OnHidden + (System.Action) (() =>
    {
      followerSummaryMenu = (UIFollowerSummaryMenuController) null;
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReadMind);
    });
    followerSummaryMenu.Show(FollowerManager.FindFollowerByID(this._followerInfo.ID));
    this.PushInstance<UIFollowerSummaryMenuController>(followerSummaryMenu);
  }

  private void OnReleaseClicked()
  {
    Action<FollowerInfo> followerReleased = this.OnFollowerReleased;
    if (followerReleased != null)
      followerReleased(this._followerInfo);
    this.Hide();
  }

  private void OnReeducateSelected()
  {
    if (this._followerBrain.Stats.ReeducatedAction)
      return;
    this._faithProgressBarWhite.DOFillAmount((float) (1.0 - ((double) this._followerBrain.Stats.Reeducation - 25.0) / 100.0), 0.25f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  private void OnReeducateDeselected()
  {
    if (this._followerBrain.Info.CursedState != Thought.Dissenter)
      return;
    this._faithProgressBarWhite.DOFillAmount(this._faithProgressBar.fillAmount, 0.25f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  private void OnReeducatePressed()
  {
    Action<FollowerInfo> onReEducate = this.OnReEducate;
    if (onReEducate != null)
      onReEducate(this._followerInfo);
    this.UpdateStats();
  }

  private void ShakeReeducate()
  {
    this._reeducateButtonText.DOKill();
    this._reeducateButtonText.rectTransform.anchoredPosition = Vector2.zero;
    this._reeducateButtonText.rectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
    this._reeducatedTextTransform.DOKill();
    this._reeducatedTextTransform.anchoredPosition = Vector2.zero;
    this._reeducatedTextTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public override void OnCancelButtonInput()
  {
    this._didCancel = true;
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
