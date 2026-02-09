// Decompiled with JetBrains decompiler
// Type: GlobalCraftControlGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GlobalCraftControlGUI : BaseGUI
{
  public static bool is_global_control_active;
  public CraftControlItem craft_control_item_prefab;
  public UIScrollView scroll_view;
  public UITable list_grid;
  public UITable table_grid;
  public UniversalObjectInfoGUI object_info;
  [NonSerialized]
  public List<CraftControlItem> list_items = new List<CraftControlItem>();
  public UITableOrGrid tabs_table;
  public CraftTabGUI craft_tab_prefab;
  public static GlobalCraftControlGUI current_instance;
  public UIWidget header_height_widget;
  public GameObject tabs_go;
  public GameObject no_totem_go;
  public UILabel LB;
  public UILabel RB;
  public UILabel no_crafts;
  public CraftControlItem last_selected;
  public List<string> _tabs_ids = new List<string>();
  public List<CraftTabGUI> _tabs = new List<CraftTabGUI>();
  public string _zone_group;
  public string _cur_tab_id;
  public WorldGameObject builder;
  public List<WorldZone> _zones = new List<WorldZone>();

  public List<WorldZone> zones
  {
    get
    {
      this._zones.Clear();
      for (int index = 0; index < GameBalance.me.world_zones_data.Count; ++index)
      {
        WorldZone zoneById = WorldZone.GetZoneByID(GameBalance.me.world_zones_data[index].id, false);
        if (!((UnityEngine.Object) zoneById == (UnityEngine.Object) null) && !string.IsNullOrEmpty(zoneById.definition.zone_group) && zoneById.definition.zone_group == this._zone_group && !zoneById.IsDisabled())
          this._zones.Add(zoneById);
      }
      return this._zones;
    }
  }

  public override void Init()
  {
    this.craft_control_item_prefab.SetActive(false);
    this.craft_tab_prefab.SetActive(false);
    this.tabs_go.SetActive(false);
    base.Init();
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.builder == (UnityEngine.Object) null)
      return;
    UniversalObjectInfo universalObjectInfo = this.builder.GetUniversalObjectInfo();
    universalObjectInfo.header = GJL.L(this.builder.obj_id);
    universalObjectInfo.descr = "";
    if (this.builder.HasSoulsTotemInZone() && GlobalCraftControlGUI.is_global_control_active && !universalObjectInfo.right_items.ContainsKey("gratitude_as_item"))
      universalObjectInfo.right_items.Add("gratitude_as_item", (int) MainGame.me.player.gratitude_points);
    this.object_info.Draw(universalObjectInfo);
  }

  public void Open(string zone_group)
  {
    if (this.is_shown || this.isActiveAndEnabled)
      return;
    GlobalCraftControlGUI.current_instance = this;
    base.Open();
    GlobalCraftControlGUI.is_global_control_active = true;
    this._zone_group = zone_group;
    this.tabs_table.DestroyChildren<CraftTabGUI>(new CraftTabGUI[1]
    {
      this.craft_tab_prefab
    });
    this.RedrawTabs();
    if (!string.IsNullOrEmpty(this._cur_tab_id) && this._tabs_ids.Contains(this._cur_tab_id))
      this.SwitchTab(this._cur_tab_id);
    else if (this._tabs_ids.Count > 0)
      this.SwitchTab(this._tabs_ids[0]);
    else
      Debug.Log((object) $"#GLOBAL CONTROL# No tabs for group:[{zone_group}]");
    this.header_height_widget.height = this._tabs_ids.Count > 0 ? 82 : 50;
    if ((UnityEngine.Object) this.LB != (UnityEngine.Object) null)
      this.LB.text = GameKeyTip.GetIcon(GameKey.PrevTab);
    if (!((UnityEngine.Object) this.RB != (UnityEngine.Object) null))
      return;
    this.RB.text = GameKeyTip.GetIcon(GameKey.NextTab);
  }

  public override void Open()
  {
    base.Open();
    this.gamepad_controller.ReinitItems(false);
    if ((UnityEngine.Object) this.last_selected != (UnityEngine.Object) null)
      this.gamepad_controller.SetFocusedItem(this.last_selected._gamepad_navigation_item);
    else
      this.gamepad_controller.SetFocusedItem(this.list_items[0]._gamepad_navigation_item);
  }

  public void ResetScroll()
  {
    this.scroll_view.Scroll(0.0f);
    this.scroll_view.currentMomentum = Vector3.zero;
    this.UpdateAllAnchors();
    this.scroll_view.UpdatePosition();
  }

  public void RedrawTabs()
  {
    this._tabs_ids.Clear();
    this._tabs.Clear();
    List<WorldZone> zones = this.zones;
    for (int index = 0; index < zones.Count; ++index)
    {
      if (!zones[index].IsDisabled() && MainGame.me.save.IsWorldZoneKnown(zones[index].id))
        this._tabs_ids.Add(zones[index].id);
    }
    this.tabs_go.SetActive(this._tabs_ids.Count > 1);
    this.table_grid.Reposition();
    this.table_grid.repositionNow = true;
    for (int index = 0; index < this._tabs_ids.Count; ++index)
    {
      string tabsId = this._tabs_ids[index];
      this.builder = zones[index].GetZoneWGOs().Find((Predicate<WorldGameObject>) (w => w.obj_def.interaction_type == ObjectDefinition.InteractionType.Builder));
      if ((UnityEngine.Object) this.builder == (UnityEngine.Object) null)
      {
        Debug.Log((object) $"#GLOBAL CONTROL# Null builder for zone:[{zones[index].id}]");
      }
      else
      {
        CraftTabGUI craftTabGui = this.craft_tab_prefab.Copy<CraftTabGUI>();
        this._tabs.Add(craftTabGui);
        craftTabGui.Draw(this.builder, tabsId, new Action<string>(this.SwitchTab));
      }
    }
  }

  public void SwitchTab(string tab_id)
  {
    this.ClearList();
    this._cur_tab_id = tab_id;
    foreach (CraftTabGUI tab in this._tabs)
      tab.SetSelectedState(tab_id == tab.tab_id);
    List<WorldGameObject> worldGameObjectList = new List<WorldGameObject>();
    List<WorldZone> zones = this.zones;
    WorldZone worldZone = (WorldZone) null;
    for (int index1 = 0; index1 < zones.Count; ++index1)
    {
      if (zones[index1].id == tab_id)
      {
        worldZone = zones[index1];
        List<WorldGameObject> zoneWgOs = zones[index1].GetZoneWGOs();
        for (int index2 = 0; index2 < zoneWgOs.Count; ++index2)
        {
          if (zoneWgOs[index2].obj_def.interaction_type == ObjectDefinition.InteractionType.Craft && zoneWgOs[index2].obj_def.global_craft_control_access != ObjectDefinition.GlobalControlAccess.Ignore || zoneWgOs[index2].obj_def.global_craft_control_access == ObjectDefinition.GlobalControlAccess.ForceAdd)
            worldGameObjectList.Add(zoneWgOs[index2]);
        }
      }
    }
    if ((UnityEngine.Object) worldZone != (UnityEngine.Object) null)
      this.builder = worldZone.GetZoneWGOs().Find((Predicate<WorldGameObject>) (w => w.obj_def.interaction_type == ObjectDefinition.InteractionType.Builder));
    for (int index = 0; index < worldGameObjectList.Count; ++index)
    {
      CraftControlItem craftControlItem = this.craft_control_item_prefab.Copy<CraftControlItem>();
      GJL.EnsureChildLabelsHasCorrectFont(craftControlItem.gameObject, false);
      craftControlItem.Draw(worldGameObjectList[index], this.builder.HasSoulsTotemInZone());
      this.list_items.Add(craftControlItem);
    }
    this.no_crafts.gameObject.SetActive(worldGameObjectList.Count <= 0);
    this.scroll_view.transform.localPosition = new Vector3(this.scroll_view.transform.localPosition.x, 0.0f, 0.0f);
    this.UpdateAllAnchors();
    this.scroll_view.ResetPosition();
    Sounds.OnGUITabClick();
    if ((UnityEngine.Object) worldZone != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.builder == (UnityEngine.Object) null)
        return;
      UniversalObjectInfo universalObjectInfo = this.builder.GetUniversalObjectInfo();
      universalObjectInfo.header = GJL.L(this.builder.obj_id);
      universalObjectInfo.descr = "";
      bool flag = this.builder.HasSoulsTotemInZone();
      if (flag && GlobalCraftControlGUI.is_global_control_active && !universalObjectInfo.right_items.ContainsKey("gratitude_as_item"))
        universalObjectInfo.right_items.Add("gratitude_as_item", (int) MainGame.me.player.gratitude_points);
      this.object_info.Draw(universalObjectInfo);
      this.no_totem_go.SetActive(!flag);
    }
    else
      Debug.Log((object) $"#GLOBAL CONTROL# Null builder for zone:[{tab_id}]");
    this.UpdateAllAnchors();
    this.list_grid.Reposition();
    this.list_grid.repositionNow = true;
    Sounds.OnGUITabClick();
    if (BaseGUI.for_gamepad)
    {
      this.button_tips.PrintClose();
      this.gamepad_controller.ReinitItems(false);
      if (this.list_items.Count <= 0)
        return;
      this.gamepad_controller.SetFocusedItem(this.list_items[0]._gamepad_navigation_item);
    }
    else
      this.ResetScroll();
  }

  public ButtonTipsStr GetButtonTips() => this.button_tips;

  public new void UpdateAllAnchors()
  {
    this.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
    this.GetComponentInParent<UIPanel>().BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
  }

  public void ClearList()
  {
    foreach (CraftControlItem listItem in this.list_items)
    {
      listItem.gameObject.SetActive(false);
      UnityEngine.Object.Destroy((UnityEngine.Object) listItem.gameObject);
    }
    this.list_items.Clear();
    this.list_grid.Reposition();
  }

  public override void OnClosePressed()
  {
    GUIElements.me.game_gui.Open();
    GlobalCraftControlGUI.is_global_control_active = false;
    base.OnClosePressed();
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return base.OnPressedBack();
  }

  public new void OnRightClick() => base.OnRightClick();

  public override bool CanCloseWithRightClick() => true;

  public override bool OnPressedNextTab()
  {
    if (this._tabs_ids.Count < 2)
      return false;
    int num = this._tabs_ids.IndexOf(this._cur_tab_id);
    if (num == -1)
      num = 0;
    int index;
    if ((index = num + 1) >= this._tabs_ids.Count)
      index = 0;
    this.SwitchTab(this._tabs_ids[index]);
    return true;
  }

  public override bool OnPressedPrevTab()
  {
    if (this._tabs_ids.Count < 2)
      return false;
    int num = this._tabs_ids.IndexOf(this._cur_tab_id);
    if (num == -1)
      num = 0;
    int index;
    if ((index = num - 1) < 0)
      index = this._tabs_ids.Count - 1;
    this.SwitchTab(this._tabs_ids[index]);
    return true;
  }
}
