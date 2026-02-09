// Decompiled with JetBrains decompiler
// Type: Steamworks.LobbyKicked_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(512 /*0x0200*/)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct LobbyKicked_t
{
  public const int k_iCallback = 512 /*0x0200*/;
  public ulong m_ulSteamIDLobby;
  public ulong m_ulSteamIDAdmin;
  public byte m_bKickedDueToDisconnect;
}
