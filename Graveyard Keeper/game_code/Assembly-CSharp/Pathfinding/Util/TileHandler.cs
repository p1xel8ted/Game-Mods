// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.TileHandler
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.ClipperLib;
using Pathfinding.Poly2Tri;
using Pathfinding.Voxels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Pathfinding.Util;

public class TileHandler
{
  public RecastGraph _graph;
  public Clipper clipper;
  public int[] cached_int_array = new int[32 /*0x20*/];
  public Dictionary<Int3, int> cached_Int3_int_dict = new Dictionary<Int3, int>();
  public Dictionary<Int2, int> cached_Int2_int_dict = new Dictionary<Int2, int>();
  public TileHandler.TileType[] activeTileTypes;
  public int[] activeTileRotations;
  public int[] activeTileOffsets;
  public bool[] reloadedInBatch;
  public bool isBatching;
  public const int CUT_ALL = 0;
  public const int CUT_DUAL = 1;
  public const int CUT_BREAK = 2;

  public RecastGraph graph => this._graph;

  public TileHandler(RecastGraph graph)
  {
    if (graph == null)
      throw new ArgumentNullException(nameof (graph));
    if (graph.GetTiles() == null)
      throw new ArgumentException("graph has no tiles. Please scan the graph before creating a TileHandler");
    this.activeTileTypes = new TileHandler.TileType[graph.tileXCount * graph.tileZCount];
    this.activeTileRotations = new int[this.activeTileTypes.Length];
    this.activeTileOffsets = new int[this.activeTileTypes.Length];
    this.reloadedInBatch = new bool[this.activeTileTypes.Length];
    this._graph = graph;
  }

  public int GetActiveRotation(Int2 p)
  {
    return this.activeTileRotations[p.x + p.y * this._graph.tileXCount];
  }

  [Obsolete("Use the result from RegisterTileType instead")]
  public TileHandler.TileType GetTileType(int index)
  {
    throw new Exception("This method has been deprecated. Use the result from RegisterTileType instead.");
  }

  [Obsolete("Use the result from RegisterTileType instead")]
  public int GetTileTypeCount()
  {
    throw new Exception("This method has been deprecated. Use the result from RegisterTileType instead.");
  }

  public TileHandler.TileType RegisterTileType(
    Mesh source,
    Int3 centerOffset,
    int width = 1,
    int depth = 1)
  {
    return new TileHandler.TileType(source, new Int3(this.graph.tileSizeX, 1, this.graph.tileSizeZ) * (1000f * this.graph.cellSize), centerOffset, width, depth);
  }

  public void CreateTileTypesFromGraph()
  {
    RecastGraph.NavmeshTile[] tiles = this.graph.GetTiles();
    if (tiles == null || tiles.Length != this.graph.tileXCount * this.graph.tileZCount)
      throw new InvalidOperationException("Graph tiles are invalid (either null or number of tiles is not equal to width*depth of the graph");
    for (int index1 = 0; index1 < this.graph.tileZCount; ++index1)
    {
      for (int index2 = 0; index2 < this.graph.tileXCount; ++index2)
        this.UpdateTileType(tiles[index2 + index1 * this.graph.tileXCount]);
    }
  }

  public void UpdateTileType(RecastGraph.NavmeshTile tile)
  {
    int x = tile.x;
    int z = tile.z;
    Int3 min = (Int3) this.graph.GetTileBounds(x, z).min;
    Int3 tileSize = new Int3(this.graph.tileSizeX, 1, this.graph.tileSizeZ) * (1000f * this.graph.cellSize);
    Int3 centerOffset = -(min + new Int3(tileSize.x * tile.w / 2, 0, tileSize.z * tile.d / 2));
    TileHandler.TileType tileType = new TileHandler.TileType(tile.verts, tile.tris, tileSize, centerOffset, tile.w, tile.d);
    int index = x + z * this.graph.tileXCount;
    this.activeTileTypes[index] = tileType;
    this.activeTileRotations[index] = 0;
    this.activeTileOffsets[index] = 0;
  }

