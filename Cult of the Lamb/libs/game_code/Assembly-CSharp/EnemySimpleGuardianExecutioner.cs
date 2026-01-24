// Decompiled with JetBrains decompiler
// Type: EnemySimpleGuardianExecutioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySimpleGuardianExecutioner : EnemySimpleGuardian
{
  public string attackMeleeBSFX = "event:/dlc/dungeon06/enemy/skeleton_guardian/attack_melee_b";
  public string attackRangedStartSFX = "event:/dlc/dungeon06/enemy/skeleton_guardian/attack_ranged_start";

  public override IEnumerator FightPlayer()
  {
    EnemySimpleGuardianExecutioner guardianExecutioner = this;
    while (EnemySimpleGuardian.SimpleGuardians.Count <= 1 && ((UnityEngine.Object) guardianExecutioner.TargetObject == (UnityEngine.Object) null || (double) Vector3.Distance(guardianExecutioner.TargetObject.transform.position, guardianExecutioner.transform.position) > 12.0))
    {
      if ((UnityEngine.Object) guardianExecutioner.TargetObject != (UnityEngine.Object) null)
        guardianExecutioner.LookAtTarget();
      yield return (object) null;
    }
    guardianExecutioner.GetPath();
    float RepathTimer = 0.0f;
    int NumAttacks = guardianExecutioner.NumberOfAttacks;
    guardianExecutioner.LocalAttackDelay = UnityEngine.Random.Range(guardianExecutioner.AttackRandomDelay.x, guardianExecutioner.AttackRandomDelay.y);
    float AttackSpeed = guardianExecutioner.SpeedOfAttack;
    bool Loop = true;
    while (Loop)
    {
      switch (guardianExecutioner.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
        case StateMachine.State.Moving:
          guardianExecutioner.LookAtTarget();
          if ((double) Vector2.Distance((Vector2) guardianExecutioner.transform.position, (Vector2) guardianExecutioner.TargetObject.transform.position) < 3.0)
          {
            if ((double) GameManager.GetInstance().CurrentTime > ((double) EnemySimpleGuardian.LastSimpleGuardianAttacked + (double) guardianExecutioner.GlobalAttackDelay + (double) guardianExecutioner.LocalAttackDelay) / (double) guardianExecutioner.Spine.timeScale)
            {
              guardianExecutioner.LookAtTarget();
              DataManager.Instance.LastSimpleGuardianAttacked = TimeManager.TotalElapsedGameTime;
              EnemySimpleGuardian.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
              guardianExecutioner.LocalAttackDelay = UnityEngine.Random.Range(guardianExecutioner.AttackRandomDelay.x, guardianExecutioner.AttackRandomDelay.y);
              guardianExecutioner.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              guardianExecutioner.Spine.AnimationState.SetAnimation(0, "attack" + (4 - NumAttacks).ToString(), false);
              guardianExecutioner.Spine.AnimationState.AddAnimation(0, guardianExecutioner.idleAnim, true, 0.0f);
              if (!string.IsNullOrEmpty(guardianExecutioner.AttackSwipeSFX))
                guardianExecutioner.swipeInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(guardianExecutioner.AttackSwipeSFX, guardianExecutioner.transform);
              if (!string.IsNullOrEmpty(guardianExecutioner.WarningVO))
                AudioManager.Instance.PlayOneShot(guardianExecutioner.WarningVO, guardianExecutioner.transform.position);
            }
            else if (guardianExecutioner.state.CURRENT_STATE != StateMachine.State.Idle)
            {
              guardianExecutioner.state.CURRENT_STATE = StateMachine.State.Idle;
              guardianExecutioner.Spine.AnimationState.SetAnimation(0, guardianExecutioner.idleAnim, true);
            }
          }
          if (guardianExecutioner.CanDoRingShot && (double) Vector2.Distance((Vector2) guardianExecutioner.transform.position, (Vector2) guardianExecutioner.TargetObject.transform.position) >= 5.0 && (double) GameManager.GetInstance().CurrentTime > ((double) EnemySimpleGuardian.LastSimpleGuardianRingProjectiles + (double) guardianExecutioner.GlobalRingAttackDelay) / (double) guardianExecutioner.Spine.timeScale)
          {
            DataManager.Instance.LastSimpleGuardianRingProjectiles = TimeManager.TotalElapsedGameTime;
            EnemySimpleGuardian.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
            guardianExecutioner.ProjectileRings();
            yield break;
          }
          if (guardianExecutioner.ChasingPlayer)
          {
            if ((double) Vector2.Distance((Vector2) guardianExecutioner.transform.position, (Vector2) guardianExecutioner.TargetObject.transform.position) >= 3.0 && (double) (RepathTimer += Time.deltaTime * guardianExecutioner.Spine.timeScale) > 0.20000000298023224)
            {
              RepathTimer = 0.0f;
              guardianExecutioner.GetPath();
            }
          }
          else if (guardianExecutioner.state.CURRENT_STATE == StateMachine.State.Idle)
          {
            if (guardianExecutioner.Spine.AnimationName != guardianExecutioner.idleAnim)
              guardianExecutioner.Spine.AnimationState.SetAnimation(0, guardianExecutioner.idleAnim, true);
            if ((double) (RepathTimer += Time.deltaTime * guardianExecutioner.Spine.timeScale) > 1.0)
            {
              RepathTimer = 0.0f;
              guardianExecutioner.GetPath();
            }
          }
          if ((UnityEngine.Object) guardianExecutioner.damageColliderEvents != (UnityEngine.Object) null)
          {
            guardianExecutioner.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          guardianExecutioner.LookAtTarget();
          if (guardianExecutioner.UpdateAngleAfterAttack && (UnityEngine.Object) guardianExecutioner.TargetObject != (UnityEngine.Object) null)
          {
            if ((double) guardianExecutioner.state.Timer + (double) Time.deltaTime * (double) guardianExecutioner.Spine.timeScale < 0.25)
              guardianExecutioner.state.facingAngle = Utils.GetAngle(guardianExecutioner.transform.position, guardianExecutioner.TargetObject.transform.position);
          }
          else
            guardianExecutioner.state.facingAngle = Utils.GetAngle(guardianExecutioner.transform.position, guardianExecutioner.TargetPosition);
          if ((double) (guardianExecutioner.state.Timer += Time.deltaTime * guardianExecutioner.Spine.timeScale) >= (double) guardianExecutioner.AttackSignpost)
          {
            guardianExecutioner.SimpleSpineFlash.FlashWhite(false);
            guardianExecutioner.DashParticles.Play();
            CameraManager.shakeCamera(0.4f, guardianExecutioner.state.facingAngle);
            guardianExecutioner.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            guardianExecutioner.speed = AttackSpeed * Time.deltaTime;
            if ((UnityEngine.Object) guardianExecutioner.damageColliderEvents != (UnityEngine.Object) null)
              guardianExecutioner.damageColliderEvents.SetActive(true);
            guardianExecutioner.LookAtTarget();
            if (guardianExecutioner.UseForceForAttack)
            {
              guardianExecutioner.DisableForces = true;
              Vector3 force = (Vector3) new Vector2(guardianExecutioner.AttackForce * Mathf.Cos(guardianExecutioner.state.facingAngle * ((float) Math.PI / 180f)), guardianExecutioner.AttackForce * Mathf.Sin(guardianExecutioner.state.facingAngle * ((float) Math.PI / 180f)));
              guardianExecutioner.rb.AddForce((Vector2) force);
            }
            if (!string.IsNullOrEmpty(guardianExecutioner.AttackVO))
            {
              AudioManager.Instance.PlayOneShot(guardianExecutioner.AttackVO);
              break;
            }
            break;
          }
          guardianExecutioner.SimpleSpineFlash.FlashWhite(guardianExecutioner.state.Timer / guardianExecutioner.AttackSignpost);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) AttackSpeed > 0.0)
            AttackSpeed -= guardianExecutioner.AttackDeceleration * GameManager.DeltaTime * guardianExecutioner.Spine.timeScale;
          guardianExecutioner.speed = AttackSpeed * Time.deltaTime * guardianExecutioner.Spine.timeScale;
          if ((double) guardianExecutioner.state.Timer >= 0.25 && (UnityEngine.Object) guardianExecutioner.damageColliderEvents != (UnityEngine.Object) null)
            guardianExecutioner.damageColliderEvents.SetActive(false);
          if ((double) (guardianExecutioner.state.Timer += Time.deltaTime * guardianExecutioner.Spine.timeScale) >= (double) guardianExecutioner.DurationOfAttack)
          {
            if (guardianExecutioner.UseForceForAttack)
            {
              guardianExecutioner.DisableForces = false;
              guardianExecutioner.rb.velocity = Vector2.zero;
            }
            guardianExecutioner.DashParticles.Stop();
            if (--NumAttacks > 0)
            {
              AttackSpeed = guardianExecutioner.SpeedOfAttack + (float) ((3 - NumAttacks) * 2);
              guardianExecutioner.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              guardianExecutioner.Spine.AnimationState.SetAnimation(0, "attack" + (4 - NumAttacks).ToString(), false);
              guardianExecutioner.Spine.AnimationState.AddAnimation(0, guardianExecutioner.idleAnim, true, 0.0f);
              if (!string.IsNullOrEmpty(guardianExecutioner.attackMeleeBSFX))
              {
                AudioManager.Instance.PlayOneShot(guardianExecutioner.attackMeleeBSFX, guardianExecutioner.gameObject);
                break;
              }
              break;
            }
            Loop = false;
            guardianExecutioner.state.CURRENT_STATE = StateMachine.State.Idle;
            guardianExecutioner.ReconsiderTarget();
            guardianExecutioner.Spine.AnimationState.SetAnimation(0, guardianExecutioner.idleAnim, true);
            break;
          }
          break;
      }
      yield return (object) null;
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * guardianExecutioner.Spine.timeScale) < 0.5)
      yield return (object) null;
    guardianExecutioner.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianExecutioner.Spine.AnimationState.SetAnimation(0, guardianExecutioner.idleAnim, true);
    if ((UnityEngine.Object) guardianExecutioner.TargetObject != (UnityEngine.Object) null && (double) Vector3.Distance(guardianExecutioner.TargetObject.transform.position, guardianExecutioner.transform.position) > 5.0)
    {
      guardianExecutioner.LookAtTarget();
      time = 0.0f;
      while ((double) (time += Time.deltaTime * guardianExecutioner.Spine.timeScale) < 1.0)
        yield return (object) null;
    }
    guardianExecutioner.StartCoroutine((IEnumerator) guardianExecutioner.FightPlayer());
  }

  public override IEnumerator ProjectileRingsRoutine()
  {
    EnemySimpleGuardianExecutioner guardianExecutioner = this;
    if (!string.IsNullOrEmpty(guardianExecutioner.WarningVO))
      AudioManager.Instance.PlayOneShot(guardianExecutioner.WarningVO, guardianExecutioner.transform.position);
    guardianExecutioner.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    guardianExecutioner.LookAtTarget();
    guardianExecutioner.Spine.AnimationState.SetAnimation(0, guardianExecutioner.shootAnim, false);
    guardianExecutioner.Spine.AnimationState.AddAnimation(0, guardianExecutioner.idleAnim, true, 0.0f);
    if (!string.IsNullOrEmpty(guardianExecutioner.attackRangedStartSFX))
      AudioManager.Instance.PlayOneShot(guardianExecutioner.attackRangedStartSFX, guardianExecutioner.transform.position);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * guardianExecutioner.Spine.timeScale) < 1.0)
      yield return (object) null;
    Projectile arrow = UnityEngine.Object.Instantiate<ProjectileCircle>(guardianExecutioner.projectilePatternRings, guardianExecutioner.transform.parent).GetComponent<Projectile>();
    arrow.transform.position = guardianExecutioner.transform.position;
    arrow.health = guardianExecutioner.health;
    arrow.team = guardianExecutioner.health.team;
    arrow.Speed = guardianExecutioner.projectilePatternRingsSpeed;
    arrow.Acceleration = guardianExecutioner.projectilePatternRingsAcceleration;
    arrow.Owner = guardianExecutioner.health;
    arrow.GetComponent<ProjectileCircle>().InitDelayed((UnityEngine.Object) guardianExecutioner.TargetObject != (UnityEngine.Object) null ? guardianExecutioner.TargetObject : PlayerFarming.Instance.gameObject, guardianExecutioner.projectilePatternRingsRadius, 0.0f, (System.Action) (() =>
    {
      CameraManager.instance.ShakeCameraForDuration(0.8f, 0.9f, 0.3f, false);
      if (!string.IsNullOrEmpty(this.AttackVO))
        AudioManager.Instance.PlayOneShot(this.AttackVO, this.transform.position);
      if ((UnityEngine.Object) this.guardianGameObject != (UnityEngine.Object) null)
      {
        if (!string.IsNullOrEmpty(this.AttackProjectileSFX))
          this.projectileInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackProjectileSFX, this.guardianGameObject.transform);
        arrow.Angle = Mathf.Round(arrow.Angle / 45f) * 45f;
      }
      else
        arrow.DestroyProjectile();
    }));
    time = 0.0f;
    while ((double) (time += Time.deltaTime * guardianExecutioner.Spine.timeScale) < 1.5666667222976685)
      yield return (object) null;
    guardianExecutioner.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianExecutioner.Spine.AnimationState.SetAnimation(0, guardianExecutioner.idleAnim, true);
    guardianExecutioner.ReconsiderTarget();
    guardianExecutioner.StartCoroutine((IEnumerator) guardianExecutioner.FightPlayer());
  }
}
