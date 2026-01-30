// Decompiled with JetBrains decompiler
// Type: UnitObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using MMRoomGeneration;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
[RequireComponent(typeof (Seeker))]
[RequireComponent(typeof (StateMachine))]
[RequireComponent(typeof (Health))]
[RequireComponent(typeof (CircleCollider2D))]
public class UnitObject : BaseMonoBehaviour
{
  [SerializeField]
  public Enemy enemyType;
  public bool SeperateObject;
  public bool SeparateObjectFromPlayer;
  public int VisionRange = 15;
  public bool CheckSightBeforePath = true;
  public bool UsePathing = true;
  public LayerMask layerToCheck;
  public float maxSpeed = 0.05f;
  public float StoppingDistance = 0.1f;
  public float SpeedMultiplier = 1f;
  public bool emitDustClouds = true;
  public bool OffsetDustCloud;
  public Vector3 dustCloudsOffset = Vector3.zero;
  public float distanceBetweenDustClouds = 0.5f;
  [CompilerGenerated]
  public bool \u003CUseDeltaTime\u003Ek__BackingField = true;
  public Seeker seeker;
  public static NNConstraint constraint = new NNConstraint();
  [HideInInspector]
  public StateMachine state;
  public Vector2 targetLocation;
  [HideInInspector]
  public float vx;
  [HideInInspector]
  public float vy;
  [HideInInspector]
  public float seperatorVX;
  [HideInInspector]
  public float seperatorVY;
  [HideInInspector]
  public float moveVX;
  [HideInInspector]
  public float moveVY;
  [HideInInspector]
  public float knockBackVX;
  [HideInInspector]
  public float knockBackVY;
  public int currentWaypoint;
  [HideInInspector]
  public List<Vector3> pathToFollow;
  [HideInInspector]
  public float speed;
  public bool isFlyingEnemy;
  public bool isImmuneToKnockback;
  public Vector2 positionLastFrame;
  [HideInInspector]
  public Health health;
  [HideInInspector]
  public Health TargetEnemy;
  public System.Action EndOfPath;
  public Coroutine knockRoutine;
  public EnemyModifier modifier;
  public float modifierTimer;
  public float EnemyModifierIconOffset = 2.25f;
  [HideInInspector]
  public EnemyOrderGroupIndicator orderIndicator;
  [SerializeField]
  public bool isBoss;
  public float distanceTravelledSinceLastDustCloud;
  public bool CanHaveModifier = true;
  public static List<UnitObject> Seperaters = new List<UnitObject>();
  public MeshRenderer[] childRenderers = new MeshRenderer[0];
  public Vector3 previousPosition = Vector3.zero;
  public bool UseFixedDirectionalPathing;
  public LayerMask groundCheckMask;
  public ModifierIcon modifierIcon;
  public Vector3 goToNoPathfinding;
  public Vector3 pointToCheck;
  public CircleCollider2D ColliderRadius;
  [HideInInspector]
  public Rigidbody2D rb;
  public bool DisableForces;
  public Vector3 PrevPosition;
  public bool LockToGround = true;
  public Vector3 offsetDirection;
  public RaycastHit LockToGroundHit;
  public Vector3 LockToGroundPosition;
  public Vector3 LockToGroundNewPosition;
  public bool dead;
  public float checkFrame;
  public Health cachedTarget;
  public static int LeaderEncounterColorBoost = Shader.PropertyToID("_LeaderEncounterColorBoost");
  public PlayerFarming closestPlayerFarming;

  public Enemy EnemyType => this.enemyType;

  public bool UseDeltaTime
  {
    get => this.\u003CUseDeltaTime\u003Ek__BackingField;
    set => this.\u003CUseDeltaTime\u003Ek__BackingField = value;
  }

  public bool IsBoss => this.isBoss;

  public bool HasModifier => (UnityEngine.Object) this.modifier != (UnityEngine.Object) null;

  public static event UnitObject.EnemyKilled OnEnemyKilled;

  public MeshRenderer[] ChildRenderers => this.childRenderers;

  public virtual float timeStopMultiplier => 1f;

