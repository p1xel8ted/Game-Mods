// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.DualShock4SpecialFeaturesExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.ControllerExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class DualShock4SpecialFeaturesExample : MonoBehaviour
{
  public const int maxTouches = 2;
  public int playerId;
  public Transform touchpadTransform;
  public GameObject lightObject;
  public Transform accelerometerTransform;
  public List<DualShock4SpecialFeaturesExample.Touch> touches;
  public Queue<DualShock4SpecialFeaturesExample.Touch> unusedTouches;
  public bool isFlashing;
  public GUIStyle textStyle;

  public Player player => ReInput.players.GetPlayer(this.playerId);

  public void Awake() => this.InitializeTouchObjects();

  public void Update()
  {
    if (!ReInput.isReady)
      return;
    IDualShock4Extension firstDs4 = this.GetFirstDS4(this.player);
    if (firstDs4 != null)
    {
      this.transform.rotation = firstDs4.GetOrientation();
      this.HandleTouchpad(firstDs4);
      this.accelerometerTransform.LookAt(this.accelerometerTransform.position + firstDs4.GetAccelerometerValue());
    }
    if (this.player.GetButtonDown("CycleLight"))
      this.SetRandomLightColor();
    if (this.player.GetButtonDown("ResetOrientation"))
      this.ResetOrientation();
    if (this.player.GetButtonDown("ToggleLightFlash"))
    {
      if (this.isFlashing)
        this.StopLightFlash();
      else
        this.StartLightFlash();
      this.isFlashing = !this.isFlashing;
    }
    if (this.player.GetButtonDown("VibrateLeft"))
      firstDs4.SetVibration(0, 1f, 1f);
    if (!this.player.GetButtonDown("VibrateRight"))
      return;
    firstDs4.SetVibration(1, 1f, 1f);
  }

  public void OnGUI()
  {
    if (this.textStyle == null)
    {
      this.textStyle = new GUIStyle(GUI.skin.label);
      this.textStyle.fontSize = 20;
      this.textStyle.wordWrap = true;
    }
    if (this.GetFirstDS4(this.player) == null)
      return;
    GUILayout.BeginArea(new Rect(200f, 100f, (float) Screen.width - 400f, (float) Screen.height - 200f));
    GUILayout.Label("Rotate the Dual Shock 4 to see the model rotate in sync.", this.textStyle);
    GUILayout.Label("Touch the touchpad to see them appear on the model.", this.textStyle);
    ActionElementMap elementMapWithAction1 = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "ResetOrientation", true);
    if (elementMapWithAction1 != null)
      GUILayout.Label($"Press {elementMapWithAction1.elementIdentifierName} to reset the orientation. Hold the gamepad facing the screen with sticks pointing up and press the button.", this.textStyle);
    ActionElementMap elementMapWithAction2 = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "CycleLight", true);
    if (elementMapWithAction2 != null)
      GUILayout.Label($"Press {elementMapWithAction2.elementIdentifierName} to change the light color.", this.textStyle);
    ActionElementMap elementMapWithAction3 = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "ToggleLightFlash", true);
    if (elementMapWithAction3 != null)
      GUILayout.Label($"Press {elementMapWithAction3.elementIdentifierName} to start or stop the light flashing.", this.textStyle);
    ActionElementMap elementMapWithAction4 = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "VibrateLeft", true);
    if (elementMapWithAction4 != null)
      GUILayout.Label($"Press {elementMapWithAction4.elementIdentifierName} vibrate the left motor.", this.textStyle);
    ActionElementMap elementMapWithAction5 = this.player.controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, "VibrateRight", true);
    if (elementMapWithAction5 != null)
      GUILayout.Label($"Press {elementMapWithAction5.elementIdentifierName} vibrate the right motor.", this.textStyle);
    GUILayout.EndArea();
  }

  public void ResetOrientation() => this.GetFirstDS4(this.player)?.ResetOrientation();

  public void SetRandomLightColor()
  {
    IDualShock4Extension firstDs4 = this.GetFirstDS4(this.player);
    if (firstDs4 == null)
      return;
    Color color = new Color(UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f), 1f);
    firstDs4.SetLightColor(color);
    this.lightObject.GetComponent<MeshRenderer>().material.color = color;
  }

  public void StartLightFlash()
  {
    if (!(this.GetFirstDS4(this.player) is DualShock4Extension firstDs4))
      return;
    firstDs4.SetLightFlash(0.5f, 0.5f);
  }

  public void StopLightFlash()
  {
    if (!(this.GetFirstDS4(this.player) is DualShock4Extension firstDs4))
      return;
    firstDs4.StopLightFlash();
  }

  public IDualShock4Extension GetFirstDS4(Player player)
  {
    foreach (Controller joystick in (IEnumerable<Joystick>) player.controllers.Joysticks)
    {
      IDualShock4Extension extension = joystick.GetExtension<IDualShock4Extension>();
      if (extension != null)
        return extension;
    }
    return (IDualShock4Extension) null;
  }

  public void InitializeTouchObjects()
  {
    this.touches = new List<DualShock4SpecialFeaturesExample.Touch>(2);
    this.unusedTouches = new Queue<DualShock4SpecialFeaturesExample.Touch>(2);
    for (int index = 0; index < 2; ++index)
    {
      DualShock4SpecialFeaturesExample.Touch touch = new DualShock4SpecialFeaturesExample.Touch()
      {
        go = GameObject.CreatePrimitive(PrimitiveType.Sphere)
      };
      touch.go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
      touch.go.transform.SetParent(this.touchpadTransform, true);
      touch.go.GetComponent<MeshRenderer>().material.color = index == 0 ? Color.red : Color.green;
      touch.go.SetActive(false);
      this.unusedTouches.Enqueue(touch);
    }
  }

  public void HandleTouchpad(IDualShock4Extension ds4)
  {
    for (int index = this.touches.Count - 1; index >= 0; --index)
    {
      DualShock4SpecialFeaturesExample.Touch touch = this.touches[index];
      if (!ds4.IsTouchingByTouchId(touch.touchId))
      {
        touch.go.SetActive(false);
        this.unusedTouches.Enqueue(touch);
        this.touches.RemoveAt(index);
      }
    }
    for (int index = 0; index < ds4.maxTouches; ++index)
    {
      if (ds4.IsTouching(index))
      {
        int touchId = ds4.GetTouchId(index);
        DualShock4SpecialFeaturesExample.Touch touch = this.touches.Find((Predicate<DualShock4SpecialFeaturesExample.Touch>) (x => x.touchId == touchId));
        if (touch == null)
        {
          touch = this.unusedTouches.Dequeue();
          this.touches.Add(touch);
        }
        touch.touchId = touchId;
        touch.go.SetActive(true);
        Vector2 position;
        ds4.GetTouchPosition(index, out position);
        touch.go.transform.localPosition = new Vector3(position.x - 0.5f, (float) (0.5 + (double) touch.go.transform.localScale.y * 0.5), position.y - 0.5f);
      }
    }
  }

  public class Touch
  {
    public GameObject go;
    public int touchId = -1;
  }
}
