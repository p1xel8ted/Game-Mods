// Decompiled with JetBrains decompiler
// Type: UIFont
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Font")]
public class UIFont : MonoBehaviour
{
  [HideInInspector]
  [SerializeField]
  public Material mMat;
  [HideInInspector]
  [SerializeField]
  public Rect mUVRect = new Rect(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  [HideInInspector]
  public BMFont mFont = new BMFont();
  [SerializeField]
  [HideInInspector]
  public UIAtlas mAtlas;
  [SerializeField]
  [HideInInspector]
  public UIFont mReplacement;
  [SerializeField]
  [HideInInspector]
  public List<BMSymbol> mSymbols = new List<BMSymbol>();
  [SerializeField]
  [HideInInspector]
  public Font mDynamicFont;
  [HideInInspector]
  [SerializeField]
  public int mDynamicFontSize = 16 /*0x10*/;
  [SerializeField]
  [HideInInspector]
  public FontStyle mDynamicFontStyle;
  [NonSerialized]
  public UISpriteData mSprite;
  public int mPMA = -1;
  public int mPacked = -1;
  public int shift_y;

  public BMFont bmFont
  {
    get => !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? this.mFont : this.mReplacement.bmFont;
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        this.mReplacement.bmFont = value;
      else
        this.mFont = value;
    }
  }

