// Decompiled with JetBrains decompiler
// Type: Pathfinding.LayerGridGraph
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Serialization;
using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class LayerGridGraph : GridGraph, IUpdatableGraph
{
  [JsonMember]
  public int layerCount;
  [JsonMember]
  public float mergeSpanRange = 0.5f;
  [JsonMember]
  public float characterHeight = 0.4f;
  public int lastScannedWidth;
  public int lastScannedDepth;
  public LevelGridNode[] nodes;

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.RemoveGridGraphFromStatic();
  }

  public new void RemoveGridGraphFromStatic()
  {
    LevelGridNode.SetGridGraph(this.active.astarData.GetGraphIndex((NavGraph) this), (LayerGridGraph) null);
  }

  public override bool uniformWidthDepthGrid => false;

  public override int CountNodes()
  {
    if (this.nodes == null)
      return 0;
    int num = 0;
    for (int index = 0; index < this.nodes.Length; ++index)
    {
      if (this.nodes[index] != null)
        ++num;
    }
    return num;
  }

  public override void GetNodes(GraphNodeDelegateCancelable del)
  {
    if (this.nodes == null)
      return;
    int index = 0;
    while (index < this.nodes.Length && (this.nodes[index] == null || del((GraphNode) this.nodes[index])))
      ++index;
  }

  public new void UpdateArea(GraphUpdateObject o)
  {
    if (this.nodes == null || this.nodes.Length != this.width * this.depth * this.layerCount)
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
      bool flag1 = o.updatePhysics || o.modifyWalkability;
      bool flag2 = o is LayerGridGraphUpdate && ((LayerGridGraphUpdate) o).recalculateNodes;
      bool preserveExistingNodes = !(o is LayerGridGraphUpdate) || ((LayerGridGraphUpdate) o).preserveExistingNodes;
      int erodeIterations = o.updateErosion ? this.erodeIterations : 0;
      if (o.trackChangedNodes & flag2)
      {
        Debug.LogError((object) "Cannot track changed nodes when creating or deleting nodes.\nWill not update LayerGridGraph");
      }
      else
      {
        if (o.updatePhysics && !o.modifyWalkability && this.collision.collisionCheck)
        {
          Vector3 vector3_1 = new Vector3(this.collision.diameter, 0.0f, this.collision.diameter) * 0.5f;
          Vector3 vector3_2 = min - vector3_1 * 1.02f;
          Vector3 vector3_3 = max + vector3_1 * 1.02f;
          intRect2 = new IntRect(Mathf.RoundToInt(vector3_2.x - 0.5f), Mathf.RoundToInt(vector3_2.z - 0.5f), Mathf.RoundToInt(vector3_3.x - 0.5f), Mathf.RoundToInt(vector3_3.z - 0.5f));
          intRect1 = IntRect.Union(intRect2, intRect1);
        }
        if (flag1 || erodeIterations > 0)
          intRect1 = intRect1.Expand(erodeIterations + 1);
        IntRect intRect3 = IntRect.Intersection(intRect1, b);
        if (!flag2)
        {
          for (int xmin2 = intRect3.xmin; xmin2 <= intRect3.xmax; ++xmin2)
          {
            for (int ymin2 = intRect3.ymin; ymin2 <= intRect3.ymax; ++ymin2)
            {
              for (int index = 0; index < this.layerCount; ++index)
                o.WillUpdateNode((GraphNode) this.nodes[index * this.width * this.depth + ymin2 * this.width + xmin2]);
            }
          }
        }
        if (o.updatePhysics && !o.modifyWalkability)
        {
          this.collision.Initialize(this.matrix, this.nodeSize);
          IntRect intRect4 = IntRect.Intersection(intRect2, b);
          bool flag3 = false;
          for (int xmin3 = intRect4.xmin; xmin3 <= intRect4.xmax; ++xmin3)
          {
            for (int ymin3 = intRect4.ymin; ymin3 <= intRect4.ymax; ++ymin3)
              flag3 |= this.RecalculateCell(xmin3, ymin3, preserveExistingNodes);
          }
          for (int xmin4 = intRect4.xmin; xmin4 <= intRect4.xmax; ++xmin4)
          {
            for (int ymin4 = intRect4.ymin; ymin4 <= intRect4.ymax; ++ymin4)
            {
              for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
              {
                LevelGridNode node = this.nodes[layerIndex * this.width * this.depth + ymin4 * this.width + xmin4];
                if (node != null)
                  this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) node, xmin4, ymin4, layerIndex);
              }
            }
          }
        }
        IntRect intRect5 = IntRect.Intersection(a1, b);
        for (int xmin5 = intRect5.xmin; xmin5 <= intRect5.xmax; ++xmin5)
        {
          for (int ymin5 = intRect5.ymin; ymin5 <= intRect5.ymax; ++ymin5)
          {
            for (int index = 0; index < this.layerCount; ++index)
            {
              LevelGridNode node = this.nodes[index * this.width * this.depth + ymin5 * this.width + xmin5];
              if (node != null)
              {
                if (flag1)
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
          }
        }
        if (flag1 && erodeIterations == 0)
        {
          IntRect intRect6 = IntRect.Intersection(intRect1, b);
          for (int xmin6 = intRect6.xmin; xmin6 <= intRect6.xmax; ++xmin6)
          {
            for (int ymin6 = intRect6.ymin; ymin6 <= intRect6.ymax; ++ymin6)
            {
              for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
              {
                LevelGridNode node = this.nodes[layerIndex * this.width * this.depth + ymin6 * this.width + xmin6];
                if (node != null)
                  this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) node, xmin6, ymin6, layerIndex);
              }
            }
          }
        }
        else
        {
          if (!flag1 || erodeIterations <= 0)
            return;
          IntRect a2 = IntRect.Union(a1, intRect2).Expand(erodeIterations);
          IntRect a3 = a2.Expand(erodeIterations);
          a2 = IntRect.Intersection(a2, b);
          IntRect intRect7 = IntRect.Intersection(a3, b);
          for (int xmin7 = intRect7.xmin; xmin7 <= intRect7.xmax; ++xmin7)
          {
            for (int ymin7 = intRect7.ymin; ymin7 <= intRect7.ymax; ++ymin7)
            {
              for (int index = 0; index < this.layerCount; ++index)
              {
                LevelGridNode node = this.nodes[index * this.width * this.depth + ymin7 * this.width + xmin7];
                if (node != null)
                {
                  bool walkable = node.Walkable;
                  node.Walkable = node.WalkableErosion;
                  if (!a2.Contains(xmin7, ymin7))
                    node.TmpWalkable = walkable;
                }
              }
            }
          }
          for (int xmin8 = intRect7.xmin; xmin8 <= intRect7.xmax; ++xmin8)
          {
            for (int ymin8 = intRect7.ymin; ymin8 <= intRect7.ymax; ++ymin8)
            {
              for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
              {
                LevelGridNode node = this.nodes[layerIndex * this.width * this.depth + ymin8 * this.width + xmin8];
                if (node != null)
                  this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) node, xmin8, ymin8, layerIndex);
              }
            }
          }
          this.ErodeWalkableArea(intRect7.xmin, intRect7.ymin, intRect7.xmax + 1, intRect7.ymax + 1);
          for (int xmin9 = intRect7.xmin; xmin9 <= intRect7.xmax; ++xmin9)
          {
            for (int ymin9 = intRect7.ymin; ymin9 <= intRect7.ymax; ++ymin9)
            {
              if (!a2.Contains(xmin9, ymin9))
              {
                for (int index = 0; index < this.layerCount; ++index)
                {
                  LevelGridNode node = this.nodes[index * this.width * this.depth + ymin9 * this.width + xmin9];
                  if (node != null)
                    node.Walkable = node.TmpWalkable;
                }
              }
            }
          }
          for (int xmin10 = intRect7.xmin; xmin10 <= intRect7.xmax; ++xmin10)
          {
            for (int ymin10 = intRect7.ymin; ymin10 <= intRect7.ymax; ++ymin10)
            {
              for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
              {
                LevelGridNode node = this.nodes[layerIndex * this.width * this.depth + ymin10 * this.width + xmin10];
                if (node != null)
                  this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) node, xmin10, ymin10, layerIndex);
              }
            }
          }
        }
      }
    }
  }

  public override void ScanInternal(OnScanStatus statusCallback)
  {
    if ((double) this.nodeSize <= 0.0)
      return;
    this.GenerateMatrix();
    if (this.width > 1024 /*0x0400*/ || this.depth > 1024 /*0x0400*/)
    {
      Debug.LogError((object) "One of the grid's sides is longer than 1024 nodes");
    }
    else
    {
      this.lastScannedWidth = this.width;
      this.lastScannedDepth = this.depth;
      this.SetUpOffsetsAndCosts();
      LevelGridNode.SetGridGraph(this.active.astarData.GetGraphIndex((NavGraph) this), this);
      this.maxClimb = Mathf.Clamp(this.maxClimb, 0.0f, this.characterHeight);
      LinkedLevelCell[] linkedLevelCellArray = new LinkedLevelCell[this.width * this.depth];
      this.collision = this.collision ?? new GraphCollision();
      this.collision.Initialize(this.matrix, this.nodeSize);
      for (int index1 = 0; index1 < this.depth; ++index1)
      {
        for (int index2 = 0; index2 < this.width; ++index2)
        {
          linkedLevelCellArray[index1 * this.width + index2] = new LinkedLevelCell();
          LinkedLevelCell linkedLevelCell = linkedLevelCellArray[index1 * this.width + index2];
          Vector3 position = this.matrix.MultiplyPoint3x4(new Vector3((float) index2 + 0.5f, 0.0f, (float) index1 + 0.5f));
          RaycastHit[] raycastHitArray = this.collision.CheckHeightAll(position);
          for (int index3 = 0; index3 < raycastHitArray.Length / 2; ++index3)
          {
            RaycastHit raycastHit = raycastHitArray[index3];
            raycastHitArray[index3] = raycastHitArray[raycastHitArray.Length - 1 - index3];
            raycastHitArray[raycastHitArray.Length - 1 - index3] = raycastHit;
          }
          if (raycastHitArray.Length != 0)
          {
            LinkedLevelNode linkedLevelNode1 = (LinkedLevelNode) null;
            for (int index4 = 0; index4 < raycastHitArray.Length; ++index4)
            {
              LinkedLevelNode linkedLevelNode2 = new LinkedLevelNode();
              linkedLevelNode2.position = raycastHitArray[index4].point;
              if (linkedLevelNode1 != null && (double) linkedLevelNode2.position.y - (double) linkedLevelNode1.position.y <= (double) this.mergeSpanRange)
              {
                linkedLevelNode1.position = linkedLevelNode2.position;
                linkedLevelNode1.hit = raycastHitArray[index4];
                linkedLevelNode1.walkable = this.collision.Check(linkedLevelNode2.position);
              }
              else
              {
                linkedLevelNode2.walkable = this.collision.Check(linkedLevelNode2.position);
                linkedLevelNode2.hit = raycastHitArray[index4];
                linkedLevelNode2.height = float.PositiveInfinity;
                if (linkedLevelCell.first == null)
                {
                  linkedLevelCell.first = linkedLevelNode2;
                  linkedLevelNode1 = linkedLevelNode2;
                }
                else
                {
                  linkedLevelNode1.next = linkedLevelNode2;
                  linkedLevelNode1.height = linkedLevelNode2.position.y - linkedLevelNode1.position.y;
                  linkedLevelNode1 = linkedLevelNode1.next;
                }
              }
            }
          }
          else
            linkedLevelCell.first = new LinkedLevelNode()
            {
              position = position,
              height = float.PositiveInfinity,
              walkable = !this.collision.unwalkableWhenNoGround
            };
        }
      }
      int num1 = 0;
      this.layerCount = 0;
      for (int index5 = 0; index5 < this.depth; ++index5)
      {
        for (int index6 = 0; index6 < this.width; ++index6)
        {
          LinkedLevelNode linkedLevelNode = linkedLevelCellArray[index5 * this.width + index6].first;
          int num2 = 0;
          do
          {
            ++num2;
            ++num1;
            linkedLevelNode = linkedLevelNode.next;
          }
          while (linkedLevelNode != null);
          this.layerCount = num2 > this.layerCount ? num2 : this.layerCount;
        }
      }
      if (this.layerCount > (int) byte.MaxValue)
      {
        Debug.LogError((object) $"Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (found {this.layerCount.ToString()})");
      }
      else
      {
        this.nodes = new LevelGridNode[this.width * this.depth * this.layerCount];
        for (int index = 0; index < this.nodes.Length; ++index)
        {
          this.nodes[index] = new LevelGridNode(this.active);
          this.nodes[index].Penalty = this.initialPenalty;
        }
        int num3 = 0;
        float num4 = Mathf.Cos(this.maxSlope * ((float) Math.PI / 180f));
        for (int index7 = 0; index7 < this.depth; ++index7)
        {
          for (int index8 = 0; index8 < this.width; ++index8)
          {
            LinkedLevelCell linkedLevelCell = linkedLevelCellArray[index7 * this.width + index8];
            LinkedLevelNode linkedLevelNode = linkedLevelCell.first;
            linkedLevelCell.index = num3;
            int num5 = 0;
            int num6 = 0;
            do
            {
              LevelGridNode node = this.nodes[index7 * this.width + index8 + this.width * this.depth * num6];
              node.SetPosition((Int3) linkedLevelNode.position);
              node.Walkable = linkedLevelNode.walkable;
              if (linkedLevelNode.hit.normal != Vector3.zero && (this.penaltyAngle || (double) num4 < 1.0))
              {
                float num7 = Vector3.Dot(linkedLevelNode.hit.normal.normalized, this.collision.up);
                if (this.penaltyAngle)
                  node.Penalty += (uint) Mathf.RoundToInt((1f - num7) * this.penaltyAngleFactor);
                if ((double) num7 < (double) num4)
                  node.Walkable = false;
              }
              node.NodeInGridIndex = index7 * this.width + index8;
              if ((double) linkedLevelNode.height < (double) this.characterHeight)
                node.Walkable = false;
              node.WalkableErosion = node.Walkable;
              ++num3;
              ++num5;
              linkedLevelNode = linkedLevelNode.next;
              ++num6;
            }
            while (linkedLevelNode != null);
            for (; num6 < this.layerCount; ++num6)
              this.nodes[index7 * this.width + index8 + this.width * this.depth * num6] = (LevelGridNode) null;
            linkedLevelCell.count = num5;
          }
        }
        for (int z = 0; z < this.depth; ++z)
        {
          for (int x = 0; x < this.width; ++x)
          {
            for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
              this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) this.nodes[z * this.width + x + this.width * this.depth * layerIndex], x, z, layerIndex);
          }
        }
        uint graphIndex = (uint) this.active.astarData.GetGraphIndex((NavGraph) this);
        for (int index = 0; index < this.nodes.Length; ++index)
        {
          LevelGridNode node = this.nodes[index];
          if (node != null)
          {
            this.UpdatePenalty(node);
            node.GraphIndex = graphIndex;
            if (!node.HasAnyGridConnections())
            {
              node.Walkable = false;
              node.WalkableErosion = node.Walkable;
            }
          }
        }
        this.ErodeWalkableArea();
      }
    }
  }

  public bool RecalculateCell(int x, int z, bool preserveExistingNodes)
  {
    LinkedLevelCell linkedLevelCell = new LinkedLevelCell();
    Vector3 position = this.matrix.MultiplyPoint3x4(new Vector3((float) x + 0.5f, 0.0f, (float) z + 0.5f));
    RaycastHit[] raycastHitArray = this.collision.CheckHeightAll(position);
    for (int index = 0; index < raycastHitArray.Length / 2; ++index)
    {
      RaycastHit raycastHit = raycastHitArray[index];
      raycastHitArray[index] = raycastHitArray[raycastHitArray.Length - 1 - index];
      raycastHitArray[raycastHitArray.Length - 1 - index] = raycastHit;
    }
    bool flag = false;
    if (raycastHitArray.Length != 0)
    {
      LinkedLevelNode linkedLevelNode1 = (LinkedLevelNode) null;
      for (int index = 0; index < raycastHitArray.Length; ++index)
      {
        LinkedLevelNode linkedLevelNode2 = new LinkedLevelNode();
        linkedLevelNode2.position = raycastHitArray[index].point;
        if (linkedLevelNode1 != null && (double) linkedLevelNode2.position.y - (double) linkedLevelNode1.position.y <= (double) this.mergeSpanRange)
        {
          linkedLevelNode1.position = linkedLevelNode2.position;
          linkedLevelNode1.hit = raycastHitArray[index];
          linkedLevelNode1.walkable = this.collision.Check(linkedLevelNode2.position);
        }
        else
        {
          linkedLevelNode2.walkable = this.collision.Check(linkedLevelNode2.position);
          linkedLevelNode2.hit = raycastHitArray[index];
          linkedLevelNode2.height = float.PositiveInfinity;
          if (linkedLevelCell.first == null)
          {
            linkedLevelCell.first = linkedLevelNode2;
            linkedLevelNode1 = linkedLevelNode2;
          }
          else
          {
            linkedLevelNode1.next = linkedLevelNode2;
            linkedLevelNode1.height = linkedLevelNode2.position.y - linkedLevelNode1.position.y;
            linkedLevelNode1 = linkedLevelNode1.next;
          }
        }
      }
    }
    else
      linkedLevelCell.first = new LinkedLevelNode()
      {
        position = position,
        height = float.PositiveInfinity,
        walkable = !this.collision.unwalkableWhenNoGround
      };
    uint graphIndex = (uint) this.active.astarData.GetGraphIndex((NavGraph) this);
    LinkedLevelNode linkedLevelNode = linkedLevelCell.first;
    int num1 = 0;
    int num2 = 0;
    do
    {
      if (num2 >= this.layerCount)
      {
        if (num2 + 1 > (int) byte.MaxValue)
        {
          Debug.LogError((object) $"Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (required {(num2 + 1).ToString()})");
          return flag;
        }
        this.AddLayers(1);
        flag = true;
      }
      LevelGridNode node = this.nodes[z * this.width + x + this.width * this.depth * num2];
      if (node == null || !preserveExistingNodes)
      {
        this.nodes[z * this.width + x + this.width * this.depth * num2] = new LevelGridNode(this.active);
        node = this.nodes[z * this.width + x + this.width * this.depth * num2];
        node.Penalty = this.initialPenalty;
        node.GraphIndex = graphIndex;
        flag = true;
      }
      node.SetPosition((Int3) linkedLevelNode.position);
      node.Walkable = linkedLevelNode.walkable;
      node.WalkableErosion = node.Walkable;
      if (linkedLevelNode.hit.normal != Vector3.zero)
      {
        float num3 = Vector3.Dot(linkedLevelNode.hit.normal.normalized, this.collision.up);
        if (this.penaltyAngle)
          node.Penalty += (uint) Mathf.RoundToInt((1f - num3) * this.penaltyAngleFactor);
        float num4 = Mathf.Cos(this.maxSlope * ((float) Math.PI / 180f));
        if ((double) num3 < (double) num4)
          node.Walkable = false;
      }
      node.NodeInGridIndex = z * this.width + x;
      if ((double) linkedLevelNode.height < (double) this.characterHeight)
        node.Walkable = false;
      ++num1;
      linkedLevelNode = linkedLevelNode.next;
      ++num2;
    }
    while (linkedLevelNode != null);
    for (; num2 < this.layerCount; ++num2)
      this.nodes[z * this.width + x + this.width * this.depth * num2] = (LevelGridNode) null;
    linkedLevelCell.count = num1;
    return flag;
  }

  public void AddLayers(int count)
  {
    int num = this.layerCount + count;
    if (num > (int) byte.MaxValue)
    {
      Debug.LogError((object) $"Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (required {num.ToString()})");
    }
    else
    {
      LevelGridNode[] nodes = this.nodes;
      this.nodes = new LevelGridNode[this.width * this.depth * num];
      for (int index = 0; index < nodes.Length; ++index)
        this.nodes[index] = nodes[index];
      this.layerCount = num;
    }
  }

  public virtual void UpdatePenalty(LevelGridNode node)
  {
    node.Penalty = 0U;
    node.Penalty = this.initialPenalty;
    if (!this.penaltyPosition)
      return;
    node.Penalty += (uint) Mathf.RoundToInt(((float) node.position.y - this.penaltyPositionOffset) * this.penaltyPositionFactor);
  }

  public override void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
  {
    xmin = Mathf.Clamp(xmin, 0, this.Width);
    xmax = Mathf.Clamp(xmax, 0, this.Width);
    zmin = Mathf.Clamp(zmin, 0, this.Depth);
    zmax = Mathf.Clamp(zmax, 0, this.Depth);
    if (this.erosionUseTags)
      Debug.LogError((object) "Erosion Uses Tags is not supported for LayerGridGraphs yet");
    for (int index1 = 0; index1 < this.erodeIterations; ++index1)
    {
      for (int index2 = 0; index2 < this.layerCount; ++index2)
      {
        for (int index3 = zmin; index3 < zmax; ++index3)
        {
          for (int index4 = xmin; index4 < xmax; ++index4)
          {
            LevelGridNode node = this.nodes[index3 * this.width + index4 + this.width * this.depth * index2];
            if (node != null && node.Walkable)
            {
              bool flag = false;
              for (int i = 0; i < 4; ++i)
              {
                if (!node.GetConnection(i))
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
                node.Walkable = false;
            }
          }
        }
      }
      for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
      {
        for (int z = zmin; z < zmax; ++z)
        {
          for (int x = xmin; x < xmax; ++x)
          {
            LevelGridNode node = this.nodes[z * this.width + x + this.width * this.depth * layerIndex];
            if (node != null)
              this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) node, x, z, layerIndex);
          }
        }
      }
    }
  }

  public void CalculateConnections(
    GraphNode[] nodes,
    GraphNode node,
    int x,
    int z,
    int layerIndex)
  {
    if (node == null)
      return;
    LevelGridNode levelGridNode = (LevelGridNode) node;
    levelGridNode.ResetAllGridConnections();
    if (!node.Walkable)
      return;
    float num1 = layerIndex == this.layerCount - 1 || nodes[levelGridNode.NodeInGridIndex + this.width * this.depth * (layerIndex + 1)] == null ? float.PositiveInfinity : (float) Math.Abs(levelGridNode.position.y - nodes[levelGridNode.NodeInGridIndex + this.width * this.depth * (layerIndex + 1)].position.y) * (1f / 1000f);
    for (int dir = 0; dir < 4; ++dir)
    {
      int num2 = x + this.neighbourXOffsets[dir];
      int num3 = z + this.neighbourZOffsets[dir];
      if (num2 >= 0 && num3 >= 0 && num2 < this.width && num3 < this.depth)
      {
        int num4 = num3 * this.width + num2;
        int num5 = (int) byte.MaxValue;
        for (int index = 0; index < this.layerCount; ++index)
        {
          GraphNode node1 = nodes[num4 + this.width * this.depth * index];
          if (node1 != null && node1.Walkable)
          {
            float num6 = index == this.layerCount - 1 || nodes[num4 + this.width * this.depth * (index + 1)] == null ? float.PositiveInfinity : (float) Math.Abs(node1.position.y - nodes[num4 + this.width * this.depth * (index + 1)].position.y) * (1f / 1000f);
            float num7 = Mathf.Max((float) node1.position.y * (1f / 1000f), (float) levelGridNode.position.y * (1f / 1000f));
            if ((double) Mathf.Min((float) node1.position.y * (1f / 1000f) + num6, (float) levelGridNode.position.y * (1f / 1000f) + num1) - (double) num7 >= (double) this.characterHeight && (double) Mathf.Abs(node1.position.y - levelGridNode.position.y) * (1.0 / 1000.0) <= (double) this.maxClimb)
              num5 = index;
          }
        }
        levelGridNode.SetConnectionValue(dir, num5);
      }
    }
  }

  public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
  {
    if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length)
      return new NNInfo();
    Vector3 vector3 = this.inverseMatrix.MultiplyPoint3x4(position);
    int x = Mathf.Clamp(Mathf.RoundToInt(vector3.x - 0.5f), 0, this.width - 1);
    int z = Mathf.Clamp(Mathf.RoundToInt(vector3.z - 0.5f), 0, this.depth - 1);
    return new NNInfo((GraphNode) this.GetNearestNode(position, x, z, (NNConstraint) null));
  }

  public LevelGridNode GetNearestNode(Vector3 position, int x, int z, NNConstraint constraint)
  {
    int num1 = this.width * z + x;
    float num2 = float.PositiveInfinity;
    LevelGridNode nearestNode = (LevelGridNode) null;
    for (int index = 0; index < this.layerCount; ++index)
    {
      LevelGridNode node = this.nodes[num1 + this.width * this.depth * index];
      if (node != null)
      {
        float sqrMagnitude = ((Vector3) node.position - position).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num2 && (constraint == null || constraint.Suitable((GraphNode) node)))
        {
          num2 = sqrMagnitude;
          nearestNode = node;
        }
      }
    }
    return nearestNode;
  }

  public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
  {
    if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length || this.layerCount == 0)
      return new NNInfo();
    Vector3 position1 = position;
    position = this.inverseMatrix.MultiplyPoint3x4(position);
    int x1 = Mathf.Clamp(Mathf.RoundToInt(position.x - 0.5f), 0, this.width - 1);
    int z1 = Mathf.Clamp(Mathf.RoundToInt(position.z - 0.5f), 0, this.depth - 1);
    float num1 = float.PositiveInfinity;
    int num2 = 2;
    LevelGridNode node = this.GetNearestNode(position1, x1, z1, constraint);
    if (node != null)
      num1 = ((Vector3) node.position - position1).sqrMagnitude;
    if (node != null)
    {
      if (num2 == 0)
        return new NNInfo((GraphNode) node);
      --num2;
    }
    float num3 = constraint.constrainDistance ? AstarPath.active.maxNearestNodeDistance : float.PositiveInfinity;
    float num4 = num3 * num3;
    int num5 = 1;
    while (true)
    {
      int z2 = z1 + num5;
      if ((double) this.nodeSize * (double) num5 <= (double) num3)
      {
        for (int x2 = x1 - num5; x2 <= x1 + num5; ++x2)
        {
          if (x2 >= 0 && z2 >= 0 && x2 < this.width && z2 < this.depth)
          {
            LevelGridNode nearestNode = this.GetNearestNode(position1, x2, z2, constraint);
            if (nearestNode != null)
            {
              float sqrMagnitude = ((Vector3) nearestNode.position - position1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < (double) num4)
              {
                num1 = sqrMagnitude;
                node = nearestNode;
              }
            }
          }
        }
        int z3 = z1 - num5;
        for (int x3 = x1 - num5; x3 <= x1 + num5; ++x3)
        {
          if (x3 >= 0 && z3 >= 0 && x3 < this.width && z3 < this.depth)
          {
            LevelGridNode nearestNode = this.GetNearestNode(position1, x3, z3, constraint);
            if (nearestNode != null)
            {
              float sqrMagnitude = ((Vector3) nearestNode.position - position1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < (double) num4)
              {
                num1 = sqrMagnitude;
                node = nearestNode;
              }
            }
          }
        }
        int x4 = x1 - num5;
        for (int z4 = z1 - num5 + 1; z4 <= z1 + num5 - 1; ++z4)
        {
          if (x4 >= 0 && z4 >= 0 && x4 < this.width && z4 < this.depth)
          {
            LevelGridNode nearestNode = this.GetNearestNode(position1, x4, z4, constraint);
            if (nearestNode != null)
            {
              float sqrMagnitude = ((Vector3) nearestNode.position - position1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < (double) num4)
              {
                num1 = sqrMagnitude;
                node = nearestNode;
              }
            }
          }
        }
        int x5 = x1 + num5;
        for (int z5 = z1 - num5 + 1; z5 <= z1 + num5 - 1; ++z5)
        {
          if (x5 >= 0 && z5 >= 0 && x5 < this.width && z5 < this.depth)
          {
            LevelGridNode nearestNode = this.GetNearestNode(position1, x5, z5, constraint);
            if (nearestNode != null)
            {
              float sqrMagnitude = ((Vector3) nearestNode.position - position1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < (double) num4)
              {
                num1 = sqrMagnitude;
                node = nearestNode;
              }
            }
          }
        }
        if (node != null)
        {
          if (num2 != 0)
            --num2;
          else
            goto label_41;
        }
        ++num5;
      }
      else
        break;
    }
    return new NNInfo((GraphNode) node);
