// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIDefaultControllerElementGlyphSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
