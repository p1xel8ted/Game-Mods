// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeMarkedClawsAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeMarkedClawsAnimation : FlockadeAnimation
{
  public const string _SLASHING_SOUND = "event:/dlc/ui/flockade_minigame/point_loss";
  public const float _APPEARANCE_DURATION = 0.166666672f;
  public const Ease _APPEARANCE_EASING = Ease.OutCubic;
  public const float _DISAPPEARANCE_DURATION = 0.5f;
  public const Ease _DISAPPEARANCE_EASING = Ease.InCubic;
  public const float _INITIAL_SCALE = 1.6f;
  public const float _INITIAL_TILT_ANGLE = 45f;
  public const float _INITIAL_Y_OFFSET = 30f;

  public override DG.Tweening.Sequence Animate()
  {
    this.RectTransform.anchoredPosition += new Vector2(0.0f, 30f);
    this.RectTransform.localEulerAngles = new Vector3(0.0f, 0.0f, this.Flip.X ? -45f : 45f);
    Vector3 localScale = this.RectTransform.localScale;
    RectTransform rectTransform = this.RectTransform;
    rectTransform.localScale = rectTransform.localScale * 1.6f;
    return DOTween.Sequence().AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/dlc/ui/flockade_minigame/point_loss"))).Append((Tween) this.CanvasGroup.DOFade(1f, 0.166666672f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic)).Join((Tween) this.RectTransform.DOScale(localScale, 0.166666672f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic)).Join((Tween) this.RectTransform.DORotate(Vector3.zero, 0.166666672f).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.OutCubic)).Join((Tween) this.RectTransform.DOAnchorPosY(0.0f, 0.166666672f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCubic)).Append((Tween) this.CanvasGroup.DOFade(0.0f, 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InCubic));
  }
}
