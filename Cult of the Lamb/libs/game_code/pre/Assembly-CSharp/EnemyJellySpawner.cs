// Decompiled with JetBrains decompiler
// Type: EnemyJellySpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyJellySpawner : EnemyJellyCharger
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string spawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string teleportAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string attackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnimation;
  [SerializeField]
  private bool teleportOnHit;
  [SerializeField]
  private float circleCastRadius = 0.5f;
  [SerializeField]
  private float teleportDelay = 0.5f;
  [SerializeField]
  private float teleportMoveDelay = 0.5f;
  [SerializeField]
  private float teleportCooldownDelay = 0.5f;
  [SerializeField]
  private AssetReferenceGameObject[] spawnableEnemies;
  [SerializeField]
  private float randomSpawnMinDelay;
  [SerializeField]
  private float randomSpawnMaxDelay;
  [SerializeField]
  private float spawnSpitOutForce;
  [SerializeField]
  private float spawnDuration;
  [SerializeField]
  private float spawnCooldown;
  [SerializeField]
  private int spawnAmountMin;
  [SerializeField]
  private int spawnAmountMax;
  [SerializeField]
  private float growSpeed;
  [SerializeField]
  private Ease growEase;
  [SerializeField]
  private int maxEnemiesActive;
  [Tooltip("will not spawn when target is within min radius")]
  [SerializeField]
  private float targetMinSpawnRadius;
  [SerializeField]
  private bool randomSpawnDirection = true;
  [SerializeField]
  private bool killSpawnablesOnDeath;
  [SerializeField]
  private bool canAttack;
  [SerializeField]
  private bool attackOnHit = true;
  [SerializeField]
  private float attackDistance;
  [SerializeField]
  private float attackChargeDur;
  [SerializeField]
  private float attackCooldown;
  [SerializeField]
  private float damageDuration = 0.2f;
  [SerializeField]
  private bool canShoot;
  [SerializeField]
  private global::ProjectilePattern projectilePattern;
  [SerializeField]
  private float projectileAnticipation;
  [SerializeField]
  private Vector2 timeBetweenProjectileShots;
  [Space]
  [SerializeField]
  private bool canShootSwirl;
  [SerializeField]
  private ProjectilePatternBase projectileSwirlPattern;
  [SerializeField]
  private float projectileSwirlAnticipation;
  [SerializeField]
  private Vector2 timeBetweenProjectileSwirlShots;
  private float lastProjectileShootingTime;
  private float lastProjectileSwirlShootingTime;
  private float spawnTime = float.MaxValue;
  private bool spawning;
  private int spawnedAmount;
  private bool teleporting;
  private float attackTimer;
  private bool charging;
  private bool attacking;
  private Coroutine spawnRoutine;
  private ShowHPBar hpBar;
  private List<UnitObject> spawnedEnemies = new List<UnitObject>();
  private Vector3 v3Shake = Vector3.zero;
  private Vector3 v3ShakeSpeed = Vector3.zero;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  protected override void Start()
  {
    base.Start();
    this.SetSpawnTime();
    this.hpBar = this.GetComponent<ShowHPBar>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.spawning = false;
    this.teleporting = false;
    this.charging = false;
    this.attacking = false;
    this.lastProjectileSwirlShootingTime = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.timeBetweenProjectileSwirlShots.x, this.timeBetweenProjectileSwirlShots.y);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.simpleSpineFlash.FlashWhite(false);
    this.simpleSpineFlash.FlashFillRed();
    if (this.teleportOnHit && !this.attacking && (AttackType == Health.AttackTypes.Melee || AttackType == Health.AttackTypes.Projectile))
    {
      this.StartCoroutine((IEnumerator) this.TeleportIE());
    }
    else
    {
      if (this.canAttack && this.attackOnHit && AttackType == Health.AttackTypes.Melee)
        this.ChargeAttack();
      base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
      this.v3ShakeSpeed = this.transform.position - Attacker.transform.position;
      this.v3ShakeSpeed = this.v3ShakeSpeed.normalized * 30f;
    }
  }

  public override void Update()
  {
    if (!this.teleporting)
      base.Update();
    if ((double) Time.deltaTime > 0.0)
    {
      this.v3ShakeSpeed += (Vector3.zero - this.v3Shake) * 0.4f / Time.deltaTime;
      this.v3Shake += (this.v3ShakeSpeed *= 0.7f) * Time.deltaTime;
      this.Spine.transform.localPosition = this.v3Shake;
    }
    if (this.inRange && (double) this.spawnTime == 3.4028234663852886E+38)
      this.SetSpawnTime();
    if (this.inRange && GameManager.RoomActive && (bool) (UnityEngine.Object) this.targetObject && (bool) (UnityEngine.Object) this.gm && (double) this.gm.CurrentTime > (double) this.spawnTime && !this.attacking && !this.spawning && !this.teleporting)
      this.SpawnEnemy();
    if (GameManager.RoomActive && (bool) (UnityEngine.Object) this.gm && (double) this.gm.CurrentTime > (double) this.lastProjectileShootingTime && this.canShoot && !this.attacking && !this.spawning && !this.teleporting)
      this.ShootProjectile();
    if (GameManager.RoomActive && (bool) (UnityEngine.Object) this.gm && (double) this.gm.CurrentTime > (double) this.lastProjectileSwirlShootingTime && this.canShootSwirl && !this.attacking && !this.spawning && !this.teleporting)
      this.ShootSwirl();
    if ((bool) (UnityEngine.Object) this.targetObject)
      this.inRange = (double) Vector3.Distance(this.targetObject.transform.position, this.transform.position) < (double) this.VisionRange;
    if (!this.canAttack)
      return;
    if (this.charging)
    {
      this.attackTimer += Time.deltaTime;
      float amt = this.attackTimer / this.attackChargeDur;
      this.simpleSpineFlash.FlashWhite(amt);
      if ((double) amt > 1.0)
        this.Attack();
    }
    if (!(bool) (UnityEngine.Object) this.targetObject || this.charging || this.attacking || (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) >= (double) this.attackDistance)
      return;
    this.ChargeAttack();
  }

  protected override void UpdateMoving()
  {
    if (this.attacking || this.spawning || this.teleporting)
      return;
    base.UpdateMoving();
  }

  private void ChargeAttack()
  {
    if (this.attacking || this.spawning)
      return;
    this.charging = true;
    this.attackTimer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Charging;
    this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", this.transform.position);
  }

  private void SpawnEnemy()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting || this.spawnableEnemies.Length == 0 || this.spawnedAmount >= this.maxEnemiesActive || (double) this.targetMinSpawnRadius != 0.0 && (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) <= (double) this.targetMinSpawnRadius)
      return;
    this.spawnRoutine = this.StartCoroutine((IEnumerator) this.SpawnDelay());
  }

  private IEnumerator SpawnDelay()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    enemyJellySpawner.ClearPaths();
    enemyJellySpawner.spawning = true;
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.anticipationAnimation, true);
    yield return (object) new WaitForSeconds(enemyJellySpawner.spawnDuration / enemyJellySpawner.Spine.timeScale);
    int num = UnityEngine.Random.Range(enemyJellySpawner.spawnAmountMin, enemyJellySpawner.spawnAmountMax + 1);
    UnityEngine.Random.Range(0, 360);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser_boss/chaser_boss_egg_spawn", enemyJellySpawner.transform.position);
    for (int index = 0; index < num; ++index)
    {
      // ISSUE: reference to a compiler-generated method
      Addressables.InstantiateAsync((object) enemyJellySpawner.spawnableEnemies[UnityEngine.Random.Range(0, enemyJellySpawner.spawnableEnemies.Length)], enemyJellySpawner.GetRandomSpawnPosition(), Quaternion.identity, enemyJellySpawner.transform.parent).Completed += new System.Action<AsyncOperationHandle<GameObject>>(enemyJellySpawner.\u003CSpawnDelay\u003Eb__58_0);
    }
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.spawnAnimation, false);
    enemyJellySpawner.Spine.AnimationState.AddAnimation(0, enemyJellySpawner.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyJellySpawner.spawnCooldown);
    enemyJellySpawner.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellySpawner.SetSpawnTime();
    enemyJellySpawner.spawning = false;
    enemyJellySpawner.spawnRoutine = (Coroutine) null;
  }

  private Vector3 GetRandomSpawnPosition()
  {
    return new Vector3(UnityEngine.Random.Range(-6.5f, 6.5f), UnityEngine.Random.Range(-3.5f, 3.5f), 0.0f);
  }

  private void OnEnemyKilled(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.spawnedAmount;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!this.killSpawnablesOnDeath)
      return;
    if (this.spawnRoutine != null)
    {
      this.StopCoroutine(this.spawnRoutine);
      this.spawning = false;
    }
    foreach (UnitObject spawnedEnemy in this.spawnedEnemies)
    {
      if ((UnityEngine.Object) spawnedEnemy != (UnityEngine.Object) null)
      {
        spawnedEnemy.enabled = true;
        spawnedEnemy.health.DealDamage(spawnedEnemy.health.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy, dealDamageImmediately: true);
      }
    }
  }

  private void SetSpawnTime()
  {
    if (!(bool) (UnityEngine.Object) this.gm)
      return;
    this.spawnTime = this.gm.CurrentTime + UnityEngine.Random.Range(this.randomSpawnMinDelay, this.randomSpawnMaxDelay);
  }

  private IEnumerator TeleportIE()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    if (!enemyJellySpawner.teleporting)
    {
      if (enemyJellySpawner.spawning && enemyJellySpawner.spawnRoutine != null)
      {
        enemyJellySpawner.StopCoroutine(enemyJellySpawner.spawnRoutine);
        enemyJellySpawner.spawning = false;
      }
      enemyJellySpawner.teleporting = true;
      yield return (object) new WaitForSeconds(enemyJellySpawner.teleportDelay);
      enemyJellySpawner.ClearPaths();
      enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.teleportAnimation, false);
      yield return (object) new WaitForEndOfFrame();
      if ((bool) (UnityEngine.Object) enemyJellySpawner.hpBar)
        enemyJellySpawner.hpBar.Hide();
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", enemyJellySpawner.transform.position);
      yield return (object) new WaitForSeconds(enemyJellySpawner.teleportMoveDelay / enemyJellySpawner.Spine.timeScale);
      float num = 100f;
      while ((double) --num > 0.0)
      {
        float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
        float distance = (float) UnityEngine.Random.Range(4, 7);
        Vector3 vector3_1 = (UnityEngine.Object) enemyJellySpawner.targetObject != (UnityEngine.Object) null ? enemyJellySpawner.targetObject.transform.position : enemyJellySpawner.transform.position;
        Vector3 vector3_2 = vector3_1 + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
        RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) vector3_1, enemyJellySpawner.circleCastRadius, (Vector2) Vector3.Normalize(vector3_2 - vector3_1), distance, (int) enemyJellySpawner.layerToCheck);
        if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(vector3_1, (Vector3) raycastHit2D.centroid) > 3.0)
          {
            if (enemyJellySpawner.ShowDebug)
            {
              enemyJellySpawner.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y));
              enemyJellySpawner.PointsLink.Add(new Vector3(enemyJellySpawner.transform.position.x, enemyJellySpawner.transform.position.y));
            }
            enemyJellySpawner.transform.position = (Vector3) raycastHit2D.centroid + Vector3.Normalize(vector3_1 - vector3_2) * enemyJellySpawner.circleCastRadius;
            break;
          }
        }
        else
        {
          if (enemyJellySpawner.ShowDebug)
          {
            enemyJellySpawner.EndPoints.Add(new Vector3(vector3_2.x, vector3_2.y));
            enemyJellySpawner.EndPointsLink.Add(new Vector3(enemyJellySpawner.transform.position.x, enemyJellySpawner.transform.position.y));
          }
          enemyJellySpawner.transform.position = vector3_2;
          break;
        }
      }
      if ((UnityEngine.Object) enemyJellySpawner.targetObject != (UnityEngine.Object) null)
        enemyJellySpawner.state.facingAngle = Utils.GetAngle(enemyJellySpawner.transform.position, enemyJellySpawner.targetObject.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", enemyJellySpawner.transform.position);
      yield return (object) new WaitForSeconds(enemyJellySpawner.teleportCooldownDelay / enemyJellySpawner.Spine.timeScale);
      enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.idleAnimation, true);
      enemyJellySpawner.teleporting = false;
    }
  }

  private IEnumerator TeleportToPositionIE(Vector3 pos)
  {
    EnemyJellySpawner enemyJellySpawner = this;
    if (!enemyJellySpawner.teleporting)
    {
      if (enemyJellySpawner.spawning && enemyJellySpawner.spawnRoutine != null)
      {
        enemyJellySpawner.StopCoroutine(enemyJellySpawner.spawnRoutine);
        enemyJellySpawner.spawning = false;
      }
      enemyJellySpawner.teleporting = true;
      yield return (object) new WaitForSeconds(enemyJellySpawner.teleportDelay);
      enemyJellySpawner.ClearPaths();
      enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.teleportAnimation, false);
      yield return (object) new WaitForEndOfFrame();
      if ((bool) (UnityEngine.Object) enemyJellySpawner.hpBar)
        enemyJellySpawner.hpBar.Hide();
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", enemyJellySpawner.transform.position);
      yield return (object) new WaitForSeconds(enemyJellySpawner.teleportMoveDelay / enemyJellySpawner.Spine.timeScale);
      enemyJellySpawner.transform.position = pos;
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", enemyJellySpawner.transform.position);
      yield return (object) new WaitForSeconds(enemyJellySpawner.teleportCooldownDelay / enemyJellySpawner.Spine.timeScale);
      enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.idleAnimation, true);
      enemyJellySpawner.teleporting = false;
    }
  }

  private void Attack()
  {
    if (this.attacking)
      return;
    this.attackTimer = 0.0f;
    this.attacking = true;
    this.charging = false;
    this.Spine.AnimationState.SetAnimation(0, this.attackAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", this.transform.position);
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.TurnOnDamageColliderForDuration(this.damageDuration));
    this.StartCoroutine((IEnumerator) this.AttackCooldownIE());
  }

  private IEnumerator AttackCooldownIE()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    yield return (object) new WaitForEndOfFrame();
    enemyJellySpawner.simpleSpineFlash.FlashWhite(false);
    yield return (object) new WaitForSeconds(enemyJellySpawner.attackCooldown);
    enemyJellySpawner.attacking = false;
    enemyJellySpawner.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  private IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJellySpawner enemyJellySpawner = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyJellySpawner.damageColliderEvents.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyJellySpawner.damageColliderEvents.SetActive(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void ShootProjectile()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting)
      return;
    this.StartCoroutine((IEnumerator) this.ProjectilePattern());
  }

  private IEnumerator ProjectilePattern()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    enemyJellySpawner.attacking = true;
    enemyJellySpawner.ClearPaths();
    float t = 0.0f;
    while ((double) t < (double) enemyJellySpawner.projectileAnticipation / (double) enemyJellySpawner.Spine.timeScale)
    {
      t += Time.deltaTime;
      enemyJellySpawner.simpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyJellySpawner.projectileAnticipation * 0.75));
      yield return (object) null;
    }
    enemyJellySpawner.simpleSpineFlash.FlashWhite(false);
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.shootAnimation, false);
    enemyJellySpawner.Spine.AnimationState.AddAnimation(0, enemyJellySpawner.idleAnimation, true, 0.0f);
    yield return (object) enemyJellySpawner.StartCoroutine((IEnumerator) enemyJellySpawner.projectilePattern.ShootIE(0.0f, (GameObject) null, (Transform) null));
    enemyJellySpawner.lastProjectileShootingTime = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyJellySpawner.timeBetweenProjectileShots.x, enemyJellySpawner.timeBetweenProjectileShots.y);
    enemyJellySpawner.attacking = false;
  }

  private void ShootSwirl()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting)
      return;
    this.StartCoroutine((IEnumerator) this.ProjectileSwirlPattern());
  }

  private IEnumerator ProjectileSwirlPattern()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    enemyJellySpawner.attacking = true;
    enemyJellySpawner.ClearPaths();
    yield return (object) enemyJellySpawner.StartCoroutine((IEnumerator) enemyJellySpawner.TeleportToPositionIE(Vector3.zero));
    float t = 0.0f;
    while ((double) t < (double) enemyJellySpawner.projectileSwirlAnticipation / (double) enemyJellySpawner.Spine.timeScale)
    {
      t += Time.deltaTime;
      enemyJellySpawner.simpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyJellySpawner.projectileSwirlAnticipation * 0.75));
      yield return (object) null;
    }
    enemyJellySpawner.simpleSpineFlash.FlashWhite(false);
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.shootAnimation, true);
    yield return (object) enemyJellySpawner.StartCoroutine((IEnumerator) enemyJellySpawner.projectileSwirlPattern.ShootIE());
    enemyJellySpawner.Spine.AnimationState.AddAnimation(0, enemyJellySpawner.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    enemyJellySpawner.lastProjectileSwirlShootingTime = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyJellySpawner.timeBetweenProjectileSwirlShots.x, enemyJellySpawner.timeBetweenProjectileSwirlShots.y);
    enemyJellySpawner.attacking = false;
  }

  private void OnDrawGizmos()
  {
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], this.circleCastRadius, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], this.circleCastRadius, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }
}
