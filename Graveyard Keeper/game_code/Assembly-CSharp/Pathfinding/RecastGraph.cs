// Decompiled with JetBrains decompiler
// Type: Pathfinding.RecastGraph
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using Pathfinding.Util;
using Pathfinding.Voxels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[JsonOptIn]
[Serializable]
public class RecastGraph : NavGraph, INavmesh, IRaycastableGraph, IUpdatableGraph, INavmeshHolder
{
  public bool dynamic = true;
  [JsonMember]
  public float characterRadius = 0.5f;
  [JsonMember]
  public float contourMaxError = 2f;
  [JsonMember]
  public float cellSize = 0.5f;
  [JsonMember]
  public float cellHeight = 0.01f;
  [JsonMember]
  public float walkableHeight = 2f;
  [JsonMember]
  public float walkableClimb = 0.5f;
  [JsonMember]
  public float maxSlope = 30f;
  [JsonMember]
  public float maxEdgeLength = 20f;
  [JsonMember]
  public float minRegionSize = 3f;
  [JsonMember]
  public int editorTileSize = 128 /*0x80*/;
  [JsonMember]
  public int tileSizeX = 128 /*0x80*/;
  [JsonMember]
  public int tileSizeZ = 128 /*0x80*/;
  [JsonMember]
  public bool nearestSearchOnlyXZ;
  [JsonMember]
  public bool useTiles;
  public bool scanEmptyGraph;
  [JsonMember]
  public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;
  [JsonMember]
  public bool rasterizeColliders;
  [JsonMember]
  public bool rasterizeMeshes = true;
  [JsonMember]
  public bool rasterizeTerrain = true;
  [JsonMember]
  public bool rasterizeTrees = true;
  [JsonMember]
  public float colliderRasterizeDetail = 10f;
  [JsonMember]
  public Vector3 forcedBoundsCenter;
  [JsonMember]
  public Vector3 forcedBoundsSize = new Vector3(100f, 40f, 100f);
  [JsonMember]
  public LayerMask mask = (LayerMask) -1;
  [JsonMember]
  public List<string> tagMask = new List<string>();
  [JsonMember]
  public bool showMeshOutline = true;
  [JsonMember]
  public bool showNodeConnections;
  [JsonMember]
  public bool showMeshSurface;
  [JsonMember]
  public int terrainSampleSize = 3;
  public Voxelize globalVox;
  public int tileXCount;
  public int tileZCount;
  public RecastGraph.NavmeshTile[] tiles;
  public bool batchTileUpdate;
  public List<int> batchUpdatedTiles = new List<int>();
  public const int VertexIndexMask = 4095 /*0x0FFF*/;
  public const int TileIndexMask = 524287 /*0x07FFFF*/;
  public const int TileIndexOffset = 12;
  public const int BorderVertexMask = 1;
  public const int BorderVertexOffset = 31 /*0x1F*/;
  public Dictionary<Int2, int> cachedInt2_int_dict = new Dictionary<Int2, int>();
  public Dictionary<Int3, int> cachedInt3_int_dict = new Dictionary<Int3, int>();
  public int[] BoxColliderTris = new int[36]
  {
    0,
    1,
    2,
    0,
    2,
    3,
    6,
    5,
    4,
    7,
    6,
    4,
    0,
    5,
    1,
    0,
    4,
    5,
    1,
    6,
    2,
    1,
    5,
    6,
    2,
    7,
    3,
    2,
    6,
    7,
    3,
    4,
    0,
    3,
    7,
    4
  };
  public Vector3[] BoxColliderVerts = new Vector3[8]
  {
    new Vector3(-1f, -1f, -1f),
    new Vector3(1f, -1f, -1f),
    new Vector3(1f, -1f, 1f),
    new Vector3(-1f, -1f, 1f),
    new Vector3(-1f, 1f, -1f),
    new Vector3(1f, 1f, -1f),
    new Vector3(1f, 1f, 1f),
    new Vector3(-1f, 1f, 1f)
  };
  public List<RecastGraph.CapsuleCache> capsuleCache = new List<RecastGraph.CapsuleCache>();

  public Bounds forcedBounds => new Bounds(this.forcedBoundsCenter, this.forcedBoundsSize);

  public RecastGraph.NavmeshTile GetTile(int x, int z) => this.tiles[x + z * this.tileXCount];

  public Int3 GetVertex(int index)
  {
    return this.tiles[index >> 12 & 524287 /*0x07FFFF*/].GetVertex(index);
  }

  public int GetTileIndex(int index) => index >> 12 & 524287 /*0x07FFFF*/;

  public int GetVertexArrayIndex(int index) => index & 4095 /*0x0FFF*/;

  public void GetTileCoordinates(int tileIndex, out int x, out int z)
  {
    z = tileIndex / this.tileXCount;
    x = tileIndex - z * this.tileXCount;
  }

  public RecastGraph.NavmeshTile[] GetTiles() => this.tiles;

  public Bounds GetTileBounds(IntRect rect)
  {
    return this.GetTileBounds(rect.xmin, rect.ymin, rect.Width, rect.Height);
  }

