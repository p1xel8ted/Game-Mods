namespace QueueEverything;

public partial class Plugin
{
    private readonly static List<WorldGameObject> CurrentlyCrafting = [];

    //"axe", "hammer", "shovel", "sword",
    private readonly static string[] MultiOutCantQueue =
    [
        "chisel_2_2b", "marble_plate_3"
    ];

    //individual craft definitions
    private readonly static string[] UnSafeCraftDefPartials =
    [
        "1_to_2", "2_to_3", "3_to_4", "4_to_5", "rem_grave", "soul_workbench_craft", "burgers_place", "beer_barrels_place", "remove", "refugee", "upgrade", "fountain", "blockage", "obstacle", "builddesk", "fix", "broken", "elevator", "refugee", "Remove" //, "mf_barrel"
    ];

    //unsafe crafting objects as a whole
    private readonly static string[] UnSafeCraftObjects =
    [
        "mf_crematorium_corp", "garden_builddesk", "tree_garden_builddesk", "mf_crematorium", "grave_ground",
        "tile_church_semicircle_2floors", "mf_grindstone_1", "zombie_garden_desk_1", "zombie_garden_desk_2", "zombie_garden_desk_3",
        "zombie_vineyard_desk_1", "zombie_vineyard_desk_2", "zombie_vineyard_desk_3", "graveyard_builddesk", "blockage_H_low", "blockage_V_low",
        "blockage_H_high", "blockage_V_high", "wood_obstacle_v", "refugee_camp_garden_bed", "refugee_camp_garden_bed_1", "refugee_camp_garden_bed_2",
        "refugee_camp_garden_bed_3", "carrot_box", "elevator_top", "zombie_crafting_table", "mf_balsamation", "mf_balsamation_1", "mf_balsamation_2",
        "mf_balsamation_3", "blockage_H_high", "soul_workbench_craft", "grow_desk_planting", "grow_vineyard_planting"
    ];

    private static bool AlreadyRun { get; set; }
    private static bool CcAlreadyRun { get; set; }

    private static bool CraftsStarted { get; set; }
    private static bool ExhaustlessEnabled { get; set; }

    private static bool FasterCraftEnabled { get; set; }

    private static bool FasterCraftReloaded { get; set; }
    // private static bool OriginalFasterCraft { get; set; }

    private static float TimeAdjustment { get; set; }
}