// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathHandler
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class PathHandler
{
  public ushort pathID;
  public int threadID;
  public int totalThreadCount;
  public BinaryHeapM heap = new BinaryHeapM(128 /*0x80*/);
  public const int BucketSizeLog2 = 10;
  public const int BucketSize = 1024 /*0x0400*/;
  public const int BucketIndexMask = 1023 /*0x03FF*/;
  public PathNode[][] nodes = new PathNode[0][];
  public bool[] bucketNew = new bool[0];
  public bool[] bucketCreated = new bool[0];
  public Stack<PathNode[]> bucketCache = new Stack<PathNode[]>();
  public int filledBuckets;
  public StringBuilder DebugStringBuilder = new StringBuilder();

  public ushort PathID => this.pathID;

  public void PushNode(PathNode node) => this.heap.Add(node);

  public PathNode PopNode() => this.heap.Remove();

  public BinaryHeapM GetHeap() => this.heap;

  public void RebuildHeap() => this.heap.Rebuild();

  public bool HeapEmpty() => this.heap.numberOfItems <= 0;

  public PathHandler(int threadID, int totalThreadCount)
  {
    this.threadID = threadID;
    this.totalThreadCount = totalThreadCount;
  }

  public void InitializeForPath(Path p)
  {
    this.pathID = p.pathID;
    this.heap.Clear();
  }

  public void DestroyNode(GraphNode node)
  {
    PathNode pathNode = this.GetPathNode(node);
    pathNode.node = (GraphNode) null;
    pathNode.parent = (PathNode) null;
  }

  public void InitializeNode(GraphNode node)
  {
    int nodeIndex = node.NodeIndex;
    int index1 = nodeIndex >> 10;
    int index2 = nodeIndex & 1023 /*0x03FF*/;
    if (index1 >= this.nodes.Length)
    {
      PathNode[][] pathNodeArray = new PathNode[Math.Max(Math.Max(this.nodes.Length * 3 / 2, index1 + 1), this.nodes.Length + 2)][];
      for (int index3 = 0; index3 < this.nodes.Length; ++index3)
        pathNodeArray[index3] = this.nodes[index3];
      bool[] flagArray1 = new bool[pathNodeArray.Length];
      for (int index4 = 0; index4 < this.nodes.Length; ++index4)
        flagArray1[index4] = this.bucketNew[index4];
      bool[] flagArray2 = new bool[pathNodeArray.Length];
      for (int index5 = 0; index5 < this.nodes.Length; ++index5)
        flagArray2[index5] = this.bucketCreated[index5];
      this.nodes = pathNodeArray;
      this.bucketNew = flagArray1;
      this.bucketCreated = flagArray2;
    }
    if (this.nodes[index1] == null)
    {
      PathNode[] pathNodeArray;
      if (this.bucketCache.Count > 0)
      {
        pathNodeArray = this.bucketCache.Pop();
      }
      else
      {
        pathNodeArray = new PathNode[1024 /*0x0400*/];
        for (int index6 = 0; index6 < 1024 /*0x0400*/; ++index6)
          pathNodeArray[index6] = new PathNode();
      }
      this.nodes[index1] = pathNodeArray;
      if (!this.bucketCreated[index1])
      {
        this.bucketNew[index1] = true;
        this.bucketCreated[index1] = true;
      }
      ++this.filledBuckets;
    }
    this.nodes[index1][index2].node = node;
  }

  public PathNode GetPathNode(int nodeIndex)
  {
    return this.nodes[nodeIndex >> 10][nodeIndex & 1023 /*0x03FF*/];
  }

  public PathNode GetPathNode(GraphNode node)
  {
    int nodeIndex = node.NodeIndex;
    try
    {
      return this.nodes[nodeIndex >> 10][nodeIndex & 1023 /*0x03FF*/];
    }
    catch (IndexOutOfRangeException ex)
    {
      Debug.LogException((Exception) ex);
      Debug.LogError((object) $"node index = {nodeIndex.ToString()}, pos = {(string) node.position}");
      return (PathNode) null;
    }
  }

  public void ClearPathIDs()
  {
    for (int index1 = 0; index1 < this.nodes.Length; ++index1)
    {
      PathNode[] node = this.nodes[index1];
      if (node != null)
      {
        for (int index2 = 0; index2 < 1024 /*0x0400*/; ++index2)
          node[index2].pathID = (ushort) 0;
      }
    }
  }
}
