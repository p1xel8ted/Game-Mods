// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.FallbackJoystickIdentificationDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class FallbackJoystickIdentificationDemo : MonoBehaviour
{
  private const float windowWidth = 250f;
  private const float windowHeight = 250f;
  private const float inputDelay = 1f;
  private bool identifyRequired;
  private Queue<Joystick> joysticksToIdentify;
  private float nextInputAllowedTime;
  private GUIStyle style;

  private void Awake()
  {
    if (!ReInput.unityJoystickIdentificationRequired)
      return;
    ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.JoystickConnected);
    ReInput.ControllerDisconnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.JoystickDisconnected);
    this.IdentifyAllJoysticks();
  }

  private void JoystickConnected(ControllerStatusChangedEventArgs args)
  {
    this.IdentifyAllJoysticks();
  }

  private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
  {
    this.IdentifyAllJoysticks();
  }

  public void IdentifyAllJoysticks()
  {
    this.Reset();
    if (ReInput.controllers.joystickCount == 0)
      return;
    Joystick[] joysticks = ReInput.controllers.GetJoysticks();
    if (joysticks == null)
      return;
    this.identifyRequired = true;
    this.joysticksToIdentify = new Queue<Joystick>((IEnumerable<Joystick>) joysticks);
    this.SetInputDelay();
  }

  private void SetInputDelay() => this.nextInputAllowedTime = Time.time + 1f;

  private void OnGUI()
  {
    if (!this.identifyRequired)
      return;
    if (this.joysticksToIdentify == null || this.joysticksToIdentify.Count == 0)
    {
      this.Reset();
    }
    else
    {
      GUILayout.Window(0, new Rect((float) ((double) Screen.width * 0.5 - 125.0), (float) ((double) Screen.height * 0.5 - 125.0), 250f, 250f), new GUI.WindowFunction(this.DrawDialogWindow), "Joystick Identification Required");
      GUI.FocusWindow(0);
      if ((double) Time.time < (double) this.nextInputAllowedTime || !ReInput.controllers.SetUnityJoystickIdFromAnyButtonOrAxisPress(this.joysticksToIdentify.Peek().id, 0.8f, false))
        return;
      this.joysticksToIdentify.Dequeue();
      this.SetInputDelay();
      if (this.joysticksToIdentify.Count != 0)
        return;
      this.Reset();
    }
  }

  private void DrawDialogWindow(int windowId)
  {
    if (!this.identifyRequired)
      return;
    if (this.style == null)
    {
      this.style = new GUIStyle(GUI.skin.label);
      this.style.wordWrap = true;
    }
    GUILayout.Space(15f);
    GUILayout.Label("A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:", this.style);
    GUILayout.Label($"Press any button on \"{this.joysticksToIdentify.Peek().name}\" now.", this.style);
    GUILayout.FlexibleSpace();
    if (!GUILayout.Button("Skip"))
      return;
    this.joysticksToIdentify.Dequeue();
  }

  private void Reset()
  {
    this.joysticksToIdentify = (Queue<Joystick>) null;
    this.identifyRequired = false;
  }
}
