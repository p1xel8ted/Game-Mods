// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable_Toggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using WebSocketSharp;

#nullable disable
namespace Lamb.UI;

public class MMSelectable_Toggle : MMSelectable
{
  [SerializeField]
  public MMToggle _toggle;

  public MMToggle Toggle => this._toggle;

  public override bool Interactable
  {
    get => this.interactable;
    set
    {
      base.Interactable = value;
      this._toggle.Interactable = this.Interactable;
    }
  }

  public override bool TryPerformConfirmAction()
  {
    this._toggle.Toggle();
    if (!this._confirmSFX.IsNullOrEmpty())
    {
      UIManager.PlayAudio(this._confirmSFX);
      RumbleManager.Instance.Rumble();
    }
    return true;
  }
}
