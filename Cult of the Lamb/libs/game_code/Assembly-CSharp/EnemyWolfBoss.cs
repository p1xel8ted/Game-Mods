// Decompiled with JetBrains decompiler
// Type: EnemyWolfBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyWolfBoss : UnitObject
{
  public static EnemyWolfBoss Instance;
  [SerializeField]
  public bool TESTING;
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public CircleCollider2D physicsCollider;
  [SerializeField]
  public GameObject meleeDamageCollider;
  [SerializeField]
  public Vector2 timeBetweenAttacks;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public Transform arrowIndicator;
  [Header("Movement")]
  [SerializeField]
  public float acceleration;
  [SerializeField]
  public float deceleration;
  [SerializeField]
  public Vector2 repaithDelay;
  [SerializeField]
  public Vector2 distanceBetweenMovement;
  [SerializeField]
  public float jumpDistance = 7f;
  [SerializeField]
  public LayerMask islandMask;
  [Header("Arms")]
  [SerializeField]
  public WolfBossArm[] arms;
  [SerializeField]
  public WolfBossArm[] stomachCreatures;
  [SerializeField]
  public Vector2 projectilesToShoot;
  [SerializeField]
  public float delayBetweenProjectiles;
  [SerializeField]
  public Projectile projectile;
  [SerializeField]
  public float projectileSpeed;
  [SerializeField]
  public float sweepAttackAnticipation;
  [SerializeField]
  public float sweepAttackDuration;
  [Header("Avalanche")]
  [SerializeField]
  public TrapAvalanche trapAvalanche;
  [SerializeField]
  public int avalancheAmount = 25;
  [SerializeField]
  public float avalancheTimeBetweenSpawns = 0.35f;
  [SerializeField]
  public float avalancheDropDelay = 4f;
  [SerializeField]
  public float avalancheDropSpeed = 22f;
  [Header("Spawning")]
  [Space]
  [SerializeField]
  public GameObject fleshEnemy;
  [SerializeField]
  public int fleshToSpawn;
  [SerializeField]
  public GameObject segmentedWorm;
  [SerializeField]
  public GameObject stomachPosition;
  [Header("Beams")]
  [SerializeField]
  public float beamDuration;
  [SerializeField]
  public float beamMoveSpeed;
  [SerializeField]
  public float beamRotationSpeed;
  [SerializeField]
  public GameObject impactPrefab;
  [SerializeField]
  public GameObject beamDamage;
  [SerializeField]
  public BoneFollower[] eyePositions;
  [SerializeField]
  public AssetReferenceGameObject indicatorPrefab;
  [Header("Fire")]
  [SerializeField]
  public TrapLavaTimed[] fireTraps;
  [SerializeField]
  public int startingColumns = 6;
  [SerializeField]
  public float startingSpread = 2f;
  [SerializeField]
  public float spreadIncrease = 1f;
  [SerializeField]
  public float distanceBetweenRows = 2f;
  [SerializeField]
  public int increasePerRow = 2;
  [SerializeField]
  public int totalRows = 10;
  [SerializeField]
  public float timeBetweenRows = 0.25f;
  [SerializeField]
  public Vector2 fireAttacks;
  [SerializeField]
  public float timeBetweenFireAttacks;
  [Header("Flesh Rocks")]
  [SerializeField]
  public TrapFleshRock[] fleshRocks;
  [Header("Rot Bombs")]
  [SerializeField]
  public GameObject rotBomb;
  [SerializeField]
  public Vector2 rotBombsToSpawn;
  [SerializeField]
  public Vector2 rotBombHeightOffset;
  [SerializeField]
  public Vector2 rotBombDuration;
  [SerializeField]
  public float rotBombExplodeDelay;
  [SerializeField]
  public Vector2 timeBetweenRotBombs;
  [Header("Secret Enemy")]
  [SerializeField]
  public EnemyHole holeEnemy;
  [SerializeField]
  public float timeBetweenHoleEnemies;
  [Header("Particles")]
  [SerializeField]
  public GameObject armRevealLeftParticle;
  [SerializeField]
  public GameObject armRevealRightParticle;
  [SerializeField]
  public GameObject armBreakLeftParticle;
  [SerializeField]
  public GameObject armBreakRightParticle;
  [SerializeField]
  public GameObject dieRightParticle;
  [SerializeField]
  public GameObject dieLeftParticle;
  [SerializeField]
  public GameObject dieFinalParticle;
  [SerializeField]
  public GameObject grabbedLeftWallParticle;
  [SerializeField]
  public GameObject grabbedRightWallParticle;
  [SerializeField]
  public GameObject diePullLeft;
  [SerializeField]
  public GameObject diePullRight;
  [Header("Projectiles")]
  [SerializeField]
  public ProjectilePatternBeam[] stomachProjectilePatterns;
  [Header("Phase 2")]
  [SerializeField]
  public float leftArmHP = 20f;
  [SerializeField]
  public float rightArmHP = 20f;
  [SerializeField]
  public WolfArmParasite[] armParasites;
  [Header("Phase 3")]
  [SerializeField]
  public float bounceDistance = 3f;
  [SerializeField]
  public float bounceRingSpeed = 3f;
  [SerializeField]
  public float bounceRingMaxRadius = 3f;
  [Space]
  [SerializeField]
  public Interaction_SimpleConversation markOfYngya;
  [SerializeField]
  public Transform[] lostSouls;
  [SerializeField]
  public TrapLightning[] lightningTraps;
  public float repathTimestamp;
  public bool activated = true;
  public bool isDead;
  public bool areSidesGrabbed;
  public bool moving = true;
  public bool reachedMaxSpeed;
  public bool phase1;
  public bool phase2;
  public bool phase3;
  public bool phase4;
  public float holeEnemiesTimer = 6f;
  public List<EnemyHole> holeEnemiesSpawned = new List<EnemyHole>();
  public Coroutine primaryAttack;
  public List<Health> spawnedEnemies = new List<Health>();
  public bool rightArmBroken;
  public bool leftArmBroken;
  public List<ArrowLightningBeam> activeBeams = new List<ArrowLightningBeam>();
  public List<GameObject> activeBeamsVisuals = new List<GameObject>();
  public LayerMask obstacleMask;
  public Vector3 startingPosition;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public GameObject loadedIndicator;
  public List<GameObject> currentIndicators = new List<GameObject>();
  public string attackSlashStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_slash_start";
  public string attackGroundSlamStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_ground_slam_start";
  public string attackGroundSlamImpactSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_ground_slam_impact";
  public string attackTentacleStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_tentacle_start";
  public string attackBigArmSlamSingleStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_slam_single_start";
  public string attackBigArmSlamImpactSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_ground_slam_impact";
  public string attackBigArmSlamDoubleStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_slam_double_start";
  public string attackBigArmSlamMultiStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_slam_multi_start";
  public string attackBigArmSlamMultiLiftSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_slam_multi_lift";
  public string footstepSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_footstep";
  public string jumpStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_jump_start";
  public string jumpLandSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_jump_land";
  public string knockdownStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_knockdown_start";
  public string knockdownGetUpSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_knockdown_getup";
  public string phaseSpawnBigArmsSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/phase_bigarms_spawn";
  public string attackBigArmSpikesSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_spikes_start";
  public string armRippedOffSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_arm_off";
  public string phaseElectrocutedArmsStartSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/phase_electrocutedarms_start";
  public string phaseElectrocutedArmsCameraWhooshSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/phase_electrocutedarms_camera_whoosh";
  public string electrocutedArmsLoop = "event:/dlc/dungeon06/enemy/miniboss_marchosias/phase_electrocutedarms_amb_loop";
  public string storyOutrroFallDownSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/story_outro_falldown";
  public string defeatMomentOfImpactSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/story_outro_defeat_impact";
  public string storyOutroSacrificeSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/story_outro_sacrifice";
  public string storyOutroGhostsAppearSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/story_outro_ghosts_appear";
  public string attackFollowingLaserSingleShotSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_laser_single_shoot";
  public string attackMultiLaserLoopSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_laser_multi_loop";
  public string attackMultiBombLaunchSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_multi_bomb_launch";
  public string attackSlashStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_slash_start_vo";
  public string attackGroundSlamStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_ground_slam_start_vo";
  public string attackTentacleStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_tentacle_start_vo";
  public string attackBigArmSlamSingleStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_slam_single_start_vo";
  public string attackBigArmSlamDoubleStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_slam_double_start_vo";
  public string attackBigArmSlamMultiStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_slam_multi_start_vo";
  public string attackBigArmSlamMultiLiftVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_slam_multi_lift_vo";
  public string jumpStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_jump_start_vo";
  public string knockdownStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_knockdown_start_vo";
  public string knockdownGetUpVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_knockdown_getup_vo";
  public string attackBigArmSpikesVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_bigarm_spikes_start_vo";
  public string armRippedOffVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/mv_arm_off_vo";
  public string getHitVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/gethit_vo";
  public string attackFollowingLaserStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_laser_single_start_vo";
  public string attackMultiLaserStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_laser_multi_start_vo";
  public string attackMultiBombStartVO = "event:/dlc/dungeon06/enemy/miniboss_marchosias/attack_multi_bomb_start_vo";
  public string bossMusic = "event:/music/snowy_mountain/snowy_mountain";
  public EventInstance electrocutedArmLoopLeftInstance;
  public EventInstance electrocutedArmLoopRightInstance;
  public EventInstance multiLasterInstance;
  public bool DISABLE_PUSH_PLAYER;
  public float SeperationRadius_X = 3.5f;
  public float SeperationRadius_Y = 1.5f;

  public float phase2Threshold => this.health.totalHP * 0.85f;

  public float phase3Threshold => this.health.totalHP * 0.4f;

  public float phase4Threshold => this.health.totalHP * 0.2f;

  public Vector3 PositionOffset => this.transform.position - this.startingPosition;

  public void Start()
  {
    this.LoadAssets();
    this.health = this.GetComponent<Health>();
    this.startingPosition = this.transform.position;
    this.obstacleMask = (LayerMask) LayerMask.GetMask("Obstacles");
    GameManager.GetInstance().AddToCamera(this.Spine.gameObject);
    foreach (WolfBossArm arm in this.arms)
      arm.SetRootDamageCollider(false);
    foreach (WolfBossArm stomachCreature in this.stomachCreatures)
      stomachCreature.SetRootDamageCollider(false);
    foreach (Component fireTrap in this.fireTraps)
      fireTrap.gameObject.SetActive(false);
    this.TESTING = false;
    this.Play();
    this.EndOfPath = this.EndOfPath + new System.Action(this.OnEndOfPath);
    foreach (BoneFollower eyePosition in this.eyePositions)
      eyePosition.PositionOffset = eyePosition.transform.forward * (this.Spine.zSpacing * 50f);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyWolfBoss.Instance = this;
  }

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.indicatorPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyWolfBoss.loadedAddressableAssets.Add(obj);
      this.loadedIndicator = obj.Result;
      this.loadedIndicator.CreatePool(20, true);
    });
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.EndOfPath = this.EndOfPath - new System.Action(this.OnEndOfPath);
    SimulationManager.UnPause();
    if (EnemyWolfBoss.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in EnemyWolfBoss.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      EnemyWolfBoss.loadedAddressableAssets.Clear();
    }
    this.CleanupEventInstances();
  }

  public void CleanupEventInstances()
  {
    AudioManager.Instance.StopLoop(this.electrocutedArmLoopLeftInstance);
    AudioManager.Instance.StopLoop(this.electrocutedArmLoopRightInstance);
    AudioManager.Instance.StopLoop(this.multiLasterInstance);
  }

  public override void Update()
  {
    base.Update();
    if (!this.isDead && !SimulationManager.IsPaused)
      SimulationManager.Pause();
    if (!this.isDead && !this.DISABLE_PUSH_PLAYER)
    {
      foreach (UnitObject seperater in UnitObject.Seperaters)
      {
        if ((UnityEngine.Object) seperater != (UnityEngine.Object) this && (UnityEngine.Object) seperater != (UnityEngine.Object) null && seperater.health.team == Health.Team.PlayerTeam && seperater.state.CURRENT_STATE != StateMachine.State.Dodging && seperater.state.CURRENT_STATE != StateMachine.State.Defending)
        {
          seperater.seperatorVX = 0.0f;
          seperater.seperatorVY = 0.0f;
          Vector2 vector2 = (Vector2) (seperater.gameObject.transform.position - this.transform.position);
          double num1 = (double) vector2.x / (double) this.SeperationRadius_X;
          float num2 = vector2.y / this.SeperationRadius_Y;
          float num3 = Mathf.Sqrt((float) (num1 * num1 + (double) num2 * (double) num2));
          if ((double) num3 < 1.0)
          {
            double num4 = 1.0 - (double) num3;
            Vector2 normalized = vector2.normalized;
            float num5 = (float) (num4 / 2.0) * GameManager.FixedDeltaTime;
            seperater.seperatorVX += normalized.x * num5;
            seperater.seperatorVY += normalized.y * num5;
          }
        }
      }
    }
    if (!this.phase3 && !this.phase4 && this.activated && !this.isDead && this.moving && (double) this.repathTimestamp != -1.0)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float repathTimestamp = this.repathTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) repathTimestamp & currentTime.HasValue && !this.TESTING)
      {
        Vector3 targetLocation = new Vector3(0.0f, 0.0f, 0.0f) + (Vector3) UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(this.distanceBetweenMovement.x, this.distanceBetweenMovement.y);
        Vector3 zero = Vector3.zero;
        foreach (PlayerFarming player in PlayerFarming.players)
          zero += player.transform.position;
        if ((double) (zero / (float) PlayerFarming.players.Count).y < -13.0)
          targetLocation += Vector3.down * UnityEngine.Random.Range(4f, 6f);
        this.givePath(targetLocation, (GameObject) null, false, false);
        this.repathTimestamp = -1f;
        this.SpeedMultiplier = 0.0f;
        this.reachedMaxSpeed = false;
        goto label_28;
      }
    }
    if ((double) this.SpeedMultiplier < 1.0 && (double) this.speed < (double) this.maxSpeed && !this.reachedMaxSpeed)
      this.SpeedMultiplier = Mathf.Clamp01(this.SpeedMultiplier + this.acceleration * Time.deltaTime * this.Spine.timeScale);
    else if ((double) this.SpeedMultiplier >= 1.0 && !this.reachedMaxSpeed)
      this.reachedMaxSpeed = true;
    else if ((double) this.SpeedMultiplier > 0.0 && this.reachedMaxSpeed)
    {
      this.SpeedMultiplier = Mathf.Clamp01(this.SpeedMultiplier - this.deceleration * Time.deltaTime * this.Spine.timeScale);
      if ((double) this.SpeedMultiplier <= 0.0)
      {
        this.ClearPaths();
        this.OnEndOfPath();
      }
    }
