// Decompiled with JetBrains decompiler
// Type: EnemyHopperMortarMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyHopperMortarMiniboss : EnemyHopperMortar
{
  public new IEnumerator damageColliderRoutine;
  public ParticleSystem aoeParticles;
  public const int totalEnemiesToSpawn = 3;
  public int numbEnemiesSpawned;
  public GameObject enemyToSpawn;
  public Vector2 TimeBetweenEnemySpawn;
  public EnemyHopperMortarMiniboss.BurpType[] burpPattern;
  public int burpPatternIndex;
  public int ShotsToFireAroundMiniboss = 6;
  public float shotsAroundMinibossDistance = 4f;
  public int ShotsToFireCross;
  public int ShotsToFireLine = 4;
  public float lastEnemySpawnTime;
  public bool playedJumpSfx;

  public override void Awake()
  {
    base.Awake();
    this.Preload();
  }

  public new void Preload()
  {
    this.enemyToSpawn.CreatePool(18);
    this.InitializeMortarStrikes();
  }

  public new void InitializeMortarStrikes()
  {
    int num = Mathf.Max(this.ShotsToFire, this.ShotsToFireAroundMiniboss, this.ShotsToFireCross, this.ShotsToFireLine);
    List<MortarBomb> mortarBombList = new List<MortarBomb>();
    for (int index = 0; index < num; ++index)
    {
      MortarBomb component = ObjectPool.Spawn(this.projectilePrefab, this.transform.parent).GetComponent<MortarBomb>();
      component.destroyOnFinish = false;
      mortarBombList.Add(component);
    }
    for (int index = 0; index < mortarBombList.Count; ++index)
      mortarBombList[index].gameObject.Recycle();
  }

  public void Start()
  {
    if ((bool) (Object) this.state)
      this.state.OnStateChange += new StateMachine.StateChange(((EnemyHopper) this).OnStateChange);
    this.KnockBackMultipier = 0.0f;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!(bool) (Object) this.state)
      return;
    this.state.OnStateChange -= new StateMachine.StateChange(((EnemyHopper) this).OnStateChange);
  }

  public void OnStateChange()
  {
    if (this.state.CURRENT_STATE != StateMachine.State.Moving)
      return;
    this.maxSpeed = Random.Range(0.1f, 0.3f);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.numbEnemiesSpawned = 0;
    this.minTimeBetweenBurps = 1.8f;
    if (!((Object) GameManager.GetInstance() != (Object) null))
      return;
    this.lastEnemySpawnTime = GameManager.GetInstance().CurrentTime + Random.Range(this.TimeBetweenEnemySpawn.x, this.TimeBetweenEnemySpawn.y);
  }

  public override void BurpFlies()
  {
    if ((bool) (Object) this.enemyToSpawn && this.numbEnemiesSpawned < 3 && (Object) this.health != (Object) null && (double) GameManager.GetInstance().CurrentTime >= (double) this.lastEnemySpawnTime)
      this.SpawnHelperEnemies();
    else
      base.BurpFlies();
  }

  public void SpawnHelperEnemies()
  {
    ++this.numbEnemiesSpawned;
    Vector2 Position = (Vector2) (-this.transform.position.normalized * 3f);
    RaycastHit2D raycastHit2D = Physics2D.Raycast(Vector2.zero, (Vector2) -this.transform.position.normalized, 3f, (int) this.layerToCheck);
    if ((bool) raycastHit2D)
      Position = raycastHit2D.point + (Vector2) this.transform.position.normalized;
    EnemySpawner.Create((Vector3) Position, this.transform.parent, this.enemyToSpawn);
    this.state.CURRENT_STATE = StateMachine.State.Dancing;
    this.dancingTimestamp = GameManager.GetInstance().CurrentTime;
    this.lastEnemySpawnTime = GameManager.GetInstance().CurrentTime + Random.Range(this.TimeBetweenEnemySpawn.x, this.TimeBetweenEnemySpawn.y);
  }

  public override void UpdateStateMoving()
  {
    if (!this.playedJumpSfx)
    {
      AudioManager.Instance.PlayOneShot(this.OnJumpSoundPath, this.gameObject);
      this.playedJumpSfx = true;
    }
    if (PlayerRelic.TimeFrozen)
      this.hoppingTimestamp += Time.deltaTime;
    this.speed = this.hopSpeedCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopMoveSpeed * this.Spine[0].timeScale;
    this.Spine[0].transform.localPosition = -Vector3.forward * this.hopZCurve.Evaluate(this.gm.TimeSince(this.hoppingTimestamp) / this.hoppingDur) * this.hopZHeight;
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) / (double) this.hoppingDur > 0.10000000149011612 && (double) this.gm.TimeSince(this.hoppingTimestamp) / (double) this.hoppingDur < 0.89999997615814209)
      this.health.untouchable = true;
    else
      this.health.untouchable = false;
    this.canBeParried = false;
    this.SimpleSpineFlash.FlashWhite(1f - Mathf.Clamp01(this.gm.TimeSince(this.hoppingTimestamp) / (this.attackingDur * 0.5f)));
    if ((double) this.gm.TimeSince(this.hoppingTimestamp) < (double) this.hoppingDur)
      return;
    this.speed = 0.0f;
    this.DoAttack();
    if (this.ShouldStartCharging())
    {
      this.playedJumpSfx = false;
      this.state.CURRENT_STATE = StateMachine.State.Charging;
    }
    else
    {
      this.playedJumpSfx = false;
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
  }

  public new void DoAttack()
  {
    if (this.damageColliderRoutine != null)
      this.StopCoroutine((IEnumerator) this.damageColliderRoutine);
    AudioManager.Instance.PlayOneShot(this.OnLandSoundPath, this.gameObject);
    AudioManager.Instance.PlayOneShot(this.AttackVO, this.gameObject);
    this.damageColliderRoutine = this.TurnOnDamageColliderForDuration(this.attackingDur);
    this.StartCoroutine((IEnumerator) this.damageColliderRoutine);
    if ((Object) this.aoeParticles != (Object) null)
      this.aoeParticles.Play();
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position + Vector3.back * 0.5f);
    CameraManager.shakeCamera(2f);
  }

  public new IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyHopperMortarMiniboss hopperMortarMiniboss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      hopperMortarMiniboss.damageColliderEvents.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    hopperMortarMiniboss.damageColliderEvents.SetActive(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override IEnumerator ShootProjectileRoutine()
  {
    EnemyHopperMortarMiniboss hopperMortarMiniboss = this;
    if (hopperMortarMiniboss.burpPattern[hopperMortarMiniboss.burpPatternIndex] == EnemyHopperMortarMiniboss.BurpType.RandomAroundTarget && (double) Random.Range(0.0f, 1f) > 0.33000001311302185)
      yield return (object) hopperMortarMiniboss.StartCoroutine((IEnumerator) hopperMortarMiniboss.\u003C\u003En__0());
    else if ((double) Vector3.Distance(hopperMortarMiniboss.transform.position, Vector3.zero) < 2.0 && (double) Random.Range(0.0f, 1f) < 0.6)
      yield return (object) hopperMortarMiniboss.StartCoroutine((IEnumerator) hopperMortarMiniboss.CrossShoot());
    else if (Random.Range(0, 2) == 0)
      yield return (object) hopperMortarMiniboss.StartCoroutine((IEnumerator) hopperMortarMiniboss.CircleShoot());
    else
      yield return (object) hopperMortarMiniboss.StartCoroutine((IEnumerator) hopperMortarMiniboss.LineShoot());
    hopperMortarMiniboss.burpPatternIndex = (hopperMortarMiniboss.burpPatternIndex + 1) % hopperMortarMiniboss.burpPattern.Length;
  }

  public IEnumerator CircleShoot()
  {
    EnemyHopperMortarMiniboss hopperMortarMiniboss = this;
    AudioManager.Instance.PlayOneShot(hopperMortarMiniboss.AttackVO, hopperMortarMiniboss.gameObject);
    Vector3 position = hopperMortarMiniboss.transform.position;
    float aimingAngle = Random.Range(0.0f, 360f);
    for (int i = 0; i < hopperMortarMiniboss.ShotsToFireAroundMiniboss; ++i)
    {
      if ((Object) hopperMortarMiniboss.targetObject == (Object) null && (Object) hopperMortarMiniboss.GetClosestTarget() != (Object) null)
        hopperMortarMiniboss.targetObject = hopperMortarMiniboss.GetClosestTarget().gameObject;
      if ((Object) hopperMortarMiniboss.targetObject == (Object) null)
        break;
      Vector3 b = hopperMortarMiniboss.transform.position + (Vector3) Utils.DegreeToVector2(aimingAngle) * Random.Range(2.5f, hopperMortarMiniboss.shotsAroundMinibossDistance);
      MortarBomb component = ObjectPool.Spawn(hopperMortarMiniboss.projectilePrefab, hopperMortarMiniboss.transform.parent, hopperMortarMiniboss.targetObject.transform.position, Quaternion.identity).GetComponent<MortarBomb>();
      if ((double) Vector2.Distance((Vector2) hopperMortarMiniboss.transform.position, (Vector2) b) < 2.5)
      {
        Vector2 vector2 = (Vector2) (hopperMortarMiniboss.transform.position + (b - hopperMortarMiniboss.transform.position).normalized * 2.5f);
        component.transform.position = (Vector3) vector2;
      }
      else
      {
        Vector2 vector2 = (Vector2) (hopperMortarMiniboss.transform.position + (b - hopperMortarMiniboss.transform.position).normalized * 5f);
        component.transform.position = (Vector3) vector2;
      }
      component.Play(hopperMortarMiniboss.transform.position + new Vector3(0.0f, 0.0f, -1.5f), hopperMortarMiniboss.bombDuration, Health.Team.Team2);
      hopperMortarMiniboss.SimpleSpineFlash.FlashWhite(false);
      aimingAngle += (float) (360 / hopperMortarMiniboss.ShotsToFireAroundMiniboss);
      yield return (object) new WaitForSeconds(0.112500004f);
    }
  }

  public IEnumerator CrossShoot()
  {
    EnemyHopperMortarMiniboss hopperMortarMiniboss = this;
    AudioManager.Instance.PlayOneShot(hopperMortarMiniboss.AttackVO, hopperMortarMiniboss.gameObject);
    Vector3 position1 = hopperMortarMiniboss.transform.position;
    hopperMortarMiniboss.SimpleSpineFlash.FlashWhite(false);
    float distance = 1f;
    for (int i = 0; i < hopperMortarMiniboss.ShotsToFireCross; ++i)
    {
      float degree = 0.0f;
      for (int index = 0; index < 4; ++index)
      {
        Vector3 position2 = hopperMortarMiniboss.transform.position + (Vector3) Utils.DegreeToVector2(degree) * distance;
        ObjectPool.Spawn(hopperMortarMiniboss.projectilePrefab, hopperMortarMiniboss.transform.parent, position2, Quaternion.identity).GetComponent<MortarBomb>().Play(hopperMortarMiniboss.transform.position + new Vector3(0.0f, 0.0f, -1.5f), hopperMortarMiniboss.bombDuration, Health.Team.Team2);
        degree += 90f;
      }
      distance += 2f;
      yield return (object) new WaitForSeconds(0.112500004f);
    }
  }

  public IEnumerator LineShoot()
  {
    EnemyHopperMortarMiniboss hopperMortarMiniboss = this;
    AudioManager.Instance.PlayOneShot(hopperMortarMiniboss.AttackVO, hopperMortarMiniboss.gameObject);
    Vector3 position1 = hopperMortarMiniboss.transform.position;
    hopperMortarMiniboss.SimpleSpineFlash.FlashWhite(false);
    float distance = 1f;
    float aimAngle = (Object) hopperMortarMiniboss.GetClosestTarget() != (Object) null ? Utils.GetAngle(hopperMortarMiniboss.transform.position, hopperMortarMiniboss.GetClosestTarget().transform.position) : Random.Range(0.0f, 360f);
    for (int i = 0; i < hopperMortarMiniboss.ShotsToFireLine; ++i)
    {
      Vector3 position2 = hopperMortarMiniboss.transform.position + (Vector3) Utils.DegreeToVector2(aimAngle) * distance;
      ObjectPool.Spawn(hopperMortarMiniboss.projectilePrefab, hopperMortarMiniboss.transform.parent, position2, Quaternion.identity).GetComponent<MortarBomb>().Play(hopperMortarMiniboss.transform.position + new Vector3(0.0f, 0.0f, -1.5f), hopperMortarMiniboss.bombDuration, Health.Team.Team2);
      distance += 2f;
      yield return (object) new WaitForSeconds(0.112500004f);
    }
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.ShootProjectileRoutine();

  public enum BurpType
  {
    RandomAroundTarget,
    RandomAroundMiniboss,
  }
}
