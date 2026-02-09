// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathNNConstraint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Pathfinding;

public class PathNNConstraint : NNConstraint
{
  public static PathNNConstraint Default
  {
    get
    {
      PathNNConstraint pathNnConstraint = new PathNNConstraint();
      pathNnConstraint.constrainArea = true;
      return pathNnConstraint;
    }
  }

  public virtual void SetStart(GraphNode node)
  {
    if (node != null)
      this.area = (int) node.Area;
    else
      this.constrainArea = false;
  }
}
