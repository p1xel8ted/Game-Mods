// Decompiled with JetBrains decompiler
// Type: EnemyScuttleCharger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyScuttleCharger : EnemyScuttleSwiper
{
  public float IdleSpeed = 0.1f;
  public float ChargeSpeed = 0.1f;
  public float ChargeAcceleration = 0.1f;
  public float StunnedDuration = 1f;
  protected bool IsCharging;
  public float KnockBackModifier = 1f;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.IsCharging || ((int) this.layerToCheck & 1 << collision.gameObject.layer) == 0)
      return;
    this.ChargeIntoWall();
  }

  protected override IEnumerator AttackRoutine()
  {
    EnemyScuttleCharger enemyScuttleCharger = this;
    enemyScuttleCharger.Attacking = true;
    enemyScuttleCharger.ClearPaths();
    enemyScuttleCharger.speed = 0.0f;
    enemyScuttleCharger.Spine.AnimationState.SetAnimation(0, enemyScuttleCharger.SignPostAttackAnimation, enemyScuttleCharger.LoopSignPostAttackAnimation);
    enemyScuttleCharger.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) enemyScuttleCharger.SignPostAttackDuration)
    {
      if ((UnityEngine.Object) enemyScuttleCharger.TargetObject != (UnityEngine.Object) null && (double) Progress < (double) enemyScuttleCharger.SignPostAttackDuration - 0.20000000298023224)
        enemyScuttleCharger.state.LookAngle = Utils.GetAngle(enemyScuttleCharger.transform.position, enemyScuttleCharger.TargetObject.transform.position);
      foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleCharger.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / enemyScuttleCharger.SignPostAttackDuration);
      enemyScuttleCharger.state.facingAngle = enemyScuttleCharger.state.LookAngle;
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleCharger.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    enemyScuttleCharger.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    enemyScuttleCharger.Spine.AnimationState.SetAnimation(0, enemyScuttleCharger.AttackAnimation, false);
    enemyScuttleCharger.Spine.AnimationState.AddAnimation(0, enemyScuttleCharger.IdleAnimation, true, 0.0f);
    float timer = 0.0f;
    enemyScuttleCharger.damageColliderEvents.SetActive(true);
    enemyScuttleCharger.DisableKnockback = true;
    enemyScuttleCharger.IsCharging = true;
    while ((double) timer < (double) enemyScuttleCharger.AttackDuration)
    {
      if (!enemyScuttleCharger.IsCharging)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleCharger.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(0.0f);
        yield break;
      }
      enemyScuttleCharger.maxSpeed = Mathf.Lerp(enemyScuttleCharger.maxSpeed, enemyScuttleCharger.ChargeSpeed, Time.deltaTime * enemyScuttleCharger.ChargeAcceleration);
      enemyScuttleCharger.speed = enemyScuttleCharger.maxSpeed;
      if ((double) timer > (double) enemyScuttleCharger.AttackDuration * 0.89999997615814209)
      {
        enemyScuttleCharger.damageColliderEvents.SetActive(false);
        foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleCharger.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(0.0f);
      }
      else
      {
        foreach (SimpleSpineFlash simpleSpineFlash in enemyScuttleCharger.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(0.8f);
      }
      timer += Time.deltaTime;
      yield return (object) null;
    }
    enemyScuttleCharger.EndCharge();
  }

  protected override bool ShouldAttack()
  {
    if (this.IsStunned)
      return false;
    Vector3 viewportPoint = Camera.main.WorldToViewportPoint(this.transform.position);
    return ((double) viewportPoint.x <= 0.0 || (double) viewportPoint.x >= 1.0 || (double) viewportPoint.y <= 0.0 ? 0 : ((double) viewportPoint.y < 1.0 ? 1 : 0)) != 0 && base.ShouldAttack();
  }

  protected virtual void ChargeIntoWall()
  {
    CameraManager.shakeCamera(0.2f, this.state.LookAngle);
    if (!this.IsStunned)
      this.StartCoroutine((IEnumerator) this.StunnedRoutine());
    this.EndCharge();
  }

  protected void EndCharge()
  {
    this.IsCharging = false;
    this.DisableKnockback = false;
    this.damageColliderEvents.SetActive(false);
    this.maxSpeed = this.IdleSpeed;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.IdleWait = this.StunnedDuration;
    this.AttackDelay = this.AttackDelayTime;
    this.TargetObject = (GameObject) null;
    this.Attacking = false;
  }

  protected void Stop()
  {
  }

  protected IEnumerator StunnedRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyScuttleCharger enemyScuttleCharger = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyScuttleCharger.IsStunned = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyScuttleCharger.IsStunned = true;
    enemyScuttleCharger.StartCoroutine((IEnumerator) enemyScuttleCharger.ApplyForceRoutine(enemyScuttleCharger.transform.position + new Vector3(Mathf.Cos(enemyScuttleCharger.state.facingAngle * ((float) Math.PI / 180f)), Mathf.Sin(enemyScuttleCharger.state.facingAngle) * ((float) Math.PI / 180f))));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyScuttleCharger.StunnedDuration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.IsStunned = false;
    if (this.DisableKnockback || AttackType == Health.AttackTypes.NoKnockBack)
      return;
    this.DoKnockBack(Attacker, this.KnockBackModifier, 0.5f);
  }
}
