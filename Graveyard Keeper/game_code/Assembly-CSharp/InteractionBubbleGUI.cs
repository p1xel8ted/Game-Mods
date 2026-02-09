// Decompiled with JetBrains decompiler
// Type: InteractionBubbleGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class InteractionBubbleGUI : WidgetsBubbleGUI
{
  public const float WIDGET_ALPHA_WHEN_NOT_HIGHLIGHTED = 1f;
  [SerializeField]
  public Color _quality_k_color = Color.cyan;
  public static InteractionBubbleGUI _prefab;
  public static Dictionary<long, InteractionBubbleGUI> _bubbles = new Dictionary<long, InteractionBubbleGUI>();
  public static bool _available = true;
  public long _instance_id;
  public string _additional_line = "";

  public static Color quality_k_color => InteractionBubbleGUI._prefab._quality_k_color;

  public override void Init()
  {
    InteractionBubbleGUI._prefab = this;
    BaseGUI.on_window_opened += (BaseGUI.OnAnyWindowStateChanged) (_param1 =>
    {
      if (!MainGame.game_started)
        return;
      this.ChangeBubblesVisibility();
    });
    BaseGUI.on_window_closed += (BaseGUI.OnAnyWindowStateChanged) (_param1 =>
    {
      if (!MainGame.game_started)
        return;
      this.ChangeBubblesVisibility();
    });
    base.Init();
  }

  public static InteractionBubbleGUI Show(WorldGameObject wgo, BubbleWidgetDataContainer data)
  {
    if ((UnityEngine.Object) InteractionBubbleGUI._prefab == (UnityEngine.Object) null)
      return (InteractionBubbleGUI) null;
    InteractionBubbleGUI bubble = InteractionBubbleGUI.GetBubble(wgo.unique_id, true);
    bubble.linked_tf = wgo.bubble_pos_tf;
    if (wgo.obj_def != null && wgo.obj_def.hint_pos == ObjectDefinition.HintPos.AbovePlayer)
      bubble.linked_tf = MainGame.me.player.bubble_pos_tf;
    bubble.Show(data);
    bubble.RefreshAlign(wgo);
    return bubble;
  }

  public void RefreshAlign(WorldGameObject obj)
  {
    BubbleCornerPoint bubbleCornerPoint = obj.GetBubbleCornerPoint();
    SimpleUITable componentInChildren = this.GetComponentInChildren<SimpleUITable>();
    componentInChildren.alignment = (UnityEngine.Object) bubbleCornerPoint == (UnityEngine.Object) null || bubbleCornerPoint.bubble_custom_align == SimpleUITable.Alignment.NotSet ? SimpleUITable.Alignment.Bottom : bubbleCornerPoint.bubble_custom_align;
    componentInChildren.Reposition();
  }

  public static void ShowAllRemoveBubbles()
  {
    foreach (InteractionBubbleGUI behaviour in InteractionBubbleGUI._bubbles.Values)
    {
      if (behaviour.HasWidgetOfType(typeof (BubbleWidgetProgressData)))
        behaviour.SetActive(true);
    }
  }

  public override void Update()
  {
    if (MainGame.game_starting)
      return;
    if (SpeechBubbleGUI.all.ContainsKey(this._instance_id))
      this.widget.alpha = 0.0f;
    else if ((UnityEngine.Object) this.data?.linked_wgo != (UnityEngine.Object) null && (UnityEngine.Object) MainGame.me.player.components.interaction.nearest != (UnityEngine.Object) null && (UnityEngine.Object) MainGame.me.player.components.interaction.nearest == (UnityEngine.Object) this.data?.linked_wgo)
      this.widget.alpha = 1f;
    else
      this.widget.alpha = 1f;
    base.Update();
  }

  public static InteractionBubbleGUI GetBubble(long instance_id, bool create_if_null = false)
  {
    if ((UnityEngine.Object) InteractionBubbleGUI._prefab == (UnityEngine.Object) null)
      return (InteractionBubbleGUI) null;
    InteractionBubbleGUI behaviour = (InteractionBubbleGUI) null;
    if (InteractionBubbleGUI._bubbles.ContainsKey(instance_id))
      behaviour = InteractionBubbleGUI._bubbles[instance_id];
    else if (create_if_null)
    {
      behaviour = InteractionBubbleGUI._prefab.Copy<InteractionBubbleGUI>();
      behaviour._instance_id = instance_id;
      InteractionBubbleGUI._bubbles.Add(instance_id, behaviour);
      if (InteractionBubbleGUI._available)
        behaviour.Activate<InteractionBubbleGUI>();
      else
        behaviour.Deactivate<InteractionBubbleGUI>();
    }
    return behaviour;
  }

  public static void RemoveBubble(WorldGameObject wgo, bool immediate = false)
  {
    InteractionBubbleGUI.RemoveBubble(wgo.unique_id, immediate);
  }

  public static void RemoveBubble(long instance_id, bool immediate = false)
  {
    if ((UnityEngine.Object) InteractionBubbleGUI._prefab == (UnityEngine.Object) null || !InteractionBubbleGUI._bubbles.ContainsKey(instance_id))
      return;
    InteractionBubbleGUI._bubbles[instance_id].DestroyMe();
  }

  public void ChangeBubblesVisibility()
  {
    bool should_be_active = MainGame.me.player.components.character.control_enabled;
    if (MainGame.me.gui_elements.build_mode_gui.is_shown)
      should_be_active = true;
    InteractionBubbleGUI.ChangeBubblesVisibility(should_be_active);
  }

  public static void ChangeBubblesVisibility(bool should_be_active)
  {
    if (!Application.isPlaying)
      return;
    Debug.Log((object) ("ChangeBubblesVisibility " + should_be_active.ToString()));
    InteractionBubbleGUI._available = should_be_active;
    if (should_be_active)
      MainGame.me.player_char.player.RedrawQualities();
    foreach (MonoBehaviour behaviour in InteractionBubbleGUI._bubbles.Values)
      behaviour.SetActive(should_be_active);
  }

  public static void DestroyAll()
  {
    List<InteractionBubbleGUI> interactionBubbleGuiList = new List<InteractionBubbleGUI>((IEnumerable<InteractionBubbleGUI>) InteractionBubbleGUI._bubbles.Values);
    Debug.Log((object) ("Destroy All bubbles, count = " + interactionBubbleGuiList.Count.ToString()));
    foreach (InteractionBubbleGUI interactionBubbleGui in interactionBubbleGuiList)
      interactionBubbleGui.DestroyMe();
    InteractionBubbleGUI._bubbles.Clear();
    foreach (InteractionBubbleGUI componentsInChild in GUIElements.me.interaction_bubble.transform.parent.GetComponentsInChildren<InteractionBubbleGUI>(true))
    {
      if (!((UnityEngine.Object) componentsInChild == (UnityEngine.Object) GUIElements.me.interaction_bubble))
        componentsInChild.DestroyMe();
    }
  }

  public void DestroyMe()
  {
    this.widget.alpha = 0.0f;
    this.gameObject.Destroy();
    if (!InteractionBubbleGUI._bubbles.ContainsKey(this._instance_id))
      return;
    InteractionBubbleGUI._bubbles.Remove(this._instance_id);
  }

  public override void SortWidgets()
  {
    this.data.data_list.Sort((Comparison<BubbleWidgetData>) ((a, b) =>
    {
      int widgetId1 = (int) a.widget_id;
      int widgetId2 = (int) b.widget_id;
      if (widgetId1 == widgetId2)
        return 0;
      return widgetId1 <= widgetId2 ? -1 : 1;
    }));
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__9_0(BaseGUI _param1)
  {
    if (!MainGame.game_started)
      return;
    this.ChangeBubblesVisibility();
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__9_1(BaseGUI _param1)
  {
    if (!MainGame.game_started)
      return;
    this.ChangeBubblesVisibility();
  }
}
