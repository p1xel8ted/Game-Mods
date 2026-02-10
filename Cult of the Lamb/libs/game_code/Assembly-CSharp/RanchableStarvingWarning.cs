// Decompiled with JetBrains decompiler
// Type: RanchableStarvingWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RanchableStarvingWarning : MonoBehaviour, IUpdateManually
{
  public Interaction_Ranchable Ranchable;
  public GameObject Container;
  public SpriteRenderer ProgressBar;
  public Gradient Gradient;
  public bool IsPlaying;
  public Vector3 originalScale;
  public float delayTimer;
  public const float DELAY_BETWEEN_UPDATES = 0.6f;

  public void Awake()
  {
    this.originalScale = this.Container.transform.localScale;
    this.Container.SetActive(false);
  }

  public void UpdateManually() => this.Update();

  public void Update()
  {
    if (!((Object) this.Ranchable != (Object) null) || this.Ranchable.BeingAscended)
      return;
    if ((double) this.Ranchable.Animal.Satiation <= 25.0 && this.Ranchable.Animal.State != Interaction_Ranchable.State.Dead)
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
        double satiation = (double) this.Ranchable.Animal.Satiation;
        float num = Mathf.Max(Mathf.Lerp((float) satiation, Mathf.Max((float) (satiation - 2.0), 0.0f), TimeManager.CurrentPhaseProgress) / 25f, 0.1f);
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
  public void \u003CHide\u003Eb__12_0() => this.Container.SetActive(false);

  [CompilerGenerated]
  public void \u003CHide\u003Eb__12_1() => this.Container.SetActive(false);
}
