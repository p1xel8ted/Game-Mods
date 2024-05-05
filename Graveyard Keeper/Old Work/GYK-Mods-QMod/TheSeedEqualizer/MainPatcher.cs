using HarmonyLib;
using System;
using System.Reflection;

namespace TheSeedEqualizer;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static Config.Options _cfg;
    private static bool _alreadyRun;

    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.TheSeedEqualizer");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            _cfg = Config.GetOptions();
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}