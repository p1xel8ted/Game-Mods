// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIBackgroundMeshRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (RectTransform))]
public class UIBackgroundMeshRenderer : MaskableGraphic
{
  [SerializeField]
  private Texture _texture;
  [Header("Background Properties")]
  [SerializeField]
  [Range(0.0f, 1000f)]
  private float _falloff;
  private List<UIVertex> _verts = new List<UIVertex>();
  private RectTransform _rectTransform;

  public override Texture mainTexture
  {
    get
    {
      return !((Object) this._texture == (Object) null) ? this._texture : (Texture) Graphic.s_WhiteTexture;
    }
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    if (!((Object) this._rectTransform == (Object) null))
      return;
    this._rectTransform = this.GetComponent<RectTransform>();
  }

  protected override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    Debug.Log((object) "Rebuild");
    this.SetVerticesDirty();
    this.SetMaterialDirty();
  }

  protected override void OnPopulateMesh(VertexHelper vh)
  {
    vh.Clear();
    this._verts.Clear();
    Vector2 size = this._rectTransform.rect.size;
    Vector2 vector2_1 = new Vector2(100f, 100f);
    UIVertex uiVertex = new UIVertex()
    {
      color = (Color32) this.color,
      tangent = new Vector4(0.0f, 0.0f, (float) (((double) size.x + (double) this._falloff) / ((double) vector2_1.x + 2.5)), 0.0f)
    };
    Vector2 vector2_2 = new Vector2(0.0f, 0.0f);
    uiVertex.position.x = (float) (-(double) size.x * 0.5);
    uiVertex.position.y = size.y * 0.5f;
    uiVertex.uv0 = vector2_2;
    this._verts.Add(uiVertex);
    uiVertex.position.x = size.x * 0.5f;
    vector2_2.x = size.x / vector2_1.x;
    uiVertex.uv0 = vector2_2;
    this._verts.Add(uiVertex);
    uiVertex.position.y = (float) (-(double) size.y * 0.5);
    vector2_2.y = size.y / vector2_1.y;
    uiVertex.uv0 = vector2_2;
    this._verts.Add(uiVertex);
    uiVertex.position.x = (float) (-(double) size.x * 0.5);
    vector2_2.x = 0.0f;
    uiVertex.uv0 = vector2_2;
    this._verts.Add(uiVertex);
    uiVertex.position.x = size.x * 0.5f + this._falloff;
    uiVertex.position.y = size.y * 0.5f;
    vector2_2.x = (size.x + this._falloff) / vector2_1.x;
    vector2_2.y = 0.0f;
    uiVertex.uv0 = vector2_2;
    this._verts.Add(uiVertex);
    uiVertex.position.y = (float) (-(double) size.y * 0.5);
    vector2_2.y = size.y / vector2_1.y;
    uiVertex.uv0 = vector2_2;
    this._verts.Add(uiVertex);
    int[] numArray = new int[12]
    {
      0,
      1,
      3,
      3,
      1,
      2,
      1,
      4,
      2,
      2,
      4,
      5
    };
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int index = 0; index < numArray.Length; index += 3)
      vh.AddTriangle(numArray[index], numArray[index + 1], numArray[index + 2]);
  }
}
