// Decompiled with JetBrains decompiler
// Type: AddressableLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class AddressableLoader : MonoBehaviour
{
  [SerializeField]
  public AssetReferenceGameObject assetReference;
  [SerializeField]
  public bool keepAlwaysInMemory;
  public static Dictionary<string, AsyncOperationHandle<GameObject>> assetHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
  public static Dictionary<string, int> assetHandlesRefCounter = new Dictionary<string, int>();
  public bool InitializeOnAwake = true;
  public bool isInitialized;

  public string AssetGUID => this.assetReference.AssetGUID;

  public void Awake()
  {
    if (!this.InitializeOnAwake)
      return;
    this.Initialize();
  }

  public void Initialize()
  {
    if (this.isInitialized)
      return;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.InitializeAsync());
  }

  public IEnumerator InitializeAsync()
  {
    AsyncOperationHandle<GameObject> opHandle;
    if (AddressableLoader.assetHandles.TryGetValue(this.AssetGUID, out opHandle))
    {
      yield return (object) new WaitUntil((Func<bool>) (() => opHandle.IsDone));
      if (opHandle.IsValid())
      {
        this.Spawn(opHandle.Result);
        this.isInitialized = true;
        yield break;
      }
      this.Unload();
    }
    AsyncOperationHandle<GameObject> op = Addressables.LoadAssetAsync<GameObject>((object) this.assetReference);
    yield return (object) new WaitUntil((Func<bool>) (() => op.IsDone));
    if (op.Status == AsyncOperationStatus.Succeeded)
    {
      if (!AddressableLoader.assetHandles.ContainsKey(this.AssetGUID))
        AddressableLoader.assetHandles.Add(this.AssetGUID, op);
      this.Spawn(op.Result);
      this.isInitialized = true;
    }
    else
      Debug.LogError((object) $"Failed to load asset: {this.assetReference}");
  }

  public void Spawn(GameObject go)
  {
    this.IncreaseRefCounter();
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(go);
    Vector3 localPosition = gameObject.transform.localPosition;
    Quaternion localRotation = gameObject.transform.localRotation;
    Vector3 localScale = gameObject.transform.localScale;
    gameObject.transform.parent = this.gameObject.transform;
    gameObject.transform.localPosition = localPosition;
    gameObject.transform.localRotation = localRotation;
    gameObject.transform.localScale = localScale;
  }

  public void IncreaseRefCounter()
  {
    int num;
    if (AddressableLoader.assetHandlesRefCounter.TryGetValue(this.AssetGUID, out num))
      AddressableLoader.assetHandlesRefCounter[this.AssetGUID] = num + 1;
    else
      AddressableLoader.assetHandlesRefCounter.Add(this.AssetGUID, 1);
  }

  public void DecreaseRefCounter()
  {
    int num;
    if (AddressableLoader.assetHandlesRefCounter.TryGetValue(this.AssetGUID, out num))
      AddressableLoader.assetHandlesRefCounter[this.AssetGUID] = num - 1;
    else
      AddressableLoader.assetHandlesRefCounter.Add(this.AssetGUID, 0);
  }

  public void OnDestroy()
  {
    if (this.keepAlwaysInMemory)
      return;
    this.Unload();
  }

  public void Unload()
  {
    this.DecreaseRefCounter();
    AsyncOperationHandle<GameObject> handle;
    if (!AddressableLoader.assetHandles.TryGetValue(this.AssetGUID, out handle))
      return;
    if (handle.IsValid() && AddressableLoader.assetHandlesRefCounter[this.AssetGUID] <= 0)
      Addressables.Release<GameObject>(handle);
    if (handle.IsValid())
      return;
    AddressableLoader.assetHandles.Remove(this.AssetGUID);
  }
}
