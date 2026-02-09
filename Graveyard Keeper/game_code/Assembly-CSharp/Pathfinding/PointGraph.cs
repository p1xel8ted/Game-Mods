// Decompiled with JetBrains decompiler
// Type: Pathfinding.PointGraph
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[JsonOptIn]
public class PointGraph : NavGraph, IUpdatableGraph
{
  [JsonMember]
  public Transform root;
  [JsonMember]
  public string searchTag;
  [JsonMember]
  public float maxDistance;
  [JsonMember]
  public Vector3 limits;
  [JsonMember]
  public bool raycast = true;
  [JsonMember]
  public bool use2DPhysics;
  [JsonMember]
  public bool thickRaycast;
  [JsonMember]
  public float thickRaycastRadius = 1f;
  [JsonMember]
  public bool recursive = true;
  [JsonMember]
  public LayerMask mask;
  [JsonMember]
  public bool optimizeForSparseGraph;
  [JsonMember]
  public bool optimizeFor2D;
  public static Int3[] ThreeDNeighbours = new Int3[27]
  {
    new Int3(-1, 0, -1),
    new Int3(0, 0, -1),
    new Int3(1, 0, -1),
    new Int3(-1, 0, 0),
    new Int3(0, 0, 0),
    new Int3(1, 0, 0),
    new Int3(-1, 0, 1),
    new Int3(0, 0, 1),
    new Int3(1, 0, 1),
    new Int3(-1, -1, -1),
    new Int3(0, -1, -1),
    new Int3(1, -1, -1),
    new Int3(-1, -1, 0),
    new Int3(0, -1, 0),
    new Int3(1, -1, 0),
    new Int3(-1, -1, 1),
    new Int3(0, -1, 1),
    new Int3(1, -1, 1),
    new Int3(-1, 1, -1),
    new Int3(0, 1, -1),
    new Int3(1, 1, -1),
    new Int3(-1, 1, 0),
    new Int3(0, 1, 0),
    new Int3(1, 1, 0),
    new Int3(-1, 1, 1),
    new Int3(0, 1, 1),
    new Int3(1, 1, 1)
  };
  public Dictionary<Int3, PointNode> nodeLookup;
  public Int3 minLookup;
  public Int3 maxLookup;
  public Int3 lookupCellSize;
  public PointNode[] nodes;
  [CompilerGenerated]
  public int \u003CnodeCount\u003Ek__BackingField;

  public int nodeCount
  {
    get => this.\u003CnodeCount\u003Ek__BackingField;
    set => this.\u003CnodeCount\u003Ek__BackingField = value;
  }

  public Int3 WorldToLookupSpace(Int3 p)
  {
    return Int3.zero with
    {
      x = this.lookupCellSize.x != 0 ? p.x / this.lookupCellSize.x : 0,
      y = this.lookupCellSize.y != 0 ? p.y / this.lookupCellSize.y : 0,
      z = this.lookupCellSize.z != 0 ? p.z / this.lookupCellSize.z : 0
    };
  }

  public override int CountNodes() => this.nodeCount;

  public override void GetNodes(GraphNodeDelegateCancelable del)
  {
    if (this.nodes == null)
      return;
    int index = 0;
    while (index < this.nodeCount && del((GraphNode) this.nodes[index]))
      ++index;
  }

  public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
  {
    return this.GetNearestForce(position, constraint);
  }

