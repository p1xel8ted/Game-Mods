// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DRooms
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-rooms/")]
public class ProCamera2DRooms : BasePC2D, IPositionOverrider, ISizeOverrider
{
  public const string ExtensionName = "Rooms";
  public int _currentRoomIndex = -1;
  public int _previousRoomIndex = -1;
  public List<Room> Rooms = new List<Room>();
  public float UpdateInterval = 0.1f;
  public bool UseTargetsMidPoint = true;
  public Transform TriggerTarget;
  public bool TransitionInstanlyOnStart = true;
  public bool RestoreOnRoomExit;
  public float RestoreDuration = 1f;
  public EaseType RestoreEaseType;
  public bool AutomaticRoomActivation = true;
  public RoomEvent OnStartedTransition;
  public RoomEvent OnFinishedTransition;
  public UnityEvent OnExitedAllRooms;
  public ProCamera2DNumericBoundaries _numericBoundaries;
  public NumericBoundariesSettings _defaultNumericBoundariesSettings;
  public bool _transitioning;
  public Vector3 _newPos;
  public float _newSize;
  public Coroutine _transitionRoutine;
  public float _originalSize;
  public int _poOrder = 1001;
  public int _soOrder = 3001;

  public int CurrentRoomIndex => this._currentRoomIndex;

  public int PreviousRoomIndex => this._previousRoomIndex;

  public override void Awake()
  {
    base.Awake();
    this._numericBoundaries = this.ProCamera2D.GetComponent<ProCamera2DNumericBoundaries>();
    this._defaultNumericBoundariesSettings = this._numericBoundaries.Settings;
    this._originalSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddPositionOverrider((IPositionOverrider) this);
    Com.LuisPedroFonseca.ProCamera2D.ProCamera2D.Instance.AddSizeOverrider((ISizeOverrider) this);
  }

  public void Start()
  {
    this.StartCoroutine(this.TestRoomRoutine());
    if (!this.TransitionInstanlyOnStart)
      return;
    Vector3 targetPos = this.ProCamera2D.TargetsMidPoint;
    if (!this.UseTargetsMidPoint && (UnityEngine.Object) this.TriggerTarget != (UnityEngine.Object) null)
      targetPos = this.TriggerTarget.position;
    int currentRoom = this.ComputeCurrentRoom(targetPos);
    if (currentRoom == -1)
      return;
    this.EnterRoom(currentRoom, false);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.ProCamera2D.RemovePositionOverrider((IPositionOverrider) this);
    this.ProCamera2D.RemoveSizeOverrider((ISizeOverrider) this);
  }

  public Vector3 OverridePosition(float deltaTime, Vector3 originalPosition)
  {
    return !this.enabled || !this._transitioning ? originalPosition : this._newPos;
  }

  public int POOrder
  {
    get => this._poOrder;
    set => this._poOrder = value;
  }

  public float OverrideSize(float deltaTime, float originalSize)
  {
    return !this.enabled || !this._transitioning ? originalSize : this._newSize;
  }

  public int SOOrder
  {
    get => this._soOrder;
    set => this._soOrder = value;
  }

  public void TestRoom()
  {
    Vector3 targetPos = this.ProCamera2D.TargetsMidPoint;
    if (!this.UseTargetsMidPoint && (UnityEngine.Object) this.TriggerTarget != (UnityEngine.Object) null)
      targetPos = this.TriggerTarget.position;
    int currentRoom = this.ComputeCurrentRoom(targetPos);
    if (currentRoom != -1 && this._currentRoomIndex != currentRoom)
      this.EnterRoom(currentRoom);
    if (currentRoom != -1 || this._currentRoomIndex == -1)
      return;
    this.ExitRoom();
  }

  public int ComputeCurrentRoom(Vector3 targetPos)
  {
    int currentRoom = -1;
    for (int index = 0; index < this.Rooms.Count; ++index)
    {
      if (Utils.IsInsideRectangle(this.Rooms[index].Dimensions.x, this.Rooms[index].Dimensions.y, this.Rooms[index].Dimensions.width, this.Rooms[index].Dimensions.height, this.Vector3H(targetPos), this.Vector3V(targetPos)))
        currentRoom = index;
    }
    return currentRoom;
  }

