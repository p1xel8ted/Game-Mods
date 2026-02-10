// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable_Dropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class MMSelectable_Dropdown : MMSelectable
{
  [SerializeField]
  public MMDropdown _dropdown;

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
