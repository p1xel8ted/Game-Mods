// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerAnalogActionHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct ControllerAnalogActionHandle_t(ulong value) : 
  IEquatable<ControllerAnalogActionHandle_t>,
  IComparable<ControllerAnalogActionHandle_t>
{
  public ulong m_ControllerAnalogActionHandle = value;

  public override string ToString() => this.m_ControllerAnalogActionHandle.ToString();

  public override bool Equals(object other)
  {
    return other is ControllerAnalogActionHandle_t analogActionHandleT && this == analogActionHandleT;
  }

  public override int GetHashCode() => this.m_ControllerAnalogActionHandle.GetHashCode();

  public static bool operator ==(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y)
  {
    return (long) x.m_ControllerAnalogActionHandle == (long) y.m_ControllerAnalogActionHandle;
  }

  public static bool operator !=(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y)
  {
    return !(x == y);
  }

  public static explicit operator ControllerAnalogActionHandle_t(ulong value)
  {
    return new ControllerAnalogActionHandle_t(value);
  }

  public static explicit operator ulong(ControllerAnalogActionHandle_t that)
  {
    return that.m_ControllerAnalogActionHandle;
  }

  public bool Equals(ControllerAnalogActionHandle_t other)
  {
    return (long) this.m_ControllerAnalogActionHandle == (long) other.m_ControllerAnalogActionHandle;
  }

  public int CompareTo(ControllerAnalogActionHandle_t other)
  {
    return this.m_ControllerAnalogActionHandle.CompareTo(other.m_ControllerAnalogActionHandle);
  }
}
