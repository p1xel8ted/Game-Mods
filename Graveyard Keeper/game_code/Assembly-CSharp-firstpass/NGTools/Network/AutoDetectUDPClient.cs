// Decompiled with JetBrains decompiler
// Type: NGTools.Network.AutoDetectUDPClient
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

#nullable disable
namespace NGTools.Network;

public sealed class AutoDetectUDPClient
{
  public const float UDPPingInterval = 3f;
  public static byte[] UDPPingMessage = new byte[4]
  {
    (byte) 78,
    (byte) 71,
    (byte) 83,
    (byte) 83
  };
  public static byte[] UDPEndMessage = new byte[2]
  {
    (byte) 69,
    (byte) 78
  };
  public MonoBehaviour behaviour;
  public UdpClient Client;
  public IPEndPoint[] BroadcastEndPoint;
  public float pingInterval;
  public Coroutine pingCoroutine;

  public AutoDetectUDPClient(
    MonoBehaviour behaviour,
    int port,
    int targetPortMin,
    int targetPortMax,
    float pingInterval)
  {
    this.behaviour = behaviour;
    this.pingInterval = pingInterval;
    this.Client = new UdpClient(port);
    this.Client.EnableBroadcast = true;
    this.BroadcastEndPoint = new IPEndPoint[targetPortMax - targetPortMin + 1];
    for (int index = 0; index < this.BroadcastEndPoint.Length; ++index)
      this.BroadcastEndPoint[index] = new IPEndPoint(IPAddress.Broadcast, targetPortMin + index);
    this.pingCoroutine = this.behaviour.StartCoroutine(this.AsyncSendPing());
  }

  public void Stop()
  {
    this.behaviour.StopCoroutine(this.pingCoroutine);
    for (int index = 0; index < this.BroadcastEndPoint.Length; ++index)
      this.Client.Send(AutoDetectUDPClient.UDPEndMessage, AutoDetectUDPClient.UDPEndMessage.Length, this.BroadcastEndPoint[index]);
    this.Client.Close();
  }

  public IEnumerator AsyncSendPing()
  {
    AutoDetectUDPClient autoDetectUdpClient = this;
    AsyncCallback callback = new AsyncCallback(autoDetectUdpClient.SendPresence);
    WaitForSeconds wait = new WaitForSeconds(autoDetectUdpClient.pingInterval);
    while (true)
    {
      for (int index = 0; index < autoDetectUdpClient.BroadcastEndPoint.Length; ++index)
        autoDetectUdpClient.Client.BeginSend(AutoDetectUDPClient.UDPPingMessage, AutoDetectUDPClient.UDPPingMessage.Length, autoDetectUdpClient.BroadcastEndPoint[index], callback, (object) null);
      yield return (object) wait;
    }
  }

  public void SendPresence(IAsyncResult ar) => this.Client.EndSend(ar);
}
