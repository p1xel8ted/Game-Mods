// Decompiled with JetBrains decompiler
// Type: Rewired.Data.UserDataStore_PlayerPrefs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Rewired.Utils.Libraries.TinyJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Data;

public class UserDataStore_PlayerPrefs : UserDataStore
{
  public const string thisScriptName = "UserDataStore_PlayerPrefs";
  public const string editorLoadedMessage = "\nIf unexpected input issues occur, the loaded XML data may be outdated or invalid. Clear PlayerPrefs using the inspector option on the UserDataStore_PlayerPrefs component.";
  public const string playerPrefsKeySuffix_controllerAssignments = "ControllerAssignments";
  [Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
  [SerializeField]
  public bool isEnabled = true;
  [Tooltip("Should saved data be loaded on start?")]
  [SerializeField]
  public bool loadDataOnStart = true;
  [Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")]
  [SerializeField]
  public bool loadJoystickAssignments = true;
  [Tooltip("Should Player Keyboard assignments be saved and loaded?")]
  [SerializeField]
  public bool loadKeyboardAssignments = true;
  [Tooltip("Should Player Mouse assignments be saved and loaded?")]
  [SerializeField]
  public bool loadMouseAssignments = true;
  [Tooltip("The PlayerPrefs key prefix. Change this to change how keys are stored in PlayerPrefs. Changing this will make saved data already stored with the old key no longer accessible.")]
  [SerializeField]
  public string playerPrefsKeyPrefix = "RewiredSaveData";
  public bool allowImpreciseJoystickAssignmentMatching = true;
  public bool deferredJoystickAssignmentLoadPending;
  public bool wasJoystickEverDetected;

  public bool IsEnabled
  {
    get => this.isEnabled;
    set => this.isEnabled = value;
  }

  public bool LoadDataOnStart
  {
    get => this.loadDataOnStart;
    set => this.loadDataOnStart = value;
  }

  public bool LoadJoystickAssignments
  {
    get => this.loadJoystickAssignments;
    set => this.loadJoystickAssignments = value;
  }

  public bool LoadKeyboardAssignments
  {
    get => this.loadKeyboardAssignments;
    set => this.loadKeyboardAssignments = value;
  }

  public bool LoadMouseAssignments
  {
    get => this.loadMouseAssignments;
    set => this.loadMouseAssignments = value;
  }

  public string PlayerPrefsKeyPrefix
  {
    get => this.playerPrefsKeyPrefix;
    set => this.playerPrefsKeyPrefix = value;
  }

  public string playerPrefsKey_controllerAssignments
  {
    get => $"{this.playerPrefsKeyPrefix}_{"ControllerAssignments"}";
  }

  public bool loadControllerAssignments
  {
    get
    {
      return this.loadKeyboardAssignments || this.loadMouseAssignments || this.loadJoystickAssignments;
    }
  }

  public override void Save()
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SaveAll();
  }

  public override void SaveControllerData(
    int playerId,
    ControllerType controllerType,
    int controllerId)
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SaveControllerDataNow(playerId, controllerType, controllerId);
  }

