namespace CultOfQoL.Patches.Gameplay;

[HarmonyPatch]
public static class PickUps
{
    private static readonly Dictionary<int, (float OriginalRange, bool OriginalMagnet)> OriginalSettings = [];
    private const float DefaultMagnetRange = 7f;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PickUp), nameof(PickUp.OnEnable))]
    public static void PickUp_OnEnable(PickUp __instance)
    {
        UpdatePickUp(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PickUp), nameof(PickUp.OnDisable))]
    public static void PickUp_OnDisable(PickUp __instance)
    {
        // Clean up dictionary entry when pickup is disabled/destroyed
        OriginalSettings.Remove(__instance.GetInstanceID());
    }

    internal static void UpdateAllPickUps()
    {
        var pickups = PickUp.PickUps.ToList(); // Use the game's static list
        Plugin.L($"Updating {pickups.Count} pick ups");
        foreach (var pickUp in pickups)
        {
            UpdatePickUp(pickUp);
        }
    }

    internal static void RestoreAllPickUps()
    {
        var pickups = PickUp.PickUps.ToList();
        Plugin.L($"Restoring {pickups.Count} pick ups");
        foreach (var pickUp in pickups)
        {
            RestorePickUp(pickUp);
        }
    }

    private static void UpdatePickUp(PickUp p)
    {
        if (!p || !p.CanBePickedUp) return;

        var id = p.GetInstanceID();
        
        // Store original settings only once
        if (!OriginalSettings.ContainsKey(id))
        {
            OriginalSettings[id] = (p.MagnetDistance, p.MagnetToPlayer);
        }

        var (originalRange, originalMagnet) = OriginalSettings[id];

        // Apply range multiplier
        if (Helpers.IsMultiplierActive(Plugin.MagnetRangeMultiplier.Value))
        {
            var baseRange = originalRange > 0 ? originalRange : DefaultMagnetRange;
            p.MagnetDistance = baseRange * Plugin.MagnetRangeMultiplier.Value;
        }
        else
        {
            p.MagnetDistance = originalRange;
        }

        // Apply magnet to all items
        p.MagnetToPlayer = Plugin.AllLootMagnets.Value || originalMagnet;
    }

    private static void RestorePickUp(PickUp p)
    {
        if (!p || !p.CanBePickedUp) return;

        var id = p.GetInstanceID();
        if (OriginalSettings.TryGetValue(id, out var settings))
        {
            p.MagnetDistance = settings.OriginalRange;
            p.MagnetToPlayer = settings.OriginalMagnet;
        }
    }

    // Call this when exiting to menu or when mod is disabled
    public static void ClearCache()
    {
        OriginalSettings.Clear();
    }
}