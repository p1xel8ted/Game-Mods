namespace CultOfQoL.Patches.Structures;

[HarmonyPatch]
public static class ShrineFuelPatches
{
    /// <summary>
    /// Tracks which shrines were fueled with Rotburn (MAGMA_STONE).
    /// Key: Shrine structure UID, Value: true if fueled with Rotburn
    /// </summary>
    private static readonly Dictionary<int, bool> RotburnFueledShrines = new();

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
    /// Also tracks which shrines are fueled with Rotburn for warmth calculation.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.AddFuel))]
    private static void Interaction_AddFuel_AddFuel_Postfix(
        Interaction_AddFuel __instance,
        InventoryItem.ITEM_TYPE itemType,
        int __state)
    {
        // Handle any shrine fuel addition to track fuel type
        if (__instance.structure?.Type is (StructureBrain.TYPES.SHRINE or
            StructureBrain.TYPES.SHRINE_II or StructureBrain.TYPES.SHRINE_III or
            StructureBrain.TYPES.SHRINE_IV))
        {
            var shrineId = __instance.structure.Structure_Info.ID;

            // Track if this shrine is using Rotburn
            if (itemType == InventoryItem.ITEM_TYPE.MAGMA_STONE && Plugin.EnableRotburnAsShrineFuel.Value)
            {
                RotburnFueledShrines[shrineId] = true;
            }
            else
            {
                // Regular fuel (logs) was added - mark as NOT Rotburn-fueled
                RotburnFueledShrines[shrineId] = false;
            }
        }

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
        // Only add warmth if both features are enabled (shrine warmth + rotburn fuel)
        if (!Plugin.EnableShrineWarmth.Value || !Plugin.EnableRotburnAsShrineFuel.Value)
        {
            return;
        }

        if (__result >= 1f)
        {
            return; // Already at max warmth
        }

        // Check if any shrine is fully fueled AND was fueled with Rotburn
        foreach (var shrine in BuildingShrine.Shrines)
        {
            if (shrine?.Structure?.Structure_Info?.FullyFueled != true)
            {
                continue;
            }

            var shrineId = shrine.Structure.Structure_Info.ID;

            // Only add warmth if this shrine was fueled with Rotburn
            if (RotburnFueledShrines.TryGetValue(shrineId, out var isRotburnFueled) && isRotburnFueled)
            {
                var originalWarmth = __result;
                // Shrine provides 20 warmth out of 100 max = 0.2 contribution
                __result = Mathf.Clamp01(__result + 0.2f);

                #if DEBUG
                Plugin.Log.LogInfo($"[Shrine Warmth] Added +20% warmth from Rotburn-fueled shrine (before: {originalWarmth:F2}, after: {__result:F2})");
                #endif

                break; // One fueled shrine is enough
            }
        }
    }
}
