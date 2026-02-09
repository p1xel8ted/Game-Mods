// Decompiled with JetBrains decompiler
// Type: CraftDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class CraftDefinition : BalanceBaseObject
{
  public const int DONT_REALLY_CRAFT_OUTPUT = 1;
  public List<string> craft_in = new List<string>();
  public List<Item> needs = new List<Item>();
  public List<Item> needs_from_wgo = new List<Item>();
  public List<Item> output = new List<Item>();
  public List<SmartExpression> out_items_expressions = new List<SmartExpression>();
  public GameRes output_res_wgo = new GameRes();
  public GameRes output_set_res_wgo = new GameRes();
  public GameRes set_when_cancelled = new GameRes();
  public List<Item> output_to_wgo = new List<Item>();
  public List<Item> output_to_wgo_on_start = new List<Item>();
  public ToolActions tool_actions = new ToolActions();
  public SmartExpression condition = new SmartExpression();
  public string end_script = "";
  public string end_event = "";
  public int flag;
  public SmartExpression craft_time;
  public SmartExpression energy;
  public SmartExpression gratitude_points_craft_cost;
  public SmartExpression sanity;
  public bool hidden;
  public bool needs_unlock;
  public string icon = "";
  public CraftDefinition.CraftType craft_type;
  public bool is_auto;
  public bool not_hide_gui;
  public bool can_craft_always;
  public string game_res_to_mirror_name = "";
  public float game_res_to_mirror_max;
  public string change_wgo = "";
  public bool use_variations;
  public int variation_index;
  public string craft_after_finish = "";
  public bool one_time_craft;
  public bool force_multi_craft;
  public bool disable_multi_craft;
  public CraftDefinition.CraftSubType sub_type;
  public bool transfer_needs_to_wgo;
  public bool set_out_wgo_params_on_start;
  public GameRes itempars_add = new GameRes();
  public GameRes itempars_set = new GameRes();
  public List<Item> item_output = new List<Item>();
  public List<Item> item_needs = new List<Item>();
  public bool item_needs_leave;
  public float dur_needs_item;
  public int dur_needs_item_index;
  public float difficulty;
  public List<string> linked_perks = new List<string>();
  public List<string> linked_buffs = new List<string>();
  public string custom_name = "";
  public string tab_id = "";
  public string buff = "";
  public float needs_quality;
  public float k_money;
  public float k_faith;
  public string linked_sub_id = "";
  public bool dont_close_window_on_craft;
  public float dur_parameter;
  public bool dont_show_in_hint;
  public string ach_key = "";
  public bool craft_time_is_zero;
  public bool puff_when_replaced;
  public bool is_item_crating_craft;
  public int store_last_craft_slot;
  public bool hide_quality_icon;
  public CraftDefinition.EnqueueType enqueue_type;
  [NonSerialized]
  public string _cached_name;
  public static string[] alchemy_goos = new string[21]
  {
    "goo_alcohol",
    "goo_ash",
    "goo_blood",
    "goo_brown",
    "goo_d_blue",
    "goo_d_green",
    "goo_d_violet",
    "goo_diamond",
    "goo_gold",
    "goo_graphite",
    "goo_green",
    "goo_oil",
    "goo_red",
    "goo_salt",
    "goo_silver",
    "goo_spice",
    "goo_violet",
    "goo_water",
    "goo_white",
    "goo_yellow",
    "goo_yellow_electro"
  };

  public bool takes_item_durability => !this.dur_needs_item.EqualsTo(0.0f);

  public string GetNameNonLocalized()
  {
    if (!string.IsNullOrEmpty(this.custom_name))
      return this.custom_name;
    if (string.IsNullOrEmpty(this._cached_name))
    {
      if (this.id.Contains(":") && !this.id.StartsWith("mix:mf_alchemy"))
      {
        this._cached_name = this.id.Split(':')[2];
      }
      else
      {
        Item firstRealOutput = this.GetFirstRealOutput();
        this._cached_name = this.id;
        if (firstRealOutput != null)
        {
          ItemDefinition dataOrNull = GameBalance.me.GetDataOrNull<ItemDefinition>(firstRealOutput.id);
          if (dataOrNull != null)
            this._cached_name = dataOrNull.GetItemName(false);
        }
      }
    }
    return this._cached_name;
  }

  public Item GetFirstRealOutput()
  {
    foreach (Item firstRealOutput in this.output)
    {
      if (!TechDefinition.TECH_POINTS.Contains(firstRealOutput.id))
        return firstRealOutput;
    }
    return (Item) null;
  }

  public string GetDescription()
  {
    if (this.output.Count > 0 && (this.is_item_crating_craft || this.GetNameNonLocalized() == this.output[0].id))
      return this.output[0].is_multiquality ? this.output[0].GetMultiqualityItemDescription() : this.output[0].definition.GetItemDescription();
    string lng_id = this.GetNameNonLocalized() + "_d";
    string str = GJL.L(lng_id);
    string description = lng_id == str ? "" : str;
    if (this is ObjectCraftDefinition objectCraftDefinition && objectCraftDefinition.build_type == ObjectCraftDefinition.BuildType.Put && objectCraftDefinition.id != "_remove_")
    {
      ObjectDefinition data1 = GameBalance.me.GetData<ObjectDefinition>(objectCraftDefinition.out_obj);
      if (data1 != null && (data1.quality_type == ObjectDefinition.QualityType.Shown || data1.quality_type == ObjectDefinition.QualityType.Grave && !data1.quality.EvaluateFloat().EqualsTo(0.0f)))
      {
        float num = data1.quality.EvaluateFloat();
        if (description.Length > 0)
          description += ", ";
        if (data1.id.EndsWith("_place"))
        {
          ObjectDefinition data2 = GameBalance.me.GetData<ObjectDefinition>(data1.id.Replace("_place", ""));
          if (data2 != null && (data2.quality_type == ObjectDefinition.QualityType.Shown || data2.quality_type == ObjectDefinition.QualityType.Grave))
            num = data2.quality.EvaluateFloat();
        }
        description = $"{description}(*){num.ToString()}";
      }
    }
    return description;
  }

  public bool IsMultiqualityOutput()
  {
    foreach (Item obj in this.output)
    {
      if (obj.is_multiquality)
        return true;
    }
    return false;
  }

  public List<string> GetNeededPerks() => this.linked_perks;

  public float GetPerkValue(string perk_id)
  {
    return !MainGame.me.save.unlocked_perks.Contains(perk_id) ? 0.0f : GameBalance.me.GetData<PerkDefinition>(perk_id).stars;
  }

  public float GetBuffValue(string buff_id)
  {
    return BuffsLogics.FindBuffByID(buff_id) == null ? 0.0f : GameBalance.me.GetData<BuffDefinition>(buff_id).craft_q;
  }

  public CraftDefinition.MultiqualityCraftResult GetMultiqualityResult(
    List<string> multiquality_ids,
    List<string> unlocked_perks = null)
  {
    if (!this.IsMultiqualityOutput())
      return (CraftDefinition.MultiqualityCraftResult) null;
    if (unlocked_perks == null)
      unlocked_perks = MainGame.me.save.unlocked_perks;
    CraftDefinition.MultiqualityCraftResult multiqualityResult = new CraftDefinition.MultiqualityCraftResult()
    {
      value_items = 0.0f,
      value_perks = 0.0f
    };
    foreach (string neededPerk in this.GetNeededPerks())
      multiqualityResult.value_perks += this.GetPerkValue(neededPerk);
    foreach (string linkedBuff in this.linked_buffs)
      multiqualityResult.value_perks += this.GetBuffValue(linkedBuff);
    multiqualityResult.value_difficulty = this.difficulty;
    int num = 0;
    for (int index = 0; index < this.needs.Count; ++index)
    {
      if (index < multiquality_ids.Count && this.needs[index].is_multiquality || (double) this.needs[index].definition.quality >= 0.5)
      {
        ++num;
        ItemDefinition data = GameBalance.me.GetData<ItemDefinition>(string.IsNullOrEmpty(multiquality_ids[index]) ? this.needs[index].id : multiquality_ids[index]);
        if (data != null)
          multiqualityResult.value_items += data.quality;
      }
    }
    if (num > 0)
      multiqualityResult.value_items /= (float) num;
    multiqualityResult.SetProbabilities(Mathf.Clamp(multiqualityResult.value_result, 0.0f, 1f), Mathf.Clamp(multiqualityResult.value_result, 1f, 2f) - 1f, Mathf.Clamp(multiqualityResult.value_result, 2f, 3f) - 2f);
    return multiqualityResult;
  }

  public string GetSpendTxt(WorldGameObject wgo, int multiplier = 1)
  {
    string s = "";
    int num1 = !GlobalCraftControlGUI.is_global_control_active ? (this.energy == null || !this.energy.has_expression ? 0 : Mathf.RoundToInt(this.energy.EvaluateFloat(wgo))) : (this.gratitude_points_craft_cost == null || !this.gratitude_points_craft_cost.has_expression ? 0 : Mathf.RoundToInt(this.gratitude_points_craft_cost.EvaluateFloat(wgo)));
    if (num1 != 0)
    {
      float a = 1f;
      if (wgo?.obj_def?.tool_actions != null)
      {
        for (int index = 0; index < wgo.obj_def.tool_actions.action_tools.Count; ++index)
        {
          ItemDefinition.ItemType actionTool = wgo.obj_def.tool_actions.action_tools[index];
          if (actionTool != ItemDefinition.ItemType.Hand)
          {
            Item equippedTool = MainGame.me.player.GetEquippedTool(actionTool);
            if (equippedTool?.definition?.tool_energy_k != null && equippedTool.definition.tool_energy_k.has_expression)
            {
              float num2 = equippedTool.definition.tool_energy_k.EvaluateFloat(wgo, MainGame.me.player);
              if ((double) num2 < (double) a)
                a = num2;
            }
          }
        }
      }
      if (!a.EqualsTo(1f, 0.01f))
        num1 = Mathf.RoundToInt((float) num1 * a);
      if (GlobalCraftControlGUI.is_global_control_active)
      {
        double gratitudePoints = (double) MainGame.me.player.gratitude_points;
        SmartExpression gratitudePointsCraftCost = this.gratitude_points_craft_cost;
        double num3 = gratitudePointsCraftCost != null ? (double) gratitudePointsCraftCost.EvaluateFloat(MainGame.me.player) : 0.0;
        s = gratitudePoints >= num3 ? $"{s}[c](gratitude_points)[/c]{num1.ToString()}" : $"{s}(gratitude_points)[c][ff1111]{num1.ToString()}[/c]";
      }
      else
        s = $"{s}[c](en)[/c]{num1.ToString()}";
    }
    if (this.is_auto)
    {
      int num4 = this.craft_time == null || !this.craft_time.has_expression ? 0 : Mathf.RoundToInt(this.craft_time.EvaluateFloat(wgo));
      if (num4 != 0)
      {
        TimeSpan timeSpan = TimeSpan.FromSeconds((double) num4);
        s = s.ConcatWithSeparator($"[c](time)[/c]{timeSpan.Minutes:0}:{timeSpan.Seconds:00}");
      }
    }
    foreach (Item obj in this.needs_from_wgo)
    {
      if (obj.id == "fire")
      {
        string ss = $"[c](fire2)[/c]{obj.value * multiplier:0}";
        if (!wgo.data.IsEnoughItems(obj, multiplier: multiplier))
          ss = $"[ff1111]{ss}[/c]";
        s = s.ConcatWithSeparator(ss);
      }
    }
    if (wgo?.obj_def?.tool_actions != null)
    {
      for (int index = 0; index < wgo.obj_def.tool_actions.action_tools.Count; ++index)
      {
        ItemDefinition.ItemType actionTool = wgo.obj_def.tool_actions.action_tools[index];
        if (actionTool != ItemDefinition.ItemType.Hand)
        {
          string lower = actionTool.ToString().ToLower();
          int num5 = Mathf.FloorToInt(100f * wgo.obj_def.tool_actions.action_k[index]);
          Item equippedTool = MainGame.me.player.GetEquippedTool(actionTool);
          if (equippedTool == null)
          {
            s = $"{s}\n[c][ff1111]({lower}_s)[-][/c]";
          }
          else
          {
            int num6 = Mathf.FloorToInt((float) num5 * equippedTool.definition.efficiency);
            s += $"\n[c]({lower}_s)[/c]\n{num6}%";
          }
        }
      }
    }
    return s;
  }

  public string GetCraftIcon()
  {
    if (!string.IsNullOrEmpty(this.icon))
      return this.icon;
    Item firstRealOutput = this.GetFirstRealOutput();
    return firstRealOutput != null ? firstRealOutput.GetIcon() : "";
  }

  public bool IsBodyPartExtractionCraft() => this.id.StartsWith("ex:");

  public bool IsBodyPartInsertionCraft() => this.id.StartsWith("insert:");

  public bool IsLocked()
  {
    return this.needs_unlock && !MainGame.me.save.unlocked_crafts.Contains(this.id) || MainGame.me.save.locked_crafts.Contains(this.id);
  }

  public bool CanCraftMultiple()
  {
    return !this.disable_multi_craft && (this.force_multi_craft || !this.takes_item_durability && !this.IsMultiqualityOutput() && !this.one_time_craft && !(this.id == "_remove_") && !this.takes_item_durability && (string.IsNullOrEmpty(this.end_script) && string.IsNullOrEmpty(this.change_wgo) && string.IsNullOrEmpty(this.craft_after_finish) && string.IsNullOrEmpty(this.end_event) || this.enqueue_type == CraftDefinition.EnqueueType.CanEnqueue));
  }

  public bool CanEnqueue()
  {
    switch (this.enqueue_type)
    {
      case CraftDefinition.EnqueueType.CanEnqueue:
        return true;
      case CraftDefinition.EnqueueType.NeverEnqueue:
        return false;
      default:
        return string.IsNullOrEmpty(this.change_wgo) && string.IsNullOrEmpty(this.end_script) && string.IsNullOrEmpty(this.end_event) && !this.IsMultiqualityOutput();
    }
  }

  public enum CraftType
  {
    None,
    ResourcesBasedCraft,
    Survey,
    MixedCraft,
    Fixing,
    AlchemyDecompose,
    PrayCraft,
    RatBuff,
    RefugeeCampCraft,
  }

  public enum CraftSubType
  {
    None,
    Alchemy,
    SurveySciencePoints,
  }

  public enum EnqueueType
  {
    Default,
    CanEnqueue,
    NeverEnqueue,
  }

  [Serializable]
  public class MultiqualityCraftResult
  {
    public float value_items;
    public float value_perks;
    public float value_difficulty;
    public float qp_1;
    public float qp_2;
    public float qp_3;

    public float[] quality_probabilities
    {
      get => new float[3]{ this.qp_1, this.qp_2, this.qp_3 };
    }

    public void SetProbabilities(float p1, float p2, float p3)
    {
      this.qp_1 = p1;
      this.qp_2 = p2;
      this.qp_3 = p3;
    }

    public float value_items_and_perks_sum => this.value_items + this.value_perks;

    public float value_result => this.value_items_and_perks_sum - this.value_difficulty;
  }
}
