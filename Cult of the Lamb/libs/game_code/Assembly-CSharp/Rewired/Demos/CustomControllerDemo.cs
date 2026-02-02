// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.CustomControllerDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class CustomControllerDemo : MonoBehaviour
{
  public int playerId;
  public string controllerTag;
  public bool useUpdateCallbacks;
  public int buttonCount;
  public int axisCount;
  public float[] axisValues;
  public bool[] buttonValues;
  public TouchJoystickExample[] joysticks;
  public TouchButtonExample[] buttons;
  public CustomController controller;
  [NonSerialized]
  public bool initialized;

  public void Awake()
  {
    ScreenOrientation screenOrientation = ScreenOrientation.LandscapeLeft;
    if (SystemInfo.deviceType == DeviceType.Handheld && Screen.orientation != screenOrientation)
      Screen.orientation = screenOrientation;
    this.Initialize();
  }

  public void Initialize()
  {
    ReInput.InputSourceUpdateEvent += (Action) new Action(this.OnInputSourceUpdate);
    this.joysticks = this.GetComponentsInChildren<TouchJoystickExample>();
    this.buttons = this.GetComponentsInChildren<TouchButtonExample>();
    this.axisCount = this.joysticks.Length * 2;
    this.buttonCount = this.buttons.Length;
    this.axisValues = new float[this.axisCount];
    this.buttonValues = new bool[this.buttonCount];
    this.controller = ReInput.players.GetPlayer(this.playerId).controllers.GetControllerWithTag<CustomController>(this.controllerTag);
    if (this.controller == null)
      Debug.LogError((object) $"A matching controller was not found for tag \"{this.controllerTag}\"");
    if (this.controller.buttonCount != this.buttonValues.Length || this.controller.axisCount != this.axisValues.Length)
      Debug.LogError((object) "Controller has wrong number of elements!");
    if (this.useUpdateCallbacks && this.controller != null)
    {
      this.controller.SetAxisUpdateCallback((Func<int, float>) new Func<int, float>(this.GetAxisValueCallback));
      this.controller.SetButtonUpdateCallback((Func<int, bool>) new Func<int, bool>(this.GetButtonValueCallback));
    }
    this.initialized = true;
  }

  public void Update()
  {
    if (!ReInput.isReady || this.initialized)
      return;
    this.Initialize();
  }

  public void OnInputSourceUpdate()
  {
    this.GetSourceAxisValues();
    this.GetSourceButtonValues();
    if (this.useUpdateCallbacks)
      return;
    this.SetControllerAxisValues();
    this.SetControllerButtonValues();
  }

  public void GetSourceAxisValues()
  {
    for (int index = 0; index < this.axisValues.Length; ++index)
      this.axisValues[index] = index % 2 == 0 ? this.joysticks[index / 2].position.x : this.joysticks[index / 2].position.y;
  }

  public void GetSourceButtonValues()
  {
    for (int index = 0; index < this.buttonValues.Length; ++index)
      this.buttonValues[index] = this.buttons[index].isPressed;
  }

  public void SetControllerAxisValues()
  {
    for (int index = 0; index < this.axisValues.Length; ++index)
      this.controller.SetAxisValue(index, this.axisValues[index]);
  }

  public void SetControllerButtonValues()
  {
    for (int index = 0; index < this.buttonValues.Length; ++index)
      this.controller.SetButtonValue(index, this.buttonValues[index]);
  }

  public float GetAxisValueCallback(int index)
  {
    return index >= this.axisValues.Length ? 0.0f : this.axisValues[index];
  }

  public bool GetButtonValueCallback(int index)
  {
    return index < this.buttonValues.Length && this.buttonValues[index];
  }
}
