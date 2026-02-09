// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.IAgent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding.RVO;

public interface IAgent
{
  Vector3 InterpolatedPosition { get; }

  Vector3 Position { get; }

  void SetYPosition(float yCoordinate);

  Vector3 DesiredVelocity { get; set; }

  Vector3 Velocity { get; set; }

  bool Locked { get; set; }

  float Radius { get; set; }

  float Height { get; set; }

  float MaxSpeed { get; set; }

  float NeighbourDist { get; set; }

  float AgentTimeHorizon { get; set; }

  float ObstacleTimeHorizon { get; set; }

  RVOLayer Layer { get; set; }

  RVOLayer CollidesWith { get; set; }

  bool DebugDraw { get; set; }

  int MaxNeighbours { get; set; }

  List<ObstacleVertex> NeighbourObstacles { get; }

  void Teleport(Vector3 pos);
}
