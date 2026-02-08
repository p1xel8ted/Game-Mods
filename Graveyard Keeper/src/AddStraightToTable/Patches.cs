using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace AddStraightToTable;

[HarmonyPatch]
public static class Patches
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(AutopsyGUI), nameof(AutopsyGUI.OnBodyItemPress))]
    private static IEnumerable<CodeInstruction> AutopsyGUI_OnBodyItemPress_Transpiler(
        IEnumerable<CodeInstruction> instructions)
    {
        var openYesNo = AccessTools.Method(typeof(DialogGUI), nameof(DialogGUI.OpenYesNo));
        var directExtract = AccessTools.Method(typeof(Patches), nameof(DirectExtract));
        var found = false;

        foreach (var instruction in instructions)
        {
            if (instruction.Calls(openYesNo))
            {
                yield return new CodeInstruction(OpCodes.Call, directExtract);
                found = true;
            }
            else
            {
                yield return instruction;
            }
        }

        if (!found)
        {
            Plugin.LOG.LogWarning("AddStraightToTable: Failed to find DialogGUI.OpenYesNo call in AutopsyGUI.OnBodyItemPress");
        }
    }

    public static void DirectExtract(
        DialogGUI dialog,
        string text,
        GJCommons.VoidDelegate yes,
        GJCommons.VoidDelegate no,
        GJCommons.VoidDelegate onHide)
    {
        yes?.Invoke();
    }
}
