// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamLeaderboard_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct SteamLeaderboard_t(ulong value) : 
  IEquatable<SteamLeaderboard_t>,
  IComparable<SteamLeaderboard_t>
{
  public ulong m_SteamLeaderboard = value;

  public override string ToString() => this.m_SteamLeaderboard.ToString();

  public override bool Equals(object other)
  {
    return other is SteamLeaderboard_t steamLeaderboardT && this == steamLeaderboardT;
  }

  public override int GetHashCode() => this.m_SteamLeaderboard.GetHashCode();

  public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y)
  {
    return (long) x.m_SteamLeaderboard == (long) y.m_SteamLeaderboard;
  }

  public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y) => !(x == y);

  public static explicit operator SteamLeaderboard_t(ulong value) => new SteamLeaderboard_t(value);

  public static explicit operator ulong(SteamLeaderboard_t that) => that.m_SteamLeaderboard;

  public bool Equals(SteamLeaderboard_t other)
  {
    return (long) this.m_SteamLeaderboard == (long) other.m_SteamLeaderboard;
  }

  public int CompareTo(SteamLeaderboard_t other)
  {
    return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
  }
}
