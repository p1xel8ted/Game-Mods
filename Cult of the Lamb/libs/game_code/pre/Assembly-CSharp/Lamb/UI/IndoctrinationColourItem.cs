// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationColourItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Lamb.UI;

public class IndoctrinationColourItem : IndoctrinationCharacterItem<IndoctrinationColourItem>
{
  public void Configure(int skin, int colour, WorshipperData.SkinAndData skinAndData)
  {
    this._skinAndData = skinAndData;
    this.Configure(skin, colour);
  }

  public void Configure(int skin, int colour)
  {
    this._spine.ConfigureFollowerSkin(this._skinAndData, skin, colour);
  }

  protected override void OnButtonClickedImpl()
  {
    Action<IndoctrinationColourItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }
}
