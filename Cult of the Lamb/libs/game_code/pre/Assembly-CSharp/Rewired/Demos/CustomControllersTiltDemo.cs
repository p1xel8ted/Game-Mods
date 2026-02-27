// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.CustomControllersTiltDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class CustomControllersTiltDemo : MonoBehaviour
{
  public Transform target;
  public float speed = 10f;
  private CustomController controller;
  private Player player;

  private void Awake()
  {
    Screen.orientation = ScreenOrientation.LandscapeLeft;
    this.player = ReInput.players.GetPlayer(0);
    ReInput.InputSourceUpdateEvent += (Action) new Action(this.OnInputUpdate);
    this.controller = (CustomController) this.player.controllers.GetControllerWithTag(ControllerType.Custom, "TiltController");
  }

  private void Update()
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

  private void OnInputUpdate()
  {
    Vector3 acceleration = Input.acceleration;
    this.controller.SetAxisValue(0, acceleration.x);
    this.controller.SetAxisValue(1, acceleration.y);
    this.controller.SetAxisValue(2, acceleration.z);
  }
}
