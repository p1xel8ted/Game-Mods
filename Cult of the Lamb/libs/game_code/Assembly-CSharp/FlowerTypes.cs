// Decompiled with JetBrains decompiler
// Type: FlowerTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class FlowerTypes
{
  public string Flower;
  public string Grass;
  public List<FollowerLocation> Location = new List<FollowerLocation>();
  public Vector2 PercentageChanceToSpawn;
}
