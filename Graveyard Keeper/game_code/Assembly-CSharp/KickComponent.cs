// Decompiled with JetBrains decompiler
// Type: KickComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KickComponent : WorldGameObjectComponent
{
  public const float VERTICAL_K = 0.8f;
  public const float K = 96f;
  public const float MIN_DELTA = 0.01f;
  public const float RADIUS = 0.14f;
  public const float KICK_DIST = 0.4f;
  public const float KICK_FACTOR = 1f;
  public const float DROP_KICK_SPEED = 1f;
  public const float DROP_KICK_FRICTION = 0.96f;
  public const int WALLS_MASK = 1;
  public KickComponent.KickType _type = KickComponent.KickType.Kickable;
  [RuntimeValue]
  public bool active = true;
  [RuntimeValue]
  [SerializeField]
  public bool _in_process;
  [RuntimeValue]
  public Vector2 delta_vec;
  public GJCommons.VoidDelegate _on_stoped;
  public bool _is_drop;
  public bool _was_in_wall_durint_kick;
  public DropResGameObject _drop_go;
  public static HashSet<KickComponent> _all_kicks = new HashSet<KickComponent>();

  public void SetDropResGameObject(DropResGameObject dgo)
  {
    this._drop_go = dgo;
    this.ForceSetGameObjectLinks((WorldGameObject) null, dgo.gameObject);
  }

  public bool is_kickable => this._type == KickComponent.KickType.Kickable;

  public bool is_kicker => this._type == KickComponent.KickType.Kicker;

  public bool in_process => this._in_process;

  public override void StartComponent()
  {
    this._is_drop = (Object) this.wgo == (Object) null;
    this.SetTf(this._is_drop ? this._drop_go.transform : this.wgo.transform);
    this._type = this._is_drop || !this.components.character.enabled ? KickComponent.KickType.Kickable : KickComponent.KickType.Kicker;
    if (!this._is_drop)
      this.CheckKickParams();
    this.active = !this._is_drop && !this.components.character.enabled;
  }

  public void CheckKickParams()
  {
    if ((Object) this.wgo == (Object) null)
      Debug.LogError((object) "WGO is null");
    else if (this.wgo.obj_def == null)
    {
      Debug.LogError((object) ("Obj def is null for WGO " + this.wgo.name), (Object) this.wgo);
    }
    else
    {
      if (!this.wgo.obj_def.res.Has("kick_speed"))
        this.wgo.obj_def.res.Set("kick_speed", 1f);
      if (this.wgo.obj_def.res.Has("kick_friction"))
        return;
      this.wgo.obj_def.res.Set("kick_friction", 0.9f);
    }
  }

  public override bool HasUpdate() => true;

  public override void UpdateComponent(float delta_time)
  {
    if (this.is_kickable || this.DelayedUpdate(delta_time))
      return;
    Vector3 position = this.tf.position;
    foreach (KickComponent allKick in KickComponent._all_kicks)
    {
      if (allKick.enabled && !allKick.is_kicker && allKick.active)
        allKick.CheckKick(position, 0.4f, 1f);
    }
  }

  public override bool HasFixedUpdate() => true;

  public override void FixedUpdateComponent(float delta_time)
  {
    if (!this._in_process || (Object) this.wgo == (Object) null || this.wgo.is_dead)
      return;
    if (this.InWall())
    {
      if (!this._was_in_wall_durint_kick)
      {
        this.InterruptKick(true);
        return;
      }
    }
    else if (this._was_in_wall_durint_kick)
      this._was_in_wall_durint_kick = false;
    this.delta_vec *= this._is_drop ? 0.96f : this.wgo.obj_def.kick_friction;
    if ((double) Mathf.Abs(this.delta_vec.x) < 0.0099999997764825821)
      this.delta_vec.x = 0.0f;
    if ((double) Mathf.Abs(this.delta_vec.y) < 0.0099999997764825821)
      this.delta_vec.y = 0.0f;
    if (this.delta_vec.magnitude.EqualsTo(0.0f))
    {
      this._in_process = false;
      this._on_stoped.TryInvoke();
      this._on_stoped = (GJCommons.VoidDelegate) null;
    }
    else
      this.body.MovePosition(this.body.position + new Vector2(this.delta_vec.x, this.delta_vec.y * 0.8f) * (this._is_drop ? 1f : this.wgo.obj_def.kick_speed) * delta_time * 96f);
  }

  public bool InWall()
  {
    return (Object) Physics2D.OverlapCircle((Vector2) this.tf.position, 0.14f, 1) != (Object) null;
  }

  public void CheckKick(Vector3 kicker_pos, float min_dist, float factor)
  {
    if (this.is_kicker)
      return;
    if ((Object) this.body == (Object) null)
    {
      Debug.LogWarning((object) "no rigidbody", (Object) this.go);
      this.enabled = false;
    }
    else
    {
      Vector2 direction = (Vector2) ((this.tf.position - kicker_pos) / 96f);
      if ((double) direction.magnitude > (double) min_dist)
        return;
      this.Kick(direction, factor);
    }
  }

  public KickComponent KickFrom(
    Vector2 from_pos,
    float force_factor = 1f,
    GJCommons.VoidDelegate on_stoped = null)
  {
    return this.Kick(this.wgo.pos - from_pos, force_factor, on_stoped);
  }

  public KickComponent Kick(
    Vector2 direction,
    float force_factor = 1f,
    GJCommons.VoidDelegate on_stoped = null)
  {
    this.delta_vec = direction.normalized * force_factor;
    this._on_stoped = on_stoped;
    this._in_process = true;
    this._was_in_wall_durint_kick = this.InWall();
    this.FixedUpdateComponent(Time.fixedDeltaTime);
    return this;
  }

  public KickComponent SetSpeed(float value)
  {
    if ((double) value > 0.0)
      this.wgo.obj_def.res.Set("kick_speed", value);
    return this;
  }

  public KickComponent SetFriction(float value)
  {
    if ((double) value > 0.0)
      this.wgo.obj_def.res.Set("kick_friction", value);
    return this;
  }

  public void InterruptKick(bool call_on_stoped = false)
  {
    this._in_process = false;
    this.delta_vec = Vector2.zero;
    if (call_on_stoped)
      this._on_stoped.TryInvoke();
    this._on_stoped = (GJCommons.VoidDelegate) null;
  }

  public static void ResetAtGameStart() => KickComponent._all_kicks.Clear();

  public override void UpdateEnableState(ObjectDefinition.ObjType obj_type)
  {
    this.enabled = obj_type == ObjectDefinition.ObjType.Mob || obj_type == ObjectDefinition.ObjType.NPC || (Object) this.wgo != (Object) null && this.wgo.is_player || (Object) this.go.GetComponent<DropResGameObject>() != (Object) null;
    this.OnEnableStateChanged();
  }

  public void OnEnableStateChanged()
  {
    if (this.enabled)
    {
      if (KickComponent._all_kicks.Contains(this))
        return;
      KickComponent._all_kicks.Add(this);
    }
    else
    {
      if (!KickComponent._all_kicks.Contains(this))
        return;
      KickComponent._all_kicks.Remove(this);
    }
  }

  public enum KickType
  {
    Kicker,
    Kickable,
  }
}
