// Decompiled with JetBrains decompiler
// Type: EnemyBruteBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Febucci.UI;
using FMOD.Studio;
using FMODUnity;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using Lamb.UI.DeathScreen;
using MMBiomeGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyBruteBoss : EnemyBrute
{
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string walkAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string roarAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string spawnAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string extractingGraveStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string extractingGraveLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string extractingGraveEndAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string throwAxeAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string throwAxeLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string throwAxeLoopLongAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string throwAxeLongAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string catchAxeAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string distanceTrapAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string closeTrapAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string closeTrapQuickAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string knockedOutStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string knockedOutLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string knockedOutEndAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string shockWaveStartAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string shockWaveLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string shockWaveEndAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string jumpAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string jumpLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string jumpLandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string throwMinionAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string swipeAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string throwAnimation = "throw";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string dieAnimation = "die";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string dieLoopAnimation = "die-loop";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string dieRevealAnimation = "die-reveal";
  [SerializeField]
  public Renderer symbols;
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/gethit_vo";
  [EventRef]
  public string AttackSkeltonSummonBasicSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_summon_basic_start";
  [EventRef]
  public string AttackSkeltonSummonBasicVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_summon_basic_start_vo";
  [EventRef]
  public string AttackSkeltonSummonGuardianSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_summon_guardian_start";
  [EventRef]
  public string AttackSkeltonSummonGuardianVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_summon_guardian_start_vo";
  [EventRef]
  public string AttackSkeletonThrowLiftSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_throw_lift";
  [EventRef]
  public string AttackSkeletonThrowLiftVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_throw_lift_vo";
  [EventRef]
  public string AttackSkeletonThrowTossSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_throw_toss";
  [EventRef]
  public string AttackSkeletonThrowTossVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_throw_toss_vo";
  [EventRef]
  public string AttackSkeletonThrowExplodeSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_skeleton_throw_explode";
  [EventRef]
  public string AttackJumpStartSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_jump_start";
  [EventRef]
  public string AttackJumpStartVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_jump_start_vo";
  [EventRef]
  public string AttackJumpLandSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_jump_land";
  [EventRef]
  public string AttackBoneWallSingleSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_bone_wall_single_start";
  [EventRef]
  public string AttackBoneWallSingleVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_bone_wall_single_start_vo";
  [EventRef]
  public string AttackBoneWallMultiStartSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_bone_wall_multi_start";
  [EventRef]
  public string AttackBoneWallMultiStartVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_bone_wall_multi_start_vo";
  [EventRef]
  public string AttackBoneWallMultiBurstSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_bone_wall_multi_burst";
  [EventRef]
  public string AttackBoneWallMultiBurstVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_bone_wall_multi_burst_vo";
  [EventRef]
  public string AttackBoneWallChasingSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_bone_wall_chasing_start";
  [EventRef]
  public string AttackBoneWallChasingVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_bone_wall_chasing_start_vo";
  [EventRef]
  public string AttackAxeThrowStartSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_axe_throw_start";
  [EventRef]
  public string AttackAxeThrowStartVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_axe_throw_start_vo";
  [EventRef]
  public string AttackAxeThrowCatchSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_axe_throw_catch";
  [EventRef]
  public string AttackAxeThrowCatchVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_axe_throw_catch_vo";
  [EventRef]
  public string AttackHammerSlamSingleStartSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_single_start";
  [EventRef]
  public string AttackHammerSlamSingleStartVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_single_start_vo";
  [EventRef]
  public string AttackHammerSlamSingleSwingSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_single_swing";
  [EventRef]
  public string AttackHammerSlamSingleSwingVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_single_swing_vo";
  [EventRef]
  public string AttackHammerSlamSingleReturnSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_single_return";
  [EventRef]
  public string AttackHammerSlamDoubleStartSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_double_start";
  [EventRef]
  public string AttackHammerSlamDoubleStartVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_double_start_vo";
  [EventRef]
  public string AttackHammerSlamDoubleSwingSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_double_swing";
  [EventRef]
  public string AttackHammerSlamDoubleSwingVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_double_swing_vo";
  [EventRef]
  public string AttackHammerSlamDoubleReturnSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_double_return";
  [EventRef]
  public string AttackHammerSlamTripleStartSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_triple_start";
  [EventRef]
  public string AttackHammerSlamTripleStartVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_triple_start_vo";
  [EventRef]
  public string AttackHammerSlamTripleSwingSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_triple_swing";
  [EventRef]
  public string AttackHammerSlamTripleSwingVO = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_triple_swing_vo";
  [EventRef]
  public string AttackHammerSlamTripleReturnSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/attack_hammer_slam_triple_return";
  [EventRef]
  public string PhaseFallDownSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_phase_falldown";
  [EventRef]
  public string PhaseGetUpSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_phase_getup";
  [EventRef]
  public string PhaseThreeStartSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_phase_03_start";
  [EventRef]
  public string OutroDetransitionSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_outro_detransition";
  [EventRef]
  public string OutroStandupSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_outro_stand_up";
  [EventRef]
  public string OutroMaskOffSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_outro_mask_off";
  [EventRef]
  public string OutroExecuteSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_outro_execute";
  [EventRef]
  public string OutroPardonSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_outro_pardon";
  [EventRef]
  public string OutroCorpseAppearsSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_outro_execute_corpse_death";
  [EventRef]
  public string OutroCameraWhooshSFX = "event:/ui/camera_whoosh";
  [EventRef]
  public string OutroNewItemPickupSFX = "event:/player/new_item_pickup";
  public string outroPrayingLoopSFX = "event:/dlc/dialogue/executioner/baseform_praying_loop";
  public EventInstance skeletonSummonInstanceSFX;
  public EventInstance skeletonSummonInstanceVO;
  public EventInstance skeletonLiftInstanceSFX;
  public EventInstance skeletonLiftInstanceVO;
  public EventInstance skeletonTossInstanceSFX;
  public EventInstance skeletonTossInstanceVO;
  public EventInstance jumpStartInstanceSFX;
  public EventInstance jumpStartInstanceVO;
  public EventInstance boneWallStartInstanceSFX;
  public EventInstance boneWallStartInstanceVO;
  public EventInstance boneWallMultiBurstInstanceSFX;
  public EventInstance boneWallMultiBurstInstanceVO;
  public EventInstance boneWallChasingInstanceSFX;
  public EventInstance boneWallChasingInstanceVO;
  public EventInstance axeThrowStartInstanceSFX;
  public EventInstance axeThrowStartInstanceVO;
  public EventInstance hammerStartInstanceSFX;
  public EventInstance hammerStartInstanceVO;
  public EventInstance hammerSwingInstanceSFX;
  public EventInstance hammerSwingInstanceVO;
  public EventInstance hammerReturnInstanceSFX;
  public EventInstance prayingLoopInstanceSFX;
  [SerializeField]
  public float hammerAttackAnticipation;
  [SerializeField]
  public float hammerAttackCooldown;
  public float pauseBetweenHammerStrikes = 0.3f;
  public Transform AxeBone;
  public AssetReferenceGameObject RingProjectile;
  public float ProjectileSpeed = 7f;
  public int HammerRingPojectileCount = 10;
  public float HammerImpactEffectDelay = 0.2f;
  [SerializeField]
  public float swipeAttackAnticipation = 0.5f;
  [SerializeField]
  public float swipeAttackCooldown;
  [SerializeField]
  public ColliderEvents shockWave;
  [SerializeField]
  public EnemyBruteBoss.SpawnData summonCountPhaseOne;
  [SerializeField]
  public EnemyBruteBoss.SpawnData summonCountPhaseTwo;
  [SerializeField]
  public EnemyBruteBoss.SpawnData summonCountPhaseThree;
  [SerializeField]
  public AssetReferenceGameObject spawnableUnit;
  [SerializeField]
  public AssetReferenceGameObject spawnableHeavyUnit;
  public int MaxSpawnCount = 8;
  [SerializeField]
  public GameObject trapLavaLarge;
  [SerializeField]
  public GameObject trapLavaSmall;
  public float LavalLifetime = 10f;
  public float MinionRiseHeight = 2f;
  public float MinionRiseDuration = 2f;
  public float PauseBeforeThrow = 1f;
  public float DelayBetweenMinionThrow = 0.5f;
  public float throwMinionCooldown = 1f;
  public AssetReferenceGameObject IndicatorPrefab;
  public float levitateEnemiesAnimDuration = 1.35f;
  public string lambSkeleFloatAnim = "floating_loop";
  [SerializeField]
  public float axeAnticipation;
  [SerializeField]
  public float axeForce;
  [SerializeField]
  public float axeDuration;
  [SerializeField]
  public float circleAxeForce;
  [SerializeField]
  public float circleAxeDuration;
  [SerializeField]
  public float angularVelocity;
  public int chasingAtkCount = 3;
  public float chasingAtkPauseBetweenAttacks = 1f;
  public float chasingAtkCooldown = 2f;
  [SerializeField]
  public GameObject trapPrefab;
  [SerializeField]
  public float jumpDuration;
  [SerializeField]
  public Interaction_SimpleConversation halfWayConvo;
  [SerializeField]
  public Interaction_SimpleConversation quarterConvo;
  public Interaction_SimpleConversation defeatedConvo;
  public Interaction_SimpleConversation pardonConvo;
  public Interaction_SimpleConversation damnConvo;
  [SerializeField]
  public Transform weaponSlot;
  [Space]
  [SerializeField]
  public int attacksUntilSpawn = 4;
  [SerializeField]
  public Vector2 timeBetweenAttacks;
  [SerializeField]
  public CircleCollider2D collider;
  public float slashColliderRadius = 2f;
  public float defaultColliderRadius;
  [SerializeField]
  public Interaction_TeleportHome teleporter;
  public int multiRingAttackCount = 3;
  public float distanceBetweenRings = 3f;
  public float timeBetweenRings = 0.5f;
  public float ringAttackCooldown = 2f;
  public float graveShakeDuration = 2f;
  public Vector3 graveShakeStrength = new Vector3(5f, 5f, 0.0f);
  public int graveShakeVibrato = 15;
  public int graveShakeRandomness = 15;
  public bool graveShakeFadeOut = true;
  public BreakableGrave[] graves;
  public Collider2D[] colliders;
  public SimpleSpineEventListener spineEventLister;
  public bool isDead;
  public bool attacking;
  public float nextAttackTimer;
  public int counter;
  public float checkPlayerTimer;
  public float checkPlayerInterval = 0.5f;
  public float spawnAnimationDuration = 1.5f;
  public float spawnEnemyCooldown = 1f;
  public float pauseBeforeSpawn = 1f;
  public Coroutine pathingRoutine;
  public float directionTimer = 1f;
  public Vector3 previousPos;
  public GameObject loadedProjectile;
  public GameObject loadedThrowableMinion;
  public GameObject loadedSpawnableHeavyUnit;
  public float closeRangeDistance = 2f;
  public List<UnitObject> minions = new List<UnitObject>();
  public List<GameObject> skeleThrowIndicators = new List<GameObject>();
  public EnemyBruteBoss.Phase currentPhase;
  public EnemyBruteBoss.Attacks[] phaseOneAttacks = new EnemyBruteBoss.Attacks[6]
  {
    EnemyBruteBoss.Attacks.HammerAttack,
    EnemyBruteBoss.Attacks.SingleDefenciveBoneWall,
    EnemyBruteBoss.Attacks.ThrowSkeles,
    EnemyBruteBoss.Attacks.SingleTargetedBoneCircle,
    EnemyBruteBoss.Attacks.ThrowAxeStraight,
    EnemyBruteBoss.Attacks.SummonSkeles
  };
  public EnemyBruteBoss.Attacks[] phaseTwoAttacks = new EnemyBruteBoss.Attacks[6]
  {
    EnemyBruteBoss.Attacks.MultiHammerAttack,
    EnemyBruteBoss.Attacks.MultiDefenciveBoneWall,
    EnemyBruteBoss.Attacks.ThrowSkeles,
    EnemyBruteBoss.Attacks.MultiTargetedBoneCircle,
    EnemyBruteBoss.Attacks.ThrowAxeStraight,
    EnemyBruteBoss.Attacks.SummonSkeles
  };
  public EnemyBruteBoss.Attacks[] phaseThreeAttacks = new EnemyBruteBoss.Attacks[6]
  {
    EnemyBruteBoss.Attacks.MultiHammerAttack,
    EnemyBruteBoss.Attacks.MultiDefenciveBoneWall,
    EnemyBruteBoss.Attacks.ThrowSkeles,
    EnemyBruteBoss.Attacks.MultiTargetedBoneCircle,
    EnemyBruteBoss.Attacks.ThrowAxeStraight,
    EnemyBruteBoss.Attacks.SummonSkeles
  };
  public int currentAttackIndex;
  public Dictionary<EnemyBruteBoss.Attacks, UnitObject.Action> attackDictionary = new Dictionary<EnemyBruteBoss.Attacks, UnitObject.Action>();
  public AsyncOperationHandle<GameObject> loadedIndicatorAsset;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableHandles = new List<AsyncOperationHandle<GameObject>>();
  public const string OutroStandupSpineEvent = "outro-standup";
  public const string OutroMaskOffSpineEvent = "outro-maskoff";
  public const string OutroAxeShakeSpineEvent = "outro-axe-shake";

  public override void Awake()
  {
    base.Awake();
    this.InitializeTraps();
    this.shockWave.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.ShockWave_OnTriggerEnterEvent);
    this.SetupAttackDictionary();
    this.LoadAssets();
    this.spineEventLister = this.GetComponent<SimpleSpineEventListener>();
    this.spineEventLister.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.defaultColliderRadius = this.damageColliderEvents.GetComponent<CircleCollider2D>().radius;
    this.ShuffleAttackArrays();
    this.simpleSpineAnimator.enabled = false;
    this.teleporter.DisableTeleporter();
  }

  public void SetupAttackDictionary()
  {
    this.attackDictionary = new Dictionary<EnemyBruteBoss.Attacks, UnitObject.Action>()
    {
      {
        EnemyBruteBoss.Attacks.SummonSkeles,
        new UnitObject.Action(this.SpawnEnemy)
      },
      {
        EnemyBruteBoss.Attacks.ThrowSkeles,
        new UnitObject.Action(this.ThrowSkeletons)
      },
      {
        EnemyBruteBoss.Attacks.ThrowAxeStraight,
        new UnitObject.Action(this.ThrowAxeStraight)
      },
      {
        EnemyBruteBoss.Attacks.HammerJumpAttack,
        new UnitObject.Action(this.HammerJumpAttack)
      },
      {
        EnemyBruteBoss.Attacks.SingleTargetedBoneCircle,
        new UnitObject.Action(this.SingleTargetedBoneCircle)
      },
      {
        EnemyBruteBoss.Attacks.MultiTargetedBoneCircle,
        new UnitObject.Action(this.MultiTargetedBoneCircle)
      },
      {
        EnemyBruteBoss.Attacks.JumpAway,
        new UnitObject.Action(this.JumpAway)
      },
      {
        EnemyBruteBoss.Attacks.SingleDefenciveBoneWall,
        new UnitObject.Action(this.SingleDefensiveBoneWall)
      },
      {
        EnemyBruteBoss.Attacks.MultiDefenciveBoneWall,
        new UnitObject.Action(this.MultiDefensiveBoneWall)
      },
      {
        EnemyBruteBoss.Attacks.ThrowAxeCircle,
        new UnitObject.Action(this.ThrowAxeCircle)
      },
      {
        EnemyBruteBoss.Attacks.HammerAttack,
        new UnitObject.Action(this.HammerAttack)
      },
      {
        EnemyBruteBoss.Attacks.Slash,
        new UnitObject.Action(this.SlashAttack)
      },
      {
        EnemyBruteBoss.Attacks.MultiHammerAttack,
        new UnitObject.Action(this.MultiHammerAttack)
      }
    };
  }

  public new void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !component.IsCharmedEnemy)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public new void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "outro-standup":
        if (string.IsNullOrEmpty(this.OutroStandupSFX))
          break;
        AudioManager.Instance.PlayOneShot(this.OutroStandupSFX);
        break;
      case "outro-maskoff":
        if (string.IsNullOrEmpty(this.OutroMaskOffSFX))
          break;
        AudioManager.Instance.PlayOneShot(this.OutroMaskOffSFX);
        break;
      case "outro-axe-shake":
        CameraManager.instance.ShakeCameraForDuration(0.25f, 0.7f, 1.7f);
        break;
    }
  }

  public new void Start()
  {
    this.Spine.Skeleton.SetSkin("Executioner_Boss");
    this.Spine.Skeleton.SetSlotsToSetupPose();
    SimulationManager.Pause();
  }

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.RingProjectile).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyBruteBoss.loadedAddressableHandles.Add(obj);
      this.loadedProjectile = obj.Result;
      this.loadedProjectile.CreatePool(this.HammerRingPojectileCount, true);
    });
    Addressables.LoadAssetAsync<GameObject>((object) this.spawnableUnit).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyBruteBoss.loadedAddressableHandles.Add(obj);
      this.loadedThrowableMinion = obj.Result;
      this.loadedThrowableMinion.CreatePool(this.MaxSpawnCount, true);
    });
    Addressables.LoadAssetAsync<GameObject>((object) this.spawnableHeavyUnit).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyBruteBoss.loadedAddressableHandles.Add(obj);
      this.loadedSpawnableHeavyUnit = obj.Result;
      this.loadedSpawnableHeavyUnit.CreatePool(this.MaxSpawnCount, true);
    });
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.IndicatorPrefab);
    asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedIndicatorAsset = obj;
      obj.Result.CreatePool(16 /*0x10*/, true);
    });
    asyncOperationHandle.WaitForCompletion();
  }

  public void ShuffleAttackArrays()
  {
    this.ShuffleAttackArray(this.phaseOneAttacks);
    this.ShuffleAttackArray(this.phaseTwoAttacks);
    this.ShuffleAttackArray(this.phaseThreeAttacks);
  }

  public override void OnDestroy()
  {
    this.CleanupSkeleThrowIndicators();
    this.CleanupEventInstances();
    this.ReleaseAddressablesHandles();
    base.OnDestroy();
    this.shockWave.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.ShockWave_OnTriggerEnterEvent);
    SimulationManager.UnPause();
  }

  public void ReleaseAddressablesHandles()
  {
    for (int index = EnemyBruteBoss.loadedAddressableHandles.Count - 1; index >= 0; --index)
      Addressables.Release<GameObject>(EnemyBruteBoss.loadedAddressableHandles[index]);
  }

  public void ShockWave_OnTriggerEnterEvent(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team != Health.Team.PlayerTeam || (double) this.shockWave.transform.localScale.x <= 0.0)
      return;
    component.DealDamage(1f, this.gameObject, component.transform.position);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    SimulationManager.Pause();
    this.simpleSpineFlash.FlashWhite(false);
    this.TargetWarning.gameObject.SetActive(false);
    this.attacking = false;
    this.counter = 0;
    GameManager.GetInstance()?.AddToCamera(this.gameObject);
    GameManager.GetInstance()?.AddPlayerToCamera();
    if ((bool) (UnityEngine.Object) PlayerFarming.Instance)
      this.givePath(PlayerFarming.Instance.transform.position);
    this.EndOfPath = this.EndOfPath + new System.Action(this.DoEndOfPathAction);
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    foreach (UnitObject throwableMinion in this.GetThrowableMinions())
    {
      throwableMinion.enabled = true;
      throwableMinion.transform.position = new Vector3(throwableMinion.transform.position.x, throwableMinion.transform.position.y, 0.0f);
      throwableMinion.state.CURRENT_STATE = StateMachine.State.Idle;
      throwableMinion.health.invincible = false;
    }
    this.EndOfPath = this.EndOfPath - new System.Action(this.DoEndOfPathAction);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    SimulationManager.UnPause();
  }

  public override void Update()
  {
    base.Update();
    this.nextAttackTimer -= Time.deltaTime * this.Spine.timeScale;
    this.checkPlayerTimer -= Time.deltaTime * this.Spine.timeScale;
    if (this.isDead)
      return;
    if (this.currentPhase != EnemyBruteBoss.Phase.Two && (double) this.health.HP < (double) this.health.totalHP * 0.6600000262260437 && (double) this.health.HP > (double) this.health.totalHP * 0.33000001311302185)
      this.BeginPhase(this.BeginPhase2IE());
    else if (this.currentPhase != EnemyBruteBoss.Phase.Three && (double) this.health.HP < (double) this.health.totalHP * 0.33000001311302185)
      this.BeginPhase(this.BeginPhase3IE());
    if ((bool) (UnityEngine.Object) PlayerFarming.Instance && (double) this.nextAttackTimer <= 0.0 && !this.attacking)
    {
      this.DoNextAttack();
    }
    else
    {
      if (this.attacking)
        return;
      this.UpdateMoving();
    }
  }

  public void DoEndOfPathAction()
  {
    if (this.isDead || !(this.Spine.AnimationState.GetCurrent(0).Animation.Name == this.walkAnimation))
      return;
    this.Spine.AnimationState.SetAnimation(0, this.idleAnimation, true);
  }

  public void DoNextAttack()
  {
    this.ClearPaths();
    this.Spine.AnimationState.SetAnimation(0, this.idleAnimation, true);
    switch (this.currentPhase)
    {
      case EnemyBruteBoss.Phase.One:
        this.DoNextAttack(this.phaseOneAttacks, ref this.currentAttackIndex);
        break;
      case EnemyBruteBoss.Phase.Two:
        this.DoNextAttack(this.phaseTwoAttacks, ref this.currentAttackIndex);
        break;
      case EnemyBruteBoss.Phase.Three:
        this.DoNextAttack(this.phaseThreeAttacks, ref this.currentAttackIndex);
        break;
    }
  }

  public void DoNextAttack(EnemyBruteBoss.Attacks[] attackArray, ref int index)
  {
    if (index >= attackArray.Length)
    {
      index = 0;
      this.ShuffleAttackArray(attackArray);
    }
    this.attackDictionary[attackArray[index]]();
    ++index;
  }

  public void ShuffleAttackArray(EnemyBruteBoss.Attacks[] attackArray)
  {
    for (int index1 = attackArray.Length - 1; index1 > 0; --index1)
    {
      int index2 = UnityEngine.Random.Range(0, index1 + 1);
      ref EnemyBruteBoss.Attacks local1 = ref attackArray[index1];
      ref EnemyBruteBoss.Attacks local2 = ref attackArray[index2];
      EnemyBruteBoss.Attacks attack1 = attackArray[index2];
      EnemyBruteBoss.Attacks attack2 = attackArray[index1];
      local1 = attack1;
      int num = (int) attack2;
      local2 = (EnemyBruteBoss.Attacks) num;
    }
  }

  public IEnumerator BeginPhase2IE()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.health.invincible = true;
    enemyBruteBoss.currentPhase = EnemyBruteBoss.Phase.Two;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    if ((UnityEngine.Object) ExecutionerAxe.AttackingAxe != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) ExecutionerAxe.AttackingAxe.gameObject);
      ExecutionerAxe.AttackingAxe = (ExecutionerAxe) null;
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyBruteBoss.gameObject, 6f);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.knockedOutStartAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.knockedOutLoopAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyBruteBoss.PhaseFallDownSFX))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.PhaseFallDownSFX);
    enemyBruteBoss.KillAllMinions();
    yield return (object) new WaitForSeconds(0.5f);
    if ((bool) (UnityEngine.Object) enemyBruteBoss.halfWayConvo)
      enemyBruteBoss.halfWayConvo.Play();
    yield return (object) new WaitForSeconds(1f);
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddToCamera(enemyBruteBoss.gameObject);
    GameManager.GetInstance().AddPlayerToCamera();
    enemyBruteBoss.health.invincible = false;
    enemyBruteBoss.timeBetweenAttacks /= 2f;
    enemyBruteBoss.attacking = false;
    if (!string.IsNullOrEmpty(enemyBruteBoss.PhaseGetUpSFX))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.PhaseGetUpSFX);
  }

  public void KillAllMinions()
  {
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null)
      {
        Health.team2[index].invincible = false;
        Health.team2[index].BloodOnDie = false;
        Health.team2[index].DealDamage(Health.team2[index].HP, Health.team2[index].gameObject, Health.team2[index].transform.position);
      }
    }
    this.minions.Clear();
  }

  public void BeginPhase(IEnumerator phaseRoutine)
  {
    this.InterruptBehaviour();
    this.StopAllActiveSFX();
    this.KillAllMinions();
    this.StartCoroutine((IEnumerator) phaseRoutine);
    this.ResetAttackIndices();
  }

  public void ResetAttackIndices() => this.currentAttackIndex = 0;

  public IEnumerator BeginPhase3IE()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.health.invincible = true;
    enemyBruteBoss.currentPhase = EnemyBruteBoss.Phase.Three;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    enemyBruteBoss.currentAttackIndex = 0;
    if ((UnityEngine.Object) ExecutionerAxe.AttackingAxe != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) ExecutionerAxe.AttackingAxe.gameObject);
      ExecutionerAxe.AttackingAxe = (ExecutionerAxe) null;
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyBruteBoss.gameObject, 6f);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.knockedOutStartAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.knockedOutLoopAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyBruteBoss.PhaseFallDownSFX))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.PhaseFallDownSFX);
    enemyBruteBoss.KillAllMinions();
    yield return (object) new WaitForSeconds(0.5f);
    if ((bool) (UnityEngine.Object) enemyBruteBoss.quarterConvo)
      enemyBruteBoss.quarterConvo.Play();
    yield return (object) new WaitForSeconds(1f);
    while (MMConversation.isPlaying)
      yield return (object) null;
    enemyBruteBoss.attacking = true;
    yield return (object) new WaitForEndOfFrame();
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.roarAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyBruteBoss.PhaseThreeStartSFX))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.PhaseThreeStartSFX);
    List<UnitObject> aliveMinions = enemyBruteBoss.GetAliveMinions();
    for (int index = 0; index < enemyBruteBoss.summonCountPhaseThree.ThrowableSkelesCount; ++index)
    {
      if (aliveMinions.Count < enemyBruteBoss.MaxSpawnCount)
        enemyBruteBoss.SpawnEnemy(enemyBruteBoss.loadedThrowableMinion, (Vector3) (UnityEngine.Random.insideUnitCircle * 5f));
    }
    for (int index = 0; index < enemyBruteBoss.summonCountPhaseThree.HeavySkelesCount; ++index)
    {
      if (aliveMinions.Count < enemyBruteBoss.MaxSpawnCount)
        enemyBruteBoss.SpawnEnemy(enemyBruteBoss.loadedSpawnableHeavyUnit, (Vector3) (UnityEngine.Random.insideUnitCircle * 5f));
    }
    GameManager.GetInstance().OnConversationNext(enemyBruteBoss.gameObject, 16f);
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.75f, 1.5f);
    yield return (object) new WaitForSeconds(2.25f);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddToCamera(enemyBruteBoss.gameObject);
    GameManager.GetInstance().AddPlayerToCamera();
    enemyBruteBoss.health.invincible = false;
    enemyBruteBoss.attacking = false;
  }

  public void ClearStatusOnDeath()
  {
    if (!(bool) (UnityEngine.Object) this.health)
      return;
    this.health.ClearAllStasisEffects();
    this.health.ClearPoison();
    this.health.CanBeBurned = false;
    this.health.CanBeFreezedInCustscene = false;
    this.health.CanBeIced = false;
    this.health.CanBeCharmed = false;
    this.health.CanBeElectrified = false;
    this.health.CanBePoisoned = false;
    this.health.CanBeTurnedIntoCritter = false;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.simpleSpineAnimator.FlashFillRed(1f);
    if (string.IsNullOrEmpty(this.GetHitVO))
      return;
    AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.BEAT_EXECUTIONER));
    this.StopAllActiveSFX();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health)
      {
        Health.team2[index].enabled = true;
        Health.team2[index].invincible = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position);
      }
    }
    if ((UnityEngine.Object) ExecutionerAxe.AttackingAxe != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) ExecutionerAxe.AttackingAxe.gameObject);
      ExecutionerAxe.AttackingAxe = (ExecutionerAxe) null;
    }
    this.ClearPaths();
    if (!this.isDead)
      this.ClearStatusOnDeath();
    this.isDead = true;
    this.InterruptBehaviour();
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.DieIE());
    }));
  }

  public IEnumerator DieIE()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.KillExecutioner);
    foreach (Behaviour collider in enemyBruteBoss.colliders)
      collider.enabled = false;
    BlackSoulUpdater.Instance.Clear();
    enemyBruteBoss.Spine.ClearState();
    GameManager.GetInstance().RemoveFromCamera(enemyBruteBoss.gameObject);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyBruteBoss.gameObject, 6f);
    GameManager.GetInstance().CameraSetOffset(Vector3.forward * -1f);
    enemyBruteBoss.state.CURRENT_STATE = StateMachine.State.Dieing;
    yield return (object) new WaitForEndOfFrame();
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.dieAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.dieLoopAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyBruteBoss.OutroDetransitionSFX))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.OutroDetransitionSFX);
    yield return (object) new WaitForSeconds(enemyBruteBoss.Spine.Skeleton.Data.FindAnimation(enemyBruteBoss.dieAnimation).Duration);
    DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.BeatenMiniBoss;
    if ((UnityEngine.Object) enemyBruteBoss.symbols != (UnityEngine.Object) null)
      enemyBruteBoss.symbols.material.DOFloat(-178f, "_Rotation", 20f);
    bool waiting = true;
    PlayerFarming.Instance.GoToAndStop(enemyBruteBoss.transform.position + Vector3.down * 2f, enemyBruteBoss.gameObject, GoToCallback: (System.Action) (() =>
    {
      waiting = false;
      PlayerFarming.Instance.state.facingAngle = PlayerFarming.Instance.state.LookAngle = 90f;
    }));
    while (waiting)
      yield return (object) null;
    PlayerFarming.Instance.EndGoToAndStop();
    enemyBruteBoss.Spine.Skeleton.SetSkin("Executioner_Defeated");
    enemyBruteBoss.Spine.Skeleton.SetSlotsToSetupPose();
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.dieLoopAnimation, true);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.dieRevealAnimation, false);
    yield return (object) new WaitForSeconds(3.4f);
    GameManager.GetInstance().OnConversationNext(enemyBruteBoss.gameObject, 7f);
    yield return (object) new WaitForSeconds(1.26666665f);
    GameManager.GetInstance().OnConversationNext(enemyBruteBoss.gameObject, 10f);
    yield return (object) new WaitForSeconds(2f);
    enemyBruteBoss.defeatedConvo.Play();
  }

  public void SpawnEnemy()
  {
    if (this.GetThrowableMinions().Count > 0)
      return;
    this.StartCoroutine((IEnumerator) this.DoSpawnEnemyRoutine());
  }

  public IEnumerator DoSpawnEnemyRoutine()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    enemyBruteBoss.rb.velocity = (Vector2) Vector3.zero;
    enemyBruteBoss.vx = enemyBruteBoss.vy = 0.0f;
    enemyBruteBoss.speed = 0.0f;
    enemyBruteBoss.DisableForces = true;
    int throwableSpawnCount = enemyBruteBoss.GetThrowableSpawnCount();
    int heavyUnitSpawnCount = enemyBruteBoss.GetHeavyUnitSpawnCount();
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.extractingGraveStartAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.extractingGraveLoopAnimation, true, 0.0f);
    string soundPath1 = heavyUnitSpawnCount > 0 ? enemyBruteBoss.AttackSkeltonSummonGuardianSFX : enemyBruteBoss.AttackSkeltonSummonBasicSFX;
    string soundPath2 = heavyUnitSpawnCount > 0 ? enemyBruteBoss.AttackSkeltonSummonGuardianVO : enemyBruteBoss.AttackSkeltonSummonBasicVO;
    if (!string.IsNullOrEmpty(soundPath1))
      enemyBruteBoss.skeletonSummonInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath1, enemyBruteBoss.transform);
    if (!string.IsNullOrEmpty(soundPath2))
      enemyBruteBoss.skeletonSummonInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath2, enemyBruteBoss.transform);
    yield return (object) new WaitForSeconds(enemyBruteBoss.pauseBeforeSpawn * enemyBruteBoss.Spine.timeScale);
    for (int index = 0; index < throwableSpawnCount; ++index)
    {
      if (enemyBruteBoss.GetAliveMinions().Count < enemyBruteBoss.MaxSpawnCount)
      {
        Vector3 point = (Vector3) (UnityEngine.Random.insideUnitCircle * 5f);
        Vector3 closestPoint;
        bool flag = BiomeGenerator.PointWithinIsland(point, out closestPoint);
        enemyBruteBoss.SpawnEnemy(enemyBruteBoss.loadedThrowableMinion, flag ? point : closestPoint);
      }
    }
    for (int index = 0; index < heavyUnitSpawnCount; ++index)
    {
      if (enemyBruteBoss.GetAliveMinions().Count < enemyBruteBoss.MaxSpawnCount)
      {
        Vector3 point = (Vector3) (UnityEngine.Random.insideUnitCircle * 5f);
        Vector3 closestPoint;
        bool flag = BiomeGenerator.PointWithinIsland(point, out closestPoint);
        enemyBruteBoss.SpawnEnemy(enemyBruteBoss.loadedSpawnableHeavyUnit, flag ? point : closestPoint);
      }
    }
    yield return (object) new WaitForSeconds(enemyBruteBoss.spawnAnimationDuration * enemyBruteBoss.Spine.timeScale);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.extractingGraveEndAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
    enemyBruteBoss.DisableForces = false;
    yield return (object) new WaitForSeconds(enemyBruteBoss.spawnEnemyCooldown * enemyBruteBoss.Spine.timeScale);
    enemyBruteBoss.attacking = false;
  }

  public void SpawnEnemy(GameObject spawnable, Vector3 pos)
  {
    if (!((UnityEngine.Object) spawnable != (UnityEngine.Object) null))
      return;
    UnitObject component1 = ObjectPool.Spawn(spawnable, this.transform.parent, pos)?.GetComponent<UnitObject>();
    this.minions.Add(component1);
    if (component1 is EnemySimpleGuardian)
      ((EnemySimpleGuardian) component1).GraveSpawn();
    if (component1 is EnemySwordsman)
      ((EnemySwordsman) component1).GraveSpawn(true, false);
    component1.health.CanIncreaseDamageMultiplier = false;
    DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    component2.SetAllowExtraItems(false);
  }

  public int GetThrowableSpawnCount()
  {
    int throwableSpawnCount = 0;
    switch (this.currentPhase)
    {
      case EnemyBruteBoss.Phase.One:
        throwableSpawnCount = this.summonCountPhaseOne.ThrowableSkelesCount;
        break;
      case EnemyBruteBoss.Phase.Two:
        throwableSpawnCount = this.summonCountPhaseTwo.ThrowableSkelesCount;
        break;
      case EnemyBruteBoss.Phase.Three:
        throwableSpawnCount = this.summonCountPhaseThree.ThrowableSkelesCount;
        break;
    }
    return throwableSpawnCount;
  }

  public int GetHeavyUnitSpawnCount()
  {
    int heavyUnitSpawnCount = 0;
    switch (this.currentPhase)
    {
      case EnemyBruteBoss.Phase.One:
        heavyUnitSpawnCount = this.summonCountPhaseOne.HeavySkelesCount;
        break;
      case EnemyBruteBoss.Phase.Two:
        heavyUnitSpawnCount = this.summonCountPhaseTwo.HeavySkelesCount;
        break;
      case EnemyBruteBoss.Phase.Three:
        heavyUnitSpawnCount = this.summonCountPhaseThree.HeavySkelesCount;
        break;
    }
    return heavyUnitSpawnCount;
  }

  public void ThrowSkeletons() => this.StartCoroutine((IEnumerator) this.DoThrowMinionsAttack());

  public void InterruptBehaviour()
  {
    this.StopAllCoroutines();
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.DisableForces = false;
    this.CleanupSkeleThrowIndicators();
    this.CleanupLava();
    this.RemoveBurningFromPlayer();
  }

  public void StopAllActiveSFX()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonSummonInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonSummonInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonLiftInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonLiftInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonTossInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonTossInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.jumpStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.jumpStartInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallStartInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallMultiBurstInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallMultiBurstInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallChasingInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallChasingInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.axeThrowStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.axeThrowStartInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerStartInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerSwingInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerSwingInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerReturnInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void CleanupSkeleThrowIndicators()
  {
    for (int index = 0; index < this.skeleThrowIndicators.Count; ++index)
      ObjectPool.Recycle(this.skeleThrowIndicators[index]);
    this.skeleThrowIndicators.Clear();
  }

  public void CleanupEventInstances()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonSummonInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonSummonInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonLiftInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonLiftInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonTossInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.skeletonTossInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.jumpStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.jumpStartInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallStartInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallMultiBurstInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallMultiBurstInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallChasingInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.boneWallChasingInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.axeThrowStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.axeThrowStartInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerStartInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerStartInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerSwingInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerSwingInstanceVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.hammerReturnInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopLoop(this.prayingLoopInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void CleanupLava()
  {
    TrapLava[] objectsOfType = UnityEngine.Object.FindObjectsOfType<TrapLava>();
    for (int index = objectsOfType.Length - 1; index >= 0; --index)
      objectsOfType[index].DespawnLava();
  }

  public void RemoveBurningFromPlayer()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      player.health.ClearBurn();
  }

  public IEnumerator DoThrowMinionsAttack()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.speed = 0.0f;
    enemyBruteBoss.ClearPaths();
    enemyBruteBoss.rb.velocity = (Vector2) Vector3.zero;
    enemyBruteBoss.vx = enemyBruteBoss.vy = 0.0f;
    enemyBruteBoss.speed = 0.0f;
    enemyBruteBoss.DisableForces = true;
    float seconds = 1f;
    float delayInBetweenThrowAnimAndThrow = 0.3f;
    List<UnitObject> availableMinions = enemyBruteBoss.GetThrowableMinions();
    float progress = 0.0f;
    if (availableMinions.Count > 0)
    {
      enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.extractingGraveStartAnimation, false);
      enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.extractingGraveLoopAnimation, true, 0.0f);
      if (!string.IsNullOrEmpty(enemyBruteBoss.AttackSkeletonThrowLiftSFX))
        enemyBruteBoss.skeletonLiftInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackSkeletonThrowLiftSFX, enemyBruteBoss.transform);
      if (!string.IsNullOrEmpty(enemyBruteBoss.AttackSkeletonThrowLiftVO))
        enemyBruteBoss.skeletonLiftInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackSkeletonThrowLiftVO, enemyBruteBoss.transform);
      yield return (object) CoroutineStatics.WaitForScaledSeconds(seconds, enemyBruteBoss.Spine);
      foreach (UnitObject minion in availableMinions)
      {
        if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
        {
          EnemySwordsman component1 = minion.GetComponent<EnemySwordsman>();
          Health component2 = minion.GetComponent<Health>();
          enemyBruteBoss.DisableMinionForThrowing(minion, component2);
          enemyBruteBoss.StartCoroutine((IEnumerator) enemyBruteBoss.RaiseMinionIntoAirForThrowing(minion, component1.Spine));
        }
      }
      while ((double) progress < (double) enemyBruteBoss.levitateEnemiesAnimDuration)
      {
        if (enemyBruteBoss.IsUnitObjectListContentsNull(availableMinions))
        {
          enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.extractingGraveEndAnimation, false);
          enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
          enemyBruteBoss.ThrowMinionsCleanup();
          yield break;
        }
        progress += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
      enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.extractingGraveEndAnimation, false);
      enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
      progress = 0.0f;
      while ((double) progress < (double) enemyBruteBoss.PauseBeforeThrow)
      {
        if (enemyBruteBoss.IsUnitObjectListContentsNull(availableMinions))
        {
          enemyBruteBoss.ThrowMinionsCleanup();
          yield break;
        }
        progress += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
      enemyBruteBoss.state.CURRENT_STATE = StateMachine.State.Attacking;
      for (int i = 0; i < availableMinions.Count; ++i)
      {
        if (!((UnityEngine.Object) availableMinions[i] == (UnityEngine.Object) null))
        {
          enemyBruteBoss.TargetObject = enemyBruteBoss.GetClosestTarget().gameObject;
          enemyBruteBoss.LookAtTarget();
          enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.throwMinionAnimation, false);
          enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
          if (!string.IsNullOrEmpty(enemyBruteBoss.AttackSkeletonThrowTossSFX))
            enemyBruteBoss.skeletonTossInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackSkeletonThrowTossSFX, enemyBruteBoss.transform);
          if (!string.IsNullOrEmpty(enemyBruteBoss.AttackSkeletonThrowTossVO))
            enemyBruteBoss.skeletonTossInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackSkeletonThrowTossVO, enemyBruteBoss.transform);
          yield return (object) CoroutineStatics.WaitForScaledSeconds(delayInBetweenThrowAnimAndThrow, enemyBruteBoss.Spine);
          if ((UnityEngine.Object) availableMinions[i] != (UnityEngine.Object) null)
          {
            Health component = availableMinions[i].GetComponent<Health>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.HP > 0.0)
            {
              yield return (object) enemyBruteBoss.StartCoroutine((IEnumerator) enemyBruteBoss.MoveMinionTowardsTarget(availableMinions[i], component));
              float t = 0.0f;
              while ((double) t < (double) enemyBruteBoss.DelayBetweenMinionThrow)
              {
                t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
                yield return (object) null;
              }
            }
          }
        }
      }
    }
    progress = 0.0f;
    while ((double) progress < (double) enemyBruteBoss.throwMinionCooldown)
    {
      progress += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.ThrowMinionsCleanup();
  }

  public void ThrowMinionsCleanup()
  {
    this.DisableForces = false;
    this.attacking = false;
    this.simpleSpineFlash.FlashWhite(false);
    this.InterruptBehaviour();
  }

  public bool IsUnitObjectListContentsNull(List<UnitObject> unitList)
  {
    for (int index = 0; index < unitList.Count; ++index)
    {
      if ((UnityEngine.Object) unitList[index] != (UnityEngine.Object) null)
        return false;
    }
    return true;
  }

  public void DisableMinionForThrowing(UnitObject minion, Health minionHealth)
  {
    minion.state.CURRENT_STATE = StateMachine.State.Idle;
    minion.enabled = false;
  }

  public IEnumerator RaiseMinionIntoAirForThrowing(UnitObject minion, SkeletonAnimation minionSpine)
  {
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null && (double) minion.health.HP > 0.0)
    {
      float progress = 0.0f;
      minionSpine.AnimationState.SetAnimation(0, this.lambSkeleFloatAnim, true);
      minion.state.CURRENT_STATE = StateMachine.State.Flying;
      minion.transform.DOKill();
      minion.transform.DOMoveZ(-this.MinionRiseHeight, this.MinionRiseDuration * minionSpine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      while ((double) progress < (double) this.MinionRiseDuration && !((UnityEngine.Object) minion == (UnityEngine.Object) null))
      {
        progress += Time.deltaTime * this.Spine.timeScale;
        yield return (object) null;
      }
    }
  }

  public IEnumerator MoveMinionTowardsTarget(UnitObject minion, Health minionHealth)
  {
    EnemyBruteBoss enemyBruteBoss = this;
    if (!((UnityEngine.Object) minion == (UnityEngine.Object) null))
    {
      Vector3 position = enemyBruteBoss.GetClosestTarget().transform.position;
      float num = 10f;
      float attackDuration = Vector3.Distance(minion.transform.position, position) / num;
      GameObject indicator = ObjectPool.Spawn(enemyBruteBoss.loadedIndicatorAsset.Result, enemyBruteBoss.transform.parent, position + new Vector3(0.0f, 0.0f, -0.1f), Quaternion.identity);
      enemyBruteBoss.skeleThrowIndicators.Add(indicator);
      minion.transform.DOMove(position, attackDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
      float timer = 0.0f;
      while ((double) timer < (double) attackDuration)
      {
        if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
          timer = attackDuration;
        timer += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
      ObjectPool.Recycle(indicator);
      if (enemyBruteBoss.skeleThrowIndicators.Contains(indicator))
        enemyBruteBoss.skeleThrowIndicators.Remove(indicator);
      if (!((UnityEngine.Object) minion == (UnityEngine.Object) null))
      {
        CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
        BiomeConstants.Instance.EmitHammerEffects(minion.transform.position);
        Explosion.CreateExplosion(minion.transform.position, enemyBruteBoss.health.team, enemyBruteBoss.health, 1f, playSFX: false);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        if (!string.IsNullOrEmpty(enemyBruteBoss.AttackSkeletonThrowExplodeSFX))
          AudioManager.Instance.PlayOneShot(enemyBruteBoss.AttackSkeletonThrowExplodeSFX, minion.transform.position);
        minion.state.CURRENT_STATE = StateMachine.State.Dieing;
        minionHealth.invincible = false;
        minionHealth.DealDamage(float.PositiveInfinity, PlayerFarming.Instance.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
      }
    }
  }

  public List<UnitObject> GetThrowableMinions()
  {
    List<UnitObject> throwableMinions = new List<UnitObject>();
    for (int index = this.minions.Count - 1; index >= 0; --index)
    {
      if ((double) this.minions[index].health.HP > 0.0 && this.minions[index].health.team == Health.Team.Team2 && this.minions[index] is EnemySwordsman)
        throwableMinions.Add(this.minions[index]);
    }
    return throwableMinions;
  }

  public List<UnitObject> GetAliveMinions()
  {
    List<UnitObject> aliveMinions = new List<UnitObject>();
    for (int index = this.minions.Count - 1; index >= 0; --index)
    {
      if ((double) this.minions[index].health.HP > 0.0 && this.minions[index].health.team == Health.Team.Team2 && (bool) (UnityEngine.Object) this.minions[index])
        aliveMinions.Add(this.minions[index]);
    }
    return aliveMinions;
  }

  public void UpdateMoving()
  {
    if (!(bool) (UnityEngine.Object) PlayerFarming.Instance || (double) this.checkPlayerTimer > 0.0 || !GameManager.RoomActive)
      return;
    this.checkPlayerTimer = this.checkPlayerInterval;
    Vector3 vector3 = PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
    if ((double) Vector3.Distance(this.transform.position, vector3) <= (double) this.StoppingDistance)
      return;
    this.givePath(vector3);
    if (this.Spine.AnimationState.GetCurrent(0).Animation.Name != this.walkAnimation)
      this.Spine.AnimationState.SetAnimation(0, this.walkAnimation, true);
    this.LookAtAngle(Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position));
  }

  public void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public void LookAtTarget()
  {
    if (!((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null))
      return;
    this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
  }

  public void HammerAttack()
  {
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.HammerAttackIE());
  }

  public void HammerJumpAttack() => this.StartCoroutine((IEnumerator) this.HammerAttackIE(true));

  public void MultiHammerAttack()
  {
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.DoMultiHammerAttack());
  }

  public IEnumerator HammerAttackIE(bool jumpAttack = false)
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.speed = 0.0f;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    float angle = Utils.GetAngle(enemyBruteBoss.transform.position, PlayerFarming.Instance.transform.position);
    enemyBruteBoss.LookAtAngle(angle);
    if (jumpAttack)
    {
      Vector3 normalized = (enemyBruteBoss.transform.position - PlayerFarming.Instance.transform.position).normalized;
      yield return (object) enemyBruteBoss.StartCoroutine((IEnumerator) enemyBruteBoss.JumpAwayIE(PlayerFarming.Instance.transform.position + normalized * 4f, enemyBruteBoss.jumpDuration / 2f, false, false));
      enemyBruteBoss.damageColliderEvents.transform.position = new Vector3(enemyBruteBoss.AxeBone.position.x, enemyBruteBoss.AxeBone.position.y, 0.0f);
    }
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.shockWaveStartAnimation, false);
    enemyBruteBoss.damageColliderEvents.transform.position = new Vector3(enemyBruteBoss.AxeBone.position.x, enemyBruteBoss.AxeBone.position.y, 0.0f);
    enemyBruteBoss.TargetWarning.transform.position = new Vector3(enemyBruteBoss.AxeBone.position.x, enemyBruteBoss.AxeBone.position.y, 0.0f);
    enemyBruteBoss.TargetWarning.gameObject.SetActive(true);
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackHammerSlamSingleStartSFX))
      enemyBruteBoss.hammerStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackHammerSlamSingleStartSFX, enemyBruteBoss.transform);
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackHammerSlamSingleStartVO))
      enemyBruteBoss.hammerStartInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackHammerSlamSingleStartVO, enemyBruteBoss.transform);
    float anticipation = jumpAttack ? enemyBruteBoss.jumpDuration + 0.35f : enemyBruteBoss.hammerAttackAnticipation;
    float t = 0.0f;
    while ((double) t < (double) anticipation)
    {
      t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.simpleSpineFlash.FlashWhite(false);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.shockWaveEndAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackHammerSlamSingleSwingSFX))
      enemyBruteBoss.hammerSwingInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackHammerSlamSingleSwingSFX, enemyBruteBoss.transform);
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackHammerSlamSingleSwingVO))
      enemyBruteBoss.hammerSwingInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackHammerSlamSingleSwingVO, enemyBruteBoss.transform);
    t = 0.0f;
    while ((double) t < (double) enemyBruteBoss.HammerImpactEffectDelay)
    {
      t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.SpawnProjectileRing();
    enemyBruteBoss.ShakeGraves();
    CameraManager.instance.ShakeCameraForDuration(0.75f, 1.25f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    enemyBruteBoss.speed = 0.0f;
    enemyBruteBoss.ParticleSystem.Play();
    enemyBruteBoss.TargetWarning.gameObject.SetActive(false);
    enemyBruteBoss.damageColliderEvents.SetActive(true);
    enemyBruteBoss.ParticleSystem.transform.position = new Vector3(enemyBruteBoss.AxeBone.position.x, enemyBruteBoss.AxeBone.position.y, 0.0f);
    t = 0.0f;
    while ((double) t < 0.10000000149011612)
    {
      t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackHammerSlamSingleReturnSFX))
      enemyBruteBoss.hammerReturnInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackHammerSlamSingleReturnSFX, enemyBruteBoss.transform);
    enemyBruteBoss.damageColliderEvents.SetActive(false);
    t = 0.0f;
    while ((double) t < (double) enemyBruteBoss.hammerAttackCooldown)
    {
      t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.attacking = false;
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.idleAnimation, true);
  }

  public IEnumerator DoMultiHammerAttack()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.speed = 0.0f;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    int attackCount = (int) (enemyBruteBoss.currentPhase + 1);
    float lastAngle = Utils.GetAngle(enemyBruteBoss.transform.position, PlayerFarming.Instance.transform.position);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.shockWaveStartAnimation, false);
    string soundPath1 = attackCount == 2 ? enemyBruteBoss.AttackHammerSlamDoubleStartSFX : enemyBruteBoss.AttackHammerSlamTripleStartSFX;
    string soundPath2 = attackCount == 2 ? enemyBruteBoss.AttackHammerSlamDoubleStartVO : enemyBruteBoss.AttackHammerSlamTripleStartVO;
    if (!string.IsNullOrEmpty(soundPath1))
      enemyBruteBoss.hammerStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath1, enemyBruteBoss.transform);
    if (!string.IsNullOrEmpty(soundPath2))
      enemyBruteBoss.hammerStartInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath2, enemyBruteBoss.transform);
    float t = 0.0f;
    while ((double) t < (double) enemyBruteBoss.hammerAttackAnticipation)
    {
      t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.simpleSpineFlash.FlashWhite(false);
    for (int i = 0; i < attackCount; ++i)
    {
      enemyBruteBoss.LookAtAngle(lastAngle);
      lastAngle -= 180f;
      enemyBruteBoss.damageColliderEvents.transform.position = new Vector3(enemyBruteBoss.AxeBone.position.x, enemyBruteBoss.AxeBone.position.y, 0.0f);
      enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.shockWaveEndAnimation, false);
      enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
      string soundPath3 = attackCount == 2 ? enemyBruteBoss.AttackHammerSlamDoubleSwingSFX : enemyBruteBoss.AttackHammerSlamTripleSwingSFX;
      string soundPath4 = attackCount == 2 ? enemyBruteBoss.AttackHammerSlamDoubleSwingVO : enemyBruteBoss.AttackHammerSlamTripleSwingVO;
      if (!string.IsNullOrEmpty(soundPath3))
        enemyBruteBoss.hammerSwingInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath3, enemyBruteBoss.transform);
      if (!string.IsNullOrEmpty(soundPath4))
        enemyBruteBoss.hammerSwingInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath4, enemyBruteBoss.transform);
      t = 0.0f;
      while ((double) t < (double) enemyBruteBoss.HammerImpactEffectDelay)
      {
        t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
      enemyBruteBoss.SpawnProjectileRing();
      CameraManager.instance.ShakeCameraForDuration(0.75f, 1.25f, 0.3f);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      enemyBruteBoss.speed = 0.0f;
      enemyBruteBoss.ParticleSystem.Play();
      enemyBruteBoss.TargetWarning.gameObject.SetActive(false);
      enemyBruteBoss.damageColliderEvents.SetActive(true);
      enemyBruteBoss.ParticleSystem.transform.position = new Vector3(enemyBruteBoss.AxeBone.position.x, enemyBruteBoss.AxeBone.position.y, 0.0f);
      yield return (object) new WaitForSeconds(0.1f);
      enemyBruteBoss.damageColliderEvents.SetActive(false);
      string soundPath5 = attackCount == 2 ? enemyBruteBoss.AttackHammerSlamDoubleReturnSFX : enemyBruteBoss.AttackHammerSlamTripleReturnSFX;
      if (!string.IsNullOrEmpty(soundPath5))
        enemyBruteBoss.hammerReturnInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath5, enemyBruteBoss.transform);
      t = 0.0f;
      while ((double) t < (double) enemyBruteBoss.pauseBetweenHammerStrikes)
      {
        t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
    }
    t = 0.0f;
    while ((double) t < (double) enemyBruteBoss.hammerAttackCooldown)
    {
      t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.idleAnimation, true);
    enemyBruteBoss.attacking = false;
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
  }

  public void SpawnProjectileRing()
  {
    float num1 = 0.0f;
    float num2 = 360f;
    AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", this.transform.position);
    if ((UnityEngine.Object) this.loadedProjectile == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "Tried to spawn projectile ring but loadedProjectile was null");
    }
    else
    {
      for (int index = 0; index < this.HammerRingPojectileCount; ++index)
      {
        Projectile component = ObjectPool.Spawn(this.loadedProjectile, this.transform.parent)?.GetComponent<Projectile>();
        if ((bool) (UnityEngine.Object) component)
        {
          component.transform.position = new Vector3(this.AxeBone.position.x, this.AxeBone.position.y, 0.0f);
          component.Angle = num1;
          component.team = this.health.team;
          component.Speed = this.ProjectileSpeed;
          component.Owner = this.health;
          num1 += num2 / (float) Mathf.Max(this.HammerRingPojectileCount, 0);
        }
      }
    }
  }

  public void SlashAttack() => this.StartCoroutine((IEnumerator) this.DoSlashAttack());

  public IEnumerator DoSlashAttack()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.speed = 0.0f;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    float angle = Utils.GetAngle(enemyBruteBoss.transform.position, PlayerFarming.Instance.transform.position);
    enemyBruteBoss.LookAtAngle(angle);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.swipeAnimation, false);
    float t = 0.0f;
    while ((double) t < (double) enemyBruteBoss.swipeAttackAnticipation)
    {
      double num = (double) t / (double) enemyBruteBoss.swipeAttackAnticipation;
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyBruteBoss.simpleSpineFlash.FlashWhite(false);
    if (!string.IsNullOrEmpty(enemyBruteBoss.swipeAttackSoundPath))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.swipeAttackSoundPath, enemyBruteBoss.transform.position);
    CircleCollider2D circleCollider = enemyBruteBoss.damageColliderEvents.GetComponent<CircleCollider2D>();
    circleCollider.radius = enemyBruteBoss.slashColliderRadius;
    enemyBruteBoss.damageColliderEvents.SetActive(true);
    enemyBruteBoss.damageColliderEvents.transform.localPosition = new Vector3(0.0f, -1f, 0.0f);
    yield return (object) new WaitForSeconds(0.1f);
    enemyBruteBoss.damageColliderEvents.SetActive(false);
    circleCollider.radius = enemyBruteBoss.defaultColliderRadius;
    yield return (object) new WaitForSeconds(enemyBruteBoss.swipeAttackCooldown);
    enemyBruteBoss.attacking = false;
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.idleAnimation, true);
  }

  public void ShockWave() => this.StartCoroutine((IEnumerator) this.ShockWaveIE());

  public IEnumerator ShockWaveIE()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    float angle = Utils.GetAngle(enemyBruteBoss.transform.position, PlayerFarming.Instance.transform.position);
    enemyBruteBoss.LookAtAngle(angle);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.shockWaveStartAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.shockWaveLoopAnimation, true, 0.0f);
    float t = 0.0f;
    while ((double) t < 2.0)
    {
      double num = (double) t / 2.0;
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyBruteBoss.simpleSpineFlash.FlashWhite(false);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.shockWaveEndAnimation, false);
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    if (!string.IsNullOrEmpty(enemyBruteBoss.areaAttackSoundPath))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.areaAttackSoundPath, enemyBruteBoss.transform.position);
    enemyBruteBoss.shockWave.transform.position = enemyBruteBoss.transform.position;
    enemyBruteBoss.shockWave.transform.localScale = Vector3.zero;
    enemyBruteBoss.shockWave.transform.DOScale(25f, 3.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    enemyBruteBoss.SpawnEnemy();
    yield return (object) new WaitForSeconds(enemyBruteBoss.hammerAttackCooldown);
    enemyBruteBoss.attacking = false;
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.idleAnimation, true);
  }

  public void ThrowAttack() => this.StartCoroutine((IEnumerator) this.ThrowAttackIE());

  public IEnumerator ThrowAttackIE()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    Vector3 position = PlayerFarming.Instance.transform.position;
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.throwAnimation, false);
    enemyBruteBoss.ClearPaths();
    float angle = Utils.GetAngle(enemyBruteBoss.transform.position, PlayerFarming.Instance.transform.position);
    enemyBruteBoss.LookAtAngle(angle);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBruteBoss.Spine.timeScale) < 1.7666666507720947)
      yield return (object) null;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBruteBoss.Spine.timeScale) < 0.699999988079071)
    {
      double num = (double) time / 0.699999988079071;
      yield return (object) null;
    }
    enemyBruteBoss.simpleSpineFlash.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyBruteBoss.Spine.timeScale) < 0.73333334922790527)
      yield return (object) null;
    enemyBruteBoss.attacking = false;
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.idleAnimation, true);
  }

  public void ThrowAxeStraight() => this.StartCoroutine((IEnumerator) this.ThrowAxeIE());

  public IEnumerator ThrowAxeIE()
  {
    EnemyBruteBoss boss = this;
    boss.attacking = true;
    boss.ClearPaths();
    Health closestTarget = boss.GetClosestTarget();
    if ((double) Vector3.Distance(boss.transform.position, closestTarget.transform.position) < (double) boss.closeRangeDistance)
      yield return (object) boss.StartCoroutine((IEnumerator) boss.JumpAwayIE(boss.GetPositionAwayFromPlayer(), boss.jumpDuration, setAttackFinishedAtEnd: false));
    yield return (object) new WaitForEndOfFrame();
    boss.Spine.AnimationState.SetAnimation(0, boss.throwAxeAnimation, false);
    boss.Spine.AnimationState.AddAnimation(0, boss.throwAxeLoopAnimation, true, 0.0f);
    boss.LookAtAngle(Utils.GetAngle(boss.transform.position, PlayerFarming.Instance.transform.position));
    if (!string.IsNullOrEmpty(boss.AttackAxeThrowStartSFX))
      boss.axeThrowStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(boss.AttackAxeThrowStartSFX, boss.transform);
    if (!string.IsNullOrEmpty(boss.AttackAxeThrowStartVO))
      boss.axeThrowStartInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(boss.AttackAxeThrowStartVO, boss.transform);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * boss.Spine.timeScale) < (double) boss.axeAnticipation)
    {
      double num = (double) time / (double) boss.axeAnticipation;
      boss.LookAtAngle(Utils.GetAngle(boss.transform.position, PlayerFarming.Instance.transform.position));
      yield return (object) null;
    }
    boss.simpleSpineFlash.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    ExecutionerAxe executionerAxe = (ExecutionerAxe) null;
    ExecutionerAxe.SpawnThrowingAxe(boss, (Health) PlayerFarming.Instance.health, boss.axeForce, boss.axeDuration, (System.Action<ExecutionerAxe>) (axe => executionerAxe = axe));
    yield return (object) new WaitForSeconds(boss.axeDuration);
    while (!executionerAxe.Destroying)
      yield return (object) null;
    boss.Spine.AnimationState.SetAnimation(0, boss.catchAxeAnimation, false);
    boss.Spine.AnimationState.AddAnimation(0, boss.idleAnimation, true, 0.0f);
    if (!string.IsNullOrEmpty(boss.AttackAxeThrowCatchSFX))
      AudioManager.Instance.PlayOneShot(boss.AttackAxeThrowCatchSFX, boss.transform.position);
    if (!string.IsNullOrEmpty(boss.AttackAxeThrowCatchVO))
      AudioManager.Instance.PlayOneShot(boss.AttackAxeThrowCatchVO, boss.transform.position);
    yield return (object) new WaitForSeconds(1f);
    boss.attacking = false;
    boss.nextAttackTimer = UnityEngine.Random.Range(boss.timeBetweenAttacks.x, boss.timeBetweenAttacks.y);
  }

  public void ThrowAxeCircle() => this.StartCoroutine((IEnumerator) this.ThrowAxeCircleIE());

  public IEnumerator ThrowAxeCircleIE()
  {
    EnemyBruteBoss boss = this;
    boss.attacking = true;
    boss.ClearPaths();
    yield return (object) new WaitForEndOfFrame();
    boss.Spine.AnimationState.SetAnimation(0, boss.throwAxeLongAnimation, false);
    boss.Spine.AnimationState.AddAnimation(0, boss.throwAxeLoopLongAnimation, true, 0.0f);
    boss.LookAtAngle(Utils.GetAngle(boss.transform.position, PlayerFarming.Instance.transform.position));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * boss.Spine.timeScale) < (double) boss.axeAnticipation)
    {
      double num = (double) time / (double) boss.axeAnticipation;
      yield return (object) null;
    }
    boss.simpleSpineFlash.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    ExecutionerAxe executionerAxe = (ExecutionerAxe) null;
    ExecutionerAxe.SpawnThrowingAxe(boss, (Health) PlayerFarming.Instance.health, boss.circleAxeDuration, boss.circleAxeDuration, (System.Action<ExecutionerAxe>) (axe => executionerAxe = axe));
    while ((UnityEngine.Object) executionerAxe == (UnityEngine.Object) null)
      yield return (object) null;
    time = 0.0f;
    while ((UnityEngine.Object) executionerAxe != (UnityEngine.Object) null && ((double) (time += Time.deltaTime * boss.Spine.timeScale) < (double) boss.axeDuration || !executionerAxe.Destroying))
    {
      Vector3 vector3 = (executionerAxe.transform.position - boss.transform.position).normalized;
      vector3 = new Vector3(vector3.y, -vector3.x);
      float num = Vector3.Distance(executionerAxe.transform.position, boss.transform.position);
      executionerAxe.transform.position += vector3 * boss.angularVelocity * num * Time.deltaTime;
      yield return (object) null;
    }
    boss.Spine.AnimationState.SetAnimation(0, boss.catchAxeAnimation, false);
    boss.Spine.AnimationState.AddAnimation(0, boss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    boss.attacking = false;
    boss.nextAttackTimer = UnityEngine.Random.Range(boss.timeBetweenAttacks.x, boss.timeBetweenAttacks.y);
  }

  public override IEnumerator ChasePlayer()
  {
    yield break;
  }

  public void PardonExecuteChoice()
  {
    this.StartCoroutine((IEnumerator) this.PardonExecuteChoiceIE());
  }

  public IEnumerator PardonExecuteChoiceIE()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    DataManager.Instance.ExecutionerDefeated = true;
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
    choice.Offset = new Vector3(0.0f, -300f);
    enemyBruteBoss.prayingLoopInstanceSFX = AudioManager.Instance.CreateLoop(enemyBruteBoss.outroPrayingLoopSFX, enemyBruteBoss.gameObject, true);
    bool pardoned = false;
    choice.Show(LocalizationManager.GetTranslation("Conversation_NPC/Executioner/Defeated/Choice_Pardon"), LocalizationManager.GetTranslation("Conversation_NPC/Executioner/Defeated/Choice_Execute"), (System.Action) (() =>
    {
      pardoned = true;
      DataManager.Instance.ExecutionerPardoned = true;
      DataManager.Instance.ExecutionerPardonedDay = TimeManager.CurrentDay;
      AudioManager.Instance.StopLoop(this.prayingLoopInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    }), (System.Action) (() =>
    {
      DataManager.Instance.ExecutionerDamned = true;
      this.damnConvo.Play();
      AudioManager.Instance.StopLoop(this.prayingLoopInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    }), enemyBruteBoss.transform.position);
    while ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      choice.UpdatePosition(enemyBruteBoss.transform.position);
      yield return (object) null;
    }
    yield return (object) null;
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().CameraSetOffset(Vector3.forward * -2f);
    enemyBruteBoss.Spine.ClearState();
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, pardoned ? "choice-absolve" : "choice-damn", false);
    string soundPath = pardoned ? enemyBruteBoss.OutroPardonSFX : enemyBruteBoss.OutroExecuteSFX;
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "break-axe", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    float duration = enemyBruteBoss.Spine.skeleton.Data.FindAnimation("choice-absolve").Duration;
    if (pardoned)
    {
      enemyBruteBoss.Spine.AnimationState.AddAnimation(0, "idle_beaten", true, 0.0f);
      enemyBruteBoss.Spine.Skeleton.SetSkin("Executioner_Absolved_Clean");
      yield return (object) new WaitForSeconds(duration);
      yield return (object) new WaitForSeconds(1.7f);
      yield return (object) new WaitForEndOfFrame();
      if (!DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Axe_Legendary) && DataManager.Instance.OnboardedLegendaryWeapons)
        yield return (object) enemyBruteBoss.StartCoroutine((IEnumerator) enemyBruteBoss.PlayerPickUpLegendaryAxe());
      enemyBruteBoss.pardonConvo.Play();
      yield return (object) null;
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    else
    {
      yield return (object) new WaitForSeconds(1.66666663f);
      CameraManager.instance.ShakeCameraForDuration(0.25f, 0.7f, 1.7f);
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
      yield return (object) new WaitForSeconds(1.7f);
      CameraManager.instance.ShakeCameraForDuration(1.2f, 1.5f, 0.1f);
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
      yield return (object) new WaitForSeconds(2.16666675f);
      if (!DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Axe_Legendary) && DataManager.Instance.OnboardedLegendaryWeapons)
        yield return (object) enemyBruteBoss.StartCoroutine((IEnumerator) enemyBruteBoss.PlayerPickUpLegendaryAxe());
      yield return (object) new WaitForSeconds(0.5f);
      if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER))
        yield return (object) enemyBruteBoss.StartCoroutine((IEnumerator) enemyBruteBoss.PlayerPickUpTrophyDeco());
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      enemyBruteBoss.Spine.gameObject.SetActive(false);
      foreach (Behaviour collider in enemyBruteBoss.colliders)
        collider.enabled = false;
      BiomeConstants.Instance.EmitSmokeExplosionVFX(enemyBruteBoss.Spine.transform.position);
      enemyBruteBoss.GetComponent<SpawnDeadBodyOnDeath>().DropBody(enemyBruteBoss.gameObject);
      AudioManager.Instance.PlayAtmos(enemyBruteBoss.OutroCorpseAppearsSFX);
      yield return (object) new WaitForSeconds(1.5f);
    }
    RoomLockController.RoomCompleted(true);
    GameManager.GetInstance().OnConversationNew();
    if ((bool) (UnityEngine.Object) enemyBruteBoss.teleporter)
      GameManager.GetInstance().OnConversationNext(enemyBruteBoss.teleporter.gameObject);
    AudioManager.Instance.PlayOneShot(enemyBruteBoss.OutroCameraWhooshSFX);
    yield return (object) new WaitForSeconds(1f);
    if ((bool) (UnityEngine.Object) enemyBruteBoss.teleporter)
    {
      enemyBruteBoss.teleporter.EnableTeleporter(Vector3.one * 1.45f);
      yield return (object) new WaitForSeconds(2f);
    }
    DataManager.Instance.BeatenExecutioner = true;
    GameManager.GetInstance().OnConversationEnd();
    enemyBruteBoss.simpleSpineAnimator.enabled = true;
    enemyBruteBoss.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-beaten");
    enemyBruteBoss.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle_beaten");
    enemyBruteBoss.maxSpeed /= 4f;
    if (pardoned)
      enemyBruteBoss.LeveaRoom();
  }

  public void JumpAway()
  {
    Debug.Log((object) "JumpAway!!!");
    this.StartCoroutine((IEnumerator) this.JumpAwayIE(this.GetPositionAwayFromPlayer(), this.jumpDuration));
  }

  public IEnumerator JumpAwayIE(
    Vector3 targetPosition,
    float jumpDuration,
    bool animateLanding = true,
    bool setAttackFinishedAtEnd = true)
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    enemyBruteBoss.speed = 0.0f;
    float timer = 0.0f;
    enemyBruteBoss.nextAttackTimer = 2f + jumpDuration;
    float angle = Utils.GetAngle(enemyBruteBoss.transform.position, targetPosition);
    enemyBruteBoss.LookAtAngle(angle);
    yield return (object) new WaitForEndOfFrame();
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.jumpAnimation, false);
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackJumpStartSFX))
      enemyBruteBoss.jumpStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackJumpStartSFX, enemyBruteBoss.transform);
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackJumpStartVO))
      enemyBruteBoss.jumpStartInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackJumpStartVO, enemyBruteBoss.transform);
    while ((double) timer < 0.6600000262260437)
    {
      timer += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.jumpLoopAnimation, true);
    float duration = jumpDuration;
    enemyBruteBoss.transform.DOMove(targetPosition, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    timer = 0.0f;
    while ((double) timer < (double) duration)
    {
      timer += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    if (animateLanding)
    {
      enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.jumpLandAnimation, false);
      enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
      if (!string.IsNullOrEmpty(enemyBruteBoss.AttackJumpLandSFX))
        AudioManager.Instance.PlayOneShot(enemyBruteBoss.AttackJumpLandSFX, enemyBruteBoss.transform.position);
      CameraManager.instance.ShakeCameraForDuration(0.75f, 1.25f, 0.3f);
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      enemyBruteBoss.ShakeGraves();
      timer = 0.0f;
      while ((double) timer < 0.82999998331069946)
      {
        timer += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
      if (setAttackFinishedAtEnd)
        enemyBruteBoss.attacking = false;
    }
    enemyBruteBoss.nextAttackTimer = 0.0f;
  }

  public Vector3 GetPositionAwayFromPlayer()
  {
    List<RaycastHit2D> raycastHit2DList = new List<RaycastHit2D>();
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(1f, 1f), 100f, (int) this.layerToCheck));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(-1f, 1f), 100f, (int) this.layerToCheck));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(-0.5f, -0.5f), 100f, (int) this.layerToCheck));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(0.5f, -0.5f), 100f, (int) this.layerToCheck));
    RaycastHit2D raycastHit2D1 = raycastHit2DList[0];
    for (int index = 1; index < raycastHit2DList.Count; ++index)
    {
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      {
        Vector3 position1 = PlayerFarming.Instance.transform.position;
        RaycastHit2D raycastHit2D2 = raycastHit2DList[index];
        Vector3 point1 = (Vector3) raycastHit2D2.point;
        if ((double) Vector3.Distance(position1, point1) > (double) Vector3.Distance(PlayerFarming.Instance.transform.position, (Vector3) raycastHit2D1.point))
        {
          Vector3 position2 = this.transform.position;
          raycastHit2D2 = raycastHit2DList[index];
          Vector3 point2 = (Vector3) raycastHit2D2.point;
          if ((double) Vector3.Distance(position2, point2) > 3.5)
            raycastHit2D1 = raycastHit2DList[index];
        }
      }
    }
    if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D1.point) <= 3.5)
    {
      raycastHit2DList.Remove(raycastHit2D1);
      raycastHit2D1 = raycastHit2DList[UnityEngine.Random.Range(0, raycastHit2DList.Count)];
    }
    return (Vector3) (raycastHit2D1.point + ((Vector2) this.transform.position - raycastHit2D1.point).normalized * 3f);
  }

  public void KnockedOut()
  {
    if (this.isDead)
      return;
    this.InterruptBehaviour();
    this.StartCoroutine((IEnumerator) this.KnockedOutIE());
  }

  public IEnumerator KnockedOutIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBruteBoss enemyBruteBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.knockedOutEndAnimation, false);
      enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, false, 0.0f);
      enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
      enemyBruteBoss.attacking = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.knockedOutStartAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.knockedOutLoopAnimation, true, 0.0f);
    enemyBruteBoss.ClearPaths();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(3f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void InitializeTraps() => ObjectPool.CreatePool(this.trapPrefab, 40);

  public void SingleDefensiveBoneWall()
  {
    this.StartCoroutine((IEnumerator) this.DoDefensiveBoneWall());
  }

  public void MultiDefensiveBoneWall()
  {
    this.StartCoroutine((IEnumerator) this.DoDefensiveBoneWall(this.multiRingAttackCount));
  }

  public IEnumerator DoDefensiveBoneWall(int ringCount = 1)
  {
    EnemyBruteBoss enemyBruteBoss = this;
    float attackDelay = 0.5f;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    enemyBruteBoss.LookAtAngle(Utils.GetAngle(enemyBruteBoss.transform.position, PlayerFarming.Instance.transform.position));
    string animationName = ringCount > 1 ? enemyBruteBoss.closeTrapQuickAnimation : enemyBruteBoss.closeTrapAnimation;
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, animationName, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
    string soundPath1 = ringCount == 1 ? enemyBruteBoss.AttackBoneWallSingleSFX : enemyBruteBoss.AttackBoneWallMultiStartSFX;
    string soundPath2 = ringCount == 1 ? enemyBruteBoss.AttackBoneWallSingleVO : enemyBruteBoss.AttackBoneWallMultiStartVO;
    if (!string.IsNullOrEmpty(soundPath1))
      enemyBruteBoss.boneWallStartInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath1, enemyBruteBoss.transform);
    if (!string.IsNullOrEmpty(soundPath2))
      enemyBruteBoss.boneWallStartInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(soundPath2, enemyBruteBoss.transform);
    int num1;
    for (int i = 0; i < ringCount; num1 = i++)
    {
      float t = 0.0f;
      while ((double) t < (double) attackDelay)
      {
        t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
      float num2 = (double) UnityEngine.Random.value < 0.5 ? 0.0f : 45f;
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
      if (ringCount > 0)
      {
        if (!string.IsNullOrEmpty(enemyBruteBoss.AttackBoneWallMultiBurstSFX))
          enemyBruteBoss.boneWallMultiBurstInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackBoneWallMultiBurstSFX, enemyBruteBoss.transform);
        if (!string.IsNullOrEmpty(enemyBruteBoss.AttackBoneWallMultiBurstVO))
          enemyBruteBoss.boneWallMultiBurstInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackBoneWallMultiBurstVO, enemyBruteBoss.transform);
      }
      int num3 = 10;
      int num4 = -1;
      while (++num4 < num3)
      {
        GameObject gameObject = ObjectPool.Spawn(enemyBruteBoss.trapPrefab);
        Skeleton skeleton = gameObject.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton;
        num1 = UnityEngine.Random.Range(1, 4);
        string skinName = num1.ToString();
        skeleton.SetSkin(skinName);
        float num5 = (float) (360 / num3 * num4);
        float num6 = enemyBruteBoss.distanceBetweenRings * (float) (i + 1);
        float num7 = UnityEngine.Random.Range(-0.5f, 0.5f);
        Vector3 vector3_1 = (Vector3) (Utils.DegreeToVector2(num5 + 90f + num2) * num7);
        Vector3 vector3_2 = enemyBruteBoss.transform.position + new Vector3(num6 * Mathf.Cos(num5 * ((float) Math.PI / 180f)), num6 * Mathf.Sin(num5 * ((float) Math.PI / 180f))) + vector3_1;
        if (BiomeGenerator.PointWithinIsland(vector3_2, out Vector3 _))
        {
          gameObject.transform.position = vector3_2;
          TrapLava.CreateLava(i == 0 ? enemyBruteBoss.trapLavaLarge : enemyBruteBoss.trapLavaSmall, vector3_2, enemyBruteBoss.transform.parent, enemyBruteBoss.health, enemyBruteBoss.LavalLifetime);
        }
        else
          gameObject.SetActive(false);
      }
      t = 0.0f;
      while ((double) t < (double) enemyBruteBoss.timeBetweenRings)
      {
        t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
    }
    yield return (object) new WaitForSeconds(enemyBruteBoss.ringAttackCooldown);
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.attacking = false;
  }

  public IEnumerator DoBoneWalls(int rowWidth = 8)
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    enemyBruteBoss.LookAtAngle(Utils.GetAngle(enemyBruteBoss.transform.position, PlayerFarming.Instance.transform.position));
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.distanceTrapAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
    float attackDelay = 1f;
    float delayBetweenWalls = 0.5f;
    float t = 0.0f;
    while ((double) t < (double) attackDelay)
    {
      t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    float gap = 4f;
    Vector3 direction = (PlayerFarming.Instance.transform.position - enemyBruteBoss.transform.position).normalized;
    Vector3 position = enemyBruteBoss.transform.position + direction;
    int i = -1;
label_12:
    int num1 = ++i;
    if (num1 < 5)
    {
      Vector3 vector3 = new Vector3(direction.y, -direction.x) * 2f;
      int num2 = -1;
      while (++num2 < rowWidth)
      {
        GameObject gameObject = ObjectPool.Spawn(enemyBruteBoss.trapPrefab);
        Skeleton skeleton = gameObject.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton;
        num1 = UnityEngine.Random.Range(1, 4);
        string skinName = num1.ToString();
        skeleton.SetSkin(skinName);
        Vector3 point = position + vector3 * (float) (num2 - rowWidth / 2);
        if (BiomeGenerator.PointWithinIsland(point, out Vector3 _))
          gameObject.transform.position = point;
        else
          gameObject.SetActive(false);
      }
      position += direction * gap;
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
      t = 0.0f;
      while ((double) t < (double) delayBetweenWalls)
      {
        t += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
      goto label_12;
    }
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.attacking = false;
  }

  public void SingleTargetedBoneCircle()
  {
    this.StartCoroutine((IEnumerator) this.DoSingleTargetedBoneCircle());
  }

  public IEnumerator DoSingleTargetedBoneCircle()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    float progress = 0.0f;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackBoneWallSingleSFX))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.AttackBoneWallSingleSFX, enemyBruteBoss.transform.position);
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackBoneWallSingleVO))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.AttackBoneWallSingleVO, enemyBruteBoss.transform.position);
    yield return (object) enemyBruteBoss.StartCoroutine((IEnumerator) enemyBruteBoss.TargetedBoneCircleAttack());
    while ((double) progress < (double) enemyBruteBoss.chasingAtkCooldown)
    {
      progress += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.attacking = false;
  }

  public void MultiTargetedBoneCircle()
  {
    this.StartCoroutine((IEnumerator) this.DoMultipleTargetedBoneCircle());
  }

  public IEnumerator DoMultipleTargetedBoneCircle()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.ClearPaths();
    for (int i = 0; i < enemyBruteBoss.chasingAtkCount; ++i)
    {
      float progress = 0.0f;
      if (!string.IsNullOrEmpty(enemyBruteBoss.AttackBoneWallChasingSFX))
        enemyBruteBoss.boneWallChasingInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackBoneWallChasingSFX, enemyBruteBoss.transform);
      if (!string.IsNullOrEmpty(enemyBruteBoss.AttackBoneWallChasingVO))
        enemyBruteBoss.boneWallChasingInstanceVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemyBruteBoss.AttackBoneWallChasingVO, enemyBruteBoss.transform);
      yield return (object) enemyBruteBoss.StartCoroutine((IEnumerator) enemyBruteBoss.TargetedBoneCircleAttack());
      while ((double) progress < (double) enemyBruteBoss.chasingAtkPauseBetweenAttacks)
      {
        progress += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
        yield return (object) null;
      }
    }
    float cooldown = 0.0f;
    while ((double) cooldown < (double) enemyBruteBoss.chasingAtkCooldown)
    {
      cooldown += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    enemyBruteBoss.nextAttackTimer = UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.attacking = false;
  }

  public IEnumerator TargetedBoneCircleAttack()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    float progress = 0.0f;
    float pauseBeforeTrigger = 0.5f;
    Vector3 targetPosition = enemyBruteBoss.ReconsiderPlayerTarget().transform.position;
    enemyBruteBoss.LookAtAngle(Utils.GetAngle(enemyBruteBoss.transform.position, targetPosition));
    enemyBruteBoss.Spine.AnimationState.SetAnimation(0, enemyBruteBoss.closeTrapAnimation, false);
    enemyBruteBoss.Spine.AnimationState.AddAnimation(0, enemyBruteBoss.idleAnimation, true, 0.0f);
    while ((double) progress < (double) pauseBeforeTrigger)
    {
      progress += Time.deltaTime * enemyBruteBoss.Spine.timeScale;
      yield return (object) null;
    }
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackBoneWallMultiBurstSFX))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.AttackBoneWallMultiBurstSFX, targetPosition);
    if (!string.IsNullOrEmpty(enemyBruteBoss.AttackBoneWallMultiBurstVO))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.AttackBoneWallMultiBurstVO, enemyBruteBoss.transform.position);
    float num1 = (double) UnityEngine.Random.value < 0.5 ? 0.0f : 45f;
    CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    GameObject gameObject1 = ObjectPool.Spawn(enemyBruteBoss.trapPrefab);
    gameObject1.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton.SetSkin(UnityEngine.Random.Range(1, 4).ToString());
    gameObject1.transform.position = targetPosition;
    int num2 = 10;
    int num3 = -1;
    while (++num3 < num2)
    {
      GameObject gameObject2 = ObjectPool.Spawn(enemyBruteBoss.trapPrefab);
      gameObject2.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton.SetSkin(UnityEngine.Random.Range(1, 4).ToString());
      float num4 = (float) (360 / num2 * num3);
      float num5 = 2f;
      float num6 = UnityEngine.Random.Range(-0.5f, 0.5f);
      Vector3 vector3 = (Vector3) (Utils.DegreeToVector2(num4 + 90f + num1) * num6);
      Vector3 position = PlayerFarming.Instance.transform.position + new Vector3(num5 * Mathf.Cos(num4 * ((float) Math.PI / 180f)), num5 * Mathf.Sin(num4 * ((float) Math.PI / 180f))) + vector3;
      gameObject2.transform.position = position;
      TrapLava.CreateLava(enemyBruteBoss.trapLavaLarge, position, enemyBruteBoss.transform.parent, enemyBruteBoss.health, enemyBruteBoss.LavalLifetime);
    }
  }

  public void SetConvoTextSpeedMultiplier(float speedMultiplier)
  {
    MMConversation.mmConversation.TextPlayer.textAnimator.GetComponent<TextAnimatorPlayer>().SetTypewriterSpeed(speedMultiplier);
    MMConversation.CURRENT_CONVERSATION.CallBack += (System.Action) (() =>
    {
      MMConversation.mmConversation.TextPlayer.textAnimator.GetComponent<TextAnimatorPlayer>().SetTypewriterSpeed(1f);
      MMConversation.CURRENT_CONVERSATION.CallBack = (System.Action) null;
    });
  }

  public void ChangeConvoTitle() => this.StartCoroutine((IEnumerator) this.ChangeConvoTitleIE());

  public IEnumerator ChangeConvoTitleIE()
  {
    yield return (object) new WaitForSeconds(1f);
    if (MMConversation.CURRENT_CONVERSATION != null && !(MMConversation.CURRENT_CONVERSATION.Entries == null | MMConversation.CURRENT_CONVERSATION.Entries.Count <= 0))
    {
      string title = LocalizationManager.GetTranslation(MMConversation.CURRENT_CONVERSATION.Entries[0].CharacterName);
      int i;
      for (i = title.Length - 1; i >= 0; --i)
      {
        title = title.Remove(title.Length - 1, 1);
        if (!((UnityEngine.Object) MMConversation.mmConversation == (UnityEngine.Object) null) && MMConversation.CURRENT_CONVERSATION != null)
        {
          MMConversation.mmConversation.SetTitle(title);
          yield return (object) new WaitForSeconds(0.1f);
        }
        else
          break;
      }
      yield return (object) new WaitForSeconds(0.5f);
      title = LocalizationManager.GetTranslation("NAMES/Gravekeeper");
      string text = "";
      for (i = 0; i < title.Length; ++i)
      {
        text += title[i].ToString();
        if (!((UnityEngine.Object) MMConversation.mmConversation == (UnityEngine.Object) null) && MMConversation.CURRENT_CONVERSATION != null)
        {
          MMConversation.mmConversation.SetTitle(text);
          yield return (object) new WaitForSeconds(0.1f);
        }
        else
          break;
      }
      if ((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null && MMConversation.CURRENT_CONVERSATION != null)
        MMConversation.mmConversation.SetTitle("NAMES/Gravekeeper");
    }
  }

  public IEnumerator PlayerPickUpLegendaryAxe()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    PickUp pickup = (PickUp) null;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(enemyBruteBoss.weaponSlot.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1, enemyBruteBoss.weaponSlot.position, result: (System.Action<PickUp>) (p =>
    {
      pickup = p;
      pickup.GetComponent<Interaction_BrokenWeapon>().SetWeapon(EquipmentType.Axe_Legendary);
    }));
    while ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
      yield return (object) null;
    pickup.enabled = false;
    pickup.child.transform.localScale = Vector3.one;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", enemyBruteBoss.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    bool isMoving = true;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = pickup.transform.DOMove(BookTargetPosition, 1.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() => isMoving = false);
    while (isMoving)
      yield return (object) null;
    pickup.transform.position = BookTargetPosition;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    yield return (object) new WaitForSeconds(1.5f);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1);
    DataManager.Instance.AddLegendaryWeaponToUnlockQueue(EquipmentType.Axe_Legendary);
    pickup.GetComponent<Interaction_BrokenWeapon>().StartBringWeaponToBlacksmithObjective();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) pickup.gameObject);
  }

  public IEnumerator PlayerPickUpTrophyDeco()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", enemyBruteBoss.transform.position);
    FoundItemPickUp deco = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, enemyBruteBoss.transform.position).GetComponent<FoundItemPickUp>();
    deco.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER;
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 itemTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", deco.gameObject);
    deco.GetComponent<PickUp>().enabled = false;
    bool wait = true;
    deco.transform.DOMove(itemTargetPosition, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => wait = false));
    while (wait)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot(enemyBruteBoss.OutroNewItemPickupSFX);
    deco.transform.position = itemTargetPosition;
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

  public void ShakeGraves()
  {
    if (this.graves == null || this.graves.Length == 0)
      return;
    foreach (BreakableGrave grave in this.graves)
    {
      if ((UnityEngine.Object) grave != (UnityEngine.Object) null && (UnityEngine.Object) grave.gameObject != (UnityEngine.Object) null && (double) this.health.HP > 0.0)
        grave.transform.DOShakeRotation(this.graveShakeDuration, this.graveShakeStrength, this.graveShakeVibrato, (float) this.graveShakeRandomness, this.graveShakeFadeOut);
    }
  }

  public void LeveaRoom() => this.StartCoroutine((IEnumerator) this.LeaveRoomRoutine());

  public IEnumerator LeaveRoomRoutine()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    Vector3 pos = LocationManager.LocationManagers[PlayerFarming.Location].GetExitPosition(FollowerLocation.Base);
    enemyBruteBoss.givePath(pos, forceAStar: true);
    enemyBruteBoss.state.facingAngle = enemyBruteBoss.state.LookAngle = Utils.GetAngle(pos, enemyBruteBoss.transform.position);
    while ((double) Vector3.Distance(pos, enemyBruteBoss.transform.position) > 3.0)
      yield return (object) null;
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < 0.5)
    {
      enemyBruteBoss.Spine.Skeleton.A = Mathf.Lerp(1f, 0.0f, t / 0.5f);
      yield return (object) null;
    }
    enemyBruteBoss.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__207_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemyBruteBoss.loadedAddressableHandles.Add(obj);
    this.loadedProjectile = obj.Result;
    this.loadedProjectile.CreatePool(this.HammerRingPojectileCount, true);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__207_1(AsyncOperationHandle<GameObject> obj)
  {
    EnemyBruteBoss.loadedAddressableHandles.Add(obj);
    this.loadedThrowableMinion = obj.Result;
    this.loadedThrowableMinion.CreatePool(this.MaxSpawnCount, true);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__207_2(AsyncOperationHandle<GameObject> obj)
  {
    EnemyBruteBoss.loadedAddressableHandles.Add(obj);
    this.loadedSpawnableHeavyUnit = obj.Result;
    this.loadedSpawnableHeavyUnit.CreatePool(this.MaxSpawnCount, true);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__207_3(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedIndicatorAsset = obj;
    obj.Result.CreatePool(16 /*0x10*/, true);
  }

  [CompilerGenerated]
  public void \u003COnDie\u003Eb__226_0()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DieIE());
  }

  [Serializable]
  public struct SpawnData
  {
    public int ThrowableSkelesCount;
    public int HeavySkelesCount;
  }

  public enum Phase
  {
    One,
    Two,
    Three,
  }

  public enum Attacks
  {
    Slash,
    SummonSkeles,
    ThrowAxeStraight,
    HammerJumpAttack,
    SingleTargetedBoneCircle,
    MultiTargetedBoneCircle,
    JumpAway,
    ThrowSkeles,
    SingleDefenciveBoneWall,
    MultiDefenciveBoneWall,
    ThrowAxeCircle,
    HammerAttack,
    MultiHammerAttack,
  }
}
