// Decompiled with JetBrains decompiler
// Type: TennisBall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TennisBall : MonoBehaviour, ISpellOwning
{
  public static List<TennisBall> TennisBalls = new List<TennisBall>();
  [HideInInspector]
  public Health targetUnit;
  [HideInInspector]
  public Health ownerUnit;
  public Health health;
  public float turnSpeedBase = 16f;
  public float speedBase = 5f;
  public float speedMax = 8f;
  public float speedVolleyIncrease = 0.25f;
  public AnimationCurve volleyBurst;
  public float volleyBurstMultiplier = 6f;
  public float lifetimePerVolley = 10f;
  public float lifetime;
  public float currentAngle;
  public float speed;
  public float turnSpeed;
  [HideInInspector]
  public float invincibleTime;
  public Transform nav;
  public TrailRenderer goodTrail;
  public TrailRenderer badTrail;
  public TrailRenderer heavyTrail;
  public ParticleSystem vfxHitExplosion;
  public Transform tennisballSprite;
  public Health Origin;
  [HideInInspector]
  public bool isActive;
  [HideInInspector]
  public bool destroyed = true;
  [HideInInspector]
  public bool heavy = true;
  public float volleyCount;
  public Health firstOwner;

  public void OnDestroy()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnHit -= new Health.HitAction(this.OnHit);
  }

  public void OnEnable() => TennisBall.TennisBalls.Add(this);

  public void OnDisable()
  {
    TennisBall.TennisBalls.Remove(this);
    this.InstantRemoveTennisBall();
  }

  public void Init(Health firstTarget, Health firstOwnerVar)
  {
    this.health = this.GetComponent<Health>();
    if ((Object) this.health != (Object) null)
      this.health.OnHit += new Health.HitAction(this.OnHit);
    this.firstOwner = firstOwnerVar;
    this.transform.rotation = Quaternion.identity;
    this.SetTarget(firstTarget, firstOwnerVar);
    this.turnSpeed = this.turnSpeedBase;
    this.speed = this.speedBase;
    this.heavy = false;
    this.volleyCount = 0.0f;
    this.invincibleTime = 0.125f;
    this.Launch(this.firstOwner, firstTarget);
  }

  public void SetTarget(Health target, Health newOwner = null)
  {
    this.targetUnit = target;
    if ((bool) (Object) newOwner)
      this.ownerUnit = newOwner;
    bool flag1 = target.isPlayer || (Object) target.GetComponent<EnemyArcherTennis>() != (Object) null;
    bool flag2 = this.ownerUnit.isPlayer || (Object) this.ownerUnit.GetComponent<EnemyArcherTennis>() != (Object) null;
    Debug.Log((object) ("Target player or tennis? " + flag1.ToString()));
    if (flag1 && flag2)
      return;
    Debug.Log((object) "Somethign has gone wrong");
  }

  public void SetTrailTeam()
  {
    this.goodTrail.emitting = this.ownerUnit.isPlayer && !this.heavy;
    this.badTrail.emitting = !this.ownerUnit.isPlayer && !this.heavy;
    this.heavyTrail.emitting = this.heavy;
  }

  public void Volley(Health attacker, bool heavyAttack = false)
  {
    if (!((Object) attacker != (Object) this.ownerUnit))
      return;
    ++this.volleyCount;
    this.SetTarget(this.ownerUnit, attacker);
    this.turnSpeed += 2f;
    ++this.speed;
    if ((double) this.speed > (double) this.speedMax)
      this.speed = this.speedMax;
    this.heavy = heavyAttack;
    if (this.heavy)
      this.speed = this.speedMax;
    this.Launch(this.ownerUnit, this.targetUnit);
  }

  public void Launch(Health ownerUnitVar, Health targetUnitVar)
  {
    this.ownerUnit = ownerUnitVar;
    this.targetUnit = targetUnitVar;
    this.lifetime = 0.0f;
    this.nav.transform.LookAt(this.targetUnit.transform);
    this.SetTrailTeam();
    this.destroyed = false;
    this.isActive = true;
    this.gameObject.SetActive(true);
    this.tennisballSprite.gameObject.SetActive(true);
  }

  public void RemoveTennisBall()
  {
    this.health = this.GetComponent<Health>();
    if ((Object) this.health != (Object) null)
      this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.destroyed = true;
    this.tennisballSprite.gameObject.SetActive(false);
    this.StartCoroutine(this.RemoveTennisBallCoroutine());
  }

  public IEnumerator RemoveTennisBallCoroutine()
  {
    yield return (object) new WaitForSeconds(1f);
    this.InstantRemoveTennisBall();
  }

  public void InstantRemoveTennisBall()
  {
    this.isActive = false;
    this.gameObject.SetActive(false);
  }

  public void FixedUpdate()
  {
    if (PlayerRelic.TimeFrozen || this.destroyed)
      return;
    this.invincibleTime -= Time.deltaTime;
    this.nav.localPosition = Vector3.zero;
    Quaternion rotation = this.nav.rotation;
    this.nav.LookAt(this.targetUnit.transform);
    this.nav.rotation = Quaternion.Lerp(rotation, this.nav.rotation, Time.deltaTime * this.turnSpeed);
    this.nav.position += this.nav.forward * (this.speed + this.volleyBurst.Evaluate(this.lifetime) * this.volleyBurstMultiplier) * Time.deltaTime;
    this.transform.position = this.nav.position;
    float num1 = Vector3.Distance(this.transform.position, this.targetUnit.transform.position);
    float num2 = 0.3f;
    if ((double) num1 < (double) num2)
    {
      if (this.targetUnit.isPlayer)
      {
        Debug.Log((object) "Is player");
        if ((double) num1 < (double) num2 / 2.0)
        {
          Debug.Log((object) ("Is player vulnerable " + this.targetUnit.state.CURRENT_STATE.ToString()));
          this.targetUnit.DealDamage(1f, this.gameObject, this.transform.position, true, dealDamageImmediately: true);
          this.vfxHitExplosion.Play();
          this.RemoveTennisBall();
        }
      }
      else if ((Object) this.targetUnit == (Object) this.firstOwner)
      {
        Debug.Log((object) "Is not player, ask them if they'll swipe");
        EnemyArcherTennis component1 = this.targetUnit.GetComponent<EnemyArcherTennis>();
        if ((Object) component1 != (Object) null)
        {
          int num3 = 1;
          switch (DifficultyManager.PrimaryDifficulty)
          {
            case DifficultyManager.Difficulty.Medium:
              num3 = 2;
              break;
            case DifficultyManager.Difficulty.Hard:
              num3 = 3;
              break;
          }
          float num4 = component1.chanceOfMissingTennisReturnBase + this.volleyCount * component1.chanceOfMissingTennisReturnIncrease;
          if ((double) this.volleyCount >= (double) num3 || (double) Random.value < (double) num4 || this.heavy)
          {
            Cower component2 = component1.GetComponent<Cower>();
            if ((Object) component2 != (Object) null)
            {
              component2.enabled = true;
              component2.preventStandardStagger = false;
              component2.Health_OnHit(this.gameObject, this.transform.position, Health.AttackTypes.Projectile, false);
              Vector3 position = this.transform.position;
              position.y -= 0.5f;
              position.z -= 0.75f;
              BiomeConstants.Instance.EmitBlockImpact(position, component1.Angle, this.transform, "Break");
            }
            else
              component1.OnHit(this.gameObject, this.transform.position, Health.AttackTypes.Projectile);
            component1.ReturnFire(false);
            this.RemoveTennisBall();
            this.vfxHitExplosion.Play();
          }
          else
          {
            component1.ReturnFire(true);
            this.Volley(this.targetUnit);
          }
        }
        else
          Debug.Log((object) "Tennis ball got confused about owner");
      }
    }
    this.lifetime += Time.deltaTime;
    if ((double) this.lifetime <= (double) this.lifetimePerVolley)
      return;
    this.RemoveTennisBall();
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.isActive || this.destroyed || (double) this.invincibleTime > 0.0)
      return;
    bool usedSpell = false;
    UnitObject attackerUnit = this.GetAttackerUnit(Attacker, out usedSpell);
    if (!((Object) attackerUnit != (Object) null) || !attackerUnit.health.isPlayer || attackerUnit.health.state.CURRENT_STATE == StateMachine.State.Dodging || AttackType != Health.AttackTypes.Melee && AttackType != Health.AttackTypes.Heavy && AttackType != Health.AttackTypes.Projectile && AttackType != Health.AttackTypes.NoReaction && AttackType != Health.AttackTypes.Bullet)
      return;
    this.Volley(attackerUnit.health, AttackType == Health.AttackTypes.Heavy);
    AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
    CameraManager.shakeCamera(5f);
    if (usedSpell)
      return;
    Vector2 dir = (Vector2) (attackerUnit.transform.position - this.transform.position);
    this.StartCoroutine(this.RecoilUnit(attackerUnit, (Vector3) dir, 0.25f, 0.1f));
  }

  public UnitObject GetAttackerUnit(GameObject Attacker, out bool usedSpell)
  {
    UnitObject component1 = Attacker.GetComponent<UnitObject>();
    usedSpell = false;
    if ((Object) component1 == (Object) null)
    {
      IHeavyAttackWeapon component2 = Attacker.GetComponent<IHeavyAttackWeapon>();
      if (component2 != null)
      {
        GameObject owner = component2.GetOwner();
        if ((Object) owner != (Object) null)
          component1 = owner.GetComponent<UnitObject>();
      }
    }
    if ((Object) component1 == (Object) null)
    {
      GameObject spellOwner = Health.GetSpellOwner(Attacker);
      if ((Object) spellOwner != (Object) null)
      {
        usedSpell = true;
        component1 = spellOwner.GetComponent<UnitObject>();
      }
    }
    return component1;
  }

  public IEnumerator RecoilUnit(UnitObject unit, Vector3 dir, float power, float duration)
  {
    float elapsedTime = 0.0f;
    dir.Normalize();
    while ((double) elapsedTime < (double) duration)
    {
      if (SimulationManager.IsPaused)
        yield return (object) null;
      elapsedTime += Time.deltaTime;
      unit.moveVX = dir.x * power;
      unit.moveVY = dir.y * power;
      yield return (object) null;
    }
  }

  public GameObject GetOwner()
  {
    return !((Object) this.Origin != (Object) null) ? (GameObject) null : this.Origin.gameObject;
  }

  public void SetOwner(GameObject owner) => this.Origin = owner.GetComponent<Health>();
}
