// Decompiled with JetBrains decompiler
// Type: DamageCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
  public static int SceneryLayer;
  public static int ObstaclesLayer;

  public void Start()
  {
    this.health = this.GetComponentInParent<Health>();
    DamageCollider.SceneryLayer = LayerMask.NameToLayer("Scenery");
    DamageCollider.ObstaclesLayer = LayerMask.NameToLayer("Obstacles");
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    GameObject gameObject = collision.gameObject;
    int layer = gameObject.layer;
    Health component1;
    if (gameObject.TryGetComponent<Health>(out component1) && ((Object) this.health == (Object) null || component1.team != this.health.team || component1.team == Health.Team.PlayerTeam && this.health.IsCharmedEnemy))
    {
      if (component1.team == Health.Team.Team2 && this.PlayersOnly || this.IgnorePlayer && (component1.isPlayer || component1.isPlayerAlly) || this.hitEnemies.Contains(component1))
        return;
      this.hitEnemies.Add(this.health);
      if (((layer == DamageCollider.SceneryLayer ? 1 : (layer == DamageCollider.ObstaclesLayer ? 1 : 0)) | (!this.DealDamageToAllNonEnemyHealth ? (false ? 1 : 0) : (component1.team != Health.Team.Team2 ? 1 : 0))) != 0)
      {
        component1.DealDamage(this.DamageToObstacles, this.gameObject, Vector3.Lerp(this.transform.position, component1.transform.position, 0.8f), this.BreakArmor, this.AttackType, component1.team != Health.Team.PlayerTeam, this.AttackFlags);
      }
      else
      {
        component1.DealDamage(this.Damage, this.gameObject, this.transform.position, this.BreakArmor, this.AttackType, component1.team != Health.Team.PlayerTeam, this.AttackFlags);
        UnitObject component2;
        if ((double) this.Knockback == 0.0 || !component1.TryGetComponent<UnitObject>(out component2))
          return;
        component2.DoKnockBack(this.gameObject, this.Knockback, 1f);
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
    this.health.DealDamage(10f, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile, dealDamageImmediately: true);
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
