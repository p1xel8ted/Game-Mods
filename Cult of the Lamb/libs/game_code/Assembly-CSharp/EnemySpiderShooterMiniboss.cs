// Decompiled with JetBrains decompiler
// Type: EnemySpiderShooterMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemySpiderShooterMiniboss : EnemySpider
{
  [Space]
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spawnAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spawnAnimation;
  [Space]
  [SerializeField]
  public EnemySpiderShooterMiniboss.Shot[] shots;
  [SerializeField]
  public float timeBetweenShots;
  [SerializeField]
  public float minShootDistance;
  [Space]
  [SerializeField]
  public bool canSpawnEnemies;
  [SerializeField]
  public Vector2 spawnAmount;
  [SerializeField]
  public int maxActiveEnemies;
  [SerializeField]
  public float spawnAnticipation;
  [SerializeField]
  public Vector2 timeBetweenSpawns;
  [SerializeField]
  public AssetReferenceGameObject[] enemiesList;
  public float shootTimestamp;
  public float spawnTimestamp;
  public int shootIndex;
  public LayerMask islandMask;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public override void Awake()
  {
    base.Awake();
    this.islandMask = (LayerMask) ((int) this.islandMask | 1 << LayerMask.NameToLayer("Island"));
    this.spawnTimestamp = ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null ? GameManager.GetInstance().CurrentTime : Time.time) + UnityEngine.Random.Range(this.timeBetweenSpawns.x, this.timeBetweenSpawns.y);
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    this.Preload();
  }

  public void Preload()
  {
    for (int index = 0; index < this.enemiesList.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.enemiesList[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        obj.Result.CreatePool(20, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
  }

  public override void Update()
  {
    base.Update();
    float? currentTime;
    if (!this.Attacking)
    {
      currentTime = GameManager.GetInstance()?.CurrentTime;
      float shootTimestamp = this.shootTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) shootTimestamp & currentTime.HasValue && (bool) (UnityEngine.Object) PlayerFarming.Instance && (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) > (double) this.minShootDistance)
      {
        this.StartCoroutine((IEnumerator) this.ShootIE(this.shots[UnityEngine.Random.Range(0, this.shots.Length)]));
        return;
      }
    }
    if (!this.canSpawnEnemies || this.Attacking)
      return;
    currentTime = GameManager.GetInstance()?.CurrentTime;
    float spawnTimestamp = this.spawnTimestamp;
    if (!((double) currentTime.GetValueOrDefault() > (double) spawnTimestamp & currentTime.HasValue) || Health.team2.Count - 1 >= this.maxActiveEnemies)
      return;
    this.StartCoroutine((IEnumerator) this.SpawnIE());
  }

  public IEnumerator ShootIE(EnemySpiderShooterMiniboss.Shot shot)
  {
    EnemySpiderShooterMiniboss spiderShooterMiniboss = this;
    ++spiderShooterMiniboss.shootIndex;
    float time = 0.0f;
    spiderShooterMiniboss.updateDirection = false;
    spiderShooterMiniboss.Attacking = true;
    spiderShooterMiniboss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    spiderShooterMiniboss.ClearPaths();
    AudioManager.Instance.PlayOneShot(spiderShooterMiniboss.warningSfx, spiderShooterMiniboss.transform.position);
    for (int i = 0; i < shot.Amount; ++i)
    {
      spiderShooterMiniboss.SetAnimation(shot.shootAnticipationAnimation);
      spiderShooterMiniboss.TargetEnemy = spiderShooterMiniboss.GetClosestTarget();
      spiderShooterMiniboss.LookAtTarget();
      yield return (object) new WaitForEndOfFrame();
      float t = 0.0f;
      while ((double) t < (double) shot.ShootAnticipation)
      {
        float amt = t / shot.ShootAnticipation;
        spiderShooterMiniboss.SimpleSpineFlash.FlashWhite(amt);
        t += Time.deltaTime * spiderShooterMiniboss.Spine.timeScale;
        yield return (object) null;
      }
      spiderShooterMiniboss.SimpleSpineFlash.FlashWhite(false);
      spiderShooterMiniboss.SetAnimation(shot.shootAnimation);
      spiderShooterMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
      spiderShooterMiniboss.StartCoroutine((IEnumerator) shot.ProjectilePattern.ShootIE());
      AudioManager.Instance.PlayOneShot(spiderShooterMiniboss.attackSfx, spiderShooterMiniboss.transform.position);
      spiderShooterMiniboss.TargetEnemy = spiderShooterMiniboss.GetClosestTarget();
      spiderShooterMiniboss.LookAtTarget();
      time = 0.0f;
      while ((double) (time += Time.deltaTime * spiderShooterMiniboss.Spine.timeScale) < (double) shot.TimeBetweenShooting)
        yield return (object) null;
    }
    spiderShooterMiniboss.AddAnimation(spiderShooterMiniboss.IdleAnimation, true);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderShooterMiniboss.Spine.timeScale) < (double) shot.ShootCooldown)
      yield return (object) null;
    spiderShooterMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    spiderShooterMiniboss.shootTimestamp = GameManager.GetInstance().CurrentTime + spiderShooterMiniboss.timeBetweenShots;
    spiderShooterMiniboss.updateDirection = true;
    spiderShooterMiniboss.Attacking = false;
  }

  public IEnumerator SpawnIE()
  {
    EnemySpiderShooterMiniboss spiderShooterMiniboss = this;
    spiderShooterMiniboss.updateDirection = false;
    spiderShooterMiniboss.Attacking = true;
    spiderShooterMiniboss.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    spiderShooterMiniboss.ClearPaths();
    spiderShooterMiniboss.SetAnimation(spiderShooterMiniboss.spawnAnticipationAnimation);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderShooterMiniboss.Spine.timeScale) < (double) spiderShooterMiniboss.spawnAnticipation)
      yield return (object) null;
    spiderShooterMiniboss.SetAnimation(spiderShooterMiniboss.spawnAnimation);
    spiderShooterMiniboss.AddAnimation(spiderShooterMiniboss.IdleAnimation);
    int num = UnityEngine.Random.Range((int) spiderShooterMiniboss.spawnAmount.x, (int) spiderShooterMiniboss.spawnAmount.y + 1);
    UnityEngine.Random.Range(0, 360);
    for (int index = 0; index < num; ++index)
    {
      GameObject gameObject = ObjectPool.Spawn(spiderShooterMiniboss.loadedAddressableAssets[UnityEngine.Random.Range(0, spiderShooterMiniboss.enemiesList.Length)].Result, spiderShooterMiniboss.transform.parent, spiderShooterMiniboss.transform.position, Quaternion.identity);
      UnitObject component1 = gameObject.GetComponent<UnitObject>();
      component1.CanHaveModifier = false;
      component1.RemoveModifier();
      Interaction_Chest.Instance?.AddEnemy(component1.health);
      EnemyRoundsBase.Instance?.AddEnemyToRound(component1.GetComponent<Health>());
      component1.RemoveModifier();
      SpawnEnemyOnDeath component2 = gameObject.GetComponent<SpawnEnemyOnDeath>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.enabled = false;
      Vector3 localScale = component1.transform.localScale;
      component1.transform.localScale = Vector3.zero;
      component1.transform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
      if (!string.IsNullOrEmpty(spiderShooterMiniboss.spawnAnimation))
      {
        foreach (SkeletonAnimation componentsInChild in component1.GetComponentsInChildren<SkeletonAnimation>())
        {
          componentsInChild.AnimationState.SetAnimation(0, spiderShooterMiniboss.spawnAnimation, false);
          componentsInChild.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        }
      }
      component1.DoKnockBack(UnityEngine.Random.Range(0.0f, 360f), 2f, 1f);
      component1.StartCoroutine((IEnumerator) spiderShooterMiniboss.DelayedEnemyHealthEnable(component1));
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderShooterMiniboss.Spine.timeScale) < 1.0)
      yield return (object) null;
    spiderShooterMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    spiderShooterMiniboss.updateDirection = true;
    spiderShooterMiniboss.spawnTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(spiderShooterMiniboss.timeBetweenSpawns.x, spiderShooterMiniboss.timeBetweenSpawns.y);
    spiderShooterMiniboss.Attacking = false;
  }

  public IEnumerator DelayedEnemyHealthEnable(UnitObject enemy)
  {
    EnemySpiderShooterMiniboss spiderShooterMiniboss = this;
    enemy.health.invincible = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderShooterMiniboss.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemy.health.invincible = false;
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) enemy.transform.position, 0.5f))
    {
      Health component = collider2D.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.team == Health.Team.Neutral)
        collider2D.GetComponent<Health>().DealDamage((float) int.MaxValue, enemy.gameObject, Vector3.Lerp(component.transform.position, enemy.transform.position, 0.7f));
    }
  }

  public override bool ShouldAttack()
  {
    return (double) (this.AttackDelay -= Time.deltaTime) < 0.0 && !this.Attacking && (bool) (UnityEngine.Object) this.TargetEnemy && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) < (double) this.VisionRange && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer && (double) UnityEngine.Random.Range(0.0f, 1f) > 0.6600000262260437;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__19_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(20, true);
  }

  [Serializable]
  public struct Shot
  {
    public int Amount;
    public float ShootAnticipation;
    public float ShootDuration;
    public float ShootCooldown;
    public float TimeBetweenShooting;
    [SpineAnimation("", "", true, false, dataField = "Spine")]
    public string shootAnticipationAnimation;
    [SpineAnimation("", "", true, false, dataField = "Spine")]
    public string shootAnimation;
    public ProjectilePatternBase ProjectilePattern;
  }
}
