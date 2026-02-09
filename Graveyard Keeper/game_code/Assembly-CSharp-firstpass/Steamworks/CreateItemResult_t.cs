// Decompiled with JetBrains decompiler
// Type: Steamworks.CreateItemResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(3403)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct CreateItemResult_t
{
  public const int k_iCallback = 3403;
  public EResult m_eResult;
  public PublishedFileId_t m_nPublishedFileId;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
}
