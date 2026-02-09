// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPRequestHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct HTTPRequestHandle(uint value) : 
  IEquatable<HTTPRequestHandle>,
  IComparable<HTTPRequestHandle>
{
  public static HTTPRequestHandle Invalid = new HTTPRequestHandle(0U);
  public uint m_HTTPRequestHandle = value;

  public override string ToString() => this.m_HTTPRequestHandle.ToString();

  public override bool Equals(object other)
  {
    return other is HTTPRequestHandle httpRequestHandle && this == httpRequestHandle;
  }

  public override int GetHashCode() => this.m_HTTPRequestHandle.GetHashCode();

  public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y)
  {
    return (int) x.m_HTTPRequestHandle == (int) y.m_HTTPRequestHandle;
  }

  public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y) => !(x == y);

  public static explicit operator HTTPRequestHandle(uint value) => new HTTPRequestHandle(value);

  public static explicit operator uint(HTTPRequestHandle that) => that.m_HTTPRequestHandle;

  public bool Equals(HTTPRequestHandle other)
  {
    return (int) this.m_HTTPRequestHandle == (int) other.m_HTTPRequestHandle;
  }

  public int CompareTo(HTTPRequestHandle other)
  {
    return this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
  }
}
