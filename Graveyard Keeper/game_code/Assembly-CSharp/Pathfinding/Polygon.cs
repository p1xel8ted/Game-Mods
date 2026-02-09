// Decompiled with JetBrains decompiler
// Type: Pathfinding.Polygon
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public static class Polygon
{
  [Obsolete("Use VectorMath.SignedTriangleAreaTimes2XZ instead")]
  public static long TriangleArea2(Int3 a, Int3 b, Int3 c)
  {
    return VectorMath.SignedTriangleAreaTimes2XZ(a, b, c);
  }

  [Obsolete("Use VectorMath.SignedTriangleAreaTimes2XZ instead")]
  public static float TriangleArea2(Vector3 a, Vector3 b, Vector3 c)
  {
    return VectorMath.SignedTriangleAreaTimes2XZ(a, b, c);
  }

  [Obsolete("Use TriangleArea2 instead to avoid confusion regarding the factor 2")]
  public static long TriangleArea(Int3 a, Int3 b, Int3 c) => Polygon.TriangleArea2(a, b, c);

  [Obsolete("Use TriangleArea2 instead to avoid confusion regarding the factor 2")]
  public static float TriangleArea(Vector3 a, Vector3 b, Vector3 c)
  {
    return Polygon.TriangleArea2(a, b, c);
  }

  [Obsolete("Use ContainsPointXZ instead")]
  public static bool ContainsPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
  {
    return Polygon.ContainsPointXZ(a, b, c, p);
  }

  public static bool ContainsPointXZ(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
  {
    return VectorMath.IsClockwiseMarginXZ(a, b, p) && VectorMath.IsClockwiseMarginXZ(b, c, p) && VectorMath.IsClockwiseMarginXZ(c, a, p);
  }

  [Obsolete("Use ContainsPointXZ instead")]
  public static bool ContainsPoint(Int3 a, Int3 b, Int3 c, Int3 p)
  {
    return Polygon.ContainsPointXZ(a, b, c, p);
  }

  public static bool ContainsPointXZ(Int3 a, Int3 b, Int3 c, Int3 p)
  {
    return VectorMath.IsClockwiseOrColinearXZ(a, b, p) && VectorMath.IsClockwiseOrColinearXZ(b, c, p) && VectorMath.IsClockwiseOrColinearXZ(c, a, p);
  }

  public static bool ContainsPoint(Int2 a, Int2 b, Int2 c, Int2 p)
  {
    return VectorMath.IsClockwiseOrColinear(a, b, p) && VectorMath.IsClockwiseOrColinear(b, c, p) && VectorMath.IsClockwiseOrColinear(c, a, p);
  }

  [Obsolete("Use ContainsPointXZ instead")]
  public static bool ContainsPoint(Vector3[] polyPoints, Vector3 p)
  {
    return Polygon.ContainsPointXZ(polyPoints, p);
  }

  public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
  {
    int index1 = polyPoints.Length - 1;
    bool flag = false;
    for (int index2 = 0; index2 < polyPoints.Length; index1 = index2++)
    {
      if (((double) polyPoints[index2].y <= (double) p.y && (double) p.y < (double) polyPoints[index1].y || (double) polyPoints[index1].y <= (double) p.y && (double) p.y < (double) polyPoints[index2].y) && (double) p.x < ((double) polyPoints[index1].x - (double) polyPoints[index2].x) * ((double) p.y - (double) polyPoints[index2].y) / ((double) polyPoints[index1].y - (double) polyPoints[index2].y) + (double) polyPoints[index2].x)
        flag = !flag;
    }
    return flag;
  }

  public static bool ContainsPointXZ(Vector3[] polyPoints, Vector3 p)
  {
    int index1 = polyPoints.Length - 1;
    bool flag = false;
    for (int index2 = 0; index2 < polyPoints.Length; index1 = index2++)
    {
      if (((double) polyPoints[index2].z <= (double) p.z && (double) p.z < (double) polyPoints[index1].z || (double) polyPoints[index1].z <= (double) p.z && (double) p.z < (double) polyPoints[index2].z) && (double) p.x < ((double) polyPoints[index1].x - (double) polyPoints[index2].x) * ((double) p.z - (double) polyPoints[index2].z) / ((double) polyPoints[index1].z - (double) polyPoints[index2].z) + (double) polyPoints[index2].x)
        flag = !flag;
    }
    return flag;
  }

  [Obsolete("Use VectorMath.RightXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
  public static bool LeftNotColinear(Vector3 a, Vector3 b, Vector3 p)
  {
    return VectorMath.RightXZ(a, b, p);
  }

  [Obsolete("Use VectorMath.RightOrColinearXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
  public static bool Left(Vector3 a, Vector3 b, Vector3 p) => VectorMath.RightOrColinearXZ(a, b, p);

  [Obsolete("Use VectorMath.RightOrColinear instead. Note that it now uses a left handed coordinate system (same as Unity)")]
  public static bool Left(Vector2 a, Vector2 b, Vector2 p) => VectorMath.RightOrColinear(a, b, p);

  [Obsolete("Use VectorMath.RightOrColinearXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
  public static bool Left(Int3 a, Int3 b, Int3 p) => VectorMath.RightOrColinearXZ(a, b, p);

  [Obsolete("Use VectorMath.RightXZ instead. Note that it now uses a left handed coordinate system (same as Unity)")]
  public static bool LeftNotColinear(Int3 a, Int3 b, Int3 p) => VectorMath.RightXZ(a, b, p);

  [Obsolete("Use VectorMath.RightOrColinear instead. Note that it now uses a left handed coordinate system (same as Unity)")]
  public static bool Left(Int2 a, Int2 b, Int2 p) => VectorMath.RightOrColinear(a, b, p);

  [Obsolete("Use VectorMath.IsClockwiseMarginXZ instead")]
  public static bool IsClockwiseMargin(Vector3 a, Vector3 b, Vector3 c)
  {
    return VectorMath.IsClockwiseMarginXZ(a, b, c);
  }

  [Obsolete("Use VectorMath.IsClockwiseXZ instead")]
  public static bool IsClockwise(Vector3 a, Vector3 b, Vector3 c)
  {
    return VectorMath.IsClockwiseXZ(a, b, c);
  }

  [Obsolete("Use VectorMath.IsClockwiseXZ instead")]
  public static bool IsClockwise(Int3 a, Int3 b, Int3 c) => VectorMath.IsClockwiseXZ(a, b, c);

  [Obsolete("Use VectorMath.IsClockwiseOrColinearXZ instead")]
  public static bool IsClockwiseMargin(Int3 a, Int3 b, Int3 c)
  {
    return VectorMath.IsClockwiseOrColinearXZ(a, b, c);
  }

  [Obsolete("Use VectorMath.IsClockwiseOrColinear instead")]
  public static bool IsClockwiseMargin(Int2 a, Int2 b, Int2 c)
  {
    return VectorMath.IsClockwiseOrColinear(a, b, c);
  }

  [Obsolete("Use VectorMath.IsColinearXZ instead")]
  public static bool IsColinear(Int3 a, Int3 b, Int3 c) => VectorMath.IsColinearXZ(a, b, c);

  [Obsolete("Use VectorMath.IsColinearAlmostXZ instead")]
  public static bool IsColinearAlmost(Int3 a, Int3 b, Int3 c)
  {
    return VectorMath.IsColinearAlmostXZ(a, b, c);
  }

  [Obsolete("Use VectorMath.IsColinearXZ instead")]
  public static bool IsColinear(Vector3 a, Vector3 b, Vector3 c)
  {
    return VectorMath.IsColinearXZ(a, b, c);
  }

  [Obsolete("Marked for removal since it is not used by any part of the A* Pathfinding Project")]
  public static bool IntersectsUnclamped(Vector3 a, Vector3 b, Vector3 a2, Vector3 b2)
  {
    return VectorMath.RightOrColinearXZ(a, b, a2) != VectorMath.RightOrColinearXZ(a, b, b2);
  }

  [Obsolete("Use VectorMath.SegmentsIntersect instead")]
  public static bool Intersects(Int2 start1, Int2 end1, Int2 start2, Int2 end2)
  {
    return VectorMath.SegmentsIntersect(start1, end1, start2, end2);
  }

  [Obsolete("Use VectorMath.SegmentsIntersectXZ instead")]
  public static bool Intersects(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    return VectorMath.SegmentsIntersectXZ(start1, end1, start2, end2);
  }

  [Obsolete("Use VectorMath.SegmentsIntersectXZ instead")]
  public static bool Intersects(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
  {
    return VectorMath.SegmentsIntersectXZ(start1, end1, start2, end2);
  }

  [Obsolete("Use VectorMath.LineDirIntersectionPointXZ instead")]
  public static Vector3 IntersectionPointOptimized(
    Vector3 start1,
    Vector3 dir1,
    Vector3 start2,
    Vector3 dir2)
  {
    return VectorMath.LineDirIntersectionPointXZ(start1, dir1, start2, dir2);
  }

  [Obsolete("Use VectorMath.LineDirIntersectionPointXZ instead")]
  public static Vector3 IntersectionPointOptimized(
    Vector3 start1,
    Vector3 dir1,
    Vector3 start2,
    Vector3 dir2,
    out bool intersects)
  {
    return VectorMath.LineDirIntersectionPointXZ(start1, dir1, start2, dir2, out intersects);
  }

  [Obsolete("Use VectorMath.RaySegmentIntersectXZ instead")]
  public static bool IntersectionFactorRaySegment(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    return VectorMath.RaySegmentIntersectXZ(start1, end1, start2, end2);
  }

  [Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
  public static bool IntersectionFactor(
    Int3 start1,
    Int3 end1,
    Int3 start2,
    Int3 end2,
    out float factor1,
    out float factor2)
  {
    return VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2, out factor1, out factor2);
  }

  [Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
  public static bool IntersectionFactor(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out float factor1,
    out float factor2)
  {
    return VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2, out factor1, out factor2);
  }

  [Obsolete("Use VectorMath.LineRayIntersectionFactorXZ instead")]
  public static float IntersectionFactorRay(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    return VectorMath.LineRayIntersectionFactorXZ(start1, end1, start2, end2);
  }

  [Obsolete("Use VectorMath.LineIntersectionFactorXZ instead")]
  public static float IntersectionFactor(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2)
  {
    return VectorMath.LineIntersectionFactorXZ(start1, end1, start2, end2);
  }

  [Obsolete("Use VectorMath.LineIntersectionPointXZ instead")]
  public static Vector3 IntersectionPoint(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2)
  {
    return VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2);
  }

  [Obsolete("Use VectorMath.LineIntersectionPointXZ instead")]
  public static Vector3 IntersectionPoint(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out bool intersects)
  {
    return VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2, out intersects);
  }

  [Obsolete("Use VectorMath.LineIntersectionPoint instead")]
  public static Vector2 IntersectionPoint(
    Vector2 start1,
    Vector2 end1,
    Vector2 start2,
    Vector2 end2)
  {
    return VectorMath.LineIntersectionPoint(start1, end1, start2, end2);
  }

  [Obsolete("Use VectorMath.LineIntersectionPoint instead")]
  public static Vector2 IntersectionPoint(
    Vector2 start1,
    Vector2 end1,
    Vector2 start2,
    Vector2 end2,
    out bool intersects)
  {
    return VectorMath.LineIntersectionPoint(start1, end1, start2, end2, out intersects);
  }

  [Obsolete("Use VectorMath.SegmentIntersectionPointXZ instead")]
  public static Vector3 SegmentIntersectionPoint(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out bool intersects)
  {
    return VectorMath.SegmentIntersectionPointXZ(start1, end1, start2, end2, out intersects);
  }

  [Obsolete("Use ConvexHullXZ instead")]
  public static Vector3[] ConvexHull(Vector3[] points) => Polygon.ConvexHullXZ(points);

  public static Vector3[] ConvexHullXZ(Vector3[] points)
  {
    if (points.Length == 0)
      return new Vector3[0];
    List<Vector3> list = ListPool<Vector3>.Claim();
    int index1 = 0;
    for (int index2 = 1; index2 < points.Length; ++index2)
    {
      if ((double) points[index2].x < (double) points[index1].x)
        index1 = index2;
    }
    int num1 = index1;
    int num2 = 0;
    do
    {
      list.Add(points[index1]);
      int index3 = 0;
      for (int index4 = 0; index4 < points.Length; ++index4)
      {
        if (index3 == index1 || !VectorMath.RightOrColinearXZ(points[index1], points[index3], points[index4]))
          index3 = index4;
      }
      index1 = index3;
      ++num2;
      if (num2 > 10000)
      {
        Debug.LogWarning((object) "Infinite Loop in Convex Hull Calculation");
        break;
      }
    }
    while (index1 != num1);
    Vector3[] array = list.ToArray();
    ListPool<Vector3>.Release(list);
    return array;
  }

  [Obsolete("Use VectorMath.SegmentIntersectsBounds instead")]
  public static bool LineIntersectsBounds(Bounds bounds, Vector3 a, Vector3 b)
  {
    return VectorMath.SegmentIntersectsBounds(bounds, a, b);
  }

  public static Vector3[] Subdivide(Vector3[] path, int subdivisions)
  {
    subdivisions = subdivisions < 0 ? 0 : subdivisions;
    if (subdivisions == 0)
      return path;
    Vector3[] vector3Array = new Vector3[(path.Length - 1) * (int) Mathf.Pow(2f, (float) subdivisions) + 1];
    int index1 = 0;
    for (int index2 = 0; index2 < path.Length - 1; ++index2)
    {
      float num = 1f / Mathf.Pow(2f, (float) subdivisions);
      for (float t = 0.0f; (double) t < 1.0; t += num)
      {
        vector3Array[index1] = Vector3.Lerp(path[index2], path[index2 + 1], Mathf.SmoothStep(0.0f, 1f, t));
        ++index1;
      }
    }
    vector3Array[index1] = path[path.Length - 1];
    return vector3Array;
  }

  [Obsolete("Scheduled for removal since it is not used by any part of the A* Pathfinding Project")]
  public static Vector3 ClosestPointOnTriangle(Vector3[] triangle, Vector3 point)
  {
    return Polygon.ClosestPointOnTriangle(triangle[0], triangle[1], triangle[2], point);
  }

  public static Vector2 ClosestPointOnTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
  {
    Vector2 lhs1 = b - a;
    Vector2 lhs2 = c - a;
    Vector2 rhs1 = p - a;
    float num1 = Vector2.Dot(lhs1, rhs1);
    float num2 = Vector2.Dot(lhs2, rhs1);
    if ((double) num1 <= 0.0 && (double) num2 <= 0.0)
      return a;
    Vector2 rhs2 = p - b;
    float num3 = Vector2.Dot(lhs1, rhs2);
    float num4 = Vector2.Dot(lhs2, rhs2);
    if ((double) num3 >= 0.0 && (double) num4 <= (double) num3)
      return b;
    if ((double) num1 >= 0.0 && (double) num3 <= 0.0 && (double) num1 * (double) num4 - (double) num3 * (double) num2 <= 0.0)
    {
      float num5 = num1 / (num1 - num3);
      return a + lhs1 * num5;
    }
    Vector2 rhs3 = p - c;
    float num6 = Vector2.Dot(lhs1, rhs3);
    float num7 = Vector2.Dot(lhs2, rhs3);
    if ((double) num7 >= 0.0 && (double) num6 <= (double) num7)
      return c;
    if ((double) num2 >= 0.0 && (double) num7 <= 0.0 && (double) num6 * (double) num2 - (double) num1 * (double) num7 <= 0.0)
    {
      float num8 = num2 / (num2 - num7);
      return a + lhs2 * num8;
    }
    if ((double) num4 - (double) num3 < 0.0 || (double) num6 - (double) num7 < 0.0 || (double) num3 * (double) num7 - (double) num6 * (double) num4 > 0.0)
      return p;
    float num9 = (float) (((double) num4 - (double) num3) / ((double) num4 - (double) num3 + ((double) num6 - (double) num7)));
    return b + (c - b) * num9;
  }

  public static Vector3 ClosestPointOnTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
  {
    Vector3 lhs1 = b - a;
    Vector3 lhs2 = c - a;
    Vector3 rhs1 = p - a;
    float num1 = Vector3.Dot(lhs1, rhs1);
    float num2 = Vector3.Dot(lhs2, rhs1);
    if ((double) num1 <= 0.0 && (double) num2 <= 0.0)
      return a;
    Vector3 rhs2 = p - b;
    float num3 = Vector3.Dot(lhs1, rhs2);
    float num4 = Vector3.Dot(lhs2, rhs2);
    if ((double) num3 >= 0.0 && (double) num4 <= (double) num3)
      return b;
    float num5 = (float) ((double) num1 * (double) num4 - (double) num3 * (double) num2);
    if ((double) num1 >= 0.0 && (double) num3 <= 0.0 && (double) num5 <= 0.0)
    {
      float num6 = num1 / (num1 - num3);
      return a + lhs1 * num6;
    }
    Vector3 rhs3 = p - c;
    float num7 = Vector3.Dot(lhs1, rhs3);
    float num8 = Vector3.Dot(lhs2, rhs3);
    if ((double) num8 >= 0.0 && (double) num7 <= (double) num8)
      return c;
    float num9 = (float) ((double) num7 * (double) num2 - (double) num1 * (double) num8);
    if ((double) num2 >= 0.0 && (double) num8 <= 0.0 && (double) num9 <= 0.0)
    {
      float num10 = num2 / (num2 - num8);
      return a + lhs2 * num10;
    }
    float num11 = (float) ((double) num3 * (double) num8 - (double) num7 * (double) num4);
    if ((double) num4 - (double) num3 >= 0.0 && (double) num7 - (double) num8 >= 0.0 && (double) num11 <= 0.0)
    {
      float num12 = (float) (((double) num4 - (double) num3) / ((double) num4 - (double) num3 + ((double) num7 - (double) num8)));
      return b + (c - b) * num12;
    }
    float num13 = (float) (1.0 / ((double) num11 + (double) num9 + (double) num5));
    float num14 = num9 * num13;
    float num15 = num5 * num13;
    return a + lhs1 * num14 + lhs2 * num15;
  }

  [Obsolete("Use VectorMath.SqrDistanceSegmentSegment instead")]
  public static float DistanceSegmentSegment3D(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
  {
    return VectorMath.SqrDistanceSegmentSegment(s1, e1, s2, e2);
  }
}
