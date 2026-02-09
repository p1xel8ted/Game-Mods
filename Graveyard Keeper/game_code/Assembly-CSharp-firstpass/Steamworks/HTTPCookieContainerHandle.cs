// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPCookieContainerHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct HTTPCookieContainerHandle(uint value) : 
  IEquatable<HTTPCookieContainerHandle>,
  IComparable<HTTPCookieContainerHandle>
{
  public static HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);
  public uint m_HTTPCookieContainerHandle = value;

  public override string ToString() => this.m_HTTPCookieContainerHandle.ToString();

  public override bool Equals(object other)
  {
    return other is HTTPCookieContainerHandle cookieContainerHandle && this == cookieContainerHandle;
  }

  public override int GetHashCode() => this.m_HTTPCookieContainerHandle.GetHashCode();

  public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
  {
    return (int) x.m_HTTPCookieContainerHandle == (int) y.m_HTTPCookieContainerHandle;
  }

  public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
  {
    return !(x == y);
  }

  public static explicit operator HTTPCookieContainerHandle(uint value)
  {
    return new HTTPCookieContainerHandle(value);
  }

  public static explicit operator uint(HTTPCookieContainerHandle that)
  {
    return that.m_HTTPCookieContainerHandle;
  }

  public bool Equals(HTTPCookieContainerHandle other)
  {
    return (int) this.m_HTTPCookieContainerHandle == (int) other.m_HTTPCookieContainerHandle;
  }

  public int CompareTo(HTTPCookieContainerHandle other)
  {
    return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
  }
}