  public Bounds GetTileBounds(int x, int z, int width = 1, int depth = 1)
  {
    Bounds tileBounds = new Bounds();
    tileBounds.SetMinMax(new Vector3((float) (x * this.tileSizeX) * this.cellSize, 0.0f, (float) (z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min, new Vector3((float) ((x + width) * this.tileSizeX) * this.cellSize, this.forcedBounds.size.y, (float) ((z + depth) * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
    return tileBounds;
  }

  public Int2 GetTileCoordinates(Vector3 p)
  {
    p -= this.forcedBounds.min;
    p.x /= this.cellSize * (float) this.tileSizeX;
    p.z /= this.cellSize * (float) this.tileSizeZ;
    return new Int2((int) p.x, (int) p.z);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex((NavGraph) this), (INavmeshHolder) null);
  }

  public override void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
  {
    if (this.tiles != null)
    {
      Matrix4x4 inverse = oldMatrix.inverse;
      Matrix4x4 matrix4x4 = newMatrix * inverse;
      if (this.tiles.Length > 1)
        throw new Exception("RelocateNodes cannot be used on tiled recast graphs");
      for (int index1 = 0; index1 < this.tiles.Length; ++index1)
      {
        RecastGraph.NavmeshTile tile = this.tiles[index1];
        if (tile != null)
        {
          Int3[] verts = tile.verts;
          for (int index2 = 0; index2 < verts.Length; ++index2)
            verts[index2] = (Int3) matrix4x4.MultiplyPoint((Vector3) verts[index2]);
          for (int index3 = 0; index3 < tile.nodes.Length; ++index3)
            tile.nodes[index3].UpdatePositionFromVertices();
          tile.bbTree.RebuildFrom((MeshNode[]) tile.nodes);
        }
      }
    }
    this.SetMatrix(newMatrix);
  }

  public static RecastGraph.NavmeshTile NewEmptyTile(int x, int z)
  {
    return new RecastGraph.NavmeshTile()
    {
      x = x,
      z = z,
      w = 1,
      d = 1,
      verts = new Int3[0],
      tris = new int[0],
      nodes = new TriangleMeshNode[0],
      bbTree = new BBTree()
    };
  }

  public override void GetNodes(GraphNodeDelegateCancelable del)
  {
    if (this.tiles == null)
      return;
    for (int index1 = 0; index1 < this.tiles.Length; ++index1)
    {
      if (this.tiles[index1] != null && this.tiles[index1].x + this.tiles[index1].z * this.tileXCount == index1)
      {
        TriangleMeshNode[] nodes = this.tiles[index1].nodes;
        if (nodes != null)
        {
          int index2 = 0;
          while (index2 < nodes.Length && del((GraphNode) nodes[index2]))
            ++index2;
        }
      }
    }
  }

  [Obsolete("Use node.ClosestPointOnNode instead")]
  public Vector3 ClosestPointOnNode(TriangleMeshNode node, Vector3 pos)
  {
    return Polygon.ClosestPointOnTriangle((Vector3) this.GetVertex(node.v0), (Vector3) this.GetVertex(node.v1), (Vector3) this.GetVertex(node.v2), pos);
  }

  [Obsolete("Use node.ContainsPoint instead")]
  public bool ContainsPoint(TriangleMeshNode node, Vector3 pos)
  {
    return VectorMath.IsClockwiseXZ((Vector3) this.GetVertex(node.v0), (Vector3) this.GetVertex(node.v1), pos) && VectorMath.IsClockwiseXZ((Vector3) this.GetVertex(node.v1), (Vector3) this.GetVertex(node.v2), pos) && VectorMath.IsClockwiseXZ((Vector3) this.GetVertex(node.v2), (Vector3) this.GetVertex(node.v0), pos);
  }

  public void SnapForceBoundsToScene()
  {
    List<ExtraMesh> extraMeshes;
    this.CollectMeshes(out extraMeshes, new Bounds(Vector3.zero, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
    if (extraMeshes.Count == 0)
      return;
    Bounds bounds = extraMeshes[0].bounds;
    for (int index = 1; index < extraMeshes.Count; ++index)
      bounds.Encapsulate(extraMeshes[index].bounds);
    this.forcedBoundsCenter = bounds.center;
    this.forcedBoundsSize = bounds.size;
  }

  public void GetRecastMeshObjs(Bounds bounds, List<ExtraMesh> buffer)
  {
    List<RecastMeshObj> recastMeshObjList = ListPool<RecastMeshObj>.Claim();
    RecastMeshObj.GetAllInBounds(recastMeshObjList, bounds);
    Dictionary<Mesh, Vector3[]> dictionary1 = new Dictionary<Mesh, Vector3[]>();
    Dictionary<Mesh, int[]> dictionary2 = new Dictionary<Mesh, int[]>();
    for (int index = 0; index < recastMeshObjList.Count; ++index)
    {
      MeshFilter meshFilter = recastMeshObjList[index].GetMeshFilter();
      Renderer component = (UnityEngine.Object) meshFilter != (UnityEngine.Object) null ? meshFilter.GetComponent<Renderer>() : (Renderer) null;
      if ((UnityEngine.Object) meshFilter != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        Mesh sharedMesh = meshFilter.sharedMesh;
        ExtraMesh extraMesh = new ExtraMesh();
        extraMesh.matrix = component.localToWorldMatrix;
        extraMesh.original = meshFilter;
        extraMesh.area = recastMeshObjList[index].area;
        if (dictionary1.ContainsKey(sharedMesh))
        {
          extraMesh.vertices = dictionary1[sharedMesh];
          extraMesh.triangles = dictionary2[sharedMesh];
        }
        else
        {
          extraMesh.vertices = sharedMesh.vertices;
          extraMesh.triangles = sharedMesh.triangles;
          dictionary1[sharedMesh] = extraMesh.vertices;
          dictionary2[sharedMesh] = extraMesh.triangles;
        }
        extraMesh.bounds = component.bounds;
        buffer.Add(extraMesh);
      }
      else
      {
        Collider collider = recastMeshObjList[index].GetCollider();
        if ((UnityEngine.Object) collider == (UnityEngine.Object) null)
        {
          UnityEngine.Debug.LogError((object) $"RecastMeshObject ({recastMeshObjList[index].gameObject.name}) didn't have a collider or MeshFilter+Renderer attached");
        }
        else
        {
          ExtraMesh extraMesh = this.RasterizeCollider(collider) with
          {
            area = recastMeshObjList[index].area
          };
          if (extraMesh.vertices != null)
            buffer.Add(extraMesh);
        }
      }
    }
    this.capsuleCache.Clear();
    ListPool<RecastMeshObj>.Release(recastMeshObjList);
  }

  public static void GetSceneMeshes(
    Bounds bounds,
    List<string> tagMask,
    LayerMask layerMask,
    List<ExtraMesh> meshes)
  {
    if ((tagMask == null || tagMask.Count <= 0) && (int) layerMask == 0)
      return;
    MeshFilter[] objectsOfType = UnityEngine.Object.FindObjectsOfType(typeof (MeshFilter)) as MeshFilter[];
    List<MeshFilter> meshFilterList = new List<MeshFilter>(objectsOfType.Length / 3);
    for (int index = 0; index < objectsOfType.Length; ++index)
    {
      MeshFilter meshFilter = objectsOfType[index];
      Renderer component = meshFilter.GetComponent<Renderer>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) meshFilter.sharedMesh != (UnityEngine.Object) null && component.enabled && ((1 << meshFilter.gameObject.layer & (int) layerMask) != 0 || tagMask.Contains(meshFilter.tag)) && (UnityEngine.Object) meshFilter.GetComponent<RecastMeshObj>() == (UnityEngine.Object) null)
        meshFilterList.Add(meshFilter);
    }
    Dictionary<Mesh, Vector3[]> dictionary1 = new Dictionary<Mesh, Vector3[]>();
    Dictionary<Mesh, int[]> dictionary2 = new Dictionary<Mesh, int[]>();
    bool flag = false;
    for (int index = 0; index < meshFilterList.Count; ++index)
    {
      MeshFilter meshFilter = meshFilterList[index];
      Renderer component = meshFilter.GetComponent<Renderer>();
      if (component.isPartOfStaticBatch)
        flag = true;
      else if (component.bounds.Intersects(bounds))
      {
        Mesh sharedMesh = meshFilter.sharedMesh;
        ExtraMesh extraMesh = new ExtraMesh();
        extraMesh.matrix = component.localToWorldMatrix;
        extraMesh.original = meshFilter;
        if (dictionary1.ContainsKey(sharedMesh))
        {
          extraMesh.vertices = dictionary1[sharedMesh];
          extraMesh.triangles = dictionary2[sharedMesh];
        }
        else
        {
          extraMesh.vertices = sharedMesh.vertices;
          extraMesh.triangles = sharedMesh.triangles;
          dictionary1[sharedMesh] = extraMesh.vertices;
          dictionary2[sharedMesh] = extraMesh.triangles;
        }
        extraMesh.bounds = component.bounds;
        meshes.Add(extraMesh);
      }
      if (flag)
        UnityEngine.Debug.LogWarning((object) "Some meshes were statically batched. These meshes can not be used for navmesh calculation due to technical constraints.\nDuring runtime scripts cannot access the data of meshes which have been statically batched.\nOne way to solve this problem is to use cached startup (Save & Load tab in the inspector) to only calculate the graph when the game is not playing.");
    }
  }

  public IntRect GetTouchingTiles(Bounds b)
  {
    b.center -= this.forcedBounds.min;
    return IntRect.Intersection(new IntRect(Mathf.FloorToInt(b.min.x / ((float) this.tileSizeX * this.cellSize)), Mathf.FloorToInt(b.min.z / ((float) this.tileSizeZ * this.cellSize)), Mathf.FloorToInt(b.max.x / ((float) this.tileSizeX * this.cellSize)), Mathf.FloorToInt(b.max.z / ((float) this.tileSizeZ * this.cellSize))), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
  }

  public IntRect GetTouchingTilesRound(Bounds b)
  {
    b.center -= this.forcedBounds.min;
    return IntRect.Intersection(new IntRect(Mathf.RoundToInt(b.min.x / ((float) this.tileSizeX * this.cellSize)), Mathf.RoundToInt(b.min.z / ((float) this.tileSizeZ * this.cellSize)), Mathf.RoundToInt(b.max.x / ((float) this.tileSizeX * this.cellSize)) - 1, Mathf.RoundToInt(b.max.z / ((float) this.tileSizeZ * this.cellSize)) - 1), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
  }

  GraphUpdateThreading IUpdatableGraph.CanUpdateAsync(GraphUpdateObject o)
  {
    return !o.updatePhysics ? GraphUpdateThreading.SeparateThread : GraphUpdateThreading.SeparateAndUnityInit;
  }

  void IUpdatableGraph.UpdateAreaInit(GraphUpdateObject o)
  {
    if (!o.updatePhysics)
      return;
    if (!this.dynamic)
      throw new Exception("Recast graph must be marked as dynamic to enable graph updates");
    RelevantGraphSurface.UpdateAllPositions();
    Bounds tileBounds = this.GetTileBounds(this.GetTouchingTiles(o.bounds));
    int num = Mathf.CeilToInt(this.characterRadius / this.cellSize) + 3;
    tileBounds.Expand(new Vector3((float) num, 0.0f, (float) num) * this.cellSize * 2f);
    List<ExtraMesh> extraMeshes;
    this.CollectMeshes(out extraMeshes, tileBounds);
    Voxelize voxelize = this.globalVox;
    if (voxelize == null)
    {
      voxelize = new Voxelize(this.cellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope);
      voxelize.maxEdgeLength = this.maxEdgeLength;
      if (this.dynamic)
        this.globalVox = voxelize;
    }
    voxelize.inputExtraMeshes = extraMeshes;
  }

  void IUpdatableGraph.UpdateArea(GraphUpdateObject guo)
  {
    IntRect a = this.GetTouchingTiles(guo.bounds);
    if (!guo.updatePhysics)
    {
      for (int ymin = a.ymin; ymin <= a.ymax; ++ymin)
      {
        for (int xmin = a.xmin; xmin <= a.xmax; ++xmin)
        {
          RecastGraph.NavmeshTile tile = this.tiles[ymin * this.tileXCount + xmin];
          NavMeshGraph.UpdateArea(guo, (INavmesh) tile);
        }
      }
    }
    else
    {
      if (!this.dynamic)
        throw new Exception("Recast graph must be marked as dynamic to enable graph updates with updatePhysics = true");
      Voxelize globalVox = this.globalVox;
      if (globalVox == null)
        throw new InvalidOperationException("No Voxelizer object. UpdateAreaInit should have been called before this function.");
      for (int ymin = a.ymin; ymin <= a.ymax; ++ymin)
      {
        for (int xmin = a.xmin; xmin <= a.xmax; ++xmin)
          this.RemoveConnectionsFromTile(this.tiles[xmin + ymin * this.tileXCount]);
      }
      for (int xmin = a.xmin; xmin <= a.xmax; ++xmin)
      {
        for (int ymin = a.ymin; ymin <= a.ymax; ++ymin)
          this.BuildTileMesh(globalVox, xmin, ymin);
      }
      uint graphIndex = (uint) AstarPath.active.astarData.GetGraphIndex((NavGraph) this);
      for (int ymin = a.ymin; ymin <= a.ymax; ++ymin)
      {
        for (int xmin = a.xmin; xmin <= a.xmax; ++xmin)
        {
          foreach (GraphNode node in (GraphNode[]) this.tiles[xmin + ymin * this.tileXCount].nodes)
            node.GraphIndex = graphIndex;
        }
      }
      a = a.Expand(1);
      IntRect intRect = IntRect.Intersection(a, new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
      for (int ymin = intRect.ymin; ymin <= intRect.ymax; ++ymin)
      {
        for (int xmin = intRect.xmin; xmin <= intRect.xmax; ++xmin)
        {
          if (intRect.Contains(xmin + 1, ymin))
            this.ConnectTiles(this.tiles[xmin + ymin * this.tileXCount], this.tiles[xmin + 1 + ymin * this.tileXCount]);
          if (intRect.Contains(xmin, ymin + 1))
            this.ConnectTiles(this.tiles[xmin + ymin * this.tileXCount], this.tiles[xmin + (ymin + 1) * this.tileXCount]);
        }
      }
    }
  }

  public void ConnectTileWithNeighbours(RecastGraph.NavmeshTile tile)
  {
    if (tile.x > 0)
    {
      int num = tile.x - 1;
      for (int z = tile.z; z < tile.z + tile.d; ++z)
        this.ConnectTiles(this.tiles[num + z * this.tileXCount], tile);
    }
    if (tile.x + tile.w < this.tileXCount)
    {
      int num = tile.x + tile.w;
      for (int z = tile.z; z < tile.z + tile.d; ++z)
        this.ConnectTiles(this.tiles[num + z * this.tileXCount], tile);
    }
    if (tile.z > 0)
    {
      int num = tile.z - 1;
      for (int x = tile.x; x < tile.x + tile.w; ++x)
        this.ConnectTiles(this.tiles[x + num * this.tileXCount], tile);
    }
    if (tile.z + tile.d >= this.tileZCount)
      return;
    int num1 = tile.z + tile.d;
    for (int x = tile.x; x < tile.x + tile.w; ++x)
      this.ConnectTiles(this.tiles[x + num1 * this.tileXCount], tile);
  }

  public void RemoveConnectionsFromTile(RecastGraph.NavmeshTile tile)
  {
    if (tile.x > 0)
    {
      int num = tile.x - 1;
      for (int z = tile.z; z < tile.z + tile.d; ++z)
        this.RemoveConnectionsFromTo(this.tiles[num + z * this.tileXCount], tile);
    }
    if (tile.x + tile.w < this.tileXCount)
    {
      int num = tile.x + tile.w;
      for (int z = tile.z; z < tile.z + tile.d; ++z)
        this.RemoveConnectionsFromTo(this.tiles[num + z * this.tileXCount], tile);
    }
    if (tile.z > 0)
    {
      int num = tile.z - 1;
      for (int x = tile.x; x < tile.x + tile.w; ++x)
        this.RemoveConnectionsFromTo(this.tiles[x + num * this.tileXCount], tile);
    }
    if (tile.z + tile.d >= this.tileZCount)
      return;
    int num1 = tile.z + tile.d;
    for (int x = tile.x; x < tile.x + tile.w; ++x)
      this.RemoveConnectionsFromTo(this.tiles[x + num1 * this.tileXCount], tile);
  }

  public void RemoveConnectionsFromTo(RecastGraph.NavmeshTile a, RecastGraph.NavmeshTile b)
  {
    if (a == null || b == null || a == b)
      return;
    int num = b.x + b.z * this.tileXCount;
    for (int index1 = 0; index1 < a.nodes.Length; ++index1)
    {
      TriangleMeshNode node = a.nodes[index1];
      if (node.connections != null)
      {
        for (int index2 = 0; index2 < node.connections.Length; ++index2)
        {
          if (node.connections[index2] is TriangleMeshNode connection && (connection.GetVertexIndex(0) >> 12 & 524287 /*0x07FFFF*/) == num)
          {
            node.RemoveConnection(node.connections[index2]);
            --index2;
          }
        }
      }
    }
  }

  public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
  {
    return this.GetNearestForce(position, (NNConstraint) null);
  }

  public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
  {
    if (this.tiles == null)
      return new NNInfo();
    Vector3 vector3 = position - this.forcedBounds.min;
    int num1 = Mathf.FloorToInt(vector3.x / (this.cellSize * (float) this.tileSizeX));
    int num2 = Mathf.FloorToInt(vector3.z / (this.cellSize * (float) this.tileSizeZ));
    int num3 = Mathf.Clamp(num1, 0, this.tileXCount - 1);
    int num4 = Mathf.Clamp(num2, 0, this.tileZCount - 1);
    int num5 = Math.Max(this.tileXCount, this.tileZCount);
    NNInfo previous = new NNInfo();
    float distance = float.PositiveInfinity;
    bool flag = this.nearestSearchOnlyXZ || constraint != null && constraint.distanceXZ;
    for (int index1 = 0; index1 < num5 && (flag || (double) distance >= (double) (index1 - 1) * (double) this.cellSize * (double) Math.Max(this.tileSizeX, this.tileSizeZ)); ++index1)
    {
      int num6 = Math.Min(index1 + num4 + 1, this.tileZCount);
      for (int index2 = Math.Max(-index1 + num4, 0); index2 < num6; ++index2)
      {
        int num7 = Math.Abs(index1 - Math.Abs(index2 - num4));
        if (-num7 + num3 >= 0)
        {
          RecastGraph.NavmeshTile tile = this.tiles[-num7 + num3 + index2 * this.tileXCount];
          if (tile != null)
          {
            if (flag)
            {
              previous = tile.bbTree.QueryClosestXZ(position, constraint, ref distance, previous);
              if ((double) distance < double.PositiveInfinity)
                break;
            }
            else
              previous = tile.bbTree.QueryClosest(position, constraint, ref distance, previous);
          }
        }
        if (num7 != 0 && num7 + num3 < this.tileXCount)
        {
          RecastGraph.NavmeshTile tile = this.tiles[num7 + num3 + index2 * this.tileXCount];
          if (tile != null)
          {
            if (flag)
            {
              previous = tile.bbTree.QueryClosestXZ(position, constraint, ref distance, previous);
              if ((double) distance < double.PositiveInfinity)
                break;
            }
            else
              previous = tile.bbTree.QueryClosest(position, constraint, ref distance, previous);
          }
        }
      }
    }
    previous.node = previous.constrainedNode;
    previous.constrainedNode = (GraphNode) null;
    previous.clampedPosition = previous.constClampedPosition;
    return previous;
  }

  public GraphNode PointOnNavmesh(Vector3 position, NNConstraint constraint)
  {
    if (this.tiles == null)
      return (GraphNode) null;
    Vector3 vector3 = position - this.forcedBounds.min;
    int num1 = Mathf.FloorToInt(vector3.x / (this.cellSize * (float) this.tileSizeX));
    int num2 = Mathf.FloorToInt(vector3.z / (this.cellSize * (float) this.tileSizeZ));
    if (num1 < 0 || num2 < 0 || num1 >= this.tileXCount || num2 >= this.tileZCount)
      return (GraphNode) null;
    RecastGraph.NavmeshTile tile = this.tiles[num1 + num2 * this.tileXCount];
    return tile != null ? (GraphNode) tile.bbTree.QueryInside(position, constraint) : (GraphNode) null;
  }

  public override void ScanInternal(OnScanStatus statusCallback)
  {
    TriangleMeshNode.SetNavmeshHolder(AstarPath.active.astarData.GetGraphIndex((NavGraph) this), (INavmeshHolder) this);
    this.ScanTiledNavmesh(statusCallback);
  }

  public void ScanTiledNavmesh(OnScanStatus statusCallback) => this.ScanAllTiles(statusCallback);

  public void ScanAllTiles(OnScanStatus statusCallback)
  {
    int num1 = (int) ((double) this.forcedBounds.size.x / (double) this.cellSize + 0.5);
    int num2 = (int) ((double) this.forcedBounds.size.z / (double) this.cellSize + 0.5);
    if (!this.useTiles)
    {
      this.tileSizeX = num1;
      this.tileSizeZ = num2;
    }
    else
    {
      this.tileSizeX = this.editorTileSize;
      this.tileSizeZ = this.editorTileSize;
    }
    int num3 = (num1 + this.tileSizeX - 1) / this.tileSizeX;
    int num4 = (num2 + this.tileSizeZ - 1) / this.tileSizeZ;
    this.tileXCount = num3;
    this.tileZCount = num4;
    if (this.tileXCount * this.tileZCount > 524288 /*0x080000*/)
    {
      string[] strArray = new string[5]
      {
        "Too many tiles (",
        null,
        null,
        null,
        null
      };
      int num5 = this.tileXCount * this.tileZCount;
      strArray[1] = num5.ToString();
      strArray[2] = ") maximum is ";
      num5 = 524288 /*0x080000*/;
      strArray[3] = num5.ToString();
      strArray[4] = "\nTry disabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* inspector.";
      throw new Exception(string.Concat(strArray));
    }
    this.tiles = new RecastGraph.NavmeshTile[this.tileXCount * this.tileZCount];
    if (this.scanEmptyGraph)
    {
      for (int z = 0; z < num4; ++z)
      {
        for (int x = 0; x < num3; ++x)
          this.tiles[z * this.tileXCount + x] = RecastGraph.NewEmptyTile(x, z);
      }
    }
    else
    {
      Console.WriteLine("Collecting Meshes");
      List<ExtraMesh> extraMeshes;
      this.CollectMeshes(out extraMeshes, this.forcedBounds);
      this.walkableClimb = Mathf.Min(this.walkableClimb, this.walkableHeight);
      Voxelize vox = new Voxelize(this.cellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope);
      vox.inputExtraMeshes = extraMeshes;
      vox.maxEdgeLength = this.maxEdgeLength;
      int num6 = -1;
      Stopwatch stopwatch = Stopwatch.StartNew();
      for (int z = 0; z < num4; ++z)
      {
        for (int x = 0; x < num3; ++x)
        {
          int num7 = z * this.tileXCount + x;
          Console.WriteLine($"Generating Tile #{num7.ToString()} of {(num4 * num3).ToString()}");
          if (statusCallback != null && (num7 * 10 / this.tiles.Length > num6 || stopwatch.ElapsedMilliseconds > 2000L))
          {
            num6 = num7 * 10 / this.tiles.Length;
            stopwatch.Reset();
            stopwatch.Start();
            statusCallback(new Progress(Mathf.Lerp(0.1f, 0.9f, (float) num7 / (float) this.tiles.Length), $"Building Tile {num7.ToString()}/{this.tiles.Length.ToString()}"));
          }
          this.BuildTileMesh(vox, x, z);
        }
      }
      Console.WriteLine("Assigning Graph Indices");
      if (statusCallback != null)
        statusCallback(new Progress(0.9f, "Connecting tiles"));
      uint graphIndex = (uint) AstarPath.active.astarData.GetGraphIndex((NavGraph) this);
      this.GetNodes((GraphNodeDelegateCancelable) (n =>
      {
        n.GraphIndex = graphIndex;
        return true;
      }));
      for (int index1 = 0; index1 < num4; ++index1)
      {
        for (int index2 = 0; index2 < num3; ++index2)
        {
          Console.WriteLine($"Connecing Tile #{(index1 * this.tileXCount + index2).ToString()} of {(num4 * num3).ToString()}");
          if (index2 < num3 - 1)
            this.ConnectTiles(this.tiles[index2 + index1 * this.tileXCount], this.tiles[index2 + 1 + index1 * this.tileXCount]);
          if (index1 < num4 - 1)
            this.ConnectTiles(this.tiles[index2 + index1 * this.tileXCount], this.tiles[index2 + (index1 + 1) * this.tileXCount]);
        }
      }
    }
  }

  public void BuildTileMesh(Voxelize vox, int x, int z)
  {
    float num1 = (float) this.tileSizeX * this.cellSize;
    float num2 = (float) this.tileSizeZ * this.cellSize;
    int radius = Mathf.CeilToInt(this.characterRadius / this.cellSize);
    Vector3 min = this.forcedBounds.min;
    Vector3 max = this.forcedBounds.max;
    Bounds bounds = new Bounds();
    bounds.SetMinMax(new Vector3((float) x * num1, 0.0f, (float) z * num2) + min, new Vector3((float) (x + 1) * num1 + min.x, max.y, (float) (z + 1) * num2 + min.z));
    vox.borderSize = radius + 3;
    bounds.Expand(new Vector3((float) vox.borderSize, 0.0f, (float) vox.borderSize) * this.cellSize * 2f);
    vox.forcedBounds = bounds;
    vox.width = this.tileSizeX + vox.borderSize * 2;
    vox.depth = this.tileSizeZ + vox.borderSize * 2;
    vox.relevantGraphSurfaceMode = this.useTiles || this.relevantGraphSurfaceMode != RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile ? this.relevantGraphSurfaceMode : RecastGraph.RelevantGraphSurfaceMode.RequireForAll;
    vox.minRegionSize = Mathf.RoundToInt(this.minRegionSize / (this.cellSize * this.cellSize));
    vox.Init();
    vox.CollectMeshes();
    vox.VoxelizeInput();
    vox.FilterLedges(vox.voxelWalkableHeight, vox.voxelWalkableClimb, vox.cellSize, vox.cellHeight, vox.forcedBounds.min);
    vox.FilterLowHeightSpans(vox.voxelWalkableHeight, vox.cellSize, vox.cellHeight, vox.forcedBounds.min);
    vox.BuildCompactField();
    vox.BuildVoxelConnections();
    vox.ErodeWalkableArea(radius);
    vox.BuildDistanceField();
    vox.BuildRegions();
    VoxelContourSet cset = new VoxelContourSet();
    vox.BuildContours(this.contourMaxError, 1, cset, 1);
    VoxelMesh mesh;
    vox.BuildPolyMesh(cset, 3, out mesh);
    for (int index = 0; index < mesh.verts.Length; ++index)
      mesh.verts[index] = mesh.verts[index] * 1000 * vox.cellScale + (Int3) vox.voxelOffset;
    RecastGraph.NavmeshTile tile = this.CreateTile(vox, mesh, x, z);
    this.tiles[tile.x + tile.z * this.tileXCount] = tile;
  }

  public RecastGraph.NavmeshTile CreateTile(Voxelize vox, VoxelMesh mesh, int x, int z)
  {
    if (mesh.tris == null)
      throw new ArgumentNullException("mesh.tris");
    if (mesh.verts == null)
      throw new ArgumentNullException("mesh.verts");
    RecastGraph.NavmeshTile graph = new RecastGraph.NavmeshTile();
    graph.x = x;
    graph.z = z;
    graph.w = 1;
    graph.d = 1;
    graph.tris = mesh.tris;
    graph.verts = mesh.verts;
    graph.bbTree = new BBTree();
    if (graph.tris.Length % 3 != 0)
      throw new ArgumentException("Indices array's length must be a multiple of 3 (mesh.tris)");
    if (graph.verts.Length >= 4095 /*0x0FFF*/)
    {
      if (this.tileXCount * this.tileZCount == 1)
        throw new ArgumentException($"Too many vertices per tile (more than {4095 /*0x0FFF*/.ToString()}).\n<b>Try enabling tiling in the recast graph settings.</b>\n");
      throw new ArgumentException($"Too many vertices per tile (more than {4095 /*0x0FFF*/.ToString()}).\n<b>Try reducing tile size or enabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector</b>");
    }
    Dictionary<Int3, int> cachedInt3IntDict = this.cachedInt3_int_dict;
    cachedInt3IntDict.Clear();
    int[] numArray = new int[graph.verts.Length];
    int length1 = 0;
    for (int index = 0; index < graph.verts.Length; ++index)
    {
      if (!cachedInt3IntDict.ContainsKey(graph.verts[index]))
      {
        cachedInt3IntDict.Add(graph.verts[index], length1);
        numArray[index] = length1;
        graph.verts[length1] = graph.verts[index];
        ++length1;
      }
      else
        numArray[index] = cachedInt3IntDict[graph.verts[index]];
    }
    for (int index = 0; index < graph.tris.Length; ++index)
      graph.tris[index] = numArray[graph.tris[index]];
    Int3[] int3Array = new Int3[length1];
    for (int index = 0; index < length1; ++index)
      int3Array[index] = graph.verts[index];
    graph.verts = int3Array;
    TriangleMeshNode[] nodes = new TriangleMeshNode[graph.tris.Length / 3];
    graph.nodes = nodes;
    int length2 = AstarPath.active.astarData.graphs.Length;
    TriangleMeshNode.SetNavmeshHolder(length2, (INavmeshHolder) graph);
    int num = x + z * this.tileXCount << 12;
    for (int index = 0; index < nodes.Length; ++index)
    {
      TriangleMeshNode triangleMeshNode = new TriangleMeshNode(this.active);
      nodes[index] = triangleMeshNode;
      triangleMeshNode.GraphIndex = (uint) length2;
      triangleMeshNode.v0 = graph.tris[index * 3] | num;
      triangleMeshNode.v1 = graph.tris[index * 3 + 1] | num;
      triangleMeshNode.v2 = graph.tris[index * 3 + 2] | num;
      if (!VectorMath.IsClockwiseXZ(triangleMeshNode.GetVertex(0), triangleMeshNode.GetVertex(1), triangleMeshNode.GetVertex(2)))
      {
        int v0 = triangleMeshNode.v0;
        triangleMeshNode.v0 = triangleMeshNode.v2;
        triangleMeshNode.v2 = v0;
      }
      triangleMeshNode.Walkable = true;
      triangleMeshNode.Penalty = this.initialPenalty;
      triangleMeshNode.UpdatePositionFromVertices();
    }
    graph.bbTree.RebuildFrom((MeshNode[]) nodes);
    this.CreateNodeConnections(graph.nodes);
    TriangleMeshNode.SetNavmeshHolder(length2, (INavmeshHolder) null);
    return graph;
  }

  public void CreateNodeConnections(TriangleMeshNode[] nodes)
  {
    List<MeshNode> list1 = ListPool<MeshNode>.Claim();
    List<uint> list2 = ListPool<uint>.Claim();
    Dictionary<Int2, int> cachedInt2IntDict = this.cachedInt2_int_dict;
    cachedInt2IntDict.Clear();
    for (int index = 0; index < nodes.Length; ++index)
    {
      TriangleMeshNode node = nodes[index];
      int vertexCount = node.GetVertexCount();
      for (int i = 0; i < vertexCount; ++i)
      {
        Int2 key = new Int2(node.GetVertexIndex(i), node.GetVertexIndex((i + 1) % vertexCount));
        if (!cachedInt2IntDict.ContainsKey(key))
          cachedInt2IntDict.Add(key, index);
      }
    }
    for (int index1 = 0; index1 < nodes.Length; ++index1)
    {
      TriangleMeshNode node1 = nodes[index1];
      list1.Clear();
      list2.Clear();
      int vertexCount1 = node1.GetVertexCount();
      for (int i1 = 0; i1 < vertexCount1; ++i1)
      {
        int vertexIndex1 = node1.GetVertexIndex(i1);
        int vertexIndex2 = node1.GetVertexIndex((i1 + 1) % vertexCount1);
        int index2;
        if (cachedInt2IntDict.TryGetValue(new Int2(vertexIndex2, vertexIndex1), out index2))
        {
          TriangleMeshNode node2 = nodes[index2];
          int vertexCount2 = node2.GetVertexCount();
          for (int i2 = 0; i2 < vertexCount2; ++i2)
          {
            if (node2.GetVertexIndex(i2) == vertexIndex2 && node2.GetVertexIndex((i2 + 1) % vertexCount2) == vertexIndex1)
            {
              uint costMagnitude = (uint) (node1.position - node2.position).costMagnitude;
              list1.Add((MeshNode) node2);
              list2.Add(costMagnitude);
              break;
            }
          }
        }
      }
      node1.connections = (GraphNode[]) list1.ToArray();
      node1.connectionCosts = list2.ToArray();
    }
    ListPool<MeshNode>.Release(list1);
    ListPool<uint>.Release(list2);
  }

  public void ConnectTiles(RecastGraph.NavmeshTile tile1, RecastGraph.NavmeshTile tile2)
  {
    if (tile1 == null || tile2 == null)
      return;
    if (tile1.nodes == null)
      throw new ArgumentException("tile1 does not contain any nodes");
    if (tile2.nodes == null)
      throw new ArgumentException("tile2 does not contain any nodes");
    int num1 = Mathf.Clamp(tile2.x, tile1.x, tile1.x + tile1.w - 1);
    int num2 = Mathf.Clamp(tile1.x, tile2.x, tile2.x + tile2.w - 1);
    int num3 = Mathf.Clamp(tile2.z, tile1.z, tile1.z + tile1.d - 1);
    int num4 = Mathf.Clamp(tile1.z, tile2.z, tile2.z + tile2.d - 1);
    int num5;
    int i1;
    int val1;
    int val2;
    float num6;
    if (num1 == num2)
    {
      num5 = 2;
      i1 = 0;
      val1 = num3;
      val2 = num4;
      num6 = (float) this.tileSizeZ * this.cellSize;
    }
    else
    {
      if (num3 != num4)
        throw new ArgumentException("Tiles are not adjacent (neither x or z coordinates match)");
      num5 = 0;
      i1 = 2;
      val1 = num1;
      val2 = num2;
      num6 = (float) this.tileSizeX * this.cellSize;
    }
    if (Math.Abs(val1 - val2) != 1)
    {
      UnityEngine.Debug.Log((object) $"{tile1.x.ToString()} {tile1.z.ToString()} {tile1.w.ToString()} {tile1.d.ToString()}\n{tile2.x.ToString()} {tile2.z.ToString()} {tile2.w.ToString()} {tile2.d.ToString()}\n{num1.ToString()} {num3.ToString()} {num2.ToString()} {num4.ToString()}");
      throw new ArgumentException($"Tiles are not adjacent (tile coordinates must differ by exactly 1. Got '{val1.ToString()}' and '{val2.ToString()}')");
    }
    int num7 = (int) Math.Round(((double) Math.Max(val1, val2) * (double) num6 + (double) this.forcedBounds.min[num5]) * 1000.0);
    TriangleMeshNode[] nodes1 = tile1.nodes;
    TriangleMeshNode[] nodes2 = tile2.nodes;
    for (int index1 = 0; index1 < nodes1.Length; ++index1)
    {
      TriangleMeshNode node1 = nodes1[index1];
      int vertexCount1 = node1.GetVertexCount();
      for (int i2 = 0; i2 < vertexCount1; ++i2)
      {
        Int3 vertex1 = node1.GetVertex(i2);
        Int3 vertex2 = node1.GetVertex((i2 + 1) % vertexCount1);
        if (Math.Abs(vertex1[num5] - num7) < 2 && Math.Abs(vertex2[num5] - num7) < 2)
        {
          int num8 = Math.Min(vertex1[i1], vertex2[i1]);
          int num9 = Math.Max(vertex1[i1], vertex2[i1]);
          if (num8 != num9)
          {
            for (int index2 = 0; index2 < nodes2.Length; ++index2)
            {
              TriangleMeshNode node2 = nodes2[index2];
              int vertexCount2 = node2.GetVertexCount();
              for (int i3 = 0; i3 < vertexCount2; ++i3)
              {
                Int3 vertex3 = node2.GetVertex(i3);
                Int3 vertex4 = node2.GetVertex((i3 + 1) % vertexCount1);
                if (Math.Abs(vertex3[num5] - num7) < 2 && Math.Abs(vertex4[num5] - num7) < 2)
                {
                  int num10 = Math.Min(vertex3[i1], vertex4[i1]);
                  int num11 = Math.Max(vertex3[i1], vertex4[i1]);
                  if (num10 != num11 && num9 > num10 && num8 < num11 && (vertex1 == vertex3 && vertex2 == vertex4 || vertex1 == vertex4 && vertex2 == vertex3 || (double) VectorMath.SqrDistanceSegmentSegment((Vector3) vertex1, (Vector3) vertex2, (Vector3) vertex3, (Vector3) vertex4) < (double) this.walkableClimb * (double) this.walkableClimb))
                  {
                    uint costMagnitude = (uint) (node1.position - node2.position).costMagnitude;
                    node1.AddConnection((GraphNode) node2, costMagnitude);
                    node2.AddConnection((GraphNode) node1, costMagnitude);
                  }
                }
              }
            }
          }
        }
      }
    }
  }

  public void StartBatchTileUpdate()
  {
    this.batchTileUpdate = !this.batchTileUpdate ? true : throw new InvalidOperationException("Calling StartBatchLoad when batching is already enabled");
  }

  public void EndBatchTileUpdate()
  {
    this.batchTileUpdate = this.batchTileUpdate ? false : throw new InvalidOperationException("Calling EndBatchLoad when batching not enabled");
    int tileXcount = this.tileXCount;
    int tileZcount = this.tileZCount;
    for (int index1 = 0; index1 < tileZcount; ++index1)
    {
      for (int index2 = 0; index2 < tileXcount; ++index2)
        this.tiles[index2 + index1 * this.tileXCount].flag = false;
    }
    for (int index = 0; index < this.batchUpdatedTiles.Count; ++index)
      this.tiles[this.batchUpdatedTiles[index]].flag = true;
    for (int index3 = 0; index3 < tileZcount; ++index3)
    {
      for (int index4 = 0; index4 < tileXcount; ++index4)
      {
        if (index4 < tileXcount - 1 && (this.tiles[index4 + index3 * this.tileXCount].flag || this.tiles[index4 + 1 + index3 * this.tileXCount].flag) && this.tiles[index4 + index3 * this.tileXCount] != this.tiles[index4 + 1 + index3 * this.tileXCount])
          this.ConnectTiles(this.tiles[index4 + index3 * this.tileXCount], this.tiles[index4 + 1 + index3 * this.tileXCount]);
        if (index3 < tileZcount - 1 && (this.tiles[index4 + index3 * this.tileXCount].flag || this.tiles[index4 + (index3 + 1) * this.tileXCount].flag) && this.tiles[index4 + index3 * this.tileXCount] != this.tiles[index4 + (index3 + 1) * this.tileXCount])
          this.ConnectTiles(this.tiles[index4 + index3 * this.tileXCount], this.tiles[index4 + (index3 + 1) * this.tileXCount]);
      }
    }
    this.batchUpdatedTiles.Clear();
  }

  public void ReplaceTile(int x, int z, Int3[] verts, int[] tris, bool worldSpace)
  {
    this.ReplaceTile(x, z, 1, 1, verts, tris, worldSpace);
  }

  public void ReplaceTile(
    int x,
    int z,
    int w,
    int d,
    Int3[] verts,
    int[] tris,
    bool worldSpace)
  {
    if (x + w > this.tileXCount || z + d > this.tileZCount || x < 0 || z < 0)
      throw new ArgumentException($"Tile is placed at an out of bounds position or extends out of the graph bounds ({x.ToString()}, {z.ToString()} [{w.ToString()}, {d.ToString()}] {this.tileXCount.ToString()} {this.tileZCount.ToString()})");
    if (w < 1 || d < 1)
      throw new ArgumentException($"width and depth must be greater or equal to 1. Was {w.ToString()}, {d.ToString()}");
    for (int index1 = z; index1 < z + d; ++index1)
    {
      for (int index2 = x; index2 < x + w; ++index2)
      {
        RecastGraph.NavmeshTile tile1 = this.tiles[index2 + index1 * this.tileXCount];
        if (tile1 != null)
        {
          this.RemoveConnectionsFromTile(tile1);
          for (int index3 = 0; index3 < tile1.nodes.Length; ++index3)
            tile1.nodes[index3].Destroy();
          for (int z1 = tile1.z; z1 < tile1.z + tile1.d; ++z1)
          {
            for (int x1 = tile1.x; x1 < tile1.x + tile1.w; ++x1)
            {
              RecastGraph.NavmeshTile tile2 = this.tiles[x1 + z1 * this.tileXCount];
              if (tile2 == null || tile2 != tile1)
                throw new Exception("This should not happen");
              if (z1 < z || z1 >= z + d || x1 < x || x1 >= x + w)
              {
                this.tiles[x1 + z1 * this.tileXCount] = RecastGraph.NewEmptyTile(x1, z1);
                if (this.batchTileUpdate)
                  this.batchUpdatedTiles.Add(x1 + z1 * this.tileXCount);
              }
              else
                this.tiles[x1 + z1 * this.tileXCount] = (RecastGraph.NavmeshTile) null;
            }
          }
        }
      }
    }
    RecastGraph.NavmeshTile navmeshTile = new RecastGraph.NavmeshTile();
    navmeshTile.x = x;
    navmeshTile.z = z;
    navmeshTile.w = w;
    navmeshTile.d = d;
    navmeshTile.tris = tris;
    navmeshTile.verts = verts;
    navmeshTile.bbTree = new BBTree();
    if (navmeshTile.tris.Length % 3 != 0)
      throw new ArgumentException("Triangle array's length must be a multiple of 3 (tris)");
    if (navmeshTile.verts.Length > (int) ushort.MaxValue)
      throw new ArgumentException("Too many vertices per tile (more than 65535)");
    if (!worldSpace)
    {
      if (!Mathf.Approximately((float) ((double) (x * this.tileSizeX) * (double) this.cellSize * 1000.0), (float) Math.Round((double) (x * this.tileSizeX) * (double) this.cellSize * 1000.0)))
        UnityEngine.Debug.LogWarning((object) "Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
      if (!Mathf.Approximately((float) ((double) (z * this.tileSizeZ) * (double) this.cellSize * 1000.0), (float) Math.Round((double) (z * this.tileSizeZ) * (double) this.cellSize * 1000.0)))
        UnityEngine.Debug.LogWarning((object) "Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
      Int3 int3 = (Int3) (new Vector3((float) (x * this.tileSizeX) * this.cellSize, 0.0f, (float) (z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
      for (int index = 0; index < verts.Length; ++index)
        verts[index] += int3;
    }
    TriangleMeshNode[] nodes = new TriangleMeshNode[navmeshTile.tris.Length / 3];
    navmeshTile.nodes = nodes;
    int length = AstarPath.active.astarData.graphs.Length;
    TriangleMeshNode.SetNavmeshHolder(length, (INavmeshHolder) navmeshTile);
    int index4 = x + z * this.tileXCount << 12;
    if (navmeshTile.verts.Length > 4095 /*0x0FFF*/)
    {
      UnityEngine.Debug.LogError((object) $"Too many vertices in the tile ({navmeshTile.verts.Length.ToString()} > {4095 /*0x0FFF*/.ToString()})\nYou can enable ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector to raise this limit.");
      this.tiles[index4] = RecastGraph.NewEmptyTile(x, z);
    }
    else
    {
      for (int index5 = 0; index5 < nodes.Length; ++index5)
      {
        TriangleMeshNode triangleMeshNode = new TriangleMeshNode(this.active);
        nodes[index5] = triangleMeshNode;
        triangleMeshNode.GraphIndex = (uint) length;
        triangleMeshNode.v0 = navmeshTile.tris[index5 * 3] | index4;
        triangleMeshNode.v1 = navmeshTile.tris[index5 * 3 + 1] | index4;
        triangleMeshNode.v2 = navmeshTile.tris[index5 * 3 + 2] | index4;
        if (!VectorMath.IsClockwiseXZ(triangleMeshNode.GetVertex(0), triangleMeshNode.GetVertex(1), triangleMeshNode.GetVertex(2)))
        {
          int v0 = triangleMeshNode.v0;
          triangleMeshNode.v0 = triangleMeshNode.v2;
          triangleMeshNode.v2 = v0;
        }
        triangleMeshNode.Walkable = true;
        triangleMeshNode.Penalty = this.initialPenalty;
        triangleMeshNode.UpdatePositionFromVertices();
      }
      navmeshTile.bbTree.RebuildFrom((MeshNode[]) nodes);
      this.CreateNodeConnections(navmeshTile.nodes);
      for (int index6 = z; index6 < z + d; ++index6)
      {
        for (int index7 = x; index7 < x + w; ++index7)
          this.tiles[index7 + index6 * this.tileXCount] = navmeshTile;
      }
      if (this.batchTileUpdate)
        this.batchUpdatedTiles.Add(x + z * this.tileXCount);
      else
        this.ConnectTileWithNeighbours(navmeshTile);
      TriangleMeshNode.SetNavmeshHolder(length, (INavmeshHolder) null);
      int graphIndex = AstarPath.active.astarData.GetGraphIndex((NavGraph) this);
      for (int index8 = 0; index8 < nodes.Length; ++index8)
        nodes[index8].GraphIndex = (uint) graphIndex;
    }
  }

  public void CollectTreeMeshes(Terrain terrain, List<ExtraMesh> extraMeshes)
  {
    TerrainData terrainData = terrain.terrainData;
    for (int index = 0; index < terrainData.treeInstances.Length; ++index)
    {
      TreeInstance treeInstance = terrainData.treeInstances[index];
      TreePrototype treePrototype = terrainData.treePrototypes[treeInstance.prototypeIndex];
      if (!((UnityEngine.Object) treePrototype.prefab == (UnityEngine.Object) null))
      {
        Collider component = treePrototype.prefab.GetComponent<Collider>();
        if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        {
          ExtraMesh extraMesh = new ExtraMesh(this.BoxColliderVerts, this.BoxColliderTris, new Bounds(terrain.transform.position + Vector3.Scale(treeInstance.position, terrainData.size), new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale)), Matrix4x4.TRS(terrain.transform.position + Vector3.Scale(treeInstance.position, terrainData.size), Quaternion.identity, new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale) * 0.5f));
          extraMeshes.Add(extraMesh);
        }
        else
        {
          Vector3 pos = terrain.transform.position + Vector3.Scale(treeInstance.position, terrainData.size);
          Vector3 s = new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale);
          ExtraMesh extraMesh = this.RasterizeCollider(component, Matrix4x4.TRS(pos, Quaternion.identity, s));
          if (extraMesh.vertices != null)
          {
            extraMesh.RecalculateBounds();
            extraMeshes.Add(extraMesh);
          }
        }
      }
    }
  }

  public void CollectTerrainMeshes(Bounds bounds, bool rasterizeTrees, List<ExtraMesh> extraMeshes)
  {
    Terrain[] objectsOfType = UnityEngine.Object.FindObjectsOfType(typeof (Terrain)) as Terrain[];
    if (objectsOfType.Length == 0)
      return;
    for (int index1 = 0; index1 < objectsOfType.Length; ++index1)
    {
      TerrainData terrainData = objectsOfType[index1].terrainData;
      if (!((UnityEngine.Object) terrainData == (UnityEngine.Object) null))
      {
        Vector3 position = objectsOfType[index1].GetPosition();
        Bounds b = new Bounds(position + terrainData.size * 0.5f, terrainData.size);
        if (b.Intersects(bounds))
        {
          float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
          this.terrainSampleSize = Math.Max(this.terrainSampleSize, 1);
          int heightmapResolution1 = terrainData.heightmapResolution;
          int heightmapResolution2 = terrainData.heightmapResolution;
          int num1 = (terrainData.heightmapResolution + this.terrainSampleSize - 1) / this.terrainSampleSize + 1;
          int num2 = (terrainData.heightmapResolution + this.terrainSampleSize - 1) / this.terrainSampleSize + 1;
          Vector3[] v = new Vector3[num1 * num2];
          Vector3 heightmapScale = terrainData.heightmapScale;
          float y = terrainData.size.y;
          int val1_1 = 0;
          for (int index2 = 0; index2 < num2; ++index2)
          {
            int val1_2 = 0;
            for (int index3 = 0; index3 < num1; ++index3)
            {
              int index4 = Math.Min(val1_2, heightmapResolution1 - 1);
              int index5 = Math.Min(val1_1, heightmapResolution2 - 1);
              v[index2 * num1 + index3] = new Vector3((float) index5 * heightmapScale.x, heights[index4, index5] * y, (float) index4 * heightmapScale.z) + position;
              val1_2 += this.terrainSampleSize;
            }
            val1_1 += this.terrainSampleSize;
          }
          int[] t = new int[(num1 - 1) * (num2 - 1) * 2 * 3];
          int index6 = 0;
          for (int index7 = 0; index7 < num2 - 1; ++index7)
          {
            for (int index8 = 0; index8 < num1 - 1; ++index8)
            {
              t[index6] = index7 * num1 + index8;
              t[index6 + 1] = index7 * num1 + index8 + 1;
              t[index6 + 2] = (index7 + 1) * num1 + index8 + 1;
              int index9 = index6 + 3;
              t[index9] = index7 * num1 + index8;
              t[index9 + 1] = (index7 + 1) * num1 + index8 + 1;
              t[index9 + 2] = (index7 + 1) * num1 + index8;
              index6 = index9 + 3;
            }
          }
          extraMeshes.Add(new ExtraMesh(v, t, b));
          if (rasterizeTrees)
            this.CollectTreeMeshes(objectsOfType[index1], extraMeshes);
        }
      }
    }
  }

  public void CollectColliderMeshes(Bounds bounds, List<ExtraMesh> extraMeshes)
  {
    Collider[] objectsOfType = UnityEngine.Object.FindObjectsOfType(typeof (Collider)) as Collider[];
    if (this.tagMask != null && this.tagMask.Count > 0 || (int) this.mask != 0)
    {
      for (int index = 0; index < objectsOfType.Length; ++index)
      {
        Collider col = objectsOfType[index];
        if (((1 << col.gameObject.layer & (int) this.mask) != 0 || this.tagMask.Contains(col.tag)) && col.enabled && !col.isTrigger && col.bounds.Intersects(bounds))
        {
          ExtraMesh extraMesh = this.RasterizeCollider(col);
          if (extraMesh.vertices != null)
            extraMeshes.Add(extraMesh);
        }
      }
    }
    this.capsuleCache.Clear();
  }

  public bool CollectMeshes(out List<ExtraMesh> extraMeshes, Bounds bounds)
  {
    extraMeshes = new List<ExtraMesh>();
    if (this.rasterizeMeshes)
      RecastGraph.GetSceneMeshes(bounds, this.tagMask, this.mask, extraMeshes);
    this.GetRecastMeshObjs(bounds, extraMeshes);
    if (this.rasterizeTerrain)
      this.CollectTerrainMeshes(bounds, this.rasterizeTrees, extraMeshes);
    if (this.rasterizeColliders)
      this.CollectColliderMeshes(bounds, extraMeshes);
    if (extraMeshes.Count != 0)
      return true;
    UnityEngine.Debug.LogWarning((object) "No MeshFilters were found contained in the layers specified by the 'mask' variables");
    return false;
  }

  public ExtraMesh RasterizeCollider(Collider col)
  {
    return this.RasterizeCollider(col, col.transform.localToWorldMatrix);
  }

  public ExtraMesh RasterizeCollider(Collider col, Matrix4x4 localToWorldMatrix)
  {
    switch (col)
    {
      case BoxCollider _:
        BoxCollider boxCollider = col as BoxCollider;
        Matrix4x4 matrix4x4_1 = Matrix4x4.TRS(boxCollider.center, Quaternion.identity, boxCollider.size * 0.5f);
        Matrix4x4 matrix1 = localToWorldMatrix * matrix4x4_1;
        return new ExtraMesh(this.BoxColliderVerts, this.BoxColliderTris, boxCollider.bounds, matrix1);
      case SphereCollider _:
      case CapsuleCollider _:
        SphereCollider sphereCollider = col as SphereCollider;
        CapsuleCollider capsuleCollider = col as CapsuleCollider;
        float num1 = (UnityEngine.Object) sphereCollider != (UnityEngine.Object) null ? sphereCollider.radius : capsuleCollider.radius;
        float b = (UnityEngine.Object) sphereCollider != (UnityEngine.Object) null ? 0.0f : (float) ((double) capsuleCollider.height * 0.5 / (double) num1 - 1.0);
        Matrix4x4 matrix4x4_2 = Matrix4x4.TRS((UnityEngine.Object) sphereCollider != (UnityEngine.Object) null ? sphereCollider.center : capsuleCollider.center, Quaternion.identity, Vector3.one * num1);
        Matrix4x4 matrix2 = localToWorldMatrix * matrix4x4_2;
        int num2 = Mathf.Max(4, Mathf.RoundToInt(this.colliderRasterizeDetail * Mathf.Sqrt(matrix2.MultiplyVector(Vector3.one).magnitude)));
        if (num2 > 100)
          UnityEngine.Debug.LogWarning((object) "Very large detail for some collider meshes. Consider decreasing Collider Rasterize Detail (RecastGraph)");
        int num3 = num2;
        RecastGraph.CapsuleCache capsuleCache1 = (RecastGraph.CapsuleCache) null;
        for (int index = 0; index < this.capsuleCache.Count; ++index)
        {
          RecastGraph.CapsuleCache capsuleCache2 = this.capsuleCache[index];
          if (capsuleCache2.rows == num2 && Mathf.Approximately(capsuleCache2.height, b))
            capsuleCache1 = capsuleCache2;
        }
        if (capsuleCache1 == null)
        {
          Vector3[] vector3Array = new Vector3[num2 * num3 + 2];
          List<int> intList = new List<int>();
          vector3Array[vector3Array.Length - 1] = Vector3.up;
          for (int index1 = 0; index1 < num2; ++index1)
          {
            for (int index2 = 0; index2 < num3; ++index2)
              vector3Array[index2 + index1 * num3] = new Vector3(Mathf.Cos((float) ((double) index2 * 3.1415927410125732 * 2.0) / (float) num3) * Mathf.Sin((float) index1 * 3.14159274f / (float) (num2 - 1)), Mathf.Cos((float) index1 * 3.14159274f / (float) (num2 - 1)) + (index1 < num2 / 2 ? b : -b), Mathf.Sin((float) ((double) index2 * 3.1415927410125732 * 2.0) / (float) num3) * Mathf.Sin((float) index1 * 3.14159274f / (float) (num2 - 1)));
          }
          vector3Array[vector3Array.Length - 2] = Vector3.down;
          int num4 = 0;
          int num5 = num3 - 1;
          for (; num4 < num3; num5 = num4++)
          {
            intList.Add(vector3Array.Length - 1);
            intList.Add(num5);
            intList.Add(num4);
          }
          for (int index = 1; index < num2; ++index)
          {
            int num6 = 0;
            int num7 = num3 - 1;
            for (; num6 < num3; num7 = num6++)
            {
              intList.Add(index * num3 + num6);
              intList.Add(index * num3 + num7);
              intList.Add((index - 1) * num3 + num6);
              intList.Add((index - 1) * num3 + num7);
              intList.Add((index - 1) * num3 + num6);
              intList.Add(index * num3 + num7);
            }
          }
          int num8 = 0;
          int num9 = num3 - 1;
          for (; num8 < num3; num9 = num8++)
          {
            intList.Add(vector3Array.Length - 2);
            intList.Add((num2 - 1) * num3 + num9);
            intList.Add((num2 - 1) * num3 + num8);
          }
          capsuleCache1 = new RecastGraph.CapsuleCache();
          capsuleCache1.rows = num2;
          capsuleCache1.height = b;
          capsuleCache1.verts = vector3Array;
          capsuleCache1.tris = intList.ToArray();
          this.capsuleCache.Add(capsuleCache1);
        }
        return new ExtraMesh(capsuleCache1.verts, capsuleCache1.tris, col.bounds, matrix2);
      case MeshCollider _:
        MeshCollider meshCollider = col as MeshCollider;
        if ((UnityEngine.Object) meshCollider.sharedMesh != (UnityEngine.Object) null)
          return new ExtraMesh(meshCollider.sharedMesh.vertices, meshCollider.sharedMesh.triangles, meshCollider.bounds, localToWorldMatrix);
        break;
    }
    return new ExtraMesh();
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
    Vector3 tmp_origin,
    Vector3 tmp_end,
    GraphNode hint,
    out GraphHitInfo hit,
    List<GraphNode> trace)
  {
    return NavMeshGraph.Linecast((INavmesh) this, tmp_origin, tmp_end, hint, out hit, trace);
  }

  public override void OnDrawGizmos(bool drawNodes)
  {
    if (!drawNodes)
      return;
    Gizmos.color = Color.white;
    Gizmos.DrawWireCube(this.forcedBounds.center, this.forcedBounds.size);
    PathHandler debugData = AstarPath.active.debugPathData;
    this.GetNodes((GraphNodeDelegateCancelable) (_node =>
    {
      TriangleMeshNode node = _node as TriangleMeshNode;
      if (AstarPath.active.showSearchTree && debugData != null)
      {
        bool flag = NavGraph.InSearchTree((GraphNode) node, AstarPath.active.debugPath);
        if (flag && this.showNodeConnections && debugData.GetPathNode((GraphNode) node).parent != null)
        {
          Gizmos.color = this.NodeColor((GraphNode) node, debugData);
          Gizmos.DrawLine((Vector3) node.position, (Vector3) debugData.GetPathNode((GraphNode) node).parent.node.position);
        }
        if (this.showMeshOutline)
        {
          Gizmos.color = node.Walkable ? this.NodeColor((GraphNode) node, debugData) : AstarColor.UnwalkableNode;
          if (!flag)
            Gizmos.color *= new Color(1f, 1f, 1f, 0.1f);
          Gizmos.DrawLine((Vector3) node.GetVertex(0), (Vector3) node.GetVertex(1));
          Gizmos.DrawLine((Vector3) node.GetVertex(1), (Vector3) node.GetVertex(2));
          Gizmos.DrawLine((Vector3) node.GetVertex(2), (Vector3) node.GetVertex(0));
        }
      }
      else
      {
        if (this.showNodeConnections)
        {
          Gizmos.color = this.NodeColor((GraphNode) node, (PathHandler) null);
          for (int index = 0; index < node.connections.Length; ++index)
            Gizmos.DrawLine((Vector3) node.position, Vector3.Lerp((Vector3) node.connections[index].position, (Vector3) node.position, 0.4f));
        }
        if (this.showMeshOutline)
        {
          Gizmos.color = node.Walkable ? this.NodeColor((GraphNode) node, debugData) : AstarColor.UnwalkableNode;
          Gizmos.DrawLine((Vector3) node.GetVertex(0), (Vector3) node.GetVertex(1));
          Gizmos.DrawLine((Vector3) node.GetVertex(1), (Vector3) node.GetVertex(2));
          Gizmos.DrawLine((Vector3) node.GetVertex(2), (Vector3) node.GetVertex(0));
        }
      }
      return true;
    }));
  }

  public override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
  {
    base.DeserializeSettingsCompatibility(ctx);
    this.characterRadius = ctx.reader.ReadSingle();
    this.contourMaxError = ctx.reader.ReadSingle();
    this.cellSize = ctx.reader.ReadSingle();
    this.cellHeight = ctx.reader.ReadSingle();
    this.walkableHeight = ctx.reader.ReadSingle();
    this.maxSlope = ctx.reader.ReadSingle();
    this.maxEdgeLength = ctx.reader.ReadSingle();
    this.editorTileSize = ctx.reader.ReadInt32();
    this.tileSizeX = ctx.reader.ReadInt32();
    this.nearestSearchOnlyXZ = ctx.reader.ReadBoolean();
    this.useTiles = ctx.reader.ReadBoolean();
    this.relevantGraphSurfaceMode = (RecastGraph.RelevantGraphSurfaceMode) ctx.reader.ReadInt32();
    this.rasterizeColliders = ctx.reader.ReadBoolean();
    this.rasterizeMeshes = ctx.reader.ReadBoolean();
    this.rasterizeTerrain = ctx.reader.ReadBoolean();
    this.rasterizeTrees = ctx.reader.ReadBoolean();
    this.colliderRasterizeDetail = ctx.reader.ReadSingle();
    this.forcedBoundsCenter = ctx.DeserializeVector3();
    this.forcedBoundsSize = ctx.DeserializeVector3();
    this.mask = (LayerMask) ctx.reader.ReadInt32();
    int capacity = ctx.reader.ReadInt32();
    this.tagMask = new List<string>(capacity);
    for (int index = 0; index < capacity; ++index)
      this.tagMask.Add(ctx.reader.ReadString());
    this.showMeshOutline = ctx.reader.ReadBoolean();
    this.showNodeConnections = ctx.reader.ReadBoolean();
    this.terrainSampleSize = ctx.reader.ReadInt32();
    this.walkableClimb = ctx.DeserializeFloat(this.walkableClimb);
    this.minRegionSize = ctx.DeserializeFloat(this.minRegionSize);
    this.tileSizeZ = ctx.DeserializeInt(this.tileSizeX);
    this.showMeshSurface = ctx.reader.ReadBoolean();
  }

  public override void SerializeExtraInfo(GraphSerializationContext ctx)
  {
    BinaryWriter writer = ctx.writer;
    if (this.tiles == null)
    {
      writer.Write(-1);
    }
    else
    {
      writer.Write(this.tileXCount);
      writer.Write(this.tileZCount);
      for (int index1 = 0; index1 < this.tileZCount; ++index1)
      {
        for (int index2 = 0; index2 < this.tileXCount; ++index2)
        {
          RecastGraph.NavmeshTile tile = this.tiles[index2 + index1 * this.tileXCount];
          if (tile == null)
            throw new Exception("NULL Tile");
          writer.Write(tile.x);
          writer.Write(tile.z);
          if (tile.x == index2 && tile.z == index1)
          {
            writer.Write(tile.w);
            writer.Write(tile.d);
            writer.Write(tile.tris.Length);
            for (int index3 = 0; index3 < tile.tris.Length; ++index3)
              writer.Write(tile.tris[index3]);
            writer.Write(tile.verts.Length);
            for (int index4 = 0; index4 < tile.verts.Length; ++index4)
              ctx.SerializeInt3(tile.verts[index4]);
            writer.Write(tile.nodes.Length);
            for (int index5 = 0; index5 < tile.nodes.Length; ++index5)
              tile.nodes[index5].SerializeNode(ctx);
          }
        }
      }
    }
  }

  public override void DeserializeExtraInfo(GraphSerializationContext ctx)
  {
    BinaryReader reader = ctx.reader;
    this.tileXCount = reader.ReadInt32();
    if (this.tileXCount < 0)
      return;
    this.tileZCount = reader.ReadInt32();
    this.tiles = new RecastGraph.NavmeshTile[this.tileXCount * this.tileZCount];
    TriangleMeshNode.SetNavmeshHolder((int) ctx.graphIndex, (INavmeshHolder) this);
    for (int index1 = 0; index1 < this.tileZCount; ++index1)
    {
      for (int index2 = 0; index2 < this.tileXCount; ++index2)
      {
        int index3 = index2 + index1 * this.tileXCount;
        int num1 = reader.ReadInt32();
        if (num1 < 0)
          throw new Exception("Invalid tile coordinates (x < 0)");
        int num2 = reader.ReadInt32();
        if (num2 < 0)
          throw new Exception("Invalid tile coordinates (z < 0)");
        if (num1 != index2 || num2 != index1)
        {
          this.tiles[index3] = this.tiles[num2 * this.tileXCount + num1];
        }
        else
        {
          RecastGraph.NavmeshTile navmeshTile = new RecastGraph.NavmeshTile();
          navmeshTile.x = num1;
          navmeshTile.z = num2;
          navmeshTile.w = reader.ReadInt32();
          navmeshTile.d = reader.ReadInt32();
          navmeshTile.bbTree = new BBTree();
          this.tiles[index3] = navmeshTile;
          int length1 = reader.ReadInt32();
          navmeshTile.tris = length1 % 3 == 0 ? new int[length1] : throw new Exception("Corrupt data. Triangle indices count must be divisable by 3. Got " + length1.ToString());
          for (int index4 = 0; index4 < navmeshTile.tris.Length; ++index4)
            navmeshTile.tris[index4] = reader.ReadInt32();
          navmeshTile.verts = new Int3[reader.ReadInt32()];
          for (int index5 = 0; index5 < navmeshTile.verts.Length; ++index5)
            navmeshTile.verts[index5] = ctx.DeserializeInt3();
          int length2 = reader.ReadInt32();
          navmeshTile.nodes = new TriangleMeshNode[length2];
          int num3 = index3 << 12;
          for (int index6 = 0; index6 < navmeshTile.nodes.Length; ++index6)
          {
            TriangleMeshNode triangleMeshNode = new TriangleMeshNode(this.active);
            navmeshTile.nodes[index6] = triangleMeshNode;
            triangleMeshNode.DeserializeNode(ctx);
            triangleMeshNode.v0 = navmeshTile.tris[index6 * 3] | num3;
            triangleMeshNode.v1 = navmeshTile.tris[index6 * 3 + 1] | num3;
            triangleMeshNode.v2 = navmeshTile.tris[index6 * 3 + 2] | num3;
            triangleMeshNode.UpdatePositionFromVertices();
          }
          navmeshTile.bbTree.RebuildFrom((MeshNode[]) navmeshTile.nodes);
        }
      }
    }
  }

  public enum RelevantGraphSurfaceMode
  {
    DoNotRequire,
    OnlyForCompletelyInsideTile,
    RequireForAll,
  }

  public class NavmeshTile : INavmeshHolder, INavmesh
  {
    public int[] tris;
    public Int3[] verts;
    public int x;
    public int z;
    public int w;
    public int d;
    public TriangleMeshNode[] nodes;
    public BBTree bbTree;
    public bool flag;

    public void GetTileCoordinates(int tileIndex, out int x, out int z)
    {
      x = this.x;
      z = this.z;
    }

    public int GetVertexArrayIndex(int index) => index & 4095 /*0x0FFF*/;

    public Int3 GetVertex(int index) => this.verts[index & 4095 /*0x0FFF*/];

    public void GetNodes(GraphNodeDelegateCancelable del)
    {
      if (this.nodes == null)
        return;
      int index = 0;
      while (index < this.nodes.Length && del((GraphNode) this.nodes[index]))
        ++index;
    }
  }

  public struct SceneMesh
  {
    public Mesh mesh;
    public Matrix4x4 matrix;
    public Bounds bounds;
  }

  public class CapsuleCache
  {
    public int rows;
    public float height;
    public Vector3[] verts;
    public int[] tris;
  }
}
