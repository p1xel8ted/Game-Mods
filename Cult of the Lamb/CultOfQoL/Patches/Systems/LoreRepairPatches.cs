namespace CultOfQoL.Patches.Systems;

/// <summary>
/// Auto-repairs missing lore rewards for players affected by a bug where LoreStone interactions
/// were blocked when menus were open. Spawns actual LoreStone objects with full animations.
/// </summary>
[Harmony]
public static class LoreRepairPatches
{
    private static bool _isRepairInProgress;

    /// <summary>
    /// Runs when ChurchJobRewards is enabled (priest job board area).
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ChurchJobRewards), nameof(ChurchJobRewards.OnEnable))]
    public static void ChurchJobRewards_OnEnable(ChurchJobRewards __instance)
    {
        if (!Plugin.AutoRepairMissingLore.Value || _isRepairInProgress)
        {
            return;
        }

        RunRepair(__instance.fleeceToLoreReward, showNotification: false);
    }

    /// <summary>
    /// Called when the config setting is changed. Triggers repair immediately if enabled.
    /// </summary>
    internal static void OnSettingChanged()
    {
        if (!Plugin.AutoRepairMissingLore.Value || _isRepairInProgress)
        {
            return;
        }

        var churchJobRewards = Resources.FindObjectsOfTypeAll<ChurchJobRewards>().ToList();

        if (churchJobRewards.Count == 0)
        {
            Plugin.Log.LogWarning("[LoreRepair] No ChurchJobRewards found - repairing lore room stones only.");
            var loreIdsToRepair = GetMissingLoreRoomStones();

            if (loreIdsToRepair.Count > 0)
            {
                GameManager.GetInstance().StartCoroutine(PlayLoreAnimationsSequentially(loreIdsToRepair));
            }
            else
            {
                ShowNoRepairsNeededNotification();
            }
            return;
        }

        foreach (var cjr in churchJobRewards)
        {
            RunRepair(cjr.fleeceToLoreReward, showNotification: true);
        }
    }

    private static void RunRepair(ChurchJobRewardDictionary fleeceToLore, bool showNotification)
    {
        // Collect all lore IDs that need repair
        var loreIdsToRepair = new List<int>();
        loreIdsToRepair.AddRange(GetMissingLoreRoomStones());
        loreIdsToRepair.AddRange(GetMissingFleeceRewards(fleeceToLore));

        if (loreIdsToRepair.Count == 0)
        {
            if (showNotification)
            {
                ShowNoRepairsNeededNotification();
            }
            return;
        }

        Plugin.Log.LogInfo($"[LoreRepair] Starting animated repair for {loreIdsToRepair.Count} tablet(s).");

        // Start coroutine to play animations sequentially
        GameManager.GetInstance().StartCoroutine(PlayLoreAnimationsSequentially(loreIdsToRepair));
    }

    /// <summary>
    /// Spawns LoreStone objects and plays their animations one after another.
    /// </summary>
    private static IEnumerator PlayLoreAnimationsSequentially(List<int> loreIds)
    {
        _isRepairInProgress = true;

        foreach (var loreId in loreIds)
        {
            // Spawn a LoreStone at the player's position
            var pickup = InventoryItem.Spawn(
                InventoryItem.ITEM_TYPE.LORE_STONE, 1,
                PlayerFarming.Instance.transform.position);

            var loreStone = pickup?.GetComponent<LoreStone>();
            if (loreStone == null)
            {
                Plugin.Log.LogWarning($"[LoreRepair] Failed to spawn LoreStone for lore {loreId}");
                continue;
            }

            // Configure which lore to show (use lamb skin like fleece rewards do)
            loreStone.SetLore(loreId, isLambLore: true);

            // Trigger the full animation sequence via OnInteract (sets required state field)
            loreStone.OnInteract(PlayerFarming.Instance.state);

            Plugin.Log.LogInfo($"[LoreRepair] Playing animation for lore {loreId}");

            // Wait for the animation to complete
            yield return new WaitUntil(() => !loreStone.IsRunning);

            // Small delay between animations for smoother experience
            yield return new WaitForSeconds(0.5f);
        }

        _isRepairInProgress = false;

        // Open the player menu and navigate to the Lore tab so user can see all restored tablets
        yield return new WaitForSeconds(0.5f);
        MonoSingleton<UIManager>.Instance.ShowDetailsMenu(PlayerFarming.Instance);
        UIManager.PlayAudio("event:/Stings/church_bell");
        yield return null;
        Lamb.UI.Menus.PlayerMenu.UIPauseDetailsMenuTabNavigatorBase.Instance.TransitionToLore();
    }

    private static void ShowNoRepairsNeededNotification()
    {
        Plugin.PopupManager.ShowPopupDlg("No missing lore tablets found - your save is already complete!", false);
    }

    /// <summary>
    /// Gets lore IDs from the Lore Room (heart sacrifice stones) that are missing.
    /// </summary>
    private static List<int> GetMissingLoreRoomStones()
    {
        var missing = new List<int>();
        var attemptedCount = Mathf.Min(DataManager.Instance.LoreStonesRoomUpTo, LoreSystem.LoreTotalLoreRoom);

        for (var loreId = 0; loreId < attemptedCount; loreId++)
        {
            if (!DataManager.Instance.LoreUnlocked.Contains(loreId))
            {
                missing.Add(loreId);
            }
        }

        return missing;
    }

    /// <summary>
    /// Gets lore IDs from completed fleece job rewards that are missing.
    /// Completed objectives are stored in CompletedObjectivesHistory as FinalizedData_ShowFleece.
    /// </summary>
    private static List<int> GetMissingFleeceRewards(ChurchJobRewardDictionary fleeceToLore)
    {
        var missing = new List<int>();
        var processedLoreIds = new HashSet<int>();

        var completedFleeceObjectives = DataManager.Instance.CompletedObjectivesHistory
            .OfType<Objectives_ShowFleece.FinalizedData_ShowFleece>();

        foreach (var objective in completedFleeceObjectives)
        {
            if (!fleeceToLore.TryGetValue(objective.FleeceType, out var loreId))
            {
                continue;
            }

            // Skip if we already processed this lore ID (avoid duplicates)
            if (!processedLoreIds.Add(loreId))
            {
                continue;
            }

            if (DataManager.Instance.LoreUnlocked.Contains(loreId))
            {
                continue;
            }

            missing.Add(loreId);
        }

        return missing;
    }
}
