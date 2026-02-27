// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SelectableColorProxyHighContrastTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;

#nullable disable
namespace Lamb.UI;

public class SelectableColorProxyHighContrastTarget : HighContrastTarget
{
  public SelectableColourProxy _colourProxy;
  public HighContrastConfiguration.HighContrastColorSet _cachedColorSet;

  public SelectableColorProxyHighContrastTarget(
    SelectableColourProxy colourProxy,
    HighContrastConfiguration configuration)
    : base(configuration)
  {
    this._colourProxy = colourProxy;
  }

  public override void Apply(bool state)
  {
    if (state)
      this._configuration.ColorTransitionSet.Apply(this._colourProxy);
    else
      this._cachedColorSet.Apply(this._colourProxy);
  }

  public override void Init()
  {
    this._cachedColorSet = new HighContrastConfiguration.HighContrastColorSet(this._colourProxy);
  }
}
