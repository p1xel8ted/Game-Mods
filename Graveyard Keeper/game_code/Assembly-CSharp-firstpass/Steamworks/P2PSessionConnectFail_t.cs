// Decompiled with JetBrains decompiler
// Type: Steamworks.P2PSessionConnectFail_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(1203)]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct P2PSessionConnectFail_t
{
  public const int k_iCallback = 1203;
  public CSteamID m_steamIDRemote;
  public byte m_eP2PSessionError;
}
