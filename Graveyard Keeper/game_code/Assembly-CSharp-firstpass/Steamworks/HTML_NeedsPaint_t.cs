// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_NeedsPaint_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(4502)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct HTML_NeedsPaint_t
{
  public const int k_iCallback = 4502;
  public HHTMLBrowser unBrowserHandle;
  public IntPtr pBGRA;
  public uint unWide;
  public uint unTall;
  public uint unUpdateX;
  public uint unUpdateY;
  public uint unUpdateWide;
  public uint unUpdateTall;
  public uint unScrollX;
  public uint unScrollY;
  public float flPageScale;
  public uint unPageSerial;
}
