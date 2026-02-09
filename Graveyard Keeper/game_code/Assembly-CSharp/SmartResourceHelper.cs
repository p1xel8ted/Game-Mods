// Decompiled with JetBrains decompiler
// Type: SmartResourceHelper
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SmartResourceHelper : MonoBehaviour
{
  public static bool _inited;
  public static SmartResourceHelper _me;
  public float _time;
  public bool _calculating_time;
  public float _start_time;
  public int max_simultaneous_loading_files = 4;
  public Dictionary<System.Type, SmartResourceHelperPool> _pools = new Dictionary<System.Type, SmartResourceHelperPool>();
  public Dictionary<string, SmartResourceHelperPool> queue = new Dictionary<string, SmartResourceHelperPool>();
  public Dictionary<string, SmartResourceHelper.AssetRequest> loading = new Dictionary<string, SmartResourceHelper.AssetRequest>();

  public static SmartResourceHelper me
  {
    get
    {
      if (!SmartResourceHelper._inited)
      {
        SmartResourceHelper._me = new GameObject(nameof (SmartResourceHelper), new System.Type[1]
        {
          typeof (SmartResourceHelper)
        }).GetComponent<SmartResourceHelper>();
        SmartResourceHelper._inited = true;
      }
      return SmartResourceHelper._me;
    }
  }

  public SmartResourceHelperPool GetOrCreatePool<T>()
  {
    System.Type type = typeof (T);
    SmartResourceHelperPool pool1;
    if (this._pools.TryGetValue(type, out pool1))
      return pool1;
    SmartResourceHelperPool pool2 = new SmartResourceHelperPool(type);
    this._pools.Add(type, pool2);
    return pool2;
  }

  public void LoadAsync<T>(string res_name)
  {
    SmartResourceHelperPool pool = this.GetOrCreatePool<T>();
    if (this.loading.ContainsKey(res_name) || this.queue.ContainsKey(res_name) || pool.loaded.ContainsKey(res_name))
      return;
    this.queue.Add(res_name, pool);
    this.UpdateLoadingQueue();
  }

  public void LoadListAsync<T>(List<string> resources)
  {
    foreach (string resource in resources)
      this.LoadAsync<T>(resource);
  }

  public void Update()
  {
    this._time += Time.deltaTime;
    if ((double) this._time < 0.029999999329447746)
      return;
    this._time = 0.0f;
    this.UpdateLoadingQueue();
    if (this.loading.Count == 0)
      return;
    List<string> list = this.loading.Keys.ToList<string>();
    int index = 0;
    bool flag = false;
    while (index < list.Count)
    {
      string key = list[index];
      SmartResourceHelper.AssetRequest assetRequest = this.loading[key];
      if (assetRequest.req.isDone)
      {
        if (assetRequest.req.asset == (UnityEngine.Object) null)
          Debug.LogWarning((object) $"Error loading asset \"{key}\"");
        if (!assetRequest.pool.loaded.ContainsKey(key))
          assetRequest.pool.loaded.Add(key, assetRequest.req.asset);
        list.RemoveAt(index);
        this.loading.Remove(key);
        flag = true;
      }
      else
        ++index;
    }
    if (!flag)
      return;
    this.UpdateLoadingQueue();
  }

  public void UpdateLoadingQueue()
  {
    while (this.loading.Count < this.max_simultaneous_loading_files && this.queue.Count != 0)
    {
      KeyValuePair<string, SmartResourceHelperPool> keyValuePair = this.queue.First<KeyValuePair<string, SmartResourceHelperPool>>();
      this.queue.Remove(keyValuePair.Key);
      this.loading.Add(keyValuePair.Key, new SmartResourceHelper.AssetRequest()
      {
        req = Resources.LoadAsync(keyValuePair.Key),
        pool = keyValuePair.Value
      });
    }
  }

  public static T GetResource<T>(string res_name) where T : MonoBehaviour
  {
    UnityEngine.Object @object = SmartResourceHelper.me.GetOrCreatePool<T>().GetObject(res_name);
    GameObject gameObject = (GameObject) @object;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return default (T);
    T component = gameObject.GetComponent<T>();
    if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
      return component;
    Debug.Log((object) $"GetResource is null for res: {res_name}, o = {(@object == (UnityEngine.Object) null ? "null" : @object.ToString())}");
    return component;
  }

  public static T GetResourceAs<T>(string res_name) where T : UnityEngine.Object
  {
    UnityEngine.Object resourceAs = SmartResourceHelper.me.GetOrCreatePool<T>().GetObject(res_name);
    if (resourceAs == (UnityEngine.Object) null)
      Debug.Log((object) ("GetResource is null for res: " + res_name));
    return resourceAs as T;
  }

  public struct AssetRequest
  {
    public ResourceRequest req;
    public SmartResourceHelperPool pool;
  }
}
