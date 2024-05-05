using System.Linq;
using HarmonyLib;
using Helper;
using TheSeedEqualizer.lang;
using UnityEngine;

namespace TheSeedEqualizer;

[HarmonyPatch]
public static partial class MainPatcher
{
    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(ResModificator), nameof(ResModificator.ProcessItemsListBeforeDrop))]
    // public static void ResModificator_ProcessItemsListBeforeDrop(ref WorldGameObject wgo, ref List<Item> __result)
    // {
    //     var message = string.Empty;
    //     if (wgo != null && (Tools.PlayerGardenCraft(wgo) || Tools.ZombieGardenCraft(wgo) || Tools.ZombieVineyardCraft(wgo)) &&
    //         wgo.components.craft.current_craft != null)
    //     {
    //         message += $"[ResModWgo]: WGO: {wgo.obj_id}\n";
    //         message += $"[ResModWgo]: CRAFT: {wgo.components.craft.current_craft.id}\n";
    //         message = __result.Aggregate(message, (current, item) => current + $"[ResModResult]: Item: {item.id}, Value: {item.value}\n");
    //         Log($"{message}");
    //     }
    // }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameBalance), nameof(GameBalance.LoadGameBalance))]
    private static void GameBalance_LoadGameBalance()
    {
        if (_alreadyRun) return;
        _alreadyRun = true;

        foreach (var craft in GameBalance.me.objs_data.Where(a => a.drop_items.Count > 0 && a.drop_items.Exists(b => b.id.Contains("seed"))))
        {
            if (_cfg.ModifyPlayerGardens && craft.id.StartsWith("garden") && craft.id.EndsWith("ready"))
            {
                Log($"Modifying Player Garden Seed Output in ObjData: {craft.id}");
                ModifyOutput(craft);
            }
        }

        foreach (var craft in GameBalance.me.craft_data.Where(a => a.needs.Count > 0 && a.needs.Exists(b => b.id.Contains("seed"))))
        {
            // if (Tools.PlayerGardenCraft(craft.id) && _cfg.modifyPlayerGardens)
            // {
            //     Log($"Modifying Player Garden Seed Output in CraftData: {craft.id}");
            //     ModifyOutput(craft);
            // }

            if (Tools.ZombieGardenCraft(craft.id) && _cfg.ModifyZombieGardens)
            {
                Log($"Modifying Zombie Garden Seed Output: {craft.id}");
                ModifyOutput(craft);
            }

            if (Tools.ZombieVineyardCraft(craft.id) && _cfg.ModifyZombieVineyards)
            {
                Log($"Modifying Zombie Vineyard Seed Output: {craft.id}");
                ModifyOutput(craft);
            }

            if (Tools.RefugeeGardenCraft(craft.id) && _cfg.ModifyRefugeeGardens)
            {
                Log($"Modifying Refugee Seed Output: {craft.id}");
                ModifyOutput(craft);
            }

            if (Tools.ZombieVineyardCraft(craft.id) && _cfg.AddWasteToZombieVineyards && !craft.output.Exists(a => a.id == "crop_waste"))
            {
                Log($"Adding Crop Waste To Zombie Vineyard Output: {craft.id}");
                var item = new Item("crop_waste", 3)
                {
                    min_value = SmartExpression.ParseExpression("3"),
                    max_value = SmartExpression.ParseExpression("5"),
                    self_chance = craft.needs[0].self_chance
                };
                craft.output.Add(item);
            }

            if (Tools.ZombieGardenCraft(craft.id) && _cfg.AddWasteToZombieGardens && !craft.output.Exists(a => a.id == "crop_waste"))
            {
                Log($"Adding Crop Waste To Zombie Garden Output: {craft.id}");
                var item = new Item("crop_waste", 3)
                {
                    min_value = SmartExpression.ParseExpression("3"),
                    max_value = SmartExpression.ParseExpression("5"),
                    self_chance = craft.needs[0].self_chance
                };
                craft.output.Add(item);
            }
        }
    }

    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.ReloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();
            _alreadyRun = false;
            GameBalance_LoadGameBalance();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.FasterCraftReloaded")]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static void CraftComponent_ReallyUpdateComponent(CraftComponent __instance, ref float delta_time)
    {
        if (__instance?.current_craft == null) return;
        if (!_cfg.BoostGrowSpeedWhenRaining) return;

       // AccessTools.Field(typeof(EnvironmentEngine), "_is_rainy").SetValue(EnvironmentEngine.me, true);
        if (!EnvironmentEngine.me.is_rainy) return;
        string[] refugee = {"garden", "planting", "refugee", "grow"};
        if (refugee.All(a => __instance.current_craft.id.Contains(a)) || __instance.current_craft.id.Contains("vineyard") || (__instance.current_craft.id.StartsWith("garden") && __instance.current_craft.id.EndsWith("growing")))
        {
            Log($"It's raining! Boosting base grow speed of {__instance.current_craft.id} by 100%!");

            delta_time *= 2f;
        }
    }
}