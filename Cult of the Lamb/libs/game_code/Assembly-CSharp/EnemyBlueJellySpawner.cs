// Decompiled with JetBrains decompiler
// Type: EnemyBlueJellySpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public string spawnAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string teleportAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string attackAnimation;
  [SerializeField]
  public bool teleportOnHit;
  [SerializeField]
  public float circleCastRadius = 0.5f;
  [SerializeField]
  public float teleportMoveDelay = 0.5f;
  [SerializeField]
  public float teleportCooldownDelay = 0.5f;
  [SerializeField]
  public float randomTeleportMinDelay;
  [SerializeField]
  public float randomTeleportMaxDelay;
  [SerializeField]
  public AssetReferenceGameObject[] spawnableEnemies;
  [SerializeField]
  public float randomSpawnMinDelay;
  [SerializeField]
  public float randomSpawnMaxDelay;
  [SerializeField]
  public float spawnSpitOutForce;
  [SerializeField]
  public float spawnDuration;
  [SerializeField]
  public float spawnCooldown;
  [SerializeField]
  public int spawnAmountMin;
  [SerializeField]
  public int spawnAmountMax;
  [SerializeField]
  public float growSpeed;
  [SerializeField]
  public Ease growEase;
  [SerializeField]
  public int maxEnemiesActive;
  [Tooltip("will not spawn when target is within min radius")]
  [SerializeField]
  public float targetMinSpawnRadius;
  [SerializeField]
  public bool randomSpawnDirection = true;
  [SerializeField]
  public bool killSpawnablesOnDeath;
  [SerializeField]
  public bool isSpawnablesIncreasingDamageMultiplier = true;
  [SerializeField]
  public bool canAttack;
  [SerializeField]
  public bool attackOnHit = true;
  [SerializeField]
  public float attackDistance;
  [SerializeField]
  public float attackChargeDur;
  [SerializeField]
  public float attackCooldown;
  [SerializeField]
  public float damageDuration = 0.2f;
  public float spawnTime = float.MaxValue;
  public float teleportTime = float.MaxValue;
  public bool spawning;
  public int spawnedAmount;
  public bool teleporting;
  public float attackTimer;
  public bool charging;
  public bool attacking;
  public Coroutine spawnRoutine;
  public ShowHPBar hpBar;
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
  public float TeleportOnHitDelay = 1f;
  public Coroutine cDelayTeleport;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  public override void Start()
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

  public IEnumerator DelayTeleport()
  {
    EnemyBlueJellySpawner blueJellySpawner = this;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * blueJellySpawner.Spine.timeScale) < 0.5)
      yield return (object) null;
    blueJellySpawner.StartCoroutine((IEnumerator) blueJellySpawner.TeleportIE());
    blueJellySpawner.cDelayTeleport = (Coroutine) null;
  }

  public override void Update()
  {
    this.TeleportOnHitDelay -= Time.deltaTime * this.Spine.timeScale;
    if (!this.teleporting)
      base.Update();
    if (this.inRange && (double) this.spawnTime == 3.4028234663852886E+38)
      this.SetSpawnTime();
    if (this.inRange && (double) this.teleportTime == 3.4028234663852886E+38)
      this.SetTeleportTime();
    if (this.inRange && GameManager.RoomActive && (bool) (UnityEngine.Object) this.targetObject && (bool) (UnityEngine.Object) this.gm && (double) this.gm.CurrentTime > (double) this.spawnTime / (double) this.Spine.timeScale)
      this.SpawnEnemy();
    if ((bool) (UnityEngine.Object) this.targetObject)
      this.inRange = (double) Vector3.Distance(this.targetObject.transform.position, this.transform.position) < (double) this.VisionRange;
    if (!this.canAttack)
      return;
    if (this.charging)
    {
      this.attackTimer += Time.deltaTime * this.Spine.timeScale;
      float amt = this.attackTimer / this.attackChargeDur;
      this.simpleSpineFlash.FlashWhite(amt);
      if ((double) amt > 1.0)
        this.Attack();
    }
    if (!(bool) (UnityEngine.Object) this.targetObject || this.charging || this.attacking || (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) >= (double) this.attackDistance)
      return;
    this.ChargeAttack();
  }

  public override void UpdateMoving()
  {
    if (this.attacking || this.spawning || this.teleporting)
      return;
    base.UpdateMoving();
  }

  public void ChargeAttack()
  {
    if (this.attacking || this.spawning)
      return;
    this.charging = true;
    this.attackTimer = 0.0f;
    this.state.CURRENT_STATE = StateMachine.State.Charging;
    this.Spine.AnimationState.SetAnimation(0, this.anticipationAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/chaser/chaser_charge", this.transform.position);
  }

  public void SpawnEnemy()
  {
    if (this.spawning || this.charging || this.attacking || this.teleporting || this.spawnableEnemies.Length == 0 || this.spawnedAmount >= this.maxEnemiesActive || (double) this.targetMinSpawnRadius != 0.0 && (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) <= (double) this.targetMinSpawnRadius)
      return;
    this.spawnRoutine = this.StartCoroutine((IEnumerator) this.SpawnDelay());
  }

  public IEnumerator SpawnDelay()
  {
    EnemyBlueJellySpawner blueJellySpawner1 = this;
    blueJellySpawner1.ClearPaths();
    blueJellySpawner1.spawning = true;
    blueJellySpawner1.Spine.AnimationState.SetAnimation(0, blueJellySpawner1.anticipationAnimation, true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * blueJellySpawner1.Spine.timeScale) < (double) blueJellySpawner1.spawnDuration)
      yield return (object) null;
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
      Addressables_wrapper.InstantiateAsync((object) blueJellySpawner1.spawnableEnemies[UnityEngine.Random.Range(0, blueJellySpawner1.spawnableEnemies.Length)], blueJellySpawner1.transform.position, Quaternion.identity, blueJellySpawner1.transform.parent, (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        EnemyExploder component1 = obj.Result.GetComponent<EnemyExploder>();
        component1.givePath(component1.transform.position + direction * 5f);
        component1.health.OnDie += new Health.DieAction(blueJellySpawner.OnEnemyKilled);
        component1.health.CanIncreaseDamageMultiplier = blueJellySpawner.isSpawnablesIncreasingDamageMultiplier;
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
      }));
    }
    blueJellySpawner1.Spine.AnimationState.SetAnimation(0, blueJellySpawner1.spawnAnimation, false);
    blueJellySpawner1.Spine.AnimationState.AddAnimation(0, blueJellySpawner1.idleAnimation, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * blueJellySpawner1.Spine.timeScale) < (double) blueJellySpawner1.spawnCooldown)
      yield return (object) null;
    blueJellySpawner1.state.CURRENT_STATE = StateMachine.State.Idle;
    blueJellySpawner1.Spine.AnimationState.SetAnimation(0, blueJellySpawner1.idleAnimation, true);
    blueJellySpawner1.SetSpawnTime();
    blueJellySpawner1.spawning = false;
    blueJellySpawner1.spawnRoutine = (Coroutine) null;
  }

  public void OnEnemyKilled(
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

  public void SetSpawnTime()
  {
    if (!(bool) (UnityEngine.Object) this.gm)
      return;
    this.spawnTime = this.gm.CurrentTime + UnityEngine.Random.Range(this.randomSpawnMinDelay, this.randomSpawnMaxDelay);
  }

  public void SetTeleportTime()
  {
    if (!(bool) (UnityEngine.Object) this.gm)
      return;
    this.teleportTime = this.gm.CurrentTime + UnityEngine.Random.Range(this.randomTeleportMinDelay, this.randomTeleportMaxDelay);
  }

  public IEnumerator TeleportIE()
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
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * blueJellySpawner.Spine.timeScale) < (double) blueJellySpawner.teleportMoveDelay)
        yield return (object) null;
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
      time = 0.0f;
      while ((double) (time += Time.deltaTime * blueJellySpawner.Spine.timeScale) < (double) blueJellySpawner.teleportCooldownDelay)
        yield return (object) null;
      blueJellySpawner.Spine.AnimationState.SetAnimation(0, blueJellySpawner.idleAnimation, true);
      blueJellySpawner.teleporting = false;
      blueJellySpawner.SetTeleportTime();
    }
  }

  public void Attack()
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

  public IEnumerator AttackCooldownIE()
  {
    EnemyBlueJellySpawner blueJellySpawner = this;
    yield return (object) new WaitForEndOfFrame();
    blueJellySpawner.simpleSpineFlash.FlashWhite(false);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * blueJellySpawner.Spine.timeScale) < (double) blueJellySpawner.attackCooldown)
      yield return (object) null;
    blueJellySpawner.attacking = false;
    blueJellySpawner.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    EnemyBlueJellySpawner blueJellySpawner = this;
    blueJellySpawner.damageColliderEvents.SetActive(true);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * blueJellySpawner.Spine.timeScale) < (double) duration)
      yield return (object) null;
    blueJellySpawner.damageColliderEvents.SetActive(false);
  }

  public new void OnDrawGizmos()
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
