// Decompiled with JetBrains decompiler
// Type: Pathfinding.Int2
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding;

public struct Int2(int x, int y)
{
  public int x = x;
  public int y = y;
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

  public long sqrMagnitudeLong => (long) this.x * (long) this.x + (long) this.y * (long) this.y;

  public static Int2 operator +(Int2 a, Int2 b) => new Int2(a.x + b.x, a.y + b.y);

  public static Int2 operator -(Int2 a, Int2 b) => new Int2(a.x - b.x, a.y - b.y);

  public static bool operator ==(Int2 a, Int2 b) => a.x == b.x && a.y == b.y;

  public static bool operator !=(Int2 a, Int2 b) => a.x != b.x || a.y != b.y;

  public static long DotLong(Int2 a, Int2 b) => (long) a.x * (long) b.x + (long) a.y * (long) b.y;

  public override bool Equals(object o)
  {
    if (o == null)
      return false;
    Int2 int2 = (Int2) o;
    return this.x == int2.x && this.y == int2.y;
  }

  public override int GetHashCode() => this.x * 49157 + this.y * 98317;

  [Obsolete("Deprecated becuase it is not used by any part of the A* Pathfinding Project")]
  public static Int2 Rotate(Int2 v, int r)
  {
    r %= 4;
    return new Int2(v.x * Int2.Rotations[r * 4] + v.y * Int2.Rotations[r * 4 + 1], v.x * Int2.Rotations[r * 4 + 2] + v.y * Int2.Rotations[r * 4 + 3]);
  }

  public static Int2 Min(Int2 a, Int2 b) => new Int2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

  public static Int2 Max(Int2 a, Int2 b) => new Int2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));

  public static Int2 FromInt3XZ(Int3 o) => new Int2(o.x, o.z);

  public static Int3 ToInt3XZ(Int2 o) => new Int3(o.x, 0, o.y);

  public override string ToString() => $"({this.x.ToString()}, {this.y.ToString()})";
}
