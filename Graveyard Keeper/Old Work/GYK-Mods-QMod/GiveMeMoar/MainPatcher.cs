using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;

namespace GiveMeMoar;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static Config.Options _cfg;

    private static readonly List<string> DropList = new()
    {
        "fruit:berry", "fruit:apple_green_crop", "fruit:apple_red_crop", "honey", "beeswax", "ash", "shr_agaric",
        "shr_boletus", "bat_wing", "jelly_slug",
        "jelly_slug_blue", "jelly_slug_orange", "jelly_slug_black", "bee", "slime", "spider_web", "1h_ore_metal",
        "nails_bloody", "nugget_silver", "nugget_gold",
        "graphite", "sand_river", "stick", "stone_plate_1", "sulfur", "clay", "coal", "lifestone", "butterfly",
        "maggot",
        "moth", "flw_chamomile", "flw_dandelion", "flw_poppy", "wheat_seed", "cabbage_seed", "carrot_seed",
        "beet_seed", "onion_seed:1", "onion_seed:2",
        "onion_seed:3", "lentils_seed:1", "lentils_seed:2", "lentils_seed:3", "pumpkin_seed:1", "pumpkin_seed:2",
        "pumpkin_seed:3", "hop_seed:1", "hop_seed:2", "hop_seed:3",
        "hamp_seed:1", "hamp_seed:2", "hamp_seed:3", "grapes_seed:1", "grapes_seed:2", "grapes_seed:3"
    };
    
    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.GiveMeMoar");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _cfg = Config.GetOptions();

        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}