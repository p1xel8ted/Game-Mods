// Decompiled with JetBrains decompiler
// Type: GDPointNode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;

#nullable disable
public class GDPointNode : PointNode
{
  public GDPoint gd_point;

  public GDPointNode(AstarPath astar, GDPoint gd_point)
    : base(astar)
  {
    this.gd_point = gd_point;
  }

  public void LinkAdjacentNode(GDPointNode n, uint cost = 1)
  {
    this.AddConnection((GraphNode) n, cost);
  }
}
