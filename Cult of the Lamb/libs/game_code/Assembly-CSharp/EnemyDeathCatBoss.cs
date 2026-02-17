// Decompiled with JetBrains decompiler
// Type: EnemyDeathCatBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using CotL.Projectiles;
using DG.Tweening;
using I2.Loc;
using Spine;
using Spine.Unity;
using src.Data;
using src.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyDeathCatBoss : UnitObject
{
  public static EnemyDeathCatBoss Instance;
  public SkeletonAnimation Spine;
  public SkeletonAnimation BaseFormSpine;
  public SimpleSpineFlash SimpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string appearAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string disappearAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string handSlamAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string summonAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string dieNoHeartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string dieHeartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string deadNoHeartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string deadHeartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string handStartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string handLoopAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string handEndAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string meleeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string hurtAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string downStartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string downIdleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string downAttackAnticipateAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string downAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string downEndAnimation;
  [SerializeField]
  public GameObject trapPrefab;
  [SerializeField]
  public float projectilePatternAnticipation;
  [SerializeField]
  public ProjectilePatternBase projectilePattern1;
  [SerializeField]
  public ProjectilePatternBase projectilePattern2;
  [SerializeField]
  public ProjectilePatternBase projectilePattern3;
  [SerializeField]
  public ProjectilePatternBase projectilePattern4;
  [SerializeField]
  public ProjectilePatternBase leftSidePattern1;
  [SerializeField]
  public ProjectilePatternBase rightSidePattern1;
  [SerializeField]
  public Vector2 poisonAmount;
  [SerializeField]
  public float poisonDelayBetween;
  [SerializeField]
  public PoisonBomb poisonBombPrefab;
  [Space]
  [SerializeField]
  public float projectilePatternLineDuration;
  [SerializeField]
  public float projectilePatternLineSpeed;
  [SerializeField]
  public float projectilePatternLineAcceleration;
  [SerializeField]
  public ProjectileLine projectilePatternLine;
  [SerializeField]
  public float projectilePatternLineVerticalAnticipation;
  [SerializeField]
  public float projectilePatternLineVerticalSpeed;
  [SerializeField]
  public ProjectileLine projectilePatternVertical;
  [Space]
  [SerializeField]
  public float projectileRingBigDuration;
  [SerializeField]
  public float projectileRingBigSpeed;
  [SerializeField]
  public float projectileRingBigAcceleration;
  [SerializeField]
  public float projectileRingBigLifetime;
  [SerializeField]
  public ProjectileCircle projectileRingBig;
  [SerializeField]
  public float projectilePatternRingsDuration;
  [SerializeField]
  public float projectilePatternRingsSpeed;
  [SerializeField]
  public float projectilePatternRingsAcceleration;
  [SerializeField]
  public float projectilePatternRingsRadius;
  [SerializeField]
  public ProjectileCircleBase projectilePatternRings;
  [SerializeField]
  public ColliderEvents meleeCollider;
  [SerializeField]
  public TrapRockFall rockFallPrefab;
  [SerializeField]
  public float rockFallDuration;
  [SerializeField]
  public Vector2 rockFallAmount;
  [SerializeField]
  public Vector2 rockFallDelay;
  [SerializeField]
  public MortarBomb bombPrefab;
  [SerializeField]
  public float bombMoveDuration;
  [SerializeField]
  public Vector2 bombAmount;
  [SerializeField]
  public Vector2 bombDelayBetween;
  [SerializeField]
  public DeathCatClone clonePrefab;
  [SerializeField]
  public EnemyDeathCatBoss.CloneWave[] cloneAttack1;
  [SerializeField]
  public EnemyDeathCatBoss.CloneWave[] cloneAttack2;
  [SerializeField]
  public EnemyDeathCatBoss.CloneWave[] cloneAttack3;
  [SerializeField]
  public EnemyDeathCatBoss.CloneWave[] cloneAttack4;
  [SerializeField]
  public EnemyDeathCatBoss.CloneWave[] cloneAttack5;
  [SerializeField]
  public EnemyRoundsBase enemyRounds1;
  [SerializeField]
  public EnemyRoundsBase enemyRounds2;
  [SerializeField]
  public EnemyRoundsBase enemyRounds3;
  [SerializeField]
  public EnemyRoundsBase enemyRounds4;
  [SerializeField]
  public GameObject cameraBone;
  [SerializeField]
  public GameObject cinematicBone;
  [SerializeField]
  public GameObject middleBone1;
  [SerializeField]
  public GameObject middleBone2;
  [SerializeField]
  public GameObject enemyDeathCatEyesManager;
  [SerializeField]
  public Collider2D blockingCollider;
  [CompilerGenerated]
  public List<DeathCatClone> \u003CCurrentActiveClones\u003Ek__BackingField = new List<DeathCatClone>();
  public List<DeathCatClone> currentDeadClones = new List<DeathCatClone>();
  public List<GameObject> spawnedEnemies = new List<GameObject>();
  public Coroutine currentMainAttackRoutine;
  public float attackTimestamp;
  public EnemyDeathCatBoss.AttackType previousAttackType;
  [CompilerGenerated]
  public bool \u003CCanAttack\u003Ek__BackingField;

  public GameObject CameraBone => this.cameraBone;

  public GameObject CinematicBone => this.cinematicBone;

  public List<DeathCatClone> CurrentActiveClones
  {
    get => this.\u003CCurrentActiveClones\u003Ek__BackingField;
    set => this.\u003CCurrentActiveClones\u003Ek__BackingField = value;
  }

  public bool CanAttack
  {
    get => this.\u003CCanAttack\u003Ek__BackingField;
    set => this.\u003CCanAttack\u003Ek__BackingField = value;
  }

  public override void Awake()
  {
    base.Awake();
    this.InitializeProjectilePatternRings();
    this.InitializeMortarStrikes();
    this.InitializeTraps();
    this.InitializeProjectileLines();
    this.health.enabled = false;
  }

  public void Start()
  {
    this.enemyRounds1.SpawnDelay = 0.5f;
    this.enemyRounds2.SpawnDelay = 0.5f;
    this.enemyRounds3.SpawnDelay = 0.5f;
    this.enemyRounds4.SpawnDelay = 0.5f;
    this.attackTimestamp = GameManager.GetInstance().CurrentTime + 10f;
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    this.meleeCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.MeleeColliderHit);
    this.meleeCollider.gameObject.SetActive(false);
    this.health.enabled = true;
    this.health.invincible = true;
    this.enemyDeathCatEyesManager.SetActive(true);
    foreach (Behaviour eye in this.enemyDeathCatEyesManager.GetComponent<EnemyDeathCatEyesManager>().Eyes)
      eye.enabled = true;
    foreach (Behaviour component in this.GetComponents<Collider2D>())
      component.enabled = false;
  }

  public void MeleeColliderHit(Collider2D collider)
  {
    if (!collider.CompareTag("Player"))
      return;
    PlayerFarming.Instance.health.DealDamage(1f, this.gameObject, PlayerFarming.Instance.transform.position, false, Health.AttackTypes.Melee, false, (Health.AttackFlags) 0);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "slam")
    {
      CameraManager.instance.ShakeCameraForDuration(2f, 2f, 0.5f);
      MMVibrate.Rumble(1f, 2f, 0.5f, (MonoBehaviour) this);
    }
    else
    {
      if (!(e.Data.Name == "Deathcat SweepAttack"))
        return;
      this.meleeCollider.SetActive(true);
      this.StartCoroutine((IEnumerator) this.DelayCallback(0.1f, (System.Action) (() => this.meleeCollider.SetActive(false))));
    }
  }

  public IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public override void Update()
  {
    base.Update();
    if ((double) this.health.HP <= 0.0)
      return;
    if (this.currentMainAttackRoutine == null && (double) this.attackTimestamp == -1.0)
      this.attackTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(1.5f, 2f);
    if (this.currentMainAttackRoutine == null && this.CanAttack)
    {
      if ((double) GameManager.GetInstance().CurrentTime > (double) this.attackTimestamp && (double) this.attackTimestamp != -1.0)
      {
        this.SecondaryAttack();
      }
      else
      {
        if ((double) this.attackTimestamp != -1.0)
          return;
        this.attackTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(1.5f, 2f);
      }
    }
    else
      this.attackTimestamp = -1f;
  }

  public void BeginPhase1() => UIBossHUD.Play(this.health, ScriptLocalization.NAMES.DeathNPC);

  public void MainAttack(System.Action callback)
  {
    this.attackTimestamp = -1f;
    EnemyDeathCatBoss.AttackType attackType = this.previousAttackType;
    while (attackType == this.previousAttackType)
    {
      switch (UnityEngine.Random.Range(DataManager.Instance.PlayerFleece == 9 ? 1 : 0, 4))
      {
        case 0:
          attackType = EnemyDeathCatBoss.AttackType.LinesMulti;
          continue;
        case 1:
          attackType = EnemyDeathCatBoss.AttackType.RingsMulti;
          continue;
        case 2:
          attackType = EnemyDeathCatBoss.AttackType.Pattern2;
          continue;
        case 3:
          attackType = EnemyDeathCatBoss.AttackType.SidePattern1;
          continue;
        default:
          continue;
      }
    }
    this.Attack(attackType);
    this.StartCoroutine((IEnumerator) this.WaitForAttackToFinish(callback));
  }

  public IEnumerator WaitForAttackToFinish(System.Action callback)
  {
    while (this.currentMainAttackRoutine != null)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SecondaryAttack()
  {
    this.attackTimestamp = -1f;
    EnemyDeathCatBoss.AttackType attackType = this.previousAttackType;
    while (attackType == this.previousAttackType)
    {
      switch (UnityEngine.Random.Range(0, 3))
      {
        case 0:
          attackType = EnemyDeathCatBoss.AttackType.ProjectilePattern1;
          continue;
        case 1:
          attackType = EnemyDeathCatBoss.AttackType.TrapPattern0;
          continue;
        case 2:
          attackType = EnemyDeathCatBoss.AttackType.TrapPattern1;
          continue;
        default:
          continue;
      }
    }
    if (EnemyDeathCatEyesManager.Instance.Eyes.Count == 1 && UnityEngine.Random.Range(0, 100) <= 60)
    {
      attackType = this.previousAttackType;
      while (attackType == this.previousAttackType)
      {
        switch (UnityEngine.Random.Range(DataManager.Instance.PlayerFleece == 9 ? 1 : 0, 4))
        {
          case 0:
            attackType = EnemyDeathCatBoss.AttackType.LinesMulti;
            continue;
          case 1:
            attackType = EnemyDeathCatBoss.AttackType.RingsMulti;
            continue;
          case 2:
            attackType = EnemyDeathCatBoss.AttackType.Pattern2;
            continue;
          case 3:
            attackType = EnemyDeathCatBoss.AttackType.SidePattern1;
            continue;
          default:
            continue;
        }
      }
    }
    this.Attack(attackType);
  }

  public void Attack(EnemyDeathCatBoss.AttackType attackType)
  {
    this.previousAttackType = attackType;
    switch (attackType)
    {
      case EnemyDeathCatBoss.AttackType.LineVertical:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileLineIE(true));
        break;
      case EnemyDeathCatBoss.AttackType.RingsMulti:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsMultiIE());
        break;
      case EnemyDeathCatBoss.AttackType.Pattern2:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectilePattern2IE());
        break;
      case EnemyDeathCatBoss.AttackType.LinesMulti:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileLinesMultiIE());
        break;
      case EnemyDeathCatBoss.AttackType.SidePattern1:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectileSidePattern1IE());
        break;
      case EnemyDeathCatBoss.AttackType.RockFall:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ScatterRockFallIE());
        break;
      case EnemyDeathCatBoss.AttackType.TargetedBombs:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.TargetedBombsIE());
        break;
      case EnemyDeathCatBoss.AttackType.Melee:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.MeleeAttackIE());
        break;
      case EnemyDeathCatBoss.AttackType.ProjectilePattern1:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectilePattern1IE());
        break;
      case EnemyDeathCatBoss.AttackType.TrapPattern0:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.TrapPattern0IE(0.0f));
        break;
      case EnemyDeathCatBoss.AttackType.TrapPattern1:
        this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.TrapPattern1IE(0.0f));
        break;
    }
  }

  public void MeleeAttack()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectilePattern1IE());
  }

  public IEnumerator MeleeAttackIE()
  {
    this.Spine.AnimationState.SetAnimation(0, this.meleeAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(5f);
    this.currentMainAttackRoutine = (Coroutine) null;
  }

  public void EnemyRounds1()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.EnemyRounds1IE());
  }

  public IEnumerator EnemyRounds1IE()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.enemyRounds1.gameObject.SetActive(true);
    bool finished = false;
    this.enemyRounds1.BeginCombat(false, (System.Action) (() => finished = true));
    while (!finished)
      yield return (object) null;
  }

  public void EnemyRounds2() => this.StartCoroutine((IEnumerator) this.EnemyRounds2IE());

  public IEnumerator EnemyRounds2IE()
  {
    this.Spine.AnimationState.SetAnimation(0, this.spawnAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    this.enemyRounds2.gameObject.SetActive(true);
    bool finished = false;
    this.enemyRounds2.BeginCombat(false, (System.Action) (() => finished = true));
    while (!finished)
      yield return (object) null;
  }

  public void EnemyRounds3() => this.StartCoroutine((IEnumerator) this.EnemyRounds2IE());

  public IEnumerator EnemyRounds3IE()
  {
    this.Spine.AnimationState.SetAnimation(0, this.spawnAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    this.enemyRounds3.gameObject.SetActive(true);
    bool finished = false;
    this.enemyRounds3.BeginCombat(false, (System.Action) (() => finished = true));
    while (!finished)
      yield return (object) null;
  }

  public void EnemyRounds4() => this.StartCoroutine((IEnumerator) this.EnemyRounds4IE());

  public IEnumerator EnemyRounds4IE()
  {
    this.Spine.AnimationState.SetAnimation(0, this.spawnAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    this.enemyRounds4.gameObject.SetActive(true);
    bool finished = false;
    this.enemyRounds4.BeginCombat(false, (System.Action) (() => finished = true));
    while (!finished)
      yield return (object) null;
  }

  public void InitializeTraps() => ObjectPool.CreatePool(this.trapPrefab, 40);

  public void TrapPattern0() => this.StartCoroutine((IEnumerator) this.TrapPattern0IE(0.0f));

  public IEnumerator TrapPattern0IE(float delay)
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    yield return (object) new WaitForSeconds(delay);
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.summonAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    int i = -1;
    while (++i < 10)
    {
      for (int index = 0; index < 2; ++index)
      {
        GameObject gameObject = ObjectPool.Spawn(enemyDeathCatBoss.trapPrefab);
        TrapSpikesSpawnOthers component = gameObject.GetComponent<TrapSpikesSpawnOthers>();
        component.Spine.Skeleton.SetSkin("White");
        component.OverrideColor = Color.black;
        float num1 = (float) (i * 2);
        float num2 = Mathf.Lerp(-5.5f, 5.5f, (float) index / 1f);
        Vector3 vector3 = (Vector3.down * num1) with
        {
          x = num2
        };
        vector3.x += UnityEngine.Random.Range(-0.5f, 0.5f);
        gameObject.transform.position = enemyDeathCatBoss.transform.position + vector3;
      }
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void TrapPattern1() => this.StartCoroutine((IEnumerator) this.TrapPattern1IE(0.0f));

  public IEnumerator TrapPattern1IE(float delay)
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    yield return (object) new WaitForSeconds(delay);
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.summonAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    int i = -1;
    while (++i < 13)
    {
      for (int index = 0; index < 2; ++index)
      {
        GameObject gameObject = ObjectPool.Spawn(enemyDeathCatBoss.trapPrefab);
        TrapSpikesSpawnOthers component = gameObject.GetComponent<TrapSpikesSpawnOthers>();
        component.Spine.Skeleton.SetSkin("White");
        component.OverrideColor = Color.black;
        float num = (float) (i * 2);
        float t = (float) index / 1f;
        Vector3 vector3_1 = (Vector3) new Vector2(Mathf.Lerp(-12f, 12f, t), Mathf.Lerp(-10f, -2f, t));
        Vector3 vector3_2 = (index == 0 ? Vector3.right : Vector3.left) * num;
        vector3_2 += vector3_1;
        vector3_2.y += UnityEngine.Random.Range(-0.5f, 0.5f);
        gameObject.transform.position = enemyDeathCatBoss.transform.position + vector3_2;
      }
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void TrapPattern2() => this.StartCoroutine((IEnumerator) this.TrapPattern2IE());

  public IEnumerator TrapPattern2IE()
  {
    this.Spine.AnimationState.SetAnimation(0, this.summonAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    int i = -1;
    while (++i < 10)
    {
      int num1 = -1;
      while (++num1 < 4)
      {
        GameObject gameObject = ObjectPool.Spawn(this.trapPrefab);
        TrapSpikesSpawnOthers component = gameObject.GetComponent<TrapSpikesSpawnOthers>();
        component.Spine.Skeleton.SetSkin("White");
        component.OverrideColor = Color.black;
        float num2 = (float) (90 * num1);
        float num3 = (float) (i * 2);
        float num4 = UnityEngine.Random.Range(-0.5f, 0.5f);
        Vector3 vector3 = (Vector3) (Utils.DegreeToVector2(num2 + 90f) * num4);
        gameObject.transform.position = new Vector3(num3 * Mathf.Cos(num2 * ((float) Math.PI / 180f)), num3 * Mathf.Sin(num2 * ((float) Math.PI / 180f))) + vector3;
      }
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(5f);
    this.currentMainAttackRoutine = (Coroutine) null;
  }

  public void TrapPattern3() => this.StartCoroutine((IEnumerator) this.TrapPattern3IE());

  public IEnumerator TrapPattern3IE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.summonAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    int i = -1;
    while (++i < 13)
    {
      for (int index = 0; index < 8; ++index)
      {
        GameObject gameObject = ObjectPool.Spawn(enemyDeathCatBoss.trapPrefab);
        TrapSpikesSpawnOthers component = gameObject.GetComponent<TrapSpikesSpawnOthers>();
        component.Spine.Skeleton.SetSkin("White");
        component.OverrideColor = Color.black;
        float num = (float) (i * 2);
        float t1 = (float) index / 7f;
        float t2 = index % 2 == 0 ? 0.0f : 1f;
        Vector3 vector3_1 = (Vector3) new Vector2(Mathf.Lerp(-12f, 12f, t2), Mathf.Lerp(-12f, 0.0f, t1));
        Vector3 vector3_2 = ((double) t2 == 0.0 ? Vector3.right : Vector3.left) * num;
        vector3_2 += vector3_1;
        vector3_2.y += UnityEngine.Random.Range(-0.5f, 0.5f);
        gameObject.transform.position = enemyDeathCatBoss.transform.position + vector3_2;
      }
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void TrapPattern4() => this.StartCoroutine((IEnumerator) this.TrapPattern4IE());

  public IEnumerator TrapPattern4IE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.summonAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    int i = -1;
    while (++i < 10)
    {
      for (int index = 0; index < 10; ++index)
      {
        GameObject gameObject = ObjectPool.Spawn(enemyDeathCatBoss.trapPrefab);
        TrapSpikesSpawnOthers component = gameObject.GetComponent<TrapSpikesSpawnOthers>();
        component.Spine.Skeleton.SetSkin("White");
        component.OverrideColor = Color.black;
        float num1 = (float) (i * 2);
        float num2 = Mathf.Lerp(-12f, 12f, (float) index / 9f);
        Vector3 vector3 = (Vector3.down * num1) with
        {
          x = num2
        };
        vector3.x += UnityEngine.Random.Range(-0.5f, 0.5f);
        gameObject.transform.position = enemyDeathCatBoss.transform.position + vector3;
      }
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void TrapPattern5() => this.StartCoroutine((IEnumerator) this.TrapPattern5IE());

  public IEnumerator TrapPattern5IE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.summonAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.TrapPatternTargeted(new Vector3(-12f, 7f)));
    enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.TrapPatternTargeted(new Vector3(12f, 7f)));
    enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.TrapPatternTargeted(new Vector3(-12f, -7f)));
    yield return (object) enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.TrapPatternTargeted(new Vector3(12f, -7f)));
    yield return (object) new WaitForSeconds(5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public IEnumerator TrapPatternTargeted(Vector3 startingPosition)
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    Vector3 Position = Vector3.zero;
    int i = -1;
    float Dist = 1f;
    enemyDeathCatBoss.state.facingAngle = Utils.GetAngle(enemyDeathCatBoss.transform.position, PlayerFarming.Instance.transform.position);
    float facingAngle = enemyDeathCatBoss.state.facingAngle;
    while (++i < 10)
    {
      GameObject gameObject = ObjectPool.Spawn(enemyDeathCatBoss.trapPrefab);
      gameObject.GetComponent<TrapSpikesSpawnOthers>();
      float angle = Utils.GetAngle(startingPosition + Position, PlayerFarming.Instance.transform.position);
      Position += new Vector3(Dist * Mathf.Cos(angle * ((float) Math.PI / 180f)), Dist * Mathf.Sin(angle * ((float) Math.PI / 180f)));
      gameObject.transform.position = startingPosition + Position;
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
  }

  public void ProjectilePattern1()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectilePattern1IE());
  }

  public IEnumerator ProjectilePattern1IE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
    yield return (object) enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.projectilePattern1.ShootIE());
    yield return (object) new WaitForSeconds(5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void ProjectilePattern2()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectilePattern2IE());
  }

  public IEnumerator ProjectilePattern2IE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
    yield return (object) enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.projectilePattern2.ShootIE());
    yield return (object) new WaitForSeconds(6f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void ProjectilePattern3()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectilePattern3IE());
  }

  public IEnumerator ProjectilePattern3IE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
    yield return (object) enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.projectilePattern3.ShootIE());
    yield return (object) new WaitForSeconds(5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void ProjectilePattern4()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectilePattern4IE());
  }

  public IEnumerator ProjectilePattern4IE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
    yield return (object) enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.projectilePattern4.ShootIE());
    yield return (object) new WaitForSeconds(5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void ProjectileSidePattern1()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectileSidePattern1IE());
  }

  public IEnumerator ProjectileSidePattern1IE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handStartAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.handLoopAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
    enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.leftSidePattern1.ShootIE());
    enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.rightSidePattern1.ShootIE());
    yield return (object) new WaitForSeconds(4f);
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handEndAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void InitializeProjectileLines()
  {
    ObjectPool.CreatePool<ProjectileLine>(this.projectilePatternVertical, 4);
    ObjectPool.CreatePool<ProjectileLine>(this.projectilePatternLine, 2);
  }

  public void ShootProjectileLine()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileLineIE(true));
  }

  public IEnumerator ShootProjectileLineIE(bool clearCoroutines)
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
    int num = (double) UnityEngine.Random.value > 0.5 ? 1 : -1;
    Projectile component = ObjectPool.Spawn<ProjectileLine>(enemyDeathCatBoss.projectilePatternLine, enemyDeathCatBoss.transform.parent).GetComponent<Projectile>();
    component.transform.position = num == 1 ? enemyDeathCatBoss.transform.position + Vector3.down * 2f : enemyDeathCatBoss.transform.position + Vector3.down * 11.5f;
    component.Angle = 0.0f;
    component.health = enemyDeathCatBoss.health;
    component.team = Health.Team.Team2;
    component.Speed = enemyDeathCatBoss.projectilePatternLineSpeed * (float) num;
    component.Acceleration = enemyDeathCatBoss.projectilePatternLineAcceleration;
    component.GetComponent<ProjectileLine>().InitDelayed(PlayerFarming.Instance.gameObject, 0.0f, 270f);
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternLineDuration);
    yield return (object) new WaitForSeconds(1f);
    if (clearCoroutines)
      enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void ShootProjectileLinesVeritcal()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileLinesMultiIE());
  }

  public IEnumerator ShootProjectileLinesMultiIE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    for (int t = 0; t < 2; ++t)
    {
      enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
      enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
      yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
      for (int index = 0; index < 2; ++index)
      {
        Projectile component = ObjectPool.Spawn<ProjectileLine>(enemyDeathCatBoss.projectilePatternVertical, enemyDeathCatBoss.transform.parent).GetComponent<Projectile>();
        component.transform.position = new Vector3(index == 0 ? -10f : 10f, 0.0f, 0.0f);
        component.Angle = 0.0f;
        component.health = enemyDeathCatBoss.health;
        component.team = Health.Team.Team2;
        component.Speed = enemyDeathCatBoss.projectilePatternLineVerticalSpeed;
        component.Acceleration = 0.0f;
        component.GetComponent<ProjectileLine>().InitDelayed(PlayerFarming.Instance.gameObject, 0.0f, index == 0 ? 0.0f : 180f);
      }
      enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.ShootProjectileLineIE(false));
      yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternLineVerticalAnticipation);
    }
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternLineDuration);
    yield return (object) new WaitForSeconds(2.5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void InitializeProjectilePatternRings()
  {
    int initialPoolSize = 9;
    if (this.projectilePatternRings is ProjectileCirclePattern)
    {
      ProjectileCirclePattern projectilePatternRings = (ProjectileCirclePattern) this.projectilePatternRings;
      if ((UnityEngine.Object) projectilePatternRings.ProjectilePrefab != (UnityEngine.Object) null)
        ObjectPool.CreatePool<Projectile>(projectilePatternRings.ProjectilePrefab, projectilePatternRings.BaseProjectilesCount * initialPoolSize);
    }
    ObjectPool.CreatePool<ProjectileCircleBase>(this.projectilePatternRings, initialPoolSize);
  }

  public void ShootProjectileRingsMulti()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsMultiIE());
  }

  public IEnumerator ShootProjectileRingsMultiIE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, true);
    yield return (object) new WaitForSeconds(0.5f);
    List<float> floatList1 = new List<float>()
    {
      1f,
      3f,
      2f
    };
    List<float> floatList2 = new List<float>()
    {
      2f,
      3f,
      1f
    };
    for (int index1 = 0; index1 < 3; ++index1)
    {
      for (int index2 = 0; index2 < 2; ++index2)
      {
        Vector3 vector3 = new Vector3(index2 == 0 ? -10f : 10f, 3f * (float) (index1 - 1), 0.0f);
        double num = (double) index1 / 2.0;
        float angleToPlayer = enemyDeathCatBoss.GetAngleToPlayer();
        Vector3 vector2 = (Vector3) Utils.DegreeToVector2(angleToPlayer);
        Projectile component = ObjectPool.Spawn<ProjectileCircleBase>(enemyDeathCatBoss.projectilePatternRings, enemyDeathCatBoss.transform.parent).GetComponent<Projectile>();
        component.transform.position = vector3;
        component.Angle = angleToPlayer;
        component.health = enemyDeathCatBoss.health;
        component.team = Health.Team.Team2;
        component.Speed = enemyDeathCatBoss.projectilePatternRingsSpeed;
        component.Acceleration = enemyDeathCatBoss.projectilePatternRingsAcceleration;
        float shootDelay = index2 == 0 ? floatList1[index1] : floatList2[index1];
        component.GetComponent<ProjectileCircleBase>().InitDelayed(PlayerFarming.Instance.gameObject, enemyDeathCatBoss.projectilePatternRingsRadius, shootDelay, new System.Action(enemyDeathCatBoss.\u003CShootProjectileRingsMultiIE\u003Eb__149_0));
      }
    }
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternRingsDuration);
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.idleAnimation, true);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void EyeDestroyed()
  {
    if (EnemyDeathCatEyesManager.Instance.Eyes.Count == 0)
      this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.HurtIE());
  }

  public IEnumerator HurtIE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.CanAttack = false;
    EnemyDeathCatEyesManager.Instance.HideAllEyes(0.0f);
    EnemyDeathCatEyesManager.Instance.Active = false;
    if (EnemyDeathCatEyesManager.Instance.Eyes.Count > 0)
    {
      enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.hurtAnimation, false);
      enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
      yield return (object) new WaitForSeconds(3f);
      enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.WaitForCurrentAttackToFinish(new System.Action(enemyDeathCatBoss.\u003CHurtIE\u003Eb__151_0)));
    }
    else
    {
      enemyDeathCatBoss.CanAttack = false;
      Projectile.ClearProjectiles();
      enemyDeathCatBoss.projectilePattern1.StopAllCoroutines();
      enemyDeathCatBoss.projectilePattern2.StopAllCoroutines();
      enemyDeathCatBoss.projectilePattern3.StopAllCoroutines();
      enemyDeathCatBoss.projectilePattern4.StopAllCoroutines();
      enemyDeathCatBoss.leftSidePattern1.StopAllCoroutines();
      enemyDeathCatBoss.rightSidePattern1.StopAllCoroutines();
      GameManager.GetInstance().CamFollowTarget.MinZoom = 8f;
      GameManager.GetInstance().CamFollowTarget.MaxZoom = 14f;
      GameManager.GetInstance().RemoveFromCamera(enemyDeathCatBoss.middleBone1);
      GameManager.GetInstance().RemoveFromCamera(enemyDeathCatBoss.middleBone2);
      GameManager.GetInstance().AddToCamera(enemyDeathCatBoss.gameObject);
      enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.downStartAnimation, false);
      enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.downIdleAnimation, true, 0.0f);
      enemyDeathCatBoss.Spine.transform.DOLocalMoveX(0.0f, 2f);
      enemyDeathCatBoss.blockingCollider.isTrigger = false;
      enemyDeathCatBoss.blockingCollider.gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(2f);
      CoopManager.Instance.WakeAllKnockedOutPlayersWithHealth();
      foreach (Behaviour component in enemyDeathCatBoss.GetComponents<Collider2D>())
        component.enabled = true;
      EnemyDeathCatBoss.Instance.health.invincible = false;
    }
  }

  public IEnumerator WaitForCurrentAttackToFinish(System.Action callback)
  {
    while (this.currentMainAttackRoutine != null)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void ScatterRockFall()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ScatterRockFallIE());
  }

  public IEnumerator ScatterRockFallIE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, true);
    yield return (object) new WaitForSeconds(0.5f);
    List<float> floatList1 = new List<float>()
    {
      1f,
      3f,
      2f
    };
    List<float> floatList2 = new List<float>()
    {
      2f,
      3f,
      1f
    };
    int amount = (int) UnityEngine.Random.Range(enemyDeathCatBoss.rockFallAmount.x, enemyDeathCatBoss.rockFallAmount.y);
    int randomTargetPlayerNumber = UnityEngine.Random.Range(0, amount);
    float delay = 0.0f;
    for (int i = 0; i < amount; ++i)
    {
      Vector3 position = (Vector3) (UnityEngine.Random.insideUnitCircle * 7f);
      if (i == randomTargetPlayerNumber)
        position = PlayerFarming.Instance.transform.position;
      UnityEngine.Object.Instantiate<TrapRockFall>(enemyDeathCatBoss.rockFallPrefab, position, Quaternion.identity, enemyDeathCatBoss.transform.parent).GetComponent<TrapRockFall>().Drop(false);
      if (i != amount - 1)
      {
        float seconds = UnityEngine.Random.Range(enemyDeathCatBoss.rockFallDelay.x, enemyDeathCatBoss.rockFallDelay.y);
        delay += seconds;
        yield return (object) new WaitForSeconds(seconds);
      }
    }
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.rockFallDuration);
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.idleAnimation, true);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public void InitializeMortarStrikes()
  {
    List<MortarBomb> mortarBombList = new List<MortarBomb>();
    for (int index = 0; (double) index < (double) this.bombAmount.y; ++index)
    {
      MortarBomb mortarBomb = ObjectPool.Spawn<MortarBomb>(this.bombPrefab, this.transform.parent);
      mortarBomb.destroyOnFinish = false;
      mortarBombList.Add(mortarBomb);
    }
    for (int index = 0; index < mortarBombList.Count; ++index)
      mortarBombList[index].gameObject.Recycle();
  }

  public void TargetedBombs()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.TargetedBombsIE());
  }

  public IEnumerator TargetedBombsIE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, true);
    yield return (object) new WaitForSeconds(0.5f);
    int amount = (int) UnityEngine.Random.Range(enemyDeathCatBoss.bombAmount.x, enemyDeathCatBoss.bombAmount.y);
    int dir = 1;
    for (int i = 0; i < amount; ++i)
    {
      yield return (object) enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.ShootMortarTarget((float) dir));
      dir *= -1;
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(enemyDeathCatBoss.bombDelayBetween.x, enemyDeathCatBoss.bombDelayBetween.y));
    }
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.idleAnimation, true);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  public IEnumerator ShootMortarTarget(float direction)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/boss/frog/mortar_explode");
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Vector3 Position = new Vector3(15f * direction, UnityEngine.Random.Range(-4f, 4f), 2f);
    MortarBomb mortarBomb = ObjectPool.Spawn<MortarBomb>(enemyDeathCatBoss.bombPrefab, enemyDeathCatBoss.transform.parent, (Vector3) AstarPath.active.GetNearest(PlayerFarming.Instance.transform.position).node.position, Quaternion.identity);
    mortarBomb.destroyOnFinish = false;
    mortarBomb.Play(Position, enemyDeathCatBoss.bombMoveDuration, Health.Team.Team2);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyDeathCatBoss.bombMoveDuration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public float GetAngleToPlayer()
  {
    return Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.SetMusicRoomID(5, "deathcat_room_id");
    this.StopAllCoroutines();
    Projectile.ClearProjectiles();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("KILL_BOSS_5"));
    foreach (GameObject spawnedEnemy in this.spawnedEnemies)
    {
      if ((UnityEngine.Object) spawnedEnemy != (UnityEngine.Object) null)
        spawnedEnemy.gameObject.SetActive(false);
    }
    foreach (DeathCatClone currentActiveClone in this.CurrentActiveClones)
    {
      if ((UnityEngine.Object) currentActiveClone != (UnityEngine.Object) null && (UnityEngine.Object) currentActiveClone != (UnityEngine.Object) this && currentActiveClone.IsFake)
        currentActiveClone.health.DealDamage(currentActiveClone.health.totalHP, this.gameObject, this.transform.position);
    }
    this.StopAllCoroutines();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.Die());
    UIBossHUD.Hide();
    this.GetComponent<Collider2D>().enabled = false;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FreeDeathCat);
    DataManager.Instance.DeathCatBeaten = true;
    PersistenceManager.PersistentData.GameCompletionSnapshots.Add(new PersistentData.GameCompletionSnapshot()
    {
      Permadeath = DataManager.Instance.PermadeDeathActive,
      Difficulty = (DifficultyManager.Difficulty) DataManager.Instance.MetaData.Difficulty
    });
    PlayerFarming.Instance.playerWeapon.DoSlowMo(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyDeathCatBoss.Instance = this;
    this.currentMainAttackRoutine = (Coroutine) null;
    this.attackTimestamp = -1f;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.AddToCamera());
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.OnLanguageChanged);
    EnemyDeathCatBoss.Instance = (EnemyDeathCatBoss) null;
  }

  public IEnumerator AddToCamera()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    yield return (object) new WaitForSeconds(0.25f);
    while (!GameManager.GetInstance().CamFollowTarget.Contains(enemyDeathCatBoss.middleBone1))
    {
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Dieing && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.GameOver)
      {
        GameManager.GetInstance().RemoveFromCamera(enemyDeathCatBoss.gameObject);
        GameManager.GetInstance().AddToCamera(enemyDeathCatBoss.middleBone1);
        GameManager.GetInstance().AddToCamera(enemyDeathCatBoss.middleBone2);
        GameManager.GetInstance().CamFollowTarget.MinZoom = 9f;
        GameManager.GetInstance().CamFollowTarget.MaxZoom = 18f;
      }
      yield return (object) null;
    }
  }

  public IEnumerator Die()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    DeathCatController.Instance.DroppingFervour = false;
    enemyDeathCatBoss.ClearPaths();
    enemyDeathCatBoss.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: false);
    GameManager.GetInstance().OnConversationNext(enemyDeathCatBoss.CinematicBone, 12f);
    yield return (object) new WaitForEndOfFrame();
    enemyDeathCatBoss.state.CURRENT_STATE = StateMachine.State.Dieing;
    if (!DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
    {
      enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.dieHeartAnimation, false);
      enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.deadHeartAnimation, true, 0.0f);
    }
    else
    {
      enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.dieNoHeartAnimation, false);
      enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.deadNoHeartAnimation, true, 0.0f);
    }
    yield return (object) new WaitForSeconds(6.6f);
    for (int index = 0; index < 20; ++index)
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemyDeathCatBoss.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f), Vector3.zero, Color.red);
    DeathCatController.Instance.DeathCatKilled();
    enemyDeathCatBoss.gameObject.SetActive(false);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.SimpleSpineFlash.FlashFillRed();
  }

  public void MonsterHeart_OnHeartTaken()
  {
  }

  public Vector3 GetRandomPosition(Vector3 fromPosition, float minDistance)
  {
    float x;
    float y;
    do
    {
      x = UnityEngine.Random.Range(-12f, 12f);
      y = UnityEngine.Random.Range(-6.5f, 6.5f);
    }
    while ((double) Vector3.Distance(fromPosition, new Vector3(x, y, 0.0f)) <= (double) minDistance);
    return new Vector3(x, y, 0.0f);
  }

  public void OnLanguageChanged()
  {
    UIBossHUD.Instance?.UpdateName(ScriptLocalization.NAMES.DeathNPC);
  }

  public void OnDrawGizmosSelected()
  {
    foreach (EnemyDeathCatBoss.CloneWave cloneWave in this.cloneAttack1)
    {
      foreach (EnemyDeathCatBoss.Cloneshot cloneShot in cloneWave.CloneShots)
        Utils.DrawCircleXY(cloneShot.Position, 0.5f, Color.green);
    }
    foreach (EnemyDeathCatBoss.CloneWave cloneWave in this.cloneAttack2)
    {
      foreach (EnemyDeathCatBoss.Cloneshot cloneShot in cloneWave.CloneShots)
        Utils.DrawCircleXY(cloneShot.Position, 0.5f, Color.green);
    }
    foreach (EnemyDeathCatBoss.CloneWave cloneWave in this.cloneAttack3)
    {
      foreach (EnemyDeathCatBoss.Cloneshot cloneShot in cloneWave.CloneShots)
        Utils.DrawCircleXY(cloneShot.Position, 0.5f, Color.green);
    }
    foreach (EnemyDeathCatBoss.CloneWave cloneWave in this.cloneAttack4)
    {
      foreach (EnemyDeathCatBoss.Cloneshot cloneShot in cloneWave.CloneShots)
        Utils.DrawCircleXY(cloneShot.Position, 0.5f, Color.green);
    }
    foreach (EnemyDeathCatBoss.CloneWave cloneWave in this.cloneAttack5)
    {
      foreach (EnemyDeathCatBoss.Cloneshot cloneShot in cloneWave.CloneShots)
        Utils.DrawCircleXY(cloneShot.Position, 0.5f, Color.green);
    }
  }

  [CompilerGenerated]
  public void \u003CAnimationState_Event\u003Eb__100_0() => this.meleeCollider.SetActive(false);

  [CompilerGenerated]
  public void \u003CShootProjectileRingsMultiIE\u003Eb__149_0()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/jellyfish/grenade_mass_launch", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CHurtIE\u003Eb__151_0()
  {
    this.MainAttack((System.Action) (() =>
    {
      this.StartCoroutine((IEnumerator) this.DelayCallback(UnityEngine.Random.Range(1.5f, 2.5f), (System.Action) (() => this.CanAttack = true)));
      EnemyDeathCatEyesManager.Instance.Active = true;
    }));
  }

  [CompilerGenerated]
  public void \u003CHurtIE\u003Eb__151_1()
  {
    this.StartCoroutine((IEnumerator) this.DelayCallback(UnityEngine.Random.Range(1.5f, 2.5f), (System.Action) (() => this.CanAttack = true)));
    EnemyDeathCatEyesManager.Instance.Active = true;
  }

  [CompilerGenerated]
  public void \u003CHurtIE\u003Eb__151_2() => this.CanAttack = true;

  [Serializable]
  public struct Cloneshot
  {
    public DeathCatClone.AttackType AttackType;
    public int CloneIndex;
    public Vector3 Position;
    public bool Moving;
  }

  [Serializable]
  public struct CloneWave
  {
    public EnemyDeathCatBoss.Cloneshot[] CloneShots;
    public float StartDelay;
    public float EndDelay;
  }

  public enum AttackType
  {
    None,
    LineVertical,
    RingsMulti,
    Pattern2,
    LinesMulti,
    SidePattern1,
    RockFall,
    TargetedBombs,
    Melee,
    ProjectilePattern1,
    TrapPattern0,
    TrapPattern1,
    TrapPattern2,
  }
}
