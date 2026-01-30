// Decompiled with JetBrains decompiler
// Type: TrapFleshBall
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TrapFleshBall : UnitObject
{
  [SerializeField]
  public Collider2D col;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public MeshRenderer ballMeshRenderer;
  [SerializeField]
  public SkeletonAnimation spineChain;
  [SerializeField]
  public ColliderEvents trapTriggerColliderEvent;
  [SerializeField]
  public ColliderEvents damageColliderEvents;
  [SerializeField]
  public GameObject indicator;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string bounceAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string breakAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string fallAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string landAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string rollAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  [SerializeField]
  public string rollBackAnimation;
  [SerializeField]
  public Vector2 dropTimeRange;
  [SerializeField]
  public AnimationCurve dropCurve;
  [SerializeField]
  public float dropDuration = 1f;
  [SerializeField]
  public float landDuration = 1.267f;
  [SerializeField]
  public ParticleSystem dropParticles;
  [SerializeField]
  public TrapFleshBall.MovementType movementType;
  [SerializeField]
  public float acceleration = 5f;
  [SerializeField]
  public Vector2 avoidAngles;
  [SerializeField]
  public float bounceDistanceCheck = 1f;
  [SerializeField]
  public float freezeOnBounceDuration = 0.3f;
  public float direction;
  [SerializeField]
  public Throwable throwable;
  [SerializeField]
  public int hitsToBreak = 5;
  [SerializeField]
  public int spawnAmount;
  [SerializeField]
  public int spawnRadius;
  [SerializeField]
  public float spawnDelay = 1f;
  [SerializeField]
  public bool hasToDrop = true;
  [SerializeField]
  public ParticleSystem spawnParticles;
  [SerializeField]
  public float damage = 1f;
  [SerializeField]
  public float damageToEnemies = 5f;
  [SerializeField]
  public float damageToSelfOnEnemyCollision = 1f;
  [EventRef]
  public string FallSFX = "event:/dlc/dungeon06/trap/flesh_ball/fall";
  [EventRef]
  public string OnBounceSFX = "event:/dlc/dungeon06/trap/flesh_ball/bounce";
  [EventRef]
  public string RollingLoopSFX = "event:/dlc/dungeon06/trap/flesh_ball/roll_loop";
  [EventRef]
  public string GetHitSFX = "event:/dlc/dungeon06/trap/flesh_ball/gethit";
  [EventRef]
  public string SplitSFX = "event:/dlc/dungeon06/trap/flesh_ball/split";
  public EventInstance rollingLoopInstance;
  public float dropTime;
  public Vector2 shakeDelay = new Vector2(2f, 3f);
  public bool move;
  public int hits;
  public float dropTimer;
  public float shakeTimer;
  public float currentShakeDelay;
  public float shakeDuration = 1f;
  public LayerMask hitLayers;
  public int obstaclesLayer;
  public int islandLayer;
  public int weaponLayer;
  public Vector3 ogPosition;
  public Vector3 ogScale;
  public bool dropped;
  public TrapFleshBall.Orientation previousOrientation;
  public TrapFleshBall.Orientation currentOrientation;

  public void Start()
  {
    if ((bool) (UnityEngine.Object) this.indicator)
      this.indicator.SetActive(false);
    FleshEgg component;
    if (this.throwable.gameObject.TryGetComponent<FleshEgg>(out component))
      FleshEgg.Prewarm((IEnumerable<GameObject>) component.Hatchlings, 3);
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.OnRoomCleared);
    this.dropTime = UnityEngine.Random.Range(this.dropTimeRange.x, this.dropTimeRange.y);
    this.obstaclesLayer = LayerMask.NameToLayer("Obstacles");
    this.islandLayer = LayerMask.NameToLayer("Island");
    this.weaponLayer = LayerMask.NameToLayer("Weapon");
    this.hitLayers = (LayerMask) ((int) this.hitLayers | 1 << this.obstaclesLayer);
    this.hitLayers = (LayerMask) ((int) this.hitLayers | 1 << this.islandLayer);
    this.hitLayers = (LayerMask) ((int) this.hitLayers | 1 << this.weaponLayer);
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.ogPosition = this.spine.transform.localPosition;
    this.ogScale = this.spine.transform.localScale;
    this.damageColliderEvents = this.GetComponent<ColliderEvents>();
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEntered);
    this.damageColliderEvents.enabled = false;
    this.previousOrientation = TrapFleshBall.Orientation.right;
    if (this.hasToDrop)
      this.col.enabled = false;
    this.ResetShakeDelay();
    this.ballMeshRenderer.enabled = false;
    this.trapTriggerColliderEvent.SetActive(true);
  }

  public new void OnDestroy()
  {
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnTriggerEntered);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.OnRoomCleared);
  }

  public void OnRoomCleared() => this.Explode(false);

  public override void OnEnable()
  {
    base.OnEnable();
    this.trapTriggerColliderEvent.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTrapTriggerEntered);
  }

  public override void OnDisable()
  {
    this.trapTriggerColliderEvent.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnTrapTriggerEntered);
    if (this.dropped)
    {
      this.col.enabled = true;
      this.damageColliderEvents.enabled = true;
      this.spine.transform.localPosition = Vector3.zero;
      this.spine.transform.localScale = this.ogScale;
      this.spine.AnimationState.SetAnimation(0, this.idleAnimation, true);
    }
    if (!string.IsNullOrEmpty(this.RollingLoopSFX))
    {
      int num = (int) this.rollingLoopInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    base.OnDisable();
  }

  public void OnTrapTriggerEntered(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) collider.gameObject != (UnityEngine.Object) this.gameObject) || component.team == this.health.team || component.team == Health.Team.Neutral || this.dropped)
      return;
    this.StartCoroutine((IEnumerator) this.DoDropImmediate());
  }

  public new void OnDie(
    GameObject attacker,
    Vector3 attacklocation,
    Health victim,
    Health.AttackTypes attacktype,
    Health.AttackFlags attackflags)
  {
    this.Explode(true);
  }

  public new void OnHit(
    GameObject attacker,
    Vector3 attacklocation,
    Health.AttackTypes attackType,
    bool fromBehind)
  {
    if ((UnityEngine.Object) this.spine == (UnityEngine.Object) null || this.spine.AnimationState == null)
      return;
    this.Redirect((Vector2) (this.transform.position - attacker.transform.position).normalized);
    this.move = true;
    this.spine.AnimationState.SetAnimation(0, this.rollAnimation, true);
    if (string.IsNullOrEmpty(this.GetHitSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.GetHitSFX, this.gameObject);
  }

  public void ResetShakeDelay()
  {
    this.currentShakeDelay = UnityEngine.Random.Range(this.shakeDelay.x, this.shakeDelay.y);
  }

  public override void Update()
  {
    if (!this.dropped)
    {
      this.dropTimer += Time.deltaTime * this.spine.timeScale;
      this.shakeTimer += Time.deltaTime * this.spine.timeScale;
      if ((double) this.dropTimer >= (double) this.dropTime)
      {
        this.spine.transform.DOKill();
        this.StartCoroutine((IEnumerator) this.DoDropImmediate());
      }
      else if ((double) this.shakeTimer >= (double) this.currentShakeDelay)
      {
        this.ResetShakeDelay();
        this.spine.transform.DOShakeScale(this.shakeDuration, 0.025f, fadeOut: false).SetEase<Tweener>(Ease.Linear);
        this.spine.transform.DOShakePosition(this.shakeDuration, 0.025f, fadeOut: false).SetEase<Tweener>(Ease.Linear);
      }
    }
    if (!this.move)
      return;
    switch (this.movementType)
    {
      case TrapFleshBall.MovementType.Linear:
        this.speed = this.maxSpeed;
        break;
      case TrapFleshBall.MovementType.Accelerated:
        this.speed += Time.deltaTime * this.acceleration;
        this.speed = Mathf.Clamp(this.speed, 0.0f, this.maxSpeed);
        break;
    }
  }

  public new void FixedUpdate()
  {
    if ((UnityEngine.Object) this.rb == (UnityEngine.Object) null || PlayerRelic.TimeFrozen)
      return;
    this.CheckCollisions();
    this.moveVX = this.speed * Mathf.Cos(this.direction * ((float) Math.PI / 180f));
    this.moveVY = this.speed * Mathf.Sin(this.direction * ((float) Math.PI / 180f));
    this.previousOrientation = this.currentOrientation;
    this.currentOrientation = (double) this.moveVX > 0.0 ? TrapFleshBall.Orientation.left : TrapFleshBall.Orientation.right;
    if (this.previousOrientation != this.currentOrientation)
      this.spine.AnimationState.SetAnimation(0, this.currentOrientation == TrapFleshBall.Orientation.right ? this.rollAnimation : this.rollBackAnimation, true);
    if (this.spine.AnimationState != null)
      this.spine.AnimationState.TimeScale = this.move ? this.speed / this.maxSpeed : 1f;
    if (float.IsNaN(this.moveVX) || float.IsInfinity(this.moveVX))
      this.moveVX = 0.0f;
    if (float.IsNaN(this.moveVY) || float.IsInfinity(this.moveVY))
      this.moveVY = 0.0f;
    this.rb.MovePosition(this.rb.position + new Vector2(this.moveVX, this.moveVY) * Time.deltaTime);
    if (string.IsNullOrEmpty(this.RollingLoopSFX))
      return;
    if ((double) Mathf.Sqrt((float) ((double) this.moveVX * (double) this.moveVX + (double) this.moveVY * (double) this.moveVY)) > 0.10000000149011612 && (double) this.health.HP > 0.0)
    {
      if (AudioManager.Instance.IsInstanceInActiveLoops(this.rollingLoopInstance))
        return;
      this.rollingLoopInstance = AudioManager.Instance.CreateLoop(this.RollingLoopSFX, this.gameObject, true);
    }
    else
      AudioManager.Instance.StopLoop(this.rollingLoopInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
  }

  public void CheckCollisions()
  {
    this.RelocateInIsland();
    foreach (RaycastHit2D hit in Physics2D.RaycastAll((Vector2) this.transform.position, new Vector2(this.moveVX, this.moveVY).normalized, this.bounceDistanceCheck, (int) this.hitLayers))
    {
      if ((UnityEngine.Object) hit.collider != (UnityEngine.Object) this.col && this.ProcessHit(hit))
      {
        ++this.hits;
        if (this.hits != this.hitsToBreak)
          break;
        this.health.Kill();
        break;
      }
    }
  }

  public bool ProcessHit(RaycastHit2D hit)
  {
    TrapFleshBall component1 = hit.collider.gameObject.GetComponent<TrapFleshBall>();
    if (hit.collider.gameObject.layer == this.weaponLayer && !(bool) (UnityEngine.Object) component1)
      return false;
    Health component2;
    if (hit.collider.gameObject.layer == this.obstaclesLayer && (UnityEngine.Object) (component2 = hit.collider.gameObject.GetComponent<Health>()) != (UnityEngine.Object) null)
      component2.DealDamage(this.damage, this.gameObject, Vector3.Lerp(this.transform.position, component2.transform.position, 0.7f), AttackFlags: Health.AttackFlags.Trap);
    this.Bounce(hit.normal);
    return true;
  }

  public void Redirect(Vector2 normal)
  {
    this.direction = Vector2.SignedAngle((Vector2) Vector3.right, normal.normalized);
    this.spine.AnimationState.SetAnimation(1, this.bounceAnimation, false);
  }

  public void Bounce(Vector2 normal, bool freeze = true, bool diagonal = true)
  {
    float direction = this.ClampDirection(Vector2.SignedAngle((Vector2) Vector3.right, Vector2.Reflect(new Vector2(this.moveVX, this.moveVY).normalized, normal.normalized)));
    this.direction = diagonal ? this.GetDiagonalBounceDirection(normal) : this.FilterDirection(direction);
    this.spine.AnimationState.SetAnimation(1, this.bounceAnimation, false);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    if (!string.IsNullOrEmpty(this.OnBounceSFX))
      AudioManager.Instance.PlayOneShot(this.OnBounceSFX, this.gameObject);
    if (!freeze)
      return;
    this.FreezeMovement();
  }

  public float GetDiagonalBounceDirection(Vector2 normal)
  {
    Vector2 normalized = normal.normalized;
    return (double) Mathf.Abs(normalized.x) > (double) Mathf.Abs(normalized.y) ? ((double) this.moveVX > 0.0 ? ((double) this.moveVY < 0.0 ? 225f : 135f) : ((double) this.moveVY < 0.0 ? 315f : 45f)) : ((double) this.moveVY > 0.0 ? ((double) this.moveVX < 0.0 ? 225f : 315f) : ((double) this.moveVX < 0.0 ? 135f : 45f));
  }

  public IEnumerator DoDropImmediate()
  {
    TrapFleshBall trapFleshBall = this;
    trapFleshBall.spine.transform.DOKill();
    trapFleshBall.dropped = true;
    float dropTimespan = 0.0f;
    trapFleshBall.trapTriggerColliderEvent.SetActive(false);
    if ((bool) (UnityEngine.Object) trapFleshBall.indicator)
      trapFleshBall.indicator.SetActive(true);
    trapFleshBall.DetachFromChain(true);
    Vector3 initialPos = trapFleshBall.spine.transform.position;
    if ((UnityEngine.Object) trapFleshBall.dropParticles != (UnityEngine.Object) null)
    {
      trapFleshBall.dropParticles.transform.position = initialPos;
      trapFleshBall.dropParticles.Play();
    }
    if (!string.IsNullOrEmpty(trapFleshBall.FallSFX))
      AudioManager.Instance.PlayOneShot(trapFleshBall.FallSFX, trapFleshBall.gameObject);
    Vector3 finalPos = trapFleshBall.transform.position;
    while ((double) dropTimespan <= (double) trapFleshBall.dropDuration)
    {
      trapFleshBall.spine.transform.position = Vector3.Lerp(initialPos, finalPos, trapFleshBall.dropCurve.Evaluate(Mathf.Clamp(dropTimespan / trapFleshBall.dropDuration, 0.0f, 1f)));
      dropTimespan += Time.deltaTime * trapFleshBall.spine.timeScale;
      yield return (object) null;
    }
    trapFleshBall.spine.transform.position = finalPos;
    if ((bool) (UnityEngine.Object) trapFleshBall.indicator)
      trapFleshBall.indicator.SetActive(false);
    BiomeConstants.Instance.EmitHammerEffects(trapFleshBall.transform.position, 0.1f, 1.2f, 0.3f, true, 1f, false);
    Explosion.CreateExplosion(trapFleshBall.transform.position, Health.Team.Team2, trapFleshBall.health, 2f, trapFleshBall.damage, attackFlags: Health.AttackFlags.Trap, playSFX: false);
    trapFleshBall.spine.Skeleton.ScaleX = (double) Mathf.Cos((float) Math.PI / 180f * trapFleshBall.direction) < 1.0 ? 1f : -1f;
    float landTimespan = 0.0f;
    trapFleshBall.spine.AnimationState.SetAnimation(0, trapFleshBall.landAnimation, false);
    trapFleshBall.spine.AnimationState.AddAnimation(0, trapFleshBall.idleAnimation, true, 0.0f);
    trapFleshBall.damageColliderEvents.enabled = true;
    trapFleshBall.col.enabled = true;
    while ((double) landTimespan <= (double) trapFleshBall.landDuration)
    {
      landTimespan += Time.deltaTime * trapFleshBall.spine.timeScale;
      yield return (object) null;
    }
    trapFleshBall.col.enabled = true;
    trapFleshBall.damageColliderEvents.enabled = true;
  }

  public float ClampDirection(float direction) => (float) (((double) direction + 360.0) % 360.0);

  public float FilterDirection(float direction)
  {
    if ((double) direction > (double) this.avoidAngles.x && (double) direction < (double) this.avoidAngles.y)
    {
      float num1 = direction - this.avoidAngles.x;
      float num2 = this.avoidAngles.y - direction;
      if ((double) num1 < (double) num2)
        direction -= num1;
      else if ((double) num1 > (double) num2)
        direction += num2;
      else
        direction += num2;
    }
    if ((double) direction > 360.0 - (double) this.avoidAngles.y && (double) direction < 360.0 - (double) this.avoidAngles.x)
    {
      float num3 = 360f - this.avoidAngles.x - direction;
      float num4 = direction - (360f - this.avoidAngles.y);
      if ((double) num3 < (double) num4)
        direction += num3;
      else if ((double) num3 > (double) num4)
        direction -= num4;
      else
        direction -= num4;
    }
    return direction;
  }

  public void FreezeMovement()
  {
    this.rb.simulated = false;
    DOVirtual.DelayedCall(0.3f, (TweenCallback) (() => this.rb.simulated = true));
  }

  public void OnTriggerEntered(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!(bool) (UnityEngine.Object) component)
      return;
    bool flag = component.team == Health.Team.PlayerTeam;
    if (!(bool) (UnityEngine.Object) collider.GetComponent<PlayerFarming>() || !TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, collider.GetComponent<PlayerFarming>()))
    {
      component.DealDamage(flag ? this.damage : this.damageToEnemies, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
      this.health.DealDamage(this.damageToSelfOnEnemyCollision, this.gameObject, this.transform.position);
    }
    Vector2 normalized = new Vector2(this.moveVX, this.moveVY).normalized;
    this.direction = this.FilterDirection(this.ClampDirection(Vector2.SignedAngle((Vector2) Vector3.right, Vector2.Reflect(normalized, -normalized))));
    this.spine.AnimationState.SetAnimation(1, this.bounceAnimation, false);
    this.move = true;
  }

  public void Explode(bool canThrowEggs)
  {
    this.StartCoroutine((IEnumerator) this.ExplodeCoroutine(canThrowEggs));
  }

  public void ThrowEgg()
  {
    Vector3 targetPosition = this.GetTargetPosition();
    if (targetPosition == Vector3.zero)
      return;
    Throwable throwable = ObjectPool.Spawn<Throwable>(this.throwable, this.transform.parent, this.transform.position, Quaternion.identity);
    throwable.Launch(targetPosition, 1f);
    Interaction_Chest.Instance?.AddEnemy(throwable.GetComponent<Health>());
  }

  public IEnumerator ExplodeCoroutine(bool canThrowEggs)
  {
    TrapFleshBall trapFleshBall = this;
    double spawnDelay = (double) trapFleshBall.spawnDelay;
    if (AudioManager.Instance.IsEventInstancePlaying(trapFleshBall.rollingLoopInstance))
      AudioManager.Instance.StopLoop(trapFleshBall.rollingLoopInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
    if (trapFleshBall.spine.AnimationState != null)
    {
      trapFleshBall.ballMeshRenderer.enabled = true;
      trapFleshBall.spine.AnimationState.SetAnimation(0, trapFleshBall.breakAnimation, false);
    }
    trapFleshBall.col.enabled = true;
    trapFleshBall.damageColliderEvents.enabled = true;
    trapFleshBall.spine.enabled = false;
    trapFleshBall.move = false;
    Health.team2.Add((Health) null);
    Interaction_Chest.Instance?.Enemies.Add((Health) null);
    yield return (object) null;
    if (trapFleshBall.dropped & canThrowEggs)
    {
      for (int index = trapFleshBall.spawnAmount - 1; index >= 0; --index)
        trapFleshBall.ThrowEgg();
    }
    if ((UnityEngine.Object) trapFleshBall.spawnParticles != (UnityEngine.Object) null)
    {
      trapFleshBall.spawnParticles.transform.parent = (Transform) null;
      trapFleshBall.spawnParticles.transform.position = new Vector3(trapFleshBall.spine.transform.position.x, trapFleshBall.spine.transform.position.y, 0.0f);
      trapFleshBall.spawnParticles.Play();
    }
    Health.team2.Remove((Health) null);
    Interaction_Chest.Instance?.Enemies.Remove((Health) null);
    UnityEngine.Object.Destroy((UnityEngine.Object) trapFleshBall.gameObject);
  }

  public Vector3 GetTargetPosition()
  {
    float num = 100f;
    while ((double) --num >= 0.0)
    {
      Vector3 point = this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * (float) this.spawnRadius;
      if (BiomeGenerator.PointWithinIsland(point, out Vector3 _) && (UnityEngine.Object) Physics2D.OverlapCircle((Vector2) point, 0.5f, (int) this.hitLayers, 0.0f, 0.0f) == (UnityEngine.Object) null)
        return point;
    }
    return Vector3.zero;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(this.moveVX, this.moveVY).normalized * this.bounceDistanceCheck, Color.blue);
  }

  public void DetachFromChain(bool keepChain = false)
  {
    this.ballMeshRenderer.enabled = true;
    this.spine.AnimationState.SetAnimation(0, this.fallAnimation, false);
    if (keepChain)
    {
      this.spineChain.transform.SetParent(this.transform.parent);
      this.spineChain.Skeleton.SetSkin("no-ball");
      this.spineChain.Skeleton.SetToSetupPose();
      this.spineChain.AnimationState.SetAnimation(0, this.fallAnimation, false);
    }
    else
      this.spineChain.gameObject.SetActive(false);
  }

  public void EnableTrap(float angle)
  {
    this.DetachFromChain();
    this.direction = this.FilterDirection(angle);
    this.move = true;
    this.damageColliderEvents = this.GetComponent<ColliderEvents>();
    this.damageColliderEvents.enabled = true;
    this.col.enabled = true;
    this.spine.transform.position = this.transform.position;
    this.spine.AnimationState.SetAnimation(0, this.rollAnimation, true);
  }

  public void RelocateInIsland()
  {
    Vector3 closestPoint;
    if (BiomeGenerator.PointWithinIsland(this.transform.position, out closestPoint))
      return;
    this.transform.position = closestPoint - closestPoint.normalized;
  }

  [CompilerGenerated]
  public void \u003CFreezeMovement\u003Eb__79_0() => this.rb.simulated = true;

  public enum MovementType
  {
    Linear,
    Accelerated,
  }

  public enum Orientation
  {
    right,
    left,
  }
}
