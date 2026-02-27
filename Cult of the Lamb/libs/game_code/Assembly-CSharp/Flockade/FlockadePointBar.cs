// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePointBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasGroup))]
public class FlockadePointBar : FlockadePointMarker
{
  public const float _APPEARANCE_DISAPPEARANCE_POP_SCALE = 1.6f;
  [SerializeField]
  public Image _image;
  [SerializeField]
  public Color _bonusColor;
  public CanvasGroup _canvasGroup;
  public Color _originColor;
  public bool _bonus;

  public override bool Bonus
  {
    get => this._bonus;
    set
    {
      this._bonus = value;
      this._image.color = value ? this._bonusColor : this._originColor;
    }
  }

  public override void Awake()
  {
    this._canvasGroup = this.GetComponent<CanvasGroup>();
    this._canvasGroup.alpha = 0.0f;
    this._originColor = this._image.color;
    base.Awake();
  }

  public override DG.Tweening.Sequence Show()
  {
    return DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(1f, 0.116666667f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad)).Join((Tween) this._image.rectTransform.DOScale(1f, 0.116666667f).From(1.6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
  }

  public override DG.Tweening.Sequence Hide()
  {
    return DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(0.0f, 0.116666667f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad)).Join((Tween) this._image.rectTransform.DOScale(1.6f, 0.116666667f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
  }
}
