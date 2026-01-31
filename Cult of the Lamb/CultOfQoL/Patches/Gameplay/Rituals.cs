namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class Rituals
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.AddCooldown))]
    public static void UpgradeSystem_AddCooldown_Prefix(UpgradeSystem.Type type, ref float duration)
    {
        var multiplier = Plugin.RitualCooldownTime.Value;
        var originalDuration = duration;
        var scaledDuration = originalDuration * multiplier;

        Plugin.L($"[UpgradeSystem_AddCooldown] Type: {type} | Original: {originalDuration:F2}s | Multiplier: {multiplier:F2} | New: {scaledDuration:F2}s");

        duration = scaledDuration;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UpgradeSystem), nameof(UpgradeSystem.GetCost))]
    public static void UpgradeSystem_GetCost_Postfix(UpgradeSystem.Type Type, ref List<StructuresData.ItemCost> __result)
    {
        var multiplier = Plugin.RitualCostMultiplier.Value;
        if (Math.Abs(multiplier - 1.0f) < 0.01f) return;

        foreach (var cost in __result)
        {
            if (cost.CostItem is InventoryItem.ITEM_TYPE.FOLLOWERS
                or InventoryItem.ITEM_TYPE.DOCTRINE_STONE
                or InventoryItem.ITEM_TYPE.DISCIPLE_POINTS)
            {
                continue;
            }

            var original = cost.CostValue;
            cost.CostValue = Mathf.Max(1, Mathf.RoundToInt(original * multiplier));
            Plugin.L($"[RitualCost] {Type}: {cost.CostItem} {original} -> {cost.CostValue} (x{multiplier:F2})");
        }
    }


    public static int GetBossLimit()
    {
        return Plugin.SinBossLimit.Value;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.TryOnboardSin))]
    public static void Interaction_TempleAltar_TryOnboardSin_Prefix()
    {
        if (Plugin.SinBossLimit.Value == 3) return;
        
        var bossCount = DataManager.Instance.BossesCompleted.Count;
        var bossLimit = GetBossLimit();
        var pleasureRevealed = DataManager.Instance.PleasureRevealed;
        var followerCount = Ritual.FollowerToAttendSermon.Count;
        var firePitUnlocked = UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit);

        var notEnoughBosses = bossCount < bossLimit;
        var noFollowers = followerCount <= 0;
        var firePitNotUnlocked = !firePitUnlocked;

        Plugin.L(
            $"[Sin Unlock] OnboardSin Check:\n" +
            $"  - BossesCompleted.Count = {bossCount} (needs >= {bossLimit})   => {(notEnoughBosses ? "FAIL (Not enough bosses)" : "OK")}\n" +
            $"  - PleasureRevealed = {pleasureRevealed}                      => {(pleasureRevealed ? "FAIL (Already revealed)" : "OK")}\n" +
            $"  - FollowerToAttendSermon.Count = {followerCount} (needs > 0)  => {(noFollowers ? "FAIL (No followers to attend)" : "OK")}\n" +
            $"  - Ritual_FirePit Unlocked = {firePitUnlocked}                 => {(firePitNotUnlocked ? "FAIL (Fire Pit not unlocked)" : "OK")}\n"
        );

        if (notEnoughBosses || pleasureRevealed || noFollowers || firePitNotUnlocked)
        {
            Plugin.L("[Sin Unlock] => Skipping onboarding ritual (one or more requirements not met)");
        }
        else
        {
            Plugin.L("[Sin Unlock] => Proceeding to onboarding ritual (all requirements met)");
        }
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.TryOnboardSin), MethodType.Enumerator)]
    public static IEnumerable<CodeInstruction> Interaction_TempleAltar_TryOnboardSin_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var dmInstanceGetter = AccessTools.PropertyGetter(typeof(DataManager), nameof(DataManager.Instance));
            var bossesCompletedField = AccessTools.Field(typeof(DataManager), nameof(DataManager.BossesCompleted));
            var listCountGetter = AccessTools.PropertyGetter(typeof(List<FollowerLocation>), "Count");
            var getBossLimitMethod = AccessTools.Method(typeof(Rituals), nameof(GetBossLimit));
            var found = false;

            for (var i = 3; i < codes.Count; i++)
            {
                if (codes[i - 3].Calls(dmInstanceGetter) &&
                    codes[i - 2].LoadsField(bossesCompletedField) &&
                    codes[i - 1].Calls(listCountGetter) &&
                    codes[i].opcode == OpCodes.Ldc_I4_3)
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, getBossLimitMethod).WithLabels(codes[i].labels);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Plugin.Log.LogWarning("[Transpiler] Interaction_TempleAltar.TryOnboardSin: Failed to find BossesCompleted.Count < 3 check.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] Interaction_TempleAltar.TryOnboardSin: Replaced boss limit with GetBossLimit().");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Interaction_TempleAltar.TryOnboardSin: {ex.Message}");
            return original;
        }
    }
}