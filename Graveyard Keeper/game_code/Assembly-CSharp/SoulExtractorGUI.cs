// Decompiled with JetBrains decompiler
// Type: SoulExtractorGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SoulExtractorGUI : BaseGUI
{
  public const int BODY_ITEMS_GROUP = 1;
  public const string MIN_DAMAGE_ID = "min_damage_chance";
  public const string MAX_DAMAGE_ID = "max_damage_chance";
  [SerializeField]
  public BodyPanelGUI _body_panel_gui;
  [SerializeField]
  public InventoryWidget _inventory_widget;
  [SerializeField]
  public GameObject _no_body_label;
  [SerializeField]
  public UIButton _remove_body_button;
  [SerializeField]
  public UIButton _extract_soul_button;
  [SerializeField]
  public CraftItemGUI _body_item_gui;
  [SerializeField]
  public UniversalObjectInfoGUI _universal_object_info;
  [SerializeField]
  public SoulExtractorInfoWidget soul_info_widget;
  [SerializeField]
  [Space]
  public GamepadNavigationItem _soul_extractor_button_nav_item;
  [SerializeField]
  public GamepadNavigationItem _exhume_body_button_nav_item;
  public WorldGameObject _soul_extractor_obj;
  public BaseItemCellGUI _last_body_item;
  public Inventory _parts_inventory = new Inventory(Item.empty);
  public Item _body;

  public override void Init()
  {
    base.Init();
    this._inventory_widget.Init();
    this._inventory_widget.interaction_enabled = false;
    this._body_panel_gui.skull_bar.on_enable_skulls_frame += new System.Action(this.OnSkullsOver);
    this._body_panel_gui.skull_bar.on_disable_skulls_frame += new System.Action(this.OnSkullsOut);
    this._body_item_gui.gamepad_navigation_item.SetCallbacks((GJCommons.VoidDelegate) (() =>
    {
      this._body_item_gui.selection_frame.Activate<UIWidget>();
      this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
      Sounds.OnGUIHover();
    }), (GJCommons.VoidDelegate) (() => this._body_item_gui.selection_frame.Deactivate<UIWidget>()), new GJCommons.VoidDelegate(this.DropBody));
    this._soul_extractor_button_nav_item.SetCallbacks((GJCommons.VoidDelegate) (() =>
    {
      this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
      Sounds.OnGUIHover();
    }), (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.StartExtractionCraft));
    this._body_panel_gui.skull_bar.InitGamepadItem();
  }

  public void Open(WorldGameObject craft_obj)
  {
    this.Open();
    this._universal_object_info.Draw(craft_obj.GetUniversalObjectInfo());
    this._soul_extractor_obj = craft_obj;
    this._parts_inventory = new Inventory(Item.empty);
    this._parts_inventory.data.inventory_size = 1;
    this._body = craft_obj.GetBodyFromInventory();
    this._body_panel_gui.Draw(this._body);
    this._extract_soul_button.isEnabled = false;
    this.soul_info_widget.SetActive(false);
    this._exhume_body_button_nav_item.SetCallbacks((GJCommons.VoidDelegate) (() =>
    {
      this.button_tips.Print(GameKeyTip.Select(this._body != null), GameKeyTip.Close());
      Sounds.OnGUIHover();
    }), (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.DropBody));
    if (this._body == null)
    {
      this.DrawEmpty();
    }
    else
    {
      if (BaseGUI.for_gamepad)
      {
        this.gamepad_controller.ReinitItems(false);
        this.gamepad_controller.Enable();
        this.gamepad_controller.SetFocusedItem(this._exhume_body_button_nav_item, false);
        this._soul_extractor_button_nav_item.active = false;
      }
      foreach (Item obj in this._body.inventory)
      {
        if (obj.definition.type == ItemDefinition.ItemType.SoulBodyPart)
        {
          if (this.TryAddSoulPartToInventory(obj))
          {
            if (BaseGUI.for_gamepad)
            {
              this._soul_extractor_button_nav_item.active = true;
              this.gamepad_controller.SetFocusedItem(this._soul_extractor_button_nav_item, false);
            }
            this._extract_soul_button.isEnabled = true;
            this.soul_info_widget.SetActive(true);
            this.soul_info_widget.SetData(this.GetDamageChance(craft_obj, true), this.GetDamageChance(craft_obj, false));
            this.soul_info_widget.Redraw();
            break;
          }
          break;
        }
        if (obj.definition.id == "sin_shard_body_part")
          this.TryAddSoulPartToInventory(obj);
      }
      this.DrawBody();
    }
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this.ClearItemsGrid();
    this._inventory_widget.Hide();
    if (GlobalCraftControlGUI.is_global_control_active)
      GUIElements.me.global_craft_control_gui.Open();
    base.Hide(play_hide_sound);
  }

  public void DropBody()
  {
    for (int index = 0; index < this._soul_extractor_obj.data.inventory.Count; ++index)
    {
      Item obj = this._soul_extractor_obj.data.inventory[index];
      if (obj.definition.type == ItemDefinition.ItemType.Body)
      {
        this._soul_extractor_obj.GiveItemToPlayersHands(obj);
        this.Hide(false);
        break;
      }
    }
  }

  public void StartExtractionCraft()
  {
    Item soul_item = this._parts_inventory.data.inventory[0];
    CraftDefinition craftDefinition = this.GetCraftDefinition(soul_item);
    if (craftDefinition != null)
    {
      this._soul_extractor_obj.components.craft.Craft(craftDefinition, new Item(soul_item));
      if (this._soul_extractor_obj.is_current_craft_gratitude)
        this._soul_extractor_obj.SetParam("craft_started_from_gc", 1f);
      this.Hide(true);
    }
    else
      Debug.LogError((object) ("Not found extraction craft for item " + soul_item.id));
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

  public void DrawEmpty()
  {
    this.ClearItemsGrid();
    this._remove_body_button.isEnabled = false;
    this._extract_soul_button.isEnabled = false;
    this.button_tips.Print(GameKeyTip.Close());
    this._no_body_label.SetActive(true);
  }

  public void DrawBody()
  {
    this._inventory_widget.Open(this._parts_inventory, BaseGUI.for_gamepad, 1);
    this._no_body_label.SetActive(false);
    this._remove_body_button.isEnabled = !GlobalCraftControlGUI.is_global_control_active;
  }

  public bool TryAddSoulPartToInventory(Item item)
  {
    item.ReplaceItemIfNeeded();
    this._parts_inventory.data.inventory.Add(item);
    return true;
  }

  public void ClearItemsGrid()
  {
    this._parts_inventory.data.inventory.Clear();
    this._inventory_widget.Redraw();
  }

  public CraftDefinition GetCraftDefinition(Item soul_item)
  {
    return GameBalance.me.GetDataOrNull<CraftDefinition>($"{this._soul_extractor_obj.obj_id}:{soul_item.id}");
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

  public float GetDamageChance(WorldGameObject craftery_wgo, bool is_min_chance)
  {
    string objId = craftery_wgo.obj_id;
    switch (objId)
    {
      case "soul_extractor":
        return !is_min_chance ? 0.3f : 0.1f;
      case "soul_extractor_2":
        return !is_min_chance ? 0.15f : 0.05f;
      case "soul_extractor_3":
        return 0.0f;
      default:
        Debug.LogError((object) $"Not expected value \"{objId}\"");
        return 0.0f;
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
  public void \u003CInit\u003Eb__17_0()
  {
    this._body_item_gui.selection_frame.Activate<UIWidget>();
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
    Sounds.OnGUIHover();
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__17_1()
  {
    this._body_item_gui.selection_frame.Deactivate<UIWidget>();
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__17_2()
  {
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
    Sounds.OnGUIHover();
  }

  [CompilerGenerated]
  public void \u003COpen\u003Eb__18_0()
  {
    this.button_tips.Print(GameKeyTip.Select(this._body != null), GameKeyTip.Close());
    Sounds.OnGUIHover();
  }
}
