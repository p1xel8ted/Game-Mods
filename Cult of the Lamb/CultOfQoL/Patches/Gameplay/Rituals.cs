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


    public static int GetBossLimit()
    {
        Plugin.Log.LogWarning($"[DEBUG] GetBossLimit called: returning {Plugin.SinBossLimit.Value}");
        return Plugin.SinBossLimit.Value;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.TryOnboardSin))]
    public static void Interaction_TempleAltar_TryOnboardSin_Prefix()
    {
        if (Plugin.SinBossLimit.Value == 3) return;
        
        var bossCount = DataManager.Instance.BossesCompleted.Count;
        var bossLimit = Rituals.GetBossLimit();
        var pleasureRevealed = DataManager.Instance.PleasureRevealed;
        var followerCount = Ritual.FollowerToAttendSermon.Count;
        var firePitUnlocked = UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit);

        var notEnoughBosses = bossCount < bossLimit;
        var noFollowers = followerCount <= 0;
        var firePitNotUnlocked = !firePitUnlocked;

        Plugin.Log.LogInfo(
            $"[Sin Unlock] OnboardSin Check:\n" +
            $"  - BossesCompleted.Count = {bossCount} (needs >= {bossLimit})   => {(notEnoughBosses ? "FAIL (Not enough bosses)" : "OK")}\n" +
            $"  - PleasureRevealed = {pleasureRevealed}                      => {(pleasureRevealed ? "FAIL (Already revealed)" : "OK")}\n" +
            $"  - FollowerToAttendSermon.Count = {followerCount} (needs > 0)  => {(noFollowers ? "FAIL (No followers to attend)" : "OK")}\n" +
            $"  - Ritual_FirePit Unlocked = {firePitUnlocked}                 => {(firePitNotUnlocked ? "FAIL (Fire Pit not unlocked)" : "OK")}\n"
        );

        if (notEnoughBosses || pleasureRevealed || noFollowers || firePitNotUnlocked)
        {
            Plugin.Log.LogWarning("[Sin Unlock] => Skipping onboarding ritual (one or more requirements not met)");
        }
        else
        {
            Plugin.Log.LogInfo("[Sin Unlock] => Proceeding to onboarding ritual (all requirements met)");
        }
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.TryOnboardSin))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.TryOnboardSin), MethodType.Enumerator)]
    public static IEnumerable<CodeInstruction> Interaction_TempleAltar_TryOnboardSin_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var originalCode = instructions.ToList();
        var modifiedCode = new List<CodeInstruction>(originalCode);

        try
        {
            var dmInstanceGetter = AccessTools.PropertyGetter(typeof(DataManager), nameof(DataManager.Instance));
            var bossesCompletedField = AccessTools.Field(typeof(DataManager), nameof(DataManager.BossesCompleted));
            var listCountGetter = AccessTools.PropertyGetter(typeof(List<FollowerLocation>), "Count");
            var getBossLimitMethod = AccessTools.Method(typeof(Rituals), nameof(GetBossLimit));

            for (var i = 3; i < modifiedCode.Count; i++)
            {
                var matches =
                    modifiedCode[i - 3].Calls(dmInstanceGetter) &&
                    modifiedCode[i - 2].LoadsField(bossesCompletedField) &&
                    modifiedCode[i - 1].Calls(listCountGetter) &&
                    modifiedCode[i].opcode == OpCodes.Ldc_I4_3;

                if (matches)
                {
                    modifiedCode[i] = new CodeInstruction(OpCodes.Call, getBossLimitMethod).WithLabels(modifiedCode[i].labels);

                    Plugin.Log.LogInfo("[Transpiler] Replaced BossesCompleted.Count < 3 check with GetBossLimit() in Interaction_TempleAltar.TryOnboardSin()");
                    break;
                }
            }

            return modifiedCode;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[Transpiler] Error in TryOnboardSin transpiler: {ex}");
            return originalCode;
        }
    }
}