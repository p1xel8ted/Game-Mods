// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathEndingCondition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding;

public abstract class PathEndingCondition
{
  public Path path;

  public PathEndingCondition()
  {
  }

  public PathEndingCondition(Path p)
  {
    this.path = p != null ? p : throw new ArgumentNullException(nameof (p));
  }

  public abstract bool TargetFound(PathNode node);
}
