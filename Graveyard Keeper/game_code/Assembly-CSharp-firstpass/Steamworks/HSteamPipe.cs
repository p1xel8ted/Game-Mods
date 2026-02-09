// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamPipe
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct HSteamPipe(int value) : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
{
  public int m_HSteamPipe = value;

  public override string ToString() => this.m_HSteamPipe.ToString();

  public override bool Equals(object other) => other is HSteamPipe hsteamPipe && this == hsteamPipe;

  public override int GetHashCode() => this.m_HSteamPipe.GetHashCode();

  public static bool operator ==(HSteamPipe x, HSteamPipe y) => x.m_HSteamPipe == y.m_HSteamPipe;

  public static bool operator !=(HSteamPipe x, HSteamPipe y) => !(x == y);

  public static explicit operator HSteamPipe(int value) => new HSteamPipe(value);

  public static explicit operator int(HSteamPipe that) => that.m_HSteamPipe;

  public bool Equals(HSteamPipe other) => this.m_HSteamPipe == other.m_HSteamPipe;

  public int CompareTo(HSteamPipe other) => this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
}
