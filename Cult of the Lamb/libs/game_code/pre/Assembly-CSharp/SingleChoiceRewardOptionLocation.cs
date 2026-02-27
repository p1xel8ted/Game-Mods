// Decompiled with JetBrains decompiler
// Type: SingleChoiceRewardOptionLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SingleChoiceRewardOptionLocation : SingleChoiceRewardOption
{
  [SerializeField]
  private List<BuyEntry> itemOptionsDungeon1;
  [SerializeField]
  private List<BuyEntry> itemOptionsDungeon2;
  [SerializeField]
  private List<BuyEntry> itemOptionsDungeon3;
  [SerializeField]
  private List<BuyEntry> itemOptionsDungeon4;

  public override List<BuyEntry> GetOptions()
  {
    Debug.Log((object) ("PlayerFarming.Location: " + (object) PlayerFarming.Location));
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        return this.itemOptionsDungeon1;
      case FollowerLocation.Dungeon1_2:
        return this.itemOptionsDungeon2;
      case FollowerLocation.Dungeon1_3:
        return this.itemOptionsDungeon3;
      case FollowerLocation.Dungeon1_4:
        return this.itemOptionsDungeon4;
      default:
        return this.itemOptionsDungeon1;
    }
  }
}
