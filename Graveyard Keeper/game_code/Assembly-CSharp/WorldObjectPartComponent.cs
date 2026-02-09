// Decompiled with JetBrains decompiler
// Type: WorldObjectPartComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WorldObjectPartComponent : MonoBehaviour
{
  public WorldGameObject _obj;
  public WorldObjectPart _part;
  public ComponentsManager _components;
  public CustomNetworkAnimatorSync _animator;
  public BaseCharacterComponent _character;
  public bool _cached;

  public WorldGameObject wgo
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._obj;
    }
  }

  public WorldObjectPart part
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._part;
    }
  }

  public ComponentsManager components
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._components;
    }
  }

  public CustomNetworkAnimatorSync animator
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._animator;
    }
  }

  public BaseCharacterComponent character
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._character;
    }
  }

  public void Cache()
  {
    this._part = this.GetComponent<WorldObjectPart>();
    this._obj = this._part.parent;
    if ((UnityEngine.Object) this._obj == (UnityEngine.Object) null)
    {
      this._cached = false;
      Debug.Log((object) ("Part has no WGO, is null = " + ((UnityEngine.Object) this.GetComponentInParent<WorldGameObject>() == (UnityEngine.Object) null).ToString()), (UnityEngine.Object) this);
      throw new Exception();
    }
    this._components = this._obj.components;
    this._animator = this._components.animator;
    this._character = this._components.character;
    this._cached = true;
  }

  public virtual void StartComponent() => this.Cache();

  public virtual void UpdateComponent(float delta_time)
  {
  }

  public void ForceRecache() => this._cached = false;
}
