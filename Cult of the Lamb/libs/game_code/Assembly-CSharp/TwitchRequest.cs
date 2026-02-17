// Decompiled with JetBrains decompiler
// Type: TwitchRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#nullable disable
public static class TwitchRequest
{
  public static SocketIOUnity socket;
  public static EBSApi EBS_API;
  public static DefaultApi DEFAULT_API;

  public static string uri
  {
    get
    {
      return true ? "https://ebs-v2.cotl.devolver.digital" : "https://cotl-twitch-ebs-development.onrender.com";
    }
  }

  public static string socket_url
  {
    get
    {
      return true ? "https://twitch-realtime.cotl.devolver.digital" : "https://cotl-twitch-realtime-service.onrender.com";
    }
  }

  public static bool SocketConnected
  {
    get => TwitchRequest.socket != null && TwitchRequest.socket.Connected;
  }

  public static event TwitchRequest.SocketResponse OnSocketReceived;

  public static void InitialiseEBS()
  {
    if (TwitchRequest.EBS_API != null)
      return;
    Configuration configuration = new Configuration();
    configuration.BasePath = TwitchRequest.uri;
    configuration.AccessToken = TwitchManager.Token;
    TwitchRequest.EBS_API = new EBSApi(configuration);
    TwitchRequest.DEFAULT_API = new DefaultApi(configuration);
  }

  public static async void ConnectionTest(TwitchRequest.ConnectionResponse callback)
  {
    try
    {
      GetConnection200Response connectionAsync = await TwitchRequest.EBS_API.GetConnectionAsync((GetConnectionParamsParameter) null, new CancellationToken());
      if (connectionAsync.IsLiveAndPlayingGame)
        TwitchRequest.ConnectToSocket();
      TwitchRequest.ConnectionResponse connectionResponse = callback;
      if (connectionResponse == null)
        return;
      connectionResponse(connectionAsync.IsLiveAndPlayingGame ? TwitchRequest.ResponseType.Success : TwitchRequest.ResponseType.Failure);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static async void ConnectToSocket()
  {
    if (TwitchRequest.socket != null && TwitchRequest.socket.Connected)
      return;
    if (TwitchRequest.socket == null)
    {
      TwitchRequest.socket = new SocketIOUnity(TwitchRequest.socket_url);
      TwitchRequest.socket.Options.ExtraHeaders = new Dictionary<string, string>()
      {
        {
          "Authorization",
          "Bearer " + TwitchManager.Token
        }
      };
      TwitchRequest.socket.OnConnected += (EventHandler) ((sender, e) =>
      {
        TwitchRequest.socket.Emit("channelPointsRewards.enable");
        TwitchTotem.Initialise();
        TwitchMessages.Initialise();
        TwitchHelpHinder.Initialise();
        TwitchFollowers.Initialise();
        TwitchVoting.Initialise();
        TwitchRequest.EBS_API.EndPollAsync(new CancellationToken());
      });
      OnAnyHandler handler = (OnAnyHandler) ((key, response) =>
      {
        string str = response.ToString().Remove(0, 1);
        string data = str.Remove(str.Length - 1, 1);
        Debug.Log((object) ("EVENT: " + key));
        TwitchRequest.SocketResponse onSocketReceived = TwitchRequest.OnSocketReceived;
        if (onSocketReceived == null)
          return;
        onSocketReceived(key, data);
      });
      TwitchRequest.socket.OnAnyInUnityThread(handler);
    }
    await TwitchRequest.socket.ConnectAsync();
    if (TwitchRequest.socket == null || TwitchRequest.socket.Connected)
      return;
    await TwitchRequest.socket.DisconnectAsync();
    await System.Threading.Tasks.Task.Delay(5000);
    TwitchRequest.ConnectToSocket();
  }

  public static async void Abort()
  {
    if (TwitchRequest.socket == null)
      return;
    TwitchRequest.socket.Emit("channelPointsRewards.disable");
    await TwitchRequest.socket.DisconnectAsync();
    TwitchRequest.socket.Dispose();
    TwitchRequest.socket = (SocketIOUnity) null;
  }

  [Serializable]
  public class ConnectionTestData
  {
    public bool connected;
    public bool is_live;
    public long channel_id;
    public string channel_display_name;
    public TwitchRequest.ActiveData features;
  }

  [Serializable]
  public class ActiveData
  {
    public bool helpHinder;
    public bool followerRaffle;
    public bool totem;
    public bool voting;
  }

  [Serializable]
  public class SendingData
  {
    public string language;
    public string active_save_id;
  }

  public enum ResponseType
  {
    None,
    Success,
    Failure,
  }

  public enum RequestType
  {
    GET,
    POST,
    PUT,
    PATCH,
  }

  public delegate void RequestResponse(TwitchRequest.ResponseType response, string result);

  public delegate void ConnectionResponse(TwitchRequest.ResponseType response);

  public delegate void SocketResponse(string key, string data);
}
