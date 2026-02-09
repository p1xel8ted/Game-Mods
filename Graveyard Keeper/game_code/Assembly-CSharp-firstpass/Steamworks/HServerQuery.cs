// Decompiled with JetBrains decompiler
// Type: Steamworks.HServerQuery
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct HServerQuery(int value) : IEquatable<HServerQuery>, IComparable<HServerQuery>
{
  public static HServerQuery Invalid = new HServerQuery(-1);
  public int m_HServerQuery = value;

  public override string ToString() => this.m_HServerQuery.ToString();

  public override bool Equals(object other)
  {
    return other is HServerQuery hserverQuery && this == hserverQuery;
  }

  public override int GetHashCode() => this.m_HServerQuery.GetHashCode();

  public static bool operator ==(HServerQuery x, HServerQuery y)
  {
    return x.m_HServerQuery == y.m_HServerQuery;
  }

  public static bool operator !=(HServerQuery x, HServerQuery y) => !(x == y);

  public static explicit operator HServerQuery(int value) => new HServerQuery(value);

  public static explicit operator int(HServerQuery that) => that.m_HServerQuery;

  public bool Equals(HServerQuery other) => this.m_HServerQuery == other.m_HServerQuery;

  public int CompareTo(HServerQuery other) => this.m_HServerQuery.CompareTo(other.m_HServerQuery);
}
