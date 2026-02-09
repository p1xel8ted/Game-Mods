// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerActionSetHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct ControllerActionSetHandle_t(ulong value) : 
  IEquatable<ControllerActionSetHandle_t>,
  IComparable<ControllerActionSetHandle_t>
{
  public ulong m_ControllerActionSetHandle = value;

  public override string ToString() => this.m_ControllerActionSetHandle.ToString();

  public override bool Equals(object other)
  {
    return other is ControllerActionSetHandle_t actionSetHandleT && this == actionSetHandleT;
  }

  public override int GetHashCode() => this.m_ControllerActionSetHandle.GetHashCode();

  public static bool operator ==(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y)
  {
    return (long) x.m_ControllerActionSetHandle == (long) y.m_ControllerActionSetHandle;
  }

  public static bool operator !=(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y)
  {
    return !(x == y);
  }

  public static explicit operator ControllerActionSetHandle_t(ulong value)
  {
    return new ControllerActionSetHandle_t(value);
  }

  public static explicit operator ulong(ControllerActionSetHandle_t that)
  {
    return that.m_ControllerActionSetHandle;
  }

  public bool Equals(ControllerActionSetHandle_t other)
  {
    return (long) this.m_ControllerActionSetHandle == (long) other.m_ControllerActionSetHandle;
  }

  public int CompareTo(ControllerActionSetHandle_t other)
  {
    return this.m_ControllerActionSetHandle.CompareTo(other.m_ControllerActionSetHandle);
  }
}
