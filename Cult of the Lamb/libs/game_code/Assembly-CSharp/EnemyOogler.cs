// Decompiled with JetBrains decompiler
// Type: EnemyOogler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

#nullable disable
public class EnemyOogler : UnitObject
{
  public int NewPositionDistance = 3;
  public float MaintainTargetDistance = 4.5f;
  public float MoveCloserDistance = 4f;
  public float AttackWithinRange = 4f;
  public bool DoubleAttack = true;
  [FormerlySerializedAs("ChargeAndAttack")]
  public bool DefensiveAttack = true;
  public Vector2 MaxAttackDelayRandomRange = new Vector2(4f, 6f);
  public Vector2 AttackDelayRandomRange = new Vector2(0.5f, 2f);
  public ColliderEvents damageColliderEvents;
  [SerializeField]
  public bool requireLineOfSite = true;
  public bool CanBeInterrupted = true;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnim;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string teleportAnim;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string teleportOutAnim;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string summonAnim;
  [Space]
  public ParticleSystem summonParticles;
  public ParticleSystem teleportEffect;
  public ParticleSystem ghostDeathParticles;
  [SerializeField]
  public EnemyOoglerBell ooglerBell;
  [SerializeField]
  public float BellCastTime;
  [SerializeField]
  public Vector2 timeBetweenAttacks;
  [SerializeField]
  public AssetReferenceGameObject ooglerPrefab;
  [SerializeField]
  public SkeletonRendererCustomMaterials overrideMaterial;
  [SerializeField]
  public float teleportCastDuration;
  public bool QueuedTeleport;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  [HideInInspector]
  public Health cloneParentHealth;
  [EventRef]
  public string AttackClawStartSFX = "event:/dlc/dungeon05/enemy/oogler/attack_claw_start_vo";
  [EventRef]
  public string AttackClawSwipeSFX = "event:/dlc/dungeon05/enemy/oogler/attack_claw_swipe";
  [EventRef]
  public string AttackClowSwipeVO = "event:/dlc/dungeon05/enemy/oogler/attack_claw_swipe_vo";
  [EventRef]
  public string AttackSummonGhostStartSFX = "event:/dlc/dungeon05/enemy/oogler/attack_ghost_start";
  [EventRef]
  public string AttackSummonGhostStartVO = "event:/dlc/dungeon05/enemy/oogler/attack_ghost_start_vo";
  [EventRef]
  public string AttackGhostLoop = "event:/dlc/dungeon05/enemy/oogler/attack_ghost_loop";
  [EventRef]
  public string AttackGhostSwipeSFX = "event:/dlc/dungeon05/enemy/oogler/attack_ghost_swipe";
  [EventRef]
  public string TeleportStartSFX = "event:/dlc/dungeon05/enemy/oogler/mv_teleport_start";
  [EventRef]
  public string TeleportStartVO = "event:/dlc/dungeon05/enemy/oogler/mv_teleport_start_vo";
  [EventRef]
  public string TeleportAwaySFX = "event:/enemy/teleport_away";
  [EventRef]
  public string TeleportAppearSFX = "event:/enemy/teleport_appear";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/oogler/death_vo";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/oogler/gethit_vo";
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  public EventInstance ghostLoopInstance;
  public AsyncOperationHandle<GameObject> loadedOoglerAsset;
  public GameObject TargetObject;
  public float AttackDelay;
  public bool canBeParried;
  public static float signPostParryWindow = 0.2f;
  public static float attackParryWindow = 0.15f;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public float \u003CTeam2Damage\u003Ek__BackingField = 8f;
  [CompilerGenerated]
  public bool \u003CFollowPlayer\u003Ek__BackingField;
  public float attackTime;
  [CompilerGenerated]
  public bool \u003CIsClone\u003Ek__BackingField;
  public Vector3 offset;
  public int attacksPerformedCounter;
  public int pointTrys;
  public float minCloneAttackDistance = 3.5f;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public EnemyOogler.State MyState;
  public float MaxAttackDelay;
  public Color ghostAttackFlashColor = Color.red;
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

  public float Team2Damage
  {
    get => this.\u003CTeam2Damage\u003Ek__BackingField;
    set => this.\u003CTeam2Damage\u003Ek__BackingField = value;
  }

  public bool FollowPlayer
  {
    get => this.\u003CFollowPlayer\u003Ek__BackingField;
    set => this.\u003CFollowPlayer\u003Ek__BackingField = value;
  }

