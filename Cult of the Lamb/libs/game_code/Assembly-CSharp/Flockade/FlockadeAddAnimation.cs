// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeAddAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
namespace Flockade;

[CreateAssetMenu(fileName = "FlockadeAddAnimation", menuName = "Flockade/Add Animation")]
public class FlockadeAddAnimation : ScriptableObject
{
  [Header("Position")]
  [SerializeField]
  public bool moveAnchorPos;
  [SerializeField]
  public Vector2 endAnchorValue;
  [SerializeField]
  public bool shakeAnchorPos;
  [SerializeField]
  public float shakeAnchorPosStrength;
  [SerializeField]
  public int shakeAnchorPosVibrato = 10;
  [Header("Scale")]
  [SerializeField]
  public bool changeScale;
  [SerializeField]
  public float scaleMultiplier;
  [Header("Rotation")]
  [SerializeField]
  public bool rotateAnchor;
  [SerializeField]
  public Vector3 rotation;

  public Tween GetAddAnimation(RectTransform target, Vector3 scale, float duration, Ease ease)
  {
    DG.Tweening.Sequence s = DOTween.Sequence();
    if (this.moveAnchorPos)
    {
      Vector2 endAnchorValue = this.endAnchorValue;
      endAnchorValue.x *= scale.x;
      endAnchorValue.y *= scale.y;
      s.Join((Tween) target.DOAnchorPos(target.anchoredPosition + endAnchorValue, duration).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(ease));
    }
    if (this.shakeAnchorPos)
      s.Join((Tween) target.DOShakeAnchorPos(duration, this.shakeAnchorPosStrength, this.shakeAnchorPosVibrato).SetEase<Tweener>(ease));
    if (this.rotateAnchor)
      s.Join((Tween) target.DOLocalRotate(this.rotation * -scale.x, duration).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(ease));
    if (this.changeScale)
      s.Join((Tween) target.DOScale(this.scaleMultiplier * scale, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ease));
    return (Tween) s;
  }

  public Tween ResetAnimationChanges(RectTransform target)
  {
    DG.Tweening.Sequence s = DOTween.Sequence();
    if (this.moveAnchorPos)
      s.Join((Tween) target.DOAnchorPos(Vector2.zero, 0.0f));
    if (this.shakeAnchorPos)
      s.Join((Tween) target.DOScale(1f, 0.0f));
    if (this.rotateAnchor)
      s.Join((Tween) target.DOLocalRotate(Vector3.zero, 0.0f));
    if (this.changeScale)
      s.Join((Tween) target.DOScale(Vector3.one, 0.0f));
    return (Tween) s;
  }
}
