// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MainMenu.SaveSlotButton_Delete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.MainMenu;

public class SaveSlotButton_Delete : SaveSlotButtonBase
{
  protected override void LocalizeOccupied()
  {
    if (!this._metaData.HasValue)
      return;
    this._text.text = this._metaData.Value.ToString();
  }

  protected override void LocalizeEmpty() => this._text.text = "---";

  protected override void SetupOccupiedSlot()
  {
    if (!this._metaData.HasValue)
      return;
    this._completionBadge.SetActive(this._metaData.Value.GameBeaten);
    this._button.interactable = true;
  }

  protected override void SetupEmptySlot()
  {
    this._button.enabled = false;
    this._button.interactable = false;
    this._completionBadge.SetActive(false);
  }
}
