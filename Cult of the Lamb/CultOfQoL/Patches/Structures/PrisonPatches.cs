namespace CultOfQoL.Patches.Structures;

[Harmony]
public static class PrisonPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIFollowerSelectMenuController), nameof(UIFollowerSelectMenuController.Show), 
        typeof(List<FollowerSelectEntry>), typeof(bool), typeof(UpgradeSystem.Type), 
        typeof(bool), typeof(bool), typeof(bool), typeof(bool))]
    public static void UIFollowerSelectMenuController_Show(
        UIFollowerSelectMenuController __instance, 
        ref List<FollowerSelectEntry> followerSelectEntries)
    {
        if (!Plugin.OnlyShowDissenters.Value) return;
        if (__instance is not UIPrisonMenuController) return;
        
        // Filter to only show dissenters in prison menu
        followerSelectEntries.RemoveAll(follower => 
            follower.FollowerInfo.CursedState != Thought.Dissenter);
    }
}