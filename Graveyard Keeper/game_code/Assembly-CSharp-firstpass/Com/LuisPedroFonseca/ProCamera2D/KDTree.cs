// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.KDTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public class KDTree
{
  public KDTree[] lr;
  public Vector3 pivot;
  public int pivotIndex;
  public int axis;
  public const int numDims = 3;

  public KDTree() => this.lr = new KDTree[2];

  public static KDTree MakeFromPoints(params Vector3[] points)
  {
    int[] inds = KDTree.Iota(points.Length);
    return KDTree.MakeFromPointsInner(0, 0, points.Length - 1, points, inds);
  }

  public static KDTree MakeFromPointsInner(
    int depth,
    int stIndex,
    int enIndex,
    Vector3[] points,
    int[] inds)
  {
    KDTree kdTree = new KDTree();
    kdTree.axis = depth % 3;
    int pivotIndex = KDTree.FindPivotIndex(points, inds, stIndex, enIndex, kdTree.axis);
    kdTree.pivotIndex = inds[pivotIndex];
    kdTree.pivot = points[kdTree.pivotIndex];
    int enIndex1 = pivotIndex - 1;
    if (enIndex1 >= stIndex)
      kdTree.lr[0] = KDTree.MakeFromPointsInner(depth + 1, stIndex, enIndex1, points, inds);
    int stIndex1 = pivotIndex + 1;
    if (stIndex1 <= enIndex)
      kdTree.lr[1] = KDTree.MakeFromPointsInner(depth + 1, stIndex1, enIndex, points, inds);
    return kdTree;
  }

  public static void SwapElements(int[] arr, int a, int b)
  {
    int num = arr[a];
    arr[a] = arr[b];
    arr[b] = num;
  }

  public static int FindSplitPoint(
    Vector3[] points,
    int[] inds,
    int stIndex,
    int enIndex,
    int axis)
  {
    float num1 = points[inds[stIndex]][axis];
    float num2 = points[inds[enIndex]][axis];
    int index = (stIndex + enIndex) / 2;
    float num3 = points[inds[index]][axis];
    if ((double) num1 > (double) num2)
    {
      if ((double) num3 > (double) num1)
        return stIndex;
      return (double) num2 > (double) num3 ? enIndex : index;
    }
    if ((double) num1 > (double) num3)
      return stIndex;
    return (double) num3 > (double) num2 ? enIndex : index;
  }

  public static int FindPivotIndex(
    Vector3[] points,
    int[] inds,
    int stIndex,
    int enIndex,
    int axis)
  {
    int splitPoint = KDTree.FindSplitPoint(points, inds, stIndex, enIndex, axis);
    Vector3 point = points[inds[splitPoint]];
    KDTree.SwapElements(inds, stIndex, splitPoint);
    int index = stIndex + 1;
    int b = enIndex;
    while (index <= b)
    {
      if ((double) points[inds[index]][axis] > (double) point[axis])
      {
        KDTree.SwapElements(inds, index, b);
        --b;
      }
      else
      {
        KDTree.SwapElements(inds, index - 1, index);
        ++index;
      }
    }
    return index - 1;
  }

  public static int[] Iota(int num)
  {
    int[] numArray = new int[num];
    for (int index = 0; index < num; ++index)
      numArray[index] = index;
    return numArray;
  }

  public int FindNearest(Vector3 pt)
  {
    float bestSqSoFar = 1E+09f;
    int bestIndex = -1;
    this.Search(pt, ref bestSqSoFar, ref bestIndex);
    return bestIndex;
  }

  public void Search(Vector3 pt, ref float bestSqSoFar, ref int bestIndex)
  {
    float sqrMagnitude = (this.pivot - pt).sqrMagnitude;
    if ((double) sqrMagnitude < (double) bestSqSoFar)
    {
      bestSqSoFar = sqrMagnitude;
      bestIndex = this.pivotIndex;
    }
    double num1 = (double) pt[this.axis] - (double) this.pivot[this.axis];
    int index1 = num1 <= 0.0 ? 0 : 1;
    if (this.lr[index1] != null)
      this.lr[index1].Search(pt, ref bestSqSoFar, ref bestIndex);
    int index2 = (index1 + 1) % 2;
    float num2 = (float) (num1 * num1);
    if (this.lr[index2] == null || (double) bestSqSoFar <= (double) num2)
      return;
    this.lr[index2].Search(pt, ref bestSqSoFar, ref bestIndex);
  }

  public float DistFromSplitPlane(Vector3 pt, Vector3 planePt, int axis)
  {
    return pt[axis] - planePt[axis];
  }

  public string Dump(int level)
  {
    string str = this.pivotIndex.ToString().PadLeft(level) + "\n";
    if (this.lr[0] != null)
      str += this.lr[0].Dump(level + 2);
    if (this.lr[1] != null)
      str += this.lr[1].Dump(level + 2);
    return str;
  }
}