  public bool IsClone
  {
    get => this.\u003CIsClone\u003Ek__BackingField;
    set => this.\u003CIsClone\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    this.PreloadAssets();
  }

  public void PreloadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.ooglerPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedOoglerAsset = obj;
      obj.Result.CreatePool(3, true);
    });
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
    this.attackTime = Time.time + UnityEngine.Random.Range(this.timeBetweenAttacks.x, this.timeBetweenAttacks.y);
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    if (!this.IsClone)
      return;
    this.CleanupGhostLoop();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void CleanupGhostLoop() => AudioManager.Instance.StopLoop(this.ghostLoopInstance);

  public new void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!PlayerController.CanParryAttacks || !this.canBeParried || FromBehind || AttackType != Health.AttackTypes.Melee)
      return;
    this.health.WasJustParried = true;
    this.SimpleSpineFlash.FlashWhite(false);
    this.SeperateObject = true;
    this.UsePathing = true;
    this.health.invincible = false;
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
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    if (!this.ghostDeathParticles.isPlaying)
    {
      this.ghostDeathParticles.Play();
      this.ghostDeathParticles.transform.parent = (Transform) null;
    }
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
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
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if (this.MyState != EnemyOogler.State.Attacking)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.DisableForces = false;
    }
    else
      this.QueuedTeleport = true;
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
    this.SimpleSpineFlash.FlashFillRed();
    if (this.QueuedTeleport)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.CastTeleportMechanicRoutine(this.GetDistantRandomPoint()));
  }

  public Vector3 GetDistantRandomPoint()
  {
    while (!((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null))
    {
      Vector3 position = this.TargetObject.transform.position;
      Vector3 positionInIsland = BiomeGenerator.GetRandomPositionInIsland();
      if ((double) Vector3.Distance(positionInIsland, position) < (double) this.MaintainTargetDistance)
      {
        if (this.pointTrys >= 10)
        {
          this.pointTrys = 0;
          return Vector3.zero;
        }
        ++this.pointTrys;
      }
      else
      {
        this.pointTrys = 0;
        return positionInIsland;
      }
    }
    return Vector3.zero;
  }

  public override void FixedUpdate()
  {
    if (this.IsClone)
      return;
    base.FixedUpdate();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyOogler enemyOogler = this;
    enemyOogler.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemyOogler.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemyOogler.GetClosestTarget(enemyOogler.health.team == Health.Team.PlayerTeam);
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemyOogler.TargetObject = closestTarget.gameObject;
        enemyOogler.requireLineOfSite = false;
        enemyOogler.VisionRange = int.MaxValue;
        if (!enemyOogler.TargetHasBells(enemyOogler.TargetObject) && (double) UnityEngine.Random.value < 0.5)
        {
          enemyOogler.StartCoroutine((IEnumerator) enemyOogler.CastOoglerBell());
          yield break;
        }
        if (enemyOogler.CanSpawnClones())
        {
          enemyOogler.StartCoroutine((IEnumerator) enemyOogler.SpawnClones());
          yield break;
        }
      }
      enemyOogler.RepathTimer -= Time.deltaTime * enemyOogler.Spine.timeScale;
      if ((double) enemyOogler.RepathTimer <= 0.0)
      {
        if (enemyOogler.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyOogler.Spine.AnimationName != "run")
            enemyOogler.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemyOogler.Spine.AnimationName != enemyOogler.idleAnim)
          enemyOogler.Spine.AnimationState.SetAnimation(0, enemyOogler.idleAnim, true);
        if (!enemyOogler.FollowPlayer)
          enemyOogler.TargetPosition = enemyOogler.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        enemyOogler.FindPath(enemyOogler.TargetPosition);
        enemyOogler.state.LookAngle = Utils.GetAngle(enemyOogler.transform.position, enemyOogler.TargetPosition);
      }
      yield return (object) null;
    }
    if (enemyOogler.QueuedTeleport)
    {
      enemyOogler.StopAllCoroutines();
      enemyOogler.StartCoroutine((IEnumerator) enemyOogler.CastTeleportMechanicRoutine(enemyOogler.GetDistantRandomPoint()));
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemyOogler.TargetObject == (UnityEngine.Object) null)
      {
        enemyOogler.StartCoroutine((IEnumerator) enemyOogler.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(enemyOogler.TargetObject.transform.position, enemyOogler.transform.position);
      if ((double) a <= (double) enemyOogler.VisionRange)
      {
        if (!enemyOogler.requireLineOfSite || enemyOogler.CheckLineOfSightOnTarget(enemyOogler.TargetObject, enemyOogler.TargetObject.transform.position, Mathf.Min(a, (float) enemyOogler.VisionRange)))
          InRange = true;
        else
          enemyOogler.LookAtTarget();
      }
      yield return (object) null;
    }
    enemyOogler.StartCoroutine((IEnumerator) enemyOogler.KeepDistanceFromPlayer());
  }

  public bool TargetHasBells(GameObject target) => true;

  public bool CanSpawnClones()
  {
    return !this.IsClone && (double) Time.time > (double) this.attackTime && (!((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null) || (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) > (double) this.minCloneAttackDistance);
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public IEnumerator CastOoglerBell()
  {
    EnemyOogler caster = this;
    caster.attackTime = Time.time + UnityEngine.Random.Range(caster.timeBetweenAttacks.x, caster.timeBetweenAttacks.y);
    if ((UnityEngine.Object) caster.TargetObject == (UnityEngine.Object) null)
    {
      caster.StartCoroutine((IEnumerator) caster.WaitForTarget());
    }
    else
    {
      float time = 0.0f;
      caster.Spine.AnimationState.SetAnimation(0, caster.summonAnim, false);
      while ((double) (time += Time.deltaTime * caster.Spine.timeScale) < (double) caster.BellCastTime)
        yield return (object) null;
      caster.Spine.AnimationState.SetAnimation(0, caster.idleAnim, true);
      UnityEngine.Object.Instantiate<EnemyOoglerBell>(caster.ooglerBell, caster.TargetObject.transform.position, Quaternion.identity).Initialize((UnitObject) caster, caster.TargetObject);
      caster.StartCoroutine((IEnumerator) caster.WaitForTarget());
    }
  }

  public IEnumerator SpawnClones()
  {
    EnemyOogler enemyOogler = this;
    enemyOogler.MyState = EnemyOogler.State.Attacking;
    enemyOogler.attackTime = Time.time + UnityEngine.Random.Range(enemyOogler.timeBetweenAttacks.x, enemyOogler.timeBetweenAttacks.y);
    if ((UnityEngine.Object) enemyOogler.TargetObject == (UnityEngine.Object) null)
    {
      enemyOogler.StartCoroutine((IEnumerator) enemyOogler.WaitForTarget());
    }
    else
    {
      float time = 0.0f;
      enemyOogler.Spine.AnimationState.SetAnimation(0, enemyOogler.summonAnim, false);
      if (!string.IsNullOrEmpty(enemyOogler.AttackSummonGhostStartSFX))
        AudioManager.Instance.PlayOneShot(enemyOogler.AttackSummonGhostStartSFX);
      if (!string.IsNullOrEmpty(enemyOogler.AttackSummonGhostStartVO))
        AudioManager.Instance.PlayOneShot(enemyOogler.AttackSummonGhostStartVO);
      while ((double) (time += Time.deltaTime * enemyOogler.Spine.timeScale) < (double) enemyOogler.BellCastTime)
      {
        enemyOogler.SimpleSpineFlash.FlashWhite(time / enemyOogler.BellCastTime);
        enemyOogler.state.LookAngle = enemyOogler.state.facingAngle = Utils.GetAngle(enemyOogler.transform.position, enemyOogler.TargetObject.transform.position);
        yield return (object) null;
      }
      enemyOogler.SimpleSpineFlash.FlashWhite(false);
      enemyOogler.Spine.AnimationState.SetAnimation(0, enemyOogler.idleAnim, true);
      int amount = 1;
      float delay = 0.0f;
      for (int i = 0; i < amount; ++i)
      {
        EnemyOogler component = ObjectPool.Spawn(enemyOogler.loadedOoglerAsset.Result).GetComponent<EnemyOogler>();
        if ((bool) (UnityEngine.Object) component)
        {
          component.transform.parent = (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null ? BiomeGenerator.Instance.CurrentRoom.generateRoom.transform : (Transform) null;
          component.transform.position = enemyOogler.transform.position;
          component.CanHaveModifier = false;
          component.MoveCloserDistance = 0.0f;
          component.SeperateObject = false;
          component.MaintainTargetDistance = 0.0f;
          component.health.HP = 0.01f;
          component.health.totalHP = 0.01f;
          component.IsClone = true;
          component.health.CanBeCharmed = false;
          component.health.CanBeBurned = false;
          component.health.CanBeElectrified = false;
          component.health.CanBeIced = false;
          component.health.CanBePoisoned = false;
          component.health.CanIncreaseDamageMultiplier = false;
          component.health.RemoveBurn();
          component.health.ClearBurn();
          component.health.ClearIce();
          component.health.ClearPoison();
          component.health.ClearElectrified();
          component.cloneParentHealth = enemyOogler.health;
          component.maxSpeed = 7.5f;
          if ((UnityEngine.Object) component.GetComponent<SpawnDeadBodyOnDeath>() != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) component.GetComponent<SpawnDeadBodyOnDeath>());
          component.StopAllCoroutines();
          Health closestTarget = enemyOogler.GetClosestTarget();
          component.StartCoroutine((IEnumerator) component.CloneAttack(closestTarget));
          SkeletonRendererCustomMaterials.AtlasMaterialOverride materialOverride = component.overrideMaterial.customMaterialOverrides[0] with
          {
            overrideDisabled = false
          };
          component.overrideMaterial.customMaterialOverrides[0] = materialOverride;
          component.overrideMaterial.UpdateMaterials();
          component.Spine.GetComponent<SkeletonGhost>().ghostingEnabled = true;
          component.GetComponent<SpawnParticlesOnDeath>().enabled = false;
          component.DeathVO = string.Empty;
          component.ghostLoopInstance = AudioManager.Instance.CreateLoop(enemyOogler.AttackGhostLoop, component.gameObject, true);
        }
        yield return (object) new WaitForSeconds(delay);
      }
      enemyOogler.MyState = EnemyOogler.State.WaitAndTaunt;
      enemyOogler.StartCoroutine((IEnumerator) enemyOogler.WaitForTarget());
      ++enemyOogler.attacksPerformedCounter;
    }
  }

  public IEnumerator KeepDistanceFromPlayer()
  {
    EnemyOogler enemyOogler = this;
    enemyOogler.MyState = EnemyOogler.State.WaitAndTaunt;
    enemyOogler.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyOogler.AttackDelay = UnityEngine.Random.Range(enemyOogler.AttackDelayRandomRange.x, enemyOogler.AttackDelayRandomRange.y);
    if (enemyOogler.health.HasShield)
      enemyOogler.AttackDelay = 2.5f;
    enemyOogler.MaxAttackDelay = UnityEngine.Random.Range(enemyOogler.MaxAttackDelayRandomRange.x, enemyOogler.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyOogler.TargetObject == (UnityEngine.Object) null)
      {
        enemyOogler.StartCoroutine((IEnumerator) enemyOogler.WaitForTarget());
        break;
      }
      if (!enemyOogler.TargetHasBells(enemyOogler.TargetObject) && (double) UnityEngine.Random.value < 0.5)
      {
        enemyOogler.StartCoroutine((IEnumerator) enemyOogler.CastOoglerBell());
        break;
      }
      if (enemyOogler.CanSpawnClones())
      {
        enemyOogler.StartCoroutine((IEnumerator) enemyOogler.SpawnClones());
        break;
      }
      if ((UnityEngine.Object) enemyOogler.damageColliderEvents != (UnityEngine.Object) null)
        enemyOogler.damageColliderEvents.SetActive(false);
      enemyOogler.AttackDelay -= (float) ((double) Time.deltaTime * (double) enemyOogler.Spine.timeScale * 2.0);
      enemyOogler.MaxAttackDelay -= (float) ((double) Time.deltaTime * (double) enemyOogler.Spine.timeScale * 2.0);
      if (enemyOogler.MyState == EnemyOogler.State.WaitAndTaunt)
      {
        if ((UnityEngine.Object) enemyOogler.TargetObject == (UnityEngine.Object) PlayerFarming.Instance.gameObject && enemyOogler.health.IsCharmed && (UnityEngine.Object) enemyOogler.GetClosestTarget() != (UnityEngine.Object) null)
          enemyOogler.TargetObject = enemyOogler.GetClosestTarget().gameObject;
        enemyOogler.state.LookAngle = enemyOogler.state.facingAngle = Utils.GetAngle(enemyOogler.transform.position, enemyOogler.TargetObject.transform.position);
        if (enemyOogler.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          if ((double) (enemyOogler.RepathTimer -= Time.deltaTime * enemyOogler.Spine.timeScale) < 0.0)
          {
            if (enemyOogler.CustomAttackLogic())
              break;
            if ((double) enemyOogler.MaxAttackDelay < 0.0 || (double) Vector3.Distance(enemyOogler.transform.position, enemyOogler.TargetObject.transform.position) < (double) enemyOogler.AttackWithinRange)
            {
              if ((bool) (UnityEngine.Object) enemyOogler.TargetObject)
              {
                if (enemyOogler.DefensiveAttack && ((double) enemyOogler.MaxAttackDelay < 0.0 || (double) enemyOogler.AttackDelay < 0.0))
                {
                  enemyOogler.health.invincible = false;
                  enemyOogler.StopAllCoroutines();
                  enemyOogler.DisableForces = false;
                  enemyOogler.StartCoroutine((IEnumerator) enemyOogler.FightPlayer());
                }
                else if (!enemyOogler.health.HasShield)
                {
                  enemyOogler.Angle = (float) (((double) Utils.GetAngle(enemyOogler.TargetObject.transform.position, enemyOogler.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                  enemyOogler.TargetPosition = enemyOogler.TargetObject.transform.position + new Vector3((enemyOogler.MaintainTargetDistance + (float) UnityEngine.Random.Range(0, 3)) * Mathf.Cos(enemyOogler.Angle), (enemyOogler.MaintainTargetDistance + (float) UnityEngine.Random.Range(0, 3)) * Mathf.Sin(enemyOogler.Angle));
                  enemyOogler.FindPath(enemyOogler.TargetPosition);
                }
              }
            }
            else if ((bool) (UnityEngine.Object) enemyOogler.TargetObject && (double) Vector3.Distance(enemyOogler.transform.position, enemyOogler.TargetObject.transform.position) > (double) enemyOogler.MoveCloserDistance + (enemyOogler.health.HasShield ? 0.0 : 1.0))
            {
              enemyOogler.Angle = (float) (((double) Utils.GetAngle(enemyOogler.TargetObject.transform.position, enemyOogler.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemyOogler.TargetPosition = enemyOogler.TargetObject.transform.position + new Vector3((enemyOogler.MaintainTargetDistance + (float) UnityEngine.Random.Range(0, 3)) * Mathf.Cos(enemyOogler.Angle), (enemyOogler.MaintainTargetDistance + (float) UnityEngine.Random.Range(0, 3)) * Mathf.Sin(enemyOogler.Angle));
              enemyOogler.FindPath(enemyOogler.TargetPosition);
            }
          }
        }
        else if (enemyOogler.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (((double) enemyOogler.MaxAttackDelay < 0.0 || (double) Vector3.Distance(enemyOogler.transform.position, enemyOogler.TargetObject.transform.position) < (double) enemyOogler.AttackWithinRange) && (bool) (UnityEngine.Object) enemyOogler.TargetObject)
          {
            if (enemyOogler.DefensiveAttack && ((double) enemyOogler.MaxAttackDelay < 0.0 || (double) enemyOogler.AttackDelay < 0.0))
            {
              enemyOogler.health.invincible = false;
              enemyOogler.StopAllCoroutines();
              enemyOogler.DisableForces = false;
              enemyOogler.StartCoroutine((IEnumerator) enemyOogler.FightPlayer());
            }
            else if (!enemyOogler.health.HasShield)
            {
              enemyOogler.Angle = (float) (((double) Utils.GetAngle(enemyOogler.TargetObject.transform.position, enemyOogler.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemyOogler.TargetPosition = enemyOogler.TargetObject.transform.position + new Vector3((enemyOogler.MaintainTargetDistance + (float) UnityEngine.Random.Range(0, 3)) * Mathf.Cos(enemyOogler.Angle), (enemyOogler.MaintainTargetDistance + (float) UnityEngine.Random.Range(0, 3)) * Mathf.Sin(enemyOogler.Angle));
              enemyOogler.FindPath(enemyOogler.TargetPosition);
            }
          }
        }
        else if ((double) (enemyOogler.RepathTimer += Time.deltaTime * enemyOogler.Spine.timeScale) > 2.0)
        {
          enemyOogler.RepathTimer = 0.0f;
          enemyOogler.state.CURRENT_STATE = StateMachine.State.Idle;
        }
      }
      enemyOogler.Seperate(0.5f);
      yield return (object) null;
    }
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    if (!string.IsNullOrEmpty(this.WarningVO))
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
    this.TargetObject = TargetObject;
    float a = Vector3.Distance(TargetObject.transform.position, this.transform.position);
    if ((double) a > (double) this.VisionRange)
      return;
    if (!this.requireLineOfSite || this.CheckLineOfSightOnTarget(TargetObject, TargetObject.transform.position, Mathf.Min(a, (float) this.VisionRange)))
      this.StartCoroutine((IEnumerator) this.WaitForTarget());
    else
      this.LookAtTarget();
  }

  public virtual bool CustomAttackLogic() => false;

  public void FindPath(Vector3 PointToCheck)
  {
    this.RepathTimer = 0.2f;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.NewPositionDistance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) <= 1.0)
        return;
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset;
      this.givePath(this.TargetPosition);
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

  public IEnumerator CastTeleportMechanicRoutine(Vector3 destination)
  {
    EnemyOogler enemyOogler = this;
    enemyOogler.health.invincible = true;
    enemyOogler.QueuedTeleport = false;
    enemyOogler.ClearPaths();
    enemyOogler.state.CURRENT_STATE = StateMachine.State.Teleporting;
    enemyOogler.UsePathing = false;
    enemyOogler.SeperateObject = false;
    enemyOogler.MyState = EnemyOogler.State.Teleporting;
    enemyOogler.ClearPaths();
    enemyOogler.Spine.AnimationState.SetAnimation(0, enemyOogler.teleportAnim, false);
    enemyOogler.state.facingAngle = enemyOogler.state.LookAngle = Utils.GetAngle(enemyOogler.transform.position, destination);
    if (!string.IsNullOrEmpty(enemyOogler.TeleportStartVO))
      AudioManager.Instance.PlayOneShot(enemyOogler.TeleportStartVO);
    if (!string.IsNullOrEmpty(enemyOogler.TeleportStartSFX))
      AudioManager.Instance.PlayOneShot(enemyOogler.TeleportStartSFX);
    float teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * enemyOogler.Spine.timeScale) < (double) enemyOogler.teleportCastDuration)
      yield return (object) null;
    enemyOogler.transform.position = destination;
    enemyOogler.teleportEffect.Play();
    enemyOogler.summonParticles.Play();
    enemyOogler.health.SetDrawBurnParticles(false);
    if (!string.IsNullOrEmpty(enemyOogler.TeleportAwaySFX))
      AudioManager.Instance.PlayOneShot(enemyOogler.TeleportAwaySFX);
    enemyOogler.DisableForces = false;
    enemyOogler.rb.velocity = (Vector2) Vector3.zero;
    enemyOogler.vx = enemyOogler.vy = 0.0f;
    enemyOogler.speed = 0.0f;
    enemyOogler.Spine.AnimationState.SetAnimation(0, enemyOogler.teleportOutAnim, false);
    enemyOogler.Spine.AnimationState.AddAnimation(0, enemyOogler.idleAnim, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyOogler.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyOogler.UsePathing = true;
    enemyOogler.RepathTimer = 0.5f;
    enemyOogler.SeperateObject = true;
    enemyOogler.MyState = EnemyOogler.State.WaitAndTaunt;
    enemyOogler.health.invincible = false;
    enemyOogler.health.SetDrawBurnParticles(true);
    if (!string.IsNullOrEmpty(enemyOogler.TeleportAppearSFX))
      AudioManager.Instance.PlayOneShot(enemyOogler.TeleportAppearSFX);
    enemyOogler.StartCoroutine((IEnumerator) enemyOogler.SpawnClones());
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

  public IEnumerator FightPlayer()
  {
    EnemyOogler enemyOogler = this;
    enemyOogler.MyState = EnemyOogler.State.Attacking;
    enemyOogler.RepathTimer = 0.0f;
    int NumAttacks = enemyOogler.DoubleAttack ? 2 : 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = 0.5f;
    if (enemyOogler.IsClone)
      SignPostDelay = 0.0f;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyOogler.Spine == (UnityEngine.Object) null || enemyOogler.Spine.AnimationState == null || enemyOogler.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        if (enemyOogler.SeperateObject)
          enemyOogler.Seperate(0.5f);
        switch (enemyOogler.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) enemyOogler.TargetObject)
              enemyOogler.state.LookAngle = enemyOogler.state.facingAngle = Utils.GetAngle(enemyOogler.transform.position, enemyOogler.TargetObject.transform.position);
            if ((UnityEngine.Object) enemyOogler.TargetObject != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) enemyOogler.transform.position, (Vector2) enemyOogler.TargetObject.transform.position) < (double) enemyOogler.AttackWithinRange)
            {
              enemyOogler.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              if (!string.IsNullOrEmpty(enemyOogler.AttackClawStartSFX))
                AudioManager.Instance.PlayOneShot(enemyOogler.AttackClawStartSFX);
              enemyOogler.Spine.AnimationState.SetAnimation(0, "attack-charge", false);
              if ((UnityEngine.Object) enemyOogler.damageColliderEvents != (UnityEngine.Object) null)
              {
                enemyOogler.damageColliderEvents.SetActive(false);
                break;
              }
              break;
            }
            enemyOogler.TargetObject = (GameObject) null;
            enemyOogler.StartCoroutine((IEnumerator) enemyOogler.KeepDistanceFromPlayer());
            yield break;
          case StateMachine.State.Moving:
            enemyOogler.TargetObject = (GameObject) null;
            enemyOogler.StartCoroutine((IEnumerator) enemyOogler.WaitForTarget());
            yield break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemyOogler.damageColliderEvents != (UnityEngine.Object) null)
              enemyOogler.damageColliderEvents.SetActive(false);
            enemyOogler.SimpleSpineFlash.FlashWhite(enemyOogler.state.Timer / SignPostDelay);
            enemyOogler.state.Timer += Time.deltaTime * enemyOogler.Spine.timeScale;
            if ((double) enemyOogler.state.Timer >= (double) SignPostDelay - (double) EnemyOogler.signPostParryWindow)
              enemyOogler.canBeParried = true;
            if ((double) enemyOogler.state.Timer >= (double) SignPostDelay)
            {
              enemyOogler.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, enemyOogler.state.LookAngle);
              enemyOogler.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              enemyOogler.speed = AttackSpeed * 0.0166666675f;
              enemyOogler.Spine.AnimationState.SetAnimation(0, "attack-impact", false);
              enemyOogler.canBeParried = true;
              enemyOogler.StartCoroutine((IEnumerator) enemyOogler.EnableDamageCollider(0.0f));
              if (!string.IsNullOrEmpty(enemyOogler.AttackClawSwipeSFX))
                AudioManager.Instance.PlayOneShot(enemyOogler.AttackClawSwipeSFX, enemyOogler.transform.position);
              if (!string.IsNullOrEmpty(enemyOogler.AttackClowSwipeVO))
              {
                AudioManager.Instance.PlayOneShot(enemyOogler.AttackClowSwipeVO, enemyOogler.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime * enemyOogler.Spine.timeScale;
            enemyOogler.speed = AttackSpeed * Time.deltaTime * enemyOogler.Spine.timeScale;
            enemyOogler.SimpleSpineFlash.FlashWhite(false);
            enemyOogler.canBeParried = (double) enemyOogler.state.Timer <= (double) EnemyOogler.attackParryWindow;
            if ((double) (enemyOogler.state.Timer += Time.deltaTime * enemyOogler.Spine.timeScale) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
            {
              if (++AttackCount <= NumAttacks)
              {
                AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
                enemyOogler.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                enemyOogler.Spine.AnimationState.SetAnimation(0, "attack-charge", false);
                SignPostDelay = 0.3f;
                break;
              }
              Loop = false;
              enemyOogler.SimpleSpineFlash.FlashWhite(false);
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    enemyOogler.TargetObject = (GameObject) null;
    enemyOogler.StartCoroutine((IEnumerator) enemyOogler.WaitForTarget());
  }

  public IEnumerator CloneAttack(Health target)
  {
    EnemyOogler enemyOogler = this;
    SkeletonGhost skeletonGhost = enemyOogler.GetComponentInChildren<SkeletonGhost>(true);
    Color baseColor = (Color) skeletonGhost.color;
    float time = 0.0f;
    float anticipationTime = 0.8f;
    float distanceBeforeAnticipation = 7f;
    float attackDistance = 1f;
    Vector3 vector3_1 = target.transform.position - enemyOogler.transform.position;
    Vector3 dir = vector3_1.normalized;
    Vector3 pos = new Vector3();
    enemyOogler.MyState = EnemyOogler.State.Avoiding;
    while (true)
    {
      if (enemyOogler.MyState != EnemyOogler.State.Attacking && (UnityEngine.Object) target != (UnityEngine.Object) null)
      {
        vector3_1 = target.transform.position - enemyOogler.transform.position;
        dir = vector3_1.normalized;
      }
      enemyOogler.transform.position += dir * enemyOogler.maxSpeed * Time.deltaTime;
      Vector3 vector3_2 = pos;
      vector3_1 = new Vector3();
      Vector3 vector3_3 = vector3_1;
      Vector3 toPosition = vector3_2 == vector3_3 ? target.transform.position : enemyOogler.transform.position + dir;
      enemyOogler.state.LookAngle = enemyOogler.state.facingAngle = Utils.GetAngle(enemyOogler.transform.position, toPosition);
      if ((UnityEngine.Object) target != (UnityEngine.Object) null && (double) Vector3.Distance(enemyOogler.transform.position, target.transform.position) < (double) distanceBeforeAnticipation && enemyOogler.MyState == EnemyOogler.State.Avoiding)
      {
        enemyOogler.MyState = EnemyOogler.State.WaitAndTaunt;
        pos = enemyOogler.transform.position + dir * 2f;
        time = 0.0f;
        enemyOogler.Spine.AnimationState.SetAnimation(0, "attack-charge", false);
      }
      else if ((UnityEngine.Object) target != (UnityEngine.Object) null && enemyOogler.MyState == EnemyOogler.State.WaitAndTaunt)
      {
        time += Time.deltaTime * enemyOogler.Spine.timeScale;
        skeletonGhost.color = (Color32) Color.Lerp(baseColor, enemyOogler.ghostAttackFlashColor, time / anticipationTime);
        if ((double) time > (double) anticipationTime || (double) Vector3.Distance(enemyOogler.transform.position, target.transform.position) < (double) attackDistance)
        {
          enemyOogler.damageColliderEvents.SetActive(true);
          enemyOogler.Spine.AnimationState.SetAnimation(0, "attack-impact", false);
          enemyOogler.MyState = EnemyOogler.State.Attacking;
          time = 0.0f;
          if (!string.IsNullOrEmpty(enemyOogler.AttackGhostSwipeSFX))
            AudioManager.Instance.PlayOneShot(enemyOogler.AttackGhostSwipeSFX);
        }
      }
      else if (enemyOogler.MyState == EnemyOogler.State.Attacking)
      {
        time += Time.deltaTime * enemyOogler.Spine.timeScale;
        if ((double) time <= 0.40000000596046448)
        {
          if ((double) time > 0.20000000298023224)
          {
            enemyOogler.damageColliderEvents.SetActive(false);
            if (!enemyOogler.ghostDeathParticles.isPlaying)
              enemyOogler.ghostDeathParticles.Play();
          }
          enemyOogler.Spine.Skeleton.A = Mathf.Lerp(1f, 0.0f, time / 0.4f);
        }
        else
          break;
      }
      yield return (object) new WaitForEndOfFrame();
    }
    skeletonGhost.color = (Color32) baseColor;
    enemyOogler.damageColliderEvents.SetActive(false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.25f, enemyOogler.Spine);
    enemyOogler.ghostDeathParticles.transform.parent = (Transform) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) enemyOogler.gameObject);
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.FollowPlayer)
      return;
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
  }

  public void DeflectClone()
  {
    this.health.team = Health.Team.PlayerTeam;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.CloneAttack(this.cloneParentHealth));
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || !((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) this.health))
      return;
    if (this.EnemyHealth.team != this.health.team)
    {
      this.EnemyHealth.DealDamage(this.EnemyHealth.team == Health.Team.PlayerTeam ? this.Damage : this.Team2Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
    else
    {
      if (this.health.team != Health.Team.PlayerTeam || this.health.isPlayerAlly || this.EnemyHealth.isPlayer)
        return;
      this.EnemyHealth.DealDamage(this.Team2Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
    }
  }

  public IEnumerator EnableDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.damageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.damageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.20000000298023224)
        yield return (object) null;
      this.damageColliderEvents.SetActive(false);
    }
  }

  [CompilerGenerated]
  public void \u003CPreloadAssets\u003Eb__72_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedOoglerAsset = obj;
    obj.Result.CreatePool(3, true);
  }

  public enum State
  {
    WaitAndTaunt,
    Teleporting,
    Attacking,
    Avoiding,
  }
}
