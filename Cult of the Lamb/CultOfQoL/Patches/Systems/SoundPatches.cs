using CultOfQoL.Core;

namespace CultOfQoL.Patches.Systems;

[Harmony]
public static class SoundPatches
{
    private static readonly MethodInfo PlayOneShotString = AccessTools.Method(typeof(AudioManager), nameof(AudioManager.PlayOneShot), [typeof(string)]);
    private static readonly MethodInfo PlayOneShotStringVector3 = AccessTools.Method(typeof(AudioManager), nameof(AudioManager.PlayOneShot), [typeof(string), typeof(Vector3)]);

    /// <summary>
    /// Transpiler for DepositItem - replaces PlayOneShot with conditional version
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.DepositItem))]
    public static IEnumerable<CodeInstruction> DepositItem_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var replacementMethod = AccessTools.Method(typeof(SoundPatches), nameof(PlayChestDepositSound));
            var replaced = false;

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].Calls(PlayOneShotString))
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, replacementMethod).WithLabels(codes[i].labels);
                    replaced = true;
                    break;
                }
            }

            if (!replaced)
            {
                Plugin.Log.LogWarning("[Transpiler] DepositItem: Failed to find PlayOneShot call.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] DepositItem: Replaced PlayOneShot with conditional version.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] DepositItem: {ex.Message}");
            return original;
        }
    }

    /// <summary>
    /// Transpiler for SpawnItem - replaces PlayOneShot with conditional version
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.SpawnItem))]
    public static IEnumerable<CodeInstruction> SpawnItem_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var replacementMethod = AccessTools.Method(typeof(SoundPatches), nameof(PlayChestCollectSoundWithPosition));
            var replaced = false;

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].Calls(PlayOneShotStringVector3))
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, replacementMethod).WithLabels(codes[i].labels);
                    replaced = true;
                }
            }

            if (!replaced)
            {
                Plugin.Log.LogWarning("[Transpiler] SpawnItem: Failed to find PlayOneShot call.");
                return original;
            }

            Plugin.Log.LogInfo("[Transpiler] SpawnItem: Replaced PlayOneShot with conditional version.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] SpawnItem: {ex.Message}");
            return original;
        }
    }

    /// <summary>
    /// Transpiler for Update - replaces chest-related PlayOneShot calls with conditional versions
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_CollectResourceChest), nameof(Interaction_CollectResourceChest.Update))]
    public static IEnumerable<CodeInstruction> Update_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var original = instructions.ToList();
        try
        {
            var codes = new List<CodeInstruction>(original);
            var replacementMethodPosition = AccessTools.Method(typeof(SoundPatches), nameof(PlayChestCollectSoundWithPosition));
            var replacementMethodNoPosition = AccessTools.Method(typeof(SoundPatches), nameof(PlayChestCollectSound));
            var replacedCount = 0;

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].Calls(PlayOneShotStringVector3))
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, replacementMethodPosition).WithLabels(codes[i].labels);
                    replacedCount++;
                }
                else if (codes[i].Calls(PlayOneShotString))
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, replacementMethodNoPosition).WithLabels(codes[i].labels);
                    replacedCount++;
                }
            }

            if (replacedCount == 0)
            {
                Plugin.Log.LogWarning("[Transpiler] Update: Failed to find any PlayOneShot calls.");
                return original;
            }

            Plugin.Log.LogInfo($"[Transpiler] Update: Replaced {replacedCount} PlayOneShot call(s) with conditional versions.");
            return codes;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] Update: {ex.Message}");
            return original;
        }
    }

    /// <summary>
    /// Conditional PlayOneShot for deposit sounds (no position)
    /// </summary>
    public static void PlayChestDepositSound(string path)
    {
        if (!ConfigCache.GetCachedValue(ConfigCache.Keys.ResourceChestDepositSounds, () => Plugin.ResourceChestDepositSounds.Value))
        {
            return;
        }

        AudioManager.Instance.PlayOneShot(path);
    }

    /// <summary>
    /// Conditional PlayOneShot for collect sounds (no position)
    /// </summary>
    public static void PlayChestCollectSound(string path)
    {
        if (!ConfigCache.GetCachedValue(ConfigCache.Keys.ResourceChestCollectSounds, () => Plugin.ResourceChestCollectSounds.Value))
        {
            return;
        }

        AudioManager.Instance.PlayOneShot(path);
    }

    /// <summary>
    /// Conditional PlayOneShot for collect sounds (with position)
    /// </summary>
    public static void PlayChestCollectSoundWithPosition(string path, Vector3 position)
    {
        if (!ConfigCache.GetCachedValue(ConfigCache.Keys.ResourceChestCollectSounds, () => Plugin.ResourceChestCollectSounds.Value))
        {
            return;
        }

        AudioManager.Instance.PlayOneShot(path, position);
    }
}
