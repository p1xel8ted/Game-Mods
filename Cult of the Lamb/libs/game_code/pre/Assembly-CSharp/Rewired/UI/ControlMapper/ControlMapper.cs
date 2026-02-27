// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ControlMapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class ControlMapper : MonoBehaviour
{
  public const int versionMajor = 1;
  public const int versionMinor = 1;
  public const bool usesTMPro = true;
  private const float blockInputOnFocusTimeout = 0.1f;
  private const string buttonIdentifier_playerSelection = "PlayerSelection";
  private const string buttonIdentifier_removeController = "RemoveController";
  private const string buttonIdentifier_assignController = "AssignController";
  private const string buttonIdentifier_calibrateController = "CalibrateController";
  private const string buttonIdentifier_editInputBehaviors = "EditInputBehaviors";
  private const string buttonIdentifier_mapCategorySelection = "MapCategorySelection";
  private const string buttonIdentifier_assignedControllerSelection = "AssignedControllerSelection";
  private const string buttonIdentifier_done = "Done";
  private const string buttonIdentifier_restoreDefaults = "RestoreDefaults";
  [SerializeField]
  [Tooltip("Must be assigned a Rewired Input Manager scene object or prefab.")]
  private Rewired.InputManager _rewiredInputManager;
  [SerializeField]
  [Tooltip("Set to True to prevent the Game Object from being destroyed when a new scene is loaded.\n\nNOTE: Changing this value from True to False at runtime will have no effect because Object.DontDestroyOnLoad cannot be undone once set.")]
  private bool _dontDestroyOnLoad;
  [SerializeField]
  [Tooltip("Open the control mapping screen immediately on start. Mainly used for testing.")]
  private bool _openOnStart;
  [SerializeField]
  [Tooltip("The Layout of the Keyboard Maps to be displayed.")]
  private int _keyboardMapDefaultLayout;
  [SerializeField]
  [Tooltip("The Layout of the Mouse Maps to be displayed.")]
  private int _mouseMapDefaultLayout;
  [SerializeField]
  [Tooltip("The Layout of the Mouse Maps to be displayed.")]
  private int _joystickMapDefaultLayout;
  [SerializeField]
  private Rewired.UI.ControlMapper.ControlMapper.MappingSet[] _mappingSets = new Rewired.UI.ControlMapper.ControlMapper.MappingSet[1]
  {
    Rewired.UI.ControlMapper.ControlMapper.MappingSet.Default
  };
  [SerializeField]
  [Tooltip("Display a selectable list of Players. If your game only supports 1 player, you can disable this.")]
  private bool _showPlayers = true;
  [SerializeField]
  [Tooltip("Display the Controller column for input mapping.")]
  private bool _showControllers = true;
  [SerializeField]
  [Tooltip("Display the Keyboard column for input mapping.")]
  private bool _showKeyboard = true;
  [SerializeField]
  [Tooltip("Display the Mouse column for input mapping.")]
  private bool _showMouse = true;
  [SerializeField]
  [Tooltip("The maximum number of controllers allowed to be assigned to a Player. If set to any value other than 1, a selectable list of currently-assigned controller will be displayed to the user. [0 = infinite]")]
  private int _maxControllersPerPlayer = 1;
  [SerializeField]
  [Tooltip("Display section labels for each Action Category in the input field grid. Only applies if Action Categories are used to display the Action list.")]
  private bool _showActionCategoryLabels;
  [SerializeField]
  [Tooltip("The number of input fields to display for the keyboard. If you want to support alternate mappings on the same device, set this to 2 or more.")]
  private int _keyboardInputFieldCount = 2;
  [SerializeField]
  [Tooltip("The number of input fields to display for the mouse. If you want to support alternate mappings on the same device, set this to 2 or more.")]
  private int _mouseInputFieldCount = 1;
  [SerializeField]
  [Tooltip("The number of input fields to display for joysticks. If you want to support alternate mappings on the same device, set this to 2 or more.")]
  private int _controllerInputFieldCount = 1;
  [SerializeField]
  [Tooltip("Display a full-axis input assignment field for every axis-type Action in the input field grid. Also displays an invert toggle for the user  to invert the full-axis assignment direction.\n\n*IMPORTANT*: This field is required if you have made any full-axis assignments in the Rewired Input Manager or in saved XML user data. Disabling this field when you have full-axis assignments will result in the inability for the user to view, remove, or modify these full-axis assignments. In addition, these assignments may cause conflicts when trying to remap the same axes to Actions.")]
  private bool _showFullAxisInputFields = true;
  [SerializeField]
  [Tooltip("Display a positive and negative input assignment field for every axis-type Action in the input field grid.\n\n*IMPORTANT*: These fields are required to assign buttons, keyboard keys, and hat or D-Pad directions to axis-type Actions. If you have made any split-axis assignments or button/key/D-pad assignments to axis-type Actions in the Rewired Input Manager or in saved XML user data, disabling these fields will result in the inability for the user to view, remove, or modify these assignments. In addition, these assignments may cause conflicts when trying to remap the same elements to Actions.")]
  private bool _showSplitAxisInputFields = true;
  [SerializeField]
  [Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to make the conflicting assignment anyway.")]
  private bool _allowElementAssignmentConflicts;
  [SerializeField]
  [Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to swap conflicting assignments. This only applies to the first conflicting assignment found. This option will not be displayed if allowElementAssignmentConflicts is true.")]
  private bool _allowElementAssignmentSwap;
  [SerializeField]
  [Tooltip("The width in relative pixels of the Action label column.")]
  private int _actionLabelWidth = 200;
  [SerializeField]
  [Tooltip("The width in relative pixels of the Keyboard column.")]
  private int _keyboardColMaxWidth = 360;
  [SerializeField]
  [Tooltip("The width in relative pixels of the Mouse column.")]
  private int _mouseColMaxWidth = 200;
  [SerializeField]
  [Tooltip("The width in relative pixels of the Controller column.")]
  private int _controllerColMaxWidth = 200;
  [SerializeField]
  [Tooltip("The height in relative pixels of the input grid button rows.")]
  private int _inputRowHeight = 40;
  [SerializeField]
  [Tooltip("The padding of the input grid button rows.")]
  private RectOffset _inputRowPadding = new RectOffset();
  [SerializeField]
  [Tooltip("The width in relative pixels of spacing between input fields in a single column.")]
  private int _inputRowFieldSpacing;
  [SerializeField]
  [Tooltip("The width in relative pixels of spacing between columns.")]
  private int _inputColumnSpacing = 40;
  [SerializeField]
  [Tooltip("The height in relative pixels of the space between Action Category sections. Only applicable if Show Action Category Labels is checked.")]
  private int _inputRowCategorySpacing = 20;
  [SerializeField]
  [Tooltip("The width in relative pixels of the invert toggle buttons.")]
  private int _invertToggleWidth = 40;
  [SerializeField]
  [Tooltip("The width in relative pixels of generated popup windows.")]
  private int _defaultWindowWidth = 500;
  [SerializeField]
  [Tooltip("The height in relative pixels of generated popup windows.")]
  private int _defaultWindowHeight = 400;
  [SerializeField]
  [Tooltip("The time in seconds the user has to press an element on a controller when assigning a controller to a Player. If this time elapses with no user input a controller, the assignment will be canceled.")]
  private float _controllerAssignmentTimeout = 5f;
  [SerializeField]
  [Tooltip("The time in seconds the user has to press an element on a controller while waiting for axes to be centered before assigning input.")]
  private float _preInputAssignmentTimeout = 5f;
  [SerializeField]
  [Tooltip("The time in seconds the user has to press an element on a controller when assigning input. If this time elapses with no user input on the target controller, the assignment will be canceled.")]
  private float _inputAssignmentTimeout = 5f;
  [SerializeField]
  [Tooltip("The time in seconds the user has to press an element on a controller during calibration.")]
  private float _axisCalibrationTimeout = 5f;
  [SerializeField]
  [Tooltip("If checked, mouse X-axis movement will always be ignored during input assignment. Check this if you don't want the horizontal mouse axis to be user-assignable to any Actions.")]
  private bool _ignoreMouseXAxisAssignment = true;
  [SerializeField]
  [Tooltip("If checked, mouse Y-axis movement will always be ignored during input assignment. Check this if you don't want the vertical mouse axis to be user-assignable to any Actions.")]
  private bool _ignoreMouseYAxisAssignment = true;
  [SerializeField]
  [Tooltip("An Action that when activated will alternately close or open the main screen as long as no popup windows are open.")]
  private int _screenToggleAction = -1;
  [SerializeField]
  [Tooltip("An Action that when activated will open the main screen if it is closed.")]
  private int _screenOpenAction = -1;
  [SerializeField]
  [Tooltip("An Action that when activated will close the main screen as long as no popup windows are open.")]
  private int _screenCloseAction = -1;
  [SerializeField]
  [Tooltip("An Action that when activated will cancel and close any open popup window. Use with care because the element assigned to this Action can never be mapped by the user (because it would just cancel his assignment).")]
  private int _universalCancelAction = -1;
  [SerializeField]
  [Tooltip("If enabled, Universal Cancel will also close the main screen if pressed when no windows are open.")]
  private bool _universalCancelClosesScreen = true;
  [SerializeField]
  [Tooltip("If checked, controls will be displayed which will allow the user to customize certain Input Behavior settings.")]
  private bool _showInputBehaviorSettings;
  [SerializeField]
  [Tooltip("Customizable settings for user-modifiable Input Behaviors. This can be used for settings like Mouse Look Sensitivity.")]
  private Rewired.UI.ControlMapper.ControlMapper.InputBehaviorSettings[] _inputBehaviorSettings;
  [SerializeField]
  [Tooltip("If enabled, UI elements will be themed based on the settings in Theme Settings.")]
  private bool _useThemeSettings = true;
  [SerializeField]
  [Tooltip("Must be assigned a ThemeSettings object. Used to theme UI elements.")]
  private ThemeSettings _themeSettings;
  [SerializeField]
  [Tooltip("Must be assigned a LanguageData object. Used to retrieve language entries for UI elements.")]
  private LanguageDataBase _language;
  [SerializeField]
  [Tooltip("A list of prefabs. You should not have to modify this.")]
  private Rewired.UI.ControlMapper.ControlMapper.Prefabs prefabs;
  [SerializeField]
  [Tooltip("A list of references to elements in the hierarchy. You should not have to modify this.")]
  private Rewired.UI.ControlMapper.ControlMapper.References references;
  [SerializeField]
  [Tooltip("Show the label for the Players button group?")]
  private bool _showPlayersGroupLabel = true;
  [SerializeField]
  [Tooltip("Show the label for the Controller button group?")]
  private bool _showControllerGroupLabel = true;
  [SerializeField]
  [Tooltip("Show the label for the Assigned Controllers button group?")]
  private bool _showAssignedControllersGroupLabel = true;
  [SerializeField]
  [Tooltip("Show the label for the Settings button group?")]
  private bool _showSettingsGroupLabel = true;
  [SerializeField]
  [Tooltip("Show the label for the Map Categories button group?")]
  private bool _showMapCategoriesGroupLabel = true;
  [SerializeField]
  [Tooltip("Show the label for the current controller name?")]
  private bool _showControllerNameLabel = true;
  [SerializeField]
  [Tooltip("Show the Assigned Controllers group? If joystick auto-assignment is enabled in the Rewired Input Manager and the max joysticks per player is set to any value other than 1, the Assigned Controllers group will always be displayed.")]
  private bool _showAssignedControllers = true;
  private System.Action _ScreenClosedEvent;
  private System.Action _ScreenOpenedEvent;
  private System.Action _PopupWindowOpenedEvent;
  private System.Action _PopupWindowClosedEvent;
  private System.Action _InputPollingStartedEvent;
  private System.Action _InputPollingEndedEvent;
  [SerializeField]
  [Tooltip("Event sent when the UI is closed.")]
  private UnityEvent _onScreenClosed;
  [SerializeField]
  [Tooltip("Event sent when the UI is opened.")]
  private UnityEvent _onScreenOpened;
  [SerializeField]
  [Tooltip("Event sent when a popup window is closed.")]
  private UnityEvent _onPopupWindowClosed;
  [SerializeField]
  [Tooltip("Event sent when a popup window is opened.")]
  private UnityEvent _onPopupWindowOpened;
  [SerializeField]
  [Tooltip("Event sent when polling for input has started.")]
  private UnityEvent _onInputPollingStarted;
  [SerializeField]
  [Tooltip("Event sent when polling for input has ended.")]
  private UnityEvent _onInputPollingEnded;
  private static Rewired.UI.ControlMapper.ControlMapper Instance;
  private bool initialized;
  private int playerCount;
  private Rewired.UI.ControlMapper.ControlMapper.InputGrid inputGrid;
  private Rewired.UI.ControlMapper.ControlMapper.WindowManager windowManager;
  private int currentPlayerId;
  private int currentMapCategoryId;
  private List<Rewired.UI.ControlMapper.ControlMapper.GUIButton> playerButtons;
  private List<Rewired.UI.ControlMapper.ControlMapper.GUIButton> mapCategoryButtons;
  private List<Rewired.UI.ControlMapper.ControlMapper.GUIButton> assignedControllerButtons;
  private Rewired.UI.ControlMapper.ControlMapper.GUIButton assignedControllerButtonsPlaceholder;
  private List<GameObject> miscInstantiatedObjects;
  private GameObject canvas;
  private GameObject lastUISelection;
  private int currentJoystickId = -1;
  private float blockInputOnFocusEndTime;
  private bool isPollingForInput;
  private Rewired.UI.ControlMapper.ControlMapper.InputMapping pendingInputMapping;
  private Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator pendingAxisCalibration;
  private Action<InputFieldInfo> inputFieldActivatedDelegate;
  private Action<ToggleInfo, bool> inputFieldInvertToggleStateChangedDelegate;
  private System.Action _restoreDefaultsDelegate;

  public event System.Action ScreenClosedEvent
  {
    add => this._ScreenClosedEvent += value;
    remove => this._ScreenClosedEvent -= value;
  }

  public event System.Action ScreenOpenedEvent
  {
    add => this._ScreenOpenedEvent += value;
    remove => this._ScreenOpenedEvent -= value;
  }

  public event System.Action PopupWindowClosedEvent
  {
    add => this._PopupWindowClosedEvent += value;
    remove => this._PopupWindowClosedEvent -= value;
  }

  public event System.Action PopupWindowOpenedEvent
  {
    add => this._PopupWindowOpenedEvent += value;
    remove => this._PopupWindowOpenedEvent -= value;
  }

  public event System.Action InputPollingStartedEvent
  {
    add => this._InputPollingStartedEvent += value;
    remove => this._InputPollingStartedEvent -= value;
  }

  public event System.Action InputPollingEndedEvent
  {
    add => this._InputPollingEndedEvent += value;
    remove => this._InputPollingEndedEvent -= value;
  }

  public event UnityAction onScreenClosed
  {
    add => this._onScreenClosed.AddListener(value);
    remove => this._onScreenClosed.RemoveListener(value);
  }

  public event UnityAction onScreenOpened
  {
    add => this._onScreenOpened.AddListener(value);
    remove => this._onScreenOpened.RemoveListener(value);
  }

  public event UnityAction onPopupWindowClosed
  {
    add => this._onPopupWindowClosed.AddListener(value);
    remove => this._onPopupWindowClosed.RemoveListener(value);
  }

  public event UnityAction onPopupWindowOpened
  {
    add => this._onPopupWindowOpened.AddListener(value);
    remove => this._onPopupWindowOpened.RemoveListener(value);
  }

  public event UnityAction onInputPollingStarted
  {
    add => this._onInputPollingStarted.AddListener(value);
    remove => this._onInputPollingStarted.RemoveListener(value);
  }

  public event UnityAction onInputPollingEnded
  {
    add => this._onInputPollingEnded.AddListener(value);
    remove => this._onInputPollingEnded.RemoveListener(value);
  }

  public Rewired.InputManager rewiredInputManager
  {
    get => this._rewiredInputManager;
    set
    {
      this._rewiredInputManager = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool dontDestroyOnLoad
  {
    get => this._dontDestroyOnLoad;
    set
    {
      if (value != this._dontDestroyOnLoad && value)
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.transform.gameObject);
      this._dontDestroyOnLoad = value;
    }
  }

  public int keyboardMapDefaultLayout
  {
    get => this._keyboardMapDefaultLayout;
    set
    {
      this._keyboardMapDefaultLayout = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int mouseMapDefaultLayout
  {
    get => this._mouseMapDefaultLayout;
    set
    {
      this._mouseMapDefaultLayout = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int joystickMapDefaultLayout
  {
    get => this._joystickMapDefaultLayout;
    set
    {
      this._joystickMapDefaultLayout = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showPlayers
  {
    get => this._showPlayers && ReInput.players.playerCount > 1;
    set
    {
      this._showPlayers = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showControllers
  {
    get => this._showControllers;
    set
    {
      this._showControllers = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showKeyboard
  {
    get => this._showKeyboard;
    set
    {
      this._showKeyboard = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showMouse
  {
    get => this._showMouse;
    set
    {
      this._showMouse = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int maxControllersPerPlayer
  {
    get => this._maxControllersPerPlayer;
    set
    {
      this._maxControllersPerPlayer = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showActionCategoryLabels
  {
    get => this._showActionCategoryLabels;
    set
    {
      this._showActionCategoryLabels = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int keyboardInputFieldCount
  {
    get => this._keyboardInputFieldCount;
    set
    {
      this._keyboardInputFieldCount = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int mouseInputFieldCount
  {
    get => this._mouseInputFieldCount;
    set
    {
      this._mouseInputFieldCount = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int controllerInputFieldCount
  {
    get => this._controllerInputFieldCount;
    set
    {
      this._controllerInputFieldCount = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showFullAxisInputFields
  {
    get => this._showFullAxisInputFields;
    set
    {
      this._showFullAxisInputFields = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showSplitAxisInputFields
  {
    get => this._showSplitAxisInputFields;
    set
    {
      this._showSplitAxisInputFields = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool allowElementAssignmentConflicts
  {
    get => this._allowElementAssignmentConflicts;
    set
    {
      this._allowElementAssignmentConflicts = value;
      this.InspectorPropertyChanged();
    }
  }

  public bool allowElementAssignmentSwap
  {
    get => this._allowElementAssignmentSwap;
    set
    {
      this._allowElementAssignmentSwap = value;
      this.InspectorPropertyChanged();
    }
  }

  public int actionLabelWidth
  {
    get => this._actionLabelWidth;
    set
    {
      this._actionLabelWidth = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int keyboardColMaxWidth
  {
    get => this._keyboardColMaxWidth;
    set
    {
      this._keyboardColMaxWidth = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int mouseColMaxWidth
  {
    get => this._mouseColMaxWidth;
    set
    {
      this._mouseColMaxWidth = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int controllerColMaxWidth
  {
    get => this._controllerColMaxWidth;
    set
    {
      this._controllerColMaxWidth = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int inputRowHeight
  {
    get => this._inputRowHeight;
    set
    {
      this._inputRowHeight = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int inputColumnSpacing
  {
    get => this._inputColumnSpacing;
    set
    {
      this._inputColumnSpacing = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int inputRowCategorySpacing
  {
    get => this._inputRowCategorySpacing;
    set
    {
      this._inputRowCategorySpacing = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int invertToggleWidth
  {
    get => this._invertToggleWidth;
    set
    {
      this._invertToggleWidth = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int defaultWindowWidth
  {
    get => this._defaultWindowWidth;
    set
    {
      this._defaultWindowWidth = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public int defaultWindowHeight
  {
    get => this._defaultWindowHeight;
    set
    {
      this._defaultWindowHeight = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public float controllerAssignmentTimeout
  {
    get => this._controllerAssignmentTimeout;
    set
    {
      this._controllerAssignmentTimeout = value;
      this.InspectorPropertyChanged();
    }
  }

  public float preInputAssignmentTimeout
  {
    get => this._preInputAssignmentTimeout;
    set
    {
      this._preInputAssignmentTimeout = value;
      this.InspectorPropertyChanged();
    }
  }

  public float inputAssignmentTimeout
  {
    get => this._inputAssignmentTimeout;
    set
    {
      this._inputAssignmentTimeout = value;
      this.InspectorPropertyChanged();
    }
  }

  public float axisCalibrationTimeout
  {
    get => this._axisCalibrationTimeout;
    set
    {
      this._axisCalibrationTimeout = value;
      this.InspectorPropertyChanged();
    }
  }

  public bool ignoreMouseXAxisAssignment
  {
    get => this._ignoreMouseXAxisAssignment;
    set
    {
      this._ignoreMouseXAxisAssignment = value;
      this.InspectorPropertyChanged();
    }
  }

  public bool ignoreMouseYAxisAssignment
  {
    get => this._ignoreMouseYAxisAssignment;
    set
    {
      this._ignoreMouseYAxisAssignment = value;
      this.InspectorPropertyChanged();
    }
  }

  public bool universalCancelClosesScreen
  {
    get => this._universalCancelClosesScreen;
    set
    {
      this._universalCancelClosesScreen = value;
      this.InspectorPropertyChanged();
    }
  }

  public bool showInputBehaviorSettings
  {
    get => this._showInputBehaviorSettings;
    set
    {
      this._showInputBehaviorSettings = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool useThemeSettings
  {
    get => this._useThemeSettings;
    set
    {
      this._useThemeSettings = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public LanguageDataBase language
  {
    get => this._language;
    set
    {
      this._language = value;
      if ((UnityEngine.Object) this._language != (UnityEngine.Object) null)
        this._language.Initialize();
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showPlayersGroupLabel
  {
    get => this._showPlayersGroupLabel;
    set
    {
      this._showPlayersGroupLabel = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showControllerGroupLabel
  {
    get => this._showControllerGroupLabel;
    set
    {
      this._showControllerGroupLabel = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showAssignedControllersGroupLabel
  {
    get => this._showAssignedControllersGroupLabel;
    set
    {
      this._showAssignedControllersGroupLabel = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showSettingsGroupLabel
  {
    get => this._showSettingsGroupLabel;
    set
    {
      this._showSettingsGroupLabel = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showMapCategoriesGroupLabel
  {
    get => this._showMapCategoriesGroupLabel;
    set
    {
      this._showMapCategoriesGroupLabel = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showControllerNameLabel
  {
    get => this._showControllerNameLabel;
    set
    {
      this._showControllerNameLabel = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public bool showAssignedControllers
  {
    get => this._showAssignedControllers;
    set
    {
      this._showAssignedControllers = value;
      this.InspectorPropertyChanged(true);
    }
  }

  public System.Action restoreDefaultsDelegate
  {
    get => this._restoreDefaultsDelegate;
    set => this._restoreDefaultsDelegate = value;
  }

  public bool isOpen
  {
    get
    {
      if (this.initialized)
        return this.canvas.activeInHierarchy;
      return (UnityEngine.Object) this.references.canvas != (UnityEngine.Object) null && this.references.canvas.gameObject.activeInHierarchy;
    }
  }

  private bool isFocused => this.initialized && !this.windowManager.isWindowOpen;

  private bool inputAllowed => (double) this.blockInputOnFocusEndTime <= (double) Time.unscaledTime;

  private int inputGridColumnCount
  {
    get
    {
      int inputGridColumnCount = 1;
      if (this._showKeyboard)
        ++inputGridColumnCount;
      if (this._showMouse)
        ++inputGridColumnCount;
      if (this._showControllers)
        ++inputGridColumnCount;
      return inputGridColumnCount;
    }
  }

  private int inputGridWidth
  {
    get
    {
      return this._actionLabelWidth + (this._showKeyboard ? this._keyboardColMaxWidth : 0) + (this._showMouse ? this._mouseColMaxWidth : 0) + (this._showControllers ? this._controllerColMaxWidth : 0) + (this.inputGridColumnCount - 1) * this._inputColumnSpacing;
    }
  }

  private Player currentPlayer => ReInput.players.GetPlayer(this.currentPlayerId);

  private InputCategory currentMapCategory
  {
    get => (InputCategory) ReInput.mapping.GetMapCategory(this.currentMapCategoryId);
  }

  private Rewired.UI.ControlMapper.ControlMapper.MappingSet currentMappingSet
  {
    get
    {
      if (this.currentMapCategoryId < 0)
        return (Rewired.UI.ControlMapper.ControlMapper.MappingSet) null;
      for (int index = 0; index < this._mappingSets.Length; ++index)
      {
        if (this._mappingSets[index].mapCategoryId == this.currentMapCategoryId)
          return this._mappingSets[index];
      }
      return (Rewired.UI.ControlMapper.ControlMapper.MappingSet) null;
    }
  }

  private Joystick currentJoystick => ReInput.controllers.GetJoystick(this.currentJoystickId);

  private bool isJoystickSelected => this.currentJoystickId >= 0;

  private GameObject currentUISelection
  {
    get
    {
      return !((UnityEngine.Object) EventSystem.current != (UnityEngine.Object) null) ? (GameObject) null : EventSystem.current.currentSelectedGameObject;
    }
  }

  private bool showSettings
  {
    get => this._showInputBehaviorSettings && this._inputBehaviorSettings.Length != 0;
  }

  private bool showMapCategories => this._mappingSets != null && this._mappingSets.Length > 1;

  private void Awake()
  {
    if (this._dontDestroyOnLoad)
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.transform.gameObject);
    this.PreInitialize();
    if (!this.isOpen)
      return;
    this.Initialize();
    this.Open(true);
  }

  private void Start()
  {
    if (!this._openOnStart)
      return;
    this.Open(false);
  }

  private void Update()
  {
    if (!this.isOpen || !this.initialized)
      return;
    this.CheckUISelection();
  }

  private void OnDestroy()
  {
    ReInput.ControllerConnectedEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnJoystickConnected);
    ReInput.ControllerDisconnectedEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnJoystickDisconnected);
    ReInput.ControllerPreDisconnectEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnJoystickPreDisconnect);
    this.UnsubscribeMenuControlInputEvents();
  }

  private void PreInitialize()
  {
    if (!ReInput.isReady)
      Debug.LogError((object) "Rewired Control Mapper: Rewired has not been initialized! Are you missing a Rewired Input Manager in your scene?");
    else
      this.SubscribeMenuControlInputEvents();
  }

  private void Initialize()
  {
    if (this.initialized || !ReInput.isReady)
      return;
    if ((UnityEngine.Object) this._rewiredInputManager == (UnityEngine.Object) null)
    {
      this._rewiredInputManager = UnityEngine.Object.FindObjectOfType<Rewired.InputManager>();
      if ((UnityEngine.Object) this._rewiredInputManager == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: A Rewired Input Manager was not assigned in the inspector or found in the current scene! Control Mapper will not function.");
        return;
      }
    }
    if ((UnityEngine.Object) Rewired.UI.ControlMapper.ControlMapper.Instance != (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Rewired Control Mapper: Only one ControlMapper can exist at one time!");
    }
    else
    {
      Rewired.UI.ControlMapper.ControlMapper.Instance = this;
      if (this.prefabs == null || !this.prefabs.Check())
        Debug.LogError((object) "Rewired Control Mapper: All prefabs must be assigned in the inspector!");
      else if (this.references == null || !this.references.Check())
      {
        Debug.LogError((object) "Rewired Control Mapper: All references must be assigned in the inspector!");
      }
      else
      {
        this.references.inputGridLayoutElement = this.references.inputGridContainer.GetComponent<LayoutElement>();
        if ((UnityEngine.Object) this.references.inputGridLayoutElement == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "Rewired Control Mapper: InputGridContainer is missing LayoutElement component!");
        }
        else
        {
          if (this._showKeyboard && this._keyboardInputFieldCount < 1)
          {
            Debug.LogWarning((object) "Rewired Control Mapper: Keyboard Input Fields must be at least 1!");
            this._keyboardInputFieldCount = 1;
          }
          if (this._showMouse && this._mouseInputFieldCount < 1)
          {
            Debug.LogWarning((object) "Rewired Control Mapper: Mouse Input Fields must be at least 1!");
            this._mouseInputFieldCount = 1;
          }
          if (this._showControllers && this._controllerInputFieldCount < 1)
          {
            Debug.LogWarning((object) "Rewired Control Mapper: Controller Input Fields must be at least 1!");
            this._controllerInputFieldCount = 1;
          }
          if (this._maxControllersPerPlayer < 0)
          {
            Debug.LogWarning((object) "Rewired Control Mapper: Max Controllers Per Player must be at least 0 (no limit)!");
            this._maxControllersPerPlayer = 0;
          }
          if (this._useThemeSettings && (UnityEngine.Object) this._themeSettings == (UnityEngine.Object) null)
          {
            Debug.LogWarning((object) "Rewired Control Mapper: To use theming, Theme Settings must be set in the inspector! Theming has been disabled.");
            this._useThemeSettings = false;
          }
          if ((UnityEngine.Object) this._language == (UnityEngine.Object) null)
          {
            Debug.LogError((object) "Rawired UI: Language must be set in the inspector!");
          }
          else
          {
            this._language.Initialize();
            this.inputFieldActivatedDelegate = new Action<InputFieldInfo>(this.OnInputFieldActivated);
            this.inputFieldInvertToggleStateChangedDelegate = new Action<ToggleInfo, bool>(this.OnInputFieldInvertToggleStateChanged);
            ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnJoystickConnected);
            ReInput.ControllerDisconnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnJoystickDisconnected);
            ReInput.ControllerPreDisconnectEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnJoystickPreDisconnect);
            this.playerCount = ReInput.players.playerCount;
            this.canvas = this.references.canvas.gameObject;
            this.windowManager = new Rewired.UI.ControlMapper.ControlMapper.WindowManager(this.prefabs.window, this.prefabs.fader, this.references.canvas.transform);
            this.playerButtons = new List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>();
            this.mapCategoryButtons = new List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>();
            this.assignedControllerButtons = new List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>();
            this.miscInstantiatedObjects = new List<GameObject>();
            this.currentMapCategoryId = this._mappingSets[0].mapCategoryId;
            this.Draw();
            this.CreateInputGrid();
            this.CreateLayout();
            this.SubscribeFixedUISelectionEvents();
            this.initialized = true;
          }
        }
      }
    }
  }

  private void OnJoystickConnected(ControllerStatusChangedEventArgs args)
  {
    if (!this.initialized || !this._showControllers)
      return;
    this.ClearVarsOnJoystickChange();
    this.ForceRefresh();
  }

  private void OnJoystickDisconnected(ControllerStatusChangedEventArgs args)
  {
    if (!this.initialized || !this._showControllers)
      return;
    this.ClearVarsOnJoystickChange();
    this.ForceRefresh();
  }

  private void OnJoystickPreDisconnect(ControllerStatusChangedEventArgs args)
  {
    if (!this.initialized)
      return;
    int num = this._showControllers ? 1 : 0;
  }

  public void OnButtonActivated(ButtonInfo buttonInfo)
  {
    if (!this.initialized || !this.inputAllowed)
      return;
    string identifier = buttonInfo.identifier;
    // ISSUE: reference to a compiler-generated method
    switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(identifier))
    {
      case 36291085:
        if (!(identifier == "MapCategorySelection"))
          break;
        this.OnMapCategorySelected(buttonInfo.intData, true);
        break;
      case 1293854844:
        if (!(identifier == "AssignController"))
          break;
        this.ShowAssignControllerWindow();
        break;
      case 1619204974:
        if (!(identifier == "PlayerSelection"))
          break;
        this.OnPlayerSelected(buttonInfo.intData, true);
        break;
      case 1656078790:
        if (!(identifier == "EditInputBehaviors"))
          break;
        this.ShowEditInputBehaviorsWindow();
        break;
      case 2139278426:
        if (!(identifier == "CalibrateController"))
          break;
        this.ShowCalibrateControllerWindow();
        break;
      case 2379421585:
        if (!(identifier == "Done"))
          break;
        this.Close(true);
        break;
      case 2857234147:
        if (!(identifier == "RestoreDefaults"))
          break;
        this.OnRestoreDefaults();
        break;
      case 3019194153:
        if (!(identifier == "RemoveController"))
          break;
        this.OnRemoveCurrentController();
        break;
      case 3496297297:
        if (!(identifier == "AssignedControllerSelection"))
          break;
        this.OnControllerSelected(buttonInfo.intData);
        break;
    }
  }

  public void OnInputFieldActivated(InputFieldInfo fieldInfo)
  {
    if (!this.initialized || !this.inputAllowed || this.currentPlayer == null)
      return;
    InputAction action = ReInput.mapping.GetAction(fieldInfo.actionId);
    if (action == null)
      return;
    AxisRange axisRange = action.type == InputActionType.Axis ? fieldInfo.axisRange : AxisRange.Full;
    string actionName = this._language.GetActionName(action.id, axisRange);
    ControllerMap controllerMap = this.GetControllerMap(fieldInfo.controllerType);
    if (controllerMap == null)
      return;
    ActionElementMap elementMap = fieldInfo.actionElementMapId >= 0 ? controllerMap.GetElementMap(fieldInfo.actionElementMapId) : (ActionElementMap) null;
    if (elementMap != null)
      this.ShowBeginElementAssignmentReplacementWindow(fieldInfo, action, controllerMap, elementMap, actionName);
    else
      this.ShowCreateNewElementAssignmentWindow(fieldInfo, action, controllerMap, actionName);
  }

  public void OnInputFieldInvertToggleStateChanged(ToggleInfo toggleInfo, bool newState)
  {
    if (!this.initialized || !this.inputAllowed)
      return;
    this.SetActionAxisInverted(newState, toggleInfo.controllerType, toggleInfo.actionElementMapId);
  }

  private void OnPlayerSelected(int playerId, bool redraw)
  {
    if (!this.initialized)
      return;
    this.currentPlayerId = playerId;
    this.ClearVarsOnPlayerChange();
    if (!redraw)
      return;
    this.Redraw(true, true);
  }

  private void OnControllerSelected(int joystickId)
  {
    if (!this.initialized)
      return;
    this.currentJoystickId = joystickId;
    this.Redraw(true, true);
  }

  private void OnRemoveCurrentController()
  {
    if (this.currentPlayer == null || this.currentJoystickId < 0)
      return;
    this.RemoveController(this.currentPlayer, this.currentJoystickId);
    this.ClearVarsOnJoystickChange();
    this.Redraw(false, false);
  }

  private void OnMapCategorySelected(int id, bool redraw)
  {
    if (!this.initialized)
      return;
    this.currentMapCategoryId = id;
    if (!redraw)
      return;
    this.Redraw(true, true);
  }

  private void OnRestoreDefaults()
  {
    if (!this.initialized)
      return;
    this.ShowRestoreDefaultsWindow();
  }

  private void OnScreenToggleActionPressed(InputActionEventData data)
  {
    if (!this.isOpen)
    {
      this.Open();
    }
    else
    {
      if (!this.initialized || !this.isFocused)
        return;
      this.Close(true);
    }
  }

  private void OnScreenOpenActionPressed(InputActionEventData data) => this.Open();

  private void OnScreenCloseActionPressed(InputActionEventData data)
  {
    if (!this.initialized || !this.isOpen || !this.isFocused)
      return;
    this.Close(true);
  }

  private void OnUniversalCancelActionPressed(InputActionEventData data)
  {
    if (!this.initialized || !this.isOpen)
      return;
    if (this._universalCancelClosesScreen)
    {
      if (this.isFocused)
      {
        this.Close(true);
        return;
      }
    }
    else if (this.isFocused)
      return;
    this.CloseAllWindows();
  }

  private void OnWindowCancel(int windowId)
  {
    if (!this.initialized || windowId < 0)
      return;
    this.CloseWindow(windowId);
  }

  private void OnRemoveElementAssignment(int windowId, ControllerMap map, ActionElementMap aem)
  {
    if (map == null || aem == null)
      return;
    map.DeleteElementMap(aem.id);
    this.CloseWindow(windowId);
  }

  private void OnBeginElementAssignment(
    InputFieldInfo fieldInfo,
    ControllerMap map,
    ActionElementMap aem,
    string actionName)
  {
    if ((UnityEngine.Object) fieldInfo == (UnityEngine.Object) null || map == null)
      return;
    this.pendingInputMapping = new Rewired.UI.ControlMapper.ControlMapper.InputMapping(actionName, fieldInfo, map, aem, fieldInfo.controllerType, fieldInfo.controllerId);
    switch (fieldInfo.controllerType)
    {
      case ControllerType.Keyboard:
        this.ShowElementAssignmentPollingWindow();
        break;
      case ControllerType.Mouse:
        this.ShowElementAssignmentPollingWindow();
        break;
      case ControllerType.Joystick:
        this.ShowElementAssignmentPrePollingWindow();
        break;
      default:
        throw new NotImplementedException();
    }
  }

  private void OnControllerAssignmentConfirmed(int windowId, Player player, int controllerId)
  {
    if (windowId < 0 || player == null || controllerId < 0)
      return;
    this.AssignController(player, controllerId);
    this.CloseWindow(windowId);
  }

  private void OnMouseAssignmentConfirmed(int windowId, Player player)
  {
    if (windowId < 0 || player == null)
      return;
    IList<Player> players = (IList<Player>) ReInput.players.Players;
    for (int index = 0; index < players.Count; ++index)
    {
      if (players[index] != player)
        players[index].controllers.hasMouse = false;
    }
    player.controllers.hasMouse = true;
    this.CloseWindow(windowId);
  }

  private void OnElementAssignmentConflictReplaceConfirmed(
    int windowId,
    Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
    ElementAssignment assignment,
    bool skipOtherPlayers,
    bool allowSwap)
  {
    if (this.currentPlayer == null || mapping == null)
      return;
    ElementAssignmentConflictCheck conflictCheck;
    if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
    {
      Debug.LogError((object) "Rewired Control Mapper: Error creating conflict check!");
      this.CloseWindow(windowId);
    }
    else
    {
      ElementAssignmentConflictInfo conflict = new ElementAssignmentConflictInfo();
      ActionElementMap actionElementMap1 = (ActionElementMap) null;
      ActionElementMap actionElementMap2 = (ActionElementMap) null;
      bool flag = false;
      if (allowSwap && mapping.aem != null && this.GetFirstElementAssignmentConflict(conflictCheck, out conflict, skipOtherPlayers))
      {
        flag = true;
        actionElementMap2 = new ActionElementMap(mapping.aem);
        actionElementMap1 = new ActionElementMap(conflict.elementMap);
      }
      IList<Player> allPlayers = (IList<Player>) ReInput.players.AllPlayers;
      for (int index = 0; index < allPlayers.Count; ++index)
      {
        Player player = allPlayers[index];
        if (!skipOtherPlayers || player == this.currentPlayer || player == ReInput.players.SystemPlayer)
          player.controllers.conflictChecking.RemoveElementAssignmentConflicts(conflictCheck);
      }
      mapping.map.ReplaceOrCreateElementMap(assignment);
      if (allowSwap & flag)
      {
        int actionId = actionElementMap1.actionId;
        Pole axisContribution = actionElementMap1.axisContribution;
        bool invert = actionElementMap1.invert;
        AxisRange axisRange = actionElementMap2.axisRange;
        ControllerElementType elementType = actionElementMap2.elementType;
        int elementIdentifierId = actionElementMap2.elementIdentifierId;
        KeyCode keyCode = actionElementMap2.keyCode;
        ModifierKeyFlags modifierKeyFlags = actionElementMap2.modifierKeyFlags;
        if (elementType == actionElementMap1.elementType && elementType == ControllerElementType.Axis)
        {
          if (axisRange != actionElementMap1.axisRange)
          {
            if (axisRange == AxisRange.Full)
              axisRange = AxisRange.Positive;
            else if (actionElementMap1.axisRange != AxisRange.Full)
              ;
          }
        }
        else if (elementType == ControllerElementType.Axis && (actionElementMap1.elementType == ControllerElementType.Button || actionElementMap1.elementType == ControllerElementType.Axis && actionElementMap1.axisRange != AxisRange.Full) && axisRange == AxisRange.Full)
          axisRange = AxisRange.Positive;
        if (elementType != ControllerElementType.Axis || axisRange != AxisRange.Full)
          invert = false;
        int num = 0;
        foreach (ActionElementMap actionElementMap3 in (IEnumerable<ActionElementMap>) conflict.controllerMap.ElementMapsWithAction(actionId))
        {
          if (this.SwapIsSameInputRange(elementType, axisRange, axisContribution, actionElementMap3.elementType, actionElementMap3.axisRange, actionElementMap3.axisContribution))
            ++num;
        }
        if (num < this.GetControllerInputFieldCount(mapping.controllerType))
          conflict.controllerMap.ReplaceOrCreateElementMap(ElementAssignment.CompleteAssignment(mapping.controllerType, elementType, elementIdentifierId, axisRange, keyCode, modifierKeyFlags, actionId, axisContribution, invert));
      }
      this.CloseWindow(windowId);
    }
  }

  private void OnElementAssignmentAddConfirmed(
    int windowId,
    Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
    ElementAssignment assignment)
  {
    if (this.currentPlayer == null || mapping == null)
      return;
    mapping.map.ReplaceOrCreateElementMap(assignment);
    this.CloseWindow(windowId);
  }

  private void OnRestoreDefaultsConfirmed(int windowId)
  {
    if (this._restoreDefaultsDelegate == null)
    {
      IList<Player> players = (IList<Player>) ReInput.players.Players;
      for (int index = 0; index < players.Count; ++index)
      {
        Player player = players[index];
        if (this._showControllers)
          player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
        if (this._showKeyboard)
          player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
        if (this._showMouse)
          player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
      }
    }
    this.CloseWindow(windowId);
    if (this._restoreDefaultsDelegate == null)
      return;
    this._restoreDefaultsDelegate();
  }

  private void OnAssignControllerWindowUpdate(int windowId)
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.windowManager.GetWindow(windowId);
    if (windowId < 0)
      return;
    this.InputPollingStarted();
    if (window.timer.finished)
    {
      this.InputPollingStopped();
      this.CloseWindow(windowId);
    }
    else
    {
      ControllerPollingInfo controllerPollingInfo = ReInput.controllers.polling.PollAllControllersOfTypeForFirstElementDown(ControllerType.Joystick);
      if (controllerPollingInfo.success)
      {
        this.InputPollingStopped();
        if (ReInput.controllers.IsControllerAssigned(ControllerType.Joystick, controllerPollingInfo.controllerId) && !this.currentPlayer.controllers.ContainsController(ControllerType.Joystick, controllerPollingInfo.controllerId))
          this.ShowControllerAssignmentConflictWindow(controllerPollingInfo.controllerId);
        else
          this.OnControllerAssignmentConfirmed(windowId, this.currentPlayer, controllerPollingInfo.controllerId);
      }
      else
        window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
    }
  }

  private void OnElementAssignmentPrePollingWindowUpdate(int windowId)
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.windowManager.GetWindow(windowId);
    if (windowId < 0 || this.pendingInputMapping == null)
      return;
    this.InputPollingStarted();
    if (!window.timer.finished)
    {
      window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
      ControllerPollingInfo controllerPollingInfo;
      switch (this.pendingInputMapping.controllerType)
      {
        case ControllerType.Keyboard:
        case ControllerType.Mouse:
          controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, 0);
          break;
        case ControllerType.Joystick:
          if (this.currentPlayer.controllers.joystickCount == 0)
            return;
          controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, this.currentJoystick.id);
          break;
        default:
          throw new NotImplementedException();
      }
      if (!controllerPollingInfo.success)
        return;
    }
    this.ShowElementAssignmentPollingWindow();
  }

  private void OnJoystickElementAssignmentPollingWindowUpdate(int windowId)
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.windowManager.GetWindow(windowId);
    if (windowId < 0 || this.pendingInputMapping == null)
      return;
    this.InputPollingStarted();
    if (window.timer.finished)
    {
      this.InputPollingStopped();
      this.CloseWindow(windowId);
    }
    else
    {
      window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
      if (this.currentPlayer.controllers.joystickCount == 0)
        return;
      ControllerPollingInfo pollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Joystick, this.currentJoystick.id);
      if (!pollingInfo.success || !this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
        return;
      ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
      if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
      {
        this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
        this.InputPollingStopped();
        this.CloseWindow(windowId);
      }
      else
      {
        this.InputPollingStopped();
        this.ShowElementAssignmentConflictWindow(elementAssignment, false);
      }
    }
  }

  private void OnKeyboardElementAssignmentPollingWindowUpdate(int windowId)
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.windowManager.GetWindow(windowId);
    if (windowId < 0 || this.pendingInputMapping == null)
      return;
    this.InputPollingStarted();
    if (window.timer.finished)
    {
      this.InputPollingStopped();
      this.CloseWindow(windowId);
    }
    else
    {
      ControllerPollingInfo pollingInfo;
      bool modifierKeyPressed;
      ModifierKeyFlags modifierFlags;
      string label;
      this.PollKeyboardForAssignment(out pollingInfo, out modifierKeyPressed, out modifierFlags, out label);
      if (modifierKeyPressed)
        window.timer.Start(this._inputAssignmentTimeout);
      window.SetContentText(modifierKeyPressed ? string.Empty : Mathf.CeilToInt(window.timer.remaining).ToString(), 2);
      window.SetContentText(label, 1);
      if (!pollingInfo.success || !this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
        return;
      ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo, modifierFlags);
      if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
      {
        this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
        this.InputPollingStopped();
        this.CloseWindow(windowId);
      }
      else
      {
        this.InputPollingStopped();
        this.ShowElementAssignmentConflictWindow(elementAssignment, false);
      }
    }
  }

  private void OnMouseElementAssignmentPollingWindowUpdate(int windowId)
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.windowManager.GetWindow(windowId);
    if (windowId < 0 || this.pendingInputMapping == null)
      return;
    this.InputPollingStarted();
    if (window.timer.finished)
    {
      this.InputPollingStopped();
      this.CloseWindow(windowId);
    }
    else
    {
      window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
      ControllerPollingInfo pollingInfo;
      if (this._ignoreMouseXAxisAssignment || this._ignoreMouseYAxisAssignment)
      {
        pollingInfo = new ControllerPollingInfo();
        foreach (ControllerPollingInfo controllerPollingInfo in (IEnumerable<ControllerPollingInfo>) ReInput.controllers.polling.PollControllerForAllElementsDown(ControllerType.Mouse, 0))
        {
          if (controllerPollingInfo.elementType != ControllerElementType.Axis || (!this._ignoreMouseXAxisAssignment || controllerPollingInfo.elementIndex != 0) && (!this._ignoreMouseYAxisAssignment || controllerPollingInfo.elementIndex != 1))
          {
            pollingInfo = controllerPollingInfo;
            break;
          }
        }
      }
      else
        pollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Mouse, 0);
      if (!pollingInfo.success || !this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
        return;
      ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
      if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, true))
      {
        this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
        this.InputPollingStopped();
        this.CloseWindow(windowId);
      }
      else
      {
        this.InputPollingStopped();
        this.ShowElementAssignmentConflictWindow(elementAssignment, true);
      }
    }
  }

  private void OnCalibrateAxisStep1WindowUpdate(int windowId)
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.windowManager.GetWindow(windowId);
    if (windowId < 0 || this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
      return;
    this.InputPollingStarted();
    if (!window.timer.finished)
    {
      window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
      if (this.currentPlayer.controllers.joystickCount == 0 || !this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
        return;
    }
    this.pendingAxisCalibration.RecordZero();
    this.CloseWindow(windowId);
    this.ShowCalibrateAxisStep2Window();
  }

  private void OnCalibrateAxisStep2WindowUpdate(int windowId)
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.windowManager.GetWindow(windowId);
    if (windowId < 0 || this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
      return;
    if (!window.timer.finished)
    {
      window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
      this.pendingAxisCalibration.RecordMinMax();
      if (this.currentPlayer.controllers.joystickCount == 0 || !this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
        return;
    }
    this.EndAxisCalibration();
    this.InputPollingStopped();
    this.CloseWindow(windowId);
  }

  private void ShowAssignControllerWindow()
  {
    if (this.currentPlayer == null || ReInput.controllers.joystickCount == 0)
      return;
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    window.SetUpdateCallback(new Action<int>(this.OnAssignControllerWindowUpdate));
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.assignControllerWindowTitle);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), this._language.assignControllerWindowMessage);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
    window.timer.Start(this._controllerAssignmentTimeout);
    this.windowManager.Focus(window);
  }

  private void ShowControllerAssignmentConflictWindow(int controllerId)
  {
    if (this.currentPlayer == null || ReInput.controllers.joystickCount == 0)
      return;
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    string otherPlayerName = string.Empty;
    IList<Player> players = (IList<Player>) ReInput.players.Players;
    for (int index = 0; index < players.Count; ++index)
    {
      if (players[index] != this.currentPlayer && players[index].controllers.ContainsController(ControllerType.Joystick, controllerId))
      {
        otherPlayerName = this._language.GetPlayerName(players[index].id);
        break;
      }
    }
    Joystick joystick = ReInput.controllers.GetJoystick(controllerId);
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.controllerAssignmentConflictWindowTitle);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), this._language.GetControllerAssignmentConflictWindowMessage(this._language.GetControllerName((Controller) joystick), otherPlayerName, this._language.GetPlayerName(this.currentPlayer.id)));
    UnityAction unityAction = (UnityAction) (() => this.OnWindowCancel(window.id));
    window.cancelCallback = unityAction;
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, (UnityAction) (() => this.OnControllerAssignmentConfirmed(window.id, this.currentPlayer, controllerId)), unityAction, true);
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
    this.windowManager.Focus(window);
  }

  private void ShowBeginElementAssignmentReplacementWindow(
    InputFieldInfo fieldInfo,
    InputAction action,
    ControllerMap map,
    ActionElementMap aem,
    string actionName)
  {
    Rewired.UI.ControlMapper.ControlMapper.GUIInputField guiInputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData);
    if (guiInputField == null)
      return;
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, actionName);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), guiInputField.GetLabel());
    UnityAction unityAction = (UnityAction) (() => this.OnWindowCancel(window.id));
    window.cancelCallback = unityAction;
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, (UnityAction) (() => this.OnBeginElementAssignment(fieldInfo, map, aem, actionName)), unityAction, true);
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.remove, (UnityAction) (() => this.OnRemoveElementAssignment(window.id, map, aem)), unityAction, false);
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
    this.windowManager.Focus(window);
  }

  private void ShowCreateNewElementAssignmentWindow(
    InputFieldInfo fieldInfo,
    InputAction action,
    ControllerMap map,
    string actionName)
  {
    if (this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData) == null)
      return;
    this.OnBeginElementAssignment(fieldInfo, map, (ActionElementMap) null, actionName);
  }

  private void ShowElementAssignmentPrePollingWindow()
  {
    if (this.pendingInputMapping == null)
      return;
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), this._language.elementAssignmentPrePollingWindowMessage);
    if ((UnityEngine.Object) this.prefabs.centerStickGraphic != (UnityEngine.Object) null)
      window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0.0f, 40f));
    window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
    window.SetUpdateCallback(new Action<int>(this.OnElementAssignmentPrePollingWindowUpdate));
    window.timer.Start(this._preInputAssignmentTimeout);
    this.windowManager.Focus(window);
  }

  private void ShowElementAssignmentPollingWindow()
  {
    if (this.pendingInputMapping == null)
      return;
    switch (this.pendingInputMapping.controllerType)
    {
      case ControllerType.Keyboard:
        this.ShowKeyboardElementAssignmentPollingWindow();
        break;
      case ControllerType.Mouse:
        if (this.currentPlayer.controllers.hasMouse)
        {
          this.ShowMouseElementAssignmentPollingWindow();
          break;
        }
        this.ShowMouseAssignmentConflictWindow();
        break;
      case ControllerType.Joystick:
        this.ShowJoystickElementAssignmentPollingWindow();
        break;
      default:
        throw new NotImplementedException();
    }
  }

  private void ShowJoystickElementAssignmentPollingWindow()
  {
    if (this.pendingInputMapping == null)
      return;
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    string text = this.pendingInputMapping.axisRange != AxisRange.Full || !this._showFullAxisInputFields || this._showSplitAxisInputFields ? this._language.GetJoystickElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName) : this._language.GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName);
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), text);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
    window.SetUpdateCallback(new Action<int>(this.OnJoystickElementAssignmentPollingWindowUpdate));
    window.timer.Start(this._inputAssignmentTimeout);
    this.windowManager.Focus(window);
  }

  private void ShowKeyboardElementAssignmentPollingWindow()
  {
    if (this.pendingInputMapping == null)
      return;
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), this._language.GetKeyboardElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName));
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, (float) -((double) window.GetContentTextHeight(0) + 50.0)), "");
    window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
    window.SetUpdateCallback(new Action<int>(this.OnKeyboardElementAssignmentPollingWindowUpdate));
    window.timer.Start(this._inputAssignmentTimeout);
    this.windowManager.Focus(window);
  }

  private void ShowMouseElementAssignmentPollingWindow()
  {
    if (this.pendingInputMapping == null)
      return;
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    string text = this.pendingInputMapping.axisRange != AxisRange.Full || !this._showFullAxisInputFields || this._showSplitAxisInputFields ? this._language.GetMouseElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName) : this._language.GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName);
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), text);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
    window.SetUpdateCallback(new Action<int>(this.OnMouseElementAssignmentPollingWindowUpdate));
    window.timer.Start(this._inputAssignmentTimeout);
    this.windowManager.Focus(window);
  }

  private void ShowElementAssignmentConflictWindow(
    ElementAssignment assignment,
    bool skipOtherPlayers)
  {
    if (this.pendingInputMapping == null)
      return;
    bool flag = this.IsBlockingAssignmentConflict(this.pendingInputMapping, assignment, skipOtherPlayers);
    string text = flag ? this._language.GetElementAlreadyInUseBlocked(this.pendingInputMapping.elementName) : this._language.GetElementAlreadyInUseCanReplace(this.pendingInputMapping.elementName, this._allowElementAssignmentConflicts);
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.elementAssignmentConflictWindowMessage);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), text);
    UnityAction unityAction = (UnityAction) (() => this.OnWindowCancel(window.id));
    window.cancelCallback = unityAction;
    if (flag)
    {
      window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.okay, unityAction, unityAction, true);
    }
    else
    {
      window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, (UnityAction) (() => this.OnElementAssignmentConflictReplaceConfirmed(window.id, this.pendingInputMapping, assignment, skipOtherPlayers, false)), unityAction, true);
      if (this._allowElementAssignmentConflicts)
        window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.add, (UnityAction) (() => this.OnElementAssignmentAddConfirmed(window.id, this.pendingInputMapping, assignment)), unityAction, false);
      else if (this.ShowSwapButton(window.id, this.pendingInputMapping, assignment, skipOtherPlayers))
        window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.swap, (UnityAction) (() => this.OnElementAssignmentConflictReplaceConfirmed(window.id, this.pendingInputMapping, assignment, skipOtherPlayers, true)), unityAction, false);
      window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
    }
    this.windowManager.Focus(window);
  }

  private void ShowMouseAssignmentConflictWindow()
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.OpenWindow(true);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    string otherPlayerName = string.Empty;
    IList<Player> players = (IList<Player>) ReInput.players.Players;
    for (int index = 0; index < players.Count; ++index)
    {
      if (players[index] != this.currentPlayer && players[index].controllers.hasMouse)
      {
        otherPlayerName = this._language.GetPlayerName(players[index].id);
        break;
      }
    }
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.mouseAssignmentConflictWindowTitle);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), this._language.GetMouseAssignmentConflictWindowMessage(otherPlayerName, this._language.GetPlayerName(this.currentPlayer.id)));
    UnityAction unityAction = (UnityAction) (() => this.OnWindowCancel(window.id));
    window.cancelCallback = unityAction;
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, (UnityAction) (() => this.OnMouseAssignmentConfirmed(window.id, this.currentPlayer)), unityAction, true);
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
    this.windowManager.Focus(window);
  }

  private void ShowCalibrateControllerWindow()
  {
    if (this.currentPlayer == null || this.currentPlayer.controllers.joystickCount == 0)
      return;
    CalibrationWindow calibrationWindow = this.OpenWindow(this.prefabs.calibrationWindow, "CalibrationWindow", true) as CalibrationWindow;
    if ((UnityEngine.Object) calibrationWindow == (UnityEngine.Object) null)
      return;
    Joystick currentJoystick = this.currentJoystick;
    calibrationWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateControllerWindowTitle);
    calibrationWindow.SetJoystick(this.currentPlayer.id, currentJoystick);
    calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
    calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Calibrate, new Action<int>(this.StartAxisCalibration));
    calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
    this.windowManager.Focus((Window) calibrationWindow);
  }

  private void ShowCalibrateAxisStep1Window()
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.OpenWindow(false);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null || this.pendingAxisCalibration == null)
      return;
    Joystick joystick = this.pendingAxisCalibration.joystick;
    if (joystick.axisCount == 0)
      return;
    int axisIndex = this.pendingAxisCalibration.axisIndex;
    if (axisIndex < 0 || axisIndex >= joystick.axisCount)
      return;
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep1WindowTitle);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), this._language.GetCalibrateAxisStep1WindowMessage(this._language.GetElementIdentifierName((Controller) joystick, ((IList<ControllerElementIdentifier>) joystick.AxisElementIdentifiers)[axisIndex].id, AxisRange.Full)));
    if ((UnityEngine.Object) this.prefabs.centerStickGraphic != (UnityEngine.Object) null)
      window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0.0f, 40f));
    window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
    window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep1WindowUpdate));
    window.timer.Start(this._axisCalibrationTimeout);
    this.windowManager.Focus(window);
  }

  private void ShowCalibrateAxisStep2Window()
  {
    if (this.currentPlayer == null)
      return;
    Window window = this.OpenWindow(false);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null || this.pendingAxisCalibration == null)
      return;
    Joystick joystick = this.pendingAxisCalibration.joystick;
    if (joystick.axisCount == 0)
      return;
    int axisIndex = this.pendingAxisCalibration.axisIndex;
    if (axisIndex < 0 || axisIndex >= joystick.axisCount)
      return;
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep2WindowTitle);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), this._language.GetCalibrateAxisStep2WindowMessage(this._language.GetElementIdentifierName((Controller) joystick, ((IList<ControllerElementIdentifier>) joystick.AxisElementIdentifiers)[axisIndex].id, AxisRange.Full)));
    if ((UnityEngine.Object) this.prefabs.moveStickGraphic != (UnityEngine.Object) null)
      window.AddContentImage(this.prefabs.moveStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0.0f, 40f));
    window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, "");
    window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep2WindowUpdate));
    window.timer.Start(this._axisCalibrationTimeout);
    this.windowManager.Focus(window);
  }

  private void ShowEditInputBehaviorsWindow()
  {
    if (this.currentPlayer == null || this._inputBehaviorSettings == null)
      return;
    InputBehaviorWindow inputBehaviorWindow = this.OpenWindow(this.prefabs.inputBehaviorsWindow, "EditInputBehaviorsWindow", true) as InputBehaviorWindow;
    if ((UnityEngine.Object) inputBehaviorWindow == (UnityEngine.Object) null)
      return;
    inputBehaviorWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.inputBehaviorSettingsWindowTitle);
    inputBehaviorWindow.SetData(this.currentPlayer.id, this._inputBehaviorSettings);
    inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
    inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
    this.windowManager.Focus((Window) inputBehaviorWindow);
  }

  private void ShowRestoreDefaultsWindow()
  {
    if (this.currentPlayer == null)
      return;
    this.OpenModal(this._language.restoreDefaultsWindowTitle, this._language.restoreDefaultsWindowMessage, this._language.yes, new Action<int>(this.OnRestoreDefaultsConfirmed), this._language.no, new Action<int>(this.OnWindowCancel), true);
  }

  private void CreateInputGrid()
  {
    this.InitializeInputGrid();
    this.CreateHeaderLabels();
    this.CreateActionLabelColumn();
    this.CreateKeyboardInputFieldColumn();
    this.CreateMouseInputFieldColumn();
    this.CreateControllerInputFieldColumn();
    this.CreateInputActionLabels();
    this.CreateInputFields();
    this.inputGrid.HideAll();
    this.ResetInputGridScrollBar();
  }

  private void InitializeInputGrid()
  {
    if (this.inputGrid == null)
      this.inputGrid = new Rewired.UI.ControlMapper.ControlMapper.InputGrid();
    else
      this.inputGrid.ClearAll();
    for (int index1 = 0; index1 < this._mappingSets.Length; ++index1)
    {
      Rewired.UI.ControlMapper.ControlMapper.MappingSet mappingSet = this._mappingSets[index1];
      if (mappingSet != null && mappingSet.isValid)
      {
        InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
        if (mapCategory != null && mapCategory.userAssignable)
        {
          this.inputGrid.AddMapCategory(mappingSet.mapCategoryId);
          if (mappingSet.actionListMode == Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory)
          {
            IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
            for (int index2 = 0; index2 < actionCategoryIds.Count; ++index2)
            {
              int num = actionCategoryIds[index2];
              InputCategory actionCategory = ReInput.mapping.GetActionCategory(num);
              if (actionCategory != null && actionCategory.userAssignable)
              {
                this.inputGrid.AddActionCategory(mappingSet.mapCategoryId, num);
                foreach (InputAction action in (IEnumerable<InputAction>) ReInput.mapping.UserAssignableActionsInCategory(num))
                {
                  if (action.type == InputActionType.Axis)
                  {
                    if (this._showFullAxisInputFields)
                      this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Full);
                    if (this._showSplitAxisInputFields)
                    {
                      this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
                      this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Negative);
                    }
                  }
                  else if (action.type == InputActionType.Button)
                    this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
                }
              }
            }
          }
          else
          {
            IList<int> actionIds = mappingSet.actionIds;
            for (int index3 = 0; index3 < actionIds.Count; ++index3)
            {
              InputAction action = ReInput.mapping.GetAction(actionIds[index3]);
              if (action != null)
              {
                if (action.type == InputActionType.Axis)
                {
                  if (this._showFullAxisInputFields)
                    this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Full);
                  if (this._showSplitAxisInputFields)
                  {
                    this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
                    this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Negative);
                  }
                }
                else if (action.type == InputActionType.Button)
                  this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
              }
            }
          }
        }
      }
    }
    this.references.inputGridInnerGroup.GetComponent<HorizontalLayoutGroup>().spacing = (float) this._inputColumnSpacing;
    this.references.inputGridLayoutElement.flexibleWidth = 0.0f;
    this.references.inputGridLayoutElement.preferredWidth = (float) this.inputGridWidth;
  }

  private void RefreshInputGridStructure()
  {
    if (this.currentMappingSet == null)
      return;
    this.inputGrid.HideAll();
    this.inputGrid.Show(this.currentMappingSet.mapCategoryId);
    this.references.inputGridInnerGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.inputGrid.GetColumnHeight(this.currentMappingSet.mapCategoryId));
  }

  private void CreateHeaderLabels()
  {
    this.references.inputGridHeader1 = this.CreateNewColumnGroup("ActionsHeader", this.references.inputGridHeadersGroup, this._actionLabelWidth).transform;
    this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.actionColumnLabel, this.references.inputGridHeader1, Vector2.zero);
    if (this._showKeyboard)
    {
      this.references.inputGridHeader2 = this.CreateNewColumnGroup("KeybordHeader", this.references.inputGridHeadersGroup, this._keyboardColMaxWidth).transform;
      this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.keyboardColumnLabel, this.references.inputGridHeader2, Vector2.zero).SetTextAlignment(TextAlignmentOptions.Center);
    }
    if (this._showMouse)
    {
      this.references.inputGridHeader3 = this.CreateNewColumnGroup("MouseHeader", this.references.inputGridHeadersGroup, this._mouseColMaxWidth).transform;
      this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.mouseColumnLabel, this.references.inputGridHeader3, Vector2.zero).SetTextAlignment(TextAlignmentOptions.Center);
    }
    if (!this._showControllers)
      return;
    this.references.inputGridHeader4 = this.CreateNewColumnGroup("ControllerHeader", this.references.inputGridHeadersGroup, this._controllerColMaxWidth).transform;
    this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.controllerColumnLabel, this.references.inputGridHeader4, Vector2.zero).SetTextAlignment(TextAlignmentOptions.Center);
  }

  private void CreateActionLabelColumn()
  {
    this.references.inputGridActionColumn = this.CreateNewColumnGroup("ActionLabelColumn", this.references.inputGridInnerGroup, this._actionLabelWidth).transform;
  }

  private void CreateKeyboardInputFieldColumn()
  {
    if (!this._showKeyboard)
      return;
    this.CreateInputFieldColumn("KeyboardColumn", ControllerType.Keyboard, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
  }

  private void CreateMouseInputFieldColumn()
  {
    if (!this._showMouse)
      return;
    this.CreateInputFieldColumn("MouseColumn", ControllerType.Mouse, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
  }

  private void CreateControllerInputFieldColumn()
  {
    if (!this._showControllers)
      return;
    this.CreateInputFieldColumn("ControllerColumn", ControllerType.Joystick, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
  }

  private void CreateInputFieldColumn(
    string name,
    ControllerType controllerType,
    int maxWidth,
    int cols,
    bool disableFullAxis)
  {
    Transform transform = this.CreateNewColumnGroup(name, this.references.inputGridInnerGroup, maxWidth).transform;
    switch (controllerType)
    {
      case ControllerType.Keyboard:
        this.references.inputGridKeyboardColumn = transform;
        break;
      case ControllerType.Mouse:
        this.references.inputGridMouseColumn = transform;
        break;
      case ControllerType.Joystick:
        this.references.inputGridControllerColumn = transform;
        break;
      default:
        throw new NotImplementedException();
    }
  }

  private void CreateInputActionLabels()
  {
    Transform gridActionColumn = this.references.inputGridActionColumn;
    for (int index1 = 0; index1 < this._mappingSets.Length; ++index1)
    {
      Rewired.UI.ControlMapper.ControlMapper.MappingSet mappingSet = this._mappingSets[index1];
      if (mappingSet != null && mappingSet.isValid)
      {
        int y1 = 0;
        if (mappingSet.actionListMode == Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory)
        {
          int num = 0;
          IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
          for (int index2 = 0; index2 < actionCategoryIds.Count; ++index2)
          {
            InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[index2]);
            if (actionCategory != null && actionCategory.userAssignable && this.CountIEnumerable<InputAction>((IEnumerable<InputAction>) ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
            {
              if (this._showActionCategoryLabels)
              {
                if (num > 0)
                  y1 -= this._inputRowCategorySpacing;
                Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(this._language.GetActionCategoryName(actionCategory.id), gridActionColumn, new Vector2(0.0f, (float) y1));
                label.SetFontStyle(FontStyles.Bold);
                label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                this.inputGrid.AddActionCategoryLabel(mappingSet.mapCategoryId, actionCategory.id, label);
                y1 -= this._inputRowHeight;
              }
              foreach (InputAction inputAction in (IEnumerable<InputAction>) ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
              {
                if (inputAction.type == InputActionType.Axis)
                {
                  if (this._showFullAxisInputFields)
                  {
                    Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(this._language.GetActionName(inputAction.id, AxisRange.Full), gridActionColumn, new Vector2(0.0f, (float) y1));
                    label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                    this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Full, label);
                    y1 -= this._inputRowHeight;
                  }
                  if (this._showSplitAxisInputFields)
                  {
                    Rewired.UI.ControlMapper.ControlMapper.GUILabel label1 = this.CreateLabel(this._language.GetActionName(inputAction.id, AxisRange.Positive), gridActionColumn, new Vector2(0.0f, (float) y1));
                    label1.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                    this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, label1);
                    y1 -= this._inputRowHeight;
                    Rewired.UI.ControlMapper.ControlMapper.GUILabel label2 = this.CreateLabel(this._language.GetActionName(inputAction.id, AxisRange.Negative), gridActionColumn, new Vector2(0.0f, (float) y1));
                    label2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                    this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Negative, label2);
                    y1 -= this._inputRowHeight;
                  }
                }
                else if (inputAction.type == InputActionType.Button)
                {
                  Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(this._language.GetActionName(inputAction.id), gridActionColumn, new Vector2(0.0f, (float) y1));
                  label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                  this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, label);
                  y1 -= this._inputRowHeight;
                }
              }
              ++num;
            }
          }
        }
        else
        {
          IList<int> actionIds = mappingSet.actionIds;
          for (int index3 = 0; index3 < actionIds.Count; ++index3)
          {
            InputAction action = ReInput.mapping.GetAction(actionIds[index3]);
            if (action != null && action.userAssignable)
            {
              InputCategory actionCategory = ReInput.mapping.GetActionCategory(action.categoryId);
              if (actionCategory != null && actionCategory.userAssignable)
              {
                if (action.type == InputActionType.Axis)
                {
                  if (this._showFullAxisInputFields)
                  {
                    Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Full), gridActionColumn, new Vector2(0.0f, (float) y1));
                    label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                    this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Full, label);
                    y1 -= this._inputRowHeight;
                  }
                  if (this._showSplitAxisInputFields)
                  {
                    Rewired.UI.ControlMapper.ControlMapper.GUILabel label3 = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Positive), gridActionColumn, new Vector2(0.0f, (float) y1));
                    label3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                    this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, label3);
                    int y2 = y1 - this._inputRowHeight;
                    Rewired.UI.ControlMapper.ControlMapper.GUILabel label4 = this.CreateLabel(this._language.GetActionName(action.id, AxisRange.Negative), gridActionColumn, new Vector2(0.0f, (float) y2));
                    label4.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                    this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Negative, label4);
                    y1 = y2 - this._inputRowHeight;
                  }
                }
                else if (action.type == InputActionType.Button)
                {
                  Rewired.UI.ControlMapper.ControlMapper.GUILabel label = this.CreateLabel(this._language.GetActionName(action.id), gridActionColumn, new Vector2(0.0f, (float) y1));
                  label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
                  this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, label);
                  y1 -= this._inputRowHeight;
                }
              }
            }
          }
        }
        this.inputGrid.SetColumnHeight(mappingSet.mapCategoryId, (float) -y1);
      }
    }
  }

  private void CreateInputFields()
  {
    if (this._showControllers)
      this.CreateInputFields(this.references.inputGridControllerColumn, ControllerType.Joystick, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
    if (this._showKeyboard)
      this.CreateInputFields(this.references.inputGridKeyboardColumn, ControllerType.Keyboard, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
    if (!this._showMouse)
      return;
    this.CreateInputFields(this.references.inputGridMouseColumn, ControllerType.Mouse, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
  }

  private void CreateInputFields(
    Transform columnXform,
    ControllerType controllerType,
    int maxWidth,
    int cols,
    bool disableFullAxis)
  {
    for (int index1 = 0; index1 < this._mappingSets.Length; ++index1)
    {
      Rewired.UI.ControlMapper.ControlMapper.MappingSet mappingSet = this._mappingSets[index1];
      if (mappingSet != null && mappingSet.isValid)
      {
        int fieldWidth = maxWidth / cols;
        int yPos = 0;
        int num = 0;
        if (mappingSet.actionListMode == Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory)
        {
          IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
          for (int index2 = 0; index2 < actionCategoryIds.Count; ++index2)
          {
            InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[index2]);
            if (actionCategory != null && actionCategory.userAssignable && this.CountIEnumerable<InputAction>((IEnumerable<InputAction>) ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
            {
              if (this._showActionCategoryLabels)
                yPos -= num > 0 ? this._inputRowHeight + this._inputRowCategorySpacing : this._inputRowHeight;
              foreach (InputAction action in (IEnumerable<InputAction>) ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
              {
                if (action.type == InputActionType.Axis)
                {
                  if (this._showFullAxisInputFields)
                    this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Full, controllerType, cols, fieldWidth, ref yPos, disableFullAxis);
                  if (this._showSplitAxisInputFields)
                  {
                    this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, fieldWidth, ref yPos, false);
                    this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Negative, controllerType, cols, fieldWidth, ref yPos, false);
                  }
                }
                else if (action.type == InputActionType.Button)
                  this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, fieldWidth, ref yPos, false);
                ++num;
              }
            }
          }
        }
        else
        {
          IList<int> actionIds = mappingSet.actionIds;
          for (int index3 = 0; index3 < actionIds.Count; ++index3)
          {
            InputAction action = ReInput.mapping.GetAction(actionIds[index3]);
            if (action != null && action.userAssignable)
            {
              InputCategory actionCategory = ReInput.mapping.GetActionCategory(action.categoryId);
              if (actionCategory != null && actionCategory.userAssignable)
              {
                if (action.type == InputActionType.Axis)
                {
                  if (this._showFullAxisInputFields)
                    this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Full, controllerType, cols, fieldWidth, ref yPos, disableFullAxis);
                  if (this._showSplitAxisInputFields)
                  {
                    this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, fieldWidth, ref yPos, false);
                    this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Negative, controllerType, cols, fieldWidth, ref yPos, false);
                  }
                }
                else if (action.type == InputActionType.Button)
                  this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, fieldWidth, ref yPos, false);
              }
            }
          }
        }
      }
    }
  }

  private void CreateInputFieldSet(
    Transform parent,
    int mapCategoryId,
    InputAction action,
    AxisRange axisRange,
    ControllerType controllerType,
    int cols,
    int fieldWidth,
    ref int yPos,
    bool disableFullAxis)
  {
    GameObject newGuiObject = this.CreateNewGUIObject("FieldLayoutGroup", parent, new Vector2(0.0f, (float) yPos));
    HorizontalLayoutGroup horizontalLayoutGroup = newGuiObject.AddComponent<HorizontalLayoutGroup>();
    horizontalLayoutGroup.padding = this._inputRowPadding;
    horizontalLayoutGroup.spacing = (float) this._inputRowFieldSpacing;
    RectTransform component = newGuiObject.GetComponent<RectTransform>();
    component.anchorMin = new Vector2(0.0f, 1f);
    component.anchorMax = new Vector2(1f, 1f);
    component.pivot = new Vector2(0.0f, 1f);
    component.sizeDelta = Vector2.zero;
    component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) this._inputRowHeight);
    this.inputGrid.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, newGuiObject);
    for (int fieldIndex = 0; fieldIndex < cols; ++fieldIndex)
    {
      int invertToggleWidth = axisRange == AxisRange.Full ? this._invertToggleWidth : 0;
      Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField = this.CreateInputField(horizontalLayoutGroup.transform, Vector2.zero, "", action.id, axisRange, controllerType, fieldIndex);
      inputField.SetFirstChildObjectWidth(Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType.PreferredSize, fieldWidth - invertToggleWidth);
      this.inputGrid.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField);
      if (axisRange == AxisRange.Full)
      {
        if (!disableFullAxis)
        {
          Rewired.UI.ControlMapper.ControlMapper.GUIToggle toggle = this.CreateToggle(this.prefabs.inputGridFieldInvertToggle, horizontalLayoutGroup.transform, Vector2.zero, "", action.id, axisRange, controllerType, fieldIndex);
          toggle.SetFirstChildObjectWidth(Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType.MinSize, invertToggleWidth);
          inputField.AddToggle(toggle);
        }
        else
          inputField.SetInteractible(false, false, true);
      }
    }
    yPos -= this._inputRowHeight;
  }

  private void PopulateInputFields()
  {
    this.inputGrid.InitializeFields(this.currentMapCategoryId);
    if (this.currentPlayer == null)
      return;
    this.inputGrid.SetFieldsActive(this.currentMapCategoryId, true);
    foreach (Rewired.UI.ControlMapper.ControlMapper.InputActionSet actionSet in this.inputGrid.GetActionSets(this.currentMapCategoryId))
    {
      if (this._showKeyboard)
      {
        ControllerType controllerType = ControllerType.Keyboard;
        int controllerId = 0;
        int mapDefaultLayout = this._keyboardMapDefaultLayout;
        int keyboardInputFieldCount = this._keyboardInputFieldCount;
        ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, controllerId, mapDefaultLayout);
        this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, controllerId, keyboardInputFieldCount);
      }
      if (this._showMouse)
      {
        ControllerType controllerType = ControllerType.Mouse;
        int controllerId = 0;
        int mapDefaultLayout = this._mouseMapDefaultLayout;
        int mouseInputFieldCount = this._mouseInputFieldCount;
        ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, controllerId, mapDefaultLayout);
        if (this.currentPlayer.controllers.hasMouse)
          this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, controllerId, mouseInputFieldCount);
      }
      if (this.isJoystickSelected && this.currentPlayer.controllers.joystickCount > 0)
      {
        ControllerType controllerType = ControllerType.Joystick;
        int id = this.currentJoystick.id;
        int mapDefaultLayout = this._joystickMapDefaultLayout;
        int controllerInputFieldCount = this._controllerInputFieldCount;
        ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, id, mapDefaultLayout);
        this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, id, controllerInputFieldCount);
      }
      else
        this.DisableInputFieldGroup(actionSet, ControllerType.Joystick, this._controllerInputFieldCount);
    }
  }

  private void PopulateInputFieldGroup(
    Rewired.UI.ControlMapper.ControlMapper.InputActionSet actionSet,
    ControllerMap controllerMap,
    ControllerType controllerType,
    int controllerId,
    int maxFields)
  {
    if (controllerMap == null)
      return;
    int index = 0;
    this.inputGrid.SetFixedFieldData(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId);
    foreach (ActionElementMap actionElementMap in (IEnumerable<ActionElementMap>) controllerMap.ElementMapsWithAction(actionSet.actionId))
    {
      if (actionElementMap.elementType == ControllerElementType.Button)
      {
        if (actionSet.axisRange != AxisRange.Full)
        {
          if (actionSet.axisRange == AxisRange.Positive)
          {
            if (actionElementMap.axisContribution == Pole.Negative)
              continue;
          }
          else if (actionSet.axisRange == AxisRange.Negative && actionElementMap.axisContribution == Pole.Positive)
            continue;
          this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, index, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
        }
        else
          continue;
      }
      else if (actionElementMap.elementType == ControllerElementType.Axis)
      {
        if (actionSet.axisRange == AxisRange.Full)
        {
          if (actionElementMap.axisRange == AxisRange.Full)
            this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, index, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), actionElementMap.invert);
          else
            continue;
        }
        else if (actionSet.axisRange == AxisRange.Positive)
        {
          if ((actionElementMap.axisRange != AxisRange.Full || ReInput.mapping.GetAction(actionSet.actionId).type == InputActionType.Button) && actionElementMap.axisContribution != Pole.Negative)
            this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, index, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
          else
            continue;
        }
        else if (actionSet.axisRange == AxisRange.Negative)
        {
          if (actionElementMap.axisRange != AxisRange.Full && actionElementMap.axisContribution != Pole.Positive)
            this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, index, actionElementMap.id, this._language.GetElementIdentifierName(actionElementMap), false);
          else
            continue;
        }
      }
      ++index;
      if (index > maxFields)
        break;
    }
  }

  private void DisableInputFieldGroup(
    Rewired.UI.ControlMapper.ControlMapper.InputActionSet actionSet,
    ControllerType controllerType,
    int fieldCount)
  {
    for (int fieldIndex = 0; fieldIndex < fieldCount; ++fieldIndex)
      this.inputGrid.GetGUIInputField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, fieldIndex)?.SetInteractible(false, false);
  }

  private void ResetInputGridScrollBar()
  {
    this.references.inputGridInnerGroup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    this.references.inputGridVScrollbar.value = 1f;
    this.references.inputGridScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
  }

  private void CreateLayout()
  {
    this.references.playersGroup.gameObject.SetActive(this.showPlayers);
    this.references.controllerGroup.gameObject.SetActive(this._showControllers);
    this.references.assignedControllersGroup.gameObject.SetActive(this._showControllers && this.ShowAssignedControllers());
    this.references.settingsAndMapCategoriesGroup.gameObject.SetActive(this.showSettings || this.showMapCategories);
    this.references.settingsGroup.gameObject.SetActive(this.showSettings);
    this.references.mapCategoriesGroup.gameObject.SetActive(this.showMapCategories);
  }

  private void Draw()
  {
    this.DrawPlayersGroup();
    this.DrawControllersGroup();
    this.DrawSettingsGroup();
    this.DrawMapCategoriesGroup();
    this.DrawWindowButtonsGroup();
  }

  private void DrawPlayersGroup()
  {
    if (!this.showPlayers)
      return;
    this.references.playersGroup.labelText = this._language.playersGroupLabel;
    this.references.playersGroup.SetLabelActive(this._showPlayersGroupLabel);
    for (int playerId = 0; playerId < this.playerCount; ++playerId)
    {
      Player player = ReInput.players.GetPlayer(playerId);
      if (player != null)
      {
        Rewired.UI.ControlMapper.ControlMapper.GUIButton guiButton = new Rewired.UI.ControlMapper.ControlMapper.GUIButton(UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.playersGroup.content, $"Player{(object) playerId}Button"));
        guiButton.SetLabel(this._language.GetPlayerName(player.id));
        guiButton.SetButtonInfoData("PlayerSelection", player.id);
        guiButton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
        guiButton.buttonInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
        this.playerButtons.Add(guiButton);
      }
    }
  }

  private void DrawControllersGroup()
  {
    if (!this._showControllers)
      return;
    this.references.controllerSettingsGroup.labelText = this._language.controllerSettingsGroupLabel;
    this.references.controllerSettingsGroup.SetLabelActive(this._showControllerGroupLabel);
    this.references.controllerNameLabel.gameObject.SetActive(this._showControllerNameLabel);
    this.references.controllerGroupLabelGroup.gameObject.SetActive(this._showControllerGroupLabel || this._showControllerNameLabel);
    if (this.ShowAssignedControllers())
    {
      this.references.assignedControllersGroup.labelText = this._language.assignedControllersGroupLabel;
      this.references.assignedControllersGroup.SetLabelActive(this._showAssignedControllersGroupLabel);
    }
    this.references.removeControllerButton.GetComponent<ButtonInfo>().text.text = this._language.removeControllerButtonLabel;
    this.references.calibrateControllerButton.GetComponent<ButtonInfo>().text.text = this._language.calibrateControllerButtonLabel;
    this.references.assignControllerButton.GetComponent<ButtonInfo>().text.text = this._language.assignControllerButtonLabel;
    Rewired.UI.ControlMapper.ControlMapper.GUIButton button = this.CreateButton(this._language.none, this.references.assignedControllersGroup.content, Vector2.zero);
    button.SetInteractible(false, false, true);
    this.assignedControllerButtonsPlaceholder = button;
  }

  private void DrawSettingsGroup()
  {
    if (!this.showSettings)
      return;
    this.references.settingsGroup.labelText = this._language.settingsGroupLabel;
    this.references.settingsGroup.SetLabelActive(this._showSettingsGroupLabel);
    Rewired.UI.ControlMapper.ControlMapper.GUIButton button = this.CreateButton(this._language.inputBehaviorSettingsButtonLabel, this.references.settingsGroup.content, Vector2.zero);
    this.miscInstantiatedObjects.Add(button.gameObject);
    button.buttonInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
    button.SetButtonInfoData("EditInputBehaviors", 0);
    button.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
  }

  private void DrawMapCategoriesGroup()
  {
    if (!this.showMapCategories || this._mappingSets == null)
      return;
    this.references.mapCategoriesGroup.labelText = this._language.mapCategoriesGroupLabel;
    this.references.mapCategoriesGroup.SetLabelActive(this._showMapCategoriesGroupLabel);
    for (int index = 0; index < this._mappingSets.Length; ++index)
    {
      Rewired.UI.ControlMapper.ControlMapper.MappingSet mappingSet = this._mappingSets[index];
      if (mappingSet != null)
      {
        InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
        if (mapCategory != null)
        {
          Rewired.UI.ControlMapper.ControlMapper.GUIButton guiButton = new Rewired.UI.ControlMapper.ControlMapper.GUIButton(UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.mapCategoriesGroup.content, mapCategory.name + "Button"));
          guiButton.SetLabel(this._language.GetMapCategoryName(mapCategory.id));
          guiButton.SetButtonInfoData("MapCategorySelection", mapCategory.id);
          guiButton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
          guiButton.buttonInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
          this.mapCategoryButtons.Add(guiButton);
        }
      }
    }
  }

  private void DrawWindowButtonsGroup()
  {
    this.references.doneButton.GetComponent<ButtonInfo>().text.text = this._language.doneButtonLabel;
    this.references.restoreDefaultsButton.GetComponent<ButtonInfo>().text.text = this._language.restoreDefaultsButtonLabel;
  }

  private void Redraw(bool listsChanged, bool playTransitions)
  {
    this.RedrawPlayerGroup(playTransitions);
    this.RedrawControllerGroup();
    this.RedrawMapCategoriesGroup(playTransitions);
    this.RedrawInputGrid(listsChanged);
    if (!((UnityEngine.Object) this.currentUISelection == (UnityEngine.Object) null) && this.currentUISelection.activeInHierarchy)
      return;
    this.RestoreLastUISelection();
  }

  private void RedrawPlayerGroup(bool playTransitions)
  {
    if (!this.showPlayers)
      return;
    for (int index = 0; index < this.playerButtons.Count; ++index)
    {
      bool state = this.currentPlayerId != this.playerButtons[index].buttonInfo.intData;
      this.playerButtons[index].SetInteractible(state, playTransitions);
    }
  }

  private void RedrawControllerGroup()
  {
    int num = -1;
    this.references.controllerNameLabel.text = this._language.none;
    UITools.SetInteractable((Selectable) this.references.removeControllerButton, false, false);
    UITools.SetInteractable((Selectable) this.references.assignControllerButton, false, false);
    UITools.SetInteractable((Selectable) this.references.calibrateControllerButton, false, false);
    if (this.ShowAssignedControllers())
    {
      foreach (Rewired.UI.ControlMapper.ControlMapper.GUIButton controllerButton in this.assignedControllerButtons)
      {
        if (!((UnityEngine.Object) controllerButton.gameObject == (UnityEngine.Object) null))
        {
          if ((UnityEngine.Object) this.currentUISelection == (UnityEngine.Object) controllerButton.gameObject)
            num = controllerButton.buttonInfo.intData;
          UnityEngine.Object.Destroy((UnityEngine.Object) controllerButton.gameObject);
        }
      }
      this.assignedControllerButtons.Clear();
      this.assignedControllerButtonsPlaceholder.SetActive(true);
    }
    Player player = ReInput.players.GetPlayer(this.currentPlayerId);
    if (player == null)
      return;
    if (this.ShowAssignedControllers())
    {
      if (player.controllers.joystickCount > 0)
        this.assignedControllerButtonsPlaceholder.SetActive(false);
      foreach (Joystick joystick in (IEnumerable<Joystick>) player.controllers.Joysticks)
      {
        Rewired.UI.ControlMapper.ControlMapper.GUIButton button = this.CreateButton(this._language.GetControllerName((Controller) joystick), this.references.assignedControllersGroup.content, Vector2.zero);
        button.SetButtonInfoData("AssignedControllerSelection", joystick.id);
        button.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
        button.buttonInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
        this.assignedControllerButtons.Add(button);
        if (joystick.id == this.currentJoystickId)
          button.SetInteractible(false, true);
      }
      if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
      {
        this.currentJoystickId = ((IList<Joystick>) player.controllers.Joysticks)[0].id;
        this.assignedControllerButtons[0].SetInteractible(false, false);
      }
      if (num >= 0)
      {
        foreach (Rewired.UI.ControlMapper.ControlMapper.GUIButton controllerButton in this.assignedControllerButtons)
        {
          if (controllerButton.buttonInfo.intData == num)
          {
            this.SetUISelection(controllerButton.gameObject);
            break;
          }
        }
      }
    }
    else if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
      this.currentJoystickId = ((IList<Joystick>) player.controllers.Joysticks)[0].id;
    if (this.isJoystickSelected && player.controllers.joystickCount > 0)
    {
      this.references.removeControllerButton.interactable = true;
      this.references.controllerNameLabel.text = this._language.GetControllerName((Controller) this.currentJoystick);
      if (this.currentJoystick.axisCount > 0)
        this.references.calibrateControllerButton.interactable = true;
    }
    int joystickCount1 = player.controllers.joystickCount;
    int joystickCount2 = ReInput.controllers.joystickCount;
    int controllersPerPlayer = this.GetMaxControllersPerPlayer();
    bool flag = controllersPerPlayer == 0;
    if (joystickCount2 <= 0 || joystickCount1 >= joystickCount2 || !(controllersPerPlayer == 1 | flag) && joystickCount1 >= controllersPerPlayer)
      return;
    UITools.SetInteractable((Selectable) this.references.assignControllerButton, true, false);
  }

  private void RedrawMapCategoriesGroup(bool playTransitions)
  {
    if (!this.showMapCategories)
      return;
    for (int index = 0; index < this.mapCategoryButtons.Count; ++index)
    {
      bool state = this.currentMapCategoryId != this.mapCategoryButtons[index].buttonInfo.intData;
      this.mapCategoryButtons[index].SetInteractible(state, playTransitions);
    }
  }

  private void RedrawInputGrid(bool listsChanged)
  {
    if (listsChanged)
      this.RefreshInputGridStructure();
    this.PopulateInputFields();
    if (!listsChanged)
      return;
    this.ResetInputGridScrollBar();
  }

  private void ForceRefresh()
  {
    if (this.windowManager.isWindowOpen)
      this.CloseAllWindows();
    else
      this.Redraw(false, false);
  }

  private void CreateInputCategoryRow(ref int rowCount, InputCategory category)
  {
    this.CreateLabel(this._language.GetMapCategoryName(category.id), this.references.inputGridActionColumn, new Vector2(0.0f, (float) (rowCount * this._inputRowHeight) * -1f));
    ++rowCount;
  }

  private Rewired.UI.ControlMapper.ControlMapper.GUILabel CreateLabel(
    string labelText,
    Transform parent,
    Vector2 offset)
  {
    return this.CreateLabel(this.prefabs.inputGridLabel, labelText, parent, offset);
  }

  private Rewired.UI.ControlMapper.ControlMapper.GUILabel CreateLabel(
    GameObject prefab,
    string labelText,
    Transform parent,
    Vector2 offset)
  {
    GameObject gameObject = this.InstantiateGUIObject(prefab, parent, offset);
    TMP_Text inSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<TMP_Text>(gameObject);
    if ((UnityEngine.Object) inSelfOrChildren == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Rewired Control Mapper: Label prefab is missing Text component!");
      return (Rewired.UI.ControlMapper.ControlMapper.GUILabel) null;
    }
    inSelfOrChildren.text = labelText;
    return new Rewired.UI.ControlMapper.ControlMapper.GUILabel(gameObject);
  }

  private Rewired.UI.ControlMapper.ControlMapper.GUIButton CreateButton(
    string labelText,
    Transform parent,
    Vector2 offset)
  {
    Rewired.UI.ControlMapper.ControlMapper.GUIButton button = new Rewired.UI.ControlMapper.ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.button, parent, offset));
    button.SetLabel(labelText);
    return button;
  }

  private Rewired.UI.ControlMapper.ControlMapper.GUIButton CreateFitButton(
    string labelText,
    Transform parent,
    Vector2 offset)
  {
    Rewired.UI.ControlMapper.ControlMapper.GUIButton fitButton = new Rewired.UI.ControlMapper.ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.fitButton, parent, offset));
    fitButton.SetLabel(labelText);
    return fitButton;
  }

  private Rewired.UI.ControlMapper.ControlMapper.GUIInputField CreateInputField(
    Transform parent,
    Vector2 offset,
    string label,
    int actionId,
    AxisRange axisRange,
    ControllerType controllerType,
    int fieldIndex)
  {
    Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField = this.CreateInputField(parent, offset);
    inputField.SetLabel("");
    inputField.SetFieldInfoData(actionId, axisRange, controllerType, fieldIndex);
    inputField.SetOnClickCallback(this.inputFieldActivatedDelegate);
    inputField.fieldInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
    return inputField;
  }

  private Rewired.UI.ControlMapper.ControlMapper.GUIInputField CreateInputField(
    Transform parent,
    Vector2 offset)
  {
    return new Rewired.UI.ControlMapper.ControlMapper.GUIInputField(this.InstantiateGUIObject(this.prefabs.inputGridFieldButton, parent, offset));
  }

  private Rewired.UI.ControlMapper.ControlMapper.GUIToggle CreateToggle(
    GameObject prefab,
    Transform parent,
    Vector2 offset,
    string label,
    int actionId,
    AxisRange axisRange,
    ControllerType controllerType,
    int fieldIndex)
  {
    Rewired.UI.ControlMapper.ControlMapper.GUIToggle toggle = this.CreateToggle(prefab, parent, offset);
    toggle.SetToggleInfoData(actionId, axisRange, controllerType, fieldIndex);
    toggle.SetOnSubmitCallback(this.inputFieldInvertToggleStateChangedDelegate);
    toggle.toggleInfo.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
    return toggle;
  }

  private Rewired.UI.ControlMapper.ControlMapper.GUIToggle CreateToggle(
    GameObject prefab,
    Transform parent,
    Vector2 offset)
  {
    return new Rewired.UI.ControlMapper.ControlMapper.GUIToggle(this.InstantiateGUIObject(prefab, parent, offset));
  }

  private GameObject InstantiateGUIObject(GameObject prefab, Transform parent, Vector2 offset)
  {
    if (!((UnityEngine.Object) prefab == (UnityEngine.Object) null))
      return this.InitializeNewGUIGameObject(UnityEngine.Object.Instantiate<GameObject>(prefab), parent, offset);
    Debug.LogError((object) "Rewired Control Mapper: Prefab is null!");
    return (GameObject) null;
  }

  private GameObject CreateNewGUIObject(string name, Transform parent, Vector2 offset)
  {
    GameObject gameObject = new GameObject();
    gameObject.name = name;
    gameObject.AddComponent<RectTransform>();
    return this.InitializeNewGUIGameObject(gameObject, parent, offset);
  }

  private GameObject InitializeNewGUIGameObject(
    GameObject gameObject,
    Transform parent,
    Vector2 offset)
  {
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Rewired Control Mapper: GameObject is null!");
      return (GameObject) null;
    }
    RectTransform component = gameObject.GetComponent<RectTransform>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Rewired Control Mapper: GameObject does not have a RectTransform component!");
      return gameObject;
    }
    if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
      component.SetParent(parent, false);
    component.anchoredPosition = offset;
    return gameObject;
  }

  private GameObject CreateNewColumnGroup(string name, Transform parent, int maxWidth)
  {
    GameObject newGuiObject = this.CreateNewGUIObject(name, parent, Vector2.zero);
    this.inputGrid.AddGroup(newGuiObject);
    LayoutElement layoutElement = newGuiObject.AddComponent<LayoutElement>();
    if (maxWidth >= 0)
      layoutElement.preferredWidth = (float) maxWidth;
    RectTransform component = newGuiObject.GetComponent<RectTransform>();
    component.anchorMin = new Vector2(0.0f, 0.0f);
    component.anchorMax = new Vector2(1f, 0.0f);
    return newGuiObject;
  }

  private Window OpenWindow(bool closeOthers) => this.OpenWindow(string.Empty, closeOthers);

  private Window OpenWindow(string name, bool closeOthers)
  {
    if (closeOthers)
      this.windowManager.CancelAll();
    Window window = this.windowManager.OpenWindow(name, this._defaultWindowWidth, this._defaultWindowHeight);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return (Window) null;
    this.ChildWindowOpened();
    return window;
  }

  private Window OpenWindow(GameObject windowPrefab, bool closeOthers)
  {
    return this.OpenWindow(windowPrefab, string.Empty, closeOthers);
  }

  private Window OpenWindow(GameObject windowPrefab, string name, bool closeOthers)
  {
    if (closeOthers)
      this.windowManager.CancelAll();
    Window window = this.windowManager.OpenWindow(windowPrefab, name);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return (Window) null;
    this.ChildWindowOpened();
    return window;
  }

  private void OpenModal(
    string title,
    string message,
    string confirmText,
    Action<int> confirmAction,
    string cancelText,
    Action<int> cancelAction,
    bool closeOthers)
  {
    Window window = this.OpenWindow(closeOthers);
    if ((UnityEngine.Object) window == (UnityEngine.Object) null)
      return;
    window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, title);
    window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0.0f, -100f), message);
    UnityAction unityAction = (UnityAction) (() => this.OnWindowCancel(window.id));
    window.cancelCallback = unityAction;
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, confirmText, (UnityAction) (() => this.OnRestoreDefaultsConfirmed(window.id)), unityAction, false);
    window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, cancelText, unityAction, unityAction, true);
    this.windowManager.Focus(window);
  }

  private void CloseWindow(int windowId)
  {
    if (!this.windowManager.isWindowOpen)
      return;
    this.windowManager.CloseWindow(windowId);
    this.ChildWindowClosed();
  }

  private void CloseTopWindow()
  {
    if (!this.windowManager.isWindowOpen)
      return;
    this.windowManager.CloseTop();
    this.ChildWindowClosed();
  }

  private void CloseAllWindows()
  {
    if (!this.windowManager.isWindowOpen)
      return;
    this.windowManager.CancelAll();
    this.ChildWindowClosed();
    this.InputPollingStopped();
  }

  private void ChildWindowOpened()
  {
    if (!this.windowManager.isWindowOpen)
      return;
    this.SetIsFocused(false);
    if (this._PopupWindowOpenedEvent != null)
      this._PopupWindowOpenedEvent();
    if (this._onPopupWindowOpened == null)
      return;
    this._onPopupWindowOpened.Invoke();
  }

  private void ChildWindowClosed()
  {
    if (this.windowManager.isWindowOpen)
      return;
    this.SetIsFocused(true);
    if (this._PopupWindowClosedEvent != null)
      this._PopupWindowClosedEvent();
    if (this._onPopupWindowClosed == null)
      return;
    this._onPopupWindowClosed.Invoke();
  }

  private bool HasElementAssignmentConflicts(
    Player player,
    Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
    ElementAssignment assignment,
    bool skipOtherPlayers)
  {
    ElementAssignmentConflictCheck conflictCheck;
    if (player == null || mapping == null || !this.CreateConflictCheck(mapping, assignment, out conflictCheck))
      return false;
    if (!skipOtherPlayers)
      return ReInput.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck);
    return ReInput.players.SystemPlayer.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck) || player.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck);
  }

  private bool IsBlockingAssignmentConflict(
    Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
    ElementAssignment assignment,
    bool skipOtherPlayers)
  {
    ElementAssignmentConflictCheck conflictCheck;
    if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
      return false;
    if (skipOtherPlayers)
    {
      foreach (ElementAssignmentConflictInfo assignmentConflict in (IEnumerable<ElementAssignmentConflictInfo>) ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
      {
        if (!assignmentConflict.isUserAssignable)
          return true;
      }
      foreach (ElementAssignmentConflictInfo assignmentConflict in (IEnumerable<ElementAssignmentConflictInfo>) this.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
      {
        if (!assignmentConflict.isUserAssignable)
          return true;
      }
    }
    else
    {
      foreach (ElementAssignmentConflictInfo assignmentConflict in (IEnumerable<ElementAssignmentConflictInfo>) ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
      {
        if (!assignmentConflict.isUserAssignable)
          return true;
      }
    }
    return false;
  }

  private IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(
    Player player,
    Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
    ElementAssignment assignment,
    bool skipOtherPlayers)
  {
    ElementAssignmentConflictCheck conflictCheck;
    if (player != null && mapping != null && this.CreateConflictCheck(mapping, assignment, out conflictCheck))
    {
      if (skipOtherPlayers)
      {
        foreach (ElementAssignmentConflictInfo assignmentConflict in (IEnumerable<ElementAssignmentConflictInfo>) ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
        {
          if (!assignmentConflict.isUserAssignable)
            yield return assignmentConflict;
        }
        foreach (ElementAssignmentConflictInfo assignmentConflict in (IEnumerable<ElementAssignmentConflictInfo>) player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
        {
          if (!assignmentConflict.isUserAssignable)
            yield return assignmentConflict;
        }
      }
      else
      {
        foreach (ElementAssignmentConflictInfo assignmentConflict in (IEnumerable<ElementAssignmentConflictInfo>) ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
        {
          if (!assignmentConflict.isUserAssignable)
            yield return assignmentConflict;
        }
      }
    }
  }

  private bool CreateConflictCheck(
    Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
    ElementAssignment assignment,
    out ElementAssignmentConflictCheck conflictCheck)
  {
    if (mapping == null || this.currentPlayer == null)
    {
      conflictCheck = new ElementAssignmentConflictCheck();
      return false;
    }
    conflictCheck = assignment.ToElementAssignmentConflictCheck();
    conflictCheck.playerId = this.currentPlayer.id;
    conflictCheck.controllerType = mapping.controllerType;
    conflictCheck.controllerId = mapping.controllerId;
    conflictCheck.controllerMapId = mapping.map.id;
    conflictCheck.controllerMapCategoryId = mapping.map.categoryId;
    if (mapping.aem != null)
      conflictCheck.elementMapId = mapping.aem.id;
    return true;
  }

  private void PollKeyboardForAssignment(
    out ControllerPollingInfo pollingInfo,
    out bool modifierKeyPressed,
    out ModifierKeyFlags modifierFlags,
    out string label)
  {
    pollingInfo = new ControllerPollingInfo();
    label = string.Empty;
    modifierKeyPressed = false;
    modifierFlags = ModifierKeyFlags.None;
    int num = 0;
    ControllerPollingInfo controllerPollingInfo1 = new ControllerPollingInfo();
    ControllerPollingInfo controllerPollingInfo2 = new ControllerPollingInfo();
    ModifierKeyFlags flags = ModifierKeyFlags.None;
    foreach (ControllerPollingInfo pollForAllKey in (IEnumerable<ControllerPollingInfo>) ReInput.controllers.Keyboard.PollForAllKeys())
    {
      KeyCode keyboardKey = pollForAllKey.keyboardKey;
      if (keyboardKey != KeyCode.AltGr)
      {
        if (Keyboard.IsModifierKey(pollForAllKey.keyboardKey))
        {
          if (num == 0)
            controllerPollingInfo2 = pollForAllKey;
          flags |= Keyboard.KeyCodeToModifierKeyFlags(keyboardKey);
          ++num;
        }
        else if (controllerPollingInfo1.keyboardKey == KeyCode.None)
          controllerPollingInfo1 = pollForAllKey;
      }
    }
    if (controllerPollingInfo1.keyboardKey != KeyCode.None)
    {
      if (!ReInput.controllers.Keyboard.GetKeyDown(controllerPollingInfo1.keyboardKey))
        return;
      if (num == 0)
      {
        pollingInfo = controllerPollingInfo1;
      }
      else
      {
        pollingInfo = controllerPollingInfo1;
        modifierFlags = flags;
      }
    }
    else
    {
      if (num <= 0)
        return;
      modifierKeyPressed = true;
      if (num == 1)
      {
        if (ReInput.controllers.Keyboard.GetKeyTimePressed(controllerPollingInfo2.keyboardKey) > 1.0)
          pollingInfo = controllerPollingInfo2;
        else
          label = Keyboard.GetKeyName(controllerPollingInfo2.keyboardKey);
      }
      else
        label = this._language.ModifierKeyFlagsToString(flags);
    }
  }

  private bool GetFirstElementAssignmentConflict(
    ElementAssignmentConflictCheck conflictCheck,
    out ElementAssignmentConflictInfo conflict,
    bool skipOtherPlayers)
  {
    if (this.GetFirstElementAssignmentConflict(this.currentPlayer, conflictCheck, out conflict) || this.GetFirstElementAssignmentConflict(ReInput.players.SystemPlayer, conflictCheck, out conflict))
      return true;
    if (!skipOtherPlayers)
    {
      IList<Player> players = (IList<Player>) ReInput.players.Players;
      for (int index = 0; index < players.Count; ++index)
      {
        Player player = players[index];
        if (player != this.currentPlayer && this.GetFirstElementAssignmentConflict(player, conflictCheck, out conflict))
          return true;
      }
    }
    return false;
  }

  private bool GetFirstElementAssignmentConflict(
    Player player,
    ElementAssignmentConflictCheck conflictCheck,
    out ElementAssignmentConflictInfo conflict)
  {
    using (IEnumerator<ElementAssignmentConflictInfo> enumerator = ((IEnumerable<ElementAssignmentConflictInfo>) player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck)).GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        ElementAssignmentConflictInfo current = enumerator.Current;
        conflict = current;
        return true;
      }
    }
    conflict = new ElementAssignmentConflictInfo();
    return false;
  }

  private void StartAxisCalibration(int axisIndex)
  {
    if (this.currentPlayer == null || this.currentPlayer.controllers.joystickCount == 0)
      return;
    Joystick currentJoystick = this.currentJoystick;
    if (axisIndex < 0 || axisIndex >= currentJoystick.axisCount)
      return;
    this.pendingAxisCalibration = new Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator(currentJoystick, axisIndex);
    this.ShowCalibrateAxisStep1Window();
  }

  private void EndAxisCalibration()
  {
    if (this.pendingAxisCalibration == null)
      return;
    this.pendingAxisCalibration.Commit();
    this.pendingAxisCalibration = (Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator) null;
  }

  private void SetUISelection(GameObject selection)
  {
    if ((UnityEngine.Object) EventSystem.current == (UnityEngine.Object) null)
      return;
    EventSystem.current.SetSelectedGameObject(selection);
  }

  private void RestoreLastUISelection()
  {
    if ((UnityEngine.Object) this.lastUISelection == (UnityEngine.Object) null || !this.lastUISelection.activeInHierarchy)
      this.SetDefaultUISelection();
    else
      this.SetUISelection(this.lastUISelection);
  }

  private void SetDefaultUISelection()
  {
    if (!this.isOpen)
      return;
    if ((UnityEngine.Object) this.references.defaultSelection == (UnityEngine.Object) null)
      this.SetUISelection((GameObject) null);
    else
      this.SetUISelection(this.references.defaultSelection.gameObject);
  }

  private void SelectDefaultMapCategory(bool redraw)
  {
    this.currentMapCategoryId = this.GetDefaultMapCategoryId();
    this.OnMapCategorySelected(this.currentMapCategoryId, redraw);
    if (!this.showMapCategories)
      return;
    for (int index = 0; index < this._mappingSets.Length; ++index)
    {
      if (ReInput.mapping.GetMapCategory(this._mappingSets[index].mapCategoryId) != null)
      {
        this.currentMapCategoryId = this._mappingSets[index].mapCategoryId;
        break;
      }
    }
    if (this.currentMapCategoryId < 0)
      return;
    for (int index = 0; index < this._mappingSets.Length; ++index)
    {
      bool state = this._mappingSets[index].mapCategoryId != this.currentMapCategoryId;
      this.mapCategoryButtons[index].SetInteractible(state, false);
    }
  }

  private void CheckUISelection()
  {
    if (!this.isFocused || !((UnityEngine.Object) this.currentUISelection == (UnityEngine.Object) null))
      return;
    this.RestoreLastUISelection();
  }

  private void OnUIElementSelected(GameObject selectedObject)
  {
    this.lastUISelection = selectedObject;
  }

  private void SetIsFocused(bool state)
  {
    this.references.mainCanvasGroup.interactable = state;
    if (!state)
      return;
    this.Redraw(false, false);
    this.RestoreLastUISelection();
    this.blockInputOnFocusEndTime = Time.unscaledTime + 0.1f;
  }

  public void Toggle()
  {
    if (this.isOpen)
      this.Close(true);
    else
      this.Open();
  }

  public void Open() => this.Open(false);

  private void Open(bool force)
  {
    if (!this.initialized)
      this.Initialize();
    if (!this.initialized || !force && this.isOpen)
      return;
    this.Clear();
    this.canvas.SetActive(true);
    this.OnPlayerSelected(0, false);
    this.SelectDefaultMapCategory(false);
    this.SetDefaultUISelection();
    this.Redraw(true, false);
    if (this._ScreenOpenedEvent != null)
      this._ScreenOpenedEvent();
    if (this._onScreenOpened == null)
      return;
    this._onScreenOpened.Invoke();
  }

  public void Close(bool save)
  {
    if (!this.initialized || !this.isOpen)
      return;
    if (save && ReInput.userDataStore != null)
      ReInput.userDataStore.Save();
    this.Clear();
    this.canvas.SetActive(false);
    this.SetUISelection((GameObject) null);
    if (this._ScreenClosedEvent != null)
      this._ScreenClosedEvent();
    if (this._onScreenClosed == null)
      return;
    this._onScreenClosed.Invoke();
  }

  private void Clear()
  {
    this.CloseAllWindows();
    this.lastUISelection = (GameObject) null;
    this.pendingInputMapping = (Rewired.UI.ControlMapper.ControlMapper.InputMapping) null;
    this.pendingAxisCalibration = (Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator) null;
    this.InputPollingStopped();
  }

  private void ClearCompletely()
  {
    this.Clear();
    this.ClearSpawnedObjects();
    this.ClearAllVars();
  }

  private void ClearSpawnedObjects()
  {
    this.windowManager.ClearCompletely();
    this.inputGrid.ClearAll();
    foreach (Rewired.UI.ControlMapper.ControlMapper.GUIElement playerButton in this.playerButtons)
      UnityEngine.Object.Destroy((UnityEngine.Object) playerButton.gameObject);
    this.playerButtons.Clear();
    foreach (Rewired.UI.ControlMapper.ControlMapper.GUIElement mapCategoryButton in this.mapCategoryButtons)
      UnityEngine.Object.Destroy((UnityEngine.Object) mapCategoryButton.gameObject);
    this.mapCategoryButtons.Clear();
    foreach (Rewired.UI.ControlMapper.ControlMapper.GUIElement controllerButton in this.assignedControllerButtons)
      UnityEngine.Object.Destroy((UnityEngine.Object) controllerButton.gameObject);
    this.assignedControllerButtons.Clear();
    if (this.assignedControllerButtonsPlaceholder != null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.assignedControllerButtonsPlaceholder.gameObject);
      this.assignedControllerButtonsPlaceholder = (Rewired.UI.ControlMapper.ControlMapper.GUIButton) null;
    }
    foreach (UnityEngine.Object instantiatedObject in this.miscInstantiatedObjects)
      UnityEngine.Object.Destroy(instantiatedObject);
    this.miscInstantiatedObjects.Clear();
  }

  private void ClearVarsOnPlayerChange() => this.currentJoystickId = -1;

  private void ClearVarsOnJoystickChange() => this.currentJoystickId = -1;

  private void ClearAllVars()
  {
    this.initialized = false;
    Rewired.UI.ControlMapper.ControlMapper.Instance = (Rewired.UI.ControlMapper.ControlMapper) null;
    this.playerCount = 0;
    this.inputGrid = (Rewired.UI.ControlMapper.ControlMapper.InputGrid) null;
    this.windowManager = (Rewired.UI.ControlMapper.ControlMapper.WindowManager) null;
    this.currentPlayerId = -1;
    this.currentMapCategoryId = -1;
    this.playerButtons = (List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>) null;
    this.mapCategoryButtons = (List<Rewired.UI.ControlMapper.ControlMapper.GUIButton>) null;
    this.miscInstantiatedObjects = (List<GameObject>) null;
    this.canvas = (GameObject) null;
    this.lastUISelection = (GameObject) null;
    this.currentJoystickId = -1;
    this.pendingInputMapping = (Rewired.UI.ControlMapper.ControlMapper.InputMapping) null;
    this.pendingAxisCalibration = (Rewired.UI.ControlMapper.ControlMapper.AxisCalibrator) null;
    this.inputFieldActivatedDelegate = (Action<InputFieldInfo>) null;
    this.inputFieldInvertToggleStateChangedDelegate = (Action<ToggleInfo, bool>) null;
    this.isPollingForInput = false;
  }

  public void Reset()
  {
    if (!this.initialized)
      return;
    this.ClearCompletely();
    this.Initialize();
    if (!this.isOpen)
      return;
    this.Open(true);
  }

  private void SetActionAxisInverted(
    bool state,
    ControllerType controllerType,
    int actionElementMapId)
  {
    if (this.currentPlayer == null || !(this.GetControllerMap(controllerType) is ControllerMapWithAxes controllerMap))
      return;
    ActionElementMap elementMap = controllerMap.GetElementMap(actionElementMapId);
    if (elementMap == null)
      return;
    elementMap.invert = state;
  }

  private ControllerMap GetControllerMap(ControllerType type)
  {
    if (this.currentPlayer == null)
      return (ControllerMap) null;
    int controllerId = 0;
    switch (type)
    {
      case ControllerType.Keyboard:
      case ControllerType.Mouse:
        return this.currentPlayer.controllers.maps.GetFirstMapInCategory(type, controllerId, this.currentMapCategoryId);
      case ControllerType.Joystick:
        if (this.currentPlayer.controllers.joystickCount <= 0)
          return (ControllerMap) null;
        controllerId = this.currentJoystick.id;
        goto case ControllerType.Keyboard;
      default:
        throw new NotImplementedException();
    }
  }

  private ControllerMap GetControllerMapOrCreateNew(
    ControllerType controllerType,
    int controllerId,
    int layoutId)
  {
    ControllerMap controllerMapOrCreateNew = this.GetControllerMap(controllerType);
    if (controllerMapOrCreateNew == null)
    {
      this.currentPlayer.controllers.maps.AddEmptyMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
      controllerMapOrCreateNew = this.currentPlayer.controllers.maps.GetMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
    }
    return controllerMapOrCreateNew;
  }

  private int CountIEnumerable<T>(IEnumerable<T> enumerable)
  {
    if (enumerable == null)
      return 0;
    IEnumerator<T> enumerator = enumerable.GetEnumerator();
    if (enumerator == null)
      return 0;
    int num = 0;
    while (enumerator.MoveNext())
      ++num;
    return num;
  }

  private int GetDefaultMapCategoryId()
  {
    if (this._mappingSets.Length == 0)
      return 0;
    for (int index = 0; index < this._mappingSets.Length; ++index)
    {
      if (ReInput.mapping.GetMapCategory(this._mappingSets[index].mapCategoryId) != null)
        return this._mappingSets[index].mapCategoryId;
    }
    return 0;
  }

  private void SubscribeFixedUISelectionEvents()
  {
    if (this.references.fixedSelectableUIElements == null)
      return;
    foreach (GameObject selectableUiElement in this.references.fixedSelectableUIElements)
    {
      UIElementInfo component = UnityTools.GetComponent<UIElementInfo>(selectableUiElement);
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
        component.OnSelectedEvent += new Action<GameObject>(this.OnUIElementSelected);
    }
  }

  private void SubscribeMenuControlInputEvents()
  {
    this.SubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
    this.SubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
    this.SubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
    this.SubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
  }

  private void UnsubscribeMenuControlInputEvents()
  {
    this.UnsubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
    this.UnsubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
    this.UnsubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
    this.UnsubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
  }

  private void SubscribeRewiredInputEventAllPlayers(
    int actionId,
    Action<InputActionEventData> callback)
  {
    if (actionId < 0 || callback == null)
      return;
    if (ReInput.mapping.GetAction(actionId) == null)
    {
      Debug.LogWarning((object) $"Rewired Control Mapper: {(object) actionId} is not a valid Action id!");
    }
    else
    {
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
        allPlayer.AddInputEventDelegate((Action<InputActionEventData>) callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId);
    }
  }

  private void UnsubscribeRewiredInputEventAllPlayers(
    int actionId,
    Action<InputActionEventData> callback)
  {
    if (actionId < 0 || callback == null || !ReInput.isReady)
      return;
    if (ReInput.mapping.GetAction(actionId) == null)
    {
      Debug.LogWarning((object) $"Rewired Control Mapper: {(object) actionId} is not a valid Action id!");
    }
    else
    {
      foreach (Player allPlayer in (IEnumerable<Player>) ReInput.players.AllPlayers)
        allPlayer.RemoveInputEventDelegate((Action<InputActionEventData>) callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId);
    }
  }

  private int GetMaxControllersPerPlayer()
  {
    return this._rewiredInputManager.userData.ConfigVars.autoAssignJoysticks ? this._rewiredInputManager.userData.ConfigVars.maxJoysticksPerPlayer : this._maxControllersPerPlayer;
  }

  private bool ShowAssignedControllers()
  {
    return this._showControllers && (this._showAssignedControllers || this.GetMaxControllersPerPlayer() != 1);
  }

  private void InspectorPropertyChanged(bool reset = false)
  {
    if (!reset)
      return;
    this.Reset();
  }

  private void AssignController(Player player, int controllerId)
  {
    if (player == null || player.controllers.ContainsController(ControllerType.Joystick, controllerId))
      return;
    if (this.GetMaxControllersPerPlayer() == 1)
    {
      this.RemoveAllControllers(player);
      this.ClearVarsOnJoystickChange();
    }
    foreach (Player player1 in (IEnumerable<Player>) ReInput.players.Players)
    {
      if (player1 != player)
        this.RemoveController(player1, controllerId);
    }
    player.controllers.AddController(ControllerType.Joystick, controllerId, false);
    if (ReInput.userDataStore == null)
      return;
    ReInput.userDataStore.LoadControllerData(player.id, ControllerType.Joystick, controllerId);
  }

  private void RemoveAllControllers(Player player)
  {
    if (player == null)
      return;
    IList<Joystick> joysticks = (IList<Joystick>) player.controllers.Joysticks;
    for (int index = joysticks.Count - 1; index >= 0; --index)
      this.RemoveController(player, joysticks[index].id);
  }

  private void RemoveController(Player player, int controllerId)
  {
    if (player == null || !player.controllers.ContainsController(ControllerType.Joystick, controllerId))
      return;
    if (ReInput.userDataStore != null)
      ReInput.userDataStore.SaveControllerData(player.id, ControllerType.Joystick, controllerId);
    player.controllers.RemoveController(ControllerType.Joystick, controllerId);
  }

  private bool IsAllowedAssignment(
    Rewired.UI.ControlMapper.ControlMapper.InputMapping pendingInputMapping,
    ControllerPollingInfo pollingInfo)
  {
    return pendingInputMapping != null && (pendingInputMapping.axisRange != AxisRange.Full || this._showSplitAxisInputFields || pollingInfo.elementType != ControllerElementType.Button);
  }

  private void InputPollingStarted()
  {
    int num = this.isPollingForInput ? 1 : 0;
    this.isPollingForInput = true;
    if (num != 0)
      return;
    if (this._InputPollingStartedEvent != null)
      this._InputPollingStartedEvent();
    if (this._onInputPollingStarted == null)
      return;
    this._onInputPollingStarted.Invoke();
  }

  private void InputPollingStopped()
  {
    int num = this.isPollingForInput ? 1 : 0;
    this.isPollingForInput = false;
    if (num == 0)
      return;
    if (this._InputPollingEndedEvent != null)
      this._InputPollingEndedEvent();
    if (this._onInputPollingEnded == null)
      return;
    this._onInputPollingEnded.Invoke();
  }

  private int GetControllerInputFieldCount(ControllerType controllerType)
  {
    switch (controllerType)
    {
      case ControllerType.Keyboard:
        return this._keyboardInputFieldCount;
      case ControllerType.Mouse:
        return this._mouseInputFieldCount;
      case ControllerType.Joystick:
        return this._controllerInputFieldCount;
      default:
        throw new NotImplementedException();
    }
  }

  private bool ShowSwapButton(
    int windowId,
    Rewired.UI.ControlMapper.ControlMapper.InputMapping mapping,
    ElementAssignment assignment,
    bool skipOtherPlayers)
  {
    if (this.currentPlayer == null || !this._allowElementAssignmentSwap || mapping == null || mapping.aem == null)
      return false;
    ElementAssignmentConflictCheck conflictCheck;
    if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
    {
      Debug.LogError((object) "Rewired Control Mapper: Error creating conflict check!");
      return false;
    }
    List<ElementAssignmentConflictInfo> assignmentConflictInfoList = new List<ElementAssignmentConflictInfo>();
    assignmentConflictInfoList.AddRange((IEnumerable<ElementAssignmentConflictInfo>) this.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck));
    assignmentConflictInfoList.AddRange((IEnumerable<ElementAssignmentConflictInfo>) ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck));
    if (assignmentConflictInfoList.Count == 0)
      return false;
    ActionElementMap aem1 = mapping.aem;
    ElementAssignmentConflictInfo assignmentConflictInfo = assignmentConflictInfoList[0];
    int actionId = assignmentConflictInfo.elementMap.actionId;
    Pole axisContribution = assignmentConflictInfo.elementMap.axisContribution;
    AxisRange origAxisRange = aem1.axisRange;
    ControllerElementType elementType = aem1.elementType;
    if (elementType == assignmentConflictInfo.elementMap.elementType && elementType == ControllerElementType.Axis)
    {
      if (origAxisRange != assignmentConflictInfo.elementMap.axisRange)
      {
        if (origAxisRange == AxisRange.Full)
          origAxisRange = AxisRange.Positive;
        else if (assignmentConflictInfo.elementMap.axisRange != AxisRange.Full)
          ;
      }
    }
    else if (elementType == ControllerElementType.Axis && (assignmentConflictInfo.elementMap.elementType == ControllerElementType.Button || assignmentConflictInfo.elementMap.elementType == ControllerElementType.Axis && assignmentConflictInfo.elementMap.axisRange != AxisRange.Full) && origAxisRange == AxisRange.Full)
      origAxisRange = AxisRange.Positive;
    int num = 0;
    if (assignment.actionId == assignmentConflictInfo.actionId && mapping.map == assignmentConflictInfo.controllerMap)
    {
      Controller controller = ReInput.controllers.GetController(mapping.controllerType, mapping.controllerId);
      if (this.SwapIsSameInputRange(elementType, origAxisRange, axisContribution, controller.GetElementById(assignment.elementIdentifierId).type, assignment.axisRange, assignment.axisContribution))
        ++num;
    }
    foreach (ActionElementMap actionElementMap in (IEnumerable<ActionElementMap>) assignmentConflictInfo.controllerMap.ElementMapsWithAction(actionId))
    {
      ActionElementMap aem = actionElementMap;
      if (aem.id != aem1.id && assignmentConflictInfoList.FindIndex((Predicate<ElementAssignmentConflictInfo>) (x => x.elementMapId == aem.id)) < 0 && this.SwapIsSameInputRange(elementType, origAxisRange, axisContribution, aem.elementType, aem.axisRange, aem.axisContribution))
        ++num;
    }
    return num < this.GetControllerInputFieldCount(mapping.controllerType);
  }

  private bool SwapIsSameInputRange(
    ControllerElementType origElementType,
    AxisRange origAxisRange,
    Pole origAxisContribution,
    ControllerElementType conflictElementType,
    AxisRange conflictAxisRange,
    Pole conflictAxisContribution)
  {
    if (origElementType == ControllerElementType.Button || origElementType == ControllerElementType.Axis && origAxisRange != AxisRange.Full)
    {
      switch (conflictElementType)
      {
        case ControllerElementType.Axis:
          if (conflictAxisRange == AxisRange.Full)
            break;
          goto case ControllerElementType.Button;
        case ControllerElementType.Button:
          if (conflictAxisContribution == origAxisContribution)
            return true;
          break;
      }
    }
    return origElementType == ControllerElementType.Axis && origAxisRange == AxisRange.Full && conflictElementType == ControllerElementType.Axis && conflictAxisRange == AxisRange.Full;
  }

  public static void ApplyTheme(ThemedElement.ElementInfo[] elementInfo)
  {
    if ((UnityEngine.Object) Rewired.UI.ControlMapper.ControlMapper.Instance == (UnityEngine.Object) null || (UnityEngine.Object) Rewired.UI.ControlMapper.ControlMapper.Instance._themeSettings == (UnityEngine.Object) null || !Rewired.UI.ControlMapper.ControlMapper.Instance._useThemeSettings)
      return;
    Rewired.UI.ControlMapper.ControlMapper.Instance._themeSettings.Apply(elementInfo);
  }

  public static LanguageDataBase GetLanguage()
  {
    return (UnityEngine.Object) Rewired.UI.ControlMapper.ControlMapper.Instance == (UnityEngine.Object) null ? (LanguageDataBase) null : Rewired.UI.ControlMapper.ControlMapper.Instance._language;
  }

  private abstract class GUIElement
  {
    public readonly GameObject gameObject;
    protected readonly TMP_Text text;
    public readonly Selectable selectable;
    protected readonly UIElementInfo uiElementInfo;
    protected bool permanentStateSet;
    protected readonly List<Rewired.UI.ControlMapper.ControlMapper.GUIElement> children;

    public RectTransform rectTransform { get; private set; }

    public GUIElement(GameObject gameObject)
    {
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: gameObject is null!");
      }
      else
      {
        this.selectable = gameObject.GetComponent<Selectable>();
        if ((UnityEngine.Object) this.selectable == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "Rewired Control Mapper: Selectable is null!");
        }
        else
        {
          this.gameObject = gameObject;
          this.rectTransform = gameObject.GetComponent<RectTransform>();
          this.text = UnityTools.GetComponentInSelfOrChildren<TMP_Text>(gameObject);
          this.uiElementInfo = gameObject.GetComponent<UIElementInfo>();
          this.children = new List<Rewired.UI.ControlMapper.ControlMapper.GUIElement>();
        }
      }
    }

    public GUIElement(Selectable selectable, TMP_Text label)
    {
      if ((UnityEngine.Object) selectable == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: Selectable is null!");
      }
      else
      {
        this.selectable = selectable;
        this.gameObject = selectable.gameObject;
        this.rectTransform = this.gameObject.GetComponent<RectTransform>();
        this.text = label;
        this.uiElementInfo = this.gameObject.GetComponent<UIElementInfo>();
        this.children = new List<Rewired.UI.ControlMapper.ControlMapper.GUIElement>();
      }
    }

    public virtual void SetInteractible(bool state, bool playTransition)
    {
      this.SetInteractible(state, playTransition, false);
    }

    public virtual void SetInteractible(bool state, bool playTransition, bool permanent)
    {
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index] != null)
          this.children[index].SetInteractible(state, playTransition, permanent);
      }
      if (this.permanentStateSet || (UnityEngine.Object) this.selectable == (UnityEngine.Object) null)
        return;
      if (permanent)
        this.permanentStateSet = true;
      if (this.selectable.interactable == state)
        return;
      UITools.SetInteractable(this.selectable, state, playTransition);
    }

    public virtual void SetTextWidth(int value)
    {
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
        return;
      LayoutElement layoutElement = this.text.GetComponent<LayoutElement>();
      if ((UnityEngine.Object) layoutElement == (UnityEngine.Object) null)
        layoutElement = this.text.gameObject.AddComponent<LayoutElement>();
      layoutElement.preferredWidth = (float) value;
    }

    public virtual void SetFirstChildObjectWidth(
      Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType type,
      int value)
    {
      if (this.rectTransform.childCount == 0)
        return;
      Transform child = this.rectTransform.GetChild(0);
      LayoutElement layoutElement = child.GetComponent<LayoutElement>();
      if ((UnityEngine.Object) layoutElement == (UnityEngine.Object) null)
        layoutElement = child.gameObject.AddComponent<LayoutElement>();
      if (type == Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType.MinSize)
      {
        layoutElement.minWidth = (float) value;
      }
      else
      {
        if (type != Rewired.UI.ControlMapper.ControlMapper.LayoutElementSizeType.PreferredSize)
          throw new NotImplementedException();
        layoutElement.preferredWidth = (float) value;
      }
    }

    public virtual void SetLabel(string label)
    {
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
        return;
      this.text.text = label;
    }

    public virtual string GetLabel()
    {
      return (UnityEngine.Object) this.text == (UnityEngine.Object) null ? string.Empty : this.text.text;
    }

    public virtual void AddChild(Rewired.UI.ControlMapper.ControlMapper.GUIElement child)
    {
      this.children.Add(child);
    }

    public void SetElementInfoData(string identifier, int intData)
    {
      if ((UnityEngine.Object) this.uiElementInfo == (UnityEngine.Object) null)
        return;
      this.uiElementInfo.identifier = identifier;
      this.uiElementInfo.intData = intData;
    }

    public virtual void SetActive(bool state)
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
        return;
      this.gameObject.SetActive(state);
    }

    protected virtual bool Init()
    {
      bool flag = true;
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index] != null && !this.children[index].Init())
          flag = false;
      }
      if ((UnityEngine.Object) this.selectable == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: UI Element is missing Selectable component!");
        flag = false;
      }
      if ((UnityEngine.Object) this.rectTransform == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: UI Element is missing RectTransform component!");
        flag = false;
      }
      if ((UnityEngine.Object) this.uiElementInfo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: UI Element is missing UIElementInfo component!");
        flag = false;
      }
      return flag;
    }
  }

  private class GUIButton : Rewired.UI.ControlMapper.ControlMapper.GUIElement
  {
    protected UnityEngine.UI.Button button => this.selectable as UnityEngine.UI.Button;

    public ButtonInfo buttonInfo => this.uiElementInfo as ButtonInfo;

    public GUIButton(GameObject gameObject)
      : base(gameObject)
    {
      this.Init();
    }

    public GUIButton(UnityEngine.UI.Button button, TMP_Text label)
      : base((Selectable) button, label)
    {
      this.Init();
    }

    public void SetButtonInfoData(string identifier, int intData)
    {
      this.SetElementInfoData(identifier, intData);
    }

    public void SetOnClickCallback(Action<ButtonInfo> callback)
    {
      if ((UnityEngine.Object) this.button == (UnityEngine.Object) null)
        return;
      this.button.onClick.AddListener((UnityAction) (() => callback(this.buttonInfo)));
    }
  }

  private class GUIInputField : Rewired.UI.ControlMapper.ControlMapper.GUIElement
  {
    protected UnityEngine.UI.Button button => this.selectable as UnityEngine.UI.Button;

    public InputFieldInfo fieldInfo => this.uiElementInfo as InputFieldInfo;

    public bool hasToggle => this.toggle != null;

    public Rewired.UI.ControlMapper.ControlMapper.GUIToggle toggle { get; private set; }

    public int actionElementMapId
    {
      get => (UnityEngine.Object) this.fieldInfo == (UnityEngine.Object) null ? -1 : this.fieldInfo.actionElementMapId;
      set
      {
        if ((UnityEngine.Object) this.fieldInfo == (UnityEngine.Object) null)
          return;
        this.fieldInfo.actionElementMapId = value;
      }
    }

    public int controllerId
    {
      get => (UnityEngine.Object) this.fieldInfo == (UnityEngine.Object) null ? -1 : this.fieldInfo.controllerId;
      set
      {
        if ((UnityEngine.Object) this.fieldInfo == (UnityEngine.Object) null)
          return;
        this.fieldInfo.controllerId = value;
      }
    }

    public GUIInputField(GameObject gameObject)
      : base(gameObject)
    {
      this.Init();
    }

    public GUIInputField(UnityEngine.UI.Button button, TMP_Text label)
      : base((Selectable) button, label)
    {
      this.Init();
    }

    public void SetFieldInfoData(
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int intData)
    {
      this.SetElementInfoData(string.Empty, intData);
      if ((UnityEngine.Object) this.fieldInfo == (UnityEngine.Object) null)
        return;
      this.fieldInfo.actionId = actionId;
      this.fieldInfo.axisRange = axisRange;
      this.fieldInfo.controllerType = controllerType;
    }

    public void SetOnClickCallback(Action<InputFieldInfo> callback)
    {
      if ((UnityEngine.Object) this.button == (UnityEngine.Object) null)
        return;
      this.button.onClick.AddListener((UnityAction) (() => callback(this.fieldInfo)));
    }

    public virtual void SetInteractable(bool state, bool playTransition, bool permanent)
    {
      if (this.permanentStateSet)
        return;
      if (this.hasToggle && !state)
        this.toggle.SetInteractible(state, playTransition, permanent);
      this.SetInteractible(state, playTransition, permanent);
    }

    public void AddToggle(Rewired.UI.ControlMapper.ControlMapper.GUIToggle toggle)
    {
      if (toggle == null)
        return;
      this.toggle = toggle;
    }
  }

  private class GUIToggle : Rewired.UI.ControlMapper.ControlMapper.GUIElement
  {
    protected UnityEngine.UI.Toggle toggle => this.selectable as UnityEngine.UI.Toggle;

    public ToggleInfo toggleInfo => this.uiElementInfo as ToggleInfo;

    public int actionElementMapId
    {
      get => (UnityEngine.Object) this.toggleInfo == (UnityEngine.Object) null ? -1 : this.toggleInfo.actionElementMapId;
      set
      {
        if ((UnityEngine.Object) this.toggleInfo == (UnityEngine.Object) null)
          return;
        this.toggleInfo.actionElementMapId = value;
      }
    }

    public GUIToggle(GameObject gameObject)
      : base(gameObject)
    {
      this.Init();
    }

    public GUIToggle(UnityEngine.UI.Toggle toggle, TMP_Text label)
      : base((Selectable) toggle, label)
    {
      this.Init();
    }

    public void SetToggleInfoData(
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int intData)
    {
      this.SetElementInfoData(string.Empty, intData);
      if ((UnityEngine.Object) this.toggleInfo == (UnityEngine.Object) null)
        return;
      this.toggleInfo.actionId = actionId;
      this.toggleInfo.axisRange = axisRange;
      this.toggleInfo.controllerType = controllerType;
    }

    public void SetOnSubmitCallback(Action<ToggleInfo, bool> callback)
    {
      if ((UnityEngine.Object) this.toggle == (UnityEngine.Object) null)
        return;
      EventTrigger eventTrigger = this.toggle.GetComponent<EventTrigger>();
      if ((UnityEngine.Object) eventTrigger == (UnityEngine.Object) null)
        eventTrigger = this.toggle.gameObject.AddComponent<EventTrigger>();
      EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
      triggerEvent.AddListener((UnityAction<BaseEventData>) (data =>
      {
        if (data is PointerEventData pointerEventData2 && pointerEventData2.button != PointerEventData.InputButton.Left)
          return;
        callback(this.toggleInfo, this.toggle.isOn);
      }));
      EventTrigger.Entry entry1 = new EventTrigger.Entry()
      {
        callback = triggerEvent,
        eventID = EventTriggerType.Submit
      };
      EventTrigger.Entry entry2 = new EventTrigger.Entry()
      {
        callback = triggerEvent,
        eventID = EventTriggerType.PointerClick
      };
      if (eventTrigger.triggers != null)
        eventTrigger.triggers.Clear();
      else
        eventTrigger.triggers = new List<EventTrigger.Entry>();
      eventTrigger.triggers.Add(entry1);
      eventTrigger.triggers.Add(entry2);
    }

    public void SetToggleState(bool state)
    {
      if ((UnityEngine.Object) this.toggle == (UnityEngine.Object) null)
        return;
      this.toggle.isOn = state;
    }
  }

  private class GUILabel
  {
    public GameObject gameObject { get; private set; }

    private TMP_Text text { get; set; }

    public RectTransform rectTransform { get; private set; }

    public GUILabel(GameObject gameObject)
    {
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: gameObject is null!");
      }
      else
      {
        this.text = UnityTools.GetComponentInSelfOrChildren<TMP_Text>(gameObject);
        this.Check();
      }
    }

    public GUILabel(TMP_Text label)
    {
      this.text = label;
      this.Check();
    }

    public void SetSize(int width, int height)
    {
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
        return;
      this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float) width);
      this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) height);
    }

    public void SetWidth(int width)
    {
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
        return;
      this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float) width);
    }

    public void SetHeight(int height)
    {
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
        return;
      this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) height);
    }

    public void SetLabel(string label)
    {
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
        return;
      this.text.text = label;
    }

    public void SetFontStyle(FontStyles style)
    {
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
        return;
      this.text.fontStyle = style;
    }

    public void SetTextAlignment(TextAlignmentOptions alignment)
    {
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
        return;
      this.text.alignment = alignment;
    }

    public void SetActive(bool state)
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
        return;
      this.gameObject.SetActive(state);
    }

    private bool Check()
    {
      bool flag = true;
      if ((UnityEngine.Object) this.text == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: Button is missing Text child component!");
        flag = false;
      }
      this.gameObject = this.text.gameObject;
      this.rectTransform = this.text.GetComponent<RectTransform>();
      return flag;
    }
  }

  [Serializable]
  public class MappingSet
  {
    [SerializeField]
    [Tooltip("The Map Category that will be displayed to the user for remapping.")]
    private int _mapCategoryId;
    [SerializeField]
    [Tooltip("Choose whether you want to list Actions to display for this Map Category by individual Action or by all the Actions in an Action Category.")]
    private Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode _actionListMode;
    [SerializeField]
    private int[] _actionCategoryIds;
    [SerializeField]
    private int[] _actionIds;
    private IList<int> _actionCategoryIdsReadOnly;
    private IList<int> _actionIdsReadOnly;

    public int mapCategoryId => this._mapCategoryId;

    public Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode actionListMode
    {
      get => this._actionListMode;
    }

    public IList<int> actionCategoryIds
    {
      get
      {
        if (this._actionCategoryIds == null)
          return (IList<int>) null;
        if (this._actionCategoryIdsReadOnly == null)
          this._actionCategoryIdsReadOnly = (IList<int>) new ReadOnlyCollection<int>((IList<int>) this._actionCategoryIds);
        return this._actionCategoryIdsReadOnly;
      }
    }

    public IList<int> actionIds
    {
      get
      {
        if (this._actionIds == null)
          return (IList<int>) null;
        if (this._actionIdsReadOnly == null)
          this._actionIdsReadOnly = (IList<int>) new ReadOnlyCollection<int>((IList<int>) this._actionIds);
        return (IList<int>) this._actionIds;
      }
    }

    public bool isValid
    {
      get
      {
        return this._mapCategoryId >= 0 && ReInput.mapping.GetMapCategory(this._mapCategoryId) != null;
      }
    }

    public MappingSet()
    {
      this._mapCategoryId = -1;
      this._actionCategoryIds = new int[0];
      this._actionIds = new int[0];
      this._actionListMode = Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory;
    }

    private MappingSet(
      int mapCategoryId,
      Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode actionListMode,
      int[] actionCategoryIds,
      int[] actionIds)
    {
      this._mapCategoryId = mapCategoryId;
      this._actionListMode = actionListMode;
      this._actionCategoryIds = actionCategoryIds;
      this._actionIds = actionIds;
    }

    public static Rewired.UI.ControlMapper.ControlMapper.MappingSet Default
    {
      get
      {
        return new Rewired.UI.ControlMapper.ControlMapper.MappingSet(0, Rewired.UI.ControlMapper.ControlMapper.MappingSet.ActionListMode.ActionCategory, new int[1], new int[0]);
      }
    }

    public enum ActionListMode
    {
      ActionCategory,
      Action,
    }
  }

  [Serializable]
  public class InputBehaviorSettings
  {
    [SerializeField]
    [Tooltip("The Input Behavior that will be displayed to the user for modification.")]
    private int _inputBehaviorId = -1;
    [SerializeField]
    [Tooltip("If checked, a slider will be displayed so the user can change this value.")]
    private bool _showJoystickAxisSensitivity = true;
    [SerializeField]
    [Tooltip("If checked, a slider will be displayed so the user can change this value.")]
    private bool _showMouseXYAxisSensitivity = true;
    [SerializeField]
    [Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed as the title for the Input Behavior control set. Otherwise, the name field of the InputBehavior will be used.")]
    private string _labelLanguageKey = string.Empty;
    [SerializeField]
    [Tooltip("If set to a non-blank value, this name will be displayed above the individual slider control. Otherwise, no name will be displayed.")]
    private string _joystickAxisSensitivityLabelLanguageKey = string.Empty;
    [SerializeField]
    [Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed above the individual slider control. Otherwise, no name will be displayed.")]
    private string _mouseXYAxisSensitivityLabelLanguageKey = string.Empty;
    [SerializeField]
    [Tooltip("The icon to display next to the slider. Set to none for no icon.")]
    private Sprite _joystickAxisSensitivityIcon;
    [SerializeField]
    [Tooltip("The icon to display next to the slider. Set to none for no icon.")]
    private Sprite _mouseXYAxisSensitivityIcon;
    [SerializeField]
    [Tooltip("Minimum value the user is allowed to set for this property.")]
    private float _joystickAxisSensitivityMin;
    [SerializeField]
    [Tooltip("Maximum value the user is allowed to set for this property.")]
    private float _joystickAxisSensitivityMax = 2f;
    [SerializeField]
    [Tooltip("Minimum value the user is allowed to set for this property.")]
    private float _mouseXYAxisSensitivityMin;
    [SerializeField]
    [Tooltip("Maximum value the user is allowed to set for this property.")]
    private float _mouseXYAxisSensitivityMax = 2f;

    public int inputBehaviorId => this._inputBehaviorId;

    public bool showJoystickAxisSensitivity => this._showJoystickAxisSensitivity;

    public bool showMouseXYAxisSensitivity => this._showMouseXYAxisSensitivity;

    public string labelLanguageKey => this._labelLanguageKey;

    public string joystickAxisSensitivityLabelLanguageKey
    {
      get => this._joystickAxisSensitivityLabelLanguageKey;
    }

    public string mouseXYAxisSensitivityLabelLanguageKey
    {
      get => this._mouseXYAxisSensitivityLabelLanguageKey;
    }

    public Sprite joystickAxisSensitivityIcon => this._joystickAxisSensitivityIcon;

    public Sprite mouseXYAxisSensitivityIcon => this._mouseXYAxisSensitivityIcon;

    public float joystickAxisSensitivityMin => this._joystickAxisSensitivityMin;

    public float joystickAxisSensitivityMax => this._joystickAxisSensitivityMax;

    public float mouseXYAxisSensitivityMin => this._mouseXYAxisSensitivityMin;

    public float mouseXYAxisSensitivityMax => this._mouseXYAxisSensitivityMax;

    public bool isValid
    {
      get
      {
        if (this._inputBehaviorId < 0)
          return false;
        return this._showJoystickAxisSensitivity || this._showMouseXYAxisSensitivity;
      }
    }
  }

  [Serializable]
  private class Prefabs
  {
    [SerializeField]
    private GameObject _button;
    [SerializeField]
    private GameObject _fitButton;
    [SerializeField]
    private GameObject _inputGridLabel;
    [SerializeField]
    private GameObject _inputGridHeaderLabel;
    [SerializeField]
    private GameObject _inputGridFieldButton;
    [SerializeField]
    private GameObject _inputGridFieldInvertToggle;
    [SerializeField]
    private GameObject _window;
    [SerializeField]
    private GameObject _windowTitleText;
    [SerializeField]
    private GameObject _windowContentText;
    [SerializeField]
    private GameObject _fader;
    [SerializeField]
    private GameObject _calibrationWindow;
    [SerializeField]
    private GameObject _inputBehaviorsWindow;
    [SerializeField]
    private GameObject _centerStickGraphic;
    [SerializeField]
    private GameObject _moveStickGraphic;

    public GameObject button => this._button;

    public GameObject fitButton => this._fitButton;

    public GameObject inputGridLabel => this._inputGridLabel;

    public GameObject inputGridHeaderLabel => this._inputGridHeaderLabel;

    public GameObject inputGridFieldButton => this._inputGridFieldButton;

    public GameObject inputGridFieldInvertToggle => this._inputGridFieldInvertToggle;

    public GameObject window => this._window;

    public GameObject windowTitleText => this._windowTitleText;

    public GameObject windowContentText => this._windowContentText;

    public GameObject fader => this._fader;

    public GameObject calibrationWindow => this._calibrationWindow;

    public GameObject inputBehaviorsWindow => this._inputBehaviorsWindow;

    public GameObject centerStickGraphic => this._centerStickGraphic;

    public GameObject moveStickGraphic => this._moveStickGraphic;

    public bool Check()
    {
      return !((UnityEngine.Object) this._button == (UnityEngine.Object) null) && !((UnityEngine.Object) this._fitButton == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridLabel == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridHeaderLabel == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridFieldButton == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridFieldInvertToggle == (UnityEngine.Object) null) && !((UnityEngine.Object) this._window == (UnityEngine.Object) null) && !((UnityEngine.Object) this._windowTitleText == (UnityEngine.Object) null) && !((UnityEngine.Object) this._windowContentText == (UnityEngine.Object) null) && !((UnityEngine.Object) this._fader == (UnityEngine.Object) null) && !((UnityEngine.Object) this._calibrationWindow == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputBehaviorsWindow == (UnityEngine.Object) null);
    }
  }

  [Serializable]
  private class References
  {
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private CanvasGroup _mainCanvasGroup;
    [SerializeField]
    private Transform _mainContent;
    [SerializeField]
    private Transform _mainContentInner;
    [SerializeField]
    private UIGroup _playersGroup;
    [SerializeField]
    private Transform _controllerGroup;
    [SerializeField]
    private Transform _controllerGroupLabelGroup;
    [SerializeField]
    private UIGroup _controllerSettingsGroup;
    [SerializeField]
    private UIGroup _assignedControllersGroup;
    [SerializeField]
    private Transform _settingsAndMapCategoriesGroup;
    [SerializeField]
    private UIGroup _settingsGroup;
    [SerializeField]
    private UIGroup _mapCategoriesGroup;
    [SerializeField]
    private Transform _inputGridGroup;
    [SerializeField]
    private Transform _inputGridContainer;
    [SerializeField]
    private Transform _inputGridHeadersGroup;
    [SerializeField]
    private Scrollbar _inputGridVScrollbar;
    [SerializeField]
    private ScrollRect _inputGridScrollRect;
    [SerializeField]
    private Transform _inputGridInnerGroup;
    [SerializeField]
    private TMP_Text _controllerNameLabel;
    [SerializeField]
    private UnityEngine.UI.Button _removeControllerButton;
    [SerializeField]
    private UnityEngine.UI.Button _assignControllerButton;
    [SerializeField]
    private UnityEngine.UI.Button _calibrateControllerButton;
    [SerializeField]
    private UnityEngine.UI.Button _doneButton;
    [SerializeField]
    private UnityEngine.UI.Button _restoreDefaultsButton;
    [SerializeField]
    private Selectable _defaultSelection;
    [SerializeField]
    private GameObject[] _fixedSelectableUIElements;
    [SerializeField]
    private Image _mainBackgroundImage;

    public Canvas canvas => this._canvas;

    public CanvasGroup mainCanvasGroup => this._mainCanvasGroup;

    public Transform mainContent => this._mainContent;

    public Transform mainContentInner => this._mainContentInner;

    public UIGroup playersGroup => this._playersGroup;

    public Transform controllerGroup => this._controllerGroup;

    public Transform controllerGroupLabelGroup => this._controllerGroupLabelGroup;

    public UIGroup controllerSettingsGroup => this._controllerSettingsGroup;

    public UIGroup assignedControllersGroup => this._assignedControllersGroup;

    public Transform settingsAndMapCategoriesGroup => this._settingsAndMapCategoriesGroup;

    public UIGroup settingsGroup => this._settingsGroup;

    public UIGroup mapCategoriesGroup => this._mapCategoriesGroup;

    public Transform inputGridGroup => this._inputGridGroup;

    public Transform inputGridContainer => this._inputGridContainer;

    public Transform inputGridHeadersGroup => this._inputGridHeadersGroup;

    public Scrollbar inputGridVScrollbar => this._inputGridVScrollbar;

    public ScrollRect inputGridScrollRect => this._inputGridScrollRect;

    public Transform inputGridInnerGroup => this._inputGridInnerGroup;

    public TMP_Text controllerNameLabel => this._controllerNameLabel;

    public UnityEngine.UI.Button removeControllerButton => this._removeControllerButton;

    public UnityEngine.UI.Button assignControllerButton => this._assignControllerButton;

    public UnityEngine.UI.Button calibrateControllerButton => this._calibrateControllerButton;

    public UnityEngine.UI.Button doneButton => this._doneButton;

    public UnityEngine.UI.Button restoreDefaultsButton => this._restoreDefaultsButton;

    public Selectable defaultSelection => this._defaultSelection;

    public GameObject[] fixedSelectableUIElements => this._fixedSelectableUIElements;

    public Image mainBackgroundImage => this._mainBackgroundImage;

    public LayoutElement inputGridLayoutElement { get; set; }

    public Transform inputGridActionColumn { get; set; }

    public Transform inputGridKeyboardColumn { get; set; }

    public Transform inputGridMouseColumn { get; set; }

    public Transform inputGridControllerColumn { get; set; }

    public Transform inputGridHeader1 { get; set; }

    public Transform inputGridHeader2 { get; set; }

    public Transform inputGridHeader3 { get; set; }

    public Transform inputGridHeader4 { get; set; }

    public bool Check()
    {
      return !((UnityEngine.Object) this._canvas == (UnityEngine.Object) null) && !((UnityEngine.Object) this._mainCanvasGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._mainContent == (UnityEngine.Object) null) && !((UnityEngine.Object) this._mainContentInner == (UnityEngine.Object) null) && !((UnityEngine.Object) this._playersGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._controllerGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._controllerGroupLabelGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._controllerSettingsGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._assignedControllersGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._settingsAndMapCategoriesGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._settingsGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._mapCategoriesGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridContainer == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridHeadersGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridVScrollbar == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridScrollRect == (UnityEngine.Object) null) && !((UnityEngine.Object) this._inputGridInnerGroup == (UnityEngine.Object) null) && !((UnityEngine.Object) this._controllerNameLabel == (UnityEngine.Object) null) && !((UnityEngine.Object) this._removeControllerButton == (UnityEngine.Object) null) && !((UnityEngine.Object) this._assignControllerButton == (UnityEngine.Object) null) && !((UnityEngine.Object) this._calibrateControllerButton == (UnityEngine.Object) null) && !((UnityEngine.Object) this._doneButton == (UnityEngine.Object) null) && !((UnityEngine.Object) this._restoreDefaultsButton == (UnityEngine.Object) null) && !((UnityEngine.Object) this._defaultSelection == (UnityEngine.Object) null);
    }
  }

  private class InputActionSet
  {
    private int _actionId;
    private AxisRange _axisRange;

    public int actionId => this._actionId;

    public AxisRange axisRange => this._axisRange;

    public InputActionSet(int actionId, AxisRange axisRange)
    {
      this._actionId = actionId;
      this._axisRange = axisRange;
    }
  }

  private class InputMapping
  {
    public string actionName { get; private set; }

    public InputFieldInfo fieldInfo { get; private set; }

    public ControllerMap map { get; private set; }

    public ActionElementMap aem { get; private set; }

    public ControllerType controllerType { get; private set; }

    public int controllerId { get; private set; }

    public ControllerPollingInfo pollingInfo { get; set; }

    public ModifierKeyFlags modifierKeyFlags { get; set; }

    public AxisRange axisRange
    {
      get
      {
        AxisRange axisRange = AxisRange.Positive;
        if (this.pollingInfo.elementType == ControllerElementType.Axis)
          axisRange = this.fieldInfo.axisRange != AxisRange.Full ? (this.pollingInfo.axisPole == Pole.Positive ? AxisRange.Positive : AxisRange.Negative) : AxisRange.Full;
        return axisRange;
      }
    }

    public string elementName
    {
      get
      {
        return this.controllerType == ControllerType.Keyboard ? Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetElementIdentifierName(this.pollingInfo.keyboardKey, this.modifierKeyFlags) : Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetElementIdentifierName(this.pollingInfo.controller, this.pollingInfo.elementIdentifierId, this.pollingInfo.axisPole == Pole.Positive ? AxisRange.Positive : AxisRange.Negative);
      }
    }

    public InputMapping(
      string actionName,
      InputFieldInfo fieldInfo,
      ControllerMap map,
      ActionElementMap aem,
      ControllerType controllerType,
      int controllerId)
    {
      this.actionName = actionName;
      this.fieldInfo = fieldInfo;
      this.map = map;
      this.aem = aem;
      this.controllerType = controllerType;
      this.controllerId = controllerId;
    }

    public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo)
    {
      this.pollingInfo = pollingInfo;
      return this.ToElementAssignment();
    }

    public ElementAssignment ToElementAssignment(
      ControllerPollingInfo pollingInfo,
      ModifierKeyFlags modifierKeyFlags)
    {
      this.pollingInfo = pollingInfo;
      this.modifierKeyFlags = modifierKeyFlags;
      return this.ToElementAssignment();
    }

    public ElementAssignment ToElementAssignment()
    {
      int controllerType = (int) this.controllerType;
      ControllerPollingInfo pollingInfo = this.pollingInfo;
      int elementType = (int) pollingInfo.elementType;
      pollingInfo = this.pollingInfo;
      int elementIdentifierId = pollingInfo.elementIdentifierId;
      int axisRange = (int) this.axisRange;
      pollingInfo = this.pollingInfo;
      int keyboardKey = (int) pollingInfo.keyboardKey;
      int modifierKeyFlags = (int) this.modifierKeyFlags;
      int actionId = this.fieldInfo.actionId;
      int axisContribution = this.fieldInfo.axisRange == AxisRange.Negative ? 1 : 0;
      int elementMapId = this.aem != null ? this.aem.id : -1;
      return new ElementAssignment((ControllerType) controllerType, (ControllerElementType) elementType, elementIdentifierId, (AxisRange) axisRange, (KeyCode) keyboardKey, (ModifierKeyFlags) modifierKeyFlags, actionId, (Pole) axisContribution, false, elementMapId);
    }
  }

  private class AxisCalibrator
  {
    public AxisCalibrationData data;
    public readonly Joystick joystick;
    public readonly int axisIndex;
    private Controller.Axis axis;
    private bool firstRun;

    public bool isValid => this.axis != null;

    public AxisCalibrator(Joystick joystick, int axisIndex)
    {
      this.data = new AxisCalibrationData();
      this.joystick = joystick;
      this.axisIndex = axisIndex;
      if (joystick != null && axisIndex >= 0 && joystick.axisCount > axisIndex)
      {
        this.axis = ((IList<Controller.Axis>) joystick.Axes)[axisIndex];
        this.data = joystick.calibrationMap.GetAxis(axisIndex).GetData();
      }
      this.firstRun = true;
    }

    public void RecordMinMax()
    {
      if (this.axis == null)
        return;
      float valueRaw = this.axis.valueRaw;
      if (this.firstRun || (double) valueRaw < (double) this.data.min)
        this.data.min = valueRaw;
      if (this.firstRun || (double) valueRaw > (double) this.data.max)
        this.data.max = valueRaw;
      this.firstRun = false;
    }

    public void RecordZero()
    {
      if (this.axis == null)
        return;
      this.data.zero = this.axis.valueRaw;
    }

    public void Commit()
    {
      if (this.axis == null)
        return;
      AxisCalibration axis = this.joystick.calibrationMap.GetAxis(this.axisIndex);
      if (axis == null || (double) Mathf.Abs(this.data.max - this.data.min) < 0.1)
        return;
      axis.SetData(this.data);
    }
  }

  private class IndexedDictionary<TKey, TValue>
  {
    private List<Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<TKey, TValue>.Entry> list;

    public int Count => this.list.Count;

    public IndexedDictionary()
    {
      this.list = new List<Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<TKey, TValue>.Entry>();
    }

    public TValue this[int index] => this.list[index].value;

    public TValue Get(TKey key)
    {
      int index = this.IndexOfKey(key);
      if (index < 0)
        throw new Exception("Key does not exist!");
      return this.list[index].value;
    }

    public bool TryGet(TKey key, out TValue value)
    {
      value = default (TValue);
      int index = this.IndexOfKey(key);
      if (index < 0)
        return false;
      value = this.list[index].value;
      return true;
    }

    public void Add(TKey key, TValue value)
    {
      if (this.ContainsKey(key))
        throw new Exception($"Key {key.ToString()} is already in use!");
      this.list.Add(new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<TKey, TValue>.Entry(key, value));
    }

    public int IndexOfKey(TKey key)
    {
      int count = this.list.Count;
      for (int index = 0; index < count; ++index)
      {
        if (EqualityComparer<TKey>.Default.Equals(this.list[index].key, key))
          return index;
      }
      return -1;
    }

    public bool ContainsKey(TKey key)
    {
      int count = this.list.Count;
      for (int index = 0; index < count; ++index)
      {
        if (EqualityComparer<TKey>.Default.Equals(this.list[index].key, key))
          return true;
      }
      return false;
    }

    public void Clear() => this.list.Clear();

    private class Entry
    {
      public TKey key;
      public TValue value;

      public Entry(TKey key, TValue value)
      {
        this.key = key;
        this.value = value;
      }
    }
  }

  private enum LayoutElementSizeType
  {
    MinSize,
    PreferredSize,
  }

  private enum WindowType
  {
    None,
    ChooseJoystick,
    JoystickAssignmentConflict,
    ElementAssignment,
    ElementAssignmentPrePolling,
    ElementAssignmentPolling,
    ElementAssignmentResult,
    ElementAssignmentConflict,
    Calibration,
    CalibrateStep1,
    CalibrateStep2,
  }

  private class InputGrid
  {
    private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList list;
    private List<GameObject> groups;

    public InputGrid()
    {
      this.list = new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList();
      this.groups = new List<GameObject>();
    }

    public void AddMapCategory(int mapCategoryId) => this.list.AddMapCategory(mapCategoryId);

    public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
    {
      this.list.AddAction(mapCategoryId, action, axisRange);
    }

    public void AddActionCategory(int mapCategoryId, int actionCategoryId)
    {
      this.list.AddActionCategory(mapCategoryId, actionCategoryId);
    }

    public void AddInputFieldSet(
      int mapCategoryId,
      InputAction action,
      AxisRange axisRange,
      ControllerType controllerType,
      GameObject fieldSetContainer)
    {
      this.list.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, fieldSetContainer);
    }

    public void AddInputField(
      int mapCategoryId,
      InputAction action,
      AxisRange axisRange,
      ControllerType controllerType,
      int fieldIndex,
      Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField)
    {
      this.list.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField);
    }

    public void AddGroup(GameObject group) => this.groups.Add(group);

    public void AddActionLabel(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
    {
      this.list.AddActionLabel(mapCategoryId, actionId, axisRange, label);
    }

    public void AddActionCategoryLabel(
      int mapCategoryId,
      int actionCategoryId,
      Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
    {
      this.list.AddActionCategoryLabel(mapCategoryId, actionCategoryId, label);
    }

    public bool Contains(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int fieldIndex)
    {
      return this.list.Contains(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
    }

    public Rewired.UI.ControlMapper.ControlMapper.GUIInputField GetGUIInputField(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int fieldIndex)
    {
      return this.list.GetGUIInputField(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
    }

    public IEnumerable<Rewired.UI.ControlMapper.ControlMapper.InputActionSet> GetActionSets(
      int mapCategoryId)
    {
      return this.list.GetActionSets(mapCategoryId);
    }

    public void SetColumnHeight(int mapCategoryId, float height)
    {
      this.list.SetColumnHeight(mapCategoryId, height);
    }

    public float GetColumnHeight(int mapCategoryId) => this.list.GetColumnHeight(mapCategoryId);

    public void SetFieldsActive(int mapCategoryId, bool state)
    {
      this.list.SetFieldsActive(mapCategoryId, state);
    }

    public void SetFieldLabel(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int index,
      string label)
    {
      this.list.SetLabel(mapCategoryId, actionId, axisRange, controllerType, index, label);
    }

    public void PopulateField(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int controllerId,
      int index,
      int actionElementMapId,
      string label,
      bool invert)
    {
      this.list.PopulateField(mapCategoryId, actionId, axisRange, controllerType, controllerId, index, actionElementMapId, label, invert);
    }

    public void SetFixedFieldData(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int controllerId)
    {
      this.list.SetFixedFieldData(mapCategoryId, actionId, axisRange, controllerType, controllerId);
    }

    public void InitializeFields(int mapCategoryId) => this.list.InitializeFields(mapCategoryId);

    public void Show(int mapCategoryId) => this.list.Show(mapCategoryId);

    public void HideAll() => this.list.HideAll();

    public void ClearLabels(int mapCategoryId) => this.list.ClearLabels(mapCategoryId);

    private void ClearGroups()
    {
      for (int index = 0; index < this.groups.Count; ++index)
      {
        if (!((UnityEngine.Object) this.groups[index] == (UnityEngine.Object) null))
          UnityEngine.Object.Destroy((UnityEngine.Object) this.groups[index]);
      }
    }

    public void ClearAll()
    {
      this.ClearGroups();
      this.list.Clear();
    }
  }

  private class InputGridEntryList
  {
    private Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry> entries;

    public InputGridEntryList()
    {
      this.entries = new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry>();
    }

    public void AddMapCategory(int mapCategoryId)
    {
      if (mapCategoryId < 0 || this.entries.ContainsKey(mapCategoryId))
        return;
      this.entries.Add(mapCategoryId, new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry());
    }

    public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
    {
      this.AddActionEntry(mapCategoryId, action, axisRange);
    }

    private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry AddActionEntry(
      int mapCategoryId,
      InputAction action,
      AxisRange axisRange)
    {
      if (action == null)
        return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null;
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      return !this.entries.TryGet(mapCategoryId, out mapCategoryEntry) ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null : mapCategoryEntry.AddAction(action, axisRange);
    }

    public void AddActionLabel(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
        return;
      mapCategoryEntry.GetActionEntry(actionId, axisRange)?.SetLabel(label);
    }

    public void AddActionCategory(int mapCategoryId, int actionCategoryId)
    {
      this.AddActionCategoryEntry(mapCategoryId, actionCategoryId);
    }

    private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategoryEntry(
      int mapCategoryId,
      int actionCategoryId)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      return !this.entries.TryGet(mapCategoryId, out mapCategoryEntry) ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry) null : mapCategoryEntry.AddActionCategory(actionCategoryId);
    }

    public void AddActionCategoryLabel(
      int mapCategoryId,
      int actionCategoryId,
      Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
        return;
      mapCategoryEntry.GetActionCategoryEntry(actionCategoryId)?.SetLabel(label);
    }

    public void AddInputFieldSet(
      int mapCategoryId,
      InputAction action,
      AxisRange axisRange,
      ControllerType controllerType,
      GameObject fieldSetContainer)
    {
      this.GetActionEntry(mapCategoryId, action, axisRange)?.AddInputFieldSet(controllerType, fieldSetContainer);
    }

    public void AddInputField(
      int mapCategoryId,
      InputAction action,
      AxisRange axisRange,
      ControllerType controllerType,
      int fieldIndex,
      Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField)
    {
      this.GetActionEntry(mapCategoryId, action, axisRange)?.AddInputField(controllerType, fieldIndex, inputField);
    }

    public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange)
    {
      return this.GetActionEntry(mapCategoryId, actionId, axisRange) != null;
    }

    public bool Contains(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int fieldIndex)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
      return actionEntry != null && actionEntry.Contains(controllerType, fieldIndex);
    }

    public void SetColumnHeight(int mapCategoryId, float height)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
        return;
      mapCategoryEntry.columnHeight = height;
    }

    public float GetColumnHeight(int mapCategoryId)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      return !this.entries.TryGet(mapCategoryId, out mapCategoryEntry) ? 0.0f : mapCategoryEntry.columnHeight;
    }

    public Rewired.UI.ControlMapper.ControlMapper.GUIInputField GetGUIInputField(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int fieldIndex)
    {
      return this.GetActionEntry(mapCategoryId, actionId, axisRange)?.GetGUIInputField(controllerType, fieldIndex);
    }

    private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange)
    {
      if (actionId < 0)
        return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null;
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      return !this.entries.TryGet(mapCategoryId, out mapCategoryEntry) ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null : mapCategoryEntry.GetActionEntry(actionId, axisRange);
    }

    private Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(
      int mapCategoryId,
      InputAction action,
      AxisRange axisRange)
    {
      return action == null ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null : this.GetActionEntry(mapCategoryId, action.id, axisRange);
    }

    public IEnumerable<Rewired.UI.ControlMapper.ControlMapper.InputActionSet> GetActionSets(
      int mapCategoryId)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      if (this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
      {
        List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> list = mapCategoryEntry.actionList;
        int count = list != null ? list.Count : 0;
        for (int i = 0; i < count; ++i)
          yield return list[i].actionSet;
      }
    }

    public void SetFieldsActive(int mapCategoryId, bool state)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
        return;
      List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
      int count = actionList != null ? actionList.Count : 0;
      for (int index = 0; index < count; ++index)
        actionList[index].SetFieldsActive(state);
    }

    public void SetLabel(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int index,
      string label)
    {
      this.GetActionEntry(mapCategoryId, actionId, axisRange)?.SetFieldLabel(controllerType, index, label);
    }

    public void PopulateField(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int controllerId,
      int index,
      int actionElementMapId,
      string label,
      bool invert)
    {
      this.GetActionEntry(mapCategoryId, actionId, axisRange)?.PopulateField(controllerType, controllerId, index, actionElementMapId, label, invert);
    }

    public void SetFixedFieldData(
      int mapCategoryId,
      int actionId,
      AxisRange axisRange,
      ControllerType controllerType,
      int controllerId)
    {
      this.GetActionEntry(mapCategoryId, actionId, axisRange)?.SetFixedFieldData(controllerType, controllerId);
    }

    public void InitializeFields(int mapCategoryId)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
        return;
      List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
      int count = actionList != null ? actionList.Count : 0;
      for (int index = 0; index < count; ++index)
        actionList[index].Initialize();
    }

    public void Show(int mapCategoryId)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
        return;
      mapCategoryEntry.SetAllActive(true);
    }

    public void HideAll()
    {
      for (int index = 0; index < this.entries.Count; ++index)
        this.entries[index].SetAllActive(false);
    }

    public void ClearLabels(int mapCategoryId)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
      if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
        return;
      List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
      int count = actionList != null ? actionList.Count : 0;
      for (int index = 0; index < count; ++index)
        actionList[index].ClearLabels();
    }

    public void Clear() => this.entries.Clear();

    private class MapCategoryEntry
    {
      private List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> _actionList;
      private Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry> _actionCategoryList;
      private float _columnHeight;

      public List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry> actionList
      {
        get => this._actionList;
      }

      public Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry> actionCategoryList
      {
        get => this._actionCategoryList;
      }

      public float columnHeight
      {
        get => this._columnHeight;
        set => this._columnHeight = value;
      }

      public MapCategoryEntry()
      {
        this._actionList = new List<Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry>();
        this._actionCategoryList = new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry>();
      }

      public Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(
        int actionId,
        AxisRange axisRange)
      {
        int index = this.IndexOfActionEntry(actionId, axisRange);
        return index < 0 ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null : this._actionList[index];
      }

      public int IndexOfActionEntry(int actionId, AxisRange axisRange)
      {
        int count = this._actionList.Count;
        for (int index = 0; index < count; ++index)
        {
          if (this._actionList[index].Matches(actionId, axisRange))
            return index;
        }
        return -1;
      }

      public bool ContainsActionEntry(int actionId, AxisRange axisRange)
      {
        return this.IndexOfActionEntry(actionId, axisRange) >= 0;
      }

      public Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry AddAction(
        InputAction action,
        AxisRange axisRange)
      {
        if (action == null)
          return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null;
        if (this.ContainsActionEntry(action.id, axisRange))
          return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry) null;
        this._actionList.Add(new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionEntry(action, axisRange));
        return this._actionList[this._actionList.Count - 1];
      }

      public Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry GetActionCategoryEntry(
        int actionCategoryId)
      {
        return !this._actionCategoryList.ContainsKey(actionCategoryId) ? (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry) null : this._actionCategoryList.Get(actionCategoryId);
      }

      public Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategory(
        int actionCategoryId)
      {
        if (actionCategoryId < 0)
          return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry) null;
        if (this._actionCategoryList.ContainsKey(actionCategoryId))
          return (Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry) null;
        this._actionCategoryList.Add(actionCategoryId, new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.ActionCategoryEntry(actionCategoryId));
        return this._actionCategoryList.Get(actionCategoryId);
      }

      public void SetAllActive(bool state)
      {
        for (int index = 0; index < this._actionCategoryList.Count; ++index)
          this._actionCategoryList[index].SetActive(state);
        for (int index = 0; index < this._actionList.Count; ++index)
          this._actionList[index].SetActive(state);
      }
    }

    private class ActionEntry
    {
      private Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet> fieldSets;
      public Rewired.UI.ControlMapper.ControlMapper.GUILabel label;
      public readonly InputAction action;
      public readonly AxisRange axisRange;
      public readonly Rewired.UI.ControlMapper.ControlMapper.InputActionSet actionSet;

      public ActionEntry(InputAction action, AxisRange axisRange)
      {
        this.action = action;
        this.axisRange = axisRange;
        this.actionSet = new Rewired.UI.ControlMapper.ControlMapper.InputActionSet(action.id, axisRange);
        this.fieldSets = new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet>();
      }

      public void SetLabel(Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
      {
        this.label = label;
      }

      public bool Matches(int actionId, AxisRange axisRange)
      {
        return this.action.id == actionId && this.axisRange == axisRange;
      }

      public void AddInputFieldSet(ControllerType controllerType, GameObject fieldSetContainer)
      {
        if (this.fieldSets.ContainsKey((int) controllerType))
          return;
        this.fieldSets.Add((int) controllerType, new Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet(fieldSetContainer));
      }

      public void AddInputField(
        ControllerType controllerType,
        int fieldIndex,
        Rewired.UI.ControlMapper.ControlMapper.GUIInputField inputField)
      {
        if (!this.fieldSets.ContainsKey((int) controllerType))
          return;
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int) controllerType);
        if (fieldSet.fields.ContainsKey(fieldIndex))
          return;
        fieldSet.fields.Add(fieldIndex, inputField);
      }

      public Rewired.UI.ControlMapper.ControlMapper.GUIInputField GetGUIInputField(
        ControllerType controllerType,
        int fieldIndex)
      {
        if (!this.fieldSets.ContainsKey((int) controllerType))
          return (Rewired.UI.ControlMapper.ControlMapper.GUIInputField) null;
        return !this.fieldSets.Get((int) controllerType).fields.ContainsKey(fieldIndex) ? (Rewired.UI.ControlMapper.ControlMapper.GUIInputField) null : this.fieldSets.Get((int) controllerType).fields.Get(fieldIndex);
      }

      public bool Contains(ControllerType controllerType, int fieldId)
      {
        return this.fieldSets.ContainsKey((int) controllerType) && this.fieldSets.Get((int) controllerType).fields.ContainsKey(fieldId);
      }

      public void SetFieldLabel(ControllerType controllerType, int index, string label)
      {
        if (!this.fieldSets.ContainsKey((int) controllerType) || !this.fieldSets.Get((int) controllerType).fields.ContainsKey(index))
          return;
        this.fieldSets.Get((int) controllerType).fields.Get(index).SetLabel(label);
      }

      public void PopulateField(
        ControllerType controllerType,
        int controllerId,
        int index,
        int actionElementMapId,
        string label,
        bool invert)
      {
        if (!this.fieldSets.ContainsKey((int) controllerType) || !this.fieldSets.Get((int) controllerType).fields.ContainsKey(index))
          return;
        Rewired.UI.ControlMapper.ControlMapper.GUIInputField guiInputField = this.fieldSets.Get((int) controllerType).fields.Get(index);
        guiInputField.SetLabel(label);
        guiInputField.actionElementMapId = actionElementMapId;
        guiInputField.controllerId = controllerId;
        if (!guiInputField.hasToggle)
          return;
        guiInputField.toggle.SetInteractible(true, false);
        guiInputField.toggle.SetToggleState(invert);
        guiInputField.toggle.actionElementMapId = actionElementMapId;
      }

      public void SetFixedFieldData(ControllerType controllerType, int controllerId)
      {
        if (!this.fieldSets.ContainsKey((int) controllerType))
          return;
        Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int) controllerType);
        int count = fieldSet.fields.Count;
        for (int index = 0; index < count; ++index)
          fieldSet.fields[index].controllerId = controllerId;
      }

      public void Initialize()
      {
        for (int index1 = 0; index1 < this.fieldSets.Count; ++index1)
        {
          Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[index1];
          int count = fieldSet.fields.Count;
          for (int index2 = 0; index2 < count; ++index2)
          {
            Rewired.UI.ControlMapper.ControlMapper.GUIInputField field = fieldSet.fields[index2];
            if (field.hasToggle)
            {
              field.toggle.SetInteractible(false, false);
              field.toggle.SetToggleState(false);
              field.toggle.actionElementMapId = -1;
            }
            field.SetLabel("");
            field.actionElementMapId = -1;
            field.controllerId = -1;
          }
        }
      }

      public void SetActive(bool state)
      {
        if (this.label != null)
          this.label.SetActive(state);
        int count = this.fieldSets.Count;
        for (int index = 0; index < count; ++index)
          this.fieldSets[index].groupContainer.SetActive(state);
      }

      public void ClearLabels()
      {
        for (int index1 = 0; index1 < this.fieldSets.Count; ++index1)
        {
          Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[index1];
          int count = fieldSet.fields.Count;
          for (int index2 = 0; index2 < count; ++index2)
            fieldSet.fields[index2].SetLabel("");
        }
      }

      public void SetFieldsActive(bool state)
      {
        for (int index1 = 0; index1 < this.fieldSets.Count; ++index1)
        {
          Rewired.UI.ControlMapper.ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[index1];
          int count = fieldSet.fields.Count;
          for (int index2 = 0; index2 < count; ++index2)
          {
            Rewired.UI.ControlMapper.ControlMapper.GUIInputField field = fieldSet.fields[index2];
            field.SetInteractible(state, false);
            if (field.hasToggle && (!state || field.toggle.actionElementMapId >= 0))
              field.toggle.SetInteractible(state, false);
          }
        }
      }
    }

    private class FieldSet
    {
      public readonly GameObject groupContainer;
      public readonly Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.GUIInputField> fields;

      public FieldSet(GameObject groupContainer)
      {
        this.groupContainer = groupContainer;
        this.fields = new Rewired.UI.ControlMapper.ControlMapper.IndexedDictionary<int, Rewired.UI.ControlMapper.ControlMapper.GUIInputField>();
      }
    }

    private class ActionCategoryEntry
    {
      public readonly int actionCategoryId;
      public Rewired.UI.ControlMapper.ControlMapper.GUILabel label;

      public ActionCategoryEntry(int actionCategoryId) => this.actionCategoryId = actionCategoryId;

      public void SetLabel(Rewired.UI.ControlMapper.ControlMapper.GUILabel label)
      {
        this.label = label;
      }

      public void SetActive(bool state)
      {
        if (this.label == null)
          return;
        this.label.SetActive(state);
      }
    }
  }

  private class WindowManager
  {
    private List<Window> windows;
    private GameObject windowPrefab;
    private Transform parent;
    private GameObject fader;
    private int idCounter;

    public bool isWindowOpen
    {
      get
      {
        for (int index = this.windows.Count - 1; index >= 0; --index)
        {
          if (!((UnityEngine.Object) this.windows[index] == (UnityEngine.Object) null))
            return true;
        }
        return false;
      }
    }

    public Window topWindow
    {
      get
      {
        for (int index = this.windows.Count - 1; index >= 0; --index)
        {
          if (!((UnityEngine.Object) this.windows[index] == (UnityEngine.Object) null))
            return this.windows[index];
        }
        return (Window) null;
      }
    }

    public WindowManager(GameObject windowPrefab, GameObject faderPrefab, Transform parent)
    {
      this.windowPrefab = windowPrefab;
      this.parent = parent;
      this.windows = new List<Window>();
      this.fader = UnityEngine.Object.Instantiate<GameObject>(faderPrefab);
      this.fader.transform.SetParent(parent, false);
      this.fader.GetComponent<RectTransform>().localScale = (Vector3) Vector2.one;
      this.SetFaderActive(false);
    }

    public Window OpenWindow(string name, int width, int height)
    {
      Window window = this.InstantiateWindow(name, width, height);
      this.UpdateFader();
      return window;
    }

    public Window OpenWindow(GameObject windowPrefab, string name)
    {
      if ((UnityEngine.Object) windowPrefab == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Rewired Control Mapper: Window Prefab is null!");
        return (Window) null;
      }
      Window window = this.InstantiateWindow(name, windowPrefab);
      this.UpdateFader();
      return window;
    }

    public void CloseTop()
    {
      for (int index = this.windows.Count - 1; index >= 0; --index)
      {
        if ((UnityEngine.Object) this.windows[index] == (UnityEngine.Object) null)
        {
          this.windows.RemoveAt(index);
        }
        else
        {
          this.DestroyWindow(this.windows[index]);
          this.windows.RemoveAt(index);
          break;
        }
      }
      this.UpdateFader();
    }

    public void CloseWindow(int windowId) => this.CloseWindow(this.GetWindow(windowId));

    public void CloseWindow(Window window)
    {
      if ((UnityEngine.Object) window == (UnityEngine.Object) null)
        return;
      for (int index = this.windows.Count - 1; index >= 0; --index)
      {
        if ((UnityEngine.Object) this.windows[index] == (UnityEngine.Object) null)
          this.windows.RemoveAt(index);
        else if (!((UnityEngine.Object) this.windows[index] != (UnityEngine.Object) window))
        {
          this.DestroyWindow(this.windows[index]);
          this.windows.RemoveAt(index);
          break;
        }
      }
      this.UpdateFader();
      this.FocusTopWindow();
    }

    public void CloseAll()
    {
      this.SetFaderActive(false);
      for (int index = this.windows.Count - 1; index >= 0; --index)
      {
        if ((UnityEngine.Object) this.windows[index] == (UnityEngine.Object) null)
        {
          this.windows.RemoveAt(index);
        }
        else
        {
          this.DestroyWindow(this.windows[index]);
          this.windows.RemoveAt(index);
        }
      }
      this.UpdateFader();
    }

    public void CancelAll()
    {
      if (!this.isWindowOpen)
        return;
      for (int index = this.windows.Count - 1; index >= 0; --index)
      {
        if (!((UnityEngine.Object) this.windows[index] == (UnityEngine.Object) null))
          this.windows[index].Cancel();
      }
      this.CloseAll();
    }

    public Window GetWindow(int windowId)
    {
      if (windowId < 0)
        return (Window) null;
      for (int index = this.windows.Count - 1; index >= 0; --index)
      {
        if (!((UnityEngine.Object) this.windows[index] == (UnityEngine.Object) null) && this.windows[index].id == windowId)
          return this.windows[index];
      }
      return (Window) null;
    }

    public bool IsFocused(int windowId)
    {
      return windowId >= 0 && !((UnityEngine.Object) this.topWindow == (UnityEngine.Object) null) && this.topWindow.id == windowId;
    }

    public void Focus(int windowId) => this.Focus(this.GetWindow(windowId));

    public void Focus(Window window)
    {
      if ((UnityEngine.Object) window == (UnityEngine.Object) null)
        return;
      window.TakeInputFocus();
      this.DefocusOtherWindows(window.id);
    }

    private void DefocusOtherWindows(int focusedWindowId)
    {
      if (focusedWindowId < 0)
        return;
      for (int index = this.windows.Count - 1; index >= 0; --index)
      {
        if (!((UnityEngine.Object) this.windows[index] == (UnityEngine.Object) null) && this.windows[index].id != focusedWindowId)
          this.windows[index].Disable();
      }
    }

    private void UpdateFader()
    {
      if (!this.isWindowOpen)
      {
        this.SetFaderActive(false);
      }
      else
      {
        if ((UnityEngine.Object) this.topWindow.transform.parent == (UnityEngine.Object) null)
          return;
        this.SetFaderActive(true);
        this.fader.transform.SetAsLastSibling();
        this.fader.transform.SetSiblingIndex(this.topWindow.transform.GetSiblingIndex());
      }
    }

    private void FocusTopWindow()
    {
      if ((UnityEngine.Object) this.topWindow == (UnityEngine.Object) null)
        return;
      this.topWindow.TakeInputFocus();
    }

    private void SetFaderActive(bool state) => this.fader.SetActive(state);

    private Window InstantiateWindow(string name, int width, int height)
    {
      if (string.IsNullOrEmpty(name))
        name = "Window";
      GameObject gameObject = UITools.InstantiateGUIObject<Window>(this.windowPrefab, this.parent, name);
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return (Window) null;
      Window component = gameObject.GetComponent<Window>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
        this.windows.Add(component);
        component.SetSize(width, height);
      }
      return component;
    }

    private Window InstantiateWindow(string name, GameObject windowPrefab)
    {
      if (string.IsNullOrEmpty(name))
        name = "Window";
      if ((UnityEngine.Object) windowPrefab == (UnityEngine.Object) null)
        return (Window) null;
      GameObject gameObject = UITools.InstantiateGUIObject<Window>(windowPrefab, this.parent, name);
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return (Window) null;
      Window component = gameObject.GetComponent<Window>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
        this.windows.Add(component);
      }
      return component;
    }

    private void DestroyWindow(Window window)
    {
      if ((UnityEngine.Object) window == (UnityEngine.Object) null)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) window.gameObject);
    }

    private int GetNewId()
    {
      int idCounter = this.idCounter;
      ++this.idCounter;
      return idCounter;
    }

    public void ClearCompletely()
    {
      this.CloseAll();
      if (!((UnityEngine.Object) this.fader != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.fader);
    }
  }
}
