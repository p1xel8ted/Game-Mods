// Decompiled with JetBrains decompiler
// Type: DLCMapParallax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class DLCMapParallax : MonoBehaviour
{
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _mapContainer;
  [SerializeField]
  public ParallaxLayer[] _layers;
  [Header("Settings")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  public float _horizon = 100f;
  [SerializeField]
  [Range(0.0f, 2f)]
  public float _globalIntensity = 1f;
  [SerializeField]
  [Range(0.0f, 2f)]
  public float _horizontalIntensity = 1f;
  [SerializeField]
  [Range(0.0f, 2f)]
  public float _verticalIntensity = 1f;
  [SerializeField]
  [Range(0.0f, 2f)]
  public float _scaleStrength = 1f;
  [Header("Extents")]
  [SerializeField]
  public float _left;
  [SerializeField]
  public float _right;
  [SerializeField]
  public float _top;
  [SerializeField]
  public float _bottom;
  [Header("Offset")]
  [SerializeField]
  public float _xOffset;
  [SerializeField]
  public float _yOffset;
  [Header("Zoom")]
  [SerializeField]
  [Range(0.1f, 2f)]
  public float _zoom = 1f;
  [Header("Special Parallax Output")]
  [SerializeField]
  public RectTransform _specialLayer;
  [SerializeField]
  public Vector2 _specialLayerOffset = Vector2.zero;
  [SerializeField]
  public float _specialXRange = 100f;
  [SerializeField]
  public float _specialParallaxStrength = 1f;
  [Header("Pivot Override")]
  [SerializeField]
  public bool _forcePivot;
  [SerializeField]
  public Vector2 _pivotOverride = new Vector2(0.5f, 0.5f);
  [Header("Debug")]
  [SerializeField]
  public bool _debugMode;
  [SerializeField]
  public Vector2 _editorTestPosition;
  [SerializeField]
  public RectTransform _editorTestTargetRect;
  public Vector2 movePos;
  public Vector2 rawPos;
  public Vector2 clampedPos;
  public bool _isTweening = true;
  public Vector2 _targetMapPosition;

  public float Zoom => this._zoom;

  public void ApplyEditorTest()
  {
  }

  public void Update()
  {
    if ((Object) this._rectTransform == (Object) null || (Object) this._mapContainer == (Object) null || this._layers == null)
      return;
    if (this._debugMode)
      this.ApplyEditorTest();
    this._rectTransform.localScale = Vector3.one * this._zoom;
    Vector2 mapAnchoredPos = this._targetMapPosition - new Vector2(this._xOffset, this._yOffset) + new Vector2(this._xOffset, this._yOffset);
    this._rectTransform.anchoredPosition = mapAnchoredPos;
    this.UpdateParallaxLayers(mapAnchoredPos);
    RectTransform root = this._rectTransform.root as RectTransform;
    if (!((Object) root != (Object) null) || !((Object) this._specialLayer != (Object) null))
      return;
    this._specialLayer.anchoredPosition = new Vector2(-Mathf.Clamp((this._rectTransform.anchoredPosition.x - this._xOffset) / root.rect.width, -1f, 1f) * this._specialXRange * this._specialParallaxStrength * this._globalIntensity, 0.0f) + this._specialLayerOffset;
  }

  public void UpdateParallaxLayers(Vector2 mapAnchoredPos)
  {
    foreach (ParallaxLayer layer in this._layers)
    {
      if (this._forcePivot && (Object) layer != (Object) null && (Object) layer.RectTransform != (Object) null)
        layer.RectTransform.pivot = this._pivotOverride;
      float depthNormalized = this.GetDepthNormalized(layer);
      Vector2 vector2 = new Vector2((float) (((double) mapAnchoredPos.x - (double) this._xOffset) * (double) depthNormalized * ((double) this._horizontalIntensity * (double) this._globalIntensity)), (float) (((double) mapAnchoredPos.y - (double) this._yOffset) * (double) depthNormalized * ((double) this._verticalIntensity * (double) this._globalIntensity)));
      Vector3 vector3 = Vector3.one * this._zoom + Vector3.one * ((1f - depthNormalized) * this._scaleStrength);
      layer.RectTransform.anchoredPosition = vector2;
      layer.RectTransform.localScale = vector3;
      int num = this._debugMode ? 1 : 0;
    }
  }

  public Vector2 ClampPosition(Vector2 target)
  {
    if ((Object) this._mapContainer == (Object) null)
      return target;
    Vector2 localScale = (Vector2) this._mapContainer.localScale;
    float num1 = (float) (1.0 / ((double) this._horizontalIntensity * (double) this._globalIntensity));
    float num2 = (float) (1.0 / ((double) this._verticalIntensity * (double) this._globalIntensity));
    float num3 = this._zoom * localScale.x * num1;
    float num4 = this._zoom * localScale.y * num2;
    target.x = Mathf.Clamp(target.x, this._left * num3, this._right * num3);
    target.y = Mathf.Clamp(target.y, this._bottom * num4, this._top * num4);
    return target;
  }

  public float GetDepthNormalized(ParallaxLayer layer)
  {
    return Mathf.Clamp01((layer.Distance - this._horizon) / this._horizon);
  }

  public void SetZoom(float zoom) => this._zoom = Mathf.Clamp(zoom, 0.1f, 2f);

  public void RefreshLayers()
  {
    if ((Object) this._mapContainer == (Object) null)
    {
      Debug.LogWarning((object) "Parallax: _mapContainer is null.");
    }
    else
    {
      this._layers = this._mapContainer.GetComponentsInChildren<ParallaxLayer>(true);
      Debug.Log((object) $"Parallax: found {this._layers.Length} layers.");
    }
  }

  public async System.Threading.Tasks.Task TweenZoom(float targetZoom, float duration = 0.5f, Ease ease = Ease.OutCubic)
  {
    DLCMapParallax dlcMapParallax = this;
    targetZoom = Mathf.Clamp(targetZoom, 0.1f, 2f);
    DOTween.Kill((object) (dlcMapParallax.name + "_zoom"));
    await DOVirtual.Float(dlcMapParallax._zoom, targetZoom, duration, new TweenCallback<float>(dlcMapParallax.\u003CTweenZoom\u003Eb__33_0)).SetEase<Tweener>(ease).SetUpdate<Tweener>(true).SetId<Tweener>(dlcMapParallax.name + "_zoom").AsyncWaitForCompletion();
    Debug.Log((object) $"[Parallax] Set zoom: {dlcMapParallax._zoom}");
  }

  public Vector2 TargetMapPosition => this._targetMapPosition;

  public void Awake() => this._targetMapPosition = this._rectTransform.anchoredPosition;

  public void SetParallax(Vector2 targetMapPosition)
  {
    this._targetMapPosition = targetMapPosition;
    this.UpdateParallaxLayers(this._rectTransform.anchoredPosition);
  }

  public void SetParallaxTween(Vector2 to, float duration = 0.5f, Ease ease = Ease.OutCubic)
  {
    DOTween.Kill((object) this);
    DOVirtual.Vector3((Vector3) this._targetMapPosition, (Vector3) to, duration, (TweenCallback<Vector3>) (v => this._targetMapPosition = (Vector2) v)).SetEase<Tweener>(ease).SetUpdate<Tweener>(true).SetId<Tweener>((object) this);
  }

  public void TweenParallaxToNode(
    RectTransform nodeRectTransform,
    float targetZoom = -1f,
    float duration = 0.5f,
    Ease ease = Ease.OutCubic)
  {
    DOTween.Kill((object) this);
    Vector2 anchoredPosition = nodeRectTransform.anchoredPosition;
    anchoredPosition.y = -anchoredPosition.y;
    DOVirtual.Vector3((Vector3) this._targetMapPosition, (Vector3) anchoredPosition, duration, (TweenCallback<Vector3>) (v => this._targetMapPosition = (Vector2) v)).SetEase<Tweener>(ease).SetUpdate<Tweener>(true).SetId<Tweener>((object) this);
    if ((double) targetZoom <= 0.0)
      return;
    DOVirtual.Float(this._zoom, targetZoom, duration, (TweenCallback<float>) (v => this.SetZoom(v))).SetEase<Tweener>(ease).SetUpdate<Tweener>(true).SetId<Tweener>((object) this);
  }

  [CompilerGenerated]
  public void \u003CTweenZoom\u003Eb__33_0(float v)
  {
    this.SetZoom(v);
    if ((Object) this._rectTransform != (Object) null)
      this._rectTransform.localScale = Vector3.one * this._zoom;
    if (!((Object) this._rectTransform != (Object) null))
      return;
    this.UpdateParallaxLayers(this._rectTransform.anchoredPosition);
  }

  [CompilerGenerated]
  public void \u003CSetParallaxTween\u003Eb__43_0(Vector3 v)
  {
    this._targetMapPosition = (Vector2) v;
  }

  [CompilerGenerated]
  public void \u003CTweenParallaxToNode\u003Eb__44_0(Vector3 v)
  {
    this._targetMapPosition = (Vector2) v;
  }

  [CompilerGenerated]
  public void \u003CTweenParallaxToNode\u003Eb__44_1(float v) => this.SetZoom(v);
}
