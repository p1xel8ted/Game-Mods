// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct SteamInventoryResult_t(int value) : 
  IEquatable<SteamInventoryResult_t>,
  IComparable<SteamInventoryResult_t>
{
  public static SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);
  public int m_SteamInventoryResult = value;

  public override string ToString() => this.m_SteamInventoryResult.ToString();

  public override bool Equals(object other)
  {
    return other is SteamInventoryResult_t inventoryResultT && this == inventoryResultT;
  }

  public override int GetHashCode() => this.m_SteamInventoryResult.GetHashCode();

  public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y)
  {
    return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
  }

  public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y) => !(x == y);

  public static explicit operator SteamInventoryResult_t(int value)
  {
    return new SteamInventoryResult_t(value);
  }

  public static explicit operator int(SteamInventoryResult_t that) => that.m_SteamInventoryResult;

  public bool Equals(SteamInventoryResult_t other)
  {
    return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
  }

  public int CompareTo(SteamInventoryResult_t other)
  {
    return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
  }
}
