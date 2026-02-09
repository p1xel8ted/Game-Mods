// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.WGOBehaviourCondition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

public class WGOBehaviourCondition : ConditionTask
{
  public WorldObjectPart _self_wgo_part;
  public WorldGameObject _self_wgo;
  public BaseCharacterComponent _self_ch;
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

  public WorldGameObject player_wgo => MainGame.me.player;

  public BaseCharacterComponent self_ch
  {
    get
    {
      if (!this._cached)
        this.Cache();
      return this._self_ch;
    }
  }

  public void Cache()
  {
    this._self_wgo_part = this.ownerAgent.GetComponent<WorldObjectPart>();
    this._self_wgo = this._self_wgo_part.parent;
    this._self_ch = (Object) this._self_wgo != (Object) null ? this._self_wgo.components.character : (BaseCharacterComponent) null;
    this._cached = (Object) this._self_wgo != (Object) null && (Object) this.player_wgo != (Object) null;
  }
}
