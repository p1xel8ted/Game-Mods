// Decompiled with JetBrains decompiler
// Type: Projectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Ara;
using FMOD.Studio;
using FMODUnity;
using MMBiomeGeneration;
using Spine.Unity;
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
public class Projectile : BaseMonoBehaviour, ISpellOwning
{
  public static List<Projectile> Projectiles = new List<Projectile>();
  public Transform ArrowImage;
  public Transform ArrowImageEmission;
  public bool IsProjectilesParent;
  public bool followDirection;
  public float Damage = 1f;
  public float Speed;
  public float SpeedMultiplier = 1f;
  public float Angle;
  public float LifeTime = 1f;
  public float Acceleration;
  public float Deceleration;
  public bool canParry = true;
  public bool destroyOnParry;
  public float timestamp;
  public float InvincibleTime;
  public float SinLength;
  public bool NoKnockback;
  public TrailRenderer LQ_Trail;
  public AraTrail Trail;
  public bool SpawnLavaOnHit;
  public GameObject lavaPrefab;
  public bool Explosive;
  public bool CanKnockBack = true;
  public bool ForceHomeInKnockback;
  public bool OverrideSpeedHomeIn;
  public float HomeInSpeed;
  public bool ReturnToSenderOnKnockback;
  public bool ModifyingZ;
  public int AllowedBounces;
  public bool Destroyable = true;
  public bool DestroyOnWallHit = true;
  public float ScreenShakeMultiplier = 1f;
  public bool UseDelay;
  public bool CollideOnlyTarget;
  public bool CollideOnlyTargets;
  public bool UseCurve;
  public AnimationCurve AnimationCurve;
  public CompositeCollider2D RoomBoundsCompositeCollider;
  public float physicsTimer;
  public float radius;
  public int layerCollisionMask;
  [CompilerGenerated]
  public SkeletonAnimation \u003CSpine\u003Ek__BackingField;
  public Health.Team _team = Health.Team.PlayerTeam;
  public float InitScale;
  public float Scale;
  public float ScaleSpeed;
  public List<Sprite> ChunksToSpawn;
  public List<Sprite> ValidParticleChunksToSpawn;
  public float Timer;
  public Health health;
  public Health Owner;
  public Health.AttackFlags AttackFlags;
  public int bouncesRemaining;
  public bool destroyed;
  public Rigidbody2D rb;
  public Vector3 startPos = Vector3.zero;
  public UnityEvent OnDestroyProjectile;
  public float originalSpeedMultiplier = 1f;
  [HideInInspector]
  public Projectile.CollisionEvent collisionEventQueue;
  public bool SetTargetToClosest;
  public bool homeInOnTarget;
  public bool temporaryHomeInOnTarget;
  public float turningSpeed = 1f;
  [HideInInspector]
  public float turningSpeedMultiplier = 1f;
  public float angleNoiseAmplitude;
  public float angleNoiseFrequency;
  public float AngleIncrement;
  public float DamageToNeutral = 10f;
  public float NeutralSplashRadius;
  public float HitFxScaleMultiplier = 1f;
  public bool Seperate;
  public Health targetObject;
  public List<Collider2D> TargetObjects = new List<Collider2D>();
  public bool PulseMove;
  public bool EmitHitParticle = true;
  [EventRef]
  public string OnSpawnSoundPath = string.Empty;
  [EventRef]
  public string LoopedSoundPath = string.Empty;
  [EventRef]
  public string OnKnockbackSoundPath = string.Empty;
  public EventInstance LoopedSound;
  public float spawnTimestamp;
  public GameObject hitPrefab;
  public GameObject hitParticleObj;
  public Collider2D collider;
  public LayerMask bounceMask;
  public LayerMask unitMask;
  public int layerObstacles;
  public int layerIsland;
  public Collider2D[] hitsBuffer = new Collider2D[100];
  public static GameObject projectilePrefab;
  public Vector2 Seperator = Vector2.zero;
  public float SeperationRadius = 0.6f;
  [HideInInspector]
  public float SeparationCooldownTime;
  public float PhysicsDelay = 0.1f;
  public static GameObject loadedArrow;
  public static GameObject loadedPlayerArrow;
  public CircleCollider2D circleCollider2D;
  public bool initialized;
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public Projectile.OnHitUnit onHitOwner;
  public Projectile.OnHitUnit onHitPlayer;
  public Projectile.OnKnockedBack onKnockedBack;
  [HideInInspector]
  public bool pulseMove;
  public SkeletonAnimation parentSpine;
  public Coroutine recycleRoutine;
  public GameManager gameManager;
  public bool disableOnce;
  public bool useLowQualityAnimation;
  public static Material material_low;
  public static Sprite flySprite;
  public static AsyncOperationHandle<Material> addrHandle;
  [CompilerGenerated]
  public bool \u003CKnockedBack\u003Ek__BackingField;
  public float ZWave;
  public float ZWaveSize;
  public float ZWaveSpeed;
  public bool stoppedLoop;
  public Vector2 lowestPoint;
  public Vector2 highestPoint;
  public static Dictionary<int, Projectile> ProjectileComponents = new Dictionary<int, Projectile>();
  public static Dictionary<int, Health> HealthComponents = new Dictionary<int, Health>();
  public bool IgnoreIsland;

  public SkeletonAnimation Spine
  {
    get => this.\u003CSpine\u003Ek__BackingField;
    set => this.\u003CSpine\u003Ek__BackingField = value;
  }

  public Health.Team team
  {
    get => this._team;
    set => this._team = value;
  }

  public CircleCollider2D CircleCollider2D => this.circleCollider2D;

