// Decompiled with JetBrains decompiler
// Type: UnitObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
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
  private Enemy enemyType;
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
  public float distanceBetweenDustClouds = 0.5f;
  private Seeker seeker;
  public static NNConstraint constraint = new NNConstraint();
  [HideInInspector]
  public StateMachine state;
  private Vector2 targetLocation;
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
  protected int currentWaypoint;
  [HideInInspector]
  public List<Vector3> pathToFollow;
  [HideInInspector]
  public float speed;
  private Vector2 positionLastFrame;
  [HideInInspector]
  public Health health;
  [HideInInspector]
  public Health TargetEnemy;
  public System.Action EndOfPath;
  private Coroutine knockRoutine;
  private EnemyModifier modifier;
  private float modifierTimer;
  public float EnemyModifierIconOffset = 2.25f;
  [SerializeField]
  private bool isBoss;
  private float distanceTravelledSinceLastDustCloud;
  public bool CanHaveModifier = true;
  public static List<UnitObject> Seperaters = new List<UnitObject>();
  private MeshRenderer[] childRenderers = new MeshRenderer[0];
  private Vector3 previousPosition = Vector3.zero;
  public bool UseFixedDirectionalPathing;
  private ModifierIcon modifierIcon;
  private Vector3 goToNoPathfinding;
  private Vector3 pointToCheck;
  protected CircleCollider2D ColliderRadius;
  [HideInInspector]
  public Rigidbody2D rb;
  public bool DisableForces;
  private Vector3 PrevPosition;
  public bool LockToGround = true;
  private RaycastHit LockToGroundHit;
  private Vector3 LockToGroundPosition;
  private Vector3 LockToGroundNewPosition;
  private bool dead;
  private float checkFrame;
  private Health cachedTarget;
  public static readonly int LeaderEncounterColorBoost = Shader.PropertyToID("_LeaderEncounterColorBoost");

  public Enemy EnemyType => this.enemyType;

  public bool UseDeltaTime { get; set; } = true;

  public bool IsBoss
  {
    get => (UnityEngine.Object) this.GetComponentInParent<MiniBossController>() != (UnityEngine.Object) null || this.isBoss;
  }

  public static event UnitObject.EnemyKilled OnEnemyKilled;

  public MeshRenderer[] ChildRenderers => this.childRenderers;

  protected virtual float timeStopMultiplier => 1f;

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
    if (this.health.team != Health.Team.Team2 || !this.CanHaveModifier || !((UnityEngine.Object) this.modifier == (UnityEngine.Object) null))
      return;
    this.modifier = EnemyModifier.GetModifier(DataManager.Instance.EnemiesInNextRoomHaveModifiers ? 1f : 0.0f);
    if (!(bool) (UnityEngine.Object) this.modifier)
      return;
    this.ForceSetModifier(this.modifier);
  }

  public void ForceSetModifier(EnemyModifier modifier)
  {
    this.modifier = modifier;
    this.modifierIcon = UnityEngine.Object.Instantiate<GameObject>(modifier.ModifierIcon, this.transform.position, Quaternion.identity).GetComponent<ModifierIcon>();
    GameObject gameObject = this.modifierIcon.gameObject;
    gameObject.transform.parent = this.transform;
    gameObject.transform.localPosition = Vector3.back * this.EnemyModifierIconOffset;
    this.modifierIcon.Init(modifier);
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
    ShowHPBar component = this.GetComponent<ShowHPBar>();
    if ((bool) (UnityEngine.Object) component)
      component.zOffset *= modifier.Scale;
    this.GetComponent<Health>().totalHP *= modifier.HealthMultiplier;
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
    this.health.OnHit += new Health.HitAction(this.OnHit);
    Color color = !((UnityEngine.Object) LightingManager.Instance != (UnityEngine.Object) null) || !LightingManager.Instance.inLeaderEncounter ? new Color(0.0f, 0.0f, 0.0f, 0.0f) : new Color(-0.25f, 0.0f, 0.25f, 0.0f);
    if (SceneManager.GetActiveScene().name == "Base Biome 1")
      color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
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
    UnitObject.Seperaters.Remove(this);
    this.seeker.pathCallback -= new OnPathDelegate(this.startPath);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
  }

  public void DoKnockBack(
    GameObject Attacker,
    float KnockbackModifier,
    float Duration,
    bool appendForce = true)
  {
    if ((UnityEngine.Object) this.rb == (UnityEngine.Object) null)
      return;
    if (this.knockRoutine != null)
      this.StopCoroutine(this.knockRoutine);
    if (!appendForce)
      this.rb.velocity = (Vector2) Vector3.zero;
    this.knockRoutine = this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), KnockbackModifier, Duration));
  }

  public void DoKnockBack(float angle, float KnockbackModifier, float Duration, bool appendForce = true)
  {
    if (this.knockRoutine != null)
      this.StopCoroutine(this.knockRoutine);
    if (!appendForce)
      this.rb.velocity = (Vector2) Vector3.zero;
    this.knockRoutine = this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(angle, KnockbackModifier, Duration));
  }

  private IEnumerator ApplyForceRoutine(float angle, float KnockbackModifier, float Duration)
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

  public void givePath(Vector3 targetLocation)
  {
    this.ClearPaths();
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
    {
      if (this.CheckSightBeforePath && this.CheckLineOfSight(targetLocation, Vector2.Distance((Vector2) this.transform.position, (Vector2) targetLocation)))
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
      Debug.Log((object) ("No need pathfinding " + (object) targetLocation));
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

  private IEnumerator DrawRay(Vector3 Position)
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
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return false;
    this.pointToCheck = pointToCheck;
    if ((UnityEngine.Object) this.ColliderRadius == (UnityEngine.Object) null)
      this.ColliderRadius = this.GetComponent<CircleCollider2D>();
    RaycastHit2D raycastHit2D1 = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D1.collider != (UnityEngine.Object) null && (UnityEngine.Object) raycastHit2D1.collider != (UnityEngine.Object) PlayerFarming.Instance.circleCollider2D)
      return false;
    float angle = Utils.GetAngle(this.transform.position, pointToCheck);
    RaycastHit2D raycastHit2D2 = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle + 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle + 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) null && (UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) PlayerFarming.Instance.circleCollider2D)
      return false;
    raycastHit2D2 = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle - 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle - 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    return !((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) null) || !((UnityEngine.Object) raycastHit2D2.collider != (UnityEngine.Object) PlayerFarming.Instance.circleCollider2D);
  }

  public bool CheckLineOfSight(float distance)
  {
    return !(bool) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (PlayerFarming.Instance.transform.position - this.transform.position).normalized, distance, (int) this.layerToCheck);
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
        Projectile.CreateProjectiles(5, this.health, new Vector3(this.transform.position.x, this.transform.position.y, 0.0f));
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

  protected void move()
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

  protected virtual void FixedUpdate()
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

  private void LateUpdate()
  {
    if ((this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing || this.state.CURRENT_STATE == StateMachine.State.DashAcrossIsland || this.state.CURRENT_STATE == StateMachine.State.Dodging) && this.emitDustClouds)
    {
      this.distanceTravelledSinceLastDustCloud += (this.transform.position - this.PrevPosition).magnitude;
      if ((double) this.distanceTravelledSinceLastDustCloud >= (double) this.distanceBetweenDustClouds)
      {
        this.distanceTravelledSinceLastDustCloud = 0.0f;
        if ((UnityEngine.Object) this.transform != (UnityEngine.Object) null && (UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
          BiomeConstants.Instance.EmitDustCloudParticles(this.transform.position);
      }
    }
    this.PrevPosition = this.transform.position;
    if (!this.LockToGround)
      return;
    this.LockToGroundPosition = this.transform.position + Vector3.back * 3f;
    if (Physics.Raycast(this.LockToGroundPosition, Vector3.forward, out this.LockToGroundHit, float.PositiveInfinity))
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

  public virtual void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.dead)
      return;
    if (this.modifier.HasModifier(EnemyModifier.ModifierType.DropPoison))
    {
      TrapPoison.CreatePoison(this.transform.position, 10, 0.5f, this.transform.parent);
      AudioManager.Instance.PlayOneShot("event:/player/poison_damage", this.transform.position);
    }
    if (this.modifier.HasModifier(EnemyModifier.ModifierType.DropBomb))
    {
      Bomb.CreateBomb(this.transform.position, this.health, this.transform.parent);
      AudioManager.Instance.PlayOneShot("event:/boss/spider/bomb_shoot", this.transform.position);
    }
    foreach (InventoryItem inventoryItem in TrinketManager.GetItemsToDrop())
      InventoryItem.Spawn((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity, this.transform.position);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.3f, 0.4f, false);
    if (AttackType != Health.AttackTypes.Projectile && AttackType != Health.AttackTypes.Poison)
      GameManager.GetInstance().HitStop(0.1f * this.timeStopMultiplier);
    DataManager.Instance.AddEnemyKilled(this.enemyType);
    UnitObject.EnemyKilled onEnemyKilled = UnitObject.OnEnemyKilled;
    if (onEnemyKilled != null)
      onEnemyKilled(this.enemyType);
    this.dead = true;
  }

  public virtual void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    CameraManager.instance.ShakeCameraForDuration(0.6f, 0.8f, 0.3f, false);
    if (this.modifier.HasModifier(EnemyModifier.ModifierType.DropPoison))
    {
      TrapPoison.CreatePoison(this.transform.position, 5, 0.1f, this.transform.parent);
      AudioManager.Instance.PlayOneShot("event:/player/poison_damage", this.transform.position);
    }
    if (AttackType == Health.AttackTypes.Projectile || AttackType == Health.AttackTypes.Poison)
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

  protected Health GetClosestTarget()
  {
    if ((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || BiomeGenerator.Instance.CurrentRoom == null || (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom == (UnityEngine.Object) null)
      return !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Instance.GoToAndStopping ? (Health) null : PlayerFarming.Instance.health;
    if ((double) Time.time == (double) this.checkFrame)
      return this.cachedTarget;
    Health.Team team = this.health.team == Health.Team.PlayerTeam ? Health.Team.Team2 : Health.Team.PlayerTeam;
    List<Health> healthList1 = new List<Health>((IEnumerable<Health>) Health.team2);
    List<Health> healthList2 = new List<Health>();
    if (team == Health.Team.PlayerTeam)
    {
      if ((bool) (UnityEngine.Object) PlayerFarming.Instance && Health.playerTeam.Count <= 1)
        return PlayerFarming.Instance.GoToAndStopping ? (Health) null : PlayerFarming.Instance.health;
      healthList1.Clear();
      for (int index = 0; index < Health.playerTeam.Count; ++index)
      {
        if ((UnityEngine.Object) Health.playerTeam[index] != (UnityEngine.Object) null)
          healthList1.Add(Health.playerTeam[index]);
      }
    }
    foreach (Health health in healthList1)
    {
      if (!((UnityEngine.Object) health == (UnityEngine.Object) null) && health.enabled && !health.invincible && !health.untouchable && !health.InanimateObject && (double) health.HP > 0.0 && (bool) (UnityEngine.Object) health && health.team == team)
        healthList2.Add(health);
    }
    Health closestTarget = (Health) null;
    foreach (Health health in healthList2)
    {
      if ((double) Vector3.Distance(health.transform.position, this.transform.position) <= (double) this.VisionRange && ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null || (double) Vector3.Distance(health.transform.position, this.transform.position) < (double) Vector3.Distance(closestTarget.transform.position, this.transform.position)))
        closestTarget = health;
    }
    this.checkFrame = Time.time;
    this.cachedTarget = closestTarget;
    return closestTarget;
  }

  public delegate void EnemyKilled(Enemy enemy);

  public delegate void Action();
}
