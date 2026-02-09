// Decompiled with JetBrains decompiler
// Type: VisibilitySector
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class VisibilitySector : MonoBehaviour
{
  public const int DELTA_STEP = 20;
  public float size;
  [Range(0.0f, 1f)]
  public float start_width;
  [Range(0.0f, 360f)]
  public int angle;
  [Range(0.0f, 180f)]
  public int delta;
  [Space]
  public bool draw_debug = true;
  [SerializeField]
  public List<Vector2> raw_dots = new List<Vector2>();
  [SerializeField]
  public List<Vector2> dots = new List<Vector2>();
  [SerializeField]
  [HideInInspector]
  public PolygonCollider2D _col;
  public BaseCharacterComponent _ch;
  public float _angle;
  public Transform _tf;
  public bool _tf_cached;
  public bool _ch_initialized;

  public Transform tf
  {
    get
    {
      return !this._tf_cached ? this.Cache<Transform>(out this._tf, out this._tf_cached, false) : this._tf;
    }
  }

  public Vector2 pos => (Vector2) this.tf.position;

  public void OnValidate()
  {
    this.delta = this.delta / 20 * 20;
    this.SetRawPositions();
  }

  public void Init(BaseCharacterComponent ch)
  {
    this._ch = ch;
    this._ch_initialized = this._ch != null;
  }

  public bool IsTouching(Vector3 other_pos, bool ignore_obstacles)
  {
    this.CalcPositions(this._ch_initialized ? this._ch.anim_dir_angle : 0.0f);
    if (!this._col.OverlapPoint((Vector2) other_pos))
      return false;
    if (ignore_obstacles)
      return true;
    Vector2 vector2 = (Vector2) (other_pos - this.transform.position);
    return Physics2D.RaycastAll(this.pos, vector2.normalized, vector2.magnitude, 1).Length == 0;
  }

  public void SetRawPositions()
  {
    Vector2 vector2_1 = Vector2.left * this.start_width / 2f;
    Vector2 vector2_2 = Vector2.right * this.start_width / 2f;
    if (this.raw_dots == null)
      this.raw_dots = new List<Vector2>();
    else
      this.raw_dots.Clear();
    this.raw_dots.Add(vector2_1);
    List<Vector2> vector2List1 = new List<Vector2>();
    List<Vector2> vector2List2 = new List<Vector2>();
    Vector2 vec = Vector2.down * this.size;
    for (float a = 0.0f; (double) a <= (double) (this.delta - 20); a += 20f)
    {
      vector2List2.Add(this.Rotate(vec, this.Sin(-a), this.Cos(-a)) + vector2_1);
      vector2List1.Add(this.Rotate(vec, this.Sin(a), this.Cos(a)) + vector2_2);
    }
    vector2List2.Add(this.Rotate(vec, this.Sin((float) -this.delta), this.Cos((float) -this.delta)) + vector2_1);
    vector2List1.Add(this.Rotate(vec, this.Sin((float) this.delta), this.Cos((float) this.delta)) + vector2_2);
    for (int index = vector2List2.Count - 1; index >= 0; --index)
      this.raw_dots.Add(vector2List2[index]);
    foreach (Vector2 vector2_3 in vector2List1)
      this.raw_dots.Add(vector2_3);
    this.raw_dots.Add(vector2_2);
    this._angle = float.MinValue;
  }

  public void CalcPositions(float obj_angle)
  {
    if (this._angle.EqualsTo((float) this.angle + obj_angle))
      return;
    this._angle = (float) this.angle + obj_angle;
    if (this.dots == null)
      this.dots = new List<Vector2>();
    else
      this.dots.Clear();
    float sin = this.Sin(this._angle);
    float cos = this.Cos(this._angle);
    foreach (Vector2 rawDot in this.raw_dots)
      this.dots.Add(this.Rotate(rawDot, Vector2.zero, sin, cos));
    this._col = this.GetComponent<PolygonCollider2D>();
    if ((UnityEngine.Object) this._col == (UnityEngine.Object) null)
      this._col = this.gameObject.AddComponent<PolygonCollider2D>();
    this._col.isTrigger = true;
    this._col.points = this.dots.ToArray();
    for (int index = 0; index < this.dots.Count; ++index)
      this.dots[index] = this.ToWorldPos(this.dots[index]);
  }

  public float Sin(float a) => Mathf.Sin(a * ((float) Math.PI / 180f));

  public float Cos(float a) => Mathf.Cos(a * ((float) Math.PI / 180f));

  public Vector2 ToWorldPos(Vector2 vec) => vec * 96f + this.pos;

  public Vector2 Rotate(Vector2 vec, float sin, float cos)
  {
    return new Vector2((float) ((double) vec.x * (double) cos - (double) vec.y * (double) sin), (float) ((double) vec.x * (double) sin + (double) vec.y * (double) cos));
  }

  public Vector2 Rotate(Vector2 vec, Vector2 axis, float sin, float cos)
  {
    return axis + new Vector2((float) (((double) vec.x - (double) axis.x) * (double) cos - ((double) vec.y - (double) axis.y) * (double) sin), (float) (((double) vec.x - (double) axis.x) * (double) sin + ((double) vec.y - (double) axis.y) * (double) cos));
  }

  public void DrawDebug()
  {
    Color magenta = Color.magenta;
    int count = this.dots.Count;
    for (int index = 0; index < count - 1; ++index)
      Debug.DrawLine((Vector3) this.dots[index], (Vector3) this.dots[index + 1], magenta);
    if (count <= 2)
      return;
    Debug.DrawLine((Vector3) this.dots[count - 1], (Vector3) this.dots[0], magenta);
  }
}
