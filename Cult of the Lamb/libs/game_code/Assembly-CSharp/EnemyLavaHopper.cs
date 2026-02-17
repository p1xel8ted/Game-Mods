// Decompiled with JetBrains decompiler
// Type: EnemyLavaHopper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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

#nullable disable
public class EnemyLavaHopper : UnitObject
{
  public ColliderEvents damageColliderEvents;
  public IEnumerator damageColliderRoutine;
  public List<SkeletonAnimation> Spine = new List<SkeletonAnimation>();
  public SimpleSpineFlash SimpleSpineFlash;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string OnLandSoundPath = string.Empty;
  [EventRef]
  public string OnJumpSoundPath = string.Empty;
  [EventRef]
  public string OnLayEggSoundPath = string.Empty;
  [EventRef]
  public string OnCroakSoundPath = string.Empty;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  public bool _playedVO;
  public GameObject targetObject;
  public float TargetAngle;
  public List<Collider2D> collider2DList = new List<Collider2D>();
  public Health EnemyHealth;
  public bool canBeParried = true;
  public float signPostParryWindow = 0.2f;
  public float attackParryWindow = 0.3f;
  public GameManager gm;
  public CircleCollider2D physicsCollider;
  public float dancingTimestamp;
  public float dancingDuration;
  public float idleTimestamp;
  public float idleDur = 0.6f;
  public float signPostDur = 0.2f;
  public bool canBeStunned = true;
  public bool stunnedStopsAttack = true;
  public float stunnedTimestamp;
  public float stunnedDur = 0.5f;
  public float chargingTimestamp;
  public float chargingDuration = 0.5f;
  public float attackingTimestamp;
  public float attackRange;
  public float hoppingTimestamp;
  public float hoppingDur = 0.4f;
  public float attackingDur = 0.2f;
  public bool isFleeing;
  public float hopMoveSpeed = 1f;
  public AnimationCurve hopSpeedCurve;
  public AnimationCurve hopZCurve;
  public float hopZHeight;
  public static float zFallSpeed = 15f;
  public bool hasCollidedWithObstacle;
  public bool canLayEggs;
  public GameObject eggPrefab;
  public float lastLaidEggTimestamp;
  public float minTimeBetweenEggs = 6f;
  public bool alwaysTargetPlayer;
  public static List<EnemyLavaHopper> EnemyHoppers = new List<EnemyLavaHopper>();
  public static int maxHoppersPerRoom = 3;
  public static int maxEggsPerRoom = 1;
  public string chargingAnimationString = "lay-egg";
  public bool canSprayPoison;
  public bool canSprayPoisonOnLand;
  public float SprayPoisonRandomCircleSize = 1f;
  public bool EmitSmokeVfxOnLand = true;
  public bool forcePoisonActive;
  public GameObject poisonPrefab;
  public bool WillSeekRegenPointsAtLowHealth;
  [SerializeField]
  public float healthRegenThreshold = 0.6f;
  public List<GameObject> regenPoints = new List<GameObject>();
  [SerializeField]
  public List<GameObject> regenPointCandidates = new List<GameObject>();
  public ParticleSystem regenParticles;
  public bool seekingRegenPoint;
  public GameObject targetRegenPoint;
  public GameObject notSeekingRegenVisualGameObject;
  public GameObject seekingRegenVisualGameObject;
  public bool jumpingToRegenPoint;
  public List<FollowAsTail> TailPieces = new List<FollowAsTail>();
  public float KnockBackMultipier = 0.7f;

