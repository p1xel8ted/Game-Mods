// Decompiled with JetBrains decompiler
// Type: EnemyNecromancer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
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
public class EnemyNecromancer : UnitObject
{
  public bool Melee = true;
  public bool Summon = true;
  public bool Throw = true;
  public SkeletonAnimation skeletonAnimation;
  public SimpleSpineFlash simpleSpineFlash;
  public GameObject Arrow;
  public SpriteRenderer Shadow;
  public ParticleSystem summonParticles;
  public float SeperationRadius = 0.5f;
  public GameObject TargetObject;
  public float Range = 6f;
  public float KnockbackSpeed = 0.2f;
  public CircleCollider2D CircleCollider;
  public AssetReferenceGameObject minionPrefab;
  [SerializeField]
  public bool isSpawnablesIncreasingDamageMultiplier = true;
  public string IdleAnimation = "idle";
  public string DashAnimation = "dash";
  public string SummonAnimation = "summon";
  public string LevitateAnimation = "levitate";
  public string ThrowAnimation = "throw";
  public string AttackAnimation = "attack";
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string SkeletonSummonSFX = "event:/dlc/dungeon06/enemy/necromancer/attack_skeleton_summon";
  [EventRef]
  public string SkeletonSummonVO = "event:/dlc/dungeon06/enemy/necromancer/attack_skeleton_summon_vo";
  [EventRef]
  public string AttackSkeletonLiftSFX = "event:/dlc/dungeon06/enemy/necromancer/attack_skeleton_throw_lift";
  [EventRef]
  public string AttackSkeletonLiftVO = "event:/dlc/dungeon06/enemy/necromancer/attack_skeleton_throw_lift_vo";
  [EventRef]
  public string AttackSkeletonThrowTossSFX = "event:/dlc/dungeon06/enemy/necromancer/attack_skeleton_throw_toss";
  [EventRef]
  public string AttackSkeletonThrowTossVO = "event:/dlc/dungeon06/enemy/necromancer/attack_skeleton_throw_toss_vo";
  [EventRef]
  public string AttackSkeletonThrowExplodeSFX = "event:/dlc/dungeon06/enemy/necromancer/attack_skeleton_throw_explode";
  [EventRef]
  public string MoveSFX = "event:/enemy/summon";
  [EventRef]
  public string AttackScytheStartSFX = "event:/dlc/dungeon06/enemy/necromancer/attack_scythe_start";
  [EventRef]
  public string AttackScytheStartVO = "event:/dlc/dungeon06/enemy/necromancer/attack_scythe_start_vo";
  [EventRef]
  public string AmbientLoopSFX = "event:/dlc/dungeon06/enemy/necromancer/amb";
  public float CircleCastRadius = 0.5f;
  [Tooltip("If a player is within this radius, flee!")]
  public float FleeRadius = 2.5f;
  public float FleeDelay = 3f;
  public float MoveForce = 1750f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  public float PauseBeforeRise = 1f;
  public float MinionRiseHeight = 2.5f;
  public float MinionRiseDuration = 2f;
  public float PauseBeforeThrow = 1f;
  public float DelayBetweenMinionThrow = 0.5f;
  [UnityEngine.Range(0.0f, 1f)]
  public float ChanceOfThrowAll = 0.3f;
  public float SignPostCloseCombatDelay = 1f;
  public ColliderEvents DamageColliderEvents;
  public int MeleeAttackCount = 2;
  public float DelayBetweenMeleeAttacks = 0.5f;
  public float MeleeDamageToPlayer = 1f;
  public float MeleeDamageToTeam2 = 10f;
  public int NumToSpawn;
  public float randomDirection;
  public float moveDelay;
  public GameObject EnemySpawnerGO;
  public const float SPAWN_DISTANCE_MIN = 1f;
  public const float SPAWN_DISTANCE_MAX = 4f;
  public List<UnitObject> minions = new List<UnitObject>();
  public bool Stunned;
  public bool isThrowing;
  public bool Teleporting;
  public Coroutine cTeleporting;
  public int summonedCount;
  public bool hasSummoned;
  public float ThrowDelay = 0.5f;
  public float CloseCombatCooldown = 1f;
  public float SummonDelay = 1f;
  public float meleeDistance = 2f;
  public bool canBeParried;
  public float signPostParryWindow = 0.2f;
  public float attackParryWindow = 0.15f;
  public float StartSpeed = 0.4f;
  public int IndicatorCount = 15;
  public string IndicatorPath = "Assets/Prefabs/Enemies/Weapons/ProjectileIndicator.prefab";
  public Dictionary<UnitObject, GameObject> levitateUnitDictionary = new Dictionary<UnitObject, GameObject>();
  public Coroutine throwRoutine;
  public string lambSkeleFloatAnim = "floating_loop";
  public EventInstance spookyLoopSFX;
  public EventInstance attackSFX;
  public EventInstance attackVO;
  public GameObject currentIndicator;
  public AssetReferenceGameObject IndicatorPrefab;
  public GameObject loadedIndicatorAsset;
  public GameObject loadedMinionAsset;
  public List<AsyncOperationHandle<GameObject>> addressablesHandles = new List<AsyncOperationHandle<GameObject>>();

