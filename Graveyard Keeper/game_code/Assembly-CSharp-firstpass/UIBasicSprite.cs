// Decompiled with JetBrains decompiler
// Type: UIBasicSprite
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public abstract class UIBasicSprite : UIWidget
{
  [SerializeField]
  [HideInInspector]
  public UIBasicSprite.Type mType;
  [HideInInspector]
  [SerializeField]
  public UIBasicSprite.FillDirection mFillDirection = UIBasicSprite.FillDirection.Radial360;
  [SerializeField]
  [Range(0.0f, 1f)]
  [HideInInspector]
  public float mFillAmount = 1f;
  [SerializeField]
  [HideInInspector]
  public bool mInvert;
  [SerializeField]
  [HideInInspector]
  public UIBasicSprite.Flip mFlip;
  [SerializeField]
  [HideInInspector]
  public bool mApplyGradient;
  [HideInInspector]
  [SerializeField]
  public Color mGradientTop = Color.white;
  [HideInInspector]
  [SerializeField]
  public Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);
  [NonSerialized]
  public Rect mInnerUV;
  [NonSerialized]
  public Rect mOuterUV;
  public UIBasicSprite.AdvancedType centerType = UIBasicSprite.AdvancedType.Sliced;
  public UIBasicSprite.AdvancedType leftType = UIBasicSprite.AdvancedType.Sliced;
  public UIBasicSprite.AdvancedType rightType = UIBasicSprite.AdvancedType.Sliced;
  public UIBasicSprite.AdvancedType bottomType = UIBasicSprite.AdvancedType.Sliced;
  public UIBasicSprite.AdvancedType topType = UIBasicSprite.AdvancedType.Sliced;
  public static Vector2[] mTempPos = new Vector2[4];
  public static Vector2[] mTempUVs = new Vector2[4];

  public virtual UIBasicSprite.Type type
  {
    get => this.mType;
    set
    {
      if (this.mType == value)
        return;
      this.mType = value;
      this.MarkAsChanged();
    }
  }

  public UIBasicSprite.Flip flip
  {
    get => this.mFlip;
    set
    {
      if (this.mFlip == value)
        return;
      this.mFlip = value;
      this.MarkAsChanged();
    }
  }

  public UIBasicSprite.FillDirection fillDirection
  {
    get => this.mFillDirection;
    set
    {
      if (this.mFillDirection == value)
        return;
      this.mFillDirection = value;
      this.mChanged = true;
    }
  }

  public float fillAmount
  {
    get => this.mFillAmount;
    set
    {
      float num = Mathf.Clamp01(value);
      if ((double) this.mFillAmount == (double) num)
        return;
      this.mFillAmount = num;
      this.mChanged = true;
    }
  }

  public override int minWidth
  {
    get
    {
      if (this.type != UIBasicSprite.Type.Sliced && this.type != UIBasicSprite.Type.Advanced)
        return base.minWidth;
      Vector4 vector4 = this.border * this.pixelSize;
      int num = Mathf.RoundToInt(vector4.x + vector4.z);
      return Mathf.Max(base.minWidth, (num & 1) == 1 ? num + 1 : num);
    }
  }

  public override int minHeight
  {
    get
    {
      if (this.type != UIBasicSprite.Type.Sliced && this.type != UIBasicSprite.Type.Advanced)
        return base.minHeight;
      Vector4 vector4 = this.border * this.pixelSize;
      int num = Mathf.RoundToInt(vector4.y + vector4.w);
      return Mathf.Max(base.minHeight, (num & 1) == 1 ? num + 1 : num);
    }
  }

  public bool invert
  {
    get => this.mInvert;
    set
    {
      if (this.mInvert == value)
        return;
      this.mInvert = value;
      this.mChanged = true;
    }
  }

  public bool hasBorder
  {
    get
    {
      Vector4 border = this.border;
      return (double) border.x != 0.0 || (double) border.y != 0.0 || (double) border.z != 0.0 || (double) border.w != 0.0;
    }
  }

  public virtual bool premultipliedAlpha => false;

  public virtual float pixelSize => 1f;

  public Vector4 drawingUVs
  {
    get
    {
      switch (this.mFlip)
      {
        case UIBasicSprite.Flip.Horizontally:
          return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMin, this.mOuterUV.xMin, this.mOuterUV.yMax);
        case UIBasicSprite.Flip.Vertically:
          return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMax, this.mOuterUV.xMax, this.mOuterUV.yMin);
        case UIBasicSprite.Flip.Both:
          return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMax, this.mOuterUV.xMin, this.mOuterUV.yMin);
        default:
          return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMin, this.mOuterUV.xMax, this.mOuterUV.yMax);
      }
    }
  }

  public Color drawingColor
  {
    get
    {
      Color c = this.color with { a = this.finalAlpha };
      if (this.premultipliedAlpha)
        c = NGUITools.ApplyPMA(c);
      return c;
    }
  }

  public void Fill(
    List<Vector3> verts,
    List<Vector2> uvs,
    List<Color> cols,
    Rect outer,
    Rect inner)
  {
    this.mOuterUV = outer;
    this.mInnerUV = inner;
    switch (this.type)
    {
      case UIBasicSprite.Type.Simple:
        this.SimpleFill(verts, uvs, cols);
        break;
      case UIBasicSprite.Type.Sliced:
        this.SlicedFill(verts, uvs, cols);
        break;
      case UIBasicSprite.Type.Tiled:
        this.TiledFill(verts, uvs, cols);
        break;
      case UIBasicSprite.Type.Filled:
        this.FilledFill(verts, uvs, cols);
        break;
      case UIBasicSprite.Type.Advanced:
        this.AdvancedFill(verts, uvs, cols);
        break;
    }
  }

  public void SimpleFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
  {
    Vector4 drawingDimensions = this.drawingDimensions;
    Vector4 drawingUvs = this.drawingUVs;
    Color drawingColor = this.drawingColor;
    verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
    verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
    verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
    verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
    uvs.Add(new Vector2(drawingUvs.x, drawingUvs.y));
    uvs.Add(new Vector2(drawingUvs.x, drawingUvs.w));
    uvs.Add(new Vector2(drawingUvs.z, drawingUvs.w));
    uvs.Add(new Vector2(drawingUvs.z, drawingUvs.y));
    if (!this.mApplyGradient)
    {
      cols.Add(drawingColor);
      cols.Add(drawingColor);
      cols.Add(drawingColor);
      cols.Add(drawingColor);
    }
    else
    {
      this.AddVertexColours(cols, ref drawingColor, 1, 1);
      this.AddVertexColours(cols, ref drawingColor, 1, 2);
      this.AddVertexColours(cols, ref drawingColor, 2, 2);
      this.AddVertexColours(cols, ref drawingColor, 2, 1);
    }
  }

  public void SlicedFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
  {
    Vector4 vector4 = this.border * this.pixelSize;
    if ((double) vector4.x == 0.0 && (double) vector4.y == 0.0 && (double) vector4.z == 0.0 && (double) vector4.w == 0.0)
    {
      this.SimpleFill(verts, uvs, cols);
    }
    else
    {
      Color drawingColor = this.drawingColor;
      Vector4 drawingDimensions = this.drawingDimensions;
      UIBasicSprite.mTempPos[0].x = drawingDimensions.x;
      UIBasicSprite.mTempPos[0].y = drawingDimensions.y;
      UIBasicSprite.mTempPos[3].x = drawingDimensions.z;
      UIBasicSprite.mTempPos[3].y = drawingDimensions.w;
      if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
      {
        UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector4.z;
        UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector4.x;
        UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
        UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
        UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
        UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
      }
      else
      {
        UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector4.x;
        UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector4.z;
        UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
        UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
        UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
        UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
      }
      if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
      {
        UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector4.w;
        UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector4.y;
        UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
        UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
        UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
        UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
      }
      else
      {
        UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector4.y;
        UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector4.w;
        UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
        UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
        UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
        UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
      }
      for (int x1 = 0; x1 < 3; ++x1)
      {
        int x2 = x1 + 1;
        for (int y1 = 0; y1 < 3; ++y1)
        {
          if (this.centerType != UIBasicSprite.AdvancedType.Invisible || x1 != 1 || y1 != 1)
          {
            int y2 = y1 + 1;
            verts.Add(new Vector3(UIBasicSprite.mTempPos[x1].x, UIBasicSprite.mTempPos[y1].y));
            verts.Add(new Vector3(UIBasicSprite.mTempPos[x1].x, UIBasicSprite.mTempPos[y2].y));
            verts.Add(new Vector3(UIBasicSprite.mTempPos[x2].x, UIBasicSprite.mTempPos[y2].y));
            verts.Add(new Vector3(UIBasicSprite.mTempPos[x2].x, UIBasicSprite.mTempPos[y1].y));
            uvs.Add(new Vector2(UIBasicSprite.mTempUVs[x1].x, UIBasicSprite.mTempUVs[y1].y));
            uvs.Add(new Vector2(UIBasicSprite.mTempUVs[x1].x, UIBasicSprite.mTempUVs[y2].y));
            uvs.Add(new Vector2(UIBasicSprite.mTempUVs[x2].x, UIBasicSprite.mTempUVs[y2].y));
            uvs.Add(new Vector2(UIBasicSprite.mTempUVs[x2].x, UIBasicSprite.mTempUVs[y1].y));
            if (!this.mApplyGradient)
            {
              cols.Add(drawingColor);
              cols.Add(drawingColor);
              cols.Add(drawingColor);
              cols.Add(drawingColor);
            }
            else
            {
              this.AddVertexColours(cols, ref drawingColor, x1, y1);
              this.AddVertexColours(cols, ref drawingColor, x1, y2);
              this.AddVertexColours(cols, ref drawingColor, x2, y2);
              this.AddVertexColours(cols, ref drawingColor, x2, y1);
            }
          }
        }
      }
    }
  }

  [DebuggerHidden]
  [DebuggerStepThrough]
  public void AddVertexColours(List<Color> cols, ref Color color, int x, int y)
  {
    switch (y)
    {
      case 0:
      case 1:
        cols.Add(color * this.mGradientBottom);
        break;
      case 2:
      case 3:
        cols.Add(color * this.mGradientTop);
        break;
    }
  }

  public void TiledFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
  {
    Texture mainTexture = this.mainTexture;
    if ((UnityEngine.Object) mainTexture == (UnityEngine.Object) null)
      return;
    Vector2 vector2 = new Vector2(this.mInnerUV.width * (float) mainTexture.width, this.mInnerUV.height * (float) mainTexture.height) * this.pixelSize;
    if ((UnityEngine.Object) mainTexture == (UnityEngine.Object) null || (double) vector2.x < 2.0 || (double) vector2.y < 2.0)
      return;
    Color drawingColor = this.drawingColor;
    Vector4 drawingDimensions = this.drawingDimensions;
    Vector4 vector4;
    if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
    {
      vector4.x = this.mInnerUV.xMax;
      vector4.z = this.mInnerUV.xMin;
    }
    else
    {
      vector4.x = this.mInnerUV.xMin;
      vector4.z = this.mInnerUV.xMax;
    }
    if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
    {
      vector4.y = this.mInnerUV.yMax;
      vector4.w = this.mInnerUV.yMin;
    }
    else
    {
      vector4.y = this.mInnerUV.yMin;
      vector4.w = this.mInnerUV.yMax;
    }
    float x1 = drawingDimensions.x;
    float y1 = drawingDimensions.y;
    float x2 = vector4.x;
    float y2 = vector4.y;
    for (; (double) y1 < (double) drawingDimensions.w; y1 += vector2.y)
    {
      float x3 = drawingDimensions.x;
      float y3 = y1 + vector2.y;
      float y4 = vector4.w;
      if ((double) y3 > (double) drawingDimensions.w)
      {
        y4 = Mathf.Lerp(vector4.y, vector4.w, (drawingDimensions.w - y1) / vector2.y);
        y3 = drawingDimensions.w;
      }
      for (; (double) x3 < (double) drawingDimensions.z; x3 += vector2.x)
      {
        float x4 = x3 + vector2.x;
        float x5 = vector4.z;
        if ((double) x4 > (double) drawingDimensions.z)
        {
          x5 = Mathf.Lerp(vector4.x, vector4.z, (drawingDimensions.z - x3) / vector2.x);
          x4 = drawingDimensions.z;
        }
        verts.Add(new Vector3(x3, y1));
        verts.Add(new Vector3(x3, y3));
        verts.Add(new Vector3(x4, y3));
        verts.Add(new Vector3(x4, y1));
        uvs.Add(new Vector2(x2, y2));
        uvs.Add(new Vector2(x2, y4));
        uvs.Add(new Vector2(x5, y4));
        uvs.Add(new Vector2(x5, y2));
        cols.Add(drawingColor);
        cols.Add(drawingColor);
        cols.Add(drawingColor);
        cols.Add(drawingColor);
      }
    }
  }

  public void FilledFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
  {
    if ((double) this.mFillAmount < 1.0 / 1000.0)
      return;
    Vector4 drawingDimensions = this.drawingDimensions;
    Vector4 drawingUvs = this.drawingUVs;
    Color drawingColor = this.drawingColor;
    if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal || this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
    {
      if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal)
      {
        float num = (drawingUvs.z - drawingUvs.x) * this.mFillAmount;
        if (this.mInvert)
        {
          drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
          drawingUvs.x = drawingUvs.z - num;
        }
        else
        {
          drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
          drawingUvs.z = drawingUvs.x + num;
        }
      }
      else if (this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
      {
        float num = (drawingUvs.w - drawingUvs.y) * this.mFillAmount;
        if (this.mInvert)
        {
          drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
          drawingUvs.y = drawingUvs.w - num;
        }
        else
        {
          drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
          drawingUvs.w = drawingUvs.y + num;
        }
      }
    }
    UIBasicSprite.mTempPos[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
    UIBasicSprite.mTempPos[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
    UIBasicSprite.mTempPos[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
    UIBasicSprite.mTempPos[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
    UIBasicSprite.mTempUVs[0] = new Vector2(drawingUvs.x, drawingUvs.y);
    UIBasicSprite.mTempUVs[1] = new Vector2(drawingUvs.x, drawingUvs.w);
    UIBasicSprite.mTempUVs[2] = new Vector2(drawingUvs.z, drawingUvs.w);
    UIBasicSprite.mTempUVs[3] = new Vector2(drawingUvs.z, drawingUvs.y);
    if ((double) this.mFillAmount < 1.0)
    {
      if (this.mFillDirection == UIBasicSprite.FillDirection.Radial90)
      {
        if (!UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, this.mFillAmount, this.mInvert, 0))
          return;
        for (int index = 0; index < 4; ++index)
        {
          verts.Add((Vector3) UIBasicSprite.mTempPos[index]);
          uvs.Add(UIBasicSprite.mTempUVs[index]);
          cols.Add(drawingColor);
        }
        return;
      }
      if (this.mFillDirection == UIBasicSprite.FillDirection.Radial180)
      {
        for (int index1 = 0; index1 < 2; ++index1)
        {
          float t1 = 0.0f;
          float t2 = 1f;
          float t3;
          float t4;
          if (index1 == 0)
          {
            t3 = 0.0f;
            t4 = 0.5f;
          }
          else
          {
            t3 = 0.5f;
            t4 = 1f;
          }
          UIBasicSprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
          UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
          UIBasicSprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
          UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
          UIBasicSprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t1);
          UIBasicSprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
          UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
          UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
          UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(drawingUvs.x, drawingUvs.z, t3);
          UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
          UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(drawingUvs.x, drawingUvs.z, t4);
          UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
          UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(drawingUvs.y, drawingUvs.w, t1);
          UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(drawingUvs.y, drawingUvs.w, t2);
          UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
          UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
          float num = !this.mInvert ? this.fillAmount * 2f - (float) index1 : this.mFillAmount * 2f - (float) (1 - index1);
          if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(num), !this.mInvert, NGUIMath.RepeatIndex(index1 + 3, 4)))
          {
            for (int index2 = 0; index2 < 4; ++index2)
            {
              verts.Add((Vector3) UIBasicSprite.mTempPos[index2]);
              uvs.Add(UIBasicSprite.mTempUVs[index2]);
              cols.Add(drawingColor);
            }
          }
        }
        return;
      }
      if (this.mFillDirection == UIBasicSprite.FillDirection.Radial360)
      {
        for (int index3 = 0; index3 < 4; ++index3)
        {
          float t5;
          float t6;
          if (index3 < 2)
          {
            t5 = 0.0f;
            t6 = 0.5f;
          }
          else
          {
            t5 = 0.5f;
            t6 = 1f;
          }
          float t7;
          float t8;
          if (index3 == 0 || index3 == 3)
          {
            t7 = 0.0f;
            t8 = 0.5f;
          }
          else
          {
            t7 = 0.5f;
            t8 = 1f;
          }
          UIBasicSprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t5);
          UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
          UIBasicSprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t6);
          UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
          UIBasicSprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t7);
          UIBasicSprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t8);
          UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
          UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
          UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(drawingUvs.x, drawingUvs.z, t5);
          UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
          UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(drawingUvs.x, drawingUvs.z, t6);
          UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
          UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(drawingUvs.y, drawingUvs.w, t7);
          UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(drawingUvs.y, drawingUvs.w, t8);
          UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
          UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
          float num = this.mInvert ? this.mFillAmount * 4f - (float) NGUIMath.RepeatIndex(index3 + 2, 4) : this.mFillAmount * 4f - (float) (3 - NGUIMath.RepeatIndex(index3 + 2, 4));
          if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(num), this.mInvert, NGUIMath.RepeatIndex(index3 + 2, 4)))
          {
            for (int index4 = 0; index4 < 4; ++index4)
            {
              verts.Add((Vector3) UIBasicSprite.mTempPos[index4]);
              uvs.Add(UIBasicSprite.mTempUVs[index4]);
              cols.Add(drawingColor);
            }
          }
        }
        return;
      }
    }
    for (int index = 0; index < 4; ++index)
    {
      verts.Add((Vector3) UIBasicSprite.mTempPos[index]);
      uvs.Add(UIBasicSprite.mTempUVs[index]);
      cols.Add(drawingColor);
    }
  }

  public void AdvancedFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
  {
    Texture mainTexture = this.mainTexture;
    if ((UnityEngine.Object) mainTexture == (UnityEngine.Object) null)
      return;
    Vector4 vector4 = this.border * this.pixelSize;
    if ((double) vector4.x == 0.0 && (double) vector4.y == 0.0 && (double) vector4.z == 0.0 && (double) vector4.w == 0.0)
    {
      this.SimpleFill(verts, uvs, cols);
    }
    else
    {
      Color drawingColor = this.drawingColor;
      Vector4 drawingDimensions = this.drawingDimensions;
      Vector2 vector2 = new Vector2(this.mInnerUV.width * (float) mainTexture.width, this.mInnerUV.height * (float) mainTexture.height);
      vector2 *= this.pixelSize;
      if ((double) vector2.x < 1.0)
        vector2.x = 1f;
      if ((double) vector2.y < 1.0)
        vector2.y = 1f;
      UIBasicSprite.mTempPos[0].x = drawingDimensions.x;
      UIBasicSprite.mTempPos[0].y = drawingDimensions.y;
      UIBasicSprite.mTempPos[3].x = drawingDimensions.z;
      UIBasicSprite.mTempPos[3].y = drawingDimensions.w;
      if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
      {
        UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector4.z;
        UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector4.x;
        UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
        UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
        UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
        UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
      }
      else
      {
        UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector4.x;
        UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector4.z;
        UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
        UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
        UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
        UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
      }
      if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
      {
        UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector4.w;
        UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector4.y;
        UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
        UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
        UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
        UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
      }
      else
      {
        UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector4.y;
        UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector4.w;
        UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
        UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
        UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
        UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
      }
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int index2 = index1 + 1;
        for (int index3 = 0; index3 < 3; ++index3)
        {
          if (this.centerType != UIBasicSprite.AdvancedType.Invisible || index1 != 1 || index3 != 1)
          {
            int index4 = index3 + 1;
            if (index1 == 1 && index3 == 1)
            {
              if (this.centerType == UIBasicSprite.AdvancedType.Tiled)
              {
                float x1 = UIBasicSprite.mTempPos[index1].x;
                float x2 = UIBasicSprite.mTempPos[index2].x;
                double y1 = (double) UIBasicSprite.mTempPos[index3].y;
                float y2 = UIBasicSprite.mTempPos[index4].y;
                float x3 = UIBasicSprite.mTempUVs[index1].x;
                float y3 = UIBasicSprite.mTempUVs[index3].y;
                for (float v0y = (float) y1; (double) v0y < (double) y2; v0y += vector2.y)
                {
                  float v0x = x1;
                  float num1 = UIBasicSprite.mTempUVs[index4].y;
                  float v1y = v0y + vector2.y;
                  if ((double) v1y > (double) y2)
                  {
                    num1 = Mathf.Lerp(y3, num1, (y2 - v0y) / vector2.y);
                    v1y = y2;
                  }
                  for (; (double) v0x < (double) x2; v0x += vector2.x)
                  {
                    float v1x = v0x + vector2.x;
                    float num2 = UIBasicSprite.mTempUVs[index2].x;
                    if ((double) v1x > (double) x2)
                    {
                      num2 = Mathf.Lerp(x3, num2, (x2 - v0x) / vector2.x);
                      v1x = x2;
                    }
                    UIBasicSprite.Fill(verts, uvs, cols, v0x, v1x, v0y, v1y, x3, num2, y3, num1, drawingColor);
                  }
                }
              }
              else if (this.centerType == UIBasicSprite.AdvancedType.Sliced)
                UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[index1].x, UIBasicSprite.mTempPos[index2].x, UIBasicSprite.mTempPos[index3].y, UIBasicSprite.mTempPos[index4].y, UIBasicSprite.mTempUVs[index1].x, UIBasicSprite.mTempUVs[index2].x, UIBasicSprite.mTempUVs[index3].y, UIBasicSprite.mTempUVs[index4].y, drawingColor);
            }
            else if (index1 == 1)
            {
              if (index3 == 0 && this.bottomType == UIBasicSprite.AdvancedType.Tiled || index3 == 2 && this.topType == UIBasicSprite.AdvancedType.Tiled)
              {
                double x4 = (double) UIBasicSprite.mTempPos[index1].x;
                float x5 = UIBasicSprite.mTempPos[index2].x;
                float y4 = UIBasicSprite.mTempPos[index3].y;
                float y5 = UIBasicSprite.mTempPos[index4].y;
                float x6 = UIBasicSprite.mTempUVs[index1].x;
                float y6 = UIBasicSprite.mTempUVs[index3].y;
                float y7 = UIBasicSprite.mTempUVs[index4].y;
                for (float v0x = (float) x4; (double) v0x < (double) x5; v0x += vector2.x)
                {
                  float v1x = v0x + vector2.x;
                  float num = UIBasicSprite.mTempUVs[index2].x;
                  if ((double) v1x > (double) x5)
                  {
                    num = Mathf.Lerp(x6, num, (x5 - v0x) / vector2.x);
                    v1x = x5;
                  }
                  UIBasicSprite.Fill(verts, uvs, cols, v0x, v1x, y4, y5, x6, num, y6, y7, drawingColor);
                }
              }
              else if (index3 == 0 && this.bottomType != UIBasicSprite.AdvancedType.Invisible || index3 == 2 && this.topType != UIBasicSprite.AdvancedType.Invisible)
                UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[index1].x, UIBasicSprite.mTempPos[index2].x, UIBasicSprite.mTempPos[index3].y, UIBasicSprite.mTempPos[index4].y, UIBasicSprite.mTempUVs[index1].x, UIBasicSprite.mTempUVs[index2].x, UIBasicSprite.mTempUVs[index3].y, UIBasicSprite.mTempUVs[index4].y, drawingColor);
            }
            else if (index3 == 1)
            {
              if (index1 == 0 && this.leftType == UIBasicSprite.AdvancedType.Tiled || index1 == 2 && this.rightType == UIBasicSprite.AdvancedType.Tiled)
              {
                float x7 = UIBasicSprite.mTempPos[index1].x;
                float x8 = UIBasicSprite.mTempPos[index2].x;
                double y8 = (double) UIBasicSprite.mTempPos[index3].y;
                float y9 = UIBasicSprite.mTempPos[index4].y;
                float x9 = UIBasicSprite.mTempUVs[index1].x;
                float x10 = UIBasicSprite.mTempUVs[index2].x;
                float y10 = UIBasicSprite.mTempUVs[index3].y;
                for (float v0y = (float) y8; (double) v0y < (double) y9; v0y += vector2.y)
                {
                  float num = UIBasicSprite.mTempUVs[index4].y;
                  float v1y = v0y + vector2.y;
                  if ((double) v1y > (double) y9)
                  {
                    num = Mathf.Lerp(y10, num, (y9 - v0y) / vector2.y);
                    v1y = y9;
                  }
                  UIBasicSprite.Fill(verts, uvs, cols, x7, x8, v0y, v1y, x9, x10, y10, num, drawingColor);
                }
              }
              else if (index1 == 0 && this.leftType != UIBasicSprite.AdvancedType.Invisible || index1 == 2 && this.rightType != UIBasicSprite.AdvancedType.Invisible)
                UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[index1].x, UIBasicSprite.mTempPos[index2].x, UIBasicSprite.mTempPos[index3].y, UIBasicSprite.mTempPos[index4].y, UIBasicSprite.mTempUVs[index1].x, UIBasicSprite.mTempUVs[index2].x, UIBasicSprite.mTempUVs[index3].y, UIBasicSprite.mTempUVs[index4].y, drawingColor);
            }
            else if ((index3 != 0 || this.bottomType != UIBasicSprite.AdvancedType.Invisible) && (index3 != 2 || this.topType != UIBasicSprite.AdvancedType.Invisible) && (index1 != 0 || this.leftType != UIBasicSprite.AdvancedType.Invisible) && (index1 != 2 || this.rightType != UIBasicSprite.AdvancedType.Invisible))
              UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[index1].x, UIBasicSprite.mTempPos[index2].x, UIBasicSprite.mTempPos[index3].y, UIBasicSprite.mTempPos[index4].y, UIBasicSprite.mTempUVs[index1].x, UIBasicSprite.mTempUVs[index2].x, UIBasicSprite.mTempUVs[index3].y, UIBasicSprite.mTempUVs[index4].y, drawingColor);
          }
        }
      }
    }
  }

  public static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
  {
    if ((double) fill < 1.0 / 1000.0)
      return false;
    if ((corner & 1) == 1)
      invert = !invert;
    if (!invert && (double) fill > 0.99900001287460327)
      return true;
    float num = Mathf.Clamp01(fill);
    if (invert)
      num = 1f - num;
    float f = num * 1.57079637f;
    float cos = Mathf.Cos(f);
    float sin = Mathf.Sin(f);
    UIBasicSprite.RadialCut(xy, cos, sin, invert, corner);
    UIBasicSprite.RadialCut(uv, cos, sin, invert, corner);
    return true;
  }

  public static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
  {
    int index1 = corner;
    int index2 = NGUIMath.RepeatIndex(corner + 1, 4);
    int index3 = NGUIMath.RepeatIndex(corner + 2, 4);
    int index4 = NGUIMath.RepeatIndex(corner + 3, 4);
    if ((corner & 1) == 1)
    {
      if ((double) sin > (double) cos)
      {
        cos /= sin;
        sin = 1f;
        if (invert)
        {
          xy[index2].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
          xy[index3].x = xy[index2].x;
        }
      }
      else if ((double) cos > (double) sin)
      {
        sin /= cos;
        cos = 1f;
        if (!invert)
        {
          xy[index3].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
          xy[index4].y = xy[index3].y;
        }
      }
      else
      {
        cos = 1f;
        sin = 1f;
      }
      if (!invert)
        xy[index4].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
      else
        xy[index2].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
    }
    else
    {
      if ((double) cos > (double) sin)
      {
        sin /= cos;
        cos = 1f;
        if (!invert)
        {
          xy[index2].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
          xy[index3].y = xy[index2].y;
        }
      }
      else if ((double) sin > (double) cos)
      {
        cos /= sin;
        sin = 1f;
        if (invert)
        {
          xy[index3].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
          xy[index4].x = xy[index3].x;
        }
      }
      else
      {
        cos = 1f;
        sin = 1f;
      }
      if (invert)
        xy[index4].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
      else
        xy[index2].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
    }
  }

  public static void Fill(
    List<Vector3> verts,
    List<Vector2> uvs,
    List<Color> cols,
    float v0x,
    float v1x,
    float v0y,
    float v1y,
    float u0x,
    float u1x,
    float u0y,
    float u1y,
    Color col)
  {
    verts.Add(new Vector3(v0x, v0y));
    verts.Add(new Vector3(v0x, v1y));
    verts.Add(new Vector3(v1x, v1y));
    verts.Add(new Vector3(v1x, v0y));
    uvs.Add(new Vector2(u0x, u0y));
    uvs.Add(new Vector2(u0x, u1y));
    uvs.Add(new Vector2(u1x, u1y));
    uvs.Add(new Vector2(u1x, u0y));
    cols.Add(col);
    cols.Add(col);
    cols.Add(col);
    cols.Add(col);
  }

  public enum Type
  {
    Simple,
    Sliced,
    Tiled,
    Filled,
    Advanced,
  }

  public enum FillDirection
  {
    Horizontal,
    Vertical,
    Radial90,
    Radial180,
    Radial360,
  }

  public enum AdvancedType
  {
    Invisible,
    Sliced,
    Tiled,
  }

  public enum Flip
  {
    Nothing,
    Horizontally,
    Vertically,
    Both,
  }
}
