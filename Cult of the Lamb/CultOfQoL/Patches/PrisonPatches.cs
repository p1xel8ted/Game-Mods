namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class PrisonPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIFollowerSelectMenuController), nameof(UIFollowerSelectMenuController.Show), typeof(List<FollowerSelectEntry>), typeof(bool), typeof(UpgradeSystem.Type), typeof(bool), typeof(bool), typeof(bool), typeof(bool))]
    public static void UIFollowerSelectMenuController_Show(ref UIFollowerSelectMenuController __instance, ref List<FollowerSelectEntry> followerSelectEntries)
    {
        if (__instance is not UIPrisonMenuController) return;
        if (!Plugin.OnlyShowDissenters.Value) return;
        followerSelectEntries.RemoveAll(follower => follower.FollowerInfo.CursedState != Thought.Dissenter);
    }

}