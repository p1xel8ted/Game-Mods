// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct UGCHandle_t(ulong value) : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
{
  public static UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);
  public ulong m_UGCHandle = value;

  public override string ToString() => this.m_UGCHandle.ToString();

  public override bool Equals(object other)
  {
    return other is UGCHandle_t ugcHandleT && this == ugcHandleT;
  }

  public override int GetHashCode() => this.m_UGCHandle.GetHashCode();

  public static bool operator ==(UGCHandle_t x, UGCHandle_t y)
  {
    return (long) x.m_UGCHandle == (long) y.m_UGCHandle;
  }

  public static bool operator !=(UGCHandle_t x, UGCHandle_t y) => !(x == y);

  public static explicit operator UGCHandle_t(ulong value) => new UGCHandle_t(value);

  public static explicit operator ulong(UGCHandle_t that) => that.m_UGCHandle;

  public bool Equals(UGCHandle_t other) => (long) this.m_UGCHandle == (long) other.m_UGCHandle;

  public int CompareTo(UGCHandle_t other) => this.m_UGCHandle.CompareTo(other.m_UGCHandle);
}
