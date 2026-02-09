// Decompiled with JetBrains decompiler
// Type: RadiusModifier
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Pathfinding/Modifiers/Radius Offset")]
[HelpURL("http://arongranberg.com/astar/docs/class_radius_modifier.php")]
public class RadiusModifier : MonoModifier
{
  public float radius = 1f;
  public float detail = 10f;
  public float[] radi = new float[10];
  public float[] a1 = new float[10];
  public float[] a2 = new float[10];
  public bool[] dir = new bool[10];

  public override int Order => 41;

  public bool CalculateCircleInner(
    Vector3 p1,
    Vector3 p2,
    float r1,
    float r2,
    out float a,
    out float sigma)
  {
    float magnitude = (p1 - p2).magnitude;
    if ((double) r1 + (double) r2 > (double) magnitude)
    {
      a = 0.0f;
      sigma = 0.0f;
      return false;
    }
    a = (float) Math.Acos(((double) r1 + (double) r2) / (double) magnitude);
    sigma = (float) Math.Atan2((double) p2.z - (double) p1.z, (double) p2.x - (double) p1.x);
    return true;
  }

  public bool CalculateCircleOuter(
    Vector3 p1,
    Vector3 p2,
    float r1,
    float r2,
    out float a,
    out float sigma)
  {
    float magnitude = (p1 - p2).magnitude;
    if ((double) Math.Abs(r1 - r2) > (double) magnitude)
    {
      a = 0.0f;
      sigma = 0.0f;
      return false;
    }
    a = (float) Math.Acos(((double) r1 - (double) r2) / (double) magnitude);
    sigma = (float) Math.Atan2((double) p2.z - (double) p1.z, (double) p2.x - (double) p1.x);
    return true;
  }

  public RadiusModifier.TangentType CalculateTangentType(
    Vector3 p1,
    Vector3 p2,
    Vector3 p3,
    Vector3 p4)
  {
    bool flag1 = VectorMath.RightOrColinearXZ(p1, p2, p3);
    bool flag2 = VectorMath.RightOrColinearXZ(p2, p3, p4);
    return (RadiusModifier.TangentType) (1 << (flag1 ? 2 : 0) + (flag2 ? 1 : 0));
  }

  public RadiusModifier.TangentType CalculateTangentTypeSimple(Vector3 p1, Vector3 p2, Vector3 p3)
  {
    bool flag = VectorMath.RightOrColinearXZ(p1, p2, p3);
    return (RadiusModifier.TangentType) (1 << (flag ? 2 : 0) + (flag ? 1 : 0));
  }

  public void DrawCircleSegment(Vector3 p1, float rad, Color col, float start = 0.0f, float end = 6.28318548f)
  {
    Vector3 start1 = new Vector3((float) Math.Cos((double) start), 0.0f, (float) Math.Sin((double) start)) * rad + p1;
    for (float num = start; (double) num < (double) end; num += (float) Math.PI / 100f)
    {
      Vector3 end1 = new Vector3((float) Math.Cos((double) num), 0.0f, (float) Math.Sin((double) num)) * rad + p1;
      Debug.DrawLine(start1, end1, col);
      start1 = end1;
    }
    if ((double) end != 2.0 * Math.PI)
      return;
    Vector3 end2 = new Vector3((float) Math.Cos((double) start), 0.0f, (float) Math.Sin((double) start)) * rad + p1;
    Debug.DrawLine(start1, end2, col);
  }

  public override void Apply(Path p)
  {
    List<Vector3> vectorPath = p.vectorPath;
    List<Vector3> vector3List = this.Apply(vectorPath);
    if (vector3List == vectorPath)
      return;
    ListPool<Vector3>.Release(p.vectorPath);
    p.vectorPath = vector3List;
  }

