// Decompiled with JetBrains decompiler
// Type: ChunkManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#nullable disable
public class ChunkManager : MonoBehaviour
{
  public static ChunkManager _me = (ChunkManager) null;
  public static bool _inited = false;
  public static List<ChunkedGameObject> _objs = new List<ChunkedGameObject>();
  public static HashSet<int> _objs_ids = new HashSet<int>();
  public static List<ChunkedGameObject> _objs_to_add = new List<ChunkedGameObject>();
  public static List<ChunkedGameObject> _objs_to_remove = new List<ChunkedGameObject>();
  [Range(1f, 30f)]
  public int visible_chunk_radius_x = 18;
  [Range(1f, 30f)]
  public int visible_chunk_radius_y = 12;
  public const int EXTRA_VISIBLE_CHUNKS = 1;
  public const float SCREEN_TO_CHUNK_KOEFF = 160f;
  public static Thread _thread = (Thread) null;
  public static List<ChunkedGameObject> _changed_objs = new List<ChunkedGameObject>();
  public bool _thread_calculating;
  public bool _thread_done_calculating;
  public bool _thread_ready_to_start;
  public static Vector3 _camera_pos = Vector3.zero;
  public bool _need_astar_recalc;
  public Bounds _astar_recalc_bounds;
  public bool _is_destroy_temp_mode;
  public bool _pending_temp_remove;

  public static void Init()
  {
    if (!Application.isPlaying)
      return;
    ChunkManager._me = SingletonGameObjects.FindOrCreate<ChunkManager>();
    ChunkManager._inited = true;
    ChunkManager.RecalculateResolution();
    ChunkManager._thread = new Thread(new ThreadStart(ChunkManager._me.UpdateThread));
    ChunkManager._thread.Start();
  }

  public static void RecalculateResolution(int w = -1, int h = -1)
  {
    if (!ChunkManager._inited)
      return;
    if (w == -1)
      w = Screen.width;
    if (h == -1)
      h = Screen.height;
    ChunkManager._me.visible_chunk_radius_x = Mathf.CeilToInt((float) w / 160f) + 1;
    ChunkManager._me.visible_chunk_radius_y = Mathf.CeilToInt((float) h / 160f) + 1;
  }

  public static void ClearChunksList()
  {
    if (ChunkManager._thread != null && ChunkManager._thread.IsAlive)
      ChunkManager._thread.Abort();
    ChunkManager._objs.Clear();
    ChunkManager._objs_ids.Clear();
    ChunkManager._objs_to_add.Clear();
    ChunkManager._objs_to_remove.Clear();
    ChunkManager._thread = new Thread(new ThreadStart(ChunkManager._me.UpdateThread));
    ChunkManager._thread.Start();
  }

