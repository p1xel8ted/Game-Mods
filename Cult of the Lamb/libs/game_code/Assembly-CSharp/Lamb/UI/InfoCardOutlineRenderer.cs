// Decompiled with JetBrains decompiler
// Type: Lamb.UI.InfoCardOutlineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (CanvasRenderer))]
public class InfoCardOutlineRenderer : MaskableGraphic
{
  [SerializeField]
  public Texture _texture;
  [Header("Outline Properties")]
  [SerializeField]
  public bool _showBadge = true;
  [SerializeField]
  [Range(-1f, 7f)]
  public int _badgeVariant;
  [SerializeField]
  [Range(1f, 100f)]
  public float _cornerSize = 100f;
  [SerializeField]
  [Range(1f, 100f)]
  public float _capSize = 10f;
  [SerializeField]
  [Range(-100f, 100f)]
  public float _offset;
  [Header("Lines")]
  [SerializeField]
  public bool _topLine = true;
  [SerializeField]
  public bool _bottomLine = true;
  [SerializeField]
  public bool _leftLine = true;
  [SerializeField]
  public bool _rightLine = true;
  [Header("Gaps")]
  [SerializeField]
  [Range(0.0f, 250f)]
  public float _topGap;
  [SerializeField]
  [Range(0.0f, 250f)]
  public float _leftGap;
  [SerializeField]
  [Range(0.0f, 250f)]
  public float _rightGap;
  [SerializeField]
  [Range(0.0f, 250f)]
  public float _bottomGap;
  public List<UIVertex> _verts = new List<UIVertex>();

  public override Texture mainTexture
  {
    get
    {
      return !((Object) this._texture == (Object) null) ? this._texture : (Texture) Graphic.s_WhiteTexture;
    }
  }

