// Decompiled with JetBrains decompiler
// Type: DooglerMageMiniBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
using UnityEngine.Serialization;

#nullable disable
public class DooglerMageMiniBoss : UnitObject
{
  public bool IsNG;
  public int NewPositionDistance = 3;
  public float MaintainTargetDistance = 4.5f;
  public float AttackWithinRange = 4f;
  public bool DoubleAttack = true;
  [FormerlySerializedAs("ChargeAndAttack")]
  public bool DefensiveAttack = true;
  public Vector2 MaxAttackDelayRandomRange = new Vector2(4f, 6f);
  public ColliderEvents damageColliderEvents;
  [SerializeField]
  public bool requireLineOfSite = true;
  public bool CanBeInterrupted = true;
  [SerializeField]
  public Transform[] avalanchePoints;
  public AssetReferenceGameObject LightningStrikePrefab;
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
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string castForwardAnim;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string castAnim;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string castDownAnim;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string castSpiralAnim;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string meleeChargeAnim;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string meleeImpactAnim;
  [Space]
  public ParticleSystem teleportEffect_Out;
  public ParticleSystem teleportEffect_In;
  public ParticleSystem teleportEffect_In_Attack;
  [SerializeField]
  public EnemyOoglerBell ooglerBell;
  [SerializeField]
  public float BellCastTime;
  [SerializeField]
  public float normHealthSpawnBell;
  [SerializeField]
  public float teleportCastDuration;
  public int teleportAttackCount;
  public bool QueuedTeleport;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/death_vo";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/gethit_vo";
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string BigCrossStrikeSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_big_cross_strike";
  [EventRef]
  public string ChasingLineAttackSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_chasingline_start";
  [EventRef]
  public string ChasingLineExplosionSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_chasingline_explosion";
  [EventRef]
  public string IndividualStrikeStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_individualstrikes_start";
  [EventRef]
  public string IndividualStrikeFoleySFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_individualstrikes_foley";
  [EventRef]
  public string MiniCrossStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_mini_cross_start";
  [EventRef]
  public string MiniCrossFoleySFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_mini_cross_foley";
  [EventRef]
  public string LightningSpiralStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_spiral_start";
  [EventRef]
  public string LightningSpiralExplosionSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_lightning_spiral_explosion";
  [EventRef]
  public string MeleeClawTeleportStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_melee_claw_teleport_start";
  [EventRef]
  public string BasicTeleportStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/mv_teleport_start";
  [EventRef]
  public string TeleportAwaySFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/mv_teleport_away";
  [EventRef]
  public string TeleportReappearSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/mv_teleport_reappear";
  [EventRef]
  public string MeleeClawWindupSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_melee_claw_windup";
  [EventRef]
  public string MeleeClawSwipeSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_melee_claw_swipe";
  [EventRef]
  public string MeleeStaffTeleportStartSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_melee_staff_teleport_start";
  [EventRef]
  public string MeleeStaffWindupSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_melee_staff_windup";
  [EventRef]
  public string MeleeStaffSwipeSFX = "event:/dlc/dungeon05/enemy/miniboss_dooglermage/attack_melee_staff_swipe";
  [Space]
  public bool Attacking;
  public float AttackDelayTime = 2f;
  public float DelayAfterFlash1;
  public float DelayAfterFlash2;
  [SerializeField]
  public float LineAttackDelayTime = 4f;
  [SerializeField]
  public int maxChasingAvalanches;
  [SerializeField]
  public float delayAttackChasing;
  [SerializeField]
  public float distanceBetweenAvalanchesChasing;
  [SerializeField]
  public float timeBetweenAvalanchesChasing;
  [SerializeField]
  public float delayAvalancheChasing;
  [SerializeField]
  public float speedAvalancheChasing;
  [SerializeField]
  public int chasingAvalancheFlashAfter;
  public int amountSingleLightningStrikes = 8;
  public float timeBetweenIndividualStrikes = 0.2f;
  public float delayIndividualAttacksStart = 0.1f;
  [SerializeField]
  public float startDelayAttackGiant;
  [SerializeField]
  public int amountBigLightningStrikes = 1;
  public float BigLightningAdjacentStrikeDistance = 1f;
  public float BigLightningIndividualStrikesDelay = 0.2f;
  public float BigLightningTimeBetweenStrikes = 1f;
  [SerializeField]
  public int giantAvalancheFlashAfter;
  [SerializeField]
  public int numAvalanchesSpiral;
  [SerializeField]
  public float delayAttackSpiral;
  [SerializeField]
  public float timeBetweenAvalanchesSpiral;
  [SerializeField]
  public float angleBetweenAvalanchesSpiral;
  [SerializeField]
  public float distanceBetweenAvalanchesSpiral;
  [SerializeField]
  public float delayAvalancheSpiral;
  [SerializeField]
  public float speedAvalancheSpiral;
  [SerializeField]
  public int spiralAvalancheFlashAfter;
  public float QuadLightningStartDelay = 1f;
  public int QuadLightningAttackCount = 8;
  public float QuadLightningSignpostTime = 0.5f;
  public float QuadLightningTimeBetweenBeams = 1f;
  public string LightningSFX = "event:/explosion/explosion";
  [SerializeField]
  [Range(0.0f, 1f)]
  public float phase1HP = 0.6f;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float phase2HP = 0.3f;
  [Space]
  public float AttackTimer;
  public float LineAttackTimer;
  public float CircleAttackTimer;
  public GameObject TargetObject;
  public float AttackDelay;
  public bool canBeParried;
  public static float signPostParryWindow = 0.2f;
  public static float attackParryWindow = 0.15f;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public bool \u003CFollowPlayer\u003Ek__BackingField;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public GameObject loadedLightningImpact;
  public List<DooglerMageMiniBoss.RangedAttacks> rangedAttackOrder = new List<DooglerMageMiniBoss.RangedAttacks>();
  public DooglerMageMiniBoss.RangedAttacks lastRangedAttack;
  public List<DooglerMageMiniBoss.MeleeAttacks> meleeAttackOrder = new List<DooglerMageMiniBoss.MeleeAttacks>();
  public DooglerMageMiniBoss.MeleeAttacks lastMeleeAttack;
  public int pointTrys;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public DooglerMageMiniBoss.State MyState;
  public float MaxAttackDelay;
  public Coroutine keepDistanceCoroutine;
  public float TeleportMinDistance = 7f;
  public float TeleportMaxDistance = 10f;
  public Health EnemyHealth;
  [SerializeField]
  public DooglerMageMiniBoss.WaveSpawnable firstWave;
  [SerializeField]
  public DooglerMageMiniBoss.WaveSpawnable secondWave;
  [SerializeField]
  public DooglerMageMiniBoss.WaveSpawnable thirdWave;
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
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

  public bool CanSpawnBell
  {
    get
    {
      return !this.TargetHasBells(this.TargetObject) && (double) this.health.HP / (double) this.health.totalHP < (double) this.normHealthSpawnBell;
    }
  }

