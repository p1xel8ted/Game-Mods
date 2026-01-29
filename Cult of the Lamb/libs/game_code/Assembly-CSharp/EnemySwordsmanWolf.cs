// Decompiled with JetBrains decompiler
// Type: EnemySwordsmanWolf
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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

#nullable disable
public class EnemySwordsmanWolf : UnitObject, IAttackResilient
{
  public int NewPositionDistance = 3;
  public float MaintainTargetDistance = 4.5f;
  public float MoveCloserDistance = 4f;
  public float AttackWithinRange = 4f;
  public bool DoubleAttack = true;
  public bool ChargeAndAttack = true;
  public Vector2 MaxAttackDelayRandomRange = new Vector2(4f, 6f);
  public Vector2 AttackDelayRandomRange = new Vector2(0.5f, 2f);
  public ColliderEvents damageColliderEvents;
  [SerializeField]
  public bool requireLineOfSite = true;
  [SerializeField]
  public GameObject trapAvalanche;
  [SerializeField]
  public Transform[] avalanchePoints;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public float TeleportDelayTarget = 1f;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/calm_attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_small/death";
  [EventRef]
  public string EnragedDeathVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_death_vo";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon05/enemy/vocals_shared/dog_humanoid_small/gethit";
  [EventRef]
  public string EnragedGetHitVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_gethit_vo";
  [EventRef]
  public string DrawbackSwordSFX = "event:/dlc/dungeon05/enemy/wolf_swordsman/calm_warning";
  [EventRef]
  public string attackSoundPath = "event:/dlc/dungeon05/enemy/wolf_swordsman/calm_attack";
  [EventRef]
  public string onHitSoundPath = "event:/enemy/impact_normal";
  [EventRef]
  public string TransformIntoEnragedVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_transform_vo";
  [EventRef]
  public string TransformIntoEnragedSFX = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_transform";
  [EventRef]
  public string EnragedChargeAttackVO = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_charge_vo";
  [EventRef]
  public string EnragedChargeAttackSFX = "event:/dlc/dungeon05/enemy/wolf_swordsman/enraged_charge";
  [EventRef]
  public string InvincibleOnHitSFX = "event:/dlc/combat/invulnerable_impact";
  public GameObject TargetObject;
  public float AttackDelay;
  public bool canBeParried;
  public static float signPostParryWindow = 0.2f;
  public static float attackParryWindow = 0.15f;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public bool \u003CFollowPlayer\u003Ek__BackingField;
  [SerializeField]
  public bool hasRage;
  [SerializeField]
  public float rageSpeedMultiplier = 1f;
  public bool rageActive;
  [SerializeField]
  public float rageAttackDuration = 0.4f;
  [SerializeField]
  public float rageAttackDashAmount = 30f;
  [SerializeField]
  public float rageAttackDistance = 2.5f;
  [SerializeField]
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string rageSkin;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string rageEnterAnimationStart;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string rageEnterAnimationEnd;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string rageRun;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string rageBiteCharge;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string rageBiteImpact;
  [SerializeField]
  public GameObject rageParticle;
  [SerializeField]
  public GameObject helmet;
  [SerializeField]
  public SkeletonGhost ghost;
  public bool enraging;
  public bool canEnrage = true;
  public bool mustRage;
  public static List<EnemySwordsmanWolf> swordsmans = new List<EnemySwordsmanWolf>();
  public EventInstance enragedChargeSFXInstance;
  public EventInstance enragedChargeVOInstance;
  public Vector3 Force;
  public float KnockbackModifier = 1f;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public float TeleportDelay;
  public EnemySwordsmanWolf.State MyState;
  public float MaxAttackDelay;
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
    EnemySwordsmanWolf.swordsmans.Add(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb.simulated = true;
  }

  public override void OnDisable()
  {
    this.CleanupEventInstances();
    this.health.invincible = false;
    this.health.IsDeflecting = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    EnemySwordsmanWolf.swordsmans.Remove(this);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
    this.DisableForces = false;
    if (!this.enraging)
      return;
    this.enraging = false;
    this.health.invincible = false;
    this.health.IsDeflecting = false;
    this.health.HP = this.health.totalHP;
    this.GetComponent<ShowHPBar>().ForceUpdate();
  }

