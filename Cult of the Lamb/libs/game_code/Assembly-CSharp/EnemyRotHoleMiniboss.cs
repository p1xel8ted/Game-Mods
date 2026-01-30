// Decompiled with JetBrains decompiler
// Type: EnemyRotHoleMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyRotHoleMiniboss : UnitObject
{
  public GameObject TargetObject;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  public bool isDying;
  public bool isReadyToDie;
  public BossRotHole[] BossRotHoles;
  [HideInInspector]
  public BossRotHole currentBossRotHole;
  public Coroutine currentStateRoutine;
  public Vector3 startingScale;
  [SerializeField]
  public float shotgunProjectileSpeed = 0.5f;
  [SerializeField]
  public float shotgunProjectileRadius = 0.4f;
  [SerializeField]
  public Vector2 shotgunProjectilesPerShot = new Vector2(6f, 8f);
  [SerializeField]
  public int shotgunShots = 3;
  [SerializeField]
  public float shotgunTimeToSpawnBullets = 0.433333f;
  [SerializeField]
  public float shotgunTimeBetweenShots = 0.7333333f;
  [SerializeField]
  public float shotgunAttackWindup = 0.7f;
  [SerializeField]
  public GameObject shotgunProjectilePrefab;
  [SerializeField]
  public Transform shotgunProjectileOrigin;
  [SerializeField]
  public SimpleSpineFlash SimpleSpineFlash;
  public float timeInIdle;
  public int nextStateIndex;
  public int oldShuffleIndex = -1;
  public GameObject mortarPrefab;
  public EnemyBomb bombPrefab;
  public const float minBombRange = 3f;
  public const float maxBombRange = 6f;
  public float bombSpeed = 8f;
  public float mortarSpeed = 4f;

  public override void Awake()
  {
    base.Awake();
    this.startingScale = this.transform.localScale;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.UsePathing = false;
    this.InitializeShotgunBullets();
  }

  public void InitializeShotgunBullets()
  {
    ObjectPool.CreatePool(this.shotgunProjectilePrefab, (int) this.shotgunProjectilesPerShot.y * 4);
  }

  public override void OnEnable()
  {
    this.currentBossRotHole = this.BossRotHoles[0];
    this.currentBossRotHole.SetDangerous(false);
    this.StartState(this.IdleState());
    base.OnEnable();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.StopAllCoroutines();
  }

  public void StartState(IEnumerator newState)
  {
    if (this.currentStateRoutine != null)
      this.StopCoroutine(this.currentStateRoutine);
    this.currentStateRoutine = this.StartCoroutine((IEnumerator) newState);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (this.isDying)
      return;
    this.isDying = true;
    this.StartState(this.DyingState());
  }

  public IEnumerator IdleState()
  {
    this.Spine.AnimationState.SetAnimation(0, this.IdleAnimation, true);
    this.timeInIdle = 0.5f;
    Debug.Log((object) $"IDLE STATE {this.timeInIdle.ToString()} {this.nextStateIndex.ToString()}");
    yield return (object) new WaitForSeconds(this.timeInIdle);
    ++this.nextStateIndex;
    if (this.nextStateIndex == 10)
      this.nextStateIndex = 0;
    switch (this.nextStateIndex)
    {
      case 0:
        if ((double) Random.value < 0.5)
        {
          this.StartState(this.ShuffleHoles());
          break;
        }
        this.StartState(this.SmashSurroundings());
        break;
      case 1:
        if ((Object) this.BossRotHoles[0] == (Object) this.currentBossRotHole)
        {
          this.StartState(this.ShotgunProjectilesAttack(3f));
          break;
        }
        this.StartState(this.ShotgunProjectilesAttack());
        break;
      case 2:
        this.StartState(this.SmashSurroundings());
        break;
      case 3:
        if ((double) Random.value < 0.5)
        {
          this.StartState(this.FleshMonsterSpitAtttack());
          break;
        }
        this.StartState(this.MortarBombsFromHoleAttack());
        break;
      case 4:
        if ((double) Random.value < 0.5)
        {
          this.StartState(this.ShuffleHoles());
          break;
        }
        this.StartState(this.SmashSurroundings());
        break;
      case 5:
        if ((Object) this.BossRotHoles[0] == (Object) this.currentBossRotHole)
        {
          this.StartState(this.ShotgunProjectilesAttack(3f));
          break;
        }
        this.StartState(this.ShotgunProjectilesAttack());
        break;
      case 6:
        if ((double) Random.value < 0.5)
        {
          this.StartState(this.FleshMonsterSpitAtttack());
          break;
        }
        this.StartState(this.MortarBombsFromHoleAttack(true));
        break;
      case 7:
        if ((double) Random.value < 0.5)
        {
          this.StartState(this.ShuffleHoles());
          break;
        }
        this.StartState(this.SmashSurroundings());
        break;
      case 8:
        if ((Object) this.BossRotHoles[0] == (Object) this.currentBossRotHole)
        {
          this.StartState(this.ShotgunProjectilesAttack(3f));
          break;
        }
        this.StartState(this.ShotgunProjectilesAttack());
        break;
      case 9:
        if ((double) Random.value < 0.5)
        {
          this.StartState(this.ShuffleHoles());
          break;
        }
        this.StartState(this.SmashSurroundings());
        break;
      case 10:
        if ((double) Random.value < 0.5)
        {
          this.StartState(this.FleshMonsterSpitAtttack());
          break;
        }
        this.StartState(this.MortarBombsFromHoleAttack());
        break;
    }
  }

  public IEnumerator DyingState()
  {
    EnemyRotHoleMiniboss enemyRotHoleMiniboss = this;
    float progress = 0.0f;
    float signpostDuration = 1f;
    bool hasExploded = false;
    while ((double) progress < (double) signpostDuration)
    {
      progress += Time.deltaTime;
      yield return (object) null;
    }
    if (!hasExploded)
    {
      hasExploded = true;
      enemyRotHoleMiniboss.health.DealDamage(float.PositiveInfinity, (GameObject) null, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
    }
  }

  public IEnumerator SmashSurroundings(int forceIndex = -1)
  {
    EnemyRotHoleMiniboss enemyRotHoleMiniboss = this;
    if (enemyRotHoleMiniboss.BossRotHoles.Length == 0)
    {
      enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
    }
    else
    {
      int index = forceIndex;
      if (index == -1)
        index = Random.Range(0, enemyRotHoleMiniboss.BossRotHoles.Length);
      BossRotHole newHole = enemyRotHoleMiniboss.BossRotHoles[index];
      if ((Object) newHole == (Object) enemyRotHoleMiniboss.currentBossRotHole)
      {
        enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
      }
      else
      {
        float num = 1f;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append((Tween) enemyRotHoleMiniboss.transform.DOScale(Vector3.zero, num / 2f)).AppendCallback((TweenCallback) (() => this.transform.position = newHole.transform.position)).Append((Tween) enemyRotHoleMiniboss.transform.DOScale(enemyRotHoleMiniboss.startingScale, num / 2f));
        yield return (object) sequence.WaitForCompletion();
        enemyRotHoleMiniboss.currentBossRotHole.SetDangerous(true);
        enemyRotHoleMiniboss.currentBossRotHole = newHole;
        enemyRotHoleMiniboss.currentBossRotHole.SetDangerous(false);
        enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
      }
    }
  }

  public IEnumerator TeleportToNewHoleState(int forceIndex = -1)
  {
    EnemyRotHoleMiniboss enemyRotHoleMiniboss = this;
    if (enemyRotHoleMiniboss.BossRotHoles.Length == 0)
    {
      enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
    }
    else
    {
      int index = forceIndex;
      if (index == -1)
        index = Random.Range(0, enemyRotHoleMiniboss.BossRotHoles.Length);
      BossRotHole newHole = enemyRotHoleMiniboss.BossRotHoles[index];
      if ((Object) newHole == (Object) enemyRotHoleMiniboss.currentBossRotHole)
      {
        enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
      }
      else
      {
        float num = 1f;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append((Tween) enemyRotHoleMiniboss.transform.DOScale(Vector3.zero, num / 2f)).AppendCallback((TweenCallback) (() => this.transform.position = newHole.transform.position)).Append((Tween) enemyRotHoleMiniboss.transform.DOScale(enemyRotHoleMiniboss.startingScale, num / 2f));
        yield return (object) sequence.WaitForCompletion();
        enemyRotHoleMiniboss.currentBossRotHole.SetDangerous(true);
        enemyRotHoleMiniboss.currentBossRotHole = newHole;
        enemyRotHoleMiniboss.currentBossRotHole.SetDangerous(false);
        enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
      }
    }
  }

  public IEnumerator ShuffleHoles()
  {
    EnemyRotHoleMiniboss enemyRotHoleMiniboss = this;
    int shuffleIndex = Random.Range(0, enemyRotHoleMiniboss.BossRotHoles[0].shufflePositions.Count);
    if (enemyRotHoleMiniboss.oldShuffleIndex != shuffleIndex)
    {
      enemyRotHoleMiniboss.oldShuffleIndex = shuffleIndex;
      BossRotHole[] bossRotHoleArray = enemyRotHoleMiniboss.BossRotHoles;
      for (int index = 0; index < bossRotHoleArray.Length; ++index)
      {
        BossRotHole bossRotHole = bossRotHoleArray[index];
        if (bossRotHole.shufflePositions.Count - 1 >= shuffleIndex)
        {
          Vector3 shufflePosition = bossRotHole.shufflePositions[shuffleIndex];
          bossRotHole.transform.DOMove(bossRotHole.shufflePositions[shuffleIndex], 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad);
          if ((Object) bossRotHole == (Object) enemyRotHoleMiniboss.currentBossRotHole)
            enemyRotHoleMiniboss.transform.DOMove(bossRotHole.shufflePositions[shuffleIndex], 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad);
          yield return (object) new WaitForSeconds(0.25f);
        }
      }
      bossRotHoleArray = (BossRotHole[]) null;
      yield return (object) new WaitForSeconds(0.5f);
    }
    enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
  }

  public void DoThrowBomb<T>(T projectile, Vector3 startVector, Vector3 targetVector) where T : EnemyBomb
  {
    T component = Object.Instantiate<GameObject>(projectile.gameObject, targetVector, Quaternion.identity, this.transform.parent).GetComponent<T>();
    if (!((Object) component != (Object) null))
      return;
    component.gameObject.SetActive(true);
    component.transform.position = startVector + (targetVector - this.transform.position).normalized * 6f;
    float bombSpeed = this.bombSpeed;
    float moveDuration = Vector2.Distance((Vector2) startVector, (Vector2) component.transform.position) / bombSpeed;
    component.Play(startVector, moveDuration, Health.Team.Team2, (SkeletonAnimation) null);
  }

  public void DoThrowMortar(GameObject projectile, Vector3 startVector, Vector3 targetVector)
  {
    MortarBomb component = Object.Instantiate<GameObject>(projectile, targetVector, Quaternion.identity, this.transform.parent).GetComponent<MortarBomb>();
    if (!((Object) component != (Object) null))
      return;
    component.gameObject.SetActive(true);
    component.transform.position = startVector + (targetVector - startVector).normalized * 6f;
    float mortarSpeed = this.mortarSpeed;
    float moveDuration = Vector2.Distance((Vector2) startVector, (Vector2) component.transform.position) / mortarSpeed;
    component.Play(startVector + new Vector3(0.0f, 0.0f, -1.5f), moveDuration, Health.Team.Team2);
  }

  public IEnumerator FleshMonsterSpitAtttack()
  {
    EnemyRotHoleMiniboss enemyRotHoleMiniboss = this;
    yield return (object) new WaitForSeconds(0.5f);
    Debug.Log((object) "ATTACK STATE");
    float attackInterval = 0.125f;
    float projectileCount = 3f;
    while ((double) projectileCount > 0.0)
    {
      Health closestTarget = enemyRotHoleMiniboss.GetClosestTarget();
      if ((Object) closestTarget != (Object) null)
      {
        Vector3 vector3 = closestTarget.transform.position + new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0.0f);
        if ((Object) Physics2D.Raycast((Vector2) vector3, (Vector2) (closestTarget.transform.position - vector3).normalized, Vector3.Distance(vector3, closestTarget.transform.position), LayerMask.GetMask("Island")).collider == (Object) null)
          enemyRotHoleMiniboss.DoThrowBomb<EnemyBomb>(enemyRotHoleMiniboss.bombPrefab, enemyRotHoleMiniboss.transform.position + new Vector3(0.0f, 0.0f, -1.5f), vector3);
      }
      --projectileCount;
      yield return (object) new WaitForSeconds(attackInterval);
    }
    enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
  }

  public IEnumerator MortarBombsFromHoleAttack(bool singleHoleOnly = false)
  {
    EnemyRotHoleMiniboss enemyRotHoleMiniboss = this;
    Debug.Log((object) "Bombs from hole attack");
    BossRotHole selectedHole = enemyRotHoleMiniboss.BossRotHoles[Random.Range(0, enemyRotHoleMiniboss.BossRotHoles.Length)];
    for (int i = 0; i < 5; ++i)
    {
      if (!singleHoleOnly)
        selectedHole = enemyRotHoleMiniboss.BossRotHoles[Random.Range(0, enemyRotHoleMiniboss.BossRotHoles.Length)];
      if ((Object) selectedHole != (Object) enemyRotHoleMiniboss.currentBossRotHole && (Object) selectedHole != (Object) null)
      {
        Health closestTarget = enemyRotHoleMiniboss.GetClosestTarget(new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0.0f) * 2f);
        if ((Object) closestTarget != (Object) null)
          enemyRotHoleMiniboss.DoThrowMortar(enemyRotHoleMiniboss.mortarPrefab, selectedHole.transform.position, closestTarget.transform.position);
        yield return (object) new WaitForSeconds(0.25f);
      }
    }
    enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
  }

  public IEnumerator ShotgunProjectilesAttack(float multiplier = 1f)
  {
    EnemyRotHoleMiniboss enemyRotHoleMiniboss = this;
    int shots = (int) Random.Range(enemyRotHoleMiniboss.shotgunProjectilesPerShot.x, enemyRotHoleMiniboss.shotgunProjectilesPerShot.y);
    float radius = enemyRotHoleMiniboss.shotgunProjectileRadius;
    int amount = (int) ((double) enemyRotHoleMiniboss.shotgunShots * (double) multiplier);
    float SignPostDelay = enemyRotHoleMiniboss.shotgunAttackWindup;
    Health closestTarget = enemyRotHoleMiniboss.GetClosestTarget();
    if ((Object) closestTarget != (Object) null)
      enemyRotHoleMiniboss.TargetObject = closestTarget.gameObject;
    if ((Object) enemyRotHoleMiniboss.TargetObject == (Object) null)
      enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
    if ((bool) (Object) enemyRotHoleMiniboss.SimpleSpineFlash)
      enemyRotHoleMiniboss.SimpleSpineFlash.FlashWhite(enemyRotHoleMiniboss.state.Timer / SignPostDelay);
    float windupTimeElapsed = 0.0f;
    while ((double) (windupTimeElapsed += Time.deltaTime * enemyRotHoleMiniboss.Spine.timeScale) < (double) SignPostDelay)
      yield return (object) null;
    for (int t = 0; t < amount; ++t)
    {
      enemyRotHoleMiniboss.Spine.AnimationState.SetAnimation(0, "roar", false);
      float betweenShotsTime = 0.0f;
      while ((double) (betweenShotsTime += Time.deltaTime * enemyRotHoleMiniboss.Spine.timeScale) < (double) enemyRotHoleMiniboss.shotgunTimeToSpawnBullets)
        yield return (object) null;
      for (int index = 0; index < shots; ++index)
        enemyRotHoleMiniboss.ShootProjectile(radius, enemyRotHoleMiniboss.TargetObject.transform.position, enemyRotHoleMiniboss.shotgunProjectileSpeed, enemyRotHoleMiniboss.shotgunProjectileOrigin.position);
      while ((double) (betweenShotsTime += Time.deltaTime * enemyRotHoleMiniboss.Spine.timeScale) < (double) enemyRotHoleMiniboss.shotgunTimeBetweenShots)
        yield return (object) null;
    }
    enemyRotHoleMiniboss.StartState(enemyRotHoleMiniboss.IdleState());
  }

  public void ShootProjectile(
    float radius,
    Vector3 position,
    float projectileSpeed,
    Vector3 origin)
  {
    Vector3 vector3 = (Vector3) (Random.insideUnitCircle * radius);
    Projectile component = ObjectPool.Spawn(this.shotgunProjectilePrefab, origin).GetComponent<Projectile>();
    component.transform.position = origin + vector3;
    component.Angle = Utils.GetAngle(origin, position + vector3);
    component.team = this.health.team;
    component.Speed = projectileSpeed;
    component.LifeTime = 4f + Random.Range(0.0f, 0.3f);
    component.Owner = this.health;
    component.SetOwner(this.health.gameObject);
  }
}