label_41:
    return new NNInfo((GraphNode) node);
  }

  public override GridNodeBase GetNeighbourAlongDirection(GridNodeBase node, int direction)
  {
    LevelGridNode levelGridNode = node as LevelGridNode;
    return levelGridNode.GetConnection(direction) ? (GridNodeBase) this.nodes[levelGridNode.NodeInGridIndex + this.neighbourOffsets[direction] + this.width * this.depth * levelGridNode.GetConnectionValue(direction)] : (GridNodeBase) null;
  }

  public static bool CheckConnection(LevelGridNode node, int dir) => node.GetConnection(dir);

  public override void OnDrawGizmos(bool drawNodes)
  {
    if (!drawNodes)
      return;
    base.OnDrawGizmos(false);
    if (this.nodes == null)
      return;
    PathHandler debugPathData = AstarPath.active.debugPathData;
    for (int index1 = 0; index1 < this.nodes.Length; ++index1)
    {
      LevelGridNode node1 = this.nodes[index1];
      if (node1 != null && node1.Walkable)
      {
        Gizmos.color = this.NodeColor((GraphNode) node1, AstarPath.active.debugPathData);
        if (AstarPath.active.showSearchTree && AstarPath.active.debugPathData != null)
        {
          if (NavGraph.InSearchTree((GraphNode) node1, AstarPath.active.debugPath))
          {
            PathNode pathNode = debugPathData.GetPathNode((GraphNode) node1);
            if (pathNode != null && pathNode.parent != null)
              Gizmos.DrawLine((Vector3) node1.position, (Vector3) pathNode.parent.node.position);
          }
        }
        else
        {
          for (int dir = 0; dir < 4; ++dir)
          {
            int connectionValue = node1.GetConnectionValue(dir);
            if (connectionValue != (int) byte.MaxValue)
            {
              int index2 = node1.NodeInGridIndex + this.neighbourOffsets[dir] + this.width * this.depth * connectionValue;
              if (index2 >= 0 && index2 < this.nodes.Length)
              {
                GraphNode node2 = (GraphNode) this.nodes[index2];
                if (node2 != null)
                  Gizmos.DrawLine((Vector3) node1.position, (Vector3) node2.position);
              }
            }
          }
        }
      }
    }
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
  }

  public override void DeserializeExtraInfo(GraphSerializationContext ctx)
  {
    int length = ctx.reader.ReadInt32();
    if (length == -1)
    {
      this.nodes = (LevelGridNode[]) null;
    }
    else
    {
      this.nodes = new LevelGridNode[length];
      for (int index = 0; index < this.nodes.Length; ++index)
      {
        if (ctx.reader.ReadInt32() != -1)
        {
          this.nodes[index] = new LevelGridNode(this.active);
          this.nodes[index].DeserializeNode(ctx);
        }
        else
          this.nodes[index] = (LevelGridNode) null;
      }
    }
  }

  public override void PostDeserialization()
  {
    this.GenerateMatrix();
    this.lastScannedWidth = this.width;
    this.lastScannedDepth = this.depth;
    this.SetUpOffsetsAndCosts();
    if (this.nodes == null || this.nodes.Length == 0)
      return;
    LevelGridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex((NavGraph) this), this);
    for (int index1 = 0; index1 < this.depth; ++index1)
    {
      for (int index2 = 0; index2 < this.width; ++index2)
      {
        for (int index3 = 0; index3 < this.layerCount; ++index3)
        {
          LevelGridNode node = this.nodes[index1 * this.width + index2 + this.width * this.depth * index3];
          if (node != null)
            node.NodeInGridIndex = index1 * this.width + index2;
        }
      }
    }
  }
}
