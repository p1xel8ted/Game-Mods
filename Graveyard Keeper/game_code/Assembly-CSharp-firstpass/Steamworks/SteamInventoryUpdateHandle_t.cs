// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryUpdateHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct SteamInventoryUpdateHandle_t(ulong value) : 
  IEquatable<SteamInventoryUpdateHandle_t>,
  IComparable<SteamInventoryUpdateHandle_t>
{
  public static SteamInventoryUpdateHandle_t Invalid = new SteamInventoryUpdateHandle_t(ulong.MaxValue);
  public ulong m_SteamInventoryUpdateHandle = value;

  public override string ToString() => this.m_SteamInventoryUpdateHandle.ToString();

  public override bool Equals(object other)
  {
    return other is SteamInventoryUpdateHandle_t inventoryUpdateHandleT && this == inventoryUpdateHandleT;
  }

  public override int GetHashCode() => this.m_SteamInventoryUpdateHandle.GetHashCode();

  public static bool operator ==(SteamInventoryUpdateHandle_t x, SteamInventoryUpdateHandle_t y)
  {
    return (long) x.m_SteamInventoryUpdateHandle == (long) y.m_SteamInventoryUpdateHandle;
  }

  public static bool operator !=(SteamInventoryUpdateHandle_t x, SteamInventoryUpdateHandle_t y)
  {
    return !(x == y);
  }

  public static explicit operator SteamInventoryUpdateHandle_t(ulong value)
  {
    return new SteamInventoryUpdateHandle_t(value);
  }

  public static explicit operator ulong(SteamInventoryUpdateHandle_t that)
  {
    return that.m_SteamInventoryUpdateHandle;
  }

  public bool Equals(SteamInventoryUpdateHandle_t other)
  {
    return (long) this.m_SteamInventoryUpdateHandle == (long) other.m_SteamInventoryUpdateHandle;
  }

  public int CompareTo(SteamInventoryUpdateHandle_t other)
  {
    return this.m_SteamInventoryUpdateHandle.CompareTo(other.m_SteamInventoryUpdateHandle);
  }
}
