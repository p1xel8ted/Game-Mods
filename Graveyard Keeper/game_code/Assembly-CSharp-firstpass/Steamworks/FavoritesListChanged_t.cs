// Decompiled with JetBrains decompiler
// Type: Steamworks.FavoritesListChanged_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(502)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct FavoritesListChanged_t
{
  public const int k_iCallback = 502;
  public uint m_nIP;
  public uint m_nQueryPort;
  public uint m_nConnPort;
  public uint m_nAppID;
  public uint m_nFlags;
  [MarshalAs(UnmanagedType.I1)]
  public bool m_bAdd;
  public AccountID_t m_unAccountId;
}
