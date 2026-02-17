// Decompiled with JetBrains decompiler
// Type: Rewired.Integration.UnityUI.RewiredPointerInputModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.UI;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Rewired.Integration.UnityUI;

public abstract class RewiredPointerInputModule : BaseInputModule
{
  public const int kMouseLeftId = -1;
  public const int kMouseRightId = -2;
  public const int kMouseMiddleId = -3;
  public const int kFakeTouchesId = -4;
  public const int customButtonsStartingId = -2147483520 /*0x80000080*/;
  public const int customButtonsMaxCount = 128 /*0x80*/;
  public const int customButtonsLastId = -2147483392 /*0x80000100*/;
  public List<IMouseInputSource> m_MouseInputSourcesList = new List<IMouseInputSource>();
  public Dictionary<int, Dictionary<int, PlayerPointerEventData>[]> m_PlayerPointerData = new Dictionary<int, Dictionary<int, PlayerPointerEventData>[]>();
  public ITouchInputSource m_UserDefaultTouchInputSource;
  public RewiredPointerInputModule.UnityInputSource __m_DefaultInputSource;
  public RewiredPointerInputModule.MouseState m_MouseState = new RewiredPointerInputModule.MouseState();

  public RewiredPointerInputModule.UnityInputSource defaultInputSource
  {
    get
    {
      return this.__m_DefaultInputSource == null ? (this.__m_DefaultInputSource = new RewiredPointerInputModule.UnityInputSource()) : this.__m_DefaultInputSource;
    }
  }

  public IMouseInputSource defaultMouseInputSource => (IMouseInputSource) this.defaultInputSource;

  public ITouchInputSource defaultTouchInputSource => (ITouchInputSource) this.defaultInputSource;

  public bool IsDefaultMouse(IMouseInputSource mouse) => this.defaultMouseInputSource == mouse;

