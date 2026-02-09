// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractivityManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using WebSocketSharp;

#nullable disable
namespace Microsoft.Mixer;

public class InteractivityManager : IDisposable
{
  public static InteractivityManager _singletonInstance;
  [CompilerGenerated]
  public LoggingLevel \u003CLoggingLevel\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CProjectVersionID\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CAppID\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CShareCode\u003Ek__BackingField;
  [CompilerGenerated]
  public InteractivityState \u003CInteractivityState\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CShortCode\u003Ek__BackingField;
  public List<InteractiveEventArgs> _queuedEvents = new List<InteractiveEventArgs>();
  public Dictionary<uint, string> _outstandingMessages = new Dictionary<uint, string>();
  public WebSocket _websocket;
  public string _interactiveWebSocketUrl = string.Empty;
  public uint _currentmessageID = 1;
  public bool _disposed;
  public string _authShortCodeRequestHandle;
  public string _authToken;
  public string _oauthRefreshToken;
  public bool _initializedGroups;
  public bool _initializedScenes;
  public bool _pendingConnectToWebSocket;
  public bool _websocketConnected;
  public bool _shouldStartInteractive = true;
  public string _streamingAssetsPath = string.Empty;
  public List<InteractiveGroup> _groups;
  public List<InteractiveScene> _scenes;
  public List<InteractiveParticipant> _participants;
  public List<InteractiveControl> _controls;
  public List<InteractiveButtonControl> _buttons;
  public List<InteractiveJoystickControl> _joysticks;
  public List<string> _websocketHosts;
  public int _activeWebsocketHostIndex;
  public MixerInteractiveHelper mixerInteractiveHelper;
  public const string API_BASE = "https://mixer.com/api/v1/";
  public const string WEBSOCKET_DISCOVERY_URL = "https://mixer.com/api/v1/interactive/hosts";
  public const string API_CHECK_SHORT_CODE_AUTH_STATUS_PATH = "https://mixer.com/api/v1/oauth/shortcode/check/";
  public const string API_GET_SHORT_CODE_PATH = "https://mixer.com/api/v1/oauth/shortcode";
  public const string API_GET_OAUTH_TOKEN_PATH = "https://mixer.com/api/v1/oauth/token";
  public const string INTERACTIVE_DATA_FILE_NAME = "interactivedata.json";
  public const string CONFIG_FILE_NAME = "interactiveconfig.json";
  public const float POLL_FOR_SHORT_CODE_AUTH_INTERVAL = 0.5f;
  public const float WEBSOCKET_RECONNECT_INTERVAL = 0.5f;
  public const string INTERACTIVE_CONFIG_FILE_NAME = "interactiveconfig.json";
  public const string WS_MESSAGE_KEY_ACCESS_TOKEN_FROM_FILE = "AuthToken";
  public const string WS_MESSAGE_KEY_APPID = "appid";
  public const string WS_MESSAGE_KEY_CHANNEL_GROUPS = "channelGroups";
  public const string WS_MESSAGE_KEY_CODE = "code";
  public const string WS_MESSAGE_KEY_COOLDOWN = "cooldown";
  public const string WS_MESSAGE_KEY_CONNECTED_AT = "connectedAt";
  public const string WS_MESSAGE_KEY_CONTROLS = "controls";
  public const string WS_MESSAGE_KEY_CONTROL_ID = "controlID";
  public const string _WS_MESSAGE_KEY_COST = "cost";
  public const string WS_MESSAGE_KEY_DISABLED = "disabled";
  public const string WS_MESSAGE_KEY_ERROR_CODE = "code";
  public const string WS_MESSAGE_KEY_ERROR_MESSAGE = "message";
  public const string WS_MESSAGE_KEY_ERROR_PATH = "path";
  public const string WS_MESSAGE_KEY_ETAG = "etag";
  public const string WS_MESSAGE_KEY_EVENT = "event";
  public const string WS_MESSAGE_KEY_EXPIRATION = "expires_in";
  public const string WS_MESSAGE_KEY_GROUP = "group";
  public const string WS_MESSAGE_KEY_GROUPS = "groups";
  public const string WS_MESSAGE_KEY_GROUP_ID = "groupID";
  public const string WS_MESSAGE_KEY_LAST_INPUT_AT = "lastInputAt";
  public const string WS_MESSAGE_KEY_HANDLE = "handle";
  public const string WS_MESSAGE_KEY_ID = "id";
  public const string WS_MESSAGE_KEY_INPUT = "input";
  public const string WS_MESSAGE_KEY_INTENSITY = "intensity";
  public const string WS_MESSAGE_KEY_ISREADY = "isReady";
  public const string WS_MESSAGE_KEY_KIND = "kind";
  public const string WS_MESSAGE_KEY_LEVEL = "level";
  public const string WS_MESSAGE_KEY_REFRESH_TOKEN = "refresh_token";
  public const string WS_MESSAGE_KEY_REFRESH_TOKEN_FROM_FILE = "RefreshToken";
  public const string WS_MESSAGE_KEY_META = "meta";
  public const string WS_MESSAGE_KEY_PARTICIPANT_ID = "participantID";
  public const string WS_MESSAGE_KEY_PARTICIPANTS = "participants";
  public const string WS_MESSAGE_KEY_PARAMETERS = "params";
  public const string _WS_MESSAGE_KEY_PROGRESS = "progress";
  public const string WS_MESSAGE_KEY_PROJECT_VERSION_ID = "projectversionid";
  public const string WS_MESSAGE_KEY_RESULT = "result";
  public const string WS_MESSAGE_KEY_SCENE_ID = "sceneID";
  public const string WS_MESSAGE_KEY_SCENES = "scenes";
  public const string WS_MESSAGE_KEY_SCHEME = "scheme";
  public const string WS_MESSAGE_KEY_SESSION_ID = "sessionID";
  public const string WS_MESSAGE_KEY_PROJECT_SHARE_CODE = "sharecode";
  public const string _WS_MESSAGE_KEY_TEXT = "text";
  public const string WS_MESSAGE_KEY_TRANSACTION_ID = "transactionID";
  public const string WS_MESSAGE_KEY_TYPE = "type";
  public const string WS_MESSAGE_KEY_USER_ID = "userID";
  public const string WS_MESSAGE_KEY_USERNAME = "username";
  public const string WS_MESSAGE_KEY_VALUE = "value";
  public const string WS_MESSAGE_KEY_WEBSOCKET_ACCESS_TOKEN = "access_token";
  public const string WS_MESSAGE_KEY_WEBSOCKET_ADDRESS = "address";
  public const string WS_MESSAGE_KEY_X = "x";
  public const string WS_MESSAGE_KEY_Y = "y";
  public const string _WS_MESSAGE_VALUE_CONTROL_TYPE_BUTTON = "button";
  public const string _WS_MESSAGE_VALUE_DISABLED = "disabled";
  public const string _WS_MESSAGE_VALUE_DEFAULT_GROUP_ID = "default";
  public const string _WS_MESSAGE_VALUE_DEFAULT_SCENE_ID = "default";
  public const string _WS_MESSAGE_VALUE_CONTROL_TYPE_JOYSTICK = "joystick";
  public const string _WS_MESSAGE_VALUE_CONTROL_TYPE_LABEL = "label";
  public const string _WS_MESSAGE_VALUE_CONTROL_TYPE_TEXTBOX = "textbox";
  public const bool WS_MESSAGE_VALUE_TRUE = true;
  public const string WS_MESSAGE_TYPE_METHOD = "method";
  public const string WS_MESSAGE_TYPE_REPLY = "reply";
  public const string WS_MESSAGE_METHOD_CREATE_GROUPS = "createGroups";
  public const string WS_MESSAGE_METHOD_GET_ALL_PARTICIPANTS = "getAllParticipants";
  public const string WS_MESSAGE_METHOD_GET_GROUPS = "getGroups";
  public const string WS_MESSAGE_METHOD_GET_SCENES = "getScenes";
  public const string WS_MESSAGE_METHOD_GIVE_INPUT = "giveInput";
  public const string WS_MESSAGE_METHOD_HELLO = "hello";
  public const string WS_MESSAGE_METHOD_PARTICIPANT_JOIN = "onParticipantJoin";
  public const string WS_MESSAGE_METHOD_PARTICIPANT_LEAVE = "onParticipantLeave";
  public const string WS_MESSAGE_METHOD_PARTICIPANT_UPDATE = "onParticipantUpdate";
  public const string WS_MESSAGE_METHOD_READY = "ready";
  public const string WS_MESSAGE_METHOD_ON_CONTROL_UPDATE = "onControlUpdate";
  public const string WS_MESSAGE_METHOD_ON_CONTROL_CREATE = "onControlCreate";
  public const string WS_MESSAGE_METHOD_ON_GROUP_CREATE = "onGroupCreate";
  public const string WS_MESSAGE_METHOD_ON_GROUP_UPDATE = "onGroupUpdate";
  public const string WS_MESSAGE_METHOD_ON_READY = "onReady";
  public const string WS_MESSAGE_METHOD_ON_SCENE_CREATE = "onSceneCreate";
  public const string WS_MESSAGE_METHOD_SET_CAPTURE_TRANSACTION = "capture";
  public const string WS_MESSAGE_METHOD_SET_COMPRESSION = "setCompression";
  public const string WS_MESSAGE_METHOD_SET_CONTROL_FIRED = "setControlFired";
  public const string WS_MESSAGE_METHOD_SET_JOYSTICK_COORDINATES = "setJoystickCoordinates";
  public const string WS_MESSAGE_METHOD_SET_JOYSTICK_INTENSITY = "setJoystickIntensity";
  public const string WS_MESSAGE_METHOD_SET_BUTTON_CONTROL_PROPERTIES = "setButtonControlProperties";
  public const string WS_MESSAGE_METHOD_SET_CONTROL_TEXT = "setControlText";
  public const string WS_MESSAGE_METHOD_SET_CURRENT_SCENE = "setCurrentScene";
  public const string WS_MESSAGE_METHOD_UPDATE_CONTROLS = "updateControls";
  public const string WS_MESSAGE_METHOD_UPDATE_GROUPS = "updateGroups";
  public const string WS_MESSAGE_METHOD_UPDATE_PARTICIPANTS = "updateParticipants";
  public const string WS_MESSAGE_METHOD_UPDATE_SCENES = "updateScenes";
  public const string WS_MESSAGE_ERROR = "error";
  public const string _CONTROL_TYPE_BUTTON = "button";
  public const string _CONTROL_TYPE_JOYSTICK = "joystick";
  public const string _CONTROL_KIND_LABEL = "label";
  public const string _CONTROL_KIND_TEXTBOX = "textbox";
  public const string _CONTROL_KIND_SCREEN = "screen";
  public const string EVENT_NAME_MOUSE_DOWN = "mousedown";
  public const string EVENT_NAME_MOUSE_UP = "mouseup";
  public const string EVENT_NAME_KEY_DOWN = "keydown";
  public const string EVENT_NAME_KEY_UP = "keyup";
  public const string EVENT_NAME_MOVE = "move";
  public const string EVENT_NAME_SUBMIT = "submit";
  public const string BOOLEAN_TRUE_VALUE = "true";
  public const string COMPRESSION_TYPE_GZIP = "gzip";
  public const string READY_PARAMETER_IS_READY = "isReady";
  public int ERROR_FAIL = 83;
  public const string PROTOCOL_VERSION = "2.0";
  public static Dictionary<string, _InternalButtonCountState> _buttonStates;
  public static Dictionary<uint, Dictionary<string, _InternalButtonState>> _buttonStatesByParticipant;
  public static Dictionary<string, _InternalJoystickState> _joystickStates;
  public static Dictionary<uint, Dictionary<string, _InternalJoystickState>> _joystickStatesByParticipant;
  public static Dictionary<uint, Dictionary<string, string>> _textboxValuesByParticipant;
  public static Dictionary<uint, _InternalMouseButtonState> _mouseButtonStateByParticipant;
  public static Dictionary<uint, Vector2> _mousePositionsByParticipant;
  public static Dictionary<string, Dictionary<uint, Dictionary<string, object>>> _giveInputControlDataByParticipant;
  public static Dictionary<string, Dictionary<string, object>> _giveInputControlData;
  public static Dictionary<string, object> _giveInputKeyValues;
  public static Dictionary<string, _InternalParticipantTrackingState> _participantsWhoTriggeredGiveInput;
  public static Dictionary<string, Dictionary<string, _InternalControlPropertyUpdateData>> _queuedControlPropertyUpdates;
  public static Dictionary<string, InternalTransactionIDState> _transactionIDsState;
  public static bool useMockData;

  public event InteractivityManager.OnErrorEventHandler OnError;

  public event InteractivityManager.OnInteractivityStateChangedHandler OnInteractivityStateChanged;

  public event InteractivityManager.OnParticipantStateChangedHandler OnParticipantStateChanged;

  public event InteractivityManager.OnInteractiveButtonEventHandler OnInteractiveButtonEvent;

  public event InteractivityManager.OnInteractiveJoystickControlEventHandler OnInteractiveJoystickControlEvent;

  public event InteractivityManager.OnInteractiveMouseButtonEventHandler OnInteractiveMouseButtonEvent;

  public event InteractivityManager.OnInteractiveCoordinatesChangedHandler OnInteractiveCoordinatesChangedEvent;

  public event InteractivityManager.OnInteractiveTextControlEventHandler OnInteractiveTextControlEvent;

  public event InteractivityManager.OnInteractiveMessageEventHandler OnInteractiveMessageEvent;

  public event InteractivityManager.OnInteractiveDoWorkEventHandler OnInteractiveDoWorkEvent;

  public static InteractivityManager SingletonInstance
  {
    get
    {
      if (InteractivityManager._singletonInstance == null)
      {
        InteractivityManager._singletonInstance = new InteractivityManager();
        InteractivityManager._singletonInstance.InitializeInternal();
      }
      return InteractivityManager._singletonInstance;
    }
  }

  public LoggingLevel LoggingLevel
  {
    get => this.\u003CLoggingLevel\u003Ek__BackingField;
    set => this.\u003CLoggingLevel\u003Ek__BackingField = value;
  }

  public string ProjectVersionID
  {
    get => this.\u003CProjectVersionID\u003Ek__BackingField;
    set => this.\u003CProjectVersionID\u003Ek__BackingField = value;
  }

  public string AppID
  {
    get => this.\u003CAppID\u003Ek__BackingField;
    set => this.\u003CAppID\u003Ek__BackingField = value;
  }

  public string ShareCode
  {
    get => this.\u003CShareCode\u003Ek__BackingField;
    set => this.\u003CShareCode\u003Ek__BackingField = value;
  }

  public InteractivityState InteractivityState
  {
    get => this.\u003CInteractivityState\u003Ek__BackingField;
    set => this.\u003CInteractivityState\u003Ek__BackingField = value;
  }

