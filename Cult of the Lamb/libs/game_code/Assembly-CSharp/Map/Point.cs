// Decompiled with JetBrains decompiler
// Type: Map.Point
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Map;

[Serializable]
public class Point : IEquatable<Point>
{
  public int x;
  public int y;

  public Point(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  public bool Equals(Point other)
  {
    if (other == null)
      return false;
    if (this == other)
      return true;
    return this.x == other.x && this.y == other.y;
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if (this == obj)
      return true;
    return !(obj.GetType() != this.GetType()) && this.Equals((Point) obj);
  }

  public override int GetHashCode() => this.x * 397 ^ this.y;

  public override string ToString() => $"({this.x}, {this.y})";
}
