namespace CultOfQoL.Patches.Structures;

[HarmonyPatch]
public static class ShrineFuelPatches
{
    /// <summary>
    /// Adds MAGMA_STONE (Rotburn) to the shrine's fuel list when enabled.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildingShrine), nameof(BuildingShrine.Start))]
    private static void BuildingShrine_Start(BuildingShrine __instance)
    {
        if (!Plugin.EnableRotburnAsShrineFuel.Value)
        {
            return;
        }

        if (__instance.addFuel == null)
        {
            return;
        }

        if (!__instance.addFuel.fuel.Contains(InventoryItem.ITEM_TYPE.MAGMA_STONE))
        {
            __instance.addFuel.fuel.Add(InventoryItem.ITEM_TYPE.MAGMA_STONE);
        }
    }

    /// <summary>
    /// Stores the current fuel level before adding MAGMA_STONE so we can recalculate with custom weight.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.AddFuel))]
    private static void Interaction_AddFuel_AddFuel_Prefix(
        Interaction_AddFuel __instance,
        InventoryItem.ITEM_TYPE itemType,
        ref int __state)
    {
        __state = -1;

        if (!Plugin.EnableRotburnAsShrineFuel.Value)
        {
            return;
        }

        if (itemType != InventoryItem.ITEM_TYPE.MAGMA_STONE)
        {
            return;
        }

        if (__instance.structure?.Type is not (StructureBrain.TYPES.SHRINE or
            StructureBrain.TYPES.SHRINE_II or StructureBrain.TYPES.SHRINE_III or
            StructureBrain.TYPES.SHRINE_IV))
        {
            return;
        }

        // Store original fuel to calculate our custom addition
        __state = __instance.structure.Structure_Info.Fuel;
    }

    /// <summary>
    /// Recalculates fuel with custom weight for MAGMA_STONE in shrines.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.AddFuel))]
    private static void Interaction_AddFuel_AddFuel_Postfix(
        Interaction_AddFuel __instance,
        InventoryItem.ITEM_TYPE itemType,
        int __state)
    {
        if (__state < 0)
        {
            return;
        }

        if (!Plugin.EnableRotburnAsShrineFuel.Value)
        {
            return;
        }

        if (itemType != InventoryItem.ITEM_TYPE.MAGMA_STONE)
        {
            return;
        }

        if (__instance.structure?.Type is not (StructureBrain.TYPES.SHRINE or
            StructureBrain.TYPES.SHRINE_II or StructureBrain.TYPES.SHRINE_III or
            StructureBrain.TYPES.SHRINE_IV))
        {
            return;
        }

        // Recalculate with custom fuel weight
        var customWeight = Plugin.RotburnShrineFuelWeight.Value;
        __instance.structure.Structure_Info.Fuel = Mathf.Clamp(
            __state + customWeight, 0, __instance.structure.Structure_Info.MaxFuel);
    }

    /// <summary>
    /// Adds warmth contribution from fueled shrine to the warmth bar.
    /// The game already defines shrine warmth (20) in StructureManager.GetBuildingWarmth()
    /// but WarmthBar.WarmthNormalized only reads from DLC Furnace.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WarmthBar), nameof(WarmthBar.WarmthNormalized), MethodType.Getter)]
    private static void WarmthBar_WarmthNormalized_Postfix(ref float __result)
    {
        if (!Plugin.EnableShrineWarmth.Value)
        {
            return;
        }

        if (__result >= 1f)
        {
            return; // Already at max warmth
        }

        // Check if any shrine is fueled and add warmth contribution
        foreach (var shrine in BuildingShrine.Shrines)
        {
            if (shrine?.Structure?.Structure_Info?.FullyFueled == true)
            {
                // Shrine provides 20 warmth out of 100 max = 0.2 contribution
                __result = Mathf.Clamp01(__result + 0.2f);
                break; // One fueled shrine is enough
            }
        }
    }
}
