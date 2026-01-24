// Decompiled with JetBrains decompiler
// Type: EnemyYngyaBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using Lamb.UI.DeathScreen;
using MMTools;
using Spine.Unity;
using Spine.Unity.Examples;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyYngyaBoss : UnitObject
{
  public bool TESTING;
  public static EnemyYngyaBoss Instance;
  public UnitObject.Action OnStartTransforming;
  public UnitObject.Action OnFinishingTransformation;
  public SkeletonAnimation Spine;
  [SerializeField]
  public Projectile projectile;
  [SerializeField]
  public float cameraZoom;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public SkeletonAnimation YngyaRecruitSpine;
  public SimpleSpineEventListener spineEventListener;
  [Header("Suicide")]
  [SerializeField]
  public float timeBetweenDamagingSelf;
  [SerializeField]
  public int suicideDamage;
  [Header("Homing Projectiles")]
  [SerializeField]
  public ProjectileYngya projectileHoming;
  [SerializeField]
  public int homingAmount;
  [SerializeField]
  public int homingTimeBetween;
  [SerializeField]
  public int homingRingAmount;
  [SerializeField]
  public float homingRingDelayBetween;
  [SerializeField]
  public float homingRingDelay;
  [Header("Patterns")]
  [SerializeField]
  public ProjectilePattern projectilePattern1;
  [SerializeField]
  public ProjectilePattern projectilePattern2;
  [SerializeField]
  public ProjectilePattern projectilePattern3;
  [SerializeField]
  public ProjectilePattern projectilePattern4;
  [SerializeField]
  public ProjectilePatternBeam projectilePatternBeam1;
  [SerializeField]
  public ProjectilePatternBeam projectilePatternBeam2;
  [Header("Pulsing Projectiles")]
  [SerializeField]
  public ProjectilePattern projectilePatternPulse1;
  [SerializeField]
  public ProjectilePattern projectilePatternPulse2;
  [SerializeField]
  public float pulsingAcceleration;
  [SerializeField]
  public float pulsingDeceleration;
  [SerializeField]
  public float pulseDelay;
  [Header("Fire Arrows")]
  [SerializeField]
  public Projectile arrowFire;
  [SerializeField]
  public float arrowFireSpeed;
  [SerializeField]
  public float arrowTurningSpeed;
  [Header("Chunks")]
  [SerializeField]
  public YngyaChunk projectileRingChunk;
  [SerializeField]
  public YngyaChunk projectileSplatterChunk;
  [SerializeField]
  public int hitsBetweenChunks;
  [SerializeField]
  public Vector2 multiChunkAmount;
  [Header("Multi Chunks")]
  [SerializeField]
  public float multiChunkBuildUp;
  [SerializeField]
  public float multiChunkDuration;
  [SerializeField]
  public float multiChunkMaxShake;
  [SerializeField]
  public float multiChunkTimeBetween;
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
  [Header("Misc")]
  [SerializeField]
  public GameObject followerToSpawn;
  [SerializeField]
  public Interaction_SimpleConversation conversation;
  [SerializeField]
  public GameObject[] weapons;
  [SerializeField]
  public RoomLockController standaloneLockController;
  [Header("Intro")]
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public SimpleSetCamera[] simpleSetCameras;
  [SerializeField]
  public SkeletonAnimation[] ghosts;
  [SerializeField]
  public GameObject[] ghostKnockedOutPositions;
  [SerializeField]
  public SkeletonAnimation genericGhost;
  [SerializeField]
  public Interaction_Generic kickOffInteraction;
  [SerializeField]
  public GrenadeBullet grenadeBullet;
  [SerializeField]
  public GameObject arenaLighting;
  [SerializeField]
  public RoomLockController lockController;
  [SerializeField]
  public GameObject[] torches;
  [SerializeField]
  public Interaction_TeleportHome teleporter;
  [SerializeField]
  public GameObject normalScene;
  [SerializeField]
  public GameObject flashbackScene;
  [SerializeField]
  public ParticleSystem rotGoopParticles;
  [SerializeField]
  public SpawnParticles rotParticles;
  [SerializeField]
  public ParticleSystem yngyaRevealParticles;
  [SerializeField]
  public SpriteRenderer FloorSymbols;
  [SerializeField]
  public SkeletonAnimation introTentacles1;
  [SerializeField]
  public SkeletonAnimation introTentacles2;
  [SerializeField]
  public SkeletonAnimation introTentacles3;
  [SerializeField]
  public SpriteRenderer[] floorRot;
  [SerializeField]
  public GameObject displacement;
  public EventInstance loopingSoundInstance;
  public string holdTime = "hold_time";
  public bool playingHoldDown;
  public int ghostsKnockedOut;
  [CompilerGenerated]
  public bool \u003CSKIP_INTRO\u003Ek__BackingField;
  public bool hasReceivedExternalDamage;
  public float DAMAGE_SELF_MULTIPLIER = 7.5f;
  public const int DAMAGE_SELF_COUNT = 5;
  public int originalFleece;
  public int originalVisualFleece;
  public List<Vector3> ghostsOriginalPositions = new List<Vector3>();
  public string introInteractSFX = "event:/dlc/dungeon06/enemy/yngya/story_intro_interact";
  public string introTransformationSFX = "event:/dlc/dungeon06/enemy/yngya/story_intro_transformation";
  public string introTransformationPostDLCSFX = "event:/dlc/dungeon06/enemy/yngya/story_intro_transformation_postdlc";
  public string introTransformationPostDLCSkip = "event:/dlc/dungeon06/enemy/yngya/story_intro_transformation_postdlc_skip";
  public string attackMultiChunkOneStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_multichunk_start";
  public string attackMultiChunkOneStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_multichunk_start_vo";
  public string attackMultiChunkTwoStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_multichunk_start";
  public string attackMultiChunkTwoStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_multichunk_start_vo";
  public string attackMultiChunkThreeStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_multichunk_start";
  public string attackMultiChunkThreeStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_multichunk_start_vo";
  public string attackMultiChunkThreePartBSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_multichunk_part_b";
  public string attackMultiChunkThreePartBVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_multichunk_part_b_vo";
  public string attackMultiChunkFourStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_multichunk_start";
  public string attackMultiChunkFourStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_multichunk_start_vo";
  public string attackMultiChunkFourPartBSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_multichunk_part_b";
  public string attackMultiChunkFourPartBVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_multichunk_part_b_vo";
  public string attackMultiChunkFourPartCSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_multichunk_part_c";
  public string attackMultiChunkFourPartCVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_multichunk_part_c_vo";
  public string attackProjectileArcOneStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_projectile_arc_start";
  public string attackProjectileArcOneSingleWindupSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_projectile_arc_windup_single";
  public string attackProjectileArcOneSingleWindupVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_projectile_arc_windup_single_vo";
  public string attackProjectileArcTwoStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_projectile_arc_start";
  public string attackProjectileArcTwoStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_projectile_arc_start_vo";
  public string attackProjectileArcThreeStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_projectile_arc_start";
  public string attackProjectileArcThreeStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_projectile_arc_start_vo";
  public string attackProjectileArcFourStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_projectile_arc_start";
  public string attackProjectileArcFourStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_projectile_arc_start_vo";
  public string attackProjectileSpiralOneStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_projectile_spiral_start";
  public string attackProjectileSpiralOneStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_projectile_spiral_start_vo";
  public string attackProjectileSpiralTwoStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_projectile_spiral_start";
  public string attackProjectileSpiralTwoStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_projectile_spiral_start_vo";
  public string attackProjectileSpiralThreeStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_projectile_spiral_start";
  public string attackProjectileSpiralThreeStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_projectile_spiral_start_vo";
  public string attackProjectileSpiralFourStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_projectile_spiral_start";
  public string attackProjectileSpiralFourStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase04_projectile_spiral_start_vo";
  public string attackProjectileCircleTwoStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_projectile_circle_start";
  public string attackProjectileCircleTwoSingleWindupSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_projectile_circle_windup_single";
  public string attackProjectileCircleTwoSingleWindupVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase02_projectile_circle_windup_single_vo";
  public string attackProjectileSpiralStopSFX = "event:/dlc/dungeon06/enemy/yngya/attack_projectile_shared_stop";
  public string attackRotBombsStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_rotbombs_start";
  public string attackRotBombsStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase01_rotbombs_start_vo";
  public string attackFireStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_fire_start";
  public string attackFireStartVO = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_fire_start_vo";
  public string attackFireShootSFX = "event:/dlc/dungeon06/enemy/yngya/attack_phase03_fire_shoot";
  public string attackRoarStartSFX = "event:/dlc/dungeon06/enemy/yngya/attack_roar_start";
  public string getHitBasicVO = "event:/dlc/dungeon06/enemy/yngya/gethit_basic_vo";
  public string getHitSpawnGhostsVO = "event:/dlc/dungeon06/enemy/yngya/gethit_ghostspawn_vo";
  public string getHitMaskBreaksFirstTimeSFX = "event:/dlc/dungeon06/enemy/yngya/gethit_break_head_a";
  public string getHitMaskBreaksSecondTimeSFX = "event:/dlc/dungeon06/enemy/yngya/gethit_break_head_b";
  public string outroDefeatSFX = "event:/dlc/dungeon06/enemy/yngya/story_outro_defeat";
  public string outroDefeatPostDLCSFX = "event:/dlc/dungeon06/enemy/yngya/story_outro_defeat_postdlc";
  public string introAmbientLoopSFX = "event:/dlc/dungeon06/enemy/yngya/story_intro_amb_humming_loop";
  public string introHeartbeatSFX = "event:/dlc/dungeon06/enemy/yngya/story_intro_heartbeat";
  public string introBlinkSFX = "event:/dlc/dungeon06/enemy/yngya/story_intro_blink";
  public string introPreBattleLoopSFX = "event:/dlc/dungeon06/enemy/yngya/story_intro_prebattle_loop";
  public string headBobSFX = "event:/dlc/dungeon06/enemy/yngya/mv_head_bob";
  public string outroTransformIntoFollowerSFX = "event:/dlc/dungeon06/enemy/yngya/story_outro_transform_into_follower";
  public string outroLowerYngyaSFX = "event:/dlc/dungeon06/enemy/yngya/story_outro_lower_yngya_to_ground";
  public string slowMoSnapshotSFX = "snapshot:/slowdown_during_yngya_final_battle";
  public string duckAtmosSnapshot = "snapshot:/duck_atmos_upon_interaction_with_heart_in_yngya_final_battle";
  public EventInstance duckAtmostInstance;
  public EventInstance ambientIntroLoopInstance;
  public EventInstance introPreBattleLoopInstance;
  public EventInstance slowMoSnapshotInstance;
  public EventInstance introPostDLCOneShotInstance;
  public const string spineEventHeartbeat = "heartbeat";
  public const string spineEventBlink = "blink";
  public Coroutine bossIntro;

  public bool SKIP_INTRO
  {
    get => this.\u003CSKIP_INTRO\u003Ek__BackingField;
    set => this.\u003CSKIP_INTRO\u003Ek__BackingField = value;
  }

  public bool isPostDLC => PlayerFarming.Location == FollowerLocation.Dungeon1_6;

  public override void Awake()
  {
    base.Awake();
    this.spineEventListener = this.GetComponent<SimpleSpineEventListener>();
    foreach (Component ghost in this.ghosts)
      this.ghostsOriginalPositions.Add(ghost.transform.position);
    this.TESTING = false;
    if (!this.SKIP_INTRO)
      this.enabled = false;
    if (DataManager.Instance.DiedToYngyaBoss)
      this.SKIP_INTRO = true;
    if ((bool) (UnityEngine.Object) this.spineEventListener)
      this.spineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.ambientIntroLoopInstance = AudioManager.Instance.CreateLoop(this.introAmbientLoopSFX, this.gameObject, true);
    if (this.isPostDLC)
    {
      AudioManager.Instance.PlayMusic("event:/music/death_cat_battle/death_cat_battle");
      AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
      foreach (GameObject weapon in this.weapons)
        weapon.gameObject.SetActive(false);
      this.standaloneLockController.gameObject.SetActive(false);
      this.SKIP_INTRO = true;
    }
    else
    {
      this.originalFleece = DataManager.Instance.PlayerFleece;
      this.originalVisualFleece = DataManager.Instance.PlayerVisualFleece;
      DataManager.Instance.PlayerFleece = 0;
      DataManager.Instance.PlayerVisualFleece = 0;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyYngyaBoss.Instance = this;
  }

  public override void OnDisable()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.slowMoSnapshotInstance, STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.duckAtmostInstance, STOP_MODE.IMMEDIATE);
    base.OnDisable();
    EnemyYngyaBoss.Instance = (EnemyYngyaBoss) null;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!this.isPostDLC)
    {
      DataManager.Instance.PlayerFleece = this.originalFleece;
      DataManager.Instance.PlayerVisualFleece = this.originalVisualFleece;
    }
    SimulationManager.UnPause();
  }

  public void Play()
  {
    this.enabled = true;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(this.gameObject);
    GameManager.GetInstance().AddPlayersToCamera();
    GameManager.GetInstance().CameraSetZoom(this.cameraZoom);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ProceedToYngya);
    if (this.TESTING)
      return;
    this.StartCoroutine((IEnumerator) this.Phase1IE());
  }

  public bool DamageSelf(int amount)
  {
    int num = (double) this.health.HP - (double) amount * (double) this.DAMAGE_SELF_MULTIPLIER <= 0.0 ? 1 : 0;
    this.health.DealDamage((float) amount * this.DAMAGE_SELF_MULTIPLIER, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.NoHitStop);
    return num != 0;
  }

  public IEnumerator Phase1IE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    if (PlayerFarming.Location == FollowerLocation.Boss_Yngya)
      ResurrectOnHud.ResurrectionType = ResurrectionType.None;
    SimulationManager.Pause();
    enemyYngyaBoss.health.enabled = true;
    if (DataManager.Instance.playerDeathsInARow >= 3)
      enemyYngyaBoss.health.totalHP -= 150f;
    enemyYngyaBoss.health.HP = enemyYngyaBoss.health.totalHP;
    enemyYngyaBoss.lockController.DoorUp();
    if (PlayerFarming.players.Count > 1)
    {
      PlayerFarming playerFarming = (PlayerFarming) null;
      float num1 = float.MaxValue;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (!((UnityEngine.Object) player == (UnityEngine.Object) null))
        {
          float num2 = Vector2.Distance((Vector2) enemyYngyaBoss.transform.position, (Vector2) player.transform.position);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            playerFarming = player;
          }
        }
      }
      if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) null)
      {
        foreach (PlayerFarming player in PlayerFarming.players)
        {
          if (!((UnityEngine.Object) player == (UnityEngine.Object) null) && !((UnityEngine.Object) player == (UnityEngine.Object) playerFarming))
          {
            if ((double) Vector2.Distance((Vector2) player.transform.position, (Vector2) playerFarming.transform.position) > 5.0)
              player.transform.position = playerFarming.transform.position;
            else
              player.GoToAndStop(playerFarming.gameObject, IdleOnEnd: true, DisableCollider: true, maxDuration: 5f, forcePositionOnTimeout: true);
          }
        }
      }
    }
    UIBossHUD.Play(enemyYngyaBoss.health, LocalizationManager.GetTranslation("NAMES/Yngya"));
    GameManager.GetInstance().AddToCamera(enemyYngyaBoss.gameObject);
    while (true)
    {
      do
      {
        yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.MultiChunkMiniIE());
        yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyYngyaBoss.Spine);
        enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.DelayedProjectilePatternPulse1(1.16666663f, false));
        AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcOneStartSFX, enemyYngyaBoss.gameObject);
        for (int i = 0; i < UnityEngine.Random.Range(3, 6) * 2; ++i)
        {
          AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcOneSingleWindupSFX, enemyYngyaBoss.gameObject);
          AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcOneSingleWindupVO, enemyYngyaBoss.gameObject);
          enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring", false);
          enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
          yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, enemyYngyaBoss.Spine);
        }
        enemyYngyaBoss.projectilePattern1.StopAllCoroutines();
        if ((double) UnityEngine.Random.Range(0, 101) < 50.0)
          yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.RotBombsIE());
        else
          yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.ProjectileBeam1AttackIE(enemyYngyaBoss.attackProjectileSpiralOneStartSFX, enemyYngyaBoss.attackProjectileSpiralOneStartVO));
        if ((double) enemyYngyaBoss.health.HP / (double) enemyYngyaBoss.health.totalHP <= 0.800000011920929)
        {
          enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.Phase2IE());
          yield break;
        }
      }
      while (enemyYngyaBoss.hasReceivedExternalDamage);
      ++enemyYngyaBoss.DAMAGE_SELF_MULTIPLIER;
    }
  }

  public IEnumerator Phase2IE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackRoarStartSFX, enemyYngyaBoss.gameObject);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "roar", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.4f, enemyYngyaBoss.Spine);
    BiomeConstants.Instance.ImpactFrameForDuration();
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 2.16666675f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 2.16666675f);
    enemyYngyaBoss.Spine.Skeleton.SetSkin("Yngya_1");
    enemyYngyaBoss.Spine.Skeleton.SetSlotsToSetupPose();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2.9333334f, enemyYngyaBoss.Spine);
    while (true)
    {
      do
      {
        yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.MultiChunkMediumIE());
        AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcTwoStartSFX, enemyYngyaBoss.gameObject);
        AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcTwoStartVO, enemyYngyaBoss.gameObject);
        enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-start", false);
        enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "particles-ring-fast-loop", true, 0.0f);
        enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.DelayedProjectilePattern2(1.16666663f));
        for (int i = 0; i < UnityEngine.Random.Range(3, 6) * 2; ++i)
          yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, enemyYngyaBoss.Spine);
        enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-end", false);
        enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
        AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileSpiralStopSFX, enemyYngyaBoss.gameObject);
        enemyYngyaBoss.projectilePattern2.StopAllCoroutines();
        yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyYngyaBoss.Spine);
        if ((double) UnityEngine.Random.Range(0, 101) < 50.0)
          yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.ProjectileBeam1AttackIE(enemyYngyaBoss.attackProjectileSpiralTwoStartSFX, enemyYngyaBoss.attackProjectileSpiralTwoStartVO));
        else
          yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.PulsingProjectileCircleAttackIE());
        if ((double) enemyYngyaBoss.health.HP / (double) enemyYngyaBoss.health.totalHP <= 0.60000002384185791)
        {
          enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.Phase3IE());
          yield break;
        }
      }
      while (enemyYngyaBoss.hasReceivedExternalDamage);
      enemyYngyaBoss.DAMAGE_SELF_MULTIPLIER += 0.75f;
    }
  }

  public IEnumerator DelayedProjectilePattern2(float delay)
  {
    yield return (object) CoroutineStatics.WaitForScaledSeconds(delay, this.Spine);
    this.projectilePattern2.Shoot();
  }

  public IEnumerator Phase3IE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackRoarStartSFX, enemyYngyaBoss.gameObject);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "roar", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.4f, enemyYngyaBoss.Spine);
    BiomeConstants.Instance.ImpactFrameForDuration();
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 2.16666675f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 2.16666675f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.getHitMaskBreaksFirstTimeSFX);
    enemyYngyaBoss.Spine.Skeleton.SetSkin("Yngya_2");
    enemyYngyaBoss.Spine.Skeleton.SetSlotsToSetupPose();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2.9333334f, enemyYngyaBoss.Spine);
    while (true)
    {
      do
      {
        yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.MultiChunkBigIE());
        AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcThreeStartSFX, enemyYngyaBoss.gameObject);
        AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcThreeStartVO, enemyYngyaBoss.gameObject);
        enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-start", false);
        enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "particles-ring-fast-loop", true, 0.0f);
        enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.DelayedProjectilePattern3(1.16666663f));
        for (int i = 0; i < UnityEngine.Random.Range(3, 6) * 2; ++i)
          yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, enemyYngyaBoss.Spine);
        enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-end", false);
        enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
        AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileSpiralStopSFX, enemyYngyaBoss.gameObject);
        enemyYngyaBoss.projectilePattern3.StopAllCoroutines();
        yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyYngyaBoss.Spine);
        float num = (float) UnityEngine.Random.Range(0, 101);
        if ((double) num < 33.0)
          yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.ProjeectileFireAttackIE());
        else if ((double) num < 66.0)
          yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.ProjectileBeam2AttackIE(enemyYngyaBoss.attackProjectileSpiralThreeStartSFX, enemyYngyaBoss.attackProjectileSpiralThreeStartVO));
        if ((double) enemyYngyaBoss.health.HP / (double) enemyYngyaBoss.health.totalHP <= 0.40000000596046448)
        {
          enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.Phase4IE());
          yield break;
        }
      }
      while (enemyYngyaBoss.hasReceivedExternalDamage);
      enemyYngyaBoss.DAMAGE_SELF_MULTIPLIER += 0.5f;
    }
  }

  public IEnumerator DelayedProjectilePattern3(float delay)
  {
    yield return (object) CoroutineStatics.WaitForScaledSeconds(delay, this.Spine);
    this.projectilePattern3.Shoot();
  }

  public IEnumerator Phase4IE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackRoarStartSFX, enemyYngyaBoss.gameObject);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "roar", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.4f, enemyYngyaBoss.Spine);
    BiomeConstants.Instance.ImpactFrameForDuration();
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 2.16666675f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 2.16666675f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.getHitMaskBreaksSecondTimeSFX);
    enemyYngyaBoss.Spine.Skeleton.SetSkin("Yngya_3");
    enemyYngyaBoss.Spine.Skeleton.SetSlotsToSetupPose();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2.9333334f, enemyYngyaBoss.Spine);
    while (true)
    {
      yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.MultiChunkHugeIE());
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcFourStartSFX, enemyYngyaBoss.gameObject);
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileArcFourStartVO, enemyYngyaBoss.gameObject);
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-start", false);
      enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "particles-ring-fast-loop", true, 0.0f);
      enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.DelayedProjectilePattern4(1.16666663f));
      for (int i = 0; i < UnityEngine.Random.Range(3, 6) * 2; ++i)
        yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, enemyYngyaBoss.Spine);
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-end", false);
      enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileSpiralStopSFX, enemyYngyaBoss.gameObject);
      enemyYngyaBoss.projectilePattern4.StopAllCoroutines();
      yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyYngyaBoss.Spine);
      if ((double) UnityEngine.Random.Range(0, 101) < 50.0)
        yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.PulsingProjectileCircleAttack2IE());
      else
        yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.ProjectileBeam2AttackIE(enemyYngyaBoss.attackProjectileSpiralThreeStartSFX, enemyYngyaBoss.attackProjectileSpiralThreeStartVO));
    }
  }

  public IEnumerator DelayedProjectilePattern4(float delay)
  {
    yield return (object) CoroutineStatics.WaitForScaledSeconds(delay, this.Spine);
    this.projectilePattern4.Shoot();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) this.gameObject && AttackType != Health.AttackTypes.NoKnockBack)
    {
      Debug.Log((object) $"Detected hit from non-Yngya Game Object:{Attacker.name} (Yngya={this.gameObject.name})");
      this.hasReceivedExternalDamage = true;
    }
    ISpellOwning component;
    if (Attacker.TryGetComponent<ISpellOwning>(out component))
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) component.GetOwner() == (UnityEngine.Object) player.gameObject)
        {
          this.hasReceivedExternalDamage = true;
          break;
        }
      }
    }
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.simpleSpineFlash?.FlashFillRed();
    AudioManager.Instance.PlayOneShot(this.ReleaseGhostNPC() ? this.getHitSpawnGhostsVO : this.getHitBasicVO);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (this.isPostDLC)
      AudioManager.Instance.PlayOneShot(this.outroDefeatPostDLCSFX);
    else
      AudioManager.Instance.PlayOneShot(this.outroDefeatSFX);
    AudioManager.Instance.SetMusicParam("deathcat_room_id", 5f);
    this.yngyaRevealParticles.Play();
    this.ReleaseGhostNPC();
    this.StopAllCoroutines();
    this.projectilePattern1.StopAllCoroutines();
    this.projectilePattern2.StopAllCoroutines();
    this.projectilePattern3.StopAllCoroutines();
    this.projectilePattern4.StopAllCoroutines();
    this.projectilePatternBeam1.StopAllCoroutines();
    this.projectilePatternBeam2.StopAllCoroutines();
    this.projectilePatternPulse1.StopAllCoroutines();
    this.projectilePatternPulse2.StopAllCoroutines();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.cameraTarget);
    DataManager.Instance.BossesCompleted.Add(FollowerLocation.Dungeon1_5);
    DataManager.Instance.BossesCompleted.Add(FollowerLocation.Dungeon1_6);
    UIBossHUD.Hide();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.OnDieIE());
  }

  public IEnumerator OnDieIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SlowMo());
    HUD_Manager.Instance.Hide(false);
    Debug.Log((object) $"Has Yngya been damaged by anything other then herself? {enemyYngyaBoss.hasReceivedExternalDamage}");
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.BEAT_YNGYA));
    if (!enemyYngyaBoss.hasReceivedExternalDamage)
    {
      Debug.Log((object) "Attempting to award the 'yngya died from purely suicide damage' cheevo");
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.BEAT_YNGYA_NOATTACK));
      Debug.Log((object) "Cheevo unlocked");
    }
    yield return (object) new WaitForEndOfFrame();
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "die-start", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "die-loop2", true, 0.0f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 4f);
    if (DataManager.Instance.BeatenYngya)
    {
      yield return (object) new WaitForSeconds(4f);
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "die", false);
      enemyYngyaBoss.yngyaRevealParticles.Play();
      yield return (object) new WaitForSeconds(1f);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.teleporter.gameObject);
      AudioManager.Instance.PlayOneShot("event:/ui/camera_whoosh");
      yield return (object) new WaitForSeconds(1f);
      enemyYngyaBoss.teleporter.CachePosition = enemyYngyaBoss.teleporter.transform.position;
      enemyYngyaBoss.teleporter.EnableTeleporter();
      yield return (object) new WaitForSeconds(2f);
      GameManager.GetInstance().OnConversationEnd();
    }
    else
    {
      DataManager.Instance.BeatenYngya = true;
      PlayerFarming playerFarming = PlayerFarming.Instance;
      playerFarming.GoToAndStop(new Vector3(0.0f, 70f, 0.0f), enemyYngyaBoss.gameObject);
      yield return (object) new WaitForSeconds(2f);
      enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SpawnGhostsOut(180, 0.02f));
      GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.cameraTarget, 12f);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 5f);
      MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 5f);
      BiomeConstants.Instance.ImpactFrameForDuration();
      enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SpawnSplatter(1000, 30f, enemyYngyaBoss.cameraTarget.transform.position));
      yield return (object) new WaitForSeconds(5f);
      for (int index = 0; index < enemyYngyaBoss.ghosts.Length; ++index)
      {
        Vector3 endValue = (enemyYngyaBoss.transform.position + (enemyYngyaBoss.ghosts[index].transform.position - enemyYngyaBoss.transform.position).normalized * 7.5f) with
        {
          z = -0.5f
        };
        enemyYngyaBoss.ghosts[index].transform.DOMove(endValue, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>((float) index * 0.2f);
      }
      enemyYngyaBoss.cameraTarget.transform.position += new Vector3(0.0f, 0.0f, 1f);
      GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.cameraTarget, 14f);
      yield return (object) new WaitForSeconds(4f);
      ConversationObject conv = new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(enemyYngyaBoss.ghosts[0].gameObject, "Conversation_NPC/Yngya/PostFight/0")
      }, (List<MMTools.Response>) null, (System.Action) null);
      for (int i = 0; i < 6; ++i)
      {
        conv.Entries[0].Speaker = enemyYngyaBoss.ghosts[i].gameObject.gameObject;
        conv.Entries[0].TermToSpeak = $"Conversation_NPC/Yngya/PostFight/{i}";
        CameraManager.instance.ShakeCameraForDuration(0.3f, 0.6f, 0.2f);
        MMConversation.PlayBark(conv);
        yield return (object) new WaitForSeconds(5f);
      }
      MMConversation.mmConversation.Close();
      yield return (object) new WaitForSeconds(0.5f);
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "die", false);
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.outroTransformIntoFollowerSFX, enemyYngyaBoss.gameObject);
      GameManager.GetInstance().WaitForSeconds(1f, new System.Action(enemyYngyaBoss.\u003COnDieIE\u003Eb__171_0));
      enemyYngyaBoss.YngyaRecruitSpine.gameObject.SetActive(true);
      enemyYngyaBoss.YngyaRecruitSpine.Skeleton.A = 0.0f;
      DOTween.To(new DOGetter<float>(enemyYngyaBoss.\u003COnDieIE\u003Eb__171_1), new DOSetter<float>(enemyYngyaBoss.\u003COnDieIE\u003Eb__171_2), 1f, 1f);
      AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
      GameObject Follower = UnityEngine.Object.Instantiate<GameObject>(enemyYngyaBoss.followerToSpawn, new Vector3(-0.22f, 73.42f, -0.86f), Quaternion.identity);
      Interaction_FollowerSpawn followerSpawn = Follower.GetComponent<Interaction_FollowerSpawn>();
      followerSpawn.Play("Yngya", ScriptLocalization.NAMES.Yngya, moveToZero: false);
      followerSpawn.AutomaticallyInteract = false;
      followerSpawn.Interactable = false;
      followerSpawn.DisableOnHighlighted = true;
      followerSpawn.EndIndicateHighlighted(playerFarming);
      followerSpawn.transform.localScale = new Vector3(-1f, 1f, 1f);
      enemyYngyaBoss.ReplaceYngyaFollowerDefaultSFX(Follower);
      followerSpawn.Spine.AnimationState.SetAnimation(0, "yngya/floating-appear", false);
      followerSpawn.Spine.AnimationState.AddAnimation(0, "yngya/floating-worried", true, 0.0f);
      DataManager.SetFollowerSkinUnlocked("Yngya");
      while (LetterBox.IsPlaying)
        yield return (object) null;
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(Follower.gameObject, 4f);
      string[] strArray = new string[3]
      {
        "yngya/floating-worried",
        "yngya/floating-talk",
        "yngya/floating-talk2"
      };
      for (int index = 0; index < enemyYngyaBoss.conversation.Entries.Count; ++index)
      {
        ConversationEntry entry = enemyYngyaBoss.conversation.Entries[index];
        entry.Speaker = followerSpawn.Spine.gameObject;
        entry.SkeletonData = followerSpawn.Spine;
        entry.Animation = strArray[index % strArray.Length];
        entry.pitchValue = followerSpawn._followerInfo.follower_pitch;
        entry.vibratoValue = followerSpawn._followerInfo.follower_vibrato;
        entry.followerID = followerSpawn._followerInfo.ID;
        entry.soundPath = "event:/dlc/dialogue/yngya/follower_general_talk";
      }
      enemyYngyaBoss.conversation.Entries[0].Animation = "yngya/floating-worried";
      enemyYngyaBoss.conversation.Entries[0].soundPath = " ";
      yield return (object) new WaitForEndOfFrame();
      enemyYngyaBoss.conversation.Play(playerFarming.gameObject);
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().CamFollowTarget.TargetOffset = new Vector3(0.0f, 0.0f, 0.3f);
      while (LetterBox.IsPlaying)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(Follower.gameObject, 4f);
      yield return (object) new WaitForEndOfFrame();
      followerSpawn.Spine.AnimationState.SetAnimation(0, "yngya/floating-finish", false);
      followerSpawn.Spine.AnimationState.AddAnimation(0, "Worship/worship", true, 0.0f);
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.outroLowerYngyaSFX, followerSpawn.Spine.gameObject);
      followerSpawn.transform.DOMoveZ(0.0f, 1f);
      DOTween.To(new DOGetter<float>(enemyYngyaBoss.\u003COnDieIE\u003Eb__171_3), new DOSetter<float>(enemyYngyaBoss.\u003COnDieIE\u003Eb__171_4), 0.0f, 1f);
      yield return (object) new WaitForSeconds(3f);
      GameManager.GetInstance().OnConversationNew();
      if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA))
        yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.PlayerPickUpThrophyDeco(followerSpawn.transform.position));
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(Follower.gameObject, 4f);
      GameManager.GetInstance().CamFollowTarget.TargetOffset = new Vector3(0.0f, 0.0f, 0.3f);
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) playerFarming)
        {
          if ((double) Vector3.Distance(player.transform.position, playerFarming.transform.position) > 7.5)
            player.transform.position = playerFarming.transform.position + Vector3.right;
          else
            player.GoToAndStop(playerFarming.transform.position + Vector3.right);
        }
      }
      GameManager.GetInstance().OnConversationNext(followerSpawn.gameObject, 6f);
      playerFarming.GoToAndStop(Follower.transform.position + Vector3.right * 1.5f, followerSpawn.gameObject);
      while (PlayerFarming.Instance.GoToAndStopping)
        yield return (object) null;
      followerSpawn.Spine.GetComponent<SimpleSpineAnimator>().enabled = false;
      yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) followerSpawn.ConvertFollower());
      enemyYngyaBoss.YngyaRecruitSpine.gameObject.SetActive(false);
      AstarPath.active = (AstarPath) null;
      DataManager.Instance.DiedLastRun = false;
      DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.None;
      for (int index = 0; index < DataManager.Instance.Followers_Demons_IDs.Count; ++index)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index]);
        if (infoById != null)
          FollowerBrain.GetOrCreateBrain(infoById)?.AddThought(Thought.DemonSuccessfulRun);
      }
      DataManager.Instance.Followers_Demons_IDs.Clear();
      DataManager.Instance.Followers_Demons_Types.Clear();
      if (!UIDeathScreenOverlayController.UsedBossPortal)
        DataManager.CompleteDungeonMapNode();
      else
        UIDeathScreenOverlayController.UsedBossPortal = false;
      AudioManager.Instance.StopCurrentAtmos();
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Credits", 2f, "", (System.Action) (() =>
      {
        DataManager.ResetRunData();
        SaveAndLoad.Save();
      }));
    }
  }

  public void StopBehaviour() => this.StopAllCoroutines();

  public void ProjectileFireAttack()
  {
    this.StartCoroutine((IEnumerator) this.ProjeectileFireAttackIE());
  }

  public IEnumerator ProjeectileFireAttackIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    float angle = (float) UnityEngine.Random.Range(0, 360);
    int dir = (double) UnityEngine.Random.value < 0.5 ? 1 : -1;
    for (int x = 0; x < 1; ++x)
    {
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackFireStartSFX, enemyYngyaBoss.gameObject);
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackFireStartVO, enemyYngyaBoss.gameObject);
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "fire-attack", false);
      enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(1.13333333f, enemyYngyaBoss.Spine);
      int num = 5;
      for (int index = 0; index < num; ++index)
      {
        AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackFireShootSFX, enemyYngyaBoss.gameObject);
        Projectile component = ObjectPool.Spawn<Projectile>(enemyYngyaBoss.arrowFire, enemyYngyaBoss.transform.parent, enemyYngyaBoss.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)).GetComponent<Projectile>();
        component.transform.position = enemyYngyaBoss.transform.position - Vector3.forward * 0.2f;
        component.Angle = angle;
        component.team = Health.Team.Team2;
        component.Owner = enemyYngyaBoss.health;
        component.Speed = enemyYngyaBoss.arrowFireSpeed;
        component.AngleIncrement = enemyYngyaBoss.arrowTurningSpeed * (float) dir;
        angle += 360f / (float) num;
      }
      angle += 45f;
    }
  }

  public void ProjectileBeam1Attack()
  {
    this.StartCoroutine((IEnumerator) this.ProjectileBeam1AttackIE(this.attackProjectileSpiralOneStartSFX, this.attackProjectileSpiralOneStartVO));
  }

  public IEnumerator ProjectileBeam1AttackIE(string startSFX, string startVO)
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    AudioManager.Instance.PlayOneShot(startSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(startVO, enemyYngyaBoss.gameObject);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-start", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "particles-ring-fast-loop", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyYngyaBoss.Spine);
    yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.projectilePatternBeam1.ShootIE(0.0f, (GameObject) null, (Transform) null, true));
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-end", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileSpiralStopSFX, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyYngyaBoss.Spine);
  }

  public void ProjectileBeam2Attack()
  {
    this.StartCoroutine((IEnumerator) this.ProjectileBeam2AttackIE(this.attackProjectileSpiralTwoStartSFX, this.attackProjectileSpiralTwoStartVO));
  }

  public IEnumerator ProjectileBeam2AttackIE(string startSFX, string startVO)
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    AudioManager.Instance.PlayOneShot(startSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(startVO, enemyYngyaBoss.gameObject);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-start", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "particles-ring-fast-loop", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyYngyaBoss.Spine);
    yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.projectilePatternBeam2.ShootIE(0.0f, (GameObject) null, (Transform) null, true));
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-end", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileSpiralStopSFX, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyYngyaBoss.Spine);
  }

  public void PulsingProjectileAttack()
  {
    this.StartCoroutine((IEnumerator) this.PulsingProjectileCircleAttackIE());
  }

  public IEnumerator PulsingProjectileCircleAttackIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.DelayedProjectilePatternPulse1(1f, true));
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileCircleTwoStartSFX, enemyYngyaBoss.gameObject);
    for (int i = 0; i < 6; ++i)
    {
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileCircleTwoSingleWindupSFX, enemyYngyaBoss.gameObject);
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileCircleTwoSingleWindupVO, enemyYngyaBoss.gameObject);
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring", false);
      enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyYngyaBoss.Spine);
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyYngyaBoss.Spine);
  }

  public IEnumerator DelayedProjectilePatternPulse1(float delay, bool registerEvent)
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(delay, enemyYngyaBoss.Spine);
    if (registerEvent)
    {
      enemyYngyaBoss.projectilePatternPulse1.OnProjectileWaveShot -= new ProjectilePattern.ProjectileWaveEvent(enemyYngyaBoss.PulsingProjectileWaveShot);
      enemyYngyaBoss.projectilePatternPulse1.OnProjectileWaveShot += new ProjectilePattern.ProjectileWaveEvent(enemyYngyaBoss.PulsingProjectileWaveShot);
    }
    enemyYngyaBoss.projectilePatternPulse1.Shoot();
  }

  public void PulsingProjectileAttack2()
  {
    this.StartCoroutine((IEnumerator) this.PulsingProjectileCircleAttack2IE());
  }

  public IEnumerator PulsingProjectileCircleAttack2IE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-start", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "particles-ring-fast-loop", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileSpiralFourStartSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileSpiralFourStartVO, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyYngyaBoss.Spine);
    enemyYngyaBoss.projectilePatternPulse2.OnProjectileWaveShot -= new ProjectilePattern.ProjectileWaveEvent(enemyYngyaBoss.PulsingProjectileWaveShot);
    enemyYngyaBoss.projectilePatternPulse2.OnProjectileWaveShot += new ProjectilePattern.ProjectileWaveEvent(enemyYngyaBoss.PulsingProjectileWaveShot);
    enemyYngyaBoss.projectilePatternPulse2.Shoot();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(UnityEngine.Random.Range(6f, 8f), enemyYngyaBoss.Spine);
    enemyYngyaBoss.projectilePatternPulse2.StopAllCoroutines();
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "particles-ring-fast-end", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackProjectileSpiralStopSFX, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyYngyaBoss.Spine);
  }

  public void PulsingProjectileWaveShot(ProjectilePattern.BulletWave wave)
  {
    this.StartCoroutine((IEnumerator) this.PulsingProjectileWaveIE(wave));
  }

  public IEnumerator PulsingProjectileWaveIE(ProjectilePattern.BulletWave wave)
  {
    foreach (Projectile spawnedProjectile in wave.SpawnedProjectiles)
      spawnedProjectile.Acceleration = this.pulsingAcceleration;
label_3:
    yield return (object) CoroutineStatics.WaitForScaledSeconds(this.pulseDelay, this.Spine);
    for (int index = 0; index < wave.SpawnedProjectiles.Length; ++index)
    {
      if ((UnityEngine.Object) wave.SpawnedProjectiles[index] != (UnityEngine.Object) null)
      {
        wave.SpawnedProjectiles[index].Acceleration = 0.0f;
        wave.SpawnedProjectiles[index].Deceleration = this.pulsingDeceleration;
      }
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(this.pulseDelay, this.Spine);
    for (int index = 0; index < wave.SpawnedProjectiles.Length; ++index)
    {
      if ((UnityEngine.Object) wave.SpawnedProjectiles[index] != (UnityEngine.Object) null)
      {
        wave.SpawnedProjectiles[index].Acceleration = this.pulsingAcceleration;
        wave.SpawnedProjectiles[index].Deceleration = 0.0f;
      }
    }
    goto label_3;
  }

  public void RotBombs() => this.StartCoroutine((IEnumerator) this.RotBombsIE());

  public IEnumerator RotBombsIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackRotBombsStartSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackRotBombsStartVO, enemyYngyaBoss.gameObject);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big-start", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "throw-flesh-big-loop", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.66f, enemyYngyaBoss.Spine);
    for (int i = 0; (double) i < (double) UnityEngine.Random.Range(enemyYngyaBoss.rotBombsToSpawn.x, enemyYngyaBoss.rotBombsToSpawn.y); ++i)
    {
      Vector3 position1 = enemyYngyaBoss.transform.position;
      float num = UnityEngine.Random.Range(enemyYngyaBoss.rotBombHeightOffset.x, enemyYngyaBoss.rotBombHeightOffset.y);
      Vector3 position2 = new Vector3(position1.x, position1.y + num, position1.z);
      KnockableMortarBomb component = ObjectPool.Spawn(enemyYngyaBoss.rotBomb, position2, Quaternion.identity).GetComponent<KnockableMortarBomb>();
      component.transform.position = enemyYngyaBoss.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 7f;
      component.Play(position2 + new Vector3(0.0f, 0.0f, -1.5f), UnityEngine.Random.Range(enemyYngyaBoss.rotBombDuration.x, enemyYngyaBoss.rotBombDuration.y), enemyYngyaBoss.rotBombExplodeDelay, enemyYngyaBoss.health, false);
      if (i % 5 == 0 && enemyYngyaBoss.DamageSelf(1))
        yield break;
      yield return (object) CoroutineStatics.WaitForScaledSeconds(UnityEngine.Random.Range(enemyYngyaBoss.timeBetweenRotBombs.x, enemyYngyaBoss.timeBetweenRotBombs.y), enemyYngyaBoss.Spine);
    }
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big-end", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.headBobSFX, enemyYngyaBoss.gameObject);
  }

  public void SpawnProjectileRingChunk() => this.SpawnChunk(this.projectileRingChunk);

  public void SpawnProjectileSplatterChunk() => this.SpawnChunk(this.projectileSplatterChunk);

  public void SpawnRandomChunk()
  {
    if ((double) UnityEngine.Random.value < 0.5)
      this.SpawnChunk(this.projectileRingChunk);
    else
      this.SpawnChunk(this.projectileSplatterChunk);
  }

  public void SpawnChunk(YngyaChunk chunk, Vector3 target = default (Vector3), bool animate = true)
  {
    this.StartCoroutine((IEnumerator) this.SpawnChunkIE(chunk, target, animate));
  }

  public IEnumerator SpawnChunkIE(YngyaChunk chunk, Vector3 target = default (Vector3), bool animate = true)
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    if (animate)
    {
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh" + ((double) UnityEngine.Random.value < 0.5 ? "" : "2"), false);
      enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(1.26666665f, enemyYngyaBoss.Spine);
    }
    YngyaChunk yngyaChunk = UnityEngine.Object.Instantiate<YngyaChunk>(chunk, enemyYngyaBoss.transform.position, Quaternion.identity);
    Vector3 a = enemyYngyaBoss.transform.position;
    while ((double) Vector3.Distance(a, enemyYngyaBoss.transform.position) < 3.0)
    {
      a = enemyYngyaBoss.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 8f;
      if ((double) Mathf.Abs(a.x - enemyYngyaBoss.transform.position.x) < 4.0 && (double) a.y > 25.0)
        a = enemyYngyaBoss.transform.position;
    }
    yngyaChunk.Configure(target != new Vector3() ? target : a);
  }

  public void MultiChunkHuge() => this.StartCoroutine((IEnumerator) this.MultiChunkHugeIE());

  public IEnumerator MultiChunkHugeIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkFourStartSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkFourStartVO, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.33f, enemyYngyaBoss.Spine);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.right * 4f + Vector3.up * 6f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.left * 4f + Vector3.up * 6f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.right * 4f + Vector3.down * 6f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.left * 4f + Vector3.down * 6f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, enemyYngyaBoss.Spine);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkFourPartBSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkFourPartBVO, enemyYngyaBoss.gameObject);
    List<Vector3> targetPositions = new List<Vector3>();
    float degree = UnityEngine.Random.Range(0.0f, 360f);
    int amount = 20;
    for (int index = 0; index < amount; ++index)
    {
      Vector3 vector3 = enemyYngyaBoss.transform.position + (Vector3) Utils.DegreeToVector2(degree) * UnityEngine.Random.Range(7f, 8.5f);
      degree += (float) (360 / amount);
      targetPositions.Add(vector3);
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.33f, enemyYngyaBoss.Spine);
    targetPositions.Shuffle<Vector3>();
    for (int i = 0; i < amount; ++i)
    {
      enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileSplatterChunk, targetPositions[0], false);
      targetPositions.RemoveAt(0);
      if (i % 5 == 0 && enemyYngyaBoss.DamageSelf(1))
        yield break;
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyYngyaBoss.multiChunkTimeBetween, enemyYngyaBoss.Spine);
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2.5f, enemyYngyaBoss.Spine);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkFourPartCSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkFourPartCVO, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.33f, enemyYngyaBoss.Spine);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.right * 4.5f + Vector3.up * 6f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.left * 4.5f + Vector3.up * 6f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.right * 4.5f + Vector3.down * 6f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.left * 4.5f + Vector3.down * 6f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    if (!enemyYngyaBoss.DamageSelf(3))
    {
      CameraManager.instance.ShakeCameraForDuration(1f, 1.05f, 0.3f);
      WeatherSystemController.Instance.EmitLightningFlash();
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big-end", false);
      enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.headBobSFX, enemyYngyaBoss.gameObject);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(3f, enemyYngyaBoss.Spine);
    }
  }

  public void MultiChunkBig() => this.StartCoroutine((IEnumerator) this.MultiChunkBigIE());

  public IEnumerator MultiChunkBigIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkThreeStartSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkThreeStartVO, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.33f, enemyYngyaBoss.Spine);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.right * 5.5f + Vector3.up * 5.5f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.left * 5.5f + Vector3.up * 5.5f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.right * 5.5f + Vector3.down * 5.5f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.left * 5.5f + Vector3.down * 5.5f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2.5f, enemyYngyaBoss.Spine);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkThreePartBSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkThreePartBVO, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.33f, enemyYngyaBoss.Spine);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.right * 3f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.left * 3f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.up * 3f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileRingChunk, enemyYngyaBoss.transform.position + Vector3.down * 3f + (Vector3) UnityEngine.Random.insideUnitCircle, false);
    if (!enemyYngyaBoss.DamageSelf(3))
    {
      CameraManager.instance.ShakeCameraForDuration(1f, 1.05f, 0.3f);
      WeatherSystemController.Instance.EmitLightningFlash();
      yield return (object) CoroutineStatics.WaitForScaledSeconds(2.5f, enemyYngyaBoss.Spine);
      enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big-end", false);
      enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      AudioManager.Instance.PlayOneShot(enemyYngyaBoss.headBobSFX, enemyYngyaBoss.gameObject);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(3f, enemyYngyaBoss.Spine);
    }
  }

  public void MultiChunkMedium() => this.StartCoroutine((IEnumerator) this.MultiChunkMediumIE());

  public IEnumerator MultiChunkMediumIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    List<Vector3> targetPositions = new List<Vector3>();
    float degree = UnityEngine.Random.Range(0.0f, 360f);
    int amount = 20;
    for (int index = 0; index < amount; ++index)
    {
      Vector3 vector3 = enemyYngyaBoss.transform.position + (Vector3) Utils.DegreeToVector2(degree) * UnityEngine.Random.Range(7f, 8.5f);
      degree += (float) (360 / amount);
      targetPositions.Add(vector3);
    }
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkTwoStartSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkTwoStartVO, enemyYngyaBoss.gameObject);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big-start", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "throw-flesh-big-loop", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.66f, enemyYngyaBoss.Spine);
    targetPositions.Shuffle<Vector3>();
    for (int i = 0; i < amount; ++i)
    {
      enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileSplatterChunk, targetPositions[0], false);
      targetPositions.RemoveAt(0);
      if (i % 5 == 0 && enemyYngyaBoss.DamageSelf(1))
        yield break;
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyYngyaBoss.multiChunkTimeBetween, enemyYngyaBoss.Spine);
    }
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big-end", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.headBobSFX, enemyYngyaBoss.gameObject);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyYngyaBoss.Spine);
  }

  public void MultiSpawnChunkMini() => this.StartCoroutine((IEnumerator) this.MultiChunkMiniIE());

  public IEnumerator MultiChunkMiniIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    float num = 0.6166667f;
    enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.ShakeCameraWithRampUp(0.6166667f, num, enemyYngyaBoss.multiChunkMaxShake / 2f));
    MMVibrate.RumbleForAllPlayers(1f, 1.2f, num);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkOneStartSFX, enemyYngyaBoss.gameObject);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.attackMultiChunkOneStartVO, enemyYngyaBoss.gameObject);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "throw-flesh-big", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.33f, enemyYngyaBoss.Spine);
    for (int i = 0; i < 5 && (i % 5 != 0 || !enemyYngyaBoss.DamageSelf(1)); ++i)
    {
      yield return (object) CoroutineStatics.WaitForScaledSeconds(0.06666667f, enemyYngyaBoss.Spine);
      enemyYngyaBoss.SpawnChunk(enemyYngyaBoss.projectileSplatterChunk, animate: false);
    }
  }

  public IEnumerator ShakeCameraWithRampUp(float buildUp, float totalDuration, float maxShake)
  {
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < (double) buildUp)
    {
      float t1 = t / buildUp;
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, maxShake / 2f, t1), Mathf.Lerp(0.0f, maxShake, t1), buildUp, false);
      yield return (object) null;
    }
    CameraManager.instance.ShakeCameraForDuration(maxShake / 2f, maxShake, totalDuration - buildUp, false);
  }

  public IEnumerator ShakeYngyaWithRampUp(float buildUp, float totalDuration, float maxShake)
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < (double) buildUp)
    {
      float t1 = t / buildUp;
      enemyYngyaBoss.Spine.transform.localPosition = enemyYngyaBoss.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * Mathf.Lerp(0.0f, maxShake, t1);
      yield return (object) null;
    }
    while ((double) (t += Time.deltaTime) < (double) totalDuration - (double) buildUp)
      enemyYngyaBoss.Spine.transform.localPosition = enemyYngyaBoss.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * maxShake;
    enemyYngyaBoss.Spine.transform.localPosition = Vector3.zero;
  }

  public void Initialise()
  {
    if (this.SKIP_INTRO)
    {
      this.kickOffInteraction.ActivateDistance = 7f;
      this.kickOffInteraction.AutomaticallyInteract = true;
    }
    BiomeConstants.Instance.VignetteTween(0.0f, 1f, 1f);
  }

  public void Intro()
  {
    this.kickOffInteraction.enabled = false;
    DataManager.Instance.DiedToYngyaBoss = true;
    EnemyYngyaIntro.Instance.ResetPlayerWalking();
    if (this.SKIP_INTRO)
    {
      this.simpleSetCameras[0].Disable();
      this.introPostDLCOneShotInstance = AudioManager.Instance.PlayOneShotWithInstance(this.introTransformationPostDLCSFX);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.cameraTarget, 10f);
      PlayerFarming.Instance.GoToAndStop(this.transform.position + Vector3.down * 2f, this.cameraTarget, maxDuration: 8f, forcePositionOnTimeout: true, groupAction: true);
      this.bossIntro = this.StartCoroutine((IEnumerator) this.BossIntroIE(0.75f));
    }
    else
      this.StartCoroutine((IEnumerator) this.IntroIE());
  }

  public IEnumerator IntroIE()
  {
    EnemyYngyaBoss enemyYngyaBoss1 = this;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ProceedToYngya);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss1.introInteractSFX);
    enemyYngyaBoss1.duckAtmostInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyYngyaBoss1.duckAtmosSnapshot, (Transform) null, false);
    enemyYngyaBoss1.introPreBattleLoopInstance = AudioManager.Instance.CreateLoop(enemyYngyaBoss1.introPreBattleLoopSFX, true);
    GameManager.GetInstance().OnConversationNew();
    PlayerFarming.Instance.GoToAndStop(enemyYngyaBoss1.transform.position + Vector3.down * 2f + Vector3.right * 0.0f, enemyYngyaBoss1.cameraTarget, maxDuration: 8f, forcePositionOnTimeout: true, groupAction: true);
    enemyYngyaBoss1.simpleSetCameras[1].Play();
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss1.cameraTarget);
    yield return (object) new WaitForSeconds(1f);
    float num = 9f;
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 5f, num).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    enemyYngyaBoss1.StartCoroutine((IEnumerator) enemyYngyaBoss1.Shake(1f, 2f, num));
    yield return (object) new WaitForSeconds(2f);
    SkeletonAnimation[] skeletonAnimationArray = enemyYngyaBoss1.ghosts;
    for (int index = 0; index < skeletonAnimationArray.Length; ++index)
    {
      SkeletonAnimation skeletonAnimation = skeletonAnimationArray[index];
      skeletonAnimation.gameObject.SetActive(true);
      skeletonAnimation.transform.DOMove(skeletonAnimation.transform.position + Vector3.back * 3f, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      yield return (object) new WaitForSeconds(0.5f);
    }
    skeletonAnimationArray = (SkeletonAnimation[]) null;
    yield return (object) new WaitForSeconds(2f);
    CameraManager.instance.Stopshake();
    yield return (object) new WaitForSeconds(1f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(enemyYngyaBoss1.ghosts[0].gameObject, "Conversation_NPC/Yngya/Fight/Ghosts/0", "blacksmith/blacksmith-talk-shy-sad", CharacterName: "NAMES/BlacksmithNPC"),
      new ConversationEntry(enemyYngyaBoss1.ghosts[1].gameObject, "Conversation_NPC/Yngya/Fight/Ghosts/1", "Merchant/merchant-talk-general", CharacterName: "NAMES/DecoNPC")
    }, (List<MMTools.Response>) null, (System.Action) null), false);
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/yngya/story_intro_flashback_01");
    yield return (object) enemyYngyaBoss1.StartCoroutine((IEnumerator) enemyYngyaBoss1.Flicker());
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(enemyYngyaBoss1.ghosts[2].gameObject, "Conversation_NPC/Yngya/Fight/Ghosts/2", "farmer/farmer-talk", CharacterName: "NAMES/Rancher"),
      new ConversationEntry(enemyYngyaBoss1.ghosts[3].gameObject, "Conversation_NPC/Yngya/Fight/Ghosts/3", "Priest/priest-talk-sad3", CharacterName: "NAMES/GraveyardNPC")
    }, (List<MMTools.Response>) null, (System.Action) null), false);
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    EnemyYngyaIntro.Instance.SetYngyaVisionAnimation("idle");
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/yngya/story_intro_flashback_02");
    yield return (object) enemyYngyaBoss1.StartCoroutine((IEnumerator) enemyYngyaBoss1.Flicker());
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(enemyYngyaBoss1.ghosts[3].gameObject, "Conversation_NPC/Yngya/Fight/Ghosts/4", "Priest/priest-talk-angry", CharacterName: "NAMES/GraveyardNPC")
    }, (List<MMTools.Response>) null, (System.Action) null), false);
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    yield return (object) enemyYngyaBoss1.StartCoroutine((IEnumerator) enemyYngyaBoss1.StruggleIE());
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss1.gameObject);
    AudioManager.Instance.StopLoop(enemyYngyaBoss1.ambientIntroLoopInstance);
    AudioManager.Instance.StopLoop(enemyYngyaBoss1.introPreBattleLoopInstance);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss1.introTransformationSFX);
    foreach (SkeletonAnimation ghost1 in enemyYngyaBoss1.ghosts)
    {
      EnemyYngyaBoss enemyYngyaBoss = enemyYngyaBoss1;
      SkeletonAnimation ghost = ghost1;
      ghost.transform.DOMove(ghost.transform.position + Vector3.back * 3f, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(UnityEngine.Random.Range(0.0f, 0.5f)).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => ghost.transform.DOMove(enemyYngyaBoss.cameraTarget.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        CameraManager.instance.ShakeCameraForDuration(2f, 3f, 0.1f);
        ghost.gameObject.SetActive(false);
      }))));
    }
    enemyYngyaBoss1.simpleSetCameras[1].Disable();
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss1.cameraTarget);
    yield return (object) new WaitForSeconds(3f);
    CameraManager.instance.ShakeCameraForDuration(2f, 3f, 0.5f, false);
    yield return (object) new WaitForSeconds(1f);
    enemyYngyaBoss1.StartCoroutine((IEnumerator) enemyYngyaBoss1.Shake(1f, 1f, 6f));
    enemyYngyaBoss1.Spine.timeScale = 1f;
    enemyYngyaBoss1.StartCoroutine((IEnumerator) enemyYngyaBoss1.GhostsFlyInIE());
    yield return (object) new WaitForSeconds(1.25f);
    yield return (object) enemyYngyaBoss1.StartCoroutine((IEnumerator) enemyYngyaBoss1.BossIntroIE());
  }

  public IEnumerator StruggleIE()
  {
    int struggleMaxCount = 3;
    bool accepted = false;
    float Progress = 0.0f;
    float maxZoom = 9f;
    float progressSpeedMutliplier = 1f;
    Color c = this.FloorSymbols.color with { a = 0.5f };
    this.FloorSymbols.color = c;
    float fadeDuration = 1.5f;
    float t = 0.0f;
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/yngya/story_intro_pentagram_appears");
    while ((double) t < (double) fadeDuration)
    {
      t += Time.deltaTime;
      c.a = Mathf.Lerp(0.0f, 1f, t / fadeDuration);
      this.FloorSymbols.color = c;
      yield return (object) null;
    }
    c.a = 1f;
    this.FloorSymbols.color = c;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.state.LockStateChanges = true;
    Camera targetCamera = GameManager.GetInstance().CamFollowTarget.TargetCamera;
    t = 0.0f;
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 1.5f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.Lerp(Vector3.forward * 2f, Vector3.zero, t))).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
    for (int x = 0; x < struggleMaxCount; ++x)
    {
      this.floorRot[x].material.SetFloat("_RotReveal", 0.0f);
      this.floorRot[x].gameObject.SetActive(true);
      float num1 = x == 0 ? 1.5f : 0.5f;
      GameManager.GetInstance().CamFollowTarget.SetTargetCamera(targetCamera, num1, GameManager.GetInstance().CamFollowTarget.TargetCameraAnimationCurve);
      this.floorRot[x].material.DOFloat(1f, "_RotReveal", num1 + 0.5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
      if (x == 0)
        yield return (object) new WaitForSeconds(num1);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/yngya/story_intro_mutationpile_wrapup");
      this.PlayYngyaCallPlayerAnimation();
      yield return (object) new WaitForSeconds(1.25f);
      this.PlayNextStruggleYngyaConvo(x);
      yield return (object) null;
      while (MMConversation.isPlaying)
        yield return (object) null;
      GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(1f);
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
      PlayerFarming.Instance.indicator.ForceShown = true;
      PlayerFarming.Instance.indicator.HoldToInteract = SettingsManager.Settings.Accessibility.HoldActions;
      PlayerFarming.Instance.indicator.text.text = ScriptLocalization.UI_Generic.Decline;
      PlayerFarming.Instance.indicator.SetForceShownPosition(Vector3.down * 400f);
      PlayerFarming.Instance.indicator.SetGameObjectActive(true);
      PlayerFarming.Instance.indicator.Interactable = true;
      PlayerFarming.Instance.indicator.Reset();
      yield return (object) null;
      if (!SettingsManager.Settings.Accessibility.HoldActions)
        yield return (object) new WaitForSecondsRealtime(0.5f);
      else
        this.SetHoldProgressDown(PlayerFarming.Instance.indicator);
      accepted = false;
      Progress = 0.0f;
      float minZoom = 4f + (float) struggleMaxCount - (float) (x + 1);
      float shakeMaxIntenstity = (float) (x + 1) * 0.15f;
      MMConversation.isPlaying = true;
      while (true)
      {
        if (SettingsManager.Settings.Accessibility.HoldActions)
        {
          if (x == struggleMaxCount - 1)
            progressSpeedMutliplier = (double) PlayerFarming.Instance.indicator.Progress <= 0.5 ? ((double) PlayerFarming.Instance.indicator.Progress <= 0.75 ? 1f : 0.4f) : 0.6f;
          if (InputManager.Gameplay.GetInteractButtonHeld(PlayerFarming.Instance))
            Progress += Time.deltaTime / 3f * progressSpeedMutliplier;
          else
            Progress = Mathf.Clamp01(PlayerFarming.Instance.indicator.Progress - Time.deltaTime);
          PlayerFarming.Instance.indicator.Progress = Progress;
          GameManager.GetInstance().CameraSetZoom(Mathf.Lerp(maxZoom, minZoom, Progress));
          CameraManager.shakeCamera(shakeMaxIntenstity * (Progress + 1f));
          this.UpdateHoldProgressDown(PlayerFarming.Instance.indicator);
          if ((double) Progress <= 0.0)
            this.StopHoldProgressDown();
          else if (!this.playingHoldDown)
            this.SetHoldProgressDown(PlayerFarming.Instance.indicator);
        }
        else if (!accepted)
        {
          accepted = InputManager.Gameplay.GetInteractButtonHeld(PlayerFarming.Instance);
          if (accepted)
            break;
        }
        if ((double) Progress < 1.0)
          yield return (object) null;
        else
          goto label_28;
      }
      PlayerFarming.Instance.indicator.ForceShown = false;
      PlayerFarming.Instance.indicator.gameObject.SetActive(false);
label_28:
      PlayerFarming.Instance.simpleSpineAnimator.Animate("Yngya_Calls/rot-free", 0, false);
      this.StopHoldProgressDown();
      MMConversation.isPlaying = false;
      PlayerFarming.Instance.indicator.ForceShown = false;
      PlayerFarming.Instance.indicator.Deactivate();
      yield return (object) new WaitForSeconds(1.15f);
      if (x == struggleMaxCount - 1)
      {
        this.PulseDisplacementObject();
        foreach (Renderer renderer in this.floorRot)
          renderer.material.DOFloat(0.0f, "_RotReveal", 0.5f);
      }
      GameManager.GetInstance().CameraSetZoom(maxZoom);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/yngya/story_intro_mutationpile_breakfree");
      string soundPath1 = "event:/dlc/dungeon06/enemy/yngya/story_intro_hanging_bell_ring";
      string soundPath2 = "event:/dlc/dungeon06/enemy/yngya/story_intro_hanging_bell_ring";
      AudioManager.Instance.PlayOneShot(soundPath1);
      AudioManager.Instance.PlayOneShot(soundPath2);
      EnemyYngyaIntro.Instance.PlayRipple(PlayerFarming.Instance.transform);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/yngya/story_intro_pulse");
      float num2 = 1.5f;
      CameraManager.instance.ShakeCameraForDuration((float) (2.0 + (double) x * (double) num2), (float) (2.5 + (double) x * (double) num2), 0.5f);
      MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.35f);
      this.rotGoopParticles.Play();
      this.rotParticles.Spawn(90f, 360f);
      this.introTentacles1.AnimationState.SetAnimation(0, "hidden", true);
      this.introTentacles2.AnimationState.SetAnimation(0, "hidden", true);
      this.introTentacles3.AnimationState.SetAnimation(0, "hidden", true);
      yield return (object) new WaitForSeconds(1.85f);
    }
    PlayerFarming.Instance.state.LockStateChanges = false;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
  }

  public void PlayNextStruggleYngyaConvo(int convoNumber)
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.ghosts[3].gameObject, "Conversation_NPC/Yngya/Fight/Ghosts/4_ALT/" + convoNumber.ToString(), "Priest/priest-talk-angry", CharacterName: ScriptLocalization.NAMES.GraveyardNPC)
    }, (List<MMTools.Response>) null, (System.Action) null), false);
  }

  public void PlayYngyaCallPlayerAnimation()
  {
    PlayerFarming.Instance.simpleSpineAnimator.Animate("Yngya_Calls/rot-start", 0, false).TimeScale = 1f;
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("Yngya_Calls/rot-loop", 0, true, 0.0f);
    this.introTentacles1.AnimationState.SetAnimation(0, "appear", false);
    this.introTentacles1.AnimationState.AddAnimation(0, "loop", true, 0.0f);
    this.introTentacles2.AnimationState.SetAnimation(0, "appear", false);
    this.introTentacles2.AnimationState.AddAnimation(0, "loop", true, 0.0f);
    this.introTentacles3.AnimationState.SetAnimation(0, "appear", false);
    this.introTentacles3.AnimationState.AddAnimation(0, "loop", true, 0.0f);
  }

  public void SetHoldProgressDown(Indicator indicator)
  {
    this.playingHoldDown = true;
    indicator.ControlPromptContainer.DOKill();
    indicator.ControlPromptContainer.localScale = Vector3.one;
    indicator.ControlPromptContainer.DOPunchScale(new Vector3(0.2f, 0.2f), 0.2f);
    this.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/dlc/dungeon06/enemy/yngya/story_intro_hold_button_loop", this.gameObject, true);
    if ((double) indicator.Progress <= 0.949999988079071)
      return;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public void UpdateHoldProgressDown(Indicator indicator)
  {
    int num = (int) this.loopingSoundInstance.setParameterByName(this.holdTime, indicator.Progress);
    MMVibrate.RumbleContinuous(indicator.Progress * 0.2f, indicator.Progress * 0.2f, PlayerFarming.Instance);
  }

  public void StopHoldProgressDown()
  {
    this.playingHoldDown = false;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public IEnumerator BossIntroIE(float delay = 0.0f)
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    if ((double) delay > 0.0)
      yield return (object) new WaitForSeconds(delay);
    UnitObject.Action startTransforming = enemyYngyaBoss.OnStartTransforming;
    if (startTransforming != null)
      startTransforming();
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "heart-ritual", false);
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(4.66666651f);
    enemyYngyaBoss.simpleSpineFlash.FlashWhite(false);
    BiomeConstants.Instance.VignetteTween(0.0f, 1f, 0.0f);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 0.5f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.35f);
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.cameraTarget, 10f);
    enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SpawnSplatter(25, 0.2f, enemyYngyaBoss.cameraTarget.transform.position));
    enemyYngyaBoss.yngyaRevealParticles.Play();
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.35f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.35f);
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.cameraTarget, 11f);
    enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SpawnSplatter(15, 0.1f, enemyYngyaBoss.cameraTarget.transform.position + Vector3.right * 3f - Vector3.forward * 2f));
    yield return (object) new WaitForSeconds(0.933333337f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.35f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.35f);
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.cameraTarget, 10.5f);
    enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SpawnSplatter(15, 0.1f, enemyYngyaBoss.cameraTarget.transform.position + Vector3.left * 3f - Vector3.forward * 2f));
    UnitObject.Action finishingTransformation = enemyYngyaBoss.OnFinishingTransformation;
    if (finishingTransformation != null)
      finishingTransformation();
    yield return (object) new WaitForSeconds(2.0666666f);
    CameraManager.instance.ShakeCameraForDuration(3f, 4.5f, 1f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 1.2f);
    BiomeConstants.Instance.ImpactFrameForDuration();
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.cameraTarget, 14f);
    enemyYngyaBoss.arenaLighting.SetActive(true);
    foreach (GameObject torch in enemyYngyaBoss.torches)
      torch.SetActive(true);
    yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SpawnSplatter(100, 0.2f, enemyYngyaBoss.cameraTarget.transform.position));
    HUD_DisplayName.Play(ScriptLocalization.NAMES.Yngya, 3, HUD_DisplayName.Positions.Centre);
    AudioManager.Instance.SetMusicParam("deathcat_room_id", 9f);
    AudioManager.Instance.StopOneShotInstanceEarly(enemyYngyaBoss.duckAtmostInstance, STOP_MODE.IMMEDIATE);
    if (enemyYngyaBoss.SKIP_INTRO)
    {
      AudioManager.Instance.StopLoop(enemyYngyaBoss.ambientIntroLoopInstance);
      AudioManager.Instance.StartMusic();
    }
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationEnd();
    enemyYngyaBoss.Play();
  }

  public void SkipIntro()
  {
    this.StopCoroutine(this.bossIntro);
    this.StartCoroutine((IEnumerator) this.SkipIntroIE());
  }

  public IEnumerator SkipIntroIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.cameraTarget, 10.5f);
    BiomeConstants.Instance.VignetteTween(0.0f, 1f, 0.0f);
    enemyYngyaBoss.Spine.AnimationState.SetAnimation(0, "heart-ritual", false).AnimationStart = 7.5f;
    enemyYngyaBoss.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    AudioManager.Instance.StopOneShotInstanceEarly(enemyYngyaBoss.introPostDLCOneShotInstance, STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.PlayOneShot(enemyYngyaBoss.introTransformationPostDLCSkip);
    yield return (object) new WaitForSeconds(1.2f);
    GameManager.GetInstance().OnConversationNext(enemyYngyaBoss.cameraTarget, 14f);
    CameraManager.instance.ShakeCameraForDuration(3f, 4.5f, 1f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 1.2f);
    BiomeConstants.Instance.ImpactFrameForDuration();
    enemyYngyaBoss.arenaLighting.SetActive(true);
    foreach (GameObject torch in enemyYngyaBoss.torches)
      torch.SetActive(true);
    yield return (object) enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SpawnSplatter(100, 0.2f, enemyYngyaBoss.cameraTarget.transform.position));
    HUD_DisplayName.Play(ScriptLocalization.NAMES.Yngya, 3, HUD_DisplayName.Positions.Centre);
    AudioManager.Instance.SetMusicParam("deathcat_room_id", 9f);
    AudioManager.Instance.StopOneShotInstanceEarly(enemyYngyaBoss.duckAtmostInstance, STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StartMusic();
    AudioManager.Instance.StopLoop(enemyYngyaBoss.ambientIntroLoopInstance);
    yield return (object) new WaitForSeconds(1.7f);
    GameManager.GetInstance().OnConversationEnd();
    enemyYngyaBoss.Play();
  }

  public IEnumerator GhostsFlyInIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    int ghostCount = 110;
    float increment = 0.225f;
    float delay = 0.0125f;
    float t = 0.0f;
    DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 4.66666651f).SetDelay<TweenerCore<float, float, FloatOptions>>(1.25f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.simpleSpineFlash.FlashWhite(Mathf.Lerp(0.0f, 0.75f, t)))).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.simpleSpineFlash.FlashWhite(0.0f);
      this.simpleSpineFlash.FlashWhite(false);
    }));
    for (int i = 0; i < ghostCount; ++i)
    {
      SkeletonAnimation ghost = UnityEngine.Object.Instantiate<SkeletonAnimation>(enemyYngyaBoss.genericGhost);
      ghost.gameObject.SetActive(true);
      ghost.transform.position = enemyYngyaBoss.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle.normalized * 15f);
      ghost.transform.position = new Vector3(ghost.transform.position.x, ghost.transform.position.y, UnityEngine.Random.Range(-3f, -1f));
      ghost.transform.DOMove(enemyYngyaBoss.cameraTarget.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) ghost.gameObject)));
      if ((double) ghost.transform.position.x < (double) enemyYngyaBoss.transform.position.x)
        ghost.transform.localScale = new Vector3(-1f, 1f, 1f);
      if ((double) increment < 0.0099999997764825821)
        yield return (object) new WaitForFixedUpdate();
      else
        yield return (object) new WaitForSeconds(increment);
      increment -= delay;
    }
  }

  public IEnumerator SpawnGhostsOut(int amount, float delay)
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    for (int i = 0; i < amount; ++i)
    {
      SkeletonAnimation ghost = UnityEngine.Object.Instantiate<SkeletonAnimation>(enemyYngyaBoss.genericGhost);
      ghost.gameObject.SetActive(true);
      ghost.transform.position = enemyYngyaBoss.cameraTarget.transform.position;
      Vector3 endValue = (enemyYngyaBoss.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle.normalized * 15f)) with
      {
        z = UnityEngine.Random.Range(-3f, -1f)
      };
      ghost.transform.DOMove(endValue, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) ghost.gameObject)));
      if ((double) endValue.x > (double) enemyYngyaBoss.transform.position.x)
        ghost.transform.localScale = new Vector3(-1f, 1f, 1f);
      yield return (object) new WaitForSeconds(delay);
    }
  }

  public IEnumerator SpawnSplatter(int amount, float duration, Vector3 pos)
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    float increment = duration / (float) amount;
    for (int i = 0; i < amount; ++i)
    {
      GrenadeBullet component = ObjectPool.Spawn<GrenadeBullet>(enemyYngyaBoss.grenadeBullet, pos, Quaternion.identity).GetComponent<GrenadeBullet>();
      component.SetOwner(enemyYngyaBoss.gameObject);
      component.Play(UnityEngine.Random.Range(-3f, -1f), (float) UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(6f, 10f), 0.0f, hideIndicator: true);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(increment, enemyYngyaBoss.Spine);
    }
  }

  public IEnumerator Shake(float fromTimescale, float toTimescale, float dur)
  {
    float time = 0.0f;
    while ((double) (time += Time.deltaTime) < (double) dur)
    {
      float t = time / dur;
      this.Spine.timeScale = Mathf.Lerp(fromTimescale, toTimescale, t);
      CameraManager.instance.ShakeCameraForDuration(Mathf.Lerp(0.0f, 1f, t), Mathf.Lerp(0.0f, 2f, t), 0.1f, false);
      yield return (object) null;
    }
  }

  public bool ReleaseGhostNPC()
  {
    bool flag = false;
    if (DataManager.Instance.BeatenYngya)
      return flag;
    for (int index = 0; index < 8; ++index)
    {
      if (1.0 - (double) this.health.HP / (double) this.health.totalHP > (double) ((float) (this.ghostsKnockedOut + 1) / 9f))
      {
        this.health.StartCoroutine((IEnumerator) this.ReleaseGhostNPCIE());
        flag = true;
      }
    }
    return flag;
  }

  public IEnumerator ReleaseGhostNPCIE()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    enemyYngyaBoss.StartCoroutine((IEnumerator) enemyYngyaBoss.SlowMo());
    int ghostsKnockedOut = enemyYngyaBoss.ghostsKnockedOut;
    ++enemyYngyaBoss.ghostsKnockedOut;
    SkeletonAnimation ghost = enemyYngyaBoss.ghosts[ghostsKnockedOut];
    Vector3[] path = new Vector3[2]
    {
      enemyYngyaBoss.cameraTarget.transform.position + enemyYngyaBoss.ghostKnockedOutPositions[ghostsKnockedOut].transform.position.normalized * 3f + Vector3.down * 5f,
      enemyYngyaBoss.ghostKnockedOutPositions[ghostsKnockedOut].transform.position
    };
    ghost.transform.position = enemyYngyaBoss.cameraTarget.transform.position;
    ghost.gameObject.SetActive(true);
    ghost.transform.DOPath(path, 1.5f, gizmoColor: (Color?) new Color?()).SetEase<TweenerCore<Vector3, Path, PathOptions>>(Ease.OutSine);
    ghost.AnimationState.SetAnimation(0, "farmer/farmer-flip", false);
    ghost.AnimationState.AddAnimation(0, "farmer/farmer-ritual", true, 0.0f);
    enemyYngyaBoss.torches[ghostsKnockedOut].gameObject.SetActive(false);
    if (enemyYngyaBoss.ghostsKnockedOut > 2 && enemyYngyaBoss.ghostsKnockedOut < 8)
      ghost.transform.localScale = new Vector3(ghost.transform.localScale.x * -1f, ghost.transform.localScale.y, ghost.transform.localScale.z);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1.5f, enemyYngyaBoss.Spine);
    ghost.transform.localScale = new Vector3(ghost.transform.localScale.x * -1f, ghost.transform.localScale.y, ghost.transform.localScale.z);
  }

  public IEnumerator SlowMo()
  {
    this.slowMoSnapshotInstance = AudioManager.Instance.PlayOneShotWithInstance(this.slowMoSnapshotSFX);
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < 0.699999988079071)
    {
      GameManager.SetTimeScale(0.2f);
      yield return (object) null;
    }
    AudioManager.Instance.StopOneShotInstanceEarly(this.slowMoSnapshotInstance, STOP_MODE.IMMEDIATE);
    GameManager.SetTimeScale(1f);
  }

  public IEnumerator PlayerPickUpThrophyDeco(Vector3 itemSpawnPosition)
  {
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", this.transform.position);
    FoundItemPickUp deco = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, itemSpawnPosition).GetComponent<FoundItemPickUp>();
    PickUp component1 = deco.GetComponent<PickUp>();
    component1.enabled = false;
    component1.child.transform.localScale = Vector3.one;
    deco.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_YNGYA;
    deco.transform.position = itemSpawnPosition;
    PlayerSimpleInventory component2 = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 itemTargetPosition = new Vector3(component2.ItemImage.transform.position.x, component2.ItemImage.transform.position.y, -1f);
    yield return (object) new WaitForSeconds(0.5f);
    bool wait = true;
    deco.transform.DOMove(itemTargetPosition, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => wait = false));
    while (wait)
      yield return (object) null;
    deco.transform.position = itemTargetPosition;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid");
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    yield return (object) new WaitForSeconds(1f);
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

  public void OnSpineEvent(string eventName)
  {
    switch (eventName)
    {
      case "heartbeat":
        AudioManager.Instance.PlayOneShot(this.introHeartbeatSFX, this.gameObject);
        MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
        break;
      case "blink":
        AudioManager.Instance.PlayOneShot(this.introBlinkSFX, this.gameObject);
        break;
    }
  }

  public void ReplaceYngyaFollowerDefaultSFX(GameObject followerPrefab)
  {
    foreach (followerSpineEventListeners spineEventListener in followerPrefab.GetComponentInChildren<FollowerSpineEventListener>(true).spineEventListeners)
    {
      if (spineEventListener.eventName == "VO/talk short nice")
        spineEventListener.soundPath = "event:/dlc/dialogue/yngya/follower_general_talk_short_nice";
    }
  }

  public IEnumerator Flicker()
  {
    EnemyYngyaBoss enemyYngyaBoss = this;
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.gameObject.SetActive(false);
    CameraFollowTarget.Instance.ClearAllTargets();
    enemyYngyaBoss.normalScene.transform.position = Vector3.up * 500f;
    enemyYngyaBoss.flashbackScene.gameObject.SetActive(true);
    BiomeConstants.Instance.ImpactFrameForDuration(0.1f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.6f);
    BiomeConstants.Instance.ImpactFrameForDuration(0.1f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.1f);
    enemyYngyaBoss.normalScene.transform.position = Vector3.zero;
    enemyYngyaBoss.flashbackScene.gameObject.SetActive(false);
    CameraFollowTarget.Instance.AddTarget(enemyYngyaBoss.gameObject, 1f);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.gameObject.SetActive(true);
  }

  public void PulseDisplacementObject()
  {
    this.displacement.transform.position = this.transform.position with
    {
      z = -0.01f
    };
    this.displacement.SetActive(true);
    this.displacement.transform.DOKill();
    this.displacement.transform.localScale = Vector3.zero;
    this.displacement.transform.DOScale(9f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.displacement.SetActive(false)));
  }

  [CompilerGenerated]
  public void \u003COnDieIE\u003Eb__171_0() => this.Spine.gameObject.SetActive(false);

  [CompilerGenerated]
  public float \u003COnDieIE\u003Eb__171_1() => this.YngyaRecruitSpine.Skeleton.A;

  [CompilerGenerated]
  public void \u003COnDieIE\u003Eb__171_2(float x) => this.YngyaRecruitSpine.Skeleton.A = x;

  [CompilerGenerated]
  public float \u003COnDieIE\u003Eb__171_3() => this.YngyaRecruitSpine.Skeleton.A;

  [CompilerGenerated]
  public void \u003COnDieIE\u003Eb__171_4(float x) => this.YngyaRecruitSpine.Skeleton.A = x;

  [CompilerGenerated]
  public void \u003CPulseDisplacementObject\u003Eb__226_0() => this.displacement.SetActive(false);
}