  public IMouseInputSource GetMouseInputSource(int playerId, int mouseIndex)
  {
    if (mouseIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (mouseIndex));
    if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
      return this.defaultMouseInputSource;
    int count = this.m_MouseInputSourcesList.Count;
    int num = 0;
    for (int index = 0; index < count; ++index)
    {
      IMouseInputSource mouseInputSources = this.m_MouseInputSourcesList[index];
      if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>(mouseInputSources) && mouseInputSources.playerId == playerId)
      {
        if (mouseIndex == num)
          return mouseInputSources;
        ++num;
      }
    }
    return (IMouseInputSource) null;
  }

  public void RemoveMouseInputSource(IMouseInputSource source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.m_MouseInputSourcesList.Remove(source);
  }

  public void AddMouseInputSource(IMouseInputSource source)
  {
    if (UnityTools.IsNullOrDestroyed<IMouseInputSource>(source))
      throw new ArgumentNullException(nameof (source));
    this.m_MouseInputSourcesList.Add(source);
  }

  public int GetMouseInputSourceCount(int playerId)
  {
    if (this.m_MouseInputSourcesList.Count == 0 && this.IsDefaultPlayer(playerId))
      return 1;
    int count = this.m_MouseInputSourcesList.Count;
    int inputSourceCount = 0;
    for (int index = 0; index < count; ++index)
    {
      IMouseInputSource mouseInputSources = this.m_MouseInputSourcesList[index];
      if (!UnityTools.IsNullOrDestroyed<IMouseInputSource>(mouseInputSources) && mouseInputSources.playerId == playerId)
        ++inputSourceCount;
    }
    return inputSourceCount;
  }

  public ITouchInputSource GetTouchInputSource(int playerId, int sourceIndex)
  {
    return !UnityTools.IsNullOrDestroyed<ITouchInputSource>(this.m_UserDefaultTouchInputSource) ? this.m_UserDefaultTouchInputSource : this.defaultTouchInputSource;
  }

  public void RemoveTouchInputSource(ITouchInputSource source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (this.m_UserDefaultTouchInputSource != source)
      return;
    this.m_UserDefaultTouchInputSource = (ITouchInputSource) null;
  }

  public void AddTouchInputSource(ITouchInputSource source)
  {
    this.m_UserDefaultTouchInputSource = !UnityTools.IsNullOrDestroyed<ITouchInputSource>(source) ? source : throw new ArgumentNullException(nameof (source));
  }

  public int GetTouchInputSourceCount(int playerId) => !this.IsDefaultPlayer(playerId) ? 0 : 1;

  public void ClearMouseInputSources() => this.m_MouseInputSourcesList.Clear();

  public virtual bool isMouseSupported
  {
    get
    {
      int count = this.m_MouseInputSourcesList.Count;
      if (count == 0)
        return this.defaultMouseInputSource.enabled;
      for (int index = 0; index < count; ++index)
      {
        if (this.m_MouseInputSourcesList[index].enabled)
          return true;
      }
      return false;
    }
  }

  public abstract bool IsDefaultPlayer(int playerId);

  public bool GetPointerData(
    int playerId,
    int pointerIndex,
    int pointerTypeId,
    out PlayerPointerEventData data,
    bool create,
    PointerEventType pointerEventType)
  {
    Dictionary<int, PlayerPointerEventData>[] dictionaryArray1;
    if (!this.m_PlayerPointerData.TryGetValue(playerId, out dictionaryArray1))
    {
      dictionaryArray1 = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
      for (int index = 0; index < dictionaryArray1.Length; ++index)
        dictionaryArray1[index] = new Dictionary<int, PlayerPointerEventData>();
      this.m_PlayerPointerData.Add(playerId, dictionaryArray1);
    }
    if (pointerIndex >= dictionaryArray1.Length)
    {
      Dictionary<int, PlayerPointerEventData>[] dictionaryArray2 = new Dictionary<int, PlayerPointerEventData>[pointerIndex + 1];
      for (int index = 0; index < dictionaryArray1.Length; ++index)
        dictionaryArray2[index] = dictionaryArray1[index];
      dictionaryArray2[pointerIndex] = new Dictionary<int, PlayerPointerEventData>();
      dictionaryArray1 = dictionaryArray2;
      this.m_PlayerPointerData[playerId] = dictionaryArray1;
    }
    Dictionary<int, PlayerPointerEventData> dictionary = dictionaryArray1[pointerIndex];
    if (!dictionary.TryGetValue(pointerTypeId, out data))
    {
      if (!create)
        return false;
      data = this.CreatePointerEventData(playerId, pointerIndex, pointerTypeId, pointerEventType);
      dictionary.Add(pointerTypeId, data);
      return true;
    }
    data.mouseSource = pointerEventType == PointerEventType.Mouse ? this.GetMouseInputSource(playerId, pointerIndex) : (IMouseInputSource) null;
    data.touchSource = pointerEventType == PointerEventType.Touch ? this.GetTouchInputSource(playerId, pointerIndex) : (ITouchInputSource) null;
    return false;
  }

  public PlayerPointerEventData CreatePointerEventData(
    int playerId,
    int pointerIndex,
    int pointerTypeId,
    PointerEventType pointerEventType)
  {
    PlayerPointerEventData pointerEventData1 = new PlayerPointerEventData(this.eventSystem);
    pointerEventData1.playerId = playerId;
    pointerEventData1.inputSourceIndex = pointerIndex;
    pointerEventData1.pointerId = pointerTypeId;
    pointerEventData1.sourceType = pointerEventType;
    PlayerPointerEventData pointerEventData2 = pointerEventData1;
    switch (pointerEventType)
    {
      case PointerEventType.Mouse:
        pointerEventData2.mouseSource = this.GetMouseInputSource(playerId, pointerIndex);
        break;
      case PointerEventType.Touch:
        pointerEventData2.touchSource = this.GetTouchInputSource(playerId, pointerIndex);
        break;
    }
    switch (pointerTypeId)
    {
      case -3:
        pointerEventData2.buttonIndex = 2;
        break;
      case -2:
        pointerEventData2.buttonIndex = 1;
        break;
      case -1:
        pointerEventData2.buttonIndex = 0;
        break;
      default:
        if (pointerTypeId >= -2147483520 /*0x80000080*/ && pointerTypeId <= -2147483392 /*0x80000100*/)
        {
          pointerEventData2.buttonIndex = pointerTypeId - -2147483520 /*0x80000080*/;
          break;
        }
        break;
    }
    return pointerEventData2;
  }

  public void RemovePointerData(PlayerPointerEventData data)
  {
    Dictionary<int, PlayerPointerEventData>[] dictionaryArray;
    if (!this.m_PlayerPointerData.TryGetValue(data.playerId, out dictionaryArray) || (uint) data.inputSourceIndex >= (uint) dictionaryArray.Length)
      return;
    dictionaryArray[data.inputSourceIndex].Remove(data.pointerId);
  }

  public PlayerPointerEventData GetTouchPointerEventData(
    int playerId,
    int touchDeviceIndex,
    Touch input,
    out bool pressed,
    out bool released)
  {
    PlayerPointerEventData data;
    bool pointerData = this.GetPointerData(playerId, touchDeviceIndex, input.fingerId, out data, true, PointerEventType.Touch);
    data.Reset();
    pressed = pointerData || input.phase == TouchPhase.Began;
    released = input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended;
    if (pointerData)
      data.position = input.position;
    if (pressed)
      data.delta = Vector2.zero;
    else
      data.delta = input.position - data.position;
    data.position = input.position;
    data.button = PointerEventData.InputButton.Left;
    this.eventSystem.RaycastAll((PointerEventData) data, this.m_RaycastResultCache);
    RaycastResult firstRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
    data.pointerCurrentRaycast = firstRaycast;
    this.m_RaycastResultCache.Clear();
    return data;
  }

  public virtual RewiredPointerInputModule.MouseState GetMousePointerEventData(
    int playerId,
    int mouseIndex)
  {
    IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
    if (mouseInputSource == null)
      return (RewiredPointerInputModule.MouseState) null;
    PlayerPointerEventData data1;
    int num = this.GetPointerData(playerId, mouseIndex, -1, out data1, true, PointerEventType.Mouse) ? 1 : 0;
    data1.Reset();
    if (num != 0)
      data1.position = mouseInputSource.screenPosition;
    Vector2 screenPosition = mouseInputSource.screenPosition;
    if (mouseInputSource.locked || !mouseInputSource.enabled)
    {
      data1.position = new Vector2(-1f, -1f);
      data1.delta = Vector2.zero;
    }
    else
    {
      data1.delta = screenPosition - data1.position;
      data1.position = screenPosition;
    }
    data1.scrollDelta = mouseInputSource.wheelDelta;
    data1.button = PointerEventData.InputButton.Left;
    this.eventSystem.RaycastAll((PointerEventData) data1, this.m_RaycastResultCache);
    RaycastResult firstRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
    data1.pointerCurrentRaycast = firstRaycast;
    this.m_RaycastResultCache.Clear();
    PlayerPointerEventData data2;
    this.GetPointerData(playerId, mouseIndex, -2, out data2, true, PointerEventType.Mouse);
    this.CopyFromTo((PointerEventData) data1, (PointerEventData) data2);
    data2.button = PointerEventData.InputButton.Right;
    PlayerPointerEventData data3;
    this.GetPointerData(playerId, mouseIndex, -3, out data3, true, PointerEventType.Mouse);
    this.CopyFromTo((PointerEventData) data1, (PointerEventData) data3);
    data3.button = PointerEventData.InputButton.Middle;
    for (int index = 3; index < mouseInputSource.buttonCount; ++index)
    {
      PlayerPointerEventData data4;
      this.GetPointerData(playerId, mouseIndex, index - 2147483520, out data4, true, PointerEventType.Mouse);
      this.CopyFromTo((PointerEventData) data1, (PointerEventData) data4);
      data4.button = ~PointerEventData.InputButton.Left;
    }
    this.m_MouseState.SetButtonState(0, this.StateForMouseButton(playerId, mouseIndex, 0), data1);
    this.m_MouseState.SetButtonState(1, this.StateForMouseButton(playerId, mouseIndex, 1), data2);
    this.m_MouseState.SetButtonState(2, this.StateForMouseButton(playerId, mouseIndex, 2), data3);
    for (int index = 3; index < mouseInputSource.buttonCount; ++index)
    {
      PlayerPointerEventData data5;
      this.GetPointerData(playerId, mouseIndex, index - 2147483520, out data5, false, PointerEventType.Mouse);
      this.m_MouseState.SetButtonState(index, this.StateForMouseButton(playerId, mouseIndex, index), data5);
    }
    return this.m_MouseState;
  }

  public PlayerPointerEventData GetLastPointerEventData(
    int playerId,
    int pointerIndex,
    int pointerTypeId,
    bool ignorePointerTypeId,
    PointerEventType pointerEventType)
  {
    if (!ignorePointerTypeId)
    {
      PlayerPointerEventData data;
      this.GetPointerData(playerId, pointerIndex, pointerTypeId, out data, false, pointerEventType);
      return data;
    }
    Dictionary<int, PlayerPointerEventData>[] dictionaryArray;
    if (!this.m_PlayerPointerData.TryGetValue(playerId, out dictionaryArray))
      return (PlayerPointerEventData) null;
    if ((uint) pointerIndex >= (uint) dictionaryArray.Length)
      return (PlayerPointerEventData) null;
    using (Dictionary<int, PlayerPointerEventData>.Enumerator enumerator = dictionaryArray[pointerIndex].GetEnumerator())
    {
      if (enumerator.MoveNext())
        return enumerator.Current.Value;
    }
    return (PlayerPointerEventData) null;
  }

  public static bool ShouldStartDrag(
    Vector2 pressPos,
    Vector2 currentPos,
    float threshold,
    bool useDragThreshold)
  {
    return !useDragThreshold || (double) (pressPos - currentPos).sqrMagnitude >= (double) threshold * (double) threshold;
  }

  public virtual void ProcessMove(PlayerPointerEventData pointerEvent)
  {
    GameObject gameObject;
    if (pointerEvent.sourceType == PointerEventType.Mouse)
    {
      IMouseInputSource mouseInputSource = this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex);
      gameObject = mouseInputSource == null ? (GameObject) null : (!mouseInputSource.enabled || mouseInputSource.locked ? (GameObject) null : pointerEvent.pointerCurrentRaycast.gameObject);
    }
    else
    {
      if (pointerEvent.sourceType != PointerEventType.Touch)
        throw new NotImplementedException();
      gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
    }
    this.HandlePointerExitAndEnter((PointerEventData) pointerEvent, gameObject);
  }

  public virtual void ProcessDrag(PlayerPointerEventData pointerEvent)
  {
    if (!pointerEvent.IsPointerMoving() || (UnityEngine.Object) pointerEvent.pointerDrag == (UnityEngine.Object) null)
      return;
    if (pointerEvent.sourceType == PointerEventType.Mouse)
    {
      IMouseInputSource mouseInputSource = this.GetMouseInputSource(pointerEvent.playerId, pointerEvent.inputSourceIndex);
      if (mouseInputSource == null || mouseInputSource.locked || !mouseInputSource.enabled)
        return;
    }
    if (!pointerEvent.dragging && RewiredPointerInputModule.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float) this.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
    {
      ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.beginDragHandler);
      pointerEvent.dragging = true;
    }
    if (!pointerEvent.dragging)
      return;
    if ((UnityEngine.Object) pointerEvent.pointerPress != (UnityEngine.Object) pointerEvent.pointerDrag)
    {
      ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData) pointerEvent, ExecuteEvents.pointerUpHandler);
      pointerEvent.eligibleForClick = false;
      pointerEvent.pointerPress = (GameObject) null;
      pointerEvent.rawPointerPress = (GameObject) null;
    }
    ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.dragHandler);
  }

  public override bool IsPointerOverGameObject(int pointerTypeId)
  {
    foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair in this.m_PlayerPointerData)
    {
      foreach (Dictionary<int, PlayerPointerEventData> dictionary in keyValuePair.Value)
      {
        PlayerPointerEventData pointerEventData;
        if (dictionary.TryGetValue(pointerTypeId, out pointerEventData) && (UnityEngine.Object) pointerEventData.pointerEnter != (UnityEngine.Object) null)
          return true;
      }
    }
    return false;
  }

  public void ClearSelection()
  {
    BaseEventData baseEventData = this.GetBaseEventData();
    foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair1 in this.m_PlayerPointerData)
    {
      Dictionary<int, PlayerPointerEventData>[] dictionaryArray = keyValuePair1.Value;
      for (int index = 0; index < dictionaryArray.Length; ++index)
      {
        foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in dictionaryArray[index])
          this.HandlePointerExitAndEnter((PointerEventData) keyValuePair2.Value, (GameObject) null);
        dictionaryArray[index].Clear();
      }
    }
    this.eventSystem.SetSelectedGameObject((GameObject) null, baseEventData);
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder("<b>Pointer Input Module of type: </b>" + ((object) this).GetType()?.ToString());
    stringBuilder.AppendLine();
    foreach (KeyValuePair<int, Dictionary<int, PlayerPointerEventData>[]> keyValuePair1 in this.m_PlayerPointerData)
    {
      stringBuilder.AppendLine("<B>Player Id:</b> " + keyValuePair1.Key.ToString());
      Dictionary<int, PlayerPointerEventData>[] dictionaryArray = keyValuePair1.Value;
      for (int index = 0; index < dictionaryArray.Length; ++index)
      {
        stringBuilder.AppendLine("<B>Pointer Index:</b> " + index.ToString());
        foreach (KeyValuePair<int, PlayerPointerEventData> keyValuePair2 in dictionaryArray[index])
        {
          stringBuilder.AppendLine("<B>Button Id:</b> " + keyValuePair2.Key.ToString());
          stringBuilder.AppendLine(keyValuePair2.Value.ToString());
        }
      }
    }
    return stringBuilder.ToString();
  }

  public void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
  {
    if (!((UnityEngine.Object) ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo) != (UnityEngine.Object) this.eventSystem.currentSelectedGameObject))
      return;
    this.eventSystem.SetSelectedGameObject((GameObject) null, pointerEvent);
  }

  public void CopyFromTo(PointerEventData from, PointerEventData to)
  {
    to.position = from.position;
    to.delta = from.delta;
    to.scrollDelta = from.scrollDelta;
    to.pointerCurrentRaycast = from.pointerCurrentRaycast;
    to.pointerEnter = from.pointerEnter;
  }

  public PointerEventData.FramePressState StateForMouseButton(
    int playerId,
    int mouseIndex,
    int buttonId)
  {
    IMouseInputSource mouseInputSource = this.GetMouseInputSource(playerId, mouseIndex);
    if (mouseInputSource == null)
      return PointerEventData.FramePressState.NotChanged;
    bool buttonDown = mouseInputSource.GetButtonDown(buttonId);
    bool buttonUp = mouseInputSource.GetButtonUp(buttonId);
    if (buttonDown & buttonUp)
      return PointerEventData.FramePressState.PressedAndReleased;
    if (buttonDown)
      return PointerEventData.FramePressState.Pressed;
    return buttonUp ? PointerEventData.FramePressState.Released : PointerEventData.FramePressState.NotChanged;
  }

  public class MouseState
  {
    public List<RewiredPointerInputModule.ButtonState> m_TrackedButtons = new List<RewiredPointerInputModule.ButtonState>();

    public bool AnyPressesThisFrame()
    {
      for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
      {
        if (this.m_TrackedButtons[index].eventData.PressedThisFrame())
          return true;
      }
      return false;
    }

    public bool AnyReleasesThisFrame()
    {
      for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
      {
        if (this.m_TrackedButtons[index].eventData.ReleasedThisFrame())
          return true;
      }
      return false;
    }

    public RewiredPointerInputModule.ButtonState GetButtonState(int button)
    {
      RewiredPointerInputModule.ButtonState buttonState = (RewiredPointerInputModule.ButtonState) null;
      for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
      {
        if (this.m_TrackedButtons[index].button == button)
        {
          buttonState = this.m_TrackedButtons[index];
          break;
        }
      }
      if (buttonState == null)
      {
        buttonState = new RewiredPointerInputModule.ButtonState()
        {
          button = button,
          eventData = new RewiredPointerInputModule.MouseButtonEventData()
        };
        this.m_TrackedButtons.Add(buttonState);
      }
      return buttonState;
    }

    public void SetButtonState(
      int button,
      PointerEventData.FramePressState stateForMouseButton,
      PlayerPointerEventData data)
    {
      RewiredPointerInputModule.ButtonState buttonState = this.GetButtonState(button);
      buttonState.eventData.buttonState = stateForMouseButton;
      buttonState.eventData.buttonData = data;
    }
  }

  public class MouseButtonEventData
  {
    public PointerEventData.FramePressState buttonState;
    public PlayerPointerEventData buttonData;

    public bool PressedThisFrame()
    {
      return this.buttonState == PointerEventData.FramePressState.Pressed || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
    }

    public bool ReleasedThisFrame()
    {
      return this.buttonState == PointerEventData.FramePressState.Released || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
    }
  }

  public class ButtonState
  {
    public int m_Button;
    public RewiredPointerInputModule.MouseButtonEventData m_EventData;

    public RewiredPointerInputModule.MouseButtonEventData eventData
    {
      get => this.m_EventData;
      set => this.m_EventData = value;
    }

    public int button
    {
      get => this.m_Button;
      set => this.m_Button = value;
    }
  }

  public sealed class UnityInputSource : IMouseInputSource, ITouchInputSource
  {
    public Vector2 m_MousePosition;
    public Vector2 m_MousePositionPrev;
    public int m_LastUpdatedFrame = -1;

    int IMouseInputSource.playerId
    {
      get
      {
        this.TryUpdate();
        return 0;
      }
    }

    int ITouchInputSource.playerId
    {
      get
      {
        this.TryUpdate();
        return 0;
      }
    }

    bool IMouseInputSource.enabled
    {
      get
      {
        this.TryUpdate();
        return Input.mousePresent;
      }
    }

    bool IMouseInputSource.locked
    {
      get
      {
        this.TryUpdate();
        return Cursor.lockState == CursorLockMode.Locked;
      }
    }

    int IMouseInputSource.buttonCount
    {
      get
      {
        this.TryUpdate();
        return 3;
      }
    }

    bool IMouseInputSource.GetButtonDown(int button)
    {
      this.TryUpdate();
      return Input.GetMouseButtonDown(button);
    }

    bool IMouseInputSource.GetButtonUp(int button)
    {
      this.TryUpdate();
      return Input.GetMouseButtonUp(button);
    }

    bool IMouseInputSource.GetButton(int button)
    {
      this.TryUpdate();
      return Input.GetMouseButton(button);
    }

    Vector2 IMouseInputSource.screenPosition
    {
      get
      {
        this.TryUpdate();
        return (Vector2) Input.mousePosition;
      }
    }

    Vector2 IMouseInputSource.screenPositionDelta
    {
      get
      {
        this.TryUpdate();
        return this.m_MousePosition - this.m_MousePositionPrev;
      }
    }

    Vector2 IMouseInputSource.wheelDelta
    {
      get
      {
        this.TryUpdate();
        return Input.mouseScrollDelta;
      }
    }

    bool ITouchInputSource.touchSupported
    {
      get
      {
        this.TryUpdate();
        return Input.touchSupported;
      }
    }

    int ITouchInputSource.touchCount
    {
      get
      {
        this.TryUpdate();
        return Input.touchCount;
      }
    }

    Touch ITouchInputSource.GetTouch(int index)
    {
      this.TryUpdate();
      return Input.GetTouch(index);
    }

    public void TryUpdate()
    {
      if (Time.frameCount == this.m_LastUpdatedFrame)
        return;
      this.m_LastUpdatedFrame = Time.frameCount;
      this.m_MousePositionPrev = this.m_MousePosition;
      this.m_MousePosition = (Vector2) Input.mousePosition;
    }
  }
}
