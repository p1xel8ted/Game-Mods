// Decompiled with JetBrains decompiler
// Type: AreaBurnTick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (ColliderEvents))]
public class AreaBurnTick : MonoBehaviour
{
  [SerializeField]
  public bool isCollisionBased = true;
  [SerializeField]
  public float tickIntervalPerEnemy;
  [SerializeField]
  public float tickDamage;
  [SerializeField]
  public float burnDuration;
  [SerializeField]
  public Health ownerHealth;
  public Health.Team team = Health.Team.Team2;
  public bool burnOwnerForDuration = true;
  public bool areaAffectsOwner;
  public bool canDamage = true;
  public bool setActiveOnInitialize;
  public bool ignoreOtherAreaBurnTick;
  public ColliderEvents damageColliderEvents;
  public List<AreaBurnTick.IntervalHitEvent> burnedEnemies;
  public Health.AttackFlags attackFlags = Health.AttackFlags.Burn;

  public void Initialize()
  {
    this.burnedEnemies = new List<AreaBurnTick.IntervalHitEvent>();
    this.damageColliderEvents = this.GetComponent<ColliderEvents>();
    if (this.isCollisionBased)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.TryBurnTarget);
      this.damageColliderEvents.OnTriggerStayEvent += new ColliderEvents.TriggerEvent(this.TryBurnTarget);
      this.damageColliderEvents.OnTriggerExitEvent += new ColliderEvents.TriggerEvent(this.RemoveBurn);
    }
    else
      this.damageColliderEvents.OnTriggerStayEvent += new ColliderEvents.TriggerEvent(this.TryAttackTarget);
    this.damageColliderEvents.SetActive(this.setActiveOnInitialize);
  }

  public void SetOwner(Health health)
  {
    this.ownerHealth = health;
    if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      this.team = health.team;
    else
      this.team = Health.Team.Team2;
  }

  public void Cleanup()
  {
    this.burnedEnemies.Clear();
    this.damageColliderEvents.OnTriggerStayEvent -= new ColliderEvents.TriggerEvent(this.TryAttackTarget);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.TryBurnTarget);
    this.damageColliderEvents.OnTriggerStayEvent -= new ColliderEvents.TriggerEvent(this.TryBurnTarget);
    this.damageColliderEvents.OnTriggerExitEvent -= new ColliderEvents.TriggerEvent(this.RemoveBurn);
  }

  public void TryAttackTarget(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if ((UnityEngine.Object) this.ownerHealth != (UnityEngine.Object) null && (UnityEngine.Object) component == (UnityEngine.Object) this.ownerHealth && !this.areaAffectsOwner)
      return;
    Health.Team team = !((UnityEngine.Object) this.ownerHealth != (UnityEngine.Object) null) ? this.team : this.ownerHealth.team;
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.invincible || component.untouchable || (team != Health.Team.Team2 || !component.IsCharmedEnemy && component.team == Health.Team.Team2) && (team != Health.Team.PlayerTeam || component.team == Health.Team.PlayerTeam) && team != Health.Team.KillAll || component.IsBurned)
      return;
    this.ProcessDamageTick(component);
  }

  public void ProcessDamageTick(Health targetHealth)
  {
    AreaBurnTick.IntervalHitEvent intervalHitEvent = this.burnedEnemies.FirstOrDefault<AreaBurnTick.IntervalHitEvent>((Func<AreaBurnTick.IntervalHitEvent, bool>) (hitEvent => (UnityEngine.Object) hitEvent.HitTargetHealth == (UnityEngine.Object) targetHealth));
    if (intervalHitEvent == null)
    {
      this.burnedEnemies.Add(new AreaBurnTick.IntervalHitEvent(Time.unscaledTime, targetHealth));
      this.TryDealDamage(targetHealth);
    }
    else
    {
      if ((double) intervalHitEvent.UnscaledTimestamp >= (double) Time.unscaledTime - (double) this.tickIntervalPerEnemy)
        return;
      this.TryDealDamage(targetHealth);
      intervalHitEvent.UnscaledTimestamp = Time.unscaledTime;
    }
  }

  public void TryDealDamage(Health targetHealth)
  {
    if ((double) targetHealth._HP <= 0.0)
      return;
    if (this.canDamage && (!this.ignoreOtherAreaBurnTick || (UnityEngine.Object) targetHealth.GetComponentInChildren<AreaBurnTick>() == (UnityEngine.Object) null))
      targetHealth.DealDamage(this.tickDamage, this.gameObject, Vector3.Lerp(this.transform.position, targetHealth.transform.position, 0.7f), AttackFlags: this.attackFlags);
    targetHealth.AddBurn(this.gameObject, this.burnDuration, this.attackFlags);
    UnitObject component = targetHealth.GetComponent<UnitObject>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !(component is IWoodmanFlammableUnit woodmanFlammableUnit))
      return;
    woodmanFlammableUnit.TrySetFire();
  }

  public void TryBurnTarget(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if ((UnityEngine.Object) this.ownerHealth != (UnityEngine.Object) null && (UnityEngine.Object) component == (UnityEngine.Object) this.ownerHealth && !this.areaAffectsOwner)
      return;
    Health.Team team = !((UnityEngine.Object) this.ownerHealth != (UnityEngine.Object) null) ? this.team : this.ownerHealth.team;
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.invincible || component.untouchable || (team != Health.Team.Team2 || !component.IsCharmedEnemy && component.team == Health.Team.Team2) && (team != Health.Team.PlayerTeam || component.team == Health.Team.PlayerTeam) && team != Health.Team.KillAll || component.IsBurned || component.team == Health.Team.PlayerTeam && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (LetterBox.IsPlaying || PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.CustomAnimation))
      return;
    if ((UnityEngine.Object) this.ownerHealth != (UnityEngine.Object) null)
      component.AddBurn(this.ownerHealth.gameObject, attackFlags: this.attackFlags);
    else
      component.AddBurn(this.gameObject, attackFlags: this.attackFlags);
  }

  public void RemoveBurn(Collider2D collider)
  {
    Health.Team team = !((UnityEngine.Object) this.ownerHealth != (UnityEngine.Object) null) ? this.team : this.ownerHealth.team;
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == team)
      return;
    component.RemoveBurn();
  }

  public void EnableDamage(
    float damage,
    float tickInterval,
    float duration,
    Health.AttackFlags extraAttackFlags = (Health.AttackFlags) 0)
  {
    this.attackFlags |= extraAttackFlags;
    if (this.burnOwnerForDuration)
      this.ownerHealth?.AddBurn(this.gameObject, duration, this.attackFlags);
    this.tickIntervalPerEnemy = tickInterval;
    this.tickDamage = damage;
    this.burnDuration = duration;
    this.damageColliderEvents.SetActive(true);
  }

  public void DisableDamage()
  {
    this.burnedEnemies.Clear();
    this.damageColliderEvents.SetActive(false);
  }

  public class IntervalHitEvent
  {
    public float UnscaledTimestamp;
    public Health HitTargetHealth;

    public IntervalHitEvent(float timestamp, Health health)
    {
      this.UnscaledTimestamp = timestamp;
      this.HitTargetHealth = health;
    }
  }
}
