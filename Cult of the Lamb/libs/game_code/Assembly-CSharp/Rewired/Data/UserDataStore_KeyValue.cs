// Decompiled with JetBrains decompiler
// Type: Rewired.Data.UserDataStore_KeyValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Utils.Libraries.TinyJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
namespace Rewired.Data;

public abstract class UserDataStore_KeyValue : UserDataStore
{
  public static string thisScriptName = typeof (UserDataStore_KeyValue).Name;
  public const string logPrefix = "Rewired: ";
  public const string key_controllerAssignments = "ControllerAssignments";
  public const int controllerMapKeyVersion = 0;
  [Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
  [SerializeField]
  public bool _isEnabled = true;
  [Tooltip("Should saved data be loaded on start?")]
  [SerializeField]
  public bool _loadDataOnStart = true;
  [Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")]
  [SerializeField]
  public bool _loadJoystickAssignments = true;
  [Tooltip("Should Player Keyboard assignments be saved and loaded?")]
  [SerializeField]
  public bool _loadKeyboardAssignments = true;
  [Tooltip("Should Player Mouse assignments be saved and loaded?")]
  [SerializeField]
  public bool _loadMouseAssignments = true;
  [NonSerialized]
  public bool _allowImpreciseJoystickAssignmentMatching = true;
  [NonSerialized]
  public bool _deferredJoystickAssignmentLoadPending;
  [NonSerialized]
  public bool _wasJoystickEverDetected;
  [NonSerialized]
  public List<int> __allActionIds;
  [NonSerialized]
  public string __allActionIdsString;
  [NonSerialized]
  public StringBuilder _sb = new StringBuilder();

  public bool isEnabled
  {
    get => this._isEnabled;
    set => this._isEnabled = value;
  }

  public bool loadDataOnStart
  {
    get => this._loadDataOnStart;
    set => this._loadDataOnStart = value;
  }

  public bool loadJoystickAssignments
  {
    get => this._loadJoystickAssignments;
    set => this._loadJoystickAssignments = value;
  }

  public bool loadKeyboardAssignments
  {
    get => this._loadKeyboardAssignments;
    set => this._loadKeyboardAssignments = value;
  }

  public bool loadMouseAssignments
  {
    get => this._loadMouseAssignments;
    set => this._loadMouseAssignments = value;
  }

  public abstract UserDataStore_KeyValue.IDataStore dataStore { get; }

  public bool loadControllerAssignments
  {
    get
    {
      return this._loadKeyboardAssignments || this._loadMouseAssignments || this._loadJoystickAssignments;
    }
  }

  public List<int> allActionIds
  {
    get
    {
      if (this.__allActionIds != null)
        return this.__allActionIds;
      List<int> allActionIds = new List<int>();
      IList<InputAction> actions = (IList<InputAction>) ReInput.mapping.Actions;
      for (int index = 0; index < actions.Count; ++index)
        allActionIds.Add(actions[index].id);
      this.__allActionIds = allActionIds;
      return allActionIds;
    }
  }

  public string allActionIdsString
  {
    get
    {
      if (!string.IsNullOrEmpty(this.__allActionIdsString))
        return this.__allActionIdsString;
      StringBuilder stringBuilder = new StringBuilder();
      List<int> allActionIds = this.allActionIds;
      for (int index = 0; index < allActionIds.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(",");
        stringBuilder.Append(allActionIds[index]);
      }
      this.__allActionIdsString = stringBuilder.ToString();
      return this.__allActionIdsString;
    }
  }

  public override void Save()
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SaveAll();
  }

  public override void SaveControllerData(
    int playerId,
    ControllerType controllerType,
    int controllerId)
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SaveControllerDataNow(playerId, controllerType, controllerId);
  }

