namespace CultOfQoL.Patches.UI;

[HarmonyPatch]
public static class Notifications
{
    private static readonly List<int> StructureID = [];

    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(NotificationCentre), nameof(NotificationCentre.Show))]
    // public static void NotificationCentre_Show(ref NotificationCentre __instance)
    // {
    //     var scale = Plugin.NotificationsScale.Value / 100f;
    //     __instance._rectTransform.transform.localScale = new Vector3(scale, scale, 1f);
    // }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Scarecrow), nameof(Scarecrow.ShutTrap))]
    public static void Scarecrow_ShutTrap(ref Scarecrow __instance)
    {
        var name = __instance.Structure.Structure_Info.GetLocalizedName();
        if (Plugin.NotifyOfScarecrowTraps.Value && !NotificationCentre.Instance.notificationsThisFrame.Contains(name))
        {
            NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams($"{name} has caught a {__instance.Bird.name}!");
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Bed), nameof(Structures_Bed.Collapse))]
    public static void Structures_Bed_Collapse(ref Structures_Bed __instance)
    {
        var name = __instance.Data.GetLocalizedName();
        if (Plugin.NotifyOfBedCollapse.Value && !NotificationCentre.Instance.notificationsThisFrame.Contains(name))
        {
            NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams($"{name} has collapsed!");
        }
    }
    

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_AddFuel), nameof(Interaction_AddFuel.Update))]
    public static void Interaction_AddFuel_Update(ref Interaction_AddFuel __instance)
    {
        if (!Plugin.NotifyOfNoFuel.Value) return;
        if (__instance.structure != null && __instance.structure.Structure_Info != null)
        {
            var name = __instance.Structure.Structure_Info.GetLocalizedName();
            if (!NotificationCentre.Instance.notificationsThisFrame.Contains(name) && __instance.Structure.Structure_Info.Fuel <= 0 && !StructureID.Contains(__instance.Structure.Structure_Info.ID))
            {
                NotificationCentre.Instance.PlayGenericNotificationNonLocalizedParams($"{name} has no fuel!");
                StructureID.Add(__instance.Structure.Structure_Info.ID);
                //return;
            }

            if (StructureID.Contains(__instance.Structure.Structure_Info.ID) && __instance.Structure.Structure_Info.Fuel > 0)
            {
                StructureID.Remove(__instance.Structure.Structure_Info.ID);
            }
        }
    }
}