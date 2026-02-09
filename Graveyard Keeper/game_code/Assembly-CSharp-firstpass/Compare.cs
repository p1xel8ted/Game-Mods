// Decompiled with JetBrains decompiler
// Type: Compare
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
public static class Compare
{
  public static bool Floats(Compare.CompareType comp, float v1, float v2)
  {
    switch (comp)
    {
      case Compare.CompareType.Equal:
        return (double) Math.Abs(v1 - v2) < 9.9999997473787516E-06;
      case Compare.CompareType.More:
        return (double) v1 > (double) v2;
      case Compare.CompareType.Less:
        return (double) v1 < (double) v2;
      case Compare.CompareType.MoreEqual:
        return (double) v1 >= (double) v2;
      case Compare.CompareType.LessEqual:
        return (double) v1 <= (double) v2;
      case Compare.CompareType.NotEqual:
        return !Compare.Floats(Compare.CompareType.Equal, v1, v2);
      default:
        throw new Exception("Unknown compare type: " + comp.ToString());
    }
  }

  public static bool Integers(Compare.CompareType comp, int v1, int v2)
  {
    switch (comp)
    {
      case Compare.CompareType.Equal:
        return v1 == v2;
      case Compare.CompareType.More:
        return v1 > v2;
      case Compare.CompareType.Less:
        return v1 < v2;
      case Compare.CompareType.MoreEqual:
        return v1 >= v2;
      case Compare.CompareType.LessEqual:
        return v1 <= v2;
      case Compare.CompareType.NotEqual:
        return v1 != v2;
      default:
        throw new Exception("Unknown compare type: " + comp.ToString());
    }
  }

  public enum CompareType
  {
    Equal,
    More,
    Less,
    MoreEqual,
    LessEqual,
    NotEqual,
  }
}
