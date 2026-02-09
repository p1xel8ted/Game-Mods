// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamUser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct HSteamUser(int value) : IEquatable<HSteamUser>, IComparable<HSteamUser>
{
  public int m_HSteamUser = value;

  public override string ToString() => this.m_HSteamUser.ToString();

  public override bool Equals(object other) => other is HSteamUser hsteamUser && this == hsteamUser;

  public override int GetHashCode() => this.m_HSteamUser.GetHashCode();

  public static bool operator ==(HSteamUser x, HSteamUser y) => x.m_HSteamUser == y.m_HSteamUser;

  public static bool operator !=(HSteamUser x, HSteamUser y) => !(x == y);

  public static explicit operator HSteamUser(int value) => new HSteamUser(value);

  public static explicit operator int(HSteamUser that) => that.m_HSteamUser;

  public bool Equals(HSteamUser other) => this.m_HSteamUser == other.m_HSteamUser;

  public int CompareTo(HSteamUser other) => this.m_HSteamUser.CompareTo(other.m_HSteamUser);
}
