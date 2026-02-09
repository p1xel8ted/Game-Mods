// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatMemberStateChange
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Flags]
public enum EChatMemberStateChange
{
  k_EChatMemberStateChangeEntered = 1,
  k_EChatMemberStateChangeLeft = 2,
  k_EChatMemberStateChangeDisconnected = 4,
  k_EChatMemberStateChangeKicked = 8,
  k_EChatMemberStateChangeBanned = 16, // 0x00000010
}
