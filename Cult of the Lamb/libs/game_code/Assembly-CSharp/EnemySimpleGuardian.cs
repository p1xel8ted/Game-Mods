// Decompiled with JetBrains decompiler
// Type: EnemySimpleGuardian
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemySimpleGuardian : UnitObject
{
  public static float LastSimpleGuardianAttacked = float.MinValue;
  public static float LastSimpleGuardianRingProjectiles = float.MinValue;
  public static float LastSimpleGuardianPatternShot = float.MinValue;
  public GuardianPetController PrimaryPetContainer;
  public GuardianPetController SecondaryPetController;
  public GuardianPetController ThirdPetController;
  public Vector2 FirstLaunchDuration = new Vector2(3f, 5f);
  public bool CanDoRingShot = true;
  public bool CanDoBoomerangShot = true;
  public bool CanDoPatternShot;
  public List<ProjectilePattern> ProjectilePatterns = new List<ProjectilePattern>();
  public int ProjectilePatternIndex;
  [SerializeField]
  public bool requireLineOfSite = true;
  [SerializeField]
  public bool isMiniBoss;
  public GameObject TargetObject;
  public Health EnemyHealth;
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public SimpleSpineEventListener simpleSpineEventListener;
  public List<Collider2D> collider2DList;
  public Collider2D HealthCollider;
  public GameObject guardianGameObject;
  public ParticleSystem DashParticles;
  public ProjectilePattern projectilePattern;
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/warning";
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_large/death";
  [EventRef]
  public string AttackSwipeSFX = "event:/dlc/dungeon06/enemy/petguardian/attack_swipe_start";
  [EventRef]
  public string AttackProjectileSFX = "event:/dlc/dungeon06/enemy/petguardian/attack_projectile_start";
  [EventRef]
  public string OnHitSFX = "event:/enemy/impact_blunt";
  [EventRef]
  public string SpawnPetSFX = "event:/dlc/dungeon06/enemy/petguardian/attack_pet_spawn_start";
  [EventRef]
  public string BossIntroSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/intro_windup";
  [EventRef]
  public string RegenerateSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/attack_regenerate_start";
  [EventRef]
  public string BossHeadBreaksHalfSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/mv_break_head";
  [EventRef]
  public string BossHeadBreaksFullSFX = "event:/dlc/dungeon06/enemy/miniboss_petguardian/mv_break_head_b";
  public EventInstance projectileInstanceSFX;
  public EventInstance swipeInstanceSFX;
  public EventInstance spawnInstanceSFX;
  public EventInstance regenerateInstanceSFX;
  public bool hasPlayedHeadCracksHalfSFX;
  public bool hasPlayedHeadCrackFullSFX;
  public List<string> petSkins = new List<string>();
  [SerializeField]
  public string shootAnim = "summon";
  [SerializeField]
  public string summonPetAnim = "summon-pet";
  [SerializeField]
  public string idleAnim = "idle";
  public float launchPetTimer;
  public Coroutine spawningPetsRoutine;
  public DeadBodySliding deadBodySliding;
  public static List<EnemySimpleGuardian> SimpleGuardians = new List<EnemySimpleGuardian>();
  public bool chasingTarget;
  public bool AlwaysMoveToRandomPoint;
  public string WalkAnimation = "walk2";
  public float GlobalAttackDelay = 2f;
  public float GlobalRingAttackDelay = 10f;
  public float GlobalPatternAttackDelay = 5f;
  public Vector2 AttackRandomDelay = new Vector2(0.0f, 0.0f);
  public float LocalAttackDelay;
  public int NumberOfAttacks = 2;
  public string ForceAttackAnimation = "";
  public float AttackSignpost = 0.5f;
  public float DurationOfAttack = 0.5f;
  public bool UpdateAngleAfterAttack;
  public float AttackDeceleration = 1f;
  public float SpeedOfAttack = 15f;
  public bool UseForceForAttack;
  public float AttackForce = 1000f;
  public Vector3 TargetPosition;
  [SerializeField]
  public ProjectileCircle projectilePatternRings;
  [SerializeField]
  public float projectilePatternRingsSpeed = 2.5f;
  [SerializeField]
  public float projectilePatternRingsRadius = 1f;
  [SerializeField]
  public float projectilePatternRingsAcceleration = 7.5f;
  [SerializeField]
  public GameObject BoomerangArrow;
  [SerializeField]
  public float BoomerangCount = 20f;
  [SerializeField]
  public float BoomerangSpeed = 5f;
  [SerializeField]
  public float BoomerangReturnSpeed = -1f;
  [SerializeField]
  public float OutwardDuration = 3f;
  [SerializeField]
  public float ReturnDuration = 2f;
  [SerializeField]
  public float PauseDuration = 0.5f;
  public List<Projectile> Boomeranges = new List<Projectile>();

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    EnemySimpleGuardian.SimpleGuardians.Add(this);
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
    this.StartCoroutine(this.WaitForTarget());
    this.health.OnAddCharm += new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared += new Health.StasisEvent(this.ReconsiderTarget);
    this.TargetPosition = this.transform.position;
    if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null)
    {
      EnemySimpleGuardian.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
      EnemySimpleGuardian.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
      EnemySimpleGuardian.LastSimpleGuardianPatternShot = GameManager.GetInstance().CurrentTime;
    }
    if ((UnityEngine.Object) this.PrimaryPetContainer != (UnityEngine.Object) null)
    {
      EnemySimpleGuardian.LastSimpleGuardianAttacked = 0.0f;
      EnemySimpleGuardian.LastSimpleGuardianRingProjectiles = 0.0f;
      EnemySimpleGuardian.LastSimpleGuardianPatternShot = 0.0f;
      this.PrimaryPetContainer.Init(this.health, this.Spine);
    }
    if ((UnityEngine.Object) this.SecondaryPetController != (UnityEngine.Object) null)
      this.SecondaryPetController.Init(this.health, this.Spine);
    if (!((UnityEngine.Object) this.ThirdPetController != (UnityEngine.Object) null))
      return;
    this.ThirdPetController.Init(this.health, this.Spine);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemySimpleGuardian.SimpleGuardians.Remove(this);
    this.simpleSpineEventListener.OnSpineEvent -= new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.health.OnAddCharm -= new Health.StasisEvent(this.ReconsiderTarget);
    this.health.OnStasisCleared -= new Health.StasisEvent(this.ReconsiderTarget);
  }

  public override void Awake()
  {
    base.Awake();
    this.guardianGameObject = this.gameObject;
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    DataManager.Instance.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
  }

  public void Start()
  {
    if ((UnityEngine.Object) this.PrimaryPetContainer != (UnityEngine.Object) null)
    {
      this.PrimaryPetContainer.Init(this.health, this.Spine);
      this.SetPetSkins();
      this.launchPetTimer = UnityEngine.Random.Range(this.FirstLaunchDuration.x, this.FirstLaunchDuration.y);
    }
    this.Spine.AnimationState.SetAnimation(0, this.idleAnim, true);
  }

  public void SetPetSkins()
  {
    Skin newSkin = new Skin("skin");
    List<string> stringList = new List<string>();
    if ((UnityEngine.Object) this.SecondaryPetController != (UnityEngine.Object) null && (UnityEngine.Object) this.ThirdPetController != (UnityEngine.Object) null)
    {
      stringList.Add(this.PrimaryPetContainer.NameFromIndex(this.PrimaryPetContainer.PetIndex));
      stringList.Add(this.SecondaryPetController.NameFromIndex(this.SecondaryPetController.PetIndex));
      stringList.Add(this.ThirdPetController.NameFromIndex(this.ThirdPetController.PetIndex));
      for (int index = 0; index < stringList.Count; ++index)
      {
        stringList[index] = $"Pet{index + 1}_{stringList[index]}";
        newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin(stringList[index]));
      }
    }
    else
    {
      stringList.Add(this.PrimaryPetContainer.NameFromIndex(this.PrimaryPetContainer.PetIndex));
      int num = UnityEngine.Random.Range(0, 2);
      stringList[0] = $"Pet{num}_{stringList[0]}";
      newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin(stringList[0]));
    }
    string skinName = this.isMiniBoss ? "Miniboss_NoPets" : "Normal_NoPets";
    this.petSkins = stringList;
    newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin(skinName));
    if (this.isMiniBoss)
    {
      if ((double) this.health.HP < (double) this.health.totalHP * 0.33000001311302185)
        newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Miniboss_Head_3"));
      else if ((double) this.health.HP < (double) this.health.totalHP * 0.6600000262260437)
        newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Miniboss_Head_2"));
    }
    this.Spine.Skeleton.SetSkin(newSkin);
    this.Spine.Skeleton.SetSlotsToSetupPose();
  }

  public void RemovePetSkin(string skinToRemove)
  {
    Skin skin = new Skin("skin");
    for (int index = this.petSkins.Count - 1; index >= 0; --index)
    {
      if (!this.petSkins[index].Contains(skinToRemove))
      {
        skin.AddSkin(this.Spine.Skeleton.Data.FindSkin(this.petSkins[index]));
      }
      else
      {
        this.petSkins.RemoveAt(index);
        break;
      }
    }
    this.UpdateSkins();
  }

  public void UpdateSkins()
  {
    Skin newSkin = new Skin("skin");
    for (int index = this.petSkins.Count - 1; index >= 0; --index)
      newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin(this.petSkins[index]));
    string skinName = this.isMiniBoss ? "Miniboss_NoPets" : "Normal_NoPets";
    newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin(skinName));
    if ((double) this.health.HP < (double) this.health.totalHP * 0.33000001311302185)
    {
      newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Miniboss_Head_3"));
      if (!this.hasPlayedHeadCrackFullSFX && string.IsNullOrEmpty(this.BossHeadBreaksFullSFX))
      {
        this.hasPlayedHeadCrackFullSFX = true;
        AudioManager.Instance.PlayOneShot(this.BossHeadBreaksFullSFX, this.gameObject);
      }
    }
    else if ((double) this.health.HP < (double) this.health.totalHP * 0.6600000262260437)
    {
      newSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Miniboss_Head_2"));
      if (!this.hasPlayedHeadCracksHalfSFX && !string.IsNullOrEmpty(this.BossHeadBreaksHalfSFX))
      {
        this.hasPlayedHeadCracksHalfSFX = true;
        AudioManager.Instance.PlayOneShot(this.BossHeadBreaksHalfSFX, this.gameObject);
      }
    }
    this.Spine.Skeleton.SetSkin(newSkin);
    this.Spine.Skeleton.SetSlotsToSetupPose();
  }

  public IEnumerator WaitForTarget()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    enemySimpleGuardian.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemySimpleGuardian.TargetObject == (UnityEngine.Object) null)
    {
      enemySimpleGuardian.SetTargetObject();
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemySimpleGuardian.TargetObject == (UnityEngine.Object) null)
      {
        enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(enemySimpleGuardian.TargetObject.transform.position, enemySimpleGuardian.transform.position);
      if ((double) a <= (double) enemySimpleGuardian.VisionRange)
      {
        if (!enemySimpleGuardian.requireLineOfSite || enemySimpleGuardian.CheckLineOfSightOnTarget(enemySimpleGuardian.TargetObject, enemySimpleGuardian.TargetObject.transform.position, Mathf.Min(a, (float) enemySimpleGuardian.VisionRange)))
          InRange = true;
        else
          enemySimpleGuardian.LookAtTarget();
      }
      yield return (object) null;
    }
    enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
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

  public Vector3 LookAtTarget()
  {
    this.ReconsiderTarget();
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
      return Vector3.zero;
    float angle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    return this.TargetObject.transform.position;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if (!string.IsNullOrEmpty(this.OnHitSFX))
      AudioManager.Instance.PlayOneShot(this.OnHitSFX, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.SimpleSpineFlash.FlashFillRed();
    if (!this.isMiniBoss)
      return;
    this.UpdateSkins();
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
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    foreach (Projectile boomerange in this.Boomeranges)
      boomerange.DestroyProjectile();
    if ((bool) (UnityEngine.Object) this.PrimaryPetContainer)
      this.PrimaryPetContainer.OnHostDie();
    if ((bool) (UnityEngine.Object) this.SecondaryPetController)
      this.SecondaryPetController.OnHostDie();
    if ((bool) (UnityEngine.Object) this.ThirdPetController)
      this.ThirdPetController.OnHostDie();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StopAllCoroutines();
    if (this.spawningPetsRoutine != null)
      this.StopCoroutine(this.spawningPetsRoutine);
    this.CleanupInstanceSFX();
  }

  public void CleanupInstanceSFX()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.projectileInstanceSFX, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopOneShotInstanceEarly(this.spawnInstanceSFX, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopOneShotInstanceEarly(this.swipeInstanceSFX, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopOneShotInstanceEarly(this.regenerateInstanceSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
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
    }
  }

  public void GetPath()
  {
    this.chasingTarget = false;
    if (this.AlwaysMoveToRandomPoint)
    {
      this.TargetPosition = BiomeGenerator.GetRandomPositionInIsland();
      this.chasingTarget = false;
    }
    else if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
    {
      this.TargetPosition = BiomeGenerator.GetRandomPositionInIsland();
      this.chasingTarget = false;
    }
    else if (EnemySimpleGuardian.SimpleGuardians.Count > 1)
    {
      float num1 = Vector3.Distance(this.transform.position, this.TargetObject.transform.position);
      EnemySimpleGuardian enemySimpleGuardian = this;
      foreach (EnemySimpleGuardian simpleGuardian in EnemySimpleGuardian.SimpleGuardians)
      {
        if (!((UnityEngine.Object) enemySimpleGuardian == (UnityEngine.Object) simpleGuardian) && !((UnityEngine.Object) simpleGuardian.TargetObject != (UnityEngine.Object) this.TargetObject) && !((UnityEngine.Object) simpleGuardian.TargetObject == (UnityEngine.Object) null))
        {
          float num2 = Vector3.Distance(simpleGuardian.transform.position, this.TargetObject.transform.position);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            enemySimpleGuardian = simpleGuardian;
          }
        }
      }
      if ((UnityEngine.Object) enemySimpleGuardian == (UnityEngine.Object) this)
      {
        this.TargetPosition = this.TargetObject.transform.position;
        this.chasingTarget = true;
      }
      else
      {
        this.TargetPosition = BiomeGenerator.GetRandomPositionInIsland();
        this.chasingTarget = false;
      }
    }
    else
    {
      this.TargetPosition = this.TargetObject.transform.position;
      this.chasingTarget = true;
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
      if (!(this.Spine.AnimationName != this.idleAnim))
        return;
      this.Spine.AnimationState.SetAnimation(0, this.idleAnim, true);
    }
  }

  public virtual IEnumerator FightPlayer()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    while (EnemySimpleGuardian.SimpleGuardians.Count <= 1 && ((UnityEngine.Object) enemySimpleGuardian.TargetObject == (UnityEngine.Object) null || (double) Vector3.Distance(enemySimpleGuardian.TargetObject.transform.position, enemySimpleGuardian.transform.position) > 12.0))
    {
      if ((UnityEngine.Object) enemySimpleGuardian.TargetObject != (UnityEngine.Object) null)
        enemySimpleGuardian.LookAtTarget();
      yield return (object) null;
    }
    enemySimpleGuardian.GetPath();
    float RepathTimer = 0.0f;
    int NumAttacks = enemySimpleGuardian.NumberOfAttacks;
    enemySimpleGuardian.LocalAttackDelay = UnityEngine.Random.Range(enemySimpleGuardian.AttackRandomDelay.x, enemySimpleGuardian.AttackRandomDelay.y);
    float AttackSpeed = enemySimpleGuardian.SpeedOfAttack;
    bool Loop = true;
    while (Loop)
    {
      if ((double) enemySimpleGuardian.launchPetTimer != -1.0)
        enemySimpleGuardian.launchPetTimer -= Time.deltaTime;
      switch (enemySimpleGuardian.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
        case StateMachine.State.Moving:
          enemySimpleGuardian.LookAtTarget();
          if ((double) enemySimpleGuardian.launchPetTimer <= 0.0 && (double) enemySimpleGuardian.launchPetTimer != -1.0 && (UnityEngine.Object) enemySimpleGuardian.PrimaryPetContainer != (UnityEngine.Object) null)
          {
            enemySimpleGuardian.launchPetTimer = -1f;
            enemySimpleGuardian.SpawnPet();
            yield break;
          }
          if ((UnityEngine.Object) enemySimpleGuardian.TargetObject == (UnityEngine.Object) null)
          {
            if (enemySimpleGuardian.state.CURRENT_STATE == StateMachine.State.Idle)
            {
              if (enemySimpleGuardian.Spine.AnimationName != enemySimpleGuardian.idleAnim)
                enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.idleAnim, true);
              if ((double) (RepathTimer += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) > 1.0)
              {
                RepathTimer = 0.0f;
                enemySimpleGuardian.GetPath();
              }
            }
            if ((UnityEngine.Object) enemySimpleGuardian.damageColliderEvents != (UnityEngine.Object) null)
            {
              enemySimpleGuardian.damageColliderEvents.SetActive(false);
              break;
            }
            break;
          }
          if ((double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) < 3.0)
          {
            if ((double) GameManager.GetInstance().CurrentTime > ((double) EnemySimpleGuardian.LastSimpleGuardianAttacked + (double) enemySimpleGuardian.GlobalAttackDelay + (double) enemySimpleGuardian.LocalAttackDelay) / (double) enemySimpleGuardian.Spine.timeScale)
            {
              enemySimpleGuardian.LookAtTarget();
              DataManager.Instance.LastSimpleGuardianAttacked = TimeManager.TotalElapsedGameTime;
              EnemySimpleGuardian.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
              enemySimpleGuardian.LocalAttackDelay = UnityEngine.Random.Range(enemySimpleGuardian.AttackRandomDelay.x, enemySimpleGuardian.AttackRandomDelay.y);
              enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.ForceAttackAnimation == "" ? "attack" + (4 - NumAttacks).ToString() : enemySimpleGuardian.ForceAttackAnimation, false);
              enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
              if (!string.IsNullOrEmpty(enemySimpleGuardian.AttackSwipeSFX))
                enemySimpleGuardian.swipeInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySimpleGuardian.AttackSwipeSFX, enemySimpleGuardian.transform);
              if (!string.IsNullOrEmpty(enemySimpleGuardian.WarningVO))
                AudioManager.Instance.PlayOneShot(enemySimpleGuardian.WarningVO, enemySimpleGuardian.transform.position);
            }
            else if (enemySimpleGuardian.state.CURRENT_STATE != StateMachine.State.Idle)
            {
              enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
              enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.idleAnim, true);
            }
          }
          if (enemySimpleGuardian.CanDoRingShot && (double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) >= 5.0 && (double) GameManager.GetInstance().CurrentTime > ((double) EnemySimpleGuardian.LastSimpleGuardianRingProjectiles + (double) enemySimpleGuardian.GlobalRingAttackDelay) / (double) enemySimpleGuardian.Spine.timeScale)
          {
            DataManager.Instance.LastSimpleGuardianRingProjectiles = TimeManager.TotalElapsedGameTime;
            EnemySimpleGuardian.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
            enemySimpleGuardian.ProjectileRings();
            yield break;
          }
          if (enemySimpleGuardian.CanDoBoomerangShot && ((double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) >= 5.0 || (double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) < 2.0) && (double) GameManager.GetInstance().CurrentTime > ((double) EnemySimpleGuardian.LastSimpleGuardianRingProjectiles + (double) enemySimpleGuardian.GlobalRingAttackDelay) / (double) enemySimpleGuardian.Spine.timeScale)
          {
            DataManager.Instance.LastSimpleGuardianRingProjectiles = TimeManager.TotalElapsedGameTime;
            EnemySimpleGuardian.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
            enemySimpleGuardian.ProjectileBoomerangs();
            yield break;
          }
          if (enemySimpleGuardian.CanDoPatternShot && enemySimpleGuardian.ProjectilePatterns.Count > 0 && (double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) >= 1.0 && (double) GameManager.GetInstance().CurrentTime > ((double) EnemySimpleGuardian.LastSimpleGuardianPatternShot + (double) enemySimpleGuardian.GlobalPatternAttackDelay) / (double) enemySimpleGuardian.Spine.timeScale)
          {
            DataManager.Instance.LastSimpleGuardianPatternShot = TimeManager.TotalElapsedGameTime;
            EnemySimpleGuardian.LastSimpleGuardianPatternShot = GameManager.GetInstance().CurrentTime;
            enemySimpleGuardian.ProjectilePatternShot();
            yield break;
          }
          if (enemySimpleGuardian.chasingTarget)
          {
            if ((double) Vector2.Distance((Vector2) enemySimpleGuardian.transform.position, (Vector2) enemySimpleGuardian.TargetObject.transform.position) >= 3.0 && (double) (RepathTimer += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) > 0.20000000298023224)
            {
              RepathTimer = 0.0f;
              enemySimpleGuardian.GetPath();
            }
          }
          else if (enemySimpleGuardian.state.CURRENT_STATE == StateMachine.State.Idle)
          {
            if (enemySimpleGuardian.Spine.AnimationName != enemySimpleGuardian.idleAnim)
              enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.idleAnim, true);
            if ((double) (RepathTimer += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) > 1.0)
            {
              RepathTimer = 0.0f;
              enemySimpleGuardian.GetPath();
            }
          }
          if ((UnityEngine.Object) enemySimpleGuardian.damageColliderEvents != (UnityEngine.Object) null)
          {
            enemySimpleGuardian.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          enemySimpleGuardian.LookAtTarget();
          if (enemySimpleGuardian.UpdateAngleAfterAttack && (UnityEngine.Object) enemySimpleGuardian.TargetObject != (UnityEngine.Object) null)
          {
            if ((double) enemySimpleGuardian.state.Timer + (double) Time.deltaTime * (double) enemySimpleGuardian.Spine.timeScale < 0.25)
              enemySimpleGuardian.state.facingAngle = Utils.GetAngle(enemySimpleGuardian.transform.position, enemySimpleGuardian.TargetObject.transform.position);
          }
          else
            enemySimpleGuardian.state.facingAngle = Utils.GetAngle(enemySimpleGuardian.transform.position, enemySimpleGuardian.TargetPosition);
          if ((double) (enemySimpleGuardian.state.Timer += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) >= (double) enemySimpleGuardian.AttackSignpost)
          {
            enemySimpleGuardian.SimpleSpineFlash.FlashWhite(false);
            enemySimpleGuardian.DashParticles.Play();
            CameraManager.shakeCamera(0.4f, enemySimpleGuardian.state.facingAngle);
            enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemySimpleGuardian.speed = AttackSpeed * Time.deltaTime;
            if ((UnityEngine.Object) enemySimpleGuardian.damageColliderEvents != (UnityEngine.Object) null)
              enemySimpleGuardian.damageColliderEvents.SetActive(true);
            enemySimpleGuardian.LookAtTarget();
            if (enemySimpleGuardian.UseForceForAttack)
            {
              enemySimpleGuardian.DisableForces = true;
              Vector3 force = (Vector3) new Vector2(enemySimpleGuardian.AttackForce * Mathf.Cos(enemySimpleGuardian.state.facingAngle * ((float) Math.PI / 180f)), enemySimpleGuardian.AttackForce * Mathf.Sin(enemySimpleGuardian.state.facingAngle * ((float) Math.PI / 180f)));
              enemySimpleGuardian.rb.AddForce((Vector2) force);
            }
            if (!string.IsNullOrEmpty(enemySimpleGuardian.AttackVO))
            {
              AudioManager.Instance.PlayOneShot(enemySimpleGuardian.AttackVO);
              break;
            }
            break;
          }
          enemySimpleGuardian.SimpleSpineFlash.FlashWhite(enemySimpleGuardian.state.Timer / enemySimpleGuardian.AttackSignpost);
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) AttackSpeed > 0.0)
            AttackSpeed -= enemySimpleGuardian.AttackDeceleration * GameManager.DeltaTime * enemySimpleGuardian.Spine.timeScale;
          enemySimpleGuardian.speed = AttackSpeed * Time.deltaTime * enemySimpleGuardian.Spine.timeScale;
          if ((double) enemySimpleGuardian.state.Timer >= 0.25 && (UnityEngine.Object) enemySimpleGuardian.damageColliderEvents != (UnityEngine.Object) null)
            enemySimpleGuardian.damageColliderEvents.SetActive(false);
          if ((double) (enemySimpleGuardian.state.Timer += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) >= (double) enemySimpleGuardian.DurationOfAttack)
          {
            if (enemySimpleGuardian.UseForceForAttack)
            {
              enemySimpleGuardian.DisableForces = false;
              enemySimpleGuardian.rb.velocity = Vector2.zero;
            }
            enemySimpleGuardian.DashParticles.Stop();
            if (--NumAttacks > 0)
            {
              AttackSpeed = enemySimpleGuardian.SpeedOfAttack + (float) ((3 - NumAttacks) * 2);
              enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.ForceAttackAnimation == "" ? "attack" + (4 - NumAttacks).ToString() : enemySimpleGuardian.ForceAttackAnimation, false);
              enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
              break;
            }
            Loop = false;
            enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
            enemySimpleGuardian.ReconsiderTarget();
            enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.idleAnim, true);
            break;
          }
          break;
      }
      yield return (object) null;
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.idleAnim, true);
    if ((UnityEngine.Object) enemySimpleGuardian.TargetObject != (UnityEngine.Object) null && (double) Vector3.Distance(enemySimpleGuardian.TargetObject.transform.position, enemySimpleGuardian.transform.position) > 5.0)
    {
      enemySimpleGuardian.LookAtTarget();
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.0)
        yield return (object) null;
    }
    enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.spawningPetsRoutine == null)
      return;
    this.StopCoroutine(this.spawningPetsRoutine);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && !component.IsCharmedEnemy)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void SpawnPet()
  {
    this.StopAllCoroutines();
    this.spawningPetsRoutine = GameManager.GetInstance().StartCoroutine(this.SpawnPetRoutine());
  }

  public IEnumerator SpawnPetRoutine()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    if (!string.IsNullOrEmpty(enemySimpleGuardian.WarningVO))
      AudioManager.Instance.PlayOneShot(enemySimpleGuardian.WarningVO, enemySimpleGuardian.transform.position);
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.summonPetAnim, false);
    enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
    if (!string.IsNullOrEmpty(enemySimpleGuardian.SpawnPetSFX))
      enemySimpleGuardian.spawnInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySimpleGuardian.SpawnPetSFX, enemySimpleGuardian.transform);
    yield return (object) new WaitForSeconds(1f);
    enemySimpleGuardian.PrimaryPetContainer.Launch();
    GameManager.GetInstance().WaitForSeconds(0.25f, new System.Action(enemySimpleGuardian.\u003CSpawnPetRoutine\u003Eb__93_0));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 0.800000011920929)
      yield return (object) null;
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.idleAnim, true);
    enemySimpleGuardian.ReconsiderTarget();
    enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
    if (enemySimpleGuardian.isMiniBoss)
    {
      time = 0.0f;
      while (true)
      {
        if (enemySimpleGuardian.gameObject.activeSelf && !PlayerRelic.TimeFrozen)
          time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale;
        if ((double) time < 25.0)
          yield return (object) null;
        else
          break;
      }
      enemySimpleGuardian.StopAllCoroutines();
      enemySimpleGuardian.ClearPaths();
      enemySimpleGuardian.SetAttacksMax();
      enemySimpleGuardian.SimpleSpineFlash.FlashWhite(false);
      enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.summonPetAnim, false);
      enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.0)
        yield return (object) null;
      enemySimpleGuardian.SecondaryPetController.Launch();
      enemySimpleGuardian.ReconsiderTarget();
      enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
      time = 0.0f;
      while (true)
      {
        if (enemySimpleGuardian.gameObject.activeSelf && !PlayerRelic.TimeFrozen)
          time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale;
        if ((double) time < 10.0)
          yield return (object) null;
        else
          break;
      }
      int flipFlop = 2;
