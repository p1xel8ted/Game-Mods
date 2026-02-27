// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIUpgradeUnlockOverlayControllerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using src.UI.InfoCards;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public abstract class UIUpgradeUnlockOverlayControllerBase : UIMenuBase
{
  public System.Action OnUnlocked;
  [SerializeField]
  private UpgradeTreeInfoCard infoCardBase;
  [SerializeField]
  private RectTransform _redOutline;
  [SerializeField]
  private UIHoldInteraction _holdInteraction;
  [SerializeField]
  private ParticleSystem _particleSystem;
  [SerializeField]
  private ParticleSystem _particleSystemExplode;
  [SerializeField]
  private Image _redFlash;
  protected UpgradeTreeNode _node;
  protected UpgradeSystem.Type _upgrade;
  private EventInstance? _loopingSound;
  private bool _tooLateToCancel;
  public ParticleSystem.EmitParams emitParams;

  public void Show(UpgradeTreeNode node, bool instant = false)
  {
    this._node = node;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    this.infoCardBase.Show(node, false);
    this._holdInteraction.gameObject.SetActive(this.NodeAvailable() && this.IsAvailable());
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    base.OnShowStarted();
    this._particleSystem.Clear();
  }

  protected override void OnShowCompleted()
  {
    if (!this.NodeAvailable() || !this.IsAvailable())
      return;
    this.StartCoroutine((IEnumerator) this.RunMenu());
  }

  private IEnumerator RunMenu()
  {
    UIUpgradeUnlockOverlayControllerBase overlayControllerBase = this;
    ParticleSystem.EmissionModule particleSystemEmission = overlayControllerBase._particleSystem.emission;
    bool cancel = false;
    overlayControllerBase._tooLateToCancel = false;
    yield return (object) overlayControllerBase._holdInteraction.DoHoldInteraction(new Action<float>(UpdateProgress), new System.Action(Cancel));
    if (overlayControllerBase._loopingSound.HasValue)
      AudioManager.Instance.StopLoop(overlayControllerBase._loopingSound.Value);
    MMVibrate.StopRumble();
    if (!cancel)
    {
      overlayControllerBase._tooLateToCancel = true;
      overlayControllerBase._holdInteraction.gameObject.SetActive(false);
      UIManager.PlayAudio("event:/hearts_of_the_faithful/draw_power_end");
      overlayControllerBase._redFlash.gameObject.SetActive(true);
      DOTweenModuleUI.DOFade(overlayControllerBase._redFlash, 0.0f, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.3f);
      overlayControllerBase._redOutline.DOScale(Vector3.zero, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      overlayControllerBase.infoCardBase.RectTransform.DOScale(Vector3.one, 0.5f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      overlayControllerBase.CanvasGroup.DOFade(0.0f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetDelay<TweenerCore<float, float, FloatOptions>>(0.8f);
      overlayControllerBase._particleSystem.Clear();
      overlayControllerBase._particleSystem.Stop();
      overlayControllerBase._particleSystemExplode.Play();
      yield return (object) new WaitForSecondsRealtime(1.3f);
    }
    if (cancel)
    {
      yield return (object) new WaitForSecondsRealtime(0.3f);
      overlayControllerBase.infoCardBase.RectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    }
    if (cancel)
    {
      System.Action onCancel = overlayControllerBase.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    else
    {
      System.Action onUnlocked = overlayControllerBase.OnUnlocked;
      if (onUnlocked != null)
        onUnlocked();
    }
    overlayControllerBase.Hide();

    void UpdateProgress(float progress)
    {
      float num1 = (float) (1.0 - 0.25 * (double) progress);
      this.infoCardBase.RectTransform.localScale = new Vector3(num1, num1, num1);
      this.infoCardBase.RectTransform.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * progress * this._holdInteraction.HoldTime * 2f);
      if (this._redOutline.gameObject.activeSelf != (double) progress > 0.0)
        this._redOutline.gameObject.SetActive((double) progress > 0.0);
      this._redOutline.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 1.2f, 1.2f), progress);
      if (InputManager.UI.GetAcceptButtonHeld())
      {
        particleSystemEmission.rateOverTime = (ParticleSystem.MinMaxCurve) (float) (5.0 + (double) progress * 9.0);
        if (!this._particleSystem.isPlaying)
          this._particleSystem.Play();
        MMVibrate.RumbleContinuous(progress * 0.2f, progress * 0.2f);
        if (!this._loopingSound.HasValue)
          this._loopingSound = new EventInstance?(AudioManager.Instance.CreateLoop("event:/hearts_of_the_faithful/draw_power_loop", true));
      }
      else
      {
        MMVibrate.StopRumble();
        this._particleSystem.Stop();
      }
      ref EventInstance? local = ref this._loopingSound;
      if (!local.HasValue)
        return;
      int num2 = (int) local.GetValueOrDefault().setParameterByName("power", progress);
    }

    void Cancel()
    {
      if (this._tooLateToCancel)
        return;
      cancel = true;
      MMVibrate.StopRumble();
    }
  }

  public override void OnCancelButtonInput()
  {
    if (this._tooLateToCancel)
      return;
    if (this._loopingSound.HasValue)
      AudioManager.Instance.StopLoop(this._loopingSound.Value);
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideStarted()
  {
    this._redFlash.gameObject.SetActive(false);
    this._particleSystem.Stop();
    this._particleSystem.Clear();
    this._redOutline.gameObject.SetActive(false);
    this.infoCardBase.Hide();
  }

  protected override void OnHideCompleted()
  {
    if (this._loopingSound.HasValue)
      AudioManager.Instance.StopLoop(this._loopingSound.Value);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private bool NodeAvailable()
  {
    return this._node.State == UpgradeTreeNode.NodeState.Available && !UpgradeSystem.GetUnlocked(this._upgrade);
  }

  protected abstract bool IsAvailable();
}
