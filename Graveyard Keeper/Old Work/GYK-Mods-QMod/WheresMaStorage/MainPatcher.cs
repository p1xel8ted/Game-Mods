using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace WheresMaStorage;


[HarmonyPatch]
[HarmonyAfter("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs")]
public static partial class MainPatcher
{
    private const string Chest = "chest";
    private const string Gerry = "gerry";
    private const float LogGap = 3f;
    private const string Multi = "multi";
    private const string NpcBarman = "npc_tavern_barman";
    private const string Player = "player";

    private const string Storage = "storage";
    private const string Tavern = "tavern";
    private const string Vendor = "vendor";
    private const string Writer = "writer";
    private const string Soul = "soul_container";
    private const string Bag = "bag";

    private const string ShippingBoxTag = "shipping_box";

    private static readonly string[] AlwaysHidePartials =
    {
       "refugee_camp_well", "refugee_camp_tent", "pump", "pallet", "refugee_camp_well_2"
    };

    private static readonly string[] ChiselItems =
    {
        "chisel"
    };

    private static readonly ItemDefinition.ItemType[] GraveItems =
    {
        ItemDefinition.ItemType.GraveStone, ItemDefinition.ItemType.GraveFence, ItemDefinition.ItemType.GraveCover,
        ItemDefinition.ItemType.GraveStoneReq, ItemDefinition.ItemType.GraveFenceReq, ItemDefinition.ItemType.GraveCoverReq
    };

    private static readonly string[] PenPaperInkItems =
    {
        "book", "chapter", "ink", "pen"
    };


    private static readonly string[] StockpileWidgetsPartials =
    {
        "mf_stones", "mf_ore", "mf_timber"
    };

    private static readonly ItemDefinition.ItemType[] ToolItems =
    {
        ItemDefinition.ItemType.Axe, ItemDefinition.ItemType.Shovel, ItemDefinition.ItemType.Hammer,
        ItemDefinition.ItemType.Pickaxe, ItemDefinition.ItemType.FishingRod, ItemDefinition.ItemType.BodyArmor,
        ItemDefinition.ItemType.HeadArmor, ItemDefinition.ItemType.Sword, ItemDefinition.ItemType.Preach
    };

    private static Config.Options _cfg;
    private static bool _gameBalanceAlreadyRun;
    private static bool _gratitudeCraft;
    private static int _invSize;

    internal static MultiInventory _mi = new();
    private static MultiInventory _refugeeMi = new();
    private static float _timeSix,_timeSeven, _timeEight, _timeNine;
    private static bool _usingBag;
    private static bool _zombieWorker;
    
    private static readonly string[] ExcludeTheseWildernessInventories =
    {
        "vendor", "npc", "donkey", "zombie", "worker", "refugee", "pile", "carrot", "cooking", "guard", "working", "obj_church"
    };

    private static readonly Dictionary<WorldGameObject, MultiInventory> WildernessMultiInventories = new();
    private static readonly List<Inventory> WildernessInventories = new();
    
    private static bool _invsLoaded;

    public static void Patch()
    {
        try
        {
            _cfg = Config.GetOptions();
            _gameBalanceAlreadyRun = false;

            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.WheresMaStorage");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _cfg = Config.GetOptions();
            _invSize = 20 + _cfg.AdditionalInventorySpace;
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}