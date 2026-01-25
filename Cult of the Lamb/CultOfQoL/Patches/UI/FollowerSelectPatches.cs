namespace CultOfQoL.Patches.UI;

[Harmony]
public static class FollowerSelectPatches
{
    /// <summary>
    /// Re-sorts followers so those with active objectives appear at the top of the selection list.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerSelectMenuController), nameof(UIFollowerSelectMenuController.OnShowFinished))]
    public static void UIFollowerSelectMenuController_OnShowFinished_Postfix(UIFollowerSelectMenuController __instance)
    {
        if (!Plugin.PrioritizeRequestedFollowers.Value) return;

        // Build set of follower IDs with objectives
        var followersWithObjectives = new HashSet<int>();
        foreach (var box in __instance._followerInfoBoxes)
        {
            if (__instance.DoesFollowerHaveObjective(box.FollowerInfo))
            {
                followersWithObjectives.Add(box.FollowerInfo.ID);
            }
        }

        if (followersWithObjectives.Count == 0) return;

        // Re-sort: followers with objectives first, then by original order
        var sortedBoxes = __instance._followerInfoBoxes
            .OrderBy(box => followersWithObjectives.Contains(box.FollowerInfo.ID) ? 0 : 1)
            .ThenBy(box => __instance.FollowerSelectEntries.IndexOf(box.FollowerSelectEntry))
            .ToList();

        // Apply the new order using SetSiblingIndex
        for (var i = 0; i < sortedBoxes.Count; i++)
        {
            sortedBoxes[i].transform.SetSiblingIndex(i);
        }

        // Update the internal list to match
        __instance._followerInfoBoxes.Clear();
        __instance._followerInfoBoxes.AddRange(sortedBoxes);
    }
}
