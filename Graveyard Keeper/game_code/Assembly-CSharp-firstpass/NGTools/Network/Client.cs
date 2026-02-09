// Decompiled with JetBrains decompiler
// Type: NGTools.Network.Client
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NGTools.Network;

public class Client
{
  public const int SendBufferCapacity = 1024 /*0x0400*/;
  public const int ReadBufferSize = 16384 /*0x4000*/;
  public const int TempBufferSize = 4048;
  public TcpClient tcpClient;
  public Client.BatchMode batchMode;
  [CompilerGenerated]
  public long \u003CbytesSent\u003Ek__BackingField;
  [CompilerGenerated]
  public long \u003CbytesReceived\u003Ek__BackingField;
  public List<Packet> batchedPackets;
  public string[] batchNames;
  public bool saveSentPackets;
  public List<string> receivedPacketsHistoric;
  public List<Client.ExecutedPacket> sentPacketsHistoric;
  public List<Client.Batch> batchesHistoric;
  public List<Packet> pendingPackets;
  public List<Packet> receivedPackets;
  public ByteBuffer packetBuffer;
  public ByteBuffer receiveBuffer;
  public ByteBuffer fullPacketBuffer;
  public NetworkStream reader;
  public NetworkStream writer;
  public ByteBuffer sendBuffer;
  public ByteBuffer tinySendBuffer;
  public byte[] tempBuffer;
  public object[] packetArgument;
  public int packetId = -1;
  public uint length;
  public Dictionary<int, System.Type> packetTypes;

  public long bytesSent
  {
    get => this.\u003CbytesSent\u003Ek__BackingField;
    set => this.\u003CbytesSent\u003Ek__BackingField = value;
  }

  public long bytesReceived
  {
    get => this.\u003CbytesReceived\u003Ek__BackingField;
    set => this.\u003CbytesReceived\u003Ek__BackingField = value;
  }

  public int PendingPacketsCount => this.pendingPackets.Count;

