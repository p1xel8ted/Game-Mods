// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileUpdateHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct PublishedFileUpdateHandle_t(ulong value) : 
  IEquatable<PublishedFileUpdateHandle_t>,
  IComparable<PublishedFileUpdateHandle_t>
{
  public static PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);
  public ulong m_PublishedFileUpdateHandle = value;

  public override string ToString() => this.m_PublishedFileUpdateHandle.ToString();

  public override bool Equals(object other)
  {
    return other is PublishedFileUpdateHandle_t fileUpdateHandleT && this == fileUpdateHandleT;
  }

  public override int GetHashCode() => this.m_PublishedFileUpdateHandle.GetHashCode();

  public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
  {
    return (long) x.m_PublishedFileUpdateHandle == (long) y.m_PublishedFileUpdateHandle;
  }

  public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
  {
    return !(x == y);
  }

  public static explicit operator PublishedFileUpdateHandle_t(ulong value)
  {
    return new PublishedFileUpdateHandle_t(value);
  }

  public static explicit operator ulong(PublishedFileUpdateHandle_t that)
  {
    return that.m_PublishedFileUpdateHandle;
  }

  public bool Equals(PublishedFileUpdateHandle_t other)
  {
    return (long) this.m_PublishedFileUpdateHandle == (long) other.m_PublishedFileUpdateHandle;
  }

  public int CompareTo(PublishedFileUpdateHandle_t other)
  {
    return this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
  }
}
