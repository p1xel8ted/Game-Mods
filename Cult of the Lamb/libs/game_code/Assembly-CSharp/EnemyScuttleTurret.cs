// Decompiled with JetBrains decompiler
// Type: EnemyScuttleTurret
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyScuttleTurret : UnitObject
{
  public SkeletonAnimation spine;
  [SerializeField]
  public SkeletonAnimation Warning;
  public SimpleSpineFlash SimpleSpineFlash;
  public GameObject TargetObject;
  public float RandomDirection;
  public float acceleration = 5f;
  public float turningSpeed = 1f;
  public float knockbackModifier = 0.75f;
  public bool isMiniBoss;
  public GameObject Arrow;
  public bool Anticipating;
  public bool Shooting;
  public float ShootDelay = float.MaxValue;
  public int ShotsToFire = 3;
  public float ShootDelayTime = 1f;
  public float anticipateDuration = 1f;
  public float TimeBetweenShooting = 1f;
  public SpriteRenderer Aiming;
  public float AngleArc = 90f;
  public GameObject SecondaryArrow;
  public int SecondaryShotsToFire = 3;
  public float SecondaryShootDelayTime = 1f;
  public float SecondaryTimeBetweenShooting = 1f;
  public float SecondaryAngleArc = 90f;
  public int numSecondaryAttacks;
  public int secondaryAttackCounter;
  public bool LimitTo45Degrees;
  public float KnockbackDuration = 0.5f;
  public float KnockbackForce = 1f;
  public float Angle;
  public Vector3 Force;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public Vector2 IdleWaitRange = new Vector2(1f, 3f);
  public float IdleWait;
  public bool ShownWarning;
  public Health EnemyHealth;
  public int DetectEnemyRange = 8;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  [SerializeField]
  public bool hasMegaAttack;
  [SerializeField]
  public float anticipationTime;
  [SerializeField]
  public Vector2 timeBetweenMegaAttacks;
  [SerializeField]
  public ProjectilePattern projectilePattern;
  public float lastMegaAttackTime;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Awake()
  {
    base.Awake();
    this.Preload();
  }

  public void Preload()
  {
    this.Arrow.CreatePool(this.ShotsToFire);
    this.SecondaryArrow.CreatePool(this.SecondaryShotsToFire);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.SeperateObject = true;
    this.RandomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    this.state.facingAngle = this.RandomDirection * 57.29578f;
    this.Aiming.gameObject.SetActive(false);
    this.Shooting = false;
    this.health.OnAddCharm += new Health.StasisEvent(this.GetNewTarget);
    this.health.OnStasisCleared += new Health.StasisEvent(this.GetNewTarget);
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
    this.secondaryAttackCounter = this.numSecondaryAttacks;
    this.speed = 0.0f;
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    this.lastMegaAttackTime = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.timeBetweenMegaAttacks.x, this.timeBetweenMegaAttacks.y);
  }

  public override void OnDisable()
  {
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.ClearPaths();
    this.StopAllCoroutines();
    this.health.OnAddCharm -= new Health.StasisEvent(this.GetNewTarget);
    this.health.OnStasisCleared -= new Health.StasisEvent(this.GetNewTarget);
  }

  public IEnumerator ShowWarning()
  {
    this.Warning.gameObject.SetActive(true);
    yield return (object) this.Warning.YieldForAnimation("warn");
    this.Warning.gameObject.SetActive(false);
  }

  public override void Update()
  {
    if (this.UsePathing)
    {
      if (this.pathToFollow == null)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime;
        this.move();
        return;
      }
      if (this.currentWaypoint >= this.pathToFollow.Count)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime;
        this.move();
        return;
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
    {
      this.speed += (float) (((double) this.maxSpeed * (double) this.SpeedMultiplier - (double) this.speed) / (7.0 * (double) this.acceleration)) * GameManager.DeltaTime;
      if (this.UsePathing)
      {
        this.state.facingAngle = Mathf.LerpAngle(this.state.facingAngle, Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]), Time.deltaTime * this.turningSpeed);
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.pathToFollow[this.currentWaypoint]) <= (double) this.StoppingDistance)
        {
          ++this.currentWaypoint;
          if (this.currentWaypoint == this.pathToFollow.Count)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            System.Action endOfPath = this.EndOfPath;
            if (endOfPath != null)
              endOfPath();
            this.pathToFollow = (List<Vector3>) null;
          }
        }
      }
    }
    else
      this.speed += (float) ((0.0 - (double) this.speed) / (4.0 * (double) this.acceleration)) * GameManager.DeltaTime;
    this.move();
  }

  public IEnumerator ActiveRoutine()
  {
    EnemyScuttleTurret enemyScuttleTurret = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyScuttleTurret.spine.timeScale) < 0.20000000298023224)
      yield return (object) null;
    if ((UnityEngine.Object) enemyScuttleTurret.spine != (UnityEngine.Object) null)
      enemyScuttleTurret.spine.AnimationState.SetAnimation(0, "animation", true);
    while (true)
    {
      if (enemyScuttleTurret.state.CURRENT_STATE == StateMachine.State.Idle)
      {
        double num = (double) (enemyScuttleTurret.IdleWait -= Time.deltaTime);
      }
      enemyScuttleTurret.GetNewTargetPosition();
      if (GameManager.RoomActive && (double) enemyScuttleTurret.ShootDelay == 3.4028234663852886E+38)
        enemyScuttleTurret.ShootDelay = enemyScuttleTurret.numSecondaryAttacks <= 0 || enemyScuttleTurret.secondaryAttackCounter <= 0 ? enemyScuttleTurret.ShootDelayTime : enemyScuttleTurret.SecondaryShootDelayTime;
      if ((UnityEngine.Object) enemyScuttleTurret.TargetObject == (UnityEngine.Object) null)
      {
        if (Time.frameCount % 10 == 0)
          enemyScuttleTurret.GetNewTarget();
      }
      else if (!enemyScuttleTurret.Shooting && GameManager.RoomActive)
      {
        enemyScuttleTurret.ShootDelay -= Time.deltaTime;
        if ((double) GameManager.GetInstance().CurrentTime > (double) enemyScuttleTurret.lastMegaAttackTime && (double) enemyScuttleTurret.ShootDelay <= (double) enemyScuttleTurret.anticipateDuration && !enemyScuttleTurret.Anticipating && enemyScuttleTurret.hasMegaAttack)
        {
          enemyScuttleTurret.StartCoroutine((IEnumerator) enemyScuttleTurret.MegaAttackIE());
        }
        else
        {
          if (!enemyScuttleTurret.Anticipating && (double) enemyScuttleTurret.ShootDelay <= (double) enemyScuttleTurret.anticipateDuration && (enemyScuttleTurret.secondaryAttackCounter < enemyScuttleTurret.numSecondaryAttacks || enemyScuttleTurret.numSecondaryAttacks == 0))
          {
            enemyScuttleTurret.Anticipating = true;
            if (enemyScuttleTurret.isMiniBoss)
              AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", enemyScuttleTurret.gameObject);
            else
              AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/warning", enemyScuttleTurret.gameObject);
            enemyScuttleTurret.spine.AnimationState.SetAnimation(0, "anticipate", true);
          }
          if (enemyScuttleTurret.Anticipating && (double) enemyScuttleTurret.ShootDelay <= (double) enemyScuttleTurret.anticipateDuration && (double) enemyScuttleTurret.ShootDelay > 0.0)
            enemyScuttleTurret.SimpleSpineFlash.FlashWhite(1f - Mathf.Clamp01(enemyScuttleTurret.ShootDelay / enemyScuttleTurret.anticipateDuration));
          if ((double) enemyScuttleTurret.ShootDelay <= 0.0)
          {
            enemyScuttleTurret.SimpleSpineFlash.FlashWhite(false);
            enemyScuttleTurret.Anticipating = false;
            enemyScuttleTurret.StartCoroutine((IEnumerator) enemyScuttleTurret.ShootArrowRoutine());
          }
        }
      }
      yield return (object) null;
    }
  }

  public override void FixedUpdate()
  {
    if ((double) this.spine.timeScale == 9.9999997473787516E-05)
      return;
    base.FixedUpdate();
  }

  public IEnumerator ShootArrowRoutine()
  {
    EnemyScuttleTurret enemyScuttleTurret = this;
    if (enemyScuttleTurret.isMiniBoss)
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/attack", enemyScuttleTurret.gameObject);
    else
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/attack", enemyScuttleTurret.gameObject);
    enemyScuttleTurret.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyScuttleTurret.IdleWait = 1f;
    enemyScuttleTurret.Shooting = true;
    bool secondaryAttack = false;
    if (enemyScuttleTurret.numSecondaryAttacks > 0 && UnityEngine.Random.Range(0, 2) == 0)
    {
      secondaryAttack = true;
      enemyScuttleTurret.secondaryAttackCounter = 1;
    }
    else
      enemyScuttleTurret.secondaryAttackCounter = 0;
    int _shotsToFire = secondaryAttack ? enemyScuttleTurret.SecondaryShotsToFire : enemyScuttleTurret.ShotsToFire;
    float _angleArc = secondaryAttack ? enemyScuttleTurret.SecondaryAngleArc : enemyScuttleTurret.AngleArc;
    float _timeBetweenShooting = secondaryAttack ? enemyScuttleTurret.SecondaryTimeBetweenShooting : enemyScuttleTurret.TimeBetweenShooting;
    int i = _shotsToFire;
    float aimingAngle = enemyScuttleTurret.state.LookAngle;
    if ((UnityEngine.Object) enemyScuttleTurret.TargetObject != (UnityEngine.Object) null)
      aimingAngle = Utils.GetAngle(enemyScuttleTurret.transform.position, enemyScuttleTurret.TargetObject.transform.position);
    if (enemyScuttleTurret.LimitTo45Degrees)
      aimingAngle = Mathf.Round(aimingAngle / 45f) * 45f;
    float KnockbackAngle = Mathf.Repeat(aimingAngle + 180f, 360f) * ((float) Math.PI / 180f);
    if ((UnityEngine.Object) enemyScuttleTurret.TargetObject != (UnityEngine.Object) null)
      KnockbackAngle = Utils.GetAngle(enemyScuttleTurret.TargetObject.transform.position, enemyScuttleTurret.transform.position) * ((float) Math.PI / 180f);
    aimingAngle -= _angleArc / 2f;
    while (--i >= 0)
    {
      if (!secondaryAttack && (double) _timeBetweenShooting > 0.10000000149011612)
        enemyScuttleTurret.Aiming.gameObject.SetActive(true);
      float Progress = 0.0f;
      while ((double) (Progress += Time.deltaTime) < (double) _timeBetweenShooting / (double) enemyScuttleTurret.spine.timeScale)
      {
        if (!secondaryAttack)
          enemyScuttleTurret.SimpleSpineFlash.FlashWhite(Mathf.Clamp01(Progress / _timeBetweenShooting) * 0.75f);
        enemyScuttleTurret.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, aimingAngle);
        if (Time.frameCount % 5 == 0)
          enemyScuttleTurret.Aiming.color = enemyScuttleTurret.Aiming.color == Color.red ? Color.white : Color.red;
        yield return (object) null;
      }
      enemyScuttleTurret.Aiming.gameObject.SetActive(false);
      enemyScuttleTurret.SimpleSpineFlash.FlashWhite(false);
      CameraManager.shakeCamera(0.2f, aimingAngle);
      Projectile component = ObjectPool.Spawn(secondaryAttack ? enemyScuttleTurret.SecondaryArrow : enemyScuttleTurret.Arrow, enemyScuttleTurret.transform.parent).GetComponent<Projectile>();
      AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", enemyScuttleTurret.transform.position);
      component.transform.position = enemyScuttleTurret.transform.position;
      component.Angle = aimingAngle;
      component.team = enemyScuttleTurret.health.team;
      component.Speed = 6f;
      component.Owner = enemyScuttleTurret.health;
      enemyScuttleTurret.spine.AnimationState.SetAnimation(0, "attack-shoot", false);
      enemyScuttleTurret.spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
      aimingAngle += _angleArc / (float) Mathf.Max(_shotsToFire, 0);
    }
    enemyScuttleTurret.TargetObject = (GameObject) null;
    enemyScuttleTurret.Shooting = false;
    enemyScuttleTurret.ShootDelay = enemyScuttleTurret.numSecondaryAttacks <= 0 || enemyScuttleTurret.secondaryAttackCounter <= 0 ? enemyScuttleTurret.ShootDelayTime : enemyScuttleTurret.SecondaryShootDelayTime;
    enemyScuttleTurret.DoKnockBack(KnockbackAngle, enemyScuttleTurret.KnockbackForce, enemyScuttleTurret.KnockbackDuration);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.isMiniBoss)
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/gethit", this.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/gethit", this.transform.position);
    this.Anticipating = false;
    this.StartCoroutine((IEnumerator) this.HurtRoutine(Attacker));
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (this.isMiniBoss)
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/death", this.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/death", this.transform.position);
  }

  public IEnumerator HurtRoutine(GameObject Attacker)
  {
    EnemyScuttleTurret enemyScuttleTurret = this;
    enemyScuttleTurret.ClearPaths();
    enemyScuttleTurret.state.CURRENT_STATE = StateMachine.State.KnockBack;
    enemyScuttleTurret.DisableForces = true;
    enemyScuttleTurret.Angle = Utils.GetAngle(Attacker.transform.position, enemyScuttleTurret.transform.position);
    enemyScuttleTurret.Force = (Vector3) new Vector2(25f * Mathf.Cos(enemyScuttleTurret.Angle * ((float) Math.PI / 180f)), 25f * Mathf.Sin(enemyScuttleTurret.Angle * ((float) Math.PI / 180f)));
    enemyScuttleTurret.rb.velocity = (Vector2) enemyScuttleTurret.Force;
    enemyScuttleTurret.SimpleSpineFlash.FlashFillRed();
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyScuttleTurret.spine.timeScale) < 0.5)
      yield return (object) null;
    enemyScuttleTurret.DisableForces = false;
    enemyScuttleTurret.IdleWait = 0.0f;
    enemyScuttleTurret.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void GetNewTargetPosition()
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      return;
    float num1 = 100f;
    while ((double) --num1 > 0.0)
    {
      float num2 = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      this.RandomDirection += (float) UnityEngine.Random.Range(-45, 45) * ((float) Math.PI / 180f);
      float radius = 0.1f;
      Vector3 targetLocation = this.transform.position + new Vector3(num2 * Mathf.Cos(this.RandomDirection), num2 * Mathf.Sin(this.RandomDirection));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), num2 * 0.5f, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if (this.ShowDebug)
        {
          this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - targetLocation) * this.CircleCastOffset);
          this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.RandomDirection += 0.17453292f;
      }
      else
      {
        if (this.ShowDebug)
        {
          this.EndPoints.Add(new Vector3(targetLocation.x, targetLocation.y));
          this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
        this.givePath(targetLocation);
        break;
      }
    }
  }

  public void GetNewTarget()
  {
    Health closestTarget = this.GetClosestTarget();
    if (!((UnityEngine.Object) closestTarget != (UnityEngine.Object) null))
      return;
    if (this.ShownWarning)
    {
      if (this.isMiniBoss)
        AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", this.gameObject);
      else
        AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/warning", this.gameObject);
      this.StartCoroutine((IEnumerator) this.ShowWarning());
      this.ShownWarning = true;
    }
    this.TargetObject = closestTarget.gameObject;
    this.EnemyHealth = closestTarget;
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if ((int) this.layerToCheck != ((int) this.layerToCheck | 1 << collision.gameObject.layer))
      return;
    if ((double) this.speed < (double) this.maxSpeed)
      this.speed *= 1.2f;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
    this.state.facingAngle = Utils.GetAngle((Vector3) (Vector2) this.transform.position, (Vector3) ((Vector2) this.transform.position + Vector2.Reflect(Utils.DegreeToVector2(this.state.facingAngle), collision.contacts[0].normal)));
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

  public IEnumerator MegaAttackIE()
  {
    EnemyScuttleTurret enemyScuttleTurret = this;
    enemyScuttleTurret.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyScuttleTurret.IdleWait = 1f;
    enemyScuttleTurret.Shooting = true;
    float s = enemyScuttleTurret.maxSpeed;
    enemyScuttleTurret.maxSpeed = 0.0f;
    enemyScuttleTurret.ClearPaths();
    if (enemyScuttleTurret.isMiniBoss)
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", enemyScuttleTurret.gameObject);
    else
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/warning", enemyScuttleTurret.gameObject);
    enemyScuttleTurret.spine.AnimationState.SetAnimation(0, "anticipate", true);
    float t = 0.0f;
    while ((double) t < (double) enemyScuttleTurret.anticipationTime)
    {
      t += Time.deltaTime * enemyScuttleTurret.spine.timeScale;
      enemyScuttleTurret.SimpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyScuttleTurret.anticipationTime * 0.75));
      yield return (object) null;
    }
    enemyScuttleTurret.SimpleSpineFlash.FlashWhite(false);
    if (enemyScuttleTurret.isMiniBoss)
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/attack", enemyScuttleTurret.gameObject);
    else
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish/attack", enemyScuttleTurret.gameObject);
    enemyScuttleTurret.projectilePattern.Shoot();
    for (int i = 0; i < enemyScuttleTurret.projectilePattern.Waves.Length; ++i)
    {
      enemyScuttleTurret.spine.AnimationState.SetAnimation(0, "attack-shoot", false);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyScuttleTurret.spine.timeScale) < (double) enemyScuttleTurret.projectilePattern.Waves[i].FinishDelay)
        yield return (object) null;
    }
    enemyScuttleTurret.spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    enemyScuttleTurret.lastMegaAttackTime = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyScuttleTurret.timeBetweenMegaAttacks.x, enemyScuttleTurret.timeBetweenMegaAttacks.y);
    enemyScuttleTurret.ShootDelay = 2f;
    enemyScuttleTurret.maxSpeed = s;
    enemyScuttleTurret.Shooting = false;
  }
}
