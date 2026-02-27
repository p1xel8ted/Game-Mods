// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable_Dropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class MMSelectable_Dropdown : MMSelectable
{
  [SerializeField]
  private MMDropdown _dropdown;

  public MMDropdown Dropdown => this._dropdown;

  public override bool Interactable
  {
    get => this.interactable;
    set
    {
      base.Interactable = value;
      this._dropdown.Interactable = this.Interactable;
    }
  }

  public override bool TryPerformConfirmAction()
  {
    this._dropdown.Open();
    return true;
  }
}
