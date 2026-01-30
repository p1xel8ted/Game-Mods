// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CanvasParallax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

[ExecuteAlways]
public class CanvasParallax : MonoBehaviour
{
  [Tooltip("All the ParallaxLayer components to parallax.")]
  public List<ParallaxLayer> layers = new List<ParallaxLayer>();
  [Tooltip("Normalized pivot within each layer that the offset originates from.")]
  public Vector2 layerPivot = new Vector2(0.5f, 0.5f);
  [Tooltip("Parallax set by the editor trackpad or runtime mouse mapping (read-only).")]
  public Vector2 parallax = Vector2.zero;
  [Tooltip("Base offset added on top of parallax.")]
  public Vector2 offset = Vector2.zero;
  [Tooltip("Invert the parallax vector before applying.")]
  public bool invertParallax;
  [Tooltip("When enabled in Play Mode, the mouse position relative to screen center continuously controls parallax (edges → ±strength).")]
  public bool parallaxToRelativeMousePosition;
  [Tooltip("Max absolute parallax per axis. Trackpad/mouse edges map to (-strength..+strength).")]
  public Vector2 strength = Vector2.one;
  [Tooltip("Uniform zoom factor applied to all layers.")]
  [Range(0.1f, 10f)]
  public float zoom = 1f;
  [Tooltip("Additional zoom applied per layer proportional to ParallaxLayer.Distance. Final scale = zoom * (1 + parallaxZoom * Distance).")]
  public float parallaxZoom;
  public Vector2 _shakeOffset = Vector2.zero;
  public Tween _shakeTween;
  public Vector2 _rumbleOffset = Vector2.zero;
  public Tween _rumbleTween;
  public Tween _rumbleTimer;
  public CancellationTokenSource _cts;

  public Vector2 TotalApplied
  {
    get
    {
      Vector2 vector2_1 = new Vector2(Mathf.Abs(this.strength.x), Mathf.Abs(this.strength.y));
      Vector2 vector2_2 = new Vector2(Mathf.Clamp(this.parallax.x, -vector2_1.x, vector2_1.x), Mathf.Clamp(this.parallax.y, -vector2_1.y, vector2_1.y));
      if (this.invertParallax)
        vector2_2 *= -1f;
      return this.offset + vector2_2;
    }
  }

  public void Awake() => this._cts = new CancellationTokenSource();

  public void OnDestroy()
  {
    this._cts.Cancel();
    this._cts.Dispose();
  }

  public void Update()
  {
    if (Application.isPlaying && this.parallaxToRelativeMousePosition)
    {
      Vector2 vector = new Vector2(InputManager.Gameplay.GetHorizontalSecondaryAxis(), InputManager.Gameplay.GetVerticalSecondaryAxis());
      if ((double) vector.sqrMagnitude > 0.0099999997764825821)
      {
        vector = Vector2.ClampMagnitude(vector, 1f);
        Vector2 vector2 = new Vector2(Mathf.Abs(this.strength.x), Mathf.Abs(this.strength.y));
        this.parallax = new Vector2(vector.x * vector2.x, vector.y * vector2.y);
      }
      else
      {
        float num1 = Screen.width > 0 ? (float) ((double) Input.mousePosition.x / (double) Screen.width * 2.0 - 1.0) : 0.0f;
        float num2 = Screen.height > 0 ? (float) ((double) Input.mousePosition.y / (double) Screen.height * 2.0 - 1.0) : 0.0f;
        float num3 = Mathf.Clamp(num1, -1f, 1f);
        float num4 = Mathf.Clamp(num2, -1f, 1f);
        Vector2 vector2 = new Vector2(Mathf.Abs(this.strength.x), Mathf.Abs(this.strength.y));
        this.parallax = new Vector2(num3 * vector2.x, num4 * vector2.y);
      }
    }
    Vector2 vector2_1 = new Vector2(Mathf.Abs(this.strength.x), Mathf.Abs(this.strength.y));
    Vector2 vector2_2 = new Vector2(Mathf.Clamp(this.parallax.x, -vector2_1.x, vector2_1.x), Mathf.Clamp(this.parallax.y, -vector2_1.y, vector2_1.y));
    if (this.invertParallax)
      vector2_2 *= -1f;
    Vector2 vector2_3 = this.offset + vector2_2 + this._shakeOffset + this._rumbleOffset;
    foreach (ParallaxLayer layer in this.layers)
    {
      if (!((Object) layer == (Object) null))
      {
        RectTransform transform = layer.transform as RectTransform;
        if (!((Object) transform == (Object) null))
        {
          transform.pivot = this.layerPivot;
          float num = Mathf.Max(0.0001f, this.zoom * (float) (1.0 + (double) this.parallaxZoom / 100.0 * (double) layer.Distance));
          transform.localScale = new Vector3(num, num, 1f);
          transform.anchoredPosition = vector2_3 * layer.Distance;
        }
      }
    }
  }

