// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIDefaultControllerElementGlyphSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

[AddComponentMenu("Rewired/Glyphs/Unity UI/Unity UI Default Controller Element Glyph Settings")]
public class UnityUIDefaultControllerElementGlyphSettings : DefaultControllerElementGlyphSettingsBase
{
  public override void SetDefaultGlyphOrTextPrefab()
  {
    UnityUIControllerElementGlyphBase.defaultGlyphOrTextPrefab = this.glyphOrTextPrefab;
  }
}
