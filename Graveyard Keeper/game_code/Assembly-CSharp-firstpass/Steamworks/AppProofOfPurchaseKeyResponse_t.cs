// Decompiled with JetBrains decompiler
// Type: Steamworks.AppProofOfPurchaseKeyResponse_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[CallbackIdentity(1021)]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct AppProofOfPurchaseKeyResponse_t
{
  public const int k_iCallback = 1021;
  public EResult m_eResult;
  public uint m_nAppID;
  public uint m_cchKeyLength;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 240 /*0xF0*/)]
  public string m_rgchKey;
}
