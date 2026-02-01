namespace Rebirth;

[HarmonyPatch]
[HarmonyWrapSafe]
public static class Patches
{
    // Track followers currently undergoing rebirth
    private static readonly HashSet<int> RebirthingFollowers = new();

    // Localized death messages for rebirth (with rich text formatting to match vanilla)
    private static readonly Dictionary<string, string> RebirthDeathMessages = new()
    {
        { "English", "<color=#FFD201>{0}</color> succumbed to unknown forces..." },
        { "French", "<color=#FFD201>{0}</color> a succombé à des forces inconnues..." },
        { "German", "<color=#FFD201>{0}</color> erlag unbekannten Mächten..." },
        { "Spanish", "<color=#FFD201>{0}</color> sucumbió a fuerzas desconocidas..." },
        { "Italian", "<color=#FFD201>{0}</color> è soccombuto a forze sconosciute..." },
        { "Portuguese(Brazil)", "<color=#FFD201>{0}</color> sucumbiu a forças desconhecidas..." },
        { "Russian", "<color=#FFD201>{0}</color> пал жертвой неведомых сил..." },
        { "Japanese", "<color=#FFD201>{0}</color>は未知の力に屈した..." },
        { "Korean", "<color=#FFD201>{0}</color>이(가) 알 수 없는 힘에 굴복했습니다..." },
        { "SimplifiedChinese", "<color=#FFD201>{0}</color>屈服于未知的力量..." },
        { "TraditionalChinese", "<color=#FFD201>{0}</color>屈服於未知的力量..." }
    };

    public static void MarkFollowerForRebirth(int followerId)
    {
        RebirthingFollowers.Add(followerId);
        Plugin.Log.LogInfo($"[Rebirth] Marked follower ID {followerId} for rebirth. Total marked: {RebirthingFollowers.Count}");
    }

    public static void UnmarkFollowerRebirth(int followerId)
    {
        var removed = RebirthingFollowers.Remove(followerId);
        Plugin.Log.LogInfo($"[Rebirth] Unmarked follower ID {followerId} (removed: {removed}). Remaining marked: {RebirthingFollowers.Count}");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInfo), nameof(FollowerInfo.GetDeathText))]
    public static void FollowerInfo_GetDeathText_Postfix(FollowerInfo __instance, ref string __result)
    {
        Plugin.Log.LogInfo($"[Rebirth] GetDeathText called for follower '{__instance.Name}' (ID: {__instance.ID})");
        Plugin.Log.LogInfo($"[Rebirth] Currently marked follower IDs: [{string.Join(", ", RebirthingFollowers)}]");
        Plugin.Log.LogInfo($"[Rebirth] Is follower in rebirth set? {RebirthingFollowers.Contains(__instance.ID)}");
        Plugin.Log.LogInfo($"[Rebirth] Original death text: {__result}");

        if (RebirthingFollowers.Contains(__instance.ID))
        {
            var currentLanguage = LocalizationManager.CurrentLanguage;
            Plugin.Log.LogInfo($"[Rebirth] Current language: {currentLanguage}");

            // Get localized message, fall back to English if language not found
            var messageTemplate = RebirthDeathMessages.TryGetValue(currentLanguage, out var msg)
                ? msg
                : RebirthDeathMessages["English"];

            Plugin.Log.LogInfo($"[Rebirth] Using message template: {messageTemplate}");

            var oldResult = __result;
            __result = string.Format(messageTemplate, __instance.Name);

            Plugin.Log.LogInfo($"[Rebirth] Changed death text from '{oldResult}' to '{__result}'");

            // Unmark the follower now that we've used it (GetDeathText is called during the death animation)
            UnmarkFollowerRebirth(__instance.ID);
        }
        else
        {
            Plugin.Log.LogInfo($"[Rebirth] Follower not in rebirth set - using vanilla death text");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DropLootOnDeath), nameof(DropLootOnDeath.OnDie))]
    public static void DropLootOnDeath_OnDie(DropLootOnDeath __instance, Health Victim)
    {
        if (Victim.team == Health.Team.Team2)
        {
            if (Random.Range(0, 101) <= Plugin.EnemyDropRate.Value)
            {
                Plugin.Log.LogInfo($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(Plugin.DropMinQuantity.Value, Plugin.DropMaxQuantity.Value + 1), __instance.transform.position);
            }
        }
        else if (Victim.name.ToLower(CultureInfo.InvariantCulture).Contains("breakable body pile"))
        {
            if (Random.Range(0, 101) <= Plugin.EnemyDropRate.Value)
            {
                Plugin.Log.LogInfo($"Got a Rebirth token from {__instance.name}!");
                InventoryItem.Spawn(Plugin.RebirthItem, Random.Range(Plugin.DropMinQuantity.Value, Plugin.DropMaxQuantity.Value + 1), __instance.transform.position);
            }
        }
    }
}