// Decompiled with JetBrains decompiler
// Type: Pathfinding.VectorMath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public static class VectorMath
{
  public static Vector3 ClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    Vector3 rhs = Vector3.Normalize(lineEnd - lineStart);
    float num = Vector3.Dot(point - lineStart, rhs);
    return lineStart + num * rhs;
  }

  public static float ClosestPointOnLineFactor(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    Vector3 rhs = lineEnd - lineStart;
    float sqrMagnitude = rhs.sqrMagnitude;
    return (double) sqrMagnitude <= 1E-06 ? 0.0f : Vector3.Dot(point - lineStart, rhs) / sqrMagnitude;
  }

  public static float ClosestPointOnLineFactor(Int3 lineStart, Int3 lineEnd, Int3 point)
  {
    Int3 rhs = lineEnd - lineStart;
    float sqrMagnitude = rhs.sqrMagnitude;
    float num = (float) Int3.Dot(point - lineStart, rhs);
    if ((double) sqrMagnitude != 0.0)
      num /= sqrMagnitude;
    return num;
  }

  public static float ClosestPointOnLineFactor(Int2 lineStart, Int2 lineEnd, Int2 point)
  {
    Int2 b = lineEnd - lineStart;
    double sqrMagnitudeLong = (double) b.sqrMagnitudeLong;
    double num = (double) Int2.DotLong(point - lineStart, b);
    if (sqrMagnitudeLong != 0.0)
      num /= sqrMagnitudeLong;
    return (float) num;
  }

  public static Vector3 ClosestPointOnSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    Vector3 rhs = lineEnd - lineStart;
    float sqrMagnitude = rhs.sqrMagnitude;
    if ((double) sqrMagnitude <= 1E-06)
      return lineStart;
    float num = Vector3.Dot(point - lineStart, rhs) / sqrMagnitude;
    return lineStart + Mathf.Clamp01(num) * rhs;
  }

  public static Vector3 ClosestPointOnSegmentXZ(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    lineStart.y = point.y;
    lineEnd.y = point.y;
    Vector3 vector3 = (lineEnd - lineStart) with
    {
      y = 0.0f
    };
    float magnitude = vector3.magnitude;
    Vector3 rhs = (double) magnitude > 1.4012984643248171E-45 ? vector3 / magnitude : Vector3.zero;
    float num = Vector3.Dot(point - lineStart, rhs);
    return lineStart + Mathf.Clamp(num, 0.0f, vector3.magnitude) * rhs;
  }

  public static float SqrDistancePointSegmentApproximate(
    int x,
    int z,
    int px,
    int pz,
    int qx,
    int qz)
  {
    float num1 = (float) (qx - px);
    float num2 = (float) (qz - pz);
    float num3 = (float) (x - px);
    float num4 = (float) (z - pz);
    float num5 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    float num6 = (float) ((double) num1 * (double) num3 + (double) num2 * (double) num4);
    if ((double) num5 > 0.0)
      num6 /= num5;
    if ((double) num6 < 0.0)
      num6 = 0.0f;
    else if ((double) num6 > 1.0)
      num6 = 1f;
    float num7 = (float) px + num6 * num1 - (float) x;
    float num8 = (float) pz + num6 * num2 - (float) z;
    return (float) ((double) num7 * (double) num7 + (double) num8 * (double) num8);
  }

  public static float SqrDistancePointSegmentApproximate(Int3 a, Int3 b, Int3 p)
  {
    float num1 = (float) (b.x - a.x);
    float num2 = (float) (b.z - a.z);
    float num3 = (float) (p.x - a.x);
    float num4 = (float) (p.z - a.z);
    float num5 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    float num6 = (float) ((double) num1 * (double) num3 + (double) num2 * (double) num4);
    if ((double) num5 > 0.0)
      num6 /= num5;
    if ((double) num6 < 0.0)
      num6 = 0.0f;
    else if ((double) num6 > 1.0)
      num6 = 1f;
    float num7 = (float) a.x + num6 * num1 - (float) p.x;
    float num8 = (float) a.z + num6 * num2 - (float) p.z;
    return (float) ((double) num7 * (double) num7 + (double) num8 * (double) num8);
  }

  public static float SqrDistancePointSegment(Vector3 a, Vector3 b, Vector3 p)
  {
    return (VectorMath.ClosestPointOnSegment(a, b, p) - p).sqrMagnitude;
  }

  public static float SqrDistanceSegmentSegment(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
  {
    Vector3 vector3_1 = e1 - s1;
    Vector3 vector3_2 = e2 - s2;
    Vector3 rhs = s1 - s2;
    float num1 = Vector3.Dot(vector3_1, vector3_1);
    float num2 = Vector3.Dot(vector3_1, vector3_2);
    float num3 = Vector3.Dot(vector3_2, vector3_2);
    float num4 = Vector3.Dot(vector3_1, rhs);
    float num5 = Vector3.Dot(vector3_2, rhs);
    double num6;
    float num7 = (float) (num6 = (double) num1 * (double) num3 - (double) num2 * (double) num2);
    float num8 = (float) num6;
    float num9;
    float num10;
    if (num6 < 9.9999999747524271E-07)
    {
      num9 = 0.0f;
      num7 = 1f;
      num10 = num5;
      num8 = num3;
    }
    else
    {
      num9 = (float) ((double) num2 * (double) num5 - (double) num3 * (double) num4);
      num10 = (float) ((double) num1 * (double) num5 - (double) num2 * (double) num4);
      if ((double) num9 < 0.0)
      {
        num9 = 0.0f;
        num10 = num5;
        num8 = num3;
      }
      else if ((double) num9 > (double) num7)
      {
        num9 = num7;
        num10 = num5 + num2;
        num8 = num3;
      }
    }
    if ((double) num10 < 0.0)
    {
      num10 = 0.0f;
      if (-(double) num4 < 0.0)
        num9 = 0.0f;
      else if (-(double) num4 > (double) num1)
      {
        num9 = num7;
      }
      else
      {
        num9 = -num4;
        num7 = num1;
      }
    }
    else if ((double) num10 > (double) num8)
    {
      num10 = num8;
      if (-(double) num4 + (double) num2 < 0.0)
        num9 = 0.0f;
      else if (-(double) num4 + (double) num2 > (double) num1)
      {
        num9 = num7;
      }
      else
      {
        num9 = -num4 + num2;
        num7 = num1;
      }
    }
    float num11 = (double) Math.Abs(num9) < 9.9999999747524271E-07 ? 0.0f : num9 / num7;
    float num12 = (double) Math.Abs(num10) < 9.9999999747524271E-07 ? 0.0f : num10 / num8;
    return (rhs + num11 * vector3_1 - num12 * vector3_2).sqrMagnitude;
  }

  public static float SqrDistanceXZ(Vector3 a, Vector3 b)
  {
    Vector3 vector3 = a - b;
    return (float) ((double) vector3.x * (double) vector3.x + (double) vector3.z * (double) vector3.z);
  }

  public static long SignedTriangleAreaTimes2XZ(Int3 a, Int3 b, Int3 c)
  {
    return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z);
  }

  public static float SignedTriangleAreaTimes2XZ(Vector3 a, Vector3 b, Vector3 c)
  {
    return (float) (((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z));
  }

  public static bool RightXZ(Vector3 a, Vector3 b, Vector3 p)
  {
    return ((double) b.x - (double) a.x) * ((double) p.z - (double) a.z) - ((double) p.x - (double) a.x) * ((double) b.z - (double) a.z) < -1.4012984643248171E-45;
  }

  public static bool RightXZ(Int3 a, Int3 b, Int3 p)
  {
    return (long) (b.x - a.x) * (long) (p.z - a.z) - (long) (p.x - a.x) * (long) (b.z - a.z) < 0L;
  }

  public static bool RightOrColinear(Vector2 a, Vector2 b, Vector2 p)
  {
    return ((double) b.x - (double) a.x) * ((double) p.y - (double) a.y) - ((double) p.x - (double) a.x) * ((double) b.y - (double) a.y) <= 0.0;
  }

  public static bool RightOrColinear(Int2 a, Int2 b, Int2 p)
  {
    return (long) (b.x - a.x) * (long) (p.y - a.y) - (long) (p.x - a.x) * (long) (b.y - a.y) <= 0L;
  }

  public static bool RightOrColinearXZ(Vector3 a, Vector3 b, Vector3 p)
  {
    return ((double) b.x - (double) a.x) * ((double) p.z - (double) a.z) - ((double) p.x - (double) a.x) * ((double) b.z - (double) a.z) <= 0.0;
  }

  public static bool RightOrColinearXZ(Int3 a, Int3 b, Int3 p)
  {
    return (long) (b.x - a.x) * (long) (p.z - a.z) - (long) (p.x - a.x) * (long) (b.z - a.z) <= 0L;
  }

  public static bool IsClockwiseMarginXZ(Vector3 a, Vector3 b, Vector3 c)
  {
    return ((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z) <= 1.4012984643248171E-45;
  }

  public static bool IsClockwiseXZ(Vector3 a, Vector3 b, Vector3 c)
  {
    return ((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z) < 0.0;
  }

  public static bool IsClockwiseXZ(Int3 a, Int3 b, Int3 c) => VectorMath.RightXZ(a, b, c);

  public static bool IsClockwiseOrColinearXZ(Int3 a, Int3 b, Int3 c)
  {
    return VectorMath.RightOrColinearXZ(a, b, c);
  }

  public static bool IsClockwiseOrColinear(Int2 a, Int2 b, Int2 c)
  {
    return VectorMath.RightOrColinear(a, b, c);
  }

  public static bool IsColinearXZ(Int3 a, Int3 b, Int3 c)
  {
    return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z) == 0L;
  }

  public static bool IsColinearXZ(Vector3 a, Vector3 b, Vector3 c)
  {
    float num = (float) (((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z));
    return (double) num <= 1.0000000116860974E-07 && (double) num >= -1.0000000116860974E-07;
  }

  public static bool IsColinearAlmostXZ(Int3 a, Int3 b, Int3 c)
  {
    long num = (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z);
    return num > -1L && num < 1L;
  }

  public static bool SegmentsIntersect(Int2 start1, Int2 end1, Int2 start2, Int2 end2)
  {
    return VectorMath.RightOrColinear(start1, end1, start2) != VectorMath.RightOrColinear(start1, end1, end2) && VectorMath.RightOrColinear(start2, end2, start1) != VectorMath.RightOrColinear(start2, end2, end1);
  }

  public static bool SegmentsIntersectXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    return VectorMath.RightOrColinearXZ(start1, end1, start2) != VectorMath.RightOrColinearXZ(start1, end1, end2) && VectorMath.RightOrColinearXZ(start2, end2, start1) != VectorMath.RightOrColinearXZ(start2, end2, end1);
  }

  public static bool SegmentsIntersectXZ(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num1 = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
    if ((double) num1 == 0.0)
      return false;
    float num2 = (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x));
    double num3 = (double) vector3_1.x * ((double) start1.z - (double) start2.z) - (double) vector3_1.z * ((double) start1.x - (double) start2.x);
    float num4 = num2 / num1;
    double num5 = (double) num1;
    float num6 = (float) (num3 / num5);
    return (double) num4 >= 0.0 && (double) num4 <= 1.0 && (double) num6 >= 0.0 && (double) num6 <= 1.0;
  }

  public static Vector3 LineDirIntersectionPointXZ(
    Vector3 start1,
    Vector3 dir1,
    Vector3 start2,
    Vector3 dir2)
  {
    float num1 = (float) ((double) dir2.z * (double) dir1.x - (double) dir2.x * (double) dir1.z);
    if ((double) num1 == 0.0)
      return start1;
    float num2 = (float) ((double) dir2.x * ((double) start1.z - (double) start2.z) - (double) dir2.z * ((double) start1.x - (double) start2.x)) / num1;
    return start1 + dir1 * num2;
  }

  public static Vector3 LineDirIntersectionPointXZ(
    Vector3 start1,
    Vector3 dir1,
    Vector3 start2,
    Vector3 dir2,
    out bool intersects)
  {
    float num1 = (float) ((double) dir2.z * (double) dir1.x - (double) dir2.x * (double) dir1.z);
    if ((double) num1 == 0.0)
    {
      intersects = false;
      return start1;
    }
    float num2 = (float) ((double) dir2.x * ((double) start1.z - (double) start2.z) - (double) dir2.z * ((double) start1.x - (double) start2.x)) / num1;
    intersects = true;
    return start1 + dir1 * num2;
  }

  public static bool RaySegmentIntersectXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    Int3 int3_1 = end1 - start1;
    Int3 int3_2 = end2 - start2;
    long num1 = (long) (int3_2.z * int3_1.x - int3_2.x * int3_1.z);
    if (num1 == 0L)
      return false;
    long num2 = (long) (int3_2.x * (start1.z - start2.z) - int3_2.z * (start1.x - start2.x));
    long num3 = (long) (int3_1.x * (start1.z - start2.z) - int3_1.z * (start1.x - start2.x));
    return num2 < 0L ^ num1 < 0L && num3 < 0L ^ num1 < 0L && (num1 < 0L || num3 <= num1) && (num1 >= 0L || num3 > num1);
  }

  public static bool LineIntersectionFactorXZ(
    Int3 start1,
    Int3 end1,
    Int3 start2,
    Int3 end2,
    out float factor1,
    out float factor2)
  {
    Int3 int3_1 = end1 - start1;
    Int3 int3_2 = end2 - start2;
    long num1 = (long) (int3_2.z * int3_1.x - int3_2.x * int3_1.z);
    if (num1 == 0L)
    {
      factor1 = 0.0f;
      factor2 = 0.0f;
      return false;
    }
    long num2 = (long) (int3_2.x * (start1.z - start2.z) - int3_2.z * (start1.x - start2.x));
    long num3 = (long) (int3_1.x * (start1.z - start2.z) - int3_1.z * (start1.x - start2.x));
    factor1 = (float) num2 / (float) num1;
    factor2 = (float) num3 / (float) num1;
    return true;
  }

  public static bool LineIntersectionFactorXZ(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out float factor1,
    out float factor2)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num1 = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
    if ((double) num1 <= 9.9999997473787516E-06 && (double) num1 >= -9.9999997473787516E-06)
    {
      factor1 = 0.0f;
      factor2 = 0.0f;
      return false;
    }
    float num2 = (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x));
    double num3 = (double) vector3_1.x * ((double) start1.z - (double) start2.z) - (double) vector3_1.z * ((double) start1.x - (double) start2.x);
    float num4 = num2 / num1;
    double num5 = (double) num1;
    float num6 = (float) (num3 / num5);
    factor1 = num4;
    factor2 = num6;
    return true;
  }

  public static float LineRayIntersectionFactorXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    Int3 int3_1 = end1 - start1;
    Int3 int3_2 = end2 - start2;
    int num1 = int3_2.z * int3_1.x - int3_2.x * int3_1.z;
    if (num1 == 0)
      return float.NaN;
    int num2 = int3_2.x * (start1.z - start2.z) - int3_2.z * (start1.x - start2.x);
    return (double) (int3_1.x * (start1.z - start2.z) - int3_1.z * (start1.x - start2.x)) / (double) num1 < 0.0 ? float.NaN : (float) num2 / (float) num1;
  }

  public static float LineIntersectionFactorXZ(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
    return (double) num == 0.0 ? -1f : (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x)) / num;
  }

  public static Vector3 LineIntersectionPointXZ(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2)
  {
    return VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2, out bool _);
  }

  public static Vector3 LineIntersectionPointXZ(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out bool intersects)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num1 = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
    if ((double) num1 == 0.0)
    {
      intersects = false;
      return start1;
    }
    float num2 = (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x)) / num1;
    intersects = true;
    return start1 + vector3_1 * num2;
  }

  public static Vector2 LineIntersectionPoint(
    Vector2 start1,
    Vector2 end1,
    Vector2 start2,
    Vector2 end2)
  {
    return VectorMath.LineIntersectionPoint(start1, end1, start2, end2, out bool _);
  }

  public static Vector2 LineIntersectionPoint(
    Vector2 start1,
    Vector2 end1,
    Vector2 start2,
    Vector2 end2,
    out bool intersects)
  {
    Vector2 vector2_1 = end1 - start1;
    Vector2 vector2_2 = end2 - start2;
    float num1 = (float) ((double) vector2_2.y * (double) vector2_1.x - (double) vector2_2.x * (double) vector2_1.y);
    if ((double) num1 == 0.0)
    {
      intersects = false;
      return start1;
    }
    float num2 = (float) ((double) vector2_2.x * ((double) start1.y - (double) start2.y) - (double) vector2_2.y * ((double) start1.x - (double) start2.x)) / num1;
    intersects = true;
    return start1 + vector2_1 * num2;
  }

  public static Vector3 SegmentIntersectionPointXZ(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out bool intersects)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num1 = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
    if ((double) num1 == 0.0)
    {
      intersects = false;
      return start1;
    }
    float num2 = (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x));
    double num3 = (double) vector3_1.x * ((double) start1.z - (double) start2.z) - (double) vector3_1.z * ((double) start1.x - (double) start2.x);
    float num4 = num2 / num1;
    double num5 = (double) num1;
    float num6 = (float) (num3 / num5);
    if ((double) num4 < 0.0 || (double) num4 > 1.0 || (double) num6 < 0.0 || (double) num6 > 1.0)
    {
      intersects = false;
      return start1;
    }
    intersects = true;
    return start1 + vector3_1 * num4;
  }

  public static bool SegmentIntersectsBounds(Bounds bounds, Vector3 a, Vector3 b)
  {
    a -= bounds.center;
    b -= bounds.center;
    Vector3 vector3_1 = (a + b) * 0.5f;
    Vector3 vector3_2 = a - vector3_1;
    Vector3 vector3_3 = new Vector3(Math.Abs(vector3_2.x), Math.Abs(vector3_2.y), Math.Abs(vector3_2.z));
    Vector3 extents = bounds.extents;
    return (double) Math.Abs(vector3_1.x) <= (double) extents.x + (double) vector3_3.x && (double) Math.Abs(vector3_1.y) <= (double) extents.y + (double) vector3_3.y && (double) Math.Abs(vector3_1.z) <= (double) extents.z + (double) vector3_3.z && (double) Math.Abs((float) ((double) vector3_1.y * (double) vector3_2.z - (double) vector3_1.z * (double) vector3_2.y)) <= (double) extents.y * (double) vector3_3.z + (double) extents.z * (double) vector3_3.y && (double) Math.Abs((float) ((double) vector3_1.x * (double) vector3_2.z - (double) vector3_1.z * (double) vector3_2.x)) <= (double) extents.x * (double) vector3_3.z + (double) extents.z * (double) vector3_3.x && (double) Math.Abs((float) ((double) vector3_1.x * (double) vector3_2.y - (double) vector3_1.y * (double) vector3_2.x)) <= (double) extents.x * (double) vector3_3.y + (double) extents.y * (double) vector3_3.x;
  }

  public static bool ReversesFaceOrientations(Matrix4x4 matrix)
  {
    Vector3 lhs = matrix.MultiplyVector(new Vector3(1f, 0.0f, 0.0f));
    Vector3 vector3 = matrix.MultiplyVector(new Vector3(0.0f, 1f, 0.0f));
    Vector3 rhs1 = matrix.MultiplyVector(new Vector3(0.0f, 0.0f, 1f));
    Vector3 rhs2 = vector3;
    return (double) Vector3.Dot(Vector3.Cross(lhs, rhs2), rhs1) < 0.0;
  }

  public static bool ReversesFaceOrientationsXZ(Matrix4x4 matrix)
  {
    Vector3 vector3_1 = matrix.MultiplyVector(new Vector3(1f, 0.0f, 0.0f));
    Vector3 vector3_2 = matrix.MultiplyVector(new Vector3(0.0f, 0.0f, 1f));
    return (double) vector3_1.x * (double) vector3_2.z - (double) vector3_2.x * (double) vector3_1.z < 0.0;
  }
}
