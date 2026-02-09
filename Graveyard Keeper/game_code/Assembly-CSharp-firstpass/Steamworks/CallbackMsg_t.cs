// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackMsg_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct CallbackMsg_t
{
  public int m_hSteamUser;
  public int m_iCallback;
  public IntPtr m_pubParam;
  public int m_cubParam;
}
