// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[StructLayout(LayoutKind.Sequential)]
public class CCallbackBase
{
  public const byte k_ECallbackFlagsRegistered = 1;
  public const byte k_ECallbackFlagsGameServer = 2;
  public IntPtr m_vfptr;
  public byte m_nCallbackFlags;
  public int m_iCallback;
}
