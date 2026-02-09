// Decompiled with JetBrains decompiler
// Type: Pathfinding.EndingConditionProximity
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public class EndingConditionProximity : ABPathEndingCondition
{
  public float maxDistance = 10f;

  public EndingConditionProximity(ABPath p, float maxDistance)
    : base(p)
  {
    this.maxDistance = maxDistance;
  }

  public override bool TargetFound(PathNode node)
  {
    return (double) ((Vector3) node.node.position - this.abPath.originalEndPoint).sqrMagnitude <= (double) this.maxDistance * (double) this.maxDistance;
  }
}
