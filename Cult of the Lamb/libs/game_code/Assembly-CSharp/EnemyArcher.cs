// Decompiled with JetBrains decompiler
// Type: EnemyArcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyArcher : UnitObject, IAttackResilient
{
  public static float LastArcherShot = float.MinValue;
  public float GlobalShotDelay = 5f;
  public int ShotsToFire = 1;
  public int ShotsWavesToFire = 1;
  public float DelayBetweenWaves = 0.2f;
  public float DelayReaiming = 0.5f;
  public float shotConeAngle;
  public bool CrossbowShots;
  public GameObject Arrow;
  public GameObject TargetObject;
  public SkeletonAnimation Spine;
  public int DistanceFromTargetPosition = 3;
  public SimpleSpineFlash SimpleSpineFlash;
  public SpriteRenderer Aiming;
  public ColliderEvents damageColliderEvents;
  public bool canBeParried;
  public static float signPostParryWindow = 0.2f;
  public static float attackParryWindow = 0.15f;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackChargeAnimation = "archer-attack-charge";
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackImpactAnimation = "archer-attack-impact";
  [SerializeField]
  public bool dodgeOnHit;
  [SerializeField]
  public float dodgeMultiplier = 1f;
  [SerializeField]
  public float dodgeDuration = 1f;
  [SerializeField]
  public float dodgeCooldown = 1f;
  [SerializeField]
  public SkeletonGhost dodgeGhost;
  public float dodgedTimestamp;
  public bool canDodgeOnHit;
  public ParticleSystem spawnParticles;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string shootSoundPath = string.Empty;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string MeleeWarningVO = "event:/dlc/dungeon05/enemy/wolf_crossbow/melee_warning";
  [EventRef]
  public string DashSFX = string.Empty;
  [EventRef]
  public string BowLoadSFX = "event:/dlc/dungeon06/enemy/archer_mutated/ranged_warning";
  [EventRef]
  public string RollSFX;
  public Coroutine dodgeCoroutine;
  public Coroutine cWaitForTarget;
  public float KnockbackMultiplier = 1f;
  public float ShootDelay;
  public float TeleportDelay;
  public float Angle;
  public float RandomChangeAngle = 3f;
  public Vector3 TargetPosition;
  public int MaintainDistance = 3;
  public float RepathTimer;
  public EnemyArcher.State MyState;
  public float CloseCombatCooldown;
  public float SignPostCloseCombatDelay = 1f;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public int AcceptableMove = 2;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Awake()
  {
    base.Awake();
    if (!((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null))
      return;
    this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
  }

  public void Start()
  {
    this.SeperateObject = true;
    this.canDodgeOnHit = this.dodgeOnHit;
    this.Aiming.gameObject.SetActive(false);
  }

  public void DoWaitForTarget()
  {
    if (this.cWaitForTarget != null)
      this.StopCoroutine(this.cWaitForTarget);
    this.cWaitForTarget = this.StartCoroutine((IEnumerator) this.WaitForTarget());
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.DoWaitForTarget();
    this.health.OnHitEarly += new Health.HitAction(((UnitObject) this).OnHitEarly);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.Aiming.gameObject.SetActive(false);
    this.ClearPaths();
    this.InternalStopAllCoroutines();
    this.DisableForces = false;
    this.SimpleSpineFlash.FlashWhite(false);
    if ((UnityEngine.Object) this.dodgeGhost != (UnityEngine.Object) null)
      this.dodgeGhost.ghostingEnabled = false;
    this.health.invincible = false;
    this.health.OnHitEarly -= new Health.HitAction(((UnitObject) this).OnHitEarly);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    if (!string.IsNullOrEmpty(this.WarningVO))
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
    this.TargetObject = TargetObject;
    this.DoWaitForTarget();
  }

  public override void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.canBeParried && !FromBehind && AttackType == Health.AttackTypes.Melee)
    {
      this.health.WasJustParried = true;
      this.Aiming.gameObject.SetActive(false);
      this.SimpleSpineFlash.FlashWhite(false);
      this.InternalStopAllCoroutines();
      this.DisableForces = false;
    }
    if (!this.dodgeOnHit || !this.canDodgeOnHit || (double) Time.time <= (double) this.dodgedTimestamp || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack || !((UnityEngine.Object) Attacker.GetComponent<PlayerFarming>() != (UnityEngine.Object) null))
      return;
    this.InternalStopAllCoroutines();
    this.dodgeCoroutine = this.StartCoroutine((IEnumerator) this.DodgeOnHitIE(Attacker));
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.health.WasJustParried)
      return;
    this.Aiming.gameObject.SetActive(false);
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnHit(Attacker, AttackLocation, AttackType);
    this.SimpleSpineFlash.FlashFillRed();
    this.TargetObject = (GameObject) null;
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.Spine.AnimationState.SetAnimation(1, "hurt-eyes", false);
    if (this.MyState != EnemyArcher.State.Shooting)
    {
      switch (AttackType)
      {
        case Health.AttackTypes.Heavy:
          goto label_11;
        case Health.AttackTypes.Projectile:
          if (!this.health.HasShield)
            goto label_11;
          break;
      }
      if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
        this.state.CURRENT_STATE = StateMachine.State.HitRight;
      if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
        this.state.CURRENT_STATE = StateMachine.State.HitLeft;
      this.InternalStopAllCoroutines();
      this.DisableForces = false;
      this.StartCoroutine((IEnumerator) this.HurtRoutine());
    }
