// Decompiled with JetBrains decompiler
// Type: NGTools.Network.ServerHasDisconnectedPacket
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

#nullable disable
namespace NGTools.Network;

[PacketLinkTo(1, false)]
public sealed class ServerHasDisconnectedPacket : Packet
{
  public ServerHasDisconnectedPacket(ByteBuffer buffer)
    : base(buffer)
  {
  }

  public ServerHasDisconnectedPacket()
  {
  }
}
