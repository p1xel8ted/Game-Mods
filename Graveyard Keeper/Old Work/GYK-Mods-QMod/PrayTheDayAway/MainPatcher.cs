using HarmonyLib;
using System;
using System.Reflection;

namespace PrayTheDayAway;

public static partial class MainPatcher
{
    
    private static Config.Options _cfg;
    
    public static void Patch()
    {
        try
        {
            _cfg = Config.GetOptions();
            
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.PrayTheDayAway");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }
}