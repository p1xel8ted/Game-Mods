// Decompiled with JetBrains decompiler
// Type: EnemyBatBurp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyBatBurp : EnemyBat
{
  public int ShotsToFire = 10;
  public float DetectEnemyRange = 10f;
  public GameObject projectilePrefab;
  public float LookAngle;
  public float lastBurpedFliesTimestamp;
  public float minTimeBetweenBurps = 7f;
  public float chargingTimestamp;
  public float chargingDuration = 0.5f;
  [SerializeField]
  public bool projectileTrail;
  [SerializeField]
  public GameObject bulletPrefab;
  [SerializeField]
  public LayerMask wallMask;
  [SerializeField]
  public float projectileSpeed;
  [SerializeField]
  public float projectileAcceleration;
  [SerializeField]
  public float projectileMoveSpeed;
  [SerializeField]
  public float distanceBetweenProjectiles;
  [SerializeField]
  public float timeBetweenProjectileTrails;
  [SerializeField]
  public bool spawning;
  [SerializeField]
  public float timeBetweenSpawning;
  [SerializeField]
  public Vector2 spawningAmount;
  [SerializeField]
  public int maxEnemies;
  [SerializeField]
  public float spawnForce;
  [SerializeField]
  public AssetReferenceGameObject[] spawnables;
  [SerializeField]
  public bool flyAcrossScreen;
  [SerializeField]
  public float flyAcrossScreenSpeed;
  [SerializeField]
  public Vector2 flyAcrossScreenAmount;
  [SerializeField]
  public float timeBetweenFlyAcrossScreenAmount;
  public ParticleSystem flyParticles;
  public float lastProjectileTrailTime;
  public float lastSurroundingFliesTime;
  public float lastFlyAcrossScreenTime;
  public bool fleeing;
  public List<Projectile> activeProjectiles = new List<Projectile>();
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public override void OnEnable()
  {
    base.OnEnable();
    this.health.invincible = false;
  }

  public override void Awake() => base.Awake();

  public IEnumerator Start()
  {
    yield return (object) null;
    this.Preload();
  }

  public void Preload()
  {
    for (int index = 0; index < this.spawnables.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.spawnables[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        obj.Result.CreatePool(16 /*0x10*/, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
    this.projectilePrefab.CreatePool(this.ShotsToFire);
  }

  public override IEnumerator ActiveRoutine()
  {
    EnemyBatBurp enemyBatBurp = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      float turningSpeed = enemyBatBurp.turningSpeed;
      if (!enemyBatBurp.ChasingPlayer)
      {
        enemyBatBurp.state.LookAngle = enemyBatBurp.state.facingAngle;
        if (GameManager.RoomActive && (UnityEngine.Object) enemyBatBurp.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.GetClosestTarget().transform.position) < (double) enemyBatBurp.noticePlayerDistance)
        {
          if (!enemyBatBurp.NoticedPlayer)
          {
            if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
              AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
            enemyBatBurp.warningIcon.AnimationState.SetAnimation(0, "warn-start", false);
            enemyBatBurp.warningIcon.AnimationState.AddAnimation(0, "warn-stop", false, 2f);
            enemyBatBurp.NoticedPlayer = true;
          }
          enemyBatBurp.maxSpeed = enemyBatBurp.ChaseSpeed;
          enemyBatBurp.ChasingPlayer = true;
        }
      }
      else
      {
        if (!enemyBatBurp.fleeing)
        {
          if ((UnityEngine.Object) enemyBatBurp.GetClosestTarget() == (UnityEngine.Object) null || (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.GetClosestTarget().transform.position) > 12.0)
          {
            enemyBatBurp.TargetPosition = enemyBatBurp.StartingPosition.Value;
            enemyBatBurp.maxSpeed = enemyBatBurp.IdleSpeed;
            enemyBatBurp.ChasingPlayer = false;
          }
          else
            enemyBatBurp.TargetPosition = enemyBatBurp.GetClosestTarget().transform.position;
        }
        enemyBatBurp.state.LookAngle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
        if ((double) (enemyBatBurp.AttackCoolDown -= Time.deltaTime) < 0.0)
        {
          if (!enemyBatBurp.ShouldStartCharging() || (double) UnityEngine.Random.value <= 0.5)
          {
            if (!enemyBatBurp.ShouldTrail() || (double) UnityEngine.Random.value <= 0.5)
            {
              if (!enemyBatBurp.ShouldAttack() || (double) UnityEngine.Random.value <= 0.5)
              {
                if (!enemyBatBurp.ShouldSpawnEnemiess() || (double) UnityEngine.Random.value <= 0.5)
                {
                  if (enemyBatBurp.ShouldFlyAcrossScreenAttack() && (double) UnityEngine.Random.value > 0.5)
                    goto label_22;
                }
                else
                  goto label_20;
              }
              else
                goto label_18;
            }
            else
              goto label_16;
          }
          else
            break;
        }
      }
      enemyBatBurp.Angle = Mathf.LerpAngle(enemyBatBurp.Angle, Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition), Time.deltaTime * turningSpeed);
      if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null && (double) enemyBatBurp.angleNoiseAmplitude > 0.0 && (double) enemyBatBurp.angleNoiseFrequency > 0.0 && (double) Vector3.Distance(enemyBatBurp.TargetPosition, enemyBatBurp.transform.position) < (double) enemyBatBurp.MaximumRange)
        enemyBatBurp.Angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(enemyBatBurp.timestamp) * enemyBatBurp.angleNoiseFrequency, 0.0f) - 0.5f) * enemyBatBurp.angleNoiseAmplitude * (float) enemyBatBurp.RanDirection;
      if (!enemyBatBurp.useAcceleration)
        enemyBatBurp.speed = enemyBatBurp.maxSpeed * enemyBatBurp.SpeedMultiplier;
      enemyBatBurp.state.facingAngle = enemyBatBurp.Angle;
      yield return (object) null;
    }
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ChargingRoutine());
    yield break;
