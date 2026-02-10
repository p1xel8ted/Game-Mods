// Decompiled with JetBrains decompiler
// Type: EnemySpawningEnemies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemySpawningEnemies : BaseMonoBehaviour
{
  public AssetReferenceGameObject EnemyPrefab;
  public int Limit = 5;
  public float SpawnDelay = 3f;
  public float ScaleUpDuration = 1f;
  public float DistanceFromPlayerToSpawn = 5f;
  public bool PlayAnimations;
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string PreSpawnAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SpawnAnimation;
  public TrackEntry TrackEntry;
  public bool DoScaleUp;
  public bool DoTweenMove;
  public float TweenMoveDuration = 0.5f;
  public float TweenMoveTowardsPlayerDistance = 0.5f;
  public StateMachine state;
  public bool Spawning;
  public Health health;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public List<Health> ChildrenList = new List<Health>();
  public GameObject g;

  public void OnDestroy()
  {
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    this.Preload();
  }

  public void Preload()
  {
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.EnemyPrefab);
    asyncOperationHandle.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      this.loadedAddressableAssets.Add(obj);
      obj.Result.CreatePool(24, true);
    });
    asyncOperationHandle.WaitForCompletion();
  }

  public void OnEnable()
  {
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.Spawning = false;
  }

  public void OnDisable() => this.health.OnDie -= new Health.DieAction(this.OnDie);

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    foreach (Health children in this.ChildrenList)
      children.DestroyNextFrame();
  }

  public void Update()
  {
    if (Time.frameCount % 10 != 0 || this.Spawning)
      return;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
        this.Spawn();
        break;
    }
  }

  public void Spawn()
  {
    if (this.ChildrenList.Count >= this.Limit || !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) <= (double) this.DistanceFromPlayerToSpawn)
      return;
    this.StartCoroutine((IEnumerator) this.SpawnRoutine());
  }

  public IEnumerator SpawnRoutine()
  {
    EnemySpawningEnemies enemySpawningEnemies = this;
    enemySpawningEnemies.Spawning = true;
    AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", enemySpawningEnemies.transform.position);
    if (enemySpawningEnemies.PlayAnimations)
    {
      enemySpawningEnemies.TrackEntry = enemySpawningEnemies.Spine.AnimationState.SetAnimation(0, enemySpawningEnemies.PreSpawnAnimation, false);
      yield return (object) new WaitForSeconds(enemySpawningEnemies.TrackEntry.TrackEnd);
    }
    enemySpawningEnemies.g = ObjectPool.Spawn(enemySpawningEnemies.loadedAddressableAssets[0].Result, enemySpawningEnemies.transform.parent, enemySpawningEnemies.transform.position, Quaternion.identity);
    EnemyScuttleSwiper component1 = enemySpawningEnemies.g.GetComponent<EnemyScuttleSwiper>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.AttackDelay = enemySpawningEnemies.ScaleUpDuration + 0.5f;
    foreach (DropLootOnDeath component2 in enemySpawningEnemies.g.GetComponents<DropLootOnDeath>())
      component2.GiveXP = false;
    foreach (Behaviour component3 in enemySpawningEnemies.g.GetComponents<DropMultipleLootOnDeath>())
      component3.enabled = false;
    Health component4 = enemySpawningEnemies.g.GetComponent<Health>();
    component4.OnDie += new Health.DieAction(enemySpawningEnemies.OnSpawnedDie);
    enemySpawningEnemies.ChildrenList.Add(component4);
    if (enemySpawningEnemies.DoScaleUp)
      component4.StartCoroutine((IEnumerator) enemySpawningEnemies.ScaleUp(enemySpawningEnemies.g.transform));
    if (enemySpawningEnemies.DoTweenMove)
      component4.StartCoroutine((IEnumerator) enemySpawningEnemies.MoveTweenRoutine(enemySpawningEnemies.g.transform));
    if (enemySpawningEnemies.PlayAnimations)
      enemySpawningEnemies.TrackEntry = enemySpawningEnemies.Spine.AnimationState.SetAnimation(0, enemySpawningEnemies.SpawnAnimation, false);
    yield return (object) new WaitForSeconds(enemySpawningEnemies.SpawnDelay + (enemySpawningEnemies.PlayAnimations ? 1.25f : 0.0f));
    enemySpawningEnemies.Spawning = false;
  }

  public void OnSpawnedDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    Victim.OnDie -= new Health.DieAction(this.OnSpawnedDie);
    this.ChildrenList.Remove(Victim);
  }

  public IEnumerator MoveTweenRoutine(Transform t)
  {
    EnemySpawningEnemies enemySpawningEnemies = this;
    yield return (object) new WaitForEndOfFrame();
    float Progress = 0.0f;
    float Duration = enemySpawningEnemies.TweenMoveDuration;
    Vector3 StartPosition = enemySpawningEnemies.transform.position;
    Vector3 TargetPosition = enemySpawningEnemies.transform.position + ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null ? Vector3.zero : (PlayerFarming.Instance.transform.position - enemySpawningEnemies.transform.position) * enemySpawningEnemies.TweenMoveTowardsPlayerDistance);
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      t.position = Vector3.Lerp(StartPosition, TargetPosition, Progress / Duration);
      yield return (object) null;
    }
    t.position = TargetPosition;
  }

  public IEnumerator ScaleUp(Transform t)
  {
    yield return (object) new WaitForEndOfFrame();
    float Progress = 0.0f;
    float Duration = this.ScaleUpDuration;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      t.localScale = Vector3.one * Mathf.SmoothStep(0.2f, 1f, Progress / Duration);
      yield return (object) null;
    }
    t.localScale = Vector3.one;
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__20_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(24, true);
  }
}
