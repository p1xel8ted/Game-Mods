// Decompiled with JetBrains decompiler
// Type: EnemyJuicedBurrowingMiniBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyJuicedBurrowingMiniBoss : UnitObject
{
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAttackAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spawnEnemiesAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootSpikesAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string appearAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string disapearAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string hiddenAnimation;
  [Space]
  [SerializeField]
  public Vector2 timeBetweenAttacks;
  [SerializeField]
  public JuicedTrail trailPrefab;
  [SerializeField]
  public float spikesAnticipation;
  [SerializeField]
  public float spikesPost;
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
  public float spawnForce;
  [SerializeField]
  public Vector2 timeBetweenSpawns;
  [SerializeField]
  public AssetReferenceGameObject[] spawnables;
  [Header("Weights")]
  [SerializeField]
  public float targetedSpikesWeight;
  [SerializeField]
  public float targetedShootWeight;
  [SerializeField]
  public float patternSpikesWeight;
  [SerializeField]
  public float spawnEnemiesWeight;
  [SerializeField]
  public float circleSpikesWeight;
  [Space]
  [SerializeField]
  public GameObject lighting;
  public List<JuicedTrail> spawnedSpikes = new List<JuicedTrail>();
  public Coroutine currentAttack;
  public EventInstance loopingSoundInstance;
  public ShowHPBar showHPBar;
  public float idleTime;
  public float spawnTime;
  public float initialDelay;
  public Vector3 targetPosition;
  public Coroutine currentRoutine;
  public bool phase2;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public GameObject t;
  public Vector3 previousSpawnPosition;
  public float TrailsTimer;
  public float DelayBetweenTrails = 0.1f;
  public List<GameObject> Trails = new List<GameObject>();
  [Range(0.0f, 1f)]
  public float ChanceToPathTowardsPlayer = 0.8f;
  public float TurningArc = 90f;
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  public bool PathingToPlayer;
  public float RandomDirection;
  public float DistanceToPathTowardsPlayer = 6f;
  public float MinimumPlayerDistance = 3f;
  public float MoveSpeed = 5f;
  public float MoveDelay;
  public Vector2 MaxMoveDistance;

  public IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      burrowingMiniBoss.Preload();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    burrowingMiniBoss.initialDelay = UnityEngine.Random.Range(0.0f, 2f);
    burrowingMiniBoss.showHPBar = burrowingMiniBoss.GetComponent<ShowHPBar>();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
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
  }

  public override void Update()
  {
    base.Update();
    if (this.phase2 && this.currentRoutine == null)
      this.transform.position = Vector3.zero;
    this.idleTime -= Time.deltaTime * this.Spine.timeScale;
    this.spawnTime -= Time.deltaTime * this.Spine.timeScale;
    if (this.currentAttack != null || this.currentRoutine != null || !((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null) || (double) this.idleTime > 0.0 || !this.phase2)
      return;
    if ((double) UnityEngine.Random.value <= (double) this.targetedSpikesWeight && !this.phase2)
      this.ShootTripleSoftTargeted();
    else if ((double) UnityEngine.Random.value <= (double) this.targetedShootWeight)
      this.TargetedShootProjectiles();
    else if (Health.team2.Count - 1 < this.maxEnemies && (double) this.spawnTime <= 0.0 && (Health.team2.Count - 1 <= 2 || (double) UnityEngine.Random.value <= (double) this.spawnEnemiesWeight))
    {
      this.SpawnEnemies();
    }
    else
    {
      if ((double) UnityEngine.Random.value > (double) this.patternSpikesWeight || this.phase2)
        return;
      if ((double) UnityEngine.Random.value > 0.5)
        this.ShootCrossAngled();
      else
        this.ShootCross();
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.currentAttack = (Coroutine) null;
    if (this.currentRoutine != null)
      this.StopCoroutine(this.currentRoutine);
    this.currentRoutine = (Coroutine) null;
    if (this.phase2)
      return;
    this.currentRoutine = this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public void ShootCross()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.ShootCrossIE(4, 0.0f, false));
  }

  public void ShootCrossAngled()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.ShootCrossIE(4, 45f, false));
  }

  public void ShootCrossAngledContinous()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.ShootCrossIE(4, 45f, true));
  }

  public void ShootTripleSoftTargeted()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.ShootTripleSoftTargetedIE());
  }

  public IEnumerator ShootTripleSoftTargetedIE()
  {
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    Vector3 dir = (Vector3) Utils.DegreeToVector2(Mathf.Round(Utils.GetAngle(burrowingMiniBoss.transform.position, burrowingMiniBoss.GetClosestTarget().transform.position) / 90f) * 90f);
    burrowingMiniBoss.Spine.AnimationState.SetAnimation(0, burrowingMiniBoss.shootSpikesAnimation, false);
    burrowingMiniBoss.Spine.AnimationState.AddAnimation(0, burrowingMiniBoss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) burrowingMiniBoss.spikesAnticipation)
      yield return (object) null;
    burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.ShootSpikesInDirectionIE(dir, 15, 0.75f, false));
    Vector3 direction1 = Quaternion.Euler(0.0f, 0.0f, -25f) * dir;
    Vector3 direction2 = Quaternion.Euler(0.0f, 0.0f, 25f) * dir;
    burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.ShootSpikesInDirectionIE(direction1, 15, 0.75f, false));
    burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.ShootSpikesInDirectionIE(direction2, 15, 0.75f, false));
    time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < 2.0)
      yield return (object) null;
    burrowingMiniBoss.currentAttack = (Coroutine) null;
    burrowingMiniBoss.idleTime = UnityEngine.Random.Range(burrowingMiniBoss.timeBetweenAttacks.x, burrowingMiniBoss.timeBetweenAttacks.y);
  }

  public IEnumerator ShootCrossIE(int directions, float directionOffset, bool continous)
  {
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    burrowingMiniBoss.Spine.AnimationState.SetAnimation(0, burrowingMiniBoss.shootSpikesAnimation, false);
    burrowingMiniBoss.Spine.AnimationState.AddAnimation(0, burrowingMiniBoss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) burrowingMiniBoss.spikesAnticipation)
      yield return (object) null;
    List<KeyValuePair<GameObject, float>> spikes = new List<KeyValuePair<GameObject, float>>();
    int count = 0;
    float num1 = 0.0f;
    float num2 = 360f / (float) directions;
    for (int index = 0; index < directions; ++index)
    {
      Vector3 vector2 = (Vector3) Utils.DegreeToVector2(num1 + directionOffset);
      burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.ShootSpikesInDirectionIE(vector2, 15, 0.5f, continous, (System.Action<List<KeyValuePair<GameObject, float>>>) (s =>
      {
        spikes.AddRange((IEnumerable<KeyValuePair<GameObject, float>>) s);
        ++count;
      })));
      num1 += num2;
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) burrowingMiniBoss.spikesPost)
      yield return (object) null;
    burrowingMiniBoss.currentAttack = (Coroutine) null;
    burrowingMiniBoss.idleTime = UnityEngine.Random.Range(burrowingMiniBoss.timeBetweenAttacks.x, burrowingMiniBoss.timeBetweenAttacks.y);
  }

  public IEnumerator ShootSpikesInDirectionIE(
    Vector3 direction,
    int distance,
    float spacing,
    bool continous,
    System.Action<List<KeyValuePair<GameObject, float>>> callback = null)
  {
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    List<KeyValuePair<GameObject, float>> spikes = new List<KeyValuePair<GameObject, float>>();
    Vector3 position = burrowingMiniBoss.transform.position;
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", burrowingMiniBoss.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_screech", burrowingMiniBoss.transform.position);
    for (int i = 0; i < distance; ++i)
    {
      GameObject spawnSpike = burrowingMiniBoss.GetSpawnSpike(continous);
      spawnSpike.transform.position = position;
      position += direction * spacing;
      spikes.Add(new KeyValuePair<GameObject, float>(spawnSpike, Utils.GetAngle(burrowingMiniBoss.transform.position, spawnSpike.transform.position)));
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < 0.10000000149011612)
        yield return (object) null;
    }
    System.Action<List<KeyValuePair<GameObject, float>>> action = callback;
    if (action != null)
      action(spikes);
  }

  public void TargetedShootProjectiles()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.TargetedShootProjectilesIE());
  }

  public IEnumerator TargetedShootProjectilesIE()
  {
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    float time = 0.0f;
    int randomAmount = (int) UnityEngine.Random.Range(burrowingMiniBoss.amountOfRounds.x, burrowingMiniBoss.amountOfRounds.y + 1f);
    for (int i = 0; i < randomAmount; ++i)
    {
      burrowingMiniBoss.Spine.AnimationState.SetAnimation(0, burrowingMiniBoss.shootAttackAnticipationAnimation, true);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) burrowingMiniBoss.targetedShootingAnticipation)
        yield return (object) null;
      burrowingMiniBoss.Spine.AnimationState.SetAnimation(0, burrowingMiniBoss.shootAttackAnimation, false);
      burrowingMiniBoss.Spine.AnimationState.AddAnimation(0, burrowingMiniBoss.idleAnimation, true, 0.0f);
      yield return (object) burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.ShootProjectilesIE(true, (System.Action) null));
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) burrowingMiniBoss.targetedShootingPost)
      yield return (object) null;
    burrowingMiniBoss.Spine.AnimationState.SetAnimation(0, burrowingMiniBoss.idleAnimation, true);
    burrowingMiniBoss.currentAttack = (Coroutine) null;
    burrowingMiniBoss.idleTime = UnityEngine.Random.Range(burrowingMiniBoss.timeBetweenAttacks.x, burrowingMiniBoss.timeBetweenAttacks.y);
  }

  public void ShootProjectiles()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.ShootProjectilesIE(false, (System.Action) null));
  }

  public IEnumerator ShootProjectilesIE(bool shootAtTarget, System.Action callback)
  {
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    int shotsToFire = (int) UnityEngine.Random.Range(burrowingMiniBoss.amountOfShots.x, burrowingMiniBoss.amountOfShots.y + 1f);
    int i = -1;
    while (++i < shotsToFire)
    {
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", burrowingMiniBoss.transform.position);
      Vector3 position = burrowingMiniBoss.shootPosition.transform.position with
      {
        z = 0.0f
      };
      float Speed = UnityEngine.Random.Range(burrowingMiniBoss.shootDistance.x, burrowingMiniBoss.shootDistance.y);
      float Angle = UnityEngine.Random.Range(0.0f, 360f);
      if (shootAtTarget && (UnityEngine.Object) burrowingMiniBoss.GetClosestTarget() != (UnityEngine.Object) null)
      {
        Speed = Vector3.Distance(burrowingMiniBoss.transform.position, burrowingMiniBoss.GetClosestTarget().transform.position) / 1.5f + UnityEngine.Random.Range(-2f, 2f);
        Angle = Utils.GetAngle(burrowingMiniBoss.transform.position, burrowingMiniBoss.GetClosestTarget().transform.position) + UnityEngine.Random.Range(-20f, 20f);
      }
      GrenadeBullet component = ObjectPool.Spawn(burrowingMiniBoss.grenadeBullet, position).GetComponent<GrenadeBullet>();
      component.SetOwner(burrowingMiniBoss.gameObject);
      component.Play(-3f, Angle, Speed, burrowingMiniBoss.gravSpeed);
      float dur = UnityEngine.Random.Range(burrowingMiniBoss.delayBetweenShots.x, burrowingMiniBoss.delayBetweenShots.y);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SpawnEnemies()
  {
    this.currentAttack = this.StartCoroutine((IEnumerator) this.SpawnEnemiesIE());
  }

  public IEnumerator SpawnEnemiesIE()
  {
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    burrowingMiniBoss.Spine.AnimationState.SetAnimation(0, burrowingMiniBoss.spawnEnemiesAnimation, false);
    burrowingMiniBoss.Spine.AnimationState.AddAnimation(0, burrowingMiniBoss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) burrowingMiniBoss.spawnEnemiesAnticipation)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_screech", burrowingMiniBoss.transform.position);
    int amount = (int) UnityEngine.Random.Range(burrowingMiniBoss.spawnAmount.x, burrowingMiniBoss.spawnAmount.y + 1f);
    for (int i = 0; i < amount; ++i)
    {
      Vector3 position = burrowingMiniBoss.transform.position;
      UnitObject component = ObjectPool.Spawn(burrowingMiniBoss.loadedAddressableAssets[UnityEngine.Random.Range(0, burrowingMiniBoss.loadedAddressableAssets.Count)].Result, burrowingMiniBoss.transform.parent, position, Quaternion.identity).GetComponent<UnitObject>();
      component.CanHaveModifier = false;
      component.RemoveModifier();
      component.DoKnockBack(UnityEngine.Random.Range(0.0f, 360f), burrowingMiniBoss.spawnForce, 0.75f);
      float dur = UnityEngine.Random.Range(burrowingMiniBoss.delayBetweenSpawns.x, burrowingMiniBoss.delayBetweenSpawns.y);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) dur)
        yield return (object) null;
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) burrowingMiniBoss.spawnEnemiesPost)
      yield return (object) null;
    burrowingMiniBoss.currentAttack = (Coroutine) null;
    burrowingMiniBoss.spawnTime = UnityEngine.Random.Range(burrowingMiniBoss.timeBetweenSpawns.x, burrowingMiniBoss.timeBetweenSpawns.y);
    burrowingMiniBoss.idleTime = UnityEngine.Random.Range(burrowingMiniBoss.timeBetweenAttacks.x, burrowingMiniBoss.timeBetweenAttacks.y);
  }

  public GameObject GetSpawnSpike(bool continous)
  {
    JuicedTrail juicedTrail = (JuicedTrail) null;
    if (this.spawnedSpikes.Count > 0)
    {
      foreach (JuicedTrail spawnedSpike in this.spawnedSpikes)
      {
        if (!spawnedSpike.gameObject.activeSelf)
        {
          juicedTrail = spawnedSpike;
          juicedTrail.transform.position = this.transform.position;
          juicedTrail.gameObject.SetActive(true);
          break;
        }
      }
    }
    if ((UnityEngine.Object) juicedTrail == (UnityEngine.Object) null)
    {
      juicedTrail = UnityEngine.Object.Instantiate<JuicedTrail>(this.trailPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      juicedTrail.transform.localScale = Vector3.one * 0.85f;
      this.spawnedSpikes.Add(juicedTrail);
      juicedTrail.ColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    juicedTrail.SetContinious(continous);
    return juicedTrail.gameObject;
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

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, component.transform.position);
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
    for (int index = this.spawnedSpikes.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedSpikes[index] != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.spawnedSpikes[index].gameObject);
    }
  }

  public IEnumerator MoveRoutine()
  {
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    burrowingMiniBoss.Spine.AnimationState.AddAnimation(0, burrowingMiniBoss.idleAnimation, true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) burrowingMiniBoss.initialDelay)
      yield return (object) null;
    burrowingMiniBoss.initialDelay = 0.0f;
    yield return (object) new WaitForEndOfFrame();
    burrowingMiniBoss.Spine.AnimationState.SetAnimation(0, burrowingMiniBoss.disapearAnimation, false);
    burrowingMiniBoss.Spine.AnimationState.AddAnimation(0, burrowingMiniBoss.hiddenAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_disappear_underground", burrowingMiniBoss.gameObject);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < 0.25)
      yield return (object) null;
    burrowingMiniBoss.health.invincible = true;
    burrowingMiniBoss.previousSpawnPosition = Vector3.positiveInfinity;
    float Progress = 0.0f;
    float Duration = 0.366666675f;
    while ((double) (Progress += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) Duration)
    {
      burrowingMiniBoss.SpawnTrails();
      yield return (object) null;
    }
    if ((bool) (UnityEngine.Object) burrowingMiniBoss.lighting)
      burrowingMiniBoss.lighting.SetActive(false);
    burrowingMiniBoss.showHPBar?.Hide();
    burrowingMiniBoss.state.CURRENT_STATE = StateMachine.State.Fleeing;
    if (!burrowingMiniBoss.loopingSoundInstance.isValid())
      burrowingMiniBoss.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/enemy/tunnel_worm/tunnel_worm_underground_loop", burrowingMiniBoss.gameObject, true);
    Vector3 StartPosition = burrowingMiniBoss.transform.position with
    {
      z = 0.0f
    };
    burrowingMiniBoss.targetPosition.z = 0.0f;
    Progress = 0.0f;
    Duration = Vector3.Distance(StartPosition, burrowingMiniBoss.targetPosition) / burrowingMiniBoss.MoveSpeed;
    burrowingMiniBoss.previousSpawnPosition = Vector3.positiveInfinity;
    while ((double) (Progress += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < (double) Duration)
    {
      burrowingMiniBoss.transform.position = Vector3.Lerp(StartPosition, burrowingMiniBoss.targetPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      burrowingMiniBoss.SpawnTrails();
      yield return (object) null;
    }
    burrowingMiniBoss.transform.position = burrowingMiniBoss.targetPosition;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < 0.5)
      yield return (object) null;
    burrowingMiniBoss.health.invincible = false;
    AudioManager.Instance.StopLoop(burrowingMiniBoss.loopingSoundInstance);
    AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground", burrowingMiniBoss.gameObject);
    burrowingMiniBoss.Spine.AnimationState.SetAnimation(0, burrowingMiniBoss.appearAnimation, false);
    burrowingMiniBoss.Spine.AnimationState.AddAnimation(0, burrowingMiniBoss.idleAnimation, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * burrowingMiniBoss.Spine.timeScale) < 0.550000011920929)
      yield return (object) null;
    if ((bool) (UnityEngine.Object) burrowingMiniBoss.lighting)
      burrowingMiniBoss.lighting.SetActive(true);
    burrowingMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    burrowingMiniBoss.currentAttack = (Coroutine) null;
    while (burrowingMiniBoss.currentAttack == null)
    {
      if (burrowingMiniBoss.phase2)
        burrowingMiniBoss.ShootCrossAngledContinous();
      else if ((double) UnityEngine.Random.value <= (double) burrowingMiniBoss.targetedSpikesWeight)
        burrowingMiniBoss.ShootTripleSoftTargeted();
      else if ((double) UnityEngine.Random.value <= (double) burrowingMiniBoss.targetedShootWeight)
        burrowingMiniBoss.TargetedShootProjectiles();
      else if (Health.team2.Count - 1 < burrowingMiniBoss.maxEnemies && (double) burrowingMiniBoss.spawnTime <= 0.0 && (Health.team2.Count - 1 <= 2 || (double) UnityEngine.Random.value <= (double) burrowingMiniBoss.spawnEnemiesWeight))
        burrowingMiniBoss.SpawnEnemies();
      else if ((double) UnityEngine.Random.value <= (double) burrowingMiniBoss.circleSpikesWeight)
        burrowingMiniBoss.ShootCrossAngled();
      else if ((double) UnityEngine.Random.value <= (double) burrowingMiniBoss.patternSpikesWeight)
      {
        if ((double) UnityEngine.Random.value > 0.5)
          burrowingMiniBoss.ShootCrossAngled();
        else
          burrowingMiniBoss.ShootCross();
      }
    }
    if (burrowingMiniBoss.currentAttack != null)
      yield return (object) burrowingMiniBoss.currentAttack;
    if (!burrowingMiniBoss.phase2)
    {
      if ((double) burrowingMiniBoss.health.HP < (double) burrowingMiniBoss.health.totalHP / 2.0)
      {
        burrowingMiniBoss.phase2 = true;
        if (burrowingMiniBoss.currentRoutine != null)
          burrowingMiniBoss.StopCoroutine(burrowingMiniBoss.currentRoutine);
        burrowingMiniBoss.targetPosition = Vector3.zero;
        burrowingMiniBoss.currentRoutine = burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.MoveRoutine());
      }
      else if (burrowingMiniBoss.GetNewTargetPosition())
      {
        if (burrowingMiniBoss.currentRoutine != null)
          burrowingMiniBoss.StopCoroutine(burrowingMiniBoss.currentRoutine);
        burrowingMiniBoss.currentRoutine = burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.MoveRoutine());
      }
      else
      {
        if (burrowingMiniBoss.currentRoutine != null)
          burrowingMiniBoss.StopCoroutine(burrowingMiniBoss.currentRoutine);
        burrowingMiniBoss.currentRoutine = burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.ActiveRoutine());
      }
    }
    else
      burrowingMiniBoss.currentRoutine = (Coroutine) null;
  }

  public void SpawnTrails()
  {
    if ((double) (this.TrailsTimer += Time.deltaTime) <= (double) this.DelayBetweenTrails || (double) Vector3.Distance(this.transform.position, this.previousSpawnPosition) <= 0.10000000149011612)
      return;
    this.TrailsTimer = 0.0f;
    this.t = this.GetSpawnSpike(false);
    this.t.transform.position = this.transform.position;
    this.previousSpawnPosition = this.t.transform.position;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    foreach (GameObject trail in this.Trails)
    {
      if ((UnityEngine.Object) trail != (UnityEngine.Object) null)
      {
        ColliderEvents componentInChildren = trail.GetComponentInChildren<ColliderEvents>();
        if ((bool) (UnityEngine.Object) componentInChildren)
          componentInChildren.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      }
    }
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  public bool GetNewTargetPosition()
  {
    float num1 = 100f;
    if ((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null && (double) this.ChanceToPathTowardsPlayer > 0.0 && (double) UnityEngine.Random.value < (double) this.ChanceToPathTowardsPlayer && (double) Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position) < (double) this.DistanceToPathTowardsPlayer)
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, this.GetClosestTarget().transform.position) * ((float) Math.PI / 180f);
    }
    if ((double) this.ChanceToPathTowardsPlayer >= 1.0 && GameManager.RoomActive && (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
    {
      float num2 = Vector3.Distance(this.transform.position, this.GetClosestTarget().transform.position);
      if ((double) num2 > (double) this.MinimumPlayerDistance && (double) num2 < (double) this.DistanceToPathTowardsPlayer)
      {
        Vector3 normalized = (this.GetClosestTarget().transform.position - this.transform.position).normalized;
        float num3 = this.MaxMoveDistance.y;
        RaycastHit2D raycastHit2D;
        if ((UnityEngine.Object) (raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) normalized, 100f, (int) this.layerToCheck)).collider != (UnityEngine.Object) null)
          num3 = Mathf.Min(num3, Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.point));
        this.targetPosition = this.transform.position + normalized * UnityEngine.Random.Range(this.MaxMoveDistance.x, num3);
        return true;
      }
      this.PathingToPlayer = false;
    }
    while ((double) --num1 > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      if (!this.PathingToPlayer)
        this.RandomDirection += UnityEngine.Random.Range(-this.TurningArc, this.TurningArc) * ((float) Math.PI / 180f);
      this.PathingToPlayer = false;
      float radius = 0.2f;
      Vector3 vector3 = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      {
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
        this.targetPosition = vector3;
        return true;
      }
    }
    return false;
  }

  public IEnumerator ActiveRoutine()
  {
    EnemyJuicedBurrowingMiniBoss burrowingMiniBoss = this;
    burrowingMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    while (burrowingMiniBoss.state.CURRENT_STATE != StateMachine.State.Idle || (double) burrowingMiniBoss.idleTime > 0.0 || !burrowingMiniBoss.GetNewTargetPosition())
      yield return (object) null;
    if (burrowingMiniBoss.currentRoutine != null)
      burrowingMiniBoss.StopCoroutine(burrowingMiniBoss.currentRoutine);
    burrowingMiniBoss.currentRoutine = burrowingMiniBoss.StartCoroutine((IEnumerator) burrowingMiniBoss.MoveRoutine());
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__49_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(24, true);
  }
}
