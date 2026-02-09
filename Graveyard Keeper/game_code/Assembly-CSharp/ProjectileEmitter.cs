// Decompiled with JetBrains decompiler
// Type: ProjectileEmitter
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ProjectileEmitter : MonoBehaviour
{
  public const string SHOOT_TRIGGER = "do_shot";
  public bool is_paused;
  public string projectile_name;
  public Direction direction;
  public Vector2 direction_vector;
  [Range(0.0f, 45f)]
  public float direction_delta;
  public GameObject throwing_position_go;
  public float period;
  public float period_delta;
  public float projectile_spawn_delay;
  public WorldGameObject father_wgo;
  public Animator animator;
  public Transform target_tf;
  public ProjectileEmitter.ShootingEnded on_anim_end;
  public ProjectileEmitter.ProjectileEmitterType type;
  public List<Vector2> directions;
  public bool _has_period;
  public Transform _throwing_position_tf;
  public Transform _parent_tf;
  public float _time_left_for_shot;
  public Collider2D[] _self_colliders;
  public bool _has_shoot_trigger;
  public float _spawn_projectile_after;
  public bool _waiting_for_delay;

  public bool has_period => this._has_period;

  public Transform throwing_pos_tf => this._throwing_position_tf;

  public void Start()
  {
    this.father_wgo = this.GetComponentInParent<WorldGameObject>();
    this._has_period = (double) this.period > 0.0099999997764825821;
    this.is_paused = true;
    switch (this.direction)
    {
      case Direction.None:
        this.direction_vector = this.direction_vector.normalized;
        break;
      case Direction.Right:
      case Direction.Up:
      case Direction.Left:
      case Direction.Down:
        this.direction_vector = this.direction.ToVec();
        break;
      case Direction.IgnoreDirection:
      case Direction.ToPlayer:
        Debug.LogError((object) ("Wrong direction: " + this.direction.ToString()));
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this._throwing_position_tf = (UnityEngine.Object) this.throwing_position_go == (UnityEngine.Object) null ? this.transform : this.throwing_position_go.transform;
    this._time_left_for_shot = 0.0f;
    if ((UnityEngine.Object) this.father_wgo != (UnityEngine.Object) null)
    {
      this._parent_tf = this.father_wgo.tf.parent;
      if ((UnityEngine.Object) this.animator == (UnityEngine.Object) null)
        this.animator = this.father_wgo.wop.GetComponent<Animator>();
      this._self_colliders = this.father_wgo.wop.GetComponentsInChildren<Collider2D>(true);
    }
    else
    {
      WorldSimpleObject componentInParent1 = this.GetComponentInParent<WorldSimpleObject>();
      if ((UnityEngine.Object) componentInParent1 != (UnityEngine.Object) null)
      {
        this._parent_tf = componentInParent1.transform.parent;
        if ((UnityEngine.Object) this.animator == (UnityEngine.Object) null)
          this.animator = componentInParent1.GetComponent<Animator>();
        this._self_colliders = componentInParent1.GetComponentsInChildren<Collider2D>(true);
      }
      else
      {
        ChunkedGameObject componentInParent2 = this.GetComponentInParent<ChunkedGameObject>();
        if ((UnityEngine.Object) componentInParent2 != (UnityEngine.Object) null)
        {
          this._parent_tf = componentInParent2.transform.parent;
          if ((UnityEngine.Object) this.animator == (UnityEngine.Object) null)
            this.animator = componentInParent2.GetComponent<Animator>();
          this._self_colliders = componentInParent2.GetComponentsInChildren<Collider2D>(true);
        }
        else
        {
          this._parent_tf = this.transform;
          if ((UnityEngine.Object) this.animator == (UnityEngine.Object) null)
            this.animator = this.GetComponent<Animator>();
          this._self_colliders = this.GetComponentsInChildren<Collider2D>(true);
        }
      }
    }
    this._has_shoot_trigger = false;
    if (!((UnityEngine.Object) this.animator != (UnityEngine.Object) null))
      return;
    foreach (AnimatorControllerParameter parameter in this.animator.parameters)
    {
      if (parameter.name == "do_shot")
      {
        this._has_shoot_trigger = true;
        break;
      }
    }
  }

  public void Update()
  {
    if (this._waiting_for_delay && (double) this._spawn_projectile_after > 0.0)
    {
      this._spawn_projectile_after -= Time.deltaTime;
      if ((double) this._spawn_projectile_after < 0.0)
      {
        this._waiting_for_delay = false;
        this.SpawnProjectile();
      }
    }
    if (!this._has_period || this.is_paused)
      return;
    this._time_left_for_shot -= Time.deltaTime;
    if ((double) this._time_left_for_shot > 0.0)
      return;
    this.DoShot();
    this._time_left_for_shot = this.period;
    if (this.period_delta.EqualsTo(0.0f, 0.01f))
      return;
    this._time_left_for_shot += UnityEngine.Random.Range(-this.period_delta, this.period_delta);
  }

  public void DoShot(ProjectileEmitter.ShootingEnded anim_end = null)
  {
    if (this._has_shoot_trigger)
      this.animator.SetTrigger("do_shot");
    if (this.projectile_spawn_delay.EqualsTo(0.0f))
    {
      this.SpawnProjectile();
    }
    else
    {
      this._waiting_for_delay = true;
      this._spawn_projectile_after = this.projectile_spawn_delay;
    }
    this.on_anim_end = anim_end;
  }

  public void SpawnProjectile()
  {
    switch (this.type)
    {
      case ProjectileEmitter.ProjectileEmitterType.Single:
        ProjectileObject.Create(this.projectile_name, this._parent_tf, (Vector2) this._throwing_position_tf.position, this.GetThrowingDirection(), this.father_wgo, ((IEnumerable<Collider2D>) this._self_colliders).ToList<Collider2D>());
        this.target_tf = (Transform) null;
        break;
      case ProjectileEmitter.ProjectileEmitterType.Around:
        if (this.directions == null || this.directions.Count == 0)
        {
          Debug.LogError((object) "Can not spawn projectile: directions list is null or empty.");
          break;
        }
        using (List<Vector2>.Enumerator enumerator = this.directions.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Vector2 current = enumerator.Current;
            if ((double) current.magnitude < 0.5)
              Debug.LogWarning((object) ("ProjectileEmitter skip direction " + current.ToString()), (UnityEngine.Object) this);
            else
              ProjectileObject.Create(this.projectile_name, this._parent_tf, (Vector2) this._throwing_position_tf.position, current, this.father_wgo, ((IEnumerable<Collider2D>) this._self_colliders).ToList<Collider2D>());
          }
          break;
        }
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void DoShot(Vector2 dir, ProjectileEmitter.ShootingEnded anim_end = null)
  {
    this.type = ProjectileEmitter.ProjectileEmitterType.Single;
    this.direction_vector = dir.normalized;
    this.direction_delta = 0.0f;
    this.DoShot(anim_end);
  }

  public void DoShot(Transform target, ProjectileEmitter.ShootingEnded anim_end = null)
  {
    this.type = ProjectileEmitter.ProjectileEmitterType.Single;
    this.target_tf = target;
    this.DoShot(anim_end);
  }

  public void DoShots(List<Vector2> dirs, ProjectileEmitter.ShootingEnded anim_end = null)
  {
    if (dirs == null || dirs.Count == 0)
    {
      Debug.LogError((object) "Can not DoShots: dirs is nuil or empty!", (UnityEngine.Object) this);
    }
    else
    {
      this.type = ProjectileEmitter.ProjectileEmitterType.Around;
      this.directions = dirs;
      this.DoShot(anim_end);
    }
  }

  public Vector2 GetThrowingDirection()
  {
    if ((UnityEngine.Object) this.target_tf != (UnityEngine.Object) null)
      return this._throwing_position_tf.DirTo(this.target_tf);
    if (this.direction_delta.EqualsTo(0.0f, 0.1f))
      return this.direction_vector;
    float z = UnityEngine.Random.Range(-this.direction_delta, this.direction_delta);
    Vector2 throwingDirection = (Vector2) (Quaternion.Euler(0.0f, 0.0f, z) * (Vector3) this.direction_vector);
    Debug.Log((object) $"Projectile rand direction: direction={this.direction_vector.ToString()}, degree={z.ToString()}, rand_direction={throwingDirection.ToString()}");
    return throwingDirection;
  }

  public delegate void ShootingEnded(bool succeed);

  public enum ProjectileEmitterType
  {
    Single,
    Around,
  }
}
