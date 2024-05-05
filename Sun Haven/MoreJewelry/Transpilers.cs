namespace MoreJewelry;

/// <summary>
/// Contains Harmony transpilers for modifying game methods at runtime.
/// </summary>
/// <remarks>
/// This class uses Harmony, a library for patching .NET and Mono methods during runtime.
/// The transpilers within are used to inject or alter IL code in methods for custom functionality.
/// </remarks>
[Harmony]
public static class Transpilers
{

    /// <summary>
    /// Harmony transpiler for the <see cref="PlayerInventory.SetUpInventoryData"/> method.
    /// </summary>
    /// <param name="instructions">The original IL instructions of the method being patched.</param>
    /// <param name="originalMethod">The original method information.</param>
    /// <returns>The modified set of IL instructions.</returns>
    /// <remarks>
    /// This transpiler modifies the <see cref="PlayerInventory.SetUpInventoryData"/> method to add extra jewelry slots.
    /// It searches for a specific IL instruction sequence and injects additional instructions to incorporate the custom slots.
    /// What it's essential adding is the following:
    /// List.AddRange(Patches.GearSlots);
    /// List is a local variable within the method, so it's loaded with Ldloc_0.
    /// <para>Logs an informational message if the sequence is found and an error if not.</para>
    /// </remarks>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.SetUpInventoryData))]
    public static IEnumerable<CodeInstruction> PlayerInventory_SetUpInventoryData_Transpiler(IEnumerable<CodeInstruction> instructions,
        MethodBase originalMethod)
    {
        var addRange = AccessTools.Method(typeof(List<Slot>), nameof(List<Slot>.AddRange));
        //find last instance of addRange
        var codes = new List<CodeInstruction>(instructions);
        var foundMatchingSequence = false;
        for (var i = codes.Count - 1; i >= 0; i--)
        {
            if (!codes[i].Calls(addRange)) continue;
            foundMatchingSequence = true;
            codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldloc_0));
            codes.Insert(i + 2, new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(Patches), nameof(Patches.GearSlots))));
            codes.Insert(i + 3, codes[i]);
            break;
        }
        if (foundMatchingSequence)
        {
            Plugin.LOG.LogInfo(
                $"Found the matching opcode sequence in {originalMethod.Name}. Extra jewelry slots will be added.");
        }
        else
        {
            Plugin.LOG.LogError(
                $"Failed to find the matching opcode sequence in {originalMethod.Name}. Extra jewelry slots will NOT be added!");
        }
        return codes;
    }

}