// Decompiled with JetBrains decompiler
// Type: ChunkedGameObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChunkedGameObject : MonoBehaviour
{
  public const bool RESCAN_ASTAR_ON_ENABLE = true;
  public int chunk_x;
  public int chunk_y;
  public int chunk_x_min;
  public int chunk_x_max;
  public int chunk_y_min;
  public int chunk_y_max;
  public bool reset_transform_change;
  public bool _obj_really_visible = true;
  [NonSerialized]
  public bool obj_visible = true;
  public bool always_active;
  public bool _active_now_because_of_movement;
  public bool _active_now_because_of_events;
  public bool _active_now_because_of_work;
  [NonSerialized]
  public int instance_id = -1;
  public DynamicLight _dyn_light;
  public bool _dyn_light_cached;
  public const int CHUNK_SIZE = 96 /*0x60*/;
  public GraphicsObjectOptimizer _optimizer;
  public bool _optimized_inited;
  public bool _inited;
  public bool _inited_after_change_wgo;
  public Bounds _bounds;
  public float _bounds_x_plus;
  public float _bounds_y_plus;
  public float _bounds_x_minus;
  public float _bounds_y_minus;
  public float out_x_1;
  public float out_x_2;
  public float out_y_1;
  public float out_y_2;
  public bool _shadow_inited;
  public ObjectDynamicShadow _shadow;
  [NonSerialized]
  public bool destroyed;
  [NonSerialized]
  public bool destroy_instead_of_turnong_off;
  public bool _started;
  [NonSerialized]
  public bool is_temp;
  [NonSerialized]
  public bool pending_to_remove;

  public bool can_go_inactive
  {
    get
    {
      return !this.always_active && !this.active_now_because_of_movement && !this._active_now_because_of_events && !this._active_now_because_of_work;
    }
  }

  public bool active_now_because_of_events
  {
    get => this._active_now_because_of_events;
    set
    {
      this._active_now_because_of_events = value;
      if (this.gameObject.activeInHierarchy || !this._active_now_because_of_events)
        return;
      this.gameObject.SetActive(true);
    }
  }

  public bool active_now_because_of_movement
  {
    get => this._active_now_because_of_movement;
    set
    {
      this._active_now_because_of_movement = value;
      if (this.gameObject.activeInHierarchy || !this._active_now_because_of_movement)
        return;
      this.gameObject.SetActive(true);
    }
  }

  public bool active_now_because_of_work
  {
    get => this._active_now_because_of_work;
    set
    {
      this._active_now_because_of_work = value;
      if (this.gameObject.activeInHierarchy || !this._active_now_because_of_work)
        return;
      this.gameObject.SetActive(true);
    }
  }

  public virtual void Update()
  {
    if (MainGame.disable_all_game || !this.transform.hasChanged)
      return;
    this.reset_transform_change = true;
    this.RecalculateChunk();
  }

  public void UpdateVisibility()
  {
    if (this.destroyed)
      return;
    if (this.pending_to_remove)
      return;
    try
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
        this.destroyed = true;
    }
    catch (Exception ex)
    {
      this.destroyed = true;
    }
    if (this.destroyed || this._obj_really_visible == this.obj_visible)
      return;
    WorldGameObject component1 = this.GetComponent<WorldGameObject>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && !this.obj_visible)
      component1.PreDisable();
    this.gameObject.SetActive(this.obj_visible);
    this._obj_really_visible = this.obj_visible;
    if (this.obj_visible && !this._optimized_inited)
    {
      this._optimized_inited = true;
      this._optimizer = new GraphicsObjectOptimizer(this.gameObject);
      this.RescanAStar();
    }
    if (this.obj_visible && !this._shadow_inited)
    {
      this._shadow_inited = true;
      this._shadow = this.GetComponentInChildren<ObjectDynamicShadow>(true);
      if ((UnityEngine.Object) this._shadow != (UnityEngine.Object) null)
        this._shadow.EnsureObjectHasShadows(true);
    }
    if (this.obj_visible || !this.destroy_instead_of_turnong_off)
      return;
    ChunkManager.OnDestroyObject(this);
    this.destroyed = true;
    ProjectileObject component2 = this.gameObject.GetComponent<ProjectileObject>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.OnOutOfScreen();
    else
      Debug.LogError((object) "Found object, that can not destroy from chunk manager (because need instructions here)!");
  }

  public void RescanAStar()
  {
    bool flag = false;
    Bounds b = new Bounds();
    foreach (OptimizedCollider2D componentsInChild in this.GetComponentsInChildren<OptimizedCollider2D>(true))
      componentsInChild.Init();
    foreach (Collider2D componentsInChild in this.GetComponentsInChildren<Collider2D>())
    {
      if (componentsInChild.gameObject.layer == 0)
      {
        if (!flag)
        {
          b = componentsInChild.bounds;
          flag = true;
        }
        else
          b.Encapsulate(componentsInChild.bounds);
      }
    }
    b.Expand((Vector3) (Vector2.one / 6f));
    if (!flag)
      return;
    ChunkManager.RecalcAStarBounds(b);
  }

  public DynamicLight dynamic_light
  {
    get
    {
      if (this._dyn_light_cached)
        return this._dyn_light;
      this._dyn_light_cached = true;
      DynamicLight[] componentsInChildren = this.GetComponentsInChildren<DynamicLight>(true);
      if (componentsInChildren.Length != 0)
        this._dyn_light = componentsInChildren[0];
      return this._dyn_light;
    }
  }

  public virtual void LateUpdate()
  {
    if (!this.reset_transform_change)
      return;
    this.transform.hasChanged = false;
    this.reset_transform_change = false;
  }

  public virtual void Start()
  {
    if (this._started)
      return;
    this._started = true;
    this.RecalculateChunk();
    if (!Application.isPlaying)
      return;
    ChunkManager.OnAddNewObject(this);
  }

  public static void GetChunk(Transform t, out int x, out int y)
  {
    Vector2 position = (Vector2) t.position;
    x = Mathf.RoundToInt(position.x / 96f);
    y = Mathf.RoundToInt(position.y / 96f);
  }

  public void RecalculateChunk()
  {
    ChunkedGameObject.GetChunk(this.transform, out this.chunk_x, out this.chunk_y);
    this.chunk_x_min = this.chunk_x + Mathf.CeilToInt(this._bounds_x_minus / 96f);
    this.chunk_x_max = this.chunk_x + Mathf.CeilToInt(this._bounds_x_plus / 96f);
    this.chunk_y_min = this.chunk_y + Mathf.CeilToInt(this._bounds_y_minus / 96f);
    this.chunk_y_max = this.chunk_y + Mathf.CeilToInt(this._bounds_y_plus / 96f);
  }

  public virtual void OnDestroy()
  {
    if (!Application.isPlaying)
      return;
    ChunkManager.OnDestroyObject(this);
  }

  public void OnJustSpawnedWGO()
  {
    if (this.can_go_inactive)
    {
      this._obj_really_visible = this.obj_visible = false;
    }
    else
    {
      this._obj_really_visible = this.obj_visible = true;
      this.gameObject.SetActive(true);
    }
    this.Start();
  }

  public void ResetAtTheBeginning()
  {
    this._obj_really_visible = this.obj_visible = this.gameObject.activeSelf;
  }

  public void Init(bool init_after_change_wgo = false)
  {
    if (!init_after_change_wgo)
    {
      if (this._inited)
        return;
    }
    else
    {
      if (this._inited_after_change_wgo)
        return;
      this._inited_after_change_wgo = init_after_change_wgo;
    }
    WorldGameObject component1 = this.GetComponent<WorldGameObject>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      ObjectDefinition objDef = component1.obj_def;
      if (objDef != null && objDef.always_active)
        this.always_active = true;
    }
    this._inited = true;
    Vector3 position = this.transform.position;
    this._shadow_inited = false;
    this._bounds = new Bounds(position, (Vector3) LazyConsts.GRID_SIZE_VECTOR2);
    Component[] componentsInChildren = this.GetComponentsInChildren<Component>(true);
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (Component component2 in componentsInChildren)
    {
      ObjectDynamicShadowChild dynamicShadowChild = component2 as ObjectDynamicShadowChild;
      if ((UnityEngine.Object) dynamicShadowChild != (UnityEngine.Object) null)
      {
        gameObjectList.Add(dynamicShadowChild.gameObject);
      }
      else
      {
        Light light = component2 as Light;
        if (!((UnityEngine.Object) light == (UnityEngine.Object) null) && light.isActiveAndEnabled)
          this._bounds.Encapsulate(new Bounds(light.transform.position, Vector3.one * light.range * 2f));
      }
    }
    foreach (Component component3 in componentsInChildren)
    {
      SpriteRenderer spriteRenderer = component3 as SpriteRenderer;
      if (!((UnityEngine.Object) spriteRenderer == (UnityEngine.Object) null) && !gameObjectList.Contains(spriteRenderer.gameObject))
        this._bounds.Encapsulate(spriteRenderer.bounds);
    }
    this._bounds.center -= position;
    this._bounds_x_plus = this._bounds.center.x + this._bounds.extents.x;
    this._bounds_y_plus = this._bounds.center.y + this._bounds.extents.y;
    this._bounds_x_minus = this._bounds.center.x - this._bounds.extents.x;
    this._bounds_y_minus = this._bounds.center.y - this._bounds.extents.y;
    this.RecalculateChunk();
  }

  public Bounds bounds
  {
    get
    {
      if (!this._inited)
        this.Init();
      return this._bounds;
    }
  }

  public void OnDrawGizmosSelected()
  {
    if (!this._inited || !Application.isPlaying || (UnityEngine.Object) this.GetComponent<PlayerComponent>() != (UnityEngine.Object) null)
      return;
    Vector3 position = this.transform.position;
    Gizmos.color = Color.blue;
    Vector3 center = this._bounds.center;
    Gizmos.DrawWireCube(position + center, new Vector3(this._bounds.size.x, this._bounds.size.y, 0.0f));
  }

  public void OnEnable() => this.StartCoroutine(this.LateEnableCoroutine());

  public IEnumerator LateEnableCoroutine()
  {
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForEndOfFrame();
    this.LateEnable();
  }

  public virtual void LateEnable()
  {
    foreach (OptimizedCollider2D componentsInChild in this.gameObject.GetComponentsInChildren<OptimizedCollider2D>(true))
      componentsInChild.Init();
  }

  public SerializableWGO.SerializableChunk SerializeChunk()
  {
    SerializableWGO.SerializableChunk serializableChunk = new SerializableWGO.SerializableChunk()
    {
      active_now_because_of_movement = this._active_now_because_of_movement,
      active_now_because_of_events = this._active_now_because_of_events,
      active_now_because_of_work = this._active_now_because_of_work,
      always_active = this.always_active
    };
    if (!serializableChunk.active_now_because_of_movement)
      return serializableChunk;
    Debug.Log((object) ("TO: data.active_now_bacause_of_action " + this.gameObject.name), (UnityEngine.Object) this);
    return serializableChunk;
  }

  public void DeserializeChunk(SerializableWGO data)
  {
    if (data.chunk.active_now_because_of_movement)
      Debug.Log((object) ("FROM: data.active_now_bacause_of_action " + this.gameObject.name), (UnityEngine.Object) this);
    this.active_now_because_of_movement = data.chunk.active_now_because_of_movement;
    this.active_now_because_of_events = data.chunk.active_now_because_of_events;
    this.active_now_because_of_work = data.chunk.active_now_because_of_work;
    this.always_active = data.chunk.always_active;
  }
}
