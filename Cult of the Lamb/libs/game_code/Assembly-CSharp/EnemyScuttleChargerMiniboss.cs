// Decompiled with JetBrains decompiler
// Type: EnemyScuttleChargerMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyScuttleChargerMiniboss : EnemyScuttleCharger
{
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IntroAnimation;
  public float turningSpeedWhileCharging = 10f;
  public bool IsTargetingPlayer;
  public GameObject Arrow;
  public float ShootDelay;
  public int ShotsToFire = 3;
  public float TimeBetweenShoots = 1f;

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.StartHidden != EnemyScuttleSwiper.StartingStates.Intro)
      return;
    this.StartCoroutine((IEnumerator) this.IntroRoutine());
  }

  public IEnumerator IntroRoutine()
  {
    EnemyScuttleChargerMiniboss scuttleChargerMiniboss = this;
    yield return (object) new WaitForEndOfFrame();
    scuttleChargerMiniboss.health.enabled = false;
    scuttleChargerMiniboss.Spine.AnimationState.SetAnimation(0, scuttleChargerMiniboss.IntroAnimation, false);
    scuttleChargerMiniboss.Spine.AnimationState.AddAnimation(0, scuttleChargerMiniboss.IdleAnimation, true, 0.0f);
    scuttleChargerMiniboss.AttackDelay = 0.0f;
    yield return (object) new WaitForSeconds(1.5f);
    scuttleChargerMiniboss.StartCoroutine((IEnumerator) scuttleChargerMiniboss.ActiveRoutine());
  }

  public override IEnumerator AttackRoutine()
  {
    EnemyScuttleChargerMiniboss scuttleChargerMiniboss = this;
    scuttleChargerMiniboss.Attacking = true;
    scuttleChargerMiniboss.GetNewTarget();
    scuttleChargerMiniboss.Spine.AnimationState.SetAnimation(0, scuttleChargerMiniboss.SignPostAttackAnimation, scuttleChargerMiniboss.LoopSignPostAttackAnimation);
    scuttleChargerMiniboss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) scuttleChargerMiniboss.SignPostAttackDuration)
    {
      if ((Object) scuttleChargerMiniboss.TargetObject != (Object) null)
        scuttleChargerMiniboss.state.LookAngle = Utils.GetAngle(scuttleChargerMiniboss.transform.position, scuttleChargerMiniboss.TargetObject.transform.position);
      foreach (SimpleSpineFlash simpleSpineFlash in scuttleChargerMiniboss.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / scuttleChargerMiniboss.SignPostAttackDuration);
      scuttleChargerMiniboss.state.facingAngle = scuttleChargerMiniboss.state.LookAngle;
      yield return (object) null;
    }
    scuttleChargerMiniboss.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    scuttleChargerMiniboss.Spine.AnimationState.SetAnimation(0, scuttleChargerMiniboss.AttackAnimation, false);
    scuttleChargerMiniboss.Spine.AnimationState.AddAnimation(0, scuttleChargerMiniboss.IdleAnimation, true, 0.0f);
    float timer = 0.0f;
    scuttleChargerMiniboss.damageColliderEvents.SetActive(true);
    scuttleChargerMiniboss.IsCharging = true;
    scuttleChargerMiniboss.DisableKnockback = true;
    scuttleChargerMiniboss.IsTargetingPlayer = true;
    while ((double) timer < (double) scuttleChargerMiniboss.AttackDuration)
    {
      if (!scuttleChargerMiniboss.IsCharging)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in scuttleChargerMiniboss.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(0.0f);
        yield break;
      }
      if (scuttleChargerMiniboss.IsTargetingPlayer)
        scuttleChargerMiniboss.TurnTowardsTarget();
      scuttleChargerMiniboss.maxSpeed = Mathf.Lerp(scuttleChargerMiniboss.maxSpeed, scuttleChargerMiniboss.ChargeSpeed, Time.deltaTime * scuttleChargerMiniboss.ChargeAcceleration);
      scuttleChargerMiniboss.speed = scuttleChargerMiniboss.maxSpeed;
      if ((double) timer > (double) scuttleChargerMiniboss.AttackDuration * 0.89999997615814209)
      {
        scuttleChargerMiniboss.damageColliderEvents.SetActive(false);
        foreach (SimpleSpineFlash simpleSpineFlash in scuttleChargerMiniboss.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(0.0f);
      }
      else
      {
        foreach (SimpleSpineFlash simpleSpineFlash in scuttleChargerMiniboss.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(0.5f);
      }
      timer += Time.deltaTime;
      yield return (object) null;
    }
    scuttleChargerMiniboss.EndCharge();
  }

  public override void ChargeIntoWall()
  {
    base.ChargeIntoWall();
    CameraManager.shakeCamera(1f, this.state.LookAngle);
    this.StartCoroutine((IEnumerator) this.ShootArrowRoutine());
  }

  public void TurnTowardsTarget()
  {
    if ((Object) this.TargetObject == (Object) null)
      return;
    float speedWhileCharging = this.turningSpeedWhileCharging;
    if ((double) Mathf.Abs(this.state.LookAngle - Utils.GetAngle(this.transform.position, this.TargetObject.transform.position)) > 180.0)
      speedWhileCharging /= 2f;
    this.state.LookAngle = Mathf.LerpAngle(this.state.LookAngle, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position), Time.deltaTime * speedWhileCharging);
    this.state.facingAngle = this.state.LookAngle;
  }

  public override void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((Object) this.EnemyHealth != (Object) null) || this.EnemyHealth.team == this.health.team && this.health.team != Health.Team.PlayerTeam || !this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f)))
      return;
    this.IsTargetingPlayer = false;
  }

  public IEnumerator ShootArrowRoutine()
  {
    EnemyScuttleChargerMiniboss scuttleChargerMiniboss = this;
    int i = scuttleChargerMiniboss.ShotsToFire;
    float angle = 0.0f;
    while (--i >= 0)
    {
      float Progress = 0.0f;
      while ((double) (Progress += Time.deltaTime) < (double) scuttleChargerMiniboss.TimeBetweenShoots)
        yield return (object) null;
      Projectile component = ObjectPool.Spawn(scuttleChargerMiniboss.Arrow, scuttleChargerMiniboss.transform.parent).GetComponent<Projectile>();
      component.transform.position = scuttleChargerMiniboss.transform.position;
      component.Angle = angle;
      component.team = scuttleChargerMiniboss.health.team;
      component.Speed = 6f;
      component.Owner = scuttleChargerMiniboss.health;
      angle += (float) (360.0 / ((double) scuttleChargerMiniboss.ShotsToFire + 1.0));
    }
  }
}
