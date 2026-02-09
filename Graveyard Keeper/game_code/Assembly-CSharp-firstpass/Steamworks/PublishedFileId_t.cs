// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct PublishedFileId_t(ulong value) : 
  IEquatable<PublishedFileId_t>,
  IComparable<PublishedFileId_t>
{
  public static PublishedFileId_t Invalid = new PublishedFileId_t(0UL);
  public ulong m_PublishedFileId = value;

  public override string ToString() => this.m_PublishedFileId.ToString();

  public override bool Equals(object other)
  {
    return other is PublishedFileId_t publishedFileIdT && this == publishedFileIdT;
  }

  public override int GetHashCode() => this.m_PublishedFileId.GetHashCode();

  public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y)
  {
    return (long) x.m_PublishedFileId == (long) y.m_PublishedFileId;
  }

  public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y) => !(x == y);

  public static explicit operator PublishedFileId_t(ulong value) => new PublishedFileId_t(value);

  public static explicit operator ulong(PublishedFileId_t that) => that.m_PublishedFileId;

  public bool Equals(PublishedFileId_t other)
  {
    return (long) this.m_PublishedFileId == (long) other.m_PublishedFileId;
  }

  public int CompareTo(PublishedFileId_t other)
  {
    return this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);
  }
}
