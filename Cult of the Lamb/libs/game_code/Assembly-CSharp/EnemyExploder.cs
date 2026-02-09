// Decompiled with JetBrains decompiler
// Type: EnemyExploder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyExploder : EnemyChaser
{
  [SerializeField]
  public float lockOnDistance;
  [SerializeField]
  public float lockOnAngle;
  [SerializeField]
  public LayerMask lockOnMask;
  [SerializeField]
  public GameObject explosionFXprefab;
  [SerializeField]
  public bool canExplode = true;
  [SerializeField]
  public float explodeTargetDistance;
  [SerializeField]
  public float explodeTime;
  [SerializeField]
  public float explosionPlayerDamage;
  [SerializeField]
  public float explosionEnemyDamage;
  [SerializeField]
  public float explosionRadius;
  [SerializeField]
  public float knockExplodeDelay;
  [SerializeField]
  public bool hittableWhileExploding;
  [SerializeField]
  public bool moveWhileExploding;
  [SerializeField]
  public bool flashWhite;
  [SerializeField]
  public bool flashRed;
  [SerializeField]
  public bool explodingOnStart;
  [SerializeField]
  public float startExplodeDelay;
  [SerializeField]
  public bool explodeWhenMeleed = true;
  [SerializeField]
  public bool restartExplodeOnHit = true;
  [SerializeField]
  public bool chase;
  [SerializeField]
  public bool patrol;
  [SerializeField]
  public bool flee;
  [SerializeField]
  public float TurningArc = 90f;
  [SerializeField]
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  [SerializeField]
  public List<Vector3> patrolRoute = new List<Vector3>();
  [SerializeField]
  public float fleeCheckIntervalTime;
  [SerializeField]
  public float wallCheckDistance;
  [SerializeField]
  public float distanceToFlee;
  [Space]
  [SerializeField]
  public ColliderEvents enemyCollider;
  public SkeletonAnimation Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string anticipationAnimation;
  public bool isHit;
  public bool isExploding;
  public GameObject hitInitiator;
  public bool exploded;
  public float explodeTimer;
  public float enemyExplodeDistance = 0.5f;
  public float flashTime = 0.15f;
  public float flashSpeed = 3f;
  public float startExplodeTimer = -1f;
  public float fleeTimestamp;
  public float repathTimestamp;
  public float initialSpawnTimestamp;
  public float randomDirection;
  public int patrolIndex;
  public Vector3 startPosition;
  public float distanceToTarget = float.MaxValue;
  public float repathTimeInterval = 2f;
  public float spawnExplodeDelay = 0.35f;
  public Vector2 lowestRoomBoundPoint;
  public Vector2 highestRoomBoundPoint;
  public bool isOptimizationsInitialized;
  public float ExplodeCountDown;
  public float ExplodeCountDownTarget = 0.75f;
  public static bool isRunning;
  public Collider2D[] colliders = new Collider2D[20];
  public static List<EnemyExploder> EnemyExploders = new List<EnemyExploder>();
  public bool CheckClosest;
  public System.Action OnExplode;
  public bool DelayingDestroy;

  public void InitializeOptimization(Vector2 lowest, Vector2 highest)
  {
    if (this.isOptimizationsInitialized)
      return;
    this.lowestRoomBoundPoint = new Vector2(lowest.x + 2f, lowest.y + 2f);
    this.highestRoomBoundPoint = new Vector2(highest.x - 2f, highest.y - 2f);
    this.isOptimizationsInitialized = true;
  }

  public override float timeStopMultiplier => 0.0f;

  public override void Awake()
  {
    base.Awake();
    this.explosionFXprefab.CreatePool(10, true);
  }

  public override void Start()
  {
    base.Start();
    this.startPosition = this.transform.position;
    if (this.patrolRoute.Count > 0)
      this.patrolRoute.Insert(0, Vector3.zero);
    if (!this.explodingOnStart && (bool) (UnityEngine.Object) this.gm)
    {
      this.health.enabled = false;
      this.initialSpawnTimestamp = this.gm.CurrentTime + this.spawnExplodeDelay;
    }
    if ((bool) (UnityEngine.Object) this.enemyCollider)
      this.enemyCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
    this.spine = this.Spine;
    this.health.OnPoisonedHit += new Health.HitAction(((UnitObject) this).OnHit);
    this.health.OnBurnHit += new Health.HitAction(((UnitObject) this).OnHit);
  }

  public override void Update()
  {
    base.Update();
    if ((double) this.gm.CurrentTime > (double) this.initialSpawnTimestamp)
      this.health.enabled = true;
    if ((UnityEngine.Object) this.targetObject != (UnityEngine.Object) null)
    {
      this.distanceToTarget = Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.transform.position, this.targetObject.transform.position));
      if (this.canExplode && !this.isExploding && (double) this.distanceToTarget < (double) this.explodeTargetDistance && (UnityEngine.Object) this.targetObject.state != (UnityEngine.Object) null && this.targetObject.state.CURRENT_STATE != StateMachine.State.Dodging)
      {
        if ((double) (this.ExplodeCountDown -= Time.deltaTime * this.Spine.timeScale) < 0.0)
          this.WithinDistanceOfTarget();
      }
      else
        this.ExplodeCountDown = this.ExplodeCountDownTarget;
    }
    if (this.isExploding && !this.DelayingDestroy)
    {
      this.explodeTimer += Time.deltaTime * this.Spine.timeScale;
      if ((double) this.explodeTimer > (double) this.flashTime)
      {
        float amt = this.explodeTimer / this.explodeTime;
        if (this.flashWhite)
          this.simpleSpineFlash.FlashWhite(amt);
        else if (this.flashRed)
          this.simpleSpineFlash.FlashRed(Mathf.PingPong(amt * this.flashSpeed, 0.75f));
        if ((double) amt > 1.0)
          this.Explode();
      }
    }
    if (!this.inRange || !this.explodingOnStart || this.isExploding || !((UnityEngine.Object) this.gm != (UnityEngine.Object) null))
      return;
    if ((double) this.startExplodeTimer == -1.0)
    {
      this.startExplodeTimer = this.gm.CurrentTime + this.startExplodeDelay;
    }
    else
    {
      if ((double) this.gm.CurrentTime <= (double) this.startExplodeTimer)
        return;
      this.ExplodeCharge();
    }
  }

  public virtual void WithinDistanceOfTarget() => this.ExplodeCharge();

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.isHit || !this.restartExplodeOnHit || !this.canExplode)
      return;
    this.health.ClearFreezeTime();
    this.isHit = true;
    this.isExploding = false;
    this.hitInitiator = Attacker;
    this.ExplodeCharge();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (this.exploded)
      return;
    this.health.ClearFreezeTime();
    this.isHit = true;
    this.KnockTowardsEnemy(Attacker, AttackType);
  }

  public virtual void KnockTowardsEnemy(GameObject attacker, Health.AttackTypes attackType)
  {
    this.StartCoroutine((IEnumerator) this.NotExplodingHittable(attacker, attackType));
  }

  public IEnumerator NotExplodingHittable(GameObject Attacker, Health.AttackTypes AttackType)
  {
    EnemyExploder enemyExploder = this;
    if (EnemyExploder.isRunning)
      enemyExploder.StopCoroutine((IEnumerator) enemyExploder.NotExplodingHittable(Attacker, AttackType));
    EnemyExploder.isRunning = true;
    if (!enemyExploder.isExploding || enemyExploder.hittableWhileExploding)
    {
      Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2) enemyExploder.transform.position, enemyExploder.lockOnDistance, (int) enemyExploder.lockOnMask);
      yield return (object) null;
      Vector3 from = new Vector3(InputManager.Gameplay.GetHorizontalAxis(), InputManager.Gameplay.GetVerticalAxis(), 0.0f);
      Collider2D collider2D1 = (Collider2D) null;
      float num1 = 0.0f;
      float num2 = 0.0f;
      enemyExploder.simpleSpineFlash.FlashFillRed();
      if (colliders.Length != 0)
      {
        foreach (Collider2D collider2D2 in colliders)
        {
          if (!((UnityEngine.Object) collider2D2 == (UnityEngine.Object) null) && !((UnityEngine.Object) collider2D2.gameObject == (UnityEngine.Object) null))
          {
            Vector3 normalized = (enemyExploder.transform.position - collider2D2.transform.position).normalized;
            float f = Mathf.Abs(Vector3.Angle(from, normalized) - 180f);
            bool flag = (double) f < (double) enemyExploder.lockOnAngle;
            if ((UnityEngine.Object) collider2D1 == (UnityEngine.Object) null || (double) Mathf.Abs(f) < (double) num2)
            {
              Health component = collider2D2.GetComponent<Health>();
              if (flag && (bool) (UnityEngine.Object) component && component.team == Health.Team.Team2 && (UnityEngine.Object) collider2D2.gameObject != (UnityEngine.Object) enemyExploder.gameObject && (double) component.HP >= (double) num1)
              {
                num1 = component.HP;
                collider2D1 = collider2D2;
                num2 = f;
              }
            }
          }
        }
      }
      Collider2D collider2D3 = collider2D1;
      if ((UnityEngine.Object) collider2D3 != (UnityEngine.Object) null && (UnityEngine.Object) collider2D3.gameObject != (UnityEngine.Object) enemyExploder.gameObject && (double) enemyExploder.MagnitudeFindDistanceBetween(collider2D3.transform.position, enemyExploder.transform.position) < (double) enemyExploder.lockOnDistance * (double) enemyExploder.lockOnDistance)
      {
        float angle = Utils.GetAngle(enemyExploder.transform.position, collider2D3.transform.position) * ((float) Math.PI / 180f);
        enemyExploder.DoKnockBack(angle, enemyExploder.knockbackMultiplier, 1f, false);
        enemyExploder.targetObject = collider2D3.GetComponent<Health>();
      }
      else if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
        enemyExploder.DoKnockBack(Attacker, enemyExploder.knockbackMultiplier, 1f, false);
      if (!enemyExploder.isExploding)
        enemyExploder.explodeTime = (enemyExploder.knockExplodeDelay + enemyExploder.explodeTime / 2f) / enemyExploder.Spine.timeScale;
      if (enemyExploder.explodeWhenMeleed || !enemyExploder.explodeWhenMeleed && AttackType != Health.AttackTypes.Melee)
        enemyExploder.ExplodeCharge();
      else if (enemyExploder.gameObject.activeInHierarchy && !enemyExploder.DelayingDestroy)
      {
        enemyExploder.StartCoroutine((IEnumerator) enemyExploder.DelayedDestroy());
        enemyExploder.Spine.gameObject.SetActive(false);
      }
      enemyExploder.ClearPaths();
      colliders = (Collider2D[]) null;
    }
    EnemyExploder.isRunning = false;
  }

  public void ExplodeCharge()
  {
    if ((double) GameManager.GetInstance().CurrentTime <= (double) this.initialSpawnTimestamp)
      return;
    this.explodeTimer = 0.0f;
    this.isExploding = true;
    if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null) || this.Spine.AnimationState == null)
      return;
    this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
  }

  public bool GetClosest()
  {
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
      return false;
    EnemyExploder enemyExploder1 = (EnemyExploder) null;
    float num1 = float.MaxValue;
    foreach (EnemyExploder enemyExploder2 in EnemyExploder.EnemyExploders)
    {
      float num2 = Vector3.Distance(enemyExploder2.transform.position, this.targetObject.transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        enemyExploder1 = enemyExploder2;
      }
    }
    return (UnityEngine.Object) enemyExploder1 == (UnityEngine.Object) this;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.CheckClosest)
      EnemyExploder.EnemyExploders.Add(this);
    if (!this.DelayingDestroy)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyExploder.EnemyExploders.Remove(this);
  }

  public override void UpdateMoving()
  {
    if (this.isExploding && (!this.moveWhileExploding || this.targetObject.team != Health.Team.PlayerTeam) || this.isHit)
      return;
    if (this.chase)
    {
      if (!this.CheckClosest)
        base.UpdateMoving();
      if (!this.CheckClosest)
        return;
      if (this.GetClosest())
      {
        base.UpdateMoving();
      }
      else
      {
        if (this.state.CURRENT_STATE != StateMachine.State.Idle && (double) this.gm.CurrentTime <= (double) this.repathTimestamp / (double) this.Spine.timeScale)
          return;
        if (this.patrolRoute.Count == 0)
          this.GetRandomTargetPosition();
        else if (this.pathToFollow == null)
        {
          this.patrolIndex = ++this.patrolIndex % this.patrolRoute.Count;
          this.givePath(this.startPosition + this.patrolRoute[this.patrolIndex]);
          this.LookAtAngle(Utils.GetAngle(this.transform.position, this.startPosition + this.patrolRoute[this.patrolIndex]));
        }
        this.repathTimestamp = this.gm.CurrentTime + this.repathTimeInterval;
      }
    }
    else if (this.patrol && (this.state.CURRENT_STATE == StateMachine.State.Idle || (double) this.gm.CurrentTime > (double) this.repathTimestamp / (double) this.Spine.timeScale))
    {
      if (this.patrolRoute.Count == 0)
        this.GetRandomTargetPosition();
      else if (this.pathToFollow == null)
      {
        this.patrolIndex = ++this.patrolIndex % this.patrolRoute.Count;
        this.givePath(this.startPosition + this.patrolRoute[this.patrolIndex]);
        this.LookAtAngle(Utils.GetAngle(this.transform.position, this.startPosition + this.patrolRoute[this.patrolIndex]));
      }
      this.repathTimestamp = this.gm.CurrentTime + this.repathTimeInterval;
    }
    else
    {
      if (!this.flee || !(bool) (UnityEngine.Object) this.gm || (double) this.gm.CurrentTime <= (double) this.fleeTimestamp / (double) this.Spine.timeScale)
        return;
      this.fleeTimestamp = this.gm.CurrentTime + this.fleeCheckIntervalTime;
      if ((double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) > (double) this.distanceToFlee)
      {
        this.GetRandomTargetPosition();
      }
      else
      {
        this.ClearPaths();
        this.Flee();
      }
    }
  }

  public void Flee()
  {
    float num1 = 2f;
    while ((double) --num1 > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(4, 7);
      Vector3 targetLocation = this.targetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      Vector3 direction = Vector3.Normalize(targetLocation - this.targetObject.transform.position);
      bool flag = false;
      if (this.isOptimizationsInitialized)
      {
        Vector2 vector2 = (Vector2) targetLocation;
        if ((double) vector2.x <= (double) this.lowestRoomBoundPoint.x || (double) vector2.x >= (double) this.highestRoomBoundPoint.x || (double) vector2.y <= (double) this.lowestRoomBoundPoint.y || (double) vector2.y >= (double) this.highestRoomBoundPoint.y)
          flag = true;
      }
      else if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.targetObject.transform.position, 0.5f, (Vector2) direction, distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
        flag = true;
      if (flag)
      {
        float num2 = 180f - f;
      }
      else
      {
        this.givePath(targetLocation);
        break;
      }
    }
  }

  public void Explode()
  {
    if (!this.canExplode || this.DelayingDestroy)
      return;
    if ((UnityEngine.Object) this.explosionFXprefab != (UnityEngine.Object) null)
      Explosion.CreateExplosionCustomFX(this.transform.position, this.isHit ? Health.Team.PlayerTeam : Health.Team.KillAll, this.health, this.explosionRadius, this.explosionFXprefab, this.explosionPlayerDamage, this.explosionEnemyDamage);
    else
      Explosion.CreateExplosion(this.transform.position, this.isHit ? Health.Team.PlayerTeam : Health.Team.KillAll, this.health, this.explosionRadius, this.explosionPlayerDamage, this.explosionEnemyDamage);
    AudioManager.Instance.PlayOneShot("event:/explosion/explosion", this.transform.position);
    System.Action onExplode = this.OnExplode;
    if (onExplode != null)
      onExplode();
    this.exploded = true;
    this.health.DealDamage(this.health.totalHP, (UnityEngine.Object) this.hitInitiator != (UnityEngine.Object) null ? this.hitInitiator : this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile, AttackFlags: Health.AttackFlags.DoesntChargeRelics);
    if (!this.gameObject.activeInHierarchy || this.DelayingDestroy)
      return;
    this.StartCoroutine((IEnumerator) this.DelayedDestroy());
    this.Spine.gameObject.SetActive(false);
  }

  public IEnumerator DelayedDestroy()
  {
    EnemyExploder enemyExploder = this;
    enemyExploder.DelayingDestroy = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyExploder.Spine.timeScale) < 1.0)
      yield return (object) null;
    if ((bool) (UnityEngine.Object) enemyExploder.gameObject)
      UnityEngine.Object.Destroy((UnityEngine.Object) enemyExploder.gameObject);
  }

  public void GetRandomTargetPosition()
  {
    float num = 2f;
    while ((double) --num > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      this.randomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      float radius = 0.2f;
      Vector3 vector3 = this.transform.position + new Vector3(distance * Mathf.Cos(this.randomDirection), distance * Mathf.Sin(this.randomDirection));
      bool flag = false;
      if (this.isOptimizationsInitialized)
      {
        Vector2 vector2 = (Vector2) vector3;
        if ((double) vector2.x <= (double) this.lowestRoomBoundPoint.x || (double) vector2.x >= (double) this.highestRoomBoundPoint.x || (double) vector2.y <= (double) this.lowestRoomBoundPoint.y || (double) vector2.y >= (double) this.highestRoomBoundPoint.y)
          flag = true;
      }
      else if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
        flag = true;
      if (flag)
      {
        this.randomDirection = 180f - this.randomDirection;
      }
      else
      {
        float angle = Utils.GetAngle(this.transform.position, vector3);
        this.givePath(vector3);
        this.LookAtAngle(angle);
        break;
      }
    }
  }

  public virtual void OnTriggerEnterEvent(Collider2D collision)
  {
    if ((int) this.lockOnMask != ((int) this.lockOnMask | 1 << collision.gameObject.layer) || !this.isHit || !((UnityEngine.Object) collision.gameObject != (UnityEngine.Object) this.gameObject))
      return;
    this.Explode();
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if ((int) this.lockOnMask != ((int) this.lockOnMask | 1 << collision.collider.gameObject.layer) || !this.isHit || !((UnityEngine.Object) collision.gameObject != (UnityEngine.Object) this.gameObject))
      return;
    this.Explode();
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.explodeTargetDistance, Color.red);
    if (!this.patrol)
      return;
    if (!Application.isPlaying)
    {
      int index = -1;
      while (++index < this.patrolRoute.Count)
      {
        if (index == this.patrolRoute.Count - 1 || index == 0)
          Utils.DrawLine(this.transform.position, this.transform.position + this.patrolRoute[index], Color.yellow);
        if (index > 0)
          Utils.DrawLine(this.transform.position + this.patrolRoute[index - 1], this.transform.position + this.patrolRoute[index], Color.yellow);
        Utils.DrawCircleXY(this.transform.position + this.patrolRoute[index], 0.2f, Color.yellow);
      }
    }
    else
    {
      int index = -1;
      while (++index < this.patrolRoute.Count)
      {
        if (index == this.patrolRoute.Count - 1 || index == 0)
          Utils.DrawLine(this.startPosition, this.startPosition + this.patrolRoute[index], Color.yellow);
        if (index > 0)
          Utils.DrawLine(this.startPosition + this.patrolRoute[index - 1], this.startPosition + this.patrolRoute[index], Color.yellow);
        Utils.DrawCircleXY(this.startPosition + this.patrolRoute[index], 0.2f, Color.yellow);
      }
    }
  }
}
