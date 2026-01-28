// Decompiled with JetBrains decompiler
// Type: RanchableFeralWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RanchableFeralWarning : MonoBehaviour, IUpdateManually
{
  public Interaction_Ranchable Ranchable;
  public GameObject Container;
  public SpriteRenderer ProgressBar;
  public Gradient Gradient;
  public bool IsPlaying;
  public Vector3 originalScale;
  public float delayTimer;
  public const float DELAY_BETWEEN_UPDATES = 0.6f;
  public Tween fillTween;
  public bool InSequence;

  public void Awake()
  {
    this.originalScale = this.Container.transform.localScale;
    this.Container.SetActive(false);
  }

  public void Update()
  {
    if (this.InSequence || !((Object) this.Ranchable != (Object) null) || this.Ranchable.BeingAscended)
      return;
    if (this.Ranchable.Animal.Ailment == Interaction_Ranchable.Ailment.Feral && this.Ranchable.Animal.State != Interaction_Ranchable.State.Dead)
    {
      if (!this.IsPlaying)
      {
        this.Show();
      }
      else
      {
        if (!((Object) this.ProgressBar != (Object) null))
          return;
        this.delayTimer -= Time.deltaTime;
        if ((double) this.delayTimer > 0.0)
          return;
        this.delayTimer = 0.6f;
        double feralCalming = (double) this.Ranchable.Animal.FeralCalming;
        float num = Mathf.Max(Mathf.Lerp((float) feralCalming, Mathf.Max((float) (feralCalming - 1.0), 0.0f), TimeManager.CurrentPhaseProgress) / 10f, 0.1f);
        Color color = this.Gradient.Evaluate(num);
        this.ProgressBar.color = new Color(color.r, color.g, color.b, num);
      }
    }
    else
    {
      if (!this.IsPlaying)
        return;
      this.Hide();
    }
  }

  public float PlaySequence(float Target, float Speed)
  {
    float Current = Mathf.Max(this.ProgressBar.color.a, 0.1f);
    float Duration = Mathf.Abs(Current - Target) / Speed;
    this.StartCoroutine((IEnumerator) this.PlaySequenceIE(Current, Target, Duration));
    return Duration;
  }

  public IEnumerator PlaySequenceIE(float Current, float Target, float Duration)
  {
    RanchableFeralWarning ranchableFeralWarning = this;
    ranchableFeralWarning.InSequence = true;
    Tween fillTween = ranchableFeralWarning.fillTween;
    if (fillTween != null)
      fillTween.Kill();
    Color color = ranchableFeralWarning.Gradient.Evaluate(Current);
    ranchableFeralWarning.ProgressBar.color = new Color(color.r, color.g, color.b, Current);
    yield return (object) null;
    ranchableFeralWarning.fillTween = (Tween) DOTween.To(new DOGetter<float>(ranchableFeralWarning.\u003CPlaySequenceIE\u003Eb__13_0), new DOSetter<float>(ranchableFeralWarning.\u003CPlaySequenceIE\u003Eb__13_1), Target, Duration).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutCubic);
    yield return (object) new WaitForSeconds(Duration + 0.5f);
    ranchableFeralWarning.InSequence = false;
  }

  public void UpdateManually()
  {
  }

  public void Show()
  {
    if ((Object) this == (Object) null || this.Container.activeInHierarchy)
      return;
    this.Container.SetActive(true);
    this.Container.transform.localScale = Vector3.zero;
    this.Container.transform.DOKill();
    this.Container.transform.DOScale(this.originalScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.IsPlaying = true;
  }

  public void Hide()
  {
    if ((Object) this == (Object) null || !this.Container.activeInHierarchy)
      return;
    this.Container.transform.localScale = this.originalScale;
    this.Container.transform.DOKill();
    this.Container.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Container.SetActive(false))).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Container.SetActive(false)));
    this.IsPlaying = false;
  }

  [CompilerGenerated]
  public float \u003CPlaySequenceIE\u003Eb__13_0() => this.ProgressBar.color.a;

  [CompilerGenerated]
  public void \u003CPlaySequenceIE\u003Eb__13_1(float x)
  {
    Color color = this.Gradient.Evaluate(x);
    this.ProgressBar.color = new Color(color.r, color.g, color.b, x);
  }

  [CompilerGenerated]
  public void \u003CHide\u003Eb__16_0() => this.Container.SetActive(false);

  [CompilerGenerated]
  public void \u003CHide\u003Eb__16_1() => this.Container.SetActive(false);
}
