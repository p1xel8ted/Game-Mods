// Decompiled with JetBrains decompiler
// Type: CraftGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CraftGUI : BaseCraftGUI, CraftGUIInterface, CraftInterface
{
  public System.Action ON_CRAFT_GUI_HIDE;
  [HideInInspector]
  public CraftItemGUI craft_item_prefab;
  [HideInInspector]
  public UIScrollView scroll_view;
  public UITable craft_list_grid;
  [NonSerialized]
  public List<CraftItemGUI> list_items = new List<CraftItemGUI>();
  public Vector3 _panel_start_pos;
  public List<Item> _bodies = new List<Item>();
  public static ObjectCraftDefinition _remove_building_craft;
  public CraftItemGUI _exanded_item;
  public UITableOrGrid tabs_table;
  public CraftTabGUI craft_tab_prefab;
  public List<string> _tabs_ids = new List<string>();
  public List<CraftTabGUI> _tabs = new List<CraftTabGUI>();
  public UIWidget header_height_widget;
  public GameObject tabs_go;
  public bool has_amount_buttons;
  public GameObject mixed_craft_go;
  public CraftInterface custom_craft_interface;
  public UILabel LB;
  public UILabel RB;
  public const bool OLD_ALCHEMY_CRAFT = false;
  public CraftQueueGUI queue;
  public UILabel gamepad_hint_queue_big;
  public bool _gamepad_in_queue_area;
  public bool _gamepad_was_in_queue_area;
  public bool hide_after_build_gui;
  public static bool in_redraw_mode;
  public static CraftGUI _current_instance;

  public override void Init()
  {
    this.craft_item_prefab = this.GetComponentInChildren<CraftItemGUI>(true);
    this.craft_item_prefab.Init();
    if ((UnityEngine.Object) this.craft_list_grid == (UnityEngine.Object) null)
      this.craft_list_grid = this.GetComponentInChildren<UITable>();
    this.scroll_view = this.GetComponentInChildren<UIScrollView>(true);
    this._panel_start_pos = this.scroll_view.transform.localPosition;
    if ((UnityEngine.Object) this.tabs_go != (UnityEngine.Object) null)
    {
      this.craft_tab_prefab.SetActive(false);
      this.tabs_go.SetActive(false);
    }
    this.queue?.Init(this);
    base.Init();
  }

  public void OpenAsBuild(WorldGameObject build_desk, CraftsInventory crafts_inventory)
  {
    this.hide_after_build_gui = true;
    this.custom_craft_interface = (CraftInterface) null;
    this.has_amount_buttons = false;
    this.Open(build_desk, crafts_inventory);
  }

  public void OpenCraftList(WorldGameObject craftery_wgo)
  {
    this.custom_craft_interface = (CraftInterface) null;
    this.has_amount_buttons = true;
    this.Open(craftery_wgo, (CraftsInventory) null);
  }

  public void OpenAsRatCell(WorldGameObject rat_cell_wgo, Item rat_item)
  {
    CraftsInventory crafts_inventory = new CraftsInventory()
    {
      is_building = false,
      craft_type = CraftDefinition.CraftType.RatBuff
    };
    foreach (CraftDefinition craftDefinition in GameBalance.me.craft_data)
    {
      if (craftDefinition.craft_type == CraftDefinition.CraftType.RatBuff)
      {
        bool flag1 = true;
        if (craftDefinition.item_needs.Count > 0)
        {
          flag1 = rat_item.IsEnoughItems(craftDefinition.item_needs);
          foreach (Item itemNeed in craftDefinition.item_needs)
          {
            if (!rat_item.HasItemInInventory(itemNeed.id))
            {
              flag1 = false;
              break;
            }
          }
        }
        if (flag1)
        {
          bool flag2 = true;
          if (craftDefinition.item_output.Count > 0)
          {
            foreach (Item obj in craftDefinition.item_output)
            {
              if (rat_item.HasItemInInventory(obj.id))
              {
                flag2 = false;
                break;
              }
            }
          }
          if (flag2)
            crafts_inventory.AddCraft(craftDefinition.id);
        }
      }
    }
    this.craftery_wgo = rat_cell_wgo;
    this.Open(this.craftery_wgo, crafts_inventory);
  }

  public void OpenAsGrave(
    WorldGameObject grave_wgo,
    Item grave_part,
    ItemDefinition.ItemType grave_part_type)
  {
    if (grave_part == null)
      grave_part = Item.empty;
    string str;
    switch (grave_part_type)
    {
      case ItemDefinition.ItemType.GraveStone:
        str = "cross";
        break;
      case ItemDefinition.ItemType.GraveFence:
        str = "fence";
        break;
      default:
        Debug.LogError((object) ("Unsupported grave part type: " + grave_part_type.ToString()));
        return;
    }
    CraftsInventory crafts_inventory = new CraftsInventory()
    {
      additional_crafts = new List<ObjectCraftDefinition>()
    };
    Item fix_price;
    CraftDefinition fixCraftAndPrice = grave_part.GetFixCraftAndPrice(out fix_price);
    ObjectCraftDefinition objectCraftDefinition1 = new ObjectCraftDefinition();
    objectCraftDefinition1.id = "fix_grave_craft_" + str;
    objectCraftDefinition1.icon = "i_b_hammer";
    objectCraftDefinition1.needs = fix_price.inventory;
    objectCraftDefinition1.enabled = fix_price.inventory.Count > 0;
    objectCraftDefinition1.energy = fixCraftAndPrice.energy;
    ObjectCraftDefinition objectCraftDefinition2 = objectCraftDefinition1;
    crafts_inventory.additional_crafts.Add(objectCraftDefinition2);
    CraftDefinition removeCraftForItem = GameBalance.me.GetRemoveCraftForItem(grave_wgo.obj_id, grave_part.id);
    List<ObjectCraftDefinition> additionalCrafts = crafts_inventory.additional_crafts;
    ObjectCraftDefinition objectCraftDefinition3 = new ObjectCraftDefinition();
    objectCraftDefinition3.id = removeCraftForItem.id;
    objectCraftDefinition3.icon = "i_b_remove";
    objectCraftDefinition3.needs = removeCraftForItem.needs;
    objectCraftDefinition3.craft_time = removeCraftForItem.craft_time;
    objectCraftDefinition3.custom_name = "_remove_";
    additionalCrafts.Add(objectCraftDefinition3);
    this.craftery_wgo = grave_wgo;
    this.custom_craft_interface = (CraftInterface) GUIElements.me.grave;
    this.Open(grave_wgo, crafts_inventory);
    UniversalObjectInfo universalObjectInfo = grave_wgo.GetUniversalObjectInfo();
    universalObjectInfo.header = grave_part.IsEmpty() ? GJL.L("no_grave_" + str) : GJL.L(grave_part.id);
    universalObjectInfo.descr = "";
    if (!grave_part.IsEmpty())
    {
      universalObjectInfo.descr = string.Format("+(wr){0}, " + grave_part.GetDurabilityHint(), (object) grave_part.definition.GetQualityString(grave_part));
      universalObjectInfo.icon = "i_" + grave_part.id;
      universalObjectInfo.icon_color = "739ECCFF";
    }
    this.object_info.Draw(universalObjectInfo);
  }

  public void OpenAsOrganEnhancer(WorldGameObject craftery_wgo, Item item, System.Action on_gui_hide = null)
  {
    this.Hide(true);
    int craftIndex1 = OrganEnhancerGUI.GetCraftIndex(craftery_wgo, "total_white_skulls");
    int craftIndex2 = OrganEnhancerGUI.GetCraftIndex(craftery_wgo, "total_red_skulls");
    Item itemForCraftOutput1 = OrganEnhancerGUI.GetModifiedItemForCraftOutput(item, true);
    Item itemForCraftOutput2 = OrganEnhancerGUI.GetModifiedItemForCraftOutput(item, false);
    CraftDefinition dataOrNull1 = GameBalance.me.GetDataOrNull<CraftDefinition>($"organ_enhance_w_{craftIndex1}");
    CraftDefinition craftDefinition1 = new CraftDefinition();
    craftDefinition1.id = dataOrNull1.id;
    craftDefinition1.craft_in = new List<string>()
    {
      "soul_workbench"
    };
    craftDefinition1.sanity = new SmartExpression();
    craftDefinition1.disable_multi_craft = dataOrNull1.disable_multi_craft;
    craftDefinition1.icon = $"i_{item.id.Split(':')[0]}_w_upgrd";
    craftDefinition1.condition = dataOrNull1.condition;
    craftDefinition1.needs = dataOrNull1.needs;
    craftDefinition1.energy = dataOrNull1.energy;
    craftDefinition1.craft_time = dataOrNull1.craft_time;
    craftDefinition1.enqueue_type = dataOrNull1.enqueue_type;
    craftDefinition1.output = new List<Item>()
    {
      itemForCraftOutput1
    };
    CraftDefinition craftDefinition2 = craftDefinition1;
    foreach (Item obj in craftDefinition2.output)
    {
      obj.min_value = new SmartExpression();
      obj.max_value = new SmartExpression();
      obj.self_chance = new SmartExpression()
      {
        default_value = 1f
      };
    }
    CraftDefinition dataOrNull2 = GameBalance.me.GetDataOrNull<CraftDefinition>($"organ_enhance_r_{craftIndex2}");
    CraftDefinition craftDefinition3 = new CraftDefinition();
    craftDefinition3.id = dataOrNull2.id;
    craftDefinition3.craft_in = new List<string>()
    {
      "soul_workbench"
    };
    craftDefinition3.sanity = new SmartExpression();
    craftDefinition3.disable_multi_craft = dataOrNull2.disable_multi_craft;
    craftDefinition3.icon = $"i_{item.id.Split(':')[0]}_r_upgrd";
    craftDefinition3.condition = dataOrNull2.condition;
    craftDefinition3.needs = dataOrNull2.needs;
    craftDefinition3.energy = dataOrNull2.energy;
    craftDefinition3.craft_time = dataOrNull2.craft_time;
    craftDefinition3.enqueue_type = dataOrNull2.enqueue_type;
    craftDefinition3.output = new List<Item>()
    {
      itemForCraftOutput2
    };
    CraftDefinition craftDefinition4 = craftDefinition3;
    foreach (Item obj in craftDefinition4.output)
    {
      obj.min_value = new SmartExpression();
      obj.max_value = new SmartExpression();
      obj.self_chance = new SmartExpression()
      {
        default_value = 1f
      };
    }
    List<CraftDefinition> craftDefinitionList = new List<CraftDefinition>();
    if (craftDefinition2.condition.EvaluateBoolean(craftery_wgo, MainGame.me.player))
      craftDefinitionList.Add(craftDefinition2);
    if (craftDefinition4.condition.EvaluateBoolean(craftery_wgo, MainGame.me.player))
      craftDefinitionList.Add(craftDefinition4);
    craftery_wgo.components.craft.crafts = craftDefinitionList;
    this.custom_craft_interface = (CraftInterface) GUIElements.me.organ_enhancer_gui;
    this.Open(craftery_wgo, (CraftsInventory) null);
    this.ON_CRAFT_GUI_HIDE = on_gui_hide;
  }

  public void Open(
    WorldGameObject craftery_wgo,
    CraftsInventory crafts_inventory,
    string add_custom_tab = null)
  {
    if (this.is_shown || this.isActiveAndEnabled)
      return;
    CraftGUI._current_instance = this;
    CraftItemGUI.last_item_pressed = (CraftItemGUI) null;
    this.crafts_inventory = crafts_inventory;
    CraftGUI.in_redraw_mode = false;
    this._gamepad_was_in_queue_area = this._gamepad_in_queue_area = false;
    if ((UnityEngine.Object) this.mixed_craft_go != (UnityEngine.Object) null)
      this.mixed_craft_go.SetActive(false);
    CraftComponent craft = craftery_wgo?.components?.craft;
    if (craft != null && craft.is_crafting && craft.current_craft != null && !craft.current_craft.CanEnqueue())
      CraftGUI.NeedToCancelCraftsDialog(craft, (System.Action) (() => this.ProceedOpen(craftery_wgo, crafts_inventory, add_custom_tab)));
    else
      this.ProceedOpen(craftery_wgo, crafts_inventory, add_custom_tab);
  }

  public void ProceedOpen(
    WorldGameObject craftery_wgo,
    CraftsInventory crafts_inventory,
    string add_custom_tab = null)
  {
    this.CommonOpen(craftery_wgo, crafts_inventory == null ? CraftDefinition.CraftType.None : crafts_inventory.craft_type);
    this.scroll_view.gameObject.SetActive(true);
    Debug.Log((object) ("Open craft window, crafts: " + this.crafts.Count.ToString()));
    bool flag1 = false;
    this._tabs_ids.Clear();
    foreach (CraftDefinition craft in this.crafts)
    {
      CraftItemGUI craftItemGui1 = this.ExistingCraftItemWithSimilarCraft(craft);
      if ((UnityEngine.Object) craftItemGui1 != (UnityEngine.Object) null)
      {
        craftItemGui1.AddOneMoreCraftDefinition(craft);
      }
      else
      {
        CraftItemGUI craftItemGui2 = this.craft_item_prefab.Copy<CraftItemGUI>();
        GJL.EnsureChildLabelsHasCorrectFont(craftItemGui2.gameObject, false);
        craftItemGui2.Draw(craft);
        this.list_items.Add(craftItemGui2);
        if (!string.IsNullOrEmpty(craft.tab_id))
        {
          if (!this._tabs_ids.Contains(craft.tab_id))
            this._tabs_ids.Add(craft.tab_id);
        }
        else
          flag1 = true;
      }
    }
    if (this._tabs_ids.Count > 0 & flag1)
      this._tabs_ids.Insert(0, "");
    if (!string.IsNullOrEmpty(add_custom_tab))
    {
      this._tabs_ids.Insert(0, "?" + add_custom_tab);
      if (this._tabs_ids.Count == 1 && this.crafts.Count > 0)
        this._tabs_ids.Add("");
    }
    this.RedrawCraftTabs();
    if (craftery_wgo.obj_def.interaction_type == ObjectDefinition.InteractionType.Builder)
      this.AddRemoveBuildingItem();
    this.UpdateAllAnchors();
    this.craft_list_grid.Reposition();
    this.craft_list_grid.repositionNow = true;
    if (BaseGUI.for_gamepad)
    {
      this.button_tips.PrintClose();
      CraftItemGUI lastCraftedItem = this.GetLastCraftedItem();
      if ((UnityEngine.Object) lastCraftedItem == (UnityEngine.Object) null)
        this.ResetScroll();
      this.gamepad_controller.ReinitItems(false);
      this.gamepad_controller.SetFocusedItem((UnityEngine.Object) lastCraftedItem == (UnityEngine.Object) null ? (GamepadNavigationItem) null : lastCraftedItem.gamepad_navigation_item, false);
    }
    else
      this.ResetScroll();
    this.SetCustomDirectionForFirstAndLast();
    if (this._tabs_ids.Count > 0)
    {
      bool flag2 = false;
      foreach (string tabsId in this._tabs_ids)
      {
        if (tabsId == craftery_wgo.last_opened_tab)
        {
          flag2 = true;
          this.SwitchTab(tabsId);
          break;
        }
      }
      if (!flag2)
        this.SwitchTab(this._tabs_ids[0]);
    }
    this.scroll_view.ResetPosition();
    if ((UnityEngine.Object) this.LB != (UnityEngine.Object) null)
      this.LB.text = GameKeyTip.GetIcon(GameKey.PrevTab);
    if ((UnityEngine.Object) this.RB != (UnityEngine.Object) null)
      this.RB.text = GameKeyTip.GetIcon(GameKey.NextTab);
    this.queue?.Draw(craftery_wgo?.components?.craft);
  }

  public void RedrawCraftTabs()
  {
    if ((UnityEngine.Object) this.tabs_go == (UnityEngine.Object) null)
      return;
    this.tabs_go.SetActive(this._tabs_ids.Count > 0);
    this.tabs_table.DestroyChildren<CraftTabGUI>(new CraftTabGUI[1]
    {
      this.craft_tab_prefab
    });
    this.header_height_widget.height = this._tabs_ids.Count > 0 ? 82 : 50;
    this._tabs.Clear();
    foreach (string tabsId in this._tabs_ids)
    {
      CraftTabGUI craftTabGui = this.craft_tab_prefab.Copy<CraftTabGUI>();
      this._tabs.Add(craftTabGui);
      craftTabGui.Draw(this.craftery_wgo, tabsId, new Action<string>(this.SwitchTab));
    }
    this.tabs_table.Reposition();
  }

  public void SwitchTab(string tab_id)
  {
    if ((UnityEngine.Object) this._exanded_item != (UnityEngine.Object) null)
      this.CollapseItem();
    this.craftery_wgo.last_opened_tab = tab_id;
    foreach (CraftItemGUI listItem in this.list_items)
    {
      listItem.SetActive(listItem.craft_definition.tab_id == tab_id || listItem.craft_definition.id == "_remove_");
      if (listItem.craft_definition.id == "_remove_")
        this.SetCustomDirectionForFirstAndLast(tab_id);
    }
    bool flag = false;
    foreach (CraftTabGUI tab in this._tabs)
    {
      tab.SetSelectedState(tab_id == tab.tab_id);
      if (tab.tab_id.StartsWith("?"))
        flag = true;
    }
    if (flag)
    {
      if (tab_id.StartsWith("?"))
      {
        this.scroll_view.gameObject.SetActive(false);
        this.mixed_craft_go.SetActive(true);
        GUIElements.me.mixed_craft_tabbed.OpenAsAlchemy(this.craftery_wgo, this.craftery_wgo.obj_def.craft_preset);
        if (BaseGUI.for_gamepad)
          this.button_tips.Deactivate<ButtonTipsStr>();
      }
      else
      {
        int num = this.scroll_view.gameObject.activeInHierarchy ? 1 : 0;
        this.scroll_view.gameObject.SetActive(true);
        this.mixed_craft_go.SetActive(false);
        GUIElements.me.mixed_craft_tabbed.Hide(false);
        if (BaseGUI.for_gamepad)
          this.button_tips.Activate<ButtonTipsStr>();
        if (num == 0 && BaseGUI.for_gamepad)
          this.gamepad_controller.Enable();
      }
    }
    this.craft_list_grid.Reposition();
    this.scroll_view.transform.localPosition = new Vector3(this.scroll_view.transform.localPosition.x, 0.0f, 0.0f);
    this.UpdateAllAnchors();
    this.scroll_view.ResetPosition();
    Sounds.OnGUITabClick();
    if (!BaseGUI.for_gamepad)
      return;
    this.gamepad_controller.ReinitItems(true);
  }

  public void OpenAsAlchemy(WorldGameObject craftery_wgo)
  {
    this.custom_craft_interface = (CraftInterface) null;
    CraftsInventory crafts_inventory = new CraftsInventory()
    {
      is_building = false,
      craft_type = CraftDefinition.CraftType.MixedCraft
    };
    foreach (string completedOneTimeCraft in MainGame.me.save.completed_one_time_crafts)
    {
      CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>(completedOneTimeCraft);
      if (dataOrNull != null && dataOrNull.craft_in.Contains(craftery_wgo.obj_id))
        crafts_inventory.AddCraft(completedOneTimeCraft);
    }
    this.has_amount_buttons = true;
    this.Open(craftery_wgo, crafts_inventory, "alch");
  }

  public CraftItemGUI ExistingCraftItemWithSimilarCraft(CraftDefinition craft_definition)
  {
    if (craft_definition.output.Count == 0)
      return (CraftItemGUI) null;
    string id = craft_definition.output[0].id;
    ItemDefinition dataOrNull = GameBalance.me.GetDataOrNull<ItemDefinition>(id);
    if (dataOrNull != null && dataOrNull.quality_type != ItemDefinition.QualityType.Stars)
      return (CraftItemGUI) null;
    int count = craft_definition.needs.Count;
    bool flag = craft_definition.IsMultiqualityOutput();
    foreach (CraftItemGUI listItem in this.list_items)
    {
      CraftDefinition currentCraft = listItem.current_craft;
      if (currentCraft.needs.Count == count && currentCraft.IsMultiqualityOutput() == flag && currentCraft.output.Count != 0 && !(currentCraft.output[0].id != id))
        return listItem;
    }
    return (CraftItemGUI) null;
  }

  public void AddRemoveBuildingItem()
  {
    if (CraftGUI._remove_building_craft == null)
    {
      ObjectCraftDefinition objectCraftDefinition = new ObjectCraftDefinition();
      objectCraftDefinition.id = "_remove_";
      objectCraftDefinition.icon = "i_b_remove";
      CraftGUI._remove_building_craft = objectCraftDefinition;
    }
    CraftItemGUI craftItemGui = this.craft_item_prefab.Copy<CraftItemGUI>();
    GJL.EnsureChildLabelsHasCorrectFont(craftItemGui.gameObject, false);
    craftItemGui.Draw((CraftDefinition) CraftGUI._remove_building_craft);
    this.list_items.Add(craftItemGui);
  }

  public override bool OnCraft(
    CraftDefinition craft,
    Item try_use_particular_item = null,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null,
    WorldGameObject other_obj_override = null)
  {
    other_obj_override = (WorldGameObject) null;
    if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this.craftery_wgo != (UnityEngine.Object) null)
    {
      WorldZone myWorldZone = this.craftery_wgo.GetMyWorldZone();
      if ((UnityEngine.Object) myWorldZone != (UnityEngine.Object) null && !myWorldZone.IsPlayerInZone())
        other_obj_override = this.craftery_wgo;
    }
    return base.OnCraft(craft, try_use_particular_item, multiquality_ids, amount, other_obj_override: other_obj_override);
  }

  public CraftItemGUI GetLastCraftedItem() => (CraftItemGUI) null;

  public void ResetScroll()
  {
    this.scroll_view.Scroll(0.0f);
    this.scroll_view.currentMomentum = Vector3.zero;
    this.UpdateAllAnchors();
    this.scroll_view.UpdatePosition();
  }

  public override void Hide(bool play_hide_sound = true)
  {
    if (DOTween.IsTweening((object) this.scroll_view.transform))
      this.scroll_view.transform.DOKill();
    this.scroll_view.StopScrolling();
    this.ClearList();
    base.Hide(play_hide_sound);
    if ((UnityEngine.Object) GUIElements.me.mixed_craft_tabbed != (UnityEngine.Object) null && GUIElements.me.mixed_craft_tabbed.is_shown)
      GUIElements.me.mixed_craft_tabbed.Hide(false);
    if (this.hide_after_build_gui)
    {
      MainGame.paused = false;
      this.hide_after_build_gui = false;
    }
    System.Action onCraftGuiHide = this.ON_CRAFT_GUI_HIDE;
    if (onCraftGuiHide != null)
      onCraftGuiHide();
    this.ON_CRAFT_GUI_HIDE = (System.Action) null;
  }

  public override void OnAboveWindowClosed()
  {
    if (!BaseGUI.for_gamepad)
      return;
    if ((UnityEngine.Object) CraftItemGUI.current_overed != (UnityEngine.Object) null)
    {
      this.button_tips.Activate<ButtonTipsStr>();
      CraftItemGUI.current_overed.OnAboveWindowClosed();
    }
    else
      this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAllAndFocus);
  }

  public void ExpandItem(CraftItemGUI craft_item_gui)
  {
    Debug.Log((object) nameof (ExpandItem), (UnityEngine.Object) craft_item_gui);
    craft_item_gui.full_detailed_view = true;
    craft_item_gui.Redraw();
    foreach (GamepadNavigationItem componentsInChild in craft_item_gui.GetComponentsInChildren<GamepadNavigationItem>())
      componentsInChild.active = true;
    this._exanded_item = craft_item_gui;
    if (BaseGUI.for_gamepad)
    {
      foreach (CraftItemGUI listItem in this.list_items)
      {
        listItem.gamepad_navigation_item.active = false;
        listItem.GetComponent<UIWidget>().alpha = (UnityEngine.Object) listItem == (UnityEngine.Object) craft_item_gui ? 1f : 0.5f;
      }
      this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
      this.gamepad_controller.SetFocusedItem(this._exanded_item.ingredients_table.GetComponentsInChildren<BaseItemCellGUI>()[0].GetComponent<GamepadNavigationItem>());
      this._exanded_item.selection_frame.gameObject.SetActive(true);
    }
    else
      this._exanded_item.selection_frame.gameObject.SetActive(false);
    this.UpdateAllAnchors();
    this.craft_list_grid.Reposition();
  }

  public override bool OnPressedBack()
  {
    if (this._gamepad_in_queue_area)
    {
      this._gamepad_in_queue_area = false;
      this.RedrawQueueAreaFocus();
      return true;
    }
    if ((UnityEngine.Object) this._exanded_item != (UnityEngine.Object) null)
    {
      this.CollapseItem();
      return true;
    }
    this.OnClosePressed();
    return true;
  }

  public void ExitFromQueueArea()
  {
    this._gamepad_in_queue_area = false;
    this.RedrawQueueAreaFocus();
  }

  public override bool OnPressedOption1()
  {
    if ((UnityEngine.Object) this.queue == (UnityEngine.Object) null || this.queue.IsEmpty())
      return false;
    this._gamepad_in_queue_area = !this._gamepad_in_queue_area;
    this.RedrawQueueAreaFocus();
    return true;
  }

  public void RedrawQueueAreaFocus()
  {
    if (this._gamepad_in_queue_area == this._gamepad_was_in_queue_area)
      return;
    CraftGUI.in_redraw_mode = true;
    if (this._gamepad_was_in_queue_area)
      CraftQueueItemGUI.current_over?.ForceRemoveSelectionFrame();
    this._gamepad_was_in_queue_area = this._gamepad_in_queue_area;
    this.gamepad_controller.RestoreSelection(this._gamepad_in_queue_area ? 5 : 0);
    CraftGUI.in_redraw_mode = false;
    this.queue.gamepad_hint.gameObject.SetActive(BaseGUI.for_gamepad && !this._gamepad_in_queue_area);
    this.gamepad_hint_queue_big.text = this._gamepad_was_in_queue_area ? GameKeyTip.Get(GameKey.Back, "Back") : "";
  }

  public void CollapseItem(CraftItemGUI specific_item = null)
  {
    CraftItemGUI context = (UnityEngine.Object) specific_item == (UnityEngine.Object) null ? this._exanded_item : specific_item;
    Debug.Log((object) nameof (CollapseItem), (UnityEngine.Object) context);
    if ((UnityEngine.Object) context == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Can't collapse item, _exanded_item is null");
    }
    else
    {
      context.full_detailed_view = false;
      context.Redraw();
      if (BaseGUI.for_gamepad)
      {
        foreach (GamepadNavigationItem componentsInChild in context.GetComponentsInChildren<GamepadNavigationItem>())
          componentsInChild.active = false;
        foreach (CraftItemGUI listItem in this.list_items)
        {
          listItem.gamepad_navigation_item.active = true;
          listItem.GetComponent<UIWidget>().alpha = 1f;
        }
        this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
        this.gamepad_controller.SetFocusedItem(context.gamepad_navigation_item);
      }
      this._exanded_item = (CraftItemGUI) null;
      this.UpdateAllAnchors();
      this.craft_list_grid.Reposition();
    }
  }

  public override void OnClosePressed()
  {
    if (GlobalCraftControlGUI.is_global_control_active)
      GUIElements.me.global_craft_control_gui.Open();
    base.OnClosePressed();
  }

  public void ClearList()
  {
    foreach (CraftItemGUI listItem in this.list_items)
    {
      listItem.gameObject.SetActive(false);
      UnityEngine.Object.Destroy((UnityEngine.Object) listItem.gameObject);
    }
    this.list_items.Clear();
    this.craft_list_grid.Reposition();
  }

  public new void LateUpdate()
  {
    if ((UnityEngine.Object) this.scroll_view == (UnityEngine.Object) null || !this.scroll_view.RestrictWithinBounds(false) || !DOTween.IsTweening((object) this.scroll_view.transform))
      return;
    this.scroll_view.transform.DOKill();
  }

  public List<CraftItemGUI> GetItemsList() => this.list_items;

  public new void OnRightClick() => base.OnRightClick();

  public override bool CanCloseWithRightClick() => true;

  public override bool OnPressedNextTab()
  {
    if (this._gamepad_in_queue_area || this._tabs_ids.Count < 2)
      return false;
    if ((UnityEngine.Object) this.craftery_wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Craftery wgo is null");
      return false;
    }
    int num = this._tabs_ids.IndexOf(this.craftery_wgo.last_opened_tab);
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
    if (this._gamepad_in_queue_area || this._tabs_ids.Count < 2)
      return false;
    if ((UnityEngine.Object) this.craftery_wgo == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Craftery wgo is null");
      return false;
    }
    int num = this._tabs_ids.IndexOf(this.craftery_wgo.last_opened_tab);
    if (num == -1)
      num = 0;
    int index;
    if ((index = num - 1) < 0)
      index = this._tabs_ids.Count - 1;
    this.SwitchTab(this._tabs_ids[index]);
    return true;
  }

  public bool PressPrevTab() => this.OnPressedPrevTab();

  public bool PressNextTab() => this.OnPressedNextTab();

  public override bool OnPressedLeft()
  {
    if (this._gamepad_in_queue_area)
    {
      CraftQueueItemGUI.current_over?.OnDecreasePressed();
      return true;
    }
    GamepadNavigationItem focusedItem = this.gamepad_controller.focused_item;
    CraftItemGUI component = (UnityEngine.Object) focusedItem == (UnityEngine.Object) null ? (CraftItemGUI) null : focusedItem.GetComponent<CraftItemGUI>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.amount_buttons != (UnityEngine.Object) null) || !component.amount_buttons.activeSelf)
      return base.OnPressedLeft();
    component.OnAmountMinus();
    return true;
  }

  public override bool OnPressedRight()
  {
    if (this._gamepad_in_queue_area)
    {
      CraftQueueItemGUI.current_over?.OnIncreasePressed();
      return true;
    }
    GamepadNavigationItem focusedItem = this.gamepad_controller.focused_item;
    CraftItemGUI component = (UnityEngine.Object) focusedItem == (UnityEngine.Object) null ? (CraftItemGUI) null : focusedItem.GetComponent<CraftItemGUI>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.amount_buttons != (UnityEngine.Object) null) || !component.amount_buttons.activeSelf)
      return base.OnPressedRight();
    component.OnAmountPlus();
    return true;
  }

  public override bool OnPressedDown()
  {
    GamepadNavigationItem focusedItem = this.gamepad_controller.focused_item;
    CraftItemGUI componentInParent = (UnityEngine.Object) focusedItem == (UnityEngine.Object) null ? (CraftItemGUI) null : focusedItem.GetComponentInParent<CraftItemGUI>();
    return (UnityEngine.Object) componentInParent != (UnityEngine.Object) null && componentInParent.ProcessIngredientStep(-1) || base.OnPressedDown();
  }

  public override bool OnPressedUp()
  {
    GamepadNavigationItem focusedItem = this.gamepad_controller.focused_item;
    CraftItemGUI componentInParent = (UnityEngine.Object) focusedItem == (UnityEngine.Object) null ? (CraftItemGUI) null : focusedItem.GetComponentInParent<CraftItemGUI>();
    return (UnityEngine.Object) componentInParent != (UnityEngine.Object) null && componentInParent.ProcessIngredientStep(1) || base.OnPressedUp();
  }

  public override bool OnPressedOption2()
  {
    if (this._gamepad_in_queue_area)
    {
      CraftQueueItemGUI.current_over?.OnInfinityButtonPressed();
      return true;
    }
    GamepadNavigationItem focusedItem = this.gamepad_controller.focused_item;
    CraftItemGUI component = (UnityEngine.Object) focusedItem == (UnityEngine.Object) null ? (CraftItemGUI) null : focusedItem.GetComponent<CraftItemGUI>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.current_craft.IsMultiqualityOutput())
    {
      GamepadNavigationItem focused = this.gamepad_controller.focused_item;
      GUIElements.me.tech_dialog.OpenAsItemsList(GJL.L("craft_recipe_hint", GJL.L(component.current_craft.GetNameNonLocalized())), component.current_craft.needs, (GJCommons.VoidDelegate) (() =>
      {
        Debug.Log((object) "Back to craft gui");
        if (!BaseGUI.for_gamepad)
          return;
        this.gamepad_controller.Enable();
        this.gamepad_controller.ReinitItems(false);
        this.gamepad_controller.SetFocusedItem(focused);
        this.gamepad_controller.RememberFocused(focused);
      }));
    }
    return base.OnPressedOption2();
  }

  public static void NeedToCancelCraftsDialog(CraftComponent craft, System.Action on_confirmed)
  {
    GUIElements.me.dialog.Open("need_cancel_craft", "OK", (GJCommons.VoidDelegate) (() =>
    {
      craft.craft_queue.Clear();
      craft.Cancel();
      on_confirmed();
      CraftGUI.RestoreFocusAfterDialogWindow();
    }), "Cancel");
  }

  public static void RestoreFocusAfterDialogWindow()
  {
    if (!BaseGUI.for_gamepad || (UnityEngine.Object) CraftGUI._current_instance == (UnityEngine.Object) null || !CraftGUI._current_instance.is_shown)
      return;
    CraftGUI._current_instance.gamepad_controller.Enable();
    CraftGUI._current_instance.gamepad_controller.ReinitItems(true);
    if (!((UnityEngine.Object) CraftItemGUI.last_item_pressed != (UnityEngine.Object) null))
      return;
    CraftGUI._current_instance.gamepad_controller.SetFocusedItem(CraftItemGUI.last_item_pressed.GetComponent<GamepadNavigationItem>());
  }

  public void SetCustomDirectionForFirstAndLast(string only_tab = null)
  {
    if (this._tabs_ids.Count == 0)
    {
      CraftItemGUI craftItemGui1 = (CraftItemGUI) null;
      CraftItemGUI craftItemGui2 = (CraftItemGUI) null;
      foreach (CraftItemGUI listItem in this.list_items)
      {
        if ((UnityEngine.Object) craftItemGui1 == (UnityEngine.Object) null)
          craftItemGui1 = listItem;
        craftItemGui2 = listItem;
      }
      if (!((UnityEngine.Object) craftItemGui1 != (UnityEngine.Object) null) || !((UnityEngine.Object) craftItemGui2 != (UnityEngine.Object) null))
        return;
      craftItemGui1.gamepad_navigation_item.SetCustomDirectionItem(craftItemGui2.gamepad_navigation_item, Direction.Up);
      craftItemGui2.gamepad_navigation_item.SetCustomDirectionItem(craftItemGui1.gamepad_navigation_item, Direction.Down);
    }
    else if (!string.IsNullOrEmpty(only_tab) && this._tabs_ids.Contains(only_tab))
    {
      CraftItemGUI craftItemGui3 = (CraftItemGUI) null;
      CraftItemGUI craftItemGui4 = (CraftItemGUI) null;
      CraftItemGUI craftItemGui5 = (CraftItemGUI) null;
      foreach (CraftItemGUI listItem in this.list_items)
      {
        if (listItem.craft_definition.tab_id == only_tab || CraftGUI._current_instance.craftery_wgo.obj_def.interaction_type == ObjectDefinition.InteractionType.Builder && listItem.craft_definition.id == "_remove_")
        {
          if ((UnityEngine.Object) craftItemGui3 == (UnityEngine.Object) null)
            craftItemGui3 = listItem;
          if ((UnityEngine.Object) craftItemGui4 != (UnityEngine.Object) null)
            craftItemGui5 = craftItemGui4;
          craftItemGui4 = listItem;
        }
      }
      if (!((UnityEngine.Object) craftItemGui3 != (UnityEngine.Object) null) || !((UnityEngine.Object) craftItemGui4 != (UnityEngine.Object) null))
        return;
      craftItemGui3.gamepad_navigation_item.SetCustomDirectionItem(craftItemGui4.gamepad_navigation_item, Direction.Up);
      craftItemGui4.gamepad_navigation_item.SetCustomDirectionItem(craftItemGui3.gamepad_navigation_item, Direction.Down);
      if (!((UnityEngine.Object) craftItemGui5 != (UnityEngine.Object) null))
        return;
      craftItemGui5.gamepad_navigation_item.SetCustomDirectionItem(craftItemGui4.gamepad_navigation_item, Direction.Down);
    }
    else
    {
      foreach (string tabsId in this._tabs_ids)
      {
        CraftItemGUI craftItemGui6 = (CraftItemGUI) null;
        CraftItemGUI craftItemGui7 = (CraftItemGUI) null;
        foreach (CraftItemGUI listItem in this.list_items)
        {
          if (listItem.craft_definition.tab_id == tabsId)
          {
            if ((UnityEngine.Object) craftItemGui6 == (UnityEngine.Object) null)
              craftItemGui6 = listItem;
            craftItemGui7 = listItem;
          }
        }
        if ((UnityEngine.Object) craftItemGui6 != (UnityEngine.Object) null && (UnityEngine.Object) craftItemGui7 != (UnityEngine.Object) null)
        {
          craftItemGui6.gamepad_navigation_item.SetCustomDirectionItem(craftItemGui7.gamepad_navigation_item, Direction.Up);
          craftItemGui7.gamepad_navigation_item.SetCustomDirectionItem(craftItemGui6.gamepad_navigation_item, Direction.Down);
        }
      }
    }
  }
}
