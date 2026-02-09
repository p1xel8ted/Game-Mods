// Decompiled with JetBrains decompiler
// Type: Steamworks.LobbyEnter_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(504)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct LobbyEnter_t
{
  public const int k_iCallback = 504;
  public ulong m_ulSteamIDLobby;
  public uint m_rgfChatPermissions;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bLocked;
  public uint m_EChatRoomEnterResponse;
}