  public void Seperate(float SeperationRadius, bool IgnorePlayer = false)
  {
    this.seperatorVX = 0.0f;
    this.seperatorVY = 0.0f;
    foreach (UnitObject seperater in UnitObject.Seperaters)
    {
      if ((!IgnorePlayer || seperater.health.team != Health.Team.PlayerTeam) && (!((UnityEngine.Object) seperater != (UnityEngine.Object) null) || !((UnityEngine.Object) seperater != (UnityEngine.Object) this) || this.health.team != Health.Team.PlayerTeam || seperater.SeparateObjectFromPlayer) && (UnityEngine.Object) seperater != (UnityEngine.Object) this && (UnityEngine.Object) seperater != (UnityEngine.Object) null && this.SeperateObject && seperater.SeperateObject && this.state.CURRENT_STATE != StateMachine.State.Dodging && this.state.CURRENT_STATE != StateMachine.State.Defending)
      {
        float num = Vector2.Distance((Vector2) seperater.gameObject.transform.position, (Vector2) this.transform.position);
        float angle = Utils.GetAngle(seperater.gameObject.transform.position, this.transform.position);
        if ((double) num < (double) SeperationRadius)
        {
          this.seperatorVX += (float) (((double) SeperationRadius - (double) num) / 2.0) * Mathf.Cos(angle * ((float) Math.PI / 180f)) * GameManager.FixedDeltaTime;
          this.seperatorVY += (float) (((double) SeperationRadius - (double) num) / 2.0) * Mathf.Sin(angle * ((float) Math.PI / 180f)) * GameManager.FixedDeltaTime;
        }
      }
    }
  }