label_22:
      enemySimpleGuardian.StopAllCoroutines();
      enemySimpleGuardian.ClearPaths();
      enemySimpleGuardian.SetAttacksMax();
      enemySimpleGuardian.SimpleSpineFlash.FlashWhite(false);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 2.0)
        yield return (object) null;
      if (!string.IsNullOrEmpty(enemySimpleGuardian.WarningVO))
        AudioManager.Instance.PlayOneShot(enemySimpleGuardian.WarningVO, enemySimpleGuardian.transform.position);
      enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
      yield return (object) null;
      enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "spawn-more-pets", false);
      enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
      if (!string.IsNullOrEmpty(enemySimpleGuardian.RegenerateSFX))
        enemySimpleGuardian.regenerateInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySimpleGuardian.RegenerateSFX, enemySimpleGuardian.transform);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.0)
        yield return (object) null;
      enemySimpleGuardian.PrimaryPetContainer.PetIndex = 1;
      enemySimpleGuardian.SecondaryPetController.PetIndex = flipFlop;
      enemySimpleGuardian.ThirdPetController.PetIndex = 1;
      enemySimpleGuardian.SetPetSkins();
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.0)
        yield return (object) null;
      enemySimpleGuardian.ReconsiderTarget();
      enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
      flipFlop = flipFlop == 2 ? 0 : 2;
      enemySimpleGuardian.petSkins.Clear();
      time = 0.0f;
      while (true)
      {
        if (enemySimpleGuardian.gameObject.activeSelf && !PlayerRelic.TimeFrozen)
          time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale;
        if ((double) time < 20.0)
          yield return (object) null;
        else
          break;
      }
      enemySimpleGuardian.StopAllCoroutines();
      enemySimpleGuardian.ClearPaths();
      enemySimpleGuardian.SetAttacksMax();
      enemySimpleGuardian.SimpleSpineFlash.FlashWhite(false);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 2.0)
        yield return (object) null;
      enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
      yield return (object) null;
      enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.summonPetAnim, false);
      enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
      if (!string.IsNullOrEmpty(enemySimpleGuardian.SpawnPetSFX))
        enemySimpleGuardian.spawnInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySimpleGuardian.SpawnPetSFX, enemySimpleGuardian.transform);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.0)
        yield return (object) null;
      enemySimpleGuardian.PrimaryPetContainer.Launch();
      GameManager.GetInstance().WaitForSeconds(0.25f, new System.Action(enemySimpleGuardian.\u003CSpawnPetRoutine\u003Eb__93_1));
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.0)
        yield return (object) null;
      enemySimpleGuardian.ReconsiderTarget();
      enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
      time = 0.0f;
      while (true)
      {
        if (enemySimpleGuardian.gameObject.activeSelf && !PlayerRelic.TimeFrozen)
          time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale;
        if ((double) time < 25.0)
          yield return (object) null;
        else
          break;
      }
      enemySimpleGuardian.StopAllCoroutines();
      enemySimpleGuardian.ClearPaths();
      enemySimpleGuardian.SetAttacksMax();
      enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.summonPetAnim, false);
      enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
      if (!string.IsNullOrEmpty(enemySimpleGuardian.SpawnPetSFX))
        enemySimpleGuardian.spawnInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySimpleGuardian.SpawnPetSFX, enemySimpleGuardian.transform);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.0)
        yield return (object) null;
      enemySimpleGuardian.SecondaryPetController.Launch();
      enemySimpleGuardian.ReconsiderTarget();
      enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
      time = 0.0f;
      while (true)
      {
        if (enemySimpleGuardian.gameObject.activeSelf && !PlayerRelic.TimeFrozen)
          time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale;
        if ((double) time < 10.0)
          yield return (object) null;
        else
          goto label_22;
      }
    }
  }

  public void SetAttacksMax()
  {
    this.LocalAttackDelay = UnityEngine.Random.Range(this.AttackRandomDelay.x, this.AttackRandomDelay.y);
    EnemySimpleGuardian.LastSimpleGuardianAttacked = GameManager.GetInstance().CurrentTime;
    EnemySimpleGuardian.LastSimpleGuardianRingProjectiles = GameManager.GetInstance().CurrentTime;
    EnemySimpleGuardian.LastSimpleGuardianPatternShot = GameManager.GetInstance().CurrentTime;
  }

  public void ProjectileRings()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProjectileRingsRoutine());
  }

  public virtual IEnumerator ProjectileRingsRoutine()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    if (!string.IsNullOrEmpty(enemySimpleGuardian.WarningVO))
      AudioManager.Instance.PlayOneShot(enemySimpleGuardian.WarningVO, enemySimpleGuardian.transform.position);
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    enemySimpleGuardian.LookAtTarget();
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.shootAnim, false);
    enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.0)
      yield return (object) null;
    if (!string.IsNullOrEmpty(enemySimpleGuardian.AttackVO))
      AudioManager.Instance.PlayOneShot(enemySimpleGuardian.AttackVO, enemySimpleGuardian.transform.position);
    Projectile arrow = UnityEngine.Object.Instantiate<ProjectileCircle>(enemySimpleGuardian.projectilePatternRings, enemySimpleGuardian.transform.parent).GetComponent<Projectile>();
    arrow.transform.position = enemySimpleGuardian.transform.position;
    arrow.health = enemySimpleGuardian.health;
    arrow.team = enemySimpleGuardian.health.team;
    arrow.Speed = enemySimpleGuardian.projectilePatternRingsSpeed;
    arrow.Acceleration = enemySimpleGuardian.projectilePatternRingsAcceleration;
    arrow.Owner = enemySimpleGuardian.health;
    arrow.SetOwner(enemySimpleGuardian.health.gameObject);
    arrow.GetComponent<ProjectileCircle>().InitDelayed((UnityEngine.Object) enemySimpleGuardian.TargetObject != (UnityEngine.Object) null ? enemySimpleGuardian.TargetObject : PlayerFarming.Instance.gameObject, enemySimpleGuardian.projectilePatternRingsRadius, 0.0f, (System.Action) (() =>
    {
      CameraManager.instance.ShakeCameraForDuration(0.8f, 0.9f, 0.3f, false);
      if ((UnityEngine.Object) this.guardianGameObject != (UnityEngine.Object) null)
      {
        if (!string.IsNullOrEmpty(this.AttackProjectileSFX))
          this.projectileInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackProjectileSFX, this.guardianGameObject.transform);
        arrow.Angle = Mathf.Round(arrow.Angle / 45f) * 45f;
      }
      else
        arrow.DestroyProjectile();
    }));
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.5666667222976685)
      yield return (object) null;
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian.idleAnim, true);
    enemySimpleGuardian.ReconsiderTarget();
    enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
  }

  public void ProjectilePatternShot()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProjectilePatternShotRoutine());
  }

  public IEnumerator ProjectilePatternShotRoutine()
  {
    EnemySimpleGuardian enemySimpleGuardian1 = this;
    if (!string.IsNullOrEmpty(enemySimpleGuardian1.WarningVO))
      AudioManager.Instance.PlayOneShot(enemySimpleGuardian1.WarningVO, enemySimpleGuardian1.transform.position);
    enemySimpleGuardian1.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    Vector3 lastTargetPosition = enemySimpleGuardian1.LookAtTarget();
    enemySimpleGuardian1.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian1.shootAnim, false);
    enemySimpleGuardian1.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian1.idleAnim, true, 0.0f);
    if (!string.IsNullOrEmpty(enemySimpleGuardian1.AttackProjectileSFX))
      enemySimpleGuardian1.projectileInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySimpleGuardian1.AttackProjectileSFX, enemySimpleGuardian1.transform);
    float dur = 1f;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian1.Spine.timeScale) < (double) dur)
    {
      enemySimpleGuardian1.SimpleSpineFlash.FlashWhite(time / dur);
      yield return (object) null;
    }
    enemySimpleGuardian1.SimpleSpineFlash.FlashWhite(false);
    if (!string.IsNullOrEmpty(enemySimpleGuardian1.AttackVO))
      AudioManager.Instance.PlayOneShot(enemySimpleGuardian1.AttackVO, enemySimpleGuardian1.transform.position);
    enemySimpleGuardian1.projectilePattern = enemySimpleGuardian1.ProjectilePatterns[enemySimpleGuardian1.ProjectilePatternIndex];
    EnemySimpleGuardian enemySimpleGuardian2 = enemySimpleGuardian1;
    EnemySimpleGuardian enemySimpleGuardian3 = enemySimpleGuardian1;
    int num1 = enemySimpleGuardian1.ProjectilePatternIndex + 1;
    int num2 = num1;
    enemySimpleGuardian3.ProjectilePatternIndex = num2;
    int num3 = num1 % enemySimpleGuardian1.ProjectilePatterns.Count;
    enemySimpleGuardian2.ProjectilePatternIndex = num3;
    enemySimpleGuardian1.projectilePattern.Shoot(0.0f, lastTargetPosition, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian1.Spine.timeScale) < 1.5666667222976685)
      yield return (object) null;
    enemySimpleGuardian1.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian1.Spine.AnimationState.SetAnimation(0, enemySimpleGuardian1.idleAnim, true);
    enemySimpleGuardian1.StartCoroutine(enemySimpleGuardian1.FightPlayer());
  }

  public void ProjectileBoomerangs()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ProjectileBoomerangsRoutine());
  }

  public IEnumerator ProjectileBoomerangsRoutine()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    if (!string.IsNullOrEmpty(enemySimpleGuardian.WarningVO))
      AudioManager.Instance.PlayOneShot(enemySimpleGuardian.WarningVO, enemySimpleGuardian.transform.position);
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) null;
    enemySimpleGuardian.LookAtTarget();
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "projectiles-start", false);
    enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, "projectiles-loop", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 1.7666666507720947)
      yield return (object) null;
    if (!string.IsNullOrEmpty(enemySimpleGuardian.AttackVO))
      AudioManager.Instance.PlayOneShot(enemySimpleGuardian.AttackVO, enemySimpleGuardian.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.8f, 0.9f, 0.3f, false);
    enemySimpleGuardian.Boomeranges = new List<Projectile>();
    float boomerangCount = enemySimpleGuardian.BoomerangCount;
    for (float num = 0.0f; (double) num < (double) boomerangCount; ++num)
    {
      Projectile component = UnityEngine.Object.Instantiate<GameObject>(enemySimpleGuardian.BoomerangArrow, enemySimpleGuardian.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemySimpleGuardian.transform.position;
      component.team = enemySimpleGuardian.health.team;
      component.Speed = enemySimpleGuardian.BoomerangSpeed;
      component.Angle = 360f / boomerangCount * num;
      component.LifeTime = 30f;
      component.IgnoreIsland = true;
      component.Trail.time = 0.3f;
      component.Owner = enemySimpleGuardian.health;
      component.SetOwner(enemySimpleGuardian.health.gameObject);
      enemySimpleGuardian.Boomeranges.Add(component);
    }
    if (!string.IsNullOrEmpty(enemySimpleGuardian.AttackProjectileSFX))
      enemySimpleGuardian.projectileInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySimpleGuardian.AttackProjectileSFX, enemySimpleGuardian.transform);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < (double) enemySimpleGuardian.OutwardDuration)
      yield return (object) null;
    foreach (Projectile boomerange in enemySimpleGuardian.Boomeranges)
    {
      Projectile a = boomerange;
      if ((UnityEngine.Object) a != (UnityEngine.Object) null)
        DOTween.To((DOGetter<float>) (() => a.SpeedMultiplier), (DOSetter<float>) (x => a.SpeedMultiplier = x), 0.0f, 0.2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < (double) enemySimpleGuardian.PauseDuration)
      yield return (object) null;
    foreach (Projectile boomerange in enemySimpleGuardian.Boomeranges)
    {
      Projectile a = boomerange;
      if ((UnityEngine.Object) a != (UnityEngine.Object) null)
        DOTween.To((DOGetter<float>) (() => a.SpeedMultiplier), (DOSetter<float>) (x => a.SpeedMultiplier = x), enemySimpleGuardian.BoomerangReturnSpeed, 0.3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBack);
    }
    if (!string.IsNullOrEmpty(enemySimpleGuardian.AttackProjectileSFX))
      enemySimpleGuardian.projectileInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySimpleGuardian.AttackProjectileSFX, enemySimpleGuardian.transform);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < (double) enemySimpleGuardian.ReturnDuration)
      yield return (object) null;
    bool flag = true;
    foreach (Projectile boomerange in enemySimpleGuardian.Boomeranges)
    {
      if ((UnityEngine.Object) boomerange != (UnityEngine.Object) null)
      {
        boomerange.EndOfLife();
        if (flag)
        {
          boomerange.EndOfLife();
          flag = false;
        }
        else
          boomerange.DestroyProjectile();
      }
    }
    enemySimpleGuardian.Boomeranges.Clear();
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "projectiles-stop", true);
    enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySimpleGuardian.Spine.timeScale) < 0.699999988079071)
      yield return (object) null;
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian.ReconsiderTarget();
    enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.FightPlayer());
  }

  public void GraveSpawn()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.GraveSpawnRoutine());
  }

  public IEnumerator GraveSpawnRoutine()
  {
    EnemySimpleGuardian enemySimpleGuardian = this;
    enemySimpleGuardian.ClearPaths();
    enemySimpleGuardian.health.invincible = true;
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    enemySimpleGuardian.Spine.AnimationState.SetAnimation(0, "grave-spawn-long", false);
    enemySimpleGuardian.Spine.AnimationState.AddAnimation(0, enemySimpleGuardian.idleAnim, true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    enemySimpleGuardian.health.invincible = false;
    enemySimpleGuardian.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySimpleGuardian.StartCoroutine(enemySimpleGuardian.WaitForTarget());
  }

  [CompilerGenerated]
  public void \u003CSpawnPetRoutine\u003Eb__93_0()
  {
    if (!((UnityEngine.Object) this.ThirdPetController != (UnityEngine.Object) null))
      return;
    this.ThirdPetController.Launch();
  }

  [CompilerGenerated]
  public void \u003CSpawnPetRoutine\u003Eb__93_1()
  {
    if (!((UnityEngine.Object) this.ThirdPetController != (UnityEngine.Object) null))
      return;
    this.ThirdPetController.Launch();
  }
}
