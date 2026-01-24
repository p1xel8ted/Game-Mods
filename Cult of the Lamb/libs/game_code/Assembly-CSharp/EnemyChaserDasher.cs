// Decompiled with JetBrains decompiler
// Type: EnemyChaserDasher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyChaserDasher : EnemyChaser
{
  [SerializeField]
  public float attackDistance;
  [SerializeField]
  public float dashChargeDur;
  [SerializeField]
  public float dashStrength;
  [SerializeField]
  public float dashCooldown;
  [SerializeField]
  public float damageDuration = 0.2f;
  public new ColliderEvents damageColliderEvents;
  public float attackTimer;
  public bool attacking;

  public override void Awake()
  {
    base.Awake();
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.Charge();
  }

  public void Charge()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Charging)
      return;
    this.attackTimer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Charging;
  }

  public override void Update()
  {
    base.Update();
    if (this.attacking)
      return;
    if (this.state.CURRENT_STATE == StateMachine.State.Charging)
    {
      this.attackTimer += Time.deltaTime;
      float num = this.attackTimer / this.dashChargeDur;
      this.simpleSpineFlash.FlashWhite(num * 0.75f);
      if ((double) num > 1.0)
        this.Attack();
    }
    if (!(bool) (UnityEngine.Object) this.targetObject || (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) >= (double) this.attackDistance)
      return;
    this.Charge();
  }

  public override void UpdateMoving()
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Charging || this.attacking)
      return;
    base.UpdateMoving();
  }

  public void Attack()
  {
    if (this.attacking)
      return;
    this.attackTimer = 0.0f;
    this.attacking = true;
    this.simpleSpineFlash.FlashWhite(false);
    this.ClearPaths();
    this.DoKnockBack(Utils.GetAngle(this.transform.position, this.targetObject.transform.position) * ((float) Math.PI / 180f), this.dashStrength, this.dashCooldown);
    this.StartCoroutine((IEnumerator) this.TurnOnDamageColliderForDuration(this.damageDuration));
    this.StartCoroutine((IEnumerator) this.AttackCooldownIE());
  }

  public IEnumerator AttackCooldownIE()
  {
    EnemyChaserDasher enemyChaserDasher = this;
    yield return (object) new WaitForEndOfFrame();
    enemyChaserDasher.simpleSpineFlash.FlashWhite(false);
    yield return (object) new WaitForSeconds(enemyChaserDasher.dashCooldown);
    enemyChaserDasher.attacking = false;
    enemyChaserDasher.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public new void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive(true);
    yield return (object) new WaitForSeconds(duration);
    this.damageColliderEvents.SetActive(false);
  }
}
