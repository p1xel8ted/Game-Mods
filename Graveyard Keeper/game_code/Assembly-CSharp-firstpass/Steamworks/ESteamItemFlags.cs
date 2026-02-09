// Decompiled with JetBrains decompiler
// Type: Steamworks.ESteamItemFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Flags]
public enum ESteamItemFlags
{
  k_ESteamItemNoTrade = 1,
  k_ESteamItemRemoved = 256, // 0x00000100
  k_ESteamItemConsumed = 512, // 0x00000200
}
