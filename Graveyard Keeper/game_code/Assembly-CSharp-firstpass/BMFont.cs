// Decompiled with JetBrains decompiler
// Type: BMFont
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class BMFont
{
  [HideInInspector]
  [SerializeField]
  public int mSize = 16 /*0x10*/;
  [HideInInspector]
  [SerializeField]
  public int mBase;
  [HideInInspector]
  [SerializeField]
  public int mWidth;
  [HideInInspector]
  [SerializeField]
  public int mHeight;
  [SerializeField]
  [HideInInspector]
  public string mSpriteName;
  [HideInInspector]
  [SerializeField]
  public List<BMGlyph> mSaved = new List<BMGlyph>();
  public Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();

  public bool isValid => this.mSaved.Count > 0;

  public int charSize
  {
    get => this.mSize;
    set => this.mSize = value;
  }

  public int baseOffset
  {
    get => this.mBase;
    set => this.mBase = value;
  }

  public int texWidth
  {
    get => this.mWidth;
    set => this.mWidth = value;
  }

  public int texHeight
  {
    get => this.mHeight;
    set => this.mHeight = value;
  }

  public int glyphCount => !this.isValid ? 0 : this.mSaved.Count;

  public string spriteName
  {
    get => this.mSpriteName;
    set => this.mSpriteName = value;
  }

  public List<BMGlyph> glyphs => this.mSaved;

  public BMGlyph GetGlyph(int index, bool createIfMissing)
  {
    BMGlyph glyph = (BMGlyph) null;
    if (this.mDict.Count == 0)
    {
      int index1 = 0;
      for (int count = this.mSaved.Count; index1 < count; ++index1)
      {
        BMGlyph bmGlyph = this.mSaved[index1];
        this.mDict.Add(bmGlyph.index, bmGlyph);
      }
    }
    if (!this.mDict.TryGetValue(index, out glyph) & createIfMissing)
    {
      glyph = new BMGlyph();
      glyph.index = index;
      this.mSaved.Add(glyph);
      this.mDict.Add(index, glyph);
    }
    return glyph;
  }

  public BMGlyph GetGlyph(int index) => this.GetGlyph(index, false);

  public void Clear()
  {
    this.mDict.Clear();
    this.mSaved.Clear();
  }

  public void Trim(int xMin, int yMin, int xMax, int yMax)
  {
    if (!this.isValid)
      return;
    int index = 0;
    for (int count = this.mSaved.Count; index < count; ++index)
      this.mSaved[index]?.Trim(xMin, yMin, xMax, yMax);
  }
}
