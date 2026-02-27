// Decompiled with JetBrains decompiler
// Type: WorldMapParallax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class WorldMapParallax : BaseMonoBehaviour
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
  public float _horizon;
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

  public RectTransform RectTransform => this._rectTransform;

  public RectTransform MapContainer => this._mapContainer;

  public float GlobalIntensity => this._globalIntensity;

  public float HorizontalIntensity => this._horizontalIntensity;

  public float VerticalIntensity => this._verticalIntensity;

  public void Update()
  {
    this._rectTransform.localScale = Vector3.one * this._zoom;
    Vector2 vector2_1 = this.ClampPosition(this._rectTransform.anchoredPosition - new Vector2(this._xOffset, this._yOffset)) + new Vector2(this._xOffset, this._yOffset);
    this._rectTransform.anchoredPosition = vector2_1;
    foreach (ParallaxLayer layer in this._layers)
    {
      float depthNormalized = this.GetDepthNormalized(layer);
      Vector2 vector2_2 = new Vector2()
      {
        x = (float) (((double) vector2_1.x - (double) this._xOffset) * (double) depthNormalized * ((double) this._horizontalIntensity * (double) this._globalIntensity)),
        y = (float) (((double) vector2_1.y - (double) this._yOffset) * (double) depthNormalized * ((double) this._verticalIntensity * (double) this._globalIntensity))
      };
      layer.RectTransform.anchoredPosition = vector2_2;
      float num = (1f - depthNormalized) * this._scaleStrength;
      layer.RectTransform.localScale = Vector3.one * this._zoom + Vector3.one * num;
    }
  }

  public Vector2 ClampPosition(Vector2 target)
  {
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
    return (float) (1.0 - (double) layer.Distance / (double) this._horizon);
  }

  public void SetZoom(float zoom) => this._zoom = Mathf.Clamp(zoom, 0.1f, 2f);
}
