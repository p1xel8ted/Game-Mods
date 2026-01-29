// Decompiled with JetBrains decompiler
// Type: WeathervaneController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class WeathervaneController : MonoBehaviour
{
  public Transform structureContainer;
  [Header("Graphics References")]
  public GameObject winterGraphic;
  public GameObject springGraphic;
  [Header("Positioning")]
  public float activePositionY = 1f;
  public float inactivePositionY = -1f;
  [Header("Fill Bars")]
  public Transform winterFillBarTransform;
  public SpriteRenderer winterFillBarSpriteRenderer;
  public Transform springFillBarTransform;
  public SpriteRenderer springFillBarSpriteRenderer;
  public float fillFullScale = 1f;
  public AnimationCurve curve;
  [Header("Animation")]
  public float tweenDuration = 0.5f;
  public Ease tweenEase = Ease.OutBack;
  public float preChangeShakeDuration = 0.3f;
  public float preChangeShakeStrength = 0.1f;
  public int preChangeShakeVibrato = 15;
  public float preChangeScaleDownY = 0.95f;
  public float preChangeScaleDuration = 0.15f;
  public SeasonsManager.Season currentSeason;
  public SeasonsManager.Season previousSeason;
  public Vector3 initialWinterBarScale;
  public Vector3 initialSpringBarScale;
  public bool isInitialized;
  public DG.Tweening.Sequence preChangeSequence;

  public void Start() => this.InitializeWeathervane();

  public void InitializeWeathervane()
  {
    if ((Object) this.winterGraphic == (Object) null || (Object) this.springGraphic == (Object) null || (Object) this.winterFillBarTransform == (Object) null || (Object) this.winterFillBarSpriteRenderer == (Object) null || (Object) this.springFillBarTransform == (Object) null || (Object) this.springFillBarSpriteRenderer == (Object) null)
    {
      Debug.Log((object) "Weathervane setup incomplete in the inspector!");
      this.enabled = false;
    }
    else
    {
      this.initialWinterBarScale = this.winterFillBarTransform.localScale;
      this.initialSpringBarScale = this.springFillBarTransform.localScale;
      this.currentSeason = SeasonsManager.CurrentSeason;
      this.SetSeasonState(this.currentSeason, false);
      this.isInitialized = true;
    }
  }

  public void Update()
  {
    if (!this.isInitialized)
      return;
    float targetXScale = SeasonsManager.SEASON_NORMALISED_PROGRESS * this.fillFullScale;
    if (this.currentSeason == SeasonsManager.Season.Spring)
    {
      if (this.winterFillBarTransform.gameObject.activeSelf)
        this.SetFillBarScale(this.winterFillBarTransform, this.winterFillBarSpriteRenderer, targetXScale);
      if (this.springFillBarTransform.gameObject.activeSelf)
        this.springFillBarTransform.gameObject.SetActive(false);
    }
    else
    {
      if (this.springFillBarTransform.gameObject.activeSelf)
        this.SetFillBarScale(this.springFillBarTransform, this.springFillBarSpriteRenderer, targetXScale);
      if (this.winterFillBarTransform.gameObject.activeSelf)
        this.winterFillBarTransform.gameObject.SetActive(false);
    }
    if (SeasonsManager.CurrentSeason == this.previousSeason)
      return;
    this.ChangeSeason();
  }

  public void ChangeSeason()
  {
    if (this.preChangeSequence != null && this.preChangeSequence.IsActive())
      return;
    SeasonsManager.Season nextSeason = this.previousSeason == SeasonsManager.Season.Winter ? SeasonsManager.Season.Spring : SeasonsManager.Season.Winter;
    this.previousSeason = SeasonsManager.CurrentSeason;
    this.preChangeSequence = DOTween.Sequence();
    this.preChangeSequence.Append((Tween) this.structureContainer.DOShakePosition(this.preChangeShakeDuration, this.preChangeShakeStrength, this.preChangeShakeVibrato)).Join((Tween) this.structureContainer.DOScaleY(this.preChangeScaleDownY, this.preChangeScaleDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad)).Append((Tween) this.structureContainer.DOScaleY(1f, this.preChangeScaleDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    this.preChangeSequence.OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
    {
      this.SetSeasonState(nextSeason, true);
      this.currentSeason = nextSeason;
      this.preChangeSequence = (DG.Tweening.Sequence) null;
    }));
  }

  public void SetSeasonState(SeasonsManager.Season season, bool animate)
  {
    if (season == SeasonsManager.Season.Spring)
    {
      if (animate)
      {
        AudioManager.Instance.PlayOneShot("event:/dlc/building/weathervane/winter_end", this.transform.position);
        this.AnimateGraphicUp(this.springGraphic);
        this.AnimateGraphicDown(this.winterGraphic);
      }
      else
      {
        this.SetGraphicPositionImmediate(this.springGraphic, this.activePositionY, true);
        this.SetGraphicPositionImmediate(this.winterGraphic, this.inactivePositionY, false);
      }
      if ((bool) (Object) this.winterFillBarTransform)
      {
        this.winterFillBarTransform.gameObject.SetActive(true);
        this.SetFillBarScale(this.winterFillBarTransform, this.winterFillBarSpriteRenderer, 0.0f);
      }
      if (!(bool) (Object) this.springFillBarTransform)
        return;
      this.springFillBarTransform.gameObject.SetActive(false);
    }
    else
    {
      if (animate)
      {
        AudioManager.Instance.PlayOneShot("event:/dlc/building/weathervane/winter_start", this.transform.position);
        this.AnimateGraphicUp(this.winterGraphic);
        this.AnimateGraphicDown(this.springGraphic);
      }
      else
      {
        this.SetGraphicPositionImmediate(this.winterGraphic, this.activePositionY, true);
        this.SetGraphicPositionImmediate(this.springGraphic, this.inactivePositionY, false);
      }
      if ((bool) (Object) this.springFillBarTransform)
      {
        this.springFillBarTransform.gameObject.SetActive(true);
        this.SetFillBarScale(this.springFillBarTransform, this.springFillBarSpriteRenderer, 0.0f);
      }
      if (!(bool) (Object) this.winterFillBarTransform)
        return;
      this.winterFillBarTransform.gameObject.SetActive(false);
    }
  }

  public void AnimateGraphicUp(GameObject graphic)
  {
    if (!(bool) (Object) graphic)
      return;
    graphic.SetActive(true);
    graphic.transform.DOKill();
    Vector3 localPosition = graphic.transform.localPosition;
    Vector3 endValue = new Vector3(localPosition.x, this.activePositionY, localPosition.z);
    graphic.transform.DOLocalMove(endValue, this.tweenDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.tweenEase);
  }

  public void AnimateGraphicDown(GameObject graphic)
  {
    if (!(bool) (Object) graphic)
      return;
    graphic.SetActive(true);
    graphic.transform.DOKill();
    Vector3 localPosition = graphic.transform.localPosition;
    Vector3 endValue = new Vector3(localPosition.x, this.inactivePositionY, localPosition.z);
    graphic.transform.DOLocalMove(endValue, this.tweenDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.tweenEase).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if (!(bool) (Object) graphic || (!((Object) graphic == (Object) this.winterGraphic) || this.currentSeason != SeasonsManager.Season.Winter ? (!((Object) graphic == (Object) this.springGraphic) ? 0 : (this.currentSeason == SeasonsManager.Season.Spring ? 1 : 0)) : 1) != 0)
        return;
      graphic.SetActive(false);
    }));
  }

  public void SetGraphicPositionImmediate(GameObject graphic, float yPos, bool isActive)
  {
    if (!(bool) (Object) graphic)
      return;
    Vector3 localPosition = graphic.transform.localPosition;
    graphic.transform.localPosition = new Vector3(localPosition.x, yPos, localPosition.z);
    graphic.SetActive(isActive);
  }

  public void SetFillBarScale(Transform barTransform, SpriteRenderer visual, float targetXScale)
  {
    if (!(bool) (Object) barTransform || !barTransform.gameObject.activeSelf)
      return;
    targetXScale = Mathf.Clamp01(targetXScale);
    float normalisedProgress = SeasonsManager.SEASON_NORMALISED_PROGRESS;
    visual.transform.localScale = new Vector3(visual.transform.localScale.x, Mathf.Lerp(0.5f, 0.73f, this.curve.Evaluate(normalisedProgress)), visual.transform.localScale.z);
    if ((Object) barTransform == (Object) this.winterFillBarTransform)
    {
      barTransform.localScale = new Vector3(targetXScale, this.initialWinterBarScale.y, this.initialWinterBarScale.z);
    }
    else
    {
      if (!((Object) barTransform == (Object) this.springFillBarTransform))
        return;
      barTransform.localScale = new Vector3(targetXScale, this.initialSpringBarScale.y, this.initialSpringBarScale.z);
    }
  }

  public void ForceSeason(SeasonsManager.Season season, bool animate = true)
  {
    if (season == this.currentSeason && this.isInitialized)
      return;
    if (this.preChangeSequence != null)
      this.preChangeSequence.Kill();
    if ((bool) (Object) this.winterGraphic)
      this.winterGraphic.transform.DOKill();
    if ((bool) (Object) this.springGraphic)
      this.springGraphic.transform.DOKill();
    this.structureContainer.DOKill();
    if (!this.isInitialized)
    {
      this.currentSeason = season;
    }
    else
    {
      this.SetSeasonState(season, animate);
      this.transform.localScale = Vector3.one;
      this.currentSeason = season;
    }
  }

  public void OnDestroy()
  {
    if (this.preChangeSequence != null)
      this.preChangeSequence.Kill();
    if ((bool) (Object) this.winterGraphic)
      this.winterGraphic.transform.DOKill();
    if ((bool) (Object) this.springGraphic)
      this.springGraphic.transform.DOKill();
    this.structureContainer.DOKill();
  }
}
