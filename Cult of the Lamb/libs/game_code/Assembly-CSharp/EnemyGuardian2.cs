// Decompiled with JetBrains decompiler
// Type: EnemyGuardian2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using CotL.Projectiles;
using FMODUnity;
using I2.Loc;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyGuardian2 : UnitObject
{
  public AudioClip CombatMusic;
  public Interaction_MonsterHeart interaction_MonsterHeart;
  public BreakTempleChain BreakTempleChain;
  public Goat_GuardianDoor Goat_GuardianDoor;
  public SimpleSpineAnimator simpleSpineAnimator;
  public SimpleSpineEventListener simpleSpineEventListener;
  public Vector3 CentreOfLevel = Vector3.zero;
  public GameObject CenterOfLevelObject;
  public GameObject TargetObject;
  public ColliderEvents damageColliderEvents;
  public Health EnemyHealth;
  public GameObject[] EnemyToSpawn;
  public Transform CameraBone;
  public ParticleSystem Particles;
  public Collider2D HealthCollider;
  [SerializeField]
  public ProjectileCircleBase projectilePatternRings;
  [SerializeField]
  public float projectilePatternRingsSpeed;
  [SerializeField]
  public float projectilePatternRingsAcceleration;
  [SerializeField]
  public float projectilePatternRingsRadius;
  [SerializeField]
  public float projectilePatternRingsTimeBetween;
  [SerializeField]
  public float projectilePatternShootDelay;
  [SerializeField]
  public Vector2 projectilePatternRingsAmount;
  [SerializeField]
  public float projectilePatternScattedSpeed;
  [SerializeField]
  public float projectilePatternScattedAcceleration;
  [SerializeField]
  public float projectileScatteredRadius;
  [SerializeField]
  public Vector2 projectilePatternScatteredAmount;
  [SerializeField]
  public Vector2 projectilePatternScatteredDelay;
  [TermsPopup("")]
  public string DisplayName;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  public bool active;
  public int enemiesAlive;
  public const int maxEnemies = 13;
  public DeadBodySliding deadBodySliding;
  public GameObject Trap;
  public int TrapPattern;
  public int TargetEnemies = 3;
  public int NumWaves = 2;
  public List<GameObject> enemies = new List<GameObject>();
  public LineRenderer lineRenderer;

  public void Activate() => this.StartCoroutine((IEnumerator) this.ActivateIE());

  public override void Awake()
  {
    base.Awake();
    this.InitializeTraps();
    this.InitializeProjectilePatternRings();
  }

  public IEnumerator ActivateIE()
  {
    EnemyGuardian2 enemyGuardian2 = this;
    enemyGuardian2.health.BlackSoulOnHit = true;
    if ((bool) (UnityEngine.Object) DeathCatController.Instance)
      DeathCatController.Instance.conversation1.Play();
    yield return (object) new WaitForEndOfFrame();
    while (MMConversation.isPlaying)
      yield return (object) null;
    enemyGuardian2.TargetObject = enemyGuardian2.ReconsiderPlayerTarget().gameObject;
    HUD_DisplayName.Play(LocalizationManager.GetTranslation(enemyGuardian2.DisplayName), 2, HUD_DisplayName.Positions.Centre, HUD_DisplayName.textBlendMode.DungeonFinal);
    enemyGuardian2.CentreOfLevel = enemyGuardian2.CenterOfLevelObject.transform.position;
    enemyGuardian2.simpleSpineAnimator = enemyGuardian2.GetComponentInChildren<SimpleSpineAnimator>();
    enemyGuardian2.simpleSpineEventListener = enemyGuardian2.GetComponent<SimpleSpineEventListener>();
    enemyGuardian2.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(enemyGuardian2.OnSpineEvent);
    enemyGuardian2.Particles.Stop();
    enemyGuardian2.HealthCollider = enemyGuardian2.GetComponent<Collider2D>();
    enemyGuardian2.HealthCollider.enabled = false;
    enemyGuardian2.health.invincible = true;
    if ((UnityEngine.Object) enemyGuardian2.damageColliderEvents != (UnityEngine.Object) null)
    {
      enemyGuardian2.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(enemyGuardian2.OnDamageTriggerEnter);
      enemyGuardian2.damageColliderEvents.SetActive(false);
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyGuardian2.CameraBone.gameObject, 6f);
    enemyGuardian2.simpleSpineAnimator.Animate("intro2", 0, false);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    AudioManager.Instance.SetMusicRoomID(2, "deathcat_room_id");
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (this.active)
    {
      this.health.invincible = false;
      this.HealthCollider.enabled = true;
      this.StartCoroutine((IEnumerator) this.SpawnTraps());
      UIBossHUD.Play(this.health, LocalizationManager.GetTranslation(this.DisplayName));
      AudioManager.Instance.SetMusicRoomID(2, "deathcat_room_id");
    }
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "Intro Complete":
        Debug.Log((object) "INTRO COMPLETE?");
        this.health.invincible = false;
        this.HealthCollider.enabled = true;
        if (UnityEngine.Random.Range(0, 2) == 0)
          this.StartCoroutine((IEnumerator) this.ShootRingsScattered());
        else
          this.StartCoroutine((IEnumerator) this.ShootRingsTargeted());
        GameManager.GetInstance().OnConversationEnd();
        this.active = true;
        UIBossHUD.Play(this.health, LocalizationManager.GetTranslation(this.DisplayName));
        GameManager.GetInstance().AddToCamera(this.gameObject);
        GameManager.GetInstance().AddPlayersToCamera();
        break;
      case "Start Particles":
        CameraManager.instance.ShakeCameraForDuration(0.1f, 0.4f, 2.5f);
        this.Particles.Play();
        break;
      case "Stop Particles":
        this.Particles.Stop();
        break;
    }
  }

  public IEnumerator Die()
  {
    EnemyGuardian2 enemyGuardian2 = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyGuardian2.CameraBone.gameObject, 7f);
    enemyGuardian2.state.CURRENT_STATE = StateMachine.State.Dead;
    yield return (object) new WaitForEndOfFrame();
    enemyGuardian2.simpleSpineAnimator.Animate("dead", 0, false);
    yield return (object) new WaitForSeconds(4f);
    if ((double) UnityEngine.Random.value < 0.33000001311302185)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, enemyGuardian2.transform.position + Vector3.back);
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RED_HEART, 1, enemyGuardian2.transform.position + Vector3.back);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(enemyGuardian2.transform.position);
    if (PlayerFarming.IsAnyPlayerKnockedOut())
    {
      CoopManager.Instance.OnKnockedPlayerAwoken += new System.Action(enemyGuardian2.ActivateTheNextPhase);
      CoopManager.Instance.WakeAllKnockedOutPlayersWithHealth();
    }
    else
      enemyGuardian2.ActivateTheNextPhase();
    UnityEngine.Object.Destroy((UnityEngine.Object) enemyGuardian2.gameObject);
  }

  public void ActivateTheNextPhase()
  {
    CoopManager.Instance.OnKnockedPlayerAwoken -= new System.Action(this.ActivateTheNextPhase);
    DeathCatController.Instance.DeathCatCloneTransform();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.simpleSpineAnimator.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    this.simpleSpineAnimator.FlashWhite(false);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Die());
    this.HealthCollider.enabled = false;
    GameManager.GetInstance().RemoveFromCamera(this.gameObject);
    UIBossHUD.Hide();
    foreach (GameObject enemy in this.enemies)
    {
      if ((bool) (UnityEngine.Object) enemy)
      {
        Health component = enemy.GetComponent<Health>();
        component.enabled = true;
        component.invincible = false;
        component.DealDamage(float.MaxValue, component.gameObject, component.transform.position);
      }
    }
  }

  public IEnumerator DoHurt()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyGuardian2 enemyGuardian2 = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyGuardian2.simpleSpineAnimator.Animate("idle", 0, true);
      GameManager.GetInstance().RemoveFromCamera(enemyGuardian2.CameraBone.gameObject);
      enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.TeleportAway());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyGuardian2.simpleSpineAnimator.Animate("Coughing", 0, true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator Teleport(Vector3 Destination)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyGuardian2 enemyGuardian2 = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      CameraManager.shakeCamera(0.5f, Utils.GetAngle(enemyGuardian2.transform.position, Vector3.zero));
      BiomeConstants.Instance.SpawnInWhite.Spawn().transform.position = enemyGuardian2.transform.position + Vector3.down * 2f;
      enemyGuardian2.transform.position = Destination;
      BiomeConstants.Instance.SpawnInWhite.Spawn().transform.position = enemyGuardian2.transform.position + Vector3.down * 2f;
      enemyGuardian2.state.facingAngle = Utils.GetAngle(enemyGuardian2.transform.position, enemyGuardian2.TargetObject.transform.position);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyGuardian2.simpleSpineAnimator.Animate("dash", 0, false);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.15f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator TeleportAway()
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    EnemyGuardian2 enemyGuardian2 = this;
    if (num1 != 0)
    {
      if (num1 != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.SpawnTraps());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    float f = (float) (((double) Utils.GetAngle(enemyGuardian2.TargetObject.transform.position, enemyGuardian2.CentreOfLevel) + (double) UnityEngine.Random.Range(-90, 90)) * (Math.PI / 180.0));
    float num2 = 4f;
    Vector3 Destination = enemyGuardian2.CentreOfLevel + new Vector3(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.Teleport(Destination));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator SpawnTraps(int c = 0)
  {
    EnemyGuardian2 enemyGuardian2 = this;
    enemyGuardian2.simpleSpineAnimator.Animate("summon-fast", 0, false);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    enemyGuardian2.state.facingAngle = Utils.GetAngle(enemyGuardian2.transform.position, enemyGuardian2.TargetObject.transform.position);
    yield return (object) new WaitForSeconds(0.66f);
    for (int i = 0; i < UnityEngine.Random.Range(3, 6); ++i)
    {
      yield return (object) new WaitForSeconds(0.5f);
      enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.TrapPattern0());
    }
    yield return (object) new WaitForSeconds(2f);
    if (c == 0 && UnityEngine.Random.Range(0, 2) == 0)
      enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.SpawnTraps(1));
    else
      enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.TeleportAndShoot());
  }

  public void InitializeProjectilePatternRings()
  {
    int initialPoolSize = Mathf.Max((int) this.projectilePatternScatteredAmount.y, (int) this.projectilePatternRingsAmount.y);
    if (this.projectilePatternRings is ProjectileCirclePattern)
    {
      ProjectileCirclePattern projectilePatternRings = (ProjectileCirclePattern) this.projectilePatternRings;
      if ((UnityEngine.Object) projectilePatternRings.ProjectilePrefab != (UnityEngine.Object) null)
        ObjectPool.CreatePool<Projectile>(projectilePatternRings.ProjectilePrefab, projectilePatternRings.BaseProjectilesCount * initialPoolSize);
    }
    ObjectPool.CreatePool<ProjectileCircleBase>(this.projectilePatternRings, initialPoolSize);
  }

  public IEnumerator ShootRingsTargeted()
  {
    EnemyGuardian2 enemyGuardian2 = this;
    yield return (object) enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.Teleport(enemyGuardian2.CentreOfLevel));
    enemyGuardian2.simpleSpineAnimator.Animate("floating-start", 0, true);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("floating-spin", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; (double) i < (double) UnityEngine.Random.Range(enemyGuardian2.projectilePatternRingsAmount.x, enemyGuardian2.projectilePatternRingsAmount.y); ++i)
    {
      Projectile component = ObjectPool.Spawn<ProjectileCircleBase>(enemyGuardian2.projectilePatternRings, enemyGuardian2.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyGuardian2.transform.position;
      component.Angle = enemyGuardian2.GetAngleToPlayer();
      component.health = enemyGuardian2.health;
      component.team = Health.Team.Team2;
      component.Speed = enemyGuardian2.projectilePatternRingsSpeed;
      component.Acceleration = enemyGuardian2.projectilePatternRingsAcceleration;
      component.GetComponent<ProjectileCircleBase>().InitDelayed(enemyGuardian2.TargetObject, enemyGuardian2.projectilePatternRingsRadius, enemyGuardian2.projectilePatternShootDelay, new System.Action(enemyGuardian2.\u003CShootRingsTargeted\u003Eb__51_0));
      yield return (object) new WaitForSeconds(enemyGuardian2.projectilePatternRingsTimeBetween);
    }
    enemyGuardian2.simpleSpineAnimator.Animate("floating-stop", 0, false);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("Coughing", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1.5f);
    enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.TeleportAway());
  }

  public IEnumerator ShootRingsScattered()
  {
    EnemyGuardian2 enemyGuardian2 = this;
    yield return (object) enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.Teleport(enemyGuardian2.CentreOfLevel));
    enemyGuardian2.simpleSpineAnimator.Animate("floating-start", 0, true);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("floating-spin", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; (double) i < (double) UnityEngine.Random.Range(enemyGuardian2.projectilePatternScatteredAmount.x, enemyGuardian2.projectilePatternScatteredAmount.y); ++i)
    {
      Projectile component = ObjectPool.Spawn<ProjectileCircleBase>(enemyGuardian2.projectilePatternRings, enemyGuardian2.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyGuardian2.transform.position;
      component.Angle = UnityEngine.Random.Range(0.0f, 360f);
      component.health = enemyGuardian2.health;
      component.team = Health.Team.Team2;
      component.Speed = enemyGuardian2.projectilePatternScattedSpeed;
      component.Deceleration = UnityEngine.Random.Range(2f, 7f);
      component.GetComponent<ProjectileCircleBase>().InitDelayed(i % 4 == 0 ? enemyGuardian2.TargetObject : (GameObject) null, enemyGuardian2.projectileScatteredRadius, 0.0f, new System.Action(enemyGuardian2.\u003CShootRingsScattered\u003Eb__52_0));
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyGuardian2.projectilePatternScatteredDelay.x, enemyGuardian2.projectilePatternScatteredDelay.y));
    }
    yield return (object) new WaitForSeconds(enemyGuardian2.projectilePatternScatteredDelay.y + 0.5f);
    enemyGuardian2.simpleSpineAnimator.Animate("floating-stop", 0, false);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("Coughing", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1.5f);
    enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.TeleportAway());
  }

  public float GetAngleToPlayer()
  {
    return !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) ? 0.0f : Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position);
  }

  public IEnumerator TeleportAndShoot()
  {
    EnemyGuardian2 enemyGuardian2 = this;
    int NumAttacks = 2;
    bool Loop = true;
    enemyGuardian2.TargetObject = enemyGuardian2.ReconsiderPlayerTarget().gameObject;
    while (Loop)
    {
      float degree = (float) UnityEngine.Random.Range(0, 360);
      float distance = 6f;
      int num = 0;
      while (num++ < 32 /*0x20*/ && (bool) Physics2D.Raycast((Vector2) enemyGuardian2.transform.position, Utils.DegreeToVector2(degree), distance, (int) enemyGuardian2.layerToCheck))
        degree += Utils.Repeat((float) UnityEngine.Random.Range(0, 360), 360f);
      float f = degree * ((float) Math.PI / 180f);
      Vector3 Destination = enemyGuardian2.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      yield return (object) enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.Teleport(Destination));
      yield return (object) new WaitForSeconds(0.5f);
      enemyGuardian2.simpleSpineAnimator.Animate("summon-fast", 0, false);
      enemyGuardian2.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      yield return (object) enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.TrapPatternChasePlayer(UnityEngine.Random.Range(8, 15)));
      yield return (object) new WaitForSeconds(1f);
      if (--NumAttacks <= 0)
        Loop = false;
      yield return (object) null;
    }
    if (UnityEngine.Random.Range(0, 2) == 0)
      enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.ShootRingsScattered());
    else
      enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.ShootRingsTargeted());
  }

  public IEnumerator SpawnEnemies()
  {
    EnemyGuardian2 enemyGuardian2 = this;
    yield return (object) enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.Teleport(enemyGuardian2.CentreOfLevel));
    yield return (object) new WaitForSeconds(1f);
    enemyGuardian2.simpleSpineAnimator.Animate("floating-start", 0, true);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("floating-spin", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    bool Loop = true;
    int WaveCount = enemyGuardian2.NumWaves;
    while (Loop)
    {
      int Count = -1;
      while (++Count < enemyGuardian2.TargetEnemies && enemyGuardian2.enemiesAlive < 13)
      {
        int num = 5;
        Vector3 Position = enemyGuardian2.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * (float) num);
        GameObject gameObject = EnemySpawner.Create(Position, enemyGuardian2.transform.parent, enemyGuardian2.EnemyToSpawn[UnityEngine.Random.Range(0, enemyGuardian2.EnemyToSpawn.Length)]);
        enemyGuardian2.enemies.Add(gameObject);
        gameObject.GetComponent<UnitObject>().CanHaveModifier = false;
        gameObject.GetComponent<UnitObject>().RemoveModifier();
        gameObject.GetComponent<Health>().OnDie += new Health.DieAction(enemyGuardian2.EnemyDied);
        ++enemyGuardian2.enemiesAlive;
        CameraManager.shakeCamera(0.3f, Utils.GetAngle(enemyGuardian2.transform.position, enemyGuardian2.transform.position + Position));
        yield return (object) new WaitForSeconds(0.2f);
      }
      yield return (object) new WaitForSeconds(0.5f);
      GameManager.GetInstance().RemoveFromCamera(enemyGuardian2.gameObject);
      enemyGuardian2.simpleSpineAnimator.Animate("floating", 0, true);
      yield return (object) new WaitForSeconds(3f);
      if (--WaveCount <= 0)
        Loop = false;
      else
        enemyGuardian2.simpleSpineAnimator.Animate("floating-spin", 0, true);
    }
    if (enemyGuardian2.NumWaves < 3)
      ++enemyGuardian2.NumWaves;
    if (enemyGuardian2.TargetEnemies < 6)
      ++enemyGuardian2.TargetEnemies;
    float BatTimer = 0.0f;
    while ((double) (BatTimer += Time.deltaTime) < 5.0 && FormationFighter.fighters.Count > 0)
      yield return (object) null;
    enemyGuardian2.simpleSpineAnimator.Animate("floating-stop", 0, false);
    enemyGuardian2.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForSeconds(2f);
    enemyGuardian2.StartCoroutine((IEnumerator) enemyGuardian2.TeleportAway());
  }

  public void EnemyDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.enemiesAlive;
  }

  public void InitializeTraps() => ObjectPool.CreatePool(this.Trap, 40);

  public IEnumerator TrapPattern0()
  {
    EnemyGuardian2 enemyGuardian2 = this;
    enemyGuardian2.state.facingAngle = Utils.GetAngle(enemyGuardian2.transform.position, enemyGuardian2.TargetObject.transform.position);
    float Angle = enemyGuardian2.state.facingAngle * ((float) Math.PI / 180f);
    int i = -1;
    while (++i < 10)
    {
      GameObject gameObject = ObjectPool.Spawn(enemyGuardian2.Trap);
      float num = (float) (i * 2);
      Vector3 vector3 = new Vector3(num * Mathf.Cos(Angle), num * Mathf.Sin(Angle));
      gameObject.transform.position = enemyGuardian2.transform.position + vector3;
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.1f);
    }
  }

  public IEnumerator TrapPatternChasePlayer(int NumToSpawn)
  {
    EnemyGuardian2 enemyGuardian2 = this;
    Vector3 Position = Vector3.zero;
    int i = -1;
    float Dist = 1f;
    enemyGuardian2.state.facingAngle = Utils.GetAngle(enemyGuardian2.transform.position, enemyGuardian2.TargetObject.transform.position);
    float facingAngle = enemyGuardian2.state.facingAngle;
    while (++i < NumToSpawn)
    {
      GameObject gameObject = ObjectPool.Spawn(enemyGuardian2.Trap);
      float angle = Utils.GetAngle(enemyGuardian2.transform.position + Position, enemyGuardian2.TargetObject.transform.position);
      Position += new Vector3(Dist * Mathf.Cos(angle * ((float) Math.PI / 180f)), Dist * Mathf.Sin(angle * ((float) Math.PI / 180f)));
      gameObject.transform.position = enemyGuardian2.transform.position + Position;
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.15f);
    }
  }

  public IEnumerator ShowLineRenderer(Vector3 Destination)
  {
    float Progress = 1f;
    Color c = Color.white;
    Gradient gradient = new Gradient();
    while ((double) (Progress -= Time.deltaTime) >= 0.0)
    {
      if ((double) Progress < 0.0)
        Progress = 0.0f;
      gradient.SetKeys(new GradientColorKey[2]
      {
        new GradientColorKey(c, 0.0f),
        new GradientColorKey(c, 1f)
      }, new GradientAlphaKey[2]
      {
        new GradientAlphaKey(Progress, 0.0f),
        new GradientAlphaKey(Progress, 1f)
      });
      yield return (object) null;
    }
  }

  public Vector3 TargetPosition => this.TargetObject.transform.position;

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public void OnLanguageChanged()
  {
    UIBossHUD.Instance?.UpdateName(LocalizationManager.GetTranslation(this.DisplayName));
  }

  public void OnDrawGizmos() => Utils.DrawCircleXY(this.CentreOfLevel, 0.4f, Color.blue);

  [CompilerGenerated]
  public void \u003CShootRingsTargeted\u003Eb__51_0()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CShootRingsScattered\u003Eb__52_0()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.gameObject);
  }
}