  public override void OnDieEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.enraging && !this.rageActive && (double) this.health.HP <= 0.0 && this.canEnrage && !AttackFlags.HasFlag((Enum) Health.AttackFlags.ForceKill))
    {
      this.ClearPaths();
      this.SimpleSpineFlash.FlashFillRed();
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
      this.enraging = true;
      this.rageActive = true;
      this.health.invincible = true;
      this.health.IsDeflecting = true;
      if ((double) this.health.HP <= 0.0)
        this.health.HP = 0.1f;
      this.StartCoroutine((IEnumerator) this.Enrage());
      Vector3 vector3 = ((this.transform.position - Attacker.transform.position).normalized * 3f) with
      {
        z = -2f
      };
      int num = (double) Attacker.transform.position.x > (double) this.transform.position.x ? -1 : 1;
      this.helmet.gameObject.SetActive(true);
      this.helmet.transform.parent = (Transform) null;
      this.helmet.transform.localScale = new Vector3((float) num * this.Spine.transform.localScale.x, 1f, 1f);
      this.helmet.transform.GetChild(0).transform.DOLocalRotate(Vector3.back * 3240f, 3f, RotateMode.FastBeyond360).SetEase<TweenerCore<Quaternion, Vector3, QuaternionOptions>>(Ease.Linear);
      this.helmet.transform.GetComponentInChildren<SpriteRenderer>().DOFade(0.0f, 0.5f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.5f);
      this.helmet.transform.DOMove(this.helmet.transform.position + vector3, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.helmet.gameObject)));
    }
    else
      base.OnDieEarly(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHitEarly(Attacker, AttackLocation, AttackType, FromBehind);
    if ((UnityEngine.Object) Attacker == (UnityEngine.Object) this.gameObject)
      return;
    if (PlayerController.CanParryAttacks && this.canBeParried && !FromBehind && AttackType == Health.AttackTypes.Melee && !this.enraging)
    {
      this.health.WasJustParried = true;
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.StopAllCoroutines();
      this.DisableForces = false;
    }
    if (!this.enraging)
      return;
    BiomeConstants.Instance.EmitHitVFX(this.transform.position + Vector3.back, Quaternion.identity.z, "HitFX_Weak");
    this.SimpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    string soundPath = this.rageActive ? this.EnragedDeathVO : this.DeathVO;
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath, this.transform.position);
    this.CleanupEventInstances();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public void CleanupEventInstances()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.enragedChargeSFXInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.enragedChargeVOInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
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
    string soundPath = this.rageActive ? this.EnragedGetHitVO : this.GetHitVO;
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath, this.transform.position);
    this.Spine.AnimationState.SetAnimation(1, "hurt-eyes", false);
    if (this.MyState != EnemySwordsmanWolf.State.Attacking && !this.enraging)
    {
      this.SimpleSpineFlash.FlashWhite(false);
      this.SeperateObject = true;
      this.UsePathing = true;
      this.health.invincible = false;
      this.StopAllCoroutines();
      this.DisableForces = false;
      if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
        this.state.CURRENT_STATE = StateMachine.State.HitRight;
      if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
        this.state.CURRENT_STATE = StateMachine.State.HitLeft;
      if (AttackType != Health.AttackTypes.Heavy && (!(AttackType == Health.AttackTypes.Projectile & FromBehind) || this.health.HasShield))
        this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    if (AttackType != Health.AttackTypes.NoKnockBack)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    enemySwordsmanWolf.DisableForces = true;
    enemySwordsmanWolf.Force = (enemySwordsmanWolf.transform.position - Attacker.transform.position).normalized * 500f;
    enemySwordsmanWolf.rb.AddForce((Vector2) (enemySwordsmanWolf.Force * enemySwordsmanWolf.KnockbackModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemySwordsmanWolf.DisableForces = false;
  }

  public IEnumerator HurtRoutine()
  {
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.WaitForTarget());
  }

  public IEnumerator WaitForTarget()
  {
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    enemySwordsmanWolf.Spine.Initialize(false);
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    while ((UnityEngine.Object) enemySwordsmanWolf.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemySwordsmanWolf.GetClosestTarget(enemySwordsmanWolf.health.team == Health.Team.PlayerTeam);
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemySwordsmanWolf.TargetObject = closestTarget.gameObject;
        enemySwordsmanWolf.requireLineOfSite = false;
        enemySwordsmanWolf.VisionRange = int.MaxValue;
      }
      enemySwordsmanWolf.RepathTimer -= Time.deltaTime * enemySwordsmanWolf.Spine.timeScale;
      if ((double) enemySwordsmanWolf.RepathTimer <= 0.0)
      {
        if (enemySwordsmanWolf.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemySwordsmanWolf.rageActive)
          {
            if (enemySwordsmanWolf.Spine.AnimationName != enemySwordsmanWolf.rageRun)
              enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, enemySwordsmanWolf.rageRun, true);
          }
          else if (enemySwordsmanWolf.Spine.AnimationName != "run")
            enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemySwordsmanWolf.Spine.AnimationName != "idle")
          enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "idle", true);
        if (!enemySwordsmanWolf.FollowPlayer)
          enemySwordsmanWolf.TargetPosition = enemySwordsmanWolf.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        enemySwordsmanWolf.FindPath(enemySwordsmanWolf.TargetPosition);
        enemySwordsmanWolf.state.LookAngle = Utils.GetAngle(enemySwordsmanWolf.transform.position, enemySwordsmanWolf.TargetPosition);
        enemySwordsmanWolf.Spine.skeleton.ScaleX = (double) enemySwordsmanWolf.state.LookAngle <= 90.0 || (double) enemySwordsmanWolf.state.LookAngle >= 270.0 ? -1f : 1f;
      }
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemySwordsmanWolf.TargetObject == (UnityEngine.Object) null)
      {
        enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.WaitForTarget());
        yield break;
      }
      float a = Vector3.Distance(enemySwordsmanWolf.TargetObject.transform.position, enemySwordsmanWolf.transform.position);
      if ((double) a <= (double) enemySwordsmanWolf.VisionRange)
      {
        if (!enemySwordsmanWolf.requireLineOfSite || enemySwordsmanWolf.CheckLineOfSightOnTarget(enemySwordsmanWolf.TargetObject, enemySwordsmanWolf.TargetObject.transform.position, Mathf.Min(a, (float) enemySwordsmanWolf.VisionRange)))
          InRange = true;
        else
          enemySwordsmanWolf.LookAtTarget();
      }
      yield return (object) null;
    }
    enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.ChasePlayer());
  }

  public void LookAtTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return;
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
    if (!(this.Spine.AnimationName != "jeer"))
      return;
    this.Spine.randomOffset = true;
    this.Spine.AnimationState.SetAnimation(0, "jeer", true);
  }

  public IEnumerator ChasePlayer()
  {
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    enemySwordsmanWolf.MyState = EnemySwordsmanWolf.State.WaitAndTaunt;
    enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsmanWolf.AttackDelay = UnityEngine.Random.Range(enemySwordsmanWolf.AttackDelayRandomRange.x, enemySwordsmanWolf.AttackDelayRandomRange.y);
    if (enemySwordsmanWolf.health.HasShield)
      enemySwordsmanWolf.AttackDelay = 2.5f;
    enemySwordsmanWolf.MaxAttackDelay = UnityEngine.Random.Range(enemySwordsmanWolf.MaxAttackDelayRandomRange.x, enemySwordsmanWolf.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemySwordsmanWolf.TargetObject == (UnityEngine.Object) null)
      {
        enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.WaitForTarget());
        break;
      }
      if ((UnityEngine.Object) enemySwordsmanWolf.damageColliderEvents != (UnityEngine.Object) null)
        enemySwordsmanWolf.damageColliderEvents.SetActive(false);
      enemySwordsmanWolf.TeleportDelay -= Time.deltaTime * enemySwordsmanWolf.Spine.timeScale;
      enemySwordsmanWolf.AttackDelay -= Time.deltaTime * enemySwordsmanWolf.Spine.timeScale;
      enemySwordsmanWolf.MaxAttackDelay -= Time.deltaTime * enemySwordsmanWolf.Spine.timeScale;
      if (enemySwordsmanWolf.MyState == EnemySwordsmanWolf.State.WaitAndTaunt)
      {
        if (enemySwordsmanWolf.Spine.AnimationName != "roll-stop" && enemySwordsmanWolf.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemySwordsmanWolf.rageActive)
          {
            if (enemySwordsmanWolf.Spine.AnimationName != enemySwordsmanWolf.rageRun)
              enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, enemySwordsmanWolf.rageRun, true);
          }
          else if (enemySwordsmanWolf.Spine.AnimationName != "run")
            enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemySwordsmanWolf.Spine.AnimationName != "cheer1")
          enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "cheer1", true);
        if ((UnityEngine.Object) enemySwordsmanWolf.TargetObject == (UnityEngine.Object) PlayerFarming.Instance.gameObject && enemySwordsmanWolf.health.IsCharmed && (UnityEngine.Object) enemySwordsmanWolf.GetClosestTarget() != (UnityEngine.Object) null)
          enemySwordsmanWolf.TargetObject = enemySwordsmanWolf.GetClosestTarget().gameObject;
        enemySwordsmanWolf.state.LookAngle = Utils.GetAngle(enemySwordsmanWolf.transform.position, enemySwordsmanWolf.TargetObject.transform.position);
        enemySwordsmanWolf.Spine.skeleton.ScaleX = (double) enemySwordsmanWolf.state.LookAngle <= 90.0 || (double) enemySwordsmanWolf.state.LookAngle >= 270.0 ? -1f : 1f;
        if (enemySwordsmanWolf.state.CURRENT_STATE == StateMachine.State.Idle)
        {
          if ((double) (enemySwordsmanWolf.RepathTimer -= Time.deltaTime * enemySwordsmanWolf.Spine.timeScale) < 0.0)
          {
            if (enemySwordsmanWolf.CustomAttackLogic())
              break;
            if ((double) enemySwordsmanWolf.MaxAttackDelay < 0.0 || (double) Vector3.Distance(enemySwordsmanWolf.transform.position, enemySwordsmanWolf.TargetObject.transform.position) < (double) enemySwordsmanWolf.AttackWithinRange)
            {
              enemySwordsmanWolf.AttackWithinRange = float.MaxValue;
              if ((bool) (UnityEngine.Object) enemySwordsmanWolf.TargetObject)
              {
                if (enemySwordsmanWolf.ChargeAndAttack && !enemySwordsmanWolf.enraging && ((double) enemySwordsmanWolf.MaxAttackDelay < 0.0 || (double) enemySwordsmanWolf.AttackDelay < 0.0 || enemySwordsmanWolf.rageActive))
                {
                  enemySwordsmanWolf.health.invincible = false;
                  enemySwordsmanWolf.StopAllCoroutines();
                  enemySwordsmanWolf.DisableForces = false;
                  if (enemySwordsmanWolf.rageActive)
                    enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.FightPlayer(enemySwordsmanWolf.rageAttackDistance));
                  else
                    enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.FightPlayer());
                }
                else if (!enemySwordsmanWolf.health.HasShield)
                {
                  enemySwordsmanWolf.Angle = (float) (((double) Utils.GetAngle(enemySwordsmanWolf.TargetObject.transform.position, enemySwordsmanWolf.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
                  enemySwordsmanWolf.TargetPosition = enemySwordsmanWolf.TargetObject.transform.position + new Vector3(enemySwordsmanWolf.MaintainTargetDistance * Mathf.Cos(enemySwordsmanWolf.Angle), enemySwordsmanWolf.MaintainTargetDistance * Mathf.Sin(enemySwordsmanWolf.Angle));
                  enemySwordsmanWolf.FindPath(enemySwordsmanWolf.TargetPosition);
                }
              }
            }
            else if ((bool) (UnityEngine.Object) enemySwordsmanWolf.TargetObject && (double) Vector3.Distance(enemySwordsmanWolf.transform.position, enemySwordsmanWolf.TargetObject.transform.position) > (double) enemySwordsmanWolf.MoveCloserDistance + (enemySwordsmanWolf.health.HasShield ? 0.0 : 1.0))
            {
              enemySwordsmanWolf.Angle = (float) (((double) Utils.GetAngle(enemySwordsmanWolf.TargetObject.transform.position, enemySwordsmanWolf.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemySwordsmanWolf.TargetPosition = enemySwordsmanWolf.TargetObject.transform.position + new Vector3(enemySwordsmanWolf.MaintainTargetDistance * Mathf.Cos(enemySwordsmanWolf.Angle), enemySwordsmanWolf.MaintainTargetDistance * Mathf.Sin(enemySwordsmanWolf.Angle));
              enemySwordsmanWolf.FindPath(enemySwordsmanWolf.TargetPosition);
            }
          }
        }
        else if ((double) (enemySwordsmanWolf.RepathTimer += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale) > 2.0 && !enemySwordsmanWolf.rageActive)
        {
          enemySwordsmanWolf.RepathTimer = 0.0f;
          enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.Idle;
        }
        else if (enemySwordsmanWolf.rageActive)
        {
          enemySwordsmanWolf.Angle = (float) (((double) Utils.GetAngle(enemySwordsmanWolf.TargetObject.transform.position, enemySwordsmanWolf.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
          enemySwordsmanWolf.TargetPosition = enemySwordsmanWolf.TargetObject.transform.position;
          enemySwordsmanWolf.FindPath(enemySwordsmanWolf.TargetPosition);
        }
      }
      enemySwordsmanWolf.Seperate(0.5f);
      yield return (object) null;
    }
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    if (!string.IsNullOrEmpty(this.DrawbackSwordSFX))
      AudioManager.Instance.PlayOneShot(this.DrawbackSwordSFX, this.transform.position);
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
    if (this.MyState != EnemySwordsmanWolf.State.WaitAndTaunt || (double) this.health.HP <= 0.0)
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
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    enemySwordsmanWolf.ClearPaths();
    enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.Moving;
    enemySwordsmanWolf.UsePathing = false;
    enemySwordsmanWolf.health.invincible = true;
    enemySwordsmanWolf.SeperateObject = false;
    enemySwordsmanWolf.MyState = EnemySwordsmanWolf.State.Teleporting;
    enemySwordsmanWolf.ClearPaths();
    Vector3 position = enemySwordsmanWolf.transform.position;
    float Progress = 0.0f;
    enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "roll", true);
    enemySwordsmanWolf.state.facingAngle = enemySwordsmanWolf.state.LookAngle = Utils.GetAngle(enemySwordsmanWolf.transform.position, Position);
    enemySwordsmanWolf.Spine.skeleton.ScaleX = (double) enemySwordsmanWolf.state.LookAngle <= 90.0 || (double) enemySwordsmanWolf.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 b = Position;
    float Duration = Vector3.Distance(position, b) / 10f;
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/swordsman_mutated/mv_roll", enemySwordsmanWolf.gameObject);
    while ((double) (Progress += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale) < (double) Duration)
    {
      enemySwordsmanWolf.speed = 10f * Time.deltaTime * enemySwordsmanWolf.Spine.timeScale;
      yield return (object) null;
    }
    enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "roll-stop", false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemySwordsmanWolf.UsePathing = true;
    enemySwordsmanWolf.RepathTimer = 0.5f;
    enemySwordsmanWolf.TeleportDelay = enemySwordsmanWolf.TeleportDelayTarget;
    enemySwordsmanWolf.SeperateObject = true;
    enemySwordsmanWolf.health.invincible = false;
    enemySwordsmanWolf.MyState = EnemySwordsmanWolf.State.WaitAndTaunt;
  }

  public void GraveSpawn(bool longAnim = false)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.GraveSpawnRoutine(longAnim));
  }

  public IEnumerator GraveSpawnRoutine(bool longAnim = false)
  {
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    enemySwordsmanWolf.health.invincible = true;
    enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, longAnim ? "grave-spawn-long" : "grave-spawn", false);
    enemySwordsmanWolf.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    enemySwordsmanWolf.health.invincible = false;
    enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.WaitForTarget());
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

  public IEnumerator FightPlayer(float AttackDistance = 1.5f)
  {
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    enemySwordsmanWolf.MyState = EnemySwordsmanWolf.State.Attacking;
    enemySwordsmanWolf.UsePathing = true;
    enemySwordsmanWolf.givePath(enemySwordsmanWolf.TargetObject.transform.position);
    enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "run-charge", true);
    if (!enemySwordsmanWolf.rageActive && !string.IsNullOrEmpty(enemySwordsmanWolf.DrawbackSwordSFX))
      AudioManager.Instance.PlayOneShot(enemySwordsmanWolf.DrawbackSwordSFX, enemySwordsmanWolf.gameObject);
    float distanceResetTimer = 0.0f;
    Vector2 previousPosition = (Vector2) enemySwordsmanWolf.transform.position;
    enemySwordsmanWolf.RepathTimer = 0.0f;
    int NumAttacks = enemySwordsmanWolf.DoubleAttack ? 2 : 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = 0.5f;
    while (Loop)
    {
      if ((UnityEngine.Object) enemySwordsmanWolf.Spine == (UnityEngine.Object) null || enemySwordsmanWolf.Spine.AnimationState == null || enemySwordsmanWolf.Spine.Skeleton == null)
      {
        yield return (object) null;
      }
      else
      {
        enemySwordsmanWolf.Seperate(0.5f);
        switch (enemySwordsmanWolf.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
            enemySwordsmanWolf.TargetObject = (GameObject) null;
            enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.WaitForTarget());
            yield break;
          case StateMachine.State.Moving:
            if ((bool) (UnityEngine.Object) enemySwordsmanWolf.TargetObject)
            {
              enemySwordsmanWolf.state.LookAngle = Utils.GetAngle(enemySwordsmanWolf.transform.position, enemySwordsmanWolf.TargetObject.transform.position);
              enemySwordsmanWolf.Spine.skeleton.ScaleX = (double) enemySwordsmanWolf.state.LookAngle <= 90.0 || (double) enemySwordsmanWolf.state.LookAngle >= 270.0 ? -1f : 1f;
              enemySwordsmanWolf.state.LookAngle = enemySwordsmanWolf.state.facingAngle = Utils.GetAngle(enemySwordsmanWolf.transform.position, enemySwordsmanWolf.TargetObject.transform.position);
            }
            if ((UnityEngine.Object) enemySwordsmanWolf.TargetObject != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) enemySwordsmanWolf.transform.position, (Vector2) enemySwordsmanWolf.TargetObject.transform.position) < (double) AttackDistance)
            {
              enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              if (enemySwordsmanWolf.rageActive)
              {
                enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "berserk-bite-charge" : "berserk-bite-charge", false);
                if (!string.IsNullOrEmpty(enemySwordsmanWolf.EnragedChargeAttackSFX))
                  enemySwordsmanWolf.enragedChargeSFXInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySwordsmanWolf.EnragedChargeAttackSFX, enemySwordsmanWolf.transform);
                if (!string.IsNullOrEmpty(enemySwordsmanWolf.AttackVO))
                  enemySwordsmanWolf.enragedChargeVOInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup(enemySwordsmanWolf.EnragedChargeAttackVO, enemySwordsmanWolf.transform);
              }
              else
                enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "grunt-attack-charge2" : "grunt-attack-charge", false);
            }
            else
            {
              if ((double) (enemySwordsmanWolf.RepathTimer += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale) > 0.20000000298023224 && (bool) (UnityEngine.Object) enemySwordsmanWolf.TargetObject && !enemySwordsmanWolf.rageActive)
              {
                enemySwordsmanWolf.RepathTimer = 0.0f;
                enemySwordsmanWolf.givePath(enemySwordsmanWolf.TargetObject.transform.position);
              }
              if (enemySwordsmanWolf.FollowPlayer && (bool) (UnityEngine.Object) enemySwordsmanWolf.TargetObject)
              {
                double magnitude = (double) ((Vector2) enemySwordsmanWolf.transform.position - previousPosition).magnitude;
                previousPosition = (Vector2) enemySwordsmanWolf.transform.position;
                if (magnitude <= 0.14000000059604645)
                {
                  distanceResetTimer += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale;
                  if ((double) distanceResetTimer > 6.5)
                    enemySwordsmanWolf.transform.position = PlayerFarming.Instance.transform.position;
                }
                else
                  distanceResetTimer = 0.0f;
              }
              if ((UnityEngine.Object) enemySwordsmanWolf.damageColliderEvents != (UnityEngine.Object) null)
              {
                if ((double) enemySwordsmanWolf.state.Timer < 0.20000000298023224 && !enemySwordsmanWolf.health.WasJustParried)
                  enemySwordsmanWolf.damageColliderEvents.SetActive(true);
                else
                  enemySwordsmanWolf.damageColliderEvents.SetActive(false);
              }
            }
            if ((UnityEngine.Object) enemySwordsmanWolf.damageColliderEvents != (UnityEngine.Object) null)
            {
              enemySwordsmanWolf.damageColliderEvents.SetActive(false);
              break;
            }
            break;
          case StateMachine.State.SignPostAttack:
            if ((UnityEngine.Object) enemySwordsmanWolf.TargetObject == (UnityEngine.Object) null)
            {
              enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "idle", true);
              enemySwordsmanWolf.SimpleSpineFlash.FlashWhite(false);
              enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.Idle;
              break;
            }
            if ((UnityEngine.Object) enemySwordsmanWolf.damageColliderEvents != (UnityEngine.Object) null)
              enemySwordsmanWolf.damageColliderEvents.SetActive(false);
            enemySwordsmanWolf.SimpleSpineFlash.FlashWhite(enemySwordsmanWolf.state.Timer / SignPostDelay);
            enemySwordsmanWolf.state.Timer += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale;
            if ((double) enemySwordsmanWolf.state.Timer >= (double) SignPostDelay - (double) EnemySwordsmanWolf.signPostParryWindow)
              enemySwordsmanWolf.canBeParried = true;
            if (enemySwordsmanWolf.rageActive)
            {
              enemySwordsmanWolf.state.LookAngle = Utils.GetAngle(enemySwordsmanWolf.transform.position, enemySwordsmanWolf.TargetObject.transform.position);
              enemySwordsmanWolf.Spine.skeleton.ScaleX = (double) enemySwordsmanWolf.state.LookAngle <= 90.0 || (double) enemySwordsmanWolf.state.LookAngle >= 270.0 ? -1f : 1f;
              enemySwordsmanWolf.state.LookAngle = enemySwordsmanWolf.state.facingAngle = Utils.GetAngle(enemySwordsmanWolf.transform.position, enemySwordsmanWolf.TargetObject.transform.position);
            }
            if ((double) enemySwordsmanWolf.state.Timer >= (double) SignPostDelay)
            {
              enemySwordsmanWolf.SimpleSpineFlash.FlashWhite(false);
              CameraManager.shakeCamera(0.4f, enemySwordsmanWolf.state.LookAngle);
              enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
              if (enemySwordsmanWolf.rageActive)
              {
                enemySwordsmanWolf.speed = AttackSpeed * 0.06666667f;
                AttackSpeed = enemySwordsmanWolf.rageAttackDashAmount;
                enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "berserk-bite-impact", false);
              }
              else
              {
                enemySwordsmanWolf.speed = AttackSpeed * 0.0166666675f;
                enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, AttackCount == NumAttacks ? "grunt-attack-impact2" : "grunt-attack-impact", false);
              }
              enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.DropAvalanches());
              enemySwordsmanWolf.canBeParried = true;
              if (enemySwordsmanWolf.rageActive)
              {
                enemySwordsmanWolf.health.invincible = true;
                enemySwordsmanWolf.ghost.ghostingEnabled = true;
                enemySwordsmanWolf.health.IsDeflecting = true;
                enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.EnableDamageCollider(0.35f, enemySwordsmanWolf.rageAttackDuration, new System.Action(enemySwordsmanWolf.\u003CFightPlayer\u003Eb__95_0)));
              }
              else
                enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.EnableDamageCollider(0.0f));
              if (!enemySwordsmanWolf.rageActive)
              {
                if (!string.IsNullOrEmpty(enemySwordsmanWolf.attackSoundPath))
                  AudioManager.Instance.PlayOneShot(enemySwordsmanWolf.attackSoundPath, enemySwordsmanWolf.transform.position);
                if (!string.IsNullOrEmpty(enemySwordsmanWolf.AttackVO))
                {
                  AudioManager.Instance.PlayOneShot(enemySwordsmanWolf.AttackVO, enemySwordsmanWolf.transform.position);
                  break;
                }
                break;
              }
              break;
            }
            break;
          case StateMachine.State.RecoverFromAttack:
            if ((double) AttackSpeed > 0.0)
              AttackSpeed -= 1f * GameManager.DeltaTime * enemySwordsmanWolf.Spine.timeScale;
            enemySwordsmanWolf.speed = AttackSpeed * Time.deltaTime * enemySwordsmanWolf.Spine.timeScale;
            enemySwordsmanWolf.SimpleSpineFlash.FlashWhite(false);
            enemySwordsmanWolf.canBeParried = (double) enemySwordsmanWolf.state.Timer <= (double) EnemySwordsmanWolf.attackParryWindow;
            if ((double) (enemySwordsmanWolf.state.Timer += Time.deltaTime * enemySwordsmanWolf.Spine.timeScale) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
            {
              if (++AttackCount <= NumAttacks)
              {
                AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
                enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
                enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "grunt-attack-charge2", false);
                SignPostDelay = 0.3f;
                break;
              }
              Loop = false;
              enemySwordsmanWolf.SimpleSpineFlash.FlashWhite(false);
              break;
            }
            break;
        }
        yield return (object) null;
      }
    }
    enemySwordsmanWolf.TargetObject = (GameObject) null;
    enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.WaitForTarget());
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.FollowPlayer)
      return;
    this.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
  }

  public IEnumerator PlaceIE()
  {
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    enemySwordsmanWolf.ClearPaths();
    Vector3 offset = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 vector3 = (PlayerFarming.Instance.transform.position + offset) with
      {
        z = 0.0f
      };
      enemySwordsmanWolf.transform.position = vector3;
      yield return (object) null;
    }
    enemySwordsmanWolf.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
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

  public IEnumerator EnableDamageCollider(float initialDelay, float duration = 0.2f, System.Action callback = null)
  {
    if ((bool) (UnityEngine.Object) this.damageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.damageColliderEvents.SetActive(true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) duration)
        yield return (object) null;
      this.damageColliderEvents.SetActive(false);
    }
    System.Action action = callback;
    if (action != null)
      action();
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
    ObjectPool.Spawn(this.trapAvalanche, position, Quaternion.identity).GetComponent<TrapAvalanche>().Drop();
  }

  public IEnumerator Enrage()
  {
    EnemySwordsmanWolf enemySwordsmanWolf = this;
    enemySwordsmanWolf.enraging = true;
    enemySwordsmanWolf.rageActive = true;
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    if (!string.IsNullOrEmpty(enemySwordsmanWolf.TransformIntoEnragedVO))
      AudioManager.Instance.PlayOneShot(enemySwordsmanWolf.TransformIntoEnragedVO, enemySwordsmanWolf.gameObject);
    if (!string.IsNullOrEmpty(enemySwordsmanWolf.TransformIntoEnragedSFX))
      AudioManager.Instance.PlayOneShot(enemySwordsmanWolf.TransformIntoEnragedSFX, enemySwordsmanWolf.gameObject);
    yield return (object) new WaitForEndOfFrame();
    enemySwordsmanWolf.SpeedMultiplier = enemySwordsmanWolf.rageSpeedMultiplier;
    enemySwordsmanWolf.maxSpeed *= enemySwordsmanWolf.SpeedMultiplier;
    enemySwordsmanWolf.rageParticle.gameObject.SetActive(true);
    enemySwordsmanWolf.Spine.Skeleton.SetSkin(enemySwordsmanWolf.rageSkin);
    enemySwordsmanWolf.Spine.Skeleton.SetSlotsToSetupPose();
    enemySwordsmanWolf.Spine.AnimationState.SetAnimation(0, enemySwordsmanWolf.rageEnterAnimationEnd, true);
    enemySwordsmanWolf.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    ShowHPBar hpBar = enemySwordsmanWolf.GetComponent<ShowHPBar>();
    Vector3 endValue = enemySwordsmanWolf.Spine.transform.localScale * 1.2f;
    enemySwordsmanWolf.Spine.transform.DOScale(endValue, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f);
    float healthToAdd = enemySwordsmanWolf.health.totalHP;
    float increment = enemySwordsmanWolf.health.totalHP * Time.fixedDeltaTime;
    while ((double) healthToAdd > 0.0)
    {
      enemySwordsmanWolf.health.HP += increment * 1.5f;
      healthToAdd -= increment * 1.5f;
      hpBar.ForceUpdate();
      yield return (object) new WaitForFixedUpdate();
    }
    enemySwordsmanWolf.enraging = false;
    enemySwordsmanWolf.health.invincible = false;
    enemySwordsmanWolf.health.IsDeflecting = false;
    enemySwordsmanWolf.StartCoroutine((IEnumerator) enemySwordsmanWolf.WaitForTarget());
  }

  public void ResetResilience() => this.canEnrage = true;

  public void StopResilience() => this.canEnrage = false;

  [CompilerGenerated]
  public void \u003COnDieEarly\u003Eb__68_0() => UnityEngine.Object.Destroy((UnityEngine.Object) this.helmet.gameObject);

  [CompilerGenerated]
  public void \u003CFightPlayer\u003Eb__95_0()
  {
    this.health.invincible = false;
    this.health.IsDeflecting = false;
    this.ghost.ghostingEnabled = false;
  }

  public enum State
  {
    WaitAndTaunt,
    Teleporting,
    Attacking,
  }
}
