// Decompiled with JetBrains decompiler
// Type: ObjectDynamicShadowsManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ObjectDynamicShadowsManager : MonoBehaviour
{
  public static bool _inited;
  public static ObjectDynamicShadowsManager _me;
  public Dictionary<System.Action, System.Action> _queue = new Dictionary<System.Action, System.Action>();
  public int _queue_size;

  public static ObjectDynamicShadowsManager me
  {
    get
    {
      if (!ObjectDynamicShadowsManager._inited)
      {
        ObjectDynamicShadowsManager._me = new GameObject(nameof (ObjectDynamicShadowsManager), new System.Type[1]
        {
          typeof (ObjectDynamicShadowsManager)
        }).GetComponent<ObjectDynamicShadowsManager>();
        ObjectDynamicShadowsManager._inited = true;
      }
      return ObjectDynamicShadowsManager._me;
    }
  }

  public static void QueueShadowCreation(System.Action action, System.Action on_done)
  {
    ObjectDynamicShadowsManager.me._queue.Add(action, on_done);
  }

  public static void ForceShadowAction(System.Action action)
  {
    if (action == null)
      return;
    if (!ObjectDynamicShadowsManager.me._queue.ContainsKey(action))
    {
      Debug.LogWarning((object) "Trying to force absent action");
      action();
    }
    else
    {
      System.Action action1 = ObjectDynamicShadowsManager.me._queue[action];
      ObjectDynamicShadowsManager.me._queue.Remove(action);
      action();
      action1();
    }
  }

  public void Update()
  {
    this._queue_size = this._queue.Count;
    if (this._queue_size == 0)
      return;
    int num = this._queue_size > 4 ? 4 : this._queue_size;
    for (int index = 0; index < num; ++index)
    {
      KeyValuePair<System.Action, System.Action> keyValuePair = this._queue.First<KeyValuePair<System.Action, System.Action>>();
      this._queue.Remove(keyValuePair.Key);
      keyValuePair.Key();
      keyValuePair.Value();
    }
  }

  public static void TerminateQueue() => ObjectDynamicShadowsManager.me._queue.Clear();
}
