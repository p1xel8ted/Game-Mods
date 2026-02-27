// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable_Slider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class MMSelectable_Slider : MMSelectable
{
  [SerializeField]
  private MMSlider _slider;

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
