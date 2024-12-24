namespace WheresMaStorage;

public static class Fields
{
    internal const string Chest = "chest";
    internal const string Gerry = "gerry";
    internal const float LogGap = 3f;
    internal const string Multi = "multi";
    internal const string NpcBarman = "npc_tavern_barman";
    internal const string Player = "player";

    internal const string Storage = "storage";
    internal const string Tavern = "tavern";
    internal const string Vendor = "vendor";
    internal const string Writer = "writer";
    internal const string Soul = "soul_container";
    internal const string Bag = "bag";

    internal const string ShippingBoxTag = "shipping_box";
    internal static bool DebugMessageShown { get; set; }

    internal static readonly string[] AlwaysHidePartials =
    [
        "refugee_camp_well", "refugee_camp_tent", "pump", "pallet", "refugee_camp_well_2"
    ];

    internal static readonly string[] ChiselItems =
    [
        "chisel"
    ];

    internal static readonly ItemDefinition.ItemType[] GraveItems =
    [
        ItemDefinition.ItemType.GraveStone, ItemDefinition.ItemType.GraveFence, ItemDefinition.ItemType.GraveCover,
        ItemDefinition.ItemType.GraveStoneReq, ItemDefinition.ItemType.GraveFenceReq, ItemDefinition.ItemType.GraveCoverReq
    ];

    internal static readonly string[] PenPaperInkItems =
    [
        "book", "chapter", "ink", "pen"
    ];


    internal static readonly string[] StockpileWidgetsPartials =
    [
        "mf_stones", "mf_ore", "mf_timber"
    ];

    // internal readonly static ItemDefinition.ItemType[] ToolItems =
    // [
    //     ItemDefinition.ItemType.Axe, ItemDefinition.ItemType.Shovel, ItemDefinition.ItemType.Hammer,
    //     ItemDefinition.ItemType.Pickaxe, ItemDefinition.ItemType.FishingRod, ItemDefinition.ItemType.BodyArmor,
    //     ItemDefinition.ItemType.HeadArmor, ItemDefinition.ItemType.Sword
    // ];

    internal static bool GameBalanceAlreadyRun { get; set; }
    internal static bool GratitudeCraft { get; set; }
    internal static int PlayerInventorySize => 20 + Plugin.AdditionalInventorySpace.Value;
    internal static MultiInventory Mi { get; set; } = new();
    internal static MultiInventory RefugeeMi { get; set; } = new();
    internal static float TimeSix { get; set; }
    internal static float TimeEight { get; set; }
    internal static float TimeNine { get; set; }
    internal static bool UsingBag { get; set; }
    internal static bool ZombieWorker { get; set; }
    internal static List<Item> OldDrops { get; set; } = [];

    internal static readonly string[] ExcludeTheseWildernessInventories =
    [
        "vendor", "npc", "donkey", "zombie", "worker", "refugee", "pile", "carrot", "cooking", "guard", "working", "obj_church"
    ];

    internal static readonly Dictionary<WorldGameObject, MultiInventory> WildernessMultiInventories = new();
    internal static readonly List<Inventory> WildernessInventories = [];

    internal static bool InventoriesLoaded { get; set; }
    public static bool DropsCleaned { get; set; }
}