// Decompiled with JetBrains decompiler
// Type: Steamworks.ClientGameServerDeny_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(113)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct ClientGameServerDeny_t
{
  public const int k_iCallback = 113;
  public uint m_uAppID;
  public uint m_unGameServerIP;
  public ushort m_usGameServerPort;
  public ushort m_bSecure;
  public uint m_uReason;
}
