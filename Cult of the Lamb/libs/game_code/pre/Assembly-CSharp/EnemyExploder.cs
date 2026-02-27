// Decompiled with JetBrains decompiler
// Type: EnemyExploder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyExploder : EnemyChaser
{
  [SerializeField]
  private float lockOnDistance;
  [SerializeField]
  private float lockOnAngle;
  [SerializeField]
  private LayerMask lockOnMask;
  [SerializeField]
  private GameObject explosionFXprefab;
  [SerializeField]
  protected bool canExplode = true;
  [SerializeField]
  private float explodeTargetDistance;
  [SerializeField]
  private float explodeTime;
  [SerializeField]
  private float explosionPlayerDamage;
  [SerializeField]
  private float explosionEnemyDamage;
  [SerializeField]
  private float explosionRadius;
  [SerializeField]
  private float knockExplodeDelay;
  [SerializeField]
  private bool hittableWhileExploding;
  [SerializeField]
  private bool moveWhileExploding;
  [SerializeField]
  private bool flashWhite;
  [SerializeField]
  private bool flashRed;
  [SerializeField]
  private bool explodingOnStart;
  [SerializeField]
  private float startExplodeDelay;
  [SerializeField]
  private bool explodeWhenMeleed = true;
  [SerializeField]
  private bool restartExplodeOnHit = true;
  [SerializeField]
  public bool chase;
  [SerializeField]
  private bool patrol;
  [SerializeField]
  private bool flee;
  [SerializeField]
  private float TurningArc = 90f;
  [SerializeField]
  private Vector2 DistanceRange = new Vector2(1f, 3f);
  [SerializeField]
  private List<Vector3> patrolRoute = new List<Vector3>();
  [SerializeField]
  private float fleeCheckIntervalTime;
  [SerializeField]
  private float wallCheckDistance;
  [SerializeField]
  private float distanceToFlee;
  [Space]
  [SerializeField]
  private ColliderEvents enemyCollider;
  public SkeletonAnimation Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string anticipationAnimation;
  protected bool isHit;
  protected bool isExploding;
  private bool exploded;
  private float explodeTimer;
  private float enemyExplodeDistance = 0.5f;
  private float flashTime = 0.15f;
  private float flashSpeed = 3f;
  private float startExplodeTimer = -1f;
  private float fleeTimestamp;
  private float repathTimestamp;
  protected float initialSpawnTimestamp;
  private float randomDirection;
  private int patrolIndex;
  private Vector3 startPosition;
  protected float distanceToTarget = float.MaxValue;
  private float repathTimeInterval = 2f;
  private float spawnExplodeDelay = 0.35f;
  private float ExplodeCountDown;
  public float ExplodeCountDownTarget = 0.75f;
  private static bool isRunning;
  private Collider2D[] colliders = new Collider2D[20];
  private static List<EnemyExploder> EnemyExploders = new List<EnemyExploder>();
  public bool CheckClosest;
  public System.Action OnExplode;
  public bool DelayingDestroy;

  protected override float timeStopMultiplier => 0.0f;

  protected override void Start()
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
    if (!(bool) (UnityEngine.Object) this.enemyCollider)
      return;
    this.enemyCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
  }

  public override void Update()
  {
    base.Update();
    if ((double) this.gm.CurrentTime > (double) this.initialSpawnTimestamp)
      this.health.enabled = true;
    if ((UnityEngine.Object) this.targetObject != (UnityEngine.Object) null)
    {
      this.distanceToTarget = Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.transform.position, this.targetObject.transform.position));
      if (this.canExplode && !this.isExploding && (double) this.distanceToTarget < (double) this.explodeTargetDistance && this.targetObject.state.CURRENT_STATE != StateMachine.State.Dodging)
      {
        if ((double) (this.ExplodeCountDown -= Time.deltaTime) < 0.0)
          this.WithinDistanceOfTarget();
      }
      else
        this.ExplodeCountDown = this.ExplodeCountDownTarget;
    }
    if (this.isExploding && !this.DelayingDestroy)
    {
      this.explodeTimer += Time.deltaTime;
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

  protected virtual void WithinDistanceOfTarget() => this.ExplodeCharge();

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.isHit || !this.restartExplodeOnHit || !this.canExplode)
      return;
    this.isHit = true;
    this.isExploding = false;
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
    this.isHit = true;
    this.KnockTowardsEnemy(Attacker, AttackType);
  }

  protected virtual void KnockTowardsEnemy(GameObject attacker, Health.AttackTypes attackType)
  {
    this.StartCoroutine((IEnumerator) this.NotExplodingHittable(attacker, attackType));
  }

  private IEnumerator NotExplodingHittable(GameObject Attacker, Health.AttackTypes AttackType)
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
      else
        enemyExploder.DoKnockBack(Attacker, enemyExploder.knockbackMultiplier, 1f, false);
      if (!enemyExploder.isExploding)
        enemyExploder.explodeTime = enemyExploder.knockExplodeDelay + enemyExploder.explodeTime / 2f;
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

  protected void ExplodeCharge()
  {
    if ((double) this.gm.CurrentTime <= (double) this.initialSpawnTimestamp)
      return;
    this.explodeTimer = 0.0f;
    this.isExploding = true;
    if (!((UnityEngine.Object) this.Spine != (UnityEngine.Object) null) || this.Spine.AnimationState == null)
      return;
    this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
  }

  private bool GetClosest()
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
    if (!this.CheckClosest)
      return;
    EnemyExploder.EnemyExploders.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyExploder.EnemyExploders.Remove(this);
  }

  protected override void UpdateMoving()
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
        if (this.state.CURRENT_STATE != StateMachine.State.Idle && (double) this.gm.CurrentTime <= (double) this.repathTimestamp)
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
    else if (this.patrol && (this.state.CURRENT_STATE == StateMachine.State.Idle || (double) this.gm.CurrentTime > (double) this.repathTimestamp))
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
      if (!this.flee || !(bool) (UnityEngine.Object) this.gm || (double) this.gm.CurrentTime <= (double) this.fleeTimestamp)
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

  private void Flee()
  {
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(4, 7);
      Vector3 targetLocation = this.targetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.targetObject.transform.position, 0.5f, (Vector2) Vector3.Normalize(targetLocation - this.targetObject.transform.position), distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if ((double) Vector3.Distance(this.targetObject.transform.position, (Vector3) raycastHit2D.centroid) > 3.0)
          this.givePath(targetLocation);
      }
      else
        this.givePath(targetLocation);
    }
  }

  protected void Explode()
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
    this.health.DealDamage(this.health.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile);
    if (!this.gameObject.activeInHierarchy || this.DelayingDestroy)
      return;
    this.StartCoroutine((IEnumerator) this.DelayedDestroy());
    this.Spine.gameObject.SetActive(false);
  }

  private IEnumerator DelayedDestroy()
  {
    EnemyExploder enemyExploder = this;
    enemyExploder.DelayingDestroy = true;
    yield return (object) new WaitForSeconds(1f);
    if ((bool) (UnityEngine.Object) enemyExploder.gameObject)
      UnityEngine.Object.Destroy((UnityEngine.Object) enemyExploder.gameObject);
  }

  public void GetRandomTargetPosition()
  {
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      this.randomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      float radius = 0.2f;
      Vector3 vector3 = this.transform.position + new Vector3(distance * Mathf.Cos(this.randomDirection), distance * Mathf.Sin(this.randomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
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

  protected virtual void OnTriggerEnterEvent(Collider2D collision)
  {
    if ((int) this.lockOnMask != ((int) this.lockOnMask | 1 << collision.gameObject.layer) || !this.isHit || !((UnityEngine.Object) collision.gameObject != (UnityEngine.Object) this.gameObject))
      return;
    this.Explode();
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if ((int) this.lockOnMask != ((int) this.lockOnMask | 1 << collision.collider.gameObject.layer) || !this.isHit || !((UnityEngine.Object) collision.gameObject != (UnityEngine.Object) this.gameObject))
      return;
    this.Explode();
  }

  private float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  private void OnDrawGizmos()
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
