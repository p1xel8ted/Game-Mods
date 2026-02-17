// Decompiled with JetBrains decompiler
// Type: SimpleRadialProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class SimpleRadialProgressBar : MonoBehaviour
{
  [SerializeField]
  public SpriteRenderer progressBar;
  [SerializeField]
  public float showDuration = 0.3f;
  [SerializeField]
  public float hideDuration = 0.3f;
  [SerializeField]
  public bool canPulse = true;
  [SerializeField]
  public float pulseSpeed = 6f;
  [SerializeField]
  public float pulseScaleMultiplier = 1.1f;
  [SerializeField]
  public bool isUsingAtlas;
  [SerializeField]
  public SimpleRadialProgressBar.ProgressShaderType progressShaderType;
  public Vector3 minPulseScale = Vector3.one;
  public Vector3 maxPulseScale;
  public bool isVisible;
  public float pulseScaleTimer;
  public Vector3 initialScale = Vector3.one;

  public bool IsVisible => this.isVisible;

  public void Awake()
  {
    this.initialScale = this.transform.localScale;
    this.transform.localScale = Vector3.zero;
    this.gameObject.SetActive(false);
    this.maxPulseScale = this.minPulseScale * this.pulseScaleMultiplier;
  }

  public void Show(bool instant = false, System.Action callback = null)
  {
    if (this.isVisible)
      return;
    this.gameObject.SetActive(true);
    this.isVisible = true;
    this.transform.DOKill();
    if (this.isUsingAtlas)
      this.SetSpriteAtlasArcCenterOffset();
    if (instant)
    {
      this.transform.localScale = this.initialScale;
      System.Action action = callback;
      if (action == null)
        return;
      action();
    }
    else
    {
      this.transform.localScale = Vector3.zero;
      this.transform.DOScale(this.initialScale, this.showDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        System.Action action = callback;
        if (action == null)
          return;
        action();
      }));
    }
  }

  public void Hide(bool instant = false, System.Action callback = null)
  {
    if (!this.isVisible)
      return;
    this.isVisible = false;
    this.transform.DOKill();
    if (instant)
    {
      this.transform.localScale = Vector3.zero;
      this.pulseScaleTimer = 0.0f;
      System.Action action = callback;
      if (action != null)
        action();
      this.gameObject.SetActive(false);
    }
    else
    {
      this.transform.localScale = this.initialScale;
      this.transform.DOScale(Vector3.zero, this.hideDuration).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        System.Action action = callback;
        if (action != null)
          action();
        this.gameObject.SetActive(false);
      }));
      this.pulseScaleTimer = 0.0f;
    }
  }

  public void UpdateProgress(float progress)
  {
    progress = Mathf.Clamp01(progress);
    if (this.progressShaderType == SimpleRadialProgressBar.ProgressShaderType.Arc)
    {
      this.progressBar.material.SetFloat("_Arc2", (float) (360.0 - (double) progress * 360.0));
      this.progressBar.material.SetFloat("_Arc1", 0.0f);
    }
    else
    {
      if (this.progressShaderType != SimpleRadialProgressBar.ProgressShaderType.Progress)
        return;
      this.progressBar.material.SetFloat("_Progress", progress);
    }
  }

  public void UpdatePulse()
  {
    if (!this.isVisible || !this.canPulse)
      return;
    this.pulseScaleTimer += Time.deltaTime * this.pulseSpeed;
    this.transform.localScale = Vector3.Lerp(this.minPulseScale, this.maxPulseScale, Mathf.PingPong(this.pulseScaleTimer, 1f));
  }

  public void SetSpriteAtlasArcCenterOffset()
  {
    Vector2 center = this.progressBar.sprite.textureRect.center;
    this.progressBar.material.SetVector("_ArcCenterOffset", (Vector4) (new Vector2(center.x / (float) this.progressBar.sprite.texture.width, center.y / (float) this.progressBar.sprite.texture.height) - Vector2.one * 0.5f));
  }

  public enum ProgressShaderType
  {
    Progress,
    Arc,
  }
}