  public float SpineTimeScale
  {
    get => !(bool) (UnityEngine.Object) this.parentSpine ? 1f : this.parentSpine.timeScale;
  }

  public static void CreateProjectiles(int amount, Health owner, Vector3 position, bool explosive = false)
  {
    Transform parent = owner.transform.parent;
    if ((UnityEngine.Object) Projectile.projectilePrefab == (UnityEngine.Object) null)
    {
      Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Weapons/ArrowTurrets.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        Projectile.loadedAddressableAssets.Add(obj);
        Projectile.projectilePrefab = obj.Result;
        if (ControlUtilities.GetPlatformFromUnifyPlatform() == Platform.Switch)
        {
          SpriteRenderer component = Projectile.projectilePrefab.GetComponent<Projectile>().ArrowImage.Find("Bullet_Core").GetComponent<SpriteRenderer>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.material != (UnityEngine.Object) null)
          {
            component.material.SetFloat("_OPNOISEA", 0.0f);
            component.material.SetFloat("_OPNOISEB", 0.0f);
          }
        }
        Projectile.CreateProjectiles(amount, owner, position, explosive);
      });
    }
    else
    {
      float num1 = 0.0f;
      int num2 = -1;
      while (++num2 < amount)
      {
        Projectile component = ObjectPool.Spawn(Projectile.projectilePrefab, parent).GetComponent<Projectile>();
        component.transform.position = position;
        component.Angle = num1;
        component.team = Health.Team.Team2;
        component.Speed = 4f;
        component.Owner = owner;
        component.Explosive = explosive;
        if ((UnityEngine.Object) owner != (UnityEngine.Object) null)
          component.SetOwner(owner.gameObject);
        num1 += 360f / (float) amount;
        if (owner.team != Health.Team.PlayerTeam)
          component.Speed *= DataManager.Instance.ProjectileMoveSpeedMultiplier;
      }
    }
  }

  public static void CreatePlayerProjectiles(
    int amount,
    Health owner,
    Vector3 position,
    float speed = 4f,
    float damage = 1f,
    float angleOffset = 0.0f,
    bool explosive = false,
    Action<List<Projectile>> callback = null)
  {
    Projectile.CreatePlayerProjectiles(amount, owner, position, "Assets/Prefabs/Enemies/Weapons/ArrowPlayer.prefab", damage, speed, angleOffset, explosive, callback);
  }

  public static void CreatePlayerProjectiles(
    int amount,
    Health owner,
    Vector3 position,
    string prefabPath,
    float speed = 4f,
    float damage = 1f,
    float angleOffset = 0.0f,
    bool explosive = false,
    Action<List<Projectile>> callback = null,
    Health.AttackFlags attackFlags = (Health.AttackFlags) 0)
  {
    if ((UnityEngine.Object) Projectile.loadedPlayerArrow == (UnityEngine.Object) null)
    {
      Addressables.LoadAssetAsync<GameObject>((object) prefabPath).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        Projectile.loadedAddressableAssets.Add(obj);
        Projectile.loadedPlayerArrow = obj.Result;
        Projectile.loadedArrow = obj.Result;
        Projectile.CreateProjectiles(amount, owner, position, speed, damage, angleOffset, explosive, callback);
      });
    }
    else
    {
      float num1 = angleOffset;
      int num2 = -1;
      List<Projectile> projectileList = new List<Projectile>();
      while (++num2 < amount)
      {
        Projectile component = ObjectPool.Spawn(Projectile.loadedPlayerArrow).GetComponent<Projectile>();
        component.transform.position = position;
        component.Angle = num1;
        component.team = owner.team;
        component.Speed = speed;
        component.Owner = owner;
        component.Damage = damage;
        component.Explosive = explosive;
        component.AttackFlags = attackFlags;
        component.transform.parent = (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null ? BiomeGenerator.Instance.CurrentRoom.generateRoom.transform : (Transform) null;
        num1 += Utils.Repeat(360f / (float) amount, 360f);
        if (owner.team != Health.Team.PlayerTeam)
          component.Speed *= DataManager.Instance.ProjectileMoveSpeedMultiplier;
        projectileList.Add(component);
      }
      Action<List<Projectile>> action = callback;
      if (action == null)
        return;
      action(projectileList);
    }
  }

  public static void CreateProjectiles(
    int amount,
    Health owner,
    Vector3 position,
    float speed = 4f,
    float damage = 1f,
    float angleOffset = 0.0f,
    bool explosive = false,
    Action<List<Projectile>> callback = null)
  {
    if ((UnityEngine.Object) Projectile.loadedArrow == (UnityEngine.Object) null)
    {
      Addressables.LoadAssetAsync<GameObject>((object) "Assets/Prefabs/Enemies/Weapons/ArrowTurrets.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        Projectile.loadedAddressableAssets.Add(obj);
        Projectile.loadedArrow = obj.Result;
        Projectile.CreateProjectiles(amount, owner, position, speed, damage, angleOffset, explosive, callback);
      });
    }
    else
    {
      float num1 = angleOffset;
      int num2 = -1;
      List<Projectile> projectileList = new List<Projectile>();
      while (++num2 < amount)
      {
        Projectile component = ObjectPool.Spawn(Projectile.loadedArrow).GetComponent<Projectile>();
        component.transform.position = position;
        component.transform.parent = owner.transform.parent;
        component.Angle = num1;
        component.team = owner.team;
        component.Speed = speed;
        component.Owner = owner;
        component.Damage = damage;
        component.Explosive = explosive;
        if ((UnityEngine.Object) owner != (UnityEngine.Object) null)
          component.SetOwner(owner.gameObject);
        num1 += Utils.Repeat(360f / (float) amount, 360f);
        if (owner.team != Health.Team.PlayerTeam)
          component.Speed *= DataManager.Instance.ProjectileMoveSpeedMultiplier;
        projectileList.Add(component);
      }
      Action<List<Projectile>> action = callback;
      if (action == null)
        return;
      action(projectileList);
    }
  }

  public void Awake()
  {
    this.originalSpeedMultiplier = this.SpeedMultiplier;
    this.gameManager = GameManager.GetInstance();
    this.collider = this.GetComponentInChildren<Collider2D>();
    this.circleCollider2D = this.GetComponent<CircleCollider2D>();
    this.layerObstacles = LayerMask.NameToLayer("Obstacles");
    this.layerIsland = LayerMask.NameToLayer("Island");
    Rigidbody2D component = this.GetComponent<Rigidbody2D>();
    this.radius = (UnityEngine.Object) this.circleCollider2D != (UnityEngine.Object) null ? this.circleCollider2D.radius : 0.5f;
    if ((UnityEngine.Object) this.circleCollider2D != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.circleCollider2D);
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) component);
    this.layerCollisionMask = Physics2D.GetLayerCollisionMask(this.gameObject.layer);
  }

  public void Start()
  {
    this.Scale = this.InitScale * 2f;
    if ((bool) (UnityEngine.Object) this.health)
      this.health.team = this.team;
    this.Spine = this.GetComponentInChildren<SkeletonAnimation>();
    this.bounceMask = (LayerMask) ((int) this.bounceMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.bounceMask = (LayerMask) ((int) this.bounceMask | 1 << LayerMask.NameToLayer("Island"));
    this.unitMask = (LayerMask) ((int) this.unitMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.unitMask = (LayerMask) ((int) this.unitMask | 1 << LayerMask.NameToLayer("Units"));
    this.PhysicsDelay += UnityEngine.Random.Range(0.0f, this.PhysicsDelay);
    this.initialized = true;
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    this.Acceleration = 0.0f;
    this.Deceleration = 0.0f;
    this.turningSpeedMultiplier = 1f;
    this.pulseMove = this.PulseMove;
    this.KnockedBack = false;
    this.temporaryHomeInOnTarget = false;
    this.team = Health.Team.Team2;
    this.SpeedMultiplier = this.originalSpeedMultiplier;
    if ((bool) (UnityEngine.Object) this.health)
    {
      this.health.OnHit -= new Health.HitAction(this.OnHit);
      this.health.team = Health.Team.Team2;
    }
    Projectile.Projectiles.Remove(this);
    if ((UnityEngine.Object) this.hitParticleObj != (UnityEngine.Object) null)
      this.hitParticleObj.Recycle();
    if (!this.initialized || this.destroyed || !((UnityEngine.Object) this.transform.parent == (UnityEngine.Object) null) && (!((UnityEngine.Object) this.transform.parent.GetComponent<Projectile>() == (UnityEngine.Object) null) || !((UnityEngine.Object) this.transform.parent.GetComponent<ObjectPool>() == (UnityEngine.Object) null)))
      return;
    GameManager.GetInstance()?.StartCoroutine((IEnumerator) this.RecycleIE(1f));
  }

  public virtual void OnEnable()
  {
    if ((bool) (UnityEngine.Object) this.health)
      this.health.OnHit += new Health.HitAction(this.OnHit);
    this.LoopedSound = AudioManager.Instance.CreateLoop(this.LoopedSoundPath, this.gameObject, true);
    if (!string.IsNullOrEmpty(this.OnSpawnSoundPath))
      AudioManager.Instance.PlayOneShot(this.OnSpawnSoundPath, this.gameObject);
    if (!Projectile.Projectiles.Contains(this))
      Projectile.Projectiles.Add(this);
    this.timestamp = GameManager.GetInstance().CurrentTime;
    this.Timer = 0.0f;
    this.destroyed = false;
    this.Explosive = false;
    this.Damage = 1f;
    GameManager.GetInstance().WaitForSeconds(Time.unscaledDeltaTime, (System.Action) (() =>
    {
      if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
        return;
      this.startPos = this.transform.position;
    }));
    this.spawnTimestamp = GameManager.GetInstance().CurrentTime;
    if (this.recycleRoutine != null)
    {
      this.StopCoroutine(this.recycleRoutine);
      this.recycleRoutine = (Coroutine) null;
    }
    this.bouncesRemaining = this.AllowedBounces;
    if ((bool) (UnityEngine.Object) this.ArrowImage)
      this.ArrowImage.gameObject.SetActive(true);
    if ((bool) (UnityEngine.Object) this.Trail)
    {
      this.Trail.Clear();
      this.Trail.enabled = true;
      this.Trail.emit = true;
    }
    if ((bool) (UnityEngine.Object) this.collider)
      this.collider.enabled = true;
    if (SettingsManager.Settings.Graphics.EnvironmentDetail == GraphicsSettingsUtilities.LowPreset.EnvironmentDetail)
      this.gameObject.layer = LayerMask.NameToLayer("ObstaclesAndPlayer");
    else
      this.gameObject.layer = LayerMask.NameToLayer("VFX");
    this.ValidParticleChunksToSpawn = this.ChunksToSpawn.Where<Sprite>((Func<Sprite, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<Sprite>();
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null) || this.team == Health.Team.PlayerTeam)
      return;
    GameObject gameObject = PlayerFarming.Instance.gameObject;
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    this.targetObject = gameObject.GetComponent<Health>();
  }

  public void UseLowQualityAnimation()
  {
    if (this.useLowQualityAnimation)
      return;
    this.GetComponentInChildren<CircleCollider2D>().enabled = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.ArrowImage.gameObject);
    if ((UnityEngine.Object) Projectile.flySprite == (UnityEngine.Object) null)
      Projectile.flySprite = Resources.Load<Sprite>("PerformanceMode/fly_low");
    if ((UnityEngine.Object) Projectile.material_low == (UnityEngine.Object) null)
    {
      Projectile.addrHandle = Addressables.LoadAssetAsync<Material>((object) "Assets/Art/Shaders/AmplifyShaderEditor/Environment/Sprites-Default.mat");
      Projectile.addrHandle.WaitForCompletion();
      Projectile.material_low = Projectile.addrHandle.Result;
    }
    GameObject gameObject = new GameObject("spine_low");
    SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    spriteRenderer.sprite = Projectile.flySprite;
    spriteRenderer.material = Projectile.material_low;
    gameObject.transform.Rotate(new Vector3(-60f, 0.0f, 0.0f));
    gameObject.transform.parent = this.transform;
    gameObject.transform.localPosition = new Vector3(0.0f, 0.022f, -0.467f);
    this.ArrowImage = gameObject.transform;
    this.useLowQualityAnimation = true;
  }

  public static void UnloadResources()
  {
    Projectile.material_low = (Material) null;
    Projectile.flySprite = (Sprite) null;
    if (!Projectile.addrHandle.IsValid())
      return;
    Addressables.Release<Material>(Projectile.addrHandle);
  }

  public IEnumerator DelayTrailByFrame()
  {
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    this.LQ_Trail.emitting = true;
  }

  public bool KnockedBack
  {
    get => this.\u003CKnockedBack\u003Ek__BackingField;
    set => this.\u003CKnockedBack\u003Ek__BackingField = value;
  }

  public virtual void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!this.canParry || (double) GameManager.GetInstance().CurrentTime < (double) this.spawnTimestamp + (double) this.InvincibleTime || !this.CanKnockBack)
      return;
    PlayerFarming component = Attacker.GetComponent<PlayerFarming>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.KnockedBack = true;
    Projectile.OnKnockedBack onKnockedBack = this.onKnockedBack;
    if (onKnockedBack != null)
      onKnockedBack();
    this.team = Health.Team.PlayerTeam;
    this.health.team = Health.Team.PlayerTeam;
    if (this.destroyOnParry)
    {
      if (AttackType == Health.AttackTypes.Melee && !this.destroyed)
        this.EmitParticle();
      AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
      CameraManager.shakeCamera(0.1f);
      this.DestroyProjectile();
    }
    else if (AttackType == Health.AttackTypes.Projectile)
    {
      CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, AttackLocation));
      Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 3f, attackFlags: this.AttackFlags);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
      this.DestroyProjectile();
    }
    else
    {
      GameManager.GetInstance().HitStop();
      CameraManager.shakeCamera(0.2f, (float) UnityEngine.Random.Range(0, 360));
      if (!this.destroyed)
        this.EmitParticle();
      this.Angle += 180f;
      this.Speed = 15f;
      this.Damage *= 3f;
      this.LifeTime = 3f;
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        this.ForgiveCollisionWithPlayer();
      if ((UnityEngine.Object) this.Owner != (UnityEngine.Object) null)
      {
        this.Angle = Utils.GetAngle(this.transform.position, this.Owner.transform.position);
        if (!this.ForceHomeInKnockback)
          return;
        if (!string.IsNullOrEmpty(this.OnKnockbackSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnKnockbackSoundPath, this.gameObject);
        this.SetTarget(this.Owner);
        this.turningSpeed = float.MaxValue;
        this.CollideOnlyTarget = true;
        if (!this.OverrideSpeedHomeIn)
          return;
        this.Speed = this.HomeInSpeed;
      }
      else
      {
        Health health = (Health) null;
        float num1 = float.MaxValue;
        float num2 = 0.0f;
        float num3 = 90f;
        if (!string.IsNullOrEmpty(this.OnKnockbackSoundPath))
          AudioManager.Instance.PlayOneShot(this.OnKnockbackSoundPath, this.gameObject);
        foreach (Health allUnit in Health.allUnits)
        {
          float angle = Utils.GetAngle(this.transform.position, allUnit.transform.position);
          float num4 = Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.transform.position);
          if ((UnityEngine.Object) allUnit != (UnityEngine.Object) this.health && !allUnit.InanimateObject && allUnit.team == Health.Team.Team2 && (double) num4 < 8.0 && (double) num4 < (double) num1 && (double) Mathf.Abs(this.Angle - angle) < 180.0 && (double) angle > (double) this.Angle - (double) num3 && (double) angle < (double) this.Angle + (double) num3)
          {
            health = allUnit;
            num1 = num4;
            num2 = angle;
          }
        }
        if (!((UnityEngine.Object) health != (UnityEngine.Object) null))
          return;
        this.Angle = num2;
      }
    }
  }

  public void EmitParticle(float scaleMultiplier = 1f)
  {
    if (!((UnityEngine.Object) this.ArrowImage != (UnityEngine.Object) null) || !this.EmitHitParticle)
      return;
    BiomeConstants.Instance.EmitHitFX_BlockedRedSmall(this.ArrowImage.position, Quaternion.identity, Vector3.one * scaleMultiplier * this.HitFxScaleMultiplier);
  }

  public void Deflect()
  {
  }

  public void InitializeRoomBounds(Vector2 lowestP, Vector2 highestP)
  {
    this.lowestPoint = lowestP;
    this.highestPoint = highestP;
  }

  public void SetParentSpine(SkeletonAnimation spine) => this.parentSpine = spine;

  public void FixedUpdate()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || this.destroyed)
      return;
    bool flag = false;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      if ((double) PlayerFarming.players[index].health.HP > 0.0)
      {
        flag = true;
        break;
      }
    }
    if (!flag && !this.stoppedLoop)
    {
      AudioManager.Instance.StopLoop(this.LoopedSound);
      this.stoppedLoop = true;
    }
    float time = Time.time;
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    float fixedDeltaTime = Time.fixedDeltaTime;
    this.ScaleSpeed += (float) (((double) this.InitScale - (double) this.Scale) * 0.40000000596046448);
    this.Scale += (this.ScaleSpeed *= 0.6f);
    this.Speed += this.Acceleration * fixedDeltaTime * this.SpineTimeScale;
    this.Speed -= this.Deceleration * fixedDeltaTime * this.SpineTimeScale;
    if (this.PulseMove)
    {
      if ((double) this.Speed <= 0.0 && (double) this.Deceleration > 0.0)
      {
        this.Acceleration = this.Deceleration;
        this.Deceleration = 0.0f;
      }
      else if ((double) this.Acceleration != 0.0 && (double) this.Speed >= (double) this.Acceleration)
      {
        this.Deceleration = this.Acceleration;
        this.Acceleration = 0.0f;
      }
    }
    this.Seperator = Vector2.zero;
    if (this.Seperate && (double) this.SeparationCooldownTime < (double) time)
    {
      for (int index = 0; index < Projectile.Projectiles.Count; ++index)
      {
        Projectile projectile = Projectile.Projectiles[index];
        if (!((UnityEngine.Object) projectile == (UnityEngine.Object) null) && projectile.GetInstanceID() != this.GetInstanceID() && projectile.Seperate)
        {
          float distanceBetween = this.MagnitudeFindDistanceBetween(projectile.transform.position, this.transform.position);
          if ((double) distanceBetween < (double) this.SeperationRadius)
          {
            float angle = Utils.GetAngle(projectile.transform.position, this.transform.position);
            float num1 = (float) (((double) this.SeperationRadius - (double) distanceBetween) / 2.0 * (double) Mathf.Cos(angle * ((float) Math.PI / 180f)) * (double) unscaledDeltaTime * 60.0) * this.SpineTimeScale;
            float num2 = (float) (((double) this.SeperationRadius - (double) distanceBetween) / 2.0 * (double) Mathf.Sin(angle * ((float) Math.PI / 180f)) * (double) unscaledDeltaTime * 60.0) * this.SpineTimeScale;
            this.Seperator.x += num1;
            this.Seperator.y += num2;
            projectile.Seperator.x -= num1;
            projectile.Seperator.y -= num2;
          }
        }
      }
      this.SeparationCooldownTime = time + 0.5f;
    }
    if ((this.homeInOnTarget || this.temporaryHomeInOnTarget) && (!this.KnockedBack || this.ForceHomeInKnockback))
    {
      if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null && this.team != Health.Team.PlayerTeam)
      {
        PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(this.transform.position);
        if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null)
          this.targetObject = (Health) closestPlayer.health;
      }
      this.TurnTowardsTarget();
    }
    this.Angle = Utils.Repeat(this.Angle + this.AngleIncrement, 360f);
    float angle1 = this.Angle;
    if ((double) this.angleNoiseAmplitude > 0.0 && (double) this.angleNoiseFrequency > 0.0)
      angle1 += (Mathf.PerlinNoise(this.gameManager.TimeSince(this.timestamp) * this.angleNoiseFrequency, 0.0f) - 0.5f) * this.angleNoiseAmplitude;
    float f = angle1 * ((float) Math.PI / 180f);
    Vector2 vector2_1 = (Vector2) (new Vector3(this.Speed * Mathf.Cos(f), this.Speed * Mathf.Sin(f), 0.0f) * fixedDeltaTime * this.SpineTimeScale);
    if ((double) this.SinLength != 0.0)
    {
      Vector2 vector2_2 = Utils.DegreeToVector2(angle1 + 90f).normalized * Mathf.PingPong(time, this.SinLength);
    }
    if (this.followDirection)
      this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle1);
    double speedMultiplier = (double) this.SpeedMultiplier;
    Vector2 vector2_3 = vector2_1 * (float) speedMultiplier + this.Seperator;
    if (!PlayerRelic.TimeFrozen || this.team == Health.Team.PlayerTeam)
      this.transform.position = this.transform.position + new Vector3(vector2_3.x, vector2_3.y, 0.0f);
    if (!this.useLowQualityAnimation && this.ModifyingZ)
      this.Spine.transform.localPosition = Vector3.Lerp(this.Spine.transform.localPosition, Vector3.zero, fixedDeltaTime * this.SpineTimeScale);
    if (this.UseCurve)
      this.ArrowImage.transform.localPosition = new Vector3(this.ArrowImage.transform.localPosition.x, this.ArrowImage.transform.localPosition.y, this.AnimationCurve.Evaluate(this.Timer / this.LifeTime) * -1f);
    if (this.collisionEventQueue != null && (UnityEngine.Object) this.gameManager != (UnityEngine.Object) null)
    {
      if ((double) this.gameManager.UnscaledTimeSince(this.collisionEventQueue.UnscaledTimestamp) >= (double) Health.DealDamageForgivenessWindow && (double) this.gameManager.UnscaledTimeSince(this.collisionEventQueue.UnscaledTimestamp) >= (double) Health.DealDamageForgivenessWindow)
        this.OnCollisionWithPlayer(this.collisionEventQueue.TargetHealth, true);
    }
    else if (!PlayerRelic.TimeFrozen && (double) (this.Timer += fixedDeltaTime * this.SpineTimeScale) > (double) this.LifeTime && !this.destroyed && this.Destroyable)
      this.EndOfLife();
    if ((UnityEngine.Object) this.ArrowImageEmission != (UnityEngine.Object) null)
      this.ArrowImageEmission.position = this.startPos;
    if (this.IsProjectilesParent)
      return;
    if (this.CollideOnlyTarget)
    {
      if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null || !this.targetObject.enabled || (double) this.MagnitudeFindDistanceBetween(this.targetObject.transform.position, this.transform.position) > (double) this.radius * 2.0)
        return;
      CircleCollider2D circleCollider2D = this.targetObject.CircleCollider2D;
      if (!((UnityEngine.Object) circleCollider2D != (UnityEngine.Object) null))
        return;
      this.OnRayEnter2D((Collider2D) circleCollider2D);
    }
    else if (this.CollideOnlyTargets)
    {
      for (int index = 0; index < this.TargetObjects.Count; ++index)
      {
        if (!((UnityEngine.Object) this.TargetObjects[index] == (UnityEngine.Object) null) && (double) this.MagnitudeFindDistanceBetween(this.TargetObjects[index].transform.position, this.transform.position) <= (double) this.radius * 2.0 && (UnityEngine.Object) this.TargetObjects[index] != (UnityEngine.Object) null)
        {
          this.OnRayEnter2D(this.TargetObjects[index]);
          return;
        }
      }
      Vector2 position = (Vector2) this.transform.position;
      if ((double) position.x > (double) this.lowestPoint.x && (double) position.x < (double) this.highestPoint.x && (double) position.y > (double) this.lowestPoint.y && (double) position.y < (double) this.highestPoint.y)
        return;
      this.OnRayEnter2D((Collider2D) this.RoomBoundsCompositeCollider);
    }
    else
    {
      if (this.UseDelay && (!this.UseDelay || (double) (this.physicsTimer -= fixedDeltaTime * this.SpineTimeScale) > 0.0))
        return;
      this.physicsTimer = this.PhysicsDelay;
      int num = Physics2D.OverlapCircleNonAlloc((Vector2) this.transform.position, this.radius, this.hitsBuffer, this.layerCollisionMask);
      if (this.hitsBuffer == null || num <= 0)
        return;
      for (int index = 0; index < num; ++index)
        this.OnRayEnter2D(this.hitsBuffer[index]);
    }
  }

  public virtual void EndOfLife()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    if (this.Explosive)
    {
      Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 4f, 1f, 1f, attackFlags: this.AttackFlags);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/explosive_shot", this.transform.position);
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hitwall", this.transform.position);
      if ((UnityEngine.Object) this.ArrowImage != (UnityEngine.Object) null)
        this.EmitParticle(0.5f);
    }
    this.DestroyProjectile();
  }

  public void SetTarget(Health go)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return;
    this.targetObject = go;
  }

  public Health GetTarget() => this.targetObject;

  public bool IsAttachedToProjectileTrap()
  {
    return (UnityEngine.Object) this.health != (UnityEngine.Object) null && this.health.ProjectileTrap != null;
  }

  public void TurnTowardsTarget()
  {
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null || !this.targetObject.enabled)
      return;
    Vector3 position1 = this.transform.position;
    Vector3 position2 = this.targetObject.transform.position;
    if ((double) this.MagnitudeFindDistanceBetween(position1, position2) <= 0.25)
      return;
    float num = this.turningSpeed * this.turningSpeedMultiplier;
    if ((double) Math.Abs(this.Angle - Utils.GetAngle(position1, position2)) > 180.0)
      num /= 2f;
    this.Angle = Mathf.LerpAngle(this.Angle, Utils.GetAngle(position1, position2), Time.fixedDeltaTime * num * this.SpineTimeScale);
  }

  public virtual void OnCollisionWithPlayer(Health targetHealth, bool collideImmediately = false)
  {
    if (this.destroyed)
      return;
    if (!collideImmediately)
    {
      if (this.collisionEventQueue != null)
        return;
      this.collisionEventQueue = new Projectile.CollisionEvent(Time.unscaledTime, targetHealth);
    }
    else
    {
      if ((UnityEngine.Object) targetHealth != (UnityEngine.Object) null && targetHealth.state.CURRENT_STATE != StateMachine.State.Dodging)
      {
        CameraManager.shakeCamera(0.5f, Utils.GetAngle(this.transform.position, targetHealth.transform.position));
        AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
        if (this.SpawnLavaOnHit)
          TrapLava.CreateLava(this.lavaPrefab, this.transform.position, this.transform.parent, this.Owner);
        Projectile.OnHitUnit onHitPlayer = this.onHitPlayer;
        if (onHitPlayer != null)
          onHitPlayer(this);
        this.DestroyProjectile();
      }
      if (!collideImmediately)
        return;
      this.collisionEventQueue = (Projectile.CollisionEvent) null;
    }
  }

  public void ForgiveCollisionWithPlayer()
  {
    if (this.collisionEventQueue != null)
      Debug.Log((object) "Projectile collision forgiven!");
    this.collisionEventQueue = (Projectile.CollisionEvent) null;
  }

  public T GetCachedComponent<T>(
    Dictionary<int, T> dictionary,
    int objectID,
    GameObject cachedCollider)
    where T : MonoBehaviour
  {
    T obj1 = default (T);
    T obj2;
    T cachedComponent;
    if (dictionary.TryGetValue(objectID, out obj2))
    {
      cachedComponent = obj2;
    }
    else
    {
      cachedComponent = cachedCollider.GetComponent<T>();
      dictionary.Add(objectID, cachedComponent);
    }
    return cachedComponent;
  }

  public static void CleanCache()
  {
    Projectile.ProjectileComponents.Clear();
    Projectile.HealthComponents.Clear();
  }

  public virtual void OnRayEnter2D(Collider2D collider)
  {
    int instanceId = collider.gameObject.GetInstanceID();
    Projectile cachedComponent1 = this.GetCachedComponent<Projectile>(Projectile.ProjectileComponents, instanceId, collider.gameObject);
    if (this.destroyed || (UnityEngine.Object) collider == (UnityEngine.Object) null || (UnityEngine.Object) cachedComponent1 != (UnityEngine.Object) null || this.destroyed)
      return;
    Health cachedComponent2 = this.GetCachedComponent<Health>(Projectile.HealthComponents, instanceId, collider.gameObject);
    if ((UnityEngine.Object) cachedComponent2 != (UnityEngine.Object) null && cachedComponent2.enabled && (UnityEngine.Object) cachedComponent2 != (UnityEngine.Object) this.health && (cachedComponent2.team != this.team || cachedComponent2.IsCharmedEnemy) && !cachedComponent2.untouchable && !cachedComponent2.invincible && !cachedComponent2.IgnoreProjectiles && ((UnityEngine.Object) cachedComponent2.state == (UnityEngine.Object) null || cachedComponent2.state.CURRENT_STATE != StateMachine.State.Dodging))
    {
      if ((cachedComponent2.team == Health.Team.Neutral || !((UnityEngine.Object) this.ArrowImage == (UnityEngine.Object) null) && !this.ArrowImage.gameObject.activeSelf || cachedComponent2.invincible || cachedComponent2.CompareTag("ProjectileIgnore")) && (cachedComponent2.team != Health.Team.Neutral || (double) this.DamageToNeutral <= 0.0 || cachedComponent2.CompareTag("BreakableDecoration")) || (UnityEngine.Object) cachedComponent2 == (UnityEngine.Object) this.Owner && cachedComponent2.IsCharmed || cachedComponent2.isPlayer && TrinketManager.HasTrinket(TarotCards.Card.ImmuneToTraps, PlayerFarming.GetPlayerFarmingComponent(cachedComponent2.gameObject)) && this.IsAttachedToProjectileTrap())
        return;
      bool flag = cachedComponent2.DealDamage(cachedComponent2.team == Health.Team.Neutral ? this.DamageToNeutral : this.Damage, this.gameObject, this.transform.position, AttackType: this.NoKnockback ? Health.AttackTypes.NoKnockBack : Health.AttackTypes.Projectile, AttackFlags: this.AttackFlags);
      if (cachedComponent2.isPlayer)
      {
        this.OnCollisionWithPlayer(cachedComponent2);
      }
      else
      {
        if ((UnityEngine.Object) collider?.gameObject == (UnityEngine.Object) this.Owner?.gameObject && (UnityEngine.Object) this.targetObject?.gameObject == (UnityEngine.Object) this.Owner?.gameObject)
        {
          Projectile.OnHitUnit onHitOwner = this.onHitOwner;
          if (onHitOwner != null)
            onHitOwner(this);
        }
        if (!flag)
        {
          CameraManager.shakeCamera(0.2f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, cachedComponent2.transform.position));
          this.SpawnChunks(collider.transform.position);
        }
        else
          CameraManager.shakeCamera(0.5f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, cachedComponent2.transform.position));
        if (cachedComponent2.team == Health.Team.Neutral && (double) this.NeutralSplashRadius > 0.0)
        {
          foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.NeutralSplashRadius, (int) this.unitMask))
          {
            Health component2 = component1.GetComponent<Health>();
            if ((bool) (UnityEngine.Object) component2 && component2.team == Health.Team.Neutral)
              component2.DealDamage(this.DamageToNeutral, this.gameObject, this.transform.position, AttackType: this.NoKnockback ? Health.AttackTypes.NoKnockBack : Health.AttackTypes.Projectile, AttackFlags: this.AttackFlags);
          }
        }
        if (this.KnockedBack)
        {
          Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 3f, attackFlags: this.AttackFlags);
        }
        else
        {
          if (this.Explosive)
            Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 4f, 1f, 1f, attackFlags: this.AttackFlags);
          if (this.SpawnLavaOnHit)
            TrapLava.CreateLava(this.lavaPrefab, this.transform.position, this.transform.parent, this.Owner);
        }
        if (!this.Explosive)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
        else
          AudioManager.Instance.PlayOneShot("event:/player/Curses/explosive_shot", this.transform.position);
        if ((bool) (UnityEngine.Object) this.health && this.health.team == Health.Team.PlayerTeam && (bool) (UnityEngine.Object) collider && (UnityEngine.Object) collider.GetComponentInParent<Projectile>() != (UnityEngine.Object) null)
        {
          foreach (Component component in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.NeutralSplashRadius))
          {
            Projectile componentInParent = component.GetComponentInParent<Projectile>();
            if ((bool) (UnityEngine.Object) componentInParent)
              componentInParent.DestroyProjectile();
          }
        }
        if (this.IsAttachedToProjectileTrap())
          return;
        this.DestroyProjectile();
      }
    }
    else
    {
      if (!((UnityEngine.Object) collider != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.Owner != (UnityEngine.Object) null && (UnityEngine.Object) this.targetObject != (UnityEngine.Object) null && (UnityEngine.Object) collider.gameObject == (UnityEngine.Object) this.Owner.gameObject && (UnityEngine.Object) this.targetObject.gameObject == (UnityEngine.Object) this.Owner.gameObject)
      {
        Projectile.OnHitUnit onHitOwner = this.onHitOwner;
        if (onHitOwner != null)
          onHitOwner(this);
      }
      if (this.IgnoreIsland || collider.gameObject.layer != this.layerObstacles && collider.gameObject.layer != this.layerIsland)
        return;
      if (this.bouncesRemaining <= 0)
      {
        if (!this.DestroyOnWallHit || this.IsAttachedToProjectileTrap())
          return;
        if (!this.CollideOnlyTargets || !SettingsManager.Settings.Game.PerformanceMode)
        {
          this.EmitParticle();
          if ((bool) (UnityEngine.Object) this.ArrowImage)
            this.hitParticleObj = ObjectPool.Spawn(this.hitPrefab, this.ArrowImage.position, Quaternion.identity);
          this.SpawnChunks(collider.transform.position);
        }
        CameraManager.shakeCamera(0.2f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, collider.transform.position));
        AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hitwall", this.transform.position);
        this.DestroyProjectile();
        if (!this.Explosive)
          return;
        Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 4f, 1f, 1f, attackFlags: this.AttackFlags);
      }
      else
      {
        Vector3 vector2 = (Vector3) Utils.DegreeToVector2(this.Angle);
        RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) vector2, 1f, (int) this.bounceMask);
        if (!(bool) raycastHit2D)
          return;
        this.Angle = Utils.GetAngle(Vector3.zero, Vector3.Reflect(vector2, (Vector3) raycastHit2D.normal));
        --this.bouncesRemaining;
      }
    }
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  public float MagnitudeFindDistanceBetween(Vector2 a, Vector2 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    return Mathf.Sqrt((float) (num1 * num1 + (double) num2 * (double) num2));
  }

  public void SpawnChunks(Vector3 collisionPosition)
  {
    if (this.ValidParticleChunksToSpawn.Count == 0)
      return;
    int frame = -1;
    while (++frame < this.ValidParticleChunksToSpawn.Count)
      Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle(collisionPosition, this.transform.position) + (float) UnityEngine.Random.Range(-20, 20), this.ValidParticleChunksToSpawn, frame);
  }

  public void DestroyProjectile(bool forced = false)
  {
    if (!this.Destroyable && !forced || this.destroyed)
      return;
    AudioManager.Instance.StopLoop(this.LoopedSound);
    this.destroyed = true;
    float delay = 0.0f;
    if ((bool) (UnityEngine.Object) this.Trail)
    {
      delay = this.Trail.time;
      this.Trail.emit = false;
    }
    if ((bool) (UnityEngine.Object) this.collider)
      this.collider.enabled = false;
    this.recycleRoutine = !(bool) (UnityEngine.Object) this.ArrowImage ? this.StartCoroutine((IEnumerator) this.RecycleIE(delay)) : this.StartCoroutine((IEnumerator) this.RecycleIE(0.0f));
    this.OnDestroyProjectile.Invoke();
  }

  public IEnumerator RecycleIE(float delay)
  {
    Projectile projectile = this;
    yield return (object) new WaitForSeconds(delay);
    Projectile.Projectiles.Remove(projectile);
    if ((UnityEngine.Object) projectile != (UnityEngine.Object) null)
      projectile.gameObject.Recycle();
    if ((UnityEngine.Object) projectile.hitParticleObj != (UnityEngine.Object) null)
      projectile.hitParticleObj.Recycle();
    projectile.recycleRoutine = (Coroutine) null;
  }

  public virtual void DestroyWithVFX()
  {
    if (this.destroyed)
      return;
    this.EmitParticle();
    this.DestroyProjectile();
  }

  public static void ClearProjectiles()
  {
    for (int index = 0; index < Projectile.Projectiles.Count; ++index)
    {
      if ((UnityEngine.Object) Projectile.Projectiles[index] != (UnityEngine.Object) null)
        Projectile.Projectiles[index].DestroyProjectile(true);
    }
  }

  public void OnDestroy()
  {
    if (Projectile.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in Projectile.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    Projectile.loadedAddressableAssets.Clear();
  }

  public GameObject GetOwner()
  {
    return (UnityEngine.Object) this.Owner != (UnityEngine.Object) null ? this.Owner.gameObject : (GameObject) null;
  }

  public void SetOwner(GameObject owner)
  {
    Health component = owner.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Owner = component;
  }

  public static bool Contains(Health health)
  {
    foreach (Projectile projectile in Projectile.Projectiles)
    {
      if ((UnityEngine.Object) projectile != (UnityEngine.Object) null && (UnityEngine.Object) projectile.health == (UnityEngine.Object) health && (UnityEngine.Object) projectile.Owner != (UnityEngine.Object) health)
        return true;
    }
    return false;
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__127_0()
  {
    if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
      return;
    this.startPos = this.transform.position;
  }

  public class CollisionEvent
  {
    public float UnscaledTimestamp;
    public Health TargetHealth;

    public CollisionEvent(float unscaledTimestamp, Health targetHealth)
    {
      this.UnscaledTimestamp = unscaledTimestamp;
      this.TargetHealth = targetHealth;
    }
  }

  public delegate void OnHitUnit(Projectile projectile);

  public delegate void OnKnockedBack();
}
