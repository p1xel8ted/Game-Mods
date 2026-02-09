// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageAppSyncProgress_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(1303)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct RemoteStorageAppSyncProgress_t
{
  public const int k_iCallback = 1303;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
  public string m_rgchCurrentFile;
  public AppId_t m_nAppID;
  public uint m_uBytesTransferredThisChunk;
  public double m_dAppPercentComplete;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bUploading;
}
