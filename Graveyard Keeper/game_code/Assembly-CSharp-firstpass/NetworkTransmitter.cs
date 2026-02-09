// Decompiled with JetBrains decompiler
// Type: NetworkTransmitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

#nullable disable
public class NetworkTransmitter : NetworkBehaviour
{
  public static string LOG_PREFIX = $"[{typeof (NetworkTransmitter).Name}]: ";
  public const int RELIABLE_SEQUENCED_CHANNEL = 0;
  public static int _default_buffer_size = 1024 /*0x0400*/;
  public List<int> serverTransmissionIds = new List<int>();
  public Dictionary<int, NetworkTransmitter.TransmissionData> _client_transmission_data = new Dictionary<int, NetworkTransmitter.TransmissionData>();
  public static int kRpcRpcPrepareToReceiveBytes = 1749299505;
  public static int kRpcRpcReceiveBytes;

  public event UnityAction<int, byte[]> OnDataComepletelySent;

  public event UnityAction<int, byte[]> OnDataFragmentSent;

  public event UnityAction<int, byte[]> OnDataFragmentReceived;

  public event UnityAction<int, byte[]> OnDataCompletelyReceived;

  [Server]
  public void SendBytesToClients(int transmission_id, byte[] data)
  {
    if (!NetworkServer.active)
      Debug.LogWarning((object) "[Server] function 'System.Void NetworkTransmitter::SendBytesToClients(System.Int32,System.Byte[])' called on client");
    else
      this.StartCoroutine(this.SendBytesToClientsRoutine(transmission_id, data));
  }

  [Server]
  public IEnumerator SendBytesToClientsRoutine(int transmission_id, byte[] data)
  {
    if (!NetworkServer.active)
    {
      Debug.LogWarning((object) "[Server] function 'System.Collections.IEnumerator NetworkTransmitter::SendBytesToClientsRoutine(System.Int32,System.Byte[])' called on client");
      return (IEnumerator) null;
    }
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new NetworkTransmitter.\u003CSendBytesToClientsRoutine\u003Ed__19(0)
    {
      \u003C\u003E4__this = this,
      transmission_id = transmission_id,
      data = data
    };
  }

  [ClientRpc(channel = 0)]
  public void RpcPrepareToReceiveBytes(int transmission_id, int expected_size)
  {
    if (this._client_transmission_data.ContainsKey(transmission_id))
      return;
    NetworkTransmitter.TransmissionData transmissionData = new NetworkTransmitter.TransmissionData(new byte[expected_size]);
    this._client_transmission_data.Add(transmission_id, transmissionData);
  }

  [ClientRpc(channel = 0)]
  public void RpcReceiveBytes(int transmission_id, byte[] rec_buffer)
  {
    if (!this._client_transmission_data.ContainsKey(transmission_id))
      return;
    NetworkTransmitter.TransmissionData transmissionData = this._client_transmission_data[transmission_id];
    Array.Copy((Array) rec_buffer, 0, (Array) transmissionData.data, transmissionData.cur_data_index, rec_buffer.Length);
    transmissionData.cur_data_index += rec_buffer.Length;
    if (this.OnDataFragmentReceived != null)
      this.OnDataFragmentReceived(transmission_id, rec_buffer);
    if (transmissionData.cur_data_index < transmissionData.data.Length - 1)
      return;
    Debug.Log((object) $"{NetworkTransmitter.LOG_PREFIX}Completely Received Data at transmission_id={transmission_id.ToString()}");
    this._client_transmission_data.Remove(transmission_id);
    if (this.OnDataCompletelyReceived == null)
      return;
    this.OnDataCompletelyReceived(transmission_id, transmissionData.data);
  }

  static NetworkTransmitter()
  {
    NetworkBehaviour.RegisterRpcDelegate(typeof (NetworkTransmitter), NetworkTransmitter.kRpcRpcPrepareToReceiveBytes, new NetworkBehaviour.CmdDelegate(NetworkTransmitter.InvokeRpcRpcPrepareToReceiveBytes));
    NetworkTransmitter.kRpcRpcReceiveBytes = 1006209153;
    NetworkBehaviour.RegisterRpcDelegate(typeof (NetworkTransmitter), NetworkTransmitter.kRpcRpcReceiveBytes, new NetworkBehaviour.CmdDelegate(NetworkTransmitter.InvokeRpcRpcReceiveBytes));
    NetworkCRC.RegisterBehaviour(nameof (NetworkTransmitter), 0);
  }

  public void UNetVersion()
  {
  }

  public static void InvokeRpcRpcPrepareToReceiveBytes(NetworkBehaviour obj, NetworkReader reader)
  {
    if (!NetworkClient.active)
      Debug.LogError((object) "RPC RpcPrepareToReceiveBytes called on server.");
    else
      ((NetworkTransmitter) obj).RpcPrepareToReceiveBytes((int) reader.ReadPackedUInt32(), (int) reader.ReadPackedUInt32());
  }

  public static void InvokeRpcRpcReceiveBytes(NetworkBehaviour obj, NetworkReader reader)
  {
    if (!NetworkClient.active)
      Debug.LogError((object) "RPC RpcReceiveBytes called on server.");
    else
      ((NetworkTransmitter) obj).RpcReceiveBytes((int) reader.ReadPackedUInt32(), reader.ReadBytesAndSize());
  }

  public void CallRpcPrepareToReceiveBytes(int transmission_id, int expected_size)
  {
    if (!NetworkServer.active)
    {
      Debug.LogError((object) "RPC Function RpcPrepareToReceiveBytes called on client.");
    }
    else
    {
      NetworkWriter writer = new NetworkWriter();
      writer.Write((short) 0);
      writer.Write((short) 2);
      writer.WritePackedUInt32((uint) NetworkTransmitter.kRpcRpcPrepareToReceiveBytes);
      writer.Write(this.GetComponent<NetworkIdentity>().netId);
      writer.WritePackedUInt32((uint) transmission_id);
      writer.WritePackedUInt32((uint) expected_size);
      this.SendRPCInternal(writer, 0, "RpcPrepareToReceiveBytes");
    }
  }

  public void CallRpcReceiveBytes(int transmission_id, byte[] rec_buffer)
  {
    if (!NetworkServer.active)
    {
      Debug.LogError((object) "RPC Function RpcReceiveBytes called on client.");
    }
    else
    {
      NetworkWriter writer = new NetworkWriter();
      writer.Write((short) 0);
      writer.Write((short) 2);
      writer.WritePackedUInt32((uint) NetworkTransmitter.kRpcRpcReceiveBytes);
      writer.Write(this.GetComponent<NetworkIdentity>().netId);
      writer.WritePackedUInt32((uint) transmission_id);
      writer.WriteBytesFull(rec_buffer);
      this.SendRPCInternal(writer, 0, "RpcReceiveBytes");
    }
  }

  public override bool OnSerialize(NetworkWriter writer, bool forceAll)
  {
    bool flag;
    return flag;
  }

  public override void OnDeserialize(NetworkReader reader, bool initialState)
  {
  }

  public override void PreStartClient()
  {
  }

  public class TransmissionData
  {
    public int cur_data_index;
    public byte[] data;

    public TransmissionData(byte[] _data)
    {
      this.cur_data_index = 0;
      this.data = _data;
    }
  }
}