label_16:
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ProjectileTrailRoutine());
    yield break;
label_18:
    enemyBatBurp.CurrentAttackNum = 0;
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.AttackRoutine());
    yield break;
label_20:
    enemyBatBurp.CurrentAttackNum = 0;
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.SpawnEnemuesIE());
    yield break;
label_22:
    enemyBatBurp.CurrentAttackNum = 0;
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.FlyAcrossScreenAttackIE());
  }

  public override IEnumerator ChargingRoutine()
  {
    EnemyBatBurp enemyBatBurp = this;
    if (!((UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null))
    {
      if (enemyBatBurp.Spine.AnimationState != null)
      {
        enemyBatBurp.Spine.AnimationState.SetAnimation(0, "burpcharge", true);
        if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
          AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
      }
      enemyBatBurp.chargingTimestamp = GameManager.GetInstance().CurrentTime;
      while ((double) GameManager.GetInstance().TimeSince(enemyBatBurp.chargingTimestamp) < (double) enemyBatBurp.chargingDuration)
      {
        enemyBatBurp.SimpleSpineFlash.FlashMeWhite();
        enemyBatBurp.speed = Mathf.Lerp(enemyBatBurp.speed, enemyBatBurp.IdleSpeed, Time.deltaTime * 10f);
        enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
        yield return (object) null;
      }
      enemyBatBurp.SimpleSpineFlash.FlashWhite(false);
      if (enemyBatBurp.Spine.AnimationState != null)
      {
        enemyBatBurp.Spine.AnimationState.SetAnimation(0, "burp", true);
        enemyBatBurp.Spine.AnimationState.AddAnimation(0, enemyBatBurp.IdleAnimation, true, 0.0f);
      }
      yield return (object) enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ShootProjectileRoutine());
      enemyBatBurp.AttackCoolDown = UnityEngine.Random.Range(enemyBatBurp.AttackCoolDownDuration.x, enemyBatBurp.AttackCoolDownDuration.y);
      enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ActiveRoutine());
    }
  }

  public IEnumerator ShootProjectileRoutine()
  {
    EnemyBatBurp enemyBatBurp = this;
    enemyBatBurp.speed = enemyBatBurp.IdleSpeed;
    enemyBatBurp.lastBurpedFliesTimestamp = GameManager.GetInstance().CurrentTime;
    enemyBatBurp.AttackCoolDown = UnityEngine.Random.Range(enemyBatBurp.AttackCoolDownDuration.x, enemyBatBurp.AttackCoolDownDuration.y);
    CameraManager.shakeCamera(0.2f, enemyBatBurp.LookAngle);
    List<float> shootAngles = new List<float>(enemyBatBurp.ShotsToFire);
    for (int index = 0; index < enemyBatBurp.ShotsToFire; ++index)
      shootAngles.Add(360f / (float) enemyBatBurp.ShotsToFire * (float) index);
    shootAngles.Shuffle<float>();
    float initAngle = UnityEngine.Random.Range(0.0f, 360f);
    for (int i = 0; i < shootAngles.Count; ++i)
    {
      Projectile component = ObjectPool.Spawn(enemyBatBurp.projectilePrefab, enemyBatBurp.transform.parent).GetComponent<Projectile>();
      component.UseDelay = true;
      component.CollideOnlyTarget = true;
      component.transform.position = enemyBatBurp.transform.position;
      component.Angle = initAngle + shootAngles[i];
      component.team = enemyBatBurp.health.team;
      component.Speed += UnityEngine.Random.Range(-0.5f, 0.5f);
      component.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
      component.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
      component.LifeTime += UnityEngine.Random.Range(0.0f, 0.3f);
      component.Owner = enemyBatBurp.health;
      component.SetTarget((UnityEngine.Object) enemyBatBurp.closestPlayerFarming != (UnityEngine.Object) null ? (Health) enemyBatBurp.closestPlayerFarming.health : (Health) PlayerFarming.Instance.health);
      enemyBatBurp.activeProjectiles.Add(component);
      yield return (object) new WaitForSeconds(0.03f);
    }
    yield return (object) new WaitForSeconds(0.3f);
  }

  public bool ShouldTrail()
  {
    return (double) GameManager.GetInstance().TimeSince(this.lastProjectileTrailTime) >= (double) this.timeBetweenProjectileTrails && this.projectileTrail;
  }

  public IEnumerator ProjectileTrailRoutine()
  {
    EnemyBatBurp enemyBatBurp = this;
    enemyBatBurp.Attacking = true;
    enemyBatBurp.fleeing = true;
    enemyBatBurp.maxSpeed = enemyBatBurp.projectileMoveSpeed * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.TargetPosition = enemyBatBurp.GetPositionAwayFromPlayer();
    enemyBatBurp.lastProjectileTrailTime = GameManager.GetInstance().CurrentTime;
    float t = 0.0f;
    while ((double) t < 2.0 && (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition) > 1.0)
    {
      t += Time.deltaTime;
      enemyBatBurp.state.facingAngle = enemyBatBurp.state.LookAngle = enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
      yield return (object) null;
    }
    enemyBatBurp.KnockbackForceModifier = 0.0f;
    enemyBatBurp.TargetPosition = enemyBatBurp.TargetPosition * -1f;
    enemyBatBurp.state.facingAngle = enemyBatBurp.state.LookAngle = enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
    if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
    enemyBatBurp.Spine.AnimationState.SetAnimation(0, "attackcharge", false);
    enemyBatBurp.Spine.AnimationState.AddAnimation(0, "attack", true, 0.0f);
    enemyBatBurp.Spine.AnimationState.AddAnimation(0, "Fly", true, 0.0f);
    enemyBatBurp.maxSpeed = 0.0f;
    t = 0.0f;
    while ((double) t < 1.1000000238418579)
    {
      t += Time.deltaTime;
      enemyBatBurp.SimpleSpineFlash.FlashWhite((float) ((double) t / 1.1000000238418579 * 0.75));
      yield return (object) null;
    }
    enemyBatBurp.Spine.timeScale = 0.5f;
    enemyBatBurp.maxSpeed = enemyBatBurp.projectileMoveSpeed * 2f * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.SimpleSpineFlash.FlashWhite(false);
    Vector2 previousSpawnPosition = (Vector2) enemyBatBurp.transform.position;
    t = 0.0f;
    while ((double) t < 2.0 && (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition) > 1.0)
    {
      t += Time.deltaTime;
      enemyBatBurp.state.facingAngle = enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
      if ((double) Vector3.Distance(enemyBatBurp.transform.position, (Vector3) previousSpawnPosition) > (double) enemyBatBurp.distanceBetweenProjectiles)
      {
        for (int index = 0; index < 2; ++index)
        {
          Projectile component = ObjectPool.Spawn(enemyBatBurp.bulletPrefab, enemyBatBurp.transform.parent).GetComponent<Projectile>();
          component.transform.position = enemyBatBurp.transform.position;
          component.Angle = Utils.Repeat(enemyBatBurp.state.facingAngle + (float) (45 * (index == 0 ? -1 : 1)), 360f);
          component.team = enemyBatBurp.health.team;
          component.Speed = enemyBatBurp.projectileSpeed;
          component.Acceleration = enemyBatBurp.projectileAcceleration;
          component.LifeTime = 4f + UnityEngine.Random.Range(0.0f, 0.3f);
          component.Owner = enemyBatBurp.health;
        }
        AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", enemyBatBurp.transform.position);
        previousSpawnPosition = (Vector2) enemyBatBurp.transform.position;
      }
      enemyBatBurp.state.facingAngle = enemyBatBurp.state.LookAngle = enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
      yield return (object) null;
    }
    enemyBatBurp.Spine.timeScale = 1f;
    enemyBatBurp.maxSpeed = enemyBatBurp.IdleSpeed * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.KnockbackForceModifier = 1f;
    enemyBatBurp.Attacking = false;
    enemyBatBurp.AttackCoolDown = UnityEngine.Random.Range(enemyBatBurp.AttackCoolDownDuration.x, enemyBatBurp.AttackCoolDownDuration.y);
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ActiveRoutine());
  }

  public bool ShouldSpawnEnemiess()
  {
    return (double) GameManager.GetInstance().TimeSince(this.lastSurroundingFliesTime) >= (double) this.timeBetweenSpawning && this.spawning && Health.team2.Count - 1 < this.maxEnemies;
  }

  public void SpawnEnemies() => this.StartCoroutine((IEnumerator) this.SpawnEnemuesIE());

  public IEnumerator SpawnEnemuesIE()
  {
    EnemyBatBurp enemyBatBurp = this;
    if (!((UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null))
    {
      enemyBatBurp.lastSurroundingFliesTime = GameManager.GetInstance().CurrentTime;
      if (enemyBatBurp.Spine.AnimationState != null)
      {
        enemyBatBurp.Spine.AnimationState.SetAnimation(0, "burpcharge", true);
        if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
          AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
      }
      enemyBatBurp.chargingTimestamp = GameManager.GetInstance().CurrentTime;
      while ((double) GameManager.GetInstance().TimeSince(enemyBatBurp.chargingTimestamp) < (double) enemyBatBurp.chargingDuration)
      {
        enemyBatBurp.SimpleSpineFlash.FlashMeWhite();
        enemyBatBurp.speed = Mathf.Lerp(enemyBatBurp.speed, enemyBatBurp.IdleSpeed, Time.deltaTime * 10f);
        enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
        yield return (object) null;
      }
      enemyBatBurp.SimpleSpineFlash.FlashWhite(false);
      if (enemyBatBurp.Spine.AnimationState != null)
      {
        enemyBatBurp.Spine.AnimationState.SetAnimation(0, "burp", true);
        enemyBatBurp.Spine.AnimationState.AddAnimation(0, enemyBatBurp.IdleAnimation, true, 0.0f);
      }
      int num = (int) UnityEngine.Random.Range(enemyBatBurp.spawningAmount.x, enemyBatBurp.spawningAmount.y + 1f);
      for (int index = 0; index < num; ++index)
      {
        Vector3 position = enemyBatBurp.transform.position;
        UnitObject component = ObjectPool.Spawn(enemyBatBurp.loadedAddressableAssets[UnityEngine.Random.Range(0, enemyBatBurp.spawnables.Length)].Result, enemyBatBurp.transform.parent, position, Quaternion.identity).GetComponent<UnitObject>();
        component.CanHaveModifier = false;
        component.RemoveModifier();
        component.DoKnockBack(UnityEngine.Random.Range(0.0f, 360f), enemyBatBurp.spawnForce, 0.75f);
      }
      yield return (object) new WaitForSeconds(1f);
      enemyBatBurp.AttackCoolDown = UnityEngine.Random.Range(enemyBatBurp.AttackCoolDownDuration.x, enemyBatBurp.AttackCoolDownDuration.y);
      enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ActiveRoutine());
    }
  }

  public bool ShouldFlyAcrossScreenAttack()
  {
    return (double) GameManager.GetInstance().TimeSince(this.lastFlyAcrossScreenTime) >= (double) this.timeBetweenFlyAcrossScreenAmount && this.flyAcrossScreen;
  }

  public void FlyAcrossScreenAttack()
  {
    this.StartCoroutine((IEnumerator) this.FlyAcrossScreenAttackIE());
  }

  public IEnumerator FlyAcrossScreenAttackIE()
  {
    EnemyBatBurp enemyBatBurp = this;
    enemyBatBurp.Attacking = true;
    enemyBatBurp.fleeing = true;
    enemyBatBurp.KnockbackForceModifier = 0.0f;
    enemyBatBurp.health.ApplyStasisImmunity();
    enemyBatBurp.maxSpeed = enemyBatBurp.projectileMoveSpeed * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.lastFlyAcrossScreenTime = GameManager.GetInstance().CurrentTime;
    bool right = (double) UnityEngine.Random.value > 0.5;
    if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
    enemyBatBurp.Angle = Utils.GetAngle(Vector3.zero, right ? Vector3.right : Vector3.left);
    enemyBatBurp.state.facingAngle = enemyBatBurp.state.LookAngle = enemyBatBurp.Angle;
    enemyBatBurp.Spine.AnimationState.SetAnimation(0, "attackcharge", false);
    enemyBatBurp.Spine.AnimationState.AddAnimation(0, "flyattack", true, 0.0f);
    GameManager.GetInstance().RemoveFromCamera(enemyBatBurp.gameObject);
    if ((bool) (UnityEngine.Object) BiomeGenerator.Instance)
      GameManager.GetInstance().AddToCamera(BiomeGenerator.Instance.CurrentRoom.generateRoom.gameObject);
    enemyBatBurp.maxSpeed = 0.0f;
    float t = 0.0f;
    while ((double) t < 1.1000000238418579)
    {
      t += Time.deltaTime;
      enemyBatBurp.SimpleSpineFlash.FlashWhite((float) ((double) t / 1.1000000238418579 * 0.75));
      yield return (object) null;
    }
    enemyBatBurp.SimpleSpineFlash.FlashWhite(false);
    enemyBatBurp.health.invincible = true;
    enemyBatBurp.maxSpeed = enemyBatBurp.flyAcrossScreenSpeed * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.damageColliderEvents.gameObject.SetActive(true);
    Health targetEnemy = enemyBatBurp.GetClosestTarget();
    targetEnemy = (UnityEngine.Object) targetEnemy == (UnityEngine.Object) null ? (Health) PlayerFarming.Instance.health : targetEnemy;
    enemyBatBurp.TargetPosition = right ? new Vector3(15f, enemyBatBurp.transform.position.y, enemyBatBurp.transform.position.z) : new Vector3(-15f, enemyBatBurp.transform.position.y, enemyBatBurp.transform.position.z);
    t = 0.0f;
    while ((double) t < 4.0 && (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition) > 1.0)
    {
      t += Time.deltaTime;
      enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
      enemyBatBurp.state.facingAngle = enemyBatBurp.state.LookAngle = enemyBatBurp.Angle;
      yield return (object) null;
    }
    int amount = (int) UnityEngine.Random.Range(enemyBatBurp.flyAcrossScreenAmount.x, enemyBatBurp.flyAcrossScreenAmount.y + 1f) + 1;
    for (int i = 0; i < amount; ++i)
    {
      Vector3 vector3 = right ? new Vector3(-15f, targetEnemy.transform.position.y, enemyBatBurp.transform.position.z) : new Vector3(15f, targetEnemy.transform.position.y, enemyBatBurp.transform.position.z);
      enemyBatBurp.TargetPosition = right ? new Vector3(15f, targetEnemy.transform.position.y, enemyBatBurp.transform.position.z) : new Vector3(-15f, targetEnemy.transform.position.y, enemyBatBurp.transform.position.z);
      enemyBatBurp.transform.position = vector3;
      if (i == amount - 1)
        enemyBatBurp.TargetPosition = new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y, enemyBatBurp.transform.position.z);
      t = 0.0f;
      while ((double) t < 4.0 && (double) Vector3.Distance(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition) > 1.0)
      {
        t += Time.deltaTime;
        enemyBatBurp.Angle = Utils.GetAngle(enemyBatBurp.transform.position, enemyBatBurp.TargetPosition);
        enemyBatBurp.state.facingAngle = enemyBatBurp.state.LookAngle = enemyBatBurp.Angle;
        yield return (object) null;
      }
    }
    enemyBatBurp.damageColliderEvents.gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(enemyBatBurp.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyBatBurp.WarningVO, enemyBatBurp.gameObject);
    enemyBatBurp.Spine.AnimationState.SetAnimation(0, "flyattack-stop", false);
    enemyBatBurp.Spine.AnimationState.AddAnimation(0, "Fly", true, 0.0f);
    enemyBatBurp.health.ClearStasisImmunity();
    enemyBatBurp.health.invincible = false;
    t = 0.0f;
    while ((double) t < 0.20000000298023224)
    {
      enemyBatBurp.speed = Mathf.Lerp(enemyBatBurp.maxSpeed, 0.0f, t / 0.2f);
      t += Time.deltaTime;
      yield return (object) null;
    }
    if ((bool) (UnityEngine.Object) BiomeGenerator.Instance)
      GameManager.GetInstance().RemoveFromCamera(BiomeGenerator.Instance.CurrentRoom.generateRoom.gameObject);
    GameManager.GetInstance().AddToCamera(enemyBatBurp.gameObject);
    enemyBatBurp.KnockbackForceModifier = 1f;
    enemyBatBurp.Attacking = false;
    enemyBatBurp.fleeing = false;
    t = 0.0f;
    while ((double) t < 3.0)
    {
      enemyBatBurp.speed = 0.0f;
      enemyBatBurp.maxSpeed = 0.0f;
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyBatBurp.maxSpeed = enemyBatBurp.IdleSpeed * enemyBatBurp.SpeedMultiplier;
    enemyBatBurp.StartCoroutine((IEnumerator) enemyBatBurp.ActiveRoutine());
  }

  public Vector3 GetPositionAwayFromPlayer()
  {
    Health closestTarget = this.GetClosestTarget();
    Health health = (UnityEngine.Object) closestTarget == (UnityEngine.Object) null ? (Health) PlayerFarming.Instance.health : closestTarget;
    List<RaycastHit2D> raycastHit2DList = new List<RaycastHit2D>();
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(1f, 1f), 100f, (int) this.wallMask));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(-1f, 1f), 100f, (int) this.wallMask));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(1f, -1f), 100f, (int) this.wallMask));
    raycastHit2DList.Add(Physics2D.Raycast((Vector2) this.transform.position, new Vector2(-1f, -1f), 100f, (int) this.wallMask));
    RaycastHit2D raycastHit2D = raycastHit2DList[0];
    for (int index = 1; index < raycastHit2DList.Count; ++index)
    {
      if ((double) Vector3.Distance(health.transform.position, (Vector3) raycastHit2DList[index].point) > (double) Vector3.Distance(health.transform.position, (Vector3) raycastHit2D.point) && (double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2DList[index].point) > 2.0)
        raycastHit2D = raycastHit2DList[index];
    }
    return (Vector3) (raycastHit2D.point + ((Vector2) this.transform.position - raycastHit2D.point).normalized);
  }

  public override bool ShouldStartCharging()
  {
    return (double) GameManager.GetInstance().TimeSince(this.lastBurpedFliesTimestamp) >= (double) this.minTimeBetweenBurps && this.IsPlayerNearby();
  }

  public override IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBatBurp enemyBatBurp = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyBatBurp.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyBatBurp.DisableForces = true;
    enemyBatBurp.Angle = Utils.GetAngle(Attacker.transform.position, enemyBatBurp.transform.position) * ((float) Math.PI / 180f);
    enemyBatBurp.Force = (Vector3) new Vector2(500f * Mathf.Cos(enemyBatBurp.Angle), 500f * Mathf.Sin(enemyBatBurp.Angle));
    enemyBatBurp.rb.AddForce((Vector2) enemyBatBurp.Force);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public bool IsPlayerNearby()
  {
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange)
        return true;
    }
    return false;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = 0; index < this.activeProjectiles.Count; ++index)
    {
      if ((UnityEngine.Object) this.activeProjectiles[index] != (UnityEngine.Object) null && this.activeProjectiles[index].gameObject.activeSelf)
        this.activeProjectiles[index].DestroyWithVFX();
    }
    this.activeProjectiles.Clear();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__36_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(16 /*0x10*/, true);
  }
}
