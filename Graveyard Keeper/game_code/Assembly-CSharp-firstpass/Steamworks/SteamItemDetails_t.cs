// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamItemDetails_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct SteamItemDetails_t
{
  public SteamItemInstanceID_t m_itemId;
  public SteamItemDef_t m_iDefinition;
  public ushort m_unQuantity;
  public ushort m_unFlags;
}
