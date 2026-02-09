// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable_HorizontalSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class MMSelectable_HorizontalSelector : MMSelectable
{
  [SerializeField]
  public MMHorizontalSelector _horizontalSelector;

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
