// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatEntryType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum EChatEntryType
{
  k_EChatEntryTypeInvalid = 0,
  k_EChatEntryTypeChatMsg = 1,
  k_EChatEntryTypeTyping = 2,
  k_EChatEntryTypeInviteGame = 3,
  k_EChatEntryTypeEmote = 4,
  k_EChatEntryTypeLeftConversation = 6,
  k_EChatEntryTypeEntered = 7,
  k_EChatEntryTypeWasKicked = 8,
  k_EChatEntryTypeWasBanned = 9,
  k_EChatEntryTypeDisconnected = 10, // 0x0000000A
  k_EChatEntryTypeHistoricalChat = 11, // 0x0000000B
  k_EChatEntryTypeLinkBlocked = 14, // 0x0000000E
}
