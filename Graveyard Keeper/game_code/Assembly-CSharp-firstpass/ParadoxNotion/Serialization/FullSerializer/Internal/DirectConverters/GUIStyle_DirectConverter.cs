// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters.GUIStyle_DirectConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters;

public class GUIStyle_DirectConverter : fsDirectConverter<GUIStyle>
{
  public override fsResult DoSerialize(GUIStyle model, Dictionary<string, fsData> serialized)
  {
    return fsResult.Success + this.SerializeMember<GUIStyleState>(serialized, (System.Type) null, "active", model.active) + this.SerializeMember<TextAnchor>(serialized, (System.Type) null, "alignment", model.alignment) + this.SerializeMember<RectOffset>(serialized, (System.Type) null, "border", model.border) + this.SerializeMember<TextClipping>(serialized, (System.Type) null, "clipping", model.clipping) + this.SerializeMember<Vector2>(serialized, (System.Type) null, "contentOffset", model.contentOffset) + this.SerializeMember<float>(serialized, (System.Type) null, "fixedHeight", model.fixedHeight) + this.SerializeMember<float>(serialized, (System.Type) null, "fixedWidth", model.fixedWidth) + this.SerializeMember<GUIStyleState>(serialized, (System.Type) null, "focused", model.focused) + this.SerializeMember<Font>(serialized, (System.Type) null, "font", model.font) + this.SerializeMember<int>(serialized, (System.Type) null, "fontSize", model.fontSize) + this.SerializeMember<FontStyle>(serialized, (System.Type) null, "fontStyle", model.fontStyle) + this.SerializeMember<GUIStyleState>(serialized, (System.Type) null, "hover", model.hover) + this.SerializeMember<ImagePosition>(serialized, (System.Type) null, "imagePosition", model.imagePosition) + this.SerializeMember<RectOffset>(serialized, (System.Type) null, "margin", model.margin) + this.SerializeMember<string>(serialized, (System.Type) null, "name", model.name) + this.SerializeMember<GUIStyleState>(serialized, (System.Type) null, "normal", model.normal) + this.SerializeMember<GUIStyleState>(serialized, (System.Type) null, "onActive", model.onActive) + this.SerializeMember<GUIStyleState>(serialized, (System.Type) null, "onFocused", model.onFocused) + this.SerializeMember<GUIStyleState>(serialized, (System.Type) null, "onHover", model.onHover) + this.SerializeMember<GUIStyleState>(serialized, (System.Type) null, "onNormal", model.onNormal) + this.SerializeMember<RectOffset>(serialized, (System.Type) null, "overflow", model.overflow) + this.SerializeMember<RectOffset>(serialized, (System.Type) null, "padding", model.padding) + this.SerializeMember<bool>(serialized, (System.Type) null, "richText", model.richText) + this.SerializeMember<bool>(serialized, (System.Type) null, "stretchHeight", model.stretchHeight) + this.SerializeMember<bool>(serialized, (System.Type) null, "stretchWidth", model.stretchWidth) + this.SerializeMember<bool>(serialized, (System.Type) null, "wordWrap", model.wordWrap);
  }

