// Decompiled with JetBrains decompiler
// Type: DamageCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DamageCollider : BaseMonoBehaviour
{
  public Health health;
  public Collider2D damageCollider;
  public float Damage = 1f;
  public float DamageToObstacles = 100f;
  public float Knockback;
  public bool BreakArmor;
  public bool IgnorePlayer;
  public bool DeflectBullets;
  public bool DestroyBullets;
  public bool PlayersOnly;
  public bool UseTriggerCollisionWithProjectiles;
  public Health.AttackTypes AttackType;
  public Health.AttackFlags AttackFlags;
  public bool DealDamageToAllNonEnemyHealth;
  public List<Health> hitEnemies = new List<Health>();
  public List<Projectile> hitProjectiles = new List<Projectile>();

  public void Start() => this.health = this.GetComponentInParent<Health>();

  public void OnTriggerEnter2D(Collider2D collision)
  {
    Health component = collision.gameObject.GetComponent<Health>();
    if ((Object) component != (Object) null && ((Object) this.health == (Object) null || component.team != this.health.team || component.team == Health.Team.PlayerTeam && this.health.IsCharmedEnemy))
    {
      if (component.team == Health.Team.Team2 && this.PlayersOnly || this.IgnorePlayer && (component.isPlayer || component.isPlayerAlly) || this.hitEnemies.Contains(component))
        return;
      this.hitEnemies.Add(this.health);
      if (collision.gameObject.layer == LayerMask.NameToLayer("Scenery") || collision.gameObject.layer == LayerMask.NameToLayer("Obstacles") || this.DealDamageToAllNonEnemyHealth && component.team != Health.Team.Team2)
      {
        component.DealDamage(this.DamageToObstacles, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.8f), this.BreakArmor, this.AttackType, AttackFlags: this.AttackFlags);
      }
      else
      {
        component.DealDamage(this.Damage, this.gameObject, this.transform.position, this.BreakArmor, this.AttackType, AttackFlags: this.AttackFlags);
        if ((double) this.Knockback == 0.0)
          return;
        component.GetComponent<UnitObject>()?.DoKnockBack(this.gameObject, this.Knockback, 1f);
      }
    }
    else
    {
      if (!this.UseTriggerCollisionWithProjectiles)
        return;
      Projectile componentInParent = collision.GetComponentInParent<Projectile>();
      if (!(bool) (Object) componentInParent || componentInParent.IsProjectilesParent || this.hitProjectiles.Contains(componentInParent))
        return;
      this.hitProjectiles.Add(componentInParent);
      if ((bool) (Object) componentInParent && this.DeflectBullets)
        this.DeflectProjectile(componentInParent);
      this.DamageProjetile(componentInParent);
    }
  }

  public void TriggerCheckCollision()
  {
    if (this.UseTriggerCollisionWithProjectiles)
      return;
    for (int index = 0; index < Projectile.Projectiles.Count; ++index)
    {
      Projectile projectile = Projectile.Projectiles[index];
      if (!projectile.IsProjectilesParent && !this.hitProjectiles.Contains(projectile) && this.damageCollider.OverlapPoint((Vector2) projectile.transform.position))
      {
        this.hitProjectiles.Add(projectile);
        if (this.DeflectBullets)
          this.DeflectProjectile(projectile);
        this.DamageProjetile(projectile);
      }
    }
  }

  public void DamageProjetile(Projectile projectile)
  {
    if ((bool) (Object) projectile && this.DestroyBullets && !projectile.IsAttachedToProjectileTrap() && !projectile.IsProjectilesParent)
      projectile.DestroyProjectile();
    if (!(bool) (Object) this.health || this.health.team != Health.Team.Neutral)
      return;
    this.health.DealDamage(10f, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile);
  }

  public void DeflectProjectile(Projectile projectile)
  {
    if (projectile.IsAttachedToProjectileTrap() || projectile.IsProjectilesParent)
      return;
    float angle = Utils.GetAngle(this.transform.position, projectile.transform.position);
    projectile.Angle = angle;
    if ((double) projectile.angleNoiseFrequency == 0.0)
      projectile.Speed *= 2f;
    projectile.KnockedBack = true;
    projectile.health.team = Health.Team.PlayerTeam;
    projectile.team = Health.Team.PlayerTeam;
  }
}
