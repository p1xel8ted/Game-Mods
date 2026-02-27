// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.GlyphOrTextBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
