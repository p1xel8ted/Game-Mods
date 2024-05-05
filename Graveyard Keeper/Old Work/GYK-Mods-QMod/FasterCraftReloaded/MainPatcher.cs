using HarmonyLib;
using System;
using System.Reflection;

namespace FasterCraftReloaded;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static Config.Options _cfg;

    private static readonly string[] Exclude =
    {
        "zombie", "refugee", "bee", "tree", "berry", "bush", "pump", "compost", "peat", "slime", "candelabrum", "incense", "garden", "planting"
    };

    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.FasterCraftReloaded");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _cfg = Config.GetOptions();
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}