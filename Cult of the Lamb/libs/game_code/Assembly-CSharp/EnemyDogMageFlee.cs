// Decompiled with JetBrains decompiler
// Type: EnemyDogMageFlee
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WebSocketSharp;

#nullable disable
public class EnemyDogMageFlee : UnitObject
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
  public AssetReferenceGameObject[] EnemyList;
  [SerializeField]
  public bool isSpawnablesIncreasingDamageMultiplier = true;
  [SerializeField]
  public GameObject trapAvalanche;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string SummonSfx = "event:/enemy/summon";
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public float FleeDistance = 1f;
  [Tooltip("If a player is within this radius, flee!")]
  public float FleeRadius = 2.5f;
  public float FleeDelay = 3f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  public float MinionRiseHeight = 2f;
  public float MinionRiseDuration = 2f;
  public float PauseBeforeThrow = 0.5f;
  public float DelayBetweenMinionThrow = 0.5f;
  [UnityEngine.Range(0.0f, 1f)]
  public float ChanceOfThrowAll = 0.3f;
  public float SignPostCloseCombatDelay = 1f;
  public ColliderEvents DamageColliderEvents;
  public int MeleeAttackCount = 2;
  public float DelayBetweenMeleeAttacks = 0.5f;
  public int NumToSpawn;
  [SerializeField]
  public int numAvalanchesLine;
  [SerializeField]
  public float delayAttackLine;
  [SerializeField]
  public float timeBetweenAvalanchesLine;
  [SerializeField]
  public float delayAvalancheLine;
  [SerializeField]
  public float speedAvalancheLine;
  [SerializeField]
  public float distanceActivationAttackCircle;
  [SerializeField]
  public int numAvalanchesCircle;
  [SerializeField]
  public float distanceAvalanchesCircle;
  [SerializeField]
  public float delayAttackCircle;
  [SerializeField]
  public float timeBetweenAvalanchesCircle;
  [SerializeField]
  public float delayAvalancheCircle;
  [SerializeField]
  public float speedAvalancheCircle;
  public float fleeDelay;
  public GameObject EnemySpawnerGO;
  public const float SPAWN_DISTANCE_MIN = 1f;
  public const float SPAWN_DISTANCE_MAX = 4f;
  public List<UnitObject> minions = new List<UnitObject>();
  public bool Stunned;
  public bool isThrowing;
  public bool Teleporting;
  public Coroutine cTeleporting;
  public int SummonedCount;
  public bool hasSummoned;
  public float ThrowDelay = 1f;
  public float CloseCombatCooldown = 1f;
  public float SummonDelay = 1f;
  public float meleeDistance = 2f;
  public bool canBeParried;
  public float signPostParryWindow = 0.2f;
  public float attackParryWindow = 0.15f;
  public float StartSpeed = 0.4f;
  public string LoopedSoundSFX;
  public EventInstance LoopedSound;

  public void Start()
  {
    this.SeperateObject = true;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.CircleCollider = this.GetComponent<CircleCollider2D>();
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "Fireball":
        if (!string.IsNullOrEmpty(this.AttackVO))
          AudioManager.Instance.PlayOneShot(this.AttackVO, this.transform.position);
        Projectile component = ObjectPool.Spawn(this.Arrow, this.transform.parent).GetComponent<Projectile>();
        component.transform.position = this.transform.position;
        if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
          this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
        component.Angle = Mathf.Round(this.state.facingAngle / 45f) * 45f;
        component.team = this.health.team;
        component.Owner = this.health;
        break;
    }
  }

  public override void OnEnable()
  {
    if ((UnityEngine.Object) this.DamageColliderEvents != (UnityEngine.Object) null)
    {
      this.DamageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.DamageColliderEvents.SetActive(false);
    }
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    base.OnEnable();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.ClearPaths();
    this.StopAllCoroutines();
    if (!this.LoopedSoundSFX.IsNullOrEmpty())
      AudioManager.Instance.StopLoop(this.LoopedSound);
    if (!((UnityEngine.Object) this.DamageColliderEvents != (UnityEngine.Object) null))
      return;
    this.DamageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.LoopedSoundSFX.IsNullOrEmpty())
      return;
    AudioManager.Instance.StopLoop(this.LoopedSound);
  }

  public IEnumerator WaitForTarget()
  {
    EnemyDogMageFlee enemyDogMageFlee = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    if (!enemyDogMageFlee.LoopedSoundSFX.IsNullOrEmpty())
      enemyDogMageFlee.LoopedSound = AudioManager.Instance.CreateLoop(enemyDogMageFlee.LoopedSoundSFX, enemyDogMageFlee.skeletonAnimation.gameObject, true);
    while ((double) PlayerFarming.GetClosestPlayerDist(enemyDogMageFlee.transform.position) > (double) enemyDogMageFlee.Range)
      yield return (object) null;
    enemyDogMageFlee.StopAllCoroutines();
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.ChasePlayer());
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
      if (this.Melee && !this.Stunned)
      {
        int num = this.isThrowing ? 1 : 0;
      }
      CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
      BiomeConstants.Instance.EmitHitVFX(AttackLocation + Vector3.back * 1f, Quaternion.identity.z, "HitFX_Weak");
      this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.simpleSpineFlash.FlashFillRed();
      this.state.facingAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
    }
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.LoopedSoundSFX.IsNullOrEmpty())
      AudioManager.Instance.StopLoop(this.LoopedSound);
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
  }

  public void KillAllMinions()
  {
    foreach (UnitObject minion in this.minions)
    {
      if ((double) minion.health.HP > 0.0)
      {
        minion.health.invincible = false;
        minion.health.DealDamage(float.PositiveInfinity, PlayerFarming.Instance.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
      }
    }
  }

  public IEnumerator ChasePlayer()
  {
    EnemyDogMageFlee enemyDogMageFlee = this;
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Idle;
    bool loop = true;
    while (loop)
    {
      enemyDogMageFlee.TargetObject = PlayerFarming.FindClosestPlayer(enemyDogMageFlee.transform.position).gameObject;
      enemyDogMageFlee.state.facingAngle = Utils.GetAngle(enemyDogMageFlee.transform.position, enemyDogMageFlee.TargetObject.transform.position);
      float num = Vector3.Distance(enemyDogMageFlee.TargetObject.transform.position, enemyDogMageFlee.transform.position);
      enemyDogMageFlee.fleeDelay -= Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale;
      if ((double) enemyDogMageFlee.fleeDelay < 0.0 && (double) num < (double) enemyDogMageFlee.FleeRadius || (double) enemyDogMageFlee.fleeDelay < 0.0)
      {
        if ((double) enemyDogMageFlee.fleeDelay < 0.0 && (double) num < (double) enemyDogMageFlee.FleeRadius)
          enemyDogMageFlee.ThrowDelay = UnityEngine.Random.Range(0.0f, 1f);
        enemyDogMageFlee.fleeDelay = enemyDogMageFlee.FleeDelay;
        yield return (object) enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.DoFlee());
      }
      if (enemyDogMageFlee.Summon && (!enemyDogMageFlee.hasSummoned || enemyDogMageFlee.GetAliveMinions() <= 0) && (double) (enemyDogMageFlee.SummonDelay -= Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale) < 0.0)
      {
        enemyDogMageFlee.StopAllCoroutines();
        yield return (object) enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.DoSummon());
      }
      if (enemyDogMageFlee.Melee && (double) (enemyDogMageFlee.CloseCombatCooldown -= Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale) < 0.0 && (double) Vector3.Distance(enemyDogMageFlee.transform.position, enemyDogMageFlee.TargetObject.transform.position) < (double) enemyDogMageFlee.distanceActivationAttackCircle)
      {
        enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.DoCloseCombatAttack());
        break;
      }
      if (enemyDogMageFlee.Throw && enemyDogMageFlee.hasSummoned && (double) (enemyDogMageFlee.ThrowDelay -= Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale) < 0.0 && enemyDogMageFlee.GetAliveMinions() > 0)
      {
        enemyDogMageFlee.StopAllCoroutines();
        enemyDogMageFlee.ThrowDelay = UnityEngine.Random.Range(2f, 3f);
        if ((double) UnityEngine.Random.value < (double) enemyDogMageFlee.ChanceOfThrowAll)
        {
          enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.DoThrowAllMinionsAttack());
          break;
        }
        enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.DoThrowSingleMinionAttack());
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
    EnemyDogMageFlee enemyDogMageFlee = this;
    enemyDogMageFlee.ClearPaths();
    enemyDogMageFlee.KillAllMinions();
    enemyDogMageFlee.minions.Clear();
    int SpawnCount = enemyDogMageFlee.numAvalanchesLine;
    if (!string.IsNullOrEmpty(enemyDogMageFlee.WarningVO))
      AudioManager.Instance.PlayOneShot(enemyDogMageFlee.WarningVO, enemyDogMageFlee.transform.position);
    enemyDogMageFlee.summonParticles.startDelay = 1.5f;
    enemyDogMageFlee.summonParticles.Play();
    enemyDogMageFlee.skeletonAnimation.AnimationState.SetAnimation(0, "summon", false);
    enemyDogMageFlee.skeletonAnimation.AnimationState.AddAnimation(0, enemyDogMageFlee.idleAnimation, true, 0.0f);
    Vector3 dir = (enemyDogMageFlee.TargetObject.transform.position - enemyDogMageFlee.transform.position).normalized;
    float distance = 7f;
    Vector3 originPosition = enemyDogMageFlee.transform.position;
    yield return (object) new WaitForSeconds(enemyDogMageFlee.delayAttackLine);
    while (SpawnCount > 0)
    {
      Vector3 position = originPosition + dir * (float) (enemyDogMageFlee.numAvalanchesLine - SpawnCount + 1) * (distance / (float) enemyDogMageFlee.numAvalanchesLine);
      TrapAvalanche component = ObjectPool.Spawn(enemyDogMageFlee.trapAvalanche, position, Quaternion.identity).GetComponent<TrapAvalanche>();
      component.DropDelay = enemyDogMageFlee.delayAvalancheLine;
      component.DropSpeed = enemyDogMageFlee.speedAvalancheLine;
      component.Drop();
      ++enemyDogMageFlee.SummonedCount;
      enemyDogMageFlee.EnemySpawnerGO = (GameObject) null;
      --SpawnCount;
      yield return (object) new WaitForSeconds(enemyDogMageFlee.timeBetweenAvalanchesLine);
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale) < 1.6000000238418579)
      yield return (object) null;
    enemyDogMageFlee.hasSummoned = true;
    enemyDogMageFlee.SummonDelay = 1f;
    enemyDogMageFlee.StopAllCoroutines();
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.ChasePlayer());
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public Vector3 GetRandomDirection()
  {
    double f = (double) UnityEngine.Random.Range(0.0f, 6.28318548f);
    return new Vector3(Mathf.Cos((float) f), Mathf.Sin((float) f), 0.0f);
  }

  public IEnumerator DoStunned()
  {
    EnemyDogMageFlee enemyDogMageFlee = this;
    enemyDogMageFlee.Stunned = true;
    enemyDogMageFlee.health.ArrowAttackVulnerability = 1f;
    enemyDogMageFlee.health.MeleeAttackVulnerability = 1f;
    enemyDogMageFlee.CircleCollider.enabled = true;
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemyDogMageFlee.skeletonAnimation.AnimationState.SetAnimation(0, "stunned", true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale) < 2.0)
      yield return (object) null;
    enemyDogMageFlee.Stunned = false;
    enemyDogMageFlee.health.ArrowAttackVulnerability = 1f;
    enemyDogMageFlee.StopAllCoroutines();
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.ChasePlayer());
  }

  public IEnumerator DoThrowSingleMinionAttack()
  {
    EnemyDogMageFlee enemyDogMageFlee = this;
    UnitObject minion = enemyDogMageFlee.GetRandomMinion();
    enemyDogMageFlee.isThrowing = true;
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.DoNecromancerRiseAnim());
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
    {
      SkeletonAnimation componentInChildren = minion.GetComponentInChildren<SkeletonAnimation>(true);
      Health minionHealth = minion.GetComponent<Health>();
      enemyDogMageFlee.DisableMinionForThrowing(minion, minionHealth);
      yield return (object) enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.RaiseMinionIntoAirForThrowing(minion, componentInChildren));
      yield return (object) new WaitForSeconds(enemyDogMageFlee.PauseBeforeThrow);
      yield return (object) enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.MoveMinionTowardsTarget(minion, minionHealth));
      minionHealth = (Health) null;
    }
    enemyDogMageFlee.simpleSpineFlash.FlashWhite(false);
    enemyDogMageFlee.isThrowing = false;
    enemyDogMageFlee.StopAllCoroutines();
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.ChasePlayer());
  }

  public IEnumerator DoFlee()
  {
    EnemyDogMageFlee enemyDogMageFlee = this;
    Vector3 fleePosition = Vector3.zero;
    if ((UnityEngine.Object) enemyDogMageFlee.DamageColliderEvents != (UnityEngine.Object) null)
      enemyDogMageFlee.DamageColliderEvents.SetActive(false);
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) (((double) Utils.GetAngle(enemyDogMageFlee.transform.position, enemyDogMageFlee.TargetObject.transform.position) + (double) UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
      float radius = 1f;
      Vector3 vector3 = enemyDogMageFlee.TargetObject.transform.position + new Vector3(enemyDogMageFlee.FleeDistance * Mathf.Cos(f), enemyDogMageFlee.FleeDistance * Mathf.Sin(f));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) enemyDogMageFlee.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - enemyDogMageFlee.transform.position), enemyDogMageFlee.FleeDistance, (int) enemyDogMageFlee.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if (enemyDogMageFlee.ShowDebug)
        {
          enemyDogMageFlee.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(enemyDogMageFlee.transform.position - vector3) * enemyDogMageFlee.CircleCastOffset);
          enemyDogMageFlee.PointsLink.Add(new Vector3(enemyDogMageFlee.transform.position.x, enemyDogMageFlee.transform.position.y));
        }
        fleePosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(enemyDogMageFlee.transform.position - vector3) * enemyDogMageFlee.CircleCastOffset;
        break;
      }
    }
    enemyDogMageFlee.ClearPaths();
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Fleeing;
    enemyDogMageFlee.health.invincible = true;
    enemyDogMageFlee.SeperateObject = false;
    enemyDogMageFlee.ClearPaths();
    Vector3 startPosition = enemyDogMageFlee.transform.position;
    float progress = 0.0f;
    enemyDogMageFlee.state.facingAngle = enemyDogMageFlee.state.LookAngle = Utils.GetAngle(enemyDogMageFlee.transform.position, fleePosition);
    enemyDogMageFlee.skeletonAnimation.skeleton.ScaleX = (double) enemyDogMageFlee.state.LookAngle <= 90.0 || (double) enemyDogMageFlee.state.LookAngle >= 270.0 ? -1f : 1f;
    float Duration = Vector3.Distance(startPosition, fleePosition) / 12f;
    while ((double) (progress += Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale) < (double) Duration)
    {
      enemyDogMageFlee.transform.position = Vector3.Lerp(startPosition, fleePosition, Mathf.SmoothStep(0.0f, 1f, progress / Duration));
      yield return (object) null;
    }
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Idle;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyDogMageFlee.SeperateObject = true;
    enemyDogMageFlee.health.invincible = false;
    enemyDogMageFlee.fleeDelay = enemyDogMageFlee.FleeDelay;
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDogMageFlee.StopAllCoroutines();
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.ChasePlayer());
  }

  public IEnumerator DoCloseCombatAttack()
  {
    EnemyDogMageFlee enemyDogMageFlee = this;
    enemyDogMageFlee.ClearPaths();
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemyDogMageFlee.skeletonAnimation.AnimationState.SetAnimation(0, "summon", false);
    enemyDogMageFlee.skeletonAnimation.AnimationState.AddAnimation(0, enemyDogMageFlee.idleAnimation, true, 0.0f);
    enemyDogMageFlee.state.facingAngle = enemyDogMageFlee.state.LookAngle = Utils.GetAngle(enemyDogMageFlee.transform.position, enemyDogMageFlee.TargetObject.transform.position);
    enemyDogMageFlee.skeletonAnimation.Skeleton.ScaleX = (double) enemyDogMageFlee.state.LookAngle <= 90.0 || (double) enemyDogMageFlee.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 dir = (enemyDogMageFlee.TargetObject.transform.position - enemyDogMageFlee.transform.position).normalized;
    yield return (object) new WaitForSeconds(enemyDogMageFlee.delayAttackCircle);
    for (int i = 0; i < enemyDogMageFlee.numAvalanchesCircle; ++i)
    {
      Vector3 position1 = enemyDogMageFlee.transform.position + Quaternion.AngleAxis((float) (90 / enemyDogMageFlee.numAvalanchesCircle) + 180f / (float) enemyDogMageFlee.numAvalanchesCircle * (float) i, Vector3.forward) * (dir * enemyDogMageFlee.distanceAvalanchesCircle);
      Vector3 position2 = enemyDogMageFlee.transform.position + Quaternion.AngleAxis((float) -(90 / enemyDogMageFlee.numAvalanchesCircle) - 180f / (float) enemyDogMageFlee.numAvalanchesCircle * (float) i, Vector3.forward) * (dir * enemyDogMageFlee.distanceAvalanchesCircle);
      TrapAvalanche component1 = ObjectPool.Spawn(enemyDogMageFlee.trapAvalanche, position1, Quaternion.identity).GetComponent<TrapAvalanche>();
      component1.DropDelay = enemyDogMageFlee.delayAvalancheCircle;
      component1.DropSpeed = enemyDogMageFlee.speedAvalancheCircle;
      component1.Drop();
      TrapAvalanche component2 = ObjectPool.Spawn(enemyDogMageFlee.trapAvalanche, position2, Quaternion.identity).GetComponent<TrapAvalanche>();
      component2.DropDelay = enemyDogMageFlee.delayAvalancheCircle;
      component2.DropSpeed = enemyDogMageFlee.speedAvalancheCircle;
      component2.Drop();
      yield return (object) new WaitForSeconds(enemyDogMageFlee.timeBetweenAvalanchesCircle);
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyDogMageFlee.skeletonAnimation.timeScale) < 2.5999999046325684)
      yield return (object) null;
    enemyDogMageFlee.CloseCombatCooldown = 1f;
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyDogMageFlee.fleeDelay = 0.0f;
    enemyDogMageFlee.StopAllCoroutines();
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.ChasePlayer());
  }

  public IEnumerator DoThrowAllMinionsAttack()
  {
    EnemyDogMageFlee enemyDogMageFlee = this;
    List<UnitObject> minions = enemyDogMageFlee.GetAllAvailableMinions();
    enemyDogMageFlee.isThrowing = true;
    enemyDogMageFlee.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.DoNecromancerRiseAnim());
    if (minions.Count > 0)
    {
      foreach (UnitObject minion in minions)
      {
        SkeletonAnimation componentInChildren = minion.GetComponentInChildren<SkeletonAnimation>(true);
        Health component = minion.GetComponent<Health>();
        enemyDogMageFlee.DisableMinionForThrowing(minion, component);
        enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.RaiseMinionIntoAirForThrowing(minion, componentInChildren));
      }
      yield return (object) new WaitForSeconds(enemyDogMageFlee.MinionRiseDuration);
      yield return (object) new WaitForSeconds(enemyDogMageFlee.PauseBeforeThrow);
      for (int i = 0; i < minions.Count; ++i)
      {
        Health component = minions[i].GetComponent<Health>();
        yield return (object) enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.MoveMinionTowardsTarget(minions[i], component));
        yield return (object) new WaitForSeconds(enemyDogMageFlee.DelayBetweenMinionThrow);
      }
    }
    enemyDogMageFlee.simpleSpineFlash.FlashWhite(false);
    enemyDogMageFlee.isThrowing = false;
    enemyDogMageFlee.StopAllCoroutines();
    enemyDogMageFlee.StartCoroutine((IEnumerator) enemyDogMageFlee.ChasePlayer());
  }

  public IEnumerator DoNecromancerRiseAnim()
  {
    this.skeletonAnimation.AnimationState.SetAnimation(0, "summon", false);
    this.skeletonAnimation.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime * this.skeletonAnimation.timeScale) < 1.3500000238418579)
    {
      this.simpleSpineFlash.FlashWhite(Timer / 1.35f);
      yield return (object) null;
    }
  }

  public IEnumerator MoveMinionTowardsTarget(UnitObject minion, Health minionHealth)
  {
    Vector3 position = this.GetClosestTarget().transform.position;
    float num1 = 10f;
    float num2 = Vector3.Distance(minion.transform.position, position) / num1;
    minion.transform.DOMove(position, num2).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(num2);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.5f);
    BiomeConstants.Instance.EmitHammerEffects(minion.transform.position);
    minionHealth.invincible = false;
    minionHealth.DealDamage(float.PositiveInfinity, PlayerFarming.Instance.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
  }

  public IEnumerator RaiseMinionIntoAirForThrowing(UnitObject minion, SkeletonAnimation minionSpine)
  {
    minion.transform.DOKill();
    minion.transform.DOMoveZ(-this.MinionRiseHeight, this.MinionRiseDuration * minionSpine.timeScale).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(this.MinionRiseDuration * minionSpine.timeScale);
  }

  public void DisableMinionForThrowing(UnitObject minion, Health minionHealth)
  {
    minion.state.CURRENT_STATE = StateMachine.State.Idle;
    minion.enabled = false;
    minionHealth.invincible = true;
  }

  public Health GetClosestTarget() => this.GetClosestTarget(true);

  public UnitObject GetRandomMinion()
  {
    this.minions.Shuffle<UnitObject>();
    foreach (UnitObject minion in this.minions)
    {
      if ((double) minion.health.HP > 0.0 && minion.health.team == Health.Team.Team2)
        return minion;
    }
    return (UnitObject) null;
  }

  public List<UnitObject> GetAllAvailableMinions()
  {
    List<UnitObject> availableMinions = new List<UnitObject>();
    foreach (UnitObject minion in this.minions)
    {
      if ((double) minion.health.HP > 0.0 && minion.health.team == Health.Team.Team2)
        availableMinions.Add(minion);
    }
    return availableMinions;
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
}
