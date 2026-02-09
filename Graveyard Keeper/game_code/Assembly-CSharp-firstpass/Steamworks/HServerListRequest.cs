// Decompiled with JetBrains decompiler
// Type: Steamworks.HServerListRequest
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct HServerListRequest(IntPtr value) : IEquatable<HServerListRequest>
{
  public static HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);
  public IntPtr m_HServerListRequest = value;

  public override string ToString() => this.m_HServerListRequest.ToString();

  public override bool Equals(object other)
  {
    return other is HServerListRequest hserverListRequest && this == hserverListRequest;
  }

  public override int GetHashCode() => this.m_HServerListRequest.GetHashCode();

  public static bool operator ==(HServerListRequest x, HServerListRequest y)
  {
    return x.m_HServerListRequest == y.m_HServerListRequest;
  }

  public static bool operator !=(HServerListRequest x, HServerListRequest y) => !(x == y);

  public static explicit operator HServerListRequest(IntPtr value) => new HServerListRequest(value);

  public static explicit operator IntPtr(HServerListRequest that) => that.m_HServerListRequest;

  public bool Equals(HServerListRequest other)
  {
    return this.m_HServerListRequest == other.m_HServerListRequest;
  }
}
