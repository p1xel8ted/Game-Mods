// Decompiled with JetBrains decompiler
// Type: Triangulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Triangulator
{
  private List<Vector2> m_points = new List<Vector2>();

  public Triangulator(Vector2[] points)
  {
    this.m_points = new List<Vector2>((IEnumerable<Vector2>) points);
  }

  public int[] Triangulate()
  {
    List<int> intList = new List<int>();
    int count = this.m_points.Count;
    if (count < 3)
      return intList.ToArray();
    int[] V = new int[count];
    if ((double) this.Area() > 0.0)
    {
      for (int index = 0; index < count; ++index)
        V[index] = index;
    }
    else
    {
      for (int index = 0; index < count; ++index)
        V[index] = count - 1 - index;
    }
    int n = count;
    int num1 = 2 * n;
    int num2 = 0;
    int v = n - 1;
    while (n > 2)
    {
      if (num1-- <= 0)
        return intList.ToArray();
      int u = v;
      if (n <= u)
        u = 0;
      v = u + 1;
      if (n <= v)
        v = 0;
      int w = v + 1;
      if (n <= w)
        w = 0;
      if (this.Snip(u, v, w, n, V))
      {
        int num3 = V[u];
        int num4 = V[v];
        int num5 = V[w];
        intList.Add(num3);
        intList.Add(num4);
        intList.Add(num5);
        ++num2;
        int index1 = v;
        for (int index2 = v + 1; index2 < n; ++index2)
        {
          V[index1] = V[index2];
          ++index1;
        }
        --n;
        num1 = 2 * n;
      }
    }
    intList.Reverse();
    return intList.ToArray();
  }

  private float Area()
  {
    int count = this.m_points.Count;
    float num = 0.0f;
    int index1 = count - 1;
    for (int index2 = 0; index2 < count; index1 = index2++)
    {
      Vector2 point1 = this.m_points[index1];
      Vector2 point2 = this.m_points[index2];
      num += (float) ((double) point1.x * (double) point2.y - (double) point2.x * (double) point1.y);
    }
    return num * 0.5f;
  }

  private bool Snip(int u, int v, int w, int n, int[] V)
  {
    Vector2 point1 = this.m_points[V[u]];
    Vector2 point2 = this.m_points[V[v]];
    Vector2 point3 = this.m_points[V[w]];
    if ((double) Mathf.Epsilon > ((double) point2.x - (double) point1.x) * ((double) point3.y - (double) point1.y) - ((double) point2.y - (double) point1.y) * ((double) point3.x - (double) point1.x))
      return false;
    for (int index = 0; index < n; ++index)
    {
      if (index != u && index != v && index != w)
      {
        Vector2 point4 = this.m_points[V[index]];
        if (this.InsideTriangle(point1, point2, point3, point4))
          return false;
      }
    }
    return true;
  }

  private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
  {
    double num1 = (double) C.x - (double) B.x;
    float num2 = C.y - B.y;
    float num3 = A.x - C.x;
    float num4 = A.y - C.y;
    float num5 = B.x - A.x;
    float num6 = B.y - A.y;
    float num7 = P.x - A.x;
    float num8 = P.y - A.y;
    float num9 = P.x - B.x;
    float num10 = P.y - B.y;
    float num11 = P.x - C.x;
    float num12 = P.y - C.y;
    double num13 = (double) num10;
    double num14 = num1 * num13 - (double) num2 * (double) num9;
    float num15 = (float) ((double) num5 * (double) num8 - (double) num6 * (double) num7);
    float num16 = (float) ((double) num3 * (double) num12 - (double) num4 * (double) num11);
    return num14 >= 0.0 && (double) num16 >= 0.0 && (double) num15 >= 0.0;
  }
}
