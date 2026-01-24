// Decompiled with JetBrains decompiler
// Type: SpriteAtlasLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

#nullable disable
public class SpriteAtlasLoader : MonoBehaviour
{
  public static GameObject spriteAtlasLoader;
  public static Dictionary<string, AsyncOperationHandle<SpriteAtlas>> loadedAtlases = new Dictionary<string, AsyncOperationHandle<SpriteAtlas>>();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void LoadUIManager()
  {
    if (!((UnityEngine.Object) SpriteAtlasLoader.spriteAtlasLoader == (UnityEngine.Object) null))
      return;
    SpriteAtlasLoader.spriteAtlasLoader = new GameObject(nameof (SpriteAtlasLoader));
    SpriteAtlasLoader.spriteAtlasLoader.AddComponent<SpriteAtlasLoader>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) SpriteAtlasLoader.spriteAtlasLoader);
  }

  public void OnEnable()
  {
    SpriteAtlasManager.atlasRequested += (Action<string, Action<SpriteAtlas>>) new Action<string, Action<SpriteAtlas>>(this.OnAtlasRequested);
  }

  public void OnDestroy()
  {
    SpriteAtlasManager.atlasRequested -= (Action<string, Action<SpriteAtlas>>) new Action<string, Action<SpriteAtlas>>(this.OnAtlasRequested);
    SpriteAtlasLoader.ReleaseAtlases();
  }

  public void OnAtlasRequested(string tag, Action<SpriteAtlas> action)
  {
    AsyncOperationHandle<SpriteAtlas> handle;
    if (SpriteAtlasLoader.loadedAtlases.TryGetValue(tag, out handle))
    {
      if (handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded && (UnityEngine.Object) handle.Result != (UnityEngine.Object) null)
      {
        Action<SpriteAtlas> action1 = action;
        if (action1 == null)
          return;
        action1(handle.Result);
        return;
      }
      if (handle.IsValid() && !handle.IsDone)
        return;
      if (handle.IsValid())
        Addressables.Release<SpriteAtlas>(handle);
      SpriteAtlasLoader.loadedAtlases.Remove(tag);
    }
    AsyncOperationHandle<SpriteAtlas> asyncOperationHandle = Addressables.LoadAssetAsync<SpriteAtlas>((object) tag);
    SpriteAtlasLoader.loadedAtlases[tag] = asyncOperationHandle;
    asyncOperationHandle.Completed += (Action<AsyncOperationHandle<SpriteAtlas>>) (h =>
    {
      if (h.Status == AsyncOperationStatus.Succeeded && (UnityEngine.Object) h.Result != (UnityEngine.Object) null)
      {
        Action<SpriteAtlas> action2 = action;
        if (action2 == null)
          return;
        action2(h.Result);
      }
      else
      {
        Debug.LogError((object) $"[SpriteAtlasLoader] Failed to load SpriteAtlas with tag '{tag}'.");
        if (h.IsValid())
          Addressables.Release<SpriteAtlas>(h);
        SpriteAtlasLoader.loadedAtlases.Remove(tag);
      }
    });
  }

  public static void ReleaseAtlases()
  {
    foreach (AsyncOperationHandle<SpriteAtlas> handle in SpriteAtlasLoader.loadedAtlases.Values)
      Addressables.Release<SpriteAtlas>(handle);
    SpriteAtlasLoader.loadedAtlases.Clear();
  }
}
