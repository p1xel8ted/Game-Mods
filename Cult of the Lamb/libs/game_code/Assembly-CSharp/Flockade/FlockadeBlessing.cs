// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeBlessing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeBlessing : MonoBehaviour
{
  public const float ACTIVATION_DURATION = 0.5f;
  public const float _APPEARANCE_DISAPPEARANCE_DURATION = 0.233333334f;
  public const float _NULLIFICATION_DURATION = 0.6166667f;
  public const Ease _APPEARANCE_SCALE_EASING = Ease.OutBack;
  public const Ease _APPEARANCE_EASING = Ease.OutCubic;
  public const Ease _DISAPPEARANCE_SCALE_EASING = Ease.InBack;
  public const Ease _DISAPPEARANCE_EASING = Ease.InCubic;
  public const Ease _NULLIFICATION_EASING = Ease.InSine;
  public const Ease _ACTIVATION_EASING = Ease.InOutQuad;
  public const Ease _ACTIVATION_FRONT_BUFFER_OPACITY_EASING = Ease.InSine;
  public const float _ACTIVATION_SCALE = 1.25f;
  public const float _NULLIFICATION_SHAKE_RANDOMNESS = 0.0f;
  public const float _NULLIFICATION_SHAKE_STRENGTH = 4f;
  public const int _NULLIFICATION_SHAKE_VIBRATO = 20;
  public const float _TILT_ANGLE = -10f;
  [SerializeField]
  public FlockadeBlessing.BlessingBuffer _frontBuffer;
  [SerializeField]
  public FlockadeBlessing.BlessingBuffer _backBuffer;
  [Header("Specifics")]
  [SerializeField]
  public bool _animated = true;
  [SerializeField]
  public bool _flush = true;
  [SerializeField]
  public Color _consumed;
  [SerializeField]
  public Color _nullified;
  [SerializeField]
  public bool _noBackground;
  public FlockadeGamePiece _gamePiece;
  public DG.Tweening.Sequence _sequence;

  public virtual void Awake()
  {
    DG.Tweening.Sequence t = this.Hide();
    if (t == null)
      return;
    t.Complete(true);
  }

  public void Bind(FlockadeGamePiece gamePiece)
  {
    if ((UnityEngine.Object) this._gamePiece == (UnityEngine.Object) gamePiece)
      return;
    if ((bool) (UnityEngine.Object) this._gamePiece)
    {
      this._gamePiece.Changed -= new Action<IFlockadeGamePiece.State?>(this.UpdateVisibility);
      this._gamePiece = (FlockadeGamePiece) null;
      this.UpdateVisibility(new IFlockadeGamePiece.State?());
    }
    if (!(bool) (UnityEngine.Object) gamePiece)
      return;
    this._gamePiece = gamePiece;
    this._gamePiece.Changed += new Action<IFlockadeGamePiece.State?>(this.UpdateVisibility);
    this.UpdateVisibility(new IFlockadeGamePiece.State?(this._gamePiece.Get()));
  }

  public void UpdateVisibility(IFlockadeGamePiece.State? gamePiece)
  {
    if (gamePiece.HasValue)
    {
      FlockadeVirtualBlessingActivator blessing = gamePiece.GetValueOrDefault().Blessing;
      if (blessing is FlockadeBlessingActivator blessingActivator && blessing.Active)
      {
        this.Show(blessingActivator.Get());
        return;
      }
    }
    this.Hide();
  }

  public DG.Tweening.Sequence Hide(bool killOtherAnimations = true)
  {
    if (!this._flush)
      return (DG.Tweening.Sequence) null;
    if (this._animated)
    {
      if (killOtherAnimations)
      {
        DG.Tweening.Sequence sequence = this._sequence;
        if (sequence != null)
          sequence.Kill();
      }
      this._sequence = DOTween.Sequence().Append((Tween) this._frontBuffer.Container.DOScale(Vector3.zero, 0.233333334f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack)).Join((Tween) this._frontBuffer.Container.DORotate(new Vector3(0.0f, 0.0f, -10f), 0.233333334f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.InCubic)).Join((Tween) this._frontBuffer.CanvasGroup.DOFade(0.0f, 0.233333334f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InCubic));
      this._backBuffer.Clear();
      return this._sequence;
    }
    this._frontBuffer.Clear();
    this._backBuffer.Clear();
    return (DG.Tweening.Sequence) null;
  }

  public DG.Tweening.Sequence Show(
    FlockadeBlessingActivator.State blessing,
    bool killOtherAnimations = true)
  {
    if (this._animated)
    {
      if (killOtherAnimations)
      {
        DG.Tweening.Sequence sequence = this._sequence;
        if (sequence != null)
          sequence.Kill();
      }
      this._sequence = DOTween.Sequence().AppendInterval(0.0166666675f).AppendCallback((TweenCallback) (() => this._frontBuffer.Setup(blessing, this._consumed, this._noBackground))).Append((Tween) this._frontBuffer.Container.DOScale(Vector3.one, 0.233333334f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack)).Join((Tween) this._frontBuffer.Container.DORotate(Vector3.zero, 0.233333334f).From<Quaternion, Vector3, QuaternionOptions>(new Vector3(0.0f, 0.0f, -10f)).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutCubic)).Join((Tween) this._frontBuffer.CanvasGroup.DOFade(1f, 0.233333334f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic));
      this._backBuffer.Clear();
      return this._sequence;
    }
    this._frontBuffer.Setup(blessing, this._consumed, this._noBackground);
    this._frontBuffer.Show();
    this._backBuffer.Clear();
    return (DG.Tweening.Sequence) null;
  }

  public DG.Tweening.Sequence Nullify(bool killOtherAnimations = true)
  {
    if (this._animated)
    {
      if (killOtherAnimations)
      {
        DG.Tweening.Sequence sequence = this._sequence;
        if (sequence != null)
          sequence.Kill();
      }
      this._sequence = DOTween.Sequence().AppendInterval(0.0166666675f).AppendCallback((TweenCallback) (() =>
      {
        if (this._gamePiece.Blessing.Consumed)
          return;
        this._frontBuffer.SetColor(this._nullified);
      })).Append((Tween) this._frontBuffer.Container.DOScale(Vector3.zero, 0.6166667f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine)).Join((Tween) this._frontBuffer.Container.DOShakeAnchorPos(0.6166667f, 4f, 20, 0.0f, fadeOut: false)).Join((Tween) this._frontBuffer.CanvasGroup.DOFade(0.0f, 0.6166667f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine));
      this._backBuffer.Clear();
      return this._sequence;
    }
    if (!this._gamePiece.Blessing.Consumed)
      this._frontBuffer.SetColor(this._nullified);
    this._frontBuffer.Clear();
    this._backBuffer.Clear();
    return (DG.Tweening.Sequence) null;
  }

  public DG.Tweening.Sequence Activate(
    FlockadeBlessingActivator.State blessing,
    bool killOtherAnimations = true)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    FlockadeBlessing.\u003C\u003Ec__DisplayClass31_0 cDisplayClass310 = new FlockadeBlessing.\u003C\u003Ec__DisplayClass31_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass310.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass310.blessing = blessing;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass310.appliedInFrontBuffer = false;
    if (this._animated)
    {
      if (killOtherAnimations)
      {
        DG.Tweening.Sequence sequence = this._sequence;
        if (sequence != null)
          sequence.Kill();
      }
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this._sequence = DOTween.Sequence().AppendInterval(0.0166666675f).AppendCallback(new TweenCallback(cDisplayClass310.\u003CActivate\u003Eb__0)).Append((Tween) this._frontBuffer.Container.DOScale(1.25f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad)).Join((Tween) this._frontBuffer.CanvasGroup.DOFade(0.0f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine)).Join((Tween) this._backBuffer.CanvasGroup.DOFade(1f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad)).OnComplete<DG.Tweening.Sequence>(new TweenCallback(cDisplayClass310.\u003CActivate\u003Eg__ApplyInFrontBuffer\u007C1)).OnKill<DG.Tweening.Sequence>(new TweenCallback(cDisplayClass310.\u003CActivate\u003Eg__ApplyInFrontBuffer\u007C1));
      return this._sequence;
    }
    // ISSUE: reference to a compiler-generated method
    cDisplayClass310.\u003CActivate\u003Eg__ApplyInFrontBuffer\u007C1();
    return (DG.Tweening.Sequence) null;
  }

  [CompilerGenerated]
  public void \u003CNullify\u003Eb__30_0()
  {
    if (this._gamePiece.Blessing.Consumed)
      return;
    this._frontBuffer.SetColor(this._nullified);
  }

  [Serializable]
  public class BlessingBuffer
  {
    public RectTransform Container;
    public CanvasGroup CanvasGroup;
    public Image Background;
    public Image Outline;
    public Image Icon;

    public void Clear()
    {
      this.Container.localScale = Vector3.zero;
      this.CanvasGroup.alpha = 0.0f;
    }

    public void Show()
    {
      this.Container.localScale = Vector3.one;
      this.CanvasGroup.alpha = 1f;
    }

    public void Setup(FlockadeBlessingActivator.State blessing, Color consumed, bool noBackground = false)
    {
      bool flag = !noBackground && !blessing.Activator.Consumed;
      if (flag)
        this.Background.sprite = blessing.Configuration.Background;
      this.Outline.sprite = blessing.Configuration.Outline;
      this.Icon.sprite = blessing.Configuration.Icon;
      this.Background.gameObject.SetActive(flag);
      this.SetColor(blessing.Activator.Consumed ? consumed : blessing.Configuration.Color);
    }

    public void SetColor(Color color)
    {
      this.Outline.color = color;
      this.Icon.color = color;
    }
  }
}
