// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable_Toggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using WebSocketSharp;

#nullable disable
namespace Lamb.UI;

public class MMSelectable_Toggle : MMSelectable
{
  [SerializeField]
  private MMToggle _toggle;

  public MMToggle Toggle => this._toggle;

  public override bool Interactable
  {
    get => this.interactable;
    set
    {
      base.Interactable = value;
      this._toggle.Button.Interactable = this.Interactable;
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
