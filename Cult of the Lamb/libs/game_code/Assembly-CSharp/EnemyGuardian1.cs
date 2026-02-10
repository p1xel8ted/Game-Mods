// Decompiled with JetBrains decompiler
// Type: EnemyGuardian1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyGuardian1 : UnitObject
{
  public GameObject TargetObject;
  public Health EnemyHealth;
  public bool Active = true;
  public ColliderEvents damageColliderEvents;
  public SimpleSpineAnimator simpleSpineAnimator;
  public SimpleSpineEventListener simpleSpineEventListener;
  public List<Collider2D> collider2DList;
  public Transform CameraBone;
  public ParticleSystem Particles;
  public ParticleSystem DashParticles;
  public EnemyGuardian2 Guardian2;
  public Collider2D HealthCollider;
  public Vector3 CentreOfLevel;
  public GameObject CenterOfLevelObject;
  [TermsPopup("")]
  public string DisplayName;
  [EventRef]
  public string attackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  public bool active;
  public int PlayCoughing;
  public DeadBodySliding deadBodySliding;
  public GameObject Trap;
  public int TrapPattern;
  public GameObject[] Enemies;
  public LineRenderer lineRenderer;
  public int TargetBats = 2;
  public List<GameObject> spawnedEnemies = new List<GameObject>();

  public override void Awake()
  {
    base.Awake();
    this.InitializeTraps();
    this.health.BlackSoulOnHit = true;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.TargetObject = this.ReconsiderPlayerTarget().gameObject;
    if (GameManager.RoomActive)
    {
      if (!this.active)
      {
        this.simpleSpineAnimator = this.GetComponentInChildren<SimpleSpineAnimator>();
        this.simpleSpineEventListener = this.GetComponent<SimpleSpineEventListener>();
        this.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
        this.Particles.Stop();
        this.state.facingAngle = 180f;
        this.HealthCollider = this.GetComponent<Collider2D>();
        this.HealthCollider.enabled = false;
        this.health.invincible = true;
        if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
        {
          this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
          this.damageColliderEvents.SetActive(false);
        }
        this.active = true;
      }
      else
      {
        this.Particles.Stop();
        this.health.invincible = false;
        this.HealthCollider.enabled = true;
        this.StartCoroutine((IEnumerator) this.FightPlayer());
        UIBossHUD.Play(this.health, LocalizationManager.GetTranslation(this.DisplayName));
      }
      AudioManager.Instance.SetMusicRoomID(1, "deathcat_room_id");
    }
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
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
    this.lineRenderer.gameObject.SetActive(false);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.Die());
    this.GetComponent<Collider2D>().enabled = false;
    GameManager.GetInstance().RemoveFromCamera(this.gameObject);
    UIBossHUD.Hide();
    foreach (GameObject spawnedEnemy in this.spawnedEnemies)
    {
      if ((bool) (UnityEngine.Object) spawnedEnemy)
      {
        Health component = spawnedEnemy.GetComponent<Health>();
        component.enabled = true;
        component.invincible = false;
        component.DealDamage(float.MaxValue, component.gameObject, component.transform.position);
      }
    }
  }

  public IEnumerator Die()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    enemyGuardian1.state.CURRENT_STATE = StateMachine.State.Dead;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(enemyGuardian1.CameraBone.gameObject, 7f);
    yield return (object) new WaitForEndOfFrame();
    enemyGuardian1.simpleSpineAnimator.Animate("dead", 0, false);
    yield return (object) new WaitForSeconds(4f);
    if ((double) UnityEngine.Random.value < 0.33000001311302185)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, enemyGuardian1.transform.position + Vector3.back);
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RED_HEART, 1, enemyGuardian1.transform.position + Vector3.back);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(enemyGuardian1.transform.position);
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) enemyGuardian1.gameObject);
    if (PlayerFarming.IsAnyPlayerKnockedOut())
    {
      CoopManager.Instance.OnKnockedPlayerAwoken += new System.Action(enemyGuardian1.ActivateTheNextPhase);
      CoopManager.Instance.WakeAllKnockedOutPlayersWithHealth();
    }
    else
      enemyGuardian1.ActivateTheNextPhase();
  }

  public void ActivateTheNextPhase()
  {
    CoopManager.Instance.OnKnockedPlayerAwoken -= new System.Action(this.ActivateTheNextPhase);
    this.Guardian2.Activate();
  }

  public IEnumerator Coughing()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyGuardian1 enemyGuardian1 = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyGuardian1.simpleSpineAnimator.Animate("idle", 0, true);
      enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.SpawnTraps());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyGuardian1.simpleSpineAnimator.Animate(nameof (Coughing), 0, true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Start() => this.CentreOfLevel = this.CenterOfLevelObject.transform.position;

  public void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "Intro Complete":
        GameManager.GetInstance().OnConversationEnd();
        this.health.invincible = false;
        this.HealthCollider.enabled = true;
        this.StartCoroutine((IEnumerator) this.FightPlayer());
        AudioManager.Instance.SetMusicRoomID(1, "deathcat_room_id");
        UIBossHUD.Play(this.health, LocalizationManager.GetTranslation(this.DisplayName));
        GameManager.GetInstance().AddToCamera(this.gameObject);
        GameManager.GetInstance().AddPlayersToCamera();
        PlayerFarming.SetStateForAllPlayers();
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

  public void Play()
  {
    Debug.Log((object) "PLAY!");
    this.StartCoroutine((IEnumerator) this.Activate());
  }

  public IEnumerator Activate()
  {
    Debug.Log((object) "Activate!");
    GameManager.GetInstance().OnConversationNext(this.CameraBone.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    HUD_DisplayName.Play(LocalizationManager.GetTranslation(this.DisplayName), 2, HUD_DisplayName.Positions.Centre, HUD_DisplayName.textBlendMode.DungeonFinal);
    this.simpleSpineAnimator.Animate("intro", 0, false);
    this.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
  }

  public override void Update()
  {
    base.Update();
    foreach (GameObject spawnedEnemy in this.spawnedEnemies)
    {
      if ((UnityEngine.Object) spawnedEnemy == (UnityEngine.Object) null)
      {
        this.spawnedEnemies.Remove(spawnedEnemy);
        break;
      }
    }
  }

  public IEnumerator FightPlayer()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    enemyGuardian1.TargetObject = enemyGuardian1.ReconsiderPlayerTarget().gameObject;
    enemyGuardian1.givePath(enemyGuardian1.TargetObject.transform.position);
    float RepathTimer = 0.0f;
    int NumAttacks = 3;
    float AttackSpeed = 25f;
    bool Loop = true;
    while (Loop)
    {
      switch (enemyGuardian1.state.CURRENT_STATE)
      {
        case StateMachine.State.Moving:
          if ((double) Vector2.Distance((Vector2) enemyGuardian1.transform.position, (Vector2) enemyGuardian1.TargetPosition) < 3.0)
          {
            enemyGuardian1.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            enemyGuardian1.simpleSpineAnimator.Animate("attack" + (4 - NumAttacks).ToString(), 0, false);
            enemyGuardian1.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
          }
          else if ((double) (RepathTimer += Time.deltaTime) > 0.20000000298023224)
          {
            RepathTimer = 0.0f;
            enemyGuardian1.givePath(enemyGuardian1.TargetObject.transform.position);
          }
          if ((UnityEngine.Object) enemyGuardian1.damageColliderEvents != (UnityEngine.Object) null)
          {
            enemyGuardian1.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          enemyGuardian1.state.facingAngle = Utils.GetAngle(enemyGuardian1.transform.position, enemyGuardian1.TargetPosition);
          if ((double) (enemyGuardian1.state.Timer += Time.deltaTime) >= 0.5)
          {
            enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.EnableCollider());
            CameraManager.shakeCamera(0.4f, enemyGuardian1.state.facingAngle);
            enemyGuardian1.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            enemyGuardian1.speed = AttackSpeed * Time.deltaTime;
            if (!string.IsNullOrEmpty(enemyGuardian1.attackSoundPath))
              AudioManager.Instance.PlayOneShot(enemyGuardian1.attackSoundPath, enemyGuardian1.transform.position);
          }
          if ((UnityEngine.Object) enemyGuardian1.damageColliderEvents != (UnityEngine.Object) null)
          {
            enemyGuardian1.damageColliderEvents.SetActive(false);
            break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) AttackSpeed > 0.0)
            AttackSpeed -= 1f * GameManager.DeltaTime;
          enemyGuardian1.speed = AttackSpeed * Time.deltaTime;
          if ((double) (enemyGuardian1.state.Timer += Time.deltaTime) >= 0.5)
          {
            if (--NumAttacks > 0)
            {
              AttackSpeed = (float) (25 + (3 - NumAttacks) * 2);
              enemyGuardian1.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
              enemyGuardian1.simpleSpineAnimator.Animate("attack" + (4 - NumAttacks).ToString(), 0, false);
              enemyGuardian1.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
              break;
            }
            Loop = false;
            enemyGuardian1.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          break;
      }
      yield return (object) null;
    }
    if ((UnityEngine.Object) enemyGuardian1.damageColliderEvents != (UnityEngine.Object) null)
      enemyGuardian1.damageColliderEvents.SetActive(false);
    if (UnityEngine.Random.Range(0, 4) < 3)
      enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.SpawnTraps());
    else
      enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.FightPlayer());
  }

  public IEnumerator EnableCollider()
  {
    yield return (object) new WaitForSeconds(0.1f);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(true);
    yield return (object) new WaitForSeconds(0.1f);
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(false);
  }

  public IEnumerator SpawnTraps()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    if (enemyGuardian1.active)
    {
      enemyGuardian1.simpleSpineAnimator.Animate("summon", 0, false);
      enemyGuardian1.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      enemyGuardian1.state.facingAngle = Utils.GetAngle(enemyGuardian1.transform.position, enemyGuardian1.TargetObject.transform.position);
      yield return (object) new WaitForSeconds(1.7f);
      enemyGuardian1.TrapPattern = UnityEngine.Random.Range(0, 3);
      switch (enemyGuardian1.TrapPattern)
      {
        case 0:
          yield return (object) enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.TrapPattern0());
          break;
        case 1:
          yield return (object) enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.TrapPatternChasePlayer());
          break;
        case 2:
          yield return (object) enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.TrapPattern1());
          break;
      }
      yield return (object) new WaitForSeconds(0.5f);
      if (enemyGuardian1.spawnedEnemies.Count <= 1)
        enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.SpawnEnemies());
      else
        enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.FightPlayer());
    }
  }

  public void InitializeTraps() => ObjectPool.CreatePool(this.Trap, 40);

  public IEnumerator TrapPattern0()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    if (enemyGuardian1.active)
    {
      enemyGuardian1.state.facingAngle = Utils.GetAngle(enemyGuardian1.transform.position, enemyGuardian1.TargetObject.transform.position);
      int i = -1;
      while (++i < 10)
      {
        int num1 = -1;
        while (++num1 < 4)
        {
          GameObject gameObject = ObjectPool.Spawn(enemyGuardian1.Trap);
          float f = (float) (((double) enemyGuardian1.state.facingAngle + (double) (90 * num1)) * (Math.PI / 180.0));
          float num2 = (float) (i * 2);
          Vector3 vector3 = new Vector3(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
          gameObject.transform.position = enemyGuardian1.transform.position + vector3;
        }
        CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
        yield return (object) new WaitForSeconds(0.1f);
      }
    }
  }

  public IEnumerator TrapPattern1()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    if (enemyGuardian1.active)
    {
      int i = -1;
      while (++i < 10)
      {
        int num1 = -1;
        while (++num1 < 1)
        {
          enemyGuardian1.state.facingAngle = Utils.GetAngle(enemyGuardian1.transform.position, enemyGuardian1.TargetObject.transform.position);
          GameObject gameObject = ObjectPool.Spawn(enemyGuardian1.Trap);
          float f = (float) (((double) enemyGuardian1.state.facingAngle + (double) (90 * num1)) * (Math.PI / 180.0));
          float num2 = (float) (2 + i * 2);
          Vector3 vector3 = new Vector3(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
          gameObject.transform.position = enemyGuardian1.transform.position + vector3;
        }
        CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
        yield return (object) new WaitForSeconds(0.1f);
      }
    }
  }

  public IEnumerator TrapPattern2()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    if (enemyGuardian1.active)
    {
      Vector3 vector3 = enemyGuardian1.CentreOfLevel;
      int i = -1;
      float Dist = 1f;
      enemyGuardian1.state.facingAngle = Utils.GetAngle(enemyGuardian1.transform.position, enemyGuardian1.TargetObject.transform.position);
      float Angle = enemyGuardian1.state.facingAngle;
      while (++i < 20)
      {
        GameObject gameObject = ObjectPool.Spawn(enemyGuardian1.Trap);
        Angle += 10f;
        Dist += 0.5f;
        vector3 = new Vector3(Dist * Mathf.Cos(Angle * ((float) Math.PI / 180f)), Dist * Mathf.Sin(Angle * ((float) Math.PI / 180f)));
        gameObject.transform.position = enemyGuardian1.transform.position + vector3;
        CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
        yield return (object) new WaitForSeconds(0.1f);
      }
    }
  }

  public IEnumerator TrapPatternChasePlayer()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    if (enemyGuardian1.active)
    {
      Vector3 Position = enemyGuardian1.CentreOfLevel;
      int i = -1;
      float Dist = 1f;
      enemyGuardian1.state.facingAngle = Utils.GetAngle(enemyGuardian1.transform.position, enemyGuardian1.TargetObject.transform.position);
      float facingAngle = enemyGuardian1.state.facingAngle;
      while (++i < 20)
      {
        GameObject gameObject = ObjectPool.Spawn(enemyGuardian1.Trap);
        float angle = Utils.GetAngle(enemyGuardian1.transform.position + Position, enemyGuardian1.TargetObject.transform.position);
        Position += new Vector3(Dist * Mathf.Cos(angle * ((float) Math.PI / 180f)), Dist * Mathf.Sin(angle * ((float) Math.PI / 180f)));
        gameObject.transform.position = enemyGuardian1.transform.position + Position;
        CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
        yield return (object) new WaitForSeconds(0.1f);
      }
    }
  }

  public IEnumerator ShowLineRenderer()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    enemyGuardian1.lineRenderer.gameObject.SetActive(true);
    enemyGuardian1.lineRenderer.SetPosition(0, enemyGuardian1.transform.position + Vector3.back * 1f);
    enemyGuardian1.lineRenderer.SetPosition(1, enemyGuardian1.CentreOfLevel + Vector3.back * 1f);
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
      enemyGuardian1.lineRenderer.colorGradient = gradient;
      yield return (object) null;
    }
    enemyGuardian1.lineRenderer.gameObject.SetActive(false);
  }

  public IEnumerator SpawnEnemies()
  {
    EnemyGuardian1 enemyGuardian1 = this;
    yield return (object) new WaitForSeconds(1f);
    if (enemyGuardian1.active)
    {
      enemyGuardian1.simpleSpineAnimator.Animate("dash", 0, false);
      enemyGuardian1.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      yield return (object) new WaitForSeconds(0.15f);
      CameraManager.shakeCamera(0.5f, Utils.GetAngle(enemyGuardian1.transform.position, enemyGuardian1.CentreOfLevel));
      BiomeConstants.Instance.SpawnInWhite.Spawn().transform.position = enemyGuardian1.transform.position + Vector3.down * 2f;
      enemyGuardian1.transform.position = enemyGuardian1.CentreOfLevel;
      BiomeConstants.Instance.SpawnInWhite.Spawn().transform.position = enemyGuardian1.transform.position + Vector3.down * 2f;
      yield return (object) new WaitForSeconds(1f);
      enemyGuardian1.simpleSpineAnimator.Animate("floating-start", 0, true);
      enemyGuardian1.simpleSpineAnimator.AddAnimate("floating-spin", 0, true, 0.0f);
      yield return (object) new WaitForSeconds(0.5f);
      yield return (object) new WaitForSeconds(1.5f);
      int Count = -1;
      while (++Count < enemyGuardian1.TargetBats)
      {
        GameObject gameObject = EnemySpawner.Create(new Vector3(2f * Mathf.Cos((float) (Count * (360 / enemyGuardian1.TargetBats)) * ((float) Math.PI / 180f)), 2f * Mathf.Sin((float) (Count * (360 / enemyGuardian1.TargetBats)) * ((float) Math.PI / 180f))), enemyGuardian1.transform.parent, enemyGuardian1.Enemies[UnityEngine.Random.Range(0, enemyGuardian1.Enemies.Length)]);
        gameObject.GetComponent<UnitObject>().CanHaveModifier = false;
        gameObject.GetComponent<UnitObject>().RemoveModifier();
        enemyGuardian1.spawnedEnemies.Add(gameObject);
        yield return (object) new WaitForSeconds(0.1f);
      }
      if (enemyGuardian1.TargetBats < 3)
        ++enemyGuardian1.TargetBats;
      yield return (object) new WaitForSeconds(0.5f);
      enemyGuardian1.simpleSpineAnimator.AddAnimate("floating", 0, true, 0.0f);
      enemyGuardian1.simpleSpineAnimator.Animate("floating-stop", 0, false);
      enemyGuardian1.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      yield return (object) new WaitForSeconds(0.5f);
      yield return (object) new WaitForSeconds(1f);
      enemyGuardian1.StartCoroutine((IEnumerator) enemyGuardian1.FightPlayer());
    }
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    Debug.Log((object) nameof (OnDamageTriggerEnter).Colour(Color.red));
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public Vector3 TargetPosition => this.TargetObject.transform.position;

  public void OnLanguageChanged()
  {
    UIBossHUD.Instance?.UpdateName(LocalizationManager.GetTranslation(this.DisplayName));
  }

  public void OnDrawGizmos() => Utils.DrawCircleXY(this.CentreOfLevel, 0.4f, Color.blue);
}
