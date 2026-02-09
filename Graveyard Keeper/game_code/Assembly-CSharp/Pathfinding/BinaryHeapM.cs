// Decompiled with JetBrains decompiler
// Type: Pathfinding.BinaryHeapM
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding;

public class BinaryHeapM
{
  public int numberOfItems;
  public float growthFactor = 2f;
  public const int D = 4;
  public const bool SortGScores = true;
  public BinaryHeapM.Tuple[] binaryHeap;

  public BinaryHeapM(int numberOfElements)
  {
    this.binaryHeap = new BinaryHeapM.Tuple[numberOfElements];
    this.numberOfItems = 0;
  }

  public void Clear() => this.numberOfItems = 0;

  public PathNode GetNode(int i) => this.binaryHeap[i].node;

  public void SetF(int i, uint f) => this.binaryHeap[i].F = f;

  public void Add(PathNode node)
  {
    if (node == null)
      throw new ArgumentNullException(nameof (node));
    if (this.numberOfItems == this.binaryHeap.Length)
    {
      int length = Math.Max(this.binaryHeap.Length + 4, (int) Math.Round((double) this.binaryHeap.Length * (double) this.growthFactor));
      BinaryHeapM.Tuple[] tupleArray = length <= 262144 /*0x040000*/ ? new BinaryHeapM.Tuple[length] : throw new Exception("Binary Heap Size really large (2^18). A heap size this large is probably the cause of pathfinding running in an infinite loop. \nRemove this check (in BinaryHeap.cs) if you are sure that it is not caused by a bug");
      for (int index = 0; index < this.binaryHeap.Length; ++index)
        tupleArray[index] = this.binaryHeap[index];
      this.binaryHeap = tupleArray;
    }
    BinaryHeapM.Tuple tuple = new BinaryHeapM.Tuple(node.F, node);
    this.binaryHeap[this.numberOfItems] = tuple;
    int index1 = this.numberOfItems;
    uint f = node.F;
    uint g = node.G;
    int index2;
    for (; index1 != 0; index1 = index2)
    {
      index2 = (index1 - 1) / 4;
      if (f < this.binaryHeap[index2].F || (int) f == (int) this.binaryHeap[index2].F && g > this.binaryHeap[index2].node.G)
      {
        this.binaryHeap[index1] = this.binaryHeap[index2];
        this.binaryHeap[index2] = tuple;
      }
      else
        break;
    }
    ++this.numberOfItems;
  }

  public PathNode Remove()
  {
    --this.numberOfItems;
    PathNode node = this.binaryHeap[0].node;
    this.binaryHeap[0] = this.binaryHeap[this.numberOfItems];
    int index1 = 0;
    while (true)
    {
      int index2 = index1;
      uint f1 = this.binaryHeap[index1].F;
      int index3 = index2 * 4 + 1;
      if (index3 <= this.numberOfItems && (this.binaryHeap[index3].F < f1 || (int) this.binaryHeap[index3].F == (int) f1 && this.binaryHeap[index3].node.G < this.binaryHeap[index1].node.G))
      {
        f1 = this.binaryHeap[index3].F;
        index1 = index3;
      }
      if (index3 + 1 <= this.numberOfItems && (this.binaryHeap[index3 + 1].F < f1 || (int) this.binaryHeap[index3 + 1].F == (int) f1 && this.binaryHeap[index3 + 1].node.G < this.binaryHeap[index1].node.G))
      {
        f1 = this.binaryHeap[index3 + 1].F;
        index1 = index3 + 1;
      }
      if (index3 + 2 <= this.numberOfItems && (this.binaryHeap[index3 + 2].F < f1 || (int) this.binaryHeap[index3 + 2].F == (int) f1 && this.binaryHeap[index3 + 2].node.G < this.binaryHeap[index1].node.G))
      {
        f1 = this.binaryHeap[index3 + 2].F;
        index1 = index3 + 2;
      }
      if (index3 + 3 <= this.numberOfItems && (this.binaryHeap[index3 + 3].F < f1 || (int) this.binaryHeap[index3 + 3].F == (int) f1 && this.binaryHeap[index3 + 3].node.G < this.binaryHeap[index1].node.G))
      {
        uint f2 = this.binaryHeap[index3 + 3].F;
        index1 = index3 + 3;
      }
      if (index2 != index1)
      {
        BinaryHeapM.Tuple tuple = this.binaryHeap[index2];
        this.binaryHeap[index2] = this.binaryHeap[index1];
        this.binaryHeap[index1] = tuple;
      }
      else
        break;
    }
    return node;
  }

  public void Validate()
  {
    for (int index1 = 1; index1 < this.numberOfItems; ++index1)
    {
      int index2 = (index1 - 1) / 4;
      if (this.binaryHeap[index2].F > this.binaryHeap[index1].F)
        throw new Exception($"Invalid state at {index1.ToString()}:{index2.ToString()} ( {this.binaryHeap[index2].F.ToString()} > {this.binaryHeap[index1].F.ToString()} ) ");
    }
  }

  public void Rebuild()
  {
    for (int index1 = 2; index1 < this.numberOfItems; ++index1)
    {
      int index2 = index1;
      BinaryHeapM.Tuple tuple = this.binaryHeap[index1];
      uint f = tuple.F;
      int index3;
      for (; index2 != 1; index2 = index3)
      {
        index3 = index2 / 4;
        if (f < this.binaryHeap[index3].F)
        {
          this.binaryHeap[index2] = this.binaryHeap[index3];
          this.binaryHeap[index3] = tuple;
        }
        else
          break;
      }
    }
  }

  public struct Tuple(uint f, PathNode node)
  {
    public uint F = f;
    public PathNode node = node;
  }
}
