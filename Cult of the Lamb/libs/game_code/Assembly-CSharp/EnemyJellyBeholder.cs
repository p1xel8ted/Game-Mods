// Decompiled with JetBrains decompiler
// Type: EnemyJellyBeholder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  [SerializeField]
  public SkeletonAnimation effectsSpine;
  public ParticleSystem summonParticles;
  public SkeletonAnimation Spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string anticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string flyInAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string flyOutAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string summonAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string summonAnimation;
  [Space]
  [SerializeField]
  public GameObject shadow;
  public float fleeTimestamp;
  public float repathTimestamp;
  public float initialSpawnTimestamp;
  public float randomDirection;
  public int patrolIndex;
  public Vector3 startPosition;
  public float distanceToTarget = float.MaxValue;
  public float repathTimeInterval = 2f;
  [SerializeField]
  public List<EnemyJellyBeholder.RoundsOfEnemies> enemyRounds = new List<EnemyJellyBeholder.RoundsOfEnemies>();
  public List<Health> spawnedEnemies = new List<Health>();
  public int deathCount;
  public int currentRoundIndex;
  public bool isEnemiesSpawned;
  [SerializeField]
  public EnemyJellyBeholder.BeholderState beholderState = EnemyJellyBeholder.BeholderState.Idle;
  [SerializeField]
  public float spawningChargeUpTime;
  [SerializeField]
  public float hidingExitTime;
  [SerializeField]
  public float minTimeBetweenSpawns;
  public float lastSpawnTimestamp;
  [SerializeField]
  public bool killSpawnablesOnDeath;
  [SerializeField]
  public SpriteRenderer aiming;
  [SerializeField]
  public EnemyJellyBeholder.AttackType[] attackPattern;
  [SerializeField]
  public bool randomPattern;
  public int attackPatternIndex;
  [SerializeField]
  public bool attackOnHit = true;
  [SerializeField]
  public float attackDistance;
  [SerializeField]
  public float attackChargeDur;
  [SerializeField]
  public float attackCooldown;
  public bool canAttack;
  public float flashTickTimer;
  [SerializeField]
  public float attackDuration = 1f;
  [SerializeField]
  public float damageDuration = 0.2f;
  [SerializeField]
  public float attackForceModifier = 1f;
  [SerializeField]
  public int numAttacks = 1;
  [SerializeField]
  public float timeBetweenMultiAttacks = 0.5f;
  [SerializeField]
  public GameObject ringshotArrowPrefab;
  [SerializeField]
  public bool ringshotArrowIsBomb;
  [SerializeField]
  public bool ringshotArrowIsPoison;
  [SerializeField]
  public int numRingShots;
  [SerializeField]
  public float ringShotAngleArc = 360f;
  [SerializeField]
  public float ringShotDelay = 0.05f;
  [SerializeField]
  public GameObject targetedArrowPrefab;
  [SerializeField]
  public bool targetedArrowIsBomb;
  [SerializeField]
  public bool targetedArrowIsPoison;
  [SerializeField]
  public bool targetedArrowIsGrenade;
  [SerializeField]
  public int numTargetedShots;
  [SerializeField]
  public float targetedShotDelay = 0.05f;
  [SerializeField]
  public bool targetedShowAiming = true;
  [SerializeField]
  public float arrowSpeed = 9f;
  public const float bombDuration = 1f;
  public const float minBombRange = 1f;
  public float attackTimer;
  public Coroutine spawnRoutine;
  public ProjectilePattern projectilePattern;
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
  public string TargetedShotAttackBeginSFX;
  public string TargetedShotAttackIndividualSFX = "event:/boss/spider/bomb_shoot";
  public string RingShotAttackBeginSFX = "";
  public Dictionary<AssetReferenceGameObject, AsyncOperationHandle<GameObject>> loadedAddressableAssets = new Dictionary<AssetReferenceGameObject, AsyncOperationHandle<GameObject>>();
  public Vector3 v3Shake = Vector3.zero;
  public Vector3 v3ShakeSpeed = Vector3.zero;

  public override void Awake()
  {
    this.Spine.ForceVisible = true;
    base.Awake();
    this.spine = this.Spine;
    MiniBossController componentInParent = this.GetComponentInParent<MiniBossController>();
    string name = (UnityEngine.Object) componentInParent != (UnityEngine.Object) null ? componentInParent.name : "";
    if (GameManager.Layer2)
      name += "_P2";
    if (!((UnityEngine.Object) componentInParent != (UnityEngine.Object) null) || !DataManager.Instance.CheckKilledBosses(name) || this.Spine.AnimationState == null)
      return;
    string skinName = "Dungeon1_Beaten";
    switch (componentInParent.name)
    {
      case "Boss Beholder 1":
        skinName = GameManager.Layer2 ? "Dungeon1_2_Beaten" : "Dungeon1_Beaten";
        break;
      case "Boss Beholder 2":
        skinName = GameManager.Layer2 ? "Dungeon2_2_Beaten" : "Dungeon2_Beaten";
        break;
      case "Boss Beholder 3":
        skinName = GameManager.Layer2 ? "Dungeon3_2_Beaten" : "Dungeon3_Beaten";
        break;
      case "Boss Beholder 4":
        skinName = GameManager.Layer2 ? "Dungeon4_2_Beaten" : "Dungeon4_Beaten";
        break;
    }
    this.Spine.Skeleton.SetSkin(skinName);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> handle in this.loadedAddressableAssets.Values)
      Addressables.Release((AsyncOperationHandle) handle);
    this.loadedAddressableAssets.Clear();
  }

  public override void Start()
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
    this.StartCoroutine((IEnumerator) this.PreloadEnemyAssets());
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
      this.attackTimer += Time.deltaTime * this.Spine.timeScale;
      float num = this.attackTimer / this.attackChargeDur;
      this.simpleSpineFlash.FlashWhite(num * 0.75f);
      if (this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.Melee || this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.TargetedShot && this.targetedShowAiming || this.attackPattern[this.attackPatternIndex] == EnemyJellyBeholder.AttackType.PatternShot)
      {
        this.aiming.gameObject.SetActive(true);
        if ((double) this.flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
        {
          this.aiming.color = this.aiming.color == Color.red ? Color.white : Color.red;
          this.flashTickTimer = 0.0f;
        }
        this.flashTickTimer += Time.deltaTime * this.Spine.timeScale;
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

  public bool IsTimeToComeBack()
  {
    return this.currentRoundIndex < this.enemyRounds.Count && this.isEnemiesSpawned && this.spawnedEnemies.Count <= 0;
  }

  public bool ShouldStartSpawnEnemies()
  {
    return (this.beholderState == EnemyJellyBeholder.BeholderState.Idle || this.beholderState == EnemyJellyBeholder.BeholderState.Moving) && (double) this.gm.TimeSince(this.lastSpawnTimestamp) >= (double) this.minTimeBetweenSpawns && this.currentRoundIndex < this.enemyRounds.Count && (double) this.health.HP <= (double) this.health.totalHP * (double) this.enemyRounds[this.currentRoundIndex].beholderHealthThresholdToTriggerRound;
  }

  public void ChargeAttack()
  {
    if (this.beholderState != EnemyJellyBeholder.BeholderState.Idle && this.beholderState != EnemyJellyBeholder.BeholderState.Moving)
      return;
    this.targetObject = this.GetClosestTarget();
    this.canAttack = false;
    this.attackTimer = 0.0f;
    this.beholderState = EnemyJellyBeholder.BeholderState.ChargingUpAttack;
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", this.gameObject);
    this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
    if (!((UnityEngine.Object) this.targetObject != (UnityEngine.Object) null))
      return;
    this.LookAtAngle(Utils.GetAngle(this.transform.position, this.targetObject.transform.position));
  }

  public void SpawnEnemies()
  {
    this.spawnRoutine = this.StartCoroutine((IEnumerator) this.SpawnDelay());
  }

  public IEnumerator PreloadEnemyAssets()
  {
    EnemyJellyBeholder enemyJellyBeholder1 = this;
    foreach (EnemyJellyBeholder.RoundsOfEnemies enemyRound in enemyJellyBeholder1.enemyRounds)
    {
      foreach (EnemyRounds.EnemyAndPosition enemyAndPosition1 in enemyRound.Round)
      {
        EnemyJellyBeholder enemyJellyBeholder = enemyJellyBeholder1;
        EnemyRounds.EnemyAndPosition enemyAndPosition = enemyAndPosition1;
        bool isLoaded = false;
        if (enemyJellyBeholder1.loadedAddressableAssets.ContainsKey(enemyAndPosition.EnemyTarget))
        {
          GameObject result = enemyJellyBeholder1.loadedAddressableAssets[enemyAndPosition.EnemyTarget].Result;
          result.CreatePool(result.CountPooled() + 1, true);
          isLoaded = true;
        }
        else
          Addressables.LoadAssetAsync<GameObject>((object) enemyAndPosition.EnemyTarget).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
          {
            enemyJellyBeholder.loadedAddressableAssets.Add(enemyAndPosition.EnemyTarget, obj);
            obj.Result.CreatePool(obj.Result.CountPooled() + 1, true);
            isLoaded = true;
          });
        yield return (object) new WaitUntil((Func<bool>) (() => isLoaded));
        isLoaded = false;
      }
    }
  }

  public IEnumerator SpawnDelay()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.ChargingUpSpawn;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyJellyBeholder.ClearPaths();
    enemyJellyBeholder.summonParticles.Play();
    enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.summonAnticipationAnimation, true);
    enemyJellyBeholder.effectsSpine.AnimationState.SetAnimation(0, "anticipate-summon", true);
    AudioManager.Instance.PlayOneShot(enemyJellyBeholder.SummonSfx, enemyJellyBeholder.gameObject);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) enemyJellyBeholder.spawningChargeUpTime)
      yield return (object) null;
    enemyJellyBeholder.health.invincible = true;
    enemyJellyBeholder.health.ClearBurn();
    enemyJellyBeholder.health.ClearElectrified();
    enemyJellyBeholder.health.ClearIce();
    enemyJellyBeholder.health.ClearPoison();
    enemyJellyBeholder.shadow.gameObject.SetActive(false);
    GameManager.GetInstance().RemoveFromCamera(enemyJellyBeholder.gameObject);
    if ((UnityEngine.Object) Interaction_Chest.Instance != (UnityEngine.Object) null)
      GameManager.GetInstance().AddToCamera(Interaction_Chest.Instance.gameObject);
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Spawning;
    enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.summonAnimation, false);
    enemyJellyBeholder.effectsSpine.AnimationState.SetAnimation(0, "summon", false);
    enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.flyOutAnimation, false, 0.0f);
    enemyJellyBeholder.effectsSpine.AnimationState.AddAnimation(0, "fly-out", false, 0.0f);
    AudioManager.Instance.PlayOneShot(enemyJellyBeholder.SummonedSfx, enemyJellyBeholder.gameObject);
    if (enemyJellyBeholder.enemyRounds != null && enemyJellyBeholder.enemyRounds.Count > 0)
    {
      enemyJellyBeholder.deathCount = 0;
      foreach (EnemyRounds.EnemyAndPosition e in enemyJellyBeholder.enemyRounds[enemyJellyBeholder.currentRoundIndex].Round)
      {
        GameObject prefab = (GameObject) null;
        AsyncOperationHandle<GameObject> asyncOperationHandle;
        if (enemyJellyBeholder.loadedAddressableAssets.TryGetValue(e.EnemyTarget, out asyncOperationHandle))
          prefab = asyncOperationHandle.Result;
        else
          Debug.LogError((object) (e.EnemyTarget?.ToString() + " asset is not loaded! Check PreloadEnemyAssets() method."));
        GameObject Spawn = ObjectPool.Spawn(prefab, enemyJellyBeholder.transform.parent, e.Position, Quaternion.identity);
        Health component = Spawn.GetComponent<Health>();
        component.gameObject.SetActive(false);
        component.OnDie += new Health.DieAction(enemyJellyBeholder.OnSpawnedDie);
        if ((UnityEngine.Object) component.GetComponent<ShowHPBar>() == (UnityEngine.Object) null)
          component.gameObject.AddComponent<ShowHPBar>().zOffset = 2f;
        enemyJellyBeholder.spawnedEnemies.Add(component);
        EnemySpawner.CreateWithAndInitInstantiatedEnemy(e.Position, enemyJellyBeholder.transform.parent, Spawn);
        time = 0.0f;
        while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) e.Delay)
          yield return (object) null;
      }
      enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Hiding;
      enemyJellyBeholder.health.untouchable = true;
      enemyJellyBeholder.isEnemiesSpawned = true;
    }
  }

  public IEnumerator EnemiesDefeated()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    enemyJellyBeholder.deathCount = 0;
    ++enemyJellyBeholder.currentRoundIndex;
    enemyJellyBeholder.isEnemiesSpawned = false;
    enemyJellyBeholder.transform.position = Vector3.zero;
    enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.flyInAnimation, false);
    enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.idleAnimation, true, 0.0f);
    enemyJellyBeholder.shadow.gameObject.SetActive(true);
    enemyJellyBeholder.health.untouchable = false;
    enemyJellyBeholder.health.invincible = false;
    GameManager.GetInstance().AddToCamera(enemyJellyBeholder.gameObject);
    if ((UnityEngine.Object) Interaction_Chest.Instance != (UnityEngine.Object) null)
      GameManager.GetInstance().RemoveFromCamera(Interaction_Chest.Instance.gameObject);
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.canAttack = false;
    enemyJellyBeholder.spawnRoutine = (Coroutine) null;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) enemyJellyBeholder.attackCooldown)
      yield return (object) null;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyJellyBeholder.canAttack = true;
  }

  public void OnSpawnedDie(
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
    Debug.Log((object) ("Death count: " + this.deathCount.ToString()));
  }

  public void EnemySplit(UnitObject enemy)
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

  public void MeleeAttack()
  {
    if (this.beholderState == EnemyJellyBeholder.BeholderState.Attacking)
      return;
    this.attackTimer = 0.0f;
    this.beholderState = EnemyJellyBeholder.BeholderState.Attacking;
    this.canAttack = false;
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.MeleeAttackIE());
    this.attackPatternIndex = (this.attackPatternIndex + 1) % this.attackPattern.Length;
    if (!this.randomPattern)
      return;
    this.attackPatternIndex = UnityEngine.Random.Range(0, this.attackPattern.Length);
  }

  public IEnumerator MeleeAttackIE()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    float time = 0.0f;
    float flashMeleeTickTimer = 0.0f;
    for (int i = 0; i < enemyJellyBeholder.numAttacks; ++i)
    {
      enemyJellyBeholder.targetObject = enemyJellyBeholder.GetClosestTarget();
      enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.attackAnimation, false);
      enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.idleAnimation, true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/jellyfish_large/attack", enemyJellyBeholder.gameObject);
      yield return (object) new WaitForEndOfFrame();
      enemyJellyBeholder.StartCoroutine((IEnumerator) enemyJellyBeholder.TurnOnDamageColliderForDuration(enemyJellyBeholder.damageDuration));
      enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
      enemyJellyBeholder.DisableForces = true;
      Vector2 force = new Vector2(2500f * Mathf.Cos(enemyJellyBeholder.state.LookAngle * ((float) Math.PI / 180f)), 2500f * Mathf.Sin(enemyJellyBeholder.state.LookAngle * ((float) Math.PI / 180f))) * enemyJellyBeholder.attackForceModifier;
      enemyJellyBeholder.rb.AddForce(force);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) enemyJellyBeholder.attackDuration)
        yield return (object) null;
      enemyJellyBeholder.DisableForces = false;
      if (i < enemyJellyBeholder.numAttacks - 1)
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", enemyJellyBeholder.gameObject);
        enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.anticipationAnimation, true);
        float t = 0.0f;
        while ((double) t < (double) enemyJellyBeholder.timeBetweenMultiAttacks)
        {
          t += Time.deltaTime * enemyJellyBeholder.Spine.timeScale;
          flashMeleeTickTimer += Time.deltaTime * enemyJellyBeholder.Spine.timeScale;
          enemyJellyBeholder.simpleSpineFlash.FlashWhite((float) ((double) t / (double) enemyJellyBeholder.timeBetweenMultiAttacks * 0.75));
          enemyJellyBeholder.aiming.gameObject.SetActive(true);
          if ((double) flashMeleeTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
          {
            enemyJellyBeholder.aiming.color = enemyJellyBeholder.aiming.color == Color.red ? Color.white : Color.red;
            flashMeleeTickTimer = 0.0f;
          }
          if ((double) t / (double) enemyJellyBeholder.timeBetweenMultiAttacks < 0.5 && (UnityEngine.Object) enemyJellyBeholder.targetObject != (UnityEngine.Object) null)
            enemyJellyBeholder.LookAtAngle(Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position));
          enemyJellyBeholder.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyJellyBeholder.state.LookAngle);
          yield return (object) null;
        }
        enemyJellyBeholder.aiming.gameObject.SetActive(false);
      }
    }
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) enemyJellyBeholder.attackCooldown)
      yield return (object) null;
    enemyJellyBeholder.canAttack = true;
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    enemyJellyBeholder.damageColliderEvents.SetActive(true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) duration)
      yield return (object) null;
    enemyJellyBeholder.damageColliderEvents.SetActive(false);
  }

  public void RingShotAttack()
  {
    if (this.beholderState == EnemyJellyBeholder.BeholderState.Shooting)
      return;
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.RingShotAttackIE());
    this.attackPatternIndex = (this.attackPatternIndex + 1) % this.attackPattern.Length;
    if (!this.randomPattern)
      return;
    this.attackPatternIndex = UnityEngine.Random.Range(0, this.attackPattern.Length);
  }

  public IEnumerator RingShotAttackIE()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Shooting;
    enemyJellyBeholder.canAttack = false;
    if (!string.IsNullOrEmpty(enemyJellyBeholder.RingShotAttackBeginSFX))
      AudioManager.Instance.PlayOneShot(enemyJellyBeholder.RingShotAttackBeginSFX, enemyJellyBeholder.transform.position);
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
          gameObject.GetComponent<PoisonBomb>().Play(enemyJellyBeholder.transform.position, 1f, enemyJellyBeholder.health.team);
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
          component.Play(enemyJellyBeholder.transform.position + new Vector3(0.0f, 0.0f, -1.5f), 1f, Health.Team.Team2, parentSpine: enemyJellyBeholder.spine);
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
        component.SetParentSpine(enemyJellyBeholder.spine);
        AudioManager.Instance.PlayOneShot(enemyJellyBeholder.ShootSFX, enemyJellyBeholder.transform.position);
      }
      aimingAngle += enemyJellyBeholder.ringShotAngleArc / (float) Mathf.Max(enemyJellyBeholder.numRingShots, 0);
      float Progress = 0.0f;
      while ((double) Progress < (double) enemyJellyBeholder.ringShotDelay)
      {
        Progress += Time.deltaTime * enemyJellyBeholder.Spine.timeScale;
        yield return (object) null;
      }
    }
    enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) enemyJellyBeholder.attackCooldown)
      yield return (object) null;
    enemyJellyBeholder.canAttack = true;
  }

  public void TargetedShotAttack()
  {
    if (this.beholderState == EnemyJellyBeholder.BeholderState.Shooting)
      return;
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.TargetedShotAttackIE());
    this.attackPatternIndex = (this.attackPatternIndex + 1) % this.attackPattern.Length;
    if (!this.randomPattern)
      return;
    this.attackPatternIndex = UnityEngine.Random.Range(0, this.attackPattern.Length);
  }

  public IEnumerator TargetedShotAttackIE()
  {
    EnemyJellyBeholder enemyJellyBeholder = this;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Shooting;
    enemyJellyBeholder.canAttack = false;
    int i = enemyJellyBeholder.numTargetedShots;
    float flashTargetShotTickTimer = 0.0f;
    float aimingAngle = enemyJellyBeholder.state.LookAngle;
    if (!enemyJellyBeholder.targetedArrowIsBomb)
      enemyJellyBeholder.aiming.gameObject.SetActive(enemyJellyBeholder.targetedShowAiming);
    if ((UnityEngine.Object) enemyJellyBeholder.targetObject != (UnityEngine.Object) null)
    {
      aimingAngle = Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position);
      enemyJellyBeholder.LookAtAngle(Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position));
    }
    enemyJellyBeholder.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyJellyBeholder.state.LookAngle);
    if (!string.IsNullOrEmpty(enemyJellyBeholder.TargetedShotAttackBeginSFX))
      AudioManager.Instance.PlayOneShot(enemyJellyBeholder.TargetedShotAttackBeginSFX, enemyJellyBeholder.gameObject);
    while (--i >= 0)
    {
      enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
      CameraManager.shakeCamera(0.2f, aimingAngle);
      if (enemyJellyBeholder.targetedArrowIsBomb)
      {
        Vector3 vector3 = enemyJellyBeholder.targetObject.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        GameObject gameObject = ObjectPool.Spawn(enemyJellyBeholder.targetedArrowPrefab, enemyJellyBeholder.transform.parent, vector3, Quaternion.identity);
        if (!string.IsNullOrEmpty(enemyJellyBeholder.TargetedShotAttackIndividualSFX))
          AudioManager.Instance.PlayOneShot(enemyJellyBeholder.TargetedShotAttackIndividualSFX, enemyJellyBeholder.transform.position);
        if (enemyJellyBeholder.targetedArrowIsPoison)
        {
          gameObject.GetComponent<PoisonBomb>().Play(enemyJellyBeholder.transform.position, 1f, enemyJellyBeholder.health.team, enemyJellyBeholder.spine);
        }
        else
        {
          MortarBomb component = gameObject.GetComponent<MortarBomb>();
          if ((double) Vector2.Distance((Vector2) enemyJellyBeholder.transform.position, (Vector2) vector3) < 1.0)
            component.transform.position = enemyJellyBeholder.transform.position + (vector3 - enemyJellyBeholder.transform.position).normalized * 1f;
          else
            component.transform.position = vector3;
          component.Play(enemyJellyBeholder.transform.position + new Vector3(0.0f, 0.0f, -1.5f), 1f, Health.Team.Team2, parentSpine: enemyJellyBeholder.spine);
        }
      }
      else if (enemyJellyBeholder.targetedArrowIsGrenade)
      {
        Vector3 vector3 = enemyJellyBeholder.targetObject.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
        GameObject gameObject = ObjectPool.Spawn(enemyJellyBeholder.targetedArrowPrefab, enemyJellyBeholder.transform.parent, enemyJellyBeholder.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", enemyJellyBeholder.transform.position);
        GrenadeBullet component = gameObject.GetComponent<GrenadeBullet>();
        component.SetOwner(enemyJellyBeholder.gameObject);
        component.Play(-1f, (float) UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(2f, 4f), UnityEngine.Random.Range(enemyJellyBeholder.arrowSpeed - 2f, enemyJellyBeholder.arrowSpeed + 2f), parentSpine: enemyJellyBeholder.spine);
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
        component.SetParentSpine(enemyJellyBeholder.spine);
        AudioManager.Instance.PlayOneShot(enemyJellyBeholder.ShootSFX, enemyJellyBeholder.transform.position);
      }
      enemyJellyBeholder.Spine.AnimationState.SetAnimation(0, enemyJellyBeholder.attackAnimation, false);
      enemyJellyBeholder.Spine.AnimationState.AddAnimation(0, enemyJellyBeholder.idleAnimation, true, 0.0f);
      float Progress = 0.0f;
      if (i == 0)
        enemyJellyBeholder.aiming.gameObject.SetActive(false);
      while ((double) (Progress += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) enemyJellyBeholder.targetedShotDelay)
      {
        if ((UnityEngine.Object) enemyJellyBeholder.targetObject != (UnityEngine.Object) null)
        {
          aimingAngle = Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position);
          enemyJellyBeholder.LookAtAngle(Utils.GetAngle(enemyJellyBeholder.transform.position, enemyJellyBeholder.targetObject.transform.position));
        }
        if ((double) flashTargetShotTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
        {
          enemyJellyBeholder.aiming.color = enemyJellyBeholder.aiming.color == Color.red ? Color.white : Color.red;
          flashTargetShotTickTimer = 0.0f;
        }
        enemyJellyBeholder.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyJellyBeholder.state.LookAngle);
        flashTargetShotTickTimer += Time.deltaTime;
        yield return (object) null;
      }
    }
    enemyJellyBeholder.simpleSpineFlash.FlashWhite(false);
    enemyJellyBeholder.beholderState = EnemyJellyBeholder.BeholderState.Idle;
    enemyJellyBeholder.state.CURRENT_STATE = StateMachine.State.Idle;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) enemyJellyBeholder.attackCooldown)
      yield return (object) null;
    enemyJellyBeholder.canAttack = true;
  }

  public void PatternShotAttack()
  {
    if (this.beholderState == EnemyJellyBeholder.BeholderState.Shooting)
      return;
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.PatternShotAttackIE());
    this.attackPatternIndex = (this.attackPatternIndex + 1) % this.attackPattern.Length;
    if (!this.randomPattern)
      return;
    this.attackPatternIndex = UnityEngine.Random.Range(0, this.attackPattern.Length);
  }

  public IEnumerator PatternShotAttackIE()
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
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyJellyBeholder.Spine.timeScale) < (double) enemyJellyBeholder.attackCooldown)
      yield return (object) null;
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
    AudioManager.Instance.PlayOneShot("event:/jellyfish_large/death", this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/enemy_death_large", this.transform.position);
  }

  public override void UpdateMoving()
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

  public void Flee()
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

  public IEnumerator DelayedDestroy()
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

  public void OnDrawGizmos()
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
    public float ComeBackDelay = 30f;
  }

  public enum BeholderState
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
