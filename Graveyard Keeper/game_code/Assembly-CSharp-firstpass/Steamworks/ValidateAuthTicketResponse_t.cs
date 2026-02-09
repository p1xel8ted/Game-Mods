// Decompiled with JetBrains decompiler
// Type: Steamworks.ValidateAuthTicketResponse_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(143)]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct ValidateAuthTicketResponse_t
{
  public const int k_iCallback = 143;
  public CSteamID m_SteamID;
  public EAuthSessionResponse m_eAuthSessionResponse;
  public CSteamID m_OwnerSteamID;
}
