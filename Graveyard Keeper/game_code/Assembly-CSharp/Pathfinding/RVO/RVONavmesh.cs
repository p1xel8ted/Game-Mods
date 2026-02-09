// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVONavmesh
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding.RVO;

[AddComponentMenu("Pathfinding/Local Avoidance/RVO Navmesh")]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_navmesh.php")]
public class RVONavmesh : GraphModifier
{
  public float wallHeight = 5f;
  public List<ObstacleVertex> obstacles = new List<ObstacleVertex>();
  public Simulator lastSim;

  public override void OnPostCacheLoad() => this.OnLatePostScan();

  public override void OnLatePostScan()
  {
    if (!Application.isPlaying)
      return;
    this.RemoveObstacles();
    NavGraph[] graphs = AstarPath.active.graphs;
    RVOSimulator objectOfType = UnityEngine.Object.FindObjectOfType(typeof (RVOSimulator)) as RVOSimulator;
    Simulator sim = !((UnityEngine.Object) objectOfType == (UnityEngine.Object) null) ? objectOfType.GetSimulator() : throw new NullReferenceException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
    for (int index = 0; index < graphs.Length; ++index)
      this.AddGraphObstacles(sim, graphs[index]);
    sim.UpdateObstacles();
  }

  public void RemoveObstacles()
  {
    if (this.lastSim == null)
      return;
    Simulator lastSim = this.lastSim;
    this.lastSim = (Simulator) null;
    for (int index = 0; index < this.obstacles.Count; ++index)
      lastSim.RemoveObstacle(this.obstacles[index]);
    this.obstacles.Clear();
  }

  public void AddGraphObstacles(Simulator sim, NavGraph graph)
  {
    if (this.obstacles.Count > 0 && this.lastSim != null && this.lastSim != sim)
    {
      Debug.LogError((object) "Simulator has changed but some old obstacles are still added for the previous simulator. Deleting previous obstacles.");
      this.RemoveObstacles();
    }
    this.lastSim = sim;
    if (!(graph is INavmesh navmesh))
      return;
    int[] uses = new int[20];
    navmesh.GetNodes((GraphNodeDelegateCancelable) (_node =>
    {
      TriangleMeshNode triangleMeshNode = _node as TriangleMeshNode;
      uses[0] = uses[1] = uses[2] = 0;
      if (triangleMeshNode != null)
      {
        for (int index1 = 0; index1 < triangleMeshNode.connections.Length; ++index1)
        {
          if (triangleMeshNode.connections[index1] is TriangleMeshNode connection2)
          {
            int index2 = triangleMeshNode.SharedEdge((GraphNode) connection2);
            if (index2 != -1)
              uses[index2] = 1;
          }
        }
        for (int i = 0; i < 3; ++i)
        {
          if (uses[i] == 0)
          {
            Vector3 vertex1 = (Vector3) triangleMeshNode.GetVertex(i);
            Vector3 vertex2 = (Vector3) triangleMeshNode.GetVertex((i + 1) % triangleMeshNode.GetVertexCount());
            double num = (double) Math.Max(Math.Abs(vertex1.y - vertex2.y), 5f);
            this.obstacles.Add(sim.AddObstacle(vertex1, vertex2, this.wallHeight));
          }
        }
      }
      return true;
    }));
  }
}
