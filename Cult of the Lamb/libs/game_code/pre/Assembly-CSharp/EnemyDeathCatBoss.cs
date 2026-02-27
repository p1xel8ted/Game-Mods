// Decompiled with JetBrains decompiler
// Type: EnemyDeathCatBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
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
  private string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string appearAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string disappearAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string handSlamAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string summonAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string spawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string dieNoHeartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string dieHeartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string deadNoHeartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string deadHeartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string handStartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string handLoopAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string handEndAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string meleeAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string hurtAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string downStartAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string downIdleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string downAttackAnticipateAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string downAttackAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string downEndAnimation;
  [SerializeField]
  private GameObject trapPrefab;
  [SerializeField]
  private float projectilePatternAnticipation;
  [SerializeField]
  private ProjectilePatternBase projectilePattern1;
  [SerializeField]
  private ProjectilePatternBase projectilePattern2;
  [SerializeField]
  private ProjectilePatternBase projectilePattern3;
  [SerializeField]
  private ProjectilePatternBase projectilePattern4;
  [SerializeField]
  private ProjectilePatternBase leftSidePattern1;
  [SerializeField]
  private ProjectilePatternBase rightSidePattern1;
  [SerializeField]
  private Vector2 poisonAmount;
  [SerializeField]
  private float poisonDelayBetween;
  [SerializeField]
  private PoisonBomb poisonBombPrefab;
  [Space]
  [SerializeField]
  private float projectilePatternLineDuration;
  [SerializeField]
  private float projectilePatternLineSpeed;
  [SerializeField]
  private float projectilePatternLineAcceleration;
  [SerializeField]
  private ProjectileLine projectilePatternLine;
  [SerializeField]
  private float projectilePatternLineVerticalAnticipation;
  [SerializeField]
  private float projectilePatternLineVerticalSpeed;
  [SerializeField]
  private ProjectileLine projectilePatternVertical;
  [Space]
  [SerializeField]
  private float projectileRingBigDuration;
  [SerializeField]
  private float projectileRingBigSpeed;
  [SerializeField]
  private float projectileRingBigAcceleration;
  [SerializeField]
  private float projectileRingBigLifetime;
  [SerializeField]
  private ProjectileCircle projectileRingBig;
  [SerializeField]
  private float projectilePatternRingsDuration;
  [SerializeField]
  private float projectilePatternRingsSpeed;
  [SerializeField]
  private float projectilePatternRingsAcceleration;
  [SerializeField]
  private float projectilePatternRingsRadius;
  [SerializeField]
  private ProjectileCircle projectilePatternRings;
  [SerializeField]
  private ColliderEvents meleeCollider;
  [SerializeField]
  private TrapRockFall rockFallPrefab;
  [SerializeField]
  private float rockFallDuration;
  [SerializeField]
  private Vector2 rockFallAmount;
  [SerializeField]
  private Vector2 rockFallDelay;
  [SerializeField]
  private MortarBomb bombPrefab;
  [SerializeField]
  private float bombMoveDuration;
  [SerializeField]
  private Vector2 bombAmount;
  [SerializeField]
  private Vector2 bombDelayBetween;
  [SerializeField]
  private DeathCatClone clonePrefab;
  [SerializeField]
  private EnemyDeathCatBoss.CloneWave[] cloneAttack1;
  [SerializeField]
  private EnemyDeathCatBoss.CloneWave[] cloneAttack2;
  [SerializeField]
  private EnemyDeathCatBoss.CloneWave[] cloneAttack3;
  [SerializeField]
  private EnemyDeathCatBoss.CloneWave[] cloneAttack4;
  [SerializeField]
  private EnemyDeathCatBoss.CloneWave[] cloneAttack5;
  [SerializeField]
  private EnemyRoundsBase enemyRounds1;
  [SerializeField]
  private EnemyRoundsBase enemyRounds2;
  [SerializeField]
  private EnemyRoundsBase enemyRounds3;
  [SerializeField]
  private EnemyRoundsBase enemyRounds4;
  [SerializeField]
  private GameObject cameraBone;
  [SerializeField]
  private GameObject cinematicBone;
  [SerializeField]
  private GameObject middleBone1;
  [SerializeField]
  private GameObject middleBone2;
  [SerializeField]
  private GameObject enemyDeathCatEyesManager;
  [SerializeField]
  private Collider2D blockingCollider;
  private List<DeathCatClone> currentDeadClones = new List<DeathCatClone>();
  private List<GameObject> spawnedEnemies = new List<GameObject>();
  private Coroutine currentMainAttackRoutine;
  private float attackTimestamp;
  private EnemyDeathCatBoss.AttackType previousAttackType;

  public GameObject CameraBone => this.cameraBone;

  public GameObject CinematicBone => this.cinematicBone;

  public List<DeathCatClone> CurrentActiveClones { get; private set; } = new List<DeathCatClone>();

  public bool CanAttack { get; set; }

  private void Start()
  {
    this.enemyRounds1.SpawnDelay = 0.5f;
    this.enemyRounds2.SpawnDelay = 0.5f;
    this.enemyRounds3.SpawnDelay = 0.5f;
    this.enemyRounds4.SpawnDelay = 0.5f;
    this.attackTimestamp = GameManager.GetInstance().CurrentTime + 10f;
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    this.meleeCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.MeleeColliderHit);
    this.meleeCollider.gameObject.SetActive(false);
    this.GetComponent<Health>().invincible = true;
    this.enemyDeathCatEyesManager.SetActive(true);
    foreach (Behaviour eye in this.enemyDeathCatEyesManager.GetComponent<EnemyDeathCatEyesManager>().Eyes)
      eye.enabled = true;
    foreach (Behaviour component in this.GetComponents<Collider2D>())
      component.enabled = false;
  }

  private void MeleeColliderHit(Collider2D collider)
  {
    if (!(collider.tag == "Player"))
      return;
    PlayerFarming.Instance.health.DealDamage(1f, this.gameObject, PlayerFarming.Instance.transform.position);
  }

  private void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
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

  private IEnumerator DelayCallback(float delay, System.Action callback)
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
      switch (UnityEngine.Random.Range(0, 4))
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

  private IEnumerator WaitForAttackToFinish(System.Action callback)
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
        switch (UnityEngine.Random.Range(0, 4))
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

  private void Attack(EnemyDeathCatBoss.AttackType attackType)
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

  private IEnumerator MeleeAttackIE()
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

  private IEnumerator EnemyRounds1IE()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.enemyRounds1.gameObject.SetActive(true);
    bool finished = false;
    this.enemyRounds1.BeginCombat(false, (System.Action) (() => finished = true));
    while (!finished)
      yield return (object) null;
  }

  public void EnemyRounds2() => this.StartCoroutine((IEnumerator) this.EnemyRounds2IE());

  private IEnumerator EnemyRounds2IE()
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

  private IEnumerator EnemyRounds3IE()
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

  private IEnumerator EnemyRounds4IE()
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

  public void TrapPattern0() => this.StartCoroutine((IEnumerator) this.TrapPattern0IE(0.0f));

  private IEnumerator TrapPattern0IE(float delay)
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
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyDeathCatBoss.trapPrefab);
        gameObject.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton.SetSkin("White");
        gameObject.GetComponent<TrapSpikesSpawnOthers>().OverrideColor = Color.black;
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

  private IEnumerator TrapPattern1IE(float delay)
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
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyDeathCatBoss.trapPrefab);
        gameObject.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton.SetSkin("White");
        gameObject.GetComponent<TrapSpikesSpawnOthers>().OverrideColor = Color.black;
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

  private IEnumerator TrapPattern2IE()
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
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.trapPrefab);
        gameObject.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton.SetSkin("White");
        gameObject.GetComponent<TrapSpikesSpawnOthers>().OverrideColor = Color.black;
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

  private IEnumerator TrapPattern3IE()
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
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyDeathCatBoss.trapPrefab);
        gameObject.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton.SetSkin("White");
        gameObject.GetComponent<TrapSpikesSpawnOthers>().OverrideColor = Color.black;
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

  private IEnumerator TrapPattern4IE()
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
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyDeathCatBoss.trapPrefab);
        gameObject.GetComponent<TrapSpikesSpawnOthers>().Spine.Skeleton.SetSkin("White");
        gameObject.GetComponent<TrapSpikesSpawnOthers>().OverrideColor = Color.black;
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

  private IEnumerator TrapPattern5IE()
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

  private IEnumerator TrapPatternTargeted(Vector3 startingPosition)
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    Vector3 Position = Vector3.zero;
    int i = -1;
    float Dist = 1f;
    enemyDeathCatBoss.state.facingAngle = Utils.GetAngle(enemyDeathCatBoss.transform.position, PlayerFarming.Instance.transform.position);
    float facingAngle = enemyDeathCatBoss.state.facingAngle;
    while (++i < 10)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyDeathCatBoss.trapPrefab);
      gameObject.GetComponent<TrapSpikesSpawnOthers>();
      float angle = Utils.GetAngle(startingPosition + Position, PlayerFarming.Instance.transform.position);
      Position += new Vector3(Dist * Mathf.Cos(angle * ((float) Math.PI / 180f)), Dist * Mathf.Sin(angle * ((float) Math.PI / 180f)));
      gameObject.transform.position = startingPosition + Position;
      CameraManager.shakeCamera(0.4f, (float) UnityEngine.Random.Range(0, 360));
      yield return (object) new WaitForSeconds(0.2f);
    }
  }

  private void CloneAttack1()
  {
    this.StartCoroutine((IEnumerator) this.CloneAttack(this.cloneAttack1));
  }

  private void CloneAttack2()
  {
    this.StartCoroutine((IEnumerator) this.CloneAttack(this.cloneAttack2));
  }

  private void CloneAttack3()
  {
    this.StartCoroutine((IEnumerator) this.CloneAttack(this.cloneAttack3));
  }

  private void CloneAttack4()
  {
    this.StartCoroutine((IEnumerator) this.CloneAttack(this.cloneAttack4));
  }

  private void CloneAttack5()
  {
    this.StartCoroutine((IEnumerator) this.CloneAttack(this.cloneAttack5));
  }

  private IEnumerator CloneAttack(EnemyDeathCatBoss.CloneWave[] waves)
  {
    EnemyDeathCatBoss enemyDeathCatBoss1 = this;
    enemyDeathCatBoss1.CurrentActiveClones.Clear();
    enemyDeathCatBoss1.currentDeadClones.Clear();
    EnemyDeathCatBoss.CloneWave[] cloneWaveArray = waves;
    for (int index = 0; index < cloneWaveArray.Length; ++index)
    {
      EnemyDeathCatBoss.CloneWave wave = cloneWaveArray[index];
      bool flag = false;
      foreach (EnemyDeathCatBoss.Cloneshot cloneShot in wave.CloneShots)
      {
        if (cloneShot.CloneIndex >= enemyDeathCatBoss1.CurrentActiveClones.Count)
        {
          EnemyDeathCatBoss enemyDeathCatBoss = enemyDeathCatBoss1;
          DeathCatClone clone = UnityEngine.Object.Instantiate<DeathCatClone>(enemyDeathCatBoss1.clonePrefab, cloneShot.Position, Quaternion.identity, enemyDeathCatBoss1.transform.parent);
          clone.maxSpeed = cloneShot.Moving ? clone.maxSpeed : 0.0f;
          clone.health.OnDie += (Health.DieAction) ((Attacker, AttackLocation, Victim, AttackType, AttackFlags) => enemyDeathCatBoss.currentDeadClones.Add(clone));
          enemyDeathCatBoss1.CurrentActiveClones.Add(clone);
          flag = true;
        }
      }
      if (flag)
      {
        int num = UnityEngine.Random.Range(0, enemyDeathCatBoss1.CurrentActiveClones.Count);
        for (int index1 = 0; index1 < enemyDeathCatBoss1.CurrentActiveClones.Count; ++index1)
        {
          enemyDeathCatBoss1.CurrentActiveClones[index1].SetReal();
          if (num != index1)
            enemyDeathCatBoss1.CurrentActiveClones[index1].SetFake();
        }
      }
      yield return (object) new WaitForSeconds(wave.StartDelay);
      if (enemyDeathCatBoss1.CurrentActiveClones.Count <= 0)
        yield break;
      for (int index2 = 0; index2 < wave.CloneShots.Length; ++index2)
      {
        int cloneIndex = wave.CloneShots[index2].CloneIndex;
        if ((UnityEngine.Object) enemyDeathCatBoss1.CurrentActiveClones[cloneIndex] != (UnityEngine.Object) null)
          enemyDeathCatBoss1.CurrentActiveClones[cloneIndex].Attack(wave.CloneShots[index2].AttackType);
      }
      yield return (object) new WaitForSeconds(wave.EndDelay);
      wave = new EnemyDeathCatBoss.CloneWave();
    }
    cloneWaveArray = (EnemyDeathCatBoss.CloneWave[]) null;
    foreach (DeathCatClone currentActiveClone in enemyDeathCatBoss1.CurrentActiveClones)
    {
      if ((bool) (UnityEngine.Object) currentActiveClone)
        currentActiveClone.Hide();
    }
    enemyDeathCatBoss1.CurrentActiveClones.Clear();
    enemyDeathCatBoss1.currentDeadClones.Clear();
  }

  public void ProjectilePattern1()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ProjectilePattern1IE());
  }

  private IEnumerator ProjectilePattern1IE()
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

  private IEnumerator ProjectilePattern2IE()
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

  private IEnumerator ProjectilePattern3IE()
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

  private IEnumerator ProjectilePattern4IE()
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

  private IEnumerator ProjectileSidePattern1IE()
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

  private void ShootProjectileLine()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileLineIE(true));
  }

  private IEnumerator ShootProjectileLineIE(bool clearCoroutines)
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
    int num = (double) UnityEngine.Random.value > 0.5 ? 1 : -1;
    Projectile component = UnityEngine.Object.Instantiate<ProjectileLine>(enemyDeathCatBoss.projectilePatternLine, enemyDeathCatBoss.transform.parent).GetComponent<Projectile>();
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

  private void ShootProjectileLinesVeritcal()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileLinesMultiIE());
  }

  private IEnumerator ShootProjectileLinesMultiIE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    for (int t = 0; t < 2; ++t)
    {
      enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
      enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
      yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectilePatternAnticipation);
      for (int index = 0; index < 2; ++index)
      {
        Projectile component = UnityEngine.Object.Instantiate<ProjectileLine>(enemyDeathCatBoss.projectilePatternVertical, enemyDeathCatBoss.transform.parent).GetComponent<Projectile>();
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

  private void ShootProjectileRingsBig()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsBigIE());
  }

  private IEnumerator ShootProjectileRingsBigIE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    Projectile arrow = UnityEngine.Object.Instantiate<ProjectileCircle>(enemyDeathCatBoss.projectileRingBig, enemyDeathCatBoss.transform.parent).GetComponent<Projectile>();
    arrow.transform.position = PlayerFarming.Instance.transform.position;
    arrow.Angle = 0.0f;
    arrow.health = enemyDeathCatBoss.health;
    arrow.team = Health.Team.Team2;
    arrow.Speed = 0.0f;
    arrow.Acceleration = 0.0f;
    arrow.GetComponent<ProjectileCircle>().InitDelayed(PlayerFarming.Instance.gameObject, 3f, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    arrow.GetComponent<ProjectileCircle>().TargetMiddle(enemyDeathCatBoss.projectileRingBigSpeed, enemyDeathCatBoss.projectileRingBigLifetime, enemyDeathCatBoss.projectileRingBigAcceleration);
    yield return (object) new WaitForSeconds((float) ((double) enemyDeathCatBoss.projectileRingBigDuration - 1.0 + 5.0));
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  private void ShootProjectileRingsBigRandom()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsBigRandomIE());
  }

  private IEnumerator ShootProjectileRingsBigRandomIE()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    enemyDeathCatBoss.Spine.AnimationState.SetAnimation(0, enemyDeathCatBoss.handSlamAnimation, false);
    enemyDeathCatBoss.Spine.AnimationState.AddAnimation(0, enemyDeathCatBoss.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    Vector3 pos = Vector3.zero;
    for (int i = 0; i < UnityEngine.Random.Range(6, 10); ++i)
    {
      pos = enemyDeathCatBoss.GetRandomPosition(pos, 3f);
      Projectile arrow = UnityEngine.Object.Instantiate<ProjectileCircle>(enemyDeathCatBoss.projectileRingBig, enemyDeathCatBoss.transform.parent).GetComponent<Projectile>();
      arrow.transform.position = pos;
      arrow.Angle = 0.0f;
      arrow.health = enemyDeathCatBoss.health;
      arrow.team = Health.Team.Team2;
      arrow.Speed = 0.0f;
      arrow.Acceleration = 0.0f;
      arrow.GetComponent<ProjectileCircle>().InitDelayed(PlayerFarming.Instance.gameObject, 3f, 0.0f);
      float d = UnityEngine.Random.Range(0.25f, 0.5f);
      yield return (object) new WaitForSeconds(d);
      arrow.GetComponent<ProjectileCircle>().TargetMiddleInverse(enemyDeathCatBoss.projectileRingBigSpeed, 1000f, enemyDeathCatBoss.projectileRingBigAcceleration);
      yield return (object) new WaitForSeconds(2f - d);
      arrow = (Projectile) null;
    }
    yield return (object) new WaitForSeconds(enemyDeathCatBoss.projectileRingBigDuration - 5f);
    enemyDeathCatBoss.currentMainAttackRoutine = (Coroutine) null;
  }

  private void ShootProjectileRingsMulti()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ShootProjectileRingsMultiIE());
  }

  private IEnumerator ShootProjectileRingsMultiIE()
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
        Projectile component = UnityEngine.Object.Instantiate<ProjectileCircle>(enemyDeathCatBoss.projectilePatternRings, enemyDeathCatBoss.transform.parent).GetComponent<Projectile>();
        component.transform.position = vector3;
        component.Angle = angleToPlayer;
        component.health = enemyDeathCatBoss.health;
        component.team = Health.Team.Team2;
        component.Speed = enemyDeathCatBoss.projectilePatternRingsSpeed;
        component.Acceleration = enemyDeathCatBoss.projectilePatternRingsAcceleration;
        float shootDelay = index2 == 0 ? floatList1[index1] : floatList2[index1];
        // ISSUE: reference to a compiler-generated method
        component.GetComponent<ProjectileCircle>().InitDelayed(PlayerFarming.Instance.gameObject, enemyDeathCatBoss.projectilePatternRingsRadius, shootDelay, new System.Action(enemyDeathCatBoss.\u003CShootProjectileRingsMultiIE\u003Eb__155_0));
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

  private IEnumerator HurtIE()
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
      // ISSUE: reference to a compiler-generated method
      enemyDeathCatBoss.StartCoroutine((IEnumerator) enemyDeathCatBoss.WaitForCurrentAttackToFinish(new System.Action(enemyDeathCatBoss.\u003CHurtIE\u003Eb__157_0)));
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
      foreach (Behaviour component in enemyDeathCatBoss.GetComponents<Collider2D>())
        component.enabled = true;
      EnemyDeathCatBoss.Instance.health.invincible = false;
    }
  }

  private IEnumerator WaitForCurrentAttackToFinish(System.Action callback)
  {
    while (this.currentMainAttackRoutine != null)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void ScatterPoison() => this.StartCoroutine((IEnumerator) this.ScatterPoisonIE());

  private IEnumerator ScatterPoisonIE()
  {
    int amount = Mathf.RoundToInt(UnityEngine.Random.Range(this.poisonAmount.x, this.poisonAmount.y + 1f));
    for (int i = 0; i < amount; ++i)
      yield return (object) new WaitForSeconds(this.poisonDelayBetween);
  }

  private void ScatterRockFall()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.ScatterRockFallIE());
  }

  private IEnumerator ScatterRockFallIE()
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

  private void TargetedBombs()
  {
    this.currentMainAttackRoutine = this.StartCoroutine((IEnumerator) this.TargetedBombsIE());
  }

  private IEnumerator TargetedBombsIE()
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

  private IEnumerator ShootMortarTarget(float direction)
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
    UnityEngine.Object.Instantiate<MortarBomb>(enemyDeathCatBoss.bombPrefab, (Vector3) AstarPath.active.GetNearest(PlayerFarming.Instance.transform.position).node.position, Quaternion.identity, enemyDeathCatBoss.transform.parent).Play(Position, enemyDeathCatBoss.bombMoveDuration, Health.Team.Team2);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(enemyDeathCatBoss.bombMoveDuration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private float GetAngleToPlayer()
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
    PlayerFarming.Instance.playerWeapon.DoSlowMo(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyDeathCatBoss.Instance = this;
    this.currentMainAttackRoutine = (Coroutine) null;
    this.attackTimestamp = -1f;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.AddToCamera());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyDeathCatBoss.Instance = (EnemyDeathCatBoss) null;
  }

  private IEnumerator AddToCamera()
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

  private IEnumerator Die()
  {
    EnemyDeathCatBoss enemyDeathCatBoss = this;
    DeathCatController.Instance.DroppingFervour = false;
    enemyDeathCatBoss.ClearPaths();
    enemyDeathCatBoss.speed = 0.0f;
    GameManager.GetInstance().OnConversationNew();
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

  private void MonsterHeart_OnHeartTaken()
  {
  }

  private Vector3 GetRandomPosition(Vector3 fromPosition, float minDistance)
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

  private void OnDrawGizmosSelected()
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

  [Serializable]
  private struct Cloneshot
  {
    public DeathCatClone.AttackType AttackType;
    public int CloneIndex;
    public Vector3 Position;
    public bool Moving;
  }

  [Serializable]
  private struct CloneWave
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
