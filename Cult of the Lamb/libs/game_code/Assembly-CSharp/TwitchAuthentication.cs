// Decompiled with JetBrains decompiler
// Type: TwitchAuthentication
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Newtonsoft.Json;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public static class TwitchAuthentication
{
  [CompilerGenerated]
  public static bool \u003CIsAuthenticated\u003Ek__BackingField;
  public static AuthenticationApi AUTHENTICATION_API;

  public static bool IsAuthenticated
  {
    get => TwitchAuthentication.\u003CIsAuthenticated\u003Ek__BackingField;
    set => TwitchAuthentication.\u003CIsAuthenticated\u003Ek__BackingField = value;
  }

  public static event TwitchAuthentication.AuthenticationResponse OnAuthenticated;

  public static event TwitchAuthentication.AuthenticationResponse OnLoggedOut;

  public static string uri
  {
    get
    {
      return true ? "https://twitch-link.cotl.devolver.digital" : "https://cotl-twitch-auth-service-development.onrender.com";
    }
  }

  public static void TryAuthenticate(Action<TwitchRequest.ResponseType> callback)
  {
    if (TwitchAuthentication.IsAuthenticated || string.IsNullOrEmpty(TwitchManager.Token))
      return;
    TwitchRequest.InitialiseEBS();
    TwitchRequest.ConnectionTest((TwitchRequest.ConnectionResponse) (res =>
    {
      TwitchAuthentication.IsAuthenticated = res == TwitchRequest.ResponseType.Success;
      if (res == TwitchRequest.ResponseType.Success)
      {
        TwitchAuthentication.UpdateChannelInformation(TwitchManager.Token);
        TwitchAuthentication.AuthenticationResponse onAuthenticated = TwitchAuthentication.OnAuthenticated;
        if (onAuthenticated != null)
          onAuthenticated();
        TwitchManager.SetLanguage(LocalizationManager.CurrentLanguageCode);
      }
      Action<TwitchRequest.ResponseType> action = callback;
      if (action == null)
        return;
      action(TwitchRequest.ResponseType.Success);
    }));
  }

  public static async void RequestLogIn(Action<TwitchRequest.ResponseType> callback)
  {
    TwitchAuthentication.AUTHENTICATION_API = new AuthenticationApi(new Configuration()
    {
      BasePath = TwitchAuthentication.uri
    });
    try
    {
      CreateSession200Response result = await TwitchAuthentication.AUTHENTICATION_API.CreateSessionAsync(new CancellationToken());
      Debug.Log((object) result);
      Application.OpenURL(result.AuthorizationUrl);
      Task<AuthSession> sessionResult;
      do
      {
        sessionResult = TwitchAuthentication.AUTHENTICATION_API.GetSessionResultAsync(result.SessionId, new CancellationToken());
        await System.Threading.Tasks.Task.Delay(1000);
      }
      while (sessionResult.Result.State == AuthSession.StateEnum.Pending);
      TwitchManager.Token = sessionResult.Result.Token;
      TwitchAuthentication.UpdateChannelInformation(TwitchManager.Token);
      TwitchAuthentication.TryAuthenticate(callback);
      result = (CreateSession200Response) null;
      sessionResult = (Task<AuthSession>) null;
    }
    catch (ApiException ex)
    {
      Debug.LogError((object) ("Exception when calling DefaultApi.CreateSession: " + ex.Message));
      Debug.LogError((object) ("Status Code: " + ex.ErrorCode.ToString()));
      Debug.LogError((object) ex.StackTrace);
      Action<TwitchRequest.ResponseType> action = callback;
      if (action == null)
        return;
      action(TwitchRequest.ResponseType.Failure);
    }
  }

  public static void UpdateChannelInformation(string token)
  {
    bool flag = true;
    for (int index = token.Length - 1; index >= 0; --index)
    {
      if (token[index] == '.')
      {
        flag = !flag;
        token = token.Remove(index, 1);
      }
      else if (flag)
        token = token.Remove(index, 1);
    }
    int num = token.Length % 4;
    if (num != 0)
      token = token.PadRight(token.Length + (4 - num), '=');
    TwitchAuthentication.data data = JsonConvert.DeserializeObject<TwitchAuthentication.data>(Encoding.UTF8.GetString(Convert.FromBase64String(token)));
    TwitchManager.ChannelID = data.channel_id;
    TwitchManager.ChannelName = data.channel_display_name;
  }

  public static void SignOut()
  {
    TwitchManager.Abort();
    TwitchAuthentication.IsAuthenticated = false;
    TwitchManager.Token = "";
    TwitchAuthentication.AuthenticationResponse onLoggedOut = TwitchAuthentication.OnLoggedOut;
    if (onLoggedOut == null)
      return;
    onLoggedOut();
  }

  [Serializable]
  public class LogInResultData
  {
    public bool success;
    public TwitchAuthentication.data data;
  }

  [Serializable]
  public class data
  {
    public string channel_id;
    public string channel_display_name;
    public int iat;
    public string iss;
  }

  public delegate void LogInResponse(
    TwitchRequest.ResponseType response,
    TwitchAuthentication.LogInResultData resultData);

  public delegate void AuthenticationResponse();
}
