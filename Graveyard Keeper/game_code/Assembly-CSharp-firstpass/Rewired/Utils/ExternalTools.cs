// Decompiled with JetBrains decompiler
// Type: Rewired.Utils.ExternalTools
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Rewired.Internal;
using Rewired.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Utils;

[EditorBrowsable(EditorBrowsableState.Never)]
public class ExternalTools : IExternalTools
{
  public object GetPlatformInitializer() => (object) null;

  public string GetFocusedEditorWindowTitle() => string.Empty;

  public bool IsEditorSceneViewFocused() => false;

  public bool LinuxInput_IsJoystickPreconfigured(string name) => false;

  public event Action<uint, bool> XboxOneInput_OnGamepadStateChange;

  public int XboxOneInput_GetUserIdForGamepad(uint id) => 0;

  public ulong XboxOneInput_GetControllerId(uint unityJoystickId) => 0;

  public bool XboxOneInput_IsGamepadActive(uint unityJoystickId) => false;

  public string XboxOneInput_GetControllerType(ulong xboxControllerId) => string.Empty;

  public uint XboxOneInput_GetJoystickId(ulong xboxControllerId) => 0;

  public void XboxOne_Gamepad_UpdatePlugin()
  {
  }

  public bool XboxOne_Gamepad_SetGamepadVibration(
    ulong xboxOneJoystickId,
    float leftMotor,
    float rightMotor,
    float leftTriggerLevel,
    float rightTriggerLevel)
  {
    return false;
  }

  public void XboxOne_Gamepad_PulseVibrateMotor(
    ulong xboxOneJoystickId,
    int motorInt,
    float startLevel,
    float endLevel,
    ulong durationMS)
  {
  }

  public Vector3 PS4Input_GetLastAcceleration(int id) => Vector3.zero;

  public Vector3 PS4Input_GetLastGyro(int id) => Vector3.zero;

  public Vector4 PS4Input_GetLastOrientation(int id) => Vector4.zero;

  public void PS4Input_GetLastTouchData(
    int id,
    out int touchNum,
    out int touch0x,
    out int touch0y,
    out int touch0id,
    out int touch1x,
    out int touch1y,
    out int touch1id)
  {
    touchNum = 0;
    touch0x = 0;
    touch0y = 0;
    touch0id = 0;
    touch1x = 0;
    touch1y = 0;
    touch1id = 0;
  }

  public void PS4Input_GetPadControllerInformation(
    int id,
    out float touchpixelDensity,
    out int touchResolutionX,
    out int touchResolutionY,
    out int analogDeadZoneLeft,
    out int analogDeadZoneright,
    out int connectionType)
  {
    touchpixelDensity = 0.0f;
    touchResolutionX = 0;
    touchResolutionY = 0;
    analogDeadZoneLeft = 0;
    analogDeadZoneright = 0;
    connectionType = 0;
  }

  public void PS4Input_PadSetMotionSensorState(int id, bool bEnable)
  {
  }

  public void PS4Input_PadSetTiltCorrectionState(int id, bool bEnable)
  {
  }

  public void PS4Input_PadSetAngularVelocityDeadbandState(int id, bool bEnable)
  {
  }

  public void PS4Input_PadSetLightBar(int id, int red, int green, int blue)
  {
  }

  public void PS4Input_PadResetLightBar(int id)
  {
  }

  public void PS4Input_PadSetVibration(int id, int largeMotor, int smallMotor)
  {
  }

  public void PS4Input_PadResetOrientation(int id)
  {
  }

  public bool PS4Input_PadIsConnected(int id) => false;

  public void PS4Input_GetUsersDetails(int slot, object loggedInUser)
  {
  }

  public int PS4Input_GetDeviceClassForHandle(int handle) => -1;

  public string PS4Input_GetDeviceClassString(int intValue) => (string) null;

  public int PS4Input_PadGetUsersHandles2(int maxControllers, int[] handles) => 0;

  public Vector3 PS4Input_GetLastMoveAcceleration(int id, int index) => Vector3.zero;

  public Vector3 PS4Input_GetLastMoveGyro(int id, int index) => Vector3.zero;

  public int PS4Input_MoveGetButtons(int id, int index) => 0;

  public int PS4Input_MoveGetAnalogButton(int id, int index) => 0;

  public bool PS4Input_MoveIsConnected(int id, int index) => false;

  public int PS4Input_MoveGetUsersMoveHandles(
    int maxNumberControllers,
    int[] primaryHandles,
    int[] secondaryHandles)
  {
    return 0;
  }

  public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers, int[] primaryHandles) => 0;

  public int PS4Input_MoveGetUsersMoveHandles(int maxNumberControllers) => 0;

  public IntPtr PS4Input_MoveGetControllerInputForTracking() => IntPtr.Zero;

  public void GetSpecialControllerInformation(int id, int padIndex, object controllerInformation)
  {
  }

  public Vector3 PS4Input_SpecialGetLastAcceleration(int id) => Vector3.zero;

  public Vector3 PS4Input_SpecialGetLastGyro(int id) => Vector3.zero;

  public Vector4 PS4Input_SpecialGetLastOrientation(int id) => Vector4.zero;

  public int PS4Input_SpecialGetUsersHandles(int maxNumberControllers, int[] handles) => 0;

  public int PS4Input_SpecialGetUsersHandles2(int maxNumberControllers, int[] handles) => 0;

  public bool PS4Input_SpecialIsConnected(int id) => false;

  public void PS4Input_SpecialResetLightSphere(int id)
  {
  }

  public void PS4Input_SpecialResetOrientation(int id)
  {
  }

  public void PS4Input_SpecialSetAngularVelocityDeadbandState(int id, bool bEnable)
  {
  }

  public void PS4Input_SpecialSetLightSphere(int id, int red, int green, int blue)
  {
  }

  public void PS4Input_SpecialSetMotionSensorState(int id, bool bEnable)
  {
  }

  public void PS4Input_SpecialSetTiltCorrectionState(int id, bool bEnable)
  {
  }

  public void PS4Input_SpecialSetVibration(int id, int largeMotor, int smallMotor)
  {
  }

  public void GetDeviceVIDPIDs(out List<int> vids, out List<int> pids)
  {
    vids = new List<int>();
    pids = new List<int>();
  }

  public int GetAndroidAPILevel() => -1;

  public bool UnityUI_Graphic_GetRaycastTarget(object graphic)
  {
    return !((UnityEngine.Object) (graphic as Graphic) == (UnityEngine.Object) null) && (graphic as Graphic).raycastTarget;
  }

  public void UnityUI_Graphic_SetRaycastTarget(object graphic, bool value)
  {
    if ((UnityEngine.Object) (graphic as Graphic) == (UnityEngine.Object) null)
      return;
    (graphic as Graphic).raycastTarget = value;
  }

  public bool UnityInput_IsTouchPressureSupported => Input.touchPressureSupported;

  public float UnityInput_GetTouchPressure(ref Touch touch) => touch.pressure;

  public float UnityInput_GetTouchMaximumPossiblePressure(ref Touch touch)
  {
    return touch.maximumPossiblePressure;
  }

  public IControllerTemplate CreateControllerTemplate(Guid typeGuid, object payload)
  {
    return ControllerTemplateFactory.Create(typeGuid, payload);
  }

  public System.Type[] GetControllerTemplateTypes() => ControllerTemplateFactory.templateTypes;

  public System.Type[] GetControllerTemplateInterfaceTypes()
  {
    return ControllerTemplateFactory.templateInterfaceTypes;
  }
}
