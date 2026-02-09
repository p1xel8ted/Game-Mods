// Decompiled with JetBrains decompiler
// Type: Steamworks.EItemUpdateStatus
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum EItemUpdateStatus
{
  k_EItemUpdateStatusInvalid,
  k_EItemUpdateStatusPreparingConfig,
  k_EItemUpdateStatusPreparingContent,
  k_EItemUpdateStatusUploadingContent,
  k_EItemUpdateStatusUploadingPreviewFile,
  k_EItemUpdateStatusCommittingChanges,
}
