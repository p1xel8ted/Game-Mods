// Decompiled with JetBrains decompiler
// Type: DockPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DockPoint : MonoBehaviour, IComparable<DockPoint>
{
  public const float DROP_OFFSET_FORWARD = 19.2f;
  public const float DROP_OFFSET_RIGHT = 24f;
  public const float EPS = 0.002f;
  public const int REACHABILITY_MASK = 1;
  public Direction action_dir;
  public Vector2 _direction_vec;
  public WorldGameObject _parent_wgo;
  public CraftComponent _craft;
  public bool _parent_cached;
  public BaseCharacterComponent _target;
  public Transform _tf;
  public bool _tf_cached;
  public float _dist_to_target = float.MaxValue;
  public bool can_place_worker = true;
  [CompilerGenerated]
  public bool \u003Cshouldnt_be_used\u003Ek__BackingField;
  [CompilerGenerated]
  public Vector2 \u003Creach_dir\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003Creached\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003Cjust_rotate\u003Ek__BackingField;

  public bool is_busy => this._target != null;

  public bool shouldnt_be_used
  {
    get => this.\u003Cshouldnt_be_used\u003Ek__BackingField;
    set => this.\u003Cshouldnt_be_used\u003Ek__BackingField = value;
  }

  public Vector2 reach_dir
  {
    get => this.\u003Creach_dir\u003Ek__BackingField;
    set => this.\u003Creach_dir\u003Ek__BackingField = value;
  }

  public bool reached
  {
    get => this.\u003Creached\u003Ek__BackingField;
    set => this.\u003Creached\u003Ek__BackingField = value;
  }

  public bool just_rotate
  {
    get => this.\u003Cjust_rotate\u003Ek__BackingField;
    set => this.\u003Cjust_rotate\u003Ek__BackingField = value;
  }

  public BaseCharacterComponent target => this._target;

  public WorldGameObject parent_wgo
  {
    get
    {
      if (this._parent_cached)
        return this._parent_wgo;
      this._parent_cached = true;
      this._parent_wgo = this.GetComponentInParent<WorldGameObject>();
      this._craft = (UnityEngine.Object) this._parent_wgo != (UnityEngine.Object) null ? this._parent_wgo.components.craft : (CraftComponent) null;
      return this._parent_wgo;
    }
  }

  public CraftComponent craft => this._craft;

  public Transform tf
  {
    get
    {
      return !this._tf_cached ? this.Cache<Transform>(out this._tf, out this._tf_cached, false) : this._tf;
    }
  }

  public void StartDocks(WorldGameObject parent)
  {
    this.shouldnt_be_used = false;
    this._parent_wgo = parent;
    this._craft = this._parent_wgo.components.craft;
    this._parent_cached = true;
    if (this.action_dir == Direction.None)
      this.ReFindActionDir();
    this._direction_vec = this.GetActionDir().ToVec();
  }

  public void ReFindActionDir()
  {
    if ((UnityEngine.Object) this._parent_wgo == (UnityEngine.Object) null)
    {
      this._parent_wgo = this.GetComponentInParent<WorldGameObject>();
      if ((UnityEngine.Object) this._parent_wgo != (UnityEngine.Object) null)
        this._craft = this._parent_wgo.components.craft;
    }
    this.action_dir = this.tf.DirTo(((UnityEngine.Object) this._parent_wgo != (UnityEngine.Object) null ? (Component) this._parent_wgo.tf : (Component) this.GetComponentInParent<WorldObjectPart>().transform).transform).normalized.ToDirection();
  }

  public bool IsUnreachable(float player_radius)
  {
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) this.tf.position, player_radius, 1))
    {
      if (!((UnityEngine.Object) collider2D == (UnityEngine.Object) null) && !collider2D.isTrigger)
      {
        WorldGameObject worldGameObject = collider2D.GetComponent<WorldGameObject>() ?? collider2D.GetComponentInParent<WorldGameObject>();
        if ((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null || worldGameObject.GetInstanceID() != this.parent_wgo.GetInstanceID())
          return true;
      }
    }
    return false;
  }

  public void SetTarget(BaseCharacterComponent target)
  {
    this._target = target;
    this._dist_to_target = this._target != null ? this.CalcDistToTarget(this._target) : float.MaxValue;
  }

  public void Reset(bool shouldnt_be_used)
  {
    this.shouldnt_be_used = shouldnt_be_used;
    if (this._target == null)
      return;
    this._target.ResetDockPoints();
  }

  public void UpdateOverlapedCollider()
  {
  }

  public float CalcDistToTarget(BaseCharacterComponent target = null)
  {
    if (target == null)
    {
      if (this._target == null)
      {
        Debug.LogError((object) "null target");
        return 0.0f;
      }
      target = this._target;
    }
    this._dist_to_target = this.tf.position.DistTo(target.tf.position + target.anim_direction.ToVec3() * 48f);
    return this._dist_to_target;
  }

  public void CheckIfReached()
  {
    this.reached = this.just_rotate = false;
    Vector2 vector2 = this._target.tf.DirTo(this.tf);
    this._dist_to_target = vector2.magnitude;
    if ((double) this._dist_to_target < (double) this._target.step || this._dist_to_target.EqualsTo(0.0f, 1f / 500f))
    {
      this.reached = this._dist_to_target.EqualsTo(0.0f, 1f / 500f) && this.GetActionDir() == this._target.anim_direction;
      if (this.reached)
        return;
      this.just_rotate = true;
      this.reach_dir = this._direction_vec;
    }
    else
      this.reach_dir = vector2.normalized;
  }

  public int CompareTo(DockPoint other)
  {
    if (this._dist_to_target.EqualsTo(other._dist_to_target))
      return 0;
    return (double) this._dist_to_target <= (double) other._dist_to_target ? -1 : 1;
  }

  public Vector3 GetDropPos()
  {
    Direction actionDir = this.GetActionDir();
    return (Vector3) (Vector2) (this.tf.position + actionDir.ToVec3() * 19.2f + actionDir.ClockwiseDir().ToVec3() * 24f);
  }

  public Direction GetActionDir()
  {
    Direction actionDir = this.action_dir;
    switch (actionDir)
    {
      case Direction.Right:
      case Direction.Left:
        if ((double) this.transform.lossyScale.x < 0.0)
        {
          switch (actionDir)
          {
            case Direction.Right:
              actionDir = Direction.Left;
              break;
            case Direction.Left:
              actionDir = Direction.Right;
              break;
          }
        }
        else
          break;
        break;
    }
    return actionDir;
  }
}
