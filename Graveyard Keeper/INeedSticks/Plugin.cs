namespace INeedSticks;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.5")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.ineedsticks";
    private const string PluginName = "I Neeeed Sticks!";
    private const string PluginVer = "1.6.4";
    private static CraftDefinition _newItem;
    private const string WoodenStick = "wooden_stick";
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        Actions.GameBalanceLoad += GameBalance_LoadGameBalance;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
        
    }
    
    private static void GameBalance_LoadGameBalance()
    {
        if (GameBalance.me == null) return;
        if (GameBalance.me.craft_data.Exists(a => a == _newItem)) return;

        var newCd = new CraftDefinition();
        var cd = GameBalance.me.GetData<CraftDefinition>("wood1_2");
        var output = new List<Item>
        {
            new("stick", 1),
            new("r", 1)
        };
        newCd.craft_in = cd.craft_in;
        newCd.needs = cd.needs;
        newCd.needs_from_wgo = cd.needs_from_wgo;
        newCd.output = output;
        newCd.output[0].definition.base_price = 1;
        newCd.output[0].min_value = SmartExpression.ParseExpression("12");
        newCd.output[0].max_value = SmartExpression.ParseExpression("12");
        newCd.output[0].self_chance = SmartExpression.ParseExpression("1");
        newCd.output[1].min_value = SmartExpression.ParseExpression("5");
        newCd.output[1].max_value = SmartExpression.ParseExpression("5");
        newCd.output[1].self_chance = SmartExpression.ParseExpression("1");
        newCd.out_items_expressions = cd.out_items_expressions;
        newCd.output_res_wgo = cd.output_res_wgo;
        newCd.output_set_res_wgo = cd.output_set_res_wgo;
        newCd.set_when_cancelled = cd.set_when_cancelled;
        newCd.output_to_wgo = cd.output_to_wgo;
        newCd.output_to_wgo_on_start = cd.output_to_wgo_on_start;
        newCd.tool_actions = cd.tool_actions;
        newCd.condition = cd.condition;
        newCd.end_script = cd.end_script;
        newCd.end_event = cd.end_event;
        newCd.flag = cd.flag;
        newCd.craft_time = SmartExpression.ParseExpression("10");
        newCd.energy = SmartExpression.ParseExpression("15");
        newCd.gratitude_points_craft_cost = SmartExpression.ParseExpression("8");
        newCd.sanity = SmartExpression.ParseExpression((cd.sanity.EvaluateFloat() + 2).ToString(CultureInfo.InvariantCulture));
        newCd.hidden = false;
        newCd.needs_unlock = false;
        newCd.icon = "i_stick";
        newCd.craft_type = cd.craft_type;
        newCd.is_auto = cd.is_auto;
        newCd.not_hide_gui = cd.not_hide_gui;
        newCd.can_craft_always = cd.can_craft_always;
        newCd.game_res_to_mirror_name = cd.game_res_to_mirror_name;
        newCd.game_res_to_mirror_max = cd.game_res_to_mirror_max;
        newCd.change_wgo = cd.change_wgo;
        newCd.use_variations = cd.use_variations;
        newCd.variation_index = cd.variation_index;
        newCd.craft_after_finish = cd.craft_after_finish;
        newCd.one_time_craft = cd.one_time_craft;
        newCd.force_multi_craft = cd.force_multi_craft;
        newCd.disable_multi_craft = cd.disable_multi_craft;
        newCd.sub_type = cd.sub_type;
        newCd.transfer_needs_to_wgo = cd.transfer_needs_to_wgo;
        newCd.set_out_wgo_params_on_start = cd.set_out_wgo_params_on_start;
        newCd.itempars_add = cd.itempars_add;
        newCd.itempars_set = cd.itempars_set;
        newCd.item_output = cd.item_output;
        newCd.item_needs = cd.item_needs;
        newCd.item_needs_leave = cd.item_needs_leave;
        newCd.dur_needs_item = cd.dur_needs_item;
        newCd.dur_needs_item_index = cd.dur_needs_item_index;
        newCd.difficulty = cd.difficulty;
        newCd.linked_perks = cd.linked_perks;
        newCd.linked_buffs = cd.linked_buffs;
        newCd.custom_name = "Wooden stick";
        newCd.tab_id = cd.tab_id;
        newCd.buff = cd.buff;
        newCd.needs_quality = cd.needs_quality;
        newCd.k_money = cd.k_money;
        newCd.k_faith = cd.k_faith;
        newCd.linked_sub_id = cd.linked_sub_id;
        newCd.dont_close_window_on_craft = cd.dont_close_window_on_craft;
        newCd.dur_parameter = cd.dur_parameter;
        newCd.dont_show_in_hint = cd.dont_show_in_hint;
        newCd.ach_key = cd.ach_key;
        newCd.craft_time_is_zero = cd.craft_time_is_zero;
        newCd.puff_when_replaced = cd.puff_when_replaced;
        newCd.is_item_crating_craft = cd.is_item_crating_craft;
        newCd.store_last_craft_slot = cd.store_last_craft_slot;
        newCd.hide_quality_icon = cd.hide_quality_icon;
        newCd.enqueue_type = CraftDefinition.EnqueueType.CanEnqueue;
        newCd.id = WoodenStick;
        _newItem = newCd;

        GameBalance.me.craft_data.Add(_newItem);
        GameBalance.me.AddDataUniversal(_newItem);
        GameBalance.me.AddData(_newItem);
        Log.LogWarning($"Added {WoodenStick} to game balance.");
    }
}