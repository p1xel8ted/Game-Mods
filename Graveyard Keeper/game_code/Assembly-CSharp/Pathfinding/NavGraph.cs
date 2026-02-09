// Decompiled with JetBrains decompiler
// Type: Pathfinding.NavGraph
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public abstract class NavGraph
{
  public AstarPath active;
  [JsonMember]
  public Pathfinding.Util.Guid guid;
  [JsonMember]
  public uint initialPenalty;
  [JsonMember]
  public bool open;
  public uint graphIndex;
  [JsonMember]
  public string name;
  [JsonMember]
  public bool drawGizmos = true;
  [JsonMember]
  public bool infoScreenOpen;
  public Matrix4x4 matrix = Matrix4x4.identity;
  public Matrix4x4 inverseMatrix = Matrix4x4.identity;

  public virtual int CountNodes()
  {
    int count = 0;
    this.GetNodes((GraphNodeDelegateCancelable) (node =>
    {
      ++count;
      return true;
    }));
    return count;
  }

  public abstract void GetNodes(GraphNodeDelegateCancelable del);

  public virtual void FastClearNodes()
  {
    this.GetNodes((GraphNodeDelegateCancelable) (node =>
    {
      node.Destroy();
      return true;
    }));
  }

  public void SetMatrix(Matrix4x4 m)
  {
    this.matrix = m;
    this.inverseMatrix = m.inverse;
  }

  public virtual void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
  {
    Matrix4x4 inverse = oldMatrix.inverse;
    Matrix4x4 m = newMatrix * inverse;
    this.GetNodes((GraphNodeDelegateCancelable) (node =>
    {
      node.position = (Int3) m.MultiplyPoint((Vector3) node.position);
      return true;
    }));
    this.SetMatrix(newMatrix);
  }

  public NNInfo GetNearest(Vector3 position) => this.GetNearest(position, NNConstraint.None);

  public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
  {
    return this.GetNearest(position, constraint, (GraphNode) null);
  }

  public virtual NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
  {
    float maxDistSqr = constraint.constrainDistance ? AstarPath.active.maxNearestNodeDistanceSqr : float.PositiveInfinity;
    float minDist = float.PositiveInfinity;
    GraphNode minNode = (GraphNode) null;
    float minConstDist = float.PositiveInfinity;
    GraphNode minConstNode = (GraphNode) null;
    this.GetNodes((GraphNodeDelegateCancelable) (node =>
    {
      float sqrMagnitude = (position - (Vector3) node.position).sqrMagnitude;
      if ((double) sqrMagnitude < (double) minDist)
      {
        minDist = sqrMagnitude;
        minNode = node;
      }
      if ((double) sqrMagnitude < (double) minConstDist && (double) sqrMagnitude < (double) maxDistSqr && constraint.Suitable(node))
      {
        minConstDist = sqrMagnitude;
        minConstNode = node;
      }
      return true;
    }));
    NNInfo nearest = new NNInfo(minNode);
    nearest.constrainedNode = minConstNode;
    if (minConstNode != null)
      nearest.constClampedPosition = (Vector3) minConstNode.position;
    else if (minNode != null)
    {
      nearest.constrainedNode = minNode;
      nearest.constClampedPosition = (Vector3) minNode.position;
    }
    return nearest;
  }

  public virtual NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
  {
    return this.GetNearest(position, constraint);
  }

  public virtual void Awake()
  {
  }

  public virtual void OnDestroy()
  {
    if (AstarPath.quittingApplication)
      this.FastClearNodes();
    else
      this.GetNodes((GraphNodeDelegateCancelable) (node =>
      {
        node.Destroy();
        return true;
      }));
  }

  public void ScanGraph()
  {
    if (AstarPath.OnPreScan != null)
      AstarPath.OnPreScan(AstarPath.active);
    if (AstarPath.OnGraphPreScan != null)
      AstarPath.OnGraphPreScan(this);
    this.ScanInternal();
    if (AstarPath.OnGraphPostScan != null)
      AstarPath.OnGraphPostScan(this);
    if (AstarPath.OnPostScan == null)
      return;
    AstarPath.OnPostScan(AstarPath.active);
  }

  [Obsolete("Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had")]
  public void Scan()
  {
    throw new Exception("This method is deprecated. Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had.");
  }

  public void ScanInternal() => this.ScanInternal((OnScanStatus) null);

  public abstract void ScanInternal(OnScanStatus statusCallback);

  public virtual Color NodeColor(GraphNode node, PathHandler data)
  {
    Color color = AstarColor.NodeConnection;
    switch (AstarPath.active.debugMode)
    {
      case GraphDebugMode.Areas:
        color = AstarColor.GetAreaColor(node.Area);
        break;
      case GraphDebugMode.Penalty:
        color = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (float) (((double) node.Penalty - (double) AstarPath.active.debugFloor) / ((double) AstarPath.active.debugRoof - (double) AstarPath.active.debugFloor)));
        break;
      case GraphDebugMode.Tags:
        color = AstarColor.GetAreaColor(node.Tag);
        break;
      default:
        if (data == null)
          return AstarColor.NodeConnection;
        PathNode pathNode = data.GetPathNode(node);
        switch (AstarPath.active.debugMode)
        {
          case GraphDebugMode.G:
            color = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (float) (((double) pathNode.G - (double) AstarPath.active.debugFloor) / ((double) AstarPath.active.debugRoof - (double) AstarPath.active.debugFloor)));
            break;
          case GraphDebugMode.H:
            color = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (float) (((double) pathNode.H - (double) AstarPath.active.debugFloor) / ((double) AstarPath.active.debugRoof - (double) AstarPath.active.debugFloor)));
            break;
          case GraphDebugMode.F:
            color = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (float) (((double) pathNode.F - (double) AstarPath.active.debugFloor) / ((double) AstarPath.active.debugRoof - (double) AstarPath.active.debugFloor)));
            break;
        }
        break;
    }
    color.a *= 0.5f;
    return color;
  }

  public virtual void SerializeExtraInfo(GraphSerializationContext ctx)
  {
  }

  public virtual void DeserializeExtraInfo(GraphSerializationContext ctx)
  {
  }

  public virtual void PostDeserialization()
  {
  }

  public virtual void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
  {
    this.guid = new Pathfinding.Util.Guid(ctx.reader.ReadBytes(16 /*0x10*/));
    this.initialPenalty = ctx.reader.ReadUInt32();
    this.open = ctx.reader.ReadBoolean();
    this.name = ctx.reader.ReadString();
    this.drawGizmos = ctx.reader.ReadBoolean();
    this.infoScreenOpen = ctx.reader.ReadBoolean();
    for (int index1 = 0; index1 < 4; ++index1)
    {
      Vector4 zero = Vector4.zero;
      for (int index2 = 0; index2 < 4; ++index2)
        zero[index2] = ctx.reader.ReadSingle();
      this.matrix.SetRow(index1, zero);
    }
  }

  public static bool InSearchTree(GraphNode node, Path path)
  {
    return path == null || path.pathHandler == null || (int) path.pathHandler.GetPathNode(node).pathID == (int) path.pathID;
  }

  public virtual void OnDrawGizmos(bool drawNodes)
  {
    if (!drawNodes)
      return;
    PathHandler data = AstarPath.active.debugPathData;
    GraphNode node = (GraphNode) null;
    GraphNodeDelegate drawConnection = (GraphNodeDelegate) (otherNode => Gizmos.DrawLine((Vector3) node.position, (Vector3) otherNode.position));
    this.GetNodes((GraphNodeDelegateCancelable) (_node =>
    {
      node = _node;
      Gizmos.color = this.NodeColor(node, AstarPath.active.debugPathData);
      if (AstarPath.active.showSearchTree && !NavGraph.InSearchTree(node, AstarPath.active.debugPath))
        return true;
      PathNode pathNode = data != null ? data.GetPathNode(node) : (PathNode) null;
      if (AstarPath.active.showSearchTree && pathNode != null && pathNode.parent != null)
        Gizmos.DrawLine((Vector3) node.position, (Vector3) pathNode.parent.node.position);
      else
        node.GetConnections(drawConnection);
      return true;
    }));
  }

  public virtual void UnloadGizmoMeshes()
  {
  }
}
