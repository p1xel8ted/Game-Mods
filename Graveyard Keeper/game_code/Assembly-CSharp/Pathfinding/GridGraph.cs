// Decompiled with JetBrains decompiler
// Type: Pathfinding.GridGraph
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
public class GridGraph : NavGraph, IUpdatableGraph, IRaycastableGraph
{
  public int width;
  public int depth;
  [JsonMember]
  public float aspectRatio = 1f;
  [JsonMember]
  public float isometricAngle;
  [JsonMember]
  public bool uniformEdgeCosts;
  [JsonMember]
  public Vector3 rotation;
  [JsonMember]
  public Vector3 center;
  [JsonMember]
  public Vector2 unclampedSize;
  [JsonMember]
  public float nodeSize = 1f;
  [JsonMember]
  public GraphCollision collision;
  [JsonMember]
  public float maxClimb = 0.4f;
  [JsonMember]
  public int maxClimbAxis = 1;
  [JsonMember]
  public float maxSlope = 90f;
  [JsonMember]
  public int erodeIterations;
  [JsonMember]
  public bool erosionUseTags;
  [JsonMember]
  public int erosionFirstTag = 1;
  [JsonMember]
  public bool autoLinkGrids;
  [JsonMember]
  public float autoLinkDistLimit = 10f;
  [JsonMember]
  public NumNeighbours neighbours = NumNeighbours.Eight;
  [JsonMember]
  public bool cutCorners = true;
  [JsonMember]
  public float penaltyPositionOffset;
  [JsonMember]
  public bool penaltyPosition;
  [JsonMember]
  public float penaltyPositionFactor = 1f;
  [JsonMember]
  public bool penaltyAngle;
  [JsonMember]
  public float penaltyAngleFactor = 100f;
  [JsonMember]
  public float penaltyAnglePower = 1f;
  [JsonMember]
  public bool useJumpPointSearch;
  [JsonMember]
  public GridGraph.TextureData textureData = new GridGraph.TextureData();
  [CompilerGenerated]
  public Vector2 \u003Csize\u003Ek__BackingField;
  [NonSerialized]
  public int[] neighbourOffsets = new int[8];
  [NonSerialized]
  public uint[] neighbourCosts = new uint[8];
  [NonSerialized]
  public int[] neighbourXOffsets = new int[8];
  [NonSerialized]
  public int[] neighbourZOffsets = new int[8];
  public static int[] hexagonNeighbourIndices = new int[6]
  {
    0,
    1,
    2,
    3,
    5,
    7
  };
  public const int getNearestForceOverlap = 2;
  [CompilerGenerated]
  public Matrix4x4 \u003CboundsMatrix\u003Ek__BackingField;
  public GridNode[] nodes;

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.RemoveGridGraphFromStatic();
  }

  public void RemoveGridGraphFromStatic()
  {
    GridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex((NavGraph) this), (GridGraph) null);
  }

  public virtual bool uniformWidthDepthGrid => true;

  public override int CountNodes() => this.nodes.Length;

  public override void GetNodes(GraphNodeDelegateCancelable del)
  {
    if (this.nodes == null)
      return;
    int index = 0;
    while (index < this.nodes.Length && del((GraphNode) this.nodes[index]))
      ++index;
  }

  public bool useRaycastNormal => (double) Math.Abs(90f - this.maxSlope) > 1.4012984643248171E-45;

  public Vector2 size
  {
    get => this.\u003Csize\u003Ek__BackingField;
    set => this.\u003Csize\u003Ek__BackingField = value;
  }

  public Matrix4x4 boundsMatrix
  {
    get => this.\u003CboundsMatrix\u003Ek__BackingField;
    set => this.\u003CboundsMatrix\u003Ek__BackingField = value;
  }

  public GridGraph()
  {
    this.unclampedSize = new Vector2(10f, 10f);
    this.nodeSize = 1f;
    this.collision = new GraphCollision();
  }

  public void RelocateNodes(
    Vector3 center,
    Quaternion rotation,
    float nodeSize,
    float aspectRatio = 1f,
    float isometricAngle = 0.0f)
  {
    Matrix4x4 matrix = this.matrix;
    this.center = center;
    this.rotation = rotation.eulerAngles;
    this.nodeSize = nodeSize;
    this.aspectRatio = aspectRatio;
    this.isometricAngle = isometricAngle;
    this.UpdateSizeFromWidthDepth();
    this.RelocateNodes(matrix, this.matrix);
  }

  public Int3 GraphPointToWorld(int x, int z, float height)
  {
    return (Int3) this.matrix.MultiplyPoint3x4(new Vector3((float) x + 0.5f, height, (float) z + 0.5f));
  }

  public int Width
  {
    get => this.width;
    set => this.width = value;
  }

  public int Depth
  {
    get => this.depth;
    set => this.depth = value;
  }

  public uint GetConnectionCost(int dir) => this.neighbourCosts[dir];

  public GridNode GetNodeConnection(GridNode node, int dir)
  {
    if (!node.GetConnectionInternal(dir))
      return (GridNode) null;
    if (!node.EdgeNode)
      return this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir]];
    int nodeInGridIndex = node.NodeInGridIndex;
    int z = nodeInGridIndex / this.Width;
    int x = nodeInGridIndex - z * this.Width;
    return this.GetNodeConnection(nodeInGridIndex, x, z, dir);
  }

  public bool HasNodeConnection(GridNode node, int dir)
  {
    if (!node.GetConnectionInternal(dir))
      return false;
    if (!node.EdgeNode)
      return true;
    int nodeInGridIndex = node.NodeInGridIndex;
    int z = nodeInGridIndex / this.Width;
    int x = nodeInGridIndex - z * this.Width;
    return this.HasNodeConnection(nodeInGridIndex, x, z, dir);
  }

  public void SetNodeConnection(GridNode node, int dir, bool value)
  {
    int nodeInGridIndex = node.NodeInGridIndex;
    int z = nodeInGridIndex / this.Width;
    int x = nodeInGridIndex - z * this.Width;
    this.SetNodeConnection(nodeInGridIndex, x, z, dir, value);
  }

  public GridNode GetNodeConnection(int index, int x, int z, int dir)
  {
    if (!this.nodes[index].GetConnectionInternal(dir))
      return (GridNode) null;
    int num1 = x + this.neighbourXOffsets[dir];
    if (num1 < 0 || num1 >= this.Width)
      return (GridNode) null;
    int num2 = z + this.neighbourZOffsets[dir];
    return num2 < 0 || num2 >= this.Depth ? (GridNode) null : this.nodes[index + this.neighbourOffsets[dir]];
  }

  public void SetNodeConnection(int index, int x, int z, int dir, bool value)
  {
    this.nodes[index].SetConnectionInternal(dir, value);
  }

  public bool HasNodeConnection(int index, int x, int z, int dir)
  {
    if (!this.nodes[index].GetConnectionInternal(dir))
      return false;
    int num1 = x + this.neighbourXOffsets[dir];
    if (num1 < 0 || num1 >= this.Width)
      return false;
    int num2 = z + this.neighbourZOffsets[dir];
    return num2 >= 0 && num2 < this.Depth;
  }

  public void UpdateSizeFromWidthDepth()
  {
    this.unclampedSize = new Vector2((float) this.width, (float) this.depth) * this.nodeSize;
    this.GenerateMatrix();
  }

  public void GenerateMatrix()
  {
    Vector2 unclampedSize = this.unclampedSize;
    unclampedSize.x *= Mathf.Sign(unclampedSize.x);
    unclampedSize.y *= Mathf.Sign(unclampedSize.y);
    this.nodeSize = Mathf.Clamp(this.nodeSize, unclampedSize.x / 1024f, float.PositiveInfinity);
    this.nodeSize = Mathf.Clamp(this.nodeSize, unclampedSize.y / 1024f, float.PositiveInfinity);
    unclampedSize.x = (double) unclampedSize.x < (double) this.nodeSize ? this.nodeSize : unclampedSize.x;
    unclampedSize.y = (double) unclampedSize.y < (double) this.nodeSize ? this.nodeSize : unclampedSize.y;
    this.size = unclampedSize;
    Matrix4x4 matrix4x4_1 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 45f, 0.0f), Vector3.one);
    Matrix4x4 matrix4x4_2 = Matrix4x4.Scale(new Vector3(Mathf.Cos((float) Math.PI / 180f * this.isometricAngle), 1f, 1f)) * matrix4x4_1;
    Matrix4x4 matrix4x4_3 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, -45f, 0.0f), Vector3.one) * matrix4x4_2;
    this.boundsMatrix = Matrix4x4.TRS(this.center, Quaternion.Euler(this.rotation), new Vector3(this.aspectRatio, 1f, 1f)) * matrix4x4_3;
    this.width = Mathf.FloorToInt(this.size.x / this.nodeSize);
    this.depth = Mathf.FloorToInt(this.size.y / this.nodeSize);
    if (Mathf.Approximately(this.size.x / this.nodeSize, (float) Mathf.CeilToInt(this.size.x / this.nodeSize)))
      this.width = Mathf.CeilToInt(this.size.x / this.nodeSize);
    if (Mathf.Approximately(this.size.y / this.nodeSize, (float) Mathf.CeilToInt(this.size.y / this.nodeSize)))
      this.depth = Mathf.CeilToInt(this.size.y / this.nodeSize);
    this.SetMatrix(Matrix4x4.TRS(this.boundsMatrix.MultiplyPoint3x4(-new Vector3(this.size.x, 0.0f, this.size.y) * 0.5f), Quaternion.Euler(this.rotation), new Vector3(this.nodeSize * this.aspectRatio, 1f, this.nodeSize)) * matrix4x4_3);
  }

  public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
  {
    if (this.nodes == null || this.depth * this.width != this.nodes.Length)
      return new NNInfo();
    position = this.inverseMatrix.MultiplyPoint3x4(position);
    float f1 = position.x - 0.5f;
    float f2 = position.z - 0.5f;
    int num1 = Mathf.Clamp(Mathf.RoundToInt(f1), 0, this.width - 1);
    int num2 = Mathf.Clamp(Mathf.RoundToInt(f2), 0, this.depth - 1);
    NNInfo nearest = new NNInfo((GraphNode) this.nodes[num2 * this.width + num1]);
    float y = this.inverseMatrix.MultiplyPoint3x4((Vector3) this.nodes[num2 * this.width + num1].position).y;
    nearest.clampedPosition = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) num1 - 0.5f, (float) num1 + 0.5f) + 0.5f, y, Mathf.Clamp(f2, (float) num2 - 0.5f, (float) num2 + 0.5f) + 0.5f));
    return nearest;
  }

  public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
  {
    if (this.nodes == null || this.depth * this.width != this.nodes.Length)
      return new NNInfo();
    Vector3 vector3_1 = position;
    position = this.inverseMatrix.MultiplyPoint3x4(position);
    float f1 = position.x - 0.5f;
    float f2 = position.z - 0.5f;
    int num1 = Mathf.Clamp(Mathf.RoundToInt(f1), 0, this.width - 1);
    int num2 = Mathf.Clamp(Mathf.RoundToInt(f2), 0, this.depth - 1);
    GridNode node = this.nodes[num1 + num2 * this.width];
    GridNode gridNode = (GridNode) null;
    float num3 = float.PositiveInfinity;
    int num4 = 2;
    Vector3 vector3_2 = Vector3.zero;
    NNInfo nearestForce = new NNInfo((GraphNode) null);
    if (constraint.Suitable((GraphNode) node))
    {
      gridNode = node;
      num3 = ((Vector3) gridNode.position - vector3_1).sqrMagnitude;
      float y = this.inverseMatrix.MultiplyPoint3x4((Vector3) node.position).y;
      vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) num1 - 0.5f, (float) num1 + 0.5f) + 0.5f, y, Mathf.Clamp(f2, (float) num2 - 0.5f, (float) num2 + 0.5f) + 0.5f));
    }
    if (gridNode != null)
    {
      nearestForce.node = (GraphNode) gridNode;
      nearestForce.clampedPosition = vector3_2;
      if (num4 == 0)
        return nearestForce;
      --num4;
    }
    float num5 = constraint.constrainDistance ? AstarPath.active.maxNearestNodeDistance : float.PositiveInfinity;
    float num6 = num5 * num5;
    for (int index1 = 1; (double) this.nodeSize * (double) index1 <= (double) num5; ++index1)
    {
      bool flag = false;
      int num7 = num2 + index1;
      int num8 = num7 * this.width;
      Vector3 vector3_3;
      for (int index2 = num1 - index1; index2 <= num1 + index1; ++index2)
      {
        if (index2 >= 0 && num7 >= 0 && index2 < this.width && num7 < this.depth)
        {
          flag = true;
          if (constraint.Suitable((GraphNode) this.nodes[index2 + num8]))
          {
            vector3_3 = (Vector3) this.nodes[index2 + num8].position - vector3_1;
            float sqrMagnitude = vector3_3.sqrMagnitude;
            if ((double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num6)
            {
              num3 = sqrMagnitude;
              gridNode = this.nodes[index2 + num8];
              vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) index2 - 0.5f, (float) index2 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode.position).y, Mathf.Clamp(f2, (float) num7 - 0.5f, (float) num7 + 0.5f) + 0.5f));
            }
          }
        }
      }
      int num9 = num2 - index1;
      int num10 = num9 * this.width;
      for (int index3 = num1 - index1; index3 <= num1 + index1; ++index3)
      {
        if (index3 >= 0 && num9 >= 0 && index3 < this.width && num9 < this.depth)
        {
          flag = true;
          if (constraint.Suitable((GraphNode) this.nodes[index3 + num10]))
          {
            vector3_3 = (Vector3) this.nodes[index3 + num10].position - vector3_1;
            float sqrMagnitude = vector3_3.sqrMagnitude;
            if ((double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num6)
            {
              num3 = sqrMagnitude;
              gridNode = this.nodes[index3 + num10];
              vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) index3 - 0.5f, (float) index3 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode.position).y, Mathf.Clamp(f2, (float) num9 - 0.5f, (float) num9 + 0.5f) + 0.5f));
            }
          }
        }
      }
      int num11 = num1 - index1;
      for (int index4 = num2 - index1 + 1; index4 <= num2 + index1 - 1; ++index4)
      {
        if (num11 >= 0 && index4 >= 0 && num11 < this.width && index4 < this.depth)
        {
          flag = true;
          if (constraint.Suitable((GraphNode) this.nodes[num11 + index4 * this.width]))
          {
            vector3_3 = (Vector3) this.nodes[num11 + index4 * this.width].position - vector3_1;
            float sqrMagnitude = vector3_3.sqrMagnitude;
            if ((double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num6)
            {
              num3 = sqrMagnitude;
              gridNode = this.nodes[num11 + index4 * this.width];
              vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) num11 - 0.5f, (float) num11 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode.position).y, Mathf.Clamp(f2, (float) index4 - 0.5f, (float) index4 + 0.5f) + 0.5f));
            }
          }
        }
      }
      int num12 = num1 + index1;
      for (int index5 = num2 - index1 + 1; index5 <= num2 + index1 - 1; ++index5)
      {
        if (num12 >= 0 && index5 >= 0 && num12 < this.width && index5 < this.depth)
        {
          flag = true;
          if (constraint.Suitable((GraphNode) this.nodes[num12 + index5 * this.width]))
          {
            vector3_3 = (Vector3) this.nodes[num12 + index5 * this.width].position - vector3_1;
            float sqrMagnitude = vector3_3.sqrMagnitude;
            if ((double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num6)
            {
              num3 = sqrMagnitude;
              gridNode = this.nodes[num12 + index5 * this.width];
              vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) num12 - 0.5f, (float) num12 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode.position).y, Mathf.Clamp(f2, (float) index5 - 0.5f, (float) index5 + 0.5f) + 0.5f));
            }
          }
        }
      }
      if (gridNode != null)
      {
        if (num4 == 0)
        {
          nearestForce.node = (GraphNode) gridNode;
          nearestForce.clampedPosition = vector3_2;
          return nearestForce;
        }
        --num4;
      }
      if (!flag)
      {
        nearestForce.node = (GraphNode) gridNode;
        nearestForce.clampedPosition = vector3_2;
        return nearestForce;
      }
    }
    nearestForce.node = (GraphNode) gridNode;
    nearestForce.clampedPosition = vector3_2;
    return nearestForce;
  }

  public virtual void SetUpOffsetsAndCosts()
  {
    this.neighbourOffsets[0] = -this.width;
    this.neighbourOffsets[1] = 1;
    this.neighbourOffsets[2] = this.width;
    this.neighbourOffsets[3] = -1;
    this.neighbourOffsets[4] = -this.width + 1;
    this.neighbourOffsets[5] = this.width + 1;
    this.neighbourOffsets[6] = this.width - 1;
    this.neighbourOffsets[7] = -this.width - 1;
    uint num1 = (uint) Mathf.RoundToInt(this.nodeSize * 1000f);
    uint num2 = this.uniformEdgeCosts ? num1 : (uint) Mathf.RoundToInt((float) ((double) this.nodeSize * (double) Mathf.Sqrt(2f) * 1000.0));
    this.neighbourCosts[0] = num1;
    this.neighbourCosts[1] = num1;
    this.neighbourCosts[2] = num1;
    this.neighbourCosts[3] = num1;
    this.neighbourCosts[4] = num2;
    this.neighbourCosts[5] = num2;
    this.neighbourCosts[6] = num2;
    this.neighbourCosts[7] = num2;
    this.neighbourXOffsets[0] = 0;
    this.neighbourXOffsets[1] = 1;
    this.neighbourXOffsets[2] = 0;
    this.neighbourXOffsets[3] = -1;
    this.neighbourXOffsets[4] = 1;
    this.neighbourXOffsets[5] = 1;
    this.neighbourXOffsets[6] = -1;
    this.neighbourXOffsets[7] = -1;
    this.neighbourZOffsets[0] = -1;
    this.neighbourZOffsets[1] = 0;
    this.neighbourZOffsets[2] = 1;
    this.neighbourZOffsets[3] = 0;
    this.neighbourZOffsets[4] = -1;
    this.neighbourZOffsets[5] = 1;
    this.neighbourZOffsets[6] = 1;
    this.neighbourZOffsets[7] = -1;
  }

  public override void ScanInternal(OnScanStatus statusCallback)
  {
    AstarPath.OnPostScan += new OnScanDelegate(this.OnPostScan);
    if ((double) this.nodeSize <= 0.0)
      return;
    this.GenerateMatrix();
    if (this.width > 1024 /*0x0400*/ || this.depth > 1024 /*0x0400*/)
    {
      Debug.LogError((object) "One of the grid's sides is longer than 1024 nodes");
    }
    else
    {
      if (this.useJumpPointSearch)
        Debug.LogError((object) "Trying to use Jump Point Search, but support for it is not enabled. Please enable it in the inspector (Grid Graph settings).");
      this.SetUpOffsetsAndCosts();
      int graphIndex = AstarPath.active.astarData.GetGraphIndex((NavGraph) this);
      GridNode.SetGridGraph(graphIndex, this);
      this.nodes = new GridNode[this.width * this.depth];
      for (int index = 0; index < this.nodes.Length; ++index)
      {
        this.nodes[index] = new GridNode(this.active);
        this.nodes[index].GraphIndex = (uint) graphIndex;
      }
      if (this.collision == null)
        this.collision = new GraphCollision();
      this.collision.Initialize(this.matrix, this.nodeSize);
      this.textureData.Initialize();
      for (int z = 0; z < this.depth; ++z)
      {
        for (int x = 0; x < this.width; ++x)
        {
          GridNode node = this.nodes[z * this.width + x];
          node.NodeInGridIndex = z * this.width + x;
          this.UpdateNodePositionCollision(node, x, z);
          this.textureData.Apply(node, x, z);
        }
      }
      for (int z = 0; z < this.depth; ++z)
      {
        for (int x = 0; x < this.width; ++x)
        {
          GridNode node = this.nodes[z * this.width + x];
          this.CalculateConnections(x, z, node);
        }
      }
      this.ErodeWalkableArea();
    }
  }

  public virtual void UpdateNodePositionCollision(GridNode node, int x, int z, bool resetPenalty = true)
  {
    node.position = this.GraphPointToWorld(x, z, 0.0f);
    RaycastHit hit;
    bool walkable;
    Vector3 vector3 = this.collision.CheckHeight((Vector3) node.position, out hit, out walkable);
    node.position = (Int3) vector3;
    if (resetPenalty)
    {
      node.Penalty = this.initialPenalty;
      if (this.penaltyPosition)
        node.Penalty += (uint) Mathf.RoundToInt(((float) node.position.y - this.penaltyPositionOffset) * this.penaltyPositionFactor);
    }
    if (walkable && this.useRaycastNormal && this.collision.heightCheck && hit.normal != Vector3.zero)
    {
      float f = Vector3.Dot(hit.normal.normalized, this.collision.up);
      if (this.penaltyAngle & resetPenalty)
        node.Penalty += (uint) Mathf.RoundToInt((1f - Mathf.Pow(f, this.penaltyAnglePower)) * this.penaltyAngleFactor);
      float num = Mathf.Cos(this.maxSlope * ((float) Math.PI / 180f));
      if ((double) f < (double) num)
        walkable = false;
    }
    node.Walkable = walkable && this.collision.Check((Vector3) node.position);
    node.WalkableErosion = node.Walkable;
  }

  public virtual void ErodeWalkableArea() => this.ErodeWalkableArea(0, 0, this.Width, this.Depth);

  public bool ErosionAnyFalseConnections(GridNode node)
  {
    if (this.neighbours == NumNeighbours.Six)
    {
      for (int index = 0; index < 6; ++index)
      {
        if (!this.HasNodeConnection(node, GridGraph.hexagonNeighbourIndices[index]))
          return true;
      }
    }
    else
    {
      for (int dir = 0; dir < 4; ++dir)
      {
        if (!this.HasNodeConnection(node, dir))
          return true;
      }
    }
    return false;
  }

  public virtual void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
  {
    xmin = Mathf.Clamp(xmin, 0, this.Width);
    xmax = Mathf.Clamp(xmax, 0, this.Width);
    zmin = Mathf.Clamp(zmin, 0, this.Depth);
    zmax = Mathf.Clamp(zmax, 0, this.Depth);
    if (!this.erosionUseTags)
    {
      for (int index1 = 0; index1 < this.erodeIterations; ++index1)
      {
        for (int index2 = zmin; index2 < zmax; ++index2)
        {
          for (int index3 = xmin; index3 < xmax; ++index3)
          {
            GridNode node = this.nodes[index2 * this.Width + index3];
            if (node.Walkable && this.ErosionAnyFalseConnections(node))
              node.Walkable = false;
          }
        }
        for (int z = zmin; z < zmax; ++z)
        {
          for (int x = xmin; x < xmax; ++x)
          {
            GridNode node = this.nodes[z * this.Width + x];
            this.CalculateConnections(x, z, node);
          }
        }
      }
    }
    else if (this.erodeIterations + this.erosionFirstTag > 31 /*0x1F*/)
      Debug.LogError((object) $"Too few tags available for {this.erodeIterations.ToString()} erode iterations and starting with tag {this.erosionFirstTag.ToString()} (erodeIterations+erosionFirstTag > 31)");
    else if (this.erosionFirstTag <= 0)
    {
      Debug.LogError((object) "First erosion tag must be greater or equal to 1");
    }
    else
    {
      for (int index4 = 0; index4 < this.erodeIterations; ++index4)
      {
        for (int index5 = zmin; index5 < zmax; ++index5)
        {
          for (int index6 = xmin; index6 < xmax; ++index6)
          {
            GridNode node = this.nodes[index5 * this.width + index6];
            if (node.Walkable && (long) node.Tag >= (long) this.erosionFirstTag && (long) node.Tag < (long) (this.erosionFirstTag + index4))
            {
              if (this.neighbours == NumNeighbours.Six)
              {
                for (int index7 = 0; index7 < 6; ++index7)
                {
                  GridNode nodeConnection = this.GetNodeConnection(node, GridGraph.hexagonNeighbourIndices[index7]);
                  if (nodeConnection != null)
                  {
                    uint tag = nodeConnection.Tag;
                    if ((long) tag > (long) (this.erosionFirstTag + index4) || (long) tag < (long) this.erosionFirstTag)
                      nodeConnection.Tag = (uint) (this.erosionFirstTag + index4);
                  }
                }
              }
              else
              {
                for (int dir = 0; dir < 4; ++dir)
                {
                  GridNode nodeConnection = this.GetNodeConnection(node, dir);
                  if (nodeConnection != null)
                  {
                    uint tag = nodeConnection.Tag;
                    if ((long) tag > (long) (this.erosionFirstTag + index4) || (long) tag < (long) this.erosionFirstTag)
                      nodeConnection.Tag = (uint) (this.erosionFirstTag + index4);
                  }
                }
              }
            }
            else if (node.Walkable && index4 == 0 && this.ErosionAnyFalseConnections(node))
              node.Tag = (uint) (this.erosionFirstTag + index4);
          }
        }
      }
    }
  }

  public virtual bool IsValidConnection(GridNode n1, GridNode n2)
  {
    if (!n1.Walkable || !n2.Walkable)
      return false;
    return (double) this.maxClimb <= 0.0 || (double) Math.Abs(n1.position[this.maxClimbAxis] - n2.position[this.maxClimbAxis]) <= (double) this.maxClimb * 1000.0;
  }

  public static void CalculateConnections(GridNode node)
  {
    if (!(AstarData.GetGraph((GraphNode) node) is GridGraph graph))
      return;
    int nodeInGridIndex = node.NodeInGridIndex;
    int x = nodeInGridIndex % graph.width;
    int z = nodeInGridIndex / graph.width;
    graph.CalculateConnections(x, z, node);
  }

  [Obsolete("CalculateConnections no longer takes a node array, it just uses the one on the graph")]
  public virtual void CalculateConnections(GridNode[] nodes, int x, int z, GridNode node)
  {
    this.CalculateConnections(x, z, node);
  }

  public virtual void CalculateConnections(int x, int z, GridNode node)
  {
    if (!node.Walkable)
    {
      node.ResetConnectionsInternal();
    }
    else
    {
      int nodeInGridIndex = node.NodeInGridIndex;
      if (this.neighbours == NumNeighbours.Four || this.neighbours == NumNeighbours.Eight)
      {
        int num1 = 0;
        for (int index = 0; index < 4; ++index)
        {
          int num2 = x + this.neighbourXOffsets[index];
          int num3 = z + this.neighbourZOffsets[index];
          if (num2 >= 0 & num3 >= 0 & num2 < this.width & num3 < this.depth)
          {
            GridNode node1 = this.nodes[nodeInGridIndex + this.neighbourOffsets[index]];
            if (this.IsValidConnection(node, node1))
              num1 |= 1 << index;
          }
        }
        int num4 = 0;
        if (this.neighbours == NumNeighbours.Eight)
        {
          if (this.cutCorners)
          {
            for (int index1 = 0; index1 < 4; ++index1)
            {
              if (((num1 >> index1 | num1 >> index1 + 1 | num1 >> index1 + 1 - 4) & 1) != 0)
              {
                int index2 = index1 + 4;
                int num5 = x + this.neighbourXOffsets[index2];
                int num6 = z + this.neighbourZOffsets[index2];
                if (num5 >= 0 & num6 >= 0 & num5 < this.width & num6 < this.depth)
                {
                  GridNode node2 = this.nodes[nodeInGridIndex + this.neighbourOffsets[index2]];
                  if (this.IsValidConnection(node, node2))
                    num4 |= 1 << index2;
                }
              }
            }
          }
          else
          {
            for (int index = 0; index < 4; ++index)
            {
              if ((num1 >> index & 1) != 0 && ((num1 >> index + 1 | num1 >> index + 1 - 4) & 1) != 0)
              {
                GridNode node3 = this.nodes[nodeInGridIndex + this.neighbourOffsets[index + 4]];
                if (this.IsValidConnection(node, node3))
                  num4 |= 1 << index + 4;
              }
            }
          }
        }
        node.SetAllConnectionInternal(num1 | num4);
      }
      else
      {
        node.ResetConnectionsInternal();
        for (int index = 0; index < GridGraph.hexagonNeighbourIndices.Length; ++index)
        {
          int hexagonNeighbourIndex = GridGraph.hexagonNeighbourIndices[index];
          int num7 = x + this.neighbourXOffsets[hexagonNeighbourIndex];
          int num8 = z + this.neighbourZOffsets[hexagonNeighbourIndex];
          if (num7 >= 0 & num8 >= 0 & num7 < this.width & num8 < this.depth)
          {
            GridNode node4 = this.nodes[nodeInGridIndex + this.neighbourOffsets[hexagonNeighbourIndex]];
            node.SetConnectionInternal(hexagonNeighbourIndex, this.IsValidConnection(node, node4));
          }
        }
      }
    }
  }

  public void OnPostScan(AstarPath script)
  {
    AstarPath.OnPostScan -= new OnScanDelegate(this.OnPostScan);
    if (this.autoLinkGrids && (double) this.autoLinkDistLimit > 0.0)
      throw new NotSupportedException();
  }

  public override void OnDrawGizmos(bool drawNodes)
  {
    Gizmos.matrix = this.boundsMatrix;
    Gizmos.color = Color.white;
    Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.size.x, 0.0f, this.size.y));
    Gizmos.matrix = Matrix4x4.identity;
    if (!drawNodes || this.nodes == null || this.nodes.Length != this.width * this.depth)
      return;
    PathHandler debugPathData = AstarPath.active.debugPathData;
    bool flag = AstarPath.active.showSearchTree && debugPathData != null;
    for (int index1 = 0; index1 < this.depth; ++index1)
    {
      for (int index2 = 0; index2 < this.width; ++index2)
      {
        GridNode node1 = this.nodes[index1 * this.width + index2];
        if (node1.Walkable)
        {
          Gizmos.color = this.NodeColor((GraphNode) node1, debugPathData);
          Vector3 position = (Vector3) node1.position;
          if (flag)
          {
            if (NavGraph.InSearchTree((GraphNode) node1, AstarPath.active.debugPath))
            {
              PathNode pathNode = debugPathData.GetPathNode((GraphNode) node1);
              if (pathNode != null && pathNode.parent != null)
                Gizmos.DrawLine(position, (Vector3) pathNode.parent.node.position);
            }
          }
          else
          {
            for (int dir = 0; dir < 8; ++dir)
            {
              if (node1.GetConnectionInternal(dir))
              {
                GridNode node2 = this.nodes[node1.NodeInGridIndex + this.neighbourOffsets[dir]];
                Gizmos.DrawLine(position, (Vector3) node2.position);
              }
            }
            if (node1.connections != null)
            {
              for (int index3 = 0; index3 < node1.connections.Length; ++index3)
              {
                GraphNode connection = node1.connections[index3];
                Gizmos.DrawLine(position, (Vector3) connection.position);
              }
            }
          }
        }
      }
    }
  }

  public static void GetBoundsMinMax(Bounds b, Matrix4x4 matrix, out Vector3 min, out Vector3 max)
  {
    Vector3[] vector3Array = new Vector3[8]
    {
      matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, b.extents.y, b.extents.z)),
      matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, b.extents.y, -b.extents.z)),
      matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, -b.extents.y, b.extents.z)),
      matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, -b.extents.y, -b.extents.z)),
      matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, b.extents.y, b.extents.z)),
      matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, b.extents.y, -b.extents.z)),
      matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, -b.extents.y, b.extents.z)),
      matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, -b.extents.y, -b.extents.z))
    };
    min = vector3Array[0];
    max = vector3Array[0];
    for (int index = 1; index < 8; ++index)
    {
      min = Vector3.Min(min, vector3Array[index]);
      max = Vector3.Max(max, vector3Array[index]);
    }
  }

  public List<GraphNode> GetNodesInArea(Bounds b)
  {
    return this.GetNodesInArea(b, (GraphUpdateShape) null);
  }

  public List<GraphNode> GetNodesInArea(GraphUpdateShape shape)
  {
    return this.GetNodesInArea(shape.GetBounds(), shape);
  }

  public List<GraphNode> GetNodesInArea(Bounds b, GraphUpdateShape shape)
  {
    if (this.nodes == null || this.width * this.depth != this.nodes.Length)
      return (List<GraphNode>) null;
    List<GraphNode> nodesInArea = ListPool<GraphNode>.Claim();
    Vector3 min;
    Vector3 max;
    GridGraph.GetBoundsMinMax(b, this.inverseMatrix, out min, out max);
    int xmin1 = Mathf.RoundToInt(min.x - 0.5f);
    int xmax = Mathf.RoundToInt(max.x - 0.5f);
    int ymin1 = Mathf.RoundToInt(min.z - 0.5f);
    int ymax = Mathf.RoundToInt(max.z - 0.5f);
    IntRect intRect = IntRect.Intersection(new IntRect(xmin1, ymin1, xmax, ymax), new IntRect(0, 0, this.width - 1, this.depth - 1));
    for (int xmin2 = intRect.xmin; xmin2 <= intRect.xmax; ++xmin2)
    {
      for (int ymin2 = intRect.ymin; ymin2 <= intRect.ymax; ++ymin2)
      {
        GraphNode node = (GraphNode) this.nodes[ymin2 * this.width + xmin2];
        if (b.Contains((Vector3) node.position) && (shape == null || shape.Contains((Vector3) node.position)))
          nodesInArea.Add(node);
      }
    }
    return nodesInArea;
  }

  public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
  {
    return GraphUpdateThreading.UnityThread;
  }

  public void UpdateAreaInit(GraphUpdateObject o)
  {
  }

  public void UpdateArea(GraphUpdateObject o)
  {
    if (this.nodes == null || this.nodes.Length != this.width * this.depth)
    {
      Debug.LogWarning((object) "The Grid Graph is not scanned, cannot update area ");
    }
    else
    {
      Vector3 min;
      Vector3 max;
      GridGraph.GetBoundsMinMax(o.bounds, this.inverseMatrix, out min, out max);
      int xmin1 = Mathf.RoundToInt(min.x - 0.5f);
      int xmax = Mathf.RoundToInt(max.x - 0.5f);
      int ymin1 = Mathf.RoundToInt(min.z - 0.5f);
      int ymax = Mathf.RoundToInt(max.z - 0.5f);
      IntRect a1 = new IntRect(xmin1, ymin1, xmax, ymax);
      IntRect intRect1 = a1;
      IntRect b = new IntRect(0, 0, this.width - 1, this.depth - 1);
      IntRect intRect2 = a1;
      int erodeIterations = o.updateErosion ? this.erodeIterations : 0;
      bool flag = o.updatePhysics || o.modifyWalkability;
      if (o.updatePhysics && !o.modifyWalkability && this.collision.collisionCheck)
      {
        Vector3 vector3_1 = new Vector3(this.collision.diameter, 0.0f, this.collision.diameter) * 0.5f;
        Vector3 vector3_2 = min - vector3_1 * 1.02f;
        Vector3 vector3_3 = max + vector3_1 * 1.02f;
        intRect2 = new IntRect(Mathf.RoundToInt(vector3_2.x - 0.5f), Mathf.RoundToInt(vector3_2.z - 0.5f), Mathf.RoundToInt(vector3_3.x - 0.5f), Mathf.RoundToInt(vector3_3.z - 0.5f));
        intRect1 = IntRect.Union(intRect2, intRect1);
      }
      if (flag || erodeIterations > 0)
        intRect1 = intRect1.Expand(erodeIterations + 1);
      IntRect intRect3 = IntRect.Intersection(intRect1, b);
      for (int xmin2 = intRect3.xmin; xmin2 <= intRect3.xmax; ++xmin2)
      {
        for (int ymin2 = intRect3.ymin; ymin2 <= intRect3.ymax; ++ymin2)
          o.WillUpdateNode((GraphNode) this.nodes[ymin2 * this.width + xmin2]);
      }
      if (o.updatePhysics && !o.modifyWalkability)
      {
        this.collision.Initialize(this.matrix, this.nodeSize);
        IntRect intRect4 = IntRect.Intersection(intRect2, b);
        for (int xmin3 = intRect4.xmin; xmin3 <= intRect4.xmax; ++xmin3)
        {
          for (int ymin3 = intRect4.ymin; ymin3 <= intRect4.ymax; ++ymin3)
            this.UpdateNodePositionCollision(this.nodes[ymin3 * this.width + xmin3], xmin3, ymin3, o.resetPenaltyOnPhysics);
        }
      }
      IntRect intRect5 = IntRect.Intersection(a1, b);
      for (int xmin4 = intRect5.xmin; xmin4 <= intRect5.xmax; ++xmin4)
      {
        for (int ymin4 = intRect5.ymin; ymin4 <= intRect5.ymax; ++ymin4)
        {
          GridNode node = this.nodes[ymin4 * this.width + xmin4];
          if (flag)
          {
            node.Walkable = node.WalkableErosion;
            if (o.bounds.Contains((Vector3) node.position))
              o.Apply((GraphNode) node);
            node.WalkableErosion = node.Walkable;
          }
          else if (o.bounds.Contains((Vector3) node.position))
            o.Apply((GraphNode) node);
        }
      }
      if (flag && erodeIterations == 0)
      {
        IntRect intRect6 = IntRect.Intersection(intRect1, b);
        for (int xmin5 = intRect6.xmin; xmin5 <= intRect6.xmax; ++xmin5)
        {
          for (int ymin5 = intRect6.ymin; ymin5 <= intRect6.ymax; ++ymin5)
          {
            GridNode node = this.nodes[ymin5 * this.width + xmin5];
            this.CalculateConnections(xmin5, ymin5, node);
          }
        }
      }
      else
      {
        if (!flag || erodeIterations <= 0)
          return;
        IntRect a2 = IntRect.Union(a1, intRect2).Expand(erodeIterations);
        IntRect a3 = a2.Expand(erodeIterations);
        IntRect intRect7 = IntRect.Intersection(a2, b);
        IntRect intRect8 = IntRect.Intersection(a3, b);
        for (int xmin6 = intRect8.xmin; xmin6 <= intRect8.xmax; ++xmin6)
        {
          for (int ymin6 = intRect8.ymin; ymin6 <= intRect8.ymax; ++ymin6)
          {
            GridNode node = this.nodes[ymin6 * this.width + xmin6];
            bool walkable = node.Walkable;
            node.Walkable = node.WalkableErosion;
            if (!intRect7.Contains(xmin6, ymin6))
              node.TmpWalkable = walkable;
          }
        }
        for (int xmin7 = intRect8.xmin; xmin7 <= intRect8.xmax; ++xmin7)
        {
          for (int ymin7 = intRect8.ymin; ymin7 <= intRect8.ymax; ++ymin7)
          {
            GridNode node = this.nodes[ymin7 * this.width + xmin7];
            this.CalculateConnections(xmin7, ymin7, node);
          }
        }
        this.ErodeWalkableArea(intRect8.xmin, intRect8.ymin, intRect8.xmax + 1, intRect8.ymax + 1);
        for (int xmin8 = intRect8.xmin; xmin8 <= intRect8.xmax; ++xmin8)
        {
          for (int ymin8 = intRect8.ymin; ymin8 <= intRect8.ymax; ++ymin8)
          {
            if (!intRect7.Contains(xmin8, ymin8))
            {
              GridNode node = this.nodes[ymin8 * this.width + xmin8];
              node.Walkable = node.TmpWalkable;
            }
          }
        }
        for (int xmin9 = intRect8.xmin; xmin9 <= intRect8.xmax; ++xmin9)
        {
          for (int ymin9 = intRect8.ymin; ymin9 <= intRect8.ymax; ++ymin9)
          {
            GridNode node = this.nodes[ymin9 * this.width + xmin9];
            this.CalculateConnections(xmin9, ymin9, node);
          }
        }
      }
    }
  }

  public bool Linecast(Vector3 _a, Vector3 _b)
  {
    return this.Linecast(_a, _b, (GraphNode) null, out GraphHitInfo _);
  }

  public bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint)
  {
    return this.Linecast(_a, _b, hint, out GraphHitInfo _);
  }

  public bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint, out GraphHitInfo hit)
  {
    return this.Linecast(_a, _b, hint, out hit, (List<GraphNode>) null);
  }

  public static float CrossMagnitude(Vector2 a, Vector2 b)
  {
    return (float) ((double) a.x * (double) b.y - (double) b.x * (double) a.y);
  }

  public virtual GridNodeBase GetNeighbourAlongDirection(GridNodeBase node, int direction)
  {
    GridNode gridNode = node as GridNode;
    return gridNode.GetConnectionInternal(direction) ? (GridNodeBase) this.nodes[gridNode.NodeInGridIndex + this.neighbourOffsets[direction]] : (GridNodeBase) null;
  }

  public bool ClipLineSegmentToBounds(Vector3 a, Vector3 b, out Vector3 outA, out Vector3 outB)
  {
    if ((double) a.x < 0.0 || (double) a.z < 0.0 || (double) a.x > (double) this.width || (double) a.z > (double) this.depth || (double) b.x < 0.0 || (double) b.z < 0.0 || (double) b.x > (double) this.width || (double) b.z > (double) this.depth)
    {
      Vector3 vector3_1 = new Vector3(0.0f, 0.0f, 0.0f);
      Vector3 vector3_2 = new Vector3(0.0f, 0.0f, (float) this.depth);
      Vector3 vector3_3 = new Vector3((float) this.width, 0.0f, (float) this.depth);
      Vector3 vector3_4 = new Vector3((float) this.width, 0.0f, 0.0f);
      int num = 0;
      bool intersects;
      Vector3 vector3_5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector3_1, vector3_2, out intersects);
      if (intersects)
      {
        ++num;
        if (!VectorMath.RightOrColinearXZ(vector3_1, vector3_2, a))
          a = vector3_5;
        else
          b = vector3_5;
      }
      Vector3 vector3_6 = VectorMath.SegmentIntersectionPointXZ(a, b, vector3_2, vector3_3, out intersects);
      if (intersects)
      {
        ++num;
        if (!VectorMath.RightOrColinearXZ(vector3_2, vector3_3, a))
          a = vector3_6;
        else
          b = vector3_6;
      }
      Vector3 vector3_7 = VectorMath.SegmentIntersectionPointXZ(a, b, vector3_3, vector3_4, out intersects);
      if (intersects)
      {
        ++num;
        if (!VectorMath.RightOrColinearXZ(vector3_3, vector3_4, a))
          a = vector3_7;
        else
          b = vector3_7;
      }
      Vector3 vector3_8 = VectorMath.SegmentIntersectionPointXZ(a, b, vector3_4, vector3_1, out intersects);
      if (intersects)
      {
        ++num;
        if (!VectorMath.RightOrColinearXZ(vector3_4, vector3_1, a))
          a = vector3_8;
        else
          b = vector3_8;
      }
      if (num == 0)
      {
        outA = Vector3.zero;
        outB = Vector3.zero;
        return false;
      }
    }
    outA = a;
    outB = b;
    return true;
  }

  public bool Linecast(
    Vector3 _a,
    Vector3 _b,
    GraphNode hint,
    out GraphHitInfo hit,
    List<GraphNode> trace)
  {
    hit = new GraphHitInfo();
    hit.origin = _a;
    Vector3 outA = this.inverseMatrix.MultiplyPoint3x4(_a);
    Vector3 outB = this.inverseMatrix.MultiplyPoint3x4(_b);
    if (!this.ClipLineSegmentToBounds(outA, outB, out outA, out outB))
      return false;
    GridNodeBase node1 = this.GetNearest(this.matrix.MultiplyPoint3x4(outA), NNConstraint.None).node as GridNodeBase;
    GridNodeBase node2 = this.GetNearest(this.matrix.MultiplyPoint3x4(outB), NNConstraint.None).node as GridNodeBase;
    if (!node1.Walkable)
    {
      hit.node = (GraphNode) node1;
      hit.point = this.matrix.MultiplyPoint3x4(outA);
      hit.tangentOrigin = hit.point;
      return true;
    }
    Vector2 vector2_1 = new Vector2(outA.x, outA.z);
    Vector2 vector2_2 = new Vector2(outB.x, outB.z);
    Vector2 start2 = vector2_1 - Vector2.one * 0.5f;
    Vector2 end2 = vector2_2 - Vector2.one * 0.5f;
    if (node1 == null || node2 == null)
    {
      hit.node = (GraphNode) null;
      hit.point = _a;
      return true;
    }
    Vector2 a = end2 - start2;
    Int2 int2 = new Int2((int) Mathf.Sign(a.x), (int) Mathf.Sign(a.y));
    float num1 = GridGraph.CrossMagnitude(a, new Vector2((float) int2.x, (float) int2.y)) * 0.5f;
    int num2;
    int num3;
    if ((double) a.y >= 0.0)
    {
      if ((double) a.x >= 0.0)
      {
        num2 = 1;
        num3 = 2;
      }
      else
      {
        num2 = 2;
        num3 = 3;
      }
    }
    else if ((double) a.x < 0.0)
    {
      num2 = 3;
      num3 = 0;
    }
    else
    {
      num2 = 0;
      num3 = 1;
    }
    GridNodeBase node3;
    GridNodeBase neighbourAlongDirection;
    for (node3 = node1; node3.NodeInGridIndex != node2.NodeInGridIndex; node3 = neighbourAlongDirection)
    {
      trace?.Add((GraphNode) node3);
      Vector2 vector2_3 = new Vector2((float) (node3.NodeInGridIndex % this.width), (float) (node3.NodeInGridIndex / this.width));
      int direction = (double) GridGraph.CrossMagnitude(a, vector2_3 - start2) + (double) num1 < 0.0 ? num3 : num2;
      neighbourAlongDirection = this.GetNeighbourAlongDirection(node3, direction);
      if (neighbourAlongDirection == null)
      {
        Vector2 start1 = vector2_3 + new Vector2((float) this.neighbourXOffsets[direction], (float) this.neighbourZOffsets[direction]) * 0.5f;
        Vector2 vector2_4 = this.neighbourXOffsets[direction] != 0 ? new Vector2(0.0f, 1f) : new Vector2(1f, 0.0f);
        Vector2 vector2_5 = VectorMath.LineIntersectionPoint(start1, start1 + vector2_4, start2, end2);
        Vector3 vector3 = this.inverseMatrix.MultiplyPoint3x4((Vector3) node3.position);
        Vector3 point1 = new Vector3(vector2_5.x + 0.5f, vector3.y, vector2_5.y + 0.5f);
        Vector3 point2 = new Vector3(start1.x + 0.5f, vector3.y, start1.y + 0.5f);
        hit.point = this.matrix.MultiplyPoint3x4(point1);
        hit.tangentOrigin = this.matrix.MultiplyPoint3x4(point2);
        hit.tangent = this.matrix.MultiplyVector(new Vector3(vector2_4.x, 0.0f, vector2_4.y));
        hit.node = (GraphNode) node3;
        return true;
      }
    }
    trace?.Add((GraphNode) node3);
    if (node3 == node2)
      return false;
    hit.point = (Vector3) node3.position;
    hit.tangentOrigin = hit.point;
    return true;
  }

  public bool SnappedLinecast(Vector3 a, Vector3 b, GraphNode hint, out GraphHitInfo hit)
  {
    return this.Linecast((Vector3) this.GetNearest(a, NNConstraint.None).node.position, (Vector3) this.GetNearest(b, NNConstraint.None).node.position, hint, out hit);
  }

  public bool CheckConnection(GridNode node, int dir)
  {
    if (this.neighbours == NumNeighbours.Eight || this.neighbours == NumNeighbours.Six || dir < 4)
      return this.HasNodeConnection(node, dir);
    int dir1 = dir - 4 - 1 & 3;
    int dir2 = dir - 4 + 1 & 3;
    if (!this.HasNodeConnection(node, dir1) || !this.HasNodeConnection(node, dir2))
      return false;
    GridNode node1 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir1]];
    GridNode node2 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir2]];
    return node1.Walkable && node2.Walkable && this.HasNodeConnection(node2, dir1) && this.HasNodeConnection(node1, dir2);
  }

  public override void SerializeExtraInfo(GraphSerializationContext ctx)
  {
    if (this.nodes == null)
    {
      ctx.writer.Write(-1);
    }
    else
    {
      ctx.writer.Write(this.nodes.Length);
      for (int index = 0; index < this.nodes.Length; ++index)
        this.nodes[index].SerializeNode(ctx);
    }
  }

  public override void DeserializeExtraInfo(GraphSerializationContext ctx)
  {
    int length = ctx.reader.ReadInt32();
    if (length == -1)
    {
      this.nodes = (GridNode[]) null;
    }
    else
    {
      this.nodes = new GridNode[length];
      for (int index = 0; index < this.nodes.Length; ++index)
      {
        this.nodes[index] = new GridNode(this.active);
        this.nodes[index].DeserializeNode(ctx);
      }
    }
  }

  public override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
  {
    base.DeserializeSettingsCompatibility(ctx);
    this.aspectRatio = ctx.reader.ReadSingle();
    this.rotation = ctx.DeserializeVector3();
    this.center = ctx.DeserializeVector3();
    this.unclampedSize = (Vector2) ctx.DeserializeVector3();
    this.nodeSize = ctx.reader.ReadSingle();
    this.collision.DeserializeSettingsCompatibility(ctx);
    this.maxClimb = ctx.reader.ReadSingle();
    this.maxClimbAxis = ctx.reader.ReadInt32();
    this.maxSlope = ctx.reader.ReadSingle();
    this.erodeIterations = ctx.reader.ReadInt32();
    this.erosionUseTags = ctx.reader.ReadBoolean();
    this.erosionFirstTag = ctx.reader.ReadInt32();
    this.autoLinkGrids = ctx.reader.ReadBoolean();
    this.neighbours = (NumNeighbours) ctx.reader.ReadInt32();
    this.cutCorners = ctx.reader.ReadBoolean();
    this.penaltyPosition = ctx.reader.ReadBoolean();
    this.penaltyPositionFactor = ctx.reader.ReadSingle();
    this.penaltyAngle = ctx.reader.ReadBoolean();
    this.penaltyAngleFactor = ctx.reader.ReadSingle();
    this.penaltyAnglePower = ctx.reader.ReadSingle();
    this.isometricAngle = ctx.reader.ReadSingle();
    this.uniformEdgeCosts = ctx.reader.ReadBoolean();
    this.useJumpPointSearch = ctx.reader.ReadBoolean();
  }

  public override void PostDeserialization()
  {
    this.GenerateMatrix();
    this.SetUpOffsetsAndCosts();
    if (this.nodes == null || this.nodes.Length == 0)
      return;
    if (this.width * this.depth != this.nodes.Length)
    {
      Debug.LogError((object) "Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph just prior to saving it. Nodes will be discarded");
      this.nodes = new GridNode[0];
    }
    else
    {
      GridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex((NavGraph) this), this);
      for (int index1 = 0; index1 < this.depth; ++index1)
      {
        for (int index2 = 0; index2 < this.width; ++index2)
        {
          GridNode node = this.nodes[index1 * this.width + index2];
          if (node == null)
          {
            Debug.LogError((object) "Deserialization Error : Couldn't cast the node to the appropriate type - GridGenerator");
            return;
          }
          node.NodeInGridIndex = index1 * this.width + index2;
        }
      }
    }
  }

  public override void FastClearNodes() => this.nodes = new GridNode[0];

  public class TextureData
  {
    public bool enabled;
    public Texture2D source;
    public float[] factors = new float[3];
    public GridGraph.TextureData.ChannelUse[] channels = new GridGraph.TextureData.ChannelUse[3];
    public Color32[] data;

    public void Initialize()
    {
      if (!this.enabled || !((UnityEngine.Object) this.source != (UnityEngine.Object) null))
        return;
      for (int index = 0; index < this.channels.Length; ++index)
      {
        if (this.channels[index] != GridGraph.TextureData.ChannelUse.None)
        {
          try
          {
            this.data = this.source.GetPixels32();
            break;
          }
          catch (UnityException ex)
          {
            Debug.LogWarning((object) ex.ToString());
            this.data = (Color32[]) null;
            break;
          }
        }
      }
    }

    public void Apply(GridNode node, int x, int z)
    {
      if (!this.enabled || this.data == null || x >= this.source.width || z >= this.source.height)
        return;
      Color32 color32 = this.data[z * this.source.width + x];
      if (this.channels[0] != GridGraph.TextureData.ChannelUse.None)
        this.ApplyChannel(node, x, z, (int) color32.r, this.channels[0], this.factors[0]);
      if (this.channels[1] != GridGraph.TextureData.ChannelUse.None)
        this.ApplyChannel(node, x, z, (int) color32.g, this.channels[1], this.factors[1]);
      if (this.channels[2] == GridGraph.TextureData.ChannelUse.None)
        return;
      this.ApplyChannel(node, x, z, (int) color32.b, this.channels[2], this.factors[2]);
    }

    public void ApplyChannel(
      GridNode node,
      int x,
      int z,
      int value,
      GridGraph.TextureData.ChannelUse channelUse,
      float factor)
    {
      switch (channelUse)
      {
        case GridGraph.TextureData.ChannelUse.Penalty:
          node.Penalty += (uint) Mathf.RoundToInt((float) value * factor);
          break;
        case GridGraph.TextureData.ChannelUse.Position:
          node.position = GridNode.GetGridGraph(node.GraphIndex).GraphPointToWorld(x, z, (float) value);
          break;
        case GridGraph.TextureData.ChannelUse.WalkablePenalty:
          if (value == 0)
          {
            node.Walkable = false;
            break;
          }
          node.Penalty += (uint) Mathf.RoundToInt((float) (value - 1) * factor);
          break;
      }
    }

    public enum ChannelUse
    {
      None,
      Penalty,
      Position,
      WalkablePenalty,
    }
  }
}
