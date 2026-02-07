namespace MysticAssistantRedux;

/// <summary>
/// Defines Apple Arcade exclusive content that is not available on PC without this mod.
/// </summary>
internal static class ExclusiveContent
{
    /// <summary>
    /// Apple Arcade exclusive follower skins mapped to their real animal names.
    /// The wiki labels these Apple_1 through Apple_5, but the actual Spine skins
    /// and WorshipperData entries use the animal names directly.
    /// All 5 have PC assets (atlas textures, .skel skins, WorshipperData entries).
    /// </summary>
    public static readonly (string SkinName, bool HasPcAssets)[] AppleSkins =
    [
        ("Orangutan", true),
        ("Robin", true),
        ("Trout", true),
        ("Beaver", true),
        ("Lobster", true)
    ];

    /// <summary>
    /// Apple Arcade exclusive clothing/outfits.
    /// </summary>
    public static readonly FollowerClothingType[] AppleClothing =
    [
        FollowerClothingType.Apple_1,
        FollowerClothingType.Apple_2
    ];

    /// <summary>
    /// Apple Arcade exclusive decorations.
    /// Prefabs and addressable bundles exist on PC, but the TypeAndPlacementObjects scene
    /// entries have Addr_PlacementObject GUIDs that only resolve on Apple Arcade builds.
    /// MysticShopPatches.PlaceBuilding_Prefix creates runtime PlacementObject ghosts
    /// using the real decoration prefab paths to bypass the broken GUIDs.
    /// </summary>
    public static readonly StructureBrain.TYPES[] AppleDecorations =
    [
        StructureBrain.TYPES.DECORATION_APPLE_BUSH,
        StructureBrain.TYPES.DECORATION_APPLE_LANTERN,
        StructureBrain.TYPES.DECORATION_APPLE_STATUE,
        StructureBrain.TYPES.DECORATION_APPLE_VASE,
        StructureBrain.TYPES.DECORATION_APPLE_WELL
    ];

    /// <summary>
    /// Prefab paths for Apple decorations (from StructuresData.GetInfoByType).
    /// Used by PlaceBuilding_Prefix to create runtime placement ghosts.
    /// PlacementObject.Start() auto-prepends "Assets/" and appends ".prefab".
    /// </summary>
    public static readonly Dictionary<StructureBrain.TYPES, string> AppleDecorationPrefabPaths = new()
    {
        { StructureBrain.TYPES.DECORATION_APPLE_BUSH, "Prefabs/Structures/Buildings/Decoration Apple Bush" },
        { StructureBrain.TYPES.DECORATION_APPLE_LANTERN, "Prefabs/Structures/Buildings/Decoration Apple Lantern" },
        { StructureBrain.TYPES.DECORATION_APPLE_STATUE, "Prefabs/Structures/Buildings/Decoration Apple Statue" },
        { StructureBrain.TYPES.DECORATION_APPLE_VASE, "Prefabs/Structures/Buildings/Decoration Apple Vase" },
        { StructureBrain.TYPES.DECORATION_APPLE_WELL, "Prefabs/Structures/Buildings/Decoration Apple Well" },
    };

    /// <summary>
    /// Apple Arcade exclusive fleece ID (Fleece of the Verdant).
    /// Note: This fleece exists in Apple Arcade but may not have a PC enum entry.
    /// </summary>
    public const int AppleFleece = 680;

    // Palworld crossover follower skins — commented out pending atlas textures.
    // WorshipperData entries exist but head sprites are missing, causing eyeless rendering.
    // public static readonly string[] PalworldSkins =
    // [
    //     "PalworldOne",
    //     "PalworldTwo",
    //     "PalworldThree",
    //     "PalworldFour",
    //     "PalworldFive"
    // ];

    /// <summary>
    /// Boss follower skins. The game explicitly excludes these from normal skin pools
    /// and SetFollowerSkinUnlocked skips StripNumbers for names containing "Boss".
    /// </summary>
    public static readonly string[] BossSkins =
    [
        "Boss Mama Worm",
        "Boss Mama Maggot",
        "Boss Flying Burp Frog",
        "Boss Egg Hopper",
        "Boss Burrow Worm",
        "Boss Mortar Hopper",
        "Boss Scuttle Turret",
        "Boss Spiker",
        "Boss Charger",
        "Boss Beholder 1",
        "Boss Beholder 2",
        "Boss Beholder 3",
        "Boss Beholder 4",
        "Boss Beholder 5",
        "Boss Beholder 6",
        "Boss Spider Jump",
        "Boss Millipede Poisoner",
        "Boss Scorpion",
        "Boss Death Cat",
        "Boss Aym",
        "Boss Baal",
        "Boss Dog 1",
        "Boss Dog 2",
        "Boss Dog 3",
        "Boss Dog 4",
        "Boss Dog 5",
        "Boss Dog 6",
        "Boss Rot 1",
        "Boss Rot 2",
        "Boss Rot 3",
        "Boss Rot 4"
    ];
}
