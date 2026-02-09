// Decompiled with JetBrains decompiler
// Type: Steamworks.P2PSessionState_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct P2PSessionState_t
{
  public byte m_bConnectionActive;
  public byte m_bConnecting;
  public byte m_eP2PSessionError;
  public byte m_bUsingRelay;
  public int m_nBytesQueuedForSend;
  public int m_nPacketsQueuedForSend;
  public uint m_nRemoteIP;
  public ushort m_nRemotePort;
}
