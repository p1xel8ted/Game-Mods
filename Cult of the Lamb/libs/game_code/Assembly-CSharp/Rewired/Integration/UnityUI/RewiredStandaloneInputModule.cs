// Decompiled with JetBrains decompiler
// Type: Rewired.Integration.UnityUI.RewiredStandaloneInputModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.UI;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

#nullable disable
namespace Rewired.Integration.UnityUI;

[AddComponentMenu("Rewired/Rewired Standalone Input Module")]
public sealed class RewiredStandaloneInputModule : RewiredPointerInputModule
{
  public const string DEFAULT_ACTION_MOVE_HORIZONTAL = "UIHorizontal";
  public const string DEFAULT_ACTION_MOVE_VERTICAL = "UIVertical";
  public const string DEFAULT_ACTION_SUBMIT = "UISubmit";
  public const string DEFAULT_ACTION_CANCEL = "UICancel";
  [Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Player ids, etc.")]
  [SerializeField]
  public InputManager_Base rewiredInputManager;
  [SerializeField]
  [Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")]
  public bool useAllRewiredGamePlayers;
  [SerializeField]
  [Tooltip("Allow the Rewired System Player to control the UI.")]
  public bool useRewiredSystemPlayer;
  [SerializeField]
  [Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")]
  public int[] rewiredPlayerIds = new int[1];
  [SerializeField]
  [Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")]
  public bool usePlayingPlayersOnly;
  [SerializeField]
  [Tooltip("Player Mice allowed to interact with the UI. Each Player that owns a Player Mouse must also be allowed to control the UI or the Player Mouse will not function.")]
  public List<Rewired.Components.PlayerMouse> playerMice = new List<Rewired.Components.PlayerMouse>();
  [SerializeField]
  [Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
  public bool moveOneElementPerAxisPress;
  [SerializeField]
  [Tooltip("If enabled, Action Ids will be used to set the Actions. If disabled, string names will be used to set the Actions.")]
  public bool setActionsById;
  [SerializeField]
  [Tooltip("Id of the horizontal Action for movement (if axis events are used).")]
  public int horizontalActionId = -1;
  [SerializeField]
  [Tooltip("Id of the vertical Action for movement (if axis events are used).")]
  public int verticalActionId = -1;
  [SerializeField]
  [Tooltip("Id of the Action used to submit.")]
  public int submitActionId = -1;
  [SerializeField]
  [Tooltip("Id of the Action used to cancel.")]
  public int cancelActionId = -1;
  [SerializeField]
  [Tooltip("Name of the horizontal axis for movement (if axis events are used).")]
  public string m_HorizontalAxis = "UIHorizontal";
  [SerializeField]
  [Tooltip("Name of the vertical axis for movement (if axis events are used).")]
  public string m_VerticalAxis = "UIVertical";
  [SerializeField]
  [Tooltip("Name of the action used to submit.")]
  public string m_SubmitButton = "UISubmit";
  [SerializeField]
  [Tooltip("Name of the action used to cancel.")]
  public string m_CancelButton = "UICancel";
  [SerializeField]
  [Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
  public float m_InputActionsPerSecond = 10f;
  [SerializeField]
  [Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
  public float m_RepeatDelay;
  [SerializeField]
  [Tooltip("Allows the mouse to be used to select elements.")]
  public bool m_allowMouseInput = true;
  [SerializeField]
  [Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
  public bool m_allowMouseInputIfTouchSupported = true;
  [SerializeField]
  [Tooltip("Allows touch input to be used to select elements.")]
  public bool m_allowTouchInput = true;
  [SerializeField]
  [Tooltip("Deselects the current selection on mouse/touch click when the pointer is not over a selectable object.")]
  public bool m_deselectIfBackgroundClicked = true;
  [SerializeField]
  [Tooltip("Deselects the current selection on mouse/touch click before selecting the next object.")]
  public bool m_deselectBeforeSelecting = true;
  [SerializeField]
  [FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
  [Tooltip("Forces the module to always be active.")]
  public bool m_ForceModuleActive;
  [NonSerialized]
  public int[] playerIds;
  public bool recompiling;
  [NonSerialized]
  public bool isTouchSupported;
  [NonSerialized]
  public double m_PrevActionTime;
  [NonSerialized]
  public Vector2 m_LastMoveVector;
  [NonSerialized]
  public int m_ConsecutiveMoveCount;
  [NonSerialized]
  public bool m_HasFocus = true;

  public InputManager_Base RewiredInputManager
  {
    get => this.rewiredInputManager;
    set => this.rewiredInputManager = value;
  }

  public bool UseAllRewiredGamePlayers
  {
    get => this.useAllRewiredGamePlayers;
    set
    {
      int num = value != this.useAllRewiredGamePlayers ? 1 : 0;
      this.useAllRewiredGamePlayers = value;
      if (num == 0)
        return;
      this.SetupRewiredVars();
    }
  }

  public bool UseRewiredSystemPlayer
  {
    get => this.useRewiredSystemPlayer;
    set
    {
      int num = value != this.useRewiredSystemPlayer ? 1 : 0;
      this.useRewiredSystemPlayer = value;
      if (num == 0)
        return;
      this.SetupRewiredVars();
    }
  }

  public int[] RewiredPlayerIds
  {
    get => (int[]) this.rewiredPlayerIds.Clone();
    set
    {
      this.rewiredPlayerIds = value != null ? (int[]) value.Clone() : new int[0];
      this.SetupRewiredVars();
    }
  }

  public bool UsePlayingPlayersOnly
  {
    get => this.usePlayingPlayersOnly;
    set => this.usePlayingPlayersOnly = value;
  }

  public List<Rewired.Components.PlayerMouse> PlayerMice
  {
    get => new List<Rewired.Components.PlayerMouse>((IEnumerable<Rewired.Components.PlayerMouse>) this.playerMice);
    set
    {
      if (value == null)
      {
        this.playerMice = new List<Rewired.Components.PlayerMouse>();
        this.SetupRewiredVars();
      }
      else
      {
        this.playerMice = new List<Rewired.Components.PlayerMouse>((IEnumerable<Rewired.Components.PlayerMouse>) value);
        this.SetupRewiredVars();
      }
    }
  }

  public bool MoveOneElementPerAxisPress
  {
    get => this.moveOneElementPerAxisPress;
    set => this.moveOneElementPerAxisPress = value;
  }

  public bool allowMouseInput
  {
    get => this.m_allowMouseInput;
    set => this.m_allowMouseInput = value;
  }

  public bool allowMouseInputIfTouchSupported
  {
    get => this.m_allowMouseInputIfTouchSupported;
    set => this.m_allowMouseInputIfTouchSupported = value;
  }

  public bool allowTouchInput
  {
    get => this.m_allowTouchInput;
    set => this.m_allowTouchInput = value;
  }

  public bool deselectIfBackgroundClicked
  {
    get => this.m_deselectIfBackgroundClicked;
    set => this.m_deselectIfBackgroundClicked = value;
  }

  public bool deselectBeforeSelecting
  {
    get => this.m_deselectBeforeSelecting;
    set => this.m_deselectBeforeSelecting = value;
  }

  public bool SetActionsById
  {
    get => this.setActionsById;
    set
    {
      if (this.setActionsById == value)
        return;
      this.setActionsById = value;
      this.SetupRewiredVars();
    }
  }

  public int HorizontalActionId
  {
    get => this.horizontalActionId;
    set
    {
      if (value == this.horizontalActionId)
        return;
      this.horizontalActionId = value;
      if (!ReInput.isReady)
        return;
      this.m_HorizontalAxis = ReInput.mapping.GetAction(value) != null ? ReInput.mapping.GetAction(value).name : string.Empty;
    }
  }

  public int VerticalActionId
  {
    get => this.verticalActionId;
    set
    {
      if (value == this.verticalActionId)
        return;
      this.verticalActionId = value;
      if (!ReInput.isReady)
        return;
      this.m_VerticalAxis = ReInput.mapping.GetAction(value) != null ? ReInput.mapping.GetAction(value).name : string.Empty;
    }
  }

  public int SubmitActionId
  {
    get => this.submitActionId;
    set
    {
      if (value == this.submitActionId)
        return;
      this.submitActionId = value;
      if (!ReInput.isReady)
        return;
      this.m_SubmitButton = ReInput.mapping.GetAction(value) != null ? ReInput.mapping.GetAction(value).name : string.Empty;
    }
  }

  public int CancelActionId
  {
    get => this.cancelActionId;
    set
    {
      if (value == this.cancelActionId)
        return;
      this.cancelActionId = value;
      if (!ReInput.isReady)
        return;
      this.m_CancelButton = ReInput.mapping.GetAction(value) != null ? ReInput.mapping.GetAction(value).name : string.Empty;
    }
  }

  public override bool isMouseSupported
  {
    get
    {
      if (!base.isMouseSupported || !this.m_allowMouseInput)
        return false;
      return !this.isTouchSupported || this.m_allowMouseInputIfTouchSupported;
    }
  }

  public bool isTouchAllowed => this.m_allowTouchInput;

  [Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")]
  public bool allowActivationOnMobileDevice
  {
    get => this.m_ForceModuleActive;
    set => this.m_ForceModuleActive = value;
  }

  public bool forceModuleActive
  {
    get => this.m_ForceModuleActive;
    set => this.m_ForceModuleActive = value;
  }

  public float inputActionsPerSecond
  {
    get => this.m_InputActionsPerSecond;
    set => this.m_InputActionsPerSecond = value;
  }

  public float repeatDelay
  {
    get => this.m_RepeatDelay;
    set => this.m_RepeatDelay = value;
  }

  public string horizontalAxis
  {
    get => this.m_HorizontalAxis;
    set
    {
      if (this.m_HorizontalAxis == value)
        return;
      this.m_HorizontalAxis = value;
      if (!ReInput.isReady)
        return;
      this.horizontalActionId = ReInput.mapping.GetActionId(value);
    }
  }

  public string verticalAxis
  {
    get => this.m_VerticalAxis;
    set
    {
      if (this.m_VerticalAxis == value)
        return;
      this.m_VerticalAxis = value;
      if (!ReInput.isReady)
        return;
      this.verticalActionId = ReInput.mapping.GetActionId(value);
    }
  }

  public string submitButton
  {
    get => this.m_SubmitButton;
    set
    {
      if (this.m_SubmitButton == value)
        return;
      this.m_SubmitButton = value;
      if (!ReInput.isReady)
        return;
      this.submitActionId = ReInput.mapping.GetActionId(value);
    }
  }

  public string cancelButton
  {
    get => this.m_CancelButton;
    set
    {
      if (this.m_CancelButton == value)
        return;
      this.m_CancelButton = value;
      if (!ReInput.isReady)
        return;
      this.cancelActionId = ReInput.mapping.GetActionId(value);
    }
  }

  public override void Awake()
  {
    base.Awake();
    this.isTouchSupported = this.defaultTouchInputSource.touchSupported;
    ReInput.InitializedEvent += (Action) new Action(this.OnRewiredInitialized);
    this.InitializeRewired();
  }

  public override void UpdateModule()
  {
    this.CheckEditorRecompile();
    if (this.recompiling || !ReInput.isReady || this.m_HasFocus)
      return;
    this.ShouldIgnoreEventsOnNoFocus();
  }

  public override bool IsModuleSupported() => true;

  public override bool ShouldActivateModule()
  {
    if (!base.ShouldActivateModule() || this.recompiling || !ReInput.isReady)
      return false;
    bool flag1 = this.m_ForceModuleActive;
    for (int index = 0; index < this.playerIds.Length; ++index)
    {
      Player player = ReInput.players.GetPlayer(this.playerIds[index]);
      if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
      {
        bool flag2 = flag1 | this.GetButtonDown(player, this.submitActionId) | this.GetButtonDown(player, this.cancelActionId);
        flag1 = !this.moveOneElementPerAxisPress ? flag2 | !Mathf.Approximately(this.GetAxis(player, this.horizontalActionId), 0.0f) | !Mathf.Approximately(this.GetAxis(player, this.verticalActionId), 0.0f) : ((((flag2 ? 1 : 0) | (this.GetButtonDown(player, this.horizontalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.horizontalActionId) ? 1 : 0))) != 0 ? 1 : 0) | (this.GetButtonDown(player, this.verticalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.verticalActionId) ? 1 : 0))) != 0;
      }
    }
    if (this.isMouseSupported)
      flag1 = flag1 | this.DidAnyMouseMove() | this.GetMouseButtonDownOnAnyMouse(0);
    if (this.isTouchAllowed)
    {
      for (int index = 0; index < this.defaultTouchInputSource.touchCount; ++index)
      {
        Touch touch = this.defaultTouchInputSource.GetTouch(index);
        flag1 = ((flag1 ? 1 : 0) | (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved ? 1 : (touch.phase == TouchPhase.Stationary ? 1 : 0))) != 0;
      }
    }
    return flag1;
  }

  public override void ActivateModule()
  {
    if (!this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus())
      return;
    base.ActivateModule();
    GameObject selectedGameObject = this.eventSystem.currentSelectedGameObject;
    if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) null)
      selectedGameObject = this.eventSystem.firstSelectedGameObject;
    this.eventSystem.SetSelectedGameObject(selectedGameObject, this.GetBaseEventData());
  }

  public override void DeactivateModule()
  {
    base.DeactivateModule();
    this.ClearSelection();
  }

  public override void Process()
  {
    if (!ReInput.isReady || !this.m_HasFocus && this.ShouldIgnoreEventsOnNoFocus() || !this.enabled || !this.gameObject.activeInHierarchy)
      return;
    bool selectedObject = this.SendUpdateEventToSelectedObject();
    if (this.eventSystem.sendNavigationEvents)
    {
      if (!selectedObject)
        selectedObject |= this.SendMoveEventToSelectedObject();
      if (!selectedObject)
        this.SendSubmitEventToSelectedObject();
    }
    if (this.ProcessTouchEvents() || !this.isMouseSupported)
      return;
    this.ProcessMouseEvents();
  }

  public bool ProcessTouchEvents()
  {
    if (!this.isTouchAllowed)
      return false;
    for (int index = 0; index < this.defaultTouchInputSource.touchCount; ++index)
    {
      Touch touch = this.defaultTouchInputSource.GetTouch(index);
      if (touch.type != TouchType.Indirect)
      {
        bool pressed;
        bool released;
        PlayerPointerEventData pointerEventData = this.GetTouchPointerEventData(0, 0, touch, out pressed, out released);
        this.ProcessTouchPress((PointerEventData) pointerEventData, pressed, released);
        if (!released)
        {
          this.ProcessMove(pointerEventData);
          this.ProcessDrag(pointerEventData);
        }
        else
          this.RemovePointerData(pointerEventData);
      }
    }
    return this.defaultTouchInputSource.touchCount > 0;
  }

  public void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
  {
    GameObject gameObject1 = pointerEvent.pointerCurrentRaycast.gameObject;
    if (pressed)
    {
      pointerEvent.eligibleForClick = true;
      pointerEvent.delta = Vector2.zero;
      pointerEvent.dragging = false;
      pointerEvent.useDragThreshold = true;
      pointerEvent.pressPosition = pointerEvent.position;
      pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
      this.HandleMouseTouchDeselectionOnSelectionChanged(gameObject1, (BaseEventData) pointerEvent);
      if ((UnityEngine.Object) pointerEvent.pointerEnter != (UnityEngine.Object) gameObject1)
      {
        this.HandlePointerExitAndEnter(pointerEvent, gameObject1);
        pointerEvent.pointerEnter = gameObject1;
      }
      GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) pointerEvent, ExecuteEvents.pointerDownHandler);
      if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
        gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
      double unscaledTime = ReInput.time.unscaledTime;
      if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) pointerEvent.lastPress)
      {
        if (unscaledTime - (double) pointerEvent.clickTime < 0.30000001192092896)
          ++pointerEvent.clickCount;
        else
          pointerEvent.clickCount = 1;
        pointerEvent.clickTime = (float) unscaledTime;
      }
      else
        pointerEvent.clickCount = 1;
      pointerEvent.pointerPress = gameObject2;
      pointerEvent.rawPointerPress = gameObject1;
      pointerEvent.clickTime = (float) unscaledTime;
      pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
      if ((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null)
        ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.initializePotentialDrag);
    }
    if (!released)
      return;
    ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData) pointerEvent, ExecuteEvents.pointerUpHandler);
    GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
    if ((UnityEngine.Object) pointerEvent.pointerPress == (UnityEngine.Object) eventHandler && pointerEvent.eligibleForClick)
      ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, (BaseEventData) pointerEvent, ExecuteEvents.pointerClickHandler);
    else if ((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null && pointerEvent.dragging)
      ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) pointerEvent, ExecuteEvents.dropHandler);
    pointerEvent.eligibleForClick = false;
    pointerEvent.pointerPress = (GameObject) null;
    pointerEvent.rawPointerPress = (GameObject) null;
    if ((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null && pointerEvent.dragging)
      ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.endDragHandler);
    pointerEvent.dragging = false;
    pointerEvent.pointerDrag = (GameObject) null;
    if ((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null)
      ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.endDragHandler);
    pointerEvent.pointerDrag = (GameObject) null;
    ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, (BaseEventData) pointerEvent, ExecuteEvents.pointerExitHandler);
    pointerEvent.pointerEnter = (GameObject) null;
  }

  public bool SendSubmitEventToSelectedObject()
  {
    if ((UnityEngine.Object) this.eventSystem.currentSelectedGameObject == (UnityEngine.Object) null || this.recompiling)
      return false;
    BaseEventData baseEventData = this.GetBaseEventData();
    for (int index = 0; index < this.playerIds.Length; ++index)
    {
      Player player = ReInput.players.GetPlayer(this.playerIds[index]);
      if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
      {
        if (this.GetButtonDown(player, this.submitActionId))
        {
          ExecuteEvents.Execute<ISubmitHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
          break;
        }
        if (this.GetButtonDown(player, this.cancelActionId))
        {
          ExecuteEvents.Execute<ICancelHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
          break;
        }
      }
    }
    return baseEventData.used;
  }

  public Vector2 GetRawMoveVector()
  {
    if (this.recompiling)
      return Vector2.zero;
    Vector2 zero = Vector2.zero;
    for (int index = 0; index < this.playerIds.Length; ++index)
    {
      Player player = ReInput.players.GetPlayer(this.playerIds[index]);
      if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
      {
        float a1 = this.GetAxis(player, this.horizontalActionId);
        float a2 = this.GetAxis(player, this.verticalActionId);
        if (Mathf.Approximately(a1, 0.0f))
          a1 = 0.0f;
        if (Mathf.Approximately(a2, 0.0f))
          a2 = 0.0f;
        if (this.moveOneElementPerAxisPress)
        {
          if (this.GetButtonDown(player, this.horizontalActionId) && (double) a1 > 0.0)
            ++zero.x;
          if (this.GetNegativeButtonDown(player, this.horizontalActionId) && (double) a1 < 0.0)
            --zero.x;
          if (this.GetButtonDown(player, this.verticalActionId) && (double) a2 > 0.0)
            ++zero.y;
          if (this.GetNegativeButtonDown(player, this.verticalActionId) && (double) a2 < 0.0)
            --zero.y;
        }
        else
        {
          if (this.GetButton(player, this.horizontalActionId) && (double) a1 > 0.0)
            ++zero.x;
          if (this.GetNegativeButton(player, this.horizontalActionId) && (double) a1 < 0.0)
            --zero.x;
          if (this.GetButton(player, this.verticalActionId) && (double) a2 > 0.0)
            ++zero.y;
          if (this.GetNegativeButton(player, this.verticalActionId) && (double) a2 < 0.0)
            --zero.y;
        }
      }
    }
    return zero;
  }

  public bool SendMoveEventToSelectedObject()
  {
    if (this.recompiling)
      return false;
    double unscaledTime = ReInput.time.unscaledTime;
    Vector2 rawMoveVector = this.GetRawMoveVector();
    if (Mathf.Approximately(rawMoveVector.x, 0.0f) && Mathf.Approximately(rawMoveVector.y, 0.0f))
    {
      this.m_ConsecutiveMoveCount = 0;
      return false;
    }
    bool flag1 = (double) Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0.0;
    bool downHorizontal;
    bool downVertical;
    this.CheckButtonOrKeyMovement(out downHorizontal, out downVertical);
    AxisEventData eventData = (AxisEventData) null;
    bool flag2 = downHorizontal | downVertical;
    if (flag2)
    {
      eventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.0f);
      MoveDirection moveDir = eventData.moveDir;
      flag2 = ((moveDir == MoveDirection.Up ? 1 : (moveDir == MoveDirection.Down ? 1 : 0)) & (downVertical ? 1 : 0)) != 0 || ((moveDir == MoveDirection.Left ? 1 : (moveDir == MoveDirection.Right ? 1 : 0)) & (downHorizontal ? 1 : 0)) != 0;
    }
    if (!flag2)
      flag2 = (double) this.m_RepeatDelay <= 0.0 ? unscaledTime > this.m_PrevActionTime + 1.0 / (double) this.m_InputActionsPerSecond : (!flag1 || this.m_ConsecutiveMoveCount != 1 ? unscaledTime > this.m_PrevActionTime + 1.0 / (double) this.m_InputActionsPerSecond : unscaledTime > this.m_PrevActionTime + (double) this.m_RepeatDelay);
    if (!flag2)
      return false;
    if (eventData == null)
      eventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.0f);
    if (eventData.moveDir != MoveDirection.None)
    {
      ExecuteEvents.Execute<IMoveHandler>(this.eventSystem.currentSelectedGameObject, (BaseEventData) eventData, ExecuteEvents.moveHandler);
      if (!flag1)
        this.m_ConsecutiveMoveCount = 0;
      if (this.m_ConsecutiveMoveCount == 0 || !(downHorizontal | downVertical))
        ++this.m_ConsecutiveMoveCount;
      this.m_PrevActionTime = unscaledTime;
      this.m_LastMoveVector = rawMoveVector;
    }
    else
      this.m_ConsecutiveMoveCount = 0;
    return eventData.used;
  }

  public void CheckButtonOrKeyMovement(out bool downHorizontal, out bool downVertical)
  {
    downHorizontal = false;
    downVertical = false;
    for (int index = 0; index < this.playerIds.Length; ++index)
    {
      Player player = ReInput.players.GetPlayer(this.playerIds[index]);
      if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
      {
        downHorizontal = ((downHorizontal ? 1 : 0) | (this.GetButtonDown(player, this.horizontalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.horizontalActionId) ? 1 : 0))) != 0;
        downVertical = ((downVertical ? 1 : 0) | (this.GetButtonDown(player, this.verticalActionId) ? 1 : (this.GetNegativeButtonDown(player, this.verticalActionId) ? 1 : 0))) != 0;
      }
    }
  }

  public void ProcessMouseEvents()
  {
    for (int index = 0; index < this.playerIds.Length; ++index)
    {
      Player player = ReInput.players.GetPlayer(this.playerIds[index]);
      if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
      {
        int inputSourceCount = this.GetMouseInputSourceCount(this.playerIds[index]);
        for (int pointerIndex = 0; pointerIndex < inputSourceCount; ++pointerIndex)
          this.ProcessMouseEvent(this.playerIds[index], pointerIndex);
      }
    }
  }

  public void ProcessMouseEvent(int playerId, int pointerIndex)
  {
    RewiredPointerInputModule.MouseState pointerEventData = this.GetMousePointerEventData(playerId, pointerIndex);
    if (pointerEventData == null)
      return;
    RewiredPointerInputModule.MouseButtonEventData eventData = pointerEventData.GetButtonState(0).eventData;
    this.ProcessMousePress(eventData);
    this.ProcessMove(eventData.buttonData);
    this.ProcessDrag(eventData.buttonData);
    this.ProcessMousePress(pointerEventData.GetButtonState(1).eventData);
    this.ProcessDrag(pointerEventData.GetButtonState(1).eventData.buttonData);
    this.ProcessMousePress(pointerEventData.GetButtonState(2).eventData);
    this.ProcessDrag(pointerEventData.GetButtonState(2).eventData.buttonData);
    IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, pointerIndex);
    if (mouseInputSource == null)
      return;
    for (int button = 3; button < mouseInputSource.buttonCount; ++button)
    {
      this.ProcessMousePress(pointerEventData.GetButtonState(button).eventData);
      this.ProcessDrag(pointerEventData.GetButtonState(button).eventData.buttonData);
    }
    if (Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
      return;
    ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), (BaseEventData) eventData.buttonData, ExecuteEvents.scrollHandler);
  }

  public bool SendUpdateEventToSelectedObject()
  {
    if ((UnityEngine.Object) this.eventSystem.currentSelectedGameObject == (UnityEngine.Object) null)
      return false;
    BaseEventData baseEventData = this.GetBaseEventData();
    ExecuteEvents.Execute<IUpdateSelectedHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
    return baseEventData.used;
  }

  public void ProcessMousePress(
    RewiredPointerInputModule.MouseButtonEventData data)
  {
    PlayerPointerEventData buttonData = data.buttonData;
    if (this.GetMouseInputSource(buttonData.playerId, buttonData.inputSourceIndex) == null)
      return;
    GameObject gameObject1 = buttonData.pointerCurrentRaycast.gameObject;
    if (data.PressedThisFrame())
    {
      buttonData.eligibleForClick = true;
      buttonData.delta = Vector2.zero;
      buttonData.dragging = false;
      buttonData.useDragThreshold = true;
      buttonData.pressPosition = buttonData.position;
      buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
      this.HandleMouseTouchDeselectionOnSelectionChanged(gameObject1, (BaseEventData) buttonData);
      GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.pointerDownHandler);
      if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
        gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
      double unscaledTime = ReInput.time.unscaledTime;
      if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) buttonData.lastPress)
      {
        if (unscaledTime - (double) buttonData.clickTime < 0.30000001192092896)
          ++buttonData.clickCount;
        else
          buttonData.clickCount = 1;
        buttonData.clickTime = (float) unscaledTime;
      }
      else
        buttonData.clickCount = 1;
      buttonData.pointerPress = gameObject2;
      buttonData.rawPointerPress = gameObject1;
      buttonData.clickTime = (float) unscaledTime;
      buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
      if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null)
        ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.initializePotentialDrag);
    }
    if (!data.ReleasedThisFrame())
      return;
    ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerUpHandler);
    GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
    if ((UnityEngine.Object) buttonData.pointerPress == (UnityEngine.Object) eventHandler && buttonData.eligibleForClick)
      ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerClickHandler);
    else if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
      ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.dropHandler);
    buttonData.eligibleForClick = false;
    buttonData.pointerPress = (GameObject) null;
    buttonData.rawPointerPress = (GameObject) null;
    if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
      ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.endDragHandler);
    buttonData.dragging = false;
    buttonData.pointerDrag = (GameObject) null;
    if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) buttonData.pointerEnter))
      return;
    this.HandlePointerExitAndEnter((PointerEventData) buttonData, (GameObject) null);
    this.HandlePointerExitAndEnter((PointerEventData) buttonData, gameObject1);
  }

  public void HandleMouseTouchDeselectionOnSelectionChanged(
    GameObject currentOverGo,
    BaseEventData pointerEvent)
  {
    if (this.m_deselectIfBackgroundClicked && this.m_deselectBeforeSelecting)
    {
      this.DeselectIfSelectionChanged(currentOverGo, pointerEvent);
    }
    else
    {
      GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
      if (this.m_deselectIfBackgroundClicked)
      {
        if (!((UnityEngine.Object) eventHandler != (UnityEngine.Object) this.eventSystem.currentSelectedGameObject) || !((UnityEngine.Object) eventHandler != (UnityEngine.Object) null))
          return;
        this.eventSystem.SetSelectedGameObject((GameObject) null, pointerEvent);
      }
      else
      {
        if (!this.m_deselectBeforeSelecting || !((UnityEngine.Object) eventHandler != (UnityEngine.Object) null) || !((UnityEngine.Object) eventHandler != (UnityEngine.Object) this.eventSystem.currentSelectedGameObject))
          return;
        this.eventSystem.SetSelectedGameObject((GameObject) null, pointerEvent);
      }
    }
  }

  public void OnApplicationFocus(bool hasFocus) => this.m_HasFocus = hasFocus;

  public bool ShouldIgnoreEventsOnNoFocus()
  {
    return !ReInput.isReady || ReInput.configuration.ignoreInputWhenAppNotInFocus;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    ReInput.InitializedEvent -= (Action) new Action(this.OnRewiredInitialized);
    ReInput.ShutDownEvent -= (Action) new Action(this.OnRewiredShutDown);
    ReInput.EditorRecompileEvent -= (Action) new Action(this.OnEditorRecompile);
  }

  public override bool IsDefaultPlayer(int playerId)
  {
    if (this.playerIds == null || !ReInput.isReady)
      return false;
    for (int index1 = 0; index1 < 3; ++index1)
    {
      for (int index2 = 0; index2 < this.playerIds.Length; ++index2)
      {
        Player player = ReInput.players.GetPlayer(this.playerIds[index2]);
        if (player != null && (index1 >= 1 || !this.usePlayingPlayersOnly || player.isPlaying) && (index1 >= 2 || player.controllers.hasMouse))
          return this.playerIds[index2] == playerId;
      }
    }
    return false;
  }

  public void InitializeRewired()
  {
    if (!ReInput.isReady)
    {
      Debug.LogError((object) "Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?");
    }
    else
    {
      ReInput.ShutDownEvent -= (Action) new Action(this.OnRewiredShutDown);
      ReInput.ShutDownEvent += (Action) new Action(this.OnRewiredShutDown);
      ReInput.EditorRecompileEvent -= (Action) new Action(this.OnEditorRecompile);
      ReInput.EditorRecompileEvent += (Action) new Action(this.OnEditorRecompile);
      this.SetupRewiredVars();
    }
  }

  public void SetupRewiredVars()
  {
    if (!ReInput.isReady)
      return;
    this.SetUpRewiredActions();
    if (this.useAllRewiredGamePlayers)
    {
      IList<Player> playerList = this.useRewiredSystemPlayer ? (IList<Player>) ReInput.players.AllPlayers : (IList<Player>) ReInput.players.Players;
      this.playerIds = new int[playerList.Count];
      for (int index = 0; index < playerList.Count; ++index)
        this.playerIds[index] = playerList[index].id;
    }
    else
    {
      bool flag = false;
      List<int> intList = new List<int>(this.rewiredPlayerIds.Length + 1);
      for (int index = 0; index < this.rewiredPlayerIds.Length; ++index)
      {
        Player player = ReInput.players.GetPlayer(this.rewiredPlayerIds[index]);
        if (player != null && !intList.Contains(player.id))
        {
          intList.Add(player.id);
          if (player.id == 9999999)
            flag = true;
        }
      }
      if (this.useRewiredSystemPlayer && !flag)
        intList.Insert(0, ReInput.players.GetSystemPlayer().id);
      this.playerIds = intList.ToArray();
    }
    this.SetUpRewiredPlayerMice();
  }

  public void SetUpRewiredPlayerMice()
  {
    if (!ReInput.isReady)
      return;
    this.ClearMouseInputSources();
    for (int index = 0; index < this.playerMice.Count; ++index)
    {
      Rewired.Components.PlayerMouse playerMouse = this.playerMice[index];
      if (!UnityTools.IsNullOrDestroyed<Rewired.Components.PlayerMouse>(playerMouse))
        this.AddMouseInputSource((IMouseInputSource) playerMouse);
    }
  }

  public void SetUpRewiredActions()
  {
    if (!ReInput.isReady)
      return;
    if (!this.setActionsById)
    {
      this.horizontalActionId = ReInput.mapping.GetActionId(this.m_HorizontalAxis);
      this.verticalActionId = ReInput.mapping.GetActionId(this.m_VerticalAxis);
      this.submitActionId = ReInput.mapping.GetActionId(this.m_SubmitButton);
      this.cancelActionId = ReInput.mapping.GetActionId(this.m_CancelButton);
    }
    else
    {
      InputAction action1 = ReInput.mapping.GetAction(this.horizontalActionId);
      this.m_HorizontalAxis = action1 != null ? action1.name : string.Empty;
      if (action1 == null)
        this.horizontalActionId = -1;
      InputAction action2 = ReInput.mapping.GetAction(this.verticalActionId);
      this.m_VerticalAxis = action2 != null ? action2.name : string.Empty;
      if (action2 == null)
        this.verticalActionId = -1;
      InputAction action3 = ReInput.mapping.GetAction(this.submitActionId);
      this.m_SubmitButton = action3 != null ? action3.name : string.Empty;
      if (action3 == null)
        this.submitActionId = -1;
      InputAction action4 = ReInput.mapping.GetAction(this.cancelActionId);
      this.m_CancelButton = action4 != null ? action4.name : string.Empty;
      if (action4 != null)
        return;
      this.cancelActionId = -1;
    }
  }

  public bool GetButton(Player player, int actionId) => actionId >= 0 && player.GetButton(actionId);

  public bool GetButtonDown(Player player, int actionId)
  {
    return actionId >= 0 && player.GetButtonDown(actionId);
  }

  public bool GetNegativeButton(Player player, int actionId)
  {
    return actionId >= 0 && player.GetNegativeButton(actionId);
  }

  public bool GetNegativeButtonDown(Player player, int actionId)
  {
    return actionId >= 0 && player.GetNegativeButtonDown(actionId);
  }

  public float GetAxis(Player player, int actionId)
  {
    return actionId < 0 ? 0.0f : player.GetAxis(actionId);
  }

  public void CheckEditorRecompile()
  {
    if (!this.recompiling || !ReInput.isReady)
      return;
    this.recompiling = false;
    this.InitializeRewired();
  }

  public void OnEditorRecompile()
  {
    this.recompiling = true;
    this.ClearRewiredVars();
  }

  public void ClearRewiredVars()
  {
    Array.Clear((Array) this.playerIds, 0, this.playerIds.Length);
    this.ClearMouseInputSources();
  }

  public bool DidAnyMouseMove()
  {
    for (int index = 0; index < this.playerIds.Length; ++index)
    {
      int playerId = this.playerIds[index];
      Player player = ReInput.players.GetPlayer(playerId);
      if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
      {
        int inputSourceCount = this.GetMouseInputSourceCount(playerId);
        for (int mouseIndex = 0; mouseIndex < inputSourceCount; ++mouseIndex)
        {
          IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
          if (mouseInputSource != null && (double) mouseInputSource.screenPositionDelta.sqrMagnitude > 0.0)
            return true;
        }
      }
    }
    return false;
  }

  public bool GetMouseButtonDownOnAnyMouse(int buttonIndex)
  {
    for (int index = 0; index < this.playerIds.Length; ++index)
    {
      int playerId = this.playerIds[index];
      Player player = ReInput.players.GetPlayer(playerId);
      if (player != null && (!this.usePlayingPlayersOnly || player.isPlaying))
      {
        int inputSourceCount = this.GetMouseInputSourceCount(playerId);
        for (int mouseIndex = 0; mouseIndex < inputSourceCount; ++mouseIndex)
        {
          IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
          if (mouseInputSource != null && mouseInputSource.GetButtonDown(buttonIndex))
            return true;
        }
      }
    }
    return false;
  }

  public void OnRewiredInitialized() => this.InitializeRewired();

  public void OnRewiredShutDown() => this.ClearRewiredVars();

  [Serializable]
  public class PlayerSetting
  {
    public int playerId;
    public List<Rewired.Components.PlayerMouse> playerMice = new List<Rewired.Components.PlayerMouse>();

    public PlayerSetting()
    {
    }

    public PlayerSetting(RewiredStandaloneInputModule.PlayerSetting other)
    {
      this.playerId = other != null ? other.playerId : throw new ArgumentNullException(nameof (other));
      this.playerMice = new List<Rewired.Components.PlayerMouse>();
      if (other.playerMice == null)
        return;
      foreach (Rewired.Components.PlayerMouse playerMouse in other.playerMice)
        this.playerMice.Add(playerMouse);
    }

    public RewiredStandaloneInputModule.PlayerSetting Clone()
    {
      return new RewiredStandaloneInputModule.PlayerSetting(this);
    }
  }
}
