// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationVariantItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Lamb.UI;

public class IndoctrinationVariantItem : IndoctrinationCharacterItem<IndoctrinationVariantItem>
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

  public override void OnButtonClickedImpl()
  {
    Action<IndoctrinationVariantItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }

  public override void OnButtonHighlightedImpl()
  {
    Action<IndoctrinationVariantItem> onItemHighlighted = this.OnItemHighlighted;
    if (onItemHighlighted == null)
      return;
    onItemHighlighted(this);
  }
}
