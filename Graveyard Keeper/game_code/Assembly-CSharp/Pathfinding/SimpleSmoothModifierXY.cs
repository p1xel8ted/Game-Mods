// Decompiled with JetBrains decompiler
// Type: Pathfinding.SimpleSmoothModifierXY
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Pathfinding;

[AddComponentMenu("Pathfinding/Modifiers/Simple Smooth XY")]
[RequireComponent(typeof (Seeker))]
[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_simple_smooth_modifier.php")]
[Serializable]
public class SimpleSmoothModifierXY : MonoModifier
{
  public SimpleSmoothModifierXY.SmoothType smoothType;
  [Tooltip("The number of times to subdivide (divide in half) the path segments. [0...inf] (recommended [1...10])")]
  public int subdivisions = 2;
  [Tooltip("Number of times to apply smoothing")]
  public int iterations = 2;
  [Tooltip("Determines how much smoothing to apply in each smooth iteration. 0.5 usually produces the nicest looking curves")]
  public float strength = 0.5f;
  [Tooltip("Toggle to divide all lines in equal length segments")]
  public bool uniformLength = true;
  [Tooltip("The length of each segment in the smoothed path. A high value yields rough paths and low value yields very smooth paths, but is slower")]
  public float maxSegmentLength = 2f;
  [Tooltip("Length factor of the bezier curves' tangents")]
  public float bezierTangentLength = 0.4f;
  [Tooltip("Offset to apply in each smoothing iteration when using Offset Simple")]
  public float offset = 0.2f;
  [Tooltip("How much to smooth the path. A higher value will give a smoother path, but might take the character far off the optimal path.")]
  public float factor = 0.1f;

  public override int Order => 50;

  public override void Apply(Path p)
  {
    if (p.vectorPath == null)
    {
      Debug.LogWarning((object) "Can't process NULL path (has another modifier logged an error?)");
    }
    else
    {
      List<Vector3> vector3List = (List<Vector3>) null;
      switch (this.smoothType)
      {
        case SimpleSmoothModifierXY.SmoothType.Simple:
          vector3List = this.SmoothSimple(p.vectorPath);
          break;
        case SimpleSmoothModifierXY.SmoothType.Bezier:
          vector3List = this.SmoothBezier(p.vectorPath);
          break;
        case SimpleSmoothModifierXY.SmoothType.OffsetSimple:
          vector3List = this.SmoothOffsetSimple(p.vectorPath);
          break;
        case SimpleSmoothModifierXY.SmoothType.CurvedNonuniform:
          vector3List = this.CurvedNonuniform(p.vectorPath);
          break;
      }
      if (vector3List == p.vectorPath)
        return;
      ListPool<Vector3>.Release(p.vectorPath);
      p.vectorPath = vector3List;
    }
  }

  public List<Vector3> CurvedNonuniform(List<Vector3> path)
  {
    if ((double) this.maxSegmentLength <= 0.0)
    {
      Debug.LogWarning((object) "Max Segment Length is <= 0 which would cause DivByZero-exception or other nasty errors (avoid this)");
      return path;
    }
    int capacity = 0;
    for (int index = 0; index < path.Count - 1; ++index)
    {
      float magnitude = (path[index] - path[index + 1]).magnitude;
      for (float num = 0.0f; (double) num <= (double) magnitude; num += this.maxSegmentLength)
        ++capacity;
    }
    List<Vector3> vector3List = ListPool<Vector3>.Claim(capacity);
    Vector3 vector3_1 = path[1] - path[0];
    Vector3 vector3_2 = vector3_1.normalized;
    for (int index = 0; index < path.Count - 1; ++index)
    {
      vector3_1 = path[index] - path[index + 1];
      float magnitude = vector3_1.magnitude;
      Vector3 vector3_3 = vector3_2;
      Vector3 normalized1;
      if (index >= path.Count - 2)
      {
        vector3_1 = path[index + 1] - path[index];
        normalized1 = vector3_1.normalized;
      }
      else
      {
        vector3_1 = path[index + 2] - path[index + 1];
        Vector3 normalized2 = vector3_1.normalized;
        vector3_1 = path[index] - path[index + 1];
        Vector3 normalized3 = vector3_1.normalized;
        vector3_1 = normalized2 - normalized3;
        normalized1 = vector3_1.normalized;
      }
      Vector3 vector3_4 = normalized1;
      Vector3 tan1 = vector3_3 * magnitude * this.factor;
      Vector3 tan2 = vector3_4 * magnitude * this.factor;
      Vector3 a = path[index];
      Vector3 b = path[index + 1];
      float num1 = 1f / magnitude;
      for (float num2 = 0.0f; (double) num2 <= (double) magnitude; num2 += this.maxSegmentLength)
      {
        float t = num2 * num1;
        vector3List.Add(SimpleSmoothModifierXY.GetPointOnCubic(a, b, tan1, tan2, t));
      }
      vector3_2 = vector3_4;
    }
    vector3List[vector3List.Count - 1] = path[path.Count - 1];
    return vector3List;
  }

  public static Vector3 GetPointOnCubic(
    Vector3 a,
    Vector3 b,
    Vector3 tan1,
    Vector3 tan2,
    float t)
  {
    float num1 = t * t;
    float num2 = num1 * t;
    double num3 = 2.0 * (double) num2 - 3.0 * (double) num1 + 1.0;
    float num4 = (float) (-2.0 * (double) num2 + 3.0 * (double) num1);
    float num5 = num2 - 2f * num1 + t;
    float num6 = num2 - num1;
    Vector3 vector3 = a;
    return (float) num3 * vector3 + num4 * b + num5 * tan1 + num6 * tan2;
  }

  public List<Vector3> SmoothOffsetSimple(List<Vector3> path)
  {
    if (path.Count <= 2 || this.iterations <= 0)
      return path;
    if (this.iterations > 12)
    {
      Debug.LogWarning((object) "A very high iteration count was passed, won't let this one through");
      return path;
    }
    int capacity = (path.Count - 2) * (int) Mathf.Pow(2f, (float) this.iterations) + 2;
    List<Vector3> vector3List1 = ListPool<Vector3>.Claim(capacity);
    List<Vector3> list = ListPool<Vector3>.Claim(capacity);
    for (int index = 0; index < capacity; ++index)
    {
      vector3List1.Add(Vector3.zero);
      list.Add(Vector3.zero);
    }
    for (int index = 0; index < path.Count; ++index)
      vector3List1[index] = path[index];
    for (int p = 0; p < this.iterations; ++p)
    {
      int num = (path.Count - 2) * (int) Mathf.Pow(2f, (float) p) + 2;
      List<Vector3> vector3List2 = vector3List1;
      vector3List1 = list;
      list = vector3List2;
      for (int index = 0; index < num - 1; ++index)
      {
        Vector3 a = list[index];
        Vector3 b = list[index + 1];
        Vector3 normalized = Vector3.Cross(b - a, Vector3.up).normalized;
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        if (index != 0 && !VectorMathXY.IsColinearXY(a, b, list[index - 1]))
        {
          flag3 = true;
          flag1 = VectorMathXY.RightOrColinearXY(a, b, list[index - 1]);
        }
        if (index < num - 1 && !VectorMathXY.IsColinearXY(a, b, list[index + 2]))
        {
          flag4 = true;
          flag2 = VectorMathXY.RightOrColinearXY(a, b, list[index + 2]);
        }
        vector3List1[index * 2] = !flag3 ? a : a + (flag1 ? normalized * this.offset * 1f : -normalized * this.offset * 1f);
        vector3List1[index * 2 + 1] = !flag4 ? b : b + (flag2 ? normalized * this.offset * 1f : -normalized * this.offset * 1f);
      }
      vector3List1[(path.Count - 2) * (int) Mathf.Pow(2f, (float) (p + 1)) + 2 - 1] = list[num - 1];
    }
    ListPool<Vector3>.Release(list);
    return vector3List1;
  }

  public List<Vector3> SmoothSimple(List<Vector3> path)
  {
    if (path.Count < 2)
      return path;
    List<Vector3> vector3List;
    if (this.uniformLength)
    {
      this.maxSegmentLength = Mathf.Max(this.maxSegmentLength, 0.005f);
      float num1 = 0.0f;
      for (int index = 0; index < path.Count - 1; ++index)
        num1 += Vector3.Distance(path[index], path[index + 1]);
      vector3List = ListPool<Vector3>.Claim(Mathf.FloorToInt(num1 / this.maxSegmentLength) + 2);
      float num2 = 0.0f;
      for (int index = 0; index < path.Count - 1; ++index)
      {
        Vector3 a = path[index];
        Vector3 b = path[index + 1];
        float num3;
        for (num3 = Vector3.Distance(a, b); (double) num2 < (double) num3; num2 += this.maxSegmentLength)
          vector3List.Add(Vector3.Lerp(a, b, num2 / num3));
        num2 -= num3;
      }
    }
    else
    {
      this.subdivisions = Mathf.Max(this.subdivisions, 0);
      if (this.subdivisions > 10)
      {
        Debug.LogWarning((object) $"Very large number of subdivisions. Cowardly refusing to subdivide every segment into more than {(1 << this.subdivisions).ToString()} subsegments");
        this.subdivisions = 10;
      }
      int num = 1 << this.subdivisions;
      vector3List = ListPool<Vector3>.Claim((path.Count - 1) * num + 1);
      for (int index1 = 0; index1 < path.Count - 1; ++index1)
      {
        for (int index2 = 0; index2 < num; ++index2)
          vector3List.Add(Vector3.Lerp(path[index1], path[index1 + 1], (float) index2 / (float) num));
      }
    }
    vector3List.Add(path[path.Count - 1]);
    if ((double) this.strength > 0.0)
    {
      for (int index3 = 0; index3 < this.iterations; ++index3)
      {
        Vector3 vector3 = vector3List[0];
        for (int index4 = 1; index4 < vector3List.Count - 1; ++index4)
        {
          Vector3 a = vector3List[index4];
          vector3List[index4] = Vector3.Lerp(a, (vector3 + vector3List[index4 + 1]) / 2f, this.strength);
          vector3 = a;
        }
      }
    }
    return vector3List;
  }

  public List<Vector3> SmoothBezier(List<Vector3> path)
  {
    if (this.subdivisions < 0)
      this.subdivisions = 0;
    int num = 1 << this.subdivisions;
    List<Vector3> vector3List = ListPool<Vector3>.Claim();
    for (int index1 = 0; index1 < path.Count - 1; ++index1)
    {
      Vector3 vector3_1 = index1 != 0 ? path[index1 + 1] - path[index1 - 1] : path[index1 + 1] - path[index1];
      Vector3 vector3_2 = index1 != path.Count - 2 ? path[index1] - path[index1 + 2] : path[index1] - path[index1 + 1];
      Vector3 vector3_3 = vector3_1 * this.bezierTangentLength;
      Vector3 vector3_4 = vector3_2 * this.bezierTangentLength;
      Vector3 p0 = path[index1];
      Vector3 p1 = p0 + vector3_3;
      Vector3 p3 = path[index1 + 1];
      Vector3 p2 = p3 + vector3_4;
      for (int index2 = 0; index2 < num; ++index2)
        vector3List.Add(AstarSplines.CubicBezier(p0, p1, p2, p3, (float) index2 / (float) num));
    }
    vector3List.Add(path[path.Count - 1]);
    return vector3List;
  }

  public enum SmoothType
  {
    Simple,
    Bezier,
    OffsetSimple,
    CurvedNonuniform,
  }
}
