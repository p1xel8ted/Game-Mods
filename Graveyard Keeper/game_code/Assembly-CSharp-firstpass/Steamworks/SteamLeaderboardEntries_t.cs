// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamLeaderboardEntries_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct SteamLeaderboardEntries_t(ulong value) : 
  IEquatable<SteamLeaderboardEntries_t>,
  IComparable<SteamLeaderboardEntries_t>
{
  public ulong m_SteamLeaderboardEntries = value;

  public override string ToString() => this.m_SteamLeaderboardEntries.ToString();

  public override bool Equals(object other)
  {
    return other is SteamLeaderboardEntries_t leaderboardEntriesT && this == leaderboardEntriesT;
  }

  public override int GetHashCode() => this.m_SteamLeaderboardEntries.GetHashCode();

  public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
  {
    return (long) x.m_SteamLeaderboardEntries == (long) y.m_SteamLeaderboardEntries;
  }

  public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
  {
    return !(x == y);
  }

  public static explicit operator SteamLeaderboardEntries_t(ulong value)
  {
    return new SteamLeaderboardEntries_t(value);
  }

  public static explicit operator ulong(SteamLeaderboardEntries_t that)
  {
    return that.m_SteamLeaderboardEntries;
  }

  public bool Equals(SteamLeaderboardEntries_t other)
  {
    return (long) this.m_SteamLeaderboardEntries == (long) other.m_SteamLeaderboardEntries;
  }

  public int CompareTo(SteamLeaderboardEntries_t other)
  {
    return this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
  }
}
