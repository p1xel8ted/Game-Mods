// Decompiled with JetBrains decompiler
// Type: Steamworks.UserAchievementStored_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(1103)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct UserAchievementStored_t
{
  public const int k_iCallback = 1103;
  public ulong m_nGameID;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bGroupAchievement;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
  public string m_rgchAchievementName;
  public uint m_nCurProgress;
  public uint m_nMaxProgress;
}
