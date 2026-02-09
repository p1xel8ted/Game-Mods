// Decompiled with JetBrains decompiler
// Type: PorterStationGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PorterStationGUI : BaseGameGUI
{
  public BodyPanelGUI body_panel;
  public InventoryWidget inventory_widget;
  public UniversalObjectInfoGUI universal_info;
  public UIScrollView scroll_view;
  public PorterStation _station;
  public WorldGameObject _wgo;

  public override void Init()
  {
    base.Init();
    this.body_panel.skull_bar.on_enable_skulls_frame += new System.Action(this.OnSkullsOver);
    this.body_panel.skull_bar.on_disable_skulls_frame += new System.Action(this.OnSkullsOut);
  }

  public void Open(WorldGameObject porter_station_wgo)
  {
    this.Open();
    this.universal_info.Draw(porter_station_wgo.GetUniversalObjectInfo());
    this._station = porter_station_wgo.porter_station;
    this._wgo = porter_station_wgo;
    Item data = new Item() { inventory_size = 999 };
    WorldZone myWorldZone = porter_station_wgo.GetMyWorldZone();
    if ((UnityEngine.Object) myWorldZone == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "World zone is null for porter station", (UnityEngine.Object) porter_station_wgo);
    }
    else
    {
      List<string> stringList = new List<string>();
      foreach (TransportPathsDefinition transportPath in GameBalance.me.transport_paths)
      {
        if (!(transportPath.source_zone_id != myWorldZone.id) && !(transportPath.station_wgo_id != porter_station_wgo.obj_id) && !(transportPath.destination_zone_id != porter_station_wgo.porter_station?.destination?.id))
          stringList = transportPath.transport_items;
      }
      foreach (string str in stringList)
      {
        data.inventory.Add(new Item(str, 1));
        if (GameBalance.me.GetData<ItemDefinition>(str).is_big)
          data.inventory.Add(new Item());
      }
      foreach (Item obj in data.inventory)
        obj.value = this._station.blacklist.Contains(obj.id) ? 2 : 1;
      data.inventory_size = data.inventory.Count;
      this.inventory_widget.Open(new Inventory(data, "transfer_items"), BaseGUI.for_gamepad, dont_show_empty_rows: true);
      foreach (BaseItemCellGUI componentsInChild in this.inventory_widget.gameObject.GetComponentsInChildren<BaseItemCellGUI>())
      {
        componentsInChild.SetCallbacks((BaseItemCellGUI.OnItemAction) (t => this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close())), (BaseItemCellGUI.OnItemAction) null, new BaseItemCellGUI.OnItemAction(this.OnItemClicked));
        this.RedrawItemSelectionState(componentsInChild);
        if ((UnityEngine.Object) componentsInChild?.container?.container == (UnityEngine.Object) null || !componentsInChild.container.container.activeInHierarchy)
        {
          GamepadNavigationItem component = componentsInChild.GetComponent<GamepadNavigationItem>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            UnityEngine.Object.Destroy((UnityEngine.Object) component);
        }
      }
      this.scroll_view.ResetPosition();
      this.body_panel.DrawWorker(porter_station_wgo.linked_worker);
      if (!porter_station_wgo.has_linked_worker)
        this.body_panel.spr_body.enabled = false;
      if (this._station.HasLinkedWorker() && this._station.state == PorterStation.PorterState.Waiting)
      {
        if (BaseGUI.for_gamepad)
          this.body_panel.btn_remove_body.GetComponent<GamepadNavigationItem>().SetCallbacks((GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.TakeWorker));
      }
      else
        this.body_panel.btn_remove_body.SetActive(false);
      MainGame.SetPausedMode(true);
      if (!BaseGUI.for_gamepad)
        return;
      this.gamepad_controller.ReinitItems(true);
    }
  }

  public void OnItemClicked(BaseItemCellGUI itm)
  {
    if (++itm.item.value > 2)
      itm.item.value = 1;
    this.RedrawItemSelectionState(itm);
  }

  public void RedrawItemSelectionState(BaseItemCellGUI itm)
  {
    itm.container.counter.text = itm.item.value == 1 ? "(check)" : "";
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this.inventory_widget.ClearItems();
    MainGame.SetPausedMode(false);
    base.Hide(play_hide_sound);
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    if ((UnityEngine.Object) this._station == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "PorterStationGUI.OnClosePressed error: _station is null!");
    }
    else
    {
      List<string> stringList = new List<string>();
      if (this.inventory_widget?.inventory_data?.inventory == null)
      {
        Debug.LogError((object) "PorterStationGUI.OnClosePressed error: inventory is null!");
      }
      else
      {
        foreach (Item obj in this.inventory_widget.inventory_data.inventory)
        {
          if (obj.value != 1)
            stringList.Add(obj.id);
        }
      }
      this._station.blacklist = stringList;
    }
    this.Hide(true);
  }

  public void TakeWorker()
  {
    if (!this._wgo.has_linked_worker)
    {
      Debug.LogError((object) "TakeWorker error: WGO has not linked worker!");
    }
    else
    {
      WorldGameObject linkedWorker = this._wgo.linked_worker;
      if ((UnityEngine.Object) linkedWorker == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "TakeWorker error: worker_wgo is null!");
      }
      else
      {
        WorldMap.RemoveZombieWorkerToStock(linkedWorker);
        Item overheadItem = linkedWorker.worker.GetOverheadItem();
        if (MainGame.me.player_char.has_overhead)
          MainGame.me.player_char.DropOverheadItem();
        MainGame.me.player_char.SetOverheadItem(overheadItem);
        this.OnClosePressed();
      }
    }
  }

  public void OnSkullsOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Close(), GameKeyTip.RightStick(text: "move_tip"));
  }

  public void OnSkullsOut()
  {
    int num = BaseGUI.for_gamepad ? 1 : 0;
  }

  [CompilerGenerated]
  public void \u003COpen\u003Eb__7_0(BaseItemCellGUI t)
  {
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
  }
}
