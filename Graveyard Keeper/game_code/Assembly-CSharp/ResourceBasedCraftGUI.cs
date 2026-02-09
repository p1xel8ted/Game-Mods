// Decompiled with JetBrains decompiler
// Type: ResourceBasedCraftGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ResourceBasedCraftGUI : BaseCraftGUI
{
  public static CraftDefinition last_previewed_craft;
  [HideInInspector]
  public UITable craft_list_table;
  public GameObject close_btn;
  public GameObject back_for_inactive_stuff;
  public GameObject plus;
  public BaseItemCellGUI main_ingredient;
  public List<BaseItemCellGUI> ingredients;
  public UI2DSprite decoration;
  [NonSerialized]
  public GamepadSelectableButton craft_button;
  public CraftDefinition _current_craft_definition;
  public ResourceBasedCraftGUI.State _state;
  public Transform _main_ingredient_container;
  public UILabel header;
  public UILabel label_craft_btn;
  public UILabel label_resourse_hint;
  public Item _selected_item;
  public ItemDefinition _specific_item;
  public List<string> _allowed_ids = new List<string>();
  public List<string> _multiquality_ids;
  public const string SIENCE_FONT_TOKEN = "(sci)";

  public override void Init()
  {
    this.craft_button = this.GetComponentInChildren<GamepadSelectableButton>(true);
    this.craft_button.Init();
    this.craft_list_table = this.GetComponentInChildren<UITable>(true);
    this.main_ingredient.SetCallbacks(new GJCommons.VoidDelegate(this.OnItemOver), (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.OnChooseItem));
    this._main_ingredient_container = this.main_ingredient.transform.parent;
    base.Init();
  }

  public void Open(WorldGameObject craftery_wgo, CraftDefinition.CraftType allowed_type = CraftDefinition.CraftType.ResourcesBasedCraft)
  {
    if (this.is_shown || this.isActiveAndEnabled)
      return;
    this.CommonOpen(craftery_wgo, allowed_type);
    this.header.text = GJL.L(craftery_wgo.obj_id);
    this._current_craft_definition = (CraftDefinition) null;
    this.DrawText();
    this._allowed_ids.Clear();
    foreach (CraftDefinition craft in this.crafts)
    {
      if (craft.needs == null || craft.needs.Count == 0)
        Debug.LogError((object) $"Craft {craft.id} has no needs!");
      else
        this._allowed_ids.Add(craft.needs[0].id);
    }
    string craftPreset = craftery_wgo.obj_def.craft_preset;
    if (this.UseCustomDecorations())
      this.decoration.sprite2D = !string.IsNullOrEmpty(craftPreset) ? EasySpritesCollection.GetSprite(craftPreset) : (UnityEngine.Sprite) null;
    if (this.ingredients.Count > 0)
    {
      this.ingredients.Add(this.ingredients[0].Copy<BaseItemCellGUI>());
      this.ingredients.Add(this.ingredients[0].Copy<BaseItemCellGUI>());
    }
    this.main_ingredient.InitInputBehaviour();
    foreach (BaseItemCellGUI ingredient in this.ingredients)
    {
      ingredient.InitInputBehaviour();
      ingredient.SetCallbacks(new BaseItemCellGUI.OnItemAction(this.OnIngredientOver), (BaseItemCellGUI.OnItemAction) null, (BaseItemCellGUI.OnItemAction) null);
    }
    GUIElements.me.resource_picker.ClearResultDelegate();
    GUIElements.me.resource_picker.Hide(true);
    this._current_craft_definition = (CraftDefinition) null;
    this._state = ResourceBasedCraftGUI.State.Start;
    this._current_craft_definition = (CraftDefinition) null;
    this.Redraw();
    if (craftery_wgo != null && craftery_wgo.components?.craft?.last_craft_id != null && craftery_wgo.obj_def != null && !craftery_wgo.obj_def.dont_restore_last_craft)
    {
      CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>(craftery_wgo.components.craft.last_craft_id);
      if (dataOrNull != null && dataOrNull.needs.Count > 0)
        this.OnResourcePickerClosed(dataOrNull.needs[0]);
    }
    if (BaseGUI.for_gamepad)
      this.button_tips.Activate<ButtonTipsStr>();
    this.craft_list_table.Reposition();
  }

  public void DrawText()
  {
    string str = this.craftery_wgo.obj_def.inventory_preset;
    if (!string.IsNullOrEmpty(str))
      str = "_" + str;
    this.label_craft_btn.text = GJL.L("btn_craft" + str);
    this.label_resourse_hint.text = GJL.L("craft_pick_res_hint" + str);
    if (this._selected_item != null && this._current_craft_definition != null && this._current_craft_definition.sub_type == CraftDefinition.CraftSubType.SurveySciencePoints)
    {
      this.label_craft_btn.text = GJL.L("btn_science_decompose");
      this.label_resourse_hint.text = GJL.L("science_decompose", "+(sci)" + this._current_craft_definition.output_to_wgo[0].value.ToString());
    }
    else
    {
      if (this._current_craft_definition == null)
        return;
      this.label_resourse_hint.text = "";
    }
  }

  public override void Redraw()
  {
    base.Redraw();
    this.Redraw((ItemDefinition) null);
    this.DrawText();
    if (!((UnityEngine.Object) this.object_info != (UnityEngine.Object) null))
      return;
    this.object_info.Draw(this.craftery_wgo.GetUniversalObjectInfo());
  }

  public void Redraw(ItemDefinition specific_item)
  {
    this._specific_item = specific_item;
    switch (this._state)
    {
      case ResourceBasedCraftGUI.State.Start:
        this.main_ingredient.DrawEmpty();
        using (List<BaseItemCellGUI>.Enumerator enumerator = this.ingredients.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.Deactivate<BaseItemCellGUI>();
          break;
        }
    }
    this.back_for_inactive_stuff.SetActive(false);
    this.craft_list_table.Reposition();
    this.craft_list_table.repositionNow = true;
    this.craft_button.SetCallbacks(new GJCommons.VoidDelegate(this.OnCraftButtonPressed), new GJCommons.VoidDelegate(this.OnCraftButtonOver));
    List<Item> objList = new List<Item>();
    if (this._current_craft_definition != null)
    {
      foreach (Item need in this._current_craft_definition.needs)
        objList.Add(new Item(need));
      foreach (Item obj in this._current_craft_definition.needs_from_wgo)
      {
        if (this.craftery_wgo.obj_def.additional_header_items.Contains(obj.id))
          objList.Add(new Item(obj));
      }
    }
    if (GlobalCraftControlGUI.is_global_control_active && this._current_craft_definition != null)
    {
      SmartExpression gratitudePointsCraftCost = this._current_craft_definition.gratitude_points_craft_cost;
      float item_value = gratitudePointsCraftCost != null ? gratitudePointsCraftCost.EvaluateFloat() : 0.0f;
      if ((double) item_value > 0.0)
        objList.Add(new Item("gratitude_as_item", (int) item_value));
    }
    if (specific_item != null)
      objList[0].id = specific_item.id;
    if (objList.Count > 0 && objList[0].definition == null)
    {
      this._current_craft_definition = (CraftDefinition) null;
      objList.Clear();
    }
    this._main_ingredient_container.localPosition = this._main_ingredient_container.localPosition with
    {
      x = this._state == ResourceBasedCraftGUI.State.Start || objList.Count <= 1 ? 0.0f : -80f
    };
    if (this._current_craft_definition == null)
    {
      this.plus.Deactivate();
      foreach (BaseItemCellGUI ingredient in this.ingredients)
        ingredient.Deactivate<BaseItemCellGUI>();
      this.craft_list_table.Reposition();
      this.craft_button.SetEnabled(false);
      if (BaseGUI.for_gamepad && this._state != ResourceBasedCraftGUI.State.ChoosingItem)
      {
        this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
        this.gamepad_controller.SetFocusedItem(this.main_ingredient.gamepad_item);
      }
    }
    else
    {
      this.main_ingredient.DrawIngredient(objList[0], this.multi_inventory, init_tooltip: true);
      objList.RemoveAt(0);
      this.plus.SetActive(objList.Count > 0);
      foreach (BaseItemCellGUI ingredient in this.ingredients)
      {
        if (objList.Count == 0)
        {
          ingredient.Deactivate<BaseItemCellGUI>();
        }
        else
        {
          ingredient.DrawIngredient(objList[0], this.multi_inventory, init_tooltip: true, additional_inventory: this.craftery_wgo.data);
          objList.RemoveAt(0);
        }
      }
      this.craft_list_table.Reposition();
      this.craft_button.SetEnabled(this.CanCraft());
      if (BaseGUI.for_gamepad && this._state != ResourceBasedCraftGUI.State.ChoosingItem)
      {
        this.gamepad_controller.Enable(GamepadNavigationController.OpenMethod.GetAll);
        this.gamepad_controller.SetFocusedItem(this.CanCraft() ? this.craft_button.navigation_item : this.main_ingredient.gamepad_item);
      }
    }
    this.DrawText();
  }

  public void OnIngredientOver(BaseItemCellGUI item_gui)
  {
    if (BaseGUI.for_gamepad)
      this.button_tips.Print(GameKeyTip.Select(false), GameKeyTip.Close());
    else
      item_gui.SetVisualyOveredState(false, BaseGUI.for_gamepad);
  }

  public void OnItemOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(), GameKeyTip.Close());
  }

  public void OnCraftButtonPressed()
  {
    PrayCraftGUI component = this.GetComponent<PrayCraftGUI>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (GUIElements.me.pray_craft?.pray_craft == null)
        return;
      component.OnPrayButtonPressed();
    }
    else
      this.OnCraft();
  }

  public void OnCraftButtonOver()
  {
    if (!BaseGUI.for_gamepad)
      return;
    this.button_tips.Print(GameKeyTip.Select(this.CanCraft()), GameKeyTip.Close());
  }

  public void OnChooseItem()
  {
    this.ChooseItem();
    this.Redraw(this._specific_item);
    this.button_tips.Deactivate<ButtonTipsStr>();
    this.close_btn.Deactivate();
  }

  public void OnCraft()
  {
    if (!this.CanCraft())
      return;
    WorldGameObject other_obj_override = (WorldGameObject) null;
    if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this.craftery_wgo != (UnityEngine.Object) null)
    {
      WorldZone myWorldZone = this.craftery_wgo.GetMyWorldZone();
      if ((UnityEngine.Object) myWorldZone != (UnityEngine.Object) null && !myWorldZone.IsPlayerInZone())
        other_obj_override = this.craftery_wgo;
    }
    this.OnCraft(this._current_craft_definition, this._selected_item, this._multiquality_ids, other_obj_override: other_obj_override);
  }

  public virtual bool CanCraft()
  {
    this._multiquality_ids = (List<string>) null;
    if (this._specific_item != null)
    {
      this._multiquality_ids = new List<string>();
      foreach (Item need in this._current_craft_definition.needs)
        this._multiquality_ids.Add(need.id);
      this._multiquality_ids[0] = this._specific_item.id;
    }
    return this.CanCraft(this._current_craft_definition, this._multiquality_ids);
  }

  public override bool OnPressedBack()
  {
    this.OnClosePressed();
    return true;
  }

  public virtual void ChooseItem()
  {
    WorldGameObject worldGameObject = MainGame.me.player;
    if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this.craftery_wgo != (UnityEngine.Object) null && !WorldZone.GetZoneOfObject(this.craftery_wgo).IsPlayerInZone())
      worldGameObject = this.craftery_wgo;
    GUIElements.me.resource_picker.Open(worldGameObject, (InventoryWidget.ItemFilterDelegate) ((item, widget) => !this.IsItemAllowed(item) ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active), new CraftResourcesSelectGUI.ResourceSelectResultDelegate(this.OnResourcePickerClosed));
    this._state = ResourceBasedCraftGUI.State.ChoosingItem;
  }

  public bool IsItemAllowed(Item item)
  {
    if (item == null)
      return false;
    return item.id.Contains(":") && this._allowed_ids.Contains(item.definition.GetNameWithoutQualitySuffix()) || this._allowed_ids.Contains(item.id);
  }

  public virtual void OnResourcePickerClosed(Item item)
  {
    string str = item != null ? item.id : "";
    if (item != null)
    {
      this._selected_item = item;
      bool flag = false;
      if (str.Contains(":"))
      {
        string withoutQualitySuffix = item.definition.GetNameWithoutQualitySuffix();
        if (this._allowed_ids.Contains(withoutQualitySuffix))
        {
          flag = true;
          str = withoutQualitySuffix;
        }
      }
      if (this._allowed_ids.Contains(str))
      {
        foreach (CraftDefinition craft in this.crafts)
        {
          if (craft.needs[0].id == str)
          {
            this._current_craft_definition = craft;
            break;
          }
        }
      }
      this._state = this._current_craft_definition == null ? ResourceBasedCraftGUI.State.Start : ResourceBasedCraftGUI.State.CraftOrChoose;
      this.Redraw(flag ? item.definition : (ItemDefinition) null);
    }
    if (BaseGUI.for_gamepad)
    {
      this.button_tips.Activate<ButtonTipsStr>();
      this.gamepad_controller.Enable();
      this.gamepad_controller.ReinitItems(true);
    }
    else
      this.close_btn.Activate();
    if ((UnityEngine.Object) this.GetComponent<PrayCraftGUI>() != (UnityEngine.Object) null)
      this.GetComponent<PrayCraftGUI>().OnResourcePickerClosed(item);
    this.DrawText();
  }

  public override void Update()
  {
    if ((UnityEngine.Object) this.craft_list_table == (UnityEngine.Object) null)
      return;
    this.craft_list_table.Reposition();
    if (!BaseGUI.for_gamepad && LazyInput.GetKeyDown(GameKey.Interaction))
      this.OnCraftButtonPressed();
    base.Update();
  }

  public override void Hide(bool play_hide_sound = true)
  {
    if ((UnityEngine.Object) this.GetComponent<PrayCraftGUI>() != (UnityEngine.Object) null && !this.GetComponent<PrayCraftGUI>().CanClose())
      return;
    while (this.ingredients.Count > 1)
    {
      this.ingredients[1].DestroyGO<BaseItemCellGUI>();
      this.ingredients.RemoveAt(1);
    }
    base.Hide(play_hide_sound);
  }

  public bool UseCustomDecorations() => (UnityEngine.Object) this.GetComponent<PrayCraftGUI>() == (UnityEngine.Object) null;

  [CompilerGenerated]
  public InventoryWidget.ItemFilterResult \u003CChooseItem\u003Eb__34_0(
    Item item,
    InventoryWidget widget)
  {
    return !this.IsItemAllowed(item) ? InventoryWidget.ItemFilterResult.Inactive : InventoryWidget.ItemFilterResult.Active;
  }

  public enum State
  {
    Start,
    CraftOrChoose,
    ChoosingItem,
  }
}
