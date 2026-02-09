// Decompiled with JetBrains decompiler
// Type: TimeMachineGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TimeMachineGUI : BaseGUI
{
  public const string CUTSCENE_NUM_KEY = "cutscene_dlc_num";
  [SerializeField]
  public TimeMachineItemGUI _time_machine_item;
  public List<TimeMachineItemGUI> _items;
  public bool _is_data_initialized;
  public const string _TIME_MACHINE_CUSTOM_TAG = "tavern_time_machine";
  public const string _TIME_MACHINE_ACT_ANIM = "activate_long";
  public const string _TIME_MACHINE_DEACT_ANIM = "deactivate_long";

  public List<TimeMachineItemGUI> items => this._items;

  public override void Open()
  {
    base.Open();
    if (!this._is_data_initialized)
      this.InitData();
    List<CutscenesDLCDefinition> cutscenesData = GameBalance.me.cutscenes_data;
    int paramInt = MainGame.me.player.data.GetParamInt("cutscene_dlc_num");
    string str = "dlc_cutscene_" + paramInt.ToString();
    for (int index = 0; index < cutscenesData.Count; ++index)
      this._items[index].CheckEnabled(false);
    if (paramInt != 0)
    {
      bool flag = false;
      for (int index = 0; index < cutscenesData.Count; ++index)
      {
        if (cutscenesData[index].id.Equals(str))
        {
          flag = true;
          this._items[index].CheckEnabled(true);
        }
        else
          this._items[index].CheckEnabled(!flag);
      }
    }
    if (!BaseGUI.for_gamepad)
      return;
    this.SetCustomDirectionForFirstAndLast();
    this.gamepad_controller.ReinitItems(true);
  }

  public override void Hide(bool play_hide_sound = true)
  {
    base.Hide(play_hide_sound);
    if (this._items == null)
      return;
    foreach (TimeMachineItemGUI timeMachineItemGui in this._items)
      timeMachineItemGui.OnMouseOuted();
  }

  public void InitData()
  {
    this._items = new List<TimeMachineItemGUI>();
    this._items.Add(this._time_machine_item);
    List<CutscenesDLCDefinition> cutscenesData = GameBalance.me.cutscenes_data;
    for (int index = 1; index < cutscenesData.Count; ++index)
    {
      TimeMachineItemGUI timeMachineItemGui = this._time_machine_item.Copy<TimeMachineItemGUI>();
      this._items.Add(timeMachineItemGui);
      CutscenesDLCDefinition cutscenesDlcDefinition = cutscenesData[index];
      timeMachineItemGui.Initialize(cutscenesData[index].id, cutscenesData[index].GetIconName(), cutscenesData[index].flow_script, new Action<string>(this.OnCutscenePicked));
    }
    this._time_machine_item.Initialize(cutscenesData[0].id, cutscenesData[0].GetIconName(), cutscenesData[0].flow_script, new Action<string>(this.OnCutscenePicked));
    this._is_data_initialized = true;
  }

  public void OnCutscenePicked(string flowscript)
  {
    Debug.LogWarning((object) ("OnCutscenePicked: " + flowscript));
    this.Hide(true);
    WorldGameObject tavern_machine_wgo = WorldMap.GetWorldGameObjectByCustomTag("tavern_time_machine");
    if ((UnityEngine.Object) tavern_machine_wgo != (UnityEngine.Object) null)
    {
      GS.SetPlayerEnable(false, false);
      tavern_machine_wgo.TriggerSmartAnimation("activate_long");
      ChunkedGameObject ch_obj = tavern_machine_wgo.GetComponent<ChunkedGameObject>();
      ch_obj.always_active = true;
      GJTimer.AddTimer(2.5f, (GJTimer.VoidDelegate) (() => CutsceneManager.ExecuteCutscene(flowscript, (CustomFlowScript.OnFinishedDelegate) (finisded_script =>
      {
        ch_obj.always_active = false;
        tavern_machine_wgo.TriggerSmartAnimation("deactivate_long");
      }))));
    }
    else
      CutsceneManager.ExecuteCutscene(flowscript);
  }

  public override bool OnPressedBack()
  {
    GUIElements.me.time_machine_gui.Hide(true);
    return true;
  }

  public override void OnClosePressed() => this.Hide(true);

  public void SetCustomDirectionForFirstAndLast()
  {
    TimeMachineItemGUI timeMachineItemGui1 = (TimeMachineItemGUI) null;
    TimeMachineItemGUI timeMachineItemGui2 = (TimeMachineItemGUI) null;
    foreach (TimeMachineItemGUI timeMachineItemGui3 in this.items)
    {
      if ((UnityEngine.Object) timeMachineItemGui1 == (UnityEngine.Object) null)
        timeMachineItemGui1 = timeMachineItemGui3;
      timeMachineItemGui2 = timeMachineItemGui3;
    }
    if (!((UnityEngine.Object) timeMachineItemGui1 != (UnityEngine.Object) null) || !((UnityEngine.Object) timeMachineItemGui2 != (UnityEngine.Object) null))
      return;
    timeMachineItemGui1.gamepad_navigation_item.SetCustomDirectionItem(timeMachineItemGui2.gamepad_navigation_item, Direction.Up);
    timeMachineItemGui2.gamepad_navigation_item.SetCustomDirectionItem(timeMachineItemGui1.gamepad_navigation_item, Direction.Down);
  }
}
