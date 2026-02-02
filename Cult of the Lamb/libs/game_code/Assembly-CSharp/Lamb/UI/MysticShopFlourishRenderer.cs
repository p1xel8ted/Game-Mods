// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MysticShopFlourishRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (CanvasRenderer))]
public class MysticShopFlourishRenderer : MaskableGraphic
{
  [SerializeField]
  public Texture _texture;
  [SerializeField]
  [Range(0.0f, 500f)]
  public float _radius = 100f;
  [Header("Line")]
  [SerializeField]
  [Range(0.0f, 100f)]
  public int _resolution;
  [SerializeField]
  [Range(0.0f, 0.5f)]
  public float _fill = 0.25f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float _fillScaler = 1f;
  [SerializeField]
  [Range(0.0f, 500f)]
  public float _centerSize;
  [SerializeField]
  [Range(0.0f, 3f)]
  public float _uvFix = 1f;
  [Header("Cap")]
  [SerializeField]
  [Range(0.0f, 360f)]
  public float _capSize;
  public RectTransform _rectTransform;
  public List<UIVertex> _verts = new List<UIVertex>();

  public override Texture mainTexture
  {
    get
    {
      return !((UnityEngine.Object) this._texture == (UnityEngine.Object) null) ? this._texture : (Texture) Graphic.s_WhiteTexture;
    }
  }

  public float Radius
  {
    get => this._radius;
    set
    {
      this._radius = value;
      this.OnRectTransformDimensionsChange();
    }
  }

  public float Fill
  {
    get => this._fill;
    set
    {
      this._fill = value;
      this.OnRectTransformDimensionsChange();
    }
  }

  public float FillScaler
  {
    get => this._fillScaler;
    set
    {
      this._fillScaler = value;
      this.OnRectTransformDimensionsChange();
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this._rectTransform = this.GetComponent<RectTransform>();
  }

  public override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    this.SetVerticesDirty();
    this.SetMaterialDirty();
  }

