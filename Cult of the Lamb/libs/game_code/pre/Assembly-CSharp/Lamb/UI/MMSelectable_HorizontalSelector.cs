// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable_HorizontalSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class MMSelectable_HorizontalSelector : MMSelectable
{
  [SerializeField]
  private MMHorizontalSelector _horizontalSelector;

  public MMHorizontalSelector HorizontalSelector => this._horizontalSelector;

  public override bool Interactable
  {
    get => this.interactable;
    set
    {
      base.Interactable = value;
      this._horizontalSelector.Interactable = this.Interactable;
    }
  }

  public override IMMSelectable TryNavigateLeft()
  {
    if (this.Interactable)
      this._horizontalSelector.LeftButton.TryPerformConfirmAction();
    return (IMMSelectable) this;
  }

  public override IMMSelectable TryNavigateRight()
  {
    if (this.Interactable)
      this._horizontalSelector.RightButton.TryPerformConfirmAction();
    return (IMMSelectable) this;
  }
}
