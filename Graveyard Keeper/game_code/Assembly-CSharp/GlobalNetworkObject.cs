// Decompiled with JetBrains decompiler
// Type: GlobalNetworkObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

#nullable disable
public class GlobalNetworkObject : NetworkTransmitter
{
  public bool _is_server;
  public static int kRpcRpcPrepareToReceiveGameMap;
  public static int kCmdCmdOnClientReceivedMap = -10160345;

  public void Init(bool is_server)
  {
    Debug.Log((object) ("GlobalNetworkObject init, is_server = " + is_server.ToString()));
    this._is_server = is_server;
  }

  public void SendMapFromServerToClients(SerializableGameMap map)
  {
    this.CallRpcPrepareToReceiveGameMap();
    map.RestoreScene();
  }

  [ClientRpc(channel = 0)]
  public void RpcPrepareToReceiveGameMap()
  {
    Debug.Log((object) ("RpcPrepareToReceiveGameMap, is_server = " + this._is_server.ToString()));
    if (this._is_server)
    {
      this.OnDataFragmentReceived -= new UnityAction<int, byte[]>(this.ClientOnMapProgress);
      this.OnDataCompletelyReceived -= new UnityAction<int, byte[]>(this.ClientOnMapReceived);
    }
    else
    {
      this.OnDataFragmentReceived += new UnityAction<int, byte[]>(this.ClientOnMapProgress);
      this.OnDataCompletelyReceived += new UnityAction<int, byte[]>(this.ClientOnMapReceived);
      MainGame.me.save.map.ClearSceneMap();
    }
  }

  [Command]
  public void CmdOnClientReceivedMap()
  {
    if (!this._is_server)
      return;
    Debug.Log((object) nameof (CmdOnClientReceivedMap));
  }

  [Client]
  public void ClientOnMapProgress(int transmission_id, byte[] data)
  {
    if (!NetworkClient.active)
    {
      Debug.LogWarning((object) "[Client] function 'System.Void GlobalNetworkObject::ClientOnMapProgress(System.Int32,System.Byte[])' called on server");
    }
    else
    {
      if (this._is_server)
        return;
      Debug.Log((object) ("ClientOnMapProgress, data len = " + data.Length.ToString()));
    }
  }

  [Client]
  public void ClientOnMapReceived(int transmission_id, byte[] data)
  {
    if (!NetworkClient.active)
    {
      Debug.LogWarning((object) "[Client] function 'System.Void GlobalNetworkObject::ClientOnMapReceived(System.Int32,System.Byte[])' called on server");
    }
    else
    {
      if (this._is_server)
        return;
      Debug.Log((object) nameof (ClientOnMapReceived));
      string json = Encoding.UTF8.GetString(data);
      MainGame.me.save.map.FromJSON(json);
      MainGame.me.save.map.RestoreScene();
      this.CallCmdOnClientReceivedMap();
    }
  }

  public new void UNetVersion()
  {
  }

  public static void InvokeCmdCmdOnClientReceivedMap(NetworkBehaviour obj, NetworkReader reader)
  {
    if (!NetworkServer.active)
      Debug.LogError((object) "Command CmdOnClientReceivedMap called on client.");
    else
      ((GlobalNetworkObject) obj).CmdOnClientReceivedMap();
  }

  public void CallCmdOnClientReceivedMap()
  {
    if (!NetworkClient.active)
      Debug.LogError((object) "Command function CmdOnClientReceivedMap called on server.");
    else if (this.isServer)
    {
      this.CmdOnClientReceivedMap();
    }
    else
    {
      NetworkWriter writer = new NetworkWriter();
      writer.Write((short) 0);
      writer.Write((short) 5);
      writer.WritePackedUInt32((uint) GlobalNetworkObject.kCmdCmdOnClientReceivedMap);
      writer.Write(this.GetComponent<NetworkIdentity>().netId);
      this.SendCommandInternal(writer, 0, "CmdOnClientReceivedMap");
    }
  }

  public static void InvokeRpcRpcPrepareToReceiveGameMap(NetworkBehaviour obj, NetworkReader reader)
  {
    if (!NetworkClient.active)
      Debug.LogError((object) "RPC RpcPrepareToReceiveGameMap called on server.");
    else
      ((GlobalNetworkObject) obj).RpcPrepareToReceiveGameMap();
  }

  public void CallRpcPrepareToReceiveGameMap()
  {
    if (!NetworkServer.active)
    {
      Debug.LogError((object) "RPC Function RpcPrepareToReceiveGameMap called on client.");
    }
    else
    {
      NetworkWriter writer = new NetworkWriter();
      writer.Write((short) 0);
      writer.Write((short) 2);
      writer.WritePackedUInt32((uint) GlobalNetworkObject.kRpcRpcPrepareToReceiveGameMap);
      writer.Write(this.GetComponent<NetworkIdentity>().netId);
      this.SendRPCInternal(writer, 0, "RpcPrepareToReceiveGameMap");
    }
  }

  static GlobalNetworkObject()
  {
    NetworkBehaviour.RegisterCommandDelegate(typeof (GlobalNetworkObject), GlobalNetworkObject.kCmdCmdOnClientReceivedMap, new NetworkBehaviour.CmdDelegate(GlobalNetworkObject.InvokeCmdCmdOnClientReceivedMap));
    GlobalNetworkObject.kRpcRpcPrepareToReceiveGameMap = 1049007849;
    NetworkBehaviour.RegisterRpcDelegate(typeof (GlobalNetworkObject), GlobalNetworkObject.kRpcRpcPrepareToReceiveGameMap, new NetworkBehaviour.CmdDelegate(GlobalNetworkObject.InvokeRpcRpcPrepareToReceiveGameMap));
    NetworkCRC.RegisterBehaviour(nameof (GlobalNetworkObject), 0);
  }

  public override bool OnSerialize(NetworkWriter writer, bool forceAll)
  {
    bool flag1 = base.OnSerialize(writer, forceAll);
    bool flag2;
    return flag2 | flag1;
  }

  public override void OnDeserialize(NetworkReader reader, bool initialState)
  {
    base.OnDeserialize(reader, initialState);
  }

  public override void PreStartClient() => base.PreStartClient();
}
