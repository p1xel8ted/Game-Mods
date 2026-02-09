// Decompiled with JetBrains decompiler
// Type: Pathfinding.IntRect
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public struct IntRect(int xmin, int ymin, int xmax, int ymax)
{
  public int xmin = xmin;
  public int ymin = ymin;
  public int xmax = xmax;
  public int ymax = ymax;
  public static int[] Rotations = new int[16 /*0x10*/]
  {
    1,
    0,
    0,
    1,
    0,
    1,
    -1,
    0,
    -1,
    0,
    0,
    -1,
    0,
    -1,
    1,
    0
  };

  public bool Contains(int x, int y)
  {
    return x >= this.xmin && y >= this.ymin && x <= this.xmax && y <= this.ymax;
  }

  public int Width => this.xmax - this.xmin + 1;

  public int Height => this.ymax - this.ymin + 1;

  public bool IsValid() => this.xmin <= this.xmax && this.ymin <= this.ymax;

  public static bool operator ==(IntRect a, IntRect b)
  {
    return a.xmin == b.xmin && a.xmax == b.xmax && a.ymin == b.ymin && a.ymax == b.ymax;
  }

  public static bool operator !=(IntRect a, IntRect b)
  {
    return a.xmin != b.xmin || a.xmax != b.xmax || a.ymin != b.ymin || a.ymax != b.ymax;
  }

  public override bool Equals(object _b)
  {
    IntRect intRect = (IntRect) _b;
    return this.xmin == intRect.xmin && this.xmax == intRect.xmax && this.ymin == intRect.ymin && this.ymax == intRect.ymax;
  }

  public override int GetHashCode()
  {
    return this.xmin * 131071 /*0x01FFFF*/ ^ this.xmax * 3571 ^ this.ymin * 3109 ^ this.ymax * 7;
  }

  public static IntRect Intersection(IntRect a, IntRect b)
  {
    return new IntRect(Math.Max(a.xmin, b.xmin), Math.Max(a.ymin, b.ymin), Math.Min(a.xmax, b.xmax), Math.Min(a.ymax, b.ymax));
  }

  public static bool Intersects(IntRect a, IntRect b)
  {
    return a.xmin <= b.xmax && a.ymin <= b.ymax && a.xmax >= b.xmin && a.ymax >= b.ymin;
  }

  public static IntRect Union(IntRect a, IntRect b)
  {
    return new IntRect(Math.Min(a.xmin, b.xmin), Math.Min(a.ymin, b.ymin), Math.Max(a.xmax, b.xmax), Math.Max(a.ymax, b.ymax));
  }

  public IntRect ExpandToContain(int x, int y)
  {
    return new IntRect(Math.Min(this.xmin, x), Math.Min(this.ymin, y), Math.Max(this.xmax, x), Math.Max(this.ymax, y));
  }

  public IntRect Expand(int range)
  {
    return new IntRect(this.xmin - range, this.ymin - range, this.xmax + range, this.ymax + range);
  }

  public IntRect Rotate(int r)
  {
    int rotation1 = IntRect.Rotations[r * 4];
    int rotation2 = IntRect.Rotations[r * 4 + 1];
    int rotation3 = IntRect.Rotations[r * 4 + 2];
    int rotation4 = IntRect.Rotations[r * 4 + 3];
    int val1_1 = rotation1 * this.xmin + rotation2 * this.ymin;
    int val1_2 = rotation3 * this.xmin + rotation4 * this.ymin;
    int val2_1 = rotation1 * this.xmax + rotation2 * this.ymax;
    int val2_2 = rotation3 * this.xmax + rotation4 * this.ymax;
    return new IntRect(Math.Min(val1_1, val2_1), Math.Min(val1_2, val2_2), Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2));
  }

  public IntRect Offset(Int2 offset)
  {
    return new IntRect(this.xmin + offset.x, this.ymin + offset.y, this.xmax + offset.x, this.ymax + offset.y);
  }

  public IntRect Offset(int x, int y)
  {
    return new IntRect(this.xmin + x, this.ymin + y, this.xmax + x, this.ymax + y);
  }

  public override string ToString()
  {
    return $"[x: {this.xmin.ToString()}...{this.xmax.ToString()}, y: {this.ymin.ToString()}...{this.ymax.ToString()}]";
  }

  public void DebugDraw(Matrix4x4 matrix, Color col)
  {
    Vector3 vector3_1 = matrix.MultiplyPoint3x4(new Vector3((float) this.xmin, 0.0f, (float) this.ymin));
    Vector3 vector3_2 = matrix.MultiplyPoint3x4(new Vector3((float) this.xmin, 0.0f, (float) this.ymax));
    Vector3 vector3_3 = matrix.MultiplyPoint3x4(new Vector3((float) this.xmax, 0.0f, (float) this.ymax));
    Vector3 vector3_4 = matrix.MultiplyPoint3x4(new Vector3((float) this.xmax, 0.0f, (float) this.ymin));
    Debug.DrawLine(vector3_1, vector3_2, col);
    Debug.DrawLine(vector3_2, vector3_3, col);
    Debug.DrawLine(vector3_3, vector3_4, col);
    Debug.DrawLine(vector3_4, vector3_1, col);
  }
}
