// Decompiled with JetBrains decompiler
// Type: UISprite
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Sprite")]
public class UISprite : UIBasicSprite
{
  [SerializeField]
  [HideInInspector]
  public UIAtlas mAtlas;
  [SerializeField]
  [HideInInspector]
  public string mSpriteName;
  [HideInInspector]
  [SerializeField]
  public bool mFillCenter = true;
  [NonSerialized]
  public UISpriteData mSprite;
  [NonSerialized]
  public bool mSpriteSet;

  public override Texture mainTexture
  {
    get
    {
      Material spriteMaterial = (UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null ? this.mAtlas.spriteMaterial : (Material) null;
      return !((UnityEngine.Object) spriteMaterial != (UnityEngine.Object) null) ? (Texture) null : spriteMaterial.mainTexture;
    }
    set => base.mainTexture = value;
  }

  public override Material material
  {
    get
    {
      Material material = base.material;
      if ((UnityEngine.Object) material != (UnityEngine.Object) null)
        return material;
      return !((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null) ? (Material) null : this.mAtlas.spriteMaterial;
    }
    set => base.material = value;
  }

  public UIAtlas atlas
  {
    get => this.mAtlas;
    set
    {
      if (!((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) value))
        return;
      this.RemoveFromPanel();
      this.mAtlas = value;
      this.mSpriteSet = false;
      this.mSprite = (UISpriteData) null;
      if (string.IsNullOrEmpty(this.mSpriteName) && (UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null && this.mAtlas.spriteList.Count > 0)
      {
        this.SetAtlasSprite(this.mAtlas.spriteList[0]);
        this.mSpriteName = this.mSprite.name;
      }
      if (string.IsNullOrEmpty(this.mSpriteName))
        return;
      string mSpriteName = this.mSpriteName;
      this.mSpriteName = "";
      this.spriteName = mSpriteName;
      this.MarkAsChanged();
    }
  }

  public string spriteName
  {
    get => this.mSpriteName;
    set
    {
      if (string.IsNullOrEmpty(value))
      {
        if (string.IsNullOrEmpty(this.mSpriteName))
          return;
        this.mSpriteName = "";
        this.mSprite = (UISpriteData) null;
        this.mChanged = true;
        this.mSpriteSet = false;
      }
      else
      {
        if (!(this.mSpriteName != value))
          return;
        this.mSpriteName = value;
        this.mSprite = (UISpriteData) null;
        this.mChanged = true;
        this.mSpriteSet = false;
      }
    }
  }

  public bool isValid => this.GetAtlasSprite() != null;

  [Obsolete("Use 'centerType' instead")]
  public bool fillCenter
  {
    get => this.centerType != 0;
    set
    {
      if (value == (this.centerType != 0))
        return;
      this.centerType = value ? UIBasicSprite.AdvancedType.Sliced : UIBasicSprite.AdvancedType.Invisible;
      this.MarkAsChanged();
    }
  }

  public bool applyGradient
  {
    get => this.mApplyGradient;
    set
    {
      if (this.mApplyGradient == value)
        return;
      this.mApplyGradient = value;
      this.MarkAsChanged();
    }
  }

  public Color gradientTop
  {
    get => this.mGradientTop;
    set
    {
      if (!(this.mGradientTop != value))
        return;
      this.mGradientTop = value;
      if (!this.mApplyGradient)
        return;
      this.MarkAsChanged();
    }
  }

  public Color gradientBottom
  {
    get => this.mGradientBottom;
    set
    {
      if (!(this.mGradientBottom != value))
        return;
      this.mGradientBottom = value;
      if (!this.mApplyGradient)
        return;
      this.MarkAsChanged();
    }
  }

  public override Vector4 border
  {
    get
    {
      UISpriteData atlasSprite = this.GetAtlasSprite();
      return atlasSprite == null ? base.border : new Vector4((float) atlasSprite.borderLeft, (float) atlasSprite.borderBottom, (float) atlasSprite.borderRight, (float) atlasSprite.borderTop);
    }
  }

  public override float pixelSize
  {
    get => !((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null) ? 1f : this.mAtlas.pixelSize;
  }

  public override int minWidth
  {
    get
    {
      if (this.type != UIBasicSprite.Type.Sliced && this.type != UIBasicSprite.Type.Advanced)
        return base.minWidth;
      float pixelSize = this.pixelSize;
      Vector4 vector4 = this.border * this.pixelSize;
      int num = Mathf.RoundToInt(vector4.x + vector4.z);
      UISpriteData atlasSprite = this.GetAtlasSprite();
      if (atlasSprite != null)
        num += Mathf.RoundToInt(pixelSize * (float) (atlasSprite.paddingLeft + atlasSprite.paddingRight));
      return Mathf.Max(base.minWidth, (num & 1) == 1 ? num + 1 : num);
    }
  }

  public override int minHeight
  {
    get
    {
      if (this.type != UIBasicSprite.Type.Sliced && this.type != UIBasicSprite.Type.Advanced)
        return base.minHeight;
      float pixelSize = this.pixelSize;
      Vector4 vector4 = this.border * this.pixelSize;
      int num = Mathf.RoundToInt(vector4.y + vector4.w);
      UISpriteData atlasSprite = this.GetAtlasSprite();
      if (atlasSprite != null)
        num += Mathf.RoundToInt(pixelSize * (float) (atlasSprite.paddingTop + atlasSprite.paddingBottom));
      return Mathf.Max(base.minHeight, (num & 1) == 1 ? num + 1 : num);
    }
  }

  public override Vector4 drawingDimensions
  {
    get
    {
      Vector2 pivotOffset = this.pivotOffset;
      float a1 = -pivotOffset.x * (float) this.mWidth;
      float a2 = -pivotOffset.y * (float) this.mHeight;
      float b1 = a1 + (float) this.mWidth;
      float b2 = a2 + (float) this.mHeight;
      if (this.GetAtlasSprite() != null && this.mType != UIBasicSprite.Type.Tiled)
      {
        int paddingLeft = this.mSprite.paddingLeft;
        int paddingBottom = this.mSprite.paddingBottom;
        int paddingRight = this.mSprite.paddingRight;
        int paddingTop = this.mSprite.paddingTop;
        if (this.mType != UIBasicSprite.Type.Simple)
        {
          float pixelSize = this.pixelSize;
          if ((double) pixelSize != 1.0)
          {
            paddingLeft = Mathf.RoundToInt(pixelSize * (float) paddingLeft);
            paddingBottom = Mathf.RoundToInt(pixelSize * (float) paddingBottom);
            paddingRight = Mathf.RoundToInt(pixelSize * (float) paddingRight);
            paddingTop = Mathf.RoundToInt(pixelSize * (float) paddingTop);
          }
        }
        int num1 = this.mSprite.width + paddingLeft + paddingRight;
        int num2 = this.mSprite.height + paddingBottom + paddingTop;
        float num3 = 1f;
        float num4 = 1f;
        if (num1 > 0 && num2 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
        {
          if ((num1 & 1) != 0)
            ++paddingRight;
          if ((num2 & 1) != 0)
            ++paddingTop;
          num3 = 1f / (float) num1 * (float) this.mWidth;
          num4 = 1f / (float) num2 * (float) this.mHeight;
        }
        if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
        {
          a1 += (float) paddingRight * num3;
          b1 -= (float) paddingLeft * num3;
        }
        else
        {
          a1 += (float) paddingLeft * num3;
          b1 -= (float) paddingRight * num3;
        }
        if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
        {
          a2 += (float) paddingTop * num4;
          b2 -= (float) paddingBottom * num4;
        }
        else
        {
          a2 += (float) paddingBottom * num4;
          b2 -= (float) paddingTop * num4;
        }
      }
      Vector4 vector4 = (UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null ? this.border * this.pixelSize : Vector4.zero;
      float num5 = vector4.x + vector4.z;
      float num6 = vector4.y + vector4.w;
      double x = (double) Mathf.Lerp(a1, b1 - num5, this.mDrawRegion.x);
      float num7 = Mathf.Lerp(a2, b2 - num6, this.mDrawRegion.y);
      float num8 = Mathf.Lerp(a1 + num5, b1, this.mDrawRegion.z);
      float num9 = Mathf.Lerp(a2 + num6, b2, this.mDrawRegion.w);
      double y = (double) num7;
      double z = (double) num8;
      double w = (double) num9;
      return new Vector4((float) x, (float) y, (float) z, (float) w);
    }
  }

  public override bool premultipliedAlpha
  {
    get => (UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null && this.mAtlas.premultipliedAlpha;
  }

  public UISpriteData GetAtlasSprite()
  {
    if (!this.mSpriteSet)
      this.mSprite = (UISpriteData) null;
    if (this.mSprite == null && (UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null)
    {
      if (!string.IsNullOrEmpty(this.mSpriteName))
      {
        UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
        if (sprite == null)
          return (UISpriteData) null;
        this.SetAtlasSprite(sprite);
      }
      if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
      {
        UISpriteData sprite = this.mAtlas.spriteList[0];
        if (sprite == null)
          return (UISpriteData) null;
        this.SetAtlasSprite(sprite);
        if (this.mSprite == null)
        {
          Debug.LogError((object) (this.mAtlas.name + " seems to have a null sprite!"));
          return (UISpriteData) null;
        }
        this.mSpriteName = this.mSprite.name;
      }
    }
    return this.mSprite;
  }

  public void SetAtlasSprite(UISpriteData sp)
  {
    this.mChanged = true;
    this.mSpriteSet = true;
    if (sp != null)
    {
      this.mSprite = sp;
      this.mSpriteName = this.mSprite.name;
    }
    else
    {
      this.mSpriteName = this.mSprite != null ? this.mSprite.name : "";
      this.mSprite = sp;
    }
  }

  public override void MakePixelPerfect()
  {
    if (!this.isValid)
      return;
    base.MakePixelPerfect();
    if (this.mType == UIBasicSprite.Type.Tiled)
      return;
    UISpriteData atlasSprite = this.GetAtlasSprite();
    if (atlasSprite == null)
      return;
    Texture mainTexture = this.mainTexture;
    if ((UnityEngine.Object) mainTexture == (UnityEngine.Object) null || this.mType != UIBasicSprite.Type.Simple && this.mType != UIBasicSprite.Type.Filled && atlasSprite.hasBorder || !((UnityEngine.Object) mainTexture != (UnityEngine.Object) null))
      return;
    int num1 = Mathf.RoundToInt(this.pixelSize * (float) (atlasSprite.width + atlasSprite.paddingLeft + atlasSprite.paddingRight));
    int num2 = Mathf.RoundToInt(this.pixelSize * (float) (atlasSprite.height + atlasSprite.paddingTop + atlasSprite.paddingBottom));
    if ((num1 & 1) == 1)
      ++num1;
    if ((num2 & 1) == 1)
      ++num2;
    this.width = num1;
    this.height = num2;
  }

  public override void OnInit()
  {
    if (!this.mFillCenter)
    {
      this.mFillCenter = true;
      this.centerType = UIBasicSprite.AdvancedType.Invisible;
    }
    base.OnInit();
  }

  public override void OnUpdate()
  {
    base.OnUpdate();
    if (!this.mChanged && this.mSpriteSet)
      return;
    this.mSpriteSet = true;
    this.mSprite = (UISpriteData) null;
    this.mChanged = true;
  }

  public override void OnFill(List<Vector3> verts, List<Vector2> uvs, List<Color> cols)
  {
    Texture mainTexture = this.mainTexture;
    if ((UnityEngine.Object) mainTexture == (UnityEngine.Object) null)
      return;
    if (this.mSprite == null)
      this.mSprite = this.atlas.GetSprite(this.spriteName);
    if (this.mSprite == null)
      return;
    Rect rect1 = new Rect((float) this.mSprite.x, (float) this.mSprite.y, (float) this.mSprite.width, (float) this.mSprite.height);
    Rect rect2 = new Rect((float) (this.mSprite.x + this.mSprite.borderLeft), (float) (this.mSprite.y + this.mSprite.borderTop), (float) (this.mSprite.width - this.mSprite.borderLeft - this.mSprite.borderRight), (float) (this.mSprite.height - this.mSprite.borderBottom - this.mSprite.borderTop));
    Rect texCoords1 = NGUIMath.ConvertToTexCoords(rect1, mainTexture.width, mainTexture.height);
    Rect texCoords2 = NGUIMath.ConvertToTexCoords(rect2, mainTexture.width, mainTexture.height);
    int count = verts.Count;
    this.Fill(verts, uvs, cols, texCoords1, texCoords2);
    if (this.onPostFill == null)
      return;
    this.onPostFill((UIWidget) this, count, verts, uvs, cols);
  }
}
