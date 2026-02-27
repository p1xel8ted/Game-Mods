// Decompiled with JetBrains decompiler
// Type: EnemyArcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyArcher : UnitObject
{
  public float GlobalShotDelay = 5f;
  public int ShotsToFire = 1;
  public float DelayBetweenShots = 0.2f;
  public float DelayReaiming = 0.5f;
  public GameObject Arrow;
  private GameObject TargetObject;
  public SkeletonAnimation Spine;
  public int DistanceFromTargetPosition = 3;
  public SimpleSpineFlash SimpleSpineFlash;
  public SpriteRenderer Aiming;
  public ColliderEvents damageColliderEvents;
  private bool canBeParried;
  private static float signPostParryWindow = 0.2f;
  private static float attackParryWindow = 0.15f;
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
  private Coroutine cWaitForTarget;
  public float KnockbackMultiplier = 1f;
  private float ShootDelay;
  private float TeleportDelay;
  private float Angle;
  private float RandomChangeAngle = 3f;
  private Vector3 TargetPosition;
  public int MaintainDistance = 3;
  private float RepathTimer;
  private EnemyArcher.State MyState;
  private float CloseCombatCooldown;
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

  private void Start()
  {
    this.SeperateObject = true;
    this.Aiming.gameObject.SetActive(false);
  }

  private void DoWaitForTarget()
  {
    if (this.cWaitForTarget != null)
      this.StopCoroutine(this.cWaitForTarget);
    this.cWaitForTarget = this.StartCoroutine((IEnumerator) this.WaitForTarget());
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.DoWaitForTarget();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
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
    this.StopAllCoroutines();
    this.DisableForces = false;
    this.SimpleSpineFlash.FlashWhite(false);
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
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

  private void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.canBeParried || FromBehind || AttackType != Health.AttackTypes.Melee)
      return;
    this.health.WasJustParried = true;
    this.Aiming.gameObject.SetActive(false);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StopAllCoroutines();
    this.DisableForces = false;
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
      this.StopAllCoroutines();
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

  private IEnumerator HurtRoutine()
  {
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    yield return (object) new WaitForSeconds(0.5f);
    this.DoWaitForTarget();
    this.ShootDelay = 0.0f;
    if ((double) this.TeleportDelay < 0.0 && (double) UnityEngine.Random.Range(0.0f, 1f) < 0.5)
      this.Teleport();
  }

  private IEnumerator WaitForTarget()
  {
    EnemyArcher enemyArcher = this;
    if ((UnityEngine.Object) enemyArcher.damageColliderEvents != (UnityEngine.Object) null)
      enemyArcher.damageColliderEvents.SetActive(false);
    enemyArcher.RepathTimer = 2f;
    while ((UnityEngine.Object) enemyArcher.TargetObject == (UnityEngine.Object) null)
    {
      Debug.Log((object) "A");
      Health closestTarget = enemyArcher.GetClosestTarget();
      if ((bool) (UnityEngine.Object) closestTarget)
      {
        enemyArcher.TargetObject = closestTarget.gameObject;
        enemyArcher.VisionRange = int.MaxValue;
      }
      enemyArcher.RepathTimer -= Time.deltaTime;
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
        enemyArcher.state.LookAngle = Utils.GetAngle(enemyArcher.transform.position, enemyArcher.TargetPosition);
        enemyArcher.Spine.skeleton.ScaleX = (double) enemyArcher.state.LookAngle <= 90.0 || (double) enemyArcher.state.LookAngle >= 270.0 ? -1f : 1f;
      }
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemyArcher.TargetObject == (UnityEngine.Object) null)
      {
        enemyArcher.DoWaitForTarget();
        yield break;
      }
      float a = Vector3.Distance(enemyArcher.TargetObject.transform.position, enemyArcher.transform.position);
      if ((double) a <= (double) enemyArcher.VisionRange && enemyArcher.CheckLineOfSight(enemyArcher.TargetObject.transform.position, Mathf.Min(a, (float) enemyArcher.VisionRange)))
        InRange = true;
      yield return (object) null;
    }
    enemyArcher.StartCoroutine((IEnumerator) enemyArcher.ChasePlayer());
  }

  private IEnumerator ChasePlayer()
  {
    EnemyArcher enemyArcher = this;
    Debug.Log((object) "ChasePlayer()".Colour(Color.red));
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
        if ((double) (enemyArcher.CloseCombatCooldown -= Time.deltaTime) < 0.0 && (double) Vector3.Distance(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position) < 2.0)
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
        enemyArcher.TeleportDelay -= Time.deltaTime;
        enemyArcher.state.LookAngle = Utils.GetAngle(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position);
        enemyArcher.Spine.skeleton.ScaleX = (double) enemyArcher.state.LookAngle <= 90.0 || (double) enemyArcher.state.LookAngle >= 270.0 ? -1f : 1f;
        if ((double) (enemyArcher.RepathTimer -= Time.deltaTime) < 0.0)
          enemyArcher.TargetPosition = enemyArcher.TargetObject.transform.position + new Vector3((float) enemyArcher.MaintainDistance * Mathf.Cos(enemyArcher.Angle), (float) enemyArcher.MaintainDistance * Mathf.Sin(enemyArcher.Angle));
        if ((double) Vector3.Distance(enemyArcher.TargetPosition, enemyArcher.transform.position) > (double) enemyArcher.DistanceFromTargetPosition && Time.frameCount % 5 == 0)
          enemyArcher.FindPath(enemyArcher.TargetPosition);
        if ((double) (enemyArcher.ShootDelay -= Time.deltaTime) < 0.0 && (double) Vector3.Distance(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position) < 8.0 && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.LastArcherShot + (double) enemyArcher.GlobalShotDelay)
        {
          DataManager.Instance.LastArcherShot = TimeManager.TotalElapsedGameTime;
          enemyArcher.StartCoroutine((IEnumerator) enemyArcher.ShootArrowRoutine());
          break;
        }
      }
      yield return (object) null;
    }
  }

  private IEnumerator CloseCombatAttack()
  {
    EnemyArcher enemyArcher = this;
    Debug.Log((object) "CloseCombatAttack()".Colour(Color.red));
    enemyArcher.ClearPaths();
    enemyArcher.MyState = EnemyArcher.State.CloseCombatAttack;
    enemyArcher.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float Progress = 0.0f;
    enemyArcher.Spine.AnimationState.SetAnimation(0, "grunt-attack-charge2", false);
    enemyArcher.state.facingAngle = enemyArcher.state.LookAngle = Utils.GetAngle(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position);
    enemyArcher.Spine.skeleton.ScaleX = (double) enemyArcher.state.LookAngle <= 90.0 || (double) enemyArcher.state.LookAngle >= 270.0 ? -1f : 1f;
    while ((double) (Progress += Time.deltaTime) < (double) enemyArcher.SignPostCloseCombatDelay)
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
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
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

  private IEnumerator ShootArrowRoutine()
  {
    EnemyArcher enemyArcher = this;
    enemyArcher.ClearPaths();
    enemyArcher.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enemyArcher.MyState = EnemyArcher.State.Shooting;
    enemyArcher.ShootDelay = (float) UnityEngine.Random.Range(3, 4);
    yield return (object) null;
    enemyArcher.Spine.AnimationState.SetAnimation(0, "archer-attack-charge", false);
    enemyArcher.Aiming.gameObject.SetActive(true);
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < 1.0)
    {
      enemyArcher.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyArcher.state.LookAngle);
      enemyArcher.SimpleSpineFlash?.FlashWhite(Progress / 1f);
      if (Time.frameCount % 5 == 0)
        enemyArcher.Aiming.color = enemyArcher.Aiming.color == Color.red ? Color.white : Color.red;
      yield return (object) null;
    }
    enemyArcher.SimpleSpineFlash.FlashWhite(false);
    enemyArcher.Aiming.gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(enemyArcher.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyArcher.AttackVO, enemyArcher.transform.position);
    int i = enemyArcher.ShotsToFire;
    while (--i >= 0)
    {
      if (!string.IsNullOrEmpty(enemyArcher.shootSoundPath))
        AudioManager.Instance.PlayOneShot(enemyArcher.shootSoundPath, enemyArcher.transform.position);
      CameraManager.shakeCamera(0.2f, enemyArcher.state.LookAngle);
      Projectile component = ObjectPool.Spawn(enemyArcher.Arrow, enemyArcher.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyArcher.transform.position + new Vector3(0.5f * Mathf.Cos(enemyArcher.state.LookAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(enemyArcher.state.LookAngle * ((float) Math.PI / 180f)));
      component.Angle = enemyArcher.state.LookAngle;
      component.team = enemyArcher.health.team;
      component.Owner = enemyArcher.health;
      enemyArcher.Spine.AnimationState.SetAnimation(0, "archer-attack-impact", false);
      yield return (object) new WaitForSeconds(enemyArcher.DelayBetweenShots);
      if ((UnityEngine.Object) enemyArcher.TargetObject != (UnityEngine.Object) null && i > 0)
      {
        enemyArcher.Aiming.gameObject.SetActive(true);
        enemyArcher.state.LookAngle = Utils.GetAngle(enemyArcher.transform.position, enemyArcher.TargetObject.transform.position);
        enemyArcher.Spine.skeleton.ScaleX = (double) enemyArcher.state.LookAngle <= 90.0 || (double) enemyArcher.state.LookAngle >= 270.0 ? -1f : 1f;
        enemyArcher.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyArcher.state.LookAngle);
        yield return (object) new WaitForSeconds(enemyArcher.DelayReaiming);
      }
    }
    enemyArcher.Aiming.gameObject.SetActive(false);
    enemyArcher.TargetObject = (GameObject) null;
    yield return (object) new WaitForSeconds(0.3f);
    enemyArcher.MyState = EnemyArcher.State.Idle;
    enemyArcher.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcher.DoWaitForTarget();
  }

  private IEnumerator TeleportRoutine(Vector3 Position)
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
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      enemyArcher.speed = 10f * Time.deltaTime;
      yield return (object) null;
    }
    enemyArcher.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyArcher.Spine.AnimationState.SetAnimation(0, "roll-stop", false);
    yield return (object) new WaitForSeconds(0.3f);
    enemyArcher.UsePathing = true;
    enemyArcher.SeperateObject = true;
    enemyArcher.TargetPosition = Position;
    enemyArcher.RepathTimer = 0.0f;
    enemyArcher.ShootDelay = UnityEngine.Random.Range(0.0f, 1f);
    enemyArcher.health.invincible = false;
    enemyArcher.TeleportDelay = 1f;
    enemyArcher.MyState = EnemyArcher.State.Idle;
  }

  private void FindPath(Vector3 PointToCheck)
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

  private void Teleport()
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

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  private void OnDrawGizmos()
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

  private enum State
  {
    Idle,
    Shooting,
    Teleporting,
    CloseCombatAttack,
  }
}
