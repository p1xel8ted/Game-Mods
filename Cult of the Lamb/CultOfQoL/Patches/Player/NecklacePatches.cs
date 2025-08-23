namespace CultOfQoL.Patches.Player;

[HarmonyPatch]
public static class NecklacePatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GiveItemRoutine))]
    public static void interaction_FollowerInteraction_GiveItemRoutine(ref interaction_FollowerInteraction __instance, ref InventoryItem.ITEM_TYPE itemToGive)
    {
        if (!Plugin.GiveFollowersNewNecklaces.Value) return;
        if (!itemToGive.ToString().Contains("Necklace")) return;
        InventoryItem.Spawn(__instance.follower.Brain.Info.Necklace, 1, __instance.follower.transform.position);
        __instance.follower.Brain.Info.Necklace = InventoryItem.ITEM_TYPE.NONE;
        __instance.follower.SetOutfit(FollowerOutfitType.Follower, false);
    }
}