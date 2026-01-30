// Decompiled with JetBrains decompiler
// Type: ObjectPoolExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class ObjectPoolExtensions
{
  public static void CreatePool<T>(this T prefab) where T : Component
  {
    ObjectPool.CreatePool<T>(prefab, 0);
  }

  public static void CreatePoolUI<T>(this T prefab, int initialPoolSize, bool worldPositionStays) where T : Component
  {
    ObjectPool.CreatePool<T>(prefab, initialPoolSize, worldPositionStays: worldPositionStays);
  }

  public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
  {
    ObjectPool.CreatePool<T>(prefab, initialPoolSize);
  }

  public static void CreatePool(this GameObject prefab) => ObjectPool.CreatePool(prefab, 0);

  public static void CreatePool(this GameObject prefab, int initialPoolSize)
  {
    ObjectPool.CreatePool(prefab, initialPoolSize);
  }

  public static void CreatePool(this GameObject prefab, int initialPoolSize, bool updateExisting)
  {
    ObjectPool.CreatePool(prefab, initialPoolSize, updateExisting);
  }

  public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
  {
    return ObjectPool.Spawn<T>(prefab, parent, position, rotation);
  }

  public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
  {
    return ObjectPool.Spawn<T>(prefab, (Transform) null, position, rotation);
  }

  public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
  {
    return ObjectPool.Spawn<T>(prefab, parent, position, Quaternion.identity);
  }

  public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
  {
    return ObjectPool.Spawn<T>(prefab, (Transform) null, position, Quaternion.identity);
  }

  public static T Spawn<T>(this T prefab, Transform parent, bool active = true) where T : Component
  {
    return ObjectPool.Spawn<T>(prefab, parent, Vector3.zero, Quaternion.identity, active);
  }

  public static T SpawnUI<T>(this T prefab, Transform parent, bool worldPostionStays = true) where T : Component
  {
    return ObjectPool.SpawnUI<T>(prefab, parent, Vector3.zero, Quaternion.identity, worldPositionStays: worldPostionStays);
  }

  public static GameObject SpawnUI(
    this GameObject prefab,
    Transform parent,
    bool worldPostionStays = true)
  {
    return ObjectPool.SpawnUI(prefab, parent, Vector3.zero, Quaternion.identity, worldPositionStays: worldPostionStays);
  }

  public static T Spawn<T>(this T prefab) where T : Component
  {
    return ObjectPool.Spawn<T>(prefab, (Transform) null, Vector3.zero, Quaternion.identity);
  }

  public static GameObject Spawn(
    this GameObject prefab,
    Transform parent,
    Vector3 position,
    Quaternion rotation)
  {
    return ObjectPool.Spawn(prefab, parent, position, rotation);
  }

  public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
  {
    return ObjectPool.Spawn(prefab, (Transform) null, position, rotation);
  }

  public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
  {
    return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
  }

  public static GameObject Spawn(this GameObject prefab, Vector3 position)
  {
    return ObjectPool.Spawn(prefab, (Transform) null, position, Quaternion.identity);
  }

  public static GameObject Spawn(this GameObject prefab, Transform parent)
  {
    return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
  }

  public static GameObject Spawn(this GameObject prefab)
  {
    return ObjectPool.Spawn(prefab, (Transform) null, Vector3.zero, Quaternion.identity);
  }

  public static void Recycle<T>(this T obj) where T : Component => ObjectPool.Recycle<T>(obj);

  public static void Recycle(this GameObject obj) => ObjectPool.Recycle(obj);

  public static void RecycleAll<T>(this T prefab) where T : Component
  {
    ObjectPool.RecycleAll<T>(prefab);
  }

  public static void RecycleAll(this GameObject prefab) => ObjectPool.RecycleAll(prefab);

  public static int CountPooled<T>(this T prefab) where T : Component
  {
    return ObjectPool.CountPooled<T>(prefab);
  }

  public static int CountPooled(this GameObject prefab) => ObjectPool.CountPooled(prefab);

  public static int CountSpawned<T>(this T prefab) where T : Component
  {
    return ObjectPool.CountSpawned<T>(prefab);
  }

  public static int CountSpawned(this GameObject prefab) => ObjectPool.CountSpawned(prefab);

  public static List<GameObject> GetSpawned(
    this GameObject prefab,
    List<GameObject> list,
    bool appendList)
  {
    return ObjectPool.GetSpawned(prefab, list, appendList);
  }

  public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
  {
    return ObjectPool.GetSpawned(prefab, list, false);
  }

  public static List<GameObject> GetSpawned(this GameObject prefab)
  {
    return ObjectPool.GetSpawned(prefab, (List<GameObject>) null, false);
  }

  public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
  {
    return ObjectPool.GetSpawned<T>(prefab, list, appendList);
  }

  public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
  {
    return ObjectPool.GetSpawned<T>(prefab, list, false);
  }

  public static List<T> GetSpawned<T>(this T prefab) where T : Component
  {
    return ObjectPool.GetSpawned<T>(prefab, (List<T>) null, false);
  }

  public static List<GameObject> GetPooled(
    this GameObject prefab,
    List<GameObject> list,
    bool appendList)
  {
    return ObjectPool.GetPooled(prefab, list, appendList);
  }

  public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
  {
    return ObjectPool.GetPooled(prefab, list, false);
  }

  public static List<GameObject> GetPooled(this GameObject prefab)
  {
    return ObjectPool.GetPooled(prefab, (List<GameObject>) null, false);
  }

  public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
  {
    return ObjectPool.GetPooled<T>(prefab, list, appendList);
  }

  public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
  {
    return ObjectPool.GetPooled<T>(prefab, list, false);
  }

  public static List<T> GetPooled<T>(this T prefab) where T : Component
  {
    return ObjectPool.GetPooled<T>(prefab, (List<T>) null, false);
  }

  public static void DestroyPooled(this GameObject prefab) => ObjectPool.DestroyPooled(prefab);

  public static void DestroyPooled<T>(this T prefab) where T : Component
  {
    ObjectPool.DestroyPooled(prefab.gameObject);
  }

  public static void DestroyAll(this GameObject prefab) => ObjectPool.DestroyAll(prefab);

  public static void DestroyAll<T>(this T prefab) where T : Component
  {
    ObjectPool.DestroyAll(prefab.gameObject);
  }
}
