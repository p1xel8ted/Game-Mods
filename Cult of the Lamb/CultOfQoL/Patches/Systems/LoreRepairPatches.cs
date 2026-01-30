using Lamb.UI.Menus.PlayerMenu;

namespace CultOfQoL.Patches.Systems;

/// <summary>
/// Auto-repairs missing lore rewards for players affected by a bug where LoreStone interactions
/// were blocked when menus were open. Spawns actual LoreStone objects with full animations.
/// Only repairs tablets for fleece quests where the reward was actually claimed at the job board.
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
        Plugin.Log.LogInfo("[LoreRepair] ChurchJobRewards_OnEnable triggered");
        Plugin.Log.LogInfo($"[LoreRepair]   Config enabled: {Plugin.AutoRepairMissingLore.Value}");
        Plugin.Log.LogInfo($"[LoreRepair]   Repair in progress: {_isRepairInProgress}");

        if (!Plugin.AutoRepairMissingLore.Value || _isRepairInProgress)
        {
            Plugin.Log.LogInfo("[LoreRepair]   -> Skipping repair");
            return;
        }

        Plugin.Log.LogInfo("[LoreRepair]   -> Starting repair");
        RunRepair(__instance.fleeceToLoreReward, showNotification: false);
    }

    /// <summary>
    /// Called when the config setting is changed. Triggers repair immediately if enabled.
    /// </summary>
    internal static void OnSettingChanged()
    {
        Plugin.Log.LogInfo($"[LoreRepair] OnSettingChanged: Value={Plugin.AutoRepairMissingLore.Value}, InProgress={_isRepairInProgress}");

        if (!Plugin.AutoRepairMissingLore.Value || _isRepairInProgress)
        {
            Plugin.Log.LogInfo("[LoreRepair]   -> Skipping (disabled or in progress)");
            return;
        }

        var churchJobRewards = Resources.FindObjectsOfTypeAll<ChurchJobRewards>().ToList();
        Plugin.Log.LogInfo($"[LoreRepair] ChurchJobRewards found: {churchJobRewards.Count}");

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
        Plugin.Log.LogInfo($"[LoreRepair] RunRepair called, showNotification={showNotification}");

        // Collect all lore IDs that need repair
        var loreIdsToRepair = new List<int>();
        loreIdsToRepair.AddRange(GetMissingLoreRoomStones());
        loreIdsToRepair.AddRange(GetMissingFleeceRewards(fleeceToLore));

        Plugin.Log.LogInfo($"[LoreRepair] Total lore to repair: {loreIdsToRepair.Count}");
        Plugin.Log.LogInfo($"[LoreRepair] Lore IDs: [{string.Join(", ", loreIdsToRepair)}]");

        if (loreIdsToRepair.Count == 0)
        {
            Plugin.Log.LogInfo("[LoreRepair] No repairs needed");
            if (showNotification)
            {
                ShowNoRepairsNeededNotification();
            }
            return;
        }

        Plugin.Log.LogInfo($"[LoreRepair] Starting animated repair for {loreIdsToRepair.Count} tablet(s)");

        // Start coroutine to play animations sequentially
        GameManager.GetInstance().StartCoroutine(PlayLoreAnimationsSequentially(loreIdsToRepair));
    }

    /// <summary>
    /// Spawns LoreStone objects and plays their animations one after another.
    /// </summary>
    private static IEnumerator PlayLoreAnimationsSequentially(List<int> loreIds)
    {
        _isRepairInProgress = true;
        Plugin.Log.LogInfo($"[LoreRepair] PlayLoreAnimationsSequentially started with {loreIds.Count} lore IDs");

        foreach (var loreId in loreIds)
        {
            Plugin.Log.LogInfo($"[LoreRepair] Spawning LoreStone for lore {loreId}");

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
        Plugin.Log.LogInfo("[LoreRepair] All animations complete");

        // Open the player menu and navigate to the Lore tab so user can see all restored tablets
        yield return new WaitForSeconds(0.5f);
        MonoSingleton<UIManager>.Instance.ShowDetailsMenu(PlayerFarming.Instance);
        UIManager.PlayAudio("event:/Stings/church_bell");
        yield return null;
        UIPauseDetailsMenuTabNavigatorBase.Instance.TransitionToLore();
    }

    private static void ShowNoRepairsNeededNotification()
    {
        Plugin.Log.LogInfo("[LoreRepair] Showing 'no repairs needed' notification");
        Plugin.PopupManager.ShowPopupDlg("No missing lore tablets found - your save is already complete!", false);
    }

    /// <summary>
    /// Gets lore IDs from the Lore Room (heart sacrifice stones) that are missing.
    /// </summary>
    private static List<int> GetMissingLoreRoomStones()
    {
        var missing = new List<int>();
        var attemptedCount = Mathf.Min(DataManager.Instance.LoreStonesRoomUpTo, LoreSystem.LoreTotalLoreRoom);

        Plugin.Log.LogInfo("[LoreRepair] GetMissingLoreRoomStones:");
        Plugin.Log.LogInfo($"[LoreRepair]   LoreStonesRoomUpTo: {DataManager.Instance.LoreStonesRoomUpTo}");
        Plugin.Log.LogInfo($"[LoreRepair]   LoreTotalLoreRoom: {LoreSystem.LoreTotalLoreRoom}");
        Plugin.Log.LogInfo($"[LoreRepair]   Checking lore IDs 0 to {attemptedCount - 1}");

        for (var loreId = 0; loreId < attemptedCount; loreId++)
        {
            var isUnlocked = DataManager.Instance.LoreUnlocked.Contains(loreId);
            Plugin.Log.LogInfo($"[LoreRepair]   Lore {loreId}: unlocked={isUnlocked}");
            if (!isUnlocked)
            {
                missing.Add(loreId);
            }
        }

        Plugin.Log.LogInfo($"[LoreRepair]   Missing lore room stones: [{string.Join(", ", missing)}]");
        return missing;
    }

    /// <summary>
    /// Gets lore IDs from fleece job rewards where the reward was CLAIMED but lore wasn't received.
    /// Only repairs tablets for quests that were actually claimed at the job board.
    /// </summary>
    private static List<int> GetMissingFleeceRewards(ChurchJobRewardDictionary fleeceToLore)
    {
        Plugin.Log.LogInfo("[LoreRepair] GetMissingFleeceRewards called");

        var missing = new List<int>();
        var processedLoreIds = new HashSet<int>();

        // Log all claimed job board quests
        Plugin.Log.LogInfo($"[LoreRepair] JobBoardsClaimedQuests count: {DataManager.Instance.JobBoardsClaimedQuests.Count}");
        foreach (var claimedId in DataManager.Instance.JobBoardsClaimedQuests)
        {
            Plugin.Log.LogInfo($"[LoreRepair]   Claimed quest ID: {claimedId}");
        }

        // Get claimed Priest job board quest indices (Priest host = 400)
        var claimedPriestIndices = DataManager.Instance.JobBoardsClaimedQuests
            .Where(id => id >= 400 && id < 500)
            .Select(id => id - 400)
            .ToHashSet();

        Plugin.Log.LogInfo($"[LoreRepair] Priest job board claimed indices: [{string.Join(", ", claimedPriestIndices)}]");

        // Log all unlocked lore
        Plugin.Log.LogInfo($"[LoreRepair] LoreUnlocked count: {DataManager.Instance.LoreUnlocked.Count}");
        foreach (var loreId in DataManager.Instance.LoreUnlocked)
        {
            Plugin.Log.LogInfo($"[LoreRepair]   Unlocked lore ID: {loreId}");
        }

        // Log fleece-to-lore mapping
        Plugin.Log.LogInfo("[LoreRepair] FleeceToLore mapping:");
        foreach (var kvp in fleeceToLore)
        {
            Plugin.Log.LogInfo($"[LoreRepair]   {kvp.Key} -> lore {kvp.Value}");
        }

        // Build fleece-to-index map from the Priest job board
        var fleeceToIndex = BuildFleeceToIndexMap();
        Plugin.Log.LogInfo("[LoreRepair] FleeceToIndex mapping:");
        foreach (var kvp in fleeceToIndex)
        {
            Plugin.Log.LogInfo($"[LoreRepair]   {kvp.Key} -> index {kvp.Value}");
        }

        // For each fleece in the reward dictionary, check if:
        // 1. The quest was claimed at the job board
        // 2. The lore is NOT unlocked
        foreach (var kvp in fleeceToLore)
        {
            var fleeceType = kvp.Key;
            var loreId = kvp.Value;

            // Skip if we already processed this lore ID (avoid duplicates)
            if (!processedLoreIds.Add(loreId))
            {
                Plugin.Log.LogInfo($"[LoreRepair] Fleece {fleeceType}: lore {loreId} - skipping (already processed)");
                continue;
            }

            var isLoreUnlocked = DataManager.Instance.LoreUnlocked.Contains(loreId);
            Plugin.Log.LogInfo($"[LoreRepair] Checking fleece {fleeceType}: lore {loreId}, unlocked={isLoreUnlocked}");

            // Skip if lore already unlocked
            if (isLoreUnlocked)
            {
                Plugin.Log.LogInfo($"[LoreRepair]   -> Skipping, lore already unlocked");
                continue;
            }

            // Check if this fleece's quest was claimed at the job board
            if (fleeceToIndex.TryGetValue(fleeceType, out var questIndex))
            {
                var isClaimed = claimedPriestIndices.Contains(questIndex);
                Plugin.Log.LogInfo($"[LoreRepair]   -> Quest index {questIndex}, claimed={isClaimed}");

                if (isClaimed)
                {
                    Plugin.Log.LogInfo($"[LoreRepair]   -> ADDING lore {loreId} to missing list (reward claimed but lore missing)");
                    missing.Add(loreId);
                }
                else
                {
                    Plugin.Log.LogInfo($"[LoreRepair]   -> Skipping, quest reward not claimed yet at job board");
                }
            }
            else
            {
                Plugin.Log.LogInfo($"[LoreRepair]   -> Fleece {fleeceType} not found in job board objectives");
            }
        }

        Plugin.Log.LogInfo($"[LoreRepair] Missing fleece lore IDs: [{string.Join(", ", missing)}]");
        return missing;
    }

    /// <summary>
    /// Builds a map from FleeceType to the quest index on the Priest job board.
    /// </summary>
    private static Dictionary<PlayerFleeceManager.FleeceType, int> BuildFleeceToIndexMap()
    {
        var map = new Dictionary<PlayerFleeceManager.FleeceType, int>();

        // Find the Priest job board
        var priestBoards = Resources.FindObjectsOfTypeAll<Interaction_JobBoard>()
            .Where(jb => jb.Host == Interaction_JobBoard.HostEnum.Priest)
            .ToList();

        Plugin.Log.LogInfo($"[LoreRepair] BuildFleeceToIndexMap: Found {priestBoards.Count} Priest job board(s)");

        if (priestBoards.Count == 0)
        {
            Plugin.Log.LogWarning("[LoreRepair] No Priest job board found - cannot verify fleece quest claims");
            return map;
        }

        // Use the first one (there should only be one)
        var board = priestBoards[0];

        for (var i = 0; i < board.Objectives.Count; i++)
        {
            var objective = board.Objectives[i];
            var fleeceType = objective.Objective.Fleece;

            // Only add if it's a fleece objective (not None)
            if (fleeceType != PlayerFleeceManager.FleeceType.Default)
            {
                map[fleeceType] = i;
                Plugin.Log.LogInfo($"[LoreRepair] BuildFleeceToIndexMap: index {i} = {fleeceType}");
            }
        }

        return map;
    }
}
