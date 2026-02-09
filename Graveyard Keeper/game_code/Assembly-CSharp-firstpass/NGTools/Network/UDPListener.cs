// Decompiled with JetBrains decompiler
// Type: NGTools.Network.UDPListener
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Net;
using System.Net.Sockets;

#nullable disable
namespace NGTools.Network;

public class UDPListener : NetworkListener
{
  public UdpClient client;
  public IPEndPoint endPoint;
  public IPEndPoint clientEndPoint;
  public ByteBuffer packetBuffer = new ByteBuffer(6220800);
  public ByteBuffer sendBuffer = new ByteBuffer(6220800);

  public override void StartServer()
  {
    this.endPoint = new IPEndPoint(IPAddress.Any, this.port);
    this.clientEndPoint = new IPEndPoint(IPAddress.Any, this.port);
    this.client = new UdpClient(this.endPoint);
    this.client.EnableBroadcast = true;
    this.client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
    this.client.BeginReceive(new AsyncCallback(this.ReceivedPacket), (object) null);
    InternalNGDebug.LogFile((object) "Started UDPListener.");
  }

  public override void StopServer()
  {
    this.client.Close();
    this.client = (UdpClient) null;
    InternalNGDebug.LogFile((object) "Stopped UDPListener.");
  }

  public void Send(Packet packet)
  {
    this.sendBuffer.Clear();
    this.packetBuffer.Clear();
    packet.Out(this.packetBuffer);
    this.sendBuffer.Append(packet.packetId);
    this.sendBuffer.Append((uint) this.packetBuffer.Length);
    this.sendBuffer.Append(this.packetBuffer);
    byte[] datagram = this.sendBuffer.Flush();
    this.client.BeginSend(datagram, datagram.Length, this.clientEndPoint, new AsyncCallback(this.SendPacket), (object) null);
  }

  public void SendPacket(IAsyncResult ar) => this.client.EndSend(ar);

  public void ReceivedPacket(IAsyncResult ar)
  {
    this.client.EndReceive(ar, ref this.clientEndPoint);
    this.client.BeginReceive(new AsyncCallback(this.ReceivedPacket), (object) null);
  }
}
