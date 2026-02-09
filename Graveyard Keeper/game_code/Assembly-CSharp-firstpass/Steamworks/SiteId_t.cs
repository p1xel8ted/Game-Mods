// Decompiled with JetBrains decompiler
// Type: Steamworks.SiteId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct SiteId_t(ulong value) : IEquatable<SiteId_t>, IComparable<SiteId_t>
{
  public static SiteId_t Invalid = new SiteId_t(0UL);
  public ulong m_SiteId = value;

  public override string ToString() => this.m_SiteId.ToString();

  public override bool Equals(object other) => other is SiteId_t siteIdT && this == siteIdT;

  public override int GetHashCode() => this.m_SiteId.GetHashCode();

  public static bool operator ==(SiteId_t x, SiteId_t y) => (long) x.m_SiteId == (long) y.m_SiteId;

  public static bool operator !=(SiteId_t x, SiteId_t y) => !(x == y);

  public static explicit operator SiteId_t(ulong value) => new SiteId_t(value);

  public static explicit operator ulong(SiteId_t that) => that.m_SiteId;

  public bool Equals(SiteId_t other) => (long) this.m_SiteId == (long) other.m_SiteId;

  public int CompareTo(SiteId_t other) => this.m_SiteId.CompareTo(other.m_SiteId);
}
