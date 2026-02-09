// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.Voxelize
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding.Voxels;

public class Voxelize
{
  public static List<int[]> intArrCache = new List<int[]>();
  public static int[] emptyArr = new int[0];
  public List<ExtraMesh> inputExtraMeshes;
  public Vector3[] inputVertices;
  public int[] inputTriangles;
  public int voxelWalkableClimb;
  public uint voxelWalkableHeight;
  public float cellSize = 0.2f;
  public float cellHeight = 0.1f;
  public int minRegionSize = 100;
  public int borderSize;
  public float maxEdgeLength = 20f;
  public float maxSlope = 30f;
  public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;
  public Bounds forcedBounds;
  public VoxelArea voxelArea;
  public VoxelContourSet countourSet;
  public int width;
  public int depth;
  public Vector3 voxelOffset;
  public const uint NotConnected = 63 /*0x3F*/;
  public const int MaxLayers = 65535 /*0xFFFF*/;
  public const int MaxRegions = 500;
  public const int UnwalkableArea = 0;
  public const ushort BorderReg = 32768 /*0x8000*/;
  public const int RC_BORDER_VERTEX = 65536 /*0x010000*/;
  public const int RC_AREA_BORDER = 131072 /*0x020000*/;
  public const int VERTEX_BUCKET_COUNT = 4096 /*0x1000*/;
  public const int RC_CONTOUR_TESS_WALL_EDGES = 1;
  public const int RC_CONTOUR_TESS_AREA_EDGES = 2;
  public const int ContourRegMask = 65535 /*0xFFFF*/;
  public string debugString = "";
  public Vector3 cellScale;
  public Vector3 cellScaleDivision;