  public void InitRegenPoints()
  {
    DOVirtual.DelayedCall(1f, (TweenCallback) (() =>
    {
      this.regenPoints.Clear();
      for (int index = 0; index < 8; ++index)
      {
        Vector3 direction = Vector3.zero;
        switch (index)
        {
          case 0:
            direction = new Vector3(1f, 0.0f, 0.0f);
            break;
          case 1:
            direction = new Vector3(-1f, 0.0f, 0.0f);
            break;
          case 2:
            direction = new Vector3(0.0f, 1f, 0.0f);
            break;
          case 3:
            direction = new Vector3(0.0f, -1f, 0.0f);
            break;
          case 4:
            direction = new Vector3(1f, 1f, 0.0f);
            break;
          case 5:
            direction = new Vector3(-1f, 1f, 0.0f);
            break;
          case 6:
            direction = new Vector3(-1f, -1f, 0.0f);
            break;
          case 7:
            direction = new Vector3(1f, -1f, 0.0f);
            break;
        }
        Vector3 vector3 = this.CastAndValidateRegenPoint(direction);
        if (vector3 != Vector3.zero)
          this.regenPoints.Add(new GameObject()
          {
            name = "REGEN POINT " + index.ToString(),
            transform = {
              position = vector3
            }
          });
      }
      if (this.regenPoints.Count == 0)
      {
        Debug.Log((object) "No valid regen points found.");
        this.WillSeekRegenPointsAtLowHealth = false;
      }
      else
        Debug.Log((object) ("Valid regen points found: " + this.regenPoints.Count.ToString()));
    }));
  }

  public override void OnDestroy()
  {
    for (int index = 0; index < this.regenPoints.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.regenPoints[index]);
    base.OnDestroy();
  }