  public List<Vector3> Apply(List<Vector3> vs)
  {
    if (vs == null || vs.Count < 3)
      return vs;
    if (this.radi.Length < vs.Count)
    {
      this.radi = new float[vs.Count];
      this.a1 = new float[vs.Count];
      this.a2 = new float[vs.Count];
      this.dir = new bool[vs.Count];
    }
    for (int index = 0; index < vs.Count; ++index)
      this.radi[index] = this.radius;
    this.radi[0] = 0.0f;
    this.radi[vs.Count - 1] = 0.0f;
    int num1 = 0;
    for (int index1 = 0; index1 < vs.Count - 1; ++index1)
    {
      ++num1;
      if (num1 > 2 * vs.Count)
      {
        Debug.LogWarning((object) "Could not resolve radiuses, the path is too complex. Try reducing the base radius");
        break;
      }
      RadiusModifier.TangentType tangentType = index1 != 0 ? (index1 != vs.Count - 2 ? this.CalculateTangentType(vs[index1 - 1], vs[index1], vs[index1 + 1], vs[index1 + 2]) : this.CalculateTangentTypeSimple(vs[index1 - 1], vs[index1], vs[index1 + 1])) : this.CalculateTangentTypeSimple(vs[index1], vs[index1 + 1], vs[index1 + 2]);
      Vector3 vector3;
      if ((tangentType & RadiusModifier.TangentType.Inner) != (RadiusModifier.TangentType) 0)
      {
        float a;
        float sigma;
        if (!this.CalculateCircleInner(vs[index1], vs[index1 + 1], this.radi[index1], this.radi[index1 + 1], out a, out sigma))
        {
          vector3 = vs[index1 + 1] - vs[index1];
          float magnitude = vector3.magnitude;
          this.radi[index1] = magnitude * (this.radi[index1] / (this.radi[index1] + this.radi[index1 + 1]));
          this.radi[index1 + 1] = magnitude - this.radi[index1];
          this.radi[index1] *= 0.99f;
          this.radi[index1 + 1] *= 0.99f;
          index1 -= 2;
        }
        else if (tangentType == RadiusModifier.TangentType.InnerRightLeft)
        {
          this.a2[index1] = sigma - a;
          this.a1[index1 + 1] = (float) ((double) sigma - (double) a + 3.1415927410125732);
          this.dir[index1] = true;
        }
        else
        {
          this.a2[index1] = sigma + a;
          this.a1[index1 + 1] = (float) ((double) sigma + (double) a + 3.1415927410125732);
          this.dir[index1] = false;
        }
      }
      else
      {
        float a;
        float sigma;
        if (!this.CalculateCircleOuter(vs[index1], vs[index1 + 1], this.radi[index1], this.radi[index1 + 1], out a, out sigma))
        {
          if (index1 == vs.Count - 2)
          {
            float[] radi = this.radi;
            int index2 = index1;
            vector3 = vs[index1 + 1] - vs[index1];
            double magnitude = (double) vector3.magnitude;
            radi[index2] = (float) magnitude;
            this.radi[index1] *= 0.99f;
            --index1;
          }
          else
          {
            if ((double) this.radi[index1] > (double) this.radi[index1 + 1])
            {
              float[] radi = this.radi;
              int index3 = index1 + 1;
              double num2 = (double) this.radi[index1];
              vector3 = vs[index1 + 1] - vs[index1];
              double magnitude = (double) vector3.magnitude;
              double num3 = num2 - magnitude;
              radi[index3] = (float) num3;
            }
            else
            {
              float[] radi = this.radi;
              int index4 = index1 + 1;
              double num4 = (double) this.radi[index1];
              vector3 = vs[index1 + 1] - vs[index1];
              double magnitude = (double) vector3.magnitude;
              double num5 = num4 + magnitude;
              radi[index4] = (float) num5;
            }
            this.radi[index1 + 1] *= 0.99f;
          }
          --index1;
        }
        else if (tangentType == RadiusModifier.TangentType.OuterRight)
        {
          this.a2[index1] = sigma - a;
          this.a1[index1 + 1] = sigma - a;
          this.dir[index1] = true;
        }
        else
        {
          this.a2[index1] = sigma + a;
          this.a1[index1 + 1] = sigma + a;
          this.dir[index1] = false;
        }
      }
    }
    List<Vector3> vector3List = ListPool<Vector3>.Claim();
    vector3List.Add(vs[0]);
    if ((double) this.detail < 1.0)
      this.detail = 1f;
    float num6 = 6.28318548f / this.detail;
    for (int index = 1; index < vs.Count - 1; ++index)
    {
      float num7 = this.a1[index];
      float num8 = this.a2[index];
      float num9 = this.radi[index];
      if (this.dir[index])
      {
        if ((double) num8 < (double) num7)
          num8 += 6.28318548f;
        for (float num10 = num7; (double) num10 < (double) num8; num10 += num6)
          vector3List.Add(new Vector3((float) Math.Cos((double) num10), 0.0f, (float) Math.Sin((double) num10)) * num9 + vs[index]);
      }
      else
      {
        if ((double) num7 < (double) num8)
          num7 += 6.28318548f;
        for (float num11 = num7; (double) num11 > (double) num8; num11 -= num6)
          vector3List.Add(new Vector3((float) Math.Cos((double) num11), 0.0f, (float) Math.Sin((double) num11)) * num9 + vs[index]);
      }
    }
    vector3List.Add(vs[vs.Count - 1]);
    return vector3List;
  }

  [Flags]
  public enum TangentType
  {
    OuterRight = 1,
    InnerRightLeft = 2,
    InnerLeftRight = 4,
    OuterLeft = 8,
    Outer = OuterLeft | OuterRight, // 0x00000009
    Inner = InnerLeftRight | InnerRightLeft, // 0x00000006
  }
}