  public virtual void Awake()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.rb = this.gameObject.GetComponent<Rigidbody2D>();
    this.childRenderers = this.GetComponentsInChildren<MeshRenderer>(true);
    if (this.health.team == Health.Team.Team2 && this.CanHaveModifier && (UnityEngine.Object) this.modifier == (UnityEngine.Object) null)
    {
      this.modifier = EnemyModifier.GetModifier(DataManager.Instance.EnemiesInNextRoomHaveModifiers ? 1f : 0.0f);
      if ((bool) (UnityEngine.Object) this.modifier)
        this.ForceSetModifier(this.modifier);
    }
    this.groundCheckMask = (LayerMask) -1;
    this.groundCheckMask = (LayerMask) ((int) this.groundCheckMask & -3);
  }

  public void ForceSetModifier(EnemyModifier modifier, bool tintAndScale = true)
  {
    this.modifier = modifier;
    this.modifierIcon = UnityEngine.Object.Instantiate<GameObject>(modifier.ModifierIcon, this.transform.position, Quaternion.identity).GetComponent<ModifierIcon>();
    GameObject gameObject = this.modifierIcon.gameObject;
    gameObject.transform.parent = this.transform;
    gameObject.transform.localPosition = Vector3.back * (tintAndScale ? this.EnemyModifierIconOffset : 1.5f);
    this.modifierIcon.Init(modifier);
    if (tintAndScale)
    {
      MaterialPropertyBlock properties = new MaterialPropertyBlock();
      properties.SetColor("_Color", modifier.ColorTint);
      foreach (MeshRenderer childRenderer in this.childRenderers)
      {
        if (childRenderer.sortingLayerID != 15 && childRenderer.sortingLayerID != 20)
        {
          childRenderer.SetPropertyBlock(properties);
          childRenderer.transform.localScale *= modifier.Scale;
        }
      }
      foreach (SimpleSpineFlash componentsInChild in this.GetComponentsInChildren<SimpleSpineFlash>())
        componentsInChild.OverrideBaseColor(modifier.ColorTint);
    }
    ShowHPBar component = this.GetComponent<ShowHPBar>();
    if ((bool) (UnityEngine.Object) component)
      component.zOffset *= modifier.Scale;
    this.GetComponent<Health>().totalHP *= modifier.HealthMultiplier;
  }

  public void ForceSetModifier(EnemyModifier.ModifierType modifierType, bool tintAndScale = true)
  {
    this.modifier = EnemyModifier.GetModifier(modifierType);
    if (!((UnityEngine.Object) this.modifier != (UnityEngine.Object) null))
      return;
    this.ForceSetModifier(this.modifier, tintAndScale);
  }

  public void RemoveModifier()
  {
    if (!(bool) (UnityEngine.Object) this.modifier)
      return;
    MaterialPropertyBlock properties = new MaterialPropertyBlock();
    properties.SetColor("_Color", Color.white);
    foreach (MeshRenderer childRenderer in this.childRenderers)
    {
      if (childRenderer.sortingLayerID != 15 && childRenderer.sortingLayerID != 20)
      {
        childRenderer.SetPropertyBlock(properties);
        childRenderer.transform.localScale /= this.modifier.Scale;
      }
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.modifierIcon.gameObject);
    this.health.totalHP /= this.modifier.HealthMultiplier;
    this.health.HP = this.health.totalHP;
    this.modifier = (EnemyModifier) null;
  }

  public virtual void OnEnable()
  {
    if ((bool) (UnityEngine.Object) this.seeker)
      this.seeker.pathCallback += new OnPathDelegate(this.startPath);
    UnitObject.Seperaters.Add(this);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.OnDieEarly += new Health.DieAction(this.OnDieEarly);
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnHitEarly += new Health.HitAction(this.OnHitEarly);
    Color color = !((UnityEngine.Object) LightingManager.Instance != (UnityEngine.Object) null) || !LightingManager.Instance.inLeaderEncounter ? new Color(0.0f, 0.0f, 0.0f, 0.0f) : new Color(-0.25f, 0.0f, 0.25f, 0.0f);
    if (SceneManager.GetActiveScene().name == "Base Biome 1")
      color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5)
      return;
    foreach (MeshRenderer componentsInChild in this.GetComponentsInChildren<MeshRenderer>())
    {
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) null && (UnityEngine.Object) componentsInChild.sharedMaterial != (UnityEngine.Object) null && componentsInChild.sortingLayerID != 15 && componentsInChild.sortingLayerID != 20)
        componentsInChild.sharedMaterial.SetColor(UnitObject.LeaderEncounterColorBoost, color);
    }
    MeshRenderer component = this.gameObject.GetComponent<MeshRenderer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.sharedMaterial != (UnityEngine.Object) null))
      return;
    component.sharedMaterial.SetColor(UnitObject.LeaderEncounterColorBoost, color);
  }

  public virtual void OnDisable()
  {
    this.seeker.CancelCurrentPathRequest();
    if (this.DisableForces && this.knockRoutine != null)
    {
      this.DisableForces = false;
      this.knockRoutine = (Coroutine) null;
    }
    UnitObject.Seperaters.Remove(this);
    this.seeker.pathCallback -= new OnPathDelegate(this.startPath);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnDieEarly -= new Health.DieAction(this.OnDieEarly);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnHitEarly -= new Health.HitAction(this.OnHitEarly);
  }

  public virtual void DoKnockBack(
    GameObject Attacker,
    float KnockbackModifier,
    float Duration,
    bool appendForce = true)
  {
    if ((UnityEngine.Object) this.rb == (UnityEngine.Object) null || this.isImmuneToKnockback)
      return;
    if (this.knockRoutine != null)
      this.StopCoroutine(this.knockRoutine);
    if (!appendForce)
      this.rb.velocity = (Vector2) Vector3.zero;
    this.knockRoutine = this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), KnockbackModifier, Duration));
  }

  public virtual void DoKnockBack(
    float angle,
    float KnockbackModifier,
    float Duration,
    bool appendForce = true)
  {
    if (this.knockRoutine != null)
      this.StopCoroutine(this.knockRoutine);
    if (!appendForce)
      this.rb.velocity = (Vector2) Vector3.zero;
    this.knockRoutine = this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(angle, KnockbackModifier, Duration));
  }

  public IEnumerator ApplyForceRoutine(float angle, float KnockbackModifier, float Duration)
  {
    this.DisableForces = true;
    this.rb.velocity = (Vector2) ((Vector3) new Vector2(25f * Mathf.Cos(angle), 25f * Mathf.Sin(angle)) * KnockbackModifier);
    yield return (object) new WaitForSeconds(Duration);
    this.DisableForces = false;
    this.knockRoutine = (Coroutine) null;
  }

  public virtual void BeAlarmed(GameObject TargetObject)
  {
  }

  public event UnitObject.Action NewPath;

  public virtual void givePath(
    Vector3 targetLocation,
    GameObject targetObject = null,
    bool forceAStar = false,
    bool forceIgnoreAStar = false)
  {
    this.ClearPaths();
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null && !forceIgnoreAStar)
    {
      if ((UnityEngine.Object) targetObject == (UnityEngine.Object) null)
      {
        Health closestTarget = this.GetClosestTarget();
        if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
          targetObject = closestTarget.gameObject;
      }
      bool flag = this.CheckSightBeforePath;
      if (flag && (UnityEngine.Object) targetObject != (UnityEngine.Object) null)
        flag = this.CheckLineOfSightOnTarget(targetObject, targetLocation, Vector2.Distance((Vector2) this.transform.position, (Vector2) targetLocation));
      else if (flag)
        flag = this.CheckLineOfSight(targetLocation, Vector2.Distance((Vector2) this.transform.position, (Vector2) targetLocation));
      if (flag && !forceAStar)
      {
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        this.pathToFollow = new List<Vector3>();
        this.goToNoPathfinding = AstarPath.active.GetNearest(targetLocation).node == null ? targetLocation : (Vector3) AstarPath.active.GetNearest(targetLocation).node.position;
        this.pathToFollow.Add(this.goToNoPathfinding);
        this.currentWaypoint = 0;
      }
      else
      {
        this.state.CURRENT_STATE = StateMachine.State.Moving;
        GraphNode node = AstarPath.active.GetNearest(targetLocation).node;
        if (node != null)
        {
          this.goToNoPathfinding = (Vector3) node.position;
          this.seeker.StartPath(this.transform.position, this.goToNoPathfinding);
        }
      }
    }
    else
    {
      Debug.Log((object) ("No need pathfinding " + targetLocation.ToString()));
      this.state.CURRENT_STATE = StateMachine.State.Moving;
      this.pathToFollow = new List<Vector3>();
      this.pathToFollow.Add(targetLocation);
      this.currentWaypoint = 0;
    }
    if (this.NewPath == null)
      return;
    this.NewPath();
  }

  public bool OnGround(Vector3 Position)
  {
    LayerMask mask = (LayerMask) LayerMask.GetMask("Island");
    this.StartCoroutine((IEnumerator) this.DrawRay(Position));
    return Physics.Raycast(Position, Vector3.forward, out RaycastHit _, float.PositiveInfinity, (int) mask);
  }

  public IEnumerator DrawRay(Vector3 Position)
  {
    float Timer = 3f;
    while ((double) (Timer -= Time.deltaTime) > 0.0)
    {
      Debug.DrawRay(Position, Vector3.forward, Color.blue);
      yield return (object) null;
    }
  }

  public bool IsPathPossible(Vector3 PathStart, Vector3 PathEnd)
  {
    return PathUtilities.IsPathPossible(AstarPath.active.GetNearest(PathStart, NNConstraint.Default).node, AstarPath.active.GetNearest(PathEnd, NNConstraint.Default).node);
  }

  public bool CheckLineOfSight(Vector3 pointToCheck, float distance)
  {
    PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(pointToCheck);
    if ((UnityEngine.Object) closestPlayer == (UnityEngine.Object) null)
      return false;
    this.pointToCheck = pointToCheck;
    if ((UnityEngine.Object) this.ColliderRadius == (UnityEngine.Object) null)
      this.ColliderRadius = this.GetComponent<CircleCollider2D>();
    RaycastHit2D raycastHit2D1 = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D1.collider != (UnityEngine.Object) null && (UnityEngine.Object) raycastHit2D1.collider != (UnityEngine.Object) closestPlayer.circleCollider2D)
      return false;
    float angle = Utils.GetAngle(this.transform.position, pointToCheck);
    RaycastHit2D raycastHit2D2 = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle + 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle + 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) null && (UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) closestPlayer.circleCollider2D)
      return false;
    raycastHit2D2 = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle - 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle - 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    return !((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) null) || !((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) closestPlayer.circleCollider2D);
  }

  public bool CheckLineOfSightAllPlayers(Vector3 pointToCheck, float distance)
  {
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if (!((UnityEngine.Object) player == (UnityEngine.Object) null))
      {
        this.pointToCheck = pointToCheck;
        if ((UnityEngine.Object) this.ColliderRadius == (UnityEngine.Object) null)
          this.ColliderRadius = this.GetComponent<CircleCollider2D>();
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
        if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null) || !((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) player.circleCollider2D))
        {
          float angle = Utils.GetAngle(this.transform.position, pointToCheck);
          raycastHit2D = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle + 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle + 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
          if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null) || !((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) player.circleCollider2D))
          {
            raycastHit2D = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle - 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle - 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
            if (!((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null) || !((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) player.circleCollider2D))
              return true;
          }
        }
      }
    }
    return false;
  }

  public bool CheckLineOfSightOnTarget(
    GameObject targetObject,
    Vector3 pointToCheck,
    float distance,
    bool ignoreKnockedOut = true)
  {
    PlayerFarming component = targetObject.GetComponent<PlayerFarming>();
    Collider2D collider2D;
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (ignoreKnockedOut && component.IsKnockedOut)
        return false;
      collider2D = (Collider2D) component.circleCollider2D;
    }
    else
      collider2D = targetObject.GetComponent<Collider2D>();
    this.pointToCheck = pointToCheck;
    if ((UnityEngine.Object) this.ColliderRadius == (UnityEngine.Object) null)
      this.ColliderRadius = this.GetComponent<CircleCollider2D>();
    RaycastHit2D raycastHit2D1 = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D1.collider != (UnityEngine.Object) null && (UnityEngine.Object) raycastHit2D1.collider != (UnityEngine.Object) collider2D)
      return false;
    float angle = Utils.GetAngle(this.transform.position, pointToCheck);
    RaycastHit2D raycastHit2D2 = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle + 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle + 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) null && (UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) collider2D)
      return false;
    raycastHit2D2 = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle - 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle - 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    return !((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) null) || !((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) collider2D);
  }

  public bool CheckLineOfSight(float distance)
  {
    return !(bool) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (PlayerFarming.FindClosestPlayer(this.pointToCheck).transform.position - this.transform.position).normalized, distance, (int) this.layerToCheck);
  }

  public void CreateFleePath(Vector3 FleeFromPosition)
  {
    int searchLength = 50000;
    FleePath p = FleePath.Construct(this.transform.position, FleeFromPosition, searchLength);
    p.aimStrength = 0.5f;
    p.spread = 4000;
    this.seeker.StartPath((Path) p);
    this.state.CURRENT_STATE = StateMachine.State.Fleeing;
  }

  public void startPath(Path p)
  {
    if (p.error)
      return;
    this.pathToFollow = new List<Vector3>();
    for (int index = 0; index < p.vectorPath.Count; ++index)
      this.pathToFollow.Add(p.vectorPath[index]);
    this.currentWaypoint = 1;
  }

  public static Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition)
  {
    Vector2 vector2 = targetPosition - startingPosition;
    return Quaternion.AngleAxis(Mathf.Atan2(vector2.y, vector2.x) * 57.29578f, Vector3.forward);
  }

  public void ClearPaths()
  {
    this.pathToFollow = (List<Vector3>) null;
    this.move();
  }

  public virtual void Update()
  {
    if (this.modifier.HasModifier(EnemyModifier.ModifierType.DropProjectiles))
    {
      float num = 5f;
      this.modifierTimer += Time.deltaTime;
      float progress = this.modifierTimer / num;
      if ((UnityEngine.Object) this.modifierIcon != (UnityEngine.Object) null)
        this.modifierIcon.UpdateTimer(progress);
      if ((double) this.modifierTimer >= (double) num)
      {
        Projectile.CreateProjectiles(5, this.health, new Vector3(this.transform.position.x, this.transform.position.y, 0.0f), false);
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", this.gameObject);
        this.modifierTimer = 0.0f;
      }
    }
    float num1 = this.UseDeltaTime ? GameManager.DeltaTime : GameManager.UnscaledDeltaTime;
    if (this.UsePathing)
    {
      if (this.pathToFollow == null)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / 4.0) * num1;
        this.move();
        return;
      }
      if (this.currentWaypoint >= this.pathToFollow.Count)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / 4.0) * num1;
        this.move();
        return;
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
    {
      this.speed += (float) (((double) this.maxSpeed * (double) this.SpeedMultiplier - (double) this.speed) / 7.0) * num1;
      if (this.UsePathing && this.pathToFollow != null)
      {
        if (this.UseFixedDirectionalPathing)
        {
          int num2 = Mathf.CeilToInt((this.previousPosition == Vector3.zero ? this.StoppingDistance * 2f : Vector3.Distance(this.previousPosition, this.transform.position)) / this.StoppingDistance);
          for (int index = 0; index < num2; ++index)
          {
            Vector3 a = Vector3.Lerp(this.previousPosition, this.transform.position, (float) index / (float) num2);
            this.state.facingAngle = Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]);
            if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.pathToFollow[this.currentWaypoint]) <= (double) this.StoppingDistance || (double) Vector2.Distance((Vector2) a, (Vector2) this.pathToFollow[this.currentWaypoint]) <= (double) this.StoppingDistance)
            {
              ++this.currentWaypoint;
              if (this.currentWaypoint == this.pathToFollow.Count)
              {
                this.state.CURRENT_STATE = StateMachine.State.Idle;
                System.Action endOfPath = this.EndOfPath;
                if (endOfPath != null)
                  endOfPath();
                this.pathToFollow = (List<Vector3>) null;
                this.speed = 0.0f;
                break;
              }
            }
          }
        }
        else
        {
          this.state.facingAngle = Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]);
          if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.pathToFollow[this.currentWaypoint]) <= (double) this.StoppingDistance)
          {
            ++this.currentWaypoint;
            if (this.currentWaypoint == this.pathToFollow.Count)
            {
              this.state.CURRENT_STATE = StateMachine.State.Idle;
              System.Action endOfPath = this.EndOfPath;
              if (endOfPath != null)
                endOfPath();
              this.pathToFollow = (List<Vector3>) null;
              this.speed = 0.0f;
            }
          }
        }
      }
    }
    else
      this.speed += (float) ((0.0 - (double) this.speed) / 4.0) * num1;
    this.move();
  }

  public void move()
  {
    if (float.IsNaN(this.state.facingAngle))
      return;
    if (float.IsNaN(this.speed) || float.IsInfinity(this.speed))
      this.speed = 0.0f;
    this.speed = Mathf.Clamp(this.speed, 0.0f, this.maxSpeed);
    this.moveVX = this.speed * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f));
    this.moveVY = this.speed * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f));
    this.previousPosition = this.transform.position;
  }

  public virtual void FixedUpdate()
  {
    if ((UnityEngine.Object) this.rb == (UnityEngine.Object) null || this.DisableForces)
      return;
    float num = this.UseDeltaTime ? GameManager.DeltaTime : GameManager.UnscaledDeltaTime;
    this.knockBackVX += (float) ((0.0 - (double) this.knockBackVX) / 4.0) * num;
    this.knockBackVY += (float) ((0.0 - (double) this.knockBackVY) / 4.0) * num;
    if (float.IsNaN(this.moveVX) || float.IsInfinity(this.moveVX))
      this.moveVX = 0.0f;
    if (float.IsNaN(this.moveVY) || float.IsInfinity(this.moveVY))
      this.moveVY = 0.0f;
    Vector2 position = this.rb.position + new Vector2(this.vx, this.vy) * Time.deltaTime + new Vector2(this.moveVX, this.moveVY) * num + new Vector2(this.seperatorVX, this.seperatorVY) * num + new Vector2(this.knockBackVX, this.knockBackVY) * num;
    this.rb.MovePosition(position);
    this.positionLastFrame = position;
  }

  public void LateUpdate()
  {
    if ((this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing || this.state.CURRENT_STATE == StateMachine.State.DashAcrossIsland || this.state.CURRENT_STATE == StateMachine.State.Dodging) && this.emitDustClouds)
    {
      this.distanceTravelledSinceLastDustCloud += (this.transform.position - this.PrevPosition).magnitude;
      if ((double) this.distanceTravelledSinceLastDustCloud >= (double) this.distanceBetweenDustClouds)
      {
        this.distanceTravelledSinceLastDustCloud = 0.0f;
        if ((UnityEngine.Object) this.transform != (UnityEngine.Object) null && (UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
        {
          if (this.OffsetDustCloud)
          {
            this.offsetDirection = this.PrevPosition - this.transform.position;
            this.offsetDirection.z = 0.0f;
            this.offsetDirection.Normalize();
            BiomeConstants.Instance.EmitDustCloudParticles(this.transform.position + this.offsetDirection * this.dustCloudsOffset.magnitude);
          }
          else
            BiomeConstants.Instance.EmitDustCloudParticles(this.transform.position);
        }
      }
    }
    this.PrevPosition = this.transform.position;
    if (!this.LockToGround)
      return;
    this.LockToGroundPosition = this.transform.position + Vector3.back * 10f;
    if (Physics.Raycast(this.LockToGroundPosition, Vector3.forward, out this.LockToGroundHit, float.PositiveInfinity, (int) this.groundCheckMask))
    {
      if (!((UnityEngine.Object) this.LockToGroundHit.collider.gameObject.GetComponent<MeshCollider>() != (UnityEngine.Object) null))
        return;
      this.LockToGroundNewPosition = this.transform.position;
      this.LockToGroundNewPosition.z = this.LockToGroundHit.point.z;
      this.transform.position = this.LockToGroundNewPosition;
    }
    else
    {
      this.LockToGroundNewPosition = this.transform.position;
      this.LockToGroundNewPosition.z = 0.0f;
      this.transform.position = this.LockToGroundNewPosition;
    }
  }

  public virtual void OnDieEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
  }

  public virtual void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.dead)
      return;
    Vector3 vector3 = new Vector3(this.transform.position.x, this.transform.position.y, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform.position.z : this.transform.position.z);
    if (this.modifier.HasModifier(EnemyModifier.ModifierType.DropPoison))
    {
      TrapPoison.CreatePoison(vector3, 10, 0.5f, this.transform.parent);
      AudioManager.Instance.PlayOneShot("event:/player/poison_damage", vector3);
    }
    if (this.modifier.HasModifier(EnemyModifier.ModifierType.DropBomb))
    {
      Bomb.CreateBomb(vector3, this.health, this.transform.parent);
      AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", vector3);
    }
    if (this.health.team == Health.Team.Team2 && DataManager.Instance.EnemiesDropBombOnDeath)
    {
      Bomb.CreateBomb(vector3, this.health, this.transform.parent);
      AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", vector3);
    }
    PlayerFarming playerFarming = Attacker.GetComponent<PlayerFarming>();
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
    {
      GameObject spellOwner = Health.GetSpellOwner(Attacker);
      playerFarming = !((UnityEngine.Object) spellOwner == (UnityEngine.Object) null) ? PlayerFarming.GetPlayerFarmingComponent(spellOwner) : PlayerFarming.GetPlayerFarmingComponent(Attacker);
    }
    foreach (InventoryItem inventoryItem in TrinketManager.GetItemsToDrop(playerFarming))
      InventoryItem.Spawn((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity, this.transform.position);
    if ((double) UnityEngine.Random.value < (double) TrinketManager.GetLightningChanceOnKill(playerFarming) && ((UnityEngine.Object) this.health == (UnityEngine.Object) null || this.health.team == Health.Team.Team2))
    {
      int damage = 12;
      new LightningStrikeAbility(1).Play(playerFarming.gameObject, Health.Team.Team2, (float) damage, playerFarming, true, (List<Health>) null, "");
    }
    if (AttackType != Health.AttackTypes.Projectile && AttackType != Health.AttackTypes.Poison && AttackType != Health.AttackTypes.Burn && (double) this.timeStopMultiplier > 0.0)
      GameManager.GetInstance().HitStop(0.1f * this.timeStopMultiplier);
    DataManager.Instance.AddEnemyKilled(this.enemyType);
    UnitObject.EnemyKilled onEnemyKilled = UnitObject.OnEnemyKilled;
    if (onEnemyKilled != null)
      onEnemyKilled(this.enemyType);
    this.dead = true;
  }

  public virtual void OnHitEarly(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
  }

  public virtual void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    CameraManager.instance.ShakeCameraForDuration(0.6f, 0.8f, 0.3f, false);
    Vector3 vector3 = new Vector3(this.transform.position.x, this.transform.position.y, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform.position.z : this.transform.position.z);
    if (this.modifier.HasModifier(EnemyModifier.ModifierType.DropPoison))
    {
      TrapPoison.CreatePoison(vector3, 5, 0.1f, this.transform.parent);
      AudioManager.Instance.PlayOneShot("event:/player/poison_damage", vector3);
    }
    if (AttackType == Health.AttackTypes.Projectile || AttackType == Health.AttackTypes.Poison || AttackType == Health.AttackTypes.Burn || AttackType == Health.AttackTypes.NoHitStop)
      return;
    GameManager.GetInstance().HitStop(0.05f * this.timeStopMultiplier);
  }

  public virtual void OnDestroy()
  {
  }

  public void EmitFootstep() => AudioManager.Instance.PlayFootstep(this.transform.position);

  public static string GetLocalisedEnemyName(Enemy enemy)
  {
    return LocalizationManager.GetTranslation("Enemies/" + enemy.ToString());
  }

  public Health GetClosestTarget(
    bool ignoreBreakables = false,
    bool ignoreProjectiles = true,
    bool ignoreDownedPlayers = true,
    bool ignoreNonUnits = false)
  {
    return this.GetClosestTarget(Vector3.zero, ignoreBreakables, ignoreProjectiles, ignoreDownedPlayers, ignoreNonUnits);
  }

  public virtual Health GetClosestTarget(
    Vector3 offset,
    bool ignoreBreakables = false,
    bool ignoreProjectiles = true,
    bool ignoreDownedPlayers = true,
    bool ignoreNonUnits = false)
  {
    if ((double) Time.time == (double) this.checkFrame && (UnityEngine.Object) this.cachedTarget != (UnityEngine.Object) null)
      return this.cachedTarget;
    this.closestPlayerFarming = PlayerFarming.FindClosestPlayer(this.pointToCheck, !ignoreDownedPlayers);
    if (((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || BiomeGenerator.Instance.CurrentRoom == null || (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom == (UnityEngine.Object) null) && this.health.team != Health.Team.PlayerTeam)
      return !((UnityEngine.Object) this.closestPlayerFarming != (UnityEngine.Object) null) || this.closestPlayerFarming.GoToAndStopping ? (Health) null : (Health) this.closestPlayerFarming.health;
    Health.Team team = this.health.team == Health.Team.PlayerTeam ? Health.Team.Team2 : Health.Team.PlayerTeam;
    List<Health> healthList1 = new List<Health>((IEnumerable<Health>) Health.team2);
    List<Health> healthList2 = new List<Health>();
    if (team == Health.Team.PlayerTeam)
    {
      if ((bool) (UnityEngine.Object) this.closestPlayerFarming && Health.playerTeam.Count <= 1 && !this.closestPlayerFarming.IsKnockedOut)
        return this.closestPlayerFarming.GoToAndStopping ? (Health) null : (Health) this.closestPlayerFarming.health;
      healthList1.Clear();
      for (int index = 0; index < Health.playerTeam.Count; ++index)
      {
        if ((UnityEngine.Object) Health.playerTeam[index] != (UnityEngine.Object) null && (!ignoreProjectiles || !Projectile.Contains(Health.playerTeam[index])) && (!ignoreProjectiles || !ProjectileGhost.Contains(Health.playerTeam[index])))
          healthList1.Add(Health.playerTeam[index]);
      }
    }
    foreach (Health health in healthList1)
    {
      if (!((UnityEngine.Object) health == (UnityEngine.Object) null) && health.enabled && !health.invincible && !health.untouchable && !health.InanimateObject && (double) health.HP > 0.0 && (!ignoreBreakables || health.team != Health.Team.Team2 || !health.CompareTag("BreakableDecoration")) && (!ignoreProjectiles || !Projectile.Contains(health)) && (!ignoreProjectiles || !ProjectileGhost.Contains(health)))
      {
        if (ignoreDownedPlayers && health.team == Health.Team.PlayerTeam)
        {
          PlayerFarming component = health.GetComponent<PlayerFarming>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.IsKnockedOut)
            continue;
        }
        if ((!ignoreNonUnits || !((UnityEngine.Object) health.GetComponent<UnitObject>() == (UnityEngine.Object) null)) && (bool) (UnityEngine.Object) health && (health.team == team || health.IsCharmedEnemy && (UnityEngine.Object) health != (UnityEngine.Object) this.health))
          healthList2.Add(health);
      }
    }
    if (healthList2.Count == 0 && team == Health.Team.PlayerTeam && (bool) (UnityEngine.Object) this.closestPlayerFarming && (!ignoreDownedPlayers || !this.closestPlayerFarming.IsKnockedOut))
      return (Health) this.closestPlayerFarming.health;
    Health closestTarget = (Health) null;
    foreach (Health health in healthList2)
    {
      if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null || (double) Vector3.Distance(health.transform.position, this.transform.position + offset) < (double) Vector3.Distance(closestTarget.transform.position, this.transform.position + offset))
        closestTarget = health;
    }
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null && (UnityEngine.Object) this.closestPlayerFarming != (UnityEngine.Object) null && (!ignoreDownedPlayers || !this.closestPlayerFarming.IsKnockedOut) && team == Health.Team.PlayerTeam)
      return (Health) this.closestPlayerFarming.health;
    this.checkFrame = Time.time;
    this.cachedTarget = closestTarget;
    return closestTarget;
  }

  public Health ReconsiderPlayerTarget()
  {
    this.pointToCheck = this.transform.position;
    Health health = this.GetClosestTarget();
    if ((UnityEngine.Object) health == (UnityEngine.Object) null && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      health = (Health) PlayerFarming.Instance.health;
    return health;
  }

  public static Health GetClosestTarget(Transform obj, Health.Team targetTeam)
  {
    PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(obj.position);
    if ((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || BiomeGenerator.Instance.CurrentRoom == null || (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom == (UnityEngine.Object) null)
      return !((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null) || closestPlayer.GoToAndStopping ? (Health) null : (Health) closestPlayer.health;
    List<Health> healthList1 = new List<Health>((IEnumerable<Health>) Health.team2);
    List<Health> healthList2 = new List<Health>();
    if (targetTeam == Health.Team.PlayerTeam)
    {
      if ((bool) (UnityEngine.Object) closestPlayer && Health.playerTeam.Count <= 1)
        return closestPlayer.GoToAndStopping ? (Health) null : (Health) closestPlayer.health;
      healthList1.Clear();
      for (int index = 0; index < Health.playerTeam.Count; ++index)
      {
        if ((UnityEngine.Object) Health.playerTeam[index] != (UnityEngine.Object) null)
          healthList1.Add(Health.playerTeam[index]);
      }
    }
    foreach (Health health in healthList1)
    {
      if (!((UnityEngine.Object) health == (UnityEngine.Object) null) && health.enabled && !health.invincible && !health.untouchable && !health.InanimateObject && (double) health.HP > 0.0 && !Projectile.Contains(health) && !ProjectileGhost.Contains(health) && (bool) (UnityEngine.Object) health && health.team == targetTeam)
        healthList2.Add(health);
    }
    Health closestTarget = (Health) null;
    foreach (Health health in healthList2)
    {
      if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null || (double) Vector3.Distance(health.transform.position, obj.position) < (double) Vector3.Distance(closestTarget.transform.position, obj.position))
        closestTarget = health;
    }
    return closestTarget;
  }

  public delegate void EnemyKilled(Enemy enemy);

  public delegate void Action();
}
