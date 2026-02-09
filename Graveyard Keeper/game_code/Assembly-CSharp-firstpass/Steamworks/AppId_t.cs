// Decompiled with JetBrains decompiler
// Type: Steamworks.AppId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct AppId_t(uint value) : IEquatable<AppId_t>, IComparable<AppId_t>
{
  public static AppId_t Invalid = new AppId_t(0U);
  public uint m_AppId = value;

  public override string ToString() => this.m_AppId.ToString();

  public override bool Equals(object other) => other is AppId_t appIdT && this == appIdT;

  public override int GetHashCode() => this.m_AppId.GetHashCode();

  public static bool operator ==(AppId_t x, AppId_t y) => (int) x.m_AppId == (int) y.m_AppId;

  public static bool operator !=(AppId_t x, AppId_t y) => !(x == y);

  public static explicit operator AppId_t(uint value) => new AppId_t(value);

  public static explicit operator uint(AppId_t that) => that.m_AppId;

  public bool Equals(AppId_t other) => (int) this.m_AppId == (int) other.m_AppId;

  public int CompareTo(AppId_t other) => this.m_AppId.CompareTo(other.m_AppId);
}
