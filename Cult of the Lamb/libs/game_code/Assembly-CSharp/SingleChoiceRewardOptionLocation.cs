// Decompiled with JetBrains decompiler
// Type: SingleChoiceRewardOptionLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SingleChoiceRewardOptionLocation : SingleChoiceRewardOption
{
  [SerializeField]
  public List<BuyEntry> itemOptionsDungeon1;
  [SerializeField]
  public List<BuyEntry> itemOptionsDungeon2;
  [SerializeField]
  public List<BuyEntry> itemOptionsDungeon3;
  [SerializeField]
  public List<BuyEntry> itemOptionsDungeon4;

  public override List<BuyEntry> GetOptions()
  {
    Debug.Log((object) ("PlayerFarming.Location: " + PlayerFarming.Location.ToString()));
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
