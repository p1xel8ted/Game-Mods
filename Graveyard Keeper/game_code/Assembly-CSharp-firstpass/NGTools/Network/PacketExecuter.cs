// Decompiled with JetBrains decompiler
// Type: NGTools.Network.PacketExecuter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NGTools.Network;

public class PacketExecuter
{
  public Dictionary<int, Action<Client, Packet>> packets;

  public PacketExecuter() => this.packets = new Dictionary<int, Action<Client, Packet>>();

  public void ExecutePacket(Client sender, Packet packet)
  {
    Action<Client, Packet> action;
    if (!this.packets.TryGetValue(packet.packetId, out action))
      return;
    action(sender, packet);
  }

  public void HandlePacket(int packetId, Action<Client, Packet> callback)
  {
    if (this.packets.ContainsKey(packetId))
      Debug.LogError((object) $"Packet with id \"{packetId.ToString()}\" is already handled by {this.GetType().Name}.");
    this.packets.Add(packetId, callback);
  }

  public void UnhandlePacket(int packetId)
  {
    if (!this.packets.ContainsKey(packetId))
      Debug.LogError((object) $"Packet with id \"{packetId.ToString()}\" is being removed but is not even handled.");
    this.packets.Remove(packetId);
  }
}
