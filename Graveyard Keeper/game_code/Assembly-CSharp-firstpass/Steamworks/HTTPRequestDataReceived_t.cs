// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPRequestDataReceived_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(2103)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct HTTPRequestDataReceived_t
{
  public const int k_iCallback = 2103;
  public HTTPRequestHandle m_hRequest;
  public ulong m_ulContextValue;
  public uint m_cOffset;
  public uint m_cBytesReceived;
}
