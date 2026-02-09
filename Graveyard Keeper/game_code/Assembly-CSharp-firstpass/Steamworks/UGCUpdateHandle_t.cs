// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCUpdateHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct UGCUpdateHandle_t(ulong value) : 
  IEquatable<UGCUpdateHandle_t>,
  IComparable<UGCUpdateHandle_t>
{
  public static UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);
  public ulong m_UGCUpdateHandle = value;

  public override string ToString() => this.m_UGCUpdateHandle.ToString();

  public override bool Equals(object other)
  {
    return other is UGCUpdateHandle_t ugcUpdateHandleT && this == ugcUpdateHandleT;
  }

  public override int GetHashCode() => this.m_UGCUpdateHandle.GetHashCode();

  public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
  {
    return (long) x.m_UGCUpdateHandle == (long) y.m_UGCUpdateHandle;
  }

  public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y) => !(x == y);

  public static explicit operator UGCUpdateHandle_t(ulong value) => new UGCUpdateHandle_t(value);

  public static explicit operator ulong(UGCUpdateHandle_t that) => that.m_UGCUpdateHandle;

  public bool Equals(UGCUpdateHandle_t other)
  {
    return (long) this.m_UGCUpdateHandle == (long) other.m_UGCUpdateHandle;
  }

  public int CompareTo(UGCUpdateHandle_t other)
  {
    return this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
  }
}
