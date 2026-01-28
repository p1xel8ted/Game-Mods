// Decompiled with JetBrains decompiler
// Type: EnemyJuicedWormMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyJuicedWormMiniboss : UnitObject
{
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string movingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string enragedAttackAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string enragedMovingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string enragedFinishedAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string chargeAttackAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string chargeAttackingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string chargeAttackImpactAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string dashAttackAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string dashAttackImpactAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAttackAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spawnEnemiesAnimation;
  [Space]
  [SerializeField]
  public Vector2 timeBetweenAttacks;
  [SerializeField]
  public Vector2 timeBetweenEnemySpawns;
  [SerializeField]
  public float enragedAnticipation;
  [SerializeField]
  public float enragedPost;
  [SerializeField]
  public float enragedDuration;
  [SerializeField]
  public float enragedMaxSpeed;
  [SerializeField]
  public GameObject grenadeBullet;
  [SerializeField]
  public GameObject shootPosition;
  [SerializeField]
  public Vector2 amountOfShots;
  [SerializeField]
  public Vector2 delayBetweenShots;
  [SerializeField]
  public Vector2 shootDistance;
  [SerializeField]
  public float gravSpeed;
  [SerializeField]
  public Vector2 amountOfRounds;
  [SerializeField]
  public float targetedShootingAnticipation;
  [SerializeField]
  public float targetedShootingPost;
  [SerializeField]
  public float minDistanceToTargetShooting;
  [SerializeField]
  public float chargeAnticipation;
  [SerializeField]
  public float chargeMaxSpeed;
  [SerializeField]
  public float chargePost;
  [SerializeField]
  public float minDistanceToStartCharging;
  [SerializeField]
  public float dashAnticipation;
  [SerializeField]
  public float dashDuration;
  [SerializeField]
  public float dashPost;
  [SerializeField]
  public float dashForce;
  [SerializeField]
  public float maxDistanceToDash;
  [SerializeField]
  public Vector2 spawnAmount;
  [SerializeField]
  public int maxEnemies;
  [SerializeField]
  public float spawnEnemiesAnticipation;
  [SerializeField]
  public float spawnEnemiesPost;
  [SerializeField]
  public Vector2 delayBetweenSpawns;
  [SerializeField]
  public AssetReferenceGameObject[] spawnables;
  [SerializeField]
  public float chargeAttackWeight;
  [SerializeField]
  public float targetedShootAttackWeight;
  [SerializeField]
  public float enragedAttackWeight;
  [SerializeField]
  public float dashAttackWeight;
  [SerializeField]
  public float spawnEnemiesWeight;
  [Space]
  [SerializeField]
  public ColliderEvents damageCollider;
  [EventRef]
  public string GetHitVO = string.Empty;
  public float randomDirection;
  public Vector2 distanceRange = new Vector2(3f, 4f);
  public Vector2 idleWaitRange = new Vector2(0.2f, 1.5f);
  public bool moving = true;
  public float originalMaxSpeed;
  public Coroutine currentAttack;
  public SimpleSpineFlash[] simpleSpineFlashes;
  public float roamTime;
  public float spawnTime;
  public float targetTime;
  public bool canBeKnockedBack = true;
  public bool targeting;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public override void Awake()
  {
    base.Awake();
    this.originalMaxSpeed = this.maxSpeed;
    this.simpleSpineFlashes = this.GetComponentsInChildren<SimpleSpineFlash>();
    this.damageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamagedEnemy);
  }

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
        obj.Result.CreatePool(24, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
    this.grenadeBullet.CreatePool(Mathf.CeilToInt(this.amountOfShots.y));
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.damageCollider.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamagedEnemy);
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.currentAttack = (Coroutine) null;
  }

  public override void Update()
  {
    this.roamTime -= Time.deltaTime * this.Spine.timeScale;
    this.spawnTime -= Time.deltaTime * this.Spine.timeScale;
    this.targetTime -= Time.deltaTime * this.Spine.timeScale;
    if ((double) this.targetTime <= 0.0)
    {
      this.targetTime = UnityEngine.Random.Range(0.0f, 5f);
      this.targeting = !this.targeting;
    }
    this.GetNewTargetPosition();
    if (this.moving)
    {
      if (this.UsePathing)
      {
        if (this.pathToFollow == null)
        {
          this.speed += (float) ((0.0 - (double) this.speed) / 20.0) * GameManager.DeltaTime;
          this.move();
          return;
        }
        if (this.currentWaypoint >= this.pathToFollow.Count)
        {
          this.speed += (float) ((0.0 - (double) this.speed) / 20.0) * GameManager.DeltaTime;
          this.move();
          return;
        }
      }
      if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
      {
        this.speed += (float) (((double) this.maxSpeed * (double) this.SpeedMultiplier - (double) this.speed) / 35.0) * GameManager.DeltaTime;
        if (this.UsePathing)
        {
          this.state.facingAngle = Mathf.LerpAngle(this.state.facingAngle, Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]), Time.deltaTime * 2f);
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
        this.speed += (float) ((0.0 - (double) this.speed) / 20.0) * GameManager.DeltaTime;
    }
    this.move();
    if (this.currentAttack != null || !((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null) || (double) this.roamTime > 0.0)
      return;
    if ((double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) <= (double) this.maxDistanceToDash && (double) UnityEngine.Random.value <= (double) this.dashAttackWeight)
      this.DashAttack();
    else if (Health.team2.Count - 1 < this.maxEnemies && (double) this.spawnTime <= 0.0 && (Health.team2.Count - 1 <= 2 || (double) UnityEngine.Random.value <= (double) this.spawnEnemiesWeight))
      this.SpawnEnemies();
    else if ((double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) >= (double) this.minDistanceToTargetShooting && (double) UnityEngine.Random.value <= (double) this.targetedShootAttackWeight)
    {
      this.TargetedShoot();
    }
    else
    {
      if ((double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.minDistanceToStartCharging || (double) UnityEngine.Random.value > (double) this.chargeAttackWeight)
        return;
      this.ChargeAttack();
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if (this.canBeKnockedBack)
      this.DoKnockBack(Attacker, 0.25f, 0.5f);
    foreach (SimpleSpineFlash simpleSpineFlash in this.simpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  public void GetNewTargetPosition()
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      return;
    if (this.targeting & (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
    {
      this.givePath(this.GetClosestTarget().transform.position);
    }
    else
    {
      float num1 = 100f;
      while ((double) --num1 > 0.0)
      {
        float num2 = UnityEngine.Random.Range(this.distanceRange.x, this.distanceRange.y);
        this.randomDirection += (float) UnityEngine.Random.Range(-45, 45) * ((float) Math.PI / 180f);
        float radius = 0.1f;
        Vector3 targetLocation = this.transform.position + new Vector3(num2 * Mathf.Cos(this.randomDirection), num2 * Mathf.Sin(this.randomDirection));
        if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), num2 * 0.5f, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
        {
          this.randomDirection += 0.17453292f;
        }
        else
        {
          this.givePath(targetLocation);
          break;
        }
      }
    }
  }

  public void DisableMovement()
  {
    this.speed = 0.0f;
    this.ClearPaths();
    this.moving = false;
  }

  public void EnragedAttack()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.EnragedAttackIE());
  }

  public IEnumerator EnragedAttackIE()
  {
    EnemyJuicedWormMiniboss juicedWormMiniboss = this;
    juicedWormMiniboss.DisableMovement();
    foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
    {
      simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.enragedAttackAnticipationAnimation, false);
      simpleSpineFlash.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.enragedMovingAnimation, true, 0.0f);
    }
    juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.enragedAttackAnticipationAnimation, false);
    juicedWormMiniboss.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.enragedMovingAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.enragedAnticipation)
      yield return (object) null;
    juicedWormMiniboss.moving = true;
    juicedWormMiniboss.maxSpeed = juicedWormMiniboss.enragedMaxSpeed;
    juicedWormMiniboss.speed = juicedWormMiniboss.maxSpeed;
    juicedWormMiniboss.canBeKnockedBack = false;
    Coroutine shootCoroutine = (Coroutine) null;
    juicedWormMiniboss.idleWaitRange /= 2f;
    juicedWormMiniboss.distanceRange /= 2f;
    float t = 0.0f;
    while ((double) t < (double) juicedWormMiniboss.enragedDuration)
    {
      t += Time.deltaTime;
      juicedWormMiniboss.speed = juicedWormMiniboss.maxSpeed;
      if (shootCoroutine == null)
        shootCoroutine = juicedWormMiniboss.StartCoroutine((IEnumerator) juicedWormMiniboss.ShootIE(false, (System.Action) (() => shootCoroutine = (Coroutine) null)));
      yield return (object) null;
    }
    juicedWormMiniboss.maxSpeed = juicedWormMiniboss.originalMaxSpeed;
    juicedWormMiniboss.DisableMovement();
    juicedWormMiniboss.canBeKnockedBack = true;
    juicedWormMiniboss.idleWaitRange *= 2f;
    juicedWormMiniboss.distanceRange *= 2f;
    foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
    {
      simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.enragedFinishedAnimation, false);
      simpleSpineFlash.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
    }
    juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.enragedFinishedAnimation, false);
    juicedWormMiniboss.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.enragedPost)
      yield return (object) null;
    juicedWormMiniboss.moving = true;
    juicedWormMiniboss.currentAttack = (Coroutine) null;
    juicedWormMiniboss.roamTime = UnityEngine.Random.Range(juicedWormMiniboss.timeBetweenAttacks.x, juicedWormMiniboss.timeBetweenAttacks.y);
  }

  public void TargetedShoot()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.TargetedShootIE());
  }

  public IEnumerator TargetedShootIE()
  {
    EnemyJuicedWormMiniboss juicedWormMiniboss = this;
    juicedWormMiniboss.DisableMovement();
    float time = 0.0f;
    int randomAmount = (int) UnityEngine.Random.Range(juicedWormMiniboss.amountOfRounds.x, juicedWormMiniboss.amountOfRounds.y + 1f);
    for (int i = 0; i < randomAmount; ++i)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
        simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.shootAttackAnticipationAnimation, true);
      juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.shootAttackAnticipationAnimation, true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.targetedShootingAnticipation)
        yield return (object) null;
      foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
      {
        simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.shootAttackAnimation, false);
        simpleSpineFlash.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
      }
      juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.shootAttackAnimation, false);
      juicedWormMiniboss.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
      yield return (object) juicedWormMiniboss.StartCoroutine((IEnumerator) juicedWormMiniboss.ShootIE(true, (System.Action) null));
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.targetedShootingPost)
      yield return (object) null;
    juicedWormMiniboss.moving = true;
    juicedWormMiniboss.currentAttack = (Coroutine) null;
    juicedWormMiniboss.roamTime = UnityEngine.Random.Range(juicedWormMiniboss.timeBetweenAttacks.x, juicedWormMiniboss.timeBetweenAttacks.y);
  }

  public void Shoot() => this.StartCoroutine((IEnumerator) this.ShootIE(false, (System.Action) null));

  public IEnumerator ShootIE(bool shootAtTarget, System.Action callback)
  {
    EnemyJuicedWormMiniboss juicedWormMiniboss = this;
    int shotsToFire = (int) UnityEngine.Random.Range(juicedWormMiniboss.amountOfShots.x, juicedWormMiniboss.amountOfShots.y + 1f);
    int i = -1;
    while (++i < shotsToFire)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", juicedWormMiniboss.transform.position);
      Vector3 position = juicedWormMiniboss.shootPosition.transform.position with
      {
        z = 0.0f
      };
      float Speed = UnityEngine.Random.Range(juicedWormMiniboss.shootDistance.x, juicedWormMiniboss.shootDistance.y);
      float Angle = UnityEngine.Random.Range(0.0f, 360f);
      if (shootAtTarget && (UnityEngine.Object) juicedWormMiniboss.GetClosestTarget() != (UnityEngine.Object) null)
      {
        Speed = Vector3.Distance(juicedWormMiniboss.transform.position, juicedWormMiniboss.GetClosestTarget().transform.position) / 1.5f + UnityEngine.Random.Range(-2f, 2f);
        Angle = Utils.GetAngle(juicedWormMiniboss.transform.position, juicedWormMiniboss.GetClosestTarget().transform.position) + UnityEngine.Random.Range(-20f, 20f);
      }
      GrenadeBullet component = ObjectPool.Spawn(juicedWormMiniboss.grenadeBullet, position).GetComponent<GrenadeBullet>();
      component.SetOwner(juicedWormMiniboss.gameObject);
      component.Play(-2f, Angle, Speed, juicedWormMiniboss.gravSpeed);
      float dur = UnityEngine.Random.Range(juicedWormMiniboss.delayBetweenShots.x, juicedWormMiniboss.delayBetweenShots.y);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void ChargeAttack()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.ChargeAttackIE());
  }

  public IEnumerator ChargeAttackIE()
  {
    EnemyJuicedWormMiniboss juicedWormMiniboss = this;
    juicedWormMiniboss.DisableMovement();
    foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
    {
      simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.chargeAttackAnticipationAnimation, false);
      simpleSpineFlash.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.chargeAttackingAnimation, true, 0.0f);
    }
    juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.chargeAttackAnticipationAnimation, false);
    juicedWormMiniboss.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.chargeAttackingAnimation, true, 0.0f);
    float progress = 0.0f;
    while ((double) (progress += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.chargeAnticipation)
    {
      foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
        simpleSpineFlash.FlashWhite(progress / juicedWormMiniboss.chargeAnticipation);
      yield return (object) null;
    }
    foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
      simpleSpineFlash.FlashWhite(false);
    Health closestTarget = juicedWormMiniboss.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
    {
      juicedWormMiniboss.currentAttack = (Coroutine) null;
    }
    else
    {
      Vector3 dir = (closestTarget.transform.position - juicedWormMiniboss.transform.position).normalized;
      float angle = Utils.GetAngle(Vector3.zero, dir);
      juicedWormMiniboss.state.facingAngle = angle;
      juicedWormMiniboss.state.LookAngle = angle;
      juicedWormMiniboss.maxSpeed = juicedWormMiniboss.chargeMaxSpeed;
      juicedWormMiniboss.speed = juicedWormMiniboss.maxSpeed;
      juicedWormMiniboss.moving = false;
      juicedWormMiniboss.canBeKnockedBack = false;
      juicedWormMiniboss.damageCollider.SetActive(true);
      while (!((UnityEngine.Object) Physics2D.Raycast((Vector2) juicedWormMiniboss.transform.position, (Vector2) dir, 1f, (int) juicedWormMiniboss.layerToCheck).collider != (UnityEngine.Object) null))
      {
        juicedWormMiniboss.move();
        yield return (object) null;
      }
      juicedWormMiniboss.damageCollider.SetActive(false);
      juicedWormMiniboss.DisableMovement();
      juicedWormMiniboss.canBeKnockedBack = true;
      AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_land", juicedWormMiniboss.transform.position);
      foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
      {
        simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.chargeAttackImpactAnimation, false);
        simpleSpineFlash.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
      }
      juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.chargeAttackImpactAnimation, false);
      juicedWormMiniboss.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.chargePost)
        yield return (object) null;
      juicedWormMiniboss.maxSpeed = juicedWormMiniboss.originalMaxSpeed;
      juicedWormMiniboss.moving = true;
      juicedWormMiniboss.currentAttack = (Coroutine) null;
      juicedWormMiniboss.roamTime = UnityEngine.Random.Range(juicedWormMiniboss.timeBetweenAttacks.x, juicedWormMiniboss.timeBetweenAttacks.y);
    }
  }

  public void SpawnEnemies()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.SpawnEnemiesIE());
  }

  public IEnumerator SpawnEnemiesIE()
  {
    EnemyJuicedWormMiniboss juicedWormMiniboss = this;
    juicedWormMiniboss.DisableMovement();
    foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
    {
      simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.spawnEnemiesAnimation, false);
      simpleSpineFlash.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
    }
    juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.spawnEnemiesAnimation, false);
    juicedWormMiniboss.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.spawnEnemiesAnticipation)
      yield return (object) null;
    int amount = (int) UnityEngine.Random.Range(juicedWormMiniboss.spawnAmount.x, juicedWormMiniboss.spawnAmount.y + 1f);
    for (int i = 0; i < amount; ++i)
    {
      Vector3 position = (Vector3) (UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(2.5f, 5f));
      UnitObject component = ObjectPool.Spawn(juicedWormMiniboss.loadedAddressableAssets[UnityEngine.Random.Range(0, juicedWormMiniboss.loadedAddressableAssets.Count)].Result, juicedWormMiniboss.transform.parent, position, Quaternion.identity).GetComponent<UnitObject>();
      component.CanHaveModifier = false;
      component.RemoveModifier();
      float dur = UnityEngine.Random.Range(juicedWormMiniboss.delayBetweenSpawns.x, juicedWormMiniboss.delayBetweenSpawns.y);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.spawnEnemiesPost)
      yield return (object) null;
    juicedWormMiniboss.moving = true;
    juicedWormMiniboss.currentAttack = (Coroutine) null;
    juicedWormMiniboss.roamTime = UnityEngine.Random.Range(juicedWormMiniboss.timeBetweenAttacks.x, juicedWormMiniboss.timeBetweenAttacks.y);
    juicedWormMiniboss.spawnTime = UnityEngine.Random.Range(juicedWormMiniboss.timeBetweenEnemySpawns.x, juicedWormMiniboss.timeBetweenEnemySpawns.y);
  }

  public void DashAttack()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.DashAttackIE());
  }

  public virtual IEnumerator DashAttackIE()
  {
    EnemyJuicedWormMiniboss juicedWormMiniboss = this;
    juicedWormMiniboss.DisableMovement();
    foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
      simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.dashAttackAnticipationAnimation, true);
    juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.dashAttackAnticipationAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", juicedWormMiniboss.transform.position);
    Health closestTarget = juicedWormMiniboss.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
    {
      juicedWormMiniboss.currentAttack = (Coroutine) null;
    }
    else
    {
      if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
      {
        juicedWormMiniboss.state.LookAngle = Utils.GetAngle(juicedWormMiniboss.transform.position, closestTarget.transform.position);
        juicedWormMiniboss.state.facingAngle = juicedWormMiniboss.state.LookAngle;
      }
      float progress = 0.0f;
      while ((double) (progress += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.dashAnticipation)
      {
        foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
          simpleSpineFlash.FlashWhite(progress / juicedWormMiniboss.dashAnticipation);
        yield return (object) null;
      }
      foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
        simpleSpineFlash.FlashWhite(false);
      Health health = juicedWormMiniboss.GetClosestTarget();
      if ((UnityEngine.Object) health == (UnityEngine.Object) null)
        health = juicedWormMiniboss.health;
      juicedWormMiniboss.state.LookAngle = Utils.GetAngle(juicedWormMiniboss.transform.position, health.transform.position);
      juicedWormMiniboss.state.facingAngle = juicedWormMiniboss.state.LookAngle;
      juicedWormMiniboss.DoKnockBack(health.gameObject, juicedWormMiniboss.dashForce * -1f, 1f);
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", juicedWormMiniboss.transform.position);
      foreach (SimpleSpineFlash simpleSpineFlash in juicedWormMiniboss.simpleSpineFlashes)
      {
        simpleSpineFlash.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.dashAttackImpactAnimation, false);
        simpleSpineFlash.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
      }
      juicedWormMiniboss.Spine.AnimationState.SetAnimation(0, juicedWormMiniboss.dashAttackImpactAnimation, false);
      juicedWormMiniboss.Spine.AnimationState.AddAnimation(0, juicedWormMiniboss.idleAnimation, true, 0.0f);
      juicedWormMiniboss.StartCoroutine((IEnumerator) juicedWormMiniboss.EnabledDamageCollider(juicedWormMiniboss.dashDuration / 1.5f));
      juicedWormMiniboss.canBeKnockedBack = false;
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.dashDuration)
        yield return (object) null;
      juicedWormMiniboss.canBeKnockedBack = true;
      time = 0.0f;
      while ((double) (time += Time.deltaTime * juicedWormMiniboss.Spine.timeScale) < (double) juicedWormMiniboss.dashPost)
        yield return (object) null;
      juicedWormMiniboss.DisableForces = false;
      juicedWormMiniboss.moving = true;
      juicedWormMiniboss.currentAttack = (Coroutine) null;
      juicedWormMiniboss.roamTime = UnityEngine.Random.Range(juicedWormMiniboss.timeBetweenAttacks.x, juicedWormMiniboss.timeBetweenAttacks.y);
    }
  }

  public IEnumerator EnabledDamageCollider(float duration)
  {
    this.damageCollider.SetActive(true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) duration)
      yield return (object) null;
    this.damageCollider.SetActive(false);
  }

  public void OnDamagedEnemy(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component != (UnityEngine.Object) this.health) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.8f));
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__67_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(24, true);
  }
}
