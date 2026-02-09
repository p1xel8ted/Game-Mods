// Decompiled with JetBrains decompiler
// Type: Steamworks.ESteamAPICallFailure
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum ESteamAPICallFailure
{
  k_ESteamAPICallFailureNone = -1, // 0xFFFFFFFF
  k_ESteamAPICallFailureSteamGone = 0,
  k_ESteamAPICallFailureNetworkFailure = 1,
  k_ESteamAPICallFailureInvalidHandle = 2,
  k_ESteamAPICallFailureMismatchedCallback = 3,
}
