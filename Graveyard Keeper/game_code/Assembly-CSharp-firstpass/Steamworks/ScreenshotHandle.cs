// Decompiled with JetBrains decompiler
// Type: Steamworks.ScreenshotHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct ScreenshotHandle(uint value) : 
  IEquatable<ScreenshotHandle>,
  IComparable<ScreenshotHandle>
{
  public static ScreenshotHandle Invalid = new ScreenshotHandle(0U);
  public uint m_ScreenshotHandle = value;

  public override string ToString() => this.m_ScreenshotHandle.ToString();

  public override bool Equals(object other)
  {
    return other is ScreenshotHandle screenshotHandle && this == screenshotHandle;
  }

  public override int GetHashCode() => this.m_ScreenshotHandle.GetHashCode();

  public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y)
  {
    return (int) x.m_ScreenshotHandle == (int) y.m_ScreenshotHandle;
  }

  public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y) => !(x == y);

  public static explicit operator ScreenshotHandle(uint value) => new ScreenshotHandle(value);

  public static explicit operator uint(ScreenshotHandle that) => that.m_ScreenshotHandle;

  public bool Equals(ScreenshotHandle other)
  {
    return (int) this.m_ScreenshotHandle == (int) other.m_ScreenshotHandle;
  }

  public int CompareTo(ScreenshotHandle other)
  {
    return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
  }
}