  public override void SaveControllerData(ControllerType controllerType, int controllerId)
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SaveControllerDataNow(controllerType, controllerId);
  }

  public override void SavePlayerData(int playerId)
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SavePlayerDataNow(playerId);
  }

  public override void SaveInputBehavior(int playerId, int behaviorId)
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", (UnityEngine.Object) this);
    else
      this.SaveInputBehaviorNow(playerId, behaviorId);
  }

  public override void Load()
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadAll();
  }

  public override void LoadControllerData(
    int playerId,
    ControllerType controllerType,
    int controllerId)
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadControllerDataNow(playerId, controllerType, controllerId);
  }

  public override void LoadControllerData(ControllerType controllerType, int controllerId)
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadControllerDataNow(controllerType, controllerId);
  }

  public override void LoadPlayerData(int playerId)
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadPlayerDataNow(playerId);
  }

  public override void LoadInputBehavior(int playerId, int behaviorId)
  {
    if (!this.isEnabled)
      Debug.LogWarning((object) "Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", (UnityEngine.Object) this);
    else
      this.LoadInputBehaviorNow(playerId, behaviorId);
  }

  public override void OnInitialize()
  {
    if (!this.loadDataOnStart)
      return;
    this.Load();
    if (!this.loadControllerAssignments || ReInput.controllers.joystickCount <= 0)
      return;
    this.SaveControllerAssignments();
  }

  public override void OnControllerConnected(ControllerStatusChangedEventArgs args)
  {
    if (!this.isEnabled || args.controllerType != ControllerType.Joystick)
      return;
    this.LoadJoystickData(args.controllerId);
    if (this.loadDataOnStart && this.loadJoystickAssignments && !this.wasJoystickEverDetected)
      this.StartCoroutine(this.LoadJoystickAssignmentsDeferred());
    if (this.loadJoystickAssignments && !this.deferredJoystickAssignmentLoadPending)
      this.SaveControllerAssignments();
    this.wasJoystickEverDetected = true;
  }

  public override void OnControllerPreDiscconnect(ControllerStatusChangedEventArgs args)
  {
    if (!this.isEnabled || args.controllerType != ControllerType.Joystick)
      return;
    this.SaveJoystickData(args.controllerId);
  }

  public override void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
  {
    if (!this.isEnabled || !this.loadControllerAssignments)
      return;
    this.SaveControllerAssignments();
  }

  public int LoadAll()
  {
    int num = 0;
    if (this.loadControllerAssignments && this.LoadControllerAssignmentsNow())
      ++num;
    IList<Player> allPlayers = ReInput.players.AllPlayers;
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
    return num;
  }

  public int LoadAllJoystickCalibrationData()
  {
    int num = 0;
    IList<Joystick> joysticks = ReInput.controllers.Joysticks;
    for (int index = 0; index < joysticks.Count; ++index)
      num += this.LoadJoystickCalibrationData(joysticks[index]);
    return num;
  }

  public int LoadJoystickCalibrationData(Joystick joystick)
  {
    return joystick == null || !joystick.ImportCalibrationMapFromXmlString(this.GetJoystickCalibrationMapXml(joystick)) ? 0 : 1;
  }

  public int LoadJoystickCalibrationData(int joystickId)
  {
    return this.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
  }

  public int LoadJoystickData(int joystickId)
  {
    int num = 0;
    IList<Player> allPlayers = ReInput.players.AllPlayers;
    for (int index = 0; index < allPlayers.Count; ++index)
    {
      Player player = allPlayers[index];
      if (player.controllers.ContainsController(ControllerType.Joystick, joystickId))
        num += this.LoadControllerMaps(player.id, ControllerType.Joystick, joystickId);
    }
    return num + this.LoadJoystickCalibrationData(joystickId);
  }

  public int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId)
  {
    return 0 + this.LoadControllerMaps(playerId, controllerType, controllerId) + this.LoadControllerDataNow(controllerType, controllerId);
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
    int num1 = 0;
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null)
      return num1;
    Controller controller = ReInput.controllers.GetController(controllerType, controllerId);
    if (controller == null)
      return num1;
    List<UserDataStore_PlayerPrefs.SavedControllerMapData> controllerMapsXml = this.GetAllControllerMapsXml(player, true, controller);
    if (controllerMapsXml.Count == 0)
      return num1;
    int num2 = num1 + player.controllers.maps.AddMapsFromXml(controllerType, controllerId, UserDataStore_PlayerPrefs.SavedControllerMapData.GetXmlStringList(controllerMapsXml));
    this.AddDefaultMappingsForNewActions(player, controllerMapsXml, controllerType, controllerId);
    return num2;
  }

  public int LoadInputBehaviors(int playerId)
  {
    Player player = ReInput.players.GetPlayer(playerId);
    if (player == null)
      return 0;
    int num = 0;
    IList<InputBehavior> inputBehaviors = ReInput.mapping.GetInputBehaviors(player.id);
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
    string inputBehaviorXml = this.GetInputBehaviorXml(player, inputBehavior.id);
    return inputBehaviorXml == null || inputBehaviorXml == string.Empty || !inputBehavior.ImportXmlString(inputBehaviorXml) ? 0 : 1;
  }

  public bool LoadControllerAssignmentsNow()
  {
    try
    {
      UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data = this.LoadControllerAssignmentData();
      if (data == null)
        return false;
      if (this.loadKeyboardAssignments || this.loadMouseAssignments)
        this.LoadKeyboardAndMouseAssignmentsNow(data);
      if (this.loadJoystickAssignments)
        this.LoadJoystickAssignmentsNow(data);
    }
    catch
    {
    }
    return true;
  }

  public bool LoadKeyboardAndMouseAssignmentsNow(
    UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
  {
    try
    {
      if (data == null && (data = this.LoadControllerAssignmentData()) == null)
        return false;
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
      {
        if (data.ContainsPlayer(allPlayer.id))
        {
          UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(allPlayer.id)];
          if (this.loadKeyboardAssignments)
            allPlayer.controllers.hasKeyboard = player.hasKeyboard;
          if (this.loadMouseAssignments)
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
    UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo data)
  {
    try
    {
      if (ReInput.controllers.joystickCount == 0 || data == null && (data = this.LoadControllerAssignmentData()) == null)
        return false;
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
        allPlayer.controllers.ClearControllersOfType(ControllerType.Joystick);
      List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo> assignmentHistoryInfoList = this.loadJoystickAssignments ? new List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>() : (List<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>) null;
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
      {
        if (data.ContainsPlayer(allPlayer.id))
        {
          UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(allPlayer.id)];
          for (int index = 0; index < player.joystickCount; ++index)
          {
            UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystick1 = player.joysticks[index];
            if (joystick1 != null)
            {
              Joystick joystick = this.FindJoystickPrecise(joystick1);
              if (joystick != null)
              {
                if (assignmentHistoryInfoList.Find((Predicate<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>) (x => x.joystick == joystick)) == null)
                  assignmentHistoryInfoList.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick, joystick1.id));
                allPlayer.controllers.AddController((Controller) joystick, false);
              }
            }
          }
        }
      }
      if (this.allowImpreciseJoystickAssignmentMatching)
      {
        foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
        {
          if (data.ContainsPlayer(allPlayer.id))
          {
            UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo player = data.players[data.IndexOfPlayer(allPlayer.id)];
            for (int index1 = 0; index1 < player.joystickCount; ++index1)
            {
              UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = player.joysticks[index1];
              if (joystickInfo != null)
              {
                Joystick joystick2 = (Joystick) null;
                int index2 = assignmentHistoryInfoList.FindIndex((Predicate<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>) (x => x.oldJoystickId == joystickInfo.id));
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
                      if (assignmentHistoryInfoList.Find((Predicate<UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo>) (x => x.joystick == match)) == null)
                      {
                        joystick2 = match;
                        break;
                      }
                    }
                    if (joystick2 != null)
                      assignmentHistoryInfoList.Add(new UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick2, joystickInfo.id));
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

  public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo LoadControllerAssignmentData()
  {
    try
    {
      if (!PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments))
        return (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null;
      string json = PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments);
      if (string.IsNullOrEmpty(json))
        return (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null;
      UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo assignmentSaveInfo = JsonParser.FromJson<UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo>(json);
      return assignmentSaveInfo == null || assignmentSaveInfo.playerCount == 0 ? (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null : assignmentSaveInfo;
    }
    catch
    {
      return (UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null;
    }
  }

  public IEnumerator LoadJoystickAssignmentsDeferred()
  {
    this.deferredJoystickAssignmentLoadPending = true;
    yield return (object) new WaitForEndOfFrame();
    if (ReInput.isReady)
    {
      this.LoadJoystickAssignmentsNow((UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) null);
      this.SaveControllerAssignments();
      this.deferredJoystickAssignmentLoadPending = false;
    }
  }

  public void SaveAll()
  {
    IList<Player> allPlayers = ReInput.players.AllPlayers;
    for (int index = 0; index < allPlayers.Count; ++index)
      this.SavePlayerDataNow(allPlayers[index]);
    this.SaveAllJoystickCalibrationData();
    if (this.loadControllerAssignments)
      this.SaveControllerAssignments();
    PlayerPrefs.Save();
  }

  public void SavePlayerDataNow(int playerId)
  {
    this.SavePlayerDataNow(ReInput.players.GetPlayer(playerId));
    PlayerPrefs.Save();
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
    IList<Joystick> joysticks = ReInput.controllers.Joysticks;
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
    PlayerPrefs.SetString(this.GetJoystickCalibrationMapPlayerPrefsKey(joystick), calibrationMapSaveData.map.ToXmlString());
  }

  public void SaveJoystickData(int joystickId)
  {
    IList<Player> allPlayers = ReInput.players.AllPlayers;
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
    this.SaveControllerDataNow(controllerType, controllerId);
    PlayerPrefs.Save();
  }

  public void SaveControllerDataNow(ControllerType controllerType, int controllerId)
  {
    if (controllerType == ControllerType.Joystick)
      this.SaveJoystickCalibrationData(controllerId);
    PlayerPrefs.Save();
  }

  public void SaveControllerMaps(Player player, PlayerSaveData playerSaveData)
  {
    foreach (ControllerMapSaveData saveData in playerSaveData.AllControllerMapSaveData)
      this.SaveControllerMap(player, saveData);
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
      this.SaveControllerMap(player, mapSaveData[index]);
  }

  public void SaveControllerMap(Player player, ControllerMapSaveData saveData)
  {
    PlayerPrefs.SetString(this.GetControllerMapPlayerPrefsKey(player, saveData.controller, saveData.categoryId, saveData.layoutId, true), saveData.map.ToXmlString());
    PlayerPrefs.SetString(this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, saveData.controller, saveData.categoryId, saveData.layoutId, true), this.GetAllActionIdsString());
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
    PlayerPrefs.Save();
  }

  public void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior)
  {
    if (player == null || inputBehavior == null)
      return;
    PlayerPrefs.SetString(this.GetInputBehaviorPlayerPrefsKey(player, inputBehavior.id), inputBehavior.ToXmlString());
  }

  public bool SaveControllerAssignments()
  {
    try
    {
      UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo assignmentSaveInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount);
      for (int index1 = 0; index1 < ReInput.players.allPlayerCount; ++index1)
      {
        Player allPlayer = ReInput.players.AllPlayers[index1];
        UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo playerInfo = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
        assignmentSaveInfo.players[index1] = playerInfo;
        playerInfo.id = allPlayer.id;
        playerInfo.hasKeyboard = allPlayer.controllers.hasKeyboard;
        playerInfo.hasMouse = allPlayer.controllers.hasMouse;
        UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] joystickInfoArray = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[allPlayer.controllers.joystickCount];
        playerInfo.joysticks = joystickInfoArray;
        for (int index2 = 0; index2 < allPlayer.controllers.joystickCount; ++index2)
        {
          Joystick joystick = allPlayer.controllers.Joysticks[index2];
          joystickInfoArray[index2] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo()
          {
            instanceGuid = joystick.deviceInstanceGuid,
            id = joystick.id,
            hardwareIdentifier = joystick.hardwareIdentifier
          };
        }
      }
      PlayerPrefs.SetString(this.playerPrefsKey_controllerAssignments, JsonWriter.ToJson((object) assignmentSaveInfo));
      PlayerPrefs.Save();
    }
    catch
    {
    }
    return true;
  }

  public bool ControllerAssignmentSaveDataExists()
  {
    return PlayerPrefs.HasKey(this.playerPrefsKey_controllerAssignments) && !string.IsNullOrEmpty(PlayerPrefs.GetString(this.playerPrefsKey_controllerAssignments));
  }

  public string GetBasePlayerPrefsKey(Player player)
  {
    return $"{this.playerPrefsKeyPrefix}|playerName={player.name}";
  }

  public string GetControllerMapPlayerPrefsKey(
    Player player,
    Controller controller,
    int categoryId,
    int layoutId,
    bool includeDuplicateIndex)
  {
    string mapPlayerPrefsKey = $"{$"{$"{this.GetBasePlayerPrefsKey(player) + "|dataType=ControllerMap"}|controllerMapType={controller.mapTypeString}"}|categoryId={categoryId.ToString()}|layoutId={layoutId.ToString()}"}|hardwareIdentifier={controller.hardwareIdentifier}";
    if (controller.type == ControllerType.Joystick)
    {
      mapPlayerPrefsKey = $"{mapPlayerPrefsKey}|hardwareGuid={((Joystick) controller).hardwareTypeGuid.ToString()}";
      if (includeDuplicateIndex)
        mapPlayerPrefsKey = $"{mapPlayerPrefsKey}|duplicate={UserDataStore_PlayerPrefs.GetDuplicateIndex(player, controller).ToString()}";
    }
    return mapPlayerPrefsKey;
  }

  public string GetControllerMapKnownActionIdsPlayerPrefsKey(
    Player player,
    Controller controller,
    int categoryId,
    int layoutId,
    bool includeDuplicateIndex)
  {
    string idsPlayerPrefsKey = $"{$"{$"{this.GetBasePlayerPrefsKey(player) + "|dataType=ControllerMap_KnownActionIds"}|controllerMapType={controller.mapTypeString}"}|categoryId={categoryId.ToString()}|layoutId={layoutId.ToString()}"}|hardwareIdentifier={controller.hardwareIdentifier}";
    if (controller.type == ControllerType.Joystick)
    {
      idsPlayerPrefsKey = $"{idsPlayerPrefsKey}|hardwareGuid={((Joystick) controller).hardwareTypeGuid.ToString()}";
      if (includeDuplicateIndex)
        idsPlayerPrefsKey = $"{idsPlayerPrefsKey}|duplicate={UserDataStore_PlayerPrefs.GetDuplicateIndex(player, controller).ToString()}";
    }
    return idsPlayerPrefsKey;
  }

  public string GetJoystickCalibrationMapPlayerPrefsKey(Joystick joystick)
  {
    return $"{$"{$"{this.playerPrefsKeyPrefix + "|dataType=CalibrationMap"}|controllerType={joystick.type.ToString()}"}|hardwareIdentifier={joystick.hardwareIdentifier}"}|hardwareGuid={joystick.hardwareTypeGuid.ToString()}";
  }

  public string GetInputBehaviorPlayerPrefsKey(Player player, int inputBehaviorId)
  {
    return $"{this.GetBasePlayerPrefsKey(player) + "|dataType=InputBehavior"}|id={inputBehaviorId.ToString()}";
  }

  public string GetControllerMapXml(
    Player player,
    Controller controller,
    int categoryId,
    int layoutId)
  {
    string mapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controller, categoryId, layoutId, true);
    if (!PlayerPrefs.HasKey(mapPlayerPrefsKey))
    {
      if (controller.type != ControllerType.Joystick)
        return string.Empty;
      mapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controller, categoryId, layoutId, false);
      if (!PlayerPrefs.HasKey(mapPlayerPrefsKey))
        return string.Empty;
    }
    return PlayerPrefs.GetString(mapPlayerPrefsKey);
  }

  public List<int> GetControllerMapKnownActionIds(
    Player player,
    Controller controller,
    int categoryId,
    int layoutId)
  {
    List<int> mapKnownActionIds = new List<int>();
    string idsPlayerPrefsKey = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controller, categoryId, layoutId, true);
    if (!PlayerPrefs.HasKey(idsPlayerPrefsKey))
    {
      if (controller.type != ControllerType.Joystick)
        return mapKnownActionIds;
      idsPlayerPrefsKey = this.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controller, categoryId, layoutId, false);
      if (!PlayerPrefs.HasKey(idsPlayerPrefsKey))
        return mapKnownActionIds;
    }
    string str = PlayerPrefs.GetString(idsPlayerPrefsKey);
    if (string.IsNullOrEmpty(str))
      return mapKnownActionIds;
    string[] strArray = str.Split(',');
    for (int index = 0; index < strArray.Length; ++index)
    {
      int result;
      if (!string.IsNullOrEmpty(strArray[index]) && int.TryParse(strArray[index], out result))
        mapKnownActionIds.Add(result);
    }
    return mapKnownActionIds;
  }

  public List<UserDataStore_PlayerPrefs.SavedControllerMapData> GetAllControllerMapsXml(
    Player player,
    bool userAssignableMapsOnly,
    Controller controller)
  {
    List<UserDataStore_PlayerPrefs.SavedControllerMapData> controllerMapsXml = new List<UserDataStore_PlayerPrefs.SavedControllerMapData>();
    IList<InputMapCategory> mapCategories = ReInput.mapping.MapCategories;
    for (int index1 = 0; index1 < mapCategories.Count; ++index1)
    {
      InputMapCategory inputMapCategory = mapCategories[index1];
      if (!userAssignableMapsOnly || inputMapCategory.userAssignable)
      {
        IList<InputLayout> inputLayoutList = ReInput.mapping.MapLayouts(controller.type);
        for (int index2 = 0; index2 < inputLayoutList.Count; ++index2)
        {
          InputLayout inputLayout = inputLayoutList[index2];
          string controllerMapXml = this.GetControllerMapXml(player, controller, inputMapCategory.id, inputLayout.id);
          if (!(controllerMapXml == string.Empty))
          {
            List<int> mapKnownActionIds = this.GetControllerMapKnownActionIds(player, controller, inputMapCategory.id, inputLayout.id);
            controllerMapsXml.Add(new UserDataStore_PlayerPrefs.SavedControllerMapData(controllerMapXml, mapKnownActionIds));
          }
        }
      }
    }
    return controllerMapsXml;
  }

  public string GetJoystickCalibrationMapXml(Joystick joystick)
  {
    string mapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(joystick);
    return !PlayerPrefs.HasKey(mapPlayerPrefsKey) ? string.Empty : PlayerPrefs.GetString(mapPlayerPrefsKey);
  }

  public string GetInputBehaviorXml(Player player, int id)
  {
    string behaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, id);
    return !PlayerPrefs.HasKey(behaviorPlayerPrefsKey) ? string.Empty : PlayerPrefs.GetString(behaviorPlayerPrefsKey);
  }

  public void AddDefaultMappingsForNewActions(
    Player player,
    List<UserDataStore_PlayerPrefs.SavedControllerMapData> savedData,
    ControllerType controllerType,
    int controllerId)
  {
    if (player == null || savedData == null)
      return;
    List<int> allActionIds = this.GetAllActionIds();
    for (int index = 0; index < savedData.Count; ++index)
    {
      UserDataStore_PlayerPrefs.SavedControllerMapData controllerMapData = savedData[index];
      if (controllerMapData != null && controllerMapData.knownActionIds != null && controllerMapData.knownActionIds.Count != 0)
      {
        ControllerMap fromXml = ControllerMap.CreateFromXml(controllerType, savedData[index].xml);
        if (fromXml != null)
        {
          ControllerMap map = player.controllers.maps.GetMap(controllerType, controllerId, fromXml.categoryId, fromXml.layoutId);
          if (map != null)
          {
            ControllerMap controllerMapInstance = ReInput.mapping.GetControllerMapInstance(ReInput.controllers.GetController(controllerType, controllerId), fromXml.categoryId, fromXml.layoutId);
            if (controllerMapInstance != null)
            {
              List<int> intList = new List<int>();
              foreach (int num in allActionIds)
              {
                if (!controllerMapData.knownActionIds.Contains(num))
                  intList.Add(num);
              }
              if (intList.Count != 0)
              {
                foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) controllerMapInstance.AllMaps)
                {
                  if (intList.Contains(allMap.actionId) && !map.DoesElementAssignmentConflict(allMap))
                  {
                    ElementAssignment elementAssignment = new ElementAssignment(controllerType, allMap.elementType, allMap.elementIdentifierId, allMap.axisRange, allMap.keyCode, allMap.modifierKeyFlags, allMap.actionId, allMap.axisContribution, allMap.invert);
                    map.CreateElementMap(elementAssignment);
                  }
                }
              }
            }
          }
        }
      }
    }
  }

  public List<int> GetAllActionIds()
  {
    List<int> allActionIds = new List<int>();
    IList<InputAction> actions = ReInput.mapping.Actions;
    for (int index = 0; index < actions.Count; ++index)
      allActionIds.Add(actions[index].id);
    return allActionIds;
  }

  public string GetAllActionIdsString()
  {
    string empty = string.Empty;
    List<int> allActionIds = this.GetAllActionIds();
    for (int index = 0; index < allActionIds.Count; ++index)
    {
      if (index > 0)
        empty += ",";
      empty += allActionIds[index].ToString();
    }
    return empty;
  }

  public Joystick FindJoystickPrecise(
    UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo)
  {
    if (joystickInfo == null)
      return (Joystick) null;
    if (joystickInfo.instanceGuid == Guid.Empty)
      return (Joystick) null;
    IList<Joystick> joysticks = ReInput.controllers.Joysticks;
    for (int index = 0; index < joysticks.Count; ++index)
    {
      if (joysticks[index].deviceInstanceGuid == joystickInfo.instanceGuid)
        return joysticks[index];
    }
    return (Joystick) null;
  }

  public bool TryFindJoysticksImprecise(
    UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo joystickInfo,
    out List<Joystick> matches)
  {
    matches = (List<Joystick>) null;
    if (joystickInfo == null || string.IsNullOrEmpty(joystickInfo.hardwareIdentifier))
      return false;
    IList<Joystick> joysticks = ReInput.controllers.Joysticks;
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

  public static int GetDuplicateIndex(Player player, Controller controller)
  {
    int duplicateIndex = 0;
    foreach (Controller controller1 in player.controllers.Controllers)
    {
      if (controller1.type == controller.type)
      {
        bool flag = false;
        if (controller.type == ControllerType.Joystick)
        {
          if (!((controller1 as Joystick).hardwareTypeGuid != (controller as Joystick).hardwareTypeGuid))
          {
            if ((controller as Joystick).hardwareTypeGuid != Guid.Empty)
              flag = true;
          }
          else
            continue;
        }
        if (flag || !(controller1.hardwareIdentifier != controller.hardwareIdentifier))
        {
          if (controller1 == controller)
            return duplicateIndex;
          ++duplicateIndex;
        }
      }
    }
    return duplicateIndex;
  }

  public class SavedControllerMapData
  {
    public string xml;
    public List<int> knownActionIds;

    public SavedControllerMapData(string xml, List<int> knownActionIds)
    {
      this.xml = xml;
      this.knownActionIds = knownActionIds;
    }

    public static List<string> GetXmlStringList(
      List<UserDataStore_PlayerPrefs.SavedControllerMapData> data)
    {
      List<string> xmlStringList = new List<string>();
      if (data == null)
        return xmlStringList;
      for (int index = 0; index < data.Count; ++index)
      {
        if (data[index] != null && !string.IsNullOrEmpty(data[index].xml))
          xmlStringList.Add(data[index].xml);
      }
      return xmlStringList;
    }
  }

  public class ControllerAssignmentSaveInfo
  {
    public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[] players;

    public int playerCount => this.players == null ? 0 : this.players.Length;

    public ControllerAssignmentSaveInfo()
    {
    }

    public ControllerAssignmentSaveInfo(int playerCount)
    {
      this.players = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo[playerCount];
      for (int index = 0; index < playerCount; ++index)
        this.players[index] = new UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo();
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
      public UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo[] joysticks;

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
}
