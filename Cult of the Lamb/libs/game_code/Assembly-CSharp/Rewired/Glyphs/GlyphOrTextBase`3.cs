// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.GlyphOrTextBase`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

public abstract class GlyphOrTextBase<TGlyphComponent, TGlyphGraphic, TTextComponent> : 
  GlyphOrTextBase
  where TGlyphComponent : Behaviour
  where TGlyphGraphic : class
  where TTextComponent : Behaviour
{
  [SerializeField]
  public TTextComponent _textComponent;
  [SerializeField]
  public TGlyphComponent _glyphComponent;

  public TTextComponent textComponent
  {
    get => this._textComponent;
    set => this._textComponent = value;
  }

  public TGlyphComponent glyphComponent
  {
    get => this._glyphComponent;
    set => this._glyphComponent = value;
  }

  public abstract TGlyphGraphic glyphGraphic { get; set; }

  public override void ShowText(string text)
  {
    if ((UnityEngine.Object) this._textComponent == (UnityEngine.Object) null)
      return;
    if (!string.Equals(this.textString, text, StringComparison.Ordinal))
      this.textString = text;
    if (!this._textComponent.gameObject.activeSelf)
    {
      this._textComponent.gameObject.SetActive(true);
      if (!this.gameObject.activeSelf)
        this.gameObject.SetActive(true);
    }
    this.Hide(GlyphOrTextBase.TypeFlags.Glyph);
  }

  public override void ShowGlyph(object glyph)
  {
    switch (glyph)
    {
      case null:
      case TGlyphGraphic _:
        this.ShowGlyph((TGlyphGraphic) glyph);
        break;
      default:
        Debug.LogError((object) $"Rewired: Glyph does not implement {typeof (TGlyphGraphic).Name}.");
        break;
    }
  }

  public virtual void ShowGlyph(TGlyphGraphic glyph)
  {
    if ((UnityEngine.Object) this._glyphComponent == (UnityEngine.Object) null)
      return;
    if ((object) this.glyphGraphic != (object) glyph)
      this.glyphGraphic = glyph;
    if (!this._glyphComponent.gameObject.activeSelf)
    {
      this._glyphComponent.gameObject.SetActive(true);
      if (!this.gameObject.activeSelf)
        this.gameObject.SetActive(true);
    }
    this.Hide(GlyphOrTextBase.TypeFlags.Text);
  }

  public override void Hide(GlyphOrTextBase.TypeFlags flags)
  {
    if ((UnityEngine.Object) this._textComponent != (UnityEngine.Object) null && (flags & GlyphOrTextBase.TypeFlags.Text) != GlyphOrTextBase.TypeFlags.None && this._textComponent.gameObject.activeSelf)
      this._textComponent.gameObject.SetActive(false);
    if ((UnityEngine.Object) this._glyphComponent != (UnityEngine.Object) null && (flags & GlyphOrTextBase.TypeFlags.Glyph) != GlyphOrTextBase.TypeFlags.None && this._glyphComponent.gameObject.activeSelf)
      this._glyphComponent.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this._glyphComponent == (UnityEngine.Object) null) && this._glyphComponent.gameObject.activeSelf || !((UnityEngine.Object) this._textComponent == (UnityEngine.Object) null) && this._textComponent.gameObject.activeSelf)
      return;
    this.gameObject.SetActive(false);
  }
}
