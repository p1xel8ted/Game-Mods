// Decompiled with JetBrains decompiler
// Type: WorldGameObjectComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class WorldGameObjectComponent : 
  WorldGameObjectComponentBase,
  IComparable<WorldGameObjectComponent>
{
  [NonSerialized]
  public int _update_every_frame = 1;
  [NonSerialized]
  public bool destroyed;
  public float _update_delay;
  public float _left_update_delay;
  public static Dictionary<System.Type, object> _lists = new Dictionary<System.Type, object>();
  public Rigidbody2D _body;
  public bool _body_cached;
  public CustomNetworkAnimatorSync _animator;
  public bool _animator_cached;

  public Rigidbody2D body
  {
    get
    {
      if (this._body_cached)
        return this._body;
      this._body_cached = true;
      this._body = this.go.GetComponent<Rigidbody2D>();
      return this._body;
    }
  }

  public virtual void StartComponent()
  {
    this._update_delay = this._update_every_frame <= 1 ? 0.0f : (float) this._update_every_frame / 60f;
    this.started = true;
  }

  public virtual void LateUpdateComponent()
  {
  }

  public virtual void UpdateComponent(float delta_time)
  {
  }

  public virtual void FixedUpdateComponent(float delta_time)
  {
  }

  public virtual bool HasFixedUpdate() => false;

  public virtual bool HasUpdate() => false;

  public virtual bool HasLateUpdate() => false;

  public virtual bool DoAction(
    WorldGameObject other_obj,
    float delta_time,
    bool for_gratitude_points = false)
  {
    return false;
  }

  public virtual bool Interact(WorldGameObject other_obj, float delta_time) => false;

  public virtual void PrepareForInteraction(BaseCharacterComponent for_whom)
  {
  }

  public virtual void UnprepareForInteraction()
  {
  }

  public bool DelayedUpdate(float delta_time)
  {
    if (this._update_every_frame <= 1)
      return false;
    this._left_update_delay -= delta_time;
    if ((double) this._left_update_delay > 0.0)
      return true;
    this._left_update_delay = this._update_delay;
    return false;
  }

  public int CompareTo(WorldGameObjectComponent other)
  {
    if (this.GetExecutionOrder() == other.GetExecutionOrder())
      return 0;
    return this.GetExecutionOrder() <= other.GetExecutionOrder() ? -1 : 1;
  }

  public virtual void InitComponent()
  {
  }

  public virtual int GetExecutionOrder() => 0;

  public WorldGameObject FindObjectForInteraction() => this.components.interaction.nearest;

  public void RedrawCurrentInteractiveHint()
  {
    if (!MainGame.game_started)
      return;
    Debug.Log((object) nameof (RedrawCurrentInteractiveHint));
    WorldGameObject objectForInteraction = this.FindObjectForInteraction();
    if (!((UnityEngine.Object) objectForInteraction != (UnityEngine.Object) null))
      return;
    objectForInteraction.RedrawBubble();
  }

  public virtual void OnEnable()
  {
  }

  public virtual void OnDisable()
  {
  }

  public virtual void UpdateEnableState(ObjectDefinition.ObjType obj_type) => this.enabled = true;

  public virtual void RefreshComponentBubbleData(bool show_interaction_buttons)
  {
  }
}
