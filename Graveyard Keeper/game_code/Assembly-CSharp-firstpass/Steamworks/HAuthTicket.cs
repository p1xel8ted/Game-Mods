// Decompiled with JetBrains decompiler
// Type: Steamworks.HAuthTicket
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct HAuthTicket(uint value) : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
{
  public static HAuthTicket Invalid = new HAuthTicket(0U);
  public uint m_HAuthTicket = value;

  public override string ToString() => this.m_HAuthTicket.ToString();

  public override bool Equals(object other)
  {
    return other is HAuthTicket hauthTicket && this == hauthTicket;
  }

  public override int GetHashCode() => this.m_HAuthTicket.GetHashCode();

  public static bool operator ==(HAuthTicket x, HAuthTicket y)
  {
    return (int) x.m_HAuthTicket == (int) y.m_HAuthTicket;
  }

  public static bool operator !=(HAuthTicket x, HAuthTicket y) => !(x == y);

  public static explicit operator HAuthTicket(uint value) => new HAuthTicket(value);

  public static explicit operator uint(HAuthTicket that) => that.m_HAuthTicket;

  public bool Equals(HAuthTicket other) => (int) this.m_HAuthTicket == (int) other.m_HAuthTicket;

  public int CompareTo(HAuthTicket other) => this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
}
