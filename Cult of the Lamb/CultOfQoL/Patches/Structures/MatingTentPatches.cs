namespace CultOfQoL.Patches.Structures;

[Harmony]
public static class MatingTentPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIMatingMenuController), nameof(UIMatingMenuController.Show),
        typeof(Interaction_MatingTent), typeof(List<FollowerSelectEntry>))]
    public static void UIMatingMenuController_Show(UIMatingMenuController __instance,
        List<FollowerSelectEntry> followerSelectEntries)
    {
        if (!Plugin.AutoSelectBestMatingPair.Value) return;

        __instance.followerSelectMenu.OnShownCompleted += () =>
        {
            AutoSelectBestPair(__instance, followerSelectEntries);
        };
    }

    private static void AutoSelectBestPair(UIMatingMenuController controller,
        List<FollowerSelectEntry> entries)
    {
        var available = entries
            .Where(e => e.AvailabilityStatus == FollowerSelectEntry.Status.Available)
            .ToList();

        if (available.Count < 2) return;

        var bestChance = -1f;
        FollowerInfo bestF1 = null;
        FollowerInfo bestF2 = null;

        for (var i = 0; i < available.Count; i++)
        {
            for (var j = i + 1; j < available.Count; j++)
            {
                var f1 = available[i].FollowerInfo;
                var f2 = available[j].FollowerInfo;
                var chance = Interaction_MatingTent.GetChanceToMate(f1.ID, f2.ID);
                if (chance > bestChance)
                {
                    bestChance = chance;
                    bestF1 = f1;
                    bestF2 = f2;
                }
            }
        }

        if (bestF1 == null || bestF2 == null) return;

        controller.OnFollowerSelected(bestF1);
        controller.OnFollowerSelected(bestF2);
    }
}