  public IList<InteractiveGroup> Groups
  {
    get
    {
      return (IList<InteractiveGroup>) new List<InteractiveGroup>((IEnumerable<InteractiveGroup>) this._groups);
    }
  }

  public IList<InteractiveScene> Scenes
  {
    get
    {
      return (IList<InteractiveScene>) new List<InteractiveScene>((IEnumerable<InteractiveScene>) this._scenes);
    }
  }

  public IList<InteractiveParticipant> Participants
  {
    get
    {
      return (IList<InteractiveParticipant>) new List<InteractiveParticipant>((IEnumerable<InteractiveParticipant>) this._participants);
    }
  }

  public IList<InteractiveControl> _Controls
  {
    get
    {
      return (IList<InteractiveControl>) new List<InteractiveControl>((IEnumerable<InteractiveControl>) this._controls);
    }
  }

  public IList<InteractiveButtonControl> Buttons
  {
    get
    {
      return (IList<InteractiveButtonControl>) new List<InteractiveButtonControl>((IEnumerable<InteractiveButtonControl>) this._buttons);
    }
  }

  public IList<InteractiveJoystickControl> Joysticks
  {
    get
    {
      return (IList<InteractiveJoystickControl>) new List<InteractiveJoystickControl>((IEnumerable<InteractiveJoystickControl>) this._joysticks);
    }
  }

  public string ShortCode
  {
    get => this.\u003CShortCode\u003Ek__BackingField;
    set => this.\u003CShortCode\u003Ek__BackingField = value;
  }

  public InteractiveGroup GetGroup(string groupID)
  {
    foreach (InteractiveGroup group in this._groups)
    {
      if (group.GroupID == groupID)
        return group;
    }
    return (InteractiveGroup) null;
  }

  public InteractiveScene GetScene(string sceneID)
  {
    foreach (InteractiveScene scene in (IEnumerable<InteractiveScene>) this.Scenes)
    {
      if (scene.SceneID == sceneID)
        return scene;
    }
    return (InteractiveScene) null;
  }

  public void Initialize(bool goInteractive = true, string authToken = "")
  {
    if (this.InteractivityState != InteractivityState.NotInitialized)
      return;
    this.ResetInternalState();
    this.UpdateInteractivityState(InteractivityState.Initializing);
    if (goInteractive)
      this._shouldStartInteractive = true;
    if (!string.IsNullOrEmpty(authToken))
      this._authToken = authToken;
    this.InitiateConnection();
  }

  public void CreateStorageDirectoryIfNotExists()
  {
  }

