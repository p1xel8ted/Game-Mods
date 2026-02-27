// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMUITiledRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (RectTransform))]
public class MMUITiledRenderer : MaskableGraphic
{
  [SerializeField]
  public Sprite _sprite;
  [SerializeField]
  public bool _tileVertical;
  [SerializeField]
  public bool _tileHorizonal;
  public List<UIVertex> _verts = new List<UIVertex>();
  public RectTransform _rectTransform;
  public Canvas _cachedCanvas;

  public override Texture mainTexture
  {
    get
    {
      return !((Object) this._sprite.texture == (Object) null) ? (Texture) this._sprite.texture : (Texture) Graphic.s_WhiteTexture;
    }
  }

  public Canvas _canvas
  {
    get
    {
      if ((Object) this._cachedCanvas == (Object) null)
      {
        foreach (Canvas canvas in this.gameObject.GetComponentsInParent<Canvas>())
        {
          if (canvas.isActiveAndEnabled)
          {
            this._cachedCanvas = canvas;
            break;
          }
        }
      }
      return this._cachedCanvas;
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
    Vector2 size = this._rectTransform.rect.size;
    Vector2 vector2_1 = new Vector2((float) this.mainTexture.width, (float) this.mainTexture.height);
    if ((Object) this._canvas != (Object) null)
    {
      vector2_1.x *= this._canvas.referencePixelsPerUnit / this._sprite.pixelsPerUnit;
      vector2_1.y *= this._canvas.referencePixelsPerUnit / this._sprite.pixelsPerUnit;
    }
    UIVertex uiVertex = new UIVertex()
    {
      color = (Color32) this.color
    };
    Vector2 vector2_2 = new Vector2(0.0f, 1f);
    uiVertex.position.x = (float) (-(double) size.x * 0.5);
    uiVertex.position.y = size.y * 0.5f;
    uiVertex.uv0 = (Vector4) vector2_2;
    this._verts.Add(uiVertex);
    uiVertex.position.x = size.x * 0.5f;
    vector2_2.x = !this._tileHorizonal ? 1f : size.x / vector2_1.x;
    uiVertex.uv0 = (Vector4) vector2_2;
    this._verts.Add(uiVertex);
    uiVertex.position.y = (float) (-(double) size.y * 0.5);
    vector2_2.y = 0.0f;
    uiVertex.uv0 = (Vector4) vector2_2;
    this._verts.Add(uiVertex);
    uiVertex.position.x = (float) (-(double) size.x * 0.5);
    vector2_2.x = 0.0f;
    uiVertex.uv0 = (Vector4) vector2_2;
    this._verts.Add(uiVertex);
    int[] numArray = new int[6]{ 0, 1, 3, 3, 1, 2 };
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int index = 0; index < numArray.Length; index += 3)
      vh.AddTriangle(numArray[index], numArray[index + 1], numArray[index + 2]);
  }
}
