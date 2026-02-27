// Decompiled with JetBrains decompiler
// Type: FollowersToBuy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class FollowersToBuy
{
  public FollowersToBuy.FollowerBuyTypes followerTypes;
  public int followerCost;
  [Range(0.0f, 100f)]
  public int chanceToSpawn;

  public FollowersToBuy()
  {
  }

  public FollowersToBuy(
    FollowersToBuy.FollowerBuyTypes followerTypes,
    int followerCost,
    int chanceToSpawn)
  {
    this.followerTypes = followerTypes;
    this.followerCost = followerCost;
    this.chanceToSpawn = chanceToSpawn;
  }

  public enum FollowerBuyTypes
  {
    None,
    Ill,
    Old,
    Faithful,
    Level1,
    Level2,
    Level3,
    Level4,
  }
}
