// Decompiled with JetBrains decompiler
// Type: EnemyJellySpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyJellySpawner : EnemyJellyCharger
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string teleportAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootLongAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootLongAnticipateAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string teleportShootAnimation;
  [SerializeField]
  public bool teleportOnHit;
  [SerializeField]
  public float circleCastRadius = 0.5f;
  [SerializeField]
  public float teleportDelay = 0.5f;
  [SerializeField]
  public float teleportMoveDelay = 0.5f;
  [SerializeField]
  public float teleportCooldownDelay = 0.5f;
  [SerializeField]
  public AssetReferenceGameObject[] spawnableEnemies;
  [SerializeField]
  public float randomSpawnMinDelay;
  [SerializeField]
  public float randomSpawnMaxDelay;
  [SerializeField]
  public float spawnSpitOutForce;
  [SerializeField]
  public float spawnDuration;
  [SerializeField]
  public float spawnCooldown;
  [SerializeField]
  public int spawnAmountMin;
  [SerializeField]
  public int spawnAmountMax;
  [SerializeField]
  public float growSpeed;
  [SerializeField]
  public Ease growEase;
  [SerializeField]
  public int maxEnemiesActive;
  [Tooltip("will not spawn when target is within min radius")]
  [SerializeField]
  public float targetMinSpawnRadius;
  [SerializeField]
  public bool randomSpawnDirection = true;
  [SerializeField]
  public bool killSpawnablesOnDeath;
  [SerializeField]
  public bool canAttack;
  [SerializeField]
  public bool attackOnHit = true;
  [SerializeField]
  public float attackDistance;
  [SerializeField]
  public float attackChargeDur;
  [SerializeField]
  public float attackCooldown;
  [SerializeField]
  public float damageDuration = 0.2f;
  [SerializeField]
  public bool canShoot;
  [SerializeField]
  public global::ProjectilePattern projectilePattern;
  [SerializeField]
  public float projectileAnticipation;
  [SerializeField]
  public Vector2 timeBetweenProjectileShots;
  [Space]
  [SerializeField]
  public bool canShootSwirl;
  [SerializeField]
  public ProjectilePatternBase projectileSwirlPattern;
  [SerializeField]
  public float projectileSwirlAnticipation;
  [SerializeField]
  public Vector2 timeBetweenProjectileSwirlShots;
  [Space]
  [SerializeField]
  public bool canDropBombs;
  [SerializeField]
  public Vector2 bombsToDrop;
  [SerializeField]
  public Vector2 timeBetweenDroppingBombs;
  public float lastProjectileShootingTime;
  public float lastProjectileSwirlShootingTime;
  public float lastDropBombTime;
  public float lastBeamTime;
  public float spawnTime = float.MaxValue;
  public bool spawning;
  public int spawnedAmount;
  public bool teleporting;
  public float attackTimer;
  public bool charging;
  public bool attacking;
  public Coroutine spawnRoutine;
  public ShowHPBar hpBar;
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public AsyncOperationHandle<GameObject> trapBomb;
  public Vector3 v3Shake = Vector3.zero;
  public Vector3 v3ShakeSpeed = Vector3.zero;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public IEnumerator Start()
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
      enemyJellySpawner.Preload();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyJellySpawner.\u003C\u003En__0();
    enemyJellySpawner.SetSpawnTime();
    enemyJellySpawner.hpBar = enemyJellySpawner.GetComponent<ShowHPBar>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Preload()
  {
    for (int index = 0; index < this.spawnableEnemies.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.spawnableEnemies[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        obj.Result.CreatePool(20, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
    AsyncOperationHandle<GameObject> trapOp = Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Dungeon/Traps/Trap Bomb.prefab");
    trapOp.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.trapBomb = trapOp;
      trapOp.Result.CreatePool(Mathf.CeilToInt(this.bombsToDrop.y), true);
    });
    trapOp.WaitForCompletion();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.spawning = false;
    this.teleporting = false;
    this.charging = false;
    this.attacking = false;
    this.health.invincible = false;
    this.lastProjectileSwirlShootingTime = ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null ? GameManager.GetInstance().CurrentTime : Time.time) + UnityEngine.Random.Range(this.timeBetweenProjectileSwirlShots.x, this.timeBetweenProjectileSwirlShots.y);
    this.lastDropBombTime = ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null ? GameManager.GetInstance().CurrentTime : Time.time) + UnityEngine.Random.Range(this.timeBetweenDroppingBombs.x, this.timeBetweenDroppingBombs.y);
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
    if (GameManager.RoomActive && (bool) (UnityEngine.Object) this.gm && (double) this.gm.CurrentTime > (double) this.lastProjectileShootingTime && this.canShoot && !this.attacking && !this.spawning && !this.teleporting && (double) UnityEngine.Random.value < 0.30000001192092896)
      this.ShootProjectile();
    if (GameManager.RoomActive && (bool) (UnityEngine.Object) this.gm && (double) this.gm.CurrentTime > (double) this.lastProjectileSwirlShootingTime && this.canShootSwirl && !this.attacking && !this.spawning && !this.teleporting && (double) UnityEngine.Random.value < 0.30000001192092896)
      this.ShootSwirl();
    if (GameManager.RoomActive && (bool) (UnityEngine.Object) this.gm && (double) this.gm.CurrentTime > (double) this.lastDropBombTime && this.canDropBombs && !this.attacking && !this.spawning && !this.teleporting && (double) UnityEngine.Random.value < 0.30000001192092896)
      this.SpawnTrapBombs();
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

  public override void UpdateMoving()
  {
    if (this.attacking || this.spawning || this.teleporting)
      return;
    base.UpdateMoving();
  }

  public void ChargeAttack()
  {
    if (this.attacking || this.spawning)
      return;
    this.charging = true;
    this.attackTimer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Charging;
    this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", this.transform.position);
  }

  public void SpawnEnemy()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting || this.spawnableEnemies.Length == 0 || this.spawnedAmount >= this.maxEnemiesActive || (double) this.targetMinSpawnRadius != 0.0 && (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) <= (double) this.targetMinSpawnRadius)
      return;
    this.spawnRoutine = this.StartCoroutine((IEnumerator) this.SpawnDelay());
  }

  public IEnumerator SpawnDelay()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    enemyJellySpawner.ClearPaths();
    enemyJellySpawner.spawning = true;
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.anticipationAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellySpawner.Spine.timeScale) < (double) enemyJellySpawner.spawnDuration)
      yield return (object) null;
    int num = UnityEngine.Random.Range(enemyJellySpawner.spawnAmountMin, enemyJellySpawner.spawnAmountMax + 1);
    UnityEngine.Random.Range(0, 360);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser_boss/chaser_boss_egg_spawn", enemyJellySpawner.transform.position);
    for (int index = 0; index < num; ++index)
    {
      GameObject Spawn = ObjectPool.Spawn(enemyJellySpawner.loadedAddressableAssets[UnityEngine.Random.Range(0, enemyJellySpawner.spawnableEnemies.Length)].Result, enemyJellySpawner.transform.parent, enemyJellySpawner.GetRandomSpawnPosition(), Quaternion.identity);
      EnemyExploder component1 = Spawn.GetComponent<EnemyExploder>();
      component1.gameObject.SetActive(false);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(Spawn.transform.position, enemyJellySpawner.transform.parent, Spawn);
      component1.health.OnDie += new Health.DieAction(enemyJellySpawner.OnEnemyKilled);
      EnemyRoundsBase.Instance?.AddEnemyToRound(component1.GetComponent<Health>());
      Interaction_Chest.Instance?.AddEnemy(component1.health);
      if ((double) enemyJellySpawner.growSpeed != 0.0)
      {
        component1.Spine.transform.localScale = Vector3.zero;
        component1.Spine.transform.DOScale(1f, enemyJellySpawner.growSpeed).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(enemyJellySpawner.growEase);
      }
      ++enemyJellySpawner.spawnedAmount;
      DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
      if ((bool) (UnityEngine.Object) component2)
        component2.GiveXP = false;
      enemyJellySpawner.spawnedEnemies.Add((UnitObject) component1);
    }
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.spawnAnimation, false);
    enemyJellySpawner.Spine.AnimationState.AddAnimation(0, enemyJellySpawner.idleAnimation, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellySpawner.Spine.timeScale) < (double) enemyJellySpawner.spawnCooldown)
      yield return (object) null;
    enemyJellySpawner.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellySpawner.SetSpawnTime();
    enemyJellySpawner.spawning = false;
    enemyJellySpawner.spawnRoutine = (Coroutine) null;
  }

  public Vector3 GetRandomSpawnPosition()
  {
    return new Vector3(UnityEngine.Random.Range(-6.5f, 6.5f), UnityEngine.Random.Range(-3.5f, 3.5f), 0.0f);
  }

  public void OnEnemyKilled(
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
    if (!this.killSpawnablesOnDeath || !(bool) (UnityEngine.Object) this.GetComponentInParent<MiniBossController>())
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

  public void SetSpawnTime()
  {
    if (!(bool) (UnityEngine.Object) this.gm)
      return;
    this.spawnTime = this.gm.CurrentTime + UnityEngine.Random.Range(this.randomSpawnMinDelay, this.randomSpawnMaxDelay);
  }

  public IEnumerator TeleportIE()
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
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyJellySpawner.Spine.timeScale) < (double) enemyJellySpawner.teleportDelay)
        yield return (object) null;
      enemyJellySpawner.ClearPaths();
      enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.teleportAnimation, false);
      yield return (object) new WaitForEndOfFrame();
      if ((bool) (UnityEngine.Object) enemyJellySpawner.hpBar)
        enemyJellySpawner.hpBar.Hide();
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", enemyJellySpawner.transform.position);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyJellySpawner.Spine.timeScale) < (double) enemyJellySpawner.teleportMoveDelay)
        yield return (object) null;
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
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyJellySpawner.Spine.timeScale) < (double) enemyJellySpawner.teleportCooldownDelay)
        yield return (object) null;
      enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.idleAnimation, true);
      enemyJellySpawner.teleporting = false;
    }
  }

  public IEnumerator TeleportToPositionIE(Vector3 pos, string teleportAnim)
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
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyJellySpawner.Spine.timeScale) < (double) enemyJellySpawner.teleportDelay)
        yield return (object) null;
      enemyJellySpawner.ClearPaths();
      enemyJellySpawner.Spine.AnimationState.SetAnimation(0, teleportAnim, false);
      yield return (object) new WaitForEndOfFrame();
      if ((bool) (UnityEngine.Object) enemyJellySpawner.hpBar)
        enemyJellySpawner.hpBar.Hide();
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", enemyJellySpawner.transform.position);
      bool waiting = true;
      enemyJellySpawner.Spine.AnimationState.Event += (Spine.AnimationState.TrackEntryEventDelegate) ((trackEntry, e) =>
      {
        if (!(e.Data.Name == "teleport"))
          return;
        waiting = false;
      });
      while (waiting || (double) enemyJellySpawner.Spine.timeScale == 9.9999997473787516E-05)
        yield return (object) null;
      enemyJellySpawner.transform.position = pos;
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", enemyJellySpawner.transform.position);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyJellySpawner.Spine.timeScale) < (double) enemyJellySpawner.teleportCooldownDelay)
        yield return (object) null;
      enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.idleAnimation, true);
      enemyJellySpawner.teleporting = false;
    }
  }

  public void Attack()
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

  public IEnumerator AttackCooldownIE()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    yield return (object) new WaitForEndOfFrame();
    enemyJellySpawner.simpleSpineFlash.FlashWhite(false);
    yield return (object) new WaitForSeconds(enemyJellySpawner.attackCooldown);
    enemyJellySpawner.attacking = false;
    enemyJellySpawner.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
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

  public void ShootProjectile()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting)
      return;
    this.StartCoroutine((IEnumerator) this.ProjectilePattern());
  }

  public IEnumerator ProjectilePattern()
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
    yield return (object) enemyJellySpawner.StartCoroutine((IEnumerator) enemyJellySpawner.projectilePattern.ShootIE(0.0f, (GameObject) null, (Transform) null, false));
    enemyJellySpawner.lastProjectileShootingTime = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyJellySpawner.timeBetweenProjectileShots.x, enemyJellySpawner.timeBetweenProjectileShots.y);
    enemyJellySpawner.attacking = false;
  }

  public void SpawnTrapBombs()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting)
      return;
    this.StartCoroutine((IEnumerator) this.DropTrapBombs());
  }

  public IEnumerator DropTrapBombs()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    enemyJellySpawner.attacking = true;
    enemyJellySpawner.ClearPaths();
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.teleportAnimation, false);
    yield return (object) new WaitForEndOfFrame();
    if ((bool) (UnityEngine.Object) enemyJellySpawner.hpBar)
      enemyJellySpawner.hpBar.Hide();
    AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", enemyJellySpawner.transform.position);
    yield return (object) new WaitForSeconds(0.35f);
    enemyJellySpawner.Spine.timeScale = 0.0f;
    yield return (object) new WaitForSeconds(0.5f);
    enemyJellySpawner.health.invincible = true;
    List<Vector3> spawnedPositions = new List<Vector3>();
    for (int i = 0; (double) i < (double) UnityEngine.Random.Range(enemyJellySpawner.bombsToDrop.x, enemyJellySpawner.bombsToDrop.y + 1f); ++i)
    {
      int num = 0;
      while (num++ < 100)
      {
        Vector3 positionInIsland = BiomeGenerator.GetRandomPositionInIsland();
        bool flag = false;
        foreach (Vector3 a in spawnedPositions)
        {
          if ((double) Vector3.Distance(a, positionInIsland) < 2.0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          GameObject gameObject = ObjectPool.Spawn(enemyJellySpawner.trapBomb.Result, enemyJellySpawner.transform.parent, positionInIsland, Quaternion.identity);
          gameObject.transform.localScale = Vector3.zero;
          gameObject.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
          spawnedPositions.Add(positionInIsland);
          break;
        }
      }
      yield return (object) new WaitForSeconds(0.25f);
    }
    yield return (object) new WaitForSeconds(1f);
    enemyJellySpawner.health.invincible = false;
    enemyJellySpawner.Spine.timeScale = 1f;
    enemyJellySpawner.Spine.AnimationState.AddAnimation(0, enemyJellySpawner.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    enemyJellySpawner.lastDropBombTime = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyJellySpawner.timeBetweenDroppingBombs.x, enemyJellySpawner.timeBetweenDroppingBombs.y);
    enemyJellySpawner.attacking = false;
  }

  public void ShootSwirl()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting)
      return;
    this.StartCoroutine((IEnumerator) this.ProjectileSwirlPattern());
  }

  public IEnumerator ProjectileSwirlPattern()
  {
    EnemyJellySpawner enemyJellySpawner = this;
    enemyJellySpawner.attacking = true;
    enemyJellySpawner.ClearPaths();
    yield return (object) enemyJellySpawner.StartCoroutine((IEnumerator) enemyJellySpawner.TeleportToPositionIE(Vector3.zero, enemyJellySpawner.teleportShootAnimation));
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.shootLongAnticipateAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/jellyfish_miniboss/jellyfish_miniboss_charge", enemyJellySpawner.transform.position);
    float t = 0.0f;
    while ((double) t < (double) enemyJellySpawner.projectileSwirlAnticipation / (double) enemyJellySpawner.Spine.timeScale)
    {
      t += Time.deltaTime;
      enemyJellySpawner.simpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyJellySpawner.projectileSwirlAnticipation * 0.75));
      yield return (object) null;
    }
    enemyJellySpawner.simpleSpineFlash.FlashWhite(false);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", enemyJellySpawner.transform.position);
    enemyJellySpawner.Spine.AnimationState.SetAnimation(0, enemyJellySpawner.shootLongAnimation, true);
    yield return (object) enemyJellySpawner.StartCoroutine((IEnumerator) enemyJellySpawner.projectileSwirlPattern.ShootIE());
    enemyJellySpawner.Spine.AnimationState.AddAnimation(0, enemyJellySpawner.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    enemyJellySpawner.lastProjectileSwirlShootingTime = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyJellySpawner.timeBetweenProjectileSwirlShots.x, enemyJellySpawner.timeBetweenProjectileSwirlShots.y);
    enemyJellySpawner.attacking = false;
  }

  public new void OnDrawGizmos()
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

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.loadedAddressableAssets != null)
    {
      foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
        Addressables.Release((AsyncOperationHandle) addressableAsset);
      this.loadedAddressableAssets.Clear();
    }
    Addressables.Release<GameObject>(this.trapBomb);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0() => base.Start();
}
