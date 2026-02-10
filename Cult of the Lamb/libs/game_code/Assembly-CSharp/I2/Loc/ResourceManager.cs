// Decompiled with JetBrains decompiler
// Type: I2.Loc.ResourceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

#nullable disable
namespace I2.Loc;

public class ResourceManager : MonoBehaviour
{
  public static bool initialising;
  public static ResourceManager mInstance;
  public List<IResourceManager_Bundles> mBundleManagers = new List<IResourceManager_Bundles>();
  public UnityEngine.Object[] Assets;
  public Dictionary<string, UnityEngine.Object> mResourcesCache = new Dictionary<string, UnityEngine.Object>((IEqualityComparer<string>) StringComparer.Ordinal);
  public Dictionary<string, AsyncOperationHandle<UnityEngine.Object>> mAddressablesCache = new Dictionary<string, AsyncOperationHandle<UnityEngine.Object>>();
  public HashSet<string> loadInProgress = new HashSet<string>();

  public static ResourceManager pInstance
  {
    get
    {
      bool flag = (UnityEngine.Object) ResourceManager.mInstance == (UnityEngine.Object) null;
      if (!ResourceManager.initialising)
      {
        Addressables.InitializeAsync();
        ResourceManager.initialising = true;
      }
      if ((UnityEngine.Object) ResourceManager.mInstance == (UnityEngine.Object) null)
        ResourceManager.mInstance = (ResourceManager) UnityEngine.Object.FindObjectOfType((System.Type) typeof (ResourceManager));
      if ((UnityEngine.Object) ResourceManager.mInstance == (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject("I2ResourceManager", (System.Type[]) new System.Type[1]
        {
          typeof (ResourceManager)
        });
        gameObject.hideFlags |= HideFlags.HideAndDontSave;
        ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
        SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(ResourceManager.MyOnLevelWasLoaded);
      }
      if (flag && Application.isPlaying)
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) ResourceManager.mInstance.gameObject);
      return ResourceManager.mInstance;
    }
  }

  public static void MyOnLevelWasLoaded(Scene scene, LoadSceneMode mode)
  {
    ResourceManager.pInstance.CleanResourceCache();
    LocalizationManager.UpdateSources();
  }

  public T GetAsset<T>(string Name) where T : UnityEngine.Object
  {
    T asset = this.FindAsset(Name) as T;
    return (UnityEngine.Object) asset != (UnityEngine.Object) null ? asset : this.LoadFromResources<T>(Name);
  }

  public UnityEngine.Object FindAsset(string Name)
  {
    if (this.Assets != null)
    {
      int index = 0;
      for (int length = this.Assets.Length; index < length; ++index)
      {
        if (this.Assets[index] != (UnityEngine.Object) null && this.Assets[index].name == Name)
          return this.Assets[index];
      }
    }
    return (UnityEngine.Object) null;
  }

  public bool HasAsset(UnityEngine.Object Obj)
  {
    return this.Assets != null && Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
  }

  public T LoadFromResources<T>(string Path) where T : UnityEngine.Object
  {
    try
    {
      if (string.IsNullOrEmpty(Path))
        return default (T);
      UnityEngine.Object @object;
      if (this.mResourcesCache.TryGetValue(Path, out @object) && @object != (UnityEngine.Object) null)
        return @object as T;
      T obj = default (T);
      if (Path.EndsWith("]", StringComparison.OrdinalIgnoreCase))
      {
        int length1 = Path.LastIndexOf("[", StringComparison.OrdinalIgnoreCase);
        int length2 = Path.Length - length1 - 2;
        string str = Path.Substring(length1 + 1, length2);
        Path = Path.Substring(0, length1);
        T[] objArray = Resources.LoadAll<T>(Path);
        int index = 0;
        for (int length3 = objArray.Length; index < length3; ++index)
        {
          if (objArray[index].name.Equals(str))
          {
            obj = objArray[index];
            break;
          }
        }
      }
      else
        obj = Resources.Load(Path, (System.Type) typeof (T)) as T;
      if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
        obj = this.LoadFromBundle<T>(Path);
      if ((UnityEngine.Object) obj != (UnityEngine.Object) null)
        this.mResourcesCache[Path] = (UnityEngine.Object) obj;
      return obj;
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("Unable to load {0} '{1}'\nERROR: {2}", (object) typeof (T), (object) Path, (object) ex.ToString());
      return default (T);
    }
  }

  public T LoadFromBundle<T>(string path) where T : UnityEngine.Object
  {
    int index = 0;
    for (int count = this.mBundleManagers.Count; index < count; ++index)
    {
      if (this.mBundleManagers[index] != null)
      {
        T obj = this.mBundleManagers[index].LoadFromBundle(path, typeof (T)) as T;
        if ((UnityEngine.Object) obj != (UnityEngine.Object) null)
          return obj;
      }
    }
    return default (T);
  }

  public void CleanResourceCache(bool unloadResources = false)
  {
    this.mResourcesCache.Clear();
    if (unloadResources)
      Resources.UnloadUnusedAssets();
    this.CancelInvoke();
  }

  public T LoadFromAddressables<T>(string Path, bool async = false, Action<T> onLoaded = null) where T : UnityEngine.Object
  {
    try
    {
      if (string.IsNullOrEmpty(Path))
        return default (T);
      if (Path == "FiraSans-Regular SDF")
        Path = "Fonts/Body/FiraSans-Regular SDF";
      else if (Path == "Font Awesome 6 Pro-Solid-900 SDF")
        Path = "Fonts/LocalisedFonts/FontAwesome/Font Awesome 6 Pro-Solid-900 SDF";
      Path = ResourceManager.ConvertPathToAddressable(Path);
      if (!ResourceManager.AssetExists((object) Path))
        return default (T);
      AsyncOperationHandle<UnityEngine.Object> ObjectOperation;
      if (this.mAddressablesCache.TryGetValue(Path, out ObjectOperation) && ObjectOperation.IsValid())
      {
        Action<T> action = onLoaded;
        if (action != null)
          action(ObjectOperation.Result as T);
        return ObjectOperation.Result as T;
      }
      if (this.loadInProgress.Contains(Path))
        return default (T);
      T obj = default (T);
      ObjectOperation = Addressables.LoadAssetAsync<UnityEngine.Object>((object) Path);
      this.loadInProgress.Add(Path);
      if (!async)
      {
        if (Path.Contains("Fonts"))
        {
          obj = ObjectOperation.WaitForCompletion() as T;
          this.loadInProgress.Remove(Path);
          if ((UnityEngine.Object) obj != (UnityEngine.Object) null)
            this.mAddressablesCache.Add(Path, ObjectOperation);
          Action<T> action = onLoaded;
          if (action != null)
            action(obj);
        }
        else
          ObjectOperation.Completed += (Action<AsyncOperationHandle<UnityEngine.Object>>) (opResult =>
          {
            if (opResult.Result != (UnityEngine.Object) null)
              this.mAddressablesCache.Add(Path, ObjectOperation);
            this.loadInProgress.Remove(Path);
          });
        return obj;
      }
      ObjectOperation.Completed += (Action<AsyncOperationHandle<UnityEngine.Object>>) (opResult =>
      {
        if (opResult.Result != (UnityEngine.Object) null)
          this.mAddressablesCache.Add(Path, ObjectOperation);
        this.loadInProgress.Remove(Path);
        Action<T> action = onLoaded;
        if (action == null)
          return;
        action(opResult.Result as T);
      });
      return default (T);
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("Unable to load {0} '{1}'\nERROR: {2}", (object) typeof (T), (object) Path, (object) ex.ToString());
      return default (T);
    }
  }

  public void UnloadAddressable(string Path)
  {
    try
    {
      if (string.IsNullOrEmpty(Path))
        return;
      Path = ResourceManager.ConvertPathToAddressable(Path);
      AsyncOperationHandle<UnityEngine.Object> handle;
      if (!this.mAddressablesCache.TryGetValue(Path, out handle) || !handle.IsValid())
        return;
      Addressables.Release<UnityEngine.Object>(handle);
      this.mAddressablesCache.Remove(Path);
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("Unable to unload '{0}'\nERROR: {1}", (object) Path, (object) ex.ToString());
    }
  }

  public static bool AssetExists(object key) => true;

  public static string ConvertPathToAddressable(string path)
  {
    string str = $"Assets/Resources_moved/{path}.asset";
    return string.IsNullOrEmpty(path) ? path : str;
  }
}
