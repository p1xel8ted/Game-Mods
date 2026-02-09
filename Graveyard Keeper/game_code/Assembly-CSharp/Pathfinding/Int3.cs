// Decompiled with JetBrains decompiler
// Type: Pathfinding.Int3
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public struct Int3
{
  public int x;
  public int y;
  public int z;
  public const int Precision = 1000;
  public const float FloatPrecision = 1000f;
  public const float PrecisionFactor = 0.001f;
  public static Int3 _zero = new Int3(0, 0, 0);

  public static Int3 zero => Int3._zero;

  public Int3(Vector3 position)
  {
    this.x = (int) Math.Round((double) position.x * 1000.0);
    this.y = (int) Math.Round((double) position.y * 1000.0);
    this.z = (int) Math.Round((double) position.z * 1000.0);
  }

  public Int3(int _x, int _y, int _z)
  {
    this.x = _x;
    this.y = _y;
    this.z = _z;
  }

  public static bool operator ==(Int3 lhs, Int3 rhs)
  {
    return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
  }

  public static bool operator !=(Int3 lhs, Int3 rhs)
  {
    return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
  }

  public static explicit operator Int3(Vector3 ob)
  {
    return new Int3((int) Math.Round((double) ob.x * 1000.0), (int) Math.Round((double) ob.y * 1000.0), (int) Math.Round((double) ob.z * 1000.0));
  }

  public static explicit operator Vector3(Int3 ob)
  {
    return new Vector3((float) ob.x * (1f / 1000f), (float) ob.y * (1f / 1000f), (float) ob.z * (1f / 1000f));
  }

  public static explicit operator Vector2(Int3 ob)
  {
    return new Vector2((float) ob.x * (1f / 1000f), (float) ob.y * (1f / 1000f));
  }

  public static Int3 operator -(Int3 lhs, Int3 rhs)
  {
    lhs.x -= rhs.x;
    lhs.y -= rhs.y;
    lhs.z -= rhs.z;
    return lhs;
  }

  public static Int3 operator -(Int3 lhs)
  {
    lhs.x = -lhs.x;
    lhs.y = -lhs.y;
    lhs.z = -lhs.z;
    return lhs;
  }

  public static Int3 operator +(Int3 lhs, Int3 rhs)
  {
    lhs.x += rhs.x;
    lhs.y += rhs.y;
    lhs.z += rhs.z;
    return lhs;
  }

  public static Int3 operator *(Int3 lhs, int rhs)
  {
    lhs.x *= rhs;
    lhs.y *= rhs;
    lhs.z *= rhs;
    return lhs;
  }

  public static Int3 operator *(Int3 lhs, float rhs)
  {
    lhs.x = (int) Math.Round((double) lhs.x * (double) rhs);
    lhs.y = (int) Math.Round((double) lhs.y * (double) rhs);
    lhs.z = (int) Math.Round((double) lhs.z * (double) rhs);
    return lhs;
  }

  public static Int3 operator *(Int3 lhs, double rhs)
  {
    lhs.x = (int) Math.Round((double) lhs.x * rhs);
    lhs.y = (int) Math.Round((double) lhs.y * rhs);
    lhs.z = (int) Math.Round((double) lhs.z * rhs);
    return lhs;
  }

  public static Int3 operator *(Int3 lhs, Vector3 rhs)
  {
    lhs.x = (int) Math.Round((double) lhs.x * (double) rhs.x);
    lhs.y = (int) Math.Round((double) lhs.y * (double) rhs.y);
    lhs.z = (int) Math.Round((double) lhs.z * (double) rhs.z);
    return lhs;
  }

  public static Int3 operator /(Int3 lhs, float rhs)
  {
    lhs.x = (int) Math.Round((double) lhs.x / (double) rhs);
    lhs.y = (int) Math.Round((double) lhs.y / (double) rhs);
    lhs.z = (int) Math.Round((double) lhs.z / (double) rhs);
    return lhs;
  }

  public int this[int i]
  {
    get
    {
      if (i == 0)
        return this.x;
      return i != 1 ? this.z : this.y;
    }
    set
    {
      if (i == 0)
        this.x = value;
      else if (i == 1)
        this.y = value;
      else
        this.z = value;
    }
  }

  public static float Angle(Int3 lhs, Int3 rhs)
  {
    double num = (double) Int3.Dot(lhs, rhs) / ((double) lhs.magnitude * (double) rhs.magnitude);
    return (float) Math.Acos(num < -1.0 ? -1.0 : (num > 1.0 ? 1.0 : num));
  }

  public static int Dot(Int3 lhs, Int3 rhs) => lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;

  public static long DotLong(Int3 lhs, Int3 rhs)
  {
    return (long) lhs.x * (long) rhs.x + (long) lhs.y * (long) rhs.y + (long) lhs.z * (long) rhs.z;
  }

  public Int3 Normal2D() => new Int3(this.z, this.y, -this.x);

  public float magnitude
  {
    get
    {
      double x = (double) this.x;
      double y = (double) this.y;
      double z = (double) this.z;
      return (float) Math.Sqrt(x * x + y * y + z * z);
    }
  }

  public int costMagnitude => (int) Math.Round((double) this.magnitude);

  [Obsolete("This property is deprecated. Use magnitude or cast to a Vector3")]
  public float worldMagnitude
  {
    get
    {
      double x = (double) this.x;
      double y = (double) this.y;
      double z = (double) this.z;
      return (float) Math.Sqrt(x * x + y * y + z * z) * (1f / 1000f);
    }
  }

  public float sqrMagnitude
  {
    get
    {
      double x = (double) this.x;
      double y = (double) this.y;
      double z = (double) this.z;
      return (float) (x * x + y * y + z * z);
    }
  }

  public long sqrMagnitudeLong
  {
    get
    {
      long x = (long) this.x;
      long y = (long) this.y;
      long z = (long) this.z;
      return x * x + y * y + z * z;
    }
  }

  public static implicit operator string(Int3 ob) => ob.ToString();

  public override string ToString()
  {
    return $"( {this.x.ToString()}, {this.y.ToString()}, {this.z.ToString()})";
  }

  public override bool Equals(object o)
  {
    if (o == null)
      return false;
    Int3 int3 = (Int3) o;
    return this.x == int3.x && this.y == int3.y && this.z == int3.z;
  }

  public override int GetHashCode() => this.x * 73856093 ^ this.y * 19349663 ^ this.z * 83492791;
}
