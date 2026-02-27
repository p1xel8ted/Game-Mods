// Decompiled with JetBrains decompiler
// Type: EnemyFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using Pathfinding;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyFollower : UnitObject
{
  public int NewPositionDistance = 3;
  public float MaintainTargetDistance = 4.5f;
  public float MoveCloserDistance = 4f;
  public float AttackWithinRange = 4f;
  public bool DoubleAttack = true;
  public bool ChargeAndAttack = true;
  public Vector2 MaxAttackDelayRandomRange = new Vector2(4f, 6f);
  public Vector2 AttackDelayRandomRange = new Vector2(0.5f, 2f);
  public ColliderEvents damageColliderEvents;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public float TeleportDelayTarget = 1f;
  public bool CanShoot;
  public int ShotsToFire = 1;
  public float DelayBetweenShots = 0.2f;
  public float DelayReaiming = 0.5f;
  public GameObject Arrow;
  public SpriteRenderer Aiming;
  public bool CanSpawnEnemies;
  public AssetReferenceGameObject[] Spawnables;
  public int MaxEnemies = 2;
  public float TimeBetweenEnemies = 3f;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string shootSoundPath = string.Empty;
  public SimpleSpineAnimator simpleSpineAnimator;
  public GameObject TargetObject;
  public float AttackDelay;
  public bool canBeParried;
  public static float signPostParryWindow = 0.2f;
  public static float attackParryWindow = 0.15f;
  public float timeBetweenShooting;
  public float enemiesSpawningProgress;
  public float followerTimestamp;
  [CompilerGenerated]
  public float \u003CDamage\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public bool \u003CVanishOnRoomComplete\u003Ek__BackingField;
  public int variant;
  public Coroutine artificialUpdate;
  public List<Health> spawnedEnemies = new List<Health>();
  public Vector3 Force;
  [HideInInspector]
  public float Angle;
  public Vector3 TargetPosition;
  public float RepathTimer;
  public float TeleportDelay;
  public EnemyFollower.State MyState;
  public float MaxAttackDelay;
  public Health EnemyHealth;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public float Damage
  {
    get => this.\u003CDamage\u003Ek__BackingField;
    set => this.\u003CDamage\u003Ek__BackingField = value;
  }

  public bool VanishOnRoomComplete
  {
    get => this.\u003CVanishOnRoomComplete\u003Ek__BackingField;
    set => this.\u003CVanishOnRoomComplete\u003Ek__BackingField = value;
  }

  public void Start()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    this.artificialUpdate = GameManager.GetInstance().StartCoroutine(this.ArtificialUpdate());
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public override void Update()
  {
    base.Update();
    this.enemiesSpawningProgress += Time.deltaTime;
    if (!this.CanSpawnEnemies || (double) this.enemiesSpawningProgress <= (double) this.TimeBetweenEnemies || Health.team2.Count - 1 >= this.MaxEnemies)
      return;
    this.enemiesSpawningProgress = 0.0f;
    int index = 0;
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_2:
        index = 1;
        break;
      case FollowerLocation.Dungeon1_3:
        index = 2;
        break;
      case FollowerLocation.Dungeon1_4:
        index = 3;
        break;
    }
    Addressables.LoadAssetAsync<GameObject>((object) this.Spawnables[index]).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      UnitObject component1 = EnemySpawner.Create(BiomeGenerator.GetRandomPositionInIsland(), this.transform.parent, obj.Result).GetComponent<UnitObject>();
      component1.CanHaveModifier = false;
      component1.RemoveModifier();
      DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
      if ((bool) (UnityEngine.Object) component2)
        component2.GiveXP = false;
      this.spawnedEnemies.Add(component1.health);
    });
  }

  public override void OnEnable()
  {
    this.SeperateObject = true;
    base.OnEnable();
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    this.health.OnDie += new Health.DieAction(this.Health_OnDie);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.StartCoroutine(this.WaitForTarget());
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
    this.timeBetweenShooting = Time.time + UnityEngine.Random.Range(3f, 6f);
  }

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.StopAllCoroutines();
    this.StopCoroutine(this.artificialUpdate);
    for (int index = this.spawnedEnemies.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) null && (UnityEngine.Object) this.spawnedEnemies[index] != (UnityEngine.Object) this.health)
      {
        this.spawnedEnemies[index].invincible = false;
        this.spawnedEnemies[index].DealDamage(this.spawnedEnemies[index].HP, this.gameObject, this.spawnedEnemies[index].transform.position, dealDamageImmediately: true);
      }
    }
  }

  public override void OnDisable()
  {
    this.health.invincible = false;
    this.SimpleSpineFlash.FlashWhite(false);
    base.OnDisable();
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
    this.health.OnDie -= new Health.DieAction(this.Health_OnDie);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.ClearPaths();
    this.StopAllCoroutines();
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public IEnumerator ArtificialUpdate()
  {
    EnemyFollower enemyFollower = this;
    while (true)
    {
      enemyFollower.gameObject.SetActive((UnityEngine.Object) RespawnRoomManager.Instance == (UnityEngine.Object) null || !RespawnRoomManager.Instance.gameObject.activeSelf);
      yield return (object) null;
    }
  }

  public new void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!PlayerController.CanParryAttacks || !this.canBeParried || FromBehind || AttackType != Health.AttackTypes.Melee)
      return;
    this.health.WasJustParried = true;
    this.SimpleSpineFlash.FlashWhite(false);
    this.SeperateObject = true;
    this.UsePathing = true;
    this.health.invincible = false;
    this.StopAllCoroutines();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (this.health.HasShield || this.health.WasJustParried)
      return;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    this.SimpleSpineFlash.FlashFillRed();
  }

  public virtual IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyFollower enemyFollower = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyFollower.DisableForces = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyFollower.DisableForces = true;
    enemyFollower.Force = (enemyFollower.transform.position - Attacker.transform.position).normalized * 500f;
    enemyFollower.rb.AddForce((Vector2) enemyFollower.Force);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator WaitForTarget()
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.Spine.Initialize(false);
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyFollower.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
    while (!GameManager.RoomActive)
      yield return (object) null;
    while ((UnityEngine.Object) enemyFollower.TargetObject == (UnityEngine.Object) null)
    {
      PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(enemyFollower.transform.position);
      if ((UnityEngine.Object) enemyFollower.GetClosestTarget() != (UnityEngine.Object) null)
        enemyFollower.TargetObject = enemyFollower.GetClosestTarget().gameObject;
      else if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null && (double) Vector3.Distance(enemyFollower.transform.position, closestPlayer.transform.position) > 2.0 && (double) Time.time > (double) enemyFollower.followerTimestamp && PathUtilities.IsPathPossible(AstarPath.active.GetNearest(enemyFollower.transform.position).node, AstarPath.active.GetNearest(closestPlayer.transform.position).node))
      {
        enemyFollower.givePath(closestPlayer.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
        enemyFollower.followerTimestamp = Time.time + 0.25f;
      }
      yield return (object) null;
    }
    bool InRange = false;
    while (!InRange)
    {
      if ((UnityEngine.Object) enemyFollower.TargetObject == (UnityEngine.Object) null)
      {
        enemyFollower.StartCoroutine(enemyFollower.WaitForTarget());
        yield return (object) null;
      }
      else
      {
        if ((double) Vector3.Distance(enemyFollower.TargetObject.transform.position, enemyFollower.transform.position) <= (double) enemyFollower.VisionRange)
          InRange = true;
        yield return (object) null;
      }
    }
    enemyFollower.StartCoroutine(enemyFollower.ChasePlayer());
  }

  public IEnumerator ChasePlayer()
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.MyState = EnemyFollower.State.WaitAndTaunt;
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyFollower.AttackDelay = UnityEngine.Random.Range(enemyFollower.AttackDelayRandomRange.x, enemyFollower.AttackDelayRandomRange.y);
    if (enemyFollower.health.HasShield)
      enemyFollower.AttackDelay = 2.5f;
    enemyFollower.MaxAttackDelay = UnityEngine.Random.Range(enemyFollower.MaxAttackDelayRandomRange.x, enemyFollower.MaxAttackDelayRandomRange.y);
    bool Loop = true;
    while (Loop)
    {
      if ((UnityEngine.Object) enemyFollower.TargetObject == (UnityEngine.Object) null || !enemyFollower.TargetObject.activeInHierarchy)
      {
        if ((UnityEngine.Object) enemyFollower.GetClosestTarget() != (UnityEngine.Object) null)
        {
          enemyFollower.TargetObject = enemyFollower.GetClosestTarget().gameObject;
        }
        else
        {
          PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(enemyFollower.transform.position);
          if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null && (double) Vector3.Distance(enemyFollower.transform.position, closestPlayer.transform.position) > 2.0 && (double) Time.time > (double) enemyFollower.followerTimestamp && PathUtilities.IsPathPossible(AstarPath.active.GetNearest(enemyFollower.transform.position).node, AstarPath.active.GetNearest(closestPlayer.transform.position).node))
          {
            enemyFollower.givePath(closestPlayer.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
            enemyFollower.followerTimestamp = Time.time + 0.25f;
          }
          yield return (object) null;
          continue;
        }
      }
      if ((UnityEngine.Object) enemyFollower.damageColliderEvents != (UnityEngine.Object) null)
        enemyFollower.damageColliderEvents.SetActive(false);
      enemyFollower.TeleportDelay -= Time.deltaTime;
      enemyFollower.AttackDelay -= Time.deltaTime;
      enemyFollower.MaxAttackDelay -= Time.deltaTime;
      if (enemyFollower.MyState == EnemyFollower.State.WaitAndTaunt)
      {
        if (enemyFollower.Spine.AnimationName != "roll-stop" && enemyFollower.state.CURRENT_STATE == StateMachine.State.Moving && enemyFollower.Spine.AnimationName != "run-enemy")
          enemyFollower.Spine.AnimationState.SetAnimation(1, "run-enemy", true);
        enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
        enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
        if (enemyFollower.state.CURRENT_STATE == StateMachine.State.Idle && (double) (enemyFollower.RepathTimer -= Time.deltaTime) < 0.0)
        {
          if (enemyFollower.CustomAttackLogic())
            break;
          if ((double) enemyFollower.MaxAttackDelay < 0.0 || (double) Vector3.Distance(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position) < (double) enemyFollower.AttackWithinRange)
          {
            if (enemyFollower.ChargeAndAttack && ((double) enemyFollower.MaxAttackDelay < 0.0 || (double) enemyFollower.AttackDelay < 0.0))
            {
              enemyFollower.health.invincible = false;
              enemyFollower.StopAllCoroutines();
              enemyFollower.DisableForces = false;
              enemyFollower.StartCoroutine(enemyFollower.FightPlayer());
            }
            else if (!enemyFollower.health.HasShield)
            {
              enemyFollower.Angle = (float) (((double) Utils.GetAngle(enemyFollower.TargetObject.transform.position, enemyFollower.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
              enemyFollower.TargetPosition = enemyFollower.TargetObject.transform.position + new Vector3(enemyFollower.MaintainTargetDistance * Mathf.Cos(enemyFollower.Angle), enemyFollower.MaintainTargetDistance * Mathf.Sin(enemyFollower.Angle));
              enemyFollower.FindPath(enemyFollower.TargetPosition);
            }
          }
          else if ((double) Vector3.Distance(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position) > (double) enemyFollower.MoveCloserDistance + (enemyFollower.health.HasShield ? 0.0 : 1.0))
          {
            enemyFollower.Angle = (float) (((double) Utils.GetAngle(enemyFollower.TargetObject.transform.position, enemyFollower.transform.position) + (double) UnityEngine.Random.Range(-20, 20)) * (Math.PI / 180.0));
            enemyFollower.TargetPosition = enemyFollower.TargetObject.transform.position + new Vector3(enemyFollower.MaintainTargetDistance * Mathf.Cos(enemyFollower.Angle), enemyFollower.MaintainTargetDistance * Mathf.Sin(enemyFollower.Angle));
            enemyFollower.FindPath(enemyFollower.TargetPosition);
          }
        }
      }
      enemyFollower.Seperate(0.5f);
      yield return (object) null;
    }
  }

  public override void BeAlarmed(GameObject TargetObject)
  {
    base.BeAlarmed(TargetObject);
    this.TargetObject = TargetObject;
    this.StartCoroutine(this.ChasePlayer());
  }

  public virtual bool CustomAttackLogic() => false;

  public void FindPath(Vector3 PointToCheck)
  {
    this.RepathTimer = 0.2f;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, 0.2f, (Vector2) Vector3.Normalize(PointToCheck - this.transform.position), (float) this.NewPositionDistance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
    {
      if ((double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.centroid) <= 1.0)
        return;
      if (this.ShowDebug)
      {
        this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset);
        this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
      }
      this.TargetPosition = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.transform.position - PointToCheck) * this.CircleCastOffset;
      this.givePath(this.TargetPosition);
    }
    else
    {
      this.TargetPosition = PointToCheck;
      this.givePath(PointToCheck);
    }
  }

  public IEnumerator TeleportRoutine(Vector3 Position)
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.ClearPaths();
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Moving;
    enemyFollower.UsePathing = false;
    enemyFollower.health.invincible = true;
    enemyFollower.SeperateObject = false;
    enemyFollower.MyState = EnemyFollower.State.Teleporting;
    enemyFollower.ClearPaths();
    Vector3 position = enemyFollower.transform.position;
    float Progress = 0.0f;
    enemyFollower.Spine.AnimationState.SetAnimation(1, "roll", true);
    enemyFollower.state.facingAngle = enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, Position);
    enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
    Vector3 b = Position;
    float Duration = Vector3.Distance(position, b) / 10f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      enemyFollower.speed = 10f * Time.deltaTime;
      yield return (object) null;
    }
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyFollower.Spine.AnimationState.SetAnimation(1, "roll-stop", false);
    yield return (object) new WaitForSeconds(0.3f);
    enemyFollower.UsePathing = true;
    enemyFollower.RepathTimer = 0.5f;
    enemyFollower.TeleportDelay = enemyFollower.TeleportDelayTarget;
    enemyFollower.SeperateObject = true;
    enemyFollower.health.invincible = false;
    enemyFollower.MyState = EnemyFollower.State.WaitAndTaunt;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
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

  public IEnumerator FightPlayer(float AttackDistance = 1.5f)
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.MyState = EnemyFollower.State.Attacking;
    enemyFollower.UsePathing = true;
    enemyFollower.givePath(enemyFollower.TargetObject.transform.position);
    enemyFollower.Spine.AnimationState.SetAnimation(1, "run-enemy", true);
    enemyFollower.RepathTimer = 0.0f;
    int NumAttacks = enemyFollower.DoubleAttack ? 2 : 1;
    int AttackCount = 1;
    float MaxAttackSpeed = 15f;
    float AttackSpeed = MaxAttackSpeed;
    bool Loop = true;
    float SignPostDelay = 0.5f;
    while (Loop)
    {
      enemyFollower.Seperate(0.5f);
      if ((UnityEngine.Object) enemyFollower.TargetObject == (UnityEngine.Object) null)
      {
        yield return (object) new WaitForSeconds(0.3f);
        enemyFollower.StartCoroutine(enemyFollower.WaitForTarget());
        yield break;
      }
      if (enemyFollower.state.CURRENT_STATE == StateMachine.State.Idle)
        enemyFollower.state.CURRENT_STATE = StateMachine.State.Moving;
      switch (enemyFollower.state.CURRENT_STATE)
      {
        case StateMachine.State.Moving:
          enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
          enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
          enemyFollower.state.LookAngle = enemyFollower.state.facingAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
          if ((double) Vector2.Distance((Vector2) enemyFollower.transform.position, (Vector2) enemyFollower.TargetObject.transform.position) < (double) AttackDistance)
          {
            enemyFollower.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            enemyFollower.variant = UnityEngine.Random.Range(0, 2);
            string animationName = enemyFollower.variant == 0 ? "attack-charge" : "attack-charge2";
            enemyFollower.Spine.AnimationState.SetAnimation(1, animationName, false);
          }
          else
          {
            if (enemyFollower.CanShoot && (double) Time.time > (double) enemyFollower.timeBetweenShooting)
            {
              enemyFollower.timeBetweenShooting = Time.time + UnityEngine.Random.Range(3f, 6f);
              enemyFollower.StartCoroutine(enemyFollower.ShootArrowRoutine());
              yield break;
            }
            if ((double) (enemyFollower.RepathTimer += Time.deltaTime) > 0.20000000298023224)
            {
              enemyFollower.RepathTimer = 0.0f;
              enemyFollower.givePath(enemyFollower.TargetObject.transform.position);
            }
            if ((UnityEngine.Object) enemyFollower.damageColliderEvents != (UnityEngine.Object) null)
            {
              if ((double) enemyFollower.state.Timer < 0.20000000298023224 && !enemyFollower.health.WasJustParried)
                enemyFollower.damageColliderEvents.SetActive(true);
              else
                enemyFollower.damageColliderEvents.SetActive(false);
            }
          }
          if ((UnityEngine.Object) enemyFollower.damageColliderEvents != (UnityEngine.Object) null)
          {
            enemyFollower.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          if ((UnityEngine.Object) enemyFollower.damageColliderEvents != (UnityEngine.Object) null)
            enemyFollower.damageColliderEvents.SetActive(false);
          enemyFollower.SimpleSpineFlash.FlashWhite(enemyFollower.state.Timer / SignPostDelay);
          enemyFollower.state.Timer += Time.deltaTime;
          if ((double) enemyFollower.state.Timer >= (double) SignPostDelay - (double) EnemyFollower.signPostParryWindow)
            enemyFollower.canBeParried = true;
          if (enemyFollower.health.team == Health.Team.PlayerTeam && (UnityEngine.Object) enemyFollower.TargetObject != (UnityEngine.Object) null)
          {
            enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
            enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
            enemyFollower.state.LookAngle = enemyFollower.state.facingAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
          }
          if ((double) enemyFollower.state.Timer >= (double) SignPostDelay)
          {
            enemyFollower.SimpleSpineFlash.FlashWhite(false);
            CameraManager.shakeCamera(0.4f, enemyFollower.state.LookAngle);
            enemyFollower.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemyFollower.speed = AttackSpeed * 0.0166666675f;
            string animationName = enemyFollower.variant == 0 ? "attack-impact" : "attack-impact2";
            enemyFollower.Spine.AnimationState.SetAnimation(1, animationName, false);
            enemyFollower.canBeParried = true;
            enemyFollower.StartCoroutine(enemyFollower.EnableDamageCollider(0.0f));
            if (!string.IsNullOrEmpty(enemyFollower.attackSoundPath))
            {
              AudioManager.Instance.PlayOneShot(enemyFollower.attackSoundPath, enemyFollower.transform.position);
              break;
            }
            break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) AttackSpeed > 0.0)
            AttackSpeed -= 1f * GameManager.DeltaTime;
          enemyFollower.speed = AttackSpeed * Time.deltaTime;
          enemyFollower.SimpleSpineFlash.FlashWhite(false);
          enemyFollower.canBeParried = (double) enemyFollower.state.Timer <= (double) EnemyFollower.attackParryWindow;
          if ((double) (enemyFollower.state.Timer += Time.deltaTime) >= (AttackCount + 1 <= NumAttacks ? 0.5 : 1.0))
          {
            if (++AttackCount <= NumAttacks)
            {
              AttackSpeed = MaxAttackSpeed + (float) ((3 - NumAttacks) * 2);
              enemyFollower.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyFollower.variant = UnityEngine.Random.Range(0, 2);
              string animationName = enemyFollower.variant == 0 ? "attack-charge" : "attack-charge2";
              enemyFollower.Spine.AnimationState.SetAnimation(1, animationName, false);
              SignPostDelay = 0.3f;
              break;
            }
            Loop = false;
            enemyFollower.SimpleSpineFlash.FlashWhite(false);
            break;
          }
          break;
      }
      yield return (object) null;
    }
    enemyFollower.StartCoroutine(enemyFollower.ChasePlayer());
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    this.ClearPaths();
    GameManager.GetInstance().StartCoroutine(this.PlaceIE());
  }

  public void RoomLockController_OnRoomCleared()
  {
    if (!this.VanishOnRoomComplete)
      return;
    this.ClearPaths();
    this.StopAllCoroutines();
    this.StopCoroutine(this.artificialUpdate);
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.Spine.AnimationState.ClearTracks();
    this.Spine.AnimationState.SetAnimation(0, "spawn-out3", false);
    GameManager.GetInstance().StartCoroutine(this.Delay(1.5f, (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject))));
  }

  public IEnumerator ShootArrowRoutine(float minDelay = 3f, float maxDelay = 4f)
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.ClearPaths();
    enemyFollower.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    enemyFollower.MyState = EnemyFollower.State.Shooting;
    yield return (object) null;
    enemyFollower.simpleSpineAnimator.Animate("bow-attack-charge", 1, false);
    enemyFollower.Aiming.gameObject.SetActive(true);
    float time = 0.0f;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime * enemyFollower.Spine.timeScale) < 1.0)
    {
      enemyFollower.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyFollower.state.LookAngle);
      enemyFollower.SimpleSpineFlash?.FlashWhite(Progress / 1f);
      if (Time.frameCount % 5 == 0)
        enemyFollower.Aiming.color = enemyFollower.Aiming.color == Color.red ? Color.white : Color.red;
      yield return (object) null;
    }
    enemyFollower.SimpleSpineFlash.FlashWhite(false);
    enemyFollower.Aiming.gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(enemyFollower.attackSoundPath))
      AudioManager.Instance.PlayOneShot(enemyFollower.attackSoundPath, enemyFollower.transform.position);
    int i = enemyFollower.ShotsToFire;
    while (--i >= 0)
    {
      if ((UnityEngine.Object) enemyFollower.TargetObject != (UnityEngine.Object) null)
        enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
      enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
      enemyFollower.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyFollower.state.LookAngle);
      if (!string.IsNullOrEmpty(enemyFollower.shootSoundPath))
        AudioManager.Instance.PlayOneShot(enemyFollower.shootSoundPath, enemyFollower.transform.position);
      CameraManager.shakeCamera(0.2f, enemyFollower.state.LookAngle);
      Projectile component = ObjectPool.Spawn(enemyFollower.Arrow, enemyFollower.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyFollower.transform.position + new Vector3(0.5f * Mathf.Cos(enemyFollower.state.LookAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(enemyFollower.state.LookAngle * ((float) Math.PI / 180f)));
      component.Angle = enemyFollower.state.LookAngle;
      component.team = enemyFollower.health.team;
      component.Owner = enemyFollower.health;
      enemyFollower.simpleSpineAnimator.Animate("bow-attack-impact", 1, false);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyFollower.Spine.timeScale) < (double) enemyFollower.DelayBetweenShots)
      {
        enemyFollower.SimpleSpineFlash?.FlashWhite(time / enemyFollower.DelayBetweenShots);
        if (Time.frameCount % 5 == 0)
          enemyFollower.Aiming.color = enemyFollower.Aiming.color == Color.red ? Color.white : Color.red;
        yield return (object) null;
      }
      enemyFollower.SimpleSpineFlash.FlashWhite(false);
      if ((UnityEngine.Object) enemyFollower.TargetObject != (UnityEngine.Object) null && i > 0)
      {
        enemyFollower.Aiming.gameObject.SetActive(true);
        enemyFollower.state.LookAngle = Utils.GetAngle(enemyFollower.transform.position, enemyFollower.TargetObject.transform.position);
        enemyFollower.Spine.skeleton.ScaleX = (double) enemyFollower.state.LookAngle <= 90.0 || (double) enemyFollower.state.LookAngle >= 270.0 ? -1f : 1f;
        enemyFollower.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyFollower.state.LookAngle);
        yield return (object) new WaitForSeconds(enemyFollower.DelayReaiming);
      }
    }
    enemyFollower.Aiming.gameObject.SetActive(false);
    enemyFollower.TargetObject = (GameObject) null;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyFollower.Spine.timeScale) < 0.30000001192092896)
      yield return (object) null;
    enemyFollower.MyState = EnemyFollower.State.WaitAndTaunt;
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyFollower.StartCoroutine(enemyFollower.WaitForTarget());
  }

  public IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator PlaceIE()
  {
    EnemyFollower enemyFollower = this;
    enemyFollower.ClearPaths();
    Vector3 offset = (Vector3) UnityEngine.Random.insideUnitCircle;
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      enemyFollower.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 vector3 = (PlayerFarming.Instance.transform.position + offset) with
      {
        z = 0.0f
      };
      enemyFollower.transform.position = vector3;
      yield return (object) null;
    }
    enemyFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyFollower.Spine.AnimationState.SetAnimation(0, "idle-enemy", true);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team)
      return;
    this.EnemyHealth.DealDamage(this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public IEnumerator EnableDamageCollider(float initialDelay)
  {
    if ((bool) (UnityEngine.Object) this.damageColliderEvents)
    {
      yield return (object) new WaitForSeconds(initialDelay);
      this.damageColliderEvents.SetActive(true);
      yield return (object) new WaitForSeconds(0.2f);
      this.damageColliderEvents.SetActive(false);
    }
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__49_0(AsyncOperationHandle<GameObject> obj)
  {
    UnitObject component1 = EnemySpawner.Create(BiomeGenerator.GetRandomPositionInIsland(), this.transform.parent, obj.Result).GetComponent<UnitObject>();
    component1.CanHaveModifier = false;
    component1.RemoveModifier();
    DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
    if ((bool) (UnityEngine.Object) component2)
      component2.GiveXP = false;
    this.spawnedEnemies.Add(component1.health);
  }

  [CompilerGenerated]
  public void \u003CRoomLockController_OnRoomCleared\u003Eb__74_0()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public enum State
  {
    WaitAndTaunt,
    Teleporting,
    Attacking,
    Shooting,
  }
}
