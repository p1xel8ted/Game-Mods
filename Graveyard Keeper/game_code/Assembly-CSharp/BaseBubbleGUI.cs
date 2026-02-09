// Decompiled with JetBrains decompiler
// Type: BaseBubbleGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BaseBubbleGUI : MonoBehaviour
{
  public const int LD = 0;
  public const int RD = 1;
  public const int LU = 2;
  public const int RU = 3;
  public const int BOTTOM_CENTER = 4;
  [Range(0.0f, 0.3f)]
  public float alpha_anim_time = 0.2f;
  [Header("ld, rd, lu, ru, (bc)")]
  public GameObject[] corners;
  [HideInInspector]
  [SerializeField]
  public Transform tf;
  public bool rigth = true;
  public bool up = true;
  public bool try_show_to_left;
  public bool try_show_down;
  public int current_corner_index;
  public Transform linked_tf;
  public UI2DSprite back_spr;
  [SerializeField]
  public List<BubbleCornerPoint> points;
  [NonSerialized]
  public BubbleCornerPoint current_point;
  [SerializeField]
  public UIWidget[] all_widgets;
  [SerializeField]
  public bool initialized;
  [NonSerialized]
  public bool serialized;
  [NonSerialized]
  public string txt;
  [NonSerialized]
  public bool try_bottom_center;
  public Vector2 offset;
  public GJCommons.VoidDelegate on_disappeared;

  public void DebugRecalcShifts() => this.RecalcShifts();

  public virtual void Init()
  {
    this.tf = this.transform;
    this.points = new List<BubbleCornerPoint>();
    foreach (GameObject corner in this.corners)
    {
      BubbleCornerPoint componentInChildren = corner.GetComponentInChildren<BubbleCornerPoint>(true);
      componentInChildren.Init(this.tf);
      this.points.Add(componentInChildren);
    }
    this.current_point = this.points[0];
    this.all_widgets = this.GetComponentsInChildren<UIWidget>(true);
    this.gameObject.SetActive(false);
    this.initialized = true;
  }

  public void RecalcShifts(bool for_hints = false)
  {
    foreach (UIWidget allWidget in this.all_widgets)
    {
      if (for_hints)
        allWidget.UpdateAnchors();
      allWidget.Update();
    }
    foreach (BubbleCornerPoint point in this.points)
      point.CalcShift(MainGame.me.gui_cam);
  }

  public virtual void UpdateBubble(
    Vector3 world_pos,
    bool use_world_cam,
    Vector3 alternative_world_pos = default (Vector3),
    bool ignore_halfres_magic = false)
  {
    Camera world_cam = use_world_cam ? MainGame.me.world_cam : MainGame.me.gui_cam;
    Vector2 vector2_1 = this.points[0].pixel_shift - this.points[3].pixel_shift;
    Vector2 screenPoint = (Vector2) world_cam.WorldToScreenPoint(world_pos);
    bool flag = (double) alternative_world_pos.magnitude > 0.0;
    Vector2 vector2_2 = (Vector2) (flag ? world_cam.WorldToScreenPoint(alternative_world_pos) : Vector3.zero);
    Vector2 vector2_3 = screenPoint + vector2_1;
    Vector2 vector2_4 = (flag ? vector2_2 : screenPoint) - vector2_1;
    this.rigth = !this.try_show_to_left && (double) vector2_3.x < (double) Screen.width;
    this.up = !this.try_show_down ? (double) vector2_3.y < (double) Screen.height : (double) vector2_4.y < 0.0;
    this.UpdateCornerPoint();
    if ((UnityEngine.Object) this.current_point == (UnityEngine.Object) null)
      return;
    if (!this.up & flag)
      world_pos = alternative_world_pos;
    this.tf.SetGUIPosToWorldPos(world_pos, world_cam, MainGame.me.gui_cam, this.current_point.shift, !ignore_halfres_magic);
    if ((double) this.offset.magnitude <= 0.0)
      return;
    this.tf.localPosition += (Vector3) this.offset;
  }

  public void OnContentChanged(bool is_hint = false) => this.RecalcShifts();

  public virtual void DestroyBubble() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public virtual void LateUpdate()
  {
    if (!((UnityEngine.Object) this.linked_tf != (UnityEngine.Object) null))
      return;
    this.UpdateBubble(this.linked_tf.position, true);
  }

  public void UpdateCornerPoint()
  {
    int num = this.up ? (this.rigth ? 0 : 1) : (this.rigth ? 2 : 3);
    if (this.try_bottom_center && this.corners.Length > 4)
      num = 4;
    if (this.current_corner_index == num && !((UnityEngine.Object) this.current_point == (UnityEngine.Object) null))
      return;
    this.current_corner_index = num;
    for (int index = 0; index < this.corners.Length; ++index)
    {
      this.corners[index].SetActive(index == num);
      if (index == num)
        this.current_point = this.points[index];
    }
  }

  public void StartDisappear()
  {
    if (this.alpha_anim_time.EqualsTo(0.0f))
    {
      this.DestroyBubble();
      this.on_disappeared.TryInvoke();
    }
    else
    {
      UIWidget component = this.GetComponent<UIWidget>();
      component.ChangeAlpha(component.alpha, 0.0f, this.alpha_anim_time, (GJCommons.VoidDelegate) (() =>
      {
        this.DestroyBubble();
        this.on_disappeared.TryInvoke();
      }));
    }
  }

  public void StartAppear()
  {
    this.GetComponent<UIWidget>().ChangeAlpha(0.0f, 1f, this.alpha_anim_time);
  }

  [CompilerGenerated]
  public void \u003CStartDisappear\u003Eb__32_0()
  {
    this.DestroyBubble();
    this.on_disappeared.TryInvoke();
  }
}
