// Decompiled with JetBrains decompiler
// Type: FollowersToBuy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