  public void getWebsocketHosts(string potentialWebsocketUrlsJson)
  {
    this._websocketHosts.Clear();
    this._activeWebsocketHostIndex = 0;
    string empty = string.Empty;
    using (StringReader reader = new StringReader(potentialWebsocketUrlsJson))
    {
      using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) reader))
      {
        while (jsonTextReader.Read())
        {
          if (jsonTextReader.Value != null && jsonTextReader.Value.ToString() == "address")
          {
            jsonTextReader.Read();
            this._websocketHosts.Add(jsonTextReader.Value.ToString());
          }
        }
      }
    }
    this._interactiveWebSocketUrl = this._websocketHosts[this._activeWebsocketHostIndex];
  }

  public void SetWebsocketInstance(Websocket newWebsocket)
  {
  }

  public void InitiateConnection()
  {
    try
    {
      this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestWebSocketHostsCompleted);
      this.mixerInteractiveHelper.OnInternalWebRequestStateChanged += new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestWebSocketHostsCompleted);
      this.mixerInteractiveHelper._MakeWebRequest("OnRequestWebSocketHostsCompleted", "https://mixer.com/api/v1/interactive/hosts");
    }
    catch (Exception ex)
    {
      this._LogError("Error: Could not retrieve the URL for the websocket. Exception details: " + ex.Message);
    }
  }

  public void CompleteInitiateConnection(string websocketHostsResponseString)
  {
    this.getWebsocketHosts(websocketHostsResponseString);
    if (string.IsNullOrEmpty(this.ProjectVersionID) || string.IsNullOrEmpty(this.AppID) && string.IsNullOrEmpty(this.ShareCode))
      this.PopulateConfigData();
    if (!string.IsNullOrEmpty(this._authToken))
    {
      this.VerifyAuthToken();
    }
    else
    {
      this.mixerInteractiveHelper.OnTryGetAuthTokensFromCacheCallback -= new MixerInteractiveHelper.OnTryGetAuthTokensFromCacheCallbackEventHandler(this.OnTryGetAuthTokensFromCacheCallback);
      this.mixerInteractiveHelper.OnTryGetAuthTokensFromCacheCallback += new MixerInteractiveHelper.OnTryGetAuthTokensFromCacheCallbackEventHandler(this.OnTryGetAuthTokensFromCacheCallback);
      this.mixerInteractiveHelper.StartTryGetAuthTokensFromCache();
    }
  }

  public void OnTryGetAuthTokensFromCacheCallback(
    object sender,
    MixerInteractiveHelper.TryGetAuthTokensFromCacheEventArgs e)
  {
    this.mixerInteractiveHelper.OnTryGetAuthTokensFromCacheCallback -= new MixerInteractiveHelper.OnTryGetAuthTokensFromCacheCallbackEventHandler(this.OnTryGetAuthTokensFromCacheCallback);
    this.OnTryGetAuthTokensFromCacheCompleted(e);
  }

  public void OnTryGetAuthTokensFromCacheCompleted(
    MixerInteractiveHelper.TryGetAuthTokensFromCacheEventArgs e)
  {
    this._authToken = e.AuthToken;
    this._oauthRefreshToken = e.RefreshToken;
    if (!string.IsNullOrEmpty(this._authToken))
      this.VerifyAuthToken();
    else
      this.RefreshShortCode();
  }

  public void OnRequestWebSocketHostsCompleted(
    object sender,
    MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs e)
  {
    if (e.RequestID != nameof (OnRequestWebSocketHostsCompleted))
      return;
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestWebSocketHostsCompleted);
    if (e.Succeeded)
      this.CompleteInitiateConnection(e.ResponseText);
    else
      this._LogError("Error: Could not retrieve the URL for the websocket. Exception details: " + e.ErrorMessage);
  }

  public void PopulateConfigData()
  {
    string empty = string.Empty;
    string path = this._streamingAssetsPath + "/interactiveconfig.json";
    string s = File.Exists(path) ? File.ReadAllText(path) : throw new Exception("Error: You need to specify an AppID and ProjectVersionID in the Interactive Editor. You can get to the Interactivity Editor from the Mixer menu (Mixer > Open editor).");
    try
    {
      using (StringReader reader = new StringReader(s))
      {
        using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) reader))
        {
          while (jsonTextReader.Read())
          {
            if (jsonTextReader.Value != null)
            {
              switch (jsonTextReader.Value.ToString().ToLowerInvariant())
              {
                case "appid":
                  jsonTextReader.Read();
                  if (jsonTextReader.Value != null)
                  {
                    this.AppID = jsonTextReader.Value.ToString();
                    continue;
                  }
                  continue;
                case "projectversionid":
                  jsonTextReader.Read();
                  if (jsonTextReader.Value != null)
                  {
                    this.ProjectVersionID = jsonTextReader.Value.ToString();
                    continue;
                  }
                  continue;
                case "sharecode":
                  jsonTextReader.Read();
                  if (jsonTextReader.Value != null)
                  {
                    this.ShareCode = jsonTextReader.Value.ToString();
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
    }
    catch
    {
      this._LogError("Error: interactiveconfig.json file could not be read. Make sure it is valid JSON and has the correct format.");
    }
  }

  public void OnInternalCheckAuthStatusTimerCallback(
    object sender,
    MixerInteractiveHelper.InternalTimerCallbackEventArgs e)
  {
    this.TryGetTokenAsync();
  }

  public void TryGetTokenAsync()
  {
    this._Log("Trying to obtain a new OAuth token. This is an expected and repeated call.");
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestOAuthExchangeTokenCompleted);
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged += new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestOAuthExchangeTokenCompleted);
    this.mixerInteractiveHelper._MakeWebRequest("OnRequestOAuthExchangeTokenCompleted", "https://mixer.com/api/v1/oauth/shortcode/check/" + this._authShortCodeRequestHandle, new Dictionary<string, string>()
    {
      {
        "Content-Type",
        "application/json"
      }
    });
  }

  public void OnRequestOAuthExchangeTokenCompleted(
    object sender,
    MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs e)
  {
    if (e.RequestID != nameof (OnRequestOAuthExchangeTokenCompleted))
      return;
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestOAuthExchangeTokenCompleted);
    if (e.Succeeded)
      this.CompleteRequestOAuthExchangeToken(e.ResponseCode, e.ResponseText);
    else
      this._LogError("Error: Failed to request an OAuth exchange token. Error message: " + e.ErrorMessage);
  }

  public void CompleteRequestOAuthExchangeToken(
    long statusCode,
    string getShortCodeStatusServerResponse)
  {
    if (statusCode != 200L)
    {
      if (statusCode == 204L)
        ;
    }
    else
    {
      string fromStringResponse = this.ParseOAuthExchangeCodeFromStringResponse(getShortCodeStatusServerResponse);
      this.mixerInteractiveHelper.OnInternalCheckAuthStatusTimerCallback -= new MixerInteractiveHelper.OnInternalCheckAuthStatusCallbackEventHandler(this.OnInternalCheckAuthStatusTimerCallback);
      this.mixerInteractiveHelper.StopTimer(MixerInteractiveHelper.InteractiveTimerType.CheckAuthStatus);
      this.GetOauthToken(fromStringResponse);
    }
  }

  public string ParseOAuthExchangeCodeFromStringResponse(string responseText)
  {
    string empty = string.Empty;
    using (StringReader reader = new StringReader(responseText))
    {
      using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) reader))
      {
        while (jsonTextReader.Read())
        {
          if (empty == string.Empty)
          {
            if (jsonTextReader.Value != null && jsonTextReader.Value.ToString() == "code")
            {
              jsonTextReader.Read();
              empty = jsonTextReader.Value.ToString();
            }
          }
          else
            break;
        }
      }
    }
    return empty;
  }

  public void GetOauthToken(string exchangeCode)
  {
    this._Log($"Retrieved an OAuth exchange token. Exchange token: {exchangeCode} Using AppID: {this.AppID} with exchange code: {exchangeCode}");
    string str = $"{{ \"client_id\": \"{this.AppID}\", \"code\": \"{exchangeCode}\", \"grant_type\": \"authorization_code\" }}";
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestOAuthTokenCompleted);
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged += new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestOAuthTokenCompleted);
    MixerInteractiveHelper interactiveHelper = this.mixerInteractiveHelper;
    Dictionary<string, string> headers = new Dictionary<string, string>();
    headers.Add("Content-Type", "application/json");
    string postData = str;
    interactiveHelper._MakeWebRequest("OnRequestOAuthTokenCompleted", "https://mixer.com/api/v1/oauth/token", headers, "POST", postData);
  }

  public void OnRequestOAuthTokenCompleted(
    object sender,
    MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs e)
  {
    if (e.RequestID != nameof (OnRequestOAuthTokenCompleted))
      return;
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestOAuthTokenCompleted);
    if (e.Succeeded)
      this.CompleteGetOAuthToken(e.ResponseCode, e.ResponseText);
    else
      this._LogError("Error: Failed to request an OAuth token. Error message: " + e.ErrorMessage);
  }

  public void CompleteGetOAuthToken(long statusCode, string getCodeServerResponse)
  {
    if (statusCode == 400L)
    {
      this._LogError($"Error: {getCodeServerResponse} while requesting an OAuth token.");
    }
    else
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      using (StringReader reader = new StringReader(getCodeServerResponse))
      {
        using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) reader))
        {
          while (jsonTextReader.Read())
          {
            if (jsonTextReader.Value != null)
            {
              if (jsonTextReader.Value.ToString() == "access_token")
              {
                jsonTextReader.Read();
                empty2 = jsonTextReader.Value.ToString();
              }
              else if (jsonTextReader.Value.ToString() == "refresh_token")
              {
                jsonTextReader.Read();
                empty1 = jsonTextReader.Value.ToString();
              }
            }
          }
        }
      }
      this._authToken = "Bearer " + empty2;
      this._oauthRefreshToken = empty1;
      this.mixerInteractiveHelper.WriteAuthTokensToCache(this._authToken, this._oauthRefreshToken);
      this._Log("Retrieved a new OAuth token. Token: " + this._authToken);
      this.mixerInteractiveHelper.StopTimer(MixerInteractiveHelper.InteractiveTimerType.RefreshShortCode);
      this.mixerInteractiveHelper.StopTimer(MixerInteractiveHelper.InteractiveTimerType.CheckAuthStatus);
      this.ConnectToWebsocket();
    }
  }

  public void RefreshShortCode()
  {
    string str = $"{{ \"client_id\": \"{this.AppID}\", \"scope\": \"interactive:robot:self\" }}";
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestRefresheShortCodeCompleted);
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged += new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestRefresheShortCodeCompleted);
    MixerInteractiveHelper interactiveHelper = this.mixerInteractiveHelper;
    Dictionary<string, string> headers = new Dictionary<string, string>();
    headers.Add("Content-Type", "application/json");
    string postData = str;
    interactiveHelper._MakeWebRequest("OnRequestRefresheShortCodeCompleted", "https://mixer.com/api/v1/oauth/shortcode", headers, "POST", postData);
  }

  public void OnRequestRefresheShortCodeCompleted(
    object sender,
    MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs e)
  {
    if (e.RequestID != nameof (OnRequestRefresheShortCodeCompleted))
      return;
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestRefresheShortCodeCompleted);
    if (e.Succeeded)
    {
      if (e.ResponseCode == 404L)
        this._LogError("Error: OAuth Client ID not found. Make sure the OAuth Client ID you specified in the Unity editor matches the one in Interactive Studio.");
      else
        this.CompleteRefreshShortCode(e.ResponseText);
    }
    else
      this._LogError("Error: Failed to retrieve a short code for short code authentication. Error message: " + e.ErrorMessage);
  }

  public void CompleteRefreshShortCode(string getShortCodeServerResponse)
  {
    int interval = -1;
    using (StringReader reader = new StringReader(getShortCodeServerResponse))
    {
      using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) reader))
      {
        while (jsonTextReader.Read())
        {
          if (jsonTextReader.Value != null)
          {
            switch (jsonTextReader.Value.ToString().ToLowerInvariant())
            {
              case "code":
                jsonTextReader.Read();
                if (jsonTextReader.Value != null)
                {
                  this.ShortCode = jsonTextReader.Value.ToString();
                  continue;
                }
                continue;
              case "expires_in":
                jsonTextReader.Read();
                if (jsonTextReader.Value != null)
                {
                  interval = Convert.ToInt32(jsonTextReader.Value.ToString());
                  continue;
                }
                continue;
              case "handle":
                jsonTextReader.Read();
                if (jsonTextReader.Value != null)
                {
                  this._authShortCodeRequestHandle = jsonTextReader.Value.ToString();
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
    }
    this.mixerInteractiveHelper.OnInternalRefreshShortCodeTimerCallback -= new MixerInteractiveHelper.OnInternalRefreshShortCodeCallbackEventHandler(this.OnInternalRefreshShortCodeTimerCallback);
    this.mixerInteractiveHelper.OnInternalRefreshShortCodeTimerCallback += new MixerInteractiveHelper.OnInternalRefreshShortCodeCallbackEventHandler(this.OnInternalRefreshShortCodeTimerCallback);
    this.mixerInteractiveHelper.StartTimer(MixerInteractiveHelper.InteractiveTimerType.RefreshShortCode, (float) interval);
    this.mixerInteractiveHelper.OnInternalCheckAuthStatusTimerCallback -= new MixerInteractiveHelper.OnInternalCheckAuthStatusCallbackEventHandler(this.OnInternalCheckAuthStatusTimerCallback);
    this.mixerInteractiveHelper.OnInternalCheckAuthStatusTimerCallback += new MixerInteractiveHelper.OnInternalCheckAuthStatusCallbackEventHandler(this.OnInternalCheckAuthStatusTimerCallback);
    this.mixerInteractiveHelper.StartTimer(MixerInteractiveHelper.InteractiveTimerType.CheckAuthStatus, 0.5f);
    this.UpdateInteractivityState(InteractivityState.ShortCodeRequired);
  }

  public void VerifyAuthToken()
  {
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnVerifyAuthTokenRequestCompleted);
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged += new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnVerifyAuthTokenRequestCompleted);
    this.mixerInteractiveHelper._MakeWebRequest("OnVerifyAuthTokenRequestCompleted", this._interactiveWebSocketUrl.Replace("wss", "https"), new Dictionary<string, string>()
    {
      {
        "Authorization",
        this._authToken
      },
      {
        "X-Interactive-Version",
        this.ProjectVersionID
      },
      {
        "X-Protocol-Version",
        "2.0"
      }
    });
  }

  public void OnVerifyAuthTokenRequestCompleted(
    object sender,
    MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs e)
  {
    if (e.RequestID != nameof (OnVerifyAuthTokenRequestCompleted))
      return;
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnVerifyAuthTokenRequestCompleted);
    if (e.Succeeded)
    {
      bool isTokenValid = false;
      if (e.ResponseCode == 401L)
        isTokenValid = false;
      else if (e.ResponseCode == 200L || e.ResponseCode == 400L)
        isTokenValid = true;
      else
        this._LogError("Error: Failed to while trying to validate a cached auth token. Error code: " + e.ResponseCode.ToString());
      this.CompleteVerifyAuthTokenRequestStart(isTokenValid);
    }
    else
      this._LogError("Error: Failed to verify the auth token. Error message: " + e.ErrorMessage);
  }

  public void CompleteVerifyAuthTokenRequestStart(bool isTokenValid)
  {
    if (!isTokenValid)
      this.RefreshAuthToken();
    else
      this.ConnectToWebsocket();
  }

  public void ConnectToWebsocket()
  {
    if (this._pendingConnectToWebSocket || this._websocketConnected)
      return;
    this._pendingConnectToWebSocket = false;
    this._websocketConnected = true;
    string str = string.Empty;
    if (this.ShareCode != string.Empty)
      str = ", Share Code: " + this.ShareCode;
    this._Log($"Connecting to websocket with Project Version ID: {this.ProjectVersionID}{str}, OAuth Client ID: {this.AppID} and Auth Token: {this._authToken}.");
    this._websocket = new WebSocket(this._interactiveWebSocketUrl, Array.Empty<string>());
    NameValueCollection collection = new NameValueCollection();
    collection.Add("Authorization", this._authToken);
    collection.Add("X-Interactive-Version", this.ProjectVersionID);
    collection.Add("X-Protocol-Version", "2.0");
    if (!string.IsNullOrEmpty(this.ShareCode))
      collection.Add("X-Interactive-Sharecode", this.ShareCode);
    this._websocket.SetHeaders(collection);
    this._websocket.OnOpen += new EventHandler(this.OnWebsocketOpen);
    this._websocket.OnMessage += new EventHandler<WebSocketSharp.MessageEventArgs>(this.OnWebSocketMessage);
    this._websocket.OnError += new EventHandler<WebSocketSharp.ErrorEventArgs>(this.OnWebSocketError);
    this._websocket.OnClose += new EventHandler<WebSocketSharp.CloseEventArgs>(this.OnWebSocketClose);
    this._websocket.Connect();
  }

  public void OnWebsocketOpen(object sender, EventArgs args)
  {
    this.mixerInteractiveHelper.StopTimer(MixerInteractiveHelper.InteractiveTimerType.Reconnect);
  }

  public void OnWebSocketMessage(object sender, WebSocketSharp.MessageEventArgs args)
  {
    string empty = string.Empty;
    if (!args.IsText)
      return;
    this.ProcessWebSocketMessage(args.Data);
  }

  public void OnWebSocketError(object sender, WebSocketSharp.ErrorEventArgs args)
  {
    this.UpdateInteractivityState(InteractivityState.InteractivityDisabled);
    this._LogError("Error: Websocket OnError: " + args.Message);
  }

  public void OnWebSocketClose(object sender, WebSocketSharp.CloseEventArgs args)
  {
    this.UpdateInteractivityState(InteractivityState.InteractivityDisabled);
    if (args.Code == (ushort) 4019)
      this._LogError("Connection failed (error code 4019): You don't have access to this project. Make sure that the account you are signed in with has access to this Version ID. If you are using a share code, make sure that the share code value matches the one in Interactive Studio for this project.");
    else if (args.Code == (ushort) 4020)
      this._LogError("Connection failed (error code 4020): The interactive version was not found or you do not have access to it. Make sure that the account you are signed in with has access to this Version ID. If you are using a share code, make sure that the share code value matches the one in Interactive Studio for this project.");
    else if (args.Code == (ushort) 4021)
    {
      this._LogError("Connection failed (error code 4021): You are connected to this session somewhere else. Please disconnect from that session and try again.");
    }
    else
    {
      this._pendingConnectToWebSocket = false;
      this._websocketConnected = false;
      ++this._activeWebsocketHostIndex;
      this._interactiveWebSocketUrl = this._websocketHosts[this._activeWebsocketHostIndex];
      this.mixerInteractiveHelper.OnInternalReconnectTimerCallback -= new MixerInteractiveHelper.OnInternalReconnectCallbackEventHandler(this.OnInternalReconnectTimerCallback);
      this.mixerInteractiveHelper.OnInternalReconnectTimerCallback += new MixerInteractiveHelper.OnInternalReconnectCallbackEventHandler(this.OnInternalReconnectTimerCallback);
      this.mixerInteractiveHelper.StartTimer(MixerInteractiveHelper.InteractiveTimerType.Reconnect, 0.5f);
    }
  }

  public void RefreshAuthToken()
  {
    string str = $"{{ \"client_id\": \"{this.AppID}\", \"refresh_token\": \"{this._oauthRefreshToken}\", \"grant_type\": \"refresh_token\" }}";
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestRefreshedAuthTokenCompleted);
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged += new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestRefreshedAuthTokenCompleted);
    MixerInteractiveHelper interactiveHelper = this.mixerInteractiveHelper;
    Dictionary<string, string> headers = new Dictionary<string, string>();
    headers.Add("Content-Type", "application/json");
    string postData = str;
    interactiveHelper._MakeWebRequest("OnRequestRefreshedAuthTokenCompleted", "https://mixer.com/api/v1/oauth/token", headers, "POST", postData);
  }

  public void OnRequestRefreshedAuthTokenCompleted(
    object sender,
    MixerInteractiveHelper._InternalWebRequestStateChangedEventArgs e)
  {
    if (e.RequestID != nameof (OnRequestRefreshedAuthTokenCompleted))
      return;
    this.mixerInteractiveHelper.OnInternalWebRequestStateChanged -= new MixerInteractiveHelper.OnInternalWebRequestStateChangedEventHandler(this.OnRequestRefreshedAuthTokenCompleted);
    if (e.Succeeded)
      this.CompleteRefreshAuthToken(e.ResponseCode, e.ResponseText);
    else
      this._LogError("Error: Web request to refresh the Auth token failed. Error message: " + e.ErrorMessage);
  }

  public void CompleteRefreshAuthToken(long statusCode, string getCodeServerResponse)
  {
    if (statusCode == 400L)
      this._LogError($"Error: {getCodeServerResponse} trying to refresh the auth token.");
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    using (StringReader reader = new StringReader(getCodeServerResponse))
    {
      using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) reader))
      {
        while (jsonTextReader.Read())
        {
          if (jsonTextReader.Value != null)
          {
            if (jsonTextReader.Value.ToString() == "access_token")
            {
              jsonTextReader.Read();
              empty1 = jsonTextReader.Value.ToString();
            }
            else if (jsonTextReader.Value.ToString() == "refresh_token")
            {
              jsonTextReader.Read();
              empty2 = jsonTextReader.Value.ToString();
            }
          }
        }
      }
    }
    this._authToken = "Bearer " + empty1;
    this._oauthRefreshToken = empty2;
    this.mixerInteractiveHelper.WriteAuthTokensToCache(this._authToken, this._oauthRefreshToken);
    this.VerifyAuthToken();
  }

  public void UpdateInteractivityState(InteractivityState state)
  {
    this.InteractivityState = state;
    this._queuedEvents.Add((InteractiveEventArgs) new InteractivityStateChangedEventArgs(InteractiveEventType.InteractivityStateChanged, state));
  }

  public InteractiveControl ControlFromControlID(string controlID)
  {
    foreach (InteractiveControl control in (IEnumerable<InteractiveControl>) this._Controls)
    {
      if (control.ControlID == controlID)
        return control;
    }
    return (InteractiveControl) null;
  }

  public void CaptureTransaction(string transactionID)
  {
    if (string.IsNullOrEmpty(transactionID))
      return;
    this._SendCaptureTransactionMessage(transactionID);
  }

  public void TriggerCooldown(string controlID, int cooldown)
  {
    if (this.InteractivityState != InteractivityState.InteractivityEnabled)
      throw new Exception("Error: The InteractivityManager's InteractivityState must be InteractivityEnabled before calling this method.");
    if (cooldown < 1000)
      this._Log($"Info: Did you mean to use a cooldown of {((float) cooldown / 1000f).ToString()} seconds? Remember, cooldowns are in milliseconds.");
    string empty = string.Empty;
    string str = string.Empty;
    InteractiveControl interactiveControl = this.ControlFromControlID(controlID);
    if (interactiveControl != null)
    {
      if (interactiveControl is InteractiveButtonControl)
      {
        str = interactiveControl._sceneID;
      }
      else
      {
        this._LogError("Error: The control is not a button. You can only trigger a cooldown on a button.");
        return;
      }
    }
    long num = (long) Math.Truncate(DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds + (double) cooldown);
    if (interactiveControl is InteractiveButtonControl interactiveButtonControl)
      interactiveButtonControl._cooldownExpirationTime = num;
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("updateControls");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("sceneID");
      jsonWriter.WriteValue(str);
      jsonWriter.WritePropertyName("controls");
      jsonWriter.WriteStartArray();
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName(nameof (controlID));
      jsonWriter.WriteValue(controlID);
      jsonWriter.WritePropertyName("etag");
      jsonWriter.WriteValue(empty);
      jsonWriter.WritePropertyName(nameof (cooldown));
      jsonWriter.WriteValue(num);
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEndArray();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "updateControls");
  }

  public void SendMessage(string message) => this.SendJsonString(message);

  public void SendMessage(string messageType, Dictionary<string, object> parameters)
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue(messageType);
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      foreach (string key in parameters.Keys)
      {
        jsonWriter.WritePropertyName(key);
        jsonWriter.WriteValue(parameters[key].ToString());
      }
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, messageType);
  }

  public void StartInteractive()
  {
    if (this.InteractivityState == InteractivityState.NotInitialized)
      MixerInteractive.GoInteractive();
    if (this.InteractivityState == InteractivityState.Initializing || this.InteractivityState == InteractivityState.ShortCodeRequired || this.InteractivityState == InteractivityState.InteractivityPending || this.InteractivityState == InteractivityState.InteractivityEnabled)
      return;
    this.SendReady(true);
    this._shouldStartInteractive = false;
    this.UpdateInteractivityState(InteractivityState.InteractivityPending);
  }

  public void StopInteractive()
  {
    if (this.InteractivityState == InteractivityState.NotInitialized || this.InteractivityState == InteractivityState.InteractivityDisabled)
      return;
    this.UpdateInteractivityState(InteractivityState.InteractivityDisabled);
    this.SendReady(false);
    this._queuedEvents.Add(new InteractiveEventArgs(InteractiveEventType.InteractivityStateChanged));
  }

  public void DoWork()
  {
    this.ClearPreviousControlState();
    this.RaiseQueuedInteractiveEvents();
    this.SendQueuedSetControlPropertyUpdates();
  }

  public void RaiseQueuedInteractiveEvents()
  {
    foreach (InteractiveEventArgs e in this._queuedEvents.ToArray())
    {
      switch (e.EventType)
      {
        case InteractiveEventType.Error:
          if (this.OnError != null)
          {
            this.OnError((object) this, e);
            break;
          }
          break;
        case InteractiveEventType.InteractivityStateChanged:
          if (this.OnInteractivityStateChanged != null)
          {
            this.OnInteractivityStateChanged((object) this, e as InteractivityStateChangedEventArgs);
            break;
          }
          break;
        case InteractiveEventType.ParticipantStateChanged:
          if (this.OnParticipantStateChanged != null)
          {
            this.OnParticipantStateChanged((object) this, e as InteractiveParticipantStateChangedEventArgs);
            break;
          }
          break;
        case InteractiveEventType.Button:
          if (this.OnInteractiveButtonEvent != null)
          {
            this.OnInteractiveButtonEvent((object) this, e as InteractiveButtonEventArgs);
            break;
          }
          break;
        case InteractiveEventType.Joystick:
          if (this.OnInteractiveJoystickControlEvent != null)
          {
            this.OnInteractiveJoystickControlEvent((object) this, e as InteractiveJoystickEventArgs);
            break;
          }
          break;
        case InteractiveEventType.MouseButton:
          if (this.OnInteractiveMouseButtonEvent != null)
          {
            this.OnInteractiveMouseButtonEvent((object) this, e as InteractiveMouseButtonEventArgs);
            break;
          }
          break;
        case InteractiveEventType.Coordinates:
          if (this.OnInteractiveCoordinatesChangedEvent != null)
          {
            this.OnInteractiveCoordinatesChangedEvent((object) this, e as InteractiveCoordinatesChangedEventArgs);
            break;
          }
          break;
        case InteractiveEventType.TextInput:
          if (this.OnInteractiveTextControlEvent != null)
          {
            this.OnInteractiveTextControlEvent((object) this, e as InteractiveTextEventArgs);
            break;
          }
          break;
        default:
          if (this.OnInteractiveMessageEvent != null)
          {
            this.OnInteractiveMessageEvent((object) this, e as InteractiveMessageEventArgs);
            break;
          }
          break;
      }
    }
    this._queuedEvents.Clear();
    if (this.OnInteractiveDoWorkEvent == null)
      return;
    this.OnInteractiveDoWorkEvent((object) this, new InteractiveEventArgs());
  }

  public void SendQueuedSetControlPropertyUpdates()
  {
    foreach (string key1 in InteractivityManager._queuedControlPropertyUpdates.Keys)
    {
      uint messageID = this._currentmessageID++;
      StringWriter stringWriter = new StringWriter(new StringBuilder());
      using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
      {
        jsonWriter.WriteStartObject();
        jsonWriter.WritePropertyName("type");
        jsonWriter.WriteValue("method");
        jsonWriter.WritePropertyName("id");
        jsonWriter.WriteValue(messageID);
        jsonWriter.WritePropertyName("method");
        jsonWriter.WriteValue("updateControls");
        jsonWriter.WritePropertyName("params");
        jsonWriter.WriteStartObject();
        jsonWriter.WritePropertyName("sceneID");
        jsonWriter.WriteValue(key1);
        jsonWriter.WritePropertyName("controls");
        jsonWriter.WriteStartArray();
        foreach (string key2 in InteractivityManager._queuedControlPropertyUpdates[key1].Keys)
        {
          jsonWriter.WriteStartObject();
          jsonWriter.WritePropertyName("controlID");
          jsonWriter.WriteValue(key2);
          Dictionary<string, _InternalControlPropertyMetaData> properties = InteractivityManager._queuedControlPropertyUpdates[key1][key2].properties;
          foreach (string key3 in properties.Keys)
          {
            jsonWriter.WritePropertyName(key3);
            _InternalControlPropertyMetaData propertyMetaData = properties[key3];
            if (propertyMetaData.type == _KnownControlPropertyPrimitiveTypes.Boolean)
              jsonWriter.WriteValue(propertyMetaData.boolValue);
            else if (propertyMetaData.type == _KnownControlPropertyPrimitiveTypes.Number)
              jsonWriter.WriteValue(propertyMetaData.numberValue);
            else
              jsonWriter.WriteValue(propertyMetaData.stringValue);
          }
          jsonWriter.WriteEndObject();
        }
        jsonWriter.WriteEndArray();
        jsonWriter.WriteEndObject();
        jsonWriter.WriteEnd();
        this.SendJsonString(stringWriter.ToString());
      }
      this.StoreIfExpectingReply(messageID, "updateControls");
    }
    InteractivityManager._queuedControlPropertyUpdates.Clear();
  }

  public void Dispose()
  {
    if (this._disposed)
      return;
    this.ResetInternalState();
    this.mixerInteractiveHelper.Dispose();
    if (this._websocket != null)
    {
      this._websocket.OnOpen -= new EventHandler(this.OnWebsocketOpen);
      this._websocket.OnMessage -= new EventHandler<WebSocketSharp.MessageEventArgs>(this.OnWebSocketMessage);
      this._websocket.OnError -= new EventHandler<WebSocketSharp.ErrorEventArgs>(this.OnWebSocketError);
      this._websocket.OnClose -= new EventHandler<WebSocketSharp.CloseEventArgs>(this.OnWebSocketClose);
      this._websocket.Close();
    }
    this._disposed = true;
  }

  public void SendMockWebSocketMessage(string rawText) => this.ProcessWebSocketMessage(rawText);

  public void ProcessWebSocketMessage(string messageText)
  {
    try
    {
      using (StringReader reader = new StringReader(messageText))
      {
        using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) reader))
        {
          int messageIDAsInt = -1;
          string empty = string.Empty;
          while (jsonTextReader.Read())
          {
            if (jsonTextReader.Value != null)
            {
              if (jsonTextReader.Value.ToString() == "id")
              {
                jsonTextReader.ReadAsInt32();
                messageIDAsInt = Convert.ToInt32(jsonTextReader.Value);
              }
              if (jsonTextReader.Value.ToString() == "type")
              {
                jsonTextReader.Read();
                if (jsonTextReader.Value != null)
                {
                  switch (jsonTextReader.Value.ToString())
                  {
                    case "method":
                      this.ProcessMethod((JsonReader) jsonTextReader);
                      continue;
                    case "reply":
                      this.ProcessReply((JsonReader) jsonTextReader, messageIDAsInt);
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
          }
        }
      }
    }
    catch
    {
      this._LogError("Error: Failed to process message: " + messageText);
    }
    this._Log(messageText);
    this._queuedEvents.Add((InteractiveEventArgs) new InteractiveMessageEventArgs(messageText));
  }

  public void ProcessMethod(JsonReader jsonReader)
  {
    try
    {
      while (jsonReader.Read())
      {
        if (jsonReader.Value != null)
        {
          string s = jsonReader.Value.ToString();
          try
          {
            if (s != null)
            {
              // ISSUE: reference to a compiler-generated method
              switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
              {
                case 504300807:
                  if (s == "onReady")
                  {
                    this.HandleInteractivityStarted(jsonReader);
                    continue;
                  }
                  continue;
                case 787791708:
                  if (s == "onParticipantUpdate")
                  {
                    this.HandleParticipantUpdate(jsonReader);
                    continue;
                  }
                  continue;
                case 828311875:
                  if (s == "onGroupCreate")
                  {
                    this.HandleGroupCreate(jsonReader);
                    continue;
                  }
                  continue;
                case 1335831723:
                  if (s == "hello")
                  {
                    this.HandleHelloMessage();
                    continue;
                  }
                  continue;
                case 1344240129:
                  if (s == "onParticipantJoin")
                  {
                    this.HandleParticipantJoin(jsonReader);
                    continue;
                  }
                  continue;
                case 3006607853:
                  if (s == "onControlCreate")
                    break;
                  continue;
                case 3104973596:
                  if (s == "onSceneCreate")
                  {
                    this.HandleSceneCreate(jsonReader);
                    continue;
                  }
                  continue;
                case 3344543728:
                  if (s == "giveInput")
                  {
                    this.HandleGiveInput(jsonReader);
                    continue;
                  }
                  continue;
                case 3428279784:
                  if (s == "onParticipantLeave")
                  {
                    this.HandleParticipantLeave(jsonReader);
                    continue;
                  }
                  continue;
                case 3801413412:
                  if (s == "onControlUpdate")
                    break;
                  continue;
                case 4246794162:
                  if (s == "onGroupUpdate")
                  {
                    this.HandleGroupUpdate(jsonReader);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
              this.HandleControlUpdate(jsonReader);
            }
          }
          catch (Exception ex)
          {
            this._LogError($"Error: Error while processing method: {s}. Error message: {ex.Message}");
          }
        }
      }
    }
    catch (Exception ex)
    {
      this._LogError("Error: Error processing websocket message. Error message: " + ex.Message);
    }
  }

  public void ProcessReply(JsonReader jsonReader, int messageIDAsInt)
  {
    uint key = 0;
    if (messageIDAsInt != -1)
    {
      key = Convert.ToUInt32(messageIDAsInt);
    }
    else
    {
      try
      {
        while (jsonReader.Read())
        {
          if (jsonReader.Value != null && jsonReader.Value.ToString() == "id")
            key = (uint) jsonReader.ReadAsInt32().Value;
        }
      }
      catch
      {
        this._LogError("Error: Failed to get the message ID from the reply message.");
      }
    }
    string empty = string.Empty;
    this._outstandingMessages.TryGetValue(key, out empty);
    try
    {
      switch (empty)
      {
        case "getAllParticipants":
          this.HandleGetAllParticipants(jsonReader);
          break;
        case "getGroups":
          this.HandleGetGroups(jsonReader);
          break;
        case "getScenes":
          this.HandleGetScenes(jsonReader);
          break;
        case "setCurrentScene":
          this.HandlePossibleError(jsonReader);
          break;
      }
    }
    catch
    {
      this._LogError("Error: An error occured while processing the reply: " + empty);
    }
  }

  public void HandlePossibleError(JsonReader jsonReader)
  {
    int code = 0;
    string message = string.Empty;
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null)
      {
        switch (jsonReader.Value.ToString())
        {
          case "code":
            jsonReader.ReadAsInt32();
            code = Convert.ToInt32(jsonReader.Value);
            continue;
          case "message":
            jsonReader.Read();
            if (jsonReader.Value != null)
            {
              message = $"{message} Message: {jsonReader.Value.ToString()}";
              continue;
            }
            continue;
          case "path":
            jsonReader.Read();
            if (jsonReader.Value != null)
            {
              message = $"{message} Path: {jsonReader.Value.ToString()}";
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    if (code == 0 || !(message != string.Empty))
      return;
    this._LogError(message, code);
  }

  public void ResetInternalState()
  {
    this._disposed = false;
    this._initializedGroups = false;
    this._initializedScenes = false;
    this._shouldStartInteractive = false;
    this._pendingConnectToWebSocket = false;
    this._websocketConnected = false;
    this.UpdateInteractivityState(InteractivityState.NotInitialized);
  }

  public void HandleHelloMessage()
  {
    this.SendGetAllGroupsMessage();
    this.SendGetAllScenesMessage();
  }

  public void HandleInteractivityStarted(JsonReader jsonReader)
  {
    bool flag = false;
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null && jsonReader.Value.ToString() == "isReady")
      {
        jsonReader.ReadAsBoolean();
        if (jsonReader.Value != null)
        {
          flag = (bool) jsonReader.Value;
          break;
        }
      }
    }
    if (!flag)
      return;
    this.UpdateInteractivityState(InteractivityState.InteractivityEnabled);
  }

  public void HandleControlUpdate(JsonReader jsonReader)
  {
    string empty = string.Empty;
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null)
      {
        switch (jsonReader.Value.ToString())
        {
          case "sceneID":
            jsonReader.Read();
            empty = jsonReader.Value.ToString();
            continue;
          case "controls":
            this.UpdateControls(jsonReader, empty);
            continue;
          default:
            continue;
        }
      }
    }
  }

  public void UpdateControls(JsonReader jsonReader, string sceneID)
  {
    try
    {
      while (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray)
      {
        if (jsonReader.TokenType == JsonToken.StartObject)
        {
          InteractiveControl interactiveControl1 = this.ReadControl(jsonReader, sceneID);
          InteractiveControl interactiveControl2 = (InteractiveControl) null;
          foreach (InteractiveControl control in (IEnumerable<InteractiveControl>) this._Controls)
          {
            if (control.ControlID == interactiveControl1.ControlID)
            {
              interactiveControl2 = control;
              break;
            }
          }
          if (interactiveControl1 is InteractiveButtonControl interactiveButtonControl1)
          {
            if (interactiveControl2 is InteractiveButtonControl interactiveButtonControl)
              this._buttons.Remove(interactiveButtonControl);
            this._buttons.Add(interactiveButtonControl1);
          }
          if (interactiveControl1 is InteractiveJoystickControl interactiveJoystickControl1)
          {
            if (interactiveControl2 is InteractiveJoystickControl interactiveJoystickControl)
              this._joysticks.Remove(interactiveJoystickControl);
            this._joysticks.Add(interactiveJoystickControl1);
          }
          if (interactiveControl2 != null)
            this._controls.Remove(interactiveControl2);
          this._controls.Add(interactiveControl1);
        }
      }
    }
    catch
    {
      this._LogError($"Error: Failed reading controls for scene: {sceneID}.");
    }
  }

  public void HandleSceneCreate(JsonReader jsonReader)
  {
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null && jsonReader.Value.ToString() == "scenes")
        this._scenes.Add(this.ReadScene(jsonReader));
    }
  }

  public void HandleGroupCreate(JsonReader jsonReader) => this.ProcessGroups(jsonReader);

  public void HandleGroupUpdate(JsonReader jsonReader) => this.ProcessGroups(jsonReader);

  public void ProcessGroups(JsonReader jsonReader)
  {
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null && jsonReader.Value.ToString() == "groups")
      {
        InteractiveGroup source = this.ReadGroup(jsonReader);
        IList<InteractiveGroup> groups = this.Groups;
        int index1 = -1;
        for (int index2 = 0; index2 < groups.Count; ++index2)
        {
          if (groups[index2].GroupID == source.GroupID)
          {
            index1 = index2;
            break;
          }
        }
        if (index1 != -1)
          this.CloneGroupValues(source, groups[index1]);
        else
          this._groups.Add(source);
      }
    }
  }

  public void CloneGroupValues(InteractiveGroup source, InteractiveGroup destination)
  {
    destination._etag = source._etag;
    destination.SceneID = source.SceneID;
    destination.GroupID = source.GroupID;
  }

  public void HandleGetAllParticipants(JsonReader jsonReader)
  {
    while (jsonReader.Read())
    {
      if (jsonReader.TokenType == JsonToken.StartObject)
        this._participants.Add(this.ReadParticipant(jsonReader));
    }
  }

  public List<InteractiveParticipant> ReadParticipants(JsonReader jsonReader)
  {
    List<InteractiveParticipant> interactiveParticipantList = new List<InteractiveParticipant>();
    while (jsonReader.Read())
    {
      if (jsonReader.TokenType == JsonToken.StartObject)
      {
        InteractiveParticipant source = this.ReadParticipant(jsonReader);
        IList<InteractiveParticipant> participants = this.Participants;
        int index1 = -1;
        for (int index2 = 0; index2 < participants.Count; ++index2)
        {
          if ((int) participants[index2].UserID == (int) source.UserID)
            index1 = index2;
        }
        if (index1 != -1)
          this.CloneParticipantValues(source, participants[index1]);
        else
          this._participants.Add(source);
        interactiveParticipantList.Add(source);
      }
    }
    return interactiveParticipantList;
  }

  public void CloneParticipantValues(
    InteractiveParticipant source,
    InteractiveParticipant destination)
  {
    destination._sessionID = source._sessionID;
    destination.UserID = source.UserID;
    destination.UserName = source.UserName;
    destination.Level = source.Level;
    destination.LastInputAt = source.LastInputAt;
    destination.ConnectedAt = source.ConnectedAt;
    destination.InputDisabled = source.InputDisabled;
    destination.State = source.State;
    destination._groupID = source._groupID;
    destination._etag = source._etag;
  }

  public InteractiveParticipant ReadParticipant(JsonReader jsonReader)
  {
    uint userID = 0;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    string empty4 = string.Empty;
    uint level = 0;
    bool inputDisabled = false;
    List<string> newChannelGroups = new List<string>();
    DateTime lastInputAt = new DateTime();
    DateTime connectedAt = new DateTime();
    int depth = jsonReader.Depth;
label_17:
    DateTime dateTime;
    while (jsonReader.Read() && jsonReader.Depth > depth)
    {
      if (jsonReader.Value != null && jsonReader.Value != null)
      {
        switch (jsonReader.Value.ToString())
        {
          case "channelGroups":
            while (true)
            {
              do
              {
                if (!jsonReader.Read() || jsonReader.TokenType == JsonToken.EndArray)
                  goto label_17;
              }
              while (jsonReader.Value == null);
              newChannelGroups.Add(jsonReader.Value.ToString());
            }
          case "connectedAt":
            jsonReader.Read();
            dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToDouble(jsonReader.Value));
            connectedAt = dateTime.ToLocalTime();
            continue;
          case "disabled":
            jsonReader.ReadAsBoolean();
            inputDisabled = (bool) jsonReader.Value;
            continue;
          case "etag":
            jsonReader.Read();
            if (jsonReader.Value != null)
            {
              empty2 = jsonReader.Value.ToString();
              continue;
            }
            continue;
          case "groupID":
            jsonReader.Read();
            empty4 = jsonReader.Value.ToString();
            continue;
          case "lastInputAt":
            jsonReader.Read();
            dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToDouble(jsonReader.Value));
            lastInputAt = dateTime.ToLocalTime();
            continue;
          case "level":
            jsonReader.Read();
            level = Convert.ToUInt32(jsonReader.Value);
            continue;
          case "sessionID":
            jsonReader.Read();
            if (jsonReader.Value != null)
            {
              empty1 = jsonReader.Value.ToString();
              continue;
            }
            continue;
          case "userID":
            jsonReader.ReadAsInt32();
            userID = Convert.ToUInt32(jsonReader.Value);
            continue;
          case "username":
            jsonReader.Read();
            empty3 = jsonReader.Value.ToString();
            continue;
          default:
            continue;
        }
      }
    }
    InteractiveParticipantState state = inputDisabled ? InteractiveParticipantState.InputDisabled : InteractiveParticipantState.Joined;
    return new InteractiveParticipant(empty1, empty2, userID, empty4, empty3, newChannelGroups, level, lastInputAt, connectedAt, inputDisabled, state);
  }

  public void HandleGetGroups(JsonReader jsonReader)
  {
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null && jsonReader.Value.ToString() == "groups")
      {
        InteractiveGroup source = this.ReadGroup(jsonReader);
        IList<InteractiveGroup> groups = this.Groups;
        int index1 = -1;
        for (int index2 = 0; index2 < groups.Count; ++index2)
        {
          if (groups[index2].GroupID == source.GroupID)
          {
            index1 = index2;
            break;
          }
        }
        if (index1 != -1)
          this.CloneGroupValues(source, groups[index1]);
        else
          this._groups.Add(source);
      }
    }
    this._initializedGroups = true;
    if (!this._initializedGroups || !this._initializedScenes)
      return;
    this.UpdateInteractivityState(InteractivityState.Initialized);
    if (!this._shouldStartInteractive)
      return;
    this.StartInteractive();
  }

  public InteractiveGroup ReadGroup(JsonReader jsonReader)
  {
    int depth = jsonReader.Depth;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    jsonReader.Read();
    while (jsonReader.Read() && jsonReader.Depth > depth)
    {
      if (jsonReader.Value != null)
      {
        switch (jsonReader.Value.ToString())
        {
          case "etag":
            jsonReader.ReadAsString();
            if (jsonReader.Value != null)
            {
              empty1 = jsonReader.Value.ToString();
              continue;
            }
            continue;
          case "sceneID":
            jsonReader.ReadAsString();
            if (jsonReader.Value != null)
            {
              empty2 = jsonReader.Value.ToString();
              continue;
            }
            continue;
          case "groupID":
            jsonReader.ReadAsString();
            if (jsonReader.Value != null)
            {
              empty3 = jsonReader.Value.ToString();
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    return new InteractiveGroup(empty1, empty2, empty3);
  }

  public Dictionary<string, object> ReadMetaProperties(JsonReader jsonReader)
  {
    Dictionary<string, object> metaProperties = new Dictionary<string, object>();
    while (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndObject)
    {
      if (jsonReader.Value != null)
      {
        string metaPropertyKey = jsonReader.Value.ToString();
        this.ReadMetaProperty(jsonReader, metaPropertyKey, metaProperties);
      }
    }
    return metaProperties;
  }

  public void ReadMetaProperty(
    JsonReader jsonReader,
    string metaPropertyKey,
    Dictionary<string, object> metaProperties)
  {
    string empty = string.Empty;
    while (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndObject && !(empty != string.Empty))
    {
      if (jsonReader.Value != null && jsonReader.Value.ToString() == "value")
      {
        jsonReader.Read();
        if (jsonReader.Value != null)
          empty = jsonReader.Value.ToString();
      }
    }
    metaProperties.Add(metaPropertyKey, (object) empty);
  }

  public void HandleGetScenes(JsonReader jsonReader)
  {
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null && jsonReader.Value.ToString() == "scenes")
        this._scenes = this.ReadScenes(jsonReader);
    }
    this._initializedScenes = true;
    if (!this._initializedGroups || !this._initializedScenes)
      return;
    this.UpdateInteractivityState(InteractivityState.Initialized);
    if (!this._shouldStartInteractive)
      return;
    this.StartInteractive();
  }

  public List<InteractiveScene> ReadScenes(JsonReader jsonReader)
  {
    List<InteractiveScene> interactiveSceneList = new List<InteractiveScene>();
    while (jsonReader.Read())
    {
      if (jsonReader.TokenType == JsonToken.StartObject)
        interactiveSceneList.Add(this.ReadScene(jsonReader));
    }
    return interactiveSceneList;
  }

  public InteractiveScene ReadScene(JsonReader jsonReader)
  {
    InteractiveScene scene = new InteractiveScene();
    try
    {
      int depth = jsonReader.Depth;
      while (jsonReader.Read())
      {
        if (jsonReader.Depth > depth)
        {
          if (jsonReader.Value != null)
          {
            switch (jsonReader.Value.ToString())
            {
              case "sceneID":
                jsonReader.ReadAsString();
                if (jsonReader.Value != null)
                {
                  scene.SceneID = jsonReader.Value.ToString();
                  continue;
                }
                continue;
              case "etag":
                jsonReader.ReadAsString();
                if (jsonReader.Value != null)
                {
                  scene._etag = jsonReader.Value.ToString();
                  continue;
                }
                continue;
              case "controls":
                this.ReadControls(jsonReader, scene);
                continue;
              default:
                continue;
            }
          }
        }
        else
          break;
      }
    }
    catch
    {
      this._LogError($"Error: Error reading scene {scene.SceneID}.");
    }
    return scene;
  }

  public void ReadControls(JsonReader jsonReader, InteractiveScene scene)
  {
    try
    {
      while (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray)
      {
        if (jsonReader.TokenType == JsonToken.StartObject)
        {
          InteractiveControl interactiveControl = this.ReadControl(jsonReader, scene.SceneID);
          if (interactiveControl is InteractiveButtonControl interactiveButtonControl)
            this._buttons.Add(interactiveButtonControl);
          if (interactiveControl is InteractiveJoystickControl interactiveJoystickControl)
            this._joysticks.Add(interactiveJoystickControl);
          this._controls.Add(interactiveControl);
        }
      }
    }
    catch
    {
      this._LogError($"Error: Failed reading controls for scene: {scene.SceneID}.");
    }
  }

  public InteractiveControl ReadControl(JsonReader jsonReader, string sceneID = "")
  {
    int depth = jsonReader.Depth;
    string empty1 = string.Empty;
    uint cost = 0;
    bool flag = false;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    string empty4 = string.Empty;
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    try
    {
      while (jsonReader.Read())
      {
        if (jsonReader.Depth > depth)
        {
          if (jsonReader.Value != null)
          {
            switch (jsonReader.Value.ToString())
            {
              case "controlID":
                jsonReader.ReadAsString();
                empty1 = jsonReader.Value.ToString();
                continue;
              case "cost":
                jsonReader.ReadAsInt32();
                cost = Convert.ToUInt32(jsonReader.Value);
                continue;
              case "disabled":
                jsonReader.ReadAsBoolean();
                flag = (bool) jsonReader.Value;
                continue;
              case "etag":
                jsonReader.Read();
                empty3 = jsonReader.Value.ToString();
                continue;
              case "kind":
                jsonReader.Read();
                empty4 = jsonReader.Value.ToString();
                continue;
              case "meta":
                while (jsonReader.Read())
                {
                  if (jsonReader.TokenType == JsonToken.StartObject)
                  {
                    dictionary = this.ReadMetaProperties(jsonReader);
                    break;
                  }
                }
                continue;
              case "text":
                jsonReader.Read();
                empty2 = jsonReader.Value.ToString();
                continue;
              default:
                continue;
            }
          }
        }
        else
          break;
      }
    }
    catch
    {
      this._LogError($"Error: Error reading control {empty1}.");
    }
    InteractiveControl interactiveControl;
    switch (empty4)
    {
      case "button":
        interactiveControl = (InteractiveControl) new InteractiveButtonControl(empty1, InteractiveEventType.Button, flag, empty2, cost, empty3, sceneID, dictionary);
        break;
      case "joystick":
        interactiveControl = (InteractiveControl) new InteractiveJoystickControl(empty1, InteractiveEventType.Joystick, flag, empty2, empty3, sceneID, dictionary);
        break;
      case "textbox":
        interactiveControl = (InteractiveControl) new InteractiveTextControl(empty1, InteractiveEventType.TextInput, flag, empty2, empty3, sceneID, dictionary);
        break;
      case "label":
        interactiveControl = (InteractiveControl) new InteractiveLabelControl(empty1, empty2, sceneID);
        break;
      default:
        interactiveControl = new InteractiveControl(empty1, empty4, InteractiveEventType.Unknown, flag, empty2, empty3, sceneID, dictionary);
        break;
    }
    return interactiveControl;
  }

  public InteractivityManager._InputEvent ReadInputObject(JsonReader jsonReader)
  {
    InteractivityManager._InputEvent inputEvent = new InteractivityManager._InputEvent();
    while (jsonReader.Read())
    {
      if (jsonReader.TokenType == JsonToken.StartObject)
        inputEvent = this.ReadInputInnerObject(jsonReader);
    }
    return inputEvent;
  }

  public InteractivityManager._InputEvent ReadInputInnerObject(JsonReader jsonReader)
  {
    int depth = jsonReader.Depth;
    string empty1 = string.Empty;
    string eventName = string.Empty;
    object obj = (object) null;
    bool isPressed = false;
    float x = 0.0f;
    float y = 0.0f;
    string empty2 = string.Empty;
    try
    {
      while (jsonReader.Read())
      {
        if (jsonReader.Depth > depth)
        {
          if (jsonReader.Value != null)
          {
            string str = jsonReader.Value.ToString();
            switch (str)
            {
              case "controlID":
                jsonReader.ReadAsString();
                if (jsonReader.Value != null)
                {
                  empty1 = jsonReader.Value.ToString();
                  break;
                }
                break;
              case "event":
                eventName = jsonReader.ReadAsString();
                if (eventName == "mousedown" || eventName == "mouseup" || eventName == "keydown" || eventName == "keyup")
                {
                  if (eventName == "mousedown" || eventName == "keydown")
                  {
                    isPressed = true;
                    break;
                  }
                  if (eventName == "mouseup" || eventName == "keyup")
                  {
                    isPressed = false;
                    break;
                  }
                  break;
                }
                break;
              case "x":
                x = (float) jsonReader.ReadAsDouble().Value;
                break;
              case "y":
                y = (float) jsonReader.ReadAsDouble().Value;
                break;
              case "value":
                jsonReader.Read();
                obj = jsonReader.Value;
                break;
            }
            foreach (string key in InteractivityManager._giveInputKeyValues.Keys)
            {
              if (key == str)
                InteractivityManager._giveInputKeyValues[key] = obj;
            }
          }
        }
        else
          break;
      }
    }
    catch
    {
      this._LogError($"Error: Error reading input from control {empty1}.");
    }
    uint cost = 0;
    InteractiveControl interactiveControl = this.ControlFromControlID(empty1);
    if (interactiveControl is InteractiveButtonControl interactiveButtonControl)
      cost = interactiveButtonControl.Cost;
    InteractiveEventType type = this.InteractiveEventTypeFromID(empty1);
    if (type == InteractiveEventType.TextInput)
      empty2 = obj.ToString();
    return new InteractivityManager._InputEvent(empty1, interactiveControl._kind, eventName, type, isPressed, x, y, cost, string.Empty, empty2);
  }

  public void HandleParticipantJoin(JsonReader jsonReader)
  {
    int depth = jsonReader.Depth;
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null)
      {
        switch (jsonReader.Value.ToString())
        {
          case "participants":
            List<InteractiveParticipant> interactiveParticipantList = this.ReadParticipants(jsonReader);
            for (int index = 0; index < interactiveParticipantList.Count; ++index)
            {
              InteractiveParticipant participant = interactiveParticipantList[index];
              participant.State = InteractiveParticipantState.Joined;
              this._queuedEvents.Add((InteractiveEventArgs) new InteractiveParticipantStateChangedEventArgs(InteractiveEventType.ParticipantStateChanged, participant, participant.State));
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  public void HandleParticipantLeave(JsonReader jsonReader)
  {
    try
    {
      int depth = jsonReader.Depth;
      while (jsonReader.Read())
      {
        if (jsonReader.Value != null)
        {
          switch (jsonReader.Value.ToString())
          {
            case "participants":
              List<InteractiveParticipant> interactiveParticipantList = this.ReadParticipants(jsonReader);
              for (int index1 = 0; index1 < interactiveParticipantList.Count; ++index1)
              {
                for (int index2 = this._participants.Count - 1; index2 >= 0; --index2)
                {
                  if ((int) this._participants[index2].UserID == (int) interactiveParticipantList[index1].UserID)
                  {
                    InteractiveParticipant participant = this._participants[index2];
                    participant.State = InteractiveParticipantState.Left;
                    this._queuedEvents.Add((InteractiveEventArgs) new InteractiveParticipantStateChangedEventArgs(InteractiveEventType.ParticipantStateChanged, participant, participant.State));
                  }
                }
              }
              continue;
            default:
              continue;
          }
        }
      }
    }
    catch
    {
      this._LogError("Error: Error while processing participant leave message.");
    }
  }

  public void HandleParticipantUpdate(JsonReader jsonReader)
  {
    int depth = jsonReader.Depth;
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null)
      {
        switch (jsonReader.Value.ToString())
        {
          case "participants":
            this.ReadParticipants(jsonReader);
            continue;
          default:
            continue;
        }
      }
    }
  }

  public void HandleGiveInput(JsonReader jsonReader)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    InteractivityManager._InputEvent inputEvent = new InteractivityManager._InputEvent();
    while (jsonReader.Read())
    {
      if (jsonReader.Value != null)
      {
        switch (jsonReader.Value.ToString())
        {
          case "participantID":
            jsonReader.Read();
            empty1 = jsonReader.Value.ToString();
            continue;
          case "input":
            inputEvent = this.ReadInputObject(jsonReader);
            continue;
          case "transactionID":
            jsonReader.Read();
            empty2 = jsonReader.Value.ToString();
            continue;
          default:
            continue;
        }
      }
    }
    inputEvent.TransactionID = empty2;
    InternalTransactionIDState transactionIdState = new InternalTransactionIDState();
    if (InteractivityManager._transactionIDsState.ContainsKey(inputEvent.ControlID))
      transactionIdState = InteractivityManager._transactionIDsState[inputEvent.ControlID];
    transactionIdState.nextTransactionID = empty2;
    InteractivityManager._transactionIDsState[inputEvent.ControlID] = transactionIdState;
    InteractiveParticipant interactiveParticipant = this.ParticipantBySessionId(empty1);
    if (!InteractivityManager._participantsWhoTriggeredGiveInput.ContainsKey(inputEvent.ControlID))
      InteractivityManager._participantsWhoTriggeredGiveInput.Add(inputEvent.ControlID, new _InternalParticipantTrackingState(interactiveParticipant));
    interactiveParticipant.LastInputAt = DateTime.UtcNow;
    if (inputEvent.Type == InteractiveEventType.Button)
    {
      InteractiveButtonEventArgs e = new InteractiveButtonEventArgs(inputEvent.Type, inputEvent.ControlID, interactiveParticipant, inputEvent.IsPressed, inputEvent.Cost, inputEvent.TransactionID);
      this._queuedEvents.Add((InteractiveEventArgs) e);
      this.UpdateInternalButtonState(e);
    }
    else if (inputEvent.Type == InteractiveEventType.Joystick)
    {
      InteractiveJoystickEventArgs e = new InteractiveJoystickEventArgs(inputEvent.Type, inputEvent.ControlID, interactiveParticipant, (double) inputEvent.X, (double) inputEvent.Y);
      this._queuedEvents.Add((InteractiveEventArgs) e);
      this.UpdateInternalJoystickState(e);
    }
    else if (inputEvent.Type == InteractiveEventType.TextInput)
    {
      InteractiveTextEventArgs e = new InteractiveTextEventArgs(inputEvent.Type, inputEvent.ControlID, interactiveParticipant, inputEvent.TextValue, inputEvent.TransactionID);
      this._queuedEvents.Add((InteractiveEventArgs) e);
      this.UpdateInternalTextBoxState(e);
    }
    uint userId = interactiveParticipant.UserID;
    if (inputEvent.Kind == "screen")
    {
      if (inputEvent.Event == "move")
      {
        Vector2 position = new Vector2(inputEvent.X, inputEvent.Y);
        position.x *= (float) Screen.width;
        position.y *= (float) Screen.height;
        if (InteractivityManager._mousePositionsByParticipant.ContainsKey(userId))
          InteractivityManager._mousePositionsByParticipant[userId] = position;
        else
          InteractivityManager._mousePositionsByParticipant.Add(userId, position);
        this._queuedEvents.Add((InteractiveEventArgs) new InteractiveCoordinatesChangedEventArgs(inputEvent.ControlID, interactiveParticipant, (Vector3) position));
      }
      else if (inputEvent.Event == "mousedown" || inputEvent.Event == "mouseup")
      {
        Vector2 position = new Vector2(inputEvent.X, inputEvent.Y);
        position.x *= (float) Screen.width;
        position.y *= (float) Screen.height;
        InteractiveMouseButtonEventArgs e = new InteractiveMouseButtonEventArgs(inputEvent.ControlID, interactiveParticipant, inputEvent.IsPressed, (Vector3) position);
        this._queuedEvents.Add((InteractiveEventArgs) e);
        this.UpdateInternalMouseButtonState(e);
      }
    }
    string controlId = inputEvent.ControlID;
    string kind = inputEvent.Kind;
    Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
    if (InteractivityManager._giveInputControlData.TryGetValue(controlId, out dictionary1))
    {
      foreach (string key in dictionary1.Keys)
      {
        object obj = (object) null;
        if (InteractivityManager._giveInputKeyValues.TryGetValue(key, out obj))
          dictionary1[key] = obj;
      }
      InteractivityManager._giveInputControlData[controlId] = dictionary1;
    }
    else
      InteractivityManager._giveInputControlData[controlId] = new Dictionary<string, object>();
    Dictionary<uint, Dictionary<string, object>> dictionary2 = new Dictionary<uint, Dictionary<string, object>>();
    if (InteractivityManager._giveInputControlDataByParticipant.TryGetValue(kind, out dictionary2))
    {
      Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
      if (dictionary2.TryGetValue(userId, out dictionary3))
      {
        foreach (string key in dictionary3.Keys)
        {
          object obj = (object) null;
          if (InteractivityManager._giveInputKeyValues.TryGetValue(key, out obj))
            dictionary3[key] = obj;
        }
        dictionary2[userId] = dictionary3;
      }
      else
      {
        Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
      }
      InteractivityManager._giveInputControlDataByParticipant[kind] = dictionary2;
    }
    else
      InteractivityManager._giveInputControlDataByParticipant[kind] = new Dictionary<uint, Dictionary<string, object>>();
  }

  public InteractiveParticipant ParticipantBySessionId(string sessionID)
  {
    InteractiveParticipant interactiveParticipant = (InteractiveParticipant) null;
    foreach (InteractiveParticipant participant in (IEnumerable<InteractiveParticipant>) this.Participants)
    {
      if (participant._sessionID == sessionID)
      {
        interactiveParticipant = participant;
        break;
      }
    }
    return interactiveParticipant;
  }

  public InteractiveParticipant _ParticipantByUserId(uint userID)
  {
    InteractiveParticipant interactiveParticipant = (InteractiveParticipant) null;
    foreach (InteractiveParticipant participant in (IEnumerable<InteractiveParticipant>) this.Participants)
    {
      if ((int) participant.UserID == (int) userID)
      {
        interactiveParticipant = participant;
        break;
      }
    }
    return interactiveParticipant;
  }

  public bool _GetButtonDown(string controlID, uint userID)
  {
    bool buttonDown = false;
    Dictionary<string, _InternalButtonState> dictionary;
    if (InteractivityManager._buttonStatesByParticipant.TryGetValue(userID, out dictionary))
    {
      _InternalButtonState internalButtonState;
      if (dictionary.TryGetValue(controlID, out internalButtonState))
        buttonDown = internalButtonState.ButtonCountState.CountOfButtonDownEvents > 0U;
    }
    else
      buttonDown = false;
    return buttonDown;
  }

  public bool _GetButtonPressed(string controlID, uint userID)
  {
    bool buttonPressed = false;
    Dictionary<string, _InternalButtonState> dictionary;
    if (InteractivityManager._buttonStatesByParticipant.TryGetValue(userID, out dictionary))
    {
      _InternalButtonState internalButtonState;
      if (dictionary.TryGetValue(controlID, out internalButtonState))
        buttonPressed = internalButtonState.ButtonCountState.CountOfButtonPressEvents > 0U;
    }
    else
      buttonPressed = false;
    return buttonPressed;
  }

  public bool _GetButtonUp(string controlID, uint userID)
  {
    bool buttonUp = false;
    Dictionary<string, _InternalButtonState> dictionary;
    if (InteractivityManager._buttonStatesByParticipant.TryGetValue(userID, out dictionary))
    {
      _InternalButtonState internalButtonState;
      if (dictionary.TryGetValue(controlID, out internalButtonState))
        buttonUp = internalButtonState.ButtonCountState.CountOfButtonUpEvents > 0U;
    }
    else
      buttonUp = false;
    return buttonUp;
  }

  public uint _GetCountOfButtonDowns(string controlID, uint userID)
  {
    uint countOfButtonDowns = 0;
    Dictionary<string, _InternalButtonState> dictionary;
    _InternalButtonState internalButtonState;
    if (InteractivityManager._buttonStatesByParticipant.TryGetValue(userID, out dictionary) && dictionary.TryGetValue(controlID, out internalButtonState))
      countOfButtonDowns = internalButtonState.ButtonCountState.CountOfButtonDownEvents;
    return countOfButtonDowns;
  }

  public uint _GetCountOfButtonPresses(string controlID, uint userID)
  {
    uint countOfButtonPresses = 0;
    Dictionary<string, _InternalButtonState> dictionary;
    _InternalButtonState internalButtonState;
    if (InteractivityManager._buttonStatesByParticipant.TryGetValue(userID, out dictionary) && dictionary.TryGetValue(controlID, out internalButtonState))
      countOfButtonPresses = internalButtonState.ButtonCountState.CountOfButtonPressEvents;
    return countOfButtonPresses;
  }

  public uint _GetCountOfButtonUps(string controlID, uint userID)
  {
    uint countOfButtonUps = 0;
    Dictionary<string, _InternalButtonState> dictionary;
    _InternalButtonState internalButtonState;
    if (InteractivityManager._buttonStatesByParticipant.TryGetValue(userID, out dictionary) && dictionary.TryGetValue(controlID, out internalButtonState))
      countOfButtonUps = internalButtonState.ButtonCountState.CountOfButtonUpEvents;
    return countOfButtonUps;
  }

  public bool _TryGetButtonStateByParticipant(
    uint userID,
    string controlID,
    out _InternalButtonState buttonState)
  {
    buttonState = new _InternalButtonState();
    bool stateByParticipant = false;
    Dictionary<string, _InternalButtonState> dictionary;
    if (InteractivityManager._buttonStatesByParticipant.TryGetValue(userID, out dictionary) && dictionary.TryGetValue(controlID, out buttonState))
      stateByParticipant = true;
    return stateByParticipant;
  }

  public InteractiveJoystickControl _GetJoystick(string controlID, uint userID)
  {
    InteractiveJoystickControl joystick1 = new InteractiveJoystickControl(controlID, InteractiveEventType.Joystick, true, string.Empty, string.Empty, string.Empty, new Dictionary<string, object>());
    foreach (InteractiveJoystickControl joystick2 in (IEnumerable<InteractiveJoystickControl>) this.Joysticks)
    {
      if (joystick2.ControlID == controlID)
        joystick1 = joystick2;
    }
    joystick1._userID = userID;
    return joystick1;
  }

  public double _GetJoystickX(string controlID, uint userID)
  {
    double joystickX = 0.0;
    _InternalJoystickState joystickState;
    if (this.TryGetJoystickStateByParticipant(userID, controlID, out joystickState))
      joystickX = joystickState.X;
    return joystickX;
  }

  public double _GetJoystickY(string controlID, uint userID)
  {
    double joystickY = 0.0;
    _InternalJoystickState joystickState;
    if (this.TryGetJoystickStateByParticipant(userID, controlID, out joystickState))
      joystickY = joystickState.Y;
    return joystickY;
  }

  public bool TryGetJoystickStateByParticipant(
    uint userID,
    string controlID,
    out _InternalJoystickState joystickState)
  {
    joystickState = new _InternalJoystickState();
    bool stateByParticipant = false;
    Dictionary<string, _InternalJoystickState> dictionary;
    if (InteractivityManager._joystickStatesByParticipant.TryGetValue(userID, out dictionary) && dictionary.TryGetValue(controlID, out joystickState))
      stateByParticipant = true;
    return stateByParticipant;
  }

  public _InternalMouseButtonState TryGetMouseButtonState(uint userID)
  {
    _InternalMouseButtonState mouseButtonState = new _InternalMouseButtonState();
    InteractivityManager._mouseButtonStateByParticipant.TryGetValue(userID, out mouseButtonState);
    return mouseButtonState;
  }

  public string GetText(string controlID, uint userID)
  {
    string empty = string.Empty;
    Dictionary<string, string> dictionary;
    if (InteractivityManager._textboxValuesByParticipant.TryGetValue(userID, out dictionary))
      dictionary.TryGetValue(controlID, out empty);
    return empty;
  }

  public InteractiveControl _GetControl(string controlID)
  {
    InteractiveControl control1 = new InteractiveControl(controlID, "", InteractiveEventType.Unknown, true, "", "", "", new Dictionary<string, object>());
    foreach (InteractiveControl control2 in (IEnumerable<InteractiveControl>) this._Controls)
    {
      if (control2.ControlID == controlID)
      {
        control1 = control2;
        break;
      }
    }
    return control1;
  }

  public InteractiveButtonControl GetButton(string controlID)
  {
    InteractiveButtonControl button1 = new InteractiveButtonControl(controlID, InteractiveEventType.Button, false, string.Empty, 0U, string.Empty, string.Empty, new Dictionary<string, object>());
    foreach (InteractiveButtonControl button2 in (IEnumerable<InteractiveButtonControl>) this.Buttons)
    {
      if (button2.ControlID == controlID)
      {
        button1 = button2;
        break;
      }
    }
    return button1;
  }

  public InteractiveJoystickControl GetJoystick(string controlID)
  {
    InteractiveJoystickControl joystick1 = new InteractiveJoystickControl(controlID, InteractiveEventType.Joystick, true, "", "", "", new Dictionary<string, object>());
    foreach (InteractiveJoystickControl joystick2 in (IEnumerable<InteractiveJoystickControl>) this.Joysticks)
    {
      if (joystick2.ControlID == controlID)
      {
        joystick1 = joystick2;
        break;
      }
    }
    return joystick1;
  }

  public string GetCurrentScene() => this.GroupFromID("default").SceneID;

  public void SetCurrentScene(string sceneID) => this.GroupFromID("default")?.SetScene(sceneID);

  public IList<InteractiveTextResult> _GetText(string controlID)
  {
    List<InteractiveTextResult> text = new List<InteractiveTextResult>();
    InteractivityManager singletonInstance = InteractivityManager.SingletonInstance;
    Dictionary<uint, Dictionary<string, string>> valuesByParticipant = InteractivityManager._textboxValuesByParticipant;
    foreach (uint key1 in valuesByParticipant.Keys)
    {
      Dictionary<string, string> dictionary = valuesByParticipant[key1];
      string empty = string.Empty;
      string key2 = controlID;
      ref string local = ref empty;
      dictionary.TryGetValue(key2, out local);
      text.Add(new InteractiveTextResult()
      {
        Participant = singletonInstance._ParticipantByUserId(key1),
        Text = empty
      });
    }
    return (IList<InteractiveTextResult>) text;
  }

  public void _SetCurrentSceneInternal(InteractiveGroup group, string sceneID)
  {
    this._SendSetUpdateGroupsMessage(group.GroupID, sceneID, group._etag);
  }

  public InteractiveGroup GroupFromID(string groupID)
  {
    InteractiveGroup interactiveGroup = new InteractiveGroup("", groupID, "default");
    foreach (InteractiveGroup group in (IEnumerable<InteractiveGroup>) this.Groups)
    {
      if (group.GroupID == groupID)
      {
        interactiveGroup = group;
        break;
      }
    }
    return interactiveGroup;
  }

  public InteractiveScene SceneFromID(string sceneID)
  {
    InteractiveScene interactiveScene = new InteractiveScene(sceneID);
    foreach (InteractiveScene scene in (IEnumerable<InteractiveScene>) this.Scenes)
    {
      if (scene.SceneID == sceneID)
      {
        interactiveScene = scene;
        break;
      }
    }
    return interactiveScene;
  }

  public InteractiveEventType InteractiveEventTypeFromID(string controlID)
  {
    InteractiveEventType interactiveEventType = InteractiveEventType.Unknown;
    foreach (InteractiveControl control in this._controls)
    {
      if (controlID == control.ControlID)
      {
        interactiveEventType = control._type;
        break;
      }
    }
    return interactiveEventType;
  }

  public void SendReady(bool isReady)
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("ready");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName(nameof (isReady));
      jsonWriter.WriteValue(isReady);
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "ready");
  }

  public void _SendCaptureTransactionMessage(string transactionID)
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("capture");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName(nameof (transactionID));
      jsonWriter.WriteValue(transactionID);
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "capture");
  }

  public void _SendCreateGroupsMessage(string groupID, string sceneID)
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("createGroups");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("groups");
      jsonWriter.WriteStartArray();
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName(nameof (groupID));
      jsonWriter.WriteValue(groupID);
      jsonWriter.WritePropertyName(nameof (sceneID));
      jsonWriter.WriteValue(sceneID);
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEndArray();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "setCurrentScene");
  }

  public void _SendSetUpdateGroupsMessage(string groupID, string sceneID, string groupEtag)
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("updateGroups");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("groups");
      jsonWriter.WriteStartArray();
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName(nameof (groupID));
      jsonWriter.WriteValue(groupID);
      jsonWriter.WritePropertyName(nameof (sceneID));
      jsonWriter.WriteValue(sceneID);
      jsonWriter.WritePropertyName("etag");
      jsonWriter.WriteValue(groupEtag);
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEndArray();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "setCurrentScene");
  }

  public void _SendSetUpdateScenesMessage(InteractiveScene scene)
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("updateScenes");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("scenes");
      jsonWriter.WriteStartArray();
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("sceneID");
      jsonWriter.WriteValue(scene.SceneID);
      jsonWriter.WritePropertyName("etag");
      jsonWriter.WriteValue(scene._etag);
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEndArray();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "setCurrentScene");
  }

  public void _SendUpdateParticipantsMessage(InteractiveParticipant participant)
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("updateParticipants");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("participants");
      jsonWriter.WriteStartArray();
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("sessionID");
      jsonWriter.WriteValue(participant._sessionID);
      jsonWriter.WritePropertyName("etag");
      jsonWriter.WriteValue(participant._etag);
      jsonWriter.WritePropertyName("groupID");
      jsonWriter.WriteValue(participant._groupID);
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEndArray();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "updateParticipants");
  }

  public void SendSetCompressionMessage()
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("setCompression");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("scheme");
      jsonWriter.WriteStartArray();
      jsonWriter.WriteValue("gzip");
      jsonWriter.WriteEndArray();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "setCompression");
  }

  public void _SendSetJoystickSetCoordinates(string controlID, double x, double y)
  {
    InteractiveControl interactiveControl = this.ControlFromControlID(controlID);
    if (interactiveControl == null)
      return;
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("updateControls");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("sceneID");
      jsonWriter.WriteValue(interactiveControl._sceneID);
      jsonWriter.WritePropertyName("controls");
      jsonWriter.WriteStartArray();
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName(nameof (controlID));
      jsonWriter.WriteValue(controlID);
      jsonWriter.WritePropertyName("etag");
      jsonWriter.WriteValue(interactiveControl._eTag);
      jsonWriter.WritePropertyName(nameof (x));
      jsonWriter.WriteValue(x);
      jsonWriter.WritePropertyName(nameof (y));
      jsonWriter.WriteValue(y);
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEndArray();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "setJoystickCoordinates");
  }

  public void _SendSetButtonControlProperties(
    string controlID,
    string propertyName,
    bool disabled,
    float progress,
    string text,
    uint cost)
  {
    InteractiveControl interactiveControl = this.ControlFromControlID(controlID);
    if (interactiveControl == null)
      return;
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue("method");
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName("method");
      jsonWriter.WriteValue("updateControls");
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("sceneID");
      jsonWriter.WriteValue(interactiveControl._sceneID);
      jsonWriter.WritePropertyName("controls");
      jsonWriter.WriteStartArray();
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName(nameof (controlID));
      jsonWriter.WriteValue(controlID);
      jsonWriter.WritePropertyName("etag");
      jsonWriter.WriteValue(interactiveControl._eTag);
      if (propertyName == nameof (disabled))
      {
        jsonWriter.WritePropertyName(nameof (disabled));
        jsonWriter.WriteValue(disabled);
      }
      if (propertyName == nameof (progress))
      {
        jsonWriter.WritePropertyName(nameof (progress));
        jsonWriter.WriteValue(progress);
      }
      if (propertyName == nameof (text))
      {
        jsonWriter.WritePropertyName(nameof (text));
        jsonWriter.WriteValue(text);
      }
      if (propertyName == nameof (cost))
      {
        jsonWriter.WritePropertyName(nameof (cost));
        jsonWriter.WriteValue(cost);
      }
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEndArray();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      this.SendJsonString(stringWriter.ToString());
    }
    this.StoreIfExpectingReply(messageID, "setButtonControlProperties");
  }

  public void SendGetAllGroupsMessage() => this.SendCallMethodMessage("getGroups");

  public void SendGetAllScenesMessage() => this.SendCallMethodMessage("getScenes");

  public void SendGetAllParticipants() => this.SendCallMethodMessage("getAllParticipants");

  public void SendCallMethodMessage(string method)
  {
    uint messageID = this._currentmessageID++;
    StringWriter stringWriter = new StringWriter(new StringBuilder());
    using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
    {
      jsonWriter.WriteStartObject();
      jsonWriter.WritePropertyName("type");
      jsonWriter.WriteValue(nameof (method));
      jsonWriter.WritePropertyName("id");
      jsonWriter.WriteValue(messageID);
      jsonWriter.WritePropertyName(nameof (method));
      jsonWriter.WriteValue(method);
      jsonWriter.WritePropertyName("params");
      jsonWriter.WriteStartObject();
      jsonWriter.WriteEndObject();
      jsonWriter.WriteEnd();
      try
      {
        this.SendJsonString(stringWriter.ToString());
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        this._LogError("Error: Unable to send message: " + method);
      }
    }
    this.StoreIfExpectingReply(messageID, method);
  }

  public void SendJsonString(string jsonString)
  {
    if (this._websocket == null)
      return;
    this._websocket.Send(jsonString);
    this._Log(jsonString);
  }

  public void StoreIfExpectingReply(uint messageID, string messageType)
  {
    if (!(messageType != "getAllParticipants") && !(messageType != "getGroups") && !(messageType != "getScenes") && !(messageType != "setCurrentScene"))
      return;
    this._outstandingMessages.Add(messageID, messageType);
  }

  public void InitializeInternal()
  {
    this.UpdateInteractivityState(InteractivityState.NotInitialized);
    this._buttons = new List<InteractiveButtonControl>();
    this._controls = new List<InteractiveControl>();
    this._groups = new List<InteractiveGroup>();
    this._joysticks = new List<InteractiveJoystickControl>();
    this._participants = new List<InteractiveParticipant>();
    this._scenes = new List<InteractiveScene>();
    this._websocketHosts = new List<string>();
    InteractivityManager._buttonStates = new Dictionary<string, _InternalButtonCountState>();
    InteractivityManager._buttonStatesByParticipant = new Dictionary<uint, Dictionary<string, _InternalButtonState>>();
    if (!Application.isEditor)
      this.LoggingLevel = LoggingLevel.None;
    InteractivityManager._joystickStates = new Dictionary<string, _InternalJoystickState>();
    InteractivityManager._joystickStatesByParticipant = new Dictionary<uint, Dictionary<string, _InternalJoystickState>>();
    InteractivityManager._mouseButtonStateByParticipant = new Dictionary<uint, _InternalMouseButtonState>();
    InteractivityManager._mousePositionsByParticipant = new Dictionary<uint, Vector2>();
    InteractivityManager._participantsWhoTriggeredGiveInput = new Dictionary<string, _InternalParticipantTrackingState>();
    InteractivityManager._queuedControlPropertyUpdates = new Dictionary<string, Dictionary<string, _InternalControlPropertyUpdateData>>();
    InteractivityManager._transactionIDsState = new Dictionary<string, InternalTransactionIDState>();
    InteractivityManager._giveInputControlDataByParticipant = new Dictionary<string, Dictionary<uint, Dictionary<string, object>>>();
    InteractivityManager._giveInputControlData = new Dictionary<string, Dictionary<string, object>>();
    InteractivityManager._giveInputKeyValues = new Dictionary<string, object>();
    InteractivityManager._textboxValuesByParticipant = new Dictionary<uint, Dictionary<string, string>>();
    this._streamingAssetsPath = Application.streamingAssetsPath;
    this.CreateStorageDirectoryIfNotExists();
    this.mixerInteractiveHelper = MixerInteractiveHelper._SingletonInstance;
  }

  public void OnInternalRefreshShortCodeTimerCallback(
    object sender,
    MixerInteractiveHelper.InternalTimerCallbackEventArgs e)
  {
    this.mixerInteractiveHelper.OnInternalRefreshShortCodeTimerCallback -= new MixerInteractiveHelper.OnInternalRefreshShortCodeCallbackEventHandler(this.OnInternalRefreshShortCodeTimerCallback);
    this.RefreshShortCode();
  }

  public void OnInternalReconnectTimerCallback(
    object sender,
    MixerInteractiveHelper.InternalTimerCallbackEventArgs e)
  {
    this.mixerInteractiveHelper.OnInternalReconnectTimerCallback -= new MixerInteractiveHelper.OnInternalReconnectCallbackEventHandler(this.OnInternalReconnectTimerCallback);
    this.VerifyAuthToken();
  }

  public void _LogError(string message) => this._LogError(message, this.ERROR_FAIL);

  public void _LogError(string message, int code)
  {
    this._queuedEvents.Add(new InteractiveEventArgs(InteractiveEventType.Error, code, message));
    this._Log(message, LoggingLevel.Minimal);
  }

  public void _Log(string message, LoggingLevel level = LoggingLevel.Verbose)
  {
    if (this.LoggingLevel == LoggingLevel.None || this.LoggingLevel == LoggingLevel.Minimal && level == LoggingLevel.Verbose)
      return;
    Debug.Log((object) message);
  }

  public void ClearPreviousControlState()
  {
    if (this.InteractivityState != InteractivityState.InteractivityEnabled)
      return;
    foreach (string key in new List<string>((IEnumerable<string>) InteractivityManager._buttonStates.Keys))
    {
      _InternalButtonCountState buttonState = InteractivityManager._buttonStates[key];
      InteractivityManager._buttonStates[key] = new _InternalButtonCountState()
      {
        PreviousCountOfButtonDownEvents = buttonState.CountOfButtonDownEvents,
        CountOfButtonDownEvents = buttonState.NextCountOfButtonDownEvents,
        NextCountOfButtonDownEvents = 0U,
        PreviousCountOfButtonPressEvents = buttonState.CountOfButtonPressEvents,
        CountOfButtonPressEvents = buttonState.NextCountOfButtonPressEvents,
        NextCountOfButtonPressEvents = 0U,
        PreviousCountOfButtonUpEvents = buttonState.CountOfButtonUpEvents,
        CountOfButtonUpEvents = buttonState.NextCountOfButtonUpEvents,
        NextCountOfButtonUpEvents = 0U,
        PreviousTransactionID = buttonState.TransactionID,
        TransactionID = buttonState.NextTransactionID,
        NextTransactionID = string.Empty
      };
    }
    foreach (uint key1 in new List<uint>((IEnumerable<uint>) InteractivityManager._buttonStatesByParticipant.Keys))
    {
      foreach (string key2 in new List<string>((IEnumerable<string>) InteractivityManager._buttonStatesByParticipant[key1].Keys))
      {
        _InternalButtonState internalButtonState = InteractivityManager._buttonStatesByParticipant[key1][key2];
        InteractivityManager._buttonStatesByParticipant[key1][key2] = new _InternalButtonState()
        {
          ButtonCountState = new _InternalButtonCountState()
          {
            PreviousCountOfButtonDownEvents = internalButtonState.ButtonCountState.CountOfButtonDownEvents,
            CountOfButtonDownEvents = internalButtonState.ButtonCountState.NextCountOfButtonDownEvents,
            NextCountOfButtonDownEvents = 0U,
            PreviousCountOfButtonPressEvents = internalButtonState.ButtonCountState.CountOfButtonPressEvents,
            CountOfButtonPressEvents = internalButtonState.ButtonCountState.NextCountOfButtonPressEvents,
            NextCountOfButtonPressEvents = 0U,
            PreviousCountOfButtonUpEvents = internalButtonState.ButtonCountState.CountOfButtonUpEvents,
            CountOfButtonUpEvents = internalButtonState.ButtonCountState.NextCountOfButtonUpEvents,
            NextCountOfButtonUpEvents = 0U
          }
        };
      }
    }
    foreach (uint key in new List<uint>((IEnumerable<uint>) InteractivityManager._mouseButtonStateByParticipant.Keys))
    {
      _InternalMouseButtonState mouseButtonState1 = InteractivityManager._mouseButtonStateByParticipant[key];
      _InternalMouseButtonState mouseButtonState2 = new _InternalMouseButtonState();
      if (mouseButtonState1.NextIsDown)
      {
        mouseButtonState2.IsDown = true;
        mouseButtonState2.IsPressed = true;
        mouseButtonState2.IsUp = false;
        mouseButtonState2.NextIsDown = false;
        mouseButtonState2.NextIsPressed = true;
        mouseButtonState2.NextIsUp = false;
      }
      else if (mouseButtonState1.NextIsUp)
      {
        mouseButtonState2.IsDown = false;
        mouseButtonState2.IsPressed = false;
        mouseButtonState2.IsUp = true;
        mouseButtonState2.NextIsDown = false;
        mouseButtonState2.NextIsPressed = false;
        mouseButtonState2.NextIsUp = false;
      }
      else if (mouseButtonState1.NextIsPressed)
      {
        mouseButtonState2.IsDown = false;
        mouseButtonState2.IsPressed = true;
        mouseButtonState2.IsUp = false;
        mouseButtonState2.NextIsDown = false;
        mouseButtonState2.NextIsPressed = true;
        mouseButtonState2.NextIsUp = false;
      }
      else
      {
        mouseButtonState2.IsDown = false;
        mouseButtonState2.IsPressed = false;
        mouseButtonState2.IsUp = false;
        mouseButtonState2.NextIsDown = false;
        mouseButtonState2.NextIsPressed = false;
        mouseButtonState2.NextIsUp = false;
      }
      InteractivityManager._mouseButtonStateByParticipant[key] = mouseButtonState2;
    }
    foreach (string key in new List<string>((IEnumerable<string>) InteractivityManager._participantsWhoTriggeredGiveInput.Keys))
    {
      _InternalParticipantTrackingState participantTrackingState = InteractivityManager._participantsWhoTriggeredGiveInput[key];
      InteractivityManager._participantsWhoTriggeredGiveInput[key] = new _InternalParticipantTrackingState()
      {
        previousParticpant = participantTrackingState.particpant,
        particpant = participantTrackingState.nextParticpant,
        nextParticpant = (InteractiveParticipant) null
      };
    }
    foreach (string key in new List<string>((IEnumerable<string>) InteractivityManager._transactionIDsState.Keys))
    {
      InternalTransactionIDState transactionIdState = InteractivityManager._transactionIDsState[key];
      InteractivityManager._transactionIDsState[key] = new InternalTransactionIDState()
      {
        previousTransactionID = transactionIdState.transactionID,
        transactionID = transactionIdState.nextTransactionID,
        nextTransactionID = string.Empty
      };
    }
  }

  public void UpdateInternalButtonState(InteractiveButtonEventArgs e)
  {
    uint userId = e.Participant.UserID;
    string controlId = e.ControlID;
    Dictionary<string, _InternalButtonState> dictionary;
    if (!InteractivityManager._buttonStatesByParticipant.TryGetValue(userId, out dictionary))
    {
      InteractivityManager._buttonStatesByParticipant.Add(userId, new Dictionary<string, _InternalButtonState>()
      {
        {
          controlId,
          new _InternalButtonState()
          {
            IsDown = e.IsPressed,
            IsPressed = e.IsPressed,
            IsUp = !e.IsPressed
          }
        }
      });
    }
    else
    {
      _InternalButtonState internalButtonState;
      if (!dictionary.TryGetValue(controlId, out internalButtonState))
      {
        internalButtonState = new _InternalButtonState();
        dictionary.Add(controlId, new _InternalButtonState()
        {
          IsDown = e.IsPressed,
          IsPressed = e.IsPressed,
          IsUp = !e.IsPressed
        });
      }
    }
    bool flag = InteractivityManager._buttonStatesByParticipant[userId][controlId].ButtonCountState.NextCountOfButtonPressEvents > 0U;
    int num = e.IsPressed ? 1 : 0;
    _InternalButtonState internalButtonState1 = InteractivityManager._buttonStatesByParticipant[userId][controlId];
    if (num != 0)
    {
      if (!flag)
      {
        internalButtonState1.IsDown = true;
        internalButtonState1.IsPressed = true;
        internalButtonState1.IsUp = false;
      }
      else
      {
        internalButtonState1.IsDown = false;
        internalButtonState1.IsPressed = true;
        internalButtonState1.IsUp = false;
      }
    }
    else
    {
      internalButtonState1.IsDown = false;
      internalButtonState1.IsPressed = false;
      internalButtonState1.IsUp = true;
    }
    _InternalButtonCountState buttonCountState = internalButtonState1.ButtonCountState;
    if (internalButtonState1.IsDown)
      ++buttonCountState.NextCountOfButtonDownEvents;
    if (internalButtonState1.IsPressed)
      ++buttonCountState.NextCountOfButtonPressEvents;
    if (internalButtonState1.IsUp)
      ++buttonCountState.NextCountOfButtonUpEvents;
    if (!string.IsNullOrEmpty(e.TransactionID))
      buttonCountState.NextTransactionID = e.TransactionID;
    internalButtonState1.ButtonCountState = buttonCountState;
    InteractivityManager._buttonStatesByParticipant[userId][controlId] = internalButtonState1;
    if (InteractivityManager._buttonStates.TryGetValue(controlId, out _InternalButtonCountState _))
      InteractivityManager._buttonStates[controlId] = internalButtonState1.ButtonCountState;
    else
      InteractivityManager._buttonStates.Add(controlId, internalButtonState1.ButtonCountState);
  }

  public void UpdateInternalJoystickState(InteractiveJoystickEventArgs e)
  {
    uint userId = e.Participant.UserID;
    string controlId = e.ControlID;
    Dictionary<string, _InternalJoystickState> dictionary;
    _InternalJoystickState internalJoystickState1;
    if (!InteractivityManager._joystickStatesByParticipant.TryGetValue(userId, out dictionary))
    {
      dictionary = new Dictionary<string, _InternalJoystickState>();
      internalJoystickState1 = new _InternalJoystickState();
      internalJoystickState1.X = e.X;
      internalJoystickState1.Y = e.Y;
      internalJoystickState1.countOfUniqueJoystickInputs = 1;
      InteractivityManager._joystickStatesByParticipant.Add(userId, dictionary);
    }
    else
    {
      internalJoystickState1 = new _InternalJoystickState();
      if (!dictionary.TryGetValue(controlId, out internalJoystickState1))
      {
        internalJoystickState1.X = e.X;
        internalJoystickState1.Y = e.Y;
        internalJoystickState1.countOfUniqueJoystickInputs = 1;
        dictionary.Add(controlId, internalJoystickState1);
      }
      int uniqueJoystickInputs = internalJoystickState1.countOfUniqueJoystickInputs;
      internalJoystickState1.X = internalJoystickState1.X * (double) (uniqueJoystickInputs - 1) / (double) uniqueJoystickInputs + e.X * (double) (1 / uniqueJoystickInputs);
      internalJoystickState1.Y = internalJoystickState1.Y * (double) (uniqueJoystickInputs - 1) / (double) uniqueJoystickInputs + e.Y * (double) (1 / uniqueJoystickInputs);
    }
    InteractivityManager._joystickStatesByParticipant[e.Participant.UserID][e.ControlID] = internalJoystickState1;
    _InternalJoystickState internalJoystickState2;
    if (!dictionary.TryGetValue(controlId, out internalJoystickState2))
    {
      internalJoystickState2.X = e.X;
      internalJoystickState2.Y = e.Y;
      internalJoystickState2.countOfUniqueJoystickInputs = 1;
      dictionary.Add(controlId, internalJoystickState2);
    }
    ++internalJoystickState2.countOfUniqueJoystickInputs;
    int uniqueJoystickInputs1 = internalJoystickState2.countOfUniqueJoystickInputs;
    internalJoystickState2.X = internalJoystickState2.X * (double) (uniqueJoystickInputs1 - 1) / (double) uniqueJoystickInputs1 + e.X * (double) (1 / uniqueJoystickInputs1);
    internalJoystickState2.Y = internalJoystickState2.Y * (double) (uniqueJoystickInputs1 - 1) / (double) uniqueJoystickInputs1 + e.Y * (double) (1 / uniqueJoystickInputs1);
    InteractivityManager._joystickStates[e.ControlID] = internalJoystickState2;
  }

  public void UpdateInternalTextBoxState(InteractiveTextEventArgs e)
  {
    uint userId = e.Participant.UserID;
    string controlId = e.ControlID;
    string text = e.Text;
    string empty = string.Empty;
    Dictionary<string, string> dictionary;
    if (!InteractivityManager._textboxValuesByParticipant.TryGetValue(userId, out dictionary))
      InteractivityManager._textboxValuesByParticipant.Add(userId, new Dictionary<string, string>()
      {
        {
          controlId,
          text
        }
      });
    else if (!dictionary.TryGetValue(controlId, out empty))
      dictionary.Add(controlId, text);
    InteractivityManager._textboxValuesByParticipant[e.Participant.UserID][e.ControlID] = text;
  }

  public void UpdateInternalMouseButtonState(InteractiveMouseButtonEventArgs e)
  {
    uint userId = e.Participant.UserID;
    bool isPressed = e.IsPressed;
    if (!InteractivityManager._mouseButtonStateByParticipant.TryGetValue(userId, out _InternalMouseButtonState _))
      InteractivityManager._mouseButtonStateByParticipant.Add(userId, new _InternalMouseButtonState()
      {
        IsDown = false,
        IsPressed = false,
        IsUp = false,
        NextIsDown = e.IsPressed,
        NextIsPressed = e.IsPressed,
        NextIsUp = !e.IsPressed
      });
    _InternalMouseButtonState mouseButtonState = InteractivityManager._mouseButtonStateByParticipant[userId] with
    {
      NextIsDown = isPressed,
      NextIsPressed = isPressed,
      NextIsUp = !isPressed
    };
    InteractivityManager._mouseButtonStateByParticipant[userId] = mouseButtonState;
  }

  public void _QueuePropertyUpdate(string sceneID, string controlID, string name, bool value)
  {
    _KnownControlPropertyPrimitiveTypes type = _KnownControlPropertyPrimitiveTypes.Boolean;
    this._QueuePropertyUpdateImpl(sceneID, controlID, name, type, (object) value);
  }

  public void _QueuePropertyUpdate(string sceneID, string controlID, string name, double value)
  {
    _KnownControlPropertyPrimitiveTypes type = _KnownControlPropertyPrimitiveTypes.Number;
    this._QueuePropertyUpdateImpl(sceneID, controlID, name, type, (object) value);
  }

  public void _QueuePropertyUpdate(string sceneID, string controlID, string name, string value)
  {
    _KnownControlPropertyPrimitiveTypes type = _KnownControlPropertyPrimitiveTypes.String;
    this._QueuePropertyUpdateImpl(sceneID, controlID, name, type, (object) value);
  }

  public void _QueuePropertyUpdate(string sceneID, string controlID, string name, object value)
  {
    _KnownControlPropertyPrimitiveTypes type = _KnownControlPropertyPrimitiveTypes.Unknown;
    this._QueuePropertyUpdateImpl(sceneID, controlID, name, type, value);
  }

  public void _QueuePropertyUpdateImpl(
    string sceneID,
    string controlID,
    string name,
    _KnownControlPropertyPrimitiveTypes type,
    object value)
  {
    if (!InteractivityManager._queuedControlPropertyUpdates.ContainsKey(sceneID))
    {
      _InternalControlPropertyUpdateData propertyUpdateData = new _InternalControlPropertyUpdateData(name, type, value);
      InteractivityManager._queuedControlPropertyUpdates.Add(sceneID, new Dictionary<string, _InternalControlPropertyUpdateData>()
      {
        {
          controlID,
          propertyUpdateData
        }
      });
    }
    else
    {
      Dictionary<string, _InternalControlPropertyUpdateData> controlPropertyUpdate = InteractivityManager._queuedControlPropertyUpdates[sceneID];
      if (!controlPropertyUpdate.ContainsKey(controlID))
      {
        _InternalControlPropertyUpdateData propertyUpdateData = new _InternalControlPropertyUpdateData(name, type, value);
        InteractivityManager._queuedControlPropertyUpdates[sceneID].Add(controlID, propertyUpdateData);
      }
      else
      {
        _InternalControlPropertyUpdateData propertyUpdateData = controlPropertyUpdate[controlID];
        _InternalControlPropertyMetaData propertyMetaData = new _InternalControlPropertyMetaData();
        propertyMetaData.type = type;
        switch (type)
        {
          case _KnownControlPropertyPrimitiveTypes.Boolean:
            propertyMetaData.boolValue = (bool) value;
            break;
          case _KnownControlPropertyPrimitiveTypes.Number:
            propertyMetaData.numberValue = (double) value;
            break;
          default:
            propertyMetaData.stringValue = value.ToString();
            break;
        }
        if (!propertyUpdateData.properties.ContainsKey(name))
          InteractivityManager._queuedControlPropertyUpdates[sceneID][controlID].properties.Add(name, propertyMetaData);
        else
          InteractivityManager._queuedControlPropertyUpdates[sceneID][controlID].properties[name] = propertyMetaData;
      }
    }
  }

  public void _RegisterControlForValueUpdates(string controlTypeName, List<string> valuesToTrack)
  {
    if (!InteractivityManager._giveInputControlData.ContainsKey(controlTypeName))
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      InteractivityManager._giveInputControlData[controlTypeName] = dictionary;
    }
    foreach (string key in valuesToTrack)
    {
      if (!InteractivityManager._giveInputKeyValues.ContainsKey(key))
        InteractivityManager._giveInputKeyValues.Add(key, (object) null);
    }
  }

  public string _InteractiveControlPropertyToString(InteractiveControlProperty property)
  {
    string str = string.Empty;
    switch (property)
    {
      case InteractiveControlProperty.Text:
        str = "text";
        break;
      case InteractiveControlProperty.BackgroundColor:
        str = "backgroundColor";
        break;
      case InteractiveControlProperty.BackgroundImage:
        str = "backgroundImage";
        break;
      case InteractiveControlProperty.TextColor:
        str = "textColor";
        break;
      case InteractiveControlProperty.TextSize:
        str = "textSize";
        break;
      case InteractiveControlProperty.BorderColor:
        str = "borderColor";
        break;
      case InteractiveControlProperty.FocusColor:
        str = "focusColor";
        break;
      case InteractiveControlProperty.AccentColor:
        str = "accentColor";
        break;
    }
    return str;
  }

  public delegate void OnErrorEventHandler(object sender, InteractiveEventArgs e);

  public delegate void OnInteractivityStateChangedHandler(
    object sender,
    InteractivityStateChangedEventArgs e);

  public delegate void OnParticipantStateChangedHandler(
    object sender,
    InteractiveParticipantStateChangedEventArgs e);

  public delegate void OnInteractiveButtonEventHandler(object sender, InteractiveButtonEventArgs e);

  public delegate void OnInteractiveJoystickControlEventHandler(
    object sender,
    InteractiveJoystickEventArgs e);

  public delegate void OnInteractiveMouseButtonEventHandler(
    object sender,
    InteractiveMouseButtonEventArgs e);

  public delegate void OnInteractiveCoordinatesChangedHandler(
    object sender,
    InteractiveCoordinatesChangedEventArgs e);

  public delegate void OnInteractiveTextControlEventHandler(
    object sender,
    InteractiveTextEventArgs e);

  public delegate void OnInteractiveMessageEventHandler(
    object sender,
    InteractiveMessageEventArgs e);

  public delegate void OnInteractiveDoWorkEventHandler(object sender, InteractiveEventArgs e);

  public struct _InputEvent(
    string controlID,
    string kind,
    string eventName,
    InteractiveEventType type,
    bool isPressed,
    float x,
    float y,
    uint cost,
    string transactionID,
    string textValue)
  {
    public string ControlID = controlID;
    public string Kind = kind;
    public string Event = eventName;
    public InteractiveEventType Type = type;
    public uint Cost = cost;
    public bool IsPressed = isPressed;
    public string TransactionID = transactionID;
    public float X = x;
    public float Y = y;
    public string TextValue = textValue;
  }
}