label_28:
    if ((double) Time.time <= (double) this.holeEnemiesTimer || this.isDead || this.phase3 || this.phase4)
      return;
    this.SpawnHoleEnemy();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.BossEntryAmbience);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    AudioManager.Instance.PlayOneShot(this.defeatMomentOfImpactSFX);
    foreach (TrapLightning lightningTrap in this.lightningTraps)
      lightningTrap.StopTrap();
    this.CleanupCurrentIndicators();
    this.arrowIndicator.gameObject.SetActive(false);
    this.StartCoroutine((IEnumerator) this.SlowMo());
    this.isDead = true;
    this.ClearPaths();
    this.enabled = false;
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health)
      {
        Health.team2[index].enabled = true;
        Health.team2[index].invincible = false;
        Health.team2[index].untouchable = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DieIE());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    float num = this.health.DamageModifier;
    if ((double) this.health.HP <= (double) this.phase4Threshold)
      num = !this.leftArmBroken || !this.rightArmBroken ? 0.0f : 1f;
    else if (this.phase3 || (this.phase1 || this.phase2) && (double) this.health.HP <= (double) this.phase3Threshold)
      num = 0.2f;
    if ((double) num != (double) this.health.DamageModifier)
      this.health.DamageModifier = num;
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    AudioManager.Instance.PlayOneShot(this.getHitVO, this.gameObject);
    if ((double) this.health.HP <= (double) this.phase4Threshold)
    {
      if ((double) this.rightArmHP != 0.0)
        this.rightArmHP = 0.0f;
      if ((double) this.leftArmHP != 0.0)
        this.leftArmHP = 0.0f;
    }
    WolfArmPiece p = (WolfArmPiece) null;
    WolfArmPiece.Lookup.TryGetValue(Attacker.GetInstanceID(), out p);
    if (this.phase3 && this.areSidesGrabbed)
    {
      if ((UnityEngine.Object) p != (UnityEngine.Object) null)
      {
        if (this.arms[1].Contains(p))
          --this.rightArmHP;
        else if (this.arms[0].Contains(p))
          --this.leftArmHP;
        else if (!this.leftArmBroken)
          --this.leftArmHP;
        else if (!this.rightArmBroken)
          --this.rightArmHP;
      }
      else if (!this.leftArmBroken)
        --this.leftArmHP;
      else if (!this.rightArmBroken)
        --this.rightArmHP;
      if ((double) this.rightArmHP <= 0.0 && !this.rightArmBroken)
        this.BreakRightArm();
      else if ((double) this.leftArmHP <= 0.0 && !this.leftArmBroken)
        this.BreakLeftArm();
    }
    this.simpleSpineFlash.FlashFillRed();
    if ((UnityEngine.Object) this.arms[0] != (UnityEngine.Object) null && !this.leftArmBroken)
      this.arms[0].FlashFillRed();
    if (!((UnityEngine.Object) this.arms[1] != (UnityEngine.Object) null) || this.rightArmBroken)
      return;
    this.arms[1].FlashFillRed();
  }

  public void Die() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.TESTING_DEATH());

  public IEnumerator TESTING_DEATH()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.RevealArmsIE());
    yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.GrabSidesIE());
    enemyWolfBoss.BreakLeftArm();
    enemyWolfBoss.BreakRightArm();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.health.HP = 1f;
    enemyWolfBoss.health.DealDamage(enemyWolfBoss.health.totalHP, PlayerFarming.Instance.gameObject, enemyWolfBoss.transform.position);
  }

  public IEnumerator DieIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    yield return (object) null;
    if (!enemyWolfBoss.leftArmBroken)
      enemyWolfBoss.BreakLeftArm();
    if (!enemyWolfBoss.rightArmBroken)
      enemyWolfBoss.BreakRightArm();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillWolf);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyWolfBoss.cameraTarget, 12f);
    UIBossHUD.Hide();
    foreach (TrapFleshRock fleshRock in enemyWolfBoss.fleshRocks)
    {
      if ((UnityEngine.Object) fleshRock != (UnityEngine.Object) null && (UnityEngine.Object) fleshRock.TrapHealth != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) fleshRock.gameObject);
    }
    foreach (Behaviour armParasite in enemyWolfBoss.armParasites)
      armParasite.enabled = false;
    Debug.Log((object) "Achievement: Defeated Marchosias!");
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.BEAT_WOLF));
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.storyOutrroFallDownSFX);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "die", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "die-loop", true, 0.0f);
    enemyWolfBoss.transform.DOMove(new Vector3(0.0f, 1f, 0.0f), 1.66666663f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.66666663f, enemyWolfBoss.Spine);
    enemyWolfBoss.GetComponent<Collider2D>().enabled = false;
    PlayerFarming.Instance.GoToAndStop(enemyWolfBoss.transform.position + Vector3.down * 2f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    if (!DataManager.Instance.BeatenWolf)
    {
      List<ConversationEntry> Entries1 = new List<ConversationEntry>()
      {
        new ConversationEntry(enemyWolfBoss.gameObject, "Conversation_NPC/Wolf/Fight/Defeated/0"),
        new ConversationEntry(enemyWolfBoss.gameObject, "Conversation_NPC/Wolf/Fight/Defeated/1"),
        new ConversationEntry(enemyWolfBoss.gameObject, "Conversation_NPC/Wolf/Fight/Defeated/2")
      };
      foreach (ConversationEntry conversationEntry in Entries1)
      {
        conversationEntry.CharacterName = LocalizationManager.GetTranslation("NAMES/Wolf");
        conversationEntry.Animation = "die-talk";
        conversationEntry.DefaultAnimation = "die-loop";
        conversationEntry.SetZoom = true;
        conversationEntry.Zoom = 12f;
      }
      MMConversation.Play(new ConversationObject(Entries1, (List<MMTools.Response>) null, (System.Action) null), false);
      yield return (object) null;
      while (MMConversation.isPlaying)
        yield return (object) null;
      for (int i = 0; i < 3; ++i)
      {
        bool indoctrinated = false;
        yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.WaitForChoice((System.Action<bool>) (r => indoctrinated = r)));
        if (indoctrinated)
        {
          List<ConversationEntry> Entries2 = new List<ConversationEntry>()
          {
            new ConversationEntry(enemyWolfBoss.gameObject, $"Conversation_NPC/Wolf/Fight/Indoctrinate/{i}")
          };
          Entries2[0].CharacterName = LocalizationManager.GetTranslation("NAMES/Wolf");
          Entries2[0].Animation = i == 0 || i == 2 ? "die-refuse" : "die-refuse2";
          Entries2[0].DefaultAnimation = "die-loop";
          Entries2[0].SetZoom = true;
          Entries2[0].Zoom = 12f;
          MMConversation.Play(new ConversationObject(Entries2, (List<MMTools.Response>) null, (System.Action) null), false);
          yield return (object) null;
          while (MMConversation.isPlaying)
            yield return (object) null;
        }
        else
        {
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.Sacrificed());
          break;
        }
      }
    }
    GameManager.GetInstance().OnConversationNext(enemyWolfBoss.cameraTarget, 12f);
    yield return (object) new UnityEngine.WaitForSeconds(0.7f);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.storyOutroSacrificeSFX);
    yield return (object) new UnityEngine.WaitForSeconds(0.3f);
    enemyWolfBoss.armParasites[0].SubSpine.transform.DOLocalRotate(new Vector3(45f, 90f, -135f), 0.5f);
    enemyWolfBoss.armParasites[1].SubSpine.transform.DOLocalRotate(new Vector3(-45f, 90f, -45f), 0.5f);
    enemyWolfBoss.armParasites[0].transform.DOMove(new Vector3(-5.5f, 4f, -5f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    enemyWolfBoss.armParasites[1].transform.DOMove(new Vector3(5.5f, 4f, -5f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new UnityEngine.WaitForSeconds(0.5f);
    enemyWolfBoss.armParasites[0].SubSpine.AnimationState.SetAnimation(0, "mouth_open", false);
    enemyWolfBoss.armParasites[0].SubSpine.AnimationState.AddAnimation(0, "mouth_open_loop", true, 0.0f);
    enemyWolfBoss.armParasites[1].SubSpine.AnimationState.SetAnimation(0, "mouth_open", false);
    enemyWolfBoss.armParasites[1].SubSpine.AnimationState.AddAnimation(0, "mouth_open_loop", true, 0.0f);
    yield return (object) new UnityEngine.WaitForSeconds(0.5f);
    string animationName1 = DataManager.Instance.BeatenWolf ? "die-end-noheart" : "die-end";
    string animationName2 = DataManager.Instance.BeatenWolf ? "dead-noheart" : "dead";
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, animationName1, false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, animationName2, true, 0.0f);
    float dur = 1.66666663f;
    yield return (object) new UnityEngine.WaitForSeconds(dur * 0.8f);
    enemyWolfBoss.armParasites[1].transform.DOMove(new Vector3(1.6f, 0.8800063f, -2.55f), dur * 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    enemyWolfBoss.armParasites[1].SubSpine.AnimationState.SetAnimation(0, "mouth_close", false);
    enemyWolfBoss.armParasites[1].SubSpine.AnimationState.AddAnimation(0, "mouth_closed_loop", true, 0.0f);
    yield return (object) new UnityEngine.WaitForSeconds(dur * 0.2f);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 0.3f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.3f);
    enemyWolfBoss.dieLeftParticle.gameObject.SetActive(true);
    yield return (object) new UnityEngine.WaitForSeconds(dur * 0.7f);
    enemyWolfBoss.armParasites[0].transform.DOMove(new Vector3(-1.6f, 0.8800063f, -2.55f), dur * 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    enemyWolfBoss.armParasites[0].SubSpine.AnimationState.SetAnimation(0, "mouth_close", false);
    enemyWolfBoss.armParasites[0].SubSpine.AnimationState.AddAnimation(0, "mouth_closed_loop", true, 0.0f);
    yield return (object) new UnityEngine.WaitForSeconds(dur * 0.2f);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 0.3f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.3f);
    enemyWolfBoss.dieRightParticle.gameObject.SetActive(true);
    yield return (object) new UnityEngine.WaitForSeconds(1.33333337f);
    enemyWolfBoss.armParasites[0].transform.DOMove(new Vector3(-2f, 0.8800063f, -3.25f), 0.333333343f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    enemyWolfBoss.armParasites[1].transform.DOMove(new Vector3(2f, 0.8800063f, -3.25f), 0.333333343f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.2f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.2f);
    enemyWolfBoss.dieRightParticle.gameObject.SetActive(false);
    enemyWolfBoss.dieLeftParticle.gameObject.SetActive(false);
    enemyWolfBoss.dieRightParticle.gameObject.SetActive(true);
    enemyWolfBoss.dieLeftParticle.gameObject.SetActive(true);
    enemyWolfBoss.diePullLeft.gameObject.SetActive(true);
    enemyWolfBoss.diePullRight.gameObject.SetActive(true);
    yield return (object) new UnityEngine.WaitForSeconds(1.2f);
    enemyWolfBoss.armParasites[0].transform.DOMove(new Vector3(-3f, 0.8800063f, -4.5f), 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    enemyWolfBoss.armParasites[1].transform.DOMove(new Vector3(3f, 0.8800063f, -4.5f), 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 1f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 1f);
    enemyWolfBoss.dieFinalParticle.gameObject.SetActive(true);
    enemyWolfBoss.diePullLeft.gameObject.SetActive(false);
    enemyWolfBoss.diePullRight.gameObject.SetActive(false);
    BiomeConstants.Instance.ImpactFrameForDuration();
    yield return (object) new UnityEngine.WaitForSeconds(0.1f);
    enemyWolfBoss.armParasites[0].SubSpine.AnimationState.SetAnimation(0, "mouth_open", false);
    enemyWolfBoss.armParasites[0].SubSpine.AnimationState.AddAnimation(0, "mouth_open_loop", true, 0.0f);
    enemyWolfBoss.armParasites[1].SubSpine.AnimationState.SetAnimation(0, "mouth_open", false);
    enemyWolfBoss.armParasites[1].SubSpine.AnimationState.AddAnimation(0, "mouth_open_loop", true, 0.0f);
    yield return (object) new UnityEngine.WaitForSeconds(1.23f);
    enemyWolfBoss.armParasites[0].SubSpine.AnimationState.SetAnimation(0, "mouth_close", false);
    enemyWolfBoss.armParasites[0].SubSpine.AnimationState.AddAnimation(0, "mouth_closed_loop", true, 0.0f);
    enemyWolfBoss.armParasites[1].SubSpine.AnimationState.SetAnimation(0, "mouth_close", false);
    enemyWolfBoss.armParasites[1].SubSpine.AnimationState.AddAnimation(0, "mouth_closed_loop", true, 0.0f);
    enemyWolfBoss.armParasites[0].transform.DOMove(new Vector3(-5f, 0.8800063f, -1f), 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
    enemyWolfBoss.armParasites[1].transform.DOMove(new Vector3(5f, 0.8800063f, -1f), 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce);
    yield return (object) new UnityEngine.WaitForSeconds(2f);
    if (!DataManager.Instance.BeatenWolf)
    {
      DataManager.Instance.BeatenWolf = true;
      enemyWolfBoss.GetComponent<Interaction_MonsterHeart>().enabled = true;
      enemyWolfBoss.GetComponent<Interaction_MonsterHeart>().OnInteraction += new Interaction.InteractionEvent(enemyWolfBoss.\u003CDieIE\u003Eb__170_0);
      GameManager.GetInstance().OnConversationEnd();
      while (!RoomLockController.DoorsOpen)
        yield return (object) null;
      RoomLockController.CloseAll();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(enemyWolfBoss.markOfYngya.gameObject);
      yield return (object) new UnityEngine.WaitForSeconds(1f);
      enemyWolfBoss.markOfYngya.gameObject.SetActive(true);
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.storyOutroGhostsAppearSFX);
      for (int index = 0; index < enemyWolfBoss.lostSouls.Length; ++index)
      {
        Vector3 position = enemyWolfBoss.lostSouls[index].position;
        enemyWolfBoss.lostSouls[index].position = new Vector3((double) enemyWolfBoss.lostSouls[index].position.x + (double) enemyWolfBoss.lostSouls[index].position.x < 0.0 ? -10f : 10f, enemyWolfBoss.lostSouls[index].position.y, enemyWolfBoss.lostSouls[index].position.z);
        enemyWolfBoss.lostSouls[index].DOMove(position, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      }
      yield return (object) new UnityEngine.WaitForSeconds(3f);
      GameManager.GetInstance().OnConversationEnd();
      enemyWolfBoss.markOfYngya.Play();
    }
    else
    {
      Interaction_MarkOfYngya componentInChildren = enemyWolfBoss.markOfYngya.GetComponentInChildren<Interaction_MarkOfYngya>();
      componentInChildren.Teleporter.CachePosition = componentInChildren.Teleporter.transform.position;
      componentInChildren.Teleporter.EnableTeleporter();
      GameManager.GetInstance().OnConversationEnd();
      RoomLockController.CloseAll();
    }
  }

  public IEnumerator Sacrificed()
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "Conversation_NPC/Wolf/Fight/Kill/0")
    }, (List<MMTools.Response>) null, (System.Action) null), false);
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
  }

  public IEnumerator WaitForChoice(System.Action<bool> callback)
  {
    EnemyWolfBoss enemyWolfBoss = this;
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
    choice.Offset = new Vector3(0.0f, -350f);
    choice.Show("Interactions/Indoctrinate", "Interactions/Sacrifice", (System.Action) (() =>
    {
      System.Action<bool> action = callback;
      if (action != null)
        action(true);
      g = (GameObject) null;
    }), (System.Action) (() =>
    {
      System.Action<bool> action = callback;
      if (action != null)
        action(false);
      g = (GameObject) null;
    }), enemyWolfBoss.transform.position);
    while ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      choice.UpdatePosition(enemyWolfBoss.transform.position);
      yield return (object) null;
    }
  }

  public IEnumerator GiveRewardSequence()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", enemyWolfBoss.transform.position);
    FoundItemPickUp deco = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, enemyWolfBoss.transform.position).GetComponent<FoundItemPickUp>();
    deco.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_WOLF;
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 itemTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    yield return (object) new UnityEngine.WaitForSeconds(0.5f);
    deco.GetComponent<PickUp>().enabled = false;
    bool wait = true;
    deco.transform.DOMove(itemTargetPosition, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => wait = false));
    while (wait)
      yield return (object) null;
    deco.transform.position = itemTargetPosition;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    UnityEngine.Object.Destroy((UnityEngine.Object) deco.gameObject);
    wait = true;
    StructuresData.CompleteResearch(deco.DecorationType);
    StructuresData.SetRevealed(deco.DecorationType);
    UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
    buildMenuController.Show(deco.DecorationType);
    UIBuildMenuController buildMenuController1 = buildMenuController;
    buildMenuController1.OnHidden = buildMenuController1.OnHidden + (System.Action) (() =>
    {
      wait = false;
      buildMenuController = (UIBuildMenuController) null;
    });
    while (wait)
      yield return (object) null;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyWolfBoss.Instance = (EnemyWolfBoss) null;
    this.CleanupCurrentIndicators();
  }

  public void Play()
  {
    if (this.TESTING)
      return;
    this.StartCoroutine((IEnumerator) this.Phase1IE());
  }

  public void RoarPlayerSequence()
  {
    BiomeConstants.Instance.EmitRoarDistortionVFX(this.cameraTarget.transform.position);
    foreach (PlayerFarming player in PlayerFarming.players)
      this.StartCoroutine((IEnumerator) this.RoarPlayerKnocbackSequence(player));
  }

  public IEnumerator RoarPlayerKnocbackSequence(PlayerFarming player)
  {
    player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    string knockbackAnim = !player.isLamb ? "Downed/Goat/knockback-to-downed-goat" : "Downed/knockback-to-downed";
    yield return (object) new WaitForEndOfFrame();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(player.simpleSpineAnimator.Animate(knockbackAnim, 0, false).Animation.Duration, this.Spine);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(player.simpleSpineAnimator.Animate("Downed/idle", 0, false).Animation.Duration, this.Spine);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(player.simpleSpineAnimator.Animate("knockback-reset", 0, false).Animation.Duration, this.Spine);
    player.simpleSpineAnimator.Animate("idle", 0, true);
  }

  public IEnumerator WaitThenActivate(GameObject objectToActivate, float waitTime)
  {
    yield return (object) CoroutineStatics.WaitForScaledSeconds(waitTime, this.Spine);
    objectToActivate?.SetActive(true);
  }

  public IEnumerator Phase1IE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    if (PlayerFarming.Location == FollowerLocation.Boss_Wolf)
      ResurrectOnHud.ResurrectionType = ResurrectionType.None;
    SimulationManager.Pause();
    if (DataManager.Instance.playerDeathsInARow >= 3)
      enemyWolfBoss.health.totalHP -= 100f;
    enemyWolfBoss.health.HP = enemyWolfBoss.health.totalHP;
    UIBossHUD.Play(enemyWolfBoss.health, LocalizationManager.GetTranslation("NAMES/Wolf"));
    enemyWolfBoss.phase1 = true;
    enemyWolfBoss.moving = false;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    if ((UnityEngine.Object) enemyWolfBoss.GetClosestTarget() == (UnityEngine.Object) null)
      yield return (object) null;
    enemyWolfBoss.primaryAttack = enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.StomachStabAttackIE());
    yield return (object) enemyWolfBoss.primaryAttack;
    foreach (TrapFleshRock fleshRock in enemyWolfBoss.fleshRocks)
      enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.WaitThenActivate(fleshRock.gameObject, UnityEngine.Random.Range(0.0f, 4f)));
    while (true)
    {
      while (!((UnityEngine.Object) enemyWolfBoss.GetClosestTarget() == (UnityEngine.Object) null))
      {
        int num = UnityEngine.Random.Range(0, 101);
        enemyWolfBoss.primaryAttack = (double) Vector3.Distance(enemyWolfBoss.transform.position, enemyWolfBoss.GetClosestTarget().transform.position) > 4.0 ? (num >= 50 ? enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.StomachStabAttackIE()) : enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.RotBombsIE())) : (num >= 50 ? enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.MeleeSlamAttackIE()) : enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.MeleeSwipeAttackIE()));
        if (enemyWolfBoss.primaryAttack != null)
        {
          yield return (object) enemyWolfBoss.primaryAttack;
          enemyWolfBoss.primaryAttack = (Coroutine) null;
          enemyWolfBoss.repathTimestamp = 0.0f;
          if ((double) enemyWolfBoss.health.HP <= (double) enemyWolfBoss.phase2Threshold)
          {
            enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.Phase2IE());
            yield break;
          }
          yield return (object) CoroutineStatics.WaitForScaledSeconds(UnityEngine.Random.Range(enemyWolfBoss.timeBetweenAttacks.x, enemyWolfBoss.timeBetweenAttacks.y), enemyWolfBoss.Spine);
        }
      }
      yield return (object) null;
    }
  }

  public void Phase2() => this.StartCoroutine((IEnumerator) this.Phase2IE());

  public IEnumerator Phase2IE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.phase1 = false;
    enemyWolfBoss.phase2 = true;
    enemyWolfBoss.ClearPaths();
    yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.JumpIE(new Vector3(0.0f, 1f / 1000f, 0.0f)));
    enemyWolfBoss.primaryAttack = enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.RevealArmsIE());
    yield return (object) enemyWolfBoss.primaryAttack;
    if ((double) enemyWolfBoss.health.HP <= (double) enemyWolfBoss.phase3Threshold)
    {
      enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.Phase3IE());
    }
    else
    {
      for (int i = 0; i < UnityEngine.Random.Range(3, 5); ++i)
      {
        enemyWolfBoss.primaryAttack = enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmAttackPlayerIE());
        yield return (object) enemyWolfBoss.primaryAttack;
        yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
        if ((double) enemyWolfBoss.health.HP <= (double) enemyWolfBoss.phase3Threshold)
        {
          enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.Phase3IE());
          yield break;
        }
      }
      while (enemyWolfBoss.phase2)
      {
        if ((UnityEngine.Object) enemyWolfBoss.GetClosestTarget() == (UnityEngine.Object) null)
        {
          yield return (object) null;
        }
        else
        {
          int num = UnityEngine.Random.Range(0, 101);
          enemyWolfBoss.primaryAttack = (double) Vector3.Distance(enemyWolfBoss.transform.position, enemyWolfBoss.GetClosestTarget().transform.position) > 5.0 ? (num >= 50 ? enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmLightningRingsAttackIE()) : enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.StomachStabAttackIE())) : (num >= 50 ? enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmCombo2IE()) : enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmCombo1IE()));
          if (enemyWolfBoss.primaryAttack != null)
          {
            yield return (object) enemyWolfBoss.primaryAttack;
            enemyWolfBoss.primaryAttack = (Coroutine) null;
            enemyWolfBoss.repathTimestamp = 0.0f;
            if ((double) enemyWolfBoss.health.HP <= (double) enemyWolfBoss.phase3Threshold)
            {
              enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.Phase3IE());
              break;
            }
            yield return (object) CoroutineStatics.WaitForScaledSeconds(UnityEngine.Random.Range(enemyWolfBoss.timeBetweenAttacks.x, enemyWolfBoss.timeBetweenAttacks.y), enemyWolfBoss.Spine);
          }
        }
      }
    }
  }

  public void Phase3() => this.StartCoroutine((IEnumerator) this.Phase3IE());

  public IEnumerator Phase3IE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.phase2 = false;
    enemyWolfBoss.phase3 = true;
    enemyWolfBoss.primaryAttack = (Coroutine) null;
    yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.JumpIE(new Vector3(0.0f, 1f / 1000f, 0.0f)));
    yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.GrabSidesIE());
    enemyWolfBoss.health.DamageModifier = 1f;
    enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.Phase3AttackLoopIE());
  }

  public IEnumerator Phase3AttackLoopIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    while (true)
    {
      while (!((UnityEngine.Object) enemyWolfBoss.GetClosestTarget() == (UnityEngine.Object) null))
      {
        if ((double) UnityEngine.Random.value < 0.5)
        {
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.LightningBeamAttackIE());
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.BeamShootIE());
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.StomachStabAttackIE());
        }
        else
        {
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.BeamShootIE());
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.LightningBeamAttackIE());
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.StomachStabAttackIE());
        }
        AudioManager.Instance.PlayOneShot(enemyWolfBoss.knockdownStartSFX, enemyWolfBoss.gameObject);
        AudioManager.Instance.PlayOneShot(enemyWolfBoss.knockdownStartVO, enemyWolfBoss.gameObject);
        enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "stagger_start", false);
        enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "stagger_loop", true, 0.0f);
        yield return (object) CoroutineStatics.WaitForScaledSeconds(4f, enemyWolfBoss.Spine);
        AudioManager.Instance.PlayOneShot(enemyWolfBoss.knockdownGetUpSFX, enemyWolfBoss.gameObject);
        AudioManager.Instance.PlayOneShot(enemyWolfBoss.knockdownGetUpVO, enemyWolfBoss.gameObject);
        enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "stagger_stop", false);
        enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
        yield return (object) CoroutineStatics.WaitForScaledSeconds(1.6f, enemyWolfBoss.Spine);
      }
      yield return (object) null;
    }
  }

  public IEnumerator Phase4IE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.phase3 = false;
    enemyWolfBoss.phase4 = true;
    enemyWolfBoss.health.DamageModifier = 1f;
    while (true)
    {
      for (int i = 0; i < 3; ++i)
      {
        yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.JumpIE());
        if ((double) UnityEngine.Random.value < 0.5)
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.StomachStabAttackIE());
        else
          yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.RotBombsIE());
      }
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.knockdownStartSFX, enemyWolfBoss.gameObject);
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.knockdownStartVO, enemyWolfBoss.gameObject);
      enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "stagger_start", false);
      enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "stagger_loop", true, 0.0f);
      foreach (WolfArmParasite armParasite in enemyWolfBoss.armParasites)
        armParasite.Shoot();
      yield return (object) CoroutineStatics.WaitForScaledSeconds(5f, enemyWolfBoss.Spine);
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.knockdownGetUpSFX, enemyWolfBoss.gameObject);
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.knockdownGetUpVO, enemyWolfBoss.gameObject);
      enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "stagger_stop", false);
      enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(1.6f, enemyWolfBoss.Spine);
    }
  }

  public void MeleeSlamAttack() => this.StartCoroutine((IEnumerator) this.MeleeSlamAttackIE());

  public IEnumerator MeleeSlamAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.moving = false;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.Spine.AnimationState.ClearTrack(0);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "bite-attack", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackGroundSlamStartSFX, enemyWolfBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackGroundSlamStartVO, enemyWolfBoss.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.66666668653488159)
    {
      enemyWolfBoss.FlashWhite(time / 0.6666667f);
      yield return (object) null;
    }
    enemyWolfBoss.FlashWhite(0.0f);
    enemyWolfBoss.transform.DOMove(enemyWolfBoss.transform.position + Vector3.down, 0.13333334f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.13333334f, enemyWolfBoss.Spine);
    Vector3 vector3_1 = enemyWolfBoss.transform.position + Vector3.back * 0.1f + Vector3.down + Vector3.left * 3f;
    Vector3 vector3_2 = enemyWolfBoss.transform.position + Vector3.back * 0.1f + Vector3.down + Vector3.right * 3f;
    BiomeConstants.Instance.EmitHammerEffectsInstantiated(vector3_2, customSFX: enemyWolfBoss.attackGroundSlamImpactSFX);
    BiomeConstants.Instance.EmitHammerEffectsInstantiated(vector3_1, customSFX: enemyWolfBoss.attackGroundSlamImpactSFX);
    BiomeConstants.Instance.EmitHammerEffectsInstantiated((vector3_1 + vector3_2) / 2f, customSFX: enemyWolfBoss.attackGroundSlamImpactSFX);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.2f);
    Health.DamageAtPosition(Health.Team.Team2, vector3_1, 2f, 1f);
    Health.DamageAtPosition(Health.Team.Team2, vector3_2, 2f, 1f);
    Health.DamageAtPosition(Health.Team.Team2, (vector3_1 + vector3_2) / 2f, 2f, 1f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.333333343f, enemyWolfBoss.Spine);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.466666669f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public void MeleeSwipeAttack() => this.StartCoroutine((IEnumerator) this.MeleeSwipeAttackIE());

  public IEnumerator MeleeSwipeAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.moving = false;
    enemyWolfBoss.ClearPaths();
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackSlashStartSFX, enemyWolfBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackSlashStartVO, enemyWolfBoss.gameObject);
    enemyWolfBoss.Spine.AnimationState.ClearTrack(0);
    if ((double) enemyWolfBoss.GetClosestTarget().transform.position.x > (double) enemyWolfBoss.transform.position.x)
      enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "swipe-left", false);
    else
      enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "swipe-right", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.66666668653488159)
    {
      enemyWolfBoss.FlashWhite(time / 0.6666667f);
      yield return (object) null;
    }
    enemyWolfBoss.FlashWhite(0.0f);
    enemyWolfBoss.simpleSpineFlash.FlashWhite(0.0f);
    enemyWolfBoss.transform.DOMove(enemyWolfBoss.transform.position + Vector3.down, 0.13333334f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.13333334f, enemyWolfBoss.Spine);
    enemyWolfBoss.meleeDamageCollider.gameObject.SetActive(true);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.333333343f, enemyWolfBoss.Spine);
    enemyWolfBoss.meleeDamageCollider.gameObject.SetActive(false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public void RevealArms() => this.StartCoroutine((IEnumerator) this.RevealArmsIE());

  public IEnumerator RevealArmsIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyWolfBoss.cameraTarget, 10f);
    enemyWolfBoss.health.invincible = true;
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.phaseSpawnBigArmsSFX);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "lose-arm-left", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.25f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.25f);
    enemyWolfBoss.arms[0].Attacking = true;
    enemyWolfBoss.arms[0].Expand(new Vector3(enemyWolfBoss.transform.position.x - 5f, enemyWolfBoss.transform.position.y - 5f, -2f), (System.Action) null, animate: true);
    enemyWolfBoss.armRevealLeftParticle.gameObject.SetActive(true);
    enemyWolfBoss.UpdateArmSkins();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "lose-arm-right", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.25f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.25f);
    enemyWolfBoss.arms[1].Attacking = true;
    enemyWolfBoss.arms[1].Expand(new Vector3(enemyWolfBoss.transform.position.x + 5f, enemyWolfBoss.transform.position.y - 5f, -2f), (System.Action) null, animate: true);
    enemyWolfBoss.armRevealRightParticle.gameObject.SetActive(true);
    enemyWolfBoss.UpdateArmSkins();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "roar", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    enemyWolfBoss.arms[0].SetRootPosition(enemyWolfBoss.arms[0].GetRootPosition() + Vector3.back * 2f + Vector3.up * 4f, 0.9f);
    enemyWolfBoss.arms[1].SetRootPosition(enemyWolfBoss.arms[1].GetRootPosition() + Vector3.back * 2f + Vector3.up * 4f, 0.9f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.9f, enemyWolfBoss.Spine);
    GameManager.GetInstance().OnConversationNext(enemyWolfBoss.cameraTarget, 15f);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 2.4333334f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 2.4333334f);
    enemyWolfBoss.RoarPlayerSequence();
    enemyWolfBoss.arms[0].SetRootPosition(enemyWolfBoss.arms[0].GetRootPosition() + Vector3.forward * 2f + Vector3.down * 4f + Vector3.left * 2f, 0.5f, Ease.OutBack);
    enemyWolfBoss.arms[1].SetRootPosition(enemyWolfBoss.arms[1].GetRootPosition() + Vector3.forward * 2f + Vector3.down * 4f + Vector3.right * 2f, 0.5f, Ease.OutBack);
    enemyWolfBoss.arms[0].JiggleArms(0.2f, 0.3f, 2f);
    enemyWolfBoss.arms[1].JiggleArms(0.2f, 0.3f, 2f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.arms[0].SetRootPosition(enemyWolfBoss.arms[0].RootTargetPosition, 1.9333334f);
    enemyWolfBoss.arms[1].SetRootPosition(enemyWolfBoss.arms[1].RootTargetPosition, 1.9333334f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.9333334f, enemyWolfBoss.Spine);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddToCamera(enemyWolfBoss.Spine.gameObject);
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.arms[0].Attacking = false;
    enemyWolfBoss.arms[1].Attacking = false;
    enemyWolfBoss.moving = true;
    enemyWolfBoss.health.invincible = false;
  }

  public void ExpandArms(System.Action callback)
  {
    this.arms[0].Expand(new Vector3(this.transform.position.x - 5f, this.transform.position.y - 5f, -2f), callback);
    this.arms[1].Expand(new Vector3(this.transform.position.x + 5f, this.transform.position.y - 5f, -2f), (System.Action) null);
    this.UpdateArmSkins();
  }

  public void RetractArms()
  {
    this.arms[0].Retract((System.Action) (() => this.UpdateArmSkins()));
    this.arms[1].Retract();
  }

  public void ArmSpikesMovement() => this.StartCoroutine((IEnumerator) this.ArmSpikesMovementIE());

  public IEnumerator ArmSpikesMovementIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.moving = false;
    enemyWolfBoss.ClearPaths();
    Vector3[] pos1 = new Vector3[5]
    {
      new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(5f, 7f), enemyWolfBoss.transform.position.z),
      new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(3f, 4f), enemyWolfBoss.transform.position.z),
      new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(1f, 2f), enemyWolfBoss.transform.position.z),
      new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(3f, 4f), enemyWolfBoss.transform.position.z),
      new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(5f, 7f), enemyWolfBoss.transform.position.z)
    };
    Vector3[] vector3Array = new Vector3[5];
    vector3Array[4] = new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(1f, 2f), enemyWolfBoss.transform.position.z);
    vector3Array[3] = new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(3f, 4f), enemyWolfBoss.transform.position.z);
    vector3Array[2] = new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(5f, 7f), enemyWolfBoss.transform.position.z);
    vector3Array[1] = new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(3f, 4f), enemyWolfBoss.transform.position.z);
    vector3Array[0] = new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(1f, 2f), enemyWolfBoss.transform.position.z);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSpikesSFX, enemyWolfBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSpikesVO, enemyWolfBoss.gameObject);
    enemyWolfBoss.arms[0].SetArmsSpikey(true);
    enemyWolfBoss.arms[1].SetArmsSpikey(true);
    enemyWolfBoss.arms[0].Attacking = true;
    enemyWolfBoss.arms[1].Attacking = true;
    float dur = 2f;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    UnityEngine.Random.Range(7, 11);
    for (int i = 0; i < pos1.Length; ++i)
    {
      if ((double) UnityEngine.Random.value < 0.5)
      {
        enemyWolfBoss.arms[1].SetRootPosition(EnemyWolfBoss.\u003CArmSpikesMovementIE\u003Eg__GetPosition\u007C195_0(enemyWolfBoss.arms[1].RootTargetPosition, enemyWolfBoss.arms[0].GetRootPosition()), UnityEngine.Random.Range(1f, 2f));
        enemyWolfBoss.arms[0].SetRootPosition(EnemyWolfBoss.\u003CArmSpikesMovementIE\u003Eg__GetPosition\u007C195_0(enemyWolfBoss.arms[0].RootTargetPosition, enemyWolfBoss.arms[1].GetRootPosition()), UnityEngine.Random.Range(1f, 2f));
      }
      else
      {
        enemyWolfBoss.arms[0].SetRootPosition(EnemyWolfBoss.\u003CArmSpikesMovementIE\u003Eg__GetPosition\u007C195_0(enemyWolfBoss.arms[0].RootTargetPosition, enemyWolfBoss.arms[1].GetRootPosition()), UnityEngine.Random.Range(1f, 2f));
        enemyWolfBoss.arms[1].SetRootPosition(EnemyWolfBoss.\u003CArmSpikesMovementIE\u003Eg__GetPosition\u007C195_0(enemyWolfBoss.arms[1].RootTargetPosition, enemyWolfBoss.arms[0].GetRootPosition()), UnityEngine.Random.Range(1f, 2f));
      }
      yield return (object) CoroutineStatics.WaitForScaledSeconds(dur, enemyWolfBoss.Spine);
    }
    enemyWolfBoss.arms[0].Retract(enemyWolfBoss.arms[0].RootTargetPosition);
    enemyWolfBoss.arms[1].Retract(enemyWolfBoss.arms[1].RootTargetPosition);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(dur, enemyWolfBoss.Spine);
    enemyWolfBoss.arms[0].SetArmsSpikey(false);
    enemyWolfBoss.arms[1].SetArmsSpikey(false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public void ArmLightningRingsAttack()
  {
    this.StartCoroutine((IEnumerator) this.ArmLightningRingsAttackIE());
  }

  public IEnumerator ArmLightningRingsAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.moving = false;
    enemyWolfBoss.ClearPaths();
    Vector3[] pos1 = new Vector3[3]
    {
      new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(5f, 7f), enemyWolfBoss.transform.position.z),
      new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(3f, 4f), enemyWolfBoss.transform.position.z),
      new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(1f, 2f), enemyWolfBoss.transform.position.z)
    };
    Vector3[] targets = new Vector3[3]
    {
      new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(5f, 7f), enemyWolfBoss.transform.position.z),
      new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(3f, 4f), enemyWolfBoss.transform.position.z),
      new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(1f, 2f), enemyWolfBoss.transform.position.z)
    };
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSlamMultiStartSFX, enemyWolfBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSlamMultiStartVO, enemyWolfBoss.gameObject);
    enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmAttackMultiIE(enemyWolfBoss.arms[0], "slam-right", targets, (System.Action) null));
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    bool waiting = true;
    enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmAttackMultiIE(enemyWolfBoss.arms[1], "slam-left", pos1, (System.Action) (() => waiting = false)));
    while (waiting)
      yield return (object) null;
    enemyWolfBoss.moving = true;
  }

  public IEnumerator ArmAttackMultiIE(
    WolfBossArm arm,
    string slamAnim,
    Vector3[] targets,
    System.Action callback)
  {
    EnemyWolfBoss enemyWolfBoss = this;
    Vector3 fromPosition = arm.RootTargetPosition;
    Vector3[] vector3Array = targets;
    for (int index = 0; index < vector3Array.Length; ++index)
    {
      Vector3 target = vector3Array[index];
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSlamMultiLiftSFX, enemyWolfBoss.gameObject);
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSlamMultiLiftVO, enemyWolfBoss.gameObject);
      enemyWolfBoss.Spine.AnimationState.SetAnimation(0, slamAnim, false);
      enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      bool waiting = true;
      arm.SmackTarget(target, true, enemyWolfBoss.attackGroundSlamImpactSFX, (System.Action) (() => waiting = false));
      while (waiting)
        yield return (object) null;
      yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    }
    vector3Array = (Vector3[]) null;
    arm.Retract(fromPosition, callback);
  }

  public void ArmAttackPlayer() => this.StartCoroutine((IEnumerator) this.ArmAttackPlayerIE());

  public IEnumerator ArmAttackPlayerIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    Vector3 position = enemyWolfBoss.GetClosestTarget().transform.position;
    if ((double) position.x < 0.0)
    {
      enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "slam-left", false);
      enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      enemyWolfBoss.arms[0].SmackAttack(position + Vector3.left, false, enemyWolfBoss.attackBigArmSlamSingleStartSFX, enemyWolfBoss.attackBigArmSlamSingleStartVO, enemyWolfBoss.attackBigArmSlamImpactSFX, new System.Action(enemyWolfBoss.\u003CArmAttackPlayerIE\u003Eb__200_0));
      enemyWolfBoss.UpdateArmSkins();
    }
    else
    {
      enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "slam-right", false);
      enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      enemyWolfBoss.arms[1].SmackAttack(position + Vector3.right, false, enemyWolfBoss.attackBigArmSlamSingleStartSFX, enemyWolfBoss.attackBigArmSlamSingleStartVO, enemyWolfBoss.attackBigArmSlamImpactSFX, new System.Action(enemyWolfBoss.\u003CArmAttackPlayerIE\u003Eb__200_1));
      enemyWolfBoss.UpdateArmSkins();
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public void ArmSweepAttack() => this.StartCoroutine((IEnumerator) this.ArmSweepAttackIE());

  public IEnumerator ArmSweepAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    bool left = (double) enemyWolfBoss.GetClosestTarget().transform.position.x > (double) enemyWolfBoss.transform.position.x;
    Vector3[] targets = new Vector3[3];
    if (left)
    {
      targets[0] = new Vector3(enemyWolfBoss.transform.position.x + 5f, enemyWolfBoss.transform.position.y - 5f, -2f);
      targets[1] = new Vector3(enemyWolfBoss.transform.position.x, enemyWolfBoss.transform.position.y - 7f, enemyWolfBoss.transform.position.z - 0.5f);
      targets[2] = new Vector3(enemyWolfBoss.transform.position.x - 5f, enemyWolfBoss.transform.position.y - 2f, enemyWolfBoss.transform.position.z - 0.5f);
      enemyWolfBoss.arms[1].SweepAttack(targets, enemyWolfBoss.sweepAttackDuration, 1.16666675f, new System.Action(enemyWolfBoss.\u003CArmSweepAttackIE\u003Eb__202_0));
      enemyWolfBoss.UpdateArmSkins();
    }
    else
    {
      targets[0] = new Vector3(enemyWolfBoss.transform.position.x - 5f, enemyWolfBoss.transform.position.y - 5f, -2f);
      targets[1] = new Vector3(enemyWolfBoss.transform.position.x, enemyWolfBoss.transform.position.y - 7f, -2f);
      targets[2] = new Vector3(enemyWolfBoss.transform.position.x + 5f, enemyWolfBoss.transform.position.y - 2f, -2f);
      enemyWolfBoss.arms[0].SweepAttack(targets, enemyWolfBoss.sweepAttackDuration, 1.16666675f, new System.Action(enemyWolfBoss.\u003CArmSweepAttackIE\u003Eb__202_1));
      enemyWolfBoss.UpdateArmSkins();
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWolfBoss.sweepAttackAnticipation, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, left ? "swipe-left" : "swipe-right", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.66666668653488159)
    {
      enemyWolfBoss.FlashWhite(time / 0.6666667f);
      yield return (object) null;
    }
    enemyWolfBoss.FlashWhite(0.0f);
    enemyWolfBoss.simpleSpineFlash.FlashWhite(0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public IEnumerator ArmSweepLeftIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSpikesSFX, enemyWolfBoss.arms[1].gameObject);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSpikesVO, enemyWolfBoss.gameObject);
    Vector3[] targets = new Vector3[3]
    {
      new Vector3(enemyWolfBoss.transform.position.x + 5f, enemyWolfBoss.transform.position.y - 5f, -2f),
      new Vector3(enemyWolfBoss.transform.position.x, enemyWolfBoss.transform.position.y - 7f, enemyWolfBoss.transform.position.z - 0.5f),
      new Vector3(enemyWolfBoss.transform.position.x - 5f, enemyWolfBoss.transform.position.y - 2f, enemyWolfBoss.transform.position.z - 0.5f)
    };
    enemyWolfBoss.arms[1].SweepAttack(targets, enemyWolfBoss.sweepAttackDuration, 0.6666667f, new System.Action(enemyWolfBoss.\u003CArmSweepLeftIE\u003Eb__203_0));
    enemyWolfBoss.UpdateArmSkins();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWolfBoss.sweepAttackAnticipation, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "swipe-left", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.66666668653488159)
    {
      enemyWolfBoss.FlashWhite(time / 0.6666667f);
      yield return (object) null;
    }
    enemyWolfBoss.FlashWhite(0.0f);
    enemyWolfBoss.simpleSpineFlash.FlashWhite(0.0f);
    enemyWolfBoss.transform.DOMove(enemyWolfBoss.transform.position + Vector3.down, 0.13333334f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(3f, enemyWolfBoss.Spine);
  }

  public IEnumerator ArmSweepRightIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSpikesSFX, enemyWolfBoss.arms[0].gameObject);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackBigArmSpikesVO, enemyWolfBoss.gameObject);
    Vector3[] targets = new Vector3[3]
    {
      new Vector3(enemyWolfBoss.transform.position.x - 5f, enemyWolfBoss.transform.position.y - 5f, -2f),
      new Vector3(enemyWolfBoss.transform.position.x, enemyWolfBoss.transform.position.y - 7f, -2f),
      new Vector3(enemyWolfBoss.transform.position.x + 5f, enemyWolfBoss.transform.position.y - 2f, -2f)
    };
    enemyWolfBoss.arms[0].SweepAttack(targets, enemyWolfBoss.sweepAttackDuration, 0.6666667f, new System.Action(enemyWolfBoss.\u003CArmSweepRightIE\u003Eb__204_0));
    enemyWolfBoss.UpdateArmSkins();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWolfBoss.sweepAttackAnticipation, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "swipe-right", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.66666668653488159)
    {
      enemyWolfBoss.FlashWhite(time / 0.6666667f);
      yield return (object) null;
    }
    enemyWolfBoss.FlashWhite(0.0f);
    enemyWolfBoss.simpleSpineFlash.FlashWhite(0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(3f, enemyWolfBoss.Spine);
  }

  public void ArmCombo1() => this.StartCoroutine((IEnumerator) this.ArmCombo1IE());

  public IEnumerator ArmCombo1IE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    Vector3 vector3 = Vector3.ClampMagnitude(enemyWolfBoss.GetClosestTarget().transform.position, 6f);
    if ((double) vector3.x < 0.0)
    {
      enemyWolfBoss.arms[0].SmackAttack(vector3 + Vector3.left, false, enemyWolfBoss.attackBigArmSlamSingleStartSFX, enemyWolfBoss.attackBigArmSlamSingleStartVO, enemyWolfBoss.attackBigArmSlamImpactSFX, new System.Action(enemyWolfBoss.\u003CArmCombo1IE\u003Eb__206_0));
      enemyWolfBoss.UpdateArmSkins();
      yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyWolfBoss.Spine);
      enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmSweepLeftIE());
    }
    else
    {
      enemyWolfBoss.arms[1].SmackAttack(vector3 + Vector3.right, false, enemyWolfBoss.attackBigArmSlamSingleStartSFX, enemyWolfBoss.attackBigArmSlamSingleStartVO, enemyWolfBoss.attackBigArmSlamImpactSFX, new System.Action(enemyWolfBoss.\u003CArmCombo1IE\u003Eb__206_1));
      enemyWolfBoss.UpdateArmSkins();
      yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyWolfBoss.Spine);
      enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmSweepRightIE());
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(3f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public void ArmCombo2() => this.StartCoroutine((IEnumerator) this.ArmCombo2IE());

  public IEnumerator ArmCombo2IE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    enemyWolfBoss.arms[1].SmackAttack(new Vector3(enemyWolfBoss.transform.position.x + UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(3f, 5f), enemyWolfBoss.transform.position.z), true, enemyWolfBoss.attackBigArmSlamDoubleStartSFX, enemyWolfBoss.attackBigArmSlamDoubleStartVO, enemyWolfBoss.attackBigArmSlamImpactSFX, new System.Action(enemyWolfBoss.\u003CArmCombo2IE\u003Eb__208_0));
    enemyWolfBoss.arms[0].SmackAttack(new Vector3(enemyWolfBoss.transform.position.x - UnityEngine.Random.Range(5f, 8f), enemyWolfBoss.transform.position.y - UnityEngine.Random.Range(3f, 5f), enemyWolfBoss.transform.position.z), true, enemyWolfBoss.attackBigArmSlamDoubleStartSFX, enemyWolfBoss.attackBigArmSlamDoubleStartVO, enemyWolfBoss.attackBigArmSlamImpactSFX, (System.Action) null);
    enemyWolfBoss.UpdateArmSkins();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyWolfBoss.Spine);
    yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.StomachStabAttackIE());
    enemyWolfBoss.moving = true;
  }

  public void StomachStabAttack() => this.StartCoroutine((IEnumerator) this.StomachStabAttackIE());

  public IEnumerator StomachStabAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    Vector3 direction = (enemyWolfBoss.GetClosestTarget().transform.position - enemyWolfBoss.transform.position).normalized;
    direction *= 10f;
    direction.z = -1.5f;
    enemyWolfBoss.arrowIndicator.gameObject.SetActive(true);
    float z = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
    enemyWolfBoss.arrowIndicator.rotation = Quaternion.Euler(0.0f, 0.0f, z);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "worm_attack_start", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "worm-attack-loop", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackTentacleStartSFX, enemyWolfBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackTentacleStartVO, enemyWolfBoss.gameObject);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.53333336114883423)
    {
      enemyWolfBoss.FlashWhite(time / 0.533333361f);
      yield return (object) null;
    }
    enemyWolfBoss.FlashWhite(0.0f);
    enemyWolfBoss.simpleSpineFlash.FlashWhite(0.0f);
    enemyWolfBoss.stomachCreatures[0].StabAttack(enemyWolfBoss.transform.position + direction);
    float timer = 0.0f;
    while ((double) timer <= 2.0)
    {
      if ((double) timer > 0.25 && enemyWolfBoss.arrowIndicator.gameObject.activeSelf)
        enemyWolfBoss.arrowIndicator.gameObject.SetActive(false);
      timer += Time.deltaTime * enemyWolfBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "worm-attack-stop", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public void StomachTripleStabAttack()
  {
    this.StartCoroutine((IEnumerator) this.StomachTripleStabAttackIE());
  }

  public IEnumerator StomachTripleStabAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    float maxLength = 12f;
    Vector3 direction = (enemyWolfBoss.GetClosestTarget().transform.position - enemyWolfBoss.transform.position).normalized;
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "worm_attack_start", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "worm-attack-loop", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.533333361f, enemyWolfBoss.Spine);
    Vector3 vector3_1 = direction * maxLength;
    Vector3 vector3_2 = (direction + new Vector3(0.5f, 0.0f, 0.0f)) * (maxLength - 2f);
    Vector3 vector3_3 = (direction - new Vector3(0.5f, 0.0f, 0.0f)) * (maxLength - 2f);
    vector3_1.z = vector3_2.z = vector3_3.z = -1.5f;
    enemyWolfBoss.stomachCreatures[0].StabAttack(enemyWolfBoss.transform.position + vector3_1);
    enemyWolfBoss.stomachCreatures[1].StabAttack(enemyWolfBoss.transform.position + vector3_2);
    enemyWolfBoss.stomachCreatures[2].StabAttack(enemyWolfBoss.transform.position + vector3_3);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.2f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "worm-attack-stop", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    enemyWolfBoss.moving = true;
  }

  public void StomachProjectileAttack()
  {
    this.StartCoroutine((IEnumerator) this.StomachProjectileAttackIE());
  }

  public IEnumerator StomachProjectileAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    ((enemyWolfBoss.GetClosestTarget().transform.position - enemyWolfBoss.transform.position).normalized * 8f).z = -1.5f;
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "worm_attack_start", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "worm-attack-loop", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.53333336114883423)
    {
      enemyWolfBoss.FlashWhite(time / 0.533333361f);
      yield return (object) null;
    }
    enemyWolfBoss.FlashWhite(0.0f);
    enemyWolfBoss.simpleSpineFlash.FlashWhite(0.0f);
    if (enemyWolfBoss.stomachProjectilePatterns.Length > 1)
    {
      for (int index = 1; index < enemyWolfBoss.stomachProjectilePatterns.Length; ++index)
        enemyWolfBoss.stomachProjectilePatterns[index].Shoot();
    }
    yield return (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.stomachProjectilePatterns[0].ShootIE(0.0f, (GameObject) null, (Transform) null, false));
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "worm-attack-stop", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    enemyWolfBoss.moving = true;
  }

  public void ArmProjectilesAttackRight()
  {
    this.StartCoroutine((IEnumerator) this.ArmProjectileAttackRightIE());
  }

  public IEnumerator ArmProjectileAttackRightIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWolfBoss enemyWolfBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyWolfBoss.UpdateArmSkins();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmProjectileAttackIE(enemyWolfBoss.arms[0], new Vector3(enemyWolfBoss.transform.position.x - 5f, enemyWolfBoss.transform.position.y - 5f, -2f), new System.Action(enemyWolfBoss.\u003CArmProjectileAttackRightIE\u003Eb__216_0)));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void ArmProjectilesAttackLeft()
  {
    this.StartCoroutine((IEnumerator) this.ArmProjectileAttackLeftIE());
  }

  public IEnumerator ArmProjectileAttackLeftIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWolfBoss enemyWolfBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyWolfBoss.UpdateArmSkins();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ArmProjectileAttackIE(enemyWolfBoss.arms[1], new Vector3(enemyWolfBoss.transform.position.x + 5f, enemyWolfBoss.transform.position.y - 5f, -2f), new System.Action(enemyWolfBoss.\u003CArmProjectileAttackLeftIE\u003Eb__218_0)));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator ArmProjectileAttackIE(WolfBossArm arm, Vector3 position, System.Action callback)
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    bool waiting = true;
    arm.Expand(position, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    int amount = (int) UnityEngine.Random.Range(enemyWolfBoss.projectilesToShoot.x, enemyWolfBoss.projectilesToShoot.y + 1f);
    for (int i = 0; i < amount; ++i)
    {
      WolfArmPiece piece = arm.Pieces[UnityEngine.Random.Range(3, arm.Pieces.Length - 3)];
      ObjectPool.Spawn<Projectile>(enemyWolfBoss.projectile, enemyWolfBoss.transform.parent).GetComponent<Projectile>().transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, -1f);
      float num1 = Utils.GetAngle(piece.transform.position, enemyWolfBoss.GetClosestTarget().transform.position);
      if ((double) UnityEngine.Random.value < 0.5)
        num1 = Mathf.Repeat(num1 + 180f, 360f);
      float num2 = Mathf.Repeat(num1 + (float) UnityEngine.Random.Range(-45, 45), 360f);
      enemyWolfBoss.projectile.Angle = num2;
      enemyWolfBoss.projectile.team = enemyWolfBoss.health.team;
      enemyWolfBoss.projectile.Owner = enemyWolfBoss.health;
      enemyWolfBoss.projectile.Speed = enemyWolfBoss.projectileSpeed;
      yield return (object) CoroutineStatics.WaitForScaledSeconds(0.1f, enemyWolfBoss.Spine);
      piece.Spine.AnimationState.SetAnimation(0, "projectiles", false);
      piece.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWolfBoss.delayBetweenProjectiles, enemyWolfBoss.Spine);
      piece = (WolfArmPiece) null;
    }
    waiting = true;
    arm.Retract((System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    enemyWolfBoss.UpdateArmSkins();
    enemyWolfBoss.moving = true;
  }

  public void GrabSides()
  {
    this.phase3 = true;
    this.StartCoroutine((IEnumerator) this.GrabSidesIE());
  }

  public IEnumerator GrabSidesIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.phaseElectrocutedArmsCameraWhooshSFX);
    GameManager.GetInstance().OnConversationNew();
    enemyWolfBoss.health.invincible = true;
    GameManager.GetInstance().OnConversationNext(enemyWolfBoss.cameraTarget, 12f);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.phaseElectrocutedArmsStartSFX);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "electric-anticipate", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.arms[0].Attacking = true;
    enemyWolfBoss.arms[1].Attacking = true;
    enemyWolfBoss.arms[0].SetRootPosition(new Vector3(-7f, 2f, -5f), 1.25f, Ease.OutBack);
    enemyWolfBoss.arms[1].SetRootPosition(new Vector3(7f, 2f, -5f), 1.25f, Ease.OutBack);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "slam-right-electric", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.25f, enemyWolfBoss.Spine);
    enemyWolfBoss.arms[0].BoneFollower.enabled = false;
    GameManager.GetInstance().CameraSetOffset(Vector3.left * 2f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.arms[0].SetRootPosition(new Vector3(-11.82f, -2f, -1.820001f), 0.15f, Ease.InBack);
    enemyWolfBoss.arms[0].SetRootRotation(100f);
    enemyWolfBoss.arms[0].Root.Spine.gameObject.SetActive(false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.15f, enemyWolfBoss.Spine);
    CameraManager.instance.ShakeCameraForDuration(1.75f, 2f, 1f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 1f);
    enemyWolfBoss.grabbedLeftWallParticle.gameObject.SetActive(true);
    enemyWolfBoss.arms[0].BoneFollower.enabled = true;
    enemyWolfBoss.electrocutedArmLoopLeftInstance = AudioManager.Instance.CreateLoop(enemyWolfBoss.electrocutedArmsLoop, enemyWolfBoss.arms[0].gameObject, true);
    enemyWolfBoss.electrocutedArmLoopRightInstance = AudioManager.Instance.CreateLoop(enemyWolfBoss.electrocutedArmsLoop, enemyWolfBoss.arms[1].gameObject, true);
    WolfArmPiece[] wolfArmPieceArray = enemyWolfBoss.arms[0].Pieces;
    int index;
    for (index = 0; index < wolfArmPieceArray.Length; ++index)
    {
      wolfArmPieceArray[index].lightningEffect.SetActive(true);
      yield return (object) new UnityEngine.WaitForSeconds(0.05f);
    }
    wolfArmPieceArray = (WolfArmPiece[]) null;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "slam-left-electric", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.25f, enemyWolfBoss.Spine);
    enemyWolfBoss.arms[1].BoneFollower.enabled = false;
    GameManager.GetInstance().CameraSetOffset(Vector3.right * 2f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.arms[1].SetRootPosition(new Vector3(11.82f, -2f, -1.820001f), 0.15f, Ease.InBack);
    enemyWolfBoss.arms[1].SetRootRotation(-480f);
    enemyWolfBoss.arms[1].Root.Spine.gameObject.SetActive(false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.15f, enemyWolfBoss.Spine);
    CameraManager.instance.ShakeCameraForDuration(1.75f, 2f, 1f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 1f);
    enemyWolfBoss.grabbedRightWallParticle.gameObject.SetActive(true);
    enemyWolfBoss.arms[1].BoneFollower.enabled = true;
    wolfArmPieceArray = enemyWolfBoss.arms[1].Pieces;
    for (index = 0; index < wolfArmPieceArray.Length; ++index)
    {
      wolfArmPieceArray[index].lightningEffect.SetActive(true);
      yield return (object) new UnityEngine.WaitForSeconds(0.05f);
    }
    wolfArmPieceArray = (WolfArmPiece[]) null;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "roar", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.9f, enemyWolfBoss.Spine);
    GameManager.GetInstance().OnConversationNext(enemyWolfBoss.cameraTarget, 15f);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 2.4333334f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 2.4333334f);
    enemyWolfBoss.RoarPlayerSequence();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2.4333334f, enemyWolfBoss.Spine);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddToCamera(enemyWolfBoss.Spine.gameObject);
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    enemyWolfBoss.areSidesGrabbed = true;
    enemyWolfBoss.health.invincible = false;
  }

  public void BreakLeftArm()
  {
    this.leftArmBroken = true;
    this.arms[0].transform.parent = (Transform) null;
    this.armBreakLeftParticle.gameObject.SetActive(true);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForStomachAttackToFinish((System.Action) (() =>
    {
      AudioManager.Instance.PlayOneShot(this.armRippedOffSFX, this.arms[0].gameObject);
      AudioManager.Instance.PlayOneShot(this.armRippedOffVO, this.gameObject);
      Vector3 vector3 = new Vector3(-7f, 3f, -5f);
      this.Spine.AnimationState.SetAnimation(0, "lose-arm-left", false);
      this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      this.arms[0].Pieces[0].Rigidbody.isKinematic = true;
      this.arms[0].Pieces[this.arms[1].Pieces.Length - 1].transform.DOMove(vector3, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.arms[0].Pieces[this.arms[1].Pieces.Length - 1].ResetColour();
      foreach (WolfArmPiece piece in this.arms[0].Pieces)
        piece.Health.invincible = true;
      this.armParasites[0].SetParasite(vector3);
      this.ArmBroken();
    })));
  }

  public void BreakRightArm()
  {
    this.rightArmBroken = true;
    this.arms[1].transform.parent = (Transform) null;
    this.armBreakRightParticle.gameObject.SetActive(true);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForStomachAttackToFinish((System.Action) (() =>
    {
      AudioManager.Instance.PlayOneShot(this.armRippedOffSFX, this.arms[1].gameObject);
      AudioManager.Instance.PlayOneShot(this.armRippedOffVO, this.gameObject);
      Vector3 vector3 = new Vector3(7f, 3f, -5f);
      this.Spine.AnimationState.SetAnimation(0, "lose-arm-right", false);
      this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      this.arms[1].Pieces[0].Rigidbody.isKinematic = true;
      this.arms[1].Pieces[this.arms[1].Pieces.Length - 1].transform.DOMove(vector3, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      this.arms[1].Pieces[this.arms[1].Pieces.Length - 1].ResetColour();
      foreach (WolfArmPiece piece in this.arms[1].Pieces)
        piece.Health.invincible = true;
      this.armParasites[1].SetParasite(vector3);
      this.ArmBroken();
    })));
  }

  public void ArmBroken()
  {
    CameraManager.instance.ShakeCameraForDuration(1.25f, 1.5f, 0.2f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.2f);
    this.StopAllCoroutines();
    foreach (ArrowLightningBeam activeBeam in this.activeBeams)
    {
      if ((UnityEngine.Object) activeBeam != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) activeBeam.gameObject);
    }
    foreach (GameObject activeBeamsVisual in this.activeBeamsVisuals)
    {
      if ((UnityEngine.Object) activeBeamsVisual != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) activeBeamsVisual);
    }
    this.CleanupCurrentIndicators();
    this.activeBeams.Clear();
    this.activeBeamsVisuals.Clear();
    this.arrowIndicator.gameObject.SetActive(false);
    AudioManager.Instance.StopLoop(this.multiLasterInstance);
    if ((double) this.health.HP > 0.0)
      this.StartCoroutine((IEnumerator) this.PhaseCheckPostArmBreak(1.5f));
    this.UpdateArmSkins();
  }

  public IEnumerator PhaseCheckPostArmBreak(float waitTime)
  {
    EnemyWolfBoss enemyWolfBoss = this;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(waitTime, enemyWolfBoss.Spine);
    if (!enemyWolfBoss.TESTING)
    {
      if (enemyWolfBoss.leftArmBroken && enemyWolfBoss.rightArmBroken)
        enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.Phase4IE());
      else
        enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.Phase3AttackLoopIE());
    }
  }

  public IEnumerator WaitForStomachAttackToFinish(System.Action callback)
  {
    while (this.stomachCreatures[0].Active)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void FireAttack() => this.StartCoroutine((IEnumerator) this.FireAttackIE());

  public IEnumerator FireAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    for (int i = 0; (double) i < (double) UnityEngine.Random.Range(enemyWolfBoss.fireAttacks.x, enemyWolfBoss.fireAttacks.y); ++i)
    {
      enemyWolfBoss.Spine.AnimationState.ClearTrack(0);
      enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "slam-both", false);
      enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.83333331346511841)
      {
        enemyWolfBoss.FlashWhite(time / 0.8333333f);
        yield return (object) null;
      }
      enemyWolfBoss.FlashWhite(0.0f);
      enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.SpawnFire());
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWolfBoss.timeBetweenFireAttacks, enemyWolfBoss.Spine);
    }
    enemyWolfBoss.moving = true;
  }

  public IEnumerator SpawnFire()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    Vector3 fromPos = enemyWolfBoss.transform.position + Vector3.down * 2f;
    int row = 0;
    int count = 0;
    int columns = enemyWolfBoss.startingColumns;
    float spread = enemyWolfBoss.startingSpread;
    for (int i = 0; i < 999; ++i)
    {
      TrapLavaTimed fireTrap = enemyWolfBoss.GetFireTrap();
      if (!((UnityEngine.Object) fireTrap == (UnityEngine.Object) null))
      {
        fireTrap.gameObject.SetActive(true);
        fireTrap.ActivateLava();
        fireTrap.transform.position = Vector3.right * 0.5f + Vector3.Lerp(fromPos - Vector3.right * spread, fromPos + Vector3.right * spread, (float) count / (float) columns);
        ++count;
        if (count >= columns)
        {
          columns += enemyWolfBoss.increasePerRow;
          count = 0;
          ++row;
          spread += enemyWolfBoss.spreadIncrease;
          fromPos += Vector3.down * enemyWolfBoss.distanceBetweenRows;
          yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWolfBoss.timeBetweenRows, enemyWolfBoss.Spine);
        }
        if (row >= enemyWolfBoss.totalRows)
          break;
      }
    }
  }

  public TrapLavaTimed GetFireTrap()
  {
    foreach (TrapLavaTimed fireTrap in this.fireTraps)
    {
      if (!fireTrap.gameObject.activeSelf)
        return fireTrap;
    }
    return (TrapLavaTimed) null;
  }

  public void LightningBeamAttack()
  {
    this.StartCoroutine((IEnumerator) this.LightningBeamAttackIE());
  }

  public IEnumerator LightningBeamAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackMultiLaserStartVO, enemyWolfBoss.gameObject);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "beams", true);
    enemyWolfBoss.multiLasterInstance = AudioManager.Instance.CreateLoop(enemyWolfBoss.attackMultiLaserLoopSFX, enemyWolfBoss.gameObject, true);
    List<ArrowLightningBeam> arrowLightningBeamList = new List<ArrowLightningBeam>();
    foreach (BoneFollower eyePosition in enemyWolfBoss.eyePositions)
      enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.BeamFollow(eyePosition.gameObject, enemyWolfBoss.beamDuration));
    yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWolfBoss.beamDuration, enemyWolfBoss.Spine);
    AudioManager.Instance.StopLoop(enemyWolfBoss.multiLasterInstance);
  }

  public IEnumerator BeamFollow(GameObject eyePosition, float duration)
  {
    EnemyWolfBoss enemyWolfBoss = this;
    ArrowLightningBeam beam = (ArrowLightningBeam) null;
    Vector3 startingPosition = enemyWolfBoss.transform.position + Vector3.down * UnityEngine.Random.Range(3f, 5f) + Vector3.right * (float) UnityEngine.Random.Range(-6, 6);
    ArrowLightningBeam.CreateBeam(new Vector3[2]
    {
      eyePosition.transform.position,
      startingPosition
    }, true, 0.5f, enemyWolfBoss.beamDuration, Health.Team.Team2, (Transform) null, true, true, (System.Action<ArrowLightningBeam>) (b =>
    {
      beam = b;
      this.activeBeams.Add(beam);
    }));
    GameObject indicator = ObjectPool.Spawn(enemyWolfBoss.loadedIndicator, enemyWolfBoss.transform.parent);
    enemyWolfBoss.currentIndicators.Add(indicator);
    while ((UnityEngine.Object) beam == (UnityEngine.Object) null)
      yield return (object) null;
    GameObject impact = UnityEngine.Object.Instantiate<GameObject>(enemyWolfBoss.impactPrefab, startingPosition, Quaternion.identity);
    GameObject damage = UnityEngine.Object.Instantiate<GameObject>(enemyWolfBoss.beamDamage, startingPosition, Quaternion.identity);
    enemyWolfBoss.activeBeamsVisuals.Add(impact);
    enemyWolfBoss.activeBeamsVisuals.Add(damage);
    damage.SetActive(true);
    float retargetTimer = UnityEngine.Random.value * 3f;
    Vector3 targetPosition = startingPosition + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(1f, 3f);
    Vector3 position = startingPosition;
    float angle = 0.0f;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < (double) duration)
    {
      angle = Mathf.LerpAngle(angle, Utils.GetAngle(startingPosition, targetPosition), Time.deltaTime * enemyWolfBoss.Spine.timeScale * enemyWolfBoss.beamRotationSpeed);
      Vector3 vector2 = (Vector3) Utils.DegreeToVector2(angle);
      if ((UnityEngine.Object) Physics2D.Raycast((Vector2) position, (Vector2) vector2, 1f, (int) enemyWolfBoss.islandMask).collider != (UnityEngine.Object) null || (double) (position + vector2).y > (double) enemyWolfBoss.transform.position.y - 3.0)
      {
        angle = Mathf.Repeat(angle + 180f, 360f);
        vector2 = (Vector3) Utils.DegreeToVector2(angle);
        targetPosition += vector2 * UnityEngine.Random.Range(1f, 3f);
      }
      position += vector2 * Time.deltaTime * enemyWolfBoss.Spine.timeScale * enemyWolfBoss.beamMoveSpeed;
      Vector3[] positions = new Vector3[2]
      {
        eyePosition.transform.position,
        position
      };
      beam.UpdatePositions(positions);
      impact.transform.position = position;
      damage.transform.position = position;
      indicator.transform.position = position;
      retargetTimer -= Time.deltaTime * enemyWolfBoss.Spine.timeScale;
      if ((double) retargetTimer <= 0.0)
      {
        retargetTimer = UnityEngine.Random.value * 3f;
        targetPosition = startingPosition + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(1f, 3f);
      }
      yield return (object) null;
    }
    enemyWolfBoss.activeBeams.Remove(beam);
    enemyWolfBoss.activeBeamsVisuals.Remove(impact);
    enemyWolfBoss.activeBeamsVisuals.Remove(damage);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "animation", true);
    UnityEngine.Object.Destroy((UnityEngine.Object) damage.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    if ((UnityEngine.Object) indicator != (UnityEngine.Object) null)
      indicator.Recycle();
    if (enemyWolfBoss.currentIndicators.Contains(indicator))
      enemyWolfBoss.currentIndicators.Remove(indicator);
    UnityEngine.Object.Destroy((UnityEngine.Object) impact.gameObject);
  }

  public void BeamShoot() => this.StartCoroutine((IEnumerator) this.BeamShootIE());

  public void CleanupCurrentIndicators()
  {
    foreach (GameObject currentIndicator in this.currentIndicators)
    {
      if ((bool) (UnityEngine.Object) currentIndicator)
        currentIndicator.Recycle();
    }
    this.currentIndicators.Clear();
  }

  public IEnumerator BeamShootIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackFollowingLaserStartVO, enemyWolfBoss.gameObject);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "beams-pulse", true);
    enemyWolfBoss.CleanupCurrentIndicators();
    int amount = UnityEngine.Random.Range(10, 15);
    for (int q = 0; q < amount && !((UnityEngine.Object) enemyWolfBoss.GetClosestTarget() == (UnityEngine.Object) null); ++q)
    {
      GameObject eye = enemyWolfBoss.eyePositions[UnityEngine.Random.Range(0, enemyWolfBoss.eyePositions.Length)].gameObject;
      Vector3 position = eye.transform.position;
      Vector3 target = enemyWolfBoss.GetClosestTarget().transform.position;
      GameObject indicator = ObjectPool.Spawn(enemyWolfBoss.loadedIndicator, enemyWolfBoss.transform.parent, target);
      enemyWolfBoss.currentIndicators.Add(indicator);
      float duration = 0.5f;
      int num = (int) Vector3.Distance(position, target) + 2;
      List<Vector3> positions = new List<Vector3>();
      positions.Add(position);
      for (int index = 1; index < num + 1; ++index)
      {
        float t = (float) index / (float) num;
        positions.Add(Vector3.Lerp(position, target, t) + (Vector3) UnityEngine.Random.insideUnitCircle * 0.2f);
      }
      positions.Add(target);
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackFollowingLaserSingleShotSFX, target);
      for (int index = 0; index < 5; ++index)
        ArrowLightningBeam.CreateBeam(positions.ToArray(), true, 1f, duration, Health.Team.Team2, (Transform) null, true, true, new System.Action<ArrowLightningBeam>(enemyWolfBoss.\u003CBeamShootIE\u003Eb__236_0), signpostWidth: 0.25f);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < (double) duration)
      {
        positions[0] = eye.transform.position;
        foreach (ArrowLightningBeam activeBeam in enemyWolfBoss.activeBeams)
          activeBeam.UpdatePositions(positions.ToArray());
        yield return (object) null;
      }
      BiomeConstants.Instance.EmitHammerEffects(target, 0.3f, 0.5f, 0.2f);
      MMVibrate.HapticAllPlayers(MMVibrate.HapticTypes.HeavyImpact);
      Explosion.CreateExplosion(target, Health.Team.Team2, enemyWolfBoss.health, 2f);
      if ((UnityEngine.Object) indicator != (UnityEngine.Object) null)
      {
        enemyWolfBoss.currentIndicators.Remove(indicator);
        indicator.Recycle();
      }
      enemyWolfBoss.activeBeams.Clear();
      eye = (GameObject) null;
      target = new Vector3();
      indicator = (GameObject) null;
      positions = (List<Vector3>) null;
    }
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "animation", true);
  }

  public void RoofAttack() => this.StartCoroutine((IEnumerator) this.RoofAttackIE());

  public IEnumerator RoofAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    bool waiting = true;
    enemyWolfBoss.arms[0].Expand(enemyWolfBoss.transform.position + Vector3.back * 10f + Vector3.left * 2f + Vector3.up * 2f, (System.Action) null);
    enemyWolfBoss.arms[1].Expand(enemyWolfBoss.transform.position + Vector3.back * 10f + Vector3.right * 2f + Vector3.up * 2f, (System.Action) (() => waiting = false));
    enemyWolfBoss.UpdateArmSkins();
    while (waiting)
      yield return (object) null;
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "roof-hang-start", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "roof-hang-loop", true, 0.0f);
    enemyWolfBoss.health.invincible = true;
    enemyWolfBoss.Spine.transform.DOLocalMove(new Vector3(0.0f, 0.0f, -0.5f), 0.5f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyWolfBoss.Spine);
    for (int i = 0; i < enemyWolfBoss.fleshToSpawn; ++i)
    {
      enemyWolfBoss.SpawnFleshEnemy(enemyWolfBoss.fleshEnemy, enemyWolfBoss.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(0.1f, enemyWolfBoss.Spine);
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.transform.DOLocalMove(new Vector3(0.0f, 0.0f, 0.0f), 0.5f);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "roof-hang-stop", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, enemyWolfBoss.Spine);
    enemyWolfBoss.health.invincible = false;
    waiting = true;
    enemyWolfBoss.arms[0].Retract();
    enemyWolfBoss.arms[1].Retract((System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    enemyWolfBoss.UpdateArmSkins();
    CameraManager.instance.ShakeCameraForDuration(0.75f, 1f, 0.3f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public void StomachSpawnAttack()
  {
    this.StartCoroutine((IEnumerator) this.StomachSpawnAttackIE());
  }

  public IEnumerator StomachSpawnAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    ((enemyWolfBoss.GetClosestTarget().transform.position - enemyWolfBoss.transform.position).normalized * 8f).z = -1.5f;
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "worm_attack_start", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "worm-attack-loop", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyWolfBoss.Spine.timeScale) < 0.53333336114883423)
    {
      enemyWolfBoss.FlashWhite(time / 0.533333361f);
      yield return (object) null;
    }
    enemyWolfBoss.FlashWhite(0.0f);
    enemyWolfBoss.simpleSpineFlash.FlashWhite(0.0f);
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyWolfBoss.segmentedWorm, enemyWolfBoss.stomachPosition.transform.position, Quaternion.identity, enemyWolfBoss.transform.parent);
    enemyWolfBoss.spawnedEnemies.Add(gameObject.GetComponent<Health>());
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "worm-attack-stop", false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    enemyWolfBoss.moving = true;
  }

  public void Jump() => this.StartCoroutine((IEnumerator) this.JumpIE());

  public IEnumerator JumpIE(Vector3 position = default (Vector3))
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    Vector3 p = enemyWolfBoss.transform.position;
    if (position != new Vector3())
    {
      p = position;
    }
    else
    {
      do
      {
        p = new Vector3(0.0f, 0.0f, 0.0f) + (Vector3) UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(enemyWolfBoss.distanceBetweenMovement.x, enemyWolfBoss.distanceBetweenMovement.y);
      }
      while ((double) Vector3.Distance(enemyWolfBoss.transform.position, p) <= (double) enemyWolfBoss.jumpDistance - 2.0);
    }
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.jumpStartSFX, enemyWolfBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.jumpStartVO, enemyWolfBoss.gameObject);
    string anim = (double) p.x > (double) enemyWolfBoss.transform.position.x ? "jump-left" : "jump-right";
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "jump-anticipate", false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.6666667f, enemyWolfBoss.Spine);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, anim, false);
    enemyWolfBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.13333334f, enemyWolfBoss.Spine);
    enemyWolfBoss.health.invincible = true;
    enemyWolfBoss.transform.DOMove(p, 0.8333333f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.4f, enemyWolfBoss.Spine);
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.jumpLandSFX, enemyWolfBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.26f, enemyWolfBoss.Spine);
    CameraManager.instance.ShakeCameraForDuration(0.75f, 1f, 0.3f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.3f);
    enemyWolfBoss.health.invincible = false;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.6666667f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public void Bounce() => this.StartCoroutine((IEnumerator) this.BounceIE());

  public IEnumerator BounceIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    Vector3 position = enemyWolfBoss.transform.position;
    Vector3 insideUnitCircle1;
    Vector3 endValue1;
    do
    {
      insideUnitCircle1 = (Vector3) UnityEngine.Random.insideUnitCircle;
      endValue1 = enemyWolfBoss.transform.position + insideUnitCircle1 * enemyWolfBoss.bounceDistance;
    }
    while (!((UnityEngine.Object) Physics2D.Raycast((Vector2) enemyWolfBoss.transform.position, (Vector2) insideUnitCircle1, enemyWolfBoss.bounceDistance, (int) enemyWolfBoss.islandMask).collider == (UnityEngine.Object) null));
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "jump-double-attack", false);
    enemyWolfBoss.health.invincible = true;
    enemyWolfBoss.transform.DOMove(endValue1, 0.4f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.4f, enemyWolfBoss.Spine);
    enemyWolfBoss.health.invincible = false;
    CameraManager.instance.ShakeCameraForDuration(0.75f, 1f, 0.3f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.3f);
    LightningRingExplosion.CreateExplosion(enemyWolfBoss.transform.position, Health.Team.Team2, enemyWolfBoss.health, enemyWolfBoss.bounceRingSpeed, maxRadiusTarget: enemyWolfBoss.bounceRingMaxRadius);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.13333334f, enemyWolfBoss.Spine);
    Vector3 insideUnitCircle2;
    Vector3 endValue2;
    do
    {
      insideUnitCircle2 = (Vector3) UnityEngine.Random.insideUnitCircle;
      endValue2 = enemyWolfBoss.transform.position + insideUnitCircle2 * enemyWolfBoss.bounceDistance;
    }
    while (!((UnityEngine.Object) Physics2D.Raycast((Vector2) enemyWolfBoss.transform.position, (Vector2) insideUnitCircle2, enemyWolfBoss.bounceDistance, (int) enemyWolfBoss.islandMask).collider == (UnityEngine.Object) null));
    enemyWolfBoss.health.invincible = true;
    enemyWolfBoss.transform.DOMove(endValue2, 0.4f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.4f, enemyWolfBoss.Spine);
    enemyWolfBoss.health.invincible = false;
    CameraManager.instance.ShakeCameraForDuration(0.75f, 1f, 0.3f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.3f);
    LightningRingExplosion.CreateExplosion(enemyWolfBoss.transform.position, Health.Team.Team2, enemyWolfBoss.health, enemyWolfBoss.bounceRingSpeed, maxRadiusTarget: enemyWolfBoss.bounceRingMaxRadius);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.6666667f, enemyWolfBoss.Spine);
    enemyWolfBoss.moving = true;
  }

  public override void givePath(
    Vector3 targetLocation,
    GameObject targetObject = null,
    bool forceAStar = false,
    bool forceIgnoreAStar = false)
  {
    base.givePath(targetLocation, targetObject, forceAStar, forceIgnoreAStar);
    this.Spine.AnimationState.SetAnimation(0, (double) targetLocation.x > (double) this.transform.position.x ? "walk-left" : "walk-right", true);
  }

  public void OnEndOfPath()
  {
    if (this.moving)
      this.Spine.AnimationState.SetAnimation(0, "animation", true);
    this.repathTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.repaithDelay.x, this.repaithDelay.y);
  }

  public void AvalancheAttack() => this.StartCoroutine((IEnumerator) this.AvalancheAttackIE());

  public IEnumerator AvalancheAttackIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.StartCoroutine((IEnumerator) enemyWolfBoss.ShakeCameraWithRampUp(3f, (float) enemyWolfBoss.avalancheAmount * enemyWolfBoss.avalancheTimeBetweenSpawns, 2f));
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyWolfBoss.Spine);
    for (int i = 0; i < enemyWolfBoss.avalancheAmount; ++i)
    {
      TrapAvalanche component = ObjectPool.Spawn<TrapAvalanche>(enemyWolfBoss.trapAvalanche, enemyWolfBoss.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 10f, Quaternion.identity).GetComponent<TrapAvalanche>();
      component.DropDelay = enemyWolfBoss.avalancheDropDelay;
      component.DropSpeed = enemyWolfBoss.avalancheDropSpeed;
      component.Drop();
      if ((double) UnityEngine.Random.value < 0.20000000298023224 || (double) i == (double) enemyWolfBoss.avalancheAmount / 2.0)
        component.transform.position = enemyWolfBoss.GetClosestTarget().transform.position;
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyWolfBoss.avalancheTimeBetweenSpawns, enemyWolfBoss.Spine);
    }
  }

  public void RotBombs() => this.StartCoroutine((IEnumerator) this.RotBombsIE());

  public IEnumerator RotBombsIE()
  {
    EnemyWolfBoss enemyWolfBoss = this;
    enemyWolfBoss.ClearPaths();
    enemyWolfBoss.moving = false;
    AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackMultiBombStartVO, enemyWolfBoss.gameObject);
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "throw-bombs", true);
    for (int i = 0; (double) i < (double) UnityEngine.Random.Range(enemyWolfBoss.rotBombsToSpawn.x, enemyWolfBoss.rotBombsToSpawn.y); ++i)
    {
      Vector3 position1 = enemyWolfBoss.transform.position;
      float num = UnityEngine.Random.Range(enemyWolfBoss.rotBombHeightOffset.x, enemyWolfBoss.rotBombHeightOffset.y);
      Vector3 position2 = new Vector3(position1.x, position1.y + num, position1.z);
      KnockableMortarBomb component = ObjectPool.Spawn(enemyWolfBoss.rotBomb, position2, Quaternion.identity).GetComponent<KnockableMortarBomb>();
      AudioManager.Instance.PlayOneShot(enemyWolfBoss.attackMultiBombLaunchSFX, enemyWolfBoss.gameObject);
      component.transform.position = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-14f, 0.0f), 0.0f);
      component.Play(position2 + new Vector3(0.0f, 0.0f, -1.5f), UnityEngine.Random.Range(enemyWolfBoss.rotBombDuration.x, enemyWolfBoss.rotBombDuration.y), enemyWolfBoss.rotBombExplodeDelay, enemyWolfBoss.health, false);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(UnityEngine.Random.Range(enemyWolfBoss.timeBetweenRotBombs.x, enemyWolfBoss.timeBetweenRotBombs.y), enemyWolfBoss.Spine);
    }
    enemyWolfBoss.Spine.AnimationState.SetAnimation(0, "animation", true);
    enemyWolfBoss.moving = true;
  }

  public Vector3 ClampProjectileTargetPosition(Vector3 target)
  {
    Vector3 closestPoint;
    return BiomeGenerator.PointWithinIsland(target, out closestPoint) ? target : closestPoint + (Vector3.zero - closestPoint).normalized * 2f;
  }

  public void SpawnHoleEnemy()
  {
    List<Vector3> vector3List = new List<Vector3>();
    for (int index = 0; index < this.fleshRocks.Length; ++index)
    {
      if (!((UnityEngine.Object) this.fleshRocks[index] == (UnityEngine.Object) null) && Physics2D.OverlapCircleAll((Vector2) this.fleshRocks[index].transform.position, 2f, (int) this.obstacleMask).Length > 5)
      {
        bool flag = true;
        foreach (EnemyHole enemyHole in this.holeEnemiesSpawned)
        {
          if ((UnityEngine.Object) enemyHole != (UnityEngine.Object) null && (double) Vector3.Distance(enemyHole.transform.position, this.fleshRocks[index].transform.position) < 2.0)
            flag = false;
        }
        if (flag)
          vector3List.Add(this.fleshRocks[index].transform.position);
      }
    }
    if (vector3List.Count > 0)
    {
      EnemyHole enemyHole = UnityEngine.Object.Instantiate<EnemyHole>(this.holeEnemy, vector3List[UnityEngine.Random.Range(0, vector3List.Count)], Quaternion.identity);
      enemyHole.CanHaveModifier = false;
      enemyHole.RemoveModifier();
      this.holeEnemiesSpawned.Add(enemyHole);
    }
    this.holeEnemiesTimer = Time.time + this.timeBetweenHoleEnemies;
  }

  public void CreateBeam(Vector3 from, Vector3 target)
  {
    for (int index1 = 0; index1 < 5; ++index1)
    {
      int num = (int) Vector3.Distance(from, target) + 2;
      List<Vector3> vector3List = new List<Vector3>();
      vector3List.Add(from);
      for (int index2 = 1; index2 < num + 1; ++index2)
      {
        float t = (float) index2 / (float) num;
        Vector3 vector3 = Vector3.Lerp(from, target, t) + (Vector3) UnityEngine.Random.insideUnitCircle * 0.5f;
        vector3List.Add(vector3);
      }
      vector3List.Add(target);
      ArrowLightningBeam.CreateBeam(vector3List.ToArray(), true, 0.75f, 0.5f, Health.Team.Team2, (Transform) null);
    }
  }

  public IEnumerator WaitForSeconds(float duration, System.Action callback)
  {
    yield return (object) CoroutineStatics.WaitForScaledSeconds(duration, this.Spine);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SpawnFleshEnemy(GameObject enemy, Vector3 position)
  {
    EnemyFleshSwarmer e = UnityEngine.Object.Instantiate<GameObject>(enemy, position, Quaternion.identity, this.transform.parent).GetComponent<EnemyFleshSwarmer>();
    e.CanHaveModifier = false;
    e.RemoveModifier();
    e.RevealAllOtherFleshSwarmersOnReveal = false;
    e.revealRange = float.MaxValue;
    e.health.OnDie += (Health.DieAction) ((Attacker, AttackLocation, Victim, AttackType, AttackFlags) => this.spawnedEnemies.Remove(e.health));
    this.spawnedEnemies.Add(e.health);
  }

  public IEnumerator ShakeCameraWithRampUp(float buildUp, float totalDuration, float maxShake)
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime * this.Spine.timeScale) < (double) buildUp)
    {
      float t1 = t / buildUp;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, maxShake / 2f, t1), Mathf.Lerp(0.0f, maxShake, t1), buildUp, false);
      yield return (object) null;
    }
    CameraManager.instance.ShakeCameraForDuration(maxShake / 2f, maxShake, totalDuration - buildUp, false);
  }

  public void FlashWhite(float amount)
  {
    if ((double) amount <= 0.0)
      this.simpleSpineFlash.FlashWhite(false);
    else
      this.simpleSpineFlash.FlashWhite(amount);
    foreach (WolfBossArm arm in this.arms)
    {
      if (arm.Active || (double) amount == 0.0)
      {
        foreach (WolfArmPiece piece in arm.Pieces)
          piece.FlashWhite(amount);
      }
    }
  }

  public void UpdateArmSkins()
  {
    if (this.arms[0].Active && this.arms[1].Active)
      this.Spine.Skeleton.SetSkin("Arms-Off");
    else if (this.arms[0].Active)
      this.Spine.Skeleton.SetSkin("Right-Arm-Off");
    else if (this.arms[1].Active)
      this.Spine.Skeleton.SetSkin("Left-Arm-Off");
    else
      this.Spine.Skeleton.SetSkin("Arms-On");
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    Health component = collision.gameObject.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.DealDamage(component.totalHP, component.gameObject, component.transform.position);
  }

  public IEnumerator SlowMo()
  {
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < 0.699999988079071)
    {
      GameManager.SetTimeScale(0.2f);
      yield return (object) null;
    }
    GameManager.SetTimeScale(1f);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__159_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemyWolfBoss.loadedAddressableAssets.Add(obj);
    this.loadedIndicator = obj.Result;
    this.loadedIndicator.CreatePool(20, true);
  }

  [CompilerGenerated]
  public void \u003CDieIE\u003Eb__170_0(StateMachine state)
  {
    GameManager.GetInstance().WaitForSeconds(0.433333337f, (System.Action) (() => this.Spine.AnimationState.SetAnimation(0, "dead-noheart", true)));
  }

  [CompilerGenerated]
  public void \u003CDieIE\u003Eb__170_2()
  {
    this.Spine.AnimationState.SetAnimation(0, "dead-noheart", true);
  }

  [CompilerGenerated]
  public void \u003CRetractArms\u003Eb__193_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public static Vector3 \u003CArmSpikesMovementIE\u003Eg__GetPosition\u007C195_0(
    Vector3 root,
    Vector3 otherRoot)
  {
    Vector3 a = Vector3.zero;
    do
    {
      a = new Vector3((double) root.x > 0.0 ? root.x + UnityEngine.Random.Range(-5f, 2f) : root.x + UnityEngine.Random.Range(-2f, 5f), root.y + UnityEngine.Random.Range(-2.5f, 2.5f));
    }
    while ((double) Vector3.Distance(a, otherRoot) < 3.0);
    a.z = -0.5f;
    return a;
  }

  [CompilerGenerated]
  public void \u003CArmAttackPlayerIE\u003Eb__200_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmAttackPlayerIE\u003Eb__200_1() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmSweepAttackIE\u003Eb__202_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmSweepAttackIE\u003Eb__202_1() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmSweepLeftIE\u003Eb__203_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmSweepRightIE\u003Eb__204_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmCombo1IE\u003Eb__206_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmCombo1IE\u003Eb__206_1() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmCombo2IE\u003Eb__208_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmProjectileAttackRightIE\u003Eb__216_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CArmProjectileAttackLeftIE\u003Eb__218_0() => this.UpdateArmSkins();

  [CompilerGenerated]
  public void \u003CBreakLeftArm\u003Eb__222_0()
  {
    AudioManager.Instance.PlayOneShot(this.armRippedOffSFX, this.arms[0].gameObject);
    AudioManager.Instance.PlayOneShot(this.armRippedOffVO, this.gameObject);
    Vector3 vector3 = new Vector3(-7f, 3f, -5f);
    this.Spine.AnimationState.SetAnimation(0, "lose-arm-left", false);
    this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    this.arms[0].Pieces[0].Rigidbody.isKinematic = true;
    this.arms[0].Pieces[this.arms[1].Pieces.Length - 1].transform.DOMove(vector3, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.arms[0].Pieces[this.arms[1].Pieces.Length - 1].ResetColour();
    foreach (WolfArmPiece piece in this.arms[0].Pieces)
      piece.Health.invincible = true;
    this.armParasites[0].SetParasite(vector3);
    this.ArmBroken();
  }

  [CompilerGenerated]
  public void \u003CBreakRightArm\u003Eb__223_0()
  {
    AudioManager.Instance.PlayOneShot(this.armRippedOffSFX, this.arms[1].gameObject);
    AudioManager.Instance.PlayOneShot(this.armRippedOffVO, this.gameObject);
    Vector3 vector3 = new Vector3(7f, 3f, -5f);
    this.Spine.AnimationState.SetAnimation(0, "lose-arm-right", false);
    this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    this.arms[1].Pieces[0].Rigidbody.isKinematic = true;
    this.arms[1].Pieces[this.arms[1].Pieces.Length - 1].transform.DOMove(vector3, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.arms[1].Pieces[this.arms[1].Pieces.Length - 1].ResetColour();
    foreach (WolfArmPiece piece in this.arms[1].Pieces)
      piece.Health.invincible = true;
    this.armParasites[1].SetParasite(vector3);
    this.ArmBroken();
  }

  [CompilerGenerated]
  public void \u003CBeamShootIE\u003Eb__236_0(ArrowLightningBeam beam)
  {
    this.activeBeams.Add(beam);
  }
}
