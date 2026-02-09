// Decompiled with JetBrains decompiler
// Type: CustomNetworkManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class CustomNetworkManager : NetworkManager
{
  public static CustomNetworkManager me;
  public static bool is_running;
  public static bool is_server;
  public static bool _server_started;
  public static GlobalNetworkObject _gno;

  public override void OnStartServer() => base.OnStartServer();

  public void OnAddPlayer(NetworkMessage netmsg) => Debug.Log((object) "Add player");

  public new void Awake()
  {
    Debug.Log((object) "CustomNetworkManager awake");
    CustomNetworkManager.me = this;
    if (!((Object) CustomNetworkManager._gno == (Object) null))
      return;
    GameObject gameObject = new GameObject("GlobalNetworkObject");
    gameObject.transform.SetParent(this.gameObject.transform, false);
    CustomNetworkManager._gno = gameObject.AddComponent<GlobalNetworkObject>();
  }

  public override void OnServerAddPlayer(
    NetworkConnection conn,
    short player_controller_id,
    NetworkReader extra_message_reader)
  {
    Debug.Log((object) ("OnServerAddPlayer id = " + player_controller_id.ToString()));
    base.OnServerAddPlayer(conn, player_controller_id, extra_message_reader);
  }

  public override void OnServerAddPlayer(NetworkConnection conn, short player_controller_id)
  {
    Debug.Log((object) $"OnServerAddPlayer (2) id = {player_controller_id.ToString()}, host = {conn.hostId.ToString()}, addr = {conn.address}");
    PlayerComponent playerComponent = PlayerComponent.SpawnPlayer(CustomNetworkManager.IsLocalPlayer(conn));
    NetworkServer.AddPlayerForConnection(conn, playerComponent.gameObject, player_controller_id);
    MainGame.me.save.QuickSave();
    CustomNetworkManager._gno.SendMapFromServerToClients(MainGame.me.save.map);
  }

  public override void OnClientConnect(NetworkConnection conn)
  {
    Debug.Log((object) ("OnClientConnect, is_server = " + CustomNetworkManager.is_server.ToString()));
    if (!CustomNetworkManager.is_server)
      MainGame.me.world.FindAndRemovePlayerPrefab();
    Debug.Log((object) "ClientScene.AddPlayer");
    ClientScene.AddPlayer(conn, (short) 0);
    ReadOnlyCollection<NetworkConnection> connections = NetworkServer.connections;
    List<PlayerController> localPlayers = ClientScene.localPlayers;
    Dictionary<short, NetworkMessageDelegate> handlers = NetworkServer.handlers;
    foreach (NetworkConnection networkConnection in connections)
      ;
  }

  public override void OnServerConnect(NetworkConnection conn)
  {
    CustomNetworkManager.is_server = true;
    bool is_server = CustomNetworkManager.IsLocalPlayer(conn);
    Debug.Log((object) $"OnServerConnect, conn.hostId = {conn.hostId.ToString()}, _server_started = {CustomNetworkManager._server_started.ToString()}");
    base.OnServerConnect(conn);
    if (!CustomNetworkManager._server_started)
    {
      if ((Object) CustomNetworkManager._gno == (Object) null)
      {
        Debug.LogError((object) "GNO is null");
        return;
      }
      CustomNetworkManager._gno.Init(is_server);
    }
    if (!is_server || CustomNetworkManager._server_started)
      return;
    CustomNetworkManager._server_started = true;
  }

  public static bool IsLocalPlayer(NetworkConnection conn) => conn.hostId == -1;
}
