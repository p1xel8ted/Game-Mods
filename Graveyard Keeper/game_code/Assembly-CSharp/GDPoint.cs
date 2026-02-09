// Decompiled with JetBrains decompiler
// Type: GDPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class GDPoint : MonoBehaviour
{
  public static string[] IDLE_POINT_PREFIXES = new string[4]
  {
    "tavern_idle_",
    "village_idle_",
    "camp_idle_",
    "camp_chicken_"
  };
  [SerializeField]
  public string gd_tag;
  [SerializeField]
  public List<GDPoint> next_gd_points = new List<GDPoint>();
  [SerializeField]
  public Vector3 _pos = Vector3.zero;
  public bool _node_inited;
  [NonSerialized]
  public bool is_graph_waypoint;
  public int idle_animation;
  public Direction direction;
  public string smart_anim_trigger = string.Empty;
  [NonSerialized]
  public bool? default_enable_state;
  public static AstarPath _astar_path;
  public GDPointNode _node;

  [HideInInspector]
  public Vector3 pos
  {
    get
    {
      if (this._pos == Vector3.zero)
        this._pos = this.transform.position;
      return this._pos;
    }
  }

  public void ResetPos() => this._pos = Vector3.zero;

  public bool IsTPPoint() => (double) this.pos.z > 1000.0;

  public static float Distance(GDPoint from, GDPoint to)
  {
    return (UnityEngine.Object) from == (UnityEngine.Object) null || (UnityEngine.Object) to == (UnityEngine.Object) null ? 0.0f : from.transform.position.DistTo(to.transform.position);
  }

  public float Distance(GDPoint to) => GDPoint.Distance(this, to);

  public static AstarPath astar_path
  {
    get => GDPoint._astar_path ?? (GDPoint._astar_path = AstarPath.active);
  }

  public GDPointNode node => this._node;

  public void InitNode()
  {
    if (this._node_inited)
      return;
    this._node_inited = true;
    this._node = new GDPointNode(GDPoint.astar_path, this);
    if (this.next_gd_points == null || this.next_gd_points.Count == 0)
      return;
    foreach (GDPoint nextGdPoint in this.next_gd_points)
    {
      if ((UnityEngine.Object) nextGdPoint == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("Found a null next_gd_point at object name = " + this.name), (UnityEngine.Object) this);
      }
      else
      {
        nextGdPoint.InitNode();
        this._node.LinkAdjacentNode(nextGdPoint.node, (uint) Mathf.CeilToInt(this.Distance(nextGdPoint)));
      }
    }
  }

  public void DeInitNode()
  {
    this._node_inited = false;
    this._node = (GDPointNode) null;
  }

  public static string GetIdlePrefix(GDPoint.IdlePointPrefix prefix_enum)
  {
    return prefix_enum == GDPoint.IdlePointPrefix.None ? string.Empty : GDPoint.IDLE_POINT_PREFIXES[(int) prefix_enum];
  }

  public static bool TryParseIdlePointTag(
    string gd_tag,
    out GDPoint.IdlePointPrefix idle_prefix,
    out int idle_num,
    bool log_errors = false)
  {
    idle_prefix = GDPoint.IdlePointPrefix.None;
    idle_num = 0;
    if (string.IsNullOrEmpty(gd_tag))
    {
      if (log_errors)
        Debug.Log((object) "#ipm# GD_tag is null!");
      return false;
    }
    for (int index = 0; index < GDPoint.IDLE_POINT_PREFIXES.Length; ++index)
    {
      if (gd_tag.StartsWith(GDPoint.IDLE_POINT_PREFIXES[index]))
      {
        string s = gd_tag.Substring(GDPoint.IDLE_POINT_PREFIXES[index].Length);
        idle_prefix = (GDPoint.IdlePointPrefix) index;
        if (int.TryParse(s, out idle_num))
          return true;
        if (log_errors)
          Debug.LogError((object) $"Can not parce num of GD_tag \"{gd_tag}\": prefis={idle_prefix.ToString()}; num={s}");
        return false;
      }
    }
    return false;
  }

  public bool IsDisabled()
  {
    foreach (GDPoint gdPoint in this.gameObject.GetComponentsInParent<GDPoint>(true))
    {
      if ((UnityEngine.Object) gdPoint != (UnityEngine.Object) this && !gdPoint.gameObject.activeSelf)
        return true;
    }
    return false;
  }

  public static void StoreGDPointsState()
  {
    foreach (GDPoint componentsInChild in MainGame.me.world_root.GetComponentsInChildren<GDPoint>(true))
      componentsInChild.default_enable_state = new bool?(componentsInChild.gameObject.activeSelf);
  }

  public static void RestoreGDPointsState()
  {
    foreach (GDPoint componentsInChild in MainGame.me.world_root.GetComponentsInChildren<GDPoint>(true))
    {
      if (componentsInChild.default_enable_state.HasValue)
        componentsInChild.gameObject.SetActive(componentsInChild.default_enable_state.GetValueOrDefault());
    }
  }

  public enum IdlePointPrefix
  {
    None = -1, // 0xFFFFFFFF
    Tavern = 0,
    Village = 1,
    Camp = 2,
    CampChickens = 3,
  }
}