  public void EnterRoom(int roomIndex, bool useTransition = true)
  {
    if (roomIndex < 0 || roomIndex > this.Rooms.Count - 1)
    {
      Debug.LogError((object) ("Can't find room with index: " + roomIndex.ToString()));
    }
    else
    {
      if (roomIndex == this._currentRoomIndex)
        return;
      this._previousRoomIndex = this._currentRoomIndex;
      this._currentRoomIndex = roomIndex;
      this.TransitionToRoom(this.Rooms[this._currentRoomIndex], useTransition);
      if (this.OnStartedTransition == null)
        return;
      this.OnStartedTransition.Invoke(roomIndex, this._previousRoomIndex);
    }
  }

  public void EnterRoom(string roomID, bool useTransition = true)
  {
    this.EnterRoom(this.Rooms.FindIndex((Predicate<Room>) (room => room.ID == roomID)), useTransition);
  }

  public void ExitRoom()
  {
    if (this.RestoreOnRoomExit)
    {
      this._currentRoomIndex = -1;
      if (this.OnStartedTransition != null)
        this.OnStartedTransition.Invoke(this._currentRoomIndex, this._previousRoomIndex);
      if (this._transitionRoutine != null)
        this.StopCoroutine(this._transitionRoutine);
      this._transitionRoutine = this.StartCoroutine(this.TransitionRoutine(this._defaultNumericBoundariesSettings, this._originalSize, this.RestoreDuration, this.RestoreEaseType));
    }
    if (this.OnExitedAllRooms == null)
      return;
    this.OnExitedAllRooms.Invoke();
  }

  public void AddRoom(
    float roomX,
    float roomY,
    float roomWidth,
    float roomHeight,
    float transitionDuration = 1f,
    EaseType transitionEaseType = EaseType.EaseInOut,
    bool scaleToFit = false,
    bool zoom = false,
    float zoomScale = 1.5f,
    string id = "")
  {
    this.Rooms.Add(new Room()
    {
      ID = id,
      Dimensions = new Rect(roomX, roomY, roomWidth, roomHeight),
      TransitionDuration = transitionDuration,
      TransitionEaseType = transitionEaseType,
      ScaleCameraToFit = scaleToFit,
      Zoom = zoom,
      ZoomScale = zoomScale
    });
  }

  public void RemoveRoom(string roomName)
  {
    Room room = this.Rooms.Find((Predicate<Room>) (obj => obj.ID == roomName));
    if (room != null)
      this.Rooms.Remove(room);
    else
      Debug.LogWarning((object) (roomName + " not found in the Rooms list."));
  }

  public void SetDefaultNumericBoundariesSettings(NumericBoundariesSettings settings)
  {
    this._defaultNumericBoundariesSettings = settings;
  }

  public Room GetRoom(string roomID)
  {
    return this.Rooms.Find((Predicate<Room>) (obj => obj.ID == roomID));
  }

  public IEnumerator TestRoomRoutine()
  {
    yield return (object) new WaitForEndOfFrame();
    WaitForSeconds waitForSeconds = new WaitForSeconds(this.UpdateInterval);
    while (true)
    {
      if (this.AutomaticRoomActivation)
        this.TestRoom();
      yield return (object) waitForSeconds;
    }
  }

  public void TransitionToRoom(Room room, bool useTransition = true)
  {
    if (this._transitionRoutine != null)
      this.StopCoroutine(this._transitionRoutine);
    NumericBoundariesSettings numericBoundariesSettings = new NumericBoundariesSettings()
    {
      UseNumericBoundaries = true,
      UseTopBoundary = true,
      TopBoundary = room.Dimensions.y + room.Dimensions.height / 2f,
      UseBottomBoundary = true,
      BottomBoundary = room.Dimensions.y - room.Dimensions.height / 2f,
      UseLeftBoundary = true,
      LeftBoundary = room.Dimensions.x - room.Dimensions.width / 2f,
      UseRightBoundary = true,
      RightBoundary = room.Dimensions.x + room.Dimensions.width / 2f
    };
    float targetSize = this.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    float cameraSizeForRoom = this.GetCameraSizeForRoom(room.Dimensions);
    if (room.ScaleCameraToFit)
      targetSize = cameraSizeForRoom;
    else if (room.Zoom && (double) this._originalSize * (double) room.ZoomScale < (double) cameraSizeForRoom)
      targetSize = this._originalSize * room.ZoomScale;
    else if ((double) cameraSizeForRoom < (double) targetSize)
      targetSize = cameraSizeForRoom;
    this._transitionRoutine = this.StartCoroutine(this.TransitionRoutine(numericBoundariesSettings, targetSize, useTransition ? room.TransitionDuration : 0.0f, room.TransitionEaseType));
  }

