// Decompiled with JetBrains decompiler
// Type: TrapChain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TrapChain : BaseMonoBehaviour
{
  public Transform[] Chomp;
  private float Orbit;
  public float OrbitSpeed = 1f;
  public float Distance = 2f;
  private Health EnemyHealth;

  private void Start() => this.Orbit = (float) UnityEngine.Random.Range(0, 360);

  private void Update()
  {
    this.Orbit += this.OrbitSpeed * GameManager.DeltaTime;
    int index = -1;
    while (++index < this.Chomp.Length)
      this.Chomp[index].localPosition = new Vector3(this.Distance * Mathf.Cos((float) (((double) this.Orbit + (double) (360 / this.Chomp.Length * index)) * (Math.PI / 180.0))), this.Distance * Mathf.Sin((float) (((double) this.Orbit + (double) (360 / this.Chomp.Length * index)) * (Math.PI / 180.0))));
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.Distance, Color.red);
  }

  private void OnCollisionEnter2D(Collision2D collision)
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
