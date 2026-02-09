// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPICallCompleted_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(703)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SteamAPICallCompleted_t
{
  public const int k_iCallback = 703;
  public SteamAPICall_t m_hAsyncCall;
  public int m_iCallback;
  public uint m_cubParam;
}
