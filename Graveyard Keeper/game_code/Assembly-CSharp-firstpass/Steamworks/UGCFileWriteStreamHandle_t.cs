// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCFileWriteStreamHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct UGCFileWriteStreamHandle_t(ulong value) : 
  IEquatable<UGCFileWriteStreamHandle_t>,
  IComparable<UGCFileWriteStreamHandle_t>
{
  public static UGCFileWriteStreamHandle_t Invalid = new UGCFileWriteStreamHandle_t(ulong.MaxValue);
  public ulong m_UGCFileWriteStreamHandle = value;

  public override string ToString() => this.m_UGCFileWriteStreamHandle.ToString();

  public override bool Equals(object other)
  {
    return other is UGCFileWriteStreamHandle_t writeStreamHandleT && this == writeStreamHandleT;
  }

  public override int GetHashCode() => this.m_UGCFileWriteStreamHandle.GetHashCode();

  public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
  {
    return (long) x.m_UGCFileWriteStreamHandle == (long) y.m_UGCFileWriteStreamHandle;
  }

  public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
  {
    return !(x == y);
  }

  public static explicit operator UGCFileWriteStreamHandle_t(ulong value)
  {
    return new UGCFileWriteStreamHandle_t(value);
  }

  public static explicit operator ulong(UGCFileWriteStreamHandle_t that)
  {
    return that.m_UGCFileWriteStreamHandle;
  }

  public bool Equals(UGCFileWriteStreamHandle_t other)
  {
    return (long) this.m_UGCFileWriteStreamHandle == (long) other.m_UGCFileWriteStreamHandle;
  }

  public int CompareTo(UGCFileWriteStreamHandle_t other)
  {
    return this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
  }
}
