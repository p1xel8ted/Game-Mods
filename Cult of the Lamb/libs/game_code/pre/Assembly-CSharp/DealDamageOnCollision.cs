// Decompiled with JetBrains decompiler
// Type: DealDamageOnCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DealDamageOnCollision : BaseMonoBehaviour
{
  private Health EnemyHealth;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    this.EnemyHealth = collision.gameObject.GetComponent<Health>();
    if (!((Object) this.EnemyHealth != (Object) null))
      return;
    this.EnemyHealth.DealDamage(2f, this.gameObject, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, this.EnemyHealth.transform.position));
    if (this.EnemyHealth.team != Health.Team.PlayerTeam)
      return;
    GameManager.GetInstance().HitStop();
  }
}
