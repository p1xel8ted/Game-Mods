// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.SpriteGlyphSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

[Serializable]
public class SpriteGlyphSet : GlyphSet
{
  [Tooltip("The list of glyphs.")]
  [SerializeField]
  public List<SpriteGlyphSet.Entry> _glyphs;

  public List<SpriteGlyphSet.Entry> glyphs
  {
    get => this._glyphs;
    set => this._glyphs = value;
  }

  public override int glyphCount => this._glyphs == null ? 0 : this._glyphs.Count;

  public override GlyphSet.EntryBase GetEntry(int index)
  {
    if (this._glyphs == null)
      return (GlyphSet.EntryBase) null;
    if ((uint) index >= (uint) this._glyphs.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    return (GlyphSet.EntryBase) this._glyphs[index];
  }

  [Serializable]
  public class Entry : GlyphSet.EntryBase<Sprite>
  {
  }
}
