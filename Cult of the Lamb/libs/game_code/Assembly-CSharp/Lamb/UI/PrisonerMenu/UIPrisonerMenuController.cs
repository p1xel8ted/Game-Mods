// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PrisonerMenu.UIPrisonerMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public TextMeshProUGUI _nameText;
  [SerializeField]
  public SkeletonGraphic _followerSkeleton;
  [Header("Buttons")]
  [SerializeField]
  public MMButton _readMindButton;
  [SerializeField]
  public MMButton _releaseButton;
  [SerializeField]
  public MMButton _reeducateButton;
  [SerializeField]
  public TextMeshProUGUI _reeducateButtonText;
  [Header("Stats")]
  [SerializeField]
  public Image _faithProgressBar;
  [SerializeField]
  public Image _faithProgressBarWhite;
  [SerializeField]
  public Image _faithGlow;
  [SerializeField]
  public Gradient _gradient;
  [SerializeField]
  public TextMeshProUGUI _durationText;
  [Header("Dissenter")]
  [SerializeField]
  public TextMeshProUGUI _dissenterText;
  [SerializeField]
  public GameObject _dissentingContainer;
  [SerializeField]
  public TextMeshProUGUI _noLongerDissentingText;
  [SerializeField]
  public GameObject _noLongerDissentingContainer;
  [SerializeField]
  public GameObject _wasNotDissentingContainer;
  [SerializeField]
  public GameObject _wasNotDissentingOriginalSinContainer;
  [Header("Other")]
  [SerializeField]
  public GameObject _reeducateAlert;
  [SerializeField]
  public RectTransform _reeducatedTextTransform;
  public FollowerInfo _followerInfo;
  public FollowerBrain _followerBrain;
  public StructuresData _structuresData;
  public bool _didCancel;

  public void Show(FollowerInfo followerInfo, StructuresData structuresData, bool instant = false)
  {
    this._followerInfo = followerInfo;
    this._structuresData = structuresData;
    this.Show(instant);
  }

  public override void OnShowStarted()
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

  public void UpdateStats()
  {
    if (this._followerBrain != null)
    {
      this._dissenterText.text = ScriptLocalization.Notifications_Follower.BecomeDissenter;
      this._dissenterText.text = string.Format(this._dissenterText.text, (object) this._followerBrain.Info.Name);
      this._dissentingContainer.SetActive(this._followerBrain.Info.CursedState == Thought.Dissenter);
      this._noLongerDissentingText.text = ScriptLocalization.Notifications_Cult_CureDissenter_Notification.On;
      this._noLongerDissentingText.text = string.Format(this._noLongerDissentingText.text, (object) this._followerBrain.Info.Name);
      this._noLongerDissentingContainer.SetActive(this._followerBrain.Info.CursedState != Thought.Dissenter && (double) this._followerBrain._directInfoAccess.Reeducation <= 0.0);
      this._wasNotDissentingContainer.SetActive(!this._noLongerDissentingContainer.activeSelf && this._followerBrain.Info.CursedState != Thought.Dissenter && !DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Disciplinarian));
      this._wasNotDissentingOriginalSinContainer.SetActive(!this._noLongerDissentingContainer.activeSelf && this._followerBrain.Info.CursedState != Thought.Dissenter && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Disciplinarian));
      this._reeducatedTextTransform.gameObject.SetActive(this._followerBrain.Stats.ReeducatedAction);
      if (this._followerBrain.Info.CursedState == Thought.Dissenter)
      {
        this._faithProgressBar.fillAmount = this._followerBrain.Stats.Reeducation / 100f;
        this._faithProgressBarWhite.fillAmount = (float) (((double) this._followerBrain.Stats.Reeducation - 25.0) / 100.0);
      }
      else
      {
        this._faithProgressBar.fillAmount = 1f;
        this._faithProgressBarWhite.fillAmount = 0.0f;
        this._faithProgressBar.color = StaticColors.GreenColor;
      }
      this._faithProgressBarWhite.color = this._gradient.Evaluate(1f - this._faithProgressBarWhite.fillAmount);
      this._faithGlow.color = this._gradient.Evaluate(1f - this._faithProgressBarWhite.fillAmount);
    }
    if (this._structuresData == null || this._structuresData == null)
      return;
    this._durationText.text = this.GetExpiryFormatted(TimeManager.TotalElapsedGameTime - this._structuresData.FollowerImprisonedTimestamp);
  }

  public string GetExpiryFormatted(float timeStamp)
  {
    return string.Format(ScriptLocalization.UI_Generic.Days, (object) Mathf.RoundToInt(timeStamp / 1200f));
  }

  public void OnReadMindButtonClicked()
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

  public void OnReleaseClicked()
  {
    Action<FollowerInfo> followerReleased = this.OnFollowerReleased;
    if (followerReleased != null)
      followerReleased(this._followerInfo);
    this.Hide();
  }

  public void OnReeducateSelected()
  {
    if (this._followerBrain.Stats.ReeducatedAction || this._followerBrain.Info.CursedState != Thought.Dissenter)
      return;
    this._faithProgressBarWhite.DOFillAmount((float) (((double) this._followerBrain.Stats.Reeducation - 25.0) / 100.0), 0.25f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public void OnReeducateDeselected()
  {
    if (this._followerBrain.Info.CursedState != Thought.Dissenter)
      return;
    this._faithProgressBarWhite.DOFillAmount(this._faithProgressBar.fillAmount, 0.25f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public void OnReeducatePressed()
  {
    Action<FollowerInfo> onReEducate = this.OnReEducate;
    if (onReEducate != null)
      onReEducate(this._followerInfo);
    this.UpdateStats();
  }

  public void ShakeReeducate()
  {
    this._reeducateButtonText.rectTransform.DOKill(true);
    this._reeducateButtonText.rectTransform.localPosition = (Vector3) Vector2.zero;
    this._reeducateButtonText.rectTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
    this._reeducatedTextTransform.DOKill(true);
    this._reeducatedTextTransform.localPosition = (Vector3) Vector2.zero;
    this._reeducatedTextTransform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public override void OnCancelButtonInput()
  {
    this._didCancel = true;
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public string CapacityString(InventoryItem.ITEM_TYPE type, int minimum)
  {
    int itemQuantity = Inventory.GetItemQuantity(type);
    string str = $"{FontImageNames.GetIconByType(type)} {itemQuantity}/{minimum}";
    if (itemQuantity < minimum)
      str = str.Colour(StaticColors.DarkRedColor);
    return str;
  }
}
