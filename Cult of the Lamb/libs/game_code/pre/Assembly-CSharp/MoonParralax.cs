// Decompiled with JetBrains decompiler
// Type: MoonParralax
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MoonParralax : BaseMonoBehaviour
{
  private GameObject Cam;
  private Vector3 Groundposition;
  public GameObject ShowGroundPos;
  private Vector3 Position;
  private CameraFollowTarget CameraFollowTarget;
  public Vector2 Parralax;
  public float YOffset;

  private void Update()
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
