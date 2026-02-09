// Decompiled with JetBrains decompiler
// Type: Steamworks.ECheckFileSignature
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace Steamworks;

public enum ECheckFileSignature
{
  k_ECheckFileSignatureInvalidSignature,
  k_ECheckFileSignatureValidSignature,
  k_ECheckFileSignatureFileNotFound,
  k_ECheckFileSignatureNoSignaturesFoundForThisApp,
  k_ECheckFileSignatureNoSignaturesFoundForThisFile,
}
