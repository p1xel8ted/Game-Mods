// Decompiled with JetBrains decompiler
// Type: EnemyKebab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyKebab : UnitObject
{
  public static List<EnemyLambFlesh> Fleshlings = new List<EnemyLambFlesh>();
  public int NewPositionDistance = 3;
  public float MaintainTargetDistance = 4.5f;
  public float MoveCloserDistance = 4f;
  public float ShortRangeAttackMaxRange = 4f;
  public float LongRangeAttackMaxRange = 4f;
  [Range(0.0f, 1f)]
  public float enableChargeColliderAtTrajectoryProgress = 0.8f;
  public Vector2 MaxSlashAttackDelayRandomRange = new Vector2(4f, 6f);
  public Vector2 SlashAttackDelayRandomRange = new Vector2(0.5f, 2f);
  public Vector2 MaxChargeAttackDelayRandomRange = new Vector2(4f, 6f);
  public Vector2 ChargeAttackDelayRandomRange = new Vector2(0.5f, 2f);
  public ColliderEvents slashDamageColliderEvents;
  public ColliderEvents chargeDamageColliderEvents;
  public AssetReferenceGameObject IndicatorPrefabLarge;
  public GameObject loadedTargetReticule;
  public GameObject currentTargetReticule;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  [SerializeField]
  public bool requireLineOfSite = true;
  [SerializeField]
  public float slashAttackWindup = 0.5f;
  [SerializeField]
  public float chargeFlightDuration = 1.2f;
  [SerializeField]
  public AnimationCurve chargeFlightCurve = AnimationCurve.Linear(0.0f, 1f, 0.0f, 1f);
  [SerializeField]
  public float chargeWindup = 0.5f;
  [SerializeField]
  public float ChargeAttackDuration = 0.5f;
  [SerializeField]
  public float afterChargeWaitTime = 0.5f;
  [SerializeField]
  public ParticleSystem landVFX;
  [SerializeField]
  public Throwable throwable;
  [SerializeField]
  public int maxSpawnThrowableNumber = 12;
  [SerializeField]
  public float throwableSpawnRadius = 4f;
  [SerializeField]
  public LayerMask hitLayers;
  public int spawnedThrowableCount;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public float chargeLandShakeIntensity = 3f;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FuseBigAnimation;
  [HideInInspector]
  public bool HasMerged;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public float TeleportDelayTarget = 1f;
  [EventRef]
  public string AttackClawSFX = "event:/dlc/dungeon06/enemy/kebab/attack_claw_start";
  [EventRef]
  public string AttackJumpSFX = "event:/dlc/dungeon06/enemy/kebab/attack_jump_start";
  [EventRef]
  public string MoveSFX = "event:/dlc/dungeon06/enemy/kebab/move";
  [EventRef]
  public string AttackLandSFX = "event:/dlc/dungeon06/enemy/kebab/attack_jump_land";
  [EventRef]
  public string AttackVOX = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/attack";
  [EventRef]
  public string DeathVOX = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/death";
  [EventRef]
  public string GetHitVOX = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/gethit";
  [EventRef]
  public string WarningVOX = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/warning";
  public GameObject TargetObject;
  public float SlashAttackDelay;
  public float ChargeAttackDelay;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public bool \u003CFollowPlayer\u003Ek__BackingField;
  public Vector3 Force;
  public float KnockbackModifier = 1f;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public float TeleportDelay;
  public EnemyKebab.State MyState;
  public float MaxSlashAttackDelay;
  public float MaxChargeAttackDelay;
  public const float TARGET_RETICULE_LERP_SPEED = 5f;
  public Health EnemyHealth;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public float Damage
  {
    get => this.\u003CDamage\u003Ek__BackingField;
    set => this.\u003CDamage\u003Ek__BackingField = value;
  }

  public bool FollowPlayer
  {
    get => this.\u003CFollowPlayer\u003Ek__BackingField;
    set => this.\u003CFollowPlayer\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    ++EnemyLambFlesh.kebabAmount;
    this.Preload();
  }

  public void Preload()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.IndicatorPrefabLarge).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedAddressableAssets.Add(obj);
      this.loadedTargetReticule = obj.Result;
      this.loadedTargetReticule.SetActive(false);
    });
    FleshEgg component;
    if (!this.throwable.gameObject.TryGetComponent<FleshEgg>(out component))
      return;
    FleshEgg.Prewarm((IEnumerable<GameObject>) component.Hatchlings, 3);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if ((UnityEngine.Object) this.slashDamageColliderEvents != (UnityEngine.Object) null)
    {
      this.slashDamageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.slashDamageColliderEvents.SetActive(false);
    }
    if ((UnityEngine.Object) this.chargeDamageColliderEvents != (UnityEngine.Object) null)
    {
      this.chargeDamageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.chargeDamageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
    this.rb.simulated = true;
    this.SlashAttackDelay = UnityEngine.Random.Range(this.SlashAttackDelayRandomRange.x, this.SlashAttackDelayRandomRange.y);
    this.MaxSlashAttackDelay = UnityEngine.Random.Range(this.MaxSlashAttackDelayRandomRange.x, this.MaxSlashAttackDelayRandomRange.y);
    this.ChargeAttackDelay = UnityEngine.Random.Range(this.ChargeAttackDelayRandomRange.x, this.ChargeAttackDelayRandomRange.y);
    this.MaxChargeAttackDelay = UnityEngine.Random.Range(this.MaxChargeAttackDelayRandomRange.x, this.MaxChargeAttackDelayRandomRange.y);
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    if ((UnityEngine.Object) this.slashDamageColliderEvents != (UnityEngine.Object) null)
      this.slashDamageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    if ((UnityEngine.Object) this.chargeDamageColliderEvents != (UnityEngine.Object) null)
      this.chargeDamageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVOX))
      AudioManager.Instance.PlayOneShot(this.DeathVOX, this.transform.position);
    this.TrySummonFleshling();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    --EnemyLambFlesh.kebabAmount;
    if (!(bool) (UnityEngine.Object) this.currentTargetReticule)
      return;
    ObjectPool.Recycle(this.currentTargetReticule);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.health.HasShield || this.health.WasJustParried)
      return;
    if (!string.IsNullOrEmpty(this.GetHitVOX))
      AudioManager.Instance.PlayOneShot(this.GetHitVOX, this.transform.position);
    this.UsePathing = true;
    this.health.invincible = false;
    this.DisableForces = false;
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.LookAtTarget();
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    this.SimpleSpineFlash.FlashFillRed();
    this.TrySummonFleshling();
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyKebab enemyKebab = this;
    enemyKebab.DisableForces = true;
    enemyKebab.Force = (enemyKebab.transform.position - Attacker.transform.position).normalized * 500f;
    enemyKebab.rb.AddForce((Vector2) (enemyKebab.Force * enemyKebab.KnockbackModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyKebab.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyKebab.DisableForces = false;
  }

  public void TrySummonFleshling()
  {
    if (PlayerFarming.Instance?.playerController?.CurrentState.GetValueOrDefault() == StateMachine.State.Resurrecting)
      return;
    int num = (int) (((double) this.health.totalHP - (double) this.health.HP) / (double) (this.health.totalHP / (float) this.maxSpawnThrowableNumber)) - this.spawnedThrowableCount;
    if (this.spawnedThrowableCount >= this.maxSpawnThrowableNumber)
      return;
    for (int index = 0; index < num; ++index)
    {
      this.ThrowEgg();
      ++this.spawnedThrowableCount;
    }
  }

  public void ThrowEgg()
  {
    Vector3 positionWithinIsland = this.GetTargetPositionWithinIsland();
    if (positionWithinIsland == Vector3.zero)
      return;
    Throwable throwable = ObjectPool.Spawn<Throwable>(this.throwable, this.transform.parent, this.transform.position, Quaternion.identity);
    throwable.Launch(positionWithinIsland, 1f);
    Interaction_Chest.Instance?.AddEnemy(throwable.GetComponent<Health>());
  }

  public Vector3 GetTargetPositionWithinIsland()
  {
    float num = 100f;
    while ((double) --num >= 0.0)
    {
      Vector3 closestPoint;
      BiomeGenerator.PointWithinIsland(this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * this.throwableSpawnRadius, out closestPoint);
      if ((UnityEngine.Object) Physics2D.OverlapCircle((Vector2) closestPoint, 0.5f, (int) this.hitLayers, 0.0f, 0.0f) == (UnityEngine.Object) null)
        return closestPoint;
    }
    return Vector3.zero;
  }

  public IEnumerator WaitForTarget()
  {
    EnemyKebab enemyKebab = this;
    enemyKebab.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    if (enemyKebab.Spine.AnimationName != "idle")
      enemyKebab.Spine.AnimationState.SetAnimation(0, "idle", true);
    if (enemyKebab.HasMerged)
      enemyKebab.Spine.AnimationState.SetAnimation(0, enemyKebab.FuseBigAnimation, false);
    yield return (object) new WaitForEndOfFrame();
    if (enemyKebab.HasMerged)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyKebab.Spine.timeScale) < 1.0)
        yield return (object) null;
      enemyKebab.HasMerged = false;
    }
    while ((UnityEngine.Object) enemyKebab.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemyKebab.GetClosestTarget(enemyKebab.health.team == Health.Team.PlayerTeam);
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemyKebab.TargetObject = closestTarget.gameObject;
        enemyKebab.requireLineOfSite = false;
        enemyKebab.VisionRange = int.MaxValue;
      }
      enemyKebab.RepathTimer -= Time.deltaTime * enemyKebab.Spine.timeScale;
      if ((double) enemyKebab.RepathTimer <= 0.0)
      {
        if (enemyKebab.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyKebab.Spine.AnimationName != "move")
          {
            enemyKebab.Spine.AnimationState.SetAnimation(0, "move", true);
            if (!string.IsNullOrEmpty(enemyKebab.MoveSFX))
              AudioManager.Instance.PlayOneShot(enemyKebab.MoveSFX, enemyKebab.gameObject);
          }
        }
        else if (enemyKebab.Spine.AnimationName != "idle")
          enemyKebab.Spine.AnimationState.SetAnimation(0, "idle", true);
        if (!enemyKebab.FollowPlayer)
          enemyKebab.TargetPosition = enemyKebab.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        enemyKebab.FindPath(enemyKebab.TargetPosition);
        enemyKebab.LookAtTarget();
        enemyKebab.Spine.skeleton.ScaleX = (double) enemyKebab.state.LookAngle <= 90.0 || (double) enemyKebab.state.LookAngle >= 270.0 ? -1f : 1f;
      }
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemyKebab.TargetObject == (UnityEngine.Object) null)
      {
        enemyKebab.StartCoroutine((IEnumerator) enemyKebab.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(enemyKebab.TargetObject.transform.position, enemyKebab.transform.position);
      if ((double) a <= (double) enemyKebab.VisionRange)
      {
        if (!enemyKebab.requireLineOfSite || enemyKebab.CheckLineOfSightOnTarget(enemyKebab.TargetObject, enemyKebab.TargetObject.transform.position, Mathf.Min(a, (float) enemyKebab.VisionRange)))
          InRange = true;
        else
          enemyKebab.LookAtTarget();
      }
      yield return (object) null;
    }
    enemyKebab.StartCoroutine((IEnumerator) enemyKebab.ChasePlayer());
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      return;
    this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
  }

  public IEnumerator ChasePlayer()
  {
    EnemyKebab enemyKebab = this;
    enemyKebab.MyState = EnemyKebab.State.WaitAndTaunt;
    enemyKebab.state.CURRENT_STATE = StateMachine.State.Idle;
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyKebab.TargetObject == (UnityEngine.Object) null)
      {
        enemyKebab.StartCoroutine((IEnumerator) enemyKebab.WaitForTarget());
        break;
      }
      if ((UnityEngine.Object) enemyKebab.slashDamageColliderEvents != (UnityEngine.Object) null)
        enemyKebab.slashDamageColliderEvents.SetActive(false);
      enemyKebab.TeleportDelay -= Time.deltaTime * enemyKebab.Spine.timeScale;
      enemyKebab.SlashAttackDelay -= Time.deltaTime * enemyKebab.Spine.timeScale;
      enemyKebab.MaxSlashAttackDelay -= Time.deltaTime * enemyKebab.Spine.timeScale;
      enemyKebab.ChargeAttackDelay -= Time.deltaTime * enemyKebab.Spine.timeScale;
      enemyKebab.MaxChargeAttackDelay -= Time.deltaTime * enemyKebab.Spine.timeScale;
      if (enemyKebab.MyState == EnemyKebab.State.WaitAndTaunt)
      {
        if (enemyKebab.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyKebab.Spine.AnimationName != "move")
          {
            enemyKebab.Spine.AnimationState.SetAnimation(0, "move", true);
            if (!string.IsNullOrEmpty(enemyKebab.MoveSFX))
              AudioManager.Instance.PlayOneShot(enemyKebab.MoveSFX, enemyKebab.gameObject);
          }
        }
        else if (enemyKebab.Spine.AnimationName != "idle")
          enemyKebab.Spine.AnimationState.SetAnimation(0, "idle", true);
        if ((UnityEngine.Object) enemyKebab.TargetObject == (UnityEngine.Object) PlayerFarming.Instance.gameObject && enemyKebab.health.IsCharmed && (UnityEngine.Object) enemyKebab.GetClosestTarget() != (UnityEngine.Object) null)
          enemyKebab.TargetObject = enemyKebab.GetClosestTarget().gameObject;
        enemyKebab.state.LookAngle = Utils.GetAngle(enemyKebab.transform.position, enemyKebab.TargetObject.transform.position);
        enemyKebab.Spine.skeleton.ScaleX = (double) enemyKebab.state.LookAngle <= 90.0 || (double) enemyKebab.state.LookAngle >= 270.0 ? -1f : 1f;
        if (enemyKebab.state.CURRENT_STATE == StateMachine.State.Idle || enemyKebab.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if ((double) (enemyKebab.RepathTimer -= Time.deltaTime * enemyKebab.Spine.timeScale) < 0.0)
          {
            if ((double) enemyKebab.MaxChargeAttackDelay <= 0.0 && (double) Vector3.Distance(enemyKebab.transform.position, enemyKebab.TargetObject.transform.position) < (double) enemyKebab.LongRangeAttackMaxRange && (double) Vector3.Distance(enemyKebab.transform.position, enemyKebab.TargetObject.transform.position) > (double) enemyKebab.ShortRangeAttackMaxRange)
            {
              if ((bool) (UnityEngine.Object) enemyKebab.TargetObject && ((double) enemyKebab.MaxChargeAttackDelay <= 0.0 || (double) enemyKebab.ChargeAttackDelay <= 0.0))
              {
                enemyKebab.health.invincible = false;
                enemyKebab.StopAllCoroutines();
                enemyKebab.StartCoroutine((IEnumerator) enemyKebab.TryChargeAttack());
              }
            }
            else if ((double) enemyKebab.MaxSlashAttackDelay <= 0.0 || (double) Vector3.Distance(enemyKebab.transform.position, enemyKebab.TargetObject.transform.position) < (double) enemyKebab.ShortRangeAttackMaxRange)
              enemyKebab.TryMeleeBehaviour();
            else if ((bool) (UnityEngine.Object) enemyKebab.TargetObject && (double) Vector3.Distance(enemyKebab.transform.position, enemyKebab.TargetObject.transform.position) > (double) enemyKebab.MoveCloserDistance)
            {
              enemyKebab.Angle = (float) (((double) Utils.GetAngle(enemyKebab.TargetObject.transform.position, enemyKebab.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemyKebab.TargetPosition = enemyKebab.TargetObject.transform.position + new Vector3(enemyKebab.MaintainTargetDistance * Mathf.Cos(enemyKebab.Angle), enemyKebab.MaintainTargetDistance * Mathf.Sin(enemyKebab.Angle));
              enemyKebab.FindPath(enemyKebab.TargetPosition);
            }
          }
        }
        else if ((double) (enemyKebab.RepathTimer += Time.deltaTime * enemyKebab.Spine.timeScale) > 2.0)
        {
          enemyKebab.RepathTimer = 0.0f;
          enemyKebab.state.CURRENT_STATE = StateMachine.State.Idle;
        }
      }
      yield return (object) null;
    }
  }

  public void TryMeleeBehaviour()
  {
    if ((double) this.MaxSlashAttackDelay > 0.0 && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) >= (double) this.ShortRangeAttackMaxRange || !(bool) (UnityEngine.Object) this.TargetObject)
      return;
    if ((double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) >= (double) this.MoveCloserDistance)
    {
      this.Angle = (float) (((double) Utils.GetAngle(this.TargetObject.transform.position, this.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
      this.TargetPosition = this.TargetObject.transform.position + new Vector3(this.MaintainTargetDistance * Mathf.Cos(this.Angle), this.MaintainTargetDistance * Mathf.Sin(this.Angle));
      this.FindPath(this.TargetPosition);
    }
    else
    {
      if ((double) this.MaxSlashAttackDelay > 0.0 && (double) this.SlashAttackDelay > 0.0)
        return;
      this.health.invincible = false;
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.TrySlashAttack());
    }
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    if (!string.IsNullOrEmpty(this.WarningVOX))
      AudioManager.Instance.PlayOneShot(this.WarningVOX, this.transform.position);
    this.TargetObject = TargetObject;
    float a = Vector3.Distance(TargetObject.transform.position, this.transform.position);
    if ((double) a > (double) this.VisionRange)
      return;
    if (!this.requireLineOfSite || this.CheckLineOfSightOnTarget(TargetObject, TargetObject.transform.position, Mathf.Min(a, (float) this.VisionRange)))
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    else
      this.LookAtTarget();
  }

  public void FindPath(Vector3 PointToCheck)
  {
    this.RepathTimer = 0.2f;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.NewPositionDistance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) > 1.0)
      {
        if (this.ShowDebug)
        {
          this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset);
          this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset;
        this.givePath(this.TargetPosition);
      }
      else
      {
        if ((double) this.TeleportDelay >= 0.0)
          return;
        this.Teleport();
      }
    }
    else if (this.FollowPlayer && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) <= 1.5)
        return;
      this.TargetPosition = PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle;
      this.givePath(this.TargetPosition);
    }
    else
    {
      this.TargetPosition = PointToCheck;
      this.givePath(PointToCheck);
    }
  }

  public void Teleport()
  {
    if (this.MyState != EnemyKebab.State.WaitAndTaunt || (double) this.health.HP <= 0.0)
      return;
    float num1 = 100f;
    float num2;
    if ((double) (num2 = num1 - 1f) <= 0.0 || (UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      return;
    float f = (float) (((double) Utils.GetAngle(this.transform.position, this.TargetObject.transform.position) + (double) UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
    float distance = 4.5f;
    float radius = 0.2f;
    Vector3 Position = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(Position - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - Position) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.StartCoroutine((IEnumerator) this.TeleportRoutine((Vector3) raycastHit2D.centroid));
    }
    else
    {
      if (this.ShowDebug)
      {
        this.EndPoints.Add(new Vector3(Position.x, Position.y));
        this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.StartCoroutine((IEnumerator) this.TeleportRoutine(Position));
    }
  }

  public IEnumerator TeleportRoutine(Vector3 Position)
  {
    EnemyKebab enemyKebab = this;
    enemyKebab.ClearPaths();
    enemyKebab.state.CURRENT_STATE = StateMachine.State.Moving;
    enemyKebab.UsePathing = false;
    enemyKebab.health.invincible = true;
    enemyKebab.MyState = EnemyKebab.State.Teleporting;
    enemyKebab.ClearPaths();
    Vector3 position = enemyKebab.transform.position;
    float Progress = 0.0f;
    enemyKebab.Spine.AnimationState.SetAnimation(0, "roll", true);
    enemyKebab.LookAtTarget();
    enemyKebab.Spine.skeleton.ScaleX = (double) enemyKebab.state.LookAngle <= 90.0 || (double) enemyKebab.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 b = Position;
    float Duration = Vector3.Distance(position, b) / 10f;
    while ((double) (Progress += Time.deltaTime * enemyKebab.Spine.timeScale) < (double) Duration)
    {
      enemyKebab.speed = 10f * Time.deltaTime * enemyKebab.Spine.timeScale;
      yield return (object) null;
    }
    enemyKebab.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyKebab.Spine.AnimationState.SetAnimation(0, "roll-stop", false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyKebab.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyKebab.UsePathing = true;
    enemyKebab.RepathTimer = 0.5f;
    enemyKebab.TeleportDelay = enemyKebab.TeleportDelayTarget;
    enemyKebab.health.invincible = false;
    enemyKebab.MyState = EnemyKebab.State.WaitAndTaunt;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
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

  public IEnumerator TryChargeAttack()
  {
    EnemyKebab enemyKebab = this;
    enemyKebab.MyState = EnemyKebab.State.Charging;
    float SignPostDelay = enemyKebab.chargeWindup;
    bool Loop = true;
    enemyKebab.ClearPaths();
    while (Loop)
    {
      if ((UnityEngine.Object) enemyKebab.Spine == (UnityEngine.Object) null || enemyKebab.Spine.AnimationState == null || enemyKebab.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        Vector3 targetPos;
        float time;
        switch (enemyKebab.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) enemyKebab.TargetObject)
            {
              enemyKebab.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyKebab.Spine.AnimationState.SetAnimation(0, "attack-launch-charge", false);
              if (!string.IsNullOrEmpty(enemyKebab.AttackJumpSFX))
                AudioManager.Instance.PlayOneShot(enemyKebab.AttackJumpSFX, enemyKebab.gameObject);
              if (!string.IsNullOrEmpty(enemyKebab.WarningVOX))
                AudioManager.Instance.PlayOneShot(enemyKebab.WarningVOX, enemyKebab.gameObject);
              enemyKebab.Spine.skeleton.ScaleX = (double) enemyKebab.state.LookAngle <= 90.0 || (double) enemyKebab.state.LookAngle >= 270.0 ? -1f : 1f;
              enemyKebab.LookAtTarget();
              enemyKebab.ActivateTargetReticule(enemyKebab.TargetObject.transform.position);
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyKebab.TargetObject == (UnityEngine.Object) null)
              enemyKebab.StartCoroutine((IEnumerator) enemyKebab.WaitForTarget());
            enemyKebab.state.Timer += Time.deltaTime * enemyKebab.Spine.timeScale;
            targetPos = Vector3.MoveTowards(enemyKebab.currentTargetReticule.transform.position, enemyKebab.TargetObject.transform.position, 5f * Time.deltaTime * enemyKebab.Spine.timeScale);
            enemyKebab.SetTargetReticulePosition(targetPos);
            if ((double) enemyKebab.state.Timer >= (double) SignPostDelay)
            {
              if (!string.IsNullOrEmpty(enemyKebab.AttackVOX))
                AudioManager.Instance.PlayOneShot(enemyKebab.AttackVOX, enemyKebab.transform.position);
              enemyKebab.isImmuneToKnockback = true;
              enemyKebab.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, enemyKebab.state.LookAngle);
              enemyKebab.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              enemyKebab.Spine.AnimationState.SetAnimation(0, "attack-launch-impact", false);
              Vector3 startPos = enemyKebab.transform.position;
              enemyKebab.SetTargetReticulePosition(targetPos);
              enemyKebab.state.LookAngle = Utils.GetAngle(enemyKebab.transform.position, targetPos);
              time = 0.0f;
              bool toggledCollider = false;
              while ((double) time < (double) enemyKebab.ChargeAttackDuration)
              {
                if (!PlayerRelic.TimeFrozen)
                {
                  time += Time.deltaTime * enemyKebab.Spine.timeScale;
                  enemyKebab.transform.position = Vector3.Lerp(startPos, targetPos, enemyKebab.chargeFlightCurve.Evaluate(time / enemyKebab.ChargeAttackDuration));
                  if (!toggledCollider && (double) time / (double) enemyKebab.ChargeAttackDuration >= (double) enemyKebab.enableChargeColliderAtTrajectoryProgress)
                  {
                    enemyKebab.chargeDamageColliderEvents.SetActive(true);
                    toggledCollider = true;
                    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
                    if (!string.IsNullOrEmpty(enemyKebab.AttackLandSFX))
                      AudioManager.Instance.PlayOneShot(enemyKebab.AttackLandSFX, enemyKebab.gameObject);
                  }
                }
                yield return (object) null;
              }
              enemyKebab.DisableTargetReticule();
              enemyKebab.landVFX.Play();
              startPos = new Vector3();
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            enemyKebab.StartCoroutine((IEnumerator) enemyKebab.EnableChargeDamageCollider(0.0f));
            enemyKebab.isImmuneToKnockback = false;
            enemyKebab.ChargeAttackDelay = UnityEngine.Random.Range(enemyKebab.ChargeAttackDelayRandomRange.x, enemyKebab.ChargeAttackDelayRandomRange.y);
            enemyKebab.MaxChargeAttackDelay = UnityEngine.Random.Range(enemyKebab.MaxChargeAttackDelayRandomRange.x, enemyKebab.MaxChargeAttackDelayRandomRange.y);
            CameraManager.shakeCamera(enemyKebab.chargeLandShakeIntensity, 270f);
            time = 0.0f;
            while ((double) time < (double) enemyKebab.afterChargeWaitTime)
            {
              time += Time.deltaTime * enemyKebab.Spine.timeScale;
              yield return (object) null;
            }
            Loop = false;
            enemyKebab.SimpleSpineFlash.FlashWhite(false);
            break;
          default:
            enemyKebab.TargetObject = (GameObject) null;
            enemyKebab.StartCoroutine((IEnumerator) enemyKebab.WaitForTarget());
            yield break;
        }
        targetPos = new Vector3();
        yield return (object) null;
      }
    }
    enemyKebab.TargetObject = (GameObject) null;
    enemyKebab.StartCoroutine((IEnumerator) enemyKebab.WaitForTarget());
  }

  public void ActivateTargetReticule(Vector3 targetPosition)
  {
    if (!((UnityEngine.Object) this.loadedTargetReticule != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.currentTargetReticule == (UnityEngine.Object) null)
      this.currentTargetReticule = ObjectPool.Spawn(this.loadedTargetReticule);
    this.currentTargetReticule.transform.position = targetPosition;
    this.currentTargetReticule.SetActive(true);
  }

  public void SetTargetReticulePosition(Vector3 targetPosition)
  {
    if (!((UnityEngine.Object) this.currentTargetReticule != (UnityEngine.Object) null))
      return;
    this.currentTargetReticule.transform.position = targetPosition;
  }

  public void DisableTargetReticule()
  {
    if (!((UnityEngine.Object) this.currentTargetReticule != (UnityEngine.Object) null))
      return;
    ObjectPool.Recycle(this.currentTargetReticule);
    this.currentTargetReticule = (GameObject) null;
  }

  public IEnumerator TrySlashAttack(float AttackDistance = 1.5f)
  {
    EnemyKebab enemyKebab = this;
    enemyKebab.MyState = EnemyKebab.State.Attacking;
    enemyKebab.UsePathing = true;
    enemyKebab.givePath(enemyKebab.TargetObject.transform.position);
    enemyKebab.RepathTimer = 0.0f;
    int NumAttacks = 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = enemyKebab.slashAttackWindup;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyKebab.Spine == (UnityEngine.Object) null || enemyKebab.Spine.AnimationState == null || enemyKebab.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        switch (enemyKebab.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            enemyKebab.TargetObject = (GameObject) null;
            enemyKebab.StartCoroutine((IEnumerator) enemyKebab.WaitForTarget());
            yield break;
          case StateMachine.State.Moving:
            if ((bool) (UnityEngine.Object) enemyKebab.TargetObject)
            {
              enemyKebab.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              if (!string.IsNullOrEmpty(enemyKebab.AttackClawSFX))
                AudioManager.Instance.PlayOneShot(enemyKebab.AttackClawSFX, enemyKebab.gameObject);
              enemyKebab.Spine.AnimationState.SetAnimation(0, "attack-melee-charge", false);
              enemyKebab.Spine.skeleton.ScaleX = (double) enemyKebab.state.LookAngle <= 90.0 || (double) enemyKebab.state.LookAngle >= 270.0 ? -1f : 1f;
              enemyKebab.LookAtTarget();
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyKebab.slashDamageColliderEvents != (UnityEngine.Object) null)
              enemyKebab.slashDamageColliderEvents.SetActive(false);
            enemyKebab.state.Timer += Time.deltaTime * enemyKebab.Spine.timeScale;
            if ((double) enemyKebab.state.Timer >= (double) SignPostDelay)
            {
              enemyKebab.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, enemyKebab.state.LookAngle);
              enemyKebab.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              enemyKebab.Spine.AnimationState.SetAnimation(0, "attack-melee-impact", false);
              enemyKebab.StartCoroutine((IEnumerator) enemyKebab.EnableSlashDamageCollider(0.0f));
              if (!string.IsNullOrEmpty(enemyKebab.AttackVOX))
              {
                AudioManager.Instance.PlayOneShot(enemyKebab.AttackVOX, enemyKebab.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime * enemyKebab.Spine.timeScale;
            enemyKebab.SimpleSpineFlash.FlashWhite(false);
            if ((double) (enemyKebab.state.Timer += Time.deltaTime * enemyKebab.Spine.timeScale) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
            {
              if (++AttackCount <= NumAttacks)
              {
                AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
                enemyKebab.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                SignPostDelay = 0.3f;
                break;
              }
              enemyKebab.SlashAttackDelay = UnityEngine.Random.Range(enemyKebab.SlashAttackDelayRandomRange.x, enemyKebab.SlashAttackDelayRandomRange.y);
              enemyKebab.MaxSlashAttackDelay = UnityEngine.Random.Range(enemyKebab.MaxSlashAttackDelayRandomRange.x, enemyKebab.MaxSlashAttackDelayRandomRange.y);
              Loop = false;
              enemyKebab.SimpleSpineFlash.FlashWhite(false);
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    enemyKebab.TargetObject = (GameObject) null;
    enemyKebab.StartCoroutine((IEnumerator) enemyKebab.WaitForTarget());
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.FollowPlayer)
      return;
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null))
      return;
    if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  public IEnumerator EnableSlashDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.slashDamageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.slashDamageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      this.slashDamageColliderEvents.SetActive(false);
    }
  }

  public IEnumerator EnableChargeDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.chargeDamageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.chargeDamageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      this.chargeDamageColliderEvents.SetActive(false);
    }
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__58_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    this.loadedTargetReticule = obj.Result;
    this.loadedTargetReticule.SetActive(false);
  }

  public enum State
  {
    WaitAndTaunt,
    Teleporting,
    Attacking,
    Charging,
  }
}
