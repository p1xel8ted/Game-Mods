using BepInEx.Configuration;

namespace AppleTreesEnhanced;

public partial class Plugin
{
    internal static ConfigEntry<bool> IncludeGardenBerryBushes = null!; 
    internal static ConfigEntry<bool> IncludeGardenTrees = null!; 
    internal static ConfigEntry<bool> IncludeWorldBerryBushes = null!; 
    internal static ConfigEntry<bool> ShowHarvestReadyMessages = null!; 
    internal static ConfigEntry<bool> RealisticHarvest = null!; 
    internal static ConfigEntry<bool> IncludeGardenBeeHives = null!; 
    internal static ConfigEntry<bool> BeeKeeperBuyback = null!;
}