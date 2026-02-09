// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeRoundCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using UnityEngine;

#nullable disable
namespace Flockade;

[RequireComponent(typeof (CanvasGroup), typeof (RectTransform))]
public class FlockadeRoundCounter : MonoBehaviour
{
  public const string _ROUND_NUMBER_PARAMETER_NAME = "NUMBER";
  [SerializeField]
  public LocalizationParamsManager _text;
  public CanvasGroup _canvasGroup;
  public Vector2 _originAnchoredPosition;
  public RectTransform _rectTransform;
  public int _count;

  public virtual void Awake()
  {
    this._canvasGroup = this.GetComponent<CanvasGroup>();
    this._rectTransform = this.GetComponent<RectTransform>();
    this._originAnchoredPosition = this._rectTransform.anchoredPosition;
    this._canvasGroup.alpha = 0.0f;
    this._rectTransform.anchoredPosition = new Vector2(this._originAnchoredPosition.x, 0.0f);
  }

  public DG.Tweening.Sequence Increment()
  {
    ++this._count;
    this._text.SetParameterValue("NUMBER", this._count.ToString());
    return DOTween.Sequence().Append((Tween) this._canvasGroup.DOFade(1f, 0.8333333f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic)).Join((Tween) this._rectTransform.DOAnchorPosY(this._originAnchoredPosition.y, 0.8333333f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutCubic));
  }
}
