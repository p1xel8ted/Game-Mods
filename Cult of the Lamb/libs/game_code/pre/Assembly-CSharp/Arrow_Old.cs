// Decompiled with JetBrains decompiler
// Type: Arrow_Old
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Arrow_Old : Bullet
{
  private float vz;
  private float grav = 3f / 1000f;
  private float displayAngle;

  private void Start()
  {
    this.vz = this.grav * (Vector2.Distance((Vector2) this.target.transform.position, (Vector2) this.transform.position) * 2f / this.Speed) / 2f;
  }

  public override void Move()
  {
    this.vz -= this.grav;
    this.displayAngle = Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(this.Speed * Mathf.Cos(this.angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(this.angle * ((float) Math.PI / 180f)) + this.vz, 0.0f));
    this.angle = Utils.GetAngle(this.transform.position, this.target.transform.position);
    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.displayAngle);
    this.transform.position = this.transform.position + new Vector3(this.Speed * Mathf.Cos(this.angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(this.angle * ((float) Math.PI / 180f)) + this.vz, 0.0f);
  }
}
