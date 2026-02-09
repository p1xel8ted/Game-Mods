// Decompiled with JetBrains decompiler
// Type: Steamworks.GSReputation_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(209)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct GSReputation_t
{
  public const int k_iCallback = 209;
  public EResult m_eResult;
  public uint m_unReputationScore;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bBanned;
  public uint m_unBannedIP;
  public ushort m_usBannedPort;
  public ulong m_ulBannedGameID;
  public uint m_unBanExpires;
}
