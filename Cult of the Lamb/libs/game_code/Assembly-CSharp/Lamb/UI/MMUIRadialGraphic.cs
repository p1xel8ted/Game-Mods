// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMUIRadialGraphic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (CanvasRenderer))]
public class MMUIRadialGraphic : MaskableGraphic
{
  [SerializeField]
  public Texture _texture;
  [SerializeField]
  public float _outerRadius;
  [SerializeField]
  public float _radius;
  [SerializeField]
  public int _resolution;
  [SerializeField]
  public bool _drawInnerRadius = true;
  [SerializeField]
  public bool _drawOuterRadius = true;
  [SerializeField]
  [Range(0.0f, 360f)]
  public float _radialFill;
  public List<UIVertex> _verts = new List<UIVertex>();

  public override Texture mainTexture
  {
    get
    {
      return !((Object) this._texture == (Object) null) ? this._texture : (Texture) Graphic.s_WhiteTexture;
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

  public float OuterRadius
  {
    get => this._outerRadius;
    set
    {
      this._outerRadius = value;
      this.OnRectTransformDimensionsChange();
    }
  }

  public float FullRadius => this._radius + this._outerRadius;

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
    if (this._resolution == 0)
      return;
    UIVertex uiVertex = new UIVertex()
    {
      color = (Color32) this.color
    } with
    {
      position = (Vector3) Vector2.zero,
      normal = (Vector3) Vector2.zero,
      uv0 = (Vector4) Vector2.zero
    };
    this._verts.Add(uiVertex);
    for (int index = 0; index <= this._resolution; ++index)
    {
      float x = (float) index / (float) this._resolution;
      float f = 6.28318548f * x;
      Vector2 vector2_1 = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
      Vector2 vector2_2 = vector2_1 * this._radius;
      Vector2 vector2_3 = vector2_1 * (this._radius + this._outerRadius);
      uiVertex.position = (Vector3) vector2_2;
      uiVertex.normal = (Vector3) Vector2.zero;
      uiVertex.uv0 = (Vector4) new Vector2(x, this._radius / this.FullRadius);
      this._verts.Add(uiVertex);
      uiVertex.position = (Vector3) vector2_3;
      uiVertex.normal = (Vector3) vector2_1;
      uiVertex.uv0 = (Vector4) new Vector2(x, 1f);
      this._verts.Add(uiVertex);
    }
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int index = 0; index < vh.currentVertCount - 4; index += 2)
    {
      if (this._drawInnerRadius)
        vh.AddTriangle(0, index + 3, index + 1);
      if (this._drawOuterRadius)
      {
        vh.AddTriangle(index + 1, index + 4, index + 2);
        vh.AddTriangle(index + 1, index + 3, index + 4);
      }
    }
  }
}