  public IEnumerator TransitionRoutine(
    NumericBoundariesSettings numericBoundariesSettings,
    float targetSize,
    float transitionDuration = 1f,
    EaseType transitionEaseType = EaseType.EaseOut)
  {
    ProCamera2DRooms proCamera2Drooms = this;
    proCamera2Drooms._transitioning = true;
    proCamera2Drooms._numericBoundaries.UseNumericBoundaries = false;
    float initialSize = proCamera2Drooms.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f;
    float initialCamPosH = proCamera2Drooms.Vector3H(proCamera2Drooms.ProCamera2D.LocalPosition);
    float initialCamPosV = proCamera2Drooms.Vector3V(proCamera2Drooms.ProCamera2D.LocalPosition);
    float t = 0.0f;
    while ((double) t <= 1.0)
    {
      if ((double) transitionDuration < 1.4012984643248171E-45)
        t = 1.1f;
      else if ((double) proCamera2Drooms.ProCamera2D.DeltaTime > 1.4012984643248171E-45)
        t += proCamera2Drooms.ProCamera2D.DeltaTime / transitionDuration;
      proCamera2Drooms._newSize = Utils.EaseFromTo(initialSize, targetSize, t, transitionEaseType);
      float x = proCamera2Drooms.ProCamera2D.CameraTargetPositionSmoothed.x;
      float y = proCamera2Drooms.ProCamera2D.CameraTargetPositionSmoothed.y;
      proCamera2Drooms.LimitToNumericBoundaries(ref x, ref y, targetSize * proCamera2Drooms.ProCamera2D.GameCamera.aspect, targetSize, numericBoundariesSettings);
      float num1 = Utils.EaseFromTo(initialCamPosH, x, t, transitionEaseType);
      float num2 = Utils.EaseFromTo(initialCamPosV, y, t, transitionEaseType);
      proCamera2Drooms._newPos = proCamera2Drooms.VectorHVD(num1, num2, 0.0f);
      yield return (object) proCamera2Drooms.ProCamera2D.GetYield();
    }
    proCamera2Drooms._transitioning = false;
    proCamera2Drooms._numericBoundaries.Settings = numericBoundariesSettings;
    proCamera2Drooms._transitionRoutine = (Coroutine) null;
    if (proCamera2Drooms.OnFinishedTransition != null)
      proCamera2Drooms.OnFinishedTransition.Invoke(proCamera2Drooms._currentRoomIndex, proCamera2Drooms._previousRoomIndex);
    proCamera2Drooms._previousRoomIndex = proCamera2Drooms._currentRoomIndex;
  }

  public void LimitToNumericBoundaries(
    ref float horizontalPos,
    ref float verticalPos,
    float halfCameraWidth,
    float halfCameraHeight,
    NumericBoundariesSettings numericBoundaries)
  {
    if (numericBoundaries.UseLeftBoundary && (double) horizontalPos - (double) halfCameraWidth < (double) numericBoundaries.LeftBoundary)
      horizontalPos = numericBoundaries.LeftBoundary + halfCameraWidth;
    else if (numericBoundaries.UseRightBoundary && (double) horizontalPos + (double) halfCameraWidth > (double) numericBoundaries.RightBoundary)
      horizontalPos = numericBoundaries.RightBoundary - halfCameraWidth;
    if (numericBoundaries.UseBottomBoundary && (double) verticalPos - (double) halfCameraHeight < (double) numericBoundaries.BottomBoundary)
    {
      verticalPos = numericBoundaries.BottomBoundary + halfCameraHeight;
    }
    else
    {
      if (!numericBoundaries.UseTopBoundary || (double) verticalPos + (double) halfCameraHeight <= (double) numericBoundaries.TopBoundary)
        return;
      verticalPos = numericBoundaries.TopBoundary - halfCameraHeight;
    }
  }

  public float GetCameraSizeForRoom(Rect roomRect)
  {
    return (double) roomRect.width / (double) this.ProCamera2D.ScreenSizeInWorldCoordinates.x < (double) (roomRect.height / this.ProCamera2D.ScreenSizeInWorldCoordinates.y) ? (float) ((double) roomRect.width / (double) this.ProCamera2D.GameCamera.aspect / 2.0) : roomRect.height / 2f;
  }
}
