// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatRoomEnterResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum EChatRoomEnterResponse
{
  k_EChatRoomEnterResponseSuccess = 1,
  k_EChatRoomEnterResponseDoesntExist = 2,
  k_EChatRoomEnterResponseNotAllowed = 3,
  k_EChatRoomEnterResponseFull = 4,
  k_EChatRoomEnterResponseError = 5,
  k_EChatRoomEnterResponseBanned = 6,
  k_EChatRoomEnterResponseLimited = 7,
  k_EChatRoomEnterResponseClanDisabled = 8,
  k_EChatRoomEnterResponseCommunityBan = 9,
  k_EChatRoomEnterResponseMemberBlockedYou = 10, // 0x0000000A
  k_EChatRoomEnterResponseYouBlockedMember = 11, // 0x0000000B
  k_EChatRoomEnterResponseRatelimitExceeded = 15, // 0x0000000F
}
