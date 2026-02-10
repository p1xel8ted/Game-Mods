// Decompiled with JetBrains decompiler
// Type: WoolhavenCritterManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class WoolhavenCritterManager : MonoBehaviour
{
  public static List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  [SerializeField]
  public AssetReferenceGameObject ratReference;
  [SerializeField]
  public AssetReferenceGameObject mouseReference;
  [SerializeField]
  public float spawnRadius = 2f;
  [SerializeField]
  public Vector3[] SpawnPositions;
  public GameObject loadedRatPrefab;
  public GameObject loadedMousePrefab;
  public bool initialized;
  public List<CritterRat> spawnedCritters = new List<CritterRat>();
  public static WoolhavenCritterManager instance;

  public int NumberOfCrittersToSpawn => this.SpawnPositions.Length;

  public void Awake()
  {
    if (DataManager.Instance.MAJOR_DLC)
      this.LoadAssets();
    WoolhavenCritterManager.instance = this;
  }

  public void Update()
  {
    if (!this.initialized || this.spawnedCritters.Count != 0)
      return;
    this.SpawnCritters();
  }

  public void OnDestroy()
  {
    this.UnloadAssets();
    this.spawnedCritters.Clear();
  }

  public void SpawnCritters()
  {
    Vector3 zero = Vector3.zero;
    CritterRat critterRat = (CritterRat) null;
    for (int index = 0; index < this.NumberOfCrittersToSpawn; ++index)
    {
      Vector3 position = this.transform.TransformPoint(this.SpawnPositions[index]) + (Vector3) UnityEngine.Random.insideUnitCircle * this.spawnRadius;
      if (((double) index + 1.0) / (double) this.NumberOfCrittersToSpawn <= (double) DataManager.Instance.TotalShrineGhostJuice / 40.0)
      {
        if ((UnityEngine.Object) this.loadedMousePrefab != (UnityEngine.Object) null)
          critterRat = UnityEngine.Object.Instantiate<GameObject>(this.loadedMousePrefab, position, Quaternion.identity, this.transform).GetComponent<CritterRat>();
      }
      else if ((UnityEngine.Object) this.loadedRatPrefab != (UnityEngine.Object) null)
        critterRat = UnityEngine.Object.Instantiate<GameObject>(this.loadedRatPrefab, position, Quaternion.identity, this.transform).GetComponent<CritterRat>();
      this.spawnedCritters.Add(critterRat);
      critterRat = (CritterRat) null;
    }
  }

  public void LoadAssets()
  {
    int assetsToLoad = 0;
    if (DataManager.Instance.TotalShrineGhostJuice != 40)
    {
      assetsToLoad++;
      Addressables.LoadAssetAsync<GameObject>((object) this.ratReference).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        WoolhavenCritterManager.loadedAddressableAssets.Add(obj);
        this.loadedRatPrefab = obj.Result;
        if (WoolhavenCritterManager.loadedAddressableAssets.Count != assetsToLoad)
          return;
        this.initialized = true;
      });
    }
    if (1.0 / (double) this.NumberOfCrittersToSpawn > (double) DataManager.Instance.TotalShrineGhostJuice / 40.0)
      return;
    assetsToLoad++;
    Addressables.LoadAssetAsync<GameObject>((object) this.mouseReference).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      WoolhavenCritterManager.loadedAddressableAssets.Add(obj);
      this.loadedMousePrefab = obj.Result;
      if (WoolhavenCritterManager.loadedAddressableAssets.Count != assetsToLoad)
        return;
      this.initialized = true;
    });
  }

  public void UnloadAssets()
  {
    if (WoolhavenCritterManager.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in WoolhavenCritterManager.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    WoolhavenCritterManager.loadedAddressableAssets.Clear();
  }

  public void OnDrawGizmosSelected()
  {
    if (this.SpawnPositions == null)
      return;
    for (int index = 0; index < this.SpawnPositions.Length; ++index)
      Utils.DrawCircleXY(this.SpawnPositions[index] + this.transform.position, this.spawnRadius, Color.yellow);
  }
}
