// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIGlyphOrText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

public class UnityUIGlyphOrText : GlyphOrTextBase<Image, Sprite, Text>
{
  public override string textString
  {
    get => !((Object) this.textComponent != (Object) null) ? string.Empty : this.textComponent.text;
    set
    {
      if ((Object) this.textComponent == (Object) null)
        return;
      this.textComponent.text = value;
    }
  }

  public override Sprite glyphGraphic
  {
    get
    {
      return !((Object) this.glyphComponent != (Object) null) ? (Sprite) null : this.glyphComponent.sprite;
    }
    set
    {
      if ((Object) this.glyphComponent == (Object) null)
        return;
      this.glyphComponent.sprite = value;
    }
  }
}
