// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMUIDialogWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (RectTransform))]
public class MMUIDialogWindow : MaskableGraphic
{
  [SerializeField]
  public Texture _texture;
  [SerializeField]
  public float _arrowWidth;
  [SerializeField]
  public float _arrowHeight;
  public List<UIVertex> _verts = new List<UIVertex>();
  public RectTransform _rectTransform;

  public override Texture mainTexture
  {
    get
    {
      return !((Object) this._texture == (Object) null) ? this._texture : (Texture) Graphic.s_WhiteTexture;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!((Object) this._rectTransform == (Object) null))
      return;
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
    UIVertex uiVertex = new UIVertex()
    {
      color = (Color32) this.color
    };
    Vector2 size = this._rectTransform.rect.size;
    uiVertex.uv0.x = 0.0f;
    uiVertex.uv0.y = 0.0f;
    uiVertex.position.x = (float) (-(double) size.x * 0.5);
    uiVertex.position.y = size.y * 0.5f;
    this._verts.Add(uiVertex);
    uiVertex.position.x = size.x * 0.5f;
    this._verts.Add(uiVertex);
    uiVertex.position.y = (float) (-(double) size.y * 0.5);
    this._verts.Add(uiVertex);
    uiVertex.position.x = this._arrowWidth * 0.5f;
    this._verts.Add(uiVertex);
    uiVertex.position.x = 0.0f;
    uiVertex.position.y = (float) (-(double) size.y * 0.5) - this._arrowHeight;
    this._verts.Add(uiVertex);
    uiVertex.position.x = (float) (-(double) this._arrowWidth * 0.5);
    uiVertex.position.y = (float) (-(double) size.y * 0.5);
    this._verts.Add(uiVertex);
    uiVertex.position.x = (float) (-(double) size.x * 0.5);
    this._verts.Add(uiVertex);
    int[] numArray = new int[15]
    {
      0,
      5,
      6,
      0,
      3,
      5,
      0,
      1,
      3,
      1,
      2,
      3,
      5,
      3,
      4
    };
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int index = 0; index < numArray.Length; index += 3)
      vh.AddTriangle(numArray[index], numArray[index + 1], numArray[index + 2]);
  }
}
