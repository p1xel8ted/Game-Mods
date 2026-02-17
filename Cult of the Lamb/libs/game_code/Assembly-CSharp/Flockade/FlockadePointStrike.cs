// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadePointStrike
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasGroup), typeof (RectTransform))]
public class FlockadePointStrike : FlockadePointMarker
{
  public const float _APPEARANCE_DISAPPEARANCE_POP_SCALE = 2f;
  [SerializeField]
  public RectTransform _firstOfGroup;
  [SerializeField]
  public HorizontalLayoutGroup _layout;
  [SerializeField]
  public Image _image;
  [SerializeField]
  public Color _bonusColor;
  public CanvasGroup _canvasGroup;
  public Color _originColor;
  public bool _bonus;
  public RectTransform _rectTransform;

  public override bool Bonus
  {
    get => this._bonus;
    set
    {
      this._bonus = value;
      this._image.color = value ? this._bonusColor : this._originColor;
    }
  }

  public RectTransform RectTransform
  {
    get
    {
      if (!(bool) (Object) this._rectTransform)
        this._rectTransform = this.GetComponent<RectTransform>();
      return this._rectTransform;
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
    return DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(1f, 0.116666667f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad)).Join((Tween) this._image.rectTransform.DOScale(1f, 0.116666667f).From(2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
  }

  public override DG.Tweening.Sequence Hide()
  {
    return DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(0.0f, 0.116666667f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutQuad)).Join((Tween) this._image.rectTransform.DOScale(2f, 0.116666667f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad));
  }

  public void Place()
  {
    if (!this.isActiveAndEnabled)
      return;
    this.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 6f * this._layout.spacing);
    this.RectTransform.position = new Vector3(this._firstOfGroup.position.x - 1.5f * this._layout.spacing, this.RectTransform.position.y, this.RectTransform.position.z);
  }
}
