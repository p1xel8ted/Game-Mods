// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SelectableColorTransitionHighContrastTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class SelectableColorTransitionHighContrastTarget : HighContrastTarget
{
  public Selectable _selectable;
  public HighContrastConfiguration.HighContrastColorSet _cachedColorSet;

  public SelectableColorTransitionHighContrastTarget(
    Selectable selectable,
    HighContrastConfiguration configuration)
    : base(configuration)
  {
    this._selectable = selectable;
  }

  public override void Apply(bool state)
  {
    if (state)
      this._configuration.ColorTransitionSet.Apply(this._selectable);
    else
      this._cachedColorSet.Apply(this._selectable);
  }

  public override void Init()
  {
    this._cachedColorSet = new HighContrastConfiguration.HighContrastColorSet(this._selectable);
  }
}
