using System.Diagnostics;
using CultOfQoL.Core;

namespace CultOfQoL.Patches.Systems;

[Harmony]
public static class SoundPatches
{
    private const string ChestSoundPrefix = "event:/chests/";

    // Types whose chest sounds we want to conditionally silence
    private static readonly HashSet<Type> TargetTypes =
    [
        typeof(Interaction_CollectResourceChest),
        typeof(LumberjackStation)
    ];

    /// <summary>
    /// Prefix patch for PlayOneShot overloads - conditionally blocks chest sounds
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AudioManager), nameof(AudioManager.PlayOneShot), typeof(string))]
    [HarmonyPatch(typeof(AudioManager), nameof(AudioManager.PlayOneShot), typeof(string), typeof(Vector3))]
    [HarmonyPatch(typeof(AudioManager), nameof(AudioManager.PlayOneShot), typeof(string), typeof(GameObject))]
    public static bool PlayOneShot_Prefix(string soundPath)
    {
        return ShouldPlayChestSound(soundPath);
    }

    /// <summary>
    /// Determines if a chest sound should play based on config settings.
    /// Returns true to play, false to skip.
    /// </summary>
    private static bool ShouldPlayChestSound(string soundPath)
    {
        if (string.IsNullOrEmpty(soundPath) || !soundPath.StartsWith(ChestSoundPrefix))
        {
            return true; // Not a chest sound, play normally
        }

        // Check if call originates from a target type
        var callingType = GetCallingTargetType();
        if (callingType == null)
        {
            return true; // Not from a target type, play normally
        }

        // chest_small_open is the deposit sound (when followers deposit)
        bool shouldPlay;
        if (soundPath.EndsWith("chest_small_open"))
        {
            shouldPlay = ConfigCache.GetCachedValue(ConfigCache.Keys.ResourceChestDepositSounds, () => Plugin.ResourceChestDepositSounds.Value);
        }
        else
        {
            // All other chest sounds are collect sounds
            shouldPlay = ConfigCache.GetCachedValue(ConfigCache.Keys.ResourceChestCollectSounds, () => Plugin.ResourceChestCollectSounds.Value);
        }

        var action = shouldPlay ? "Playing" : "Blocked";
        Plugin.L($"[SoundPatches] {action}: {soundPath} from {callingType.Name}");

        return shouldPlay;
    }

    /// <summary>
    /// Checks the call stack to see if the sound is being played from a target type.
    /// Returns the calling type if found, null otherwise.
    /// </summary>
    private static Type GetCallingTargetType()
    {
        var stackTrace = new StackTrace(false);
        var frameCount = stackTrace.FrameCount;

        // Start at frame 3 to skip: GetCallingTargetType -> ShouldPlayChestSound -> Prefix -> (Harmony internals)
        for (var i = 3; i < frameCount && i < 10; i++)
        {
            var frame = stackTrace.GetFrame(i);
            var declaringType = frame?.GetMethod()?.DeclaringType;

            if (declaringType != null && TargetTypes.Contains(declaringType))
            {
                return declaringType;
            }
        }

        return null;
    }
}
