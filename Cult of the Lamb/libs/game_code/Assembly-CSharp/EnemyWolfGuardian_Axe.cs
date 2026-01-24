// Decompiled with JetBrains decompiler
// Type: EnemyWolfGuardian_Axe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyWolfGuardian_Axe : EnemyWolfGuardian
{
  public ParticleSystem AxeSpecialImpactParticles;
  public float AxeSpecialGroundSlamSpeed = 10f;
  public bool LightningFollowsPlayer = true;
  public Transform ImpactBone;
  public float chainLightningAttackDuration = 2f;
  public float timeBetweenLightningStrikes = 0.3f;
  public float spaceBetweenLightningStrikes = 0.6f;
  public float lightningStartDistance = 2f;
  public AssetReferenceGameObject LightningStrikePrefab;
  public AssetReferenceGameObject indicatorPrefab;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AxeAttackOneAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AxeAttackTwoAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AxeSpecialAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AxeGroundSlamAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AxeGroundSlamStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AxeGroundSlamMovingAnimation;
  [EventRef]
  public string LightningAttackSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/axe_attack_lightning_start";
  [EventRef]
  public string AttackMeleeGroundSlamSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/axe_attack_ground_slam";
  [EventRef]
  public string AttackMeleeJabSFX = "event:/dlc/dungeon05/enemy/miniboss_wolf_guardian/axe_attack_jab";
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public GameObject loadedLightningImpact;
  public GameObject loadedSlamIndicator;
  public GameObject currentIndicator;
  public float LightningRingExpansionSpeed = 5f;
  public float LightningEnemyDamage = 4f;
  public float LightningRingMaxRadius = 2f;
  public const float LIGHTNING_DAMAGE_PLAYER = 1f;

  public override void Awake()
  {
    base.Awake();
    this.LoadAssets();
  }

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.LightningStrikePrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyWolfGuardian_Axe.loadedAddressableAssets.Add(obj);
      this.loadedLightningImpact = obj.Result;
      this.loadedLightningImpact.CreatePool(10, true);
    });
    Addressables.LoadAssetAsync<GameObject>((object) this.indicatorPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyWolfGuardian_Axe.loadedAddressableAssets.Add(obj);
      this.loadedSlamIndicator = obj.Result;
      this.loadedSlamIndicator.SetActive(false);
    });
  }

  public override void SetupAnimArrays()
  {
    this.meleeAnims = new string[2]
    {
      this.AxeAttackOneAnimation,
      this.AxeAttackTwoAnimation
    };
    this.meleeSfxDictionary = new Dictionary<string, string>()
    {
      {
        this.AxeAttackOneAnimation,
        this.AttackMeleeGroundSlamSFX
      },
      {
        this.AxeAttackTwoAnimation,
        this.AttackMeleeJabSFX
      }
    };
  }

  public override void GoToMeleeAttackState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Axe.AxeMeleeAttackState());
  }

  public override void GoToRangedAttackState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Axe.AxeRangedMasterState());
  }

  public override void GoToSpecialAttackState()
  {
    this.logicStateMachine.SetState((SimpleState) new EnemyWolfGuardian_Axe.AxeSpecialAttackMasterState());
  }

  public void DoChainLightningAttack()
  {
    this.StartCoroutine((IEnumerator) this.LineLightningAttack());
  }

  public IEnumerator LineLightningAttack()
  {
    EnemyWolfGuardian_Axe enemyWolfGuardianAxe = this;
    enemyWolfGuardianAxe.ClearPaths();
    Vector3 originPosition = new Vector3(enemyWolfGuardianAxe.ImpactBone.position.x, enemyWolfGuardianAxe.ImpactBone.position.y, 0.0f);
    Vector3 dir = (enemyWolfGuardianAxe.TargetObject.transform.position - originPosition).normalized;
    int spawnCount = Mathf.RoundToInt(15f / enemyWolfGuardianAxe.spaceBetweenLightningStrikes);
    int currentIndex = 0;
    while (currentIndex < spawnCount)
    {
      if (enemyWolfGuardianAxe.LightningFollowsPlayer)
        dir = (enemyWolfGuardianAxe.TargetObject.transform.position - originPosition).normalized;
      Vector3 vector3 = originPosition;
      originPosition = vector3 + dir * enemyWolfGuardianAxe.spaceBetweenLightningStrikes;
      if (!BiomeGenerator.PointWithinIsland(vector3, out Vector3 _))
      {
        --spawnCount;
      }
      else
      {
        GameObject lightning = ObjectPool.Spawn(enemyWolfGuardianAxe.loadedLightningImpact, vector3, Quaternion.identity);
        lightning.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(enemyWolfGuardianAxe.health, vector3, (System.Action) (() => ObjectPool.Recycle(lightning)), true);
        ++currentIndex;
        yield return (object) new WaitForSeconds(enemyWolfGuardianAxe.timeBetweenLightningStrikes);
      }
    }
  }

  public void DoAxeImpactEffect()
  {
    if (!((UnityEngine.Object) this.ImpactBone != (UnityEngine.Object) null))
      return;
    BiomeConstants.Instance.EmitHammerEffects(this.ImpactBone.position, cameraShakeIntensityMax: 1.5f, cameraShakeDuration: 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
  }

  public override void DoAttackHitEvent()
  {
    this.EnableMeleeDamageCollider();
    CameraManager.shakeCamera(0.4f, this.state.facingAngle);
    this.DashParticles.Play();
  }

  public void EnableMeleeDamageCollider()
  {
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
      this.MeleeColliders.localPosition = new Vector3(1.5f * ((double) closestTarget.transform.position.x > (double) this.transform.position.x ? -1f : 1f), 0.0f, 0.0f);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(true);
    foreach (Collider2D collider in Physics2D.OverlapCircleAll((Vector2) this.damageColliderEvents.transform.position, ((CircleCollider2D) this.meleeCollider).radius))
      this.OnDamageTriggerEnter(collider);
  }

  public void DisableMeleeDamageCollider()
  {
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__28_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemyWolfGuardian_Axe.loadedAddressableAssets.Add(obj);
    this.loadedLightningImpact = obj.Result;
    this.loadedLightningImpact.CreatePool(10, true);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__28_1(AsyncOperationHandle<GameObject> obj)
  {
    EnemyWolfGuardian_Axe.loadedAddressableAssets.Add(obj);
    this.loadedSlamIndicator = obj.Result;
    this.loadedSlamIndicator.SetActive(false);
  }

  public class AxeRangedMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Axe parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public int childStateIndex;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Ranged;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Axe) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
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
        (SimpleState) new EnemyWolfGuardian_Axe.ChainLightningState(),
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

  public class ChainLightningState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Axe parent;
    public float attackDuration;
    public float progress;
    public float delayBeforeHammerFX = 1.7f;
    public bool hasPlayedVFX;
    public float damageColliderAliveTime = 0.3f;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Ranged;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Axe) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.AxeSpecialAttackAnimation, false);
      this.parent.LookAtTarget();
      this.attackDuration = this.parent.chainLightningAttackDuration;
      if (string.IsNullOrEmpty(this.parent.LightningAttackSFX))
        return;
      AudioManager.Instance.PlayOneShot(this.parent.LightningAttackSFX, this.parent.transform.position);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      double progress = (double) this.progress;
      double delayBeforeHammerFx = (double) this.delayBeforeHammerFX;
      if (!this.hasPlayedVFX && (double) this.progress >= (double) this.delayBeforeHammerFX)
      {
        this.hasPlayedVFX = true;
        this.parent.DoAxeImpactEffect();
        this.parent.EnableMeleeDamageCollider();
      }
      if ((double) this.progress < (double) this.delayBeforeHammerFX + (double) this.damageColliderAliveTime)
        return;
      this.parent.DisableMeleeDamageCollider();
    }

    public override void OnExit() => this.parent.state.CURRENT_STATE = StateMachine.State.Idle;

    public override bool IsComplete() => (double) this.progress >= (double) this.attackDuration;
  }

  public class AxeSpecialAttackMasterState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian parent;
    public EnemyWolfGuardian.WolfGuardianStateMachine childStateMachine;
    public SimpleState[] stateSequence;
    public float flyUpProgress;
    public float flyDownProgress;
    public int stateIndex;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.childStateMachine = new EnemyWolfGuardian.WolfGuardianStateMachine();
      this.childStateMachine.SetParent(this.parent);
      this.SetupStateSequence();
      this.childStateMachine.SetState(this.stateSequence[this.stateIndex]);
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
    }

    public override void Update()
    {
      if (this.IsCurrentStateComplete())
        this.ProgressToNextSubState();
      this.childStateMachine.Update();
    }

    public override void OnExit()
    {
    }

    public void SetupStateSequence()
    {
      this.stateSequence = new SimpleState[4]
      {
        (SimpleState) new EnemyWolfGuardian.FlyUpState(),
        (SimpleState) new EnemyWolfGuardian_Axe.GroundSlamTravelState(),
        (SimpleState) new EnemyWolfGuardian_Axe.GroundSlamImpactState(),
        (SimpleState) new EnemyWolfGuardian.RecoveryState(this.parent.SpecialAttackRecoveryTime)
      };
    }

    public bool IsCurrentStateComplete() => this.childStateMachine.GetCurrentState().IsComplete();

    public void ProgressToNextSubState()
    {
      ++this.stateIndex;
      if (this.stateIndex < this.stateSequence.Length)
      {
        this.childStateMachine.SetState(this.stateSequence[this.stateIndex]);
      }
      else
      {
        this.parent.ResetAttackCooldown();
        this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
      }
    }
  }

  public class GroundSlamTravelState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Axe parent;
    public Vector3 startPos;
    public Vector3 targetPos;
    public float progress;
    public float distance;
    public float flightDuration;
    public bool isComplete;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Axe) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.state.CURRENT_STATE = StateMachine.State.Attacking;
      this.parent.SetTargetObject();
      this.parent.LookAtTarget();
      this.startPos = this.parent.transform.position;
      this.targetPos = this.parent.TargetObject.transform.position;
      this.distance = Vector3.Distance(this.startPos, this.targetPos);
      this.flightDuration = this.distance / this.parent.AxeSpecialGroundSlamSpeed;
      this.parent.LockToGround = false;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.AxeGroundSlamStartAnimation, false);
      this.parent.Spine.AnimationState.AddAnimation(0, this.parent.AxeGroundSlamMovingAnimation, true, 0.0f);
      if (!string.IsNullOrEmpty(this.parent.AirSlamSFX))
        AudioManager.Instance.PlayOneShot(this.parent.AirSlamSFX, this.parent.transform.position);
      if (!string.IsNullOrEmpty(this.parent.WarningVO))
        AudioManager.Instance.PlayOneShot(this.parent.WarningVO, this.parent.gameObject);
      this.parent.currentIndicator = ObjectPool.Spawn(this.parent.loadedSlamIndicator, this.targetPos, Quaternion.identity);
      this.parent.currentIndicator.SetActive(true);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      this.parent.transform.position = this.parent.CalculateFlyProgress(this.progress, this.flightDuration, this.startPos, this.targetPos, this.parent.EaseInBackCurve);
      double progress = (double) this.progress;
      double flightDuration = (double) this.flightDuration;
      if ((double) this.progress < (double) this.flightDuration)
        return;
      this.parent.transform.position = this.targetPos;
      this.parent.health.invincible = false;
      this.parent.LockToGround = true;
      this.isComplete = true;
    }

    public override void OnExit() => this.parent.DashParticles.Stop();

    public override bool IsComplete() => this.isComplete;
  }

  public class GroundSlamImpactState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Axe parent;
    public float progress;
    public float animationDuration;
    public bool isComplete;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Special;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Axe) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.AxeGroundSlamAnimation, false);
      this.animationDuration = this.parent.Spine.skeleton.Data.FindAnimation(this.parent.AxeGroundSlamAnimation).Duration;
      this.parent.AxeSpecialImpactParticles.Play();
      BiomeConstants.Instance.EmitHammerEffects(this.parent.transform.position, cameraShakeIntensityMax: 1.5f, cameraShakeDuration: 0.5f);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      ObjectPool.Recycle(this.parent.currentIndicator);
      if (!((UnityEngine.Object) this.parent.warriorTrioManager != (UnityEngine.Object) null))
        return;
      LightningRingExplosion.CreateExplosion(this.parent.transform.position, this.parent.health.team, this.parent.health, this.parent.LightningRingExpansionSpeed, 1f, this.parent.LightningEnemyDamage, maxRadiusTarget: this.parent.LightningRingMaxRadius);
    }

    public override void Update()
    {
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if ((double) this.progress < (double) this.animationDuration)
        return;
      this.isComplete = true;
    }

    public override void OnExit()
    {
      this.parent.DoAttackEndEvent();
      this.parent.damageColliderEvents.SetActive(false);
      this.parent.SetStatusImmunity(false);
    }

    public override bool IsComplete() => this.isComplete;
  }

  public class AxeMeleeAttackState : EnemyWolfGuardian.WarriorTrioSimpleState
  {
    public EnemyWolfGuardian_Axe parent;
    public bool hasDoneAttackOneFX;
    public float currentAnimTime;
    public float progress;
    public float currentAnimEventTimeStamp;
    public int attackCount = 2;
    public int currentAttackIndex = -1;
    public float attackOneFXDelay = 1.1f;

    public override EnemyWolfGuardian.StateType StateType => EnemyWolfGuardian.StateType.Melee;

    public override void OnEnter()
    {
      this.parent = (EnemyWolfGuardian_Axe) ((EnemyWolfGuardian.WolfGuardianStateMachine) this.parentStateMachine).Parent;
      this.attackCount = this.parent.meleeAnims.Length;
      this.parent.LookAtTarget();
    }

    public override void Update()
    {
      if ((double) this.progress >= (double) this.currentAnimTime)
      {
        ++this.currentAttackIndex;
        if (this.currentAttackIndex >= this.attackCount)
        {
          this.parentStateMachine.SetState((SimpleState) new EnemyWolfGuardian.IdleState());
        }
        else
        {
          this.parent.LookAtTarget();
          Spine.Animation currentMeleeAnim = this.GetCurrentMeleeAnim(this.currentAttackIndex);
          string currentMeleeSfx = this.GetCurrentMeleeSFX(this.currentAttackIndex);
          this.currentAnimTime = currentMeleeAnim.Duration;
          this.currentAnimEventTimeStamp = this.GetFirstEventTimeStamp(currentMeleeAnim);
          this.parent.Spine.AnimationState.SetAnimation(0, currentMeleeAnim, false);
          this.parent.DisableForces = true;
          this.parent.rb.AddForce((Vector2) (Vector3) new Vector2(this.parent.AttackForce * Mathf.Cos(this.parent.state.LookAngle * ((float) Math.PI / 180f)), this.parent.AttackForce * Mathf.Sin(this.parent.state.LookAngle * ((float) Math.PI / 180f))));
          this.progress = 0.0f;
          if (!string.IsNullOrEmpty(currentMeleeSfx))
            this.parent.meleeEventInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(currentMeleeSfx, this.parent.transform);
        }
      }
      this.progress += Time.deltaTime * this.parent.Spine.timeScale;
      if (this.hasDoneAttackOneFX || this.currentAttackIndex != 0 || (double) this.progress < (double) this.attackOneFXDelay)
        return;
      this.hasDoneAttackOneFX = true;
      this.parent.DoAxeImpactEffect();
      if (!((UnityEngine.Object) this.parent.warriorTrioManager != (UnityEngine.Object) null))
        return;
      LightningRingExplosion.CreateExplosion(new Vector3(this.parent.ImpactBone.position.x, this.parent.ImpactBone.position.y, 0.0f), this.parent.health.team, this.parent.health, this.parent.LightningRingExpansionSpeed, 1f, this.parent.LightningEnemyDamage, maxRadiusTarget: this.parent.LightningRingMaxRadius);
    }

    public override void OnExit()
    {
      this.parent.Spine.AnimationState.SetAnimation(0, this.parent.IdleAnimation, true);
      this.parent.damageColliderEvents.SetActive(false);
      this.parent.DisableForces = false;
      this.parent.rb.velocity = Vector2.zero;
    }

    public Spine.Animation GetCurrentMeleeAnim(int index)
    {
      return this.parent.Spine.skeleton.Data.FindAnimation(this.parent.meleeAnims[index]);
    }

    public string GetCurrentMeleeSFX(int index)
    {
      return this.parent.meleeSfxDictionary[this.parent.meleeAnims[index]];
    }

    public float GetFirstEventTimeStamp(Spine.Animation animation)
    {
      float firstEventTimeStamp = 0.0f;
      foreach (Timeline timeline in animation.Timelines)
      {
        if (timeline is EventTimeline eventTimeline)
          firstEventTimeStamp = eventTimeline.Events[0].Time;
      }
      return firstEventTimeStamp;
    }
  }
}
