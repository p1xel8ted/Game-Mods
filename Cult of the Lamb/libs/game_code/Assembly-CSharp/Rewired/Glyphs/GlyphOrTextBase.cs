// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.GlyphOrTextBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

public abstract class GlyphOrTextBase : MonoBehaviour
{
  public abstract string textString { get; set; }

  public abstract void ShowText(string text);

  public abstract void ShowGlyph(object glyph);

  public virtual void Hide() => this.Hide(GlyphOrTextBase.TypeFlags.All);

  public abstract void Hide(GlyphOrTextBase.TypeFlags flags);

  [Flags]
  public enum TypeFlags
  {
    None = 0,
    Glyph = 1,
    Text = 2,
    All = -1, // 0xFFFFFFFF
  }
}
