// Decompiled with JetBrains decompiler
// Type: Steamworks.EMarketingMessageFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Flags]
public enum EMarketingMessageFlags
{
  k_EMarketingMessageFlagsNone = 0,
  k_EMarketingMessageFlagsHighPriority = 1,
  k_EMarketingMessageFlagsPlatformWindows = 2,
  k_EMarketingMessageFlagsPlatformMac = 4,
  k_EMarketingMessageFlagsPlatformLinux = 8,
  k_EMarketingMessageFlagsPlatformRestrictions = k_EMarketingMessageFlagsPlatformLinux | k_EMarketingMessageFlagsPlatformMac | k_EMarketingMessageFlagsPlatformWindows, // 0x0000000E
}
