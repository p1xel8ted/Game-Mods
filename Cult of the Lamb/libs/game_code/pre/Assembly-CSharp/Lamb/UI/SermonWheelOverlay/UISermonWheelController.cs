// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SermonWheelOverlay.UISermonWheelController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Lamb.UI.SermonWheelOverlay;

public class UISermonWheelController : UIRadialMenuBase<SermonWheelCategory, SermonCategory>
{
  protected override bool SelectOnHighlight => false;

  protected override void OnChoiceFinalized() => this.Hide();

  protected override void MakeChoice(SermonWheelCategory item)
  {
    Action<SermonCategory> onItemChosen = this.OnItemChosen;
    if (onItemChosen == null)
      return;
    onItemChosen(item.SermonCategory);
  }

  public override void OnCancelButtonInput()
  {
  }
}
