// Decompiled with JetBrains decompiler
// Type: EnemyJuicedMortarHopperMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
public class EnemyJuicedMortarHopperMiniboss : UnitObject
{
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public SpriteRenderer shadow;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootLongerAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootNoAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootLongerNoAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string poisonSplashAnimation;
  [SerializeField]
  public GameObject smallMortarPrefab;
  [SerializeField]
  public int shotsToFireCross;
  [SerializeField]
  public int shotsToFireLine = 4;
  [SerializeField]
  public int shotsToFireAroundMiniboss = 6;
  [SerializeField]
  public float shotsAroundMinibossDistance;
  [SerializeField]
  public float timeBetweenShots;
  [SerializeField]
  public float shootAnticipation;
  [Space]
  [SerializeField]
  public float minBombRange;
  [SerializeField]
  public float maxBombRange;
  [SerializeField]
  public float bombDuration;
  [SerializeField]
  public AnimationCurve hopSpeedCurve;
  [SerializeField]
  public AnimationCurve hopZCurve;
  [SerializeField]
  public float hopZHeight;
  [SerializeField]
  public float hoppingDur = 0.4f;
  [SerializeField]
  public float attackRange;
  [SerializeField]
  public Vector2 timeBetweenJumps;
  [SerializeField]
  public ParticleSystem aoeParticles;
  [SerializeField]
  public float rageHopAnticipation;
  [SerializeField]
  public float rageHopBulletSpeed;
  [SerializeField]
  public float rageHopBulletDeceleration;
  [SerializeField]
  public int rageHopBulletAmount;
  [SerializeField]
  public Vector2 rageHopAmount;
  [SerializeField]
  public Vector2 bigRotationAmountOfShots;
  [SerializeField]
  public float bigRotationTimeBetween;
  [SerializeField]
  public float poisonRadius;
  [SerializeField]
  public float poisonAnticipation;
  [SerializeField]
  public Vector2 poisonAmount;
  [SerializeField]
  public float megaHopDuration;
  [SerializeField]
  public int megaHopProjectiles;
  [SerializeField]
  public int megaHopProjectilesSpeed;
  [SerializeField]
  public AssetReference enemyToSpawn;
  [SerializeField]
  public int maxEnemies = 3;
  [SerializeField]
  public Vector2 enemiesToSpawn;
  [SerializeField]
  public Vector2 timeBetweenEnemySpawn;
  [SerializeField]
  public float rageHopChance;
  [SerializeField]
  public float poisonSplashChance;
  [SerializeField]
  public float shootMortarChance;
  [SerializeField]
  public float moveChance;
  [SerializeField]
  public float megaHopChance;
  [Space]
  [SerializeField]
  public Vector2 timeBetweenAttacks;
  [SerializeField]
  public ColliderEvents damageColliderEvents;
  [EventRef]
  [SerializeField]
  public string attackVO = string.Empty;
  [EventRef]
  public string OnLandSoundPath = string.Empty;
  [EventRef]
  public string OnJumpSoundPath = string.Empty;
  public Coroutine currentAttack;
  public float idleTime;
  public float spawnTime;
  public bool phase2;
  public int attackIndex = -1;
  public float hoppingTimestamp;
  public bool hasCollidedWithObstacle;
  public float zFallSpeed = 15f;
  public CircleCollider2D collider;
  public AsyncOperationHandle<GameObject> loadedEnemyHandle;
  public bool isInitialized;

