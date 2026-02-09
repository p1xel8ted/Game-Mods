// Decompiled with JetBrains decompiler
// Type: ContextMenuBubbleGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ContextMenuBubbleGUI : WidgetsBubbleGUI
{
  public GJCommons.VoidDelegate _on_hide;
  [NonSerialized]
  public bool is_shown;

  public static ContextMenuBubbleGUI Show(
    string[] options,
    System.Action[] opt_delegates,
    Vector2 mouse_pos,
    GJCommons.VoidDelegate on_hide = null)
  {
    BubbleWidgetDataContainer data = new BubbleWidgetDataContainer(WidgetsBubbleGUI.Alignment.Left, Array.Empty<BubbleWidgetData>());
    data.SetData((BubbleWidgetData) new BubbleWidgetDataOptions(options, opt_delegates));
    return ContextMenuBubbleGUI.Show(data, mouse_pos, on_hide);
  }

  public static ContextMenuBubbleGUI Show(
    BubbleWidgetDataOptions options_data,
    Vector2 mouse_pos,
    GJCommons.VoidDelegate on_hide = null)
  {
    return ContextMenuBubbleGUI.Show(new BubbleWidgetDataContainer(WidgetsBubbleGUI.Alignment.Left, new BubbleWidgetData[1]
    {
      (BubbleWidgetData) options_data
    }), mouse_pos, on_hide);
  }

  public static ContextMenuBubbleGUI Show(
    BubbleWidgetDataContainer data,
    Vector2 mouse_pos,
    GJCommons.VoidDelegate on_hide = null)
  {
    if (data == null || !data.has_data)
    {
      Debug.LogError((object) "no tooltip data");
      return (ContextMenuBubbleGUI) null;
    }
    LazyInput.ClearAllKeysDown();
    ContextMenuBubbleGUI contextMenuBubble = GUIElements.me.context_menu_bubble;
    if (contextMenuBubble.is_shown)
      contextMenuBubble.OnHide();
    BubbleWidgetDataOptions data1 = data.GetData<BubbleWidgetDataOptions>();
    if (data1 != null)
      data1.on_hide = new System.Action(contextMenuBubble.OnHide);
    BaseGUI.on_window_opened += new BaseGUI.OnAnyWindowStateChanged(contextMenuBubble.OnAnyWindowStateChanged);
    BaseGUI.on_window_closed += new BaseGUI.OnAnyWindowStateChanged(contextMenuBubble.OnAnyWindowStateChanged);
    contextMenuBubble.is_shown = true;
    contextMenuBubble.try_show_down = true;
    contextMenuBubble.table.DestroyChildren();
    contextMenuBubble.gameObject.SetActive(true);
    contextMenuBubble.Show(data, true);
    contextMenuBubble._on_hide = on_hide;
    Vector3 worldPoint = MainGame.me.gui_cam.ScreenToWorldPoint(Input.mousePosition);
    contextMenuBubble.UpdateBubble(worldPoint, false);
    return contextMenuBubble;
  }

  public void OnAnyWindowStateChanged(BaseGUI window_obj) => this.OnHide();

  public void OnBackClicked()
  {
    this.OnHide();
    if (!LazyInput.GetKeyDown(GameKey.RightClick))
      return;
    foreach (Collider2D collider2D in NGUIExtensionMethods.GetCollidersUnderMouse(MainGame.me.gui_cam))
    {
      UIEventTrigger component1 = collider2D.GetComponent<UIEventTrigger>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        EventDelegate.Execute(component1.onHoverOver);
        EventDelegate.Execute(component1.onPress);
      }
      UIButton component2 = collider2D.GetComponent<UIButton>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.isEnabled)
        EventDelegate.Execute(component2.onClick);
    }
  }

  public void OnHide()
  {
    this.gameObject.SetActive(false);
    BaseGUI.on_window_opened -= new BaseGUI.OnAnyWindowStateChanged(this.OnAnyWindowStateChanged);
    BaseGUI.on_window_closed -= new BaseGUI.OnAnyWindowStateChanged(this.OnAnyWindowStateChanged);
    this.is_shown = false;
    this._on_hide.TryInvoke();
  }

  public override void Update()
  {
    foreach (BubbleWidgetBase bubbleWidget in this.bubble_widgets)
      bubbleWidget.UpdateWidget();
    if (!LazyInput.AnyKeyDown() || LazyInput.AnyClick())
      return;
    this.OnHide();
  }
}
