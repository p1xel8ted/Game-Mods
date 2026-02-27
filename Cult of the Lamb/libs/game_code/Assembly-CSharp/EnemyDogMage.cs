// Decompiled with JetBrains decompiler
// Type: EnemyDogMage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyDogMage : UnitObject
{
  public static List<EnemyDogMage> DogMages = new List<EnemyDogMage>();
  public EnemyDogMage.StartingStates StartHidden;
  public bool DetectPlayerWhileHidden = true;
  public bool HiddenOffsetIsGlobalPosition;
  public Vector3 HiddenOffset = Vector3.zero;
  public float HiddenRadius = 5f;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string UnawareAnimation;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string MovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  public bool LoopSignPostAttackAnimation = true;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FallAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostSlamAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SlamAnimation;
  public SpriteRenderer ShadowSpriteRenderer;
  public SimpleSpineFlash[] SimpleSpineFlashes;
  public float KnockbackModifier = 1f;
  public int NumberOfAttacks = 1;
  public float AttackForceModifier = 1f;
  public bool CounterAttack;
  public bool SlamAttack;
  public bool CanBeInterrupted = true;
  public bool AttackTowardsPlayer;
  public float DamageColliderDuration = -1f;
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer;
  public int DistanceToPathTowardsPlayer = 6;
  public SkeletonAnimation warningIcon;
  public float AttackDelayTime = 2f;
  public float LineAttackDelayTime = 4f;
  public float CircleAttackDelayTime = 3f;
  public bool Attacking;
  public bool IsStunned;
  public float AttackDuration = 1f;
  public float SignPostAttackDuration = 0.5f;
  public float CircleAttackRange;
  public GameObject SlamRockPrefab;
  [Space]
  public float AttackTimer;
  public float LineAttackTimer;
  public float CircleAttackTimer;
  [SerializeField]
  public GameObject avalanchePrefab;
  [SerializeField]
  public int numAvalanchesLine;
  [SerializeField]
  public float delayAttackLine;
  [SerializeField]
  public float timeBetweenAvalanchesLine;
  [SerializeField]
  public float delayAvalancheLine;
  [SerializeField]
  public float speedAvalancheLine;
  public bool LineChasesPlayer = true;
  [SerializeField]
  public float distanceActivationAttackCircle;
  [SerializeField]
  public int numAvalanchesCircle;
  [SerializeField]
  public float distanceAvalanchesCircle;
  [SerializeField]
  public float delayAttackCircle;
  [SerializeField]
  public float timeBetweenAvalanchesCircle;
  [SerializeField]
  public float delayAvalancheCircle;
  [SerializeField]
  public float speedAvalancheCircle;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_small/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_small/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_small/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_small/warning";
  [EventRef]
  public string LineAttackSFX = "event:/dlc/dungeon05/enemy/dog_mage/attack_basic_start";
  [EventRef]
  public string RingAttackSFX = "event:/dlc/dungeon05/enemy/dog_mage/attack_ring_start";
  public LightningStrikeAttack lightningStrikeAttack;
  public IndicatorFlash indicatorPrefab;
  public float RandomDirection;
  public bool ShownWarning;
  public float GravitySpeed = 1f;
  public float HidingHeight = 5f;
  public bool DisableKnockback;
  public float Angle;
  public Vector3 Force;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public Vector2 IdleWaitRange = new Vector2(1f, 3f);
  public float IdleWait;
  public bool PathingToPlayer;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Awake()
  {
    base.Awake();
    this.SimpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyDogMage.DogMages.Add(this);
    this.CircleAttackTimer = this.CircleAttackDelayTime;
    this.RandomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if (GameManager.RoomActive)
    {
      this.IdleWait = 0.0f;
      this.Attacking = false;
      this.IsStunned = false;
      this.health.invincible = false;
      foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      this.StartCoroutine(this.ActiveRoutine());
    }
    else
    {
      switch (this.StartHidden)
      {
        case EnemyDogMage.StartingStates.Hidden:
          this.StartCoroutine(this.Hidden());
          break;
        case EnemyDogMage.StartingStates.Wandering:
          this.StartCoroutine(this.ActiveRoutine());
          break;
        case EnemyDogMage.StartingStates.Animation:
          this.StartCoroutine(this.AnimationRoutine());
          break;
      }
    }
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyDogMage.DogMages.Remove(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.SetActive(false);
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
  }

  public void ShowWarningIcon()
  {
    if ((UnityEngine.Object) this.warningIcon == (UnityEngine.Object) null || this.ShownWarning)
      return;
    this.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
    this.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, 2f);
    this.ShownWarning = true;
    if (string.IsNullOrEmpty(this.WarningVO))
      return;
    AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
  }

  public IEnumerator AnimationRoutine()
  {
    EnemyDogMage enemyDogMage = this;
    yield return (object) new WaitForEndOfFrame();
    enemyDogMage.health.invincible = true;
    Debug.Log((object) ("Spine " + enemyDogMage.Spine?.ToString()));
    Debug.Log((object) ("Spine.AnimationState " + enemyDogMage.Spine.AnimationState?.ToString()));
    Debug.Log((object) ("UnawareAnimation " + enemyDogMage.UnawareAnimation));
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.UnawareAnimation, true);
    while (GameManager.RoomActive)
      yield return (object) null;
    while ((UnityEngine.Object) enemyDogMage.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance((enemyDogMage.HiddenOffsetIsGlobalPosition ? Vector3.zero : enemyDogMage.transform.position) + enemyDogMage.HiddenOffset, enemyDogMage.GetClosestTarget().transform.position) > (double) enemyDogMage.HiddenRadius)
      yield return (object) null;
    if ((UnityEngine.Object) enemyDogMage.GetClosestTarget() != (UnityEngine.Object) null)
      enemyDogMage.state.LookAngle = Utils.GetAngle(enemyDogMage.transform.position, enemyDogMage.GetClosestTarget().transform.position);
    enemyDogMage.ShowWarningIcon();
    enemyDogMage.health.invincible = false;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.IdleAnimation, true);
    enemyDogMage.LineAttackTimer = 0.0f;
    enemyDogMage.StartCoroutine(enemyDogMage.ActiveRoutine());
  }

  public IEnumerator Hidden()
  {
    EnemyDogMage enemyDogMage = this;
    enemyDogMage.health.invincible = true;
    enemyDogMage.Spine.transform.localPosition = Vector3.back * enemyDogMage.HidingHeight;
    enemyDogMage.ShadowSpriteRenderer.enabled = false;
    enemyDogMage.Spine.gameObject.GetComponent<MeshRenderer>().enabled = false;
    while (GameManager.RoomActive)
      yield return (object) null;
    while (enemyDogMage.DetectPlayerWhileHidden && ((UnityEngine.Object) enemyDogMage.GetClosestTarget() == (UnityEngine.Object) null || (double) Vector3.Distance((enemyDogMage.HiddenOffsetIsGlobalPosition ? Vector3.zero : enemyDogMage.transform.position) + enemyDogMage.HiddenOffset, enemyDogMage.GetClosestTarget().transform.position) > (double) enemyDogMage.HiddenRadius))
      yield return (object) null;
    enemyDogMage.RevealAll();
  }

  public void RevealAll()
  {
    float num = -0.2f;
    foreach (EnemyDogMage dogMage in EnemyDogMage.DogMages)
    {
      if (dogMage.StartHidden == EnemyDogMage.StartingStates.Hidden)
      {
        dogMage.StopAllCoroutines();
        this.DisableForces = false;
        dogMage.StartCoroutine(dogMage.Reveal(num += 0.2f));
      }
    }
  }

  public IEnumerator Reveal(float Delay)
  {
    EnemyDogMage enemyDogMage = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < (double) Delay)
      yield return (object) null;
    enemyDogMage.Spine.gameObject.GetComponent<MeshRenderer>().enabled = true;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.FallAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/fall_from_sky", enemyDogMage.Spine.transform.gameObject);
    enemyDogMage.ShadowSpriteRenderer.enabled = true;
    float Grav = 0.0f;
    while ((double) enemyDogMage.Spine.transform.localPosition.z + (double) Grav < 0.0)
    {
      Grav += Time.fixedDeltaTime * enemyDogMage.GravitySpeed;
      enemyDogMage.Spine.transform.localPosition = enemyDogMage.Spine.transform.localPosition + Vector3.forward * Grav;
      enemyDogMage.ShadowSpriteRenderer.transform.localScale = Vector3.one * ((-enemyDogMage.Spine.transform.localPosition.z - enemyDogMage.HidingHeight) / enemyDogMage.HidingHeight);
      yield return (object) new WaitForFixedUpdate();
    }
    if ((UnityEngine.Object) enemyDogMage.orderIndicator != (UnityEngine.Object) null)
    {
      enemyDogMage.orderIndicator.gameObject.SetActive(true);
      enemyDogMage.orderIndicator.transform.parent.gameObject.SetActive(true);
    }
    enemyDogMage.Spine.transform.localPosition = Vector3.zero;
    enemyDogMage.ShadowSpriteRenderer.transform.localScale = Vector3.one;
    enemyDogMage.health.invincible = false;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.LandAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/enemy/land_normal", enemyDogMage.Spine.transform.gameObject);
    while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.IdleAnimation, true);
    enemyDogMage.LineAttackTimer = 0.0f;
    enemyDogMage.StartCoroutine(enemyDogMage.ActiveRoutine());
  }

  public virtual IEnumerator ActiveRoutine()
  {
    EnemyDogMage enemyDogMage = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while ((double) enemyDogMage.Spine.timeScale != 9.9999997473787516E-05)
      {
        if (enemyDogMage.state.CURRENT_STATE == StateMachine.State.Idle && (double) (enemyDogMage.IdleWait -= Time.deltaTime) <= 0.0)
          enemyDogMage.GetNewTargetPosition();
        if ((UnityEngine.Object) enemyDogMage.GetClosestTarget() != (UnityEngine.Object) null && !enemyDogMage.Attacking && !enemyDogMage.IsStunned && GameManager.RoomActive)
          enemyDogMage.state.LookAngle = Utils.GetAngle(enemyDogMage.transform.position, enemyDogMage.GetClosestTarget().transform.position);
        else
          enemyDogMage.state.LookAngle = enemyDogMage.state.facingAngle;
        if (enemyDogMage.MovingAnimation != "")
        {
          if (enemyDogMage.state.CURRENT_STATE == StateMachine.State.Moving && enemyDogMage.Spine.AnimationName != enemyDogMage.MovingAnimation)
            enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.MovingAnimation, true);
          if (enemyDogMage.state.CURRENT_STATE == StateMachine.State.Idle && enemyDogMage.Spine.AnimationName != enemyDogMage.IdleAnimation)
            enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.IdleAnimation, true);
        }
        if ((UnityEngine.Object) enemyDogMage.GetClosestTarget() == (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) enemyDogMage.GetClosestTarget() != (UnityEngine.Object) null)
            enemyDogMage.ShowWarningIcon();
        }
        else
        {
          enemyDogMage.AttackTimer -= Time.deltaTime;
          enemyDogMage.LineAttackTimer -= Time.deltaTime;
          enemyDogMage.CircleAttackTimer -= Time.deltaTime;
          if (enemyDogMage.ShouldCircleAttack())
            yield return (object) enemyDogMage.StartCoroutine(enemyDogMage.CircleAvalanchesAttack());
          if (enemyDogMage.ShouldLineAttack())
            yield return (object) enemyDogMage.StartCoroutine(enemyDogMage.LineLightningAttack());
        }
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public virtual bool ShouldCircleAttack()
  {
    return !((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null) && !this.Attacking && (double) this.CircleAttackTimer <= 0.0 && (double) this.AttackTimer <= 0.0 && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.distanceActivationAttackCircle && GameManager.RoomActive;
  }

  public virtual bool ShouldLineAttack()
  {
    return !((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null) && !this.Attacking && (double) this.LineAttackTimer <= 0.0 && (double) this.AttackTimer <= 0.0 && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) > (double) this.distanceActivationAttackCircle && GameManager.RoomActive;
  }

  public IEnumerator SlamRoutine()
  {
    EnemyDogMage enemyDogMage = this;
    enemyDogMage.Attacking = true;
    enemyDogMage.ClearPaths();
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.SignPostSlamAnimation, false);
    enemyDogMage.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in enemyDogMage.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in enemyDogMage.SimpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.6f, 0.5f);
    enemyDogMage.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.SlamAnimation, false);
    float time = 0.0f;
    float SlamDistance = 1.5f;
    float Rocks = 10f;
    int j = -1;
    while (++j < 3)
    {
      int num = -1;
      float f = 0.0f;
      while ((double) ++num <= (double) Rocks)
      {
        f += (float) (360.0 / (double) Rocks * (Math.PI / 180.0));
        UnityEngine.Object.Instantiate<GameObject>(enemyDogMage.SlamRockPrefab, enemyDogMage.transform.position + new Vector3(SlamDistance * Mathf.Cos(f), SlamDistance * Mathf.Sin(f)), Quaternion.identity, enemyDogMage.transform.parent).GetComponent<ForestScuttlerSlamBarricade>().Play(0.0f);
      }
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      ++SlamDistance;
      Rocks += 2f;
    }
    while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.IdleAnimation, true);
    enemyDogMage.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDogMage.IdleWait = 0.0f;
    enemyDogMage.CircleAttackTimer = enemyDogMage.CircleAttackDelayTime;
    enemyDogMage.AttackTimer = enemyDogMage.AttackDelayTime;
    enemyDogMage.Attacking = false;
  }

  public virtual IEnumerator AttackRoutine()
  {
    EnemyDogMage enemyDogMage = this;
    enemyDogMage.Attacking = true;
    enemyDogMage.ClearPaths();
    int CurrentAttack = 0;
    float time = 0.0f;
    while (++CurrentAttack <= enemyDogMage.NumberOfAttacks)
    {
      enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.SignPostAttackAnimation, enemyDogMage.LoopSignPostAttackAnimation);
      enemyDogMage.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", enemyDogMage.transform.position);
      if ((UnityEngine.Object) enemyDogMage.GetClosestTarget() != (UnityEngine.Object) null)
      {
        enemyDogMage.state.LookAngle = Utils.GetAngle(enemyDogMage.transform.position, enemyDogMage.GetClosestTarget().transform.position);
        enemyDogMage.state.facingAngle = enemyDogMage.state.LookAngle;
      }
      float Progress = 0.0f;
      float Duration = enemyDogMage.SignPostAttackDuration;
      while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemyDogMage.Spine.timeScale)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in enemyDogMage.SimpleSpineFlashes)
          simpleSpineFlash.FlashWhite(Progress / Duration);
        yield return (object) null;
      }
      foreach (SimpleSpineFlash simpleSpineFlash in enemyDogMage.SimpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      if (enemyDogMage.AttackTowardsPlayer)
      {
        if ((UnityEngine.Object) enemyDogMage.GetClosestTarget() != (UnityEngine.Object) null)
        {
          enemyDogMage.state.LookAngle = Utils.GetAngle(enemyDogMage.transform.position, enemyDogMage.GetClosestTarget().transform.position);
          enemyDogMage.state.facingAngle = enemyDogMage.state.LookAngle;
        }
        enemyDogMage.DoKnockBack(enemyDogMage.GetClosestTarget().gameObject, -1f, 1f);
      }
      else
      {
        enemyDogMage.DisableForces = true;
        enemyDogMage.Force = (Vector3) (new Vector2(2500f * Mathf.Cos(enemyDogMage.state.LookAngle * ((float) Math.PI / 180f)), 2500f * Mathf.Sin(enemyDogMage.state.LookAngle * ((float) Math.PI / 180f))) * enemyDogMage.AttackForceModifier);
        enemyDogMage.rb.AddForce((Vector2) enemyDogMage.Force);
      }
      enemyDogMage.damageColliderEvents.SetActive(true);
      if (!string.IsNullOrEmpty(enemyDogMage.AttackVO))
        AudioManager.Instance.PlayOneShot(enemyDogMage.AttackVO, enemyDogMage.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", enemyDogMage.transform.position);
      enemyDogMage.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
      enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.AttackAnimation, false);
      enemyDogMage.Spine.AnimationState.AddAnimation(0, enemyDogMage.IdleAnimation, true, 0.0f);
      if ((double) enemyDogMage.DamageColliderDuration != -1.0)
        enemyDogMage.StartCoroutine(enemyDogMage.EnableCollider(enemyDogMage.DamageColliderDuration));
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < (double) enemyDogMage.AttackDuration * 0.699999988079071)
        yield return (object) null;
      enemyDogMage.damageColliderEvents.SetActive(false);
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < (double) enemyDogMage.AttackDuration * 0.30000001192092896)
      yield return (object) null;
    enemyDogMage.DisableForces = false;
    enemyDogMage.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDogMage.IdleWait = 0.0f;
    enemyDogMage.LineAttackTimer = enemyDogMage.LineAttackDelayTime;
    enemyDogMage.AttackTimer = enemyDogMage.AttackDelayTime;
    enemyDogMage.Attacking = false;
  }

  public IEnumerator EnableCollider(float dur)
  {
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) dur)
      yield return (object) null;
    this.damageColliderEvents.SetActive(false);
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
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if (!this.DisableKnockback)
      this.damageColliderEvents.SetActive(false);
    if (this.CanBeInterrupted && AttackType != Health.AttackTypes.NoReaction)
    {
      this.StopAllCoroutines();
      this.DisableForces = false;
      this.StartCoroutine(this.HurtRoutine());
    }
    if (AttackType != Health.AttackTypes.NoKnockBack && AttackType != Health.AttackTypes.NoReaction && !this.DisableKnockback && this.CanBeInterrupted)
      this.StartCoroutine(this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyDogMage enemyDogMage = this;
    enemyDogMage.DisableForces = true;
    enemyDogMage.Angle = Utils.GetAngle(Attacker.transform.position, enemyDogMage.transform.position) * ((float) Math.PI / 180f);
    enemyDogMage.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyDogMage.Angle), 1500f * Mathf.Sin(enemyDogMage.Angle)) * enemyDogMage.KnockbackModifier);
    enemyDogMage.rb.AddForce((Vector2) enemyDogMage.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyDogMage.DisableForces = false;
  }

  public IEnumerator ApplyForceRoutine(Vector3 forcePosition)
  {
    EnemyDogMage enemyDogMage = this;
    enemyDogMage.DisableForces = true;
    enemyDogMage.Angle = Utils.GetAngle(forcePosition, enemyDogMage.transform.position) * ((float) Math.PI / 180f);
    enemyDogMage.Force = (Vector3) (new Vector2(1500f * Mathf.Cos(enemyDogMage.Angle), 1500f * Mathf.Sin(enemyDogMage.Angle)) * enemyDogMage.KnockbackModifier);
    enemyDogMage.rb.AddForce((Vector2) enemyDogMage.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyDogMage.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemyDogMage enemyDogMage = this;
    enemyDogMage.damageColliderEvents.SetActive(false);
    enemyDogMage.Attacking = false;
    enemyDogMage.ClearPaths();
    enemyDogMage.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyDogMage.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, enemyDogMage.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMage.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyDogMage.DisableForces = false;
    enemyDogMage.IdleWait = 0.0f;
    enemyDogMage.StartCoroutine(enemyDogMage.ActiveRoutine());
    if (enemyDogMage.CounterAttack)
      enemyDogMage.StartCoroutine(enemyDogMage.SlamAttack ? enemyDogMage.SlamRoutine() : enemyDogMage.AttackRoutine());
  }

  public void GetNewTargetPosition()
  {
    float num = 100f;
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, closestTarget.transform.position) < (double) this.DistanceToPathTowardsPlayer && this.CheckLineOfSightOnTarget(closestTarget.gameObject, closestTarget.transform.position, (float) this.DistanceToPathTowardsPlayer))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = (float) (((double) Utils.GetAngle(this.transform.position, closestTarget.transform.position) + 180.0) * (Math.PI / 180.0));
    }
    while ((double) --num > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      if (!this.PathingToPlayer)
        this.RandomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      this.PathingToPlayer = false;
      float radius = 0.2f;
      Vector3 targetLocation = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if (this.ShowDebug)
        {
          this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - targetLocation) * this.CircleCastOffset);
          this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
        if (this.ShowDebug)
        {
          this.EndPoints.Add(new Vector3(targetLocation.x, targetLocation.y));
          this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
        this.givePath(targetLocation);
        break;
      }
    }
  }

  public void DoBusiness() => this.StartCoroutine(this.BusinessRoutine());

  public IEnumerator BusinessRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyDogMage enemyDogMage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", enemyDogMage.gameObject);
      GameManager.GetInstance().OnConversationNew();
      PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      DecorationCustomTarget.Create(enemyDogMage.transform.position, PlayerFarming.Instance.gameObject.transform.position, 1f, StructureBrain.TYPES.DECORATION_MONSTERSHRINE, new System.Action(enemyDogMage.FinishedGettingDecoration));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void FinishedGettingDecoration()
  {
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public IEnumerator LineLightningAttack()
  {
    EnemyDogMage enemyDogMage = this;
    enemyDogMage.ClearPaths();
    enemyDogMage.Attacking = true;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, "attack-line", false);
    if (!string.IsNullOrEmpty(enemyDogMage.LineAttackSFX))
      AudioManager.Instance.PlayOneShot(enemyDogMage.LineAttackSFX, enemyDogMage.gameObject);
    if (!string.IsNullOrEmpty(enemyDogMage.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyDogMage.WarningVO, enemyDogMage.transform.position);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyDogMage.delayAttackLine, enemyDogMage.Spine);
    Health target = enemyDogMage.GetClosestTarget();
    Vector3 dir = (target.transform.position - enemyDogMage.transform.position).normalized;
    float distanceBetweenAvalanchesChasing = 1.5f;
    Vector3 originPosition = enemyDogMage.transform.position;
    int SpawnCount = enemyDogMage.numAvalanchesLine;
    if (!string.IsNullOrEmpty(enemyDogMage.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyDogMage.AttackVO, enemyDogMage.transform.position);
    while (SpawnCount > 0)
    {
      if (enemyDogMage.LineChasesPlayer)
        dir = (target.transform.position - originPosition).normalized;
      Vector3 vector3 = originPosition + dir * distanceBetweenAvalanchesChasing;
      originPosition = vector3;
      if (!BiomeGenerator.PointWithinIsland(vector3, out Vector3 _))
      {
        --SpawnCount;
      }
      else
      {
        LightningStrikeAttack lightning = ObjectPool.Spawn<LightningStrikeAttack>(enemyDogMage.lightningStrikeAttack, vector3, Quaternion.identity);
        lightning.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(enemyDogMage.health, vector3, (System.Action) (() => ObjectPool.Recycle<LightningStrikeAttack>(lightning)), true);
        --SpawnCount;
        yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyDogMage.timeBetweenAvalanchesLine, enemyDogMage.Spine);
      }
    }
    enemyDogMage.LineAttackTimer = enemyDogMage.LineAttackDelayTime;
    enemyDogMage.AttackTimer = enemyDogMage.AttackDelayTime;
    enemyDogMage.Attacking = false;
    enemyDogMage.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator CircleAvalanchesAttack()
  {
    EnemyDogMage enemyDogMage = this;
    enemyDogMage.ClearPaths();
    enemyDogMage.Attacking = true;
    enemyDogMage.Spine.AnimationState.SetAnimation(0, "attack-circle", false);
    if (!string.IsNullOrEmpty(enemyDogMage.RingAttackSFX))
      AudioManager.Instance.PlayOneShot(enemyDogMage.RingAttackSFX, enemyDogMage.gameObject);
    if (!string.IsNullOrEmpty(enemyDogMage.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyDogMage.WarningVO, enemyDogMage.transform.position);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyDogMage.delayAttackCircle, enemyDogMage.Spine);
    Vector3 dir = (enemyDogMage.GetClosestTarget().transform.position - enemyDogMage.transform.position).normalized;
    for (int i = 0; i < enemyDogMage.numAvalanchesCircle; ++i)
    {
      Vector3 vector3_1 = enemyDogMage.transform.position + Quaternion.AngleAxis((float) (90 / enemyDogMage.numAvalanchesCircle) + 180f / (float) enemyDogMage.numAvalanchesCircle * (float) i, Vector3.forward) * (dir * enemyDogMage.distanceAvalanchesCircle);
      Vector3 vector3_2 = enemyDogMage.transform.position + Quaternion.AngleAxis((float) -(90 / enemyDogMage.numAvalanchesCircle) - 180f / (float) enemyDogMage.numAvalanchesCircle * (float) i, Vector3.forward) * (dir * enemyDogMage.distanceAvalanchesCircle);
      LightningStrikeAttack lightning1 = ObjectPool.Spawn<LightningStrikeAttack>(enemyDogMage.lightningStrikeAttack, vector3_1, Quaternion.identity);
      lightning1.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(enemyDogMage.health, vector3_1, (System.Action) (() => ObjectPool.Recycle<LightningStrikeAttack>(lightning1)), true);
      LightningStrikeAttack lightning2 = ObjectPool.Spawn<LightningStrikeAttack>(enemyDogMage.lightningStrikeAttack, vector3_2, Quaternion.identity);
      lightning2.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(enemyDogMage.health, vector3_2, (System.Action) (() => ObjectPool.Recycle<LightningStrikeAttack>(lightning2)), true);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyDogMage.timeBetweenAvalanchesCircle, enemyDogMage.Spine);
    }
    enemyDogMage.CircleAttackTimer = enemyDogMage.CircleAttackDelayTime;
    enemyDogMage.AttackTimer = enemyDogMage.AttackDelayTime;
    enemyDogMage.Attacking = false;
    enemyDogMage.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.red);
    if (this.DetectPlayerWhileHidden && (this.StartHidden == EnemyDogMage.StartingStates.Hidden || this.StartHidden == EnemyDogMage.StartingStates.Animation))
      Utils.DrawCircleXY((this.HiddenOffsetIsGlobalPosition ? Vector3.zero : this.transform.position) + this.HiddenOffset, this.HiddenRadius, Color.yellow);
    if ((double) this.ChanceToPathTowardsPlayer > 0.0)
      Utils.DrawCircleXY(this.transform.position, (float) this.DistanceToPathTowardsPlayer, Color.cyan);
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], this.CircleCastRadius, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], this.CircleCastRadius, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }

  public enum StartingStates
  {
    Hidden,
    Wandering,
    Animation,
    Intro,
  }
}
