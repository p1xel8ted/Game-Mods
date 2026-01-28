// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIControllerElementGlyph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

[AddComponentMenu("Rewired/Glyphs/Unity UI/Unity UI Controller Element Glyph")]
public class UnityUIControllerElementGlyph : ControllerElementGlyph
{
  public override GameObject GetDefaultGlyphOrTextPrefab()
  {
    return UnityUIControllerElementGlyphBase.defaultGlyphOrTextPrefab;
  }
}
