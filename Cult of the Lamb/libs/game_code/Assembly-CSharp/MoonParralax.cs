// Decompiled with JetBrains decompiler
// Type: MoonParralax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MoonParralax : BaseMonoBehaviour
{
  public GameObject Cam;
  public Vector3 Groundposition;
  public GameObject ShowGroundPos;
  public Vector3 Position;
  public CameraFollowTarget CameraFollowTarget;
  public Vector2 Parralax;
  public float YOffset;

  public void Update()
  {
    if ((UnityEngine.Object) this.Cam == (UnityEngine.Object) null)
    {
      this.Cam = Camera.main.gameObject;
      this.CameraFollowTarget = this.Cam.GetComponent<CameraFollowTarget>();
    }
    if ((UnityEngine.Object) this.CameraFollowTarget == (UnityEngine.Object) null)
      return;
    this.Groundposition = Camera.main.transform.position - new Vector3(0.0f, this.CameraFollowTarget.distance * Mathf.Sin((float) Math.PI / 180f * this.CameraFollowTarget.angle), -this.CameraFollowTarget.distance * Mathf.Cos((float) Math.PI / 180f * this.CameraFollowTarget.angle));
    this.Groundposition = this.Groundposition with
    {
      z = 0.0f
    };
    this.ShowGroundPos.transform.position = this.Groundposition;
    this.Position = this.transform.position;
    this.Position.x = this.Groundposition.x * this.Parralax.x;
    this.Position.y = (this.Groundposition.y + this.YOffset) * this.Parralax.y;
    this.transform.position = this.Position;
  }
}
