// Decompiled with JetBrains decompiler
// Type: EffectBubblesManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EffectBubblesManager : BaseGUI
{
  public const int FRAMES_DELAY = 3;
  public const float PERIOD = 0.5f;
  public const float DELAY_BUBBLES_IN_ROW = 1f;
  public static EffectBubblesManager _instance;
  public static Vector3 _last_pos;
  public static float _last_bubble_time;
  public EffectBubbleGUI effect_bubble_prefab;
  public static List<StackedWgoBubblesData> _stacked_bubbles = new List<StackedWgoBubblesData>();
  public static List<EffectBubbleGUI> _all_bubbles = new List<EffectBubbleGUI>();
  public static Camera _world_cam;
  public static Camera _gui_cam;
  public Color[] colors;

  public override void Init()
  {
    this.effect_bubble_prefab.Deactivate<EffectBubbleGUI>();
    EffectBubblesManager._instance = this;
    BaseGUI.on_window_opened += (BaseGUI.OnAnyWindowStateChanged) (_param1 => this.ChangeBubblesVisibility());
    BaseGUI.on_window_closed += (BaseGUI.OnAnyWindowStateChanged) (_param1 => this.ChangeBubblesVisibility());
    EffectBubblesManager._world_cam = MainGame.me.world_cam;
    EffectBubblesManager._gui_cam = MainGame.me.gui_cam;
  }

  public static void RemoveAllBubbles(bool stacked_too = true)
  {
    while (EffectBubblesManager._all_bubbles.Count > 0)
      EffectBubblesManager.RemoveBubble(EffectBubblesManager._all_bubbles[0]);
    if (!stacked_too)
      return;
    EffectBubblesManager._stacked_bubbles.Clear();
  }

  public static void RemoveBubble(EffectBubbleGUI effect_bubble)
  {
    EffectBubblesManager._all_bubbles.Remove(effect_bubble);
    effect_bubble.DestroyGO<EffectBubbleGUI>();
  }

  public new void Update()
  {
    float deltaTime = Time.deltaTime;
    for (int index = 0; index < EffectBubblesManager._stacked_bubbles.Count; ++index)
    {
      StackedWgoBubblesData stackedBubble = EffectBubblesManager._stacked_bubbles[index];
      bool flag = false;
      if ((Object) stackedBubble.wgo == (Object) null)
        flag = true;
      else if (stackedBubble.frames_delay > 0)
      {
        if (--stackedBubble.frames_delay == 0)
          stackedBubble.TryToShowBubble();
      }
      else
      {
        stackedBubble.period_delay -= deltaTime;
        if ((double) stackedBubble.period_delay <= 0.0)
        {
          stackedBubble.TryToShowBubble();
          if ((double) stackedBubble.period_delay < -0.5)
            flag = true;
        }
      }
      if (flag)
      {
        EffectBubblesManager._stacked_bubbles.RemoveAt(index);
        --index;
      }
    }
  }

  public new void LateUpdate()
  {
    foreach (EffectBubbleGUI allBubble in EffectBubblesManager._all_bubbles)
      allBubble.UpdateBubble();
  }

  public void ChangeBubblesVisibility()
  {
    EffectBubblesManager.ChangeBubblesVisibility(BaseGUI.all_guis_closed);
  }

  public static void ChangeBubblesVisibility(bool should_be_active)
  {
    foreach (MonoBehaviour allBubble in EffectBubblesManager._all_bubbles)
      allBubble.SetActive(should_be_active);
  }

  public static void ShowImmediately(
    Vector3 position,
    string text,
    EffectBubblesManager.BubbleColor color = EffectBubblesManager.BubbleColor.White,
    bool ignore_timescale = true,
    float custom_time = -1f,
    bool is_gui_pos = false)
  {
    if (string.IsNullOrEmpty(text))
      Debug.LogError((object) "Can't show effect bubble for empty string");
    else if (EffectBubblesManager._last_pos == position && (double) Time.time - (double) EffectBubblesManager._last_bubble_time < 0.25)
    {
      GJTimer.AddTimer(1f, (GJTimer.VoidDelegate) (() => EffectBubblesManager.ShowImmediately(position, text, color, ignore_timescale, custom_time, is_gui_pos)));
    }
    else
    {
      Debug.Log((object) ("ShowEffectBubble: " + text));
      if (is_gui_pos)
        position = EffectBubblesManager._world_cam.ScreenToWorldPoint(EffectBubblesManager._gui_cam.WorldToScreenPoint(position));
      EffectBubblesManager._last_pos = position;
      EffectBubblesManager._last_bubble_time = Time.time;
      EffectBubbleGUI effectBubbleGui = EffectBubblesManager._instance.effect_bubble_prefab.Copy<EffectBubbleGUI>();
      effectBubbleGui.InitEffect(position, text, color == EffectBubblesManager.BubbleColor.White ? Color.white : EffectBubblesManager._instance.colors[(int) color], ignore_timescale, custom_time);
      EffectBubblesManager._all_bubbles.Add(effectBubbleGui);
      BuffsLogics.CheckBuffsGiveConditions();
    }
  }

  public static void ShowImmediately(
    Vector3 position,
    GameRes res,
    bool ignore_timescale = true,
    float custom_time = -1f,
    bool is_gui_pos = false)
  {
    if (res.IsEmpty())
      return;
    EffectBubblesManager.ShowImmediately(position, res.ToFormattedString(), ignore_timescale: ignore_timescale, custom_time: custom_time, is_gui_pos: is_gui_pos);
  }

  public static void ShowImmediately(
    WorldGameObject wgo,
    GameRes res,
    bool ignore_timescale = true,
    float custom_time = -1f)
  {
    EffectBubblesManager.ShowImmediately(wgo.bubble_pos, res.ToFormattedString(), ignore_timescale: ignore_timescale, custom_time: custom_time);
  }

  public static void ShowStacked(WorldGameObject wgo, GameRes res, Vector3? custom_pos = null)
  {
    if (res.IsEmpty())
      return;
    long uniqueId = wgo.unique_id;
    foreach (StackedWgoBubblesData stackedBubble in EffectBubblesManager._stacked_bubbles)
    {
      if (stackedBubble.id == uniqueId)
      {
        stackedBubble.AddRes(res);
        if (!custom_pos.HasValue)
          return;
        stackedBubble.custom_pos = custom_pos;
        return;
      }
    }
    EffectBubblesManager._stacked_bubbles.Add(new StackedWgoBubblesData(uniqueId, wgo, res, custom_pos));
  }

  public static void RemoveStacked(WorldGameObject wgo)
  {
    for (int index = 0; index < EffectBubblesManager._stacked_bubbles.Count; ++index)
    {
      if ((Object) EffectBubblesManager._stacked_bubbles[index].wgo == (Object) wgo)
      {
        EffectBubblesManager._stacked_bubbles.RemoveAt(index);
        break;
      }
    }
  }

  public static void ShowStackedHP(WorldGameObject wgo, float value)
  {
    WorldGameObject wgo1 = wgo;
    GameRes res = new GameRes();
    res.hp = value;
    Vector3? custom_pos = new Vector3?();
    EffectBubblesManager.ShowStacked(wgo1, res, custom_pos);
  }

  public static void ShowStackedEnergy(WorldGameObject wgo, float value)
  {
    EffectBubblesManager.ShowStacked(wgo, new GameRes("energy", value));
  }

  public static void ShowStackedSanity(WorldGameObject wgo, float value)
  {
    EffectBubblesManager.ShowStacked(wgo, new GameRes("sanity", value));
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__13_0(BaseGUI _param1) => this.ChangeBubblesVisibility();

  [CompilerGenerated]
  public void \u003CInit\u003Eb__13_1(BaseGUI _param1) => this.ChangeBubblesVisibility();

  public enum BubbleColor
  {
    White = -1, // 0xFFFFFFFF
    Red = 0,
    Green = 1,
    Energy = 2,
    Sanity = 3,
    Relation = 4,
  }
}
