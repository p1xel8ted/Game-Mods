// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SelectableColorProxyHighContrastTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
