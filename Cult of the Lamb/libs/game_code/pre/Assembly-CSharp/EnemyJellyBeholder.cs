// Decompiled with JetBrains decompiler
// Type: EnemyJellyBeholder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyJellyBeholder : EnemyChaser
{
  [SerializeField]
  private bool chase;
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
  [SerializeField]
  public SkeletonAnimation effectsSpine;
  public ParticleSystem summonParticles;
  public SkeletonAnimation Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string anticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string flyInAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string flyOutAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string attackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  protected string summonAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string summonAnimation;
  [Space]
  [SerializeField]
  private GameObject shadow;
  private float fleeTimestamp;
  private float repathTimestamp;
  protected float initialSpawnTimestamp;
  private float randomDirection;
  private int patrolIndex;
  private Vector3 startPosition;
  protected float distanceToTarget = float.MaxValue;
  private float repathTimeInterval = 2f;
  [SerializeField]
  private List<EnemyJellyBeholder.RoundsOfEnemies> enemyRounds = new List<EnemyJellyBeholder.RoundsOfEnemies>();
  private List<Health> spawnedEnemies = new List<Health>();
  private int deathCount;
  private int currentRoundIndex;
  private bool isEnemiesSpawned;
  [SerializeField]
  private EnemyJellyBeholder.BeholderState beholderState = EnemyJellyBeholder.BeholderState.Idle;
  [SerializeField]
  private float spawningChargeUpTime;
  [SerializeField]
  private float hidingExitTime;
  [SerializeField]
  private float minTimeBetweenSpawns;
  private float lastSpawnTimestamp;
  [SerializeField]
  private bool killSpawnablesOnDeath;
  [SerializeField]
  private SpriteRenderer aiming;
  [SerializeField]
  private EnemyJellyBeholder.AttackType[] attackPattern;
  private int attackPatternIndex;
  [SerializeField]
  private bool attackOnHit = true;
  [SerializeField]
  private float attackDistance;
  [SerializeField]
  private float attackChargeDur;
  [SerializeField]
  private float attackCooldown;
  private bool canAttack;
  [SerializeField]
  private float attackDuration = 1f;
  [SerializeField]
  private float damageDuration = 0.2f;
  [SerializeField]
  private float attackForceModifier = 1f;
  [SerializeField]
  private int numAttacks = 1;
  [SerializeField]
  private float timeBetweenMultiAttacks = 0.5f;
  [SerializeField]
  private GameObject ringshotArrowPrefab;
  [SerializeField]
  private bool ringshotArrowIsBomb;
  [SerializeField]
  private bool ringshotArrowIsPoison;
  [SerializeField]
  private int numRingShots;
  [SerializeField]
  private float ringShotAngleArc = 360f;
  [SerializeField]
  private float ringShotDelay = 0.05f;
  [SerializeField]
  private GameObject targetedArrowPrefab;
  [SerializeField]
  private bool targetedArrowIsBomb;
  [SerializeField]
  private bool targetedArrowIsPoison;
  [SerializeField]
  private int numTargetedShots;
  [SerializeField]
  private float targetedShotDelay = 0.05f;
  [SerializeField]
  private float arrowSpeed = 9f;
  private const float bombDuration = 1f;
  private const float minBombRange = 1f;
  private float attackTimer;
  private Coroutine spawnRoutine;
  private ProjectilePattern projectilePattern;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string EnemySpawnSfx = string.Empty;
  [EventRef]
  public string ShootSFX = "event:/enemy/shoot_magicenergy";
  [EventRef]
  public string ShootSpikeSfx = "event:/enemy/shoot_arrowspike";
  [EventRef]
  public string SummonSfx = "event:/enemy/summon";
  [EventRef]
  public string SummonedSfx = "event:/enemy/summoned";
  private List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  private Vector3 v3Shake = Vector3.zero;
  private Vector3 v3ShakeSpeed = Vector3.zero;

  public override void Awake()
  {
    this.Spine.ForceVisible = true;
    base.Awake();
    MiniBossController componentInParent = this.GetComponentInParent<MiniBossController>();
    if (!((UnityEngine.Object) componentInParent != (UnityEngine.Object) null) || !DataManager.Instance.CheckKilledBosses(componentInParent.name) || this.Spine.AnimationState == null)
      return;
    string skinName = "Dungeon1_Beaten";
    switch (componentInParent.name)
    {
      case "Boss Beholder 1":
        skinName = "Dungeon1_Beaten";
        break;
      case "Boss Beholder 2":
        skinName = "Dungeon2_Beaten";
        break;
      case "Boss Beholder 3":
        skinName = "Dungeon3_Beaten";
        break;
      case "Boss Beholder 4":
        skinName = "Dungeon4_Beaten";
        break;
    }
    this.Spine.Skeleton.SetSkin(skinName);
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

  protected override void Start()
  {
    base.Start();
    this.startPosition = this.transform.position;
    if (this.patrolRoute.Count > 0)
      this.patrolRoute.Insert(0, Vector3.zero);
    if ((bool) (UnityEngine.Object) this.gm)
      this.health.enabled = false;
    this.aiming.gameObject.SetActive(false);
    this.projectilePattern = this.GetComponent<ProjectilePattern>();
    if ((UnityEngine.Object) this.projectilePattern != (UnityEngine.Object) null && (UnityEngine.Object) this.projectilePattern.BulletPrefab != (UnityEngine.Object) null && this.projectilePattern.Waves != null && this.projectilePattern.Waves.Length != 0)
    {
      int initialPoolSize = 0;
      for (int index = 0; index < this.projectilePattern.Waves.Length; ++index)
        initialPoolSize += this.projectilePattern.Waves[index].Bullets;
      ObjectPool.CreatePool<Projectile>(this.projectilePattern.BulletPrefab, initialPoolSize, true);
    }
    if ((UnityEngine.Object) this.ringshotArrowPrefab != (UnityEngine.Object) null && this.numRingShots > 0)
      ObjectPool.CreatePool(this.ringshotArrowPrefab, this.numRingShots, true);
    Health.team2.RemoveAll((Predicate<Health>) (unit => (UnityEngine.Object) unit == (UnityEngine.Object) null));
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.beholderState != EnemyJellyBeholder.BeholderState.Hiding)
      this.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.canAttack = true;
  }

  public override void Update()
  {
    base.Update();
    if ((double) this.gm.CurrentTime > (double) this.initialSpawnTimestamp && !this.health.enabled)
    {
      this.health.enabled = true;
      this.canAttack = true;
    }
    if (this.IsTimeToComeBack())
      this.StartCoroutine((IEnumerator) this.EnemiesDefeated());
    if ((double) Time.deltaTime > 0.0)
    {
      this.v3ShakeSpeed += (Vector3.zero - this.v3Shake) * 0.4f / Time.deltaTime;
      this.v3Shake += (this.v3ShakeSpeed *= 0.7f) * Time.deltaTime;
      this.Spine.transform.localPosition = this.v3Shake;
    }
    if ((bool) (UnityEngine.Object) this.targetObject)
      this.inRange = (double) Vector3.Distance(this.targetObject.transform.position, this.transform.position) < (double) this.VisionRange;
    if (this.ShouldStartSpawnEnemies())
      this.SpawnEnemies();
    if (this.attackPattern.Length == 0)
      return;
    if (this.beholderState == EnemyJellyBeholder.BeholderState.ChargingUpAttack)
    {
      this.attackTimer += Time.deltaTime;
      float num = this.attackTimer / this.attackChargeDur;
      this.simpleSpineFlash.FlashWhite(num * 0.75f);
      if (this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.Melee || this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.TargetedShot && !this.targetedArrowIsBomb || this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.PatternShot)
      {
        this.aiming.gameObject.SetActive(true);
        if (Time.frameCount % 5 == 0)
          this.aiming.color = this.aiming.color == Color.red ? Color.white : Color.red;
      }
      else
        this.aiming.gameObject.SetActive(false);
      if (((double) num < 0.5 || this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.TargetedShot || this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.PatternShot) && (UnityEngine.Object) this.targetObject != (UnityEngine.Object) null)
        this.LookAtAngle(Utils.GetAngle(this.transform.position, this.targetObject.transform.position));
      this.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.state.LookAngle);
      if ((double) num >= 1.0)
      {
        this.aiming.gameObject.SetActive(false);
        if (this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.Melee)
          this.MeleeAttack();
        else if (this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.RingShot)
          this.RingShotAttack();
        else if (this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.TargetedShot)
          this.TargetedShotAttack();
        else if (this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.PatternShot)
          this.PatternShotAttack();
      }
    }
    if (!this.canAttack || !(bool) (UnityEngine.Object) this.targetObject || (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) >= (double) this.attackDistance)
      return;
    this.ChargeAttack();
  }

  private bool IsTimeToComeBack()
  {
    return this.currentRoundIndex < this.enemyRounds.Count && this.isEnemiesSpawned && this.spawnedEnemies.Count <= 0;
  }

  private bool ShouldStartSpawnEnemies()
  {
    return (this.beholderState == EnemyJellyBeholder.BeholderState.Idle || this.beholderState == EnemyJellyBeholder.BeholderState.Moving) && (double) this.gm.TimeSince(this.lastSpawnTimestamp) >= (double) this.minTimeBetweenSpawns && this.currentRoundIndex < this.enemyRounds.Count && (double) this.health.HP <= (double) this.health.totalHP * (double) this.enemyRounds[this.currentRoundIndex].beholderHealthThresholdToTriggerRound;
  }

  private void ChargeAttack()
  {
    if (this.beholderState != EnemyJellyBeholder.BeholderState.Idle && this.beholderState != EnemyJellyBeholder.BeholderState.Moving)
      return;
    this.canAttack = false;
    this.attackTimer = 0.0f;
    this.beholderState = EnemyJellyBeholder.BeholderState.ChargingUpAttack;
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", this.gameObject);
    this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
    if (!((UnityEngine.Object) this.targetObject != (UnityEngine.Object) null))
      return;
    this.LookAtAngle(Utils.GetAngle(this.transform.position, this.targetObject.transform.position));
  }

  private void SpawnEnemies()
  {
    this.spawnRoutine = this.StartCoroutine((IEnumerator) this.SpawnDelay());
  }

  private IEnumerator SpawnDelay()
  {
    EnemyJellyBeholder enemyJellyBeholder1 = this;
    enemyJellyBeholder1.beholderState = EnemyJellyBeholder.BeholderState.ChargingUpSpawn;
    enemyJellyBeholder1.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(0.3f);
    enemyJellyBeholder1.ClearPaths();
    enemyJellyBeholder1.summonParticles.Play();
    enemyJellyBeholder1.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder1.summonAnticipationAnimation, true);
    enemyJellyBeholder1.effectsSpine.AnimationState.SetAnimation(0, "anticipate-summon", true);
    AudioManager.Instance.PlayOneShot(enemyJellyBeholder1.SummonSfx, enemyJellyBeholder1.gameObject);
    yield return (object) new WaitForSeconds(enemyJellyBeholder1.spawningChargeUpTime);
    enemyJellyBeholder1.health.invincible = true;
    enemyJellyBeholder1.shadow.gameObject.SetActive(false);
    GameManager.GetInstance().RemoveFromCamera(enemyJellyBeholder1.gameObject);
    GameManager.GetInstance().AddToCamera(Interaction_Chest.Instance.gameObject);
    enemyJellyBeholder1.beholderState = EnemyJellyBeholder.BeholderState.Spawning;
    enemyJellyBeholder1.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder1.summonAnimation, false);
    enemyJellyBeholder1.effectsSpine.AnimationState.SetAnimation(0, "summon", false);
    enemyJellyBeholder1.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder1.flyOutAnimation, false, 0.0f);
    enemyJellyBeholder1.effectsSpine.AnimationState.AddAnimation(0, "fly-out", false, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyJellyBeholder1.SummonedSfx, enemyJellyBeholder1.gameObject);
    if (enemyJellyBeholder1.enemyRounds != null && enemyJellyBeholder1.enemyRounds.Count > 0)
    {
      enemyJellyBeholder1.deathCount = 0;
      foreach (EnemyRounds.EnemyAndPosition enemyAndPosition in enemyJellyBeholder1.enemyRounds[enemyJellyBeholder1.currentRoundIndex].Round)
      {
        EnemyJellyBeholder enemyJellyBeholder = enemyJellyBeholder1;
        EnemyRounds.EnemyAndPosition e = enemyAndPosition;
        Addressables.LoadAssetAsync<GameObject>((object) e.EnemyTarget).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          enemyJellyBeholder.loadedAddressableAssets.Add(obj);
          GameObject Spawn = ObjectPool.Spawn(obj.Result, enemyJellyBeholder.transform.parent, e.Position, Quaternion.identity);
          Health component = Spawn.GetComponent<Health>();
          component.gameObject.SetActive(false);
          component.OnDie += new Health.DieAction(enemyJellyBeholder.OnSpawnedDie);
          enemyJellyBeholder.spawnedEnemies.Add(component);
          EnemySpawner.CreateWithAndInitInstantiatedEnemy(e.Position, enemyJellyBeholder.transform.parent, Spawn);
        });
        yield return (object) new WaitForSeconds(e.Delay);
      }
      enemyJellyBeholder1.beholderState = EnemyJellyBeholder.BeholderState.Hiding;
      enemyJellyBeholder1.health.untouchable = true;
      enemyJellyBeholder1.isEnemiesSpawned = true;
    }
  }

  private IEnumerator EnemiesDefeated()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJellyBeholder enemyJellyBeholder = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyJellyBeholder.canAttack = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyJellyBeholder.deathCount = 0;
    ++enemyJellyBeholder.currentRoundIndex;
    enemyJellyBeholder.isEnemiesSpawned = false;
    enemyJellyBeholder.transform.position = Vector3.zero;
    enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.flyInAnimation, false);
    enemyJellyBeholder.shadow.gameObject.SetActive(true);
    enemyJellyBeholder.health.untouchable = false;
    enemyJellyBeholder.health.invincible = false;
    GameManager.GetInstance().AddToCamera(enemyJellyBeholder.gameObject);
    GameManager.GetInstance().RemoveFromCamera(Interaction_Chest.Instance.gameObject);
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.canAttack = false;
    enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.idleAnimation, true, 0.0f);
    enemyJellyBeholder.spawnRoutine = (Coroutine) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyJellyBeholder.attackCooldown);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnSpawnedDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((UnityEngine.Object) Victim == (UnityEngine.Object) null)
      return;
    Victim.OnDie -= new Health.DieAction(this.OnSpawnedDie);
    this.spawnedEnemies.Remove(Victim);
    SpawnEnemyOnDeath component = Victim.GetComponent<SpawnEnemyOnDeath>();
    if ((bool) (UnityEngine.Object) component)
    {
      if (component.SpawnedEnemies != null && component.SpawnedEnemies.Length != 0)
      {
        foreach (UnitObject spawnedEnemy in component.SpawnedEnemies)
          this.EnemySplit(spawnedEnemy);
      }
      else
      {
        component.OnEnemySpawned += new SpawnEnemyOnDeath.SpawnEvent(this.EnemySplit);
        component.OnEnemyDespawned += new SpawnEnemyOnDeath.SpawnEvent(this.EnemySplit);
      }
    }
    ++this.deathCount;
    Debug.Log((object) ("Death count: " + (object) this.deathCount));
  }

  private void EnemySplit(UnitObject enemy)
  {
    if ((UnityEngine.Object) enemy != (UnityEngine.Object) null)
    {
      enemy.health.OnDie += new Health.DieAction(this.OnSpawnedDie);
      this.spawnedEnemies.Add(enemy.health);
      this.enemyRounds[this.currentRoundIndex].Round.Add(new EnemyRounds.EnemyAndPosition(enemy.health, Vector3.zero, 0.0f));
    }
    else if (this.enemyRounds[this.currentRoundIndex].Round.Contains((EnemyRounds.EnemyAndPosition) null))
      this.enemyRounds[this.currentRoundIndex].Round.Remove((EnemyRounds.EnemyAndPosition) null);
    else
      this.enemyRounds[this.currentRoundIndex].Round.Add((EnemyRounds.EnemyAndPosition) null);
  }

  private void MeleeAttack()
  {
    if (this.beholderState == EnemyJellyBeholder.BeholderState.Attacking)
      return;
    this.attackTimer = 0.0f;
    this.beholderState = EnemyJellyBeholder.BeholderState.Attacking;
    this.canAttack = false;
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.MeleeAttackIE());
    this.attackPatternIndex = (this.attackPatternIndex + 1) % this.attackPattern.Length;
  }

  private IEnumerator MeleeAttackIE()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    for (int i = 0; i < enemyJellyBeholder.numAttacks; ++i)
    {
      enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.attackAnimation, false);
      enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.idleAnimation, true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/attack", enemyJellyBeholder.gameObject);
      yield return (object) new WaitForEndOfFrame();
      enemyJellyBeholder.StartCoroutine((IEnumerator) enemyJellyBeholder.TurnOnDamageColliderForDuration(enemyJellyBeholder.damageDuration));
      enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
      enemyJellyBeholder.DisableForces = true;
      Vector2 force = new Vector2(2500f * Mathf.Cos(enemyJellyBeholder.state.LookAngle * ((float) Math.PI / 180f)), 2500f * Mathf.Sin(enemyJellyBeholder.state.LookAngle * ((float) Math.PI / 180f))) * enemyJellyBeholder.attackForceModifier;
      enemyJellyBeholder.rb.AddForce(force);
      yield return (object) new WaitForSeconds(enemyJellyBeholder.attackDuration);
      enemyJellyBeholder.DisableForces = false;
      if (i < enemyJellyBeholder.numAttacks - 1)
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", enemyJellyBeholder.gameObject);
        enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.anticipationAnimation, true);
        float t = 0.0f;
        while ((double) t < (double) enemyJellyBeholder.timeBetweenMultiAttacks)
        {
          t += Time.deltaTime;
          enemyJellyBeholder.simpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyJellyBeholder.timeBetweenMultiAttacks * 0.75));
          enemyJellyBeholder.aiming.gameObject.SetActive(true);
          if (Time.frameCount % 5 == 0)
            enemyJellyBeholder.aiming.color = enemyJellyBeholder.aiming.color == Color.red ? Color.white : Color.red;
          if ((double) t / (double) enemyJellyBeholder.timeBetweenMultiAttacks < 0.5)
            enemyJellyBeholder.LookAtAngle(Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position));
          enemyJellyBeholder.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyJellyBeholder.state.LookAngle);
          yield return (object) null;
        }
        enemyJellyBeholder.aiming.gameObject.SetActive(false);
      }
    }
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(enemyJellyBeholder.attackCooldown);
    enemyJellyBeholder.canAttack = true;
  }

  private IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJellyBeholder enemyJellyBeholder = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyJellyBeholder.damageColliderEvents.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyJellyBeholder.damageColliderEvents.SetActive(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void RingShotAttack()
  {
    if (this.beholderState == EnemyJellyBeholder.BeholderState.Shooting)
      return;
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.RingShotAttackIE());
    this.attackPatternIndex = (this.attackPatternIndex + 1) % this.attackPattern.Length;
  }

  private IEnumerator RingShotAttackIE()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Shooting;
    enemyJellyBeholder.canAttack = false;
    int i = enemyJellyBeholder.numRingShots;
    float aimingAngle = enemyJellyBeholder.state.LookAngle;
    if ((UnityEngine.Object) enemyJellyBeholder.targetObject != (UnityEngine.Object) null)
      aimingAngle = (float) ((double) Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position) - (double) enemyJellyBeholder.ringShotAngleArc / 2.0 - (double) enemyJellyBeholder.ringShotAngleArc * 0.20000000298023224);
    enemyJellyBeholder.aiming.gameObject.SetActive(false);
    enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.attackAnimation, false);
    enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.idleAnimation, true, 0.0f);
    while (--i >= 0)
    {
      enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
      CameraManager.shakeCamera(0.2f, aimingAngle);
      if (enemyJellyBeholder.ringshotArrowIsBomb)
      {
        Vector3 vector3 = enemyJellyBeholder.transform.position + (Vector3) Utils.DegreeToVector2(aimingAngle) * UnityEngine.Random.Range(1f, 6f);
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyJellyBeholder.ringshotArrowPrefab, vector3, Quaternion.identity, enemyJellyBeholder.transform.parent);
        if (enemyJellyBeholder.ringshotArrowIsPoison)
        {
          gameObject.GetComponent<PoisonBomb>().Play(enemyJellyBeholder.transform.position, 1f);
          AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", enemyJellyBeholder.transform.position);
        }
        else
        {
          MortarBomb component = gameObject.GetComponent<MortarBomb>();
          if ((double) Vector2.Distance((Vector2) enemyJellyBeholder.transform.position, (Vector2) vector3) < 1.0)
          {
            Vector2 position = (Vector2) (enemyJellyBeholder.transform.position + (vector3 - enemyJellyBeholder.transform.position).normalized * 1f);
            component.transform.position = (Vector3) AstarPath.active.GetNearest((Vector3) position).node.position;
          }
          else
            component.transform.position = (Vector3) AstarPath.active.GetNearest(vector3).node.position;
          component.Play(enemyJellyBeholder.transform.position + new Vector3(0.0f, 0.0f, -1.5f), 1f, Health.Team.Team2);
          AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", enemyJellyBeholder.transform.position);
        }
      }
      else
      {
        Projectile component = ObjectPool.Spawn(enemyJellyBeholder.ringshotArrowPrefab, enemyJellyBeholder.transform.parent).GetComponent<Projectile>();
        component.transform.position = enemyJellyBeholder.transform.position + new Vector3(0.0f, 0.0f, -0.5f) + (Vector3) Utils.DegreeToVector2(aimingAngle) * 0.66f;
        component.Angle = aimingAngle;
        component.team = enemyJellyBeholder.health.team;
        component.Speed = enemyJellyBeholder.arrowSpeed * 0.666f;
        component.Owner = enemyJellyBeholder.health;
        AudioManager.Instance.PlayOneShot(enemyJellyBeholder.ShootSFX, enemyJellyBeholder.transform.position);
      }
      aimingAngle += enemyJellyBeholder.ringShotAngleArc / (float) Mathf.Max(enemyJellyBeholder.numRingShots, 0);
      float Progress = 0.0f;
      while ((double) Progress < (double) enemyJellyBeholder.ringShotDelay)
      {
        Progress += Time.deltaTime;
        yield return (object) null;
      }
    }
    enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(enemyJellyBeholder.attackCooldown);
    enemyJellyBeholder.canAttack = true;
  }

  private void TargetedShotAttack()
  {
    if (this.beholderState == EnemyJellyBeholder.BeholderState.Shooting)
      return;
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.TargetedShotAttackIE());
    this.attackPatternIndex = (this.attackPatternIndex + 1) % this.attackPattern.Length;
  }

  private IEnumerator TargetedShotAttackIE()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Shooting;
    enemyJellyBeholder.canAttack = false;
    int i = enemyJellyBeholder.numTargetedShots;
    float aimingAngle = enemyJellyBeholder.state.LookAngle;
    if (!enemyJellyBeholder.targetedArrowIsBomb)
      enemyJellyBeholder.aiming.gameObject.SetActive(true);
    if ((UnityEngine.Object) enemyJellyBeholder.targetObject != (UnityEngine.Object) null)
    {
      aimingAngle = Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position);
      enemyJellyBeholder.LookAtAngle(Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position));
    }
    enemyJellyBeholder.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyJellyBeholder.state.LookAngle);
    while (--i >= 0)
    {
      enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
      CameraManager.shakeCamera(0.2f, aimingAngle);
      if (enemyJellyBeholder.targetedArrowIsBomb)
      {
        Vector3 vector3 = enemyJellyBeholder.targetObject.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyJellyBeholder.targetedArrowPrefab, vector3, Quaternion.identity, enemyJellyBeholder.transform.parent);
        AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", enemyJellyBeholder.transform.position);
        if (enemyJellyBeholder.targetedArrowIsPoison)
        {
          gameObject.GetComponent<PoisonBomb>().Play(enemyJellyBeholder.transform.position, 1f);
        }
        else
        {
          MortarBomb component = gameObject.GetComponent<MortarBomb>();
          if ((double) Vector2.Distance((Vector2) enemyJellyBeholder.transform.position, (Vector2) vector3) < 1.0)
            component.transform.position = enemyJellyBeholder.transform.position + (vector3 - enemyJellyBeholder.transform.position).normalized * 1f;
          else
            component.transform.position = vector3;
          component.Play(enemyJellyBeholder.transform.position + new Vector3(0.0f, 0.0f, -1.5f), 1f, Health.Team.Team2);
        }
      }
      else
      {
        if (enemyJellyBeholder.numTargetedShots > 1)
          aimingAngle += UnityEngine.Random.Range(-8f, 8f);
        Projectile component = ObjectPool.Spawn(enemyJellyBeholder.targetedArrowPrefab, enemyJellyBeholder.transform.parent).GetComponent<Projectile>();
        component.transform.position = enemyJellyBeholder.transform.position + new Vector3(0.0f, 0.0f, -0.5f) + (Vector3) Utils.DegreeToVector2(aimingAngle);
        component.Angle = aimingAngle;
        component.team = enemyJellyBeholder.health.team;
        component.Speed = enemyJellyBeholder.arrowSpeed;
        component.Owner = enemyJellyBeholder.health;
        AudioManager.Instance.PlayOneShot(enemyJellyBeholder.ShootSFX, enemyJellyBeholder.transform.position);
      }
      enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.attackAnimation, false);
      enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.idleAnimation, true, 0.0f);
      float Progress = 0.0f;
      if (i == 0)
        enemyJellyBeholder.aiming.gameObject.SetActive(false);
      while ((double) (Progress += Time.deltaTime) < (double) enemyJellyBeholder.targetedShotDelay)
      {
        if ((UnityEngine.Object) enemyJellyBeholder.targetObject != (UnityEngine.Object) null)
        {
          aimingAngle = Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position);
          enemyJellyBeholder.LookAtAngle(Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position));
        }
        if (Time.frameCount % 5 == 0)
          enemyJellyBeholder.aiming.color = enemyJellyBeholder.aiming.color == Color.red ? Color.white : Color.red;
        enemyJellyBeholder.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyJellyBeholder.state.LookAngle);
        yield return (object) null;
      }
    }
    enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(enemyJellyBeholder.attackCooldown);
    enemyJellyBeholder.canAttack = true;
  }

  private void PatternShotAttack()
  {
    if (this.beholderState == EnemyJellyBeholder.BeholderState.Shooting)
      return;
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.PatternShotAttackIE());
    this.attackPatternIndex = (this.attackPatternIndex + 1) % this.attackPattern.Length;
  }

  private IEnumerator PatternShotAttackIE()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Shooting;
    enemyJellyBeholder.canAttack = false;
    float direction = enemyJellyBeholder.state.LookAngle;
    if ((UnityEngine.Object) enemyJellyBeholder.targetObject != (UnityEngine.Object) null)
    {
      direction = Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position);
      enemyJellyBeholder.LookAtAngle(Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position));
    }
    enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
    CameraManager.shakeCamera(0.2f, direction);
    if ((bool) (UnityEngine.Object) enemyJellyBeholder.projectilePattern)
      enemyJellyBeholder.projectilePattern.Shoot();
    AudioManager.Instance.PlayOneShot(enemyJellyBeholder.ShootSFX, enemyJellyBeholder.transform.position);
    enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.attackAnimation, false);
    enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.idleAnimation, true, 0.0f);
    enemyJellyBeholder.aiming.gameObject.SetActive(false);
    enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    yield return (object) new WaitForSeconds(enemyJellyBeholder.attackCooldown);
    enemyJellyBeholder.canAttack = true;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (this.attackOnHit && AttackType == Health.AttackTypes.Melee)
    {
      this.ChargeAttack();
    }
    else
    {
      base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
      this.simpleSpineFlash.FlashWhite(false);
      this.simpleSpineFlash.FlashFillRed();
      AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/gethit", this.transform.position);
    }
    this.v3ShakeSpeed = this.transform.position - Attacker.transform.position;
    this.v3ShakeSpeed = this.v3ShakeSpeed.normalized * 20f;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (this.killSpawnablesOnDeath)
    {
      foreach (Health spawnedEnemy in this.spawnedEnemies)
      {
        if ((UnityEngine.Object) spawnedEnemy != (UnityEngine.Object) null)
          spawnedEnemy.DealDamage(spawnedEnemy.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy, dealDamageImmediately: true);
      }
      foreach (EnemySpider enemySpider in EnemySpider.EnemySpiders)
      {
        if ((bool) (UnityEngine.Object) enemySpider)
          enemySpider.health.DealDamage(enemySpider.health.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy, dealDamageImmediately: true);
      }
    }
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/death", this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/enemy_death_large", this.transform.position);
  }

  protected override void UpdateMoving()
  {
    if (this.chase)
      base.UpdateMoving();
    else if (this.patrol && (this.state.CURRENT_STATE == StateMachine.State.Idle || (double) this.gm.CurrentTime > (double) this.repathTimestamp) && (this.beholderState == EnemyJellyBeholder.BeholderState.Idle || this.beholderState == EnemyJellyBeholder.BeholderState.Moving))
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

  private IEnumerator DelayedDestroy()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJellyBeholder enemyJellyBeholder = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) enemyJellyBeholder.gameObject);
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

  private void OnDrawGizmos()
  {
    if (this.patrol)
    {
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
    Gradient gradient = new Gradient();
    GradientColorKey[] gradientColorKeyArray = new GradientColorKey[2];
    GradientAlphaKey[] gradientAlphaKeyArray = new GradientAlphaKey[2];
    GradientColorKey[] colorKeys = new GradientColorKey[2];
    colorKeys[0].color = Color.red;
    colorKeys[0].time = 0.0f;
    colorKeys[1].color = Color.blue;
    colorKeys[1].time = 1f;
    GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
    alphaKeys[0].alpha = 1f;
    alphaKeys[0].time = 0.0f;
    alphaKeys[1].alpha = 1f;
    alphaKeys[1].time = 1f;
    gradient.SetKeys(colorKeys, alphaKeys);
    int index1 = -1;
    while (++index1 < this.enemyRounds.Count)
    {
      EnemyJellyBeholder.RoundsOfEnemies enemyRound = this.enemyRounds[index1];
      if (enemyRound.DisplayGizmo)
      {
        foreach (EnemyRounds.EnemyAndPosition enemyAndPosition in enemyRound.Round)
          Utils.DrawCircleXY(enemyAndPosition.Position, 0.2f, gradient.Evaluate((float) index1 / (float) this.enemyRounds.Count));
      }
    }
  }

  [Serializable]
  public class RoundsOfEnemies
  {
    public bool DisplayGizmo;
    public float beholderHealthThresholdToTriggerRound = 1f;
    public List<EnemyRounds.EnemyAndPosition> Round = new List<EnemyRounds.EnemyAndPosition>();
  }

  private enum BeholderState
  {
    Idle = 1,
    Moving = 2,
    ChargingUpSpawn = 3,
    Spawning = 4,
    Hiding = 5,
    ChargingUpAttack = 6,
    Attacking = 8,
    Shooting = 9,
  }

  public enum AttackType
  {
    Melee,
    TargetedShot,
    RingShot,
    PatternShot,
  }
}
