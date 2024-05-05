using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace NewGameAtBottom;

[HarmonyPatch]
public static class Patches
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(SaveSlotsMenuGUI), nameof(SaveSlotsMenuGUI.RedrawSlots))]
    private static IEnumerable<CodeInstruction> SaveSlotsMenuGUI_RedrawSlots_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        //originally a blank save called "new" gets added the list of saves first when the save UI is generated
        //and then actual save games are added next, so New game is always at the top (dumb right)
        //this swaps it around, so that player games are first, and "New" game is at the bottom
        var codesToKeep = codes.GetRange(61, 14);
        codes.RemoveRange(61, 14);
        codes.InsertRange(91, codesToKeep);
        return codes.AsEnumerable();
    }
}