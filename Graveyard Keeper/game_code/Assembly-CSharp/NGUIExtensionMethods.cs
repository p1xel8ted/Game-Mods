// Decompiled with JetBrains decompiler
// Type: NGUIExtensionMethods
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class NGUIExtensionMethods
{
  public const float COLOR_DURATION = 0.1f;

  public static void ChangeSize(
    this UIWidget w,
    Vector2 to,
    float duration,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f)
  {
    w.ChangeSize(w.width, w.height, (int) to.x, (int) to.y, duration, on_complete, delay);
  }

  public static void ChangeSize(
    this UIWidget w,
    float to_x,
    float to_y,
    float duration,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f)
  {
    w.ChangeSize(w.width, w.height, (int) to_x, (int) to_y, duration, on_complete, delay);
  }

  public static void ChangeSize(
    this UIWidget w,
    Vector2 from,
    Vector2 to,
    float duration,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f)
  {
    w.ChangeSize((int) from.x, (int) from.y, (int) to.x, (int) to.y, duration, on_complete, delay);
  }

  public static void ChangeSize(
    this UIWidget w,
    int from_x,
    int from_y,
    int to_x,
    int to_y,
    float duration,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f)
  {
    TweenWidth tweenWidth = TweenWidth.Begin(w, duration, to_x);
    tweenWidth.from = from_x;
    tweenWidth.animationCurve = NGUIAnimCurves.me.size;
    TweenHeight tweenHeight = TweenHeight.Begin(w, duration, to_y);
    tweenHeight.from = from_y;
    tweenHeight.animationCurve = NGUIAnimCurves.me.size;
    if ((double) delay > 0.0)
    {
      tweenWidth.delay = delay;
      tweenHeight.delay = delay;
    }
    tweenHeight.SetOnFinished((EventDelegate.Callback) (() =>
    {
      Object.DestroyImmediate((Object) w.GetComponent<TweenWidth>());
      Object.DestroyImmediate((Object) w.GetComponent<TweenHeight>());
      if (on_complete == null)
        return;
      on_complete();
    }));
  }

  public static void ChangeAlpha(
    this UIRect w,
    float from,
    float to,
    float duration = 0.1f,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f,
    bool apply_from_before_delay = true)
  {
    if (duration.EqualsTo(0.0f))
    {
      EasyTimer.VoidDelegate dlgt = (EasyTimer.VoidDelegate) (() =>
      {
        w.alpha = to;
        on_complete.TryInvoke();
      });
      if (delay.EqualsTo(0.0f))
        dlgt();
      else
        EasyTimer.Add(delay, dlgt);
    }
    else
    {
      TweenAlpha tweenAlpha = TweenAlpha.Begin(w.gameObject, duration, to);
      tweenAlpha.from = from;
      if ((double) delay > 0.0)
        tweenAlpha.delay = delay;
      if (apply_from_before_delay)
        w.alpha = from;
      tweenAlpha.animationCurve = NGUIAnimCurves.me.alpha;
      tweenAlpha.SetOnFinished((EventDelegate.Callback) (() =>
      {
        Object.DestroyImmediate((Object) w.GetComponent<TweenAlpha>());
        if (on_complete == null)
          return;
        on_complete();
      }));
    }
  }

  public static void TryFinishAlphaTween(this GameObject go)
  {
    TweenAlpha component1 = go.GetComponent<TweenAlpha>();
    UIRect component2 = go.GetComponent<UIRect>();
    if (!((Object) component1 != (Object) null) || !((Object) component2 != (Object) null))
      return;
    component2.alpha = component1.to;
    component1.DestroyComponent();
  }

  public static void Open(this UIWidget w)
  {
    w.PlaceAtStartPos(true);
    w.Activate<UIWidget>();
  }

  public static void Hide(this UIWidget w)
  {
    w.Deactivate<UIWidget>();
    w.PlaceAtStartPos(false);
  }

  public static void Open(
    this UIWidget w,
    float duration,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f)
  {
    w.Activate<UIWidget>();
    w.PlaceAtStartPos(true);
    Vector3 start_pos = w.transform.localPosition;
    w.ChangePos(w.transform.localPosition - new Vector3(0.0f, -400f, 0.0f), w.transform.localPosition, duration, (GJCommons.VoidDelegate) (() =>
    {
      w.transform.localPosition = start_pos;
      if (on_complete == null)
        return;
      on_complete();
    }), delay);
  }

  public static void Hide(
    this UIWidget w,
    float duration,
    GJCommons.VoidDelegate on_complete = null,
    bool up = false,
    float delay = 0.0f)
  {
    w.Activate<UIWidget>();
    Vector3 start_pos = w.PlaceAtStartPos(true);
    w.ChangePos(w.transform.localPosition, w.transform.localPosition - new Vector3(0.0f, up ? -400f : 400f, 0.0f), duration, (GJCommons.VoidDelegate) (() =>
    {
      w.transform.localPosition = start_pos;
      if (on_complete != null)
        on_complete();
      w.Deactivate<UIWidget>();
    }), delay);
  }

  public static void ChangePos(
    this UIWidget w,
    Vector3 from,
    Vector3 to,
    float duration,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f)
  {
    TweenPosition component = w.GetComponent<TweenPosition>();
    AnimationCurve animationCurve = (Object) component == (Object) null ? NGUIAnimCurves.me.position : component.animationCurve;
    TweenPosition tweenPosition = TweenPosition.Begin(w.gameObject, duration, to);
    tweenPosition.from = from;
    if ((double) delay > 0.0)
      tweenPosition.delay = delay;
    tweenPosition.animationCurve = animationCurve;
    tweenPosition.SetOnFinished((EventDelegate.Callback) (() =>
    {
      if (on_complete == null)
        return;
      on_complete();
    }));
  }

  public static void ChangeColor(
    this UIWidget w,
    Color color,
    float duration = 0.1f,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f,
    bool ignore_alpha = false)
  {
    if (ignore_alpha)
      color.a = w.color.a;
    if (duration.EqualsTo(0.0f))
    {
      w.color = color;
      Object.DestroyImmediate((Object) w.GetComponent<TweenColor>());
      if (on_complete == null)
        return;
      on_complete();
    }
    else
    {
      TweenColor tweenColor = TweenColor.Begin(w.gameObject, duration, color);
      if ((double) delay > 0.0)
        tweenColor.delay = delay;
      tweenColor.animationCurve = NGUIAnimCurves.me.color;
      tweenColor.SetOnFinished((EventDelegate.Callback) (() =>
      {
        Object.DestroyImmediate((Object) w.GetComponent<TweenColor>());
        if (on_complete == null)
          return;
        on_complete();
      }));
    }
  }

  public static void ChangeColor(
    this GameObject go,
    Color color,
    float duration = 0.1f,
    GJCommons.VoidDelegate on_complete = null,
    float delay = 0.0f,
    bool ignore_alpha = false)
  {
  }

  public static void StopTweens(this UIWidget w, bool call_on_completes = false, bool in_children_too = true)
  {
    List<UITweener> uiTweenerList = new List<UITweener>((IEnumerable<UITweener>) w.GetComponents<UITweener>());
    if (uiTweenerList.Count == 0 & in_children_too)
      uiTweenerList.AddRange((IEnumerable<UITweener>) w.GetComponentsInChildren<UITweener>(true));
    while (uiTweenerList.Count > 0)
    {
      if (call_on_completes && uiTweenerList[0].onFinished != null)
      {
        foreach (EventDelegate eventDelegate in uiTweenerList[0].onFinished)
          eventDelegate.Execute();
      }
      Object.DestroyImmediate((Object) uiTweenerList[0]);
      uiTweenerList.RemoveAt(0);
      if (uiTweenerList.Count == 0 & in_children_too)
        uiTweenerList.AddRange((IEnumerable<UITweener>) w.GetComponentsInChildren<UITweener>(true));
    }
  }

  public static void AnimateTransition(
    this UIWidget from,
    UIWidget to,
    float anim_time,
    UIWidget alpha_target = null,
    GJCommons.VoidDelegate on_complete = null)
  {
    if ((double) anim_time < 0.0)
      anim_time = Time.deltaTime;
    to.transform.position = from.transform.position;
    Vector2 start_size = from.localSize;
    to.ChangeSize(from.localSize, to.localSize, anim_time);
    from.ChangeSize(from.localSize, to.localSize, anim_time);
    from.ChangeAlpha(1f, 0.0f, anim_time / 2f);
    if ((Object) alpha_target == (Object) null)
      alpha_target = to;
    alpha_target.ChangeAlpha(0.0f, 1f, anim_time, (GJCommons.VoidDelegate) (() =>
    {
      from.Hide();
      from.width = (int) start_size.x;
      from.height = (int) start_size.y;
      from.alpha = 1f;
      if (on_complete == null)
        return;
      on_complete();
    }));
  }

  public static Vector3 PlaceAtStartPos(this UIWidget w, bool create_start)
  {
    StartWidgetPos component = w.GetComponent<StartWidgetPos>();
    if (!((Object) component == (Object) null))
      return w.transform.localPosition = component.pos;
    return create_start ? (w.gameObject.AddComponent<StartWidgetPos>().pos = w.transform.localPosition) : -1f * Vector3.one;
  }

  public static void DrawAndResize(this UI2DSprite ui_sprite, UnityEngine.Sprite sprite)
  {
    ui_sprite.sprite2D = sprite;
    ui_sprite.ResizeByContent();
  }

  public static void ResizeByContent(this UI2DSprite ui_sprite)
  {
    UnityEngine.Sprite sprite2D = ui_sprite.sprite2D;
    if ((Object) sprite2D == (Object) null)
      return;
    ui_sprite.type = UIBasicSprite.Type.Tiled;
    UI2DSprite ui2Dsprite1 = ui_sprite;
    Rect textureRect = sprite2D.textureRect;
    int num1 = Mathf.RoundToInt(textureRect.width);
    ui2Dsprite1.width = num1;
    UI2DSprite ui2Dsprite2 = ui_sprite;
    textureRect = sprite2D.textureRect;
    int num2 = Mathf.RoundToInt(textureRect.height);
    ui2Dsprite2.height = num2;
  }

  public static Collider2D[] GetCollidersUnderMouse(Camera cam)
  {
    return Physics2D.OverlapPointAll((Vector2) cam.ScreenToWorldPoint(Input.mousePosition), 8192 /*0x2000*/);
  }

  public static void InitEventTriggers(
    MonoBehaviour behaviour,
    EventDelegate.Callback on_over,
    EventDelegate.Callback on_out,
    EventDelegate.Callback on_press,
    bool clear_previous = false)
  {
    if ((Object) behaviour == (Object) null)
      return;
    UIEventTrigger uiEventTrigger = behaviour.GetComponent<UIEventTrigger>();
    if ((Object) uiEventTrigger == (Object) null)
      uiEventTrigger = behaviour.gameObject.AddComponent<UIEventTrigger>();
    if (clear_previous)
    {
      uiEventTrigger.onHoverOver.Clear();
      uiEventTrigger.onHoverOut.Clear();
      uiEventTrigger.onPress.Clear();
    }
    uiEventTrigger.onHoverOver.Add(new EventDelegate(on_over));
    uiEventTrigger.onHoverOut.Add(new EventDelegate(on_out));
    uiEventTrigger.onPress.Add(new EventDelegate(on_press));
  }
}
