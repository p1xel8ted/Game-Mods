// Decompiled with JetBrains decompiler
// Type: Projectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Ara;
using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Projectile : BaseMonoBehaviour
{
  public static List<Projectile> Projectiles = new List<Projectile>();
  public Transform ArrowImage;
  public Transform ArrowImageEmission;
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
  public TrailRenderer LQ_Trail;
  public AraTrail Trail;
  public bool Explosive;
  public bool CanKnockBack = true;
  public bool ModifyingZ;
  public int AllowedBounces;
  public bool Destroyable = true;
  public bool DestroyOnWallHit = true;
  public float ScreenShakeMultiplier = 1f;
  public Health.Team _team = Health.Team.PlayerTeam;
  private float InitScale;
  private float Scale;
  private float ScaleSpeed;
  public List<Sprite> ChunksToSpawn;
  private List<Sprite> ValidParticleChunksToSpawn;
  private float Timer;
  public Health health;
  public Health Owner;
  public Health.AttackFlags AttackFlags;
  private int bouncesRemaining;
  private bool destroyed;
  private Rigidbody2D rb;
  private Vector3 startPos = Vector3.zero;
  [HideInInspector]
  public Projectile.CollisionEvent collisionEventQueue;
  public bool SetTargetToClosest;
  public bool homeInOnTarget;
  public float turningSpeed = 1f;
  public float angleNoiseAmplitude;
  public float angleNoiseFrequency;
  public float DamageToNeutral = 10f;
  public float NeutralSplashRadius;
  public float HitFxScaleMultiplier = 1f;
  public bool Seperate;
  private Health targetObject;
  [EventRef]
  public string OnSpawnSoundPath = string.Empty;
  [EventRef]
  public string LoopedSoundPath = string.Empty;
  private EventInstance LoopedSound;
  private float spawnTimestamp;
  private GameObject hitPrefab;
  private GameObject hitParticleObj;
  private Collider2D collider;
  private LayerMask bounceMask;
  private LayerMask unitMask;
  public static GameObject projectilePrefab;
  public Vector2 Seperator = Vector2.zero;
  private float SeperationRadius = 0.6f;
  [HideInInspector]
  private float SeparationCooldownTime;
  private CircleCollider2D circleCollider2D;
  private bool disableOnce;
  private float ZWave;
  public float ZWaveSize;
  public float ZWaveSpeed;
  private bool stoppedLoop;
  public bool IgnoreIsland;

  public SkeletonAnimation Spine { get; set; }

  public Health.Team team
  {
    get => this._team;
    set => this._team = value;
  }

  public CircleCollider2D CircleCollider2D => this.circleCollider2D;

  public static void CreateProjectiles(int amount, Health owner, Vector3 position)
  {
    Transform parent = owner.transform.parent;
    float angle = 0.0f;
    int num = -1;
    while (++num < amount)
      Addressables.InstantiateAsync((object) "Assets/Prefabs/Enemies/Weapons/ArrowTurrets.prefab", position, Quaternion.identity, owner.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        if ((UnityEngine.Object) obj.Result == (UnityEngine.Object) null)
          return;
        Projectile component = obj.Result.GetComponent<Projectile>();
        component.transform.position = position;
        component.Angle = angle;
        component.team = Health.Team.Team2;
        component.Speed = 4f;
        component.Owner = owner;
        angle += 360f / (float) amount;
        if (owner.team == Health.Team.PlayerTeam)
          return;
        component.Speed *= DataManager.Instance.ProjectileMoveSpeedMultiplier;
      });
  }

  private void Awake()
  {
    this.collider = this.GetComponentInChildren<Collider2D>();
    this.circleCollider2D = this.GetComponent<CircleCollider2D>();
  }

  private void Start()
  {
    this.Scale = this.InitScale * 2f;
    if ((bool) (UnityEngine.Object) this.health)
      this.health.team = this.team;
    this.Spine = this.GetComponentInChildren<SkeletonAnimation>();
    this.startPos = this.transform.position;
    this.spawnTimestamp = GameManager.GetInstance().CurrentTime;
    this.bounceMask = (LayerMask) ((int) this.bounceMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.bounceMask = (LayerMask) ((int) this.bounceMask | 1 << LayerMask.NameToLayer("Island"));
    this.unitMask = (LayerMask) ((int) this.unitMask | 1 << LayerMask.NameToLayer("Obstacles"));
    this.unitMask = (LayerMask) ((int) this.unitMask | 1 << LayerMask.NameToLayer("Units"));
  }

  private void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    this.Acceleration = 0.0f;
    this.Deceleration = 0.0f;
    if ((bool) (UnityEngine.Object) this.health)
      this.health.OnHit -= new Health.HitAction(this.OnHit);
    Projectile.Projectiles.Remove(this);
    if ((UnityEngine.Object) this.hitParticleObj != (UnityEngine.Object) null)
      this.hitParticleObj.Recycle();
    if (this.destroyed || !((UnityEngine.Object) this.transform.parent == (UnityEngine.Object) null) && (!((UnityEngine.Object) this.transform.parent.GetComponent<Projectile>() == (UnityEngine.Object) null) || !((UnityEngine.Object) this.transform.parent.GetComponent<ObjectPool>() == (UnityEngine.Object) null)))
      return;
    GameManager.GetInstance()?.StartCoroutine((IEnumerator) this.RecycleIE(1f));
  }

  protected virtual void OnEnable()
  {
    if ((bool) (UnityEngine.Object) this.health)
      this.health.OnHit += new Health.HitAction(this.OnHit);
    if (!this.LoopedSound.isValid())
      this.LoopedSound = AudioManager.Instance.CreateLoop(this.LoopedSoundPath, true);
    if (!string.IsNullOrEmpty(this.OnSpawnSoundPath))
      AudioManager.Instance.PlayOneShot(this.OnSpawnSoundPath, this.gameObject);
    if (!Projectile.Projectiles.Contains(this))
      Projectile.Projectiles.Add(this);
    this.timestamp = GameManager.GetInstance().CurrentTime;
    this.Timer = 0.0f;
    this.destroyed = false;
    this.Explosive = false;
    this.Damage = 1f;
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
  }

  private IEnumerator DelayTrailByFrame()
  {
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    this.LQ_Trail.emitting = true;
  }

  public bool KnockedBack { get; set; }

  protected void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    Debug.Log((object) "HIT!");
    Debug.Log((object) this.name);
    if (!this.canParry || (double) GameManager.GetInstance().CurrentTime < (double) this.spawnTimestamp + (double) this.InvincibleTime || !this.CanKnockBack || (UnityEngine.Object) Attacker.gameObject != (UnityEngine.Object) PlayerFarming.Instance.gameObject)
      return;
    this.KnockedBack = true;
    this.team = Health.Team.PlayerTeam;
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
      Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 3f);
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
      if ((UnityEngine.Object) Attacker == (UnityEngine.Object) PlayerFarming.Instance.gameObject)
        this.ForgiveCollisionWithPlayer();
      if ((UnityEngine.Object) this.Owner != (UnityEngine.Object) null)
      {
        this.Angle = Utils.GetAngle(this.transform.position, this.Owner.transform.position);
      }
      else
      {
        Health health = (Health) null;
        float num1 = float.MaxValue;
        float num2 = 0.0f;
        float num3 = 90f;
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

  private void EmitParticle(float scaleMultiplier = 1f)
  {
    if (!((UnityEngine.Object) this.ArrowImage != (UnityEngine.Object) null))
      return;
    BiomeConstants.Instance.EmitHitFX_BlockedRedSmall(this.ArrowImage.position, Quaternion.identity, Vector3.one * scaleMultiplier * this.HitFxScaleMultiplier);
  }

  public void Deflect()
  {
  }

  protected void FixedUpdate()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    if ((double) PlayerFarming.Instance.health.HP <= 0.0 && !this.stoppedLoop)
    {
      AudioManager.Instance.StopLoop(this.LoopedSound);
      this.stoppedLoop = true;
    }
    this.ScaleSpeed += (float) (((double) this.InitScale - (double) this.Scale) * 0.40000000596046448);
    this.Scale += (this.ScaleSpeed *= 0.6f);
    this.Speed += (this.Acceleration - this.Deceleration) * Time.fixedDeltaTime;
    this.Seperator = Vector2.zero;
    if (this.Seperate && (double) this.SeparationCooldownTime < (double) Time.time)
    {
      float num1 = Time.fixedDeltaTime * 60f;
      foreach (Projectile projectile in Projectile.Projectiles)
      {
        if (!((UnityEngine.Object) projectile == (UnityEngine.Object) null) && !((UnityEngine.Object) projectile == (UnityEngine.Object) this) && projectile.Seperate)
        {
          float num2 = Vector2.Distance((Vector2) projectile.transform.position, (Vector2) this.transform.position);
          if ((double) num2 < (double) this.SeperationRadius)
          {
            float f = Utils.GetAngleR(projectile.transform.position, this.transform.position) * ((float) Math.PI / 180f);
            float num3 = (float) (((double) this.SeperationRadius - (double) num2) / 2.0);
            this.Seperator.x += num3 * Mathf.Cos(f) * num1;
            this.Seperator.y += num3 * Mathf.Sin(f) * num1;
            projectile.Seperator.x -= num3 * Mathf.Cos(f) * num1;
            projectile.Seperator.y -= num3 * Mathf.Sin(f) * num1;
          }
        }
      }
      this.SeparationCooldownTime = Time.time + 0.5f;
    }
    if (this.homeInOnTarget && !this.KnockedBack)
      this.TurnTowardsTarget();
    float angle = this.Angle;
    if ((double) this.angleNoiseAmplitude > 0.0 && (double) this.angleNoiseFrequency > 0.0)
      angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(this.timestamp) * this.angleNoiseFrequency, 0.0f) - 0.5f) * this.angleNoiseAmplitude;
    float f1 = angle * ((float) Math.PI / 180f);
    Vector2 vector2_1 = (Vector2) (new Vector3(this.Speed * Mathf.Cos(f1), this.Speed * Mathf.Sin(f1), 0.0f) * Time.fixedDeltaTime);
    if ((double) this.SinLength != 0.0)
    {
      Vector2 vector2_2 = Utils.DegreeToVector2(angle + 90f).normalized * Mathf.PingPong(Time.time, this.SinLength);
    }
    if (this.followDirection)
      this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    double speedMultiplier = (double) this.SpeedMultiplier;
    Vector2 vector2_3 = vector2_1 * (float) speedMultiplier + this.Seperator;
    this.transform.position = this.transform.position + new Vector3(vector2_3.x, vector2_3.y, 0.0f);
    if (this.ModifyingZ)
      this.Spine.transform.localPosition = Vector3.Lerp(this.Spine.transform.localPosition, Vector3.zero, Time.fixedDeltaTime);
    if (this.collisionEventQueue != null)
    {
      if ((double) GameManager.GetInstance().UnscaledTimeSince(this.collisionEventQueue.UnscaledTimestamp) >= (double) Health.DealDamageForgivenessWindow)
        this.OnCollisionWithPlayer(this.collisionEventQueue.TargetHealth, true);
    }
    else if ((double) (this.Timer += Time.fixedDeltaTime) > (double) this.LifeTime && !this.destroyed && this.Destroyable)
      this.EndOfLife();
    if (!((UnityEngine.Object) this.ArrowImageEmission != (UnityEngine.Object) null))
      return;
    this.ArrowImageEmission.position = this.startPos;
  }

  public virtual void EndOfLife()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    if (this.Explosive)
    {
      Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 4f, 1f, 1f);
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

  private void TurnTowardsTarget()
  {
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null || !this.targetObject.enabled || (double) this.MagnitudeFindDistanceBetween(this.transform.position, this.targetObject.transform.position) <= 0.25)
      return;
    float turningSpeed = this.turningSpeed;
    if ((double) Mathf.Abs(this.Angle - Utils.GetAngle(this.transform.position, this.targetObject.transform.position)) > 180.0)
      turningSpeed /= 2f;
    this.Angle = Mathf.LerpAngle(this.Angle, Utils.GetAngle(this.transform.position, this.targetObject.transform.position), Time.fixedDeltaTime * turningSpeed);
  }

  protected void OnCollisionWithPlayer(Health targetHealth, bool collideImmediately = false)
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
        this.DestroyProjectile();
      }
      if (!collideImmediately)
        return;
      this.collisionEventQueue = (Projectile.CollisionEvent) null;
    }
  }

  protected void ForgiveCollisionWithPlayer()
  {
    if (this.collisionEventQueue != null)
      Debug.Log((object) "Projectile collision forgiven!");
    this.collisionEventQueue = (Projectile.CollisionEvent) null;
  }

  protected void OnTriggerEnter2D(Collider2D collider)
  {
    if (this.destroyed || (UnityEngine.Object) collider == (UnityEngine.Object) null || (UnityEngine.Object) collider.gameObject.GetComponent<Projectile>() != (UnityEngine.Object) null || this.destroyed)
      return;
    Health component1 = collider.gameObject.GetComponent<Health>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.enabled && component1.team != this.team && !component1.untouchable && !component1.invincible && !component1.IgnoreProjectiles && ((UnityEngine.Object) component1.state == (UnityEngine.Object) null || component1.state.CURRENT_STATE != StateMachine.State.Dodging))
    {
      if ((component1.team == Health.Team.Neutral || !this.ArrowImage.gameObject.activeSelf || component1.invincible) && (component1.team != Health.Team.Neutral || (double) this.DamageToNeutral <= 0.0 || !(component1.tag != "BreakableDecoration")))
        return;
      bool flag = component1.DealDamage(component1.team == Health.Team.Neutral ? this.DamageToNeutral : this.Damage, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile, AttackFlags: this.AttackFlags);
      if (component1.isPlayer)
      {
        this.OnCollisionWithPlayer(component1);
      }
      else
      {
        if (!flag)
        {
          CameraManager.shakeCamera(0.2f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, component1.transform.position));
          this.SpawnChunks(collider.transform.position);
        }
        else
          CameraManager.shakeCamera(0.5f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, component1.transform.position));
        if (component1.team == Health.Team.Neutral && (double) this.NeutralSplashRadius > 0.0)
        {
          foreach (Component component2 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.NeutralSplashRadius, (int) this.unitMask))
          {
            Health component3 = component2.GetComponent<Health>();
            if ((bool) (UnityEngine.Object) component3 && component3.team == Health.Team.Neutral)
              component3.DealDamage(this.DamageToNeutral, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Projectile, AttackFlags: this.AttackFlags);
          }
        }
        if (this.KnockedBack)
          Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 3f);
        else if (this.Explosive)
          Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 4f, 1f, 1f);
        if (!this.Explosive)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
        else
          AudioManager.Instance.PlayOneShot("event:/player/Curses/explosive_shot", this.transform.position);
        if ((bool) (UnityEngine.Object) this.health && this.health.team == Health.Team.PlayerTeam && (bool) (UnityEngine.Object) collider && (UnityEngine.Object) collider.GetComponentInParent<Projectile>() != (UnityEngine.Object) null)
        {
          foreach (Component component4 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.NeutralSplashRadius))
          {
            Projectile componentInParent = component4.GetComponentInParent<Projectile>();
            if ((bool) (UnityEngine.Object) componentInParent)
              componentInParent.DestroyProjectile();
          }
        }
        this.DestroyProjectile();
      }
    }
    else
    {
      if (this.IgnoreIsland || collider.gameObject.layer != LayerMask.NameToLayer("Obstacles") && collider.gameObject.layer != LayerMask.NameToLayer("Island"))
        return;
      if (this.bouncesRemaining <= 0)
      {
        if (!this.DestroyOnWallHit)
          return;
        this.EmitParticle();
        CameraManager.shakeCamera(0.2f * this.ScreenShakeMultiplier, Utils.GetAngle(this.transform.position, collider.transform.position));
        this.SpawnChunks(collider.transform.position);
        AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hitwall", this.transform.position);
        this.DestroyProjectile();
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

  private float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  protected void SpawnChunks(Vector3 collisionPosition)
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
    if ((bool) (UnityEngine.Object) this.ArrowImage)
      this.ArrowImage.gameObject.SetActive(false);
    if ((bool) (UnityEngine.Object) this.collider)
      this.collider.enabled = false;
    this.StartCoroutine((IEnumerator) this.RecycleIE(delay));
  }

  private IEnumerator RecycleIE(float delay)
  {
    Projectile projectile = this;
    yield return (object) new WaitForSeconds(delay);
    Projectile.Projectiles.Remove(projectile);
    if ((UnityEngine.Object) projectile != (UnityEngine.Object) null)
      projectile.gameObject.Recycle();
    if ((UnityEngine.Object) projectile.hitParticleObj != (UnityEngine.Object) null)
      projectile.hitParticleObj.Recycle();
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
}
