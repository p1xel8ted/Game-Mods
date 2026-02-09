// Decompiled with JetBrains decompiler
// Type: Steamworks.ControllerHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct ControllerHandle_t(ulong value) : 
  IEquatable<ControllerHandle_t>,
  IComparable<ControllerHandle_t>
{
  public ulong m_ControllerHandle = value;

  public override string ToString() => this.m_ControllerHandle.ToString();

  public override bool Equals(object other)
  {
    return other is ControllerHandle_t controllerHandleT && this == controllerHandleT;
  }

  public override int GetHashCode() => this.m_ControllerHandle.GetHashCode();

  public static bool operator ==(ControllerHandle_t x, ControllerHandle_t y)
  {
    return (long) x.m_ControllerHandle == (long) y.m_ControllerHandle;
  }

  public static bool operator !=(ControllerHandle_t x, ControllerHandle_t y) => !(x == y);

  public static explicit operator ControllerHandle_t(ulong value) => new ControllerHandle_t(value);

  public static explicit operator ulong(ControllerHandle_t that) => that.m_ControllerHandle;

  public bool Equals(ControllerHandle_t other)
  {
    return (long) this.m_ControllerHandle == (long) other.m_ControllerHandle;
  }

  public int CompareTo(ControllerHandle_t other)
  {
    return this.m_ControllerHandle.CompareTo(other.m_ControllerHandle);
  }
}
