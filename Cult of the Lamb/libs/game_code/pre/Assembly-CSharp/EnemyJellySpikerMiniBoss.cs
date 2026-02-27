// Decompiled with JetBrains decompiler
// Type: EnemyJellySpikerMiniBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyJellySpikerMiniBoss : UnitObject
{
  [Space]
  [SerializeField]
  private ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  [SerializeField]
  private SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string closeAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string distanceAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string spikeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string chargeAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string chargeAttackEndAnimation;
  [SerializeField]
  private float chargeAnticipation;
  [SerializeField]
  private float chargeSpeed;
  [SerializeField]
  private float chargeMinDistance;
  [SerializeField]
  private float chargeCooldown;
  [SerializeField]
  private float spikeAnticipation;
  [SerializeField]
  private float spikeColliderDuration;
  [SerializeField]
  private float spikeCooldown;
  [SerializeField]
  private float TurningArc = 90f;
  [SerializeField]
  private Vector2 DistanceRange = new Vector2(1f, 3f);
  [SerializeField]
  private List<Vector3> patrolRoute = new List<Vector3>();
  [SerializeField]
  private AssetReferenceGameObject spawnable;
  [SerializeField]
  private GameObject trailPrefab;
  [SerializeField]
  private int spikeAmount = 30;
  [SerializeField]
  private float delayBetweenSpikes = 0.05f;
  [SerializeField]
  private float distanceBetweenSpikes = 0.5f;
  [SerializeField]
  private int spikeRingAmount = 10;
  [SerializeField]
  private float delayBetweenSpikesRing = 0.1f;
  [SerializeField]
  private float distanceBetweenSpikesRing = 0.5f;
  [Space]
  [SerializeField]
  private float knockback;
  [SerializeField]
  private SpriteRenderer aiming;
  private bool cooldown;
  private bool attacking;
  private bool anticipating;
  private bool chargeAttacking;
  private bool shortAttacking;
  private bool aboutToChargeAttacking;
  private float anticipationTimer;
  private float anticipationDuration;
  private float randomDirection;
  private float repathTimestamp;
  private int patrolIndex;
  private float repathTimeInterval = 2f;
  private Vector3 startPosition;
  private EventInstance LoopedSound;
  private int phase;
  private List<GameObject> spawnedSpikes = new List<GameObject>();

  private void Start()
  {
    this.aiming.gameObject.SetActive(false);
    this.startPosition = this.transform.position;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
    this.SpawnEnemies();
  }

  private void SpawnEnemies()
  {
    if (this.spawnable == null)
      return;
    List<Vector3> vector3List = new List<Vector3>(2)
    {
      new Vector3(-2.5f, 0.0f, 0.0f),
      new Vector3(2.5f, 0.0f, 0.0f)
    };
    for (int index = 0; index < 2; ++index)
      Addressables.InstantiateAsync((object) this.spawnable, vector3List[index], Quaternion.identity, this.transform.parent).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        obj.Result.gameObject.SetActive(false);
        EnemySpawner.CreateWithAndInitInstantiatedEnemy(obj.Result.transform.position, this.transform.parent, obj.Result);
      });
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
    float num = (bool) (UnityEngine.Object) PlayerFarming.Instance ? Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position) : float.MaxValue;
    if (!this.attacking && !this.anticipating && !this.cooldown)
    {
      if (!this.chargeAttacking && (double) num > (double) this.chargeMinDistance && (double) num < (double) this.VisionRange)
      {
        if (UnityEngine.Random.Range(0, 2) == 0)
          this.StartCoroutine((IEnumerator) this.BeginChargeAttackIE());
        else
          this.StartCoroutine((IEnumerator) this.SpawnSpikesInLineIE(this.spikeAmount, this.delayBetweenSpikes, this.distanceBetweenSpikes));
      }
      else if ((double) num < (double) this.chargeMinDistance)
      {
        if (UnityEngine.Random.Range(0, 2) == 0)
          this.StartCoroutine((IEnumerator) this.SpikeAttackIE());
        else
          this.StartCoroutine((IEnumerator) this.SpawnSpikesInCircleIE(this.spikeRingAmount, this.delayBetweenSpikesRing, this.distanceBetweenSpikesRing));
      }
    }
    this.aiming.gameObject.SetActive(this.anticipating && (this.aboutToChargeAttacking || this.shortAttacking));
    if (this.anticipating && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      this.anticipationTimer += Time.deltaTime;
      this.simpleSpineFlash.FlashWhite(this.anticipationTimer / this.anticipationDuration * 0.75f);
      this.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position));
      if (Time.frameCount % 5 == 0)
        this.aiming.color = this.aiming.color == Color.red ? Color.white : Color.red;
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

  private void UpdateMovement()
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

  private void GetRandomTargetPosition()
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

  private IEnumerator BeginChargeAttackIE()
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

  private void ChargeAtTarget()
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

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!this.chargeAttacking || (this.layerToCheck.value & 1 << collision.gameObject.layer) <= 0)
      return;
    this.AttackEnd();
  }

  private void AttackEnd()
  {
    CameraManager.instance.ShakeCameraForDuration(0.5f, 1.5f, 0.5f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/gethit", this.gameObject);
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

  private IEnumerator SpikeAttackIE()
  {
    EnemyJellySpikerMiniBoss jellySpikerMiniBoss = this;
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.distanceAnticipationAnimation, false);
    jellySpikerMiniBoss.shortAttacking = true;
    jellySpikerMiniBoss.anticipating = true;
    jellySpikerMiniBoss.anticipationTimer = 0.0f;
    jellySpikerMiniBoss.anticipationDuration = jellySpikerMiniBoss.spikeAnticipation;
    yield return (object) new WaitForSeconds(jellySpikerMiniBoss.anticipationDuration);
    jellySpikerMiniBoss.DoKnockBack(Utils.GetAngle(jellySpikerMiniBoss.transform.position, PlayerFarming.Instance.transform.position) * ((float) Math.PI / 180f), 1.5f, 1.5f);
    jellySpikerMiniBoss.anticipating = false;
    jellySpikerMiniBoss.attacking = true;
    yield return (object) new WaitForEndOfFrame();
    jellySpikerMiniBoss.simpleSpineFlash.FlashWhite(false);
    jellySpikerMiniBoss.SpikeAttack();
  }

  private void SpikeAttack()
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

  private IEnumerator CoolingDown(float duration)
  {
    yield return (object) new WaitForSeconds(duration);
    this.cooldown = false;
  }

  private void LookAtTarget()
  {
    this.LookAtAngle(Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position));
  }

  private void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  private void OnTriggerEnterEvent(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  private IEnumerator TurnOnDamageColliderForDuration(float duration)
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

  private GameObject GetSpawnSpike()
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

  protected virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, component.gameObject, component.transform.position);
  }

  private IEnumerator SpawnSpikesInLineIE(
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
    Vector3 normalized = (PlayerFarming.Instance.transform.position - jellySpikerMiniBoss.transform.position).normalized;
    yield return (object) jellySpikerMiniBoss.StartCoroutine((IEnumerator) jellySpikerMiniBoss.ShootSpikesInDirectionIE(amount, normalized, delayBetweenSpikes, distanceBetweenSpikes));
    yield return (object) new WaitForSeconds(1f);
    jellySpikerMiniBoss.attacking = false;
  }

  private IEnumerator SpawnSpikesInCircleIE(
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
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.1f, 0.1f, false);
    yield return (object) new WaitForEndOfFrame();
    jellySpikerMiniBoss.simpleSpineFlash.FlashWhite(false);
    jellySpikerMiniBoss.Spine.AnimationState.SetAnimation(0, jellySpikerMiniBoss.spikeAnimation, false);
    jellySpikerMiniBoss.Spine.AnimationState.AddAnimation(0, jellySpikerMiniBoss.idleAnimation, true, 0.0f);
    int num = UnityEngine.Random.Range(0, 360);
    for (int index = 0; index < amount; ++index)
    {
      Vector3 direction = new Vector3(Mathf.Cos((float) num * ((float) Math.PI / 180f)), Mathf.Sin((float) num * ((float) Math.PI / 180f)), 0.0f);
      jellySpikerMiniBoss.StartCoroutine((IEnumerator) jellySpikerMiniBoss.ShootSpikesInDirectionIE(jellySpikerMiniBoss.spikeRingAmount, direction, jellySpikerMiniBoss.delayBetweenSpikesRing, jellySpikerMiniBoss.distanceBetweenSpikesRing));
      num = (int) Mathf.Repeat((float) (num + 360 / amount), 360f);
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
      jellySpikerMiniBoss.GetSpawnSpike().transform.position = position;
      position += direction * distanceBetweenSpikes;
      yield return (object) new WaitForSeconds(delayBetweenSpikes);
    }
  }
}
