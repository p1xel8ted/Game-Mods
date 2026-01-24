// Decompiled with JetBrains decompiler
// Type: FollowersToBuy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
