// Decompiled with JetBrains decompiler
// Type: Interaction_DepositFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DepositFollower : MonoBehaviour
{
  public static Interaction_DepositFollower Instance;
  public SimpleBark successBark;
  public SimpleBark failBark;
  [SerializeField]
  public Interaction_DepositFollowerPlant[] plants;
  public List<string> skins = new List<string>()
  {
    "Fly",
    "Bug",
    "BugTwo",
    "BugThree",
    "BugFour",
    "BugFive",
    "Chameleon"
  };

  public void Start()
  {
    Interaction_DepositFollower.Instance = this;
    List<string> ts = new List<string>();
    for (int index = 0; index < this.skins.Count; ++index)
    {
      if (!DataManager.GetFollowerSkinUnlocked(this.skins[index]))
        ts.Add(this.skins[index]);
    }
    if (ts.Count <= 0)
      ts = new List<string>((IEnumerable<string>) this.skins);
    ts.Shuffle<string>();
    ((IList<Interaction_DepositFollowerPlant>) this.plants).Shuffle<Interaction_DepositFollowerPlant>();
    this.plants[0].Configure(FollowerInfo.NewCharacter(FollowerLocation.Dungeon1_6), true, false);
    this.plants[1].Configure(FollowerInfo.NewCharacter(FollowerLocation.Dungeon1_6, ts[0]), false, true);
    this.plants[2].Configure(FollowerInfo.NewCharacter(FollowerLocation.Dungeon1_6), false, false);
    this.plants[3].Configure(FollowerInfo.NewCharacter(FollowerLocation.Dungeon1_6), false, false);
    this.plants[4].Configure(FollowerInfo.NewCharacter(FollowerLocation.Dungeon1_6), false, false);
    this.plants[5].Configure(FollowerInfo.NewCharacter(FollowerLocation.Dungeon1_6), false, false);
  }
}
