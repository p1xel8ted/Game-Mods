// Decompiled with JetBrains decompiler
// Type: GDZone
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Collider2D))]
public class GDZone : MonoBehaviour
{
  public bool _entered;
  public Vector2 _enter_point;
  public GDZone.GDZoneEvent on_enter;
  public GDZone.GDZoneEvent on_exit;
  public GDZone.GDZoneEvent on_crossed_to_right;
  public GDZone.GDZoneEvent on_crossed_to_left;
  public GDZone.GDZoneEvent on_crossed_to_up;
  public GDZone.GDZoneEvent on_crossed_to_down;
  public const float COLLIDER_MIN_SIZE = 100f;
  public List<string> sound_loops = new List<string>();
  public bool _is_player_inside;
  public GDZone.DistanceType distance_counter;
  public Vector2 dist_p1;
  public Vector2 dist_p2;
  public float dist_size = 1f;
  public float dist_fade_size = 2f;
  [SerializeField]
  public Vector2 _p1;
  [SerializeField]
  public Vector2 _p2;
  [SerializeField]
  public Vector2[] _inner_poly;
  [SerializeField]
  public Vector2 _perpendicular;
  [SerializeField]
  public Vector2 _line_direction;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if ((UnityEngine.Object) collision == (UnityEngine.Object) null)
      return;
    WorldGameObject componentInParent = collision.gameObject.GetComponentInParent<WorldGameObject>();
    if ((UnityEngine.Object) componentInParent == (UnityEngine.Object) null || !componentInParent.is_player || this._is_player_inside)
      return;
    this._is_player_inside = true;
    this._enter_point = (Vector2) componentInParent.transform.position;
    Debug.Log((object) $"GDZone.OnTriggerEnter2D {this.name}, obj = {componentInParent.name}", (UnityEngine.Object) this);
    this.ExecuteEvent(this.on_enter);
    if (!string.IsNullOrEmpty(this.on_enter.ovr_music))
      SmartAudioEngine.me.PlayOvrMusic(this.on_enter.ovr_music);
    foreach (string soundLoop in this.sound_loops)
      SmartAudioEngine.me.PlaySoundWithFade(soundLoop);
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    if ((UnityEngine.Object) collision == (UnityEngine.Object) null)
      return;
    WorldGameObject componentInParent = collision.gameObject.GetComponentInParent<WorldGameObject>();
    if ((UnityEngine.Object) componentInParent == (UnityEngine.Object) null || !componentInParent.is_player || !this._is_player_inside)
      return;
    this._is_player_inside = false;
    Vector2 vector2 = (Vector2) componentInParent.transform.position - this._enter_point;
    Debug.Log((object) $"GDZone.OnTriggerExit2D {this.name}, obj = {componentInParent.name}, diff_vector = {vector2.ToString()}", (UnityEngine.Object) this);
    this.ExecuteEvent(this.on_exit);
    if (!string.IsNullOrEmpty(this.on_enter.ovr_music))
      SmartAudioEngine.me.StopOvrMusic(this.on_enter.ovr_music);
    if ((double) vector2.x * 2.0 > 100.0)
      this.ExecuteEvent(this.on_crossed_to_right);
    if ((double) vector2.x * 2.0 < -100.0)
      this.ExecuteEvent(this.on_crossed_to_left);
    if ((double) vector2.y * 2.0 > 100.0)
      this.ExecuteEvent(this.on_crossed_to_down);
    if ((double) vector2.y * 2.0 < -100.0)
      this.ExecuteEvent(this.on_crossed_to_up);
    foreach (string soundLoop in this.sound_loops)
      SmartAudioEngine.me.StopSoundWithFade(soundLoop);
  }

  public void ExecuteEvent(GDZone.GDZoneEvent e)
  {
    if (e == null)
      return;
    if ((UnityEngine.Object) e.flow_script != (UnityEngine.Object) null)
      CustomFlowScript.Create(MainGame.me.world_root.gameObject, e.flow_script, true);
    if (!((UnityEngine.Object) e.environment_preset != (UnityEngine.Object) null))
      return;
    EnvironmentEngine.me.ApplyEnvironmentPreset(e.environment_preset);
  }

  public void Update()
  {
    if (!this._is_player_inside)
      return;
    switch (this.distance_counter)
    {
      case GDZone.DistanceType.Point:
        Debug.LogError((object) "Point not supported");
        break;
      case GDZone.DistanceType.Line:
        float distanceK = this.CalculateDistanceK((Vector2) MainGame.me.player_pos);
        using (List<string>.Enumerator enumerator = this.sound_loops.GetEnumerator())
        {
          while (enumerator.MoveNext())
            SmartAudioEngine.me.SetSoundVolume(enumerator.Current, distanceK);
          break;
        }
    }
  }

  public void RecalculateBounds()
  {
    this._p1 = (Vector2) (this.transform.position + (Vector3) this.dist_p1 * 96f);
    this._p2 = (Vector2) (this.transform.position + (Vector3) this.dist_p2 * 96f);
    this._line_direction = (this._p2 - this._p1).normalized;
    this._perpendicular = (Vector2) (Quaternion.AngleAxis(90f, (Vector3) this._line_direction) * Vector3.forward);
    this._inner_poly = new Vector2[4]
    {
      this._p1 + this._perpendicular * this.dist_size * 96f,
      this._p2 + this._perpendicular * this.dist_size * 96f,
      this._p2 - this._perpendicular * this.dist_size * 96f,
      this._p1 - this._perpendicular * this.dist_size * 96f
    };
  }

  public static float DistanceFromPointToLine(Vector2 point, Vector2 l1, Vector2 l2)
  {
    return Mathf.Abs((float) (((double) l2.x - (double) l1.x) * ((double) l1.y - (double) point.y) - ((double) l1.x - (double) point.x) * ((double) l2.y - (double) l1.y))) / Mathf.Sqrt(Mathf.Pow(l2.x - l1.x, 2f) + Mathf.Pow(l2.y - l1.y, 2f));
  }

  public float CalculateDistanceK(Vector2 p)
  {
    if (ExtentionTools.PolygonContainsPoint(this._inner_poly, p))
      return 1f;
    float num = (float) (1.0 - (double) Mathf.Min(GDZone.DistanceFromPointToLine(p, this._inner_poly[0], this._inner_poly[1]), GDZone.DistanceFromPointToLine(p, this._inner_poly[2], this._inner_poly[3])) / 96.0 / (double) this.dist_fade_size);
    return (double) num >= 0.0 ? num : 0.0f;
  }

  public void OnDrawGizmosSelected()
  {
    this.RecalculateBounds();
    Gizmos.color = Color.red;
    Gizmos.color = Color.blue;
    if ((double) this.dist_fade_size < 0.0)
      this.dist_fade_size = 0.0f;
    switch (this.distance_counter)
    {
      case GDZone.DistanceType.Point:
        Gizmos.DrawWireSphere((Vector3) this._p1, this.dist_size * 96f);
        break;
      case GDZone.DistanceType.Line:
        Gizmos.DrawSphere((Vector3) this._p1, 10f);
        Gizmos.DrawSphere((Vector3) this._p2, 10f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine((Vector3) this._p1, (Vector3) this._p2);
        float num = this.dist_size + this.dist_fade_size;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine((Vector3) (this._p1 + this._perpendicular * num * 96f), (Vector3) (this._p2 + this._perpendicular * num * 96f));
        Gizmos.DrawLine((Vector3) (this._p1 - this._perpendicular * num * 96f), (Vector3) (this._p2 - this._perpendicular * num * 96f));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine((Vector3) this._inner_poly[0], (Vector3) this._inner_poly[1]);
        Gizmos.DrawLine((Vector3) this._inner_poly[1], (Vector3) this._inner_poly[2]);
        Gizmos.DrawLine((Vector3) this._inner_poly[2], (Vector3) this._inner_poly[3]);
        Gizmos.DrawLine((Vector3) this._inner_poly[3], (Vector3) this._inner_poly[0]);
        break;
    }
    Gizmos.color = Color.white;
  }

  [Serializable]
  public class GDZoneEvent
  {
    public FlowGraph flow_script;
    public EnvironmentPreset environment_preset;
    public string ovr_music;
  }

  public enum DistanceType
  {
    None,
    Point,
    Line,
  }
}
