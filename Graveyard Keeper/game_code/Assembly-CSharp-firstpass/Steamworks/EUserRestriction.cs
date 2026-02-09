// Decompiled with JetBrains decompiler
// Type: Steamworks.EUserRestriction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum EUserRestriction
{
  k_nUserRestrictionNone = 0,
  k_nUserRestrictionUnknown = 1,
  k_nUserRestrictionAnyChat = 2,
  k_nUserRestrictionVoiceChat = 4,
  k_nUserRestrictionGroupChat = 8,
  k_nUserRestrictionRating = 16, // 0x00000010
  k_nUserRestrictionGameInvites = 32, // 0x00000020
  k_nUserRestrictionTrading = 64, // 0x00000040
}