  public Client(TcpClient tcpClient, bool save = true)
  {
    this.tcpClient = tcpClient;
    this.batchMode = Client.BatchMode.Off;
    this.reader = this.tcpClient.GetStream();
    this.writer = this.reader;
    this.saveSentPackets = save;
    if (this.saveSentPackets)
      this.sentPacketsHistoric = new List<Client.ExecutedPacket>(512 /*0x0200*/);
    this.receivedPacketsHistoric = new List<string>(512 /*0x0200*/);
    this.batchedPackets = new List<Packet>(64 /*0x40*/);
    this.batchesHistoric = new List<Client.Batch>(4);
    this.pendingPackets = new List<Packet>(4);
    this.receivedPackets = new List<Packet>(4);
    this.batchNames = new string[0];
    this.packetBuffer = new ByteBuffer(16384 /*0x4000*/);
    this.receiveBuffer = new ByteBuffer(16384 /*0x4000*/);
    this.fullPacketBuffer = new ByteBuffer(16384 /*0x4000*/);
    this.sendBuffer = new ByteBuffer(1024 /*0x0400*/);
    this.tinySendBuffer = new ByteBuffer(1024 /*0x0400*/);
    this.tempBuffer = new byte[4048];
    this.packetArgument = new object[1]
    {
      (object) this.packetBuffer
    };
    this.packetId = -1;
    this.packetTypes = new Dictionary<int, System.Type>();
    foreach (System.Type type in Utility.EachSubClassesOf(typeof (Packet)))
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (PacketLinkToAttribute), false);
      if (customAttributes.Length != 0)
      {
        if (this.packetTypes.ContainsKey((customAttributes[0] as PacketLinkToAttribute).packetId))
          InternalNGDebug.LogError((object) $"Packet \"{type.FullName}\" shares the same ID as \"{this.packetTypes[(customAttributes[0] as PacketLinkToAttribute).packetId]?.ToString()}\".");
        else
          this.packetTypes.Add((customAttributes[0] as PacketLinkToAttribute).packetId, type);
      }
    }
    if (this.reader.CanRead)
      this.reader.BeginRead(this.tempBuffer, 0, this.tempBuffer.Length, new AsyncCallback(this.ReadCallBack), (object) this);
    else
      Debug.LogError((object) "Client has a non readable NetworkStream.");
  }

  public void Close()
  {
    this.reader.Close();
    this.tcpClient.Close();
  }

  public void ReadCallBack(IAsyncResult ar)
  {
    int length = this.reader.EndRead(ar);
    lock (this.receiveBuffer)
      this.receiveBuffer.Append(this.tempBuffer, 0, length);
    lock (this.fullPacketBuffer)
    {
      this.reader.BeginRead(this.tempBuffer, 0, this.tempBuffer.Length, new AsyncCallback(this.ReadCallBack), (object) this);
      if (this.reader.DataAvailable)
        return;
      this.ExecuteBuffer();
    }
  }

  public void ExecuteBuffer()
  {
    lock (this.receiveBuffer)
    {
      this.fullPacketBuffer.Append(this.receiveBuffer);
      this.bytesReceived += (long) this.receiveBuffer.Length;
      this.receiveBuffer.Clear();
    }
    uint position = (uint) this.fullPacketBuffer.Position;
    while ((uint) this.fullPacketBuffer.Length - position >= 8U)
    {
      if (this.packetId == -1)
      {
        this.packetId = this.fullPacketBuffer.ReadInt32();
        this.length = this.fullPacketBuffer.ReadUInt32();
        position = (uint) this.fullPacketBuffer.Position;
      }
      if ((long) this.length <= (long) (this.fullPacketBuffer.Length - this.fullPacketBuffer.Position))
      {
        InternalNGDebug.LogFile((object) $"X {PacketId.GetPacketName(this.packetId)} ({this.packetId.ToString()}) {this.length.ToString()} B @ {this.fullPacketBuffer.Position.ToString()}/{this.fullPacketBuffer.Length.ToString()}");
        System.Type type;
        if (this.packetTypes.TryGetValue(this.packetId, out type))
        {
          try
          {
            this.fullPacketBuffer.CopyBuffer(this.packetBuffer, (int) position, (int) this.length);
            lock (this.receivedPackets)
            {
              Packet instance = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, (Binder) null, this.packetArgument, (CultureInfo) null) as Packet;
              this.receivedPackets.Add(instance);
              this.receivedPacketsHistoric.Add($"{DateTime.Now.ToString("HH:mm:ss.fff")} {instance?.ToString()}");
            }
          }
          catch (Exception ex)
          {
            InternalNGDebug.LogFileException("Packet parsing failed: Type: " + type?.ToString(), ex);
            this.fullPacketBuffer.Clear();
          }
        }
        else
          InternalNGDebug.LogFile((object) $"Unknown command {PacketId.GetPacketName(this.packetId)} ({this.packetId.ToString()}) of {this.length.ToString()} chars.");
        position += this.length;
        this.fullPacketBuffer.Position = (int) position;
        this.packetId = -1;
        if (this.fullPacketBuffer.Length == this.fullPacketBuffer.Position)
          this.fullPacketBuffer.Clear();
      }
      else
      {
        InternalNGDebug.LogFile((object) $"X... {PacketId.GetPacketName(this.packetId)} ({this.packetId.ToString()}) {this.length.ToString()} B @ {this.fullPacketBuffer.Position.ToString()}/{this.fullPacketBuffer.Length.ToString()}");
        break;
      }
    }
  }

  public void Write()
  {
    if (this.pendingPackets.Count == 0)
      return;
    int num = this.pendingPackets.Count;
    InternalNGDebug.LogFile((object) $"W {num.ToString()} packet(s).");
    for (int index = 0; index < this.pendingPackets.Count; ++index)
    {
      this.tinySendBuffer.Clear();
      this.pendingPackets[index].Out(this.tinySendBuffer);
      string[] strArray = new string[7]
      {
        "W ",
        PacketId.GetPacketName(this.pendingPackets[index].packetId),
        " (",
        null,
        null,
        null,
        null
      };
      num = this.pendingPackets[index].packetId;
      strArray[3] = num.ToString();
      strArray[4] = ") ";
      strArray[5] = ((uint) this.tinySendBuffer.Length).ToString();
      strArray[6] = " B.";
      InternalNGDebug.LogFile((object) string.Concat(strArray));
      this.sendBuffer.Append(this.pendingPackets[index].packetId);
      this.sendBuffer.Append((uint) this.tinySendBuffer.Length);
      this.sendBuffer.Append(this.tinySendBuffer);
    }
    byte[] buffer = this.sendBuffer.Flush();
    this.writer.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(this.WriteCallBack), (object) this);
    this.bytesSent += (long) buffer.Length;
    if (this.saveSentPackets)
    {
      for (int index = 0; index < this.pendingPackets.Count; ++index)
        this.sentPacketsHistoric.Add(new Client.ExecutedPacket(this.pendingPackets[index]));
    }
    this.pendingPackets.Clear();
  }

  public void WriteCallBack(IAsyncResult ar) => this.reader.EndWrite(ar);

  public void ExecReceivedCommands(PacketExecuter executer)
  {
    lock (this.receivedPackets)
    {
      if (this.receivedPackets.Count == 0)
        return;
      for (int index = 0; index < this.receivedPackets.Count; ++index)
      {
        try
        {
          executer.ExecutePacket(this, this.receivedPackets[index]);
        }
        catch (Exception ex)
        {
          InternalNGDebug.LogException(ex);
        }
      }
      this.receivedPackets.Clear();
    }
  }

  public void AddPacket(Packet packet)
  {
    if (this.batchMode == Client.BatchMode.On && packet.isBatchable)
    {
      for (int index = 0; index < this.batchedPackets.Count; ++index)
      {
        if (packet.AggregateInto(this.batchedPackets[index]))
          return;
      }
      this.batchedPackets.Add(packet);
    }
    else
    {
      for (int index = 0; index < this.pendingPackets.Count; ++index)
      {
        if (packet.AggregateInto(this.pendingPackets[index]))
          return;
      }
      this.pendingPackets.Add(packet);
    }
  }

  public void SaveBatch(string name)
  {
    if (this.batchedPackets.Count <= 0)
      return;
    this.batchesHistoric.Add(new Client.Batch(name, this.batchedPackets.ToArray()));
    this.batchNames = new string[this.batchesHistoric.Count];
    for (int index = 0; index < this.batchesHistoric.Count; ++index)
      this.batchNames[index] = $"{this.batchesHistoric[index].name} ({this.batchesHistoric[index].batchedPackets.Length.ToString()})";
  }

  public void ExecuteBatch()
  {
    if (this.batchedPackets.Count <= 0)
      return;
    for (int index = 0; index < this.batchedPackets.Count; ++index)
      this.pendingPackets.Add(this.batchedPackets[index]);
    this.batchedPackets.Clear();
  }

  public void LoadBatch(int i)
  {
    if (0 > i || i >= this.batchesHistoric.Count)
      return;
    this.batchedPackets.Clear();
    for (int index = 0; index < this.batchesHistoric[i].batchedPackets.Length; ++index)
      this.batchedPackets.Add(this.batchesHistoric[i].batchedPackets[index]);
  }

  public override string ToString()
  {
    return $"FPL={this.fullPacketBuffer.Position.ToString()}/{this.fullPacketBuffer.Length.ToString()} BRecv={this.bytesReceived.ToString()} PP={this.pendingPackets.Count.ToString()}";
  }

  public enum BatchMode
  {
    Off,
    On,
  }

  public struct ExecutedPacket
  {
    public string time;
    public Packet packet;

    public ExecutedPacket(Packet packet)
    {
      this.time = DateTime.Now.ToString("HH:mm:ss.fff");
      this.packet = packet;
    }
  }

  public class Batch
  {
    public string name;
    public Packet[] batchedPackets;

    public Batch(string name, Packet[] batch)
    {
      this.name = name;
      this.batchedPackets = batch;
    }
  }
}