  public bool StartBatchLoad()
  {
    if (this.isBatching)
      return false;
    this.isBatching = true;
    AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
    {
      this.graph.StartBatchTileUpdate();
      return true;
    })));
    return true;
  }

  public void EndBatchLoad()
  {
    if (!this.isBatching)
      throw new Exception("Ending batching when batching has not been started");
    for (int index = 0; index < this.reloadedInBatch.Length; ++index)
      this.reloadedInBatch[index] = false;
    this.isBatching = false;
    AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
    {
      this.graph.EndBatchTileUpdate();
      return true;
    })));
  }

  public void CutPoly(
    Int3[] verts,
    int[] tris,
    ref Int3[] outVertsArr,
    ref int[] outTrisArr,
    out int outVCount,
    out int outTCount,
    Int3[] extraShape,
    Int3 cuttingOffset,
    Bounds realBounds,
    TileHandler.CutMode mode = TileHandler.CutMode.CutAll | TileHandler.CutMode.CutDual,
    int perturbate = 0)
  {
    if (verts.Length == 0 || tris.Length == 0)
    {
      outVCount = 0;
      outTCount = 0;
      outTrisArr = new int[0];
      outVertsArr = new Int3[0];
    }
    else
    {
      List<IntPoint> pg1 = (List<IntPoint>) null;
      if (extraShape == null && (mode & TileHandler.CutMode.CutExtra) != (TileHandler.CutMode) 0)
        throw new Exception("extraShape is null and the CutMode specifies that it should be used. Cannot use null shape.");
      if ((mode & TileHandler.CutMode.CutExtra) != (TileHandler.CutMode) 0)
      {
        pg1 = new List<IntPoint>(extraShape.Length);
        for (int index = 0; index < extraShape.Length; ++index)
          pg1.Add(new IntPoint((long) (extraShape[index].x + cuttingOffset.x), (long) (extraShape[index].z + cuttingOffset.z)));
      }
      List<IntPoint> pg2 = new List<IntPoint>(5);
      Dictionary<TriangulationPoint, int> dictionary = new Dictionary<TriangulationPoint, int>();
      List<PolygonPoint> polygonPointList = new List<PolygonPoint>();
      Pathfinding.IntRect b = new Pathfinding.IntRect(verts[0].x, verts[0].z, verts[0].x, verts[0].z);
      for (int index = 0; index < verts.Length; ++index)
        b = b.ExpandToContain(verts[index].x, verts[index].z);
      List<Int3> list1 = ListPool<Int3>.Claim(verts.Length * 2);
      List<int> list2 = ListPool<int>.Claim(tris.Length);
      PolyTree polytree = new PolyTree();
      List<List<IntPoint>> solution = new List<List<IntPoint>>();
      Stack<Pathfinding.Poly2Tri.Polygon> polygonStack = new Stack<Pathfinding.Poly2Tri.Polygon>();
      this.clipper = this.clipper ?? new Clipper();
      this.clipper.ReverseSolution = true;
      this.clipper.StrictlySimple = true;
      List<NavmeshCut> list3 = mode != TileHandler.CutMode.CutExtra ? NavmeshCut.GetAllInRange(realBounds) : ListPool<NavmeshCut>.Claim();
      List<int> list4 = ListPool<int>.Claim();
      List<Pathfinding.IntRect> list5 = ListPool<Pathfinding.IntRect>.Claim();
      List<Int2> list6 = ListPool<Int2>.Claim();
      List<List<IntPoint>> buffer = new List<List<IntPoint>>();
      List<bool> list7 = ListPool<bool>.Claim();
      List<bool> list8 = ListPool<bool>.Claim();
      if (perturbate > 10)
      {
        Debug.LogError((object) ("Too many perturbations aborting : " + mode.ToString()));
        Debug.Break();
        outVCount = verts.Length;
        outTCount = tris.Length;
        outTrisArr = tris;
        outVertsArr = verts;
      }
      else
      {
        System.Random random = (System.Random) null;
        if (perturbate > 0)
          random = new System.Random();
        for (int index1 = 0; index1 < list3.Count; ++index1)
        {
          Bounds bounds = list3[index1].GetBounds();
          Int3 int3_1 = (Int3) bounds.min + cuttingOffset;
          Int3 int3_2 = (Int3) bounds.max + cuttingOffset;
          if (Pathfinding.IntRect.Intersects(new Pathfinding.IntRect(int3_1.x, int3_1.z, int3_2.x, int3_2.z), b))
          {
            Int2 int2 = new Int2(0, 0);
            if (perturbate > 0)
            {
              int2.x = random.Next() % 6 * perturbate - 3 * perturbate;
              if (int2.x >= 0)
                ++int2.x;
              int2.y = random.Next() % 6 * perturbate - 3 * perturbate;
              if (int2.y >= 0)
                ++int2.y;
            }
            int count = buffer.Count;
            list3[index1].GetContour(buffer);
            for (int index2 = count; index2 < buffer.Count; ++index2)
            {
              List<IntPoint> intPointList = buffer[index2];
              if (intPointList.Count == 0)
              {
                Debug.LogError((object) "Zero Length Contour");
                list5.Add(new Pathfinding.IntRect());
                list6.Add(new Int2(0, 0));
              }
              else
              {
                Pathfinding.IntRect intRect = new Pathfinding.IntRect((int) intPointList[0].X + cuttingOffset.x, (int) intPointList[0].Y + cuttingOffset.y, (int) intPointList[0].X + cuttingOffset.x, (int) intPointList[0].Y + cuttingOffset.y);
                for (int index3 = 0; index3 < intPointList.Count; ++index3)
                {
                  IntPoint intPoint = intPointList[index3];
                  intPoint.X += (long) cuttingOffset.x;
                  intPoint.Y += (long) cuttingOffset.z;
                  if (perturbate > 0)
                  {
                    intPoint.X += (long) int2.x;
                    intPoint.Y += (long) int2.y;
                  }
                  intPointList[index3] = intPoint;
                  intRect = intRect.ExpandToContain((int) intPoint.X, (int) intPoint.Y);
                }
                list6.Add(new Int2(int3_1.y, int3_2.y));
                list5.Add(intRect);
                list7.Add(list3[index1].isDual);
                list8.Add(list3[index1].cutsAddedGeom);
              }
            }
          }
        }
        List<NavmeshAdd> allInRange = NavmeshAdd.GetAllInRange(realBounds);
        Int3[] vbuffer = verts;
        int[] tbuffer = tris;
        int index4 = -1;
        int index5 = -3;
        Int3[] int3Array1 = (Int3[]) null;
        Int3[] int3Array2 = (Int3[]) null;
        Int3 int3_3 = Int3.zero;
        if (allInRange.Count > 0)
        {
          int3Array1 = new Int3[7];
          int3Array2 = new Int3[7];
          int3_3 = (Int3) realBounds.extents;
        }
label_37:
        Int3 int3_4;
        Int3 int3_5;
        Int3 int3_6;
        bool flag;
        int num1;
        do
        {
          int n1;
          do
          {
            int n2;
            do
            {
              int n3;
              do
              {
                for (index5 += 3; index5 >= tbuffer.Length; allInRange[index4].GetMesh(cuttingOffset, ref vbuffer, out tbuffer))
                {
                  ++index4;
                  index5 = 0;
                  if (index4 >= allInRange.Count)
                  {
                    vbuffer = (Int3[]) null;
                    break;
                  }
                  if (vbuffer == verts)
                    vbuffer = (Int3[]) null;
                }
                if (vbuffer != null)
                {
                  int3_4 = vbuffer[tbuffer[index5]];
                  int3_5 = vbuffer[tbuffer[index5 + 1]];
                  int3_6 = vbuffer[tbuffer[index5 + 2]];
                  Pathfinding.IntRect a = new Pathfinding.IntRect(int3_4.x, int3_4.z, int3_4.x, int3_4.z);
                  a = a.ExpandToContain(int3_5.x, int3_5.z);
                  a = a.ExpandToContain(int3_6.x, int3_6.z);
                  int num2 = Math.Min(int3_4.y, Math.Min(int3_5.y, int3_6.y));
                  int num3 = Math.Max(int3_4.y, Math.Max(int3_5.y, int3_6.y));
                  list4.Clear();
                  flag = false;
                  for (int index6 = 0; index6 < buffer.Count; ++index6)
                  {
                    int x = list6[index6].x;
                    int y = list6[index6].y;
                    if (Pathfinding.IntRect.Intersects(a, list5[index6]) && y >= num2 && x <= num3 && (list8[index6] || index4 == -1))
                    {
                      int3_4.y = x;
                      int3_4.y = y;
                      list4.Add(index6);
                      flag |= list7[index6];
                    }
                  }
                  if (list4.Count == 0 && (mode & TileHandler.CutMode.CutExtra) == (TileHandler.CutMode) 0 && (mode & TileHandler.CutMode.CutAll) != (TileHandler.CutMode) 0 && index4 == -1)
                  {
                    list2.Add(list1.Count);
                    list2.Add(list1.Count + 1);
                    list2.Add(list1.Count + 2);
                    list1.Add(int3_4);
                    list1.Add(int3_5);
                    list1.Add(int3_6);
                  }
                  else
                  {
                    pg2.Clear();
                    if (index4 == -1)
                    {
                      pg2.Add(new IntPoint((long) int3_4.x, (long) int3_4.z));
                      pg2.Add(new IntPoint((long) int3_5.x, (long) int3_5.z));
                      pg2.Add(new IntPoint((long) int3_6.x, (long) int3_6.z));
                      goto label_61;
                    }
                    int3Array1[0] = int3_4;
                    int3Array1[1] = int3_5;
                    int3Array1[2] = int3_6;
                    n3 = Utility.ClipPolygon(int3Array1, 3, int3Array2, 1, 0, 0);
                  }
                }
                else
                  goto label_127;
              }
              while (n3 == 0);
              n2 = Utility.ClipPolygon(int3Array2, n3, int3Array1, -1, 2 * int3_3.x, 0);
            }
            while (n2 == 0);
            n1 = Utility.ClipPolygon(int3Array1, n2, int3Array2, 1, 0, 2);
          }
          while (n1 == 0);
          num1 = Utility.ClipPolygon(int3Array2, n1, int3Array1, -1, 2 * int3_3.z, 2);
        }
        while (num1 == 0);
        for (int index7 = 0; index7 < num1; ++index7)
          pg2.Add(new IntPoint((long) int3Array1[index7].x, (long) int3Array1[index7].z));
label_61:
        dictionary.Clear();
        Int3 int3_7 = int3_5 - int3_4;
        Int3 int3_8 = int3_6 - int3_4;
        Int3 int3_9 = int3_7;
        Int3 int3_10 = int3_8;
        int3_9.y = 0;
        int3_10.y = 0;
        for (int index8 = 0; index8 < 16 /*0x10*/; ++index8)
        {
          if (((int) mode >> index8 & 1) != 0)
          {
            if (1 << index8 == 1)
            {
              this.clipper.Clear();
              this.clipper.AddPolygon(pg2, PolyType.ptSubject);
              for (int index9 = 0; index9 < list4.Count; ++index9)
                this.clipper.AddPolygon(buffer[list4[index9]], PolyType.ptClip);
              polytree.Clear();
              this.clipper.Execute(ClipType.ctDifference, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
            }
            else if (1 << index8 == 2)
            {
              if (flag)
              {
                this.clipper.Clear();
                this.clipper.AddPolygon(pg2, PolyType.ptSubject);
                for (int index10 = 0; index10 < list4.Count; ++index10)
                {
                  if (list7[list4[index10]])
                    this.clipper.AddPolygon(buffer[list4[index10]], PolyType.ptClip);
                }
                solution.Clear();
                this.clipper.Execute(ClipType.ctIntersection, solution, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
                this.clipper.Clear();
                for (int index11 = 0; index11 < solution.Count; ++index11)
                  this.clipper.AddPolygon(solution[index11], Clipper.Orientation(solution[index11]) ? PolyType.ptClip : PolyType.ptSubject);
                for (int index12 = 0; index12 < list4.Count; ++index12)
                {
                  if (!list7[list4[index12]])
                    this.clipper.AddPolygon(buffer[list4[index12]], PolyType.ptClip);
                }
                polytree.Clear();
                this.clipper.Execute(ClipType.ctDifference, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
              }
              else
                continue;
            }
            else if (1 << index8 == 4)
            {
              this.clipper.Clear();
              this.clipper.AddPolygon(pg2, PolyType.ptSubject);
              this.clipper.AddPolygon(pg1, PolyType.ptClip);
              polytree.Clear();
              this.clipper.Execute(ClipType.ctIntersection, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
            }
            for (int index13 = 0; index13 < polytree.ChildCount; ++index13)
            {
              PolyNode child = polytree.Childs[index13];
              List<IntPoint> contour = child.Contour;
              List<PolyNode> childs = child.Childs;
              if (childs.Count == 0 && contour.Count == 3 && index4 == -1)
              {
                for (int index14 = 0; index14 < contour.Count; ++index14)
                {
                  Int3 int3_11 = new Int3((int) contour[index14].X, 0, (int) contour[index14].Y);
                  double num4 = (double) (int3_5.z - int3_6.z) * (double) (int3_4.x - int3_6.x) + (double) (int3_6.x - int3_5.x) * (double) (int3_4.z - int3_6.z);
                  if (num4 == 0.0)
                  {
                    Debug.LogWarning((object) "Degenerate triangle");
                  }
                  else
                  {
                    double num5 = ((double) (int3_5.z - int3_6.z) * (double) (int3_11.x - int3_6.x) + (double) (int3_6.x - int3_5.x) * (double) (int3_11.z - int3_6.z)) / num4;
                    double num6 = ((double) (int3_6.z - int3_4.z) * (double) (int3_11.x - int3_6.x) + (double) (int3_4.x - int3_6.x) * (double) (int3_11.z - int3_6.z)) / num4;
                    int3_11.y = (int) Math.Round(num5 * (double) int3_4.y + num6 * (double) int3_5.y + (1.0 - num5 - num6) * (double) int3_6.y);
                    list2.Add(list1.Count);
                    list1.Add(int3_11);
                  }
                }
              }
              else
              {
                Pathfinding.Poly2Tri.Polygon p = (Pathfinding.Poly2Tri.Polygon) null;
                int index15 = -1;
                for (List<IntPoint> intPointList = contour; intPointList != null; intPointList = index15 < childs.Count ? childs[index15].Contour : (List<IntPoint>) null)
                {
                  polygonPointList.Clear();
                  for (int index16 = 0; index16 < intPointList.Count; ++index16)
                  {
                    PolygonPoint key = new PolygonPoint((double) intPointList[index16].X, (double) intPointList[index16].Y);
                    polygonPointList.Add(key);
                    Int3 int3_12 = new Int3((int) intPointList[index16].X, 0, (int) intPointList[index16].Y);
                    double num7 = (double) (int3_5.z - int3_6.z) * (double) (int3_4.x - int3_6.x) + (double) (int3_6.x - int3_5.x) * (double) (int3_4.z - int3_6.z);
                    if (num7 == 0.0)
                    {
                      Debug.LogWarning((object) "Degenerate triangle");
                    }
                    else
                    {
                      double num8 = ((double) (int3_5.z - int3_6.z) * (double) (int3_12.x - int3_6.x) + (double) (int3_6.x - int3_5.x) * (double) (int3_12.z - int3_6.z)) / num7;
                      double num9 = ((double) (int3_6.z - int3_4.z) * (double) (int3_12.x - int3_6.x) + (double) (int3_4.x - int3_6.x) * (double) (int3_12.z - int3_6.z)) / num7;
                      int3_12.y = (int) Math.Round(num8 * (double) int3_4.y + num9 * (double) int3_5.y + (1.0 - num8 - num9) * (double) int3_6.y);
                      dictionary[(TriangulationPoint) key] = list1.Count;
                      list1.Add(int3_12);
                    }
                  }
                  Pathfinding.Poly2Tri.Polygon poly;
                  if (polygonStack.Count > 0)
                  {
                    poly = polygonStack.Pop();
                    poly.AddPoints((IEnumerable<PolygonPoint>) polygonPointList);
                  }
                  else
                    poly = new Pathfinding.Poly2Tri.Polygon((IList<PolygonPoint>) polygonPointList);
                  if (p == null)
                    p = poly;
                  else
                    p.AddHole(poly);
                  ++index15;
                }
                try
                {
                  P2T.Triangulate(p);
                }
                catch (PointOnEdgeException ex)
                {
                  Debug.LogWarning((object) $"PointOnEdgeException, perturbating vertices slightly ( at {index8.ToString()} in {mode.ToString()})");
                  this.CutPoly(verts, tris, ref outVertsArr, ref outTrisArr, out outVCount, out outTCount, extraShape, cuttingOffset, realBounds, mode, perturbate + 1);
                  return;
                }
                for (int index17 = 0; index17 < p.Triangles.Count; ++index17)
                {
                  DelaunayTriangle triangle = p.Triangles[index17];
                  list2.Add(dictionary[triangle.Points._0]);
                  list2.Add(dictionary[triangle.Points._1]);
                  list2.Add(dictionary[triangle.Points._2]);
                }
                if (p.Holes != null)
                {
                  for (int index18 = 0; index18 < p.Holes.Count; ++index18)
                  {
                    p.Holes[index18].Points.Clear();
                    p.Holes[index18].ClearTriangles();
                    if (p.Holes[index18].Holes != null)
                      p.Holes[index18].Holes.Clear();
                    polygonStack.Push(p.Holes[index18]);
                  }
                }
                p.ClearTriangles();
                if (p.Holes != null)
                  p.Holes.Clear();
                p.Points.Clear();
                polygonStack.Push(p);
              }
            }
          }
        }
        goto label_37;
label_127:
        Dictionary<Int3, int> cachedInt3IntDict = this.cached_Int3_int_dict;
        cachedInt3IntDict.Clear();
        if (this.cached_int_array.Length < list1.Count)
          this.cached_int_array = new int[Math.Max(this.cached_int_array.Length * 2, list1.Count)];
        int[] cachedIntArray = this.cached_int_array;
        int index19 = 0;
        for (int index20 = 0; index20 < list1.Count; ++index20)
        {
          int num10;
          if (!cachedInt3IntDict.TryGetValue(list1[index20], out num10))
          {
            cachedInt3IntDict.Add(list1[index20], index19);
            cachedIntArray[index20] = index19;
            list1[index19] = list1[index20];
            ++index19;
          }
          else
            cachedIntArray[index20] = num10;
        }
        outTCount = list2.Count;
        if (outTrisArr == null || outTrisArr.Length < outTCount)
          outTrisArr = new int[outTCount];
        for (int index21 = 0; index21 < outTCount; ++index21)
          outTrisArr[index21] = cachedIntArray[list2[index21]];
        outVCount = index19;
        if (outVertsArr == null || outVertsArr.Length < outVCount)
          outVertsArr = new Int3[outVCount];
        for (int index22 = 0; index22 < outVCount; ++index22)
          outVertsArr[index22] = list1[index22];
        for (int index23 = 0; index23 < list3.Count; ++index23)
          list3[index23].UsedForCut();
        ListPool<Int3>.Release(list1);
        ListPool<int>.Release(list2);
        ListPool<int>.Release(list4);
        ListPool<Int2>.Release(list6);
        ListPool<bool>.Release(list7);
        ListPool<bool>.Release(list8);
        ListPool<Pathfinding.IntRect>.Release(list5);
        ListPool<NavmeshCut>.Release(list3);
      }
    }
  }

  public void DelaunayRefinement(
    Int3[] verts,
    int[] tris,
    ref int vCount,
    ref int tCount,
    bool delaunay,
    bool colinear,
    Int3 worldOffset)
  {
    if (tCount % 3 != 0)
      throw new Exception("Triangle array length must be a multiple of 3");
    Dictionary<Int2, int> cachedInt2IntDict = this.cached_Int2_int_dict;
    cachedInt2IntDict.Clear();
    for (int index = 0; index < tCount; index += 3)
    {
      if (!VectorMath.IsClockwiseXZ(verts[tris[index]], verts[tris[index + 1]], verts[tris[index + 2]]))
      {
        int tri = tris[index];
        tris[index] = tris[index + 2];
        tris[index + 2] = tri;
      }
      cachedInt2IntDict[new Int2(tris[index], tris[index + 1])] = index + 2;
      cachedInt2IntDict[new Int2(tris[index + 1], tris[index + 2])] = index;
      cachedInt2IntDict[new Int2(tris[index + 2], tris[index])] = index + 1;
    }
    for (int index1 = 0; index1 < tCount; index1 += 3)
    {
      for (int index2 = 0; index2 < 3; ++index2)
      {
        int index3;
        if (cachedInt2IntDict.TryGetValue(new Int2(tris[index1 + (index2 + 1) % 3], tris[index1 + index2 % 3]), out index3))
        {
          Int3 vert1 = verts[tris[index1 + (index2 + 2) % 3]];
          Int3 vert2 = verts[tris[index1 + (index2 + 1) % 3]];
          Int3 vert3 = verts[tris[index1 + (index2 + 3) % 3]];
          Int3 vert4 = verts[tris[index3]];
          vert1.y = 0;
          vert2.y = 0;
          vert3.y = 0;
          vert4.y = 0;
          bool flag = false;
          if (!VectorMath.RightOrColinearXZ(vert1, vert3, vert4) || VectorMath.RightXZ(vert1, vert2, vert4))
          {
            if (colinear)
              flag = true;
            else
              continue;
          }
          if (colinear && (double) VectorMath.SqrDistancePointSegmentApproximate(vert1, vert4, vert2) < 9.0 && !cachedInt2IntDict.ContainsKey(new Int2(tris[index1 + (index2 + 2) % 3], tris[index1 + (index2 + 1) % 3])) && !cachedInt2IntDict.ContainsKey(new Int2(tris[index1 + (index2 + 1) % 3], tris[index3])))
          {
            tCount -= 3;
            int index4 = index3 / 3 * 3;
            tris[index1 + (index2 + 1) % 3] = tris[index3];
            if (index4 != tCount)
            {
              tris[index4] = tris[tCount];
              tris[index4 + 1] = tris[tCount + 1];
              tris[index4 + 2] = tris[tCount + 2];
              cachedInt2IntDict[new Int2(tris[index4], tris[index4 + 1])] = index4 + 2;
              cachedInt2IntDict[new Int2(tris[index4 + 1], tris[index4 + 2])] = index4;
              cachedInt2IntDict[new Int2(tris[index4 + 2], tris[index4])] = index4 + 1;
              tris[tCount] = 0;
              tris[tCount + 1] = 0;
              tris[tCount + 2] = 0;
            }
            else
              tCount += 3;
            cachedInt2IntDict[new Int2(tris[index1], tris[index1 + 1])] = index1 + 2;
            cachedInt2IntDict[new Int2(tris[index1 + 1], tris[index1 + 2])] = index1;
            cachedInt2IntDict[new Int2(tris[index1 + 2], tris[index1])] = index1 + 1;
          }
          else if (delaunay && !flag)
          {
            float num1 = Int3.Angle(vert2 - vert1, vert3 - vert1);
            if ((double) Int3.Angle(vert2 - vert4, vert3 - vert4) > 6.2831854820251465 - 2.0 * (double) num1)
            {
              tris[index1 + (index2 + 1) % 3] = tris[index3];
              int index5 = index3 / 3 * 3;
              int num2 = index3 - index5;
              tris[index5 + (num2 - 1 + 3) % 3] = tris[index1 + (index2 + 2) % 3];
              cachedInt2IntDict[new Int2(tris[index1], tris[index1 + 1])] = index1 + 2;
              cachedInt2IntDict[new Int2(tris[index1 + 1], tris[index1 + 2])] = index1;
              cachedInt2IntDict[new Int2(tris[index1 + 2], tris[index1])] = index1 + 1;
              cachedInt2IntDict[new Int2(tris[index5], tris[index5 + 1])] = index5 + 2;
              cachedInt2IntDict[new Int2(tris[index5 + 1], tris[index5 + 2])] = index5;
              cachedInt2IntDict[new Int2(tris[index5 + 2], tris[index5])] = index5 + 1;
            }
          }
        }
      }
    }
  }

  public Vector3 Point2D2V3(TriangulationPoint p)
  {
    return new Vector3((float) p.X, 0.0f, (float) p.Y) * (1f / 1000f);
  }

  public Int3 IntPoint2Int3(IntPoint p) => new Int3((int) p.X, 0, (int) p.Y);

  public void ClearTile(int x, int z)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null || x < 0 || z < 0 || x >= this.graph.tileXCount || z >= this.graph.tileZCount)
      return;
    AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
    {
      this.graph.ReplaceTile(x, z, new Int3[0], new int[0], false);
      this.activeTileTypes[x + z * this.graph.tileXCount] = (TileHandler.TileType) null;
      GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
      AstarPath.active.QueueWorkItemFloodFill();
      return true;
    })));
  }

  public void ReloadInBounds(Bounds b)
  {
    Int2 tileCoordinates1 = this.graph.GetTileCoordinates(b.min);
    Int2 tileCoordinates2 = this.graph.GetTileCoordinates(b.max);
    Pathfinding.IntRect intRect = Pathfinding.IntRect.Intersection(new Pathfinding.IntRect(tileCoordinates1.x, tileCoordinates1.y, tileCoordinates2.x, tileCoordinates2.y), new Pathfinding.IntRect(0, 0, this.graph.tileXCount - 1, this.graph.tileZCount - 1));
    if (!intRect.IsValid())
      return;
    for (int ymin = intRect.ymin; ymin <= intRect.ymax; ++ymin)
    {
      for (int xmin = intRect.xmin; xmin <= intRect.xmax; ++xmin)
        this.ReloadTile(xmin, ymin);
    }
  }

  public void ReloadTile(int x, int z)
  {
    if (x < 0 || z < 0 || x >= this.graph.tileXCount || z >= this.graph.tileZCount)
      return;
    int index = x + z * this.graph.tileXCount;
    if (this.activeTileTypes[index] == null)
      return;
    this.LoadTile(this.activeTileTypes[index], x, z, this.activeTileRotations[index], this.activeTileOffsets[index]);
  }

  public void CutShapeWithTile(
    int x,
    int z,
    Int3[] shape,
    ref Int3[] verts,
    ref int[] tris,
    out int vCount,
    out int tCount)
  {
    if (this.isBatching)
      throw new Exception("Cannot cut with shape when batching. Please stop batching first.");
    int index1 = x + z * this.graph.tileXCount;
    if (x < 0 || z < 0 || x >= this.graph.tileXCount || z >= this.graph.tileZCount || this.activeTileTypes[index1] == null)
    {
      verts = new Int3[0];
      tris = new int[0];
      vCount = 0;
      tCount = 0;
    }
    else
    {
      Int3[] verts1;
      int[] tris1;
      this.activeTileTypes[index1].Load(out verts1, out tris1, this.activeTileRotations[index1], this.activeTileOffsets[index1]);
      Bounds tileBounds = this.graph.GetTileBounds(x, z);
      Int3 cuttingOffset = -(Int3) tileBounds.min;
      this.CutPoly(verts1, tris1, ref verts, ref tris, out vCount, out tCount, shape, cuttingOffset, tileBounds, TileHandler.CutMode.CutExtra);
      for (int index2 = 0; index2 < verts.Length; ++index2)
        verts[index2] -= cuttingOffset;
    }
  }

  public static T[] ShrinkArray<T>(T[] arr, int newLength)
  {
    newLength = Math.Min(newLength, arr.Length);
    T[] objArray = new T[newLength];
    if (newLength % 4 == 0)
    {
      for (int index = 0; index < newLength; index += 4)
      {
        objArray[index] = arr[index];
        objArray[index + 1] = arr[index + 1];
        objArray[index + 2] = arr[index + 2];
        objArray[index + 3] = arr[index + 3];
      }
    }
    else if (newLength % 3 == 0)
    {
      for (int index = 0; index < newLength; index += 3)
      {
        objArray[index] = arr[index];
        objArray[index + 1] = arr[index + 1];
        objArray[index + 2] = arr[index + 2];
      }
    }
    else if (newLength % 2 == 0)
    {
      for (int index = 0; index < newLength; index += 2)
      {
        objArray[index] = arr[index];
        objArray[index + 1] = arr[index + 1];
      }
    }
    else
    {
      for (int index = 0; index < newLength; ++index)
        objArray[index] = arr[index];
    }
    return objArray;
  }

  public void LoadTile(TileHandler.TileType tile, int x, int z, int rotation, int yoffset)
  {
    if (tile == null)
      throw new ArgumentNullException(nameof (tile));
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      return;
    int index = x + z * this.graph.tileXCount;
    rotation %= 4;
    if (this.isBatching && this.reloadedInBatch[index] && this.activeTileOffsets[index] == yoffset && this.activeTileRotations[index] == rotation && this.activeTileTypes[index] == tile)
      return;
    this.reloadedInBatch[index] |= this.isBatching;
    this.activeTileOffsets[index] = yoffset;
    this.activeTileRotations[index] = rotation;
    this.activeTileTypes[index] = tile;
    AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
    {
      if (this.activeTileOffsets[index] != yoffset || this.activeTileRotations[index] != rotation || this.activeTileTypes[index] != tile)
        return true;
      GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
      Int3[] verts;
      int[] tris;
      tile.Load(out verts, out tris, rotation, yoffset);
      Bounds tileBounds = this.graph.GetTileBounds(x, z, tile.Width, tile.Depth);
      Int3 cuttingOffset = -(Int3) tileBounds.min;
      Int3[] outVertsArr = (Int3[]) null;
      int[] outTrisArr = (int[]) null;
      int outVCount;
      int outTCount;
      this.CutPoly(verts, tris, ref outVertsArr, ref outTrisArr, out outVCount, out outTCount, (Int3[]) null, cuttingOffset, tileBounds);
      this.DelaunayRefinement(outVertsArr, outTrisArr, ref outVCount, ref outTCount, true, false, -cuttingOffset);
      if (outTCount != outTrisArr.Length)
        outTrisArr = TileHandler.ShrinkArray<int>(outTrisArr, outTCount);
      if (outVCount != outVertsArr.Length)
        outVertsArr = TileHandler.ShrinkArray<Int3>(outVertsArr, outVCount);
      this.graph.ReplaceTile(x, z, rotation % 2 == 0 ? tile.Width : tile.Depth, rotation % 2 == 0 ? tile.Depth : tile.Width, outVertsArr, outTrisArr, false);
      GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
      AstarPath.active.QueueWorkItemFloodFill();
      return true;
    })));
  }

  [CompilerGenerated]
  public bool \u003CStartBatchLoad\u003Eb__20_0(bool force)
  {
    this.graph.StartBatchTileUpdate();
    return true;
  }

  [CompilerGenerated]
  public bool \u003CEndBatchLoad\u003Eb__21_0(bool force)
  {
    this.graph.EndBatchTileUpdate();
    return true;
  }

  public class TileType
  {
    public Int3[] verts;
    public int[] tris;
    public Int3 offset;
    public int lastYOffset;
    public int lastRotation;
    public int width;
    public int depth;
    public static int[] Rotations = new int[16 /*0x10*/]
    {
      1,
      0,
      0,
      1,
      0,
      1,
      -1,
      0,
      -1,
      0,
      0,
      -1,
      0,
      -1,
      1,
      0
    };

    public int Width => this.width;

    public int Depth => this.depth;

    public TileType(
      Int3[] sourceVerts,
      int[] sourceTris,
      Int3 tileSize,
      Int3 centerOffset,
      int width = 1,
      int depth = 1)
    {
      if (sourceVerts == null)
        throw new ArgumentNullException(nameof (sourceVerts));
      this.tris = sourceTris != null ? new int[sourceTris.Length] : throw new ArgumentNullException(nameof (sourceTris));
      for (int index = 0; index < this.tris.Length; ++index)
        this.tris[index] = sourceTris[index];
      this.verts = new Int3[sourceVerts.Length];
      for (int index = 0; index < sourceVerts.Length; ++index)
        this.verts[index] = sourceVerts[index] + centerOffset;
      this.offset = tileSize / 2f;
      this.offset.x *= width;
      this.offset.z *= depth;
      this.offset.y = 0;
      for (int index = 0; index < sourceVerts.Length; ++index)
        this.verts[index] = this.verts[index] + this.offset;
      this.lastRotation = 0;
      this.lastYOffset = 0;
      this.width = width;
      this.depth = depth;
    }

    public TileType(Mesh source, Int3 tileSize, Int3 centerOffset, int width = 1, int depth = 1)
    {
      Vector3[] vector3Array = !((UnityEngine.Object) source == (UnityEngine.Object) null) ? source.vertices : throw new ArgumentNullException(nameof (source));
      this.tris = source.triangles;
      this.verts = new Int3[vector3Array.Length];
      for (int index = 0; index < vector3Array.Length; ++index)
        this.verts[index] = (Int3) vector3Array[index] + centerOffset;
      this.offset = tileSize / 2f;
      this.offset.x *= width;
      this.offset.z *= depth;
      this.offset.y = 0;
      for (int index = 0; index < vector3Array.Length; ++index)
        this.verts[index] = this.verts[index] + this.offset;
      this.lastRotation = 0;
      this.lastYOffset = 0;
      this.width = width;
      this.depth = depth;
    }

    public void Load(out Int3[] verts, out int[] tris, int rotation, int yoffset)
    {
      rotation = (rotation % 4 + 4) % 4;
      int num1 = rotation;
      rotation = (rotation - this.lastRotation % 4 + 4) % 4;
      this.lastRotation = num1;
      verts = this.verts;
      int num2 = yoffset - this.lastYOffset;
      this.lastYOffset = yoffset;
      if (rotation != 0 || num2 != 0)
      {
        for (int index = 0; index < verts.Length; ++index)
        {
          Int3 int3_1 = verts[index] - this.offset;
          Int3 int3_2 = int3_1;
          int3_2.y += num2;
          int3_2.x = int3_1.x * TileHandler.TileType.Rotations[rotation * 4] + int3_1.z * TileHandler.TileType.Rotations[rotation * 4 + 1];
          int3_2.z = int3_1.x * TileHandler.TileType.Rotations[rotation * 4 + 2] + int3_1.z * TileHandler.TileType.Rotations[rotation * 4 + 3];
          verts[index] = int3_2 + this.offset;
        }
      }
      tris = this.tris;
    }
  }

  [Flags]
  public enum CutMode
  {
    CutAll = 1,
    CutDual = 2,
    CutExtra = 4,
  }
}
