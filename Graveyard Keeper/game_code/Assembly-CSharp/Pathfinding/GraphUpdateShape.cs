// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphUpdateShape
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public class GraphUpdateShape
{
  public Vector3[] _points;
  public Vector3[] _convexPoints;
  public bool _convex;

  public Vector3[] points
  {
    get => this._points;
    set
    {
      this._points = value;
      if (!this.convex)
        return;
      this.CalculateConvexHull();
    }
  }

  public bool convex
  {
    get => this._convex;
    set
    {
      if (this._convex != value & value)
      {
        this._convex = value;
        this.CalculateConvexHull();
      }
      else
        this._convex = value;
    }
  }

  public void CalculateConvexHull()
  {
    if (this.points == null)
    {
      this._convexPoints = (Vector3[]) null;
    }
    else
    {
      this._convexPoints = Polygon.ConvexHullXZ(this.points);
      for (int index = 0; index < this._convexPoints.Length; ++index)
        Debug.DrawLine(this._convexPoints[index], this._convexPoints[(index + 1) % this._convexPoints.Length], Color.green);
    }
  }

  public Bounds GetBounds()
  {
    if (this.points == null || this.points.Length == 0)
      return new Bounds();
    Vector3 lhs1 = this.points[0];
    Vector3 lhs2 = this.points[0];
    for (int index = 0; index < this.points.Length; ++index)
    {
      lhs1 = Vector3.Min(lhs1, this.points[index]);
      lhs2 = Vector3.Max(lhs2, this.points[index]);
    }
    return new Bounds((lhs1 + lhs2) * 0.5f, lhs2 - lhs1);
  }

  public bool Contains(GraphNode node) => this.Contains((Vector3) node.position);

  public bool Contains(Vector3 point)
  {
    if (this.convex)
    {
      if (this._convexPoints == null)
        return false;
      int index1 = 0;
      int index2 = this._convexPoints.Length - 1;
      for (; index1 < this._convexPoints.Length; ++index1)
      {
        if (VectorMath.RightOrColinearXZ(this._convexPoints[index1], this._convexPoints[index2], point))
          return false;
        index2 = index1;
      }
      return true;
    }
    return this._points != null && Polygon.ContainsPointXZ(this._points, point);
  }
}
