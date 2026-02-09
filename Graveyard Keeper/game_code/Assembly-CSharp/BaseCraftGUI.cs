// Decompiled with JetBrains decompiler
// Type: BaseCraftGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaseCraftGUI : BaseGUI
{
  public static Dictionary<long, string> last_crafts = new Dictionary<long, string>();
  [NonSerialized]
  public List<CraftDefinition> crafts = new List<CraftDefinition>();
  [NonSerialized]
  public WorldGameObject craftery_wgo;
  [NonSerialized]
  public CraftComponent craft_component;
  public CraftsInventory crafts_inventory;
  public UniversalObjectInfoGUI object_info;

  public WorldGameObject additional_inventory
  {
    get => !this.is_shown ? (WorldGameObject) null : this.craftery_wgo;
  }

  public MultiInventory multi_inventory
  {
    get
    {
      if (GlobalCraftControlGUI.is_global_control_active && (UnityEngine.Object) this.craftery_wgo != (UnityEngine.Object) null)
      {
        WorldZone myWorldZone = this.craftery_wgo.GetMyWorldZone();
        if ((UnityEngine.Object) myWorldZone != (UnityEngine.Object) null && !myWorldZone.IsPlayerInZone())
          return this.craftery_wgo.GetMultiInventory();
      }
      return MainGame.me.player.GetMultiInventoryForInteraction();
    }
  }

  public void CommonOpen(WorldGameObject o, CraftDefinition.CraftType allowed_type)
  {
    Debug.Log((object) ("Open craft: " + o.obj_id));
    this.craftery_wgo = o;
    if ((UnityEngine.Object) this.object_info != (UnityEngine.Object) null)
      this.object_info.Draw(o.GetUniversalObjectInfo());
    base.Open();
    this.craft_component = o.components.craft;
    if (this.craft_component == null)
    {
      Debug.LogError((object) ("WorkbenchComponent not found in object " + o?.ToString()), (UnityEngine.Object) o);
    }
    else
    {
      this.crafts.Clear();
      List<CraftDefinition> craftDefinitionList;
      if (this.crafts_inventory == null)
      {
        craftDefinitionList = this.craft_component.crafts;
      }
      else
      {
        craftDefinitionList = new List<CraftDefinition>();
        if (this.crafts_inventory.is_building)
        {
          foreach (ObjectCraftDefinition objectCrafts in this.crafts_inventory.GetObjectCraftsList())
          {
            if (MainGame.me.save.IsCraftVisible((CraftDefinition) objectCrafts))
              craftDefinitionList.Add((CraftDefinition) objectCrafts);
          }
        }
        else
          craftDefinitionList.AddRange((IEnumerable<CraftDefinition>) this.crafts_inventory.GetCraftsList());
      }
      foreach (CraftDefinition craft in craftDefinitionList)
      {
        if (MainGame.me.save.IsCraftVisible(craft) && craft.craft_type == allowed_type && (allowed_type != CraftDefinition.CraftType.AlchemyDecompose || MainGame.me.save.IsSurveyComplete(CraftDefinition.CraftSubType.Alchemy, craft.needs[0].id)))
          this.crafts.Add(craft);
      }
    }
  }

  public void OverrideSettings(WorldGameObject wgo)
  {
    this.craftery_wgo = wgo;
    this.craft_component = wgo.components.craft;
  }

  public override void Open()
  {
    throw new Exception("Can't call Open() for a craft gui without any parameter.");
  }

  public bool CanCraft(
    CraftDefinition craft,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null)
  {
    if (craft == null)
      return false;
    if (craft.can_craft_always)
      return true;
    return (!(craft is ObjectCraftDefinition) || (craft as ObjectCraftDefinition).enabled) && (!GlobalCraftControlGUI.is_global_control_active || craft.gratitude_points_craft_cost == null || this.craftery_wgo.components.craft.CanSpendPlayerGratitudePoints(craft.gratitude_points_craft_cost.EvaluateFloat(MainGame.me.player))) && this.multi_inventory.IsEnoughItems(override_needs ?? craft.needs, multiquality_ids: multiquality_ids, multiplier: amount) && this.craftery_wgo.data.IsEnoughItems(craft.needs_from_wgo, amount) && (craft.item_needs.Count <= 0 || this.craftery_wgo.data.inventory.Count >= 0 && this.craftery_wgo.data.inventory[0].IsEnoughItems(craft.item_needs)) && craft.condition.EvaluateBoolean(this.craftery_wgo, MainGame.me.player);
  }

  public virtual bool OnCraft(
    CraftDefinition craft,
    Item try_use_particular_item = null,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null,
    WorldGameObject other_obj_override = null)
  {
    Debug.Log((object) ("OnCraft " + craft.id), (UnityEngine.Object) this);
    if (!this.CanCraft(craft, multiquality_ids, override_needs: override_needs))
    {
      Debug.LogError((object) ("Can't start a craft: " + craft.id));
      return false;
    }
    if (this.IsBuildMode())
    {
      if (craft.id == "_remove_")
      {
        this.Hide();
        BuildGrid.ShowBuildGrid(false);
        MainGame.me.EnterBuildMode(true);
        return true;
      }
      if (!MainGame.me.build_mode_logics.CanBuild(craft))
        return false;
      MainGame.me.build_mode_logics.CraftBuilding(craft);
      this.RememberCraft(craft.id);
      return true;
    }
    CraftComponent craftComponent = this.craft_component;
    CraftDefinition craft1 = craft;
    Item try_use_particular_item1 = try_use_particular_item;
    List<string> multiquality_ids1 = multiquality_ids;
    int num = amount;
    List<Item> override_needs1 = override_needs;
    int amount1 = num;
    WorldGameObject other_obj_override1 = other_obj_override;
    if (!craftComponent.CraftAsPlayer(craft1, try_use_particular_item1, multiquality_ids1, override_needs1, amount: amount1, other_obj_override: other_obj_override1))
    {
      Debug.LogError((object) "Craft not started");
      return false;
    }
    if (craft.dont_close_window_on_craft)
    {
      this.Redraw();
    }
    else
    {
      if (GlobalCraftControlGUI.is_global_control_active)
        GUIElements.me.global_craft_control_gui.Open();
      this.Hide();
    }
    if (GUIElements.me.rat_cell_gui.is_shown)
      GUIElements.me.rat_cell_gui.Hide(false);
    return true;
  }

  public virtual void Redraw()
  {
  }

  public void RememberCraft(string craft_id)
  {
    if ((UnityEngine.Object) this.craftery_wgo == (UnityEngine.Object) null)
      return;
    if (BaseCraftGUI.last_crafts.ContainsKey(this.craftery_wgo.unique_id))
      BaseCraftGUI.last_crafts[this.craftery_wgo.unique_id] = craft_id;
    else
      BaseCraftGUI.last_crafts.Add(this.craftery_wgo.unique_id, craft_id);
  }

  public bool IsBuildMode() => this.crafts_inventory != null && this.crafts_inventory.is_building;

  public override void OnClosePressed()
  {
    if ((UnityEngine.Object) this.craftery_wgo != (UnityEngine.Object) null && BaseCraftGUI.last_crafts.ContainsKey(this.craftery_wgo.unique_id))
      BaseCraftGUI.last_crafts.Remove(this.craftery_wgo.unique_id);
    base.OnClosePressed();
    if (this.IsBuildMode())
    {
      MainGame.me.build_mode_logics.SetCurrentBuildZone(string.Empty);
      MainGame.me.player.components.interaction.RedrawCurrentInteractiveHint();
    }
    if (!GlobalCraftControlGUI.is_global_control_active)
      return;
    GUIElements.me.global_craft_control_gui.Open();
  }

  public ButtonTipsStr GetButtonTips() => this.button_tips;

  public GamepadNavigationController GetGamepadController() => this.gamepad_controller;

  public WorldGameObject GetCrafteryWGO() => this.craftery_wgo;
}
