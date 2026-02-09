// Decompiled with JetBrains decompiler
// Type: BMSymbol
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class BMSymbol
{
  public string sequence;
  public string spriteName;
  public UISpriteData mSprite;
  public bool mIsValid;
  public int mLength;
  public int mOffsetX;
  public int mOffsetY;
  public int mWidth;
  public int mHeight;
  public int mAdvance;
  public Rect mUV;

  public int length
  {
    get
    {
      if (this.mLength == 0)
        this.mLength = this.sequence.Length;
      return this.mLength;
    }
  }

  public int offsetX => this.mOffsetX;

  public int offsetY => this.mOffsetY;

  public int width => this.mWidth;

  public int height => this.mHeight;

  public int advance => this.mAdvance;

  public Rect uvRect => this.mUV;

  public void MarkAsChanged() => this.mIsValid = false;

  public bool Validate(UIAtlas atlas)
  {
    if ((UnityEngine.Object) atlas == (UnityEngine.Object) null)
      return false;
    if (!this.mIsValid)
    {
      if (string.IsNullOrEmpty(this.spriteName))
        return false;
      this.mSprite = (UnityEngine.Object) atlas != (UnityEngine.Object) null ? atlas.GetSprite(this.spriteName) : (UISpriteData) null;
      if (this.mSprite != null)
      {
        Texture texture = atlas.texture;
        if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
        {
          this.mSprite = (UISpriteData) null;
        }
        else
        {
          this.mUV = new Rect((float) this.mSprite.x, (float) this.mSprite.y, (float) this.mSprite.width, (float) this.mSprite.height);
          this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
          this.mOffsetX = this.mSprite.paddingLeft;
          this.mOffsetY = this.mSprite.paddingTop;
          this.mWidth = this.mSprite.width;
          this.mHeight = this.mSprite.height;
          this.mAdvance = this.mSprite.width + (this.mSprite.paddingLeft + this.mSprite.paddingRight);
          this.mIsValid = true;
        }
      }
    }
    return this.mSprite != null;
  }

  public BMSymbol Copy()
  {
    return new BMSymbol()
    {
      sequence = this.sequence,
      spriteName = this.spriteName,
      mSprite = this.mSprite,
      mLength = this.mLength,
      mOffsetX = this.mOffsetX,
      mOffsetY = this.mOffsetY,
      mWidth = this.mWidth,
      mHeight = this.mHeight,
      mAdvance = this.mAdvance
    };
  }
}
