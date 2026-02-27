// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMUIRadialGraphic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (CanvasRenderer))]
public class MMUIRadialGraphic : MaskableGraphic
{
  [SerializeField]
  private Texture _texture;
  [SerializeField]
  private float _outerRadius;
  [SerializeField]
  private float _radius;
  [SerializeField]
  private int _resolution;
  [SerializeField]
  private bool _drawInnerRadius = true;
  [SerializeField]
  private bool _drawOuterRadius = true;
  [SerializeField]
  [Range(0.0f, 360f)]
  private float _radialFill;
  private List<UIVertex> _verts = new List<UIVertex>();

  public override Texture mainTexture
  {
    get
    {
      return !((Object) this._texture == (Object) null) ? this._texture : (Texture) Graphic.s_WhiteTexture;
    }
  }

  public float Radius => this._radius;

  public float OuterRadius => this._outerRadius;

  public float FullRadius => this._radius + this._outerRadius;

  protected override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    this.SetVerticesDirty();
    this.SetMaterialDirty();
  }

  protected override void OnPopulateMesh(VertexHelper vh)
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
      uv0 = Vector2.zero
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
      uiVertex.uv0 = new Vector2(x, this._radius / this.FullRadius);
      this._verts.Add(uiVertex);
      uiVertex.position = (Vector3) vector2_3;
      uiVertex.normal = (Vector3) vector2_1;
      uiVertex.uv0 = new Vector2(x, 1f);
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
