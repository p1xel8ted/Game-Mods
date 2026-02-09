// Decompiled with JetBrains decompiler
// Type: Pathfinding.VectorMathXY
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public static class VectorMathXY
{
  public static float SqrDistanceXY(Vector3 a, Vector3 b)
  {
    Vector3 vector3 = a - b;
    return (float) ((double) vector3.x * (double) vector3.x + (double) vector3.y * (double) vector3.y);
  }

  public static long SignedTriangleAreaTimes2XY(Int3 a, Int3 b, Int3 c)
  {
    return (long) (b.x - a.x) * (long) (c.y - a.y) - (long) (c.x - a.x) * (long) (b.y - a.y);
  }

  public static float SignedTriangleAreaTimes2XY(Vector3 a, Vector3 b, Vector3 c)
  {
    return (float) (((double) b.x - (double) a.x) * ((double) c.y - (double) a.y) - ((double) c.x - (double) a.x) * ((double) b.y - (double) a.y));
  }

  public static bool RightXY(Vector3 a, Vector3 b, Vector3 p)
  {
    return ((double) b.x - (double) a.x) * ((double) p.y - (double) a.y) - ((double) p.x - (double) a.x) * ((double) b.y - (double) a.y) < -1.4012984643248171E-45;
  }

  public static bool RightXY(Int3 a, Int3 b, Int3 p)
  {
    return (long) (b.x - a.x) * (long) (p.y - a.y) - (long) (p.x - a.x) * (long) (b.y - a.y) < 0L;
  }

  public static bool RightOrColinearXY(Vector3 a, Vector3 b, Vector3 p)
  {
    return ((double) b.x - (double) a.x) * ((double) p.y - (double) a.y) - ((double) p.x - (double) a.x) * ((double) b.y - (double) a.y) <= 0.0;
  }

  public static bool RightOrColinearXY(Int3 a, Int3 b, Int3 p)
  {
    return (long) (b.x - a.x) * (long) (p.y - a.y) - (long) (p.x - a.x) * (long) (b.y - a.y) <= 0L;
  }

  public static bool IsClockwiseMarginXY(Vector3 a, Vector3 b, Vector3 c)
  {
    return ((double) b.x - (double) a.x) * ((double) c.y - (double) a.y) - ((double) c.x - (double) a.x) * ((double) b.y - (double) a.y) <= 1.4012984643248171E-45;
  }

  public static bool IsClockwiseXY(Vector3 a, Vector3 b, Vector3 c)
  {
    return ((double) b.x - (double) a.x) * ((double) c.y - (double) a.y) - ((double) c.x - (double) a.x) * ((double) b.y - (double) a.y) < 0.0;
  }

  public static bool IsClockwiseXY(Int3 a, Int3 b, Int3 c) => VectorMathXY.RightXY(a, b, c);

  public static bool IsClockwiseOrColinearXY(Int3 a, Int3 b, Int3 c)
  {
    return VectorMathXY.RightOrColinearXY(a, b, c);
  }

  public static bool IsColinearXY(Int3 a, Int3 b, Int3 c)
  {
    return (long) (b.x - a.x) * (long) (c.y - a.y) - (long) (c.x - a.x) * (long) (b.y - a.y) == 0L;
  }

  public static bool IsColinearXY(Vector3 a, Vector3 b, Vector3 c)
  {
    float num = (float) (((double) b.x - (double) a.x) * ((double) c.y - (double) a.y) - ((double) c.x - (double) a.x) * ((double) b.y - (double) a.y));
    return (double) num <= 1.0000000116860974E-07 && (double) num >= -1.0000000116860974E-07;
  }

  public static bool IsColinearAlmostXY(Int3 a, Int3 b, Int3 c)
  {
    long num = (long) (b.x - a.x) * (long) (c.y - a.y) - (long) (c.x - a.x) * (long) (b.y - a.y);
    return num > -1L && num < 1L;
  }

  public static bool SegmentsIntersectXY(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    return VectorMathXY.RightOrColinearXY(start1, end1, start2) != VectorMathXY.RightOrColinearXY(start1, end1, end2) && VectorMathXY.RightOrColinearXY(start2, end2, start1) != VectorMathXY.RightOrColinearXY(start2, end2, end1);
  }

  public static bool SegmentsIntersectXY(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num1 = (float) ((double) vector3_2.y * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.y);
    if ((double) num1 == 0.0)
      return false;
    float num2 = (float) ((double) vector3_2.x * ((double) start1.y - (double) start2.y) - (double) vector3_2.y * ((double) start1.x - (double) start2.x));
    double num3 = (double) vector3_1.x * ((double) start1.y - (double) start2.y) - (double) vector3_1.y * ((double) start1.x - (double) start2.x);
    float num4 = num2 / num1;
    double num5 = (double) num1;
    float num6 = (float) (num3 / num5);
    return (double) num4 >= 0.0 && (double) num4 <= 1.0 && (double) num6 >= 0.0 && (double) num6 <= 1.0;
  }

  public static Vector3 LineDirIntersectionPointXY(
    Vector3 start1,
    Vector3 dir1,
    Vector3 start2,
    Vector3 dir2)
  {
    float num1 = (float) ((double) dir2.y * (double) dir1.x - (double) dir2.x * (double) dir1.y);
    if ((double) num1 == 0.0)
      return start1;
    float num2 = (float) ((double) dir2.x * ((double) start1.y - (double) start2.y) - (double) dir2.y * ((double) start1.x - (double) start2.x)) / num1;
    return start1 + dir1 * num2;
  }

  public static Vector3 LineDirIntersectionPointXY(
    Vector3 start1,
    Vector3 dir1,
    Vector3 start2,
    Vector3 dir2,
    out bool intersects)
  {
    float num1 = (float) ((double) dir2.y * (double) dir1.x - (double) dir2.x * (double) dir1.y);
    if ((double) num1 == 0.0)
    {
      intersects = false;
      return start1;
    }
    float num2 = (float) ((double) dir2.x * ((double) start1.y - (double) start2.y) - (double) dir2.y * ((double) start1.x - (double) start2.x)) / num1;
    intersects = true;
    return start1 + dir1 * num2;
  }

  public static bool RaySegmentIntersectXY(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    Int3 int3_1 = end1 - start1;
    Int3 int3_2 = end2 - start2;
    long num1 = (long) (int3_2.y * int3_1.x - int3_2.x * int3_1.y);
    if (num1 == 0L)
      return false;
    long num2 = (long) (int3_2.x * (start1.y - start2.y) - int3_2.y * (start1.x - start2.x));
    long num3 = (long) (int3_1.x * (start1.y - start2.y) - int3_1.y * (start1.x - start2.x));
    return num2 < 0L ^ num1 < 0L && num3 < 0L ^ num1 < 0L && (num1 < 0L || num3 <= num1) && (num1 >= 0L || num3 > num1);
  }

  public static bool LineIntersectionFactorXY(
    Int3 start1,
    Int3 end1,
    Int3 start2,
    Int3 end2,
    out float factor1,
    out float factor2)
  {
    Int3 int3_1 = end1 - start1;
    Int3 int3_2 = end2 - start2;
    long num1 = (long) (int3_2.y * int3_1.x - int3_2.x * int3_1.y);
    if (num1 == 0L)
    {
      factor1 = 0.0f;
      factor2 = 0.0f;
      return false;
    }
    long num2 = (long) (int3_2.x * (start1.y - start2.y) - int3_2.y * (start1.x - start2.x));
    long num3 = (long) (int3_1.x * (start1.y - start2.y) - int3_1.y * (start1.x - start2.x));
    factor1 = (float) num2 / (float) num1;
    factor2 = (float) num3 / (float) num1;
    return true;
  }

  public static bool LineIntersectionFactorXY(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out float factor1,
    out float factor2)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num1 = (float) ((double) vector3_2.y * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.y);
    if ((double) num1 <= 9.9999997473787516E-06 && (double) num1 >= -9.9999997473787516E-06)
    {
      factor1 = 0.0f;
      factor2 = 0.0f;
      return false;
    }
    float num2 = (float) ((double) vector3_2.x * ((double) start1.y - (double) start2.y) - (double) vector3_2.y * ((double) start1.x - (double) start2.x));
    double num3 = (double) vector3_1.x * ((double) start1.y - (double) start2.y) - (double) vector3_1.y * ((double) start1.x - (double) start2.x);
    float num4 = num2 / num1;
    double num5 = (double) num1;
    float num6 = (float) (num3 / num5);
    factor1 = num4;
    factor2 = num6;
    return true;
  }

  public static float LineRayIntersectionFactorXY(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
  {
    Int3 int3_1 = end1 - start1;
    Int3 int3_2 = end2 - start2;
    int num1 = int3_2.y * int3_1.x - int3_2.x * int3_1.y;
    if (num1 == 0)
      return float.NaN;
    int num2 = int3_2.x * (start1.y - start2.y) - int3_2.y * (start1.x - start2.x);
    return (double) (int3_1.x * (start1.y - start2.y) - int3_1.y * (start1.x - start2.x)) / (double) num1 < 0.0 ? float.NaN : (float) num2 / (float) num1;
  }

  public static float LineIntersectionFactorXY(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num = (float) ((double) vector3_2.y * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.y);
    return (double) num == 0.0 ? -1f : (float) ((double) vector3_2.x * ((double) start1.y - (double) start2.y) - (double) vector3_2.y * ((double) start1.x - (double) start2.x)) / num;
  }

  public static Vector3 LineIntersectionPointXY(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2)
  {
    return VectorMathXY.LineIntersectionPointXY(start1, end1, start2, end2, out bool _);
  }

  public static Vector3 LineIntersectionPointXY(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out bool intersects)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num1 = (float) ((double) vector3_2.y * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.y);
    if ((double) num1 == 0.0)
    {
      intersects = false;
      return start1;
    }
    float num2 = (float) ((double) vector3_2.x * ((double) start1.y - (double) start2.y) - (double) vector3_2.y * ((double) start1.x - (double) start2.x)) / num1;
    intersects = true;
    return start1 + vector3_1 * num2;
  }

  public static Vector3 SegmentIntersectionPointXY(
    Vector3 start1,
    Vector3 end1,
    Vector3 start2,
    Vector3 end2,
    out bool intersects)
  {
    Vector3 vector3_1 = end1 - start1;
    Vector3 vector3_2 = end2 - start2;
    float num1 = (float) ((double) vector3_2.y * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.y);
    if ((double) num1 == 0.0)
    {
      intersects = false;
      return start1;
    }
    float num2 = (float) ((double) vector3_2.x * ((double) start1.y - (double) start2.y) - (double) vector3_2.y * ((double) start1.x - (double) start2.x));
    double num3 = (double) vector3_1.x * ((double) start1.y - (double) start2.y) - (double) vector3_1.y * ((double) start1.x - (double) start2.x);
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

  public static bool ReversesFaceOrientationsXY(Matrix4x4 matrix)
  {
    Vector3 vector3_1 = matrix.MultiplyVector(new Vector3(1f, 0.0f, 0.0f));
    Vector3 vector3_2 = matrix.MultiplyVector(new Vector3(0.0f, 0.0f, 1f));
    return (double) vector3_1.x * (double) vector3_2.y - (double) vector3_2.x * (double) vector3_1.y < 0.0;
  }
}