  public int BadgeVariant
  {
    get => this._badgeVariant;
    set
    {
      this._badgeVariant = value;
      this.SetVerticesDirty();
      this.SetMaterialDirty();
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
    // ISSUE: variable of a compiler-generated type
    InfoCardOutlineRenderer.\u003C\u003Ec__DisplayClass21_0 cDisplayClass210;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.\u003C\u003E4__this = this;
    vh.Clear();
    this._verts.Clear();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.scale = (float) this._texture.width / 4f / this._cornerSize;
    Rect rect = this.rectTransform.rect;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.halfPI = 1.57079637f;
    float num = rect.width * 0.5f + this._offset;
    float y = rect.height * 0.5f + this._offset;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.colorTransparent = new Color(this.color.r, this.color.g, this.color.b, 0.0f);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass210.vert = new UIVertex()
    {
      color = (Color32) this.color,
      normal = (Vector3) Vector2.zero
    };
    this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2(-num, y), new Vector2(0.0f, 0.5f), ref cDisplayClass210);
    this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2(num - this._cornerSize, y), new Vector2(0.25f, 0.5f), ref cDisplayClass210);
    this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2(num - this._cornerSize, -y + this._cornerSize), new Vector2(0.5f, 0.5f), ref cDisplayClass210);
    this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2(-num, -y + this._cornerSize), new Vector2(0.75f, 0.5f), ref cDisplayClass210);
    if (this._topLine)
    {
      if (this._showBadge)
      {
        this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2((float) (-(double) this._cornerSize * 0.5), y), new Vector2(0.25f * (float) this._badgeVariant - Mathf.Floor((float) this._badgeVariant / 4f), (float) (1.0 - 0.25 * (double) Mathf.Floor((float) this._badgeVariant / 4f))), ref cDisplayClass210);
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(-num + this._cornerSize, y - this._cornerSize * 0.5f), new Vector2((float) (-(double) this._cornerSize * 0.5), y - this._cornerSize * 0.5f), ref cDisplayClass210);
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize, y - this._cornerSize * 0.5f), new Vector2(this._cornerSize * 0.5f, y - this._cornerSize * 0.5f), ref cDisplayClass210);
      }
      else
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(-num + this._cornerSize, y - this._cornerSize * 0.5f), new Vector2(num - this._cornerSize, y - this._cornerSize * 0.5f), ref cDisplayClass210);
    }
    if (this._leftLine)
    {
      if ((double) this._leftGap > 0.0)
      {
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), y - this._cornerSize), new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), this._leftGap * 0.5f), ref cDisplayClass210);
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), -y + this._cornerSize), new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), (float) (-(double) this._leftGap * 0.5)), ref cDisplayClass210);
      }
      else
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), y - this._cornerSize), new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), -y + this._cornerSize), ref cDisplayClass210);
    }
    if (this._rightLine)
    {
      if ((double) this._rightGap > 0.0)
      {
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize * 0.5f, y - this._cornerSize), new Vector2(num - this._cornerSize * 0.5f, this._rightGap * 0.5f), ref cDisplayClass210);
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize * 0.5f, -y + this._cornerSize), new Vector2(num - this._cornerSize * 0.5f, (float) (-(double) this._rightGap * 0.5)), ref cDisplayClass210);
      }
      else
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize * 0.5f, y - this._cornerSize), new Vector2(num - this._cornerSize * 0.5f, -y + this._cornerSize), ref cDisplayClass210);
    }
    if (this._bottomLine)
    {
      if ((double) this._bottomGap > 0.0)
      {
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(-num + this._cornerSize, (float) (-(double) y + (double) this._cornerSize * 0.5)), new Vector2((float) (-(double) this._bottomGap * 0.5), (float) (-(double) y + (double) this._cornerSize * 0.5)), ref cDisplayClass210);
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize, (float) (-(double) y + (double) this._cornerSize * 0.5)), new Vector2(this._bottomGap * 0.5f, (float) (-(double) y + (double) this._cornerSize * 0.5)), ref cDisplayClass210);
      }
      else
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(-num + this._cornerSize, (float) (-(double) y + (double) this._cornerSize * 0.5)), new Vector2(num - this._cornerSize, (float) (-(double) y + (double) this._cornerSize * 0.5)), ref cDisplayClass210);
    }
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int idx0 = 0; idx0 <= vh.currentVertCount - 4; idx0 += 4)
    {
      vh.AddTriangle(idx0, idx0 + 1, idx0 + 3);
      vh.AddTriangle(idx0 + 3, idx0 + 1, idx0 + 2);
    }
  }

  [CompilerGenerated]
  public void \u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(
    Vector2 squarePosition,
    Vector2 squareUV,
    [In] ref InfoCardOutlineRenderer.\u003C\u003Ec__DisplayClass21_0 obj2)
  {
    // ISSUE: reference to a compiler-generated field
    obj2.position = squarePosition;
    // ISSUE: reference to a compiler-generated field
    obj2.uv = squareUV;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.vert.position = (Vector3) obj2.position;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.vert.uv0 = (Vector4) obj2.uv;
    // ISSUE: reference to a compiler-generated field
    this._verts.Add(obj2.vert);
    // ISSUE: reference to a compiler-generated field
    obj2.position.x += this._cornerSize;
    // ISSUE: reference to a compiler-generated field
    obj2.uv.x += 0.25f;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.vert.position = (Vector3) obj2.position;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.vert.uv0 = (Vector4) obj2.uv;
    // ISSUE: reference to a compiler-generated field
    this._verts.Add(obj2.vert);
    // ISSUE: reference to a compiler-generated field
    obj2.position.y -= this._cornerSize;
    // ISSUE: reference to a compiler-generated field
    obj2.uv.y -= 0.25f;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.vert.position = (Vector3) obj2.position;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.vert.uv0 = (Vector4) obj2.uv;
    // ISSUE: reference to a compiler-generated field
    this._verts.Add(obj2.vert);
    // ISSUE: reference to a compiler-generated field
    obj2.position.x -= this._cornerSize;
    // ISSUE: reference to a compiler-generated field
    obj2.uv.x -= 0.25f;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.vert.position = (Vector3) obj2.position;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.vert.uv0 = (Vector4) obj2.uv;
    // ISSUE: reference to a compiler-generated field
    this._verts.Add(obj2.vert);
  }

  [CompilerGenerated]
  public void \u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(
    Vector2 from,
    Vector2 to,
    [In] ref InfoCardOutlineRenderer.\u003C\u003Ec__DisplayClass21_0 obj2)
  {
    // ISSUE: reference to a compiler-generated field
    obj2.length = Vector2.Distance(from, to);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.capSizeNormalized = this._capSize / obj2.length;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.uvLength = obj2.length / ((float) this._texture.width / obj2.scale);
    // ISSUE: reference to a compiler-generated field
    obj2.angle = Mathf.Atan2(to.y - from.y, to.x - from.x);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.left.x = Mathf.Cos(obj2.angle + obj2.halfPI);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.left.y = Mathf.Sin(obj2.angle + obj2.halfPI);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.right.x = Mathf.Cos(obj2.angle - obj2.halfPI);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj2.right.y = Mathf.Sin(obj2.angle - obj2.halfPI);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003COnPopulateMesh\u003Eg__Add\u007C21_2(from, Vector2.Lerp(from, to, obj2.capSizeNormalized), new Vector2[4]
    {
      new Vector2(0.0f, 0.25f),
      new Vector2(obj2.capSizeNormalized, 0.25f),
      new Vector2(obj2.capSizeNormalized, 0.0f),
      new Vector2(0.0f, 0.0f)
    }, new Color[4]
    {
      obj2.colorTransparent,
      this.color,
      this.color,
      obj2.colorTransparent
    }, ref obj2);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003COnPopulateMesh\u003Eg__Add\u007C21_2(Vector2.Lerp(from, to, obj2.capSizeNormalized), Vector2.Lerp(from, to, 1f - obj2.capSizeNormalized), new Vector2[4]
    {
      new Vector2(obj2.capSizeNormalized, 0.25f),
      new Vector2(obj2.uvLength - obj2.capSizeNormalized * 2f, 0.25f),
      new Vector2(obj2.uvLength - obj2.capSizeNormalized * 2f, 0.0f),
      new Vector2(obj2.capSizeNormalized, 0.0f)
    }, new Color[4]
    {
      this.color,
      this.color,
      this.color,
      this.color
    }, ref obj2);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.\u003COnPopulateMesh\u003Eg__Add\u007C21_2(Vector2.Lerp(from, to, 1f - obj2.capSizeNormalized), to, new Vector2[4]
    {
      new Vector2(obj2.uvLength - obj2.capSizeNormalized * 2f, 0.25f),
      new Vector2(obj2.uvLength, 0.25f),
      new Vector2(obj2.uvLength, 0.0f),
      new Vector2(obj2.uvLength - obj2.capSizeNormalized * 2f, 0.0f)
    }, new Color[4]
    {
      this.color,
      obj2.colorTransparent,
      obj2.colorTransparent,
      this.color
    }, ref obj2);
  }

  [CompilerGenerated]
  public void \u003COnPopulateMesh\u003Eg__Add\u007C21_2(
    Vector2 addFrom,
    Vector2 addTo,
    Vector2[] uvs,
    Color[] colors,
    [In] ref InfoCardOutlineRenderer.\u003C\u003Ec__DisplayClass21_0 obj4)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj4.vert.position = (Vector3) (addFrom + obj4.left * (this._cornerSize * 0.5f));
    // ISSUE: reference to a compiler-generated field
    obj4.vert.uv0 = (Vector4) uvs[0];
    // ISSUE: reference to a compiler-generated field
    obj4.vert.color = (Color32) colors[0];
    // ISSUE: reference to a compiler-generated field
    this._verts.Add(obj4.vert);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj4.vert.position = (Vector3) (addTo + obj4.left * (this._cornerSize * 0.5f));
    // ISSUE: reference to a compiler-generated field
    obj4.vert.uv0 = (Vector4) uvs[1];
    // ISSUE: reference to a compiler-generated field
    obj4.vert.color = (Color32) colors[1];
    // ISSUE: reference to a compiler-generated field
    this._verts.Add(obj4.vert);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj4.vert.position = (Vector3) (addTo + obj4.right * (this._cornerSize * 0.5f));
    // ISSUE: reference to a compiler-generated field
    obj4.vert.uv0 = (Vector4) uvs[2];
    // ISSUE: reference to a compiler-generated field
    obj4.vert.color = (Color32) colors[2];
    // ISSUE: reference to a compiler-generated field
    this._verts.Add(obj4.vert);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    obj4.vert.position = (Vector3) (addFrom + obj4.right * (this._cornerSize * 0.5f));
    // ISSUE: reference to a compiler-generated field
    obj4.vert.uv0 = (Vector4) uvs[3];
    // ISSUE: reference to a compiler-generated field
    obj4.vert.color = (Color32) colors[3];
    // ISSUE: reference to a compiler-generated field
    this._verts.Add(obj4.vert);
  }
}
