// Decompiled with JetBrains decompiler
// Type: YT_RotateCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class YT_RotateCamera : MonoBehaviour
{
  public float speedH = 2f;
  public float speedV = 2f;
  public float yaw;
  public float pitch;

  public void Update()
  {
    this.yaw += this.speedH * Input.GetAxis("Mouse X");
    this.pitch -= this.speedV * Input.GetAxis("Mouse Y");
    this.transform.eulerAngles = new Vector3(this.pitch, this.yaw, 0.0f);
  }
}
