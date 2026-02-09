// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable_Slider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class MMSelectable_Slider : MMSelectable
{
  [SerializeField]
  public MMSlider _slider;

  public MMSlider Slider => this._slider;

  public override IMMSelectable TryNavigateLeft()
  {
    this._slider.DecrementValue();
    return (IMMSelectable) this;
  }

  public override IMMSelectable TryNavigateRight()
  {
    this._slider.IncrementValue();
    return (IMMSelectable) this;
  }
}
