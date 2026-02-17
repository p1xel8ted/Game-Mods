// Decompiled with JetBrains decompiler
// Type: EnemyDeathCatEye
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using CotL.Projectiles;
using FMOD.Studio;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyDeathCatEye : UnitObject
{
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string enterAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string exitAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string dieAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackAnticipateAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackLoopAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackEndAnimation;
  [SerializeField]
  public float shootAnticipation;
  [SerializeField]
  public GameObject deathParticle;
  public GameObject SplashParticle;
  public GameObject PopParticle;
  [SerializeField]
  public float projectilePatternRingsSpeed;
  [SerializeField]
  public float projectilePatternRingsAcceleration;
  [SerializeField]
  public float projectilePatternRingsRadius;
  [SerializeField]
  public ProjectileCircleBase projectilePatternRings;
  [SerializeField]
  public GameObject grenadeBullet;
  [SerializeField]
  public Vector2 grenadeNumberOfShots;
  [SerializeField]
  public float grenadeGravitySpeed;
  [SerializeField]
  public Vector2 grenadeShootDistanceRange;
  [SerializeField]
  public Vector2 grenadeDelayBetweenShots;
  [SerializeField]
  public GameObject chunkBullet;
  [SerializeField]
  public Vector2 chunkNumberOfShots;
  [SerializeField]
  public float chunkRadius;
  [SerializeField]
  public float chunkSpeed;
  [SerializeField]
  public ProjectilePatternBeam projectilePatternBeam;
  [SerializeField]
  public ProjectilePattern projectilePatternCircles;
  [SerializeField]
  public GameObject trailPrefab;
  public List<GameObject> Trails = new List<GameObject>();
  [CompilerGenerated]
  public bool \u003CAttacking\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CActive\u003Ek__BackingField;
  public Coroutine hidingRoutine;
  public Coroutine attackingRoutine;
  public Coroutine movementRoutine;
  public float damageRequiredToHide = 10f;
  public float currentHP;
  public float delayBetweenTrails = 0.1f;
  public float trailsTimer;
  public GameObject trail;
  public Vector3 previousSpawnPosition;
  public GameObject targetEnemy;
  public EventInstance loopingSoundInstance;
  [CompilerGenerated]
  public bool \u003CspawnTrails\u003Ek__BackingField;
  public Vector2 DistanceRange = new Vector2(3f, 4f);
  public Vector2 IdleWaitRange = new Vector2(0.2f, 1.5f);
  public float RandomDirection;
  public float IdleWait;

  public bool Attacking
  {
    get => this.\u003CAttacking\u003Ek__BackingField;
    set => this.\u003CAttacking\u003Ek__BackingField = value;
  }

  public bool Active
  {
    get => this.\u003CActive\u003Ek__BackingField;
    set => this.\u003CActive\u003Ek__BackingField = value;
  }

  public bool spawnTrails
  {
    get => this.\u003CspawnTrails\u003Ek__BackingField;
    set => this.\u003CspawnTrails\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    this.GetComponent<Health>().OnHit += new Health.HitAction(this.EnemyDeathCatEye_OnHit);
    ProjectilePatternBase.OnProjectileSpawned += new ProjectilePatternBase.ProjectileEvent(this.ProjectilePatternBase_OnProjectileSpawned);
    this.InitializeProjectilePatternRings();
    this.InitializeGranadeBullets();
    this.InitializeChunkBullets();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    ProjectilePatternBase.OnProjectileSpawned -= new ProjectilePatternBase.ProjectileEvent(this.ProjectilePatternBase_OnProjectileSpawned);
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public void ProjectilePatternBase_OnProjectileSpawned()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_spawn", this.gameObject);
  }

  public void Attack(int index, int activeEyes, float delay)
  {
    this.StartCoroutine((IEnumerator) this.AttackIE(index, activeEyes, delay));
  }

  public IEnumerator AttackIE(int index, int activeEyes, float delay)
  {
    EnemyDeathCatEye enemyDeathCatEye = this;
    enemyDeathCatEye.Attacking = true;
    enemyDeathCatEye.targetEnemy = enemyDeathCatEye.ReconsiderPlayerTarget().gameObject;
    yield return (object) new WaitForSeconds(delay);
    enemyDeathCatEye.Spine.AnimationState.SetAnimation(0, enemyDeathCatEye.attackAnticipateAnimation, false);
    enemyDeathCatEye.Spine.AnimationState.AddAnimation(0, enemyDeathCatEye.attackLoopAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/jellyfish_large/warning", enemyDeathCatEye.gameObject);
    yield return (object) new WaitForSeconds(1.33f);
    if (enemyDeathCatEye.Active)
    {
      switch (index)
      {
        case 0:
          yield return (object) (enemyDeathCatEye.attackingRoutine = enemyDeathCatEye.StartCoroutine((IEnumerator) enemyDeathCatEye.ShootProjectileRingsIE(activeEyes)));
          break;
        case 1:
          yield return (object) (enemyDeathCatEye.attackingRoutine = enemyDeathCatEye.StartCoroutine((IEnumerator) enemyDeathCatEye.ShootProjectileBeamIE(activeEyes)));
          break;
        case 2:
          yield return (object) (enemyDeathCatEye.attackingRoutine = enemyDeathCatEye.StartCoroutine((IEnumerator) enemyDeathCatEye.ShootProjectileCircleIE(activeEyes)));
          break;
        case 3:
          yield return (object) (enemyDeathCatEye.attackingRoutine = enemyDeathCatEye.StartCoroutine((IEnumerator) enemyDeathCatEye.ShootGrenadeBulletsIE(activeEyes)));
          break;
        case 4:
          yield return (object) (enemyDeathCatEye.attackingRoutine = enemyDeathCatEye.StartCoroutine((IEnumerator) enemyDeathCatEye.ShootProjectileChunkIE(activeEyes)));
          break;
      }
      enemyDeathCatEye.Spine.AnimationState.SetAnimation(0, enemyDeathCatEye.attackEndAnimation, false);
      enemyDeathCatEye.Spine.AnimationState.AddAnimation(0, enemyDeathCatEye.idleAnimation, true, 0.0f);
      yield return (object) new WaitForSeconds(1.33f);
      enemyDeathCatEye.Attacking = false;
    }
  }

  public void InitializeProjectilePatternRings()
  {
    int initialPoolSize = 10;
    if (this.projectilePatternRings is ProjectileCirclePattern)
    {
      ProjectileCirclePattern projectilePatternRings = (ProjectileCirclePattern) this.projectilePatternRings;
      if ((UnityEngine.Object) projectilePatternRings.ProjectilePrefab != (UnityEngine.Object) null)
        ObjectPool.CreatePool<Projectile>(projectilePatternRings.ProjectilePrefab, projectilePatternRings.BaseProjectilesCount * initialPoolSize);
    }
    ObjectPool.CreatePool<ProjectileCircleBase>(this.projectilePatternRings, initialPoolSize);
  }

  public void ShootProjectileRings()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileRingsIE(3));
  }

  public IEnumerator ShootProjectileRingsIE(int activeEyes)
  {
    EnemyDeathCatEye enemyDeathCatEye = this;
    int amount = 1;
    if (activeEyes < 3)
      amount = activeEyes == 2 ? UnityEngine.Random.Range(2, 4) : UnityEngine.Random.Range(3, 5);
    for (int i = 0; i < amount; ++i)
    {
      enemyDeathCatEye.speed = 0.0f;
      Projectile component = ObjectPool.Spawn<ProjectileCircleBase>(enemyDeathCatEye.projectilePatternRings, enemyDeathCatEye.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyDeathCatEye.transform.position;
      component.Angle = enemyDeathCatEye.GetAngleToPlayer();
      component.health = enemyDeathCatEye.health;
      component.team = Health.Team.Team2;
      component.Speed = enemyDeathCatEye.projectilePatternRingsSpeed;
      component.Acceleration = enemyDeathCatEye.projectilePatternRingsAcceleration;
      component.GetComponent<ProjectileCircleBase>().InitDelayed(enemyDeathCatEye.targetEnemy, enemyDeathCatEye.projectilePatternRingsRadius * 2f, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_spawn", enemyDeathCatEye.gameObject);
      yield return (object) new WaitForSeconds(1f);
    }
  }

  public void ShootProjectileBeam()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileBeamIE(3));
  }

  public IEnumerator ShootProjectileBeamIE(int activeEyes)
  {
    float postDelay = 5f;
    if (activeEyes < 3)
    {
      for (int index = 0; index < this.projectilePatternBeam.BulletWaves.Length; ++index)
      {
        this.projectilePatternBeam.BulletWaves[index].Bullets = activeEyes == 2 ? 12 : 14;
        this.projectilePatternBeam.BulletWaves[index].DelayBetweenBullets = activeEyes == 2 ? 0.35f : 0.2f;
      }
      postDelay = activeEyes == 2 ? 3.5f : 3f;
    }
    yield return (object) this.projectilePatternBeam.ShootIE(0.0f, this.targetEnemy, (Transform) null, false);
    yield return (object) new WaitForSeconds(postDelay);
  }

  public void ShootProjectileCircle()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileCircleIE(3));
  }

  public IEnumerator ShootProjectileCircleIE(int activeEyes)
  {
    if (activeEyes < 3)
    {
      for (int index = 0; index < this.projectilePatternCircles.Waves.Length; ++index)
      {
        this.projectilePatternCircles.Waves[index].Bullets = activeEyes == 2 ? 10 : 20;
        this.projectilePatternCircles.Waves[index].AngleBetweenBullets = activeEyes == 2 ? 36f : 18f;
      }
    }
    yield return (object) this.projectilePatternCircles.ShootIE(0.0f, this.targetEnemy, (Transform) null, false);
    yield return (object) new WaitForSeconds(1f);
  }

  public void ShootGrenadeBullets()
  {
    this.StartCoroutine((IEnumerator) this.ShootGrenadeBulletsIE(3));
  }

  public void InitializeGranadeBullets()
  {
    ObjectPool.CreatePool(this.grenadeBullet, (int) this.grenadeNumberOfShots.y * 3);
  }

  public IEnumerator ShootGrenadeBulletsIE(int activeEyes)
  {
    EnemyDeathCatEye enemyDeathCatEye = this;
    int shots = (int) UnityEngine.Random.Range(enemyDeathCatEye.grenadeNumberOfShots.x, enemyDeathCatEye.grenadeNumberOfShots.y);
    if (activeEyes < 3)
      shots *= activeEyes == 2 ? 2 : 2;
    int i = -1;
    while (++i < shots)
    {
      float Angle = (float) UnityEngine.Random.Range(0, 360);
      GrenadeBullet component = ObjectPool.Spawn(enemyDeathCatEye.grenadeBullet, enemyDeathCatEye.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>();
      component.SetOwner(enemyDeathCatEye.gameObject);
      component.Play(-1f, Angle, UnityEngine.Random.Range(enemyDeathCatEye.grenadeShootDistanceRange.x, enemyDeathCatEye.grenadeShootDistanceRange.y), UnityEngine.Random.Range(enemyDeathCatEye.grenadeGravitySpeed - 2f, enemyDeathCatEye.grenadeGravitySpeed + 2f), enemyDeathCatEye.health.team);
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", enemyDeathCatEye.gameObject);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyDeathCatEye.grenadeDelayBetweenShots.x, enemyDeathCatEye.grenadeDelayBetweenShots.y));
    }
    yield return (object) new WaitForSeconds(1f);
  }

  public void InitializeChunkBullets()
  {
    ObjectPool.CreatePool(this.chunkBullet, (int) this.chunkNumberOfShots.y * 4);
  }

  [SerializeField]
  public void ShootProjectileChunk()
  {
    this.StartCoroutine((IEnumerator) this.ShootProjectileChunkIE(3));
  }

  public IEnumerator ShootProjectileChunkIE(int activeEyes)
  {
    EnemyDeathCatEye enemyDeathCatEye = this;
    int shots = (int) UnityEngine.Random.Range(enemyDeathCatEye.chunkNumberOfShots.x, enemyDeathCatEye.chunkNumberOfShots.y);
    float radius = enemyDeathCatEye.chunkRadius;
    int amount = 1;
    if (activeEyes < 3)
    {
      shots *= activeEyes == 2 ? 1 : 2;
      radius *= activeEyes == 2 ? 1.2f : 1.5f;
      amount = activeEyes == 2 ? UnityEngine.Random.Range(1, 3) : UnityEngine.Random.Range(2, 4);
    }
    GameObject target = (UnityEngine.Object) enemyDeathCatEye.targetEnemy == (UnityEngine.Object) null ? PlayerFarming.Instance.gameObject : enemyDeathCatEye.targetEnemy;
    for (int t = 0; t < amount; ++t)
    {
      for (int index = 0; index < shots; ++index)
      {
        Vector3 vector3 = (Vector3) (UnityEngine.Random.insideUnitCircle * radius);
        Projectile component = ObjectPool.Spawn(enemyDeathCatEye.chunkBullet, enemyDeathCatEye.transform.parent).GetComponent<Projectile>();
        component.transform.position = enemyDeathCatEye.transform.position + vector3;
        component.Angle = Utils.GetAngle(enemyDeathCatEye.transform.position, target.transform.position + vector3);
        component.team = enemyDeathCatEye.health.team;
        component.Speed = enemyDeathCatEye.chunkSpeed;
        component.LifeTime = 4f + UnityEngine.Random.Range(0.0f, 0.3f);
        component.Owner = enemyDeathCatEye.health;
      }
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/frog_large/attack", enemyDeathCatEye.gameObject);
      yield return (object) new WaitForSeconds(1f);
    }
  }

  public float GetAngleToPlayer()
  {
    return Utils.GetAngle(this.transform.position, this.targetEnemy.transform.position);
  }

  public void Hide(float delay)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.HideIE(delay));
  }

  public IEnumerator HideIE(float delay = 0.0f)
  {
    EnemyDeathCatEye enemyDeathCatEye = this;
    if (enemyDeathCatEye.movementRoutine != null)
      enemyDeathCatEye.StopCoroutine(enemyDeathCatEye.movementRoutine);
    if (enemyDeathCatEye.Active)
    {
      enemyDeathCatEye.Active = false;
      if (enemyDeathCatEye.attackingRoutine != null)
        enemyDeathCatEye.StopCoroutine(enemyDeathCatEye.attackingRoutine);
      GameManager.GetInstance().RemoveFromCamera(enemyDeathCatEye.gameObject);
      yield return (object) new WaitForSeconds(delay);
      enemyDeathCatEye.speed = 0.0f;
      enemyDeathCatEye.health.enabled = false;
      enemyDeathCatEye.Attacking = false;
      enemyDeathCatEye.projectilePatternCircles.StopAllCoroutines();
      enemyDeathCatEye.projectilePatternBeam.StopAllCoroutines();
      if (enemyDeathCatEye.attackingRoutine != null)
        enemyDeathCatEye.StopCoroutine(enemyDeathCatEye.attackingRoutine);
      enemyDeathCatEye.Spine.AnimationState.SetAnimation(0, enemyDeathCatEye.exitAnimation, false);
      AudioManager.Instance.PlayOneShot("event:/enemy/chaser_boss/chaser_boss_egg_spawn", enemyDeathCatEye.gameObject);
      yield return (object) new WaitForSeconds(1f);
      enemyDeathCatEye.spawnTrails = true;
      enemyDeathCatEye.Spine.gameObject.SetActive(false);
      enemyDeathCatEye.hidingRoutine = (Coroutine) null;
    }
  }

  public void Show() => this.StartCoroutine((IEnumerator) this.ShowIE());

  public IEnumerator ShowIE()
  {
    EnemyDeathCatEye enemyDeathCatEye = this;
    if (EnemyDeathCatEyesManager.Instance.Eyes.Count <= 1)
      enemyDeathCatEye.Spine.Skeleton.SetSkin("Hurt");
    GameManager.GetInstance().AddToCamera(enemyDeathCatEye.gameObject);
    enemyDeathCatEye.speed = 0.0f;
    enemyDeathCatEye.health.enabled = true;
    enemyDeathCatEye.currentHP = enemyDeathCatEye.health.HP;
    enemyDeathCatEye.damageRequiredToHide = (float) UnityEngine.Random.Range(20, 25);
    enemyDeathCatEye.Active = true;
    enemyDeathCatEye.Spine.gameObject.SetActive(true);
    enemyDeathCatEye.Spine.AnimationState.SetAnimation(0, enemyDeathCatEye.enterAnimation, false);
    enemyDeathCatEye.Spine.AnimationState.AddAnimation(0, enemyDeathCatEye.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser_boss/chaser_boss_egg_spawn", enemyDeathCatEye.gameObject);
    yield return (object) new WaitForSeconds(1f);
  }

  public void Reposition(Vector3 position, float delayBetween)
  {
    this.movementRoutine = this.StartCoroutine((IEnumerator) this.RepositionIE(position, delayBetween));
  }

  public IEnumerator RepositionIE(Vector3 position, float delayBetween)
  {
    EnemyDeathCatEye enemyDeathCatEye = this;
    Vector3 startPosition = enemyDeathCatEye.transform.position;
    enemyDeathCatEye.spawnTrails = true;
    enemyDeathCatEye.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/fishing/caught_something_loop", enemyDeathCatEye.gameObject, true);
    float t = 0.0f;
    while ((double) t < (double) delayBetween)
    {
      t += Time.deltaTime;
      float t1 = t / delayBetween;
      enemyDeathCatEye.speed = 0.0f;
      enemyDeathCatEye.transform.position = Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0.0f, 1f, t1));
      if ((double) t1 > 0.925000011920929)
        enemyDeathCatEye.spawnTrails = false;
      yield return (object) null;
    }
    AudioManager.Instance.StopLoop(enemyDeathCatEye.loopingSoundInstance);
    enemyDeathCatEye.Show();
  }

  public void EnemyDeathCatEye_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    int count = EnemyDeathCatEyesManager.Instance.Eyes.Count;
    float num = EnemyDeathCatBoss.Instance.health.HP / EnemyDeathCatBoss.Instance.health.totalHP;
    this.SimpleSpineFlash.FlashFillRed();
    if (count == 3 && (double) num <= 0.6600000262260437 || count == 2 && (double) num <= 0.33000001311302185 || count == 1 && (double) num <= 0.10000000149011612)
    {
      EnemyDeathCatEyesManager.Instance.Eyes.Remove(this);
      this.health.invincible = true;
      if (EnemyDeathCatEyesManager.Instance.Eyes.Count > 0)
      {
        if ((double) UnityEngine.Random.value < 0.33000001311302185)
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, this.transform.position + Vector3.back);
        else
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RED_HEART, 1, this.transform.position + Vector3.back);
      }
      this.projectilePatternCircles.StopAllCoroutines();
      this.projectilePatternBeam.StopAllCoroutines();
      if (this.attackingRoutine != null)
        this.StopCoroutine(this.attackingRoutine);
      EnemyDeathCatBoss.Instance.EyeDestroyed();
      this.StopAllCoroutines();
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.Die());
    }
    else if ((double) this.currentHP - (double) this.health.HP >= (double) this.damageRequiredToHide && this.Attacking)
    {
      if (this.hidingRoutine == null)
      {
        this.StopAllCoroutines();
        this.hidingRoutine = this.StartCoroutine((IEnumerator) this.HideIE());
      }
      AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", this.gameObject);
      AudioManager.Instance.PlayOneShot("event:/enemy/fly_spawn", this.gameObject);
    }
    EnemyDeathCatBoss.Instance.health.invincible = false;
    EnemyDeathCatBoss.Instance.health.team = Health.Team.Neutral;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(Attacker);
    if (AttackType == Health.AttackTypes.Melee)
      EnemyDeathCatBoss.Instance.health.DealDamage(PlayerWeapon.GetDamage(1f, farmingComponent.currentWeaponLevel, farmingComponent), Attacker, AttackLocation);
    else
      EnemyDeathCatBoss.Instance.health.DealDamage(EquipmentManager.GetCurseData(farmingComponent.currentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier(farmingComponent), Attacker, AttackLocation, AttackType: Health.AttackTypes.Projectile);
    EnemyDeathCatBoss.Instance.health.team = Health.Team.Team2;
    EnemyDeathCatBoss.Instance.health.invincible = true;
  }

  public IEnumerator Die()
  {
    EnemyDeathCatEye enemyDeathCatEye = this;
    if (enemyDeathCatEye.attackingRoutine != null)
      enemyDeathCatEye.StopCoroutine(enemyDeathCatEye.attackingRoutine);
    enemyDeathCatEye.speed = 0.0f;
    enemyDeathCatEye.health.enabled = false;
    enemyDeathCatEye.Attacking = false;
    enemyDeathCatEye.Active = false;
    GameManager.GetInstance().RemoveFromCamera(enemyDeathCatEye.gameObject);
    enemyDeathCatEye.projectilePatternCircles.StopAllCoroutines();
    enemyDeathCatEye.projectilePatternBeam.StopAllCoroutines();
    if (enemyDeathCatEye.attackingRoutine != null)
      enemyDeathCatEye.StopCoroutine(enemyDeathCatEye.attackingRoutine);
    enemyDeathCatEye.deathParticle.SetActive(true);
    enemyDeathCatEye.Spine.AnimationState.SetAnimation(0, enemyDeathCatEye.dieAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/explosion/explosion", enemyDeathCatEye.gameObject);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm_large/warning", enemyDeathCatEye.gameObject);
    PlayerFarming.Instance.playerWeapon.DoSlowMo();
    yield return (object) new WaitForSeconds(0.6f);
    enemyDeathCatEye.Spine.gameObject.SetActive(false);
    enemyDeathCatEye.hidingRoutine = (Coroutine) null;
  }

  public override void Update()
  {
    double num = (double) (this.IdleWait -= Time.deltaTime);
    this.GetNewTargetPosition();
    if (this.spawnTrails)
      this.SpawnTrails();
    if ((UnityEngine.Object) EnemyDeathCatEyesManager.Instance != (UnityEngine.Object) null)
    {
      if (EnemyDeathCatEyesManager.Instance.Eyes.Count == 3)
        this.maxSpeed = 0.02f;
      else if (EnemyDeathCatEyesManager.Instance.Eyes.Count == 2)
        this.maxSpeed = 0.04f;
      else if (EnemyDeathCatEyesManager.Instance.Eyes.Count == 1)
        this.maxSpeed = 0.06f;
    }
    if (this.UsePathing)
    {
      if (this.pathToFollow == null)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / 20.0) * GameManager.DeltaTime;
        this.move();
        return;
      }
      if (this.currentWaypoint >= this.pathToFollow.Count)
      {
        this.speed += (float) ((0.0 - (double) this.speed) / 20.0) * GameManager.DeltaTime;
        this.move();
        return;
      }
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Fleeing)
    {
      this.speed += (float) (((double) this.maxSpeed * (double) this.SpeedMultiplier - (double) this.speed) / 35.0) * GameManager.DeltaTime;
      if (this.UsePathing)
      {
        this.state.facingAngle = Mathf.LerpAngle(this.state.facingAngle, Utils.GetAngle(this.transform.position, this.pathToFollow[this.currentWaypoint]), Time.deltaTime * 2f);
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
          }
        }
      }
    }
    else
      this.speed += (float) ((0.0 - (double) this.speed) / 20.0) * GameManager.DeltaTime;
    this.move();
  }

  public void GetNewTargetPosition()
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      return;
    float num1 = 100f;
    while ((double) --num1 > 0.0)
    {
      float num2 = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      this.RandomDirection += (float) UnityEngine.Random.Range(-45, 45) * ((float) Math.PI / 180f);
      float radius = 0.1f;
      Vector3 targetLocation = this.transform.position + new Vector3(num2 * Mathf.Cos(this.RandomDirection), num2 * Mathf.Sin(this.RandomDirection));
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(targetLocation - this.transform.position), num2 * 0.5f, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      {
        this.RandomDirection += 0.17453292f;
      }
      else
      {
        this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
        this.givePath(targetLocation);
        break;
      }
    }
  }

  public void SpawnTrails()
  {
    if ((double) (this.trailsTimer += Time.deltaTime) <= (double) this.delayBetweenTrails || (double) Vector3.Distance(this.transform.position, this.previousSpawnPosition) <= 0.10000000149011612)
      return;
    this.trailsTimer = 0.0f;
    this.trail = (GameObject) null;
    if (this.Trails.Count > 0)
    {
      foreach (GameObject trail in this.Trails)
      {
        if (!trail.activeSelf)
        {
          this.trail = trail;
          this.trail.transform.position = this.transform.position;
          this.trail.SetActive(true);
          break;
        }
      }
    }
    if ((UnityEngine.Object) this.trail == (UnityEngine.Object) null)
    {
      this.trail = UnityEngine.Object.Instantiate<GameObject>(this.trailPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
      this.Trails.Add(this.trail);
    }
    this.previousSpawnPosition = this.trail.transform.position;
  }
}
