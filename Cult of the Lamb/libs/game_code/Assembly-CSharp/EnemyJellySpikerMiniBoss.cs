// Decompiled with JetBrains decompiler
// Type: EnemyJellySpikerMiniBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
public class EnemyJellySpikerMiniBoss : UnitObject
{
  [Space]
  [SerializeField]
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string closeAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string distanceAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spikeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string chargeAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string chargeAttackEndAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string slamAnimation;
  [SerializeField]
  public float chargeAnticipation;
  [SerializeField]
  public float chargeSpeed;
  [SerializeField]
  public float chargeMinDistance;
  [SerializeField]
  public float chargeCooldown;
  [SerializeField]
  public ProjectilePattern hitWallProjectilePattern;
  [SerializeField]
  public float spikeAnticipation;
  [SerializeField]
  public float spikeColliderDuration;
  [SerializeField]
  public float spikeCooldown;
  [SerializeField]
  public float TurningArc = 90f;
  [SerializeField]
  public Vector2 DistanceRange = new Vector2(1f, 3f);
  [SerializeField]
  public List<Vector3> patrolRoute = new List<Vector3>();
  [SerializeField]
  public AssetReferenceGameObject spawnable;
  [SerializeField]
  public GameObject trailPrefab;
  [SerializeField]
  public int spikeAmount = 30;
  [SerializeField]
  public float delayBetweenSpikes = 0.05f;
  [SerializeField]
  public float distanceBetweenSpikes = 0.5f;
  [SerializeField]
  public float spikeScale = 1f;
  [SerializeField]
  public bool tripleBeams;
  [Space]
  [SerializeField]
  public bool spawnLongSpikes;
  [SerializeField]
  public int spikeRingAmount = 10;
  [SerializeField]
  public float delayBetweenSpikesRing = 0.1f;
  [SerializeField]
  public float distanceBetweenSpikesRing = 0.5f;
  [SerializeField]
  public int spikeRingLongAmount = 10;
  [Space]
  [SerializeField]
  public float knockback;
  [SerializeField]
  public SpriteRenderer aiming;
  public bool cooldown;
  public bool attacking;
  public bool anticipating;
  public bool chargeAttacking;
  public bool shortAttacking;
  public bool aboutToChargeAttacking;
  public float anticipationTimer;
  public float anticipationDuration;
  public float randomDirection;
  public float repathTimestamp;
  public int patrolIndex;
  public float repathTimeInterval = 2f;
  public float flashTickTimer;
  public Health currentTarget;
  public Vector3 startPosition;
  public EventInstance LoopedSound;
  public int phase;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public List<GameObject> spawnedSpikes = new List<GameObject>();

