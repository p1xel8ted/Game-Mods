// Decompiled with JetBrains decompiler
// Type: DealDamageOnCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DealDamageOnCollision : BaseMonoBehaviour
{
  public bool NoDamageWhileDodging;
  public bool IncludeStayingInCollider;
  public int Damage = 2;
  public Health EnemyHealth;
  public bool DamageDealt;

  public void CheckCollision(GameObject collisionGameObject)
  {
    if (!this.IncludeStayingInCollider || this.DamageDealt)
      return;
    this.EnemyHealth = collisionGameObject.GetComponent<Health>();
    if (!((Object) this.EnemyHealth != (Object) null) || this.NoDamageWhileDodging && this.EnemyHealth.state.CURRENT_STATE == StateMachine.State.Dodging)
      return;
    this.DamageDealt = true;
    this.EnemyHealth.DealDamage((float) this.Damage, this.gameObject, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, this.EnemyHealth.transform.position));
    if (this.EnemyHealth.team != Health.Team.PlayerTeam)
      return;
    GameManager.GetInstance().HitStop();
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (this.IncludeStayingInCollider)
      return;
    this.CheckCollision(collision.gameObject);
  }

  public void OnCollisionStay2D(Collision2D collision) => this.CheckCollision(collision.gameObject);

  public void OnCollisionExit2D(Collision2D collision) => this.DamageDealt = false;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.IncludeStayingInCollider)
      return;
    this.CheckCollision(collision.gameObject);
  }

  public void OnTriggerStay2D(Collider2D collision) => this.CheckCollision(collision.gameObject);

  public void OnTriggerExit2D(Collider2D collision) => this.DamageDealt = false;
}
