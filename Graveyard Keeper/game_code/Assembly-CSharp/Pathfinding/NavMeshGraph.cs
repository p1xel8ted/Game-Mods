// Decompiled with JetBrains decompiler
// Type: Pathfinding.NavMeshGraph
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[JsonOptIn]
[Serializable]
public class NavMeshGraph : NavGraph, INavmesh, IUpdatableGraph, INavmeshHolder, IRaycastableGraph
{
  [JsonMember]
  public Mesh sourceMesh;
  [JsonMember]
  public Vector3 offset;
  [JsonMember]
  public Vector3 rotation;
  [JsonMember]
  public float scale = 1f;
  [JsonMember]
  public bool accurateNearestNode = true;
  public TriangleMeshNode[] nodes;
  public BBTree _bbTree;
  [NonSerialized]
  public Int3[] _vertices;
  [NonSerialized]
  public Vector3[] originalVertices;
  [NonSerialized]
  public int[] triangles;

  public TriangleMeshNode[] TriNodes => this.nodes;

  public override void GetNodes(GraphNodeDelegateCancelable del)
  {
    if (this.nodes == null)
      return;
    int index = 0;
    while (index < this.nodes.Length && del((GraphNode) this.nodes[index]))
      ++index;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex((NavGraph) this), (INavmeshHolder) null);
  }

  public Int3 GetVertex(int index) => this.vertices[index];

  public int GetVertexArrayIndex(int index) => index;

  public void GetTileCoordinates(int tileIndex, out int x, out int z) => x = z = 0;

  public BBTree bbTree
  {
    get => this._bbTree;
    set => this._bbTree = value;
  }

  public Int3[] vertices
  {
    get => this._vertices;
    set => this._vertices = value;
  }

  public void GenerateMatrix()
  {
    this.SetMatrix(Matrix4x4.TRS(this.offset, Quaternion.Euler(this.rotation), new Vector3(this.scale, this.scale, this.scale)));
  }

  public override void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
  {
    if (this.vertices == null || this.vertices.Length == 0 || this.originalVertices == null || this.originalVertices.Length != this.vertices.Length)
      return;
    for (int index = 0; index < this._vertices.Length; ++index)
      this._vertices[index] = (Int3) newMatrix.MultiplyPoint3x4(this.originalVertices[index]);
    for (int index1 = 0; index1 < this.nodes.Length; ++index1)
    {
      TriangleMeshNode node = this.nodes[index1];
      node.UpdatePositionFromVertices();
      if (node.connections != null)
      {
        for (int index2 = 0; index2 < node.connections.Length; ++index2)
          node.connectionCosts[index2] = (uint) (node.position - node.connections[index2].position).costMagnitude;
      }
    }
    this.SetMatrix(newMatrix);
    NavMeshGraph.RebuildBBTree(this);
  }

  public static NNInfo GetNearest(
    NavMeshGraph graph,
    GraphNode[] nodes,
    Vector3 position,
    NNConstraint constraint,
    bool accurateNearestNode)
  {
    if (nodes == null || nodes.Length == 0)
    {
      Debug.LogError((object) "NavGraph hasn't been generated yet or does not contain any nodes");
      return new NNInfo();
    }
    if (constraint == null)
      constraint = NNConstraint.None;
    Int3[] vertices = graph.vertices;
    if (graph.bbTree == null)
      return NavMeshGraph.GetNearestForce((NavGraph) graph, (INavmeshHolder) graph, position, constraint, accurateNearestNode);
    Rect size = graph.bbTree.Size;
    double width = (double) size.width;
    size = graph.bbTree.Size;
    double height = (double) size.height;
    float radius = (float) ((width + height) * 0.5 * 0.019999999552965164);
    NNInfo nearest = graph.bbTree.QueryCircle(position, radius, constraint);
    if (nearest.node == null)
    {
      for (int index = 1; index <= 8; ++index)
      {
        nearest = graph.bbTree.QueryCircle(position, (float) (index * index) * radius, constraint);
        if (nearest.node != null || (double) ((index - 1) * (index - 1)) * (double) radius > (double) AstarPath.active.maxNearestNodeDistance * 2.0)
          break;
      }
    }
    if (nearest.node != null)
      nearest.clampedPosition = NavMeshGraph.ClosestPointOnNode(nearest.node as TriangleMeshNode, vertices, position);
    if (nearest.constrainedNode != null)
    {
      if (constraint.constrainDistance && (double) ((Vector3) nearest.constrainedNode.position - position).sqrMagnitude > (double) AstarPath.active.maxNearestNodeDistanceSqr)
        nearest.constrainedNode = (GraphNode) null;
      else
        nearest.constClampedPosition = NavMeshGraph.ClosestPointOnNode(nearest.constrainedNode as TriangleMeshNode, vertices, position);
    }
    return nearest;
  }

  public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
  {
    return NavMeshGraph.GetNearest(this, (GraphNode[]) this.nodes, position, constraint, this.accurateNearestNode);
  }

  public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
  {
    return NavMeshGraph.GetNearestForce((NavGraph) this, (INavmeshHolder) this, position, constraint, this.accurateNearestNode);
  }

  public static NNInfo GetNearestForce(
    NavGraph graph,
    INavmeshHolder navmesh,
    Vector3 position,
    NNConstraint constraint,
    bool accurateNearestNode)
  {
    NNInfo nearestForceBoth = NavMeshGraph.GetNearestForceBoth(graph, navmesh, position, constraint, accurateNearestNode);
    nearestForceBoth.node = nearestForceBoth.constrainedNode;
    nearestForceBoth.clampedPosition = nearestForceBoth.constClampedPosition;
    return nearestForceBoth;
  }

  public static NNInfo GetNearestForceBoth(
    NavGraph graph,
    INavmeshHolder navmesh,
    Vector3 position,
    NNConstraint constraint,
    bool accurateNearestNode)
  {
    Int3 pos = (Int3) position;
    float minDist = -1f;
    GraphNode minNode = (GraphNode) null;
    float minConstDist = -1f;
    GraphNode minConstNode = (GraphNode) null;
    float maxDistSqr = constraint.constrainDistance ? AstarPath.active.maxNearestNodeDistanceSqr : float.PositiveInfinity;
    GraphNodeDelegateCancelable del = (GraphNodeDelegateCancelable) (_node =>
    {
      TriangleMeshNode node = _node as TriangleMeshNode;
      if (accurateNearestNode)
      {
        float sqrMagnitude = ((Vector3) pos - node.ClosestPointOnNode(position)).sqrMagnitude;
        if (minNode == null || (double) sqrMagnitude < (double) minDist)
        {
          minDist = sqrMagnitude;
          minNode = (GraphNode) node;
        }
        if ((double) sqrMagnitude < (double) maxDistSqr && constraint.Suitable((GraphNode) node) && (minConstNode == null || (double) sqrMagnitude < (double) minConstDist))
        {
          minConstDist = sqrMagnitude;
          minConstNode = (GraphNode) node;
        }
      }
      else if (!node.ContainsPoint((Int3) position))
      {
        float sqrMagnitude = (node.position - pos).sqrMagnitude;
        if (minNode == null || (double) sqrMagnitude < (double) minDist)
        {
          minDist = sqrMagnitude;
          minNode = (GraphNode) node;
        }
        if ((double) sqrMagnitude < (double) maxDistSqr && constraint.Suitable((GraphNode) node) && (minConstNode == null || (double) sqrMagnitude < (double) minConstDist))
        {
          minConstDist = sqrMagnitude;
          minConstNode = (GraphNode) node;
        }
      }
      else
      {
        int num = Math.Abs(node.position.y - pos.y);
        if (minNode == null || (double) num < (double) minDist)
        {
          minDist = (float) num;
          minNode = (GraphNode) node;
        }
        if ((double) num < (double) maxDistSqr && constraint.Suitable((GraphNode) node) && (minConstNode == null || (double) num < (double) minConstDist))
        {
          minConstDist = (float) num;
          minConstNode = (GraphNode) node;
        }
      }
      return true;
    });
    graph.GetNodes(del);
    NNInfo nearestForceBoth = new NNInfo(minNode);
    if (nearestForceBoth.node != null)
    {
      Vector3 vector3 = (nearestForceBoth.node as TriangleMeshNode).ClosestPointOnNode(position);
      nearestForceBoth.clampedPosition = vector3;
    }
    nearestForceBoth.constrainedNode = minConstNode;
    if (nearestForceBoth.constrainedNode != null)
    {
      Vector3 vector3 = (nearestForceBoth.constrainedNode as TriangleMeshNode).ClosestPointOnNode(position);
      nearestForceBoth.constClampedPosition = vector3;
    }
    return nearestForceBoth;
  }

  public bool Linecast(Vector3 origin, Vector3 end)
  {
    return this.Linecast(origin, end, this.GetNearest(origin, NNConstraint.None).node);
  }

  public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit)
  {
    return NavMeshGraph.Linecast((INavmesh) this, origin, end, hint, out hit, (List<GraphNode>) null);
  }

  public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint)
  {
    return NavMeshGraph.Linecast((INavmesh) this, origin, end, hint, out GraphHitInfo _, (List<GraphNode>) null);
  }

  public bool Linecast(
    Vector3 origin,
    Vector3 end,
    GraphNode hint,
    out GraphHitInfo hit,
    List<GraphNode> trace)
  {
    return NavMeshGraph.Linecast((INavmesh) this, origin, end, hint, out hit, trace);
  }

  public static bool Linecast(
    INavmesh graph,
    Vector3 tmp_origin,
    Vector3 tmp_end,
    GraphNode hint,
    out GraphHitInfo hit)
  {
    return NavMeshGraph.Linecast(graph, tmp_origin, tmp_end, hint, out hit, (List<GraphNode>) null);
  }

  public static bool Linecast(
    INavmesh graph,
    Vector3 tmp_origin,
    Vector3 tmp_end,
    GraphNode hint,
    out GraphHitInfo hit,
    List<GraphNode> trace)
  {
    Int3 p1 = (Int3) tmp_end;
    Int3 p2 = (Int3) tmp_origin;
    hit = new GraphHitInfo();
    if (float.IsNaN(tmp_origin.x + tmp_origin.y + tmp_origin.z))
      throw new ArgumentException("origin is NaN");
    if (float.IsNaN(tmp_end.x + tmp_end.y + tmp_end.z))
      throw new ArgumentException("end is NaN");
    if (!(hint is TriangleMeshNode triangleMeshNode2) && !((graph as NavGraph).GetNearest(tmp_origin, NNConstraint.None).node is TriangleMeshNode triangleMeshNode2))
    {
      Debug.LogError((object) "Could not find a valid node to start from");
      hit.point = tmp_origin;
      return true;
    }
    if (p2 == p1)
    {
      hit.node = (GraphNode) triangleMeshNode2;
      return false;
    }
    Int3 int3 = (Int3) triangleMeshNode2.ClosestPointOnNode((Vector3) p2);
    hit.origin = (Vector3) int3;
    if (!triangleMeshNode2.Walkable)
    {
      hit.point = (Vector3) int3;
      hit.tangentOrigin = (Vector3) int3;
      return true;
    }
    List<Vector3> vector3List1 = ListPool<Vector3>.Claim();
    List<Vector3> vector3List2 = ListPool<Vector3>.Claim();
    int num = 0;
    while (true)
    {
      ++num;
      if (num <= 2000)
      {
        TriangleMeshNode triangleMeshNode3 = (TriangleMeshNode) null;
        trace?.Add((GraphNode) triangleMeshNode2);
        if (!triangleMeshNode2.ContainsPoint(p1))
        {
          for (int index = 0; index < triangleMeshNode2.connections.Length; ++index)
          {
            if ((int) triangleMeshNode2.connections[index].GraphIndex == (int) triangleMeshNode2.GraphIndex)
            {
              vector3List1.Clear();
              vector3List2.Clear();
              if (triangleMeshNode2.GetPortal(triangleMeshNode2.connections[index], vector3List1, vector3List2, false))
              {
                Vector3 vector3_1 = vector3List1[0];
                Vector3 vector3_2 = vector3List2[0];
                float factor1;
                float factor2;
                if ((VectorMath.RightXZ(vector3_1, vector3_2, hit.origin) || !VectorMath.RightXZ(vector3_1, vector3_2, tmp_end)) && VectorMath.LineIntersectionFactorXZ(vector3_1, vector3_2, hit.origin, tmp_end, out factor1, out factor2) && (double) factor2 >= 0.0 && (double) factor1 >= 0.0 && (double) factor1 <= 1.0)
                {
                  triangleMeshNode3 = triangleMeshNode2.connections[index] as TriangleMeshNode;
                  break;
                }
              }
            }
          }
          if (triangleMeshNode3 != null)
            triangleMeshNode2 = triangleMeshNode3;
          else
            goto label_25;
        }
        else
          goto label_16;
      }
      else
        break;
    }
    Debug.LogError((object) "Linecast was stuck in infinite loop. Breaking.");
    ListPool<Vector3>.Release(vector3List1);
    ListPool<Vector3>.Release(vector3List2);
    return true;
label_16:
    ListPool<Vector3>.Release(vector3List1);
    ListPool<Vector3>.Release(vector3List2);
    return false;
label_25:
    int vertexCount = triangleMeshNode2.GetVertexCount();
    for (int i = 0; i < vertexCount; ++i)
    {
      Vector3 vertex1 = (Vector3) triangleMeshNode2.GetVertex(i);
      Vector3 vertex2 = (Vector3) triangleMeshNode2.GetVertex((i + 1) % vertexCount);
      float factor1;
      float factor2;
      if ((VectorMath.RightXZ(vertex1, vertex2, hit.origin) || !VectorMath.RightXZ(vertex1, vertex2, tmp_end)) && VectorMath.LineIntersectionFactorXZ(vertex1, vertex2, hit.origin, tmp_end, out factor1, out factor2) && (double) factor2 >= 0.0 && (double) factor1 >= 0.0 && (double) factor1 <= 1.0)
      {
        Vector3 vector3 = vertex1 + (vertex2 - vertex1) * factor1;
        hit.point = vector3;
        hit.node = (GraphNode) triangleMeshNode2;
        hit.tangent = vertex2 - vertex1;
        hit.tangentOrigin = vertex1;
        ListPool<Vector3>.Release(vector3List1);
        ListPool<Vector3>.Release(vector3List2);
        return true;
      }
    }
    Debug.LogWarning((object) "Linecast failing because point not inside node, and line does not hit any edges of it");
    ListPool<Vector3>.Release(vector3List1);
    ListPool<Vector3>.Release(vector3List2);
    return false;
  }

  public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
  {
    return GraphUpdateThreading.UnityThread;
  }

  public void UpdateAreaInit(GraphUpdateObject o)
  {
  }

  public void UpdateArea(GraphUpdateObject o) => NavMeshGraph.UpdateArea(o, (INavmesh) this);

  public static void UpdateArea(GraphUpdateObject o, INavmesh graph)
  {
    Bounds bounds = o.bounds;
    Rect r = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
    IntRect r2 = new IntRect(Mathf.FloorToInt(bounds.min.x * 1000f), Mathf.FloorToInt(bounds.min.z * 1000f), Mathf.FloorToInt(bounds.max.x * 1000f), Mathf.FloorToInt(bounds.max.z * 1000f));
    Int3 a = new Int3(r2.xmin, 0, r2.ymin);
    Int3 b = new Int3(r2.xmin, 0, r2.ymax);
    Int3 c = new Int3(r2.xmax, 0, r2.ymin);
    Int3 d = new Int3(r2.xmax, 0, r2.ymax);
    int ymin = ((Int3) bounds.min).y;
    int ymax = ((Int3) bounds.max).y;
    graph.GetNodes((GraphNodeDelegateCancelable) (_node =>
    {
      TriangleMeshNode node = _node as TriangleMeshNode;
      bool flag = false;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      for (int i = 0; i < 3; ++i)
      {
        Int3 vertex = node.GetVertex(i);
        Vector3 vector3 = (Vector3) vertex;
        if (r2.Contains(vertex.x, vertex.z))
        {
          flag = true;
          break;
        }
        if ((double) vector3.x < (double) r.xMin)
          ++num1;
        if ((double) vector3.x > (double) r.xMax)
          ++num2;
        if ((double) vector3.z < (double) r.yMin)
          ++num3;
        if ((double) vector3.z > (double) r.yMax)
          ++num4;
      }
      if (!flag && (num1 == 3 || num2 == 3 || num3 == 3 || num4 == 3))
        return true;
      for (int i1 = 0; i1 < 3; ++i1)
      {
        int i2 = i1 > 1 ? 0 : i1 + 1;
        Int3 vertex1 = node.GetVertex(i1);
        Int3 vertex2 = node.GetVertex(i2);
        if (VectorMath.SegmentsIntersectXZ(a, b, vertex1, vertex2))
        {
          flag = true;
          break;
        }
        if (VectorMath.SegmentsIntersectXZ(a, c, vertex1, vertex2))
        {
          flag = true;
          break;
        }
        if (VectorMath.SegmentsIntersectXZ(c, d, vertex1, vertex2))
        {
          flag = true;
          break;
        }
        if (VectorMath.SegmentsIntersectXZ(d, b, vertex1, vertex2))
        {
          flag = true;
          break;
        }
      }
      if (flag || node.ContainsPoint(a) || node.ContainsPoint(b) || node.ContainsPoint(c) || node.ContainsPoint(d))
        flag = true;
      if (!flag)
        return true;
      int num5 = 0;
      int num6 = 0;
      for (int i = 0; i < 3; ++i)
      {
        Int3 vertex = node.GetVertex(i);
        if (vertex.y < ymin)
          ++num6;
        if (vertex.y > ymax)
          ++num5;
      }
      if (num6 == 3 || num5 == 3)
        return true;
      o.WillUpdateNode((GraphNode) node);
      o.Apply((GraphNode) node);
      return true;
    }));
  }

  public static Vector3 ClosestPointOnNode(TriangleMeshNode node, Int3[] vertices, Vector3 pos)
  {
    return Polygon.ClosestPointOnTriangle((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], (Vector3) vertices[node.v2], pos);
  }

  [Obsolete("Use TriangleMeshNode.ContainsPoint instead")]
  public bool ContainsPoint(TriangleMeshNode node, Vector3 pos)
  {
    return VectorMath.IsClockwiseXZ((Vector3) this.vertices[node.v0], (Vector3) this.vertices[node.v1], pos) && VectorMath.IsClockwiseXZ((Vector3) this.vertices[node.v1], (Vector3) this.vertices[node.v2], pos) && VectorMath.IsClockwiseXZ((Vector3) this.vertices[node.v2], (Vector3) this.vertices[node.v0], pos);
  }

  [Obsolete("Use TriangleMeshNode.ContainsPoint instead")]
  public static bool ContainsPoint(TriangleMeshNode node, Vector3 pos, Int3[] vertices)
  {
    if (!VectorMath.IsClockwiseMarginXZ((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], (Vector3) vertices[node.v2]))
      Debug.LogError((object) "Noes!");
    return VectorMath.IsClockwiseMarginXZ((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], pos) && VectorMath.IsClockwiseMarginXZ((Vector3) vertices[node.v1], (Vector3) vertices[node.v2], pos) && VectorMath.IsClockwiseMarginXZ((Vector3) vertices[node.v2], (Vector3) vertices[node.v0], pos);
  }

  public void ScanInternal(string objMeshPath)
  {
    Mesh mesh = ObjImporter.ImportFile(objMeshPath);
    if ((UnityEngine.Object) mesh == (UnityEngine.Object) null)
    {
      Debug.LogError((object) $"Couldn't read .obj file at '{objMeshPath}'");
    }
    else
    {
      this.sourceMesh = mesh;
      this.ScanInternal();
    }
  }

  public override void ScanInternal(OnScanStatus statusCallback)
  {
    if ((UnityEngine.Object) this.sourceMesh == (UnityEngine.Object) null)
      return;
    this.GenerateMatrix();
    Vector3[] vertices = this.sourceMesh.vertices;
    this.triangles = this.sourceMesh.triangles;
    TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex((NavGraph) this), (INavmeshHolder) this);
    this.GenerateNodes(vertices, this.triangles, out this.originalVertices, out this._vertices);
  }

  public void GenerateNodes(
    Vector3[] vectorVertices,
    int[] triangles,
    out Vector3[] originalVertices,
    out Int3[] vertices)
  {
    if (vectorVertices.Length == 0 || triangles.Length == 0)
    {
      originalVertices = vectorVertices;
      vertices = new Int3[0];
      this.nodes = new TriangleMeshNode[0];
    }
    else
    {
      vertices = new Int3[vectorVertices.Length];
      int length = 0;
      for (int index = 0; index < vertices.Length; ++index)
        vertices[index] = (Int3) this.matrix.MultiplyPoint3x4(vectorVertices[index]);
      Dictionary<Int3, int> dictionary1 = new Dictionary<Int3, int>();
      int[] numArray = new int[vertices.Length];
      for (int index = 0; index < vertices.Length; ++index)
      {
        if (!dictionary1.ContainsKey(vertices[index]))
        {
          numArray[length] = index;
          dictionary1.Add(vertices[index], length);
          ++length;
        }
      }
      for (int index = 0; index < triangles.Length; ++index)
      {
        Int3 key = vertices[triangles[index]];
        triangles[index] = dictionary1[key];
      }
      Int3[] int3Array = vertices;
      vertices = new Int3[length];
      originalVertices = new Vector3[length];
      for (int index = 0; index < length; ++index)
      {
        vertices[index] = int3Array[numArray[index]];
        originalVertices[index] = vectorVertices[numArray[index]];
      }
      this.nodes = new TriangleMeshNode[triangles.Length / 3];
      int graphIndex = this.active.astarData.GetGraphIndex((NavGraph) this);
      for (int index = 0; index < this.nodes.Length; ++index)
      {
        this.nodes[index] = new TriangleMeshNode(this.active);
        TriangleMeshNode node = this.nodes[index];
        node.GraphIndex = (uint) graphIndex;
        node.Penalty = this.initialPenalty;
        node.Walkable = true;
        node.v0 = triangles[index * 3];
        node.v1 = triangles[index * 3 + 1];
        node.v2 = triangles[index * 3 + 2];
        if (!VectorMath.IsClockwiseXZ(vertices[node.v0], vertices[node.v1], vertices[node.v2]))
        {
          int v0 = node.v0;
          node.v0 = node.v2;
          node.v2 = v0;
        }
        if (VectorMath.IsColinearXZ(vertices[node.v0], vertices[node.v1], vertices[node.v2]))
        {
          Debug.DrawLine((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], Color.red);
          Debug.DrawLine((Vector3) vertices[node.v1], (Vector3) vertices[node.v2], Color.red);
          Debug.DrawLine((Vector3) vertices[node.v2], (Vector3) vertices[node.v0], Color.red);
        }
        node.UpdatePositionFromVertices();
      }
      Dictionary<Int2, TriangleMeshNode> dictionary2 = new Dictionary<Int2, TriangleMeshNode>();
      int index1 = 0;
      int index2 = 0;
      for (; index1 < triangles.Length; index1 += 3)
      {
        dictionary2[new Int2(triangles[index1], triangles[index1 + 1])] = this.nodes[index2];
        dictionary2[new Int2(triangles[index1 + 1], triangles[index1 + 2])] = this.nodes[index2];
        dictionary2[new Int2(triangles[index1 + 2], triangles[index1])] = this.nodes[index2];
        ++index2;
      }
      List<MeshNode> meshNodeList = new List<MeshNode>();
      List<uint> uintList = new List<uint>();
      int num = 0;
      int index3 = 0;
      for (; num < triangles.Length; num += 3)
      {
        meshNodeList.Clear();
        uintList.Clear();
        TriangleMeshNode node = this.nodes[index3];
        for (int index4 = 0; index4 < 3; ++index4)
        {
          TriangleMeshNode triangleMeshNode;
          if (dictionary2.TryGetValue(new Int2(triangles[num + (index4 + 1) % 3], triangles[num + index4]), out triangleMeshNode))
          {
            meshNodeList.Add((MeshNode) triangleMeshNode);
            uintList.Add((uint) (node.position - triangleMeshNode.position).costMagnitude);
          }
        }
        node.connections = (GraphNode[]) meshNodeList.ToArray();
        node.connectionCosts = uintList.ToArray();
        ++index3;
      }
      NavMeshGraph.RebuildBBTree(this);
    }
  }

  public static void RebuildBBTree(NavMeshGraph graph)
  {
    BBTree bbTree = graph.bbTree ?? new BBTree();
    bbTree.RebuildFrom((MeshNode[]) graph.nodes);
    graph.bbTree = bbTree;
  }

  public void PostProcess()
  {
  }

  public override void OnDrawGizmos(bool drawNodes)
  {
    if (!drawNodes)
      return;
    Matrix4x4 matrix = this.matrix;
    this.GenerateMatrix();
    TriangleMeshNode[] nodes = this.nodes;
    if (this.nodes == null)
      return;
    if (matrix != this.matrix)
      this.RelocateNodes(matrix, this.matrix);
    PathHandler debugPathData = AstarPath.active.debugPathData;
    for (int index1 = 0; index1 < this.nodes.Length; ++index1)
    {
      TriangleMeshNode node = this.nodes[index1];
      Gizmos.color = this.NodeColor((GraphNode) node, AstarPath.active.debugPathData);
      if (node.Walkable)
      {
        if (AstarPath.active.showSearchTree && debugPathData != null && debugPathData.GetPathNode((GraphNode) node).parent != null)
        {
          Gizmos.DrawLine((Vector3) node.position, (Vector3) debugPathData.GetPathNode((GraphNode) node).parent.node.position);
        }
        else
        {
          for (int index2 = 0; index2 < node.connections.Length; ++index2)
            Gizmos.DrawLine((Vector3) node.position, Vector3.Lerp((Vector3) node.position, (Vector3) node.connections[index2].position, 0.45f));
        }
        Gizmos.color = AstarColor.MeshEdgeColor;
      }
      else
        Gizmos.color = AstarColor.UnwalkableNode;
      Gizmos.DrawLine((Vector3) this.vertices[node.v0], (Vector3) this.vertices[node.v1]);
      Gizmos.DrawLine((Vector3) this.vertices[node.v1], (Vector3) this.vertices[node.v2]);
      Gizmos.DrawLine((Vector3) this.vertices[node.v2], (Vector3) this.vertices[node.v0]);
    }
  }

  public override void DeserializeExtraInfo(GraphSerializationContext ctx)
  {
    TriangleMeshNode.SetNavmeshHolder((int) ctx.graphIndex, (INavmeshHolder) this);
    int length1 = ctx.reader.ReadInt32();
    int length2 = ctx.reader.ReadInt32();
    if (length1 == -1)
    {
      this.nodes = new TriangleMeshNode[0];
      this._vertices = new Int3[0];
      this.originalVertices = new Vector3[0];
    }
    else
    {
      this.nodes = new TriangleMeshNode[length1];
      this._vertices = new Int3[length2];
      this.originalVertices = new Vector3[length2];
      for (int index = 0; index < length2; ++index)
      {
        this._vertices[index] = ctx.DeserializeInt3();
        this.originalVertices[index] = ctx.DeserializeVector3();
      }
      this.bbTree = new BBTree();
      for (int index = 0; index < length1; ++index)
      {
        this.nodes[index] = new TriangleMeshNode(this.active);
        TriangleMeshNode node = this.nodes[index];
        node.DeserializeNode(ctx);
        node.UpdatePositionFromVertices();
      }
      this.bbTree.RebuildFrom((MeshNode[]) this.nodes);
    }
  }

  public override void SerializeExtraInfo(GraphSerializationContext ctx)
  {
    if (this.nodes == null || this.originalVertices == null || this._vertices == null || this.originalVertices.Length != this._vertices.Length)
    {
      ctx.writer.Write(-1);
      ctx.writer.Write(-1);
    }
    else
    {
      ctx.writer.Write(this.nodes.Length);
      ctx.writer.Write(this._vertices.Length);
      for (int index = 0; index < this._vertices.Length; ++index)
      {
        ctx.SerializeInt3(this._vertices[index]);
        ctx.SerializeVector3(this.originalVertices[index]);
      }
      for (int index = 0; index < this.nodes.Length; ++index)
        this.nodes[index].SerializeNode(ctx);
    }
  }

  public override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
  {
    base.DeserializeSettingsCompatibility(ctx);
    this.sourceMesh = ctx.DeserializeUnityObject() as Mesh;
    this.offset = ctx.DeserializeVector3();
    this.rotation = ctx.DeserializeVector3();
    this.scale = ctx.reader.ReadSingle();
    this.accurateNearestNode = ctx.reader.ReadBoolean();
  }
}
