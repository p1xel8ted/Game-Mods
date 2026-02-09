// Decompiled with JetBrains decompiler
// Type: Steamworks.GSClientGroupStatus_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(208 /*0xD0*/)]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GSClientGroupStatus_t
{
  public const int k_iCallback = 208 /*0xD0*/;
  public CSteamID m_SteamIDUser;
  public CSteamID m_SteamIDGroup;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bMember;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bOfficer;
}