  public override void SaveControllerData(ControllerType controllerType, int controllerId)
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SaveControllerDataNow(controllerType, controllerId);
  }

  public override void SavePlayerData(int playerId)
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SavePlayerDataNow(playerId);
  }

  public override void SaveInputBehavior(int playerId, int behaviorId)
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SaveInputBehaviorNow(playerId, behaviorId);
  }

  public override void Load()
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadAll();
  }

  public override void LoadControllerData(
    int playerId,
    ControllerType controllerType,
    int controllerId)
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadControllerDataNow(playerId, controllerType, controllerId);
  }

  public override void LoadControllerData(ControllerType controllerType, int controllerId)
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadControllerDataNow(controllerType, controllerId);
  }

  public override void LoadPlayerData(int playerId)
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadPlayerDataNow(playerId);
  }

  public override void LoadInputBehavior(int playerId, int behaviorId)
  {
    if (!this._isEnabled)
      Debug.LogWarning((object) $"Rewired: {UserDataStore_KeyValue.thisScriptName} is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadInputBehaviorNow(playerId, behaviorId);
  }

  public override void OnInitialize()
  {
    if (!this._loadDataOnStart)
      return;
    this.Load();
    if (!this.loadControllerAssignments || ReInput.controllers.joystickCount <= 0)
      return;
    this._wasJoystickEverDetected = true;
    this.SaveControllerAssignments();
  }

  public override void OnControllerConnected(ControllerStatusChangedEventArgs args)
  {
    if (!this._isEnabled || args.controllerType != ControllerType.Joystick)
      return;
    this.LoadJoystickData(args.controllerId);
    if (this._loadDataOnStart && this._loadJoystickAssignments && !this._wasJoystickEverDetected)
      this.StartCoroutine((IEnumerator) this.LoadJoystickAssignmentsDeferred());
    if (this._loadJoystickAssignments && !this._deferredJoystickAssignmentLoadPending)
      this.SaveControllerAssignments();
    this._wasJoystickEverDetected = true;
  }

  public override void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
  {
    if (!this._isEnabled || args.controllerType != ControllerType.Joystick)
      return;
    this.SaveJoystickData(args.controllerId);
  }

  public override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
  {
    if (!this._isEnabled || !this.loadControllerAssignments)
      return;
    this.SaveControllerAssignments();
  }

  public override void SaveControllerMap(int playerId, ControllerMap controllerMap)
  {
    if (controllerMap == null)
      return;
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null)
      return;
    this.SaveControllerMap(player, controllerMap);
    this.dataStore.Save();
  }

  public override ControllerMap LoadControllerMap(
    int playerId,
    ControllerIdentifier controllerIdentifier,
    int categoryId,
    int layoutId)
  {
    Player player = ReInput.players.GetPlayer(playerId);
    return player == null ? (ControllerMap) null : this.LoadControllerMap(player, controllerIdentifier, categoryId, layoutId);
  }

  public virtual void ClearSaveData() => this.dataStore.Clear();

  public int LoadAll()
  {
    int num = 0;
    if (this.loadControllerAssignments && this.LoadControllerAssignmentsNow())
      ++num;
    IList<Player> allPlayers = (IList<Player>) ReInput.players.AllPlayers;
    for (int index = 0; index < allPlayers.Count; ++index)
      num += this.LoadPlayerDataNow(allPlayers[index]);
    return num + this.LoadAllJoystickCalibrationData();
  }

  public int LoadPlayerDataNow(int playerId)
  {
    return this.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId));
  }

  public int LoadPlayerDataNow(Player player)
  {
    if (player == null)
      return 0;
    int num = 0 + this.LoadInputBehaviors(player.id) + this.LoadControllerMaps(player.id, ControllerType.Keyboard, 0) + this.LoadControllerMaps(player.id, ControllerType.Mouse, 0);
    foreach (Joystick joystick in (IEnumerable<Joystick>) player.controllers.Joysticks)
      num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystick.id);
    this.RefreshLayoutManager(player.id);
    return num;
  }

  public int LoadAllJoystickCalibrationData()
  {
    int num = 0;
    IList<Joystick> joysticks = (IList<Joystick>) ReInput.controllers.Joysticks;
    for (int index = 0; index < joysticks.Count; ++index)
      num += this.LoadJoystickCalibrationData(joysticks[index]);
    return num;
  }

  public int LoadJoystickCalibrationData(Joystick joystick)
  {
    return joystick == null || !joystick.ImportCalibrationMapFromJsonString(this.GetJoystickCalibrationMapJson(joystick)) ? 0 : 1;
  }

  public int LoadJoystickCalibrationData(int joystickId)
  {
    return this.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
  }

  public int LoadJoystickData(int joystickId)
  {
    int num = 0;
    IList<Player> allPlayers = (IList<Player>) ReInput.players.AllPlayers;
    for (int index = 0; index < allPlayers.Count; ++index)
    {
      Player player = allPlayers[index];
      if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
      {
        num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystickId);
        this.RefreshLayoutManager(player.id);
      }
    }
    return num + this.LoadJoystickCalibrationData(joystickId);
  }

  public int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
  {
    int num1 = 0 + this.LoadControllerMaps(playerId, controllerType, controllerId);
    this.RefreshLayoutManager(playerId);
    int num2 = this.LoadControllerDataNow(controllerType, controllerId);
    return num1 + num2;
  }

  public int LoadControllerDataNow(ControllerType controllerType, int controllerId)
  {
    int num = 0;
    if (controllerType == ControllerType.Joystick)
      num += this.LoadJoystickCalibrationData(controllerId);
    return num;
  }

  public int LoadControllerMaps(int playerId, ControllerType controllerType, int controllerId)
  {
    int num = 0;
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null)
      return num;
    Controller controller = ReInput.controllers.GetController(controllerType, controllerId);
    if (controller == null)
      return num;
    IList<InputMapCategory> mapCategories = (IList<InputMapCategory>) ReInput.mapping.MapCategories;
    for (int index1 = 0; index1 < mapCategories.Count; ++index1)
    {
      InputMapCategory inputMapCategory = mapCategories[index1];
      if (inputMapCategory.userAssignable)
      {
        IList<InputLayout> inputLayoutList = (IList<InputLayout>) ReInput.mapping.MapLayouts(controller.type);
        for (int index2 = 0; index2 < inputLayoutList.Count; ++index2)
        {
          InputLayout inputLayout = inputLayoutList[index2];
          ControllerMap map = this.LoadControllerMap(player, controller.identifier, inputMapCategory.id, inputLayout.id);
          if (map != null)
          {
            player.controllers.maps.AddMap(controller, map);
            ++num;
          }
        }
      }
    }
    return num;
  }

  public ControllerMap LoadControllerMap(
    Player player,
    ControllerIdentifier controllerIdentifier,
    int categoryId,
    int layoutId)
  {
    if (player == null)
      return (ControllerMap) null;
    string controllerMapJson = this.GetControllerMapJson(player, controllerIdentifier, categoryId, layoutId);
    if (string.IsNullOrEmpty(controllerMapJson))
      return (ControllerMap) null;
    ControllerMap fromJson = ControllerMap.CreateFromJson(controllerIdentifier.controllerType, controllerMapJson);
    if (fromJson == null)
      return (ControllerMap) null;
    List<int> mapKnownActionIds = this.GetControllerMapKnownActionIds(player, controllerIdentifier, categoryId, layoutId);
    this.AddDefaultMappingsForNewActions(controllerIdentifier, fromJson, mapKnownActionIds);
    return fromJson;
  }

  public int LoadInputBehaviors(int playerId)
  {
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null)
      return 0;
    int num = 0;
    IList<InputBehavior> inputBehaviors = (IList<InputBehavior>) ReInput.mapping.GetInputBehaviors(player.id);
    for (int index = 0; index < inputBehaviors.Count; ++index)
      num += this.LoadInputBehaviorNow(player, inputBehaviors[index]);
    return num;
  }

  public int LoadInputBehaviorNow(int playerId, int behaviorId)
  {
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null)
      return 0;
    InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
    return inputBehavior == null ? 0 : this.LoadInputBehaviorNow(player, inputBehavior);
  }

  public int LoadInputBehaviorNow(Player player, InputBehavior inputBehavior)
  {
    if (player == null || inputBehavior == null)
      return 0;
    string inputBehaviorJson = this.GetInputBehaviorJson(player, inputBehavior.id);
    return inputBehaviorJson == null || inputBehaviorJson == string.Empty || !inputBehavior.ImportJsonString(inputBehaviorJson) ? 0 : 1;
  }

  public bool LoadControllerAssignmentsNow()
  {
    try
    {
      UserDataStore_KeyValue.ControllerAssignmentSaveInfo data = this.LoadControllerAssignmentData();
      if (data == null)
        return false;
      if (this._loadKeyboardAssignments || this._loadMouseAssignments)
        this.LoadKeyboardAndMouseAssignmentsNow(data);
      if (this._loadJoystickAssignments)
        this.LoadJoystickAssignmentsNow(data);
    }
    catch
    {
    }
    return true;
  }

  public bool LoadKeyboardAndMouseAssignmentsNow(
    UserDataStore_KeyValue.ControllerAssignmentSaveInfo data)
  {
    try
    {
      if (data == null && (data = this.LoadControllerAssignmentData()) == null)
        return false;
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
      {
        if (data.ContainsPlayer(allPlayer.id))
        {
          UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(allPlayer.id)];
          if (this._loadKeyboardAssignments)
            allPlayer.controllers.hasKeyboard = player.hasKeyboard;
          if (this._loadMouseAssignments)
            allPlayer.controllers.hasMouse = player.hasMouse;
        }
      }
    }
    catch
    {
    }
    return true;
  }

  public bool LoadJoystickAssignmentsNow(
    UserDataStore_KeyValue.ControllerAssignmentSaveInfo data)
  {
    try
    {
      if (ReInput.controllers.joystickCount == 0 || data == null && (data = this.LoadControllerAssignmentData()) == null)
        return false;
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
        allPlayer.controllers.ClearControllersOfType(ControllerType.Joystick);
      List<UserDataStore_KeyValue.JoystickAssignmentHistoryInfo> assignmentHistoryInfoList = this._loadJoystickAssignments ? new List<UserDataStore_KeyValue.JoystickAssignmentHistoryInfo>() : (List<UserDataStore_KeyValue.JoystickAssignmentHistoryInfo>) null;
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
      {
        if (data.ContainsPlayer(allPlayer.id))
        {
          UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(allPlayer.id)];
          for (int index = 0; index < player.joystickCount; ++index)
          {
            UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo joystick1 = player.joysticks[index];
            if (joystick1 != null)
            {
              Joystick joystick = this.FindJoystickPrecise(joystick1);
              if (joystick != null)
              {
                if (assignmentHistoryInfoList.Find((Predicate<UserDataStore_KeyValue.JoystickAssignmentHistoryInfo>) (x => x.joystick == joystick)) == null)
                  assignmentHistoryInfoList.Add(new UserDataStore_KeyValue.JoystickAssignmentHistoryInfo(joystick, joystick1.id));
                allPlayer.controllers.AddController((Controller) joystick, false);
              }
            }
          }
        }
      }
      if (this._allowImpreciseJoystickAssignmentMatching)
      {
        foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
        {
          if (data.ContainsPlayer(allPlayer.id))
          {
            UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(allPlayer.id)];
            for (int index1 = 0; index1 < player.joystickCount; ++index1)
            {
              UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = player.joysticks[index1];
              if (joystickInfo != null)
              {
                Joystick joystick2 = (Joystick) null;
                int index2 = assignmentHistoryInfoList.FindIndex((Predicate<UserDataStore_KeyValue.JoystickAssignmentHistoryInfo>) (x => x.oldJoystickId == joystickInfo.id));
                if (index2 >= 0)
                {
                  joystick2 = assignmentHistoryInfoList[index2].joystick;
                }
                else
                {
                  List<Joystick> matches;
                  if (this.TryFindJoysticksImprecise(joystickInfo, out matches))
                  {
                    foreach (Joystick joystick3 in matches)
                    {
                      Joystick match = joystick3;
                      if (assignmentHistoryInfoList.Find((Predicate<UserDataStore_KeyValue.JoystickAssignmentHistoryInfo>) (x => x.joystick == match)) == null)
                      {
                        joystick2 = match;
                        break;
                      }
                    }
                    if (joystick2 != null)
                      assignmentHistoryInfoList.Add(new UserDataStore_KeyValue.JoystickAssignmentHistoryInfo(joystick2, joystickInfo.id));
                    else
                      continue;
                  }
                  else
                    continue;
                }
                allPlayer.controllers.AddController((Controller) joystick2, false);
              }
            }
          }
        }
      }
    }
    catch
    {
    }
    if (ReInput.configuration.autoAssignJoysticks)
      ReInput.controllers.AutoAssignJoysticks();
    return true;
  }

  public UserDataStore_KeyValue.ControllerAssignmentSaveInfo LoadControllerAssignmentData()
  {
    try
    {
      string result;
      if (!UserDataStore_KeyValue.TryGetString(this.dataStore, "ControllerAssignments", out result) || string.IsNullOrEmpty(result))
        return (UserDataStore_KeyValue.ControllerAssignmentSaveInfo) null;
      UserDataStore_KeyValue.ControllerAssignmentSaveInfo assignmentSaveInfo = JsonParser.FromJson<UserDataStore_KeyValue.ControllerAssignmentSaveInfo>(result);
      return assignmentSaveInfo == null || assignmentSaveInfo.playerCount == 0 ? (UserDataStore_KeyValue.ControllerAssignmentSaveInfo) null : assignmentSaveInfo;
    }
    catch
    {
      return (UserDataStore_KeyValue.ControllerAssignmentSaveInfo) null;
    }
  }

  public IEnumerator LoadJoystickAssignmentsDeferred()
  {
    this._deferredJoystickAssignmentLoadPending = true;
    yield return (object) new WaitForEndOfFrame();
    if (ReInput.isReady)
    {
      this.LoadJoystickAssignmentsNow((UserDataStore_KeyValue.ControllerAssignmentSaveInfo) null);
      this.SaveControllerAssignments();
      this._deferredJoystickAssignmentLoadPending = false;
    }
  }

  public void SaveAll()
  {
    IList<Player> allPlayers = (IList<Player>) ReInput.players.AllPlayers;
    for (int index = 0; index < allPlayers.Count; ++index)
      this.SavePlayerDataNow(allPlayers[index]);
    this.SaveAllJoystickCalibrationData();
    if (this.loadControllerAssignments)
      this.SaveControllerAssignments();
    this.dataStore.Save();
  }

  public void SavePlayerDataNow(int playerId)
  {
    this.SavePlayerDataNow(ReInput.players.GetPlayer(playerId));
    this.dataStore.Save();
  }

  public void SavePlayerDataNow(Player player)
  {
    if (player == null)
      return;
    PlayerSaveData saveData = player.GetSaveData(true);
    this.SaveInputBehaviors(player, saveData);
    this.SaveControllerMaps(player, saveData);
  }

  public void SaveAllJoystickCalibrationData()
  {
    IList<Joystick> joysticks = (IList<Joystick>) ReInput.controllers.Joysticks;
    for (int index = 0; index < joysticks.Count; ++index)
      this.SaveJoystickCalibrationData(joysticks[index]);
  }

  public void SaveJoystickCalibrationData(int joystickId)
  {
    this.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
  }

  public void SaveJoystickCalibrationData(Joystick joystick)
  {
    if (joystick == null)
      return;
    JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
    this.dataStore.SetValue(this.GetJoystickCalibrationMapKey(joystick), (object) calibrationMapSaveData.map.ToJsonString());
  }

  public void SaveJoystickData(int joystickId)
  {
    IList<Player> allPlayers = (IList<Player>) ReInput.players.AllPlayers;
    for (int index = 0; index < allPlayers.Count; ++index)
    {
      Player player = allPlayers[index];
      if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
        this.SaveControllerMaps(player.id, ControllerType.Joystick, joystickId);
    }
    this.SaveJoystickCalibrationData(joystickId);
  }

  public void SaveControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
  {
    this.SaveControllerMaps(playerId, controllerType, controllerId);
    this.SaveControllerData(controllerType, controllerId);
  }

  public void SaveControllerDataNow(ControllerType controllerType, int controllerId)
  {
    if (controllerType != ControllerType.Joystick)
      return;
    this.SaveJoystickCalibrationData(controllerId);
  }

  public void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
  {
    foreach (ControllerMapSaveData controllerMapSaveData in (IEnumerable<ControllerMapSaveData>) playerSaveData.AllControllerMapSaveData)
      this.SaveControllerMap(player, controllerMapSaveData.map);
  }

  public void SaveControllerMaps(int playerId, ControllerType controllerType, int controllerId)
  {
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null || !player.controllers.ContainsController(controllerType, controllerId))
      return;
    ControllerMapSaveData[] mapSaveData = player.controllers.maps.GetMapSaveData(controllerType, controllerId, true);
    if (mapSaveData == null)
      return;
    for (int index = 0; index < mapSaveData.Length; ++index)
      this.SaveControllerMap(player, mapSaveData[index].map);
  }

  public void SaveControllerMap(Player player, ControllerMap controllerMap)
  {
    this.dataStore.SetValue(this.GetControllerMapKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, 0), (object) controllerMap.ToJsonString());
    this.dataStore.SetValue(this.GetControllerMapKnownActionIdsKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, 0), (object) this.allActionIdsString);
  }

  public void SaveInputBehaviors(Player player, PlayerSaveData playerSaveData)
  {
    if (player == null)
      return;
    foreach (InputBehavior inputBehavior in playerSaveData.inputBehaviors)
      this.SaveInputBehaviorNow(player, inputBehavior);
  }

  public void SaveInputBehaviorNow(int playerId, int behaviorId)
  {
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null)
      return;
    InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
    if (inputBehavior == null)
      return;
    this.SaveInputBehaviorNow(player, inputBehavior);
    this.dataStore.Save();
  }

  public void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
  {
    if (player == null || inputBehavior == null)
      return;
    this.dataStore.SetValue(this.GetInputBehaviorKey(player, inputBehavior.id), (object) inputBehavior.ToJsonString());
  }

  public bool SaveControllerAssignments()
  {
    try
    {
      UserDataStore_KeyValue.ControllerAssignmentSaveInfo assignmentSaveInfo = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount);
      for (int index1 = 0; index1 < ReInput.players.allPlayerCount; ++index1)
      {
        Player allPlayer = ((IList<Player>) ReInput.players.AllPlayers)[index1];
        UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo();
        assignmentSaveInfo.players[index1] = playerInfo;
        playerInfo.id = allPlayer.id;
        playerInfo.hasKeyboard = allPlayer.controllers.hasKeyboard;
        playerInfo.hasMouse = allPlayer.controllers.hasMouse;
        UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo[] joystickInfoArray = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo[allPlayer.controllers.joystickCount];
        playerInfo.joysticks = joystickInfoArray;
        for (int index2 = 0; index2 < allPlayer.controllers.joystickCount; ++index2)
        {
          Joystick joystick = ((IList<Joystick>) allPlayer.controllers.Joysticks)[index2];
          joystickInfoArray[index2] = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo()
          {
            instanceGuid = (Guid) ((Controller) joystick).deviceInstanceGuid,
            id = joystick.id,
            hardwareIdentifier = joystick.hardwareIdentifier
          };
        }
      }
      this.dataStore.SetValue("ControllerAssignments", (object) JsonWriter.ToJson((object) assignmentSaveInfo));
      this.dataStore.Save();
    }
    catch
    {
    }
    return true;
  }

  public static void AppendBaseKey(StringBuilder sb, Player player)
  {
    sb.Append("playerId=");
    sb.Append(player.id);
  }

  public string GetControllerMapKey(
    Player player,
    ControllerIdentifier controllerIdentifier,
    int categoryId,
    int layoutId,
    int ppKeyVersion)
  {
    this._sb.Length = 0;
    UserDataStore_KeyValue.AppendBaseKey(this._sb, player);
    this._sb.Append("|dataType=ControllerMap");
    UserDataStore_KeyValue.AppendControllerMapKeyCommonSuffix(this._sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
    return this._sb.ToString();
  }

  public string GetControllerMapKnownActionIdsKey(
    Player player,
    ControllerIdentifier controllerIdentifier,
    int categoryId,
    int layoutId,
    int ppKeyVersion)
  {
    this._sb.Length = 0;
    UserDataStore_KeyValue.AppendBaseKey(this._sb, player);
    this._sb.Append("|dataType=ControllerMap_KnownActionIds");
    UserDataStore_KeyValue.AppendControllerMapKeyCommonSuffix(this._sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
    return this._sb.ToString();
  }

  public static void AppendControllerMapKeyCommonSuffix(
    StringBuilder sb,
    Player player,
    ControllerIdentifier controllerIdentifier,
    int categoryId,
    int layoutId,
    int keyVersion)
  {
    sb.Append("|kv=");
    sb.Append(keyVersion);
    sb.Append("|controllerMapType=");
    sb.Append((int) controllerIdentifier.controllerType);
    sb.Append("|categoryId=");
    sb.Append(categoryId);
    sb.Append("|");
    sb.Append("layoutId=");
    sb.Append(layoutId);
    sb.Append("|hardwareGuid=");
    sb.Append((object) (Guid) controllerIdentifier.hardwareTypeGuid);
    if ((Guid) controllerIdentifier.hardwareTypeGuid == Guid.Empty)
    {
      sb.Append("|hardwareIdentifier=");
      sb.Append(controllerIdentifier.hardwareIdentifier);
    }
    if (controllerIdentifier.controllerType != ControllerType.Joystick)
      return;
    sb.Append("|duplicate=");
    sb.Append(UserDataStore_KeyValue.GetDuplicateIndex(player, controllerIdentifier).ToString());
  }

  public string GetJoystickCalibrationMapKey(Joystick joystick)
  {
    this._sb.Length = 0;
    this._sb.Append("dataType=CalibrationMap");
    this._sb.Append("|controllerType=");
    this._sb.Append((int) joystick.type);
    this._sb.Append("|hardwareIdentifier=");
    this._sb.Append(joystick.hardwareIdentifier);
    this._sb.Append("|hardwareGuid=");
    this._sb.Append(((Guid) joystick.hardwareTypeGuid).ToString());
    return this._sb.ToString();
  }

  public string GetInputBehaviorKey(Player player, int inputBehaviorId)
  {
    this._sb.Length = 0;
    UserDataStore_KeyValue.AppendBaseKey(this._sb, player);
    this._sb.Append("|dataType=InputBehavior");
    this._sb.Append("|id=");
    this._sb.Append(inputBehaviorId);
    return this._sb.ToString();
  }

  public string GetControllerMapJson(
    Player player,
    ControllerIdentifier controllerIdentifier,
    int categoryId,
    int layoutId)
  {
    for (int ppKeyVersion = 0; ppKeyVersion >= 0; --ppKeyVersion)
    {
      string result;
      if (UserDataStore_KeyValue.TryGetString(this.dataStore, this.GetControllerMapKey(player, controllerIdentifier, categoryId, layoutId, ppKeyVersion), out result) && !string.IsNullOrEmpty(result))
        return result;
    }
    return (string) null;
  }

  public List<int> GetControllerMapKnownActionIds(
    Player player,
    ControllerIdentifier controllerIdentifier,
    int categoryId,
    int layoutId)
  {
    List<int> mapKnownActionIds = new List<int>();
    string result1 = (string) null;
    bool flag = false;
    for (int ppKeyVersion = 0; ppKeyVersion >= 0; --ppKeyVersion)
    {
      if (UserDataStore_KeyValue.TryGetString(this.dataStore, this.GetControllerMapKnownActionIdsKey(player, controllerIdentifier, categoryId, layoutId, ppKeyVersion), out result1))
      {
        flag = true;
        break;
      }
    }
    if (!flag || string.IsNullOrEmpty(result1))
      return mapKnownActionIds;
    string[] strArray = result1.Split(',', StringSplitOptions.None);
    for (int index = 0; index < strArray.Length; ++index)
    {
      int result2;
      if (!string.IsNullOrEmpty(strArray[index]) && int.TryParse(strArray[index], out result2))
        mapKnownActionIds.Add(result2);
    }
    return mapKnownActionIds;
  }

  public string GetJoystickCalibrationMapJson(Joystick joystick)
  {
    string result;
    UserDataStore_KeyValue.TryGetString(this.dataStore, this.GetJoystickCalibrationMapKey(joystick), out result);
    return result;
  }

  public string GetInputBehaviorJson(Player player, int id)
  {
    string result;
    UserDataStore_KeyValue.TryGetString(this.dataStore, this.GetInputBehaviorKey(player, id), out result);
    return result;
  }

  public void AddDefaultMappingsForNewActions(
    ControllerIdentifier controllerIdentifier,
    ControllerMap controllerMap,
    List<int> knownActionIds)
  {
    if (controllerMap == null || knownActionIds == null || knownActionIds == null || knownActionIds.Count == 0)
      return;
    ControllerMap controllerMapInstance = ReInput.mapping.GetControllerMapInstance(controllerIdentifier, controllerMap.categoryId, controllerMap.layoutId);
    if (controllerMapInstance == null)
      return;
    List<int> intList = new List<int>();
    foreach (int allActionId in this.allActionIds)
    {
      if (!knownActionIds.Contains(allActionId))
        intList.Add(allActionId);
    }
    if (intList.Count == 0)
      return;
    foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) controllerMapInstance.AllMaps)
    {
      if (intList.Contains(allMap.actionId) && !controllerMap.DoesElementAssignmentConflict(allMap))
      {
        ElementAssignment elementAssignment = new ElementAssignment(controllerMap.controllerType, allMap.elementType, allMap.elementIdentifierId, allMap.axisRange, allMap.keyCode, allMap.modifierKeyFlags, allMap.actionId, allMap.axisContribution, allMap.invert);
        controllerMap.CreateElementMap(elementAssignment);
      }
    }
  }

  public Joystick FindJoystickPrecise(
    UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo)
  {
    if (joystickInfo == null)
      return (Joystick) null;
    if (joystickInfo.instanceGuid == Guid.Empty)
      return (Joystick) null;
    IList<Joystick> joysticks = (IList<Joystick>) ReInput.controllers.Joysticks;
    for (int index = 0; index < joysticks.Count; ++index)
    {
      if ((Guid) ((Controller) joysticks[index]).deviceInstanceGuid == joystickInfo.instanceGuid)
        return joysticks[index];
    }
    return (Joystick) null;
  }

  public bool TryFindJoysticksImprecise(
    UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo,
    out List<Joystick> matches)
  {
    matches = (List<Joystick>) null;
    if (joystickInfo == null || string.IsNullOrEmpty(joystickInfo.hardwareIdentifier))
      return false;
    IList<Joystick> joysticks = (IList<Joystick>) ReInput.controllers.Joysticks;
    for (int index = 0; index < joysticks.Count; ++index)
    {
      if (string.Equals(joysticks[index].hardwareIdentifier, joystickInfo.hardwareIdentifier, StringComparison.OrdinalIgnoreCase))
      {
        if (matches == null)
          matches = new List<Joystick>();
        matches.Add(joysticks[index]);
      }
    }
    return matches != null;
  }

  public void RefreshLayoutManager(int playerId)
  {
    ReInput.players.GetPlayer(playerId)?.controllers.maps.layoutManager.Apply();
  }

  public static int GetDuplicateIndex(Player player, ControllerIdentifier controllerIdentifier)
  {
    Controller controller1 = ReInput.controllers.GetController(controllerIdentifier);
    if (controller1 == null)
      return 0;
    int duplicateIndex = 0;
    foreach (Controller controller2 in (IEnumerable<Controller>) player.controllers.Controllers)
    {
      if (controller2.type == controller1.type)
      {
        bool flag = false;
        if (controller1.type == ControllerType.Joystick)
        {
          if (!((Guid) (controller2 as Joystick).hardwareTypeGuid != (Guid) controller1.hardwareTypeGuid))
          {
            if ((Guid) controller1.hardwareTypeGuid != Guid.Empty)
              flag = true;
          }
          else
            continue;
        }
        if (flag || !(controller2.hardwareIdentifier != controller1.hardwareIdentifier))
        {
          if (controller2 == controller1)
            return duplicateIndex;
          ++duplicateIndex;
        }
      }
    }
    return duplicateIndex;
  }

  public static bool TryGetString(
    UserDataStore_KeyValue.IDataStore store,
    string key,
    out string result)
  {
    if (store == null || string.IsNullOrEmpty(key))
    {
      result = (string) null;
      return false;
    }
    object result1;
    if (!store.TryGetValue(key, out result1))
    {
      result = (string) null;
      return false;
    }
    result = result1 as string;
    return result1 is string;
  }

  public class ControllerAssignmentSaveInfo
  {
    public UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo[] players;

    public int playerCount => this.players == null ? 0 : this.players.Length;

    public ControllerAssignmentSaveInfo()
    {
    }

    public ControllerAssignmentSaveInfo(int playerCount)
    {
      this.players = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo[playerCount];
      for (int index = 0; index < playerCount; ++index)
        this.players[index] = new UserDataStore_KeyValue.ControllerAssignmentSaveInfo.PlayerInfo();
    }

    public int IndexOfPlayer(int playerId)
    {
      for (int index = 0; index < this.playerCount; ++index)
      {
        if (this.players[index] != null && this.players[index].id == playerId)
          return index;
      }
      return -1;
    }

    public bool ContainsPlayer(int playerId) => this.IndexOfPlayer(playerId) >= 0;

    public class PlayerInfo
    {
      public int id;
      public bool hasKeyboard;
      public bool hasMouse;
      public UserDataStore_KeyValue.ControllerAssignmentSaveInfo.JoystickInfo[] joysticks;

      public int joystickCount => this.joysticks == null ? 0 : this.joysticks.Length;

      public int IndexOfJoystick(int joystickId)
      {
        for (int index = 0; index < this.joystickCount; ++index)
        {
          if (this.joysticks[index] != null && this.joysticks[index].id == joystickId)
            return index;
        }
        return -1;
      }

      public bool ContainsJoystick(int joystickId) => this.IndexOfJoystick(joystickId) >= 0;
    }

    public class JoystickInfo
    {
      public Guid instanceGuid;
      public string hardwareIdentifier;
      public int id;
    }
  }

  public class JoystickAssignmentHistoryInfo
  {
    public Joystick joystick;
    public int oldJoystickId;

    public JoystickAssignmentHistoryInfo(Joystick joystick, int oldJoystickId)
    {
      this.joystick = joystick != null ? joystick : throw new ArgumentNullException(nameof (joystick));
      this.oldJoystickId = oldJoystickId;
    }
  }

  public interface IDataStore
  {
    bool Save();

    bool Load();

    bool Clear();

    bool TryGetValue(string key, out object result);

    bool SetValue(string key, object value);
  }
}