  public Vector3 CastAndValidateRegenPoint(Vector3 direction)
  {
    int mask = LayerMask.GetMask("Island");
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) direction, float.PositiveInfinity, mask);
    if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
      return Vector3.zero;
    Debug.Log((object) ("Path to regen point is blocked by: " + raycastHit2D.collider.name));
    Vector3 a = (Vector3) raycastHit2D.point + new Vector3(direction.x * 3f, direction.y * 3f, 4f);
    return (double) Vector3.Distance(a, this.transform.position) > 30.0 ? Vector3.zero : a;
  }

  public void FindFurthestRegenPoint()
  {
    if ((UnityEngine.Object) this.targetRegenPoint != (UnityEngine.Object) null)
      return;
    float num1 = 0.0f;
    GameObject gameObject = (GameObject) null;
    foreach (GameObject regenPoint in this.regenPoints)
    {
      float num2 = Vector3.Distance(this.transform.position, regenPoint.transform.position);
      if ((double) num2 > (double) num1)
      {
        num1 = num2;
        gameObject = regenPoint;
      }
    }
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    this.seekingRegenPoint = true;
    this.targetRegenPoint = gameObject;
    this.state.CURRENT_STATE = StateMachine.State.Moving;
  }

  public void ModifyExcludeLayers(CircleCollider2D collider, string layerName, bool add)
  {
    int layer = LayerMask.NameToLayer(layerName);
    if (layer < 0)
    {
      Debug.LogWarning((object) $"Layer '{layerName}' does not exist!");
    }
    else
    {
      LayerMask excludeLayers = collider.excludeLayers;
      LayerMask layerMask;
      if (add)
      {
        layerMask = (LayerMask) ((int) excludeLayers | 1 << layer);
        Debug.Log((object) $"Added '{layerName}' to Exclude Layers");
      }
      else
      {
        layerMask = (LayerMask) ((int) excludeLayers & ~(1 << layer));
        Debug.Log((object) $"Removed '{layerName}' from Exclude Layers");
      }
      collider.excludeLayers = layerMask;
    }
  }

  public float SeekRegenPointBehavior()
  {
    if (this.jumpingToRegenPoint)
      return 0.0f;
    this.FindFurthestRegenPoint();
    return Utils.GetAngle(this.transform.position, this.targetRegenPoint.transform.position);
  }

  public IEnumerator JumpToRegenPoint(GameObject regenPoint)
  {
    EnemyLavaHopper enemyLavaHopper = this;
    Vector3 originalPosition = enemyLavaHopper.transform.position;
    float jumpHeight = 8f;
    float jumpDuration = 1f;
    float elapsedTime = 0.0f;
    enemyLavaHopper.state.CURRENT_STATE = StateMachine.State.HitRecover;
    foreach (SkeletonAnimation skeletonAnimation in enemyLavaHopper.Spine)
    {
      if (skeletonAnimation.AnimationState != null)
        skeletonAnimation.AnimationState.SetAnimation(0, "animation", false);
    }
    yield return (object) new WaitForSeconds(0.5f);
    Vector3 startPosition = enemyLavaHopper.transform.position;
    Vector3 endPosition = regenPoint.transform.position;
    while ((double) elapsedTime < (double) jumpDuration)
    {
      elapsedTime += Time.deltaTime;
      float t = elapsedTime / jumpDuration;
      float num = Mathf.Sin(3.14159274f * t) * jumpHeight;
      enemyLavaHopper.transform.position = Vector3.Lerp(startPosition, endPosition, t) + new Vector3(0.0f, 0.0f, -num);
      yield return (object) null;
    }
    enemyLavaHopper.RegenerateHealth();
    enemyLavaHopper.seekingRegenPoint = false;
    enemyLavaHopper.transform.position = endPosition;
    yield return (object) enemyLavaHopper.StartCoroutine((IEnumerator) enemyLavaHopper.JumpBackToOriginalPosition(originalPosition));
  }

  public IEnumerator JumpBackToOriginalPosition(Vector3 originalPosition)
  {
    EnemyLavaHopper enemyLavaHopper = this;
    yield return (object) new WaitForSeconds(1f);
    float jumpHeight = 8f;
    float jumpDuration = 1f;
    float elapsedTime = 0.0f;
    Vector3 startPosition = enemyLavaHopper.transform.position;
    Vector3 endPosition = originalPosition;
    while ((double) elapsedTime < (double) jumpDuration)
    {
      elapsedTime += Time.deltaTime;
      float t = elapsedTime / jumpDuration;
      float num = Mathf.Sin(3.14159274f * t) * jumpHeight;
      enemyLavaHopper.transform.position = Vector3.Lerp(startPosition, endPosition, t) + new Vector3(0.0f, 0.0f, -num);
      yield return (object) null;
    }
    enemyLavaHopper.transform.position = endPosition;
    enemyLavaHopper.seekingRegenPoint = false;
    enemyLavaHopper.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyLavaHopper.jumpingToRegenPoint = false;
    enemyLavaHopper.targetRegenPoint = (GameObject) null;
    enemyLavaHopper.ModifyExcludeLayers(enemyLavaHopper.GetComponent<CircleCollider2D>(), "Island", false);
  }

  public void RegenerateHealth()
  {
    this.health.HP = this.health.totalHP;
    ShowHPBar component = this.GetComponent<ShowHPBar>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.gameObject.SetActive(false);
      component.gameObject.SetActive(true);
    }
    if ((UnityEngine.Object) this.regenParticles != (UnityEngine.Object) null)
      this.regenParticles.Play();
    Debug.Log((object) "Health fully regenerated.");
  }

  public override void Awake()
  {
    base.Awake();
    this.InitRegenPoints();
    this.physicsCollider = this.GetComponent<CircleCollider2D>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    this.state.OnStateChange += new StateMachine.StateChange(this.OnStateChange);
    this.gm = GameManager.GetInstance();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if ((UnityEngine.Object) this.gm != (UnityEngine.Object) null)
      this.idleTimestamp = this.gm.CurrentTime;
    if (EnemyLavaHopper.EnemyHoppers != null)
      this.idleTimestamp += (float) EnemyLavaHopper.EnemyHoppers.Count * 0.1f;
    else
      this.idleTimestamp += UnityEngine.Random.Range(0.0f, this.idleDur);
    EnemyLavaHopper.EnemyHoppers.Add(this);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnDisable()
  {
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    this.state.OnStateChange -= new StateMachine.StateChange(this.OnStateChange);
    this.ClearPaths();
    EnemyLavaHopper.EnemyHoppers.Remove(this);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void Update()
  {
    base.Update();
    this.Seperate(0.5f, true);
    if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
    {
      this.gm = GameManager.GetInstance();
      if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
        return;
    }
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.UpdateStateIdle();
        break;
      case StateMachine.State.Moving:
        this.UpdateStateMoving();
        break;
      case StateMachine.State.Charging:
        this.UpdateStateCharging();
        break;
      case StateMachine.State.Vulnerable:
        this.UpdateStateVulnerable();
        break;
      case StateMachine.State.Dancing:
        this.UpdateStateDancing();
        break;
    }
    if (this.WillSeekRegenPointsAtLowHealth)
      this.notSeekingRegenVisualGameObject.SetActive(!this.seekingRegenPoint);
    int index = -1;
    while (++index < this.TailPieces.Count)
    {
      if ((UnityEngine.Object) this.TailPieces[index] != (UnityEngine.Object) null)
        this.TailPieces[index].UpdatePosition();
    }
    if (this.state.CURRENT_STATE != StateMachine.State.Moving)
      this.Spine[0].transform.localPosition = Vector3.Lerp(this.Spine[0].transform.localPosition, Vector3.zero, Time.deltaTime * EnemyLavaHopper.zFallSpeed);
    this.move();
  }

  public virtual void UpdateStateIdle()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.idleTimestamp) >= ((double) this.idleDur - (double) this.signPostParryWindow) * (double) this.Spine[0].timeScale)
      this.canBeParried = true;
    float num = this.idleDur - this.signPostDur;
    if ((double) this.gm.TimeSince(this.idleTimestamp) >= (double) num * (double) this.Spine[0].timeScale)
      this.SimpleSpineFlash.FlashWhite((float) (0.75 * ((double) this.gm.TimeSince(this.idleTimestamp) - (double) num / (double) this.signPostDur)) * this.Spine[0].timeScale);
    if ((double) this.gm.TimeSince(this.idleTimestamp) < (double) this.idleDur * (double) this.Spine[0].timeScale)
      return;
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null && (UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null)
      this.targetObject = this.GetClosestTarget().gameObject;
    bool flag = this.TargetIsVisible();
    if (this.canLayEggs && (double) this.gm.TimeSince(this.lastLaidEggTimestamp - 0.5f) >= (double) this.minTimeBetweenEggs * (double) this.Spine[0].timeScale && EnemyLavaHopper.EnemyHoppers.Count < EnemyLavaHopper.maxHoppersPerRoom && EnemyEgg.EnemyEggs.Count < EnemyLavaHopper.maxEggsPerRoom)
    {
      this.alwaysTargetPlayer = false;
      this.isFleeing = true;
    }
    this.TargetAngle = !this.alwaysTargetPlayer ? (!this.seekingRegenPoint ? (!this.isFleeing ? (!flag ? this.GetRandomFacingAngle() : this.GetAngleToTarget()) : this.GetFleeAngle()) : this.SeekRegenPointBehavior()) : (!((UnityEngine.Object) this.GetClosestTarget() != (UnityEngine.Object) null) ? this.GetFleeAngle() : this.GetAngleToTarget());
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
    foreach (SkeletonAnimation skeletonAnimation in this.Spine)
    {
      if ((double) this.Spine[0].timeScale != 9.9999997473787516E-05)
        skeletonAnimation.skeleton.ScaleX = (double) this.state.LookAngle <= 90.0 || (double) this.state.LookAngle >= 270.0 ? -1f : 1f;
    }
    if (this.ShouldSprayPoison())
      this.SprayPoison();
    this.state.CURRENT_STATE = StateMachine.State.Moving;
  }

  public virtual void UpdateStateMoving()
  {
    if (this.jumpingToRegenPoint)
      return;
    if (!this._playedVO)
    {
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.gameObject);
      this._playedVO = true;
    }
    this.speed = this.hopSpeedCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / (this.hoppingDur * this.Spine[0].timeScale)) * this.hopMoveSpeed;
    if (this.hasCollidedWithObstacle || this.TargetIsInAttackRange())
      this.speed *= 0.5f;
    this.speed *= this.Spine[0].timeScale;
    this.Spine[0].transform.localPosition = -Vector3.forward * this.hopZCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopZHeight * this.Spine[0].timeScale;
    this.canBeParried = (double) this.gm.TimeSince(this.hoppingTimestamp) <= (double) this.attackParryWindow * (double) this.Spine[0].timeScale;
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) < (double) this.hoppingDur / (double) this.Spine[0].timeScale)
      return;
    this.speed = 0.0f;
    this.DoAttack();
    this._playedVO = false;
    if (this.ShouldStartCharging())
      this.state.CURRENT_STATE = StateMachine.State.Charging;
    else
      this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void DoAttack()
  {
    AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
    if (this.damageColliderRoutine != null)
      this.StopCoroutine((IEnumerator) this.damageColliderRoutine);
    this.damageColliderRoutine = this.TurnOnDamageColliderForDuration(this.attackingDur);
    this.StartCoroutine((IEnumerator) this.damageColliderRoutine);
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive((double) this.Spine[0].timeScale != 9.9999997473787516E-05);
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      t += Time.deltaTime;
      this.SimpleSpineFlash.FlashWhite(1f - Mathf.Clamp01(t / duration));
      yield return (object) null;
    }
    this.SimpleSpineFlash.FlashWhite(false);
    this.damageColliderEvents.SetActive(false);
  }

  public virtual void UpdateStateVulnerable()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.stunnedTimestamp) < (double) this.stunnedDur * (double) this.Spine[0].timeScale)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void UpdateStateCharging()
  {
    if ((double) this.gm.TimeSince(this.chargingTimestamp) < (double) this.chargingDuration * (double) this.Spine[0].timeScale || !this.canLayEggs || EnemyLavaHopper.EnemyHoppers.Count >= EnemyLavaHopper.maxHoppersPerRoom || EnemyEgg.EnemyEggs.Count >= EnemyLavaHopper.maxEggsPerRoom)
      return;
    this.LayEgg();
    this.idleDur = this.signPostParryWindow;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.alwaysTargetPlayer = true;
    this.isFleeing = false;
  }

  public virtual void UpdateStateDancing()
  {
    this.speed = 0.0f;
    if ((double) this.gm.TimeSince(this.dancingTimestamp) < (double) this.dancingDuration * (double) this.Spine[0].timeScale)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void GetTailPieces()
  {
    this.TailPieces = new List<FollowAsTail>((IEnumerable<FollowAsTail>) this.GetComponentsInChildren<FollowAsTail>());
  }

  public bool TargetIsVisible()
  {
    if (!GameManager.RoomActive || (UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
      return false;
    float a = Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.targetObject.transform.position, this.transform.position));
    return (double) a <= (double) this.VisionRange && this.CheckLineOfSightOnTarget(this.targetObject, this.targetObject.transform.position, Mathf.Min(a, (float) this.VisionRange));
  }

  public bool TargetIsInAttackRange()
  {
    return GameManager.RoomActive && !((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null) && (double) Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.targetObject.transform.position, this.transform.position)) <= (double) this.attackRange * 0.75;
  }

  public Vector3 GetHopPositionTowardsTarget(Vector3 targetPos)
  {
    return (targetPos - this.transform.position).normalized * this.attackRange + this.transform.position;
  }

  public float GetAngleToTarget()
  {
    float angle = Utils.GetAngle(this.transform.position, this.targetObject.transform.position);
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return angle;
    float num = 32f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(angle * ((float) Math.PI / 180f)), Mathf.Sin(angle * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      angle += (float) (360.0 / ((double) num + 1.0));
    return angle;
  }

  public float GetRandomFacingAngle()
  {
    float randomFacingAngle = (float) UnityEngine.Random.Range(0, 360);
    if ((UnityEngine.Object) this.physicsCollider == (UnityEngine.Object) null)
      return randomFacingAngle;
    float num = 16f;
    for (int index = 0; (double) index < (double) num && (bool) Physics2D.CircleCast((Vector2) this.transform.position, this.physicsCollider.radius, new Vector2(Mathf.Cos(randomFacingAngle * ((float) Math.PI / 180f)), Mathf.Sin(randomFacingAngle * ((float) Math.PI / 180f))), this.attackRange * 0.5f, (int) this.layerToCheck); ++index)
      randomFacingAngle += (float) (360.0 / ((double) num + 1.0));
    return randomFacingAngle;
  }

  public float GetFleeAngle()
  {
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
      return this.GetRandomFacingAngle();
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(4, 7);
      Vector3 toPosition = this.targetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.targetObject.transform.position, 1.5f, (Vector2) Vector3.Normalize(toPosition - this.targetObject.transform.position), distance, (int) this.layerToCheck);
      if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null))
        return Utils.GetAngle(this.transform.position, toPosition);
      if ((double) this.MagnitudeFindDistanceBetween(this.targetObject.transform.position, (Vector3) raycastHit2D.centroid) > 9.0)
        return Utils.GetAngle(this.transform.position, toPosition);
    }
    return this.GetRandomFacingAngle();
  }

  public virtual bool ShouldStartCharging()
  {
    return GameManager.RoomActive && this.canLayEggs && (double) this.gm.TimeSince(this.lastLaidEggTimestamp) >= (double) this.minTimeBetweenEggs * (double) this.Spine[0].timeScale && EnemyLavaHopper.EnemyHoppers.Count < EnemyLavaHopper.maxHoppersPerRoom && EnemyEgg.EnemyEggs.Count < EnemyLavaHopper.maxEggsPerRoom;
  }

  public bool ShouldSprayPoison() => this.canSprayPoison;

  public bool ShouldSprayPoisonOnLand() => this.canSprayPoisonOnLand;

  public virtual void OnStateChange(StateMachine.State newState, StateMachine.State prevState)
  {
    if ((UnityEngine.Object) this.gm == (UnityEngine.Object) null)
      return;
    switch (newState)
    {
      case StateMachine.State.Idle:
        if (newState == prevState)
          break;
        this.idleTimestamp = this.gm.CurrentTime;
        foreach (SkeletonAnimation skeletonAnimation in this.Spine)
        {
          if (skeletonAnimation.AnimationState != null)
          {
            if (prevState == StateMachine.State.Moving)
              skeletonAnimation.AnimationState.SetAnimation(0, "jump-end", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          }
        }
        if (!string.IsNullOrEmpty(this.OnLandSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnLandSoundPath, this.transform.position);
        this.targetObject = (GameObject) null;
        this.SimpleSpineFlash.FlashWhite(false);
        break;
      case StateMachine.State.Moving:
        this.hasCollidedWithObstacle = false;
        this.hoppingTimestamp = this.gm.CurrentTime;
        this.SimpleSpineFlash.FlashWhite(false);
        foreach (SkeletonAnimation skeletonAnimation in this.Spine)
        {
          if (skeletonAnimation.AnimationState != null)
            skeletonAnimation.AnimationState.SetAnimation(0, "jump", false);
        }
        if (string.IsNullOrEmpty(this.OnJumpSoundPath))
          break;
        AudioManager.Instance.PlayOneShot(this.OnJumpSoundPath, this.transform.position);
        break;
      case StateMachine.State.Charging:
        foreach (SkeletonAnimation skeletonAnimation in this.Spine)
        {
          if (skeletonAnimation.AnimationState != null)
          {
            skeletonAnimation.AnimationState.SetAnimation(0, this.chargingAnimationString, false);
            skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          }
        }
        if (!string.IsNullOrEmpty(this.OnLayEggSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnLayEggSoundPath, this.transform.position);
        this.chargingTimestamp = this.gm.CurrentTime;
        break;
      case StateMachine.State.Vulnerable:
        this.stunnedTimestamp = this.gm.CurrentTime;
        this.SimpleSpineFlash.FlashWhite(false);
        if (!this.stunnedStopsAttack || !((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
          break;
        this.damageColliderEvents.SetActive(false);
        break;
      case StateMachine.State.Dancing:
        this.dancingTimestamp = this.gm.CurrentTime;
        foreach (SkeletonAnimation skeletonAnimation in this.Spine)
        {
          if (skeletonAnimation.AnimationState != null)
          {
            skeletonAnimation.AnimationState.SetAnimation(0, "roar", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          }
        }
        if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
          this.damageColliderEvents.SetActive(false);
        if (string.IsNullOrEmpty(this.OnCroakSoundPath))
          break;
        AudioManager.Instance.PlayOneShot(this.OnCroakSoundPath, this.transform.position);
        break;
    }
  }

  public new void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.canBeParried)
      return;
    this.health.WasJustParried = true;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (string.IsNullOrEmpty(this.DeathVO))
      return;
    AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.health.HasShield)
      return;
    if ((double) this.KnockBackMultipier != 0.0)
      this.DoKnockBack(Attacker, this.KnockBackMultipier, 1f);
    if (this.WillSeekRegenPointsAtLowHealth && 1.0 / (double) this.health.totalHP * (double) this.health.HP < (double) this.healthRegenThreshold)
      this.seekingRegenPoint = true;
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.SimpleSpineFlash.FlashWhite(false);
    this.UsePathing = true;
    this.health.invincible = false;
    this.targetObject = (GameObject) null;
    if (this.canBeStunned && (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Attacking))
    {
      this.state.CURRENT_STATE = StateMachine.State.Vulnerable;
      foreach (SkeletonAnimation skeletonAnimation in this.Spine)
      {
        if (skeletonAnimation.AnimationState != null)
        {
          skeletonAnimation.AnimationState.SetAnimation(0, "hit", false);
          skeletonAnimation.AnimationState.AddAnimation(0, "idle", false, 0.0f);
        }
      }
    }
    this.SimpleSpineFlash.FlashFillRed();
  }

  public void LayEgg()
  {
    if ((UnityEngine.Object) this.gm != (UnityEngine.Object) null)
      this.lastLaidEggTimestamp = this.gm.CurrentTime;
    this.SimpleSpineFlash.FlashWhite(false);
    int num = 1;
    for (int index = 0; index < num; ++index)
    {
      GameObject gameObject = ObjectPool.Spawn(this.eggPrefab, (UnityEngine.Object) this.transform.parent != (UnityEngine.Object) null ? this.transform.parent : (Transform) null, this.transform.position + new Vector3(0.0f, -0.5f, 0.0f), Quaternion.identity);
      gameObject.GetComponent<UnitObject>().DisableForces = true;
      gameObject.GetComponent<DeadBodySliding>().Init(this.gameObject, UnityEngine.Random.Range(0.0f, 360f), 1200f);
      gameObject.transform.localScale = Vector3.zero;
      gameObject.transform.DOScale(Vector3.one, 1f);
    }
  }

  public void SprayPoison()
  {
    this.SimpleSpineFlash.FlashWhite(false);
    int num = 1;
    for (int index = 0; index < num; ++index)
    {
      GameObject trap = UnityEngine.Object.Instantiate<GameObject>(this.poisonPrefab, this.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * this.SprayPoisonRandomCircleSize), Quaternion.identity, (Transform) null);
      if (this.forcePoisonActive)
      {
        trap.gameObject.SetActive(true);
        trap.transform.localScale = Vector3.zero;
        trap.transform.DOScale(Vector3.one * 0.66f, 0.33f);
        trap.transform.DOScale(Vector3.zero, 0.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) trap.gameObject)));
      }
    }
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Moving || collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
      return;
    Debug.Log((object) "I have hopped into an obstacle", (UnityEngine.Object) this.gameObject);
    this.hasCollidedWithObstacle = true;
    this.TargetAngle += (double) this.TargetAngle > 180.0 ? -180f : 180f;
    this.state.LookAngle = this.TargetAngle;
    this.state.facingAngle = this.TargetAngle;
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  [CompilerGenerated]
  public void \u003CInitRegenPoints\u003Eb__72_0()
  {
    this.regenPoints.Clear();
    for (int index = 0; index < 8; ++index)
    {
      Vector3 direction = Vector3.zero;
      switch (index)
      {
        case 0:
          direction = new Vector3(1f, 0.0f, 0.0f);
          break;
        case 1:
          direction = new Vector3(-1f, 0.0f, 0.0f);
          break;
        case 2:
          direction = new Vector3(0.0f, 1f, 0.0f);
          break;
        case 3:
          direction = new Vector3(0.0f, -1f, 0.0f);
          break;
        case 4:
          direction = new Vector3(1f, 1f, 0.0f);
          break;
        case 5:
          direction = new Vector3(-1f, 1f, 0.0f);
          break;
        case 6:
          direction = new Vector3(-1f, -1f, 0.0f);
          break;
        case 7:
          direction = new Vector3(1f, -1f, 0.0f);
          break;
      }
      Vector3 vector3 = this.CastAndValidateRegenPoint(direction);
      if (vector3 != Vector3.zero)
      {
        GameObject gameObject = new GameObject();
        gameObject.name = "REGEN POINT " + index.ToString();
        gameObject.transform.position = vector3;
        this.regenPoints.Add(gameObject);
      }
    }
    if (this.regenPoints.Count == 0)
    {
      Debug.Log((object) "No valid regen points found.");
      this.WillSeekRegenPointsAtLowHealth = false;
    }
    else
      Debug.Log((object) ("Valid regen points found: " + this.regenPoints.Count.ToString()));
  }
}
