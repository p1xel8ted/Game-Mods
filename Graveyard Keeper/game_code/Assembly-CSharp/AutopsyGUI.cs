// Decompiled with JetBrains decompiler
// Type: AutopsyGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class AutopsyGUI : BaseGUI
{
  public const int BODY_ITEMS_GROUP = 1;
  public const int REMOVE_BODY_GROUP = 2;
  public GameObject no_body_label;
  public BodyPartGUI[] _body_parts;
  public InventoryWidget _inventory_widget;
  public CraftItemGUI _body_item_gui;
  public UIButton remove_body_button;
  public WorldGameObject _autopti_obj;
  public BodyPartGUI _current_part;
  public Item _current_part_inventory;
  public Inventory _parts_inventory = new Inventory(Item.empty);
  public BaseItemCellGUI _last_body_item;
  public UniversalObjectInfoGUI _universal_info;
  public Item _body;
  public BodyPanelGUI body_panel;

  public override void Init()
  {
    this._body_parts = this.GetComponentsInChildren<BodyPartGUI>(true);
    foreach (BodyPartGUI bodyPart in this._body_parts)
      bodyPart.Init();
    this._universal_info = this.GetComponentInChildren<UniversalObjectInfoGUI>(true);
    this._inventory_widget = this.GetComponentInChildren<InventoryWidget>(true);
    this._inventory_widget.Init();
    this._inventory_widget.SetCallbacks(new InventoryWidget.ItemDelegate(this.OnBodyItemOver), (InventoryWidget.ItemDelegate) null, new InventoryWidget.ItemDelegate(this.OnBodyItemPress));
    this._body_item_gui = this.GetComponentInChildren<CraftItemGUI>(true);
    this._body_item_gui.gamepad_navigation_item.SetCallbacks((GJCommons.VoidDelegate) (() =>
    {
      this._body_item_gui.selection_frame.Activate<UIWidget>();
      this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
      Sounds.OnGUIHover();
    }), (GJCommons.VoidDelegate) (() => this._body_item_gui.selection_frame.Deactivate<UIWidget>()), new GJCommons.VoidDelegate(this.DropBody));
    this.body_panel.button_item.SetCallbacks((GJCommons.VoidDelegate) (() =>
    {
      this._body_item_gui.selection_frame.Activate<UIWidget>();
      this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
      Sounds.OnGUIHover();
    }), (GJCommons.VoidDelegate) (() => this._body_item_gui.selection_frame.Deactivate<UIWidget>()), new GJCommons.VoidDelegate(this.DropBody));
    this.body_panel.skull_bar.on_enable_skulls_frame += new System.Action(this.OnSkullsOver);
    this.body_panel.skull_bar.on_disable_skulls_frame += new System.Action(this.OnSkullsOut);
    base.Init();
  }

  public void Open(WorldGameObject craft_obj)
  {
    this.Open();
    this._body_item_gui.gamepad_navigation_item.UnFocus();
    this._autopti_obj = craft_obj;
    this._body = craft_obj.GetBodyFromInventory();
    this._parts_inventory = new Inventory(Item.empty);
    this._universal_info.Draw(craft_obj.GetUniversalObjectInfo());
    this.body_panel.Draw(this._body);
    if (this._body == null)
    {
      Debug.Log((object) "AutopsyGUI: no body in inventory, drawing empty");
      this.DrawEmpty();
    }
    else
    {
      if (this.CanInsertInsideBody())
        this.AddInsertionButtonToPartsInventory();
      foreach (Item obj1 in this._body.inventory)
      {
        if (obj1.definition.type == ItemDefinition.ItemType.BodyUniversalPart)
          this.TryAddBodyPartToPartsInventory(obj1);
        foreach (Item obj2 in obj1.inventory)
        {
          if (obj2.id.Contains<char>('\n'))
          {
            Debug.LogError((object) $"Item {obj2.id} contains \\n in id. This should never happen.");
            obj2.SetItemID(obj2.id.Trim('\n'));
          }
          if (obj2.definition.type == ItemDefinition.ItemType.BodyUniversalPart)
            this.TryAddBodyPartToPartsInventory(obj2);
        }
      }
      this.DrawBody();
    }
  }

  public void AddInsertionButtonToPartsInventory()
  {
    this._parts_inventory.data.inventory.Add(new Item("insertion_button_pseudoitem"));
  }

  public bool CanInsertInsideBody() => true;

  public void TryAddBodyPartToPartsInventory(Item item)
  {
    if (this.GetExtractCraftDefinition(item) == null && item.id != "surgeon_mistake")
      item = new Item("unknown_body_part");
    item.ReplaceItemIfNeeded();
    if (item.id == "sin_shard_body_part" && !DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Souls))
      return;
    this._parts_inventory.data.inventory.Add(item);
  }

  public void DrawEmpty()
  {
    this.ClearItemsGrid();
    foreach (BodyPartGUI bodyPart in this._body_parts)
      bodyPart.Deactivate<BodyPartGUI>();
    this.no_body_label.SetActive(true);
    this.button_tips.PrintClose();
    this.remove_body_button.isEnabled = false;
    this._inventory_widget.interaction_enabled = false;
  }

  public void DrawBody()
  {
    this._last_body_item = (BaseItemCellGUI) null;
    this._inventory_widget.Open(this._parts_inventory, BaseGUI.for_gamepad, 1);
    this._inventory_widget.interaction_enabled = true;
    this.no_body_label.SetActive(false);
    if (BaseGUI.for_gamepad)
    {
      this.gamepad_controller.ReinitItems(false);
      this.gamepad_controller.FocusOnFirstActive(1);
    }
    this.remove_body_button.isEnabled = !GlobalCraftControlGUI.is_global_control_active;
  }

  public void OnBodyItemOver(BaseItemCellGUI item_gui)
  {
    if (item_gui.item.id == "insertion_button_pseudoitem")
    {
      this._last_body_item = item_gui;
      if (!BaseGUI.for_gamepad)
        return;
      this.button_tips.Print(GameKeyTip.Select("insertion_button_pseudoitem"), GameKeyTip.Close());
    }
    else
    {
      CraftDefinition extractCraftDefinition = this.GetExtractCraftDefinition(item_gui.item);
      this._last_body_item = item_gui;
      if (!BaseGUI.for_gamepad)
        return;
      this.button_tips.Print(GameKeyTip.Select("extract", extractCraftDefinition != null), GameKeyTip.Close());
    }
  }

  public void OnBodyItemPress(BaseItemCellGUI item_gui)
  {
    if (item_gui.item.id == "insertion_button_pseudoitem")
    {
      Debug.Log((object) "Not done yet.");
      WorldGameObject worldGameObject = MainGame.me.player;
      if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this._autopti_obj != (UnityEngine.Object) null)
        worldGameObject = this._autopti_obj;
      GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((item, widget) =>
      {
        if (item == null || item.IsEmpty())
          return InventoryWidget.ItemFilterResult.Hide;
        if (item.definition.type != ItemDefinition.ItemType.BodyUniversalPart)
          return InventoryWidget.ItemFilterResult.Inactive;
        string id = item.id;
        if (id.Contains(":"))
          id = id.Split(':')[0];
        string str = id.Replace("_dark", "");
        foreach (Item obj in this._parts_inventory.data.inventory)
        {
          if (obj != null && !obj.IsEmpty() && obj.id.StartsWith(str))
            return InventoryWidget.ItemFilterResult.Inactive;
        }
        return this.GetInsertCraftDefinition(item) == null ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
      }), new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnItemForInsertionPicked));
    }
    else
    {
      CraftDefinition craft_definition = this.GetExtractCraftDefinition(item_gui.item);
      if (craft_definition == null)
        return;
      GUIElements.me.dialog.OpenYesNo(GJL.L("extract_question", item_gui.item.definition.GetItemName()), (GJCommons.VoidDelegate) (() =>
      {
        AutopsyGUI.RemoveBodyPartFromBody(this._body, item_gui.item);
        this._autopti_obj.components.craft.CraftAsPlayer(craft_definition, item_gui.item);
        this.Hide(true);
      }), (GJCommons.VoidDelegate) (() => { }));
    }
  }

  public void OnItemForInsertionPicked(Item item)
  {
    if (item == null || item.IsEmpty())
      return;
    CraftDefinition insertCraftDefinition = this.GetInsertCraftDefinition(item);
    if (insertCraftDefinition == null)
    {
      Debug.LogError((object) $"Not found insertion CraftDefinition for item \"{item.id}\"");
    }
    else
    {
      this._autopti_obj.components.craft.CraftAsPlayer(insertCraftDefinition, new Item(item.id, 1));
      this.Hide(true);
    }
  }

  public static void RemoveBodyPartFromBody(Item body, Item item)
  {
    foreach (Item obj1 in body.inventory)
    {
      if (obj1.id == item.id)
      {
        body.RemoveItem(item, 1);
        break;
      }
      foreach (Item obj2 in obj1.inventory)
      {
        if (obj2.id == item.id)
        {
          obj1.RemoveItem(item, 1);
          return;
        }
      }
    }
  }

  public override void OnAboveWindowClosed()
  {
    if (!BaseGUI.for_gamepad || (UnityEngine.Object) this._last_body_item == (UnityEngine.Object) null)
      return;
    this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
    this.gamepad_controller.SetFocusedItem(this._last_body_item.gamepad_item);
  }

  public CraftDefinition GetExtractCraftDefinition(Item item)
  {
    if (item.IsEmpty())
      return (CraftDefinition) null;
    string id = item.id;
    if (id.Contains(":"))
      id = id.Split(':')[0];
    CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>($"ex:{this._autopti_obj.obj_id}:{id}");
    return dataOrNull != null && !MainGame.me.save.IsCraftVisible(dataOrNull) ? (CraftDefinition) null : dataOrNull;
  }

  public CraftDefinition GetInsertCraftDefinition(Item item)
  {
    if (item == null || item.IsEmpty())
      return (CraftDefinition) null;
    string id = item.id;
    if (id.Contains(":"))
      id = id.Split(':')[0];
    CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>($"insert:{this._autopti_obj.obj_id}:{id}");
    return dataOrNull != null && !MainGame.me.save.IsCraftVisible(dataOrNull) ? (CraftDefinition) null : dataOrNull;
  }

  public void DropBody()
  {
    for (int index = 0; index < this._autopti_obj.data.inventory.Count; ++index)
    {
      Item obj = this._autopti_obj.data.inventory[index];
      if (obj.definition.type == ItemDefinition.ItemType.Body)
      {
        this._autopti_obj.GiveItemToPlayersHands(obj);
        this.Hide(false);
        break;
      }
    }
  }

  public void ClearItemsGrid()
  {
    this._parts_inventory.data.inventory.Clear();
    this._inventory_widget.Redraw();
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public override void OnClosePressed()
  {
    if (GlobalCraftControlGUI.is_global_control_active)
      GUIElements.me.global_craft_control_gui.Open();
    base.OnClosePressed();
  }

  public override void Hide(bool play_hide_sound = true)
  {
    if (GlobalCraftControlGUI.is_global_control_active)
      GUIElements.me.global_craft_control_gui.Open();
    this.ClearItemsGrid();
    base.Hide(play_hide_sound);
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
  public void \u003CInit\u003Eb__15_0()
  {
    this._body_item_gui.selection_frame.Activate<UIWidget>();
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
    Sounds.OnGUIHover();
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__15_1()
  {
    this._body_item_gui.selection_frame.Deactivate<UIWidget>();
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__15_2()
  {
    this._body_item_gui.selection_frame.Activate<UIWidget>();
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
    Sounds.OnGUIHover();
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__15_3()
  {
    this._body_item_gui.selection_frame.Deactivate<UIWidget>();
  }
}
