// Decompiled with JetBrains decompiler
// Type: MixerInteractive
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Microsoft;
using Microsoft.Mixer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class MixerInteractive : MonoBehaviour
{
  public bool runInBackground = true;
  public string defaultSceneID;
  public List<string> groupIDs;
  public List<string> sceneIDs;
  public static InteractivityManager interactivityManager;
  public static List<InteractiveEventArgs> queuedEvents;
  public static bool previousRunInBackgroundValue;
  public static MixerInteractiveDialog mixerDialog;
  public static bool pendingGoInteractive;
  public static string outstandingSetDefaultSceneRequest;
  public static List<string> outstandingCreateGroupsRequests;
  public static bool outstandingRequestsCompleted;
  public static float lastCheckForOutstandingRequestsTime;
  public static bool processedSerializedProperties;
  public static bool hasFiredGoInteractiveEvent;
  public static bool shouldCheckForOutstandingRequests;
  public GameObject addNewRpcMethodSource;
  public List<string> rpcOwningMonoBehaviorNames;
  public List<string> rpcMethodNames;
  public static List<string> outboundMessages;
  public static Websocket _websocket;
  public static BackgroundWorker backgroundWorker;
  public const string DEFAULT_GROUP_ID = "default";
  public const float CHECK_FOR_OUTSTANDING_REQUESTS_INTERVAL = 1f;
  public const float _DEFAULT_MIXER_SYNCVAR_UPDATE_INTERVAL = 1f;
  [CompilerGenerated]
  public static bool \u003CManuallyHandleSparkTransactions\u003Ek__BackingField;

  public static event MixerInteractive.OnErrorEventHandler OnError;

  public static event MixerInteractive.OnGoInteractiveHandler OnGoInteractive;

  public static event MixerInteractive.OnInteractivityStateChangedHandler OnInteractivityStateChanged;

  public static event MixerInteractive.OnParticipantStateChangedHandler OnParticipantStateChanged;

  public static event MixerInteractive.OnInteractiveButtonEventHandler OnInteractiveButtonEvent;

  public static event MixerInteractive.OnInteractiveJoystickControlEventHandler OnInteractiveJoystickControlEvent;

  public static event MixerInteractive.OnInteractiveMouseButtonEventHandler OnInteractiveMouseButtonEvent;

  public static event MixerInteractive.OnInteractiveCoordinatesChangedHandler OnInteractiveCoordinatesChangedEvent;

  public static event MixerInteractive.OnInteractiveTextControlEventHandler OnInteractiveTextControlEvent;

  public static event MixerInteractive.OnInteractiveMessageEventHandler OnInteractiveMessageEvent;

  public void Awake()
  {
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.transform.gameObject);
    this.gameObject.AddComponent<MixerInteractiveHelper>();
  }

  public void Start() => this.Initialize();

  public void Initialize()
  {
    if ((UnityEngine.Object) MixerInteractive.mixerDialog == (UnityEngine.Object) null)
      MixerInteractive.mixerDialog = UnityEngine.Object.FindObjectOfType<MixerInteractiveDialog>();
    if (MixerInteractive.queuedEvents == null)
      MixerInteractive.queuedEvents = new List<InteractiveEventArgs>();
    bool flag = false;
    if (MixerInteractive.interactivityManager == null)
    {
      MixerInteractive.interactivityManager = InteractivityManager.SingletonInstance;
      MixerInteractive.interactivityManager.OnError -= new InteractivityManager.OnErrorEventHandler(MixerInteractive.HandleError);
      MixerInteractive.interactivityManager.OnInteractivityStateChanged -= new InteractivityManager.OnInteractivityStateChangedHandler(MixerInteractive.HandleInteractivityStateChanged);
      MixerInteractive.interactivityManager.OnParticipantStateChanged -= new InteractivityManager.OnParticipantStateChangedHandler(MixerInteractive.HandleParticipantStateChanged);
      MixerInteractive.interactivityManager.OnInteractiveButtonEvent -= new InteractivityManager.OnInteractiveButtonEventHandler(MixerInteractive.HandleInteractiveButtonEvent);
      MixerInteractive.interactivityManager.OnInteractiveJoystickControlEvent -= new InteractivityManager.OnInteractiveJoystickControlEventHandler(MixerInteractive.HandleInteractiveJoystickControlEvent);
      MixerInteractive.interactivityManager.OnInteractiveMouseButtonEvent -= new InteractivityManager.OnInteractiveMouseButtonEventHandler(MixerInteractive.HandleInteractiveMouseButtonEvent);
      MixerInteractive.interactivityManager.OnInteractiveCoordinatesChangedEvent -= new InteractivityManager.OnInteractiveCoordinatesChangedHandler(MixerInteractive.HandleInteractiveCoordinatesChangedHandler);
      MixerInteractive.interactivityManager.OnInteractiveTextControlEvent -= new InteractivityManager.OnInteractiveTextControlEventHandler(this.HandleInteractiveTextControlEvent);
      MixerInteractive.interactivityManager.OnInteractiveMessageEvent -= new InteractivityManager.OnInteractiveMessageEventHandler(MixerInteractive.HandleInteractiveMessageEvent);
      MixerInteractive.interactivityManager.OnError += new InteractivityManager.OnErrorEventHandler(MixerInteractive.HandleError);
      MixerInteractive.interactivityManager.OnInteractivityStateChanged += new InteractivityManager.OnInteractivityStateChangedHandler(MixerInteractive.HandleInteractivityStateChanged);
      MixerInteractive.interactivityManager.OnParticipantStateChanged += new InteractivityManager.OnParticipantStateChangedHandler(MixerInteractive.HandleParticipantStateChanged);
      MixerInteractive.interactivityManager.OnInteractiveButtonEvent += new InteractivityManager.OnInteractiveButtonEventHandler(MixerInteractive.HandleInteractiveButtonEvent);
      MixerInteractive.interactivityManager.OnInteractiveMouseButtonEvent += new InteractivityManager.OnInteractiveMouseButtonEventHandler(MixerInteractive.HandleInteractiveMouseButtonEvent);
      MixerInteractive.interactivityManager.OnInteractiveCoordinatesChangedEvent += new InteractivityManager.OnInteractiveCoordinatesChangedHandler(MixerInteractive.HandleInteractiveCoordinatesChangedHandler);
      MixerInteractive.interactivityManager.OnInteractiveJoystickControlEvent += new InteractivityManager.OnInteractiveJoystickControlEventHandler(MixerInteractive.HandleInteractiveJoystickControlEvent);
      MixerInteractive.interactivityManager.OnInteractiveTextControlEvent += new InteractivityManager.OnInteractiveTextControlEventHandler(this.HandleInteractiveTextControlEvent);
      MixerInteractive.interactivityManager.OnInteractiveMessageEvent += new InteractivityManager.OnInteractiveMessageEventHandler(MixerInteractive.HandleInteractiveMessageEvent);
    }
    else
      flag = true;
    MixerInteractiveHelper singletonInstance = MixerInteractiveHelper._SingletonInstance;
    singletonInstance._runInBackgroundIfInteractive = this.runInBackground;
    singletonInstance._defaultSceneID = this.defaultSceneID;
    for (int index = 0; index < this.groupIDs.Count; ++index)
    {
      string groupId = this.groupIDs[index];
      if (groupId != string.Empty && !singletonInstance._groupSceneMapping.ContainsKey(groupId))
        singletonInstance._groupSceneMapping.Add(groupId, this.sceneIDs[index]);
    }
    if (MixerInteractive.outstandingCreateGroupsRequests == null)
      MixerInteractive.outstandingCreateGroupsRequests = new List<string>();
    MixerInteractive.outstandingSetDefaultSceneRequest = string.Empty;
    MixerInteractive.processedSerializedProperties = false;
    MixerInteractive.outstandingRequestsCompleted = false;
    MixerInteractive.shouldCheckForOutstandingRequests = false;
    MixerInteractive.lastCheckForOutstandingRequestsTime = -1f;
    MixerInteractive.outboundMessages = new List<string>();
    MixerInteractive.backgroundWorker = new BackgroundWorker();
    if (flag && InteractivityManager.SingletonInstance.InteractivityState == InteractivityState.InteractivityEnabled)
      MixerInteractive.ProcessSerializedProperties();
    MixerInteractive._websocket = this.gameObject.AddComponent<Websocket>();
    InteractivityManager.SingletonInstance.SetWebsocketInstance(MixerInteractive._websocket);
  }

  public static void HandleInteractiveJoystickControlEvent(
    object sender,
    InteractiveJoystickEventArgs e)
  {
    MixerInteractive.queuedEvents.Add((InteractiveEventArgs) e);
  }

  public static void HandleInteractiveMouseButtonEvent(
    object sender,
    InteractiveMouseButtonEventArgs e)
  {
    MixerInteractive.queuedEvents.Add((InteractiveEventArgs) e);
  }

  public static void HandleInteractiveCoordinatesChangedHandler(
    object sender,
    InteractiveCoordinatesChangedEventArgs e)
  {
    MixerInteractive.queuedEvents.Add((InteractiveEventArgs) e);
  }

  public static void HandleInteractiveButtonEvent(object sender, InteractiveButtonEventArgs e)
  {
    MixerInteractive.queuedEvents.Add((InteractiveEventArgs) e);
  }

  public void HandleInteractiveTextControlEvent(object sender, InteractiveTextEventArgs e)
  {
    MixerInteractive.queuedEvents.Add((InteractiveEventArgs) e);
  }

  public static void HandleParticipantStateChanged(object sender, InteractiveEventArgs e)
  {
    MixerInteractive.queuedEvents.Add(e);
  }

  public static void HandleInteractivityStateChanged(
    object sender,
    InteractivityStateChangedEventArgs e)
  {
    MixerInteractive.queuedEvents.Add((InteractiveEventArgs) e);
  }

  public static void HandleError(object sender, InteractiveEventArgs e)
  {
    MixerInteractive.queuedEvents.Add(e);
  }

  public static void HandleInteractiveMessageEvent(object sender, InteractiveEventArgs e)
  {
    MixerInteractive.queuedEvents.Add(e);
  }

  public static void InvokeRpcMethod(
    string methodName,
    List<MixerInteractive.MixerHelperParameterInfo> mixerParameterInfos)
  {
    if (MixerInteractive.FindAndInvokeRpcMethod(methodName, mixerParameterInfos))
      return;
    MixerInteractive.RefreshRPCMethods();
    MixerInteractive.FindAndInvokeRpcMethod(methodName, mixerParameterInfos);
  }

  public static bool FindAndInvokeRpcMethod(
    string methodName,
    List<MixerInteractive.MixerHelperParameterInfo> mixerParameterInfos)
  {
    bool andInvokeRpcMethod = false;
    MixerInteractive.RpcCachedMethodInfo cachedMethodInfo = new MixerInteractive.RpcCachedMethodInfo();
    if (MixerInteractiveHelper._SingletonInstance.cachedRPCMethods.TryGetValue(methodName, out cachedMethodInfo))
    {
      MethodInfo methodInfo = cachedMethodInfo.methodInfo;
      ParameterInfo[] parameters1 = methodInfo.GetParameters();
      object[] parameters2 = new object[parameters1.Length];
      for (int index = 0; index < parameters1.Length; ++index)
      {
        ParameterInfo parameterInfo = parameters1[index];
        string typeValue = mixerParameterInfos[index].typeValue;
        parameters2[index] = Convert.ChangeType((object) typeValue, parameterInfo.ParameterType);
      }
      andInvokeRpcMethod = true;
      try
      {
        methodInfo.Invoke((object) cachedMethodInfo.owningMonoBehavior, parameters2);
      }
      catch (Exception ex)
      {
        Debug.Log((object) $"Error calling method {cachedMethodInfo.owningMonoBehavior.name}.{methodInfo.Name}. Details: {ex.Message}");
      }
    }
    return andInvokeRpcMethod;
  }

  public static void AddRpcMethodsFromTheEditor(
    GameObject owningGameObject,
    List<string> rpcMethodNames)
  {
    if (rpcMethodNames.Count == 0)
      return;
    MixerInteractiveHelper singletonInstance = MixerInteractiveHelper._SingletonInstance;
    MonoBehaviour[] components = owningGameObject.GetComponents<MonoBehaviour>();
    foreach (string rpcMethodName in rpcMethodNames)
    {
      string str = MixerInteractive.TrimMethodName(rpcMethodName);
      foreach (MonoBehaviour monoBehaviour in components)
      {
        foreach (MethodInfo method in monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
          if (method.Name == str && !singletonInstance.cachedRPCMethods.ContainsKey(method.Name))
            singletonInstance.cachedRPCMethods.Add(method.Name, new MixerInteractive.RpcCachedMethodInfo()
            {
              owningMonoBehavior = monoBehaviour,
              methodInfo = method
            });
        }
      }
    }
  }

  public static string TrimMethodName(string unTrimmedMethodName)
  {
    return unTrimmedMethodName.Split('(')[0].Trim();
  }

  public static void RefreshRPCMethods(bool includeMethodsFromTheInspector = false)
  {
    MixerInteractiveHelper singletonInstance = MixerInteractiveHelper._SingletonInstance;
    foreach (MonoBehaviour monoBehaviour in UnityEngine.Object.FindObjectsOfType<MonoBehaviour>())
    {
      foreach (MethodInfo method in monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (method.IsDefined(typeof (MixerRpcMethod), false) && !singletonInstance.cachedRPCMethods.ContainsKey(method.Name))
          singletonInstance.cachedRPCMethods.Add(method.Name, new MixerInteractive.RpcCachedMethodInfo()
          {
            owningMonoBehavior = monoBehaviour,
            methodInfo = method
          });
        if (includeMethodsFromTheInspector)
        {
          string name = monoBehaviour.name;
          for (int index = 0; index < singletonInstance.rpcOwningMonoBehaviorNames.Count; ++index)
          {
            if (singletonInstance.rpcOwningMonoBehaviorNames[index] == name && singletonInstance.rpcMethodNames[index] == method.Name && !singletonInstance.cachedRPCMethods.ContainsKey(method.Name))
              singletonInstance.cachedRPCMethods.Add(method.Name, new MixerInteractive.RpcCachedMethodInfo()
              {
                owningMonoBehavior = monoBehaviour,
                methodInfo = method
              });
          }
        }
      }
    }
  }

  public static void FlushUpdates() => MixerInteractive.SendOutboundMessages();

  public static void SendOutboundMessages()
  {
    foreach (string outboundMessage in MixerInteractive.outboundMessages)
      InteractivityManager.SingletonInstance.SendMessage(outboundMessage);
  }

  public static void SerializeSyncVars()
  {
    foreach (MonoBehaviour monoBehaviour in UnityEngine.Object.FindObjectsOfType<MonoBehaviour>())
    {
      foreach (FieldInfo field in monoBehaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
      {
        if (Attribute.GetCustomAttribute((MemberInfo) field, typeof (MixerSyncVar)) is MixerSyncVar)
        {
          object obj = field.GetValue((object) monoBehaviour);
          string empty = string.Empty;
          if (obj != null)
            empty = obj.ToString();
          MixerInteractive.ParseAndSendCustomMessage(field.Name, empty);
        }
      }
    }
  }

  public static void ParseAndSendCustomMessage(string name, string value)
  {
    InteractivityManager.SingletonInstance.SendMessage($"{{   name: {name}   value: {value}}}");
  }

  public static void QueueCustomMessage(string newMessage)
  {
    if (MixerInteractive.outboundMessages == null)
      MixerInteractive.outboundMessages = new List<string>();
    MixerInteractive.outboundMessages.Add(newMessage);
  }

  public static int GetTimeSinceStartUpInMilliSeconds() => (int) Time.realtimeSinceStartup * 1000;

  public static string Token
  {
    get => InteractivityManager.SingletonInstance._authToken;
    set => InteractivityManager.SingletonInstance._authToken = value;
  }

  public static InteractivityState InteractivityState
  {
    get => InteractivityManager.SingletonInstance.InteractivityState;
  }

  public static IList<InteractiveGroup> Groups => InteractivityManager.SingletonInstance.Groups;

  public static IList<InteractiveScene> Scenes => InteractivityManager.SingletonInstance.Scenes;

  public static IList<InteractiveParticipant> Participants
  {
    get => InteractivityManager.SingletonInstance.Participants;
  }

  public static IList<InteractiveButtonControl> Buttons
  {
    get => InteractivityManager.SingletonInstance.Buttons;
  }

  public static IList<InteractiveJoystickControl> Joysticks
  {
    get => InteractivityManager.SingletonInstance.Joysticks;
  }

  public static bool ManuallyHandleSparkTransactions
  {
    get => MixerInteractive.\u003CManuallyHandleSparkTransactions\u003Ek__BackingField;
    set => MixerInteractive.\u003CManuallyHandleSparkTransactions\u003Ek__BackingField = value;
  }

  public static Vector3 MousePosition
  {
    get
    {
      Vector3 zero = Vector3.zero;
      if (InteractivityManager._mousePositionsByParticipant.Count > 0)
      {
        Dictionary<uint, Vector2> positionsByParticipant = InteractivityManager._mousePositionsByParticipant;
        Dictionary<uint, Vector2>.KeyCollection keys = positionsByParticipant.Keys;
        float num1 = 0.0f;
        float num2 = 0.0f;
        foreach (uint key in keys)
        {
          num1 += positionsByParticipant[key].x;
          num2 += positionsByParticipant[key].y;
        }
        zero.x = num1 / (float) keys.Count;
        zero.y = num2 / (float) keys.Count;
      }
      return zero;
    }
  }

  public static string ShortCode => InteractivityManager.SingletonInstance.ShortCode;

  public static InteractiveParticipant GetParticipantWhoGaveInputForControl(string controlID)
  {
    InteractiveParticipant gaveInputForControl = (InteractiveParticipant) null;
    _InternalParticipantTrackingState participantTrackingState = new _InternalParticipantTrackingState();
    if (InteractivityManager._participantsWhoTriggeredGiveInput.TryGetValue(controlID, out participantTrackingState))
      gaveInputForControl = participantTrackingState.particpant;
    return gaveInputForControl;
  }

  public static bool HasSubmissions(string controlID)
  {
    bool flag = false;
    if (MixerInteractive.GetControl(controlID) is InteractiveTextControl && MixerInteractive.GetText(controlID).Count > 0)
      flag = true;
    if (flag)
      MixerInteractive.CaptureTransactionForControlID(controlID);
    return flag;
  }

  public static void Initialize(bool goInteractive = true)
  {
    InteractivityManager.SingletonInstance.Initialize(goInteractive);
  }

  public static void TriggerCooldown(string controlID, int cooldown)
  {
    InteractivityManager.SingletonInstance.TriggerCooldown(controlID, cooldown);
  }

  public static void StartInteractive()
  {
    InteractivityManager.SingletonInstance.StartInteractive();
  }

  public static void StopInteractive()
  {
    InteractivityManager.SingletonInstance.StopInteractive();
    MixerInteractive.pendingGoInteractive = false;
    if (!MixerInteractiveHelper._SingletonInstance._runInBackgroundIfInteractive)
      return;
    Application.runInBackground = MixerInteractive.previousRunInBackgroundValue;
  }

  public static void DoWork()
  {
    InteractivityManager.SingletonInstance.DoWork();
    MixerInteractive.SendOutboundMessages();
  }

  public static void Dispose()
  {
    InteractivityManager singletonInstance = InteractivityManager.SingletonInstance;
    if (singletonInstance != null)
    {
      singletonInstance.OnInteractivityStateChanged -= new InteractivityManager.OnInteractivityStateChangedHandler(MixerInteractive.HandleInteractivityStateChangedInternal);
      MixerInteractive.backgroundWorker.DoWork -= new DoWorkEventHandler(MixerInteractive.BackgroundWorkerDoWork);
    }
    if (MixerInteractive.queuedEvents != null)
      MixerInteractive.queuedEvents.Clear();
    MixerInteractive.previousRunInBackgroundValue = true;
    MixerInteractive.pendingGoInteractive = false;
    MixerInteractive.outstandingSetDefaultSceneRequest = string.Empty;
    if (MixerInteractive.outstandingCreateGroupsRequests != null)
      MixerInteractive.outstandingCreateGroupsRequests.Clear();
    MixerInteractive.outstandingRequestsCompleted = false;
    MixerInteractive.lastCheckForOutstandingRequestsTime = -1f;
    MixerInteractive.processedSerializedProperties = false;
    MixerInteractive.hasFiredGoInteractiveEvent = false;
    singletonInstance.Dispose();
  }

  public void ResetInternalState()
  {
    MixerInteractive.previousRunInBackgroundValue = true;
    MixerInteractive.outstandingSetDefaultSceneRequest = string.Empty;
    if (MixerInteractive.outstandingCreateGroupsRequests != null)
      MixerInteractive.outstandingCreateGroupsRequests.Clear();
    MixerInteractive.outstandingRequestsCompleted = false;
    MixerInteractive.lastCheckForOutstandingRequestsTime = -1f;
    MixerInteractive.processedSerializedProperties = false;
  }

  public void Update()
  {
    if (MixerInteractive.processedSerializedProperties && MixerInteractive.shouldCheckForOutstandingRequests && !MixerInteractive.outstandingRequestsCompleted && (double) Time.time - (double) MixerInteractive.lastCheckForOutstandingRequestsTime > 1.0)
    {
      MixerInteractive.lastCheckForOutstandingRequestsTime = Time.time;
      MixerInteractive.outstandingRequestsCompleted = MixerInteractive.CheckForOutStandingRequestsCompleted();
    }
    MixerInteractive.DoWork();
    List<InteractiveEventArgs> interactiveEventArgsList = new List<InteractiveEventArgs>();
    if (MixerInteractive.queuedEvents != null)
    {
      foreach (InteractiveEventArgs queuedEvent in MixerInteractive.queuedEvents)
      {
        if (queuedEvent != null)
        {
          switch (queuedEvent.EventType)
          {
            case InteractiveEventType.Error:
              if (MixerInteractive.OnError != null)
                MixerInteractive.OnError((object) this, queuedEvent);
              interactiveEventArgsList.Add(queuedEvent);
              continue;
            case InteractiveEventType.InteractivityStateChanged:
              InteractivityStateChangedEventArgs e = queuedEvent as InteractivityStateChangedEventArgs;
              if (e.State == InteractivityState.InteractivityEnabled && (!MixerInteractive.shouldCheckForOutstandingRequests || MixerInteractive.outstandingRequestsCompleted) && !MixerInteractive.hasFiredGoInteractiveEvent && MixerInteractive.OnGoInteractive != null)
              {
                MixerInteractive.hasFiredGoInteractiveEvent = true;
                MixerInteractive.OnGoInteractive((object) this, (InteractiveEventArgs) e);
              }
              if (MixerInteractive.OnInteractivityStateChanged != null)
                MixerInteractive.OnInteractivityStateChanged((object) this, e);
              interactiveEventArgsList.Add(queuedEvent);
              continue;
            case InteractiveEventType.ParticipantStateChanged:
              if (MixerInteractive.outstandingRequestsCompleted)
              {
                if (MixerInteractive.OnParticipantStateChanged != null)
                  MixerInteractive.OnParticipantStateChanged((object) this, queuedEvent as InteractiveParticipantStateChangedEventArgs);
                interactiveEventArgsList.Add(queuedEvent);
                continue;
              }
              continue;
            case InteractiveEventType.Button:
              if (MixerInteractive.OnInteractiveButtonEvent != null)
                MixerInteractive.OnInteractiveButtonEvent((object) this, queuedEvent as InteractiveButtonEventArgs);
              interactiveEventArgsList.Add(queuedEvent);
              continue;
            case InteractiveEventType.Joystick:
              if (MixerInteractive.OnInteractiveJoystickControlEvent != null)
                MixerInteractive.OnInteractiveJoystickControlEvent((object) this, queuedEvent as InteractiveJoystickEventArgs);
              interactiveEventArgsList.Add(queuedEvent);
              continue;
            case InteractiveEventType.MouseButton:
              if (MixerInteractive.OnInteractiveMouseButtonEvent != null)
                MixerInteractive.OnInteractiveMouseButtonEvent((object) this, queuedEvent as InteractiveMouseButtonEventArgs);
              interactiveEventArgsList.Add(queuedEvent);
              continue;
            case InteractiveEventType.Coordinates:
              if (MixerInteractive.OnInteractiveCoordinatesChangedEvent != null)
                MixerInteractive.OnInteractiveCoordinatesChangedEvent((object) this, queuedEvent as InteractiveCoordinatesChangedEventArgs);
              interactiveEventArgsList.Add(queuedEvent);
              continue;
            case InteractiveEventType.TextInput:
              if (MixerInteractive.OnInteractiveTextControlEvent != null)
                MixerInteractive.OnInteractiveTextControlEvent((object) this, queuedEvent as InteractiveTextEventArgs);
              interactiveEventArgsList.Add(queuedEvent);
              continue;
            default:
              if (MixerInteractive.OnInteractiveMessageEvent != null)
                MixerInteractive.OnInteractiveMessageEvent((object) this, queuedEvent as InteractiveMessageEventArgs);
              interactiveEventArgsList.Add(queuedEvent);
              continue;
          }
        }
      }
      foreach (InteractiveEventArgs interactiveEventArgs in interactiveEventArgsList)
        MixerInteractive.queuedEvents.Remove(interactiveEventArgs);
    }
    if (InteractivityManager.SingletonInstance.InteractivityState != InteractivityState.InteractivityEnabled || !MixerInteractive.shouldCheckForOutstandingRequests || !MixerInteractive.outstandingRequestsCompleted || MixerInteractive.hasFiredGoInteractiveEvent || MixerInteractive.OnGoInteractive == null)
      return;
    MixerInteractive.hasFiredGoInteractiveEvent = true;
    MixerInteractive.OnGoInteractive((object) this, new InteractiveEventArgs());
  }

  public static bool GetButtonDown(string controlID)
  {
    int num = InteractivityManager.SingletonInstance.GetButton(controlID).ButtonDown ? 1 : 0;
    if (num == 0)
      return num != 0;
    if (MixerInteractive.ManuallyHandleSparkTransactions)
      return num != 0;
    MixerInteractive.CaptureTransactionForButtonControlID(controlID);
    return num != 0;
  }

  public static bool GetButton(string controlID)
  {
    int num = InteractivityManager.SingletonInstance.GetButton(controlID).ButtonPressed ? 1 : 0;
    if (num == 0)
      return num != 0;
    if (MixerInteractive.ManuallyHandleSparkTransactions)
      return num != 0;
    MixerInteractive.CaptureTransactionForButtonControlID(controlID);
    return num != 0;
  }

  public static bool GetButtonUp(string controlID)
  {
    int num = InteractivityManager.SingletonInstance.GetButton(controlID).ButtonUp ? 1 : 0;
    if (num == 0)
      return num != 0;
    if (MixerInteractive.ManuallyHandleSparkTransactions)
      return num != 0;
    MixerInteractive.CaptureTransactionForButtonControlID(controlID);
    return num != 0;
  }

  public static uint GetCountOfButtonDowns(string controlID)
  {
    return InteractivityManager.SingletonInstance.GetButton(controlID).CountOfButtonDowns;
  }

  public static uint GetCountOfButtons(string controlID)
  {
    return InteractivityManager.SingletonInstance.GetButton(controlID).CountOfButtonPresses;
  }

  public static uint GetCountOfButtonUps(string controlID)
  {
    return InteractivityManager.SingletonInstance.GetButton(controlID).CountOfButtonUps;
  }

  public static InteractiveJoystickControl GetJoystick(string controlID)
  {
    return InteractivityManager.SingletonInstance.GetJoystick(controlID);
  }

  public static float GetJoystickX(string controlID)
  {
    return (float) InteractivityManager.SingletonInstance.GetJoystick(controlID).X;
  }

  public static float GetJoystickY(string controlID)
  {
    return (float) InteractivityManager.SingletonInstance.GetJoystick(controlID).Y;
  }

  public static bool GetMouseButtonDown(int buttonIndex = 0)
  {
    bool mouseButtonDown = false;
    Dictionary<uint, _InternalMouseButtonState> stateByParticipant = InteractivityManager._mouseButtonStateByParticipant;
    foreach (uint key in stateByParticipant.Keys)
    {
      if (stateByParticipant[key].IsDown)
      {
        mouseButtonDown = true;
        break;
      }
    }
    return mouseButtonDown;
  }

  public static bool GetMouseButton(int buttonIndex = 0)
  {
    bool mouseButton = false;
    Dictionary<uint, _InternalMouseButtonState> stateByParticipant = InteractivityManager._mouseButtonStateByParticipant;
    foreach (uint key in stateByParticipant.Keys)
    {
      if (stateByParticipant[key].IsPressed)
      {
        mouseButton = true;
        break;
      }
    }
    return mouseButton;
  }

  public static bool GetMouseButtonUp(int buttonIndex = 0)
  {
    bool mouseButtonUp = false;
    Dictionary<uint, _InternalMouseButtonState> stateByParticipant = InteractivityManager._mouseButtonStateByParticipant;
    foreach (uint key in stateByParticipant.Keys)
    {
      if (stateByParticipant[key].IsUp)
      {
        mouseButtonUp = true;
        break;
      }
    }
    return mouseButtonUp;
  }

  public static InteractiveButtonControl Button(string controlID)
  {
    return InteractivityManager.SingletonInstance.GetButton(controlID);
  }

  public static string GetCurrentScene()
  {
    return InteractivityManager.SingletonInstance.GetCurrentScene();
  }

  public static void SetCurrentScene(string sceneID)
  {
    InteractivityManager.SingletonInstance.SetCurrentScene(sceneID);
  }

  public static InteractiveGroup GetGroup(string groupID)
  {
    return InteractivityManager.SingletonInstance.GetGroup(groupID);
  }

  public static InteractiveScene GetScene(string sceneID)
  {
    return InteractivityManager.SingletonInstance.GetScene(sceneID);
  }

  public static void SendInteractiveMessage(string message)
  {
    InteractivityManager.SingletonInstance.SendMessage(message);
  }

  public static void SendInteractiveMessage(
    string messageType,
    Dictionary<string, object> parameters)
  {
    InteractivityManager.SingletonInstance.SendMessage(messageType, parameters);
  }

  public static void ClearSavedLoginInformation()
  {
    PlayerPrefs.DeleteKey("MixerInteractive-AuthToken");
    PlayerPrefs.DeleteKey("MixerInteractive-RefreshToken");
    PlayerPrefs.Save();
  }

  public IEnumerator InitializeCoRoutine()
  {
    using (UnityWebRequest request = UnityWebRequest.Get("https://mixer.com/api/v1/interactive/hosts"))
    {
      yield return (object) request.SendWebRequest();
      if (request.isNetworkError)
      {
        Debug.Log((object) ("Error: Could not retrieve websocket URL. " + request.error));
      }
      else
      {
        string text = request.downloadHandler.text;
        InteractivityManager.SingletonInstance.Initialize(authToken: (string) null);
      }
    }
  }

  public static InteractiveControl GetControl(string controlID)
  {
    return InteractivityManager.SingletonInstance._GetControl(controlID);
  }

  public static IList<InteractiveTextResult> GetText(string controlID)
  {
    return InteractivityManager.SingletonInstance._GetText(controlID);
  }

  public static void GoInteractive()
  {
    if (MixerInteractive.pendingGoInteractive)
      return;
    MixerInteractive.pendingGoInteractive = true;
    MixerInteractive.hasFiredGoInteractiveEvent = false;
    InteractivityManager singletonInstance = InteractivityManager.SingletonInstance;
    singletonInstance.OnInteractivityStateChanged -= new InteractivityManager.OnInteractivityStateChangedHandler(MixerInteractive.HandleInteractivityStateChangedInternal);
    singletonInstance.OnInteractivityStateChanged += new InteractivityManager.OnInteractivityStateChangedHandler(MixerInteractive.HandleInteractivityStateChangedInternal);
    if (MixerInteractive.backgroundWorker == null)
      MixerInteractive.backgroundWorker = new BackgroundWorker();
    MixerInteractive.backgroundWorker.DoWork -= new DoWorkEventHandler(MixerInteractive.BackgroundWorkerDoWork);
    MixerInteractive.backgroundWorker.DoWork += new DoWorkEventHandler(MixerInteractive.BackgroundWorkerDoWork);
    MixerInteractive.backgroundWorker.RunWorkerAsync();
    if (!MixerInteractiveHelper._SingletonInstance._runInBackgroundIfInteractive)
      return;
    MixerInteractive.previousRunInBackgroundValue = Application.runInBackground;
    Application.runInBackground = true;
  }

  public static void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
  {
    InteractivityManager.SingletonInstance.Initialize();
  }

  public static void HandleInteractivityStateChangedInternal(
    object sender,
    InteractivityStateChangedEventArgs e)
  {
    if (e == null)
      return;
    switch (e.State)
    {
      case InteractivityState.ShortCodeRequired:
        if (!MixerInteractive.mixerDialog.gameObject.activeInHierarchy)
          MixerInteractive.mixerDialog.gameObject.SetActive(true);
        MixerInteractive.mixerDialog.Show(InteractivityManager.SingletonInstance.ShortCode);
        break;
      case InteractivityState.InteractivityEnabled:
        MixerInteractive.mixerDialog.Hide();
        MixerInteractive.ProcessSerializedProperties();
        MixerInteractive.pendingGoInteractive = false;
        break;
    }
  }

  public static void ProcessSerializedProperties()
  {
    MixerInteractiveHelper singletonInstance1 = MixerInteractiveHelper._SingletonInstance;
    InteractivityManager singletonInstance2 = InteractivityManager.SingletonInstance;
    string defaultSceneId = singletonInstance1._defaultSceneID;
    if (singletonInstance1._groupSceneMapping.Count > 0 || defaultSceneId != string.Empty)
      MixerInteractive.shouldCheckForOutstandingRequests = true;
    if (singletonInstance1._groupSceneMapping.Count > 0)
    {
      foreach (string key in singletonInstance1._groupSceneMapping.Keys)
      {
        if (!(key == string.Empty))
        {
          string sceneID = singletonInstance1._groupSceneMapping[key];
          if (sceneID != string.Empty)
          {
            InteractiveGroup interactiveGroup1 = new InteractiveGroup(key, sceneID);
          }
          else
          {
            InteractiveGroup interactiveGroup2 = new InteractiveGroup(key);
          }
          MixerInteractive.outstandingCreateGroupsRequests.Add(key);
        }
      }
      if (defaultSceneId != string.Empty)
      {
        singletonInstance2.SetCurrentScene(defaultSceneId);
        MixerInteractive.outstandingSetDefaultSceneRequest = defaultSceneId;
      }
    }
    MixerInteractive.processedSerializedProperties = true;
  }

  public static bool CheckForOutStandingRequestsCompleted()
  {
    bool flag = false;
    List<string> stringList = new List<string>();
    if (MixerInteractive.outstandingSetDefaultSceneRequest == string.Empty)
    {
      foreach (string createGroupsRequest in MixerInteractive.outstandingCreateGroupsRequests)
      {
        foreach (InteractiveGroup group in (IEnumerable<InteractiveGroup>) InteractivityManager.SingletonInstance.Groups)
        {
          if (group.GroupID == createGroupsRequest)
            stringList.Add(createGroupsRequest);
        }
      }
      foreach (string str in stringList)
        MixerInteractive.outstandingCreateGroupsRequests.Remove(str);
    }
    else
    {
      foreach (InteractiveGroup group in (IEnumerable<InteractiveGroup>) InteractivityManager.SingletonInstance.Groups)
      {
        if (group.GroupID == "default" && group.SceneID == MixerInteractive.outstandingSetDefaultSceneRequest)
        {
          MixerInteractive.outstandingSetDefaultSceneRequest = string.Empty;
          break;
        }
      }
    }
    if (MixerInteractive.outstandingCreateGroupsRequests.Count == 0 && MixerInteractive.outstandingSetDefaultSceneRequest == string.Empty)
      flag = true;
    return flag;
  }

  public static void CaptureTransactionForButtonControlID(string controlID)
  {
    IList<InteractiveButtonControl> buttons = MixerInteractive.Buttons;
    foreach (string key in InteractivityManager._buttonStates.Keys)
    {
      if (key == controlID)
      {
        InteractivityManager.SingletonInstance.CaptureTransaction(InteractivityManager._buttonStates[key].TransactionID);
        break;
      }
    }
  }

  public static void CaptureTransactionForControlID(string controlID)
  {
    foreach (string key in InteractivityManager._transactionIDsState.Keys)
    {
      if (key == controlID)
      {
        InteractivityManager.SingletonInstance.CaptureTransaction(InteractivityManager._transactionIDsState[key].transactionID);
        break;
      }
    }
  }

  public void OnDestroy() => this.ResetInternalState();

  public void OnApplicationQuit() => MixerInteractive.StopInteractive();

  public delegate void OnErrorEventHandler(object sender, InteractiveEventArgs e);

  public delegate void OnGoInteractiveHandler(object sender, InteractiveEventArgs e);

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

  public class RpcCachedMethodInfo
  {
    public MonoBehaviour owningMonoBehavior;
    public MethodInfo methodInfo;
  }

  public class ObservedCachedFieldInfo
  {
    public FieldInfo fieldInfo;
    public object owningObject;
    public float updateInterval;
    public float lastSendTime;
    public string previousValueAsString;
  }

  public struct MixerHelperParameterInfo
  {
    public string typeName;
    public string typeValue;
  }
}
