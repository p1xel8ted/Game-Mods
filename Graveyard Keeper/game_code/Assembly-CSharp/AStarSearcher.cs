// Decompiled with JetBrains decompiler
// Type: AStarSearcher
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AStarSearcher
{
  public Seeker _seeker;
  public Path _path;
  public MovementComponent _movement_comp;
  public bool _finding;
  public Vector2 _dest;
  public GJCommons.VoidDelegate _on_completed;
  public GJCommons.VoidDelegate _on_failed;

  public Seeker seeker => this._seeker;

  public bool finding => this._finding;

  public Vector2 destination => this._dest;

  public List<Vector3> path => this._path != null ? this._path.vectorPath : (List<Vector3>) null;

  public bool not_avaible => this._path == null || (Object) this._seeker == (Object) null;

  public AStarSearcher(MovementComponent movement_component)
  {
    this._movement_comp = movement_component;
    this._seeker = this._movement_comp.wgo.gameObject.GetComponent<Seeker>();
    if (!((Object) this._seeker == (Object) null))
      return;
    Debug.Log((object) "Seeker not found, adding", (Object) this._movement_comp.wgo);
    this._seeker = this._movement_comp.wgo.gameObject.AddComponent<Seeker>();
    AStarSearcher.AddDefaultSeekerModifier(this._movement_comp.wgo.gameObject);
  }

  public void EnablePathSmoother(bool enabled)
  {
    SimpleSmoothModifierXY componentInChildren = this._movement_comp.wgo.GetComponentInChildren<SimpleSmoothModifierXY>();
    if ((Object) componentInChildren == (Object) null)
      Debug.LogError((object) "No path smoother found", (Object) this._movement_comp.wgo);
    else
      componentInChildren.enabled = enabled;
  }

  public void Find(
    Vector2 dest,
    GJCommons.VoidDelegate on_completed,
    GJCommons.VoidDelegate on_failed,
    int graph_mask = 1)
  {
    this._finding = true;
    this._dest = dest;
    this._on_completed = on_completed;
    this._on_failed = on_failed;
    this.seeker.StartPath((Vector3) this._movement_comp.wgo.pos, (Vector3) this._dest, new OnPathDelegate(this.OnPathComplete), graph_mask);
  }

  public static void AddDefaultSeekerModifier(GameObject go, bool is_player = false)
  {
    SimpleSmoothModifierXY smoothModifierXy = go.AddComponent<SimpleSmoothModifierXY>();
    smoothModifierXy.subdivisions = 2;
    smoothModifierXy.iterations = 12;
    smoothModifierXy.strength = 5f;
    smoothModifierXy.uniformLength = false;
  }

  public void SetDest(Vector2 dest)
  {
    this._dest = dest;
    this._on_completed = (GJCommons.VoidDelegate) null;
    this._on_failed = (GJCommons.VoidDelegate) null;
  }

  public void Find(
    Vector2 start_pos,
    Vector2 dest,
    GJCommons.VoidDelegate on_completed,
    GJCommons.VoidDelegate on_failed,
    int graph_mask = 1)
  {
    this._finding = true;
    this._dest = dest;
    this._on_completed = on_completed;
    this._on_failed = on_failed;
    this.seeker.StartPath((Vector3) start_pos, (Vector3) this._dest, new OnPathDelegate(this.OnPathComplete), graph_mask);
  }

  public void Clear()
  {
    this._finding = false;
    this._path = (Path) null;
    this._dest = Vector2.zero;
    this._on_completed = this._on_failed = (GJCommons.VoidDelegate) null;
  }

  public void OnPathComplete(Path p)
  {
    if (!this._finding)
      return;
    if (p.error)
    {
      Debug.Log((object) "#path# Path finding failed", (Object) this._movement_comp.wgo);
      this.OnPathFailed(p.errorLog);
    }
    else
    {
      float magnitude = (p.path.LastElement<GraphNode>().position.ToVector2() - this._dest).magnitude;
      if (this._movement_comp.wgo.is_player && (double) magnitude > 17.0)
      {
        this.OnPathFailed("end point is too far, mag = " + magnitude.ToString());
      }
      else
      {
        this._finding = false;
        this._path = p;
        this._on_completed.TryInvoke();
      }
    }
  }

  public void OnPathFailed(string error_message)
  {
    Debug.LogWarning((object) error_message, (Object) this._movement_comp.wgo.gameObject);
    this._on_failed.TryInvoke();
    this.Clear();
  }

  public void RestoreSerialized(Vector2 dest) => this._dest = dest;
}
