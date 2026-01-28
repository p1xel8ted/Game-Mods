// Decompiled with JetBrains decompiler
// Type: ObjectPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public sealed class ObjectPool : BaseMonoBehaviour
{
  public static ObjectPool _instance;
  public static List<GameObject> tempList = new List<GameObject>();
  public Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
  public Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();
  public ObjectPool.StartupPoolMode startupPoolMode;
  public ObjectPool.StartupPool[] startupPools;
  public bool startupPoolsCreated;
  public Dictionary<string, AsyncOperationHandle<GameObject>> loadedAddressables = new Dictionary<string, AsyncOperationHandle<GameObject>>();
  public Dictionary<string, GameObject> loadedFromResources = new Dictionary<string, GameObject>();

  public Dictionary<GameObject, List<GameObject>> GetPooledObjectsDictionary()
  {
    return this.pooledObjects;
  }

  public Dictionary<GameObject, GameObject> GetSpawnedObjectsDictionary() => this.spawnedObjects;

  public void Awake()
  {
    if ((UnityEngine.Object) ObjectPool._instance != (UnityEngine.Object) null)
    {
      Debug.Log((object) "OBJECT POOL: Dupe Awake");
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this);
    }
    else
    {
      Debug.Log((object) "OBJECT POOL: Awake");
      ObjectPool._instance = this;
      if (this.startupPoolMode != ObjectPool.StartupPoolMode.Awake)
        return;
      ObjectPool.CreateStartupPools();
    }
  }

  public void Start()
  {
    if (this.startupPoolMode != ObjectPool.StartupPoolMode.Start)
      return;
    ObjectPool.CreateStartupPools();
  }

  public void OnDestroy()
  {
  }

  public static void CreateStartupPools()
  {
    Debug.Log((object) "OBJECT POOL CreateStartupPools");
    if (ObjectPool.instance.startupPoolsCreated)
      return;
    ObjectPool.instance.startupPoolsCreated = true;
    ObjectPool.StartupPool[] startupPools = ObjectPool.instance.startupPools;
    if (startupPools == null || startupPools.Length == 0)
      return;
    for (int index = 0; index < startupPools.Length; ++index)
      ObjectPool.CreatePool(startupPools[index].prefab, startupPools[index].size);
  }

  public static IEnumerator PoolReset()
  {
    Debug.Log((object) "OBJECT POOL PoolReset");
    foreach (KeyValuePair<GameObject, List<GameObject>> pooledObject in ObjectPool.instance.pooledObjects)
    {
      ObjectPool.DestroyPooled(pooledObject.Key);
      Debug.Log((object) ("OBJECT POOL Clearing Pool:" + pooledObject.Key.name));
      yield return (object) new WaitForEndOfFrame();
    }
    yield return (object) new WaitForEndOfFrame();
    ObjectPool.instance.startupPoolsCreated = false;
    yield return (object) new WaitForEndOfFrame();
    ObjectPool.CreateStartupPools();
  }

  public static void CreatePool<T>(
    T prefab,
    int initialPoolSize,
    bool updateExisting = false,
    bool worldPositionStays = true)
    where T : Component
  {
    ObjectPool.CreatePool(prefab.gameObject, initialPoolSize, updateExisting, worldPositionStays);
  }

  public static void CreatePool(
    GameObject prefab,
    int initialPoolSize,
    bool updateExisting = false,
    bool worldPositionStays = true)
  {
    if (!((UnityEngine.Object) prefab != (UnityEngine.Object) null) || !(!ObjectPool.instance.pooledObjects.ContainsKey(prefab) | updateExisting))
      return;
    List<GameObject> gameObjectList = new List<GameObject>();
    if (updateExisting && ObjectPool.instance.pooledObjects.ContainsKey(prefab))
      gameObjectList = ObjectPool.instance.pooledObjects[prefab];
    if (initialPoolSize > 0)
    {
      int num = prefab.activeSelf ? 1 : 0;
      Transform transform = ObjectPool.instance.transform;
      while (gameObjectList.Count < initialPoolSize)
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
        gameObject.transform.SetParent(transform, worldPositionStays);
        gameObject.SetActive(false);
        gameObjectList.Add(gameObject);
      }
    }
    if (updateExisting && ObjectPool.instance.pooledObjects.ContainsKey(prefab))
      ObjectPool.instance.pooledObjects[prefab] = gameObjectList;
    else
      ObjectPool.instance.pooledObjects.Add(prefab, gameObjectList);
  }

  public static T SpawnUI<T>(
    T prefab,
    Transform parent,
    Vector3 position,
    Quaternion rotation,
    bool active = true,
    bool worldPositionStays = true)
    where T : Component
  {
    return ObjectPool.Spawn(prefab.gameObject, parent, position, rotation, active, worldPositionStays).GetComponent<T>();
  }

  public static GameObject SpawnUI(
    GameObject prefab,
    Transform parent,
    Vector3 position,
    Quaternion rotation,
    bool active = true,
    bool worldPositionStays = true)
  {
    return ObjectPool.Spawn(prefab.gameObject, parent, position, rotation, active, worldPositionStays);
  }

  public static T Spawn<T>(
    T prefab,
    Transform parent,
    Vector3 position,
    Quaternion rotation,
    bool active = true)
    where T : Component
  {
    return ObjectPool.Spawn(prefab.gameObject, parent, position, rotation, active).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
  {
    return ObjectPool.Spawn(prefab.gameObject, (Transform) null, position, rotation).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
  {
    return ObjectPool.Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab, Vector3 position) where T : Component
  {
    return ObjectPool.Spawn(prefab.gameObject, (Transform) null, position, Quaternion.identity).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab, Transform parent) where T : Component
  {
    return ObjectPool.Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab) where T : Component
  {
    return ObjectPool.Spawn(prefab.gameObject, (Transform) null, Vector3.zero, Quaternion.identity).GetComponent<T>();
  }

  public static bool HasPool<T>(T prefab) where T : Component
  {
    List<GameObject> gameObjectList;
    ObjectPool.instance.pooledObjects.TryGetValue(prefab.gameObject, out gameObjectList);
    return gameObjectList != null && gameObjectList.Count > 0;
  }

  public static GameObject Spawn(
    GameObject prefab,
    Transform parent,
    Vector3 position,
    Quaternion rotation,
    bool active = true,
    bool worldPositionStays = true)
  {
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return (GameObject) null;
    List<GameObject> gameObjectList;
    if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
    {
      GameObject key1 = (GameObject) null;
      if (gameObjectList.Count > 0)
      {
        for (int index = 0; index < gameObjectList.Count; ++index)
        {
          if ((UnityEngine.Object) gameObjectList[index] != (UnityEngine.Object) null)
          {
            key1 = gameObjectList[index];
            break;
          }
        }
        if ((UnityEngine.Object) key1 != (UnityEngine.Object) null)
        {
          Transform transform = key1.transform;
          transform.SetParent(parent, worldPositionStays);
          transform.localPosition = position;
          transform.localRotation = rotation;
          key1.SetActive(active);
          gameObjectList.Remove(key1);
          ObjectPool.instance.pooledObjects[prefab] = gameObjectList;
          ObjectPool.instance.spawnedObjects.Add(key1, prefab);
          return key1;
        }
      }
      GameObject key2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
      key2.SetActive(active);
      Transform transform1 = key2.transform;
      transform1.SetParent(parent, worldPositionStays);
      transform1.localPosition = position;
      transform1.localRotation = rotation;
      ObjectPool.instance.spawnedObjects.Add(key2, prefab);
      return key2;
    }
    GameObject key = UnityEngine.Object.Instantiate<GameObject>(prefab);
    key.SetActive(active);
    Transform component = key.GetComponent<Transform>();
    component.SetParent(parent, worldPositionStays);
    component.localPosition = position;
    component.localRotation = rotation;
    ObjectPool.instance.pooledObjects.Add(prefab, new List<GameObject>());
    ObjectPool.instance.spawnedObjects.Add(key, prefab);
    return key;
  }

  public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
  {
    return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
  }

  public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
  {
    return ObjectPool.Spawn(prefab, (Transform) null, position, rotation);
  }

  public static GameObject Spawn(GameObject prefab, Transform parent)
  {
    return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
  }

  public static GameObject Spawn(GameObject prefab, Vector3 position)
  {
    return ObjectPool.Spawn(prefab, (Transform) null, position, Quaternion.identity);
  }

  public static GameObject Spawn(GameObject prefab)
  {
    return ObjectPool.Spawn(prefab, (Transform) null, Vector3.zero, Quaternion.identity);
  }

  public static void Spawn(
    string path,
    Vector3 position,
    Quaternion rotation,
    Transform parent,
    Action<GameObject> callback,
    bool isAddressable = true)
  {
    if (string.IsNullOrEmpty(path))
      return;
    if (isAddressable)
    {
      if (!(bool) (UnityEngine.Object) GameManager.GetInstance())
        return;
      GameManager.GetInstance().StartCoroutine((IEnumerator) ObjectPool.SpawnAddressableIE(path, position, rotation, parent, callback));
    }
    else
      ObjectPool.SpawnFromResources(path, position, rotation, parent, callback);
  }

  public static void SpawnFromResources(
    string path,
    Vector3 position,
    Quaternion rotation,
    Transform parent,
    Action<GameObject> callback)
  {
    if (ObjectPool.instance.loadedFromResources.ContainsKey(path))
    {
      if ((UnityEngine.Object) ObjectPool.instance.loadedFromResources[path] != (UnityEngine.Object) null)
      {
        if (callback == null)
          return;
        callback(ObjectPool.Spawn(ObjectPool.instance.loadedFromResources[path], parent, position, rotation));
      }
      else
        Debug.Log((object) ("OBJECT POOL, failed resource load: " + path).Colour(Color.red));
    }
    else
    {
      GameObject prefab = Resources.Load<GameObject>(path);
      ObjectPool.instance.loadedFromResources.Add(path, prefab);
      if (callback == null)
        return;
      callback(ObjectPool.Spawn(prefab, parent, position, rotation));
    }
  }

  public static IEnumerator SpawnAddressableIE(
    string path,
    Vector3 position,
    Quaternion rotation,
    Transform parent,
    Action<GameObject> callback)
  {
    if (ObjectPool.instance.loadedAddressables.ContainsKey(path))
    {
      AsyncOperationHandle<GameObject> loadedAddressable;
      while (true)
      {
        loadedAddressable = ObjectPool.instance.loadedAddressables[path];
        if ((UnityEngine.Object) loadedAddressable.Result == (UnityEngine.Object) null)
          yield return (object) null;
        else
          break;
      }
      loadedAddressable = ObjectPool.instance.loadedAddressables[path];
      if (loadedAddressable.Status != AsyncOperationStatus.Failed)
      {
        loadedAddressable = ObjectPool.instance.loadedAddressables[path];
        GameObject gameObject = ObjectPool.Spawn(loadedAddressable.Result, parent, position, rotation);
        Action<GameObject> action = callback;
        if (action != null)
          action(gameObject);
      }
      else
        Debug.Log((object) ("OBJECT POOL, failed addressable: " + path).Colour(Color.red));
    }
    else
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) path);
      ObjectPool.instance.loadedAddressables.Add(path, asyncOperationHandle);
      asyncOperationHandle.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        ObjectPool.instance.loadedAddressables[path] = obj;
        Action<GameObject> action = callback;
        if (action == null)
          return;
        action(ObjectPool.Spawn(obj.Result, parent, position, rotation));
      });
    }
  }

  public static void Recycle<T>(T obj) where T : Component
  {
    if (!((UnityEngine.Object) obj != (UnityEngine.Object) null) || !((UnityEngine.Object) obj.gameObject != (UnityEngine.Object) null))
      return;
    ObjectPool.Recycle(obj.gameObject);
  }

  public static void Recycle(GameObject obj)
  {
    GameObject prefab;
    if (ObjectPool.instance.spawnedObjects.TryGetValue(obj, out prefab))
      ObjectPool.Recycle(obj, prefab);
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) obj);
  }

  public static void Recycle(GameObject obj, GameObject prefab)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    if (!ObjectPool.instance.pooledObjects.ContainsKey(prefab))
      return;
    List<GameObject> pooledObject = ObjectPool.instance.pooledObjects[prefab];
    pooledObject.Add(obj);
    ObjectPool.instance.spawnedObjects.Remove(obj);
    ObjectPool.instance.pooledObjects[prefab] = pooledObject;
    obj.transform.SetParent(ObjectPool.instance.transform);
    obj.SetActive(false);
    IPoolListener component;
    if (!obj.TryGetComponent<IPoolListener>(out component))
      return;
    component.OnRecycled();
  }

  public static void RecycleAll<T>(T prefab) where T : Component
  {
    ObjectPool.RecycleAll(prefab.gameObject);
  }

  public static void RecycleAll(GameObject prefab)
  {
    foreach (KeyValuePair<GameObject, GameObject> spawnedObject in ObjectPool.instance.spawnedObjects)
    {
      if ((UnityEngine.Object) spawnedObject.Value == (UnityEngine.Object) prefab)
        ObjectPool.tempList.Add(spawnedObject.Key);
    }
    for (int index = 0; index < ObjectPool.tempList.Count; ++index)
      ObjectPool.Recycle(ObjectPool.tempList[index]);
    ObjectPool.tempList.Clear();
  }

  public static void RecycleAll()
  {
    ObjectPool.tempList.AddRange((IEnumerable<GameObject>) ObjectPool.instance.spawnedObjects.Keys);
    for (int index = 0; index < ObjectPool.tempList.Count; ++index)
    {
      if ((UnityEngine.Object) ObjectPool.tempList[index] != (UnityEngine.Object) null && (UnityEngine.Object) ObjectPool.tempList[index].gameObject != (UnityEngine.Object) null)
        ObjectPool.Recycle(ObjectPool.tempList[index]);
    }
    ObjectPool.tempList.Clear();
    Debug.Log((object) ("OBJECT POOL RecycleAll Remaining spawnedObjects Count " + ObjectPool.instance.spawnedObjects.Count.ToString()));
    ObjectPool.instance.spawnedObjects.Clear();
  }

  public static bool IsSpawned(GameObject obj)
  {
    return (UnityEngine.Object) ObjectPool.instance != (UnityEngine.Object) null && ObjectPool.instance.spawnedObjects != null && ObjectPool.instance.spawnedObjects.ContainsKey(obj);
  }

  public static int CountPooled<T>(T prefab) where T : Component
  {
    return ObjectPool.CountPooled(prefab.gameObject);
  }

  public static int CountPooled(GameObject prefab)
  {
    List<GameObject> gameObjectList;
    return ObjectPool.instance.pooledObjects.TryGetValue(prefab, out gameObjectList) ? gameObjectList.Count : 0;
  }

  public static int CountSpawned<T>(T prefab) where T : Component
  {
    return ObjectPool.CountSpawned(prefab.gameObject);
  }

  public static int CountSpawned(GameObject prefab)
  {
    int num = 0;
    foreach (GameObject gameObject in ObjectPool.instance.spawnedObjects.Values)
    {
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) gameObject)
        ++num;
    }
    return num;
  }

  public static int CountAllSpawned()
  {
    return (UnityEngine.Object) ObjectPool.instance == (UnityEngine.Object) null || ObjectPool.instance.spawnedObjects == null ? 0 : ObjectPool.instance.spawnedObjects.Count;
  }

  public static int CountAllPooled()
  {
    if ((UnityEngine.Object) ObjectPool.instance == (UnityEngine.Object) null || ObjectPool.instance.pooledObjects == null)
      return 0;
    int num = 0;
    foreach (List<GameObject> gameObjectList in ObjectPool.instance.pooledObjects.Values)
      num += gameObjectList.Count;
    return num;
  }

  public static List<GameObject> GetPooled(
    GameObject prefab,
    List<GameObject> list,
    bool appendList)
  {
    if (list == null)
      list = new List<GameObject>();
    if (!appendList)
      list.Clear();
    List<GameObject> collection;
    if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out collection))
      list.AddRange((IEnumerable<GameObject>) collection);
    return list;
  }

  public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
  {
    if (list == null)
      list = new List<T>();
    if (!appendList)
      list.Clear();
    List<GameObject> gameObjectList;
    if (ObjectPool.instance.pooledObjects.TryGetValue(prefab.gameObject, out gameObjectList))
    {
      for (int index = 0; index < gameObjectList.Count; ++index)
        list.Add(gameObjectList[index].GetComponent<T>());
    }
    return list;
  }

  public static List<GameObject> GetSpawned(
    GameObject prefab,
    List<GameObject> list,
    bool appendList)
  {
    if (list == null)
      list = new List<GameObject>();
    if (!appendList)
      list.Clear();
    foreach (KeyValuePair<GameObject, GameObject> spawnedObject in ObjectPool.instance.spawnedObjects)
    {
      if ((UnityEngine.Object) spawnedObject.Value == (UnityEngine.Object) prefab)
        list.Add(spawnedObject.Key);
    }
    return list;
  }

  public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
  {
    if (list == null)
      list = new List<T>();
    if (!appendList)
      list.Clear();
    GameObject gameObject = prefab.gameObject;
    foreach (KeyValuePair<GameObject, GameObject> spawnedObject in ObjectPool.instance.spawnedObjects)
    {
      if ((UnityEngine.Object) spawnedObject.Value == (UnityEngine.Object) gameObject)
        list.Add(spawnedObject.Key.GetComponent<T>());
    }
    return list;
  }

  public static void DestroyPooled(GameObject prefab)
  {
    List<GameObject> gameObjectList;
    if (!ObjectPool.instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
      return;
    for (int index = 0; index < gameObjectList.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObjectList[index]);
    gameObjectList.Clear();
  }

  public static void DestroyPooled<T>(T prefab) where T : Component
  {
    ObjectPool.DestroyPooled(prefab.gameObject);
  }

  public static void DestroyAll(GameObject prefab)
  {
    ObjectPool.RecycleAll(prefab);
    ObjectPool.DestroyPooled(prefab);
  }

  public static void DestroyAll<T>(T prefab) where T : Component
  {
    ObjectPool.DestroyAll(prefab.gameObject);
  }

  public static void DestroyAll()
  {
    Debug.Log((object) "OBJECT POOL DestroyAll");
    ObjectPool.RecycleAll();
    ObjectPool.instance.StopAllCoroutines();
    for (int index = ObjectPool.instance.transform.childCount - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) ObjectPool.instance.transform.GetChild(index) != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) ObjectPool.instance.transform.GetChild(index).gameObject);
    }
    Debug.Log((object) ("OBJECT POOL Release Addressables" + ObjectPool.instance.loadedAddressables.Count.ToString()));
    foreach (AsyncOperationHandle<GameObject> handle in ObjectPool.instance.loadedAddressables.Values)
    {
      if ((UnityEngine.Object) handle.Result != (UnityEngine.Object) null)
        Addressables.Release<GameObject>(handle);
    }
    ObjectPool.instance.loadedFromResources.Clear();
    ObjectPool.instance.loadedAddressables.Clear();
    ObjectPool.instance.pooledObjects.Clear();
    ObjectPool.tempList.Clear();
    Projectile.UnloadResources();
    SimpleSpineDeactivateAfterPlay.UnloadResources();
    ProjectileGhost.Unload();
    Projectile.CleanCache();
    EnemySpawner.Unload();
    FleshEgg.CleanCache();
    SpriteAtlasLoader.ReleaseAtlases();
    if (!((UnityEngine.Object) FollowersNameManager.Instance != (UnityEngine.Object) null))
      return;
    FollowersNameManager.Instance.Unload();
  }

  public static ObjectPool instance
  {
    get
    {
      if ((UnityEngine.Object) ObjectPool._instance == (UnityEngine.Object) null)
      {
        GameObject target = new GameObject(nameof (ObjectPool));
        target.transform.localPosition = Vector3.zero;
        target.transform.localRotation = Quaternion.identity;
        target.transform.localScale = Vector3.one;
        target.AddComponent<ObjectPool>();
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
      }
      return ObjectPool._instance;
    }
  }

  public enum StartupPoolMode
  {
    Awake,
    Start,
    CallManually,
  }

  [Serializable]
  public class StartupPool
  {
    public int size;
    public GameObject prefab;
  }
}
