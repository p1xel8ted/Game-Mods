// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPRequestCompleted_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(2101)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct HTTPRequestCompleted_t
{
  public const int k_iCallback = 2101;
  public HTTPRequestHandle m_hRequest;
  public ulong m_ulContextValue;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bRequestSuccessful;
  public EHTTPStatusCode m_eStatusCode;
  public uint m_unBodySize;
}