  public void Start()
  {
    this.SeperateObject = true;
    this.CircleCollider = this.GetComponent<CircleCollider2D>();
    this.randomDirection = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    this.state.facingAngle = this.randomDirection * 57.29578f;
    this.LoadAssets();
  }

  public void LoadAssets()
  {
    AsyncOperationHandle<GameObject> asyncOperationHandle1 = Addressables.LoadAssetAsync<GameObject>((object) this.IndicatorPrefab);
    asyncOperationHandle1.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedIndicatorAsset = obj.Result;
      obj.Result.CreatePool(this.IndicatorCount, true);
    });
    asyncOperationHandle1.WaitForCompletion();
    AsyncOperationHandle<GameObject> asyncOperationHandle2 = Addressables.LoadAssetAsync<GameObject>((object) this.minionPrefab);
    asyncOperationHandle2.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedMinionAsset = obj.Result;
      obj.Result.CreatePool(this.IndicatorCount, true);
    });
    asyncOperationHandle2.WaitForCompletion();
    this.addressablesHandles.Add(asyncOperationHandle1);
    this.addressablesHandles.Add(asyncOperationHandle2);
  }

  public override void OnEnable()
  {
    if ((UnityEngine.Object) this.DamageColliderEvents != (UnityEngine.Object) null)
    {
      this.DamageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.DamageColliderEvents.SetActive(false);
    }
    this.health.OnCharmed += new Health.StasisEvent(this.OnCharmed);
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    base.OnEnable();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.ClearPaths();
    this.StopAllCoroutines();
    this.health.OnCharmed -= new Health.StasisEvent(this.OnCharmed);
    if ((UnityEngine.Object) this.DamageColliderEvents != (UnityEngine.Object) null)
      this.DamageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    foreach (UnitObject minion in this.minions)
    {
      if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
      {
        minion.transform.position = new Vector3(minion.transform.position.x, minion.transform.position.y, 0.0f);
        minion.state.CURRENT_STATE = StateMachine.State.Idle;
        minion.enabled = true;
        minion.health.invincible = false;
      }
    }
    this.levitateUnitDictionary.Clear();
    this.StopSpookyLoop();
    if ((bool) (UnityEngine.Object) this.currentIndicator)
      ObjectPool.Recycle(this.currentIndicator);
    this.simpleSpineFlash.FlashWhite(false);
  }

  public override void OnDestroy()
  {
    this.StopSpookyLoop();
    this.ReleaseAddressablesHandles();
    base.OnDestroy();
  }

  public IEnumerator WaitForTarget()
  {
    EnemyNecromancer enemyNecromancer = this;
    while ((UnityEngine.Object) enemyNecromancer.GetClosestTarget() == (UnityEngine.Object) null)
      yield return (object) null;
    Health closestTarget = enemyNecromancer.GetClosestTarget();
    if (!enemyNecromancer.IsLoopPlaying(enemyNecromancer.spookyLoopSFX))
      enemyNecromancer.spookyLoopSFX = AudioManager.Instance.CreateLoop(enemyNecromancer.AmbientLoopSFX, enemyNecromancer.skeletonAnimation.gameObject, true);
    while ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null && (double) Vector3.Distance(enemyNecromancer.transform.position, closestTarget.transform.position) > (double) enemyNecromancer.Range)
      yield return (object) null;
    enemyNecromancer.StopAllCoroutines();
    enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.ChaseTarget());
  }

  public bool IsLoopPlaying(EventInstance loop)
  {
    if (!loop.isValid())
      return false;
    PLAYBACK_STATE state;
    int playbackState = (int) loop.getPlaybackState(out state);
    return state == PLAYBACK_STATE.PLAYING;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.simpleSpineFlash.FlashWhite(false);
    if (AttackType == Health.AttackTypes.Projectile)
    {
      if (!this.Stunned)
      {
        if (!string.IsNullOrEmpty(this.GetHitVO))
          AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
        CameraManager.shakeCamera(0.4f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
        GameManager.GetInstance().HitStop();
        BiomeConstants.Instance.EmitHitVFX(AttackLocation + Vector3.back * 1f, Quaternion.identity.z, "HitFX_Weak");
        this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
        this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
        this.simpleSpineFlash.FlashFillRed();
        if (this.throwRoutine != null)
          return;
        this.StopAllCoroutines();
        this.StartCoroutine((IEnumerator) this.DoStunned());
      }
      else
      {
        CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
        GameObject gameObject = BiomeConstants.Instance.HitFX_Blocked.Spawn();
        gameObject.transform.position = AttackLocation + Vector3.back * 0.5f;
        gameObject.transform.rotation = Quaternion.identity;
      }
    }
    else
    {
      if (!string.IsNullOrEmpty(this.GetHitVO))
        AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
      CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
      BiomeConstants.Instance.EmitHitVFX(AttackLocation + Vector3.back * 1f, Quaternion.identity.z, "HitFX_Weak");
      this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.simpleSpineFlash.FlashFillRed();
      this.state.facingAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
    }
  }

  public void PlayAttackSFX()
  {
    this.attackSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackScytheStartSFX, this.transform);
    this.attackVO = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.AttackScytheStartVO, this.transform);
  }

  public void StopAttackSFX()
  {
    AudioManager.Instance.StopOneShotInstanceEarly(this.attackSFX, FMOD.Studio.STOP_MODE.IMMEDIATE);
    AudioManager.Instance.StopOneShotInstanceEarly(this.attackVO, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.StopAttackSFX();
    this.StopSpookyLoop();
    if (this.state.CURRENT_STATE != StateMachine.State.Dieing)
    {
      if (!string.IsNullOrEmpty(this.DeathVO))
        AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
      this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    }
    this.StopAllCoroutines();
    this.KillAllMinions();
    if (!(bool) (UnityEngine.Object) this.currentIndicator)
      return;
    ObjectPool.Recycle(this.currentIndicator);
  }

  public void KillAllMinions()
  {
    for (int index = this.minions.Count - 1; index >= 0; --index)
    {
      if ((double) this.minions[index].health.HP > 0.0 && this.minions[index].health.team == this.health.team)
      {
        this.minions[index].health.invincible = false;
        this.minions[index].health.DealDamage(float.PositiveInfinity, PlayerFarming.Instance.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
        this.minions.RemoveAt(index);
      }
    }
  }

  public void KillAllMinionsOfState(StateMachine.State stateToKill)
  {
    for (int index = this.minions.Count - 1; index >= 0; --index)
    {
      if ((double) this.minions[index].health.HP > 0.0 && this.minions[index].state.CURRENT_STATE == stateToKill)
      {
        this.minions[index].state.CURRENT_STATE = StateMachine.State.Dieing;
        this.minions[index].health.invincible = false;
        this.minions[index].health.DealDamage(float.PositiveInfinity, PlayerFarming.Instance.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
        this.minions.RemoveAt(index);
      }
    }
  }

  public IEnumerator ChaseTarget()
  {
    EnemyNecromancer enemyNecromancer = this;
    enemyNecromancer.LookAtTarget();
    enemyNecromancer.state.CURRENT_STATE = StateMachine.State.Idle;
    bool loop = true;
    while (loop)
    {
      Health closestTarget = enemyNecromancer.GetClosestTarget();
      if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
      {
        enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.WaitForTarget());
        break;
      }
      enemyNecromancer.TargetObject = closestTarget.gameObject;
      enemyNecromancer.state.facingAngle = Utils.GetAngle(enemyNecromancer.transform.position, enemyNecromancer.TargetObject.transform.position);
      float num = Vector3.Distance(enemyNecromancer.TargetObject.transform.position, enemyNecromancer.transform.position);
      enemyNecromancer.moveDelay -= Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale;
      if ((double) enemyNecromancer.moveDelay < 0.0 && (double) num < (double) enemyNecromancer.FleeRadius || (double) enemyNecromancer.moveDelay < 0.0)
      {
        if ((double) enemyNecromancer.moveDelay < 0.0 && (double) num < (double) enemyNecromancer.FleeRadius)
          enemyNecromancer.ThrowDelay = UnityEngine.Random.Range(0.0f, 1f);
        enemyNecromancer.moveDelay = enemyNecromancer.FleeDelay;
        yield return (object) enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.DoMove());
      }
      if (enemyNecromancer.Summon && (!enemyNecromancer.hasSummoned || enemyNecromancer.GetAliveMinions() <= 0) && (double) (enemyNecromancer.SummonDelay -= Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < 0.0 && !enemyNecromancer.health.IsCharmed)
      {
        enemyNecromancer.StopAllCoroutines();
        yield return (object) enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.DoSummon());
      }
      if ((double) (enemyNecromancer.CloseCombatCooldown -= Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < 0.0 && (double) Vector3.Distance(enemyNecromancer.transform.position, enemyNecromancer.TargetObject.transform.position) < (double) enemyNecromancer.meleeDistance)
      {
        enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.DoCloseCombatAttack());
        break;
      }
      if (enemyNecromancer.Throw && (double) (enemyNecromancer.ThrowDelay -= Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < 0.0 && enemyNecromancer.GetAliveMinions() > 0)
      {
        enemyNecromancer.StopAllCoroutines();
        enemyNecromancer.ThrowDelay = UnityEngine.Random.Range(2f, 3f);
        enemyNecromancer.throwRoutine = enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.DoThrowMinionsAttack((double) UnityEngine.Random.value < (double) enemyNecromancer.ChanceOfThrowAll ? enemyNecromancer.minions.Count : 1));
        break;
      }
      yield return (object) null;
    }
  }

  public int GetAliveMinions()
  {
    int aliveMinions = 0;
    foreach (UnitObject minion in this.minions)
    {
      if ((double) minion.health.HP > 0.0 && minion.health.team == Health.Team.Team2)
        ++aliveMinions;
    }
    return aliveMinions;
  }

  public IEnumerator DoSummon()
  {
    EnemyNecromancer enemyNecromancer = this;
    enemyNecromancer.ClearPaths();
    enemyNecromancer.KillAllMinions();
    int numToSpawn = enemyNecromancer.NumToSpawn;
    AudioManager.Instance.PlayOneShot(enemyNecromancer.SkeletonSummonSFX, enemyNecromancer.gameObject);
    AudioManager.Instance.PlayOneShot(enemyNecromancer.SkeletonSummonVO, enemyNecromancer.gameObject);
    for (; numToSpawn > 0; --numToSpawn)
    {
      enemyNecromancer.summonParticles.startDelay = 1.5f;
      enemyNecromancer.summonParticles.Play();
      enemyNecromancer.skeletonAnimation.AnimationState.SetAnimation(0, enemyNecromancer.SummonAnimation, false);
      enemyNecromancer.skeletonAnimation.AnimationState.AddAnimation(0, enemyNecromancer.IdleAnimation, true, 0.0f);
      Vector3 randomDirection = enemyNecromancer.GetRandomDirection();
      float distance = UnityEngine.Random.Range(1f, 4f);
      Vector3 Position = enemyNecromancer.transform.position + randomDirection * distance;
      if ((bool) Physics2D.Raycast((Vector2) enemyNecromancer.transform.position, (Vector2) randomDirection, distance, (int) enemyNecromancer.layerToCheck))
      {
        Position = enemyNecromancer.transform.position + randomDirection * -2f;
        if ((bool) Physics2D.Raycast((Vector2) enemyNecromancer.transform.position, (Vector2) (randomDirection * -1f), distance, (int) enemyNecromancer.layerToCheck))
          Position = enemyNecromancer.transform.position;
      }
      GameObject gameObject = EnemySpawner.Create(Position, enemyNecromancer.transform.parent, enemyNecromancer.loadedMinionAsset);
      gameObject.SetActive(false);
      Health component1 = gameObject.GetComponent<Health>();
      component1.CanIncreaseDamageMultiplier = false;
      Interaction_Chest.Instance?.AddEnemy(component1);
      UnitObject component2 = gameObject.GetComponent<UnitObject>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        enemyNecromancer.minions.Add(component2);
        DropLootOnDeath component3 = component2.GetComponent<DropLootOnDeath>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          component3.SetAllowExtraItems(false);
      }
      ++enemyNecromancer.summonedCount;
      enemyNecromancer.EnemySpawnerGO = (GameObject) null;
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < 1.6000000238418579)
      yield return (object) null;
    enemyNecromancer.hasSummoned = true;
    enemyNecromancer.StopAllCoroutines();
    enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.ChaseTarget());
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team || !((UnityEngine.Object) component != (UnityEngine.Object) this.health))
      return;
    float Damage = component.team == Health.Team.PlayerTeam ? this.MeleeDamageToPlayer : this.MeleeDamageToTeam2;
    component.DealDamage(Damage, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public Vector3 GetRandomDirection()
  {
    double f = (double) UnityEngine.Random.Range(0.0f, 6.28318548f);
    return new Vector3(Mathf.Cos((float) f), Mathf.Sin((float) f), 0.0f);
  }

  public IEnumerator DoStunned()
  {
    EnemyNecromancer enemyNecromancer = this;
    enemyNecromancer.Stunned = true;
    enemyNecromancer.health.ArrowAttackVulnerability = 1f;
    enemyNecromancer.health.MeleeAttackVulnerability = 1f;
    enemyNecromancer.CircleCollider.enabled = true;
    enemyNecromancer.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemyNecromancer.skeletonAnimation.AnimationState.SetAnimation(0, enemyNecromancer.IdleAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < 2.0)
      yield return (object) null;
    enemyNecromancer.Stunned = false;
    enemyNecromancer.health.ArrowAttackVulnerability = 1f;
    enemyNecromancer.StopAllCoroutines();
    enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.ChaseTarget());
  }

  public IEnumerator DoMove()
  {
    EnemyNecromancer enemyNecromancer = this;
    if ((UnityEngine.Object) enemyNecromancer.DamageColliderEvents != (UnityEngine.Object) null)
      enemyNecromancer.DamageColliderEvents.SetActive(false);
    enemyNecromancer.LookAtTarget();
    Vector3 zero = Vector3.zero;
    Vector3 toPosition = (double) Vector3.Distance(enemyNecromancer.TargetObject.transform.position, enemyNecromancer.transform.position) >= (double) enemyNecromancer.FleeRadius ? enemyNecromancer.GetMoveDirection() : enemyNecromancer.GetFleeDirection();
    enemyNecromancer.state.CURRENT_STATE = StateMachine.State.Moving;
    enemyNecromancer.skeletonAnimation.AnimationState.SetAnimation(0, enemyNecromancer.DashAnimation, false);
    enemyNecromancer.skeletonAnimation.AnimationState.AddAnimation(0, enemyNecromancer.IdleAnimation, true, 0.0f);
    enemyNecromancer.DisableForces = true;
    float f = Utils.GetAngle(enemyNecromancer.transform.position, toPosition) * ((float) Math.PI / 180f);
    Vector2 force = new Vector2(enemyNecromancer.MoveForce * Mathf.Cos(f), enemyNecromancer.MoveForce * Mathf.Sin(f));
    enemyNecromancer.rb.AddForce(force);
    if (!string.IsNullOrEmpty(enemyNecromancer.MoveSFX))
      AudioManager.Instance.PlayOneShot(enemyNecromancer.MoveSFX, enemyNecromancer.transform.position);
    float progress = 0.0f;
    while ((double) (progress += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < 1.0)
    {
      enemyNecromancer.LookAtTarget();
      yield return (object) null;
    }
    enemyNecromancer.DisableForces = false;
    enemyNecromancer.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyNecromancer.StopAllCoroutines();
    enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.ChaseTarget());
  }

  public Vector3 GetFleeDirection()
  {
    return this.transform.position - this.TargetObject.transform.position;
  }

  public Vector3 GetMoveDirection()
  {
    Vector3 direction = Vector3.right;
    float num = 100f;
    while ((double) --num > 0.0)
    {
      this.randomDirection += (float) UnityEngine.Random.Range(-45, 45) * ((float) Math.PI / 180f);
      float radius = 0.1f;
      direction = (this.transform.position + new Vector3(Mathf.Cos(this.randomDirection), Mathf.Sin(this.randomDirection))).normalized;
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) direction, 1f, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
        this.randomDirection += 0.17453292f;
      else
        break;
    }
    return direction;
  }

  public IEnumerator DoCloseCombatAttack()
  {
    EnemyNecromancer enemyNecromancer = this;
    enemyNecromancer.ClearPaths();
    enemyNecromancer.state.CURRENT_STATE = StateMachine.State.Attacking;
    float Progress = 0.0f;
    for (int i = 0; i < enemyNecromancer.MeleeAttackCount; ++i)
    {
      enemyNecromancer.PlayAttackSFX();
      enemyNecromancer.skeletonAnimation.AnimationState.SetAnimation(0, enemyNecromancer.AttackAnimation, false);
      enemyNecromancer.skeletonAnimation.AnimationState.AddAnimation(0, enemyNecromancer.IdleAnimation, true, 0.0f);
      enemyNecromancer.LookAtTarget();
      while ((double) (Progress += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < (double) enemyNecromancer.SignPostCloseCombatDelay)
      {
        if ((double) Progress >= (double) enemyNecromancer.SignPostCloseCombatDelay - (double) enemyNecromancer.signPostParryWindow)
          enemyNecromancer.canBeParried = true;
        enemyNecromancer.simpleSpineFlash.FlashWhite(Progress / enemyNecromancer.SignPostCloseCombatDelay);
        yield return (object) null;
      }
      enemyNecromancer.speed = 0.2f;
      enemyNecromancer.simpleSpineFlash.FlashWhite(false);
      Progress = 0.0f;
      float Duration = 0.2f;
      while ((double) (Progress += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < (double) Duration)
      {
        if ((UnityEngine.Object) enemyNecromancer.DamageColliderEvents != (UnityEngine.Object) null)
          enemyNecromancer.DamageColliderEvents.SetActive(true);
        enemyNecromancer.canBeParried = (double) Progress <= (double) enemyNecromancer.attackParryWindow;
        yield return (object) null;
      }
      if ((UnityEngine.Object) enemyNecromancer.DamageColliderEvents != (UnityEngine.Object) null)
        enemyNecromancer.DamageColliderEvents.SetActive(false);
      enemyNecromancer.canBeParried = false;
      yield return (object) CoroutineStatics.WaitForScaledSeconds(enemyNecromancer.DelayBetweenMeleeAttacks, enemyNecromancer.skeletonAnimation);
    }
    enemyNecromancer.CloseCombatCooldown = 1f;
    enemyNecromancer.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyNecromancer.moveDelay = 0.0f;
    enemyNecromancer.StopAllCoroutines();
    enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.ChaseTarget());
  }

  public IEnumerator DoThrowMinionsAttack(int count)
  {
    EnemyNecromancer enemyNecromancer = this;
    List<UnitObject> minions = enemyNecromancer.GetAvailableMinions(count);
    enemyNecromancer.isThrowing = true;
    enemyNecromancer.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.DoNecromancerRiseAnim());
    if (minions.Count > 0)
    {
      foreach (UnitObject minion in minions)
      {
        if (minion.health.team == enemyNecromancer.health.team)
        {
          EnemySwordsman enemySwordsman = (EnemySwordsman) minion;
          Health component = minion.GetComponent<Health>();
          enemyNecromancer.DisableMinionForThrowing(minion, component);
          enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.RaiseMinionIntoAirForThrowing(minion, enemySwordsman.Spine));
        }
      }
      float t = 0.0f;
      while ((double) t < (double) enemyNecromancer.PauseBeforeThrow + (double) enemyNecromancer.PauseBeforeRise)
      {
        t += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale;
        yield return (object) null;
      }
      for (int i = 0; i < minions.Count; ++i)
      {
        if ((UnityEngine.Object) minions[i] == (UnityEngine.Object) null || minions[i].state.CURRENT_STATE != StateMachine.State.PickedUp)
        {
          enemyNecromancer.skeletonAnimation.AnimationState.SetAnimation(0, enemyNecromancer.IdleAnimation, true);
        }
        else
        {
          Health closestTarget = enemyNecromancer.GetClosestTarget();
          if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null)
          {
            enemyNecromancer.KillAllMinionsOfState(StateMachine.State.PickedUp);
            break;
          }
          enemyNecromancer.LookAtTarget();
          Vector3 position = closestTarget.transform.position;
          AudioManager.Instance.PlayOneShot(enemyNecromancer.AttackSkeletonThrowTossVO, enemyNecromancer.gameObject);
          AudioManager.Instance.PlayOneShot(enemyNecromancer.AttackSkeletonThrowTossSFX, enemyNecromancer.gameObject);
          enemyNecromancer.skeletonAnimation.AnimationState.SetAnimation(0, enemyNecromancer.ThrowAnimation, false);
          enemyNecromancer.skeletonAnimation.AnimationState.AddAnimation(0, enemyNecromancer.IdleAnimation, true, 0.0f);
          yield return (object) enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.MoveMinionTowardsTarget(minions[i], position));
          t = 0.0f;
          while ((double) t < (double) enemyNecromancer.DelayBetweenMinionThrow)
          {
            t += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale;
            yield return (object) null;
          }
        }
      }
    }
    enemyNecromancer.simpleSpineFlash.FlashWhite(false);
    enemyNecromancer.isThrowing = false;
    enemyNecromancer.StopAllCoroutines();
    enemyNecromancer.throwRoutine = (Coroutine) null;
    enemyNecromancer.StartCoroutine((IEnumerator) enemyNecromancer.ChaseTarget());
  }

  public void LookAtTarget()
  {
    if (!((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null) || (double) this.skeletonAnimation.timeScale <= 1.0 / 1000.0)
      return;
    this.state.facingAngle = this.state.LookAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
    this.skeletonAnimation.Skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
  }

  public IEnumerator DoNecromancerRiseAnim()
  {
    EnemyNecromancer enemyNecromancer = this;
    AudioManager.Instance.PlayOneShot(enemyNecromancer.AttackSkeletonLiftSFX, enemyNecromancer.gameObject);
    AudioManager.Instance.PlayOneShot(enemyNecromancer.AttackSkeletonLiftVO, enemyNecromancer.gameObject);
    enemyNecromancer.skeletonAnimation.AnimationState.SetAnimation(0, enemyNecromancer.LevitateAnimation, false);
    enemyNecromancer.skeletonAnimation.AnimationState.AddAnimation(0, enemyNecromancer.IdleAnimation, true, 0.0f);
    enemyNecromancer.summonParticles.startDelay = 0.0f;
    enemyNecromancer.summonParticles.Play();
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale) < 1.3500000238418579)
    {
      enemyNecromancer.LookAtTarget();
      yield return (object) null;
    }
  }

  public IEnumerator MoveMinionTowardsTarget(UnitObject minion, Vector3 targetPosition)
  {
    EnemyNecromancer enemyNecromancer = this;
    if (!((UnityEngine.Object) minion == (UnityEngine.Object) null))
    {
      minion.state.CURRENT_STATE = StateMachine.State.Flying;
      Vector3 startPos = minion.transform.position;
      targetPosition.z = 0.0f;
      float num = 10f;
      float attackDuration = Vector3.Distance(minion.transform.position, targetPosition) / num;
      enemyNecromancer.currentIndicator = ObjectPool.Spawn(enemyNecromancer.loadedIndicatorAsset, enemyNecromancer.transform.parent, targetPosition, Quaternion.identity);
      float time = 0.0f;
      while ((double) time < (double) attackDuration)
      {
        if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
        {
          enemyNecromancer.RecycleCurrentReticule();
          yield break;
        }
        time += Time.deltaTime * enemyNecromancer.skeletonAnimation.timeScale;
        float t = Mathf.Clamp01(time / attackDuration);
        minion.transform.position = Vector3.Lerp(startPos, targetPosition, enemyNecromancer.EaseInBack(t));
        yield return (object) null;
      }
      if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
      {
        enemyNecromancer.RecycleCurrentReticule();
      }
      else
      {
        minion.transform.position = targetPosition;
        minion.state.CURRENT_STATE = StateMachine.State.Dieing;
        AudioManager.Instance.PlayOneShot(enemyNecromancer.AttackSkeletonThrowExplodeSFX, minion.gameObject);
        CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
        BiomeConstants.Instance.EmitHammerEffects(minion.transform.position);
        Explosion.CreateExplosion(minion.transform.position, enemyNecromancer.health.team, enemyNecromancer.health, 1f, playSFX: false);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        enemyNecromancer.RecycleCurrentReticule();
        if (enemyNecromancer.minions.Contains(minion))
          enemyNecromancer.minions.Remove(minion);
        minion.health.invincible = false;
        minion.health.DealDamage(float.PositiveInfinity, enemyNecromancer.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
      }
    }
  }

  public IEnumerator RaiseMinionIntoAirForThrowing(UnitObject minion, SkeletonAnimation minionSpine)
  {
    if (!((UnityEngine.Object) minion == (UnityEngine.Object) null))
    {
      minionSpine.AnimationState.SetAnimation(0, this.lambSkeleFloatAnim, true);
      Collider2D component = minion.GetComponent<Collider2D>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.enabled = false;
      minion.transform.DOKill();
      minion.state.CURRENT_STATE = StateMachine.State.PickedUp;
      Vector3 startPos = minion.transform.position;
      float startZ = startPos.z;
      float time = 0.0f;
      float shakePower = 0.2f;
      while ((double) time < (double) this.PauseBeforeRise)
      {
        if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
          yield break;
        time += Time.deltaTime * this.skeletonAnimation.timeScale;
        float num1 = (float) ((double) UnityEngine.Random.value * (double) shakePower - (double) shakePower / 2.0);
        float num2 = (float) ((double) UnityEngine.Random.value * (double) shakePower - (double) shakePower / 2.0);
        if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
          minion.transform.position = new Vector3(startPos.x + num1, startPos.y + num2, 0.0f);
        yield return (object) null;
      }
      if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
        AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/vocals_shared/skeleton/lift", minion.transform.position);
      time = 0.0f;
      while ((double) time < (double) this.MinionRiseDuration)
      {
        if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
          yield break;
        time += Time.deltaTime * this.skeletonAnimation.timeScale;
        float t = this.EaseOutBack(Mathf.Clamp01(time / this.MinionRiseDuration));
        minion.transform.position = new Vector3(startPos.x, startPos.y, Mathf.Lerp(startZ, -this.MinionRiseHeight, t));
        yield return (object) null;
      }
      if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
        minion.transform.position = new Vector3(startPos.x, startPos.y, -this.MinionRiseHeight);
    }
  }

  public float EaseOutBack(float t)
  {
    float num = 1.70158f;
    return (float) (1.0 + (double) (num + 1f) * (double) Mathf.Pow(t - 1f, 3f) + (double) num * (double) Mathf.Pow(t - 1f, 2f));
  }

  public float EaseInBack(float t)
  {
    float num = 1.70158f;
    return (float) (((double) num + 1.0) * (double) t * (double) t * (double) t - (double) num * (double) t * (double) t);
  }

  public void DisableMinionForThrowing(UnitObject minion, Health minionHealth)
  {
    minion.state.CURRENT_STATE = StateMachine.State.PickedUp;
    minion.enabled = false;
    minionHealth.CanBeCharmed = false;
  }

  public List<UnitObject> GetAvailableMinions(int numberToGet)
  {
    List<UnitObject> availableMinions = new List<UnitObject>();
    int num = 0;
    foreach (UnitObject minion in this.minions)
    {
      if ((double) minion.health.HP > 0.0 && minion.health.team == this.health.team)
      {
        availableMinions.Add(minion);
        ++num;
      }
      if (num >= numberToGet)
        break;
    }
    return availableMinions;
  }

  public void OnCharmed()
  {
    this.StopAllCoroutines();
    this.KillAllMinionsOfState(StateMachine.State.Flying);
    this.KillAllMinionsOfState(StateMachine.State.PickedUp);
    this.RecycleCurrentReticule();
    this.StartCoroutine((IEnumerator) this.ChaseTarget());
  }

  public void RecycleCurrentReticule()
  {
    if ((bool) (UnityEngine.Object) this.currentIndicator)
      ObjectPool.Recycle(this.currentIndicator);
    this.currentIndicator = (GameObject) null;
  }

  public void OnDrawGizmos()
  {
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

  public void StopSpookyLoop()
  {
    if (!this.IsLoopPlaying(this.spookyLoopSFX))
      return;
    AudioManager.Instance.StopLoop(this.spookyLoopSFX);
  }

  public void ReleaseAddressablesHandles()
  {
    for (int index = this.addressablesHandles.Count - 1; index >= 0; --index)
      Addressables.Release<GameObject>(this.addressablesHandles[index]);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__90_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedIndicatorAsset = obj.Result;
    obj.Result.CreatePool(this.IndicatorCount, true);
  }

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__90_1(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedMinionAsset = obj.Result;
    obj.Result.CreatePool(this.IndicatorCount, true);
  }
}
