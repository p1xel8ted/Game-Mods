namespace AppleTreesEnhanced;

public partial class Plugin
{
    internal static ConfigEntry<bool> IncludeGardenBerryBushes { get; private set; }
    internal static ConfigEntry<bool> IncludeGardenTrees { get; private set; }
    internal static ConfigEntry<bool> IncludeWorldBerryBushes { get; private set; }
    internal static ConfigEntry<bool> ShowHarvestReadyMessages { get; private set; }
    internal static ConfigEntry<bool> RealisticHarvest { get; private set; }
    internal static ConfigEntry<bool> IncludeGardenBeeHives { get; private set; }
    internal static ConfigEntry<bool> BeeKeeperBuyback { get; private set; }
}