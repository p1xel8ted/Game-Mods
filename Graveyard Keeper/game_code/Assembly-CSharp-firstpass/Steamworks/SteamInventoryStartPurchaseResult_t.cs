// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryStartPurchaseResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(4704)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SteamInventoryStartPurchaseResult_t
{
  public const int k_iCallback = 4704;
  public EResult m_result;
  public ulong m_ulOrderID;
  public ulong m_ulTransID;
}
