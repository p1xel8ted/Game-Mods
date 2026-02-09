// Decompiled with JetBrains decompiler
// Type: TooltipsManager
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TooltipsManager : MonoBehaviour
{
  public static TooltipsManager _instance;
  public Tooltip current_tooltip;
  public bool _skip_update;

  public static TooltipsManager me
  {
    get
    {
      return TooltipsManager._instance ?? (TooltipsManager._instance = Object.FindObjectOfType<TooltipsManager>());
    }
  }

  public void Awake() => TooltipsManager._instance = this;

  public void Update()
  {
    Tooltip overedTooltip = this.FindOveredTooltip();
    if ((Object) this.current_tooltip == (Object) overedTooltip)
      return;
    if ((Object) this.current_tooltip != (Object) null)
      this.current_tooltip.Hide();
    this.current_tooltip = overedTooltip;
    if ((Object) this.current_tooltip == (Object) null)
      return;
    this.current_tooltip.Show(true);
  }

  public Tooltip FindOveredTooltip()
  {
    if ((Object) BaseGUI.active_gui == (Object) null || GUIElements.me.context_menu_bubble.is_shown)
      return (Tooltip) null;
    Vector2 point;
    if (BaseGUI.for_gamepad)
    {
      if (!((Object) GamepadNavigationController.current != (Object) null) || !((Object) GamepadNavigationController.current.focused_item != (Object) null))
        return (Tooltip) null;
      point = GamepadNavigationController.current.focused_item.pos;
    }
    else
      point = (Vector2) MainGame.me.gui_cam.ScreenToWorldPoint(Input.mousePosition);
    Collider2D[] collider2DArray = Physics2D.OverlapPointAll(point, 8192 /*0x2000*/);
    int num = 0;
    foreach (Collider2D collider2D in collider2DArray)
    {
      UIWidget component1 = collider2D.GetComponent<UIWidget>();
      int depth;
      if ((Object) component1 != (Object) null && (Object) component1.panel != (Object) null)
      {
        depth = component1.panel.depth;
      }
      else
      {
        UIPanel component2 = collider2D.GetComponent<UIPanel>();
        if (!((Object) component2 == (Object) null))
          depth = component2.depth;
        else
          continue;
      }
      if (depth > num)
        num = depth;
    }
    foreach (Component component in collider2DArray)
    {
      if ((Object) component.GetComponent<TooltipBlocker>() != (Object) null)
        return (Tooltip) null;
    }
    foreach (Component component3 in collider2DArray)
    {
      Tooltip component4 = component3.GetComponent<Tooltip>();
      if (!((Object) component4 == (Object) null) && component4.gameObject.activeInHierarchy && component4.available && component4.has_info && !component4.IsScrolling() && !component4.IsClippedByScrollView())
      {
        UIWidget component5 = component4.GetComponent<UIWidget>();
        if (!((Object) component5 == (Object) null) && !((Object) component5.panel == (Object) null) && component5.panel.depth >= num)
          return component4;
      }
    }
    return (Tooltip) null;
  }

  public static void Redraw()
  {
    if (TooltipsManager._instance._skip_update)
      return;
    Tooltip currentTooltip = TooltipsManager._instance.current_tooltip;
    if ((Object) currentTooltip != (Object) null)
      currentTooltip.Hide();
    TooltipsManager._instance.current_tooltip = (Tooltip) null;
    TooltipsManager._instance.Update();
  }
}
