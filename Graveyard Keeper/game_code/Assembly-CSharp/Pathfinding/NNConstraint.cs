// Decompiled with JetBrains decompiler
// Type: Pathfinding.NNConstraint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Pathfinding;

public class NNConstraint
{
  public int graphMask = -1;
  public bool constrainArea;
  public int area = -1;
  public bool constrainWalkability = true;
  public bool walkable = true;
  public bool distanceXZ;
  public bool constrainTags = true;
  public int tags = -1;
  public bool constrainDistance = true;

  public virtual bool SuitableGraph(int graphIndex, NavGraph graph)
  {
    return (this.graphMask >> graphIndex & 1) != 0;
  }

  public virtual bool Suitable(GraphNode node)
  {
    return (!this.constrainWalkability || node.Walkable == this.walkable) && (!this.constrainArea || this.area < 0 || (long) node.Area == (long) this.area) && (!this.constrainTags || (this.tags >> (int) node.Tag & 1) != 0);
  }

  public static NNConstraint Default => new NNConstraint();

  public static NNConstraint None
  {
    get
    {
      return new NNConstraint()
      {
        constrainWalkability = false,
        constrainArea = false,
        constrainTags = false,
        constrainDistance = false,
        graphMask = -1
      };
    }
  }
}
