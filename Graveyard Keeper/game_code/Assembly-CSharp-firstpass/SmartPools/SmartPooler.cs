// Decompiled with JetBrains decompiler
// Type: SmartPools.SmartPooler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace SmartPools;

public class SmartPooler : MonoBehaviour
{
  public Dictionary<System.Type, Pool> _pools = new Dictionary<System.Type, Pool>();
  public static bool _inited;
  public static SmartPooler _me;

  public static SmartPooler me
  {
    get
    {
      if (!SmartPooler._inited)
      {
        SmartPooler._inited = true;
        SmartPooler._me = new GameObject("Smart Pooler").AddComponent<SmartPooler>();
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) SmartPooler._me);
      }
      return SmartPooler._me;
    }
  }

  public static Pool CreatePool<T>(T prefab, int pool_size = 0) where T : MonoBehaviour
  {
    System.Type key = typeof (T);
    if (SmartPooler.me._pools.ContainsKey(key))
    {
      Debug.LogError((object) ("Can't create a second pool of: " + key?.ToString()));
      return (Pool) null;
    }
    Debug.Log((object) $"Creating pool of objects {key?.ToString()}, n = {pool_size.ToString()}");
    Pool pool = new Pool()
    {
      prefab = (MonoBehaviour) prefab,
      pool_go = new GameObject(key.ToString()),
      max_pool_size = pool_size,
      prefab_local_scale = prefab.transform.localScale
    };
    pool.pool_go.transform.parent = SmartPooler.me.transform;
    SmartPooler.me._pools.Add(key, pool);
    return pool;
  }

  public static Pool GetPoolByType<T>() where T : MonoBehaviour
  {
    Pool poolByType;
    if (SmartPooler.me._pools.TryGetValue(typeof (T), out poolByType))
      return poolByType;
    Debug.LogError((object) $"Pooler for object {typeof (T)?.ToString()} was not created");
    return (Pool) null;
  }

  public static T CreateObject<T>() where T : MonoBehaviour
  {
    Pool poolByType = SmartPooler.GetPoolByType<T>();
    return poolByType != null ? poolByType.CreateObject<T>() : default (T);
  }

  public static void DestroyObject<T>(T obj) where T : MonoBehaviour
  {
    SmartPooler.GetPoolByType<T>()?.DestroyObject<T>(obj);
  }

  public void Update()
  {
    foreach (KeyValuePair<System.Type, Pool> pool in this._pools)
    {
      if (!pool.Value.paused)
        pool.Value.Update();
    }
  }

  public static void PausePool<T>() where T : MonoBehaviour
  {
    Pool poolByType = SmartPooler.GetPoolByType<T>();
    if (poolByType == null)
      return;
    poolByType.paused = true;
  }

  public static void ResumePool<T>() where T : MonoBehaviour
  {
    Pool poolByType = SmartPooler.GetPoolByType<T>();
    if (poolByType == null)
      return;
    poolByType.paused = false;
  }
}
