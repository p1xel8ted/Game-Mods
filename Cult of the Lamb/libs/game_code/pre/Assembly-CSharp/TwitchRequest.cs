// Decompiled with JetBrains decompiler
// Type: TwitchRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public static class TwitchRequest
{
  private static SocketIOUnity socket;

  public static string uri
  {
    get
    {
      return true ? "https://ebs.cotl.devolver.digital/" : "https://cotl-ebs.dev.streamingtoolsmith.com/";
    }
  }

  public static bool SocketConnected
  {
    get => TwitchRequest.socket != null && TwitchRequest.socket.Connected;
  }

  public static event TwitchRequest.SocketResponse OnSocketReceived;

  public static void SendEBSData()
  {
    TwitchRequest.Request(TwitchRequest.uri + "channel", (TwitchRequest.RequestResponse) null, TwitchRequest.RequestType.PATCH, JsonUtility.ToJson((object) new TwitchRequest.SendingData()
    {
      language = LocalizationManager.CurrentLanguageCode,
      active_save_id = DataManager.Instance.SaveUniqueID
    }), new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  public static void ConnectionTest(TwitchRequest.ConnectionResponse callback)
  {
    TwitchRequest.Request(TwitchRequest.uri + "auth/connection", (TwitchRequest.RequestResponse) ((response, result) =>
    {
      if (response != TwitchRequest.ResponseType.Success)
        return;
      try
      {
        TwitchRequest.ConnectionTestData result1 = JsonUtility.FromJson<TwitchRequest.ConnectionTestData>(result);
        TwitchManager.ChannelID = result1.channel_id.ToString();
        TwitchManager.ChannelName = result1.channel_display_name;
        if (result1.is_live)
          TwitchRequest.ConnectToSocket();
        if (result1.features != null)
        {
          TwitchTotem.Deactivated = !result1.features.totem;
          TwitchFollowers.Deactivated = !result1.features.followerRaffle;
          TwitchHelpHinder.Deactivated = !result1.features.helpHinder;
        }
        TwitchTotem.Initialise();
        TwitchHelpHinder.Initialise();
        TwitchRequest.ConnectionResponse connectionResponse = callback;
        if (connectionResponse != null)
          connectionResponse(response, result1);
        TwitchRequest.SendEBSData();
      }
      catch (Exception ex)
      {
      }
    }), TwitchRequest.RequestType.GET, "", new KeyValuePair<string, string>("x-cotl-channel-secret", TwitchManager.SecretKey));
  }

  private static async void ConnectToSocket()
  {
    if (TwitchRequest.socket != null && TwitchRequest.socket.Connected)
      return;
    if (TwitchRequest.socket == null)
    {
      TwitchRequest.socket = new SocketIOUnity(TwitchRequest.uri);
      TwitchRequest.socket.OnConnected += (EventHandler) ((sender, e) => TwitchRequest.socket.Emit("channel.join", (object) TwitchManager.ChannelID));
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

  public static void Request(
    string uri,
    TwitchRequest.RequestResponse callback,
    TwitchRequest.RequestType requestType = TwitchRequest.RequestType.GET,
    string data = "",
    params KeyValuePair<string, string>[] headers)
  {
    MonoSingleton<TwitchManager>.Instance.StartCoroutine((IEnumerator) TwitchRequest.RequestIE(uri, callback, requestType, data, headers));
  }

  public static IEnumerator RequestIE(
    string uri,
    TwitchRequest.RequestResponse callback,
    TwitchRequest.RequestType requestType = TwitchRequest.RequestType.GET,
    string data = "",
    params KeyValuePair<string, string>[] headers)
  {
    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    {
      webRequest.method = requestType.ToString();
      foreach (KeyValuePair<string, string> header in headers)
        webRequest.SetRequestHeader(header.Key, header.Value);
      if (!string.IsNullOrEmpty(data))
      {
        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
        uploadHandlerRaw.contentType = "application/json";
        webRequest.uploadHandler = (UploadHandler) uploadHandlerRaw;
      }
      UnityWebRequestAsyncOperation requestOperation = webRequest.SendWebRequest();
      while (!requestOperation.isDone)
        yield return (object) null;
      if (webRequest.isNetworkError)
      {
        TwitchRequest.RequestResponse requestResponse = callback;
        if (requestResponse != null)
          requestResponse(TwitchRequest.ResponseType.Failure, webRequest.downloadHandler.text);
      }
      else
      {
        TwitchRequest.RequestResponse requestResponse = callback;
        if (requestResponse != null)
          requestResponse(TwitchRequest.ResponseType.Success, webRequest.downloadHandler.text);
      }
      requestOperation = (UnityWebRequestAsyncOperation) null;
    }
  }

  public static async void Abort()
  {
    if (TwitchRequest.socket == null)
      return;
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

  public delegate void ConnectionResponse(
    TwitchRequest.ResponseType response,
    TwitchRequest.ConnectionTestData result);

  public delegate void SocketResponse(string key, string data);
}
