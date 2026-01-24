// Decompiled with JetBrains decompiler
// Type: EnemyFleshSwarmer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyFleshSwarmer : UnitObject
{
  public EnemyFleshSwarmer.StartingStates StartingState;
  public ColliderEvents damageColliderEvents;
  public IEnumerator damageColliderRoutine;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public SpriteRenderer ShadowSpriteRenderer;
  public ParticleSystem BloodDrip;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FallAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [EventRef]
  public string OnHitSFX = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/gethit";
  [EventRef]
  public string OnFallSFX = "event:/dlc/dungeon06/enemy/flesh_swarmer_small/fall";
  [EventRef]
  public string MoveSFX = "event:/dlc/dungeon06/enemy/flesh_swarmer_small/move";
  [EventRef]
  public string ThrowSFX = "event:/dlc/dungeon06/enemy/flesh_swarmer_large/attack_throw";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/flesh_swarmer_small/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/warning";
  [EventRef]
  public string HidingVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/hiding";
  public bool _playedVO;
  public bool RevealAllOtherFleshSwarmersOnReveal = true;
  public bool breedingFleshSwarmer;
  public float breedingSpawnWhenPercentLost = 10f;
  public float breedingLastPercentSpawned = 100f;
  public float breederFinalScaleSize = 0.25f;
  public GameObject enemyToBreedOnHit;
  public float TargetAngle;
  public bool canBeParried = true;
  public float signPostParryWindow = 0.2f;
  public float attackParryWindow = 0.3f;
  public GameManager gm;
  public CircleCollider2D physicsCollider;
  public float moveTimer;
  public Vector2 MoveFrequencyRange = new Vector2(0.5f, 1.5f);
  public float MoveForce = 1250f;
  public float DeviationAngleMax = 10f;
  public float idleTimestamp;
  public float idleDur = 0.6f;
  public float signPostDur = 0.2f;
  public float KnockBackMultipier = 0.7f;
  public bool canBeStunned = true;
  public bool stunnedStopsAttack = true;
  public float stunnedTimestamp;
  public float stunnedDur = 0.5f;
  public float attackRange;
  public float revealRange = 3f;
  public bool isFleeing;
  public bool hasCollidedWithObstacle;
  public float IdleTimeVariance = 0.5f;
  public float AngleDrift = 20f;
  public bool alwaysTargetPlayer;
  public static List<EnemyFleshSwarmer> FleshHoppers = new List<EnemyFleshSwarmer>();
  public bool isRevealed;
  public bool isInProcessOfReveal;
  public float GravitySpeed = 1f;
  public float HidingHeight = 5f;
  public Vector3 shadowScale;
  public float hiddenSoundTimer;
  public Vector2 hiddenSoundInterval = new Vector2(4f, 8f);
  public bool couldBeCritter = true;
  public float eggAnimTimer = -1f;
  public float eggAnimBufferTime = 2f;
  public GameObject projectilePrefab;
  public const float minBombRange = 2.5f;
  public const float maxBombRange = 5f;
  public float bombSpeed = 6f;
  public float bombHealthPercent = 50f;
  public float bombChancePercent = 50f;
  public SkeletonAnimation skeletonAnimation;
  public Health spawnerHealth;
  public float idleTimer;
  public Coroutine AutoRevealCoroutine;

  public void Start()
  {
    this.physicsCollider = this.GetComponent<CircleCollider2D>();
    this.ResetMoveTimer();
    this.ResetHiddenSoundTimer();
    this.isInProcessOfReveal = false;
    this.couldBeCritter = this.health.CanBeTurnedIntoCritter;
    this.isRevealed = this.StartingState != 0;
    if (this.StartingState != EnemyFleshSwarmer.StartingStates.Hidden)
      return;
    this.health.invincible = true;
    this.health.CanBeTurnedIntoCritter = false;
    this.health.IsHidden = true;
    this.Spine.transform.localPosition = Vector3.back * this.HidingHeight;
    this.ShadowSpriteRenderer.enabled = false;
    this.Spine.gameObject.GetComponent<MeshRenderer>().enabled = false;
    if (!(bool) (UnityEngine.Object) this.BloodDrip)
      return;
    this.BloodDrip.Play();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    this.health.OnDie += new Health.DieAction(((UnitObject) this).OnDie);
    this.state.OnStateChange += new StateMachine.StateChange(this.OnStateChange);
    this.gm = GameManager.GetInstance();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if ((UnityEngine.Object) this.ShadowSpriteRenderer != (UnityEngine.Object) null)
      this.shadowScale = this.ShadowSpriteRenderer.transform.localScale;
    Debug.Log((object) ("Shadow scale is " + this.shadowScale.ToString()));
    if ((UnityEngine.Object) this.gm != (UnityEngine.Object) null)
      this.idleTimestamp = this.gm.CurrentTime;
    this.idleTimestamp += EnemyFleshSwarmer.FleshHoppers != null ? (float) EnemyFleshSwarmer.FleshHoppers.Count * 0.1f : UnityEngine.Random.Range(0.0f, this.idleDur);
    EnemyFleshSwarmer.FleshHoppers.Add(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    if (!this.health.IsHidden || !(bool) (UnityEngine.Object) this.BloodDrip || this.BloodDrip.isPlaying)
      return;
    this.BloodDrip.Play();
  }

  public void SetSpawner(Health spawnerHealthTarget)
  {
    if ((bool) (UnityEngine.Object) this.spawnerHealth)
      return;
    this.spawnerHealth = spawnerHealthTarget;
    this.spawnerHealth.OnDie += new Health.DieAction(this.spawnerDied);
  }

  public void spawnerDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((double) this.health.HP <= 0.0)
      return;
    this.health.invincible = false;
    this.health.DealDamage(1000f, this.gameObject, this.transform.position);
  }

  public override void OnDisable()
  {
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    this.health.OnDie -= new Health.DieAction(((UnitObject) this).OnDie);
    this.state.OnStateChange -= new StateMachine.StateChange(this.OnStateChange);
    if ((bool) (UnityEngine.Object) this.spawnerHealth)
      this.spawnerHealth.OnDie -= new Health.DieAction(this.spawnerDied);
    this.ClearPaths();
    EnemyFleshSwarmer.FleshHoppers.Remove(this);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void Update()
  {
    base.Update();
    if ((double) this.eggAnimTimer > 0.0)
      this.eggAnimTimer -= Time.deltaTime * this.Spine.timeScale;
    if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
    {
      this.gm = GameManager.GetInstance();
      if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
        return;
    }
    if (this.isRevealed)
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          this.UpdateStateIdle();
          break;
        case StateMachine.State.Moving:
          this.UpdateStateMoving();
          break;
        case StateMachine.State.Vulnerable:
          this.UpdateStateVulnerable();
          break;
      }
    }
    else if (!this.isInProcessOfReveal && this.TargetIsVisible() && this.TargetIsInRevealRange())
    {
      if (this.RevealAllOtherFleshSwarmersOnReveal)
        this.RevealAll();
      else if (this.AutoRevealCoroutine == null)
        this.AutoRevealCoroutine = this.StartCoroutine((IEnumerator) this.Reveal(0.5f));
    }
    if (!this.isRevealed && !this.isInProcessOfReveal)
    {
      this.hiddenSoundTimer -= Time.deltaTime * this.Spine.timeScale;
      if ((double) this.hiddenSoundTimer <= 0.0 && !string.IsNullOrEmpty(this.HidingVO))
      {
        if (!AudioManager.Instance.CurrentEventIsPlaying(this.HidingVO))
          AudioManager.Instance.PlayOneShot(this.HidingVO, this.transform.position);
        this.ResetHiddenSoundTimer();
      }
    }
    if (!((UnityEngine.Object) this.spawnerHealth != (UnityEngine.Object) null) || (double) this.spawnerHealth.HP > 0.0)
      return;
    this.spawnerDied((GameObject) null, (Vector3) Vector2.zero, (Health) null, Health.AttackTypes.Melee, Health.AttackFlags.ForceKill);
  }

  public void ResetHiddenSoundTimer()
  {
    this.hiddenSoundTimer = UnityEngine.Random.Range(this.hiddenSoundInterval.x, this.hiddenSoundInterval.y);
  }

  public void RevealAll(bool instantReveal = false, bool startInCombat = false)
  {
    float num = -0.2f;
    foreach (EnemyFleshSwarmer fleshHopper in EnemyFleshSwarmer.FleshHoppers)
    {
      if (fleshHopper.StartingState == EnemyFleshSwarmer.StartingStates.Hidden)
      {
        fleshHopper.StopAllCoroutines();
        this.DisableForces = false;
        if (fleshHopper.AutoRevealCoroutine == null)
          fleshHopper.AutoRevealCoroutine = fleshHopper.StartCoroutine((IEnumerator) fleshHopper.Reveal(0.2f + (num += 0.2f), !instantReveal, instantReveal, startInCombat));
      }
    }
  }

  public IEnumerator Reveal(
    float delay,
    bool drop = true,
    bool instantReveal = false,
    bool startInCombat = false,
    bool eggSpawn = false)
  {
    EnemyFleshSwarmer enemyFleshSwarmer = this;
    enemyFleshSwarmer.isInProcessOfReveal = true;
    if ((bool) (UnityEngine.Object) enemyFleshSwarmer.BloodDrip)
      enemyFleshSwarmer.BloodDrip.Stop();
    if (instantReveal)
    {
      enemyFleshSwarmer.Spine.gameObject.GetComponent<MeshRenderer>().enabled = true;
      enemyFleshSwarmer.ShadowSpriteRenderer.enabled = true;
      enemyFleshSwarmer.Spine.transform.localPosition = Vector3.zero;
      enemyFleshSwarmer.ShadowSpriteRenderer.transform.localScale = new Vector3(enemyFleshSwarmer.shadowScale.x, enemyFleshSwarmer.shadowScale.y, enemyFleshSwarmer.shadowScale.z);
      enemyFleshSwarmer.health.invincible = false;
      enemyFleshSwarmer.health.IsHidden = false;
      enemyFleshSwarmer.health.CanBeTurnedIntoCritter = enemyFleshSwarmer.couldBeCritter;
      if (eggSpawn)
      {
        enemyFleshSwarmer.eggAnimTimer = enemyFleshSwarmer.eggAnimBufferTime;
        enemyFleshSwarmer.Spine.AnimationState.SetAnimation(0, "egg-spawn", false);
        enemyFleshSwarmer.Spine.AnimationState.AddAnimation(0, enemyFleshSwarmer.IdleAnimation, true, 0.0f);
      }
      else if ((double) enemyFleshSwarmer.eggAnimTimer <= 0.0)
        enemyFleshSwarmer.Spine.AnimationState.SetAnimation(0, startInCombat ? enemyFleshSwarmer.AttackAnimation : enemyFleshSwarmer.IdleAnimation, true);
      enemyFleshSwarmer.ResetMoveTimer();
      enemyFleshSwarmer.state.CURRENT_STATE = startInCombat ? StateMachine.State.Moving : StateMachine.State.Idle;
      enemyFleshSwarmer.isRevealed = true;
    }
    else
    {
      if (drop)
        yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyFleshSwarmer.Spine);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyFleshSwarmer.Spine.timeScale) < (double) delay)
        yield return (object) null;
      enemyFleshSwarmer.Spine.gameObject.GetComponent<MeshRenderer>().enabled = true;
      enemyFleshSwarmer.ShadowSpriteRenderer.transform.localScale = Vector3.one;
      if (!string.IsNullOrEmpty(enemyFleshSwarmer.WarningVO))
        AudioManager.Instance.PlayOneShot(enemyFleshSwarmer.WarningVO, enemyFleshSwarmer.gameObject);
      if (drop)
      {
        if ((double) enemyFleshSwarmer.eggAnimTimer <= 0.0)
          enemyFleshSwarmer.Spine.AnimationState.SetAnimation(0, enemyFleshSwarmer.FallAnimation, true);
        AudioManager.Instance.PlayOneShot(enemyFleshSwarmer.OnFallSFX, enemyFleshSwarmer.Spine.transform.gameObject);
        enemyFleshSwarmer.ShadowSpriteRenderer.enabled = true;
        enemyFleshSwarmer.health.CanBeTurnedIntoCritter = true;
        float grav = 0.0f;
        while ((double) enemyFleshSwarmer.Spine.transform.localPosition.z + (double) grav < 0.0)
        {
          grav += Time.fixedDeltaTime * enemyFleshSwarmer.GravitySpeed * enemyFleshSwarmer.Spine.timeScale;
          enemyFleshSwarmer.Spine.transform.localPosition += Vector3.forward * grav;
          enemyFleshSwarmer.ShadowSpriteRenderer.transform.localScale = new Vector3()
          {
            x = enemyFleshSwarmer.shadowScale.x * ((-enemyFleshSwarmer.Spine.transform.localPosition.z - enemyFleshSwarmer.HidingHeight) / enemyFleshSwarmer.HidingHeight),
            y = enemyFleshSwarmer.shadowScale.y * ((-enemyFleshSwarmer.Spine.transform.localPosition.z - enemyFleshSwarmer.HidingHeight) / enemyFleshSwarmer.HidingHeight),
            z = enemyFleshSwarmer.shadowScale.z * ((-enemyFleshSwarmer.Spine.transform.localPosition.z - enemyFleshSwarmer.HidingHeight) / enemyFleshSwarmer.HidingHeight)
          };
          yield return (object) new WaitForFixedUpdate();
        }
      }
      enemyFleshSwarmer.Spine.transform.localPosition = Vector3.zero;
      enemyFleshSwarmer.health.invincible = false;
      enemyFleshSwarmer.health.IsHidden = false;
      if (drop)
      {
        if (enemyFleshSwarmer.breedingFleshSwarmer)
        {
          CameraManager.shakeCamera(4f);
          MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
        }
        if ((double) enemyFleshSwarmer.eggAnimTimer <= 0.0)
          enemyFleshSwarmer.Spine.AnimationState.SetAnimation(0, enemyFleshSwarmer.LandAnimation, false);
        while ((double) (time += Time.deltaTime * enemyFleshSwarmer.Spine.timeScale) < 0.5)
          yield return (object) null;
      }
      else if (!string.IsNullOrEmpty(enemyFleshSwarmer.MoveSFX))
        AudioManager.Instance.PlayOneShot(enemyFleshSwarmer.MoveSFX, enemyFleshSwarmer.gameObject);
      if ((double) enemyFleshSwarmer.eggAnimTimer <= 0.0)
        enemyFleshSwarmer.Spine.AnimationState.SetAnimation(0, enemyFleshSwarmer.IdleAnimation, true);
      if (startInCombat)
        enemyFleshSwarmer.state.CURRENT_STATE = StateMachine.State.Moving;
      enemyFleshSwarmer.isRevealed = true;
    }
  }

  public void DoThrowProjectile()
  {
    Health closestTarget = this.GetClosestTarget();
    if (!((UnityEngine.Object) closestTarget != (UnityEngine.Object) null))
      return;
    Vector3 position = closestTarget.transform.position;
    this.LookAtClosestTarget();
    MortarBomb component = UnityEngine.Object.Instantiate<GameObject>(this.projectilePrefab, closestTarget.transform.position, Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.GameObject.transform).GetComponent<MortarBomb>();
    if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) position) < 2.5)
      component.transform.position = this.transform.position + (position - this.transform.position).normalized * 2.5f;
    else
      component.transform.position = this.transform.position + (position - this.transform.position).normalized * 5f;
    float bombSpeed = this.bombSpeed;
    float moveDuration = Vector2.Distance((Vector2) this.transform.position, (Vector2) component.transform.position) / bombSpeed;
    component.Play(this.transform.position + new Vector3(0.0f, 0.0f, -1.5f), moveDuration, Health.Team.Team2, PlayDefaultSFX: false, parentSpine: this.Spine);
    this.SimpleSpineFlash.FlashWhite(false);
    if (string.IsNullOrEmpty(this.ThrowSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.ThrowSFX, this.gameObject);
  }

  public virtual void UpdateStateIdle()
  {
    this.speed = 0.0f;
    this.idleTimer += Time.deltaTime * this.Spine.timeScale;
    if ((double) this.idleTimer >= (double) this.idleDur - (double) this.signPostParryWindow)
      this.canBeParried = true;
    if ((double) this.idleTimer < (double) this.idleDur + (double) UnityEngine.Random.Range(-this.IdleTimeVariance, this.IdleTimeVariance))
      return;
    bool flag = this.TargetIsVisible();
    this.TargetAngle = !this.alwaysTargetPlayer ? (!this.isFleeing ? (!flag ? this.GetRandomFacingAngle() : this.GetAngleToTarget()) : this.GetFleeAngle()) : (!((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null) ? this.GetFleeAngle() : this.GetAngleToTarget());
    this.LookAtTargetAngle();
    this.state.CURRENT_STATE = StateMachine.State.Moving;
  }

  public void ResetMoveTimer()
  {
    this.moveTimer = UnityEngine.Random.Range(this.MoveFrequencyRange.x, this.MoveFrequencyRange.y);
  }

  public virtual void UpdateStateMoving()
  {
    if ((double) (this.moveTimer -= Time.deltaTime * this.Spine.timeScale) > 0.0)
      return;
    this.ResetMoveTimer();
    if ((double) this.eggAnimTimer <= 0.0)
    {
      this.Spine.AnimationState.SetAnimation(0, this.AttackAnimation, false);
      this.Spine.AnimationState.AddAnimation(0, this.IdleAnimation, true, 0.0f);
    }
    bool flag = false;
    if ((double) this.health.HP < (double) this.health.totalHP * ((double) this.bombHealthPercent / 100.0) && (double) UnityEngine.Random.Range(0.0f, 100f) <= (double) this.bombChancePercent)
      flag = true;
    if (flag)
    {
      DOVirtual.DelayedCall(0.25f, (TweenCallback) (() => this.DoThrowProjectile()));
    }
    else
    {
      this.DisableForces = true;
      Health closestTarget = this.GetClosestTarget();
      if (!((UnityEngine.Object) closestTarget != (UnityEngine.Object) null))
        return;
      this.LookAtClosestTarget();
      Vector2 normalized = (Vector2) (closestTarget.transform.position - this.transform.position).normalized;
      int num1 = 100;
      while (--num1 > 0)
      {
        double num2 = (double) UnityEngine.Random.Range(-this.DeviationAngleMax, this.DeviationAngleMax);
        float num3 = Mathf.Cos((float) (num2 * (Math.PI / 180.0)));
        float num4 = Mathf.Sin((float) (num2 * (Math.PI / 180.0)));
        Vector2 direction = new Vector2((float) ((double) normalized.x * (double) num3 - (double) normalized.y * (double) num4), (float) ((double) normalized.x * (double) num4 + (double) normalized.y * (double) num3));
        Vector2 customSeparation = this.GetCustomSeparation(0.5f);
        direction += customSeparation;
        Vector2 force = direction * this.MoveForce;
        float distance = force.magnitude * 0.5f;
        if ((UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, direction, distance, LayerMask.GetMask("Obstacles Player Ignore")).collider == (UnityEngine.Object) null)
        {
          this.rb.AddForce(force);
          this.StartCoroutine((IEnumerator) this.TurnOnDamageColliderForDuration(0.3f));
          if (string.IsNullOrEmpty(this.MoveSFX))
            break;
          AudioManager.Instance.PlayOneShot(this.MoveSFX, this.transform.position);
          break;
        }
      }
    }
  }

  public Vector2 GetCustomSeparation(float seperationRadius, bool ignorePlayer = true)
  {
    float x = 0.0f;
    float y = 0.0f;
    foreach (UnitObject seperater in UnitObject.Seperaters)
    {
      if ((!ignorePlayer || seperater.health.team != Health.Team.PlayerTeam) && (!((UnityEngine.Object) seperater != (UnityEngine.Object) null) || !((UnityEngine.Object) seperater != (UnityEngine.Object) this) || this.health.team != Health.Team.PlayerTeam || seperater.SeparateObjectFromPlayer) && (UnityEngine.Object) seperater != (UnityEngine.Object) this && (UnityEngine.Object) seperater != (UnityEngine.Object) null && this.SeperateObject && seperater.SeperateObject && this.state.CURRENT_STATE != StateMachine.State.Dodging && this.state.CURRENT_STATE != StateMachine.State.Defending)
      {
        float num = Vector2.Distance((Vector2) seperater.gameObject.transform.position, (Vector2) this.transform.position);
        float angle = Utils.GetAngle(seperater.gameObject.transform.position, this.transform.position);
        if ((double) num < (double) seperationRadius)
        {
          x += (float) (((double) seperationRadius - (double) num) / 2.0) * Mathf.Cos(angle * ((float) Math.PI / 180f)) * GameManager.FixedDeltaTime;
          y += (float) (((double) seperationRadius - (double) num) / 2.0) * Mathf.Sin(angle * ((float) Math.PI / 180f)) * GameManager.FixedDeltaTime;
        }
      }
    }
    return new Vector2(x, y);
  }

  public void LookAtClosestTarget()
  {
    this.TargetAngle = this.GetAngleToTarget();
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
    if ((double) this.Spine.timeScale == 9.9999997473787516E-05)
      return;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
  }

  public void LookAtTargetAngle()
  {
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
    if ((double) this.Spine.timeScale == 9.9999997473787516E-05)
      return;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive((double) this.Spine.timeScale > 1.0 / 1000.0);
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.damageColliderEvents.SetActive(false);
  }

  public virtual void UpdateStateVulnerable()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.stunnedTimestamp) < (double) this.stunnedDur * (double) this.Spine.timeScale)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public bool TargetIsVisible()
  {
    if (!GameManager.RoomActive || (UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return false;
    float a = Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.GetClosestTarget().transform.position, this.transform.position));
    return (double) a <= (double) this.VisionRange && this.CheckLineOfSightOnTarget(this.GetClosestTarget().gameObject, this.GetClosestTarget().transform.position, Mathf.Min(a, (float) this.VisionRange));
  }

  public bool TargetIsInRevealRange()
  {
    return GameManager.RoomActive && !((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null) && (double) Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.GetClosestTarget().transform.position, this.transform.position)) <= (double) this.revealRange * 0.75;
  }

  public float GetAngleToTarget()
  {
    float angleToTarget = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position) + UnityEngine.Random.Range(-this.AngleDrift, this.AngleDrift);
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return angleToTarget;
    float num = 32f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(angleToTarget * ((float) Math.PI / 180f)), Mathf.Sin(angleToTarget * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      angleToTarget += (float) (360.0 / ((double) num + 1.0));
    return angleToTarget;
  }

  public float GetRandomFacingAngle()
  {
    float randomFacingAngle = (float) UnityEngine.Random.Range(0, 360);
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return randomFacingAngle;
    float num = 16f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(randomFacingAngle * ((float) Math.PI / 180f)), Mathf.Sin(randomFacingAngle * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      randomFacingAngle += (float) (360.0 / ((double) num + 1.0));
    return randomFacingAngle;
  }

  public float GetFleeAngle()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return this.GetRandomFacingAngle();
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(4, 7);
      Vector3 toPosition = this.GetClosestTarget().transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      Vector3 direction = Vector3.Normalize(toPosition - this.GetClosestTarget().transform.position);
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.GetClosestTarget().transform.position, 1.5f, (Vector2) direction, distance, (int) this.layerToCheck);
      if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
        return Utils.GetAngle(this.transform.position, toPosition);
      if ((double) this.MagnitudeFindDistanceBetween(this.GetClosestTarget().transform.position, (Vector3) raycastHit2D.centroid) > 9.0)
        return Utils.GetAngle(this.transform.position, toPosition);
    }
    return this.GetRandomFacingAngle();
  }

  public virtual void OnStateChange(StateMachine.State newState, StateMachine.State prevState)
  {
    if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
      return;
    switch (newState)
    {
      case StateMachine.State.Idle:
        if (newState == prevState)
          break;
        this.idleTimestamp = this.gm.CurrentTime;
        if (this.Spine.AnimationState == null || (double) this.eggAnimTimer > 0.0)
          break;
        if (prevState == StateMachine.State.Moving)
          this.Spine.AnimationState.SetAnimation(0, "jump-end", false);
        this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        break;
      case StateMachine.State.Vulnerable:
        this.stunnedTimestamp = this.gm.CurrentTime;
        if (!this.stunnedStopsAttack || !((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
          break;
        this.damageColliderEvents.SetActive(false);
        break;
    }
  }

  public new void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.canBeParried)
      return;
    this.health.WasJustParried = true;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.health.HasShield)
      return;
    if ((double) this.KnockBackMultipier != 0.0)
      this.DoKnockBack(Attacker, this.KnockBackMultipier, 1f);
    if (!string.IsNullOrEmpty(this.OnHitSFX))
      AudioManager.Instance.PlayOneShot(this.OnHitSFX, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if (this.breedingFleshSwarmer && (UnityEngine.Object) this.enemyToBreedOnHit != (UnityEngine.Object) null)
    {
      float currentHealthPercent = (float) ((double) this.health.HP / (double) this.health.totalHP * 100.0);
      this.UpdateMonsterScale(currentHealthPercent);
      this.HandleSpawnEnemies(AttackLocation, currentHealthPercent);
    }
    this.UsePathing = true;
    this.health.invincible = false;
    if (this.canBeStunned && (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Attacking))
    {
      this.state.CURRENT_STATE = StateMachine.State.Vulnerable;
      if (this.Spine.AnimationState != null && (double) this.eggAnimTimer <= 0.0)
      {
        this.Spine.AnimationState.SetAnimation(0, "hit", false);
        this.Spine.AnimationState.AddAnimation(0, "idle", false, 0.0f);
      }
    }
    this.SimpleSpineFlash.FlashFillRed();
  }

  public void InstantSpawnOnBomb(float blastHeight = 1f)
  {
    if (this.AutoRevealCoroutine != null)
    {
      Debug.Log((object) "Stopping previous reveal coroutine");
      this.StopCoroutine(this.AutoRevealCoroutine);
    }
    this.AutoRevealCoroutine = this.StartCoroutine((IEnumerator) this.Reveal(0.0f, false, true, true, true));
    this.RevealAllOtherFleshSwarmersOnReveal = false;
    if (this.TryGetComponent<Rigidbody2D>(out Rigidbody2D _) && (UnityEngine.Object) this.SimpleSpineFlash != (UnityEngine.Object) null)
    {
      float flashPower = 1f;
      this.SimpleSpineFlash.FlashRed(flashPower);
      this.SimpleSpineFlash.FlashRed(flashPower * 0.25f);
      CameraManager.shakeCamera(4f);
      DOTween.To((DOGetter<float>) (() => flashPower), (DOSetter<float>) (value =>
      {
        flashPower = value;
        this.SimpleSpineFlash.FlashRed(flashPower * 0.25f);
      }), 0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
    }
    float duration = 0.75f;
    this.Spine.transform.DOLocalMoveZ(blastHeight, duration / 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Spine.transform.DOLocalMoveZ(0.0f, duration / 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad)));
  }

  public void SpawnEnemyOnHit(Vector3 attackLocation)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.enemyToBreedOnHit, this.transform.position, Quaternion.identity, BiomeGenerator.Instance.CurrentRoom.GameObject.transform);
    gameObject.SetActive(true);
    Vector3 position = this.transform.position;
    gameObject.transform.position = attackLocation;
    Vector3 vector3 = attackLocation - position;
    vector3 = vector3.normalized + this.transform.up * 0.5f;
    Vector3 normalized = vector3.normalized;
    Health component1 = gameObject.GetComponent<Health>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      Interaction_Chest.Instance?.AddEnemy(component1);
    EnemyFleshSwarmer enemyFleshSpawner;
    if (!gameObject.TryGetComponent<EnemyFleshSwarmer>(out enemyFleshSpawner))
      return;
    if (enemyFleshSpawner.AutoRevealCoroutine != null)
    {
      Debug.Log((object) "Stopping previous reveal coroutine");
      enemyFleshSpawner.StopCoroutine(enemyFleshSpawner.AutoRevealCoroutine);
    }
    enemyFleshSpawner.AutoRevealCoroutine = enemyFleshSpawner.StartCoroutine((IEnumerator) enemyFleshSpawner.Reveal(0.0f, false, true, true, true));
    enemyFleshSpawner.RevealAllOtherFleshSwarmersOnReveal = false;
    Rigidbody2D component2;
    if (!enemyFleshSpawner.TryGetComponent<Rigidbody2D>(out component2))
      return;
    SimpleSpineFlash newEnemySimpleSpineFlash = enemyFleshSpawner.SimpleSpineFlash;
    if ((UnityEngine.Object) newEnemySimpleSpineFlash != (UnityEngine.Object) null)
    {
      float flashPower = 1f;
      this.SimpleSpineFlash.FlashRed(flashPower);
      newEnemySimpleSpineFlash.FlashRed(flashPower * 0.25f);
      CameraManager.shakeCamera(4f);
      DOTween.To((DOGetter<float>) (() => flashPower), (DOSetter<float>) (value =>
      {
        flashPower = value;
        newEnemySimpleSpineFlash.FlashRed(flashPower * 0.25f);
        this.SimpleSpineFlash.FlashRed(flashPower);
      }), 0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.Linear);
    }
    float num = 10f;
    component2.AddForce((Vector2) (normalized * num), ForceMode2D.Impulse);
    float endValue = -3f;
    float duration = 0.75f;
    enemyFleshSpawner.Spine.transform.DOLocalMoveZ(endValue, duration / 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => enemyFleshSpawner.Spine.transform.DOLocalMoveZ(0.0f, duration / 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad)));
  }

  public void HandleSpawnEnemies(Vector3 attackLocation, float currentHealthPercent)
  {
    int num = Mathf.FloorToInt((this.breedingLastPercentSpawned - currentHealthPercent) / this.breedingSpawnWhenPercentLost);
    Vector3 vector3 = Vector3.Lerp(this.transform.position, attackLocation, 0.5f);
    for (int index = 0; index < num; ++index)
      this.SpawnEnemyOnHit(vector3 + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), UnityEngine.Random.Range(-0.2f, 0.2f), 0.0f));
    if (num > 0)
      this.breedingLastPercentSpawned = currentHealthPercent;
    Debug.Log((object) $"/////////Last sapwned percent {this.breedingLastPercentSpawned.ToString()}, enemiesToSpawn {num.ToString()}");
  }

  public void UpdateMonsterScale(float currentHealthPercent)
  {
    float num1 = Mathf.Lerp(this.breederFinalScaleSize, 1f, currentHealthPercent / 100f) - 0.1f;
    int num2 = Mathf.Clamp(5 - Mathf.FloorToInt(num1 * 6f), 0, 4);
    this.Spine.Skeleton.SetSkin("Big_" + num2.ToString());
    Debug.Log((object) $"Frame for scale factor {num1.ToString()} should be frame {num2.ToString()}");
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Moving || collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    Debug.Log((object) "I have hopped into an obstacle", (UnityEngine.Object) this.gameObject);
    this.hasCollidedWithObstacle = true;
    this.TargetAngle += (double) this.TargetAngle > 180.0 ? -180f : 180f;
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  public override Health GetClosestTarget(
    Vector3 offset,
    bool ignoreBreakables = false,
    bool ignoreProjectiles = true,
    bool ignoreDownedPlayers = true,
    bool ignoreNonUnits = false)
  {
    if ((double) Time.time == (double) this.checkFrame && (UnityEngine.Object) this.cachedTarget != (UnityEngine.Object) null)
      return this.cachedTarget;
    if (this.isRevealed)
      return base.GetClosestTarget(offset, ignoreBreakables, ignoreProjectiles, ignoreDownedPlayers, ignoreNonUnits);
    this.closestPlayerFarming = PlayerFarming.FindClosestPlayer(this.pointToCheck, !ignoreDownedPlayers);
    if (((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || BiomeGenerator.Instance.CurrentRoom == null || (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom == (UnityEngine.Object) null) && this.health.team != Health.Team.PlayerTeam)
      return !((UnityEngine.Object) this.closestPlayerFarming != (UnityEngine.Object) null) || this.closestPlayerFarming.GoToAndStopping ? (Health) null : (Health) this.closestPlayerFarming.health;
    Health.Team team = this.health.team == Health.Team.PlayerTeam ? Health.Team.Team2 : Health.Team.PlayerTeam;
    List<Health> healthList1 = new List<Health>((IEnumerable<Health>) Health.team2);
    List<Health> healthList2 = new List<Health>();
    if (team == Health.Team.PlayerTeam)
    {
      if ((bool) (UnityEngine.Object) this.closestPlayerFarming && Health.playerTeam.Count <= 1 && !this.closestPlayerFarming.IsKnockedOut)
        return this.closestPlayerFarming.GoToAndStopping ? (Health) null : (Health) this.closestPlayerFarming.health;
      healthList1.Clear();
      for (int index = 0; index < Health.playerTeam.Count; ++index)
      {
        if ((UnityEngine.Object) Health.playerTeam[index] != (UnityEngine.Object) null && (!ignoreProjectiles || !Projectile.Contains(Health.playerTeam[index])) && (!ignoreProjectiles || !ProjectileGhost.Contains(Health.playerTeam[index])))
          healthList1.Add(Health.playerTeam[index]);
      }
    }
    foreach (Health health in healthList1)
    {
      if (!((UnityEngine.Object) health == (UnityEngine.Object) null) && health.enabled && !health.InanimateObject && (double) health.HP > 0.0 && (!ignoreBreakables || health.team != Health.Team.Team2 || !health.CompareTag("BreakableDecoration")) && (!ignoreProjectiles || !Projectile.Contains(health)) && (!ignoreProjectiles || !ProjectileGhost.Contains(health)))
      {
        if (ignoreDownedPlayers && health.team == Health.Team.PlayerTeam)
        {
          PlayerFarming component = health.GetComponent<PlayerFarming>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.IsKnockedOut)
            continue;
        }
        if ((!ignoreNonUnits || !((UnityEngine.Object) health.GetComponent<UnitObject>() == (UnityEngine.Object) null)) && (bool) (UnityEngine.Object) health && (health.team == team || health.IsCharmedEnemy && (UnityEngine.Object) health != (UnityEngine.Object) this.health))
          healthList2.Add(health);
      }
    }
    if (healthList2.Count == 0 && team == Health.Team.PlayerTeam && (bool) (UnityEngine.Object) this.closestPlayerFarming && (!ignoreDownedPlayers || !this.closestPlayerFarming.IsKnockedOut))
      return (Health) this.closestPlayerFarming.health;
    Health closestTarget = (Health) null;
    foreach (Health health in healthList2)
    {
      if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null || (double) Vector3.Distance(health.transform.position, this.transform.position + offset) < (double) Vector3.Distance(closestTarget.transform.position, this.transform.position + offset))
        closestTarget = health;
    }
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null && (UnityEngine.Object) this.closestPlayerFarming != (UnityEngine.Object) null && (!ignoreDownedPlayers || !this.closestPlayerFarming.IsKnockedOut) && team == Health.Team.PlayerTeam)
      return (Health) this.closestPlayerFarming.health;
    this.checkFrame = Time.time;
    this.cachedTarget = closestTarget;
    return closestTarget;
  }

  [CompilerGenerated]
  public void \u003CUpdateStateMoving\u003Eb__84_0() => this.DoThrowProjectile();

  public enum StartingStates
  {
    Hidden,
    Wandering,
  }
}