  public int texWidth
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.texWidth;
      return this.mFont == null ? 1 : this.mFont.texWidth;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.texWidth = value;
      }
      else
      {
        if (this.mFont == null)
          return;
        this.mFont.texWidth = value;
      }
    }
  }

  public int texHeight
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.texHeight;
      return this.mFont == null ? 1 : this.mFont.texHeight;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.texHeight = value;
      }
      else
      {
        if (this.mFont == null)
          return;
        this.mFont.texHeight = value;
      }
    }
  }

  public bool hasSymbols
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.hasSymbols;
      return this.mSymbols != null && this.mSymbols.Count != 0;
    }
  }

  public List<BMSymbol> symbols
  {
    get
    {
      return !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? this.mSymbols : this.mReplacement.symbols;
    }
  }

  public UIAtlas atlas
  {
    get => !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? this.mAtlas : this.mReplacement.atlas;
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.atlas = value;
      }
      else
      {
        if (!((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) value))
          return;
        this.mPMA = -1;
        this.mAtlas = value;
        if ((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null)
        {
          this.mMat = this.mAtlas.spriteMaterial;
          if (this.sprite != null)
            this.mUVRect = this.uvRect;
        }
        this.MarkAsChanged();
      }
    }
  }

  public Material material
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.material;
      if ((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null)
        return this.mAtlas.spriteMaterial;
      if ((UnityEngine.Object) this.mMat != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.mDynamicFont != (UnityEngine.Object) null && (UnityEngine.Object) this.mMat != (UnityEngine.Object) this.mDynamicFont.material)
          this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
        return this.mMat;
      }
      return (UnityEngine.Object) this.mDynamicFont != (UnityEngine.Object) null ? this.mDynamicFont.material : (Material) null;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.material = value;
      }
      else
      {
        if (!((UnityEngine.Object) this.mMat != (UnityEngine.Object) value))
          return;
        this.mPMA = -1;
        this.mMat = value;
        this.MarkAsChanged();
      }
    }
  }

  [Obsolete("Use UIFont.premultipliedAlphaShader instead")]
  public bool premultipliedAlpha => this.premultipliedAlphaShader;

  public bool premultipliedAlphaShader
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.premultipliedAlphaShader;
      if ((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null)
        return this.mAtlas.premultipliedAlpha;
      if (this.mPMA == -1)
      {
        Material material = this.material;
        this.mPMA = !((UnityEngine.Object) material != (UnityEngine.Object) null) || !((UnityEngine.Object) material.shader != (UnityEngine.Object) null) || !material.shader.name.Contains("Premultiplied") ? 0 : 1;
      }
      return this.mPMA == 1;
    }
  }

  public bool packedFontShader
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.packedFontShader;
      if ((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null)
        return false;
      if (this.mPacked == -1)
      {
        Material material = this.material;
        this.mPacked = !((UnityEngine.Object) material != (UnityEngine.Object) null) || !((UnityEngine.Object) material.shader != (UnityEngine.Object) null) || !material.shader.name.Contains("Packed") ? 0 : 1;
      }
      return this.mPacked == 1;
    }
  }

  public Texture2D texture
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.texture;
      Material material = this.material;
      return !((UnityEngine.Object) material != (UnityEngine.Object) null) ? (Texture2D) null : material.mainTexture as Texture2D;
    }
  }

  public Rect uvRect
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.uvRect;
      return !((UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null) || this.sprite == null ? new Rect(0.0f, 0.0f, 1f, 1f) : this.mUVRect;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.uvRect = value;
      }
      else
      {
        if (this.sprite != null || !(this.mUVRect != value))
          return;
        this.mUVRect = value;
        this.MarkAsChanged();
      }
    }
  }

  public string spriteName
  {
    get
    {
      return !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? this.mFont.spriteName : this.mReplacement.spriteName;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.spriteName = value;
      }
      else
      {
        if (!(this.mFont.spriteName != value))
          return;
        this.mFont.spriteName = value;
        this.MarkAsChanged();
      }
    }
  }

  public bool isValid => (UnityEngine.Object) this.mDynamicFont != (UnityEngine.Object) null || this.mFont.isValid;

  [Obsolete("Use UIFont.defaultSize instead")]
  public int size
  {
    get => this.defaultSize;
    set => this.defaultSize = value;
  }

  public int defaultSize
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.defaultSize;
      return this.isDynamic || this.mFont == null ? this.mDynamicFontSize : this.mFont.charSize;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        this.mReplacement.defaultSize = value;
      else
        this.mDynamicFontSize = value;
    }
  }

  public UISpriteData sprite
  {
    get
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        return this.mReplacement.sprite;
      if (this.mSprite == null && (UnityEngine.Object) this.mAtlas != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.mFont.spriteName))
      {
        this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
        if (this.mSprite == null)
          this.mSprite = this.mAtlas.GetSprite(this.name);
        if (this.mSprite == null)
          this.mFont.spriteName = (string) null;
        else
          this.UpdateUVRect();
        int index = 0;
        for (int count = this.mSymbols.Count; index < count; ++index)
          this.symbols[index].MarkAsChanged();
      }
      return this.mSprite;
    }
  }

  public UIFont replacement
  {
    get => this.mReplacement;
    set
    {
      UIFont uiFont = value;
      if ((UnityEngine.Object) uiFont == (UnityEngine.Object) this)
        uiFont = (UIFont) null;
      if (!((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) uiFont))
        return;
      if ((UnityEngine.Object) uiFont != (UnityEngine.Object) null && (UnityEngine.Object) uiFont.replacement == (UnityEngine.Object) this)
        uiFont.replacement = (UIFont) null;
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
        this.MarkAsChanged();
      this.mReplacement = uiFont;
      if ((UnityEngine.Object) uiFont != (UnityEngine.Object) null)
      {
        this.mPMA = -1;
        this.mMat = (Material) null;
        this.mFont = (BMFont) null;
        this.mDynamicFont = (Font) null;
      }
      this.MarkAsChanged();
    }
  }

  public bool isDynamic
  {
    get
    {
      return !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? (UnityEngine.Object) this.mDynamicFont != (UnityEngine.Object) null : this.mReplacement.isDynamic;
    }
  }

  public Font dynamicFont
  {
    get
    {
      return !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? this.mDynamicFont : this.mReplacement.dynamicFont;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.dynamicFont = value;
      }
      else
      {
        if (!((UnityEngine.Object) this.mDynamicFont != (UnityEngine.Object) value))
          return;
        if ((UnityEngine.Object) this.mDynamicFont != (UnityEngine.Object) null)
          this.material = (Material) null;
        this.mDynamicFont = value;
        this.MarkAsChanged();
      }
    }
  }

  public FontStyle dynamicFontStyle
  {
    get
    {
      return !((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null) ? this.mDynamicFontStyle : this.mReplacement.dynamicFontStyle;
    }
    set
    {
      if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      {
        this.mReplacement.dynamicFontStyle = value;
      }
      else
      {
        if (this.mDynamicFontStyle == value)
          return;
        this.mDynamicFontStyle = value;
        this.MarkAsChanged();
      }
    }
  }

  public void Trim()
  {
    if (!((UnityEngine.Object) this.mAtlas.texture != (UnityEngine.Object) null) || this.mSprite == null)
      return;
    Rect pixels = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
    Rect rect = new Rect((float) this.mSprite.x, (float) this.mSprite.y, (float) this.mSprite.width, (float) this.mSprite.height);
    this.mFont.Trim(Mathf.RoundToInt(rect.xMin - pixels.xMin), Mathf.RoundToInt(rect.yMin - pixels.yMin), Mathf.RoundToInt(rect.xMax - pixels.xMin), Mathf.RoundToInt(rect.yMax - pixels.yMin));
  }

  public bool References(UIFont font)
  {
    if ((UnityEngine.Object) font == (UnityEngine.Object) null)
      return false;
    if ((UnityEngine.Object) font == (UnityEngine.Object) this)
      return true;
    return (UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null && this.mReplacement.References(font);
  }

  public static bool CheckIfRelated(UIFont a, UIFont b)
  {
    if ((UnityEngine.Object) a == (UnityEngine.Object) null || (UnityEngine.Object) b == (UnityEngine.Object) null)
      return false;
    return a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0] || (UnityEngine.Object) a == (UnityEngine.Object) b || a.References(b) || b.References(a);
  }

  public Texture dynamicTexture
  {
    get
    {
      if ((bool) (UnityEngine.Object) this.mReplacement)
        return this.mReplacement.dynamicTexture;
      return this.isDynamic ? this.mDynamicFont.material.mainTexture : (Texture) null;
    }
  }

  public void MarkAsChanged()
  {
    if ((UnityEngine.Object) this.mReplacement != (UnityEngine.Object) null)
      this.mReplacement.MarkAsChanged();
    this.mSprite = (UISpriteData) null;
    UILabel[] active = NGUITools.FindActive<UILabel>();
    int index1 = 0;
    for (int length = active.Length; index1 < length; ++index1)
    {
      UILabel uiLabel = active[index1];
      if (uiLabel.enabled && NGUITools.GetActive(uiLabel.gameObject) && UIFont.CheckIfRelated(this, uiLabel.bitmapFont))
      {
        UIFont bitmapFont = uiLabel.bitmapFont;
        uiLabel.bitmapFont = (UIFont) null;
        uiLabel.bitmapFont = bitmapFont;
      }
    }
    int index2 = 0;
    for (int count = this.symbols.Count; index2 < count; ++index2)
      this.symbols[index2].MarkAsChanged();
  }

  public void UpdateUVRect()
  {
    if ((UnityEngine.Object) this.mAtlas == (UnityEngine.Object) null)
      return;
    Texture texture = this.mAtlas.texture;
    if (!((UnityEngine.Object) texture != (UnityEngine.Object) null))
      return;
    this.mUVRect = new Rect((float) (this.mSprite.x - this.mSprite.paddingLeft), (float) (this.mSprite.y - this.mSprite.paddingTop), (float) (this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight), (float) (this.mSprite.height + this.mSprite.paddingTop + this.mSprite.paddingBottom));
    this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
    if (!this.mSprite.hasPadding)
      return;
    this.Trim();
  }

  public BMSymbol GetSymbol(string sequence, bool createIfMissing)
  {
    int index = 0;
    for (int count = this.mSymbols.Count; index < count; ++index)
    {
      BMSymbol mSymbol = this.mSymbols[index];
      if (mSymbol.sequence == sequence)
        return mSymbol;
    }
    if (!createIfMissing)
      return (BMSymbol) null;
    BMSymbol symbol = new BMSymbol();
    symbol.sequence = sequence;
    this.mSymbols.Add(symbol);
    return symbol;
  }

  public BMSymbol MatchSymbol(string text, int offset, int textLength)
  {
    int count = this.mSymbols.Count;
    if (count == 0)
      return (BMSymbol) null;
    textLength -= offset;
    for (int index1 = 0; index1 < count; ++index1)
    {
      BMSymbol mSymbol = this.mSymbols[index1];
      int length = mSymbol.length;
      if (length != 0 && textLength >= length)
      {
        bool flag = true;
        for (int index2 = 0; index2 < length; ++index2)
        {
          if ((int) text[offset + index2] != (int) mSymbol.sequence[index2])
          {
            flag = false;
            break;
          }
        }
        if (flag && mSymbol.Validate(this.atlas))
          return mSymbol;
      }
    }
    return (BMSymbol) null;
  }

  public void AddSymbol(string sequence, string spriteName)
  {
    this.GetSymbol(sequence, true).spriteName = spriteName;
    this.MarkAsChanged();
  }

  public void RemoveSymbol(string sequence)
  {
    BMSymbol symbol = this.GetSymbol(sequence, false);
    if (symbol != null)
      this.symbols.Remove(symbol);
    this.MarkAsChanged();
  }

  public void RenameSymbol(string before, string after)
  {
    BMSymbol symbol = this.GetSymbol(before, false);
    if (symbol != null)
      symbol.sequence = after;
    this.MarkAsChanged();
  }

  public bool UsesSprite(string s)
  {
    if (!string.IsNullOrEmpty(s))
    {
      if (s.Equals(this.spriteName))
        return true;
      int index = 0;
      for (int count = this.symbols.Count; index < count; ++index)
      {
        BMSymbol symbol = this.symbols[index];
        if (s.Equals(symbol.spriteName))
          return true;
      }
    }
    return false;
  }
}
