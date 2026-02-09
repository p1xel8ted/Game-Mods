// Decompiled with JetBrains decompiler
// Type: Steamworks.GameServerChangeRequested_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(332)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct GameServerChangeRequested_t
{
  public const int k_iCallback = 332;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64 /*0x40*/)]
  public string m_rgchServer;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64 /*0x40*/)]
  public string m_rgchPassword;
}
