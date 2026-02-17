// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIGlyphOrTextTMPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

public class UnityUIGlyphOrTextTMPro : GlyphOrTextBase<Image, Sprite, TMP_Text>
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
