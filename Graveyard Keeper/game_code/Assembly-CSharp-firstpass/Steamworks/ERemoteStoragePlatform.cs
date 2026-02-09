// Decompiled with JetBrains decompiler
// Type: Steamworks.ERemoteStoragePlatform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Flags]
public enum ERemoteStoragePlatform
{
  k_ERemoteStoragePlatformNone = 0,
  k_ERemoteStoragePlatformWindows = 1,
  k_ERemoteStoragePlatformOSX = 2,
  k_ERemoteStoragePlatformPS3 = 4,
  k_ERemoteStoragePlatformLinux = 8,
  k_ERemoteStoragePlatformReserved2 = 16, // 0x00000010
  k_ERemoteStoragePlatformAll = -1, // 0xFFFFFFFF
}
