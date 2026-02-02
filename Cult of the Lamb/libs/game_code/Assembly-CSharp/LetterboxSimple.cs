// Decompiled with JetBrains decompiler
// Type: LetterboxSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class LetterboxSimple : MonoBehaviour
{
  [SerializeField]
  public RectTransform _letterboxTop;
  [SerializeField]
  public RectTransform _letterboxBottom;
  [SerializeField]
  public CanvasGroup _letterboxCanvasGroup;
  [SerializeField]
  public bool startDelay;
  [SerializeField]
  public float letterboxDelay = 0.5f;
  [SerializeField]
  public Vector2 _letterboxTopStartPos;
  [SerializeField]
  public Vector2 _letterboxBottomStartPos;
  [SerializeField]
  public float _letterboxStartingScale;

  public void Start()
  {
    this._letterboxTopStartPos = this._letterboxTop.anchoredPosition;
    this._letterboxBottomStartPos = this._letterboxBottom.anchoredPosition;
    this._letterboxStartingScale = this._letterboxTop.localScale.y;
    this._letterboxCanvasGroup.alpha = 0.0f;
    if (this.startDelay)
      this.StartCoroutine((IEnumerator) this.ShowDelayed());
    else
      this.Show();
  }

  public IEnumerator ShowDelayed()
  {
    yield return (object) new WaitForSecondsRealtime(this.letterboxDelay);
    this.Show();
  }

  public void TweenHeightScale(float height)
  {
    this._letterboxTop.DOKill();
    this._letterboxBottom.DOKill();
    this._letterboxTop.DOAnchorPosY(this._letterboxTopStartPos.y + height, 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart);
    this._letterboxBottom.DOAnchorPosY(this._letterboxBottomStartPos.y - height, 1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart);
  }

  public void Show()
  {
    this._letterboxCanvasGroup.DOKill();
    this._letterboxCanvasGroup.DOFade(1f, 0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this._letterboxTop.DOKill();
    this._letterboxBottom.DOKill();
    this._letterboxTop.anchoredPosition = this._letterboxTopStartPos + new Vector2(0.0f, this._letterboxTop.rect.height);
    this._letterboxBottom.anchoredPosition = this._letterboxBottomStartPos - new Vector2(0.0f, this._letterboxBottom.rect.height);
    this._letterboxTop.DOAnchorPosY(this._letterboxTopStartPos.y, 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart).SetDelay<TweenerCore<Vector2, Vector2, VectorOptions>>(0.1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    this._letterboxBottom.DOAnchorPosY(this._letterboxBottomStartPos.y, 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart).SetDelay<TweenerCore<Vector2, Vector2, VectorOptions>>(0.1f).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
  }

  public void Hide()
  {
    this._letterboxTop.DOKill();
    this._letterboxBottom.DOKill();
    this._letterboxTop.DOAnchorPosY(this._letterboxTopStartPos.y + this._letterboxTop.rect.height, 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    this._letterboxBottom.DOAnchorPosY(this._letterboxBottomStartPos.y - this._letterboxBottom.rect.height, 1f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true).OnComplete<TweenerCore<Vector2, Vector2, VectorOptions>>((TweenCallback) (() =>
    {
      this._letterboxCanvasGroup.DOKill();
      this._letterboxCanvasGroup.DOFade(0.0f, 0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }));
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__12_0()
  {
    this._letterboxCanvasGroup.DOKill();
    this._letterboxCanvasGroup.DOFade(0.0f, 0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }
}
