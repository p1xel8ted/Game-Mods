// Decompiled with JetBrains decompiler
// Type: Addressables_wrapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

#nullable disable
public class Addressables_wrapper : MonoBehaviour
{
  public static Action<AsyncOperationHandle<GameObject>> action;

  public static void InstantiateAsync(object key, Action<AsyncOperationHandle<GameObject>> callback = null)
  {
    Addressables_wrapper.action = callback;
    Addressables.InstantiateAsync(key).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      callback(obj);
      obj.Result.AddComponent((System.Type) typeof (SelfCleanup));
    });
  }

  public static GameObject InstantiateSynchrous(object key, Transform parent = null)
  {
    AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(key, parent);
    asyncOperationHandle.WaitForCompletion();
    asyncOperationHandle.Result.AddComponent((System.Type) typeof (SelfCleanup));
    return asyncOperationHandle.Result;
  }

  public static void InstantiateAsync(
    object key,
    Transform parent = null,
    Action<AsyncOperationHandle<GameObject>> callback = null)
  {
    Addressables_wrapper.action = callback;
    Addressables.InstantiateAsync(key, parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      callback(obj);
      obj.Result.AddComponent((System.Type) typeof (SelfCleanup));
    });
  }

  public static void InstantiateAsync(
    object key,
    Transform parent = null,
    bool instantiateInWorldSpace = false,
    Action<AsyncOperationHandle<GameObject>> callback = null)
  {
    Addressables_wrapper.action = callback;
    Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      callback(obj);
      obj.Result.AddComponent((System.Type) typeof (SelfCleanup));
    });
  }

  public static void InstantiateAsync(
    object key,
    Vector3 position,
    Quaternion rotation,
    Transform parent = null,
    Action<AsyncOperationHandle<GameObject>> callback = null)
  {
    Addressables_wrapper.action = callback;
    Addressables.InstantiateAsync(key, position, rotation, parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      callback(obj);
      obj.Result.AddComponent((System.Type) typeof (SelfCleanup));
    });
  }

  public static void InstantiateAsync(
    object key,
    InstantiationParameters instantiateParameters,
    Action<AsyncOperationHandle<GameObject>> callback)
  {
    Addressables.InstantiateAsync(key, instantiateParameters).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      callback(obj);
      obj.Result.AddComponent((System.Type) typeof (SelfCleanup));
    });
  }
}
