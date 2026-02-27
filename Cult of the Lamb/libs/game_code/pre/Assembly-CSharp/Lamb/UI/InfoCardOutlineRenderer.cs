// Decompiled with JetBrains decompiler
// Type: Lamb.UI.InfoCardOutlineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[RequireComponent(typeof (CanvasRenderer))]
public class InfoCardOutlineRenderer : MaskableGraphic
{
  [SerializeField]
  private Texture _texture;
  [Header("Outline Properties")]
  [SerializeField]
  private bool _showBadge = true;
  [SerializeField]
  [Range(0.0f, 7f)]
  private int _badgeVariant;
  [SerializeField]
  [Range(1f, 100f)]
  private float _cornerSize = 100f;
  [SerializeField]
  [Range(1f, 100f)]
  private float _capSize = 10f;
  [SerializeField]
  [Range(-100f, 100f)]
  private float _offset;
  [Header("Lines")]
  [SerializeField]
  private bool _topLine = true;
  [SerializeField]
  private bool _bottomLine = true;
  [SerializeField]
  private bool _leftLine = true;
  [SerializeField]
  private bool _rightLine = true;
  [Header("Gaps")]
  [SerializeField]
  [Range(0.0f, 250f)]
  private float _topGap;
  [SerializeField]
  [Range(0.0f, 250f)]
  private float _leftGap;
  [SerializeField]
  [Range(0.0f, 250f)]
  private float _rightGap;
  [SerializeField]
  [Range(0.0f, 250f)]
  private float _bottomGap;
  private List<UIVertex> _verts = new List<UIVertex>();

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

  protected override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    this.SetVerticesDirty();
    this.SetMaterialDirty();
  }

  protected override void OnPopulateMesh(VertexHelper vh)
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
    // ISSUE: reference to a compiler-generated method
    this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2(-num, y), new Vector2(0.0f, 0.5f), ref cDisplayClass210);
    // ISSUE: reference to a compiler-generated method
    this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2(num - this._cornerSize, y), new Vector2(0.25f, 0.5f), ref cDisplayClass210);
    // ISSUE: reference to a compiler-generated method
    this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2(num - this._cornerSize, -y + this._cornerSize), new Vector2(0.5f, 0.5f), ref cDisplayClass210);
    // ISSUE: reference to a compiler-generated method
    this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2(-num, -y + this._cornerSize), new Vector2(0.75f, 0.5f), ref cDisplayClass210);
    if (this._topLine)
    {
      if (this._showBadge)
      {
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddSquare\u007C21_0(new Vector2((float) (-(double) this._cornerSize * 0.5), y), new Vector2(0.25f * (float) this._badgeVariant - Mathf.Floor((float) this._badgeVariant / 4f), (float) (1.0 - 0.25 * (double) Mathf.Floor((float) this._badgeVariant / 4f))), ref cDisplayClass210);
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(-num + this._cornerSize, y - this._cornerSize * 0.5f), new Vector2((float) (-(double) this._cornerSize * 0.5), y - this._cornerSize * 0.5f), ref cDisplayClass210);
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize, y - this._cornerSize * 0.5f), new Vector2(this._cornerSize * 0.5f, y - this._cornerSize * 0.5f), ref cDisplayClass210);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(-num + this._cornerSize, y - this._cornerSize * 0.5f), new Vector2(num - this._cornerSize, y - this._cornerSize * 0.5f), ref cDisplayClass210);
      }
    }
    if (this._leftLine)
    {
      if ((double) this._leftGap > 0.0)
      {
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), y - this._cornerSize), new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), this._leftGap * 0.5f), ref cDisplayClass210);
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), -y + this._cornerSize), new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), (float) (-(double) this._leftGap * 0.5)), ref cDisplayClass210);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), y - this._cornerSize), new Vector2((float) (-(double) num + (double) this._cornerSize * 0.5), -y + this._cornerSize), ref cDisplayClass210);
      }
    }
    if (this._rightLine)
    {
      if ((double) this._rightGap > 0.0)
      {
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize * 0.5f, y - this._cornerSize), new Vector2(num - this._cornerSize * 0.5f, this._rightGap * 0.5f), ref cDisplayClass210);
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize * 0.5f, -y + this._cornerSize), new Vector2(num - this._cornerSize * 0.5f, (float) (-(double) this._rightGap * 0.5)), ref cDisplayClass210);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize * 0.5f, y - this._cornerSize), new Vector2(num - this._cornerSize * 0.5f, -y + this._cornerSize), ref cDisplayClass210);
      }
    }
    if (this._bottomLine)
    {
      if ((double) this._bottomGap > 0.0)
      {
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(-num + this._cornerSize, (float) (-(double) y + (double) this._cornerSize * 0.5)), new Vector2((float) (-(double) this._bottomGap * 0.5), (float) (-(double) y + (double) this._cornerSize * 0.5)), ref cDisplayClass210);
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(num - this._cornerSize, (float) (-(double) y + (double) this._cornerSize * 0.5)), new Vector2(this._bottomGap * 0.5f, (float) (-(double) y + (double) this._cornerSize * 0.5)), ref cDisplayClass210);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.\u003COnPopulateMesh\u003Eg__AddLine\u007C21_1(new Vector2(-num + this._cornerSize, (float) (-(double) y + (double) this._cornerSize * 0.5)), new Vector2(num - this._cornerSize, (float) (-(double) y + (double) this._cornerSize * 0.5)), ref cDisplayClass210);
      }
    }
    foreach (UIVertex vert in this._verts)
      vh.AddVert(vert);
    for (int idx0 = 0; idx0 <= vh.currentVertCount - 4; idx0 += 4)
    {
      vh.AddTriangle(idx0, idx0 + 1, idx0 + 3);
      vh.AddTriangle(idx0 + 3, idx0 + 1, idx0 + 2);
    }
  }
}
