// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.CustomControllersTiltDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class CustomControllersTiltDemo : MonoBehaviour
{
  public Transform target;
  public float speed = 10f;
  public CustomController controller;
  public Player player;

  public void Awake()
  {
    Screen.orientation = ScreenOrientation.LandscapeLeft;
    this.player = ReInput.players.GetPlayer(0);
    ReInput.InputSourceUpdateEvent += (Action) new Action(this.OnInputUpdate);
    this.controller = (CustomController) this.player.controllers.GetControllerWithTag(ControllerType.Custom, "TiltController");
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    Vector3 zero = Vector3.zero with
    {
      y = this.player.GetAxis("Tilt Vertical"),
      x = this.player.GetAxis("Tilt Horizontal")
    };
    if ((double) zero.sqrMagnitude > 1.0)
      zero.Normalize();
    zero *= Time.deltaTime;
    this.target.Translate(zero * this.speed);
  }

  public void OnInputUpdate()
  {
    Vector3 acceleration = Input.acceleration;
    this.controller.SetAxisValue(0, acceleration.x);
    this.controller.SetAxisValue(1, acceleration.y);
    this.controller.SetAxisValue(2, acceleration.z);
  }
}
