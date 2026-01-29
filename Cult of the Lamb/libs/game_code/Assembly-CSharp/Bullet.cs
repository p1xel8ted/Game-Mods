// Decompiled with JetBrains decompiler
// Type: Bullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Bullet : BaseMonoBehaviour
{
  public Health target;
  public float Damage = 1f;
  public float Speed = 0.05f;
  public float angle;

  public void Start()
  {
  }

  public void Update()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.Move();
      if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.target.transform.position) > (double) this.Speed)
        return;
      this.target.DealDamage(this.Damage, this.gameObject, this.transform.position);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
  }

  public virtual void Move()
  {
    this.angle = Utils.GetAngle(this.transform.position, this.target.transform.position);
    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.angle);
    this.transform.position = this.transform.position + new Vector3(this.Speed * Mathf.Cos(this.angle * ((float) Math.PI / 180f)), this.Speed * Mathf.Sin(this.angle * ((float) Math.PI / 180f)), 0.0f);
  }
}
