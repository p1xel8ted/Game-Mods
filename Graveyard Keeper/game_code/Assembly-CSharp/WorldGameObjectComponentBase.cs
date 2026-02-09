// Decompiled with JetBrains decompiler
// Type: WorldGameObjectComponentBase
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class WorldGameObjectComponentBase
{
  public bool _enabled;
  public Transform _tf;
  public WorldGameObject _wgo;
  public GameObject _go;
  public bool _cached_tf;
  public long _unique_id = -1;
  public string _type_name = "";
  public bool _type_name_set;
  public bool started;

  public bool enabled
  {
    get => this._enabled;
    set
    {
      if (this._enabled == value)
        return;
      this._enabled = value;
      if (this._enabled)
        this.OnEnabled();
      else
        this.OnDisabled();
    }
  }

  public virtual void Init(WorldGameObject wobj)
  {
    this._cached_tf = (Object) this._wgo != (Object) null && (Object) this._tf != (Object) null;
    if (this._cached_tf)
      return;
    this._wgo = wobj;
    if ((Object) this._wgo == (Object) null)
    {
      this._cached_tf = false;
      this._tf = (Transform) null;
    }
    else
    {
      this._go = wobj.gameObject;
      this._cached_tf = true;
      this._tf = this._wgo.transform;
    }
    this.enabled = true;
  }

  public virtual void OnEnabled()
  {
  }

  public virtual void OnDisabled()
  {
  }

  public void ForceSetGameObjectLinks(WorldGameObject wgo, GameObject go = null)
  {
    this._wgo = wgo;
    this._go = (Object) go == (Object) null ? wgo.gameObject : go;
  }

  public WorldGameObjectComponentBase() => this._unique_id = UniqueID.GetUniqueID();

  public long GetInstanceID() => this._unique_id;

  public ComponentsManager components => this._wgo.components;

  public WorldGameObject wgo => this._wgo;

  public GameObject go => this._go;

  public Transform tf
  {
    get
    {
      if (!this._cached_tf)
      {
        this._tf = this._go.transform;
        this._cached_tf = true;
      }
      return this._tf;
    }
  }

  public void SetTf(Transform tf)
  {
    this._tf = tf;
    this._cached_tf = true;
  }

  public string GetTypeName()
  {
    if (!this._type_name_set)
    {
      this._type_name_set = true;
      this._type_name = this.GetType().Name;
    }
    return this._type_name;
  }

  public void BeginProfilerSample()
  {
  }
}