  public IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJellySpikerMiniBoss jellySpikerMiniBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      jellySpikerMiniBoss.Preload();
      jellySpikerMiniBoss.SpawnEnemies();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    jellySpikerMiniBoss.aiming.gameObject.SetActive(false);
    jellySpikerMiniBoss.startPosition = jellySpikerMiniBoss.transform.position;
    jellySpikerMiniBoss.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(jellySpikerMiniBoss.OnTriggerEnterEvent);
    jellySpikerMiniBoss.currentTarget = jellySpikerMiniBoss.ReconsiderPlayerTarget();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Preload()
  {
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.spawnable);
    asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedAddressableAssets.Add(obj);
      obj.Result.CreatePool(6, true);
    });
    asyncOperationHandle.WaitForCompletion();
    this.InitializeTrailSpikes();
  }

  public void SpawnEnemies()
  {
    if (this.spawnable == null)
      return;
    List<Vector3> vector3List = new List<Vector3>(2)
    {
      new Vector3(-2.5f, 0.0f, 0.0f),
      new Vector3(2.5f, 0.0f, 0.0f)
    };
    for (int index = 0; index < 2; ++index)
    {
      GameObject Spawn = ObjectPool.Spawn(this.loadedAddressableAssets[0].Result, this.transform.parent, vector3List[index], Quaternion.identity);
      Spawn.gameObject.SetActive(false);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(Spawn.transform.position, this.transform.parent, Spawn);
    }
  }

  public void InitializeTrailSpikes()
  {
    for (int index = 0; index < 180; ++index)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.trailPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      gameObject.SetActive(false);
      SkeletonAnimation componentInChildren1 = gameObject.GetComponentInChildren<SkeletonAnimation>(true);
      if (SettingsManager.Settings.Game.PerformanceMode)
        gameObject.GetComponent<SimpleSpineDeactivateAfterPlay>().UseLowQualitySpikes(SimpleSpineDeactivateAfterPlay.SpikeType.EnemyJellySpikerMiniBoss);
      else if ((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null)
        SkeletonAnimationLODGlobalManager.Instance.DisableCulling(gameObject.transform, componentInChildren1);
      this.spawnedSpikes.Add(gameObject);
      ColliderEvents componentInChildren2 = gameObject.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren2)
        componentInChildren2.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.attacking = false;
    this.anticipating = false;
    this.cooldown = false;
    this.chargeAttacking = false;
  }

  public override void Update()
  {
    base.Update();
    float num1 = (bool) (UnityEngine.Object) this.currentTarget ? Vector3.Distance(this.currentTarget.transform.position, this.transform.position) : float.MaxValue;
    if (!this.attacking && !this.anticipating && !this.cooldown)
    {
      if (!this.chargeAttacking && (double) num1 > (double) this.chargeMinDistance && (double) num1 < (double) this.VisionRange)
      {
        int num2 = UnityEngine.Random.Range(0, this.spawnLongSpikes ? 3 : 2);
        this.currentTarget = this.ReconsiderPlayerTarget();
        switch (num2)
        {
          case 0:
            this.StartCoroutine((IEnumerator) this.BeginChargeAttackIE());
            break;
          case 1:
            this.StartCoroutine((IEnumerator) this.SpawnSpikesInLineIE(this.spikeAmount, this.delayBetweenSpikes, this.distanceBetweenSpikes));
            break;
          default:
            this.StartCoroutine((IEnumerator) this.SpawnSpikesInCircleLongIE(8, this.delayBetweenSpikes));
            break;
        }
      }
      else if ((double) num1 < (double) this.chargeMinDistance)
      {
        int num3 = UnityEngine.Random.Range(0, this.spawnLongSpikes ? 3 : 2);
        this.currentTarget = this.ReconsiderPlayerTarget();
        switch (num3)
        {
          case 0:
            this.StartCoroutine((IEnumerator) this.SpikeAttackIE());
            break;
          case 1:
            this.StartCoroutine((IEnumerator) this.SpawnSpikesInCircleIE(this.spikeRingAmount, this.delayBetweenSpikesRing, this.distanceBetweenSpikesRing));
            break;
          default:
            this.StartCoroutine((IEnumerator) this.SpawnSpikesInCircleLongIE(8, this.delayBetweenSpikes));
            break;
        }
      }
    }
    this.aiming.gameObject.SetActive(this.anticipating && (this.aboutToChargeAttacking || this.shortAttacking));
    if (this.anticipating && (UnityEngine.Object) this.currentTarget != (UnityEngine.Object) null)
    {
      this.anticipationTimer += Time.deltaTime;
      this.flashTickTimer += Time.deltaTime;
      this.simpleSpineFlash.FlashWhite(this.anticipationTimer / this.anticipationDuration * 0.75f);
      this.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, Utils.GetAngle(this.transform.position, this.currentTarget.transform.position));
      if ((double) this.flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
      {
        this.aiming.color = this.aiming.color == Color.red ? Color.white : Color.red;
        this.flashTickTimer = 0.0f;
      }
    }
    if (this.chargeAttacking)
    {
      this.speed = this.chargeSpeed;
      this.maxSpeed = this.chargeSpeed;
      this.move();
    }
    else
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float repathTimestamp = this.repathTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) repathTimestamp & currentTime.HasValue)
        this.UpdateMovement();
      else
        this.maxSpeed = 0.015f;
    }
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    if (this.phase == 0 && (double) this.health.HP < (double) this.health.totalHP * 0.6600000262260437)
    {
      this.phase = 1;
      this.SpawnEnemies();
    }
    else
    {
      if (this.phase != 1 || (double) this.health.HP >= (double) this.health.totalHP * 0.33000001311302185)
        return;
      this.phase = 2;
      this.SpawnEnemies();
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
    if ((double) this.knockback == 0.0 || this.chargeAttacking)
      return;
    this.DoKnockBack(Attacker, this.knockback, 0.5f);
  }

  public void UpdateMovement()
  {
    if (this.patrolRoute.Count == 0)
      this.GetRandomTargetPosition();
    else if (this.pathToFollow == null)
    {
      this.patrolIndex = ++this.patrolIndex % this.patrolRoute.Count;
      this.givePath(this.startPosition + this.patrolRoute[this.patrolIndex]);
      this.LookAtAngle(Utils.GetAngle(this.transform.position, this.startPosition + this.patrolRoute[this.patrolIndex]));
    }
    this.repathTimestamp = GameManager.GetInstance().CurrentTime + this.repathTimeInterval;
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

  public IEnumerator BeginChargeAttackIE()
  {
    EnemyJellySpikerMiniBoss jellySpikerMiniBoss = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", jellySpikerMiniBoss.gameObject);
    jellySpikerMiniBoss.LoopedSound = AudioManager.Instance.CreateLoop("event:/enemy/jellyfish_miniboss/jellyfish_miniboss_charge", jellySpikerMiniBoss.gameObject, true);
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.distanceAnticipationAnimation, false);
    jellySpikerMiniBoss.aboutToChargeAttacking = true;
    jellySpikerMiniBoss.anticipating = true;
    jellySpikerMiniBoss.anticipationTimer = 0.0f;
    jellySpikerMiniBoss.anticipationDuration = jellySpikerMiniBoss.chargeAnticipation;
    yield return (object) new WaitForSeconds(jellySpikerMiniBoss.chargeAnticipation);
    AudioManager.Instance.StopLoop(jellySpikerMiniBoss.LoopedSound);
    jellySpikerMiniBoss.anticipating = false;
    jellySpikerMiniBoss.aboutToChargeAttacking = false;
    jellySpikerMiniBoss.ChargeAtTarget();
    yield return (object) new WaitForEndOfFrame();
    jellySpikerMiniBoss.simpleSpineFlash.FlashWhite(false);
  }

  public void ChargeAtTarget()
  {
    this.attacking = true;
    this.chargeAttacking = true;
    this.ClearPaths();
    this.damageColliderEvents.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/attack", this.gameObject);
    this.Spine.AnimationState.SetAnimation(0, this.chargeAttackAnimation, true);
    this.LookAtTarget();
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.chargeAttacking || (this.layerToCheck.value & 1 << collision.gameObject.layer) <= 0)
      return;
    this.AttackEnd();
  }

  public void AttackEnd()
  {
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1.5f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/gethit", this.gameObject);
    if ((UnityEngine.Object) this.hitWallProjectilePattern != (UnityEngine.Object) null)
      this.hitWallProjectilePattern.Shoot();
    if (!this.chargeAttacking)
      return;
    this.chargeAttacking = false;
    this.attacking = false;
    this.ClearPaths();
    this.Spine.AnimationState.SetAnimation(0, this.chargeAttackEndAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.damageColliderEvents.gameObject.SetActive(false);
    this.speed = 0.0f;
    this.cooldown = true;
    this.StartCoroutine((IEnumerator) this.CoolingDown(this.chargeCooldown));
  }

  public IEnumerator SpikeAttackIE()
  {
    EnemyJellySpikerMiniBoss jellySpikerMiniBoss = this;
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.distanceAnticipationAnimation, false);
    jellySpikerMiniBoss.shortAttacking = true;
    jellySpikerMiniBoss.anticipating = true;
    jellySpikerMiniBoss.anticipationTimer = 0.0f;
    jellySpikerMiniBoss.anticipationDuration = jellySpikerMiniBoss.spikeAnticipation;
    yield return (object) new WaitForSeconds(jellySpikerMiniBoss.anticipationDuration);
    jellySpikerMiniBoss.DoKnockBack(Utils.GetAngle(jellySpikerMiniBoss.transform.position, jellySpikerMiniBoss.currentTarget.transform.position) * ((float) Math.PI / 180f), 1.5f, 1.5f);
    jellySpikerMiniBoss.anticipating = false;
    jellySpikerMiniBoss.attacking = true;
    yield return (object) new WaitForEndOfFrame();
    jellySpikerMiniBoss.simpleSpineFlash.FlashWhite(false);
    jellySpikerMiniBoss.SpikeAttack();
  }

  public void SpikeAttack()
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/attack", this.gameObject);
    this.Spine.AnimationState.SetAnimation(0, this.spikeAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.TurnOnDamageColliderForDuration(this.spikeColliderDuration));
    this.cooldown = true;
    this.attacking = false;
    this.shortAttacking = false;
    this.StartCoroutine((IEnumerator) this.CoolingDown(this.spikeCooldown));
  }

  public IEnumerator CoolingDown(float duration)
  {
    yield return (object) new WaitForSeconds(duration);
    this.cooldown = false;
  }

  public void LookAtTarget()
  {
    this.LookAtAngle(Utils.GetAngle(this.transform.position, this.currentTarget.transform.position));
  }

  public void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  public void OnTriggerEnterEvent(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive(true);
    yield return (object) new WaitForSeconds(duration);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!(bool) (UnityEngine.Object) this.transform.parent.GetComponent<MiniBossController>())
      return;
    for (int index = 0; index < Health.team2.Count; ++index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health)
      {
        Health.team2[index].invincible = false;
        Health.team2[index].enabled = true;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position);
      }
    }
  }

  public GameObject GetSpawnSpike()
  {
    GameObject spawnSpike = (GameObject) null;
    if (this.spawnedSpikes.Count > 0)
    {
      foreach (GameObject spawnedSpike in this.spawnedSpikes)
      {
        if (!spawnedSpike.activeSelf)
        {
          spawnSpike = spawnedSpike;
          spawnSpike.transform.position = this.transform.position;
          spawnSpike.SetActive(true);
          break;
        }
      }
    }
    if ((UnityEngine.Object) spawnSpike == (UnityEngine.Object) null)
    {
      spawnSpike = UnityEngine.Object.Instantiate<GameObject>(this.trailPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      this.spawnedSpikes.Add(spawnSpike);
      ColliderEvents componentInChildren = spawnSpike.GetComponentInChildren<ColliderEvents>();
      if ((bool) (UnityEngine.Object) componentInChildren)
        componentInChildren.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    return spawnSpike;
  }

  public virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, component.transform.position);
  }

  public IEnumerator SpawnSpikesInLineIE(
    int amount,
    float delayBetweenSpikes,
    float distanceBetweenSpikes)
  {
    EnemyJellySpikerMiniBoss jellySpikerMiniBoss = this;
    jellySpikerMiniBoss.attacking = true;
    jellySpikerMiniBoss.anticipating = true;
    jellySpikerMiniBoss.anticipationTimer = 0.0f;
    jellySpikerMiniBoss.anticipationDuration = jellySpikerMiniBoss.spikeAnticipation;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", jellySpikerMiniBoss.gameObject);
    jellySpikerMiniBoss.LoopedSound = AudioManager.Instance.CreateLoop("event:/enemy/jellyfish_miniboss/jellyfish_miniboss_charge", jellySpikerMiniBoss.gameObject, true);
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.closeAnticipationAnimation, false);
    yield return (object) new WaitForSeconds(jellySpikerMiniBoss.chargeAnticipation);
    AudioManager.Instance.StopLoop(jellySpikerMiniBoss.LoopedSound);
    jellySpikerMiniBoss.anticipating = false;
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.1f, 0.1f, false);
    yield return (object) new WaitForEndOfFrame();
    jellySpikerMiniBoss.simpleSpineFlash.FlashWhite(false);
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.spikeAnimation, false);
    jellySpikerMiniBoss.Spine.AnimationState.AddAnimation(0, jellySpikerMiniBoss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", jellySpikerMiniBoss.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/attack", jellySpikerMiniBoss.gameObject);
    Vector3 normalized = (jellySpikerMiniBoss.currentTarget.transform.position - jellySpikerMiniBoss.transform.position).normalized;
    if (jellySpikerMiniBoss.tripleBeams)
    {
      jellySpikerMiniBoss.StartCoroutine((IEnumerator) jellySpikerMiniBoss.ShootSpikesInDirectionIE(amount, Quaternion.Euler(0.0f, 0.0f, -15f) * normalized, delayBetweenSpikes, distanceBetweenSpikes));
      jellySpikerMiniBoss.StartCoroutine((IEnumerator) jellySpikerMiniBoss.ShootSpikesInDirectionIE(amount, Quaternion.Euler(0.0f, 0.0f, 15f) * normalized, delayBetweenSpikes, distanceBetweenSpikes));
    }
    yield return (object) jellySpikerMiniBoss.StartCoroutine((IEnumerator) jellySpikerMiniBoss.ShootSpikesInDirectionIE(amount, normalized, delayBetweenSpikes, distanceBetweenSpikes));
    yield return (object) new WaitForSeconds(1f);
    jellySpikerMiniBoss.attacking = false;
  }

  public IEnumerator SpawnSpikesInCircleIE(
    int amount,
    float delayBetweenSpikes,
    float distanceBetweenSpikes)
  {
    EnemyJellySpikerMiniBoss jellySpikerMiniBoss = this;
    jellySpikerMiniBoss.attacking = true;
    jellySpikerMiniBoss.anticipating = true;
    jellySpikerMiniBoss.anticipationTimer = 0.0f;
    jellySpikerMiniBoss.anticipationDuration = jellySpikerMiniBoss.spikeAnticipation;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", jellySpikerMiniBoss.gameObject);
    jellySpikerMiniBoss.LoopedSound = AudioManager.Instance.CreateLoop("event:/enemy/jellyfish_miniboss/jellyfish_miniboss_charge", jellySpikerMiniBoss.gameObject, true);
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.closeAnticipationAnimation, false);
    yield return (object) new WaitForSeconds(jellySpikerMiniBoss.anticipationDuration);
    AudioManager.Instance.StopLoop(jellySpikerMiniBoss.LoopedSound);
    jellySpikerMiniBoss.anticipating = false;
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1.5f, 0.5f);
    yield return (object) new WaitForEndOfFrame();
    jellySpikerMiniBoss.simpleSpineFlash.FlashWhite(false);
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.spikeAnimation, false);
    jellySpikerMiniBoss.Spine.AnimationState.AddAnimation(0, jellySpikerMiniBoss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", jellySpikerMiniBoss.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/attack", jellySpikerMiniBoss.gameObject);
    int num = UnityEngine.Random.Range(0, 360);
    for (int index = 0; index < amount; ++index)
    {
      Vector3 direction = new Vector3(Mathf.Cos((float) num * ((float) Math.PI / 180f)), Mathf.Sin((float) num * ((float) Math.PI / 180f)), 0.0f);
      jellySpikerMiniBoss.StartCoroutine((IEnumerator) jellySpikerMiniBoss.ShootSpikesInDirectionIE(jellySpikerMiniBoss.spikeRingAmount, direction, jellySpikerMiniBoss.delayBetweenSpikesRing, jellySpikerMiniBoss.distanceBetweenSpikesRing));
      num = (int) Utils.Repeat((float) (num + 360 / amount), 360f);
    }
    yield return (object) new WaitForSeconds((float) ((double) jellySpikerMiniBoss.spikeAnticipation + (double) delayBetweenSpikes + 1.0));
    jellySpikerMiniBoss.attacking = false;
  }

  public IEnumerator SpawnSpikesInCircleLongIE(int amount, float delayBetweenSpikes)
  {
    EnemyJellySpikerMiniBoss jellySpikerMiniBoss = this;
    jellySpikerMiniBoss.attacking = true;
    jellySpikerMiniBoss.anticipating = true;
    jellySpikerMiniBoss.anticipationTimer = 0.0f;
    jellySpikerMiniBoss.anticipationDuration = jellySpikerMiniBoss.spikeAnticipation;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", jellySpikerMiniBoss.gameObject);
    jellySpikerMiniBoss.LoopedSound = AudioManager.Instance.CreateLoop("event:/enemy/jellyfish_miniboss/jellyfish_miniboss_charge", jellySpikerMiniBoss.gameObject, true);
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.slamAnimation, false);
    bool waiting = true;
    jellySpikerMiniBoss.Spine.AnimationState.Event += (Spine.AnimationState.TrackEntryEventDelegate) ((trackEntry, e) =>
    {
      if (!(e.Data.Name == "slam-impact"))
        return;
      waiting = false;
    });
    while (waiting || (double) jellySpikerMiniBoss.Spine.timeScale == 9.9999997473787516E-05)
      yield return (object) null;
    AudioManager.Instance.StopLoop(jellySpikerMiniBoss.LoopedSound);
    jellySpikerMiniBoss.anticipating = false;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.1f, false);
    yield return (object) new WaitForEndOfFrame();
    jellySpikerMiniBoss.simpleSpineFlash.FlashWhite(false);
    jellySpikerMiniBoss.Spine.AnimationState.AddAnimation(0, jellySpikerMiniBoss.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/spike_trap/spike_trap_trigger", jellySpikerMiniBoss.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/attack", jellySpikerMiniBoss.gameObject);
    int num = UnityEngine.Random.Range(0, 360);
    for (int index = 0; index < amount; ++index)
    {
      Vector3 direction = new Vector3(Mathf.Cos((float) num * ((float) Math.PI / 180f)), Mathf.Sin((float) num * ((float) Math.PI / 180f)), 0.0f);
      jellySpikerMiniBoss.StartCoroutine((IEnumerator) jellySpikerMiniBoss.ShootSpikesInDirectionIE(jellySpikerMiniBoss.spikeRingLongAmount, direction, delayBetweenSpikes, jellySpikerMiniBoss.distanceBetweenSpikes));
      num = (int) Utils.Repeat((float) (num + 360 / amount), 360f);
    }
    yield return (object) new WaitForSeconds((float) ((double) jellySpikerMiniBoss.spikeAnticipation + (double) delayBetweenSpikes + 1.0));
    jellySpikerMiniBoss.attacking = false;
  }

  public IEnumerator ShootSpikesInDirectionIE(
    int amount,
    Vector3 direction,
    float delayBetweenSpikes,
    float distanceBetweenSpikes)
  {
    EnemyJellySpikerMiniBoss jellySpikerMiniBoss = this;
    Vector3 position = jellySpikerMiniBoss.transform.position;
    for (int i = 0; i < amount; ++i)
    {
      GameObject spawnSpike = jellySpikerMiniBoss.GetSpawnSpike();
      spawnSpike.transform.localScale = Vector3.one * jellySpikerMiniBoss.spikeScale;
      spawnSpike.transform.position = position;
      position += direction * distanceBetweenSpikes;
      while (PlayerRelic.TimeFrozen)
        yield return (object) null;
      yield return (object) new WaitForSeconds(delayBetweenSpikes);
    }
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
  public void \u003CPreload\u003Eb__54_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(6, true);
  }
}
