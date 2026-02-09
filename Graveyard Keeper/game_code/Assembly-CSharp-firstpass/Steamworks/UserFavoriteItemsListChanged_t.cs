// Decompiled with JetBrains decompiler
// Type: Steamworks.UserFavoriteItemsListChanged_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(3407)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct UserFavoriteItemsListChanged_t
{
  public const int k_iCallback = 3407;
  public PublishedFileId_t m_nPublishedFileId;
  public EResult m_eResult;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bWasAddRequest;
}
