// Decompiled with JetBrains decompiler
// Type: Pathfinding.FloodPathConstraint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public class FloodPathConstraint : NNConstraint
{
  public FloodPath path;

  public FloodPathConstraint(FloodPath path)
  {
    if (path == null)
      Debug.LogWarning((object) "FloodPathConstraint should not be used with a NULL path");
    this.path = path;
  }

  public override bool Suitable(GraphNode node) => base.Suitable(node) && this.path.HasPathTo(node);
}
