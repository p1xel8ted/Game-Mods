// Decompiled with JetBrains decompiler
// Type: EnemyWolfGuardian_Sword
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyWolfGuardian_Sword : EnemyWolfGuardian
{
  public float TimeBetweenSlashes = 1f;
  public int SwordRangedProjectileCount = 3;
  public float LightningExpansionSpeed = 5f;
  public float LightningEnemyDamage = 4f;
  public ProjectilePattern projectilePattern;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SwordAttackOneAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SwordAttackThreeAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SwordRangedAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SwordLightningAttackAnimation;
  [EventRef]
  public string AttackProjectileSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/sword_attack_projectile_start";
  [EventRef]
  public string AttackMeleeSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/sword_attack_multistrike_start";
  public EventInstance projectileEventInstanceSFX;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;

  public override void SetupAnimArrays()
  {
    this.meleeAnims = new string[2]
    {
      this.SwordAttackOneAnimation,
      this.SwordAttackThreeAnimation
    };
    this.meleeSfxDictionary = new Dictionary<string, string>()
    {
      {
        this.SwordAttackOneAnimation,
        this.AttackMeleeSFX
      },
      {
        this.SwordAttackThreeAnimation,
        this.AttackMeleeSFX
      }
    };
  }

  public override void CleanupEventInstances()
  {
    base.CleanupEventInstances();
    AudioManager.Instance.StopOneShotInstanceEarly(this.projectileEventInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public override void GoToRangedAttackState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Sword.SwordRangedAttackMasterState());
  }

  public override void GoToSpecialAttackState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Sword.SwordSpecialAttackMasterState());
  }

  public void DoLightningExplosion()
  {
    LightningRingExplosion.CreateExplosion(this.transform.position, this.health.team, this.health, this.LightningExpansionSpeed, 1f, this.LightningEnemyDamage);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    this.health.invincible = false;
    if (!((Object) this.damageColliderEvents != (Object) null))
      return;
    this.damageColliderEvents.SetActive(true);
    this.damageColliderEvents.StartTimedDisable(0.2f);
  }

  public override void LookAtTarget(bool UpdateMeleeColliderPosition = true)
  {
    Health closestTarget = this.GetClosestTarget();
    if ((Object) closestTarget == (Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, closestTarget.transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    if (!UpdateMeleeColliderPosition)
      return;
    this.MeleeColliders.localPosition = new Vector3(1.5f * ((double) closestTarget.transform.position.x > (double) this.transform.position.x ? -1f : 1f), 0.0f, 0.0f);
  }

  public class SwordRangedAttackMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Sword parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public int childStateIndex;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Ranged;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Sword) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.childStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
      this.childStateMachine.SetParent((EnemyWolfGuardian) this.parent);
      this.SetupStateSequence();
      this.childStateMachine.SetState(this.stateSequence[this.childStateIndex]);
    }

    public override void Update()
    {
      this.childStateMachine.Update();
      if (!this.IsCurrentStateComplete())
        return;
      this.ProgressToNextChildState();
    }

    public override void OnExit()
    {
    }

    public void SetupStateSequence()
    {
      this.stateSequence = new SimpleState[3]
      {
        (SimpleState) new EnemyWolfGuardian.JumpAwayState(),
        (SimpleState) new EnemyWolfGuardian_Sword.GroundedRangedAttackState(),
        (SimpleState) new EnemyWolfGuardian.RecoveryState(this.parent.RangedAttackRecoveryTime)
      };
    }

    public bool IsCurrentStateComplete() => this.childStateMachine.GetCurrentState().IsComplete();

    public void ProgressToNextChildState()
    {
      ++this.childStateIndex;
      if (this.childStateIndex < this.stateSequence.Length)
      {
        this.childStateMachine.SetState(this.stateSequence[this.childStateIndex]);
      }
      else
      {
        this.parent.ResetAttackCooldown();
        this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
      }
    }
  }

  public class GroundedRangedAttackState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Sword parent;
    public Vector3 targetLocation;
    public float currentAttackProgress;
    public float attackDuration;
    public float projectileDelay = 0.9f;
    public int attackIndex;
    public int attackCount;
    public bool hasFired;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Ranged;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Sword) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
      this.attackDuration = this.parent.GetAnimationDuration(this.parent.SwordRangedAttackAnimation) + this.parent.TimeBetweenSlashes;
      this.attackCount = this.parent.SwordRangedProjectileCount;
      this.parent.SetTargetObject();
      this.parent.LookAtTarget(true);
    }

    public override void Update()
    {
      if (this.attackIndex >= this.attackCount)
        return;
      if ((double) this.currentAttackProgress <= 0.0)
      {
        this.hasFired = false;
        if ((Object) this.parent.TargetObject == (Object) null)
          this.parent.SetTargetObject();
        this.parent.LookAtTarget(true);
        if ((Object) this.parent.TargetObject != (Object) null)
          this.targetLocation = this.parent.TargetObject.transform.position;
        this.parent.Spine.AnimationState.SetAnimation(0, this.parent.SwordRangedAttackAnimation, false);
        this.parent.Spine.AnimationState.AddAnimation(0, this.parent.IdleAnimation, true, 0.0f);
        if (!string.IsNullOrEmpty(this.parent.AttackProjectileSFX))
          this.parent.projectileEventInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.parent.AttackProjectileSFX, this.parent.transform);
      }
      else if (!this.hasFired && (double) this.currentAttackProgress >= (double) this.projectileDelay)
      {
        this.hasFired = true;
        this.parent.StartCoroutine((IEnumerator) this.parent.projectilePattern.ShootIE(this.targetLocation));
        foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(0.0f);
      }
      if (!this.hasFired)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(Mathf.Clamp01(this.currentAttackProgress / this.projectileDelay));
      }
      this.currentAttackProgress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.currentAttackProgress < (double) this.attackDuration)
        return;
      ++this.attackIndex;
      this.currentAttackProgress = 0.0f;
    }

    public override void OnExit()
    {
      this.parent.state.CURRENT_STATE = StateMachine.State.Idle;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
    }

    public override bool IsComplete() => this.attackIndex >= this.attackCount;
  }

  public class SwordSpecialAttackMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public int childStateIndex;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.childStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
      this.childStateMachine.SetParent(this.parent);
      this.SetupStateSequence();
      this.childStateMachine.SetState(this.stateSequence[this.childStateIndex]);
    }

    public override void Update()
    {
      this.childStateMachine.Update();
      if (!this.IsCurrentStateComplete())
        return;
      this.ProgressToNextSubState();
    }

    public override void OnExit()
    {
    }

    public void SetupStateSequence()
    {
      this.stateSequence = new SimpleState[3]
      {
        (SimpleState) new EnemyWolfGuardian_Sword.SwordFlyUpState(),
        (SimpleState) new EnemyWolfGuardian_Sword.SwordLightningRingState(),
        (SimpleState) new EnemyWolfGuardian.RecoveryState(this.parent.SpecialAttackRecoveryTime)
      };
    }

    public bool IsCurrentStateComplete() => this.childStateMachine.GetCurrentState().IsComplete();

    public void ProgressToNextSubState()
    {
      ++this.childStateIndex;
      if (this.childStateIndex < this.stateSequence.Length)
      {
        this.childStateMachine.SetState(this.stateSequence[this.childStateIndex]);
      }
      else
      {
        this.parent.ResetAttackCooldown();
        this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
      }
    }
  }

  public class SwordFlyUpState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Sword parent;
    public float progress;
    public float stateDuration;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Sword) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.ClearPaths();
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.FlyingAnticipationAnimation, false);
      this.parent.LockToGround = false;
      this.stateDuration = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.FlyingAnticipationAnimation).Duration;
      this.parent.SetStatusImmunity(true);
      if (string.IsNullOrEmpty(this.parent.FlyUpSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.FlyUpSFX, this.parent.transform.position);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(this.progress / this.stateDuration);
    }

    public override void OnExit()
    {
      foreach (SimpleSpineFlash simpleSpineFlash in this.parent.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
    }

    public override bool IsComplete() => (double) this.progress >= (double) this.stateDuration;
  }

  public class SwordLightningRingState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Sword parent;
    public Vector3 startPos;
    public Vector3 targetPos;
    public float progress;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Sword) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.health.invincible = true;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.SwordLightningAttackAnimation, false);
      if (string.IsNullOrEmpty(this.parent.AirSlamSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.AirSlamSFX, this.parent.transform.position);
    }

    public override void Update() => this.progress += Time.deltaTime * this.parent.Spine.timeScale;

    public override void OnExit()
    {
      this.parent.SetStatusImmunity(false);
      this.parent.LockToGround = true;
      this.parent.health.invincible = false;
    }

    public override bool IsComplete()
    {
      return (double) this.progress >= (double) this.parent.SwordLightningTime;
    }
  }
}