  public void BuildContours(
    float maxError,
    int maxEdgeLength,
    VoxelContourSet cset,
    int buildFlags)
  {
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    List<VoxelContour> voxelContourList = new List<VoxelContour>(Mathf.Max(8, 8));
    ushort[] flags = this.voxelArea.tmpUShortArr;
    if (flags.Length < this.voxelArea.compactSpanCount)
      flags = this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount];
    for (int index1 = 0; index1 < num1; index1 += this.voxelArea.width)
    {
      for (int index2 = 0; index2 < this.voxelArea.width; ++index2)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[index2 + index1];
        int index3 = (int) compactCell.index;
        for (int index4 = (int) compactCell.index + (int) compactCell.count; index3 < index4; ++index3)
        {
          ushort num2 = 0;
          CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[index3];
          if (compactSpan.reg == 0 || (compactSpan.reg & 32768 /*0x8000*/) == 32768 /*0x8000*/)
          {
            flags[index3] = (ushort) 0;
          }
          else
          {
            for (int dir = 0; dir < 4; ++dir)
            {
              int num3 = 0;
              if (compactSpan.GetConnection(dir) != 63 /*0x3F*/)
                num3 = this.voxelArea.compactSpans[(int) this.voxelArea.compactCells[index2 + this.voxelArea.DirectionX[dir] + (index1 + this.voxelArea.DirectionZ[dir])].index + compactSpan.GetConnection(dir)].reg;
              if (num3 == compactSpan.reg)
                num2 |= (ushort) (1 << dir);
            }
            flags[index3] = (ushort) ((uint) num2 ^ 15U);
          }
        }
      }
    }
    List<int> intList1 = ListPool<int>.Claim(256 /*0x0100*/);
    List<int> intList2 = ListPool<int>.Claim(64 /*0x40*/);
    for (int z = 0; z < num1; z += this.voxelArea.width)
    {
      for (int x = 0; x < this.voxelArea.width; ++x)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[x + z];
        int index5 = (int) compactCell.index;
        for (int index6 = (int) compactCell.index + (int) compactCell.count; index5 < index6; ++index5)
        {
          if (flags[index5] == (ushort) 0 || flags[index5] == (ushort) 15)
          {
            flags[index5] = (ushort) 0;
          }
          else
          {
            int reg = this.voxelArea.compactSpans[index5].reg;
            if (reg != 0 && (reg & 32768 /*0x8000*/) != 32768 /*0x8000*/)
            {
              int areaType = this.voxelArea.areaTypes[index5];
              intList1.Clear();
              intList2.Clear();
              this.WalkContour(x, z, index5, flags, intList1);
              this.SimplifyContour(intList1, intList2, maxError, maxEdgeLength, buildFlags);
              this.RemoveDegenerateSegments(intList2);
              VoxelContour voxelContour = new VoxelContour();
              voxelContour.verts = Voxelize.ClaimIntArr(intList2.Count, false);
              for (int index7 = 0; index7 < intList2.Count; ++index7)
                voxelContour.verts[index7] = intList2[index7];
              voxelContour.nverts = intList2.Count / 4;
              voxelContour.reg = reg;
              voxelContour.area = areaType;
              voxelContourList.Add(voxelContour);
            }
          }
        }
      }
    }
    ListPool<int>.Release(intList1);
    ListPool<int>.Release(intList2);
    for (int index8 = 0; index8 < voxelContourList.Count; ++index8)
    {
      VoxelContour cb = voxelContourList[index8];
      if (this.CalcAreaOfPolygon2D(cb.verts, cb.nverts) < 0)
      {
        int index9 = -1;
        for (int index10 = 0; index10 < voxelContourList.Count; ++index10)
        {
          if (index8 != index10 && voxelContourList[index10].nverts > 0 && voxelContourList[index10].reg == cb.reg && this.CalcAreaOfPolygon2D(voxelContourList[index10].verts, voxelContourList[index10].nverts) > 0)
          {
            index9 = index10;
            break;
          }
        }
        if (index9 == -1)
        {
          Debug.LogError((object) $"rcBuildContours: Could not find merge target for bad contour {index8.ToString()}.");
        }
        else
        {
          VoxelContour ca = voxelContourList[index9];
          int ia = 0;
          int ib = 0;
          this.GetClosestIndices(ca.verts, ca.nverts, cb.verts, cb.nverts, ref ia, ref ib);
          if (ia == -1 || ib == -1)
            Debug.LogWarning((object) $"rcBuildContours: Failed to find merge points for {index8.ToString()} and {index9.ToString()}.");
          else if (!Voxelize.MergeContours(ref ca, ref cb, ia, ib))
          {
            Debug.LogWarning((object) $"rcBuildContours: Failed to merge contours {index8.ToString()} and {index9.ToString()}.");
          }
          else
          {
            voxelContourList[index9] = ca;
            voxelContourList[index8] = cb;
          }
        }
      }
    }
    cset.conts = voxelContourList;
  }

  public void GetClosestIndices(
    int[] vertsa,
    int nvertsa,
    int[] vertsb,
    int nvertsb,
    ref int ia,
    ref int ib)
  {
    int num1 = 268435455 /*0x0FFFFFFF*/;
    ia = -1;
    ib = -1;
    for (int index1 = 0; index1 < nvertsa; ++index1)
    {
      int num2 = (index1 + 1) % nvertsa;
      int num3 = (index1 + nvertsa - 1) % nvertsa;
      int index2 = index1 * 4;
      int b = num2 * 4;
      int a = num3 * 4;
      for (int index3 = 0; index3 < nvertsb; ++index3)
      {
        int c = index3 * 4;
        if (Voxelize.Ileft(a, index2, c, vertsa, vertsa, vertsb) && Voxelize.Ileft(index2, b, c, vertsa, vertsa, vertsb))
        {
          int num4 = vertsb[c] - vertsa[index2];
          int num5 = vertsb[c + 2] / this.voxelArea.width - vertsa[index2 + 2] / this.voxelArea.width;
          int num6 = num4 * num4 + num5 * num5;
          if (num6 < num1)
          {
            ia = index1;
            ib = index3;
            num1 = num6;
          }
        }
      }
    }
  }

  public static void ReleaseIntArr(int[] arr)
  {
    if (arr == null)
      return;
    Voxelize.intArrCache.Add(arr);
  }

  public static int[] ClaimIntArr(int minCapacity, bool zero)
  {
    for (int index = 0; index < Voxelize.intArrCache.Count; ++index)
    {
      if (Voxelize.intArrCache[index].Length >= minCapacity)
      {
        int[] array = Voxelize.intArrCache[index];
        Voxelize.intArrCache.RemoveAt(index);
        if (zero)
          Memory.MemSet<int>(array, 0, 4);
        return array;
      }
    }
    return new int[minCapacity];
  }

  public static void ReleaseContours(VoxelContourSet cset)
  {
    for (int index = 0; index < cset.conts.Count; ++index)
    {
      VoxelContour cont = cset.conts[index];
      Voxelize.ReleaseIntArr(cont.verts);
      Voxelize.ReleaseIntArr(cont.rverts);
    }
    cset.conts = (List<VoxelContour>) null;
  }

  public static bool MergeContours(ref VoxelContour ca, ref VoxelContour cb, int ia, int ib)
  {
    int[] numArray = Voxelize.ClaimIntArr((ca.nverts + cb.nverts + 2) * 4, false);
    int num = 0;
    for (int index1 = 0; index1 <= ca.nverts; ++index1)
    {
      int index2 = num * 4;
      int index3 = (ia + index1) % ca.nverts * 4;
      numArray[index2] = ca.verts[index3];
      numArray[index2 + 1] = ca.verts[index3 + 1];
      numArray[index2 + 2] = ca.verts[index3 + 2];
      numArray[index2 + 3] = ca.verts[index3 + 3];
      ++num;
    }
    for (int index4 = 0; index4 <= cb.nverts; ++index4)
    {
      int index5 = num * 4;
      int index6 = (ib + index4) % cb.nverts * 4;
      numArray[index5] = cb.verts[index6];
      numArray[index5 + 1] = cb.verts[index6 + 1];
      numArray[index5 + 2] = cb.verts[index6 + 2];
      numArray[index5 + 3] = cb.verts[index6 + 3];
      ++num;
    }
    Voxelize.ReleaseIntArr(ca.verts);
    Voxelize.ReleaseIntArr(cb.verts);
    ca.verts = numArray;
    ca.nverts = num;
    cb.verts = Voxelize.emptyArr;
    cb.nverts = 0;
    return true;
  }

  public void SimplifyContour(
    List<int> verts,
    List<int> simplified,
    float maxError,
    int maxEdgeLenght,
    int buildFlags)
  {
    bool flag1 = false;
    for (int index = 0; index < verts.Count; index += 4)
    {
      if ((verts[index + 3] & (int) ushort.MaxValue) != 0)
      {
        flag1 = true;
        break;
      }
    }
    if (flag1)
    {
      int num1 = 0;
      for (int index = verts.Count / 4; num1 < index; ++num1)
      {
        int num2 = (num1 + 1) % index;
        if ((verts[num1 * 4 + 3] & (int) ushort.MaxValue) != (verts[num2 * 4 + 3] & (int) ushort.MaxValue) | (verts[num1 * 4 + 3] & 131072 /*0x020000*/) != (verts[num2 * 4 + 3] & 131072 /*0x020000*/))
        {
          simplified.Add(verts[num1 * 4]);
          simplified.Add(verts[num1 * 4 + 1]);
          simplified.Add(verts[num1 * 4 + 2]);
          simplified.Add(num1);
        }
      }
    }
    if (simplified.Count == 0)
    {
      int num3 = verts[0];
      int num4 = verts[1];
      int num5 = verts[2];
      int num6 = 0;
      int num7 = verts[0];
      int num8 = verts[1];
      int num9 = verts[2];
      int num10 = 0;
      for (int index = 0; index < verts.Count; index += 4)
      {
        int vert1 = verts[index];
        int vert2 = verts[index + 1];
        int vert3 = verts[index + 2];
        if (vert1 < num3 || vert1 == num3 && vert3 < num5)
        {
          num3 = vert1;
          num4 = vert2;
          num5 = vert3;
          num6 = index / 4;
        }
        if (vert1 > num7 || vert1 == num7 && vert3 > num9)
        {
          num7 = vert1;
          num8 = vert2;
          num9 = vert3;
          num10 = index / 4;
        }
      }
      simplified.Add(num3);
      simplified.Add(num4);
      simplified.Add(num5);
      simplified.Add(num6);
      simplified.Add(num7);
      simplified.Add(num8);
      simplified.Add(num9);
      simplified.Add(num10);
    }
    int num11 = verts.Count / 4;
    maxError *= maxError;
    int num12 = 0;
    while (num12 < simplified.Count / 4)
    {
      int num13 = (num12 + 1) % (simplified.Count / 4);
      int px = simplified[num12 * 4];
      int num14 = simplified[num12 * 4 + 2];
      int num15 = simplified[num12 * 4 + 3];
      int qx = simplified[num13 * 4];
      int num16 = simplified[num13 * 4 + 2];
      int num17 = simplified[num13 * 4 + 3];
      float num18 = 0.0f;
      int num19 = -1;
      int num20;
      int num21;
      int num22;
      if (qx > px || qx == px && num16 > num14)
      {
        num20 = 1;
        num21 = (num15 + num20) % num11;
        num22 = num17;
      }
      else
      {
        num20 = num11 - 1;
        num21 = (num17 + num20) % num11;
        num22 = num15;
      }
      if ((verts[num21 * 4 + 3] & (int) ushort.MaxValue) == 0 || (verts[num21 * 4 + 3] & 131072 /*0x020000*/) == 131072 /*0x020000*/)
      {
        for (; num21 != num22; num21 = (num21 + num20) % num11)
        {
          float num23 = VectorMath.SqrDistancePointSegmentApproximate(verts[num21 * 4], verts[num21 * 4 + 2] / this.voxelArea.width, px, num14 / this.voxelArea.width, qx, num16 / this.voxelArea.width);
          if ((double) num23 > (double) num18)
          {
            num18 = num23;
            num19 = num21;
          }
        }
      }
      if (num19 != -1 && (double) num18 > (double) maxError)
      {
        simplified.Add(0);
        simplified.Add(0);
        simplified.Add(0);
        simplified.Add(0);
        for (int index = simplified.Count / 4 - 1; index > num12; --index)
        {
          simplified[index * 4] = simplified[(index - 1) * 4];
          simplified[index * 4 + 1] = simplified[(index - 1) * 4 + 1];
          simplified[index * 4 + 2] = simplified[(index - 1) * 4 + 2];
          simplified[index * 4 + 3] = simplified[(index - 1) * 4 + 3];
        }
        simplified[(num12 + 1) * 4] = verts[num19 * 4];
        simplified[(num12 + 1) * 4 + 1] = verts[num19 * 4 + 1];
        simplified[(num12 + 1) * 4 + 2] = verts[num19 * 4 + 2];
        simplified[(num12 + 1) * 4 + 3] = num19;
      }
      else
        ++num12;
    }
    float num24 = this.maxEdgeLength / this.cellSize;
    if ((double) num24 > 0.0 && (buildFlags & 3) != 0)
    {
      int num25 = 0;
      while (num25 < simplified.Count / 4 && simplified.Count / 4 <= 200)
      {
        int num26 = (num25 + 1) % (simplified.Count / 4);
        int num27 = simplified[num25 * 4];
        int num28 = simplified[num25 * 4 + 2];
        int num29 = simplified[num25 * 4 + 3];
        int num30 = simplified[num26 * 4];
        int num31 = simplified[num26 * 4 + 2];
        int num32 = simplified[num26 * 4 + 3];
        int num33 = -1;
        int num34 = (num29 + 1) % num11;
        bool flag2 = false;
        if ((buildFlags & 1) == 1 && (verts[num34 * 4 + 3] & (int) ushort.MaxValue) == 0)
          flag2 = true;
        if ((buildFlags & 2) == 1 && (verts[num34 * 4 + 3] & 131072 /*0x020000*/) == 1)
          flag2 = true;
        if (flag2)
        {
          int num35 = num30 - num27;
          int num36 = num31 / this.voxelArea.width - num28 / this.voxelArea.width;
          if ((double) (num35 * num35 + num36 * num36) > (double) num24 * (double) num24)
          {
            if (num30 > num27 || num30 == num27 && num31 > num28)
            {
              int num37 = num32 < num29 ? num32 + num11 - num29 : num32 - num29;
              num33 = (num29 + num37 / 2) % num11;
            }
            else
            {
              int num38 = num32 < num29 ? num32 + num11 - num29 : num32 - num29;
              num33 = (num29 + (num38 + 1) / 2) % num11;
            }
          }
        }
        if (num33 != -1)
        {
          simplified.AddRange((IEnumerable<int>) new int[4]);
          for (int index = simplified.Count / 4 - 1; index > num25; --index)
          {
            simplified[index * 4] = simplified[(index - 1) * 4];
            simplified[index * 4 + 1] = simplified[(index - 1) * 4 + 1];
            simplified[index * 4 + 2] = simplified[(index - 1) * 4 + 2];
            simplified[index * 4 + 3] = simplified[(index - 1) * 4 + 3];
          }
          simplified[(num25 + 1) * 4] = verts[num33 * 4];
          simplified[(num25 + 1) * 4 + 1] = verts[num33 * 4 + 1];
          simplified[(num25 + 1) * 4 + 2] = verts[num33 * 4 + 2];
          simplified[(num25 + 1) * 4 + 3] = num33;
        }
        else
          ++num25;
      }
    }
    for (int index = 0; index < simplified.Count / 4; ++index)
    {
      int num39 = (simplified[index * 4 + 3] + 1) % num11;
      int num40 = simplified[index * 4 + 3];
      simplified[index * 4 + 3] = verts[num39 * 4 + 3] & (int) ushort.MaxValue | verts[num40 * 4 + 3] & 65536 /*0x010000*/;
    }
  }

  public void WalkContour(int x, int z, int i, ushort[] flags, List<int> verts)
  {
    int dir = 0;
    while (((int) flags[i] & (int) (ushort) (1 << dir)) == 0)
      ++dir;
    int num1 = dir;
    int num2 = i;
    int areaType = this.voxelArea.areaTypes[i];
    int num3 = 0;
    while (num3++ < 40000)
    {
      if (((int) flags[i] & (int) (ushort) (1 << dir)) != 0)
      {
        bool isBorderVertex = false;
        bool flag = false;
        int num4 = x;
        int cornerHeight = this.GetCornerHeight(x, z, i, dir, ref isBorderVertex);
        int num5 = z;
        switch (dir)
        {
          case 0:
            num5 += this.voxelArea.width;
            break;
          case 1:
            ++num4;
            num5 += this.voxelArea.width;
            break;
          case 2:
            ++num4;
            break;
        }
        int num6 = 0;
        CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[i];
        if (compactSpan.GetConnection(dir) != 63 /*0x3F*/)
        {
          int index = (int) this.voxelArea.compactCells[x + this.voxelArea.DirectionX[dir] + (z + this.voxelArea.DirectionZ[dir])].index + compactSpan.GetConnection(dir);
          num6 = this.voxelArea.compactSpans[index].reg;
          if (areaType != this.voxelArea.areaTypes[index])
            flag = true;
        }
        if (isBorderVertex)
          num6 |= 65536 /*0x010000*/;
        if (flag)
          num6 |= 131072 /*0x020000*/;
        verts.Add(num4);
        verts.Add(cornerHeight);
        verts.Add(num5);
        verts.Add(num6);
        flags[i] = (ushort) ((uint) flags[i] & (uint) ~(1 << dir));
        dir = dir + 1 & 3;
      }
      else
      {
        int num7 = -1;
        int num8 = x + this.voxelArea.DirectionX[dir];
        int num9 = z + this.voxelArea.DirectionZ[dir];
        CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[i];
        if (compactSpan.GetConnection(dir) != 63 /*0x3F*/)
          num7 = (int) this.voxelArea.compactCells[num8 + num9].index + compactSpan.GetConnection(dir);
        if (num7 == -1)
        {
          Debug.LogWarning((object) "Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
          break;
        }
        x = num8;
        z = num9;
        i = num7;
        dir = dir + 3 & 3;
      }
      if (num2 == i && num1 == dir)
        break;
    }
  }

  public int GetCornerHeight(int x, int z, int i, int dir, ref bool isBorderVertex)
  {
    CompactVoxelSpan compactSpan1 = this.voxelArea.compactSpans[i];
    int val1 = (int) compactSpan1.y;
    int dir1 = dir + 1 & 3;
    uint[] numArray = new uint[4]
    {
      (uint) (this.voxelArea.compactSpans[i].reg | this.voxelArea.areaTypes[i] << 16 /*0x10*/),
      0U,
      0U,
      0U
    };
    if (compactSpan1.GetConnection(dir) != 63 /*0x3F*/)
    {
      int num1 = x + this.voxelArea.DirectionX[dir];
      int num2 = z + this.voxelArea.DirectionZ[dir];
      int index1 = (int) this.voxelArea.compactCells[num1 + num2].index + compactSpan1.GetConnection(dir);
      CompactVoxelSpan compactSpan2 = this.voxelArea.compactSpans[index1];
      val1 = Math.Max(val1, (int) compactSpan2.y);
      numArray[1] = (uint) (compactSpan2.reg | this.voxelArea.areaTypes[index1] << 16 /*0x10*/);
      if (compactSpan2.GetConnection(dir1) != 63 /*0x3F*/)
      {
        int index2 = (int) this.voxelArea.compactCells[num1 + this.voxelArea.DirectionX[dir1] + (num2 + this.voxelArea.DirectionZ[dir1])].index + compactSpan2.GetConnection(dir1);
        CompactVoxelSpan compactSpan3 = this.voxelArea.compactSpans[index2];
        val1 = Math.Max(val1, (int) compactSpan3.y);
        numArray[2] = (uint) (compactSpan3.reg | this.voxelArea.areaTypes[index2] << 16 /*0x10*/);
      }
    }
    if (compactSpan1.GetConnection(dir1) != 63 /*0x3F*/)
    {
      int num3 = x + this.voxelArea.DirectionX[dir1];
      int num4 = z + this.voxelArea.DirectionZ[dir1];
      int index3 = (int) this.voxelArea.compactCells[num3 + num4].index + compactSpan1.GetConnection(dir1);
      CompactVoxelSpan compactSpan4 = this.voxelArea.compactSpans[index3];
      val1 = Math.Max(val1, (int) compactSpan4.y);
      numArray[3] = (uint) (compactSpan4.reg | this.voxelArea.areaTypes[index3] << 16 /*0x10*/);
      if (compactSpan4.GetConnection(dir) != 63 /*0x3F*/)
      {
        int index4 = (int) this.voxelArea.compactCells[num3 + this.voxelArea.DirectionX[dir] + (num4 + this.voxelArea.DirectionZ[dir])].index + compactSpan4.GetConnection(dir);
        CompactVoxelSpan compactSpan5 = this.voxelArea.compactSpans[index4];
        val1 = Math.Max(val1, (int) compactSpan5.y);
        numArray[2] = (uint) (compactSpan5.reg | this.voxelArea.areaTypes[index4] << 16 /*0x10*/);
      }
    }
    for (int index5 = 0; index5 < 4; ++index5)
    {
      int index6 = index5;
      int index7 = index5 + 1 & 3;
      int index8 = index5 + 2 & 3;
      int index9 = index5 + 3 & 3;
      int num5 = ((int) numArray[index6] & (int) numArray[index7] & 32768 /*0x8000*/) == 0 ? 0 : ((int) numArray[index6] == (int) numArray[index7] ? 1 : 0);
      bool flag1 = (((int) numArray[index8] | (int) numArray[index9]) & 32768 /*0x8000*/) == 0;
      bool flag2 = (int) (numArray[index8] >> 16 /*0x10*/) == (int) (numArray[index9] >> 16 /*0x10*/);
      bool flag3 = numArray[index6] != 0U && numArray[index7] != 0U && numArray[index8] != 0U && numArray[index9] > 0U;
      int num6 = flag1 ? 1 : 0;
      if ((num5 & num6 & (flag2 ? 1 : 0) & (flag3 ? 1 : 0)) != 0)
      {
        isBorderVertex = true;
        break;
      }
    }
    return val1;
  }

  public void RemoveDegenerateSegments(List<int> simplified)
  {
    for (int index = 0; index < simplified.Count / 4; ++index)
    {
      int num = index + 1;
      if (num >= simplified.Count / 4)
        num = 0;
      if (simplified[index * 4] == simplified[num * 4] && simplified[index * 4 + 2] == simplified[num * 4 + 2])
        simplified.RemoveRange(index, 4);
    }
  }

  public int CalcAreaOfPolygon2D(int[] verts, int nverts)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = nverts - 1;
    for (; num2 < nverts; num3 = num2++)
    {
      int index1 = num2 * 4;
      int index2 = num3 * 4;
      num1 += verts[index1] * (verts[index2 + 2] / this.voxelArea.width) - verts[index2] * (verts[index1 + 2] / this.voxelArea.width);
    }
    return (num1 + 1) / 2;
  }

  public static bool Ileft(int a, int b, int c, int[] va, int[] vb, int[] vc)
  {
    return (vb[b] - va[a]) * (vc[c + 2] - va[a + 2]) - (vc[c] - va[a]) * (vb[b + 2] - va[a + 2]) <= 0;
  }

  public static bool Diagonal(int i, int j, int n, int[] verts, int[] indices)
  {
    return Voxelize.InCone(i, j, n, verts, indices) && Voxelize.Diagonalie(i, j, n, verts, indices);
  }

  public static bool InCone(int i, int j, int n, int[] verts, int[] indices)
  {
    int num1 = (indices[i] & 268435455 /*0x0FFFFFFF*/) * 4;
    int num2 = (indices[j] & 268435455 /*0x0FFFFFFF*/) * 4;
    int c = (indices[Voxelize.Next(i, n)] & 268435455 /*0x0FFFFFFF*/) * 4;
    int num3 = (indices[Voxelize.Prev(i, n)] & 268435455 /*0x0FFFFFFF*/) * 4;
    return Voxelize.LeftOn(num3, num1, c, verts) ? Voxelize.Left(num1, num2, num3, verts) && Voxelize.Left(num2, num1, c, verts) : !Voxelize.LeftOn(num1, num2, c, verts) || !Voxelize.LeftOn(num2, num1, num3, verts);
  }

  public static bool Left(int a, int b, int c, int[] verts) => Voxelize.Area2(a, b, c, verts) < 0;

  public static bool LeftOn(int a, int b, int c, int[] verts)
  {
    return Voxelize.Area2(a, b, c, verts) <= 0;
  }

  public static bool Collinear(int a, int b, int c, int[] verts)
  {
    return Voxelize.Area2(a, b, c, verts) == 0;
  }

  public static int Area2(int a, int b, int c, int[] verts)
  {
    return (verts[b] - verts[a]) * (verts[c + 2] - verts[a + 2]) - (verts[c] - verts[a]) * (verts[b + 2] - verts[a + 2]);
  }

  public static bool Diagonalie(int i, int j, int n, int[] verts, int[] indices)
  {
    int a = (indices[i] & 268435455 /*0x0FFFFFFF*/) * 4;
    int num1 = (indices[j] & 268435455 /*0x0FFFFFFF*/) * 4;
    for (int i1 = 0; i1 < n; ++i1)
    {
      int index = Voxelize.Next(i1, n);
      if (i1 != i && index != i && i1 != j && index != j)
      {
        int num2 = (indices[i1] & 268435455 /*0x0FFFFFFF*/) * 4;
        int num3 = (indices[index] & 268435455 /*0x0FFFFFFF*/) * 4;
        if (!Voxelize.Vequal(a, num2, verts) && !Voxelize.Vequal(num1, num2, verts) && !Voxelize.Vequal(a, num3, verts) && !Voxelize.Vequal(num1, num3, verts) && Voxelize.Intersect(a, num1, num2, num3, verts))
          return false;
      }
    }
    return true;
  }

  public static bool Xorb(bool x, bool y) => !x ^ !y;

  public static bool IntersectProp(int a, int b, int c, int d, int[] verts)
  {
    return !Voxelize.Collinear(a, b, c, verts) && !Voxelize.Collinear(a, b, d, verts) && !Voxelize.Collinear(c, d, a, verts) && !Voxelize.Collinear(c, d, b, verts) && Voxelize.Xorb(Voxelize.Left(a, b, c, verts), Voxelize.Left(a, b, d, verts)) && Voxelize.Xorb(Voxelize.Left(c, d, a, verts), Voxelize.Left(c, d, b, verts));
  }

  public static bool Between(int a, int b, int c, int[] verts)
  {
    if (!Voxelize.Collinear(a, b, c, verts))
      return false;
    if (verts[a] != verts[b])
    {
      if (verts[a] <= verts[c] && verts[c] <= verts[b])
        return true;
      return verts[a] >= verts[c] && verts[c] >= verts[b];
    }
    if (verts[a + 2] <= verts[c + 2] && verts[c + 2] <= verts[b + 2])
      return true;
    return verts[a + 2] >= verts[c + 2] && verts[c + 2] >= verts[b + 2];
  }

  public static bool Intersect(int a, int b, int c, int d, int[] verts)
  {
    return Voxelize.IntersectProp(a, b, c, d, verts) || Voxelize.Between(a, b, c, verts) || Voxelize.Between(a, b, d, verts) || Voxelize.Between(c, d, a, verts) || Voxelize.Between(c, d, b, verts);
  }

  public static bool Vequal(int a, int b, int[] verts)
  {
    return verts[a] == verts[b] && verts[a + 2] == verts[b + 2];
  }

  public static int Prev(int i, int n) => i - 1 < 0 ? n - 1 : i - 1;

  public static int Next(int i, int n) => i + 1 >= n ? 0 : i + 1;

  public void BuildPolyMesh(VoxelContourSet cset, int nvp, out VoxelMesh mesh)
  {
    nvp = 3;
    int length1 = 0;
    int num1 = 0;
    int val1 = 0;
    for (int index = 0; index < cset.conts.Count; ++index)
    {
      if (cset.conts[index].nverts >= 3)
      {
        length1 += cset.conts[index].nverts;
        num1 += cset.conts[index].nverts - 2;
        val1 = Math.Max(val1, cset.conts[index].nverts);
      }
    }
    if (length1 >= 65534)
      Debug.LogWarning((object) "To many vertices for unity to render - Unity might screw up rendering, but hopefully the navmesh will work ok");
    Int3[] int3Array1 = new Int3[length1];
    int[] numArray = new int[num1 * nvp];
    Memory.MemSet<int>(numArray, (int) byte.MaxValue, 4);
    int[] indices = new int[val1];
    int[] tris = new int[val1 * 3];
    int length2 = 0;
    int length3 = 0;
    for (int index1 = 0; index1 < cset.conts.Count; ++index1)
    {
      VoxelContour cont = cset.conts[index1];
      if (cont.nverts >= 3)
      {
        for (int index2 = 0; index2 < cont.nverts; ++index2)
        {
          indices[index2] = index2;
          cont.verts[index2 * 4 + 2] /= this.voxelArea.width;
        }
        int num2 = this.Triangulate(cont.nverts, cont.verts, ref indices, ref tris);
        int num3 = length2;
        for (int index3 = 0; index3 < num2 * 3; ++index3)
        {
          numArray[length3] = tris[index3] + num3;
          ++length3;
        }
        for (int index4 = 0; index4 < cont.nverts; ++index4)
        {
          int3Array1[length2] = new Int3(cont.verts[index4 * 4], cont.verts[index4 * 4 + 1], cont.verts[index4 * 4 + 2]);
          ++length2;
        }
      }
    }
    mesh = new VoxelMesh();
    Int3[] int3Array2 = new Int3[length2];
    for (int index = 0; index < length2; ++index)
      int3Array2[index] = int3Array1[index];
    int[] dst = new int[length3];
    Buffer.BlockCopy((Array) numArray, 0, (Array) dst, 0, length3 * 4);
    mesh.verts = int3Array2;
    mesh.tris = dst;
  }

  public int Triangulate(int n, int[] verts, ref int[] indices, ref int[] tris)
  {
    int num1 = 0;
    int[] numArray = tris;
    int index1 = 0;
    for (int i1 = 0; i1 < n; ++i1)
    {
      int i2 = Voxelize.Next(i1, n);
      int j = Voxelize.Next(i2, n);
      if (Voxelize.Diagonal(i1, j, n, verts, indices))
        indices[i2] |= 1073741824 /*0x40000000*/;
    }
    while (n > 3)
    {
      int num2 = -1;
      int num3 = -1;
      for (int i3 = 0; i3 < n; ++i3)
      {
        int i4 = Voxelize.Next(i3, n);
        if ((indices[i4] & 1073741824 /*0x40000000*/) != 0)
        {
          int index2 = (indices[i3] & 268435455 /*0x0FFFFFFF*/) * 4;
          int index3 = (indices[Voxelize.Next(i4, n)] & 268435455 /*0x0FFFFFFF*/) * 4;
          int num4 = verts[index3] - verts[index2];
          int num5 = verts[index3 + 2] - verts[index2 + 2];
          int num6 = num4 * num4 + num5 * num5;
          if (num2 < 0 || num6 < num2)
          {
            num2 = num6;
            num3 = i3;
          }
        }
      }
      if (num3 == -1)
      {
        Debug.LogWarning((object) "Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
        return -num1;
      }
      int i5 = num3;
      int index4 = Voxelize.Next(i5, n);
      int index5 = Voxelize.Next(index4, n);
      numArray[index1] = indices[i5] & 268435455 /*0x0FFFFFFF*/;
      int index6 = index1 + 1;
      numArray[index6] = indices[index4] & 268435455 /*0x0FFFFFFF*/;
      int index7 = index6 + 1;
      numArray[index7] = indices[index5] & 268435455 /*0x0FFFFFFF*/;
      index1 = index7 + 1;
      ++num1;
      --n;
      for (int index8 = index4; index8 < n; ++index8)
        indices[index8] = indices[index8 + 1];
      if (index4 >= n)
        index4 = 0;
      int i6 = Voxelize.Prev(index4, n);
      if (Voxelize.Diagonal(Voxelize.Prev(i6, n), index4, n, verts, indices))
        indices[i6] |= 1073741824 /*0x40000000*/;
      else
        indices[i6] &= 268435455 /*0x0FFFFFFF*/;
      if (Voxelize.Diagonal(i6, Voxelize.Next(index4, n), n, verts, indices))
        indices[index4] |= 1073741824 /*0x40000000*/;
      else
        indices[index4] &= 268435455 /*0x0FFFFFFF*/;
    }
    numArray[index1] = indices[0] & 268435455 /*0x0FFFFFFF*/;
    int index9 = index1 + 1;
    numArray[index9] = indices[1] & 268435455 /*0x0FFFFFFF*/;
    int index10 = index9 + 1;
    numArray[index10] = indices[2] & 268435455 /*0x0FFFFFFF*/;
    int num7 = index10 + 1;
    return num1 + 1;
  }

  public Vector3 CompactSpanToVector(int x, int z, int i)
  {
    return this.voxelOffset + new Vector3(((float) x + 0.5f) * this.cellSize, (float) this.voxelArea.compactSpans[i].y * this.cellHeight, ((float) z + 0.5f) * this.cellSize);
  }

  public void VectorToIndex(Vector3 p, out int x, out int z)
  {
    p -= this.voxelOffset;
    x = Mathf.RoundToInt((float) ((double) p.x / (double) this.cellSize - 0.5));
    z = Mathf.RoundToInt((float) ((double) p.z / (double) this.cellSize - 0.5));
  }

  public void OnGUI() => GUI.Label(new Rect(5f, 5f, 200f, (float) Screen.height), this.debugString);

  public Voxelize(float ch, float cs, float wc, float wh, float ms)
  {
    this.cellSize = cs;
    this.cellHeight = ch;
    float num1 = wh;
    float num2 = wc;
    this.maxSlope = ms;
    this.cellScale = new Vector3(this.cellSize, this.cellHeight, this.cellSize);
    this.cellScaleDivision = new Vector3(1f / this.cellSize, 1f / this.cellHeight, 1f / this.cellSize);
    this.voxelWalkableHeight = (uint) ((double) num1 / (double) this.cellHeight);
    this.voxelWalkableClimb = Mathf.RoundToInt(num2 / this.cellHeight);
  }

  public void CollectMeshes()
  {
    Voxelize.CollectMeshes(this.inputExtraMeshes, this.forcedBounds, out this.inputVertices, out this.inputTriangles);
  }

  public static void CollectMeshes(
    List<ExtraMesh> extraMeshes,
    Bounds bounds,
    out Vector3[] verts,
    out int[] tris)
  {
    verts = (Vector3[]) null;
    tris = (int[]) null;
  }

  public void Init()
  {
    if (this.voxelArea == null || this.voxelArea.width != this.width || this.voxelArea.depth != this.depth)
      this.voxelArea = new VoxelArea(this.width, this.depth);
    else
      this.voxelArea.Reset();
  }

  public void VoxelizeInput()
  {
    Vector3 min = this.forcedBounds.min;
    this.voxelOffset = min;
    float num1 = 1f / this.cellSize;
    float y = 1f / this.cellHeight;
    float num2 = Mathf.Cos(Mathf.Atan(Mathf.Tan(this.maxSlope * ((float) Math.PI / 180f)) * (y * this.cellSize)));
    float[] numArray1 = new float[9];
    float[] numArray2 = new float[21];
    float[] numArray3 = new float[21];
    float[] numArray4 = new float[21];
    float[] vOut = new float[21];
    if (this.inputExtraMeshes == null)
      throw new NullReferenceException("inputExtraMeshes not set");
    int val2_1 = 0;
    for (int index = 0; index < this.inputExtraMeshes.Count; ++index)
    {
      if (this.inputExtraMeshes[index].bounds.Intersects(this.forcedBounds))
        val2_1 = Math.Max(this.inputExtraMeshes[index].vertices.Length, val2_1);
    }
    Vector3[] vector3Array = new Vector3[val2_1];
    Matrix4x4 matrix4x4 = Matrix4x4.TRS(-new Vector3(0.5f, 0.0f, 0.5f), Quaternion.identity, Vector3.one) * Matrix4x4.Scale(new Vector3(num1, y, num1)) * Matrix4x4.TRS(-min, Quaternion.identity, Vector3.one);
    for (int index1 = 0; index1 < this.inputExtraMeshes.Count; ++index1)
    {
      ExtraMesh inputExtraMesh = this.inputExtraMeshes[index1];
      if (inputExtraMesh.bounds.Intersects(this.forcedBounds))
      {
        Matrix4x4 matrix1 = inputExtraMesh.matrix;
        Matrix4x4 matrix2 = matrix4x4 * matrix1;
        bool flag = VectorMath.ReversesFaceOrientations(matrix2);
        Vector3[] vertices = inputExtraMesh.vertices;
        int[] triangles = inputExtraMesh.triangles;
        int length = triangles.Length;
        for (int index2 = 0; index2 < vertices.Length; ++index2)
          vector3Array[index2] = matrix2.MultiplyPoint3x4(vertices[index2]);
        int area1 = inputExtraMesh.area;
        for (int index3 = 0; index3 < length; index3 += 3)
        {
          Vector3 v1 = vector3Array[triangles[index3]];
          Vector3 v2 = vector3Array[triangles[index3 + 1]];
          Vector3 v3 = vector3Array[triangles[index3 + 2]];
          if (flag)
          {
            Vector3 vector3 = v1;
            v1 = v3;
            v3 = vector3;
          }
          int num3 = (int) Utility.Min(v1.x, v2.x, v3.x);
          int num4 = (int) Utility.Min(v1.z, v2.z, v3.z);
          int num5 = (int) Math.Ceiling((double) Utility.Max(v1.x, v2.x, v3.x));
          int num6 = (int) Math.Ceiling((double) Utility.Max(v1.z, v2.z, v3.z));
          int num7 = Mathf.Clamp(num3, 0, this.voxelArea.width - 1);
          int num8 = Mathf.Clamp(num5, 0, this.voxelArea.width - 1);
          int num9 = Mathf.Clamp(num4, 0, this.voxelArea.depth - 1);
          int num10 = Mathf.Clamp(num6, 0, this.voxelArea.depth - 1);
          if (num7 < this.voxelArea.width && num9 < this.voxelArea.depth && num8 > 0 && num10 > 0)
          {
            int area2 = (double) Vector3.Dot(Vector3.Cross(v2 - v1, v3 - v1).normalized, Vector3.up) >= (double) num2 ? 1 + area1 : 0;
            Utility.CopyVector(numArray1, 0, v1);
            Utility.CopyVector(numArray1, 3, v2);
            Utility.CopyVector(numArray1, 6, v3);
            for (int index4 = num7; index4 <= num8; ++index4)
            {
              int n1 = Utility.ClipPolygon(numArray1, 3, numArray2, 1f, (float) -index4 + 0.5f, 0);
              if (n1 >= 3)
              {
                int n2 = Utility.ClipPolygon(numArray2, n1, numArray3, -1f, (float) index4 + 0.5f, 0);
                if (n2 >= 3)
                {
                  float num11 = numArray3[2];
                  float num12 = numArray3[2];
                  for (int index5 = 1; index5 < n2; ++index5)
                  {
                    float val2_2 = numArray3[index5 * 3 + 2];
                    num11 = Math.Min(num11, val2_2);
                    num12 = Math.Max(num12, val2_2);
                  }
                  int num13 = Mathf.Clamp((int) Math.Round((double) num11), 0, this.voxelArea.depth - 1);
                  int num14 = Mathf.Clamp((int) Math.Round((double) num12), 0, this.voxelArea.depth - 1);
                  for (int index6 = num13; index6 <= num14; ++index6)
                  {
                    int n3 = Utility.ClipPolygon(numArray3, n2, numArray4, 1f, (float) -index6 + 0.5f, 2);
                    if (n3 >= 3)
                    {
                      int num15 = Utility.ClipPolygonY(numArray4, n3, vOut, -1f, (float) index6 + 0.5f, 2);
                      if (num15 >= 3)
                      {
                        float num16 = vOut[1];
                        float num17 = vOut[1];
                        for (int index7 = 1; index7 < num15; ++index7)
                        {
                          float val2_3 = vOut[index7 * 3 + 1];
                          num16 = Math.Min(num16, val2_3);
                          num17 = Math.Max(num17, val2_3);
                        }
                        int val2_4 = (int) Math.Ceiling((double) num17);
                        if (val2_4 >= 0)
                        {
                          int bottom = Math.Max(0, (int) num16);
                          int top = Math.Max(bottom + 1, val2_4);
                          this.voxelArea.AddLinkedSpan(index6 * this.voxelArea.width + index4, (uint) bottom, (uint) top, area2, this.voxelWalkableClimb);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }

  public void DebugDrawSpans()
  {
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    Vector3 min = this.forcedBounds.min;
    LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
    int num2 = 0;
    int num3 = 0;
    while (num2 < num1)
    {
      for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
      {
        for (int index2 = num2 + index1; index2 != -1 && linkedSpans[index2].bottom != uint.MaxValue; index2 = linkedSpans[index2].next)
        {
          uint top = linkedSpans[index2].top;
          uint num4 = linkedSpans[index2].next != -1 ? linkedSpans[linkedSpans[index2].next].bottom : 65536U /*0x010000*/;
          if (top > num4)
          {
            Debug.Log((object) $"{top.ToString()} {num4.ToString()}");
            Debug.DrawLine(new Vector3((float) index1 * this.cellSize, (float) top * this.cellHeight, (float) num3 * this.cellSize) + min, new Vector3((float) index1 * this.cellSize, (float) num4 * this.cellHeight, (float) num3 * this.cellSize) + min, Color.yellow, 1f);
          }
          int num5 = (int) num4 - (int) top;
          int voxelWalkableHeight = (int) this.voxelWalkableHeight;
        }
      }
      num2 += this.voxelArea.width;
      ++num3;
    }
  }

  public void DebugDrawCompactSpans()
  {
    int length = this.voxelArea.compactSpans.Length;
    Vector3[] topVerts = new Vector3[length];
    Vector3[] bottomVerts = new Vector3[length];
    Color[] vertexColors = new Color[length];
    int index1 = 0;
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    Vector3 min = this.forcedBounds.min;
    int num2 = 0;
    int z = 0;
    while (num2 < num1)
    {
      for (int x = 0; x < this.voxelArea.width; ++x)
      {
        Vector3 vector3 = new Vector3((float) x, 0.0f, (float) z) * this.cellSize + min;
        CompactVoxelCell compactCell = this.voxelArea.compactCells[x + num2];
        int index2 = (int) compactCell.index;
        for (int index3 = (int) compactCell.index + (int) compactCell.count; index2 < index3; ++index2)
        {
          CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[index2];
          vector3.y = ((float) compactSpan.y + 0.1f) * this.cellHeight + min.y;
          topVerts[index1] = vector3;
          vector3.y = (float) compactSpan.y * this.cellHeight + min.y;
          bottomVerts[index1] = vector3;
          Color color = Color.black;
          switch (compactSpan.reg)
          {
            case 0:
              color = Color.red;
              break;
            case 1:
              color = Color.green;
              break;
            case 2:
              color = Color.yellow;
              break;
            case 3:
              color = Color.magenta;
              break;
          }
          vertexColors[index1] = color;
          ++index1;
        }
      }
      num2 += this.voxelArea.width;
      ++z;
    }
    DebugUtility.DrawCubes(topVerts, bottomVerts, vertexColors, this.cellSize);
  }

  public void BuildCompactField()
  {
    int spanCount = this.voxelArea.GetSpanCount();
    this.voxelArea.compactSpanCount = spanCount;
    if (this.voxelArea.compactSpans == null || this.voxelArea.compactSpans.Length < spanCount)
    {
      this.voxelArea.compactSpans = new CompactVoxelSpan[spanCount];
      this.voxelArea.areaTypes = new int[spanCount];
    }
    uint index1 = 0;
    int width = this.voxelArea.width;
    int depth = this.voxelArea.depth;
    int num1 = width * depth;
    if (this.voxelWalkableHeight >= (uint) ushort.MaxValue)
      Debug.LogWarning((object) "Too high walkable height to guarantee correctness. Increase voxel height or lower walkable height.");
    LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
    int num2 = 0;
    int num3 = 0;
    while (num2 < num1)
    {
      for (int index2 = 0; index2 < width; ++index2)
      {
        int index3 = index2 + num2;
        if (linkedSpans[index3].bottom == uint.MaxValue)
        {
          this.voxelArea.compactCells[index2 + num2] = new CompactVoxelCell(0U, 0U);
        }
        else
        {
          uint i = index1;
          uint c = 0;
          for (; index3 != -1; index3 = linkedSpans[index3].next)
          {
            if (linkedSpans[index3].area != 0)
            {
              int top = (int) linkedSpans[index3].top;
              int next = linkedSpans[index3].next;
              int num4 = next != -1 ? (int) linkedSpans[next].bottom : 65536 /*0x010000*/;
              this.voxelArea.compactSpans[(int) index1] = new CompactVoxelSpan(top > (int) ushort.MaxValue ? ushort.MaxValue : (ushort) top, num4 - top > (int) ushort.MaxValue ? (uint) ushort.MaxValue : (uint) (num4 - top));
              this.voxelArea.areaTypes[(int) index1] = linkedSpans[index3].area;
              ++index1;
              ++c;
            }
          }
          this.voxelArea.compactCells[index2 + num2] = new CompactVoxelCell(i, c);
        }
      }
      num2 += width;
      ++num3;
    }
  }

  public void BuildVoxelConnections()
  {
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    CompactVoxelSpan[] compactSpans = this.voxelArea.compactSpans;
    CompactVoxelCell[] compactCells = this.voxelArea.compactCells;
    int num2 = 0;
    int num3 = 0;
    while (num2 < num1)
    {
      for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
      {
        CompactVoxelCell compactVoxelCell1 = compactCells[index1 + num2];
        int index2 = (int) compactVoxelCell1.index;
        for (int index3 = (int) compactVoxelCell1.index + (int) compactVoxelCell1.count; index2 < index3; ++index2)
        {
          CompactVoxelSpan compactVoxelSpan1 = compactSpans[index2];
          compactSpans[index2].con = uint.MaxValue;
          for (int dir = 0; dir < 4; ++dir)
          {
            int num4 = index1 + this.voxelArea.DirectionX[dir];
            int num5 = num2 + this.voxelArea.DirectionZ[dir];
            if (num4 >= 0 && num5 >= 0 && num5 < num1 && num4 < this.voxelArea.width)
            {
              CompactVoxelCell compactVoxelCell2 = compactCells[num4 + num5];
              int index4 = (int) compactVoxelCell2.index;
              for (int index5 = (int) compactVoxelCell2.index + (int) compactVoxelCell2.count; index4 < index5; ++index4)
              {
                CompactVoxelSpan compactVoxelSpan2 = compactSpans[index4];
                int num6 = (int) Math.Max(compactVoxelSpan1.y, compactVoxelSpan2.y);
                if ((long) (Math.Min((int) compactVoxelSpan1.y + (int) compactVoxelSpan1.h, (int) compactVoxelSpan2.y + (int) compactVoxelSpan2.h) - num6) >= (long) this.voxelWalkableHeight && Math.Abs((int) compactVoxelSpan2.y - (int) compactVoxelSpan1.y) <= this.voxelWalkableClimb)
                {
                  uint num7 = (uint) index4 - compactVoxelCell2.index;
                  if (num7 > (uint) ushort.MaxValue)
                  {
                    Debug.LogError((object) "Too many layers");
                  }
                  else
                  {
                    compactSpans[index2].SetConnection(dir, num7);
                    break;
                  }
                }
              }
            }
          }
        }
      }
      num2 += this.voxelArea.width;
      ++num3;
    }
  }

  public void DrawLine(int a, int b, int[] indices, int[] verts, Color col)
  {
    int index1 = (indices[a] & 268435455 /*0x0FFFFFFF*/) * 4;
    int index2 = (indices[b] & 268435455 /*0x0FFFFFFF*/) * 4;
    Debug.DrawLine(this.ConvertPosCorrZ(verts[index1], verts[index1 + 1], verts[index1 + 2]), this.ConvertPosCorrZ(verts[index2], verts[index2 + 1], verts[index2 + 2]), col);
  }

  public Vector3 ConvertPos(int x, int y, int z)
  {
    return Vector3.Scale(new Vector3((float) x + 0.5f, (float) y, (float) ((double) z / (double) this.voxelArea.width + 0.5)), this.cellScale) + this.voxelOffset;
  }

  public Vector3 ConvertPosCorrZ(int x, int y, int z)
  {
    return Vector3.Scale(new Vector3((float) x, (float) y, (float) z), this.cellScale) + this.voxelOffset;
  }

  public Vector3 ConvertPosWithoutOffset(int x, int y, int z)
  {
    return Vector3.Scale(new Vector3((float) x, (float) y, (float) z / (float) this.voxelArea.width), this.cellScale) + this.voxelOffset;
  }

  public Vector3 ConvertPosition(int x, int z, int i)
  {
    CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[i];
    return new Vector3((float) x * this.cellSize, (float) compactSpan.y * this.cellHeight, (float) z / (float) this.voxelArea.width * this.cellSize) + this.voxelOffset;
  }

  public void ErodeWalkableArea(int radius)
  {
    ushort[] numArray = this.voxelArea.tmpUShortArr;
    if (numArray == null || numArray.Length < this.voxelArea.compactSpanCount)
      numArray = this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount];
    Memory.MemSet<ushort>(numArray, ushort.MaxValue, 2);
    int distanceField = (int) this.CalculateDistanceField(numArray);
    for (int index = 0; index < numArray.Length; ++index)
    {
      if ((int) numArray[index] < radius * 2)
        this.voxelArea.areaTypes[index] = 0;
    }
  }

  public void BuildDistanceField()
  {
    ushort[] numArray = this.voxelArea.tmpUShortArr;
    if (numArray == null || numArray.Length < this.voxelArea.compactSpanCount)
      numArray = this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount];
    Memory.MemSet<ushort>(numArray, ushort.MaxValue, 2);
    this.voxelArea.maxDistance = this.CalculateDistanceField(numArray);
    ushort[] dst = this.voxelArea.dist;
    if (dst == null || dst.Length < this.voxelArea.compactSpanCount)
      dst = new ushort[this.voxelArea.compactSpanCount];
    this.voxelArea.dist = this.BoxBlur(numArray, dst);
  }

  [Obsolete("This function is not complete and should not be used")]
  public void ErodeVoxels(int radius)
  {
    if (radius > (int) byte.MaxValue)
    {
      Debug.LogError((object) "Max Erode Radius is 255");
      radius = (int) byte.MaxValue;
    }
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    int[] numArray = new int[this.voxelArea.compactSpanCount];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = (int) byte.MaxValue;
    for (int index1 = 0; index1 < num1; index1 += this.voxelArea.width)
    {
      for (int index2 = 0; index2 < this.voxelArea.width; ++index2)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[index2 + index1];
        int index3 = (int) compactCell.index;
        for (int index4 = (int) compactCell.index + (int) compactCell.count; index3 < index4; ++index3)
        {
          if (this.voxelArea.areaTypes[index3] != 0)
          {
            CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[index3];
            int num2 = 0;
            for (int dir = 0; dir < 4; ++dir)
            {
              if (compactSpan.GetConnection(dir) != 63 /*0x3F*/)
                ++num2;
            }
            if (num2 != 4)
              numArray[index3] = 0;
          }
        }
      }
    }
  }

  public void FilterLowHeightSpans(uint voxelWalkableHeight, float cs, float ch, Vector3 min)
  {
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
    int num2 = 0;
    int num3 = 0;
    while (num2 < num1)
    {
      for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
      {
        for (int index2 = num2 + index1; index2 != -1 && linkedSpans[index2].bottom != uint.MaxValue; index2 = linkedSpans[index2].next)
        {
          uint top = linkedSpans[index2].top;
          if ((linkedSpans[index2].next != -1 ? linkedSpans[linkedSpans[index2].next].bottom : 65536U /*0x010000*/) - top < voxelWalkableHeight)
            linkedSpans[index2].area = 0;
        }
      }
      num2 += this.voxelArea.width;
      ++num3;
    }
  }

  public void FilterLedges(
    uint voxelWalkableHeight,
    int voxelWalkableClimb,
    float cs,
    float ch,
    Vector3 min)
  {
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    LinkedVoxelSpan[] linkedSpans = this.voxelArea.linkedSpans;
    int[] directionX = this.voxelArea.DirectionX;
    int[] directionZ = this.voxelArea.DirectionZ;
    int width = this.voxelArea.width;
    int num2 = 0;
    int num3 = 0;
    while (num2 < num1)
    {
      for (int index1 = 0; index1 < width; ++index1)
      {
        if (linkedSpans[index1 + num2].bottom != uint.MaxValue)
        {
          for (int index2 = index1 + num2; index2 != -1; index2 = linkedSpans[index2].next)
          {
            if (linkedSpans[index2].area != 0)
            {
              int top1 = (int) linkedSpans[index2].top;
              int val1_1 = linkedSpans[index2].next != -1 ? (int) linkedSpans[linkedSpans[index2].next].bottom : 65536 /*0x010000*/;
              int val1_2 = 65536 /*0x010000*/;
              int num4 = (int) linkedSpans[index2].top;
              int num5 = num4;
              for (int index3 = 0; index3 < 4; ++index3)
              {
                int num6 = index1 + directionX[index3];
                int num7 = num2 + directionZ[index3];
                if (num6 < 0 || num7 < 0 || num7 >= num1 || num6 >= width)
                {
                  linkedSpans[index2].area = 0;
                  break;
                }
                int index4 = num6 + num7;
                int val2_1 = -voxelWalkableClimb;
                int val2_2 = linkedSpans[index4].bottom != uint.MaxValue ? (int) linkedSpans[index4].bottom : 65536 /*0x010000*/;
                if ((long) (Math.Min(val1_1, val2_2) - Math.Max(top1, val2_1)) > (long) voxelWalkableHeight)
                  val1_2 = Math.Min(val1_2, val2_1 - top1);
                if (linkedSpans[index4].bottom != uint.MaxValue)
                {
                  for (int index5 = index4; index5 != -1; index5 = linkedSpans[index5].next)
                  {
                    int top2 = (int) linkedSpans[index5].top;
                    int val2_3 = linkedSpans[index5].next != -1 ? (int) linkedSpans[linkedSpans[index5].next].bottom : 65536 /*0x010000*/;
                    if ((long) (Math.Min(val1_1, val2_3) - Math.Max(top1, top2)) > (long) voxelWalkableHeight)
                    {
                      val1_2 = Math.Min(val1_2, top2 - top1);
                      if (Math.Abs(top2 - top1) <= voxelWalkableClimb)
                      {
                        if (top2 < num4)
                          num4 = top2;
                        if (top2 > num5)
                          num5 = top2;
                      }
                    }
                  }
                }
              }
              if (val1_2 < -voxelWalkableClimb || num5 - num4 > voxelWalkableClimb)
                linkedSpans[index2].area = 0;
            }
          }
        }
      }
      num2 += width;
      ++num3;
    }
  }

  public ushort[] ExpandRegions(
    int maxIterations,
    uint level,
    ushort[] srcReg,
    ushort[] srcDist,
    ushort[] dstReg,
    ushort[] dstDist,
    List<int> stack)
  {
    int width = this.voxelArea.width;
    int depth = this.voxelArea.depth;
    int num1 = width * depth;
    stack.Clear();
    int num2 = 0;
    int num3 = 0;
    while (num2 < num1)
    {
      for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[num2 + index1];
        int index2 = (int) compactCell.index;
        for (int index3 = (int) compactCell.index + (int) compactCell.count; index2 < index3; ++index2)
        {
          if ((uint) this.voxelArea.dist[index2] >= level && srcReg[index2] == (ushort) 0 && this.voxelArea.areaTypes[index2] != 0)
          {
            stack.Add(index1);
            stack.Add(num2);
            stack.Add(index2);
          }
        }
      }
      num2 += width;
      ++num3;
    }
    int num4 = 0;
    int count = stack.Count;
    if (count > 0)
    {
      do
      {
        do
        {
          int num5 = 0;
          Buffer.BlockCopy((Array) srcReg, 0, (Array) dstReg, 0, srcReg.Length * 2);
          Buffer.BlockCopy((Array) srcDist, 0, (Array) dstDist, 0, dstDist.Length * 2);
          for (int index4 = 0; index4 < count && index4 < count; index4 += 3)
          {
            int num6 = stack[index4];
            int num7 = stack[index4 + 1];
            int index5 = stack[index4 + 2];
            if (index5 < 0)
            {
              ++num5;
            }
            else
            {
              ushort num8 = srcReg[index5];
              ushort num9 = ushort.MaxValue;
              CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[index5];
              int areaType = this.voxelArea.areaTypes[index5];
              for (int dir = 0; dir < 4; ++dir)
              {
                if (compactSpan.GetConnection(dir) != 63 /*0x3F*/)
                {
                  int index6 = (int) this.voxelArea.compactCells[num6 + this.voxelArea.DirectionX[dir] + (num7 + this.voxelArea.DirectionZ[dir])].index + compactSpan.GetConnection(dir);
                  if (areaType == this.voxelArea.areaTypes[index6] && srcReg[index6] > (ushort) 0 && ((int) srcReg[index6] & 32768 /*0x8000*/) == 0 && (int) srcDist[index6] + 2 < (int) num9)
                  {
                    num8 = srcReg[index6];
                    num9 = (ushort) ((uint) srcDist[index6] + 2U);
                  }
                }
              }
              if (num8 != (ushort) 0)
              {
                stack[index4 + 2] = -1;
                dstReg[index5] = num8;
                dstDist[index5] = num9;
              }
              else
                ++num5;
            }
          }
          ushort[] numArray1 = srcReg;
          srcReg = dstReg;
          dstReg = numArray1;
          ushort[] numArray2 = srcDist;
          srcDist = dstDist;
          dstDist = numArray2;
          if (num5 * 3 >= count)
            goto label_29;
        }
        while (level <= 0U);
        ++num4;
      }
      while (num4 < maxIterations);
    }
label_29:
    return srcReg;
  }

  public bool FloodRegion(
    int x,
    int z,
    int i,
    uint level,
    ushort r,
    ushort[] srcReg,
    ushort[] srcDist,
    List<int> stack)
  {
    int areaType = this.voxelArea.areaTypes[i];
    stack.Clear();
    stack.Add(x);
    stack.Add(z);
    stack.Add(i);
    srcReg[i] = r;
    srcDist[i] = (ushort) 0;
    int num1 = level >= 2U ? (int) level - 2 : 0;
    int num2 = 0;
    while (stack.Count > 0)
    {
      int index1 = stack[stack.Count - 1];
      stack.RemoveAt(stack.Count - 1);
      int num3 = stack[stack.Count - 1];
      stack.RemoveAt(stack.Count - 1);
      int num4 = stack[stack.Count - 1];
      stack.RemoveAt(stack.Count - 1);
      CompactVoxelSpan compactSpan1 = this.voxelArea.compactSpans[index1];
      ushort num5 = 0;
      for (int dir1 = 0; dir1 < 4; ++dir1)
      {
        if (compactSpan1.GetConnection(dir1) != 63 /*0x3F*/)
        {
          int num6 = num4 + this.voxelArea.DirectionX[dir1];
          int num7 = num3 + this.voxelArea.DirectionZ[dir1];
          int index2 = (int) this.voxelArea.compactCells[num6 + num7].index + compactSpan1.GetConnection(dir1);
          if (this.voxelArea.areaTypes[index2] == areaType)
          {
            ushort num8 = srcReg[index2];
            if (((int) num8 & 32768 /*0x8000*/) != 32768 /*0x8000*/)
            {
              if (num8 != (ushort) 0 && (int) num8 != (int) r)
                num5 = num8;
              CompactVoxelSpan compactSpan2 = this.voxelArea.compactSpans[index2];
              int dir2 = dir1 + 1 & 3;
              if (compactSpan2.GetConnection(dir2) != 63 /*0x3F*/)
              {
                int index3 = (int) this.voxelArea.compactCells[num6 + this.voxelArea.DirectionX[dir2] + (num7 + this.voxelArea.DirectionZ[dir2])].index + compactSpan2.GetConnection(dir2);
                if (this.voxelArea.areaTypes[index3] == areaType)
                {
                  ushort num9 = srcReg[index3];
                  if (num9 != (ushort) 0 && (int) num9 != (int) r)
                    num5 = num9;
                }
              }
            }
          }
        }
      }
      if (num5 != (ushort) 0)
      {
        srcReg[index1] = (ushort) 0;
      }
      else
      {
        ++num2;
        for (int dir = 0; dir < 4; ++dir)
        {
          if (compactSpan1.GetConnection(dir) != 63 /*0x3F*/)
          {
            int num10 = num4 + this.voxelArea.DirectionX[dir];
            int num11 = num3 + this.voxelArea.DirectionZ[dir];
            int index4 = (int) this.voxelArea.compactCells[num10 + num11].index + compactSpan1.GetConnection(dir);
            if (this.voxelArea.areaTypes[index4] == areaType && (int) this.voxelArea.dist[index4] >= num1 && srcReg[index4] == (ushort) 0)
            {
              srcReg[index4] = r;
              srcDist[index4] = (ushort) 0;
              stack.Add(num10);
              stack.Add(num11);
              stack.Add(index4);
            }
          }
        }
      }
    }
    return num2 > 0;
  }

  public void MarkRectWithRegion(
    int minx,
    int maxx,
    int minz,
    int maxz,
    ushort region,
    ushort[] srcReg)
  {
    int num = maxz * this.voxelArea.width;
    for (int index1 = minz * this.voxelArea.width; index1 < num; index1 += this.voxelArea.width)
    {
      for (int index2 = minx; index2 < maxx; ++index2)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[index1 + index2];
        int index3 = (int) compactCell.index;
        for (int index4 = (int) compactCell.index + (int) compactCell.count; index3 < index4; ++index3)
        {
          if (this.voxelArea.areaTypes[index3] != 0)
            srcReg[index3] = region;
        }
      }
    }
  }

  public ushort CalculateDistanceField(ushort[] src)
  {
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    for (int index1 = 0; index1 < num1; index1 += this.voxelArea.width)
    {
      for (int index2 = 0; index2 < this.voxelArea.width; ++index2)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[index2 + index1];
        int index3 = (int) compactCell.index;
        for (int index4 = (int) compactCell.index + (int) compactCell.count; index3 < index4; ++index3)
        {
          CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[index3];
          int num2 = 0;
          for (int dir = 0; dir < 4 && compactSpan.GetConnection(dir) != 63 /*0x3F*/; ++dir)
            ++num2;
          if (num2 != 4)
            src[index3] = (ushort) 0;
        }
      }
    }
    for (int index5 = 0; index5 < num1; index5 += this.voxelArea.width)
    {
      for (int index6 = 0; index6 < this.voxelArea.width; ++index6)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[index6 + index5];
        int index7 = (int) compactCell.index;
        for (int index8 = (int) compactCell.index + (int) compactCell.count; index7 < index8; ++index7)
        {
          CompactVoxelSpan compactSpan1 = this.voxelArea.compactSpans[index7];
          if (compactSpan1.GetConnection(0) != 63 /*0x3F*/)
          {
            int num3 = index6 + this.voxelArea.DirectionX[0];
            int num4 = index5 + this.voxelArea.DirectionZ[0];
            int index9 = (int) ((long) this.voxelArea.compactCells[num3 + num4].index + (long) compactSpan1.GetConnection(0));
            if ((int) src[index9] + 2 < (int) src[index7])
              src[index7] = (ushort) ((uint) src[index9] + 2U);
            CompactVoxelSpan compactSpan2 = this.voxelArea.compactSpans[index9];
            if (compactSpan2.GetConnection(3) != 63 /*0x3F*/)
            {
              int index10 = (int) ((long) this.voxelArea.compactCells[num3 + this.voxelArea.DirectionX[3] + (num4 + this.voxelArea.DirectionZ[3])].index + (long) compactSpan2.GetConnection(3));
              if ((int) src[index10] + 3 < (int) src[index7])
                src[index7] = (ushort) ((uint) src[index10] + 3U);
            }
          }
          if (compactSpan1.GetConnection(3) != 63 /*0x3F*/)
          {
            int num5 = index6 + this.voxelArea.DirectionX[3];
            int num6 = index5 + this.voxelArea.DirectionZ[3];
            int index11 = (int) ((long) this.voxelArea.compactCells[num5 + num6].index + (long) compactSpan1.GetConnection(3));
            if ((int) src[index11] + 2 < (int) src[index7])
              src[index7] = (ushort) ((uint) src[index11] + 2U);
            CompactVoxelSpan compactSpan3 = this.voxelArea.compactSpans[index11];
            if (compactSpan3.GetConnection(2) != 63 /*0x3F*/)
            {
              int index12 = (int) ((long) this.voxelArea.compactCells[num5 + this.voxelArea.DirectionX[2] + (num6 + this.voxelArea.DirectionZ[2])].index + (long) compactSpan3.GetConnection(2));
              if ((int) src[index12] + 3 < (int) src[index7])
                src[index7] = (ushort) ((uint) src[index12] + 3U);
            }
          }
        }
      }
    }
    for (int index13 = num1 - this.voxelArea.width; index13 >= 0; index13 -= this.voxelArea.width)
    {
      for (int index14 = this.voxelArea.width - 1; index14 >= 0; --index14)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[index14 + index13];
        int index15 = (int) compactCell.index;
        for (int index16 = (int) compactCell.index + (int) compactCell.count; index15 < index16; ++index15)
        {
          CompactVoxelSpan compactSpan4 = this.voxelArea.compactSpans[index15];
          if (compactSpan4.GetConnection(2) != 63 /*0x3F*/)
          {
            int num7 = index14 + this.voxelArea.DirectionX[2];
            int num8 = index13 + this.voxelArea.DirectionZ[2];
            int index17 = (int) ((long) this.voxelArea.compactCells[num7 + num8].index + (long) compactSpan4.GetConnection(2));
            if ((int) src[index17] + 2 < (int) src[index15])
              src[index15] = (ushort) ((uint) src[index17] + 2U);
            CompactVoxelSpan compactSpan5 = this.voxelArea.compactSpans[index17];
            if (compactSpan5.GetConnection(1) != 63 /*0x3F*/)
            {
              int index18 = (int) ((long) this.voxelArea.compactCells[num7 + this.voxelArea.DirectionX[1] + (num8 + this.voxelArea.DirectionZ[1])].index + (long) compactSpan5.GetConnection(1));
              if ((int) src[index18] + 3 < (int) src[index15])
                src[index15] = (ushort) ((uint) src[index18] + 3U);
            }
          }
          if (compactSpan4.GetConnection(1) != 63 /*0x3F*/)
          {
            int num9 = index14 + this.voxelArea.DirectionX[1];
            int num10 = index13 + this.voxelArea.DirectionZ[1];
            int index19 = (int) ((long) this.voxelArea.compactCells[num9 + num10].index + (long) compactSpan4.GetConnection(1));
            if ((int) src[index19] + 2 < (int) src[index15])
              src[index15] = (ushort) ((uint) src[index19] + 2U);
            CompactVoxelSpan compactSpan6 = this.voxelArea.compactSpans[index19];
            if (compactSpan6.GetConnection(0) != 63 /*0x3F*/)
            {
              int index20 = (int) ((long) this.voxelArea.compactCells[num9 + this.voxelArea.DirectionX[0] + (num10 + this.voxelArea.DirectionZ[0])].index + (long) compactSpan6.GetConnection(0));
              if ((int) src[index20] + 3 < (int) src[index15])
                src[index15] = (ushort) ((uint) src[index20] + 3U);
            }
          }
        }
      }
    }
    ushort val2 = 0;
    for (int index = 0; index < this.voxelArea.compactSpanCount; ++index)
      val2 = Math.Max(src[index], val2);
    return val2;
  }

  public ushort[] BoxBlur(ushort[] src, ushort[] dst)
  {
    ushort num1 = 20;
    for (int index1 = this.voxelArea.width * this.voxelArea.depth - this.voxelArea.width; index1 >= 0; index1 -= this.voxelArea.width)
    {
      for (int index2 = this.voxelArea.width - 1; index2 >= 0; --index2)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[index2 + index1];
        int index3 = (int) compactCell.index;
        for (int index4 = (int) compactCell.index + (int) compactCell.count; index3 < index4; ++index3)
        {
          CompactVoxelSpan compactSpan1 = this.voxelArea.compactSpans[index3];
          ushort num2 = src[index3];
          if ((int) num2 < (int) num1)
          {
            dst[index3] = num2;
          }
          else
          {
            int num3 = (int) num2;
            for (int dir1 = 0; dir1 < 4; ++dir1)
            {
              if (compactSpan1.GetConnection(dir1) != 63 /*0x3F*/)
              {
                int num4 = index2 + this.voxelArea.DirectionX[dir1];
                int num5 = index1 + this.voxelArea.DirectionZ[dir1];
                int index5 = (int) ((long) this.voxelArea.compactCells[num4 + num5].index + (long) compactSpan1.GetConnection(dir1));
                int num6 = num3 + (int) src[index5];
                CompactVoxelSpan compactSpan2 = this.voxelArea.compactSpans[index5];
                int dir2 = dir1 + 1 & 3;
                if (compactSpan2.GetConnection(dir2) != 63 /*0x3F*/)
                {
                  int index6 = (int) ((long) this.voxelArea.compactCells[num4 + this.voxelArea.DirectionX[dir2] + (num5 + this.voxelArea.DirectionZ[dir2])].index + (long) compactSpan2.GetConnection(dir2));
                  num3 = num6 + (int) src[index6];
                }
                else
                  num3 = num6 + (int) num2;
              }
              else
                num3 += (int) num2 * 2;
            }
            dst[index3] = (ushort) ((double) (num3 + 5) / 9.0);
          }
        }
      }
    }
    return dst;
  }

  public void FloodOnes(List<Int3> st1, ushort[] regs, uint level, ushort reg)
  {
    for (int index = 0; index < st1.Count; ++index)
    {
      int x = st1[index].x;
      int y = st1[index].y;
      int z = st1[index].z;
      regs[y] = reg;
      CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[y];
      int areaType = this.voxelArea.areaTypes[y];
      for (int dir = 0; dir < 4; ++dir)
      {
        if (compactSpan.GetConnection(dir) != 63 /*0x3F*/)
        {
          int _x = x + this.voxelArea.DirectionX[dir];
          int _z = z + this.voxelArea.DirectionZ[dir];
          int _y = (int) this.voxelArea.compactCells[_x + _z].index + compactSpan.GetConnection(dir);
          if (areaType == this.voxelArea.areaTypes[_y] && regs[_y] == (ushort) 1)
          {
            regs[_y] = reg;
            st1.Add(new Int3(_x, _y, _z));
          }
        }
      }
    }
  }

  public void BuildRegions()
  {
    int width = this.voxelArea.width;
    int depth = this.voxelArea.depth;
    int num1 = width * depth;
    int compactSpanCount = this.voxelArea.compactSpanCount;
    int maxIterations = 8;
    List<int> intList = ListPool<int>.Claim(1024 /*0x0400*/);
    ushort[] numArray1 = new ushort[compactSpanCount];
    ushort[] srcDist = new ushort[compactSpanCount];
    ushort[] dstReg = new ushort[compactSpanCount];
    ushort[] dstDist = new ushort[compactSpanCount];
    ushort num2 = 2;
    this.MarkRectWithRegion(0, this.borderSize, 0, depth, (ushort) ((uint) num2 | 32768U /*0x8000*/), numArray1);
    ushort num3 = (ushort) ((uint) num2 + 1U);
    this.MarkRectWithRegion(width - this.borderSize, width, 0, depth, (ushort) ((uint) num3 | 32768U /*0x8000*/), numArray1);
    ushort num4 = (ushort) ((uint) num3 + 1U);
    this.MarkRectWithRegion(0, width, 0, this.borderSize, (ushort) ((uint) num4 | 32768U /*0x8000*/), numArray1);
    ushort num5 = (ushort) ((uint) num4 + 1U);
    this.MarkRectWithRegion(0, width, depth - this.borderSize, depth, (ushort) ((uint) num5 | 32768U /*0x8000*/), numArray1);
    ushort r = (ushort) ((uint) num5 + 1U);
    uint level = (uint) ((int) this.voxelArea.maxDistance + 1 & -2);
    int num6 = 0;
    while (level > 0U)
    {
      level = level >= 2U ? level - 2U : 0U;
      if (this.ExpandRegions(maxIterations, level, numArray1, srcDist, dstReg, dstDist, intList) != numArray1)
      {
        ushort[] numArray2 = numArray1;
        numArray1 = dstReg;
        dstReg = numArray2;
        ushort[] numArray3 = srcDist;
        srcDist = dstDist;
        dstDist = numArray3;
      }
      int z = 0;
      int num7 = 0;
      while (z < num1)
      {
        for (int x = 0; x < this.voxelArea.width; ++x)
        {
          CompactVoxelCell compactCell = this.voxelArea.compactCells[z + x];
          int index1 = (int) compactCell.index;
          for (int index2 = (int) compactCell.index + (int) compactCell.count; index1 < index2; ++index1)
          {
            if ((uint) this.voxelArea.dist[index1] >= level && numArray1[index1] == (ushort) 0 && this.voxelArea.areaTypes[index1] != 0 && this.FloodRegion(x, z, index1, level, r, numArray1, srcDist, intList))
              ++r;
          }
        }
        z += width;
        ++num7;
      }
      ++num6;
    }
    if (this.ExpandRegions(maxIterations * 8, 0U, numArray1, srcDist, dstReg, dstDist, intList) != numArray1)
      numArray1 = dstReg;
    this.voxelArea.maxRegions = (int) r;
    this.FilterSmallRegions(numArray1, this.minRegionSize, this.voxelArea.maxRegions);
    for (int index = 0; index < this.voxelArea.compactSpanCount; ++index)
      this.voxelArea.compactSpans[index].reg = (int) numArray1[index];
    ListPool<int>.Release(intList);
  }

  public static int union_find_find(int[] arr, int x)
  {
    return arr[x] < 0 ? x : (arr[x] = Voxelize.union_find_find(arr, arr[x]));
  }

  public static void union_find_union(int[] arr, int a, int b)
  {
    a = Voxelize.union_find_find(arr, a);
    b = Voxelize.union_find_find(arr, b);
    if (a == b)
      return;
    if (arr[a] > arr[b])
    {
      int num = a;
      a = b;
      b = num;
    }
    arr[a] += arr[b];
    arr[b] = a;
  }

  public void FilterSmallRegions(ushort[] reg, int minRegionSize, int maxRegions)
  {
    RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.Root;
    bool flag = relevantGraphSurface != null && this.relevantGraphSurfaceMode != 0;
    if (!flag && minRegionSize <= 0)
      return;
    int[] numArray = new int[maxRegions];
    ushort[] array = this.voxelArea.tmpUShortArr;
    if (array == null || array.Length < maxRegions)
      array = this.voxelArea.tmpUShortArr = new ushort[maxRegions];
    Memory.MemSet<int>(numArray, -1, 4);
    Memory.MemSet<ushort>(array, (ushort) 0, maxRegions, 2);
    int length = numArray.Length;
    int num1 = this.voxelArea.width * this.voxelArea.depth;
    int num2 = 2 | (this.relevantGraphSurfaceMode == RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile ? 1 : 0);
    if (flag)
    {
      while (relevantGraphSurface != null)
      {
        int x;
        int z;
        this.VectorToIndex(relevantGraphSurface.Position, out x, out z);
        if (x < 0 || z < 0 || x >= this.voxelArea.width || z >= this.voxelArea.depth)
        {
          relevantGraphSurface = relevantGraphSurface.Next;
        }
        else
        {
          int num3 = (int) (((double) relevantGraphSurface.Position.y - (double) this.voxelOffset.y) / (double) this.cellHeight);
          int num4 = (int) ((double) relevantGraphSurface.maxRange / (double) this.cellHeight);
          CompactVoxelCell compactCell = this.voxelArea.compactCells[x + z * this.voxelArea.width];
          for (int index = (int) compactCell.index; (long) index < (long) (compactCell.index + compactCell.count); ++index)
          {
            if (Math.Abs((int) this.voxelArea.compactSpans[index].y - num3) <= num4 && reg[index] != (ushort) 0)
              array[Voxelize.union_find_find(numArray, (int) reg[index] & -32769)] |= (ushort) 2;
          }
          relevantGraphSurface = relevantGraphSurface.Next;
        }
      }
    }
    int num5 = 0;
    int num6 = 0;
    while (num5 < num1)
    {
      for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
      {
        CompactVoxelCell compactCell = this.voxelArea.compactCells[index1 + num5];
        for (int index2 = (int) compactCell.index; (long) index2 < (long) (compactCell.index + compactCell.count); ++index2)
        {
          CompactVoxelSpan compactSpan = this.voxelArea.compactSpans[index2];
          int x = (int) reg[index2];
          if ((x & -32769) != 0)
          {
            if (x >= length)
            {
              array[Voxelize.union_find_find(numArray, x & -32769)] |= (ushort) 1;
            }
            else
            {
              int find = Voxelize.union_find_find(numArray, x);
              --numArray[find];
              for (int dir = 0; dir < 4; ++dir)
              {
                if (compactSpan.GetConnection(dir) != 63 /*0x3F*/)
                {
                  int index3 = (int) this.voxelArea.compactCells[index1 + this.voxelArea.DirectionX[dir] + (num5 + this.voxelArea.DirectionZ[dir])].index + compactSpan.GetConnection(dir);
                  int b = (int) reg[index3];
                  if (x != b && (b & -32769) != 0)
                  {
                    if ((b & 32768 /*0x8000*/) != 0)
                      array[find] |= (ushort) 1;
                    else
                      Voxelize.union_find_union(numArray, find, b);
                  }
                }
              }
            }
          }
        }
      }
      num5 += this.voxelArea.width;
      ++num6;
    }
    for (int x = 0; x < numArray.Length; ++x)
      array[Voxelize.union_find_find(numArray, x)] |= array[x];
    for (int x = 0; x < numArray.Length; ++x)
    {
      int find = Voxelize.union_find_find(numArray, x);
      if (((int) array[find] & 1) != 0)
        numArray[find] = -minRegionSize - 2;
      if (flag && ((int) array[find] & num2) == 0)
        numArray[find] = -1;
    }
    for (int index = 0; index < this.voxelArea.compactSpanCount; ++index)
    {
      int x = (int) reg[index];
      if (x < length && numArray[Voxelize.union_find_find(numArray, x)] >= -minRegionSize - 1)
        reg[index] = (ushort) 0;
    }
  }
}
