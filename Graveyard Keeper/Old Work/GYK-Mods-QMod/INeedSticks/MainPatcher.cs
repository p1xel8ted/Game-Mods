using HarmonyLib;
using System;
using System.Reflection;
using Tools = Helper.Tools;

namespace INeedSticks;

[HarmonyPatch]
[HarmonyBefore("p1xel8ted.GraveyardKeeper.QueueEverything")]
public static partial class MainPatcher
{
    private static CraftDefinition _newItem;
    private const string WoodenStick = "wooden_stick";


    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.INeedSticks");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private static void Log(string message, bool error = false)
    {
        Tools.Log("INeedSticks", $"{message}", error);
    }
}