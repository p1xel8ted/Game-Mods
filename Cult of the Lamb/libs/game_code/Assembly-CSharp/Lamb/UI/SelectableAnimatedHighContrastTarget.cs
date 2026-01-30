// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SelectableAnimatedHighContrastTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class SelectableAnimatedHighContrastTarget : HighContrastTarget
{
  public Selectable _selectable;

  public SelectableAnimatedHighContrastTarget(
    Selectable selectable,
    HighContrastConfiguration configuration)
    : base(configuration)
  {
    this._selectable = selectable;
  }

  public override void Apply(bool state)
  {
  }

  public override void Init()
  {
  }
}
