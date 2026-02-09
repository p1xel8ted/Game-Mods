// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.VoxelArea
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding.Voxels;

public class VoxelArea
{
  public const uint MaxHeight = 65536 /*0x010000*/;
  public const int MaxHeightInt = 65536 /*0x010000*/;
  public const uint InvalidSpanValue = 4294967295 /*0xFFFFFFFF*/;
  public const float AvgSpanLayerCountEstimate = 8f;
  public int width;
  public int depth;
  public CompactVoxelSpan[] compactSpans;
  public CompactVoxelCell[] compactCells;
  public int compactSpanCount;
  public ushort[] tmpUShortArr;
  public int[] areaTypes;
  public ushort[] dist;
  public ushort maxDistance;
  public int maxRegions;
  public int[] DirectionX;
  public int[] DirectionZ;
  public Vector3[] VectorDirection;
  public int linkedSpanCount;
  public LinkedVoxelSpan[] linkedSpans;
  public int[] removedStack = new int[128 /*0x80*/];
  public int removedStackCount;

  public void Reset()
  {
    this.ResetLinkedVoxelSpans();
    for (int index = 0; index < this.compactCells.Length; ++index)
    {
      this.compactCells[index].count = 0U;
      this.compactCells[index].index = 0U;
    }
  }

  public void ResetLinkedVoxelSpans()
  {
    int length = this.linkedSpans.Length;
    this.linkedSpanCount = this.width * this.depth;
    LinkedVoxelSpan linkedVoxelSpan = new LinkedVoxelSpan(uint.MaxValue, uint.MaxValue, -1, -1);
    int index1;
    for (int index2 = 0; index2 < length; index2 = index1 + 1)
    {
      this.linkedSpans[index2] = linkedVoxelSpan;
      int index3 = index2 + 1;
      this.linkedSpans[index3] = linkedVoxelSpan;
      int index4 = index3 + 1;
      this.linkedSpans[index4] = linkedVoxelSpan;
      int index5 = index4 + 1;
      this.linkedSpans[index5] = linkedVoxelSpan;
      int index6 = index5 + 1;
      this.linkedSpans[index6] = linkedVoxelSpan;
      int index7 = index6 + 1;
      this.linkedSpans[index7] = linkedVoxelSpan;
      int index8 = index7 + 1;
      this.linkedSpans[index8] = linkedVoxelSpan;
      int index9 = index8 + 1;
      this.linkedSpans[index9] = linkedVoxelSpan;
      int index10 = index9 + 1;
      this.linkedSpans[index10] = linkedVoxelSpan;
      int index11 = index10 + 1;
      this.linkedSpans[index11] = linkedVoxelSpan;
      int index12 = index11 + 1;
      this.linkedSpans[index12] = linkedVoxelSpan;
      int index13 = index12 + 1;
      this.linkedSpans[index13] = linkedVoxelSpan;
      int index14 = index13 + 1;
      this.linkedSpans[index14] = linkedVoxelSpan;
      int index15 = index14 + 1;
      this.linkedSpans[index15] = linkedVoxelSpan;
      int index16 = index15 + 1;
      this.linkedSpans[index16] = linkedVoxelSpan;
      index1 = index16 + 1;
      this.linkedSpans[index1] = linkedVoxelSpan;
    }
    this.removedStackCount = 0;
  }

  public VoxelArea(int width, int depth)
  {
    this.width = width;
    this.depth = depth;
    int length = width * depth;
    this.compactCells = new CompactVoxelCell[length];
    this.linkedSpans = new LinkedVoxelSpan[(int) ((double) length * 8.0) + 15 & -16];
    this.ResetLinkedVoxelSpans();
    this.DirectionX = new int[4]{ -1, 0, 1, 0 };
    this.DirectionZ = new int[4]{ 0, width, 0, -width };
    this.VectorDirection = new Vector3[4]
    {
      Vector3.left,
      Vector3.forward,
      Vector3.right,
      Vector3.back
    };
  }

