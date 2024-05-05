using HarmonyLib;
using System;
using System.Reflection;

namespace RegenerationReloaded;

public static partial class MainPatcher
{
    private static Config.Options _cfg;
    private static float _delay;

    public static void Patch()
    {
        try
        {
            _cfg = Config.GetOptions();

            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.RegenerationReloaded");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _delay = _cfg.regenDelay;
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }


}