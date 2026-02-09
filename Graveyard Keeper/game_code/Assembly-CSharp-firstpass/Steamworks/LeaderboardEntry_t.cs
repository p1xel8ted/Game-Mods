// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardEntry_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct LeaderboardEntry_t
{
  public CSteamID m_steamIDUser;
  public int m_nGlobalRank;
  public int m_nScore;
  public int m_cDetails;
  public UGCHandle_t m_hUGC;
}
