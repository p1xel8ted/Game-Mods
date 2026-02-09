// Decompiled with JetBrains decompiler
// Type: Pathfinding.RaycastModifier
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[AddComponentMenu("Pathfinding/Modifiers/Raycast Simplifier")]
[RequireComponent(typeof (Seeker))]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_raycast_modifier.php")]
[Serializable]
public class RaycastModifier : MonoModifier
{
  [HideInInspector]
  public bool useRaycasting = true;
  [HideInInspector]
  public LayerMask mask = (LayerMask) -1;
  [HideInInspector]
  public bool thickRaycast;
  [HideInInspector]
  public float thickRaycastRadius;
  [HideInInspector]
  public Vector3 raycastOffset = Vector3.zero;
  [HideInInspector]
  public bool subdivideEveryIter;
  public int iterations = 2;
  [HideInInspector]
  public bool useGraphRaycasting;
  public static List<Vector3> nodes;

  public override int Order => 40;

  public override void Apply(Path p)
  {
    if (this.iterations <= 0)
      return;
    if (RaycastModifier.nodes == null)
      RaycastModifier.nodes = new List<Vector3>(p.vectorPath.Count);
    else
      RaycastModifier.nodes.Clear();
    RaycastModifier.nodes.AddRange((IEnumerable<Vector3>) p.vectorPath);
    for (int index1 = 0; index1 < this.iterations; ++index1)
    {
      if (this.subdivideEveryIter && index1 != 0)
      {
        if (RaycastModifier.nodes.Capacity < RaycastModifier.nodes.Count * 3)
          RaycastModifier.nodes.Capacity = RaycastModifier.nodes.Count * 3;
        int count = RaycastModifier.nodes.Count;
        for (int index2 = 0; index2 < count - 1; ++index2)
        {
          RaycastModifier.nodes.Add(Vector3.zero);
          RaycastModifier.nodes.Add(Vector3.zero);
        }
        for (int index3 = count - 1; index3 > 0; --index3)
        {
          Vector3 node1 = RaycastModifier.nodes[index3];
          Vector3 node2 = RaycastModifier.nodes[index3 + 1];
          RaycastModifier.nodes[index3 * 3] = RaycastModifier.nodes[index3];
          if (index3 != count - 1)
          {
            RaycastModifier.nodes[index3 * 3 + 1] = Vector3.Lerp(node1, node2, 0.33f);
            RaycastModifier.nodes[index3 * 3 + 2] = Vector3.Lerp(node1, node2, 0.66f);
          }
        }
      }
      int index4 = 0;
      while (index4 < RaycastModifier.nodes.Count - 2)
      {
        Vector3 node3 = RaycastModifier.nodes[index4];
        Vector3 node4 = RaycastModifier.nodes[index4 + 2];
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (this.ValidateLine((GraphNode) null, (GraphNode) null, node3, node4))
          RaycastModifier.nodes.RemoveAt(index4 + 1);
        else
          ++index4;
        stopwatch.Stop();
      }
    }
    p.vectorPath.Clear();
    p.vectorPath.AddRange((IEnumerable<Vector3>) RaycastModifier.nodes);
  }

  public bool ValidateLine(GraphNode n1, GraphNode n2, Vector3 v1, Vector3 v2)
  {
    if (this.useRaycasting)
    {
      if (this.thickRaycast && (double) this.thickRaycastRadius > 0.0)
      {
        if (Physics.SphereCast(v1 + this.raycastOffset, this.thickRaycastRadius, v2 - v1, out RaycastHit _, (v2 - v1).magnitude, (int) this.mask))
          return false;
      }
      else if (Physics.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, out RaycastHit _, (int) this.mask))
        return false;
    }
    if (this.useGraphRaycasting && n1 == null)
    {
      n1 = AstarPath.active.GetNearest(v1).node;
      n2 = AstarPath.active.GetNearest(v2).node;
    }
    if (this.useGraphRaycasting && n1 != null && n2 != null)
    {
      NavGraph graph1 = AstarData.GetGraph(n1);
      NavGraph graph2 = AstarData.GetGraph(n2);
      if (graph1 != graph2 || graph1 != null && graph1 is IRaycastableGraph raycastableGraph && raycastableGraph.Linecast(v1, v2, n1))
        return false;
    }
    return true;
  }
}