  public void Preload()
  {
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.enemyToSpawn);
    asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedEnemyHandle = obj;
      obj.Result.CreatePool(20, true);
    });
    asyncOperationHandle.WaitForCompletion();
    this.isInitialized = true;
  }

  public IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJuicedMortarHopperMiniboss mortarHopperMiniboss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      mortarHopperMiniboss.Preload();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    mortarHopperMiniboss.collider = mortarHopperMiniboss.GetComponent<CircleCollider2D>();
    mortarHopperMiniboss.damageColliderEvents.SetActive(false);
    mortarHopperMiniboss.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(mortarHopperMiniboss.OnDamageTriggerEnter);
    mortarHopperMiniboss.shadow.enabled = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    Addressables.Release<GameObject>(this.loadedEnemyHandle);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.currentAttack = (Coroutine) null;
    this.attackIndex = -1;
    this.health.invincible = false;
    this.state.OnStateChange += new StateMachine.StateChange(this.OnStateChange);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.state.OnStateChange -= new StateMachine.StateChange(this.OnStateChange);
  }

  public override void Update()
  {
    base.Update();
    this.idleTime -= Time.deltaTime * this.Spine.timeScale;
    this.spawnTime -= Time.deltaTime * this.Spine.timeScale;
    if (this.state.CURRENT_STATE != StateMachine.State.Moving)
    {
      if (this.currentAttack == null)
        this.Spine.transform.localPosition = Vector3.Lerp(this.Spine.transform.localPosition, Vector3.zero, Time.deltaTime * this.zFallSpeed);
    }
    else
    {
      this.UpdateStateMoving();
      this.move();
    }
    if (this.state.CURRENT_STATE != StateMachine.State.Moving && this.currentAttack == null && (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) this.idleTime <= 0.0)
    {
      if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
        this.state.facingAngle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
      if (!this.phase2 && (double) this.health.HP < (double) this.health.totalHP / 2.0)
      {
        this.phase2 = true;
        this.timeBetweenAttacks /= 1.5f;
        this.bombDuration /= 1.5f;
        this.maxEnemies = (int) ((double) this.maxEnemies * 1.5);
      }
      if ((double) UnityEngine.Random.value <= (double) this.moveChance && this.phase2)
      {
        float num = UnityEngine.Random.value;
        this.state.facingAngle = this.state.LookAngle = (double) num >= 0.33000001311302185 ? ((double) num >= 0.6600000262260437 ? this.GetFleeAngle() : this.GetRandomFacingAngle()) : this.GetAngleToTarget();
        this.state.CURRENT_STATE = StateMachine.State.Moving;
      }
      else if ((double) UnityEngine.Random.value <= (double) this.shootMortarChance && this.attackIndex != 0)
      {
        this.ShootMortars();
        this.attackIndex = 0;
      }
      else if ((double) UnityEngine.Random.value <= (double) this.poisonSplashChance && (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < 3.0 && TrapPoison.ActivePoison.Count <= 0 && this.attackIndex != 1)
      {
        this.PoisonSplash();
        this.attackIndex = 1;
      }
      else if ((double) UnityEngine.Random.value <= (double) this.rageHopChance && this.attackIndex != 2 && !this.phase2)
      {
        this.RageHop();
        this.attackIndex = 2;
      }
      else if ((double) UnityEngine.Random.value <= (double) this.megaHopChance && this.attackIndex != 3 && this.phase2)
      {
        this.MegaHop();
        this.attackIndex = 3;
      }
    }
    if (!this.isInitialized || (double) this.spawnTime > 0.0 || Health.team2.Count - 1 >= this.maxEnemies)
      return;
    int num1 = (int) UnityEngine.Random.Range(this.enemiesToSpawn.x, this.enemiesToSpawn.y + 1f);
    for (int index = 0; index < num1; ++index)
    {
      GameObject Spawn = ObjectPool.Spawn(this.loadedEnemyHandle.Result, this.transform.parent, (Vector3) (UnityEngine.Random.insideUnitCircle * 4.5f), Quaternion.identity);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(Spawn.transform.position, this.transform.parent, Spawn);
    }
    this.spawnTime = UnityEngine.Random.Range(this.timeBetweenEnemySpawn.x, this.timeBetweenEnemySpawn.y);
  }

  public void ShootMortars()
  {
    if ((double) UnityEngine.Random.value < 0.5 || this.phase2)
      this.LineShootTargeted();
    else
      this.BigRotationShot();
  }

  public void RageHop() => this.currentAttack = this.StartCoroutine((IEnumerator) this.RageHopIE());

  public IEnumerator RageHopIE()
  {
    EnemyJuicedMortarHopperMiniboss mortarHopperMiniboss = this;
    float t = 0.0f;
    while ((double) t < (double) mortarHopperMiniboss.rageHopAnticipation)
    {
      t += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale;
      mortarHopperMiniboss.simpleSpineFlash.FlashWhite((float) ((double) t / (double) mortarHopperMiniboss.rageHopAnticipation * 0.75));
      yield return (object) null;
    }
    mortarHopperMiniboss.simpleSpineFlash.FlashWhite(false);
    mortarHopperMiniboss.state.CURRENT_STATE = StateMachine.State.Aiming;
    float a = 0.0f;
    for (int i = 0; (double) i < (double) UnityEngine.Random.Range(mortarHopperMiniboss.rageHopAmount.x, mortarHopperMiniboss.rageHopAmount.y + 1f); ++i)
    {
      mortarHopperMiniboss.Spine.AnimationState.SetAnimation(0, "jumpcombined", false);
      mortarHopperMiniboss.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < 0.5)
        yield return (object) null;
      CameraManager.instance.ShakeCameraForDuration(1f, 1f, 0.25f);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(mortarHopperMiniboss.transform.position + Vector3.back * 0.5f);
      Projectile.CreateProjectiles(mortarHopperMiniboss.rageHopBulletAmount, mortarHopperMiniboss.health, mortarHopperMiniboss.transform.position, mortarHopperMiniboss.rageHopBulletSpeed, angleOffset: a += 45f, callback: new System.Action<List<Projectile>>(mortarHopperMiniboss.\u003CRageHopIE\u003Eb__72_0));
      time = 0.0f;
      while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < 0.40000000596046448)
        yield return (object) null;
    }
    mortarHopperMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    mortarHopperMiniboss.currentAttack = (Coroutine) null;
    mortarHopperMiniboss.idleTime = UnityEngine.Random.Range(mortarHopperMiniboss.timeBetweenAttacks.x, mortarHopperMiniboss.timeBetweenAttacks.y);
  }

  public IEnumerator SetProjectilePulse(List<Projectile> projectiles)
  {
    yield return (object) new WaitForEndOfFrame();
    foreach (Projectile projectile in projectiles)
    {
      projectile.Deceleration = this.rageHopBulletDeceleration;
      projectile.pulseMove = true;
    }
  }

  public void CrossShoot()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.CrossShootIE());
  }

  public IEnumerator CrossShootIE()
  {
    EnemyJuicedMortarHopperMiniboss mortarHopperMiniboss = this;
    mortarHopperMiniboss.Spine.AnimationState.SetAnimation(0, mortarHopperMiniboss.shootNoAnticipationAnimation, false);
    mortarHopperMiniboss.Spine.AnimationState.AddAnimation(0, mortarHopperMiniboss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) time < (double) mortarHopperMiniboss.shootAnticipation)
    {
      time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale;
      mortarHopperMiniboss.simpleSpineFlash.FlashWhite((float) ((double) time / (double) mortarHopperMiniboss.shootAnticipation * 0.75));
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot(mortarHopperMiniboss.attackVO, mortarHopperMiniboss.gameObject);
    mortarHopperMiniboss.simpleSpineFlash.FlashWhite(false);
    float distance = 1f;
    for (int i = 0; i < mortarHopperMiniboss.shotsToFireCross; ++i)
    {
      float degree = 0.0f;
      for (int index = 0; index < 4; ++index)
      {
        Vector3 position = mortarHopperMiniboss.transform.position + (Vector3) Utils.DegreeToVector2(degree) * distance;
        UnityEngine.Object.Instantiate<GameObject>(mortarHopperMiniboss.smallMortarPrefab, position, Quaternion.identity, mortarHopperMiniboss.transform.parent).GetComponent<MortarBomb>().Play(mortarHopperMiniboss.transform.position + new Vector3(0.0f, 0.0f, -1.5f), mortarHopperMiniboss.bombDuration, Health.Team.Team2);
        degree += 90f;
      }
      distance += 2f;
      time = 0.0f;
      while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < (double) mortarHopperMiniboss.timeBetweenShots * 0.75)
        yield return (object) null;
    }
    mortarHopperMiniboss.currentAttack = (Coroutine) null;
    mortarHopperMiniboss.idleTime = UnityEngine.Random.Range(mortarHopperMiniboss.timeBetweenAttacks.x, mortarHopperMiniboss.timeBetweenAttacks.y);
  }

  public IEnumerator LineShootIE(bool anticipate, float aimAngle, int shotsToFireLine)
  {
    EnemyJuicedMortarHopperMiniboss mortarHopperMiniboss = this;
    float time;
    if (anticipate)
    {
      mortarHopperMiniboss.Spine.AnimationState.SetAnimation(0, mortarHopperMiniboss.shootLongerAnimation, false);
      mortarHopperMiniboss.Spine.AnimationState.AddAnimation(0, mortarHopperMiniboss.idleAnimation, true, 0.0f);
      time = 0.0f;
      while ((double) time < (double) mortarHopperMiniboss.shootAnticipation)
      {
        time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale;
        mortarHopperMiniboss.simpleSpineFlash.FlashWhite((float) ((double) time / (double) mortarHopperMiniboss.shootAnticipation * 0.75));
        yield return (object) null;
      }
      AudioManager.Instance.PlayOneShot(mortarHopperMiniboss.attackVO, mortarHopperMiniboss.gameObject);
      mortarHopperMiniboss.simpleSpineFlash.FlashWhite(false);
    }
    float distance = 1f;
    for (int i = 0; i < shotsToFireLine; ++i)
    {
      Vector3 position = mortarHopperMiniboss.transform.position + (Vector3) Utils.DegreeToVector2(aimAngle) * distance;
      UnityEngine.Object.Instantiate<GameObject>(mortarHopperMiniboss.smallMortarPrefab, position, Quaternion.identity, mortarHopperMiniboss.transform.parent).GetComponent<MortarBomb>().Play(mortarHopperMiniboss.transform.position + new Vector3(0.0f, 0.0f, -1.5f), mortarHopperMiniboss.bombDuration, Health.Team.Team2);
      distance += 2f;
      time = 0.0f;
      while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < (double) mortarHopperMiniboss.timeBetweenShots * 0.75)
        yield return (object) null;
    }
    mortarHopperMiniboss.idleTime = UnityEngine.Random.Range(mortarHopperMiniboss.timeBetweenAttacks.x, mortarHopperMiniboss.timeBetweenAttacks.y);
  }

  public void LineShootTargeted()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.LineShootTargetedIE());
  }

  public IEnumerator LineShootTargetedIE()
  {
    EnemyJuicedMortarHopperMiniboss mortarHopperMiniboss = this;
    yield return (object) mortarHopperMiniboss.StartCoroutine((IEnumerator) mortarHopperMiniboss.LineShootIE(true, (UnityEngine.Object) mortarHopperMiniboss.GetClosestTarget() != (UnityEngine.Object) null ? Utils.GetAngle(mortarHopperMiniboss.transform.position, mortarHopperMiniboss.GetClosestTarget().transform.position) : UnityEngine.Random.Range(0.0f, 360f), mortarHopperMiniboss.shotsToFireLine));
    mortarHopperMiniboss.currentAttack = (Coroutine) null;
  }

  public void BigRotationShot()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.BigRotationShotIE());
  }

  public IEnumerator BigRotationShotIE()
  {
    EnemyJuicedMortarHopperMiniboss mortarHopperMiniboss = this;
    mortarHopperMiniboss.Spine.AnimationState.SetAnimation(0, mortarHopperMiniboss.shootLongerAnimation, false);
    mortarHopperMiniboss.Spine.AnimationState.AddAnimation(0, mortarHopperMiniboss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) time < (double) mortarHopperMiniboss.shootAnticipation)
    {
      time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale;
      mortarHopperMiniboss.simpleSpineFlash.FlashWhite((float) ((double) time / (double) mortarHopperMiniboss.shootAnticipation * 0.75));
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot(mortarHopperMiniboss.attackVO, mortarHopperMiniboss.gameObject);
    mortarHopperMiniboss.simpleSpineFlash.FlashWhite(false);
    float offset = 0.0f;
    int shotsToFire = (int) UnityEngine.Random.Range(mortarHopperMiniboss.bigRotationAmountOfShots.x, mortarHopperMiniboss.bigRotationAmountOfShots.y + 1f);
    for (int i = 0; i < shotsToFire; ++i)
    {
      float aimAngle = offset;
      if (i != 0)
      {
        mortarHopperMiniboss.Spine.AnimationState.SetAnimation(0, mortarHopperMiniboss.shootLongerNoAnticipationAnimation, false);
        mortarHopperMiniboss.Spine.AnimationState.AddAnimation(0, mortarHopperMiniboss.idleAnimation, true, 0.0f);
      }
      for (int index = 0; index < 4; ++index)
      {
        mortarHopperMiniboss.StartCoroutine((IEnumerator) mortarHopperMiniboss.LineShootIE(false, aimAngle, 5));
        aimAngle = Utils.Repeat(aimAngle + 90f, 360f);
      }
      offset += 30f;
      time = 0.0f;
      while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < (double) mortarHopperMiniboss.bigRotationTimeBetween)
        yield return (object) null;
    }
    mortarHopperMiniboss.currentAttack = (Coroutine) null;
    mortarHopperMiniboss.idleTime = UnityEngine.Random.Range(mortarHopperMiniboss.timeBetweenAttacks.x, mortarHopperMiniboss.timeBetweenAttacks.y);
  }

  public void PoisonSplash()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.PoisonSplashIE());
  }

  public IEnumerator PoisonSplashIE()
  {
    EnemyJuicedMortarHopperMiniboss mortarHopperMiniboss = this;
    mortarHopperMiniboss.Spine.AnimationState.SetAnimation(0, mortarHopperMiniboss.poisonSplashAnimation, false);
    mortarHopperMiniboss.Spine.AnimationState.AddAnimation(0, mortarHopperMiniboss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) time < (double) mortarHopperMiniboss.poisonAnticipation)
    {
      time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale;
      mortarHopperMiniboss.simpleSpineFlash.FlashWhite((float) ((double) time / (double) mortarHopperMiniboss.poisonAnticipation * 0.75));
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot(mortarHopperMiniboss.attackVO, mortarHopperMiniboss.gameObject);
    Vector3 position = mortarHopperMiniboss.transform.position;
    mortarHopperMiniboss.simpleSpineFlash.FlashWhite(false);
    int amount = (int) UnityEngine.Random.Range(mortarHopperMiniboss.poisonAmount.x, mortarHopperMiniboss.poisonAmount.y + 1f);
    for (int i = 0; i < amount; ++i)
    {
      float num = UnityEngine.Random.Range(0.0f, mortarHopperMiniboss.poisonRadius);
      TrapPoison.CreatePoison(mortarHopperMiniboss.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * num, 1, 0.0f, mortarHopperMiniboss.transform.parent);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < 0.05000000074505806)
        yield return (object) null;
    }
    mortarHopperMiniboss.currentAttack = (Coroutine) null;
    mortarHopperMiniboss.idleTime = UnityEngine.Random.Range(mortarHopperMiniboss.timeBetweenAttacks.x, mortarHopperMiniboss.timeBetweenAttacks.y);
  }

  public void MegaHop() => this.currentAttack = this.StartCoroutine((IEnumerator) this.MegaHopIE());

  public IEnumerator MegaHopIE()
  {
    EnemyJuicedMortarHopperMiniboss mortarHopperMiniboss = this;
    mortarHopperMiniboss.health.invincible = true;
    mortarHopperMiniboss.health.untouchable = true;
    mortarHopperMiniboss.Spine.AnimationState.SetAnimation(0, "jump-start", false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < 0.60000002384185791)
      yield return (object) null;
    mortarHopperMiniboss.Spine.transform.DOLocalMoveZ(-20f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    mortarHopperMiniboss.shadow.transform.DOScale(3f, 1f);
    mortarHopperMiniboss.shadow.enabled = true;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < 1.0)
      yield return (object) null;
    Vector3 position1 = mortarHopperMiniboss.transform.position;
    Vector3 position2 = mortarHopperMiniboss.GetClosestTarget().transform.position;
    mortarHopperMiniboss.transform.DOMove(position2, mortarHopperMiniboss.megaHopDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < (double) mortarHopperMiniboss.megaHopDuration)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot(mortarHopperMiniboss.attackVO, mortarHopperMiniboss.gameObject);
    mortarHopperMiniboss.shadow.transform.DOScale(5f, 1f);
    mortarHopperMiniboss.Spine.transform.DOLocalMoveZ(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    mortarHopperMiniboss.state.facingAngle = mortarHopperMiniboss.state.LookAngle = Utils.GetAngle(mortarHopperMiniboss.transform.position, mortarHopperMiniboss.GetClosestTarget().transform.position);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < 0.5)
      yield return (object) null;
    mortarHopperMiniboss.Spine.AnimationState.SetAnimation(0, "jump-end-big", false);
    mortarHopperMiniboss.Spine.AnimationState.AddAnimation(0, mortarHopperMiniboss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot(mortarHopperMiniboss.OnLandSoundPath, mortarHopperMiniboss.gameObject);
    AudioManager.Instance.PlayOneShot(mortarHopperMiniboss.attackVO, mortarHopperMiniboss.gameObject);
    mortarHopperMiniboss.StartCoroutine((IEnumerator) mortarHopperMiniboss.TurnOnDamageColliderForDuration(0.1f));
    mortarHopperMiniboss.aoeParticles.Play();
    mortarHopperMiniboss.shadow.enabled = false;
    mortarHopperMiniboss.health.invincible = false;
    mortarHopperMiniboss.health.untouchable = false;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(mortarHopperMiniboss.transform.position + Vector3.back * 0.5f);
    CameraManager.shakeCamera(2f);
    Projectile.CreateProjectiles(mortarHopperMiniboss.megaHopProjectiles, mortarHopperMiniboss.health, mortarHopperMiniboss.transform.position, (float) mortarHopperMiniboss.megaHopProjectilesSpeed);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * mortarHopperMiniboss.Spine.timeScale) < 1.0)
      yield return (object) null;
    mortarHopperMiniboss.currentAttack = (Coroutine) null;
    mortarHopperMiniboss.idleTime = UnityEngine.Random.Range(mortarHopperMiniboss.timeBetweenAttacks.x, mortarHopperMiniboss.timeBetweenAttacks.y);
  }

  public virtual void UpdateStateMoving()
  {
    this.speed = this.hopSpeedCurve.Evaluate((this.hoppingTimestamp - Time.time) / this.hoppingDur);
    if (this.hasCollidedWithObstacle || this.TargetIsInAttackRange())
      this.speed *= 0.5f;
    this.Spine.transform.localPosition = -Vector3.forward * this.hopZCurve.Evaluate((this.hoppingTimestamp - Time.time) / this.hoppingDur) * this.hopZHeight;
    if ((double) Time.time < (double) this.hoppingTimestamp)
      return;
    this.speed = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    AudioManager.Instance.PlayOneShot(this.OnLandSoundPath, this.gameObject);
    AudioManager.Instance.PlayOneShot(this.attackVO, this.gameObject);
    this.StartCoroutine((IEnumerator) this.TurnOnDamageColliderForDuration(0.1f));
    this.aoeParticles.Play();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.back * 0.5f);
    CameraManager.shakeCamera(1f);
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Moving || collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    this.hasCollidedWithObstacle = true;
  }

  public bool TargetIsInAttackRange()
  {
    return GameManager.RoomActive && !((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null) && (double) Vector3.Distance(this.GetClosestTarget().transform.position, this.transform.position) <= (double) this.attackRange * 0.75;
  }

  public virtual void OnStateChange(StateMachine.State newState, StateMachine.State prevState)
  {
    switch (newState)
    {
      case StateMachine.State.Idle:
        if (newState == prevState)
          break;
        this.idleTime = UnityEngine.Random.Range(this.timeBetweenJumps.x, this.timeBetweenJumps.y);
        if (prevState == StateMachine.State.Moving)
          this.Spine.AnimationState.SetAnimation(0, "jump-end", false);
        this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        if (!string.IsNullOrEmpty(this.OnLandSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnLandSoundPath, this.transform.position);
        this.simpleSpineFlash.FlashWhite(false);
        break;
      case StateMachine.State.Moving:
        this.hasCollidedWithObstacle = false;
        this.hoppingTimestamp = Time.time + this.hoppingDur;
        this.simpleSpineFlash.FlashWhite(false);
        this.Spine.AnimationState.SetAnimation(0, "jump", false);
        if (string.IsNullOrEmpty(this.OnJumpSoundPath))
          break;
        AudioManager.Instance.PlayOneShot(this.OnJumpSoundPath, this.transform.position);
        break;
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.simpleSpineFlash.FlashFillRed();
  }

  public float GetAngleToTarget()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return this.GetRandomFacingAngle();
    float angle = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position);
    if ((UnityEngine.Object) this.collider == (UnityEngine.Object) null)
      return angle;
    float num = 32f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.collider.radius, new Vector2(Mathf.Cos(angle * ((float) Math.PI / 180f)), Mathf.Sin(angle * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      angle += (float) (360.0 / ((double) num + 1.0));
    return angle;
  }

  public float GetRandomFacingAngle()
  {
    float randomFacingAngle = (float) UnityEngine.Random.Range(0, 360);
    if ((UnityEngine.Object) this.collider == (UnityEngine.Object) null)
      return randomFacingAngle;
    float num = 16f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.collider.radius, new Vector2(Mathf.Cos(randomFacingAngle * ((float) Math.PI / 180f)), Mathf.Sin(randomFacingAngle * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      randomFacingAngle += (float) (360.0 / ((double) num + 1.0));
    return randomFacingAngle;
  }

  public float GetFleeAngle()
  {
    if ((UnityEngine.Object) this.GetClosestTarget() == (UnityEngine.Object) null)
      return this.GetRandomFacingAngle();
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(4, 7);
      Vector3 toPosition = this.GetClosestTarget().transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      Vector3 direction = Vector3.Normalize(toPosition - this.GetClosestTarget().transform.position);
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.GetClosestTarget().transform.position, 1.5f, (Vector2) direction, distance, (int) this.layerToCheck);
      if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
        return Utils.GetAngle(this.transform.position, toPosition);
      if ((double) Vector3.Distance(this.GetClosestTarget().transform.position, (Vector3) raycastHit2D.centroid) > 3.0)
        return Utils.GetAngle(this.transform.position, toPosition);
    }
    return this.GetRandomFacingAngle();
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive(true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) duration)
      yield return (object) null;
    this.damageColliderEvents.SetActive(false);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health)
      {
        Health.team2[index].enabled = true;
        Health.team2[index].invincible = false;
        Health.team2[index].untouchable = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
      }
    }
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__64_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedEnemyHandle = obj;
    obj.Result.CreatePool(20, true);
  }

  [CompilerGenerated]
  public void \u003CRageHopIE\u003Eb__72_0(List<Projectile> projectiles)
  {
    this.StartCoroutine((IEnumerator) this.SetProjectilePulse(projectiles));
  }
}
