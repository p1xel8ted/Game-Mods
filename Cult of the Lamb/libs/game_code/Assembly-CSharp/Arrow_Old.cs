// Decompiled with JetBrains decompiler
// Type: Arrow_Old
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Arrow_Old : Bullet
{
  public float vz;
  public float grav = 3f / 1000f;
  public float displayAngle;

  public new void Start()
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
