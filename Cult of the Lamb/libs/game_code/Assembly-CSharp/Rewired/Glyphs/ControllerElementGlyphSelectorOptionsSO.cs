// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.ControllerElementGlyphSelectorOptionsSO
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

[Serializable]
public class ControllerElementGlyphSelectorOptionsSO : ControllerElementGlyphSelectorOptionsSOBase
{
  [SerializeField]
  public ControllerElementGlyphSelectorOptions _options;

  public override ControllerElementGlyphSelectorOptions options => this._options;
}
