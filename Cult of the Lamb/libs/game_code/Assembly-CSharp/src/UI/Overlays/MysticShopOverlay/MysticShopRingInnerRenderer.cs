// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.MysticShopOverlay.MysticShopRingInnerRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Overlays.MysticShopOverlay;

public class MysticShopRingInnerRenderer : MaskableGraphic
{
  [SerializeField]
  public Texture _texture;
  [SerializeField]
  [Range(0.0f, 10f)]
  public int _segments = 4;
  [SerializeField]
  public float _radius = 100f;
  [SerializeField]
  [Range(0.0f, 500f)]
  public float _thickness;
  [SerializeField]
  [Range(0.0f, 250f)]
  public int _resolution;
  [SerializeField]
  [Range(0.0f, 3f)]
  public float _uvFix;
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

  public int Segments
  {
    get => this._segments;
    set
    {
      this._segments = value;
      this.OnRectTransformDimensionsChange();
    }
  }

  public override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    this.SetVerticesDirty();
    this.SetMaterialDirty();
  }

  public override void OnPopulateMesh(VertexHelper vh)
  {
    if (this._resolution == 0)
      return;
    vh.Clear();
    this._verts.Clear();
    int num1 = 0;
    int width = this.mainTexture.width;
    float num2 = 360f / (float) this._segments;
    float num3 = num2 / (float) this._resolution;
    UIVertex uiVertex = new UIVertex()
    {
      color = (Color32) this.color,
      normal = (Vector3) Vector2.zero
    };
    for (int index1 = 0; index1 < this._segments; ++index1)
    {
      num1 += this._verts.Count;
      this._verts.Clear();
      float num4 = 0.0f;
      for (int index2 = 0; index2 <= this._resolution; ++index2)
      {
        float f = (float) ((90.0 + (double) num2 * (double) index1 + (double) num3 * (double) index2) * (Math.PI / 180.0));
        Vector2 vector2_1 = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
        Vector2 vector2_2 = vector2_1 * (this._radius - this._thickness * 0.5f);
        uiVertex.position = (Vector3) vector2_2;
        if (index2 > 0)
          num4 += Vector2.Distance((Vector2) uiVertex.position, (Vector2) this._verts[this._verts.Count - 2].position);
        uiVertex.uv0.y = 0.0f;
        uiVertex.uv0.x = num4 / (float) width * this._uvFix;
        this._verts.Add(uiVertex);
        Vector2 vector2_3 = vector2_1 * (this._radius + this._thickness * 0.5f);
        uiVertex.position = (Vector3) vector2_3;
        uiVertex.uv0.y = 1f;
        uiVertex.uv0.x = num4 / (float) width * this._uvFix;
        this._verts.Add(uiVertex);
      }
      foreach (UIVertex vert in this._verts)
        vh.AddVert(vert);
      for (int idx0 = num1; idx0 <= vh.currentVertCount - 4; idx0 += 2)
      {
        vh.AddTriangle(idx0, idx0 + 3, idx0 + 1);
        vh.AddTriangle(idx0, idx0 + 2, idx0 + 3);
      }
    }
  }
}
