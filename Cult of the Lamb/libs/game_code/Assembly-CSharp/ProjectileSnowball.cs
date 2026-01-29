// Decompiled with JetBrains decompiler
// Type: ProjectileSnowball
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ProjectileSnowball : Projectile
{
  public string snowballImpactGroundSFX = "event:/dlc/material/snowball_impact_ground";
  public string snowballImpactUnit = "event:/dlc/material/snowball_impact_follower";

  public override void OnCollisionWithPlayer(Health targetHealth, bool collideImmediately = false)
  {
    if (this.destroyed)
      return;
    if (!collideImmediately)
    {
      if (this.collisionEventQueue != null)
        return;
      this.collisionEventQueue = new Projectile.CollisionEvent(Time.unscaledTime, targetHealth);
    }
    else
    {
      if ((Object) targetHealth != (Object) null && targetHealth.state.CURRENT_STATE != StateMachine.State.Dodging)
      {
        if (targetHealth.isPlayer)
          CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, targetHealth.transform.position));
        AudioManager.Instance.PlayOneShot(this.snowballImpactUnit, this.transform.position);
        Projectile.OnHitUnit onHitPlayer = this.onHitPlayer;
        if (onHitPlayer != null)
          onHitPlayer((Projectile) this);
        this.DestroyProjectile();
      }
      if (!collideImmediately)
        return;
      this.collisionEventQueue = (Projectile.CollisionEvent) null;
    }
  }

  public override void EndOfLife()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    AudioManager.Instance.PlayOneShot(this.snowballImpactGroundSFX, this.transform.position);
    if ((Object) this.ArrowImage != (Object) null)
      this.EmitParticle(0.5f);
    this.DestroyProjectile();
  }

  public override void OnRayEnter2D(Collider2D collider)
  {
    int instanceId = collider.gameObject.GetInstanceID();
    Projectile cachedComponent1 = this.GetCachedComponent<Projectile>(Projectile.ProjectileComponents, instanceId, collider.gameObject);
    if (this.destroyed || (Object) collider == (Object) null || (Object) cachedComponent1 != (Object) null || this.destroyed)
      return;
    Health cachedComponent2 = this.GetCachedComponent<Health>(Projectile.HealthComponents, instanceId, collider.gameObject);
    if ((Object) cachedComponent2 != (Object) null && cachedComponent2.enabled && (Object) cachedComponent2 != (Object) this.health && (cachedComponent2.team != this.team || cachedComponent2.IsCharmedEnemy) && !cachedComponent2.untouchable && !cachedComponent2.invincible && !cachedComponent2.IgnoreProjectiles && ((Object) cachedComponent2.state == (Object) null || cachedComponent2.state.CURRENT_STATE != StateMachine.State.Dodging) && (cachedComponent2.team != Health.Team.Neutral && ((Object) this.ArrowImage == (Object) null || this.ArrowImage.gameObject.activeSelf) && !cachedComponent2.invincible && !cachedComponent2.CompareTag("ProjectileIgnore") || cachedComponent2.team == Health.Team.Neutral && (double) this.DamageToNeutral > 0.0 && !cachedComponent2.CompareTag("BreakableDecoration")))
    {
      if ((Object) cachedComponent2 == (Object) this.Owner && cachedComponent2.IsCharmed || cachedComponent2.isPlayer && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, PlayerFarming.GetPlayerFarmingComponent(cachedComponent2.gameObject)) && this.IsAttachedToProjectileTrap())
        return;
      cachedComponent2.DealDamage(cachedComponent2.team == Health.Team.Neutral ? this.DamageToNeutral : this.Damage, this.gameObject, this.transform.position, AttackType: this.NoKnockback ? Health.AttackTypes.NoKnockBack : Health.AttackTypes.Projectile, AttackFlags: this.AttackFlags);
      this.OnCollisionWithPlayer(cachedComponent2, false);
    }
    else
    {
      if (!((Object) collider != (Object) null))
        return;
      if ((Object) this.Owner != (Object) null && (Object) this.targetObject != (Object) null && (Object) collider.gameObject == (Object) this.Owner.gameObject && (Object) this.targetObject.gameObject == (Object) this.Owner.gameObject)
      {
        Projectile.OnHitUnit onHitOwner = this.onHitOwner;
        if (onHitOwner != null)
          onHitOwner((Projectile) this);
      }
      if (this.IgnoreIsland || collider.gameObject.layer != this.layerObstacles && collider.gameObject.layer != this.layerIsland)
        return;
      if (this.bouncesRemaining <= 0)
      {
        if (!this.DestroyOnWallHit || this.IsAttachedToProjectileTrap())
          return;
        if (!this.CollideOnlyTargets || !SettingsManager.Settings.Game.PerformanceMode)
        {
          this.EmitParticle();
          if ((bool) (Object) this.ArrowImage)
            this.hitParticleObj = ObjectPool.Spawn(this.hitPrefab, this.ArrowImage.position, Quaternion.identity);
          this.SpawnChunks(collider.transform.position);
        }
        CameraManager.shakeCamera(0.2f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, collider.transform.position));
        AudioManager.Instance.PlayOneShot(this.snowballImpactGroundSFX, this.transform.position);
        this.DestroyProjectile();
        if (!this.Explosive)
          return;
        Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 4f, 1f, 1f, attackFlags: this.AttackFlags);
      }
      else
      {
        Vector3 vector2 = (Vector3) Utils.DegreeToVector2(this.Angle);
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) vector2, 1f, (int) this.bounceMask);
        if (!(bool) raycastHit2D)
          return;
        this.Angle = Utils.GetAngle(Vector3.zero, Vector3.Reflect(vector2, (Vector3) raycastHit2D.normal));
        --this.bouncesRemaining;
      }
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.canParry || (double) GameManager.GetInstance().CurrentTime < (double) this.spawnTimestamp + (double) this.InvincibleTime || !this.CanKnockBack)
      return;
    PlayerFarming component = Attacker.GetComponent<PlayerFarming>();
    if ((Object) component == (Object) null)
      return;
    this.KnockedBack = true;
    Projectile.OnKnockedBack onKnockedBack = this.onKnockedBack;
    if (onKnockedBack != null)
      onKnockedBack();
    this.team = Health.Team.PlayerTeam;
    this.health.team = Health.Team.PlayerTeam;
    if (this.destroyOnParry)
    {
      if (AttackType == Health.AttackTypes.Melee && !this.destroyed)
        this.EmitParticle();
      AudioManager.Instance.PlayOneShot(this.snowballImpactUnit, this.transform.position);
      CameraManager.shakeCamera(0.1f);
      this.DestroyProjectile();
    }
    else if (AttackType == Health.AttackTypes.Projectile)
    {
      CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, AttackLocation));
      Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 3f, attackFlags: this.AttackFlags);
      AudioManager.Instance.PlayOneShot(this.snowballImpactUnit, this.transform.position);
      this.DestroyProjectile();
    }
    else
    {
      GameManager.GetInstance().HitStop();
      CameraManager.shakeCamera(0.2f, (float) Random.Range(0, 360));
      if (!this.destroyed)
        this.EmitParticle();
      this.Angle += 180f;
      this.Speed = 15f;
      this.Damage *= 3f;
      this.LifeTime = 3f;
      if ((Object) component != (Object) null)
        this.ForgiveCollisionWithPlayer();
      if ((Object) this.Owner != (Object) null)
      {
        this.Angle = Utils.GetAngle(this.transform.position, this.Owner.transform.position);
        if (!this.ForceHomeInKnockback)
          return;
        if (!string.IsNullOrEmpty(this.OnKnockbackSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnKnockbackSoundPath, this.gameObject);
        this.SetTarget(this.Owner);
        this.turningSpeed = float.MaxValue;
        this.CollideOnlyTarget = true;
        if (!this.OverrideSpeedHomeIn)
          return;
        this.Speed = this.HomeInSpeed;
      }
      else
      {
        Health health = (Health) null;
        float num1 = float.MaxValue;
        float num2 = 0.0f;
        float num3 = 90f;
        if (!string.IsNullOrEmpty(this.OnKnockbackSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnKnockbackSoundPath, this.gameObject);
        foreach (Health allUnit in Health.allUnits)
        {
          float angle = Utils.GetAngle(this.transform.position, allUnit.transform.position);
          float num4 = Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.transform.position);
          if ((Object) allUnit != (Object) this.health && !allUnit.InanimateObject && allUnit.team == Health.Team.Team2 && (double) num4 < 8.0 && (double) num4 < (double) num1 && (double) Mathf.Abs(this.Angle - angle) < 180.0 && (double) angle > (double) this.Angle - (double) num3 && (double) angle < (double) this.Angle + (double) num3)
          {
            health = allUnit;
            num1 = num4;
            num2 = angle;
          }
        }
        if (!((Object) health != (Object) null))
          return;
        this.Angle = num2;
      }
    }
  }
}
