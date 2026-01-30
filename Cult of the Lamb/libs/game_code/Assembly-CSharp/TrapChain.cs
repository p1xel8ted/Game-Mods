// Decompiled with JetBrains decompiler
// Type: TrapChain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TrapChain : BaseMonoBehaviour
{
  public Transform[] Chomp;
  public float Orbit;
  public float OrbitSpeed = 1f;
  public float Distance = 2f;
  public Health EnemyHealth;

  public void Start() => this.Orbit = (float) UnityEngine.Random.Range(0, 360);

  public void Update()
  {
    this.Orbit += this.OrbitSpeed * GameManager.DeltaTime;
    int index = -1;
    while (++index < this.Chomp.Length)
      this.Chomp[index].localPosition = new Vector3(this.Distance * Mathf.Cos((float) (((double) this.Orbit + (double) (360 / this.Chomp.Length * index)) * (Math.PI / 180.0))), this.Distance * Mathf.Sin((float) (((double) this.Orbit + (double) (360 / this.Chomp.Length * index)) * (Math.PI / 180.0))));
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.Distance, Color.red);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    this.EnemyHealth = collision.gameObject.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null))
      return;
    Debug.Log((object) "COLLISION!");
    this.EnemyHealth.DealDamage(2f, this.gameObject, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, this.EnemyHealth.transform.position));
    if (this.EnemyHealth.team != Health.Team.PlayerTeam)
      return;
    GameManager.GetInstance().HitStop();
  }
}
