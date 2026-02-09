// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.ObstacleVertex
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding.RVO;

public class ObstacleVertex
{
  public bool ignore;
  public Vector3 position;
  public Vector2 dir;
  public float height;
  public RVOLayer layer = RVOLayer.DefaultObstacle;
  public bool convex;
  public bool split;
  public ObstacleVertex next;
  public ObstacleVertex prev;
}
