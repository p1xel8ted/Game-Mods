// Decompiled with JetBrains decompiler
// Type: ControlsGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ControlsGUI : MonoBehaviour
{
  public UILabel control_move;
  public UILabel control_interact;
  public UILabel control_work;
  public UILabel control_atk;
  public UILabel control_dash;
  public UILabel control_gmenu;
  public UILabel control_pause;
  public UILabel control_qslot;
  public UILabel control_tab;
  public UILabel control_map;
  public UIGrid grid;
  public int grid_height_for_keyboard = 15;
  public int grid_height_for_gamepad = 20;
  public int grid_pos_y_for_keyboard = -134;
  public int grid_pos_y_for_gamepad = -147;
  [NonSerialized]
  public bool just_opened;
  public List<ControlKeyLineGUI> redefine_key_item = new List<ControlKeyLineGUI>();

  public void OnEnable()
  {
    foreach (PlatformDependentElement componentsInChild in this.GetComponentsInChildren<PlatformDependentElement>())
      componentsInChild.Init(LazyInput.gamepad_active);
    if ((UnityEngine.Object) this.grid != (UnityEngine.Object) null)
    {
      this.grid.cellHeight = LazyInput.gamepad_active ? (float) this.grid_height_for_gamepad : (float) this.grid_height_for_keyboard;
      this.grid.transform.localPosition = new Vector3(this.grid.transform.localPosition.x, LazyInput.gamepad_active ? (float) this.grid_pos_y_for_gamepad : (float) this.grid_pos_y_for_keyboard);
      this.control_move.text = GameKeyTip.GetIcon(GameKey.Move);
      this.control_interact.text = GameKeyTip.GetIcon(GameKey.Interaction);
      this.control_work.text = GameKeyTip.GetIcon(GameKey.Work);
      this.control_atk.text = GameKeyTip.GetIcon(GameKey.Attack);
      this.control_dash.text = GameKeyTip.GetIcon(GameKey.Dash);
      if (LazyInput.gamepad_active)
      {
        UILabel controlDash = this.control_dash;
        controlDash.text = $"{controlDash.text} / {GameKeyTip.GetIcon(GameKey.Dash2)}";
      }
      this.control_gmenu.text = GameKeyTip.GetIcon(GameKey.GameGUI);
      this.control_tab.text = "I, T, N";
      this.control_map.text = GameKeyTip.GetIcon(GameKey.Map);
      this.control_pause.text = GameKeyTip.GetIcon(GameKey.IngameMenu);
      this.control_qslot.text = GameKeyTip.GetIcon(GameKey.AnyQuickslot);
      this.grid.Reposition();
      this.grid.repositionNow = true;
    }
    this.redefine_key_item.Clear();
    foreach (ControlKeyLineGUI componentsInChild in this.gameObject.GetComponentsInChildren<ControlKeyLineGUI>(true))
    {
      componentsInChild.Redraw();
      componentsInChild.changed = false;
      this.redefine_key_item.Add(componentsInChild);
    }
  }

  public void OnDisable()
  {
    this.just_opened = false;
    bool flag = false;
    foreach (ControlKeyLineGUI controlKeyLineGui in this.redefine_key_item)
      flag |= controlKeyLineGui.changed;
    if (!flag)
      return;
    GameSettings.Save();
  }

  public void ResetKeyBindings()
  {
    KeyBindings.Reset();
    foreach (ControlKeyLineGUI controlKeyLineGui in this.redefine_key_item)
      controlKeyLineGui.Redraw();
  }
}