  public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
  {
    if (this.nodes == null)
      return new NNInfo();
    float maxDistSqr = constraint.constrainDistance ? AstarPath.active.maxNearestNodeDistanceSqr : float.PositiveInfinity;
    float num1 = float.PositiveInfinity;
    GraphNode node1 = (GraphNode) null;
    float num2 = float.PositiveInfinity;
    GraphNode graphNode = (GraphNode) null;
    if (this.optimizeForSparseGraph)
    {
      Int3 lookupSpace = this.WorldToLookupSpace((Int3) position);
      Int3 int3_1 = lookupSpace - this.minLookup;
      int val1_1 = Math.Max(Math.Max(Math.Max(0, Math.Abs(int3_1.x)), Math.Abs(int3_1.y)), Math.Abs(int3_1.z));
      Int3 int3_2 = lookupSpace - this.maxLookup;
      int val1_2 = Math.Max(Math.Max(Math.Max(val1_1, Math.Abs(int3_2.x)), Math.Abs(int3_2.y)), Math.Abs(int3_2.z));
      PointGraph.GetNearestHelper getNearestHelper = new PointGraph.GetNearestHelper(position, maxDistSqr, constraint, this.nodeLookup);
      getNearestHelper.Search(lookupSpace);
      for (int index = 1; index <= val1_2; ++index)
      {
        if (index >= 20)
        {
          Debug.LogWarning((object) "Aborting GetNearest call at maximum distance because it has iterated too many times.\nIf you get this regularly, check your settings for PointGraph -> <b>Optimize For Sparse Graph</b> and PointGraph -> <b>Optimize For 2D</b>.\nThis happens when the closest node was very far away (20*link distance between nodes). When optimizing for sparse graphs, getting the nearest node from far away positions is <b>very slow</b>.\n");
          break;
        }
        if (this.lookupCellSize.y == 0)
        {
          Int3 int3_3 = lookupSpace + new Int3(-index, 0, -index);
          for (int _x = 0; _x <= 2 * index; ++_x)
          {
            getNearestHelper.Search(int3_3 + new Int3(_x, 0, 0));
            getNearestHelper.Search(int3_3 + new Int3(_x, 0, 2 * index));
          }
          for (int _z = 1; _z < 2 * index; ++_z)
          {
            getNearestHelper.Search(int3_3 + new Int3(0, 0, _z));
            getNearestHelper.Search(int3_3 + new Int3(2 * index, 0, _z));
          }
        }
        else
        {
          Int3 int3_4 = lookupSpace + new Int3(-index, -index, -index);
          for (int _x = 0; _x <= 2 * index; ++_x)
          {
            for (int _y = 0; _y <= 2 * index; ++_y)
            {
              getNearestHelper.Search(int3_4 + new Int3(_x, _y, 0));
              getNearestHelper.Search(int3_4 + new Int3(_x, _y, 2 * index));
            }
          }
          for (int _z = 1; _z < 2 * index; ++_z)
          {
            for (int _y = 0; _y <= 2 * index; ++_y)
            {
              getNearestHelper.Search(int3_4 + new Int3(0, _y, _z));
              getNearestHelper.Search(int3_4 + new Int3(2 * index, _y, _z));
            }
          }
          for (int _x = 1; _x < 2 * index; ++_x)
          {
            for (int _z = 1; _z < 2 * index; ++_z)
            {
              getNearestHelper.Search(int3_4 + new Int3(_x, 0, _z));
              getNearestHelper.Search(int3_4 + new Int3(_x, 2 * index, _z));
            }
          }
        }
        graphNode = (GraphNode) getNearestHelper.minConstNode;
        node1 = (GraphNode) getNearestHelper.minNode;
        float minDist = getNearestHelper.minDist;
        float minConstDist = getNearestHelper.minConstDist;
        if (graphNode != null)
          val1_2 = Math.Min(val1_2, index + 1);
      }
    }
    else
    {
      for (int index = 0; index < this.nodeCount; ++index)
      {
        PointNode node2 = this.nodes[index];
        float sqrMagnitude = ((Vector2) position - (Vector2) node2.position).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num1)
        {
          num1 = sqrMagnitude;
          node1 = (GraphNode) node2;
        }
        if (constraint == null || (double) sqrMagnitude < (double) num2 && (double) sqrMagnitude < (double) maxDistSqr && constraint.Suitable((GraphNode) node2))
        {
          num2 = sqrMagnitude;
          graphNode = (GraphNode) node2;
        }
      }
    }
    NNInfo nearestForce = new NNInfo(node1);
    nearestForce.constrainedNode = graphNode;
    if (graphNode != null)
      nearestForce.constClampedPosition = (Vector3) (Vector2) graphNode.position;
    else if (node1 != null)
    {
      nearestForce.constrainedNode = node1;
      nearestForce.constClampedPosition = (Vector3) (Vector2) node1.position;
    }
    return nearestForce;
  }

  public PointNode AddNode(Int3 position)
  {
    return this.AddNode<PointNode>(new PointNode(this.active), position);
  }

  public T AddNode<T>(T node, Int3 position) where T : PointNode
  {
    if (this.nodes == null || this.nodeCount == this.nodes.Length)
    {
      PointNode[] pointNodeArray = new PointNode[this.nodes != null ? Math.Max(this.nodes.Length + 4, this.nodes.Length * 2) : 4];
      for (int index = 0; index < this.nodeCount; ++index)
        pointNodeArray[index] = this.nodes[index];
      this.nodes = pointNodeArray;
    }
    node.SetPosition(position);
    node.GraphIndex = this.graphIndex;
    node.Walkable = true;
    this.nodes[this.nodeCount] = (PointNode) node;
    ++this.nodeCount;
    this.AddToLookup((PointNode) node);
    return node;
  }

  public static int CountChildren(Transform tr)
  {
    int num = 0;
    foreach (Transform tr1 in tr)
    {
      ++num;
      num += PointGraph.CountChildren(tr1);
    }
    return num;
  }

  public void AddChildren(ref int c, Transform tr)
  {
    foreach (Transform tr1 in tr)
    {
      this.nodes[c].SetPosition((Int3) tr1.position);
      this.nodes[c].Walkable = true;
      this.nodes[c].gameObject = tr1.gameObject;
      ++c;
      this.AddChildren(ref c, tr1);
    }
  }

  public void RebuildNodeLookup()
  {
    if (!this.optimizeForSparseGraph)
      return;
    if ((double) this.maxDistance == 0.0)
    {
      this.lookupCellSize = (Int3) this.limits;
    }
    else
    {
      this.lookupCellSize.x = Mathf.CeilToInt((float) (1000.0 * ((double) this.limits.x != 0.0 ? (double) Mathf.Min(this.maxDistance, this.limits.x) : (double) this.maxDistance)));
      this.lookupCellSize.y = Mathf.CeilToInt((float) (1000.0 * ((double) this.limits.y != 0.0 ? (double) Mathf.Min(this.maxDistance, this.limits.y) : (double) this.maxDistance)));
      this.lookupCellSize.z = Mathf.CeilToInt((float) (1000.0 * ((double) this.limits.z != 0.0 ? (double) Mathf.Min(this.maxDistance, this.limits.z) : (double) this.maxDistance)));
    }
    if (this.optimizeFor2D)
      this.lookupCellSize.y = 0;
    if (this.nodeLookup == null)
      this.nodeLookup = new Dictionary<Int3, PointNode>();
    this.nodeLookup.Clear();
    for (int index = 0; index < this.nodeCount; ++index)
      this.AddToLookup(this.nodes[index]);
  }

  public void AddToLookup(PointNode node)
  {
    if (this.nodeLookup == null)
      return;
    Int3 lookupSpace = this.WorldToLookupSpace(node.position);
    if (this.nodeLookup.Count == 0)
    {
      this.minLookup = lookupSpace;
      this.maxLookup = lookupSpace;
    }
    else
    {
      this.minLookup = new Int3(Math.Min(this.minLookup.x, lookupSpace.x), Math.Min(this.minLookup.y, lookupSpace.y), Math.Min(this.minLookup.z, lookupSpace.z));
      this.maxLookup = new Int3(Math.Max(this.minLookup.x, lookupSpace.x), Math.Max(this.minLookup.y, lookupSpace.y), Math.Max(this.minLookup.z, lookupSpace.z));
    }
    if (node.next != null)
      throw new Exception("This node has already been added to the lookup structure");
    PointNode pointNode;
    if (this.nodeLookup.TryGetValue(lookupSpace, out pointNode))
    {
      node.next = pointNode.next;
      pointNode.next = node;
    }
    else
      this.nodeLookup[lookupSpace] = node;
  }

  public override void ScanInternal(OnScanStatus statusCallback)
  {
    if ((UnityEngine.Object) this.root == (UnityEngine.Object) null)
    {
      GameObject[] gameObjectsWithTag = this.searchTag != null ? GameObject.FindGameObjectsWithTag(this.searchTag) : (GameObject[]) null;
      if (gameObjectsWithTag == null)
      {
        this.nodes = new PointNode[0];
        this.nodeCount = 0;
        return;
      }
      this.nodes = new PointNode[gameObjectsWithTag.Length];
      this.nodeCount = this.nodes.Length;
      for (int index = 0; index < this.nodes.Length; ++index)
        this.nodes[index] = new PointNode(this.active);
      for (int index = 0; index < gameObjectsWithTag.Length; ++index)
      {
        this.nodes[index].SetPosition((Int3) gameObjectsWithTag[index].transform.position);
        this.nodes[index].Walkable = true;
        this.nodes[index].gameObject = gameObjectsWithTag[index].gameObject;
      }
    }
    else if (!this.recursive)
    {
      this.nodes = new PointNode[this.root.childCount];
      this.nodeCount = this.nodes.Length;
      for (int index = 0; index < this.nodes.Length; ++index)
        this.nodes[index] = new PointNode(this.active);
      int index1 = 0;
      foreach (Transform transform in this.root)
      {
        this.nodes[index1].SetPosition((Int3) transform.position);
        this.nodes[index1].Walkable = true;
        this.nodes[index1].gameObject = transform.gameObject;
        ++index1;
      }
    }
    else
    {
      this.nodes = new PointNode[PointGraph.CountChildren(this.root)];
      this.nodeCount = this.nodes.Length;
      for (int index = 0; index < this.nodes.Length; ++index)
        this.nodes[index] = new PointNode(this.active);
      int c = 0;
      this.AddChildren(ref c, this.root);
    }
    if (this.optimizeForSparseGraph)
      this.RebuildNodeLookup();
    if ((double) this.maxDistance < 0.0)
      return;
    List<PointNode> pointNodeList = new List<PointNode>(3);
    List<uint> uintList = new List<uint>(3);
    for (int index2 = 0; index2 < this.nodes.Length; ++index2)
    {
      pointNodeList.Clear();
      uintList.Clear();
      PointNode node1 = this.nodes[index2];
      if (this.optimizeForSparseGraph)
      {
        Int3 lookupSpace = this.WorldToLookupSpace(node1.position);
        int num = this.lookupCellSize.y == 0 ? 9 : PointGraph.ThreeDNeighbours.Length;
        for (int index3 = 0; index3 < num; ++index3)
        {
          PointNode next;
          if (this.nodeLookup.TryGetValue(lookupSpace + PointGraph.ThreeDNeighbours[index3], out next))
          {
            for (; next != null; next = next.next)
            {
              float dist;
              if (this.IsValidConnection((GraphNode) node1, (GraphNode) next, out dist))
              {
                pointNodeList.Add(next);
                uintList.Add((uint) Mathf.RoundToInt(dist * 1000f));
              }
            }
          }
        }
      }
      else
      {
        for (int index4 = 0; index4 < this.nodes.Length; ++index4)
        {
          if (index2 != index4)
          {
            PointNode node2 = this.nodes[index4];
            float dist;
            if (this.IsValidConnection((GraphNode) node1, (GraphNode) node2, out dist))
            {
              pointNodeList.Add(node2);
              uintList.Add((uint) Mathf.RoundToInt(dist * 1000f));
            }
          }
        }
      }
      node1.connections = (GraphNode[]) pointNodeList.ToArray();
      node1.connectionCosts = uintList.ToArray();
    }
  }

  public virtual bool IsValidConnection(GraphNode a, GraphNode b, out float dist)
  {
    dist = 0.0f;
    if (!a.Walkable || !b.Walkable)
      return false;
    Vector3 direction = (Vector3) (b.position - a.position);
    if (!Mathf.Approximately(this.limits.x, 0.0f) && (double) Mathf.Abs(direction.x) > (double) this.limits.x || !Mathf.Approximately(this.limits.y, 0.0f) && (double) Mathf.Abs(direction.y) > (double) this.limits.y || !Mathf.Approximately(this.limits.z, 0.0f) && (double) Mathf.Abs(direction.z) > (double) this.limits.z)
      return false;
    dist = direction.magnitude;
    if ((double) this.maxDistance != 0.0 && (double) dist >= (double) this.maxDistance)
      return false;
    if (!this.raycast)
      return true;
    Ray ray1 = new Ray((Vector3) a.position, direction);
    Ray ray2 = new Ray((Vector3) b.position, -direction);
    return this.use2DPhysics ? (this.thickRaycast ? !(bool) Physics2D.CircleCast((Vector2) ray1.origin, this.thickRaycastRadius, (Vector2) ray1.direction, dist, (int) this.mask) && !(bool) Physics2D.CircleCast((Vector2) ray2.origin, this.thickRaycastRadius, (Vector2) ray2.direction, dist, (int) this.mask) : !(bool) Physics2D.Linecast((Vector2) (Vector3) a.position, (Vector2) (Vector3) b.position, (int) this.mask) && !(bool) Physics2D.Linecast((Vector2) (Vector3) b.position, (Vector2) (Vector3) a.position, (int) this.mask)) : (this.thickRaycast ? !Physics.SphereCast(ray1, this.thickRaycastRadius, dist, (int) this.mask) && !Physics.SphereCast(ray2, this.thickRaycastRadius, dist, (int) this.mask) : !Physics.Linecast((Vector3) a.position, (Vector3) b.position, (int) this.mask) && !Physics.Linecast((Vector3) b.position, (Vector3) a.position, (int) this.mask));
  }

  public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
  {
    return GraphUpdateThreading.UnityThread;
  }

  public void UpdateAreaInit(GraphUpdateObject o)
  {
  }

  public void UpdateArea(GraphUpdateObject guo)
  {
    if (this.nodes == null)
      return;
    for (int index = 0; index < this.nodeCount; ++index)
    {
      if (guo.bounds.Contains((Vector3) this.nodes[index].position))
      {
        guo.WillUpdateNode((GraphNode) this.nodes[index]);
        guo.Apply((GraphNode) this.nodes[index]);
      }
    }
    if (!guo.updatePhysics)
      return;
    Bounds bounds = guo.bounds;
    if (this.thickRaycast)
      bounds.Expand(this.thickRaycastRadius * 2f);
    List<GraphNode> list1 = ListPool<GraphNode>.Claim();
    List<uint> list2 = ListPool<uint>.Claim();
    for (int index1 = 0; index1 < this.nodeCount; ++index1)
    {
      PointNode node1 = this.nodes[index1];
      Vector3 position1 = (Vector3) node1.position;
      List<GraphNode> graphNodeList = (List<GraphNode>) null;
      List<uint> uintList = (List<uint>) null;
      for (int index2 = 0; index2 < this.nodeCount; ++index2)
      {
        if (index2 != index1)
        {
          Vector3 position2 = (Vector3) this.nodes[index2].position;
          if (VectorMath.SegmentIntersectsBounds(bounds, position1, position2))
          {
            PointNode node2 = this.nodes[index2];
            bool flag1 = node1.ContainsConnection((GraphNode) node2);
            float dist;
            bool flag2 = this.IsValidConnection((GraphNode) node1, (GraphNode) node2, out dist);
            if (!flag1 & flag2)
            {
              if (graphNodeList == null)
              {
                list1.Clear();
                list2.Clear();
                graphNodeList = list1;
                uintList = list2;
                if (node1.connections != null)
                {
                  graphNodeList.AddRange((IEnumerable<GraphNode>) node1.connections);
                  uintList.AddRange((IEnumerable<uint>) node1.connectionCosts);
                }
              }
              uint num = (uint) Mathf.RoundToInt(dist * 1000f);
              graphNodeList.Add((GraphNode) node2);
              uintList.Add(num);
            }
            else if (flag1 && !flag2)
            {
              if (graphNodeList == null)
              {
                list1.Clear();
                list2.Clear();
                graphNodeList = list1;
                uintList = list2;
                graphNodeList.AddRange((IEnumerable<GraphNode>) node1.connections);
                uintList.AddRange((IEnumerable<uint>) node1.connectionCosts);
              }
              int index3 = graphNodeList.IndexOf((GraphNode) node2);
              if (index3 != -1)
              {
                graphNodeList.RemoveAt(index3);
                uintList.RemoveAt(index3);
              }
            }
          }
        }
      }
      if (graphNodeList != null)
      {
        node1.connections = graphNodeList.ToArray();
        node1.connectionCosts = uintList.ToArray();
      }
    }
    ListPool<GraphNode>.Release(list1);
    ListPool<uint>.Release(list2);
  }

  public override void PostDeserialization() => this.RebuildNodeLookup();

  public override void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
  {
    base.RelocateNodes(oldMatrix, newMatrix);
    this.RebuildNodeLookup();
  }

  public override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
  {
    base.DeserializeSettingsCompatibility(ctx);
    this.root = ctx.DeserializeUnityObject() as Transform;
    this.searchTag = ctx.reader.ReadString();
    this.maxDistance = ctx.reader.ReadSingle();
    this.limits = ctx.DeserializeVector3();
    this.raycast = ctx.reader.ReadBoolean();
    this.use2DPhysics = ctx.reader.ReadBoolean();
    this.thickRaycast = ctx.reader.ReadBoolean();
    this.thickRaycastRadius = ctx.reader.ReadSingle();
    this.recursive = ctx.reader.ReadBoolean();
    ctx.reader.ReadBoolean();
    this.mask = (LayerMask) ctx.reader.ReadInt32();
    this.optimizeForSparseGraph = ctx.reader.ReadBoolean();
    this.optimizeFor2D = ctx.reader.ReadBoolean();
  }

  public override void SerializeExtraInfo(GraphSerializationContext ctx)
  {
    if (this.nodes == null)
      ctx.writer.Write(-1);
    ctx.writer.Write(this.nodeCount);
    for (int index = 0; index < this.nodeCount; ++index)
    {
      if (this.nodes[index] == null)
      {
        ctx.writer.Write(-1);
      }
      else
      {
        ctx.writer.Write(0);
        this.nodes[index].SerializeNode(ctx);
      }
    }
  }

  public override void DeserializeExtraInfo(GraphSerializationContext ctx)
  {
    int length = ctx.reader.ReadInt32();
    if (length == -1)
    {
      this.nodes = (PointNode[]) null;
    }
    else
    {
      this.nodes = new PointNode[length];
      this.nodeCount = length;
      for (int index = 0; index < this.nodes.Length; ++index)
      {
        if (ctx.reader.ReadInt32() != -1)
        {
          this.nodes[index] = new PointNode(this.active);
          this.nodes[index].DeserializeNode(ctx);
        }
      }
    }
  }

  public struct GetNearestHelper
  {
    public Vector3 position;
    public float minDist;
    public float minConstDist;
    public float maxDistSqr;
    public PointNode minNode;
    public PointNode minConstNode;
    public NNConstraint constraint;
    public Dictionary<Int3, PointNode> nodeLookup;

    public GetNearestHelper(
      Vector3 position,
      float maxDistSqr,
      NNConstraint constraint,
      Dictionary<Int3, PointNode> nodeLookup)
    {
      this.position = position;
      this.maxDistSqr = maxDistSqr;
      this.constraint = constraint;
      this.nodeLookup = nodeLookup;
      this.minDist = float.PositiveInfinity;
      this.minConstDist = float.PositiveInfinity;
      this.minNode = this.minConstNode = (PointNode) null;
    }

    public void Search(Int3 p)
    {
      PointNode next;
      if (!this.nodeLookup.TryGetValue(p, out next))
        return;
      for (; next != null; next = next.next)
      {
        float sqrMagnitude = ((Vector2) this.position - (Vector2) next.position).sqrMagnitude;
        if ((double) sqrMagnitude < (double) this.minDist)
        {
          this.minDist = sqrMagnitude;
          this.minNode = next;
        }
        if (this.constraint == null || (double) sqrMagnitude < (double) this.minConstDist && (double) sqrMagnitude < (double) this.maxDistSqr && this.constraint.Suitable((GraphNode) next))
        {
          this.minConstDist = sqrMagnitude;
          this.minConstNode = next;
        }
      }
    }
  }
}
