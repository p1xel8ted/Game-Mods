// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.ControllerElementGlyphSelectorOptionsSO
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
