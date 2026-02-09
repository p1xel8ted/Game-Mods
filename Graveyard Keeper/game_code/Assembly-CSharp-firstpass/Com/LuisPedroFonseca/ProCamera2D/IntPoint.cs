// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.IntPoint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public struct IntPoint(int x, int y) : IEquatable<IntPoint>
{
  public static IntPoint MaxValue = new IntPoint()
  {
    X = int.MaxValue,
    Y = int.MaxValue
  };
  public int X = x;
  public int Y = y;

  public bool IsEqual(IntPoint other) => other.X == this.X && other.Y == this.Y;

  public override string ToString()
  {
    return string.Format($"X: {this.X.ToString()} - Y: {this.Y.ToString()}");
  }

  public bool Equals(IntPoint other) => other.X == this.X && other.Y == this.Y;

  public override int GetHashCode() => (0 * 397 ^ this.X) * 397 ^ this.Y;
}
