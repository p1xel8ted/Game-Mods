// Decompiled with JetBrains decompiler
// Type: Map.Point
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Map;

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
