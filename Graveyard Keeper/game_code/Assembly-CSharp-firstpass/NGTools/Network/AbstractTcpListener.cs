// Decompiled with JetBrains decompiler
// Type: NGTools.Network.AbstractTcpListener
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

#nullable disable
namespace NGTools.Network;

public abstract class AbstractTcpListener : NetworkListener
{
  public int backLog = 1;
  [CompilerGenerated]
  public List<Client> \u003Cclients\u003Ek__BackingField;
  public TcpListener tcpListener;
  public List<Packet> delayedPackets;

  public List<Client> clients
  {
    get => this.\u003Cclients\u003Ek__BackingField;
    set => this.\u003Cclients\u003Ek__BackingField = value;
  }

  public virtual void Awake()
  {
    this.clients = new List<Client>();
    this.delayedPackets = new List<Packet>();
  }

  public virtual void Update()
  {
    if (this.tcpListener == null)
      return;
    for (int index = 0; index < this.clients.Count; ++index)
    {
      if (this.DetectClientDisced(this.clients[index]))
      {
        this.clients[index].Close();
        this.clients.RemoveAt(index);
        --index;
      }
      else
        this.clients[index].Write();
    }
    if (this.delayedPackets.Count > 0)
    {
      for (int index = 0; index < this.delayedPackets.Count; ++index)
        this.BroadcastPacket(this.delayedPackets[index]);
      this.delayedPackets.Clear();
    }
    for (int index = 0; index < this.clients.Count; ++index)
      this.clients[index].ExecReceivedCommands(this.server.executer);
  }

  public bool DetectClientDisced(Client client) => !client.tcpClient.Connected;

  public override void StopServer()
  {
    if (this.tcpListener == null)
      return;
    this.BroadcastPacket((Packet) new ServerHasDisconnectedPacket());
    for (int index = 0; index < this.clients.Count; ++index)
    {
      if (this.DetectClientDisced(this.clients[index]))
      {
        this.clients[index].Close();
        this.clients.RemoveAt(index);
        --index;
      }
      else
        this.clients[index].Write();
    }
    this.tcpListener.Server.Close();
    this.tcpListener = (TcpListener) null;
    InternalNGDebug.LogFile((object) "Stopped AbstractTcpListener.");
  }

  public void BroadcastPacket(Packet packet)
  {
    for (int index = 0; index < this.clients.Count; ++index)
      this.clients[index].AddPacket(packet);
  }

  public void BroadcastPostPacket(Packet packet)
  {
    for (int index = 0; index < this.clients.Count; ++index)
      this.delayedPackets.Add(packet);
  }
}
