// Decompiled with JetBrains decompiler
// Type: BlastPush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class BlastPush : BaseMonoBehaviour, ISpellOwning
{
  [SerializeField]
  public float force = 10f;
  [SerializeField]
  public float radius = 3f;
  [SerializeField]
  public bool dealDamage = true;
  [SerializeField]
  public Health.AttackFlags attackFlags;
  public GameObject owner;
  public float LifeTime = 2f;
  public float Timer;
  public float damageMultiplier = 1f;

  public void Start()
  {
    if (!this.dealDamage)
      return;
    this.StartCoroutine((IEnumerator) this.PushEnemies());
  }

  public void OnDisable() => Object.Destroy((Object) this.gameObject);

  public IEnumerator PushEnemies()
  {
    BlastPush blastPush = this;
    yield return (object) new WaitForSeconds(0.3f);
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(blastPush.owner);
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) blastPush.transform.position, blastPush.radius))
    {
      UnitObject component1 = collider2D.GetComponent<UnitObject>();
      if ((bool) (Object) component1 && component1.health.team == Health.Team.Team2)
      {
        float num = (float) (1.0 - (double) Vector3.Distance(blastPush.transform.position, collider2D.transform.position) / (double) blastPush.radius);
        component1.DoKnockBack(blastPush.gameObject, num * blastPush.force, 1f);
        if (blastPush.IsDeflectingProjectilesPush())
        {
          if (!(component1 is EnemyOogler enemyOogler) || !enemyOogler.IsClone)
            component1.health.DealDamage(EquipmentManager.GetCurseData(farmingComponent.currentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier(farmingComponent) * blastPush.damageMultiplier, blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile, AttackFlags: blastPush.attackFlags);
        }
        else
          component1.health.DealDamage(EquipmentManager.GetCurseData(farmingComponent.currentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier(farmingComponent) * blastPush.damageMultiplier, blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile, AttackFlags: blastPush.attackFlags);
        if (blastPush.IsPoisonPush())
          component1.health.AddPoison(PlayerFarming.Instance.gameObject);
        else if (blastPush.IsIcePush())
          component1.health.AddIce();
        else if (blastPush.IsFlamePush())
          component1.health.AddBurn(blastPush.owner);
      }
      else
      {
        Health component2 = collider2D.GetComponent<Health>();
        if ((bool) (Object) component2)
        {
          if (component2.team == Health.Team.Team2)
          {
            if (blastPush.IsPoisonPush())
              component2.DealDamage(10f * blastPush.damageMultiplier, blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.Poison);
            else if (blastPush.IsIcePush())
              component2.DealDamage(10f * blastPush.damageMultiplier, blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.Ice);
            else if (blastPush.IsFlamePush())
              component2.DealDamage(10f * blastPush.damageMultiplier, blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.Burn);
            else
              component2.DealDamage(10f * blastPush.damageMultiplier, blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile);
          }
          else if (component2.team != Health.Team.PlayerTeam)
            component2.DealDamage(10f * blastPush.damageMultiplier, blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile);
        }
      }
    }
    if (EquipmentManager.GetCurseData(farmingComponent.currentCurse).EquipmentType == EquipmentType.EnemyBlast_DeflectsProjectiles)
      blastPush.KnockbackProjectiles();
    else
      blastPush.DestroyProjectiles();
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.1f);
  }

  public void Update()
  {
    if ((double) (this.Timer += Time.deltaTime) <= (double) this.LifeTime)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public void KnockbackProjectiles()
  {
    Vector3 position1 = this.transform.position;
    foreach (Projectile projectile in Projectile.Projectiles)
    {
      if (!projectile.IsProjectilesParent)
      {
        if (projectile.destroyOnParry)
        {
          projectile.DestroyProjectile();
        }
        else
        {
          Vector3 position2 = projectile.transform.position;
          if ((double) (position1 - position2).magnitude <= (double) this.radius && !projectile.IsAttachedToProjectileTrap())
          {
            float angle = Utils.GetAngle(position1, position2);
            projectile.Angle = angle;
            if ((double) projectile.angleNoiseFrequency == 0.0)
              projectile.SpeedMultiplier = 2f;
            projectile.KnockedBack = true;
            projectile.team = Health.Team.PlayerTeam;
            projectile.health.team = Health.Team.PlayerTeam;
          }
        }
      }
    }
    foreach (Component component1 in Health.team2)
    {
      EnemyOogler component2 = component1.GetComponent<EnemyOogler>();
      if ((Object) component2 != (Object) null && component2.IsClone)
        component2.DeflectClone();
    }
  }

  public void DestroyProjectiles()
  {
    Vector3 position1 = this.transform.position;
    foreach (Projectile projectile in Projectile.Projectiles)
    {
      if (!projectile.IsProjectilesParent && projectile.team != Health.Team.PlayerTeam)
      {
        Vector3 position2 = projectile.transform.position;
        if ((double) (position1 - position2).magnitude <= (double) this.radius && !projectile.IsAttachedToProjectileTrap())
          projectile.DestroyProjectile();
      }
    }
  }

  public bool IsDeflectingProjectilesPush()
  {
    return PlayerFarming.GetPlayerFarmingComponent(this.owner).currentCurse == EquipmentType.EnemyBlast_DeflectsProjectiles;
  }

  public bool IsPoisonPush()
  {
    return PlayerFarming.GetPlayerFarmingComponent(this.owner).currentCurse == EquipmentType.EnemyBlast_Poison && (double) Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.EnemyBlast_Poison).Chance;
  }

  public bool IsIcePush()
  {
    return PlayerFarming.GetPlayerFarmingComponent(this.owner).currentCurse == EquipmentType.EnemyBlast_Ice && (double) Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.EnemyBlast_Ice).Chance;
  }

  public bool IsFlamePush()
  {
    return PlayerFarming.GetPlayerFarmingComponent(this.owner).currentCurse == EquipmentType.EnemyBlast_Flame && (double) Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.EnemyBlast_Flame).Chance;
  }

  public GameObject GetOwner() => this.owner;

  public void SetOwner(GameObject owner) => this.owner = owner;

  public void SetDamageMultiplier(float multiplier = 1f) => this.damageMultiplier = multiplier;
}