  public static void OnAddNewObject(ChunkedGameObject go)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return;
    foreach (ChunkedGameObject componentsInChild in go.GetComponentsInChildren<ChunkedGameObject>())
    {
      if ((UnityEngine.Object) componentsInChild.gameObject != (UnityEngine.Object) go.gameObject)
        return;
      componentsInChild.Init();
    }
    ChunkManager._objs_to_add.Add(go);
  }

  public static void OnDestroyObject(WorldGameObject wgo)
  {
    if (wgo.is_removed)
      return;
    ChunkedGameObject componentInChildren = wgo.GetComponentInChildren<ChunkedGameObject>();
    if (!((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null))
      return;
    ChunkManager.OnDestroyObject(componentInChildren);
    componentInChildren.destroyed = true;
  }

  public static void OnDestroyObject(ChunkedGameObject go)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return;
    foreach (ChunkedGameObject componentsInChild in go.GetComponentsInChildren<ChunkedGameObject>())
    {
      if (componentsInChild.is_temp)
        componentsInChild.pending_to_remove = true;
      else if (ChunkManager._objs_to_add.Contains(componentsInChild))
        ChunkManager._objs_to_add.Remove(componentsInChild);
      else
        ChunkManager._objs_to_remove.Add(componentsInChild);
    }
  }

  public void ProcessRemove()
  {
    foreach (ChunkedGameObject context in ChunkManager._objs_to_remove)
    {
      int num = context.instance_id;
      if (num == -1)
      {
        if (MainGame.game_started)
          Debug.LogWarning((object) "Strange. Trying to remove an object without iid", (UnityEngine.Object) context);
        try
        {
          num = context.instance_id = context.gameObject.GetInstanceID();
        }
        catch (Exception ex)
        {
        }
      }
      if (ChunkManager._objs_ids.Contains(num))
      {
        ChunkManager._objs.Remove(context);
        ChunkManager._objs_ids.Remove(num);
      }
    }
    ChunkManager._objs_to_remove.Clear();
    if (this._pending_temp_remove)
    {
      for (int index = ChunkManager._objs.Count - 1; index >= 0; --index)
      {
        ChunkedGameObject chunkedGameObject = ChunkManager._objs[index];
        if (chunkedGameObject.is_temp && chunkedGameObject.pending_to_remove)
        {
          ChunkManager._objs.RemoveAt(index);
          int num = chunkedGameObject.instance_id;
          if (num == -1)
          {
            try
            {
              num = chunkedGameObject.instance_id = chunkedGameObject.gameObject.GetInstanceID();
            }
            catch (Exception ex)
            {
            }
          }
          ChunkManager._objs_ids.Remove(num);
        }
      }
    }
    this._pending_temp_remove = false;
  }

  public void Update()
  {
    if ((UnityEngine.Object) MainGame.me.player == (UnityEngine.Object) null || !Application.isPlaying)
      return;
    ChunkManager._camera_pos = MainGame.me.transform.position;
    this.ProcessRemove();
    foreach (ChunkedGameObject chunkedGameObject in ChunkManager._objs_to_add)
    {
      int num = chunkedGameObject.instance_id;
      if (num == -1)
        num = chunkedGameObject.instance_id = chunkedGameObject.gameObject.GetInstanceID();
      if (!ChunkManager._objs_ids.Contains(num))
      {
        ChunkManager._objs.Add(chunkedGameObject);
        ChunkManager._objs_ids.Add(num);
      }
    }
    ChunkManager._objs_to_add.Clear();
    if (!this._thread_calculating)
    {
      if (this._thread_done_calculating)
        this.UpdateObjectsVisibility();
      this.StartThread();
    }
    if (!this._need_astar_recalc)
      return;
    this._need_astar_recalc = false;
    AStarTools.UpdateAstarBounds(this._astar_recalc_bounds);
  }

  public void LateUpdate()
  {
    if (!this._thread_done_calculating)
      return;
    this.UpdateObjectsVisibility();
  }

  public void UpdateObjectsVisibility()
  {
    int count = ChunkManager._changed_objs.Count;
    if (count == 0)
      return;
    this.ProcessRemove();
    for (int index = 0; index < count; ++index)
      ChunkManager._changed_objs[index].UpdateVisibility();
    ChunkManager._changed_objs.Clear();
    this._thread_done_calculating = false;
  }

  public void StartThread()
  {
    if (this._thread_calculating)
      return;
    this._thread_ready_to_start = true;
    this._thread_done_calculating = false;
    this._thread_calculating = true;
  }

  public void UpdateThread()
  {
    while (true)
    {
      while (!this._thread_ready_to_start)
        Thread.Sleep(1);
      this.UpdateThreadFunction();
      this._thread_done_calculating = true;
      this._thread_ready_to_start = false;
      this._thread_calculating = false;
    }
  }

  public void UpdateThreadFunction()
  {
    Vector2 vector2 = (Vector2) (ChunkManager._camera_pos / 96f);
    int num1 = Mathf.RoundToInt(vector2.x);
    int num2 = Mathf.RoundToInt(vector2.y);
    for (int index = 0; index < ChunkManager._objs.Count; ++index)
    {
      ChunkedGameObject chunkedGameObject = ChunkManager._objs[index];
      if (chunkedGameObject.can_go_inactive)
      {
        chunkedGameObject.out_x_1 = (float) (chunkedGameObject.chunk_x_max - num1);
        chunkedGameObject.out_x_2 = (float) (chunkedGameObject.chunk_x_min - num1);
        chunkedGameObject.out_y_1 = (float) (chunkedGameObject.chunk_y_max - num2);
        chunkedGameObject.out_y_2 = (float) (chunkedGameObject.chunk_y_min - num2);
        bool flag = (double) chunkedGameObject.out_x_1 > (double) -this.visible_chunk_radius_x && (double) chunkedGameObject.out_x_2 < (double) this.visible_chunk_radius_x && (double) chunkedGameObject.out_y_1 > (double) -this.visible_chunk_radius_y && (double) chunkedGameObject.out_y_2 < (double) this.visible_chunk_radius_y;
        if (chunkedGameObject.obj_visible != flag)
        {
          chunkedGameObject.obj_visible = flag;
          ChunkManager._changed_objs.Add(chunkedGameObject);
        }
      }
    }
  }

  public static void RescanAllObjects()
  {
    Debug.Log((object) nameof (RescanAllObjects));
    if ((UnityEngine.Object) ChunkManager._me == (UnityEngine.Object) null)
      ChunkManager.Init();
    ChunkManager.ClearChunksList();
    ChunkedGameObject[] componentsInChildren = MainGame.me.world_root.GetComponentsInChildren<ChunkedGameObject>(true);
    int num = 0;
    foreach (ChunkedGameObject go in componentsInChildren)
    {
      ChunkManager.OnAddNewObject(go);
      ++num;
    }
    Debug.Log((object) ("RescanAllObjects: added objects = " + num.ToString()));
  }

  public void OnDestroy()
  {
    if (ChunkManager._thread == null || !ChunkManager._thread.IsAlive)
      return;
    ChunkManager._thread.Abort();
  }

  public static void RecalcAStarBounds(Bounds b)
  {
    if (!ChunkManager._me._need_astar_recalc)
    {
      ChunkManager._me._need_astar_recalc = true;
      ChunkManager._me._astar_recalc_bounds = b;
    }
    else
      ChunkManager._me._astar_recalc_bounds.Encapsulate(b);
  }

  public static void RemovePedingTempObjects() => ChunkManager._me._pending_temp_remove = true;
}
