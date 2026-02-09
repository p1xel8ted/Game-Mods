// Decompiled with JetBrains decompiler
// Type: ProjectileObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ProjectileObject : MonoBehaviour
{
  public WorldGameObject father;
  public string id;
  public ProjectileDefinition definition;
  public ProjectileObjectPart pop;
  public Vector2 direction;
  public Transform _tf;
  public bool _is_wrong;
  public bool _waiting_for_animation_finished;
  public float _dist_to_cover;
  public float _time_to_live;
  public bool _has_max_living_time;
  public bool _update_movement;
  public int damaged_objs;

  public static ProjectileObject Create(
    string id,
    Transform parent,
    Vector2 pos,
    Vector2 direction,
    WorldGameObject father,
    List<Collider2D> skip_colliders = null)
  {
    ProjectileObject context = UnityEngine.Object.Instantiate<ProjectileObject>(Prefabs.projectile_prefab, parent);
    context.gameObject.name = id;
    context.transform.position = (Vector3) pos;
    context.direction = direction.normalized;
    context.father = father;
    Debug.Log((object) $"Creating projectile: id={id}; pos={pos.ToString()}; direction={direction.ToString()}; father={((UnityEngine.Object) father == (UnityEngine.Object) null ? "null" : father.obj_id)}.", (UnityEngine.Object) context);
    context.SetID(id);
    if (skip_colliders != null && skip_colliders.Count != 0 && (UnityEngine.Object) context.pop.collider_controller != (UnityEngine.Object) null)
      context.pop.collider_controller.AddSkipColliders(skip_colliders);
    context.gameObject.SetActive(true);
    return context;
  }

  public void SetID(string projectile_id)
  {
    this._is_wrong = true;
    if (string.IsNullOrEmpty(projectile_id))
    {
      Debug.LogError((object) "Can not SetProjectileID: id is null!");
    }
    else
    {
      this.id = projectile_id;
      this.definition = GameBalance.me.GetData<ProjectileDefinition>(this.id);
      if (this.definition == null)
      {
        Debug.LogError((object) "Can not SetProjectileID: definition is null!");
      }
      else
      {
        this._dist_to_cover = this.definition.GetMaxDist();
        this._time_to_live = this.definition.max_time;
        this._has_max_living_time = !this._time_to_live.EqualsTo(0.0f, 0.1f);
        this.damaged_objs = 0;
        ProjectileObjectPart original = ProjectileObjectPart.Load(this.definition.prefab_name);
        if ((UnityEngine.Object) original == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "Can not SetProjectileID: prefab is null!");
        }
        else
        {
          this._tf = this.transform;
          this.pop = UnityEngine.Object.Instantiate<ProjectileObjectPart>(original, this._tf);
          this.pop.Init();
          this.pop.collider_controller.father = this;
          this._is_wrong = false;
          this._update_movement = false;
          this._waiting_for_animation_finished = false;
          this.OnStart();
        }
      }
    }
  }

  public void DoDestroy()
  {
    this._is_wrong = true;
    ChunkedGameObject component = this.GetComponent<ChunkedGameObject>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) $"Projectile \"{this.id}\" has ChunkedGameObject.");
      ChunkManager.OnDestroyObject(component);
      component.destroyed = true;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Update()
  {
    if (this._is_wrong || this._waiting_for_animation_finished)
      return;
    if ((double) this._dist_to_cover < 0.0)
      this.OnMaxDistReached();
    else if (this._has_max_living_time && (double) this._time_to_live < 0.0)
    {
      this.OnMaxDistReached();
    }
    else
    {
      if (!this._update_movement)
        return;
      Vector2 vector2 = Time.deltaTime * this.definition.speed * this.direction;
      this._tf.position = (Vector3) ((Vector2) this._tf.position + 96f * vector2);
      this._dist_to_cover -= vector2.magnitude;
      this._time_to_live -= Time.deltaTime;
    }
  }

  public void OnHitCombat(CombatComponent combat)
  {
    if (!combat.components.hp.enabled || combat.wgo.is_dead)
    {
      Debug.LogError((object) $"Can not hit to {combat.wgo.obj_id} hp disabled or wgo is dead", (UnityEngine.Object) combat.wgo);
    }
    else
    {
      combat.components.hp.DecHP(this.definition.damage);
      Vector2 normalized = ((Vector2) this._tf.position - combat.wgo.pos).normalized;
      float damage_direction = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
      if (combat.components.character.enabled)
        combat.components.character.OnWasDamaged(damage_direction);
      ++this.damaged_objs;
      bool do_destroy = this.definition.pierce > 0 && this.damaged_objs >= this.definition.pierce;
      this.pop.on_hit_combat = this.definition.on_hit_combat.Invoke((Vector2) this._tf.position, this.pop.animator, (System.Action) (() =>
      {
        if (!do_destroy)
          return;
        Debug.Log((object) $"Destroy projectile \"{this.id}\": max damaged objs reached: {this.damaged_objs.ToString()}>={this.definition.pierce.ToString()}");
        this.DoDestroy();
      }), combat.wgo, out this._waiting_for_animation_finished);
    }
  }

  public void OnStart()
  {
    this.pop.on_start = this.definition.on_start.Invoke((Vector2) this._tf.position, this.pop.animator, (System.Action) (() => this._update_movement = true), (WorldGameObject) null, out this._waiting_for_animation_finished);
  }

  public void OnHitNonCombat(WorldGameObject wgo)
  {
    if ((UnityEngine.Object) wgo != (UnityEngine.Object) null)
    {
      if (wgo.components.hp.enabled)
        wgo.components.hp.DecHP(this.definition.damage);
      Debug.Log((object) $"Destroy projectile \"{this.id}\": hit non-combat obj \"{wgo.gameObject.name}\"[{wgo.obj_id}]", (UnityEngine.Object) wgo);
    }
    else
      Debug.Log((object) $"Destroy projectile \"{this.id}\": hit non-wgo obj");
    this.pop.on_hit_non_combat = this.definition.on_hit_non_combat.Invoke((Vector2) this._tf.position, this.pop.animator, new System.Action(this.DoDestroy), wgo, out this._waiting_for_animation_finished);
  }

  public void OnOutOfScreen()
  {
    Debug.Log((object) "Destroy projectile: out of screen");
    this.pop.on_out_of_screen = this.definition.on_out_of_screen.Invoke((Vector2) this._tf.position, this.pop.animator, new System.Action(this.DoDestroy), (WorldGameObject) null, out this._waiting_for_animation_finished);
  }

  public void OnMaxDistReached()
  {
    Debug.Log((object) "Destroy projectile: max dist reached");
    this.pop.on_max_dist_reached = this.definition.on_max_dist_reached.Invoke((Vector2) this._tf.position, this.pop.animator, new System.Action(this.DoDestroy), (WorldGameObject) null, out this._waiting_for_animation_finished);
  }

  [CompilerGenerated]
  public void \u003COnStart\u003Eb__18_0() => this._update_movement = true;
}
