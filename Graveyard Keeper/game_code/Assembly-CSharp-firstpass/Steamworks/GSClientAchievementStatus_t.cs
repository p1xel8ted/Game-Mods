// Decompiled with JetBrains decompiler
// Type: Steamworks.GSClientAchievementStatus_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(206)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct GSClientAchievementStatus_t
{
  public const int k_iCallback = 206;
  public ulong m_SteamID;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
  public string m_pchAchievement;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bUnlocked;
}
