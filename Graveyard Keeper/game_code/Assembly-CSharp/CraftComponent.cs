// Decompiled with JetBrains decompiler
// Type: CraftComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class CraftComponent : WorldGameObjectComponent
{
  public const float DROP_OFFSET = 48f;
  public CraftDefinition current_craft;
  public List<CraftDefinition> crafts = new List<CraftDefinition>();
  [Space]
  public bool is_crafting;
  public WorldGameObject other_obj;
  public MultiInventory used_multi_inventory;
  public Item _current_item;
  public Item _dur_item;
  public List<Item> _cur_craft_items_used = new List<Item>();
  public CraftDefinition.MultiqualityCraftResult _multiquality_craft_result;
  public string _multiquality_craft_item_id;
  public int craft_amount = 1;
  public const float AUTO_CRAFT_RECALC_PERIOD = 0.5f;
  public static HashSet<CraftComponent> _all_crafts = new HashSet<CraftComponent>();
  public static bool _all_crafts_iterating = false;
  public static List<CraftComponent> _crafts_to_add;
  public static List<CraftComponent> _crafts_to_del;
  public int _cur_last_craft_slot;
  public string _last_craft_id = "";
  public string _last_craft_id_2 = "";
  public float _time_worker_tried_to_start_craft;
  public const float TIME_BETWEEN_TRIES_TO_START_CRAFT = 1f;
  public bool _worker_is_paused;
  public float _time_worker_tried_to_continue_craft;
  public const float TIME_BETWEEN_TRIES_TO_CONTINUE_CRAFT = 1f;
  public bool is_gratitude_points_spent_for_craft;
  public List<CraftComponent.CraftQueueItem> craft_queue = new List<CraftComponent.CraftQueueItem>();

  public Item current_item => this._current_item;

  public bool worker_is_paused => this._worker_is_paused;

  public int craft_index => this.crafts.IndexOf(this.current_craft);

  public string last_craft_id
  {
    get => this._cur_last_craft_slot == 1 ? this._last_craft_id_2 : this._last_craft_id;
  }

  public override void OnEnabled()
  {
    base.OnEnabled();
    if (CraftComponent._all_crafts.Contains(this))
      return;
    if (CraftComponent._all_crafts_iterating)
      CraftComponent._crafts_to_add.Add(this);
    else
      CraftComponent._all_crafts.Add(this);
  }

  public override void OnDisabled()
  {
    base.OnDisabled();
    if (!CraftComponent._all_crafts.Contains(this))
      return;
    if (CraftComponent._all_crafts_iterating)
      CraftComponent._crafts_to_del.Add(this);
    else
      CraftComponent._all_crafts.Remove(this);
  }

  public bool has_visible_crafts
  {
    get
    {
      foreach (CraftDefinition craft in this.crafts)
      {
        if (!craft.hidden)
          return true;
      }
      return false;
    }
  }

  public override void StartComponent()
  {
    if (!Application.isPlaying || this.started)
      return;
    base.StartComponent();
    this.FillCraftsList();
    this.started = true;
  }

  public void FillCraftsList()
  {
    this.crafts.Clear();
    this.crafts.AddRange((IEnumerable<CraftDefinition>) GameBalance.me.GetCraftsForObject(this.wgo.obj_id));
  }

  public WorldGameObject GetOtherObj() => this.other_obj;

  public override bool DoAction(
    WorldGameObject other_obj,
    float delta_time,
    bool for_gratitude_points = false)
  {
    if (!this.is_crafting || other_obj.is_player && (this.current_craft.is_auto || this.current_craft.hidden))
      return false;
    this.other_obj = other_obj;
    this.used_multi_inventory = this.other_obj.GetMultiInventory();
    float k;
    if (!for_gratitude_points)
    {
      if (this.other_obj.is_player)
      {
        if (!this.GetCraftCoeffForPlayer(out k))
          return false;
      }
      else if ((UnityEngine.Object) this.wgo == (UnityEngine.Object) this.other_obj)
        k = 1f;
      else if (this.other_obj.IsWorker())
      {
        k = this.other_obj.data.GetParam("working_k");
        ItemDefinition.ItemType tool_type = ItemDefinition.ItemType.None;
        float num = 0.0f;
        ToolActions toolActions = this.wgo.obj_def.tool_actions;
        for (int index = 0; index < toolActions.action_tools.Count; ++index)
        {
          if ((double) toolActions.action_k[index] > (double) num)
          {
            num = toolActions.action_k[index];
            tool_type = toolActions.action_tools[index];
          }
        }
        if (tool_type == ItemDefinition.ItemType.None)
          tool_type = ItemDefinition.ItemType.Hand;
        this.other_obj.components.character.SetWorkerToolNum(tool_type);
      }
      else
        k = 1f;
    }
    else
      k = 0.125f;
    if (!for_gratitude_points)
    {
      if (other_obj.is_player)
      {
        if (!this.TrySpendPlayerEnergy(other_obj, delta_time))
          return false;
        this.SpendPlayerSanity(other_obj, delta_time);
        this.wgo.OnWorkAction();
      }
      else if (this.wgo.has_linked_worker)
        this.wgo.OnWorkAction();
    }
    if ((double) this.current_craft.craft_time.EvaluateFloat(this.wgo, MainGame.me.player) <= 0.001)
      this.wgo.progress = 1f;
    else
      this.wgo.progress += k * delta_time / this.current_craft.craft_time.EvaluateFloat(this.wgo, MainGame.me.player);
    if (!string.IsNullOrEmpty(this.current_craft.game_res_to_mirror_name) && (double) this.current_craft.game_res_to_mirror_max > 0.0)
      this.wgo.SetParam(this.current_craft.game_res_to_mirror_name, this.wgo.progress * this.current_craft.game_res_to_mirror_max);
    if ((double) this.wgo.progress < 1.0)
    {
      this.FastRedrawWhileInProgress();
      return false;
    }
    this.FinishCurrentCraft();
    return true;
  }

  public void FinishCurrentCraft()
  {
    if (this.current_craft.craft_type == CraftDefinition.CraftType.Survey && this.current_craft.sub_type != CraftDefinition.CraftSubType.SurveySciencePoints)
      this.ShowSurveyCompleteWindow(this.current_craft);
    else
      this.ProcessFinishedCraft();
  }

  public void ProcessFinishedCraft()
  {
    this.wgo.OnBeganObjectModifications();
    if (this.current_craft.flag != 1)
    {
      List<Item> cant_insert1 = new List<Item>();
      if (this.current_craft.IsBodyPartExtractionCraft() && this._current_item != null)
      {
        List<Item> objList = ResModificator.ProcessItemsListBeforeDrop(this.current_craft.output, this.wgo, MainGame.me.player, this._current_item);
        objList.Add(this._current_item);
        if (!this.wgo.is_current_craft_gratitude)
          this.wgo.DropItems(objList);
        else
          this.DistributeDropsFromSoulsCraft(objList);
      }
      else if (this.current_craft.IsBodyPartInsertionCraft() && this._current_item != null)
        (this.wgo.data.inventory.Count > 0 ? this.wgo.data.inventory[0] : (Item) null)?.AddItem(this._current_item);
      else if (this.other_obj.is_player || this.current_craft.is_auto)
      {
        if (this.current_craft.IsMultiqualityOutput() && this._multiquality_craft_result != null)
        {
          cant_insert1 = this.DropMultiqualityOutput(false);
        }
        else
        {
          cant_insert1 = ResModificator.ProcessItemsListBeforeDrop(this.current_craft.output, this.wgo, this.other_obj);
          if (this.wgo.obj_id == "refugee_camp_cooking_table" || this.wgo.obj_id == "refugee_camp_cooking_table_2" || this.wgo.obj_id == "refugee_camp_hive")
            WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_depot").AddToInventory(cant_insert1);
          else if (this.wgo.obj_id == "refugee_camp_well")
          {
            WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_well").AddToInventory(cant_insert1);
          }
          else
          {
            if (this.wgo.obj_id == "tavern_kitchen" || this.wgo.obj_id == "tavern_oven")
            {
              WorldGameObject gameObjectByObjId = WorldMap.GetWorldGameObjectByObjId("npc_tavern_barman");
              if ((UnityEngine.Object) gameObjectByObjId == (UnityEngine.Object) null)
              {
                Debug.LogError((object) "Can not put tavern_kitchen output to barmen: not found barmen WGO! Call Bulat. #2");
              }
              else
              {
                int count1 = cant_insert1.Count;
                gameObjectByObjId.TryPutToInventory(cant_insert1, out cant_insert1);
                int count2 = cant_insert1.Count;
                if (count1 > count2)
                  this.wgo.SetParam("do_roll_anim", 1f);
              }
            }
            if (this.wgo.is_current_craft_gratitude)
              this.DistributeDropsFromSoulsCraft(cant_insert1);
            else
              this.wgo.DropItems(cant_insert1);
          }
        }
      }
      else if (this.HasLinkedWorker() || this.wgo.is_current_craft_gratitude)
      {
        bool flag = true;
        if (this.current_craft.IsMultiqualityOutput() && this._multiquality_craft_result != null)
        {
          cant_insert1 = this.DropMultiqualityOutput(true);
          flag = false;
        }
        else
          cant_insert1 = ResModificator.ProcessItemsListBeforeDrop(this.current_craft.output, this.wgo, MainGame.me.player);
        List<Item> items = new List<Item>();
        for (int index = 0; index < cant_insert1.Count; ++index)
        {
          if (cant_insert1[index].is_tech_point)
          {
            if (this.wgo.is_current_craft_gratitude)
              items.Add(cant_insert1[index]);
            cant_insert1.RemoveAt(index);
            --index;
          }
        }
        List<Item> objList;
        if (flag && !this.wgo.CanPutToAllPossibleInventories(cant_insert1, out objList))
        {
          if (this.wgo.is_current_craft_gratitude)
          {
            List<Item> cant_insert2;
            this.wgo.PutToAllPossibleInventories(objList, out cant_insert2);
            if (cant_insert2 != null && cant_insert2.Count > 0)
            {
              this.wgo.progress = 0.999999f;
              this.SetWorkerPausedMode(true);
              this._time_worker_tried_to_continue_craft = 1f;
              return;
            }
            this.wgo.DropItems(items);
          }
          else
          {
            this.wgo.progress = 0.999999f;
            this.SetWorkerPausedMode(true);
            this._time_worker_tried_to_continue_craft = 1f;
            this.wgo.linked_worker.components.character.SetNoWorkerTool();
            return;
          }
        }
        else
        {
          this.wgo.PutToAllPossibleInventories(cant_insert1, out objList);
          if (objList != null && objList.Count > 0)
            this.wgo.DropItems(objList);
          this.wgo.DropItems(items);
        }
      }
      else
        this.other_obj.AddToInventory(ResModificator.ProcessItemsListBeforeDrop(this.current_craft.output, this.wgo, this.other_obj));
      if (this.wgo.is_current_craft_gratitude && this.worker_is_paused)
        this.SetWorkerPausedMode(false);
      this.wgo.AddToParams(this.current_craft.output_res_wgo);
      foreach (Item obj1 in this.current_craft.output_to_wgo)
      {
        if (obj1.value > 0)
        {
          this.wgo.AddToInventory(obj1);
        }
        else
        {
          if (obj1.definition.has_durability && obj1.value == -1)
          {
            foreach (Item obj2 in cant_insert1)
            {
              if (obj2.id == obj1.id)
              {
                Item lastItem = this.wgo.data.GetLastItem(obj1.id);
                obj2.durability = lastItem.durability;
                break;
              }
            }
          }
          this.wgo.data.RemoveItem(obj1.id, Mathf.Abs(obj1.value));
        }
      }
      if (this.current_craft.craft_type == CraftDefinition.CraftType.Survey && this.current_craft.sub_type != CraftDefinition.CraftSubType.SurveySciencePoints || this.current_craft.takes_item_durability)
      {
        if (this._current_item == null)
        {
          if (this.current_craft.craft_type == CraftDefinition.CraftType.Survey)
            this.wgo.DropItem(new Item(string.IsNullOrEmpty(this._multiquality_craft_item_id) ? this.current_craft.needs[0].id : this._multiquality_craft_item_id, 1));
          if (string.IsNullOrEmpty(this._multiquality_craft_item_id) || this.current_craft.craft_type == CraftDefinition.CraftType.None && this._dur_item != null)
          {
            if (this._dur_item != null)
              this.DropDurItem();
            else
              Debug.LogError((object) "Can't give back a craft item because it is null");
          }
        }
        else if (this._dur_item == null)
        {
          if (this._current_item.GetParamInt("taken_from_player_inventory") == 1 || this.current_craft.craft_type == CraftDefinition.CraftType.Survey)
          {
            this.wgo.DropItem(this._current_item, Direction.ToPlayer);
          }
          else
          {
            WorldGameObject wgo = this.wgo;
            List<Item> drop_list = new List<Item>();
            drop_list.Add(this._current_item);
            List<Item> objList;
            ref List<Item> local = ref objList;
            wgo.PutToAllPossibleInventories(drop_list, out local);
            if (objList != null && objList.Count > 0)
              this.wgo.DropItem(objList[0]);
          }
        }
        else
          this.DropDurItem();
      }
      foreach (SmartExpression outItemsExpression in this.current_craft.out_items_expressions)
        outItemsExpression.Evaluate(cant_insert1);
    }
    if (!this.current_craft.set_out_wgo_params_on_start)
      this.wgo.SetParam(this.current_craft.output_set_res_wgo);
    int num = this.other_obj.is_player ? 1 : 0;
    if (this.HasLinkedWorker())
      this.wgo.linked_worker.components.character.SetNoWorkerTool();
    Item obj = this.wgo.data.inventory.Count > 0 ? this.wgo.data.inventory[0] : (Item) null;
    if (obj != null)
    {
      obj.AddToParams(this.current_craft.itempars_add);
      obj.SetParam(this.current_craft.itempars_set);
      obj.AddNotFoldedItemsWithoutCheck(ResModificator.ProcessItemsListBeforeDrop(this.current_craft.item_output, this.wgo, MainGame.me.player));
    }
    if (!string.IsNullOrEmpty(this.current_craft.end_script))
    {
      if (this.current_craft.end_script.StartsWith("g:"))
        GS.RunFlowScript(this.current_craft.end_script.Substring(2));
      else if (!this.current_craft.end_script.StartsWith(":") && this.current_craft.end_script.Contains(":"))
      {
        CustomFlowScript customFlowScript = GS.RunFlowScript(this.current_craft.end_script.Split(':')[0]);
        customFlowScript.StartBehaviour();
        if (this.current_craft.end_script.Split(':').Length > 2)
          customFlowScript.FireEvent(this.current_craft.end_script.Split(':')[1], this.current_craft.end_script.Split(':')[2]);
        else
          customFlowScript.FireEvent(this.current_craft.end_script.Split(':')[1]);
      }
      else
        this.wgo.AttachFlowScript(this.current_craft.end_script);
    }
    if (!string.IsNullOrEmpty(this.current_craft.end_event))
      this.wgo.FireEvent(this.current_craft.end_event);
    if (!string.IsNullOrEmpty(this.current_craft.change_wgo))
    {
      if (this.current_craft.use_variations)
        this.wgo.ReplaceWithObject(this.current_craft.change_wgo, this.current_craft.puff_when_replaced, this.current_craft.variation_index);
      else
        this.wgo.ReplaceWithObject(this.current_craft.change_wgo, this.current_craft.puff_when_replaced);
    }
    if (!string.IsNullOrEmpty(this.current_craft.craft_after_finish))
    {
      string craft_to_start_name = this.current_craft.craft_after_finish;
      GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() =>
      {
        CraftDefinition dataOrNull = GameBalance.me.GetDataOrNull<CraftDefinition>(craft_to_start_name);
        if (dataOrNull == null)
        {
          Debug.LogError((object) $"Can not start craft: craft [{craft_to_start_name}] is null!");
        }
        else
        {
          if (this.wgo.components.craft.Craft(dataOrNull))
            return;
          Debug.LogError((object) "Failed to start craft!");
        }
      }));
    }
    MainGame.me.save.OnFinishedCraft(this.current_craft);
    if (!string.IsNullOrEmpty(this.wgo.obj_def.anim_on_craft_finish))
      this.wgo.TriggerSmartAnimation(this.wgo.obj_def.anim_on_craft_finish);
    this.End();
    this.wgo.Redraw();
  }

  public void DropDurItem()
  {
    if (this._dur_item.definition.dont_break_on_zero_dur || this._dur_item.durability_state != Item.DurabilityState.Broken)
    {
      if (this._dur_item.GetParamInt("taken_from_player_inventory") == 1 || this.current_craft.craft_type == CraftDefinition.CraftType.Survey)
      {
        this.wgo.DropItem(this._dur_item, Direction.ToPlayer);
      }
      else
      {
        List<Item> cant_insert = new List<Item>();
        cant_insert.Add(this._dur_item);
        this.wgo.PutToAllPossibleInventories(cant_insert, out cant_insert);
        if (cant_insert != null && cant_insert.Count > 0)
          this.wgo.DropItem(cant_insert[0]);
      }
    }
    this._dur_item = (Item) null;
  }

  public List<Item> DropMultiqualityOutput(bool do_not_really_drop)
  {
    int index1 = 0;
    List<Item> objList = new List<Item>();
    float[] qualityProbabilities = this._multiquality_craft_result.quality_probabilities;
    for (int index2 = qualityProbabilities.Length - 1; index2 >= 0; --index2)
    {
      if ((double) UnityEngine.Random.value < (double) qualityProbabilities[index2])
      {
        index1 = index2;
        break;
      }
    }
    foreach (Item obj1 in this.current_craft.output)
    {
      bool flag = false;
      if (obj1.is_multiquality)
      {
        foreach (string multiqualityItem in obj1.multiquality_items)
        {
          if ((double) Mathf.Abs((float) ((double) GameBalance.me.GetData<ItemDefinition>(multiqualityItem).quality - (double) index1 - 1.0)) < 0.0099999997764825821)
          {
            flag = true;
            break;
          }
        }
      }
      if (!flag)
      {
        if (!do_not_really_drop)
          this.wgo.DropItem(obj1);
        objList.Add(obj1);
      }
      else
      {
        Item obj2 = new Item(obj1.multiquality_items[index1], obj1.value);
        objList.Add(obj2);
        if (!do_not_really_drop)
        {
          if (this.wgo.obj_id == "tavern_kitchen" || this.wgo.obj_id == "tavern_oven")
          {
            WorldGameObject gameObjectByObjId = WorldMap.GetWorldGameObjectByObjId("npc_tavern_barman");
            if ((UnityEngine.Object) gameObjectByObjId == (UnityEngine.Object) null)
            {
              Debug.LogError((object) "Can not put tavern_kitchen output to barmen: not found barmen WGO! Call Bulat. #3");
              this.wgo.DropItem(obj2);
            }
            else
            {
              WorldGameObject worldGameObject = gameObjectByObjId;
              List<Item> items_to_insert = new List<Item>();
              items_to_insert.Add(obj2);
              List<Item> items;
              ref List<Item> local = ref items;
              worldGameObject.TryPutToInventory(items_to_insert, out local);
              if (items != null && items.Count > 0)
                this.wgo.DropItems(items);
              else
                this.wgo.SetParam("do_roll_anim", 1f);
            }
          }
          else if (this.wgo.obj_id == "refugee_camp_cooking_table" || this.wgo.obj_id == "refugee_camp_cooking_table_2" || this.wgo.obj_id == "refugee_camp_hive")
            WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_depot").AddToInventory(obj2);
          else if (this.wgo.obj_id == "refugee_camp_well")
            WorldMap.GetWorldGameObjectByCustomTag("refugee_camp_well").AddToInventory(obj2);
          else
            this.wgo.DropItem(obj2);
        }
      }
    }
    return objList;
  }

  public void FastRedrawWhileInProgress() => this.wgo.custom_drawers.FastRedraw();

  public Vector3 GetDropBos(WorldGameObject target_obj = null)
  {
    if ((UnityEngine.Object) target_obj == (UnityEngine.Object) null)
      target_obj = MainGame.me.player;
    return target_obj.tf.position + target_obj.components.character.anim_direction.ClockwiseDir().ToVec3() * 48f;
  }

  public override bool Interact(WorldGameObject other_obj, float delta_time)
  {
    if (!this.CanInteractCraft())
      return false;
    this.other_obj = other_obj;
    this.used_multi_inventory = other_obj.GetMultiInventory();
    GUIElements.me.OpenCraftGUI(this.wgo);
    return false;
  }

  public bool CanInteractCraft()
  {
    return (this.wgo.obj_def.can_insert_zombie || !this.is_crafting) && !this.wgo.is_removing && this.has_visible_crafts && this.wgo.obj_def.interaction_type == ObjectDefinition.InteractionType.Craft;
  }

  public bool CraftAsPlayer(
    CraftDefinition craft,
    Item try_use_particular_item = null,
    List<string> multiquality_ids = null,
    List<Item> override_needs = null,
    bool ignore_crafts_list = false,
    int amount = 1,
    WorldGameObject other_obj_override = null)
  {
    this.other_obj = (UnityEngine.Object) other_obj_override != (UnityEngine.Object) null ? other_obj_override : MainGame.me.player;
    if (craft == null)
    {
      this._last_craft_id = "";
      this._cur_last_craft_slot = 0;
    }
    else
    {
      if (this._cur_last_craft_slot == 1)
        this._last_craft_id_2 = craft.id;
      else
        this._last_craft_id = craft.id;
      this._cur_last_craft_slot = craft.store_last_craft_slot;
    }
    return !this.IsCraftQueueEmpty() || this.is_crafting || this.Craft(craft, try_use_particular_item, multiquality_ids, override_needs, ignore_crafts_list, amount);
  }

  public void StartRemovalCraft(CraftDefinition craft)
  {
    if (this.wgo.has_linked_worker)
    {
      this.wgo.DropItem(this.wgo.linked_worker.worker.GetOnGroundItem());
      WorldMap.RemoveZombieWorkerToStock(this.wgo.linked_worker);
    }
    if (this.is_crafting && this.current_craft != null)
    {
      Item obj = new Item() { inventory_size = 100 };
      for (int index = 0; index < this.craft_amount; ++index)
        obj.AddItems(this.current_craft.needs, false);
      this.wgo.DropItems(obj.inventory);
      this.craft_amount = 0;
      if (this.current_craft.output_to_wgo_on_start.Count > 0)
        this.wgo.data.RemoveItems(this.current_craft.output_to_wgo_on_start);
    }
    this.enabled = true;
    this.is_crafting = true;
    this.craft_queue = new List<CraftComponent.CraftQueueItem>();
    this.current_craft = craft;
    this._multiquality_craft_item_id = (string) null;
    if (!this.crafts.Contains(craft))
      this.crafts.Add(craft);
    this.wgo.progress = 0.0f;
    this.wgo.is_current_craft_gratitude = false;
    this.wgo.RedrawBubble();
  }

  public virtual void EnqueueCraft(
    CraftDefinition craft,
    List<string> multiquality_ids,
    int amount,
    bool can_use_player_inventory = false)
  {
    if (craft == null)
    {
      Debug.LogError((object) "Trying to enqueue a null craft");
    }
    else
    {
      Debug.Log((object) $"EnqueueCraft {craft.id}, amount={amount}");
      bool flag = true;
      foreach (CraftComponent.CraftQueueItem craft1 in this.craft_queue)
      {
        if (craft1.id == craft.id)
        {
          if (GlobalCraftControlGUI.is_global_control_active)
          {
            if (craft1.is_gratitude_points_craft)
              flag = false;
          }
          else if (!craft1.is_gratitude_points_craft)
            flag = false;
          if (!flag)
          {
            craft1.n += amount;
            break;
          }
        }
      }
      if (flag)
      {
        this.craft_queue.Add(new CraftComponent.CraftQueueItem()
        {
          id = craft.id,
          n = amount,
          is_gratitude_points_craft = GlobalCraftControlGUI.is_global_control_active
        });
        if (GlobalCraftControlGUI.is_global_control_active)
        {
          this.TryStartCraftFromQueue(start_by_player: false);
          this.RefreshComponentBubbleData(false);
        }
      }
      this.TryStartCraftFromQueue(can_use_player_inventory, !GlobalCraftControlGUI.is_global_control_active);
    }
  }

  public bool HasGratitudeCraftInQueue()
  {
    for (int index = 0; index < this.craft_queue.Count; ++index)
    {
      if (this.craft_queue[index].is_gratitude_points_craft)
        return true;
    }
    return false;
  }

  public bool IsCraftQueueEmpty()
  {
    if (this.craft_queue == null || this.craft_queue.Count == 0)
      return true;
    for (int index = this.craft_queue.Count - 1; index >= 0; --index)
    {
      if (this.craft_queue[index]?.craft == null)
        this.craft_queue.RemoveAt(index);
    }
    return false;
  }

  public CraftComponent.CraftQueueItem CanStartCraftFromQueue(
    bool use_player_inventory = false,
    bool start_by_player = true)
  {
    if (this.IsCraftQueueEmpty())
      return (CraftComponent.CraftQueueItem) null;
    MultiInventory multiInventory = !(GlobalCraftControlGUI.is_global_control_active & use_player_inventory) ? this.wgo.GetMultiInventory(player_mi: use_player_inventory ? MultiInventory.PlayerMultiInventory.IncludePlayer : MultiInventory.PlayerMultiInventory.ExcludePlayer) : (!WorldZone.GetZoneOfObject(this.wgo).IsPlayerInZone() ? this.wgo.GetMultiInventory(player_mi: MultiInventory.PlayerMultiInventory.ExcludePlayer) : this.wgo.GetMultiInventory(player_mi: use_player_inventory ? MultiInventory.PlayerMultiInventory.IncludePlayer : MultiInventory.PlayerMultiInventory.ExcludePlayer));
    foreach (CraftComponent.CraftQueueItem craft in this.craft_queue)
    {
      if (multiInventory.IsEnoughItems(craft.craft.needs) && this.wgo.data.IsEnoughItems(craft.craft.needs_from_wgo) && craft.craft.condition.EvaluateBoolean(this.wgo, MainGame.me.player))
      {
        if (!start_by_player && craft.is_gratitude_points_craft)
        {
          SmartExpression gratitudePointsCraftCost = craft.craft.gratitude_points_craft_cost;
          if (!this.CanSpendPlayerGratitudePoints(gratitudePointsCraftCost != null ? gratitudePointsCraftCost.EvaluateFloat() : 0.0f))
            continue;
        }
        return craft;
      }
    }
    return (CraftComponent.CraftQueueItem) null;
  }

  public void TryStartCraftFromQueue(bool can_use_player_inventory = false, bool start_by_player = true)
  {
    int num = 0;
    for (int index = 0; index < this.craft_queue.Count; ++index)
    {
      if (this.craft_queue[index].n == 0)
        this.craft_queue.RemoveAt(index--);
    }
    while (!this.is_crafting)
    {
      CraftComponent.CraftQueueItem craftQueueItem = this.CanStartCraftFromQueue(can_use_player_inventory, start_by_player);
      if (craftQueueItem == null)
        break;
      if (!craftQueueItem.infinite && --craftQueueItem.n == 0)
        this.craft_queue.Remove(craftQueueItem);
      this.CraftReally(craftQueueItem.craft, for_gratitude_points: craftQueueItem.is_gratitude_points_craft, use_player_inv: can_use_player_inventory, start_by_player: start_by_player);
      if (!string.IsNullOrEmpty(craftQueueItem.craft.craft_after_finish) || !craftQueueItem.craft.craft_time_is_zero)
        break;
      if (++num > 500)
      {
        Debug.LogError((object) $"TryStartCraftFromQueue iterator is too big, wgo: {this.wgo.name}, craft: {craftQueueItem.id}", (UnityEngine.Object) this.wgo);
        break;
      }
    }
  }

  public virtual bool Craft(
    CraftDefinition craft,
    Item try_use_particular_item = null,
    List<string> multiquality_ids = null,
    List<Item> override_needs = null,
    bool ignore_crafts_list = false,
    int amount = 1)
  {
    Debug.Log((object) ("Craft " + craft?.id));
    if (this.crafts.IndexOf(craft) < 0 && !ignore_crafts_list)
    {
      Debug.LogError((object) $"craft {craft?.id} not found in the wgo's crafts inventory", (UnityEngine.Object) this.wgo);
      string s = "";
      foreach (CraftDefinition craft1 in this.crafts)
        s = s.ConcatWithSeparator(craft1.id, ", ");
      Debug.LogError((object) ("Available crafts: " + (string.IsNullOrEmpty(s) ? "[none]" : s)));
      return false;
    }
    if (this.craft_queue == null)
      this.craft_queue = new List<CraftComponent.CraftQueueItem>();
    return this.CraftReally(craft, try_use_particular_item, multiquality_ids, override_needs, ignore_crafts_list, amount, GlobalCraftControlGUI.is_global_control_active, start_by_player: GlobalCraftControlGUI.is_global_control_active);
  }

  public bool CraftReally(
    CraftDefinition craft,
    Item try_use_particular_item = null,
    List<string> multiquality_ids = null,
    List<Item> override_needs = null,
    bool ignore_crafts_list = false,
    int amount = 1,
    bool for_gratitude_points = false,
    bool use_player_inv = false,
    bool start_by_player = true)
  {
    this._current_item = (Item) null;
    this._cur_craft_items_used = new List<Item>();
    if ((UnityEngine.Object) this.other_obj == (UnityEngine.Object) null)
      this.other_obj = MainGame.me.player;
    this.used_multi_inventory = this.other_obj.is_player ? this.other_obj.GetMultiInventoryForInteraction() : this.other_obj.GetMultiInventory();
    if (for_gratitude_points && !this.other_obj.is_player && (UnityEngine.Object) this.wgo != (UnityEngine.Object) null)
      this.used_multi_inventory = !WorldZone.GetZoneOfObject(this.wgo).IsPlayerInZone() ? this.wgo.GetMultiInventory(player_mi: MultiInventory.PlayerMultiInventory.ExcludePlayer) : this.wgo.GetMultiInventory(player_mi: use_player_inv ? MultiInventory.PlayerMultiInventory.IncludePlayer : MultiInventory.PlayerMultiInventory.ExcludePlayer);
    if (craft.item_needs.Count > 0)
    {
      if (this.wgo.data.inventory.Count > 0)
      {
        Item obj = this.wgo.data.inventory[0];
        foreach (Item itemNeed in craft.item_needs)
        {
          if (obj.GetItemsCount(itemNeed.id) < itemNeed.value)
          {
            Debug.LogError((object) $"Can not start craft {craft.id}: not found item {itemNeed.id} in first item inventory.");
            return false;
          }
        }
        if (!craft.item_needs_leave)
          obj.RemoveItems(new List<Item>((IEnumerable<Item>) craft.item_needs));
      }
      else
      {
        Debug.LogError((object) $"Can not start craft {craft.id}: not found any item in workbench inventory.");
        return false;
      }
    }
    if (for_gratitude_points && !start_by_player)
    {
      int num = 0;
      if (craft.gratitude_points_craft_cost != null)
        num = Mathf.RoundToInt(craft.gratitude_points_craft_cost.EvaluateFloat(MainGame.me.player) * (1f - this.wgo.progress));
      this.TrySpendPlayerGratitudePoints((float) num);
    }
    List<Item> objList = override_needs ?? new List<Item>((IEnumerable<Item>) craft.needs);
    this.craft_amount = amount;
    if (amount > 1)
    {
      for (int index = 0; index < objList.Count; ++index)
        objList[index] = new Item(objList[index].id, objList[index].value * amount);
    }
    if (try_use_particular_item != null && craft.IsBodyPartInsertionCraft())
      this._current_item = try_use_particular_item;
    else if (try_use_particular_item != null && try_use_particular_item.definition.stack_count == 1)
    {
      if (craft.IsBodyPartExtractionCraft())
        this._current_item = try_use_particular_item;
      else if (objList.Count > 0 && objList[0].id == try_use_particular_item.id)
      {
        int itemsCount = MainGame.me.player.data.GetItemsCount(try_use_particular_item.id, true);
        if (this.used_multi_inventory.TryRemoveSpecificItemNoCheck(try_use_particular_item))
        {
          this._current_item = try_use_particular_item;
          this._current_item.SetParam("taken_from_player_inventory", itemsCount - MainGame.me.player.data.GetItemsCount(try_use_particular_item.id, true) > 0 ? 1f : 0.0f);
          objList.RemoveAt(0);
          this._cur_craft_items_used.Add(this._current_item);
        }
      }
    }
    if (craft.takes_item_durability && this._dur_item == null)
    {
      if (craft.needs.Count <= craft.dur_needs_item_index)
      {
        Debug.LogError((object) $"Can't take durability of 'needs' because it is empty, dur_needs_item_index = {craft.dur_needs_item_index.ToString()}, needs.Count = {craft.needs.Count.ToString()}");
        return false;
      }
      Item obj = craft.needs[craft.dur_needs_item_index];
      string id = obj.id;
      if (obj.definition == null && multiquality_ids != null && craft.dur_needs_item_index < multiquality_ids.Count)
        obj = new Item(multiquality_ids[craft.dur_needs_item_index], obj.value);
      if (obj.definition != null && obj.definition.stack_count != 1)
      {
        Debug.LogError((object) ("Balance error: takes_1st_item_durability can be used only with stack_count = 1, item_id = " + obj.id));
        return false;
      }
      this._dur_item = this.used_multi_inventory.GetItem(obj.id, Item.ItemFindLogics.WithLowestDurability);
      if (this._dur_item == null)
      {
        Debug.LogError((object) ("Can't find item in multi-inventory, id = " + obj.id));
        return false;
      }
      int itemsCount = MainGame.me.player.data.GetItemsCount(this._dur_item.id, true);
      this.used_multi_inventory.TryRemoveSpecificItemNoCheck(this._dur_item);
      this._dur_item.SetParam("taken_from_player_inventory", itemsCount - MainGame.me.player.data.GetItemsCount(this._dur_item.id, true) > 0 ? 1f : 0.0f);
      this._cur_craft_items_used.Add(this._dur_item);
      Item.RemoveItemWithIDFromTheList(objList, id, true);
    }
    if (craft.takes_item_durability)
      this._dur_item.durability -= craft.dur_needs_item;
    if (objList.Count > 0)
    {
      int index = 0;
      foreach (Item obj in objList)
      {
        if (multiquality_ids != null && index < multiquality_ids.Count && !string.IsNullOrEmpty(multiquality_ids[index]))
          this._cur_craft_items_used.Add(new Item(multiquality_ids[index], obj.value));
        else
          this._cur_craft_items_used.Add(obj);
        ++index;
      }
      List<Item> out_really_removed_items = new List<Item>();
      if (craft.IsBodyPartInsertionCraft() && this._current_item != null)
      {
        if (this.used_multi_inventory.RemoveItems(new List<Item>()
        {
          new Item(this._current_item)
        }, MultiInventory.DestinationType.AllFromFirst, multiquality_ids))
          ;
      }
      else
      {
        if (!this.used_multi_inventory.RemoveItems(objList, MultiInventory.DestinationType.AllFromFirst, multiquality_ids, out_really_removed_items))
        {
          this._cur_craft_items_used.Clear();
          return false;
        }
        if (out_really_removed_items.Count > 0)
        {
          this._cur_craft_items_used = out_really_removed_items;
          if (craft.takes_item_durability && this._dur_item != null)
            this._cur_craft_items_used.Add(this._dur_item);
        }
      }
    }
    if (!this.wgo.data.RemoveItems(craft.needs_from_wgo, amount))
    {
      Debug.LogError((object) "Not enough needs_from_wgo to craft");
      return false;
    }
    if (craft.transfer_needs_to_wgo)
    {
      if (craft.needs.Count == 1 && this._current_item != null && this._current_item.id == craft.needs[0].id && craft.needs[0].value == 1 && this._current_item.definition.has_durability)
        this.wgo.data.inventory.Add(this._current_item);
      else if (craft.needs.Count == 1 && try_use_particular_item != null && try_use_particular_item.id == craft.needs[0].id)
        this.wgo.data.inventory.Add(try_use_particular_item);
      else
        this.wgo.data.AddItems(craft.needs, false);
    }
    if (craft.set_out_wgo_params_on_start)
      this.wgo.SetParam(craft.output_set_res_wgo);
    foreach (Item obj in craft.output_to_wgo_on_start)
    {
      if (obj.value > 0)
        this.wgo.AddToInventory(obj);
      else
        this.wgo.data.RemoveItem(obj.id, Mathf.Abs(obj.value));
    }
    this.current_craft = craft;
    this._multiquality_craft_item_id = multiquality_ids == null || multiquality_ids.Count == 0 ? (string) null : multiquality_ids[0];
    this._multiquality_craft_result = craft.GetMultiqualityResult(multiquality_ids);
    this.wgo.progress = 0.0f;
    this.wgo.auto_craft_time_spent = 0.0f;
    this.wgo.is_current_craft_gratitude = for_gratitude_points;
    this.is_crafting = true;
    if (!string.IsNullOrEmpty(this.wgo.obj_def.craft_start_sound))
      Sounds.PlaySound(this.wgo.obj_def.craft_start_sound);
    if (!string.IsNullOrEmpty(this.wgo.obj_def.anim_on_craft_start))
      this.wgo.TriggerSmartAnimation(this.wgo.obj_def.anim_on_craft_start);
    this.OnEnabled();
    this.wgo.OnCraftStateChanged();
    if (craft.transfer_needs_to_wgo || craft.needs_from_wgo.Count > 0)
      this.wgo.Redraw();
    if (craft.is_auto | for_gratitude_points && craft.craft_time.EvaluateFloat(this.wgo, MainGame.me.player).EqualsTo(0.0f))
      this.FinishCurrentCraft();
    return true;
  }

  public void CancelRemovalCraft()
  {
    Debug.Log((object) "Canceling removal craft...");
    this.End();
  }

  public virtual void Cancel()
  {
    List<Item> curCraftItemsUsed = this._cur_craft_items_used;
    // ISSUE: explicit non-virtual call
    if ((curCraftItemsUsed != null ? (__nonvirtual (curCraftItemsUsed.Count) > 0 ? 1 : 0) : 0) != 0)
    {
      Debug.Log((object) ("Canceling craft, dropping items: " + this._cur_craft_items_used.JoinToString<Item>()));
      if (this.current_craft != null && this.current_craft.takes_item_durability && this._dur_item != null)
        this._dur_item.durability += this.current_craft.dur_needs_item;
      this.wgo.DropItems(this._cur_craft_items_used);
      this._cur_craft_items_used = new List<Item>();
    }
    if (this.current_craft != null)
      this.wgo.SetParam(this.current_craft.set_when_cancelled);
    else
      Debug.LogError((object) ("Trying to cancel craft when current_craft is null at wgo: " + this.wgo.name), (UnityEngine.Object) this.wgo);
    this.End();
  }

  public virtual void End()
  {
    if (this.destroyed)
      return;
    this.wgo.progress = 0.0f;
    this.wgo.data.RemoveZeroParams();
    this.is_gratitude_points_spent_for_craft = false;
    string str = "NULL";
    try
    {
      if ((UnityEngine.Object) this.other_obj != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.other_obj.gameObject != (UnityEngine.Object) null)
          str = this.other_obj.name;
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) $"WGO instance has no gameObject! other_obj.id = {((UnityEngine.Object) this.other_obj == (UnityEngine.Object) null ? "NULL" : this.other_obj.obj_id)} \nException: {ex?.ToString()}");
    }
    Debug.Log((object) $"Craft end, other_obj = {str}, n = {this.craft_amount}, q = {this.craft_queue?.Count}");
    if (this.craft_amount <= 1)
    {
      this.is_crafting = false;
      this.current_craft = (CraftDefinition) null;
      this._multiquality_craft_item_id = (string) null;
      this._dur_item = (Item) null;
      this._multiquality_craft_result = (CraftDefinition.MultiqualityCraftResult) null;
      this.wgo.is_current_craft_gratitude = false;
      this.TryStartCraftFromQueue((UnityEngine.Object) this.other_obj != (UnityEngine.Object) null && this.other_obj.is_player);
    }
    else
      --this.craft_amount;
    this.wgo.OnCraftStateChanged();
  }

  public override void UnprepareForInteraction()
  {
    if ((UnityEngine.Object) MainGame.me.player.components.character.wgo_hilighted_for_work == (UnityEngine.Object) this.wgo)
      MainGame.me.player.components.character.wgo_hilighted_for_work = (WorldGameObject) null;
    if (!this.wgo.prepared_for_interaction || GUIElements.me.craft.is_just_opened || this.wgo.is_autopsy_table)
      return;
    if (GUIElements.me.craft.is_shown)
      GUIElements.me.craft.Hide(true);
    if (!GUIElements.me.rat_cell_gui.is_shown)
      return;
    GUIElements.me.rat_cell_gui.Hide(true);
  }

  public void RemoveBubble() => InteractionBubbleGUI.RemoveBubble(this.wgo.unique_id, true);

  public override void UpdateComponent(float delta_time)
  {
  }

  public void ReallyUpdateComponent(float delta_time)
  {
    if (!this.is_crafting && (double) this._time_worker_tried_to_start_craft <= 0.0 && this.HasLinkedWorker() && !this.IsCraftQueueEmpty())
    {
      this.TryStartCraftFromQueue();
      this._time_worker_tried_to_start_craft = 1f;
    }
    if (!this.is_crafting && this.HasGratitudeCraftInQueue())
    {
      WorldZone myWorldZone = this.wgo.GetMyWorldZone();
      this.TryStartCraftFromQueue((UnityEngine.Object) myWorldZone != (UnityEngine.Object) null && myWorldZone.IsPlayerInZone(), false);
    }
    this._time_worker_tried_to_start_craft -= delta_time;
    if (this.is_crafting && this.current_craft != null)
    {
      if (this.current_craft.is_auto)
      {
        this.wgo.auto_craft_time_spent += delta_time;
        float delta_time1 = this.current_craft.craft_time_is_zero ? 0.01f : 0.5f;
        if (this.current_craft.craft_time.has_expression)
        {
          while ((double) this.wgo.auto_craft_time_spent >= (double) delta_time1)
          {
            this.wgo.auto_craft_time_spent -= delta_time1;
            this.DoAction(this.wgo, delta_time1, false);
          }
        }
      }
      else if (this.HasLinkedWorker() && !this.wgo.is_removing)
      {
        if (this._worker_is_paused)
        {
          this._time_worker_tried_to_continue_craft -= delta_time;
          if ((double) this._time_worker_tried_to_continue_craft < 0.0)
          {
            this._time_worker_tried_to_continue_craft = 1f;
            this.SetWorkerPausedMode(false);
          }
        }
        if (!this._worker_is_paused)
        {
          if ((UnityEngine.Object) this.wgo.linked_worker != (UnityEngine.Object) null)
            this.DoAction(this.wgo.linked_worker, delta_time, false);
          else
            Debug.LogError((object) "ReallyUpdateCraftComponent: Linked Worker is Null");
        }
      }
    }
    if (!this.is_crafting || this.current_craft == null || this.current_craft.is_auto || !this.wgo.is_current_craft_gratitude)
      return;
    if (!this.wgo.components.craft.is_gratitude_points_spent_for_craft)
    {
      int num = 0;
      if (this.current_craft.gratitude_points_craft_cost != null)
        num = Mathf.RoundToInt(this.current_craft.gratitude_points_craft_cost.EvaluateFloat(MainGame.me.player) * (1f - this.wgo.progress));
      if (!this.TrySpendPlayerGratitudePoints((float) num))
        return;
      this.RefreshComponentBubbleData(false);
    }
    this.wgo.auto_craft_time_spent += delta_time;
    float delta_time2 = this.current_craft.craft_time_is_zero ? 0.01f : 0.5f;
    if (!this.current_craft.craft_time.has_expression)
      return;
    while ((double) this.wgo.auto_craft_time_spent >= (double) delta_time2)
    {
      this.wgo.auto_craft_time_spent -= delta_time2;
      this.DoAction(this.wgo, delta_time2, true);
    }
  }

  public bool HasLinkedWorker() => (UnityEngine.Object) this.wgo != (UnityEngine.Object) null && this.wgo.has_linked_worker;

  public override bool HasUpdate() => true;

  public bool GetCraftCoeffForPlayer(out float k)
  {
    Item equippedTool = this.other_obj.GetEquippedTool();
    ItemDefinition.ItemType item_type = equippedTool != null ? equippedTool.definition.type : ItemDefinition.ItemType.None;
    if (this.wgo.is_removing)
    {
      k = 1f;
      return true;
    }
    if (!this.wgo.obj_def.tool_actions.GetToolK(item_type, out k))
      return false;
    if (equippedTool != null)
      k *= equippedTool.definition.efficiency;
    return true;
  }

  public bool TrySpendPlayerEnergy(WorldGameObject player_wgo, float delta_time)
  {
    Item equippedTool = player_wgo.GetEquippedTool();
    float num = 1f;
    if (equippedTool != null && equippedTool.definition != null && equippedTool.definition.tool_energy_k != null && equippedTool.definition.tool_energy_k.has_expression)
      num = equippedTool.definition.tool_energy_k.EvaluateFloat(this.wgo, player_wgo);
    float a = this.current_craft.craft_time.EvaluateFloat(this.wgo, player_wgo);
    if (a.EqualsTo(0.0f))
    {
      Debug.LogWarning((object) ("Time = 0 in craft id = " + this.current_craft.id), (UnityEngine.Object) this.wgo);
      return true;
    }
    float need_energy = this.current_craft.energy.EvaluateFloat(this.wgo, player_wgo) * delta_time / a * num;
    return player_wgo.components.character.player.TrySpendEnergy(need_energy);
  }

  public bool CanSpendPlayerEnergy(WorldGameObject player_wgo, float delta_time)
  {
    if (!this.is_crafting || this.current_craft == null)
      return false;
    float a = this.current_craft.craft_time.EvaluateFloat(this.wgo, player_wgo);
    if (a.EqualsTo(0.0f))
    {
      Debug.LogWarning((object) ("Time = 0 in craft id = " + this.current_craft.id), (UnityEngine.Object) this.wgo);
      return true;
    }
    float num = this.current_craft.energy.EvaluateFloat(this.wgo) * delta_time / a;
    return (double) player_wgo.energy >= (double) num;
  }

  public bool TrySpendPlayerGratitudePoints(float value)
  {
    if (!this.CanSpendPlayerGratitudePoints(value))
      return false;
    MainGame.me.player.gratitude_points -= value;
    this.is_gratitude_points_spent_for_craft = true;
    return true;
  }

  public void ReturnPlayerGratitudePoints()
  {
    if (this.current_craft == null || this.current_craft.gratitude_points_craft_cost == null)
      return;
    float num = this.current_craft.gratitude_points_craft_cost?.EvaluateFloat().Value;
    MainGame.me.player.gratitude_points += num;
  }

  public bool CanSpendPlayerGratitudePoints(float value)
  {
    return (double) MainGame.me.player.gratitude_points >= (double) value;
  }

  public void SpendPlayerSanity(WorldGameObject player_wgo, float delta_time)
  {
    float need_sanity = this.current_craft.sanity.EvaluateFloat(this.wgo) * delta_time / this.current_craft.craft_time.EvaluateFloat(this.wgo, player_wgo);
    player_wgo.GetComponent<PlayerComponent>().SpendSanity(need_sanity);
  }

  public override void UpdateEnableState(ObjectDefinition.ObjType obj_type)
  {
    this.enabled = this.wgo.obj_def.has_craft || this.wgo.is_removing;
  }

  public SerializableWGO.SerializableCraft GetSerializedCraftComponent()
  {
    SerializableWGO.SerializableCraft serializedCraftComponent = new SerializableWGO.SerializableCraft();
    serializedCraftComponent.available = this.enabled;
    serializedCraftComponent.multiquality_item_id = this._multiquality_craft_item_id;
    serializedCraftComponent.multiquality_craft_result = this._multiquality_craft_result;
    serializedCraftComponent.last_craft_id = this._last_craft_id;
    serializedCraftComponent.last_craft_id_2 = this._last_craft_id_2;
    serializedCraftComponent.cur_last_craft_slot = this._cur_last_craft_slot;
    serializedCraftComponent.cur_craft_items_used = this._cur_craft_items_used;
    if (!this.enabled)
      return serializedCraftComponent;
    serializedCraftComponent.is_crafting = this.is_crafting && this.current_craft != null;
    if (serializedCraftComponent.is_crafting)
    {
      serializedCraftComponent.cur_craft_id = this.current_craft.id;
      serializedCraftComponent.craft_amount = this.craft_amount;
    }
    serializedCraftComponent.queue = this.craft_queue;
    if (this._current_item == null)
    {
      serializedCraftComponent.cur_item_id = "";
    }
    else
    {
      serializedCraftComponent.cur_item_id = this._current_item.id;
      serializedCraftComponent.cur_item_dur = this._current_item.durability;
    }
    if (this._dur_item == null)
    {
      serializedCraftComponent.dur_item_id = "";
    }
    else
    {
      serializedCraftComponent.dur_item_id = this._dur_item.id;
      serializedCraftComponent.dur_item_dur = this._dur_item.durability;
    }
    serializedCraftComponent.is_gratitude_points_spent_for_craft = this.is_gratitude_points_spent_for_craft;
    return serializedCraftComponent;
  }

  public CraftDefinition DeserializeCraftDefinition(string craft_id)
  {
    return string.IsNullOrEmpty(craft_id) ? (CraftDefinition) null : GameBalance.me.GetDataOrNull<CraftDefinition>(craft_id) ?? (CraftDefinition) GameBalance.me.GetData<ObjectCraftDefinition>(craft_id);
  }

  public void DeserializeCraftComponent(SerializableWGO.SerializableCraft data)
  {
    this.enabled = data.available;
    this._last_craft_id = data.last_craft_id;
    this._last_craft_id_2 = data.last_craft_id_2;
    this._cur_last_craft_slot = data.cur_last_craft_slot;
    if (!this.enabled)
      return;
    this.is_crafting = data.is_crafting;
    if (this.is_crafting)
      this.craft_amount = data.craft_amount;
    this._multiquality_craft_item_id = data.multiquality_item_id;
    this._multiquality_craft_result = data.multiquality_craft_result;
    this.current_craft = this.DeserializeCraftDefinition(data.cur_craft_id);
    this.craft_queue = data.queue ?? new List<CraftComponent.CraftQueueItem>();
    this._cur_craft_items_used = data.cur_craft_items_used ?? new List<Item>();
    Item obj1;
    if (!string.IsNullOrEmpty(data.cur_item_id))
      obj1 = new Item(data.cur_item_id, 1)
      {
        durability = data.cur_item_dur
      };
    else
      obj1 = (Item) null;
    this._current_item = obj1;
    Item obj2;
    if (!string.IsNullOrEmpty(data.dur_item_id))
      obj2 = new Item(data.dur_item_id, 1)
      {
        durability = data.dur_item_dur
      };
    else
      obj2 = (Item) null;
    this._dur_item = obj2;
    this.is_gratitude_points_spent_for_craft = data.is_gratitude_points_spent_for_craft;
    this.wgo.OnCraftStateChanged();
    if (!this.is_crafting)
      return;
    this.OnEnabled();
  }

  public void ShowSurveyCompleteWindow(CraftDefinition craft)
  {
    Item obj = craft.needs[0];
    if (GameBalance.me.GetDataOrNull<ItemDefinition>(obj.id) == null)
    {
      List<string> itemsOfBaseName = GameBalance.me.GetItemsOfBaseName(obj.id);
      if (itemsOfBaseName.Count > 0)
      {
        obj = new Item(itemsOfBaseName[0], obj.value);
      }
      else
      {
        Debug.LogError((object) ("Couldn't show survey complete window for item id = " + obj.id));
        return;
      }
    }
    ItemDefinition.ItemDetails itemDetails = obj.definition.GetItemDetails();
    if (itemDetails == null)
    {
      Debug.LogError((object) ("Couldn't get survey details for item " + obj.id));
    }
    else
    {
      string text2 = "";
      if (itemDetails.alchemy != null)
      {
        if (itemDetails.alchemy.details_type != ItemDefinition.ItemDetailsAlchemy.DetailsType.Decompose)
          throw new ArgumentOutOfRangeException();
        string str = "";
        foreach (int decompose in itemDetails.alchemy.decomposes)
        {
          if (!string.IsNullOrEmpty(str))
            str += ", ";
          str = $"{str}(alcd{decompose.ToString()}){GJL.L("alc_ingr_" + decompose.ToString())}";
        }
        if (string.IsNullOrEmpty(str))
          str = "-";
        text2 = $"\n{GJL.L("alch_decompose_full")}{GJL.L(":")}\n[AAAEBBFF]{str}[-]";
      }
      GUIElements.me.dialog.OpenDialog(GJL.L("survey_complete_1"), GJL.L("OK"), new GJCommons.VoidDelegate(this.ProcessFinishedCraft), text2: text2, item: new Item(obj.id, 0));
      GUIElements.me.dialog.item_container.GetComponent<BaseItemCellGUI>().quality_icon.gameObject.SetActive(false);
    }
  }

  public static void ClearCraftsListOnGameStart()
  {
    Debug.Log((object) nameof (ClearCraftsListOnGameStart));
    CraftComponent._all_crafts.Clear();
  }

  public static void UpdateAllCrafts(float delta_time)
  {
    CraftComponent._all_crafts_iterating = true;
    CraftComponent._crafts_to_add = new List<CraftComponent>();
    CraftComponent._crafts_to_del = new List<CraftComponent>();
    foreach (CraftComponent allCraft in CraftComponent._all_crafts)
    {
      try
      {
        allCraft.ReallyUpdateComponent(delta_time);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Error while updating craft component, item " + allCraft?._current_item?.ToString()));
        Debug.LogException(ex);
      }
    }
    foreach (CraftComponent craftComponent in CraftComponent._crafts_to_del)
      CraftComponent._all_crafts.Remove(craftComponent);
    foreach (CraftComponent craftComponent in CraftComponent._crafts_to_add)
      CraftComponent._all_crafts.Add(craftComponent);
    CraftComponent._all_crafts_iterating = false;
  }

  public override void RefreshComponentBubbleData(bool show_interaction_buttons)
  {
    BubbleWidgetItemData wdata1 = (BubbleWidgetItemData) null;
    BubbleWidgetProgressData wdata2 = (BubbleWidgetProgressData) null;
    string text1 = "";
    string text2 = "";
    if (this.wgo.obj_def == null)
    {
      Debug.LogError((object) ("Object definition is null for object id = " + this.wgo.obj_id), (UnityEngine.Object) this.wgo);
    }
    else
    {
      ObjectInteractionDefinition validInteraction = this.wgo.obj_def.GetValidInteraction(this.wgo);
      if (!string.IsNullOrEmpty(validInteraction?.hint))
        text1 = GameKeyTip.Get(GameKey.Interaction, validInteraction.hint);
      if (this.is_crafting)
      {
        Item obj = (Item) null;
        bool show_quality = true;
        if (this.current_craft == null)
        {
          Debug.LogError((object) "Current_craft is null", (UnityEngine.Object) this.wgo);
          return;
        }
        if (this.current_craft.hidden)
          return;
        if (this.current_craft.craft_type != CraftDefinition.CraftType.None)
          text1 = "";
        switch (this.current_craft.craft_type)
        {
          case CraftDefinition.CraftType.None:
          case CraftDefinition.CraftType.ResourcesBasedCraft:
          case CraftDefinition.CraftType.AlchemyDecompose:
            if (this.wgo.obj_id == "grave_ground" && this.current_craft.needs.Count > 0 && this.current_craft.id.Contains("set_"))
            {
              obj = new Item(this.current_craft.needs[0]);
              break;
            }
            if (this.current_craft.IsBodyPartInsertionCraft() && this._current_item != null)
            {
              obj = new Item(this._current_item);
              break;
            }
            if (!this.current_craft.id.Contains(":r:") && this.current_craft.GetFirstRealOutput() != null)
            {
              string item_id = this.current_craft.output[0].is_multiquality ? this.current_craft.output[0].multiquality_items[0] : this.current_craft.output[0].id;
              show_quality = !this.current_craft.output[0].is_multiquality;
              obj = new Item(item_id);
              break;
            }
            break;
          case CraftDefinition.CraftType.Survey:
            if (this.current_craft.needs.Count > 0)
            {
              obj = this.current_craft.needs[0];
              break;
            }
            break;
          case CraftDefinition.CraftType.MixedCraft:
            if (!MainGame.me.save.completed_one_time_crafts.Contains(this.current_craft.id))
            {
              obj = new Item("unknown", 1);
              break;
            }
            if (this.current_craft.GetFirstRealOutput() != null)
            {
              obj = new Item(this.current_craft.output[0]);
              break;
            }
            break;
        }
        if (obj != null)
        {
          int num = !this.current_craft.is_auto | show_interaction_buttons ? 1 : 0;
          string item_id = obj.id;
          if (!string.IsNullOrEmpty(this._multiquality_craft_item_id) && this.current_craft.craft_type != CraftDefinition.CraftType.None)
            item_id = this._multiquality_craft_item_id;
          wdata1 = new BubbleWidgetItemData(item_id, show_quality: show_quality, counter: this.GetCraftAnmountCounter(), infinity_counter: this.IsCraftCounterInfinite());
          if (this._worker_is_paused)
            wdata1.cap_limit = true;
          wdata1.is_gratitude = this.wgo.is_current_craft_gratitude;
          wdata1.is_enough_gratitude = this.is_gratitude_points_spent_for_craft || this.current_craft.is_auto;
          if (!string.IsNullOrEmpty(this.current_craft.icon) && this.current_craft.craft_type == CraftDefinition.CraftType.None)
          {
            wdata1.icon_id = this.current_craft.icon;
            if (this.current_craft.hide_quality_icon)
              wdata1.show_quality = false;
          }
        }
        else if (this.wgo.is_removing || this.current_craft.id.Contains(":r:"))
          wdata1 = new BubbleWidgetItemData()
          {
            icon_id = "i_b_remove"
          };
        if (!this.current_craft.hidden && !this.current_craft.is_auto | show_interaction_buttons)
          wdata2 = new BubbleWidgetProgressData((BubbleWidgetProgressData.ProgressDelegate) (() => this.wgo.progress));
        if (this.current_craft.id.StartsWith("camp_kitchen") || this.current_craft.id.StartsWith("refugee_honey_production"))
        {
          wdata1 = new BubbleWidgetItemData(this.current_craft.output[0].id, show_quality: !this.current_craft.output[0].is_multiquality, counter: this.GetCraftAnmountCounter(), infinity_counter: this.IsCraftCounterInfinite());
          wdata2 = new BubbleWidgetProgressData((BubbleWidgetProgressData.ProgressDelegate) (() => this.wgo.progress));
        }
      }
      else if (!this.IsCraftQueueEmpty())
      {
        List<Item> output = this.craft_queue[0].craft.output;
        if (output.Count != 0)
        {
          wdata1 = new BubbleWidgetItemData(output[0].id);
          if (!string.IsNullOrEmpty(this.craft_queue[0].craft.icon))
          {
            wdata1.icon_id = this.craft_queue[0].craft.icon;
            if (this.craft_queue[0].craft.hide_quality_icon)
              wdata1.show_quality = false;
          }
          wdata1.is_gratitude = !this.is_crafting && this.HasGratitudeCraftInQueue();
          wdata1.is_enough_gratitude = false;
        }
      }
      if (show_interaction_buttons && this.CanInteractCraft() && string.IsNullOrEmpty(text1))
        text1 = GameKeyTip.Get(GameKey.Interaction, "craft_hint");
      else if (this.is_crafting && !this.CanInteractCraft())
        text1 = "";
      this.wgo.SetBubbleWidgetData((BubbleWidgetData) wdata1, BubbleWidgetData.WidgetID.CraftingItem);
      this.wgo.SetBubbleWidgetData((BubbleWidgetData) wdata2, BubbleWidgetData.WidgetID.CraftingProgress);
      if (show_interaction_buttons)
      {
        if (this.PlayerCanWork())
        {
          text2 = GameKeyTip.Get(GameKey.Work, "work");
          MainGame.me.player.components.character.wgo_hilighted_for_work = this.wgo;
        }
      }
      else
      {
        text1 = "";
        text2 = "";
      }
      this.wgo.SetBubbleWidgetData(text1, BubbleWidgetData.WidgetID.Interaction);
      if (!string.IsNullOrEmpty(text2))
        this.wgo.SetBubbleWidgetData(text2, BubbleWidgetData.WidgetID.Work);
      if (!MainGame.game_started || !string.IsNullOrEmpty(text2) || !((UnityEngine.Object) MainGame.me.player.components.character.wgo_hilighted_for_work == (UnityEngine.Object) this.wgo))
        return;
      MainGame.me.player.components.character.wgo_hilighted_for_work = (WorldGameObject) null;
    }
  }

  public bool PlayerCanWork()
  {
    return !this.wgo.has_linked_worker && !this.wgo.player_cant_work && (this.is_crafting && !this.current_craft.is_auto || !this.IsCraftQueueEmpty());
  }

  public int GetCraftAnmountCounter()
  {
    if (!this.wgo.obj_def.can_insert_zombie)
      return this.craft_amount;
    if (this.IsCraftQueueEmpty())
      return 1;
    foreach (CraftComponent.CraftQueueItem craft in this.craft_queue)
    {
      if (craft.craft == this.current_craft)
        return craft.n + 1;
    }
    return 1;
  }

  public bool IsCraftCounterInfinite()
  {
    foreach (CraftComponent.CraftQueueItem craft in this.craft_queue)
    {
      if (craft.craft == this.current_craft && craft.infinite)
        return true;
    }
    return false;
  }

  public void SetWorkerPausedMode(bool paused)
  {
    if (this._worker_is_paused == paused)
      return;
    this._worker_is_paused = paused;
    this.components.RefreshBubblesData(new bool?());
  }

  public void DistributeDropsFromSoulsCraft(List<Item> drop_list)
  {
    List<Item> items = new List<Item>();
    for (int index = 0; index < drop_list.Count; ++index)
    {
      if (drop_list[index].is_tech_point)
      {
        items.Add(drop_list[index]);
        drop_list.RemoveAt(index);
        --index;
      }
    }
    List<Item> cant_insert;
    this.wgo.PutToAllPossibleInventories(drop_list, out cant_insert);
    this.wgo.DropItems(cant_insert);
    this.wgo.DropItems(items);
  }

  [CompilerGenerated]
  public float \u003CRefreshComponentBubbleData\u003Eb__84_0() => this.wgo.progress;

  [CompilerGenerated]
  public float \u003CRefreshComponentBubbleData\u003Eb__84_1() => this.wgo.progress;

  [Serializable]
  public class CraftQueueItem
  {
    public string id = "";
    public int n;
    public bool infinite;
    public bool is_gratitude_points_craft;
    public CraftDefinition _craft;

    public CraftDefinition craft
    {
      get
      {
        if (this._craft == null)
          this._craft = GameBalance.me.GetData<CraftDefinition>(this.id);
        return this._craft;
      }
    }
  }
}
