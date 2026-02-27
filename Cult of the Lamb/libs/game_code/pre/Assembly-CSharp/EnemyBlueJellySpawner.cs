// Decompiled with JetBrains decompiler
// Type: EnemyBlueJellySpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyBlueJellySpawner : EnemyJellyCharger
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string spawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string teleportAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string attackAnimation;
  [SerializeField]
  private bool teleportOnHit;
  [SerializeField]
  private float circleCastRadius = 0.5f;
  [SerializeField]
  private float teleportMoveDelay = 0.5f;
  [SerializeField]
  private float teleportCooldownDelay = 0.5f;
  [SerializeField]
  private float randomTeleportMinDelay;
  [SerializeField]
  private float randomTeleportMaxDelay;
  [SerializeField]
  private AssetReferenceGameObject[] spawnableEnemies;
  [SerializeField]
  private float randomSpawnMinDelay;
  [SerializeField]
  private float randomSpawnMaxDelay;
  [SerializeField]
  private float spawnSpitOutForce;
  [SerializeField]
  private float spawnDuration;
  [SerializeField]
  private float spawnCooldown;
  [SerializeField]
  private int spawnAmountMin;
  [SerializeField]
  private int spawnAmountMax;
  [SerializeField]
  private float growSpeed;
  [SerializeField]
  private Ease growEase;
  [SerializeField]
  private int maxEnemiesActive;
  [Tooltip("will not spawn when target is within min radius")]
  [SerializeField]
  private float targetMinSpawnRadius;
  [SerializeField]
  private bool randomSpawnDirection = true;
  [SerializeField]
  private bool killSpawnablesOnDeath;
  [SerializeField]
  private bool canAttack;
  [SerializeField]
  private bool attackOnHit = true;
  [SerializeField]
  private float attackDistance;
  [SerializeField]
  private float attackChargeDur;
  [SerializeField]
  private float attackCooldown;
  [SerializeField]
  private float damageDuration = 0.2f;
  private float spawnTime = float.MaxValue;
  private float teleportTime = float.MaxValue;
  private bool spawning;
  private int spawnedAmount;
  private bool teleporting;
  private float attackTimer;
  private bool charging;
  private bool attacking;
  private Coroutine spawnRoutine;
  private ShowHPBar hpBar;
  private List<UnitObject> spawnedEnemies = new List<UnitObject>();
  private float TeleportOnHitDelay = 1f;
  private Coroutine cDelayTeleport;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  protected override void Start()
  {
    base.Start();
    this.SetSpawnTime();
    this.SetTeleportTime();
    this.hpBar = this.GetComponent<ShowHPBar>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.spawning = false;
    this.teleporting = false;
    this.charging = false;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.simpleSpineFlash.FlashWhite(false);
    this.simpleSpineFlash.FlashFillRed();
    this.DoKnockBack(Attacker, 0.25f, 0.5f);
    if ((double) this.TeleportOnHitDelay >= 0.0 || this.cDelayTeleport != null)
      return;
    this.TeleportOnHitDelay = 2f;
    this.cDelayTeleport = this.StartCoroutine((IEnumerator) this.DelayTeleport());
  }

  private IEnumerator DelayTeleport()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBlueJellySpawner blueJellySpawner = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      blueJellySpawner.StartCoroutine((IEnumerator) blueJellySpawner.TeleportIE());
      blueJellySpawner.cDelayTeleport = (Coroutine) null;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void Update()
  {
    this.TeleportOnHitDelay -= Time.deltaTime;
    if (!this.teleporting)
      base.Update();
    if (this.inRange && (double) this.spawnTime == 3.4028234663852886E+38)
      this.SetSpawnTime();
    if (this.inRange && (double) this.teleportTime == 3.4028234663852886E+38)
      this.SetTeleportTime();
    if (this.inRange && GameManager.RoomActive && (bool) (UnityEngine.Object) this.targetObject && (bool) (UnityEngine.Object) this.gm && (double) this.gm.CurrentTime > (double) this.spawnTime)
      this.SpawnEnemy();
    if ((bool) (UnityEngine.Object) this.targetObject)
      this.inRange = (double) Vector3.Distance(this.targetObject.transform.position, this.transform.position) < (double) this.VisionRange;
    if (!this.canAttack)
      return;
    if (this.charging)
    {
      this.attackTimer += Time.deltaTime;
      float amt = this.attackTimer / this.attackChargeDur;
      this.simpleSpineFlash.FlashWhite(amt);
      if ((double) amt > 1.0)
        this.Attack();
    }
    if (!(bool) (UnityEngine.Object) this.targetObject || this.charging || this.attacking || (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) >= (double) this.attackDistance)
      return;
    this.ChargeAttack();
  }

  protected override void UpdateMoving()
  {
    if (this.attacking || this.spawning || this.teleporting)
      return;
    base.UpdateMoving();
  }

  private void ChargeAttack()
  {
    if (this.attacking || this.spawning)
      return;
    this.charging = true;
    this.attackTimer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Charging;
    this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", this.transform.position);
  }

  private void SpawnEnemy()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting || this.spawnableEnemies.Length == 0 || this.spawnedAmount >= this.maxEnemiesActive || (double) this.targetMinSpawnRadius != 0.0 && (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) <= (double) this.targetMinSpawnRadius)
      return;
    this.spawnRoutine = this.StartCoroutine((IEnumerator) this.SpawnDelay());
  }

  private IEnumerator SpawnDelay()
  {
    EnemyBlueJellySpawner blueJellySpawner1 = this;
    blueJellySpawner1.ClearPaths();
    blueJellySpawner1.spawning = true;
    blueJellySpawner1.Spine.AnimationState.SetAnimation(0, blueJellySpawner1.anticipationAnimation, true);
    yield return (object) new WaitForSeconds(blueJellySpawner1.spawnDuration);
    int num1 = UnityEngine.Random.Range(blueJellySpawner1.spawnAmountMin, blueJellySpawner1.spawnAmountMax + 1);
    float num2 = (float) UnityEngine.Random.Range(0, 360);
    if ((UnityEngine.Object) blueJellySpawner1.targetObject != (UnityEngine.Object) null)
      num2 = Utils.GetAngle(blueJellySpawner1.transform.position, blueJellySpawner1.targetObject.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser_boss/chaser_boss_egg_spawn", blueJellySpawner1.transform.position);
    for (int index = 0; index < num1; ++index)
    {
      EnemyBlueJellySpawner blueJellySpawner = blueJellySpawner1;
      Vector3 direction;
      if (blueJellySpawner1.randomSpawnDirection)
      {
        direction = (Vector3) UnityEngine.Random.insideUnitCircle;
      }
      else
      {
        direction = new Vector3(Mathf.Cos(num2 * ((float) Math.PI / 180f)), Mathf.Sin(num2 * ((float) Math.PI / 180f)), 0.0f);
        num2 += (float) (360 / num1);
      }
      Addressables.InstantiateAsync((object) blueJellySpawner1.spawnableEnemies[UnityEngine.Random.Range(0, blueJellySpawner1.spawnableEnemies.Length)], blueJellySpawner1.transform.position, Quaternion.identity, blueJellySpawner1.transform.parent).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        EnemyExploder component1 = obj.Result.GetComponent<EnemyExploder>();
        component1.givePath(component1.transform.position + direction * 5f);
        component1.health.OnDie += new Health.DieAction(blueJellySpawner.OnEnemyKilled);
        EnemyRoundsBase.Instance?.AddEnemyToRound(component1.GetComponent<Health>());
        Interaction_Chest.Instance?.AddEnemy(component1.health);
        if ((double) blueJellySpawner.growSpeed != 0.0)
        {
          component1.Spine.transform.localScale = Vector3.zero;
          component1.Spine.transform.DOScale(1f, blueJellySpawner.growSpeed).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(blueJellySpawner.growEase);
        }
        if ((UnityEngine.Object) blueJellySpawner.transform != (UnityEngine.Object) null)
        {
          float angle = Utils.GetAngle(blueJellySpawner.transform.position, blueJellySpawner.transform.position + direction) * ((float) Math.PI / 180f);
          component1.DoKnockBack(angle, blueJellySpawner.spawnSpitOutForce, 0.75f);
        }
        ++blueJellySpawner.spawnedAmount;
        DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
        if ((bool) (UnityEngine.Object) component2)
          component2.GiveXP = false;
        blueJellySpawner.spawnedEnemies.Add((UnitObject) component1);
      });
    }
    blueJellySpawner1.Spine.AnimationState.SetAnimation(0, blueJellySpawner1.spawnAnimation, false);
    blueJellySpawner1.Spine.AnimationState.AddAnimation(0, blueJellySpawner1.idleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(blueJellySpawner1.spawnCooldown);
    blueJellySpawner1.state.CURRENT_STATE = StateMachine.State.Idle;
    blueJellySpawner1.Spine.AnimationState.SetAnimation(0, blueJellySpawner1.idleAnimation, true);
    blueJellySpawner1.SetSpawnTime();
    blueJellySpawner1.spawning = false;
    blueJellySpawner1.spawnRoutine = (Coroutine) null;
  }

  private void OnEnemyKilled(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.spawnedAmount;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!this.killSpawnablesOnDeath)
      return;
    foreach (UnitObject spawnedEnemy in this.spawnedEnemies)
    {
      if ((UnityEngine.Object) spawnedEnemy != (UnityEngine.Object) null)
      {
        spawnedEnemy.enabled = true;
        spawnedEnemy.health.DealDamage(spawnedEnemy.health.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Heavy, dealDamageImmediately: true);
      }
    }
  }

  private void SetSpawnTime()
  {
    if (!(bool) (UnityEngine.Object) this.gm)
      return;
    this.spawnTime = this.gm.CurrentTime + UnityEngine.Random.Range(this.randomSpawnMinDelay, this.randomSpawnMaxDelay);
  }

  private void SetTeleportTime()
  {
    if (!(bool) (UnityEngine.Object) this.gm)
      return;
    this.teleportTime = this.gm.CurrentTime + UnityEngine.Random.Range(this.randomTeleportMinDelay, this.randomTeleportMaxDelay);
  }

  private IEnumerator TeleportIE()
  {
    EnemyBlueJellySpawner blueJellySpawner = this;
    if (!blueJellySpawner.teleporting)
    {
      if (blueJellySpawner.spawning && blueJellySpawner.spawnRoutine != null)
      {
        blueJellySpawner.StopCoroutine(blueJellySpawner.spawnRoutine);
        blueJellySpawner.spawning = false;
      }
      blueJellySpawner.teleporting = true;
      blueJellySpawner.ClearPaths();
      blueJellySpawner.Spine.AnimationState.SetAnimation(0, blueJellySpawner.teleportAnimation, false);
      yield return (object) new WaitForEndOfFrame();
      if ((bool) (UnityEngine.Object) blueJellySpawner.hpBar)
        blueJellySpawner.hpBar.Hide();
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", blueJellySpawner.transform.position);
      yield return (object) new WaitForSeconds(blueJellySpawner.teleportMoveDelay / blueJellySpawner.Spine.timeScale);
      float num = 100f;
      while ((double) --num > 0.0)
      {
        float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
        float distance = (float) UnityEngine.Random.Range(4, 7);
        Vector3 vector3_1 = (UnityEngine.Object) blueJellySpawner.targetObject != (UnityEngine.Object) null ? blueJellySpawner.targetObject.transform.position : blueJellySpawner.transform.position;
        Vector3 vector3_2 = vector3_1 + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
        RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) vector3_1, blueJellySpawner.circleCastRadius, (Vector2) Vector3.Normalize(vector3_2 - vector3_1), distance, (int) blueJellySpawner.layerToCheck);
        if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        {
          if ((double) Vector3.Distance(vector3_1, (Vector3) raycastHit2D.centroid) > 3.0)
          {
            if (blueJellySpawner.ShowDebug)
            {
              blueJellySpawner.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y));
              blueJellySpawner.PointsLink.Add(new Vector3(blueJellySpawner.transform.position.x, blueJellySpawner.transform.position.y));
            }
            blueJellySpawner.transform.position = (Vector3) raycastHit2D.centroid + Vector3.Normalize(vector3_1 - vector3_2) * blueJellySpawner.circleCastRadius;
            break;
          }
        }
        else
        {
          if (blueJellySpawner.ShowDebug)
          {
            blueJellySpawner.EndPoints.Add(new Vector3(vector3_2.x, vector3_2.y));
            blueJellySpawner.EndPointsLink.Add(new Vector3(blueJellySpawner.transform.position.x, blueJellySpawner.transform.position.y));
          }
          blueJellySpawner.transform.position = vector3_2;
          break;
        }
      }
      if ((UnityEngine.Object) blueJellySpawner.targetObject != (UnityEngine.Object) null)
        blueJellySpawner.state.facingAngle = Utils.GetAngle(blueJellySpawner.transform.position, blueJellySpawner.targetObject.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", blueJellySpawner.transform.position);
      yield return (object) new WaitForSeconds(blueJellySpawner.teleportCooldownDelay / blueJellySpawner.Spine.timeScale);
      blueJellySpawner.Spine.AnimationState.SetAnimation(0, blueJellySpawner.idleAnimation, true);
      blueJellySpawner.teleporting = false;
      blueJellySpawner.SetTeleportTime();
    }
  }

  private void Attack()
  {
    if (this.attacking)
      return;
    this.attackTimer = 0.0f;
    this.attacking = true;
    this.charging = false;
    this.Spine.AnimationState.SetAnimation(0, this.attackAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_attack", this.transform.position);
    this.ClearPaths();
    this.StartCoroutine((IEnumerator) this.TurnOnDamageColliderForDuration(this.damageDuration));
    this.StartCoroutine((IEnumerator) this.AttackCooldownIE());
  }

  private IEnumerator AttackCooldownIE()
  {
    EnemyBlueJellySpawner blueJellySpawner = this;
    yield return (object) new WaitForEndOfFrame();
    blueJellySpawner.simpleSpineFlash.FlashWhite(false);
    yield return (object) new WaitForSeconds(blueJellySpawner.attackCooldown);
    blueJellySpawner.attacking = false;
    blueJellySpawner.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  private IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBlueJellySpawner blueJellySpawner = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      blueJellySpawner.damageColliderEvents.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    blueJellySpawner.damageColliderEvents.SetActive(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnDrawGizmos()
  {
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], this.circleCastRadius, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], this.circleCastRadius, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }
}
