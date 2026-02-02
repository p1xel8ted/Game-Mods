// Decompiled with JetBrains decompiler
// Type: BarrierAbility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BarrierAbility : BaseMonoBehaviour, ISpellOwning
{
  [SerializeField]
  public EquipmentType barrierType;
  [SerializeField]
  public Health health;
  [SerializeField]
  public float stompRadius = 1f;
  [SerializeField]
  public float activeTime = 10f;
  [SerializeField]
  public float absrobRadius;
  [SerializeField]
  public Collider2D colldier;
  [SerializeField]
  public GameObject particle;
  public List<Projectile> collidedProjectiles = new List<Projectile>();
  public float stompDamage = 1f;
  public GameObject owner;
  public Health ownerHealth;
  public float activeTimer;

  public EquipmentType BarrierType => this.barrierType;

  public bool showAbsorbParameters => this.barrierType == EquipmentType.Barrier_Absorb;

  public void Setup(
    GameObject owner,
    float spellDamage,
    float moveDistance,
    float moveDuration,
    float showHideDuration)
  {
    this.SetOwner(owner);
    this.ownerHealth = owner.GetComponent<Health>();
    this.health.team = this.ownerHealth.team;
    this.stompDamage = spellDamage;
    this.health.HP = this.health.totalHP;
    this.activeTimer = 0.0f;
    this.particle.transform.parent = (Transform) null;
    this.particle.transform.position = owner.transform.position;
    if (this.health.team == Health.Team.PlayerTeam)
      this.health.ImmuneToPlayer = true;
    this.CreateStomp();
    this.transform.DOMove(this.transform.position + this.transform.right * moveDistance, moveDuration);
    this.transform.localScale = Vector3.zero;
    this.transform.DOScale(1f, showHideDuration);
    this.transform.DOScale(0.0f, showHideDuration).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(moveDuration - showHideDuration * 2f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => Object.Destroy((Object) this.gameObject)));
  }

  public void OnDisable() => this.FreeProjectiles();

  public void OnDestroy() => this.collidedProjectiles.Clear();

  public void Update()
  {
    if ((double) this.activeTime <= 0.0)
      return;
    if ((double) this.activeTimer >= (double) this.activeTime)
      this.gameObject.Recycle();
    this.activeTimer += Time.deltaTime;
  }

  public void FixedUpdate()
  {
    foreach (Projectile projectile in Projectile.Projectiles)
    {
      if (!this.collidedProjectiles.Contains(projectile) && !projectile.IsProjectilesParent && projectile.team != this.health.team && !projectile.IsAttachedToProjectileTrap())
      {
        if (this.barrierType == EquipmentType.Barrier_Absorb)
          this.AbsorbProjectile(projectile);
        else if (this.barrierType == EquipmentType.Barrier_Deflection)
          this.DeflectProjectile(projectile);
        else
          this.DestroyProjectile(projectile);
      }
    }
    foreach (TennisBall tennisBall in TennisBall.TennisBalls)
      this.DeflectTennisBall(tennisBall);
    foreach (KnockableMortarBomb knockableMortarBomb in KnockableMortarBomb.KnockableMortarBombs)
    {
      if ((Object) knockableMortarBomb != (Object) null)
        this.DeflectKockableMortar(knockableMortarBomb);
    }
  }

  public void CreateStomp()
  {
    List<Health> healthList = new List<Health>((IEnumerable<Health>) Health.neutralTeam);
    healthList.AddRange((IEnumerable<Health>) Health.team2);
    BiomeConstants.Instance.EmitGroundSmashVFXParticles(this.transform.position, 0.7f);
    for (int index = 0; index < healthList.Count; ++index)
    {
      if ((Object) healthList[index] != (Object) null && (double) Vector2.Distance((Vector2) healthList[index].transform.position, (Vector2) this.transform.position) <= (double) this.stompRadius)
        healthList[index].DealDamage(this.stompDamage, this.owner, this.transform.position);
    }
  }

  public void DeflectTennisBall(TennisBall tennisBall)
  {
    if (tennisBall.health.team == this.health.team || !this.colldier.OverlapPoint((Vector2) tennisBall.transform.position))
      return;
    tennisBall.health.DealDamage(1f, this.owner, this.transform.position);
  }

  public void DeflectKockableMortar(KnockableMortarBomb mortar)
  {
    if (mortar.health.team == this.health.team || !this.IsPositionWithinColliderBoundsAndHeight(mortar.Spine.transform.position))
      return;
    mortar.health.DealDamage(1f, this.owner, this.transform.position);
    mortar.OnHit(this.owner, this.transform.position, Health.AttackTypes.Melee);
  }

  public void DeflectProjectile(Projectile projectile)
  {
    if (this.collidedProjectiles.Contains(projectile) || !this.colldier.OverlapPoint((Vector2) projectile.transform.position))
      return;
    this.collidedProjectiles.Add(projectile);
    this.health.DealDamage(1f, projectile.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile);
    if (projectile.destroyOnParry)
    {
      projectile.DestroyProjectile();
    }
    else
    {
      if (projectile.IsAttachedToProjectileTrap())
        return;
      float num = projectile.Angle + 180f;
      projectile.Angle = num;
      if ((double) projectile.angleNoiseFrequency == 0.0)
        projectile.Speed *= 2f;
      projectile.KnockedBack = true;
      projectile.team = this.health.team;
      projectile.health.team = this.health.team;
    }
  }

  public void DestroyProjectile(Projectile projectile)
  {
    if (this.collidedProjectiles.Contains(projectile) || !this.colldier.OverlapPoint((Vector2) projectile.transform.position))
      return;
    this.collidedProjectiles.Add(projectile);
    this.health.DealDamage(1f, projectile.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile);
    this.collidedProjectiles.Remove(projectile);
    projectile.DestroyProjectile();
  }

  public void AbsorbProjectile(Projectile projectile)
  {
    if ((double) (this.transform.position - projectile.transform.position).magnitude <= (double) this.absrobRadius && (double) this.health.CurrentHP > 0.0)
    {
      if (projectile.IsAttachedToProjectileTrap())
        return;
      projectile.turningSpeedMultiplier = 4f;
      projectile.temporaryHomeInOnTarget = true;
      projectile.SetTarget(this.health);
    }
    else
      this.FreeProjectile(projectile);
    this.DestroyProjectile(projectile);
  }

  public void FreeProjectiles()
  {
    if (this.barrierType != EquipmentType.Barrier_Absorb)
      return;
    foreach (Projectile projectile in Projectile.Projectiles)
      this.FreeProjectile(projectile);
  }

  public void FreeProjectile(Projectile projectile)
  {
    if (!((Object) projectile.GetTarget() == (Object) this.health))
      return;
    projectile.turningSpeedMultiplier = 1f;
    projectile.temporaryHomeInOnTarget = false;
    projectile.SetTarget((Health) null);
  }

  public GameObject GetOwner() => this.owner;

  public void SetOwner(GameObject owner) => this.owner = owner;

  public bool IsActive()
  {
    return (double) this.health.CurrentHP > 0.0 && this.gameObject.activeInHierarchy;
  }

  public bool IsPositionWithinColliderBoundsAndHeight(Vector3 position)
  {
    return this.colldier.bounds.Contains((Vector3) (Vector2) position) & (double) position.z <= 2.0;
  }

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(this.transform.position, this.stompRadius);
    if (this.barrierType != EquipmentType.Barrier_Absorb)
      return;
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(this.transform.position, this.absrobRadius);
  }

  [CompilerGenerated]
  public void \u003CSetup\u003Eb__16_0() => Object.Destroy((Object) this.gameObject);
}
