// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.WGOBehaviourAction
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

public class WGOBehaviourAction : ActionTask
{
  public WorldObjectPart _self_wgo_part;
  public WorldGameObject _self_wgo;
  public WorldGameObject _player_wgo;
  public BaseCharacterComponent _self_ch;
  public ProjectileEmitter _projectile_emitter;
  public bool _has_projectile_emitter;
  public bool _cached;

  public WorldGameObject self_wgo
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._self_wgo;
    }
  }

  public WorldGameObject player_wgo
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._player_wgo;
    }
  }

  public BaseCharacterComponent self_ch
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._self_ch;
    }
  }

  public ProjectileEmitter projectile_emitter
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._projectile_emitter;
    }
  }

  public bool has_projectile_emitter => this._has_projectile_emitter;

  public void Cache()
  {
    this._self_wgo_part = this.ownerAgent.GetComponent<WorldObjectPart>();
    this._self_wgo = this._self_wgo_part.parent;
    this._self_ch = (Object) this._self_wgo != (Object) null ? this._self_wgo.components.character : (BaseCharacterComponent) null;
    this._player_wgo = MainGame.me.player;
    this._projectile_emitter = this._self_wgo_part.GetComponentInChildren<ProjectileEmitter>();
    this._has_projectile_emitter = (Object) this._projectile_emitter != (Object) null;
    this._cached = (Object) this._self_wgo != (Object) null && (Object) this._player_wgo != (Object) null;
  }
}