  public override void OnPopulateMesh(VertexHelper vh)
  {
    vh.Clear();
    this._verts.Clear();
    int num1 = 0;
    int width = this.mainTexture.width;
    Vector2 size = this._rectTransform.rect.size;
    UIVertex uiVertex = new UIVertex()
    {
      color = (Color32) this.color,
      normal = (Vector3) Vector2.zero
    };
    float num2 = Mathf.Sin(1.57079637f) * this._radius;
    uiVertex.position.x = (float) (-(double) size.x * 0.5);
    uiVertex.position.y = (float) (-(double) size.y * 0.5) + num2;
    uiVertex.uv0.x = 0.0f;
    uiVertex.uv0.y = 0.0f;
    this._verts.Add(uiVertex);
    uiVertex.position.x = (float) (-(double) size.x * 0.5);
    uiVertex.position.y = size.y * 0.5f + num2;
    uiVertex.uv0.x = 0.0f;
    uiVertex.uv0.y = 0.5f;
    this._verts.Add(uiVertex);
    uiVertex.position.x = size.x * 0.5f;
    uiVertex.position.y = size.y * 0.5f + num2;
    uiVertex.uv0.x = 0.5f;
    uiVertex.uv0.y = 0.5f;
    this._verts.Add(uiVertex);
    uiVertex.position.x = size.x * 0.5f;
    uiVertex.position.y = (float) (-(double) size.y * 0.5) + num2;
    uiVertex.uv0.x = 0.5f;
    uiVertex.uv0.y = 0.0f;
    this._verts.Add(uiVertex);
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int idx0 = 0; idx0 <= vh.currentVertCount - 4; idx0 += 4)
    {
      vh.AddTriangle(idx0, idx0 + 1, idx0 + 2);
      vh.AddTriangle(idx0, idx0 + 2, idx0 + 3);
    }
    if (this._resolution == 0)
      return;
    float num3 = 0.0f;
    float b = Mathf.Tan(this._centerSize * 0.5f / this._radius) * 57.29578f;
    float num4 = Mathf.Max(0.0f, 360f * this._fill * this._fillScaler - b) / (float) this._resolution;
    int num5 = num1 + this._verts.Count;
    this._verts.Clear();
    for (int index = 0; index <= this._resolution; ++index)
    {
      float f = (float) ((90.0 + (double) b + (double) num4 * (double) index) * (Math.PI / 180.0));
      Vector2 vector2_1 = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
      Vector2 vector2_2 = vector2_1 * (this._radius - size.y * 0.5f);
      uiVertex.position = (Vector3) vector2_2;
      uiVertex.position.x *= -1f;
      if (index > 0)
        num3 += Vector2.Distance((Vector2) uiVertex.position, (Vector2) this._verts[this._verts.Count - 2].position);
      uiVertex.uv0.y = 0.5f;
      uiVertex.uv0.x = num3 / (float) width * this._uvFix;
      this._verts.Add(uiVertex);
      Vector2 vector2_3 = vector2_1 * (this._radius + size.y * 0.5f);
      uiVertex.position = (Vector3) vector2_3;
      uiVertex.position.x *= -1f;
      uiVertex.uv0.y = 1f;
      uiVertex.uv0.x = num3 / (float) width * this._uvFix;
      this._verts.Add(uiVertex);
    }
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int idx2 = num5; idx2 <= vh.currentVertCount - 4; idx2 += 2)
    {
      vh.AddTriangle(idx2 + 1, idx2 + 3, idx2);
      vh.AddTriangle(idx2 + 3, idx2 + 2, idx2);
    }
    int num6 = num5 + this._verts.Count;
    this._verts.Clear();
    float num7 = 0.0f;
    for (int index = 0; index <= this._resolution; ++index)
    {
      float f = (float) ((90.0 + (double) b + (double) num4 * (double) index) * (Math.PI / 180.0));
      Vector2 vector2_4 = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
      Vector2 vector2_5 = vector2_4 * (this._radius - size.y * 0.5f);
      uiVertex.position = (Vector3) vector2_5;
      if (index > 0)
        num7 += Vector2.Distance((Vector2) uiVertex.position, (Vector2) this._verts[this._verts.Count - 2].position);
      uiVertex.uv0.y = 1f;
      uiVertex.uv0.x = num7 / (float) width * this._uvFix;
      this._verts.Add(uiVertex);
      Vector2 vector2_6 = vector2_4 * (this._radius + size.y * 0.5f);
      uiVertex.position = (Vector3) vector2_6;
      uiVertex.uv0.y = 0.5f;
      uiVertex.uv0.x = num7 / (float) width * this._uvFix;
      this._verts.Add(uiVertex);
    }
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int idx0 = num6; idx0 <= vh.currentVertCount - 4; idx0 += 2)
    {
      vh.AddTriangle(idx0, idx0 + 3, idx0 + 1);
      vh.AddTriangle(idx0, idx0 + 2, idx0 + 3);
    }
    int num8 = num6 + this._verts.Count;
    this._verts.Clear();
    float f1 = (float) ((90.0 + (double) Mathf.Max(360f * this._fill * this._fillScaler, b)) * (Math.PI / 180.0));
    Vector2 vector2_7 = new Vector2(Mathf.Cos(f1), Mathf.Sin(f1));
    Vector2 vector2_8 = vector2_7 * (this._radius - size.y * 0.5f);
    uiVertex.position = (Vector3) vector2_8;
    uiVertex.position.x *= -1f;
    uiVertex.uv0.y = 0.0f;
    uiVertex.uv0.x = 1f;
    this._verts.Add(uiVertex);
    Vector2 vector2_9 = vector2_7 * (this._radius + size.y * 0.5f);
    uiVertex.position = (Vector3) vector2_9;
    uiVertex.position.x *= -1f;
    uiVertex.uv0.y = 0.5f;
    uiVertex.uv0.x = 1f;
    this._verts.Add(uiVertex);
    Vector2 vector2_10 = (Vector2) (Quaternion.Euler(0.0f, 0.0f, 90f) * (Vector3) vector2_7);
    Vector2 vector2_11 = vector2_8 + vector2_10 * this._capSize;
    uiVertex.position = (Vector3) vector2_11;
    uiVertex.position.x *= -1f;
    uiVertex.uv0.y = 0.0f;
    uiVertex.uv0.x = 0.75f;
    this._verts.Add(uiVertex);
    Vector2 vector2_12 = vector2_9 + vector2_10 * this._capSize;
    uiVertex.position = (Vector3) vector2_12;
    uiVertex.position.x *= -1f;
    uiVertex.uv0.y = 0.5f;
    uiVertex.uv0.x = 0.75f;
    this._verts.Add(uiVertex);
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int idx2 = num8; idx2 <= vh.currentVertCount - 4; idx2 += 2)
    {
      vh.AddTriangle(idx2 + 1, idx2 + 3, idx2);
      vh.AddTriangle(idx2 + 3, idx2 + 2, idx2);
    }
    int num9 = num8 + this._verts.Count;
    this._verts.Clear();
    float f2 = (float) ((90.0 + (double) Mathf.Max(360f * this._fill * this._fillScaler, b)) * (Math.PI / 180.0));
    Vector2 vector2_13 = new Vector2(Mathf.Cos(f2), Mathf.Sin(f2));
    Vector2 vector2_14 = vector2_13 * (this._radius - size.y * 0.5f);
    uiVertex.position = (Vector3) vector2_14;
    uiVertex.uv0.y = 0.0f;
    uiVertex.uv0.x = 1f;
    this._verts.Add(uiVertex);
    Vector2 vector2_15 = vector2_13 * (this._radius + size.y * 0.5f);
    uiVertex.position = (Vector3) vector2_15;
    uiVertex.uv0.y = 0.5f;
    uiVertex.uv0.x = 1f;
    this._verts.Add(uiVertex);
    Vector2 vector2_16 = (Vector2) (Quaternion.Euler(0.0f, 0.0f, 90f) * (Vector3) vector2_13);
    Vector2 vector2_17 = vector2_14 + vector2_16 * this._capSize;
    uiVertex.position = (Vector3) vector2_17;
    uiVertex.uv0.y = 0.0f;
    uiVertex.uv0.x = 0.75f;
    this._verts.Add(uiVertex);
    Vector2 vector2_18 = vector2_15 + vector2_16 * this._capSize;
    uiVertex.position = (Vector3) vector2_18;
    uiVertex.uv0.y = 0.5f;
    uiVertex.uv0.x = 0.75f;
    this._verts.Add(uiVertex);
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int idx0 = num9; idx0 <= vh.currentVertCount - 4; idx0 += 2)
    {
      vh.AddTriangle(idx0, idx0 + 3, idx0 + 1);
      vh.AddTriangle(idx0, idx0 + 2, idx0 + 3);
    }
  }
}
