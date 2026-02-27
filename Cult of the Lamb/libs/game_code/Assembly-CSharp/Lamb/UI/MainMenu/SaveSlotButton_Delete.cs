// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.SaveSlotButton_Delete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.MainMenu;

public class SaveSlotButton_Delete : SaveSlotButtonBase
{
  public override void LocalizeOccupied()
  {
    if (!this._metaData.HasValue)
      return;
    this._text.text = this._metaData.Value.ToString();
  }

  public override void LocalizeEmpty() => this._text.text = "---";

  public override void SetupOccupiedSlot()
  {
    if (!this._metaData.HasValue)
      return;
    this._completionBadge.SetActive(this._metaData.Value.GameBeaten && !this._metaData.Value.SandboxBeaten);
    this._sandboxCompletionBadge.SetActive(this._metaData.Value.SandboxBeaten);
    this._button.enabled = true;
    this._button.interactable = true;
  }

  public override void SetupEmptySlot()
  {
    this._button.enabled = false;
    this._button.interactable = false;
    this._completionBadge.SetActive(false);
    this._sandboxCompletionBadge.SetActive(false);
  }
}