  public void PopulateLayers()
  {
    this.layers.Clear();
    this.layers.AddRange((IEnumerable<ParallaxLayer>) this.GetComponentsInChildren<ParallaxLayer>(true));
  }

  public async System.Threading.Tasks.Task ZoomAsync(float to, float duration, Ease ease = Ease.InOutSine)
  {
    CanvasParallax isIndependentUpdate = this;
    DOTween.Kill((object) (isIndependentUpdate.name + "_zoom"));
    await DOTween.To(new DOGetter<float>(isIndependentUpdate.\u003CZoomAsync\u003Eb__21_0), new DOSetter<float>(isIndependentUpdate.\u003CZoomAsync\u003Eb__21_1), to, duration).SetUpdate<TweenerCore<float, float, FloatOptions>>((bool) (Object) isIndependentUpdate).SetEase<TweenerCore<float, float, FloatOptions>>(ease).SetId<TweenerCore<float, float, FloatOptions>>(isIndependentUpdate.name + "_zoom").AsyncWaitForCompletion();
  }

  public async System.Threading.Tasks.Task PanAsync(Vector2 to, float duration, Ease ease = Ease.InOutSine)
  {
    CanvasParallax isIndependentUpdate = this;
    DOTween.Kill((object) (isIndependentUpdate.name + "_pan"));
    await DOTween.To(new DOGetter<Vector2>(isIndependentUpdate.\u003CPanAsync\u003Eb__22_0), new DOSetter<Vector2>(isIndependentUpdate.\u003CPanAsync\u003Eb__22_1), to, duration).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>((bool) (Object) isIndependentUpdate).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(ease).SetId<TweenerCore<Vector2, Vector2, VectorOptions>>(isIndependentUpdate.name + "_pan").AsyncWaitForCompletion();
  }

