// Decompiled with JetBrains decompiler
// Type: NGTools.Network.DefaultTcpListener
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

#nullable disable
namespace NGTools.Network;

public class DefaultTcpListener : AbstractTcpListener
{
  public override void StartServer()
  {
    if (this.tcpListener != null)
      return;
    try
    {
      this.tcpListener = new TcpListener(IPAddress.Any, this.port);
      this.tcpListener.Start(this.backLog);
      this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptClient), (object) null);
      InternalNGDebug.LogFile((object) ("Started TCPListener IPAddress.Any:" + this.port.ToString()));
    }
    catch (Exception ex)
    {
      InternalNGDebug.LogException(ex);
    }
  }

  public void AcceptClient(IAsyncResult ar)
  {
    if (this.tcpListener == null)
      return;
    Client client = new Client(this.tcpListener.EndAcceptTcpClient(ar), Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor);
    this.clients.Add(client);
    InternalNGDebug.LogFile((object) ("Accepted Client " + client.tcpClient.Client.RemoteEndPoint?.ToString()));
    this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptClient), (object) null);
  }
}
