// Decompiled with JetBrains decompiler
// Type: WorldMapParallax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class WorldMapParallax : BaseMonoBehaviour
{
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _mapContainer;
  [SerializeField]
  private ParallaxLayer[] _layers;
  [Header("Settings")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  private float _horizon;
  [SerializeField]
  [Range(0.0f, 2f)]
  private float _globalIntensity = 1f;
  [SerializeField]
  [Range(0.0f, 2f)]
  private float _horizontalIntensity = 1f;
  [SerializeField]
  [Range(0.0f, 2f)]
  private float _verticalIntensity = 1f;
  [SerializeField]
  [Range(0.0f, 2f)]
  private float _scaleStrength = 1f;
  [Header("Extents")]
  [SerializeField]
  private float _left;
  [SerializeField]
  private float _right;
  [SerializeField]
  private float _top;
  [SerializeField]
  private float _bottom;

  public RectTransform RectTransform => this._rectTransform;

  public RectTransform MapContainer => this._mapContainer;

  public float GlobalIntensity => this._globalIntensity;

  public float HorizontalIntensity => this._horizontalIntensity;

  public float VerticalIntensity => this._verticalIntensity;

  public void Update()
  {
    Vector2 vector2_1 = this.ClampPosition(this._rectTransform.anchoredPosition);
    this._rectTransform.anchoredPosition = vector2_1;
    foreach (ParallaxLayer layer in this._layers)
    {
      float depthNormalized = this.GetDepthNormalized(layer);
      Vector2 vector2_2 = new Vector2()
      {
        x = (float) ((double) vector2_1.x * (double) depthNormalized * ((double) this._horizontalIntensity * (double) this._globalIntensity)),
        y = (float) ((double) vector2_1.y * (double) depthNormalized * ((double) this._verticalIntensity * (double) this._globalIntensity))
      };
      layer.RectTransform.anchoredPosition = vector2_2;
      layer.RectTransform.localScale = Vector3.one + this._rectTransform.localScale * (1f - depthNormalized) * this._scaleStrength;
    }
  }

  public Vector2 ClampPosition(Vector2 target)
  {
    Vector2 localScale = (Vector2) this._mapContainer.localScale;
    float num1 = (float) (1.0 / ((double) this._horizontalIntensity * (double) this._globalIntensity));
    float num2 = (float) (1.0 / ((double) this._verticalIntensity * (double) this._globalIntensity));
    target.x = Mathf.Clamp(target.x, this._left * localScale.x * num1, this._right * localScale.x * num1);
    target.y = Mathf.Clamp(target.y, this._bottom * localScale.y * num2, this._top * localScale.y * num2);
    return target;
  }

  public float GetDepthNormalized(ParallaxLayer layer)
  {
    return (float) (1.0 - (double) layer.Distance / (double) this._horizon);
  }
}
