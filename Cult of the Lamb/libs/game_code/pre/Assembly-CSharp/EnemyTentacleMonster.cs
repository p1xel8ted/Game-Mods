// Decompiled with JetBrains decompiler
// Type: EnemyTentacleMonster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unify;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyTentacleMonster : UnitObject
{
  public SkeletonAnimation spine;
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootProjectilesAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string holyHandGrenadeAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string holyHandGrenadeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string sweepingSpawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string randomSpawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string teleportAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string daggerAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string swordAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string swordLoopAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string swordAttackAnimation;
  [Space]
  [SerializeField]
  private CircleCollider2D physicsCollider;
  [SerializeField]
  private float teleportMinDistanceToPlayer;
  [SerializeField]
  private float lungeSpeed;
  [SerializeField]
  private float daggerOverrideMinRadius;
  [SerializeField]
  private float daggerOverrideMaxRadius;
  [SerializeField]
  private float daggerOverrideIntervals;
  [SerializeField]
  private ColliderEvents daggerCollider;
  [Space]
  [SerializeField]
  private float swordAttackWithinRadius;
  [SerializeField]
  private float swordChaseSpeed;
  [SerializeField]
  private float swordMaxChaseDuration;
  [SerializeField]
  private float swordRetargetPlayerInterval;
  [SerializeField]
  private ColliderEvents swordCollider;
  [SerializeField]
  private float projectilePatternCircleAnticipation;
  [SerializeField]
  private float projectilePatternCircleDuration;
  [SerializeField]
  private ProjectilePattern projectilePatternCircle;
  [Space]
  [SerializeField]
  private float projectilePatternSnakeAnticipation;
  [SerializeField]
  private float projectilePatternSnakeDuration;
  [SerializeField]
  private ProjectilePatternBeam projectilePatternSnake;
  [Space]
  [SerializeField]
  private float projectilePatternScatterAnticipation;
  [SerializeField]
  private float projectilePatternScatterDuration;
  [SerializeField]
  private ProjectilePattern projectilePatternScatter;
  [Space]
  [SerializeField]
  private float projectilePatternRingsAnticipation;
  [SerializeField]
  private float projectilePatternRingsDuration;
  [SerializeField]
  private float projectilePatternRingsSpeed;
  [SerializeField]
  private float projectilePatternRingsAcceleration;
  [SerializeField]
  private float projectilePatternRingsRadius;
  [SerializeField]
  private ProjectileCircle projectilePatternRings;
  [Space]
  [SerializeField]
  private float projectilePatternRandomAnticipation;
  [SerializeField]
  private float projectilePatternRandomDuration;
  [SerializeField]
  private ProjectilePattern projectilePatternRandom;
  [Space]
  [SerializeField]
  private float projectilePatternBeamAnticipation;
  [SerializeField]
  private float projectilePatternBeamDuration;
  [SerializeField]
  private ProjectilePatternBeam projectilePatternBeam;
  [SerializeField]
  private float hhgAnticipation;
  [SerializeField]
  private float hhgMinDistance;
  [SerializeField]
  private Vector2 hhgSpawnAmount;
  [SerializeField]
  private Vector2 hhgSpawnOffset;
  [SerializeField]
  private Vector2 hhgSpawnDelay;
  [SerializeField]
  private AssetReferenceGameObject[] hhgEnemiesList;
  [SerializeField]
  private float ssAnticipation;
  [SerializeField]
  private float ssDistanceBetween;
  [SerializeField]
  private float ssDelayBetween;
  [SerializeField]
  private float ssForce;
  [SerializeField]
  private AnimationCurve ssArc;
  [SerializeField]
  private Vector2 ssSpawnAmount;
  [SerializeField]
  private AssetReferenceGameObject[] ssEnemiesList;
  [SerializeField]
  private float rsAnticipation;
  [SerializeField]
  private Vector2 rsSpawnAmount;
  [SerializeField]
  private AssetReferenceGameObject[] rsEnemiesList;
  [SerializeField]
  private float enragedDuration = 2f;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float p2HealthThreshold = 0.6f;
  [Space]
  [SerializeField]
  private Interaction_MonsterHeart interaction_MonsterHeart;
  [SerializeField]
  private GameObject blockingCollider;
  private List<GameObject> spawnedEnemies = new List<GameObject>();
  private Coroutine currentAttackRoutine;
  private EventInstance grenadeLoopingSoundInstance;
  private bool facePlayer = true;
  private bool isDead;
  private bool queuePhaseIncrement;
  private bool activated;
  private bool attacking;
  private bool m = true;
  private int currentPhaseNumber;
  private float startingHealth;
  private float repathTimestamp;
  private bool recentlyTeleported;

  private bool moving
  {
    get => this.m;
    set
    {
      this.m = value;
      if (this.m)
        return;
      this.ClearPaths();
    }
  }

  public override void Awake()
  {
    base.Awake();
    this.daggerCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.CombatAttack_OnTriggerEnterEvent);
    this.swordCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.CombatAttack_OnTriggerEnterEvent);
  }

  private void Start()
  {
    this.startingHealth = this.health.HP;
    ProjectilePatternBase.OnProjectileSpawned += new ProjectilePatternBase.ProjectileEvent(this.OnProjectileSpawned);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    AudioManager.Instance.StopLoop(this.grenadeLoopingSoundInstance);
    ProjectilePatternBase.OnProjectileSpawned -= new ProjectilePatternBase.ProjectileEvent(this.OnProjectileSpawned);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    if (this.queuePhaseIncrement && this.currentAttackRoutine == null)
    {
      this.queuePhaseIncrement = false;
      this.IncrementPhase();
    }
    if (this.facePlayer && this.activated && !this.isDead)
      this.LookAt(this.GetAngleToTarget());
    if (this.activated && !this.isDead && this.moving)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float repathTimestamp = this.repathTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) repathTimestamp & currentTime.HasValue)
      {
        if (this.attacking)
        {
          if ((double) Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position) < 2.0)
            this.givePath(PlayerFarming.Instance.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 2f));
          else
            this.givePath(PlayerFarming.Instance.transform.position + Vector3.up);
        }
        else
          this.givePath(this.GetPositionAwayFromPlayer());
        this.repathTimestamp = GameManager.GetInstance().CurrentTime + this.swordRetargetPlayerInterval;
      }
    }
    if (this.isDead)
      return;
    if (this.activated && this.currentPhaseNumber == 1)
    {
      this.UpdatePhase1();
    }
    else
    {
      if (!this.activated || this.currentPhaseNumber < 2)
        return;
      this.UpdatePhase2();
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.activated)
      return;
    this.StartCoroutine((IEnumerator) this.DelayAddCamera());
  }

  private IEnumerator DelayAddCamera()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyTentacleMonster enemyTentacleMonster = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().AddToCamera(enemyTentacleMonster.gameObject);
      GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private bool CanSpawnEnemies() => this.spawnedEnemies.Count < 15;

  public void BeginPhase1()
  {
    GameManager.GetInstance().AddToCamera(this.gameObject);
    GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
    this.StartCoroutine((IEnumerator) this.DelayCallback(1f, (System.Action) (() =>
    {
      this.activated = true;
      this.currentPhaseNumber = 1;
      this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.RandomSpawnIE());
    })));
  }

  private IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  private void UpdatePhase1()
  {
    float num1 = Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position);
    if (this.currentAttackRoutine != null)
      return;
    if (UnityEngine.Random.Range(0, 3) == 0 && !this.recentlyTeleported)
    {
      this.recentlyTeleported = true;
      if ((double) num1 < (double) this.daggerOverrideMinRadius)
      {
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.DaggerAttackIE());
        return;
      }
      if ((double) num1 < (double) this.daggerOverrideMinRadius * 2.0)
      {
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.TeleportAwayFromPlayerIE(0.25f));
        return;
      }
    }
    this.recentlyTeleported = false;
    int num2 = UnityEngine.Random.Range(0, 5);
    if (num2 < 2 && this.CanSpawnEnemies())
    {
      if (UnityEngine.Random.Range(0, 2) == 0 && this.spawnedEnemies.Count < 3)
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.RandomSpawnIE());
      else
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.SweepingSpawnIE());
    }
    else if (num2 == 2 && (double) num1 > 4.0)
    {
      this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.SwordAttackIE());
    }
    else
    {
      switch (UnityEngine.Random.Range(0, 4))
      {
        case 0:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileBeamIE());
          break;
        case 1:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRandomIE());
          break;
        default:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsIE());
          break;
      }
    }
  }

  public void BeginPhase2()
  {
    this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.Phase2IE());
  }

  private IEnumerator Phase2IE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.EnragedIE());
    enemyTentacleMonster.maxSpeed *= 2f;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.ShootProjectileCircleIE());
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  private void UpdatePhase2()
  {
    float num1 = Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position);
    if (this.currentAttackRoutine != null)
      return;
    if (UnityEngine.Random.Range(0, 3) == 0 && !this.recentlyTeleported)
    {
      this.recentlyTeleported = true;
      if ((double) num1 < (double) this.daggerOverrideMinRadius)
      {
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.DaggerAttackIE());
        return;
      }
      if ((double) num1 < (double) this.daggerOverrideMinRadius * 2.0)
      {
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.TeleportAwayFromPlayerIE(0.25f));
        return;
      }
    }
    this.recentlyTeleported = false;
    int num2 = UnityEngine.Random.Range(0, 5);
    if (num2 < 2 && this.CanSpawnEnemies())
    {
      int num3 = UnityEngine.Random.Range(0, 3);
      if (num3 == 0 && this.spawnedEnemies.Count < 5)
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.RandomSpawnIE());
      else if (num3 == 1)
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.HolyHandGrenadeIE());
      else
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.SweepingSpawnIE());
    }
    else if (num2 == 2)
    {
      if ((double) num1 > 4.0)
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.SwordAttackIE());
      else
        this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.DaggerRapidAttackIE());
    }
    else
    {
      switch (UnityEngine.Random.Range(0, 3))
      {
        case 0:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileCircleIE());
          break;
        case 1:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRandomIE());
          break;
        default:
          this.currentAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsTripleIE());
          break;
      }
    }
  }

  private void IncrementPhase()
  {
    if (this.currentAttackRoutine != null)
    {
      this.queuePhaseIncrement = true;
    }
    else
    {
      if (this.currentAttackRoutine != null)
        this.StopCoroutine(this.currentAttackRoutine);
      ++this.currentPhaseNumber;
      if (this.currentPhaseNumber != 2)
        return;
      this.BeginPhase2();
    }
  }

  public void HolyHandGrenade() => this.StartCoroutine((IEnumerator) this.HolyHandGrenadeIE());

  private IEnumerator HolyHandGrenadeIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.holyHandGrenadeAnticipationAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.holyHandGrenadeAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_start", enemyTentacleMonster.gameObject);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.hhgAnticipation);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    int spawnAmount = (int) UnityEngine.Random.Range(enemyTentacleMonster.hhgSpawnAmount.x, enemyTentacleMonster.hhgSpawnAmount.y + 1f);
    for (int i = 0; i < spawnAmount; ++i)
    {
      Vector3 position = enemyTentacleMonster.GetPosition(PlayerFarming.Instance.transform.position, PlayerFarming.Instance.state.LookAngle + UnityEngine.Random.Range(enemyTentacleMonster.hhgSpawnOffset.x, enemyTentacleMonster.hhgSpawnOffset.y), enemyTentacleMonster.hhgMinDistance);
      // ISSUE: reference to a compiler-generated method
      Addressables.InstantiateAsync((object) enemyTentacleMonster.hhgEnemiesList[UnityEngine.Random.Range(0, enemyTentacleMonster.hhgEnemiesList.Length)], position, Quaternion.identity, enemyTentacleMonster.transform.parent).Completed += new System.Action<AsyncOperationHandle<GameObject>>(enemyTentacleMonster.\u003CHolyHandGrenadeIE\u003Eb__97_0);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyTentacleMonster.hhgSpawnDelay.x, enemyTentacleMonster.hhgSpawnDelay.y));
    }
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void SweepingSpawn() => this.StartCoroutine((IEnumerator) this.SweepingSpawnIE());

  private IEnumerator SweepingSpawnIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.sweepingSpawnAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/staff", enemyTentacleMonster.gameObject);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.ssAnticipation);
    int spawnAmount = (int) UnityEngine.Random.Range(enemyTentacleMonster.ssSpawnAmount.x, enemyTentacleMonster.ssSpawnAmount.y + 1f);
    Mathf.RoundToInt((float) (spawnAmount / 2));
    float angle = Utils.GetAngle(enemyTentacleMonster.transform.position, PlayerFarming.Instance.transform.position) * ((float) Math.PI / 180f);
    for (int i = 0; i < spawnAmount; ++i)
    {
      float norm = (float) i / (float) spawnAmount;
      Addressables.InstantiateAsync((object) enemyTentacleMonster.ssEnemiesList[UnityEngine.Random.Range(0, enemyTentacleMonster.ssEnemiesList.Length)], enemyTentacleMonster.transform.position, Quaternion.identity, enemyTentacleMonster.transform.parent).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        GameObject spawnedEnemy = obj.Result;
        UnitObject component1 = spawnedEnemy.GetComponent<UnitObject>();
        component1.CanHaveModifier = false;
        this.spawnedEnemies.Add(spawnedEnemy);
        component1.GetComponent<Health>().OnDie += (Health.DieAction) ((Attacker, AttackLocation, Victim, AttackType, AttackFlags) => this.spawnedEnemies.Remove(spawnedEnemy));
        DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
        if ((bool) (UnityEngine.Object) component2)
          component2.GiveXP = false;
        component1.GetComponent<UnitObject>().DoKnockBack(angle, this.ssForce * this.ssArc.Evaluate(norm), 1f);
        if (!(component1 is EnemyJellyCharger))
          return;
        ((EnemyJellyCharger) component1).AllowMultipleChargers = true;
        component1.VisionRange = int.MaxValue;
      });
      angle += enemyTentacleMonster.ssDistanceBetween;
      if ((double) enemyTentacleMonster.ssDelayBetween != 0.0)
        yield return (object) new WaitForSeconds(enemyTentacleMonster.ssDelayBetween);
    }
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void RandomSpawn() => this.StartCoroutine((IEnumerator) this.RandomSpawnIE());

  private IEnumerator RandomSpawnIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.moving = false;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.randomSpawnAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/staff", enemyTentacleMonster.gameObject);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.rsAnticipation);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/staff_magic", enemyTentacleMonster.gameObject);
    int num = (int) UnityEngine.Random.Range(enemyTentacleMonster.rsSpawnAmount.x, enemyTentacleMonster.rsSpawnAmount.y + 1f);
    for (int index = 0; index < num; ++index)
    {
      // ISSUE: reference to a compiler-generated method
      Addressables.InstantiateAsync((object) enemyTentacleMonster.rsEnemiesList[UnityEngine.Random.Range(0, enemyTentacleMonster.rsEnemiesList.Length)], enemyTentacleMonster.GetRandomPosition(Vector3.zero, 0.0f, 3f, 8f), Quaternion.identity, enemyTentacleMonster.transform.parent).Completed += new System.Action<AsyncOperationHandle<GameObject>>(enemyTentacleMonster.\u003CRandomSpawnIE\u003Eb__101_0);
    }
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
    enemyTentacleMonster.moving = true;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  private void OnProjectileSpawned()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_spawn", this.gameObject);
  }

  private void ShootProjectileCircle()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileCircleIE());
  }

  private IEnumerator ShootProjectileCircleIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, PlayerFarming.Instance.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.moving = false;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternCircleAnticipation);
    enemyTentacleMonster.projectilePatternCircle.Shoot();
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternCircleDuration);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
    enemyTentacleMonster.moving = true;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  private void ShootProjectileSnake()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileSnakeIE());
  }

  private IEnumerator ShootProjectileSnakeIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, PlayerFarming.Instance.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternSnakeAnticipation);
    enemyTentacleMonster.projectilePatternSnake.Shoot();
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternSnakeDuration);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  private void ShootProjectileRandom()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileRandomIE());
  }

  private IEnumerator ShootProjectileRandomIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, PlayerFarming.Instance.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternRandomAnticipation);
    enemyTentacleMonster.projectilePatternRandom.Shoot();
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternRandomDuration);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  private void ShootProjectileScatter()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileScatterIE());
  }

  private IEnumerator ShootProjectileScatterIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, PlayerFarming.Instance.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternScatterAnticipation);
    enemyTentacleMonster.projectilePatternScatter.Shoot();
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternScatterDuration);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  private void ShootProjectileBeam()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileBeamIE());
  }

  private IEnumerator ShootProjectileBeamIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, PlayerFarming.Instance.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternBeamAnticipation);
    enemyTentacleMonster.projectilePatternBeam.Shoot();
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternBeamDuration);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  private void ShootProjectileRings()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileRingsIE());
  }

  private IEnumerator ShootProjectileRingsIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, PlayerFarming.Instance.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.moving = false;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternRingsAnticipation);
    Projectile component = UnityEngine.Object.Instantiate<ProjectileCircle>(enemyTentacleMonster.projectilePatternRings, enemyTentacleMonster.transform.parent).GetComponent<Projectile>();
    component.transform.position = enemyTentacleMonster.transform.position + (Vector3) (Utils.DegreeToVector2(enemyTentacleMonster.GetAngleToPlayer()) * enemyTentacleMonster.projectilePatternRingsRadius * 2f);
    component.Angle = enemyTentacleMonster.GetAngleToTarget();
    component.health = enemyTentacleMonster.health;
    component.team = Health.Team.Team2;
    component.Speed = enemyTentacleMonster.projectilePatternRingsSpeed;
    component.Acceleration = enemyTentacleMonster.projectilePatternRingsAcceleration;
    // ISSUE: reference to a compiler-generated method
    component.GetComponent<ProjectileCircle>().InitDelayed(PlayerFarming.Instance.gameObject, enemyTentacleMonster.projectilePatternRingsRadius, 0.0f, new System.Action(enemyTentacleMonster.\u003CShootProjectileRingsIE\u003Eb__114_0));
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternRingsDuration);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
    enemyTentacleMonster.moving = true;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void ShootProjectileRingsTriple()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileRingsTripleIE());
  }

  private IEnumerator ShootProjectileRingsTripleIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    if ((double) Vector3.Distance(enemyTentacleMonster.transform.position, PlayerFarming.Instance.transform.position) < 4.0)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportAwayFromPlayerIE(nullifyRoutine: false));
    enemyTentacleMonster.moving = false;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.shootProjectilesAnimation, true);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    enemyTentacleMonster.grenadeLoopingSoundInstance = AudioManager.Instance.CreateLoop("event:/boss/jellyfish/grenade_loop", enemyTentacleMonster.gameObject, true);
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternRingsAnticipation);
    List<float> floatList = new List<float>()
    {
      0.0f,
      0.5f,
      1f
    };
    for (int index = 0; index < 3; ++index)
    {
      float t = (float) index / 2f;
      float angleToPlayer = enemyTentacleMonster.GetAngleToPlayer();
      Vector3 vector2 = (Vector3) Utils.DegreeToVector2(angleToPlayer);
      Vector3 vector3 = Vector3.Lerp((Vector3) Utils.DegreeToVector2(angleToPlayer - 90f), (Vector3) Utils.DegreeToVector2(angleToPlayer + 90f), t) * 1.25f;
      Projectile component = UnityEngine.Object.Instantiate<ProjectileCircle>(enemyTentacleMonster.projectilePatternRings, enemyTentacleMonster.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyTentacleMonster.transform.position + (vector2 + vector3) * enemyTentacleMonster.projectilePatternRingsRadius * 2f;
      component.Angle = angleToPlayer;
      component.health = enemyTentacleMonster.health;
      component.team = Health.Team.Team2;
      component.Speed = enemyTentacleMonster.projectilePatternRingsSpeed;
      component.Acceleration = enemyTentacleMonster.projectilePatternRingsAcceleration;
      float shootDelay = floatList[UnityEngine.Random.Range(0, floatList.Count)];
      floatList.Remove(shootDelay);
      // ISSUE: reference to a compiler-generated method
      component.GetComponent<ProjectileCircle>().InitDelayed(PlayerFarming.Instance.gameObject, enemyTentacleMonster.projectilePatternRingsRadius, shootDelay, new System.Action(enemyTentacleMonster.\u003CShootProjectileRingsTripleIE\u003Eb__116_0));
    }
    yield return (object) new WaitForSeconds(enemyTentacleMonster.projectilePatternRingsDuration);
    AudioManager.Instance.StopLoop(enemyTentacleMonster.grenadeLoopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_end", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.idleAnimation, true);
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.25f));
    enemyTentacleMonster.moving = true;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void DaggerAttack() => this.StartCoroutine((IEnumerator) this.DaggerAttackIE());

  private IEnumerator DaggerAttackIE(bool finishAttack = true)
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.attacking = true;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.daggerAttackAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/dagger_attack", enemyTentacleMonster.gameObject);
    yield return (object) new WaitForSeconds(0.4f);
    float ogMaxSpeed = enemyTentacleMonster.maxSpeed;
    enemyTentacleMonster.maxSpeed = enemyTentacleMonster.lungeSpeed;
    enemyTentacleMonster.speed = enemyTentacleMonster.lungeSpeed;
    yield return (object) new WaitForSeconds(0.2f);
    enemyTentacleMonster.maxSpeed = ogMaxSpeed;
    enemyTentacleMonster.speed = enemyTentacleMonster.maxSpeed / 2f;
    enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.EnableCollider(enemyTentacleMonster.daggerCollider.gameObject, 0.25f));
    yield return (object) new WaitForSeconds(1.5f);
    if (finishAttack)
      enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    enemyTentacleMonster.attacking = false;
  }

  private IEnumerator DaggerRapidAttackIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    for (int i = 0; i < UnityEngine.Random.Range(2, 5); ++i)
      yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.DaggerAttackIE(false));
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1f));
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  public void SwordAttack() => this.StartCoroutine((IEnumerator) this.SwordAttackIE());

  private IEnumerator SwordAttackIE()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.attacking = true;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.swordAnticipationAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.swordLoopAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/sword_charge", enemyTentacleMonster.gameObject);
    float timeStamp = GameManager.GetInstance().CurrentTime + enemyTentacleMonster.swordMaxChaseDuration;
    enemyTentacleMonster.repathTimestamp = 0.0f;
    float ogMaxSpeed = enemyTentacleMonster.maxSpeed;
    enemyTentacleMonster.DisableForces = false;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    TweenerCore<float, float, FloatOptions> tween = DOTween.To(new DOGetter<float>(enemyTentacleMonster.\u003CSwordAttackIE\u003Eb__121_0), new DOSetter<float>(enemyTentacleMonster.\u003CSwordAttackIE\u003Eb__121_1), enemyTentacleMonster.swordChaseSpeed, 2f);
    float t = 0.0f;
    while ((double) Vector3.Distance(PlayerFarming.Instance.transform.position, enemyTentacleMonster.transform.position) > (double) enemyTentacleMonster.swordAttackWithinRadius && (double) GameManager.GetInstance().CurrentTime < (double) timeStamp || (double) t < 1.0)
    {
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.swordAttackAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/sword_attack", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.speed = enemyTentacleMonster.lungeSpeed;
    yield return (object) new WaitForSeconds(0.1f);
    enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.EnableCollider(enemyTentacleMonster.swordCollider.gameObject, 0.15f));
    yield return (object) new WaitForSeconds(0.1f);
    tween.Kill();
    enemyTentacleMonster.maxSpeed = ogMaxSpeed;
    enemyTentacleMonster.speed = enemyTentacleMonster.maxSpeed / 2f;
    yield return (object) new WaitForSeconds(1f);
    enemyTentacleMonster.attacking = false;
    enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
  }

  private IEnumerator EnableCollider(GameObject collider, float duration)
  {
    collider.SetActive(true);
    yield return (object) new WaitForSeconds(duration);
    collider.SetActive(false);
  }

  private void CombatAttack_OnTriggerEnterEvent(Collider2D collider)
  {
    if (!(collider.tag == "Player"))
      return;
    PlayerFarming.Instance.health.DealDamage(1f, this.gameObject, this.transform.position);
  }

  public void TeleportAwayFromPlayer()
  {
    this.StartCoroutine((IEnumerator) this.TeleportAwayFromPlayerIE(0.5f));
  }

  public void TeleportNearPlayer()
  {
    this.StartCoroutine((IEnumerator) this.TeleportNearPlayerIE(0.0f));
  }

  public void TeleportToPostion(Vector3 position)
  {
    this.StartCoroutine((IEnumerator) this.TeleportToPositionIE(position));
  }

  private IEnumerator TeleportAwayFromPlayerIE(float endDelay = 1f, bool nullifyRoutine = true)
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.moving = false;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportToPositionIE(enemyTentacleMonster.GetPositionAwayFromPlayer(), endDelay));
    if (nullifyRoutine)
    {
      enemyTentacleMonster.moving = true;
      enemyTentacleMonster.currentAttackRoutine = (Coroutine) null;
    }
  }

  private IEnumerator TeleportNearPlayerIE(float endDelay = 1f)
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.moving = false;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportOutIE());
    Vector3 position = PlayerFarming.Instance.transform.position;
    float degree = (float) UnityEngine.Random.Range(0, 360);
    int num = 0;
    while (num++ < 32 /*0x20*/)
    {
      if ((bool) Physics2D.Raycast((Vector2) position, Utils.DegreeToVector2(degree), enemyTentacleMonster.teleportMinDistanceToPlayer, (int) enemyTentacleMonster.layerToCheck))
      {
        degree = Mathf.Repeat(degree + 11.25f, 360f);
      }
      else
      {
        position += (Vector3) Utils.DegreeToVector2(degree) * enemyTentacleMonster.teleportMinDistanceToPlayer;
        break;
      }
    }
    enemyTentacleMonster.transform.position = position;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportInIE());
    yield return (object) new WaitForSeconds(endDelay);
    enemyTentacleMonster.moving = true;
  }

  private IEnumerator TeleportToPositionIE(Vector3 position, float endDelay = 0.0f)
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportOutIE());
    enemyTentacleMonster.transform.position = position;
    yield return (object) enemyTentacleMonster.StartCoroutine((IEnumerator) enemyTentacleMonster.TeleportInIE());
    yield return (object) new WaitForSeconds(endDelay);
  }

  private IEnumerator TeleportOutIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyTentacleMonster enemyTentacleMonster = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyTentacleMonster.physicsCollider.enabled = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyTentacleMonster.spine.AnimationState.SetAnimation(0, enemyTentacleMonster.teleportAnimation, false);
    enemyTentacleMonster.spine.AnimationState.AddAnimation(0, enemyTentacleMonster.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/teleport_away", enemyTentacleMonster.gameObject);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator TeleportInIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyTentacleMonster enemyTentacleMonster = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyTentacleMonster.physicsCollider.enabled = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/teleport_return", enemyTentacleMonster.gameObject);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator EnragedIE()
  {
    this.spine.AnimationState.SetAnimation(0, "roar", false);
    this.spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(0.7f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 1.3f);
    yield return (object) new WaitForSeconds(2.4f);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.simpleSpineFlash.FlashFillRed(0.25f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/gethit");
    Vector3 Position = (AttackLocation + Attacker.transform.position) / 2f;
    BiomeConstants.Instance.EmitHitVFX(Position, Quaternion.identity.z, "HitFX_Weak");
    float num = this.health.HP / this.startingHealth;
    if (this.currentPhaseNumber != 1 || (double) num > (double) this.p2HealthThreshold)
      return;
    this.IncrementPhase();
  }

  private void LookAt(float angle)
  {
    this.state.LookAngle = angle;
    this.state.facingAngle = angle;
  }

  private float GetAngleToTarget()
  {
    float angleToPlayer = this.GetAngleToPlayer();
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return angleToPlayer;
    float num = 32f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(angleToPlayer * ((float) Math.PI / 180f)), Mathf.Sin(angleToPlayer * ((float) Math.PI / 180f))), 2.5f, (int) this.layerToCheck); ++index)
      angleToPlayer += (float) (360.0 / ((double) num + 1.0));
    return angleToPlayer;
  }

  private float GetAngleToPlayer()
  {
    return !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) ? 0.0f : Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position);
  }

  private Vector3 GetPositionAwayFromPlayer()
  {
    float x = PlayerFarming.Instance.transform.position.x;
    float y;
    for (y = PlayerFarming.Instance.transform.position.y; (double) Vector3.Distance(new Vector3(x, y), PlayerFarming.Instance.transform.position) < 4.0; y = UnityEngine.Random.Range(-5.5f, 5.5f))
      x = UnityEngine.Random.Range(-10f, 10f);
    return new Vector3(x, y, 0.0f);
  }

  private Vector3 GetRandomPosition(
    Vector3 startingPosition,
    float radius,
    float minDist,
    float maxDist)
  {
    float degree = (float) UnityEngine.Random.Range(0, 360);
    Vector3 origin = startingPosition;
    int num1 = 0;
    while (num1++ < 32 /*0x20*/)
    {
      float num2 = UnityEngine.Random.Range(minDist, maxDist);
      if ((bool) Physics2D.Raycast((Vector2) origin, Utils.DegreeToVector2(degree), num2 - radius, (int) this.layerToCheck))
      {
        degree = Mathf.Repeat(degree + (float) UnityEngine.Random.Range(0, 360), 360f);
      }
      else
      {
        origin += (Vector3) Utils.DegreeToVector2(degree) * (num2 - radius);
        break;
      }
    }
    return origin;
  }

  private Vector3 GetPosition(Vector3 startingPosition, float angle, float distance)
  {
    float degree = angle;
    Vector3 origin = startingPosition;
    int num = 0;
    while (num++ < 32 /*0x20*/)
    {
      if ((bool) Physics2D.Raycast((Vector2) origin, Utils.DegreeToVector2(degree), distance, (int) this.layerToCheck))
      {
        degree = Mathf.Repeat(degree + (float) UnityEngine.Random.Range(0, 360), 360f);
      }
      else
      {
        origin += (Vector3) Utils.DegreeToVector2(degree) * distance;
        break;
      }
    }
    return origin;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    AudioManager.Instance.StopLoop(this.grenadeLoopingSoundInstance);
    PlayerFarming.Instance.health.invincible = true;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_BOSS_3"));
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.daggerCollider.gameObject.SetActive(false);
    this.swordCollider.gameObject.SetActive(false);
    for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null)
      {
        Health component = this.spawnedEnemies[index].GetComponent<Health>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.enabled = true;
          component.DealDamage(component.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy);
        }
      }
    }
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
    GameManager.GetInstance().CamFollowTarget.MinZoom = 6f;
    GameManager.GetInstance().CamFollowTarget.MaxZoom = 20f;
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/death");
    this.isDead = true;
    this.moving = false;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Die());
  }

  private IEnumerator Die()
  {
    EnemyTentacleMonster enemyTentacleMonster = this;
    enemyTentacleMonster.ClearPaths();
    enemyTentacleMonster.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyTentacleMonster.gameObject, 12f);
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/death", enemyTentacleMonster.gameObject);
    enemyTentacleMonster.rb.velocity = (Vector2) Vector3.zero;
    enemyTentacleMonster.rb.isKinematic = true;
    enemyTentacleMonster.rb.simulated = false;
    enemyTentacleMonster.rb.bodyType = RigidbodyType2D.Static;
    if ((double) enemyTentacleMonster.transform.position.x > 11.0)
      enemyTentacleMonster.transform.position = new Vector3(11f, enemyTentacleMonster.transform.position.y, 0.0f);
    if ((double) enemyTentacleMonster.transform.position.x < -11.0)
      enemyTentacleMonster.transform.position = new Vector3(-11f, enemyTentacleMonster.transform.position.y, 0.0f);
    if ((double) enemyTentacleMonster.transform.position.y > 7.0)
      enemyTentacleMonster.transform.position = new Vector3(enemyTentacleMonster.transform.position.x, 7f, 0.0f);
    if ((double) enemyTentacleMonster.transform.position.y < -7.0)
      enemyTentacleMonster.transform.position = new Vector3(enemyTentacleMonster.transform.position.x, -7f, 0.0f);
    yield return (object) new WaitForEndOfFrame();
    enemyTentacleMonster.simpleSpineFlash.StopAllCoroutines();
    enemyTentacleMonster.simpleSpineFlash.SetColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
    enemyTentacleMonster.state.CURRENT_STATE = StateMachine.State.Dieing;
    if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
    {
      enemyTentacleMonster.spine.AnimationState.SetAnimation(0, "die", false);
      enemyTentacleMonster.spine.AnimationState.AddAnimation(0, "dead", true, 0.0f);
    }
    else
    {
      enemyTentacleMonster.spine.AnimationState.SetAnimation(0, "die-noheart", false);
      enemyTentacleMonster.spine.AnimationState.AddAnimation(0, "dead-noheart", true, 0.0f);
    }
    yield return (object) new WaitForSeconds(4.5f);
    GameManager.GetInstance().OnConversationEnd();
    enemyTentacleMonster.blockingCollider.SetActive(false);
    enemyTentacleMonster.rb.isKinematic = true;
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3))
      enemyTentacleMonster.interaction_MonsterHeart.ObjectiveToComplete = Objectives.CustomQuestTypes.BishopsOfTheOldFaith3;
    enemyTentacleMonster.interaction_MonsterHeart.Play();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    AudioManager.Instance.StopLoop(this.grenadeLoopingSoundInstance);
    this.currentAttackRoutine = (Coroutine) null;
    this.moving = true;
  }
}
