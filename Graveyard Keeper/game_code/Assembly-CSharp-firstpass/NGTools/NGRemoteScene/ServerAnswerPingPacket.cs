// Decompiled with JetBrains decompiler
// Type: NGTools.NGRemoteScene.ServerAnswerPingPacket
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NGTools.Network;

#nullable disable
namespace NGTools.NGRemoteScene;

[PacketLinkTo(4, false)]
public sealed class ServerAnswerPingPacket : Packet
{
  public ServerAnswerPingPacket(ByteBuffer buffer)
    : base(buffer)
  {
  }

  public ServerAnswerPingPacket()
  {
  }
}
