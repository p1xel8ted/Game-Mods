// Decompiled with JetBrains decompiler
// Type: Steamworks.EDenyReason
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum EDenyReason
{
  k_EDenyInvalid,
  k_EDenyInvalidVersion,
  k_EDenyGeneric,
  k_EDenyNotLoggedOn,
  k_EDenyNoLicense,
  k_EDenyCheater,
  k_EDenyLoggedInElseWhere,
  k_EDenyUnknownText,
  k_EDenyIncompatibleAnticheat,
  k_EDenyMemoryCorruption,
  k_EDenyIncompatibleSoftware,
  k_EDenySteamConnectionLost,
  k_EDenySteamConnectionError,
  k_EDenySteamResponseTimedOut,
  k_EDenySteamValidationStalled,
  k_EDenySteamOwnerLeftGuestUser,
}
