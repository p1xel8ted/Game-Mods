// Decompiled with JetBrains decompiler
// Type: SmartPools.Pool
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace SmartPools;

public class Pool
{
  public Stack<MonoBehaviour> members = new Stack<MonoBehaviour>();
  public MonoBehaviour prefab;
  public GameObject pool_go;
  public int max_pool_size;
  public Vector3 prefab_local_scale;
  public int _cur_pool_size;
  public int max_objects_per_frame = 5;
  public bool paused;
  public bool activate_on_creation = true;

  public void AddObjectToPool()
  {
    MonoBehaviour monoBehaviour = Object.Instantiate<MonoBehaviour>(this.prefab);
    this.members.Push(monoBehaviour);
    monoBehaviour.transform.parent = this.pool_go.transform;
    monoBehaviour.gameObject.SetActive(false);
  }

  public void DestroyObject<T>(T obj) where T : MonoBehaviour
  {
    this.members.Push((MonoBehaviour) obj);
    obj.gameObject.SetActive(false);
  }

  public T CreateObject<T>() where T : MonoBehaviour
  {
    if (this.members.Count == 0)
      this.AddObjectToPool();
    MonoBehaviour monoBehaviour = this.members.Pop();
    monoBehaviour.transform.localScale = this.prefab_local_scale;
    if (this.activate_on_creation)
      monoBehaviour.gameObject.SetActive(true);
    return monoBehaviour as T;
  }

  public void Update()
  {
    this._cur_pool_size = this.members.Count;
    if (this._cur_pool_size >= this.max_pool_size)
      return;
    for (int index = 0; index < this.max_objects_per_frame; ++index)
      this.AddObjectToPool();
  }

  public void ConfigurePool(int object_per_frame, bool activate_on_creation)
  {
    this.activate_on_creation = activate_on_creation;
    this.max_objects_per_frame = object_per_frame;
  }
}
