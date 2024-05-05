using HarmonyLib;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NewGameAtBottom;

[HarmonyPatch]
public static class MainPatcher
{
    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.NewGameAtBottom");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private static void Log(string message, bool error = false)
    {
        Tools.Log("NewGameAtBottom", $"{message}", error);
    }


    [HarmonyTranspiler]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.RedrawSlots))]
    private static IEnumerable<CodeInstruction> SaveSlotsMenuGUI_RedrawSlots_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        try
        {
            //originally a blank save called "new" gets added the list of saves first when the save UI is generated
            //and then actual save games are added next, so New game is always at the top (dumb right)
            //this swaps it around, so that player games are first, and "New" game is at the bottom
            var codesToKeep = codes.GetRange(61, 14);
            codes.RemoveRange(61, 14);
            codes.InsertRange(91, codesToKeep);
            return codes.AsEnumerable();
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }

        return codes.AsEnumerable();
    }
}