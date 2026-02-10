// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIMysticShopOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using src.UI.Overlays.MysticShopOverlay;
using src.Utilities;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIMysticShopOverlayController : UIMenuBase
{
  public Action<InventoryItem.ITEM_TYPE> OnRewardChosen;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [Header("Controllers")]
  [SerializeField]
  public RadiusController _radiusController;
  [SerializeField]
  public FlashController _flashController;
  [Header("Wheel")]
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public RectTransform _subContainer;
  [SerializeField]
  public AnimationCurve _spinCurve;
  [SerializeField]
  public RectTransform _dial2;
  [SerializeField]
  public RectTransform _dial3;
  [SerializeField]
  public RectTransform _innerCircle1;
  [SerializeField]
  public RectTransform _innerCircle2;
  [Header("Selection Dial")]
  [SerializeField]
  public FlashController _chosenFlashController;
  [SerializeField]
  public MysticShopRewardOption _chosenOption;
  [SerializeField]
  public RectTransform _dial;
  [SerializeField]
  public GameObject _dialGraphic;
  [Header("Options")]
  [SerializeField]
  public MMRadialLayoutGroup _optionsLayoutGroup;
  [SerializeField]
  public RectTransform _optionsContainer;
  [SerializeField]
  public MysticShopRingInnerRenderer _ringRenderer;
  [Header("Flourishes")]
  [SerializeField]
  public MMRotatedLayoutGroup _flourishLayout;
  [SerializeField]
  public RectTransform _flourishContainer;
  [Header("BG")]
  [SerializeField]
  public Image _backgroundFX;
  public MysticShopRewardOption[] _rewardOptions;
  public MysticShopFlourishRenderer[] _flourishes;
  public WeightedCollection<InventoryItem.ITEM_TYPE> _rewards;
  public EventInstance _sfx;

  public override void Awake()
  {
    base.Awake();
    this._chosenFlashController.enabled = false;
    this._flashController.Flash = 1f;
    this._radiusController.Expansion = 0.0f;
    this._chosenOption.gameObject.SetActive(false);
    this._rewardOptions = this._optionsContainer.GetComponentsInChildren<MysticShopRewardOption>();
    this._flourishes = this._flourishContainer.GetComponentsInChildren<MysticShopFlourishRenderer>();
    if (!DataManager.Instance.MysticShopUsed)
      this._controlPrompts.HideAcceptButton();
    this._container.DOScale(Vector3.one * 1.1f, 7.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetLoops<TweenerCore<Vector3, Vector3, VectorOptions>>(-1, DG.Tweening.LoopType.Yoyo).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void Show(
    WeightedCollection<InventoryItem.ITEM_TYPE> rewards,
    bool instant = false)
  {
    this._rewards = rewards;
    this._flourishLayout.Offset = (float) (360.0 / (double) rewards.Count / 2.0);
    this._ringRenderer.Segments = rewards.Count;
    for (int index = 0; index < this._rewardOptions.Length; ++index)
    {
      if (index > this._rewards.Count - 1)
      {
        this._rewardOptions[index].gameObject.SetActive(false);
        this._flourishes[index].gameObject.SetActive(false);
      }
      else
      {
        this._rewardOptions[index].Configure(this._rewards[index], Interaction_MysticShop.RelicToUnlock);
        if (this._rewardOptions.Length == 3)
          this._flourishes[index].Fill = 1f;
        else if (this._rewardOptions.Length == 4)
          this._flourishes[index].Fill = 0.75f;
      }
    }
    this._sfx = AudioManager.Instance.PlayOneShotWithInstance("event:/mystic/mystic_wheel_spin");
    this.Show(instant);
  }

  public override void OnShowStarted()
  {
    this.StartCoroutine((IEnumerator) this.DoSpin());
    this._backgroundFX.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(this._backgroundFX, 1f, 1f);
  }

  public IEnumerator DoSpin()
  {
    UIMysticShopOverlayController overlayController = this;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    int index = overlayController._rewards.GetRandomIndex();
    int num = UnityEngine.Random.Range(7, 8);
    float duration = 5f;
    float z = (float) (index * -(360 / overlayController._rewards.Count) + 360 * num);
    overlayController._dial.DORotate(new Vector3(0.0f, 0.0f, z), duration, RotateMode.LocalAxisAdd).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(overlayController._spinCurve).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
    overlayController._dial2.DORotate(new Vector3(0.0f, 0.0f, z / 2f), duration, RotateMode.LocalAxisAdd).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(overlayController._spinCurve).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
    overlayController._dial3.DORotate(new Vector3(0.0f, 0.0f, z / 4f), duration, RotateMode.LocalAxisAdd).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(overlayController._spinCurve).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
    overlayController._innerCircle1.DORotate(new Vector3(0.0f, 0.0f, (float) (-(double) z / 6.0)), duration, RotateMode.LocalAxisAdd).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(overlayController._spinCurve).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
    overlayController._innerCircle2.DORotate(new Vector3(0.0f, 0.0f, (float) (-(double) z / 8.0)), duration, RotateMode.LocalAxisAdd).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(overlayController._spinCurve).SetUpdate<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(true);
    while ((double) duration > 0.0)
    {
      duration -= Time.unscaledDeltaTime;
      if (InputManager.Gameplay.GetAttackButtonDown() && DataManager.Instance.MysticShopUsed)
      {
        overlayController._animator.Play("Shown");
        overlayController._dial.DOComplete();
        overlayController._dial2.DOComplete();
        overlayController._dial3.DOComplete();
        overlayController._innerCircle1.DOComplete();
        overlayController._innerCircle2.DOComplete();
        AudioManager.Instance.StopOneShotInstanceEarly(overlayController._sfx, STOP_MODE.IMMEDIATE);
        break;
      }
      yield return (object) null;
    }
    overlayController._controlPrompts.HideAcceptButton();
    UIManager.PlayAudio("event:/mystic/mystic_prize_select");
    yield return (object) new WaitForSecondsRealtime(0.1f);
    overlayController._flashController.enabled = false;
    overlayController._rewardOptions[index].Choose();
    overlayController._dialGraphic.SetActive(false);
    overlayController._chosenOption.gameObject.SetActive(true);
    overlayController._chosenOption.Configure(overlayController._rewards[index], Interaction_MysticShop.RelicToUnlock);
    overlayController._chosenOption.transform.DOPunchScale(Vector3.one * 0.15f, 2f, 3).SetEase<Tweener>(Ease.OutBounce).SetUpdate<Tweener>(true);
    overlayController._subContainer.transform.DOPunchScale(Vector3.one * 0.05f, 2f, 3).SetEase<Tweener>(Ease.OutBounce).SetUpdate<Tweener>(true);
    overlayController._chosenFlashController.enabled = true;
    overlayController._chosenFlashController.Flash = 1f;
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoSpin\u003Eb__28_0), new DOSetter<float>(overlayController.\u003CDoSpin\u003Eb__28_1), 0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    yield return (object) new WaitForSecondsRealtime(1.5f);
    overlayController._chosenFlashController.enabled = false;
    overlayController._flashController.enabled = true;
    Action<InventoryItem.ITEM_TYPE> onRewardChosen = overlayController.OnRewardChosen;
    if (onRewardChosen != null)
      onRewardChosen(overlayController._rewards[index]);
    UIManager.PlayAudio("event:/mystic/mystic_wheel_shrink");
    DOTweenModuleUI.DOFade(overlayController._backgroundFX, 0.0f, 1f);
    overlayController.Hide();
  }

  public override void OnHideCompleted()
  {
    DataManager.Instance.MysticShopUsed = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    MonoSingleton<UIManager>.Instance.UnloadMysticShopAssets();
  }

  [CompilerGenerated]
  public float \u003CDoSpin\u003Eb__28_0() => this._chosenFlashController.Flash;

  [CompilerGenerated]
  public void \u003CDoSpin\u003Eb__28_1(float x) => this._chosenFlashController.Flash = x;
}
