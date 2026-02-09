// Decompiled with JetBrains decompiler
// Type: EasySpriteCollectionManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EasySpriteCollectionManager : MonoBehaviour
{
  public static bool _inited;
  public static EasySpriteCollectionManager _me;
  public Dictionary<EasySpriteCollectionManager.AtlasRequest, Action<UnityEngine.Object>> _reqs = new Dictionary<EasySpriteCollectionManager.AtlasRequest, Action<UnityEngine.Object>>();
  public float _time;
  public System.Action _on_all_loaded;

  public static EasySpriteCollectionManager me
  {
    get
    {
      if (!EasySpriteCollectionManager._inited)
      {
        EasySpriteCollectionManager._me = new GameObject("ESC Manager", new System.Type[1]
        {
          typeof (EasySpriteCollectionManager)
        }).GetComponent<EasySpriteCollectionManager>();
        EasySpriteCollectionManager._inited = true;
      }
      return EasySpriteCollectionManager._me;
    }
  }

  public static void StartTrackingResourceRequest(
    string name,
    ResourceRequest rq,
    Action<UnityEngine.Object> on_done)
  {
    EasySpriteCollectionManager.me._reqs.Add(new EasySpriteCollectionManager.AtlasRequest()
    {
      req = rq,
      name = name
    }, on_done);
  }

  public void Update()
  {
    this._time += Time.deltaTime;
    if (this._on_all_loaded != null && this._reqs.Count == 0)
    {
      System.Action onAllLoaded = this._on_all_loaded;
      this._on_all_loaded = (System.Action) null;
      onAllLoaded();
    }
    if ((double) this._time < 0.10000000149011612)
      return;
    this._time = 0.0f;
    List<EasySpriteCollectionManager.AtlasRequest> atlasRequestList = new List<EasySpriteCollectionManager.AtlasRequest>();
    foreach (KeyValuePair<EasySpriteCollectionManager.AtlasRequest, Action<UnityEngine.Object>> req1 in this._reqs)
    {
      ResourceRequest req2 = req1.Key.req;
      if (req2.isDone)
      {
        if (req2.asset == (UnityEngine.Object) null)
          Debug.LogError((object) ("Error loading sprite atlas: " + req1.Key.name));
        atlasRequestList.Add(req1.Key);
      }
    }
    foreach (EasySpriteCollectionManager.AtlasRequest key in atlasRequestList)
    {
      Action<UnityEngine.Object> req = this._reqs[key];
      this._reqs.Remove(key);
      if (req != null)
        req(key.req.asset);
      if (atlasRequestList.Count > 1)
      {
        this._time = 1f;
        break;
      }
    }
  }

  public static void EnsureAllAtlasesLoaded(System.Action on_loaded)
  {
    if (EasySpriteCollectionManager.me._reqs.Count == 0)
      on_loaded();
    else
      EasySpriteCollectionManager.me._on_all_loaded = on_loaded;
  }

  public struct AtlasRequest
  {
    public ResourceRequest req;
    public string name;
  }
}
