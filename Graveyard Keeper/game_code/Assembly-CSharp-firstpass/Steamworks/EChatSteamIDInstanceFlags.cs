// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatSteamIDInstanceFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Flags]
public enum EChatSteamIDInstanceFlags
{
  k_EChatAccountInstanceMask = 4095, // 0x00000FFF
  k_EChatInstanceFlagClan = 524288, // 0x00080000
  k_EChatInstanceFlagLobby = 262144, // 0x00040000
  k_EChatInstanceFlagMMSLobby = 131072, // 0x00020000
}
