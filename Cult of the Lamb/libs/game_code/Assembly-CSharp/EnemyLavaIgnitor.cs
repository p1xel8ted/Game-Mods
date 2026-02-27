// Decompiled with JetBrains decompiler
// Type: EnemyLavaIgnitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyLavaIgnitor : UnitObject
{
  public GameObject OnFireFX;
  public GameObject ExtinguishedFX;
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash SimpleSpineFlash;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RunScaredAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string JumpAnimationSignpost;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string JumpAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FlyAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SlamAnimation;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string OnFireSkin;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string ExtinguishedSkin;
  [HideInInspector]
  public bool isDying;
  [HideInInspector]
  public bool onFire = true;
  public float pounceDistance = 4f;
  public CircleCollider2D circleCollider;
  public float waitUntilNextPounceTimeMin = 2f;
  public float waitUntilNextPounceTimeMax = 4f;
  public float nextPounce;
  public bool isReadyToDie;
  public float reigniteThreshold = 0.33f;
  public Coroutine currentStateRoutine;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string RunDropSFX = "event:/enemy/hopper/hopper_land";
  [EventRef]
  public string FlyStartSFX = "event:/dlc/dungeon06/enemy/lavaignitor/mv_fly_start";
  [EventRef]
  public string SlamDownSFX = "event:/dlc/dungeon06/enemy/lavaignitor/attack_slam_ground_impact";
  [EventRef]
  public string AttackSlamExplosionSFX = "event:/dlc/dungeon06/enemy/lavaignitor/attack_slam_explode";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/warning";
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_humanoid_small/death";
  [EventRef]
  public string WingFlapSFX = "event:/dlc/dungeon06/enemy/lavaignitor/mv_wing_flap_os";
  [EventRef]
  public string SubmergeInLavaSFX = "event:/dlc/dungeon06/enemy/lavaignitor/mv_submerge_lava_impact";
  [EventRef]
  public string ReigniteSFX = "event:/dlc/dungeon06/enemy/lavaignitor/mv_reignite";
  [EventRef]
  public string ExtinguishSFX = "event:/dlc/dungeon06/enemy/lavaignitor/mv_extinguish";
  [EventRef]
  public string DiveSFX = "event:/dlc/dungeon06/enemy/lavaignitor/mv_dive";
  [EventRef]
  public string OnFireLoop = "event:/dlc/dungeon06/enemy/lavaignitor/mv_fire_loop";
  public EventInstance onFireLoopInstance;
  public ColliderEvents damageColliderEvents;
  public GameObject TargetObject;
  public bool prioritiseTraps = true;
  public Vector3 spawnPosition;
  public bool oneLuckyChanceUsed;
  public float inHealDistanceTime;
  public float minDistanceToHealPoint = 100f;
  public float healPointTimeoutTimer;
  public float healPointTimeoutDuration = 4f;
  public EnemyLavaIgnitorHealPoint currentHealPoint;
  public GameObject SlamTrapPrefab;
  public int numberOfTraps = 3;
  public float spawnRadius = 1.25f;

  public override void Awake()
  {
    base.Awake();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.UsePathing = true;
    this.SlamTrapPrefab.CreatePool(16 /*0x10*/);
    this.spawnPosition = this.transform.position;
  }

  public override void OnEnable()
  {
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.health.OnHit += new Health.HitAction(((UnitObject) this).OnHit);
    this.health.OnDieEarly += new Health.DieAction(((UnitObject) this).OnDieEarly);
    this.health.OnDie += new Health.DieAction(((UnitObject) this).OnDie);
    this.health.OnDamaged += new Health.HealthEvent(this.OnDamaged);
    this.health.OnIced += new Health.StasisEvent(this.OnIced);
    this.health.OnBurnedFailed += new Health.StasisEvent(this.OnBurned);
    if (this.onFire)
      this.SetOnFire();
    else
      this.SetExtinguised();
    base.OnEnable();
    this.StartState(this.IdleState());
  }

  public void SetOnFire()
  {
    this.onFire = true;
    this.SetSkin(this.OnFireSkin);
    this.OnFireFX.SetActive(true);
    this.ExtinguishedFX.SetActive(false);
    this.health.ClearIce();
    if (string.IsNullOrEmpty(this.OnFireLoop) || AudioManager.Instance.IsEventInstancePlaying(this.onFireLoopInstance))
      return;
    this.onFireLoopInstance = AudioManager.Instance.CreateLoop(this.OnFireLoop, this.gameObject, true);
  }

  public void SetExtinguised()
  {
    this.onFire = false;
    this.SetSkin(this.ExtinguishedSkin);
    this.OnFireFX.SetActive(false);
    this.ExtinguishedFX.SetActive(true);
    AudioManager.Instance.StopLoop(this.onFireLoopInstance);
    if (string.IsNullOrEmpty(this.ExtinguishSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.ExtinguishSFX);
  }

  public void SetSkin(string skin)
  {
    this.Spine.enabled = false;
    this.Spine.Skeleton.SetSkin(skin);
    this.Spine.enabled = true;
  }

  public override void OnDisable()
  {
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.health.OnHit -= new Health.HitAction(((UnitObject) this).OnHit);
    this.health.OnDieEarly -= new Health.DieAction(((UnitObject) this).OnDieEarly);
    this.health.OnDie -= new Health.DieAction(((UnitObject) this).OnDie);
    this.health.OnDamaged -= new Health.HealthEvent(this.OnDamaged);
    this.health.OnIced -= new Health.StasisEvent(this.OnIced);
    this.health.OnBurnedFailed -= new Health.StasisEvent(this.OnBurned);
    this.StopAllCoroutines();
    this.ClearPaths();
    AudioManager.Instance.StopLoop(this.onFireLoopInstance);
    base.OnDisable();
  }

  public override void OnDestroy()
  {
  }

  public override void OnDieEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.oneLuckyChanceUsed && (double) this.health.HP <= 0.0 && this.onFire && (double) this.health.totalHP * (double) this.reigniteThreshold >= 1.0 && AttackFlags != Health.AttackFlags.Ice)
    {
      this.health.HP = 1f;
      this.checkFireStatusFromHit(Attacker, AttackFlags);
      this.health.invincible = true;
      this.oneLuckyChanceUsed = true;
    }
    base.OnDieEarly(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO);
    AudioManager.Instance.StopLoop(this.onFireLoopInstance);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public void OnIced() => this.checkFireStatusFromHit((GameObject) null, Health.AttackFlags.Ice);

  public void OnBurned() => this.checkFireStatusFromHit((GameObject) null, Health.AttackFlags.Burn);

  public void checkFireStatusFromHit(GameObject Attacker, Health.AttackFlags attackFlags)
  {
    bool flag1 = attackFlags.HasFlag((Enum) Health.AttackFlags.Ice);
    bool flag2 = attackFlags.HasFlag((Enum) Health.AttackFlags.Burn);
    if ((double) this.health.HP <= (double) this.health.totalHP * (double) this.reigniteThreshold | flag1 && this.onFire)
    {
      this.StopAllCoroutines();
      this.ClearPaths();
      this.transform.DOKill();
      this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
      this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
      this.SetExtinguised();
      this.health.invincible = false;
      this.circleCollider.enabled = true;
      this.damageColliderEvents.SetActive(false);
      this.StartState(this.FleeEnemyState(Attacker));
    }
    else
    {
      if (!flag2)
        return;
      DOVirtual.DelayedCall(0.25f, (TweenCallback) (() =>
      {
        if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null) || (double) this.health.HP < 0.0)
          return;
        this.HealUnit();
        if (this.onFire)
          return;
        this.StartState(this.IdleState());
        this.SetOnFire();
      }));
    }
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team != Health.Team.PlayerTeam || this.health.team != Health.Team.Team2)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.SimpleSpineFlash.FlashFillRed();
    this.Spine.transform.DOKill();
    this.Spine.transform.localScale = Vector3.one;
    this.Spine.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, elasticity: 0.8f).OnComplete<Tweener>((TweenCallback) (() => this.Spine.transform.localScale = Vector3.one));
    this.DoKnockBack(Attacker, 1f, 1f);
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
  }

  public void OnDamaged(
    GameObject attacker,
    Vector3 attackLocation,
    float damage,
    Health.AttackTypes attackType,
    Health.AttackFlags attackFlag)
  {
    this.checkFireStatusFromHit(attacker, attackFlag);
    this.healPointTimeoutTimer = this.healPointTimeoutDuration;
  }

  public void StartState(IEnumerator newState)
  {
    if (this.currentStateRoutine != null)
      this.StopCoroutine(this.currentStateRoutine);
    this.currentStateRoutine = this.StartCoroutine(newState);
  }

  public IEnumerator IdleState()
  {
    EnemyLavaIgnitor enemyLavaIgnitor = this;
    enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.IdleAnimation, true);
    enemyLavaIgnitor.health.invincible = false;
    if ((double) enemyLavaIgnitor.transform.position.z < 0.0)
    {
      enemyLavaIgnitor.transform.DOKill();
      enemyLavaIgnitor.transform.DOMove(new Vector3(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y, 0.0f), 0.3f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    if (enemyLavaIgnitor.onFire)
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.ApproachEnemyState());
    else if ((bool) (UnityEngine.Object) enemyLavaIgnitor.currentHealPoint)
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.ApproachHealPointState());
    else
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
  }

  public IEnumerator ApproachEnemyState()
  {
    EnemyLavaIgnitor enemyLavaIgnitor = this;
    if (!enemyLavaIgnitor.onFire)
    {
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
    }
    else
    {
      enemyLavaIgnitor.maxSpeed = 0.05f;
      enemyLavaIgnitor.state.CURRENT_STATE = StateMachine.State.Moving;
      yield return (object) new WaitForSeconds(0.1f);
      enemyLavaIgnitor.UsePathing = true;
      while (enemyLavaIgnitor.onFire)
      {
        yield return (object) new WaitForSeconds((float) (0.20000000298023224 + (double) UnityEngine.Random.value * 0.30000001192092896));
        enemyLavaIgnitor.SpawnTrapOnWalk(enemyLavaIgnitor.transform.position);
        Health closestTarget = enemyLavaIgnitor.GetClosestTarget();
        if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
        {
          enemyLavaIgnitor.TargetObject = closestTarget.gameObject;
          enemyLavaIgnitor.FaceTarget(enemyLavaIgnitor.TargetObject);
          enemyLavaIgnitor.givePath(enemyLavaIgnitor.TargetObject.transform.position, enemyLavaIgnitor.TargetObject);
          enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.RunAnimation, true);
          AudioManager.Instance.PlayFootstep(enemyLavaIgnitor.transform.position);
          if ((double) Vector3.Distance(enemyLavaIgnitor.TargetObject.transform.position, enemyLavaIgnitor.transform.position) < (double) enemyLavaIgnitor.pounceDistance && (double) Time.time > (double) enemyLavaIgnitor.nextPounce)
          {
            enemyLavaIgnitor.StartState(enemyLavaIgnitor.PounceOnTarget(enemyLavaIgnitor.TargetObject));
            yield break;
          }
        }
      }
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
    }
  }

  public void FaceTarget(GameObject TargetObject)
  {
    if ((double) TargetObject.transform.position.x <= (double) this.transform.position.x - 0.20000000298023224)
    {
      this.state.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    else
    {
      if ((double) TargetObject.transform.position.x < (double) this.transform.position.x + 0.20000000298023224)
        return;
      this.state.transform.localScale = new Vector3(-1f, 1f, 1f);
    }
  }

  public IEnumerator ApproachHealPointState()
  {
    EnemyLavaIgnitor enemyLavaIgnitor = this;
    enemyLavaIgnitor.DisableForces = false;
    if ((UnityEngine.Object) enemyLavaIgnitor.currentHealPoint == (UnityEngine.Object) null)
    {
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
    }
    else
    {
      enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.IdleAnimation, true);
      enemyLavaIgnitor.health.invincible = true;
      enemyLavaIgnitor.maxSpeed = 0.075f;
      float t = 0.0f;
      while ((double) t < 0.25)
      {
        t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
        yield return (object) null;
      }
      enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.RunScaredAnimation, true);
      enemyLavaIgnitor.health.invincible = false;
      enemyLavaIgnitor.UsePathing = true;
      enemyLavaIgnitor.DisableForces = false;
      enemyLavaIgnitor.state.CURRENT_STATE = StateMachine.State.Moving;
      EnemyLavaIgnitorHealPoint targetHealPoint = enemyLavaIgnitor.currentHealPoint;
      enemyLavaIgnitor.FaceTarget(targetHealPoint.gameObject);
      enemyLavaIgnitor.healPointTimeoutTimer = enemyLavaIgnitor.healPointTimeoutDuration;
      enemyLavaIgnitor.givePath(targetHealPoint.transform.position, targetHealPoint.gameObject);
      Vector3 lastPosition = enemyLavaIgnitor.transform.position;
      int stuckFrames = 0;
      do
      {
        t = 0.0f;
        while ((double) t < 0.25)
        {
          t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
          ++stuckFrames;
          if ((double) Vector3.Distance(enemyLavaIgnitor.transform.position, lastPosition) < 0.05000000074505806)
          {
            if (stuckFrames >= 30)
              enemyLavaIgnitor.circleCollider.enabled = false;
          }
          else
            stuckFrames = 0;
          lastPosition = enemyLavaIgnitor.transform.position;
          yield return (object) null;
        }
        enemyLavaIgnitor.FaceTarget(targetHealPoint.gameObject);
        enemyLavaIgnitor.givePath(targetHealPoint.transform.position, targetHealPoint.gameObject);
      }
      while ((double) Vector2.Distance((Vector2) targetHealPoint.transform.position, (Vector2) enemyLavaIgnitor.transform.position) >= (double) targetHealPoint.healTriggerDistance && (double) (enemyLavaIgnitor.healPointTimeoutTimer -= Time.deltaTime * enemyLavaIgnitor.Spine.timeScale) > 0.0);
      enemyLavaIgnitor.minDistanceToHealPoint = 100f;
      t = 0.0f;
      while ((double) t < 0.25)
      {
        t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
        yield return (object) null;
      }
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.PounceOnHealPointState());
    }
  }

  public void SpineEventWingFlag()
  {
    if (string.IsNullOrEmpty(this.WingFlapSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.WingFlapSFX);
  }

  public IEnumerator PounceOnTarget(GameObject TargetObject, bool rechargeSlam = false)
  {
    EnemyLavaIgnitor enemyLavaIgnitor = this;
    Tweener moveTween = (Tweener) null;
    moveTween = (Tweener) enemyLavaIgnitor.transform.DOMove(new Vector3(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y, 0.0f), 0.35f);
    enemyLavaIgnitor.state.CURRENT_STATE = StateMachine.State.Attacking;
    if (!string.IsNullOrEmpty(enemyLavaIgnitor.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.AttackVO);
    enemyLavaIgnitor.FaceTarget(TargetObject);
    enemyLavaIgnitor.ClearPaths();
    enemyLavaIgnitor.UsePathing = false;
    enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.JumpAnimationSignpost, false);
    float t = 0.0f;
    while ((double) t < 0.34999999403953552)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    enemyLavaIgnitor.transform.position = new Vector3(enemyLavaIgnitor.transform.transform.position.x, enemyLavaIgnitor.transform.transform.position.y, 0.0f);
    moveTween.Kill();
    AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.FlyStartSFX, enemyLavaIgnitor.transform.position);
    enemyLavaIgnitor.health.invincible = true;
    enemyLavaIgnitor.circleCollider.enabled = false;
    enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.JumpAnimation, false);
    enemyLavaIgnitor.Spine.AnimationState.AddAnimation(0, enemyLavaIgnitor.FlyAnimation, true, 0.75f);
    t = 0.0f;
    while ((double) t < 0.30000001192092896)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    if ((UnityEngine.Object) TargetObject == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) enemyLavaIgnitor.GetClosestTarget() != (UnityEngine.Object) null)
      {
        TargetObject = enemyLavaIgnitor.GetClosestTarget().gameObject;
      }
      else
      {
        moveTween.Kill();
        enemyLavaIgnitor.StartCoroutine(enemyLavaIgnitor.SlamImmediate());
        yield break;
      }
    }
    if ((double) enemyLavaIgnitor.Spine.timeScale > 0.5)
      enemyLavaIgnitor.FaceTarget(TargetObject);
    moveTween.Kill();
    moveTween = (Tweener) enemyLavaIgnitor.transform.DOMove(new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, 0.0f), 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad).OnUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      if ((double) this.Spine.timeScale > 0.5 && (UnityEngine.Object) TargetObject != (UnityEngine.Object) null)
      {
        this.FaceTarget(TargetObject);
        moveTween.ChangeEndValue((object) new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, 0.0f), true);
      }
      moveTween.timeScale = this.Spine.timeScale;
    }));
    t = 0.0f;
    while ((double) t < 0.75)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    moveTween.Kill();
    if (rechargeSlam)
    {
      t = 0.0f;
      while ((double) t < 0.5)
      {
        t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
        yield return (object) null;
      }
      Vector3 vector3 = (UnityEngine.Object) TargetObject != (UnityEngine.Object) null ? TargetObject.transform.position : enemyLavaIgnitor.transform.position;
      moveTween = (Tweener) enemyLavaIgnitor.transform.DOMove(new Vector3(vector3.x, vector3.y, 5f), 0.3f);
    }
    enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.SlamAnimation, false);
    if (!string.IsNullOrEmpty(enemyLavaIgnitor.DiveSFX))
      AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.DiveSFX);
    t = 0.0f;
    while ((double) t < 0.15000000596046448)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    if (rechargeSlam)
    {
      Explosion.CreateExplosion(new Vector3(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y, 2f), enemyLavaIgnitor.health.team, enemyLavaIgnitor.health, 2f, 1f, playSFX: false);
      if (!string.IsNullOrEmpty(enemyLavaIgnitor.SubmergeInLavaSFX))
        AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.SubmergeInLavaSFX, enemyLavaIgnitor.transform.position);
    }
    t = 0.0f;
    while ((double) t < 0.15000000596046448)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    moveTween.Kill();
    enemyLavaIgnitor.damageColliderEvents.SetActive(true);
    if (!string.IsNullOrEmpty(enemyLavaIgnitor.SlamDownSFX))
      AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.SlamDownSFX, enemyLavaIgnitor.transform.position);
    CameraManager.shakeCamera(2f);
    if (!rechargeSlam)
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    t = 0.0f;
    while ((double) t < 0.20000000298023224)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    if (rechargeSlam)
    {
      enemyLavaIgnitor.health.invincible = false;
      enemyLavaIgnitor.circleCollider.enabled = true;
      enemyLavaIgnitor.damageColliderEvents.SetActive(false);
      t = 0.0f;
      while ((double) t < 0.800000011920929)
      {
        t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
        yield return (object) null;
      }
    }
    else
    {
      enemyLavaIgnitor.SpawnTrapsAroundPoint(enemyLavaIgnitor.transform.position);
      BiomeConstants.Instance.EmitHammerEffects(enemyLavaIgnitor.transform.position, 0.1f, 1.2f, 0.3f, true, 1f, false);
      Explosion.CreateExplosion(enemyLavaIgnitor.transform.position, enemyLavaIgnitor.health.team, enemyLavaIgnitor.health, 2f, 1f, playSFX: false);
      if (!string.IsNullOrEmpty(enemyLavaIgnitor.AttackSlamExplosionSFX))
        AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.AttackSlamExplosionSFX, enemyLavaIgnitor.transform.position);
      enemyLavaIgnitor.damageColliderEvents.SetActive(false);
      enemyLavaIgnitor.health.invincible = false;
      enemyLavaIgnitor.circleCollider.enabled = true;
      t = 0.0f;
      while ((double) t < 0.800000011920929)
      {
        t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
        yield return (object) null;
      }
      enemyLavaIgnitor.nextPounce = Time.time + UnityEngine.Random.Range(enemyLavaIgnitor.waitUntilNextPounceTimeMin, enemyLavaIgnitor.waitUntilNextPounceTimeMax);
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
    }
  }

  public IEnumerator SlamImmediate()
  {
    EnemyLavaIgnitor enemyLavaIgnitor = this;
    enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.SlamAnimation, false);
    if (!string.IsNullOrEmpty(enemyLavaIgnitor.DiveSFX))
      AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.DiveSFX);
    float t = 0.0f;
    while ((double) t < 0.15000000596046448)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    t = 0.0f;
    while ((double) t < 0.15000000596046448)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    enemyLavaIgnitor.damageColliderEvents.SetActive(true);
    if (!string.IsNullOrEmpty(enemyLavaIgnitor.SlamDownSFX))
      AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.SlamDownSFX, enemyLavaIgnitor.transform.position);
    CameraManager.shakeCamera(2f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    t = 0.0f;
    while ((double) t < 0.20000000298023224)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    enemyLavaIgnitor.SpawnTrapsAroundPoint(enemyLavaIgnitor.transform.position);
    BiomeConstants.Instance.EmitHammerEffects(enemyLavaIgnitor.transform.position, 0.1f, 1.2f, 0.3f, true, 1f, false);
    Explosion.CreateExplosion(enemyLavaIgnitor.transform.position, enemyLavaIgnitor.health.team, enemyLavaIgnitor.health, 2f, 1f, playSFX: false);
    if (!string.IsNullOrEmpty(enemyLavaIgnitor.AttackSlamExplosionSFX))
      AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.AttackSlamExplosionSFX, enemyLavaIgnitor.transform.position);
    enemyLavaIgnitor.damageColliderEvents.SetActive(false);
    enemyLavaIgnitor.health.invincible = false;
    enemyLavaIgnitor.circleCollider.enabled = true;
    t = 0.0f;
    while ((double) t < 0.800000011920929)
    {
      t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
      yield return (object) null;
    }
    enemyLavaIgnitor.nextPounce = Time.time + UnityEngine.Random.Range(enemyLavaIgnitor.waitUntilNextPounceTimeMin, enemyLavaIgnitor.waitUntilNextPounceTimeMax);
    enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
  }

  public IEnumerator PounceOnHealPointState()
  {
    EnemyLavaIgnitor enemyLavaIgnitor = this;
    if ((UnityEngine.Object) enemyLavaIgnitor.currentHealPoint == (UnityEngine.Object) null)
    {
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
    }
    else
    {
      enemyLavaIgnitor.state.CURRENT_STATE = StateMachine.State.Idle;
      enemyLavaIgnitor.ClearPaths();
      enemyLavaIgnitor.UsePathing = false;
      GameObject startObject = new GameObject("Return Jump Point");
      startObject.transform.SetParent(enemyLavaIgnitor.transform.parent);
      startObject.transform.position = enemyLavaIgnitor.transform.position;
      UnityEngine.Object.Destroy((UnityEngine.Object) startObject, 10f);
      GameObject TargetObject = enemyLavaIgnitor.currentHealPoint.gameObject;
      if (enemyLavaIgnitor.currentHealPoint.requiresJumpToHeal)
      {
        if ((UnityEngine.Object) enemyLavaIgnitor.currentHealPoint.jumpTargetObject != (UnityEngine.Object) null)
          TargetObject = enemyLavaIgnitor.currentHealPoint.jumpTargetObject;
        enemyLavaIgnitor.Spine.Skeleton.SetSkin(enemyLavaIgnitor.OnFireSkin);
        yield return (object) enemyLavaIgnitor.PounceOnTarget(TargetObject, true);
      }
      else
        enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.IdleAnimation, true);
      float t = 0.0f;
      while ((double) t < (double) enemyLavaIgnitor.currentHealPoint.healTimeNeeded * 0.5)
      {
        t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
        yield return (object) null;
      }
      if ((UnityEngine.Object) enemyLavaIgnitor.currentHealPoint.mustBeActiveToHealGameObject != (UnityEngine.Object) null)
      {
        bool healConditionsMet = false;
        while (!healConditionsMet)
        {
          bool flag = (double) Vector2.Distance((Vector2) enemyLavaIgnitor.currentHealPoint.transform.position, (Vector2) enemyLavaIgnitor.transform.position) < (double) enemyLavaIgnitor.currentHealPoint.healTriggerDistance;
          if (enemyLavaIgnitor.currentHealPoint.mustBeActiveToHealGameObject.activeSelf & flag)
            healConditionsMet = true;
          if (!flag)
          {
            enemyLavaIgnitor.StartState(enemyLavaIgnitor.ApproachHealPointState());
            yield break;
          }
          t = 0.0f;
          while ((double) t < 0.10000000149011612)
          {
            t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
            yield return (object) null;
          }
        }
      }
      enemyLavaIgnitor.onFire = true;
      enemyLavaIgnitor.SetOnFire();
      if (!enemyLavaIgnitor.currentHealPoint.requiresJumpBackAfterHeal && !string.IsNullOrEmpty(enemyLavaIgnitor.ReigniteSFX))
        AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.ReigniteSFX);
      enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.IdleAnimation, false);
      enemyLavaIgnitor.OnFireFX.SetActive(true);
      enemyLavaIgnitor.ExtinguishedFX.SetActive(false);
      enemyLavaIgnitor.HealUnit(false);
      enemyLavaIgnitor.prioritiseTraps = !enemyLavaIgnitor.currentHealPoint.isAttachedToFireTrap;
      if (enemyLavaIgnitor.currentHealPoint.requiresJumpBackAfterHeal)
      {
        t = 0.0f;
        while ((double) t < 0.5)
        {
          t += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
          yield return (object) null;
        }
        enemyLavaIgnitor.FaceTarget(enemyLavaIgnitor.currentHealPoint.gameObject);
        Explosion.CreateExplosion(new Vector3(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y, 2f), Health.Team.Team2, enemyLavaIgnitor.health, 2f, 1f, playSFX: false);
        if (!string.IsNullOrEmpty(enemyLavaIgnitor.ReigniteSFX))
          AudioManager.Instance.PlayOneShot(enemyLavaIgnitor.ReigniteSFX);
        yield return (object) enemyLavaIgnitor.PounceOnTarget(startObject);
      }
      enemyLavaIgnitor.currentHealPoint = (EnemyLavaIgnitorHealPoint) null;
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
    }
  }

  public void HealUnit(bool showHeartVFX = true)
  {
    this.health.totalHP *= 0.8f;
    this.health.HP = this.health.totalHP;
    this.health.DisplayHPBar();
    if (!showHeartVFX)
      return;
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position - Vector3.forward, 0.0f, "red", "burst_big");
  }

  public IEnumerator FleeEnemyState(GameObject Attacker)
  {
    EnemyLavaIgnitor enemyLavaIgnitor = this;
    yield return (object) new WaitForSeconds(0.1f);
    if ((UnityEngine.Object) Attacker == (UnityEngine.Object) null)
      Attacker = enemyLavaIgnitor.GetClosestTarget()?.gameObject;
    enemyLavaIgnitor.health.invincible = false;
    if ((bool) (UnityEngine.Object) Attacker)
    {
      enemyLavaIgnitor.DoKnockBack(Attacker, 1.5f, 1.5f);
      enemyLavaIgnitor.health.invincible = true;
      yield return (object) new WaitForSeconds(0.5f);
      enemyLavaIgnitor.health.invincible = false;
    }
    if ((UnityEngine.Object) enemyLavaIgnitor.currentHealPoint != (UnityEngine.Object) null)
    {
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.ApproachHealPointState());
    }
    else
    {
      enemyLavaIgnitor.DisableForces = true;
      enemyLavaIgnitor.UsePathing = false;
      enemyLavaIgnitor.ClearPaths();
      float fleeSpeed = 3f;
      float safeDistance = 5f;
      enemyLavaIgnitor.state.CURRENT_STATE = StateMachine.State.Moving;
      enemyLavaIgnitor.Spine.AnimationState.SetAnimation(0, enemyLavaIgnitor.RunScaredAnimation, true);
      float healSearchTimer = 0.0f;
      float healSearchInterval = 2f;
      float healSearchDistance = 3f;
      Vector3 centerPoint = Vector3.zero;
      while (true)
      {
        healSearchTimer += Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
        if ((double) healSearchTimer >= (double) healSearchInterval)
        {
          if (EnemyLavaIgnitorHealPoint.healPoints != null && EnemyLavaIgnitorHealPoint.healPoints.Count > 0)
          {
            for (int index = 0; index < EnemyLavaIgnitorHealPoint.healPoints.Count; ++index)
            {
              EnemyLavaIgnitorHealPoint healPoint = EnemyLavaIgnitorHealPoint.healPoints[index];
              if ((UnityEngine.Object) healPoint != (UnityEngine.Object) null && healPoint.isAttachedToFireTrap == enemyLavaIgnitor.prioritiseTraps && (double) Vector2.Distance(new Vector2(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y), new Vector2(healPoint.transform.position.x, healPoint.transform.position.y)) <= (double) healSearchDistance)
              {
                enemyLavaIgnitor.currentHealPoint = healPoint;
                enemyLavaIgnitor.DisableForces = false;
                enemyLavaIgnitor.UsePathing = true;
                enemyLavaIgnitor.StartState(enemyLavaIgnitor.ApproachHealPointState());
                yield break;
              }
            }
            for (int index = 0; index < EnemyLavaIgnitorHealPoint.healPoints.Count; ++index)
            {
              EnemyLavaIgnitorHealPoint healPoint = EnemyLavaIgnitorHealPoint.healPoints[index];
              if ((UnityEngine.Object) healPoint != (UnityEngine.Object) null && (double) Vector2.Distance(new Vector2(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y), new Vector2(healPoint.transform.position.x, healPoint.transform.position.y)) <= (double) healSearchDistance)
              {
                enemyLavaIgnitor.currentHealPoint = healPoint;
                enemyLavaIgnitor.DisableForces = false;
                enemyLavaIgnitor.UsePathing = true;
                enemyLavaIgnitor.StartState(enemyLavaIgnitor.ApproachHealPointState());
                yield break;
              }
            }
          }
          healSearchDistance += 3f;
          healSearchTimer = 0.0f;
        }
        GameObject TargetObject = (GameObject) null;
        float num1 = float.MaxValue;
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
        {
          float num2 = Vector2.Distance(new Vector2(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y), new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            TargetObject = gameObject;
          }
        }
        if (!((UnityEngine.Object) TargetObject == (UnityEngine.Object) null))
        {
          if ((double) num1 > (double) safeDistance)
          {
            if ((double) Vector2.Distance(new Vector2(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y), new Vector2(centerPoint.x, centerPoint.y)) >= 1.0)
            {
              Vector2 vector2_1 = new Vector2(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y);
              Vector2 b = new Vector2(centerPoint.x, centerPoint.y);
              Vector2 normalized1 = (b - vector2_1).normalized;
              float distance = Vector2.Distance(vector2_1, b);
              if (!(bool) Physics2D.Raycast(vector2_1, normalized1, distance, LayerMask.GetMask("Island", "Obstacles")))
              {
                Vector3 position = enemyLavaIgnitor.transform.position;
                position.x += normalized1.x * fleeSpeed * Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
                position.y += normalized1.y * fleeSpeed * Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
                enemyLavaIgnitor.transform.position = position;
                if ((double) normalized1.x < -0.20000000298023224)
                  enemyLavaIgnitor.state.transform.localScale = new Vector3(1f, 1f, 1f);
                else if ((double) normalized1.x > 0.20000000298023224)
                  enemyLavaIgnitor.state.transform.localScale = new Vector3(-1f, 1f, 1f);
              }
              else
              {
                Vector2 vector2_2 = new Vector2(TargetObject.transform.position.x, TargetObject.transform.position.y);
                Vector2 normalized2 = (vector2_1 - vector2_2).normalized;
                Vector3 position = enemyLavaIgnitor.transform.position;
                position.x += normalized2.x * fleeSpeed * Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
                position.y += normalized2.y * fleeSpeed * Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
                enemyLavaIgnitor.transform.position = position;
                enemyLavaIgnitor.FaceTarget(TargetObject);
              }
            }
            else
              goto label_29;
          }
          else
          {
            Vector2 normalized = (new Vector2(enemyLavaIgnitor.transform.position.x, enemyLavaIgnitor.transform.position.y) - new Vector2(TargetObject.transform.position.x, TargetObject.transform.position.y)).normalized;
            Vector3 position = enemyLavaIgnitor.transform.position;
            position.x += normalized.x * fleeSpeed * Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
            position.y += normalized.y * fleeSpeed * Time.deltaTime * enemyLavaIgnitor.Spine.timeScale;
            enemyLavaIgnitor.transform.position = position;
            enemyLavaIgnitor.FaceTarget(TargetObject);
          }
          yield return (object) null;
        }
        else
          break;
      }
      enemyLavaIgnitor.DisableForces = false;
      enemyLavaIgnitor.UsePathing = true;
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
      yield break;
label_29:
      enemyLavaIgnitor.DisableForces = false;
      enemyLavaIgnitor.UsePathing = true;
      enemyLavaIgnitor.StartState(enemyLavaIgnitor.IdleState());
    }
  }

  public EnemyLavaIgnitorHealPoint GetBestHealPoint(bool furthest = false)
  {
    if (EnemyLavaIgnitorHealPoint.healPoints == null || EnemyLavaIgnitorHealPoint.healPoints.Count == 0)
      return (EnemyLavaIgnitorHealPoint) null;
    EnemyLavaIgnitorHealPoint bestHealPoint = (EnemyLavaIgnitorHealPoint) null;
    float num1 = float.MaxValue;
    Vector3 position = this.transform.position;
    for (int index = 0; index < EnemyLavaIgnitorHealPoint.healPoints.Count; ++index)
    {
      EnemyLavaIgnitorHealPoint healPoint = EnemyLavaIgnitorHealPoint.healPoints[index];
      if (!((UnityEngine.Object) healPoint == (UnityEngine.Object) null) && healPoint.isAttachedToFireTrap == this.prioritiseTraps)
      {
        float num2 = Vector3.Distance(position, healPoint.transform.position);
        Vector2 vector2 = new Vector2(healPoint.transform.position.x, healPoint.transform.position.y);
        Vector2 b = new Vector2(position.x, position.y);
        Vector2 normalized = (b - vector2).normalized;
        float distance = Vector2.Distance(vector2, b);
        if (!(bool) Physics2D.Raycast(vector2, normalized, distance, LayerMask.GetMask("Island", "Obstacles")) && (double) num2 < (double) num1)
        {
          num1 = num2;
          bestHealPoint = healPoint;
        }
      }
    }
    if ((UnityEngine.Object) bestHealPoint == (UnityEngine.Object) null)
    {
      float num3 = float.MaxValue;
      for (int index = 0; index < EnemyLavaIgnitorHealPoint.healPoints.Count; ++index)
      {
        EnemyLavaIgnitorHealPoint healPoint = EnemyLavaIgnitorHealPoint.healPoints[index];
        if (!((UnityEngine.Object) healPoint == (UnityEngine.Object) null))
        {
          float num4 = Vector3.Distance(position, healPoint.transform.position);
          Vector2 vector2 = new Vector2(healPoint.transform.position.x, healPoint.transform.position.y);
          Vector2 b = new Vector2(position.x, position.y);
          Vector2 normalized = (b - vector2).normalized;
          float distance = Vector2.Distance(vector2, b);
          if (!(bool) Physics2D.Raycast(vector2, normalized, distance, LayerMask.GetMask("Island", "Obstacles")) && (double) num4 < (double) num3)
          {
            num3 = num4;
            bestHealPoint = healPoint;
          }
        }
      }
    }
    return bestHealPoint;
  }

  public void SpawnTrapsAroundPoint(Vector3 center)
  {
    for (int index = 0; index < this.numberOfTraps; ++index)
    {
      Vector2 vector2 = UnityEngine.Random.insideUnitCircle * this.spawnRadius;
      GameObject gameObject = this.SlamTrapPrefab.Spawn(this.transform.parent, new Vector3(center.x + vector2.x, center.y + vector2.y, center.z), Quaternion.identity);
      gameObject.SetActive(true);
      gameObject.transform.localScale = Vector3.zero;
      gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      TrapLava component = gameObject.GetComponent<TrapLava>();
      gameObject.transform.DOScale(Vector3.zero, component.GetDespawnDuration * 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(component.GetLifeTime);
    }
  }

  public void SpawnTrapOnWalk(Vector3 spawnPos)
  {
    GameObject trap = this.SlamTrapPrefab.Spawn(this.transform.parent, spawnPos, Quaternion.identity);
    trap.SetActive(true);
    trap.transform.position = spawnPos;
    trap.transform.localScale = Vector3.zero;
    trap.transform.DOScale(Vector3.one * 0.5f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    TrapLava component = trap.GetComponent<TrapLava>();
    trap.transform.DOScale(Vector3.zero, component.GetDespawnDuration * 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(component.GetLifeTime * 0.5f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => trap.Recycle()));
  }

  public IEnumerator DyingState()
  {
    EnemyLavaIgnitor enemyLavaIgnitor = this;
    float progress = 0.0f;
    float signpostDuration = 1f;
    bool hasExploded = false;
    while ((double) progress < (double) signpostDuration)
    {
      progress += Time.deltaTime;
      yield return (object) null;
    }
    if (!hasExploded)
    {
      hasExploded = true;
      enemyLavaIgnitor.health.DealDamage(float.PositiveInfinity, (GameObject) null, Vector3.zero, AttackType: Health.AttackTypes.Projectile, dealDamageImmediately: true);
    }
  }

  [CompilerGenerated]
  public void \u003CcheckFireStatusFromHit\u003Eb__55_0()
  {
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null) || (double) this.health.HP < 0.0)
      return;
    this.HealUnit();
    if (this.onFire)
      return;
    this.StartState(this.IdleState());
    this.SetOnFire();
  }

  [CompilerGenerated]
  public void \u003COnHit\u003Eb__57_0() => this.Spine.transform.localScale = Vector3.one;
}
