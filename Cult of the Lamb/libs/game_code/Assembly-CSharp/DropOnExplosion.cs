// Decompiled with JetBrains decompiler
// Type: DropOnExplosion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class DropOnExplosion : MonoBehaviour
{
  public EnemyExploder EnemyExploder;
  [SerializeField]
  public GameObject poisonPrefab;
  [SerializeField]
  public int amount = 3;
  [SerializeField]
  public float radius = 1f;
  public AssetReferenceGameObject[] EnemyList;
  public int NumToSpawn = 5;
  [SerializeField]
  public float growSpeed = 0.3f;
  [SerializeField]
  public Ease growEase = Ease.OutCubic;
  [SerializeField]
  public float spawnSpitOutForce = 0.7f;
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();

  public void OnEnable() => this.EnemyExploder.OnExplode += new System.Action(this.OnExplode);

  public void OnDisable() => this.EnemyExploder.OnExplode -= new System.Action(this.OnExplode);

  public void Awake()
  {
    if (!((UnityEngine.Object) this.poisonPrefab != (UnityEngine.Object) null))
      return;
    ObjectPool.CreatePool(this.poisonPrefab, ObjectPool.CountPooled(this.poisonPrefab) + this.amount);
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    this.Preload();
  }

  public void Preload()
  {
    for (int index = 0; index < this.EnemyList.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.EnemyList[index]);
      asyncOperationHandle.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        int num = ObjectPool.CountPooled(obj.Result);
        obj.Result.CreatePool(num + this.NumToSpawn, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
  }

  public void OnExplode()
  {
    for (int index = 0; index < this.amount; ++index)
      ObjectPool.Spawn(this.poisonPrefab, this.transform.parent, this.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * this.radius), Quaternion.identity);
    for (int index = 0; index < this.NumToSpawn; ++index)
    {
      Health.team2.Add((Health) null);
      Interaction_Chest.Instance?.Enemies.Add((Health) null);
      Vector3 position = this.transform.position;
      float f = (float) (360.0 / (double) this.NumToSpawn * (double) index * (Math.PI / 180.0));
      Vector3 vector3 = new Vector3(Mathf.Cos(f), Mathf.Sin(f));
      GameObject gameObject = ObjectPool.Spawn(this.loadedAddressableAssets[UnityEngine.Random.Range(0, this.loadedAddressableAssets.Count)].Result, this.transform.parent, position, Quaternion.identity);
      if (Health.team2.Contains((Health) null))
      {
        Health.team2.Remove((Health) null);
        Interaction_Chest.Instance?.Enemies.Remove((Health) null);
      }
      EnemyExploder component1 = gameObject.GetComponent<EnemyExploder>();
      component1.givePath(component1.transform.position + vector3 * 2f);
      EnemyRoundsBase.Instance?.AddEnemyToRound(component1.GetComponent<Health>());
      Interaction_Chest.Instance?.AddEnemy(gameObject.GetComponent<Health>());
      if ((double) this.growSpeed != 0.0)
      {
        component1.Spine.transform.localScale = Vector3.zero;
        component1.Spine.transform.DOScale(1f, this.growSpeed).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.growEase);
      }
      float angle = Utils.GetAngle(this.transform.position, this.transform.position + vector3) * ((float) Math.PI / 180f);
      component1.DoKnockBack(angle, this.spawnSpitOutForce, 0.75f);
      component1.chase = true;
      DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
      if ((bool) (UnityEngine.Object) component2)
        component2.GiveXP = false;
    }
  }

  public void OnDestroy()
  {
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__14_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    int num = ObjectPool.CountPooled(obj.Result);
    obj.Result.CreatePool(num + this.NumToSpawn, true);
  }
}
