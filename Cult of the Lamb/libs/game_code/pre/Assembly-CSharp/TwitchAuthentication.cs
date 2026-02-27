// Decompiled with JetBrains decompiler
// Type: TwitchAuthentication
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public static class TwitchAuthentication
{
  public static string redirect
  {
    get
    {
      return true ? "https://cotl.connect.streamingtoolsmith.com/redirect&state=" : "https://cotl.connect.dev.streamingtoolsmith.com/redirect&state=";
    }
  }

  public static string request
  {
    get
    {
      return true ? "https://cotl.connect.streamingtoolsmith.com/subscribe?state=" : "https://cotl.connect.dev.streamingtoolsmith.com/subscribe?state=";
    }
  }

  public static bool IsAuthenticated { get; private set; }

  public static bool IsLiveStreaming { get; private set; }

  public static event TwitchAuthentication.AuthenticationResponse OnAuthenticated;

  public static event TwitchAuthentication.AuthenticationResponse OnLoggedOut;

  public static void TryAuthenticate(TwitchRequest.ConnectionResponse response)
  {
    if (!string.IsNullOrEmpty(TwitchManager.SecretKey))
      TwitchRequest.ConnectionTest((TwitchRequest.ConnectionResponse) ((res, result) =>
      {
        TwitchAuthentication.IsAuthenticated = res == TwitchRequest.ResponseType.Success;
        TwitchRequest.ConnectionResponse connectionResponse = response;
        if (connectionResponse != null)
          connectionResponse(res, result);
        if (result != null)
          TwitchAuthentication.IsLiveStreaming = result.is_live;
        if (res != TwitchRequest.ResponseType.Success)
          return;
        TwitchAuthentication.AuthenticationResponse onAuthenticated = TwitchAuthentication.OnAuthenticated;
        if (onAuthenticated == null)
          return;
        onAuthenticated();
      }));
    else
      TwitchAuthentication.IsAuthenticated = false;
  }

  public static void RequestLogIn(Action<TwitchRequest.ResponseType> callback)
  {
    string str = SteamUser.GetSteamID().ToString();
    Application.OpenURL($"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id=wph0p912gucvcee0114kfoukn319db&redirect_uri={TwitchAuthentication.redirect}{str}");
    TwitchRequest.Request(TwitchAuthentication.request + str, (TwitchRequest.RequestResponse) ((response, result) =>
    {
      Debug.Log((object) ("Twitch authentication request finished... result: " + response.ToString()));
      if (response == TwitchRequest.ResponseType.Success)
      {
        TwitchAuthentication.LogInResultData logInResultData = JsonUtility.FromJson<TwitchAuthentication.LogInResultData>(result);
        if (logInResultData == null || logInResultData.data == null)
          return;
        TwitchManager.SecretKey = logInResultData.data.secret;
        GameManager.GetInstance().StartCoroutine((IEnumerator) Delay());
      }
      else
      {
        Action<TwitchRequest.ResponseType> action = callback;
        if (action == null)
          return;
        action(TwitchRequest.ResponseType.Failure);
      }
    }));

    IEnumerator Delay()
    {
      // ISSUE: variable of a compiler-generated type
      TwitchAuthentication.\u003C\u003Ec__DisplayClass23_0 cDisplayClass230 = this;
      yield return (object) new WaitForEndOfFrame();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TwitchAuthentication.TryAuthenticate(cDisplayClass230.\u003C\u003E9__2 ?? (cDisplayClass230.\u003C\u003E9__2 = new TwitchRequest.ConnectionResponse(cDisplayClass230.\u003CRequestLogIn\u003Eb__2)));
    }
  }

  public static void SignOut()
  {
    TwitchManager.Abort();
    TwitchAuthentication.IsAuthenticated = false;
    TwitchManager.SecretKey = "";
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
    public string secret;
  }

  public delegate void LogInResponse(
    TwitchRequest.ResponseType response,
    TwitchAuthentication.LogInResultData resultData);

  public delegate void AuthenticationResponse();
}