  public async System.Threading.Tasks.Task ZoomPanToRectAsync(
    RectTransform target,
    float zoomAmount,
    float duration,
    Ease ease = Ease.InOutSine)
  {
    CanvasParallax isIndependentUpdate = this;
    if (!(bool) (Object) target)
      return;
    ParallaxLayer componentInParent1 = target.GetComponentInParent<ParallaxLayer>();
    RectTransform transform = (bool) (Object) componentInParent1 ? componentInParent1.transform as RectTransform : (RectTransform) null;
    RectTransform parent = !(bool) (Object) transform || !(bool) (Object) transform.parent ? (RectTransform) null : transform.parent as RectTransform;
    if (!(bool) (Object) componentInParent1 || !(bool) (Object) transform || !(bool) (Object) parent)
      return;
    Canvas componentInParent2 = parent.GetComponentInParent<Canvas>();
    Camera worldCamera = (bool) (Object) componentInParent2 ? componentInParent2.worldCamera : (Camera) null;
    DOTween.Kill((object) (isIndependentUpdate.name + "_zoompan_seq"));
    DOTween.Kill((object) (isIndependentUpdate.name + "_zoom"));
    DOTween.Kill((object) (isIndependentUpdate.name + "_pan"));
    bool wasInteractive = isIndependentUpdate.parallaxToRelativeMousePosition;
    isIndependentUpdate.parallaxToRelativeMousePosition = false;
    float layerDistance = componentInParent1.Distance;
    bool canPan = (double) Mathf.Abs(layerDistance) >= 9.9999997473787516E-05;
    Vector2 vector2_1 = new Vector2(Mathf.Abs(isIndependentUpdate.strength.x), Mathf.Abs(isIndependentUpdate.strength.y));
    Vector2 clampedParallaxVec = new Vector2(Mathf.Clamp(isIndependentUpdate.parallax.x, -vector2_1.x, vector2_1.x), Mathf.Clamp(isIndependentUpdate.parallax.y, -vector2_1.y, vector2_1.y));
    if (isIndependentUpdate.invertParallax)
      clampedParallaxVec *= -1f;
    double num1 = (double) Mathf.Max(0.0001f, isIndependentUpdate.zoom);
    float endValue1 = Mathf.Max(0.0001f, zoomAmount);
    double num2 = 1.0 + (double) isIndependentUpdate.parallaxZoom / 100.0 * (double) layerDistance;
    float b = (float) (num1 * num2);
    float endLayerScale = endValue1 * (float) (1.0 + (double) isIndependentUpdate.parallaxZoom / 100.0 * (double) layerDistance);
    Vector3 worldPoint;
    RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, new Vector2((float) Screen.width * 0.5f, (float) Screen.height * 0.5f), worldCamera, out worldPoint);
    Vector2 desiredParentCenter = (Vector2) parent.InverseTransformPoint(worldPoint);
    Vector3 position = target.TransformPoint((Vector3) target.rect.center);
    Vector2 vector2_2 = (Vector2) parent.InverseTransformPoint(position);
    Vector2 vector2_3 = (isIndependentUpdate.offset + clampedParallaxVec) * layerDistance;
    Vector2 targetLocalVector = canPan ? (vector2_2 - vector2_3) / Mathf.Max(0.0001f, b) : Vector2.zero;
    Vector2 vector2_4 = canPan ? desiredParentCenter - endLayerScale * targetLocalVector : vector2_3;
    Vector2 endValue2 = canPan ? vector2_4 / layerDistance - clampedParallaxVec : isIndependentUpdate.offset;
    DG.Tweening.Sequence sequence = DOTween.Sequence().SetUpdate<DG.Tweening.Sequence>((bool) (Object) isIndependentUpdate).SetId<DG.Tweening.Sequence>(isIndependentUpdate.name + "_zoompan_seq").OnComplete<DG.Tweening.Sequence>((TweenCallback) (() =>
    {
      if (canPan)
        this.offset = (desiredParentCenter - endLayerScale * targetLocalVector) / layerDistance - clampedParallaxVec;
      this.parallaxToRelativeMousePosition = wasInteractive;
    })).OnKill<DG.Tweening.Sequence>((TweenCallback) (() => this.parallaxToRelativeMousePosition = wasInteractive));
    TweenerCore<float, float, FloatOptions> t1 = DOTween.To((DOGetter<float>) (() => this.zoom), (DOSetter<float>) (z => this.zoom = Mathf.Max(0.0001f, z)), endValue1, duration).SetEase<TweenerCore<float, float, FloatOptions>>(ease).SetUpdate<TweenerCore<float, float, FloatOptions>>((bool) (Object) isIndependentUpdate).SetId<TweenerCore<float, float, FloatOptions>>(isIndependentUpdate.name + "_zoom");
    sequence.Join((Tween) t1);
    if (canPan)
    {
      TweenerCore<Vector2, Vector2, VectorOptions> t2 = DOTween.To((DOGetter<Vector2>) (() => this.offset), (DOSetter<Vector2>) (v => this.offset = v), endValue2, duration).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(ease).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>((bool) (Object) isIndependentUpdate).SetId<TweenerCore<Vector2, Vector2, VectorOptions>>(isIndependentUpdate.name + "_pan");
      sequence.Join((Tween) t2);
    }
    await sequence.AsyncWaitForCompletion();
  }

  public async System.Threading.Tasks.Task ScreenShake(
    float amplitude = 5f,
    float duration = 0.25f,
    float frequency = 5f,
    float dampingExponent = 2f,
    bool useUnscaledTime = true)
  {
    CanvasParallax canvasParallax = this;
    DOTween.Kill((object) (canvasParallax.name + "_shake"));
    Tween shakeTween = canvasParallax._shakeTween;
    if (shakeTween != null)
      shakeTween.Kill();
    canvasParallax._shakeOffset = Vector2.zero;
    float seedX = Random.Range(0.0f, 1000f);
    float seedY = seedX + 123.456f;
    float t = 0.0f;
    canvasParallax._shakeTween = (Tween) DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, duration).SetId<TweenerCore<float, float, FloatOptions>>(canvasParallax.name + "_shake").SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).SetUpdate<TweenerCore<float, float, FloatOptions>>(useUnscaledTime ? UpdateType.Normal : UpdateType.Normal, useUnscaledTime).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._shakeOffset = new Vector2((float) (((double) Mathf.PerlinNoise(seedX, t * frequency) - 0.5) * 2.0), (float) (((double) Mathf.PerlinNoise(seedY, t * frequency) - 0.5) * 2.0)) * amplitude * Mathf.Pow(1f - t, Mathf.Max(0.0001f, dampingExponent)))).OnKill<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._shakeOffset = Vector2.zero));
    await canvasParallax._shakeTween.AsyncWaitForCompletion();
  }

  public void StartScreenRumble(
    float amplitude = 10f,
    float frequency = 8f,
    float amplitudeJitter = 0.25f,
    float frequencyJitter = 0.25f,
    bool useUnscaledTime = true)
  {
    DOTween.Kill((object) (this.name + "_rumble"));
    DOTween.Kill((object) (this.name + "_rumble_timer"));
    Tween rumbleTween = this._rumbleTween;
    if (rumbleTween != null)
      rumbleTween.Kill();
    Tween rumbleTimer = this._rumbleTimer;
    if (rumbleTimer != null)
      rumbleTimer.Kill();
    this._rumbleOffset = Vector2.zero;
    float num1 = Random.Range(0.0f, 1000f);
    float seedX = num1;
    float seedY = num1 + 123.456f;
    float seedAmp = num1 + 654.321f;
    float seedFreq = num1 + 987.654f;
    float noisePhase = 0.0f;
    float ampPhase = 0.0f;
    float freqPhase = 0.0f;
    float ticker = 0.0f;
    this._rumbleTween = (Tween) DOTween.To((DOGetter<float>) (() => ticker), (DOSetter<float>) (x => ticker = x), 1f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).SetLoops<TweenerCore<float, float, FloatOptions>>(-1, DG.Tweening.LoopType.Restart).SetUpdate<TweenerCore<float, float, FloatOptions>>(UpdateType.Normal, useUnscaledTime).SetLink<TweenerCore<float, float, FloatOptions>>(this.gameObject).SetId<TweenerCore<float, float, FloatOptions>>(this.name + "_rumble").OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      float num2 = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
      ampPhase += (float) ((double) num2 * (double) Mathf.Max(0.01f, frequency) * 0.20000000298023224);
      freqPhase += (float) ((double) num2 * (double) Mathf.Max(0.01f, frequency) * 0.15000000596046448);
      float num3 = Mathf.Lerp(1f - Mathf.Clamp01(amplitudeJitter), 1f + Mathf.Clamp01(amplitudeJitter), Mathf.PerlinNoise(seedAmp, ampPhase));
      float num4 = Mathf.Lerp(1f - Mathf.Clamp01(frequencyJitter), 1f + Mathf.Clamp01(frequencyJitter), Mathf.PerlinNoise(seedFreq, freqPhase));
      noisePhase += num2 * Mathf.Max(0.0001f, frequency) * num4;
      this._rumbleOffset = new Vector2((float) (((double) Mathf.PerlinNoise(seedX, noisePhase) - 0.5) * 2.0), (float) (((double) Mathf.PerlinNoise(seedY, noisePhase) - 0.5) * 2.0)) * amplitude * num3;
    })).OnKill<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this._rumbleOffset = Vector2.zero));
  }

  public void StopScreenRumble()
  {
    DOTween.Kill((object) (this.name + "_rumble"));
    DOTween.Kill((object) (this.name + "_rumble_timer"));
    Tween rumbleTween = this._rumbleTween;
    if (rumbleTween != null)
      rumbleTween.Kill();
    Tween rumbleTimer = this._rumbleTimer;
    if (rumbleTimer != null)
      rumbleTimer.Kill();
    this._rumbleOffset = Vector2.zero;
  }

  public async System.Threading.Tasks.Task ScreenRumble(
    float amplitude = 10f,
    float duration = 1.5f,
    float frequency = 8f,
    float amplitudeJitter = 0.25f,
    float frequencyJitter = 0.25f,
    bool useUnscaledTime = true)
  {
    CanvasParallax canvasParallax = this;
    canvasParallax.StartScreenRumble(amplitude, frequency, amplitudeJitter, frequencyJitter, useUnscaledTime);
    float t = 0.0f;
    canvasParallax._rumbleTimer = (Tween) DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, Mathf.Max(0.0f, duration)).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear).SetUpdate<TweenerCore<float, float, FloatOptions>>(UpdateType.Normal, useUnscaledTime).SetLink<TweenerCore<float, float, FloatOptions>>(canvasParallax.gameObject).SetId<TweenerCore<float, float, FloatOptions>>(canvasParallax.name + "_rumble_timer");
    await canvasParallax._rumbleTimer.AsyncWaitForCompletion();
    canvasParallax.StopScreenRumble();
  }

  [CompilerGenerated]
  public float \u003CZoomAsync\u003Eb__21_0() => this.parallaxZoom;

  [CompilerGenerated]
  public void \u003CZoomAsync\u003Eb__21_1(float x) => this.parallaxZoom = x;

  [CompilerGenerated]
  public Vector2 \u003CPanAsync\u003Eb__22_0() => this.offset;

  [CompilerGenerated]
  public void \u003CPanAsync\u003Eb__22_1(Vector2 x) => this.offset = x;
}
