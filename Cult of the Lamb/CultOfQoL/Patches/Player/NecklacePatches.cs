namespace CultOfQoL.Patches.Player;

[Harmony]
public static class NecklacePatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.GiveItemRoutine))]
    public static void interaction_FollowerInteraction_GiveItemRoutine(ref interaction_FollowerInteraction __instance, ref InventoryItem.ITEM_TYPE itemToGive)
    {
        if (!Plugin.GiveFollowersNewNecklaces.Value) return;
        if (!IsNecklace(itemToGive)) return;
        
        var followerBrain = __instance.follower.Brain;
        var currentNecklace = followerBrain.Info.Necklace;
        
        // Only drop the old necklace if they have one
        if (!IsNecklace(currentNecklace)) return;
        
        InventoryItem.Spawn(currentNecklace, 1, __instance.follower.transform.position);
        followerBrain.Info.Necklace = InventoryItem.ITEM_TYPE.NONE;
        __instance.follower.SetOutfit(FollowerOutfitType.Follower, false);
    }
    
    private static bool IsNecklace(InventoryItem.ITEM_TYPE itemType)
    {
        // Future-proof: catches any enum value with "Necklace" in the name
        return itemType != InventoryItem.ITEM_TYPE.NONE && 
               itemType.ToString().Contains("Necklace", StringComparison.OrdinalIgnoreCase);
    }
}