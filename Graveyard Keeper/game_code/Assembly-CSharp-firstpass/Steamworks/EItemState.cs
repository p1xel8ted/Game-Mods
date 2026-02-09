// Decompiled with JetBrains decompiler
// Type: Steamworks.EItemState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace Steamworks;

[Flags]
public enum EItemState
{
  k_EItemStateNone = 0,
  k_EItemStateSubscribed = 1,
  k_EItemStateLegacyItem = 2,
  k_EItemStateInstalled = 4,
  k_EItemStateNeedsUpdate = 8,
  k_EItemStateDownloading = 16, // 0x00000010
  k_EItemStateDownloadPending = 32, // 0x00000020
}
