// Decompiled with JetBrains decompiler
// Type: Pathfinding.BBTree
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public class BBTree
{
  public BBTree.BBTreeBox[] arr = new BBTree.BBTreeBox[6];
  public int count;
  public static Stopwatch watch = new Stopwatch();

  public Rect Size
  {
    get
    {
      if (this.count == 0)
        return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      IntRect rect = this.arr[0].rect;
      return Rect.MinMaxRect((float) rect.xmin * (1f / 1000f), (float) rect.ymin * (1f / 1000f), (float) rect.xmax * (1f / 1000f), (float) rect.ymax * (1f / 1000f));
    }
  }

  public void Clear() => this.count = 0;

  public void EnsureCapacity(int c)
  {
    if (this.arr.Length >= c)
      return;
    BBTree.BBTreeBox[] bbTreeBoxArray = new BBTree.BBTreeBox[Math.Max(c, (int) ((double) this.arr.Length * 1.5))];
    for (int index = 0; index < this.count; ++index)
      bbTreeBoxArray[index] = this.arr[index];
    this.arr = bbTreeBoxArray;
  }

  public int GetBox(MeshNode node, IntRect bounds)
  {
    if (this.count >= this.arr.Length)
      this.EnsureCapacity(this.count + 1);
    this.arr[this.count] = new BBTree.BBTreeBox(node, bounds);
    ++this.count;
    return this.count - 1;
  }

  public int GetBox(IntRect rect)
  {
    if (this.count >= this.arr.Length)
      this.EnsureCapacity(this.count + 1);
    this.arr[this.count] = new BBTree.BBTreeBox(rect);
    ++this.count;
    return this.count - 1;
  }

  public void RebuildFrom(MeshNode[] nodes)
  {
    this.Clear();
    if (nodes.Length == 0)
      return;
    this.EnsureCapacity(Mathf.CeilToInt((float) nodes.Length * 2.1f));
    int[] permutation = new int[nodes.Length];
    for (int index = 0; index < nodes.Length; ++index)
      permutation[index] = index;
    IntRect[] nodeBounds = new IntRect[nodes.Length];
    for (int index = 0; index < nodes.Length; ++index)
    {
      MeshNode node = nodes[index];
      Int3 vertex1 = node.GetVertex(0);
      Int3 vertex2 = node.GetVertex(1);
      Int3 vertex3 = node.GetVertex(2);
      IntRect intRect = new IntRect(vertex1.x, vertex1.z, vertex1.x, vertex1.z);
      intRect = intRect.ExpandToContain(vertex2.x, vertex2.z);
      intRect = intRect.ExpandToContain(vertex3.x, vertex3.z);
      nodeBounds[index] = intRect;
    }
    this.RebuildFromInternal(nodes, permutation, nodeBounds, 0, nodes.Length, false);
  }

  public static int SplitByX(MeshNode[] nodes, int[] permutation, int from, int to, int divider)
  {
    int index1 = to;
    for (int index2 = from; index2 < index1; ++index2)
    {
      if (nodes[permutation[index2]].position.x > divider)
      {
        --index1;
        int num = permutation[index1];
        permutation[index1] = permutation[index2];
        permutation[index2] = num;
        --index2;
      }
    }
    return index1;
  }

  public static int SplitByZ(MeshNode[] nodes, int[] permutation, int from, int to, int divider)
  {
    int index1 = to;
    for (int index2 = from; index2 < index1; ++index2)
    {
      if (nodes[permutation[index2]].position.z > divider)
      {
        --index1;
        int num = permutation[index1];
        permutation[index1] = permutation[index2];
        permutation[index2] = num;
        --index2;
      }
    }
    return index1;
  }

  public int RebuildFromInternal(
    MeshNode[] nodes,
    int[] permutation,
    IntRect[] nodeBounds,
    int from,
    int to,
    bool odd)
  {
    if (to - from == 1)
      return this.GetBox(nodes[permutation[from]], nodeBounds[permutation[from]]);
    IntRect rect = BBTree.NodeBounds(permutation, nodeBounds, from, to);
    int box = this.GetBox(rect);
    if (to - from == 2)
    {
      this.arr[box].left = this.GetBox(nodes[permutation[from]], nodeBounds[permutation[from]]);
      this.arr[box].right = this.GetBox(nodes[permutation[from + 1]], nodeBounds[permutation[from + 1]]);
      return box;
    }
    int num;
    if (odd)
    {
      int divider = (rect.xmin + rect.xmax) / 2;
      num = BBTree.SplitByX(nodes, permutation, from, to, divider);
    }
    else
    {
      int divider = (rect.ymin + rect.ymax) / 2;
      num = BBTree.SplitByZ(nodes, permutation, from, to, divider);
    }
    if (num == from || num == to)
    {
      if (!odd)
      {
        int divider = (rect.xmin + rect.xmax) / 2;
        num = BBTree.SplitByX(nodes, permutation, from, to, divider);
      }
      else
      {
        int divider = (rect.ymin + rect.ymax) / 2;
        num = BBTree.SplitByZ(nodes, permutation, from, to, divider);
      }
      if (num == from || num == to)
        num = (from + to) / 2;
    }
    this.arr[box].left = this.RebuildFromInternal(nodes, permutation, nodeBounds, from, num, !odd);
    this.arr[box].right = this.RebuildFromInternal(nodes, permutation, nodeBounds, num, to, !odd);
    return box;
  }

  public static IntRect NodeBounds(int[] permutation, IntRect[] nodeBounds, int from, int to)
  {
    if (to - from <= 0)
      throw new ArgumentException();
    IntRect nodeBound1 = nodeBounds[permutation[from]];
    for (int index = from + 1; index < to; ++index)
    {
      IntRect nodeBound2 = nodeBounds[permutation[index]];
      nodeBound1.xmin = Math.Min(nodeBound1.xmin, nodeBound2.xmin);
      nodeBound1.ymin = Math.Min(nodeBound1.ymin, nodeBound2.ymin);
      nodeBound1.xmax = Math.Max(nodeBound1.xmax, nodeBound2.xmax);
      nodeBound1.ymax = Math.Max(nodeBound1.ymax, nodeBound2.ymax);
    }
    return nodeBound1;
  }

  public NNInfo Query(Vector3 p, NNConstraint constraint)
  {
    if (this.count == 0)
      return new NNInfo((GraphNode) null);
    NNInfo nnInfo = new NNInfo();
    this.SearchBox(0, p, constraint, ref nnInfo);
    nnInfo.UpdateInfo();
    return nnInfo;
  }

  public NNInfo QueryCircle(Vector3 p, float radius, NNConstraint constraint)
  {
    if (this.count == 0)
      return new NNInfo((GraphNode) null);
    NNInfo nnInfo = new NNInfo((GraphNode) null);
    this.SearchBoxCircle(0, p, radius, constraint, ref nnInfo);
    nnInfo.UpdateInfo();
    return nnInfo;
  }

  public NNInfo QueryClosest(Vector3 p, NNConstraint constraint, out float distance)
  {
    distance = float.PositiveInfinity;
    return this.QueryClosest(p, constraint, ref distance, new NNInfo((GraphNode) null));
  }

  public NNInfo QueryClosestXZ(
    Vector3 p,
    NNConstraint constraint,
    ref float distance,
    NNInfo previous)
  {
    if (this.count == 0)
      return previous;
    this.SearchBoxClosestXZ(0, p, ref distance, constraint, ref previous);
    return previous;
  }

  public void SearchBoxClosestXZ(
    int boxi,
    Vector3 p,
    ref float closestDist,
    NNConstraint constraint,
    ref NNInfo nnInfo)
  {
    BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
    if (bbTreeBox.node != null)
    {
      Vector3 vector3 = bbTreeBox.node.ClosestPointOnNodeXZ(p);
      if (constraint != null && !constraint.Suitable((GraphNode) bbTreeBox.node))
        return;
      float d = (float) (((double) vector3.x - (double) p.x) * ((double) vector3.x - (double) p.x) + ((double) vector3.z - (double) p.z) * ((double) vector3.z - (double) p.z));
      if (nnInfo.constrainedNode == null)
      {
        nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
        nnInfo.constClampedPosition = vector3;
        closestDist = (float) Math.Sqrt((double) d);
      }
      else
      {
        if ((double) d >= (double) closestDist * (double) closestDist)
          return;
        nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
        nnInfo.constClampedPosition = vector3;
        closestDist = (float) Math.Sqrt((double) d);
      }
    }
    else
    {
      if (BBTree.RectIntersectsCircle(this.arr[bbTreeBox.left].rect, p, closestDist))
        this.SearchBoxClosestXZ(bbTreeBox.left, p, ref closestDist, constraint, ref nnInfo);
      if (!BBTree.RectIntersectsCircle(this.arr[bbTreeBox.right].rect, p, closestDist))
        return;
      this.SearchBoxClosestXZ(bbTreeBox.right, p, ref closestDist, constraint, ref nnInfo);
    }
  }

  public NNInfo QueryClosest(
    Vector3 p,
    NNConstraint constraint,
    ref float distance,
    NNInfo previous)
  {
    if (this.count == 0)
      return previous;
    this.SearchBoxClosest(0, p, ref distance, constraint, ref previous);
    return previous;
  }

  public void SearchBoxClosest(
    int boxi,
    Vector3 p,
    ref float closestDist,
    NNConstraint constraint,
    ref NNInfo nnInfo)
  {
    BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
    if (bbTreeBox.node != null)
    {
      if (!BBTree.NodeIntersectsCircle(bbTreeBox.node, p, closestDist))
        return;
      Vector3 vector3 = bbTreeBox.node.ClosestPointOnNode(p);
      if (constraint != null && !constraint.Suitable((GraphNode) bbTreeBox.node))
        return;
      float sqrMagnitude = (vector3 - p).sqrMagnitude;
      if (nnInfo.constrainedNode == null)
      {
        nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
        nnInfo.constClampedPosition = vector3;
        closestDist = (float) Math.Sqrt((double) sqrMagnitude);
      }
      else
      {
        if ((double) sqrMagnitude >= (double) closestDist * (double) closestDist)
          return;
        nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
        nnInfo.constClampedPosition = vector3;
        closestDist = (float) Math.Sqrt((double) sqrMagnitude);
      }
    }
    else
    {
      if (BBTree.RectIntersectsCircle(this.arr[bbTreeBox.left].rect, p, closestDist))
        this.SearchBoxClosest(bbTreeBox.left, p, ref closestDist, constraint, ref nnInfo);
      if (!BBTree.RectIntersectsCircle(this.arr[bbTreeBox.right].rect, p, closestDist))
        return;
      this.SearchBoxClosest(bbTreeBox.right, p, ref closestDist, constraint, ref nnInfo);
    }
  }

  public MeshNode QueryInside(Vector3 p, NNConstraint constraint)
  {
    return this.count == 0 ? (MeshNode) null : this.SearchBoxInside(0, p, constraint);
  }

  public MeshNode SearchBoxInside(int boxi, Vector3 p, NNConstraint constraint)
  {
    BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
    if (bbTreeBox.node != null)
    {
      if (bbTreeBox.node.ContainsPoint((Int3) p) && (constraint == null || constraint.Suitable((GraphNode) bbTreeBox.node)))
        return bbTreeBox.node;
    }
    else
    {
      if (this.arr[bbTreeBox.left].Contains(p))
      {
        MeshNode meshNode = this.SearchBoxInside(bbTreeBox.left, p, constraint);
        if (meshNode != null)
          return meshNode;
      }
      if (this.arr[bbTreeBox.right].Contains(p))
      {
        MeshNode meshNode = this.SearchBoxInside(bbTreeBox.right, p, constraint);
        if (meshNode != null)
          return meshNode;
      }
    }
    return (MeshNode) null;
  }

  public void SearchBoxCircle(
    int boxi,
    Vector3 p,
    float radius,
    NNConstraint constraint,
    ref NNInfo nnInfo)
  {
    BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
    if (bbTreeBox.node != null)
    {
      if (!BBTree.NodeIntersectsCircle(bbTreeBox.node, p, radius))
        return;
      Vector3 vector3 = bbTreeBox.node.ClosestPointOnNode(p);
      float sqrMagnitude = (vector3 - p).sqrMagnitude;
      if (nnInfo.node == null)
      {
        nnInfo.node = (GraphNode) bbTreeBox.node;
        nnInfo.clampedPosition = vector3;
      }
      else if ((double) sqrMagnitude < (double) (nnInfo.clampedPosition - p).sqrMagnitude)
      {
        nnInfo.node = (GraphNode) bbTreeBox.node;
        nnInfo.clampedPosition = vector3;
      }
      if (constraint != null && !constraint.Suitable((GraphNode) bbTreeBox.node))
        return;
      if (nnInfo.constrainedNode == null)
      {
        nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
        nnInfo.constClampedPosition = vector3;
      }
      else
      {
        if ((double) sqrMagnitude >= (double) (nnInfo.constClampedPosition - p).sqrMagnitude)
          return;
        nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
        nnInfo.constClampedPosition = vector3;
      }
    }
    else
    {
      if (BBTree.RectIntersectsCircle(this.arr[bbTreeBox.left].rect, p, radius))
        this.SearchBoxCircle(bbTreeBox.left, p, radius, constraint, ref nnInfo);
      if (!BBTree.RectIntersectsCircle(this.arr[bbTreeBox.right].rect, p, radius))
        return;
      this.SearchBoxCircle(bbTreeBox.right, p, radius, constraint, ref nnInfo);
    }
  }

  public void SearchBox(int boxi, Vector3 p, NNConstraint constraint, ref NNInfo nnInfo)
  {
    BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
    if (bbTreeBox.node != null)
    {
      if (!bbTreeBox.node.ContainsPoint((Int3) p))
        return;
      if (nnInfo.node == null)
        nnInfo.node = (GraphNode) bbTreeBox.node;
      else if ((double) Mathf.Abs(((Vector3) bbTreeBox.node.position).y - p.y) < (double) Mathf.Abs(((Vector3) nnInfo.node.position).y - p.y))
        nnInfo.node = (GraphNode) bbTreeBox.node;
      if (!constraint.Suitable((GraphNode) bbTreeBox.node))
        return;
      if (nnInfo.constrainedNode == null)
      {
        nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
      }
      else
      {
        if ((double) Mathf.Abs((float) bbTreeBox.node.position.y - p.y) >= (double) Mathf.Abs((float) nnInfo.constrainedNode.position.y - p.y))
          return;
        nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
      }
    }
    else
    {
      if (this.arr[bbTreeBox.left].Contains(p))
        this.SearchBox(bbTreeBox.left, p, constraint, ref nnInfo);
      if (!this.arr[bbTreeBox.right].Contains(p))
        return;
      this.SearchBox(bbTreeBox.right, p, constraint, ref nnInfo);
    }
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
    if (this.count == 0)
      return;
    this.OnDrawGizmos(0, 0);
  }

  public void OnDrawGizmos(int boxi, int depth)
  {
    BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
    Vector3 vector3_1 = (Vector3) new Int3(bbTreeBox.rect.xmin, 0, bbTreeBox.rect.ymin);
    Vector3 vector3_2 = (Vector3) new Int3(bbTreeBox.rect.xmax, 0, bbTreeBox.rect.ymax);
    Vector3 vector3_3 = vector3_2;
    Vector3 center = (vector3_1 + vector3_3) * 0.5f;
    Vector3 size = (vector3_2 - center) * 2f;
    size = new Vector3(size.x, 1f, size.z);
    center.y += (float) (depth * 2);
    Gizmos.color = AstarMath.IntToColor(depth, 1f);
    Gizmos.DrawCube(center, size);
    if (bbTreeBox.node != null)
      return;
    this.OnDrawGizmos(bbTreeBox.left, depth + 1);
    this.OnDrawGizmos(bbTreeBox.right, depth + 1);
  }

  public static bool NodeIntersectsCircle(MeshNode node, Vector3 p, float radius)
  {
    return float.IsPositiveInfinity(radius) || (double) (p - node.ClosestPointOnNode(p)).sqrMagnitude < (double) radius * (double) radius;
  }

  public static bool RectIntersectsCircle(IntRect r, Vector3 p, float radius)
  {
    if (float.IsPositiveInfinity(radius))
      return true;
    Vector3 vector3 = p;
    p.x = Math.Max(p.x, (float) r.xmin * (1f / 1000f));
    p.x = Math.Min(p.x, (float) r.xmax * (1f / 1000f));
    p.z = Math.Max(p.z, (float) r.ymin * (1f / 1000f));
    p.z = Math.Min(p.z, (float) r.ymax * (1f / 1000f));
    return ((double) p.x - (double) vector3.x) * ((double) p.x - (double) vector3.x) + ((double) p.z - (double) vector3.z) * ((double) p.z - (double) vector3.z) < (double) radius * (double) radius;
  }

  public static IntRect ExpandToContain(IntRect r, IntRect r2) => IntRect.Union(r, r2);

  public struct BBTreeBox
  {
    public IntRect rect;
    public MeshNode node;
    public int left;
    public int right;

    public bool IsLeaf => this.node != null;

    public BBTreeBox(IntRect rect)
    {
      this.node = (MeshNode) null;
      this.rect = rect;
      this.left = this.right = -1;
    }

    public BBTreeBox(MeshNode node, IntRect rect)
    {
      this.node = node;
      this.rect = rect;
      this.left = this.right = -1;
    }

    public bool Contains(Vector3 p)
    {
      Int3 int3 = (Int3) p;
      return this.rect.Contains(int3.x, int3.z);
    }
  }
}
