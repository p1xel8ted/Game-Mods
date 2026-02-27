// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMUIPolygon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMUIPolygon : MaskableGraphic
{
  [SerializeField]
  private Texture _texture;
  [SerializeField]
  private List<Vector2> _points;
  [SerializeField]
  [Range(1f, 1000f)]
  private float _roundToNearest = 1f;
  private List<UIVertex> _verts = new List<UIVertex>();

  public override Texture mainTexture
  {
    get
    {
      return !((Object) this._texture == (Object) null) ? this._texture : (Texture) Graphic.s_WhiteTexture;
    }
  }

  public List<Vector2> Points
  {
    get => this._points;
    set
    {
      this._points = value;
      this.SetVerticesDirty();
      this.SetMaterialDirty();
    }
  }

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
    if (this._points == null || this._points.Count < 3)
      return;
    UIVertex uiVertex = new UIVertex()
    {
      color = (Color32) this.color
    };
    int[] numArray = new Triangulator(this._points.ToArray()).Triangulate();
    foreach (Vector2 point in this._points)
    {
      uiVertex.position = (Vector3) point;
      this._verts.Add(uiVertex);
    }
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int index = 0; index < numArray.Length; index += 3)
      vh.AddTriangle(numArray[index], numArray[index + 1], numArray[index + 2]);
  }
}