  public int Phase
  {
    get
    {
      if ((double) this.health.HP / (double) this.health.totalHP < (double) this.phase2HP)
        return 2;
      return (double) this.health.HP / (double) this.health.totalHP >= (double) this.phase1HP ? 0 : 1;
    }
  }

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null)
      this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    this.LoadAssets();
  }

  public void Start() => this.SpawnEnemies(this.firstWave);

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.LightningStrikePrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      DooglerMageMiniBoss.loadedAddressableAssets.Add(obj);
      this.loadedLightningImpact = obj.Result;
      this.loadedLightningImpact.CreatePool(Mathf.Max(this.maxChasingAvalanches, this.numAvalanchesSpiral), true);
    });
  }

  public override void OnDestroy()
  {
    if (DooglerMageMiniBoss.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in DooglerMageMiniBoss.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      DooglerMageMiniBoss.loadedAddressableAssets.Clear();
    }
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
    this.health.OnDieEarly += new Health.DieAction(this.UnlockSkin);
  }

  public void UnlockSkin(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (DataManager.GetFollowerSkinUnlocked("Boss Dog 2"))
      return;
    DataManager.SetFollowerSkinUnlocked("Boss Dog 2");
  }

  public void GiveSkin(GameObject Attacker)
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    FollowerSkinCustomTarget.Create(this.Spine.transform.position, Attacker.transform.position, 2f, "Boss Dog 2", (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().AddPlayerToCamera();
    }));
  }

  public override void OnDisable()
  {
    this.health.OnDieEarly -= new Health.DieAction(this.UnlockSkin);
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    MMVibrate.StopRumbleForAllPlayers();
    this.DisableForces = false;
  }

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
    MMVibrate.StopRumbleForAllPlayers();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null)
      {
        this.spawnedEnemies[index].health.enabled = true;
        this.spawnedEnemies[index].health.DealDamage(this.spawnedEnemies[index].health.totalHP, this.gameObject, this.spawnedEnemies[index].transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
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
    if (this.MyState != DooglerMageMiniBoss.State.Attacking)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.DisableForces = false;
    }
    if (this.teleportAttackCount > 0)
      this.QueuedTeleport = true;
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    this.SimpleSpineFlash.FlashFillRed();
    if (!this.secondWave.activated && (double) this.health.CurrentHP / (double) this.health.totalHP < (double) this.secondWave.normHealth)
      this.SpawnEnemies(this.secondWave);
    if (this.thirdWave.activated || (double) this.health.CurrentHP / (double) this.health.totalHP >= (double) this.thirdWave.normHealth)
      return;
    this.SpawnEnemies(this.thirdWave);
  }

  public Vector3 GetDistantRandomPoint()
  {
    Vector3 zero = Vector3.zero;
    while (!((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null))
    {
      Vector3 position = this.TargetObject.transform.position;
      Vector3 positionInIsland = BiomeGenerator.GetRandomPositionInIsland();
      if ((double) Vector3.Distance(positionInIsland, position) < (double) this.MaintainTargetDistance)
      {
        if (this.pointTrys >= 10)
        {
          this.pointTrys = 0;
          return positionInIsland;
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

  public IEnumerator DropAvalanches()
  {
    Vector3[] points = new Vector3[this.avalanchePoints.Length];
    for (int index = 0; index < points.Length; ++index)
      points[index] = this.avalanchePoints[index].position;
    yield return (object) new WaitForSeconds(0.2f);
    for (int index = 0; index < points.Length; ++index)
    {
      this.DropAvalanche(points[index]);
      yield return (object) new WaitForSeconds(0.2f);
    }
  }

  public void DropAvalanche(Vector3 position)
  {
    GameObject lightning = ObjectPool.Spawn(this.loadedLightningImpact, position, Quaternion.identity);
    lightning.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(this.health, position, (System.Action) (() => ObjectPool.Recycle(lightning)), true);
  }

  public IEnumerator WaitForTarget()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.Spine.Initialize(false);
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) dooglerMageMiniBoss.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = dooglerMageMiniBoss.GetClosestTarget(dooglerMageMiniBoss.health.team == Health.Team.PlayerTeam);
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        dooglerMageMiniBoss.TargetObject = closestTarget.gameObject;
        dooglerMageMiniBoss.requireLineOfSite = false;
        dooglerMageMiniBoss.VisionRange = int.MaxValue;
        if (dooglerMageMiniBoss.CanSpawnBell)
        {
          dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.CastOoglerBell());
          yield break;
        }
      }
      dooglerMageMiniBoss.RepathTimer -= Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
      if ((double) dooglerMageMiniBoss.RepathTimer <= 0.0)
      {
        if (dooglerMageMiniBoss.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (dooglerMageMiniBoss.Spine.AnimationName != "run")
            dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (dooglerMageMiniBoss.Spine.AnimationName != dooglerMageMiniBoss.idleAnim)
          dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.idleAnim, true);
        if (!dooglerMageMiniBoss.FollowPlayer)
          dooglerMageMiniBoss.TargetPosition = dooglerMageMiniBoss.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        dooglerMageMiniBoss.FindPath(dooglerMageMiniBoss.TargetPosition);
        dooglerMageMiniBoss.state.LookAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetPosition);
        dooglerMageMiniBoss.Spine.skeleton.ScaleX = (double) dooglerMageMiniBoss.state.LookAngle <= 90.0 || (double) dooglerMageMiniBoss.state.LookAngle >= 270.0 ? -1f : 1f;
      }
      yield return (object) null;
    }
    if (dooglerMageMiniBoss.QueuedTeleport)
    {
      dooglerMageMiniBoss.StopAllCoroutines();
      dooglerMageMiniBoss.speed = 0.0f;
      dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.CastTeleportMechanicRoutine(dooglerMageMiniBoss.GetDistantRandomPoint()));
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) dooglerMageMiniBoss.TargetObject == (UnityEngine.Object) null)
      {
        dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(dooglerMageMiniBoss.TargetObject.transform.position, dooglerMageMiniBoss.transform.position);
      if ((double) a <= (double) dooglerMageMiniBoss.VisionRange)
      {
        if (!dooglerMageMiniBoss.requireLineOfSite || dooglerMageMiniBoss.CheckLineOfSightOnTarget(dooglerMageMiniBoss.TargetObject, dooglerMageMiniBoss.TargetObject.transform.position, Mathf.Min(a, (float) dooglerMageMiniBoss.VisionRange)))
          InRange = true;
        else
          dooglerMageMiniBoss.LookAtTarget();
      }
      yield return (object) null;
    }
    if (dooglerMageMiniBoss.keepDistanceCoroutine != null)
      dooglerMageMiniBoss.StopCoroutine(dooglerMageMiniBoss.keepDistanceCoroutine);
    dooglerMageMiniBoss.keepDistanceCoroutine = dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.KeepDistanceFromPlayer());
  }

  public bool TargetHasBells(GameObject target) => BellMechanics.TargetHasOoglerBell(target);

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
  }

  public IEnumerator CastOoglerBell()
  {
    DooglerMageMiniBoss caster = this;
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

  public IEnumerator KeepDistanceFromPlayer()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.MyState = DooglerMageMiniBoss.State.WaitAndTaunt;
    dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    dooglerMageMiniBoss.MaxAttackDelay = UnityEngine.Random.Range(dooglerMageMiniBoss.MaxAttackDelayRandomRange.x, dooglerMageMiniBoss.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) dooglerMageMiniBoss.TargetObject == (UnityEngine.Object) null)
      {
        dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.WaitForTarget());
        break;
      }
      if (dooglerMageMiniBoss.CanSpawnBell)
      {
        dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.CastOoglerBell());
        break;
      }
      if ((UnityEngine.Object) dooglerMageMiniBoss.damageColliderEvents != (UnityEngine.Object) null)
        dooglerMageMiniBoss.damageColliderEvents.SetActive(false);
      dooglerMageMiniBoss.AttackDelay -= Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
      dooglerMageMiniBoss.MaxAttackDelay -= Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
      if (dooglerMageMiniBoss.MyState == DooglerMageMiniBoss.State.WaitAndTaunt)
      {
        if ((UnityEngine.Object) dooglerMageMiniBoss.TargetObject == (UnityEngine.Object) PlayerFarming.Instance.gameObject && dooglerMageMiniBoss.health.IsCharmed && (UnityEngine.Object) dooglerMageMiniBoss.GetClosestTarget() != (UnityEngine.Object) null)
          dooglerMageMiniBoss.TargetObject = dooglerMageMiniBoss.GetClosestTarget().gameObject;
        dooglerMageMiniBoss.state.LookAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetObject.transform.position);
        dooglerMageMiniBoss.Spine.skeleton.ScaleX = (double) dooglerMageMiniBoss.state.LookAngle <= 90.0 || (double) dooglerMageMiniBoss.state.LookAngle >= 270.0 ? -1f : 1f;
        switch (dooglerMageMiniBoss.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
          case StateMachine.State.Moving:
            if ((double) (dooglerMageMiniBoss.RepathTimer -= Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < 0.0 && (bool) (UnityEngine.Object) dooglerMageMiniBoss.TargetObject && (double) dooglerMageMiniBoss.MaxAttackDelay < 0.0)
            {
              if ((double) Vector3.Distance(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetObject.transform.position) < (double) dooglerMageMiniBoss.AttackWithinRange)
              {
                if ((bool) (UnityEngine.Object) dooglerMageMiniBoss.TargetObject)
                {
                  dooglerMageMiniBoss.health.invincible = false;
                  dooglerMageMiniBoss.DisableForces = false;
                  switch (dooglerMageMiniBoss.GetNextMeleeAttack())
                  {
                    case DooglerMageMiniBoss.MeleeAttacks.Swipe:
                      yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.CloseRangeAttackPlayer());
                      break;
                    case DooglerMageMiniBoss.MeleeAttacks.QuadLightning:
                      yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.QuadLightningAttack());
                      break;
                    case DooglerMageMiniBoss.MeleeAttacks.TeleportAway:
                      yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.CastTeleportMechanicRoutine(dooglerMageMiniBoss.GetTeleportFleePosition()));
                      break;
                  }
                  dooglerMageMiniBoss.QueuedTeleport = true;
                  dooglerMageMiniBoss.teleportAttackCount = 0;
                  break;
                }
                break;
              }
              if ((double) Vector3.Distance(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetObject.transform.position) > (double) dooglerMageMiniBoss.AttackWithinRange)
              {
                dooglerMageMiniBoss.Angle = (float) (((double) Utils.GetAngle(dooglerMageMiniBoss.TargetObject.transform.position, dooglerMageMiniBoss.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                dooglerMageMiniBoss.TargetPosition = dooglerMageMiniBoss.TargetObject.transform.position + new Vector3((dooglerMageMiniBoss.MaintainTargetDistance + (float) UnityEngine.Random.Range(0, 3)) * Mathf.Cos(dooglerMageMiniBoss.Angle), (dooglerMageMiniBoss.MaintainTargetDistance + (float) UnityEngine.Random.Range(0, 3)) * Mathf.Sin(dooglerMageMiniBoss.Angle));
                dooglerMageMiniBoss.FindPath(dooglerMageMiniBoss.TargetPosition);
                switch (dooglerMageMiniBoss.GetNextRangedAttack())
                {
                  case DooglerMageMiniBoss.RangedAttacks.ChasingLine:
                    yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.ChasingLightningAttack());
                    break;
                  case DooglerMageMiniBoss.RangedAttacks.IndividualStrikes:
                    yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.IndividualLightningStrikes());
                    break;
                  case DooglerMageMiniBoss.RangedAttacks.RingLightning:
                    yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.BigLightningAttack());
                    break;
                  case DooglerMageMiniBoss.RangedAttacks.Spiral:
                    yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.SpiralAvalanchesAttack());
                    break;
                }
                if (dooglerMageMiniBoss.Phase == 1)
                  yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.Flash1Attack());
                else if (dooglerMageMiniBoss.Phase == 2)
                  yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.Flash2Attack());
                dooglerMageMiniBoss.TargetObject = (GameObject) null;
                ++dooglerMageMiniBoss.teleportAttackCount;
                if (dooglerMageMiniBoss.teleportAttackCount >= UnityEngine.Random.Range(2, 4))
                {
                  dooglerMageMiniBoss.QueuedTeleport = true;
                  dooglerMageMiniBoss.teleportAttackCount = 0;
                  break;
                }
                break;
              }
              break;
            }
            break;
          default:
            if ((double) (dooglerMageMiniBoss.RepathTimer += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) > 2.0)
            {
              dooglerMageMiniBoss.RepathTimer = 0.0f;
              dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
              break;
            }
            break;
        }
      }
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

  public IEnumerator CastTeleportMechanicRoutine(Vector3 destination, bool animated = true)
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.QueuedTeleport = false;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Teleporting;
    dooglerMageMiniBoss.UsePathing = false;
    dooglerMageMiniBoss.SeperateObject = false;
    dooglerMageMiniBoss.MyState = DooglerMageMiniBoss.State.Teleporting;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.teleportEffect_In.Play();
    if (animated)
      dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.teleportAnim, false);
    if (!string.IsNullOrEmpty(dooglerMageMiniBoss.BasicTeleportStartSFX))
      AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.BasicTeleportStartSFX);
    dooglerMageMiniBoss.state.facingAngle = dooglerMageMiniBoss.state.LookAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, destination);
    dooglerMageMiniBoss.Spine.skeleton.ScaleX = (double) dooglerMageMiniBoss.state.LookAngle <= 90.0 || (double) dooglerMageMiniBoss.state.LookAngle >= 270.0 ? -1f : 1f;
    float teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.teleportCastDuration)
    {
      if ((double) teleportTime >= 0.30000001192092896 && !dooglerMageMiniBoss.health.invincible)
        dooglerMageMiniBoss.health.invincible = true;
      yield return (object) null;
    }
    dooglerMageMiniBoss.transform.position = destination;
    dooglerMageMiniBoss.teleportEffect_Out.Play();
    if (!string.IsNullOrEmpty(dooglerMageMiniBoss.TeleportReappearSFX))
      AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.TeleportReappearSFX);
    if (animated)
    {
      dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.teleportOutAnim, false);
      dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, dooglerMageMiniBoss.idleAnim, true, 0.0f);
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    dooglerMageMiniBoss.UsePathing = true;
    dooglerMageMiniBoss.DisableForces = true;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.rb.velocity = (Vector2) Vector3.zero;
    dooglerMageMiniBoss.vx = dooglerMageMiniBoss.vy = 0.0f;
    dooglerMageMiniBoss.speed = 0.0f;
    dooglerMageMiniBoss.RepathTimer = 0.5f;
    dooglerMageMiniBoss.SeperateObject = true;
    dooglerMageMiniBoss.teleportAttackCount = 0;
    dooglerMageMiniBoss.MyState = DooglerMageMiniBoss.State.WaitAndTaunt;
    dooglerMageMiniBoss.health.invincible = false;
    dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.WaitForTarget());
  }

  public Vector3 GetTeleportFleePosition()
  {
    Vector3 teleportFleePosition = this.transform.position;
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = UnityEngine.Random.Range(this.TeleportMinDistance, this.TeleportMaxDistance);
      teleportFleePosition = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.TargetObject.transform.position, this.CircleCastRadius, (Vector2) Vector3.Normalize(teleportFleePosition - this.TargetObject.transform.position), distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null && (double) Vector3.Distance(this.TargetObject.transform.position, (Vector3) raycastHit2D.centroid) > (double) this.TeleportMinDistance)
        return (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.TargetObject.transform.position - teleportFleePosition) * this.CircleCastOffset;
    }
    return teleportFleePosition;
  }

  public IEnumerator CastFlashTeleportMechanicRoutine(Vector3 destination)
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.QueuedTeleport = false;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Teleporting;
    dooglerMageMiniBoss.UsePathing = false;
    dooglerMageMiniBoss.SeperateObject = false;
    dooglerMageMiniBoss.MyState = DooglerMageMiniBoss.State.Teleporting;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.teleportEffect_In_Attack.Play();
    if (!string.IsNullOrEmpty(dooglerMageMiniBoss.TeleportAwaySFX))
      AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.TeleportAwaySFX);
    dooglerMageMiniBoss.state.facingAngle = dooglerMageMiniBoss.state.LookAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, destination);
    dooglerMageMiniBoss.Spine.skeleton.ScaleX = (double) dooglerMageMiniBoss.state.LookAngle <= 90.0 || (double) dooglerMageMiniBoss.state.LookAngle >= 270.0 ? -1f : 1f;
    float teleportTime = 0.0f;
    while ((double) (teleportTime += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.teleportCastDuration)
      yield return (object) null;
    if ((bool) (UnityEngine.Object) dooglerMageMiniBoss.TargetObject)
      destination = dooglerMageMiniBoss.TargetObject.transform.position + Vector3.up * 1f;
    Vector3 closestPoint;
    if (!BiomeGenerator.PointWithinIsland(destination, out closestPoint))
      destination = closestPoint;
    dooglerMageMiniBoss.transform.position = destination;
    dooglerMageMiniBoss.state.facingAngle = dooglerMageMiniBoss.state.LookAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, destination);
    dooglerMageMiniBoss.Spine.skeleton.ScaleX = (double) dooglerMageMiniBoss.state.LookAngle <= 90.0 || (double) dooglerMageMiniBoss.state.LookAngle >= 270.0 ? -1f : 1f;
    dooglerMageMiniBoss.teleportEffect_Out.Play();
    if (!string.IsNullOrEmpty(dooglerMageMiniBoss.TeleportReappearSFX))
      AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.TeleportReappearSFX);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    dooglerMageMiniBoss.UsePathing = true;
    dooglerMageMiniBoss.DisableForces = true;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.rb.velocity = (Vector2) Vector3.zero;
    dooglerMageMiniBoss.vx = dooglerMageMiniBoss.vy = 0.0f;
    dooglerMageMiniBoss.speed = 0.0f;
    dooglerMageMiniBoss.RepathTimer = 0.5f;
    dooglerMageMiniBoss.SeperateObject = true;
    dooglerMageMiniBoss.teleportAttackCount = 0;
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

  public IEnumerator CloseRangeAttackPlayer()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.MyState = DooglerMageMiniBoss.State.Attacking;
    Vector2 position = (Vector2) dooglerMageMiniBoss.transform.position;
    dooglerMageMiniBoss.RepathTimer = 0.0f;
    int NumAttacks = dooglerMageMiniBoss.DoubleAttack ? 2 : 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = 0.5f;
    while (Loop)
    {
      if ((UnityEngine.Object) dooglerMageMiniBoss.Spine == (UnityEngine.Object) null || dooglerMageMiniBoss.Spine.AnimationState == null || dooglerMageMiniBoss.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        dooglerMageMiniBoss.Seperate(0.5f);
        switch (dooglerMageMiniBoss.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) dooglerMageMiniBoss.TargetObject)
            {
              dooglerMageMiniBoss.state.LookAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetObject.transform.position);
              dooglerMageMiniBoss.Spine.skeleton.ScaleX = (double) dooglerMageMiniBoss.state.LookAngle <= 90.0 || (double) dooglerMageMiniBoss.state.LookAngle >= 270.0 ? -1f : 1f;
              dooglerMageMiniBoss.state.LookAngle = dooglerMageMiniBoss.state.facingAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetObject.transform.position);
            }
            if ((UnityEngine.Object) dooglerMageMiniBoss.TargetObject != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) dooglerMageMiniBoss.transform.position, (Vector2) dooglerMageMiniBoss.TargetObject.transform.position) < (double) dooglerMageMiniBoss.AttackWithinRange)
            {
              dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.meleeChargeAnim, false);
              if (!string.IsNullOrEmpty(dooglerMageMiniBoss.MeleeClawWindupSFX))
                AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.MeleeClawWindupSFX);
              if ((UnityEngine.Object) dooglerMageMiniBoss.damageColliderEvents != (UnityEngine.Object) null)
              {
                dooglerMageMiniBoss.damageColliderEvents.SetActive(false);
                break;
              }
              break;
            }
            dooglerMageMiniBoss.TargetObject = (GameObject) null;
            if (dooglerMageMiniBoss.keepDistanceCoroutine != null)
              dooglerMageMiniBoss.StopCoroutine(dooglerMageMiniBoss.keepDistanceCoroutine);
            dooglerMageMiniBoss.keepDistanceCoroutine = dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.KeepDistanceFromPlayer());
            yield break;
          case StateMachine.State.Moving:
            dooglerMageMiniBoss.TargetObject = (GameObject) null;
            dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.WaitForTarget());
            yield break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) dooglerMageMiniBoss.damageColliderEvents != (UnityEngine.Object) null)
              dooglerMageMiniBoss.damageColliderEvents.SetActive(false);
            dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(dooglerMageMiniBoss.state.Timer / SignPostDelay);
            dooglerMageMiniBoss.state.Timer += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
            if ((double) dooglerMageMiniBoss.state.Timer >= (double) SignPostDelay - (double) DooglerMageMiniBoss.signPostParryWindow)
              dooglerMageMiniBoss.canBeParried = true;
            if ((double) dooglerMageMiniBoss.state.Timer >= (double) SignPostDelay)
            {
              dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, dooglerMageMiniBoss.state.LookAngle);
              dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              dooglerMageMiniBoss.speed = AttackSpeed * 0.0166666675f;
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.meleeImpactAnim, false);
              dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, "idle", false, 0.0f);
              dooglerMageMiniBoss.canBeParried = true;
              dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.EnableDamageCollider(0.0f));
              if (!string.IsNullOrEmpty(dooglerMageMiniBoss.MeleeClawSwipeSFX))
                AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.MeleeClawSwipeSFX, dooglerMageMiniBoss.transform.position);
              if (!string.IsNullOrEmpty(dooglerMageMiniBoss.AttackVO))
              {
                AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.AttackVO, dooglerMageMiniBoss.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime * dooglerMageMiniBoss.Spine.timeScale;
            dooglerMageMiniBoss.speed = AttackSpeed * Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
            dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
            dooglerMageMiniBoss.canBeParried = (double) dooglerMageMiniBoss.state.Timer <= (double) DooglerMageMiniBoss.attackParryWindow;
            if ((double) (dooglerMageMiniBoss.state.Timer += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
            {
              if (++AttackCount <= NumAttacks)
              {
                AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
                dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, "attack-charge", false);
                SignPostDelay = 0.3f;
                break;
              }
              Loop = false;
              dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    dooglerMageMiniBoss.TargetObject = (GameObject) null;
    dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.WaitForTarget());
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.FollowPlayer)
      return;
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
  }

  public DooglerMageMiniBoss.eDooglerMageAttack ChooseNextFarRangeAttack()
  {
    int num = UnityEngine.Random.Range(0, 100);
    return num < 40 ? DooglerMageMiniBoss.eDooglerMageAttack.lineAvalanches : (num < 75 ? DooglerMageMiniBoss.eDooglerMageAttack.chaseAvalances : DooglerMageMiniBoss.eDooglerMageAttack.cicleAvalanches);
  }

  public IEnumerator ChasingLightningAttack()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.Attacking = true;
    dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.castForwardAnim, false);
    dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, dooglerMageMiniBoss.idleAnim, true, 0.0f);
    if (!string.IsNullOrEmpty(dooglerMageMiniBoss.ChasingLineAttackSFX))
      AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.ChasingLineAttackSFX);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.delayAttackChasing)
      yield return (object) null;
    Vector3 originPosition = dooglerMageMiniBoss.transform.position;
    int SpawnCount = 0;
    bool ngFlashPerformed = false;
    Coroutine ngFlash = (Coroutine) null;
    while (SpawnCount < dooglerMageMiniBoss.maxChasingAvalanches)
    {
      Vector3 vector3 = originPosition + (dooglerMageMiniBoss.TargetObject.transform.position - originPosition).normalized * dooglerMageMiniBoss.distanceBetweenAvalanchesChasing;
      originPosition = vector3;
      if (!BiomeGenerator.PointWithinIsland(vector3, out Vector3 _))
      {
        ++SpawnCount;
      }
      else
      {
        GameObject lightning = ObjectPool.Spawn(dooglerMageMiniBoss.loadedLightningImpact, vector3, Quaternion.identity);
        lightning.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(dooglerMageMiniBoss.health, vector3, (System.Action) (() => ObjectPool.Recycle(lightning)), true, customSFX: dooglerMageMiniBoss.ChasingLineExplosionSFX);
        ++SpawnCount;
        time = 0.0f;
        while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.timeBetweenAvalanchesChasing)
          yield return (object) null;
        if (dooglerMageMiniBoss.IsNG && SpawnCount >= dooglerMageMiniBoss.chasingAvalancheFlashAfter && !ngFlashPerformed)
        {
          dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
          ngFlash = dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.Flash1Attack());
          ngFlashPerformed = true;
        }
      }
    }
    if (ngFlash != null)
      yield return (object) ngFlash;
    dooglerMageMiniBoss.LineAttackTimer = dooglerMageMiniBoss.LineAttackDelayTime;
    dooglerMageMiniBoss.AttackTimer = dooglerMageMiniBoss.AttackDelayTime;
    dooglerMageMiniBoss.Attacking = false;
    dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator IndividualLightningStrikes()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.Attacking = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.delayIndividualAttacksStart)
      yield return (object) null;
    if (!string.IsNullOrEmpty(dooglerMageMiniBoss.IndividualStrikeStartSFX))
      AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.IndividualStrikeStartSFX);
    for (int i = 0; i < dooglerMageMiniBoss.amountSingleLightningStrikes; ++i)
    {
      dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.castDownAnim, false);
      dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, dooglerMageMiniBoss.idleAnim, true, 0.0f);
      Vector3 position = dooglerMageMiniBoss.TargetObject.transform.position;
      GameObject lightning = ObjectPool.Spawn(dooglerMageMiniBoss.loadedLightningImpact, position, Quaternion.identity);
      lightning.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(dooglerMageMiniBoss.health, position, (System.Action) (() => ObjectPool.Recycle(lightning)), true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.timeBetweenIndividualStrikes)
        yield return (object) null;
    }
    dooglerMageMiniBoss.LineAttackTimer = dooglerMageMiniBoss.LineAttackDelayTime;
    dooglerMageMiniBoss.AttackTimer = dooglerMageMiniBoss.AttackDelayTime;
    dooglerMageMiniBoss.Attacking = false;
    dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void DoStaffDownSpineEvent()
  {
    if (string.IsNullOrEmpty(this.IndividualStrikeFoleySFX))
      return;
    AudioManager.Instance.PlayOneShot(this.IndividualStrikeFoleySFX);
  }

  public IEnumerator BigLightningAttack()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.Attacking = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.startDelayAttackGiant)
      yield return (object) null;
    Coroutine ngFlash = (Coroutine) null;
    if (!string.IsNullOrEmpty(dooglerMageMiniBoss.MiniCrossStartSFX))
      AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.MiniCrossStartSFX);
    for (int i = 0; i < dooglerMageMiniBoss.amountBigLightningStrikes; ++i)
    {
      dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.castDownAnim, false);
      dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, dooglerMageMiniBoss.idleAnim, true, 0.0f);
      if (!string.IsNullOrEmpty(dooglerMageMiniBoss.MiniCrossFoleySFX))
        AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.MiniCrossFoleySFX);
      Vector3 position = dooglerMageMiniBoss.TargetObject.transform.position;
      GameObject[] adjacentStrikeObjects = new GameObject[4];
      LightningStrikeAttack[] adjacentStrikes = new LightningStrikeAttack[4];
      GameObject lightningCentreObj = ObjectPool.Spawn(dooglerMageMiniBoss.loadedLightningImpact, position, Quaternion.identity);
      LightningStrikeAttack component = lightningCentreObj.GetComponent<LightningStrikeAttack>();
      for (int index = 0; index < 4; ++index)
      {
        adjacentStrikeObjects[index] = ObjectPool.Spawn(dooglerMageMiniBoss.loadedLightningImpact, position, Quaternion.identity);
        adjacentStrikes[index] = adjacentStrikeObjects[index].GetComponent<LightningStrikeAttack>();
      }
      Vector3[] adjacentPositions = new Vector3[4]
      {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right
      };
      component.TriggerLightningStrike(dooglerMageMiniBoss.health, position, (System.Action) (() => ObjectPool.Recycle(lightningCentreObj)), true, indicatorSize: LightningStrikeAttack.IndicatorSize.Large);
      yield return (object) new WaitForSeconds(dooglerMageMiniBoss.BigLightningIndividualStrikesDelay);
      adjacentStrikes[0].TriggerLightningStrike(dooglerMageMiniBoss.health, position + adjacentPositions[0] * dooglerMageMiniBoss.BigLightningAdjacentStrikeDistance, (System.Action) (() => ObjectPool.Recycle(adjacentStrikeObjects[0])), false, screenShakeMult: 0.0f, playHapticsOnExplosion: false);
      yield return (object) new WaitForSeconds(dooglerMageMiniBoss.BigLightningIndividualStrikesDelay);
      adjacentStrikes[1].TriggerLightningStrike(dooglerMageMiniBoss.health, position + adjacentPositions[1] * dooglerMageMiniBoss.BigLightningAdjacentStrikeDistance, (System.Action) (() => ObjectPool.Recycle(adjacentStrikeObjects[1])), false, screenShakeMult: 0.0f, playHapticsOnExplosion: false);
      yield return (object) new WaitForSeconds(dooglerMageMiniBoss.BigLightningIndividualStrikesDelay);
      adjacentStrikes[2].TriggerLightningStrike(dooglerMageMiniBoss.health, position + adjacentPositions[2] * dooglerMageMiniBoss.BigLightningAdjacentStrikeDistance, (System.Action) (() => ObjectPool.Recycle(adjacentStrikeObjects[2])), false, screenShakeMult: 0.0f, playHapticsOnExplosion: false);
      yield return (object) new WaitForSeconds(dooglerMageMiniBoss.BigLightningIndividualStrikesDelay);
      adjacentStrikes[3].TriggerLightningStrike(dooglerMageMiniBoss.health, position + adjacentPositions[3] * dooglerMageMiniBoss.BigLightningAdjacentStrikeDistance, (System.Action) (() => ObjectPool.Recycle(adjacentStrikeObjects[3])), false, screenShakeMult: 0.0f, playHapticsOnExplosion: false);
      yield return (object) new WaitForSeconds(dooglerMageMiniBoss.BigLightningIndividualStrikesDelay);
      if (i < dooglerMageMiniBoss.amountBigLightningStrikes - 1)
        yield return (object) new WaitForSeconds(dooglerMageMiniBoss.BigLightningTimeBetweenStrikes);
      position = new Vector3();
      adjacentStrikes = (LightningStrikeAttack[]) null;
      adjacentPositions = (Vector3[]) null;
    }
    if (dooglerMageMiniBoss.IsNG)
    {
      dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
      ngFlash = dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.Flash1Attack());
    }
    if (ngFlash != null)
      yield return (object) ngFlash;
    dooglerMageMiniBoss.LineAttackTimer = dooglerMageMiniBoss.LineAttackDelayTime;
    dooglerMageMiniBoss.AttackTimer = dooglerMageMiniBoss.AttackDelayTime;
    dooglerMageMiniBoss.Attacking = false;
    dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator QuadLightningAttack()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    int beamThickness = 5;
    float beamLength = 12f;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.QuadLightningStartDelay)
      yield return (object) null;
    for (int i = 0; i < dooglerMageMiniBoss.QuadLightningAttackCount; ++i)
    {
      if (!string.IsNullOrEmpty(dooglerMageMiniBoss.BigCrossStrikeSFX))
        AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.BigCrossStrikeSFX, dooglerMageMiniBoss.transform.position);
      dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.castDownAnim, false);
      dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, dooglerMageMiniBoss.idleAnim, true, 0.0f);
      if ((UnityEngine.Object) dooglerMageMiniBoss.TargetObject == (UnityEngine.Object) null)
        dooglerMageMiniBoss.TargetObject = dooglerMageMiniBoss.GetClosestTarget().gameObject;
      Vector3 normalized = (dooglerMageMiniBoss.TargetObject.transform.position - dooglerMageMiniBoss.transform.position).normalized;
      Vector3[] vector3Array = new Vector3[4];
      for (int index = 0; index < vector3Array.Length; ++index)
        vector3Array[index] = (Vector3) dooglerMageMiniBoss.RotatePoint((Vector2) normalized, (float) ((double) index * 90.0 * (Math.PI / 180.0)));
      MMVibrate.RumbleContinuousForAllPlayers(1f, 1.2f);
      for (int index = 0; index < beamThickness; ++index)
      {
        dooglerMageMiniBoss.CreateBeam(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.transform.position + vector3Array[0] * beamLength);
        dooglerMageMiniBoss.CreateBeam(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.transform.position + vector3Array[1] * beamLength);
        dooglerMageMiniBoss.CreateBeam(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.transform.position + vector3Array[2] * beamLength);
        dooglerMageMiniBoss.CreateBeam(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.transform.position + vector3Array[3] * beamLength);
      }
      time = 0.0f;
      while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.QuadLightningTimeBetweenBeams)
        yield return (object) null;
      MMVibrate.StopRumbleForAllPlayers();
      time = 0.0f;
      while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.QuadLightningSignpostTime)
        yield return (object) null;
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.QuadLightningTimeBetweenBeams)
      yield return (object) null;
  }

  public Vector2 RotatePoint(Vector2 point, float angle)
  {
    return new Vector2((float) ((double) point.x * (double) Mathf.Cos(angle) - (double) point.y * (double) Mathf.Sin(angle)), (float) ((double) point.x * (double) Mathf.Sin(angle) + (double) point.y * (double) Mathf.Cos(angle)));
  }

  public void CreateBeam(Vector3 from, Vector3 target)
  {
    int num = (int) Vector3.Distance(from, target) + 2;
    List<Vector3> vector3List = new List<Vector3>();
    vector3List.Add(from);
    for (int index = 1; index < num + 1; ++index)
    {
      float t = (float) index / (float) num;
      Vector3 vector3 = Vector3.Lerp(from, target, t) + (Vector3) UnityEngine.Random.insideUnitCircle * 0.5f;
      vector3List.Add(vector3);
    }
    vector3List.Add(target);
    ArrowLightningBeam.CreateBeam(vector3List.ToArray(), true, 0.75f, 0.5f, Health.Team.Team2, (Transform) null, signpostDuration: this.QuadLightningSignpostTime);
  }

  public IEnumerator SpiralAvalanchesAttack()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.ClearPaths();
    dooglerMageMiniBoss.Attacking = true;
    dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, dooglerMageMiniBoss.castSpiralAnim, false);
    dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, dooglerMageMiniBoss.idleAnim, true, 0.0f);
    if (!string.IsNullOrEmpty(dooglerMageMiniBoss.LightningSpiralStartSFX))
      AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.LightningSpiralStartSFX);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.delayAttackSpiral)
      yield return (object) null;
    Vector3 originPosition = dooglerMageMiniBoss.transform.position;
    Vector3 dir = (dooglerMageMiniBoss.TargetObject.transform.position - originPosition).normalized;
    int SpawnCount = 0;
    bool ngFlashPerformed = false;
    Coroutine ngFlash = (Coroutine) null;
    while (SpawnCount < dooglerMageMiniBoss.numAvalanchesSpiral)
    {
      Vector3 vector3 = originPosition + dir * dooglerMageMiniBoss.distanceBetweenAvalanchesSpiral * (float) (SpawnCount + 1);
      if (!BiomeGenerator.PointWithinIsland(vector3, out Vector3 _))
      {
        ++SpawnCount;
        dir = Quaternion.AngleAxis(dooglerMageMiniBoss.angleBetweenAvalanchesSpiral, Vector3.forward) * dir;
      }
      else
      {
        GameObject lightning = ObjectPool.Spawn(dooglerMageMiniBoss.loadedLightningImpact, vector3, Quaternion.identity);
        lightning.GetComponent<LightningStrikeAttack>().TriggerLightningStrike(dooglerMageMiniBoss.health, vector3, (System.Action) (() => ObjectPool.Recycle(lightning)), true, customSFX: dooglerMageMiniBoss.LightningSpiralExplosionSFX);
        dir = Quaternion.AngleAxis(dooglerMageMiniBoss.angleBetweenAvalanchesSpiral, Vector3.forward) * dir;
        ++SpawnCount;
        time = 0.0f;
        while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.timeBetweenAvalanchesSpiral)
          yield return (object) null;
        if (dooglerMageMiniBoss.IsNG && SpawnCount >= dooglerMageMiniBoss.spiralAvalancheFlashAfter && !ngFlashPerformed)
        {
          dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
          ngFlash = dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.Flash1Attack());
          ngFlashPerformed = true;
        }
      }
    }
    if (ngFlash != null)
      yield return (object) ngFlash;
    dooglerMageMiniBoss.LineAttackTimer = dooglerMageMiniBoss.LineAttackDelayTime;
    dooglerMageMiniBoss.AttackTimer = dooglerMageMiniBoss.AttackDelayTime;
    dooglerMageMiniBoss.Attacking = false;
    dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator Flash1Attack()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.MyState = DooglerMageMiniBoss.State.Attacking;
    dooglerMageMiniBoss.Attacking = true;
    dooglerMageMiniBoss.RepathTimer = 0.0f;
    float AttackSpeed = 15f;
    bool Loop = true;
    float SignPostDelay = 0.3f;
    while (Loop)
    {
      if ((UnityEngine.Object) dooglerMageMiniBoss.Spine == (UnityEngine.Object) null || dooglerMageMiniBoss.Spine.AnimationState == null || dooglerMageMiniBoss.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        dooglerMageMiniBoss.Seperate(0.5f);
        switch (dooglerMageMiniBoss.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) dooglerMageMiniBoss.TargetObject)
            {
              dooglerMageMiniBoss.state.LookAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetObject.transform.position);
              dooglerMageMiniBoss.Spine.skeleton.ScaleX = (double) dooglerMageMiniBoss.state.LookAngle <= 90.0 || (double) dooglerMageMiniBoss.state.LookAngle >= 270.0 ? -1f : 1f;
              dooglerMageMiniBoss.state.LookAngle = dooglerMageMiniBoss.state.facingAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetObject.transform.position);
              dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, "flash-1-charge", false);
              if (!string.IsNullOrEmpty(dooglerMageMiniBoss.MeleeClawTeleportStartSFX))
                AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.MeleeClawTeleportStartSFX);
            }
            if ((UnityEngine.Object) dooglerMageMiniBoss.damageColliderEvents != (UnityEngine.Object) null)
            {
              dooglerMageMiniBoss.damageColliderEvents.SetActive(false);
              break;
            }
            break;
          case StateMachine.State.Moving:
            dooglerMageMiniBoss.TargetObject = (GameObject) null;
            dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.WaitForTarget());
            yield break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) dooglerMageMiniBoss.damageColliderEvents != (UnityEngine.Object) null)
              dooglerMageMiniBoss.damageColliderEvents.SetActive(false);
            dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(dooglerMageMiniBoss.state.Timer / SignPostDelay);
            dooglerMageMiniBoss.state.Timer += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
            if ((double) dooglerMageMiniBoss.state.Timer >= (double) SignPostDelay - (double) DooglerMageMiniBoss.signPostParryWindow)
              dooglerMageMiniBoss.canBeParried = true;
            if ((double) dooglerMageMiniBoss.state.Timer >= (double) SignPostDelay)
            {
              yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.CastFlashTeleportMechanicRoutine(dooglerMageMiniBoss.TargetObject.transform.position + Vector3.up * 0.5f));
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, "flash-1-appear", false);
              if (!string.IsNullOrEmpty(dooglerMageMiniBoss.MeleeClawTeleportStartSFX))
                AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.MeleeClawWindupSFX);
              float time = 0.0f;
              while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.DelayAfterFlash1)
                yield return (object) null;
              dooglerMageMiniBoss.LookAtTarget();
              dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, dooglerMageMiniBoss.state.LookAngle);
              dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              dooglerMageMiniBoss.speed = AttackSpeed * 0.0166666675f;
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, "flash-1-impact", false);
              dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, "flash-1-recover", false, 0.0f);
              dooglerMageMiniBoss.canBeParried = true;
              dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.EnableDamageCollider(0.0f));
              if (!string.IsNullOrEmpty(dooglerMageMiniBoss.MeleeClawSwipeSFX))
                AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.MeleeClawSwipeSFX, dooglerMageMiniBoss.transform.position);
              if (!string.IsNullOrEmpty(dooglerMageMiniBoss.AttackVO))
              {
                AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.AttackVO, dooglerMageMiniBoss.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime * dooglerMageMiniBoss.Spine.timeScale;
            dooglerMageMiniBoss.speed = AttackSpeed * Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
            dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
            dooglerMageMiniBoss.canBeParried = (double) dooglerMageMiniBoss.state.Timer <= (double) DooglerMageMiniBoss.attackParryWindow;
            if ((double) (dooglerMageMiniBoss.state.Timer += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) >= 0.5)
            {
              Loop = false;
              dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, "flash-1-recover", false);
              dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, dooglerMageMiniBoss.idleAnim, true, 0.0f);
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    dooglerMageMiniBoss.Attacking = false;
    dooglerMageMiniBoss.speed = 0.0f;
  }

  public IEnumerator Flash2Attack()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.MyState = DooglerMageMiniBoss.State.Attacking;
    dooglerMageMiniBoss.Attacking = true;
    dooglerMageMiniBoss.RepathTimer = 0.0f;
    float AttackSpeed = 15f;
    bool Loop = true;
    float SignPostDelay = 0.3f;
    int attackCount = 0;
    while (Loop)
    {
      if ((UnityEngine.Object) dooglerMageMiniBoss.Spine == (UnityEngine.Object) null || dooglerMageMiniBoss.Spine.AnimationState == null || dooglerMageMiniBoss.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        dooglerMageMiniBoss.Seperate(0.5f);
        switch (dooglerMageMiniBoss.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            if ((bool) (UnityEngine.Object) dooglerMageMiniBoss.TargetObject)
            {
              dooglerMageMiniBoss.state.LookAngle = dooglerMageMiniBoss.state.facingAngle = Utils.GetAngle(dooglerMageMiniBoss.transform.position, dooglerMageMiniBoss.TargetObject.transform.position);
              dooglerMageMiniBoss.Spine.skeleton.ScaleX = (double) dooglerMageMiniBoss.state.LookAngle <= 90.0 || (double) dooglerMageMiniBoss.state.LookAngle >= 270.0 ? -1f : 1f;
              dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, attackCount == 0 ? "flash-1-charge" : "flash-2-charge", false);
              string soundPath = attackCount == 0 ? dooglerMageMiniBoss.MeleeClawTeleportStartSFX : dooglerMageMiniBoss.MeleeStaffTeleportStartSFX;
              if (!string.IsNullOrEmpty(soundPath))
                AudioManager.Instance.PlayOneShot(soundPath);
            }
            if ((UnityEngine.Object) dooglerMageMiniBoss.damageColliderEvents != (UnityEngine.Object) null)
            {
              dooglerMageMiniBoss.damageColliderEvents.SetActive(false);
              break;
            }
            break;
          case StateMachine.State.Moving:
            dooglerMageMiniBoss.TargetObject = (GameObject) null;
            dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.WaitForTarget());
            yield break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) dooglerMageMiniBoss.damageColliderEvents != (UnityEngine.Object) null)
              dooglerMageMiniBoss.damageColliderEvents.SetActive(false);
            dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(dooglerMageMiniBoss.state.Timer / SignPostDelay);
            dooglerMageMiniBoss.state.Timer += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
            if ((double) dooglerMageMiniBoss.state.Timer >= (double) SignPostDelay - (double) DooglerMageMiniBoss.signPostParryWindow)
              dooglerMageMiniBoss.canBeParried = true;
            if ((double) dooglerMageMiniBoss.state.Timer >= (double) SignPostDelay)
            {
              yield return (object) dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.CastFlashTeleportMechanicRoutine(dooglerMageMiniBoss.TargetObject.transform.position + Vector3.up));
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, attackCount == 0 ? "flash-1-appear" : "flash-2-appear", false);
              string soundPath1 = attackCount == 0 ? dooglerMageMiniBoss.MeleeClawWindupSFX : dooglerMageMiniBoss.MeleeStaffWindupSFX;
              if (!string.IsNullOrEmpty(soundPath1))
                AudioManager.Instance.PlayOneShot(soundPath1);
              float time = 0.0f;
              while ((double) (time += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) < (double) dooglerMageMiniBoss.DelayAfterFlash2)
                yield return (object) null;
              dooglerMageMiniBoss.LookAtTarget();
              dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, dooglerMageMiniBoss.state.LookAngle);
              dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              dooglerMageMiniBoss.speed = AttackSpeed * 0.0166666675f;
              dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, attackCount == 0 ? "flash-1-impact" : "flash-2-impact", false);
              if (attackCount == 0)
                dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, "flash-1-recover", false, 0.0f);
              dooglerMageMiniBoss.canBeParried = true;
              dooglerMageMiniBoss.StartCoroutine((IEnumerator) dooglerMageMiniBoss.EnableDamageCollider(0.0f));
              string soundPath2 = attackCount == 0 ? dooglerMageMiniBoss.MeleeClawSwipeSFX : dooglerMageMiniBoss.MeleeStaffSwipeSFX;
              if (!string.IsNullOrEmpty(soundPath2))
                AudioManager.Instance.PlayOneShot(soundPath2);
              if (!string.IsNullOrEmpty(dooglerMageMiniBoss.AttackVO))
              {
                AudioManager.Instance.PlayOneShot(dooglerMageMiniBoss.AttackVO, dooglerMageMiniBoss.transform.position);
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime * dooglerMageMiniBoss.Spine.timeScale;
            dooglerMageMiniBoss.speed = AttackSpeed * Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale;
            dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
            dooglerMageMiniBoss.canBeParried = (double) dooglerMageMiniBoss.state.Timer <= (double) DooglerMageMiniBoss.attackParryWindow;
            if ((double) (dooglerMageMiniBoss.state.Timer += Time.deltaTime * dooglerMageMiniBoss.Spine.timeScale) >= 0.5)
            {
              dooglerMageMiniBoss.SimpleSpineFlash.FlashWhite(false);
              if (attackCount == 0)
              {
                dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
                ++attackCount;
                break;
              }
              dooglerMageMiniBoss.Spine.AnimationState.AddAnimation(0, dooglerMageMiniBoss.idleAnim, true, 0.0f);
              Loop = false;
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    dooglerMageMiniBoss.Attacking = false;
    dooglerMageMiniBoss.speed = 0.0f;
  }

  public IEnumerator PlaceIE()
  {
    DooglerMageMiniBoss dooglerMageMiniBoss = this;
    dooglerMageMiniBoss.ClearPaths();
    Vector3 offset = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 vector3 = (PlayerFarming.Instance.transform.position + offset) with
      {
        z = 0.0f
      };
      dooglerMageMiniBoss.transform.position = vector3;
      yield return (object) null;
    }
    dooglerMageMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    dooglerMageMiniBoss.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
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

  public IEnumerator EnableDamageCollider(float initialDelay, float enabledDuration = 0.2f)
  {
    if ((bool) (UnityEngine.Object) this.damageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.damageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) enabledDuration)
        yield return (object) null;
      this.damageColliderEvents.SetActive(false);
    }
  }

  public void SpawnEnemies(DooglerMageMiniBoss.WaveSpawnable waveSpawnable)
  {
    if (waveSpawnable == null)
      return;
    List<Vector3> vector3List = new List<Vector3>(2)
    {
      new Vector3(-2.5f, 0.0f, 0.0f),
      new Vector3(2.5f, 0.0f, 0.0f)
    };
    for (int index = 0; index < waveSpawnable.amount; ++index)
    {
      double num = (double) (360 / waveSpawnable.amount * index);
      Vector2 position = new Vector2(Mathf.Cos((float) (num * (Math.PI / 180.0))), Mathf.Sin((float) (num * (Math.PI / 180.0)))) * 2.5f;
      GameObject Spawn = ObjectPool.Spawn(waveSpawnable.spawnable, this.transform.parent, (Vector3) position, Quaternion.identity);
      Spawn.gameObject.SetActive(false);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(Spawn.transform.position, this.transform.parent, Spawn);
      UnitObject component = Spawn.GetComponent<UnitObject>();
      component.health.OnDie += new Health.DieAction(this.SpawnedEnemyKilled);
      this.spawnedEnemies.Add(component);
    }
    waveSpawnable.activated = true;
  }

  public void SpawnedEnemyKilled(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null && (UnityEngine.Object) this.spawnedEnemies[index].health == (UnityEngine.Object) Victim)
      {
        this.spawnedEnemies[index].health.OnDie -= new Health.DieAction(this.SpawnedEnemyKilled);
        this.spawnedEnemies.RemoveAt(index);
      }
    }
  }

  public DooglerMageMiniBoss.RangedAttacks GetNextRangedAttack()
  {
    if (this.rangedAttackOrder.Count <= 0)
      this.SetupRangedAttackOrder();
    DooglerMageMiniBoss.RangedAttacks nextRangedAttack = this.rangedAttackOrder[0];
    this.rangedAttackOrder.RemoveAt(0);
    this.lastRangedAttack = nextRangedAttack;
    return nextRangedAttack;
  }

  public DooglerMageMiniBoss.MeleeAttacks GetNextMeleeAttack()
  {
    if (this.meleeAttackOrder.Count <= 0)
      this.SetupMeleeAttackOrder();
    DooglerMageMiniBoss.MeleeAttacks nextMeleeAttack = this.meleeAttackOrder[0];
    this.meleeAttackOrder.RemoveAt(0);
    this.lastMeleeAttack = nextMeleeAttack;
    return nextMeleeAttack;
  }

  public void SetupRangedAttackOrder()
  {
    switch (this.Phase)
    {
      case 0:
        this.rangedAttackOrder = new List<DooglerMageMiniBoss.RangedAttacks>()
        {
          DooglerMageMiniBoss.RangedAttacks.ChasingLine,
          DooglerMageMiniBoss.RangedAttacks.IndividualStrikes,
          DooglerMageMiniBoss.RangedAttacks.Spiral
        };
        break;
      case 1:
      case 2:
        this.rangedAttackOrder = new List<DooglerMageMiniBoss.RangedAttacks>()
        {
          DooglerMageMiniBoss.RangedAttacks.ChasingLine,
          DooglerMageMiniBoss.RangedAttacks.RingLightning,
          DooglerMageMiniBoss.RangedAttacks.Spiral
        };
        break;
    }
    this.rangedAttackOrder = this.GetShuffledEnumList<DooglerMageMiniBoss.RangedAttacks>(this.rangedAttackOrder);
    if (this.rangedAttackOrder[0] != this.lastRangedAttack)
      return;
    List<DooglerMageMiniBoss.RangedAttacks> rangedAttackOrder1 = this.rangedAttackOrder;
    List<DooglerMageMiniBoss.RangedAttacks> rangedAttackOrder2 = this.rangedAttackOrder;
    DooglerMageMiniBoss.RangedAttacks rangedAttacks1 = this.rangedAttackOrder[1];
    DooglerMageMiniBoss.RangedAttacks rangedAttacks2 = this.rangedAttackOrder[0];
    int num;
    DooglerMageMiniBoss.RangedAttacks rangedAttacks3 = (DooglerMageMiniBoss.RangedAttacks) (num = (int) rangedAttacks1);
    rangedAttackOrder1[0] = (DooglerMageMiniBoss.RangedAttacks) num;
    rangedAttackOrder2[1] = rangedAttacks3 = rangedAttacks2;
  }

  public void SetupMeleeAttackOrder()
  {
    this.meleeAttackOrder = new List<DooglerMageMiniBoss.MeleeAttacks>((IEnumerable<DooglerMageMiniBoss.MeleeAttacks>) Enum.GetValues(typeof (DooglerMageMiniBoss.MeleeAttacks)));
    this.meleeAttackOrder = this.GetShuffledEnumList<DooglerMageMiniBoss.MeleeAttacks>(this.meleeAttackOrder);
    if (this.meleeAttackOrder[0] != this.lastMeleeAttack)
      return;
    List<DooglerMageMiniBoss.MeleeAttacks> meleeAttackOrder1 = this.meleeAttackOrder;
    List<DooglerMageMiniBoss.MeleeAttacks> meleeAttackOrder2 = this.meleeAttackOrder;
    DooglerMageMiniBoss.MeleeAttacks meleeAttacks1 = this.meleeAttackOrder[1];
    DooglerMageMiniBoss.MeleeAttacks meleeAttacks2 = this.meleeAttackOrder[0];
    int num;
    DooglerMageMiniBoss.MeleeAttacks meleeAttacks3 = (DooglerMageMiniBoss.MeleeAttacks) (num = (int) meleeAttacks1);
    meleeAttackOrder1[0] = (DooglerMageMiniBoss.MeleeAttacks) num;
    meleeAttackOrder2[1] = meleeAttacks3 = meleeAttacks2;
  }

  public List<T> GetShuffledEnumList<T>(List<T> enumValues) where T : Enum
  {
    System.Random random = new System.Random();
    int count = enumValues.Count;
    while (count > 1)
    {
      --count;
      int index1 = random.Next(count + 1);
      List<T> objList1 = enumValues;
      int num = count;
      List<T> objList2 = enumValues;
      int index2 = index1;
      T enumValue1 = enumValues[index1];
      T enumValue2 = enumValues[count];
      int index3 = num;
      T obj1;
      T obj2 = obj1 = enumValue1;
      objList1[index3] = obj1;
      objList2[index2] = obj2 = enumValue2;
    }
    return enumValues;
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__125_0(AsyncOperationHandle<GameObject> obj)
  {
    DooglerMageMiniBoss.loadedAddressableAssets.Add(obj);
    this.loadedLightningImpact = obj.Result;
    this.loadedLightningImpact.CreatePool(Mathf.Max(this.maxChasingAvalanches, this.numAvalanchesSpiral), true);
  }

  public enum RangedAttacks
  {
    ChasingLine,
    IndividualStrikes,
    RingLightning,
    Spiral,
  }

  public enum MeleeAttacks
  {
    Swipe,
    QuadLightning,
    TeleportAway,
  }

  public enum State
  {
    WaitAndTaunt,
    Teleporting,
    Attacking,
    Avoiding,
  }

  public enum eDooglerMageAttack
  {
    lineAvalanches,
    chaseAvalances,
    cicleAvalanches,
  }

  [Serializable]
  public class WaveSpawnable
  {
    public GameObject spawnable;
    public int amount;
    public float normHealth;
    public bool activated;
  }
}
