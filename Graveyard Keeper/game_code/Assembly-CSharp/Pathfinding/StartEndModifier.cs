// Decompiled with JetBrains decompiler
// Type: Pathfinding.StartEndModifier
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[Serializable]
public class StartEndModifier : PathModifier
{
  public bool addPoints;
  public StartEndModifier.Exactness exactStartPoint = StartEndModifier.Exactness.ClosestOnNode;
  public StartEndModifier.Exactness exactEndPoint = StartEndModifier.Exactness.ClosestOnNode;
  public Func<Vector3> adjustStartPoint;
  public bool useRaycasting;
  public LayerMask mask = (LayerMask) -1;
  public bool useGraphRaycasting;
  public List<GraphNode> connectionBuffer;
  public GraphNodeDelegate connectionBufferAddDelegate;

  public override int Order => 0;

  public override void Apply(Path _p)
  {
    if (!(_p is ABPath path) || path.vectorPath.Count == 0)
      return;
    if (path.vectorPath.Count == 1 && !this.addPoints)
      path.vectorPath.Add(path.vectorPath[0]);
    bool forceAddPoint1;
    Vector3 vector3_1 = this.Snap(path, this.exactStartPoint, true, out forceAddPoint1);
    bool forceAddPoint2;
    Vector3 vector3_2 = this.Snap(path, this.exactEndPoint, false, out forceAddPoint2);
    if ((forceAddPoint1 || this.addPoints) && this.exactStartPoint != StartEndModifier.Exactness.SnapToNode)
      path.vectorPath.Insert(0, vector3_1);
    else
      path.vectorPath[0] = vector3_1;
    if ((forceAddPoint2 || this.addPoints) && this.exactEndPoint != StartEndModifier.Exactness.SnapToNode)
      path.vectorPath.Add(vector3_2);
    else
      path.vectorPath[path.vectorPath.Count - 1] = vector3_2;
  }

  public Vector3 Snap(
    ABPath path,
    StartEndModifier.Exactness mode,
    bool start,
    out bool forceAddPoint)
  {
    int index1 = start ? 0 : path.path.Count - 1;
    GraphNode hint = path.path[index1];
    Vector3 position = (Vector3) hint.position;
    forceAddPoint = false;
    switch (mode)
    {
      case StartEndModifier.Exactness.SnapToNode:
        return position;
      case StartEndModifier.Exactness.Original:
      case StartEndModifier.Exactness.Interpolate:
      case StartEndModifier.Exactness.NodeConnection:
        Vector3 vector3_1 = !start ? path.originalEndPoint : (this.adjustStartPoint != null ? this.adjustStartPoint() : path.originalStartPoint);
        switch (mode)
        {
          case StartEndModifier.Exactness.Original:
            return this.GetClampedPoint(position, vector3_1, hint);
          case StartEndModifier.Exactness.Interpolate:
            Vector3 clampedPoint = this.GetClampedPoint(position, vector3_1, hint);
            GraphNode graphNode1 = path.path[Mathf.Clamp(index1 + (start ? 1 : -1), 0, path.path.Count - 1)];
            return VectorMath.ClosestPointOnSegment(position, (Vector3) graphNode1.position, clampedPoint);
          case StartEndModifier.Exactness.NodeConnection:
            this.connectionBuffer = this.connectionBuffer ?? new List<GraphNode>();
            this.connectionBufferAddDelegate = this.connectionBufferAddDelegate ?? new GraphNodeDelegate(this.connectionBuffer.Add);
            GraphNode graphNode2 = path.path[Mathf.Clamp(index1 + (start ? 1 : -1), 0, path.path.Count - 1)];
            hint.GetConnections(this.connectionBufferAddDelegate);
            Vector3 vector3_2 = position;
            float num = float.PositiveInfinity;
            for (int index2 = this.connectionBuffer.Count - 1; index2 >= 0; --index2)
            {
              GraphNode graphNode3 = this.connectionBuffer[index2];
              Vector3 vector3_3 = VectorMath.ClosestPointOnSegment(position, (Vector3) graphNode3.position, vector3_1);
              float sqrMagnitude = (vector3_3 - vector3_1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num)
              {
                vector3_2 = vector3_3;
                num = sqrMagnitude;
                forceAddPoint = graphNode3 != graphNode2;
              }
            }
            this.connectionBuffer.Clear();
            return vector3_2;
          default:
            throw new ArgumentException("Cannot reach this point, but the compiler is not smart enough to realize that.");
        }
      case StartEndModifier.Exactness.ClosestOnNode:
        return this.GetClampedPoint(position, start ? path.startPoint : path.endPoint, hint);
      default:
        throw new ArgumentException("Invalid mode");
    }
  }

  public Vector3 GetClampedPoint(Vector3 from, Vector3 to, GraphNode hint)
  {
    Vector3 end = to;
    RaycastHit hitInfo;
    if (this.useRaycasting && Physics.Linecast(from, to, out hitInfo, (int) this.mask))
      end = hitInfo.point;
    GraphHitInfo hit;
    if (this.useGraphRaycasting && hint != null && AstarData.GetGraph(hint) is IRaycastableGraph graph && graph.Linecast(from, end, hint, out hit))
      end = hit.point;
    return end;
  }

  public enum Exactness
  {
    SnapToNode,
    Original,
    Interpolate,
    ClosestOnNode,
    NodeConnection,
  }
}
