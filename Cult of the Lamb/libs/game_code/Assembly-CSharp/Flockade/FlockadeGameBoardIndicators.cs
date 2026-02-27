// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeGameBoardIndicators
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeGameBoardIndicators : MonoBehaviour
{
  public const string _FLIPPING_INDICATOR_SOUND = "event:/dlc/ui/flockade_minigame/battle_order_reverse";
  public const string _RESTORING_INDICATOR_SOUND = "event:/dlc/ui/flockade_minigame/battle_order_forward";
  public const string _HIGHLIGHTING_LINE_SOUND = "event:/dlc/ui/flockade_minigame/battle_duel_lines_highlight";
  public const float STATE_CHANGE_DURATION = 0.166666672f;
  public const Ease _ACTIVATION_EASING = Ease.InOutQuad;
  public const Ease _ACTIVATION_FRONT_ICON_OPACITY_EASING = Ease.InSine;
  public const float _ACTIVATION_SCALE = 1.25f;
  public const float _ANTICIPATION_DURATION = 0.166666672f;
  public const float _ANTICIPATED_ANGLE = 15f;
  public const float _ANTICIPATED_SCALE = 1.3f;
  public const float _FLIPPING_DURATION = 0.333333343f;
  public const Ease _FLIPPING_EASING = Ease.InBack;
  public const float _FLIPPED_SCALE = 1f;
  public const float _FLIPPED_ANGLE = -180f;
  public const Ease _STATE_CHANGE_EASING = Ease.OutCubic;
  [SerializeField]
  public TextMeshProUGUI _backIcon;
  [SerializeField]
  public TextMeshProUGUI _frontIcon;
  [SerializeField]
  public FlockadeLineRenderer _backLine;
  [SerializeField]
  public FlockadeLineRenderer _frontLine;
  [SerializeField]
  public Color _disabled;
  [SerializeField]
  public Color _enabled;
  public bool _isFlipped;
  public FlockadeGameBoardIndicators.Line _line;
  public DG.Tweening.Sequence _activationSequence;
  public DG.Tweening.Sequence _flippingSequence;
  public float _flippingOrigin;
  public float _flippingDestination;
  public DG.Tweening.Sequence _stateChangeSequence;
  public bool _disableIconStateChange;

  public virtual void Awake() => this._backIcon.alpha = 0.0f;

  public DG.Tweening.Sequence SetEnabledLine(FlockadeGameBoardIndicators.Line line, bool silently = false)
  {
    if (line == this._line)
      return (DG.Tweening.Sequence) null;
    DG.Tweening.Sequence activationSequence = this._activationSequence;
    if (activationSequence != null)
      activationSequence.Complete(true);
    this._activationSequence = (DG.Tweening.Sequence) null;
    DG.Tweening.Sequence flippingSequence = this._flippingSequence;
    if (flippingSequence != null)
      flippingSequence.Complete(true);
    this._flippingSequence = (DG.Tweening.Sequence) null;
    DG.Tweening.Sequence stateChangeSequence = this._stateChangeSequence;
    if (stateChangeSequence != null)
      stateChangeSequence.Kill();
    this._stateChangeSequence = DOTween.Sequence();
    if (line != FlockadeGameBoardIndicators.Line.None && line != this._line && !silently)
      this._stateChangeSequence.AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/battle_duel_lines_highlight")));
    int num1 = this._line != FlockadeGameBoardIndicators.Line.None ? 1 : 0;
    bool flag = line != 0;
    int num2 = flag ? 1 : 0;
    if (num1 != num2)
    {
      Color to = flag ? this._enabled : this._disabled;
      this._disableIconStateChange = false;
      this._stateChangeSequence.Join((Tween) DOVirtual.Color(this._frontIcon.color, to, 0.166666672f, (TweenCallback<Color>) (color =>
      {
        if (this._disableIconStateChange)
          return;
        this._frontIcon.color = color;
      })).SetEase<Tweener>(Ease.OutCubic)).Join((Tween) DOVirtual.Color(this._backIcon.color, to, 0.166666672f, (TweenCallback<Color>) (color =>
      {
        if (this._disableIconStateChange)
          return;
        this._backIcon.color = color;
      })).SetEase<Tweener>(Ease.OutCubic));
    }
    this._stateChangeSequence.Join((Tween) DOVirtual.Color(this._frontLine.Root.Color, line == FlockadeGameBoardIndicators.Line.Front ? this._enabled : this._disabled, 0.166666672f, (TweenCallback<Color>) (color => FlockadeGameBoardIndicators.SetColor(this._frontLine.Root, color))).SetEase<Tweener>(Ease.OutCubic)).Join((Tween) DOVirtual.Color(this._backLine.Root.Color, line == FlockadeGameBoardIndicators.Line.Back ? this._enabled : this._disabled, 0.166666672f, (TweenCallback<Color>) (color => FlockadeGameBoardIndicators.SetColor(this._backLine.Root, color))).SetEase<Tweener>(Ease.OutCubic));
    this._line = line;
    return this._stateChangeSequence;
  }

  public DG.Tweening.Sequence ActivateIcon()
  {
    bool appliedOnFrontIcon = false;
    DG.Tweening.Sequence flippingSequence = this._flippingSequence;
    if (flippingSequence != null)
      flippingSequence.Complete(true);
    this._flippingSequence = (DG.Tweening.Sequence) null;
    DG.Tweening.Sequence activationSequence = this._activationSequence;
    if (activationSequence != null)
      activationSequence.Complete(true);
    this._activationSequence = DOTween.Sequence();
    if (this._line == FlockadeGameBoardIndicators.Line.None)
    {
      this._disableIconStateChange = true;
      this._activationSequence.Append((Tween) ShortcutExtensionsTMPText.DOColor(this._frontIcon, this._enabled, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic)).Join((Tween) ShortcutExtensionsTMPText.DOColor(this._backIcon, this._enabled, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic));
    }
    this._activationSequence.Append((Tween) this._frontIcon.rectTransform.DOScale(1.25f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad)).Join((Tween) ShortcutExtensionsTMPText.DOFade(this._frontIcon, 0.0f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InSine)).Join((Tween) ShortcutExtensionsTMPText.DOFade(this._backIcon, 1f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.InOutQuad)).AppendCallback((TweenCallback) (() =>
    {
      if (appliedOnFrontIcon)
        return;
      this._frontIcon.rectTransform.localScale = Vector3.one;
      this._frontIcon.alpha = 1f;
      this._backIcon.alpha = 0.0f;
      appliedOnFrontIcon = true;
    }));
    if (this._line == FlockadeGameBoardIndicators.Line.None)
      this._activationSequence.AppendCallback((TweenCallback) (() =>
      {
        this._line = ~FlockadeGameBoardIndicators.Line.None;
        this.SetEnabledLine(FlockadeGameBoardIndicators.Line.None);
      }));
    return this._activationSequence;
  }

  public DG.Tweening.Sequence FlipIcon()
  {
    this._isFlipped = !this._isFlipped;
    float num = this._frontIcon.rectTransform.localEulerAngles.z;
    DG.Tweening.Sequence activationSequence = this._activationSequence;
    if (activationSequence != null)
      activationSequence.Complete(true);
    this._activationSequence = (DG.Tweening.Sequence) null;
    if (this._flippingSequence != null && this._flippingSequence.IsActive() && !this._flippingSequence.IsComplete())
    {
      DG.Tweening.Sequence flippingSequence = this._flippingSequence;
      if (flippingSequence != null)
        flippingSequence.Kill();
      num = this._flippingOrigin;
      this._flippingDestination += -180f;
    }
    else
    {
      this._flippingOrigin = num;
      this._flippingDestination = num - 180f;
    }
    this._flippingSequence = DOTween.Sequence();
    if (this._line == FlockadeGameBoardIndicators.Line.None)
    {
      this._disableIconStateChange = true;
      this._flippingSequence.Append((Tween) ShortcutExtensionsTMPText.DOColor(this._frontIcon, this._enabled, 0.166666672f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutCubic));
    }
    this._flippingSequence.AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot(this._isFlipped ? "event:/dlc/ui/flockade_minigame/battle_order_reverse" : "event:/dlc/ui/flockade_minigame/battle_order_forward"))).Append((Tween) this._frontIcon.rectTransform.DORotate(new Vector3(0.0f, 0.0f, num + 15f), 0.166666672f, RotateMode.FastBeyond360).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InBack)).Join((Tween) this._frontIcon.rectTransform.DOScale(1.3f, 0.166666672f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack)).Append((Tween) this._frontIcon.rectTransform.DORotate(new Vector3(0.0f, 0.0f, this._flippingDestination), 0.333333343f, RotateMode.FastBeyond360).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InBack)).Join((Tween) this._frontIcon.rectTransform.DOScale(1f, 0.333333343f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack));
    if (this._line == FlockadeGameBoardIndicators.Line.None)
      this._flippingSequence.AppendCallback((TweenCallback) (() =>
      {
        this._line = ~FlockadeGameBoardIndicators.Line.None;
        this.SetEnabledLine(FlockadeGameBoardIndicators.Line.None);
      }));
    this._backIcon.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, this._flippingDestination);
    this._backIcon.alpha = 0.0f;
    return this._flippingSequence;
  }

  public static void SetColor(FlockadeLineRenderer.Branch branch, Color color)
  {
    branch.Color = color;
    foreach (FlockadeLineRenderer.BranchPoint point in branch.Points)
    {
      foreach (FlockadeLineRenderer.Branch branch1 in point.Branches)
        FlockadeGameBoardIndicators.SetColor(branch1, color);
    }
  }

  [CompilerGenerated]
  public void \u003CSetEnabledLine\u003Eb__31_3(Color color)
  {
    if (this._disableIconStateChange)
      return;
    this._frontIcon.color = color;
  }

  [CompilerGenerated]
  public void \u003CSetEnabledLine\u003Eb__31_4(Color color)
  {
    if (this._disableIconStateChange)
      return;
    this._backIcon.color = color;
  }

  [CompilerGenerated]
  public void \u003CSetEnabledLine\u003Eb__31_1(Color color)
  {
    FlockadeGameBoardIndicators.SetColor(this._frontLine.Root, color);
  }

  [CompilerGenerated]
  public void \u003CSetEnabledLine\u003Eb__31_2(Color color)
  {
    FlockadeGameBoardIndicators.SetColor(this._backLine.Root, color);
  }

  [CompilerGenerated]
  public void \u003CFlipIcon\u003Eb__33_0()
  {
    AudioManager.Instance.PlayOneShot(this._isFlipped ? "event:/dlc/ui/flockade_minigame/battle_order_reverse" : "event:/dlc/ui/flockade_minigame/battle_order_forward");
  }

  [CompilerGenerated]
  public void \u003CFlipIcon\u003Eb__33_1()
  {
    this._line = ~FlockadeGameBoardIndicators.Line.None;
    this.SetEnabledLine(FlockadeGameBoardIndicators.Line.None);
  }

  public enum Line
  {
    None,
    Front,
    Back,
  }
}
