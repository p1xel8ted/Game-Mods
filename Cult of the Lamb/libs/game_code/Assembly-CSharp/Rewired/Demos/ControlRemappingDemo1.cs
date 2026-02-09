// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.ControlRemappingDemo1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class ControlRemappingDemo1 : MonoBehaviour
{
  public const float defaultModalWidth = 250f;
  public const float defaultModalHeight = 200f;
  public const float assignmentTimeout = 5f;
  public ControlRemappingDemo1.DialogHelper dialog;
  public InputMapper inputMapper = new InputMapper();
  public InputMapper.ConflictFoundEventData conflictFoundEventData;
  public bool guiState;
  public bool busy;
  public bool pageGUIState;
  public Player selectedPlayer;
  public int selectedMapCategoryId;
  public ControlRemappingDemo1.ControllerSelection selectedController;
  public ControllerMap selectedMap;
  public bool showMenu;
  public bool startListening;
  public Vector2 actionScrollPos;
  public Vector2 calibrateScrollPos;
  public Queue<ControlRemappingDemo1.QueueEntry> actionQueue;
  public bool setupFinished;
  [NonSerialized]
  public bool initialized;
  public bool isCompiling;
  public GUIStyle style_wordWrap;
  public GUIStyle style_centeredBox;

  public void Awake()
  {
    this.inputMapper.options.timeout = 5f;
    this.inputMapper.options.ignoreMouseXAxis = true;
    this.inputMapper.options.ignoreMouseYAxis = true;
    this.Initialize();
  }

  public void OnEnable() => this.Subscribe();

  public void OnDisable() => this.Unsubscribe();

  public void Initialize()
  {
    this.dialog = new ControlRemappingDemo1.DialogHelper();
    this.actionQueue = new Queue<ControlRemappingDemo1.QueueEntry>();
    this.selectedController = new ControlRemappingDemo1.ControllerSelection();
    ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.JoystickConnected);
    ReInput.ControllerPreDisconnectEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.JoystickPreDisconnect);
    ReInput.ControllerDisconnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.JoystickDisconnected);
    this.ResetAll();
    this.initialized = true;
    ReInput.userDataStore.Load();
    if (!ReInput.unityJoystickIdentificationRequired)
      return;
    this.IdentifyAllJoysticks();
  }

  public void Setup()
  {
    if (this.setupFinished)
      return;
    this.style_wordWrap = new GUIStyle(GUI.skin.label);
    this.style_wordWrap.wordWrap = true;
    this.style_centeredBox = new GUIStyle(GUI.skin.box);
    this.style_centeredBox.alignment = TextAnchor.MiddleCenter;
    this.setupFinished = true;
  }

  public void Subscribe()
  {
    this.Unsubscribe();
    this.inputMapper.ConflictFoundEvent += (Action<InputMapper.ConflictFoundEventData>) new Action<InputMapper.ConflictFoundEventData>(this.OnConflictFound);
    this.inputMapper.StoppedEvent += (Action<InputMapper.StoppedEventData>) new Action<InputMapper.StoppedEventData>(this.OnStopped);
  }

  public void Unsubscribe() => this.inputMapper.RemoveAllEventListeners();

  public void OnGUI()
  {
    if (!this.initialized)
      return;
    this.Setup();
    this.HandleMenuControl();
    if (!this.showMenu)
    {
      this.DrawInitialScreen();
    }
    else
    {
      this.SetGUIStateStart();
      this.ProcessQueue();
      this.DrawPage();
      this.ShowDialog();
      this.SetGUIStateEnd();
      this.busy = false;
    }
  }

  public void HandleMenuControl()
  {
    if (this.dialog.enabled || Event.current.type != EventType.Layout || !ReInput.players.GetSystemPlayer().GetButtonDown("Menu"))
      return;
    if (this.showMenu)
    {
      ReInput.userDataStore.Save();
      this.Close();
    }
    else
      this.Open();
  }

  public void Close()
  {
    this.ClearWorkingVars();
    this.showMenu = false;
  }

  public void Open() => this.showMenu = true;

  public void DrawInitialScreen()
  {
    ActionElementMap elementMapWithAction = ReInput.players.GetSystemPlayer().controllers.maps.GetFirstElementMapWithAction("Menu", true);
    GUIContent content = elementMapWithAction == null ? new GUIContent("There is no element assigned to open the menu!") : new GUIContent($"Press {elementMapWithAction.elementIdentifierName} to open the menu.");
    GUILayout.BeginArea(this.GetScreenCenteredRect(300f, 50f));
    GUILayout.Box(content, this.style_centeredBox, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
    GUILayout.EndArea();
  }

  public void DrawPage()
  {
    if (GUI.enabled != this.pageGUIState)
      GUI.enabled = this.pageGUIState;
    GUILayout.BeginArea(new Rect((float) (((double) Screen.width - (double) Screen.width * 0.89999997615814209) * 0.5), (float) (((double) Screen.height - (double) Screen.height * 0.89999997615814209) * 0.5), (float) Screen.width * 0.9f, (float) Screen.height * 0.9f));
    this.DrawPlayerSelector();
    this.DrawJoystickSelector();
    this.DrawMouseAssignment();
    this.DrawControllerSelector();
    this.DrawCalibrateButton();
    this.DrawMapCategories();
    this.actionScrollPos = GUILayout.BeginScrollView(this.actionScrollPos);
    this.DrawCategoryActions();
    GUILayout.EndScrollView();
    GUILayout.EndArea();
  }

  public void DrawPlayerSelector()
  {
    if (ReInput.players.allPlayerCount == 0)
    {
      GUILayout.Label("There are no players.");
    }
    else
    {
      GUILayout.Space(15f);
      GUILayout.Label("Players:");
      GUILayout.BeginHorizontal();
      foreach (Player player in (IEnumerable<Player>) ReInput.players.GetPlayers(true))
      {
        if (this.selectedPlayer == null)
          this.selectedPlayer = player;
        bool flag1 = player == this.selectedPlayer;
        bool flag2 = GUILayout.Toggle((flag1 ? 1 : 0) != 0, player.descriptiveName != string.Empty ? player.descriptiveName : player.name, (GUIStyle) "Button", GUILayout.ExpandWidth(false));
        if (flag2 != flag1 && flag2)
        {
          this.selectedPlayer = player;
          this.selectedController.Clear();
          this.selectedMapCategoryId = -1;
        }
      }
      GUILayout.EndHorizontal();
    }
  }

  public void DrawMouseAssignment()
  {
    bool enabled = GUI.enabled;
    if (this.selectedPlayer == null)
      GUI.enabled = false;
    GUILayout.Space(15f);
    GUILayout.Label("Assign Mouse:");
    GUILayout.BeginHorizontal();
    bool flag1 = this.selectedPlayer != null && this.selectedPlayer.controllers.hasMouse;
    bool flag2 = GUILayout.Toggle((flag1 ? 1 : 0) != 0, "Assign Mouse", (GUIStyle) "Button", GUILayout.ExpandWidth(false));
    if (flag2 != flag1)
    {
      if (flag2)
      {
        this.selectedPlayer.controllers.hasMouse = true;
        foreach (Player player in (IEnumerable<Player>) ReInput.players.Players)
        {
          if (player != this.selectedPlayer)
            player.controllers.hasMouse = false;
        }
      }
      else
        this.selectedPlayer.controllers.hasMouse = false;
    }
    GUILayout.EndHorizontal();
    if (GUI.enabled == enabled)
      return;
    GUI.enabled = enabled;
  }

  public void DrawJoystickSelector()
  {
    bool enabled = GUI.enabled;
    if (this.selectedPlayer == null)
      GUI.enabled = false;
    GUILayout.Space(15f);
    GUILayout.Label("Assign Joysticks:");
    GUILayout.BeginHorizontal();
    bool flag1 = this.selectedPlayer == null || this.selectedPlayer.controllers.joystickCount == 0;
    if (GUILayout.Toggle((flag1 ? 1 : 0) != 0, "None", (GUIStyle) "Button", GUILayout.ExpandWidth(false)) != flag1)
    {
      this.selectedPlayer.controllers.ClearControllersOfType(ControllerType.Joystick);
      this.ControllerSelectionChanged();
    }
    if (this.selectedPlayer != null)
    {
      foreach (Joystick joystick in (IEnumerable<Joystick>) ReInput.controllers.Joysticks)
      {
        bool flag2 = this.selectedPlayer.controllers.ContainsController((Controller) joystick);
        bool assign = GUILayout.Toggle((flag2 ? 1 : 0) != 0, joystick.name, (GUIStyle) "Button", GUILayout.ExpandWidth(false));
        if (assign != flag2)
          this.EnqueueAction((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.JoystickAssignmentChange(this.selectedPlayer.id, joystick.id, assign));
      }
    }
    GUILayout.EndHorizontal();
    if (GUI.enabled == enabled)
      return;
    GUI.enabled = enabled;
  }

  public void DrawControllerSelector()
  {
    if (this.selectedPlayer == null)
      return;
    bool enabled = GUI.enabled;
    GUILayout.Space(15f);
    GUILayout.Label("Controller to Map:");
    GUILayout.BeginHorizontal();
    if (!this.selectedController.hasSelection)
    {
      this.selectedController.Set(0, ControllerType.Keyboard);
      this.ControllerSelectionChanged();
    }
    bool flag1 = this.selectedController.type == ControllerType.Keyboard;
    if (GUILayout.Toggle((flag1 ? 1 : 0) != 0, "Keyboard", (GUIStyle) "Button", GUILayout.ExpandWidth(false)) != flag1)
    {
      this.selectedController.Set(0, ControllerType.Keyboard);
      this.ControllerSelectionChanged();
    }
    if (!this.selectedPlayer.controllers.hasMouse)
      GUI.enabled = false;
    bool flag2 = this.selectedController.type == ControllerType.Mouse;
    if (GUILayout.Toggle((flag2 ? 1 : 0) != 0, "Mouse", (GUIStyle) "Button", GUILayout.ExpandWidth(false)) != flag2)
    {
      this.selectedController.Set(0, ControllerType.Mouse);
      this.ControllerSelectionChanged();
    }
    if (GUI.enabled != enabled)
      GUI.enabled = enabled;
    foreach (Joystick joystick in (IEnumerable<Joystick>) this.selectedPlayer.controllers.Joysticks)
    {
      bool flag3 = this.selectedController.type == ControllerType.Joystick && this.selectedController.id == joystick.id;
      if (GUILayout.Toggle((flag3 ? 1 : 0) != 0, joystick.name, (GUIStyle) "Button", GUILayout.ExpandWidth(false)) != flag3)
      {
        this.selectedController.Set(joystick.id, ControllerType.Joystick);
        this.ControllerSelectionChanged();
      }
    }
    GUILayout.EndHorizontal();
    if (GUI.enabled == enabled)
      return;
    GUI.enabled = enabled;
  }

  public void DrawCalibrateButton()
  {
    if (this.selectedPlayer == null)
      return;
    bool enabled = GUI.enabled;
    GUILayout.Space(10f);
    Controller controller = this.selectedController.hasSelection ? this.selectedPlayer.controllers.GetController(this.selectedController.type, this.selectedController.id) : (Controller) null;
    if (controller == null || this.selectedController.type != ControllerType.Joystick)
    {
      GUI.enabled = false;
      GUILayout.Button("Select a controller to calibrate", GUILayout.ExpandWidth(false));
      if (GUI.enabled != enabled)
        GUI.enabled = enabled;
    }
    else if (GUILayout.Button("Calibrate " + controller.name, GUILayout.ExpandWidth(false)) && controller is Joystick joystick)
    {
      CalibrationMap calibrationMap = joystick.calibrationMap;
      if (calibrationMap != null)
        this.EnqueueAction((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.Calibration(this.selectedPlayer, joystick, calibrationMap));
    }
    if (GUI.enabled == enabled)
      return;
    GUI.enabled = enabled;
  }

  public void DrawMapCategories()
  {
    if (this.selectedPlayer == null || !this.selectedController.hasSelection)
      return;
    bool enabled = GUI.enabled;
    GUILayout.Space(15f);
    GUILayout.Label("Categories:");
    GUILayout.BeginHorizontal();
    foreach (InputMapCategory assignableMapCategory in (IEnumerable<InputMapCategory>) ReInput.mapping.UserAssignableMapCategories)
    {
      if (!this.selectedPlayer.controllers.maps.ContainsMapInCategory(this.selectedController.type, assignableMapCategory.id))
        GUI.enabled = false;
      else if (this.selectedMapCategoryId < 0)
      {
        this.selectedMapCategoryId = assignableMapCategory.id;
        this.selectedMap = this.selectedPlayer.controllers.maps.GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, assignableMapCategory.id);
      }
      bool flag = assignableMapCategory.id == this.selectedMapCategoryId;
      if (GUILayout.Toggle((flag ? 1 : 0) != 0, assignableMapCategory.descriptiveName != string.Empty ? assignableMapCategory.descriptiveName : assignableMapCategory.name, (GUIStyle) "Button", GUILayout.ExpandWidth(false)) != flag)
      {
        this.selectedMapCategoryId = assignableMapCategory.id;
        this.selectedMap = this.selectedPlayer.controllers.maps.GetFirstMapInCategory(this.selectedController.type, this.selectedController.id, assignableMapCategory.id);
      }
      if (GUI.enabled != enabled)
        GUI.enabled = enabled;
    }
    GUILayout.EndHorizontal();
    if (GUI.enabled == enabled)
      return;
    GUI.enabled = enabled;
  }

  public void DrawCategoryActions()
  {
    if (this.selectedPlayer == null || this.selectedMapCategoryId < 0)
      return;
    bool enabled = GUI.enabled;
    if (this.selectedMap == null)
      return;
    GUILayout.Space(15f);
    GUILayout.Label("Actions:");
    InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(this.selectedMapCategoryId);
    if (mapCategory == null)
      return;
    InputCategory actionCategory = ReInput.mapping.GetActionCategory(mapCategory.name);
    if (actionCategory == null)
      return;
    float width = 150f;
    foreach (InputAction action in (IEnumerable<InputAction>) ReInput.mapping.ActionsInCategory(actionCategory.id))
    {
      string text1 = action.descriptiveName != string.Empty ? action.descriptiveName : action.name;
      if (action.type == InputActionType.Button)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(text1, GUILayout.Width(width));
        this.DrawAddActionMapButton(this.selectedPlayer.id, action, AxisRange.Positive, this.selectedController, this.selectedMap);
        foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) this.selectedMap.AllMaps)
        {
          if (allMap.actionId == action.id)
            this.DrawActionAssignmentButton(this.selectedPlayer.id, action, AxisRange.Positive, this.selectedController, this.selectedMap, allMap);
        }
        GUILayout.EndHorizontal();
      }
      else if (action.type == InputActionType.Axis)
      {
        if (this.selectedController.type != ControllerType.Keyboard)
        {
          GUILayout.BeginHorizontal();
          GUILayout.Label(text1, GUILayout.Width(width));
          this.DrawAddActionMapButton(this.selectedPlayer.id, action, AxisRange.Full, this.selectedController, this.selectedMap);
          foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) this.selectedMap.AllMaps)
          {
            if (allMap.actionId == action.id && allMap.elementType != ControllerElementType.Button && allMap.axisType != AxisType.Split)
            {
              this.DrawActionAssignmentButton(this.selectedPlayer.id, action, AxisRange.Full, this.selectedController, this.selectedMap, allMap);
              this.DrawInvertButton(this.selectedPlayer.id, action, Pole.Positive, this.selectedController, this.selectedMap, allMap);
            }
          }
          GUILayout.EndHorizontal();
        }
        string text2 = action.positiveDescriptiveName != string.Empty ? action.positiveDescriptiveName : action.descriptiveName + " +";
        GUILayout.BeginHorizontal();
        GUILayoutOption[] guiLayoutOptionArray1 = new GUILayoutOption[1]
        {
          GUILayout.Width(width)
        };
        GUILayout.Label(text2, guiLayoutOptionArray1);
        this.DrawAddActionMapButton(this.selectedPlayer.id, action, AxisRange.Positive, this.selectedController, this.selectedMap);
        foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) this.selectedMap.AllMaps)
        {
          if (allMap.actionId == action.id && allMap.axisContribution == Pole.Positive && allMap.axisType != AxisType.Normal)
            this.DrawActionAssignmentButton(this.selectedPlayer.id, action, AxisRange.Positive, this.selectedController, this.selectedMap, allMap);
        }
        GUILayout.EndHorizontal();
        string text3 = action.negativeDescriptiveName != string.Empty ? action.negativeDescriptiveName : action.descriptiveName + " -";
        GUILayout.BeginHorizontal();
        GUILayoutOption[] guiLayoutOptionArray2 = new GUILayoutOption[1]
        {
          GUILayout.Width(width)
        };
        GUILayout.Label(text3, guiLayoutOptionArray2);
        this.DrawAddActionMapButton(this.selectedPlayer.id, action, AxisRange.Negative, this.selectedController, this.selectedMap);
        foreach (ActionElementMap allMap in (IEnumerable<ActionElementMap>) this.selectedMap.AllMaps)
        {
          if (allMap.actionId == action.id && allMap.axisContribution == Pole.Negative && allMap.axisType != AxisType.Normal)
            this.DrawActionAssignmentButton(this.selectedPlayer.id, action, AxisRange.Negative, this.selectedController, this.selectedMap, allMap);
        }
        GUILayout.EndHorizontal();
      }
    }
    if (GUI.enabled == enabled)
      return;
    GUI.enabled = enabled;
  }

  public void DrawActionAssignmentButton(
    int playerId,
    InputAction action,
    AxisRange actionRange,
    ControlRemappingDemo1.ControllerSelection controller,
    ControllerMap controllerMap,
    ActionElementMap elementMap)
  {
    if (GUILayout.Button(elementMap.elementIdentifierName, GUILayout.ExpandWidth(false), GUILayout.MinWidth(30f)))
    {
      this.EnqueueAction((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove, new InputMapper.Context()
      {
        actionId = action.id,
        actionRange = actionRange,
        controllerMap = controllerMap,
        actionElementMapToReplace = elementMap
      }));
      this.startListening = true;
    }
    GUILayout.Space(4f);
  }

  public void DrawInvertButton(
    int playerId,
    InputAction action,
    Pole actionAxisContribution,
    ControlRemappingDemo1.ControllerSelection controller,
    ControllerMap controllerMap,
    ActionElementMap elementMap)
  {
    bool invert = elementMap.invert;
    bool flag = GUILayout.Toggle((invert ? 1 : 0) != 0, "Invert", GUILayout.ExpandWidth(false));
    if (flag != invert)
      elementMap.invert = flag;
    GUILayout.Space(10f);
  }

  public void DrawAddActionMapButton(
    int playerId,
    InputAction action,
    AxisRange actionRange,
    ControlRemappingDemo1.ControllerSelection controller,
    ControllerMap controllerMap)
  {
    if (GUILayout.Button("Add...", GUILayout.ExpandWidth(false)))
    {
      this.EnqueueAction((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(ControlRemappingDemo1.ElementAssignmentChangeType.Add, new InputMapper.Context()
      {
        actionId = action.id,
        actionRange = actionRange,
        controllerMap = controllerMap
      }));
      this.startListening = true;
    }
    GUILayout.Space(10f);
  }

  public void ShowDialog() => this.dialog.Update();

  public void DrawModalWindow(string title, string message)
  {
    if (!this.dialog.enabled)
      return;
    GUILayout.Space(5f);
    GUILayout.Label(message, this.style_wordWrap);
    GUILayout.FlexibleSpace();
    GUILayout.BeginHorizontal();
    this.dialog.DrawConfirmButton("Okay");
    GUILayout.FlexibleSpace();
    this.dialog.DrawCancelButton();
    GUILayout.EndHorizontal();
  }

  public void DrawModalWindow_OkayOnly(string title, string message)
  {
    if (!this.dialog.enabled)
      return;
    GUILayout.Space(5f);
    GUILayout.Label(message, this.style_wordWrap);
    GUILayout.FlexibleSpace();
    GUILayout.BeginHorizontal();
    this.dialog.DrawConfirmButton("Okay");
    GUILayout.EndHorizontal();
  }

  public void DrawElementAssignmentWindow(string title, string message)
  {
    if (!this.dialog.enabled)
      return;
    GUILayout.Space(5f);
    GUILayout.Label(message, this.style_wordWrap);
    GUILayout.FlexibleSpace();
    if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange assignmentChange))
    {
      this.dialog.Cancel();
    }
    else
    {
      float f;
      if (!this.dialog.busy)
      {
        if (this.startListening && this.inputMapper.status == InputMapper.Status.Idle)
        {
          this.inputMapper.Start(assignmentChange.context);
          this.startListening = false;
        }
        if (this.conflictFoundEventData != null)
        {
          this.dialog.Confirm();
          return;
        }
        f = this.inputMapper.timeRemaining;
        if ((double) f == 0.0)
        {
          this.dialog.Cancel();
          return;
        }
      }
      else
        f = this.inputMapper.options.timeout;
      GUILayout.Label($"Assignment will be canceled in {((int) Mathf.Ceil(f)).ToString()}...", this.style_wordWrap);
    }
  }

  public void DrawElementAssignmentProtectedConflictWindow(string title, string message)
  {
    if (!this.dialog.enabled)
      return;
    GUILayout.Space(5f);
    GUILayout.Label(message, this.style_wordWrap);
    GUILayout.FlexibleSpace();
    if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
    {
      this.dialog.Cancel();
    }
    else
    {
      GUILayout.BeginHorizontal();
      this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
      GUILayout.FlexibleSpace();
      this.dialog.DrawCancelButton();
      GUILayout.EndHorizontal();
    }
  }

  public void DrawElementAssignmentNormalConflictWindow(string title, string message)
  {
    if (!this.dialog.enabled)
      return;
    GUILayout.Space(5f);
    GUILayout.Label(message, this.style_wordWrap);
    GUILayout.FlexibleSpace();
    if (!(this.actionQueue.Peek() is ControlRemappingDemo1.ElementAssignmentChange))
    {
      this.dialog.Cancel();
    }
    else
    {
      GUILayout.BeginHorizontal();
      this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Confirm, "Replace");
      GUILayout.FlexibleSpace();
      this.dialog.DrawConfirmButton(ControlRemappingDemo1.UserResponse.Custom1, "Add");
      GUILayout.FlexibleSpace();
      this.dialog.DrawCancelButton();
      GUILayout.EndHorizontal();
    }
  }

  public void DrawReassignOrRemoveElementAssignmentWindow(string title, string message)
  {
    if (!this.dialog.enabled)
      return;
    GUILayout.Space(5f);
    GUILayout.Label(message, this.style_wordWrap);
    GUILayout.FlexibleSpace();
    GUILayout.BeginHorizontal();
    this.dialog.DrawConfirmButton("Reassign");
    GUILayout.FlexibleSpace();
    this.dialog.DrawCancelButton("Remove");
    GUILayout.EndHorizontal();
  }

  public void DrawFallbackJoystickIdentificationWindow(string title, string message)
  {
    if (!this.dialog.enabled)
      return;
    if (!(this.actionQueue.Peek() is ControlRemappingDemo1.FallbackJoystickIdentification joystickIdentification))
    {
      this.dialog.Cancel();
    }
    else
    {
      GUILayout.Space(5f);
      GUILayout.Label(message, this.style_wordWrap);
      GUILayout.Label($"Press any button or axis on \"{joystickIdentification.joystickName}\" now.", this.style_wordWrap);
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Skip"))
      {
        this.dialog.Cancel();
      }
      else
      {
        if (this.dialog.busy || !ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(joystickIdentification.joystickId, 0.8f, false))
          return;
        this.dialog.Confirm();
      }
    }
  }

  public void DrawCalibrationWindow(string title, string message)
  {
    if (!this.dialog.enabled)
      return;
    if (!(this.actionQueue.Peek() is ControlRemappingDemo1.Calibration calibration))
    {
      this.dialog.Cancel();
    }
    else
    {
      GUILayout.Space(5f);
      GUILayout.Label(message, this.style_wordWrap);
      GUILayout.Space(20f);
      GUILayout.BeginHorizontal();
      bool enabled = GUI.enabled;
      GUILayout.BeginVertical(GUILayout.Width(200f));
      this.calibrateScrollPos = GUILayout.BeginScrollView(this.calibrateScrollPos);
      if (calibration.recording)
        GUI.enabled = false;
      IList<ControllerElementIdentifier> elementIdentifiers = (IList<ControllerElementIdentifier>) calibration.joystick.AxisElementIdentifiers;
      for (int index = 0; index < elementIdentifiers.Count; ++index)
      {
        ControllerElementIdentifier elementIdentifier = elementIdentifiers[index];
        bool flag1 = calibration.selectedElementIdentifierId == elementIdentifier.id;
        bool flag2 = GUILayout.Toggle((flag1 ? 1 : 0) != 0, elementIdentifier.name, (GUIStyle) "Button", GUILayout.ExpandWidth(false));
        if (flag1 != flag2)
          calibration.selectedElementIdentifierId = elementIdentifier.id;
      }
      if (GUI.enabled != enabled)
        GUI.enabled = enabled;
      GUILayout.EndScrollView();
      GUILayout.EndVertical();
      GUILayout.BeginVertical(GUILayout.Width(200f));
      if (calibration.selectedElementIdentifierId >= 0)
      {
        float axisRawById = calibration.joystick.GetAxisRawById(calibration.selectedElementIdentifierId);
        GUILayout.Label("Raw Value: " + axisRawById.ToString());
        int axisIndexById = calibration.joystick.GetAxisIndexById(calibration.selectedElementIdentifierId);
        AxisCalibration axis = calibration.calibrationMap.GetAxis(axisIndexById);
        GUILayout.Label("Calibrated Value: " + calibration.joystick.GetAxisById(calibration.selectedElementIdentifierId).ToString());
        GUILayout.Label("Zero: " + axis.calibratedZero.ToString());
        GUILayout.Label("Min: " + axis.calibratedMin.ToString());
        GUILayout.Label("Max: " + axis.calibratedMax.ToString());
        GUILayout.Label("Dead Zone: " + axis.deadZone.ToString());
        GUILayout.Space(15f);
        bool flag3 = GUILayout.Toggle((axis.enabled ? 1 : 0) != 0, "Enabled", (GUIStyle) "Button", GUILayout.ExpandWidth(false));
        if (axis.enabled != flag3)
          axis.enabled = flag3;
        GUILayout.Space(10f);
        bool flag4 = GUILayout.Toggle((calibration.recording ? 1 : 0) != 0, "Record Min/Max", (GUIStyle) "Button", GUILayout.ExpandWidth(false));
        if (flag4 != calibration.recording)
        {
          if (flag4)
          {
            axis.calibratedMax = 0.0f;
            axis.calibratedMin = 0.0f;
          }
          calibration.recording = flag4;
        }
        if (calibration.recording)
        {
          axis.calibratedMin = Mathf.Min(axis.calibratedMin, axisRawById, axis.calibratedMin);
          axis.calibratedMax = Mathf.Max(axis.calibratedMax, axisRawById, axis.calibratedMax);
          GUI.enabled = false;
        }
        if (GUILayout.Button("Set Zero", GUILayout.ExpandWidth(false)))
          axis.calibratedZero = axisRawById;
        if (GUILayout.Button("Set Dead Zone", GUILayout.ExpandWidth(false)))
          axis.deadZone = axisRawById;
        bool flag5 = GUILayout.Toggle((axis.invert ? 1 : 0) != 0, "Invert", (GUIStyle) "Button", GUILayout.ExpandWidth(false));
        if (axis.invert != flag5)
          axis.invert = flag5;
        GUILayout.Space(10f);
        if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
          axis.Reset();
        if (GUI.enabled != enabled)
          GUI.enabled = enabled;
      }
      else
        GUILayout.Label("Select an axis to begin.");
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      if (calibration.recording)
        GUI.enabled = false;
      if (GUILayout.Button("Close"))
      {
        this.calibrateScrollPos = new Vector2();
        this.dialog.Confirm();
      }
      if (GUI.enabled == enabled)
        return;
      GUI.enabled = enabled;
    }
  }

  public void DialogResultCallback(int queueActionId, ControlRemappingDemo1.UserResponse response)
  {
    foreach (ControlRemappingDemo1.QueueEntry action in this.actionQueue)
    {
      if (action.id == queueActionId)
      {
        if (response != ControlRemappingDemo1.UserResponse.Cancel)
        {
          action.Confirm(response);
          break;
        }
        action.Cancel();
        break;
      }
    }
  }

  public Rect GetScreenCenteredRect(float width, float height)
  {
    return new Rect((float) ((double) Screen.width * 0.5 - (double) width * 0.5), (float) ((double) Screen.height * 0.5 - (double) height * 0.5), width, height);
  }

  public void EnqueueAction(ControlRemappingDemo1.QueueEntry entry)
  {
    if (entry == null)
      return;
    this.busy = true;
    GUI.enabled = false;
    this.actionQueue.Enqueue(entry);
  }

  public void ProcessQueue()
  {
    if (this.dialog.enabled || this.busy || this.actionQueue.Count == 0)
      return;
    while (this.actionQueue.Count > 0)
    {
      ControlRemappingDemo1.QueueEntry entry = this.actionQueue.Peek();
      bool flag = false;
      switch (entry.queueActionType)
      {
        case ControlRemappingDemo1.QueueActionType.JoystickAssignment:
          flag = this.ProcessJoystickAssignmentChange((ControlRemappingDemo1.JoystickAssignmentChange) entry);
          break;
        case ControlRemappingDemo1.QueueActionType.ElementAssignment:
          flag = this.ProcessElementAssignmentChange((ControlRemappingDemo1.ElementAssignmentChange) entry);
          break;
        case ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification:
          flag = this.ProcessFallbackJoystickIdentification((ControlRemappingDemo1.FallbackJoystickIdentification) entry);
          break;
        case ControlRemappingDemo1.QueueActionType.Calibrate:
          flag = this.ProcessCalibration((ControlRemappingDemo1.Calibration) entry);
          break;
      }
      if (!flag)
        break;
      this.actionQueue.Dequeue();
    }
  }

  public bool ProcessJoystickAssignmentChange(
    ControlRemappingDemo1.JoystickAssignmentChange entry)
  {
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
      return true;
    Player player = ReInput.players.GetPlayer(entry.playerId);
    if (player == null)
      return true;
    if (!entry.assign)
    {
      player.controllers.RemoveController(ControllerType.Joystick, entry.joystickId);
      this.ControllerSelectionChanged();
      return true;
    }
    if (player.controllers.ContainsController(ControllerType.Joystick, entry.joystickId))
      return true;
    if (!ReInput.controllers.IsJoystickAssigned(entry.joystickId) || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
    {
      player.controllers.AddController(ControllerType.Joystick, entry.joystickId, true);
      this.ControllerSelectionChanged();
      return true;
    }
    this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties()
    {
      title = "Joystick Reassignment",
      message = $"This joystick is already assigned to another player. Do you want to reassign this joystick to {player.descriptiveName}?",
      rect = this.GetScreenCenteredRect(250f, 200f),
      windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
    }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
    return false;
  }

  public bool ProcessElementAssignmentChange(
    ControlRemappingDemo1.ElementAssignmentChange entry)
  {
    switch (entry.changeType)
    {
      case ControlRemappingDemo1.ElementAssignmentChangeType.Add:
      case ControlRemappingDemo1.ElementAssignmentChangeType.Replace:
        return this.ProcessAddOrReplaceElementAssignment(entry);
      case ControlRemappingDemo1.ElementAssignmentChangeType.Remove:
        return this.ProcessRemoveElementAssignment(entry);
      case ControlRemappingDemo1.ElementAssignmentChangeType.ReassignOrRemove:
        return this.ProcessRemoveOrReassignElementAssignment(entry);
      case ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck:
        return this.ProcessElementAssignmentConflictCheck(entry);
      default:
        throw new NotImplementedException();
    }
  }

  public bool ProcessRemoveOrReassignElementAssignment(
    ControlRemappingDemo1.ElementAssignmentChange entry)
  {
    if (entry.context.controllerMap == null)
      return true;
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
    {
      this.actionQueue.Enqueue((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(entry)
      {
        changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Remove
      });
      return true;
    }
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
    {
      this.actionQueue.Enqueue((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(entry)
      {
        changeType = ControlRemappingDemo1.ElementAssignmentChangeType.Replace
      });
      return true;
    }
    this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties()
    {
      title = "Reassign or Remove",
      message = "Do you want to reassign or remove this assignment?",
      rect = this.GetScreenCenteredRect(250f, 200f),
      windowDrawDelegate = new Action<string, string>(this.DrawReassignOrRemoveElementAssignmentWindow)
    }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
    return false;
  }

  public bool ProcessRemoveElementAssignment(
    ControlRemappingDemo1.ElementAssignmentChange entry)
  {
    if (entry.context.controllerMap == null || entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
      return true;
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
    {
      entry.context.controllerMap.DeleteElementMap(entry.context.actionElementMapToReplace.id);
      return true;
    }
    this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.DeleteAssignmentConfirmation, new ControlRemappingDemo1.WindowProperties()
    {
      title = "Remove Assignment",
      message = "Are you sure you want to remove this assignment?",
      rect = this.GetScreenCenteredRect(250f, 200f),
      windowDrawDelegate = new Action<string, string>(this.DrawModalWindow)
    }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
    return false;
  }

  public bool ProcessAddOrReplaceElementAssignment(
    ControlRemappingDemo1.ElementAssignmentChange entry)
  {
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
    {
      this.inputMapper.Stop();
      return true;
    }
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
    {
      if (Event.current.type != EventType.Layout)
        return false;
      if (this.conflictFoundEventData != null)
        this.actionQueue.Enqueue((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.ElementAssignmentChange(entry)
        {
          changeType = ControlRemappingDemo1.ElementAssignmentChangeType.ConflictCheck
        });
      return true;
    }
    string str;
    if (entry.context.controllerMap.controllerType == ControllerType.Keyboard)
    {
      str = (Application.platform == RuntimePlatform.OSXEditor ? 1 : (Application.platform == RuntimePlatform.OSXPlayer ? 1 : 0)) == 0 ? "Press any key to assign it to this action. You may also use the modifier keys Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second." : "Press any key to assign it to this action. You may also use the modifier keys Command, Control, Alt, and Shift. If you wish to assign a modifier key itself to this action, press and hold the key for 1 second.";
      if (Application.isEditor)
        str += "\n\nNOTE: Some modifier key combinations will not work in the Unity Editor, but they will work in a game build.";
    }
    else
      str = entry.context.controllerMap.controllerType != ControllerType.Mouse ? "Press any button or axis to assign it to this action." : "Press any mouse button or axis to assign it to this action.\n\nTo assign mouse movement axes, move the mouse quickly in the direction you want mapped to the action. Slow movements will be ignored.";
    this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties()
    {
      title = "Assign",
      message = str,
      rect = this.GetScreenCenteredRect(250f, 200f),
      windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentWindow)
    }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
    return false;
  }

  public bool ProcessElementAssignmentConflictCheck(
    ControlRemappingDemo1.ElementAssignmentChange entry)
  {
    if (entry.context.controllerMap == null)
      return true;
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled)
    {
      this.inputMapper.Stop();
      return true;
    }
    if (this.conflictFoundEventData == null)
      return true;
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
    {
      if (entry.response == ControlRemappingDemo1.UserResponse.Confirm)
      {
        ((Action<InputMapper.ConflictResponse>) this.conflictFoundEventData.responseCallback)(InputMapper.ConflictResponse.Replace);
      }
      else
      {
        if (entry.response != ControlRemappingDemo1.UserResponse.Custom1)
          throw new NotImplementedException();
        ((Action<InputMapper.ConflictResponse>) this.conflictFoundEventData.responseCallback)(InputMapper.ConflictResponse.Add);
      }
      return true;
    }
    if (this.conflictFoundEventData.isProtected)
    {
      string str = this.conflictFoundEventData.assignment.elementDisplayName + " is already in use and is protected from reassignment. You cannot remove the protected assignment, but you can still assign the action to this element. If you do so, the element will trigger multiple actions when activated.";
      this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties()
      {
        title = "Assignment Conflict",
        message = str,
        rect = this.GetScreenCenteredRect(250f, 200f),
        windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentProtectedConflictWindow)
      }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
    }
    else
    {
      string str = this.conflictFoundEventData.assignment.elementDisplayName + " is already in use. You may replace the other conflicting assignments, add this assignment anyway which will leave multiple actions assigned to this element, or cancel this assignment.";
      this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.AssignElement, new ControlRemappingDemo1.WindowProperties()
      {
        title = "Assignment Conflict",
        message = str,
        rect = this.GetScreenCenteredRect(250f, 200f),
        windowDrawDelegate = new Action<string, string>(this.DrawElementAssignmentNormalConflictWindow)
      }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
    }
    return false;
  }

  public bool ProcessFallbackJoystickIdentification(
    ControlRemappingDemo1.FallbackJoystickIdentification entry)
  {
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
      return true;
    this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties()
    {
      title = "Joystick Identification Required",
      message = "A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:",
      rect = this.GetScreenCenteredRect(250f, 200f),
      windowDrawDelegate = new Action<string, string>(this.DrawFallbackJoystickIdentificationWindow)
    }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback), 1f);
    return false;
  }

  public bool ProcessCalibration(ControlRemappingDemo1.Calibration entry)
  {
    if (entry.state == ControlRemappingDemo1.QueueEntry.State.Canceled || entry.state == ControlRemappingDemo1.QueueEntry.State.Confirmed)
      return true;
    this.dialog.StartModal(entry.id, ControlRemappingDemo1.DialogHelper.DialogType.JoystickConflict, new ControlRemappingDemo1.WindowProperties()
    {
      title = "Calibrate Controller",
      message = $"Select an axis to calibrate on the {entry.joystick.name}.",
      rect = this.GetScreenCenteredRect(450f, 480f),
      windowDrawDelegate = new Action<string, string>(this.DrawCalibrationWindow)
    }, new Action<int, ControlRemappingDemo1.UserResponse>(this.DialogResultCallback));
    return false;
  }

  public void PlayerSelectionChanged() => this.ClearControllerSelection();

  public void ControllerSelectionChanged() => this.ClearMapSelection();

  public void ClearControllerSelection()
  {
    this.selectedController.Clear();
    this.ClearMapSelection();
  }

  public void ClearMapSelection()
  {
    this.selectedMapCategoryId = -1;
    this.selectedMap = (ControllerMap) null;
  }

  public void ResetAll()
  {
    this.ClearWorkingVars();
    this.initialized = false;
    this.showMenu = false;
  }

  public void ClearWorkingVars()
  {
    this.selectedPlayer = (Player) null;
    this.ClearMapSelection();
    this.selectedController.Clear();
    this.actionScrollPos = new Vector2();
    this.dialog.FullReset();
    this.actionQueue.Clear();
    this.busy = false;
    this.startListening = false;
    this.conflictFoundEventData = (InputMapper.ConflictFoundEventData) null;
    this.inputMapper.Stop();
  }

  public void SetGUIStateStart()
  {
    this.guiState = true;
    if (this.busy)
      this.guiState = false;
    this.pageGUIState = this.guiState && !this.busy && !this.dialog.enabled && !this.dialog.busy;
    if (GUI.enabled == this.guiState)
      return;
    GUI.enabled = this.guiState;
  }

  public void SetGUIStateEnd()
  {
    this.guiState = true;
    if (GUI.enabled)
      return;
    GUI.enabled = this.guiState;
  }

  public void JoystickConnected(ControllerStatusChangedEventArgs args)
  {
    if (ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId))
    {
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
      {
        if (allPlayer.controllers.ContainsController(args.controllerType, args.controllerId))
          ReInput.userDataStore.LoadControllerData(allPlayer.id, args.controllerType, args.controllerId);
      }
    }
    else
      ReInput.userDataStore.LoadControllerData(args.controllerType, args.controllerId);
    if (!ReInput.unityJoystickIdentificationRequired)
      return;
    this.IdentifyAllJoysticks();
  }

  public void JoystickPreDisconnect(ControllerStatusChangedEventArgs args)
  {
    if (this.selectedController.hasSelection && args.controllerType == this.selectedController.type && args.controllerId == this.selectedController.id)
      this.ClearControllerSelection();
    if (!this.showMenu)
      return;
    if (ReInput.controllers.IsControllerAssigned(args.controllerType, args.controllerId))
    {
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
      {
        if (allPlayer.controllers.ContainsController(args.controllerType, args.controllerId))
          ReInput.userDataStore.SaveControllerData(allPlayer.id, args.controllerType, args.controllerId);
      }
    }
    else
      ReInput.userDataStore.SaveControllerData(args.controllerType, args.controllerId);
  }

  public void JoystickDisconnected(ControllerStatusChangedEventArgs args)
  {
    if (this.showMenu)
      this.ClearWorkingVars();
    if (!ReInput.unityJoystickIdentificationRequired)
      return;
    this.IdentifyAllJoysticks();
  }

  public void OnConflictFound(InputMapper.ConflictFoundEventData data)
  {
    this.conflictFoundEventData = data;
  }

  public void OnStopped(InputMapper.StoppedEventData data)
  {
    this.conflictFoundEventData = (InputMapper.ConflictFoundEventData) null;
  }

  public void IdentifyAllJoysticks()
  {
    if (ReInput.controllers.joystickCount == 0)
      return;
    this.ClearWorkingVars();
    this.Open();
    foreach (Joystick joystick in (IEnumerable<Joystick>) ReInput.controllers.Joysticks)
      this.actionQueue.Enqueue((ControlRemappingDemo1.QueueEntry) new ControlRemappingDemo1.FallbackJoystickIdentification(joystick.id, joystick.name));
  }

  public void CheckRecompile()
  {
  }

  public void RecompileWindow(int windowId)
  {
  }

  public class ControllerSelection
  {
    public int _id;
    public int _idPrev;
    public ControllerType _type;
    public ControllerType _typePrev;

    public ControllerSelection() => this.Clear();

    public int id
    {
      get => this._id;
      set
      {
        this._idPrev = this._id;
        this._id = value;
      }
    }

    public ControllerType type
    {
      get => this._type;
      set
      {
        this._typePrev = this._type;
        this._type = value;
      }
    }

    public int idPrev => this._idPrev;

    public ControllerType typePrev => this._typePrev;

    public bool hasSelection => this._id >= 0;

    public void Set(int id, ControllerType type)
    {
      this.id = id;
      this.type = type;
    }

    public void Clear()
    {
      this._id = -1;
      this._idPrev = -1;
      this._type = ControllerType.Joystick;
      this._typePrev = ControllerType.Joystick;
    }
  }

  public class DialogHelper
  {
    public const float openBusyDelay = 0.25f;
    public const float closeBusyDelay = 0.1f;
    public ControlRemappingDemo1.DialogHelper.DialogType _type;
    public bool _enabled;
    public float _busyTime;
    public bool _busyTimerRunning;
    public Action<int> drawWindowDelegate;
    public GUI.WindowFunction drawWindowFunction;
    public ControlRemappingDemo1.WindowProperties windowProperties;
    public int currentActionId;
    public Action<int, ControlRemappingDemo1.UserResponse> resultCallback;

    public float busyTimer
    {
      get => !this._busyTimerRunning ? 0.0f : this._busyTime - Time.realtimeSinceStartup;
    }

    public bool enabled
    {
      get => this._enabled;
      set
      {
        if (value)
        {
          if (this._type == ControlRemappingDemo1.DialogHelper.DialogType.None)
            return;
          this.StateChanged(0.25f);
        }
        else
        {
          this._enabled = value;
          this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
          this.StateChanged(0.1f);
        }
      }
    }

    public ControlRemappingDemo1.DialogHelper.DialogType type
    {
      get => !this._enabled ? ControlRemappingDemo1.DialogHelper.DialogType.None : this._type;
      set
      {
        if (value == ControlRemappingDemo1.DialogHelper.DialogType.None)
        {
          this._enabled = false;
          this.StateChanged(0.1f);
        }
        else
        {
          this._enabled = true;
          this.StateChanged(0.25f);
        }
        this._type = value;
      }
    }

    public bool busy => this._busyTimerRunning;

    public DialogHelper()
    {
      this.drawWindowDelegate = new Action<int>(this.DrawWindow);
      this.drawWindowFunction = new GUI.WindowFunction(this.drawWindowDelegate.Invoke);
    }

    public void StartModal(
      int queueActionId,
      ControlRemappingDemo1.DialogHelper.DialogType type,
      ControlRemappingDemo1.WindowProperties windowProperties,
      Action<int, ControlRemappingDemo1.UserResponse> resultCallback)
    {
      this.StartModal(queueActionId, type, windowProperties, resultCallback, -1f);
    }

    public void StartModal(
      int queueActionId,
      ControlRemappingDemo1.DialogHelper.DialogType type,
      ControlRemappingDemo1.WindowProperties windowProperties,
      Action<int, ControlRemappingDemo1.UserResponse> resultCallback,
      float openBusyDelay)
    {
      this.currentActionId = queueActionId;
      this.windowProperties = windowProperties;
      this.type = type;
      this.resultCallback = resultCallback;
      if ((double) openBusyDelay < 0.0)
        return;
      this.StateChanged(openBusyDelay);
    }

    public void Update()
    {
      this.Draw();
      this.UpdateTimers();
    }

    public void Draw()
    {
      if (!this._enabled)
        return;
      bool enabled = GUI.enabled;
      GUI.enabled = true;
      GUILayout.Window(this.windowProperties.windowId, this.windowProperties.rect, this.drawWindowFunction, this.windowProperties.title);
      GUI.FocusWindow(this.windowProperties.windowId);
      if (GUI.enabled == enabled)
        return;
      GUI.enabled = enabled;
    }

    public void DrawConfirmButton() => this.DrawConfirmButton("Confirm");

    public void DrawConfirmButton(string title)
    {
      bool enabled = GUI.enabled;
      if (this.busy)
        GUI.enabled = false;
      if (GUILayout.Button(title))
        this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);
      if (GUI.enabled == enabled)
        return;
      GUI.enabled = enabled;
    }

    public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response)
    {
      this.DrawConfirmButton(response, "Confirm");
    }

    public void DrawConfirmButton(ControlRemappingDemo1.UserResponse response, string title)
    {
      bool enabled = GUI.enabled;
      if (this.busy)
        GUI.enabled = false;
      if (GUILayout.Button(title))
        this.Confirm(response);
      if (GUI.enabled == enabled)
        return;
      GUI.enabled = enabled;
    }

    public void DrawCancelButton() => this.DrawCancelButton("Cancel");

    public void DrawCancelButton(string title)
    {
      bool enabled = GUI.enabled;
      if (this.busy)
        GUI.enabled = false;
      if (GUILayout.Button(title))
        this.Cancel();
      if (GUI.enabled == enabled)
        return;
      GUI.enabled = enabled;
    }

    public void Confirm() => this.Confirm(ControlRemappingDemo1.UserResponse.Confirm);

    public void Confirm(ControlRemappingDemo1.UserResponse response)
    {
      this.resultCallback(this.currentActionId, response);
      this.Close();
    }

    public void Cancel()
    {
      this.resultCallback(this.currentActionId, ControlRemappingDemo1.UserResponse.Cancel);
      this.Close();
    }

    public void DrawWindow(int windowId)
    {
      this.windowProperties.windowDrawDelegate(this.windowProperties.title, this.windowProperties.message);
    }

    public void UpdateTimers()
    {
      if (!this._busyTimerRunning || (double) this.busyTimer > 0.0)
        return;
      this._busyTimerRunning = false;
    }

    public void StartBusyTimer(float time)
    {
      this._busyTime = time + Time.realtimeSinceStartup;
      this._busyTimerRunning = true;
    }

    public void Close()
    {
      this.Reset();
      this.StateChanged(0.1f);
    }

    public void StateChanged(float delay) => this.StartBusyTimer(delay);

    public void Reset()
    {
      this._enabled = false;
      this._type = ControlRemappingDemo1.DialogHelper.DialogType.None;
      this.currentActionId = -1;
      this.resultCallback = (Action<int, ControlRemappingDemo1.UserResponse>) null;
    }

    public void ResetTimers() => this._busyTimerRunning = false;

    public void FullReset()
    {
      this.Reset();
      this.ResetTimers();
    }

    public enum DialogType
    {
      None = 0,
      JoystickConflict = 1,
      ElementConflict = 2,
      KeyConflict = 3,
      DeleteAssignmentConfirmation = 10, // 0x0000000A
      AssignElement = 11, // 0x0000000B
    }
  }

  public abstract class QueueEntry
  {
    [CompilerGenerated]
    public int \u003Cid\u003Ek__BackingField;
    [CompilerGenerated]
    public ControlRemappingDemo1.QueueActionType \u003CqueueActionType\u003Ek__BackingField;
    [CompilerGenerated]
    public ControlRemappingDemo1.QueueEntry.State \u003Cstate\u003Ek__BackingField;
    [CompilerGenerated]
    public ControlRemappingDemo1.UserResponse \u003Cresponse\u003Ek__BackingField;
    public static int uidCounter;

    public int id
    {
      get => this.\u003Cid\u003Ek__BackingField;
      set => this.\u003Cid\u003Ek__BackingField = value;
    }

    public ControlRemappingDemo1.QueueActionType queueActionType
    {
      get => this.\u003CqueueActionType\u003Ek__BackingField;
      set => this.\u003CqueueActionType\u003Ek__BackingField = value;
    }

    public ControlRemappingDemo1.QueueEntry.State state
    {
      get => this.\u003Cstate\u003Ek__BackingField;
      set => this.\u003Cstate\u003Ek__BackingField = value;
    }

    public ControlRemappingDemo1.UserResponse response
    {
      get => this.\u003Cresponse\u003Ek__BackingField;
      set => this.\u003Cresponse\u003Ek__BackingField = value;
    }

    public static int nextId
    {
      get
      {
        int uidCounter = ControlRemappingDemo1.QueueEntry.uidCounter;
        ++ControlRemappingDemo1.QueueEntry.uidCounter;
        return uidCounter;
      }
    }

    public QueueEntry(
      ControlRemappingDemo1.QueueActionType queueActionType)
    {
      this.id = ControlRemappingDemo1.QueueEntry.nextId;
      this.queueActionType = queueActionType;
    }

    public void Confirm(ControlRemappingDemo1.UserResponse response)
    {
      this.state = ControlRemappingDemo1.QueueEntry.State.Confirmed;
      this.response = response;
    }

    public void Cancel() => this.state = ControlRemappingDemo1.QueueEntry.State.Canceled;

    public enum State
    {
      Waiting,
      Confirmed,
      Canceled,
    }
  }

  public class JoystickAssignmentChange : ControlRemappingDemo1.QueueEntry
  {
    [CompilerGenerated]
    public int \u003CplayerId\u003Ek__BackingField;
    [CompilerGenerated]
    public int \u003CjoystickId\u003Ek__BackingField;
    [CompilerGenerated]
    public bool \u003Cassign\u003Ek__BackingField;

    public int playerId
    {
      get => this.\u003CplayerId\u003Ek__BackingField;
      set => this.\u003CplayerId\u003Ek__BackingField = value;
    }

    public int joystickId
    {
      get => this.\u003CjoystickId\u003Ek__BackingField;
      set => this.\u003CjoystickId\u003Ek__BackingField = value;
    }

    public bool assign
    {
      get => this.\u003Cassign\u003Ek__BackingField;
      set => this.\u003Cassign\u003Ek__BackingField = value;
    }

    public JoystickAssignmentChange(int newPlayerId, int joystickId, bool assign)
      : base(ControlRemappingDemo1.QueueActionType.JoystickAssignment)
    {
      this.playerId = newPlayerId;
      this.joystickId = joystickId;
      this.assign = assign;
    }
  }

  public class ElementAssignmentChange : ControlRemappingDemo1.QueueEntry
  {
    [CompilerGenerated]
    public ControlRemappingDemo1.ElementAssignmentChangeType \u003CchangeType\u003Ek__BackingField;
    [CompilerGenerated]
    public InputMapper.Context \u003Ccontext\u003Ek__BackingField;

    public ControlRemappingDemo1.ElementAssignmentChangeType changeType
    {
      get => this.\u003CchangeType\u003Ek__BackingField;
      set => this.\u003CchangeType\u003Ek__BackingField = value;
    }

    public InputMapper.Context context
    {
      get => this.\u003Ccontext\u003Ek__BackingField;
      set => this.\u003Ccontext\u003Ek__BackingField = value;
    }

    public ElementAssignmentChange(
      ControlRemappingDemo1.ElementAssignmentChangeType changeType,
      InputMapper.Context context)
      : base(ControlRemappingDemo1.QueueActionType.ElementAssignment)
    {
      this.changeType = changeType;
      this.context = context;
    }

    public ElementAssignmentChange(
      ControlRemappingDemo1.ElementAssignmentChange other)
      : this(other.changeType, other.context.Clone())
    {
    }
  }

  public class FallbackJoystickIdentification : ControlRemappingDemo1.QueueEntry
  {
    [CompilerGenerated]
    public int \u003CjoystickId\u003Ek__BackingField;
    [CompilerGenerated]
    public string \u003CjoystickName\u003Ek__BackingField;

    public int joystickId
    {
      get => this.\u003CjoystickId\u003Ek__BackingField;
      set => this.\u003CjoystickId\u003Ek__BackingField = value;
    }

    public string joystickName
    {
      get => this.\u003CjoystickName\u003Ek__BackingField;
      set => this.\u003CjoystickName\u003Ek__BackingField = value;
    }

    public FallbackJoystickIdentification(int joystickId, string joystickName)
      : base(ControlRemappingDemo1.QueueActionType.FallbackJoystickIdentification)
    {
      this.joystickId = joystickId;
      this.joystickName = joystickName;
    }
  }

  public class Calibration : ControlRemappingDemo1.QueueEntry
  {
    [CompilerGenerated]
    public Player \u003Cplayer\u003Ek__BackingField;
    [CompilerGenerated]
    public ControllerType \u003CcontrollerType\u003Ek__BackingField;
    [CompilerGenerated]
    public Joystick \u003Cjoystick\u003Ek__BackingField;
    [CompilerGenerated]
    public CalibrationMap \u003CcalibrationMap\u003Ek__BackingField;
    public int selectedElementIdentifierId;
    public bool recording;

    public Player player
    {
      get => this.\u003Cplayer\u003Ek__BackingField;
      set => this.\u003Cplayer\u003Ek__BackingField = value;
    }

    public ControllerType controllerType
    {
      get => this.\u003CcontrollerType\u003Ek__BackingField;
      set => this.\u003CcontrollerType\u003Ek__BackingField = value;
    }

    public Joystick joystick
    {
      get => this.\u003Cjoystick\u003Ek__BackingField;
      set => this.\u003Cjoystick\u003Ek__BackingField = value;
    }

    public CalibrationMap calibrationMap
    {
      get => this.\u003CcalibrationMap\u003Ek__BackingField;
      set => this.\u003CcalibrationMap\u003Ek__BackingField = value;
    }

    public Calibration(Player player, Joystick joystick, CalibrationMap calibrationMap)
      : base(ControlRemappingDemo1.QueueActionType.Calibrate)
    {
      this.player = player;
      this.joystick = joystick;
      this.calibrationMap = calibrationMap;
      this.selectedElementIdentifierId = -1;
    }
  }

  public struct WindowProperties
  {
    public int windowId;
    public Rect rect;
    public Action<string, string> windowDrawDelegate;
    public string title;
    public string message;
  }

  public enum QueueActionType
  {
    None,
    JoystickAssignment,
    ElementAssignment,
    FallbackJoystickIdentification,
    Calibrate,
  }

  public enum ElementAssignmentChangeType
  {
    Add,
    Replace,
    Remove,
    ReassignOrRemove,
    ConflictCheck,
  }

  public enum UserResponse
  {
    Confirm,
    Cancel,
    Custom1,
    Custom2,
  }
}
