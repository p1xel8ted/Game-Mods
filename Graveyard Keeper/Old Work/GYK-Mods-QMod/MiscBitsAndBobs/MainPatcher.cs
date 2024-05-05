using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace MiscBitsAndBobs;

[HarmonyPatch]
public static partial class MainPatcher
{
    private static Config.Options _cfg;
    private static WorldGameObject _wgo;
    private static bool _sprintTools, _sprintHarmony, _sprint;
    private static bool _sprintMsgShown;

    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.MiscBitsAndBobs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            _cfg = Config.GetOptions();
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }


 
}