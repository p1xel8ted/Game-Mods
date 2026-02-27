// Decompiled with JetBrains decompiler
// Type: DamageCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DamageCollider : BaseMonoBehaviour
{
  private Health health;
  public float Damage = 1f;
  public float DamageToObstacles = 100f;
  public float Knockback;
  public bool BreakArmor;
  public bool IgnorePlayer;
  public bool DeflectBullets;
  public bool DestroyBullets;
  public Health.AttackTypes AttackType;
  private List<Health> hitEnemies = new List<Health>();
  private List<Projectile> hitProjectiles = new List<Projectile>();

  private void Start() => this.health = this.GetComponentInParent<Health>();

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Health component = collision.gameObject.GetComponent<Health>();
    if ((Object) component != (Object) null && ((Object) this.health == (Object) null || component.team != this.health.team))
    {
      if (this.IgnorePlayer && (Object) component == (Object) PlayerFarming.Instance.health || this.hitEnemies.Contains(component))
        return;
      this.hitEnemies.Add(this.health);
      if (collision.gameObject.layer == LayerMask.NameToLayer("Scenery") || collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
      {
        component.DealDamage(this.DamageToObstacles, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.8f), this.BreakArmor, this.AttackType);
      }
      else
      {
        component.DealDamage(this.Damage, this.gameObject, this.transform.position, this.BreakArmor, this.AttackType);
        if ((double) this.Knockback == 0.0)
          return;
        component.GetComponent<UnitObject>()?.DoKnockBack(this.gameObject, this.Knockback, 1f);
      }
    }
    else
    {
      Projectile componentInParent = collision.GetComponentInParent<Projectile>();
      if (!(bool) (Object) componentInParent || this.hitProjectiles.Contains(componentInParent))
        return;
      this.hitProjectiles.Add(componentInParent);
      float angle = Utils.GetAngle(this.transform.position, componentInParent.transform.position);
      if ((bool) (Object) componentInParent)
      {
        componentInParent.Angle = angle;
        if ((double) componentInParent.angleNoiseFrequency == 0.0)
          componentInParent.Speed *= 2f;
        if (this.DeflectBullets)
        {
          componentInParent.KnockedBack = true;
          componentInParent.team = Health.Team.PlayerTeam;
        }
      }
      if ((bool) (Object) componentInParent && this.DestroyBullets)
        componentInParent.DestroyProjectile(true);
      if (!(bool) (Object) this.health || this.health.team != Health.Team.Neutral)
        return;
      this.health.DealDamage(10f, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile);
    }
  }
}
