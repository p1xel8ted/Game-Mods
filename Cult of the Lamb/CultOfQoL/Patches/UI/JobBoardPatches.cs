namespace CultOfQoL.Patches.UI;

[Harmony]
public static class JobBoardPatches
{
    /// <summary>
    /// Fixes vanilla crash when opening a job board where all quests have been claimed.
    /// The game tries to access jobItems[0] without checking if the list is empty.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIJobBoardMenuController), nameof(UIJobBoardMenuController.OnShowStarted))]
    public static bool UIJobBoardMenuController_OnShowStarted(UIJobBoardMenuController __instance)
    {
        if (__instance.jobItems.Count > 0)
        {
            return true;
        }

        // No unclaimed jobs - skip the problematic code and just call base
        // The base class OnShowStarted handles general menu setup
        __instance.SortJobItems();
        return false;
    }
}
