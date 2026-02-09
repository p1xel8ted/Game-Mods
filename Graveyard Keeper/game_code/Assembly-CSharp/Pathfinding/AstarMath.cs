// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarMath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public static class AstarMath
{
  [Obsolete("Use VectorMath.ClosestPointOnLine instead")]
  public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    return VectorMath.ClosestPointOnLine(lineStart, lineEnd, point);
  }

  [Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
  public static float NearestPointFactor(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    return VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
  }

  [Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
  public static float NearestPointFactor(Int3 lineStart, Int3 lineEnd, Int3 point)
  {
    return VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
  }

  [Obsolete("Use VectorMath.ClosestPointOnLineFactor instead")]
  public static float NearestPointFactor(Int2 lineStart, Int2 lineEnd, Int2 point)
  {
    return VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point);
  }

  [Obsolete("Use VectorMath.ClosestPointOnSegment instead")]
  public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    return VectorMath.ClosestPointOnSegment(lineStart, lineEnd, point);
  }

  [Obsolete("Use VectorMath.ClosestPointOnSegmentXZ instead")]
  public static Vector3 NearestPointStrictXZ(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    return VectorMath.ClosestPointOnSegmentXZ(lineStart, lineEnd, point);
  }

  [Obsolete("Use VectorMath.SqrDistancePointSegmentApproximate instead")]
  public static float DistancePointSegment(int x, int z, int px, int pz, int qx, int qz)
  {
    return VectorMath.SqrDistancePointSegmentApproximate(x, z, px, pz, qx, qz);
  }

  [Obsolete("Use VectorMath.SqrDistancePointSegmentApproximate instead")]
  public static float DistancePointSegment(Int3 a, Int3 b, Int3 p)
  {
    return VectorMath.SqrDistancePointSegmentApproximate(a, b, p);
  }

  [Obsolete("Use VectorMath.SqrDistancePointSegment instead")]
  public static float DistancePointSegmentStrict(Vector3 a, Vector3 b, Vector3 p)
  {
    return VectorMath.SqrDistancePointSegment(a, b, p);
  }

  [Obsolete("Use AstarSplines.CubicBezier instead")]
  public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
  {
    return AstarSplines.CubicBezier(p0, p1, p2, p3, t);
  }

  public static float MapTo(float startMin, float startMax, float value)
  {
    value -= startMin;
    value /= startMax - startMin;
    value = Mathf.Clamp01(value);
    return value;
  }

  public static float MapTo(
    float startMin,
    float startMax,
    float targetMin,
    float targetMax,
    float value)
  {
    value -= startMin;
    value /= startMax - startMin;
    value = Mathf.Clamp01(value);
    value *= targetMax - targetMin;
    value += targetMin;
    return value;
  }

  public static string FormatBytesBinary(int bytes)
  {
    double num = bytes >= 0 ? 1.0 : -1.0;
    bytes = bytes >= 0 ? bytes : -bytes;
    if (bytes < 1024 /*0x0400*/)
      return ((double) bytes * num).ToString() + " bytes";
    if (bytes < 1048576 /*0x100000*/)
      return ((double) bytes / 1024.0 * num).ToString("0.0") + " kb";
    return bytes < 1073741824 /*0x40000000*/ ? ((double) bytes / 1048576.0 * num).ToString("0.0") + " mb" : ((double) bytes / 1073741824.0 * num).ToString("0.0") + " gb";
  }

  public static int Bit(int a, int b) => a >> b & 1;

  public static Color IntToColor(int i, float a)
  {
    return new Color((float) (AstarMath.Bit(i, 1) + AstarMath.Bit(i, 3) * 2 + 1) * 0.25f, (float) (AstarMath.Bit(i, 2) + AstarMath.Bit(i, 4) * 2 + 1) * 0.25f, (float) (AstarMath.Bit(i, 0) + AstarMath.Bit(i, 5) * 2 + 1) * 0.25f, a);
  }

  [Obsolete("Use VectorMath.SqrDistanceXZ instead")]
  public static float SqrMagnitudeXZ(Vector3 a, Vector3 b) => VectorMath.SqrDistanceXZ(a, b);

  [Obsolete("Obsolete", true)]
  public static float DistancePointSegment2(int x, int z, int px, int pz, int qx, int qz)
  {
    throw new NotImplementedException("Obsolete");
  }

  [Obsolete("Obsolete", true)]
  public static float DistancePointSegment2(Vector3 a, Vector3 b, Vector3 p)
  {
    throw new NotImplementedException("Obsolete");
  }

  [Obsolete("Use Int3.GetHashCode instead", true)]
  public static int ComputeVertexHash(int x, int y, int z)
  {
    throw new NotImplementedException("Obsolete");
  }

  [Obsolete("Obsolete", true)]
  public static float Hermite(float start, float end, float value)
  {
    throw new NotImplementedException("Obsolete");
  }

  [Obsolete("Obsolete", true)]
  public static float MapToRange(float targetMin, float targetMax, float value)
  {
    throw new NotImplementedException("Obsolete");
  }

  [Obsolete("Obsolete", true)]
  public static string FormatBytes(int bytes) => throw new NotImplementedException("Obsolete");

  [Obsolete("Obsolete", true)]
  public static float MagnitudeXZ(Vector3 a, Vector3 b)
  {
    throw new NotImplementedException("Obsolete");
  }

  [Obsolete("Obsolete", true)]
  public static int Repeat(int i, int n) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Abs instead", true)]
  public static float Abs(float a) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Abs instead", true)]
  public static int Abs(int a) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Min instead", true)]
  public static float Min(float a, float b) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Min instead", true)]
  public static int Min(int a, int b) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Min instead", true)]
  public static uint Min(uint a, uint b) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Max instead", true)]
  public static float Max(float a, float b) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Max instead", true)]
  public static int Max(int a, int b) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Max instead", true)]
  public static uint Max(uint a, uint b) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Max instead", true)]
  public static ushort Max(ushort a, ushort b) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Sign instead", true)]
  public static float Sign(float a) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Sign instead", true)]
  public static int Sign(int a) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Clamp instead", true)]
  public static float Clamp(float a, float b, float c)
  {
    throw new NotImplementedException("Obsolete");
  }

  [Obsolete("Use Mathf.Clamp instead", true)]
  public static int Clamp(int a, int b, int c) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Clamp01 instead", true)]
  public static float Clamp01(float a) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Clamp01 instead", true)]
  public static int Clamp01(int a) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.Lerp instead", true)]
  public static float Lerp(float a, float b, float t)
  {
    throw new NotImplementedException("Obsolete");
  }

  [Obsolete("Use Mathf.RoundToInt instead", true)]
  public static int RoundToInt(float v) => throw new NotImplementedException("Obsolete");

  [Obsolete("Use Mathf.RoundToInt instead", true)]
  public static int RoundToInt(double v) => throw new NotImplementedException("Obsolete");
}
