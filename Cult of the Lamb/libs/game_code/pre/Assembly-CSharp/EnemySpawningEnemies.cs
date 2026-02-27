// Decompiled with JetBrains decompiler
// Type: EnemySpawningEnemies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
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
  private SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string PreSpawnAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string SpawnAnimation;
  private TrackEntry TrackEntry;
  public bool DoScaleUp;
  public bool DoTweenMove;
  public float TweenMoveDuration = 0.5f;
  public float TweenMoveTowardsPlayerDistance = 0.5f;
  private StateMachine state;
  private bool Spawning;
  private Health health;
  private List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  private List<Health> ChildrenList = new List<Health>();
  private GameObject g;

  private void OnDestroy()
  {
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  private void OnEnable()
  {
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.Spawning = false;
  }

  private void OnDisable() => this.health.OnDie -= new Health.DieAction(this.OnDie);

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    foreach (Health children in this.ChildrenList)
      children.DestroyNextFrame();
  }

  private void Update()
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

  private IEnumerator SpawnRoutine()
  {
    EnemySpawningEnemies enemySpawningEnemies = this;
    enemySpawningEnemies.Spawning = true;
    AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", enemySpawningEnemies.transform.position);
    if (enemySpawningEnemies.PlayAnimations)
    {
      enemySpawningEnemies.TrackEntry = enemySpawningEnemies.Spine.AnimationState.SetAnimation(0, enemySpawningEnemies.PreSpawnAnimation, false);
      yield return (object) new WaitForSeconds(enemySpawningEnemies.TrackEntry.TrackEnd);
    }
    // ISSUE: reference to a compiler-generated method
    Addressables.LoadAssetAsync<GameObject>((object) enemySpawningEnemies.EnemyPrefab).Completed += new Action<AsyncOperationHandle<GameObject>>(enemySpawningEnemies.\u003CSpawnRoutine\u003Eb__26_0);
    yield return (object) new WaitForSeconds(enemySpawningEnemies.SpawnDelay + (enemySpawningEnemies.PlayAnimations ? 1.25f : 0.0f));
    enemySpawningEnemies.Spawning = false;
  }

  private void OnSpawnedDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    Victim.OnDie -= new Health.DieAction(this.OnSpawnedDie);
    this.ChildrenList.Remove(Victim);
  }

  private IEnumerator MoveTweenRoutine(Transform t)
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

  private IEnumerator ScaleUp(Transform t)
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
}