  public int GetSpanCountAll()
  {
    int spanCountAll = 0;
    int num = this.width * this.depth;
    for (int index1 = 0; index1 < num; ++index1)
    {
      for (int index2 = index1; index2 != -1 && this.linkedSpans[index2].bottom != uint.MaxValue; index2 = this.linkedSpans[index2].next)
        ++spanCountAll;
    }
    return spanCountAll;
  }

  public int GetSpanCount()
  {
    int spanCount = 0;
    int num = this.width * this.depth;
    for (int index1 = 0; index1 < num; ++index1)
    {
      for (int index2 = index1; index2 != -1 && this.linkedSpans[index2].bottom != uint.MaxValue; index2 = this.linkedSpans[index2].next)
      {
        if (this.linkedSpans[index2].area != 0)
          ++spanCount;
      }
    }
    return spanCount;
  }

  public void PushToSpanRemovedStack(int index)
  {
    if (this.removedStackCount == this.removedStack.Length)
    {
      int[] dst = new int[this.removedStackCount * 4];
      Buffer.BlockCopy((Array) this.removedStack, 0, (Array) dst, 0, this.removedStackCount * 4);
      this.removedStack = dst;
    }
    this.removedStack[this.removedStackCount] = index;
    ++this.removedStackCount;
  }

  public void AddLinkedSpan(int index, uint bottom, uint top, int area, int voxelWalkableClimb)
  {
    if (this.linkedSpans[index].bottom == uint.MaxValue)
    {
      this.linkedSpans[index] = new LinkedVoxelSpan(bottom, top, area);
    }
    else
    {
      int index1 = -1;
      int index2 = index;
      while (index != -1 && this.linkedSpans[index].bottom <= top)
      {
        if (this.linkedSpans[index].top < bottom)
        {
          index1 = index;
          index = this.linkedSpans[index].next;
        }
        else
        {
          bottom = Math.Min(this.linkedSpans[index].bottom, bottom);
          top = Math.Max(this.linkedSpans[index].top, top);
          if (Math.Abs((int) top - (int) this.linkedSpans[index].top) <= voxelWalkableClimb)
            area = Math.Max(area, this.linkedSpans[index].area);
          int next = this.linkedSpans[index].next;
          if (index1 != -1)
          {
            this.linkedSpans[index1].next = next;
            this.PushToSpanRemovedStack(index);
            index = next;
          }
          else if (next != -1)
          {
            this.linkedSpans[index2] = this.linkedSpans[next];
            this.PushToSpanRemovedStack(next);
          }
          else
          {
            this.linkedSpans[index2] = new LinkedVoxelSpan(bottom, top, area);
            return;
          }
        }
      }
      if (this.linkedSpanCount >= this.linkedSpans.Length)
      {
        LinkedVoxelSpan[] linkedSpans = this.linkedSpans;
        int linkedSpanCount = this.linkedSpanCount;
        int removedStackCount = this.removedStackCount;
        this.linkedSpans = new LinkedVoxelSpan[this.linkedSpans.Length * 2];
        this.ResetLinkedVoxelSpans();
        this.linkedSpanCount = linkedSpanCount;
        this.removedStackCount = removedStackCount;
        for (int index3 = 0; index3 < this.linkedSpanCount; ++index3)
          this.linkedSpans[index3] = linkedSpans[index3];
        Debug.Log((object) "Layer estimate too low, doubling size of buffer.\nThis message is harmless.");
      }
      int next1;
      if (this.removedStackCount > 0)
      {
        --this.removedStackCount;
        next1 = this.removedStack[this.removedStackCount];
      }
      else
      {
        next1 = this.linkedSpanCount;
        ++this.linkedSpanCount;
      }
      if (index1 != -1)
      {
        this.linkedSpans[next1] = new LinkedVoxelSpan(bottom, top, area, this.linkedSpans[index1].next);
        this.linkedSpans[index1].next = next1;
      }
      else
      {
        this.linkedSpans[next1] = this.linkedSpans[index2];
        this.linkedSpans[index2] = new LinkedVoxelSpan(bottom, top, area, next1);
      }
    }
  }
}
