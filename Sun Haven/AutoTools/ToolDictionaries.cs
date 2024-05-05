namespace AutoTools;

public static class ToolDictionaries
{
    private static readonly Dictionary<int, string> PickAxeData = new()
    {
        {3100, "Rusty Pickaxe"},
        {3101, "Copper Pickaxe"},
        {3102, "Iron Pickaxe"},
        {3103, "Adamant Pickaxe"},
        {3104, "Mithril Pickaxe"},
        {3105, "Sunite Pickaxe"},
        {3106, "Glorite Pickaxe"},
        {3107, "Nel'Varian Pickaxe"},
        {3108, "Withergate Pickaxe"},
        {3109, "Enhanced Copper Pickaxe"},
        {3110, "Enhanced Iron Pickaxe"},
        {3111, "Enhanced Adamant Pickaxe"},
        {3112, "Enhanced Mithril Pickaxe"},
        {3113, "Enhanced Sunite Pickaxe"},
        {3114, "Enhanced Glorite Pickaxe"},
        {3115, "Enchanted Copper Pickaxe"},
        {3116, "Enchanted Iron Pickaxe"},
        {3117, "Enchanted Adamant Pickaxe"},
        {3118, "Enchanted Mithril Pickaxe"},
        {3119, "Enchanted Glorite Pickaxe"},
        {3120, "Enchanted Sunite Pickaxe"},
        {3121, "Perfect Copper Pickaxe"},
        {3122, "Perfect Iron Pickaxe"},
        {3123, "Perfect Adamant Pickaxe"},
        {3124, "Perfect Mithril Pickaxe"},
        {3125, "Perfect Sunite Pickaxe"},
        {3126, "Perfect Glorite Pickaxe"},
        {30006, "Dev Pickaxe"}
    };

    private static readonly Dictionary<int, string> AxeData = new()
    {
        {3300, "Rusty Axe"},
        {3301, "Copper Axe"},
        {3302, "Iron Axe"},
        {3303, "Adamant Axe"},
        {3304, "Mithril Axe"},
        {3305, "Sunite Axe"},
        {3306, "Glorite Axe"},
        {3307, "Enhanced Copper Axe"},
        {3308, "Enhanced Iron Axe"},
        {3309, "Enhanced Adamant Axe"},
        {3310, "Enhanced Mithril Axe"},
        {3311, "Enhanced Sunite Axe"},
        {3312, "Enhanced Glorite Axe"},
        {3313, "Enchanted Copper Axe"},
        {3314, "Enchanted Iron Axe"},
        {3315, "Enchanted Adamant Axe"},
        {3316, "Enchanted Mithril Axe"},
        {3317, "Enchanted Sunite Axe"},
        {3318, "Enchanted Glorite Axe"},
        {3319, "Perfect Copper Axe"},
        {3320, "Perfect Iron Axe"},
        {3321, "Perfect Adamant Axe"},
        {3322, "Perfect Mithril Axe"},
        {3323, "Perfect Sunite Axe"},
        {3324, "Perfect Glorite Axe"},
        {30005, "Dev Axe"}
    };

    private static readonly Dictionary<int, string> WateringCanData = new()
    {
        {3500, "Rusty Watering Can"},
        {3501, "Copper Watering Can"},
        {3502, "Iron Watering Can"},
        {3503, "Adamant Watering Can"},
        {3504, "Mithril Watering Can"},
        {3505, "Sunite Watering Can"},
        {3506, "Glorite Watering Can"},
        {30004, "Dev Watering Can"},
    };

    private static readonly Dictionary<int, string> ScytheData = new()
    {
        {3000, "Scythe"},
        {30000, "Adamant Scythe"},
        {30020, "Mithril Scythe"},
        {30030, "Sunite Scythe"},
        {30090, "Glorite Scythe"},
        {30007, "Dev Scythe"},
    };

    private static readonly Dictionary<int, string> FishingRodData = new()
    {
        {3400, "Basic Fishing Rod"},
        {3401, "Very Good Fishing Rod"},
        {3402, "Nel'Vari Fishing Rod"},
        {3403, "Branch Fishing Rod"},
        {3404, "Golden Fishing Rod"},
        {3405, "Withergate Fishing Rod"},
        {3406, "Enchanted Fishing Rod"}
    };

    private static readonly Dictionary<int, string> HoeData = new()
    {
        {3200, "Rusty Hoe"},
        {3201, "Copper Hoe"},
        {3202, "Iron Hoe"},
        {3203, "Adamant Hoe"},
        {3204, "Mithril Hoe"},
        {3205, "Sunite Hoe"},
        {3206, "Glorite Hoe"},
        {30003, "DevHoe"},
    };

    public static ReadOnlyDictionary<int, string> Hoes { get; } = new(HoeData);

    public static ReadOnlyDictionary<int, string> PickAxes { get; } = new(PickAxeData);

    public static ReadOnlyDictionary<int, string> Axes { get; } = new(AxeData);

    public static ReadOnlyDictionary<int, string> WateringCans { get; } = new(WateringCanData);

    public static ReadOnlyDictionary<int, string> Scythes { get; } = new(ScytheData);

    public static ReadOnlyDictionary<int, string> FishingRods { get; } = new(FishingRodData);
}