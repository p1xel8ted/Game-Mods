namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class PickUps
{
    private static readonly Dictionary<int, (float OriginalRange, bool OriginalMagnet)> OriginalSettings = [];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PickUp), nameof(PickUp.OnEnable))]
    public static void PickUp_OnEnable(PickUp __instance)
    {
        UpdatePickUp(__instance);
    }

    internal static void UpdateAllPickUps()
    {
        var pickupCount = GetAllPickUps().Count();
        Plugin.L($"Updating {pickupCount} pick ups");
        foreach (var pickUp in GetAllPickUps())
        {
            UpdatePickUp(pickUp);
        }
    }

    private static IEnumerable<PickUp> GetAllPickUps()
    {
        //return PickUp.PickUps;
        return Resources.FindObjectsOfTypeAll<PickUp>();
    }

    private static void UpdatePickUp(PickUp p)
    {
        if (!p.CanBePickedUp) return;

        var id = p.GetInstanceID();
        if (!OriginalSettings.ContainsKey(id))
        {
            OriginalSettings[id] = (p.MagnetDistance, p.MagnetToPlayer);
        }

        var (originalRange, _) = OriginalSettings[id];


        if (!Mathf.Approximately(Plugin.MagnetRangeMultiplier.Value, 1.0f))
        {
            if (originalRange == 0)
            {
                originalRange = 7;
            }  
            p.MagnetDistance = originalRange * Plugin.MagnetRangeMultiplier.Value;
        }

        if (Plugin.AllLootMagnets.Value)
        {
            p.MagnetToPlayer = true;
        }
    }
    
    public static void RestoreMagnets()
    {
        foreach (var p in GetAllPickUps())
        {
            if (!p.CanBePickedUp) continue;

            var id = p.GetInstanceID();
            if (OriginalSettings.TryGetValue(id, out var settings))
            {
                Plugin.L($"Restoring magnet to player to {settings.OriginalMagnet} for {p.name}");
                p.MagnetToPlayer = settings.OriginalMagnet;
            }
            else
            {
                Plugin.L($"Could not find original magnet settings for {p.name}");
            }
        }
    }
}