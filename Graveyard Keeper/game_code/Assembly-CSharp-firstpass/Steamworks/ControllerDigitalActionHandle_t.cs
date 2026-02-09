// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerDigitalActionHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct ControllerDigitalActionHandle_t(ulong value) : 
  IEquatable<ControllerDigitalActionHandle_t>,
  IComparable<ControllerDigitalActionHandle_t>
{
  public ulong m_ControllerDigitalActionHandle = value;

  public override string ToString() => this.m_ControllerDigitalActionHandle.ToString();

  public override bool Equals(object other)
  {
    return other is ControllerDigitalActionHandle_t digitalActionHandleT && this == digitalActionHandleT;
  }

  public override int GetHashCode() => this.m_ControllerDigitalActionHandle.GetHashCode();

  public static bool operator ==(
    ControllerDigitalActionHandle_t x,
    ControllerDigitalActionHandle_t y)
  {
    return (long) x.m_ControllerDigitalActionHandle == (long) y.m_ControllerDigitalActionHandle;
  }

  public static bool operator !=(
    ControllerDigitalActionHandle_t x,
    ControllerDigitalActionHandle_t y)
  {
    return !(x == y);
  }

  public static explicit operator ControllerDigitalActionHandle_t(ulong value)
  {
    return new ControllerDigitalActionHandle_t(value);
  }

  public static explicit operator ulong(ControllerDigitalActionHandle_t that)
  {
    return that.m_ControllerDigitalActionHandle;
  }

  public bool Equals(ControllerDigitalActionHandle_t other)
  {
    return (long) this.m_ControllerDigitalActionHandle == (long) other.m_ControllerDigitalActionHandle;
  }

  public int CompareTo(ControllerDigitalActionHandle_t other)
  {
    return this.m_ControllerDigitalActionHandle.CompareTo(other.m_ControllerDigitalActionHandle);
  }
}
