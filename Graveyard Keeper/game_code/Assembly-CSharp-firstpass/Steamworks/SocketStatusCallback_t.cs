// Decompiled with JetBrains decompiler
// Type: Steamworks.SocketStatusCallback_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(1201)]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct SocketStatusCallback_t
{
  public const int k_iCallback = 1201;
  public SNetSocket_t m_hSocket;
  public SNetListenSocket_t m_hListenSocket;
  public CSteamID m_steamIDRemote;
  public int m_eSNetSocketState;
}
