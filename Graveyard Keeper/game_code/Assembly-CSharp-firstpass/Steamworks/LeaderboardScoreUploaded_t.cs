// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardScoreUploaded_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(1106)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct LeaderboardScoreUploaded_t
{
  public const int k_iCallback = 1106;
  public byte m_bSuccess;
  public SteamLeaderboard_t m_hSteamLeaderboard;
  public int m_nScore;
  public byte m_bScoreChanged;
  public int m_nGlobalRankNew;
  public int m_nGlobalRankPrevious;
}
