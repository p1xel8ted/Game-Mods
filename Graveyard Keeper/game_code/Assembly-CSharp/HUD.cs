// Decompiled with JetBrains decompiler
// Type: HUD
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class HUD : MonoBehaviour
{
  public UIProgressBar bar_hp;
  public UIProgressBar bar_energy;
  public UIProgressBar time_meter;
  public UIProgressBar bar_sanity;
  public UILabel day_label;
  public UILabel hint_x;
  public UILabel hint_a;
  public UIPanel panel;
  public UILabel r_label;
  public UILabel g_label;
  public UILabel b_label;
  public ToolbarGUI toolbar;
  public bool _inited;
  public HUDSinIcon[] sin_icons;
  public AnimationCurve time_k_to_lighttime;
  public Transform time_circle_rotating;
  public UIWidget[] appear_at_night;
  public UIWidget[] appear_at_day;
  public UIWidget sin_glow;
  public UIWidget sins_circle;
  [Range(0.0f, 1f)]
  public float test_time;
  public List<UnityEngine.Sprite> _sins_spr_back;
  public List<UnityEngine.Sprite> _sins_spr_active;
  public List<Color> _sins_glow_colors;
  public const float SIN_CIRCLE_ANIM_TIME = 0.13f;
  public UILabel zone_name;
  public UILabel zone_descr;
  public GameObject zone_descr_object;
  public HUDTechPointsBar tech_points_bar;
  public HUDTechTrashCan tech_trash_can;
  public HUDRelationBubble relation_bubble;
  public UILabel version_label;

  public void Init()
  {
    BaseGUI.on_window_opened += new BaseGUI.OnAnyWindowStateChanged(this.OnAnyWindowOpened);
    BaseGUI.on_window_closed += new BaseGUI.OnAnyWindowStateChanged(this.OnAnyWindowClosed);
    this.panel = this.GetComponent<UIPanel>();
    this._inited = true;
    this.Update();
    this.version_label.text = "";
    this.toolbar = this.GetComponentInChildren<ToolbarGUI>(true);
    LazyInput.on_input_changed += new System.Action(this.Redraw);
    this._sins_glow_colors = new List<Color>();
    this._sins_spr_back = new List<UnityEngine.Sprite>();
    this._sins_spr_active = new List<UnityEngine.Sprite>();
    foreach (HUDSinIcon sinIcon in this.sin_icons)
    {
      this._sins_glow_colors.Add(sinIcon.glow_color);
      this._sins_spr_back.Add(sinIcon.spr_back.sprite2D);
      this._sins_spr_active.Add(sinIcon.spr_active.sprite2D);
    }
    this.tech_points_bar = this.GetComponentInChildren<HUDTechPointsBar>(true);
    if ((UnityEngine.Object) this.tech_points_bar != (UnityEngine.Object) null)
      this.tech_points_bar.Init();
    this.tech_trash_can = this.GetComponentInChildren<HUDTechTrashCan>(true);
    if ((UnityEngine.Object) this.tech_trash_can != (UnityEngine.Object) null)
      this.tech_trash_can.Init();
    if (!((UnityEngine.Object) this.relation_bubble != (UnityEngine.Object) null))
      return;
    this.relation_bubble.Init();
  }

  public void Update()
  {
    if (!this._inited || !MainGame.game_started)
      return;
    this.bar_hp.value = MainGame.me.save.GetHPPercentage();
    this.bar_energy.value = MainGame.me.player.energy / (float) MainGame.me.save.max_energy;
    this.bar_sanity.value = MainGame.me.player.sanity / (float) MainGame.me.save.max_sanity;
    this.day_label.text = "day " + MainGame.me.save.day.ToString();
    this.r_label.text = Mathf.RoundToInt(MainGame.me.player.GetParam("r")).ToString();
    this.g_label.text = Mathf.RoundToInt(MainGame.me.player.GetParam("g")).ToString();
    this.b_label.text = Mathf.RoundToInt(MainGame.me.player.GetParam("b")).ToString();
    if ((UnityEngine.Object) this.hint_x != (UnityEngine.Object) null)
      this.hint_x.text = !MainGame.me.player_char.has_overhead ? "(X) - attack/use tool" : "(X) - drop";
    this.RedrawTime(TimeOfDay.me.GetTimeK());
  }

  public void UpdateZoneInfo(string name, string description)
  {
    this.zone_name.text = name;
    this.zone_descr.text = description;
    this.zone_descr_object.SetActive(!string.IsNullOrEmpty(description));
  }

  public void Redraw()
  {
    if (!this.gameObject.activeSelf)
      return;
    this.toolbar.Redraw();
    this.RedrawSinsIcons();
  }

  public void Open()
  {
    Debug.Log((object) "HUD show");
    this.gameObject.SetActive(GUIElements.me.hud_enabled);
    GUIElements.me.buffs_panel.gameObject.SetActive(true);
    this.Redraw();
  }

  public void Hide()
  {
    Debug.Log((object) "HUD hide");
    this.gameObject.SetActive(false);
    GUIElements.me.buffs_panel.gameObject.SetActive(false);
  }

  public void OnAnyWindowOpened(BaseGUI gui)
  {
    if (BaseGUI.all_guis_closed)
      return;
    this.gameObject.TryFinishAlphaTween();
    this.Hide();
  }

  public void OnAnyWindowClosed(BaseGUI gui)
  {
    if (!BaseGUI.all_guis_closed || !MainGame.game_started)
      return;
    this.Open();
  }

  public void RedrawTime(float time)
  {
    this.time_meter.value = time;
    float f = this.time_k_to_lighttime.Evaluate(time) - 0.5f;
    this.time_circle_rotating.rotation = Quaternion.Euler(0.0f, 0.0f, 360f * f);
    float num = Mathf.Abs(f) * 2f;
    foreach (UIRect uiRect in this.appear_at_day)
      uiRect.alpha = 1f - num;
    foreach (UIRect uiRect in this.appear_at_night)
      uiRect.alpha = num;
  }

  public void OnValidate()
  {
    if (Application.isPlaying)
      return;
    this.RedrawTime(this.test_time);
  }

  public void OnEndOfDay()
  {
    this.sins_circle.transform.DORotate(new Vector3(0.0f, 0.0f, -60f), 0.13f).OnComplete<Tweener>((TweenCallback) (() =>
    {
      this.sins_circle.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
      this.RedrawSinsIcons();
    }));
    if ((double) this.sin_glow.alpha > 0.0)
      DOTween.To((DOGetter<float>) (() => this.sin_glow.alpha), (DOSetter<float>) (x => this.sin_glow.alpha = x), 0.0f, 0.13f);
    this.Update();
  }

  public void RedrawSinsIcons()
  {
    int index1 = 0;
    Sins.SinType sin = Sins.SinType.Envy;
    for (int index2 = 0; index2 < this.sin_icons.Length; ++index2)
    {
      int index3 = (9 - MainGame.me.save.day_of_week + index2) % 6;
      HUDSinIcon sinIcon = this.sin_icons[index2];
      Sins.SinType sinType = (Sins.SinType) (index3 + 1);
      if (index2 == 3)
      {
        index1 = index3;
        sin = sinType;
      }
      int sin_type = (int) sinType;
      UnityEngine.Sprite back = this._sins_spr_back[index3];
      UnityEngine.Sprite active = this._sins_spr_active[index3];
      Color sinsGlowColor = this._sins_glow_colors[index3];
      sinIcon.Draw((Sins.SinType) sin_type, back, active, sinsGlowColor);
    }
    this.sin_glow.color = this._sins_glow_colors[index1];
    if (!MainGame.me.save.GetSinState(sin))
      return;
    DOTween.To((DOGetter<float>) (() => this.sin_glow.alpha), (DOSetter<float>) (x => this.sin_glow.alpha = x), 1f, 0.13f);
  }

  public void ToolbarSetEnabled(bool enabled = true)
  {
    this.toolbar.gameObject.SetActive(enabled);
    if (!((UnityEngine.Object) this.tech_trash_can != (UnityEngine.Object) null))
      return;
    this.tech_trash_can.gameObject.SetActive(enabled);
  }

  [CompilerGenerated]
  public void \u003COnEndOfDay\u003Eb__42_0()
  {
    this.sins_circle.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    this.RedrawSinsIcons();
  }

  [CompilerGenerated]
  public float \u003COnEndOfDay\u003Eb__42_1() => this.sin_glow.alpha;

  [CompilerGenerated]
  public void \u003COnEndOfDay\u003Eb__42_2(float x) => this.sin_glow.alpha = x;

  [CompilerGenerated]
  public float \u003CRedrawSinsIcons\u003Eb__43_0() => this.sin_glow.alpha;

  [CompilerGenerated]
  public void \u003CRedrawSinsIcons\u003Eb__43_1(float x) => this.sin_glow.alpha = x;
}
