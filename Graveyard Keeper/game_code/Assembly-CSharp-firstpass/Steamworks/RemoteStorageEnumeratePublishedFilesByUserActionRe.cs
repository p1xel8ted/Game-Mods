// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageEnumeratePublishedFilesByUserActionResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(1328)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct RemoteStorageEnumeratePublishedFilesByUserActionResult_t
{
  public const int k_iCallback = 1328;
  public EResult m_eResult;
  public EWorkshopFileAction m_eAction;
  public int m_nResultsReturned;
  public int m_nTotalResultCount;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
  public PublishedFileId_t[] m_rgPublishedFileId;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
  public uint[] m_rgRTimeUpdated;
}