label_11:
    if (AttackType == Health.AttackTypes.Projectile && !this.health.HasShield)
    {
      this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
      this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    else
      this.DoKnockBack(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), this.KnockbackMultiplier, 0.5f);
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
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public IEnumerator HurtRoutine()
  {
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < 0.5)
      yield return (object) null;
    this.DoWaitForTarget();
    this.ShootDelay = 0.0f;
    if ((double) this.TeleportDelay < 0.0 && (double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
      this.Teleport();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyArcher enemyArcher = this;
    if ((UnityEngine.Object) enemyArcher.damageColliderEvents != (UnityEngine.Object) null)
      enemyArcher.damageColliderEvents.SetActive(false);
    enemyArcher.RepathTimer = 2f;
    while ((UnityEngine.Object) enemyArcher.TargetObject == (UnityEngine.Object) null)
    {
      Health closestTarget = enemyArcher.GetClosestTarget();
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemyArcher.TargetObject = closestTarget.gameObject;
        enemyArcher.VisionRange = int.MaxValue;
      }
      enemyArcher.RepathTimer -= Time.deltaTime * enemyArcher.Spine.timeScale;
      if ((double) enemyArcher.RepathTimer <= 0.0)
      {
        if (enemyArcher.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyArcher.Spine.AnimationName != "run")
            enemyArcher.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemyArcher.Spine.AnimationName != "idle")
          enemyArcher.Spine.AnimationState.SetAnimation(0, "idle", true);
        enemyArcher.TargetPosition = enemyArcher.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        enemyArcher.FindPath(enemyArcher.TargetPosition);
        if (!PlayerRelic.TimeFrozen)
        {
          enemyArcher.state.LookAngle = Utils.GetAngle(enemyArcher.transform.position, enemyArcher.TargetPosition);
          enemyArcher.Spine.skeleton.ScaleX = (double) enemyArcher.state.LookAngle <= 90.0 || (double) enemyArcher.state.LookAngle >= 270.0 ? -1f : 1f;
        }
      }
      yield return (object) null;
    }
    float LineOfSightLostTimer = 1f;
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemyArcher.TargetObject == (UnityEngine.Object) null)
      {
        enemyArcher.DoWaitForTarget();
        yield break;
      }
      float a = Vector3.Distance(enemyArcher.TargetObject.transform.position, enemyArcher.transform.position);
      bool flag = enemyArcher.CheckLineOfSightOnTarget(enemyArcher.TargetObject, enemyArcher.TargetObject.transform.position, Mathf.Min(a, (float) enemyArcher.VisionRange));
      if ((double) a <= (double) enemyArcher.VisionRange & flag)
        InRange = true;
      else if (!flag)
      {
        LineOfSightLostTimer -= Time.deltaTime * enemyArcher.Spine.timeScale;
        if ((double) LineOfSightLostTimer <= 0.0)
        {
          enemyArcher.PreventEndlessIdleState();
          yield break;
        }
      }
      yield return (object) null;
    }
    enemyArcher.StartCoroutine((IEnumerator) enemyArcher.ChasePlayer());
  }

  public void PreventEndlessIdleState()
  {
    this.ClearPaths();
    this.InternalStopAllCoroutines();
    this.state.LookAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
    this.StartCoroutine((IEnumerator) this.ShootArrowRoutine(1f, 2f));
  }

  public IEnumerator ChasePlayer()
  {
    EnemyArcher enemyArcher = this;
    if ((UnityEngine.Object) enemyArcher.damageColliderEvents != (UnityEngine.Object) null)
      enemyArcher.damageColliderEvents.SetActive(false);
    enemyArcher.MyState = EnemyArcher.State.Idle;
    bool Loop = true;
    enemyArcher.Angle = Utils.GetAngle(enemyArcher.TargetObject.transform.position, enemyArcher.transform.position) * ((float) Math.PI / 180f);
    enemyArcher.TargetPosition = enemyArcher.TargetObject.transform.position + new Vector3((float) enemyArcher.MaintainDistance * Mathf.Cos(enemyArcher.Angle), (float) enemyArcher.MaintainDistance * Mathf.Sin(enemyArcher.Angle));
    while (Loop)
    {
      if (enemyArcher.MyState == EnemyArcher.State.Idle)
      {
        if ((UnityEngine.Object) enemyArcher.damageColliderEvents != (UnityEngine.Object) null)
          enemyArcher.damageColliderEvents.SetActive(false);
        if ((UnityEngine.Object) enemyArcher.TargetObject == (UnityEngine.Object) null)
        {
          enemyArcher.DoWaitForTarget();
          break;
        }
        if ((double) (enemyArcher.CloseCombatCooldown -= Time.deltaTime * enemyArcher.Spine.timeScale) < 0.0 && (double) Vector3.Distance(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position) < 2.0)
        {
          enemyArcher.StartCoroutine((IEnumerator) enemyArcher.CloseCombatAttack());
          break;
        }
        if (enemyArcher.state.CURRENT_STATE == StateMachine.State.Moving)
        {
          if (enemyArcher.Spine.AnimationName != "run")
            enemyArcher.Spine.AnimationState.SetAnimation(0, "run", true);
        }
        else if (enemyArcher.Spine.AnimationName != "idle")
          enemyArcher.Spine.AnimationState.SetAnimation(0, "idle", true);
        enemyArcher.TeleportDelay -= Time.deltaTime * enemyArcher.Spine.timeScale;
        if (!PlayerRelic.TimeFrozen)
        {
          enemyArcher.state.LookAngle = Utils.GetAngle(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position);
          enemyArcher.Spine.skeleton.ScaleX = (double) enemyArcher.state.LookAngle <= 90.0 || (double) enemyArcher.state.LookAngle >= 270.0 ? -1f : 1f;
        }
        if ((double) (enemyArcher.RepathTimer -= Time.deltaTime * enemyArcher.Spine.timeScale) < 0.0)
          enemyArcher.TargetPosition = enemyArcher.TargetObject.transform.position + new Vector3((float) enemyArcher.MaintainDistance * Mathf.Cos(enemyArcher.Angle), (float) enemyArcher.MaintainDistance * Mathf.Sin(enemyArcher.Angle));
        if ((double) Vector3.Distance(enemyArcher.TargetPosition, enemyArcher.transform.position) > (double) enemyArcher.DistanceFromTargetPosition && Time.frameCount % 5 == 0)
          enemyArcher.FindPath(enemyArcher.TargetPosition);
        if ((double) (enemyArcher.ShootDelay -= Time.deltaTime * enemyArcher.Spine.timeScale) < 0.0 && (double) Vector3.Distance(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position) < 8.0 && (double) GameManager.GetInstance().CurrentTime > ((double) EnemyArcher.LastArcherShot + (double) enemyArcher.GlobalShotDelay) / (double) enemyArcher.Spine.timeScale)
        {
          DataManager.Instance.LastArcherShot = TimeManager.TotalElapsedGameTime;
          EnemyArcher.LastArcherShot = GameManager.GetInstance().CurrentTime;
          enemyArcher.StartCoroutine((IEnumerator) enemyArcher.ShootArrowRoutine());
          break;
        }
      }
      yield return (object) null;
    }
  }

  public IEnumerator CloseCombatAttack()
  {
    EnemyArcher enemyArcher = this;
    Debug.Log((object) "CloseCombatAttack()".Colour(Color.red));
    enemyArcher.ClearPaths();
    enemyArcher.MyState = EnemyArcher.State.CloseCombatAttack;
    enemyArcher.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float Progress = 0.0f;
    if (!string.IsNullOrEmpty(enemyArcher.MeleeWarningVO))
      AudioManager.Instance.PlayOneShot(enemyArcher.MeleeWarningVO);
    enemyArcher.Spine.AnimationState.SetAnimation(0, "grunt-attack-charge2", false);
    enemyArcher.state.facingAngle = enemyArcher.state.LookAngle = Utils.GetAngle(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position);
    enemyArcher.Spine.skeleton.ScaleX = (double) enemyArcher.state.LookAngle <= 90.0 || (double) enemyArcher.state.LookAngle >= 270.0 ? -1f : 1f;
    while ((double) (Progress += Time.deltaTime * enemyArcher.Spine.timeScale) < (double) enemyArcher.SignPostCloseCombatDelay)
    {
      if ((double) Progress >= (double) enemyArcher.SignPostCloseCombatDelay - (double) EnemyArcher.signPostParryWindow)
        enemyArcher.canBeParried = true;
      enemyArcher.SimpleSpineFlash.FlashWhite(Progress / enemyArcher.SignPostCloseCombatDelay);
      yield return (object) null;
    }
    enemyArcher.speed = 0.2f;
    enemyArcher.SimpleSpineFlash.FlashWhite(false);
    enemyArcher.Spine.AnimationState.SetAnimation(0, "grunt-attack-impact2", false);
    if (!string.IsNullOrEmpty(enemyArcher.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyArcher.AttackVO, enemyArcher.transform.position);
    if (!string.IsNullOrEmpty(enemyArcher.attackSoundPath))
      AudioManager.Instance.PlayOneShot(enemyArcher.attackSoundPath, enemyArcher.transform.position);
    Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.deltaTime * enemyArcher.Spine.timeScale) < (double) Duration)
    {
      if ((UnityEngine.Object) enemyArcher.damageColliderEvents != (UnityEngine.Object) null)
        enemyArcher.damageColliderEvents.SetActive(true);
      enemyArcher.canBeParried = (double) Progress <= (double) EnemyArcher.attackParryWindow;
      yield return (object) null;
    }
    if ((UnityEngine.Object) enemyArcher.damageColliderEvents != (UnityEngine.Object) null)
      enemyArcher.damageColliderEvents.SetActive(false);
    enemyArcher.canBeParried = false;
    yield return (object) new WaitForSeconds(0.8f);
    enemyArcher.CloseCombatCooldown = 1f;
    enemyArcher.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcher.DoWaitForTarget();
  }

  public IEnumerator ShootArrowRoutine(float minDelay = 3f, float maxDelay = 4f, bool hasCharge = true)
  {
    EnemyArcher enemyArcher = this;
    enemyArcher.ClearPaths();
    enemyArcher.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enemyArcher.MyState = EnemyArcher.State.Shooting;
    enemyArcher.ShootDelay = UnityEngine.Random.Range(minDelay, maxDelay);
    yield return (object) null;
    if (!string.IsNullOrEmpty(enemyArcher.BowLoadSFX))
      AudioManager.Instance.PlayOneShot(enemyArcher.BowLoadSFX, enemyArcher.gameObject);
    if (hasCharge)
    {
      enemyArcher.Spine.AnimationState.SetAnimation(0, enemyArcher.attackChargeAnimation, false);
      enemyArcher.Aiming.gameObject.SetActive(true);
      float Progress = 0.0f;
      while ((double) (Progress += Time.deltaTime * enemyArcher.Spine.timeScale) < 1.0)
      {
        enemyArcher.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyArcher.state.LookAngle);
        enemyArcher.SimpleSpineFlash?.FlashWhite(Progress / 1f);
        if (Time.frameCount % 5 == 0)
          enemyArcher.Aiming.color = enemyArcher.Aiming.color == Color.red ? Color.white : Color.red;
        yield return (object) null;
      }
    }
    enemyArcher.SimpleSpineFlash.FlashWhite(false);
    enemyArcher.Aiming.gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(enemyArcher.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyArcher.AttackVO, enemyArcher.transform.position);
    float time = 0.0f;
    if ((UnityEngine.Object) enemyArcher.TargetObject != (UnityEngine.Object) null)
    {
      enemyArcher.AimAtTarget();
      for (int x = 0; x < enemyArcher.ShotsWavesToFire; ++x)
      {
        for (int burstArrowIndex = 0; burstArrowIndex < enemyArcher.ShotsToFire; ++burstArrowIndex)
        {
          float angle = (enemyArcher.ShotsToFire % 2 == 0 ? 1 : 0) == 0 ? enemyArcher.GetOddArrowAngle(burstArrowIndex, enemyArcher.ShotsToFire, enemyArcher.state.LookAngle, enemyArcher.shotConeAngle) : (float) ((double) enemyArcher.state.LookAngle - (double) enemyArcher.shotConeAngle * 0.5 + (double) burstArrowIndex * ((double) enemyArcher.shotConeAngle / (double) enemyArcher.ShotsToFire));
          enemyArcher.ShootSingleArrow(angle);
        }
        if (enemyArcher.ShotsWavesToFire > 1)
        {
          time = 0.0f;
          while ((double) (time += Time.deltaTime * enemyArcher.Spine.timeScale) < (double) enemyArcher.DelayBetweenWaves)
            yield return (object) null;
          if ((UnityEngine.Object) enemyArcher.TargetObject != (UnityEngine.Object) null)
          {
            enemyArcher.AimAtTarget();
            yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyArcher.DelayReaiming, enemyArcher.Spine);
          }
          else
          {
            Health closestTarget = enemyArcher.GetClosestTarget();
            if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
            {
              enemyArcher.TargetObject = closestTarget.gameObject;
              enemyArcher.AimAtTarget();
              yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyArcher.DelayReaiming, enemyArcher.Spine);
            }
          }
        }
      }
    }
    enemyArcher.Aiming.gameObject.SetActive(false);
    enemyArcher.TargetObject = (GameObject) null;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyArcher.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyArcher.MyState = EnemyArcher.State.Idle;
    enemyArcher.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcher.DoWaitForTarget();
  }

  public float GetOddArrowAngle(
    int burstArrowIndex,
    int shotsToFire,
    float lookAngle,
    float coneAngle)
  {
    return shotsToFire == 1 ? lookAngle : lookAngle + (float) (((double) burstArrowIndex - (double) (shotsToFire - 1) / 2.0) * ((double) coneAngle / (double) (shotsToFire - 1)));
  }

  public void AimAtTarget()
  {
    if (!((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null))
      return;
    this.state.LookAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
    this.Spine.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    this.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.state.LookAngle);
  }

  public void ShootSingleArrow(float angle)
  {
    if (!string.IsNullOrEmpty(this.shootSoundPath))
      AudioManager.Instance.PlayOneShot(this.shootSoundPath, this.transform.position);
    CameraManager.shakeCamera(0.2f, this.state.LookAngle);
    this.Spine.AnimationState.SetAnimation(0, this.attackImpactAnimation, false);
    Projectile component = ObjectPool.Spawn(this.Arrow, this.transform.parent).GetComponent<Projectile>();
    component.transform.position = this.transform.position + new Vector3(0.5f * Mathf.Cos(this.state.LookAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(this.state.LookAngle * ((float) Math.PI / 180f)));
    component.Angle = angle;
    component.team = this.health.team;
    component.Owner = this.health;
    if (!(bool) (UnityEngine.Object) component.health)
      return;
    component.health.team = this.health.team;
  }

  public IEnumerator TeleportRoutine(Vector3 Position)
  {
    EnemyArcher enemyArcher = this;
    enemyArcher.ClearPaths();
    enemyArcher.state.CURRENT_STATE = StateMachine.State.Moving;
    enemyArcher.UsePathing = false;
    enemyArcher.health.invincible = true;
    enemyArcher.SeperateObject = false;
    enemyArcher.MyState = EnemyArcher.State.Teleporting;
    enemyArcher.ClearPaths();
    enemyArcher.ShootDelay = 1f;
    Vector3 position = enemyArcher.transform.position;
    float Progress = 0.0f;
    enemyArcher.Spine.AnimationState.SetAnimation(0, "roll", true);
    enemyArcher.state.facingAngle = enemyArcher.state.LookAngle = Utils.GetAngle(enemyArcher.transform.position, Position);
    enemyArcher.Spine.skeleton.ScaleX = (double) enemyArcher.state.LookAngle <= 90.0 || (double) enemyArcher.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 b = Position;
    float Duration = Vector3.Distance(position, b) / 12f;
    if (!string.IsNullOrEmpty(enemyArcher.RollSFX))
      AudioManager.Instance.PlayOneShot(enemyArcher.RollSFX, enemyArcher.gameObject);
    while ((double) (Progress += Time.deltaTime * enemyArcher.Spine.timeScale) < (double) Duration)
    {
      enemyArcher.speed = 10f * Time.deltaTime * enemyArcher.Spine.timeScale;
      yield return (object) null;
    }
    enemyArcher.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcher.Spine.AnimationState.SetAnimation(0, "roll-stop", false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyArcher.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyArcher.UsePathing = true;
    enemyArcher.SeperateObject = true;
    enemyArcher.TargetPosition = Position;
    enemyArcher.RepathTimer = 0.0f;
    enemyArcher.ShootDelay = UnityEngine.Random.Range(0.0f, 1f);
    enemyArcher.health.invincible = false;
    enemyArcher.TeleportDelay = 1f;
    enemyArcher.MyState = EnemyArcher.State.Idle;
    double num = (double) Vector3.Distance(enemyArcher.TargetObject.transform.position, enemyArcher.transform.position);
    if (!enemyArcher.CheckLineOfSightOnTarget(enemyArcher.TargetObject, enemyArcher.TargetObject.transform.position, Mathf.Min(5f, (float) enemyArcher.VisionRange)))
      enemyArcher.PreventEndlessIdleState();
  }

  public void FindPath(Vector3 PointToCheck)
  {
    if (this.MyState == EnemyArcher.State.Teleporting)
      return;
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
    {
      this.givePath(this.TargetPosition);
      this.RepathTimer = 2f;
    }
    else
    {
      this.Angle = Utils.GetAngle(this.TargetObject.transform.position, this.transform.position) * ((float) Math.PI / 180f);
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 1f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.MaintainDistance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) > (double) this.AcceptableMove)
        {
          if ((double) this.TeleportDelay > 0.0 || UnityEngine.Random.Range(0, 2) == 0)
          {
            if (this.ShowDebug)
            {
              this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset);
              this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
            }
            this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset;
            this.givePath(this.TargetPosition);
            this.RepathTimer = 2f;
          }
          else
            this.Teleport();
        }
        else if ((double) this.TeleportDelay < 0.0 && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < 2.0)
        {
          this.Teleport();
        }
        else
        {
          if ((double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) <= 5.0)
            return;
          if ((double) this.TeleportDelay > 0.0 || UnityEngine.Random.Range(0, 2) == 0)
          {
            this.TargetPosition = Vector3.Lerp(this.transform.position, this.TargetObject.transform.position, 0.5f);
            this.givePath(this.TargetPosition);
            this.RepathTimer = 2f;
          }
          else
            this.Teleport();
        }
      }
      else
      {
        this.TargetPosition = PointToCheck;
        this.givePath(PointToCheck);
        this.RepathTimer = 2f;
      }
    }
  }

  public void Teleport()
  {
    if (this.MyState != EnemyArcher.State.Idle)
      return;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    float num1 = 100f;
    float num2;
    if ((double) (num2 = num1 - 1f) <= 0.0)
      return;
    float f = (float) (((double) Utils.GetAngle(this.transform.position, this.TargetObject.transform.position) + (double) UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
    float distance = 4f;
    float radius = 1f;
    Vector3 Position = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(Position - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - Position) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.StartCoroutine((IEnumerator) this.TeleportRoutine((Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - Position) * this.CircleCastOffset));
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

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
    Utils.DrawCircleXY(this.TargetPosition, (float) this.DistanceFromTargetPosition, Color.red);
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

  public void GraveSpawn()
  {
    this.InternalStopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.GraveSpawnRoutine());
  }

  public IEnumerator GraveSpawnRoutine()
  {
    EnemyArcher enemyArcher = this;
    enemyArcher.health.invincible = true;
    enemyArcher.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) enemyArcher.spawnParticles != (UnityEngine.Object) null)
      enemyArcher.spawnParticles.Play();
    enemyArcher.Spine.AnimationState.SetAnimation(0, "grave-spawn-long", false);
    enemyArcher.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(2.33f);
    enemyArcher.health.invincible = false;
    enemyArcher.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcher.StartCoroutine((IEnumerator) enemyArcher.WaitForTarget());
  }

  public IEnumerator DodgeOnHitIE(GameObject attacker)
  {
    EnemyArcher enemyArcher = this;
    if (!string.IsNullOrEmpty(enemyArcher.DashSFX))
      AudioManager.Instance.PlayOneShot(enemyArcher.DashSFX, enemyArcher.gameObject);
    enemyArcher.TargetObject = attacker;
    enemyArcher.AimAtTarget();
    enemyArcher.health.invincible = true;
    enemyArcher.DoKnockBack(attacker, enemyArcher.dodgeMultiplier, enemyArcher.dodgeDuration, false);
    enemyArcher.Spine.AnimationState.SetAnimation(0, "berserk-bite-impact", false);
    enemyArcher.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    enemyArcher.dodgeGhost.ghostingEnabled = true;
    enemyArcher.dodgedTimestamp = Time.time + enemyArcher.dodgeCooldown;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyArcher.Spine.timeScale) < (double) enemyArcher.dodgeDuration)
    {
      enemyArcher.SimpleSpineFlash.FlashMeWhite(Mathf.Lerp(0.0f, 0.8f, time / enemyArcher.dodgeDuration));
      yield return (object) null;
    }
    enemyArcher.StartCoroutine((IEnumerator) enemyArcher.ShootArrowRoutine(hasCharge: false));
    enemyArcher.CleanupDodgeCoroutine();
  }

  public void ResetResilience() => this.canDodgeOnHit = this.dodgeOnHit;

  public void StopResilience() => this.canDodgeOnHit = false;

  public void InternalStopAllCoroutines()
  {
    if (this.dodgeCoroutine != null)
    {
      this.StopCoroutine(this.dodgeCoroutine);
      this.CleanupDodgeCoroutine();
    }
    this.StopAllCoroutines();
  }

  public void CleanupDodgeCoroutine()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    this.dodgeGhost.ghostingEnabled = false;
    this.dodgeCoroutine = (Coroutine) null;
  }

  public enum State
  {
    Idle,
    Shooting,
    Teleporting,
    CloseCombatAttack,
  }
}
