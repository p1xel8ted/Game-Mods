// Decompiled with JetBrains decompiler
// Type: EnemyLambFlesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyLambFlesh : UnitObject, IAttackResilient
{
  public static List<EnemyLambFlesh> LambFleshEnemies = new List<EnemyLambFlesh>();
  public static List<EnemyLambFlesh.EnemyPair> FleshlingPairs = new List<EnemyLambFlesh.EnemyPair>();
  public static int kebabAmount;
  public AssetReferenceGameObject indicatorPrefab;
  [SerializeField]
  public SpriteRenderer Aiming;
  public DeadBodyFlying DeadBody;
  public ColliderEvents damageColliderEvents;
  public ColliderEvents triggerColliderEvents;
  public SimpleSpineFlash SimpleSpineFlash;
  public UnityEvent OnReadyAction;
  public bool StandaloneSpawn;
  [CompilerGenerated]
  public EnemyLambFlesh.State \u003CMyState\u003Ek__BackingField = EnemyLambFlesh.State.Idle;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SignPostAttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AttackAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string FuseSmallAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string JumpAnimation;
  public SkeletonAnimation warningIcon;
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/death";
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/gethit";
  [EventRef]
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/warning";
  [EventRef]
  public string AttackSpikesSFX = "event:/dlc/dungeon06/enemy/lambflesh/attack_spikes";
  [EventRef]
  public string JumpSFX = "event:/dlc/dungeon06/enemy/lambflesh/mv_jump_launch";
  [EventRef]
  public string CrawlSFX = "event:/dlc/dungeon06/enemy/lambflesh/move";
  public Vector3? StartingPosition;
  public Vector3 TargetPosition;
  [SerializeField]
  public bool canMerge;
  [SerializeField]
  public float mergeDistance = 0.5f;
  [SerializeField]
  public float mergeDuration = 1f;
  [SerializeField]
  public EnemyKebab mergeResult;
  [SerializeField]
  public float spawnedKebabCheckCooldown = 1f;
  public EnemyLambFlesh mergeTarget;
  public bool merging;
  [SerializeField]
  public float dodgeMultiplier = 1f;
  [SerializeField]
  public Vector2 dodgeCooldownRange = new Vector2(2f, 4f);
  [SerializeField]
  public SkeletonGhost dodgeGhost;
  [SerializeField]
  public ParticleSystem landVFX;
  [SerializeField]
  public LayerMask attackDamageLayers;
  [SerializeField]
  public float dodgeMaxDisplacement = 1.5f;
  [SerializeField]
  public float landingDamageRadius = 1.2f;
  public float dodgedTimestamp;
  [CompilerGenerated]
  public Vector3 \u003CCurrentDodgeTargetPosition\u003Ek__BackingField;
  public float maxKebabsAllowed = 5f;
  public GameObject targetObject;
  public bool canDodge = true;
  public GameObject loadedIndicator;
  public GameObject currentIndicator;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public float MaximumRange = 5f;
  public float attackDelay = 0.5f;
  public float AttackCoolDown = 1f;
  public float attackDuration = 1f;
  public float lookForTargetInterval = 1f;
  public Coroutine WaitForTargetRoutine;
  public float Angle;
  public Vector3 Force;
  public float KnockbackForceModifier = 1f;
  public float KnockbackDuration = 0.5f;
  public bool AttackAfterKnockback;
  public Health EnemyHealth;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  public bool MoveTweenQeued;
  public bool spawning;

  public EnemyLambFlesh.State MyState
  {
    get => this.\u003CMyState\u003Ek__BackingField;
    set => this.\u003CMyState\u003Ek__BackingField = value;
  }

  public Vector3 CurrentDodgeTargetPosition
  {
    get => this.\u003CCurrentDodgeTargetPosition\u003Ek__BackingField;
    set => this.\u003CCurrentDodgeTargetPosition\u003Ek__BackingField = value;
  }

  public void Start()
  {
    if ((UnityEngine.Object) this.Aiming != (UnityEngine.Object) null)
      this.Aiming.gameObject.SetActive(false);
    this.LoadAssets();
  }

  public void LoadAssets()
  {
    Addressables.LoadAssetAsync<GameObject>((object) this.indicatorPrefab).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      EnemyLambFlesh.loadedAddressableAssets.Add(obj);
      this.loadedIndicator = obj.Result;
      this.loadedIndicator.CreatePool(10, true);
    });
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyLambFlesh.LambFleshEnemies.Add(this);
    this.TryAddFleshlingToList();
    if (!this.StartingPosition.HasValue)
    {
      this.StartingPosition = new Vector3?(this.transform.position);
      this.TargetPosition = this.StartingPosition.Value;
    }
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.MyState = EnemyLambFlesh.State.Idle;
    this.SimpleSpineFlash.FlashWhite(false);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
      this.triggerColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
      this.triggerColliderEvents.SetActive(true);
    }
    this.StartCoroutine(!this.StandaloneSpawn ? (IEnumerator) this.AwaitOnReadyAction() : (IEnumerator) this.MovementRoutine());
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (EnemyLambFlesh.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in EnemyLambFlesh.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    EnemyLambFlesh.loadedAddressableAssets.Clear();
  }

  public void TryAddFleshlingToList()
  {
    if (EnemyLambFlesh.FleshlingPairs.Any<EnemyLambFlesh.EnemyPair>((Func<EnemyLambFlesh.EnemyPair, bool>) (x => x.Contains((UnitObject) this))))
      return;
    EnemyLambFlesh.EnemyPair enemyPair = EnemyLambFlesh.FleshlingPairs.FirstOrDefault<EnemyLambFlesh.EnemyPair>((Func<EnemyLambFlesh.EnemyPair, bool>) (x => !x.IsPairComplete));
    if (enemyPair != null)
      enemyPair.AddToPair((UnitObject) this);
    else
      EnemyLambFlesh.FleshlingPairs.Add(new EnemyLambFlesh.EnemyPair((UnitObject) this));
  }

  public void RemoveFleshlingFromList()
  {
    EnemyLambFlesh.EnemyPair enemyPair = EnemyLambFlesh.FleshlingPairs.FirstOrDefault<EnemyLambFlesh.EnemyPair>((Func<EnemyLambFlesh.EnemyPair, bool>) (x => x.Contains((UnitObject) this)));
    if (enemyPair == null)
      return;
    enemyPair.RemoveFromPair((UnitObject) this);
    if (!enemyPair.IsPairEmpty)
      return;
    EnemyLambFlesh.FleshlingPairs.Remove(enemyPair);
  }

  public override void FixedUpdate()
  {
    if ((double) this.Spine.timeScale == 9.9999997473787516E-05)
      return;
    base.FixedUpdate();
  }

  public IEnumerator AwaitOnReadyAction()
  {
    bool loop = true;
    while (loop)
    {
      if (this.MoveTweenQeued)
      {
        this.OnReadyAction?.Invoke();
        this.OnReadyAction.RemoveAllListeners();
        this.MoveTweenQeued = false;
        loop = false;
      }
      yield return (object) null;
    }
  }

  public override void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHitEarly(Attacker, AttackLocation, AttackType, FromBehind);
    if (!this.canDodge || this.MyState == EnemyLambFlesh.State.Attacking || (double) Time.time <= (double) this.dodgedTimestamp || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack || AttackType == Health.AttackTypes.Projectile)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DodgeIE(Attacker));
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.StartCoroutine((IEnumerator) this.HurtRoutine());
    if (AttackType != Health.AttackTypes.NoKnockBack && (double) this.KnockbackForceModifier != 0.0)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public IEnumerator HurtRoutine()
  {
    EnemyLambFlesh enemyLambFlesh = this;
    if (!string.IsNullOrEmpty(enemyLambFlesh.GetHitVO))
      AudioManager.Instance.PlayOneShot(enemyLambFlesh.GetHitVO, enemyLambFlesh.gameObject);
    enemyLambFlesh.damageColliderEvents.SetActive(false);
    enemyLambFlesh.ClearPaths();
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLambFlesh.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemyLambFlesh.DisableForces = false;
    enemyLambFlesh.StartCoroutine((IEnumerator) enemyLambFlesh.MovementRoutine());
  }

  public IEnumerator DodgeIE(GameObject attacker)
  {
    EnemyLambFlesh enemyLambFlesh = this;
    enemyLambFlesh.MyState = EnemyLambFlesh.State.Dodging;
    if ((bool) (UnityEngine.Object) attacker)
      enemyLambFlesh.state.LookAngle = Utils.GetAngle(enemyLambFlesh.transform.position, attacker.transform.position);
    enemyLambFlesh.Spine.skeleton.ScaleX = (double) enemyLambFlesh.state.LookAngle <= 90.0 || (double) enemyLambFlesh.state.LookAngle >= 270.0 ? -1f : 1f;
    enemyLambFlesh.state.LookAngle = enemyLambFlesh.state.facingAngle = 270f;
    enemyLambFlesh.dodgedTimestamp = Time.time + UnityEngine.Random.Range(enemyLambFlesh.dodgeCooldownRange.x, enemyLambFlesh.dodgeCooldownRange.y);
    enemyLambFlesh.health.untouchable = true;
    enemyLambFlesh.health.invincible = true;
    float dodgeDuration = enemyLambFlesh.Spine.AnimationState.SetAnimation(0, enemyLambFlesh.JumpAnimation, false).Animation.Duration - 0.2f;
    enemyLambFlesh.Spine.AnimationState.AddAnimation(0, enemyLambFlesh.IdleAnimation, true, 0.0f);
    enemyLambFlesh.dodgeGhost.ghostingEnabled = true;
    if (!string.IsNullOrEmpty(enemyLambFlesh.JumpSFX))
      AudioManager.Instance.PlayOneShot(enemyLambFlesh.JumpSFX);
    Vector3 currentPosition = enemyLambFlesh.transform.position;
    enemyLambFlesh.CurrentDodgeTargetPosition = (UnityEngine.Object) attacker != (UnityEngine.Object) null ? enemyLambFlesh.CalculateCurrentDodgeTargetPosition(attacker) : currentPosition;
    if ((UnityEngine.Object) enemyLambFlesh.loadedIndicator != (UnityEngine.Object) null)
    {
      enemyLambFlesh.currentIndicator = ObjectPool.Spawn(enemyLambFlesh.loadedIndicator);
      if ((UnityEngine.Object) enemyLambFlesh.currentIndicator != (UnityEngine.Object) null)
        enemyLambFlesh.currentIndicator.transform.position = enemyLambFlesh.CurrentDodgeTargetPosition;
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime) < (double) dodgeDuration)
    {
      while (PlayerRelic.TimeFrozen || Health.isGlobalTimeFreeze)
        yield return (object) null;
      enemyLambFlesh.transform.position = Vector3.Lerp(currentPosition, enemyLambFlesh.CurrentDodgeTargetPosition, time / dodgeDuration);
      enemyLambFlesh.SimpleSpineFlash.FlashMeWhite(Mathf.Lerp(0.0f, 0.8f, time / dodgeDuration));
      yield return (object) null;
    }
    enemyLambFlesh.landVFX.Play();
    enemyLambFlesh.DealLandingDamage();
    if ((UnityEngine.Object) enemyLambFlesh.currentIndicator != (UnityEngine.Object) null)
      enemyLambFlesh.currentIndicator.Recycle();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.1f, enemyLambFlesh.Spine);
    enemyLambFlesh.health.untouchable = false;
    enemyLambFlesh.health.invincible = false;
    enemyLambFlesh.SimpleSpineFlash.FlashWhite(false);
    enemyLambFlesh.dodgeGhost.ghostingEnabled = false;
    enemyLambFlesh.MyState = EnemyLambFlesh.State.Idle;
    enemyLambFlesh.triggerColliderEvents.SetActive(true);
    enemyLambFlesh.WaitForTargetRoutine = enemyLambFlesh.StartCoroutine((IEnumerator) enemyLambFlesh.WaitForTarget());
  }

  public Vector3 CalculateCurrentDodgeTargetPosition(GameObject attacker)
  {
    Vector3 b = this.transform.position;
    Vector3 position = this.transform.position;
    Vector2 vector2 = (Vector2) (attacker.transform.position - this.transform.position);
    if ((UnityEngine.Object) attacker != (UnityEngine.Object) null)
      b = (double) vector2.magnitude >= (double) this.dodgeMaxDisplacement ? (Vector3) (vector2.normalized * this.dodgeMaxDisplacement) : attacker.transform.position;
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < EnemyLambFlesh.LambFleshEnemies.Count; ++index)
    {
      if (!((UnityEngine.Object) EnemyLambFlesh.LambFleshEnemies[index] == (UnityEngine.Object) this))
      {
        Vector3 vector3 = EnemyLambFlesh.LambFleshEnemies[index].MyState != EnemyLambFlesh.State.Dodging ? ((double) Vector3.Distance(EnemyLambFlesh.LambFleshEnemies[index].transform.position, b) >= 0.5 ? b - EnemyLambFlesh.LambFleshEnemies[index].transform.position : (Vector3) (UnityEngine.Random.insideUnitCircle.normalized * 0.5f)) : b - EnemyLambFlesh.LambFleshEnemies[index].CurrentDodgeTargetPosition;
        zero += 1f / vector3.magnitude * vector3.normalized;
      }
    }
    Vector3 point = b + zero;
    Vector3 closestPoint;
    if (!EnemyLambFlesh.PointWithinIsland(point, out closestPoint))
      point = closestPoint;
    return point;
  }

  public static bool PointWithinIsland(Vector3 point, out Vector3 closestPoint)
  {
    LayerMask layerMask = (LayerMask) ((int) (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island")) | 1 << LayerMask.NameToLayer("Obstacles Player Ignore"));
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) Vector3.zero, (Vector2) point.normalized, point.magnitude, (int) layerMask);
    closestPoint = !((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null) ? point : (Vector3) (raycastHit2D.point + raycastHit2D.normal);
    return (UnityEngine.Object) raycastHit2D.collider == (UnityEngine.Object) null;
  }

  public void DealLandingDamage()
  {
    foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.landingDamageRadius, (int) this.attackDamageLayers))
    {
      Health component2 = component1.gameObject.GetComponent<Health>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2 != (UnityEngine.Object) this.health && (component2.team != this.health.team || this.health.IsCharmedEnemy))
        component2.DealDamage(1f, this.gameObject, this.transform.position);
    }
  }

  public IEnumerator MovementRoutine()
  {
    EnemyLambFlesh enemyLambFlesh = this;
    bool loop = true;
    float kebabcheckTimer = 0.0f;
    while (loop)
    {
      if (enemyLambFlesh.MyState == EnemyLambFlesh.State.Dodging)
      {
        yield return (object) null;
      }
      else
      {
        if ((double) kebabcheckTimer > 0.0)
          kebabcheckTimer -= Time.deltaTime;
        if ((double) kebabcheckTimer <= 0.0)
        {
          kebabcheckTimer = enemyLambFlesh.spawnedKebabCheckCooldown;
          if (enemyLambFlesh.ShouldMerge() && (double) EnemyLambFlesh.kebabAmount <= (double) enemyLambFlesh.maxKebabsAllowed)
            enemyLambFlesh.StartMerge(true);
        }
        if ((bool) (UnityEngine.Object) enemyLambFlesh.targetObject && (double) Vector2.Distance((Vector2) enemyLambFlesh.transform.position, (Vector2) enemyLambFlesh.targetObject.transform.position) > 0.5)
        {
          enemyLambFlesh.FindPath(enemyLambFlesh.targetObject.transform.position);
          enemyLambFlesh.Angle = Utils.GetAngle(enemyLambFlesh.transform.position, enemyLambFlesh.targetObject.transform.position);
          if ((bool) (UnityEngine.Object) enemyLambFlesh.targetObject.GetComponent<EnemyLambFlesh>() && (double) EnemyLambFlesh.kebabAmount >= (double) enemyLambFlesh.maxKebabsAllowed)
            enemyLambFlesh.targetObject = (GameObject) null;
          enemyLambFlesh.Seperate(0.5f);
        }
        else
        {
          loop = false;
          if (enemyLambFlesh.state.CURRENT_STATE != StateMachine.State.Idle)
            enemyLambFlesh.state.CURRENT_STATE = StateMachine.State.Idle;
          if (enemyLambFlesh.WaitForTargetRoutine != null)
            enemyLambFlesh.StopCoroutine(enemyLambFlesh.WaitForTargetRoutine);
          enemyLambFlesh.WaitForTargetRoutine = enemyLambFlesh.StartCoroutine((IEnumerator) enemyLambFlesh.WaitForTarget());
        }
        if (!enemyLambFlesh.merging)
        {
          if (enemyLambFlesh.state.CURRENT_STATE == StateMachine.State.Idle && enemyLambFlesh.Spine.AnimationName != "idle")
            enemyLambFlesh.Spine.AnimationState.SetAnimation(0, "idle", true);
          if (enemyLambFlesh.state.CURRENT_STATE == StateMachine.State.Moving && enemyLambFlesh.MyState != EnemyLambFlesh.State.Attacking && enemyLambFlesh.Spine.AnimationName != "move")
            enemyLambFlesh.Spine.AnimationState.SetAnimation(0, "move", true);
        }
        enemyLambFlesh.state.facingAngle = enemyLambFlesh.Angle;
        enemyLambFlesh.state.LookAngle = enemyLambFlesh.Angle;
        yield return (object) null;
      }
    }
    yield return (object) null;
  }

  public void SpineEventMove()
  {
    if (string.IsNullOrEmpty(this.CrawlSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.CrawlSFX);
  }

  public void OnTriggerEnterEvent(Collider2D collision)
  {
    if (this.spawning || this.merging || this.MyState == EnemyLambFlesh.State.Dodging)
      return;
    Health component = collision.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) collision.gameObject != (UnityEngine.Object) this.gameObject) || component.team == this.health.team || component.team == Health.Team.Neutral)
      return;
    this.SimpleSpineFlash.FlashWhite(this.state.Timer / this.attackDelay);
    this.Spine.AnimationState.SetAnimation(0, this.SignPostAttackAnimation, false);
    this.StartCoroutine((IEnumerator) this.EnableDamageCollider(this.attackDelay));
    this.StartCoroutine((IEnumerator) this.DisableTriggerCollider(this.AttackCoolDown));
  }

  public IEnumerator DisableTriggerCollider(float cooldown)
  {
    if ((bool) (UnityEngine.Object) this.triggerColliderEvents)
    {
      this.triggerColliderEvents.SetActive(false);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) cooldown)
        yield return (object) null;
      this.triggerColliderEvents.SetActive(true);
    }
  }

  public IEnumerator EnableDamageCollider(float initialDelay)
  {
    this.MyState = EnemyLambFlesh.State.Attacking;
    if ((bool) (UnityEngine.Object) this.damageColliderEvents)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) initialDelay)
        yield return (object) null;
      this.Spine.AnimationState.SetAnimation(0, this.AttackAnimation, false);
      this.damageColliderEvents.SetActive(true);
      if (!string.IsNullOrEmpty(this.AttackSpikesSFX))
        AudioManager.Instance.PlayOneShot(this.AttackSpikesSFX);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * this.Spine.timeScale) < (double) this.attackDuration)
        yield return (object) null;
      this.damageColliderEvents.SetActive(false);
      this.MyState = EnemyLambFlesh.State.Idle;
    }
  }

  public IEnumerator WaitForTarget()
  {
    EnemyLambFlesh enemyLambFlesh = this;
    enemyLambFlesh.health.untouchable = false;
    enemyLambFlesh.health.invincible = false;
    while (!GameManager.RoomActive)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    enemyLambFlesh.ClearPaths();
    while ((UnityEngine.Object) enemyLambFlesh.targetObject == (UnityEngine.Object) null || (UnityEngine.Object) enemyLambFlesh.mergeTarget == (UnityEngine.Object) null)
    {
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyLambFlesh.Spine.timeScale) < (double) enemyLambFlesh.lookForTargetInterval)
        yield return (object) null;
      UnitObject newTarget = enemyLambFlesh.GetNewTarget();
      if ((UnityEngine.Object) newTarget != (UnityEngine.Object) null)
      {
        enemyLambFlesh.mergeTarget = newTarget as EnemyLambFlesh;
        enemyLambFlesh.targetObject = newTarget.gameObject;
      }
      else
      {
        Health closestTarget = enemyLambFlesh.GetClosestTarget(enemyLambFlesh.health.team == Health.Team.PlayerTeam);
        if ((bool) (UnityEngine.Object) closestTarget)
          enemyLambFlesh.targetObject = closestTarget.gameObject;
        enemyLambFlesh.RemoveFleshlingFromList();
        enemyLambFlesh.TryAddFleshlingToList();
      }
      enemyLambFlesh.StartCoroutine((IEnumerator) enemyLambFlesh.MovementRoutine());
      yield return (object) null;
    }
    yield return (object) null;
  }

  public UnitObject GetNewTarget()
  {
    if (!GameManager.RoomActive || (double) EnemyLambFlesh.kebabAmount >= (double) this.maxKebabsAllowed || !this.canMerge)
      return (UnitObject) null;
    EnemyLambFlesh.EnemyPair enemyPair = EnemyLambFlesh.FleshlingPairs.FirstOrDefault<EnemyLambFlesh.EnemyPair>((Func<EnemyLambFlesh.EnemyPair, bool>) (x => x.Contains((UnitObject) this)));
    return enemyPair == null || !enemyPair.IsPairComplete ? (UnitObject) null : enemyPair.TryGetPairOf((UnitObject) this);
  }

  public void FindPath(Vector3 PointToCheck)
  {
    this.TargetPosition = PointToCheck;
    this.givePath(this.TargetPosition);
    this.Points.Add(PointToCheck + Vector3.Normalize(this.transform.position - PointToCheck));
    this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyLambFlesh enemyLambFlesh = this;
    enemyLambFlesh.DisableForces = true;
    enemyLambFlesh.Angle = Utils.GetAngle(Attacker.transform.position, enemyLambFlesh.transform.position) * ((float) Math.PI / 180f);
    enemyLambFlesh.Force = (Vector3) new Vector2(1000f * Mathf.Cos(enemyLambFlesh.Angle), 1000f * Mathf.Sin(enemyLambFlesh.Angle));
    enemyLambFlesh.rb.AddForce((Vector2) (enemyLambFlesh.Force * enemyLambFlesh.KnockbackForceModifier));
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyLambFlesh.Spine.timeScale) < (double) enemyLambFlesh.KnockbackDuration)
      yield return (object) null;
    enemyLambFlesh.DisableForces = false;
    enemyLambFlesh.rb.velocity = Vector2.zero;
    if (enemyLambFlesh.AttackAfterKnockback)
      enemyLambFlesh.AttackCoolDown = 0.0f;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.gameObject);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!(bool) (UnityEngine.Object) this.DeadBody)
      return;
    GameObject gameObject = this.DeadBody.gameObject.Spawn();
    gameObject.transform.position = this.transform.position;
    gameObject.GetComponent<DeadBodyFlying>().Init(Utils.GetAngle(AttackLocation, this.transform.position));
  }

  public override void OnDisable()
  {
    this.RemoveFleshlingFromList();
    if ((bool) (UnityEngine.Object) this.currentIndicator)
      this.currentIndicator.Recycle();
    base.OnDisable();
    EnemyLambFlesh.LambFleshEnemies.Remove(this);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.SetActive(false);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.triggerColliderEvents.SetActive(false);
    this.triggerColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
  }

  public void OnDrawGizmos()
  {
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], 0.1f, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], 0.1f, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
    Utils.DrawCircleXY(this.transform.position, 1f, Color.yellow);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team && (this.health.team != Health.Team.PlayerTeam || !this.health.IsCharmedEnemy))
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public bool ShouldMerge()
  {
    return this.canMerge && !this.merging && (UnityEngine.Object) this.mergeTarget != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, this.mergeTarget.transform.position) < (double) this.mergeDistance;
  }

  public void StartMerge(bool initiatedMerge)
  {
    this.StartCoroutine((IEnumerator) this.ShowWarning());
    this.merging = true;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StopAllCoroutines();
    if (initiatedMerge)
      this.mergeTarget.StartMerge(false);
    this.StartCoroutine((IEnumerator) this.MergeRoutine(initiatedMerge));
  }

  public IEnumerator MergeRoutine(bool initiatedMerge)
  {
    EnemyLambFlesh enemyLambFlesh = this;
    float Progress = 0.0f;
    float Duration = enemyLambFlesh.mergeDuration;
    enemyLambFlesh.Spine.AnimationState.SetAnimation(0, enemyLambFlesh.FuseSmallAnimation, true);
    while ((double) (Progress += Time.deltaTime) < (double) Duration / (double) enemyLambFlesh.Spine.timeScale)
    {
      enemyLambFlesh.SimpleSpineFlash.FlashWhite(Progress / Duration);
      yield return (object) null;
    }
    enemyLambFlesh.SimpleSpineFlash.FlashWhite(false);
    if (initiatedMerge)
    {
      EnemyKebab enemyKebab = ObjectPool.Spawn<EnemyKebab>(enemyLambFlesh.mergeResult, enemyLambFlesh.transform.parent, enemyLambFlesh.transform.position, Quaternion.identity);
      enemyKebab.transform.position = enemyLambFlesh.transform.position;
      enemyKebab.HasMerged = true;
      Health component = enemyKebab.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        Interaction_Chest.Instance?.AddEnemy(component);
    }
    yield return (object) new WaitForEndOfFrame();
    enemyLambFlesh.health.DealDamage(999f, enemyLambFlesh.gameObject, enemyLambFlesh.transform.position, true, Health.AttackTypes.NoKnockBack, true);
  }

  public void SpawnTween(Transform t, Vector3 destination, float duration)
  {
    this.StartCoroutine((IEnumerator) this.SpawnTweenRoutine(t, destination, duration));
  }

  public IEnumerator SpawnTweenRoutine(Transform t, Vector3 destination, float duration)
  {
    EnemyLambFlesh enemyLambFlesh = this;
    enemyLambFlesh.spawning = true;
    enemyLambFlesh.Spine.AnimationState.SetAnimation(0, "egg-spawn", false);
    enemyLambFlesh.health.untouchable = true;
    yield return (object) new WaitForEndOfFrame();
    float Progress = 0.0f;
    Vector3 StartPosition = enemyLambFlesh.transform.position;
    Vector3 TargetPosition = destination;
    while ((double) (Progress += Time.deltaTime * enemyLambFlesh.Spine.timeScale) < (double) duration)
    {
      t.position = Vector3.Lerp(StartPosition, TargetPosition, Progress / duration);
      yield return (object) null;
    }
    t.position = TargetPosition;
    enemyLambFlesh.health.untouchable = false;
    enemyLambFlesh.spawning = false;
    enemyLambFlesh.StartCoroutine((IEnumerator) enemyLambFlesh.MovementRoutine());
  }

  public IEnumerator ShowWarning()
  {
    this.warningIcon.gameObject.SetActive(true);
    yield return (object) this.warningIcon.YieldForAnimation("warn");
    this.warningIcon.gameObject.SetActive(false);
  }

  public void StopResilience() => this.canDodge = false;

  public void ResetResilience() => this.canDodge = true;

  [CompilerGenerated]
  public void \u003CLoadAssets\u003Eb__58_0(AsyncOperationHandle<GameObject> obj)
  {
    EnemyLambFlesh.loadedAddressableAssets.Add(obj);
    this.loadedIndicator = obj.Result;
    this.loadedIndicator.CreatePool(10, true);
  }

  [CompilerGenerated]
  public bool \u003CTryAddFleshlingToList\u003Eb__61_0(EnemyLambFlesh.EnemyPair x)
  {
    return x.Contains((UnitObject) this);
  }

  [CompilerGenerated]
  public bool \u003CRemoveFleshlingFromList\u003Eb__62_0(EnemyLambFlesh.EnemyPair x)
  {
    return x.Contains((UnitObject) this);
  }

  [CompilerGenerated]
  public bool \u003CGetNewTarget\u003Eb__84_0(EnemyLambFlesh.EnemyPair x)
  {
    return x.Contains((UnitObject) this);
  }

  public enum State
  {
    Attacking,
    Idle,
    Dodging,
  }

  public class EnemyPair
  {
    public UnitObject enemyA;
    public UnitObject enemyB;

    public EnemyPair(UnitObject unit) => this.enemyA = unit;

    public bool IsPairComplete
    {
      get => (UnityEngine.Object) this.enemyA != (UnityEngine.Object) null && (UnityEngine.Object) this.enemyB != (UnityEngine.Object) null;
    }

    public bool IsPairEmpty
    {
      get => (UnityEngine.Object) this.enemyA == (UnityEngine.Object) null && (UnityEngine.Object) this.enemyB == (UnityEngine.Object) null;
    }

    public bool Contains(UnitObject unit)
    {
      if (this.IsPairEmpty)
        return false;
      return (UnityEngine.Object) this.enemyA == (UnityEngine.Object) unit || (UnityEngine.Object) this.enemyB == (UnityEngine.Object) unit;
    }

    public UnitObject TryGetPairOf(UnitObject unit)
    {
      if (!this.Contains(unit))
        return (UnitObject) null;
      return !((UnityEngine.Object) this.enemyA == (UnityEngine.Object) unit) ? this.enemyA : this.enemyB;
    }

    public bool AddToPair(UnitObject unit)
    {
      if ((UnityEngine.Object) this.enemyA == (UnityEngine.Object) null)
      {
        this.enemyA = unit;
        return true;
      }
      if (!((UnityEngine.Object) this.enemyB == (UnityEngine.Object) null))
        return false;
      this.enemyB = unit;
      return true;
    }

    public bool RemoveFromPair(UnitObject unit)
    {
      if ((UnityEngine.Object) this.enemyA == (UnityEngine.Object) unit)
      {
        this.enemyA = (UnitObject) null;
        return true;
      }
      if (!((UnityEngine.Object) this.enemyB == (UnityEngine.Object) unit))
        return false;
      this.enemyB = (UnitObject) null;
      return true;
    }
  }
}
