// Decompiled with JetBrains decompiler
// Type: Steamworks.HHTMLBrowser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Serializable]
public struct HHTMLBrowser(uint value) : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
{
  public static HHTMLBrowser Invalid = new HHTMLBrowser(0U);
  public uint m_HHTMLBrowser = value;

  public override string ToString() => this.m_HHTMLBrowser.ToString();

  public override bool Equals(object other)
  {
    return other is HHTMLBrowser hhtmlBrowser && this == hhtmlBrowser;
  }

  public override int GetHashCode() => this.m_HHTMLBrowser.GetHashCode();

  public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y)
  {
    return (int) x.m_HHTMLBrowser == (int) y.m_HHTMLBrowser;
  }

  public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y) => !(x == y);

  public static explicit operator HHTMLBrowser(uint value) => new HHTMLBrowser(value);

  public static explicit operator uint(HHTMLBrowser that) => that.m_HHTMLBrowser;

  public bool Equals(HHTMLBrowser other) => (int) this.m_HHTMLBrowser == (int) other.m_HHTMLBrowser;

  public int CompareTo(HHTMLBrowser other) => this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
}
