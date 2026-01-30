// Decompiled with JetBrains decompiler
// Type: src.Extensions.AddressableExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
namespace src.Extensions;

public static class AddressableExtensions
{
  public static async Task<T> LoadAssetFromPath<T>(this string assetPath, bool immediate = false) where T : Object
  {
    return await AddressableExtensions.PerformAsyncLoadTask<T>(Addressables.LoadAssetAsync<Object>((object) assetPath));
  }

  public static async Task<T> LoadAddressableAsync<T>(
    this AssetReference assetReferenceGameObject,
    bool immediate = false)
    where T : Object
  {
    return await AddressableExtensions.PerformAsyncLoadTask<T>(Addressables.LoadAssetAsync<Object>((object) assetReferenceGameObject));
  }

  public static async Task<T> PerformAsyncLoadTask<T>(
    AsyncOperationHandle<Object> asyncOperationHandle)
    where T : Object
  {
    Object task = await asyncOperationHandle.Task;
    return AddressableExtensions.ParseResult<T>(asyncOperationHandle.Result);
  }

  public static T ParseResult<T>(Object result) where T : Object
  {
    if (result is GameObject gameObject)
    {
      T component = gameObject.GetComponent<T>();
      if ((Object) component != (Object) null)
        return component;
    }
    return result as T;
  }

  public static IEnumerator YieldUntilCompleted(this Task task)
  {
    while (!task.IsCompleted)
      yield return (object) null;
  }
}