  public override fsResult DoDeserialize(Dictionary<string, fsData> data, ref GUIStyle model)
  {
    fsResult success = fsResult.Success;
    GUIStyleState active = model.active;
    fsResult fsResult1 = this.DeserializeMember<GUIStyleState>(data, (System.Type) null, "active", out active);
    fsResult fsResult2 = success + fsResult1;
    model.active = active;
    TextAnchor alignment = model.alignment;
    fsResult fsResult3 = this.DeserializeMember<TextAnchor>(data, (System.Type) null, "alignment", out alignment);
    fsResult fsResult4 = fsResult2 + fsResult3;
    model.alignment = alignment;
    RectOffset border = model.border;
    fsResult fsResult5 = this.DeserializeMember<RectOffset>(data, (System.Type) null, "border", out border);
    fsResult fsResult6 = fsResult4 + fsResult5;
    model.border = border;
    TextClipping clipping = model.clipping;
    fsResult fsResult7 = this.DeserializeMember<TextClipping>(data, (System.Type) null, "clipping", out clipping);
    fsResult fsResult8 = fsResult6 + fsResult7;
    model.clipping = clipping;
    Vector2 contentOffset = model.contentOffset;
    fsResult fsResult9 = this.DeserializeMember<Vector2>(data, (System.Type) null, "contentOffset", out contentOffset);
    fsResult fsResult10 = fsResult8 + fsResult9;
    model.contentOffset = contentOffset;
    float fixedHeight = model.fixedHeight;
    fsResult fsResult11 = this.DeserializeMember<float>(data, (System.Type) null, "fixedHeight", out fixedHeight);
    fsResult fsResult12 = fsResult10 + fsResult11;
    model.fixedHeight = fixedHeight;
    float fixedWidth = model.fixedWidth;
    fsResult fsResult13 = this.DeserializeMember<float>(data, (System.Type) null, "fixedWidth", out fixedWidth);
    fsResult fsResult14 = fsResult12 + fsResult13;
    model.fixedWidth = fixedWidth;
    GUIStyleState focused = model.focused;
    fsResult fsResult15 = this.DeserializeMember<GUIStyleState>(data, (System.Type) null, "focused", out focused);
    fsResult fsResult16 = fsResult14 + fsResult15;
    model.focused = focused;
    Font font = model.font;
    fsResult fsResult17 = this.DeserializeMember<Font>(data, (System.Type) null, "font", out font);
    fsResult fsResult18 = fsResult16 + fsResult17;
    model.font = font;
    int fontSize = model.fontSize;
    fsResult fsResult19 = this.DeserializeMember<int>(data, (System.Type) null, "fontSize", out fontSize);
    fsResult fsResult20 = fsResult18 + fsResult19;
    model.fontSize = fontSize;
    FontStyle fontStyle = model.fontStyle;
    fsResult fsResult21 = this.DeserializeMember<FontStyle>(data, (System.Type) null, "fontStyle", out fontStyle);
    fsResult fsResult22 = fsResult20 + fsResult21;
    model.fontStyle = fontStyle;
    GUIStyleState hover = model.hover;
    fsResult fsResult23 = this.DeserializeMember<GUIStyleState>(data, (System.Type) null, "hover", out hover);
    fsResult fsResult24 = fsResult22 + fsResult23;
    model.hover = hover;
    ImagePosition imagePosition = model.imagePosition;
    fsResult fsResult25 = this.DeserializeMember<ImagePosition>(data, (System.Type) null, "imagePosition", out imagePosition);
    fsResult fsResult26 = fsResult24 + fsResult25;
    model.imagePosition = imagePosition;
    RectOffset margin = model.margin;
    fsResult fsResult27 = this.DeserializeMember<RectOffset>(data, (System.Type) null, "margin", out margin);
    fsResult fsResult28 = fsResult26 + fsResult27;
    model.margin = margin;
    string name = model.name;
    fsResult fsResult29 = this.DeserializeMember<string>(data, (System.Type) null, "name", out name);
    fsResult fsResult30 = fsResult28 + fsResult29;
    model.name = name;
    GUIStyleState normal = model.normal;
    fsResult fsResult31 = this.DeserializeMember<GUIStyleState>(data, (System.Type) null, "normal", out normal);
    fsResult fsResult32 = fsResult30 + fsResult31;
    model.normal = normal;
    GUIStyleState onActive = model.onActive;
    fsResult fsResult33 = this.DeserializeMember<GUIStyleState>(data, (System.Type) null, "onActive", out onActive);
    fsResult fsResult34 = fsResult32 + fsResult33;
    model.onActive = onActive;
    GUIStyleState onFocused = model.onFocused;
    fsResult fsResult35 = this.DeserializeMember<GUIStyleState>(data, (System.Type) null, "onFocused", out onFocused);
    fsResult fsResult36 = fsResult34 + fsResult35;
    model.onFocused = onFocused;
    GUIStyleState onHover = model.onHover;
    fsResult fsResult37 = this.DeserializeMember<GUIStyleState>(data, (System.Type) null, "onHover", out onHover);
    fsResult fsResult38 = fsResult36 + fsResult37;
    model.onHover = onHover;
    GUIStyleState onNormal = model.onNormal;
    fsResult fsResult39 = this.DeserializeMember<GUIStyleState>(data, (System.Type) null, "onNormal", out onNormal);
    fsResult fsResult40 = fsResult38 + fsResult39;
    model.onNormal = onNormal;
    RectOffset overflow = model.overflow;
    fsResult fsResult41 = this.DeserializeMember<RectOffset>(data, (System.Type) null, "overflow", out overflow);
    fsResult fsResult42 = fsResult40 + fsResult41;
    model.overflow = overflow;
    RectOffset padding = model.padding;
    fsResult fsResult43 = this.DeserializeMember<RectOffset>(data, (System.Type) null, "padding", out padding);
    fsResult fsResult44 = fsResult42 + fsResult43;
    model.padding = padding;
    bool richText = model.richText;
    fsResult fsResult45 = this.DeserializeMember<bool>(data, (System.Type) null, "richText", out richText);
    fsResult fsResult46 = fsResult44 + fsResult45;
    model.richText = richText;
    bool stretchHeight = model.stretchHeight;
    fsResult fsResult47 = this.DeserializeMember<bool>(data, (System.Type) null, "stretchHeight", out stretchHeight);
    fsResult fsResult48 = fsResult46 + fsResult47;
    model.stretchHeight = stretchHeight;
    bool stretchWidth = model.stretchWidth;
    fsResult fsResult49 = this.DeserializeMember<bool>(data, (System.Type) null, "stretchWidth", out stretchWidth);
    fsResult fsResult50 = fsResult48 + fsResult49;
    model.stretchWidth = stretchWidth;
    bool wordWrap = model.wordWrap;
    fsResult fsResult51 = this.DeserializeMember<bool>(data, (System.Type) null, "wordWrap", out wordWrap);
    fsResult fsResult52 = fsResult50 + fsResult51;
    model.wordWrap = wordWrap;
    return fsResult52;
  }

  public override object CreateInstance(fsData data, System.Type storageType)
  {
    return (object) new GUIStyle();
  }
}
