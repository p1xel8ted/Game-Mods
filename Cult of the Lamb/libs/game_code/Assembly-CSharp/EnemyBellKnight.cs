// Decompiled with JetBrains decompiler
// Type: EnemyBellKnight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyBellKnight : UnitObject
{
  public static float LastSimpleGuardianAttacked = float.MinValue;
  public static float LastSimpleGuardianRingProjectiles = float.MinValue;
  public static float LastSimpleGuardianPatternShot = float.MinValue;
  [SerializeField]
  public bool requireLineOfSite = true;
  public GameObject TargetObject;
  public Health EnemyHealth;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public SimpleSpineEventListener simpleSpineEventListener;
  public List<Collider2D> collider2DList;
  public Collider2D HealthCollider;
  public GameObject guardianGameObject;
  public bool AlwaysMoveToRandomPoint;
  public string WalkAnimation = "walk2";
  public float GlobalAttackDelay = 2f;
  public float GlobalRingAttackDelay = 10f;
  public float GlobalPatternAttackDelay = 5f;
  public Vector3 TargetPosition;
  public Vector2 AttackRandomDelay = new Vector2(0.0f, 0.0f);
  public float LocalAttackDelay;
  public int NumberOfAttacks = 2;
  public string ForceAttackAnimation = "";
  public float DurationOfAttack = 0.5f;
  public bool UpdateAngleAfterAttack;
  public float AttackDeceleration = 1f;
  public float SpeedOfAttack = 15f;
  public bool UseForceForAttack;
  public float AttackForce = 1000f;
  public ParticleSystem DashParticles;
  public ProjectilePattern projectilePattern;
  [EventRef]
  public string BasicAttackUpSwingSFX = "event:/dlc/dungeon05/enemy/bellknight/attack_basic_swing";
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string BasicAttackWindupSFX = "event:/dlc/dungeon05/enemy/bellknight/attack_basic_windup";
  [EventRef]
  public string ChargedAttackWindupSFX = "event:/dlc/dungeon05/enemy/bellknight/attack_charged_windup";
  [EventRef]
  public string ChargedAttackImpactSFX = "event:/dlc/dungeon05/enemy/bellknight/attack_charged_impact";
  [EventRef]
  public string ChargedAttackWindupVO = "event:/dlc/dungeon05/enemy/bellknight/attack_charged_windup_vo";
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/attack";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/warning";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/gethit";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/death";
  [EventRef]
  public string DashSFX = "event:/dlc/dungeon05/enemy/bellknight/attack_charged_start";
  [EventRef]
  public string RainAttackSFX = "event:/dlc/dungeon05/enemy/bellknight/attack_rain_start";
  [EventRef]
  public string AttackSmallSpikeEmergeSFX = "event:/dlc/dungeon05/enemy/bellknight/attack_straightline_icepillar_spawn";
  public EnemyBellKnight.BellKnightAttacks currentAttack;
  [SerializeField]
  public Transform avalancheOrigin;
  [SerializeField]
  [Tooltip("0%-100%")]
  public float coneAttackProbability;
  [SerializeField]
  public GameObject attackImpactVFX;
  [SerializeField]
  public float hitGroundShakeIntensity = 5f;
  [SerializeField]
  public float swingImpactTime = 0.25f;
  [SerializeField]
  public float swingRadius = 0.8f;
  [SerializeField]
  public float swingXOffset = 1.5f;
  [SerializeField]
  public float groundAttackRadius = 1.25f;
  [SerializeField]
  public float groundAttackXOffset = 2.36f;
  [SerializeField]
  public float coneImpactTime = 1f;
  [SerializeField]
  public float timeBetweenConeAndRain = 1f;
  [SerializeField]
  public float bigConeImpactTime = 1f;
  [SerializeField]
  public int rainAvalancheAmount = 20;
  [SerializeField]
  public float timeBetweenRainAvalanche = 0.15f;
  public float rainRecoveryTime = 2f;
  public float delayBeforeRainStarts = 1f;
  [Header("Ice Attacks")]
  public GameObject iceWallPrefab;
  public LightningStrikeAttack lightningStrikeAttack;
  public float iceStrikeCooldown = 0.5f;
  public float quickIceStrikeCooldown = 0.5f;
  public GameObject indicatorPrefab;
  public float delayAttackLine = 0.5f;
  public int numStrikesPerLineIceCross = 4;
  public int numStrikesPerLineQuickIce = 8;
  public float lineDistanceIceCross = 7f;
  public float lineDistanceQuickIce = 9f;
  public float timeBetweenStrikesInLine = 0.3f;
  public float hammerOnGroundTime = 0.3f;
  public static List<EnemyBellKnight> SimpleGuardians = new List<EnemyBellKnight>();
  public bool ChasingPlayer;
  public List<Health> currentWalls = new List<Health>();
  public int maxWalls = 40;
  public List<GameObject> currentIndicators = new List<GameObject>();
  public string chargedAttackStartAnim = "attack-4-start";
  public string chargedAttackLoopAnim = "attack-4-loop";
  public string chargedAttackEndAnim = "attack-4-end";
  public DeadBodySliding deadBodySliding;

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    EnemyBellKnight.SimpleGuardians.Add(this);
    this.SimpleSpineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
    this.simpleSpineEventListener = this.GetComponent<SimpleSpineEventListener>();
    this.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.HealthCollider = this.GetComponent<Collider2D>();
    this.DashParticles.Stop();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.health.OnAddCharm += new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared += new Health.StasisEvent(this.ReconsiderTarget);
    this.TargetPosition = this.transform.position;
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    EnemyBellKnight.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
    EnemyBellKnight.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
    EnemyBellKnight.LastSimpleGuardianPatternShot = GameManager.GetInstance().CurrentTime;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyBellKnight.SimpleGuardians.Remove(this);
    this.simpleSpineEventListener.OnSpineEvent -= new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.health.OnAddCharm -= new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared -= new Health.StasisEvent(this.ReconsiderTarget);
    foreach (GameObject currentIndicator in this.currentIndicators)
    {
      if (currentIndicator != null)
        currentIndicator.Recycle();
    }
    this.currentIndicators.Clear();
  }

  public override void Awake()
  {
    base.Awake();
    this.guardianGameObject = this.gameObject;
    if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null)
      DataManager.Instance.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
    this.LoadAssets();
  }

  public void LoadAssets()
  {
    this.iceWallPrefab.CreatePool(50, true);
    this.indicatorPrefab.CreatePool(20, true);
  }

  public IEnumerator WaitForTarget()
  {
    EnemyBellKnight enemyBellKnight = this;
    enemyBellKnight.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemyBellKnight.TargetObject == (UnityEngine.Object) null)
    {
      enemyBellKnight.SetTargetObject();
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemyBellKnight.TargetObject == (UnityEngine.Object) null)
      {
        enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(enemyBellKnight.TargetObject.transform.position, enemyBellKnight.transform.position);
      if ((double) a <= (double) enemyBellKnight.VisionRange)
      {
        if (!enemyBellKnight.requireLineOfSite || enemyBellKnight.CheckLineOfSightOnTarget(enemyBellKnight.TargetObject, enemyBellKnight.TargetObject.transform.position, Mathf.Min(a, (float) enemyBellKnight.VisionRange)))
          InRange = true;
        else
          enemyBellKnight.LookAtTarget();
      }
      yield return (object) null;
    }
    enemyBellKnight.StopAllCoroutines();
    enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.FightPlayer());
  }

  public void SetTargetObject()
  {
    Health closestTarget = this.GetClosestTarget();
    if (!(bool) (UnityEngine.Object) closestTarget)
      return;
    this.TargetObject = closestTarget.gameObject;
    this.requireLineOfSite = false;
    this.VisionRange = int.MaxValue;
  }

  public void ReconsiderTarget()
  {
    this.TargetObject = (GameObject) null;
    this.SetTargetObject();
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StopAllCoroutines();
  }

  public void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "Invincible On":
        this.health.invincible = true;
        this.HealthCollider.enabled = false;
        break;
      case "Invincible Off":
        this.health.invincible = false;
        this.HealthCollider.enabled = true;
        break;
      case "attack-hit-ground":
        AudioManager.Instance.PlayOneShot("event:/chests/chest_big_land", this.gameObject);
        AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.gameObject);
        if (this.currentAttack == EnemyBellKnight.BellKnightAttacks.CHARGED_ATTACK)
          AudioManager.Instance.PlayOneShot(this.ChargedAttackImpactSFX, this.gameObject);
        CameraManager.shakeCamera(this.hitGroundShakeIntensity, 270f);
        if (!((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null))
          break;
        UnityEngine.Object.Instantiate<GameObject>(this.attackImpactVFX, this.avalancheOrigin.position, Quaternion.identity);
        break;
    }
  }

  public void GetPath()
  {
    this.DisableForces = false;
    this.ChasingPlayer = false;
    if (this.AlwaysMoveToRandomPoint)
    {
      this.TargetPosition = BiomeGenerator.GetRandomPositionInIsland();
      this.ChasingPlayer = false;
    }
    else if (EnemyBellKnight.SimpleGuardians.Count > 1)
    {
      Debug.Log((object) "CHECK! ");
      float num1 = float.MaxValue;
      EnemyBellKnight enemyBellKnight = (EnemyBellKnight) null;
      foreach (EnemyBellKnight simpleGuardian in EnemyBellKnight.SimpleGuardians)
      {
        float num2 = Vector3.Distance(simpleGuardian.transform.position, this.TargetObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          enemyBellKnight = simpleGuardian;
        }
      }
      if ((UnityEngine.Object) enemyBellKnight == (UnityEngine.Object) this)
      {
        this.TargetPosition = this.TargetObject.transform.position;
        this.ChasingPlayer = true;
      }
      else
      {
        if (!enemyBellKnight.ChasingPlayer)
          enemyBellKnight.GetPath();
        this.TargetPosition = BiomeGenerator.GetRandomPositionInIsland();
        this.ChasingPlayer = false;
      }
    }
    else
    {
      this.TargetPosition = this.TargetObject.transform.position;
      this.ChasingPlayer = true;
    }
    if ((double) Vector3.Distance(this.TargetPosition, this.transform.position) > (double) this.StoppingDistance)
    {
      this.givePath(this.TargetPosition);
      if (!(this.Spine.AnimationName != this.WalkAnimation))
        return;
      this.Spine.AnimationState.SetAnimation(0, this.WalkAnimation, true);
    }
    else
    {
      if (!(this.Spine.AnimationName != "idle"))
        return;
      this.Spine.AnimationState.SetAnimation(0, "idle", true);
    }
  }

  public IEnumerator FightPlayer()
  {
    EnemyBellKnight enemyBellKnight = this;
    while (EnemyBellKnight.SimpleGuardians.Count <= 1 && ((UnityEngine.Object) enemyBellKnight.TargetObject == (UnityEngine.Object) null || (double) Vector3.Distance(enemyBellKnight.TargetObject.transform.position, enemyBellKnight.transform.position) > 12.0))
    {
      if ((UnityEngine.Object) enemyBellKnight.TargetObject != (UnityEngine.Object) null)
        enemyBellKnight.LookAtTarget();
      yield return (object) null;
    }
    enemyBellKnight.GetPath();
    float RepathTimer = 0.0f;
    enemyBellKnight.LocalAttackDelay = UnityEngine.Random.Range(enemyBellKnight.AttackRandomDelay.x, enemyBellKnight.AttackRandomDelay.y);
    float AttackSpeed = enemyBellKnight.SpeedOfAttack;
    bool Loop = true;
    while (Loop)
    {
      switch (enemyBellKnight.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
        case StateMachine.State.Moving:
          enemyBellKnight.LookAtTarget();
          if ((double) Vector2.Distance((Vector2) enemyBellKnight.transform.position, (Vector2) enemyBellKnight.TargetObject.transform.position) < 8.0)
          {
            if ((double) GameManager.GetInstance().CurrentTime > ((double) EnemyBellKnight.LastSimpleGuardianAttacked + (double) enemyBellKnight.GlobalAttackDelay + (double) enemyBellKnight.LocalAttackDelay) / (double) enemyBellKnight.Spine.timeScale)
            {
              if ((double) UnityEngine.Random.value < 0.5)
              {
                DataManager.Instance.LastSimpleGuardianAttacked = TimeManager.TotalElapsedGameTime;
                EnemyBellKnight.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
                enemyBellKnight.LocalAttackDelay = UnityEngine.Random.Range(enemyBellKnight.AttackRandomDelay.x, enemyBellKnight.AttackRandomDelay.y);
                enemyBellKnight.currentAttack = EnemyBellKnight.BellKnightAttacks.MELEE;
                enemyBellKnight.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
                yield return (object) enemyBellKnight.QuickIceAttack();
                break;
              }
              DataManager.Instance.LastSimpleGuardianAttacked = TimeManager.TotalElapsedGameTime;
              EnemyBellKnight.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
              enemyBellKnight.LocalAttackDelay = UnityEngine.Random.Range(enemyBellKnight.AttackRandomDelay.x, enemyBellKnight.AttackRandomDelay.y);
              enemyBellKnight.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyBellKnight.currentAttack = EnemyBellKnight.BellKnightAttacks.PREPARATION_SWING;
              enemyBellKnight.Spine.AnimationState.SetAnimation(0, "attack-1", false);
              enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.PlayOneShotWithDelay(enemyBellKnight.BasicAttackUpSwingSFX, 0.3f));
              AudioManager.Instance.PlayOneShot(enemyBellKnight.WarningVO, enemyBellKnight.transform.position);
              if (enemyBellKnight.UseForceForAttack)
              {
                enemyBellKnight.DisableForces = true;
                Vector3 force = (Vector3) new Vector2(enemyBellKnight.AttackForce * Mathf.Cos(enemyBellKnight.state.facingAngle * ((float) Math.PI / 180f)), enemyBellKnight.AttackForce * Mathf.Sin(enemyBellKnight.state.facingAngle * ((float) Math.PI / 180f)));
                enemyBellKnight.rb.AddForce((Vector2) force);
                enemyBellKnight.DashParticles.Play();
                break;
              }
              break;
            }
            if (enemyBellKnight.state.CURRENT_STATE != StateMachine.State.Idle)
            {
              enemyBellKnight.state.CURRENT_STATE = StateMachine.State.Idle;
              enemyBellKnight.Spine.AnimationState.SetAnimation(0, "idle", true);
            }
          }
          if (enemyBellKnight.ChasingPlayer)
          {
            if ((double) Vector2.Distance((Vector2) enemyBellKnight.transform.position, (Vector2) enemyBellKnight.TargetObject.transform.position) >= 3.0 && (double) (RepathTimer += Time.deltaTime * enemyBellKnight.Spine.timeScale) > 0.20000000298023224)
            {
              RepathTimer = 0.0f;
              enemyBellKnight.GetPath();
            }
          }
          else if (enemyBellKnight.state.CURRENT_STATE == StateMachine.State.Idle)
          {
            if (enemyBellKnight.Spine.AnimationName != "idle")
              enemyBellKnight.Spine.AnimationState.SetAnimation(0, "idle", true);
            if ((double) (RepathTimer += Time.deltaTime * enemyBellKnight.Spine.timeScale) > 1.0)
            {
              RepathTimer = 0.0f;
              enemyBellKnight.GetPath();
            }
          }
          if ((UnityEngine.Object) enemyBellKnight.damageColliderEvents != (UnityEngine.Object) null)
          {
            enemyBellKnight.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          if (enemyBellKnight.UpdateAngleAfterAttack && (UnityEngine.Object) enemyBellKnight.TargetObject != (UnityEngine.Object) null)
          {
            if ((double) enemyBellKnight.state.Timer + (double) Time.deltaTime * (double) enemyBellKnight.Spine.timeScale < 0.25)
              enemyBellKnight.state.facingAngle = Utils.GetAngle(enemyBellKnight.transform.position, enemyBellKnight.TargetObject.transform.position);
          }
          else
            enemyBellKnight.state.facingAngle = Utils.GetAngle(enemyBellKnight.transform.position, enemyBellKnight.TargetPosition);
          float num1 = enemyBellKnight.currentAttack == EnemyBellKnight.BellKnightAttacks.CHARGED_ATTACK ? enemyBellKnight.bigConeImpactTime : (enemyBellKnight.currentAttack == EnemyBellKnight.BellKnightAttacks.PREPARATION_SWING ? enemyBellKnight.swingImpactTime : enemyBellKnight.coneImpactTime);
          if ((double) (enemyBellKnight.state.Timer += Time.deltaTime * enemyBellKnight.Spine.timeScale) >= (double) num1)
          {
            enemyBellKnight.SimpleSpineFlash.FlashWhite(false);
            if (enemyBellKnight.currentAttack == EnemyBellKnight.BellKnightAttacks.PREPARATION_SWING && !string.IsNullOrEmpty(enemyBellKnight.DashSFX))
              AudioManager.Instance.PlayOneShot(enemyBellKnight.DashSFX, enemyBellKnight.gameObject);
            CameraManager.shakeCamera(0.4f, enemyBellKnight.state.facingAngle);
            enemyBellKnight.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemyBellKnight.speed = AttackSpeed * Time.deltaTime;
            if ((UnityEngine.Object) enemyBellKnight.damageColliderEvents != (UnityEngine.Object) null)
            {
              if (enemyBellKnight.currentAttack == EnemyBellKnight.BellKnightAttacks.PREPARATION_SWING)
                enemyBellKnight.SetDamageColliderParams(enemyBellKnight.swingXOffset, enemyBellKnight.swingRadius);
              else
                enemyBellKnight.SetDamageColliderParams(enemyBellKnight.groundAttackXOffset, enemyBellKnight.groundAttackRadius);
              enemyBellKnight.damageColliderEvents.SetActive(true);
            }
            MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
            switch (enemyBellKnight.currentAttack)
            {
              case EnemyBellKnight.BellKnightAttacks.SPECIAL:
                enemyBellKnight.damageColliderEvents.SetActive(false);
                if ((double) UnityEngine.Random.value < 0.25)
                {
                  yield return (object) enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.IceWallRain());
                  break;
                }
                break;
              case EnemyBellKnight.BellKnightAttacks.CHARGED_ATTACK:
                yield return (object) enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.IceCrossAttack());
                break;
            }
          }
          else
          {
            if (enemyBellKnight.currentAttack != EnemyBellKnight.BellKnightAttacks.SPECIAL)
            {
              enemyBellKnight.SimpleSpineFlash.FlashWhite(enemyBellKnight.state.Timer / 0.5f);
              break;
            }
            break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) AttackSpeed > 0.0)
            AttackSpeed -= enemyBellKnight.AttackDeceleration * GameManager.DeltaTime * enemyBellKnight.Spine.timeScale;
          enemyBellKnight.speed = AttackSpeed * Time.deltaTime * enemyBellKnight.Spine.timeScale;
          float num2 = enemyBellKnight.currentAttack == EnemyBellKnight.BellKnightAttacks.PREPARATION_SWING ? 0.1f : 0.25f;
          if ((double) enemyBellKnight.state.Timer >= (double) num2 && (UnityEngine.Object) enemyBellKnight.damageColliderEvents != (UnityEngine.Object) null)
            enemyBellKnight.damageColliderEvents.SetActive(false);
          enemyBellKnight.state.Timer += Time.deltaTime * enemyBellKnight.Spine.timeScale;
          float num3 = enemyBellKnight.DurationOfAttack;
          if (enemyBellKnight.currentAttack == EnemyBellKnight.BellKnightAttacks.MELEE)
            num3 = enemyBellKnight.timeBetweenConeAndRain;
          if ((double) enemyBellKnight.state.Timer >= (double) num3)
          {
            if (enemyBellKnight.UseForceForAttack)
            {
              enemyBellKnight.DisableForces = false;
              enemyBellKnight.rb.velocity = Vector2.zero;
            }
            enemyBellKnight.DashParticles.Stop();
            switch (enemyBellKnight.currentAttack)
            {
              case EnemyBellKnight.BellKnightAttacks.PREPARATION_SWING:
                enemyBellKnight.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                enemyBellKnight.ReconsiderTarget();
                if ((double) UnityEngine.Random.value < (double) enemyBellKnight.coneAttackProbability / 100.0)
                {
                  enemyBellKnight.LookAtTarget();
                  yield return (object) null;
                  if (!string.IsNullOrEmpty(enemyBellKnight.BasicAttackWindupSFX))
                    AudioManager.Instance.PlayOneShot(enemyBellKnight.BasicAttackWindupSFX, enemyBellKnight.transform.position);
                  enemyBellKnight.Spine.AnimationState.SetAnimation(0, "attack-2", false);
                  enemyBellKnight.currentAttack = EnemyBellKnight.BellKnightAttacks.MELEE;
                  break;
                }
                enemyBellKnight.LookAtTarget();
                yield return (object) null;
                if (!string.IsNullOrEmpty(enemyBellKnight.ChargedAttackWindupSFX))
                  AudioManager.Instance.PlayOneShot(enemyBellKnight.ChargedAttackWindupSFX, enemyBellKnight.gameObject);
                if (!string.IsNullOrEmpty(enemyBellKnight.ChargedAttackWindupVO))
                  AudioManager.Instance.PlayOneShot(enemyBellKnight.ChargedAttackWindupVO, enemyBellKnight.gameObject);
                enemyBellKnight.Spine.AnimationState.SetAnimation(0, enemyBellKnight.chargedAttackStartAnim, false);
                enemyBellKnight.Spine.AnimationState.AddAnimation(0, enemyBellKnight.chargedAttackLoopAnim, true, 0.0f);
                enemyBellKnight.currentAttack = EnemyBellKnight.BellKnightAttacks.CHARGED_ATTACK;
                break;
              case EnemyBellKnight.BellKnightAttacks.MELEE:
                float angle = Utils.GetAngle(enemyBellKnight.transform.position, enemyBellKnight.GetClosestTarget().transform.position);
                if (enemyBellKnight.CanDoRainAttack(angle))
                {
                  enemyBellKnight.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                  if (!string.IsNullOrEmpty(enemyBellKnight.RainAttackSFX))
                    AudioManager.Instance.PlayOneShot(enemyBellKnight.RainAttackSFX, enemyBellKnight.gameObject);
                  enemyBellKnight.Spine.AnimationState.SetAnimation(0, "attack-3-start", false);
                  enemyBellKnight.Spine.AnimationState.AddAnimation(0, "attack-3-loop", true, 0.0f);
                  enemyBellKnight.currentAttack = EnemyBellKnight.BellKnightAttacks.SPECIAL;
                  break;
                }
                Loop = false;
                enemyBellKnight.state.CURRENT_STATE = StateMachine.State.Idle;
                enemyBellKnight.ReconsiderTarget();
                enemyBellKnight.Spine.AnimationState.SetAnimation(0, "attack-2-recover", false);
                enemyBellKnight.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
                enemyBellKnight.DashParticles.Stop();
                break;
              case EnemyBellKnight.BellKnightAttacks.SPECIAL:
                Loop = false;
                enemyBellKnight.state.CURRENT_STATE = StateMachine.State.Idle;
                enemyBellKnight.Spine.AnimationState.SetAnimation(0, "attack-3-end", false);
                enemyBellKnight.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
                enemyBellKnight.ReconsiderTarget();
                enemyBellKnight.DashParticles.Stop();
                break;
              case EnemyBellKnight.BellKnightAttacks.CHARGED_ATTACK:
                if ((double) enemyBellKnight.state.Timer >= (double) enemyBellKnight.DurationOfAttack + (double) enemyBellKnight.hammerOnGroundTime)
                {
                  enemyBellKnight.Spine.AnimationState.SetAnimation(0, enemyBellKnight.chargedAttackEndAnim, false);
                  enemyBellKnight.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
                  enemyBellKnight.state.CURRENT_STATE = StateMachine.State.Vulnerable;
                  break;
                }
                break;
              default:
                Loop = false;
                enemyBellKnight.state.CURRENT_STATE = StateMachine.State.Idle;
                enemyBellKnight.ReconsiderTarget();
                enemyBellKnight.Spine.AnimationState.SetAnimation(0, "idle", true);
                enemyBellKnight.DashParticles.Stop();
                break;
            }
          }
          else
            break;
          break;
        case StateMachine.State.Vulnerable:
          float hammerLiftAnim = enemyBellKnight.Spine.skeleton.Data.FindAnimation(enemyBellKnight.chargedAttackEndAnim).Duration;
          enemyBellKnight.state.Timer += Time.deltaTime * enemyBellKnight.Spine.timeScale;
          yield return (object) null;
          if ((double) enemyBellKnight.state.Timer >= (double) hammerLiftAnim)
          {
            enemyBellKnight.ReconsiderTarget();
            Loop = false;
            enemyBellKnight.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          break;
      }
      yield return (object) null;
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBellKnight.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyBellKnight.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyBellKnight.Spine.AnimationState.SetAnimation(0, "idle", true);
    if ((UnityEngine.Object) enemyBellKnight.TargetObject != (UnityEngine.Object) null && (double) Vector3.Distance(enemyBellKnight.TargetObject.transform.position, enemyBellKnight.transform.position) > 5.0)
    {
      enemyBellKnight.LookAtTarget();
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyBellKnight.Spine.timeScale) < 1.0)
        yield return (object) null;
    }
    enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.WaitForTarget());
  }

  public bool CanDoRainAttack(float targetAngle)
  {
    if ((double) this.Spine.skeleton.ScaleX < 0.0 && (double) targetAngle > 90.0 && (double) targetAngle < 270.0)
      return true;
    if ((double) this.Spine.skeleton.ScaleX <= 0.0)
      return false;
    return (double) targetAngle < 90.0 || (double) targetAngle > 270.0;
  }

  public IEnumerator IceCrossAttack()
  {
    EnemyBellKnight enemyBellKnight = this;
    int num = (double) enemyBellKnight.Spine.skeleton.ScaleX > 0.0 ? 1 : -1;
    enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.LineIceWallAttack(enemyBellKnight.transform.right * (float) num, enemyBellKnight.lineDistanceIceCross, enemyBellKnight.numStrikesPerLineIceCross));
    enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.LineIceWallAttack((enemyBellKnight.transform.right + enemyBellKnight.transform.up).normalized * (float) num, enemyBellKnight.lineDistanceIceCross, enemyBellKnight.numStrikesPerLineIceCross));
    enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.LineIceWallAttack((enemyBellKnight.transform.right + -enemyBellKnight.transform.up).normalized * (float) num, enemyBellKnight.lineDistanceIceCross, enemyBellKnight.numStrikesPerLineIceCross));
    yield return (object) new WaitForSeconds(enemyBellKnight.iceStrikeCooldown);
  }

  public IEnumerator QuickIceAttack()
  {
    EnemyBellKnight enemyBellKnight = this;
    string animationName = "attack-2";
    enemyBellKnight.Spine.AnimationState.SetAnimation(0, animationName, false);
    enemyBellKnight.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/bellknight/attack_straightline_start", enemyBellKnight.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/warning", enemyBellKnight.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    Vector3 zero = Vector3.zero;
    Vector3 normalized;
    if ((UnityEngine.Object) enemyBellKnight.TargetObject == (UnityEngine.Object) null)
    {
      float f = enemyBellKnight.state.facingAngle * ((float) Math.PI / 180f);
      normalized = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f).normalized;
    }
    else
      normalized = (enemyBellKnight.TargetObject.transform.position - enemyBellKnight.avalancheOrigin.position).normalized;
    yield return (object) enemyBellKnight.StartCoroutine((IEnumerator) enemyBellKnight.LineIceWallAttackDouble(normalized, enemyBellKnight.lineDistanceQuickIce, enemyBellKnight.numStrikesPerLineQuickIce));
  }

  public IEnumerator LineIceWallAttackDouble(
    Vector3 direction,
    float distance,
    int numStrikesPerLine)
  {
    EnemyBellKnight enemyBellKnight = this;
    Vector3 position1 = enemyBellKnight.avalancheOrigin.position;
    enemyBellKnight.ClearPaths();
    List<Vector3> impactPositions = new List<Vector3>();
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < numStrikesPerLine; ++index)
    {
      Vector3 point = position1 + direction * (float) (index + 1) * (distance / (float) numStrikesPerLine);
      if (BiomeGenerator.PointWithinIsland(point, out Vector3 _))
        impactPositions.Add(point);
    }
    yield return (object) new WaitForSeconds(enemyBellKnight.delayAttackLine);
    Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0.0f);
    float spikeOffset = 0.5f;
    float maxForwardOffset = 0.3f;
    float maxSideJitter = 0.1f;
    bool pillarSideLeft = (double) UnityEngine.Random.value > 0.5;
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_large/attack", enemyBellKnight.transform.position);
    for (int i = 0; i < impactPositions.Count; ++i)
    {
      Vector3 vector3_1 = impactPositions[i];
      Vector3 vector3_2 = -perpendicular * spikeOffset;
      Vector3 vector3_3 = perpendicular * spikeOffset;
      float num1 = UnityEngine.Random.Range(-maxForwardOffset, maxForwardOffset);
      float num2 = UnityEngine.Random.Range(-maxForwardOffset, maxForwardOffset);
      float num3 = UnityEngine.Random.Range(-maxSideJitter, maxSideJitter);
      float num4 = UnityEngine.Random.Range(-maxSideJitter, maxSideJitter);
      Vector3 position2 = vector3_1 + vector3_2 + direction.normalized * num1 + perpendicular * num3;
      Vector3 position3 = vector3_1 + vector3_3 + direction.normalized * num2 + perpendicular * num4;
      if (pillarSideLeft)
      {
        GameObject wall = ObjectPool.Spawn(enemyBellKnight.iceWallPrefab, position2, Quaternion.identity);
        enemyBellKnight.InitWall(wall);
      }
      else
      {
        GameObject wall = ObjectPool.Spawn(enemyBellKnight.iceWallPrefab, position3, Quaternion.identity);
        enemyBellKnight.InitWall(wall);
      }
      yield return (object) new WaitForSeconds(enemyBellKnight.timeBetweenStrikesInLine * 0.5f);
    }
  }

  public IEnumerator LineIceWallAttack(Vector3 direction, float distance, int numStrikesPerLine)
  {
    EnemyBellKnight enemyBellKnight = this;
    Vector3 position = enemyBellKnight.avalancheOrigin.position;
    enemyBellKnight.ClearPaths();
    List<Vector3> impactPositions = new List<Vector3>();
    List<GameObject> indicators = new List<GameObject>();
    for (int index = 0; index < numStrikesPerLine; ++index)
    {
      Vector3 vector3 = position + direction * (float) (index + 1) * (distance / (float) numStrikesPerLine);
      if (BiomeGenerator.PointWithinIsland(vector3, out Vector3 _))
      {
        impactPositions.Add(vector3);
        GameObject gameObject = ObjectPool.Spawn(enemyBellKnight.indicatorPrefab, vector3);
        indicators.Add(gameObject);
        enemyBellKnight.currentIndicators.Add(gameObject);
      }
    }
    yield return (object) new WaitForSeconds(enemyBellKnight.delayAttackLine);
    for (int i = 0; i < impactPositions.Count; ++i)
    {
      GameObject wall = ObjectPool.Spawn(enemyBellKnight.iceWallPrefab, impactPositions[i], Quaternion.identity);
      enemyBellKnight.InitWall(wall);
      enemyBellKnight.currentIndicators.Remove(indicators[i]);
      GameObject gameObject = indicators[i];
      if (gameObject != null)
        gameObject.Recycle();
      yield return (object) new WaitForSeconds(enemyBellKnight.timeBetweenStrikesInLine);
    }
  }

  public IEnumerator IceWallRain()
  {
    List<Vector3> impactPositions = new List<Vector3>();
    List<GameObject> indicators = new List<GameObject>();
    for (int index = 0; index < this.rainAvalancheAmount; ++index)
    {
      Vector3 randomPointInRing = this.GetRandomPointInRing(1f, 8f);
      if (BiomeGenerator.PointWithinIsland(randomPointInRing, out Vector3 _))
      {
        impactPositions.Add(randomPointInRing);
        GameObject gameObject = ObjectPool.Spawn(this.indicatorPrefab, randomPointInRing);
        indicators.Add(gameObject);
        this.currentIndicators.Add(gameObject);
      }
    }
    yield return (object) new WaitForSeconds(this.delayBeforeRainStarts);
    for (int i = 0; i < impactPositions.Count; ++i)
    {
      this.currentIndicators.Remove(indicators[i]);
      GameObject gameObject = indicators[i];
      if (gameObject != null)
        gameObject.Recycle();
      this.InitWall(ObjectPool.Spawn(this.iceWallPrefab, impactPositions[i], Quaternion.identity));
      yield return (object) new WaitForSeconds(this.timeBetweenRainAvalanche);
    }
    yield return (object) new WaitForSeconds(this.rainRecoveryTime);
  }

  public Vector3 GetRandomPointInRing(float innerRadius, float outerRadius)
  {
    float f = UnityEngine.Random.Range(0.0f, 6.28318548f);
    float num = Mathf.Sqrt(UnityEngine.Random.Range(innerRadius * innerRadius, outerRadius * outerRadius));
    return new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f) * num;
  }

  public void InitWall(GameObject wall)
  {
    Health componentInChildren = wall.GetComponentInChildren<Health>();
    IceWall iceWall = wall.GetComponent<IceWall>();
    iceWall.ignoreOtherTraps = false;
    DOVirtual.DelayedCall(0.25f, (TweenCallback) (() => iceWall.ignoreOtherTraps = true));
    iceWall.ignoreOtherTraps = true;
    iceWall.InitWall(this.Spine, componentInChildren);
    if ((bool) (UnityEngine.Object) componentInChildren)
    {
      this.currentWalls.Add(componentInChildren);
      componentInChildren.OnDie += new Health.DieAction(this.OnWallDies);
    }
    if (!string.IsNullOrEmpty(this.AttackSmallSpikeEmergeSFX))
      AudioManager.Instance.PlayOneShot(this.AttackSmallSpikeEmergeSFX, wall.transform.position);
    this.DoWallCleanup();
  }

  public void DoWallCleanup()
  {
    if (this.currentWalls.Count <= this.maxWalls)
      return;
    this.currentWalls[0].DealDamage(999f, this.currentWalls[0].gameObject, this.currentWalls[0].transform.position);
  }

  public void OnCollisionEnter2D(Collision2D other)
  {
    IceWall componentInParent = other.gameObject.GetComponentInParent<IceWall>();
    if (!((UnityEngine.Object) componentInParent != (UnityEngine.Object) null))
      return;
    componentInParent.Kill(this.gameObject);
  }

  public void OnWallDies(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.currentWalls.Contains(Victim))
      return;
    this.currentWalls.Remove(Victim);
  }

  public void SetDamageColliderParams(float XOffset, float radius)
  {
    this.damageColliderEvents.GetComponent<CircleCollider2D>().radius = radius;
    this.damageColliderEvents.transform.localPosition = this.damageColliderEvents.transform.localPosition with
    {
      x = XOffset
    };
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !component.IsCharmedEnemy)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public IEnumerator PlayOneShotWithDelay(string sfx, float delay)
  {
    EnemyBellKnight enemyBellKnight = this;
    float t = 0.0f;
    while ((double) t < (double) delay)
    {
      t += Time.deltaTime * enemyBellKnight.Spine.timeScale;
      yield return (object) null;
    }
    if (!string.IsNullOrEmpty(sfx))
      AudioManager.Instance.PlayOneShot(sfx, enemyBellKnight.gameObject);
  }

  public enum BellKnightAttacks
  {
    PREPARATION_SWING,
    MELEE,
    SPECIAL,
    CHARGED_ATTACK,
  }
}
