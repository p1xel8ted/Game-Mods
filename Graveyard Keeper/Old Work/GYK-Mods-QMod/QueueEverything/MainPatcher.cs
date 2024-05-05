using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

// ReSharper disable InconsistentNaming

namespace QueueEverything;

[HarmonyPatch]
[HarmonyAfter("p1xel8ted.GraveyardKeeper.exhaust-less", "com.glibfire.graveyardkeeper.fastercraft.mod", "com.graveyardkeeper.urbanvibes.maxbutton", "p1xel8ted.GraveyardKeeper.INeedSticks")]
public static partial class MainPatcher
{
    private static readonly List<WorldGameObject> currentlyCrafting = new();

    //"axe", "hammer", "shovel", "sword",
    private static readonly string[] MultiOutCantQueue =
    {
        "chisel_2_2b", "marble_plate_3"
    };

    //individual craft definitions
    private static readonly string[] UnSafeCraftDefPartials =
    {
        "1_to_2", "2_to_3", "3_to_4", "4_to_5", "rem_grave", "soul_workbench_craft", "burgers_place", "beer_barrels_place", "remove", "refugee", "upgrade", "fountain", "blockage", "obstacle", "builddesk", "fix", "broken", "elevator", "refugee", "Remove" //, "mf_barrel"
    };

    //unsafe crafting objects as a whole
    private static readonly string[] UnSafeCraftObjects =
    {
        "mf_crematorium_corp", "garden_builddesk", "tree_garden_builddesk", "mf_crematorium", "grave_ground",
        "tile_church_semicircle_2floors", "mf_grindstone_1", "zombie_garden_desk_1", "zombie_garden_desk_2", "zombie_garden_desk_3",
        "zombie_vineyard_desk_1", "zombie_vineyard_desk_2", "zombie_vineyard_desk_3", "graveyard_builddesk", "blockage_H_low", "blockage_V_low",
        "blockage_H_high", "blockage_V_high", "wood_obstacle_v", "refugee_camp_garden_bed", "refugee_camp_garden_bed_1", "refugee_camp_garden_bed_2",
        "refugee_camp_garden_bed_3", "carrot_box", "elevator_top", "zombie_crafting_table", "mf_balsamation", "mf_balsamation_1", "mf_balsamation_2",
        "mf_balsamation_3", "blockage_H_high", "soul_workbench_craft", "grow_desk_planting", "grow_vineyard_planting"
    };

    private static bool _alreadyRun;
    private static bool _ccAlreadyRun;
    private static Config.Options _cfg;
    private static bool _craftsStarted;
    private static bool _exhaustlessEnabled;
    private static FcConfig.Options _fasterCfg;
    private static bool _fasterCraftEnabled;
    private static bool _fasterCraftReloaded;
    private static bool _originalFasterCraft;

    private static float _timeAdjustment;

    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.QueueEverything");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _cfg = Config.GetOptions();
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private static bool IsUnsafeDefinition(CraftDefinition _craftDefinition)
    {
        var zombieCraft = _craftDefinition.craft_in.Any(craftIn => craftIn.Contains("zombie"));
        var refugeeCraft = _craftDefinition.craft_in.Any(craftIn => craftIn.Contains("refugee"));
        var unsafeOne = UnSafeCraftDefPartials.Any(_craftDefinition.id.Contains);
        var unsafeTwo = !_craftDefinition.icon.Contains("fire") && _craftDefinition.craft_in.Any(craftIn => UnSafeCraftObjects.Contains(craftIn));
        var unsafeThree = MultiOutCantQueue.Any(_craftDefinition.id.Contains);

        if (zombieCraft || refugeeCraft || unsafeOne || unsafeTwo || unsafeThree)
        {
            return true;
        }

        return false;
    }

    private static void LoadFasterCraftConfig()
    {
        try
        {
            const string path = "./QMods/FasterCraft/config.txt";
            var streamReader = new StreamReader(path);
            var text = streamReader.ReadLine();
            var array = text?.Split('=');
            _timeAdjustment = (float) Convert.ToDouble(array?[1], CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            Log("Issue reading FasterCraft Config, disabling integration.");
            _fasterCraftEnabled = false;
        }
    }

    private static void ReloadConfig()
    {
        _cfg = Config.GetOptions();
        _fasterCfg = FcConfig.GetOptions();
    }
}