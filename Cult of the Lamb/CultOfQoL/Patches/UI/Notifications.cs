namespace CultOfQoL.Patches.UI;

[HarmonyPatch]
public static class Notifications
{
    private static readonly HashSet<int> StructuresWithNoFuel = [];
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.ShutTrap))]
    public static void Scarecrow_ShutTrap(ref Scarecrow __instance)
    {
        if (!Plugin.NotifyOfScarecrowTraps.Value) return;
        
        var name = __instance.Structure.Structure_Info.GetLocalizedName();
        if (NotificationCentre.Instance.notificationsThisFrame.Contains(name)) return;
        
        // Clean up bird name (remove "(Clone)" suffix if present)
        var birdName = __instance.Bird.name.Replace("(Clone)", "").Trim();
        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams($"{name} has caught a {birdName}!");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Bed), nameof(Structures_Bed.Collapse))]
    public static void Structures_Bed_Collapse(ref Structures_Bed __instance)
    {
        if (!Plugin.NotifyOfBedCollapse.Value) return;
        
        var name = __instance.Data.GetLocalizedName();
        if (NotificationCentre.Instance.notificationsThisFrame.Contains(name)) return;
        
        NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams($"{name} has collapsed!");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.Update))]
    public static void Interaction_AddFuel_Update(ref Interaction_AddFuel __instance)
    {
        if (!Plugin.NotifyOfNoFuel.Value) return;
        
        var structure = __instance.Structure?.Structure_Info;
        if (structure == null) return;
        
        var structureId = structure.ID;
        var hasFuel = structure.Fuel > 0;
        var wasNotified = StructuresWithNoFuel.Contains(structureId);
        
        if (!hasFuel && !wasNotified)
        {
            // Structure just ran out of fuel - notify
            var name = structure.GetLocalizedName();
            if (!NotificationCentre.Instance.notificationsThisFrame.Contains(name))
            {
                NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams($"{name} has no fuel!");
                StructuresWithNoFuel.Add(structureId);
            }
        }
        else if (hasFuel && wasNotified)
        {
            // Structure was refueled - clear notification state
            StructuresWithNoFuel.Remove(structureId);
        }
    }
}