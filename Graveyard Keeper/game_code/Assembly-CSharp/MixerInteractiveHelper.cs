// Decompiled with JetBrains decompiler
// Type: MixerInteractiveHelper
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class MixerInteractiveHelper : MonoBehaviour
{
  public bool _runInBackgroundIfInteractive = true;
  public string _defaultSceneID;
  public Dictionary<string, string> _groupSceneMapping = new Dictionary<string, string>();
  public List<string> rpcOwningMonoBehaviorNames = new List<string>();
  public List<string> rpcMethodNames = new List<string>();
  public Dictionary<string, MixerInteractive.RpcCachedMethodInfo> cachedRPCMethods = new Dictionary<string, MixerInteractive.RpcCachedMethodInfo>();
  public List<MixerInteractiveHelper._InteractiveWebRequestData> _queuedWebRequests;
  public List<MixerInteractiveHelper.InteractiveTimerData> _queuedStartTimerRequests;
  public List<MixerInteractiveHelper.InteractiveTimerType> _queuedStopTimerRequests;
  public List<MixerInteractiveHelper.CoRoutineInfo> _runningCoRoutines;
  public bool _queuedTryGetAuthTokensFromCacheRequest;
  public bool _queuedWriteAuthTokensToCacheRequest;
  public string _authTokenValueToWriteToCache;
  public string _refreshTokenValueToWriteToCache;
  public static MixerInteractiveHelper _singletonInstance;

  public event MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler OnInternalWebRequestStateChanged;

  public event MixerInteractiveHelper.OnInternalCheckAuthStatusCallbackEventHandler OnInternalCheckAuthStatusTimerCallback;

  public event MixerInteractiveHelper.OnInternalRefreshShortCodeCallbackEventHandler OnInternalRefreshShortCodeTimerCallback;

  public event MixerInteractiveHelper.OnInternalReconnectCallbackEventHandler OnInternalReconnectTimerCallback;

  public event MixerInteractiveHelper.OnTryGetAuthTokensFromCacheCallbackEventHandler OnTryGetAuthTokensFromCacheCallback;

  public static MixerInteractiveHelper _SingletonInstance
  {
    get
    {
      if ((Object) MixerInteractiveHelper._singletonInstance == (Object) null)
      {
        MixerInteractiveHelper[] objectsOfType = Object.FindObjectsOfType<MixerInteractiveHelper>();
        if (objectsOfType.Length != 0)
          MixerInteractiveHelper._singletonInstance = objectsOfType[0];
        MixerInteractiveHelper._singletonInstance.Initialize();
      }
      return MixerInteractiveHelper._singletonInstance;
    }
  }

  public void Update()
  {
    if (!((Object) MixerInteractiveHelper._singletonInstance != (Object) null))
      return;
    foreach (MixerInteractiveHelper._InteractiveWebRequestData queuedWebRequest in this._queuedWebRequests)
      this._runningCoRoutines.Add(new MixerInteractiveHelper.CoRoutineInfo("MakeWebRequestCoRoutine", this.StartCoroutine(this.MakeWebRequestCoRoutine(queuedWebRequest.requestID, queuedWebRequest.requestUrl, queuedWebRequest.headers, queuedWebRequest.httpVerb, queuedWebRequest.postData))));
    this._queuedWebRequests.Clear();
    foreach (MixerInteractiveHelper.InteractiveTimerData startTimerRequest in this._queuedStartTimerRequests)
    {
      MixerInteractiveHelper.InteractiveTimerType type = startTimerRequest.type;
      float interval = startTimerRequest.interval;
      switch (type)
      {
        case MixerInteractiveHelper.InteractiveTimerType.CheckAuthStatus:
          this.StopCoroutineByName("CheckAuthStatusCoRoutine");
          this._runningCoRoutines.Add(new MixerInteractiveHelper.CoRoutineInfo("CheckAuthStatusCoRoutine", this.StartCoroutine(this.CheckAuthStatusCoRoutine(interval))));
          continue;
        case MixerInteractiveHelper.InteractiveTimerType.RefreshShortCode:
          this.StopCoroutineByName("RefreshShortCodeCoRoutine");
          this._runningCoRoutines.Add(new MixerInteractiveHelper.CoRoutineInfo("RefreshShortCodeCoRoutine", this.StartCoroutine(this.RefreshShortCodeCoRoutine(interval))));
          continue;
        case MixerInteractiveHelper.InteractiveTimerType.Reconnect:
          this.StopCoroutineByName("ReconnectCodeCoRoutine");
          this._runningCoRoutines.Add(new MixerInteractiveHelper.CoRoutineInfo("ReconnectCodeCoRoutine", this.StartCoroutine(this.ReconnectCodeCoRoutine(interval))));
          continue;
        default:
          continue;
      }
    }
    this._queuedStartTimerRequests.Clear();
    foreach (int stopTimerRequest in this._queuedStopTimerRequests)
    {
      switch (stopTimerRequest)
      {
        case 0:
          this.StopCoroutineByName("CheckAuthStatusCoRoutine");
          continue;
        case 1:
          this.StopCoroutineByName("RefreshShortCodeCoRoutine");
          continue;
        case 2:
          this.StopCoroutineByName("ReconnectCodeCoRoutine");
          continue;
        default:
          continue;
      }
    }
    this._queuedStopTimerRequests.Clear();
    if (this._queuedTryGetAuthTokensFromCacheRequest)
    {
      MixerInteractiveHelper.TryGetAuthTokensFromCacheEventArgs e = new MixerInteractiveHelper.TryGetAuthTokensFromCacheEventArgs();
      e.AuthToken = PlayerPrefs.GetString("MixerInteractive-AuthToken");
      e.RefreshToken = PlayerPrefs.GetString("MixerInteractive-RefreshToken");
      if (this.OnTryGetAuthTokensFromCacheCallback != null)
        this.OnTryGetAuthTokensFromCacheCallback((object) this, e);
      this._queuedTryGetAuthTokensFromCacheRequest = false;
    }
    if (!this._queuedWriteAuthTokensToCacheRequest)
      return;
    this.WriteAuthTokensToCacheImpl();
  }

  public void StopCoroutineByName(string name)
  {
    foreach (MixerInteractiveHelper.CoRoutineInfo runningCoRoutine in this._runningCoRoutines)
    {
      if (runningCoRoutine.name == name)
        this.StopCoroutine(runningCoRoutine.coRoutine);
    }
  }

  public void Initialize()
  {
    this._queuedWebRequests = new List<MixerInteractiveHelper._InteractiveWebRequestData>();
    this._queuedStartTimerRequests = new List<MixerInteractiveHelper.InteractiveTimerData>();
    this._queuedStopTimerRequests = new List<MixerInteractiveHelper.InteractiveTimerType>();
    this._runningCoRoutines = new List<MixerInteractiveHelper.CoRoutineInfo>();
  }

  public void _MakeWebRequest(
    string requestID,
    string requestUrl,
    Dictionary<string, string> headers = null,
    string httpVerb = "",
    string postData = "")
  {
    this._queuedWebRequests.Add(new MixerInteractiveHelper._InteractiveWebRequestData(requestID, requestUrl, headers, httpVerb, postData));
  }

  public void StartTimer(MixerInteractiveHelper.InteractiveTimerType type, float interval)
  {
    this._queuedStartTimerRequests.Add(new MixerInteractiveHelper.InteractiveTimerData(type, interval));
  }

  public void StopTimer(MixerInteractiveHelper.InteractiveTimerType type)
  {
    this._queuedStopTimerRequests.Add(type);
  }

  public void StartTryGetAuthTokensFromCache()
  {
    this._queuedTryGetAuthTokensFromCacheRequest = true;
  }

  public void WriteAuthTokensToCache(string authToken, string refreshToken)
  {
    this._queuedWriteAuthTokensToCacheRequest = true;
    this._authTokenValueToWriteToCache = authToken;
    this._refreshTokenValueToWriteToCache = refreshToken;
  }

  public void WriteAuthTokensToCacheImpl()
  {
    this._queuedWriteAuthTokensToCacheRequest = false;
    PlayerPrefs.SetString("MixerInteractive-AuthToken", this._authTokenValueToWriteToCache);
    PlayerPrefs.SetString("MixerInteractive-RefreshToken", this._refreshTokenValueToWriteToCache);
    PlayerPrefs.Save();
  }

  public IEnumerator MakeWebRequestCoRoutine(
    string requestID,
    string requestUrl,
    Dictionary<string, string> headers,
    string httpVerb,
    string postData)
  {
    MixerInteractiveHelper interactiveHelper = this;
    UnityWebRequest request;
    if (httpVerb == "POST")
    {
      UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(new ASCIIEncoding().GetBytes(postData));
      request = UnityWebRequest.Post(requestUrl, postData);
      request.uploadHandler = (UploadHandler) uploadHandlerRaw;
      request.SetRequestHeader("Content-Type", "application/json");
    }
    else
      request = UnityWebRequest.Get(requestUrl);
    if (headers != null)
    {
      foreach (string key in headers.Keys)
        request.SetRequestHeader(key, headers[key]);
    }
    yield return (object) request.SendWebRequest();
    BackgroundWorker backgroundWorker = new BackgroundWorker();
    backgroundWorker.DoWork -= new DoWorkEventHandler(interactiveHelper.WebRequestBackgroundWorkerDoWork);
    backgroundWorker.DoWork += new DoWorkEventHandler(interactiveHelper.WebRequestBackgroundWorkerDoWork);
    backgroundWorker.RunWorkerAsync((object) new MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs(requestID, !request.isNetworkError, request.responseCode, request.downloadHandler.text, request.error));
    request.Dispose();
  }

  public void WebRequestBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
  {
    if (!(sender is BackgroundWorker backgroundWorker))
      return;
    backgroundWorker.DoWork -= new DoWorkEventHandler(this.WebRequestBackgroundWorkerDoWork);
    if (!(e.Argument is MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs e1) || this.OnInternalWebRequestStateChanged == null)
      return;
    this.OnInternalWebRequestStateChanged((object) this, e1);
  }

  public IEnumerator CheckAuthStatusCoRoutine(float interval)
  {
    MixerInteractiveHelper interactiveHelper = this;
    while (true)
    {
      yield return (object) new WaitForSeconds(interval);
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.DoWork -= new DoWorkEventHandler(interactiveHelper.CheckAuthStatusBackgroundWorkerDoWork);
      backgroundWorker.DoWork += new DoWorkEventHandler(interactiveHelper.CheckAuthStatusBackgroundWorkerDoWork);
      backgroundWorker.RunWorkerAsync();
    }
  }

  public IEnumerator RefreshShortCodeCoRoutine(float interval)
  {
    MixerInteractiveHelper interactiveHelper = this;
    while (true)
    {
      yield return (object) new WaitForSeconds(interval);
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.DoWork -= new DoWorkEventHandler(interactiveHelper.RefreshShortCodeBackgroundWorkerDoWork);
      backgroundWorker.DoWork += new DoWorkEventHandler(interactiveHelper.RefreshShortCodeBackgroundWorkerDoWork);
      backgroundWorker.RunWorkerAsync();
    }
  }

  public IEnumerator ReconnectCodeCoRoutine(float interval)
  {
    MixerInteractiveHelper interactiveHelper = this;
    while (true)
    {
      yield return (object) new WaitForSeconds(interval);
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.DoWork -= new DoWorkEventHandler(interactiveHelper.OnInternalReconnectBackgroundWorkerDoWork);
      backgroundWorker.DoWork += new DoWorkEventHandler(interactiveHelper.OnInternalReconnectBackgroundWorkerDoWork);
      backgroundWorker.RunWorkerAsync();
    }
  }

  public void CheckAuthStatusBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
  {
    if (!(sender is BackgroundWorker backgroundWorker))
      return;
    backgroundWorker.DoWork -= new DoWorkEventHandler(this.CheckAuthStatusBackgroundWorkerDoWork);
    if (this.OnInternalCheckAuthStatusTimerCallback == null)
      return;
    this.OnInternalCheckAuthStatusTimerCallback((object) this, new MixerInteractiveHelper.InternalTimerCallbackEventArgs());
  }

  public void RefreshShortCodeBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
  {
    if (!(sender is BackgroundWorker backgroundWorker))
      return;
    backgroundWorker.DoWork -= new DoWorkEventHandler(this.RefreshShortCodeBackgroundWorkerDoWork);
    if (this.OnInternalRefreshShortCodeTimerCallback == null)
      return;
    this.OnInternalRefreshShortCodeTimerCallback((object) this, new MixerInteractiveHelper.InternalTimerCallbackEventArgs());
  }

  public void OnInternalReconnectBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
  {
    if (!(sender is BackgroundWorker backgroundWorker))
      return;
    backgroundWorker.DoWork -= new DoWorkEventHandler(this.OnInternalReconnectBackgroundWorkerDoWork);
    if (this.OnInternalReconnectTimerCallback == null)
      return;
    this.OnInternalReconnectTimerCallback((object) this, new MixerInteractiveHelper.InternalTimerCallbackEventArgs());
  }

  public void Dispose() => this.StopAllCoroutines();

  public delegate void OnInternalWebRequestStateChangedEventHandler(
    object sender,
    MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs e);

  public delegate void OnInternalCheckAuthStatusCallbackEventHandler(
    object sender,
    MixerInteractiveHelper.InternalTimerCallbackEventArgs e);

  public delegate void OnInternalRefreshShortCodeCallbackEventHandler(
    object sender,
    MixerInteractiveHelper.InternalTimerCallbackEventArgs e);

  public delegate void OnInternalReconnectCallbackEventHandler(
    object sender,
    MixerInteractiveHelper.InternalTimerCallbackEventArgs e);

  public delegate void OnTryGetAuthTokensFromCacheCallbackEventHandler(
    object sender,
    MixerInteractiveHelper.TryGetAuthTokensFromCacheEventArgs e);

  public struct _InteractiveWebRequestData(
    string newRequestID,
    string newRequestUrl,
    Dictionary<string, string> newHeaders,
    string newHttpVerb,
    string newPostData)
  {
    public string requestID = newRequestID;
    public string requestUrl = newRequestUrl;
    public Dictionary<string, string> headers = newHeaders;
    public string httpVerb = newHttpVerb;
    public string postData = newPostData;
  }

  public class _InternalWebRequestStateChangedEventArgs
  {
    [CompilerGenerated]
    public string \u003CRequestID\u003Ek__BackingField;
    [CompilerGenerated]
    public bool \u003CSucceeded\u003Ek__BackingField;
    [CompilerGenerated]
    public long \u003CResponseCode\u003Ek__BackingField;
    [CompilerGenerated]
    public string \u003CResponseText\u003Ek__BackingField;
    [CompilerGenerated]
    public string \u003CErrorMessage\u003Ek__BackingField;

    public string RequestID
    {
      get => this.\u003CRequestID\u003Ek__BackingField;
      set => this.\u003CRequestID\u003Ek__BackingField = value;
    }

    public bool Succeeded
    {
      get => this.\u003CSucceeded\u003Ek__BackingField;
      set => this.\u003CSucceeded\u003Ek__BackingField = value;
    }

    public long ResponseCode
    {
      get => this.\u003CResponseCode\u003Ek__BackingField;
      set => this.\u003CResponseCode\u003Ek__BackingField = value;
    }

    public string ResponseText
    {
      get => this.\u003CResponseText\u003Ek__BackingField;
      set => this.\u003CResponseText\u003Ek__BackingField = value;
    }

    public string ErrorMessage
    {
      get => this.\u003CErrorMessage\u003Ek__BackingField;
      set => this.\u003CErrorMessage\u003Ek__BackingField = value;
    }

    public _InternalWebRequestStateChangedEventArgs(
      string requestID,
      bool succeeded,
      long responseCode,
      string responseText,
      string errorMessage)
    {
      this.RequestID = requestID;
      this.Succeeded = succeeded;
      this.ResponseCode = responseCode;
      this.ResponseText = responseText;
      this.ErrorMessage = errorMessage;
    }
  }

  public enum InteractiveTimerType
  {
    CheckAuthStatus,
    RefreshShortCode,
    Reconnect,
  }

  public struct InteractiveTimerData(
    MixerInteractiveHelper.InteractiveTimerType newType,
    float newInterval)
  {
    public MixerInteractiveHelper.InteractiveTimerType type = newType;
    public float interval = newInterval;
  }

  public class InternalTimerCallbackEventArgs
  {
  }

  public class TryGetAuthTokensFromCacheEventArgs
  {
    public string AuthToken;
    public string RefreshToken;
  }

  public struct CoRoutineInfo(string newName, Coroutine newCoRoutine)
  {
    public string name = newName;
    public Coroutine coRoutine = newCoRoutine;
  }
}
